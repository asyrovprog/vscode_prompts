---
mode: agent
model: GPT-5 (copilot)
description: Learning `quiz` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- Conforms to LPP_SPEC v1.0.1 (.github/prompts/LPP_SPEC.md) -->

# Goal
This function helps the user to evaluate knowledge of provided so far materials on the $TOPIC. 

# Include Instructions From
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- If $DIR is not set then HALT (dispatcher must define base directory)
- Execute DESCRIBE_STEP prompt function
- Come up with 5-10 quiz questions for the user on the $TOPIC:
    - Use Y/N, A–D, or multi-select `M:`
    - Accept compact answers; score immediately
    - If score <80%: offer `explain` recap + targeted retry (fresh variants)  
- Write copy of quiz into `learn/quizNN.md`, so the user can review later. Write correct answers into `learn/quizNN_answers.md` with reasoning for each answer.
- Run EXECUTE_WRITE_LOG() and mark quiz completed with quiz results
- Output `learn/quizNN.md`.

# Command Mapping
- prev - return to learn module ($DIR/_learn.prompt.md)
- next - proceed to lab module ($DIR/_lab.prompt.md) after marking quiz completed
- explain - provide explanations and rationales for incorrect answers