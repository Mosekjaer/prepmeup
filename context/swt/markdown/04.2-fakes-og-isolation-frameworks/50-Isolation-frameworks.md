---
title: "Isolation frameworks"
source: "csfiles/home_dir/FakesAndIsolation/Isolation frameworks.pdf"
modul: "Lektion 04.2: Fakes og Isolation Frameworks"
pages: 10
type: "slides"
vision: "done"
---
# Isolation frameworks

<!-- side 1 -->

ISOLATION FRAMEWORKS
I4SWT




AARHUS                                               LUKAS ESTERLE
UNIVERSITY                                    2024   ASSOCIATE PROFESSOR
D E P A R T M E N T O F E N G IN E E R IN G

<!-- side 2 -->

AARHUS                             LUKAS ESTERLE
UNIVERSITY                  2024   ASSOCIATE PROFESSOR
DEPARTMENT OF ENGINEERING

<!-- side 3 -->

ISOLATION FRAMEWORKS
 A tool for creating fakes (stubs and mocks) and thus isolate our UUT easily
      • Exists for many languages (C++, C# , Java)…

 The basic idea is the same: Create fakes from interface definitions only!

 One Isolation Framework for C# is NSubstitute
         •      http://nsubstitute.github.io/

 To use NSubstitute, add NSubstitute via NuGet Package Manager in Visual
 Studio

 AARHUS                                                LUKAS ESTERLE
 UNIVERSITY                                     2024   ASSOCIATE PROFESSOR
 DEPARTMENT OF ENGINEERING

<!-- side 4 -->

NSUBSTITUTE – CREATING A SUB(STITUTE)
                              namespace ECS.Tests.Unit
                              {
                                  [TestFixture]
                                  public class ECSUnitTests
                                  {                                                ITemp-       ECS        IHeater
                                      private ECS _uut;                            Sensor
                                      private IHeater _heater;
                                      private ITempSensor _tempSensor;


                                      [SetUp]
                                      public void Setup()                                      Very important!
                                      {                                                        Nsubstitute fakes must
                                          _heater = Substitute.For<IHeater>();                 always be created from
                                          _tempSensor = Substitute.For<ITempSensor>();         interfaces!
                                          _uut = new ECS(25, _tempSensor, _heater);
                                      }

                                      // TEST CASES GO HERE!
                                  }
                              }                                 Note: No need to hand-code fake classes
  AARHUS                                                        implementing
                                                                       LUKAS ESTERLE
                                                                                     the interfaces
  UNIVERSITY                                                      2024   ASSOCIATE PROFESSOR
  DEPARTMENT OF ENGINEERING

**Figur:** Til højre for kodeboksen et lille dependency-diagram med tre lyseblå kasser på række: `ITemp-Sensor` — `ECS` — `IHeater`. `ECS` står i midten; en pil peger fra `ECS` mod venstre ind i `ITempSensor`, og en pil peger fra `ECS` mod højre ind i `IHeater` — UUT'en afhænger altså udelukkende af de to interfaces. To tykke mørkeblå blokpile peger skråt op i henholdsvis `ITempSensor`- og `IHeater`-kassen fra neden, og to tilsvarende blokpile i kodeboksen peger på linjerne `_heater = Substitute.For<IHeater>();` og `_tempSensor = Substitute.For<ITempSensor>();` — koblingen mellem interface i diagrammet og substitute-kaldet i koden.
Den røde advarselsboks "Very important! Nsubstitute fakes must always be created from interfaces!" står under diagrammet; "Very important!" er i fed.

<!-- side 5 -->

NSUBSTITUTE – SETTING RETURN VALUES
AND EXPECTATIONS
                                [Test]
                              public void RunSelfTest_TempSensorFails_SelfTestFails()
                              {
                                   _tempSensor.RunSelfTest().Returns(false);
                                   _heater.RunSelfTest().Returns(true);
                                   Assert.IsFalse(_uut.RunSelfTest());
                              }                                              Both subs used as stubs in
                                                                                       state-based test


                                [Test]
                              public void Regulate_TempBelowThreshold_HeaterTurnedOn()
                              {
                                   _tempSensor.GetTemperature().Returns(15);
                                   _uut.Regulate();
                                   _heater.Received(1).TurnOn();
                              }
                                                                          _heater sub used as mock in
                                                                          interaction-based test. Received() is
                                                                          an ”implicit assertion”

  AARHUS                                                                   LUKAS ESTERLE
  UNIVERSITY                                                       2024    ASSOCIATE PROFESSOR
  DEPARTMENT OF ENGINEERING

**Figur:** To adskilte kodebokse, én pr. testmetode. Blå blokpile i venstre margen peger på de linjer, der udgør pointen: i den øverste boks på begge `Returns(...)`-linjer (`_tempSensor.RunSelfTest().Returns(false);` og `_heater.RunSelfTest().Returns(true);`), i den nederste boks på `_heater.Received(1).TurnOn();`. Callout-boksen "Both subs used as stubs in state-based test" hører til den øverste boks; callouten "_heater sub used as mock in interaction-based test. Received() is an 'implicit assertion'" peger med pil direkte på `_heater.Received(1).TurnOn();`. Ordene *stubs* og *mock* er kursiveret — kontrasten stub/state-based vs. mock/interaction-based er slidets akse.

<!-- side 6 -->

NSUBSTITUTE –
STUBS WITH ARGUMENT MATCHING
                       [Test]
                       public void Calculator_Add_ResultStored()
                       {
                           // Arrange
                           var sub = Substitute.For<ICalculatorBrain>();
                           var uut = new Calculator(sub);

                             uut.SetFirstArgument(1);
                             uut.SetSecondArgument(3);
                             uut.SetOperation(Calculator.Operations.Add);
                                                                                Will return 4 only with
                             // Arrange stub                                    arguments exactly equal to
                             sub.Add(1, 3).Returns(4);                          1 and 3!
                             // Act
                             uut.Execute();

                             // Act
                             Assert.That(uut.Accumulator, Is.EqualTo(4));
                       }



                             // Arrange stub                                                  Will return 4 for any set of
                             sub.Add(Arg.Any<double>, Arg.Any<double>).Returns(4);            arguments!

 AARHUS                                                                     LUKAS ESTERLE
 UNIVERSITY                                                          2024   ASSOCIATE PROFESSOR
 DEPARTMENT OF ENGINEERING

**Figur:** Én stor kodeboks foroven med hele testmetoden, plus en separat lille kodeboks forneden med kun to linjer (`// Arrange stub` og `sub.Add(Arg.Any<double>, Arg.Any<double>).Returns(4);`), hvor `Arg.Any<double>` er sat i fed. De to blå callouts hører til hver sin variant: "Will return 4 only with arguments exactly equal to 1 and 3!" står ud for `sub.Add(1, 3).Returns(4);` i den store boks, og "Will return 4 for any set of arguments!" står ud for den lille boks. Den visuelle adskillelse markerer, at den nederste linje er en alternativ erstatning for den ene linje i den store boks, ikke ekstra kode.

<!-- side 7 -->

NSUBSTITUTE –
MOCKS WITH ARGUMENT MATCHING
                             [Test]
                             public void Calculator_Add_ProperArgumentsUsed()
                             {
                                 var sub = Substitute.For<ICalculatorBrain>();
                                 var uut = new Calculator(sub);

                                 uut.SetFirstArgument(1);
                                 uut.SetSecondArgument(3);
                                 uut.SetOperation(Calculator.Operations.Add);       This will only pass if Add
                                 uut.Execute();                                     was called with arguments
                                 sub.Received().Add(1,3);                           exactly equal to 1 and 3!
                             }



                             [Test]
                             public void Calculator_Add_SubtractNotCalled()
                             {
                                 var sub = Substitute.For<ICalculatorBrain>();
                                 var uut = new Calculator(sub);

                                 uut.SetFirstArgument(1);
                                 uut.SetSecondArgument(3);                                   This will pass if Subtract was
                                 uut.SetOperation(Calculator.Operations.Add);
                                 uut.Execute();
                                                                                             not called at all
                                 sub.DidNotReceive().Subtract(Arg.Any<double>,Arg.Any<double>);
 AARHUS                          // DidNotReceive() is the same as Received(0)
                                                                             LUKAS ESTERLE
 UNIVERSITY                  }                                         2024   ASSOCIATE PROFESSOR
 DEPARTMENT OF ENGINEERING

**Figur:** To kodebokse, én pr. testmetode. Assert-linjerne er sat i fed: `sub.Received().Add(1,3);` i den øverste og `sub.DidNotReceive().Subtract(Arg.Any<double>,Arg.Any<double>);` plus kommentarlinjen i den nederste. Blå callout "This will only pass if Add was called with arguments exactly equal to 1 and 3!" står ud for den øverste boks' `Received()`-linje; "This will pass if Subtract was not called at all" ud for den nederste boks' `DidNotReceive()`-linje.

<!-- side 8 -->

NSUBSTITUTE –
DANGERS OF ARGUMENT MATCHING

                             [Test]
                             public void Calculator_Add_SubtractNotCalled()
                             {
                                 var sub = Substitute.For<ICalculatorBrain>();
                                 var uut = new Calculator(sub);

                                 uut.SetFirstArgument(1);
                                 uut.SetSecondArgument(3);
                                 uut.SetOperation(Calculator.Operations.Add);          This will pass even if
                                 uut.Execute();
                                 sub.DidNotReceive().Subtract(3, 1);
                                                                                       Subtract was actually
                             }                                                         called!




 AARHUS                                                                       LUKAS ESTERLE
 UNIVERSITY                                                            2024   ASSOCIATE PROFESSOR
 DEPARTMENT OF ENGINEERING

**Figur:** Én kodeboks, næsten identisk med forrige slides nederste test, men med `sub.DidNotReceive().Subtract(3, 1);` (konkrete argumenter i ombyttet rækkefølge i stedet for `Arg.Any<double>`) sat i fed. En **rød** callout-boks — farven markerer faldgruben — står ud for netop den linje: "This will pass even if Subtract was actually called!". Pointen ligger i kontrasten til forrige slide: argument-matchning på `DidNotReceive()` gør assertionen falsk-positiv, fordi den kun udelukker kaldet med præcis (3, 1).

<!-- side 9 -->

NSUBSTITUTE – THROWING EXCEPTIONS
                       [Test]
                       public void Calculator_DivideSecondOperandIsZero_ExceptionThrown()
                       {
                           var sub = Substitute.For<ICalculatorBrain>();    // Create the substitute
                           var uut = new Calculator(sub);

                              uut.SetFirstArgument(6);
                              uut.SetSecondArgument(0);
                              uut.SetOperation(Calculator.Operations.Divide);

                              sub
                                    .When( x => x.Divide(6,0))
                                    .Do( x => {throw new ArgumentException(); });

                              Assert.Throws<ArgumentException>(() => uut.Execute());
                       }



  AARHUS                                                                 LUKAS ESTERLE
  UNIVERSITY                                                      2024   ASSOCIATE PROFESSOR
  DEPARTMENT OF ENGINEERING

<!-- side 10 -->

NSUBSTITUTE WRAP-UP

  Basically, whatever you want a fake to do, NSubstitute will do it –
  and easy!


  Learn your knife skills! NSubstitute is only useful with routine!




  AARHUS                                  LUKAS ESTERLE
  UNIVERSITY                       2024   ASSOCIATE PROFESSOR
  DEPARTMENT OF ENGINEERING

