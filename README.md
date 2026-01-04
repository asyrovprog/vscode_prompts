# vscode_prompts

A GitHub Copilot Chat **learning prompt** for VS Code.

It uses an iterative top-down loop:
**select a topic → learn → quiz → lab → repeat**.

## Who it’s for

Software developers who want a lightweight, structured way to learn a topic and then immediately practice it.

## How it works

- **Select a topic:** either provide your own topic or pick from suggestions.
- **Learn (~20 minutes):** Copilot generates a compact, top-down set of learning materials.
- **Quiz (6–10 questions):** Copilot checks understanding and scores you.
- **Lab (~25–30 minutes):** Copilot generates a LeetCode-style programming assignment to practice the same topic.
- **Repeat:** Pick the next topic (to go deeper) and run another iteration.

The prompt behaves like a wizard: each step prints available commands. When you finish a step (such as read learning materials or answered quiz questions), type `next` to mark it complete and move on.

The prompt maintains a `learnlog.md` so you can stop anytime and resume from where you left off.

## Under the hood

This is implemented as a small set of VS Code “prompt files” under `.github/prompts`. The `/ulearn ...` command runs a dispatcher prompt that looks at `learnlog.md` to determine which step you’re currently in (topic, learn, quiz, or lab) and then routes execution to the corresponding step prompt.

Each step prompt generates (or reuses) files in your workspace (for example, notes/quizzes under `learn/` and coding exercises under `lab/`) and updates `learnlog.md` as you progress. That’s what makes the flow resumable: if you close VS Code and come back later, rerunning simply as `/ulearn` can continue from the last recorded step.

## Recommended VS Code / Copilot setup

- Enable the tool `ms-vscode.vscode-websearchforcopilot/websearch` so learning materials can be kept up to date and include external references.
  - Reference (setup + API key + settings): <https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-websearchforcopilot> (see `websearch.useSearchResultsDirectly`).
- For faster generation, you can configure VS Code/Copilot auto-approve for agent actions (edits/commands). Only do this if you trust the prompt.
  - Reference: <https://code.visualstudio.com/docs/copilot/chat/chat-tools> (see “Automatically approve terminal commands” and the tool approval sections). Useful settings include `chat.tools.terminal.autoApprove` (terminal commands) and `chat.tools.urls.autoApprove` (URL/web tools, including web search).

## Setup

1. Clone this repo:

```bash
git clone https://github.com/asyrovprog/vscode_prompts.git
cd vscode_prompts
```

1. Create a separate folder where your learning artifacts will live:

```bash
mkdir -p ~/learning
```

1. Symlink this repo’s `.github` into that learning folder:

```bash
ln -s "$PWD/.github" ~/learning/.github
```

1. Open the learning folder in VS Code:

```bash
code ~/learning
```

## Run

Open Copilot Chat in VS Code and start the workflow:

`/ulearn I want to learn <topic>`

Example:

`/ulearn I want to learn A* search`
