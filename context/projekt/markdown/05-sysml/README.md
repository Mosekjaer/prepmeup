# 05 — SysML (L6–L11)

Modelbaseret systemudvikling med SysML: Block Definition Diagram (BDD), Internal Block Diagram (IBD), Sequence Diagram (SD) og State Machine Diagram (STM). Underviser: Tommy Bjerre Nielsen (TBN); kursussider signeret /JMK. Brightspace-modul: "L7-11 SysML".

Bemærk to lektionsnumreringer: HTML-filnavnene siger L6 BDD / L7 BDD+IBD / L8 IBD / L9 SD / L10 STM, mens E25-lektionsplanen siger L10 BDD / L11 IBD / L12 SD / L13 STM. Mapping i [kursussider-l6-l11.md](kursussider-l6-l11.md).

Kildezip: `SWISE-01 ... - 1212 PM.zip` (zip 11 i konverteringen).

## Slides

| Fil | Kilde | Sider | Indhold |
|---|---|---|---|
| [sysml-introduction.md](sysml-introduction.md) | `SysML Introduction (F24).pdf` | 11 | SysML/MBSE, de fire pillars, diagram frame, SysML-vs-UML-taksonomi, kursets fire diagramtyper |
| [sysml-structural-diagrams-1-bdd.md](sysml-structural-diagrams-1-bdd.md) | `SysML Structural Diagrams 1.pdf` | 16 | Blokke og compartments, komposition, part names, hierarki, logisk→fysisk allokering, system context (Access Control System) |
| [sysml-structural-diagrams-2-ibd.md](sysml-structural-diagrams-2-ibd.md) | `SysML Structural Diagrams 2.pdf` | 25 | IBD, block vs part, items og item flows, port-taksonomi, atomic/nonatomic/conjugate flow ports, ASE-konventioner |
| [sysml-structural-diagrams-3.md](sysml-structural-diagrams-3.md) | `SysML Structural Diagrams 3.pdf` + Brightspace L8-prosa | 12 | Opsamling BDD+IBD trin for trin (User Interface, Secure Door), fuld løsning Access Control System |
| [sysml-sequence-diagrams.md](sysml-sequence-diagrams.md) | `SysML Behavioural Diagrams - Sequence Diagrams.pdf` | 17 | Lifelines, beskedtyper, activations, fragments (alt/opt/loop/par/ref), tid i SD, Parkeringsautomat- og RVM-øvelser |
| [sysml-state-machine-diagrams.md](sysml-state-machine-diagrams.md) | `SysML Behavioural Diagrams - State Machine Diagrams.pdf` | 19 | States/transitions, entry/do/exit, guards/effects, choice, nested states, regioner, Bridge Control System, egg timer-øvelser |
| [sysml-quick-guide.md](sysml-quick-guide.md) | `SysMLQuickGuide.pdf` (scannet) | 17 | Notationstabeller fra Friedenthal-appendix (BDD/IBD/SD/STM/Activity) |
| [kursussider-l6-l11.md](kursussider-l6-l11.md) | Brightspace HTML | — | Indhold, materialer, læsestof, forkortelser, værktøjer (UMLet/Draw.io), numreringsmapping |

## Øvelser og løsningsforslag

| Fil | Kilder | Indhold |
|---|---|---|
| [oevelse-parkeringsautomat.md](oevelse-parkeringsautomat.md) | `(Exercise) SysML Structural Parkeringsautomat Ovelse.pdf`, `Parkeringsautomat_BDD_Løsning1/2.pdf`, `Pakeringsautomat_IBD_Løsning.pdf`, `(solution)Parkeringsautomat_SD.pdf` | Opgave + BDD (to varianter), IBD og SD |
| [oevelse-rvm-sekvensdiagram.md](oevelse-rvm-sekvensdiagram.md) | `(solution)RVM_SD.pdf`, `(solution)RVM_SD_withExtraRVMisFull.pdf` | Reverse Vending Machine SD, med/uden "RVM is full" |
| [oevelse-access-control-ibd.md](oevelse-access-control-ibd.md) | `AccessControlSystem_IBD.pdf` | IBD-løsning (opgavetekst findes i Structural 1/3) |
| [eksamensopgave-f2015-smartfridge.md](eksamensopgave-f2015-smartfridge.md) | `I2ISE Eksamensopgave F2015.pdf`, `SYSMLBDDSmartFridgeSolution.pdf`, `SYSMLIBDSmartFridgeSolution.pdf` | Gammel eksamensopgave (opg. 1–6) + BDD/IBD-løsning til opg. 5–6 |
| [oevelse-egg-timer-state-machine.md](oevelse-egg-timer-state-machine.md) | `StateEggTimerSolutionF2019.pdf`, `StatePimpedEggTimerSolutionF2019.pdf`, `(L10Ex2)StatePimedEggTimerSolution_MultiRegions_E23.pdf` | ET2000/PET3000 STM inkl. multiregion |
| [oevelse-telefon-state-machine.md](oevelse-telefon-state-machine.md) | `SysML State Machines (telefon).pdf`, `SysML SD Løsningsforslag (telefon1/2).pdf` | Telefon-STM, to løsninger (flad / composite med regioner) |
| [oevelse-konsol-state-machine.md](oevelse-konsol-state-machine.md) | `SysML State Machines (konsol).pdf` + løsningsforslag | Aflæsning af konsol-output fra STM med regioner og choice |
| [loesningssider-l6-l11.md](loesningssider-l6-l11.md) | Brightspace HTML (løsningssider) | Øvelse → løsningsfil-mapping, .uxf = UMLet |

## Referencecase: BeoSound F

| Fil | Kilde | Indhold |
|---|---|---|
| [beosound-f-concept-report.md](beosound-f-concept-report.md) | `BeoSoundF_ConceptReport.pdf` (49 s.) | Fuld konceptrapport: scenarier, kravtabel (43 krav), House of Quality, SWOT, elektronik, datablade, Business Model Canvas |
| [beosound-f-bdd-ibd.md](beosound-f-bdd-ibd.md) | `BeosoundF BDD.pdf`, `BeoSoundF_BDD_IBD.pdf` | BDD (to versioner) og IBD som tabeller |

## Kendte fejl i originalslides (bevaret, markeret i filerne)

- ibd Access Control System (Structural 3, slide 11) og ibd User Interface (SD-slides, slide 11; Parkeringsautomat-IBD-løsning): rød/grøn port krydser til modsat LED/knap.
- Konsol-løsning: manglende "Exit B" (opg. 2) og "Enter A" (opg. 3).
- Stavefejl i kilderne (`Pakeringsautomat`, `Acesss`, `Toucshskærm` m.fl.) bevaret med *(sic)*.

Ikke i materialet: opgavetekst til Access Control System som selvstændig fil; `Parkeringsautomat_*.uxf` (UMLet-kildefiler) er ikke konverteret.
