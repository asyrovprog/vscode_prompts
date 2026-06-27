---
name: ucode
user-invocable: false
description: >-
    Help the user understand a part of a codebase by producing structured
    responsibilities and interactions documents (with mermaid diagrams), refined
    interactively. Use when the user wants to understand a file, class, module, or
    component of the current project.
---

# ucode — understand a part of the codebase

Help the user understand a `$SUBJECT` (a file, class, module, or component) by
building two short, refined documents with diagrams.

## Instructions

1. Determine `$SUBJECT`:
   - If the user pointed at an existing `responsibilities.md`, derive `$SUBJECT` and
     `$SUBJECT_ID` from it.
   - Else if the user said what to understand (e.g. a code file), store it as
     `$SUBJECT`.
   - Else ask the user, and store the answer as `$SUBJECT`.
2. Create a short 2–3 word `$SUBJECT_ID` suitable for a directory name. Ensure
   `.local/ucode/$SUBJECT_ID/` exists (create it if needed).
3. **Responsibilities** — if `.local/ucode/$SUBJECT_ID/responsibilities.md` is
   missing:
   - Propose up to 7 responsibilities of `$SUBJECT`, each with: a 0-based Id (0..7),
     a short Name (e.g. "Store data into database"), and a 1–3 sentence description.
   - Refine the list interactively with the user. When they confirm it is final,
     write `responsibilities.md` with each responsibility, its description, and 1–3
     paragraphs on how it is fulfilled. Add mermaid diagrams where helpful.
4. **Interactions** — if `.local/ucode/$SUBJECT_ID/interactions.md` is missing:
   - Propose up to 7 interactions between `$SUBJECT` and other classes/files/parts
     of the system, each with: a 0-based Id (0..7), a short Name (e.g. "DB
     Abstraction Layer"), and a 1–3 sentence description.
   - Refine interactively. When confirmed final, write `interactions.md` with each
     interaction, its description, and 1–3 paragraphs on how it works. Add a mermaid
     diagram to explain the interaction / class hierarchy.

Use whatever code-search and file tools you have; stay tool- and model-agnostic.
