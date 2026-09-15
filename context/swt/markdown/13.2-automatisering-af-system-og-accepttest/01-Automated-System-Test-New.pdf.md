---
title: "Automated System Test New.pdf"
source: "csfiles/home_dir/SystemAcceptanceTest/Automated System Test New.pdf"
modul: "Lektion 13.2: Automatisering af System- og Accepttest"
pages: 26
type: "slides"
vision: "done"
---
# Automated System Test New.pdf

<!-- side 1 -->

AUTOMATED SYSTEM AND
ACCEPTANCE TESTS


AARHUS                           SWT    PETER HØGH MIKKELSEN
UNIVERSITY               051 M AY2025   AU TOMATED SYSYEM TESTS
TEC HNI CAL SCI EN CES

<!-- side 2 -->

SYSTEM AND ACCEPTANCE TEST

                           Requirements                                                                      Accept testing


                          System specification                                                         System testing


                                       System design                                         Integration testing


                                        Component design                         Unit test



                                                       Implement component

 AARHUS                                                              SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                                   01 MA Y 2025   AU TOMATED SYSTEM TESTS
 TEC HNI CAL SCI EN CES

**Figur:** V-model tegnet som to nedadgående grene, der mødes i bunden. Venstre gren (specifikation/design) går trinvist nedad: Requirements → System specification → System design → Component design → Implement component. Højre gren (test) går trinvist opad fra bunden: Unit test → Integration testing → System testing → Accept testing. En mørkeblå streg løber diagonalt ned gennem venstre grens bokse til Implement component, og en tilsvarende diagonal streg med pilespids løber opad fra Implement component gennem højre grens bokse og ud over Accept testing. Vandrette dobbeltpile parrer hvert niveau: Requirements ↔ Accept testing, System specification ↔ System testing, System design ↔ Integration testing, Component design ↔ Unit test — hvert specifikationsniveau verificeres af sit modsvarende testniveau. Et lyseblåt afrundet felt fremhæver de tre øverste niveauer (Requirements/System specification/System design over for Accept/System/Integration testing) som lektionens fokusområde.

<!-- side 3 -->

SYSTEM TEST: COMPARE AND CONTRAST
  • System tests are like other tests in that they…
              •            Require a test setup
              •            Require expected and actual outputs, and evaluation
              •            Are made easier by control of external dependencies


  • System tests differ from other tests in that they…
              •            Run on the complete system – makes validation harder
              •            Can test a lot of aspects that are less relevant
                           for unit/integration tests
              •            Run in the real environment, not a test bench

  AARHUS                                                   SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                        01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 4 -->

SYSTEM TEST TYPES
  •           Usability
  •           Functionality
  •           Performance
  •           Scalability
  •           Reliability
  •           Load/stability
  •           Security
  •           …


  AARHUS                              SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                   01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 5 -->

AUTOMATION OF SYSTEM TEST
  Advantages
   • Repeatable
   • Objective
   • Exhaustive/comprehensive (code coverage)
   • They will be executed
   • You don't need a user/product owner




  AARHUS                          SWT     PETER HØGH MIKKELSEN
  UNIVERSITY               01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 6 -->

AUTOMATION OF SYSTEM TEST
  Disadvantages
   • May be hard or expensive to construct or have only partial
     similarity to real use
   • It's a bad excuse for NOT involving the user/PO
       • Endangering the user feedback in agile processes
   • Not included:
       • Monkey tests (for robustness)
       • Explorative tests (for Agile feedback)


  AARHUS                             SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                  01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 7 -->

VERIFICATION/VALIDATION
  Verification – did we do it correctly?
   • Structured and disciplined deduction if we have done
     correctly everything we specified
   • e.g. automated tests

  Validation – did we make the right thing?
   • Open minded and disciplined deduction if the specification
     was good enough
   • e.g. by letting the user use the system under test

  AARHUS                            SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                 01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 8 -->

TYPES OF AUTOMATED SYSTEM TEST
  Two types of automated (functional) system tests:

  • Testing of GUI interaction

  • Testing executable specifications




  AARHUS                                SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                     01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 9 -->

AUTOMATED GUI TESTING
  Graphical/low level comparison
   • Suitable for look and feel tests
   • Exact X/Y coordinates, mouse and keyboard
   • Exact image comparison pixel by pixel
   • Intelligent image comparison – e.g. OCR / AI

  Using the Document Object Model
   • Suitable for multiplatform tests
   • Using UI element ID's to click and type
   • Using UI elements contents and state after action
   • https://fitnesse.org/ can create DOM-based tests

  AARHUS                                  SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                       01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 10 -->

EXCECUTABLE SPECIFICATIONS
                                                       Executable                                         Acceptance
                           Requirements
                                                      Specification                                          Test




                            Requirements                                                                Accept testing

                           System specification                                                    System testing

                                      System design                                   Integration testing

                                       Component design                     Unit test


                                                  Implement component


  AARHUS                                                         SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                              01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

**Figur:** Øverst en vandret kæde af tre dokumentformede bokse: Requirements ↔ Executable Specification ↔ Acceptance Test, forbundet af dobbeltrettede pile begge veje mellem hvert par — den eksekverbare specifikation er både oversættelse af kravene og selve accepttesten. Nedenunder V-modellen fra tidligere (Requirements/System specification/System design/Component design ned til Implement component, og Unit test/Integration testing/System testing/Accept testing op igen, med vandrette dobbeltpile mellem parrene). Det lyseblå felt fremhæver her kun de to øverste niveauer — Requirements ↔ Accept testing og System specification ↔ System testing — altså netop de niveauer, den eksekverbare specifikation dækker.

<!-- side 11 -->

GHERKIN FEATURE EXAMPLE
   Name                     Feature: MicrowavePower
 Description                   I want to cook or heat my food at the correct
                               power

                            @power1
   Name                     Scenario: Set Power once
  Arrange                      Given The oven is reset
    Act                        When I press the power button 1 time(s)
   Assert                      Then the display should show 50 W


                              This is also known as BDD: Behavior Driven Development

   AARHUS                                                     SWT     PETER HØGH MIKKELSEN
   UNIVERSITY                                          01 MA Y 2025   AU TOMATED SYSTEM TESTS
   TEC HNI CAL SCI EN CES

<!-- side 12 -->

EXECUTABLE SPECIFICATIONS WITH GHERKIN
  Advantages
    • Can involve the user/PO closer to the product
    • More concise than pure text
    • Combines requirements and test specification
    • Supports user stories, Behavior Driven Development, Feature Driven
      Development, agile processes
  Disadvantages
    • The general disadvantages mentioned above
    • You only get a code skeleton
    • Does the user trust this translation

  AARHUS                                  SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                       01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 13 -->

STRUCTURE OF GHERKIN ELEMENTS

                                           Executable
                                          Specification



                                                      1..*

                                                                                    Steps can be reused
                                               Feature
                                                                                   between features (with
                                                                                      context injection)
                                                        1..*

                                                                                     Steps can be reused
                                               Scenario
                                                                                     between Scenarios
                                        1..*          1..*     1..*

                              1..*             1..*                                     1..*


                           Given Step      When Step                                 Then Step



  AARHUS                                                                     SWT      PETER HØGH MIKKELSEN
  UNIVERSITY                                                          01 MA Y 2025    AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 14 -->

GHERKIN TEST ADAPTERS FOR .NET
  To run Gherkin-based tests with .NET you must:
    • Create a Unit Test Project (Ex Nunit)
    • Install a Cucumber/Gherkin test adapter: Reqnroll.Nunit (NuGet package)

        • This allows you to run Gherkin-based tests using ‘dotnet test’ or from your IDE’s Test
          Runner



        • You can find a script to create a Reqnroll project in an existing solution here:
             https://gitlab.au.dk/au-ece-swt/swt-student/-
             /blob/main/dotnet/projects/README.md



  AARHUS                                              SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                   01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 15 -->

GHERKIN SPECIFICATION WORKFLOW
 1) Create a .feature file
 2) Create feature(s) and scenario(s) as Gherkin specification(s)
 3) Create skeletons to Step Definitions
   • Reqnroll/SpecFlow: Right click and select Generate Step Definitions (In VS Code select
     feature and press ctrl+alt+2 to save a new step def file)
   • Save the step definition file
 4) Fill in the code required in the step definitions

 5) Run Tests with IDE’s test runner or ‘dotnet test’


                          Similar to TDD, you can use the tests derived from your
                          features to create the nescessary code, is known as BDD:
                          Behavior Driven Development
 AARHUS                                                    SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                         01 MA Y 2025   AU TOMATED SYSTEM TESTS
 TEC HNI CAL SCI EN CES

<!-- side 16 -->

GENERATE STEP DEFINITION
                           Visual Studio                                                    Visual Studio Code




                            Right-click                                             Select text and menu will appear
  AARHUS                                          SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                               01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

**Figur:** To skærmbilleder side om side. Venstre (Visual Studio): en .feature-fil MicrowaveSpec.feature åben med Gherkin-scenarier (`Feature: MicrowavePower`, `@power1 Scenario: Set Power once`, `@power2 Scenario: Set Power twice` med 100 W, `@power14 Scenario: Set Power 14` med 700 W, `@power15 Scenario: Set Power 15` der viser 50 W igen). En højreklik-kontekstmenu er åben, og menupunktet **Generate Step Definitions** er ringet ind med blåt; nederst ses Output-vinduet med "Build: 0 succeeded, 0 failed, 2 up-to-date, 0 skipped". Højre (Visual Studio Code): samme MicrowavePower.feature åben med farvet Gherkin-syntaks og grønne test-flueben ud for Scenario-linjerne; øverst en Quick-Pick-menu med valgmulighederne "BDD Step Definition - Generate Class to Clipboard" (^⇧1), "BDD Step Definition - Generate in an Existing File" (^⌘2) og "BDD Step Definition - Generate" (^⌘3), hvor sidstnævnte er ringet ind med blåt. Nederst VS Code-terminalen med `dotnet`-restore-output for Microwave.Classes og Microwave.Classes.Test.AcceptTest.

<!-- side 17 -->

 GENERATED CODE SKELETONS/TEMPLATES
                  namespace Microwave.Test.AcceptTest;
                      [Binding]                                                                                    Given: Arrange code here
                      public class MicrowavePowerSteps
                      {
                          [Given(@"The oven is reset")]
                          public void GivenTheOvenIsReset()                                                            Default code, must be
                          {                                                                                                  replaced
                              ScenarioContext.Current.Pending();
                          }
                                                                                                                        When: Act code here
                                  [When(@"I press the power button (.*) time\(s\)")]
                                  public void WhenIPressThePowerButtonTimeS(int p0)
Note! With                        {                                                                                Steps can be reused, because a
some Gherkin                          ScenarioContext.Current.Pending();                                            value is replaced by a regular
test adapters,                    }                                                                                            expression
this is called
[Steps].                          [Then(@"then the display should show (.*) W")]
Ex with
                                  public void ThenThenTheDisplayShouldShowW(int p0)
GherkinSpec.                                                                                                           Then: Assert code here
TestAdapter                       {
                                      ScenarioContext.Current.Pending();
         AARHUS                   } }                                  SWT  PETER HØGH MIKKELSEN
         UNIVERSITY                                                       01 MA Y 2025   AU TOMATED SYSTEM TESTS
         TEC HNI CAL SCI EN CES

**Figur:** Callout-linjer knytter de mørkeblå tekstbokse til bestemte kodelinjer: "Given: Arrange code here" peger på `[Given(@"The oven is reset")]`; "Default code, must be replaced" peger på `ScenarioContext.Current.Pending();`; "When: Act code here" peger på `[When(@"I press the power button (.*) time\(s\)")]`; "Steps can be reused, because a value is replaced by a regular expression" peger på samme When-attributs `(.*)`-mønster; "Then: Assert code here" peger på `[Then(@"then the display should show (.*) W")]`. Attributten `[Binding]` øverst er ringet ind med blåt, og en linje fra ringen fører til den blå note i venstre margen om `[Steps]` i andre test-adaptere.

<!-- side 18 -->

GIVEN STEP
public partial class MicrowaveSteps                                               powerButton = new Button();
    {                                                                             timeButton = new Button();
        private IOutput output;                                                   startCancelButton = new Button();
        private Timer timer;                                                      door = new Door();
        private Display display;                                                  timer = new Timer();
        private PowerTube powerTube;                                              display = new Display(output);
        private CookController cooker;                                            powerTube = new PowerTube(output);
        private UserInterface ui;                                                 light = new Light(output);
        private Light light;                                                      cooker = new CookController(timer,
        private Button powerButton;                                                              display, powerTube);
        private Button timeButton;                                                ui = new UserInterface(
        private Button startCancelButton;                                             powerButton, timeButton,
        private Door door;                                                            startCancelButton,
                                                                                      door,
             [Given(@"The oven is reset")]                                            display, light, cooker);
             public void GivenTheOvenIsReset()                                    cooker.UI = ui;
             {                                                            }
                 output = Substitute.For<IOutput>();                                            Note! Like with integration testing, we
                                                                                                aim to test the system, not just units!
    AARHUS
                                                                                                (Minimize the use of fakes to hardware,
                                                              SWT     PETER HØGH MIKKELSEN
    UNIVERSITY
    TEC HNI CAL SCI EN CES
                                                       01 MA Y 2025   AU TOMATED SYSTEM TESTS   external resources etc)

**Figur:** Koden er ét sammenhængende kodeeksempel opdelt i to spalter: venstre spalte er klassehovedet med felterne og starten på Given-metoden, højre spalte er fortsættelsen af samme metodekrop og den afsluttende `}`. Læsningen fortsætter altså fra `output = Substitute.For<IOutput>();` (venstre) direkte til `powerButton = new Button();` (højre). Kun IOutput erstattes af en substitut; alle øvrige objekter (Button, Door, Timer, Display, PowerTube, Light, CookController, UserInterface) instantieres som rigtige klasser — hvilket er pointen i den blå note nederst til højre.

<!-- side 19 -->

WHEN AND THEN STEPS
     [When(@"I press the power button (.*) time\(s\)")]
     public void WhenIPressThePowerButtonTimeS(int p0)
     {
         for (int i = 0; i < p0; ++i)
            powerButton.Press();
     }

     [Then(@"then the display should show (.*) W")]
     public void ThenThenTheDisplayShouldShowW(int p0)
     {
         output.
         Received(1).
         OutputLine(Arg.Is<string>(str =>
                       str.Contains($"Display shows: {p0} W")));
     }



  AARHUS                                        SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                             01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 20 -->

CONTEXT INJECTION
using Microwave.Classes.Boundary;                           public Button StartCancelButton { get; set; }
using Microwave.Classes.Controllers;                        public Door Door { get; set; }
using Microwave.Classes.Interfaces;                   }
using NSubstitute;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Accept.Support;

public class MicrowaveContext
{
   public IOutput Output { get; set; }
                                                                                             The context is
   public Timer Timer { get; set; }
   public Display Display { get; set; }
                                                                                           encapsulated in a
   public PowerTube PowerTube { get; set; }                                                 separate class
   public CookController Cooker { get; set; }
   public UserInterface UI { get; set; }
   public Light Light { get; set; }
   public Button PowerButton { get; set; }
   public Button TimeButton { get; set; }

    AARHUS                                                SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                                    01 MA Y 2025   AU TOMATED SYSTEM TESTS
    TEC HNI CAL SCI EN CES

<!-- side 21 -->

CONTEXT INJECTION - GIVEN STEP
namespace Microwave.Test.Accept.StepDefinitions;               _context.PowerTube = new PowerTube(_context.Output);
                                                               _context.Light = new Light(_context.Output);
[Binding]                                                      _context.Cooker = new CookController(_context.Timer,
public class MicrowavePowerStepDefinitions                       _context.Display, _context.PowerTube);
{                                                              _context.UI = new UserInterface(
   private readonly MicrowaveContext _context;                     _context.PowerButton,
   public MicrowavePowerStepDefinitions(                           _context.TimeButton,
     MicrowaveContext context)                                     _context.StartCancelButton,
   {                                                               _context.Door,
     _context = context;                                           _context.Display,
   }                                                               _context.Light,
                                                                   _context.Cooker);
  [Given(@"System is reset")]                                  _context.Cooker.UI = _context.UI;
  public void GivenSystemIsReset()                             }
  {
  _context.Output = Substitute.For<IOutput>();
  _context.PowerButton = new Button();
  _context.TimeButton = new Button();                              The test adapter injects the context automatically
  _context.StartCancelButton = new Button();                           Additional setup can be done with hooks:
  _context.Door = new Door();
                                                                https://docs.reqnroll.net/latest/automation/hooks.html
  _context.Timer = new Timer();
  _context.Display = new Display(_context.Output);

     AARHUS                                                 SWT     PETER HØGH MIKKELSEN
     UNIVERSITY                                      01 MA Y 2025   AU TOMATED SYSTEM TESTS
     TEC HNI CAL SCI EN CES

<!-- side 22 -->

CONTEXT INJECTION - WHEN AND THEN
  [When(@"I press Power Button (.*) time\(s\)")]
  public void WhenIPressPowerButtonTimeS(int count)
  {
        for (int i = 0; i < count; i++)
               _context.PowerButton.Press();
  }

  [Then(@"The Display will show (.*) W")]
  public void ThenTheDisplayWillShowW(int expectedWatts)
  {
    _context.Output.Received(1).OutputLine($"Display shows:
                                                                                        The tests work on the
  {expectedWatts} W");
                                                                                           given context
  }


  AARHUS                                              SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                   01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

<!-- side 23 -->

MATCHING GENERATED CODE

                                          Executable
                                         Specification



                                                     1..*                                              A step becomes a
                                                                                                       method in a class
                                              Feature
                                                                     1..*
                                                                                             1..*
                                                                                                                 FeatureSteps
                                                       1..*                                          - sut
                                                                                                     + GivenXxxYyy(int)
                                              Scenario                                               + WhenAAABBB()
                                                                                                     + ThenMmmNnn(string)

                                       1..*          1..*     1..*

                             1..*             1..*                                    1..*


                          Given Step      When Step                               Then Step




 AARHUS                                                                            SWT       PETER HØGH MIKKELSEN
 UNIVERSITY                                                                 01 MA Y 2025     AU TOMATED SYSTEM TESTS
 TEC HNI CAL SCI EN CES

<!-- side 24 -->

COMBINING EXECUTABLE SPECIFICATIONS
WITH GUI TESTING
  1. Create a scenario
      • Given weight is set to 92 kgs
        and height is set to 1.93 meters.
        When ”Calculate BMI” is pressed,
        Then the BMI should be 24.7



  2. Use the GUI test framework to test from the
     generated code


  AARHUS                                  SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                       01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

**Figur:** Skærmbillede af BMI-regnerens vindue til højre: titellinje "BMI Calc..." med minimér/maksimér/luk-knapper, teksten "Please enter your weight and height", inputfeltet "Your weight" med værdien 92.0 efterfulgt af "kg.", inputfeltet "Your height" med værdien 1.93 efterfulgt af "meters", en knap "Calculate BMI" og nederst "Your BMI:" med resultatet 24.7 vist på grøn baggrund. GUI'ens felter svarer én-til-én til Given/When/Then-trinnene i scenariet til venstre.

<!-- side 25 -->

SYSTEM AND CODED UI TESTS




  AARHUS                          SWT     PETER HØGH MIKKELSEN
  UNIVERSITY               01 MA Y 2025   AU TOMATED SYSTEM TESTS
  TEC HNI CAL SCI EN CES

**Figur:** Fire kolonner side om side, der sammenligner testtilgange langs en akse fra Manual (grøn streg under kolonne 1) til Coded (blå pil under kolonne 2-4).

<!-- side 26 -->

AARHUS
UNIVERSITY

