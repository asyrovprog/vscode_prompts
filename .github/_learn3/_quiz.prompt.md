---
mode: agent
model: GPT-5-Codex (Preview) (copilot)
description: Learning `quiz` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
This function helps the user to evaluate knowledge of provided so far materials on the $TOPIC. 

# Include Instructions From
- .github/agents.md
- .github/_learn3/shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- Come up with 5-10 quiz questions for the user on the $TOPIC:
    - Use Y/N, A–D, or multi-select `M:`
    - Accept compact answers; score immediately
    - If score <80%: offer `explain` recap + targeted retry (fresh variants)  
- Create checkpoint in `learnlog.md` with summary of quiz results, so learning can be resumed from this step and $TOPIC
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/_learn3/_learn.prompt.md)
     - `next` - update checkpoint including quiz questions and answers and then EXECUTE_PROMPT(.github/_learn3/_lab.prompt.md)
     - `explain` - explain to the user incorrectly answered quiz questions