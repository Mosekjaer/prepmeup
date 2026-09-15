# ISE Quick Guide for SysML Diagrams and Symbols

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L6 — SysML BDD (selvlæsning; reference for hele L6–L11) |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `SysMLQuickGuide.pdf` (17 sider, ren billed-PDF — scannede tabeller fra *A Practical Guide to SysML*, Friedenthal/Moore/Steiner, Elsevier 2008, Appendix A) |
| **Type** | skabelon / notationsoversigt |
| **Emner dækket** | Notationstabeller for BDD (nodes: blocks, value types, enumerations, actors, flow specifications, interfaces, ports; paths: composite/reference association, association block, generalization), IBD (nodes: parts, references, value properties, ports; paths: connectors, item flows), Activity Diagram (A11–A14), Sequence Diagram (A15–A16), State Machine Diagram (A18–A20) |

Tabelnumre refererer til "SysML Reference Guide" i Friedenthal et al. Kolonnen *Section* er bogens kapitelafsnit. Tekst i `<...>` er pladsholdere i notationen; `[...]` er valgfrie dele.

---

## SysML Structure diagrams

Kilde: *A Practical Guide to SysML*. Sanford Friedenthal, Alan Moore and Rick Steiner. Elsevier 2008. SysML Reference Guide: Table A3, A4, A5, A8 and A9.

> Side 1–2

### Table A.3 — Block Definition Diagram Nodes for Representing Block Structure and Values

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Block Node | Rektangel med `«block»` `<Name>` øverst og compartments: *parts* `<Part>:<Block>[<Multiplicity>]`; *references* `<Reference>:<Block>[<Multiplicity>]`; *values* `<ValueProperty>:<ValueType>=<ValueExpression>`; *operations* `<Operation>(<Parameter>,…):<Type>`; *receptions* `«signal»<Signal>(<Parameter>,…)` | The block is the fundamental modular unit for describing system structure in SysML. Compartments are used to show structural features (parts, references, values) and behavioral features (operations, receptions) of the block. Additional properties on blocks are {encapsulated, abstract}. Abstract may also be indicated by italicizing the `<Name>`. Additional properties on structural features include {ordered, unordered, unique, nonunique, subsets `<Property>`, redefines `<Property>`}. A forward slash (/) before a property name indicates that it is derived. | 6.2, 6.3, 6.5.2 |
| Dimension and Unit Nodes | `«unit»` `<Name>` med `dimension = <Dimension>`; `«dimension»` `<Name>` | A dimension identifies a physical quantity such as length, whose value may be stated in terms of defined units, such as meters or feet. A unit must always be related to a dimension. | 6.3.3 |
| Value Type Node | `«valueType»` `<Name>` med compartments *values* `<ValueProperty>:<ValueType>=<ValueExpression>`; *operations* `<Operation>(<Parameter>,…):<Type>`; `dimension=<Dimension>` `unit=<Unit>` | A value type is used to provide a uniform definition of a quantity with units that can be shared by many value properties. | 6.3.3 |
| Enumeration Node | `«enumeration»` `<Name>` med compartment `<EnumerationLiteral>` | An enumeration defines a set of named values called literals. | 6.3.3 |
| Actor Node | `«actor»` `<Name>` (rektangel) eller stregfigur med `<Name>` | An actor is used represent the role of a human, an organization, or any external system that participates in the use of some system being investigated. | 11.3 |

> Side 3

### Table A.4 — Block Definition Diagram Nodes for Representing Interfaces

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Flow Specification Node | `«flowSpecification»` `<Name>` med compartment *flowProperties* `<Direction> <FlowProperty>:<Item>` | A flow specification defines the set of input and/or output flows for a noncomposite flow port. `<Direction>` may be one of: in, out, or inout. | 6.4.3 |
| Interface Node | `«interface»` `<Name>` med compartments *operations* `<Operation>(<Parameters>,…):<Type>` og *receptions* `«signal»<Signal>(<Parameter>,…)` | An interface is used to specify the set of behavioral features either required or provided by a standard (service-based) port. | 6.5.3 |
| Port Compartments for Block Node | `«block»` `<Name>` med compartments *standardPorts* `<Port>:<Interface>` og *flowPorts* `<Direction> <Port>:<Type>` | Ports can be shown in separate compartments labeled flow ports and standard ports. `<Direction>` may be one of: in, out, or inout. Non-atomic flow ports do not have a direction but may have the keyword {conjugated}. | 6.4.3, 6.5.2 |
| Nonatomic Flow Port Node | `<Name>:<FlowSpecification>[<Multiplicity>]` med hvidt kvadrat med dobbeltpil (normal) eller sort/skygget kvadrat (conjugate) på blokkens kant | A nonatomic flow port describes an interaction point where multiple different items may flow into or out of a block. A shaded symbol implies a conjugate port. | 6.4.3 |
| Atomic Flow Port Node | `<Name>:<Item>[<Multiplicity>]` med kvadrat med pil ind (in), dobbeltpil (inout) eller pil ud (out) | An atomic flow port describes an interaction point where an item can flow into or out of a block, or both, as indicated by the direction of the arrow in the Atomic Flow Port Node. | 6.4.3 |
| Standard Port Node | `<Interface>` ball (cirkel) — kvadrat — `<Name>[<Multiplicity>]`; `<Interface>` socket (halvcirkel) — kvadrat — `<Name>[<Multiplicity>]` | A standard port defines the service-based interaction points on the interface to a block. The shape of the `<Interface>` symbol indicates whether services are required (socket) or provided (ball) by the block. | 6.5.2, 6.5.3 |
| Interface Realization Path | Stiplet linje med hul trekantet pilespids | A realization dependency asserts that a block will declare a behavioral feature for each behavioral feature in an interface. | 6.5.3 |
| Usage Dependency Path | Stiplet linje med åben pilespids | A uses dependency asserts that a block requires a set of behavioral features defined by an interface. | 6.5.3 |

> Side 4

### Table A.5 — Block Definition Diagram Paths

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Composite Association Path | Linje med udfyldt rombe i whole-enden: `<Reference>` `<Multiplicity>` (whole-ende) — `<Name>` — `<Part>` `<Multiplicity>` (part-ende). Varianter: uden pil (reference-navn i whole-enden), med pil (kun `<End>`-navn), og lodret/træ-form hvor flere parts hænger fra samme rombe | A composite association relates a whole to its parts showing the relative multiplicity at both whole and part ends. A composite association always defines a part property in the whole (indicated by `<Part>`). Where there is no arrow on the nondiamond end of the association it also specifies a reference property to the whole in the part (indicated by `<Reference>`). Otherwise when there is an arrow, the name at the whole end simply gives a name to the association end (indicated by `<End>`). | 6.3.1 |
| Reference Association Path | Linje med hul rombe eller ingen rombe: `<Reference>` `<Multiplicity>` — `<Name>` — `<Reference>` `<Multiplicity>`; varianter med pil (`<End>` i den ene ende) og med hul rombe; træ-form med hul rombe | A reference association can be used to specify a relationship between two blocks. A reference association can specify a reference property on the blocks at one or both ends. The white diamond is the same as no diamond, but profiles can be used to differentiate them by specifying additional constraints. | 6.3.2 |
| Association Block Path and Node | Association-linje `<Reference>` `<Multiplicity>` — `<Reference>` `<Multiplicity>` med stiplet linje ned til en blok `<Name>` med compartment `«participant»{end=<Reference>}<Participant>: <Block>` | An association block, as the name implies, is a combination of an association and a block, so it can relate two blocks together but can also have internal structure and other features of its own. Participants are placeholders that represent the blocks at each end of the association block, and are used when it is desired to decompose a connector. | 6.3.2 |
| Generalization Path | Linje med hul trekantet pilespids mod den generelle blok; flere linjer kan samles i én pilespids med `<GeneralizationSet>` (evt. med stiplet linje på tværs) | A generalization describes the relationship between the general classifier and specialized classifier. A set of generalizations may either be {disjoint} or {overlapping}. They may also be {complete} or {incomplete}. | 6.6 |

> Side 5

### Table A.8 — Internal Block Diagram Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Part Node | Rektangel `<Name>:<Block>[<Multiplicity>]` (multiplicitet også i hjørnet) med compartment *initialValues* `<Property>=<ValueExpression>` | A part is a property of an owning block that is defined (typed) by another block. The part represents a usage of the defined block in the context of the owning block. Note that a Part Node may have the same compartments as a Block Node. `<Block>` represents a property-specific type. | 6.3.1, 6.6.5 |
| Actor Part Node | Stregfigur `<Name>:<Actor>[<Multiplicity>]` | An actor part is a property of a owning block that is defined (typed) by an actor. | 11.5 |
| Reference Node | *Stiplet* rektangel `<Name>:<Block>[<Multiplicity>]` med compartment *initialValues* `<Property>=<ValueExpression>` | A reference property of a block is a reference to another block. Note that a Reference Property Node may have the same compartments as a Block Node. `<Block>` represents a property-specific type. | 6.3.2 |
| Participant Property Node | Stiplet rektangel `«participant»` `{end=<Reference>}` `<Participant>:<Block>` | A participant property represents one end of an association block. Using a participant property, a modeler can show the relationship between the internal structure of the association block and the internal structure of its related ends. | 6.3.2 |
| Value Property Node | Rektangel `<Name>:<ValueType>[<Multiplicity>]=<Expression>` med compartment *initialValues* `<Property>=<ValueExpression>` | A value property describes the quantitative characteristics of a block. Note that a Value Property Node may have the same compartments as a Value Type Node. `<ValueType>` represents a property-specific type. | 6.3.3 |
| Nonatomic Flow Port Node | `<Name>:<FlowSpecification>[<Multiplicity>]` med hvidt (normal) eller skygget (conjugate) kvadrat med dobbeltpil | A nonatomic flow port describes an interaction point that allows multiple different items to flow into or out of a block. A nonatomic flow port is typed by a flow specification. A shaded symbol implies a conjugate port that reverses the items' allowable in and out flow direction. | 6.4.3 |
| Atomic Flow Port Node | `<Name>:<Item>[<Multiplicity>]` med kvadrat med pil ind / dobbeltpil / pil ud | An atomic flow port describes an interaction point where an item can flow into or out of a block, or both, as indicated by the direction of the arrow in the Atomic Flow Port Node. | 6.4.3 |
| Standard Port Node | `<Interface>` ball — kvadrat — `<Name>[<Multiplicity>]`; `<Interface>` socket — kvadrat — `<Name>[<Multiplicity>]` | A standard port describes a service-based interaction point on a block. A standard port is defined by its interface. The shape of the `<Interface>` symbol indicates whether services are required by or provided by the block. | 6.5.3 |

> Side 6

### Table A.9 — Internal Block Diagram Paths

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Connector Path | Linje `<End>` `<Multiplicity>` — `<Name>:<Association>` — `<End>` `<Multiplicity>`; variant med pilespids i den ene ende | A connector is used to bind two parts (or ports) and provides the opportunity for those parts to interact, although the connector says nothing about the nature of the interaction. | 6.3.1 |
| Connector Property Path and Node | Connector `<End>` `<Multiplicity>` — `<End>` `<Multiplicity>` med stiplet linje ned til rektangel `<Name>:<Association>` | More detail can be specified for connectors by typing them with association blocks. An association block, as the name implies, is a combination of an association and a block, so it can relate two blocks together but can also have internal structure and other features of its own. | 6.3.2 |
| Item Flow Node | Connector med udfyldt trekant (pil) på linjen og label `<Name>:<Item>, …`; to modsatrettede trekanter for flow i begge retninger | An item flow is used to specify the items that flow across a connector in a particular context. An item flow specifies the type of the item that is flowing and the direction of flow. It may also be associated to a property, called an item property, of the enclosing block to identify a specific usage of an item in the context of the enclosing block. | 6.4.2 |

> Side 7

---

## SysML Behavior diagrams

Kilde: *A Practical Guide to SysML*. Sanford Friedenthal, Alan Moore and Rick Steiner. Elsevier 2008. SysML Reference Guide: Table A11, A12, A13, A14, A15, A16, A18, A19 and A20.

> Side 8

### Table A.15 — Sequence Diagram Structural Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Lifeline Node | Rektangel `<Name>:<Type>` `[<ValueSpecification>]` `ref <Interaction>` med stiplet lodret linje | A lifeline represents the relevant lifetime of an instance that is part of the interaction's owning block, which will either be represented by a part property or a reference property. | 9.4 |
| Single-compartment Fragment Node | Ramme med tab `<UnaryOp>` og guard `[<Constraint>]` | A combined fragment can be used to model complex sequences of messages. A number of combined fragments have operators with only a single compartment for all operands, shown as `<UnaryOp>`. These are: seq, opt, break, strict, loop, neg, assert, critical. | 9.7.1, 9.7.2 |
| Multi-compartment Fragment Node | Ramme med tab `<N-aryOp>`, operander adskilt af stiplet linje, hver med `[<Constraint>]` | Two combined fragments have operators with a compartment per operand, shown as `<N-aryOp>`. These are par and alt. The lifelines that participate in the fragment overlay on top of the fragment (i.e., are visible) and lifelines that don't participate are obscured behind the fragment. (Note: This is also true of Single-Compartment Fragment Nodes.) | 9.7.1 |
| Filtering Fragment Node | Ramme med tab `<FilterOp> (<Message>,…)` | There are two combined fragments with filter operators: consider and ignore, shown as `<FilterOp>`. Inside such a construct, messages that have been explicitly ignored (or not considered) may be interleaved with valid traces. | 9.7.2 |
| State Invariant Symbol | Afrundet rektangel `<State>` på lifeline, eller `{<Constraint>}` | A state invariant on a lifeline is used to add a constraint on the required state of a lifeline at a given point in a sequence of event occurrences. The invariant constraint can include the values of properties or parameters, or the state of a state machine. | 9.7.3 |
| Interaction Use Node | Ramme med tab `ref` og `<Interaction>` i midten; stiplede pile ind/ud | An interaction use allows one interaction to reference another as part of its definition. The lifelines that participate in the interaction are obscured behind the fragment, and lifelines that don't participate overlay on top of the fragment (i.e., are visible). | 9.8 |

> Side 9

### Table A.16 — Sequence Diagram Paths and Activation Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Synchronous Message | Fuldt optrukket linje med udfyldt pilespids, `<Name>(<Argument>,…)` | A synchronous message corresponds to the synchronous invocation of an operation, and is generally accompanied by a reply message. | 9.5.1 |
| Asynchronous Message | Fuldt optrukket linje med åben pilespids, `<Name>(<Argument>,…)` | Asynchronous messages correspond to either the sending of a signal or to an asynchronous invocation (or call) of an operation, and do not require a reply message. | 9.5.1 |
| Reply Message | Stiplet linje med åben pilespids, `<Attribute>=<Name>(<Attribute>=<Argument>,…):<Argument>` | A reply message shows a reply to a synchronous operation call, together with any return arguments. | 9.5.1 |
| Found Message Path | Pil der starter i en udfyldt cirkel, `<Name>(<Argument>,…)` | A found message describes the case where there is a receiving event for the message but no sending event. | 9.5.2 |
| Lost Message Path | Pil der ender i en udfyldt cirkel, `<Name>(<Argument>,…)` | A lost message describes the case where there is sending event for the message but no receiving event. | 9.5.2 |
| Focus of Control (Activation) Node | Smalt rektangel på lifeline; nestede aktiveringer forskudt til højre; alternativt et bredere rektangel med `<Name>` | Focus of control bars or activations are overlaid on lifelines and correspond to executions; they begin at the execution's start event and end at the execution's end event. When executions are nested, the focus of control bars are stacked from left to right. An alternate notation for activation is a box symbol overlaid on the lifeline with the name of the behavior or action inside. | 9.5.4 |
| Create Message Path | Stiplet pil `<Name>(<Argument>,…)` der ender i lifeline-headen | The creation of an instance is indicated by the receipt of a create message. | 9.5.5 |
| Destroy Event Node | Kryds (X) nederst på lifeline | An instance's destruction is indicated by the occurrence of a destroy event. | 9.5.5 |
| Coregion Symbol | Firkantede parenteser langs lifeline | Within a coregion, there is no implied order between any messages sent or received by the lifeline. | 9.7.1 |

> Side 10

### Table A.18 — State Machine Diagram State Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| State Machine with Entry- and Exit-Point Pseudostate Nodes | Ramme `stm <StateMachine>` med cirkel `<Name>` (entry point) og cirkel med kryds `<Name>` (exit point) på kanten | A state machine may have entry- and exit-point pseudostates, which are similar to junctions. On state machines, entry-point pseudostates can only have outgoing transitions and exit-point pseudostates can only have incoming transitions. | 10.6.5 |
| Atomic State Node | Afrundet rektangel `<State>` med body: `Entry/<Behavior>`, `Exit/<Behavior>`, `Do/<Behavior>`, `<Event>[<Constraint>]/<Behavior>`, `<Event>/defer` | A state represents some significant condition in the life of a block, typically because it represents some change in how the block responds to events. Each state may have entry and exit behaviors that are performed whenever the state is entered or exited, respectively. In addition, the state may perform a do activity that executes once the entry behavior has completed and continues to execute until it completes or the state is exited. | 10.3 |
| Composite State with Entry- and Exit-Point Pseudostate Nodes | Afrundet rektangel `<State>` med nested region; entry point (cirkel `<Name>`) og exit point (cirkel med kryds `<Name>`) på kanten | A composite state is a state with nested regions; the most common case is a single region. A composite state may have entry- and exit-point pseudostates that act like junction pseudostates. Entry points have incoming transitions from outside the state and exit points have the opposite. | 10.6.1 |
| Composite State Node with Multiple Regions | `<State>` med regioner adskilt af stiplet linje (lodret eller vandret) | A composite state may have many regions, which may each contain substates. These regions are orthogonal to each other and so a composite state with more than one region is sometimes called an orthogonal composite state. | 10.6.2 |
| Sub-State Machine Node with Connection Points | Afrundet rektangel `<State>:<StateMachine>` med entry/exit-point cirkler `<Name>` og et lille "connection point"-symbol (to små cirkler forbundet) i bunden | A state machine may be reused using a kind of state called a submachine state. A transition ending on a submachine state will start its referenced state machine. Transitions may also be connected to connection points on the boundary of the state. | 10.6.5 |

> Side 11

### Table A.19 — State Machine Diagram Pseudostate and Transition Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Terminate Pseudostate Node | Pil til et kryds (X) | If a terminate pseudostate is reached, then the behavior of the state machine terminates. | 10.3 |
| Initial Pseudostate Node | Udfyldt cirkel med udgående pil | An initial pseudostate specifies the initial state of a region. | 10.3 |
| Final State Node | Pil til cirkel med udfyldt prik i midten ("bull's eye") | The final state indicates that a region has completed execution. | 10.3 |
| Choice Pseudostate Node | Rombe | The outgoing transitions of a choice pseudostate are evaluated once it has been reached. | 10.4.2 |
| Junction Pseudostate Node | Udfyldt cirkel (uden udgående pil i notationen) | A junction pseudostate is used to construct a compound transition path between states. | 10.4.2 |
| Trigger Node | Femkant/flag med indhak `<Event>,… [<Constraint>]` | This node represents all the transition's triggers, with the descriptions of the triggering events and the transition guard inside the symbol. | 10.4.3 |
| Action Node | Rektangel `<EffectExpression>` | `<EffectExpression>` describes the effect of the transition, either the name of a behavior or the body of an opaque behavior. | 10.4.3 |
| Send Signal Node | Femkant med spids `<Signal>(<Argument>,…)` | This node represents a send signal action. The signal's name, together with any arguments that are being sent, are shown within the symbol. | 10.4.3 |
| Join Pseudostate Node | Tyk lodret bjælke med flere indgående pile og én udgående | A join pseudostate has a single outgoing transition and many incoming transitions. When all of the incoming transitions can be taken, and the join's outgoing transition is valid, then all the transitions happen. | 10.6.2 |
| Fork Pseudostate Node | Tyk lodret bjælke med én indgående pil og flere udgående | A fork pseudostate has a single incoming transition and many outgoing transitions. When an incoming transition is taken to the fork pseudostate, all of the outgoing transitions are taken. | 10.6.2 |
| History Pseudostate Node | Cirkel med `H` (shallow) eller `H*` (deep) | A history pseudostate represents the last state of its owning region, and a transition ending on a history pseudostate has the effect of returning the region to the state it was last in. | 10.6.4 |

> Side 12

### Table A.20 — State Machine Diagram Paths

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Time Event Transition Path | `after <TimeExpression>[<Constraint>]/<Behavior>` eller `at <TimeExpression>[<Constraint>]/<Behavior>` på en pil | Time events indicate either that a given time interval has passed since the current state was entered (**after**), or that a given instant of time has been reached (**at**). The transition can also include a guard and effect. | 10.4.1 |
| Signal Event Transition Path | `<Signal>(<Attribute>,…)[<Constraint>]/<Behavior>` | Signal events indicate that a new asynchronous message has arrived. A signal event may be accompanied by a number of arguments, which may be assigned to attributes. The transition can also include a guard and effect. | 10.4.1 |
| Call Event Transition Path | `<Operation>(<Attribute>,…)[<Constraint>]/<Behavior>` | Call events indicate that an operation on the state machine's owning block has been requested. A call event may also be accompanied by a number of arguments, which may be assigned to attributes. The transition can also include a guard and effect. | 10.5 |
| Change Event Transition Path | `when <Expression>[<Constraint>]/<Behavior>` | Change events indicate that some condition has been satisfied (normally that some specific set of attribute values hold). The transition can also include a guard and behavior/effect. | 10.7 |

> Side 13

### Table A.11 — Activity Diagram Structural Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Activity Parameter Node | Rektangler `<Parameter>:<Type>:<Multiplicity>` der sidder på kanten af rammen `act <Activity>` | Activity parameter node symbols are rectangles that straddle the boundary of the activity frame. Other annotations include: «noBuffer», «optional», «overwrite», «continuous», «discrete», {rate=`<Expression>`}. Parameters can be organized into parameter sets, indicated by a bounding box around the parameters in the set. Parameter sets may overlap, and may have an annotation: {probability=`<Expression>`}. | 8.4.1 |
| Interruptible Region Node | Stiplet afrundet rektangel | An interruptible region groups a subset of the actions within an activity and includes a mechanism for stopping their execution. Stopping the execution of these actions does not effect other actions in the activity. | 8.8.1 |
| Activity Partition Node | Vandret eller lodret "swimlane" med `<Partition>` i headeren | A set of activity nodes can be grouped into an activity partition (also known as a swimlane) that is used to indicate responsibility for execution of those nodes. `<Partition>` may be the name of a block or name and type of a part/reference. Partitions may overlap in a grid pattern. | 8.9.1 |
| Activity Partition in Action Node | Afrundet rektangel med `(<Partition>,…)` over `<Name>:<Behavior>` | An alternative representation for an activity partition for call actions is to include the name of the partition or partitions in parentheses inside the node above the action name. This can make the activity easier to layout than when using the swimlane notation. | 8.9.1 |

> Side 14

### Table A.12 — Activity Diagram Control Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Merge Node | Rombe med flere indgående pile og én udgående | A merge node has one output flow and multiple input flows — it routes each input token received on any input flow to its output flow. Unlike a join node, a merge node does not require tokens on all its input flows before offering them on its output flow. Rather it offers tokens on its output flow as soon as it receives them. | 8.5.1, 8.6.1 |
| Decision Node | Rombe med én indgående pil og flere udgående med `[<Expression>]`; evt. note `«decisionInput»` `<Behavior>` | A decision node has one input flow and multiple output flows — an input token can only traverse one output flow. The output flow is typically established by placing mutually exclusive guards on all outgoing flows and offering the token to the flow whose guard expression is satisfied. A decision node can have an accompanying decision input behavior, which is used to evaluate each incoming object token and whose result can be used in guard expressions. | 8.5.1, 8.6.1 |
| Join Node | Tyk bjælke med flere indgående og én udgående pil; evt. `{joinSpec=<Expression>}` | A join node has one output flow and multiple input flows — it has the important characteristic of synchronizing the flow of tokens from many sources. Its default behavior can be overridden by providing a join specification, which can specify additional control logic. | 8.5.1, 8.6.1 |
| Fork Node | Tyk bjælke med én indgående og flere udgående pile | A fork node has one input flow and multiple output flows — it replicates every input token it receives onto each of its output flows. The tokens on each output flow may be handled independently and concurrently. | 8.5.1, 8.6.1 |
| Initial Node | Udfyldt cirkel med stiplet udgående pil | When an activity starts executing a control token is placed on each initial node in the activity. The token can then trigger the execution of an action via an outgoing control flow. | 8.6.1 |
| Activity Final Node | Cirkel med udfyldt prik i midten | When a control or object token reaches an activity final node during the execution of an activity, the execution terminates. | 8.6.1 |
| Flow Final Node | Cirkel med kryds | Control or object tokens received at a flow final node are consumed but have no effect on the execution of the enclosing activity. Typically they are used to terminate a particular sequence of actions without terminating an activity. | 8.6.1 |

> Side 15

### Table A.13 — Activity Diagram Object and Action Nodes

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Call Action Node | Afrundet rektangel `<Name>:<Behavior>` med pins (små kvadrater) `<Name>:<Type>[<State>,…]` på kanten; variant `<Name>:<Operation>` med `target`-pin og stiplet note med `«localPrecondition»` `<Constraint>` / `«localPostcondition»` `<Constraint>` | Call actions can invoke other behaviors either directly or through an operation, and are referred to as call behavior actions and call operation actions, respectively. A call action must own a set of pins that match in number and type of the parameters of the invoked behavior/operation. A called operation requires a target. Streaming pins may be marked as {stream} or filled (as shown). Where the parameters of the called entity are grouped into sets, the corresponding pins are as well. Pre- and postconditions that constrain the action such that it cannot begin to execute unless the precondition is satisfied, and must satisfy the postcondition to successfully complete execution. | 8.1, 8.3, 8.4.2 |
| Central Buffer Node | Rektangel `«centralBufferNode»` `<Name>:<Type>` `[<State>,…]` | A central buffer node provides a store for object tokens outside of pins and parameter nodes. Tokens flow into a central buffer node and are stored there until they flow out again. | 8.5.3 |
| Datastore Node | Rektangel `«dataStore»` `<Name>:<Type>` `[<State>,…]` | A datastore node provides a copy of a stored token rather than the original. When an input token represents an object that is already in the store, it overwrites the previous token. | 8.5.3 |
| Control Operator Action Node | Afrundet rektangel `«controlOperator»` `<Name>:<ControlOperator>` med udgående `{control}`-pin | A control operator produces control values on an output parameter, and is able to accept a control value on an input parameter (treated as an object token). It is used to specify logic for enabling and disabling other actions. | 8.6.2 |
| Accept Event Action Node | Femkant med indhak `<Event>,…` | An activity can accept events using an accept event action. The action has (sometimes hidden) output pins for received data. | 8.7 |
| Accept Time Event Node | Timeglas med `<TimeExpression>` | A time event corresponds to an expiration of an (implicit) timer. In this case the action has a single (typically hidden) output pin that outputs a token containing the time of the accepted event occurrence. | 8.7 |
| Send Signal Action | Femkant med spids `<Signal>` med pins `signal` og `target` | An activity can send signals using a send signal action. It typically has pins corresponding to the signal data to be sent and the target for the signal. | 8.7 |
| Primitive Action Node | Afrundet rektangel `«<ActionType>»` `<Expression>` | Primitive actions include: object access/update/manipulation actions, which involve properties and variables, and value actions, which allow the specification of values. The `<Expression>` will depend on the nature of the action. | 8.12.1 |

> Side 16

### Table A.14 — Activity Diagram Paths

| Diagram Element | Notation | Description | Section |
|---|---|---|---|
| Object Flow Path | Fuldt optrukket pil med `[<Expression>]` | Object flows connect inputs and outputs. Additional annotations include «continuous», «discrete», {rate=`<Expression>`}, {probability=`<Expression>`}. | 8.1, 8.5 |
| Control Flow Path | Stiplet pil eller fuldt optrukket pil med `[<Expression>]` | Control flows provide constraints on when, and in what order, the actions within an activity will execute. A control flow can be represented using a solid line, or using a dashed line to more clearly distinguish it from object flow. | 8.1, 8.6 |
| Object Flow Node | Rektangel `<Name>:<Type>` `[<State>,…]` mellem to aktioner (erstatter pin-par) | When an object flow is between two pins that have the same characteristics, an alternative notation can be used where the pin symbols are elided and replaced by a single rectangular symbol called an object node symbol. | 8.5 |
| Interrupting Edge Path | Pil med lyn-symbol (zigzag) og `[<Expression>]` | An interrupting edge interrupts the execution of the actions in an interruptible region. Its source is a node inside the region and its destination is a node outside it. | 8.8.1 |

> Side 17

---

## Hurtig oversigt: de fire kursusdiagrammer

| Diagram | Forkortelse | Hovedelementer (nodes) | Hovedrelationer (paths) | Tabeller |
|---|---|---|---|---|
| Block Definition Diagram | bdd | block, value type, enumeration, actor, flow specification, interface, ports (atomic/nonatomic/standard) | composite association (udfyldt rombe), reference association (hul rombe/ingen), generalization (hul trekant), association block | A.3, A.4, A.5 |
| Internal Block Diagram | ibd | part, actor part, reference, value property, ports | connector, connector property, item flow | A.8, A.9 |
| Sequence Diagram | sd | lifeline, fragments (alt, opt, loop, par, seq, break, strict, neg, assert, critical, consider, ignore), state invariant, interaction use (ref), activation | synchronous / asynchronous / reply / found / lost / create message, destroy event, coregion | A.15, A.16 |
| State Machine Diagram | stm | atomic state (entry/do/exit/internal transitions), composite state, regions, submachine, initial, final, choice, junction, fork, join, history, terminate, entry/exit points | time event (after/at), signal event, call event, change event (when) — alle med `[guard]/effect` | A.18, A.19, A.20 |
