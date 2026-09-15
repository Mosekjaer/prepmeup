# System Design

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*James K. Peckol, *Embedded Systems Design, A Contemporary Design Tool*. Wiley, ISBN 978-0-471-72180-2. Chapter 9, System Design, pp. 366–369, pp. 376–390.*

The time and frequency measurements will be implemented to provide three user selectable resolution ranges: high frequency range/shorter duration signals, a second for midrange frequency/midrange duration signals, and a third for low frequency/longer duration signals. The events measurement capability will support two selectable counting durations, shorter and longer.

For frequency, period, and events measurements, the user will be able to select either a positive or negative edge trigger. For interval measurements, the user will be able to select the polarity of the start and stop signals independently.

#### Operating Specifications

The system shall operate in a standard commercial/industrial environment.

- Temperature Range 0–85C

- Humidity up to 90% RH noncondensing

- Power 120–240 VAC 50 Hz, 60 Hz, 400 Hz, 15 VDC

The system shall operate for a minimum of 8 hours on a fully charged battery.

The system time base shall meet the following specifications.

| Temperature stability 0–50C | $`< 6 \times 10^{-6}`$  |
|:----------------------------|:------------------------|
| Aging Rate                  |                         |
| 90 day                      | $`< 3 \times 10^{-8}`$  |
| 6 month                     | $`< 6 \times 10^{-7}`$  |
| 1 year                      | $`< 25 \times 10^{-6}`$ |

#### Reliability and Safety Specification

The counter shall comply with the appropriate standards.

|        |                                        |
|:-------|:---------------------------------------|
| Safety | UL-3111-1, IEC-1010, CSA 1010.1        |
| EMC    | CISPR-11, IEC 801-2, -3, -4, EN50082-1 |
| MTBF   | Minimum of 10,000 hours                |

## The System Design Specification

The *System Design Specification* is based on the *System Requirements Specification* and specifies the how of the design, not the what. The specification is written in the designer’s language and from the designer’s point of view. It serves as a bridge between the customer and the designer, as we see in Figure 1.1.

*Figur: The Customer, the Requirements, the Design, and the Engineer* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Whereas the *Requirements Specification* provides a view from the outside of the system looking in, the *Design Specification* provides a view from the inside looking out as well. Notice also that the *Design Specification* has two masters:

- It must specify the system’s public interface from inside the system.

- It must specify how the requirements defined for and by the public interface are to be met by the internal functions of the system.

We have seen that the *Requirements Specification* is written in less formal terms with the intent of capturing the customer’s view of the product. The *Design Specification* must formalize those requirements in precise, unambiguous language. Putting the inevitable changes that occur during the lifetime of any project aside for the moment, we find that the design specification should be sufficiently clear, robust, and complete that a group of engineers could develop the product without ever talking to the author of the specification.

> **Design Note**
>
> A good litmus test of the viability of a design specification is the question, “If I send this to my colleague (who is working for one of our subcontractors), will he or she understand this?” If the answer is no, the specification should be reexamined.

### The System

As part of formalizing and quantifying the system’s requirements, one must attach concrete numbers, tolerances, and constraints to all of the system’s input and output signals. All timing relationships must be defined. The system’s functional and operational behaviors are described in detail.

### Quantifying the System

The quantification of the system’s characteristics begins with the inputs and outputs, based on the specified requirements. The necessary technical details are added to enable the engineer to accurately and faithfully execute the actual design.

- *System Inputs and Outputs*

- For each I/O variable, the following are specified.

- The name of the signal

- The use of the signal as an input or output

- The nature of the signal as an event, data, state variable, and so on.

Starting with the requirements specification, we provide detailed descriptions as necessary and incorporate any additional technical or technological constraints that may be needed.

- The complete specification of the signal, including nominal value, range, level tolerances, timing, and timing tolerances.

- The interrelationships with other signals, including any constraints on those relationships.

<!-- -->

- *Responsibilities—Activities*

- *Functional and Operational Specifications*

The functional and operational specifications that will quantify the dynamic behavior of the system are now formulated. The functional requirements specification identifies the major functions that the system must perform from a high-level view. The operational specification endeavors to capture specific details of how those functions behave within the context of the operating environment.

The manner in which a particular function must operate, the conditions imposed on the operation, and the range of that operation are now captured. The specification must consider concrete numbers: precisions and tolerances, ordinary and extraordinary operating modes, limits, and expected operating ranges. The specification should include all details needed by the designer or implementer.

In stating the specific design requirements for the system, one can use bullet equations or algorithms, formal design language, or pseudo code. In addition to detailed UML diagrams, such as state charts, sequence diagrams, and timing diagrams, schematics, code, or parts lists are not included, except in limited circumstances.

- *Technological (and Other) Specifications*

The technological portion includes all detailed and concrete specifications that are relevant to the design of the system hardware and software. Five areas that should be considered can easily be identified.

1.  *Geographical constraints.* Distributed applications can span a single room, a campus, a country, or an encompassing worldwide system of communications.

2.  *Characterization of and constraints on interface signals.* The assumption is made that signals between the system and the external world are electrical, optical, or wireless, and that they can be converted into or from some digital form.

3.  *User interface requirements.* If the system interfaces to external-world devices such as medical or instrumentation equipment, how information is presented and whether relevant data are associated protocols must be considered.

4.  *Temporal constraints.* The system may have to perform under hard or soft real-time constraints. Such constraints may specify delays on signals originating from external entities, responses to system outputs, or internal system delays.

5.  *Electrical infrastructure considerations.* There must be a specification for the electrical characteristics of any electrical infrastructure. Included in this portion of the specification are power consumption, necessary power supplies, tolerances and capacities of such supplies, tolerance to degraded power, and power management schemes.

6.  *Safety and Reliability.* In formulating the design requirements for safety and reliability, the focus shifts to the detailed objectives and the strategy for achieving those goals.

Safety considerations should address understanding and specifying any environmental and safety issues. The reliability specification should include requirements for diagnostic tests, remote maintenance, remote upgrade, and their details; concrete numbers for MTTF and MTBF of any built-in self-test circuitry; concrete numbers for MTTF and MTBF of the system itself; and consideration of system performance under partial or full failure.

Let’s now bring everything together.

#### Quantifying the specification

We will now continue with the development of the counter. The *Design Specification* will follow, but extend, what has been captured in the *Requirements Specification*. The focus will now be on providing specific numbers, ranges, and tolerances for signals that are within the system.

Once again, we will put together any thoughts about the environment and the system prior to writing the specification.

**Environment.** Specifications relating to the environment have been discussed earlier. There are no changes here.

**Counter.**

- When specifying measurement and stimulus equipment, the specifications for that equipment are generally 10 times (one order of magnitude) better than those for the signals that must be measured or generated.

- That margin is provided when specifying the range and tolerances on the counter’s measurement capabilities.

- Specifications on counting events are based on the granularity of the timing of the interval during which the events are counted.

- The values to be displayed at the measurement boundaries are now defined.

The next step is to provide any additional detail that may be needed and to fully quantify the counter specifications.

> **System Design Specification for a Digital Counter**
>
> *System Description.* This specification describes and defines the basic requirements for a digital counter. The counter is to be able to measure frequency, period, time interval, and events. The system supports three measurement ranges for each signal and two for events. The counter is to be manually operated with the ability to support remote operation. The counter is to be low cost and flexible so that it may be utilized in a variety of applications.
>
> *Specification of External Environment.* The counter is to operate in an industrial environment in a commercial grade temperature and lighting environment. The unit will support either line power or battery operation. Specific details are included under Operating Specifications.

------------------------------------------------------------------------

*\[Kompendiet springer originalbog-sider 370–375 over\]*

Ideally, a specification document should be complete, consistent, comprehensible, traceable to the requirements, unambiguous, modifiable, and able to be written. The specification should be expressed in as formal a language or notation as possible, yet readable. It should also be executable. A *System Specification* should focus precisely on the system itself. It should provide a complete description of its externally visible characteristics, that is, its public interface. External visibility clearly separates those aspects that are functionally visible to the environment in which the system operates from those aspects of the system that reflect its internal structure.

## Partitioning and Decomposing a System

At this point in the design cycle, all of the system requirements have been identified, captured, and formalized into the *System Design Specification*. The next step is to move inside the system and begin the process of specifying and designing the functionality that gives rise to the external behavior.

Throughout all of the previous discussions, modularity and encapsulation have been repeatedly stressed. We will look first at why such an approach is recommended and then at what should be considered as the process of decomposing and ultimately partitioning the system into hardware and software modules proceeds.

### Initial Thoughts

Reuse is an important reason for partitioning. With each new design, one should always look to the previous project as well as the next one. What can be used from the last project to expedite the development of this one? How can the current design be implemented to support a future feature? Can parts of this design be used in future projects?

Many compilers generate object code in segments, one for each module. Such actions may place size restrictions on the individual modules. Poor module builds can significantly affect memory accesses, increase cache misses, promote thrashing, and significantly reduce performance.

Work assignments are often made on a module-by-module basis. Module boundaries should be defined so as to minimize interfaces among different parts of the system. Such a practice simplifies the process of subcontracting some of the work as well. Security issues also play a role when subcontracting is considered. Whether working for a government project or for a company on sensitive government work, one needs to consider what information to make available to outside vendors. By properly decomposing a system, the portions that can be outsourced and those for which control over should be retained can be more easily identified.

The modules should be packaged with the goal of stabilizing module interfaces during the early part of the design. Partitioning the system into well-defined, loosely coupled modules helps ensure a safe and robust design. Such an approach helps prevent a failure in one part of the system from propagating into and affecting another.

The importance of partitioning a new design should be evident; the next step is to examine the process for doing so. The process starts with the top-level system model and progressively refines that model into smaller and more manageable pieces that can more easily be designed and built.

Initially, the focus is on a functional view of the system rather than on specific pieces of hardware and software. It is important first to understand and to capture the behavior at a high level. The next step is to map those functions, that functionality, onto the hardware and software elements necessary to satisfy the constraints identified during the initial phases of the design. Partitioning is important during the early stages of the development of the system first as an aid in attacking the complexity of a large system and later as a guide in arriving at a sound physical architecture.

Prior to beginning the system partition, keep some general thoughts in mind.

1.  Remember that with every rule or guideline, there must always be room for exceptions.

2.  Each module should solve one well-defined piece of the problem.

3.  Mixing functionality across modules makes all aspects of development and support process much more difficult.

4.  Partitioning should be done so that connections between modules are only introduced because of connections between pieces of problem.

5.  Partitioning should ensure that connections between modules are as independent as possible.

6.  Partitioning is also done to help meet the economic goals of the design.

When forming partitions, the process must be considered from a number of viewpoints. At the end of the day, if the system may meet neither the customer’s expectations nor the performance specifications, the system architecture must change.

As the decomposition process proceeds, the design should first be considered from a functional point of view. The outcome from the decomposition steps is a functional model that can be used to define the system architecture. Among the many things that should be considered, two that should appear early in the process are coupling and cohesion.

### Coupling

Coupling is a heuristic that provides an estimate of how interdependent the modules are. Tightly coupled modules will generally utilize shared data or interchange control information. As module interdependence increases, so does the complexity of managing those modules, and the more difficulty one will have in debugging the design during development, troubleshooting the system in the event of field failures, maintaining the modules and system, and modifying the design to add features or capabilities.

The major goal is to make the system’s modules as independent as possible and to reduce or minimize coupling.

> **Design Heuristic** The lower the coupling, the better job that has been done during partitioning.

During the early stages of the design, think about the following to help reduce coupling:

1.  Eliminate all unessential interaction between modules.

2.  Minimize the amount of essential interaction between modules.

3.  Loosen the essential interaction between modules, if possible.

Unnecessary interaction demands a high degree of coordination between several modules to accomplish a task or to ensure error-free communication; simply pass the module the information necessary to get the job done. Wait for an indication that the task has completed. Execute some other part of the task.

### Cohesion

An idea related to coupling is cohesion. The notion of coupling addresses the partitioning of a system; cohesion addresses bringing the pieces together. Cohesion is a measure of strength of the functional relatedness of elements in a module. The goal is to create strong, highly cohesive modules whose elements are genuinely and tightly related to one another. Conversely, elements should not be strongly related to elements in another module. We want to maximize cohesion and minimize coupling.

> **Functional Cohesion** The module implements a single task, and all comprising elements contribute to the execution of that one task.
>
> **Sequential Cohesion** The module implements a task as a sequential set of procedures. The output data of each procedure becomes the input data to the next. All comprising elements are involved in one of those procedures.
>
> **Communicational Cohesion** The module implements a task that has a number of procedures working on the same set of input data such as an image processing task.
>
> **Procedural Cohesion** The module implements a number of procedures that may or may not be related to a common activity. Control, rather than data, flows from one procedure to the next.
>
> **Temporal Cohesion** The module implements a number of unrelated procedures or activities that are sequentially ordered in time.
>
> **Logical Cohesion** The module implements a number of procedures that are possible alternative methods for accomplishing a task. A subset of those alternatives is selected by an outside user to actually execute the task.
>
> **Coincidental Cohesion** The module aggregates a number of unrelated procedures. Such cohesion, or lack thereof, should not be used.

| Cohesion | Coupling | Ease of Modification | Ease of Understanding | Ease of Maintenance |
|:---|:--:|:--:|:--:|:--:|
| Functional | 5 | 5 | 5 | 5 |
| Sequential | 4 | 4 | 4 | 3–4 |
| Communicational | 3 | 3 | 3 | 3 |
| Procedural | 2–3 | 3 | 2–3 | 2 |
| Temporal | 1 | 2–3 | 3 | 2 |
| Logical | 1 | 3 | 2–3 | 1 |
| Coincidental | 1 | 1 | 1 | 1 |

Comparison of Coupling and Types Cohesion from Different Perspectives

Cohesion and coupling analyses provide a good set of metrics by which to begin to assess the high-level architectural aspects of a design. The work is subjective and still must be guided by experience, context, and the specific requirements.

### More Considerations

With today’s systems, a spatial point of view is often essential. This is an external view of the system, and it yields a distributed functional architecture. With such a view, performance and communication costs are taken into consideration.

Closely associated with the spatial viewpoint is that of resource allocation. Such efforts result in a resource architecture. Once again, performance, costs, and dependability are factors that must be considered.

Finally, one must consider the hardware and the software. Decomposition becomes a design process that leads to a hardware architecture as was discussed earlier. Now performance must be considered. As embedded developers, we are playing a direct role in the design and selection of the hardware platform as well as the software environment. Making trade-offs intelligently in these two areas can take us a long way toward developing a safe, robust, and high-quality/high-performance system.

## Functional Design

The purpose at this stage of the design is to find an appropriate internal functional architecture for the system. We are beginning to formalize how the requirements that have been identified can be implemented. The current focus is on analyzing the problem. Through such analysis, one should lose understanding of the design can be transformed into a precise description. The result of such a process is a detailed textual or graphical description of the system. The end result is a complete consistent functional definition of the required tasks.

To establish an appreciation of a functional model of a system, consider an aircraft. If an aircraft is the system to be designed, the top-level functional model should probably not consist of three major functions: take-off, fly, and land. With such a view, we make no statements about such issues as the support structure for the aircraft, propulsion, the propulsion system, control aircraft or blade, helicopter, or the method of lift. Such decisions can be postponed until later. The advantage of such an approach is early flexibility: one can explore before beginning to constrain the system. A functional description simply formalizes the intended behavior of the design.

The functional description should be written to be understood by those knowledgeable in the application domain and by those who will do the hardware and software development. The specification must also be such that it can be reviewed by the many diverse and interested parties and tested against reality.

A first functional decomposition is carried out based on a search of essential internal variables and events in the system. The design process then consists of successive refinements or decompositions for each function using exactly the same process until elementary or leaf functions are obtained. Such decomposition forms a functional model of the system. The model expressed by the collection of such functions should be sufficient to verify the design quality and to evaluate system behavior and performance.

During modeling and verification, the system’s operations and associated performance requirements can be allocated to the internal functions and the relations between such functions can be defined. Such a process also allows one to estimate the expected performance of the system.

The functional model is different from the specification and also from the physical architecture that will be developed next. The specification describes the external behavior of the system; the functional model targets the internal behavior that will lead to that external behavior. The architectural model addresses the physical hardware and software components onto which the functions are mapped.

Figure 1.2 illustrates a first-level decomposition of a simple input/output task. The system must receive data from and transmit data to the outside world. Associated with the task is a code conversion to ASCII.

*Figur: First-Level I/O Task Decomposition* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Each of these functions may be further decomposed as necessary. If required, the second-level functions may also be successively refined to give the detail needed to understand and to execute the design.

The next step in the analysis is to identify the messages that flow between the user and other active external objects and the system as well as the internal signals that flow between the major functional blocks.

The first diagram, in Figure 1.3, presents an aggregation of the objects in the system. This aggregation includes both the environment and the counter being designed.

*Figur: A Model of the Environment and the Counter as an Aggregation of Objects* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

The model of the measurement system is expressed as a collection of the user, the factory, the future remote computer, and the counter. The factory is an aggregation of test lines and numbers of navigation radios that must be tested.

The design specification provided a high-level block diagram of the system. For this problem such a diagram provides a good starting place for the initial hierarchical decomposition of the system. Figure 1.4 elaborates on the counter component and gives one possible decomposition for that system.

*Figur: A Possible Hierarchical Decomposition of the Counter System* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

This drawing is somewhat straightforward. Front-panel operations tend to be rather straightforward; the remote operations can be more involved. Certainly, these are not the only choices. The reader may infer that Figure 1.5 captures the interface between the counter and the surrounding environment.

*Figur: The Counter–Environment Interface* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

The next drawing, in Figure 1.6, expresses a functional partition and the signal flow between the major functional blocks.

*Figur: A Functional Partition of the Counter System* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Next the system architecture is formulated, and functions are mapped onto the hardware and software blocks comprising that system.

## Architectural Design

In executing an architectural design, the goal is to select the most appropriate solution to the original problems based on exploration of a variety of architectures and the choice of the best-suited hardware/software partitioning and allocation of functionality.

### Mapping Functions to Hardware

The view of a partition now changes to reflect a more detailed understanding of the system and involves the mapping or allocation of each functional module onto the appropriate physical hardware or software block(s). Such a mapping completely describes the hardware implementation of the system.

As noted earlier, one should always endeavor to broaden the scope of the architectural design so as not to preclude possible future enhancements. Certainly, this involves a balancing act between generality and practicality as well as simultaneously satisfying other specified requirements. Nonetheless, the plan should be for a system that evolves over its lifetime; if this is done well, add-ons that are inevitable in today’s systems will be much easier.

The major objective of the architectural design activity is the allocation or mapping of the different pieces of system functionality to the appropriate hardware and software blocks. Work is based on the detailed functional structure. The performance requirements are analyzed, and finally the constraints imposed by the available technologies as well as those that arise from the hardware and software specifications are taken into consideration.

- The important constraints that must be considered include geographical distribution.

- Physical and user interfaces.

- System performance specifications.

- Timing constraints and dependability requirements.

- Power consumption.

- Legacy components and cost.

Such constraints are strong factors for deciding which portions of the system should be implemented in software and which portions should be done in hardware. The process of assigning the pieces of functionality is generally obvious for a significant part of the system. For those, it is easy to say, “this part must be hardware” or “this part must be software.” The power supply, display, communication port, and package containing the system are necessarily hardware. The operating system and associated drivers, if present, are generally software.

The situation is expressed graphically in Figure 1.7. There is a gray area between the hardware and software where the implementation approach is not precisely defined.

*Figur: The Hardware–Software Continuum* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

The mapping onto such an architecture completely defines the hardware implementation of the system. The hardware portion of the system is specified by a physical architecture that may comprise one or more microprocessors, complex logical devices or arrayed logics, and custom-integrated circuits. In today’s systems, these microprocessors and microcontrollers can take on a variety of personalities: CISC, RISC, and DSP.

For most applications, a substantial portion of the software can be easily separated from the hardware and thereby permits concurrent development. The remaining part, the hardware boundary, is more difficult to partition and falls under what is called co-design.

### Hardware and Software Specification and Design

The system specification gives a detailed identification of the system’s inputs, outputs, and functional behavior based on our original requirements. The functional decomposition is analogous to those steps taken in defining the requirements. As the architecture of the design begins to take shape, the objective is to determine as fully as possible the specifications for each physical component in the system and the interface between them.

For each software component of the architecture, a detailed software specification that expresses the priority of each task and the temporal and spatial information is necessary. Such a software specification uses UML diagrams, including detailed state charts, timing diagrams, sequence diagrams, activity diagrams, and collaboration diagrams, and can be very useful at this stage in the design.

A software implementation may or may not use a real-time kernel. With an off-the-shelf real-time kernel, the development time is reduced, but not the factory cost or time-based performance specifications. For systems that do not use a real-time kernel, one can achieve a better optimization of the design when addressing high-speed, hard real-time constraints. Under such circumstances, the solution is being hand tailored to the specific problem rather than adapting a general-purpose solution to a specific case.

For the software design, the following must be analyzed and decided: whether to use a real-time kernel; whether several functions can be combined in order to reduce the number of software tasks; a priority for each task; and an implementation technique for each intertask relationship.

Under such circumstances, a frequent choice is the Rate-Monotonic Scheduling policy. Permanent functions are assigned higher priority, and some cyclic functions without timing constraints are usually implemented within a background task.

For the implementation of intertask relationships, it is desirable to use procedure calls as much as possible, thereby simplifying the organizational part and reducing the intertask overhead. Such an implementation is only possible between functions with increasing relative priorities. Tasks triggered by hardware events are invoked through the processor interrupt or polling systems.

For each specific subpart of the system in which the partition is not obvious, a detailed specification is written; the final hardware/software partition is determined through a process of successive requirements.

The next step in developing the counter begins with formulating the hardware architecture; the software architecture follows. We then map each of the functions identified earlier onto the architecture. Figure 1.8 presents the hardware components.

*Figur: The Hardware Architecture of the Counter* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

In the design, the microprocessor, the display, the front panel controls, and the power system are clearly hardware. In theory, the clock system as well as the counter-divider chain and associated control could be implemented in software. However, the frequency at which the counter is intended to operate makes the decision toward a hardware solution.

Figure 1.9 identifies the major software tasks, shared data, and I/O in a data and control flow diagram. The front panel task is continually checking for polling or indirectly by interrupt to the state of the front panel for user input. A change in input is captured and passed to the display task, which will update the display accordingly, and to the measurement task. The measurement task issues the appropriate commands to the external counter-divider chain control block. At the end of each measurement, the raw data is read from the counter-divider and passed to the output task.

*Figur: A Data and Control Flow Diagram for the Counter System* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

The output task properly formats the data and sends it to the display task for display on the front panel. The master control task manages the scheduling of all tasks and performs any necessary housekeeping or other duties as necessary.

## Functional Model versus Architectural Model

A good question at this stage is, “Why is it necessary to design a functional model and an architectural model?” We start by looking at any system—hardware, software, or a mix—it quickly becomes evident that the internal organization of a system is based on a collection of components and interconnections among them. An appropriate model has to include elements both at the functional level and at the architectural level to represent and evaluate hardware/software system.

### The Functional Model

The functional model describes a system through a set of interacting functional elements. The design proceeds at a high level without initial bias toward any specific implementation. It is often best described with a hierarchical and graphical model. The functional modules will interact using one of the following three types of relations:

- *The shared variable relation*—which defines a data exchange without temporal dependencies.

- *The synchronization relation*—which specifies temporal dependency.

- *The message transfer by port*—which implies a producer/consumer kind of relationship.

We will discuss each of these relations when we study processes and interprocess communication. All of them are critical in the design and development of today’s embedded systems.

### The Architectural Model

The architectural model describes the physical architecture of the system based on real components such as microprocessors, arrayed logics, special-purpose processors, analog and digital components, and the many interconnections between them.

### The Need for Both Models

These two views, when considered separately, are not sufficient to completely describe the design of contemporary systems. It is necessary to add the mapping between the functional viewpoint and the architectural one. Such a mapping defines a functional partition and the allocation of functional components to the hardware elements. This is also called architectural configuration.

The functional model, located between specification model and architectural model, is suitable for representing the internal organization of a system. It explains all necessary functions and the coupling between them, expressed from the point of view of the original problem. Using such a scheme leads to a technology-independent solution.

The functional model is the basis for a coarse-grain partitioning of the system. Such a partitioning leads naturally to the selection of which functions to implement in hardware or software. The architectural structure is fine grained and generally follows from the functional model; the architecture may also be imposed a priori.

## Prototyping

The prototype phase leads to an operational system prototype. A prototype implementation includes detailed design, debugging, validation, and testing.

Prototyping is naturally a bottom-up process because it consists of assembling individual parts and fleshing out more and more of the abstract functionalities. Each level of the implementation must be validated. That is, it must be checked for compliance with the specifications on the corresponding level in the top-down design.

Hardware and software implementations can be developed simultaneously and involve specialists in both domains, hopefully reducing the total implementation time. Often this does not happen in reality. Typically, the software leads hardware; nonetheless, a complete solution can be generated and/or synthesized for both hardware and software in the forms of ASIC and standard cores, etc., and software blocks. The resulting prototype can then be verified.

### Implementation

Activities in this step are highly dependent on the technology used. Remember, the prototype is a tool for understanding and confirming system design. It is a proof of concept. A word of caution: one should not rush the analysis or design to get to prototype. Also, one should not be afraid to throw the prototype away. For large projects, it is usually more of a rule to transform the prototype into the final product.

Those who hurry through the design and coding because a lot of testing needs to be done are going to be spending long nights debugging to work and even longer nights with unhappy customers. For some reason, customers do not have much of a sense of humor when the failure of a product they have purchased has just cost them several million dollars. If you are selling to a general market, your company has just lost several million R&D costs and you still do not have a product to take to market. So now, it is even worse, because you have missed an opportunity for sales revenue with a product that you cannot sell because it is poorly conceived, or it still is not ready.

### Analyzing the System Design

We have been studying the system design process while moving from requirements to a design. Now that the first-level design is in place, it must be critically analyzed. This step provides several important checks on the design. First and foremost, it verifies that the design meets the original requirements and specifications. At this stage in the design flow, it may also be necessary to trade off different architectural and functional aspects of the design.

#### Static Analysis

Static analysis should consider three areas:

1.  *Coupling.* We have examined this aspect of a design already. Coupling is related to the number and complexities of the relationships that exist among the various system modules. It also gives a measure of the implications of a change. The goal is loose coupling.

<!-- -->

2.  **Cohesiveness**

    Another issue that is worth stressing again is cohesiveness, which is a measure of the functional homogeneity of elements that comprise the modules. This applies to both the components and the relations. One must consider both external and internal views. External cohesion begins with the appropriate naming and meaning for elements. Internally, the structure and relationships among components is analyzed. For example, coupling through shared data is more cohesive than messages. Messages imply a temporal dependency.

3.  **Complexity**

    Two kinds of complexity are identified: *functional* and *behavioral*.

    Functional complexity is characterized by:

    - The number of internal functions and relational components. The goal is to keep these small. Generally, as the number of functions and relations decreases, so does the complexity of the design. Note: this does not mean to sacrifice clarity.

    - Interconnections among elements comprising each module. The earlier discussion of coupling applies here as well. Keep things simple.

    Behavioral complexity is characterized by:

    - The number of inputs and outputs. Once again, the target is a smaller number.

    - The length and ease of reading and understanding the description of the module. If several paragraphs or a page of written text in sub 6 point font are required to describe the function of one of the modules, that module is probably too complex. To simplify such descriptions, use tables, logical equations, or pseudocode.

    - The flow control through the module and the number and structure of state variables. Have a single major thread of control through the module and keep the number of states small.

### Dynamic Analysis

The objective in performing a dynamic analysis on the system is to determine how it will behave in a context that closely approximates the ultimate working environment. Dynamic analysis considers the following:

- **Behavior Verification.** The goal is to ensure that the behavior of the system, in its operating environment, meets the operational specification. That is, does it perform the functions it was intended to perform? This verification includes behavior at the boundaries of those functions. To be able to do so, of course, we need a good specification in the first place.

- **Performance Analysis.** Performance analysis ensures that the system, in its operating environment, meets the performance specification. The focus is on specific values for inputs and outputs. We’ll talk about this in a later chapter.

- **Trade-off Analysis.** A trade-off analysis is necessary to determine the optimal solution for the given constraints and objectives. Such an analysis, based on only a small set of performance criteria, may affect the ultimate success or failure of the product.
