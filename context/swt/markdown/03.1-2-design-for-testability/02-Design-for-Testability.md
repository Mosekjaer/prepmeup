---
title: "Design for Testability"
source: "Design for Testability.pdf"
modul: "Lektion 03.1+2: Design For Testability"
pages: 24
type: "slides"
vision: "done"
---
# Design for Testability

<!-- side 1 -->

DESIGN FOR TESTABILITY



AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   DESIGN FOR TESTABILITY
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

DISCUSSION – A GENERAL PROBLEM
                                                                                All test involves interaction
                               Test code                                        with the Unit Under Test
                                                                                (UUT)
                                                                                Either directly, by
                                                                                monitoring responses
                                                                                (return values) or
                                 UUT                                            resulting state.

                                                                                Or indirectly, by
                                                                                monitoring
                                                                                how the UUT
                              Dependencies                                      interacts with its
                                                                                dependencies
  AARHUS                                        SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                           FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

<!-- side 3 -->

TESTING A CLASS

                                                                                                                  Testclass TestA wants to test
                                          TestA
                                                                                                                          A::methodA

                                                                 A

                                                  - objB : B                                                          methodA calls
                                                  + methodA(int) : int                                                objB.methodB


                                                                                                                         The test fails!
                                                                 B
                                                  + methodB(int) : int


                                                                                                                      Where is the error?
                              The problem is that MethodA depends on
                              methodB (and it’s correctness or not)
  AARHUS                                                                          SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                             FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

<!-- side 4 -->

DESIGN FOR TESTABILITY - DEPENDENCIES
In order to test a functionality of a class, we first need to detach it from the rest of

system in which it is designed to work. We then need to create an instance from

that class, activate the tested functionality and finish by making sure the resulting

behavior matches our expectations.



However, unless the system is designed specifically to enable this, in most cases, it

will not be simple.

                                                                                Gil Zilberfeld, http://www.infoq.com/articles/Testability



   AARHUS                                       SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                          FEBRUARY 2026   DESIGN FOR TESTABILITY
   T ECH N IC AL SC IEN C ES

<!-- side 5 -->

TOWARDS CONTROLLING DEPENDENCIES
 • A testable design allows us to detach a single class (the UUT) from the
   rest of the system.

 • Then, we can control which dependencies the UUT uses, e.g. our “fake”
   versions of the dependencies

 • There are 3 steps towards control (III or Triple-I):
          1. IDENTIFY        Identify the external dependency
          2. INTERFACE       Introduce an interface (TAOUT: a seam) at the dependency
          3. INJECT          Replace (inject) the dependency



 AARHUS                                             SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                FEBRUARY 2026   DESIGN FOR TESTABILITY
 T ECH N IC AL SC IEN C ES

<!-- side 6 -->

1: IDENTIFY THE DEPENDENCIES
  class House
  {
      private readonly Bedroom _bedroom;
      private readonly Kitchen _kitchen;
                                                                                Where are the
      private readonly FrontDoor _door;
                                                                                dependencies?
           public House()
           {
               _bedroom = new Bedroom();                                        What is the problem?
               _kitchen = new Kitchen();
               _door = new FrontDoor();                                         What is the design flaw?
           }

           public void Leave()
           {                                                                                     House
               _kitchen.ShutDownAllAppliances();
               _bedroom.TurnLightOff();                                                + Leave()
               _door.Lock();
           }
  }


                                                   Bedroom                                      Kitchen        Frontdoor
                                             + TurnLightOff()                          + TurnOffAll...()   + Lock()


  AARHUS                                                                 SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                    FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

**Figur:** Klassediagram til højre for kodeboksen: en boks House med operationen + Leave(). Fra House går tre associationspile nedad til hver sin boks: Bedroom (+ TurnLightOff()), Kitchen (+ TurnOffAll...()) og Frontdoor (+ Lock()). Alle pile peger fra House ned mod afhængighederne, dvs. House er hårdt koblet til de tre konkrete klasser. Tre store mørkeblå pile peger fra spørgsmålsboksen "Where are the dependencies?" hen mod kodeboksens felt- og konstruktorafsnit, hvor `new Bedroom()`, `new Kitchen()` og `new FrontDoor()` instantieres — designfejlen er, at House selv konstruerer sine afhængigheder.

<!-- side 7 -->

2: INTERFACE – LOOSEN THE COUPLING




                              • We can loosen the coupling by introducing
                                interfaces to the dependencies
  AARHUS                                                    SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                       FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

<!-- side 8 -->

2: INTERFACE – LOOSEN THE COUPLING
 What does House really want to do with the Bedroom?
 • “To call TurnLightOff()on its Bedroom object”, or
 • “To turn the light off in the bedroom”

  How can interfaces help us?                                                          House
  Where do the interfaces go?                                                    + Leave()
  How does this improve testability?


                                       Bedroom                                         Kitchen           Frontdoor
                                  + TurnLightOff()                               + TurnOffAll...()   + Lock()




   AARHUS                                            SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                               FEBRUARY 2026   DESIGN FOR TESTABILITY
   T ECH N IC AL SC IEN C ES

**Figur:** Klassediagram til højre: House (+ Leave()) med tre associationspile ned til Bedroom (+ TurnLightOff()), Kitchen (+ TurnOffAll...()) og Frontdoor (+ Lock()) — samme direkte, konkrete kobling som på foregående slide, endnu uden interfaces. Spørgsmålsboksen til venstre står visuelt over for diagrammet som det, der skal løses.

<!-- side 9 -->

2: INTERFACE – LOOSEN THE COUPLING
                                                       House
                                                 + Leave()




                                  IBedroom            IKitchen                                     IFrontdoor
                              + TurnLightOff()   + TurnOffAll...()                             + Lock()




                                  Bedroom              Kitchen                                     Frontdoor
                              + TurnLightOff()   + TurnOffAll...()                             + Lock()




  AARHUS                                                       SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                          FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

<!-- side 10 -->

2: INTERFACE – LOOSEN THE COUPLING
MAKE FAKES!
                                                                                                     House
                                                                                              + Leave()




                                                 IBedroom                                            IKitchen                                       IFrontdoor
                                             + TurnLightOff()                                 + TurnOffAll...()                                + Lock()




                                  Bedroom                   FakeBedroom            Kitchen                        FakeKitchen           Frontdoor                 FakeFrontdoor
                              + TurnLightOff()             + TurnLightOff()   + TurnOffAll...()                 + TurnOffAll...()   + Lock()                     + Lock()




                                                      Implementing ”fake” instances of interfaces
                                                      provides us with control over dependencies.
  AARHUS                                                                                      SWT         PETER HØGH MIKKELSEN
  UNIVERSITY                                                                         FEBRUARY 2026        DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

<!-- side 11 -->

2: INTERFACE – LOOSEN THE COUPLING
         // *** Production code ***                                                                    // *** Test code ***
         class House                                                                                   class House
         {                                                                                             {
             private readonly IBedroom _bedroom;                                                           private readonly IBedroom _bedroom;
             private readonly IKitchen _kitchen;                                                           private readonly IKitchen _kitchen;
             private readonly IFrontDoor _door;                                                            private readonly IFrontDoor _door;

                   public House()                                                                          public House()
                   {                                                                                       {
                       _bedroom = new Bedroom();           Change production                                   _bedroom = new FakeBedroom();
                       _kitchen = new Kitchen();           code to test it!!!                                  _kitchen = new FakeKitchen();
                       _door = new FrontDoor();                                                                _door = new FakeFrontDoor();
                   }                                                                                       }

                   public void Leavehouse()                                                                public void Leavehouse()
                   {                                                                                       {
                       _kitchen.ShutDownAllAppliances();                                                       _kitchen.ShutDownAllAppliances();
                       _bedroom.TurnLightOff();                                                                _bedroom.TurnLightOff();
                       _door.Lock();                                                                           _door.Lock();
                   }                                                                                       }
         }                                                                                             }


         This is not an optimal way to do it…
         The problem is that dependencies are created within the House class
  AARHUS                                                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                  FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

**Figur:** To kodebokse side om side forbundet af en bred blå pil mærket "Change production code to test it!!!", der peger fra produktionskoden (venstre) til testkoden (højre). Begge bokse indeholder samme klasse House med felterne typet som interfaces (IBedroom, IKitchen, IFrontDoor); den eneste forskel er konstruktorkroppen: venstre instantierer `new Bedroom()`, `new Kitchen()`, `new FrontDoor()`, højre `new FakeBedroom()`, `new FakeKitchen()`, `new FakeFrontDoor()`. Pointen visualiseret af pilen: man må ændre selve produktionsklassen for at kunne teste den.

<!-- side 12 -->

3: INJECT DEPENDENCY
 • We would like to control the type of dependencies that House uses.
       – Fake dependencies or real dependencies


 • We do this by injecting the dependencies into House.
       – Remember: Since House uses interfaces to its’ dependencies, it does not
         care about their concrete types
       – So instead of letting House construct it’s own dependencies, we inject them
         into House
       – This means that we can isolate House from the
         rest of the system!

  AARHUS                                       SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                          FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

<!-- side 13 -->

3: CONSTRUCTOR INJECTION
                              class House                                               // Production
                              {                                                         class Program
                                  private readonly IBedroom _bedroom;                   {
                                  private readonly IKitchen _kitchen;                      static void Main(string[] args)
                                  private readonly IFrontDoor _door;                       {
                                                                                              // Use constructor injection to inject reals
                                  // Constructor injection                                    var house = new House(
                                  public House (                                                 new Bedroom(),
                                      IBedroom bedroom,                                          new Kitchen(),
                                      IKitchen kitchen,                                          new FrontDoor()
                                      IFrontDoor door)                                        );
                                  {                                                        }
                                      _bedroom = bedroom;                               }
                                      _kitchen = kitchen;
                                      _door = door;
                                  }

                                  public void Leavehouse()                              // Test
                                  {                                                     [Test]
                                      _kitchen.ShutDownAllAppliances();                 public void House_Ctor_ConstructorInj()
                                      _bedroom.TurnLightOff();                          {
                                      _door.Lock();                                         // Use constructor injection to inject fakes
                                  }                                                         var uut = new House(
                              }                                                                 new FakeBedroom(),
                                                                                                new FakeKitchen(),
                                                                                                new FakeFrontDoor());
                                                                                        }

                                  House remains
  AARHUS
  UNIVERSITY
                                  unchanged                                      SWT
                                                                        FEBRUARY 2026
                                                                                        PETER HØGH MIKKELSEN
                                                                                        DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

**Figur:** Tre kodebokse. Venstre boks (klassen House) er stor og uændret; en blå pil peger opad fra kommentarboksen "House remains unchanged" til bunden af den. Højre side er delt i to: øverst produktionskoden, nederst testkoden — samme konstruktorkald med henholdsvis rigtige og fake implementationer:

<!-- side 14 -->

3: PROPERTY INJECTION
                              public class House                                         // Production
                              {                                                          class Program
                                  // Properties allow property injection                 {
                                  public IBedroom Bedroom { private get; set; }             static void Main(string[] args)
                                  public IKitchen Kitchen { private get; set; }             {
                                  public IFrontDoor Door { private get; set; }                 // Use default properties (reals)
                                                                                               var house = new House();
                                 public House()                                             }
                                 {                                                       }
                                     // Initialize dep’s – may be overwritten
                                     // later
                                     Bedroom = new Bedroom();                            // Test
                                     Kitchen = new Kitchen();                            [Test]
                                     Door = new FrontDoor();                             public void House_Ctor_PropertyInj()
                                 }                                                       {
                                                                                             var uut = new House();
                                 public void Leavehouse()
                                 {                                                             // Use property injection to inject fakes
                                     Kitchen.ShutDownAllAppliances();                          uut.Bedroom = new FakeBedroom();
                                     Bedroom.TurnLightOff();                                   uut.Kitchen = new FakeKitchen();
                                     Door.Lock();                                              uut.Door = new FakeFrontDoor();
                                 }                                                       }


  Problems: 1) If the class creates dependencies in the constructor, these could potentially do non-reversible
            things, even though they are overwritten later.
            2) There has to be a ”shell” implementation of the dependencies, so they can be created inside the
            constructor – otherwise it cannot compile!
  AARHUS                                                                          SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                             FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

**Figur:** Samme tredelte layout som constructor injection-sliden: venstre boks er klassen House, hvor afhængighederne er auto-properties med `private get; set;`, så de kan overskrives udefra. Højre side er delt i produktionskode (øverst) og testkode (nederst):

<!-- side 15 -->

CLEAN ARCHITECTURE
With interfaces and
dependency injection
implementing priority inversion,
we now have interfaces at the
center together with data
classes a.o. without
dependencies.
These are being used by our
code (Use Cases) and
implemented by presenters,
devices etc…

Dependencies only go inward!


    AARHUS                      (Source: Robert C. Martin)            SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                                               FEBRUARY 2026   DESIGN FOR TESTABILITY
    T ECH N IC AL SC IEN C ES

**Figur:** Robert C. Martins "The Clean Architecture"-diagram: fire koncentriske ringe. Yderste ring (blå, Frameworks & Drivers) er mærket Devices, Web, UI, DB og External Interfaces. Næste ring ind (grøn, Interface Adapters) er mærket Controllers, Gateways og Presenters. Næste (rød/pink, Application Business Rules) er Use Cases. Inderste cirkel (gul, Enterprise Business Rules) er Entities. En farvelegende til højre kobler de fire farver til de fire lag. Tre vandrette pile fra venstre peger indad gennem ringene mod Entities — afhængighedsretningen går kun indad.

<!-- side 16 -->

THE INVISIBLE DEPENDENCIES!


  • At some point, we must consider the real world in our system
  • Classics are “uncontrollable” dependencies
          – Time, file system, console, randomness, external hardware and its drivers, …
  • We do not omit dependency inclusion, we defer it!
          – During unit testing (and initial integration testing), we isolate the use of
            these dependencies as much as possible
          – When time comes, we can integrate external dependencies with a higher
            degree of confidence




   AARHUS                                         SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                            FEBRUARY 2026   DESIGN FOR TESTABILITY
   T ECH N IC AL SC IEN C ES

<!-- side 17 -->

EXAMPLE OF UNCONTROLLABLE EXTERNAL
DEPENDENCIES
                                                                                                             How can we test this:
                                                                                                                 Anytime?
                                                                                                               Automatically?

                              public class House
                              {
                                  public void CloseHouse()
                                  {
                                      // If after bedtime – close down house                                 Wait until after 22:00
                                      if (System.DateTime.Now.Hour > 22)
                                      {                                                                       but before 00:00?
                                              Kitchen.ShutDownAllAppliances();
                                              Bedroom.TurnLightOff();
                                              Door.Lock();
                                              // Log the event
                                              System.Console.WriteLine("House closed down");
                                  }




                                                                                                             Using picture analysis
                                                                                                             on a camera picture
  AARHUS                                                                     SWT    PETER HØGH MIKKELSEN
                                                                                                                of the console?
  UNIVERSITY                                                        FEBRUARY 2026   DESIGN FOR TESTABILITY
  T ECH N IC AL SC IEN C ES

**Figur:** To røde pile knytter tekstboksene til de problematiske kodelinjer: pilen fra boksen "Wait until after 22:00 but before 00:00?" peger på linjen `if (System.DateTime.Now.Hour > 22)`, og pilen fra boksen "Using picture analysis on a camera picture of the console?" peger på linjen `System.Console.WriteLine("House closed down")`. De to markerede linjer er præcis de ukontrollerbare eksterne afhængigheder: systemuret og konsollen.

<!-- side 18 -->

SOLVING UNCONTROLLABLE EXTERNAL
DEPENDENCIES
                                                                                           House
                                                                                     + Leave()
                                                                                                                                                                         Encapsulation!



                                 IBedroom            IKitchen          IFrontdoor                 ITimeProvider                                           ILogger
                             + TurnLightOff()   + TurnOffAll...()   + Lock()                     + GetHour()                                         + WriteLogLine()




                                                                               FakeTimeProvider                TimeProvider           FakeLogger                         Logger
                                                                           + GetHour()                         + GetHour()        + WriteLogLine()                  + WriteLogLine()


                                                                                                                    Using                                                 Using



                                                                                                                System.Time                                         System.Console


 AARHUS                                                                                        SWT       PETER HØGH MIKKELSEN
 UNIVERSITY                                                                           FEBRUARY 2026      DESIGN FOR TESTABILITY
 T ECH N IC AL SC IEN C ES

<!-- side 19 -->

SOLVING UNCONTROLLABLE EXTERNAL
DEPENDENCIES                 public class House
                             {
                                 public House (
                                     IBedroom bedroom,
                                     IKitchen kitchen,
                                     IFrontDoor door,
                                     ITimeProvider timep,
                                     Ilogger logger)
                                 {
                                                                                         Then
                                     _bedroom = bedroom;                                 injection
                                     _kitchen = kitchen;
                                     _door = door;
                                     _timeProvider = timep;
                                     _logger = logger;
                                 }

                                public void Leavehouse()
                                {
                                    // If after bedtime – close down house
                                    if (_timeProvider.getHour() > 22)
                                    {
                                            Kitchen.ShutDownAllAppliances();
                                            Bedroom.TurnLightOff();
                                            Door.Lock();
                                            // Log the event
                                            _logger.WriteLogLine("House closed down");
                                      }
                                }



 AARHUS                                         SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                            FEBRUARY 2026   DESIGN FOR TESTABILITY
 T ECH N IC AL SC IEN C ES

**Figur:** En blå taleboble mærket "Then injection" peger på kodeboksens konstruktorparameterliste, hvor de to nye interfaces ITimeProvider timep og Ilogger logger er tilføjet ved siden af IBedroom, IKitchen og IFrontDoor. I metodekroppen er `System.DateTime.Now.Hour` erstattet af `_timeProvider.getHour()` og `System.Console.WriteLine(...)` af `_logger.WriteLogLine("House closed down")` — de ukontrollerbare afhængigheder er nu skjult bag injicerede interfaces.

<!-- side 20 -->

RECAP
 • A sensible design is required for testability

 • Primary concern: Dependencies = Loss of control in testing.

 • Other concern: Dependencies = Errors cannot be located precisely

 • Gain control by designing for loose coupling - III
        1. Identify the dependency
        2. Introduce an interface (seam) for the dependency
        3. Inject the dependency


   AARHUS                                          SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                             FEBRUARY 2026   DESIGN FOR TESTABILITY
   T ECH N IC AL SC IEN C ES

<!-- side 21 -->

DESIGN PRINCIPLES AND PATTERNS FOR
DESIGN FOR TESTABILITY
Low coupling
  • III – design with interfaces
  • Use Observer Pattern
     • In C# this is normally done with C# events and delegates
  • Use Exceptions (but not for normal return of data!)


Single responsibility
  • Reduces the number of internal states (by using injection)
  • Makes the code and testing simpler!
  • Refactor                                            See also section 11.2 of SWD for C# recommendations

     • Use the tests to ensure you did it right!
    AARHUS                                             SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                                FEBRUARY 2026   DESIGN FOR TESTABILITY
    T ECH N IC AL SC IEN C ES

<!-- side 22 -->

SYMPTOMS OF WRONG DESIGN
 If the tests are hard to write, maybe the design is wrong!

 If the tests are hard to write, it may be hard to use the unit in code!

  If the design is good then tests should be relatively easy to write




 AARHUS                                     SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                        FEBRUARY 2026   DESIGN FOR TESTABILITY
 T ECH N IC AL SC IEN C ES

<!-- side 23 -->

QUESTIONS?




 AARHUS                               SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   DESIGN FOR TESTABILITY
 T ECH N IC AL SC IEN C ES

<!-- side 24 -->

AARHUS
UNIVERSITY

