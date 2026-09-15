---
title: "Exception-Testing"
source: "Exception-Testing.pdf"
modul: "Lektion 01.2: Exceptions + Git Workflow"
pages: 7
type: "slides"
vision: "done"
---
# Exception-Testing

<!-- side 1 -->

EXCEPTION TESTING
WITH NUNIT


AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   EXCEPTION TESTING WITH NUNIT
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

EXCEPTIONS AND TEST
  Exceptions must also be tested

  Exceptions are part of the ”contract” for using a class
         • If you do so-and-so, this particular exception will be thrown


  Create test cases that verify that exceptions occur when they
  should




  AARHUS                                         SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                            FEBRUARY 2026   EXCEPTION TESTING WITH NUNIT
  T ECH N IC AL SC IEN C ES

<!-- side 3 -->

EXCEPTIONS AND TEST

                                                                                  The constraint specifies
           // Testing using constraints                                            the expected exception
           [Test]                                                                           type
           public void TestThatThrowsException3()
           {
              uut = new UUT();
              uut.SetUpScenario();
              Assert.That(() => uut.StuffThatThrowsException(), Throws.TypeOf<MyException>());
           }



                                    First parameter must be a
                                   function/method with zero
                                    parameters, here defined
                                   using a lambda expression

  AARHUS                                                SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                   FEBRUARY 2026   EXCEPTION TESTING WITH NUNIT
  T ECH N IC AL SC IEN C ES

**Figur:** Kodeboks med to mørkeblå taleboble-callouts, der peger på hver sin del af Assert-linjen. Callout'en øverst til højre peger på `Throws.TypeOf<MyException>()` og angiver, at constraint'en fastlægger den forventede exception-type. Callout'en nederst peger på lambdaen `() => uut.StuffThatThrowsException()` og angiver, at første parameter skal være en funktion/metode uden parametre, her skrevet som lambda-udtryk.

<!-- side 4 -->

EXCEPTIONS AND TEST

                              // Testing for values in the exception using additional constraints
                              [Test]
                              public void TestThatThrowsException3()
                              {
                                 uut = new UUT();
                                 uut.SetUpScenario();
                                 Assert.That(() => uut.StuffThatThrowsException(),
                              Throws.TypeOf<MyException>().With.Property("Value").EqualTo(42));

                              }




  AARHUS                                                           SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                              FEBRUARY 2026   EXCEPTION TESTING WITH NUNIT
  T ECH N IC AL SC IEN C ES

<!-- side 5 -->

EXCEPTIONS – THE BASIC PRINCIPLES (6)
  A class can throw different exceptions.

              public class CourseException : Exception               class Ship
              {                                                      {
                public uint Course { get; private set; }                public uint Course { get; private set; }
                public CourseException(uint course)                     public uint Speed { get; private set; }
                {
                  Course = course;                                           public Ship(uint course, uint speed)
                }                                                            {
              }                                                                if (speed < 100)
                                                                                     Speed = speed;
              public class SpeedException : Exception                          else
              {                                                                      throw new SpeedException(speed);
                public uint Speed { get; private set; }
                public SpeedException(uint s)                                    if (course < 360)
                {                                                                      Course = course;
                  Speed = s;                                                     else throw new CourseException(course);
                }                                                            }
              }                                                      }



  AARHUS                                                      SWT        PETER HØGH MIKKELSEN
  UNIVERSITY                                         FEBRUARY 2026       EXCEPTION TESTING WITH NUNIT
  T ECH N IC AL SC IEN C ES

**Figur:** Tre kodebokse: venstre kolonne har to exception-klasser over hinanden, højre kolonne Ship-klassen. Begge exception-typer arver fra Exception og gemmer den ugyldige værdi i en read-only property. Ship-konstruktøren validerer i rækkefølgen speed før course, så en Ship med både ugyldig speed og ugyldig course kaster SpeedException — CourseException nås aldrig i det tilfælde.

<!-- side 6 -->

EXCEPTIONS AND TEST

                                                                                  class Ship
                                                                                  {
                                                                                     public uint Course { get; private set; }
                                                                                     public uint Speed { get; private set; }
// Testing for values in the exception using constraints
[Test]                                                                                     public Ship(uint course, uint speed)
public void Ctor_SetInvalidCourse_ThrowsCourseException()                                  {
{                                                                                            if (speed < 100)
   Assert.That(() => new Ship(789),                                                                Speed = speed;
               Throws.TypeOf<CourseException>());                                            else
}                                                                                                  throw new SpeedException(speed);

                                                                                               if (course < 360)
                                                                                                     Course = course;
                                                                                               else throw new CourseException(course);
                                                                                           }
                                                                                  }




     AARHUS                                          SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                             FEBRUARY 2026   EXCEPTION TESTING WITH NUNIT
     T ECH N IC AL SC IEN C ES

**Figur:** To kodebokse side om side: testmetoden til venstre, Ship-klassen (samme kode som side 5) til højre som reference, så man kan se hvilken gren i konstruktøren testen rammer. Bemærk at kaldet i testen er `new Ship(789)` med kun ét argument, mens konstruktøren er deklareret `Ship(uint course, uint speed)` — slidet er inkonsistent på det punkt.

<!-- side 7 -->

AARHUS
UNIVERSITY

