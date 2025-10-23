# C# TPL Dataflow Library Quiz Answers & Explanations

1. B – TransformBlock performs one-to-one transformation producing exactly one output per input.
2. Y – BoundedCapacity limits queued items enforcing natural backpressure.
3. B – MaxDegreeOfParallelism sets concurrency of the processing delegate.
4. N – Post() returns false immediately if the block cannot accept; it does NOT wait. `SendAsync` waits.
5. C – TransformManyBlock allows one-to-many expansion (fan-out).
6. AB – BoundedCapacity shapes queue size; MaxDegreeOfParallelism shapes concurrent execution. EnsureOrdered controls ordering, not throughput directly; Completion is a lifecycle signal.
7. Y – PropagateCompletion forwards completion or fault status downstream.
8. A – The block’s Completion task faults and awaiting it surfaces the exception.

Passing threshold: ≥7 correct (≥80%).
