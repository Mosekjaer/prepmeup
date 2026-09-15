# SWISE-01 Indledende System Engineering / SW4PRJ4-02 Projekt 4 — kursusmateriale som markdown

Alt Brightspace-materiale fra SWISE-01 (undervist som del af SW4PRJ4-02 Projekt 4, AU, 4. semester diplomingeniør i softwareteknologi, efterår 2026) plus ISE-kompendiet konverteret til markdown og struktureret efter lektionsplanen. Beregnet som AI-context og til semesterprojekt- og eksamensforberedelse.

Eksamen: **20 minutters mundtlig projekteksamen med ekstern censur, 7-trinsskala**, på baggrund af semesterprojektrapporten. Forudsætning: obligatoriske opgaver A, B og C godkendt. Semesterprojekt afleveres fredag 11/12 2026 kl. 13.

---

## Start her

| Fil | Hvad |
|---|---|
| [00-kursus/sw4prj4-introduktion.md](00-kursus/sw4prj4-introduktion.md) | Læringsmål, eksamensform, iterationer, krav til projektopgaven, tools |
| [00-kursus/lektionsplan.md](00-kursus/lektionsplan.md) | L1–L28 med emne, aflevering A/B/C, feedback, underviser |
| [00-kursus/semesterplan.md](00-kursus/semesterplan.md) | Uge for uge, deadlines, ferie, eksamensperiode |
| [00-kursus/semestergrupper.md](00-kursus/semestergrupper.md) | Grupper, vejledere, projekter (Frederik: gruppe 3) |
| [00-kursus/velkommen-til-swise.md](00-kursus/velkommen-til-swise.md) | L1: hvad er system engineering, ECE-modellen, SysML-overblik |

Obligatoriske afleveringer (Pakkeboksen): [A — specifikation og validering](00-kursus/afleveringsopgave-a-pakkeboksen.md) · [B — SysML struktur og adfærd](00-kursus/afleveringsopgave-b-pakkeboksen.md) · [C — domæne- og applikationsmodel](00-kursus/afleveringsopgave-c-pakkeboksen.md) · [forsider](00-kursus/forsider.md)

---

## Lektion for lektion

| Lektion | Emne | Modul | Bog |
|---|---|---|---|
| 1 | Intro, hvad er SE, ECE-modellen | [00-kursus](00-kursus/README.md) | — |
| 2 | Kravspecifikation, FURPS+, MoSCoW, traceability | [01-kravspecifikation](01-kravspecifikation/README.md) | [Larman kap. 5](bog/01-larman-ch5-requirements.md) |
| 3–4 | Use cases, fully dressed UC, aktører, scenarier | [02-use-cases](02-use-cases/README.md) | [Larman kap. 6](bog/02-larman-ch6-use-cases.md) |
| 5 | Systemtest, accepttest, testspecifikation | [03-systemtest](03-systemtest/README.md) | [Peckol kap. 10](bog/03-peckol-ch10-hardware-test-debug.md), [SPU softwaretest](bog/04-spu-vejledning-softwaretest.md) |
| 6 | Projektledelse, WBS, PERT, risikoanalyse | [07-projektledelse](07-projektledelse/README.md) | [Vinje projektledelse](bog/14-vinje-projektledelse.md) |
| 7 | Kvalitetssikring, review, konfigurationsstyring | [08-kvalitetssikring](08-kvalitetssikring/README.md) | [SPU review](bog/06-spu-vejledning-review.md) |
| 8–9 | Udviklingsprocesser, Kanban, Scrum | [04-udviklingsprocesser](04-udviklingsprocesser/README.md) | [Vinje udviklingsprocesser](bog/13-vinje-udviklingsprocesser.md) |
| 10–13 | SysML: BDD, IBD, SD, STM | [05-sysml](05-sysml/README.md) | [Friedenthal kap. 3](bog/05-friedenthal-ch3-sysml.md) |
| 14 | Domæneanalyse, domænemodeller | [09-domaeneanalyse](09-domaeneanalyse/README.md) | [Larman kap. 9](bog/07-larman-ch9-domain-models.md) |
| 15–16 | Applikationsmodel (boundary/control/entity, SD, cd, STM) | [10-applikationsmodel](10-applikationsmodel/README.md) | — |
| 17 | SW design → implementation (minutur på Arduino og WPF) | [11-implementation](11-implementation/README.md) | [Vahid kap. 1](bog/12-vahid-ch1-embedded-systems-overview.md) |
| 18 | Arkitektur → systemdesign, interfaces, coupling/cohesion | [06-arkitektur-og-design](06-arkitektur-og-design/README.md) | [Peckol kap. 9](bog/08-peckol-ch9-system-design.md), [Vahid interfacing](bog/09-vahid-ch6-interfacing-137-153.md) |
| 20 | Protokoller: OSI, UART/I2C/SPI, Ethernet, HTTP | [12-protokoller](12-protokoller/README.md) | [Vahid serielle protokoller](bog/10-vahid-ch6-interfacing-166-169.md), [Peckol netværk](bog/11-peckol-ch16-7-network-architecture.md) |
| 23, 27–28 | Projektrapport, rapportskrivning, litteratursøgning, LaTeX | [13-rapport](13-rapport/README.md) | — |

Lektionsnumrene følger E25-lektionsplanen. Brightspace-sidernes egne titler bruger ældre numre (fx "L6 SysML BDD", "L17 Applikationsmodel"); mappingen står i hvert moduls README.

## Moduler

| Mappe | Indhold | Filer |
|---|---|---|
| [00-kursus/](00-kursus/README.md) | Kursusfakta, planer, undervisere, forsider, afleveringsopgave A/B/C, L1-slides | 12 |
| [01-kravspecifikation/](01-kravspecifikation/README.md) | System Specification-slides, Vasa-casen | 3 |
| [02-use-cases/](02-use-cases/README.md) | Use case-slides, Bilvaskehal-case, fully dressed UC-skabelon | 4 |
| [03-systemtest/](03-systemtest/README.md) | System Test-slides, TTT-accepttestspecifikation (eksempel), accepttest-øvelse | 4 |
| [04-udviklingsprocesser/](04-udviklingsprocesser/README.md) | Development Processes, Kanban, Scrum Guide 2016 | 4 |
| [05-sysml/](05-sysml/README.md) | Intro, BDD, IBD, SD, STM, quick guide, 7 øvelser m. løsninger, eksamensopgave F2015, BeoSound F-case | 19 |
| [06-arkitektur-og-design/](06-arkitektur-og-design/README.md) | System Architecture and Design, System Design and Interfaces | 3 |
| [07-projektledelse/](07-projektledelse/README.md) | Project Management-slides, øvelser | 2 |
| [08-kvalitetssikring/](08-kvalitetssikring/README.md) | Quality Management-slides | 2 |
| [09-domaeneanalyse/](09-domaeneanalyse/README.md) | System Domain Analysis, artikel + gennemarbejdet eksempel, Benzinstander- og Poultry Galore-øvelser | 6 |
| [10-applikationsmodel/](10-applikationsmodel/README.md) | SAM Part 1–3, ATM/SmartFridge/Benzinstander/Kamerasystem-øvelser, eksamensopgave E2015 | 10 |
| [11-implementation/](11-implementation/README.md) | UML-Light-Ur, applikationsmodel → Arduino (C, C++, C++ m. interrupts) og WPF, al kode | 9 |
| [12-protokoller/](12-protokoller/README.md) | SWISE Protocols-slides | 2 |
| [13-rapport/](13-rapport/README.md) | L27 Projektrapport, God rapportskrivning, Lean dokumentation, AU ECE-rapportskabelon, Overleaf | 6 |
| [bog/](bog/README.md) | ISE-kompendiet: 14 lærebogsuddrag, struktureret + [fuldtekst](bog/fuldtekst/) | 29 |

## Gamle eksamensopgaver

| Sæt | Hvor | Dækker |
|---|---|---|
| I2ISE F2015 (SmartFridge) | [05-sysml/eksamensopgave-f2015-smartfridge.md](05-sysml/eksamensopgave-f2015-smartfridge.md) | Opg. 1–6 med BDD/IBD-løsning |
| I2ISE E2015 (SmartFridge) | [10-applikationsmodel/eksamensopgave-e2015-smartfridge.md](10-applikationsmodel/eksamensopgave-e2015-smartfridge.md) | Opg. 2, 3A, 3B med domænemodel, IBD, SD, cd-løsning |

Bemærk: disse er fra da ISE var et selvstændigt kursus med skriftlig eksamen. Nu er eksamen en mundtlig projekteksamen.

---

## Om konverteringen

Kilderne (15 Brightspace-zips, `SWISE Protocols.pdf` og `ISE Book/` som LaTeX) ligger i [`../kilder/`](../kilder/). Konverteret september 2026.

**Slides** er renderet til PNG og læst visuelt, suppleret af `pdftotext` for ordret kode og identifiers. SysML-diagrammer (BDD/IBD) har ingen mermaid-type og gengives som tabeller (blok | parts | ports | values) plus connector-/item flow-tabeller. Sekvensdiagrammer, state machines og klassediagrammer er gengivet som mermaid **kun hvor de var entydigt læsbare**; ellers tabel eller prosa. Alle 178 mermaid-blokke er parset med `mermaid.parse()` uden fejl. Slide-referencer (`> Slide N`) peger tilbage i kilde-PDF'en.

**1-sides diagram-PDF'er** (løsningsforslag) er renderet ved 110–300 dpi og beskrevet præcist: blokke, ports, connectors, states, transitions, guards, actions.

**Bogkapitler** findes i to udgaver: struktureret (overskrifter, tabeller, figurer som mermaid/tabel, prosa sammenfattet) og fuldtekst (mekanisk pandoc `latex → gfm`, ordret). Se [bog/README.md](bog/README.md).

**Kode** (minutur i C, C++, C++ m. interrupts, WPF) er gengivet fuldt ud i fenced code blocks.

**Fejl i originalerne** (krydsede ports i IBD'er, stavefejl, inkonsistente lektionsnumre, OCR-skader i kompendiet) er bevaret og markeret, ikke rettet. Hvert moduls README lister dem.

**Ikke konverteret:** Brightspace-videoer (kun embed), `.uxf` UMLet-kildefiler, projektfiler i kodezips, og filer der er nævnt på Brightspace men ikke i eksporten (`AcceptTestOvelseLosning.pdf`, `System Design and Interfaces with Solution.pdf`, Kamerasystem-løsning, Windows Forms-versionen af minuturet, opgave D).

Sprog: dansk med engelske fagtermer. Bogtekst er ikke oversat. Diagramnavne (BDD, IBD, SD, STM), mønsternavne og identifiers er bevaret ordret.
