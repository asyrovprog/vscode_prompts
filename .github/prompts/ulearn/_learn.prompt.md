---
mode: agent
model: GPT-5 (copilot)
description: Learning `learn` workflow step function
tools: ['search/codebase','search','new','edit/editFiles','runCommands','runTasks','problems','changes','vscodeAPI','openSimpleBrowser','fetch','githubRepo','extensions']
---
<!-- File specification (read for semantics): .github/prompts/LPP_SPEC.md (LPP_SPEC_ID: LPP_STABLE) -->

# Goal

This function helps the user to learn provided $TOPIC. Your task is to come up with top-down learning materials for this $TOPIC which could be learned within ~20 minutes. Use top-down approach, look into `learnlog.md` to come up with best set of materials. Material should be fun to learn, include intuitions, visualizations, examples and mermaid diagrams. Do not rush into advanced topics till fundamentals and basics are solid (i.e. master of basics approach).

# Include Instructions From
- $DIR/_shared.prompt.md

# Instructions
- If $DIR is not set then Execute EXECUTE_HALT()
- Execute DESCRIBE_STEP()
- Provide learning materials for the $TOPIC, and write copy of this learning materials into `learn/learnNN.md`, so the user can review later.
- Execute EXECUTE_WRITE_LOG(...) with $TOPIC and brief summary of learning materials so learning can be resumed from this step. Mark this step as started.

# Command Mapping
- prev - load topic selection module ($DIR/_topic.prompt.md)
- next - mark learn completed and load quiz module ($DIR/_quiz.prompt.md)