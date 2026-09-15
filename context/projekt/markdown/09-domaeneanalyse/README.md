# 09 — Domæneanalyse og domænemodeller (L15–L17)

Modul fra SWISE-01 Indledende System Engineering (undervist som del af SW4PRJ4-02 Projekt 4, E2026). Dækker Brightspace-siderne "L16-17 Domæneanalyse" (Table of Contents: 1. L16 Introduktion til domænemodeller, 2. L17 Domænemodeller fortsat med øvelser, 3. Løsningsforslag til Domænemodel for Benzintanken, 4. Løsningsforslag til Domænemodel for Poultry Galore). Slides er fra "Introduktion til Systems Engineering, I2ISE"; artikel og eksempel er skrevet af Frank Bodholdt Jakobsen (Ingeniørhøjskolen Aarhus Universitet). Underviser på det aktuelle hold: ikke angivet i kilderne.

Kerneindhold: Systemdomæneanalyse afgrænser systemets Domæne (de begreber fra virkeligheden systemet skal huske). Resultatet er Domænemodellen — et UML-klassediagram med begreber (conceptual classes), associationer med relationstekst og læsepil (ingen pile i enderne), multiplicitet og attributter uden typer, ingen metoder. Kogebog i 4 steps: (1) begreber via navneord + kategoriliste, (2) relationer via udsagnsord + relationsliste, (3) multiplicitet, (4) finpudsning (attribut vs. klasse, aktør som tændstiksmand vs. klasse, systembegreb, komposition/arv sjældent, oprydning).

## Filer

| Fil | Kilde-PDF | Type | Sider | Indhold |
|---|---|---|---|---|
| `system-domain-analysis.md` | `System Domain Analysis.pdf` | slides | 34 | Hvad/hvorfor/hvordan domænemodel, Video-eksempel (mermaid), kogebog step 1–4, aktør, attribut vs. klasse, praktiske råd, øvelsesoplæg |
| `artikel-domaenemodeller.md` | `DomæneModeller.pdf` | artikel | 11 | "Systemdomæneanalyse og Domænemodeller" v1.0 — fuld tekst inkl. komplet kategoriliste (1.6.1) og relationsliste (1.6.2) |
| `eksempel-domaenemodel.md` | `DomænemodellerEksempel.pdf` | eksempel | 11 | Føtex-selvbetjeningskasse gennemarbejdet efter kogebogen; alle trin som tabeller, endelig model som mermaid classDiagram |
| `oevelse-benzinstander-domaenemodel.md` | `SDA_Benzinstander_Opgave1.pdf` + `SDA Benzinstation Løsning E2018.pdf` | øvelse + løsningsforslag | 3 + 6 | BDD for tankstation, UC1 Optank Bil, opgave; løsning trin for trin, endelig model som mermaid |
| `oevelse-poultry-galore-domaenemodel.md` | `SDA Poultry Galore.pdf` + `SDA Poultry Galore solution.pdf` + `SDA Poultry Galore solution3.pdf` | øvelse + 2 løsningsforslag | 1 + 1 + 1 | Batching-anlæg; begge løsninger som tabel + mermaid, forskelle opsummeret |

**Dubletter:** `DomæneModeller.pdf` og `DomænemodellerEksempel.pdf` findes både i zip 1 og zip 10. De er konverteret én gang (fra zip 10).

Ikke konverteret: `Table of Contents.html` (kun sidenavigation, gengivet ovenfor).

## Brightspace: L15-16 Domæneanalyse

**Indhold:**
- Domæneanalyse (Fra Use Cases til struktur)
- Objekter og konceptuelle klasser – begreber
- Relationer og attributter

**Læsestof:**
- Læs om "Hvad er Domain?" og "Hvad er Domain model?" under Content → L16-L17 Domæneanalyse
- `DomæneModeller.pdf` — artikel om Systemdomæneanalyse og Domænemodeller som forberedelse til lektionerne
- `DomænemodellerEksempel.pdf` — eksempel, gennemarbejdet efter den arbejdsmetode (Kogebog for Domænemodeller), der gennemgås i artiklen
- Underviserens forslag: læs artiklen og kig parallelt på gennemarbejdelsen af eksemplet for også at få en praktisk forståelse for arbejdet med at lave en Domænemodel

**Slides:** `System Domain Analysis.pdf` — anvendes ved forelæsningen. Prøver at forstå:
- Hvad er domæne? Hvad er domænemodel?
- Begreber, relationer (associationer), multiplicitet
- Hvorfor bruger man domænemodel?
- Hvordan laver man faktisk en domænemodel trin for trin?
- Hvordan passer det med at fuldstændiggøre og begrænse Domænemodellen?

**Øvelse:** SDA Benzinstander Opgave1 (`SDA_Benzinstander_Opgave1.pdf`)

**Referencer: UML.** I denne fase af systemudviklingen er der fokus på softwareudvikling. Det kan derfor være lidt nemmere at bruge et rent UML-tegneværktøj til at lave diagrammerne. Husk stadig ramme med diagramtype.

**Tool:** UMLet eller Draw.io kan bruges.

**SysML vs. UML (note fra kursussiden):** SysML er en extension og præsenterer mere brede områder som HW, SW og de andre elementer af systemer, ikke kun software. SysML præsenterer generel System Engineering, så ISE-faget bruger SysML for System Architecture (HW+SW). UML er fokuseret på software, dvs. klassediagram m.m. ISE-faget bruger UML for software.

## Brightspace: Øvelsesopgaver til L15 og L16

**Indhold:** Øvelser i domæneanalyse med UML (løsningsforslag gennemgås i L16)
- SDA Benzinstander Opgave1 — løsningsforslag gennemgås i L17
- SDA Poultry Galore — der arbejdes på klassen i L17

**Lektionsmateriale:** Refer Lektion 16. Recap Domain Model: https://en.wikipedia.org/wiki/Domain_model

**Spørgsmål (Questions):**
- Hvad er domain model?
- Hvad er domain?
- Hvorfor bruger vi domain model?
- Fra domain model til OOP software development

**Øvelse:** Fortsat SDA Benzinstander fra L16; `SDA Poultry Galore.pdf`

## Brightspace: Løsningsforslag til Domænemodel for Poultry Galore

Vedhæftet: `SDA Poultry Galore solution3.pdf`, `SDA Poultry Galore solution.pdf`. Der er to løsningsforslag til Poultry Galore. Af historiske årsager hedder løsning nr. 2 "nr. 3".
