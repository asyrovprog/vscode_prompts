using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Lab.Iter08
{
    public record Order(string CustomerName, string Email, decimal Amount, bool IsExpress);

    // TODO N1 – Implement Result Type and Validation
    // 
    // Create a Result<T> record type that represents success or failure WITHOUT exceptions.
    // Required properties:
    //   - IsSuccess: bool indicating success
    //   - Value: T? containing the value on success (null on failure)
    //   - ErrorMessage: string? containing error message on failure (null on success)
    // 
    // Required static factory methods:
    //   - Success(T value): Returns a successful Result<T>
    //   - Failure(string error): Returns a failed Result<T>
    // 
    // [YOUR CODE GOES HERE]
    public record Result<T>
    {
        public bool IsSuccess { get; init; }
        public T? Value { get; init; }
        public string? ErrorMessage { get; init; }

        public static Result<T> Success(T value) => throw new NotImplementedException("TODO[N1]");
        public static Result<T> Failure(string error) => throw new NotImplementedException("TODO[N1]");
    }

    // TODO N3 – Implement Statistics Tracking
    // 
    // Create a ProcessorStats class to track processing metrics in a thread-safe manner.
    // Required properties (read-only):
    //   - TotalProcessed: Total orders received
    //   - SuccessCount: Successfully validated orders
    //   - FailureCount: Failed validation orders
    //   - ExpressCount: Express orders (successful only)
    //   - StandardCount: Standard orders (successful only)
    // 
    // Required methods (use Interlocked.Increment for thread safety):
    //   - IncrementTotal(): Increment total count
    //   - IncrementSuccess(): Increment success count
    //   - IncrementFailure(): Increment failure count
    //   - IncrementExpress(): Increment express count
    //   - IncrementStandard(): Increment standard count
    // 
    // [YOUR CODE GOES HERE]
    public class ProcessorStats
    {
        public int TotalProcessed => throw new NotImplementedException("TODO[N3]");
        public int SuccessCount => throw new NotImplementedException("TODO[N3]");
        public int FailureCount => throw new NotImplementedException("TODO[N3]");
        public int ExpressCount => throw new NotImplementedException("TODO[N3]");
        public int StandardCount => throw new NotImplementedException("TODO[N3]");

        public void IncrementTotal() => throw new NotImplementedException("TODO[N3]");
        public void IncrementSuccess() => throw new NotImplementedException("TODO[N3]");
        public void IncrementFailure() => throw new NotImplementedException("TODO[N3]");
        public void IncrementExpress() => throw new NotImplementedException("TODO[N3]");
        public void IncrementStandard() => throw new NotImplementedException("TODO[N3]");
    }

    public class OrderProcessor
    {
        // TODO N1 – Implement Result Type and Validation
        // 
        // Implement ValidateOrder to check:
        //   1. order.Amount > 0 (return Failure("Invalid amount") if not)
        //   2. order.Email.Contains("@") (return Failure("Invalid email") if not)
        //   3. Return Success(order) if all validations pass
        // 
        // CRITICAL: Never throw exceptions - always return Result<Order>
        // 
        // [YOUR CODE GOES HERE]
        public static Result<Order> ValidateOrder(Order order)
        {
            throw new NotImplementedException("TODO[N1]");
        }

        // TODO N2 – Build Encapsulated Pipeline
        // 
        // Create a resilient order processing pipeline using DataflowBlock.Encapsulate.
        // 
        // Pipeline architecture (4 stages):
        //   1. Entry: BufferBlock<Order>
        //   2. Validator: TransformBlock<Order, Result<Order>>
        //      - Call stats.IncrementTotal()
        //      - Call ValidateOrder(order)
        //      - If success: call stats.IncrementSuccess()
        //      - If failure: call stats.IncrementFailure()
        //      - Return result
        //   3. Pricer: TransformBlock<Result<Order>, Result<Order>>
        //      - Pass through failures unchanged
        //      - For successes: if Amount > 1000, apply 10% discount
        //   4. Router: TransformBlock<Result<Order>, Result<Order>>
        //      - Pass through failures unchanged
        //      - For successes: track express/standard stats
        //   5. Exit: BufferBlock<Result<Order>>
        // 
        // CRITICAL: Use DataflowLinkOptions { PropagateCompletion = true } on ALL links
        // 
        // Return: DataflowBlock.Encapsulate(entry, exit)
        // 
        // [YOUR CODE GOES HERE]
        public static IPropagatorBlock<Order, Result<Order>> CreateProcessor(ProcessorStats stats)
        {
            throw new NotImplementedException("TODO[N2]");
        }
    }
}
