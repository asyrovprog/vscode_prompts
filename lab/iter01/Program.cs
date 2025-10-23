using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Lab.Iter01;

internal class Program
{
    private static async Task<int> Main()
    {
        var tests = new List<(string name, Func<Task<bool>> run)> {
            ("NormalizeLine", TestNormalize),
            ("ShouldSelect", TestShouldSelect),
            ("Pipeline", TestPipeline)
        };

        bool allOk = true;
        foreach (var (name, run) in tests)
        {
            try
            {
                var ok = await run();
                if (ok)
                    Console.WriteLine($"TEST {name}: OK");
                else
                {
                    allOk = false;
                    Console.WriteLine($"TEST {name}: FAIL (refer to README.md TODO guidance)");
                }
            }
            catch (Exception ex)
            {
                allOk = false;
                Console.WriteLine($"TEST {name}: EXCEPTION => {ex.Message}");
            }
        }

        Console.WriteLine(allOk ? "ALL TESTS PASSED" : "ONE OR MORE TESTS FAILED");
        return allOk ? 0 : 1;
    }

    private static Task<bool> TestNormalize()
    {
        var input = "  HeLLo   WORLD  ";
        var expect = "hello world";
        try
        {
            var actual = PipelineLab.NormalizeLine(input);
            return Task.FromResult(actual == expect);
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("FAIL: TODO[N1] not implemented – see README.md section for NormalizeLine");
            return Task.FromResult(false);
        }
    }

    private static Task<bool> TestShouldSelect()
    {
        try
        {
            bool a = PipelineLab.ShouldSelect("hello", 3) == true;
            bool b = PipelineLab.ShouldSelect("he", 3) == false;
            bool c = PipelineLab.ShouldSelect("12345", 3) == false; // no letters
            return Task.FromResult(a && b && c);
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("FAIL: TODO[N2] not implemented – see README.md section for ShouldSelect");
            return Task.FromResult(false);
        }
    }

    private static async Task<bool> TestPipeline()
    {
        var lines = new [] {
            "  apple pie  ",
            "x  ", // too short after normalize
            "   123   456   ", // no letters
            " BANANA  bread   recipe  ",
            "   churros   and   coffee   "
        };
        int expectedCount = 3; // apple pie, banana bread recipe, churros and coffee

        try
        {
            var (input, _, resultTask) = PipelineLab.BuildPipeline(minLength:5, boundedCapacity:2);
            foreach (var l in lines)
                await input.SendAsync(l);
            input.Complete();
            var final = await resultTask; // returns count
            return final == expectedCount;
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("FAIL: TODO[N3|N4] pipeline not implemented – see README.md BuildPipeline section");
            return false;
        }
    }
}
