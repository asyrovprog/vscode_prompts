---
mode: agent
model: GPT-5 (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- File specification (read for semantics): .github/prompts/LPP_SPEC.md (LPP_SPEC_ID: LPP_STABLE) -->

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
- next - proceed to learn module ($DIR/_learn.prompt.md)