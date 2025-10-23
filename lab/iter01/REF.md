# REF.md – Hints for Lab iter01

## Hint 1 (TODO N1)
Use `new TransformBlock<string,string>(async url => { ... }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4, BoundedCapacity = 4, CancellationToken = token });`

## Hint 2 (TODO N1)
A simple retry loop:
```csharp
for (int attempt = 0; attempt < 3; attempt++) {
    try { /* fetch */ break; } catch when (attempt < 2) { await Task.Delay(50, token); }
}
```
Throw if still not found.

## Hint 3 (TODO N2)
Use `Regex.Split` with pattern `"[^A-Za-z]+"` then `Where(w => w.Length > 0)`.

## Hint 4 (TODO N2)
Concurrent update:
```csharp
counts.AddOrUpdate(word, 1, (_, old) => old + 1);
```

## Hint 5 (TODO N2)
Ordering: `OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).Take(topN)`.

## Hint 6 (General)
Link blocks with `new DataflowLinkOptions { PropagateCompletion = true }` and call `fetchBlock.Complete()` then await `parse.Completion`.