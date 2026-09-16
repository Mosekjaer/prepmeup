# clients/

TypeScript-monorepo: `apps/mobile` (Expo) og `packages/api-client` (genereret).
Rod-[CLAUDE.md](../CLAUDE.md) gælder også her — især **Kilder** og **Link-format**. Herunder står kun det, der er specifikt for klienterne.

## Stak

- Expo, React Native, TypeScript i `strict`-mode. Ingen `any`, ingen `@ts-ignore` uden begrundelse i samme commit.
- Expo Router til navigation. Ruter er mapper og filer, ikke en manuelt vedligeholdt navigator.
- Vitest til tests.

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
