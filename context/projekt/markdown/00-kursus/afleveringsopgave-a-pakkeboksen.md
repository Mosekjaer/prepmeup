# Obligatorisk Afleveringsopgave A — Pakkeboksen

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Aflevering A — Specifikation og validering (afleveres uge 4 / KW38 ved L8, feedback L10). Peer-review via FeedbackFruits. |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `Obligatorisk Afleveringsopgave A - Pakkeboksen.pdf` (3 sider; sidehoved "SWISE Efterår 2025") |
| **Type** | øvelse (obligatorisk aflevering) |
| **Emner dækket** | Aktør-kontekstdiagram, aktørbeskrivelse, use case diagram, fully dressed use case, ikke-funktionelle krav (FURPS+), MoSCoW-prioritering, accepttest-cases |

---

## Beskrivelse af Pakkeboksen

I de følgende opgaver, skal du specificere og designe en pakkeboks til midlertidig opbevaring af pakker. Kunden skal kunne afhente pakker uden for postkontorets åbningstider. Pakker bliver opbevaret i pakkeboksen, og kunden har mulighed for at hente sine pakker 24 timer i døgnet. Pakkeboksen består af en Computer tilkoblet Wi-Fi, Printer, Touchskærm samt Boksstyring til at låse og åbne bokse med plads til 4 bokse, som vist i figur 1.

*Figur 1 — Skitse af pakkeboksen: Én stor ramme mærket "PAKKEBOKSEN". Øverst to stiplede bokse "Computer" (venstre) og "Wi-Fi" (højre). Under dem "Touchskærm" (venstre, stor) og "Printer" (højre, lille). Derunder fire vandrette rektangler "Boks 1", "Boks 2", "Boks 3", "Boks 4" over hinanden. Nederst en boks "Boksstyring". Ingen forbindelseslinjer er tegnet.*

Pakkeboksen har 4 rum (bokse), som kan åbnes og aflåses med en låge. I hvert rum er der plads til en eller flere pakker til en bestemt kunde. Touchskærmen viser en menu, hvor det er muligt at vælge funktionerne "Hent pakke" og "Placer pakke" for henholdsvis kunde og postbud.

Når postbuddet ankommer med nye pakker, vælger han/hun "Placer pakke" og bruger et pakkenummer til at åbne ledige bokse, hvor tilhørende pakke bliver placeret og registreret. Systemet sender en SMS til kunden om at pakken er klar til afhentning.

Når kunden ankommer til pakkeboksen, vælger han/hun funktionen "Hent Pakke". Kunden bliver herefter anmodet om at indtaste en 4-cifret pinkode, som han/hun har modtaget via SMS, da pakken blev klar til afhentning. Pinkoden bruges til at låse op for boksen med pakken. Hvis pinkoden er ukendt, anmodes om ny indtastning. Når pinkoden er verificeret og godkendt, åbner boksstyringen for boksen med kundens pakke. Touchskærmen viser "Tag pakken fra boks <nr.> og luk igen", hvorefter kunden tager pakken og lukker boksen. Boksstyringen detekterer om boksen er åbnet/lukket og tom og sender besked til computeren, hvis boksen blev tømt. Computeren printer en kvittering og sender besked via Wi-Fi til en central pakkeserver, om at pakken er afhentet.

Hvis kunden ikke åbner boksen og fjerner pakken inden for 60 sekunder, låses boksen automatisk igen. Herudover kan kunden også afbryde (afbryd-knap) på Touchskærmen, hvorefter boksen skal låses eller forblive låst.

> Side 1–2

## Opgave 1

Tegn et aktør-kontekst diagram for pakkeboksen. Beskriv kort hver aktør (Aktørbeskrivelse) til pakkeboksen.

## Opgave 2

Tegn et use case diagram for pakkeboksen med udgangspunkt i beskrivelsen ovenfor.

## Opgave 3

En oplagt use case for pakkeboksen er "Hent pakke". Giv en fully dressed use case-beskrivelse for hovedscenariet med udgangspunkt i beskrivelsen ovenfor. Medtag udvidelser/undtagelser, som kan læses ud af beskrivelsen af pakkeboksen. Evt. fejl i pakkeboksens komponenter skal ikke medtages i Use Case-beskrivelsen.

Brug skabelonen nedenfor:

| Felt | |
|---|---|
| Navn: | |
| Mål | |
| Initiering | |
| Aktører | |
| Antal samtidige forekomster | |
| Prækondition | |
| Postkondition | |
| Hovedscenarie | |
| Udvidelser/undtagelser | |
| Datavariationsliste | |

## Opgave 4

Specificer 8 ikke-funktionelle krav for pakkeboksen, med brug af FURPS+ (se slides omhandlende "System specification"). Find selv på krav, der som minimum omhandler kategorierne: pålidelighed (reliability herunder MTBF), sikkerhed, performance og support. Prioriter kravene med brug af MoSCoW metoden. Formuler krav med: skal (must), bør (should) og kunne (could).

> Side 2

## Opgave 5

Skriv en specifikation af to test cases til accepttesten, der hver især tester ét af følgende to scenarier i use casen specificeret i opgave 3.

- Hovedscenariet
- Udvidelse/undtagelse: Kunden trykker afbryd-knap på touchskærmen

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

> Side 3
