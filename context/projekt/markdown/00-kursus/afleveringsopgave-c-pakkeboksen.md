# Afleveringsopgave C — Pakkeboksen

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Aflevering C — Domæneanalyse og applikationsmodel (afleveres uge 11 / KW46 ved L21, feedback L24). Ingen peer-review — direkte til godkendelse. |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `Afleveringsopgave C - Pakkeboksen.pdf` (2 sider; sidehoved "SWISE Forår 2025") |
| **Type** | øvelse (obligatorisk aflevering) |
| **Emner dækket** | Domænemodel (navneordsanalyse, konceptuelle klasser, associationer, multipliciteter, attributter), applikationsmodel med boundary/controller/domain-klasser, Sequence Diagram for applikationsmodel, metoder i klassediagram |

---

## Beskrivelse af Pakkeboksen

Pakkeboksen består af en Computer tilkoblet Wi-Fi, Printer, Touchskærm samt Boksstyring til at låse og åbne bokse med plads til 4 bokse, som vist i figur 1.

*Figur 1 — Skitse af pakkeboksen: samme skitse som i opgave A (ramme "PAKKEBOKSEN" med Computer, Wi-Fi, Touchskærm, Printer, Boks 1–4 og Boksstyring).*

### Forenklet Hovedscenarie

1. Pakkeboksen (eller Systemet) anmoder om at indtaste en 4-cifret pinkode
2. Kunden indtaster 4-cifret pinkode
3. Pakkeboksen validerer pinkode og åbner for tilhørende boks
4. Touchskærm viser: "Tag pakken fra boks <nr.> og luk igen"
5. Kunden tager pakken og lukker boksen
6. Boksstyringen detekterer at boksen er tom og lukket
7. Boksstyringen sender besked til computeren
8. Computeren printer en kvittering
9. Computeren sender besked til den centrale pakkeserver at pakken er afhentet

> Side 1

## Opgave 1

Udarbejd en domænemodel for "Pakkeboksen" med udgangspunkt i den forenklede hovedscenariebeskrivelse for use casen "Hente Pakke" ovenfor.

- Find konceptuelle klasser med brug af navneordsanalyse og kategorierne beskrevet i listen over generelle begreber i artiklen om Domænemodeller
- Tegn en domænemodel med konceptuelle klasser og associationer
- Angiv associationernes navne, læseretning og multipliciteter
- Angiv relevante attributter som f.eks. vandstand samt tilstand af ventiler og porte

(Sidste punkt nævner "vandstand", "ventiler" og "porte" — det er ordret fra kilden og stammer tydeligvis fra slusesystem-opgaven; for pakkeboksen er de relevante attributter fx pakkenummer, boksnummer, pinkode, boksens tilstand.)

## Opgave 2

Der skal designes software til computeren, som styrer pakkeboksen for use casen "Hent pakke". Lav et klassediagram for applikationsmodellen, hvor "boundary" og "controller" klasser (indgår). Du skal som minimum medtage domain-klasser, der indeholder information om pakker med attributter som: Pakkenummer, boksnummer og pinkode.

Du skal ikke lave en domænemodel. Du skal foreslå associationer, multipliciteter og attributter, men ikke metoder.

## Opgave 3

Lav et Sequence Diagram (SD) for use case "Hente Pakke" til applikationsmodel for interaktionen mellem klasserne vist i ovenstående klassediagram. Du skal udelukkende behandle hovedscenariet for use casen "Hent pakke". Udvidelser/undtagelser til hovedscenariet skal ikke medtages.

## Opgave 4

Opdater klassediagrammet i applikationsmodellen med metoder fundet i Opgave 3.

Der skal ikke udarbejdes en tilstandsmaskine, STM, for applikationsmodellen (men du må gerne, hvis du har lyst).

> Side 2
