---
mode: agent
model: GPT-5 (copilot)
description: Learning `topic` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- Conforms to LPP_SPEC v1.0.1 (.github/prompts/LPP_SPEC.md) -->

# Goal
This function help user the user to choose next learning topic based on `learnlog.md`.

# Include Instructions From
- .github/prompts/ulearn/_shared.prompt.md

# Instructions
- Execute DESCRIBE_STEP prompt function
- If $DIR is not set then HALT (dispatcher must define base directory)
- if `learnlog.md` present, come up with 4-5 suggestions for next topic. Use top-down approach to select next topic. Do not rush into advanced topic till basics are solid and mastered with quizzes and labs. Next topic should be continuation, deepening, refinement of already learning material without rush. Set selected by the user topic to $TOPIC. 
- Otherwise if `learnlog.md` is not present ask user for $TOPIC. 
- Run EXECUTE_WRITE_LOG() and this learning step completed.

# Command Mapping
- next - proceed to learn module ($DIR/_learn.prompt.md)