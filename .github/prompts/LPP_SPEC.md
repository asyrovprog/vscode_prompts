# Literal Prompt Programming (LPP) Specification (Draft)

Status: Draft
Last-Updated: 2024-10-26
Scope: Defines universal structure and semantics for LPP prompt modules. Domain-agnostic.

## 1. Core Concepts

Prompt Module: Single text file containing: front-matter metadata, goal, optional imports, optional prompt function declarations, instructions (runnable prompt), optional command mapping, optional kernel spec.

Runnable Prompt: The `# Instructions` section (exactly one). Executes sequentially, invokes Prompt Functions, handles commands. Must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally.

Prompt Function: Named reusable instruction block (ALL_CAPS optional `()` suffix), MAY accept parameters (referenced as `$NAME`), and MAY return any value type: status string, plain text, number, object, array, or even another function. If returning a status string used for control-flow, its meaning SHOULD be documented (Kernel Spec if formal). Must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally.

Variable: Dynamic placeholder marked with `$` prefix (`$VAR`). Session-scoped unless explicitly persisted.

Command: User token mapped to an action (branch, function invocation, transition).

Outcome Statuses: Recommended baseline: SUCCESS (normal completion), FAILURE (did not achieve intended result). Authors MAY introduce additional status strings, but functions are free to return ANY value (text, number, object, array, function) and are NOT expected to use special keywords like HALT or RETRY unless they find them helpful and explicitly define them.

## 2. Module Types and Mandatory Sections

There are two module types:

1. Runnable Module (default): provides executable workflow logic.
2. Library Module: provides only prompt function definitions; has no top-level execution.

### 2.1 Runnable Module Required Order (normative):

1. Front-Matter block
2. `# Goal`
3. `# Instructions`

Optional sections:

- Imports (`# Include Instructions From` or `# Referenced Instructions`)
- Prompt Functions (`# Prompt Functions`)
- Command Mapping (`# Command Mapping` or “Response command handling”)
- Kernel Spec (`# LPP Kernel Spec`)

### 2.2 Library Module Required Order (normative):

1. Front-Matter block
2. `# Goal`
3. `# Prompt Functions`

Library Modules MUST NOT include an `# Instructions` section.

## 3. Goal Section

Header: `# Goal`
Purpose: States intent boundary (what this module is for). MUST NOT contain procedural steps.

## 4. Imports

Header: `# Include Instructions From` (or `# Referenced Instructions`)
Each line: `- relative/path/to/other.prompt.md`
All paths MUST exist. Imports are declarative; execution only happens via explicit function or macro calls inside `# Instructions`.

## 5. Prompt Functions Section

Header: `# Prompt Functions`

Each listed function SHOULD include:

- Name
- Brief description
Optionally: params, returns, side-effects.
Actual invocation logic lives in shared modules or in instructions.

Optional Subsection Conventions (non-normative but recommended for clarity):

- `### Goal` – plain-language intent of the function (no procedural steps).
- `### Constraints` – bounded resource/time/LOC limits or invariants relevant to implementation; descriptive only.
- `### Instructions` – ordered or bulleted implementation guidance internal to the function definition.
Tooling MAY ignore these headings; their presence MUST NOT be required for validity.

## 6. Instructions Section

Header: `# Instructions` (exactly one per module).
Contains ordered imperative or control-flow statements which must be executed as program code: sequentially, without skipping steps, without reordering, interpreting each instruction literally. Steps MAY begin with a verb (Execute, Load, Validate, Ask, Set, Return) for clarity, but any single clear instruction is valid. Conditionals MUST be explicit ("If X then … else …").

## 7. Command Mapping

Header: `# Command Mapping` (or “Response command handling”).
Format each line: `- <command> - <action description>`
All user-exposed commands referenced in instructions MUST appear here.
No duplicate command tokens.

## 8. Variables

Syntax: `$IDENTIFIER`
MUST be validated before critical use if derived from uncertain sources (user input, external files).
SHOULD avoid silent mutation—any mutation is an explicit instruction step.

---
End of LPP Specification

