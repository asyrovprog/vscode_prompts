using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Iter01Lab
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

            await RunTest("FetchBlock basic", async () => await Tests.TestFetchBlock(cts.Token));
            await RunTest("Aggregator top words", async () => await Tests.TestAggregator(cts.Token));

            Console.WriteLine("--- Summary ---");
            Console.WriteLine(failures == 0 ? "ALL TESTS PASS" : $"{failures} TEST(S) FAILED");
            return failures == 0 ? 0 : 1;
        }
    }
}