# Quiz 01: C# TPL Dataflow Library

Answer format:
- Y/N questions: reply Y or N
- Single choice: A/B/C/D
- Multi-select (M): list letters without spaces (e.g., ACD)

1. (Y/N) BufferBlock<T> preserves FIFO ordering of messages.  
2. (A-D) Primary reason to set BoundedCapacity on a TransformBlock:  
   A. Increase memory usage  
   B. Enable backpressure to upstream producers  
   C. Guarantee ordering  
   D. Reduce thread pool size  
3. (Y/N) Setting MaxDegreeOfParallelism > 1 on a TransformBlock may cause outputs to be reordered unless EnsureOrdered=true.  
4. (A-D) Which block type produces zero or many outputs per single input?  
   A. TransformBlock  
   B. TransformManyBlock  
   C. ActionBlock  
   D. BufferBlock  
5. (M) Which of the following propagate completion & faults downstream automatically when PropagateCompletion=true on links?  
   A. Completion of head block  
   B. Exceptions thrown inside a block delegate  
   C. CancellationToken triggering  
   D. Post() returning false  
6. (Y/N) Awaiting tailBlock.Completion is sufficient to observe exceptions in the pipeline (if completion was propagated).  
7. (A-D) Best primitive to branch valid vs invalid messages using a predicate:  
   A. BroadcastBlock  
   B. LinkTo with predicate + fallback link  
   C. ActionBlock with if/else  
   D. BatchBlock  
8. (M) Common signs you need BoundedCapacity (choose all):  
   A. Memory usage steadily growing  
   B. Producers much faster than consumers  
   C. Need to guarantee ordering  
   D. Desire natural backpressure  
9. (A-D) Alternative more suitable than Dataflow for simple single producer/consumer queue:  
   A. Channels (System.Threading.Channels)  
   B. Akka.NET  
   C. Reactive Extensions  
   D. Manual Thread + Lock  
10. (Y/N) ActionBlock<T> implements ISourceBlock<T>.  

Provide your answers in order, e.g.: Y B Y B ABC Y B ABD A N

Commands: prev (return to learn), next (after scoring & logging), explain (after scoring <80%)