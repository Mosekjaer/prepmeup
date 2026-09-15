# Eksempel: Accepttestspecifikation — Tic Tac Toeminator (TTT)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L5/L7 — Systemtest og accepttest |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `TTT_Accepttestspecifikation.pdf` (13 sider) |
| **Type** | eksempel (accepttestspecifikation fra et 3.-semesterprojekt, AU Ingeniørhøjskolen, gruppe 8, 22.12.2017, vejleder Peter Høgh Mikkelsen) |
| **Emner dækket** | Accepttest pr. use case-scenarie (hovedscenarie, EXT, EXC), testtabel med Handling / Forventet / Faktisk / Vurdering, test af ikke-funktionelle krav med måleudstyr |

---

## Forside

**Tic Tac Toeminator** — Semesterprojektgruppe: 8 — **Accepttestspecifikation**

| Studienummer | Deltager |
|---|---|
| 1# 201609213 | Jakob Rune Skov |
| 2# 201608892 | Jesper Mundbjerg Madsen |
| 3# 201607598 | Mathias Kirkeby |
| 4# 201605348 | Morten Dahl Nielsen |
| 5# 201605961 | Morten Rahr Nielsen |
| 6# 201608798 | Simon Fabricius Nielsen |
| 7# 201609922 | Tobias Saaby Steffensen |
| 8# 201600313 | Rune Bjørn Lassen |

Vejleder: Peter Høgh Mikkelsen. Dato: 22.12.17.

> Side 1 (forside)

## Indholdsfortegnelse

1. Use Case 1: Initier Spil
2. Use Case 2: Spil "Finite Mode"
   - 2.1 EXT1: Systemet er spilstarter og dette er første træk
   - 2.2 EXC1: System registrerer tre på stribe (Bruger)
   - 2.3 EXC2: Systemet registrerer, at pladen er fyldt
   - 2.4 EXC3: Systemet registrerer tre på stribe (TTT)
3. Use Case 3: Spil "Infinity Mode"
   - 3.1 EXT1: Brugeren er spilstarter og dette er første træk
   - 3.2 EXT2: Seks brikker registreret på spillepladen (Bruger)
   - 3.3 EXT3: Seks brikker registreret på spillepladen (TTT)
   - 3.4 EXC1: System har vundet på Easy/Medium
   - 3.5 EXC2: System har vundet på Hard
   - 3.6 EXC3: Bruger sætter sin brik samme sted som han tog den fra
   - 3.7 EXC4: Bruger har vundet
4. Test af ikke-funktionelle krav
   - 4.1 Fysiske dimensioner
   - 4.2 GUI
   - 4.3 Andet

Tabeller 1–17 (én accepttesttabel pr. scenarie, se nedenfor).

> Side 1

## Ordforklaring

| Forkortelse | Forklaring |
|---|---|
| Arm | Robotarm |
| EXC | Exception |
| EXT | Extension |
| FM | Finite Mode, spillemode til pladen fuld |
| GUI | Graphical User Interface |
| IM | Infinity Mode, spillemode med tre brikker til hver spiller |
| KB | Kryds og bolle |
| TTT | Tic Tac Toeminator |

> Side 2

## Accepttestspecifikation

I dette dokument er accepttesten for systemet specificeret og udfyldt jf. dokumentet Kravspecifikation.

### 1 Use Case 1: Initier Spil

| | |
|---|---|
| **Use Case under test:** | Initier Spil |
| **Scenarie:** | Hovedscenarie |
| **Prækondition:** | Systemet er funktionsdygtigt og spillepladen er tom |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Tilslut systemet til strøm | Systemet tænder og viser "Play Tic Tac Toeminator" skærmen på GUI | Skærmen viser "Play Tic Tac Toeminator" | OK |
| 2 | Vælg "Finite Mode" | Systemet bekræfter Finite Mode som spiltype via GUI | Skærmen viser "Finite" oppe i venstre hjørne | OK |
| 3 | Vælg "Hard mode" | Systemet bekræfter Hard Mode som sværhedsgrad via GUI | Skærmen viser "Hard" oppe i venstre hjørne | OK |
| 4 | Vælg "I'll go first" | Systemet bekræfter og viser "Game in progress... GL & HF!" via GUI | Skærmen viser "Game in progress... GL & HF!" | OK |
| 5 | Vælg "Start game" | Robotarmen vinker | Robotarmen bevæger sig | OK |

*Tabel 1: Accepttest for Use Case 1: Initier Spil*

> Side 3

### 2 Use Case 2: Spil "Finite Mode"

| | |
|---|---|
| **Use Case under test:** | Spil "Finite Mode" |
| **Scenarie:** | Hovedscenarie |
| **Prækondition:** | Systemet er tilsluttet strøm og er initieret. Mode er valgt til "Finite Mode", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Vælg "Start game" | Robotarmen vinker | Robotarmen bevæger sig | OK |
| 2 | Sæt brik på plads 5 | Robotarmen placerer sin brik på plads 2 | Robotarmen placerer sin brik på plads 2 | OK |

*Tabel 2: Accepttest for Use Case 2: Spil "Finite Mode"*

> Side 3

#### 2.1 Use Case 2: EXT1: Systemet er spilstarter og dette er første træk

| | |
|---|---|
| **Use Case under test:** | Spil "Finite Mode" |
| **Scenarie:** | EXT1: Systemet er spilstarter og dette er første træk |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Finite Mode", "Hard Mode" samt "TTT goes first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Vælg "Start game" | Robotarmen vinker og placerer sin brik på plads 0 | Robotarmen bevæger sig og placerer sin brik på plads 0 | OK |
| 2 | Sæt brik på plads 4 | Robotarmen placerer sin brik på plads 1 | Robotarmen placerer sin brik på plads 1 | OK |

*Tabel 3: Accepttest for Use Case 2: EXT1: Systemet er spilstarter og dette er første træk*

> Side 4

#### 2.2 Use Case 2: EXC1: System registrerer tre på stribe (Bruger)

| | |
|---|---|
| **Use Case under test:** | Spil "Finite Mode" |
| **Scenarie:** | EXC1: System registrerer tre på stribe (Bruger) |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Finite Test", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 1, 4 og 6 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 2, 3 og 8 og vinker | Robotarmen placerer sine brikker på pladserne 2,3 og 8 og bevæger sig | OK |
| 2 | Sæt brik på plads 7 | Systemet viser vinderskærm for FM | Systemet viser vinderskærm for FM | OK |

*Tabel 4: Accepttest for Use Case 2: EXC1: System registrerer tre på stribe (Bruger)*

> Side 4

#### 2.3 Use Case 2: EXC2: Systemet registrerer, at pladen er fyldt

Easy- og Medium Mode er til en vis grad tilfældige og det kan derfor tage op til flere forsøg at få testet denne Use Case for disse sværhedsgrader.

| | |
|---|---|
| **Use Case under test:** | Spil "Finite Mode" |
| **Scenarie:** | EXC2: Systemet registrerer, at pladen er fyldt |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Finite Mode", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 1, 4 og 6 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 2, 3 og 8 og vinker | Robotarmen placerer brikker på pladserne 2, 3 og 8 og bevæger sig | OK |
| 2 | Sæt brik på plads 5 | Robotarmen placerer sin brik på plads 7 | Robotarmen placerer sin brik på plads 7 | OK |
| 3 | Sæt brik på plads 0 | Systemet viser uafgjortskærm | Systemet viser uafgjortskærm | OK |

*Tabel 5: Accepttest for Use Case 2: EXC2: Systemet registrerer, at pladen er fyldt*

> Side 5

#### 2.4 Use Case 2: EXC3: Systemet registrerer tre på stribe (TTT)

Easy- og Medium Mode er til en vis grad tilfældige og det kan derfor tage op til flere forsøg at få testet denne Use Case for disse sværhedsgrader.

| | |
|---|---|
| **Use Case under test:** | Spil "Finite Mode" |
| **Scenarie:** | EXC3: Systemet registrerer tre på stribe (TTT) |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Finite Mode", "Hard Mode" samt "TTT goes first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 1, 4 og 6 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 2, 3 og 8 og vinker | Robotarmen placerer sine brikker på pladserne 2, 3 og 8 og bevæger sig | OK |
| 2 | Vent 5 sekunder | Robotarmen placerer sin brik på plads 5 og viser taberskærm for FM | Robotarmen placerer sin brik på plads 5 og viser taberskærm for FM | OK |

*Tabel 6: Accepttest for Use Case 2: EXC3: Systemet registrerer tre på stribe (TTT)*

> Side 5

### 3 Use Case 3: Spil "Infinity Mode"

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | Hovedscenarie |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Infinity Mode", "Hard Mode" samt "TTT goes first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Vælg "Start game" | Robotarmen vinker og placerer en brik på plads 0 | Robotarmen bevæger sig og placerer en brik på plads 0 | OK |

*Tabel 7: Accepttest for Use Case 3: Spil "Infinity Mode"*

> Side 6

#### 3.1 Use Case 3: EXT1: Brugeren er spilstarter og dette er første træk

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXT1: Brugeren er spilstarter og dette er første træk |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Infinity Mode", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Vælg "Start game" | Robotarmen vinker | Robotarmen bevæger sig | OK |
| 2 | Sæt brik på plads 5 | Robotarmen placerer sin brik på plads 2 | Robotarmen placerer sin brik på plads 2 | OK |

*Tabel 8: Accepttest for Use Case 3: EXT1: Brugeren er spilstarter og dette er første træk*

> Side 6

#### 3.2 Use Case 3: EXT2: Seks brikker registreret på spillepladen (Bruger)

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXT2: Seks brikker registreret på spillepladen (Bruger) |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Infinity Mode", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 5, 7 og 8 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og vinker | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og bevæger sig | OK |
| 2 | Fjern brikken på plads 5 og placer den på plads 4 | Robotarmen fjerner brikken på plads 3 og placerer den på plads 2 samt viser taberskærm for IM | Robotarmen fjerner brikken på plads 3, placerer den på plads 2 og viser taberskærm for IM | OK |

*Tabel 9: Accepttest for Use Case 3: EXT2: Seks brikker registreret på spillepladen (Bruger)*

> Side 6

#### 3.3 Use Case 3: EXT3: Seks brikker registreret på spillepladen (TTT)

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXT3: Seks brikker registreret på spillepladen (TTT) |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Infinity Mode", "Hard Mode" samt "TTT goes first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 5, 7 og 8 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og vinker | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og bevæger sig | OK |
| 2 | Vent 5 sekunder | Robotarmen fjerner brikken på plads 3 og placerer den på plads 2 samt viser taberskræm for IM | Robotarmen fjerner brikken på plads 3, placerer den på plads 2 og viser taber skærm for IM | OK |

*Tabel 10: Accepttest for Use Case 3: EXT3: Seks brikker registreret på spillepladen (TTT)*

> Side 7

#### 3.4 Use Case 3: EXC1: System har vundet på Easy/Medium

I Hard Mode kendes robotarmens træk, da den altid fjerner den dårligste brik og placerer den bedste. I Easy og Medium Mode vil den derimod altid fjerne en tilfældig brik, hvilket kan medføre at testen skal gennemgås flere gange før det fornødne resultat opnås. I Hard Mode sendes en værdi tilbage til GUI'en, som skal gemmes på highscoren.

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXC1: System har vundet på Easy/Medium |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Infinity Mode", "Medium Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 5, 7 og 8 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og vinker | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og bevæger sig. | OK |
| 2 | Fjern brikken på plads 5 og placer den på plads 4 | Robotarmen fjerner en tilfældig af de tre brikker den har på spillepladen og placerer den det bedst mulige sted | Robotarmen tager brikken fra plads 3 og placerer den på plads 2 | (OK) |
| 3 | Vent 5 sekunder | Systemet viser taberskærm for FM | Systemet viser taberskærm for FM | OK |

*Tabel 11: Accepttest for Use Case 3: EXC1: System har vundet på Easy/Medium*

> Side 7

#### 3.5 Use Case 3: EXC2: System har vundet på Hard

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXC2: System har vundet på Hard |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Infinity Mode", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 5, 7 og 8 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og vinker | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og bevæger sig | OK |
| 2 | Fjern brikken på plads 5 og placer den på plads 4 | Robotarmen fjerner brikken på plads 3 og placerer den på plads 2 | Robotarmen fjerner brikken på plads 3 og placerer den på plads 2 | OK |
| 3 | Vent 5 sekunder | Systemet viser taberskærm for IM | Systemet viser taberskærm for IM | OK |
| 4 | Indtast "Testnavn" | Systemet gemmer navn og score på highscorelisten | Systemet gemmer Testnavn og score = 7 på highscorelisten | OK |

*Tabel 12: Accepttest for Use Case 3: EXC2: System har vundet på Hard*

> Side 8

#### 3.6 Use Case 3: EXC3: Bruger sætter sin brik samme sted som han tog den fra

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXC3: Bruger sætter sin brik samme sted som han tog den fra |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Infinity Mode", "Hard Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 5, 7 og 8 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og vinker | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og bevæger sig | OK |
| 2 | Fjern brikken på plads 5, placer den på plads 5 og vent 1 minut | Systemet viser teksten "Der er opstået en kritisk situation" via GUI'en | Systemet viser teksten "Der er opstået en kritisk situation" via GUI'en | OK |
| 3 | Vælg "New Game" | Systemer viser hovedvinduet via GUI'en | Systemer viser hovedvinduet via GUI'en | OK |

*Tabel 13: Accepttest for Use Case 3: EXC3: Bruger sætter sin brik samme sted som han tog den fra*

> Side 8

#### 3.7 Use Case 3: EXC4: Bruger har vundet

| | |
|---|---|
| **Use Case under test:** | Spil "Infinity Mode" |
| **Scenarie:** | EXC4: Bruger har vundet (Easy/Medium Mode) |
| **Prækondition:** | Systemet er initieret, mode er valgt til "Test Infinity Mode", "Medium Mode" samt "I'll go first" |

| No. | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Sæt brikker på pladserne 5, 7 og 8 og vælg "Start game" | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og vinker | Robotarmen placerer sine brikker på pladserne 0, 1 og 3 og bevæger sig | OK |
| 2 | Fjern brikken på plads 5 og placer den på plads 6 | Systemet viser vinderskærm for IM | Systemet viser vinderskærm for IM | OK |
| 3 | Indtast "Testnavn", vælg "Submit" og vælg "Vis highscore" | Systemet gemmer Testnavn og score = 7 på highscorelisten | Systemet gemmer Testnavn og score = 7 på highscorelisten | OK |

*Tabel 14: Accepttest for Use Case 3, EXC4: Bruger har vundet*

> Side 9

### 4 Test af ikke-funktionelle krav

I dette afsnit er de ikke-funktionelle krav, specificeret i afsnit 4 i Kravspecifikation, testet. Til udførelsen af testen anvendes følgende udstyr.

Udstyr til test af krav:
- Stopur (bruges til tidtagning af alle tests med et tidsinterval)
- Målebånd/lineal og skydelære (bruges til måling af alle tests med en længde)
- Køkkenvægt (bruges til vejning af spillebrikkerne)
- Badevægt (bruges til vejning af det samlede system)

#### 4.1 Fysiske dimensioner

**Ikke-funktionelle krav:** Fysiske dimensioner

| No. | Krav | Test / udførelse | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Spillepladens dimensioner skal være: 170 x 170 x 40 (± 10) | Mål med målebånd/lineal spillepladens dimensioner | Spillepladens dimensioner er: 170 x 170 x 30 | OK |
| 2 | Kassens dimensioner skal være: 400 x 500 x 200 (± 25) | Mål med målebånd/lineal kassens dimensioner | Kassens dimensioner er: 400 x 500 x 180 | OK |
| 3 | Spillebrikkernes dimensioner: 15 i diameter (± 5) | Mål med skydelære spillebrikkernes dimensioner | Spillebrikkernes dimensioner er: 12.7 i diameter | OK |
| 4 | Robotarmens overarm skal have dimensionerne: 50 x 220 x 70 (± 10) | Mål med målebånd/lineal dimensionerne af overarmen på robotarmen | Overarmen på robotarmen har dimensionerne: 45 x 220 x 65 | OK |
| 5 | Robotarmens underarm skal have dimensionerne: 30 x 220 x 40 (± 10) | Mål med målebånd/lineal dimensionerne af underarmen på robotarmen | Underarmen på robotarmen har dimensionerne: 30 x 220 x 42 | OK |

*Tabel 15: Accepttest for ikke-funktionelle krav: Fysiske dimensioner*

> Side 10

#### 4.2 GUI

**Ikke-funktionelle krav:** GUI

| No. | Krav | Test / udførelse | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Reaktionstiden mellem hvert vindue skal være under 1 sekund | Mål reaktionstiden mellem hvert vindue, ved at starte stopuret ved tryk på GUI og stop stopuret ved visning af næste vindue | Reaktionstiden mellem hvert vindue er: 0.1 sekund | OK |
| 2 | Opdatering af topscorer på databasen skal ske på under 1 sekund | Mål opdateringen af topscoren, ved at starte stopuret idet navnet på en vundet bruger indtastes, og stop stopuret idet databasen er opdateret med brugerens navn og score | Opdateringen af topscore med navn og score er: 0.41 sekund | OK |
| 3 | Varigheden for initiering af spillet skal vare maksimalt 20 sekunder | Mål varigheden for initiering af spillet, ved at starte stopuret idet brugeren trykker på GUI'en, brugeren skal herefter vælge spiltype, sværhedsgrad og spilstarter, og stop stopuret idet robotarmen vinker | Varigheden for initiering af spillet er: 2.58 sekunder | OK |
| 4 | Opløsningen af touchskærmen skal minimum være 400x240 | Den skrevne kode tjekkes for indstillinger af opløsning | Opløsningen for touchskærmen er 800x480 | OK |

*Tabel 16: Accepttest for ikke-funktionelle krav: GUI*

> Side 11

#### 4.3 Andet

**Ikke-funktionelle krav:** Andet

| No. | Krav | Test / udførelse | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | Reaktionstiden for beregning af træk skal være under 2 sekund | Mål reaktionstiden for beregning af træk, ved at starte stopuret idet brugeren ligger en brik, og stop stopuret idet robotarmen begynder sin bevægelse | Reaktionstiden for beregning af træk er: 0.48 sekund | OK |
| 2 | Robotarmen skal kunne placere en brik på under 10 sekunder | Mål tidsintervallet for robotarmen at placere en brik, ved at starte stopuret idet robotarmen begynder sin bevægelse, og stop stopuret idet spillebrikken er placeret retmæssigt | Robotarmen kan placere en brik på: 6.78 sekunder | OK |
| 3 | Spillebrikkerne skal veje mellem 5 og 20 gram | Mål spillebrikkernes vægt ved at veje dem på en køkkenvægt | Spillebrikkerne vejer 8.6 gram | OK |
| 4 | Det samlede system må maksimalt veje 10 kilogram | Vej det samlede system, ved at træde op på badevægten, nulstil vægten og løft det samlede system. Angiv den viste vægt | Det samlede system vejer 8 kilogram | OK |

*Tabel 17: Accepttest for ikke-funktionelle krav: Andet*

> Side 12

---

## Hvad eksemplet illustrerer (ift. slides L5/L7)

- Én testtabel pr. use case-sti: hovedscenarie + hver EXT/EXC får sit eget scenarie ("for each path you need a test scenario").
- Tabelstrukturen følger skabelonen fra slides: Use case under test / Scenarie / Prækondition, dernæst Step, Handling, Forventet, Faktisk, Vurdering.
- "Handling" er input/aktioner; "Forventet" er observerbare outputs (validation rules fra slide 30).
- Prækonditionen fastlægger den tilstand systemet bringes i (mode, sværhedsgrad, hvem starter), så testen er repeatable.
- Ikke-funktionelle krav testes med angivet måleudstyr og målemetode, og faktisk måling noteres mod kravets tolerance.
- Tabel 11, step 2 er vurderet "(OK)" i parentes, fordi forventet resultat er tilfældigt (Easy/Medium) og derfor ikke kan verificeres deterministisk.
