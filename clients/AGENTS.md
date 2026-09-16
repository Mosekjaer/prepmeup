# clients/

TypeScript-monorepo (npm workspaces, rod i `clients/`): `apps/mobile` (Expo, kunde-app), `apps/admin` (React + Vite, admin-panel) og `packages/api-client` (genereret).
Rod-[CLAUDE.md](../CLAUDE.md) gælder også her — især **Kilder** og **Link-format**. Herunder står kun det, der er specifikt for klienterne.

## Stak

- TypeScript i `strict`-mode overalt. Ingen `any`, ingen `@ts-ignore` uden begrundelse i samme commit.
- `apps/mobile`: Expo, React Native. Expo Router til navigation — ruter er mapper og filer under `src/app/`, ikke en manuelt vedligeholdt navigator.
- `apps/admin`: React + Vite. React Router; ruter defineres i `src/router.tsx` og ligger i `src/routes/`.
- Vitest til tests i begge apps. Mobilens komponenter testes mod `react-native-web` — se `apps/mobile/vitest.config.mts`.

## Forretningslogik hører ikke til her

Al beregning af pakkeindhold, vandbehov, udløb og genbestilling ligger i API'et (`PrepMeUp.Domain` / `PrepMeUp.Application`). Klienten kalder, viser og holder UI-state.
Fristes du til at regne noget ud i en komponent: det er et manglende endpoint, ikke en manglende hjælpefunktion.

## api-client

`packages/api-client` er genereret fra API'ets OpenAPI-dokument.

- Redigér **aldrig** den genererede kode i hånden. En håndrettelse er væk ved næste generering.
- Mangler der et felt eller et endpoint: ret contract'en i `PrepMeUp.Api` og generér om med `/api-contract`.
- Al netværkskald går gennem `api-client`. Ingen rå `fetch` mod API'et fra en skærm.

## Fire tilstande pr. skærm

Hver skærm, der henter data, håndterer alle fire eksplicit:

| Tilstand | Krav |
|---|---|
| loading | synlig indikator, ingen blank skærm |
| tom | forklarende tekst og næste handling, ikke en tom liste |
| fejl | læsbar besked + mulighed for at prøve igen |
| data | selve indholdet |

Mangler én af dem, er skærmen ikke færdig.

## UI-tekst

Engelsk. Labels, knapper, tomme tilstande, validerings- og fejlbeskeder — også dem der kommer fra API'et. Ingen i18n, ingen dansk brugertekst.

## Test

Vitest. Test komponentlogik og tilstandshåndtering, ikke den genererede klient.
