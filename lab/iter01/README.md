# Lab iter01: Web Content Pipeline with TPL Dataflow

## Goal
Implement a bounded, fault-tolerant TPL Dataflow pipeline that:
1. Accepts a list of URL strings.
2. Fetches page content (simulated) with retries + cancellation.
3. Tokenizes and normalizes words, filters stop words.
4. Aggregates global word frequencies with concurrency-safe updates and backpressure.
5. Produces the Top N (default 5) most frequent words.

## Learning Focus
- BoundedCapacity & backpressure.
- Linking blocks & completion propagation.
- Cancellation & fault handling.
- Parallelism configuration.
- Thread-safe shared state updates.

## Provided Simulation
Fetching is simulated by an in-memory dictionary so the lab is deterministic and offline-friendly.

## Files
- `Task.cs` – contains methods to implement.
- `Program.cs` – test harness.
- `REF.md` – hints for each TODO.

## Running Tests
```pwsh
cd lab/iter01
dotnet run
```
Tests output PASS/FAIL lines and final summary.

## TODO Sections
### TODO N1: Fetch Block
Implement creation of a `TransformBlock<string,string>` that:
- Accepts URLs (string).
- Returns HTML/text content (string).
- Uses MaxDegreeOfParallelism > 1.
- Has BoundedCapacity (e.g. 4) to demonstrate backpressure.
- Retries (simple retry loop) up to 2 extra attempts on transient failure.
- Respects cancellation via passed `CancellationToken`.
- Throws (faults block) if content ultimately unavailable.

See failure message referencing: "TODO[N1] - See README section 'TODO N1: Fetch Block'".

### TODO N2: Aggregator Logic
Implement word frequency aggregation that:
- Normalizes to lowercase.
- Splits on non-letter characters.
- Filters out stop words in `StopWords` set.
- Updates a shared `ConcurrentDictionary<string,int>` safely.
- Returns top N words ordered by descending frequency then alphabetically.

See failure message referencing: "TODO[N2] - See README section 'TODO N2: Aggregator'".

## Success Criteria
All tests PASS:
- Fetch block returns expected contents for test URLs.
- Aggregator produces expected top words list.

## After Completing
Run `dotnet run` again to confirm all PASS lines. Then use workflow command `next` to mark lab complete.

## Reference Sections
See `REF.md` for incremental hints.

---

## Checklist
- [ ] TODO N1 implemented
- [ ] TODO N2 implemented
- [ ] All tests PASS

Happy coding!