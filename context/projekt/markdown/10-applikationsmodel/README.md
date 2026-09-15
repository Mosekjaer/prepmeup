# 10 — Applikationsmodel (L17–L21)

Fra use cases til software: System Application Model (SAM) med boundary/control/entity-klasser, sekvensdiagrammer, klassediagrammer og state machines. Metoden er step 1.1–1.4 (find klasser) og 2.1–2.5 (find interaktion) og udvides i tre versioner: simpel (Part 1), med hardware/IBD som input (Part 2), sammensatte systemer med delsystemer (Part 3). Underviser: Jørn Martin Hajek (HAJ); kursussider signeret /JMK. Brightspace-modul: "L18-L21 Applikationsmodel" (sidernes egne titler siger L17–L20).

Kildezip: `SWISE-01 ... - 1213 PM.zip` (zip 14 i konverteringen).

## Slides

| Fil | Kilde | Sider | Indhold |
|---|---|---|---|
| [system-application-models-1.md](system-application-models-1.md) | `System Application Models Part1.pdf` | 30 | AM's plads i processen, ECB-mønstret, Step 1.1–1.4 og 2.1–2.5, ATM-eksempel (SD/cd/STM for Withdraw Cash) |
| [system-application-models-2.md](system-application-models-2.md) | `System Application Models Part2.pdf` (+ .pptx, ingen noter) | 28 | Boundary-klasser fra HW-interfaces/IBD, systemsekvensdiagram som input, kommunikations- og designregler, fuldt ATM-klassediagram, control↔actor-guideline |
| [system-application-models-3.md](system-application-models-3.md) | `System Application Models Part3.pdf` | 24 | Sammensatte systemer: UC×subsystem-matrix, Tankstation BDD/IBD/domænemodel, afledning af Benzinstanderstyringens klasser, system-SD |
| [kursussider-l17-l21.md](kursussider-l17-l21.md) | Brightspace HTML | — | Indhold, forberedelse, øvelser per lektion |

## Øvelser og løsningsforslag

| Fil | Kilder | Indhold |
|---|---|---|
| [oevelse-atm-withdraw-cash.md](oevelse-atm-withdraw-cash.md) | `SAM_ATM UC description.pdf`, `SAM_ATM_Løsning.pdf` | Hjemmeopgave L17: UC Withdraw Cash → SD (main + ext 5.1 + ext 12.1), STM, klassediagram |
| [eksamensopgave-e2015-smartfridge.md](eksamensopgave-e2015-smartfridge.md) | `I2ISE Eksamensopgave E2015.pdf`, `SAM_SmartFridge_Opgave_Løsning.pdf` | Gammel eksamensopgave (opg. 2, 3A, 3B) + løsning: SD og klassediagram for "Tilføj vare" |
| [oevelse-benzinstander-applikationsmodel.md](oevelse-benzinstander-applikationsmodel.md) | `SAM_Benzinstander_Opgave.pdf`, `SAM_Benzinstander_Opgave_Løsning.pdf` | UC Optank Bil for Benzinstanderstyring og Benzinstander, udvidede løsninger, polling af boundary-klasse i loop |
| [oevelse-kamerasystem.md](oevelse-kamerasystem.md) | `L21_Kamerasystem Exercise.pdf` | Opg. 1–6 (opg. 6 = applikationsmodel). Ingen løsningsfil i materialet |
| [loesningssider-l17-l21.md](loesningssider-l17-l21.md) | Brightspace HTML (løsningssider) | Opgave → løsning-mapping og noter |

Relateret: domænemodellen for Benzinstationen i [../09-domaeneanalyse/](../09-domaeneanalyse/README.md); implementation af en applikationsmodel i [../11-implementation/](../11-implementation/README.md); SmartFridge BDD/IBD (F2015) i [../05-sysml/eksamensopgave-f2015-smartfridge.md](../05-sysml/eksamensopgave-f2015-smartfridge.md).

## Bemærkninger

- Alle mermaid-blokke i øvelsesfilerne er valideret med `mermaid.parse()`. Slide-filernes mermaid er skrevet efter samme syntaks, men ikke maskinvalideret.
- IBD/BDD gengives som tabeller (ingen mermaid-type). Sekvensdiagrammers state-invarianter og multiobjekt-notation er kun i noter/lifeline-navne.
- Inkonsistenser i originalerne bevaret og markeret: `insertCard()` vs `cardInserted(card)`, `NyPINCode`/`NyPINKode`, `Card` vs `CreditCard`, `Brændsstoftype`.
