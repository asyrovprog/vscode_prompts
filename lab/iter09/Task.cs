using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lab.Iter09;

public record ProcessingStats(int Processed, int Skipped, int Failed, int Total);

public class FileProcessor
{
    private readonly CancellationTokenSource _cts = new();
    private readonly ActionBlock<string> _block;
    
    // TODO[N1]: Add thread-safe counters for tracking progress
    // [YOUR CODE GOES HERE]
    // Add three int fields: _processedCount, _skippedCount, _failedCount
    
    public FileProcessor()
    {
        // TODO[N3]: Pass CancellationToken to block options and async operations
        // [YOUR CODE GOES HERE]
        // Set CancellationToken = _cts.Token in ExecutionDataflowBlockOptions
        _block = new ActionBlock<string>(
            ProcessFileAsync,
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 3
            });
    }
    
    private async Task ProcessFileAsync(string filePath)
    {
        // TODO[N1]: Check cancellation and track progress with Interlocked
        // [YOUR CODE GOES HERE]
        // Before processing, check if cancellation was requested using _cts.Token.IsCancellationRequested
        // If canceled, increment _skippedCount using Interlocked.Increment and return early
        // On successful processing, increment _processedCount using Interlocked.Increment
        // On exception (any exception), increment _failedCount using Interlocked.Increment
        
        // TODO[N3]: Pass CancellationToken to async operations
        // [YOUR CODE GOES HERE]
        // Pass _cts.Token to File.ReadAllTextAsync and Task.Delay
        var content = await File.ReadAllTextAsync(filePath);
        
        // Simulate processing
        await Task.Delay(50);
        
        // Count words
        var wordCount = content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        
        throw new NotImplementedException("TODO[N1]: Track processed/skipped/failed counts with Interlocked");
    }
    
    public bool Post(string filePath) => _block.Post(filePath);
    
    public void Cancel() => _cts.Cancel();
    
    // TODO[N2]: Implement graceful shutdown with timeout
    // [YOUR CODE GOES HERE]
    // Phase 1: Call _block.Complete() to stop accepting new files
    // Phase 2: Use await _block.Completion.WaitAsync(TimeSpan.FromSeconds(5)) to wait with timeout
    // Phase 3: If TimeoutException is thrown, call _cts.Cancel() and await _block.Completion again
    // Return true if graceful completion succeeded, false if forced cancellation was needed
    public async Task<bool> ShutdownAsync()
    {
        throw new NotImplementedException("TODO[N2]: Implement graceful shutdown with timeout - see README section 'TODO N2 – Implement Graceful Shutdown with Timeout'");
    }
    
    // TODO[N1]: Implement GetStats to return current counts
    // [YOUR CODE GOES HERE]
    // Return a ProcessingStats record with current counts
    public ProcessingStats GetStats()
    {
        throw new NotImplementedException("TODO[N1]: Return ProcessingStats - see README section 'TODO N1 – Implement Thread-Safe Progress Tracking'");
    }
}
