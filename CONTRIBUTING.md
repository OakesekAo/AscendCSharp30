# Contributing to AscendCSharp30

Thanks for contributing to AscendCSharp30 — this repository is designed as a 30‑day hands‑on learning path for modern C# and .NET. To keep the project consistent and easy to use for learners, please follow these contribution guidelines.

## Table of Contents

- Getting started
- Repository layout
- Daily folder conventions
- Coding & formatting
- Branches & commit messages
- Pull requests
- Issues & templates
- Tests
- Visual Studio settings
- License

## Getting started

1. Fork the repo and create branches from `main`.
2. Keep your PRs small and focused — one day or one feature per PR.
3. Run the included build and tests before submitting a PR: `dotnet build` and `dotnet test` where applicable.

## Repository layout

Top-level folder structure:

- `Day01-Setup-And-Tooling/`
  - `Day01-Starter/` (minimal starter code + README)
  - `Day01-Complete/` (runnable complete solution for Day 01)
- `Day02-.../`
- `README.md`
- `CONTRIBUTING.md`
- `.editorconfig` (coding style)

Each day should be self-contained: starter instructions and any boilerplate go in `*-Starter`, while the finished version goes in `*-Complete`.

## Daily folder conventions

- Use `DayNN-Title/DayNN-Starter/` and `DayNN-Title/DayNN-Complete/` (zero-padded day number, e.g., `Day01`).
- Each `*-Starter` must include a `README.md` with objectives, required steps, and any commands to run.
- Each `*-Complete` must be a runnable state (solution or project) and include a short `README.md` showing how to run or inspect the solution.
- Keep project files under their day folders; avoid placing build artifacts in source control.

## Coding & formatting

- Follow the repository's `.editorconfig`. If you modify or add rules, update `.editorconfig` at the repo root.
- Target frameworks are .NET 10 where relevant. Prefer C# latest stable language features allowed by .NET 10.
- Keep APIs and examples minimal and focused on the learning objective for the day.

## Branches & commit messages

- Branch names: `dayNN/short-description` or `feature/short-description`.
- Commit messages: use present tense and be descriptive. Example: `Add Day03 starter: control-flow examples`.
- Keep PRs scoped to one day or one issue.

## Pull requests

- Create PRs against `main`.
- Include a short description of what the PR adds/changes and a link to any related issue.
- Add reviewers and request feedback early.

## Issues & templates

- Use issue templates for Bug, Feature, and Content Request.
- Label issues with `area:dayNN`, `type:bug`, `type:content`, etc.

## Tests

- Add unit tests for examples where appropriate using xUnit.
- Run `dotnet test` in the project folders that include test projects.

## Visual Studio

- Recommended: open the solution with Visual Studio. __Open Folder__ or use the solution file if provided.
- Useful commands/settings:
  - __Format Document__ to apply repository style
  - __Git Changes__ for staging/committing
  - __Build Solution__ before creating a PR

## License

This project is MIT licensed — see the repo `LICENSE` file.

---

Thanks for helping make AscendCSharp30 a consistent and friendly learning path.
