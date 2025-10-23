---
mode: agent
model: GPT-5 (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
This function help user the user to choose next learning topic based on `learnlog.md`.

# Include Instructions From
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- if `learnlog.md` present, come up with up to 5 suggestions on next related deeper, top-down style  topics based on recorder in `learnlog.md`. Set selected by the user topic to $TOPIC. 
- Otherwise if $TOPIC 
- Run EXECUTE_WRITE_LOG() and this learning step completed.
- Response command handling:
     - `next` - EXECUTE_PROMPT(.github/prompts/ulearn/_learn.prompt.md)