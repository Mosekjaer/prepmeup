# SysML Structural Diagrams 3 — BDD + IBD opsamling (connectors, item flows, flow ports)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L8 (E25 ToC: L9) — SysML IBD (E24-slides: Structural Diagrams 3) |
| **Kursus** | SWISE-01 Indledende System Engineering (I2ISE) |
| **Kilde** | `SysML Structural Diagrams 3.pdf` (12 slides) + Brightspace-siden "L8 SysML IBD" (forklarende tekst) |
| **Type** | slides + kursusside |
| **Emner dækket** | IBD's relation til BDD, connectors, item flows, atomic flow ports, nonatomic flow ports og flowSpecification, konjugerede porte (~ på BDD, negativt symbol på IBD), porte til omverdenen (external flow ports), fuld løsning på Access Control System (BDD + ibd System Context + ibd Access Control System), øvelser (IBD for Parkeringsautomat User Interface og Smart Fridge) |

---

## Introduktion (fra Brightspace-siden)

I dag ser vi på de strukturelle diagrammer i SysML, især Block Definition Diagrams og Internal Block Diagrams — hvordan de bruges, og hvordan de supplerer hinanden, når vi modellerer komplekse systemer som brugergrænseflader, sensorer og elektroniske komponenter.

Block Definition Diagrams (BDD) kan sammenlignes med ingredienslisten i en opskrift. De viser hvilke blokke systemet består af, deres egenskaber og relationer. Internal Block Diagrams (IBD) svarer til selve opskriftens fremgangsmåde. De viser, hvordan ingredienserne blandes og i hvilken rækkefølge. Tænk på et lysstyringsprojekt i et klasseværelse: BDD'et viser, at I har lamper, kontakter og bevægelsessensorer. IBD'et viser, hvordan de er koblet sammen og kommunikerer med hinanden.

> Brightspace L8

## SysML Diagram types

*Figur: Taksonomien SysML Diagram → Behavior Diagram (Activity, Sequence, State Machine, Use Case), Requirement Diagram, Structure Diagram (Block Definition, Internal Block, Parametric, Package). Samme som i introduktionen.*

> Slide 2

## SysML: Internal Block Diagram

- An Internal Block Diagram (ibd) is used to define
  - the interconnection and interfaces of the parts of a block, and
  - the information flow between parts
- An ibd always relates to a block on a bdd. It shows the internal connections of the block's constituents

Eksemplet er Aircraft: `bdd Aircraft [Structural hierarchy]` (Aircraft komponeret af Wing Structure `wings` [2], Engine `eng`, Cockpit) og `ibd Aircraft` med parts `eng : Engine`, `: Cockpit`, `wings[0]: Wing Structure`, `wings[1]: Wing Structure`, hvor Cockpit er forbundet til de tre øvrige. (Identisk med Structural Diagrams 2, slide 3.)

Brightspace-tekst: Et Internal Block Diagram viser de interne forbindelser og grænseflader mellem delene i en blok. Det hænger altid sammen med en blok i et BDD. BDD viser typerne. IBD viser de interne forbindelser. I SysML laver man aldrig et IBD i tom luft. Det er altid knyttet til en blok, man allerede har defineret i BDD'et. Tænk på, at du først designer printpladen og placerer komponenterne (BDD), og derefter tegner du forbindelserne og ledningsføringen (IBD).

> Slide 3

## Modeling connections

- We would like to express more about the connection between parts on the ibd
  - This would help us to define the interface of the parts
- Connections
- Item flows
- Flow Ports
  - Atomic Flow Ports
  - Nonatomic Flow Ports

Brightspace-tekst: Når vi modellerer forbindelser, kan vi gøre dem mere detaljerede ved hjælp af connectors, item flows og flow ports. Det hjælper os med at definere grænsefladerne mellem delene præcist. **Connectors** er selve ledningerne eller signalvejene. **Item flows** viser hvad der løber i ledningen. **Flow ports** viser de steder, hvor signaler eller data går ind og ud af en del. Det er især vigtigt i elektroniske systemer, hvor en port kan være et stik eller en pin på et kredsløb.

> Slide 4

## Simple connections

Gennemgående eksempel: **User Interface**.

`bdd User Interface [Hierarchical Structure]`:

| Relation | Whole | Part | Multiplicitet | Part name |
|---|---|---|---|---|
| Komposition | «block» User Interface | «block» Keypad | (1) | — |
| Komposition | «block» User Interface | «block» Micro Controller | (1) | — |
| Komposition | «block» User Interface | «block» Led | 2 | leds |

`ibd User Interface [Connections]`:

| Part | Type |
|---|---|
| `: Keypad` | Keypad |
| `: Micro Controller` | Micro Controller |
| `leds[1] : Led` | Led |
| `leds[2] : Led` | Led |

Connectors (simple linjer, ingen retning): `: Keypad` — `: Micro Controller`; `: Micro Controller` — `leds[1] : Led`; `: Micro Controller` — `leds[2] : Led`. Røde markeringer på sliden: `ibd` i headeren, og multipliciteten `2` på `leds` i BDD'et, som bliver til de to parts `leds[1]` og `leds[2]` på IBD'et.

Brightspace-tekst: I BDD'et definerer vi blokke som Keypad, User Interface og LED. I IBD'et for User Interface ser vi, hvordan microcontroller, keypad og LED'er faktisk er forbundet. I lysstyringsprojektet viser BDD'et blokken Klasseværelse med Sensor, Controller og Lampe; IBD'et viser sensorens output til controlleren og controllerens output til lampen. Det gør arkitekturen konkret.

> Slide 5

## Flow Items

Samme BDD. `ibd User Interface [Item flows]` — connectors med item flows (udfyldt trekant på linjen angiver retning, markeret "One way"):

| Fra part | Item flow (item name : type) | Til part |
|---|---|---|
| `: Keypad` | `touchEvent: RS232` | `: Micro Controller` |
| `: Micro Controller` | `on : GPIO` | `leds[1] : Led` |
| `: Micro Controller` | `on : GPIO` | `leds[2] : Led` |

Brightspace-tekst: Item flows viser, hvad der specifikt flyder mellem komponenterne, for eksempel data eller signaler. Connectors viser kun, at de er forbundet. Item flows er som at skrive på ledningen "her løber temperaturmålinger" eller "her løber betalingsdata". Det giver modellen transparens og hjælper med at undgå misforståelser.

> Slide 6

## Atomic Flow Ports

`bdd User Interface [Hierarchical Structure]` — nu med ports-compartments:

| Blok | Ports |
|---|---|
| «block» User Interface | — |
| «block» Keypad | `out touch : RS232` |
| «block» Micro Controller | `in touch : RS232`, `out leds[2] : GPIO` |
| «block» Led | `in on : GPIO` |

`ibd User Interface [Atomic Flow Ports]` — porte som små kvadrater med pilesymbol på part-rammen:

| Fra part / port | Til part / port |
|---|---|
| `: Keypad` port `touch : RS232` (out) | `: Micro Controller` port `touch : RS232` (in) |
| `: Micro Controller` port `leds[1] : GPIO` (out) | `leds[1] : Led` port `on : GPIO` (in) |
| `: Micro Controller` port `leds[2] : GPIO` (out) | `leds[2] : Led` port `on : GPIO` (in) |

Note på sliden: **In/out/inout not used on the IBD! The symbol says it all!** — og "Do this way" (den anbefalede notation i kurset).

Brightspace-tekst: Atomic Flow Ports viser ind- og udgange for specifikke signaler eller data. I BDD'et definerer vi portene. I IBD'et placeres de på de enkelte dele og forbindes til de relevante elementer. Portene kan være RS232-stik, GPIO-pins, CAN-bus eller en software-API. Symbolerne viser retningen, så du ikke skal skrive in og out.

> Slide 7

## Nonatomic Flow Ports

`bdd Secure Door [Hierarchical Structure]`:

| Relation | Whole | Part |
|---|---|---|
| Komposition | «block» Secure Door | «block» Access Control Interface |
| Komposition | «block» Secure Door | «block» Door |

| Blok | Ports |
|---|---|
| «block» Access Control Interface | `inout doorCtrl : ~Door Control` |
| «block» Door | `inout ctrl : Door Control` |

| «flowSpecification» Door Control — flowProperties |
|---|
| `in unlock : GPIO` |
| `in open : GPIO` |
| `out status : RS232` |

`ibd Secure Door [Nonatomic Flow Ports]`: part `: Access Control Interface` med port `doorCtrl : Door Control` (negativt dobbeltpil-symbol = konjugeret) forbundet til part `: Door` med port `ctrl : Door Control` (dobbeltpil). Eksempel-annoteringer: Access Control Interface sender "Open"; Door svarer "Received, Opening".

Noter på sliden:
- **The ~ (tilde) is not used on the IBD! The symbol says it all!**
- **Opposite direction. Conjugate Interface:** usually bi-directional; a *conjugate pair*: one side sends control commands, and the other side interprets.
- Example) `doorCtrl` on the Access Control Interface may be responsible for outputting control command (locking, unlocking, or checking the door's status, etc.)
- Example) `doorCtrl` on the Door is responsible for receiving these commands e.g. as interrupt
- *They don't do the same thing, but they are part of the same operation.*

Brightspace-tekst: Nonatomic Flow Ports bruges til mere komplekse interfaces, hvor flere forskellige signaler går gennem samme port. Vi definerer det i en flowSpecification og viser i IBD'et, hvordan komponenter er forbundet. Forestil dig et multifunktionsstik på et embedded board, hvor der både går strøm, data og styringssignaler igennem. Det er et ikke-atomisk flowport. Conjugerede porte viser to sider af samme interface med modsat retning.

> Slide 8

## Ports to the outside

`bdd User Interface [Hierarchical Structure]` — nu har også User Interface selv porte:

| Blok | Ports |
|---|---|
| «block» User Interface | `in keyPress[10] : Force`, `out status[2] : Light` |
| «block» Keypad | `in keyPress[10] : Force`, `out touch : RS232` |
| «block» Micro Controller | `in touch : RS232`, `out leds[2] : GPIO` |
| «block» Led (leds, 2) | `in on : GPIO`, `out state : Light` |

Note: **These ports must be implemented by one of the parts on the BDD!** — User Interfaces `keyPress[10]` implementeres af Keypad, `status[2]` implementeres af Led (pile fra User Interface-portene til Keypad- og Led-portene).

`ibd User Interface [Showing External Flow Ports]`:

| Fra | Til |
|---|---|
| ydre port `keyPress[1..10] : Force` (på IBD-rammen, in) | `: Keypad` port `keyPress[1..10] : Force` (in) |
| `: Keypad` port `touch : RS232` (out) | `: Micro Controller` port `touch : RS232` (in) |
| `: Micro Controller` port `leds[1] : GPIO` (out) | `leds[1] : Led` port `on : GPIO` (in) |
| `: Micro Controller` port `leds[2] : GPIO` (out) | `leds[2] : Led` port `on : GPIO` (in) |
| `leds[1] : Led` port `state : Light` (out) | ydre port `status[1] : Light` (på IBD-rammen, out) |
| `leds[2] : Led` port `state : Light` (out) | ydre port `status[2] : Light` (på IBD-rammen, out) |

Note: **They are shown on the IBD as connected to the outside** — de ydre porte sidder på selve diagram-rammen (blokkens grænse).

Brightspace-tekst: Ports skal implementeres af en af delene i BDD'et. I IBD'et vises de forbundet ud af systemet, så man ser, hvor systemet har sine ydre grænseflader. Det svarer til at tegne alle stikkene på et printkort, så du ser hvad der går ud til brugeren, uden at tegne hele brugerens system.

> Slide 9

## Exercise from last – solutions on next slide

`bdd Access Control System Context` (udleveret BDD, identisk med Structural Diagrams 2 slide 25):

System Context er komponeret af «system of interest» Access Control System, Access Card, Door og aktøren User. Access Control System er komponeret af Card Reader, Keypad, Control, LED (×2: part names `red` og `green`) og Buzzer.

| Blok | Ports |
|---|---|
| «system of interest» Access Control System | `in card: Card`, `in keyPressed[12]: Force`, `inout doorCtrl: ~DoorCtrl` |
| Access Card | `out cardVal: Card` |
| Door | `inout ctrl: DoorCtrl` |
| Card Reader | `in card: Card`, `out id: string` |
| Keypad | `in keyPressed[12]: Force`, `out key: string` |
| Control | `in id: string`, `in keyVal: string`, `out red: GPIO`, `out green: GPIO`, `out buzzer: GPIO`, `inout doorCtrl: ~DoorCtrl` |
| LED | `in on: GPIO` |
| Buzzer | `in ctrl: GPIO` |
| «flowSpecification» DoorCtrl (values) | `in unlock: bool`, `in openDoor: bool`, `out status: string` |

"Observe the new element: a Control block."

Brightspace-tekst: Her ser vi et samlet eksempel med Access Control System. Vi har Card Reader, Keypad, Control, LED'er og Buzzer. Vi viser porte og flow mellem elementerne. Det er en kontekstmodel for systemet. Samme metode kan bruges til jeres egne projekter. Hvis jeres system er et klasselokale med lysstyring, kan aktørerne være elever eller bygningens kontrolsystem.

> Slide 10

## Løsning: ibd System Context og ibd Access Control System

### `ibd System Context`

| Part | Ports |
|---|---|
| `: Access Card` | `cardVal : Card` (out) |
| `User` (aktør, stregfigur) | — |
| `: Access Control System` | `card : Card` (in), `press : Force` (in), `doorCtrl: DoorCtrl` (konjugeret, negativt symbol) |
| `: Door` | `ctrl: DoorCtrl` (dobbeltpil) |

Connectors og item flows:

| Fra | Item flow | Til |
|---|---|---|
| `: Access Card` port `cardVal : Card` | `: Card` (pil mod Access Control System) | `: Access Control System` port `card : Card` |
| `User` | `: Force` (pil mod Access Control System) | `: Access Control System` port `press : Force` |
| `: Access Control System` port `doorCtrl: DoorCtrl` | — | `: Door` port `ctrl: DoorCtrl` |

### `ibd Access Control System`

Note på sliden: **Observe how the conjugated flow port is repeated, when going to the outside!** — den konjugerede port `doorCtrl: DoorCtrl` på Control gentages som ydre port `doorCtrl: DoorCtrl` på IBD-rammen, forbundet med en connector.

| Part | Ports |
|---|---|
| `: Card Reader` | `card : Card` (in), `id : string` (out) |
| `: Keypad` | `keyPressed[1..12] : Force` (in), `key : string` (out) |
| `: Control` | `id : string` (in), `keyVal : string` (in), `red : GPIO` (out), `green : GPIO` (out), `buzzer : GPIO` (out), `doorCtrl : DoorCtrl` (konjugeret) |
| `green : LED` | `on : GPIO` (in) |
| `red : LED` | `on : GPIO` (in) |
| `: Buzzer` | `ctrl : GPIO` (in) |

Connectors:

| Fra | Til | Bemærkning |
|---|---|---|
| ydre port `card : Card` (IBD-ramme, in) | `: Card Reader` port `card : Card` | ydre port |
| `: Card Reader` port `id : string` | `: Control` port `id : string` | item flow `id : string` med retningspil |
| ydre port `keyPressed[1..12] : Force` (IBD-ramme, in) | `: Keypad` port `keyPressed[1..12] : Force` | ydre port |
| `: Keypad` port `key : string` | `: Control` port `keyVal : string` | port-navnene behøver ikke være ens i hver ende, kun typen |
| `: Control` port `red : GPIO` | `green : LED` port `on : GPIO` | *(som tegnet på sliden: red-porten går til green-LED'en og green-porten til red-LED'en — sandsynligvis en tegnefejl i løsningen)* |
| `: Control` port `green : GPIO` | `red : LED` port `on : GPIO` | se ovenfor |
| `: Control` port `buzzer : GPIO` | `: Buzzer` port `ctrl : GPIO` | — |
| `: Control` port `doorCtrl : DoorCtrl` (konjugeret) | ydre port `doorCtrl: DoorCtrl` (IBD-ramme, konjugeret) | konjugeret port gentages udadtil |

Brightspace-tekst: Her er IBD'et for systemkonteksten. Vi viser, hvordan komponenterne er forbundet, for eksempel hvordan Card Reader sender kortdata, Keypad sender tastetryk, og Control styrer LED'er og buzzer. Bemærk forskellen: samme blokke som i BDD'et, men nu ser I forbindelser og flows.

> Slide 11

## Today's exercises

- The solutions to last lecture's exercises
  - BDD for Parkeringsautomat
  - BDD for Smart Fridge
- Can be found on Brightspace
- Use them as input to create IBD's for
  - The User Interface block for Parkeringsautomat
  - The complete Smart Fridge

> Slide 12

## Øvelse (fra Brightspace-siden)

Nu skal I tage det skridtet videre og lave et Internal Block Diagram for jeres semesterprojekt. Sidste gang arbejdede I med at lave et Block Definition Diagram for jeres projekt, så det er allerede klar til at blive brugt som udgangspunkt for IBD'et. Det er helt i orden, hvis nogle af jer i jeres BDD'er allerede har tegnet forbindelser. Nu får I et ekstra lag, hvor I kan finde ud af, hvordan det bliver et rent BDD og et rent IBD.

I kan altid iterere og forbedre modellerne hen ad vejen. Brug det BDD, I lavede sidste gang, som fundament. Skil eventuelle forbindelser ud og lav et separat IBD, hvor I viser, hvordan delene hænger sammen og kommunikerer.

> Brightspace L8

## Opsummering: notationsregler for porte (BDD vs. IBD)

| Element | På BDD (ports-compartment, tekst) | På IBD (symbol på part-rammen) |
|---|---|---|
| Atomic flow port, in | `in name : Type` | kvadrat med pil ind |
| Atomic flow port, out | `out name : Type` | kvadrat med pil ud |
| Atomic flow port, inout | `inout name : Type` | kvadrat med dobbeltpil |
| Nonatomic flow port | `inout name : FlowSpec` | kvadrat med dobbeltpil (`inout` skrives ikke) |
| Konjugeret nonatomic flow port | `inout name : ~FlowSpec` | kvadrat med negativt dobbeltpil-symbol (`~` skrives ikke) |
| Item flow | (ikke på BDD) | `name : Type` ved connector + udfyldt trekant for retning |
| Ydre port (blokkens egen) | ports-compartment på den blok IBD'et tilhører | kvadrat på IBD-rammen, forbundet til den part der implementerer den |
