---
title: "Test Types and Fake Types"
source: "csfiles/home_dir/FakesAndIsolation/Test Types and Fake Types.pdf"
modul: "Lektion 04.1: Test Types and Fake Types"
pages: 26
type: "slides"
vision: "done"
---
# Test Types and Fake Types

<!-- side 1 -->

TEST TYPES AND FAKE
TYPES
I4SW




AARHUS
UNIVERSITY                                           LUKAS ESTERLE
D E P A R T M E N T O F E N G IN E E R IN G
                                              2024   ASSOCIATE PROFESSOR

<!-- side 2 -->

FAKES - WHY
                                                                                     All test involves interaction
                                                 Test code                           with the Unit Under Test
                                                                                     (UUT)

                                                                                          Either directly, by
                                                                                          monitoring responses

                                                   UUT                                    or resulting state.

                                                                                          Or indirectly, by
                                                                                          monitoring
                                                                                          how the UUT
                                                Dependencies                              interacts with its
  AARHUS
                                                                                          dependencies
  UNIVERSITY                                                        LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                             2024   ASSOCIATE PROFESSOR

<!-- side 3 -->

RECAP - DESIGN FOR TESTABILITY

   Low coupling                                                          Single responsibility
           • III – design with interfaces                                  • Reduces the number of internal states
           • Use Observer Pattern                                          • Makes the code simpler!
                       • In C# this is normally done with C# events        • Makes testing simpler!
                         and delegates
                                                                           • Refactor
           • Use Exceptions (but not for normal return of
                                                                                  • Use the tests to ensure you did it right!
             data!)




  AARHUS
  UNIVERSITY                                                              LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                  2024    ASSOCIATE PROFESSOR

<!-- side 4 -->

RECAP: WHAT IS A FAKE?
  WHAT: A fake is a “fake” version of one of a class’ dependencies


  WHY: We use fakes to isolate the UUT from the system it is intended to function in, in order to be able
  to test the UUT in isolation and have control of the test


  HOW: We enable the use of fakes by III
                        1.                      Identifying the dependencies
                        2.                      Inserting interfaces for them, and
                        3.                      Injecting the dependencies into the UUT




  AARHUS
  UNIVERSITY                                                                                     LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                                          2024   ASSOCIATE PROFESSOR

<!-- side 5 -->

RECAP: WHAT IS A FAKE?
                                                                                                                                 House
                                                                                                                          + Leave()




                                                                                               Bedroom                           Kitchen                             Frontdoor
                                                                                         + TurnLightOff()                 + TurnOffAll...()                     + Lock()




                                                Tight coupling

                                                                                                                                 House House
                                                                                                                          + Leave() + Leave()




                                                                                    IBedroom                                             IKitchen                                                IFrontdoor
                                                                                               IBedroom                        IKitchen                             IFrontdoor
                                                                                + TurnLightOff()                                   + TurnOffAll...()                                        + Lock()
                                                                                          + TurnLightOff()                + TurnOffAll...()                     + Lock()




                                                                     Bedroom                   Bedroom
                                                                                               FakeBedroom                      Kitchen
                                                                                                                           Kitchen                     FakeKitchen Frontdoor         Frontdoor                 FakeFrontdoor
                                                                 + TurnLightOff()        + TurnLightOff()
                                                                                            + TurnLightOff()               + TurnOffAll...()
                                                                                                                      + TurnOffAll...()                          + Lock()
                                                                                                                                                   + TurnOffAll...()             + Lock()                     + Lock()
  AARHUS
  UNIVERSITY                                    Loose coupling                                                        LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                                                               2024   ASSOCIATE PROFESSOR

**Figur:** To UML-klassediagrammer over samme House-eksempel, adskilt af en stor nedadpegende pil; øverste mærket "Tight coupling" (rød tommel ned), nederste "Loose coupling" (grøn tommel op). Øverst: klassen House med operationen + Leave() har direkte associationslinjer ned til tre konkrete klasser: Bedroom (+ TurnLightOff()), Kitchen (+ TurnOffAll…()) og Frontdoor (+ Lock()). Nederst: House + Leave() peger i stedet på tre interfaces IBedroom (+ TurnLightOff()), IKitchen (+ TurnOffAll…()) og IFrontdoor (+ Lock()). Fra hvert interface går to stiplede realiseringspile (hul trekantspids) op fra hhv. den rigtige implementering og fake-implementeringen: Bedroom og FakeBedroom realiserer IBedroom, Kitchen og FakeKitchen realiserer IKitchen, Frontdoor og FakeFrontdoor realiserer IFrontdoor. Fake-klasserne har samme operationssignaturer som deres rigtige modparter. Pointen i figuren: interfacelaget bryder den direkte afhængighed, så fakes kan substitueres.

<!-- side 6 -->

TEST TYPES

Fakes




AARHUS
UNIVERSITY                                           LUKAS ESTERLE
D E P A R T M E N T O F E N G IN E E R IN G
                                              2024   ASSOCIATE PROFESSOR

<!-- side 7 -->

TEST TYPES
   Unit tests fall in one of two groups:
           • Value-based and State-based tests are used to test if the UUT returns the correct value or is in the expected
             (external) state after we have acted upon it.
           • Interaction-based tests are used to test if the UUT has the expected behavior with it’s dependencies as a result of our
             acting upon it.



   Each test group have different minimum requirements concerning what type of fake they need


   (See section 1.1 – the grey box – and section 4.1 in the book)




  AARHUS
  UNIVERSITY                                                              LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                   2024   ASSOCIATE PROFESSOR

<!-- side 8 -->

STATE-BASED TESTS
Fakes




AARHUS
UNIVERSITY                                           LUKAS ESTERLE
D E P A R T M E N T O F E N G IN E E R IN G
                                              2024   ASSOCIATE PROFESSOR

<!-- side 9 -->

STATE-BASED TESTS: STUB S ARE ENOUGH
  For state-based testing we may use stubs
           • Stubs exist to provide the UUT with fake dependencies and/or values it needs to function (i.e. just be present or to
             return a value)



  The procedure for state-based tests is the usual:
                        1.                      Arrange   Set up your UUT and its fake dependencies
                        2.                      Act       Stimulate the UUT
                        3.                      Assert    That the UUT is in the expected state or returned
                                                          the expected value


     1.                   A correct stub can never cause a test to fail, since assertion is on UUT, never on the stub


  AARHUS
  UNIVERSITY                                                                              LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                                   2024   ASSOCIATE PROFESSOR

<!-- side 10 -->

STATE/VALUE-BASED TESTS

                                                                                                                        Dependency 1
                                                                                                                           (stub )

 1. Arrange                                     Set up your UUT and its                                   Act
                                                                                   Test code                      UUT
                                                fake dependencies
                                                                                                         Assert
 2. Act                                         Stimulate the UUT
 3. Assert                                      that the state of the                                                   Dependency 2
                                                                                                                           (stub )
                                                UUT is as expected
                                                or it returns the correct
                                                value.                       In a state/value-based test, the assertion
                                                                             is ALWAYS on the UUT, NEVER on the
                                                                             fake(s)

  AARHUS
  UNIVERSITY                                                                       LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                            2024   ASSOCIATE PROFESSOR

**Figur:** Dataflowdiagram til højre for Arrange/Act/Assert-boksen: kassen "Test code" har en tyk mørkeblå pil mærket Act ind i kassen "UUT", og en rød pil mærket Assert der også peger på UUT (assertion rettes altså mod UUT selv). Fra UUT går stiplede pile ud til to grønne kasser "Dependency 1 (stub)" og "Dependency 2 (stub)", og tykke mørkeblå dobbeltpile viser toevejs-kald/svar mellem UUT og hver stub. Ingen pil rører stubbene fra testkoden.

<!-- side 11 -->

STUBS – EXAMPLE:
ENVIRONMENTAL CONTROL SYSTEM (ECS)
  We wish to do value-based test on RunSelfTest of the class Control, which depends on an
  IHeater.
  To do this, we provide it with a stub of the heater, StubHeater
  This allows us to control the dependency and thus test Control in a controlled environment.
                                                                                                                           << interface >>
                                                                       Control
                                                                                                                               IHeater                                               class StubHeater: IHeater
                                                                                                                                                                                     {
                                                + Control (s: ITempSensor, h: IHeater, thr: int)                     + TurnOn(): void
                                                + Regulate(): void                                                   + TurnOff(): void                                                   public bool _result;
                                                + SetThreshold(thr: int)                                             + RunSelfTest(): bool                                               ...
                                                + GetThreshold(): int
                                                + GetCurTemp(): int
                                                                                                                                                                                         public bool RunSelfTest()
                                                + RunSelfTest(): bool                                                                                                                    {
                                                                                                                                                                                           return _result;
                                                                                                                                                                                         }
                                                                                                                                                                                     }
                                                                                                            Heater                                    StubHeater


                                                                                                   + Heater()                                + FakeHeater()
                                                                                                   + TurnOn(): void                          + TurnOn(): void
                                                                                                   + TurnOff(): void                         + TurnOff(): void
                                                                                                   + RunSelfTest(): bool                     + RunSelfTest(): bool                   The StubHeater will return the
                                                                                                                                             + SetSelfTestResult(r: bool)            configured value when RunSelfTest()
  AARHUS                                                                                                                                                                             is called
  UNIVERSITY                                                                                                                                                   LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                                                                                                    2024       ASSOCIATE PROFESSOR

**Figur:** UML-klassediagram plus kodeboks. Control (operationer: + Control(s: ITempSensor, h: IHeater, thr: int), + Regulate(): void, + SetThreshold(thr: int), + GetThreshold(): int, + GetCurTemp(): int, + RunSelfTest(): bool) har en associationspil til interfacet << interface >> IHeater (+ TurnOn(): void, + TurnOff(): void, + RunSelfTest(): bool). To stiplede realiseringspile med hul trekantspids går op til IHeater fra Heater (+ Heater(), + TurnOn(): void, + TurnOff(): void, + RunSelfTest(): bool) og fra StubHeater (+ FakeHeater(), + TurnOn(): void, + TurnOff(): void, + RunSelfTest(): bool, samt i eget felt + SetSelfTestResult(r: bool)). En pil fra StubHeater-boksen peger på en kodeboks med C#:

<!-- side 12 -->

INTERACTION-BASED TESTS
Fakes




AARHUS
UNIVERSITY                                           LUKAS ESTERLE
D E P A R T M E N T O F E N G IN E E R IN G
                                              2024   ASSOCIATE PROFESSOR

<!-- side 13 -->

INTERACTION-BASED TESTS – WE NEED A MOCK
  For interaction-based testing we need a mock (and perhaps stubs)
           • A mock exists to “record” that the interaction took place, because we wish to test that the UUT had the expected
             interactions with the dependency
           • Stubs exist to provide the UUT with any other dependencies it needs to function (i.e. to return a value or just be
             present)


  The procedure for state based tests is the usual:
                        1.                      Arrange   Set up your UUT and its fake dependencies
                        2.                      Act       Stimulate the UUT
                        3.                      Assert    That the mock received the expected interactions with the UUT


                          The mock can cause a test to fail since the assertion is on the mock

  AARHUS
  UNIVERSITY                                                                            LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                                 2024   ASSOCIATE PROFESSOR

<!-- side 14 -->

INTERACTION-BASED TESTS: ASSERT ON THE MOCK
                                                1. Arrange   Set up your UUT and its fake dependencies
                                                2. Act       Stimulate the UUT
                                                3. Assert    that the mock received the expected call and input

                                                                                                              Dependency 1
                                                                                                                 (stub )


                                                                         Act
                                                             Test code                   UUT



                                                                                                              Dependency 2
                                                                                                                 (mock )



                                                                               Assert


                                                         In an interaction-based test, the
  AARHUS                                                 assertion is on the mock, not the UUT
  UNIVERSITY                                                                            LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                               2024     ASSOCIATE PROFESSOR

**Figur:** Dataflowdiagram: "Test code" → tyk mørkeblå pil mærket Act → "UUT". Fra UUT stiplede pile ud til grøn kasse "Dependency 1 (stub)" og grøn kasse "Dependency 2 (mock)", hver med tykke mørkeblå dobbeltpile for kald og retursvar. En stor buet rød pil mærket Assert går fra Test code uden om UUT og rammer "Dependency 2 (mock)" direkte — modsat state-based-diagrammet, hvor Assert-pilen ramte UUT.

<!-- side 15 -->

MOCK – EXAMPLE:
ENVIRONMENTAL CONTROL SYSTEM (ECS)
  We wish to do interaction-based test of the class Control, which depends on an ITempSensor
  and an IHeater.
  To do this, we provide it with a stub of the temperature sensor, StubTempSensor, and a mock of the
  heater, MockHeater
  This allows us to control the test and thus test that Control calls IHeater correctly
                                                                                                                     <<interface>>
                                                                    Control
                                                                                                                        IHeater

                                                + Control(s: Itempsensor, h: Iheater, thr: int)                   + TurnOn(): void
                                                + Regulate(): void                                                + TurnOff(): void
                                                + SetThreshold(thr: int)                                          + RunSelfTest(): bool
                                                + GetThreshold(): int
                                                + GetCurTemp(): int
                                                + RunSelfTest(): bool




                                                                                                         Heater                           MockHeater

                                                                                                  + Heater()                        + selfTestRun: bool
                                                                                                  + TurnOn(): void                  + CountTurnOff: int
                                                                                                  + TurnOff(): void
                                                                                                                                    + MockHeater()
                                                                                                  + RunSelfTest(): bool
  AARHUS                                                                                                                            + TurnOn(): void
  UNIVERSITY                                                         LUKAS ESTERLE                                                  + TurnOff(): void
  D E P A R T M E N T O F E N G IN E E R IN G
                                                          2024       ASSOCIATE PROFESSOR                                            + RunSelfTest(): bool

**Figur:** UML-klassediagram: Control (+ Control(s: Itempsensor, h: Iheater, thr: int), + Regulate(): void, + SetThreshold(thr: int), + GetThreshold(): int, + GetCurTemp(): int, + RunSelfTest(): bool) har associationspil til <<interface>> IHeater (+ TurnOn(): void, + TurnOff(): void, + RunSelfTest(): bool). To stiplede realiseringspile med hul trekantspids peger op på IHeater fra Heater (+ Heater(), + TurnOn(): void, + TurnOff(): void, + RunSelfTest(): bool) og fra MockHeater, hvis boks har et attributfelt (+ selfTestRun: bool, + CountTurnOff: int) adskilt fra operationsfeltet (+ MockHeater(), + TurnOn(): void, + TurnOff(): void, + RunSelfTest(): bool). Mocken adskiller sig altså fra stubben ved at bære tilstandsfelter, der registrerer kald.

<!-- side 16 -->

CAUTION: AVOID COMPLEX MOCKS
  To allow assertions to be made, the mock will need to store….what?
           • The fact that a method was called
           • Number of times a method was called
           • Parameter values
           • Call sequence
           • It may have to act as a stub at the same time!

  This typically make mocks more complicated than (pure) stubs
           • Take longer to write, harder to re-use, more error-prone
           • Soon, you will have to test the mocks L

  Strive to keep your mocks simple


  AARHUS
  UNIVERSITY                                                              LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                   2024   ASSOCIATE PROFESSOR

<!-- side 17 -->

RECAP
Fakes




AARHUS
UNIVERSITY                                           LUKAS ESTERLE
D E P A R T M E N T O F E N G IN E E R IN G
                                              2024   ASSOCIATE PROFESSOR

<!-- side 18 -->

                                                  (External) State/Value
          Black Box Unit Test                          based tests

                          Interact
                                               Observe
      Act




                                           As long as the interface to
                                           the UUT and the
                                           interfaces it uses remain
                    Unit Under Test
                                           unchanged – the tests are
                                           safe!




                                                    Interaction based
Observe                                React              tests


             Mock             Stub    and/or Mock

**Figur:** Diagram med en sort firkant i midten mærket "Unit Under Test" (black box). Fra oven: blå pil mærket Act ind i boksen (hånd-ikon), og en blå pil mærket Interact ind i boksen med en stiplet returpil op til et øje-ikon mærket Observe. Denne øverste halvdel er knyttet til den røde etiket "(External) State/Value based tests". Fra bunden af boksen: en blå pil ned til den blå kasse "Mock" med et øje-ikon mærket Observe ved siden af, og en blå pil ned til kassen "Stub" med en stiplet pil tilbage op i UUT og et hånd-ikon mærket React; ved siden heraf kassen "and/or Mock". Denne nederste halvdel er knyttet til den røde etiket "Interaction based tests". Grøn tekstboks til højre: "As long as the interface to the UUT and the interfaces it uses remain unchanged – the tests are safe!"

<!-- side 19 -->

RECAP: TEST TYPES AND FAKE TYPES
  The big differences between state- and interaction-based tests is where you make the assertion


  State-based test
           • Test acts on UUT - UUT might interact
             with stub, but will change state or return
             a value
           • Test asserts that UUT assumed
             the expected (external) state or returned
             the expected result


  Interaction-based test
           • Test acts on UUT - UUT interacts with
             mock and might change state
           • Test asserts on mock that UUT inter-
             acted as expected with the mock


  AARHUS
  UNIVERSITY                                                             LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                                  2024   ASSOCIATE PROFESSOR

**Figur:** To små blokdiagrammer til højre for punktlisterne. Øverst (state-based): kasserne UUT og Stub forbundet med to mørkeblå pile i hver sin retning; fra "Test code" nedenunder går en mørkeblå pil op i UUT (act) og en tyk rød pil op i UUT (assert). Nederst (interaction-based): UUT og Mock forbundet med to mørkeblå pile i hver sin retning; fra "Test code" går en mørkeblå pil op i UUT (act), mens den tykke røde assert-pil går skråt op til højre og rammer Mock i stedet for UUT.

<!-- side 20 -->

FAKES, STUBS AND MOCKS – ONE DEFINITION
  A fake is an item – object or function – that can take the place of a dependency for the unit-under-
  test – UUT
  A stub is a fake that returns a value the UUT needs. This value can be controlled and therefore we
  can control the test of UUT
  A mock is a fake that records if and how the UUT used it. It can then be asked in an assert what
  happened to check the correct behavior of the UUT
  Some mocks are also stubs because UUT needs it!




  AARHUS
  UNIVERSITY                                             LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                  2024   ASSOCIATE PROFESSOR

<!-- side 21 -->

THE UNIVERSE OF FAKES – ONE DEFINITION

                                                               Fakes




                                                Stubs                                 Mocks

                                                               Fakes




  AARHUS
  UNIVERSITY                                                    LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                        2024    ASSOCIATE PROFESSOR

**Figur:** Venn-/mængdediagram: en stor ydre ellipse mærket "Fakes" indeholder to overlappende ellipser mærket "Stubs" (venstre) og "Mocks" (højre). Overlapszonen repræsenterer fakes der både er stub og mock; der findes altså mocks som ikke er stubs og omvendt, og begge er delmængder af fakes.

<!-- side 22 -->

TEST TYPES VS FAKES – ONE DEFINITION

   A value/state based test
            • Perhaps needs fakes some of which may be stubs
            • It never needs a mock
            • Asserts on the state or return value of the UUT


   A behavior based test
            • Needs a mock (which may also be a stub)
            • Perhaps it needs other fakes and stubs
            • Asserts on the mock


   AARHUS
   UNIVERSITY                                                     LUKAS ESTERLE
   D E P A R T M E N T O F E N G IN E E R IN G
                                                           2024   ASSOCIATE PROFESSOR

<!-- side 23 -->

FAKES, STUBS AND MOCKS – ANOTHER DEFINITION
MUCH LESS PRECISE
  A fake is an item – object or function – that can take the place of a dependency for the unit-under-
  test – UUT
  A stub is simply a fake and vice-versa
  Some fakes/stubs must returns a value the UUT needs. This value can be controlled and therefore
  we can control the test of UUT
  A mock is a fake/stub that records if and how the UUT used it. It can then be asked in an assert
  what happened to check the correct behavior of the UUT
  With this definition all mocks are stubs!




  AARHUS
  UNIVERSITY                                             LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                  2024   ASSOCIATE PROFESSOR

<!-- side 24 -->

THE UNIVERSE OF FAKES – ANOTHER DEFINITION

                                                       Fakes == Stubs




                                                                                Mocks

                                                         Fakes




  AARHUS
  UNIVERSITY                                              LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                2024      ASSOCIATE PROFESSOR

**Figur:** Mængdediagram: én stor ydre ellipse mærket "Fakes == Stubs" med én mindre ellipse "Mocks" helt inde i sig. Ingen overlap-zone — alle mocks er stubs, men ikke alle stubs er mocks. Kontrast til side 21, hvor Stubs og Mocks kun delvis overlappede.

<!-- side 25 -->

TEST TYPES VS FAKES – ANOTHER DEFINITION

   A value/state based test
           • Perhaps needs fakes/stubs
           • It never needs a mock
           • Asserts on the state or return value of the UUT


   A behavior based test
           • Needs a mock
           • Perhaps it needs other fakes/stubs
           • Asserts on the mock


  AARHUS
  UNIVERSITY                                                     LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                          2024   ASSOCIATE PROFESSOR

<!-- side 26 -->

QUESTIONS?




  AARHUS
  UNIVERSITY                                           LUKAS ESTERLE
  D E P A R T M E N T O F E N G IN E E R IN G
                                                2024   ASSOCIATE PROFESSOR

