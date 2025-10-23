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
    - .github/prompts/ulearn/_lab.prompt.md if last recorded uncompleted checkpoint in the learning log is lab
    - .github/prompts/ulearn/_quiz.prompt.md if last recorded uncompleted checkpoint in the learning log is quiz
    - .github/prompts/ulearn/_learn.prompt.md if last recorded uncompleted checkpoint in the learning log is learn
    - .github/prompts/ulearn/_topic.prompt.md with latest learning topic from `learnlog.md`
- If no `learnlog.md` then ask the user to provide topic ($TOPIC) to learn and EXECUTE_PROMPT(.github/prompts/ulearn/_learn.prompt.md)