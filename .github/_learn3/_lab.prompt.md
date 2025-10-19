---
mode: agent
description: Learning `lab` workflow step function
model: GPT-5-Codex (Preview) (copilot)
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
- This function helps the user to learn provided $TOPIC through leetcode style programming assignment (lab)

# Referenced Instructions
- .github/agents.md
- .github/_learn3/shared.prompt.md

# Lab Design Standards
**Critical:** Before creating any lab, the agent must implement the lab completely and verify all tests pass. This ensures the lab is solvable and well-designed.

## Pre-Lab Validation Checklist
- **Implement the entire lab yourself** without stubs. Write full working code that satisfies all test cases.
- **Run the complete test suite** and confirm all tests pass on the first or second attempt.
- **If you cannot implement it cleanly in 2–3 tries**, the lab is too complex or poorly designed. Reject it and propose a simpler alternative.
- **Only after validation**, replace your working code with `[YOUR CODE GOES HERE]` stubs for the learner.

This prevents wasting learner time on broken or unclear assignments.

# Instructions
- Execute DESCRIBE_STEP prompt function
- Ensure `lab/` exists; if not then what language should be used (C# or Python).
- Verify if all labs are marked in `learnlog.md` as finished and if there is any unfinished ask the user to complete it.
- Otherwise:
    - Create an assignment in `lab/iterNN/` that **cannot be complete** without user's edits. Prefer assignment to be a single command line project which outputs success or failure for each test. Also create README.md in the folder with requirements to complete lab assignment. Also create `REF.md` with detailed hints tied to each TODO.
    - Scaffolding Rules:
        - Provide **≥2 TODOs** and at least one **[YOUR CODE GOES HERE]** region.
        - Ship **failing tests first** (red → green). Fail messages must reference TODO IDs and point to README.md sections.
        - **Never** auto-complete TODOs.
        - Include clear method signatures and parameter documentation in stubs so learner intent is obvious.
        - Language specifics:
            - C#: create `lab/iterNN/Task.cs` with markers like:
                ```csharp
                // TODO[N1]: implement stable hashing using X algorithm  
                /// <param name="input">The string to hash</param>
                /// <returns>A stable hash code for the input</returns>
                public static string StableId(string input)  
                {  
                    // [YOUR CODE GOES HERE]  
                    throw new NotImplementedException("TODO[N1]");  
                }
                ```
            - Python: create `lab/iterNN/task.py` with functions and `# TODO[N1]` markers; include doctests or `assert` checks that fail initially.
- Create checkpoint in `learnlog.md`, so learning can be resumed from this step and $TOPIC
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/_learn3/_quiz.prompt.md)
     - `check` - execute tests to verify completion of the tab and output success or failure result.
     - `next` - update `learnlog.md` with record that the lab was finished and then EXECUTE_PROMPT(.github/_learn3/_topic.prompt.md)
     - `explain` - ask the user where help is needed and provide hints on the lab tasks