---
name: lpp-lint
user-invocable: false
description: >-
    Validate a Literal Prompt Programming (LPP) module against the LPP protocol and
    report errors and warnings. Use when the user asks to lint, validate, or check
    an LPP module, skill, or agent file in this plugin.
---

# lpp-lint — validate an LPP module

Lint an LPP module (a skill `SKILL.md`, an agent file, or a referenced instruction
file) against the LPP protocol and report findings. The checks below are the LPP
protocol in operational form; `lpp_spec.md` at the repo root has the background.

## Instructions

1. If `$PROMPT_PATH` is not set, ask the user for a path to lint and set it.
2. If `$STRICT` is not set, set `$STRICT = false`.
3. Run **LINT_MODULE**(`$PROMPT_PATH`, `$STRICT`).

## LINT_MODULE($PATH, $STRICT)

### Instructions
1. Initialize `$VISITED = []` and `$FINDINGS = []`.
2. Run **LINT_FILE**(`$PATH`, `$STRICT`, `$VISITED`, `$FINDINGS`).
3. Output an "LPP Lint Report": target path; a summary (files checked, error count,
   warning count); then findings ordered by severity (ERROR before WARN), then file
   path, then line number. If there are none, output "No issues found."

## LINT_FILE($PATH, $STRICT, $VISITED, $FINDINGS)

### Instructions
1. If `$PATH` is in `$VISITED`, return. Otherwise add it.
2. Read `$PATH`. If unreadable, add ERROR "File not found or unreadable" and return.
3. **Front matter (host-specific, optional):** LPP itself requires none. If a host
   front-matter block is present (delimited by `---`), it must be well-formed
   (opening and closing `---`); if malformed, add ERROR. Do **not** require any
   specific fields.
4. **Top-level sections** (`# ` lines): capture titles and line numbers.
   - Exactly one `# Goal`. Missing or duplicated → ERROR.
   - If `# Instructions` exists, the module is runnable; more than one → ERROR.
   - If `# Instructions` is absent, the module is a library and must contain
     `# Prompt Functions`; if missing → ERROR.
   - For runnable modules, `# Goal` must precede `# Instructions`; if not → ERROR.
5. **Includes** (`# Include Instructions From` / `# Referenced Instructions`): for
   each `- path` line — if the path contains `$`, add WARN "dynamic include path";
   if static, try to read it (missing → ERROR) and recurse with **LINT_FILE**.
6. **EXECUTE_PROMPT calls:** scan `# Instructions` for `Set $VAR = value` (collect
   into `$VARS`) and `EXECUTE_PROMPT($PATH_STRING)`. Substitute known vars; if still
   dynamic, add WARN; if static, read it (missing → ERROR) and recurse.
7. **Prompt functions** (if present): each `## NAME` must include `### Goal` and
   `### Instructions`; otherwise ERROR for that function. A library module that
   contains `# Instructions` → ERROR.
8. **Command mapping** (if present): each line must match `- <command> - <action>`;
   tokens must be unique; otherwise ERROR.
9. **Command references (heuristic):** if a backticked command token paired with a
   verb like "type/enter/run/command" appears in instructions but is missing from
   `# Command Mapping`, add WARN; if a mapped command is never referenced, add WARN.
10. **Strict checks** (only if `$STRICT == true`): flag any top-level section other
    than Goal, Instructions, Prompt Functions, Include Instructions From,
    Referenced Instructions, Command Mapping, or Initialization as WARN.
