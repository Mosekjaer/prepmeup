# SysML Language Overview

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Sanford Friedenthal, Alan Moore, Rich Steiner. *A Practical Guide to SysML*, Morgan Kaufmann, ISBN 978-0-123-74379-4. Chapter 3 SysML Language Overview, pp. 29–60.*

## SysML Purpose and Key Features

This chapter provides an overview of SysML by applying the language to the system design of an automobile introduced in Chapter 1. The example references the detailed language descriptions in Part II and the larger method examples in Part III.

SysML is a general-purpose graphical modeling language for the analysis, specification, design, verification, and validation of complex systems. These systems may include hardware, software, data, personnel, procedures, facilities, and other natural or man-made elements. SysML is intended to help specify and architect systems and their components, which may then be designed using other domain-specific languages such as UML for software and VHDL for hardware.

SysML can represent systems, components, and other entities in terms of:

- structural composition, interconnection, and classification;

- function-based, message-based, and state-based behavior;

- constraints on physical and performance properties;

- allocations between behavior, structure, and constraints;

- requirements and their relationships to other requirements, design elements, and test cases.

## SysML Diagram Overview

SysML includes nine diagrams. Each diagram type represents a particular view of the underlying model repository and constrains the kinds of elements and notation that may appear on that diagram. A diagram is therefore a view, not the complete model. Tabular views may also complement diagrams, for example allocation tables.

*Figur: SysML diagram taxonomy.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Requirement diagram  
represents text-based requirements and their relationship with other requirements, design elements, and test cases to support requirements traceability.

Activity diagram  
represents behavior in terms of the ordering of actions based on inputs, outputs, and control, and how actions transform inputs to outputs.

Sequence diagram  
represents behavior in terms of a sequence of messages exchanged between parts.

State machine diagram  
represents the behavior of an entity in terms of its states and transitions between states triggered by events.

Use case diagram  
represents functionality in terms of how a system or other entity is used by external actors to accomplish goals.

Block definition diagram  
represents structural elements called blocks and their composition and classification.

Internal block diagram  
represents interconnection and interfaces between the parts of a block.

Parametric diagram  
represents constraints on property values, such as $`F=m*a`$, used to support engineering analysis.

Package diagram  
represents the organization of a model in terms of packages that contain model elements.

## Using SysML in Support of MBSE

SysML provides a means to capture system modeling information as part of an MBSE approach without imposing a specific method. The selected method determines which activities are performed, their ordering, and the modeling artifacts used to represent the system. Structured analysis can decompose functions and allocate them to components. A use-case-driven approach can derive functionality from scenario analysis and associated interactions among parts.

A typical use of the language includes one or more iterations of activities to specify and design the system:

- capture and analyze black-box system requirements;

- capture text-based requirements in a requirements management tool;

- import requirements into the SysML modeling tool;

- identify top-level functionality in terms of system use cases;

- capture traceability between use cases and requirements;

- model use case scenarios as activity, sequence, and/or state machine diagrams;

- create the system context diagram;

- identify system test cases to support system verification;

- develop candidate system architectures to satisfy the requirements;

- decompose the system using block definition diagrams;

- define interactions among parts using activity or sequence diagrams;

- define interconnections among the parts using internal block diagrams;

- perform engineering and trade-off analysis using parametric diagrams;

- specify component requirements and trace them to the system requirements;

- verify that the system design satisfies requirements by executing system-level test cases.

Other systems engineering activities, including configuration management and risk management, are performed in conjunction with these modeling activities.

## A Simple Example Using SysML for an Automobile Design

The automobile example illustrates how SysML can be applied to specify and design a system. It includes at least one diagram for each SysML diagram type, but only highlights selected language features. The diagrams are representative of a typical model-based approach and their order may vary with the process and method used.

### Example Background and Scope

The example uses a simplified automobile design problem. A marketing analysis indicated a need to improve the automobile’s acceleration and fuel efficiency. The trade-off analysis considers alternative vehicle configurations, including 4-cylinder and 6-cylinder engines, to determine whether they satisfy the acceleration and fuel-efficiency requirements.

The proposed vehicle design also includes a controller and associated software to control the fuel-air mixture and maximize fuel efficiency and engine performance. Only selected aspects of the design are included to support the initial trade-off and demonstrate the language.

### Problem Summary

The automobile is modeled as a system with external users and environmental interactions. The driver commands the vehicle through controls, the vehicle interacts with the road and surrounding environment, and the engine and controller interact to produce propulsion. The example is intentionally small enough to fit in one chapter while still showing requirements, behavior, structure, parametrics, allocation, and packaging.

### Capturing the Automobile Specification in a Requirement Diagram

The automobile specification is captured with a requirement diagram. This gives the text requirements model identity and allows them to be related to other requirements, design elements, rationale, and test cases.

*Figur: Automobile model package organization.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Defining the Vehicle and Its External Environment Using a Block Definition Diagram

The block definition diagram establishes the main blocks in the automobile domain and the relationships among the vehicle, driver, road, and external environment.

*Figur: Automobile requirements.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Use Case Diagram for Operate Vehicle

Use cases capture the black-box services expected from the automobile. The driver is the primary actor. Other actors include a maintainer, fuel source, and external environment. The use cases provide an initial bridge from stakeholder goals to scenarios and test cases.

*Figur: Operate Vehicle use case diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Representing Drive Vehicle Behavior with a Sequence Diagram

The Drive Vehicle behavior can be represented by a sequence diagram showing the interaction between the driver and the vehicle over time.

*Figur: Control Power activity diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Referenced Sequence Diagram to Start Vehicle

The Start Vehicle interaction is referenced from the Drive Vehicle behavior. The referenced interaction keeps the higher-level scenario readable while allowing the lower-level start sequence to be modeled separately.

*Figur: Referenced Start Vehicle sequence diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Control Power Activity Diagram

Continuous behavior such as controlling power can be represented effectively with an activity diagram. Actions are partitioned between driver and vehicle responsibilities and connected by control and object flows.

*Figur: Drive Vehicle state machine diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### State Machine Diagram for Drive Vehicle States

The state machine diagram captures how the vehicle changes state in response to events such as starting, accelerating, braking, and shutting down.

### Vehicle Context Using an Internal Block Diagram

The vehicle context diagram identifies external actors and environmental blocks that interact with the vehicle. It helps define the system boundary and external interfaces.

### Vehicle Hierarchy Represented on a Block Definition Diagram

Block definition diagrams define the structural vocabulary for the model. The automobile is a block composed of other blocks such as engine, transmission, controller, fuel system, wheels, and driver interface. Generalization and composition relationships capture classification and part-whole structure.

*Figur: Vehicle hierarchy block definition diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Activity Diagram for Provide Power

The Provide Power activity refines the power behavior and identifies how inputs, outputs, and control move through the functional decomposition.

### Internal Block Diagram for the Power Subsystem

Internal block diagrams show how the parts of a block are interconnected and how items flow through connectors. For the automobile, the controller communicates with the engine and receives driver commands. The engine provides torque through the transmission to the wheels.

*Figur: Power subsystem internal block diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Defining the Equations to Analyze Vehicle Performance

Parametric diagrams constrain values of properties and connect those values to engineering equations. The automobile example uses constraints for acceleration, mass, force, power, and fuel efficiency to compare candidate architectures.

*Figur: Vehicle acceleration analysis parametric diagram.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Analyzing Vehicle Acceleration Using the Parametric Diagram

The acceleration analysis binds vehicle properties to the parameters of constraint blocks. This supports evaluation of candidate engine and drivetrain configurations.

### Analysis Results from Analyzing Vehicle Acceleration

Analysis results are used to compare the alternatives against the acceleration and fuel-efficiency requirements. The result is not just a numeric value; it becomes model information that supports design decisions.

### Using the Vehicle Controller to Optimize Engine Performance

The vehicle controller and associated software are included to optimize engine performance by controlling fuel-air mixture and related engine behavior.

### Specifying the Vehicle and Its Components

Allocations establish relationships among requirements, behavior, structure, and constraints. They can show that a function is allocated to a component, that a constraint is allocated to a physical property, or that a test verifies a requirement.

*Figur: Vehicle and component allocation relationships.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Requirements Traceability

The example emphasizes end-to-end traceability. Stakeholder requirements are refined into system requirements, system requirements are satisfied by design blocks and constraints, and tests verify the requirements. Traceability supports impact analysis when a requirement, design decision, or test changes.

*Figur: Requirement diagram showing the traceability of the Max Acceleration requirement.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Package Diagram for Organizing the Model

The model organization is captured using a package diagram. It shows how model elements are grouped into packages and how elements in one package may relate to elements in another package.

*Figur: Candidate architecture trade-off.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

*Figur: Test cases verify requirements.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Model Interchange

A SysML model captured in a model repository can be imported and exported from a SysML-compliant tool in the standard XML metadata interchange (XMI) format. This enables other tools to exchange the information if they also support XMI. Examples include exporting selected parts of the SysML model to a UML tool for controller software development, importing or exporting requirements from a requirements management tool, and exchanging parametric diagrams and related information with engineering analysis tools. Seamless interchange depends on model quality and tool implementation, but this capability continues to improve.

## Summary

SysML is a general-purpose graphical language for modeling systems that may include hardware, software, data, people, facilities, and other elements of the physical environment. The language supports modeling requirements, structure, behavior, and parametrics to provide a robust description of a system, its components, and its environment.

The semantics of the language enable a modeler to develop an integrated model where model elements on one diagram can be related to elements on other diagrams. The diagrams enable capturing and viewing information in the model repository to help specify, design, analyze, and verify systems. Repository information can be imported and exported via XMI and other exchange mechanisms.

The SysML language is a critical enabler of MBSE and can be used with a variety of processes and methods. Effective use still requires a well-defined MBSE method. The automobile example illustrates one such method, and the book includes other examples in Part III.

## Questions

1.  What are some of the aspects of a system that SysML can represent?

2.  What is a requirement diagram used for?

3.  What is an activity diagram used for?

4.  What is a sequence diagram used for?

5.  What is a state machine diagram used for?

6.  What is a use case diagram used for?

7.  What is the primary unit of structure in SysML?

8.  What is the block definition diagram used for?

9.  What is an internal block diagram used for?

10. What is a parametric diagram used for?

11. What is a package diagram used for?
