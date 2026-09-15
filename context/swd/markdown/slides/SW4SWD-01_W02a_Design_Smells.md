# Uge 2a — Design Smells: The odors of Rotting Software

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Uge 2 — Design Smells |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Forelæser** | Jørn Martin Hajek (HAJ) — slides oprindeligt af Alexander Mølsted Hulgaard Rasmussen, teaching assistant / IT software engineer, 02 September 2023 |
| **Kilde** | `Design Smells - The Odors of Rotting Software.pdf` (11 slides) |
| **Sprog/kode** | C# |
| **Emner dækket** | Technical debt, software rot (what/why/how), Rigidity, Fragility, Immobility, Viscosity, Needless complexity (YAGNI), Needless repetitions (DRY), Opacity, refactoring som modtræk |

---

## Agenda

1. Technical debt — hvorfor et simpelt ønske tager lang tid
2. The what, why and how of software rot
3. Rigidity og Fragility
4. Immobility og Viscosity
5. Needless complexity (YAGNI) og Needless repetitions (DRY)
6. Opacity
7. Bottom line — refactoring som eneste vej ud

---

## 1. Technical debt

Åbningssliden er en tegneserie (Vincent Déniel) med titlen "Technical Debt". To bygningsarbejdere står foran et forfaldent hus — revnede mure, skæve søjler der holder facaden oppe, en paraply stukket ind i muren i stedet for tag, murbrokker på jorden. Den ene siger:

> "I don't understand why it takes so long to add a new window."

> Slide 1

Pointen er det grundlæggende misforhold, resten af forelæsningen handler om: den, der beder om ændringen, ser kun ændringens egen størrelse — ét vindue. Den, der skal udføre den, ser den akkumulerede struktur som ændringen skal ind i. Når konstruktionen allerede holdes sammen af hacks, er selv en lille tilføjelse dyr. Technical debt er metaforen for netop den akkumulerede omkostning: genveje taget tidligere skal betales tilbage med renter senere.

## 2. Design Smells — the oders of Rotting software

Titelsliden: **DESIGN SMELLS — The oders of Rotting software**. (Stavefejlen "oders" for "odors" står sådan på sliden.)

> Slide 2

Metaforen er central. Ligesom mad der er ved at fordærve afgiver en lugt før den er synligt fordærvet, afgiver et design der er ved at rådne op en række genkendelige symptomer, før systemet bryder sammen. En design smell er ikke i sig selv en fejl — koden virker typisk — men et signal om at strukturen er ved at miste evnen til at bære fremtidige ændringer.

## 3. The what, why and how of software rot

Software rot beskrives i tre led:

> - **What**: Inability to continuously support product
> - **Why**: Design does not support (unexpected) changes in requirements
> - **How**: Unexpected changes are hacked into existing code

> Slide 3

Kæden skal læses bagfra for at forstå mekanismen. Requirements ændrer sig på måder designet ikke forudså. Fordi designet ikke understøtter de ændringer, kan de ikke implementeres pænt — de hackes ind. Hver hack forringer strukturen en smule. Efter tilstrækkelig mange hacks er produktet ikke længere muligt at supportere kontinuerligt: hver ny ændring koster mere end den forrige, indtil udviklingshastigheden går i stå.

Bemærk at årsagen ikke er "dårlige programmører" eller "dårlig kode" isoleret set. Årsagen er at designet ikke var åbent for netop de ændringer, virkeligheden bragte. Det er derfor kurset senere arbejder med principper som Open/Closed — de handler præcis om at gøre designet i stand til at absorbere uventede ændringer uden hacks.

Slide 4 er en videoreference — et filmklip indlejret som billede med linket:

> https://www.youtube.com/watch?v=AbSehcT19u0

> Slide 4

## 4. Rigidity og Fragility

### Rigidity

> - **Symptom**: apparently simple changes causes a cascade of changes in related component
> - **Problem**: Managers and/or programmers don't dare to make non-critical changes

Illustrerende citat på sliden:

> "I just needed to change A, but there were some expectations to A hard-coded in B when it used C and D, so to change that I had to change B, C and D, but then…"

> Slide 5

Rigidity handler om **udbredelse af ændringer**. Symptomet er at en tilsyneladende triviel ændring udløser en kaskade af følgeændringer i relaterede komponenter. Problemet er organisatorisk lige så meget som teknisk: når konsekvenserne af en ændring ikke kan afgrænses, tør hverken ledelse eller udviklere røre ved noget der ikke er kritisk. Dermed bliver ikke-kritiske forbedringer aldrig lavet, og systemet forfalder yderligere — rigidity er selvforstærkende.

Årsagen i citatet er værd at bemærke: `B` havde antagelser om `A` hard-kodet ind i sig. Det er kobling gennem implicitte forventninger frem for gennem eksplicitte kontrakter.

### Fragility

> - **Symptom**: Fixing one problem introduces more problems
> - **Problems**: Software like this is impossible to maintain. Programmers cannot trust that errors isn't introduced.

Illustrerende citat:

> "Fixing A looked really simple. I didn't expect B to break, though, and when I tried to fix that I inadvertently broke C and D, too…"

> Slide 5

Fragility er rigidity's onde tvilling. Hvor rigidity betyder at ændringer *kræver* mange andre ændringer, betyder fragility at ændringer *knækker* ting — og typisk i moduler der ikke har nogen begrebsmæssig sammenhæng med det man rettede. Konsekvensen er tab af tillid: udvikleren kan ikke længere ræsonnere sig frem til om en rettelse er sikker, og software man ikke kan ræsonnere om, er umulig at vedligeholde.

## 5. Immobility og Viscosity

### Immobility

> - **Symptom**: A component could be reused in some other place, but depends on to much 'bagage', for it to be easily reused
> - **Problem**: Software components are not reused, but rewritten

Illustrerende citat:

> "The `Calendar` class really has generic functionality, but it relies on the DS1339A RTC, so we can't reuse the software"

> Slide 6

Immobility handler om **uønsket afhængighed**. Komponenten indeholder generisk, genbrugelig funktionalitet, men den er viklet ind i afhængigheder der ikke hører til dens egentlige ansvar. Eksemplet er præcist: en `Calendar`-klasse er i sit væsen generisk kalenderlogik, men hvis den direkte kalder en konkret DS1339A real-time clock, kan den kun bruges på hardware med netop den chip. Omkostningen ved at hive den fri overstiger omkostningen ved at skrive den forfra — så den bliver skrevet forfra. Resultatet er duplikeret logik, hvilket i sig selv er en ny smell.

### Viscosity

Sliden indleder med observationen:

> Given a problem – there are usually more then one way to solve it, some preserve the design, others do not i.e hack.

> - **Symptom**: It is easier to solve a problem by making a hack than preserve the design
> - **Problem**: The software degenerates – because to many hacks, workarounds, shortcuts etc. are introduced

Illustrerende citat:

> "It's sooo much quicker to put an `#ifdef` there than to redesign the component…yeah."

> Slide 6

Viscosity handler om **modstanden i den rigtige vej**. For et givet problem findes der typisk flere løsninger: nogle bevarer designet, andre er hacks. Viscosity er tilstanden hvor hacket er den nemmeste vej. Det afgørende er, at det ikke skyldes dårlig vilje — det er en rationel reaktion på de omkostninger udvikleren står overfor i øjeblikket. Er den designbevarende løsning tre gange dyrere end hacket, bliver hacket valgt, og hvert valg gør den designbevarende vej endnu dyrere næste gang.

`#ifdef`-eksemplet illustrerer det: en preprocessor-betingelse løser problemet på ét minut, hvor en redesign af komponenten koster dage.

## 6. Needless complexity (YAGNI) og Needless repetitions (DRY)

### Needless complexity (YAGNI)

> When a software system is "overengineered" to account for possible future changes (there is a balance)

Illustrerende citat:

> "The `Report` class uses an XML file and is prepared for the use of a printer, a speech synthesizer, smoke signals, Morse code and telepathy. I'm sure my boss will be pleased!"

> Slide 7

**YAGNI** står for "You Aren't Gonna Need It". Denne smell er modtrækket til de foregående: hvis man overkorrigerer for rigidity og fragility ved at bygge udvidelsespunkter ind til enhver tænkelig fremtidig ændring, ender man med et system hvis kompleksitet er drevet af hypoteser i stedet for krav. Al den ubrugte abstraktion skal stadig forstås, vedligeholdes og navigeres af den næste udvikler.

Sliden er eksplicit om at "there is a balance". Design skal være åbent for de ændringer man har grund til at forvente, ikke for alle ændringer man kan forestille sig. Den balance er kursets egentlige emne.

### Needless repetitions (DRY)

> Changes implemented by copy'n'paste instead of proper abstration

Illustrerende citat:

> "The code is right there, so why not just CnP it? I only need to change a little bit, and if it works for A it'll work for B…and C, and D"

> Slide 7

**DRY** står for "Don't Repeat Yourself". Symptomet er at ændringer implementeres ved copy-paste frem for ved at trække en ordentlig abstraktion ud. Prisen betales ved næste rettelse: en fejl i den kopierede logik findes nu N steder, og der er intet i koden der fortæller hvor mange kopier der findes eller hvor de er. Det er også netop det, immobility fører til — kan en komponent ikke genbruges, bliver den kopieret.

## 7. Opacity

> Code evolves to become harder and harder to understand in lieu of proper refactoring.

Sliden viser samme metode, `IsPrinterOn()`, i tre generationer forbundet af pile — en evolution fra læselig til uigennemtrængelig.

**Første generation** — triviel og læsbar:

```csharp
public bool IsPrinterOn() {
  return printer.IsOn();
}
```

**Anden generation** — der er tilføjet håndtering af sleep mode og en busy-wait:

```csharp
public bool IsPrinterOn() {
  if(!printer.IsOn()) {
    printer.WakeFromSleepMode();
    while(i<125000) i++;
    return printer.IsOn();
  }
  return true;
}
```

**Tredje generation** — tilstand cachet i `curPS`, et `goto` til en label, og en kommentar der ikke længere kan garantere svaret:

```csharp
public bool IsPrinterOn() {
  if(!(curPS = printer.IsOn()))
  {
    printer.WakeFromSleepMode();
    while(i<125000) i++;
    curPS = printer.IsOn();
    if(!curPS)
      goto CHECK_ON_AGAIN;
  }
  return true;

CHECK_ON_AGAIN:
  while(i<32500) i++;
  if(!printer.IsON())
    return false; //probably
}
```

> Slide 8

Opacity er den smell der handler om **læsbarhed over tid**. Ingen af de tre versioner blev skrevet som dårlig kode fra bunden; hver enkelt tilføjelse var et rimeligt svar på et konkret problem. Det er akkumulationen uden refactoring der gør koden uigennemtrængelig — nøgleordene på sliden er "in lieu of proper refactoring".

Værd at hæfte sig ved i tredje generation: tildelingen `curPS = printer.IsOn()` inde i en negeret betingelse blander sideeffekt og test; `goto CHECK_ON_AGAIN` bryder det strukturerede kontrolflow; magiske tal `125000` og `32500` som busy-wait forklarer intet om hvad de venter på; `printer.IsON()` er stavet anderledes end `printer.IsOn()` i de øvrige kald; og kommentaren `//probably` indrømmer direkte at forfatteren ikke længere ved om returværdien er korrekt. Sidstnævnte er den præcise definition af opacity: koden er blevet så uklar at dens egen forfatter ikke kan læse dens opførsel ud af den.

## 8. Bottom line

Afslutningssliden viser en tweet-udveksling fra 21. januar 2018. Programming Wisdom (@CodeWisdom) citerer:

> "If you can get today's work done today, but you do it in such a way that you can't possibly get tomorrow's work done tomorrow, then you lose."
> — tilskrevet Martin Fowler, *Refactoring: Improving the Design of Existing Code*

Martin Fowler (@martinfowler) svarer selv på tweetet:

> "quote is actually by @KentBeck. It appears in Refactoring, but in a sidebar that he wrote."

> Slide 9

Citatet er forelæsningens konklusion. Alle otte smells har det til fælles at de er lokale optimeringer: hver enkelt hack, hver copy-paste, hver `#ifdef` får dagens arbejde færdigt i dag. Regnskabet gøres først op i morgen, når arbejdet ikke længere kan gøres færdigt. Modtrækket er refactoring — det er derfor det er præcis den bog, citatet stammer fra.

Slide 10 er en ren AU-logoslide uden indhold.

> Slide 10

## 9. References

> Technical Debt: [www.larochelab.com](http://www.larochelab.com)

> Slide 11

---

## Opsummering

- Software rot: designet understøtter ikke uventede ændringer i requirements, derfor hackes ændringerne ind, derfor kan produktet ikke supporteres kontinuerligt.
- **Rigidity** — en simpel ændring udløser en kaskade af følgeændringer, så ingen tør røre det der ikke er kritisk.
- **Fragility** — en rettelse ét sted knækker noget helt andet, så udvikleren mister evnen til at ræsonnere om egne ændringer.
- **Immobility** — genbrugelig funktionalitet er viklet ind i for meget bagage, så komponenter skrives om i stedet for at blive genbrugt.
- **Viscosity** — hacket er den nemmeste vej, så designet degenererer af selv velmenende udviklere.
- **Needless complexity (YAGNI)** — overengineering til hypotetiske fremtidige krav; der findes en balance mellem for lidt og for meget åbenhed.
- **Needless repetitions (DRY)** — copy-paste i stedet for ordentlig abstraktion; fejlen skal nu rettes N steder.
- **Opacity** — koden bliver gradvist uforståelig når den udvikles uden løbende refactoring.
- Bottom line: at få dagens arbejde gjort på en måde der gør morgendagens arbejde umuligt, er et tab — uanset at det ser produktivt ud i dag.
