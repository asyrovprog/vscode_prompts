---
name: lab
user-invocable: false
description: >-
    Generate a LeetCode-style programming lab to practice a topic from the ulearn
    loop, with TODO stubs and failing tests that guide the learner, plus a hidden
    reference solution. Use when the user wants a hands-on coding exercise or
    practice assignment for a topic.
---

# lab — generate a practice lab

Create a solvable, LeetCode-style lab for `$TOPIC`: privately implement and pass all
tests, then ship a stubbed version whose tests fail and guide the learner via TODO
IDs.

## Instructions

1. Determine `$TOPIC` and `$ID`: use values from the coach; otherwise read
   `learnlog.md` to infer them.
2. Print the step context (goal + available commands), separated by lines of `-`.
3. Ensure `lab/` exists; if not, ask the user which programming language to use,
   then create it.
4. If any lab is marked unfinished in `learnlog.md`, ask the user to complete it
   first and halt.
5. Otherwise: propose 2–3 high-level lab concept options and ask the user to choose,
   then run **IMPLEMENT_LAB**. If it returns FAILURE, propose alternative concepts
   and retry once; on a second FAILURE, halt.
6. Tell the coach the lab is `started` with its path, then ask the user to complete
   lab `$LAB_ID`.

## IMPLEMENT_LAB

### Constraints
- Learner effort ≤ ~25 minutes (reading + coding); learner-added code ≤ 90 LOC
  total; any single TODO body ≤ 60 LOC.
- Provide ≥ 2 TODOs (`N1`, `N2`; optional `N3`), at least one non-trivial.
- Shipped public code contains stubs with `[YOUR CODE GOES HERE]` and a
  NotImplemented throw/raise.
- Public tests must FAIL and mention both the TODO ID and the matching README
  section title.
- Stay strictly on `$TOPIC`. Tests must be deterministic (seed randomness) and fully
  automated (no user input).
- The private working solution lives only inside a collapsed section in `REF.md`.

### Instructions
1. **Design:** generate 2–3 concise concepts tied to `$TOPIC`; pick the simplest
   viable one that exercises the core ideas.
2. **Plan TODOs:** define `N1`, `N2` (optional `N3`), each with a short title and
   learning objective.
3. **Scaffold** `lab/iter<ID>/` (set `$LAB_ID` = `<ID>`):
   - `README.md` with one section per TODO (title `TODO N1 – <title>`).
   - A task file in the chosen language (e.g. `Task.cs`, `task.py`) with stub blocks
     containing the TODO id, a `[YOUR CODE GOES HERE]` marker, the copied
     instructions, and a NotImplemented throw/raise.
   - A test harness in the chosen language (e.g. `Program.cs`, `run.py`) that prints
     PASS/FAIL per test.
   - `REF.md` with hint sections per TODO (no solution yet).
   - Tests/asserts that FAIL while stubs are present.
4. **Private implementation (green):** fill each TODO with a working solution; run
   tests until all PASS (≤ 3 attempts). If still failing, delete `lab/iter<ID>/` and
   RETURN FAILURE.
5. **Public conversion (red):** revert solution bodies to stubs; ensure tests now
   FAIL with messages naming the TODO ID and README section. Do not relax coverage.
6. **Reference:** append the full passing solution in a collapsed
   `<details>` block in `REF.md`.
7. **Registration:** wire the lab so it can be run and debugged (for example, in C#
   add the project to the root `*.sln` and update `launch.json`).
8. **Validation:** verify `README.md`, `REF.md`, task file, and harness exist;
   public tests fail with TODO ID + README title; stubs contain the markers and
   correct NotImplemented throws/raises. If anything fails, RETURN FAILURE; else
   RETURN SUCCESS.

## Commands

- `next` — proceed to the **topic** step (next iteration).
- `prev` — go back to the **quiz** step.
- `check` — run the lab tests and report success/failure only; do **not** fix any
  compilation, runtime, or test failure.
- `explain` — ask where help is needed and give hints tied to the TODO IDs.
