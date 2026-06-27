# Literal Prompt Programming (LPP) — Protocol

LPP is a small, shared vocabulary between a prompt author and an LLM. It gives
instruction files a predictable shape — variables, reusable functions, commands —
so the author and the model agree on what each construct means.

> **Best-effort, not a runtime.** No engine *executes* these files. The model
> reads them and SHOULD follow them as faithfully as it can: in order, without
> skipping or reordering steps, interpreting each instruction literally. Treat
> this as a protocol the agent commits to, not a guarantee the platform enforces.
> Keep modules small and explicit so the model can honor them reliably.

## 1. Core concepts

- **Module** — a single Markdown file using LPP: a goal, optional imports, optional
  prompt-function declarations, instructions, optional command mapping.
- **Runnable module** — has exactly one `# Instructions` section (the entry point).
- **Library module** — has only `# Prompt Functions`; no top-level `# Instructions`.
- **Prompt function** — a named, reusable instruction block (`ALL_CAPS`, optional
  `()`); MAY take `$PARAMETERS` and MAY return any value (status, text, number,
  object, array).
- **Variable** — a `$IDENTIFIER` placeholder. Session-scoped unless a function
  declares it local. Mutation is always an explicit step.
- **Command** — a user token mapped to an action. Only the user triggers commands.

## 2. Module shape

A runnable module SHOULD appear in this order:

1. `# Goal`
2. `# Instructions`

Optional sections: `# Include Instructions From` (imports), `# Prompt Functions`,
`# Command Mapping`, `# Initialization`.

A library module SHOULD appear in this order:

1. `# Goal`
2. `# Prompt Functions`

Library modules MUST NOT contain a top-level `# Instructions` section.

> LPP modules are plain Markdown — no platform-specific front matter is required
> or interpreted. A host (skill or agent) MAY add its own front matter; LPP
> ignores it.

## 3. Sections

- **`# Goal`** — states the intent boundary (what the module is for). No procedural
  steps.
- **`# Initialization`** — setup that runs before other logic (load dependencies,
  set initial variables).
- **`# Include Instructions From`** (or `# Referenced Instructions`) — one `- path`
  per line, relative to the module. Imports are declarative; nothing runs until an
  instruction explicitly calls into them. If a listed path is missing, report it as
  a fatal error and stop.
- **`# Instructions`** — ordered, imperative or control-flow steps the agent SHOULD
  follow in order. Steps MAY start with a verb (Execute, Load, Validate, Ask, Set,
  Return). Conditionals MUST be explicit ("If X then … else …"). If a step calls a
  prompt function that is not defined or imported, report a fatal error and stop.
- **`# Prompt Functions`** — each function declares:
  - a name with optional parameters (`## SHOW($MESSAGE)`),
  - `### Goal` (intent boundary, no steps),
  - `### Instructions` (ordered steps, same rules as above),
  - optional `### Constraints` (descriptive limits/invariants).
- **`# Command Mapping`** — one `- <command> - <action>` per line. Every
  user-exposed command MUST appear here; tokens MUST be unique. A command's action
  runs only when, and exactly once when, the user issues that command.

## 4. Dispatch & EXECUTE_PROMPT

In a skills/agents host, **the primary way to move between steps is to invoke the
relevant skill** (the agent calls a step skill). This is more robust than chaining
files.

`EXECUTE_PROMPT($PATH)` remains a convention for **in-module includes** — pulling in
a sibling instruction file within the same skill/agent bundle:

- **Re-read the target file from disk on every call** (do not rely on memory of a
  prior read).
- Announce it: `Following instructions from $PATH`.
- Then follow that file's instructions.
- If the file cannot be read, report the failure and halt.

Keep include chains shallow and modules small; each hop is a place the model may
paraphrase instead of reproduce.

## 5. Authoring guidance

- Be model- and tool-agnostic: refer to capabilities ("use your web-search tool"),
  not specific product tool IDs or model names.
- Prefer many small, single-purpose modules over one large one.
- Make every mutation and conditional explicit.
- State constraints (time, LOC, determinism) where they matter.
