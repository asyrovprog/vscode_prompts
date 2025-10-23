---
mode: agent
model: GPT-5 (copilot)
description: Learning `learn` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
This function helps the user to learn provided $TOPIC. Your task is to come up with top-down learning materials for this $TOPIC which could be learned within ~15 minutes. Prefer top-down approach, look into `learnlog.md` to come up with best set of materials. Material should be fun to learn, include intuitions, visualizations and examples.

# Include Instructions From
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- Provide learning materials for the $TOPIC, and write copy of this learning materials into `learn/learnNN.md`, so the user can review later.
- Execute EXECUTE_WRITE_LOG() instructions, with $TOPIC and brief summary of learning materials so learning can be resumed from this step.
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/prompts/ulearn/_topic.prompt.md)
     - `next` - Update checkpoint and EXECUTE_PROMPT(.github/prompts/ulearn/_quiz.prompt.md)