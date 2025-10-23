# C# TPL Dataflow Library Quiz

Answer the questions below. Reply with answers in order, e.g.: `1:B 2:Y 3:AC 4:D ...`.

Formats:

- Single choice: A–D
- Yes/No: Y or N
- Multi-select: prefix `M:` in question; answer by concatenating letters (e.g. AC)

---
1. Which block type is best for transforming one input item into exactly one output item?  
   A. ActionBlock  
   B. TransformBlock  
   C. BufferBlock  
   D. BroadcastBlock  

2. Y/N: Setting `BoundedCapacity` helps introduce backpressure to prevent unbounded memory growth.  

3. What option controls the number of items a TransformBlock may process concurrently?  
   A. EnsureOrdered  
   B. MaxDegreeOfParallelism  
   C. BoundedCapacity  
   D. TaskScheduler  

4. Y/N: `Post()` will asynchronously wait until capacity is available when the block is full.  

5. When you want a block to emit multiple outputs from a single input (fan-out) you should use:  
   A. BatchBlock  
   B. BroadcastBlock  
   C. TransformManyBlock  
   D. WriteOnceBlock  

6. M: Which of the following contribute directly to controlling throughput & memory usage?  
   A. BoundedCapacity  
   B. MaxDegreeOfParallelism  
   C. EnsureOrdered  
   D. Completion  

7. Y/N: Using `PropagateCompletion = true` when linking ensures downstream blocks complete (or fault) automatically after upstream completes (or faults).  

8. If a delegate inside a TransformBlock throws an exception and you await its `Completion`, what happens?  
   A. Completion task faults with the exception  
   B. Completion task returns normally with default value  
   C. Block silently swallows the exception  
   D. Downstream blocks complete successfully regardless  

---
Score rule: Each correct answer = 1 point. 8/8 = 100%, 7/8 = 87.5%, 6/8 = 75%, etc. Need 80% (≥7/8) to proceed without remediation.

Commands after answering:
- `next` (if >=80%) proceed to lab design.
- `explain` (if <80%) get targeted recap + retry variant.
- `prev` go back to learning materials.

## Awaiting your answers
