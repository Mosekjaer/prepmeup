# Beskrivelse af bilvaskehal

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L3 — Use Cases (Exercise 3) |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `Bilvaskehal.pdf` (2 sider) |
| **Type** | øvelse |
| **Emner dækket** | Case til use case-diagram (SysML UC) og fully dressed use case "Vask bil" |

---

## Beskrivelse af bilvaskehal

Der skal designes en vaskehal til biler med tilhørende automatbetjening og betaling samt tilhørende styring. Systemet installeres på en bemandet tankstation. En typisk brugssituation er som følger:

Kunden ankommer til vaskehallen og kører bilen ind i hallen (figur 1.). En indikator angiver med grønne lysende pile om kunden skal køre frem eller bakke indtil bilen er kørt i den rigtige position. Den rigtige position angives med et rødt stop signal. Kunden stiger derefter ud af sin bil og sikre at døre og vinduer er lukket.

Kunden forlader vaskehallen og indsætter sit kreditkort i betjeningsstanderen (figur 2.) udenfor vaskehallen. Når pinkoden er indtastet og godkendt af PBS vælger kunden den ønskede vask. Her kan han vælge imellem typerne *budget*, *normal*, *luksus* og *guldvask*. Når den ønskede type af vask er valgt lukkes porten til vaskehallen og vasken gennemføres. Herefter informeres tankstationens centrale computer om vasketype og pris. Beløbet trækkes automatiske fra kundens betalingskort via. PBS. Kunden kan til enhver tid aktivere nødstoppet på betjeningsstandere. Når vasken er afsluttet kan kunden vælge at få printet en kvittering.

Figur 1 nedenfor viser en skitse af vaskehal med varmetørrer, vand- og sæbedyser, børster, navigationsindikator, sensorer og elektroniskstyring. Den elektroniskstyring er forbundet til alle enheder i vaskehallen. Den modtager signal fra sensorer som bruges til indikation af korrekt placering af bilen. Den styrer børster, vand- og sæbedyser til vask og varmetørrerens bevægelse under tørring.

Automatisk åbning og lukning af port er ikke vist på figuren og skal ikke medtages i opgaven.

*Figur 1 — Skitse af vaskehal med elektroniskstyring: Rektangulær hal set ovenfra. Øverst en bred boks "Varmetørrer". To lodrette bjælker "Børster" langs venstre og højre side. I midten en "Navigationsindikator" med grøn trekant "Frem" (op), rund "Stop" og hvid trekant "Bak" (ned). En lille grå boks "Elektroniskstyring" under indikatoren. "Vand- og sæbedyser" markeret som små cirkler i de øverste hjørner; "Sensorer" som små cirkler i de nederste hjørner.*

> Side 1

*Figur 2 — Skitse af betjeningsstander placeret foran vaskehallen: Høj lodret stander. Øverst en skærm med knappen "Vælg program" — "Touchskærm og computer". Under skærmen to slots: "Kortlæser" (højre) og "Printer" (venstre). Nederst en rund knap "Nødstop".*

Computeren i betjeningsstanderen og den elektroniskstyring i vaskehallen er tilkoblet tankstationens netværk som har forbindelse til tankstationens central computer.

## Opgave A

Med udgangspunkt i ovenstående beskrivelse af bilvaskehal (vaskehal og betjeningsstander): Tegn et SysML *Use Case Diagram* (UC) hvorpå aktører og use cases for systemet er identificeret.

## Opgave B

En oplagt use case for bilvaskehallen er "Vask bil". Skriv en *fully dressed* use case-beskrivelse for denne use case. Du skal medtage undtagelsen hvor kunden afbryder købet inden vasken er startet, men ikke andre undtagelser. Anvend skabelonen nedenfor til at skrive use case-beskrivelsen:

| Felt | |
|---|---|
| **Navn:** | |
| **Mål** | |
| **Initiering** | |
| **Aktører** | |
| **Antal samtidige forekomster** | |
| **Prækondition** | |
| **Postkondition** | |
| **Hovedscenarie** | |
| **Udvidelser/undtagelser** | |

> Side 2
