# Eksamensopgave F2015: SmartFridge (med BDD/IBD-løsningsforslag til opg. 5 og 6)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L8 — SysML BDD og IBD (Brightspace "Exercise L8 SysML BDD, IBD: Tidligere Eksamsopgave F2015 — opg5, opg6") |
| **Kursus** | SWISE-01 Indledende System Engineering (tidligere I2ISE) |
| **Kilde** | `I2ISE Eksamensopgave F2015.pdf` (3 sider) + `SYSMLBDDSmartFridgeSolution.pdf` (1 side) + `SYSMLIBDSmartFridgeSolution.pdf` (1 side) |
| **Type** | eksamensopgave + løsningsforslag (kun opg. 5 og 6) |
| **Emner dækket** | ækvivalensklasser, kobling, use case diagram, fully dressed use case, system-sekvensdiagram, BDD med ports, IBD med parts/connectors |

---

## Opgavetekst

**Aarhus Universitet - ISE**
Eksamenstermin: Q4 eksamen – Sommer 2015. Prøve i: XX. Dato: XX.XX.XXX.

*Denne eksamen består af 6 opgaver. Opgave 1 og 2 udgør hver især 10% af den samlede eksamensopgave, opgaverne 3,4,5 og 6 udgør hver med 20%.*

### Opgave 1 (10%)

*Hvert spørgsmål udgør 5% af den samlede eksamensopgave.*

a. Hvad bruges ækvivalensklasser til i forbindelse med test?

b. Hvad betyder det, at et system har høj kobling?

### Introduktion til SmartFridge

De følgende opgaver omhandler *SmartFridge*, et intelligent køleskab der kan hjælpe en bruger med at vedligeholde en indkøbsliste. Køleskabet har en indbygget stregkodescanner, touchskærm, printer og Computer, samt WiFi-forbindelse, som giver køleskabet mulighed for at oprette forbindelse til en ekstern stregkode-database (*BarCode DataBase*, eller BCDB). Systemet er skitseret på Figur 1 nedenfor:

*Figur 1: SmartFridge med stregkodescanner, touchskærm, printer, WiFi-interface og Computer. (Skitse af et køleskab set forfra med fem komponenter placeret lodret i døren: Stregkodescanner, Touchskærm, Printer, WiFi (stiplet), Computer (stiplet).)*

SmartFridge har et interface til omverdenen som vist på Tabel 1:

| Komponent | Interface | Type | Retning |
|---|---|---|---|
| SmartFridge | barcode | Light | In |
| | push | Force | In |
| | info | Image | Out |
| | paper | Paper | Out |
| | wireless | IEEE 802.11 | In/out |

*Tabel 1: SmartFridge's interface til omverdnen*

> Side 1 af 3

De komponenter, der er indbygget i SmartFridge, har interfaces som vist i Tabel 2 nedenfor:

| Komponent | Interface | Type | Retning |
|---|---|---|---|
| Stregkodescanner | barcode | Light | In |
| | data | RS232 | Out |
| Touchskærm | hdmi | HDMI | In |
| | touch | USB | Out |
| | push | Force | In |
| | info | Image | Out |
| Printer | data | USB | In |
| | paper | Paper | Out |
| WiFi | data | WiFiData | In/out |
| | wireless | IEEE 802.11 | In/out |
| Computer | scanner | RS232 | In |
| | hdmi | HDMI | Out |
| | touch | USB | In |
| | printer | USB | Out |
| | WiFi | WiFiData | In/out |

*Tabel 2: Oversigt over komponenter og deres interfaces*

Der er tre brugssituationer for SmartFridge's indkøbsliste-funktioner. Disse er beskrevet i punktform herunder.

**Brugssituation 1: Tilføj vare**

- Når en vare ønskes tilføjet SmartFridge's indkøbsliste, scanner brugeren varens stregkode vha. SmartFridge's stregkode-scanner.
- SmartFridge sender stregkoden til BCDB. Hvis BCDB indeholder den pågældende stregkode returneres varens navn til SmartFridge, som præsenterer dette for brugeren på sin touchskærm. Hvis BCDB ikke indeholder den pågældende stregkode returnerer BCDB en fejlkode til SmartFridge. SmartFridge giver herefter brugeren mulighed for at angive varens navn. Når brugeren har gjort dette, sendes navnet og stregkoden til BCDB.
- SmartFridge giver brugeren mulighed for at redigere antallet af den pågældende vare, der skal tilføjes indkøbslisten vha. sin touchskærm.
- SmartFridge giver brugeren mulighed for at tilføje varen og antallet til indkøbslisten vha. sin touchskærm. Når brugeren gør det, tilføjer SmartFridge varen til indkøbslisten og viser denne på skærmen.
- Brugeren kan afbryde tilføjelsen af en vare til indkøbslisten.

> Side 2 af 3

**Brugssituation 2: Udskriv indkøbsliste:**

- Brugeren bruger SmartFridge's touchskærm til at starte en udskrift af indkøbslisten.
- SmartFridge udskriver indkøbslisten.

**Brugssituation 3: Slet indkøbsliste**

- Brugeren bruger SmartFridge's touchskærm til at starte en nulstilling af indkøbslisten.
- SmartFridge beder brugeren bekræfte nulstillingen. Hvis brugeren bekræfter nulstillingen, nulstilles indkøbslisten.

### Opgave 2 (10%)

Tegn et use case diagram for SmartFridge med udgangspunkt i brugssituationerne 1-3 ovenfor.

### Opgave 3 (20%)

Skriv en fully dressed use case for SmartFridge's Brugssituation 1: "Tilføj vare", som den er skitseret tidligere. Du skal medtage hovedscenariet og eventuelle undtagelser fra dette i din use case beskrivelse. Du skal bruge skabelonen nedenfor:

| Felt |
|---|
| Navn: |
| Initiering |
| Aktører |
| Antal samtidige forekomster |
| Prækondition |
| Postkondition |
| Hovedscenarie |
| Udvidelser/undtagelser |

### Opgave 4 (20%)

Lav et system-sekvensdiagram for use case beskrivelsen, som du lavede i Opgave 3.

### Opgave 5 (20%)

Lav et SysML Block Definition Diagram (BDD) for SmartFridge, baseret på systemskitsen på Figur 1 og komponenternes interface-beskrivelse i Tabel 1 og Tabel 2. Bemærk at interface-typen "WiFiData" ikke skal beskrives yderligere på BDD'et.

### Opgave 6 (20%)

Lav et SysML Internal Block Diagram (IBD) for SmartFridge baseret på dit BDD fra Opgave 5.

> Side 3 af 3

---

## Løsningsforslag opg. 5: BDD SmartFridge (SYSMLBDDSmartFridgeSolution.pdf)

Tegningen har ingen diagramramme. Alle seks blokke er «block». Kompositionen er tegnet som én sort diamant under `SmartFridge` med en samlet træ-linje til de fem dele. Ingen multipliciteter.

| Blok | Ports (som skrevet i tegningen) |
|---|---|
| **SmartFridge** | `In barcode : Light`, `In push : Force`, `Out info : Image`, `Out paper : Paper`, `Inout wireless : IEEE 802.11` |
| **Stregkodeskanner** | `In barcode:Light`, `out data : RS232` |
| **Touchskærm** | `in hdmi : HDMI`, `out touch : USB`, `in push : Force`, `out info : Image` |
| **Printer** | `in data : USB`, `out paper : Paper` |
| **WiFI** | `inout data: WiFiData`, `inout wireless : IEEE 802.11` |
| **Computer** | `in scanner : RS232`, `out hdmi : HDMI`, `in touch : USB`, `out printer : USB`, `inout WiFi : WiFiData` |

Relationer: `SmartFridge` ◆— `Stregkodeskanner`, `Touchskærm`, `Printer`, `WiFI`, `Computer`.

Bemærk:
- Ports på SmartFridge er præcis Tabel 1; ports på delene er præcis Tabel 2 (retning → `in`/`out`/`inout`).
- Stavning i tegningen afviger fra opgaven: `Stregkodeskanner` (opgave: Stregkodescanner), `WiFI` (opgave: WiFi). Gengivet ordret.
- `WiFiData` er kun brugt som porttype og ikke defineret som egen blok, jf. opgaveteksten.

---

## Løsningsforslag opg. 6: `ibd Smart Fridge` (SYSMLIBDSmartFridgeSolution.pdf)

Diagramramme: `ibd Smart Fridge`. Fem unavngivne parts: `: Stregkodeskanner`, `: WiFi`, `: Computer`, `: Printer`, `: Toucshskærm` (stavefejl i originalen).

### Boundary ports (på rammen)

| Side | Port | Retning |
|---|---|---|
| top | `wireless : IEEE 802.11` | inout (dobbeltpil) |
| venstre, øverst | `barcode : Light` | in |
| venstre, nederst (to ports) | `info : Image` | out (pil ud af rammen) |
| | `push : Force` | in |
| højre | `paper : Paper` | out |

### Connectors (med portnavne i hver ende)

| Fra | Til |
|---|---|
| boundary `barcode : Light` | `: Stregkodeskanner`.`barcode : Light` |
| `: Stregkodeskanner`.`data : RS232` | `: Computer`.`scanner : RS232` (top af Computer, venstre) |
| boundary `wireless : IEEE 802.11` | `: WiFi`.`wireless : IEEE 802.11` (top af WiFi) |
| `: WiFi`.`data : WiFiData` (bund af WiFi) | `: Computer`.`WiFi : WiFiData` (top af Computer, højre) |
| `: Computer`.`printer : USB` (højre) | `: Printer`.`data : USB` |
| `: Printer`.`paper : Paper` | boundary `paper : Paper` |
| `: Computer`.`hdmi : HDMI` (bund, venstre) | `: Toucshskærm`.`hdmi : HDMI` (højre øverst) |
| `: Toucshskærm`.`touch : USB` (højre nederst) | `: Computer`.`touch : USB` (bund, højre) |
| `: Toucshskærm`.`info : Image` (venstre øverst) | boundary `info : Image` — connectoren har en **item flow-pil** (sort trekant) pegende mod rammen/ud |
| boundary `push : Force` | `: Toucshskærm`.`push : Force` (venstre nederst) — connectoren har en item flow-pil pegende mod Toucshskærm |

De to connectors ved touchskærmen er de eneste med item flow-trekanter; de øvrige har kun retning via port-symbolerne (små pile i port-kasserne: `scanner`/`hdmi` peger ind i/ud af Computer, `touch` peger ind, `WiFi`/`wireless`/`data` er dobbeltpile).

### Pointer til eksamen

- IBD'et skal kun indeholde parts af de blokke der er dele af SmartFridge i BDD'et, og hver connector skal gå mellem ports med **samme type** (RS232↔RS232, USB↔USB, HDMI↔HDMI, WiFiData↔WiFiData, IEEE 802.11↔IEEE 802.11).
- SmartFridge's egne ports (Tabel 1) bliver boundary ports på rammen og forbindes til den del der realiserer dem: `barcode`→scanner, `push`/`info`→touchskærm, `paper`→printer, `wireless`→WiFi.
- Computer er "hub'en": alle fire andre dele forbindes til Computer, ingen til hinanden.
