# PrepMeUp

Beredskabsabonnement. En husstand opretter en profil med sin sammensætning (voksne, børn, præferencer), og systemet beregner en anbefalet beredskabspakke efter Beredskabsstyrelsens retningslinjer.
Første pakke leveres samlet, og systemet registrerer udløbsdatoen på hver vare i husstandens lager. Når en vare nærmer sig udløb, planlægges en genbestilling automatisk, og brugeren får besked.
Leverandøren er ikke en rigtig integration: den er simuleret internt med et varekatalog og et mock betalings- og leveringsflow.
SW4PRJ4, 4. semester softwareteknologi, gruppe 3.

## Stak

| Område | Valg |
|---|---|
| Kunde-app | Expo (TypeScript, Expo Router). Mindst Android, måske web |
| Admin-panel | React + Vite (TypeScript, React Router) |
| Backend | ASP.NET Core Web API, .NET 10, EF Core |
| Database | SQL i Docker |
| Migrations | EF Core Migrations (code-first) i `PrepMeUp.Infrastructure`. Skemaet ændres kun via `dotnet ef migrations add`, aldrig i hånden. Deploy kører dem som migration bundle |
| Test | NUnit + NSubstitute (backend), Vitest (klienter) |
| CI | GitHub Actions: grøn build + test før merge (`.github/workflows/ci.yml`) |
| Drift | Coolify på egen server. Én `Dockerfile` pr. service, samlet i `docker-compose.yml` |
| Auth | Trin 2: Keycloak som ekstern authority, selvhostet i samme compose |

## Sprog

- Chat, planer og eksamination: dansk.
- Kode, identifiers, kommentarer og commits: engelsk.
- Agenter skriver ikke kommentarer i kildekoden. Begrundelsen hører i commit-beskeden, PR-teksten eller chatten. Kommentarer skrevet af gruppen står urørt.
- Alt brugeren ser i appen: engelsk. Labels, knapper, validerings- og API-fejlbeskeder. Ingen i18n.
- Rapport og bilag i `docs/`: engelsk. **Undtagelse:** dagsordener (`docs/appendices/process/12-meeting-invitations`) og mødereferater (`docs/appendices/process/13-meeting-minutes`) skrives på dansk.

## Kilder (obligatorisk)

- Slå op i [context/INDEX.md](context/INDEX.md) **før** du læser kursusmateriale. Læs kun de filer, der matcher emnet. Læs aldrig `context/` igennem i blinde.
- Hvert svar, der foreslår eller skriver kode, slutter med sektionen **Kilder** med markdown-links til de `context/`-filer og linjer, mønsteret stammer fra. Henvis også til `docs/standalone/architecture-pitch.tex`, når en lagregel er grundlaget.
- Ingen kildehenvisninger som kommentarer i kildekoden. Kilder står i chatten, aldrig i koden.
- Findes mønsteret ikke i kursusmaterialet, skriv: `⚠ Ikke i kursusmaterialet: <begrundelse>`. Det gælder bl.a. Coolify, TanStack Query, klientgenerering til TypeScript og Keycloak-integration.

## Link-format

Altid `[sti:linjer](sti#Lstart-Lslut)` — linkteksten er selv den relative sti med linjenumre:
`[src/PrepMeUp.Application/HouseholdService.cs:42-51](src/PrepMeUp.Application/HouseholdService.cs#L42-L51)`

VS Code-udvidelsen bruger linkmålet, åbner filen og markerer intervallet. VS Code's integrerede terminal genkender `sti:linje` i den viste linktekst og åbner på linjen med Ctrl+klik. Én skrivemåde dækker begge.

## Diagrammer

- Diagrammer der skal i rapporten eller bilagene laves i draw.io. Kilden er en `.drawio`-fil i `docs/assets/drawio/`, og den committes — PDF'en genereres af `latexmk` og er et build-artefakt.
- Indsæt med `\drawiofig{filnavn}{caption}{fig:label}`. Ingen `\includegraphics` direkte mod en genereret PDF.
- Agenter bruger `drawio-skill` til at skrive og validere XML'en. Skriv ikke draw.io-XML i hånden uden validering.
- Mermaid og PlantUML er til skitser i chat og i `.plans/` — aldrig til noget der ender i `docs/`. Ét format i rapporten.
- **Undtagelse: dataplots** (fx burn-up) laves med `pgfplots` direkte i LaTeX og læser data fra en CSV-fil i `docs/assets/data/`. Tallene skrives kun i CSV-filen; tabeller og plots genereres fra den.

## Lagregler

Fra `docs/standalone/architecture-pitch.tex`, slide 5.

| modul | må | må ikke |
|---|---|---|
| `Api` | route, validere, mappe contract mod domæne, statuskoder, autorisation | forretningsregler; kende `DbContext` |
| `Application` | orkestrere use cases, definere repository-interfaces, transaktionsgrænser | kende HTTP, `ControllerBase`, EF-typer, SQL |
| `Domain` | entiteter, invarianter, vandbehov, pakkeindhold, udløb | referere til noget andet projekt overhovedet |
| `Infrastructure` | EF-konfiguration, queries, migrations | træffe beslutninger — den henter og gemmer |
| `clients/apps` | kalde API'et via `api-client`, holde UI-state | duplikere forretningsregler; gå uden om klientpakken |

## Modulafhængigheder

Slide 6. Pilen peger mod det, der refereres.

- `Api` → `Application` → `Domain`.
- `Infrastructure` implementerer `Application`s interfaces og mapper `Domain`-entiteter.
- `Domain` refererer ingenting. Derfor kan forretningsreglerne testes uden database, HTTP eller browser.
- `Api` kender kun `Infrastructure` i composition root (`Program.cs`, DI-registrering). Ingen andre steder.
- `Tests.Unit` → `Application`. `Tests.Integration` → `Api` (`WebApplicationFactory`).

## Repo-struktur

```
src/PrepMeUp.Domain/           entiteter, invarianter — ingen referencer
src/PrepMeUp.Application/      use cases, repository-interfaces
src/PrepMeUp.Infrastructure/   DbContext, EF-repositories, migrations
src/PrepMeUp.Api/              controllers, contracts, middleware, BackgroundService
clients/apps/mobile/           Expo — kunde-app
clients/apps/admin/            React + Vite — admin-panel
clients/packages/api-client/   genereret fra OpenAPI — redigeres aldrig i hånden
tests/PrepMeUp.Tests.Unit/
tests/PrepMeUp.Tests.Integration/
docs/                          rapport og bilag (LaTeX)
context/                       kursusmateriale + INDEX.md
.plans/                        planer fra /plan
```

## Kommandoer

| Kommando | Hvad |
|---|---|
| `dotnet build` | bygger hele solution |
| `dotnet test` | kører unit- og integrationstests |
| `docker compose up` | api + db lokalt; db eksponeres ikke til host. Kræver `MSSQL_SA_PASSWORD` i miljøet eller `.env` |
| `npm install` | **køres fra `clients/`** — ét workspace for begge apps og `api-client` |
| `npm test` / `npm run typecheck` | Vitest og `tsc` på tværs af alle klient-workspaces |
| `npm run generate:api-client` | genererer `api-client` fra API'ets OpenAPI-dokument (NSwag) |
| `latexmk` | **køres fra `docs/`**, ikke fra roden |

## Git

GitHub Flow. `main` er den eneste langlivede branch og altid deploybar — Coolify deployer den. Fremgangsmåde trin for trin: `.claude/skills/git-workflow/SKILL.md`.

- **Commit aldrig direkte på `main`.** Står du på `main`, så opret en branch først.
- **Branch:** `<type>/<slug>` fra en frisk `main`. Slug i små bogstaver med bindestreger, Kaneo-nummer først når der er et kort: `feat/pmu-12-household-profile`. Kortlivet — dage, ikke uger.
- **Typer** (samme for branch og commit): `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert`.
- **Commits:** Conventional Commits på engelsk, imperativ, header maks. 72 tegn: `feat(api): add household profile endpoint`. Scope er valgfrit.
- **Hold branchen ajour** med `git merge origin/main`. Ingen rebase af pushede branches, ingen force push.
- **Merge:** pull request mod `main`, grøn CI og ét review. **Squash merge** — PR-titlen bliver commit-beskeden på `main` og skal derfor selv være en Conventional Commits-header. Branchen slettes efter merge.
- **Releases:** annoteret tag på `main` ved sprint-afslutning eller aflevering (`v0.1.0`). Ingen release-branches.

Reglerne står ét sted: `.githooks/check-conventions.sh`. Den bruges af `commit-msg`- og `pre-push`-hooks lokalt og af `.github/workflows/conventions.yml` på hver PR. `npm install` i `clients/` aktiverer hooks; ellers `git config core.hooksPath .githooks` én gang pr. klon.

## Kaneo

Claude må læse, kommentere og oprette opgaver og labels, sætte labels og relationer, og sætte forfaldsdato til sprintens sidste dag. Nye kort oprettes med `status: "planned"`, så de lander i Backlog-visningen og ikke på boardet, og skrives på dansk.

Statusskift, flytning af kort mellem kolonner, tildeling, startdatoer, estimater og sletning er gruppens beslutning og et bedømt læringsmål — Claude gør det ikke. Claude må foreslå et estimat i chatten, men gruppen sætter point-labelen. Blokeringen håndhæves af deny-listen i `.claude/settings.json`.

### Sprints

To uger, onsdag til tirsdag. Sprint review, retrospective og planning holdes samme dag.

| Sprint | Periode |
|---|---|
| 1 | 16/9 – 29/9 |
| 2 | 30/9 – 13/10 |
| 3 | 14/10 – 27/10 |
| 4 | 28/10 – 10/11 |
| 5 | 11/11 – 24/11 |
| 6 | 25/11 – 8/12 |

Aflevering fredag 11/12 kl. 13.00. Dagene 9/12 – 11/12 er ikke en sprint.

- **Backlog-visningen** (`planned`) er Product Backlog. **Boardet** er Sprint Backlog. Der oprettes ikke en kolonne ved navn Backlog.
- Et kort flyttes fra Backlog til `To Do` ved sprint planning og kun når det er **ready**: beskrivelse med `Krav: FR-xx` som første linje (eller ingen FR ved `Teknisk`/`Rapport`), definition of done, point-label, epic-label og en ejer.
- Et kort der ikke bliver færdigt, beholder sin sprint-label og får den næste oveni. To sprint-labels = spillover.

### Labels

| Type | Labels | Farve |
|---|---|---|
| Point | `0 pt`, `½ pt`, `1 pt`, `2 pt`, `3 pt`, `5 pt`, `8 pt`, `13 pt`, `20 pt`, `40 pt`, `? pt` | grøn 0–2, gul 3–5, rød 8+, grå `?` |
| Sprint | `Sprint 1` … `Sprint 6` | blå |
| Epic | `Account`, `Household`, `Subscription`, `Notifications`, `Catalogue`, `Dispatch` — grupperingen fra kravspecifikationen | teal |
| Ikke-krav | `Teknisk`, `Rapport` | grå |
| Undergruppe | `Gruppe 1`, `Gruppe 2`, `Alle` | lilla, gul, mørkegrå |

Velocity: ved sprint planning noteres summen af point i `To Do` (committed), ved sprint review summen i `Done` (done). Tallene skrives i `docs/assets/data/sprint-velocity.csv` (kolonnerne `committed`, `done`, `spillover` og `scope` = samlede point i hele backloggen ved review). Tabellen i bilag 10.2 og burn-up-figuren i kap. 2 genereres fra filen.

## Secrets

Kun fra miljøvariabler. Aldrig i repoet, aldrig i en fil agenten kan læse, aldrig i konteksten.

## Definition of done

`dotnet build` og `dotnet test` er grønne. Ændringen er merget til `main` via en PR med grøn CI. Den, der committer, kan forklare ændringen — også til eksamen. Kaneo-opgaven er linket i PR'en. Kilder er angivet i svaret.
