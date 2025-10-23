---
mode: agent
description: Learning `lab` workflow step function
model: GPT-5 (copilot)
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---

# Goal
- This function helps the user to learn provided $TOPIC through leetcode style programming assignment (lab)

# Referenced Instructions
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- Ensure `lab/` exists; if not then what language should be used (C# or Python).
- Verify if all labs are marked in `learnlog.md` as finished and if there is any unfinished ask the user to complete it.
- Otherwise:
    - Come up with 2-3 high level ideas for lab and ask user to choose.
    - Execute instructions in IMPLEMENT_LAB(). If it returns FAILURE, this means it was too complex or poorly designed. Come up with different ideas which more likely to work.
- Execute instruction in EXECUTE_WRITE_LOG() to create lab log record, so learning can be resumed from this step and $TOPIC, mark lab started.
- Response command handling:
     - `prev` - EXECUTE_PROMPT(.github/prompts/ulearn/_quiz.prompt.md)
     - `check` - execute tests to verify completion of the tab and output success or failure result.
     - `next` - Run EXECUTE_WRITE_LOG() and mark lab completed, and then EXECUTE_PROMPT(.github/prompts/ulearn/_topic.prompt.md)
     - `explain` - ask the user where help is needed and provide hints on the lab tasks


# IMPLEMENT_LAB() 

## Instructions

- Create an assignment in `lab/iterNN/`. Assignment should be a single command line project which outputs success or failure for each test. Assignment should require not more than ~30 minutes ( including reading instructions) to complete and should be very focused on the $TOPIC. Each `[YOUR CODE GOES HERE]` should require not more than 60 lines of code. Complete assignment should not require more than 100 lines of code. Instructions should be clear and take into account that this is still new subject for learner. Minimize need to code unrelated to the topic functionality (if we topic is C# events, asking to parse json is unrelated.) 
- Create README.md in the folder with requirements to complete lab assignment. 
- Create `REF.md` with detailed hints tied to each TODO.
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
- **Implement yourself** the entire lab yourself without stubs. Write full working code that satisfies all test cases.
- **Run implemented program** and observe all tests succeeded 
- If you cannot implement yourself and verify all tests complete with success within 3 attempts then remove `lab/iterNN` and return FAILURE
- Otherwise:
    - Replace your working code with `[YOUR CODE GOES HERE]` stubs for the learner.
    - Return SUCCESS