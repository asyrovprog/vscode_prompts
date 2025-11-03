---
mode: agent
model: GPT-5 (copilot)
description: Learning `quiz` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Initialization
Load, read and understand .github/prompts/lpp_spec.md

# Goal
This function helps the user to evaluate knowledge of provided so far materials on the $TOPIC. 

# Include Instructions From
- $DIR/_shared.prompt.md

# Instructions
- If $DIR is not set then Execute EXECUTE_HALT() (dispatcher must define base directory)
- Execute DESCRIBE_STEP()
- Generate 5-10 quiz questions for $TOPIC:
    - Use Y/N, A–D, or multi-select `M:` formats
    - Accept compact answers; score immediately
    - If score <70% then offer `explain` recap + targeted retry with fresh variants
- Write quiz to `learn/quizNN.md` and correct answers + reasoning to `learn/quizNN_answers.md`.
- Execute EXECUTE_WRITE_LOG(...) and mark quiz completed with results.
- Output contents of `learn/quizNN.md`.

# Command Mapping
- prev - EXECUTE_PROMPT($DIR/_learn.prompt.md)
- next - EXECUTE_PROMPT($DIR/_lab.prompt.md)
- explain - provide explanations and rationales for incorrect answers