# SW4SWD-01 — Lesson plan (15 uger)

## Metadata

| Felt | Værdi |
|---|---|
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Semester** | Efterår 2026, kalenderuge 35–50 |
| **Undervisere** | HK = Henrik Bitsch Kirk, HAJ = Jørn Martin Hajek |
| **Mandag** | 12.15–13.50, lokale 5125-430 |
| **Fredag** | 10.15–12.50, lokale 5123-111 |
| **Kilde** | `Lesson plan.html` (Brightspace) |

---

## Ugeplan

| # | Kal.uge | Mandag | Emne (mandag) | Fredag | Emne (fredag) |
|---|---|---|---|---|---|
| 1 | 35 | HK | Introduction + OO Basic | HK | Extreme Programming |
| 2 | 36 | HAJ | Design smells + SOLID: SRP, OCP | HAJ | SOLID: SRP, OCP |
| 3 | 37 | HK | SOLID: LSP, ISP, DIP | HK | SOLID: LSP, ISP, DIP |
| 4 | 38 | HK | Software Architecture | HK | Software Architecture |
| 5 | 39 | HAJ | Software Architecture | HAJ | Software Architecture |
| 6 | 40 | HK | Software design patterns + GoF Observer | HK | GoF Observer (fortsat) |
| 7 | 41 | HAJ | GoF Strategy + Template Method | HAJ | GoF Factory |
| — | 42 | — | **Efterårsferie — ingen undervisning** | — | **Efterårsferie** |
| 8 | 43 | HK | State patterns: switch/case, GoF State.<br>**Introduktion til obligatorisk opgave** | HK | State patterns: switch/case, GoF State.<br>**Deadline for valg af mønster** |
| 9 | 44 | HAJ | Refactoring | HAJ | Domain Driven Design |
| 10 | 45 | HK | Obligatorisk opgave | HAJ | Obligatorisk opgave — **aflevering fredag** |
| 11 | 46 | HAJ | Task (parallelle tasks) | HAJ | Parallel Loops |
| 12 | 47 | HAJ | Dependencies and Futures | HAJ | Pipelines |
| 13 | 48 | HK | Parallel Aggregation and MapReduce | HK | Error handling.<br>**Kursusevaluering** |
| 14 | 49 | HAJ | Error handling | HAJ/HK | **Afsluttende spørgsmål fra de studerende** |
| 15 | 50 | — | Buffer | — | Buffer |

---

## Kritiske datoer

| Uge | Hvad |
|---|---|
| 8 (kal. 43) | Deadline for valg af designmønster til den obligatoriske opgave — fredag |
| 10 (kal. 45) | Aflevering af obligatorisk opgave — fredag |
| 13 (kal. 48) | Kursusevaluering — fredag |

Alle obligatoriske opgaver skal være besvaret og godkendt for at blive indstillet til eksamen.

---

## Kursets tre faser

Undervisningen falder i tre naturlige blokke:

1. **Uge 1–5: Principper og arkitektur.** OO-grundlag, XP, design smells, de fem SOLID-principper, softwarearkitektur som proces og som dokumentation (4+1, C4).
2. **Uge 6–10: Designmønstre.** Observer, Strategy, Template Method, Factory Method, Abstract Factory, State — efterfulgt af refactoring og Domain Driven Design. Kulminerer i den obligatoriske mønster-opgave.
3. **Uge 11–14: Parallelitet og fejlhåndtering.** Tasks, parallelle loops, dependencies/futures, pipelines, aggregation/MapReduce, error handling.

Bemærk at C#-tråde (`11 Threading in C#.pdf`) er selvstudie og forudsættes læst inden uge 11.

---

## Sammenhæng med noterne

| Uge | Note |
|---|---|
| 0 | [../noter/SW4SWD-01_CSharp_Interfaces_Basics.md](../noter/SW4SWD-01_CSharp_Interfaces_Basics.md) *(selvstudie)* |
| 1 | `SW4SWD-01_W01.1a_Course_Intro.md`, `SW4SWD-01_W01.1b_OO_Basics.md`, `SW4SWD-01_W01.1c_UML_Class_Diagrams.md`, `SW4SWD-01_W01.2_Extreme_Programming.md` |
| 2 | `SW4SWD-01_W02a_Design_Smells.md`, `SW4SWD-01_W02b_SOLID_SRP_OCP.md` |
| 3 | `SW4SWD-01_W03a_SOLID_LSP.md`, `SW4SWD-01_W03b_SOLID_ISP_DIP.md` |
| 4 | `SW4SWD-01_W04.1_Architecture_Process_1.md`, `SW4SWD-01_W04.2_Architecture_Process_2.md` |
| 5 | `SW4SWD-01_W05_Architecture_Documentation.md` + artiklerne i `../artikler/` |
| 6 | `SW4SWD-01_W06a_Design_Patterns_Intro.md`, `SW4SWD-01_W06b_GoF_Observer.md` |
| 7 | `SW4SWD-01_W07.1_GoF_Template_Method_Strategy.md`, `SW4SWD-01_W07.2_GoF_Factory_Abstract_Factory.md` |
| 8 | `SW4SWD-01_W08a_GoF_State.md`, `SW4SWD-01_W08b_State_Nested_Orthogonal.md` |
| 9 | `SW4SWD-01_W09.1_Refactoring.md`, `SW4SWD-01_W09.2_Domain_Driven_Design.md` |
| 10 | *(obligatorisk opgave — intet slidemateriale)* |
| 11 | `SW4SWD-01_W10_Threading_in_CSharp.md` *(selvstudie)*, `SW4SWD-01_W11.1_Concurrency_Parallel_Tasks.md`, `SW4SWD-01_W11.2_Concurrency_Parallel_Loops.md` |
| 12 | `SW4SWD-01_W12.1_Concurrency_Dependencies_Futures.md`, `SW4SWD-01_W12.2_Concurrency_Pipelines.md` |
| 13–14 | `SW4SWD-01_W13.1_Concurrency_Aggregation_MapReduce.md`, `SW4SWD-01_W13.2_Error_Handling.md` |
