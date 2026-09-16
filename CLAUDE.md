# PrepMeUp — Claude Code

Projektets regler står i [AGENTS.md](AGENTS.md) og gælder uændret her. Læs den først.

@AGENTS.md

Denne fil indeholder kun det, der er specifikt for Claude Code. Alt andet hører i `AGENTS.md`, så gruppens
øvrige værktøjer (Antigravity læser `AGENTS.md`) arbejder efter de samme regler. Ret aldrig en fælles regel
her — ret den i `AGENTS.md`.

## Skills og agenter i `.claude/`

| Skill | Hvornår |
|---|---|
| `oral-examiner` | eksamination i hele projektet på dansk, standard 3 spørgsmål |
| `defend-my-code` | forbered forsvar af egne commits i `src/`, `clients/`, `tests/` |
| `plan` | ny plan i `.plans/NNNN-slug.md` + interaktiv HTML |
| `vertical-slice` | ny funktion gennem alle lag: Domain → Application → Infrastructure → Api → api-client → skærm |
| `tdd-loop` | red/green/refactor med NUnit + NSubstitute; testtilfælde godkendes først |
| `api-contract` | hold OpenAPI og `api-client` i sync (NSwag) |
| `report-check` | kritik af rapporten, tegnoptælling mod 72.000 |
| `meeting-docs` | dagsordener og referater på dansk i LaTeX |
| `git-workflow` | start arbejde på branch, commit, PR mod `main`, release-tag (GitHub Flow) |

| Agent (read-only) | Dækker |
|---|---|
| `architecture-reviewer` | lagregler, afhængighedsretning, SOLID, design smells |
| `backend-reviewer` | REST, statuskoder, model binding, EF-model, N+1, migrations |
| `test-designer` | ækvivalensklasser, grænseværdier, fakes, integrationsplan |
| `security-reviewer` | auth (Keycloak/JWT), adgangskontrol, secrets, validering |

## Hooks

`.claude/hooks/git-behind.sh` kører ved sessionsstart og før `Edit`/`Write`. Den fetcher fra origin
(throttlet til hvert 5. minut) og siger til, hvis branchen er bagud. Den puller aldrig selv.

## Kaneo

MCP-serveren er defineret i `.mcp.json` som en stdio-server (`npx @kaneo/mcp`, peger på
`KANEO_API_URL=https://kaneo.mosekjaer.com`). Første gang serveren bruges, åbner den en
device-authorization-flow i browseren, hvor du selv godkender login — der er ikke længere
et statisk token i miljøvariabler. Kræver Node.js 24+ installeret lokalt.

Tilladt i `.claude/settings.json`: læsning, `create_task_comment`, `create_task`, `attach_label_to_task`
og `create_task_relation`. Alt andet skrivende er på deny-listen — se Kaneo-afsnittet i `AGENTS.md`.
