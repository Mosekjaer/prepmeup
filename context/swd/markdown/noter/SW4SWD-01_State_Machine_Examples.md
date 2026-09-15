# State Machine — øvelseseksempler

## Metadata

| Felt | Værdi |
|---|---|
| **Type** | Øvelsesbeskrivelser (Brightspace), ikke slides |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Lektion** | Uge 8 — State machines og GoF State Pattern |
| **Forelæser** | Henrik Bitsch Kirk (HK) |
| **Kilde** | `State 1 - Intro` og `State 2 - Phone` (Brightspace) |
| **Sprog/kode** | C# (Visual Studio-solution med flere projekter) |
| **Relaterede slides** | [`../slides/SW4SWD-01_W08a_GoF_State.md`](../slides/SW4SWD-01_W08a_GoF_State.md), [`../slides/SW4SWD-01_W08b_State_Nested_Orthogonal.md`](../slides/SW4SWD-01_W08b_State_Nested_Orthogonal.md) |

---

## Indledning

De to øvelser træner sammenhængen mellem **UML/SysML state machine-diagrammer** og de **to implementeringsstrategier**, switch/case og GoF State Pattern. Pointen er ikke at lære et diagramnotationsformat udenad, men at kunne oversætte begge veje: fra diagram til kode, og fra kode tilbage til diagram.

Øvelse 1 (cykellygten) er bygget som en **inkrementel sammenligning**. Man implementerer den *samme* state machine to gange — først med switch/case, så med GoF State — og udvider derefter begge med et ekstra event og et lag af nested states. Formålet er at mærke forskellen på egen krop: switch/case er kortest ved den simple version, men bryder sammen når nesting kommer til, hvor GoF State-versionen tværtimod bliver mere elegant, fordi nesting mapper til arv. Øvelsen slutter med et ISP-refactoring, der adresserer "the evil client"-problemet fra W08b.

Øvelse 2 (telefonen) er den større af de to. Her leveres et **ufuldstændigt** state chart fra en salgsrepræsentant, og opgaven er selv at udfylde hullerne (optaget linje, ugyldigt nummer, modparten lægger på), identificere hvor de enkelte events kommer fra, og unit-teste hver tilstand for sig. Det sidste er hele argumentet for GoF State på testability: fordi hver tilstand er sin egen klasse, kan man verificere isoleret, at den tager de rigtige transitions og udfører de rigtige actions. Øvelsens sidste del kræver **orthogonale substates** (speakerphone-toggle og mute samtidigt med en igangværende samtale) og trækker dermed direkte på W08b.

Begge øvelser understreger det samme princip fra forelæsningen: **konteksten sætter ikke selv den nye tilstand — den nuværende tilstand gør det.**

---

## Øvelse 1: 3-LED cykellygte

### Introduction to state patterns

In this exercise you will design and implement a simple state machine for a 3-LED bicycle flashlight which will gradually evolve to become more complex. Through this, you will learn how the UML/SysML state machine diagrams and the implementations are related.

![Screenshot%202021-10-11%20at%2015.41.10.png](../assets/Screenshot%202021-10-11%20at%2015.41.10.png)

### Exercise 1: Basic flashlight with switch/case

Implement the control mechanism of a basic version of 3-LED flashlight along the lines of the below state machine diagram. The implementation shall be done using the  switch/case  approach. Ensure that the names of the states, events and actions are reflected in your implementation. Run your state machine.

![Screenshot%202021-10-11%20at%2015.41.20.png](../assets/Screenshot%202021-10-11%20at%2015.41.20.png)

### Exercise 2: Basic flashlight with GoF State

Create a new VS project under the same solution as above. In this project, implement the same sate machine as above, only this time using the GoF State Pattern along the lines given below. Ensure that you understand how the states map to classes in the pattern. Run your state machine.

![Screenshot%202021-10-11%20at%2015.41.28.png](../assets/Screenshot%202021-10-11%20at%2015.41.28.png)

### Exercise 3: Understand the call sequence in GoF State.

Assume your Flashlight is in the OFF state and receives a Power event. Draw a UML sequence diagram showing all calls back and forth between the Flashlight and FlashlightState objects.

> Jf. sekvensdiagrammet på slide 22 i [`W08a`](../slides/SW4SWD-01_W08a_GoF_State.md): `PWRPressed()` → `HandlePWRPressed(this)` → `TurnLampOn()` → `SetState(onState)`. Bemærk at både action-kaldet og tilstandsskiftet går **tilbage** fra tilstandsobjektet til konteksten.

### Exercise 4: Compare implementation

Compare the two implementations, the switch/case and the GoF State Pattern implementations. Which one do you prefer? Why? What do you think of the solutions in terms of extensibility, maintainability, and testability?

### Exercise 5: Extend your implementations

Business is good, and a version 2 of the flashlight is designed. In the version, a Mode button is added. When the flashlight is on, pressing the Mode button will cause the flashlight to toggle between solid light and flashing light – yes, a very innovative and never-seen-before approach to bicycle flashlights! The design of the control mechanism is given below.

![Screenshot%202021-10-11%20at%2015.41.52.png](../assets/Screenshot%202021-10-11%20at%2015.41.52.png)

Extend both your switch/case and GoF State Pattern implementations to handle the new requirements. For the GoF State Pattern-implementation, be sure to draw your class diagram first and be sure you handle OnEnter correctly – if you do, the code becomes very elegant!

![Screenshot%202021-10-11%20at%2015.41.43.png](../assets/Screenshot%202021-10-11%20at%2015.41.43.png)

> Dette er nested states. Se [`W08b`](../slides/SW4SWD-01_W08b_State_Nested_Orthogonal.md) slide 2 for mappingen fra composite state til arvehierarki, og afsnittet om entry/exit-actions for hvorfor `onEnter` gør koden elegant.

### Exercise 6: Compare (again)

Again compare the two implementations, the switch/case and the GoF State Pattern implementations. Which one do you prefer now? Why? How easy is it to map the State Machine Diagram, including event handling, to the individual implementations now?

### Exercise 7: Adhering to ISP

Right now, the Flashlight class has a lot of methods. Some are meant as event handlers, some are meant for the state objects so that they can cause the LEDs to turn on, off, and toggle. At the moment, the class violates ISP. Refactor the GoF State Pattern implementation so that this is fixed (hint: separate the interfaces needed for UI handling and states, respectively).

> Dette er præcis "the evil client" og ISP-løsningen fra [`W08b`](../slides/SW4SWD-01_W08b_State_Nested_Orthogonal.md) slide 6-7: to interfaces, `IFlashLight` til clients (kun event handlers) og `IFlashLightInternal` til tilstandsobjekterne (state settere og STM actions).

---

## Øvelse 2: Telefon

### The GoF State Pattern

In this exercise you will design the business logic of a simple telephone by means of the GoF State Pattern. You will incrementally add functionality to the system, thereby gaining insight into the implementation of state machines

The situation: You are to design and implement a simple telephone along the lines of the below state chart, supplied by a sales representative from a meeting with a customer:

![Screenshot%202021-10-11%20at%2015.49.12.png](../assets/Screenshot%202021-10-11%20at%2015.49.12.png)

![Screenshot%202021-10-11%20at%2015.49.17.png](../assets/Screenshot%202021-10-11%20at%2015.49.17.png)

### Exercise 1:

As usual, the sales representative left a number of questions unanswered, such as

- What happens if the line is busy? If the dialed number is invalid?

- How should we handle if the other party disconnects during the call?

- ...

  Amend the state chart above to answer these questions and any other questions you may have – it's your decision!

> Dette er den centrale designøvelse: et ufuldstændigt state chart er en kilde til netop de "unreachable states" xkcd-striben på slide 1 i [`W08a`](../slides/SW4SWD-01_W08a_GoF_State.md) handler om. At gøre alle event/state-kombinationer eksplicitte er selve pointen med at modellere.

### Exercise 2:

Identify the source of the events in the diagram. Some are from the phone's GUI, some are from the telephone system.

> Sondringen svarer til `// From GUI` versus de eksternt udløste events i flashlight-eksemplet. Den er også afgørende for stubbing i Exercise 3.

### Exercise 3:

Implement and test your solution. You may need to create stubs for the microphone and GUI, but thankfully you know how to do that :D.

Since you are using the GoF State Pattern, you can unit test each state to ensure that the transitions made and the actions taken are correct. Do that.

> Dette er testability-argumentet for GoF State i praksis: hver `ConcreteState` testes isoleret med en stubbet/mocket `Context`. Man verificerer både at den rette action kaldes tilbage i konteksten, og at det rette `SetState()` udføres.

### Exercise 4:

The sales representative has had another meeting with the customer. He has two further requests for the no-longer-so-simple-telephone: When a call is connected, it shall be possible to...

- Toggle between the use of regular speaker or loudspeaker (speakerphone)

- Mute the microphone during a call

 Again, amend these changes (hint: think orthogonal substates) to your design, then –and only then – implement your design.

> Orthogonale substates: speaker/loudspeaker og muted/unmuted er aktive **samtidigt** med at samtalen er forbundet, og uafhængigt af hinanden. Se [`W08b`](../slides/SW4SWD-01_W08b_State_Nested_Orthogonal.md) slide 3-5 — vælg strategi 2 (separate state machines med en reference hver fra konteksten) frem for at collapse til krydsproduktet.
