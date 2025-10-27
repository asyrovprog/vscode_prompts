using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Lab.Iter04;

public record ProcessedItem(string Content, int LengthScore, int VowelCount);
public record ForkJoinDiagnostics(int Total, double AvgLength, double AvgVowels);

public static class DualPathForkJoin
{
    private static int _total;
    private static long _sumLength;
    private static long _sumVowels;

    private static int CountVowels(string s) => s.Count(c => "aeiouAEIOU".Contains(c));

    // TODO[N1]: Build Fork/Join Internal Pipeline
    public static (ITargetBlock<string> input, ISourceBlock<ProcessedItem> output) BuildInternalForkJoinPipeline()
    {
        // [YOUR CODE GOES HERE]
        throw new NotImplementedException("TODO[N1]");
    }

    // TODO[N2]: Encapsulate Composite as Custom Block
    public static IPropagatorBlock<string, ProcessedItem> CreateForkJoinBlock()
    {
        // [YOUR CODE GOES HERE]
        throw new NotImplementedException("TODO[N2]");
    }

    // TODO[N3]: Diagnostics (Optional)
    public static ForkJoinDiagnostics GetDiagnostics()
    {
        // [YOUR CODE GOES HERE]
        throw new NotImplementedException("TODO[N3]");
    }
}
