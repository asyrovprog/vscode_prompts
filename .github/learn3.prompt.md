---
mode: agent
description: Learning iteration main semantic function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning iteration main semantic function

You are semantic function. Your responsibility is to determine topic the user needs to study:
- If topic is not known then ask the user to provide topic to study
- Otherwise if topic is known execute instructions from _learn3/_learn.prompt.md with this topic. 
