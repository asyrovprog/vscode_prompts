# Advanced TPL Dataflow Patterns (15‑Minute Top‑Down Guide)

---
## 1. Mental Model: "Programmable Conveyor Belts"
Think of a Dataflow network as a set of specialized conveyor belts (blocks) passing items (messages). Each belt can: transform, filter, batch, or absorb side-effects. You orchestrate throughput (how fast items move), shape (where items branch), and resilience (what happens under stress/failure).

Core knobs you control:
- Parallelism: How many workers pick items off a belt simultaneously.
- Capacity: How many items can queue on a belt before producers slow (backpressure).
- Ordering: Preserve message order or relax for higher throughput.
- Completion: When upstream belts finish, downstream belts know they can wrap up.

---
## 2. Pattern: Dynamic Graph / Late Linking
Scenario: You discover at runtime you need extra processing steps (e.g., conditional enrichment).

Technique:
1. Start with a base pipeline.
2. Based on first N messages or configuration fetched asynchronously, attach new blocks using `LinkTo` with `PropagateCompletion = true`.
3. Optionally detach or redirect using predicates.

Example Snippet:
```csharp
var classify = new TransformBlock<Item, ItemInfo>(i => Classify(i));
var dynamicTargets = new List<ITargetBlock<ItemInfo>>();

// Later, when config arrives:
var extra = new ActionBlock<ItemInfo>(info => Enrich(info));
classify.LinkTo(extra, new DataflowLinkOptions { PropagateCompletion = true });
dynamicTargets.Add(extra);
```
Use Case: Feature flags, tenant-specific enrichment, progressive warm-up.

Tradeoff: Late links risk initial messages bypassing new logic—buffer or gate early.

---
## 3. Pattern: Conditional Routing (Predicate Links)
You can branch without full `TransformManyBlock` by using multiple `LinkTo` calls with predicates. The first predicate that matches consumes the message.

```csharp
var router = new BufferBlock<Event>();
router.LinkTo(highPriBlock, e => e.Priority == Priority.High);
router.LinkTo(normalBlock, e => e.Priority == Priority.Normal);
router.LinkTo(DataflowBlock.NullTarget<Event>()); // sink remainder
```
Pitfall: Ordering vs fairness—high priority could starve lower routes if entering hot loop.
Mitigation: Insert a `TransformBlock` that randomizes or batches mixed priority events.

---
## 4. Pattern: Throttling vs Bounding
Both constrain rate but in different layers:
- BoundedCapacity: Backpressure—producer awaits when queue full.
- Explicit Throttle: Delay intake regardless of queue (e.g., external API rate limit).

Hybrid Implementation:
```csharp
var intake = new TransformBlock<Request, Request>(async r => {
    await rateLimiter.WaitAsync(); // Token bucket / SemaphoreSlim
    return r;
}, new ExecutionDataflowBlockOptions { BoundedCapacity = 20 });
```
Guideline: Use bounding for internal CPU/memory balance; add throttling when external contracts (API QPS) or fairness required.

---
## 5. Pattern: Batching & Micro-batching
`BatchBlock<T>` collects a fixed size array; `GroupBatchSize` in custom blocks can emulate variable triggers.

When beneficial:
- Amortize cost (e.g., bulk database write).
- Reduce contention (e.g., one lock per batch vs per item).

Anti-Pattern: Over-large batches increase latency and memory spikes. Start small (16–64) and profile.

BatchBlock Example:
```csharp
var batch = new BatchBlock<Trade>(32);
var bulkInsert = new ActionBlock<Trade[]>(arr => InsertMany(arr));
batch.LinkTo(bulkInsert, new DataflowLinkOptions { PropagateCompletion = true });
```

---
## 6. Pattern: Error Segregation (Quarantine Block)
Goal: Keep healthy flow moving while isolating bad messages.

Implementation:
```csharp
var process = new TransformBlock<Input, Output>(async i => await SafeProcess(i));
var errors = new BufferBlock<Failed<Input>>();
process.LinkTo(DataflowBlock.NullTarget<Output>()); // normal sink
process.LinkTo(errors, o => o == null); // assuming null signals failure
```
Better: Use a wrapper result type: `Result<T>` with Success/Failure.

Quarantine Strategy:
- Retry subset with exponential backoff block.
- Persist to dead-letter queue.
- Alert metrics (count per time window).

---
## 7. Pattern: Completion & Coordinated Shutdown
Key principles:
1. Initiate shutdown from root: call `Complete()` on first producer.
2. Rely on `PropagateCompletion = true` chain to close downstream.
3. Await final block `.Completion` task.
4. Use a `CancellationToken` to preempt long-running operations if SLA exceeded.

Coordinated Example:
```csharp
startBlock.Complete();
await finalBlock.Completion; // all done
```
Add a timeout wrapper:
```csharp
await Task.WhenAny(finalBlock.Completion, Task.Delay(TimeSpan.FromSeconds(30))); // escalate if needed
```

---
## 8. Pattern: Adaptive Parallelism
Adjust `MaxDegreeOfParallelism` at runtime: you cannot change an existing block’s setting, but you can swap blocks behind an indirection target.

Technique:
1. Messages enter a `BufferBlock<T>` (stable address).
2. A current worker `TransformBlock<T,T>` is linked.
3. On load spike, create a new `TransformBlock` with higher parallelism, unlink old, drain, link new.

Sketch:
```csharp
var ingress = new BufferBlock<Job>();
var worker = MakeWorker(parallelism:4);
ingress.LinkTo(worker, new DataflowLinkOptions { PropagateCompletion = true });
// later
var upgraded = MakeWorker(parallelism:16);
ingress.LinkTo(upgraded, new DataflowLinkOptions { PropagateCompletion = true });
// mark old worker Complete after drain
```
Needs careful draining & predicate routing to avoid item loss.

---
## 9. Pattern: Metrics & Instrumentation
Wrap blocks to emit metrics:
- Throughput (items/sec).
- Queue depth vs `BoundedCapacity` usage.
- Latency per block (stopwatch around delegate).
- Fail count / retry count.

Simple wrapper:
```csharp
var instrumented = new TransformBlock<T,T>(async item => {
    var sw = Stopwatch.StartNew();
    var result = await Inner(item);
    sw.Stop();
    metrics.Record(sw.Elapsed);
    return result;
});
```
Export to Prometheus or Application Insights for live tuning.

---
## 10. Pattern: Hybrid (Dataflow + Channels + Pipelines)
Use Dataflow for high-level orchestration; channels for fine-grained async streaming; pipelines for efficient I/O parsing.

Example Blend:
1. Socket bytes enter `System.IO.Pipelines` for framing.
2. Parsed messages written to `Channel<T>` for fast producer/consumer.
3. Channel consumer posts into Dataflow graph for enrichment & persistence.

Benefit: Each abstraction used where strongest.

---
## 11. Tuning Checklist
- Start with bounded capacities (4–16) then measure blocking.
- Ensure non-blocking delegates (avoid sync I/O).
- Profile locking hotspots in aggregation dictionaries.
- Introduce batching only after verifying contention.
- Simulate failure bursts early (fault a block, confirm completion chain).

---
## 12. Practice Mini-Exercises
1. Add a quarantine block to your existing lab pipeline.
2. Introduce a BatchBlock to group words then aggregate counts.
3. Add instrumentation (items/sec) to fetch block.
4. Swap worker block at runtime for adaptive parallelism.

---
## 13. Recap Mnemonics
"LCP-BAT-FAM": Linking, Capacity, Propagation – Batch, Adapt, Throttle – Fault, Aggregate, Metrics.

Memorize core flow: Produce -> Transform/Filter -> Route/Batch -> Aggregate -> Persist -> Complete.

---
## 14. Suggested Next Deep Dives
- System.Threading.Channels patterns.
- Reactive Extensions vs Dataflow bridging.
- High-performance dictionary strategies.

---
### Summary (Log)
Advanced patterns: dynamic graphs, predicate routing, hybrid throttling/bounding, batching, quarantine, adaptive parallelism, metrics instrumentation, hybrid architecture with Channels/Pipelines, tuning checklist.
