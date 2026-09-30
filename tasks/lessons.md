## 2026-09-23 — Kaneo har en indbygget backlog

**Pattern:** Foreslog en `Backlog`-kolonne på boardet uden at tjekke, hvad Kaneo selv tilbyder. Kaneo har en Backlog-visning, der viser opgaver med status `planned`.
**Rule:** Tjek værktøjets egne visninger og datamodel (kildekode eller dokumentation) før du foreslår en workaround i det. Opret nye Kaneo-kort med `status: "planned"`, ikke `to-do`.
**Context:** Scrum-opsætning af PrepMeUp-boardet; `usekaneo/kaneo` `apps/docs/core/functional/backlog-planning.mdx`.

## 2026-09-30 — Kilder i dokumenter peger på originalen, ikke context/

**Pattern:** Skrev `\path{context/fed/markdown/...}` med linjenumre i Sources-afsnittet i bilag 2.1. `context/` er en lokal markdown-konvertering, som censor og gruppen ikke har; linjenumrene findes ikke i originalen.
**Rule:** I `docs/` citeres kursusmateriale som originalen: forelæser, slide-titel (PDF-navnet fra `Kilde:`/`source:` i metadata), kursus, AU, Brightspace, og slidenummer kun når det er slået op i den originale PDF. Bøger citeres med kapitel og side. `context/`-stier og linjenumre bruges kun i chatten.
**Context:** PMU-19, Sources-afsnittet i `docs/appendices/technical/02-analysis/01-technical-analysis.tex`. Afsnitsnumrene i `context/*.md` er ikke slidenumre (L16 React Overview: 28 afsnit, 35 slides).
