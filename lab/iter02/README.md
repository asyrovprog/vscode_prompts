# Lab Iter02: Adaptive Batching + Throughput Monitor (Advanced TPL Dataflow Patterns)

## Goal

Implement an adaptive batching pipeline using TPL Dataflow that:

- Accepts a stream of integer work items.
- Dynamically adjusts batch size based on backlog pressure (bounded buffer count).
- Computes real-time throughput (items/sec) over the pipeline run.
- Emits summary of batch sizes and final throughput.

You will complete two core TODOs:

1. TODO[N1]: Adaptive batch sizing logic.
2. TODO[N2]: Throughput calculation logic.

After completion, all tests in `Program.cs` should pass.

## Files

`Task.cs` – Contains pipeline implementation with TODOs.
`Program.cs` – Test harness (no external test framework).
`REF.md` – Hints tied to each TODO.

## Requirements

### TODO N1: Adaptive Batch Sizing

Implement logic that chooses the next target batch size based on current backlog (`inputBuffer.Count`). Rules:

- Start at `minBatchSize`.
- If backlog >= `highWaterMark`, increase batch size by `+step` up to `maxBatchSize`.
- If backlog <= `lowWaterMark`, decrease batch size by `-step` down to `minBatchSize`.
- Avoid oscillation: don't change size more than once within the same 100ms window.
- Must process all items; batch flush occurs either when current batch reaches target size OR flush timer (300ms) fires.

### TODO N2: Throughput Calculation

Compute items/sec using timestamps recorded per processed item.

- Use first and last timestamp to determine elapsed duration.
- If duration < 1ms or fewer than 2 timestamps, throughput = 0.
- Round to two decimals.

## Constraints & Patterns

- Use `BufferBlock<int>` for input with `BoundedCapacity`.
- Use `ActionBlock<List<int>>` for batch processing (simulate lightweight workload via small delay).
- Propagate completion.
- Cancellation must be cooperatively respected.
- No global locks: prefer `Interlocked` or thread-safe collections.

## Success Criteria

- `ALL TESTS PASS` printed when running the project after you implement TODOs.
- Batch sizes show adaptation (at least one > min and one == min when backlog low).
- Throughput value > 0.

## How to Run

```powershell
dotnet run --project lab/iter02
```

## Output Example (after completion)

```text
PASS: Adaptive sizing varies (TODO[N1])
PASS: Throughput computed (TODO[N2])
--- Summary ---
ALL TESTS PASS
```

## Notes

You may experiment with watermarks / step size after tests pass to observe different behaviors.
