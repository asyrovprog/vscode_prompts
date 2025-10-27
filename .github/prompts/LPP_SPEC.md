# Literal Prompt Programming (LPP) Specification (Draft)

Status: Draft
Last-Updated: 2025-10-26
Scope: Defines universal structure and semantics for LPP prompt modules. Domain-agnostic.

<!-- LPP_SPEC_ID: LPP_STABLE -->
<!-- LPP_MODEL_DIGEST_START -->
CoreEntities:
	PromptModule: frontMatter + Goal + (optional) Prompt Functions + (optional) Instructions (if runnable) + optional Command Mapping + optional Kernel Spec
	PromptFunction: reusable named block (UPPER_SNAKE, optional ()) with params as $VARS
	Variable: $NAME placeholder; validate before critical use
	Command: user token mapped to action
Returns:
	Functions MAY return: outcome status strings, arbitrary text, structured objects, numbers, arrays, or other functions (higher-order) unless constrained by Kernel Spec.
OutcomeStatuses:
	Baseline: SUCCESS, FAILURE
	Custom statuses MAY be defined (author-chosen strings) but are entirely optional. They are advisory labels unless the Kernel Spec assigns formal semantics.
RulesMinimal:
	- Exactly one '# Instructions' section in runnable modules
	- Goal is non-procedural
	- Custom statuses are advisory unless Kernel Spec declares semantics
<!-- LPP_MODEL_DIGEST_END -->

## 1. Core Concepts

Prompt Module: Single text file containing: front-matter metadata, goal, optional imports, optional prompt function declarations, instructions (runnable prompt), optional command mapping, optional kernel spec.

Runnable Prompt: The `# Instructions` section (exactly one). Executes sequentially, invokes Prompt Functions, handles commands.

Prompt Function: Named reusable instruction block (ALL_CAPS optional `()` suffix), MAY accept parameters (referenced as `$NAME`), and MAY return any value type: status string, plain text, number, object, array, or even another function. If returning a status string used for control-flow, its meaning SHOULD be documented (Kernel Spec if formal).

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

Library Modules MUST NOT include an `# Instructions` section. If present, it SHOULD be empty or a single HALT and MAY be flagged by tooling.

## 3. Front-Matter

Delimited by `---` ... `---` at file head.

Required keys:

- mode
- model

Optional keys:

- tools
- description
- specRef (path or URL pointing to this spec, e.g. `.github/prompts/LPP_SPEC.md`)

No imperative logic inside front-matter.

## 4. Goal Section

Header: `# Goal`
Purpose: States intent boundary (what this module is for). MUST NOT contain procedural steps.

## 5. Imports

Header: `# Include Instructions From` (or `# Referenced Instructions`)
Each line: `- relative/path/to/other.prompt.md`
All paths MUST exist. Imports are declarative; execution only happens via explicit function or macro calls inside `# Instructions`.

## 6. Prompt Functions Section

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

## 7. Instructions Section

Header: `# Instructions` (exactly one per module).
Contains ordered imperative or control-flow statements. Steps MAY begin with a verb (Execute, Load, Validate, Ask, Set, Return) for clarity, but any single clear instruction is valid. Conditionals MUST be explicit (“If X then … else …”).

## 8. Command Mapping

Header: `# Command Mapping` (or “Response command handling”).
Format each line: `- <command> - <action description>`
All user-exposed commands referenced in instructions MUST appear here.
No duplicate command tokens.

## 9. Kernel Spec (Optional)

Header: `# LPP Kernel Spec`
May formalize:

- variables: name, required, mutability (read|write)
- promptFunctions: name, params, returns, sideEffects, failureModes
- outcomes: enumerated semantics
- invariants: conditions that must always hold
- constraints: optional guardrails (time, LOC, resource caps)

This section is authoritative when present; tooling SHOULD validate actual usage against it.

## 10. Prompt Function Contract

Attributes (recommended):

- name (ALL_CAPS)
- params: ordered list of `$PARAM` names
- returns: ANY value (status string, text, number, object, array, function). If control-flow depends on particular status strings, list only those you actually use (e.g., SUCCESS, FAILURE) and define semantics in Kernel Spec or inline notes. Avoid implying mandatory special codes.
- sideEffects: explicit list (e.g., write:stdout, read:file, create:dir)
- failureModes: named scenarios for returning FAILURE (or other error-like statuses if defined)
- preconditions: list of conditions required before execution
- postconditions: guaranteed truths after SUCCESS (or success-like custom status)
- constraints (optional): textual limits (time budgets, max LOC) or guardrails; purely descriptive and MAY appear under a `### Constraints` heading in the module.

Invocation protocol (recommended pattern):

1. (Optional) announce intent
2. validate preconditions
3. execute steps
4. emit postconditions (on SUCCESS or success-like custom status)
5. return value (status or arbitrary data)

## 11. Variables

Syntax: `$IDENTIFIER`
MUST be validated before critical use if derived from uncertain sources (user input, external files).
SHOULD avoid silent mutation—any mutation is an explicit instruction step.

---
End of LPP Specification (Draft). Future formal appendices may introduce: validation rule catalog, machine-readable schema, grammar, authoring checklist, and versioning policy once patterns stabilize.
