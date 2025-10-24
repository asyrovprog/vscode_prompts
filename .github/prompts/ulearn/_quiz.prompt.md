---
mode: agent
model: GPT-5 (copilot)
description: Learning `quiz` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- Conforms to LPP_SPEC v1.0.2 (.github/prompts/LPP_SPEC.md) -->

# Goal
This function helps the user to evaluate knowledge of provided so far materials on the $TOPIC. 

# Include Instructions From
- $DIR/_shared.prompt.md

# Instructions
- If $DIR is not set then Return HALT (dispatcher must define base directory)
- Execute DESCRIBE_STEP()
- Generate 5-10 quiz questions for $TOPIC:
    - Use Y/N, A–D, or multi-select `M:` formats
    - Accept compact answers; score immediately
    - If score <80% then offer `explain` recap + targeted retry with fresh variants
- Write quiz to `learn/quizNN.md` and correct answers + reasoning to `learn/quizNN_answers.md`.
- Execute EXECUTE_WRITE_LOG(...) and mark quiz completed with results.
- Output contents of `learn/quizNN.md`.

# Command Mapping
- prev - return to learn module ($DIR/_learn.prompt.md)
- next - proceed to lab module ($DIR/_lab.prompt.md) after marking quiz completed
- explain - provide explanations and rationales for incorrect answers