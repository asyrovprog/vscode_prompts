---
mode: agent
model: GPT-5 (copilot)
description: Learning iteration next semantic function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning iteration next semantic function

include prompt: agents.md.

Instructions:
Your goal is to help the user to select next, related to provided $TOPIC topic to learn as follows:

1. Output $TOPIC that the user.
2. Propose $NEXT_TOPIC to user. $NEXT_TOPIC must be related to topic or be it's in-depth topic. 
3. Do not proceed till user confirms $NEXT_TOPIC.
4. Once $NEXT_TOPIC confirmed set $TOPIC to $NEXT_TOPIC and then EXECUTE_PROMPT(_learn3/_learn.prompt.md)
