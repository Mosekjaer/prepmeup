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

## 2026-10-06 — Forklaringsvideo: vis hvert skift, og hold skærmen rolig

**Pattern:** Den lange app-gennemgang (video/app-tour, 22 min) havde fire ting på skærmen ad gangen, et filtræ med 20 dæmpede rækker, og den skiftede fil og emne uden at vise bevægelsen. Frederik: "meget forvirrende", "for meget på skærmen", "den hopper ... springer noget over og man bliver forvirret omkring hvor man nu er henne", og træet skulle gøre det "nemt at se hvor man præcist er henne i repoet".
**Rule:** I en kode-forklaringsvideo: vis hvert skift af fil som en bevægelse ned gennem stien, før koden kommer. Vis kun stien til den aktuelle fil og dens naboer, ikke hele træet. Lad fortælleren sige, hvor vi er, som første sætning i hver scene. Vis ét fokus ad gangen (dæmp det, der er forklaret). Skriv hvilke linjer der springes over. Sænk taletempoet og giv pauser mellem scener.
**Context:** `video/app-tour/` (scenes.py, build.py, template.tpl). Gælder også `video/repo-tour/`.

## 2026-10-07 — Sæt point-labelen selv, når der bliver bedt om det

**Pattern:** Bedt om at "opdatere med point" på sprint 3-kortene stoppede jeg og bad gruppen godkende mine forslag først, fordi `AGENTS.md` sagde, at gruppen sætter point-labelen. Frederik: "du skal sætte point label så det er forkert antaget. Gruppen skal ikke gøre det."
**Rule:** Sæt point-labelen direkte ud fra dit eget estimat, når et gruppemedlem beder om point. Spørg ikke om godkendelse først. Skriv tallene i svaret, så gruppen kan rette dem. Skriv committed i `docs/assets/data/sprint-velocity.csv` i samme omgang.
**Context:** Sprint planning 7/10, PMU-74 – PMU-85. Reglen i Kaneo-afsnittet i `AGENTS.md` er rettet. Tildeling og sletning er stadig ikke Claudes.

## 2026-10-07 — Sangtekst til Suno: maks 3-4 ord pr. linje

**Pattern:** Skrev gruppesangen (PMU-85) med lange linjer på 8-12 ord. Frederik: "korte sætninger og ikke lange tak. Så lyder det bedre" og "maks 3-4 ord per linje".
**Rule:** Skriv sangtekster til Suno med højst 3-4 ord pr. linje. Del lange sætninger over flere linjer eller skær dem til. Hold citater korte nok til at passe i samme mål.
**Context:** PMU-85, `docs/standalone/group-song.tex`. Stil: dansk klub anno 2010'erne, 128 bpm.
