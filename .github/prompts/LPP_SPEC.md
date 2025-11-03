# Literal Prompt Programming (LPP) Specification (Draft)

Status: Draft
Last-Updated: 2024-10-26
Scope: Defines universal structure and semantics for LPP prompt modules. Domain-agnostic.

## 1. Core Concepts

Prompt Module: Single text file containing: front-matter metadata, goal, optional imports, optional prompt function declarations, instructions (runnable prompt), optional command mapping, optional kernel spec.

Runnable Prompt: The `# Instructions` section (exactly one). Executes sequentially, invokes Prompt Functions, handles commands. Must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally.

Prompt Function: Named reusable instruction block (ALL_CAPS optional `()` suffix), MAY accept parameters (referenced as `$NAME`), and MAY return any value type: status string, plain text, number, object, array, or even another function. If returning a status string used for control-flow. Must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally.

Variable: Dynamic placeholder marked with `$` prefix (`$VAR`). Session-scoped unless explicitly persisted.

Command: User token mapped to an action (branch, function invocation, transition).

## 2. Module Types and Mandatory Sections

There are two module types:

1. Runnable Module (default): provides executable workflow logic, this module contains top-level execution instructions.
2. Library Module: provides only prompt function definitions; has no top-level execution instructions.

### 2.1 Runnable Module Required Order (normative):

1. Front-Matter block
2. `# Goal`
3. `# Instructions`

Optional sections:

- Imports (`# Include Instructions From` or `# Referenced Instructions`)
- Prompt Functions (`# Prompt Functions`)
- Command Mapping (`# Command Mapping` or “Response command handling”)

### 2.2 Library Module Required Order (normative):

1. Front-Matter block
2. `# Goal`
3. `# Prompt Functions`

Library Modules MUST NOT include top-level `# Instructions` section.

## 3. Goal Section

Header: `# Goal`
Purpose: States intent boundary (what this module is for). This section does not contain procedural steps.

## 4. Imports

Header: `# Include Instructions From` (or `# Referenced Instructions`)
Each line: `- relative/path/to/other.prompt.md`
All paths MUST exist. Imports are declarative; execution only happens via explicit function or macro calls inside `# Instructions`.

## 5. Prompt Functions Section

Header: `# Prompt Functions`

Each listed function MUST include following required sections:

- Name with optional parameters. Examples: 
    - `## FOO()`
    - `## SHOW($MESSAGE)`

- `### Goal` – States intent boundary (what this module is for). This section does not contain procedural steps.

- `### Instructions` – Contains ordered imperative or control-flow statements which must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally. Steps MAY begin with a verb (Execute, Load, Validate, Ask, Set, Return) for clarity, but any single clear instruction is valid. Conditionals MUST be explicit ("If X then … else …").

Optional Subsections:

- `### Constraints` – bounded resource/time/LOC limits or invariants relevant to instructions; descriptive only. This section does not contain procedural steps.

## 6. Instructions Section

Header: `# Instructions` (exactly one per module).
Contains ordered imperative or control-flow statements which must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally. Steps MAY begin with a verb (Execute, Load, Validate, Ask, Set, Return) for clarity, but any single clear instruction is valid. Conditionals MUST be explicit ("If X then … else …").

## 7. Command Mapping

Header: `# Command Mapping` (or “Response command handling”).
Format each line: `- <command> - <action description>`
All user-exposed commands referenced in instructions MUST appear here. No duplicate command tokens.

## 8. Variables

Syntax: `$IDENTIFIER`
Prompt program variable. Global unless explicitly specified as local. SHOULD avoid silent mutation—any mutation is an explicit instruction step.

---
End of LPP Specification

