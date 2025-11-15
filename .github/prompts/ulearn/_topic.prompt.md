---
mode: agent
model: Claude Sonnet 4.5 (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase', 'new', 'edit/editFiles', 'runCommands', 'fetch', 'ms-vscode.vscode-websearchforcopilot/websearch']
---

# Initialization
Load, read and understand .github/prompts/lpp_spec.md

# Goal
This function help user the user to choose next learning topic based on `learnlog.md`.

# Include Instructions From
- $DIR/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP()
- If $DIR is not set then Execute EXECUTE_HALT() (dispatcher must define base directory)
- If `learnlog.md` present then propose 4-5 candidate next topics derived from progression of prior steps (avoid jumping ahead); ask user to choose and set $TOPIC.
- Otherwise ask user to provide initial $TOPIC.
- Execute EXECUTE_WRITE_LOG(...) and mark topic selection completed.

# Command Mapping
- next - EXECUTE_PROMPT($DIR/_learn.prompt.md)