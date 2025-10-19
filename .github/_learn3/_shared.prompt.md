---
mode: agent
model: GPT-5-Codex (Preview) (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Common Prompt Functions

## DESCRIBE_STEP Prompt Function

### Instructions
- Output line separator, such as multiple `-` characters
- Output goal of this current workflow step
- Output line separator, such as multiple `-` characters
- Output current $TOPIC, such as `TOPIC: A* algorithm`, if topic is known.
- Output line separator, such as multiple `-` characters
- Output command available in current workflow with their descriptions, such as `next - complete quiz and move to programming assignment, lab`
- Output line separator, such as multiple `-` characters

