---
mode: ask
model: GPT-5.2 (copilot)
description: LPP lint validator prompt
---

# Goal
Validate an LPP prompt module against lpp_spec.md and report errors and warnings.

# Instructions
- Load, read, and understand .github/prompts/lpp_spec.md.
- If $PROMPT_PATH is not set, ask the user for a prompt path to lint and set $PROMPT_PATH.
- If $STRICT is not set, set $STRICT = false.
- Execute LINT_WORKSPACE($PROMPT_PATH, $STRICT).

# Prompt Functions

## LINT_WORKSPACE($PROMPT_PATH, $STRICT)

### Goal
Lint the target prompt and any included prompt modules, then report findings.

### Instructions
- Initialize $VISITED = [].
- Initialize $FINDINGS = [].
- Execute LINT_FILE($PROMPT_PATH, $STRICT, $VISITED, $FINDINGS).
- Output a report with this format:
  - Title line: "LPP Lint Report"
  - Target: $PROMPT_PATH
  - Summary: total files checked, error count, warning count
  - Findings list, ordered by severity (ERROR then WARN), then by file path, then by line number
- If there are no findings, output: "No issues found."

## LINT_FILE($PATH, $STRICT, $VISITED, $FINDINGS)

### Goal
Validate a single prompt file and recursively lint its includes.

### Instructions
- If $PATH is in $VISITED, return.
- Add $PATH to $VISITED.
- Read file content from $PATH. If it cannot be read, add ERROR to $FINDINGS: "File not found or unreadable" and return.
- Parse front matter:
  - If file does not start with a front-matter block delimited by '---' on the first line and a closing '---', add ERROR.
- Parse top-level sections (lines that start with "# "):
  - Capture section titles and line numbers.
- Validate required sections:
  - There must be exactly one "# Goal". If missing or duplicated, add ERROR.
  - If "# Instructions" exists, module is runnable. If it appears more than once, add ERROR.
  - If "# Instructions" does not exist, module is library and must contain "# Prompt Functions". If missing, add ERROR.
- Validate section order for runnable modules:
  - "# Goal" must appear before "# Instructions". If not, add ERROR.
- Validate includes:
  - For each "# Include Instructions From" or "# Referenced Instructions" section, collect each "- path" line.
  - If an include path contains "$", add WARN: "Include path is dynamic; includes should be static paths."
  - If include path is static, attempt to read it. If missing, add ERROR.
  - For each include path that can be read, execute LINT_FILE(include_path, $STRICT, $VISITED, $FINDINGS).
- Validate dynamic calls:
  - Initialize $VARS = {}.
  - Scan "# Instructions" text for lines matching `Set $VAR = value`. For each match, add VAR: value to $VARS.
  - Scan "# Instructions" text for `EXECUTE_PROMPT($PATH_STRING)`.
  - For each `$PATH_STRING` found:
    - Substitute any variables from `$VARS`.
    - If the resulting path is static (no more '$'), treat it as a dynamic include.
    - If the path is dynamic, add a WARN: "EXECUTE_PROMPT path is dynamic and cannot be linted."
    - If the path is static, attempt to read it. If missing, add ERROR.
    - For each readable static path, execute LINT_FILE(resolved_path, $STRICT, $VISITED, $FINDINGS).
- Validate "# Prompt Functions" section (if present):
  - Each function starts with "## NAME" and must include "### Goal" and "### Instructions". If missing, add ERROR for that function.
  - If a library module contains "# Instructions", add ERROR.
- Validate "# Command Mapping" section (if present):
  - Each command line should follow "- <command> - <instructions>" format. If not, add ERROR.
  - Command tokens must be unique. If duplicates, add ERROR.
- Validate command references (heuristic):
  - Scan "# Instructions" text for explicit command tokens in backticks that are paired with verbs like "type", "enter", "run", or "command".
  - If any referenced command token is missing from "# Command Mapping", add WARN.
  - If a command token exists in mapping but is never referenced in instructions, add WARN.
- Strict checks (only if $STRICT == true):
  - Ensure "# Instructions" include the required pre/post output lines per LPP spec ("Starting instructions for ..." and "Finished instructions for ..."). If missing, add WARN.
  - Flag any top-level section other than: Goal, Instructions, Prompt Functions, Include Instructions From, Referenced Instructions, Command Mapping. Add WARN for unknown top-level sections.
