---
mode: agent
description: Learning iteration main semantic function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning iteration main semantic function

include prompt: agents.md.

Instructions:
- If topic ($TOPIC) is not known then ask the user to provide topic to study and set it to $TOPIC.
- Otherwise if topic is known EXECUTE_PROMPT(_learn3/_learn.prompt.md). 
