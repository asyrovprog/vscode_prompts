# Reference Hints for Iter02

## TODO[N1] Adaptive Batch Sizing

- Look at backlog (`bufferBlock.Count`).
- Use simple threshold comparison against `highWaterMark` / `lowWaterMark`.
- Maintain a `lastAdjustment` timestamp; skip adjustments if less than 100ms since previous change.
- Clamp batch size between `minBatchSize` and `maxBatchSize`.
- Flush when `currentBatch.Count >= targetBatchSize` or timer elapsed.

## TODO[N2] Throughput Calculation

- Timestamps are captured per item completion.
- Throughput = `totalItems / elapsedSeconds` using first and last timestamp.
- Guard against division by zero; return 0 if insufficient data.
- Use `Math.Round(value, 2)` for final value.
