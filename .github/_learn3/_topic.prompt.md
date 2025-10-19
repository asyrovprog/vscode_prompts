---
mode: agent
model: GPT-5-Codex (Preview) (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
This function help user the user to choose next learning topic based on `learnlog.md`.

# Include Instructions From
- .github/agents.md
- .github/_learn3/shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- If no `learnlog.md`, then ask the user for $TOPIC to study.
- If `learnlog.md` was found, come up with up to 5 suggestions on next related (top-down style), deeper learning topics
- Set selected by the user topic to $TOPIC.
- Record the chosen $TOPIC checkpoint in `learnlog.md`.
- Response command handling:
     - `next` - EXECUTE_PROMPT(.github/_learn3/_learn.prompt.md)