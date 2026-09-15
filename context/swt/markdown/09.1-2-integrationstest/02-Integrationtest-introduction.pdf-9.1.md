---
title: "Integrationtest introduction.pdf (9.1)"
source: "csfiles/home_dir/IntegrationTest/Integrationtest introduction.pdf"
modul: "Lektion 09.1+2: Integrationstest"
pages: 55
type: "slides"
vision: "done"
---
# Integrationtest introduction.pdf (9.1)

<!-- side 1 -->

INTEGRATION TESTS
INTRODUCTION
I4SWT




AARHUS
UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 2 -->

INTEGRATION TEST



                                             Requirements                                                                             Accept testing

                                          System specification                                                             System testing

                                                              System design                                   Integration testing

                                                              Component design                     Unit test


                                                                         Implement component


  AARHUS
  UNIVERSITY                                                                     SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** V-model tegnet som to skrå grene med bokse. Venstre nedadgående gren (udvikling), oppefra og ned: Requirements, System specification, System design, Component design, og i bunden Implement component. Højre opadgående gren (test), nedefra og op: Unit test, Integration testing, System testing, Accept testing. Dobbeltpilede vandrette pile forbinder hvert modsvarende par: Requirements ↔ Accept testing, System specification ↔ System testing, System design ↔ Integration testing, Component design ↔ Unit test. Et lyseblåt fremhævningsfelt ligger hen over rækken System design ↔ Integration testing og markerer lektionens fokus. En skrå pil løber ned gennem venstre gren til Implement component, og en tilsvarende skrå pil op gennem højre gren ud over Accept testing.

<!-- side 3 -->

INTEGRATION TEST IN AN ITERATIVE PROCESS




                                                        IT    IT   IT    IT             IT                 IT                IT




  AARHUS
  UNIVERSITY                                                            SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Iterativ udviklingsproces tegnet som en grøn zigzag-kurve over otte iterationer. En rød pil "Initial planning" peger ned i starten af kurven; en rød pil "Final delivery" peger op og ud i slutningen. To vandrette stiplede niveaulinjer markerer (nederst) Internal deliveries og (øverst) External deliveries. Hvert zigzag-toppunkt er mærket "Deployment evaluation" — toppene i iteration 3 og 6 når op til External deliveries-linjen, de øvrige (iteration 2, 4, 5, 7) stopper ved Internal deliveries. Dalbunden i hver iteration er mærket Implementation. Faseetiketter langs første iterationer: Requirements analysis, Design, Implementation, Testing, Iteration planning. En mørkeblå cirkel med teksten IT (integration test) er placeret på kurven i hver af de otte iterationer, dvs. integrationstest udføres i hver iteration. Nederst en tidsakse opdelt i Iteration 1 … Iteration 8.

<!-- side 4 -->

PURPOSE, AIM




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 5 -->

PURPOSE, AIM
  The purpose of integration test is to test the interactions and interfaces between several modules

  The aim is to verify correct interaction of the tested modules
    • Classes
    • Packages
    • Components

  Additionally – the interaction between the low level modules (HW drivers) and the actual
  hardware: that is hardware-software integration

  Additionally – the interaction between the subsystems and to the external systems: that is
  system integration

  Verification requires 100% interface coverage – is hard to measure and can be hard to obtain


  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 6 -->

PREREQ’S FOR STARTING INTEGRATION TEST
  ✓Unit testing of all modules is complete


  ✓System architecture (dependencies) is known


  ✓Integration test plan is defined
         • Integrated modules
         • System Under Test (SUT) structure?
         • Test fixtures / environment
         • Test cases – SUT stimuli and expected responses

  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 7 -->

BEING SMART ABOUT INTEGRATION TESTS
  Integration tests are linked to project ”heartbeat”
         • Integration testing usually requires input from several partners, so test
           planning becomes key.
         • Large-scale component integration (at least) at the end of each iteration.
  Integration tests requires knowledge of system architecture to partition the
  system into testable chunks

                                                              A strategy!
                                                              And a plan!

  AARHUS
  UNIVERSITY                                                     SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 8 -->

GETTING READY – MAPPING THE DEP. TREE
  Integration test planning is helped along using a dependency tree
         • Depicts inter-module dependencies in a tree-like structure
                                                                                                                                             A
         • Does not depict an inheritance hierarchy, layering or the like
         • Some dependencies are obvious from sequence diagrams, object diagrams, state charts, etc.
                                                                                                                       B              C              D
         • Others require inspection (members, parameter types, ..)
         • Loops must be broken using stubs                                                                        E           F      G          H       I

                                                                                                                           A depends-on B, C and D
                                                                                                                           B depends-on E and F
                                                                                                                           C depends-on F and G
                                                                                                                           D depends-on H and I




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Dependency tree til højre på siden: rodknuden A øverst med kanter ned til B, C og D. Fra B kanter ned til E og F; fra C kanter ned til F og G (F er altså delt mellem B og C); fra D kanter ned til H og I. Blade i nederste række, venstre mod højre: E, F, G, H, I. Alle knuder er afrundede rektangler, kanterne er uden pilehoveder.

<!-- side 9 -->

DEPENDENCY TREE – WHAT?
  The Dependency Tree must reflect the dependencies in the REAL code!
  It shows the real classes, it does NOT show the interface classes!
  However – the connections between the real are of course interfaces




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 10 -->

DT DESIGN RULES
  Dependency tree is a new type of diagram
  DT is not an (official) UML diagram type
  Dependencies always goes from the bottom of the dependent module to the top
  of the one it is dependent on!
  That way arrows are not needed
  Dependencies never goes sideways (horizontal)
  Move modules down to avoid this
  Show loops as loops!



  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 11 -->

STEP 1 – FIND THE DEPENDENCY TREE!
  Class diagrams (static information)
         • Remember polymorphic classes
         • Remember interfaces and base classes
                                                                                    Test cases for units
                                                                                          • Which interfaces were defined
  Sequence diagrams (behavioral information)
                                                                                          • Which IFs were faked
  Search for usage of base classes
                                                                                    Call trees? Manually or automated tool?
         • They may be used implicitly, e.g. as parameters
                                                                                          • Reveals "tool classes", objects passed around
         • Include also all derived classes!
                                                                                    Dependency tree generator tool?
  Search for usage of interfaces
                                                                                          • Visual Studio Code Maps a.o.
         • They may be used, e.g. as parameters

         • Replace them with all implementations!




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 12 -->

DEP. TREE TRAP – BIDIRECTIONAL ASSOCIATION


                                               Classes with bidirectional association
                                                          (class diagram)


                                              A                                    B
                                                                                              • Choose the one that fits the flow of the system
                                                         Which depends on which?
                                                                                                best, or gives the best integration plan
                                                          (dependency diagram)


                                                                                              • In some steps, you may need both the real and a
                                        A                                      B                fake version of one of the classes

                                                                                              • Don't panic – just do it!
                                        B                                      A




  AARHUS
  UNIVERSITY                                                                            SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 13 -->

DEP. TREE TRAP – EVENTS
                                                                                            • C# events have bidirectional associations
                                  Classes with event connection (class
                                                                                                 • Subscribing to an event, the handler must know of
                                                diagram)                                           the event
                             Button
                                              buttonEvent
                                                          myButton
                                                                                Handler          • Emitting an event, the event must know of the
                                           Which depends on which?
                                                                                                   handler to notify it.
                                            (dependency diagram)

                                                              Typical choice for a Button
                                                               – for other classes with     • Timers are uncontrollable dependencies and we wish to
                                                              events it may be different
                      Button
                                                                                              defer testing them as long as possible
                                                                                                 • At some point we have to test them, but this may
                                                                      Handler

                                                                                                   take time or may not be feasible
                     Handler
                                                                      Button




                                                                                            • Often one of the directions is only for setting up the
                                                                                              event connection; that is often the least important one

  AARHUS
  UNIVERSITY                                                                                 SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 14 -->

DEP. TREE TRAP – INHERITANCE – DEP. INVERSION
                           Class diagram with inheritance
                                      hierarchy


                RouletteGame
                                   currentBets    *
                                                                  IBet
                                                                                                                 • Integration planning concerns the integration
                                      1

                                                                                                                   of the real code.

                                                                                                                 • Dependency tree must show the dependency
                                      FieldBet                  ColorBet            OddEventBet                    between the actual code as implemented!

                                                                                                                 • Not between abstract interfaces!
                                                      Dependency Tree


                                                      Roulette-
                                                       Game


                                                                                                                 • Often this is reflected in the inversion of the
                              FieldBet                ColorBet                 OddEvenBet
                                                                                                                   inheritance hierarchy from the class diagram
                                                                         This dependency only necessary if
                                                                               IBet contains any code
                                                                                                                   to the dependency tree
                                                         IBet




   AARHUS
   UNIVERSITY                                                                                                SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
   D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 15 -->

DEPENDENCY TREE EXAMPLE
  Roulette game
  https://gitlab.au.dk/au-ece-swt/roulettegamewithintegration




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 16 -->

INTEGRATION TEST PATTERNS
  Integration test patterns are used to plan and execute the integration tests.


  This session covers the following patterns
         • Big Bang Integration
         • Bottom-up Integration
         • Top-down Integration
         • Collaboration Integration
         • Sandwich Integration




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 17 -->

SAMPLE DT FOR THE FOLLOWING SLIDES
                                                                                             A                                      L


                                                                  B       E                                              H          M


                                                              C       D   F                          I                        J     N


                                                                          G                                                   K     O

                                                                          HW



  AARHUS
  UNIVERSITY                                                                   SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 18 -->

LEGEND FOR THE DIAGRAMS

                                 System                       SUT – All the modules that
                                 Under                             take part in this
                                  Test                             integration step                                                               IUT – the Interface
                                                                                                                                                      Under Test
                                                              The Top Module that the                                                            Interfaces already
                                      T                       Test Driver is interacting                                                          (partially) tested
                                                                        with
                                                                                                                                                Test Driver – the test cases
                                                                  Any other module                                               D                and code that tests the
                                      X                             included in this                                                                      System
                                                                   integration step
                                                                                                                                                A Stub – a fake stub or
                                                                                                                                 S
                                                                                                                                                mock that supports the
                                  Other                         Modules not currently                                                                    test
                                 Modules                        taking part in the test



                                                                Modules not currently
                                      S                        taking part in the test –
                                                                    but stubbed

  AARHUS
  UNIVERSITY                                                                               SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 19 -->

BIG BANG INTEGRATION – FIRST, LAST AND
ONLY STEP!
                                                                       D                                                        D




                                                                       T                                                        T




                                         X                        X                             X                               X


                                                                                                                                    D




                                    X                         X   X           X                     X                           T




                                                                  X                                 X                           X



                                                                  HW
  AARHUS
  UNIVERSITY                                                               SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 20 -->

BIG BANG INTEGRATION




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Fem håndskrevne post-it-lignende sedler i to farvegrupper. Blå (ulemper), fire stk.: "Fire it up, see it fail"; "Works (sometimes) for small, low-complexity, stable, systems"; "Only possible late in development – errors costly to fix"; "Very little feed-back"; "Very low probability of detecting errors". Gul (neutral/betinget), én stk. adskilt til højre: "Works (sometimes) for small, low-complexity, stable, systems".

<!-- side 21 -->

BOTTOM-UP INTEGRATION – FIRST STEP




                                                              D



                                                                  T



                                                                  HW
  AARHUS
  UNIVERSITY                                                           SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 22 -->

BOTTOM-UP INTEGRATION – STEP 2




                                                              D                                                    D            D



                                                                  T                                      T                  T




                                                                  X                                      X                  X



                                                                  HW
  AARHUS
  UNIVERSITY                                                           SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 23 -->

BOTTOM-UP INTEGRATION – LAST STEP

                                                                       D                                                            D



                                                                           T                                                    T




                                                  X               X                                      X                      X




                                             X                X   X                  X                       X                  X




                                                                  X                                          X                  X



  AARHUS                                                          HW
  UNIVERSITY                                                           SPRING 2025       LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 24 -->

BOTTOM-UP INTEGRATION




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Håndskrevne sedler i tre farvegrupper. Blå (ulemper): "Requires many drivers at different levels"; "Postpones test of critical control component interfaces". Gul (neutral): "Reflects very 'engineering-like' mindset". Grøn (fordele): "No (few) stubs to develop"; "Easy to cover interfaces at all levels".

<!-- side 25 -->

TOP-DOWN INTEGRATION – FIRST STEP

                                                                           D                                                        D




                                                                           T                                                        T




                                              X                       X                               X                             X



                                          S                   S       S                  S                S



                                         S                        S   S              S                        S




  AARHUS
  UNIVERSITY
                                                                      HW       SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 26 -->

TOP-DOWN INTEGRATION – STEP 2
                                                                              D                                             D




                                                                              T                                             T




                                                      X           X                                     X                   X




                                                 X            X   X                  X                      X



                                                                  S                                          S



                                                                  S                                         S



  AARHUS                                                          HW
  UNIVERSITY                                                           SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 27 -->

TOP-DOWN INTEGRATION – LAST STEP
                                                                       D                                                    D




                                                                       T                                                    T




                                             X                    X                             X                           X


                                                                                                                            D




                                        X                     X   X           X                     X                       T




                                                                  X                                 X                       X



  AARHUS                                                          HW
  UNIVERSITY                                                           SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 28 -->

TOP-DOWN INTEGRATION




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Håndskrevne sedler i tre farvegrupper. Blå (ulempe): "Hard to exercise low-level interfaces from the top". Gul (neutral): "Needs lots of stubs (OK with isolation framework)". Grøn (fordele): "Early feedback on controller components"; "Facilitates concurrent HW and SW development".

<!-- side 29 -->

COLLABORATION INTEGRATION –
USE CASE 1
                                                                                                                            D




                                                                                                                            T



                                                                                                                            S



                                                                                                   X                        S




                                                                                 X                     X




                                                                                                       X




  AARHUS
                                                              HW
  UNIVERSITY                                                       SPRING 2025       LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 30 -->

COLLABORATION INTEGRATION –
USE CASE 3
                                                                               D




                                                                               T



                                                                  S                              S


                                         S                                X                                  S


                                                                      S



                                                              S           X




                                                                          X



                                                                          HW
  AARHUS
  UNIVERSITY                                                                       SPRING 2025       LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 31 -->

COLLABORATION INTEGRATION –
USE CASE 5



                                                                                                                        D



                                                                                                                        T




                                                                                               X                        X




                                                              HW
  AARHUS
  UNIVERSITY                                                       SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 32 -->

COLLABORATION INTEGRATION –
LAST STEP – FILLING IN THE HOLES
                                                                                   D




                                                                       T                                                        X




                                         X                        X                              X                              X




                                    X                         X   X            X                     X                          X




                                                                  X                                  X                          X



  AARHUS
  UNIVERSITY                                                      HW       SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 33 -->

COLLABORATION INTEGRATION




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Håndskrevne sedler i tre farvegrupper. Blå (ulemper): "Hard to exercise low-level interfaces"; "Participants not exercised separately". Gul (neutral): "Needs lots of stubs (OK with isolation framework)". Grøn (fordele): "Intuitive for users (may follow use cases)"; "Especially useful for higher-level system tests (component, subsystem)"; "Models iterative development with UCs as unit".

<!-- side 34 -->

SANDWICH INTEGRATION – STEP 1
COULD BE HW INTEGRATION




                                                              D



                                                                  T



                                                                  HW
  AARHUS
  UNIVERSITY                                                           SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 35 -->

SANDWICH INTEGRATION – STEP 2 – NEXT
LEVEL UP



                                                              D                                             D                   D



                                                                  T                                T                        T




                                                                  X                                X                        X



                                                                  HW
  AARHUS
  UNIVERSITY                                                           SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 36 -->

SANDWICH INTEGRATION – STEP 3
PERHAPS START FROM THE TOP SIMULTANEOUSLY
                                                                               D                                                    D




                                                                               T                                                    T




                                                  X                   X                                      X                      X



                                              S               S       S                      S                   S



                                              S                   S   S                  S                           S




  AARHUS                                                              HW
  UNIVERSITY                                                               SPRING 2025       LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 37 -->

SANDWICH INTEGRATION - STEP 4
ONE MORE LEVEL DOWN
                                                                       D                                                        D




                                                                       T                                                        T




                                           X                      X                               X                             X


                                                                  S                                    S



                                      X                       X   S             X                      S




  AARHUS                                                          HW
  UNIVERSITY                                                               SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 38 -->

SANDWICH INTEGRATION – LAST STEP
MAKE THE BOTTOM AND TOP MEET
                                                                       D                                                        D




                                                                       T                                                        T




                                         X                        X                              X                              X




                                    X                         X   X            X                     X                          X




                                                                  X                                  X                          X



  AARHUS                                                          HW
  UNIVERSITY                                                               SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 39 -->

SANDWICH INTEGRATION




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Håndskrevne sedler i to farvegrupper. Gul (neutral): "Takes lots of planning". Grøn (fordele): "The best of top down and bottom up"; "Many of the disadvantages of TD and BU are alleviated".

<!-- side 40 -->

INTEGRATION PLAN
  Goals:
         • Exercise all real interfaces between modules
         • Work towards having a complete system
  Combine patterns according to what is important: Hardware integration, usability
  tests, customer presentation, prototypes, feasability tests
  Make a step by step plan
         • Which modules are included
         • Which are faked/stubbed
         • Which is the top module in the System Under Test



  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 41 -->

ROULETTE DEPENDENCY TREE
                                                                                                                UI




                                                              RouletteGame




                                                  Roulette Game
                                                                             Output        Roulette                       FieldBet                EvenOddBet        ColorBet
                                                    Exception




                                                                                                                                                               <<abstract>>
                                                                              Randomizer        Field Factory
                                                                                                                                                                   Bet




                                                                                                                                       Field




  AARHUS
  UNIVERSITY                                                                                SPRING 2025    LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 42 -->

DEPENDENCY TREE WITH MARKINGS FOR A
BOTTOM UP STRATEGY



                                                                    U
                                                                        8   6
                                                                                                         6                       6               6



Markings:                                                                   5   7        1                                           U       U       U
U: This dependency was already tested
during unit test                                                                                                             2       3
                                                                                                                        1                4
1,2,…8: The integration step number
                                                                                                          U
where this is exercised


        AARHUS
        UNIVERSITY                                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
        D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Dependency tree for roulette-systemet med rækkefølgenumre på kanterne. Knuder: UI øverst; næste niveau RouletteGame; derefter rækken Roulette Game Exception, Output, Roulette, FieldBet, EvenOddBet, ColorBet; derefter Randomizer, Field Factory, <<abstract>> Bet; nederst Field.

<!-- side 43 -->

BOTTOM UP PLAN
    Step #                      RouletteGa                    Output   Roulette   FieldBet          EvenOddB               ColorBet        Rando-   Field     Field
                                me                                                                  et                                     mizer    Factory



    1                                                                  T                                                                   S        X         X
    2                                                                             T                                                                           X
    3                                                                                               T                                                         X
    4                                                                                                                      T                                  X
    5                                                                  T                                                                   X        X         X
    6                           T                             S        X          X                 X                      X               S        X         X
    7                           T                             S        X          X                 X                      X               X        X         X
    8*                          T                             X        X          X                 X                      X               X        X         X

                               T: This module is included, it's the/a top module, and the one driven
                               X: This module is included
                               S: This module is faked: stubbed or mocked
  AARHUS                       *Step # 8 is difficult to automate because of Output to console!
  UNIVERSITY                                                                          SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 44 -->

BOTTOM UP – STEP 5

                                                                                                               UI




                                                       RouletteGame




                                                                                     D


                                         Roulette Game
                                                                      Output             Roulette                     FieldBet            EvenOddBet             ColorBet
                                           Exception




                                                                                                                                                           <<abstract>>
                                                                        Randomizer             Field Factory
                                                                                                                                                               Bet




                                                                                                                                  Field




  AARHUS
  UNIVERSITY                                                                                          SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 45 -->

BU – STEP 5 – BLACK BOX

                                                                              D                                       Black Box eye for
                                                                                                                          testing!




                                         White Box eye for
                                             planning                           Roulette




                                                              Randomizer                 Field Factory




                                                                                                                                    Field




  AARHUS
  UNIVERSITY                                                               SPRING 2025       LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Bottom-up step 5 illustreret som en indramning: en stor rektangulær ramme omslutter delsystemet under test med de røde bokse Roulette (øverst), Randomizer og Field Factory (midt) og Field (nederst). Kanter: Roulette→Randomizer (tyk, den forbindelse der testes i step 5), Roulette→Field Factory, Roulette→Field og Field Factory→Field (tynde). Over rammen sidder en lyseblå cirkel mærket D (driver), forbundet ned til Roulette, med en pegende hånd-ikon ved siden. To øje-symboler: ét uden for rammen ved driveren med etiketten "Black Box eye for testing!" (testerens synsvinkel udefra), og ét inde i/ved rammen med etiketten "White Box eye for planning" (planlægning sker med kendskab til den indre struktur).

<!-- side 46 -->

DEPENDENCY TREE WITH MARKINGS FOR A TOP
DOWN STRATEGY



                                                                     U
                                                                         7   2
                                                                                                          1                        1               1



                                                                             4   6        3                                          U         U       U
Markings:
U: This dependency was already tested
during unit test                                                                                                              5        5
1,2,…7: The integration step number                                                                                                        5
                                                                                                                         3
where this is exercised                                                                                    U



         AARHUS
         UNIVERSITY                                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
         D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Samme dependency tree for roulette-systemet som side 42, men med top-down-nummerering. Knuder: UI øverst; RouletteGame; rækken Roulette Game Exception, Output, Roulette, FieldBet, EvenOddBet, ColorBet; derefter Randomizer, Field Factory, <<abstract>> Bet; nederst Field.

<!-- side 47 -->

TOP-DOWN PLAN
    Step #                      RouletteGa                    Output   Roulette   FieldBet          EvenOddB               ColorBet        Rando-   Field     Field
                                me                                                                  et                                     mizer    Factory



    1                           T                             S        S          X                 X                      X                                  S/X
    2                           T                             S        X          X                 X                      X               S        S         S
    3                           T                             S        X          X                 X                      X               S        X         X
    4                           T                             S        X          X                 X                      X               X        X         X
    5                           T                             S        X          X                 X                      X               S        X         X
    6                           T                             S        X          X                 X                      X               X        X         X
    7*                          T                             X        X          X                 X                      X               X        X         X
                                 T: This module is included, it's the/a top module, and the one driven
                                 X: This module is included
                                 S: This module is faked: stubbed or mocked
                                 *Step # 7 is difficult to automate because of output to console!

  AARHUS
  UNIVERSITY                                                                          SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 48 -->

TOP DOWN – STEP 1

                                                                                                            UI



                                                       D



                                                       RouletteGame




                                                                        S               S


                                         Roulette Game
                                                                      Output         Roulette                     FieldBet             EvenOddBet             ColorBet
                                           Exception




                                                                                                                                                        <<abstract>>
                                                                        Randomizer          Field Factory
                                                                                                                                                            Bet
                                                                                                                                   S



                                                                                                                               Field




  AARHUS
  UNIVERSITY                                                                                       SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 49 -->

TOP DOWN – STEP 5

                                                                                                                  UI



                                                              D



                                                              RouletteGame




                                                                               S


                                                 Roulette Game
                                                                             Output         Roulette                            FieldBet             EvenOddBet         ColorBet
                                                   Exception




                                                                                      S

                                                                                                                                                                  <<abstract>>
                                                                               Randomizer         Field Factory
                                                                                                                                                                      Bet




                                                                                                                                             Field



  AARHUS
  UNIVERSITY                                                                                     SPRING 2025           LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 50 -->

TD – STEP 5 – BLACK BOX
                                                                                      D                                Black Box eyes for
                                                                                                                             testing



                                                                                 RouletteGame




                                                    Roulette Game
                                                                                   Roulette                      FieldBet           EvenOddBet         ColorBet
                                                      Exception




                                                                                          Field Factory
                                                                                                                                                              <<abstract>>
                                                                                                                                                                  Bet


                                                                                                                            Field



                                                                                                                                                   White Box eye for
                                                                                                                                                       planning
                                                        S                 S                           Black Box eyes for
                                                                                                            testing

  AARHUS                                          Output            Randomizer
  UNIVERSITY                                                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

**Figur:** Top-down step 5 illustreret som en indramning: en stor rektangulær ramme omslutter de allerede integrerede røde bokse — RouletteGame øverst, derunder Roulette Game Exception, Roulette, FieldBet, EvenOddBet, ColorBet, samt Field Factory, Field og <<abstract>> Bet. Uden for rammen nederst står Output og Randomizer, hver forbundet til en lyseblå cirkel mærket S (stub). Over rammen en lyseblå cirkel mærket D (driver) forbundet ned til RouletteGame, med pegende hånd-ikon.

<!-- side 51 -->

TEST CASES
  There is no interface coverage tool!
  Partial use cases
         • Look in sequence diagrams
  Original Unit Test cases for the top unit
         • (top level in the current integration step - SUT)
         • Does it make sense to reuse them all? E.g. if the effect is only in the top unit they don't make
           sense for integration tests
  Original Unit Test cases for the lower units
         • How were their interfaces exercised?
         • How can you force similar relevant tests through the top unit?


  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 52 -->

HOW TO ORGANIZE ITS
  Organize your IT steps in one or more separate projects under the same solution
  as the units – one test fixture for each step




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 53 -->

INTEGRATION TEST AND CI
  You cannot use the Coverage tools to say anything about your Integration Tests
  But you can use all the same tools you used for unit testing




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 54 -->

BEST PRACTICES
  Automate, automate, automate
         • Higher manual steps complexity → lower probability of doing them
         • Use High Frequency integration: Nightly CI builds for each test scenario to
           check integration (common code repo)
         • Use Test Frameworks and Isolation Frameworks as much as possible


  Easier to code stubs than drivers


  Use a little bit of all patterns (except Big Bang)

  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

<!-- side 55 -->

DEPENDENCY TREE EXERCISE
  Find the Dependency Tree for the Microwave Oven




  AARHUS
  UNIVERSITY                                                  SPRING 2025   LUKAS ESTERLE / PETER HØGH MIKKELSEN
  D EP ARTME NT OF ELECTRI CAL AN D COMP UT ER T ECH NOLOGY

