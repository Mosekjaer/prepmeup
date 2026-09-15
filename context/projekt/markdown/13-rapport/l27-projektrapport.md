# L27 — Projektrapport (struktur og krav til semesterprojektrapporten)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L27 — Projektrapport (L27-28 Projektrapport og rapport eksempler) |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `L27_Projektrapport.pdf` (24 slides) |
| **Type** | slides |
| **Underviser** | "Jenny" Jung Min Kim (generelle spørgsmål; detaljespørgsmål tages med vejleder) |
| **Emner dækket** | Rapportens formål og format (72.000 tegn), kapitelstruktur (forside → referencer), forside, revisionshistorik, resumé/abstract, forord, ansvarsfordeling, indledning/problemformulering, krav, metode og proces, teknisk analyse, systemarkitektur (BDD/IBD/SD/STM), design og implementation, integrationstest, accepttest, resultater, diskussion, konklusion, fremtidigt arbejde, bilagsoversigt, referencer, bilagsmappe, læringsmål for SW2PRJ2 |

---

> Slidesættet er genbrugt fra SW2PRJ2 (2. semester) og præsenteres som **en reference, IKKE en absolut regel**. Titel: "Projektrapport (PRJ2) — SW2PRJ2-projektrapporten", markeret SW2ISE. Om projektrapporten: generelle spørgsmål til "Jenny" Jung Min Kim, men detaljespørgsmål kan diskuteres med vejleder.

> Slide 1

## Projektrapportens formål og format

Rapporten skal svare på tre spørgsmål:

- Hvad er opgaven/problemet?
- Hvordan er problemet løst?
- Hvilke resultater er der opnået?

Format:

- Maks. **72.000 tegn** inkl. mellemrum
- Figurer er ikke medregnet
- Det er kvaliteten der tæller
- Skal kunne læses som et **selvstændigt dokument**, der giver overblik (censor)
- Brug referencer til bilag, der indeholder yderligere information

> Slide 2

## Projektrapportens indhold (kapiteloversigt)

Forsiden af rapporten (elementer uden kapitelnummer) og de nummererede kapitler. Elementer i grå skrift på sliden (Forord, Ansvarsfordeling, Ordliste, Integrationtest) er valgfrie/nedtonede.

| Del | Element |
|---|---|
| Foran | Forside |
| Foran | Revisionshistorik |
| Foran | Resumé/Abstract |
| Foran | Indholdsfortegnelse |
| Foran | Forord *(optional)* |
| Foran | Ansvarsfordeling *(optional)* |
| Foran | Ordliste *(optional)* |
| 1 | Indledning — inkl. Projektformulering og Problemformulering |
| 2 | Krav med afgrænsning |
| 3 | Metode og proces |
| 4 | Teknisk Analyse |
| 5 | Systemarkitektur |
| 6 | Design og Implementation |
| 7 | Integrationtest *(nedtonet)* |
| 8 | Accepttancetest |
| 9 | Resultater |
| 10 | Diskussion af resultater |
| 11 | Konklusion |
| 12 | Fremtidigt arbejde |
| Bagest | Bilagsoversigt |
| Bagest | Referencer |

> Slide 3

## Forside

Projektrapportens forside skal som minimum indeholde følgende informationer:

- Projektets titel og evt. undertitel
- Gruppenummer
- Projektdeltagernes studienumre og navne
- Navn på institution
- Dato for aflevering
- Navn(e) på vejleder(e)

> Slide 4

## Revisionshistorik (eksempel)

*Figur: Tre eksempler på versionshistorik-tabeller fra tidligere rapporter.*

1. **"Versionshistorik"** med kolonnerne Version | Dato | Initialer | Ændring. Eksempelrækker: v1 23/09-2021 (SK, IKAT) "Oprettet et fælles LaTeX projekt, Forside, Indledning, Problemformulering, Projektformulering"; v2 13/12-2021 "HW-arkitektur, Implementering af motor, Integrationstest mellem motor og sensor, Risikovurdering, Specifikation"; v3 14/12-2021 "Specifikation, Implementering af sensor, Implementering af powersupply påbegyndt, Integrationstest, HW-design, Afgrænsninger, Systembeskrivelse påbegyndt"; v4 15/12-2021 "Accepttest, SW-design, SW-arkitektur, Systembeskrivelse, Specifikation tilrettet, Påbegyndt PSoCApp, Implementering af powersupply, Motor og UART, Ansvarsfordeling"; v5 16/12-2021 "Resumé, Abstract, Forord, Metode og proces, Diskussion, Konklusion, Fremtidigt arbejde, Bilagsoversigt"; v6 17/12-2021 "Gennemlæsning og småretttelser".
2. **"Tabel 1. Versionshistorik tabel"** med kolonnerne Version | Dato | Ændring: 0.1 08/12-19 "Dokument oprettet, forside lavet"; 0.2 28/12-19 "Resumé/Abstract lavet"; 0.3 30/12-19 "Forord, indledning, Krav, Afgrænsning, Metode, Analyse, Arkitektur, Design, Implementering lavet"; 0.4 02/01-20 "Tests, Resultater, Diskussion resultater, Fremtidigt arbejde lavet, samt korrektur læsning"; 0.5 04/01-20 "Korrektur læsning og rettelser, samt konklussion og fremtidigt arbejde lavet"; 1.0 05/01-20 "Korrektur og rettelser - Færdig version".
3. **"Changelog"** med kolonnerne Dato | Initialer | Ændring (rækker fra 10-Apr til 05-Jun, fx "Oprettet dokument og konverteret projektformulering til LaTeX", "Skrevet 'Hardware Design - Sound' og 'Software Design - PCApp'. Tilføjet bilagsoversigt", "Skrevet 'Systemarkitektur'...", "Skrevet 'Afgrænsning'...", "Skrevet Fremtidigt arbejde og halv konklusion, samt rettelser", "Rettelser").

Fælles mønster: version/dato, hvem, og hvad der er ændret.

> Slide 5

## Resumé/Abstract

- Dansk og engelsk
- Formålet er at fortælle, hvad rapporten kan bruges til
- Redegørelse for emnet med afgrænsning og synsvinkel
- Kort oversigt over den faglige gennemgang med konklusion
- Fylder ca. 1/3 – 1/2 side pr. sprog

> Slide 6

## Forord (optional)

- Praktisk information
  - Hvem er I?
  - Uddannelsessted
  - Retning og semester
  - Hvem er vejleder?
  - Antal tegn og sider
  - Afleveringsdato og bedømmelsesdato
  - Hvem har været ansvarlig for hvad? (Tabel)
- Læsevejledning
  - Kan indeholde en kort læsevejledning

> Slide 7

## Ansvarsfordeling (optional): eksempler

"I kan bestemme selv." Tre eksempler på ansvarsfordelingstabeller:

**Eksempel 1 ("Tabel 2 Arbejdsfordeling")** — matrix med rækker = rapportafsnit/arbejdsområder og kolonner navn1…navn6. Markering: **H = Hovedansvarlig, D = Delansvarlig**. Rækker: Kravsspecifikation, Accepttestspecifikation, HW-Arkitektur, SW-Arkitektur, HW-Design (X10 sender HW-design, X10 modtager HW-design, Gardin kredsløb HW-design, Mp3 kredsløb HW-design, Lampe kredsløb HW-design), SW-Design (PC software, DE-II software), Main controller (X10 TX-klasse, DE-II klasse, PC-klasse), Lampe controller (X10 RX-klasse, Lampe driver klasse), Implementering, Accepttest, Procesbeskrivelse, X10 protokol, Konklusion (X for alle).

**Eksempel 2 ("Tabel 1: Ansvarsfordeling")** — kolonner navn 1–7, rækker: Motorstyring Hardware, Motorstyring Software, Sensor Hardware, Sensor Software, Power Supply, UART, Web-server, 3D-print — med X hvor personen er ansvarlig.

**Eksempel 3** — kolonner Navn1…Navn8, rækker med X:

| Afsnit | Navn1 | Navn2 | Navn3 | Navn4 | Navn5 | Navn6 | Navn7 | Navn8 |
|---|---|---|---|---|---|---|---|---|
| Introduktion | X | X | X | X | X | X | X | X |
| Krav | X | X | X | X | X | X | X | X |
| Tekniske Analyse | X | X | X | X | X | X | X | X |
| Arkitektur | X | X | X | X | X | X | X | X |
| HW Design | X | X | X | X | | | | |
| — Arduino | | | X | X | | | | |
| — RPI | X | X | | | | | | |
| SW Design | | | | | X | X | X | X |
| — UI | | | | | X | X | | |
| — web | | | | | | | x | x |
| HW Implementation | X | X | X | X | | | | |
| SW Implementation | | | | | X | X | X | X |
| Acceptancetest og resultater | X | X | X | X | X | X | X | X |
| Diskussion | X | X | X | X | X | X | X | X |
| Konklusion | X | X | X | X | X | X | X | X |

> Slide 8

## 1. Indledning

- Indledning
- Projektformulering
  - Beskriver projektets overordnede idé, formål osv.
- Problemformulering
  - Hvad projektet skal besvare
  - Det spørgsmål eller problem, du vil løse eller forbedre

> Slide 9

## 2. Krav

- Funktionelle krav
  - Aktør Kontekst Diagram
  - Use Case Diagram
  - Fully Dressed beskrivelse
- Ikke-funktionelle krav
  - (F)URPS+
- Afgrænsning
  - MoSCoW af funktionelle krav og ikke-funktionelle krav
- Accepttest-krav
  - (note) kan laves her, men kan også placeres i rapportens test-sektion

> Slide 10

## 3. Metode og proces

- Hvilken metode er brugt til at løse opgaven (1-2 sider)
  - Udviklingsproces
  - Analyse- og designmetode
  - Brugen af SysML- og UML-diagrammer med formål
- Hvilken proces er brugt til styring af projektet (1-2 sider)
  - Gruppedannelse
  - Samarbejdsaftale
  - Planlægning og møder
  - Projektledelse
  - Reviews

> Slide 11

## 4. Teknisk analyse

Refererer til L6: "How to Technical Analysis".

- Undersøg hvilke muligheder (kandidater) der er af komponenter, sammenlign i samme kategorier og præsenter fordele og ulemper ved hver enkelt af disse komponenter/teknologier osv.
- Beskriv teori (funktionaliteter, characteristics, teknisk beskrivelse, baseret på fakta) af kandidaterne (mulighederne)
- Præsenter fordele og ulemper ved hver mulighed og overvejelser om de mulige løsninger I har
- Grundlæggende valg af hardware og software
- Brug evt. tabel til opstilling af mulige løsninger med fordele og ulemper
- Kategorisér sammenligningerne, f.eks.:
  - Microcontroller
  - Sensorer (og aktuator)
  - UI: f.eks. tekstbaseret UI eller grafisk UI
  - Interface: UART vs. I2C
  - Programmeringssprog
  - …

> Slide 12

## 5. Systemarkitektur

- Systemarkitektur:
  - BDD
  - IBD
  - SD
  - (optional) STM
- (Muligvis) kan det også adskilles (det kommer an på, hvad der giver mening for jeres projekt):
  - Hardwarearkitektur
  - Softwarearkitektur

Boks: **"Læseren skal have det fornødne overblik over systemet."**

> Slide 13

## 6. Design og Implementation

Design, implementering og test beskrives bedst **for hvert delsystem**.

| Design | Implementation |
|---|---|
| **HW Design**: HW schematic f.eks. Multisim; udregninger af f.eks. modstand, strøm, voltage, capacitor osv. | **HW Implementation**: f.eks. praktiske billeder af PCB eller fumlebræt, der matcher schematic i HW Design; f.eks. faktiske målinger af output af HW design osv. |
| **SW Design**: Domænemodel; Applikationsmodel — klassediagram (attributter/parametre, funktioner osv.), sequence diagram med funktioner, state machine diagram (optional) | **SW Implementation**: source code snippets — behøver ikke hele koden, de vigtige dele med forklaringer |
| | **Modultest (optional)** |

> Slide 14

## 7. Integrationtest

- Integrer delsystemerne gradvist, før hele systemet samles
- For at sikre, at samspillet mellem de relevante moduler og delsystemer fungerer korrekt
- Trinvis integration hjælper med at undgå kaotiske problemer i det samlede system til sidst og gør det lettere at opdage fejl i integrationerne undervejs

> Slide 15

## 8. Accepttest

- Accepttesten baseres på de dokumenterede accepttest-krav fra kravfasen
- Der gennemføres praktiske tests for at sikre, at systemet lever op til disse krav
  - Hvor man tjekker, om systemet lever op til de krav og forventninger, der er aftalt med kunden i Krav
- Bekræft at alle krav (dvs. jeres accepttest-krav) er opfyldt
- Giv godkendt/fejlet for hver accepttest
- Det er vigtigt, at hovedrapporten indeholder en accepttest
  - Hvis I har for lang en accepttest-liste, kan I opsummere med de vigtige accepttests, f.eks. MUST-krav, og resten kan ligge i bilag med henvisning til bilagsnummer

> Slide 16

## 9. Resultater

- Præsentér resultater med udgangspunkt i accepttesten
- Resultaterne præsenteres utvetydigt, nøgternt og objektivt
- (Muligvis) kan Accepttest og Resultater samles i ét kapitel

> Slide 17

## 10. Diskussion

Her udpeges og diskuteres relevante dele af de opnåede resultater og deres betydning. Der skal også gives en samlet vurdering af de opnåede resultater i relation til projektets problemformulering. Der kan ligeledes være en opsummerende beskrivelse af resultater, som I er særligt stolte af.

Fokus i diskussionen er primært resultaterne af accepttesten.

> Slide 18

## 11. Konklusion

- I konklusionen gives en samlet konklusion på projektarbejdet og procesarbejdet. Hvad er lykkedes, hvad er ikke lykkedes, og hvad er årsagen til dette.
- Konklusionen skal indeholde et klart budskab og forholde sig objektivt til de krav, der er opstillet i projektet, og de resultater, som I har opnået.
- Det er vigtigt, at konklusionen hænger sammen med problemformuleringen, og den skal give svar på de spørgsmål, som er opstillet for projektet, og konkludere på de vigtigste erfaringer fra selve processen. Som helhed skal konklusionen være objektiv og baseret på fakta.

> Slide 19

## 12. Fremtidigt arbejde

Her beskrives hvad der mangler for at gøre projektet færdigt, og hvilke fremtidige udvidelses- og anvendelsesmuligheder der er i projektet.

> Slide 20

## Bilagsoversigt (eksempel)

"I kan bestemme selv." To eksempler:

*Figur 1: Bilagsoversigt i rapport-form (LaTeX):*
- Bilag 01, Samarbejdskontrakt — *Samarbejdskontrakt.pdf*
- Bilag 02, Kravspecifikation — *Kravspecifikation.pdf*
- Bilag 03, Accepttestspecifikation — *Accepttestspecifikation funktionelle og ikke-funktionelle krav.pdf*
- Bilag 04, Arkitektur — *Samlet arkitektur.pdf*
- Bilag 05, Design

*Figur 2: Bilagsmappe som folderstruktur (Windows Explorer):*
Bilag 1-6_Datablade, Bilag 9_Acceptest, Bilag 10_Accepttestspecifikation, Bilag 11_Hardware design, Bilag 12_Kravspecifikation, Bilag 13_Procesbeskrivelse, Bilag 14_Projektformulering, Bilag 15_Software design, Bilag 16_Systemarkitektur, Bilag 17_Samarbejdskontrakt, Bilag 18_Vejledningsmøder, Bilag 19_Software source kode (mapper), samt PDF-filerne Bilag 7_X10-Protokol-specifikation og Bilag 8_Tidsplan Gruppe 13.

> Slide 21

## Referencer (eksempel)

Boks: **"Indtil her — Hovedrapport"** (dvs. alt ovenstående er hovedrapporten; bilag er separate).

Eksempel på referenceliste (numerisk stil):

```
[1] PRJ2, 2. semester – Projektoplæg (SW2PRJ2), Aarhus Universitet (2018)
[2] Burroughs J., AN236 X-10 Home Automation Using the PIC16F877A, Microchip Technology Inc. (2002)
[3] L. Holten, »Nye tal fra Danmarks Statistik overrasker: Indbrudstallene falder markant,« 14 oktober 2021. [Online]. Available: realdania.dk/nyheder/2021/10/nye-tal-fra-danmarks-statistik-overrasker-eksperter-indbrudstallene-falder-markant.
[4] A. A. ELECTRONICS, »Clamper Circuit Explained,« 26 april 2019. [Online]. Available: www.youtube.com/watch?v=7O3Hbkkt624.
[5] Circuit Basics. Basics of the uart communication. https://www.circuitbasics.com/basics-uart-communication/. Tilgået d. 14/12 - 2023.
```

> Slide 22

## (Optional) Bilag til projektrapporten

Boks: **"IKKE i hovedrapport. Den er 'bilag' (normalt i separat folder)."**

Separat fra hovedrapporten, kan samles som f.eks. `Bilag.zip`. Supplerende dokumentation i bilagsfolder — eksempler (behøver ikke være præcis sådan):

| Den tekniske del | Den procesmæssige del |
|---|---|
| Kravspecifikation | Procesbeskrivelse |
| Systemarkitektur | Samarbejdsaftale |
| Design- og testdokumenter (hardware og software) | Gantt-diagram (plan) |
| Accepttest (gennemført) | Mødereferater |
| Datablade, styklister, printudlæg | Review-referater |
| Source code | Logbog |

"Det er kun eksempler og reference (ikke absolut regel). I kan bestemme selv."

> Slide 23

## Læringsmål for SW2PRJ2

(Fra kursuskataloget: https://kursuskatalog.au.dk/da/course/131154/SW2PRJ2-03-Projekt-2-og-Systems-Engineering)

- Anvende en beskrevet udviklingsproces til gennemførelsen af produktudvikling
- Foretage og modtage review af en anden projektgruppes arbejde
- Foretage fælles planlægning og uddelegering af opgaver
- Anvende mødeledelse med dagsorden og referat i en projektgruppe
- Formulere egne ingeniørfaglige styrker og svagheder i projektarbejdet
- Formulere og anvende en use case-baseret kravspecifikation
- Anvende kravspecifikation til udformning af accepttest
- Beskrive systemarkitektur og design ved hjælp af SysML og UML
- Anvende korrekt fagterminologi
- Designe og implementere en prototype, der indeholder egen udviklet software
- Kombinere viden fra flere af semestrets kurser og anvende denne i projektet
- Vurdere og evaluere projektets udviklingsproces, produkt og resultater
- Udvælge og anvende supplerende viden i projektarbejdet med angivelse af referencer
- Præsentere projektets resultater ved et mundtligt forsvar

> Slide 24
