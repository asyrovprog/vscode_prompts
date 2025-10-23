using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks.Dataflow;
using System.ComponentModel;

namespace Iter01Lab
{
    public static class PipelineTasks
    {
        private static readonly Dictionary<string, string> _mockContent = new()
        {
            ["http://test/a"] = "<html><body>Dataflow pipelines enable parallel data processing with blocks.</body></html>",
            ["http://test/b"] = "Dataflow supports backpressure via BoundedCapacity and offers TransformBlock and ActionBlock.",
            ["http://test/c"] = "Blocks can propagate completion; linking ensures faults bubble. Dataflow backpressure backpressure." // intentional repeat
        };

        public static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "the","and","a","an","with","can","via","ensures","offers","html","body","can" ,"<html>","</html>"
        };

        /// <summary>
        /// TODO[N1]: Create a bounded fetch TransformBlock with retry and cancellation.
        /// <para>See README: section 'TODO N1: Fetch Block'</para>
        /// <param name="client">HttpClient instance (not heavily used for mock fetch).</param>
        /// <param name="token">Cancellation token for cooperative cancellation.</param>
        /// <returns>TransformBlock that outputs page content for each input URL.</returns>
        /// </summary>
        public static TransformBlock<string, string> CreateFetchBlock(HttpClient client, CancellationToken token)
        {
            var block = new TransformBlock<string, string>(async (url) =>
            {
                const int attempts = 2;
                for (int i = 0; i < attempts; i++)
                {
                    try
                    {
                        if (_mockContent.TryGetValue(url, out var message)) return message;
                    }
                    catch
                    {
                    }
                    await Task.Delay(100).ConfigureAwait(false);
                }
                throw new Exception();

            }, new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = 4,
                EnsureOrdered = false,
                MaxDegreeOfParallelism = 4,
                CancellationToken = token
            });

            return block;
        }

        /// <summary>
        /// TODO[N2]: Aggregate word frequencies from fetched content and return top N words.
        /// <para>See README: section 'TODO N2: Aggregator Logic'</para>
        /// <param name="urls">Sequence of URL strings to process.</param>
        /// <param name="topN">Number of top words to produce.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>List of (word,count) tuples ordered by frequency descending.</returns>
        /// </summary>
        public static async Task<IReadOnlyList<(string word, int count)>> RunPipelineAndGetTopAsync(IEnumerable<string> urls, int topN, CancellationToken token)
        {
            // Implementation for TODO[N2]
            // Build fetch block (relies on TODO[N1] correctness for realistic behavior)
            var client = new HttpClient();
            var fetch = CreateFetchBlock(client, token);

            var counts = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var parse = new ActionBlock<string>(content =>
            {
                token.ThrowIfCancellationRequested();
                var words = Regex.Split(content, "[^A-Za-z]+")
                                  .Where(w => !string.IsNullOrWhiteSpace(w))
                                  .Select(w => w.ToLowerInvariant())
                                  .Where(w => !StopWords.Contains(w));
                foreach (var w in words)
                {
                    counts.AddOrUpdate(w, 1, (_, old) => old + 1);
                }
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                BoundedCapacity = 8,
                CancellationToken = token
            });

            fetch.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });

            foreach (var url in urls)
            {
                await fetch.SendAsync(url, token);
            }
            fetch.Complete();
            try
            {
                await parse.Completion;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Pipeline failed before aggregation could complete. Check TODO[N1].", ex);
            }

            var ordered = counts
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => kv.Key)
                .Take(topN)
                .Select(kv => (kv.Key, kv.Value))
                .ToList();
            return ordered;
        }
    }

    internal static class Tests
    {
        public static async Task<bool> TestFetchBlock(CancellationToken token)
        {
            try
            {
                var client = new HttpClient();
                var block = PipelineTasks.CreateFetchBlock(client, token);
                var urls = new[] { "http://test/a", "http://test/b" };
                foreach (var u in urls) await block.SendAsync(u, token);
                block.Complete();
                var results = new List<string>();
                while (await block.OutputAvailableAsync())
                {
                    while (block.TryReceive(out var item)) results.Add(item);
                }
                await block.Completion; // ensure completion
                if (results.Count != urls.Length) throw new Exception("Unexpected result count TODO[N1] - See README section 'TODO N1: Fetch Block'");
                if (!results.Any(c => c.Contains("Dataflow pipelines"))) throw new Exception("Missing expected content snippet TODO[N1] - See README section 'TODO N1: Fetch Block'");
                return true;
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task<bool> TestAggregator(CancellationToken token)
        {
            try
            {
                var top = await PipelineTasks.RunPipelineAndGetTopAsync(new[] { "http://test/a", "http://test/b", "http://test/c" }, 5, token);
                if (!top.Any(t => t.word == "dataflow")) throw new Exception("Expected 'dataflow' in top list TODO[N2] - See README section 'TODO N2: Aggregator'");
                if (!top.Any(t => t.word == "backpressure")) throw new Exception("Expected 'backpressure' in top list TODO[N2] - See README section 'TODO N2: Aggregator'");
                return true;
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}