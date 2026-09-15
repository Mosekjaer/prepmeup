# Øvelse: Accepttest af pengeskabsstyring

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L5/L7 — Systemtest og accepttest |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `AcceptTestOvelse.pdf` (2 sider; `AcceptTestOvelse.doc` er samme dokument i Word-format) |
| **Type** | øvelse (på klassen; dateret 1. September 2014/KBE) |
| **Emner dækket** | Fully-dressed use case → accepttestspecifikation, ét testscenarie pr. sti (hovedscenarie + extensions), accepttestskabelon |

---

## Accepttest af pengeskabsstyring

Opgaven omhandler lågen til et *pengeskab*, der bruges til opbevaring af værdigenstande. Pengeskabslågen er forsynet med en låsemekanisme, så det kun er personen med det korrekte fingeraftryk, der kan få adgang til pengeskabet. Pengeskabslågen er skitseret nedenfor:

*Figur 1: Pengeskabslågen og nærbillede af buzzer og fingeraftryksscanner. Skitsen viser en låge set forfra med "Håndtag" øverst til højre, en "Lukke-sensor" i lågens overkant, en "Låsepal (låst)" der stikker ud fra lågens højre kant øverst, og en "Låsepal (oplåst)" trukket ind nederst. Et panel på lågens forside er vist forstørret til højre med "Buzzer" (rillet felt) og "Fingeraftryks-scanner" (firkantet felt).*

Pengeskabslågen består af følgende dele:
- En låge.
- En lukkesensor, der registrerer om lågen er lukket eller åben.
- To låsepaler, der i låst stilling griber i pengeskabets karm og dermed låser pengeskabet.
- En fingeraftryksscanner
- En buzzer til lydafgivelse
- En controllerenhed, der interagerer med de andre enheder (ikke vist på figuren)

> Side 1

## Accepttest specifikation

Nedenfor er vist en fully-dressed use case for betjeningen og åbning af pengeskabet.

| Felt | Indhold |
|---|---|
| **Navn:** | Åbn Pengeskab |
| **Mål** | At åbne pengeskabet og dermed tillade brugeren adgang til dettes indhold |
| **Initiering** | Bruger |
| **Aktører** | Bruger |
| **Antal samtidige forekomster** | 1 |
| **Prækondition** | Pengeskabet er låst |
| **Postkondition** | Pengeskabet er låst op og åbent |
| **Hovedscenarie** | 1. Bruger anbringer højre tommelfinger på Systems fingeraftryksscanner<br>2. System scanner brugerens fingeraftryk<br>3. System validerer brugerens fingeraftryk<br>   [Ext. 1: Fingeraftryk ej genkendt]<br>4. System afgiver "Scan OK"-lyd<br>5. System trækker låsepaler ind og låser dermed pengeskabet op<br>6. System detekterer at lågen åbnes.<br>   [Ext. 2: Låge åbnes ikke inden 5 sekunder] |
| **Udvidelser/undtagelser** | [Ext.1: Fingeraftryk ej genkendt]<br>E1.1: System afgiver "Fejl i scan"-lyd<br>E1.2: UC afsluttes<br><br>[Ext.2: Låge åbnes ikke indenfor 5 sekunder]<br>E2.1: System skyder låsepaler ud og låser dermed pengeskabet<br>E2.2: System afgiver "Pengeskab låst"-lyd |

### Opgave

Skriv en specifikation af tre test cases til accepttesten, der hver især tester ét af følgende scenarier i use case "Åbn pengeskab".

- Hovedscenariet
- Ext. 1: Fingeraftryk ej genkendt
- Ext. 2: Låge åbnes ikke indenfor 5 sekunder

Brug nedenstående skabelon:

| Use case under test | |
|---|---|
| Scenarie | |
| Prækondition | |

| Step | Handling | Forventet observation/resultat | Faktisk observation/resultat | Vurdering (OK/FAIL) |
|---|---|---|---|---|
| 1 | | | | |
| 2 | | | | |
| 3 | | | | |
| … | | | | |

> Side 2

---

*Bemærk: Brightspace-siden nævner et løsningsforslag `AcceptTestOvelseLosning.pdf`, men det er ikke med i det eksporterede materiale.*
