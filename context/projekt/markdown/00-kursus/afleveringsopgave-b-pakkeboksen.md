# Afleveringsopgave B — Pakkeboksen

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Aflevering B — Systemstruktur og adfærd (SysML) (afleveres uge 8 / KW43 ved L15, feedback L17). Peer-review via FeedbackFruits. |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `Afleveringsopgave B - Pakkeboksen.pdf` (3 sider; sidehoved "SWISE Forår 2025") |
| **Type** | øvelse (obligatorisk aflevering) |
| **Emner dækket** | SysML BDD med blokke og ports, IBD med connectors og port-typer, system Sequence Diagram (SD), State Machine Diagram (STM) med triggers/guards/actions |

---

## Beskrivelse af Pakkeboksen

Pakkeboksen består af en Computer tilkoblet Wi-Fi, Printer, Touchskærm samt Boksstyring til at låse og åbne bokse med plads til 4 bokse, som vist i figur 1.

*Figur 1 — Skitse af pakkeboksen: samme skitse som i opgave A (ramme "PAKKEBOKSEN" med Computer, Wi-Fi, Touchskærm, Printer, Boks 1–4 og Boksstyring).*

> Side 1

## Opgave 1

Tegn et SysML Block Definition Diagram (BDD) af Pakkeboksen bestående af blokke og porte som beskrevet i nedenstående tabel.

Den elektroniske del af pakkeboksen er beskrevet nedenfor med blokke, hvor kun de elektriske og trådløse kommunikationsporte er beskrevet i nedenstående tabel.

| Block | Beskrivelse | Ports |
|---|---|---|
| Pakkeboksen | Indeholder de elektriske blokke beskrevet i denne tabel. | `inout net: Wi-Fi` |
| Touchskærm | Touchskærm, der viser tekst og information til kunden og giver mulighed for betjening. | `in disp: HDMI`<br>`inout touch: ~USB` |
| Printer | Printer kvittering til kunde | `inout printer: ~USB` |
| Computer | Varetager afvikling af software for betjening af pakkeboksen. Computeren indeholder den nødvendige hardware for at kunne sende Wi-Fi. | `inout net: Wi-Fi`<br>`inout printer: USB`<br>`out disp: HDMI`<br>`inout controller: SPI`<br>`inout touch: USB` |
| Boksstyring | Styrer åbning og lås af pakkebokse. Detekterer om boksen er åbnet/lukket og tom. | `inout ctrl: ~SPI`<br>`inout lock[4]: ~BOX` |
| Boks | Boks med dør, der automatisk kan låses op. Der er 4 bokse i pakkeboksen. | `inout lock: BOX` |

(`~` markerer konjugeret port.)

## Opgave 2

Lav et SysML Internal Block Diagram (IBD) for block'en "Pakkeboksen" baseret på port- og block-definitionerne i ovenstående tabel.

Portene af typen USB, SPI og Wi-Fi er ikke-atomiske og kan betragtes som kendte. BOX-signalet er ikke-atomisk, med digitale signaler til åbning og lås af boksen, samt sensorsignaler med information om boksen er åbnet/lukket og tom. Disse signaler skal ikke specificeres yderligere. Alle andre porte er atomiske. Pakkeboksen har forbindelse til den centrale postserver og internettet via Wi-Fi-forbindelsen. SMS sendes via internettet når postbuddet kommer med nye pakker.

> Side 2

## Opgave 3

Lav et SysML system Sequence Diagram (SD) for interaktionen mellem aktørerne og de blokke, pakkeboksen består af (se opgave 2). Du skal udelukkende behandle hovedscenariet for use casen "Hent pakke", som du definerede i løsningen til opgave 3 (i opgave A). Udvidelser/undtagelser til hovedscenariet skal ikke medtages.

Her er det simplificerede hovedscenarie for use case "Hent pakke" (OBS: Dette er IKKE det fulde hovedscenarie for løsningen til opg. A, men en forenklet udgave til at bruge i Sequence Diagrammet):

1. Vælger menu "Hent Pakke"
2. Anmod om pinkode
3. Indtast pin
4. Pinkode er indtastet
5. Pinkode til computer
6. Valider pinkode
7. Åbn boks nr.
8. Åben boks
9. Tag pakke fra boks <nr> og luk igen
10. Tag pakke
11. Tager pakker og lukker boks
12. Boks tom og lukket
13. Boks nr. tom
14. Print kvittering
15. Kvittering er printet
16. Pakkenummer afhentet

(*Det behøver ikke være præcis sådan, men det er et forslag og en reference.*)

## Opgave 4

Lav et SysML State machine diagram (STM) for computeren til styring af pakkeboksen. Brug tilstande, triggers, guards og actions som beskrevet i nedenstående tabel. Besked som vises på Touchskærmen er angivet i anførselstegn. Triggeren *Afbryd* skal ikke medtages i STM.

| Tilstand | Triggers[guard] | Actions |
|---|---|---|
| Hovedmenu (Afventer menuvalg) | Hent pakke | "Indtast pinkode" |
| Afventer pinkode (Afventer indtastning af pinkode) | Pinkode | Valider pinkode |
| Godkender pinkode (Afventer validering af pinkode) | Pinkode ok | Åben boks, start timer, "Tag pakken fra boks <nr.> og luk igen" |
| | Forkert pinkode | "Forkert pinkode" |
| Afventer tom boks (Afventer at pakke fjernes fra boks) | Boks tom | Print kvittering, send besked til pakkeserver |
| | Timeout [tid > 60 sec.] | Lås boks, "Pakke ikke fjernet" |

Tabellen angiver kun triggers og actions pr. tilstand — ikke måltilstanden. Måltilstandene må udledes af beskrivelsen i opgave A (fx *Hent pakke* → Afventer pinkode; *Pinkode* → Godkender pinkode; *Pinkode ok* → Afventer tom boks; *Forkert pinkode* → Afventer pinkode; *Boks tom* og *Timeout* → Hovedmenu). Dette er en tolkning, ikke en del af opgaveteksten.

> Side 3
