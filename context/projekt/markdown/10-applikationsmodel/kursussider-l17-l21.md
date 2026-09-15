# Kursussider L17–L21: Applikationsmodel

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L17–L21 — Applikationsmodel |
| **Kursus** | SWISE-01 Indledende System Engineering (E25.28524PU014.A:S) |
| **Kilde** | Brightspace-sider `L17 Applikationsmodel.html`, `L18 Applikationsmodel og Hardware.html`, `L19 Applikationsmodel - sammensatte systemer.html`, `L20 - Applikationsmodel - sammensatte systemer for.html`, `Table of Contents.html` (zip 14) |
| **Type** | kursusside |
| **Emner dækket** | Indhold, forberedelse, slides og øvelser per lektion; oversigt over modulets filer |

*Bemærk nummereringen: Brightspace-modulets Table of Contents hedder "L18-L21 Applikationsmodel" og nummererer siderne L18–L21, mens selve sidernes titler (og filnavnene) siger L17–L20. Nedenfor bruges sidernes egne titler.*

---

## Table of Contents (modulet "L18-L21 Applikationsmodel")

1. L18 Applikationsmodel
2. L19 Applikationsmodel og Hardware
3. L20 Applikationsmodel - sammensatte systemer
4. L21 - Applikationsmodel - sammensatte systemer fortsat
5. L18 Løsningsforslag
6. L19 Løsningsforslag
7. L20 Løsningsforslag

Underviser-signatur på alle sider: **/JMK**.

---

## L17 Applikationsmodel

**Indhold**
- Applikationsmodel (Fra Use Cases til software)
- Grænseflade- og kontrol-klasser
- Sekvens- og state-diagrammer

**Forberedelse**
- Kig på disse slides og kig på Use Casen i nedenstående øvelse.
- Slides: `System Application Models Part1.pdf` → se `system-application-models-1.md`

**Øvelse på klassen og hjemme**
- Applikationsmodellen for "ATM machine" med "UC Withdraw Cash"
- `SAM_ATM UC description.pdf` (Hjemmeopgave)
- Lav et sekvensdiagram, klassediagram og statediagram som vist på slides
- Brug steps 1.1–1.4 og 2.1–2.5 for Use Casen

---

## L18 Applikationsmodel og Hardware

**Indhold**
- Nogen gange kan man være mere præcis, når man skal finde Boundaryklasserne. Fx. hvis man er så heldig, at have et IBD med angivelse af HW interfaces.
- Når man skal implementere Use Casen, kan man være heldig at have et System sekvensdiagram, der kan være input til Applikationsmodellens sekvensdiagram.

**Forberedelse**
- Lav opgaven om UC Withdraw Cash færdig. Et løsningsforslag gøres synligt inden lektionen og gennemgås der.
- Kig slides igennem.
- Kig opgave 3A og 3B fra eksamenssættet E2015 nedenfor igennem.

**Slides**
- `System Application Models Part2.pdf` / `System Application Models Part2.pptx` → se `system-application-models-2.md`

**Øvelser**
- Applikationsmodel for "SmartFridge" fra eksamensopgave E2015 (laves på klassen)
- `I2ISE Eksamensopgave E2015.pdf` — løs opgave 3A og 3B
- Løsninger følger senere.

---

## L19 Applikationsmodel – sammensatte systemer

**Indhold**
- Gennemgang af en eller to løsninger til opgaverne fra sidste lektion.
- Applikationsmodeller for sammensatte systemer

**Forberedelse**
- Lav opgaven om Smartfridge færdig. Et løsningsforslag gøres synligt inden lektionen og gennemgås der.
- Kig slides igennem.
- Kig på beskrivelsen af Benzinstationen i opgaven, og prøv at forstå hvordan systemet er bygget op af delsystemer ud fra BDD, IBD og sekvensdiagrammer. Kig på domænemodellen for Benzinstationen.

**Slides**
- `System Application Models Part3.pdf` → se `system-application-models-3.md`

**Øvelser på klassen**
- Applikationsmodellen for "Benzinstanderstyring" med "UC Optank Bil"
- `SAM_Benzinstander_Opgave.pdf`
- Brug steps 1.1–1.4 og 2.1–2.5 for Use Casen på lige netop Benzinstanderstyringen.
- (Næste gang skal vi lave den for Benzinstanderen).

---

## L20 – Applikationsmodel – sammensatte systemer fortsat

**Indhold**
- Vi arbejder: `L21_Kamerasystem Exercise.pdf`
- Kamerasystem: opg 1 – opg 6 (opg 6 er Application Model opgave).
- For at lave opgave 6 skal I også finde ud af opgave 1–5, som er en øvelse til Application Model.
- Vi arbejder med første halvdel, som I prøver at lave selv.
- I anden halvdel af lektionen gennemgår jeg løsningsforslagene til opgaverne.
- Efter den, jeg viser eksempler på, hvordan studerende anvendede Application Model til deres semesterprojekt.

**Forberedelse**
- Læs gennem Slides lektion 17 – Lektion 20
- Lav Applikationsmodellen for BenzinStanderStyringen færdig eller for Kamerasystem.
- Gør dig nogle tanker om, hvordan AM for Kamerasystem kunne se ud.

**Løsninger**
- BenzinStanderStyringen Løsning kan findes i løsningsforslag.
- Kamerasystem løsning i løsningsforslag.

---

## Oversigt: lektion → slides → øvelse → løsning

| Lektion | Slides | Øvelse | Løsningsforslag (Brightspace-side) |
|---|---|---|---|
| L17 Applikationsmodel | Part 1 | ATM, UC Withdraw Cash (`SAM_ATM UC description.pdf`) | L17 Løsningsforslag (`SAM_ATM_Lsning.pdf`) |
| L18 Applikationsmodel og Hardware | Part 2 (pdf + pptx) | SmartFridge, E2015 opg. 3A/3B (`I2ISE Eksamensopgave E2015.pdf`) | L18 Løsningsforslag (`SAM_SmartFridge_Opgave_Lsning.pdf`) |
| L19 Sammensatte systemer | Part 3 | Benzinstanderstyring, UC Optank Bil (`SAM_Benzinstander_Opgave.pdf`) | L19-20 Løsningsforslag (`SAM_Benzinstander_Opgave_Lsning.pdf`) |
| L20 Sammensatte systemer fortsat | (Part 1–3 repeteres) | Kamerasystem opg. 1–6 (`L21_Kamerasystem Exercise.pdf`) | L19-20 Løsningsforslag |
