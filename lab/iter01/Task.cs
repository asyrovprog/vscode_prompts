using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lab.Iter01;

/// <summary>
/// Provides helpers for building a bounded text processing pipeline using TPL Dataflow.
/// The learner will later implement the TODO-marked sections (after stubs are inserted).
/// </summary>
public static class PipelineLab
{
    /// <summary>
    /// Normalize a line of text by trimming, lowering case and collapsing internal whitespace.
    /// </summary>
    // TODO[N1]: Implement normalization
    // Rules: trim, lowercase, collapse internal whitespace to single space.
    // Hints: See README.md & REF.md. Regex can help.
    public static string NormalizeLine(string line)
    {
        // TODO: implement this function
    }

    /// <summary>
    /// Decide whether a normalized line should be selected based on minimum length 
    /// and containing at least one letter.
    /// </summary>
    // TODO[N2]: Implement predicate
    // Conditions: non-empty, length >= minLength, contains at least one alphabetic char.
    public static bool ShouldSelect(string normalized, int minLength)
    {
        if (string.IsNullOrWhiteSpace(normalized)) return false;
        if (normalized.Length < minLength) return false;
        return normalized.Any(c => char.IsLetter(c));
    }

    /// <summary>
    /// Build the dataflow pipeline and return (input, completion, finalCountTask).
    /// </summary>
    // TODO[N3]: Build and link the pipeline blocks with PropagateCompletion.
    // TODO[N4]: Apply bounded capacity to demonstrate backpressure.
    // Should return: (headInputBlock, tailCompletionTask, finalCountTask returning int)
    public static (ITargetBlock<string> input, Task completion, Task<int> finalCount) BuildPipeline(int minLength, int boundedCapacity)
    {
        // [YOUR CODE GOES HERE]
        throw new NotImplementedException("TODO[N3|N4]");
    }

    // Will be used by your ActionBlock to increment passing lines.
    private static int _count;
}
