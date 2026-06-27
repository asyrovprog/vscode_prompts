---
name: topic
user-invocable: false
description: >-
    Choose the next learning topic for the ulearn loop. Proposes candidate next
    topics derived from progression in learnlog.md and asks the user to pick. Use
    when starting a new learning iteration or when the user asks "what should I
    learn next".
---

# topic — choose the next learning topic

Pick the next `$TOPIC` for the learning loop. The `ulearn` coach records the result;
this skill only selects the topic.

## Instructions

1. Determine `$ID` (iteration): if the coach passed one, use it; otherwise read
   `learnlog.md` and use the next iteration number (or `01` if none).
2. Print the step context (goal + available commands), separated by lines of `-`.
3. If `learnlog.md` exists:
   - Propose **4–5** candidate next topics derived from the progression of prior
     steps. Stay close to mastered fundamentals — do not jump ahead.
   - Ask the user to choose; set `$TOPIC` to their choice (accept a free-form topic
     too).
4. If `learnlog.md` does not exist, ask the user for an initial `$TOPIC`.
5. Return `$TOPIC` (and `$ID`) to the coach.

## Commands

- `next` — proceed to the **learn** step for `$TOPIC`.
