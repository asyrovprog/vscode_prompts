# Ulearn Prompt

VS Code GitHub Copilot Chat slash prompt I use for an iterative, top-down study loop:

**topic -> learn -> quiz -> lab -> repeat**.

## Who it's for

Software developers who want a lightweight, hands-on, structured way to learn a topic and practice it, as opposed to passive learning from videos and docs. I built it for myself and share it for anyone who wants the same kind of loop.

## Why it helps

What I like about it:

- **Focused loop:** time-boxed learning, quizzing, and practice keep momentum.
- **Resumable:** progress is tracked in `learnlog.md`.
- **Artifacts:** notes/quizzes under `learn/`, exercises under `lab/`.
- **Adaptable:** adjust depth, difficulty, or format at any step.

## Prerequisites

- VS Code with GitHub Copilot Chat enabled.
- Prompt files support in VS Code (the `.github/prompts` feature).

## How this learning works

The loop is simple:

- **Select a subtopic:** based on the learning log.
- **Learn (~20 minutes):** Copilot generates a compact, top-down set of learning materials.
- **Quiz (6-10 questions):** Copilot checks understanding and scores you.
- **Lab (~25-30 minutes):** Copilot generates a LeetCode-style programming assignment to practice the same topic.
- **Repeat:** pick the next topic and run another iteration.

Each step prints available commands. When you finish a step (such as reading learning materials or answering quiz questions), type `next` to mark it complete and move on. You can stop anytime and resume from `learnlog.md`.

## Quickstart

This is how I run it:

macOS/Linux:

```bash
git clone https://github.com/asyrovprog/vscode_prompts.git
cd vscode_prompts
mkdir -p ~/learning
ln -s "$PWD/.github" ~/learning/.github
code ~/learning
```

Windows (PowerShell, Developer Mode enabled):

```powershell
git clone https://github.com/asyrovprog/vscode_prompts.git
cd vscode_prompts
mkdir "$HOME\\learning"
New-Item -ItemType SymbolicLink -Path "$HOME\\learning\\.github" -Target "$PWD\\.github"
code "$HOME\\learning"
```

Then in Copilot Chat run `/ulearn I want to learn <topic>`. If `learnlog.md` already exists, run `/ulearn` to resume.

Example:

`/ulearn I want to learn Semantic Kernel`

## Setup notes

- I keep a separate workspace folder (for example, `~/learning`) so generated `learn/`, `lab/`, and `learnlog.md` stay out of the prompt repo.
- Symlink this repo's `.github` into that folder so VS Code picks up the prompts.
- On Windows, use WSL or enable Developer Mode to create symlinks.

## Under the hood

Prompts live under `.github/prompts`. The `/ulearn` command dispatches to step prompts based on `learnlog.md`.
Each step writes or reuses files in your workspace and updates `learnlog.md`, which makes the flow resumable.

## Author notes

Copilot can be chatty and interrupt the flow, so I sometimes use YOLO (You Only Live Once) mode with 5 seconds auto-approve. Caution: auto-approve is risky; use it only for trusted repos and disable it when you're done.

Useful settings: `chat.tools.terminal.autoApprove`, `chat.tools.urls.autoApprove`. See "Automatically approve terminal commands": <https://code.visualstudio.com/docs/copilot/chat/chat-tools>.

I also enable the web search tool so learning materials can include external references: <https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-websearchforcopilot> (see `websearch.useSearchResultsDirectly`).

## References

- https://code.visualstudio.com/docs/copilot/chat/chat-tools
- https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-websearchforcopilot
