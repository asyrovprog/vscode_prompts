---
mode: agent
model: GPT-5 (copilot)
description: Learning `quiz` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
This function helps the user to evaluate knowledge of provided so far materials on the $TOPIC. 

# Include Instructions From
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- Come up with 5-10 quiz questions for the user on the $TOPIC:
    - Use Y/N, A–D, or multi-select `M:`
    - Accept compact answers; score immediately
    - If score <80%: offer `explain` recap + targeted retry (fresh variants)  
- Write copy of quiz into `learn/quizNN.md`, so the user can review later. Write correct answers into `learn/quizNN_answers.md` with reasoning for each answer.
- Run EXECUTE_WRITE_LOG() and mark quiz completed with quiz results
- Output `learn/quizNN.md`.
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/prompts/ulearn/_learn.prompt.md)
     - `next` - update checkpoint including quiz questions and answers and then EXECUTE_PROMPT(.github/prompts/ulearn/_lab.prompt.md)
     - `explain` - explain to the user incorrectly answered quiz questions