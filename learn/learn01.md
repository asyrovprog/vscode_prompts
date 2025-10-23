# C# TPL Dataflow Library (20‑Minute Top‑Down Primer)

## 1. Why Dataflow?
Modern apps juggle streams of work: downloading, parsing, transforming, saving. Threading primitives (locks, queues, manual Tasks) get verbose and error‑prone. TPL Dataflow offers Lego‑style building blocks you compose into a pipeline: messages flow through blocks; blocks apply concurrency, ordering, buffering, throttling, backpressure, and fault propagation for you.

Think: Functional pipeline + actor mailboxes + async Tasks = Dataflow network.

Visual mental model:
```
Producer ---> [ BufferBlock ] ---> [ TransformBlock ] ---> [ ActionBlock ] ---> Consumer side-effects
                 |                      |                     |
            (Queue)               (Parallel map)         (Execute + completion)
```

## 2. Core Concepts
Term              | Meaning | Analogy
------------------|---------|--------
Message           | Single data item flowing | Parcel
Block             | Unit that sends/receives messages | Processing station
Link              | Connection between output and input | Conveyor belt
Completion        | Graceful shutdown signal | "No more parcels"
Backpressure      | Block stops accepting when full | Station says "Hold on"

Blocks implement at least one interface:
- `ISourceBlock<TOutput>` (can send).
- `ITargetBlock<TInput>` (can receive).
- Most are both through `IPropagatorBlock<TInput,TOutput>`.

## 3. Essential Block Types (Start Here)
1. `BufferBlock<T>`: In-memory FIFO queue. Simple inbox.
2. `TransformBlock<TIn,TOut>`: Async map (can be parallel). Each input produces one output.
3. `ActionBlock<T>`: Terminal side-effect (store, log, call API). No outputs.
4. `BroadcastBlock<T>`: Clones latest value to many targets.
5. `BatchBlock<T>`: Groups items into arrays of a fixed size.
6. `TransformManyBlock<TIn,TOut>`: FlatMap: each input emits 0..N outputs.
7. `JoinBlock<T1,T2>` / `BatchedJoinBlock`: Synchronize tuples.

Minimal pipeline:
```csharp
var buffer = new BufferBlock<string>();
var toUpper = new TransformBlock<string,string>(s => s.ToUpperInvariant());
var printer = new ActionBlock<string>(Console.WriteLine);
buffer.LinkTo(toUpper, new DataflowLinkOptions { PropagateCompletion = true });
toUpper.LinkTo(printer, new DataflowLinkOptions { PropagateCompletion = true });
foreach (var word in new[]{"alpha","beta"}) buffer.Post(word);
buffer.Complete();
await printer.Completion; // Ensures flush
```

## 4. Parallelism & Throughput
`ExecutionDataflowBlockOptions` controls behavior:
- `MaxDegreeOfParallelism`: transform concurrency (set >1 for CPU-bound, `DataflowBlockOptions.Unbounded` for full tilt).
- `BoundedCapacity`: limits queue length creating natural backpressure.
- `EnsureOrdered`: if false, outputs can arrive out of input order, increasing throughput.
- `SingleProducerConstrained`: micro-optimization when only one producer.

Example parallel transform:
```csharp
var options = new ExecutionDataflowBlockOptions
{ MaxDegreeOfParallelism = Environment.ProcessorCount, BoundedCapacity = 32 };
var parseJson = new TransformBlock<string,MyDto>(json => JsonSerializer.Deserialize<MyDto>(json)!, options);
```

## 5. Backpressure & Flow Control
If target capacity is full, `Post` returns false; switch to `SendAsync` to await space.
```csharp
if (!buffer.Post(item)) await buffer.SendAsync(item); // cooperative
```
BoundedCapacity + proper awaiting prevents memory balloons.

## 6. Completion & Error Propagation
Call `Complete()` on the head block; set `PropagateCompletion = true` in links so completion + exceptions travel downstream.
Handle faults:
```csharp
try { await printer.Completion; }
catch (AggregateException ex) { /* inspect ex.InnerExceptions */ }
```
Inside blocks, throw normally; Dataflow aggregates.

## 7. Cancellation
Provide `CancellationToken` in `ExecutionDataflowBlockOptions`. When canceled, block transitions to faulted (OperationCanceledException).
```csharp
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
var worker = new ActionBlock<int>(async i => { await Task.Delay(1000); Console.WriteLine(i); }, new ExecutionDataflowBlockOptions{ CancellationToken = cts.Token });
```

## 8. Choosing Blocks (Design Heuristics)
Goal                          | Block(s)
------------------------------|--------
Simple buffering              | BufferBlock
Parallel compute map          | TransformBlock (MaxDegreeOfParallelism)
One input => many outputs     | TransformManyBlock
Terminal side-effect          | ActionBlock
Group N items together        | BatchBlock
Last value multicast          | BroadcastBlock
Synchronize two streams       | JoinBlock

## 9. Composition Patterns
1. Linear pipeline (classic ETL).
2. Tee / broadcast: one source feeding analytics + persistence.
3. Branch filtering: link with predicate by rejecting messages: `LinkTo(next, msg => IsValid(msg))`; add a trash block for rejects.
4. Ring / feedback: output re-posted for retries with a cap.

Predicate link example:
```csharp
source.LinkTo(validBlock, msg => msg.IsValid);
source.LinkTo(rejectBlock); // fallback when predicate false
```

## 10. Monitoring & Diagnostics
- Use `OutputCount` (BufferBlock) and `InputCount` (Propagators) for rough backlog.
- Wrap delegates to time operations.
- Consider ETW / EventSource for production tracing.

Simple instrumentation wrapper:
```csharp
var timed = new TransformBlock<string,string>(async s => {
    var sw = Stopwatch.StartNew();
    var result = await ProcessAsync(s);
    Console.WriteLine($"Processed {s} in {sw.ElapsedMilliseconds} ms");
    return result;
});
```

## 11. Common Pitfalls
Pitfall                        | Avoidance
-------------------------------|----------
Infinite queue growth          | Set BoundedCapacity; await SendAsync.
Never completing pipeline      | Always call head.Complete() & await tail.Completion.
Blocking synchronous code      | Use async delegates; avoid `.Result` or `.Wait()`.
Silent exception swallow       | Await `Completion`; inspect AggregateException.
Incorrect ordering assumption  | Remember unordered when EnsureOrdered=false.

## 12. When NOT to Use Dataflow
- Ultra-low latency single message (overhead > benefit).
- Simple single-producer/consumer with a channel (use `System.Threading.Channels`).
- Cross-process / remote actor model (use queue + service bus).

## 13. Interop & Alternatives
Compare quickly:
- `System.Threading.Channels`: lightweight; manual linking.
- Reactive Extensions (Rx): push-based, LINQ for streams, no built-in backpressure of same style.
- Akka.NET: actor supervision, distributed scenarios.
Dataflow sits sweet spot: in-process parallel pipelines with backpressure.

## 14. Mini Exercise (Try Now)
Build a pipeline:
- Input: file paths.
- Transform: read file + count lines.
- Batch: group counts in batches of 5.
- Action: print batch sum.
Extend: Add a retry branch for IO exceptions using feedback loop.

## 15. Summary (You Now Know)
- How to assemble Buffer/Transform/Action blocks.
- Control throughput with MaxDegreeOfParallelism + BoundedCapacity.
- Propagate completion & errors.
- Design selection heuristics + patterns.
- Avoid common pitfalls & compare alternatives.

Next: Quiz will solidify concepts and edge cases.

---
Commands: prev (back to topic selection), next (finish learn & go to quiz)
---
