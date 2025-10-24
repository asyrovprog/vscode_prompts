# Literal Prompt Programming (LPP) Specification v1.0.2

Status: Stable
Last-Updated: 2025-10-23
Scope: Defines universal structure and semantics for LPP prompt modules. Domain-agnostic.

## 1. Core Concepts

Prompt Module: Single text file containing: front-matter metadata, goal, optional imports, optional prompt function declarations, instructions (runnable prompt), optional command mapping, optional kernel spec.

Runnable Prompt: The `# Instructions` section (exactly one). Executes sequentially, invokes Prompt Functions, handles commands.

Prompt Function: Named reusable instruction block (ALL_CAPS optional `()` suffix), MAY accept parameters (referenced as `$NAME`), returns an outcome code.

Variable: Dynamic placeholder marked with `$` prefix (`$VAR`). Session-scoped unless explicitly persisted.

Command: User token mapped to an action (branch, function invocation, transition).

Outcome Codes: Minimal set: SUCCESS | FAILURE | HALT | RETRY (RETRY optional).

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
Contains ordered imperative statements. Each step SHOULD begin with a verb (Execute, Load, Validate, Ask, Set, Return).
Conditionals MUST be explicit (“If X then … else …”).

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
- returns: one or more allowed outcome codes
- sideEffects: explicit list (e.g., write:stdout, read:file, create:dir)
- failureModes: named scenarios for returning FAILURE
- preconditions: list of conditions required before execution
- postconditions: guaranteed truths after SUCCESS
- constraints (optional): textual limits (time budgets, max LOC) or guardrails; purely descriptive and MAY appear under a `### Constraints` heading in the module.

Invocation protocol (recommended pattern):
1. (Optional) announce intent
2. validate preconditions
3. execute steps
4. emit postconditions (on SUCCESS)
5. return outcome code

## 11. Variables

Syntax: `$IDENTIFIER`
MUST be validated before critical use if derived from uncertain sources (user input, external files).
SHOULD avoid silent mutation—any mutation is an explicit instruction step.

## 12. Outcome Codes

| Code    | Semantics |
|---------|-----------|
| SUCCESS | Operation completed normally |
| FAILURE | Recoverable issue; caller chooses alternate path or retry |
| HALT    | Execution intentionally suspended awaiting external input |
| RETRY   | Caller SHOULD immediately re-invoke with adjusted parameters |

No other codes unless extended in Kernel Spec.

## 13. Validation Rules (Abstract)

| Rule ID | Description |
|---------|-------------|
| V01     | Missing front-matter |
| V02     | Missing `# Goal` |
| V03     | Multiple `# Instructions` sections |
| V04     | Command referenced but not mapped |
| V05     | Duplicate command token |
| V06     | Imported file not found |
| V07     | Prompt Function invoked but undeclared (when Kernel Spec present) |
| V08     | Unknown outcome code |
| V09     | Variable used before any assignment/validation (optional warning) |
| V10     | Undeclared side-effect (if Kernel Spec present) |

## 14. EBNF Grammar

```
PromptModule     ::= RunnableModule | LibraryModule
RunnableModule   ::= FrontMatter GoalSection ImportSection? PFSection? InstructionSection CommandMapSection? KernelSpecSection?
LibraryModule    ::= FrontMatter GoalSection ImportSection? PFSection KernelSpecSection?
FrontMatter      ::= '---' MetaLine+ '---'
MetaLine         ::= Key ':' Value
GoalSection      ::= '# Goal' NL GoalLine+
ImportSection    ::= ('# Include Instructions From' | '# Referenced Instructions') NL ImportLine+
ImportLine       ::= '-' Path
PFSection        ::= '# Prompt Functions' NL PFLine+
PFLine           ::= '-' FunctionName (':' Description)?
InstructionSection ::= '# Instructions' NL InstLine+
InstLine         ::= '-'? VerbPhrase
CommandMapSection ::= ('# Command Mapping' | 'Response command handling') NL CmdLine+
CmdLine          ::= '-' CommandToken '-' ActionPhrase
KernelSpecSection ::= '# LPP Kernel Spec' NL SpecLine+
Key              ::= /[A-Za-z_][A-Za-z0-9_-]*/
Value            ::= /.*/
FunctionName     ::= /[A-Z0-9_]+(\(\))?/
CommandToken     ::= /[a-z0-9_-]+/
VerbPhrase       ::= /.*/
ActionPhrase     ::= /.*/
Path             ::= /.+\.prompt\.md$/
Description      ::= /.*/
SpecLine         ::= /.*/
NL               ::= '\n'
```

## 15. JSON Schema Skeleton

```json
{
  "type": "object",
  "required": ["frontMatter", "goal"],
  "properties": {
    "frontMatter": {
      "type": "object",
      "required": ["mode", "model"],
      "properties": {
        "mode": { "type": "string" },
        "model": { "type": "string" },
        "tools": { "type": "array", "items": { "type": "string" } },
        "description": { "type": "string" },
        "specRef": { "type": "string" }
      }
    },
    "goal": { "type": "string" },
    "imports": { "type": "array", "items": { "type": "string" } },
    "promptFunctions": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["name"],
        "properties": {
          "name": { "type": "string" },
          "params": { "type": "array", "items": { "type": "string" } },
          "returns": { "type": "array", "items": { "type": "string", "enum": ["SUCCESS","FAILURE","HALT","RETRY"] } }
        }
      }
    },
  "instructions": { "type": "array", "items": { "type": "string" }, "description": "Present only for runnable modules" },
    "commands": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["name","action"],
        "properties": {
          "name": { "type": "string" },
          "action": { "type": "string" }
        }
      }
    },
    "kernelSpec": { "type": "object" }
  }
}
```

## 16. Authoring Checklist

| Item | Must Be True |
|------|--------------|
| Front-matter present & closed | Yes |
| `mode`, `model` set | Yes |
| Goal concise & non-procedural | Yes |
| Exactly one Instructions section | Yes |
| Every invoked command mapped | Yes (if commands exist) |
| Imports resolve | Yes (if imports exist) |
| Functions used declared (if spec present) | Yes |
| Only allowed outcome codes used | Yes |
| No duplicate command tokens | Yes |

## 17. Change Management

Increment version when any normative rule changes. Modules reference this spec via `specRef`.

Version History:
- v1.0.2: Added optional `### Constraints` subsection convention and `constraints` attribute to Prompt Function contract (non-normative enhancement).
- v1.0.1: Initial stable release.

---
End of LPP Specification.
