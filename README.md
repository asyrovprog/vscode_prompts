# Learning Loop — a GitHub Copilot CLI plugin

An iterative, top-down study loop you can use in **any** folder from the terminal:

**topic → learn → quiz → lab → repeat**

It's resumable (progress lives in `learnlog.md`), artifacts stay in your working
directory (`learn/`, `lab/`), and the whole thing installs **once** as a Copilot CLI
plugin instead of being symlinked into every project.

## Who it's for

Developers who want a lightweight, hands-on, structured way to learn a topic and
practice it — active learning instead of passively watching videos.

## What's in the plugin

- **`ulearn` coach** (`agents/ulearn.agent.md`) — the orchestrator. It tracks
  progress in `learnlog.md`, decides the next step, and runs it.
- **Step skills** — `topic`, `learn`, `quiz`, `lab`.
- **Utility skills** — `ucode` (understand a codebase), `lpp-lint` (validate an LPP
  module).

See [`AGENTS.md`](./AGENTS.md) for the design and how the pieces compose.

## Prerequisites

- [GitHub Copilot CLI](https://docs.github.com/copilot/how-tos/set-up/install-copilot-cli)
  installed and logged in (`copilot`, then `/login` if needed).

## Install

1. **Clone this repo:**

   ```shell
   git clone https://github.com/asyrovprog/vscode_prompts.git
   cd vscode_prompts
   ```

2. **Install it as a plugin** (point at the repo directory):

   ```shell
   copilot plugin install ./
   ```

3. **Verify it loaded.** Start Copilot CLI and run:

   ```text
   /plugin list          # learning-loop should appear
   /agent                # the "ulearn" coach should be listed
   ```

   The step skills (`topic`, `learn`, `quiz`, `lab`) and utilities (`ucode`,
   `lpp-lint`) are **internal** — they're marked `user-invocable: false`, so the
   coach uses them but you don't select them directly. The only thing you pick is the
   **`ulearn` agent**.

> **Note:** installing from a local path (or a git repo/URL) currently works but is
> **deprecated** — a future CLI release will support only marketplace installs
> (`copilot plugin install <name>@<marketplace>`). Publishing this plugin to a
> marketplace is on the roadmap; until then, the local-path install above is the way
> to use it.
>
> **Updating:** after pulling changes, re-run `copilot plugin install ./` (plugin
> components are cached). Mid-session you can run `/skills reload`.
>
> **Uninstall:** `copilot plugin uninstall learning-loop`.

That's it — the coach and skills are now available in **every** folder. No
per-project setup.

## Walkthrough

Open a terminal in a folder where you want your learning notes to live (a fresh
`~/learning` folder is fine), then start Copilot CLI:

```shell
mkdir ~/learning && cd ~/learning
copilot
```

**1. Start a session.** Pick the coach with `/agent` → `ulearn`, or just ask:

```text
I want to learn Semantic Kernel
```

**2. Learn (~20 min).** The coach runs the `learn` step: it generates compact,
top-down materials (intuitions, diagrams, a few web references) and writes a copy to
`learn/learn01.md`. Read it. When you're done, type:

```text
next
```

**3. Quiz (6–10 questions).** The coach runs the `quiz` step and prints the quiz.
Answer compactly, e.g.:

```text
1:A,2:BC,3:Y
```

It scores you immediately (writing answers to `learn/quiz01_answers.md`). Score
< 70%? Type `explain` for a recap and a retry. Otherwise:

```text
next
```

**4. Lab (~25–30 min).** The coach runs the `lab` step: it asks C# or Python,
proposes a couple of concepts, and scaffolds a LeetCode-style exercise under
`lab/iter01/` with `TODO` stubs and failing tests. Fill in the TODOs, then:

```text
check        # runs the tests and reports pass/fail (won't fix anything for you)
explain      # optional: hints tied to each TODO
```

When the lab passes:

```text
next
```

**5. Repeat.** The coach proposes the next topic based on your progress and starts
iteration `02`.

**Stop anytime.** Everything is recorded in `learnlog.md`. Next time, just open the
folder, start `copilot`, select the `ulearn` coach (or say "resume my learning"),
and it picks up exactly where you left off — on this machine or another.

### Commands at a glance

| Command   | What it does                                                        |
|-----------|---------------------------------------------------------------------|
| `next`    | Complete the current step and move to the next                      |
| `prev`    | Go back to the previous step                                        |
| `explain` | Quiz: explain wrong answers · Lab: hints tied to TODO IDs           |
| `check`   | Lab only: run the tests and report pass/fail                        |

### The skills are internal

You don't invoke the step skills directly — they're marked `user-invocable: false`.
The `ulearn` coach calls them as it runs the loop. Your single entry point is the
`ulearn` agent (via `/agent` or natural language).

## Where things live

- **The tool** (this plugin): installed once via `copilot plugin install`.
- **Your learning data**: `learn/`, `lab/`, and `learnlog.md` in each learning
  folder you work in. Keep those folders in their own git repo if you want to sync
  across machines — they're independent of this plugin.

This repo's [`.gitignore`](./.gitignore) excludes generated artifacts so they never
get committed here.

## Notes

- This was originally a set of VS Code Copilot slash-prompts (hence the repo name
  `vscode_prompts`); it now targets **GitHub Copilot CLI only**, packaged as a
  plugin. VS Code support has been dropped. (The instructions inside stay model- and
  tool-agnostic — they avoid hardcoded tool IDs and model names — but the packaging
  is CLI-specific.)
- The prompt modules use **Literal Prompt Programming (LPP)** — a small shared
  vocabulary between the author and the model. See [`lpp_spec.md`](./lpp_spec.md).
- Future: publish to a plugin marketplace for one-command install.

## License

MIT — see [`LICENSE`](./LICENSE).
