# Ugeplaner — forberedelse og forventet tidsforbrug

## Metadata

| Felt | Værdi |
|---|---|
| **Type** | Ugentlige forberedelsesbeskrivelser |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Kilde** | `week2 plan.txt`, `week5 plan.txt` (Brightspace) |
| **Bemærk** | Kun uge 2 og uge 5 er downloadet. Øvrige ugers planer ligger i Brightspace. |

Se [SW4SWD-01_Lesson_Plan.md](SW4SWD-01_Lesson_Plan.md) for det fulde skema.

---

## Uge 2 — Design smells + SOLID: SRP, OCP

**Forelæser:** HAJ · **Slides:** [../slides/SW4SWD-01_W02a_Design_Smells.md](../slides/SW4SWD-01_W02a_Design_Smells.md), [../slides/SW4SWD-01_W02b_SOLID_SRP_OCP.md](../slides/SW4SWD-01_W02b_SOLID_SRP_OCP.md)

### Ugens indhold

Ugen motiverer hvorfor softwaredesign overhovedet er nødvendigt for at udvikle "god" software. Der startes med at diskutere hvad der karakteriserer *dårlig* software, og hvordan man undgår almindelige fejl og dårlige designs.

Derefter introduceres de fem begreber bag SOLID-akronymet. Hvert bogstav står for et designprincip, og principperne fungerer som fundament for alle de øvrige designs i kurset.

- **Mandag:** Design Smells og Single Responsibility Principle (SRP). Første del af øvelsen påbegyndes.
- **Fredag:** Open-Closed Principle (OCP) og anden del af øvelsen.

### Forberedelse — læs

- *The Principles and Patterns* — side 1 til 7 (eller Martin s. 104–107)
- *The SRP article* (eller Martin kapitel 8)
- *The OCP article* (eller Martin kapitel 9)

### Overvej inden forelæsningen

SRP siger at *"a class should only have one reason to change (i.e. a single responsibility)"*.

- Hvorfor er det sådan?
- Hvad er egentlig et "responsibility"?
- Hvad sker der, hvis vi ikke overholder SRP?

### Tidsforbrug

| Hvornår | Timer |
|---|---|
| Før forelæsning | 1–2 t. læsning af pensum + start på øvelse |
| Efter | 2–3 t. færdiggørelse af øvelse |

### Valgfrit supplement

**C2-wikien** er et godt sted at læse om softwaredesign. Mange designmønstre og tanker om softwaredesign og -processer blev oprindeligt diskuteret der — det var i øvrigt den første wiki der blev lavet.

Startpunkt: [CodeSmell på C2-wikien](http://wiki.c2.com/?CodeSmell)

> A code smell is a hint that something has gone wrong somewhere in your code. Use the smell to track down the problem. Kent Beck (with inspiration from the nose of Massimo Arnoldi) seems to have coined the phrase in the "OnceAndOnlyOnce" page, where he also said that code "wants to be simple". *Bad Smells in Code* was an essay by Kent Beck and Martin Fowler, published as Chapter 3 of *Refactoring: Improving the Design of Existing Code*.

Erfarne udviklere har en "fornemmelse" for godt design. Når de har nået en tilstand af *unconscious competence*, praktiserer de rutinemæssigt godt design uden at tænke synderligt over det — og de kan se på et design eller en kodebase og øjeblikkeligt fornemme kvaliteten uden at fortabe sig i lange, logisk detaljerede argumenter.

Bemærk at en code smell er et *hint* om at noget måske er galt — ikke en vished. Et udmærket idiom kan blive betragtet som en code smell, fordi det ofte misbruges, eller fordi der findes et enklere alternativ som virker i de fleste tilfælde. At kalde noget en code smell er ikke et angreb; det er blot et tegn på at et nærmere kig er på sin plads.

Mere hands-on:

- [JetBrains-bloggens artikelserie om code smells](https://blog.jetbrains.com/idea/2017/08/code-smells-null/)
- [10 coding mistakes that make your code smell](https://medium.com/@hussein_cheayto/10-coding-mistakes-that-make-your-code-smell-df548d99354f)

---

## Uge 5 — Software Architecture: Dokumentation 1

**Forelæser:** HAJ · **Slides:** [../slides/SW4SWD-01_W05_Architecture_Documentation.md](../slides/SW4SWD-01_W05_Architecture_Documentation.md)

### Ugens indhold

Ugen ser på hvordan softwarearkitektur kan dokumenteres. Arkitekturprocessen fra uge 4 fortsættes sideløbende.

### Forberedelse — læs

- **The C4 model for visualising software architecture** — mindst frem til hvor FAQ-afsnittet begynder. → [../artikler/SW4SWD-01_C4_Model.md](../artikler/SW4SWD-01_C4_Model.md)
- **4+1 architectural view** — definitionen på Wikipedia
- **4+1 med UML 2** — `41view-architecture_UML2.pdf` → [../artikler/SW4SWD-01_4plus1_View_UML2.md](../artikler/SW4SWD-01_4plus1_View_UML2.md)

### Forberedelse — se

**Visualising software architecture with the C4 model** — Simon Brown, Agile on the Beach 2019 (35 min.) → [../artikler/SW4SWD-01_C4_Talk_Simon_Brown.md](../artikler/SW4SWD-01_C4_Talk_Simon_Brown.md)

### Valgfrit

Kruchtens originalartikel fra 1995: `41view-architecture_1995.pdf` → [../artikler/SW4SWD-01_4plus1_View_Kruchten_1995.md](../artikler/SW4SWD-01_4plus1_View_Kruchten_1995.md)

Den oprindelige idé stammer fra Kruchten i 1995 og bruger en gammel OOD-notation, som senere blev smeltet sammen til UML — ideerne blev bevaret, symbolerne ikke. Det er netop derfor UML hedder *Unified* Modelling Language. Læsningen er valgfri og kan skimmes.

### Tidsforbrug

| Hvornår | Timer |
|---|---|
| Før forelæsning | 3–5 t. læsning af pensum + start på øvelse |
| Efter | 1–2 t. færdiggørelse af øvelse |
