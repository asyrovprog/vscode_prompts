---
name: learn
user-invocable: false
description: >-
    Generate compact, top-down learning materials for a topic in the ulearn loop —
    intuitions, visualizations, examples, mermaid diagrams, and a few high-quality
    web references — scoped to about 20 minutes. Use when the user wants to learn or
    study a topic.
---

# learn — generate learning materials

Produce fun, top-down learning materials for `$TOPIC`, scoped to ~20 minutes, and
save a copy the user can review later.

## Instructions

1. Determine `$TOPIC` and `$ID`: use values passed by the coach; otherwise read
   `learnlog.md` to infer the current topic and iteration (ask the user if `$TOPIC`
   is unknown).
2. Print the step context (goal + available commands), separated by lines of `-`.
3. Read `learnlog.md` and check the last `learn` step status. If it is `started`
   and `learn/learn<ID>.md` already exists, tell the user the materials already
   exist at that path and ask them to review — do not regenerate.
4. Otherwise, run **CREATE_LEARNING_MATERIALS**.

## CREATE_LEARNING_MATERIALS

### Constraints
- Scope to ~20 minutes of study; use a **top-down**, master-the-basics approach —
  don't rush into advanced material before fundamentals are solid.
- Make materials current; note any obsolete features.
- Make them fun: intuitions, visualizations, examples, and mermaid diagrams.
- Do not copy-paste materials from previous iterations (consult `learnlog.md`).

### Instructions
1. Generate the learning materials for `$TOPIC` and write a copy to
   `learn/learn<ID>.md` in the current working directory.
2. Use your web-search tool to find a few high-quality references (e.g. videos,
   articles) and include them.
3. Validate the document is high quality and well formatted.
4. Tell the coach this step is `started` (so it can record it) with a brief summary.

## Commands

- `next` — mark learn completed and proceed to the **quiz** step.
- `prev` — go back to the **topic** step.
