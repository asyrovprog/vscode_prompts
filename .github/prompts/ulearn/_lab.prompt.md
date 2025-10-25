---
mode: agent
description: Learning `lab` workflow step function
model: GPT-5 (copilot)
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- Conforms to LPP_SPEC v1.0.2 (.github/prompts/LPP_SPEC.md) -->

# Goal
- This function helps the user to learn provided $TOPIC through leetcode style programming assignment (lab)

# Referenced Instructions
- $DIR/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP()
- If $DIR is not set then Return HALT (dispatcher must define base directory)
- Ensure `lab/` exists; if not then ask user what language should be used (C# or Python) then create directory.
- Verify if any labs are marked unfinished in `learnlog.md`; if so ask user to complete those first and Return HALT.
- Otherwise:
    - Propose 2-3 high level lab concept options and ask user to choose.
    - Execute IMPLEMENT_LAB(); if it returns FAILURE (too complex or poorly designed) propose alternative concepts and retry once; on second FAILURE Return FAILURE.
- Execute EXECUTE_WRITE_LOG(...) to create lab log record with $TOPIC and mark lab started.

# Command Mapping
- prev - go back to quiz module ($DIR/_quiz.prompt.md)
- check - run lab tests and report success/failure. DO NOT fix code if tests fail
- next - mark lab completed then go to topic selection ($DIR/_topic.prompt.md)
- explain - ask for where help is needed and provide hints tied to TODOs

# Prompt Functions

## IMPLEMENT_LAB()

### Goal
- Design a solvable lab for $TOPIC; privately implement and pass all tests, then publish a stubbed version whose tests fail and guide the learner via TODO IDs.

### Constraints
- Learner effort ≤ ~30 minutes (reading + coding).
- Learner-added code ≤ 100 LOC total; any single TODO body ≤ 60 LOC.
- Provide ≥2 TODOs (N1, N2; optional N3) with at least one non-trivial function.
- Public shipped code must contain stubs with `[YOUR CODE GOES HERE]` and NotImplemented exceptions (or raises in Python).
- Tests (public form) must FAIL and mention both TODO ID and corresponding README section title.
- Scope must remain strictly on $TOPIC (avoid unrelated parsing/I/O/etc.).
- Private working solution must appear only inside a collapsed section in `REF.md`; never remain in task file when shipped.
- "Never auto-complete TODOs" applies only after private verification (public form); private completion is required.
- Up to 3 private attempts to achieve all PASSing tests; otherwise delete iteration folder and return FAILURE.
- Tests should be deterministic (seed any randomness).

### Instructions
- Concept Design:
    - Generate 2–3 concise concept options tied directly to $TOPIC.
    - Select the simplest viable concept that exercises core ideas.
- Plan TODOs:
    - Define IDs N1, N2 (optional N3) each with a short title and learning objective.
- Scaffold:
    - Create `lab/iterNN/` (increment NN).
    - Add `README.md` with a section per TODO (title format: "TODO N1 – <title>").
    - Add task file (`Task.cs` for C# or `task.py` for Python) containing stub blocks:
        ```csharp
        // TODO[N1]: <objective>
        // [YOUR CODE GOES HERE]
        throw new NotImplementedException("TODO[N1]");
        ```
        ```python
        # TODO[N1]: <objective>
        # [YOUR CODE GOES HERE]
        raise NotImplementedError("TODO[N1]")
        ```
    - Add test harness (`Program.cs` or `run.py`) printing PASS/FAIL per test.
    - Add `REF.md` with hint sections per TODO (no solution yet).
    - Add tests/asserts that will FAIL while stubs are present.
- Private Implementation (green phase):
    - Fill each TODO body with a working solution.
    - Run tests until all PASS (≤3 attempts). An attempt = modify solution + full test run.
    - If still failing after 3 attempts: delete `lab/iterNN/` and RETURN FAILURE.
- Public Conversion (red phase):
    - Revert solution bodies back to stubs (`[YOUR CODE GOES HERE]` + NotImplemented/raise).
    - Ensure tests now FAIL with messages like: "TODO[N1] not satisfied – see README section 'TODO N1 – <title>'".
    - Do not relax test coverage/assertions.
- Reference Solution Storage:
    - Append collapsed solution block in `REF.md` below hints:
        ```
        <details><summary>Reference Solution (open after completion)</summary>
        // or python full passing code
        </details>
        ```
- Validation:
    - Verify required files exist: `README.md`, `REF.md`, task file, harness.
    - Public test run: failures contain both TODO ID and matching README section title.
    - Stubs contain `[YOUR CODE GOES HERE]` markers and correct NotImplemented throws/raises.
    - If any validation fails RETURN FAILURE.
- Outcome:
    - RETURN SUCCESS on validated stubbed public lab; else FAILURE.