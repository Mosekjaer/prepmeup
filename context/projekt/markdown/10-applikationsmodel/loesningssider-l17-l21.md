# Løsningsforslag-sider L17–L21 (Brightspace): hvilken opgave → hvilken løsning

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L17–L21 — Applikationsmodel |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | Brightspace-HTML: `L17 Lsningsforslag.html`, `L18 Lsningsforslag.html`, `L19-20 Lsningsforslag.html` (+ lektionssiderne L17–L20 for kontekst) |
| **Type** | kursusside |
| **Emner dækket** | Mapping mellem øvelser og løsningsfiler; noter om udvidede løsninger og polling af boundary-klasse |

---

## Nummerering

Brightspace-modulet hedder "L18-L21 Applikationsmodel" i Table of Contents, men de enkelte sider hedder L17–L20. Siderne refererer selv til lektionsnumre, der er forskudt med én (fx "løsningsforslaget til ATM fra lektion 18" på siden "L17 Løsningsforslag"). Tabellen herunder bruger sidernes egne navne (L17–L20) som i filnavnene; Table of Contents-navnet er i parentes.

| Side (TOC-navn) | Emne |
|---|---|
| L17 Applikationsmodel (L18) | Fra Use Cases til software; grænseflade- og kontrol-klasser; sekvens- og state-diagrammer |
| L18 Applikationsmodel og Hardware (L19) | Boundary-klasser fra IBD med HW-interfaces; SSD som input til SAM-sekvensdiagram |
| L19 Applikationsmodel – sammensatte systemer (L20) | Applikationsmodeller for sammensatte systemer |
| L20 Applikationsmodel – sammensatte systemer fortsat (L21) | Kamerasystem opg. 1–6; eksempler på studerendes brug af Application Model i semesterprojekt |

## Opgave → løsning

| Øvelse (stillet på side) | Opgavefil | Løsningsside | Løsningsfil | Markdown i dette modul |
|---|---|---|---|---|
| ATM: applikationsmodel for "UC Withdraw Cash" (L17, hjemmeopgave). "Lav et sekvensdiagram, klassediagram og statediagram som vist på slides. Brug steps 1.1-1.4 og 2.1 - 2.5 for Use Casen." | `SAM_ATM UC description.pdf` | L17 Løsningsforslag | `SAM_ATM_Løsning.pdf` | `oevelse-atm-withdraw-cash.md` |
| SmartFridge fra eksamensopgave E2015, opg. 3A og 3B (L18, laves på klassen) | `I2ISE Eksamensopgave E2015.pdf` | L18 Løsningsforslag | `SAM_SmartFridge_Opgave_Løsning.pdf` | `eksamensopgave-e2015-smartfridge.md` |
| Benzinstanderstyring med "UC Optank Bil" (L19, på klassen). "Brug steps 1.1-1.4 og 2.1 - 2.5 for Use Casen på lige netop Benzinstanderstyringen. (Næste gang skal vi lave den for Benzinstanderen)." | `SAM_Benzinstander_Opgave.pdf` (opgave 1 = Benzinstanderstyring, opgave 2 = Benzinstander) | L19-20 Løsningsforslag | `SAM_Benzinstander_Opgave_Løsning.pdf` | `oevelse-benzinstander-applikationsmodel.md` |
| Kamerasystem opg. 1–6, opg. 6 er Application Model (L20, på klassen) | `L21_Kamerasystem Exercise.pdf` | — (siden skriver "Kamerasystem løsning i løsningsforslag", men ingen fil findes i modulet) | — | `oevelse-kamerasystem.md` |

## Ordlyd på løsningssiderne

**L17 Løsningsforslag:**
> `SAM_ATM_Løsning.pdf` — Her er løsningsforslaget til ATM fra lektion 18.

**L18 Løsningsforslag:**
> `SAM_SmartFridge_Opgave_Løsning.pdf` — Her er løsningsforslag til SmartFridge, fra Lektion 16.

**L19-20 Løsningsforslag:**
> `SAM_Benzinstander_Opgave_Løsning.pdf` — Her løsningsforslag til opgaverne om Benzintanken.
> Der er suppleret med nogle udvidede løsninger, der fylder nogle af hullerne ud.
> Der er også demonstreret, hvordan man kan polle en boundary klasse i et loop, nemlig der, hvor Benzinstanderen skal opdatere sine displays ud fra hvor meget Pumpen har pumpet.

## Noter om de udvidede løsninger (Benzinstander)

De "udvidede løsninger" er afsnittet *ALTERNATIVE LØSNINGER* på side 8–9 i `SAM_Benzinstander_Opgave_Løsning.pdf`:

- **Huller der fyldes ud:** Den første løsning viste, at `Brændstoftype` ikke blev brugt af Benzinstanderstyringen, og at der ingen steder i BDD/IBD/domænemodel er en printer eller et display. I den udvidede løsning flyttes kendskabet til brændstofpriser til Benzinstanderstyringen (`GetBrændstofPris` / `SendBrændstofPris` mellem `UC1_OptankBil` og `BenzinstanderIF`), og Benzinstanderen får display-metoder (`OpdaterPrisDisplay`, `OpdaterDisplay`, `NulstilDisplay`). Pointen: SAM afslører mangler i den tidligere systemmodel, og ændringerne skal føres tilbage til SSD, BDD, IBD og domænemodel og dokumenteres.
- **Polling af boundary-klasse i loop:** I Benzinstanderens udvidede sekvensdiagram er der et `loop [indtil stop]`-fragment, hvor controlleren `UC1_OptankBil` gentagne gange kalder `GetVolumen()` på `PumpeIF`, får `volume` retur og kalder `OpdaterDisplay(volume, pris)` på sig selv — indtil `TankpistolPåPlads()` modtages. Det er mønstret for en kontinuert størrelse, hvor boundary'en ikke selv genererer events. `PumpeIF` får derfor `GetVolumen() : double` i det opdaterede klassediagram.

Fuld gengivelse: se `oevelse-benzinstander-applikationsmodel.md`, afsnit "ALTERNATIVE LØSNINGER".

## Forberedelses-flow på tværs af siderne

- L17: Kig på `System Application Models Part1.pdf` og use casen i ATM-opgaven. ATM er hjemmeopgave.
- L18: Lav ATM færdig (løsning gøres synlig inden lektionen og gennemgås). Kig slides Part2 igennem. Kig E2015 opg. 3A/3B igennem. SmartFridge laves på klassen; "Løsninger følger senere."
- L19: Lav SmartFridge færdig (løsning gøres synlig inden lektionen). Kig slides Part3. Forstå Benzinstationens delsystemer ud fra BDD, IBD og sekvensdiagrammer; kig på domænemodellen. Benzinstanderstyring laves på klassen.
- L20: Læs slides L17–L20. Lav applikationsmodellen for Benzinstanderstyringen færdig eller for Kamerasystem. Kamerasystem opg. 1–6 på klassen; første halvdel selv, anden halvdel gennemgang af løsningsforslag; derefter eksempler fra studerendes semesterprojekter.
