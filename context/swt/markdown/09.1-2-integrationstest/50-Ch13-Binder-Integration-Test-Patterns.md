---
title: "Ch13-Binder Integration Test Patterns"
source: "csfiles/home_dir/BB-Cou-UUVA-69276/BB-Cou-UUVA-65748/BB-Cou-UUVA-61887/BB-Cou-STADS-UUVA-52347/NOTES(3)/Ch13-Binder Integration Test Patterns.pdf"
modul: "Lektion 09.1+2: Integrationstest"
pages: 45
type: "dokument"
vision: "n/a"
---
# Ch13-Binder Integration Test Patterns

<!-- side 1 -->

Chapter
      13 Integration




     Overview
      Integration test design is concerned with several primary. questions.
     Which components and interfaces should be exercised? In what se-
     quence? WhiChtest design patterns are appropriate? This chgpter be-
     gins by considering the role of integration testing in object-oriented
     development. Next, a simple dependency analysis technique and con.
     siderations for testing at $everal scopEls are presented. Nine integro"-
     tion test patterns provide design and execution strategies.




13.1 Integrationin Objert-oriented
                                 Development
13.1.1 Definitions
A system is eomposed of eomponents. Systems of software eomponents ean be
defined at any physieal seope. A eomponent is itself a system of smaller com-
ponents, as suggested by Table 13.1. For example, a dass is a system whose
eomponents are methods and server objeets, a dus ter is a system of dasses, an
applieation system is eomposed of exeeutables, and so on. In this ehapter, sys-
tem and component are used in this generie sense.

   Remarks obout o system or o eomponent should be understood to apply at ony
   scope, unless a portieulor scope is explicitly stated.




                                                                                627

<!-- side 2 -->

628                                                                                               PART
                                                                                                     III
                                                                                                                                                                                                       629

TABLE 13.1     Scope versus Focus of Integration          Testing
                                                                                                         .-    program, or external softWare system that applies test cases to the component
 Component                     System                               Typicallntercomponent   Interfaces         under test (see Section 19.4, Driver Patterns).
 (Focus   øf Integration)      (Scope   of Integration)             (locus øf Integration Faults)                  Sometimes, the process of selecting and merging specific versions of source
 Method                        Class                                Instance variables                         and binary files for translation and linking within a component and among
                                                                    Intraclass messages                        components is called "integration." Here, this activity is referred to as build
 Class                         Cluster                              Interclass   messages                      generation. A build is the executable(s) produced by a build generation process.
 Cluster                       Subsystem                            Interclass messages                        Build generation is a necessary precursor to integration testing and may include
                                                                    Interpackage   messages                    smoke tests. It is supported by configuration management planning, tools, and
 Subsystem                     System                               Interprocess communication                 practices. However, it is not integration testing.
                                                                    Remote procedure call
                                                                    ORB services
                                                                    OS services
                                                                                                               13.1.2 Integration
                                                                                                                                Testing
                                                                                                                                      IsEssential
                                                                                                                SoftWare systems are built with components that must interoperate. Three
 With this abstraction, integration patterns can be applied at many scopes. This                                basic kinds of testing are needed to show that components are minimally inter-
concept is crucial for effective integration test design.                                                      operable: tests of individual components, tests of the system resulting from the
     A stable system has sufficient dependability to support system scope test-                                federation of components, and tests of component interoperation. Integration
ing. The threshold for stability usually reflects practical considerations-it                                  testing is a search for component faults that cause intercomponent failures. An
                                                                                                               integration strategy must answer three questions:
varies from project to project and often changes within a project. An increment
is a subset of system components that typically (but not necessarily) has a cor-
responding subset of requirements. Increments are typically progressive; the                                       ··   Which components are the focus of the integration test?
second increment includes and depends on the first, and so on. The last incre-
ment becomes the delivered (shipped) system. A delta is a change to component
within the scope of an increment.
                                                                                                                   ·    In what sequence will component interfaces be exercised?
                                                                                                                        Which test design technique should be used to exercise each
                                                                                                                        interface?
     A smoke test is designed to pass if the implementation under test can be run
at all. Expected results are not specified for a smoke test-a no pass occurs if                                  In contrast, component seope testing is a seareh for intracomponent faults,
the lUT crashes, and the system passes if any other result is obtained. A test                                and system scope testing is a search for faults that lead to a failure to meet a
configuration is a defined and versioned set of drivers, stubs, and a build of the                             system scope responsibility. Most interoperability faults are not revealed by
system under test. The activities that begin and end with the development and                                  testing a component in isolation, so integration testing is necessary. System
execution of a test configuration are a stage. Integration coverage is achieved                                scope testing cannot be done unless components interoperate sufficiently well
when the testing meets the exit criteria for an integration pattern.                                           to exercise system scope responsibilities. The prima ry purpose of integration
     A call path is a sequence of messages, function calls, remote procedure calls,                           testing is to reveal component interoperability faults so that testing at system
interprocess communication messages, and so on, which is possible as a result                                 scope may proceed with the fewest possible interruptions.
of intercomponent interfaces. A eall trace is an aetual set of ealls made along                                    Testing theory explains why integration testing is necessary.l According to
some call path. A eall pair refers to a client and server that have a eall betWeen                            the antidecomposition axiom, a test suite that achieves coverage at system scope
them.
    A stub is a partial implementation of a eomponent (see Section 19.3, Test
Control Patterns). A proxy is a eomponent that provides an interface but sim-                                 1. These are two of the testing axioms developed in [Weyuker 88]. An axiom is a trusted
ply delegates all ineoming messages to another object. A driver is a class, main                              basic principle. An adequate test suite achieves at least statement coverage. These axioms
                                                                                                              were reinterpreted for object-oriented code in [Perry+90]. See Testing Axioms, Chapter 10.

<!-- side 3 -->

                                                                                         Integration                                                                               631
630

                                                                                      mated test suites. Some integration patterns require a stub or driver for every
does not necessarily result in the same cover age for its components (i.e., server
                                                                                      component to be integrated, adding to the items that must be induded under
objects and called modules). Testing at system scope cannot guarantee that            version control.
components have been covered. For example, achieving statement coverage of                Experience in object-oriented development has shown that these strategies
a main() program can also achieve statement cover age of its components only          are still applicable, but instead of one technique dominating, an edeetic ap-
when there is no branching, iteration, or dynamic binding in any of its compo-
                                                                                      proach is necessary Uacobson+92, Jlittner+94a, jiittner+94b, Arnold+94,
nents. According to the anticomposition axiom, coverage at system scope is not
                                                                                      Murphy+94, Bosivert 97]. Integration testing is a process in object-ariented
necessarily achieved by rerunning component test suites that have achieved
                                                                                      development, rather than an event or phase. Barbey argues that all farms of
component coverage. That is, adequate testing of components is not equivalent
                                                                                      object-oriented testing involve integration testing:
to adequate testing of a system of components. For example, suppose statement
cover age of all components called by mainO and all statements in mainO can               [Because] methods are not efficient basic test units, and the structure of object ori-
be achieved by taking one of two paths within mainO. Although all compo-                  ented systems does not allow a recursive decomposition that takes all behavioral as-
nents have been covered, mainO indudes an untested path. Testing that is ad-              pects into account, there is no basic unit testing for object oriented software, but
equate for a system of components therefore requires testing at component                 there are many kinds of integration unit testing [Barbey 97, 74].

 scope, component interaction scope, and system scope.
      Distinct but complementary strategies for build generation and integration          Thus integration testing in object-oriented development begins early, takes
 testing have been used since the early 1970s. They follow the physical structure     place at all scopes, and is repeated in each development increment:
 of the system under test: module call paths, interprocess communication, or job
                                                                                          . Within a dass
 streams, and support incremental development (see the Bibliographic Notes see-
 tion for a brief survey). A key lesson learned is that incremental integration is        ..    Within a dass hierarchy
 the most effective technique: add components a few at a time and then test their
 interoperability. Trying to integrate most or all of the components in a system
  at the same time is usually problematic.
                                                                                           ..   Betweena dient and its servers
                                                                                                Within a duster of related dasses


      ..   Debugging is difficult because the bug may be in any interface.
                                                                                          ·     Within a subsystem
                                                                                                Within an application system

           Last-minute patches are often necessary, resulting in low-quality              Testing of a dass integrates its methods. Testing of a subdass integrates the

      .    fixes that may not undergo adequate testing.
           The testing is usually not systematic, so interface bugs can escape
           detection.
                                                                                      superdass methods. Testing of a duster integrates participants in its collabora-
                                                                                      Hons. Clearly, each development increment requires integration of the compo-
                                                                                      nents in the scope of the increment. Figure 13.1 gives an overview of integration
                                                                                      patterns by scope.
     In contrast, incremental testing has several advantages. Interfaces are sys-         Integration testing is dosely tied to system architecture and to the pro-
 tematically exercised and shown to be stable before unproven interfaces are ex-      cess used for development. The architecture organizes a large system into man-
 ercised. Buggy interface code will be reached, if not triggered. Observed failures   ageable components and subsystems. Beyond technical considerations, it serves
 are most likely to come from the most recently added component, so debugging         many purposes, induding risk management, scheduling, and verification.
 is more efficient. Although conceptually simple, incremental development re.         Arehitecture and process have a symbiotic relationship as well. For example,
 quires discipline and commitment. A sequence of components must be identified        parallel development becomes easier when partitioning reduces interface eou-
 using careful analysis of component dependencies. Next, testing must be              pling. Booch argues that integration is central to object-oriented development.
 planned and managed to follow these dependencies. Automated configuration            "The macro process of object-oriented development is one of continuous inte-
 management support for build generation is necessary, as are repeatable, autO-       gration. . . . At regular intervals, the process of continuous integration yields

<!-- side 4 -->

632                                                                                                               PART
                                                                                                                     11/                                                                                                    633
                                                                                                                               Pattenj,


                                                                                Integmtion Stmtegy                                         executable releases that grow in functionality at every release . . . . It is through
 Scope of               Typical        Typical                      Integmtion Pattems
                                                                                                                                           these milestones that management can measure progress and quality, and hence
 Implementation         System         Components                                      Integration                                         anticipate, identify, and then actively attack risks on an ongoing basis" [Booch
 Under Test             Responsibility to Be                     B B T C C L S M H Covemge
                        Model          Integmted                 B U D L K Y T S F Model                                                   96,75].
 (the .System")
                                                                                                                                               Because integration test plans are typically based on intercomponent im-
 Cluster                Mode
                        machine
                                         Root client and
                                         its serve rs,
                                         upper-bounded
                                                                 ,/ ,/ ,/ ,/
                                                                                                        Diagram,
                                                                                                        ClIaboration
                                                                                                        M Tree
                                                                                                                        I   CaUpaths
                                                                                                                            M+      .     plementation dependencies, analysis of the component architecrure is required.
                                                                                                                                          If this architecture is not specified, then the integration test design must wait for
 Facade             I Extended                                        ,/       ,/ ,/
                      contra et                                                                                                           completion of some (or, in the worst case, all) of the components. The process
                                         build group                                                 ,/ Collaboration
                                                                                                        Diagram         I   N+of Mt.
                                                                                                                            facade
                      (facade),                                                                         Round-trip                ,       of integration test design often reveals errors, omissions, and ambiguities in the
                      mode           I   Classes in                                                     Scenario test
                      machine
                                                                                                                                          requirements and architecture-another        reason to begin architectural design as
                                                                                                                                          soon as possible and to make testability a design goa!.
                                         Classes in
 component with contraet,                build group,                                                   Scenarro test       M+,SD              Iterative, incremental object-oriented development increases the leverage
 APl (devel-     XUC/SD                  components,                                                    Round-rip       I   N+(APl),
                                                                                                                            branch        of integration testing. Poorly planned integration testing will be costly and in-
 Binarynot
 oped,         I Extended                VM
 reused)                                                                                                                                  effective, while timely and focused integration testing will yield high efficiency.
 Build group,           XUC/SD,          Classes in                                                                                       An integration test plan mus t answer several basic questions:
 executable with
 IPC or UI inter-
 face
                        mode
                        machine

                        XUC/SD,
                                         build group,
                                         components,
                                         VM,HW
                                         Constituent
                                                                                                     ,/ Scenario


                                                                                                        Round-trip
                                                                                                                         SDbrancb
                                                                                                        Round-triptest I M+     ·
                                                                                                                                                ··   What are the major and minor development increments?
                                                                                                                                                     Which component interfaces will be the focus of integration? Which
 Process group



                                                                                                                                              ···
 using IPC, UI          mode             processes,                                                     Scenario test                                subsystems?
                        machine          components,
                                         other applica-                                                                                              In what sequence will components and their interfaces be exercised?
                                         tions, VM, HW
                                                                                                                                                     Which stubs and drivers must be developed?
 "Component"      XUC/SD                 Binary com-                                                    Round-trip
                                                                                                                                                     Which test design patterns should be used?
 application with
 UI
 Clientlserver          XUC/SD
                                         ponents with
                                         APl
                                         Client applica-
                                                                                                        Scenario test

                                                                                                        Round-trip
                                                                                                                                              ·      When can integration testing be considered complete?
                                         tions and plat-                                                Scenario test
                                         forms, server                                                                                         Flexibility is important because a component may not be ready at a time
                                         applications                                                                                     that coincides with a strictly technical order of integration. In any event, the
                                         and platforms,
                                         middleware,                                                                                      integration test patterns should be tailored to suit the requirements and con-
                                         and platforms                                                                                    straints of a particular project. Key considerations in the design of an integra-
 Reactivel                                                   I    I        I     1     1,/1,/1,/1   1,/1 M Tree         1 M+              tion test plan include the folIowing:
 embedded               Mode
                        machine          Real-time
                                         kernel, pro-

                                                                                                                                             ··
                    I                I

                                         cesses, hard-
                                         ware                                                                                                   Scope of an increment and dependencies of the components.
 Wide area
 network            I   XUC/SD       I   Node and
                                         tions  applica-
                                                    plat-
                                         forms, middle-
                                                                                                        Round-trip
                                                                                                        Scenario test                           Testability. As the number of components increases, it becomes
                                                                                                                                                harder to control and observe an individual component. How can
                                         ware and
                                         platforms

Key:XUC = Extended Use Case (see Chapter 14); SD = Sequence Diagram; N+ = Conformanee + Sneak Path Coveraga;
                                                                                                                                             ·  we balance decreasing testability with increasing scope?
                                                                                                                                                Component stability. Table 13.2 offers a volatility spectrum. It usu-
                                                                                                                                                ally makes sense to postpone integration of highly volatile compo-
Class Stale Model; M+ = Conformance + Sneak Path Coverage. Mode Machine Model; HW = Hardware

FIGURE 13.1      Overview    of integration      strategy.                                                                                   ·  nents and begin with stable components.
                                                                                                                                                Test environment stability.

<!-- side 5 -->

634
                                                                                                                                                                                                     635


TABLE13.2 Relative Component                          Volatility
                                                                                                                     sages, perform remote procedure calls, or use interprocess communication ser-
IC':'::';'",,:':"U         >~>'            ,» »,~,~    »,"        »
                                                                                       i;1~iiVeVolQ;ili!y~!          vices. In addition, many kinds of implicit dependencies arise: communication
<;:~W~,,~rrtT~                                                                           WI ~ ~.,.",.. la -. 'MOl!
                                                                                                                     through persistent storage, sequential activation constraints, timing constraints,
                                                                                       Low
 Objed               representing   a real-world             entity (e.g., Customer)                                 and so on. Integration testing does not focus on implicit dependencies. Instead,
                                                                                       Low
 Long-lived              information   strudures                                                                     it concentrates on explicit dependencies to show minimal interoperability.
                                                                                       Medium
 Get-able              object attributes                                                                             Explicit dependencies often dictate the sequence of testing.
                                                                                       Medium
 Sequences of behavior (state model)                                                                                     Most approaches to duster scope integration use dependency analysis to
 Interface to target environment                                                       High
                                                                                                                     support bottom-up testing. Explicit intercomponent dependencies typically cor-
 Class functionality                                                                   High
                                                                                                                     respond to interfaces that should be exercised by an integration test suite. A
 Source: Adapted from [Jacobson+92,                          76 ffl.                                                 model of explicit dependencies can be used to plan a testing sequence. For ex-
                                                                                                                     ample, the dass diagram for the F;nane;alServ;ee     duster (Figure 13.2) shows

    This chapter provides analytical tools to solve the technical dimension of
integration test design. The corresponding resource allocation and control
problems are discussed in Managing the Testing Process for Object-Oriented
Systems [Binder 2000].                                                                                                                                                   Has Uniaue   .    AeetNum



13.1.3                Dependency
                              Anolysis
Components typically depend on each other in many ways. Dependencies are                                                                                  Aeeount

necessary to implement collaborations and achieve separation of concerns.                                                        Has.         0..*
Some dependencies are accidental or unavoidable side effects of a particular
implementation, programming language, or target environment. Class and dus-                                                                                      2..*
ter scope dependencies result from explicit binding mechanisms, induding the                                                                                                              Uses
                                                                                                                                                                    Applied lo
folIowing:

           .       Composition and aggregation (the use of dasses to define instance                                                                   Transaet;on                         Money
           ..      variables)
                   Inheritance                                                                                                      Providedas   .
                                                                                                                                              0..*                               Uses



            ..     Global variables
                   Calls to an APl                                                                                                                        0..*
                                                                                                                                                 In Effect for


             ..    Server objects (instance
                   Objects used as message parameters
                                                             variables   or proxies)
                                                                                                                                                                 ... Posledfor
                                                                                                                                                           ---'--1..1

              ..   Pointers to objects used as message parameters
                   Type parameters given for dedarations of generic dasses
                   Static and dynamic name scoping
                                                                                                                                                           Rates




      Similar intercomponent dependencies occur at subsystem and system                                              .2 Closs Diagram, F; nane; al Sery; ee cluster.
  scopes. Components explicitly depend on each other when they exchange mes-

<!-- side 6 -->

                                                                                           PART11/
                                                                                                                                                                                              637
636


some basic dependency information, but an order of testing is not immediately
apparent. Suppose we have the folIowing C++ dasses:

      class FinancialService       {
           1* Represents a financial        service       including   several   accounts    */};

      class Transaction       {
           1* Represents     a service      transaction       */};
      class Account {
           1* Account */};
      class Rates {
           1* Exchange     Rates    in effect    */};
                                                                                                        Rates                           Money                               AcctNum
      class Money {
           1* Basic data     type    */};

      class Array {
           1* Generic class        used to contain         rate s */};

      class AccountNum {
           1* Basic data type        */};                                                            Array[Int]


     Clearly, we must use Transaction and Account objects (or stubs) to test
Fi nanci al Servi ce, but what about Moneyand Rate? Suppose the usage of the
generic dass Array was not given on the Class Diagram because it was consid-                         3.3 Dependency tree for the Fi nanci a l Se rvi ee cluster.
ered an "implementation detail."
     Figure 13.3 shows the dependency tree for the Fi nanci al Servi ce duster.                        encies corresponded to remote procedure calls. The general dependencies and
The diagram does not depict an inheritance hierarchy or a single message in-                           the tree would be the same.
terface. Instead, an arrow represents a dient-uses-server relationship. The root                           The output of compilers, linkers, and loaders often provides a good deal of
of the dependency graph is a dient dass that is not used by any other dasses in                        dependency information. Dependency analyzers are commercially available for
the duster under test. In practice, there may be several roots. Leaf das ses do not                    many languages and target platforms. Note, however, that many of these tools
use any other dass in the duster under test. They would be tested first in a                           cannot analyze dependencies that traverse network nodes, virtual maehines,
bottom-up integration. The root is level O.                                                            and operating systems.
     With a small duster, a dependency tree can be developed by inspection.                                The Unix tsort command-line utility will produee a topological sort of
Alternatively, the dependency tree can be derived from a topological sort of de-                       components. This ubiquitous utility can be easily used to produce a depend-
pendency relationships. Formally, a topological sort is an ordering of a directed                      eney analysis.2 Table 13.3 indicates how tsort can be used to develop the
graph such that all predecessors of every node are listed before the node itself.
That is, no item appears until all of its predecessors have been listed. The topo-                    2. The tsort   utility was developed to check forward and self-referent declarations, be-
 logical sort can be applied at any scope. Suppose that the preceding components                      cause C language processors will reject these dependencies. For detaiIs, do man tsort    on
                                                                                                      your Unix platform. If you do not have access to a Unix machine, try a Web search for
 were subsystems implemented as Unix processes, each of which runs on a dif-
                                                                                                      "topological sort." You will probably find several shareware implementations that will run
 ferent processor. Instead of translation-time binding, suppose that the depend-                      on your platform. Failing this, the same search will probably yield several algorithms that
                                                                                                      you ean code.

<!-- side 7 -->

638                                                                                                                                                                            639

TABLE13.3    Using   tsort for Root-firstDependency Analysis                                       The second part of Table 13.3 shows how multiple root dusters are han-
                               "                ,~         '


                       tsort       Input file                  tsort   Output                  dled. Suppose we add a dass Customer. Customer now appears before any
                                                               Root First                      dasses that use it. The dependeney graph is produeed in the same manner.
Relationship           X Uses y
                                                               FinancialService
                                                                                                   A eyde is a loop in a direeted graph. For example, suppose dass A uses dass
Simple Case            FinancialService      Transaction
                                                               Account                         B, B uses C, and C uses A. Consequently, one dass eannot be exereised with-
                       FinancialService      Account
                       Transaction     Rates                   Transaction                     out the other two. The topologieal sort relation is not defined for a eyde. Cydes
                       Transaction     Money                   AccountNum
                                                                                               are identified by tsort, but not sorted, as shown in the lower part of Table
                       Account Money                           Money
                       Account AccountNum                      Rates                           13.3. Partieipants of a eyde must be tested as a group or deeoupled with a
                       Rates Array                             Array                           eyde-breaking stub.
                       FinancialService      Transaction       Customer                            Figure 13.4 shows the dependeney leveIs in FinancialService.        A level ean
Multiple    Roots                                              FinancialService
                       FinancialService      Account                                           be eonsidered as the root of a subtree. That is, the root and all of its dependent
                       Transaction     Rates                   Account
                       Transaction     Money                   Transaction                     eomponents may be tested as a unit. This information is essential for develop-
                       Account Money                           AccountNum                      ing sequenee of integration.
                       Account AccountNum                      Money
                       Rates Array                             Rates
                       Customer Customer                       Array
                       FinancialService      Transaction       FinancialService                                                                 level 2; Simple Uses
 Uses Cycle                                                    Transaction
                       FinancialService      Account
                       Transaction     Rates                   Rates
                       Transaction     Money                   Array
                       Account Money                           tsort:    cycle in data
                       Account AccountNum                      tsort:    Account
                       Customer Account                        tsort:    Money
                       Money Customer                          tsort:    Customer
                       Rates Array                             Customer
                                                               Account
                                                               AccountNum
                                                                Money



                                                                                                                                                  level O;Clusler Head
 dependeney tree for the Fi nanci al Servi ce duster. First, eaeh pair of de-
 pendeneies is identified. The dependeneies are given the form X uses Y. The
 Fi nanci al Serv; ce uses Transacti on dependency is given in the input file as
 Fi nanc; alServi ce Transaction. Running tsort gives the output on the right
 of Table 13.3. As expected, the root dass appears first. The remaining dasses
 are then listed in uses order. To transeribe this list to a tree, use the fjrst dass as
 root, and draw an edge for each dependeney on it. Add nodes for the depend-
 ent dasses. Next, draw edges for eaeh dependeney on these dasses. Add nodes
 for the dasses at this level. Fan-in is likely, as one dass may be used by several
 others. Continue traeing dasses and dependencies until all dasses in the sorted
 list have been transeribed.
                                                                                           13.4 Dependency leveIs in the F; nanci a l Se ry; ce cluster.

<!-- side 8 -->

640                                                                                                                                                                      641


TABlE13.4 Using tsort   for leaf-first Dependency Analysis                             nearly two-thirds were interface-related; that is, a function call or its parame-
                               .n~~..   ~   "Z_:0:0,




                   tsort Inputfile                           tsort Output              ters had a design or coding bug [Perry+87]. Typical interface bugs include the
                                                                                       folIowing:
Relationship       X Is-used-byY                             Lea!First
Simple Case        Transaction FinancialService
                   Account FinancialService
                                                             Array
                                                             AccountNum
                                                                                                ·   Configurationlversion control problems.
                   Rates Transaction
                   Money Transaction
                   Money Account
                                                             Money
                                                             Rates
                                                             Account
                                                                                                ·
                                                                                           · Missing, overlapping,         or conflicting   functions.
                                                                                                    An incorrect or inconsistent data structure used for a file or
                                                                                                    database.


                                                                                              ···
                   AccountNumAccount                         Transaction
                   Array Rates                               FinancialService                       Conflicting data views/usage used for a file or database.
Multiple Roots     Transaction FinancialService              Array                                  Violations of the data integrity of a global store or database.
                   Account FinancialService                  Customer
                   Rates Transaction                         AccountNum                             The wrong method called due to coding error or unexpected runtime
                   Money Transaction
                   Money Account
                   AccountNumAccount
                   Array Rates
                                                             Money
                                                             Rates
                                                             Account
                                                             Transaction
                                                                                             ··     binding.
                                                                                                The cIient sending a message that violates the server's preconditions.
                                                                                                The cIient sending a message that violates the server's sequential
                   Customer Customer                         Fi nanci al Servi ce               constraints.
Uses CyeJe         Transaction FinancialService
                   Account FinancialService
                   Rates Transaction
                                                             Array
                                                             AccountNum
                                                             Rates                         ···  Wrong object bound to message (polymorphic target).
                                                                                                Wrong parameters, or incorrect parameter values.



                                                                                          ··
                   Money Transaction                         tsort: cycle in data               Failures due to incorrect memory management allocationldeallocation.
                   Money Account                             tsort: Money
                   AccountNumAccount                         tsort: Account                     Incorrect usage of virtual machine, ORB, or OS services.
                   Customer Customer                         tsort: Customer                    Attempt by the lUT to use target environment services that are obsolete
                   Account Customer                          Customer
                                                             Money                              or not upward-compatible for the specified versionlrelease of the target
                   Customer Money                                                               environment.
                   Array Rates                               Account
                                                             Transaction
                                                             FinancialService
                                                                                          ·     Attempt by the lUT to use new target environment services that
                                                                                                are not supported in the current versionlrelease of the target
                                                                                                environment.
                                                                                          · Intercomponent conflicts: thread X will crash when process Y is run-
    If dependencies are modeled as X is-used-by Y (instead of X uses Y), tsort
will output the cIass list in the opposite order, as shown in Table 13.4. The is-
used-by form approximates the sequential order of testing: classes with no de-
                                                                                          ·     ning, for example.
                                                                                               Resource contention: the target environment cannot allocate resources
                                                                                               required for a nominal load. For example, a use case may op en up to
pendencies are listed first; the root cIass is listed last. Note that we may obtain            six windows, but the lUT crashes when the fifth is opened.
the tsart input for a leaf-first sequence by swapping the names in each de-
pendency pair. Cycles and multiple roots are handled in the same manner.                  Because many capabilities require the cooperation of several components,
                                                                                      some people argue that all bugs that appear at system scope are integration
                                                                                      bugs. I take a narrower view, which corresponds to the focus of integration as
13.1.4 Integration
                 Faults                                                               a dress rehearsal for system testing.
Integration testing can reveal component faults that cause failures when com-
ponents interact. For example, a study of faults in a large C system found that

<!-- side 9 -->

642
                                                                                                                                                                        643

13.2 IntegrationPatterns

This section presents nine integration test suite test design patterns:

      ·   Big Bang Integration. Attempt to demonstrate system stability by test-

      ·   ing all components at the same time.
          Bottom-up Integration. Interleave component and integration testing by

      ·   folIowing usage dependencies.
          Top-down Integration. Interleave component and integration testing by

    ·     folIowing the application control hierarchy.
          Col/aboration Integration. Choose an order of integration according to
          collaborations and their dependencies. Exercise interfaces by testing one
          collaboration at a time.
    ·     Backbone Integration. Combine Top-down Integration, Bottom-up
          Integration, and Big Bang Integration to reach a stable system that

    ·     will support iterative development.
          Layer Integration. Incrementally exercise interfaces and components in           135
                                                                                      FIGURE      Generic dependency tree.

    ·     a layered architecture.
          Client/Seroer Integration. Exercise a loosely coupled network of com-

    ·     ponents, all of which use a common server component.
          Distributed Seroices Integration. Exercise a loosely coupled network of
          peer-to-peer components.
                                                                                      13.2.1 Scope-specific
                                                                                      (lasses
                                                                                                         Considerotions


    ·     High-frequency Integration. Develop and rerun an integration test suite
          test hourly, daily, or weekly.
                                                                                      At class scope, the system is the class under test. The components to be inte-
                                                                                      grated are CUT methods, superclass methods, instance variables, and parame-
                                                                                      ters in messages received and sent by the CUT. Testing of class responsibilities
Figure 13.5 shows a generic dependency tree that will be used to show succes-         is so closely coupled to class integration that it does not make much sense to
sive configurations.                                                                  treat the two as separate test design problems.
    The integration patterns are mostly independent of scope. Each is a gen-
eral approach indicated when certain relationships hold among components.
Some are better suited to a particular kind of architecture than others, however.
                                                                                         ·   Bottom-up intraclass integration is accomplished by the alpha-omega
                                                                                             incremental developmentlintegration strategy outlined in Chapter 10.
For example, Bottom-up Integration works well for a class cluster or small                   Briefly, methods are coded and tested roughly in an order based on de-
subsystem. Client/Seroer Integration and Distributed Seroices Integration                    pendency: constructors, accessors, predicates, modifiers, iterators, and
are appropriate for a larger system of components and distributed objects. Fig-              finally destructors.
ure 13.1 on page 632 summarizes the relationship between scope and integra-
tion patterns.
                                                                                         ·   Big Bang intraclass integration is accomplished by the Small Pop
                                                                                             developmentlintegration strategy outlined in Chapter 10. Briefly, all
                                                                                             methods are coded and testing begins with the appropriate class scope
                                                                                             pattern.

<!-- side 10 -->

644                                                                          PART11/       Integration                                                                          645


      .   Collaboration integration of a modal class may be accomplished with               . If the class under test catches exceptions, Controlled Exception Test

      .   Modal Class Test.
          Integration of superclass features can be accomplished with Poly-                 · may be useful.
                                                                                              There is no direct analog to Client/Seroer Integration, Distributed
                                                                                              Services Integration, or Backbone Integration integration at the cluster
      .   morphic Server Test and Modal Hierarchy Test.
          There is no direct analog to Backbone Integration, Layer Integration,
          Client/Server Integration, or Distributed Services Integration at class           · scope.
                                                                                              Overall, Big Bang Integration is not recommended unless just a few
          scope.                                                                              new components are being added to a stable system, as is the case in
                                                                                              maintenance. This pattern is useful in some limited situations for
    Attempting to test a class in isolation is often an exercise in futility. To ac-          development.
complish complete isolation testing would require that we replace all servers of
the class under test with controllable stubs. This course of action turns out to            Procedure 15 summarizes steps for cluster integration.
be too difficult [Siepmann+94, ]iittner+95, Firesmith 96, Dorman 97]. Even if
we implement the stubs, orchestrating the necessary response from the set of           Subsystem/SystemScope
stubs for each test case is difficult at best. As a result, most class testing takes   The scope of an implementation under test is often larger than of a single class
place in a cluster of classes. Nevertheless, responsibility-based test design may      or small cluster. We should test interactions above cluster scope but below sys-
focus on one class at a time, even though the class is embedded in a cluster.          tem scope.

Clusters

At cluster scope, the system is the cluster under test. The components to be
                                                                                           ·    Incremental   development    requires that successively    larger sets of func-
                                                                                                tionality be integrated. Increments may correspond to one subsystem or
integrated are server objects used by the CUT. The testing of cluster respons i-
bilities is less coupled to integration, so it makes sense to consider an integra-
tion strategy to be a master plan for the orchestration of pattern-based
                                                                                           ·    multiple subsystems.
                                                                                                Stand-alone subsystems present for historicai or commercial reasons
                                                                                                must often be integrated with newly developed object-oriented sub-
responsibility tests. "In general, it is not possible to do the integration testing             systems.
according to only one of [inheritance, aggregation, and message passing] with-
out a lot of tedious stubbing. Therefore, one needs a flexible mixed strategy"              At subsystem and system scopes, the goal of integration testing is achieving
[Jiittner 94c, 13].                                                                    a sufficiently stable system so that responsibility testing at system scope may
                                                                                       proceed smoothly. System and subsystem integration testing should verify the
      . Bottom-up Integration is the most widely used technique.
      .   Top-down Integration is possible if one class is the head of the cluster.
          For example, a cluster that implements the Facade pattern [Gamma+95]
                                                                                       folIowing:

                                                                                           ·    Physical configuration audit: Did the build generation use the right
          could be developed with the Facade class first and stubs for the sub-                 component versions?3
          system classes. In subsequent increments, the stubs are replaced with a
          complete implementation. A single test driver would exercise the cluster
          via the Facade interface.
      .   Collaboration integration of a modal class may be accomplished with
          Class Association Test, Round-trip Scenario Test, or Mode Machine
                                                                                       3. Configuration audits are required upon completion of "acceptance testing" for compli-
                                                                                       ance with the IEEE/ANSI Standard for Software Configuration Management Plans [IEEE
                                                                                       1042]. In addition to this final audit, the same checks can prevent configuration bugs both
                                                                                       before and af ter system scope integration. An informal review to be sure that "nothing has
          Test.                                                                        slipped through the cracks" will probably suffice for six or fewer developers. As the sizes
                                                                                       of the system and the project team increase, so does the chance of configuration bugs and
                                                                                       therefore the importance of the audit.

<!-- side 11 -->

646                                                                                     Integration                                                                     647


                                                                                         . Functionalconfiguration audit: Did the build generation use the right
      1. Select an integration pattern for the cluster under test.
      2. Generate the test confjguration.
                                                                                         . family of components?
                                                                                               Environmental configuration audit: Will the build generation run on the
                                                                                               specified target environment{s}? Does the system demand too much
         2.1 Turnassertion checking on.                                                        from the hardware and software resources of the target environ-
         2.2 Turninstrumentation off.                                                          ment? Does the SUT run on all versions of opera ting systems, virtual


      3.
         2.3 Buildthe cluster or thread.
           For each test:
                                                                                          .    machines, DBMS, and so on for which compatibility is required?
                                                                                               Demonstration of minimal interoperability of components: Will A talk
                                                                                               to B? Will B run with C? Does X get to Y?
           3.1 Set the system to the requlred mode.
           3.2 Applythe thread input: manual. script. or simulator.                       At a minimum, a subsystemlsystem integration test suite should accomplish
           3.3 Comparethe expected output to the actual output.                       the steps outlined in Procedure 16. The sequence and content of specific test
           3.4 Record observed failures and assertlon violations.                     cases may be determined from the applicable pattern. Dependencies among the
           3.5 Diagnose and repalr faults.                                            subsystem and its responsibilities are primary factors in determining the appro-
                                                                                      priate integration test pattern.
      4.
      5.
           Turna6sertion checkingoff.
           Instrument the system under test using a testing too!.
                                                                                          ..   Bottom-up Integration works well for small to medium systems.
                                                                                               Top-down Integration works for almost any scope or architecture.
      6.   Rerun all your tests. Nofailures should be produced.                            .   Collaboration Integration works for almost any scope or architecture.
      7.   Evaluate call pair coverage. If insufficient. developadditional tests to       . Layer Integration applies to layered architectures.
           cover and repeat.                                                              . Client/Server Integration is appropriate for client/server architectures.
                                                                                          . Distributed Services Integration is appropriate for decentralized net-
                                                                                               works containing peer-to-peer nodes.
                                                                                          . Backbone Integration is well suited to small to medium systems and
                                                                                               especially useful for embedded applications.
                                                                                          . Big Bang Integration is useful in a few limited circumstances.
                                                                                          . High-frequency Integration can be applied at almost any scope or
                                                                                               architecture.

                                                                                          Once the integration exit criteria are met, your application is ready to sup-
                                                                                      port system scope testing.




                              Procedure15    ClusterIntegration

<!-- side 12 -->

648                                                                                                                                                                   649


                                                                                      Big Bong Integration
      1. Set up infrastructure.
         1.1 Configure environment.
                                                                                      Intent
         1.2 Create/initialize files.
          1.3 Connect input/output       devices. Establish   minimal handshaking.    Demonstrate stability by attempting to exercise an entire system with a few test
                                                                                      runs.
         1.4 Compile. link, build, make, or' assemble the entire system from a
             single library under configuration management control.
                                                                                      Contexl
      2. Verifyexecutabllity.
                                                                                      The Big Bang Integration pattern brings together all components in the sys-
         2.1 Runalf batch Job streams to completion with minimalinput.
                                                                                      tem under test without regard for intercomponent dependency or risk. A system
          2.2 Bring up all tasks in an online system. Activate each user inter-       scope test suite is applied to demonstrate minimal operability. Big Bang Integra-
              face widget at least once. Perform normal shutdown.                     tion is indieated in som e situations:
          2.3 Bring up alf tasks in an embedded system. Accept normal signal
              inputs; produce alf output at nominalvalues. Performnormal                  ·   The SUT is stabilized and only a few components have been added or
                                                                                              changed since the last time it passed a system seope test suite.
              shutdown.

      3. Verifyminimalcooperative functionality.
                                                                                          ·   The SUT is small and testable, and each of its components has passe d

         3.1 Runthrough primary end"to-end threads: use case variants, all
             CRUDpaths.
                                                                                         ·    adequate eomponent seope tests.
                                                                                              Big Bang Integration may be the only feasible approach for a mono-
                                                                                              lithic system. When components are so tightly coupled that they can-
         3.2 Runthrough all cycles: on/off, pollingloop,hourly,daily,weekly,                  not be exercised separately, it is a practical impossibility to build and
             monthly,and so on.                                                               test component subsets. This is a com mon result in unstructured sys-
                                                                                              tems developed with conventional programming languages (e.g., Basic,
                                                                                              Cobol, Fortran, assembly language), objeet-oriented implementations
                                                                                              with poor design or no design, or the accretion of ad hoc maintenanee
                                                                                              in either case. See Big BaU of Mud [Foote+97].

                                                                                         If these conditions are not present, Big Bang Integration typically ereates
                                                                                     more problems than it solves. It is a common recourse under schedule pressure,
                                                                                     but this is usually as effective as trying to put out a fire with gasoline.

                                                                                     FoultModel

                                                                                     The Big Bang fault model is ambiguous and opportunistic. The hope is that the
                                                                                     system will "run" and thereby demonstrate that system testing ean begin.

                          Procedure 16      High-Ievellntegrotion   Plon             Strotegy

                                                                                     Big Bang Integration dispenses with incremental integration testing. The entire
                                                                                     system is built and a test suite is applied to demonstrate minimal operability at

<!-- side 13 -->

650                                                                          PART
                                                                                III
                                                                                                                                                                        651

system scope. Figure 13.6 suggests this configuration. In contrast, all of the          ExitCriteria
other integration patterns presented in this chapter start by building a few com-
ponents, testing their interoperability, adding a few more, testing their inter-            ./ The test suite passes.
faces, and so on, until all interfaces have been exercised.
    The test suite for a Big Bang Integration pattern may be developed at sys-         Consequences
tem scope by using an appropriate responsibility-based test design pattern. Use
the orade for this pattern.                                                             Big Bang Integration is usually ill advised. "In its purest (and vilest) form, big-
                                                                                        bang testing is no method at all-'Let's fire it up and see if it works!' It doesn't
                                                                                       af course" [Beizer 84, 160]. A common result is that there are so many inter-
EntryCriteria                                                                          face bugs that the system barely runs, defeating the Big Bang Integration tests
      ./ Allcomponents have passed component scope testing.                            and making it hard to know where to look to diagnose a failure. "The name
      ./ The virtual machine to be used in the test environment is stable.             is somewhat misleading: the anticipated bang is all tao aften a fizzle" [Lakos
                                                                                       96, 162].
      ./ A physical, functional, and environmental audit has been conducted and
                                                                                            Big Bang Integration has two main disadvantages. First, debugging can be
         has not found any anomalies that would interfere with integration testing.    difficult because you receive fewer dues about fault locations. With incremen-
                                                                                       tal integration, the most recently added component is likely to be buggy, has
                                                                                       triggered a failure in another component, or allowed the propagation of a pre-
                                                                                       existing failure. In contrast, every component in a Big Bang Integration is
                                                                                       equally suspect. Second, even if the SUT passes, many interface faults can hide
                                                                                       and waylay subsequent system scope testing.
                                                                                           Under favorable circumstances, Big Bang Integration can result in quick
                                                                                       completion of integration testing. Few (if any) integration drivers or stubs are
                                                                                      developed. lt may be possible to demonstrate sufficient stability with a few test
                                                                                      runs. The favorable circumstances are: (1) a small, well-structured system whose
                                                                                      components have received adequate testing, (2) an existing system where only
                                                                                      a few changes have been made, and (3) a system constructed by reusing trusted
                                                                                      components. If the integration comes off without difficulty, the effort for the
                                                                                      test setup and the test run is incurred only a few times. If more than three bang-
                                                                                      fix-bang cydes are required, Big Bang Integration was a poor choice.

                                                                                      Known
                                                                                          Uses

                                                                                      Big Bang Integration has been attempted in countless projects and is discussed
                                                                                      in [Myers 76, Myers 79, Beizer 84]. Subsystem Big Bang Integration is used in




                                              .
                                                                                      Backbone Integration.



~        Driver   ~    Componentundertest           Testedcomponent

EJ Stub           -    Interfaceundertest     -     Testedinterface

     13.6 Big BangIntegration.
FIGURE

<!-- side 14 -->

652
                                                                                                                                                                       653


Bottom-upIntegration                                                                  Strategy
                                                                                      Test
                                                                                        Model
Intent                                                                                Develop a dependency tree, as outlined in Section 13.1.3, Dependency Analysis.
                                                                                      The responsibility test suite for each component may be developed wirh any ap-
Demonstrate stability by adding components to the SUT in uses-dependency              propriate test design pattern. In addition to exercising the responsibilities of the
order, beginning with components having the fewest dependencies.
                                                                                     component, however, attention must be paid to exercising subcomponents. The
                                                                                     scope of the test suite for each driver is limited to the component under test.
Context                                                                              That is, the driver does not attempt to exercise intercomponent interfaces.
The Bottom-up Integration pattern achieves stepwise verification of the inter-       Instead, the driver exercises the component that implements intercomponent
                                                                                     interfaces, thereby exercising the interfaces.
faces between tightly coupled components. Ir interleaves component scope test-
ing and integration of a system of components. Components' with the least            TestProcedure
number of dependencies are tested first. When these components pass, their
drivers are replaced with their clients; another round of testing then begins.       Bottom-up Integration works by moving from the leaves of the dependency tree
     Bottom-up Integration is often used to support unit scope testing in the it-    to the root. If the tree contains n levels, there are n stages.
erative and incremental development of a subsystem's components. That is, you
test each component as it is coded and then integrate it with already-tested com-        1. In the first stage, leaf-level components are coded. Test drivers are coded
ponents. The composition dependencies of the implementation under develop-                  for the lea f-level components, and the leaf level is tested by these drivers
ment reflect the path of least resistance. Bottom-up development is well suited             (Figure 13.7).
to a system of components with robust and stable interface definitions.                  2. Components on the next higher level are coded. These components use
     If you ean (or must) defer end-to-end, system scope exercising of the com-             (send messages to or pass as arguments) objects that were tested during
ponents until all components have been developed, bottom-up development is
indicated. This ean occur for several reasons: components developed by teams
that have limited opportunity for communication, time and space separation,
contractual requirements, departments separated by rigid bureaucracies, third-
party development, and so on. In this situation, it is critical that component
responsibility and interface definitions are stable. Architectural changes in
the middle to late stages will typically require significant reworking and re-
testing.
     Bottom-up development ean provide an early assessment of a component
that must implement a critical requirement. For example, such a component
might have to meet strict time or memory constraints, correctly perform com-
plex searching, catch a wide range of exceptions, or implement a low-level de-
vice interface with complex state behavior.

FaultModel
See Section 13.1.4, Integration Faults.


                                                                                    FIGURE13.7   Bottom-up Integration, first-stage configuration.

<!-- side 15 -->

                                                                                                                                                                  655
654                                                                          PART 111




         the previous stage. The succession of stage configurations is shown in
         Figures 13.8, 13.9, and 13.10.
      3. The entire system is exercised using the root-Ievel component, as shown
         in Figure 13.1 l.
    The actual integration test cases should be developed with an appropriate
responsibility-based test design pattern. Use the orade for this pattern.
Automation
Bottom-up Integration requires one driver for each component or component
that is a root of a subtree in the dependency relationship. A bottom-up driver
should correspond to an independent component or set of components.
Although testing several components with a single driver may be possible or
necessary, you should avoid combining the test suites for components that can
be teste d by individual drivers into a single driver. This approach favors the
reuse of the drivers and reduces test driver maintenance. Test drivers must be
revised as the dasses under test are revised, especially if refactoring occurs. The
inherently higher coupling of multicomponent drivers suggests that more effort               13.8 Bottom-up Integration, second-stoge
                                                                                        FIGURE                                                   confjguration.
can be expected to maintain them.
    Verify that each successive test configuration is checked in and under ver-
sion control before you begin development of a new configuration. In most
cases, a design change or bug fix will require revising and rerunning tests for
configurations that have already passed. If the configurations have not been
saved, this effort can be difficult with just the current configuration. The
lncremental Testing Framework is well suited to this pattern.

EntryCriteria
      .t The virtual machine to be used in the test environment is stable.
      .t The components to be integrated in each stage are minimally operable.
         That is, each meets the exit criteria for the related component scope
         tests.
      .t A physical, functional, and environmental audit has been conducted
         and has not found any anomalies that would interfere with integration
         testing.

ExitCriteria
      .t Each driven component meets the exit criteria for its test pattern.
                                                                                        FIGURE13.9   Bottom-up   Integration,   third-stoge   confjgurotion.
      .t The interface to each subcomponent has been exercised at least once.
      .t Integration testing is complete when all root-Ievel components pass
         their test suites.

<!-- side 16 -->

656                                                              PART
                                                                    11/                                                                                657


                                                                          Consequences

                                                                          Disadvantages

                                                                          Driver development is the most significant cost of Bottom-up Integration. The
                                                                          code size of the resulting test harness can easily be twice that of the system
                                                                          under test. Use of thi s pattern in conventional development assumed that driv-
                                                                          ers consisted of "throwaway" code and identified this characteristic as a weak-
                                                                          ness [Myers 76, Myers 79]. In fact, this cost may be offset if drivers are
                                                                          designed to be reusable and to promote reuse of the tested component
                                                                          [Gamma+98]. Several COTS testWare products promise to generate all or part
                                                                          of the driver code.
                                                                               If a fix, revision, or enhancement is made to a previously tested component,
                                                                          the test configuration in which this component was first tested should be re-
                                                                          vised accordingly and rerun. Subsequently, the change must be propagated
                                                                          through successive test configurations. This process can prove error-prone,
                                                                          costly, and time-consuming.
                                                                               A bottom-up driver does not directly exercise intercomponent interfaces.
FIGURE13.10 Bottom-up Integration, fourth-slage configuration.            Instead, interaction testing is limited to collaborations implemented in the com-
                                                                          ponent that is the root of the subtree under test. This may or may not consti-
                                                                          tute adequate testing of component interactions. If the components are intended
                                                                          for reuse, then an application-independent test suite of component interactions
                                                                          should be employed.
                                                                               As the upper leveIs of the dependency tree are reached, getting lower-level
                                                                          components to return values or throw the exceptions necessary to get complete
                                                                          coverage often proves difficult. This may require stubs to genera te the desired
                                                                          conditions.
                                                                               Bottom-up Integration postpones checking critical control interfaces and
                                                                          collaborations until the end of the development cyde. Operability of the high-
                                                                          est level of control and component interoperability are not demonstrated until
                                                                          the root component replaces the last driver. If component interfaces are defined
                                                                          on the fly or are unstable, the first indication of problems will come not when
                                                                          the inconsistent components are "integrated," but rather when the component
                                                                          that uses them is exercised.

                                                                          Advantages
                                                                          Bottom-up testing and integration may begin as soon as any leaf-level compo-
                                                                          nent is ready. Initially, work may proceed in parallel. That is, several develop-
                                                                          ers can work independently on subtrees. Bottom-up Integration does not limit
                                                                          testability (unlike Top-down Integration) because code coverage of the root of
                                                                          the subtree under test can typically be achieved with a suitably constructed test
 FIGURE13.11 Bottom-up Integration, final configuration.

<!-- side 17 -->

658                                                                                        Integration                                                                                  659


suite and driver. Although thi s pattern reduces stubbing, stubs may still be               . . . if we self-test every class in the system, contract-test every client-server rela-
needed to break a cycle or simulate exceptions.              .                              tionship between one client and one server, and contract-test every client-server re-
                                                                                            lationship between multiple clients of a single server, we know that the complete
     The Bottom-up Integration pattern is well suited to responsibility-based               system works correctly [Overbeck 94a, 87].
design, as only component interfaces are exercised. In contrast, Top-down
Integration requires analysis of both client and server components to develop                The question is, how do we accomplish this with minimal reliance on stubs?
server stubs.
                                                                                        A contract test cannot be passed when the usage structure of classes is cyclic (X
                                                                                        uses y, Y uses Z, Z uses X). Stubs are necessary to break a cyclic usage betWeen
KnownUses                                                                               classes, but they increase the cost of testing. An incremental strategy may be
                                                                                        used to deal with cycles and reduce the total number of test cases.
Bottom-up Integration is discussed in several sources [Myers 76, Myers 79,
Yourdon+79, Beizer 84]. Ir is recommended for use with clusters [Cheatham
+90, Lieberherr+92, Graham+93, Graham 94, Turner+95].                                       1. Find a class, C, whose superclasses and server classes have passed self-
                                                                                               testing.
                                                                                               1.1 Self-test C.
Integration
          by(ontrad                                                                            1.2 Contract test all of C's servers.
A detailed analysis of cluster scope integration testing is presented in [Overbeck             1.3 Repeat these steps until no more C is left untested.
94a]. This approach does not consider specific techniques to identify sequences,
state values, or input values. Instead, test design is based on the relationship be-       2. If any classes remain, there must be a cycle. Find a minimal set of clas ses,
tween client and server classes with all possible variations on inheritance.                  S, that have passed step 1 and which contain a cycle.
     Two basic test suites are the building blocks of the approach. A class unit              2.1 For each client in S, contract test it with all unit-tested servers.
test (se/f-test) is passed if, for all methods of a CUT: (1) message input/output             2.2 If there are cycles in S, find the smallest set of classes that could
are correct as specified, (2) every server class of the CUT is used properly, and             break the cycle. Develop and unit test stubs for each such class.
                                                                                              2.3 Test the stubbed class set.
(3) all intraclass operation sequences are correct. The second basic test suite in-
volves a client X and a server Y (X contract-test Y). X passes the contract-test              2.4 Replace the stubs with the original classes, and contract test the re-
                                                                                              sulting client and servers.
suite when: (1) X passes the self-test suite, (2) X always meets Y's preconditions,
and (3) X uses Y's output correctly.
                                                                                       Testing systems by this process ensures that
     Six cases for application of these tWo basic test suites are considered:

      .     Self-test of a base class                                                      All classes have been self-tested, all client-server   relationships   between two classes

      ·.    Self-test of a single subclass
                                                                                           have been contract-tested,      and all client-server  relationships between multiple
                                                                                           clients and a single server have been contract tested. Assuming the preconditions for
                                                                                           self-testing and contract-testing,  such systems are considered to be correct [Over-

       ..   Contract test of tWo base classes
            Contract test of the base client and subclass server
                                                                                           beck 94a 116].


                                                                                       NSafeH
                                                                                            Approach

        .   Contract test of the subclass client and base server
            Contract test of tWo subclasses                                            The "safe" approach pro vides a strategy for dealing with the potentially large
                                                                                       number of tests needed to cover bindings of polymorphic server classes [Mc-
The extent of the test suite varies in each case.                                      Cabe+94]. Each class in the system under test is tested separately by a driver.
     To integrate an arbitrarily large system, an order of integration must be es-     For a class under test to be considered "safe," a driver must execute a test suite
tablished. The self-test and contract-test patterns are then applied according to      that meets three goals:
the order of integration:

<!-- side 18 -->

660                                                                           PART
                                                                                 11/ Pafferns   .ER 13    Integration
                                                                                                                                                                                    661

      ··   Branch coverage is obtained for each method in the CUT.
           All server object methods used by the CUT are called at least once, and
                                                                                                    the cycle, but does not explain how this goal is accomplished. Once the cycle is
                                                                                                    broken, bottom-up testing may be performed. When the bottom-up testing is
           each possible binding for a polymorphic server object is used at least                   complete, the association may be reintroduced and tested.
           once.
      ·    If the CUT exports polymorphic methods, the driver must exercise, at
           least once, each possible binding of each method.
                                                                                                    ThreeOepenåencies

                                                                                                   A four-step process for C++ integration is outlined in [Lee 94]:
A class is considered "safe" when it meets all three criteria and all of its server
                                                                                                         1. Map the relationships among system components: inheritance, friends,
classes are also safe. This requirement dictates a bottom-up (uses) order of in-                            message passing, function calls, and the main program.
tegration.                                                                                               2. Test components as follows:
     McCabe et al. argued that clients of safe servers need not re-exercise bind-
ings of the server's servers. This decision limits the scope of unit-level integra-                           2.1 Root objects: those instantiated from a base class with no friends
                                                                                                                  that receives no messages from other classes.
tion for a class under test to its immediate servers, thereby avoiding a
potentially very large set of bindings for integration.                                                       2.2 Other classes that have "the fewest inheritance, friend, and message
     McCabe et al. also assert that regression testing for clients of safe classes is                             passing relationships" and do not participate in any recursive rela-
                                                                                                                  tionships.
unnecessary in some circumstances. "A change to the implementation, not the
                                                                                                             2.3 Repeat, choosing classes with higher coupling until all classes meet-
interface, of an existing method within a system requires only that the changed                                  ing these criteria have been tested.
method be tested. . . When a Safe class is extended through derivation and in-
heritance, it only needs to be tested in the weakest sense" [McCabe+94, 25].                             3. Test classes with recursive relationships. Begin with classes having "the
Because the interface is the same and client usage of this interface has already                            fewest [recursive] relationships with other untested classes. " Stub the
been tested, client uses do not need retesting. Instead, a driver is used to run a                          untested classes. Repeat until all recursive classes have been tested.
test suite on the new implementation. As a result, it is not necessary to retest                         4. Test the main program.
each client usage of the interface in an application system. It is argued that this
approach is a practical necessity when widely used server classes are changed.

ObiectRelations
A class developer must usually consider questions like, "To test this class, what
other classes must be tested?" Kung et al. propose a program representation
called the object relation diagram (ORD) to answer such questions [Kung+93].
The ORD contains a node for each class and an edge for each inheritance, ag-
gregation (instance variable), and association (message parameters and friends).
Some order (of class integration) supposedly exists that can "eliminate" test
drivers or method stubs. This order can be discovered by analysis of the ORD.
     If the ORD is acyclic, a stubless test order can be obtained from the ORD
by a topological sort (see Section 13.1.3, Dependency Analysis). Cycles happen
frequently, however. A strategy for testing without stubs is outlined. It is as-
serted that all cycles must contain at least one association (a "pr00f" for this
condition is noted but not given; it is easy to imagine counterexamples of cycles
widfout associations). The strategy calls for "eliminating" one association from

<!-- side 19 -->

                                                                                                           Integration
662                                                                                  PART
                                                                                        1/1 PaHeros                                                                                   663


Top-downIntegration                                                                                         · Framework development. Reusable frameworks often implement a
                                                                                                              general control strategy while using abstract base classes to defer
                                                                                                              application-specific detaiIs. The first increment of a framework ean
Intent                                                                                                        exercise the control strategy. In fact, a framework's abstract base classes
                                                                                                              provide an ideal interface for stub development. Subsequent increments
Demonstrate stability by adding components to the SUT in control hierarchy                                    can exercise use case and domain model responsibilities. See New
order, beginning with the top-level control objects.                                                          Framework Test.

Context                                                                                                Foult
                                                                                                          Model

Top-down Integration achieves stepwise verification of the interfaces among                            See Section 13.1.4, Integration Faults.
components that operate under a common control strategy. This control strat-
egy dictates the order of development, integration, and testing. Top-down                              Strotegy
Integration interleaves component scope testing and integration of a system of                         Test
                                                                                                          Model
components.
    Suppose you are developing a system folIowing an iterative and incremen-                           The apex of control may be represented in a Collaboration Diagram, Sequence
tal approach. The control structure of the system can be modeled as a tree in                         Diagram, or Statechart of the system under test. A responsibility test suite may
which the top-level components have control responsibilities.4 Jacobson calls                         be developed with any appropriate test design pattern. If a system scope state
these components control objects. Control objects typically implement essential                       machine model has been (or can be) developed, the cluster that implements the
and nontrivial control strategies and therefore present relatively high risk. Top-                    state machine will probably be at the top of the hierarchy. In this case, the test
down Integration focuses on these components fjrst, making the demonstration                          suite is developed by applying Modal Class Test or Mode Machine Test. If there
of system scope end-to-end operability a high priority.                                               are no sequential constraints on the collaborations, the test suite should be de-
     Because Top-down Integration exercises the control objects fjrst, deferred                       veloped based on system scope responsibilities using Collaboration Integration,
or concurrent development of the controlled components is possible. Con-                              Round-trip Scenario Test, or Covered in CRUD.
 sequently, this pattern is also useful in several variants of incremental develop-
 ment, even if control integration is trivial or low risk.                                            TestProcedure

      . Incremental   development.     Each increment     adds another    layer under the             Model the control hierarchy as a dependency tree. Next, develop a staged plan
        controllayer.                                                                                 for implementation and testing. Then, design a responsibility-based test suite at
                                                                                                      system scope.
      . Concurrent hardware/software development. High-level control compo-
        nents can be designed, coded, and tested while hardware or subsystem                              1. Develop and test the component(s) at the highest level of control first.
        interfaces are designed and developed.
                                                                                                             Implement the servers of this component as stubs or proxies. Figure
      . Concurrent software development. High-level control components can                                   13.12 shows the fjrst top-down configuration.
         be designed, coded, and tested while a virtual machine or subsystem                             2. Continue in a breadth-first swath at each level, replacing the server stubs
         interfaces are designed and developed.
                                                                                                            with a full implementation and stubbing the next lower level of servers.
                                                                                                            The configurations found at successive stages of Top-down Integration
                                                                                                            are shown in Figures 13.13, 13.14, and 13.15.
 4. Applications of hierarchic control in object-oriented development are described in                   3. Continue in this manner until all servers in the system under test have
 Manager [Sommerlad 98J, Recursive Control [Selic 98J, and Bureaucracy [Riehle 98].
 Douglass provides several implementation control patterns at the architectural, "mechanis-
                                                                                                            been implemented and exercised. Figure 13.16 shows the final-stage con-
                                                                                                            figuration.
 tic," and class scope [Douglass 98J.

<!-- side 20 -->

                                                                           13.14 Top-down Integration, third-stage configuration.
                                                                      FIGURE
FIGURE13.12 Top-down Integration, first-stage configuration.




                                                                          13.15 Top-down Integration, fourth-stage configuration.
                                                                     FIGURE
FIGURE13.13 Top-down    Integration, second-stage   configuration.
                                                                                                                                    665
664

<!-- side 21 -->

I   666                                                                                      Integration                                                                   667


I                                                                                             .I A physical, functional, and environmental audit has been conducted
                                                                                                  and has not found any configuration anomalies that would interfere
                                                                                                  with integration testing.

                                                                                          ExitCriteria

                                                                                              .I Each driven component meets the exit criteria for its test pattern.
                                                                                              .I The interface to each subcomponent has been exercised at least once.
                                                                                                 Some coverage analyzers will instrument and analyze call-pair traces,
                                                                                                 detailing exactly which interfaces have and have not been exercised.
                                                                                              .I Integration testing is complete when a build that indudes all lea f-level
                                                                                                 components passes the system scope test suite.

                                                                                          Consequences

                                                                                          OiSlJdvantages

                                                                                          Stub development and maintenance are the most significant costs associated
                                                                                          with Top-down Integration.


    FIGURE13.16 Top-down Integration,   final-stage   configuration.
                                                                                              ·   Because stubs pro vide a necessary part of each test, setting up a test re-
                                                                                                  quires that a large number of stubs be coded to provide the desired re-
                                                                                                  sponse. Complex test cases can require recoding of stubs for each test
        The test suite for a Top-down Integration may be developed at system                     case. As the number of stubs increases for a test configuration, it be-
                                                                                                 comes more difficult to maintain the necessary control and consistency.
    scope by using an appropriate responsibility-based test design pattern. Use the
    orade   for thi s pattern.                                                               ·   An unforeseen requirement in a lower-Ievel component may necessitate
                                                                                                 last-minute changes to many top-level components, breaking part of
    Automation                                                                                   the test suite. If a fix, revision, or enhancement is made to a previously
    This pattern requires a single driver for the control apex. A stub is needed for             tested component, the test configuration in which this component was
    each component in the layer below the current focus of integration.                          first tested should be revised accordingly (driver and stubs) and rerun.
        Verify that each successive test configuration is checked in and under ver-              Then, the change must be propagated through successive test configura-
    sion control before you begin developing a new configuration. A design change
    or bug fix willlikely require revising and rerunning tests for configurations that
    have already passed. If the earlier configurations have not been saved, this step
                                                                                             ·   tions. This proces s can be error-prone, costly, and time-consuming.
                                                                                                 The stubs are necessarily implementation-specific and likely to be
                                                                                                 brittle.
    can be difficult with just the current configuration.
                                                                                              Ic may be difficult to exercise lower-Ievel components sufficiently. The dis-
    EntryCriteria                                                                        tance between the testing interface and the components being integrated in-
                                                                                         creases with each successive level. This progression usually results in a loss of
        .I The virtual machine to be used in the test environment is stable.
                                                                                         test control over lower-Ievel components that are integrated at the end of the
          .I The components to be integrated in each stage are minimally operable.       cyde. Because attaining high coverage when the component is a long distance
             That is, they should meet the exit criteria for the related component       from the driver may be difficult or impossible, a low coverage report may be
             scope tests.                                                                difficult to interpret or remedy.

<!-- side 22 -->

668                                                                                  PART11/
                                                                                                                                                                          669

   The interoperabilty of all components in the SUT is not tested until the last               RelatedPalterns
component replaces its stub and the test suite passes.
                                                                                               Modified Top-down Integration follows the same integration sequence, but re-
Advantages                                                                                     quires that the components be tested under a driver before they replace a stub
 Testing and integration may begin early-when the top-level components are                     [Myers 76]. This approach avoids the major disadvantage of a pure top-down
coded. The first-stage test exercises all high-level component interfaces and can              approach-the increasing difficulty of testing lower-Ievel components.
 provide an early demonstration of end-to-end functionality.
      The cost of driver development is reduced. Component-specific drivers typ-
 ically use hard-coded test cases and are tightly coupled with a component inter-
 face. This setup limits driver and test suite reuse. In contrast, although the
driver for a top-level component shares the same problems, there is only one
driver to maintain, instead of a driver for every subtree. Typically, the test cases
can be reused to drive lower-Ievel tests. As lower-Ievel components are added
and tested, upper-Ievel components are exercised again, providing regression
testing. Because many stubs are typically needed (compared with Bottom-up
Integration), this reduced cos t of driver development may be offset by the
higher cost of stub development.
      Initially, components may be developed in parallel. Several developers can
work independently on different leveIs of components and stubs. Implementa-
tion of low-Ievel, device-dependent components can be deferred. This flexibil-
ity may be useful if the target environment is under development or likely to
change.
     If the lower-Ievel interfaces are undefined or likely to change, then Top-
down Integration can avoid commitment to an unstable interface.

KnownUses
Top-down Integration is discussed in several sources [Myers 76, Myers 79,
Yourdon+79, Beizer 84]. This pattern has been widely used. The exploratory
development style followed by Extreme Programming often corresponds to
Top-down Integration [Anderson+98]. Fred Brooks observes:

   Some years ago, Harlan MiIIs proposed that any software system should be grown
   by ineremental development. That is, the system should first be made to run, even
   though it does nothing useful exeept eall the proper set of dummy subprograms
   [stubs]. Then, bit by bit, it is fleshed out, with the subprograms in tum being de-
   veloped into aetions or ealls to empty subs in the level below. . . . Nothing in the
   past deeade has so radieally ehanged my own praetiee, or its effeetiveness. One
   always has, at every stage in the proeess, a working system. I find that teams
   ean grow mueh more eomplex entities in four months than they ean build [Brooks
   87,15].

<!-- side 23 -->

                                                                                                                                                                         671
670

                                                                                     TestProcedure

Collaboration
            Integration
                                                                                          1. Develop a dependency tree for the system under test. Map collabora-
                                                                                             tions onto the dependency tree until all components and interfaces are
Intent                                                                                       covered.
                                                                                         2. Choose a sequence in which to apply the collaborations. There are sev-
Demonstrate stability by adding sets of components to the SUT that are re-
quired to support a particular collaboration.
                                                                                             ·.
                                                                                            eral possible heuristics for choosing a sequence.
                                                                                               Begin with the simplest and finish with the most' complex.
                                                                                                  Begin with the collaboration    that requires   the fewest stubs   or that
Context
Integration by collaboration exercises interfaces betWeen the participants of a
collaboration and organizes integration according to collaborations. A system
                                                                                             .    minimizes stubs for other collaborations.
                                                                                                  Develop a state machine that modeIs sequential constraints on col-
                                                                                                  laborations. Each collaboration should correspond to a transition.
typically supports many collaborations. Their sequence may be selected ac-                        Develop the transition tree and test collaborations by traversing the
cording to dependencies, sequential activation constraints, or both. Integration
is completed when all components and interfaces have been exercised, which
may not require testing every collaboration. Indications for integration by col-
                                                                                             .    tree.
                                                                                                  Test in order of risk of disruption for system testing. Considee the rel-
                                                                                                  ative effects on the progress of system testing of a showstopper bug in
laborations indude the following:                                                                 each collaboration. Test the most influential collaborations fiest, stop-
                                                                                                  ping when the interfaces and components are covered.
      .   The system under test has dearly defined collaborations that can
          cover all components and interfaces in its scope.
                                                                                         3. Develop the test suite for the first collaboration. If possible, design the

      .   Demonstrating a working collaboration as soon as possible is
          important, possibly because of technical risk or criticality of a
                                                                                            test driver to support additional collaborations and more extensive sys-
                                                                                            tem testing. Figure 13.17 depicts a first configuration.
                                                                                         4. Run the test suite and debug until the first collaboration test passes.

      .   collaboration(s).
          Credible demonstration requires more than just a pass through the top
          of the control hierarchy. For example, all participants of a processing
                                                                                            Revise your test design as necessary. Continue until all collaborations
                                                                                            have been exercised. Figure 13.18 depicts a second configuration. Figure
                                                                                            13.19 illustrates the final configuration (all components and interfaces
          thread in a sensor-controller-actuator path or a search function with             exercised).
          critical performance objectives must be exercised to demonstrate basic
                                                                                         The test suite for Collaboration Integration may be developed at system
          interoperability.
                                                                                     scope by using an appropriate responsibility-based test design pattern. Round
                                                                                     Trip Scenario Test and Extended Use Case Test are typically usefu\. Use the
FoultModel                                                                           orade for this pattern.
 SeeSection 13.1.4, Integration Faults.
                                                                                     Automation
Strotegy                                                                             This pattern requires a single driver coupled to the interface for the collabora-
Test
   Model                                                                             tion. In applications with a GUl, this driver is typically a widget or sequence
                                                                                     of widgets. Stubs are required for untested components that do not panicipate
 The components induded in a collaboration are selected by membership in a           in a collaboration. As each collaboration is tested, the relevant stubs are
 processing thread, an event-response path, or critical performance objectives. If   replaced.
 there are no sequential constraints on the collaborations, the test suite should
                                                                                         Verify that each successive test configuration is checked in and under ver-
 be developed for system scope responsibilities using Collaboration Integration,     sion control before you begin developing a new configuration. A design change
 Round-trip Scenario Test, or Covered in CRUD.

<!-- side 24 -->

                                                                                                                                                                                  673
672


                                                                                                                                                       Scope ol Collaboration 2

  Scope ol Collaboration 1




                                                                                     FIGURE 13.18 Collaboration   Integration, second configuration.
FIGURE13.17 Collaboration    Integration, first configuration.


                                                                                     ExitCrileria
                                                                                         .I Each tested collaboration meets the exit eriteria for its test pattern.
or bug fix willlikely require revising and rerunning tests for configurations that
have already passed. If the earlier eonfigurations have not been saved, thi s step       .I All component and intereomponent messages in the system have been
ean be diffieult with just the eurrent eonfiguration.                                       exercised at least once (this does not necessarily require all collabora-
                                                                                            tions to be tested, just as statement eoverage does not require all paths
                                                                                            to be eovered). Coverage analyzers will instrument and analyze call-
EntryCrileria
                                                                                            pair traees, detailing exaetly which interfaees have and have not been
      .I The virtual machine to be used in the test environment is stable.                  exereised.
      .I The components of the collaboration are minimally operable. That is,
         they should have met the exit criteria for the appropriate component        Consequences
         scope test pattern.
                                                                                     Oisadvanfages
      .I A physical, functional, and environmental audit has been conducted
                                                                                     The disadvantages of integration by eollaboration are as follows:
         and has not found any anomalies that would interfere with integration
         testing.
                                                                                         .   Intereollaboration dependeneies may be subtle and may not have been
                                                                                             modeled.

<!-- side 25 -->

674                                                                                        Integration                                                                   675


                                                       Scope of Collaboration 3                  test suite may therefore miss bugs that are load-sensitive,arise from

                                                                                            .    component interactions, and so on.
                                                                                                 Unless several developers share development of collaboration compo-
                                                                                                 nents, it is unlikely that much overlap ean be achieved.

                                                                                        Advontoges
                                                                                        The advantages      of integration   by collaboration   are as follows.


                                                                                            . Interface coveragemay be obtained with a few test runs. It may not be
                                                                                                 necessary to exercise all collaborations to achieve integration coverage,
                                                                                                 if a few collaborations exercise all the interfaces.
                                                                                             .   The testing focuses on end-to-end functionality, so collaboration test
                                                                                                 cases will probably be useful for system scope testing. A Collabora-
                                                                                                 tion Integration test suite can often be reused and expanded for system

                                                                                             .   testing.
                                                                                                 A Collaboration Integration test suite is minimally coupled to com-
                                                                                                 ponents of a collaboration becauseit usesa single test interface. If
                                                                                                 the collaboration componentschuendue to requirementsinstabil-
                                                                                                 ity or bug fixes, then this lower coupling can reduce test suite
     13.19 Collaboration Integration, final eonfiguration.
FIGURE                                                                                           breakage.
                                                                                             .   Testing and integration may begin when the components in the first co 1-
                                                                                                 laboration are coded. As a result, end-to-end capability can be demon-
      . The exit criteria are similar to statement coverage. Exercising all com-                 strated later than it would in Top-down Integration, but sooner than
           ponents and interfaees without exercising all collaborations
           possible, missing some interface bugs.
      . Participants in a collaboration
                                                                              may be


                                             are not exercised separately. In effect,
                                                                                             .   in Bottom-up Integration.
                                                                                                 The pattern minimizes driver development costs, for the same reasons
                                                                                                 as with Top-down Integration. As lower-Ievel components are added
                                                                                                 and tested, upper-Ievel components are exercised again, providing re-
      ..   this patteen is a scenario-wise   Big Bang Integration.
           Some initial collaborations may require many stubs.
           Exercising lower-Ievel components sufficiently may be difficult. The dis-
                                                                                                 gression testing.

                                                                                        Known
                                                                                            Uses
           tance between the testing interface and the components being integrated

       .   can be long enough to make it difficult to set up test cases.
           The specifiedeollaborations may be incomplete. A Collaboration
           Diagram depicts a call path or familyof call paths that are feasible for
                                                                                        OOSEIntegration Strotegy

                                                                                        A coherent approach to integration testing is used in the OaSE methodology
                                                                                        Uacobson+92]. Integration occurs several times during development and should
           the implementation under test and relevant to the application. All feasi-    (ideally) be performed in the target environment. The initial process is essen-
                                                                                        tially Bottom-up Integration:
      .    ble paths need not be modeled.
           Collaborations do not foeus on peeuliarities of component interfaces,
           environment dependencies, and exception handling. A collaboration

<!-- side 26 -->

676                                                                                                                                                                                          671
                                                                                        PART11/   Patter"s


      When a block has been tested, you test it together with another block. When                            S;emens
      these two are working together, you add another, and so on. When all appear to
      work, we ean consequently change to black-box testing . . . [Jacobson+92,                              Integration plays a key role in an overall test strategy reponed by a unit of
      324-3251.                                                                                              Siemens AG Uiittner+94c]. The strategy has two main steps. First, intraclass in-
                                                                                                             tegration testing is performed with server objects stubbed out. The orde r of in-
     Next, integration testing is organized by use cases because they "explicitly                            tegration follows the order of inheritance: base classes first, refined and new
interconnect several blocks and classes" Uacobson+92, 327]:                                                  deri ved member functions second, followed by interleaved sequences of inher-
                                                                                                             ited, new, and redefined member functions.
     . . . integrate one use case at a time. . . The requirements model forms again a pow-                        Integration by uses (definition of class variables, message sending, and so
     erful tool here; as we test each use case, we check that the objects communicate                        on) follows. Ir requires code analysis. The steps are as follows:
     correctly [and check] user interfaces . . . the requirements model is verified by the
     testing process [Jacobson+92, 327].
                                                                                                                 1. Inventory and classify all uses in the system under test. A simple use in-
    Besides normal usage patterns, "odd courses" for use cases are tested and                                       volves two objects; a complex use involves several objects. A cyclical use
use cases identified from requirements and user documentation. Integration by                                       involves objects that use each other.
use cases is also suggested in [Firesmith 93b] and [Graham 94].                                                  2. For each simple use, devise at least one test that exercises the use.
                                                                                                                 3. For each nonsimple use, devise at least one test. Monitor simple uses
Atom;c System Fundions                                                                                              covered by complex uses to see whether any simple uses are tested as a
Conventional structural integration strategies do not work well for object-                                         side effect.
oriented systems, which make extensive use of dynamic binding Uorgenson                                          4. For cyclical uses, break the cycle with a stub. Test the parts of the cyclic
+94]. Without a "main program, there is no clearly defined integration struc-                                       use separately, then test the whole cyclic use. This process continues until
ture. Thus there is no decomposition tree to impose. . . testing orde r of objects."                                all uses have been tested.

    The shift to composition (especially when reuse occurs) adds another dimension
                                                                                                                  As use-based tests are performed, message traces are recorded as an object
    of difficulty to object-oriented software testing: it is impossible to ever know the
    full set of "adjacent" objects with which a given object may be composed                                 communication diagram. This diagram is then compared with the specified ob-
    [Jorgenson+94,33J.                                                                                       ject communication diagram. Differences are employed to devise additional test
                                                                                                             cases. Additional object-to-object relationships in the OOA/D model can be
     Integration can be accomplished by thread identification. A thread begins                               used to select more tests. Only one test case is developed for each such facet of
with an externally genera ted event (a user keypress, for example). Objects                                  the model.
genera te a message sequence in response. This sequence traces a path over                                        This integration strategy promotes classes from unproven to proven, one at
the network of method interfaces in the system under test. The end of the                                    a time. Within the scope of the system under test, objects are identified as either
sequence is the "event quiescence" message, which typically causes some re-                                  proven or unproven. Testing proceeds by examining each unproven object with
sponse to be sent to the system's environment (a message is displayed to                                     trusted components. When the tests pass, the object under test is added to the
the user).                                                                                                   proven group. When an unproven object uses other unproven components, the
     This end-to-end thread is an "atomic system function" (ASF). Ir provides a                              unproven objects are stubbed out to allow unambiguous results for the object
basis for integration testing by composition. The test approach requires identi-                             under test.
fication and execution of ASFs. ASFs may be identified either from a suitable                                     The same degree of testing is applied to modified classes as is performed for
representation ar by analysis of source code.                                                                entirely new classes. When only the implementation changes (the interface and
                                                                                                             specification remain constant), regression testing becomes easier when we use
                                                                                                             the unchanged version to produce expected results. Encapsulation is seen as

<!-- side 27 -->

678                                                                         PART11/ Potter"!                                                                                        679
                                                                                               'R 13   Integration


helpful, as it prevents side effects. Inheritance, however, increases the complex-
ity of test planning and execution.                                                               BockboneIntegration

Med/ronics Integrotion Process
                                                                                                  Intent
A four-step process is used at Medtronics to test object-oriented software used
in safety-critical applications [Brown+95]. After formal Fagan-style code in-                     This pattern combines elements of Top-down Integration, Bottom-up Integra-
spections on each dass (step one), Big Bang Integration is done at duster scope                   tion, and Big Bang Integration to verify interoperability among tightly coupled
(step two). The goal is to exercise each public member function once and                          subsystems.
demonstrate a minimal level of functionality. The primary goal of the third step
is to verify relationships among dasses in a duster or subsystem and exercise                     Context
safety-related functionality. This is accomplished using the FREE test model,
                                                                                                  Embedded system applications and their infrastructure (e.g., a real-time execu-
applied at dus ter scope. At duster scope, the transitions correspond to collab-                  tive kern el or backbone5) are often developed at the same time.
orations. System scope testing is done by an independent test gro up (the fourth
step.)
                                                                                                       ··   The application system cannot opera te without the back bone.
                                                                                                            The back bone provides services that are essential for running tests and
RelatedPafferns

 "Umbrella" and "depth-first" integration [Yourdon+79] are special cases of
Collaboration Integration. In an umbrella, components induded in an incre-
                                                                                                        ·   the application.
                                                                                                            Attempting to stub the entire back bone would be impractical or would
                                                                                                            require very complex stubs. The stubs would be either too costly or of
ment are selected according to their membership in a processing thread, an
event-response path, or time-critical performance objectives. The components
in a depth-first incremen t correspond to a root-to-Ieaf subtree of the depend-
                                                                                                       ·    dubious fidelity.
                                                                                                            The high-level control strategy can be exercised with stubs for the
ency tree or control hierarchy. Successive increments correspond to subtrees.
This approach may be useful if the collaborations require other collaborations.
                                                                                                       ·    upper and middle leveis.
                                                                                                            The middle level consists of several dusters that have loose intercluster
                                                                                                            coupling, but tight intraduster coupling.
For example, in a simulation, several collaborations may be required to set up
a model. Once the model is established, the simulation may be run in several
ways. Each setup and test run corresponds to a collaboration, which in turn                           At first glance, it might seem that this situation presents a Catch-22 test-
corresponds to a root-to-Ieaf subtree.                                                           ing problem: How can the application be run if the back bone is incomplete? In
     The "wave-front" approach defines a development increment as induding                       fact, "the back bone is the best possible test engine. The strategy is to create
only collaborations that can be tested with the fewest number of stubs. The goal                 a back bone that permits further testing. The focus of testing moves from the
is to define the scope of an increment by collaborations that are "sufficient                    deep infrastructure outward to the application components and functionality.,,6
for all dependent dasses to achieve their negotiated level of behavior"                          Backbone Integration verifies interoperability in a system that has a distinct
[McGregor+94].                                                                                   locus of control, relies on many services of a complex virtual machine, and uses
                                                                                                 several independent dusters to implement the balance of system responsibilities.

                                                                                                 Foult
                                                                                                    Model

                                                                                                 See Section 13.1,4, Integration Faults.


                                                                                                 5. Backboneas an integration metaphor is due to [Beizer 84].
                                                                                                 6. Boris Beizer,private communication,March 1999.

<!-- side 28 -->

680                                                                        PART
                                                                              11/
                                                                                                                                  681

Strategy
Test
   Model
Backbone Integration combines elements of Top-down Integration, Bottom-up
Integration, and Big Bang Integration. The test design problem is to first iden-
tify the components that support application control, the back bone, and appli-
cation subsystems. The sequence of testing is based on this analysis.

      .   The top and possibly the second level of control are tested top-down,
          using Mode Machine Test, Round-trip Scenario Test, or Collaboration
          Integration. Interfaces to the backbone and other subsystems are re-           ~                                        ~
                                                                                         ,   Backbone                                         I
      .   placed with proxies or stubs.
          The application subsystems are developed bottom-up using test design
                                                                                         I
                                                                                         ,
                                                                                         I
                                                                                                                                              I
                                                                                                                                              I
                                                                                                                                              I

      .   patterns indicated by the component responsibilities.
          The back bone components are tested in isolation under a driver and
          test design appropriate to their specific responsibilities.
                                                                                         I
                                                                                         ,
                                                                                         l
                                                                                                                                              ,
                                                                                                                                              I
                                                                                                                                              ,



                                                                                             13.20BackboneIntegration,step 1.
                                                                                         FIGURE
    The tested components (subsystems) are then big banged. The big bang
test suite should ind ude all control tests developed for the control compo-
nent. Additional tests should be developed to cover scenarios implemented by
application-specific subsystems.

TestProcedure

This back bone strategy interleaves several basic integration patterns.

      1. Do adequate testing on each back bone component, in isolation. Use
         drivers and stubs as needed, as suggested in Figure 13.20. Achieve the
         exit criteria for the applicable test pattern.
      2. Perform Top-down Integration on the application control components,
         as suggested in Figure 13.21. The resulting test suitesidrivers should be
         designed to be reusable/rerunnable for subsequent integration.
                                                                                     r                                            ~
      3. Big bang the backbone: load it, accept nominal input, and shut it down.     I~~                                                  ,
         Repeat this cyde several times.                                             I                                                I
                                                                                     I                                                ,
      4. Exercise the big banged back bone under a driver. Use an appropriate        I                                                I
         subsystem design: Mode Machine Test, Round-trip Scenario Test, or           ,                                                I
         Collaboration Integration. Figure 13.22 shows the configuration for         I                                                I
         steps 3 and 4.
                                                                                     l                                                ,




                                                                                     FIGURE
                                                                                          13.21 Backbone Integration,   step 2.

<!-- side 29 -->

682                                                                                                                                          683




                                                                                     I
:   Backbone                                                                         1
I                                                                                    1
1                                                                                    I                                                            1
1
I
                                                                                     I
                                                                                     I
                                                                                         II Backbone                                              1
                                                                                                                                                  1
1                                                                                    1   I                                                        1
I                                                                               ~        I                                                        1
                                                                                         1                                                        1
FIGURE13.22 Backbone Integration,   steps 3 and 4.                                       1                                                        1
                                                                                         I                                                    ~
                                                                                         FIGURE
                                                                                              13.23   Backbone Integration, steps 5 and 6.
      5. Rebuild the control subsystem with the back bone. Big bang the control
         components and the back bone; use the control test suite to exercise the
         interactions.
      6. Use the top-down approach on the next level. Remove stubs for second-
         level components and implement the interfaces. Expand the control test
         suite to reach all newly implemented components and rerun the tests.
         Figure 13.23 shows the configuration for steps 5 and 6.
      7. Use the top-down approach on the leaves. Remove stubs to any remain-
         ing subsystems and implement the interfaces. Expand the control test
         suite to reach all newly implemented components and remn the tests.
         Remn the control test suite, expanded to reach all backbone interfaces,
         as suggested in Figure 13.24.

     These steps complete the first cyde. High-frequency Integration may be                                                                    I
appropriate for subsequent cydes. Under incremental development, more capa-              I&~~                                                  1
bilities will be added to each configuration. As these capabilities are added, the       1                                                     1
test suite should be revised and remn.                                                   1                                                     1
     The test cases for Backbone Integration may be developed at system scope            I                                                     1
                                                                                         I                                                     1
by using an appropriate responsibility-based test design pattern. Use the orade          I                                                     1
for this pattern.                                                                        I                                                    ~
                                                                                         FIGURE
                                                                                              13.24   Backbone Integration, step 7.

<!-- side 30 -->

II
II
  I   684                                                                                                    13    Integration
 I                                                                                   PART lllPatteros   'R
                                                                                                                                                                                                              68S
 I
      Automation                                                                                                  An alternative to testing with a back bone is to develop a back bone simula-
      This pattern requires drivers and stubs for the integration pattern applied to                         tor. Although this approach is common for embedded systems, simulators are
      each configuration: stubs for clients of the control component, drivers for the                        often low-fidelity approximations of the back bone. Testing on a simulator can
      application cluster components, and so on.                                                             identify bugs that are a result of the simulator but miss bugs that the simulator
          Verify that each successive test configuration is checked in and under ver-                        cannot trigger. Using the actual back bone avoids these problems and the devel-
                                                                                                             opment cost of a simulator.
      sion control before you begin developing a new configuration. A design change
      or bug fix willlikely require revising and rerunning tests for configurations that                         Integration begins on the early side of the midpoim in developmem.
      have already passed. If the earlier configurations have not been saved, this                           Minimal system operability will be demonstrated roughly halfway, reducing the
      process can be difficult with just the current configuration.                                          risk of a big fizzle at the end of the cycle. Development and testing of compo-
                                                                                                             nems and subsystems may proceed in parallel until the fjrst major integration.
      EntryCriferio                                                                                          Known
                                                                                                                 Uses
            .t The architecture of the system and all component interfaces have been
               designe d and passe d careful review.                                                         The development of Windows NT [Zachary 94J can be characterized as a
            .t The back bone and application are roughly the same size and complexity.                       Backbone Integration followed by High-frequency Integration. Backbone Inte-
                                                                                                             gration is a good fit for developing the "executable architecture" to be pro-
                                                                                                             duced in the design phase in Booch's macro process [Booch 96].
      ExitCriterio
            .t All components meet the exit criteria for the applied test pattern.                           DataVisionCADSystem

            .t Each intercomponent interface has been exercised at least once.                            A hierarchical, incremental integration strategy was successful for Data Vision's
               Coverage analyzers will instrument and analyze call-pair traces, detail-                   computer-aided design (CAD) system implemented with 1 MLOC of C++
               ing exactly which interfaces have and have not been exercised.                             [Thuy 92J. Early integration testing was effective, but had to deal with incom-
            .t The complete increment passes a system scope test suite.                                   plete components. The alternative-waiting       for all components to be in hand
                                                                                                          (Big Bang Integration}-was       unacceptable. Ir would have required the emire
                                                                                                          system to be available and running, implying integration late in development.
      Consequences
                                                                                                          Debugging at thi s stage is difficult and time-consuming.
      Disadvantages                                                                                           A hierarchical approach to both design and testing was therefore followed.
      A careful analysis of system structure and dependencies is necessary. Backbone                     The lowest level was a "package" of classes (any cohesive logical or physical
      Integration requires development of both stubs and drivers. This approach as-                      group; a kind of high-level module). Public member functions provided the
      sumes that the conditions for a successful Big Bang are acheived by adequate                       package interface. Packages were organized into libraries and engines. There
      testing of the back bone components. If this prerequisite is not met, the prob-                    were fewer than 10 engines for the entire DataVision system. Dependencies that
      lems discussed under Big Bang Integration are likely.                                              crossed component boundaries were a significant problem. A dependency exists
                                                                                                         when each of two or more classes cannot function without a full implementa-
                                                                                                         tion of the other.
      Advantages
      Backbone Integration mitigates the disadvantages of Top-down Integration and                                . . . It is difficult and extremely costly to test a component really independently of
      Bottom-up Integration by curtailing their use at the point at which they lose ef-                           the componems it depends on. That implies replacing those componems by stubs,
      fectiveness. Top-down Integration is used only on the upper controllevels and                               for which the reliability is not guarameed and for which the costs and delays are
      Bottom-up Integration is restricted to the application subsystems. Big Bang                                 very high. It is on the other hand possible and acceptable to test a componem when
                                                                                                                  those it depends on have themselves been tested. [This can be done if there are no
      Integration of the backbone is preceded by component testing.
                                                                                                                  dependence     cydes, so] there must be no large scale dependence   cycle [sic] [Thuy 92,
                                                                                                                  4-T-1-5].

<!-- side 31 -->

686                                                                      PART11/                                                                                 687


    Dependencies among components (at any level) complicate integration test-      LayerIntegration
ing. Thus, dependencies and side effects should be removed, if at all possible.
Thuy notes that if dependencies are unavoidable betWeen components at a
given level, they should be contained within a single higher-level component.      Infenf
                                                                                   Layer Integration uses an incremental approach to verify stability in a layered
RelafedPatterns                                                                    architecture.
Myers's Sandwich integration calls for a combination of Top-down and
Bottom-up Integration, with the final configuration meeting in the middle          (onfexf
[Myers 76]. The backbone strategy is similar, but requires more testing of com-    Layer Integration verifies interoperability in a system that can be modeled
ponents in isolation [Beizer 84].                                                  as a hierarchy that allows interfaces only betWeen adjacent layers. That is, the
                                                                                   top layer does not send messages to the bottom layer, and vice versa. Instead,
                                                                                   the top layer sends messages to the middle layer and the middle layer send s
                                                                                   messages to the bottom layer. (For examples of this architecture, see Layers
                                                                                   [Buschmann+96] and Recursive Control [Selic 98].)

                                                                                   Faulf
                                                                                      Model
                                                                                   See Section 13.1.4, Integration Faults. The layered model is popular for device
                                                                                   interfaces and device drivers in embedded systems. Typically, propagation of a
                                                                                   message through the layers must meet a hard, real-time deadline. Inadequate
                                                                                   performance in a layered application is often a critical failure.

                                                                                   Sfrafegy
                                                                                   Test
                                                                                     Model
                                                                                   Layer Integration com bines elements of Top-down Integration and Bottom-up
                                                                                   Integration. The test design must identify the layers and determine which inte-
                                                                                   gration pattern applies to each layer. Test design and sequence of testing within
                                                                                   a layer follows this pattern.

                                                                                       ·   The top layer and possibly the second layer are tested top-down, us-
                                                                                           ing Mode Machine Test, Round-trip Scenario Test, or Collaboration
                                                                                           Integration to exercise control with stubs implementing the lower

                                                                                       ·   layer.
                                                                                           The middle layer is developed bottom-up, using a test design appropri-

                                                                                       ·   ate to its specific responsibilities.
                                                                                           The primary components of the bottom layer are tested in isolation,
                                                                                           using a test design appropriate to its specific responsibilities.

<!-- side 32 -->

                                                                                        PART11/       Integration                                                                  689
688

                                                                                                       4. After the interfaces in each layer have passed, remove any stubs and im-
    All the control threads already tested for the top layer should be rerun when
                                                                                                          plement the interfaces to the next layer.
each lower-level test suite is added. The test suite should exercise all collabora-
tions that traverse alllayers.
                                                                                                       Figure 13.26 gives the test configurations for a Top-down Integration ap-
                                                                                                  proach. A Bottom-up Integration approach would reverse the order, beginning
TestProcedure
                                                                                                  with the lowest-levellayer and working toward the top.
Layer Integration may be top-down or bottom-up. A top-down approach takes                              The test cases for Layer Integration may be developed at system scope by
the folIowing steps:                                                                              using an appropriate responsibility-based test design pattern. Use the orade for
                                                                                                  this pattern.
      1. Test each layer in isolation. Use drivers as needed. Achieve the exit cri-
         teria for the pattern of choice. Figure 13.25 depicts layer isolation test               Automation
         configurations.                                                                          This pattern requires drivers and stubs for each layer. The driver for the top
      2. Perform Top-down Integration of layers. The resulting test suitesl                       layer should be designed so that it can exercise the entire system.
         drivers should be designed to be reusable/rerunnable for subsequent                          Verify that each successive test configuration is checked in and under ver-
         integration.                                                                             sion control before you begin developing a new configuration. A design change
      3. Use a top-down approach on the next layer. Remove stubs to second-                       or bug fix willlikely require revising and rerunning tests for configurations that
         level subsystems and implement the interfaces. Expand the control test                   have already passed. If the earlier configurations have not been saved, this
         suite to reach all newly implemented components and rerun the tests.                     process can be difficult with just the current configuration.




                                                                                                      Layer 1-2                        Layer2-3                      Layer 3-4
                                                                                                      Integration                      Integration                   Integration




                                                      .
  System                   Layer1           Layer2             Layer3             Layer4
  Architecture             Component        Component          Component          Component
                           Test             Test               Test               Test


 ~         Driver      ~      Componentundertest             Testedcomponent

 EJ Stub -                    Interfaceundertest      -      Testedinterface
                                                                                                  FIGURE
                                                                                                       13.26 Layer Integration, top-down   configurations.
 FIGURE13.25        Layer Integration, architecture and component test confjguration.

<!-- side 33 -->

690                                                                                                                                                                      691


EntryCriteria                                                                          Client/Server
                                                                                                  Integration
    .I The virtUal machine to be used in the test environment is stable. If the
       virtual machine is under development, consider Backbone Integration.            Intent
      .I The components of the collaboration are minimally operable. That is,
         they should have met the exit criteria for the appropriate component          Demonstrate the stability of the interactions among clients and servers. Begin
         scope test pattern.                                                           by testing clients and servers in isolation, then use controlled increases in scope
                                                                                       until all interfaces have been exercised.
      .I A physical, functional, and environmental audit has been conducted
         and has not found any anomalies that would interfere with integration
                                                                                       Context
         testing.
                                                                                      The ClientlServer Integration sequence achieves stepwise verification af the in-
ExitCriteria                                                                          terfaces between client components that are loosely coupled to a single server
      .I Each driven component meets the exit criteria for its test pattern.          component. Concurrent development and testing are possible.
                                                                                           The system under test is a variant of a clientlserver architecture. Unlike the
      .I Each collaboration that traverses twO or more layers has been ex-
                                                                                      top-down situation, no single locus of control exists. Servers react to client
         ercised once. Coverage analyzers will instrument and analyze call-
                                                                                      messages, and clients react to messages from the system's environment. Each
         pair traces, detailing exacdy which interfaces have and have not been        component of the system has its own control strategy. Components that have a
         exercised.
                                                                                      single locus of control may be integrated using Bottom-up Integration, Top-
                                                                                      down Integration, or Col/aboration Integration.
Consequences
                                                                                      FaultModel
The advantages and disadvantages of Layer Integration are similar to those of
Top-down Integration (for the top-down variant) and Bottom-up Integration             See Section 13.1.4, Integration Faults.
(for the bottom-up variant). The bottom-up variant requires drivers for each
layer and reduces the number of stUbs needed. The complete behavior cannot            Strotegy
be demonstrated until the top layer has been integrated. Nevertheless, the prox-
                                                                                      Test
                                                                                         Model
imity of drivers to the layer under test will impose the fewest obstacles.
     The top-down variant requires stUbs for lower layers but only a single           The test plan must identify clients and servers. Clientlserver interaction may be
driver. If the stack of layers is needed as a subsystem, this approach can provide    modeled with any appropriate test design pattern. Both Extended Use Case Test
a working interface for integration to the clients of the top layer at the earliest   and Round-trip Scenario Test will apply. In a two-tier star, the basic integration
 possible date. Nevertheless, the viability of the stack is not demonstrated until    strategy is to exercise client-server pairs. In a three-tier star, we exercise client-
 the lowest layer is integrated. Because layered applications are often used in       server and server-server configurations, followed by client-server-server con-
 time-critical applications, this delay may pose a risk in that the performance of    figurations.
 the stack will not be demonstrated until the end of the integration cycle.
                                                                                      Test
                                                                                         Procedure
 KnownUses                                                                            Each client is tested with a stub for the server. The server is tested with stubs
                                                                                      for all client types. Next, pairs of clients are tested with the actual server. The
 Scope-specific applications in object-oriented development are discussed later in
 this chapter. Protocol stacks are a widely used layered application. Holzmann        server may retain some stubs for other clients. Finally, all stubs are removed   and
                                                                                      the individual use case tests are replayed.
 provides an excellent overview of state-based testing strategies for such stacks
 [Holzmann 91].

<!-- side 34 -->

                                                                                                                                                                                            693
692                                                                              PART
                                                                                    III   Patterns


    Figure 13.27 depicts generic two-tier star. With a large system, it may not
be practical to integrate every unique client individually. Instead, client groups
can be identified. Members in a client group should have similar server inter-
faces. A two-tier clientlserver star integration follows a three-step procedure for
each client group:

      1. Representative client + server stub
                                                                                                        Stage 1: Star Server        Stage 1: Client Group 1       Stage 1: Client Group 2
      2. Server + client stub
      3. Representative client + server

Figure 13.28 shows the configurations that correspond to these stages.
     Client/Server Integration can also be applied recursively to three-tier or
n-tier client/server architecture. Figure 13.29 illustrates a generic three-tier star.


                                                                                                      Stage 2: Client Group 1       Stage 3: Client Group 2       Stage 4: All Use Cases
                                                                                                            Use Cases                     Use Cases
           Client Group 1
                                                                                                          13.28 Client/Server Integration, stoges ond confjgurations.
                                                                                                     FIGURE


                 \                                                                                   For example, the procedure for a three-tier clientlserver star integration using
                                                                                                     an object request broker is roughly as follows:

                                                                                                         1. Client group + ORB
                                                                                                         2. Server + ORB
                                                                                                         3. Client group + ORB + server proxy
       Server
                                                                                                         4. Server + ORB + client proxy
                                                                                                         5. Client + ORB + server (repeated for client groups)

                                                                                                         The test cases for a ClientlServerIntegration may be developed at system
                                                                                                     scope by using an appropriate responsibility-based test design pattern. Use the
                                                                                                     oracle for this pattern.

                                                                                                     Automation

                                                                Client Group 2                       The interface of the component under test will determine the kind of driver used
                                                                                                     (GUl capture/playback, APl, and so on). Test scripts developed for a stage
FIGURE13.27 Generic stor client/server orchitecture.                                                 should be designed to support two kinds of reuse. First, they should be repeat-
                                                                                                     able to support regression testing upon the integration of more components.

<!-- side 35 -->

                                                                                                                                                                      695
694

                                                                                     Client/Server Integration testing should be to reveal and correct interoperabil-
                                                                                     ity problems. Multiplatform integration requires a controllable test environ-
                                                                                     ment that spans all platforms in the target environment. Typically, however,
                                                                                     debugging and testing tools are platform-specific. Thus it may be difficult to
                                                                                     perform controlled testing and debugging of system threads that span several
                                                                                     platforms.
                                                                                          The initial test configuration can be a simplified version of the target envi-
                                                                                     ronment. Integration should not be considered complete, however, until all test
                                                                                     suites pass when run in a test environment that uses the same architecture,
                                                                                     middleware, and network opera ting system as the target environment. For ex-
                                                                                     ample, suppose that the target environment for the SUT is a three-tier architec-
                                                                                     ture supporting international e-commerce. Passing a test suite on a simplified
                                                                                     two-tier approximation of the target environment may be a useful first step, but
                                                                                     it should not be considered an adequate integration test of the application and
                                                                                     the target environment. An isomorphic test environment replicates the structure
                                                                                     of the architecture and the capabilities of the target environment, if not its scale.
                                                                                          Careful planning for integration test support is important. Choose one plat-
                                                                                     form to support your testing effort. Next, place your primary test repository on
                                                                                     this platform. Then, develop reliable interfaces from your test repository to the
                                                                                     test tools on each platform. If you have not already set up and tested this kind
                                                                                     of environment, allow extra time during integration testing to do so. Besides
                                                                                     typical testing tasks, this effort often requires defining, loading, and synchro-
                                                                                     nizing databases, securing access across gateways, bridges, and routers, and es-
                                                                                     tablishing name control. The coordination of production and test environment
                                                                                     typically becomes correspondingly more complex. A robust, multiplatform con-
                                                                                     figuration management system is indispensable.

FIGURE13.29 Three-tier client/server   star architedure.                             Entry Criteria

                                                                                          .I The virtual machine to be used in the test environment is stable. If the
                                                                                             virtual machine is under development, consider Backbone Integration.
Second, they should be extensible so that system scope tests may be added after
the final integration testing is completed.                                               .I The components of the collaborations to be tested are minimally opera-
    Verify that each successive test configuration is checked in and under ver-              ble. That is, they should have met the exit criteria for the appropriate
sion control before you begin developing a new configuration. A design change                component scope test pattern.
or bug fix willlikely require revising and rerunning tests for configurations that        .I A physical, functional, and environmental audit has been conducted
have already passed. If the earlier configurations have not been saved, this                 and has not found any anomalies that would interfere with integration
proces s can be difficult with just the current configuration.                               testing.
     Client/server systems are often multiplatform-based. For example, they may           .I A multiplatform test tool suite has been insta lied and shown to work.
use mainframe MVS servers, AIX servers, Windows clients, Solaris servers, Java
                                                                                          .I A multiplatform version control system has been installed and is being
clients running on any Java Virtual Machine, and so on. A primary goal of                    used.

<!-- side 36 -->

696                                                                          PART11/ Polterns                                                                                     697


ExitCriterio                                                                                     differences. This consistently eased automation of the generation and execution
                                                                                                 of regression tests. The scope of unit testing was a C++ class or class cluster. No
      .I Each driven component meets the exit criteria for its test pattern.
                                                                                                 particular strategy for unit test was reported. On completion of unit testing,
      .I The interface to each subcomponent has been exercised at least once.                    severallevels of integration testing were performed:
         Coverage analyzers are of limited use because they typically cannot in-
         strument call-pair traces over the scope of an entire system, but are in-                    ·   Single Unix process
         stead limited to the scope of the client or server process or a virtual
         machine running on a platform. Nevertheless, some effort should be
                                                                                                     ··   Intraplatform thread (e.g., several Unix processes)
                                                                                                          Interplatform thread (e.g., PClUnixlMVS)
         made to establish coverage of each end-to-end path.
      .I The test suite passes in a test environment that is isomorphic with re-
                                                                                                 Integration tests were devised by choosing external inputs that corresponded to
         spect to the target environment.                                                        specific threads of execution.
                                                                                                    Arnold et al. identified several key lessons learned from testing this system
Consequences                                                                                    [Arnold+94]. Class unit testing is relatively easy because test tools work well
The disadvantages of ClientlServer Integration are in the cost of driver and stub               with individual classes. Unit testing can therefore concentrate on compliance to
                                                                                                specifications.
development. Testers cannot exercise end-to-end use cases until midway or late
in the testing cycle.
     The advantages of ClientlServer Integration are that it avoids the problems                     ·Individual methods tend to have simple behavior, so they are testable.
                                                                                                    · Because the class interface is typically defined before the implementa-
of Big Bang Integration. The order of client and server integration usually has
few constraints, so integration can be sequenced according to priority or risk.                       tion is developed, the corresponding test drivers can be developed in
                                                                                                      parallel.
A ClientlServer Integration test suite provides a basis for system scope testing-
drivers and test cases can typically be reused and extended. The approach sup-                      ·    Test drivers developed for higher-level classes can usually be reused
ports controllable, repeatable testing.

KnownUses
                                                                                                    ·    with a few changes for lower-level classes in the same hierarchy.
                                                                                                         Test drivers can "grow" along with the application classes, thus en-
                                                                                                         abling iterative and incremental development.
The integration experience of Mead Data Central (MDC) illustrates Clientl
Server Integration [Arnold+94]. MDC offers an online, computer-aided re-                             On the other hand, unit testing could sometimes be problematic. Some
                                                                                                class-level obstacles to testability were identified:
search product for legal, financial, medicaI, news, and business topics. The sys-
tem must provide rapid search response times, high availability (24 x 7, except
for a short daily downtime window), accurate and repeatable results, and
                                                                                                    ··   Complex behaviors and component parts
                                                                                                         Attempts to reuse code outside of the contexts envisioned by the
timely print delivery. The target environment incorporates IBM mainframe sys-
                                                                                                         designer
tems under the MVS opera ting system plus many Unix systems.
    Testing was integrated with design and programming. Unit (class) white
box testing was a developer responsibility. A test package was developed for
                                                                                                    ·    Use of non-OO software for implementation with side effects due to
                                                                                                         diminished or nonexistent encapsulation
each class, shortly after the definition of the class interface. Each developer was
required to produce test components with the application class code. The test                        Integration testing (at all levels) was difficult. The compounded effect of
package included a driver for the class (with embedded test cases) and a file                   differences at alllevels proved challenging.
containing expected output.
     Early definition of public interface s permitted early development of test-
driver interfaces. Successive class versions tended to have more similarities than
                                                                                                   ·   The effectiveness of available commercial test tools diminishes on large
                                                                                                       systems (thousands of components).

<!-- side 37 -->

                                                                             PART'"    'R 13 Integration
698                                                                                                                                                                                        699


      .   Inconsistency across cIass libraries is problematic. The only way to re-
          move these inconsistencies is by adhering to standards enforced by de-
                                                                                          DistributedServicesIntegration

          sign and code reviews. Problems noted in this implementation included
          different approaches to encapsulation and implementation of over-              Intent


      .   loaded operators for collection classes.
          Many commercial classes were not thread-safe. That is, they were not
          explicitly reentrant, and controlof resources under concurrency was
                                                                                         Demonstrate the stability of interaction among loosely coupled peer compo-
                                                                                         nents. Begin by testing some nodes in isolation, then use controlled increases in
                                                                                         scope until all interfaces have been exercised.
          not addressed or was incorrect.
      .   Debugging and testing tools are platform-specific. lt was therefore diffi-
          cult to perform controlled testing and debugging of threads that crossed
                                                                                         Context

                                                                                         Distributed Services Integration is indicated when the system under test includes
      .   platforms (e.g., MVS and Unix).
          Impedance mismatch occurs betWeen 00 design and cIientlserver archi-
          tecture. Available cIientlserver interfaces or APIs may be inconsistent
                                                                                         many components that run concurrently with no single locus of control and
                                                                                         where there is no single hierarchy of servers.7


      .   with typical 00 cIass architecture.
          The DCE and C++ exception semantics are different. lt was difficult to
          perform controlled testing of exception handling.
                                                                                         Fou/t
                                                                                            Model

                                                                                         Integration testing is not concerned with all the ways in which a distributed sys-
                                                                                         tem may fail, but rather concentrates on those preventing effective execution of
    Recent test tool offerings have reduced the severity of these problems, but         system scope testing. The list developed in Section 13.1.4, Integration Faults,
adequate integration of a large cIientlserver system remains challenging.               covers many general kinds of problems that can occur.
                                                                                             Integration faults in distributed systems are often related to incorrect bind-
                                                                                        ing of servers and cIients. Leslie Lamport provides a qualitative hint of the kind
                                                                                        of faults that can occur in distributed systems: "A distributed computing sys-
                                                                                        tem is one in which your machine can be rendered unusable by the failure of
                                                                                        some other machine you didn't even know existed" [Communications of the
                                                                                        ACM, June 1992].

                                                                                        Strotegy
                                                                                        Test
                                                                                           Moc/el

                                                                                       Integration testing of a distributed system is concerned with verifying that inter-
                                                                                       faces among remo te hosts are minimally operable. Finding a test suite that gives
                                                                                       adequate confidence in the aggregate behavior of a distributed system is a much
                                                                                       more difficult problem.


                                                                                       7. Concurrenr,   distributed,   and decentralized   software   systems have been in use since the
                                                                                       early 1960s. Although some advocates of object-oriented technologies make much of "top-
                                                                                       less» architecture, with the exception of Ada 95, general-purpose    object-oriented program-
                                                                                       ming languages do not provide built-in support for distributed processing. Indeed, it is only
                                                                                       in the last five years that object request brokers and distributed object modeis have become
                                                                                       widely adopted and only by using ORBs and related middleware.

<!-- side 38 -->

                                                                                    PART11/       Integration                                                                        701
700

                                                                                                   The content of test cases for Distributed Services Integration may be devel-
    The worst case for interface verificationis that every node is connected to
                                                                                               oped at system scope by using an appropriate responsibility-based test design
everyother node and that each interface is unique-for example, with nodes A,
                                                                                               pattern. Internode collaborations may be modeled with any appropriate test de-
B, and C, the interfacesare AB, BA, BC, CB, CA, and Ae. If there are n nodes,
                                                                                               sign pattern. Both Extended Use Case Test and Round-trip Scenario Test will
then there are n(n - 1) interfaces.    If the directionality   of the interface   is irrele-
vant in all cases, then there are (n(n - 1))/2 interfaces. The scope of testing is             probably apply. Use the orade for the applicable pattern.
therefore proportional to the number of interfaces that must be operable to sup-
                                                                                               Test Procedure
port system testing.
    As there is no single locus of control, there are many sequences in which                  Although the general goals and strategy for integration testing of distributed
component interfaces may be tested. Here are a few options:                                    and nondistributed systems are much the same, controIling and observing com-
                                                                                               ponents on remote hosts can be time-consuming and difficult. When several dif-
     . Risk-driven. Begin with interfaces and components that are most                         ferent operating systems (or versions of the same OS) are added to the mix,
likely to be problematic. Flushing out expensive or possibly catastrophic                      there are often significant interoperability problems.
problems earlier will reduce the ris k that a major problem will necessitate a                     When a test does not run for a distributed system (as frequently happens),
schedule slip or cancellation at the last minute. The initial focus targets inter-             we want to find the cause quickly. Did it fai! because of a test environment con-
faces that depend on components, objects, middleware, or other resources                       figuration problem? A testware bug? A test case bug? Or (we hope) a bug in the
that (1) are individually unstable or unproven, (2) have not been shown to                     lUT? The folIowing principles will help in making this determination:
work together before or are known to be unstable, (3) implement complex
business rules, (4) have a complex implementation (e.g., are 10 times the code
size of all other components), or (5) have been subject to high churn during
                                                                                                    . Partition your test suite into separately executable test suites that are
                                                                                               hostlnode-independent and hostlnode-dependent. All test suites must be
development.                                                                                   automated and fully repeatable.
      . Risk-averse. Begin with interfaces and components that are least likely                     . Develop a configuration specification for each typical remote host
 to be problematic. Attempt to demonstrate minimal operability as soon as                      site. A typical remote host configuration represents a dass of possible re-
possible. If expensive or possibly catastrophic problems emerge later, the min-                mote hosts. During integration testing it is rarely necessary to exercise every
 imal capabilities can be shipped. The initial focus is on interfaces that depend              physical instance of a remote host, of which there may be tens of thousands.
 on components, objects, middleware, or other resources that (1) are individ u-                Nevertheless, each kind of host with a significantly different hardware/
 ally stable and proven, (2) have be6n shown to work together before or are                    software platform and significantly different allocation of application compo-
 known to be stable, (3) implement simple business rules, (4) have a low im-                   nents should be considered a distinct configuration. Develop scripts that will
 plementation complexity or size, or (5) are being reused with no change.                      reset each typical remo te host into a known stable state and that will set the
      . Dependency-driven. Begin with interfaces and components that can                       lUT components allocated to this host into a known stable state. These scripts
 be exercised in isolation or that depend on the fewest other components.                      should be separately executable.
 Establish their operability. Next, identify use cases that thi s set supports or                   . Verify that the testware   that implements   your test harness wil/ run on
 nearly supports. Incorporate the additional components to support these use                   all typical remo te hosts. Attempt to install and run it on each remote host in
 cases and run minimal use case tests. Continue in this manner until all inter-                your test environment. As simple as this process sounds, it is often a full-time
  faces have been exercised.                                                                   job for several people, taking several months. Do not underestimate the diffi-
      .   Priority-driven.   Begin with interfaces   and components      that are re-          culty of configuring your test environment.
quired to demonstrate high-priority capabilities (not a priority as relates to                      . Debug your test suite and testware     on the simplest   possible configura-
real-time task scheduling, but rat her system features that must be demon-                     tion, first. Try to exercise the basic distributed object interfaces on a single
strated to obtain project funding, for example). Establish operability of these                machine or a stable, homogeneous dientlserver configuration. This will ease
high-priori ty features. Next, select other feature sets or interface sets that will
exercise all other interfaces with the fewest tests.

<!-- side 39 -->

702                                                                            PARTIII                                                                                   703


debugging and result in a baseline test suite. Once your baseline test suite is          solid line); others use the Internet (doud). The one-at-a-time progression is sug-
stable for this configuration, you are ready to test additional hosts/nodes.             gested in Figure 13.31. Note that some stubs may be necessary. This diagram
                                                                                         also indudes a single driver, which shows that tests are controlled from one
    . Add typical remo te hosts one at a time to the test configuration.                 platform. Additional testware and drivers will probably be needed for each re-
Generate the lUT in a known stable state on each additional host and at-
                                                                                         mote host, but are not shown to simplify this diagram. At stage 6, all typical re-
tempt to rerun the host-independent baseline test suite. Then run host-
                                                                                         mote hosts have been verified, but some interfaces may not have been exercised.
dependent tests. Add host-specific tests as you go, but do not bundle them               Test cases should be added to cover any untested interfaces among remote
with the stable baseline.
                                                                                         hosts.
     . Stop when you have exercised all targeted interfaces and typical re-
mote host types. The goal of integration testing is to establish an lUT and
test environment that is sufficiently stable to support system scope testing.
Integration testing should not attempt to verify all functionality, distributed
fault tolerance, or performance under nominal and stress loads. It may be
useful to identify a minimum spanning tree [Deo 74] for the logical network
interfaces.

    For example, consider the architecture of the lUT shown in Figure 13.30.
Six typical remote hosts are modeled. Some have locally controlled interfaces (a

                                                                                                                                      Stage 2: Typical Remote Host C




                                                                                             Stage 3: Typical Remote Host D            Stage 4: Typical Remote Host E




                                                                           D
                                                                                             Stage5:TypicalRemoteHost F                  Stage6: All RemoteHosts
 Key:D      = Typlcalrerrote host,   O-   = Internet, -   = Localnetwo<k
                                                                                         FIGURE
                                                                                              13.31   Stagesand configurations
                                                                                                                             for integrationof distributedremotehosts.
 FIGURE13.30 Example distributed architecture.

<!-- side 40 -->

                                                                                                                                                                   70S
704

Automation                                                                           ExitCriteria
The considerations for dient/server test automation discussed in Client/Seroer           .I The responsibility-based exit criteria meet the requirements of the test
Integration apply. The interface of the component under test will determine                 patterns used to develop test case content. Because integration testing
what kind of driver can be used (GUl capture/playback, APl, and so on). Test                does not require completely exercising all features, the extent af the re-
scripts should be designed to support two kinds of reuse. First, they should be             sponsibilities used for integration test design should be limited to a
repeatable to support regression testing as more components are integrated.                 practical minimum.
Second, they should be extensible so that system scope tests'may be added after          .I The interface to each component on each typical remote host is exer-
the final integration testing is completed.                                                 cised at least once. Source code coverage analyzers are of limited use
     Several COTS tools are available that can generate an exact binary image               because they typically cannot instrument call-pair trace s over the scope
of an entire hard disk on a remote host. These utilities, which were originally             of an entire system; instead, they are limited to the scope of the dient
developed to support automated software installation, are also useful for set-              or server process or a virtual machine running on a platform. Never-
ting a remote host to a known stable state before running a test suite.                     theless, some effort should be made to establish coverage of each end-
     CORBA implementations using an ORB typically can enable filters that will              to-end path.
write a log as services are requested and fulfilled.                                     .I The test suite passes in a test environment that is isomorphic with re-
     Verify that each successive test configuration is checked in and under ver-
                                                                                            spect to the target environment.
sion control before you begin developing a new configuration. A design change
or bug fix willlikely require revising and rerunning tests for configurations that
have already passed. If the earlier configurations have not been saved, thi s        Consequences

proces s can be difficult with just the current configuration.                       The disadvantages of Distributed Services Integration are in the cost of driver
                                                                                     and stub development and establishing the entire test environment. Testers are
EntryCriteria                                                                        not able to exercise end-to-end use cases until midway to late in the testing
                                                                                     cyde. Establishing a stable test environment populated with typical remote
      .I The virtual machine to be used in the test environment is stable. If the
                                                                                     hosts can be difficult, expensive, and time-consuming.
         virtual machine is under development, consider Rackbone Integration.
                                                                                          The advantages of Distributed Services Integration are that it avoids the
      .I The components of the collaborations to be tested are minimally opera-      problems of Rig Rang Integration. The order of dient and server integration
         ble. That is, they should have met the exit criteria for the appropriate    usually has few constraints, so integration can be sequenced according to pri-
         component scope test pattern.                                               ority or risk. A Distributed Services Integration test suite pro vides a basis
      .I A physical, functional, and environmental audit has been conducted          for system scope testing-drivers, test cases, and the test environment can typ-
         and has not found any anomalies that would interfere with integration       ically be reused and extended. The approach supports controllable, repeatable
         testing.                                                                    testing.
      .I A multiplatform test tool suite has been insta lied and shown to work.
      .I A multiplatform version control system has been insta lied and is being     Known
                                                                                         Uses
         used.                                                                       Elements of this pattern can be found in many reports on testing of large sys-
      .I The typical remote host configurations have been specified, insta lied,     tems [Arnold+94, Vines 98].
         and tested.
      .I The testware of choice has been installed and configured on each
         remote host.

<!-- side 41 -->

      706                                                                                                                                                                 707


                                                                                            FoultModel
      High-frequency
                  Integration
                                                                                            See Section 13.1.4, Integration Faults. The implicit fault model is that new code
                                                                                            will most likely be buggy.
      Intent
      Integrate new code with a stabilized baseline frequently to prevent integration       Strotegy
      bugs from going undiscovered and to prevent divergence from the working, sta-         Test
                                                                                               Model
      bilized baseline.
                                                                                            Tests may be developed using any appropriate system scope test pattern:
                                                                                            Collaboration Integration, Round-trip Scenario Test, Covered in CRUD.
      Context
                                                                                                 McConnell argues for "srnoke tests" that "exercise the entire system from
      Rapid iterative incremental development can result in missing ar conflicting ca-      end to end. Ir does not have to be exhaustive, but it should be capable of ex-
      pabilities. The system under development might pass a minimal integration test        posing major problems. The smoke test should be thorough enough that if the
      suite, but new code is being rapidly developed. If a large volume of code is pro-     build passes, you can assume that it is stable enough to be tested more thor-
      duced without checking for such problems, making the necessary changes late           oughly" [McConnell96, 143]. These tests should correspond to collaborations.
      in a project is usually difficult and expensive. Frequent integration can prevent          The scope of the build effort and the integration must indude all compo-
      the unnoticed festering of such problems. The folIowing conditions are neces-         nents undergoing active development. If some developers do not participate the
      sary for beginning a High-frequency Integration regime:                               value of high frequency diminishes to the extent that the exduded code contains

            .   A stable increment is available; and some subset of the system under
                development can be run without trouble. This point of stability may
                                                                                            critical interfaces with the rest of the system.
                                                                                                 Ir should be noted that getting a dean compile, a dean link, and pass-
                                                                                            ing various static analyzers is not testing. High-frequency compiling and link-
                be reached very early with just a few dasses using a Top-down Inte-         ing is not High-frequency Integration. The lUT must be loaded and test suites


r
            .   gration pattern.
                Most meaningful functional increments can be produced within the fre-
                quency interval. That is, if the integration frequency is daily, it makes
                                                                                            must be executed. Expected and actual results must be compared.

                                                                                            TestProcedure

                sense to size development increments to code that can be produced in        High-frequency Integration has three main steps.
II!
 ,
 il
            .   one day by one programmer.
                The test suite is developed in parallel with code and kept current. The
                                                                                                 First, the developers produce code delta s to be integrated and associated
                                                                                            test suites. They perform the folIowing tasks:
                development of the integration test suite cannot be deferred. Instead, it
                must be available for the first integration cyde and maintained for each        ..    Write or revise the code, working on a private branch.
                                                                                                      Write or revise a test suite for the code.
            .   succeeding cyde.
                High-frequency Integration must be automated. There must be a reli-
                able means for running and rerunning the integration test scripts. This
                                                                                                 ..   Desk check, review, or inspect the changed component.

                goal can be accomplished with commercially available GUl capture/
                playback tools, development of test driver dasses, or more exotic test
                                                                                                  .   Desk check, review, or inspect the test suite.
                                                                                                      Run all forms of static analysis and resolve all errors. Check in the
                                                                                                      changed component so that the configuration management system will
                harnesses.
            .   A configuration management tool must be installed and routinely used.
                                                                                                      search for conflicts with other versions or change packages. Run any
                                                                                                      other applicable source code analyzers.
                Without effective configuration management, the proliferation of ver-
                sions in high-frequency development will become unmanageable.
                                                                                                ·.    Compile and build the changed code.
                                                                                                      Run the test suite. Use a memory leak detector, if applicable.


  I

<!-- side 42 -->

708                                                                                                                                                                           709
                                                                              PART
                                                                                 11/ PoHerns


      ·    When the component passes all tests, check in the revised code and test
           suite to the development integration branch.
                                                                                               Automation
                                                                                               The daily build is often run overnight by script or batch job-for example, a
                                                                                               Unix cron(l) job. The entire test suite (and the merge and build cornmands)
     Second, the build tester aggregates the developer changes and runs an inte-               must be automated to support frequent repetition. The ability to report a no
gration test suite:                                                                            pass test and continue is necessary for this kind of testing. Because the test

      ·    At an agreed-on regular interval, the person responsible for running the
           integration stops accepting deltas and builds a new system. If asserrions
                                                                                               suites often run unattended overnight, the ability to continue after a no pass re-
                                                                                               sult or crashed application is critical. Thus, the test tools must be able to rerun
                                                                                               existing tests as well as newly developed tests.

      ·    have been developed (see Chapter 17), they should be enabled.
           When the build is completed successfully, the test suites are run. This
           testing will ind ude smoke tests, newly developed tests, and as many
                                                                                                   Verify that each successive test configuration is checked in and under ver-
                                                                                               sion control before you begin developing a new configuration. A design change
                                                                                               or bug fix willlikely require revising and rerunning tests for configurations that
           existing test suites as time permits.                                               have already passed. If the existing configurations have not been saved, this
                                                                                               process can be difficult with just the current configuration.
     Third, the results are evaluated. McConnell comments: "Fix any problems
immediately! If the build team discovers any errors that prevent the build from                EntryCriteria
being tested (that break the build), it notifies the developer who checked in the
code that broke the build, and that developer fixes the problem immediately.                   Several conditions should be achieved before attempting a High-frequency
Fixing the build is the project's top priority" [McConnell 96].                                Integration process.
     An effective High-frequency Integration proces s will have a workable reso-
lution for the folIowing process issues:                                                           .I The virrual machine to be used in the test environment is stable.

      ·    Who is responsible for maintaining the existing test suite? As the
           system evolves, some test suites will become obsolete. Some will be
                                                                                                   .I An automatically repeatable test suite has been developed for the build
                                                                                                      and the test harness to execute it has been developed and tested.
                                                                                                   .I High-frequency Integration may begin when there are enough compo-
           "broken" because the lUT interface will change, but the functionality                      nents to integrate. Typically, this stage corresponds to one complete
           will not.
      ··   What is the build cyde? What is the deadline for accepting a change?
           Who may run the build and the integration test? Under what circum-
                                                                                                      collaboration or completion of the high-level control objects.
                                                                                                   .I The components are under automated configuration management con-
                                                                                                      trol and a successful build can be generated from this configuration.
           stances ?
      ·    What follow-up actions are taken when the build fails due to a link
           error or a test does not pass? To which version will the system revert?
                                                                                                   .I Clear, unambiguous criteria and procedures for adding a component
                                                                                                      to the build have been established, verified, and communicated to all
                                                                                                      interested parties.
           How will problem determination be done and how will a person or or-                     .I A cutoff time for the cyde is established. Ir should be at the same time
           ganization be assigned the task of correcting the problem?                                 (hour, day, week) to set the rhythm that is essential to this pattern.

    The test cases for High-frequency Integration may be developed at system                       Once the proces s has started, the procedure for adding componenrs to the
scope by using an appropriate responsibility-based test design pattern. Use the                build must be followed by all developers.
orade for this pattern. Smoke tests typically do not require an orade or evalu-
ation, because if the lUT runs to normal termination, the smoke test is consid-                    .I The submirred component should pass a component scope test suite.
ered passed.
                                                                                                   .I The submirred component must adhere to the configuration control
                                                                                                      procedures for the build.

<!-- side 43 -->

                                                                                PART11/                                                                                        711
710                                                                                          Integration


      .I The submitted component must adhere to the established procedures                    A test suite that passes all or most of the time suggests that the Pesticide
         for check-in, timeliness, and so on. If the procedures do not work,              Paradox may be in effect [Beizer 90]. A test suite that passes has eliminated all
         change them. Do not, however, dilute their effectiveness by waiving              bugs that it can, but certainly not all bugs. If no explicit adequacy criteria have
         them on an ad hoc basis.                                                         been established for high-frequency testing, a long string of successful builds
                                                                                          may lead to unwarranted confidence. The test suite used for High-frequency
ExitCriterio                                                                              Integration should be revised to keep pace with new functionality developed for
                                                                                          the lUT. Chapter 15 discusses considerations for test suite maintenance.
High-frequency Integration ceases when the development increment is com-
pleted and the SUT passes the integration suite for the last increment. Two pos-          Advanfages
sible cases exist.
                                                                                          High-frequency Integration offers significant advantages.
      .I The High-frequency Integration test suites have been designed to
         achieve integration coverage. That is, the build tests ind ude tests that
                                                                                              ·   Ir requires   a commitment   to developing   and maintaining   both source
                                                                                                  code and a test suite. The production of test suites and the production
         would be developed using Top-down Integration, Bottom-up Integra-
                                                                                                  of code are of equal importance. This is an effective bug prevention
         tion, Collaboration Integration, Client/Server Integration, and so on.
         If the final test suite also meets the exit criteria for such a pattern, then
         the SUT is ready for system testing.                                                 ·   strategy.
                                                                                                  Gross errors, omissions, and incorrect assumptions are ofren revealed
                                                                                                  early. The process "exposes the real (vs. the imagined) dependency tree"
      .I The High-frequency Integration test suites have been designed to
         demonstrate minimal interoperability, but have not attempted to
         achieve integration coverage. Because the SUT has been exercised,                    ·   [McCarthy 95, 112].
                                                                                                  Because the most recent additions to the build are most likely to be as-
         it may actually be a simple matter to achieve an appropriate integra-
         tion coverage. Collaboration Integration coverage will probably work
         best. When these exit criteria are met, then the SUT is ready for system
                                                                                              ·   sociated with failures, debugging is usually easier.
                                                                                                  The entire development team is focused on producing a system that
                                                                                                  works, instead of working on a system. This approach will improve
         testing.
                                                                                              ·   morale, as the team can see tangible results early.
                                                                                                  Although some stubs may be necessary, none are explicitly required.
Consequences

Oisadvanfages                                                                                 ·   This approach avoids brittle and finicky test code.
                                                                                                  Development and integration testing can overlap to a high degree if the
                                                                                                  test suite indudes more than simple smoke tests.
High-frequency Integration requires a commitment to developing and main-
taining both source code and a test suite (this requirement is also an advantage).
                                                                                          Known
                                                                                              Uses
The extra effort is why most users of this pattern stick to simple test suites.
These test suites have questionable effectiveness, especially if the test suites are      Rettig's
                                                                                                Group
produced by developers who must pay a significant penalry when they "break"               An "incremental glass box" testing process for Smalltalk development inte-
the build.
                                                                                          grated new code as often as hourly [Rettig 91]. The test goals were to test every
     Attempting High-frequency Integration will reveal problems in both the               path once, develop and use reusable test procedures, perform all testing under
process and procedure s, as well as the lUT. The initial cydes are not apt to go          peer review, and reduce the tedium of testing. Programmers were respons-
smoothly. Identify and resolve any process problems quickly. The painful evo-             ible for developing executable test plans, which were reviewed using a detailed
lution of the now-fabled daily build process for Windows NT provides an in-               test standard. A dass TestManager was developed to execute a block of code
structive case study in such evolution, which spanned three years [Zachary 94].           and to complain in the event of an unexpected response. Ir had several out-
                                                                                          put modes: log to a file, display on a window, start the debug tool. A

<!-- side 44 -->

                                                                            PARTII'      Integration                                                                               713
712

TestProcedure method was developed for each application dass. It sent many                A complete build of the project was performed once a week. After a programmer
                                                                                          finished a cIass, the cIass was compiled and linked against the previous build, and
messages to TestManager. The tests were run by entering                                   then integrated into the next complete build. Although developers lost time finding
                                                                                          defects on the weekly build day, this potential for lost time created a strong incen-
      TestManager   testClass:   ClassUnderTest                                           tive to avoid defects within the development ranks since no one wanted to be
                                                                                          known as the person who held up a particular week's build [Berg+95, 58].
     Most dasses "involved no more than one or tWo days of coding effort, fol-
lowed by one or two days of writing test procedures. . . . When we complete a         AT&T
[dass] and integrate the new code with the official version of the prototype, the     The Named Stable Bases pattern advocates High-frequency Integration and
TestManager is invaluable." A master procedure checked all dasses, which fa-          cites its appJication in an AT&T project that "had a nightly build (which was
cilitated testing of the entire system at any point in development. This test suite   guaranteed on]y to compile), a weekly integration test build (which was guar-
was rerun as soon as a dass and its test procedure were complete.                     anteed to pass system-wide sanity tests), and a (rough]y biweekly) service test
     The team produced roughly 50,000 lines of Smalltalk. Thirty percent of the       build (which was considered stable enough for QA's system test)" [Coplien+95,
total effort went into design and test case development, 33 percent into coding,      224-225].
and 33 percent into test and integration. In the course of testing, "we have had
less than a dozen errors slip through the testing process and appear in a subse-      ExtremeProgromming
quent mod~le. In my experience, that is remarkable" [Rettig 91, 28].
                                                                                      Extreme Programming is a highly iterative deve]opment process that requires
                                                                                      (among other things) that executable test cases be developed with each dass.
WindaM NT 3.0
                                                                                      High-frequency Integration, conducted several times per day, is an integral part
At its release, Microsoft Windows NT 3.0 consisted of 40,000 source files con-        of the approach. It has been deployed for severallarge Smalltalk applications.
taining 5.6 million lines of C code. In response to chronic integration problems
during its development, project manager David Cutler focused his attention on            On C3 payroll, we use a practice we call Continuous Integration. Developers break
the build process and bug prevention. "His cure for bad check-ins called for             their work down into tasks requiring about a day to do. They start from the cur-
brute force. He himself would become a sentry for qua lity, reviewing as many            rendy released software, make their changes, run the tests, and integrate the
                                                                                         changes into the currendy released software. We do that multiple times a day. It's
of the seventy-five to one hundred daily check-ins. . . . No amount of rhetoric          like the daily build times ten. The good news, of course, is that if you co uld make
could equal the example of Cutler's actions. 'H I'm in the build lab, that tells         this work, your big bangs become litde bangs, and litder and litder. Since we use
[the developer] I better not check-in ****.' " [Zachary 94, 181]. A complete             Smalltalk, we can integrate in seconds rather than overnight. Since we have about
build took as many as 19 hours on several machines, but a new build was pro-             30,000 Unit Test checks, and a few hundred functional tests checking the whole sys-
                                                                                         tem, we can be really sure that we didn't break anything. So when we release, we
duced every day. The process and the system stabilized after a year, and Cutler
                                                                                         require that all the tests run perfecdy, that is, that ZERO defects are shown. . . . Of
moved out of the build lab. "White's group turned out on average a build a day,
                                                                                         course, we couldn't start this in the middle. But on the first day, it's not hard to
seven days a week. Changes in the build were usually limited to bug fixes.               release with all your tests at 100%. There aren't many. You just repeat every
 Check-ins now assumed a ritualized pattern" [Zachary 94, 245]. Further com-             day. . . . we have over 2,000 clas ses, 30,000 methods, and we've been doing this for
 mentary on thi s project and the institution of the daily build model at Microsoft      2'h years with berter results than any of us have ever seen before [Ron Jeffries,
 is offered in [McCarthy 95] and [McConnell 96].                                         posted to the comp.software-eng      newsgroup, December 6, 1998].


 05/400                                                                                    A published account of the C3 project appears in [Anderson+98]. Due to
                                                                                      the nature of Smalltalk development environments, thi s kind of integration ac-
 A weekly build regimen was used in development of IBM's OS/400 opera ting            tually imports all other changes into each developer's workspace, rather than
 system, which induded 14,000 C++ dasses and 20 million lines of C and C++            exporting changed code into a common repository.
 code.

<!-- side 45 -->

714                                                                    PART
                                                                          11/ Potterns



13.3 Bibliogrophic
                Notes

Most of the Known Uses reports first appeared in [Binder 96i].
    Myers summarizes and compares bottom-up, top-down, sandwich, and big
bang integration [Myers 76]. An expanded analysis of Top-down Integration                 Chapfer
                                                                                                14 ApplicafionSystems
and Bottom-up Integration appears in [Myers 79]. Integration strategies corre-
sponding to the architecture of modular systems are presented and compared in
[Yourdon+79]. Beizer compares Bottom-up, Top-down, and Big Bang Integra-
tion and presents the backbone approach [Beizer 84]. McCabe presents a call
path model based on analysis of intramodule and intermodule control flow and                                                         The whole is more thon the sum of its parts.
suggests a call path coverage for integration [McCabe+89]. Beizer reviews con-                                                                                           kistotfe
siderations for integration testing [Beizer 90].
     The OaSE methodology offered the first practical approach to integration
in object-oriented systems Uacobson 92]. An advanced approach to dependency
analysis and some implications for testing are presented in [Lieberherr+92].                   Overview
Graham outlines a uses-based, bottom-up integration strategy [Graham+93].
Marick argues that testing should be done at (small) subsystem scope, implying                This chapter presents three pattems for designing an application test
Big Bang Integration for subsystem components [Marick 95]. A general ap-                      suite from usa cases. Because use cases are oota completemodel
proach to integration appears in [Siegel 96]. Firesmith's Test Stubs and                      af system capabilities, additional test strategie$ for implementatiol1-
Dependency-Based Testing patterns address integration test issues [Firesmith                  specifk capabilities are autIined.
96]. Lakos's analysis of C++ dependencies is an excellent guide to design-for-
testability and integration test planning [Lakos 96].

                                                                                         14.1 TestingApplicotionSystems

                                                                                         Effective testing at system scope requires a concrete and testable system-level
                                                                                         specification. System test cases must be deri ved from some kind of functional
                                                                                         specification. Traditionally, user documentation, product literature, line-item
                                                                                         narrative requirements, and system scope modeis have been used. Use cases,
                                                                                         augmented for testing, provide much of the information (but not all) needed to
                                                                                         develop a complete system test suite. A complete system test suite also requires
                                                                                         the kind of tests outlined in Section 14.3, Implementation-specific Capabilities.


                                                                                         14.1.1 A Cautionary
                                                                                                           Tale

                                                                                         The attorneys reminded Polly Morphic of the chorus in a Greek tragedy. A year
                                                                                         ago, Abraxas's International Funds Transfer System (IFTS) had been the hot-bet
                                                                                         technology among the masters of the universe, now represented by these gray


                                                                                                                                                                            715

