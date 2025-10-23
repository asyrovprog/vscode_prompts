---
mode: agent
model: GPT-5 (copilot)
description: Learning iteration main entry point
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
Learning iteration main entry point

# Instructions
- Check if `learnlog.md` is provided, if yes then do EXECUTE_PROMPT for:
    - .github/prompts/ulearn/_lab.prompt.md if:
        - last recorded checkpoint is lab and this lab is not completed
        - last checkpoint is quiz and this quiz is completed
    - .github/prompts/ulearn/_quiz.prompt.md if:
        - last recorder checkpoint is learn and it is completed
        - last recorder checkpoint is quiz and it is not completed
    - .github/prompts/ulearn/_learn.prompt.md if:
        - last recorded checkpoint is learn and it is not completed
        - last recorder checkpoint is topic
    - .github/prompts/ulearn/_topic.prompt.md otherwise
- If no `learnlog.md` then ask the user to provide topic ($TOPIC) to learn and EXECUTE_PROMPT(.github/prompts/ulearn/_learn.prompt.md)