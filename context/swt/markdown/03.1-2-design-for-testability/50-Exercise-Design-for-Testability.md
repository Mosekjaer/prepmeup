---
title: "Exercise Design for Testability"
source: "csfiles/home_dir/DesignForTestability/Exercise Design for Testability.pdf"
modul: "Lektion 03.1+2: Design For Testability"
pages: 1
type: "dokument"
vision: "n/a"
---
# Exercise Design for Testability

<!-- side 1 -->

                                                                                              Exercise Design for Testability.docx
                                                                                                                      10-02-2023



Lab exercise – Design for Testability

In this exercise, you will use some of the techniques you have learned to design for testability. Specifically, you will be
acquainted with the identification of dependencies and introduction of interfaces to lower the couplings in your
design. This will train you to design for testability and to refactor existing code for testability.

The scenario is that you are provided with some legacy code for an ECS system, which is not particularly well-
designed. Neither is it documented other than by the occasional source code comments. This is not because your
teacher is lazy – it is a very, very common scenario in the industry, believe me!

Your first job is to investigate the legacy code. Your second job is to do some extensions to the code.

You are required to demonstrate both techniques for dependency injection (constructor injection and property
injection).


Exercise 1: Set up the team’s infrastructure
Have 1 person in the team commit and push the ECS Legacy Solution available from Brightspace to one of the group’s
empty Git repositories (as in the exercise for Lection 01.2).Then, have everybody in the team else clone the repository.
At this point, all team members have a clone of the legacy code.

Exercise 2: Reverse engineering the legacy ECS solution
Reverse-engineer the ECS legacy code: Create a UML class diagram of the existing solution and use this to identify the
flaws in the design when it comes to testability.

Exercise 3: Implement the refactored design
Exercise 3.1:
Create a design that, for each of the flaws you identified before, proposes a solution. Apply the techniques discussed
in class to make the design more testable.

Exercise 3.2
On team member 1’s PC: Implement your refactored design. Commit and push the changes.

Exercise 3.3
On team member 2’s PC: Pull the changes. Then, create unit tests for the refactored version of the class ECS. To do
this, you will need fake “versions” of the dependent classes. Consider where these fakes should be placed in the
solution – in the application project or the test project?

Exercise 3.4
Set up a Pipeline on GitLab for your solution and ensure it runs your unit test on your project.


Exercise 4: Extend the ECS
You are now going to extend the ECS with two more features. As you create your design, be sure to design your
changes for testability, implement your changes according to the design, test your changes and push them to the git
repository so that all team members – and GitLab – are in sync. Also, be sure to identify and handle any exceptions
that may occur by the design extensions.

Extension 1: Add a window to the ECS. The window should open when the temperature rises above a certain upper
temperature threshold and close if the temperature is below this threshold.

Extension 2: Make the thresholds configurable so that they may be changed at run-time.




                                                         Page 1 of 1

