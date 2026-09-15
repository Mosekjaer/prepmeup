---
title: "Exceptions Intro"
source: "csfiles/home_dir/Exceptions/Exceptions Intro.pptx"
modul: "C# materiale"
pages: 15
type: "slides"
vision: "done"
note: "pptx konverteret via LibreOffice"
---

# Exceptions Intro

<!-- side 1 -->

A crash course in exceptions

1: Introduction to exceptions in C#

            Software Test

<!-- side 2 -->

Error management in general
• Most production code has 2 sets of paths
   – Happy paths:      Things go as planned
   – Error paths:      We detect and handle errors


• Solid, robust code needs to detect errors and handle
  them

• Error detection/handling code in the “normal flow
  code” obscures normal flow and makes code messy


                         -2-

<!-- side 3 -->

Error management in general
  • Often, we cannot even handle the error where we
    detect it.
class Ship                                     class MemoryMgr
{                                              {
  private uint _course;                          public MemoryBlock GetBlock(uint size)
  public void Ship(uint initialCourse)           {
  {                                                if(canAllocate(size)) return findBlock();
    if(c > 360) // Error – but what to do?         else // Error – but what to return?
    else _course = initialCourse;                }
  }
}                                                  public bool canAllocate(uint size) {…}
                                               }




  • It’s more logical to handle an error further up the call
    chain from were it was detected
                                         -3-

**Figur:** To kodebokse side om side illustrerer problemet.

Venstre boks:

```csharp
class Ship
{
  private uint _course;
  public void Ship(uint initialCourse)
  {
    if(c > 360) // Error – but what to do?
    else _course = initialCourse;
  }
}
```

Højre boks:

```csharp
class MemoryMgr
{
  public MemoryBlock GetBlock(uint size)
  {
    if(canAllocate(size)) return findBlock();
    else // Error – but what to return?
  }

  public bool canAllocate(uint size) {…}
}
```

Pointen ligger i de to grønne kommentarer: konstruktøren kan ikke returnere noget, og `GetBlock` har ingen fornuftig returværdi at signalere fejl med.

<!-- side 4 -->

Our wishes for error
              management
• Thus, we have some wishes for our error detection
  and handling scheme: We want to be able to…
   – separate ”normal flow” and ”error management” code
   – separate error detection and error handling
   – detect errors in one place and handle them ”further up”
     the control chain


• …And this is exactly what exceptions are about (and
  some added niceties)


                            -4-

<!-- side 5 -->

Exceptions – the basic principles
                    (1)
• Normal flow code does not handle errors – it only checks for
  errors
• When an error is detected, an exception is thrown
• When an exception is thrown, normal control flow is terminated
  immediately!
• The runtime system searches for an error handler – locally or up
  the call chain (stack) – that is willing to handle (catch) the
  exception
• When an exception handler is found, the stack is popped to the
  level of the error handler (stack unwinding)
• Control flow resumes in the error handler code
• Any piece of code can declare its willingness to handle an error by
  using a try/catch block         -5-

<!-- side 6 -->

Exceptions – the basic principles
                      (2)
                           class Program
                           {
                              static void Main(string[] args)
                               {
                                  Console.Write("Course for new ship? ");
Try/catch block containing           var course = Convert.ToUInt32(Console.ReadLine());
separate ”normal flow” code          Console.WriteLine("");
and exception handling                try
                                      {
                                         var ship = new Ship(course);
                                      Console.WriteLine("Ship is heading course {0}", ship.GetCourse());
                                      }
                                      catch (Exception exception)
                                      {
                                         Console.WriteLine(”Error in course!”);
                                      }
                                 }
                             }                                     class Ship
                                                                   {
                                                                      …
                                                                      public Ship(uint initialCourse)
                                                                      {
                                                                          if (initialCourse < 360)
                                                                                 _course = initialCourse;
 Exception thrown on error                                                else
                                                                              throw new Exception();
                                                                       }
                                                                      …
                                                                   }


                                               -6-

**Figur:** To kodebokse forbundet med callout-pile.

Hovedboksen (øverst, `class Program`):

```csharp
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
        catch (Exception exception)
        {
            Console.WriteLine("Error in course!");
        }
    }
}
```

En grå/blå callout til venstre med teksten "Try/catch block containing separate ”normal flow” code and exception handling" har en pil, der peger på en klamme, som omkranser hele try/catch-blokken (fra `try` til den afsluttende `}` efter catch).

Overlappende boks nederst til højre (`class Ship`):

```csharp
class Ship
{
    …
    public Ship(uint initialCourse)
    {
        if (initialCourse < 360)
            _course = initialCourse;
        else
            throw new Exception();
    }
    …
}
```

En callout nederst til venstre med teksten "Exception thrown on error" har en lang pil, der peger præcist på `throw new Exception();`.

<!-- side 7 -->

Exceptions – the basic principles
                  (3)
• The standard runtime places a try/catch around the user
  code (i.e. before calling main()) that as default handles all
  exceptions by terminating the program

   try
   {
      Main();
   } catch (Exception e)
   {
      // terminate program
   }




                  Any uncaught exception will cause your
                  program to be forcefully terminated!

                                   -7-

**Figur:** Til venstre en kodeboks:

```csharp
try
{
    Main();
} catch (Exception e)
{
    // terminate program
}
```

En tyk blå højrepil fra kodeboksen peger på et skærmbillede af en Windows-fejldialog med titlen "ExceptionExample": overskrift "ExceptionExample has stopped working", brødtekst "A problem caused the program to stop working correctly. Windows will close the program and notify you if a solution is available." og to knapper, "Debug" og "Close program".

Nederst en rosa/rød fremhævet boks: "*Any* uncaught exception will cause your program to be forcefully terminated!"

<!-- side 8 -->

Exceptions – the basic principles
                 (4)
• An exception is a typed object which can contain
  data, e.g. error description.

• In C++, any type (int, float, user-defined classes etc.)
  can be used as an exception

• In C#, all exceptions derive from System.Exception




                          -8-

<!-- side 9 -->

Exceptions – the basic principles
                     (5)
                         class Program
                         {
                           static void Main(string[] args)
                             {
                             Console.Write("Course for new ship? ");
Info from exception               var course = Convert.ToUInt32(Console.ReadLine());
used in error handling            Console.WriteLine("");

                                 try
                                 {
                                   var ship = new Ship(course);
                                 Console.WriteLine("Ship is heading course {0}", ship.GetCourse());
                                 }
                                 catch (ShipException exc)
                                 {
                                   Console.WriteLine(”Error in course. Is {0}, should be in
                        [0;360[”,
                                      exc.GetCourse());
                                 }                          class Ship
                          }                                 {
                        }                                       …
                                                                public Ship(uint initialCourse)
                                                                {
public class ShipException : Exception
                                                                   if (initialCourse < 360)
{
                                                                          _course = initialCourse;
  public uint Course { get; private set; }
                                                                   else
  public ShipException(uint course)
                                                                       throw new ShipException(initialCourse);
  {
                                                                 }
    Course = course;
                                                                …
  }
                                                            }
}

                                                 -9-

**Figur:** Tre kodebokse med en callout-pil.

Hovedboksen (`class Program`):

```csharp
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
        catch (ShipException exc)
        {
            Console.WriteLine("Error in course. Is {0}, should be in [0;360[",
                exc.GetCourse());
        }
    }
}
```

Callout øverst til venstre, "Info from exception used in error handling", med pil ned mod `exc.GetCourse()` i catch-blokkens WriteLine.

Boks nederst til venstre:

```csharp
public class ShipException : Exception
{
  public uint Course { get; private set; }
  public ShipException(uint course)
  {
    Course = course;
  }
}
```

Boks nederst til højre:

```csharp
class Ship
{
    …
    public Ship(uint initialCourse)
    {
        if (initialCourse < 360)
            _course = initialCourse;
        else
            throw new ShipException(initialCourse);
    }
    …
}
```

<!-- side 10 -->

Exceptions – the basic principles
                 (6)
• A class can throw different exceptions. See next slide
  how to tell them apart

public class CourseException : Exception            class Ship
{                                                   {
  public uint Course { get; private set; }             public uint Course { get; private set; }
  public CourseException(uint course)                  public uint Speed { get; private set; }
  {
    Course = course;                                    public Ship(uint course, uint speed)
  }                                                     {
}                                                          if (speed < 100)
                                                                  Speed = speed;
                                                           else
                                                                  throw new SpeedException(speed);
public class SpeedException : Exception
{                                                           if (course < 360)
  public uint Speed { get; private set; }                          Course = course;
  public SpeedException(uint s)                             else throw new CourseException(course);
  {                                                     }
    Speed = s;                                      }
  }
}



                                             -10-

**Figur:** Tre kodebokse: to exception-klasser i venstre kolonne og `Ship` i højre.

Øverst til venstre:

```csharp
public class CourseException : Exception
{
  public uint Course { get; private set; }
  public CourseException(uint course)
  {
    Course = course;
  }
}
```

Nederst til venstre:

```csharp
public class SpeedException : Exception
{
  public uint Speed { get; private set; }
  public SpeedException(uint s)
  {
    Speed = s;
  }
}
```

Højre boks:

```csharp
class Ship
{
    public uint Course { get; private set; }
    public uint Speed { get; private set; }

    public Ship(uint course, uint speed)
    {
        if (speed < 100)
            Speed = speed;
        else
            throw new SpeedException(speed);

        if (course < 360)
            Course = course;
        else throw new CourseException(course);
    }
}
```

Bemærk at `Ship` deklarerer begge properties som auto-properties med `private set`, og at speed-checket kommer før course-checket i konstruktøren.

<!-- side 11 -->

Exceptions – the basic principles
                (7)
• Exception can be caught and handled based on their
  type. Multiple types can be caught.
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
             }
      }
  }

                                            -11-

**Figur:** Én stor kodeboks:

```csharp
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
            Console.WriteLine("Error in course. Is {0}, should be in [0;360[", exc.GetCourse());
        }
        catch (SpeedException exc)
        {
            Console.WriteLine("Error in speed. Is {0}, should be in [0;100[", exc.GetSpeed());
        }
    }
}
```

To `catch`-blokke efter hinanden på samme `try`, én per exception-type.

<!-- side 12 -->

Exceptions – the basic principles
                            (8)
                                     try
                                     {
                                        File x = new File(”log.txt”); // May throw
                                     exception
                                        x.doStuff(); // May throw exception
                                     }
                                     catch (MyFileException e)
                                     {
                                       // Handle app specific file error here
                                     }
                                     catch(FileNotFoundException e)
Handlers are tried top-to-bottom,    {
so put specific handlers first and     // Handle system exceptions here, e.g. convert
general handlers last                them
                                       throw new MyFileException(”…”);
                                     }
                                     catch (Exception e)
                                     {
                                       // Handle other errors here, e.g. just report
                                     them
Optional block – used for cleanup      // then rethrow the same exception
that’s always needed,                  throw;
regardless of whether exception      }
was thrown or not                    finally
                                     {         -12-

**Figur:** Én kodeboks med to callouts til venstre.

```csharp
try
{
    File x = new File("log.txt"); // May throw exception
    x.doStuff();  // May throw exception
}
catch (MyFileException e)
{
  // Handle app specific file error here
}
catch(FileNotFoundException e)
{
  // Handle system exceptions here, e.g. convert them
  throw new MyFileException("…");
}
catch (Exception e)
{
  // Handle other errors here, e.g. just report them
  // then rethrow the same exception
  throw;
}
finally
{
  // Optional cleanup code, always executed
}
```

Den øverste callout, "Handlers are tried top-to-bottom, so put specific handlers first and general handlers last", har tre pile, der peger på hver af de tre `catch`-linjer i rækkefølge — `MyFileException`, `FileNotFoundException`, `Exception` — altså fra mest specifik til mest generel.

Den nederste callout, "Optional block – used for cleanup that's always needed, regardless of whether exception was thrown or not", peger på `finally`-blokken. Slidet er beskåret i bunden, så indholdet af finally-blokken er kun delvist synligt.

<!-- side 13 -->

Exception flow - recap

   Runtime with default exception handler


       Main() with exception handler


     Method1() with exception handler


    Method2() with no exception handler


     Method3() which throws exception

               -13-

**Figur:** Lodret stak af fem kasser, der repræsenterer kaldkæden. Øverst en mørkeblå kasse med hvid tekst, de tre næste er lyseblå, og den nederste er rosa/rød:

1. Runtime with default exception handler (mørkeblå)
2. Main() with exception handler
3. Method1() with exception handler
4. Method2() with no exception handler
5. Method3() which throws exception (rosa)

Mellem hvert par af nabokasser går en tyk grøn pil nedad — det normale kaldflow, fra runtime ned til Method3().

Langs højre side går det røde exception-flow opad: fra Method3() går en rød pil ud til højre og op til Method2(); da Method2() ikke har nogen handler, fortsætter en heltrukken rød pil videre op til Method1(). Derfra fortsætter en stiplet rød pil op forbi Main() og videre op til Runtime-kassen. Heltrukken rød = den faktiske unwinding indtil første handler; stiplet rød = hvor exceptionen ville fortsætte, hvis handleren ikke tog den.

<!-- side 14 -->

Use exceptions the right way
• Exceptions are a nice way to manage errors

• But exceptions are costly!
   – The process of unwinding the stack looking for a suitable
     handler is very expensive (timewise)

• So only use exceptions for error management, and
  not as a quick and dirty way to ”return” in program
  flow
   – Quick? Not really
   – Dirty? Absolutely!

                            -14-

<!-- side 15 -->

C# system exceptions
• The standard .Net runtime defines a large set of
  predefined exceptions such as:
   –   System.AccessViolationException
   –   System.ArithmeticException
   –   System.Collections.Generic.KeyNotFoundException
   –   System.IndexOutOfRangeException
   –   System.IO.IOException
   –   System.NullReferenceException
   –   System.ArgumentException



• Consult the documentation for details


                               -15-

