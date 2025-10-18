---
mode: agent
model: Claude Sonnet 4.5 (Preview) (copilot)
description:  Learning iteration `topic` workflow step
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning iteration `topic` workflow step

Instructions:
- Include prompt: .github/agents.md.
- You are semantic function helping the user to select next learning topic based on `learnlog.md`. 
- If no `learnlog.md`, then ask the user for $TOPIC.
- If `learnlog.md` was found, come up with suggestions on next related (top-down style), deeper learning topic and set it to $TOPIC.
- Record the chosen $TOPIC checkpoint in `learnlog.md`.
- Response command handling:
     - `next` - EXECUTE_PROMPT(.github/_learn3/_learn.prompt.md)

END OF PROMPT INSTRUCTIONS     