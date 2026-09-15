---
title: "Introduction to Unit Tests.pdf"
source: "Introduction to Unit Tests.pdf"
modul: "Lektion 01.1: Introduktion + Unit Test"
pages: 21
type: "slides"
vision: "done"
---
# Introduction to Unit Tests.pdf

<!-- side 1 -->

SW4SWT
INTRODUCTION TO UNIT
TESTING

AARHUS                           SWT    PETER HØGH MIKKELSEN
UNIVERSITY               AU GUST 2025   SW4SW T TEM PLATE
TEC HNI CAL SCI EN CES

<!-- side 2 -->

HAND-TEST OF THE CALCULATOR CLASS
public class Calculator
{
    public double Add(double a,class
                                 double  b)
                                     Program
    {                          {
        return a + b;              static void Main(string[] args)
    }                              {
                                        // Declare the unit-under-test
    public double Subtract(double a, double   b) = new Calculator();
                                        var uut
    {
        return a - b;                   // Test Add()
    }                                   Console.WriteLine("Add({0}, {1}) = {2}", 3.5, 2.5, uut.Add(3.5, 2.5));
                                        Console.WriteLine("Add({0}, {1}) = {2}", -3.5, 2.5, uut.Add(-3.5, 2.5));
    public double Multiply(double a, double   b)
                                        Console.WriteLine("Add({0},  {1}) = {2}", -3.5, -2.5, uut.Add(-3.5, -2.5));
    {
        return a * b;                   // Test Subtract()
    }                                   Console.WriteLine("Subtract({0}, {1}) = {2}", 3.5, 2.5, uut.Subtract(3.5, 2.5));
                                        Console.WriteLine("Subtract({0}, {1}) = {2}", -3.5, 2.5, uut.Subtract(-3.5, 2.5));
    public double Power(double a, double   b)
                                        Console.WriteLine("Subtract({0},  {1}) = {2}", -3.5, -2.5, uut.Subtract(-3.5, -2.5));
    {
        return Math.Pow(a,b);           // Test Multiply()
    }                                   Console.WriteLine("Multiply({0}, {1}) = {2}", 3.5, 2.5, uut.Multiply(3.5, 2.5));
}                                       Console.WriteLine("Multiply({0}, {1}) = {2}", -3.5, 2.5, uut.Multiply(-3.5, 2.5));
                                        Console.WriteLine("Multiply({0}, {1}) = {2}", -3.5, -2.5, uut.Multiply(-3.5, -2.5));

                                       // Test Power()
                                       Console.WriteLine("Power({0}, {1}) = {2}", 2.0, 3.0, uut.Power(2.0, 3.0));
                                       Console.WriteLine("Power({0}, {1}) = {2}", -2.0, 3.0, uut.Power(-2.0, 3.0));
                                       Console.WriteLine("Power({0}, {1}) = {2}", -2.0, -3.0, uut.Power(-2.0, -3.0));
                                   }
                               }
     AARHUS                                                             SWT     PETER HØGH MIKKELSEN
     UNIVERSITY                                                  AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
     TEC HNI CAL SCI EN CES

**Figur:** Tre overlappende paneler i kortstak, der læses fra venstre mod højre som «unit under test → testkode → resultat». Bagerst til venstre kodeboksen med `public class Calculator` og dens fire metoder `Add`, `Subtract`, `Multiply` og `Power` (`return Math.Pow(a,b);`). Oveni ligger kodeboksen med `class Program` og dens `Main`, hvor `var uut = new Calculator();` oprettes og hver metode testes med `Console.WriteLine`-kald. Forrest til højre et skærmbillede af et rigtigt konsolvindue med titellinjen `C:\WINDOWS\system32\cmd.exe` (minimér/maksimér/luk-knapper og rullebjælke), der viser programmets faktiske output med dansk decimalkomma:
```
Add(3,5, 2,5) = 6
Add(-3,5, 2,5) = -1
Add(-3,5, -2,5) = -6
Subtract(3,5, 2,5) = 1
Subtract(-3,5, 2,5) = -6
Subtract(-3,5, -2,5) = -1
Multiply(3,5, 2,5) = 8,75
Multiply(-3,5, 2,5) = -8,75
Multiply(-3,5, -2,5) = 8,75
Power(2, 3) = 8
Power(-2, 3) = -8
Power(-2, -3) = -0,125
Press any key to continue . . .
```
Outputtet er rå tal uden forventede værdier — manuel inspektion er eneste «assert», hvilket er slidens pointe.

<!-- side 3 -->

DID WE CONSIDER…

  Which state was the UUT in before the test case…?
  Was the actual result correct…?
  Was success or failure reported…?
  Were all methods tested…?
  Were all situations tested for each method…?




  AARHUS                                 SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                      AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 4 -->

WHAT MAKES A GOOD UNIT TEST?
  A unit test should…
        • Be automated and repeatable.
        • Be easy to implement.
        • Remain valid for future use, once written
        • Be usable (runnable) by anyone, once written
        • Run at the push of a button.
        • Run quickly.


  Any deviation indicates problems


  AARHUS                                     SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                          AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 5 -->

WHAT’S IN A MAINTAINABLE TEST?
  Easy to understand
  Only tests a small and clearly bounded area of the Unit Under Test (UUT)
  Independent/isolated from other parts
    • we can change one test case without knowing or breaking the other test
      cases
  Robust
    • not brittle or easily broken when implementation details in UUT are
      changed




  AARHUS                                 SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                      AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 6 -->

AAA: ARRANGE-ACT-ASSERT
  A good pattern for each test case:

  Arrange (precondition):
   • Setup up the UUT and test case in a desired state

  Act:
   • Execute the activity to be tested

  Assert (postcondition):
   • Check that the expected things happened

 AARHUS                                   SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                        AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
 TEC HNI CAL SCI EN CES

<!-- side 7 -->

WISH LIST FOR A UNIT TEST FRAMEWORK?




  AARHUS
                                    … and friends
                                  SWT     PETER HØGH MIKKELSEN
  UNIVERSITY               AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

**Figur:** Seks gule post-it-sedler med håndskriftsfont, spredt i to kolonner med let rotation, hver med et ønske til et testframework. Venstre kolonne oppefra: «Easy addition of new tests», «Support for test automation», «Test setup and teardown». Højre kolonne: «Good assertion constructs», «Detailed reports - What failed, and why?», «Tests for ex-ceptions». Øverst til højre et rødt WANTED-stempel skråtstillet. Under de seks sedler samler en stor tuborgklamme dem alle og peger ned på NUnit-logoet (det klassiske «N unit»-mærke i grå/blåt), efterfulgt af teksten «… and friends» hvor «friends» er et link. Alle seks ønsker opfyldes altså af NUnit og tilsvarende frameworks.

<!-- side 8 -->

NUNIT IS ONE SUCH TEST FRAMEWORK
  Nunit supports (among other things)
   • Test case Individual tests
        • Test fixtures Per-unit setup/teardown, etc.
        • Test runner The ”thing” that runs the tests and reports the result
        • Test reports Result of the tests run




  Note: All (relevant) unit test frameworks support the same features, so they tend to look similar.


  AARHUS                                                           SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                                AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 9 -->

THE GREEN ZONE
  Separate your unit tests from other, more complicated tests
    • E.g. integration tests

  This gives developers a safe green zone in which…
    • only unit tests are contained
    • they know that they can get the latest code version,
    • they can run all tests in that namespace or folder, and they should all be green.

  If tests in the green zone fail, there’s a real problem, not a false positive e.g. due to
  configuration problem in the test.

  Green zone tests are quick, so they can be run often

 AARHUS                                         SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                              AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
 TEC HNI CAL SCI EN CES

<!-- side 10 -->

AAA IN NUNIT
  NUnit supports this pattern in a [TestFixture]

  [SetUp] method assists in Arrange of common stuff, before a test is called

  The individual [Test]s are standalone methods
    • further Arrange steps (if necessary)
    • Act is executed (if necessary)
    • Assert is done

  NUnit has a flexible set of Assert possibilities
   • with reporting

  AARHUS                                     SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                          AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 11 -->

NUNIT ASSERTIONS –
CLASSIC VS. CONSTRAINT-BASED
  The classic assert model                    Assert.AreEqual ( 10, x);



                                                                          Expected state Actual state


  The constraint-based model            Assert.That(x,Is.EqualTo(10));



                                                   Actual state                     Constraint with
                                                                                    expected state
  AARHUS                              SWT      PETER HØGH MIKKELSEN
  UNIVERSITY                   AU GUST 2025    SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

**Figur:** To lyseblå kodebokse, hver med to tykke mørkeblå pile op fra forklarende tekst nedenunder. Øverst boksen `Assert.AreEqual ( 10, x);` — venstre pil peger op på argumentet `10` med teksten «Expected state», højre pil peger op på `x` med teksten «Actual state». Nederst boksen `Assert.That(x,Is.EqualTo(10));` — venstre pil peger op på `x` med teksten «Actual state», højre pil peger op på `Is.EqualTo(10)` med teksten «Constraint with expected state». Argumentrækkefølgen er altså byttet om mellem de to modeller: klassisk sætter forventet værdi først, constraint-baseret sætter faktisk værdi først.

<!-- side 12 -->

NUNIT DOCUMENTATION
 https://docs.nunit.org/articles/nunit/intro.html

 NUnit supports numerous constraints

 Most follow the patterns shown on the following slides




 AARHUS                              SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                   AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
 TEC HNI CAL SCI EN CES

<!-- side 13 -->

ASSERT VERBS
  Assert.That(<item>, <verb>.<constraint>)

  <item > can be:
     • Value- ex. return value from method call or property
     • reference to an object
     • Method or lambda function, either without parameters

  <verb> can be:
    • Is.
    • Has.
    • Contains. (<item> must be string or collection)
    • Does.                                                                            Most, but not all constraints follow this pattern!
    • Throws. (<item> must be method or lambda)
  AARHUS                                         SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                              AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 14 -->

CONSTRAINTS
 <constraint>
    .EqualTo(<value>)                                          .Throw. – metoder/lambda
    .GreaterThan(<value>) /                                    .Exactly(<antal>). – collections
    .LessThan(<value>)                                         .None. (prefix) - collections
    .InRange(<value>, <value>)                                 .Some. (prefix) - collections
    .Zero                                                      .All. (prefix) - collections
    .False / .True                                             .Unique - collections
    .Null                                                      .Exist – filer og foldere
    .Empty – strings og collections                            .TypeOf<> - polymorfi og exceptions
    .EndWith(“”) / .StartWith(“”) - strings                    .InstanceOf<> - polymorfi og exceptions
    .Contain() – strings og collections




 AARHUS                                          SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                               AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
 TEC HNI CAL SCI EN CES

<!-- side 15 -->

CONSTRAINT MODIFIERS
  Modifiers can sættes på constraints:
     .Not. (prefix)
     .And. / .Or. (infix)
     .Within(<precision>) (postfix) double/float/tider
     .After. (postfix) tidsmæssig opfyldelse




  AARHUS                                         SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                              AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 16 -->

ANATOMY OF A TEST CLASS
namespace CalcTest;                           // ...cont
using CalcLib;                                [Test]
                                                public void Add_AddTwoInts_SumIsCorrect()
public class Tests {                          {
  private Calculator _uut;                        // Arrange

  [SetUp] // Common Arrange                              // Act
  public void Setup() {                                  double result = _uut.Add(2, 3);
    _uut = new Calculator();                             //Assert
  }                                                      Assert.That(result, Is.EqualTo(5.0));
                                                   }
// cont...                                    }


    AARHUS                            SWT         PETER HØGH MIKKELSEN
    UNIVERSITY                 AU GUST 2025       SW4SW T INTRODUCTION TO UNIT TESTS
    TEC HNI CAL SCI EN CES

**Figur:** Koden er delt i to lyseblå paneler side om side, der læses som én sammenhængende fil: venstre panel slutter med kommentaren `// cont...` og højre panel begynder med `// ...cont`, dvs. testmetoden `Add_AddTwoInts_SumIsCorrect()` ligger inde i `public class Tests`. Kommentarerne `// Arrange`, `// Act` og `//Assert` i højre panel markerer AAA-mønsterets tre trin, hvor Arrange-trinnet er tomt, fordi det fælles arrange sker i `[SetUp]`-metoden `Setup()` i venstre panel.

<!-- side 17 -->

ONE TEST CLASS PER CLASS IN LIB/APP
   Logical, easy to understand and navigate

   Naming convention identifies method, scenario and expected result, e.g.




                            Add_AddTwoInts_SumIsCorrect()

Name of function to test                                       Expected outcome
                                   Action




   AARHUS                                          SWT     PETER HØGH MIKKELSEN
   UNIVERSITY                               AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
   TEC HNI CAL SCI EN CES

<!-- side 18 -->

RULES FOR GOOD TEST CODE
  Only one scenario in each test method
  Only one assert in each test method (or asserts concerning the same
  scenario)
  Don't write complex test cases/methods
    • no if/else, switch, for loops, etc.
    • for same test code/asserts but different input values, use [TestCase(…)]
    • different output/behaviour are often different scenarios




  AARHUS                                   SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                        AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 19 -->

TEST RUNNERS

                                                                                      Metadata
                                                        Calculator.Test.Unit.dll
                                                            [TestFixture]
                           Calculator.dll      uses                                                uses         Nunit.Framework.dll
                                                                [Test]
                                                               [Setup]...


                                                                          Search for and run tests

                                                                                                                  Needs    Test Runners
                                                                                                          NUnit3Adapter
                               Nunit3-                                              dotnet test      NunitXML.Testlogger
                                            Resharper     Test Explorer
                             console from
                                             from VS          in VS
                                                                                     from the                                 and more
                            command line                                           command line




                                                                       Test Results




  AARHUS                                                       SWT        PETER HØGH MIKKELSEN
  UNIVERSITY                                            AU GUST 2025      SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 20 -->

DEMO TIME: CASH REGISTER
  Application project: A cash register

                                               Register

                           - noOfItems : int
                           - total : double

                           + AddItem(double) : void
                           + GetNoOfItems() : int
                           + GetTotal() : double


                                                                    AddItem with a
                                                                    negative item will
                                                                    throw an exception


  Discuss:
        • What test cases do we need for Register?
        • What is the input and expected result for each test case?

  AARHUS                                                         SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                              AU GUST 2025   SW4SW T INTRODUCTION TO UNIT TESTS
  TEC HNI CAL SCI EN CES

<!-- side 21 -->

AARHUS
UNIVERSITY

