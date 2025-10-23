# Quiz 01 Answers & Explanations

1. Y  
FIFO is preserved by BufferBlock.
2. B  
BoundedCapacity applies backpressure to avoid unbounded memory.
3. Y  
Parallelism can reorder unless EnsureOrdered=true (default true, but turning it off allows reordering).
4. B  
TransformManyBlock emits 0..N outputs per input.
5. ABC  
Completion, exceptions, and cancellation propagate; Post() returning false does not propagate anything—it's a backpressure signal.
6. Y  
Awaiting tail completion surfaces aggregated exceptions.
7. B  
LinkTo with predicate and fallback link creates branching without extra buffering.
8. ABD  
Ordering (C) is unrelated; A/B/D indicate pressure need.
9. A  
Channels are lighter for simple queue patterns.
10. N  
ActionBlock is a target only (ITargetBlock<T>), no outputs.

Scoring: 1 point per question (max 10).