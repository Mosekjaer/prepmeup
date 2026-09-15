---
title: "Exceptions and Test"
source: "csfiles/home_dir/Exceptions/Exceptions and Test.pptx"
modul: "C# materiale"
pages: 7
type: "slides"
vision: "done"
note: "pptx konverteret via LibreOffice"
---

# Exceptions and Test

<!-- side 1 -->

A crash course in exceptions

   2: Exceptions and test

         Software Test

<!-- side 2 -->

Exceptions and test
• Exceptions must also be tested

• Exceptions are part of the ”contract” for using a class
   – If you do so-and-so, this particular exception will be
     thrown


• Create test cases that verify that exceptions occur
  when they should



                             -2-

<!-- side 3 -->

Exceptions and test
                                               Combined Act/Assert – it is
//Testing using oldfashioned Assert.Throws<>
[Test]                                       necessary to Assert directly on
public void TestThatThrowsException2()                  the Act!
{
   uut = new UUT();
   uut.SetUpScenario();
   Assert.Throws<MyException>( () => uut.StuffThatThrowsException());
}




                               Function/method with zero
                               parameters, defined with a
                                   lambda expression




                                    -3-

**Figur:** Kodeboksen med testen har to blå callout-bobler med hvid tekst, der peger ind i koden.

Den øverste boble, "Combined Act/Assert – it is necessary to Assert directly on the Act!", peger med sin spids ned mod linjen `Assert.Throws<MyException>( () => uut.StuffThatThrowsException());` — nærmere bestemt mod selve `Assert.Throws`-kaldet. Boblen dækker samtidig højre del af kommentarlinjen `//Testing using oldfashioned Assert.Throws<>`.

Den nederste boble, "Function/method with zero parameters, defined with a lambda expression", peger op mod lambdaen `() => uut.StuffThatThrowsException()` inde i Assert.Throws-kaldet.

<!-- side 4 -->

Exceptions and test
// Testing using constraints
[Test]
public void TestThatThrowsException3()
{
   uut = new UUT();
   uut.SetUpScenario();
   Assert.That(() => uut.StuffThatThrowsException(),
Throws.TypeOf<MyException>());
}




                                   -4-

<!-- side 5 -->

Exceptions and test
// Testing for values in the exception using constraints
[Test]
public void TestThatThrowsException3()
{
   uut = new UUT();
   uut.SetUpScenario();
   Assert.That(() => uut.StuffThatThrowsException(),

Throws.TypeOf<MyException>().With.Property("Value").EqualTo(42));
}




                                   -5-

<!-- side 6 -->

Exceptions and test
// Testing on expected values in the exception
// Using oldfashioned Assert.Catch is more readable than the constraint
version
[Test]
public void TestThatThrowsException4()
{
   uut = new UUT();
   uut.SetUpScenario();
   var ex =
      Assert.Catch<ExceptionWithValue>( () =>
uut.StuffThatThrowsException());

    Assert.That(ex.Value, Is.EqualTo(42));
}




                                    -6-

<!-- side 7 -->

Exceptions and test
// Old fashioned, where is the failure???



                             !
[Test]




                         T E
[ExpectedException(typeof(MyException))]




                     L E
public void TestThatThrowsException1()




                  S O
{




                 B
   uut = new UUT();



                O
   uut.SetUpScenario();
   uut.StuffThatThrowsException()
}




This attribute for testing Exceptions in NUnit is no longer available for the reason
indicated in the comment. If there is more than one line of code in the test, it will
not be clear, if the exception was actually thrown from the expected line and not
from one of the other lines, when the test passes.




                                        -7-

**Figur:** Kodeboksen er overstemplet med et stort rødt, roteret vandmærke: **OBSOLETE !** skrevet diagonalt hen over koden fra nederst til venstre mod øverst til højre. Stemplet dækker delvist linjerne `uut.SetUpScenario()` og `uut.StuffThatThrowsException()`, som stadig kan læses igennem.

Attributten der markeres som forældet, er `[ExpectedException(typeof(MyException))]` placeret under `[Test]`.

