# Brightspace-kursussider L6–L11 (SysML)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L6 — SysML BDD; L7 — SysML BDD/IBD; L8 — SysML IBD; L9 — SysML SD; L10 — SysML STM |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | Brightspace-siderne `L6 SysML BDD`, `L7 SysML BDDIBD`, `L8 SysML IBD`, `L9 SysML SD`, `L10 SysML STM` (HTML-udtræk) |
| **Type** | kursusside |
| **Emner dækket** | Indhold, materialer, læsestof, øvelser og forkortelser per lektion; rækkefølge-note for F25; værktøjer (UMLet, Draw.io); referencebøger |

---

## Rækkefølge-note

Øverst på L6-siden står:

> "Det er for L7 i rækkefølge, men Vi tage L6 i F25. L7 i F25 er Accepttest Krav (original rækkefølge er L5 Accepttest Krav)."

Dvs. SysML-introduktionen var oprindeligt planlagt som L7, men blev i F25 afholdt som L6, og Accepttest/Krav (oprindeligt L5) blev rykket til L7.

**To numreringer i materialet.** HTML-filnavnene (`L6_SysML_BDD`, `L7_SysML_BDDIBD`, `L8_SysML_IBD`, `L9_SysML_SD`, `L10_SysML_STM`) er én numrering; Brightspace-indholdsfortegnelsen for E25 ("L7-11 SysML") er en anden, forskudt med én fordi L7 er Accepttest/Krav:

| Emne | HTML-filnavn | Brightspace ToC (E25) | Løsningsside (ToC E25) |
|---|---|---|---|
| SysML intro + BDD | L6 | L6 SysML BDD | L7 SysML BDD Løsning |
| Flow ports, connections, IBD | L7 | L8 SysML BDD+IBD | L8 SysML IBD Løsninger |
| IBD opsamling (Structural Diagrams 3) | L8 | L9 SysML IBD | L9 SysML BDD og IBD Løsninger |
| Sequence Diagrams | L9 | L10 SysML SD | L10 SysML SD Løsninger |
| State Machine Diagrams | L10 | L11 SysML STM | L11 SysML STM Løsninger |

Filerne i denne mappe bruger HTML-filnavnenes numrering i metadata (L6 = BDD, L7 = BDD/IBD, L8 = IBD, L9 = SD, L10 = STM); E25-nummeret står i parentes.

> Brightspace L6 + Table of Contents

## L6 — SysML BDD

**Materials**

| Fil | Note |
|---|---|
| `SysMLQuickGuide.pdf` | selvlæsning |
| `SysML Introduction.pdf` | — |
| `SysML Structural Diagrams 1.pdf` | — |
| Exercise: `SysML Structural Parkeringsautomat Ovelse.pdf` | Kun nr. 1 (BDD) |
| Ref: BeoSoundF beskrivelse: `BeoSoundF_ConceptReport.pdf` | selv |
| Ref: `BeosoundF BDD.pdf` | selv |

**Indhold:** SysML Intro & BDD
- Wrap up summary: Krav & Test (done on L6 på torsdags)
- Introduktion til SysML
- Introduktion til struktur og adfærdsbegreberne
- Strukturdiagrammer: Block Definition Diagram (BDD)

**Forkortelser (fra siden):**

| Forkortelse | Betydning |
|---|---|
| MBSE | Model Based System Engineering (grafiske modeller bruges i stedet for kun tekst) |
| UML | Unified Modelling Language (modeller for software) |
| SysML | System Modelling Language (modeller for systemer) |
| VHDL | Programmeringssprog til hardware |

**Læsestof:**
- SysML Diagram: https://sysml.org/tutorials/sysml-diagram-tutorial/
- UML Diagram: https://developer.ibm.com/articles/the-class-diagram/
- UML connectors: lektionsslide

**Referencebøger:**
- *Systems Engineering with SysML/UML*, Chapter 4 — SysML—The Systems Modeling Language
- *A Practical Guide to SysML*, 2015

OBS fra siden: "Men kun som reference for at forstå begreberne, uden at følge den måde med de komplekse metoder fra de eksempler i bogen. Fokus på lektionsslides skal være, hvordan man laver diagrammer og anvender jeres semesterprojekt og obligatoriske opgaver i ISE. Vi bruger ikke det hele præcist fra reference bogen eller internet referencer!"

**Værktøj:** I undervisningsmaterialet er der brugt UMLet til SysML- og UML-diagrammer. UMLet kan også bruges til at tegne SysML- og UML-diagrammer: vælg UMLet standalone (link: UMLet Change Log).

Signeret /JMK.

> Brightspace L6

## L7 — SysML BDD/IBD

**Slides**
- `SysML Structural Diagrams 2.pdf`
- Exercise: `SysML Structural Parkeringsautomat Ovelse.pdf`

**Reference (selvlæsning)**
- BeoSoundF beskrivelse (fra L1): `BeoSoundF_ConceptReport.pdf`
- BeoSoundF BDD & IBD: `BeoSoundF_BDD_IBD.pdf`

**Indhold:** Strukturdiagrammer
- Flow Porte og Connections
- Interne Block Diagrammer (IBD)

**Arbejde på klassen:** Parkeringsautomat øvelse 1 — tegn et ibd-diagram (`SysML Structural Parkeringsautomat Ovelse.pdf`).

**Værktøjer:** Til at tegne SysML-diagrammer kan bruges UMLet, Draw.io.

**Læsestof:**
- SysML Diagram: https://sysml.org/tutorials/sysml-diagram-tutorial/
- UML Diagram: https://developer.ibm.com/articles/the-class-diagram/
- UML connectors: lektionsslide

Signeret /JMK.

> Brightspace L7

## L8 — SysML IBD

**Materials:** `SysML Structural Diagrams 3.pdf`

Siden indeholder en lang forklarende tekst (introduktion, IBD, connectors/item flows/flow ports, atomic og nonatomic flow ports, porte til omverdenen, Access Control System-eksemplet og øvelsen "lav et IBD for jeres semesterprojekt"). Teksten er indarbejdet afsnit for afsnit i `sysml-structural-diagrams-3.md`.

**Øvelse (kort):** Lav et Internal Block Diagram for jeres semesterprojekt med udgangspunkt i BDD'et fra sidste gang. Skil eventuelle forbindelser ud af BDD'et og lav et separat, rent IBD.

> Brightspace L8

## L9 — SysML SD

**Materials**
- `SysML Behavioural Diagrams - Sequence Diagrams.pdf`
- Video for SysML — SD

**Indhold:** Adfærdsdiagrammer
- Sequence Diagram (SD)
- Eksempler og øvelser

**Læsestof:**
- https://en.wikipedia.org/wiki/Sequence_diagram
- https://sysml.org/sysml-faq/what-is-sequence-diagram.html

> Brightspace L9

## L10 — SysML STM

**Materials:** `SysML Behavioural Diagrams - State Machine Diagrams.pdf`

**Indhold:** Adfærdsdiagrammer
- State Machine Diagram (STM)

**Klasse- og hjemmeopgaver:** Eksempler (jf. slides: SysML State Machines (konsol).pdf og SysML State Machines (telefon).pdf).

> Brightspace L10

## L11

I E25-indholdsfortegnelsen er L11 = SysML STM (samme side som HTML-filen `L10_SysML_STM` ovenfor). Løsningssiden hedder tilsvarende "L11 SysML State Machine (STM) Exercise & Løsningsforslag" og lister: Egg Timer (`StateEggTimerSolutionF2019.pdf`), Pimped Egg Timer multi-regions (`(L10Ex2)StatePimedEggTimerSolution_MultiRegions_E23.pdf`), telefon-øvelsen med to løsningsforslag, og konsol-øvelsen med løsningsforslag. Løsningssiderne dækkes af `loesningssider-l6-l11.md` og øvelsesfilerne i denne mappe.

## Oversigt: slides per lektion

| Lektion (HTML / E25 ToC) | Slides | Markdown-fil |
|---|---|---|
| L6 / L6 | SysML Introduction (F24), SysML Structural Diagrams 1, SysMLQuickGuide | `sysml-introduction.md`, `sysml-structural-diagrams-1-bdd.md`, `sysml-quick-guide.md` |
| L7 / L8 | SysML Structural Diagrams 2 | `sysml-structural-diagrams-2-ibd.md` |
| L8 / L9 | SysML Structural Diagrams 3 | `sysml-structural-diagrams-3.md` |
| L9 / L10 | SysML Behavioural Diagrams — Sequence Diagrams | `sysml-sequence-diagrams.md` |
| L10 / L11 | SysML Behavioural Diagrams — State Machine Diagrams | `sysml-state-machine-diagrams.md` |
