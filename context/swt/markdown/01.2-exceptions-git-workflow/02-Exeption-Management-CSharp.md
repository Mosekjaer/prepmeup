---
title: "Exeption-Management-CSharp"
source: "Exeption-Management-CSharp.pdf"
modul: "Lektion 01.2: Exceptions + Git Workflow"
pages: 16
type: "slides"
vision: "done"
---
# Exeption-Management-CSharp

<!-- side 1 -->

EXCEPTION MANAGEMENT
IN C#


AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

ERROR MANAGEMENT IN GENERAL
 Most production code has 2 sets of paths
        • Happy paths:       Things go as planned
        • Error paths:       We detect and handle errors


 Solid, robust code needs to detect errors and handle them

 Error detection/handling code in the “normal flow code”
 obscures normal flow and makes code messy


 AARHUS                                   SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                      FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
 T ECH N IC AL SC IEN C ES

<!-- side 3 -->

ERROR MANAGEMENT IN GENERAL
 Often, we cannot even handle the error where we detect it.

     class Ship                                                    class MemoryMgr
     {                                                             {
       private uint _course;                                         public MemoryBlock GetBlock(uint size)
       public void Ship(uint initialCourse)                          {
       {                                                               if(canAllocate(size)) return findBlock();
         if(c > 360) // Error – but what to do?                        else // Error – but what to return?
         else _course = initialCourse;                               }
       }
     }                                                                 public bool canAllocate(uint size) {…}
                                                                   }



            It’s more logical to handle an error further up the call chain from were it was detected

 AARHUS                                                  SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                     FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
 T ECH N IC AL SC IEN C ES

**Figur:** To kodebokse side om side, som illustrerer det samme problem i to forskellige domæner: `Ship`-konstruktoren kan detektere `c > 360`, men har ingen returværdi at melde fejlen med; `MemoryMgr.GetBlock` har en returværdi (`MemoryBlock`), men ingen fornuftig værdi at returnere ved fejl. Fejlkommentarerne står i grøn kursiv i begge bokse. Konklusionslinjen står centreret under begge bokse og gælder dem begge.

<!-- side 4 -->

OUR WISHES FOR ERROR MANAGEMENT
 Thus, we have some wishes for our error detection and handling
 scheme: We want to be able to…
        • separate ”normal flow” and ”error management” code
        • separate error detection and error handling
        • detect errors in one place and handle them ”further up” the control
          chain


 …And this is exactly what exceptions are about (and some
 added niceties)


 AARHUS                                        SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                           FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
 T ECH N IC AL SC IEN C ES

<!-- side 5 -->

EXCEPTIONS – THE BASIC PRINCIPLES (1)
  Normal flow code does not handle errors – it only checks for errors
  When an error is detected, an exception is thrown
  When an exception is thrown, normal control flow is terminated
  immediately!
  The runtime system searches for an error handler – locally or up the call
  chain (stack) – that is willing to handle (catch) the exception
  When an exception handler is found, the stack is popped to the level of the
  error handler (stack unwinding)
  Control flow resumes in the error handler code
  Any piece of code can declare its willingness to handle an error by using a
  try/catch block

  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

<!-- side 6 -->

EXCEPTIONS – THE BASIC PRINCIPLES (2)
                                  class Program
                                  {
                                    static void Main(string[] args)
                                      {
                                       Console.Write("Course for new ship? ");
                                         var course = Convert.ToUInt32(Console.ReadLine());
                                         Console.WriteLine("");

Try/catch block containing               try
separate ”normal flow” code              {
and exception handling                      var ship = new Ship(course);
                                         Console.WriteLine("Ship is heading course {0}", ship.GetCourse());
                                         }
                                                                                                  class Ship
                                         catch (Exception exception)
                                                                                                  {
                                         {
                                                                                                    …
                                            Console.WriteLine(”Error in course!”);
                                                                                                    public Ship(uint initialCourse)
                                         }
                                                                                                       {
                                    }
                                                                                                       if (initialCourse < 360)
                                  }
                                                                                                            _course = initialCourse;
                                                                                                       else
                                    Exception thrown on error                                            throw new Exception();
      AARHUS                                                      SWT  PETER HØGH MIKKELSEN           }
      UNIVERSITY                                         FEBRUARY 2026 EXCEPTION MANAGEMENT IN C#
      T ECH N IC AL SC IEN C ES                                                                     … }

**Figur:** To overlappende kodebokse: `class Program` (stor, bagerst) og `class Ship` (mindre, forskudt ned til højre foran den første, så den delvist dækker slutningen af catch-blokken). To blå callouts med pile:
- "Try/catch block containing separate 'normal flow' code and exception handling" peger med pil ind på en klammeparentes, der spænder over hele try/catch-blokken i `Main` — altså markeres blokken som ét sammenhængende område.
- "Exception thrown on error" peger med lang pil fra venstre op i `Ship`-boksen på linjen `throw new Exception();`.
Sammenstillingen viser kaste-stedet (nede i `Ship`) og fange-stedet (oppe i `Main`) på samme billede.

<!-- side 7 -->

EXCEPTIONS – THE BASIC PRINCIPLES (3)
  The standard runtime places a try/catch around the user code (i.e.
  before calling main()) that as default handles all exceptions by terminating
  the program

                              try
                              {
                                Main();
                              } catch (Exception e)
                              {
                                // terminate program
                              }




                              Any uncaught exception will cause your program to be forcefully
                              terminated!
  AARHUS                                                        SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                           FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

**Figur:** Til venstre en kodeboks med runtimens implicitte `try { Main(); } catch (Exception e) { // terminate program }`. Fra den udgår en tyk mørkeblå blokpil mod højre til et skærmbillede af en Windows-fejldialog med titlen "ExceptionExample": overskrift "ExceptionExample has stopped working", brødtekst "A problem caused the program to stop working correctly. Windows will close the program and notify you if a solution is available." og to knapper, "Debug" og "Close program". Nederst en blå fremhævet bjælke med advarslen om tvungen programafslutning.

<!-- side 8 -->

EXCEPTIONS – THE BASIC PRINCIPLES (4)
  An exception is a typed object which can contain data, e.g. error
  description.

  In C++, any type (int, float, user-defined classes etc.) can be used as an
  exception

  In C#, all exceptions derive from System.Exception




  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

EXCEPTIONS – THE BASIC PRINCIPLES (5)
class Program
{
  static void Main(string[] args)
    {
    Console.Write("Course for new ship? ");                                                        public class ShipException : Exception
        var course = Convert.ToUInt32(Console.ReadLine());                                         {
        Console.WriteLine("");                                                                       public uint Course { get; private set; }
                                                                                                     public ShipException(uint course)
          try                                                                                        {
          {                                                                                            Course = course;
             var ship = new Ship(course);                                                            }
          Console.WriteLine("Ship is heading course {0}", ship.GetCourse()); }
          }
          catch (ShipException exc)
          {
                                                                                         class Ship
             Console.WriteLine(”Error in course. Is {0},
                                                                                         {
                           should be in [0;360[”, exc.GetCourse());
                                                                                               …
          }
                                                                                               public Ship(uint initialCourse)
    }
                                                                                                   {
}
                                                                                                   if (initialCourse < 360)
                                                                                                         _course = initialCourse;
            Info from exception                                                                    else
            used in error handling                                                                    throw new ShipException(initialCourse);
                                                                                                 }
                                                                                               …
        AARHUS                                                  SWT  PETER HØGH MIKKELSEN
        UNIVERSITY                                     FEBRUARY 2026 EXCEPTION MANAGEMENT} IN C#
        T ECH N IC AL SC IEN C ES

**Figur:** Tre kodebokse arrangeret som et hierarki: `class Program` fylder venstre side; til højre foroven `public class ShipException : Exception`; til højre forneden `class Ship`. En linje forbinder `ShipException`-boksen ned til `Ship`-boksen (Ship kaster netop denne exception-type). Blå callout "Info from exception used in error handling" peger med pil op på `exc.GetCourse()` i catch-blokkens `Console.WriteLine` — altså på selve udlæsningen af data fra exception-objektet. `throw new ShipException(initialCourse);` og `exc.GetCourse()` er fremhævet med fed.

<!-- side 10 -->

EXCEPTIONS – THE BASIC PRINCIPLES (6)
  A class can throw different exceptions. See next slide how to tell them apart

                                                                                    class Ship
                     public class CourseException : Exception                       {
                     {                                                                 public uint Course { get; private set; }
                       public uint Course { get; private set; }                        public uint Speed { get; private set; }
                       public CourseException(uint course)
                       {                                                                    public Ship(uint course, uint speed)
                         Course = course;                                                   {
                       }                                                                      if (speed < 100)
                     }                                                                              Speed = speed;
                                                                                              else
                       public class SpeedException : Exception                                      throw new SpeedException(speed);
                       {
                         public uint Speed { get; private set; }                                if (course < 360)
                         public SpeedException(uint s)                                                Course = course;
                         {                                                                      else throw new CourseException(course);
                           Speed = s;                                                       }
                         }                                                          }
                       }

  AARHUS                                                            SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                               FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

**Figur:** Tre kodebokse: venstre kolonne har `CourseException` øverst og `SpeedException` nederst (begge arver `Exception` og bærer hver sin property); højre side har `class Ship`, hvis konstruktor `Ship(uint course, uint speed)` validerer i to trin — først `speed < 100` ellers `throw new SpeedException(speed)`, derefter `course < 360` ellers `throw new CourseException(course)`. Layoutet parrer visuelt hver exception-klasse til venstre med det tilsvarende `throw` til højre.

<!-- side 11 -->

EXCEPTIONS – THE BASIC PRINCIPLES (7)
  Exception can be caught and handled based on their type. Multiple types
  can be caught.
                      class Program
                      {
                         static void Main(string[] args)
                          {
                            Console.Write("Course for new ship? ");
                               var course = Convert.ToUInt32(Console.ReadLine());
                               Console.WriteLine("");

                                    try
                                    {
                                       var ship = new Ship(course);
                                    Console.WriteLine("Ship is heading course {0}", ship.GetCourse());
                                    }
                                    catch (CourseException exc)
                                    {
                                       Console.WriteLine(”Error in course. Is {0}, should be in [0;360[”, exc.GetCourse());
                                    }
                                 catch (SpeedException exc)
                                    {
                                       Console.WriteLine(”Error in speed. Is {0}, should be in [0;100[”, exc.GetSpeed());
  AARHUS                            }                                     SWT  PETER HØGH MIKKELSEN
  UNIVERSITY                  } }                                FEBRUARY 2026 EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

<!-- side 12 -->

EXCEPTIONS – THE BASIC PRINCIPLES (8)
                                        try
                                        {
                                           File x = new File(”log.txt”); // May throw exception
                                           x.doStuff();              // May throw exception
                                        }
                                        catch (MyFileException e)
                                        {
                                          // Handle app specific file error here
Handlers are tried top-to-bottom,       }
so put specific handlers first and      catch(FileNotFoundException e)
                                        {
general handlers last                     // Handle system exceptions here, e.g. convert them
                                          throw new MyFileException(”…”);
                                        }
                                        catch (Exception e)
                                        {
                                          // Handle other errors here, e.g. just report them
Optional block – used for cleanup         // then rethrow the same exception
that’s always needed,                     throw;
regardless of whether exception         }
                                        finally
was thrown or not                       {
                                          // Optional – used for cleanup that’s always needed
                                          if (x != null) x.Close();
    AARHUS                              } SWT   PETER HØGH MIKKELSEN
    UNIVERSITY                       FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
    T ECH N IC AL SC IEN C ES

**Figur:** Én stor kodeboks til højre med hele try/catch/catch/catch/finally-kæden; kommentarer i grøn kursiv. To blå callouts til venstre med pile ind i koden:
- "Handlers are tried top-to-bottom, so put specific handlers first and general handlers last" har tre pile, én til hver af de tre catch-linjer: `catch (MyFileException e)`, `catch(FileNotFoundException e)` og `catch (Exception e)` — pilene understreger rækkefølgen fra mest specifik til mest generel.
- "Optional block – used for cleanup that's always needed, regardless of whether exception was thrown or not" peger på `finally`-nøgleordet.

<!-- side 13 -->

EXCEPTION FLOW - RECAP
  Exceptions propagate to higher                         Runtime with default exception handler
  levels in the call-stack until it is
  caught
                                                                 Main() with exception handler


                                                             Method1() with exception handler


                                                          Method2() with no exception handler


                                                            Method3() which throws exception

  AARHUS                                          SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                             FEBRUARY 2026    EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

**Figur:** Diagram over exception-propagering i call-stakken. Fem vandrette kasser lodret under hinanden, øverst til nederst:
1. "Runtime with default exception handler" (mørkeblå, hvid tekst)
2. "Main() with exception handler"
3. "Method1() with exception handler"
4. "Method2() with no exception handler"
5. "Method3() which throws exception" (mørkere blå end 2-4)

<!-- side 14 -->

USE EXCEPTIONS THE RIGHT WAY
  Exceptions are a nice way to manage errors

  But exceptions are costly!
   • The process of unwinding the stack looking for a suitable handler is very
     expensive (timewise)
   • Make the try scope as specific as possible and catch exceptions as close
     to the source as possible

  Only use exceptions for error management, and not as a quick and dirty
  way to ”return” in program flow
   • Quick? Not really
   • Dirty? Absolutely!
  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

<!-- side 15 -->

C# SYSTEM EXCEPTIONS
  The standard .Net runtime defines a large set of predefined exceptions such as:
    • System.AccessViolationException
    • System.ArithmeticException
    • System.Collections.Generic.KeyNotFoundException
    • System.IndexOutOfRangeException
    • System.IO.IOException
    • System.NullReferenceException
    • System.ArgumentException

  Consult the documentation for details


  AARHUS                                      SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                         FEBRUARY 2026   EXCEPTION MANAGEMENT IN C#
  T ECH N IC AL SC IEN C ES

<!-- side 16 -->

AARHUS
UNIVERSITY

