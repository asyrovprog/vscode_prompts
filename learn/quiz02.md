# Quiz: Advanced TPL Dataflow Patterns

Answer format:
- Y/N for yes-no
- A-D for single choice
- M: (comma separated letters) for multi-select

Provide your answers in a list like: 1:A 2:Y 3:A,C 4:D ...

1. Y/N: BoundedCapacity primarily signals backpressure by forcing producers to await when the input queue is full.
2. A-D: Which block type is best for converting one input into zero-or-many outputs? A) TransformBlock B) ActionBlock C) TransformManyBlock D) BatchBlock
3. M: Which strategies help adapt throughput without rebuilding the entire graph? A) Swapping a worker block via indirection B) Changing MaxDegreeOfParallelism property on an existing block C) Introducing batching upstream D) Adding a throttle (rate limiter) before a hot block
4. A-D: Predicate routing using multiple LinkTo calls stops evaluating further links after: A) All predicates run B) First predicate succeeds C) Last predicate succeeds D) A timeout
5. Y/N: BatchBlock can reduce lock contention by grouping updates.
6. A-D: Which pair best differentiates bounding vs throttling? A) Bounding limits memory, throttling limits external rate B) Both only limit memory C) Bounding limits CPU, throttling limits memory D) Throttling always increases throughput
7. M: Recommended metrics for Dataflow instrumentation: A) Queue depth vs capacity B) Per-block latency C) GC heap size per block D) Success vs failure counts
8. A-D: Best option to isolate faulty messages without stopping pipeline: A) Fault the fetch block B) Quarantine block capturing failures C) Increase MaxDegreeOfParallelism D) Disable PropagateCompletion
9. Y/N: You can change MaxDegreeOfParallelism at runtime on an existing running block.
10. A-D: Hybrid architecture advantage combining Channels + Pipelines + Dataflow: A) Simpler single abstraction B) Avoids all synchronization primitives C) Each layer optimized for its concern D) Eliminates need for backpressure entirely
