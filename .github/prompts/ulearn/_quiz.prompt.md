---
mode: agent
model: Claude Sonnet 4.5 (copilot)
description: Learning `quiz` workflow step function
tools: ['search/codebase', 'new', 'edit/editFiles', 'runCommands', 'fetch', 'ms-vscode.vscode-websearchforcopilot/websearch']
---

# Initialization
Load, read and understand .github/prompts/lpp_spec.md

# Goal
This function helps the user to evaluate knowledge of provided so far materials on the $TOPIC. 

# Include Instructions From
- $DIR/_shared.prompt.md

# Instructions
- If $DIR is not set then Execute EXECUTE_HALT() (dispatcher must define base directory)
- Execute DESCRIBE_STEP()
- Read `learnlog.md` and check last quiz step status
- If last quiz step status is "started":
    - Check if corresponding learn/quizNN.md file exists
    - If file exists:
        - Display message: "Quiz already exist at learn/quizNN.md. Please complete." and also display quiz content to user.
    - Otherwise if file does not exist:
        - Execute CREATE_AND_RUN_QUIZ()
- Otherwise:
    - Execute CREATE_AND_RUN_QUIZ()

# Command Mapping
- prev - EXECUTE_PROMPT($DIR/_learn.prompt.md)
- next - Execute EXECUTE_WRITE_LOG($SCORE, completed) and then EXECUTE_PROMPT($DIR/_lab.prompt.md)
- explain - provide explanations and rationales for incorrect answers

# Prompt Functions

## CREATE_AND_RUN_QUIZ()

### Goal

Generate quiz for learning $TOPIC get user to complete it with compact answers and score user answers

### Instructions
- Generate 6-10 quiz questions for $TOPIC:
- Write quiz to `learn/quizNN.md` also write correct answers + reasoning plus the user score to `learn/quizNN_answers.md`. Use Y/N, A–D, or multi-select `M:` formats
- Also output copy of generated quiz to chat, so user does not need to open `learn/quizNN.md`. 
- Invoke EXECUTE_WRITE_LOG($SCORE, started) to mark quiz NN started.
- Prompt and wait for the user to reply with compact answers (e.g., `1:A,2:BC,3:Y`)
- After the user submits answers, validate and score them immediately. Assign score to $SCORE.
- If score <70% then state that quiz failed and offer `explain` recap + targeted retry with fresh variants
- Otherwise EXECUTE_WRITE_LOG($SCORE, completed).