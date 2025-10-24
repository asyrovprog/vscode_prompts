---
mode: agent
model: GPT-5 (copilot)
description: Learning iteration main entry point
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- Conforms to LPP_SPEC v1.0.2 (.github/prompts/LPP_SPEC.md) -->

# Goal
Learning iteration main entry point

# Include Instructions From
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- Set $DIR = .github/prompts/ulearn  // Base directory for ulearn step prompt modules
- Check if `learnlog.md` is provided, if yes then do EXECUTE_PROMPT for:
    - $DIR/_lab.prompt.md if:
        - last recorded checkpoint is lab and this lab is not completed
        - last checkpoint is quiz and this quiz is completed
    - $DIR/_quiz.prompt.md if:
        - last recorder checkpoint is learn and it is completed
        - last recorder checkpoint is quiz and it is not completed
    - $DIR/_learn.prompt.md if:
        - last recorded checkpoint is learn and it is not completed
        - last recorder checkpoint is topic
    - $DIR/_topic.prompt.md otherwise
- If no `learnlog.md` then ask the user to provide topic ($TOPIC) to learn and EXECUTE_PROMPT($DIR/_learn.prompt.md)
    - Return HALT if $TOPIC not provided by user after request.