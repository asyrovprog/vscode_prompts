---
mode: agent
description: Learning iteration main entry point
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning iteration main entry point

Instructions:
- include prompt: .github/agents.md
- Check if `learnlog.md` is provided, if yes then do EXECUTE_PROMPT for:
    - .github/_learn3/_lab.prompt.md if last recorded checkpoint in the learning log is lab
    - .github/_learn3/_quiz.prompt.md if last recorded checkpoint in the learning log is quiz
    - .github/_learn3/_learn.prompt.md if last recorded checkpoint in the learning log is learn
    - .github/_learn3/_topic.prompt.md otherwise
- If no checkpoint then EXECUTE_PROMPT(.github/_learn3/_topic.prompt.md)