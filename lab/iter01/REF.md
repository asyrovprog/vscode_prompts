# Reference / Hints

## TODO[N1] NormalizeLine
Steps: Trim -> lowercase -> collapse multiple whitespace to a single space. Regex like `Regex.Replace(s, "\\s+", " ")` works.

## TODO[N2] ShouldSelect
Conditions: not null/empty, length >= minLength, contains any letter (`s.Any(char.IsLetter)`).

## TODO[N3] Linking & Propagation
Each `LinkTo` should set `PropagateCompletion = true` so `Complete()` on the head flows to tail.

## TODO[N4] Bounded Capacity
Set `BoundedCapacity` on each block (BufferBlock via DataflowBlockOptions; others via ExecutionDataflowBlockOptions). A small number like 2 or 4 makes backpressure observable.

## Verifying Backpressure (Optional Experiment)
Introduce an artificial delay in ActionBlock (e.g., `await Task.Delay(200)`) then observe `SendAsync` timing.

## Final Count
You can maintain a local `int count` closed over by the ActionBlock, then expose a Task that awaits `counter.Completion` and returns the count.
