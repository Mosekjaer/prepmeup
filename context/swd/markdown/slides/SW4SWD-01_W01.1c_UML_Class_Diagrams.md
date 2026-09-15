# Uge 1.1c — UML class diagrams, the basics

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Uge 1 — UML class diagrams, the basics |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Forelæser** | Henrik Bitsch Kirk (HK) |
| **Kilde** | `UML class diagrams - The basics.pdf` (4 slides) |
| **Sprog/kode** | C# |
| **Emner dækket** | Formål med class diagrams, klasse-notation og compartments, syntaks for attributes og operations, associations, navigability, dependency, multiplicitet, composition vs. aggregation, generalization, realization, interfaces, ball-and-socket-notation |

---

## Agenda

1. Hvad et UML class diagram er, og hvad det bruges til
2. Klassen som notationselement — de tre compartments
3. Syntaks for attributes og operations
4. Relationer: association, navigability, dependency, multiplicitet
5. Composition og aggregation (og hvorfor aggregation ikke skal bruges)
6. Generalization, realization og interfaces
7. Provided/required interfaces og ball-and-socket-notation

---

## 1. Hvad er et UML class diagram?

Noten tager udgangspunkt i Scott W. Amblers definition:

> "UML class diagrams show the classes of a system, their interrelationships, and the operations and attributes of the classes.
>
> [..Class diagrams..] are used to:
>
> - explore domain concepts in the form of a domain model.
> - analyze requirements in the form of a conceptual/analysis model.
> - depict the detailed design of object-oriented or object-based software."
>
> — Scott W. Ambler, *The elements of UML 2.0 style*

> Slide 1

Pointen er at class diagrams bruges på tre forskellige abstraktionsniveauer, og at man skal vide hvilket niveau man tegner på. En domain model udforsker begreberne i problemområdet uden hensyn til implementering. En conceptual/analysis model bruges til at analysere requirements. Og et detailed design-diagram beskriver den faktiske objektorienterede struktur i koden. Samme notation, tre vidt forskellige formål og detaljeringsgrader.

## 2. Klassen og dens compartments

Klassen er det centrale element i class diagrams. En klasse kan have op til tre compartments: navn, attributes og operations. Kun navnet er obligatorisk.

Rækkefølgen når alle tre compartments bruges: navnet øverst, **attributes i midten**, **operations nederst**. Forfatteren indrømmer at det er svært at huske, og bruger derfor det trick altid at hænge `()` på operations, så de kan skelnes fra attributes uanset hvilket compartment de står i.

Sliden viser fire varianter af samme klasse-boks:

| Variant | Compartments | Indhold |
|---|---|---|
| 1 | Kun navn | `Class name` |
| 2 | Navn + ét | `operation1`, `operation 2` |
| 3 | Navn + ét | `attribute a`, `attribute b` |
| 4 | Alle tre | attributes i midten, operations nederst |

Variant 2 og 3 illustrerer netop tvetydigheden: med kun to compartments kan man ikke se på placeringen alene om indholdet er attributes eller operations.

> Slide 1

## 3. Syntaks for attributes og operations

Attributes skrives efter formen:

```
visibility name : type multiplicity = default {property-string}
```

Eksempel:

```
– name : String [1] = "Anonymous" {readOnly}
```

Her er `–` visibility (private), `name` er navnet, `String` er typen, `[1]` er multiplicitet, `"Anonymous"` er default-værdien, og `{readOnly}` er en property-string.

Operations skrives efter formen:

```
visibility name (parameter-list) : return-type {property-string}
```

hvor hvert element i parameter-list har formen:

```
direction name : type = default-value
```

Eksempel:

```
+ balanceOn (date : Date) : Money
```

`+` er public visibility, `balanceOn` er operationens navn, `date : Date` er parameteren, og `Money` er returtypen.

Den eneste obligatoriske del af beskrivelsen af en attribute eller operation er **navnet**. Alt andet — visibility, type, multiplicitet, default, property-string — kan udelades hvis det ikke tilfører værdi på det abstraktionsniveau man tegner på.

> Slide 1

## 4. Eksempeldiagram: association, dependency og multiplicitet

Eksempeldiagrammet viser en web service-arkitektur. Strukturen er:

- **Web service** med operationerne `get()`, `post()`, `put()`, `delete()`.
- **Resource** med attributten `id` og operationerne `toJSON()`, `fomJSON()` (stavet sådan på sliden).
- **Representation** med attributten `MIME type`.
- **JSON Representation** som specialisering af `Representation`.
- **Application Interface** samt tre application-moduler under den.

Relationerne på diagrammet:

| Fra | Til | Relation | Annotering |
|---|---|---|---|
| Web service | Resource | Association | `1` i Web service-enden, `*` i Resource-enden |
| Web service | Application Interface | Association med navigability (pil) | — |
| Resource | Representation | Association | `1` i Resource-enden, `1..*` i Representation-enden |
| JSON Representation | Representation | Generalization | — |
| Application Interface | Resource | Dependency (stiplet pil) | `produces/consumes` |
| Application Interface | Application module A/B/B | Association | — |

En note på diagrammet peger på `Resource` og siger: "These are data transfer objects (DTOs). They are not persisted."

> Slide 2

**Association** betyder at klasserne "kender" hinanden eller på anden vis er relaterede. I software betyder det typisk at den ene har en reference til den anden. Vil man være specifik omkring at man kan navigere fra web service til application interface, tilføjer man et pilehoved på associationen, som peger fra web service mod application interface. Det kaldes navigability.

**Dependency** tegnes som en stiplet linje med pilehoved. Application interface har en dependency til resource-klassen. I software betyder det at hvis resource-klassen ændres, bliver application interface sandsynligvis påvirket — for eksempel fordi application interface har en operation der tager en resource som parameter. Dependency er altså den svageste form for kobling: ingen reference gemmes, men der er stadig en ændringsafhængighed.

**Multiplicitet** annoteres i associationens ender. Associationen mellem web service og resource læses: "One (1) web service is related to many (*) resources".

De multiplicitets-notationer sliden viser:

| Notation | Betydning |
|---|---|
| `1` | exactly one |
| `*` | zero or more |
| `0..1` | zero or one |
| `m..n` | from m to n |
| `{ordered} *` | ordered |

> Slide 2

## 5. Composition og aggregation

UML har symboler for både aggregation og composition.

**Composition** betyder at den ene klasses livscyklus kontrolleres af den anden. Eksemplet er en polygon, som har 3 eller flere punkter. Punkterne tilhører polygonen, og en instans af polygon-klassen har instanser af point-klassen. Slettes polygon-instansen, slettes point-instanserne også.

Diagrammet viser `Polygon` og `Point` forbundet med en composition-relation. Den udfyldte diamant sidder i `Polygon`-enden. Multipliciteten er `1` i `Polygon`-enden og `3..*` i `Point`-enden, annoteret med `{ordered}`.

> Slide 3

Notationsoversigten på sliden:

| Linjeform | Relation |
|---|---|
| Åben (uudfyldt) diamant | Aggregation — **overstreget med rødt kryds på sliden** |
| Udfyldt (sort) diamant | Composition |
| Almindelig linje | Association |

En note ved siden af aggregation-symbolet siger direkte: "Don't use the aggregation symbol."

**Aggregation skal ikke bruges.** UML superstructure-specifikationen siger at "[aggreation is] dependent on domain and modeller.". Martin Fowler kalder den "strictly meaningless". Konklusionen på sliden er kontant: "Don't use it. Please."

Den praktiske konsekvens er at man i praksis kun har to niveauer at vælge imellem: almindelig association (klasserne kender hinanden, uafhængige livscyklusser) og composition (ejerskab med kontrolleret livscyklus). Den mellemting aggregation forsøger at udtrykke, er ikke defineret præcist nok til at to modellører vil læse den ens.

## 6. Generalization, realization og dependency

Andet diagram på slide 3 viser Java Collections-hierarkiet og annoterer hver relationstype med navn:

Elementerne:

- `<<interface>> Collection`
- `<<interface>> Set`
- `AbstractCollection` (abstrakt klasse, skrevet i kursiv)
- `AbstractSet` (abstrakt klasse, kursiv)
- `HashSet`
- `Areas` med attributten `zipCodes : set` og operationen `isValidZipCode (code)`

Relationerne som de er annoteret på sliden:

| Fra | Til | Relation | Notation |
|---|---|---|---|
| `Set` | `Collection` | Generalization (inherits) | Fuldt optrukken linje, hul trekant-pil |
| `AbstractCollection` | `Collection` | Realization (implements) | Stiplet linje, hul trekant-pil |
| `AbstractSet` | `Set` | Realization (implements) | Stiplet linje, hul trekant-pil |
| `AbstractSet` | `AbstractCollection` | Generalization (inherits) | Fuldt optrukken linje, hul trekant-pil |
| `HashSet` | `AbstractSet` | Generalization (inherits) | Fuldt optrukken linje, hul trekant-pil |
| `Areas` | `Set` | Dependency (depends on) | Stiplet linje, åbent pilehoved |

> Slide 3

Diagrammet fungerer som notationslegende: interfaces markeres med stereotypen `<<interface>>`, abstrakte klasser med kursiv klassenavn. **Generalization** (arv) er fuldt optrukken linje med hul trekant. **Realization** (implementering af et interface) er stiplet linje med hul trekant. **Dependency** er stiplet linje med almindeligt åbent pilehoved. De tre stiplede/fuldtoptrukne kombinationer er det man skal kunne kende fra hinanden.

## 7. Provided og required interfaces — ball-and-socket

Interfaces kan arve fra andre interfaces, ligesom klasser kan arve fra andre klasser. Når en klasse implementerer et interface, siges den at **provide** interfacet. Når en klasse afhænger af et interface, siges den at **require** interfacet.

Det kan vises i "ball-and-socket"-notation. Første eksempel:

- `DataInputStream` arver fra `InputStream` (generalization, hul trekant-pil op mod `InputStream`).
- `DataInputStream` provider interfacet `DataInput` — vist som en kugle (ball) på en linje ud fra klassen.
- `OrderReader` requirer samme interface — vist som en halvcirkel (socket) der omslutter kuglen.
- Mellem socket og ball er tegnet en dependency (stiplet pil).

> Slide 4

Notationen med en dependency fra socket til ball er korrekt UML, men i praksis vises ball og socket ofte sammen — direkte sammenkoblet — fordi det er lettere at tegne og ser bedre ud.

Andet eksempel viser hvorfor dependency-pilen alligevel nogle gange giver mening:

- Klasse `A` provider interfacet `Set` (ball i A-enden).
- Klasse `B` requirer interfacet `Collection` (socket i B-enden).
- Mellem dem en dependency-pil.
- Ved siden af: `<<interface>> Set` har en generalization op til `<<interface>> Collection`.

> Slide 4

Konstruktionen er gyldig: klasse `B`, som kræver et `Collection`-interface, kan få sit krav opfyldt af klasse `A`, som leverer et `Set`-interface — netop fordi `Set` arver fra `Collection`. Når provided og required interface ikke er *samme* interface, men forbundet via arv, kan man ikke bare klikke ball og socket sammen; her skal dependency-pilen frem for at vise at forbindelsen faktisk holder.

---

## Opsummering

- Class diagrams bruges på tre niveauer — domain model, conceptual/analysis model og detailed design — og notationen er den samme, mens detaljeringsgraden ikke er.
- En klasse har op til tre compartments; kun navnet er obligatorisk, attributes står i midten og operations nederst. Skriv `()` efter operations for at undgå tvivl.
- Attribute-syntaksen er `visibility name : type multiplicity = default {property-string}`, operation-syntaksen `visibility name (parameter-list) : return-type {property-string}`. Kun navnet er påkrævet.
- Association betyder "kender hinanden", pilehoved angiver navigability, stiplet pil er dependency (ændringsafhængighed uden reference), og multiplicitet annoteres i associationsenderne.
- Composition (udfyldt diamant) betyder kontrolleret livscyklus — ejeren dør, delene dør med. Aggregation (åben diamant) er "strictly meaningless" ifølge Fowler og skal ikke bruges.
- Generalization er fuldt optrukken linje med hul trekant, realization er stiplet linje med hul trekant, dependency er stiplet linje med åbent pilehoved.
- Provided interface tegnes som ball, required interface som socket. De kan kobles direkte, men dependency-pilen er nødvendig når det providede interface blot arver fra det krævede.
