# Uge 1.2 — Extreme Programming (XP)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Uge 1 — Extreme Programming som procesramme om softwaredesign |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Forelæser** | Henrik Bitsch Kirk (HK) |
| **Kilde** | `Extreme Programming.pdf` (18 slides) |
| **Sprog/kode** | C# (ingen kode på disse slides) |
| **Emner dækket** | Motivation for XP, historie (Kent Beck, C3, Agile Manifesto), XP-paradigmet, de 5 values, de 14 principles, practices, rules (managing, planning, designing, coding, testing), opstart med XP i nyt og eksisterende projekt |

---

## Agenda

1. Why — hvorfor XP overhovedet
2. History — hvor XP kommer fra
3. What
   - Values
   - Principles
   - Practices

> Slide 2

---

## 1. Why

**Better quality in software** …and responsiveness to changing requirements.

Det opnås gennem:

- frequent releases
- improved productivity

> Slide 3

Den centrale mekanisme er **planning/feedback loops** med vidt forskellige tidsskalaer, indlejret i hinanden. Fra langsomst til hurtigst:

| Loop | Tidsskala |
|---|---|
| Release plan | Months |
| Iteration plan | Weeks |
| Acceptance test | Days |
| Stand-up meeting | One day |
| Pair negotiation | Hours |
| Unit test | Minutes |
| Pair programming | Seconds |
| Code | — |

> Slide 3 (kilde: https://en.wikipedia.org/wiki/Extreme_programming)

Pointen med de indlejrede loops: jo hurtigere feedbacken kommer, jo billigere er fejlen at rette. Pair programming giver feedback på sekunder, unit tests på minutter — længe før en acceptance test eller en release ville have fanget problemet.

---

## 2. History

XP blev udviklet af **Kent Beck**, mens han arbejdede på Chryslers payroll system (C3).

Beskrevet i *Extreme Programming explained* fra 1999.

XP startede den Agile-revolution, der førte til **Agile Manifesto**.

XP introducerer et antal:

- Values
- Principles
- Practices

> Slide 4

Den trelagsstruktur er værd at holde fast i: **values** er hvorfor, **principles** er hvad man forsøger at opnå, og **practices** er de konkrete daglige handlinger. Practices uden values bliver kult; values uden practices bliver plakater.

---

## 3. What — XP som proces

XP er en **lightweight agile process**:

- Mostly for small-to-medium sized teams
- **Social change** — increases collaboration
- **Focus on creating value for the customer** — by delivering often, improving quality, eliminating defects
- **12 key practices taken to their extreme**

> Slide 5

Navnet "extreme" er bogstaveligt ment: hvis code review er godt, så review hele tiden (pair programming); hvis test er godt, så test før du koder (test-driven development); hvis integration er godt, så integrér flere gange dagligt (continuous integration).

Projektflowet på højniveau går fra *User Stories* og *Architectural Spike* → *Release Planning* → *Iteration* → *Acceptance Tests* → *Small Releases*, med *Spike* til at omsætte uncertain estimates til confident estimates, og med bugs og nye user stories, der føres tilbage i loopet.

> Slide 5 (kilde: http://www.extremeprogramming.org/)

### XP-paradigmet

- Stay aware
- Adapt
- Change

**Change will happen. XP lets you adapt.**

> Slide 6

Iterationsloopet konkretiserer det: *Release Plan* leverer user stories og *Next Iteration* leverer project velocity ind i *Iteration Planning*; failed acceptance tests og bugs føres samme vej. Iteration planning giver en iteration plan til *Development*, som dag for dag producerer *Latest Version* via new functionality og bug fixes, og som gennem "learn and communicate" producerer nye user stories og opdateret project velocity.

> Slide 6

---

## 4. De 5 values

| Value | Indhold |
|---|---|
| **Communication** | building software requires good communication. customers + colleagues |
| **Simplicity** | start with the simplest solution (**YAGNI**) |
| **Feedback** | from system, customer, and team |
| **Courage** | Speak the truth, seek answers. To adapt |
| **Respect** (added in the second edition) | team members and project |

> Slide 7

Værdicyklussen på sliden: *Unfinished Features* → *Most Important Features* → *Iterative Planning* → *Honest Plans* → *Daily Communication* → *Team Empowerment* → *Working Software* → tilbage til *Iterative Planning*, med *A Project Heartbeat* i midten.

> Slide 7 (kilde: http://www.extremeprogramming.org/)

**Simplicity** og **YAGNI** ("You Aren't Gonna Need It") er de to, der rammer softwaredesign hårdest. De står i direkte spænding med instinktet om at bygge fleksibilitet ind på forhånd — flere interfaces, flere abstraktionslag, flere extension points, som ingen ender med at bruge. XP's svar er, at den fleksibilitet betaler man for nu og høster måske aldrig; byg det simpleste, der virker, og refaktorér, når behovet faktisk viser sig.

---

## 5. De 14 principles

Principles danner grundlag for XP — baseret på values. De giver en bedre idé om, hvad practices er tænkt at opnå.

| | |
|---|---|
| Humanity | Flow |
| Economics | Opportunity |
| Mutual benefit | Redundancy |
| Self-similarity | Faillure |
| Improvement | Quality |
| Diversity | Baby steps |
| Reflection | Accepted responsibity |

> Slide 8 (stavemåderne `Faillure` og `responsibity` er slidens egne)

**Baby steps** og **Improvement** er dem, der direkte understøtter iterativt design: små skridt, som hver især kan verificeres og rulles tilbage, frem for store designspring.

---

## 6. Practices — the day-to-day things

| | |
|---|---|
| Sit together | Pair programming |
| Whole team | User Stories |
| Informative workspace | Weekly cycle |
| Energized work | Quartable cycle |
| | Slack |
| | 10 min build |
| | Continuous integration |
| | Test first |
| | Incremental design |

> Slide 9

De fire til venstre handler om arbejdsmiljø og teamsammensætning; de ni til højre om selve arbejdsrytmen og den tekniske praksis.

**Incremental design** og **Test first** er de to praksisser, der direkte er designdisciplin: designet vokser sammen med koden i stedet for at blive fastlagt op front, og testen skrives før implementationen, hvilket tvinger dig til at formulere interfacet, før du bygger indmaden.

---

## 7. Rules

XP's regler grupperes efter aktivitet. De fem grupper dækker tilsammen hele projektforløbet.

### Managing

- Open workspace
- Sustainable pace
- Stand up meetings
- Velocity
- Move people
- FIX XP

> Slide 10

Development-loopet, der ligger bag: *Iteration Plan* giver tasks til *Stand Up Meeting*; "too much to do" giver unfinished tasks; "share" fører til learn and communicate (pair programming, refactor mercilessly, move people around, CRC cards); næste task eller failed acceptance test fører til *Collective Code Ownership*, som ved 100% unit tests passed giver new functionality og ved acceptance test passed giver bug fixes. Failed acceptance tests og "day by day" føder tilbage i stand up meeting.

> Slide 10

### Planning

- User stories
- Release planning
- Frequent releases
- Iterative
- Iteration planning

> Slide 11

### Designing

- Simplicity
- System metaphor
- CRC Cards
- Spike solutions
- No functionality added early
- Refactor

> Slide 12

Det er den gruppe, der griber direkte ind i resten af kursets pensum. **Simplicity** og **No functionality added early** er YAGNI omsat til designregel. **CRC Cards** (Class-Responsibility-Collaboration) er en teknik til at finde klasser og ansvar — direkte forbundet med Single Responsibility Principle fra SOLID og med klassediagrammerne fra uge 1. **Refactor** er forudsætningen for, at incremental design kan fungere: uden løbende refaktorering degenererer et inkrementelt design til rod. **Spike solutions** er små eksperimenter, der reducerer teknisk usikkerhed, før man forpligter sig i et design.

Designloopet på sliden (Collective Code Ownership): *Next Task or Failed Acceptance Test* → pair up → *Create a Unit Test* → *Pair Programming* → *Continuous Integration* → run all unit tests → *100% Unit Tests Passed*. Sideveje: simple design/complex problem fører til *CRC Cards*; simple code/complex code fører til *Refactor Mercilessly*; change pair/"we need help" fører til *Move People Around*; failed unit test og passed unit test cykler mellem unit test og pair programming; run failed acceptance test fører til *Acceptance Test Passed*.

> Slide 12 (kilde: http://www.extremeprogramming.org/)

### Coding

- Customer available
- Code standards
- **TDD**
- Pair program
- Sequential code integration
- Integrate often
- CI environment
- Collective ownership

> Slide 13 (sliden viser også et *Coding Dojo*-logo)

### Testing

- Test all code
- Pass before release
- Prove bug by unit-test — then fix
- Acceptance are run often

> Slide 14

"Prove bug by unit-test, then fix" er reglen med den største effekt på kodekvalitet over tid: hver rettet fejl efterlader en test, der forhindrer regression. Sliden viser desuden et **Bottleneck**-diagram med story points fordelt på Total Committed / In Development / In Testing / Done hen over en sprint — kurverne viser, hvor arbejdet hober sig op.

> Slide 14

---

## 8. Sådan kommer man i gang med XP

### Nyt projekt

- **User stories** — 1-3 weeks
- **Spike solution** — risk mitigation
- **Release planning** — invite the whole team
- **Begin iterative development**

*Now you have started.*

> Slide 15

### Eksisterende projekt

- **What is slowing the project down**
- **Start by fixing this**

Eksempler på diagnose → modtræk:

| Symptom | Modtræk |
|---|---|
| Many bugs | automated acceptance tests |
| Requirement specifications | start with user stories |
| One/two developers are bottlenecks | collective code ownership |

> Slide 16

Pointen for et eksisterende projekt er, at man ikke indfører XP som en pakke. Man finder flaskehalsen og indfører den praksis, der adresserer præcis den.

---

## 9. XP's berøring med resten af kursets pensum

- **Incremental design + Refactor** — designet i dette kursus vokser iterativt; SOLID-principperne er kriterierne for, hvornår en refaktorering forbedrer designet.
- **Simplicity / YAGNI / No functionality added early** — modvægten til overdesign. Et interface indføres, når et konkret behov kræver det (fx DIP), ikke på forhånd.
- **CRC Cards** — en metode til at fordele ansvar mellem klasser; hænger direkte sammen med Single Responsibility Principle og med UML-klassediagrammer.
- **Test first / TDD** — tvinger dig til at designe interfacet før implementationen, hvilket presser designet mod løs kobling og testbare afhængigheder (og dermed mod Dependency Inversion).
- **Continuous integration + 10 min build** — den infrastruktur, der gør hyppig refaktorering forsvarlig.
- **System metaphor** — et fælles sprog for arkitekturen; forløberen for det, domænemodellering og designmønstre gør mere formelt senere i kurset.

---

## Referencer

- https://xkcd.com/2166/

> Slide 18

- Diagrammer gennem hele deck'et: http://www.extremeprogramming.org/ (Copyright 2000 J. Donvan Wells)
- Feedback-loop-figur: https://en.wikipedia.org/wiki/Extreme_programming

---

## Opsummering

- XP sigter mod **better quality in software** og **responsiveness to changing requirements** via frequent releases og improved productivity.
- Kent Beck, Chrysler C3, *Extreme Programming explained* (1999) — startskuddet til Agile Manifesto.
- Strukturen er tre lag: **5 values** (communication, simplicity, feedback, courage, respect), **14 principles**, og et sæt konkrete **practices**.
- Mekanismen er indlejrede feedback-loops fra sekunder (pair programming) til måneder (release plan) — jo hurtigere loop, jo billigere fejl.
- **Change will happen — XP lets you adapt.** Derfor incremental design frem for big design up front.
- Designreglerne (simplicity, system metaphor, CRC cards, spike solutions, no functionality added early, refactor) er dem, der direkte former softwaredesign-arbejdet i resten af kurset.
- I et eksisterende projekt indføres XP ikke som pakke: find flaskehalsen, indfør den praksis der løser den.
