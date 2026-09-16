---
name: plan
description: Lav en interaktiv plan som lokal HTML — diagrammer, filkort, beslutninger, risici, åbne spørgsmål og afkrydsbare trin. Brug ved /plan, "lav en plan", "planlæg", "visual plan", ved enhver ikke-triviel feature, refactor eller migration, og når en eksisterende .plans/*.md skal bygges om efter feedback.
argument-hint: "[kort beskrivelse af opgaven]"
---

# Plan

Skriv planen som markdown med custom blokke, byg én selvstændig HTML-fil med
`scripts/build.py`, og giv Frederik `file://`-URL'en. Alt kører lokalt — ingen
netværk, ingen npx.

## Arbejdsgang

**1. Læs blok-kataloget.** Læs `references/blocks.md` i denne skill-mappe, før du
skriver planen. Den definerer de otte blok-typer (mermaid, callout, files,
questions, steps, columns, tabs, wireframe), fence-reglerne og kvalitetsbaren.
Skriv aldrig blokke fra hukommelsen.

**2. Skriv planen** som `.plans/NNNN-slug.md` i repo-roden. Fortsæt
nummereringen fra det højeste eksisterende `NNNN`. Planen er teknisk og
objektiv: objektiv og done-kriterie først, rigtige filstier og symboler,
beslutninger med begrundelse, risici, og verifikation som sidste trin.

Fast struktur i PrepMeUp:

```
# <Titel>
<2-4 linjer: objektiv + hvad "færdig" betyder>
## Kontekst og scope     ← også hvad der IKKE er med
## Løsning               ← decision-callouts, mermaid, columns
## Filer                 ← files-blok med verificerede stier
## Nøglekode             ← tabs-blok
## Trin                  ← steps-blok, sidste trin er verifikation
## Risici                ← risk-callouts
## Åbne spørgsmål        ← questions-blok
## Kilder                ← links til context/ bag de valgte mønstre
```

**3. Byg HTML:**

```bash
python3 .claude/skills/plan/scripts/build.py .plans/NNNN-slug.md    # Linux, macOS
py       .claude/skills/plan/scripts/build.py .plans/NNNN-slug.md    # Windows
```

Scriptet printer output-stien og en `file://`-URL. Output lander som
`.plans/NNNN-slug.html` (overstyr med `--out`). Kun Python 3's
standardbibliotek bruges.

**4. Åbn den:** `xdg-open` (Linux), `open` (macOS) eller `start` (Windows), og
giv Frederik `file://`-URL'en.

**5. Feedback.** Viewer'en gemmer kommentarer, svar på åbne spørgsmål og
afkrydsninger i localStorage. Frederik kopierer feedbacken ind i chatten eller
eksporterer JSON til `~/Downloads/plan-feedback-<slug>.json` (find den nyeste
med `ls -t ~/Downloads/plan-feedback-*.json | head -1`).

Når feedback kommer: ret i **markdown-filen** — fold beslutninger ind som
normal plan-prosa, skriv aldrig "rettet efter feedback" — og byg igen til
**samme** output-sti. localStorage er nøglet på slug, så afkrydsninger og
kommentarer overlever en rebuild. Bed ham genindlæse siden.

## Regler

- HTML-filen genereres. Redigér aldrig `.html` i hånden; ret markdown og byg igen.
- Planen skal kunne læses uden chat-historik. Ingen referencer til samtalen.
- Uafklarede valg hører i en `questions`-blok, ikke i prosa.
- Filstier i `files`-blokke skal være verificerede, reelle stier. Tjek med `ls`.
- Planen skrives på dansk. Kode i planen er på engelsk.
- Diagrammer kun hvor de bærer information.
- `.plans/*.md` committes — planerne er procesdokumentation til rapporten.
  `.plans/*.html` er et artefakt og er gitignored.
- Mønstre i planen skal have kilder i `context/` (slå op i `context/INDEX.md`).
  Findes der ingen: skriv `⚠ Ikke i kursusmaterialet: <begrundelse>`.

## Fejlfinding

- Mermaid-fejl vises inline i den byggede side med fejltekst. Ret syntaksen og byg igen.
- Blanke sider skyldes næsten altid en ulukket custom fence. Tjek at åbning og
  lukning har samme antal backticks.
- Vendored libs ligger i `assets/vendor/` (marked 12, mermaid 11.4,
  highlight.js 11.10) og inlines ved build. Opdatering: erstat filen og byg igen.
- Logoet ligger i `assets/prepmeup-logo.svg` og inlines i sidebar og favicon.
