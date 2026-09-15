# Uge 1.1a — Course Intro: Introduction to Software Design

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Uge 1 — Introduction (kursusintroduktion) |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Forelæser** | Henrik Bitsch Kirk (HK) |
| **Kilde** | `Course Intro.pdf` (30 slides) |
| **Sprog/kode** | C# |
| **Emner dækket** | Undervisere, læringsmål (qualifications), semesterplan, undervisningsform, litteratur, eksamensform, obligatorisk opgave, forventninger, hacker/developer/engineer-modellen |

---

## Agenda

1. Undervisere og TA
2. Læringsmål (qualifications) — hvad du skal kunne efter kurset
3. Semesterplan (initial schedule)
4. Undervisningsform og litteratur
5. Eksamen og obligatorisk opgave
6. Forventninger — begge veje
7. Hvorfor software design? Hacker → Developer → Engineer

---

## 1. Undervisere

Forelæsningerne deles mellem to undervisere, og der er tilknyttet én TA.

| Rolle | Navn | Kontakt |
|---|---|---|
| Forelæser | Jørn Martin Hajek (HAJ) | haj@ece.au.dk |
| Forelæser | Henrik Bitsch Kirk (HK) | henrik@ece.au.dk |
| TA | Khaled Rami Omar | — |

> Slide 3–4

Jørn Martin Hajek har en kandidatgrad i datalogi fra AU, kommer fra det danske mindretal i Tyskland og har arbejdet som udvikler hos Niras, Scandiatransplant, LandIT (FarmerDating) og Symfoni. Han har undervist i IT siden 2011 på Erhvervsakademi Aarhus, SmartLearning og VIA University College.

> Slide 5

Henrik Bitsch Kirk er MSc i Computer Science fra 2009, arbejdede som udvikler 2009–2017 og har været på ECE siden 2017. Han cykler.

> Slide 6

Slide 7 viser eksempler på systemer underviserne har arbejdet med — biblioteks-søgesystemer, en race-results-app til web/tablet/mobil, industriel procesanlægsvisualisering, digitale avisudgivelser og kliniske oversigtsskærme i hospitalsmiljø. Pointen er at software design ikke er en akademisk øvelse: de samme designprincipper går igen på tværs af meget forskellige domæner.

> Slide 7

---

## 2. Læringsmål (Qualifications)

Kursets officielle læringsmål er formuleret med verber fra **SOLO-taksonomien**, hvor verbet angiver hvilket niveau af forståelse der kræves. Det er værd at læse verberne nøje — de siger direkte hvad eksamen kan spørge om. At *explain* er ikke nok hvis målet siger *combine, compare, analyse*.

Efter kurset skal du kunne:

- **Use** UML as a modeling and documentation tool.
- **Explain, combine and use** fundamental design principles for object oriented software development.
- **Explain and use** the term software architecture and related design principles.
- **Explain, combine, compare, analyse and use** selected design patterns.
- **Describe, compare, and develop** software with concurrency.
- **Work** independently and assume responsibility for own learning and technical focus.
- **Carry out** an oral presentation of the result of own investigations.

> Slide 8–10

Bemærk vægtfordelingen: design patterns er det eneste område hvor alle fem verber er i spil (explain, combine, compare, analyse, use) — det er kursets tyngdepunkt. Concurrency ligger et niveau lavere (describe, compare, develop), og UML kræver kun at du kan *bruge* det som værktøj.

SOLO-taksonomien: <https://educate.au.dk/en/focus-areas/learning-objectives-and-taxonomies>

> Slide 10

---

## 3. Semesterplan

Undervisningen ligger mandag 12.15–13.50 (lokale 5125-430) og fredag 10.15–12.50 (lokale 5123-111). Planen viser den faglige progression: OO-grundlag → design smells og SOLID → arkitektur → design patterns → refactoring/DDD → concurrency → error handling.

| # | Uge | Mandag | Fredag |
|---|---|---|---|
| 1 | 35 | HK: Introduction + OO Basic | HK: Extreme Programming |
| 2 | 36 | HAJ: Design smells + SOLID: SRP, OCP | HAJ: SOLID: SRP, OCP |
| 3 | 37 | HK: SOLID: LSP, ISP, DIP | HK: SOLID: LSP, ISP, DIP |
| 4 | 38 | HK: Software Architecture | HK: Software Architecture |
| 5 | 39 | HAJ: Software Architecture | HAJ: Software Architecture |
| 6 | 40 | HK: Software design patterns + GoF Observer | HK: GoF Observer (cont.) |
| 7 | 41 | HAJ: GoF Strategy + Template | HAJ: GoF Factory |
| — | 42 | Efterårsferie — ingen undervisning | Efterårsferie — ingen undervisning |
| 8 | 43 | HK: State patterns: Switch/case, GoF State pattern. *Introduction to mandatory exercise* | HK: State patterns: Switch/case, GoF State pattern. *Deadline for valg af pattern* |
| 9 | 44 | HAJ: Refactoring | HAJ: Domain Driven Design |
| 10 | 45 | HK: Mandatory Exercise | HAJ: Mandatory Exercise — **aflevering fredag** |
| 11 | 46 | HAJ: Task | HAJ: Parallel Loops |
| 12 | 47 | HAJ: Dependencies and Futures | HAJ: Pipelines |
| 13 | 48 | HK: Parallel Aggregation and MapReduce | HK: Error handling + kursusevaluering |
| 14 | 49 | HAJ: Error handling | HAJ/HK: **Final Questions from you** |
| 15 | 50 | Buffer | Buffer |

> Slide 11

De to vigtige datoer er uge 43 (introduktion til den obligatoriske opgave, og deadline for at vælge pattern) og uge 45 fredag (aflevering).

---

## 4. Undervisningsform og litteratur

Undervisningen består af forelæsninger med teori, kodeeksempler og gennemgang af øvelsesløsninger, kombineret med mange øvelser — nogle små, nogle store.

Der er **én obligatorisk gruppeopgave (hand-in) plus to individuelle reviews**. Begge dele skal være godkendt; er de ikke det, kan du ikke gå til eksamen.

> Slide 12–14

Pensum (findes på Brightspace) består af:

- Dele af bogen **"Head First Design Patterns"** (Freeman & Robson, 2. udgave)
- Uddrag af bøger tilgængelige online
- Artikler
- Videoer

Kodeeksempler i C# til Head First Design Patterns kan hentes på <https://github.com/jkhines/hfpatternsincsharp> og ligger også på Brightspace. Bogen selv bruger Java, så C#-porteringen er den relevante for dette kursus.

> Slide 15

Som alternativ bog anbefales **"Agile Principles, Patterns, and Practices in C#"** af Robert C. Martin og Micah Martin.

> Slide 16

---

## 5. Eksamen

Eksamensformen er:

- **Mundtlig eksamen, ca. 20 minutter inklusive votering.**
- **Ekstern censor, 7-trinsskala.**
- **Ingen forberedelse.** Eksamensspørgsmålene udleveres på forhånd, engang i november. Sliden formulerer det som `using Exams.Standard;` — altså standard-eksamensform uden overraskelser.
- Den obligatoriske opgave (hand-in) skal være godkendt. Du kan ikke gå til eksamen hvis din hand-in og dit review ikke er godkendt.

> Slide 17–20

Konsekvensen af "ingen forberedelse" er at du skal kunne tale frit om spørgsmålene på forhånd — de 20 minutter går med fremlæggelse og dialog, ikke med at læse op.

---

## 6. Forventninger

Undervisernes forventninger til dig:

- Du læser materialet og/eller ser videoerne og kigger på øvelsesbeskrivelserne **før** timen. Fra en tidligere evaluering: *"The lectures are difficult to follow if you haven't prepared by reading beforehand."*
- Du møder op **og** deltager i diskussionerne i klassen.
- Du spørger om det du har svært ved at forstå — føler du sådan, er der garanteret medstuderende der føler det samme.
- Du løser øvelserne. Der forventes også at du bruger tid på at programmere **hjemme**.
- Du hjælper dine medstuderende.

> Slide 21–24

Omvendt bliver de studerende bedt om at diskutere med sidemanden: Hvad forventer du at lære af kurset? Hvad glæder du dig til? Hvad tror du bliver svært? Er der noget du håber at vi, underviserne, gør?

> Slide 25

---

## 7. Hvorfor software design? Hacker → Developer → Engineer

Kursets faglige begrundelse formuleres som en tredeling af udviklerroller (inspireret af David Mosher). Den er værd at tage alvorligt, fordi den præcist beskriver hvad kurset forsøger at flytte dig fra og til.

**Hacker.** En hacker kan finde på løsninger, men kan måske ikke kigge tilbage bagefter og se *hvordan* han kom frem til løsningen. Han prikker til tingene indtil noget virker.

**Developer.** En developer forstår *best practices*. Han har hørt andre udviklere sige ting som "du skal lægge dine scripts nederst på websiden" — og han bruger de best practices til at bygge løsninger, men forstår ikke rigtig hvad der ligger under dem, under abstraktionerne.

**Engineer.** En engineer er én der kan få tingene gjort og udforme en løsning — han forstår best practices, men forstår også *hvorfor* han bruger netop de best practices. Han bevæger sig op i en forståelse af platformen som helhed.

> Slide 26

Hele kurset sigter mod det tredje niveau: ikke bare at kende design patterns og SOLID-principperne, men at kunne begrunde hvornår og hvorfor de gælder — og hvornår de ikke gør.

Fra en tidligere kursusevaluering: *"Very relevant for the studies. Makes it easier to write code that can later be modified or expanded (has been a huge help in the semester project)." — Student F26*

> Slide 27

Slide 28 er en Q&A-slide, slide 29 lister billedreferencer (forsiden er xkcd 1513, Q&A-billedet er fra Pexels), og slide 30 er en afsluttende AU-logo-slide.

---

## Opsummering

- Kurset køres af HK og HAJ i fællesskab; læringsmålene er formuleret med SOLO-verber, og **design patterns** er det område hvor der kræves højest taksonomisk niveau (explain, combine, compare, analyse, use).
- Fagligt spænder semestret fra OO-grundlag og design smells over SOLID og software architecture til design patterns, refactoring/DDD, concurrency og error handling.
- Litteraturen er primært "Head First Design Patterns" (C#-kodeeksempler ligger på GitHub og Brightspace), med Robert C. Martins "Agile Principles, Patterns, and Practices in C#" som alternativ.
- Eksamen er mundtlig, ca. 20 min., ekstern censor, 7-trinsskala, ingen forberedelse — spørgsmålene udleveres i november.
- Én obligatorisk gruppeopgave plus to individuelle reviews skal godkendes; ellers ingen adgang til eksamen. Introduktion uge 43, aflevering uge 45.
- Kursets overordnede ambition: flytte dig fra *developer* (kender best practices) til *engineer* (forstår hvorfor de er best practices).
