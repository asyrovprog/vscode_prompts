---
name: quiz
user-invocable: false
description: >-
    Create and run a short quiz (6-10 questions) to check understanding of a topic
    in the ulearn loop, then score the user's answers. Use when the user wants to
    test or quiz their knowledge of a topic they have studied.
---

# quiz — create, run, and score a quiz

Check the user's understanding of `$TOPIC` with a compact quiz, then score it and
return `$SCORE`.

## Instructions

1. Determine `$TOPIC` and `$ID`: use values from the coach; otherwise read
   `learnlog.md` to infer them.
2. Print the step context (goal + available commands), separated by lines of `-`.
3. Read `learnlog.md` and check the last `quiz` step status. If it is `started` and
   `learn/quiz<ID>.md` exists, tell the user the quiz already exists, display it, and
   ask them to complete it. Otherwise run **CREATE_AND_RUN_QUIZ**.

## CREATE_AND_RUN_QUIZ

### Instructions
1. Generate **6–10** questions for `$TOPIC`. Use compact formats: Y/N, A–D, or
   multi-select `M:`.
2. Write the quiz to `learn/quiz<ID>.md`, and write the correct answers + reasoning
   plus the final score to `learn/quiz<ID>_answers.md`.
3. Output a copy of the quiz to chat so the user need not open the file.
4. Tell the coach the quiz is `started` (so it can record it).
5. Prompt the user to reply with compact answers (e.g. `1:A,2:BC,3:Y`) and wait.
6. When the user answers, validate and score immediately; set `$SCORE`.
7. If `$SCORE` < 70%, state the quiz failed and offer `explain` (recap of wrong
   answers) plus a targeted retry with fresh variants.
8. Otherwise return `$SCORE` to the coach as `completed`.

## Commands

- `next` — record `$SCORE`, mark quiz completed, and proceed to the **lab** step.
- `prev` — go back to the **learn** step.
- `explain` — provide explanations and rationale for incorrect answers.
