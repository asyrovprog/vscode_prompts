---
mode: agent
model: GPT-5-Codex (Preview) (copilot)
description: Learning `learn` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning `learn` workflow step function

Goal:
- This function helps the user to learn provided $TOPIC. Your task is to come up with top-down 
learning materials for this $TOPIC which could be learned within ~15 minutes. Prefer top-down approach, look into `learnlog.md` to come up with best set of materials. Material should be fun to learn, include intuitions, visualizations and examples.

Instructions:
- Include prompt: .github/agents.md.
- Output $TOPIC and available commands (see below).
- Provide learning materials for the $TOPIC, and write copy of this learning materials into `learn/learnNN.md`, so the user can review later.
- Create checkpoint in `learnlog.md`, with $TOPIC and brief summary of learning materials so learning can be resumed from this step.
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/_learn3/_topic.prompt.md)
     - `next` - Update checkpoint and EXECUTE_PROMPT(.github/_learn3/_quiz.prompt.md)