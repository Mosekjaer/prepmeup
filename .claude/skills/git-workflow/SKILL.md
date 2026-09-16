---
name: git-workflow
description: GitHub Flow i PrepMeUp — opret branch fra main, Conventional Commits, hold branchen ajour, åbn PR med squash merge, ryd op efter merge, tag en release. Brug FØR første filændring når arbejdet starter på en ny feature, fix, refactor eller opgave ("start på", "ny feature", "lav en branch", "PMU-12"), ved "commit", "push", "åbn PR", "pull request", "merge", "release", "tag", og når du står på main med ændringer.
argument-hint: "[start <type> <slug> | pr | done | release <version>]"
---

# Git workflow

Reglerne står i Git-afsnittet i `AGENTS.md`. Denne skill er fremgangsmåden. Håndhævelsen ligger i
`.githooks/check-conventions.sh` — den er sandheden, hvis de to nogensinde er uenige.

## 0. Forudsætninger

```bash
git config --get core.hooksPath   # skal give .githooks
```

Tomt svar: kør `git config core.hooksPath .githooks` og sig det til brugeren.

## 1. Start arbejde

Før første filændring.

1. `git status --short`. Er træet ikke rent, så stop og spørg: stash, commit på nuværende branch, eller tag ændringerne med over?
2. Find typen ud fra opgaven: `feat` ny funktion, `fix` fejl, `refactor` omstrukturering uden ny adfærd, `test` kun tests, `docs` rapport/dokumentation, `chore`/`build`/`ci` værktøj, afhængigheder og pipeline.
3. Find Kaneo-kortet, hvis brugeren nævner et (`PMU-12`), eller søg efter det. Intet kort: branch uden nummer, og nævn det for brugeren — DoD kræver et linket kort.
4. Opret branchen:

```bash
git fetch origin
git switch main
git pull --ff-only
git switch -c feat/pmu-12-household-profile
.githooks/check-conventions.sh branch "$(git branch --show-current)"
```

Slug: engelsk, små bogstaver, bindestreger, 2-5 ord.

## 2. Commit

- Små commits, én logisk ændring hver. Header: `<type>(<scope>): <imperativ beskrivelse>`, engelsk, maks. 72 tegn.
- Scope er valgfrit. Brug et lag eller en app, når det hjælper: `api`, `domain`, `application`, `infrastructure`, `mobile`, `admin`, `api-client`, `docs`.
- Breaking change i API-kontrakten: `!` efter typen, `feat(api)!: rename household endpoint`, og en `BREAKING CHANGE:`-linje i body.
- Afviser `commit-msg`-hooken beskeden, så ret beskeden. Brug aldrig `--no-verify`.

## 3. Hold branchen ajour

```bash
git fetch origin
git merge origin/main
```

Rebase og force push er forbudt, fordi branches deles. Squash merge fjerner alligevel merge-commits fra `main`.

## 4. Pull request

Før PR: `dotnet build` og `dotnet test` grønne; `npm run typecheck` og `npm test` fra `clients/`, hvis `clients/` er ændret.

```bash
git push -u origin HEAD
gh pr create --base main \
  --title "feat(api): add household profile endpoint" \
  --body "$(cat <<'BODY'
## What
<1-3 lines>

## Why
<1-3 lines>

## Verification
- [ ] dotnet build / dotnet test
- [ ] npm run typecheck / npm test (if clients/ changed)

Kaneo: PMU-12
BODY
)"
```

- **PR-titlen bliver commit-beskeden på `main`.** Den skal være en gyldig header; tjek med `.githooks/check-conventions.sh header "<titel>"`.
- Titel og body på engelsk.
- Kun ét Kaneo-kort pr. PR. Claude må kommentere PR-linket på kortet, men flytter det aldrig (se Kaneo i `AGENTS.md`).
- Merge ikke selv. Review og merge er gruppens.

## 5. Efter merge

```bash
git switch main
git pull --ff-only
git branch -d feat/pmu-12-household-profile
git fetch --prune
```

`git branch -d` afviser en squash-merget branch. Bekræft at PR'en er merget (`gh pr view <nr> --json state`), og brug så `-D`.

## 6. Release

Kun når brugeren beder om det — typisk ved sprint-afslutning eller aflevering.

```bash
git switch main
git pull --ff-only
git tag -a v0.2.0 -m "v0.2.0: <kort indhold>"
git push origin v0.2.0
```

Versionering: `0.MINOR.0` pr. sprint, `0.x.PATCH` for rettelser mellem sprints. `1.0.0` er den endelige aflevering.

## Aldrig

- Commit eller push direkte til `main`.
- `--no-verify`, `push --force` eller rebase af en pushet branch.
- Merge en PR eller ændre branch protection uden at brugeren beder om det.
