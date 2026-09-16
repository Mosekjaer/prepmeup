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

Claude må læse, kommentere og oprette opgaver — inklusive labels og relationer på nye kort. Nye kort lander i `To Do` og skrives på dansk.

Statusskift, flytning af kort mellem kolonner, tildeling, deadlines og sletning er gruppens beslutning og et bedømt læringsmål — Claude gør det ikke. Blokeringen håndhæves af deny-listen i `.claude/settings.json`.

## Secrets

Kun fra miljøvariabler. Aldrig i repoet, aldrig i en fil agenten kan læse, aldrig i konteksten.

## Definition of done

`dotnet build` og `dotnet test` er grønne. Ændringen er merget til `main` via en PR med grøn CI. Den, der committer, kan forklare ændringen — også til eksamen. Kaneo-opgaven er linket i PR'en. Kilder er angivet i svaret.
