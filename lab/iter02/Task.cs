using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;

namespace Iter02Lab
{
    public static class Lab02Tasks
    {
        /// <summary>
        /// Runs the adaptive batching pipeline end-to-end.
        /// </summary>
        /// <param name="itemCount">Number of integer items to feed.</param>
        /// <param name="minBatchSize">Minimum batch size.</param>
        /// <param name="maxBatchSize">Maximum batch size.</param>
        /// <param name="lowWaterMark">Backlog count at/below which we shrink batch size.</param>
        /// <param name="highWaterMark">Backlog count at/above which we grow batch size.</param>
        /// <param name="step">Increment/decrement for growth/shrink.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Result object containing batch sizes and throughput.</returns>
        public static async Task<LabRunResult> RunAdaptivePipelineAsync(int itemCount, int minBatchSize, int maxBatchSize, int lowWaterMark, int highWaterMark, int step, CancellationToken token)
        {
            // TODO[N1]: Adaptive batching logic implementation (See README: TODO N1)
            // TODO[N2]: Throughput calculation (See README: TODO N2)
            // [YOUR CODE GOES HERE]
            // Implement the adaptive buffering and batching according to README 'TODO N1'.
            // Collect per-item completion timestamps and compute throughput via TODO N2 rules.
            // Throw new NotImplementedException for now so tests fail until implemented.
            throw new NotImplementedException("TODO[N1]/TODO[N2] - Implement adaptive pipeline and throughput");
        }

        private static async Task EmitBatchAsync(List<int> currentBatch, int targetBatchSize, ActionBlock<List<int>> processBlock, List<int> batchSizes, CancellationToken token)
        {
            var toSend = currentBatch.ToList();
            currentBatch.Clear();
            batchSizes.Add(toSend.Count);
            if (!await processBlock.SendAsync(toSend, token).ConfigureAwait(false))
                throw new InvalidOperationException("Failed to post batch to processing block TODO[N1]");
        }

        /// <summary>
        /// Compute items/sec based on timestamps. (TODO[N2])
        /// </summary>
        /// <param name="timestamps">Collection of completion timestamps, one per item.</param>
        /// <returns>Items per second rounded to two decimals.</returns>
        public static double ComputeThroughput(IReadOnlyList<DateTime> timestamps)
        {
            // [YOUR CODE GOES HERE] for TODO[N2]
            // Implement throughput calculation per README.
            throw new NotImplementedException("TODO[N2] - Compute throughput");
        }
    }

    public record LabRunResult(IReadOnlyList<int> BatchSizes, double Throughput, int TotalItems);
}
