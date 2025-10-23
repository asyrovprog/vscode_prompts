# C# TPL Dataflow Library – 15‑Minute Top‑Down Guide

## 1. Intuition First

Think of Dataflow as Lego blocks for concurrent, asynchronous pipelines inside your process. Each block:

- Receives messages (inputs)
- Processes them independently (can be parallel)
- Emits results to the next block(s)

You compose blocks into a directed graph (usually linear or branched). The library quietly handles queuing, scheduling, throttling, backpressure, and completion propagation, so you focus on transformation logic.

Visual Mental Model:

```text
            BufferBlock<string>
                    │
           TransformBlock<string, Uri>
                    │
        TransformBlock<Uri, string>
            (download HTML)
                    │
      ActionBlock<string>(index + store)
```

Messages flow top → bottom. Each block can have an internal degree of parallelism.

## 2. Core Concepts (Bird’s Eye)

| Concept | Purpose | Key Options |
|---------|---------|-------------|
| Dataflow Block | Unit of work / queue | ExecutionDataflowBlockOptions |
| Source / Target | A block can expose `ISourceBlock<TOutput>` and/or `ITargetBlock<TInput>` | Link via `LinkTo` |
| Link | Connects source to target | `propagateCompletion` flag |
| Message Passing | Post / SendAsync | Backpressure via bounded capacity |
| Completion | Signals “no more data” & flush | `Complete()`, `Completion` Task |
| Faults | Exceptions stored, propagate | `Fault(Exception)` |

## 3. Main Block Types (Use Cases)

1. BufferBlock&lt;T&gt;
   - Simple FIFO queue. No processing – just holds items.
   - Use: decouple producers/consumers.
2. BroadcastBlock&lt;T&gt;
   - Clones & broadcasts input to multiple targets.
3. TransformBlock&lt;TIn,TOut&gt;
   - Applies a function returning TOut. Supports parallelism.
4. TransformManyBlock&lt;TIn,TOut&gt;
   - One-to-many expansion (like SelectMany / flatMap).
5. ActionBlock&lt;T&gt;
   - Performs an action (no output) – terminal sink.
6. BatchBlock&lt;T&gt;
   - Aggregates inputs into batches of fixed size.
7. JoinBlock&lt;T1,T2,…&gt;
   - Waits for one of each input type – tuple output. Use Grouping.
8. WriteOnceBlock&lt;T&gt;
   - Captures a single value then repeats it for future consumers.
9. Custom IPropagatorBlock
   - Build your own when existing blocks don’t fit.

## 4. Execution Options Cheat Sheet

`ExecutionDataflowBlockOptions` (pass into constructor):

- `MaxDegreeOfParallelism` – concurrency level (default 1, set to `DataflowBlockOptions.Unbounded` for unlimited).
- `BoundedCapacity` – max queued messages; enforces backpressure (producer waits on `SendAsync`).
- `EnsureOrdered` – whether outputs preserve input order when parallel.
- `TaskScheduler` – control where tasks run (e.g., UI thread).
- `CancellationToken` – cancel pipeline.

## 5. Posting vs Sending

| Method | Returns | Behavior on full | Recommended |
|--------|---------|------------------|-------------|
| Post(item) | bool | Drops (false) if cannot accept immediately | Fire-and-forget scenarios |
| SendAsync(item) | Task&lt;bool&gt; | Awaits space (respects BoundedCapacity) | Robust pipelines (backpressure) |

## 6. Completion Lifecycle

1. Build blocks.
2. Link them: `source.LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true });`
3. Feed inputs via Post/SendAsync.
4. Signal end: call `Complete()` on first block.
5. Await tail block `await actionBlock.Completion;`

PropagateCompletion = true automatically calls `Complete()` downstream when upstream completes successfully or faults.

## 7. Error Handling & Fault Propagation
If your delegate throws in a TransformBlock:
- Block transitions to a faulted state.
- Downstream blocks (with propagateCompletion) also fault.
Pattern:
```csharp
try { /* work */ }
catch(Exception ex) { /* log; maybe rethrow */ }
```
For controlled faults: `((IDataflowBlock)block).Fault(ex);`
Always inspect `block.Completion` task (await / observe Exception).

## 8. Backpressure & Flow Control
BoundedCapacity makes producers slow down naturally (`SendAsync` waits). Without it a fast producer can spike memory. Set capacity per stage to tune throughput vs memory.
Batching + BoundedCapacity + Parallel TransformBlocks are the trifecta for high-throughput ETL-style pipelines.

## 9. Example: Simple Web Scrape Pipeline
```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

class PipelineDemo
{
    public static async Task RunAsync(string[] urls)
    {
        var client = new HttpClient();

        var download = new TransformBlock<string, (string url, string html)>(
            async url => (url, await client.GetStringAsync(url)),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4, BoundedCapacity = 8 });

        var extractTitle = new TransformBlock<(string url, string html), (string url, string title)>(
            page => (page.url, TitleFromHtml(page.html)),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4, EnsureOrdered = false });

        var print = new ActionBlock<(string url, string title)>(
            result => Console.WriteLine($"{result.url} -> {result.title}"));

        download.LinkTo(extractTitle, new DataflowLinkOptions { PropagateCompletion = true });
        extractTitle.LinkTo(print, new DataflowLinkOptions { PropagateCompletion = true });

        foreach (var u in urls) await download.SendAsync(u);
        download.Complete();
        await print.Completion;
    }

    static string TitleFromHtml(string html)
    {
        var start = html.IndexOf("<title>", StringComparison.OrdinalIgnoreCase);
        if (start < 0) return "(no title)";
        start += 7;
        var end = html.IndexOf("</title>", start, StringComparison.OrdinalIgnoreCase);
        if (end < 0) return "(no title)";
        return html.Substring(start, end - start).Trim();
    }
}
```
Key Points:
- Parallel download & title extraction.
- Backpressure via BoundedCapacity (8 inflight URL fetches max).
- Non-ordered title extraction for better throughput.

## 10. When to Use / Avoid
Use Dataflow When:
- You have multi-stage transformations.
- Need in-process streaming, throttling, backpressure.
- Want to tune parallelism per stage.
Avoid / Consider Other Tools When:
- Cross-process distribution (look at messaging systems, gRPC, Azure Service Bus).
- Very simple one-off asynchronous calls (just use `Task.Run` / `async` directly).
- High-level reactive UI flows (consider Reactive Extensions).

## 11. Performance Tips
- Coarse-grained tasks outperform excessively fine-grained ones.
- Start low on `MaxDegreeOfParallelism`; measure & increase.
- Use `BoundedCapacity` (memory safety + natural throughput shaping).
- Consolidate small synchronous operations into a single Transform block to reduce queue hops.
- Prefer `SendAsync` under pressure.

## 12. Testing Pipelines
Isolate logic (e.g., `TitleFromHtml`) for unit tests; integration-test flow by feeding known messages and awaiting completion. Use cancellation tokens for timeouts.

## 13. Common Pitfalls & Fixes
| Pitfall | Symptom | Fix |
|---------|---------|-----|
| Forget `Complete()` | Pipeline never ends | Call `Complete()` then await tail block |
| Unbounded queues | High memory usage | Set `BoundedCapacity` |
| Swallowing exceptions | Silent failure | Await `Completion` and log `Exception` |
| Too much parallelism | Diminishing returns / thrash | Profile, dial back degrees |
| Missing propagateCompletion | Downstream waits forever | Use `PropagateCompletion = true` |

## 14. Mini Challenge (Self‑Check)
Add a `TransformManyBlock<string,string>` after download that emits all hyperlinks from each page before extracting titles of those linked pages. Ensure you don’t reprocess duplicates.

## 15. Quick Recap
Blocks as composable concurrency units.
Link with backpressure and completion propagation.
Tune parallelism & capacity for flow control.
Await final `Completion` for lifecycle integrity.

## 16. Next Step Suggestion
Take the mini challenge or proceed to a quiz to reinforce concepts.

Happy flowing! 🚀
