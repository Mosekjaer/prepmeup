## 2026-09-23 — Kaneo har en indbygget backlog

**Pattern:** Foreslog en `Backlog`-kolonne på boardet uden at tjekke, hvad Kaneo selv tilbyder. Kaneo har en Backlog-visning, der viser opgaver med status `planned`.
**Rule:** Tjek værktøjets egne visninger og datamodel (kildekode eller dokumentation) før du foreslår en workaround i det. Opret nye Kaneo-kort med `status: "planned"`, ikke `to-do`.
**Context:** Scrum-opsætning af PrepMeUp-boardet; `usekaneo/kaneo` `apps/docs/core/functional/backlog-planning.mdx`.

## 2026-09-30 — Kilder i dokumenter peger på originalen, ikke context/

**Pattern:** Skrev `\path{context/fed/markdown/...}` med linjenumre i Sources-afsnittet i bilag 2.1. `context/` er en lokal markdown-konvertering, som censor og gruppen ikke har; linjenumrene findes ikke i originalen.
**Rule:** I `docs/` citeres kursusmateriale som originalen: forelæser, slide-titel (PDF-navnet fra `Kilde:`/`source:` i metadata), kursus, AU, Brightspace, og slidenummer kun når det er slået op i den originale PDF. Bøger citeres med kapitel og side. `context/`-stier og linjenumre bruges kun i chatten.
**Context:** PMU-19, Sources-afsnittet i `docs/appendices/technical/02-analysis/01-technical-analysis.tex`. Afsnitsnumrene i `context/*.md` er ikke slidenumre (L16 React Overview: 28 afsnit, 35 slides).

## 2026-09-30 — Citér bøgerne og find rigtige kilder til emner uden for pensum

**Pattern:** Bilag 2.1 citerede kun Brightspace-slides, og emner uden kursusdækning (NSwag, Expo, Keycloak, Coolify, SQL Server-licens) stod bare som "not covered" uden kilde.
**Rule:** Citér kursusbøgerne (De Sanctis, Goldman, React Quickly, Database Systems) med kapitel/afsnit og trykt side, hvor de dækker påstanden — ikke kun slides. Er emnet ikke i pensum, så find en primærkilde online (RFC, OpenID-spec, officiel leverandørdokumentation, anerkendt bog) og verificér siden, før den citeres. Aldrig blogs, Medium, StackOverflow eller tutorial-sites.
**Context:** PMU-18, `docs/appendices/technical/02-analysis/01-technical-analysis.tex` og `docs/assets/references.bib`.

## 2026-09-30 — Webkilder sparsomt; bog og Brightspace først

**Pattern:** Efter "find online kilder" satte jeg ~60 webkilder ind i bilag 2.1 — også på påstande som bøgerne og slides allerede dækkede, og på trivielle fakta (health checks, volumes, restart-policy).
**Rule:** Bog- og Brightspace-kilder er førstevalg. Webkilde kun når pensum og bøger er tavse OG påstanden bærer en beslutning, en afvigelse eller en rettelse. Højst én webkilde pr. emne. Ingen webkilde ved siden af en bog- eller slidekilde, der allerede dækker påstanden.
**Context:** PMU-18, `docs/appendices/technical/02-analysis/01-technical-analysis.tex`. Endte på 10 webkilder mod 32 citerede i alt.

## 2026-09-30 — Rapporttekst skal lyde som studerende, ikke som AI

**Pattern:** Bilag 2.1 var skrevet tæt og teknisk: lange versionshistorik-linjer med alle detaljer, `\texttt{}` om klassenavne, filer, kommandoer og config-nøgler midt i sætninger, fed-start-bullets, konstruktioner som "The question is not X but Y", "There is a limit:", "The price is".
**Rule:** Skriv rapport og bilag i en almindelig studerendes stemme: "we", korte sætninger, prosa frem for bullets, én pointe ad gangen. Versionshistorik er én kort sætning pr. version ("Added client technologies"). Brug ikke `\texttt{}` i løbende tekst; omskriv til almindelige ord ("the Domain project", "the items endpoint", "the main branch") og undlad kode-identifiers, filstier, porte og config-nøgler, medmindre læseren skal bruge dem. Detaljer, der står i et andet bilag, henvises til i stedet for at gentages.
**Context:** PMU-18, `docs/appendices/technical/02-analysis/01-technical-analysis.tex`. Gælder alle dokumenter i `docs/`.

## 2026-10-06 — "Flow mellem noder" er et node-diagram, ikke et sekvensdiagram

**Pattern:** Bedt om et draw.io-diagram over auth-flowet (PMU-77) tegnede jeg et UML-sekvensdiagram med 17 beskeder. Frederik ville have et node-diagram, der giver overblik.
**Rule:** Når vejleder eller gruppen beder om et "flowdiagram" eller "flow mellem noder", så tegn først et node-diagram: én boks pr. node (klienter, Keycloak, API, database) og få nummererede pile med teknisk tekst. Tegn kun et sekvensdiagram, når der bliver bedt om det, eller som supplement.
**Context:** PMU-77, `docs/assets/drawio/auth-flow.drawio`. Michel bad 6/10 om "pile med teknisk tekst", så man kan bevare overblikket (referat 13.4).
