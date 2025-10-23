# Lab Iteration 01 – Bounded Text Processing Pipeline (TPL Dataflow)

Goal: Build a small bounded-capacity TPL Dataflow pipeline that normalizes incoming text lines, filters them by length + content, and counts how many lines pass.

You will implement the TODOs inside `Task.cs` (after stubs are inserted) to make the tests in `Program.cs` pass.

Pipeline Shape:
BufferBlock<string> -> TransformBlock (Normalize) -> TransformBlock (Filter/Map) -> ActionBlock (Count)

Learning Focus:
- BoundedCapacity & backpressure
- Transform vs Action blocks
- PropagateCompletion & orderly shutdown

Tests (run `dotnet run` inside `lab/iter01/`):
- TEST NormalizeLine (TODO[N1]) – normalization rules
- TEST ShouldSelect (TODO[N2]) – selection predicate
- TEST Pipeline (TODO[N1|N2|N3|N4]) – correct linking, propagation, counting

Success Criteria:
1. All tests print `OK` then `ALL TESTS PASSED`.
2. Your code respects provided min length and excludes lines with no alphabetic characters.
3. Completion does not hang (pipeline drains and terminates).

Time Target: ~15 minutes.

What To Do (once TODO stubs visible):
1. Implement NormalizeLine (trim, lowercase, collapse internal whitespace)
2. Implement ShouldSelect (length >= minLength AND contains letter)
3. Configure and link blocks with PropagateCompletion
4. Use bounded capacity so posting blocks when consumer is slow will apply natural backpressure
5. Update BuildPipeline to return the right tasks (final count)

Run:
```
cd lab/iter01
dotnet run
```

If a test fails, message points to the relevant TODO. Use `REF.md` for hints.
