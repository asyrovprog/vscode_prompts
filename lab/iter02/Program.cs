using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iter02Lab
{
    internal static class Program
    {
        static async Task<int> Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            int failures = 0;

            async Task RunTest(string name, Func<Task<bool>> test)
            {
                bool ok;
                try
                {
                    ok = await test();
                }
                catch (Exception ex)
                {
                    ok = false;
                    Console.WriteLine($"FAIL: {name}: {ex.Message}");
                }
                Console.WriteLine(ok ? $"PASS: {name}" : $"FAIL: {name}");
                if (!ok) failures++;
            }

            await RunTest("Adaptive sizing varies (TODO[N1])", async () =>
            {
                var result = await Lab02Tasks.RunAdaptivePipelineAsync(
                    itemCount: 200,
                    minBatchSize: 8,
                    maxBatchSize: 32,
                    lowWaterMark: 10,
                    highWaterMark: 40,
                    step: 4,
                    token: cts.Token);
                if (result.BatchSizes.Count == 0) throw new Exception("No batches produced TODO[N1] - See README section 'TODO N1: Adaptive Batch Sizing'");
                if (!result.BatchSizes.Any(s => s > 8)) throw new Exception("Expected a grown batch size TODO[N1] - See README section 'TODO N1: Adaptive Batch Sizing'");
                if (!result.BatchSizes.Any(s => s == 8)) throw new Exception("Expected baseline batch size occurrences TODO[N1] - See README section 'TODO N1: Adaptive Batch Sizing'");
                return true;
            });

            await RunTest("Throughput computed (TODO[N2])", async () =>
            {
                var result = await Lab02Tasks.RunAdaptivePipelineAsync(
                    itemCount: 150,
                    minBatchSize: 5,
                    maxBatchSize: 25,
                    lowWaterMark: 8,
                    highWaterMark: 30,
                    step: 5,
                    token: cts.Token);
                if (result.Throughput <= 0) throw new Exception("Throughput must be > 0 TODO[N2] - See README section 'TODO N2: Throughput Calculation'");
                return true;
            });

            Console.WriteLine("--- Summary ---");
            Console.WriteLine(failures == 0 ? "ALL TESTS PASS" : $"{failures} TEST(S) FAILED");
            return failures == 0 ? 0 : 1;
        }
    }
}
