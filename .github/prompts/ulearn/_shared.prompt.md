---
mode: agent
model: GPT-5 (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Common Prompt Functions

## DESCRIBE_STEP instructions

- Output line separator, such as multiple `-` characters
- Output goal of this current workflow step
- Output line separator, such as multiple `-` characters
- Output current $TOPIC, such as `TOPIC: A* algorithm`, if topic is known.
- Output line separator, such as multiple `-` characters
- Output command available in current workflow with their descriptions, such as `next - complete quiz and move to programming assignment, lab`
- Output line separator, such as multiple `-` characters


## EXECUTE_HALT() instructions

- Respond to the user that you CANNOT proceed with execution of further instructions from current prompt.
- Stop execution of current prompt and wait for the user response.

## EXECUTE_PROMPT($PROMPT) instructions

- Output that you will follow every instruction verbatim from the $PROMPT file. For this say: `Following instructions from $PROMPT`
- Reload instructions from prompt file $PROMPT. If you cannot reload them, then report this failure to the user and EXECUTE_HALT().
- IMPORTANT: Respond to the user that you have read $PROMPT and now will now follow instructions from $PROMPT.
- Execute instructions from prompt $PROMPT.


## EXECUTE_WRITE_LOG(...) instructions

- if `learnlog.md` does not exist, create it
- Record current step name and it's status. Status can be `started` or `completed`. Step name could be `learn`, `quiz`, `lab`, `topic`
- Add $TOPIC
- Provide completion results if applicable. For instance, for quiz it could be: 80%

- Example `learnlog.md`:
```
10/22/2025
- Step: quiz
- Status: completed
- Result: 80%
- Topic: TCP/IP

10/22/2025
- Step: lab
- Status: completed
- Lab: lab/iter01
```