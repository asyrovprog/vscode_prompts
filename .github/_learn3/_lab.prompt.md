---
mode: agent
description: Learning `lab` workflow step function
model: GPT-5-Codex (Preview) (copilot)
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Learning iteration `lab` workflow step

This function helps the user to learn provided $TOPIC through leetcode style programming assignment (lab).

Instructions:
- Include prompt: .github/agents.md.
- Output $TOPIC and available commands (see below).
- Ensure `lab/` exists; if not then what language should be used (C# or Python).
- Create an assignment in `lab/iterNN/` that **cannot be complete** without user's edits. Prefer assignment to be a single command line project which outputs success or failure for each test. Also create README.md in the folder with requirements to complete lab assignment.
- Scaffolding Rules:
    - Provide **≥2 TODOs** and at least one **[YOUR CODE GOES HERE]** region.
    - Ship **failing tests first** (red → green). Fail messages must reference TODO IDs and point to README.md sections.
    - **Never** auto-complete TODOs.  
    - Language specifics:
        - C#: create `lab/iterNN/Task.cs` with markers like:
            // TODO[N1]: implement stable hashing using X algorithm  
            public static string StableId(string input)  
            {  
                // [YOUR CODE GOES HERE]  
                throw new NotImplementedException("TODO[N1]");  
            }
        - Python: create `lab/iterNN/task.py` with functions and `# TODO[N1]` markers; include doctests or `assert` checks that fail initially.
- Create checkpoint in `learnlog.md`, so learning can be resumed from this step and $TOPIC
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/_learn3/_quiz.prompt.md)
     - `check` - execute tests to verify completion of the tab and output success or failure result.
     - `next` - update `learnlog.md` with lab results and EXECUTE_PROMPT(.github/_learn3/_topic.prompt.md)
     - `explain` - ask the user where help is needed and provide hints on the lab tasks