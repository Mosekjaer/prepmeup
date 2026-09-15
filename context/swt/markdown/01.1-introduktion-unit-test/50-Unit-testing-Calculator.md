---
title: "Unit-testing Calculator"
source: "csfiles/home_dir/UnitTests/Unit-testing Calculator.pdf"
modul: "Lektion 01.1: Introduktion + Unit Test"
pages: 2
type: "dokument"
vision: "n/a"
---
# Unit-testing Calculator

<!-- side 1 -->

                                                                                                   Unit-testing Calculator.docx
                                                                                                                    2023.08.22



Lab Exercise: Unit-testing class Calculator

In this exercise, you will use your existing implementation of class Calculator to create your first NUnit test suite of
an application class. You will organize your application code and test code in separate projects and “link” them
together.

It is a prerequisite for this exercise that you have downloaded and installed ReSharper. Again, check Brightspace for
instructions


In Lab Exercise 1, you created class Calculator and tested it as well as you could using hand-testing. In this exercise
you will use NUnit to test the class again.

Exercise 1: Get ready!
    • Open the solution containing your Calculator .Net project
    • Add a test project to the solution using the “NUnit Test Project” template (not any of the templates with
         “(.Net Framework)” in their name – they are obsolete!) Select the same .Net version you used for the original
         Calculator project. Name this project Calculator.Test.Unit.
    • – alternatively add a C# class library project using the “Class Library” template (not any of the templates with
         “(.Net Framework)” in their name – they are obsolete!). Select the same .Net version you used for the
         original Calculator project. Name this project Calculator.Test.Unit.
         To this you need to add at least the following Nuget packages:
              a. Microsoft.Net.Test.Sdk
              b. NUnit
              c. NUnit3TestAdapter
    • For either of these possibilities, add a reference such that the Calculator.Test.Unit project refers to the
         Calculator project (and not the other way around).

At this time, you should have 1 C# solution containing 2 C# projects, one named Calculator (this is the original
project) and one named Calculator.Test.Unit (this is the test project).

Exercise 2: Define your test fixture
Add a new C# source file to the test project (that’s Calculator.Test.Unit). In this file, define the class
CalculatorUnitTests.

You can also just use the "Add Class" menu command to the project.

This class is your test fixture and will hold all your unit tests for class Calculator.

Exercise 3: Implement your tests
Implement your unit tests in the file you added to the test project above – test the class Calculator as thoroughly as
you can using NUnit tests. Is it difficult?


Exercise 4: Compare and contrast
Compare your tests in the two lab exercises and reflect on hand-testing vs. unit testing with a framework:

Extensibility    Which form of test is easiest to extend, e.g. if new functionality is required for class Calculator?

Maintainability Which form of test is easiest to maintain?

Readability      Imagine you are new to the project. Which form of test is easiest to read?

Automation       Which type of test is easiest to automate? If you wanted to collect and compare test results every 15
                 minutes, which kind of test is it easiest to see if passed or failed?



                             Continued on next page.

                                                          Page 1 of 2

<!-- side 2 -->

                                                                                                    Unit-testing Calculator.docx
                                                                                                                     2023.08.22




Exercise 5 (optional): Investigate the [TestCase] attribute
NUnit offers a number of facilities for the definition of repetitious test cases. For example, you may have perhaps 9 or
more individual tests of the method Power(), one for each combination of positive, negative, and 0 value of the two
arguments. Each of these methods are tagged with the property [Test].

While this works just fine, it gets kind of tedious to duplicate the test code six times, only varying the arguments.
Instead, you can use the [TestCase] attribute to specify arguments for the individual tests. Investigate this and
refactor your test suite to use [TestCase] instead of [Test].




                                                         Page 2 of 2

