---
mode: agent
model: Claude Sonnet 4.5 (copilot)
description: Learning `learn` workflow step function
tools: ['search/codebase', 'new', 'edit/editFiles', 'runCommands', 'fetch', 'ms-vscode.vscode-websearchforcopilot/websearch']
---

# Initialization
Load, read and understand .github/prompts/lpp_spec.md

# Goal

This function helps the user to learn provided $TOPIC. Your task is to come up with top-down learning materials for this $TOPIC which could be learned within ~20 minutes. Use top-down approach, look into `learnlog.md` to come up with best set of materials. Material should be fun to learn, include intuitions, visualizations, examples and mermaid diagrams. Do not rush into advanced topics till fundamentals and basics are solid (i.e. master of basics approach). Also search web for few relevant for the $TOPIC and high-quality resources, such as videos and medium articles.

# Include Instructions From
- $DIR/_shared.prompt.md

# Instructions
- If $DIR is not set then Execute EXECUTE_HALT()
- Execute DESCRIBE_STEP()
- Read `learnlog.md` and check last (ordered by date and Id) learn step status
- If last learn step status is "started":
    - Check if corresponding learn/learnNN.md file exists
    - If file exists:
        - Display message: "Learning materials already exist at learn/learnNN.md. Please review them."
    - Otherwise if file does not exist:
        - Execute CREATE_LEARNING_MATERIALS()
- Otherwise:
    - Execute CREATE_LEARNING_MATERIALS()

# Command Mapping
- prev - EXECUTE_PROMPT($DIR/_topic.prompt.md)
- next - Update status of learning iteration to completed and then EXECUTE_PROMPT($DIR/_quiz.prompt.md)

# Prompt Functions

## CREATE_LEARNING_MATERIALS()

### Goal
Generate and persist learning materials for the current topic, then halt execution to wait for user to complete studying.

### Instructions
- Provide learning materials for the $TOPIC, and write copy of this learning materials into `learn/learnNN.md`, so the user can review later.
- Execute EXECUTE_WRITE_LOG(...) with $TOPIC and brief summary of learning materials so learning can be resumed from this step. Mark this step as started.