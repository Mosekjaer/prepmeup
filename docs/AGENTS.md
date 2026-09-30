# docs/

LaTeX-projektet: rapport, bilag og standalone-dokumenter. Se [docs/README.md](README.md) for opsætning.
Rod-[CLAUDE.md](../CLAUDE.md) gælder også her.

## Sprog

Rapport og bilag skrives på **engelsk**.
Undtagelse: dagsordener i `appendices/process/12-meeting-invitations/` og mødereferater i `appendices/process/13-meeting-minutes/` skrives på **dansk** og beholder `\usepackage[danish]{babel}`.

## Pakker

Alle LaTeX-pakker deklareres i `report/styles/dependencies.sty`. Aldrig et `\usepackage` i et kapitel eller i `main.tex`.
Standalone-dokumenter i `standalone/` og bilag er selvstændige og har deres egen præambel.

## Referencer

Citation style er **numerisk** (biblatex `style=numeric, sorting=none` — nummereret efter første optræden, ikke alfabetisk). Sat op i `report/styles/dependencies.sty` for rapporten og i `appendices/appendix-style.sty` for bilag, så begge bruger samme stil. AU stiller ikke krav om en bestemt style, kun konsekvens — og hvis numerisk: kronologisk nummerering (`god-rapportskrivning.md`). Denne stil matcher AU ECE's egen rapportskabelon.

Alle kilder samles i **én fælles fil**: `assets/references.bib`. Et bilag der har brug for kilder tilføjer `\addbibresource{../../../assets/references.bib}` (juster antal `../` efter dybde) og `\printbibliography` — opret aldrig en ny `.bib`-fil pr. bilag.

**Nøglekonvention:** kebab-case, `<organisation-eller-forfatter>-<emneord>[-<årstal>]`, fx `brs-forberedt`, `keycloak-authorization-services`. Undgår konflikter når flere tilføjer kilder samtidig.

**Kilder der ikke er bøger eller artikler** (myndighedsretningslinjer, leverandørdokumentation som Microsoft/Keycloak/Coolify-docs, GitHub-repos) citeres som `@online` (organisation som `author` i dobbelt-klammer, `{{Beredskabsstyrelsen}}`, så biblatex ikke tolker det som fornavn/efternavn) eller `@software` for et repo. Tilgangsdato skrives i `urldate`-feltet, ikke som fritekst i `note`.

`assets/references.bib` har én eksempelkilde af hver type (`@online`, `@software`, `@book`, `@article`) som skabelon — brug dem som forlæg, ikke som rigtige kilder (undtagen `brs-forberedt`, `agents365-drawio-skill` og `schwaber-scrum-guide-2016`, som er reelle).

Brug af AI-værktøjer citeres ikke i `references.bib` — det dækkes af AI-deklarationen, se nedenfor.

## Figurer

Dataplots er undtaget fra draw.io-reglen: de laves med `pgfplots` og læser data fra en CSV-fil i `assets/data/`. Se burn-up-figuren i `report/chapters/02-method.tex`.

Indsæt draw.io-diagrammer med alle tre argumenter:

```latex
\drawiofig{<navn>}{Caption text}{fig:my-label}
```

Kun kilden `assets/drawio/<navn>.drawio` committes. Den genererede `assets/drawio/<navn>.pdf` er et build-artefakt — `latexmk` genskaber den automatisk og den er gitignored.

Agenter tegner og retter diagrammer med skill'en `drawio-skill` (`.claude/skills/` for Claude Code, `.agents/skills/` for Antigravity):

1. `python3 <skill-dir>/scripts/validate.py assets/drawio/<navn>.drawio` skal give 0 fejl før eksport.
2. Udkast-PNG til visuelt tjek: `drawio -x -f png --width 2000 -o build/drawio-preview/<navn>.png assets/drawio/<navn>.drawio`.
3. Endelig PDF: `drawio -x -f pdf --crop -o assets/drawio/<navn>.pdf assets/drawio/<navn>.drawio` — samme kommando som `latexmkrc.example`.

**draw.io fra snap (Linux)** må ikke skrive til `/tmp` og fejler med `Error writing to file`. Eksportér derfor under `build/`, aldrig til `/tmp`. Følg heller ikke skill'ens råd om `export HOME=/tmp` — det bryder snap-udgaven.

## Build

- `latexmk` køres fra `docs/`, ikke fra repo-roden. Det er dér `main.tex` og `latexmkrc` ligger.
- `latexmkrc` er lokal og **må ikke committes** — den har maskinspecifikke stier (fx draw.io-binærens placering). Hver bruger kopierer `latexmkrc.example`.
- `build/` er gitignored og kan altid slettes; alt i den genskabes.
- Bilag er standalone-dokumenter og kompilerer hver for sig: `latexmk -pdf <fil>.tex` i bilagets egen mappe.

## Længde

Rapportens brødtekst fra `report/chapters/01-introduction.tex` til og med `09-conclusion.tex` må maks fylde **72.000 tegn**. Bilag tæller ikke med. Brug `/report-check` til optælling.

## AI-deklaration

`appendices/process/11-team/03-ai-declaration.tex` udfyldes **i hånden** før aflevering og **skal** med som bilag, når AI er brugt i projektet. Claude skriver den ikke.
