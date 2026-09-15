---
title: "SQM CI - Exercise CC"
source: "csfiles/home_dir/SoftwareQualityMetrics/SQM CI - Exercise CC.pdf"
modul: "Lektion 08.1+2: Software Quality Metrics"
pages: 2
type: "dokument"
vision: "n/a"
---
# SQM CI - Exercise CC

<!-- side 1 -->

                                                                        SQM CI - Exercise CC.docx
                                                                                      16-10-2024




Lab Exercise: Study Software Metrics and Analysis.

In this exercise you will study the Software Metrics and Static Analysis that Visual
Studio, Resharper and ReportGenerator provide and try to reduce complexity.




Study existing Cyclomatic Complexity etc.

Open one of the suggested C# projects in Visual Studio / VS Code / Rider. Not all
metrics are available in all IDEs.
A. Visual Studio Code Metrics

     • Select the Analyze -> Calculate Code Metrics menu function, and select a
       project or the entire solution. A Code Metrics Results should appear, when
       they have been calculated.

     • Study the results for you code. Search for the "hot spots" with high
       complexity or low maintenance index, by going into further details for each
       method in the Code Metrics Results. Discuss within the group, why these
       metrics are what they are, and if and what should be done about it.
B. Resharper Code Analysis

     • Select the Resharper -> Inspect -> Code Issues in Current Project. An
       Inspection Result window should open. Study the type of warnings, Resharper
       is generating. Resharper's Inspection rules can be configured under Resharper
       -> Options -> Code Analysis.


C.   Microsoft Code Analysis in Visual Studio.

     • Select the Analyze -> Configure Code Analysis menu function for your code
       project (or from Project Properties, scroll down to Code Analysis). Make sure
       the “Run on Build” and “Enable .NET analyzers” check marks are set. Set the
       Analysis level dropdown box to a setting that contains the word "all". Do a
       Rebuild Solution command. In the Error List window, the results should
       appear. Study and discuss the different types of Warnings the code analysis
       tool is issuing. Visual Studio and Intellisense generate some of the same
       Warnings. Code Analysis warnings codes start with "CA".


                                             Page 1 of 2

<!-- side 2 -->

                                                                      SQM CI - Exercise CC.docx
                                                                                    16-10-2024



D. ReportGenerator and cobertura.

   • In https://gitlab.au.dk/au-ece-swt/swt-student/-
     /tree/main/dotnet/projects?ref_type=heads you can find an example of a
     project file that contains a target to generate a coverage report and metrics
     like the ones generated on the Gitlab CI server. Check the readme file on how
     to invoke this project file target. The DoorControl project given on Brightspace
     already contains this target.
   • Generate the report and investigate the cyclomatic complexity of the
     different classes. Discuss how the numbers correlate with the program flow in
     the code.


Update your design to reduce the cyclomatic complexity

As we saw in the presentation, a method to reduce complexity is to transform
conditionals onto polymorphism. In DoorControl, you will probably find the highest
complexity in the management of state transitions. The GoF State Pattern aims to
replace a nested design into a design that uses polymorphism, so in theory, this
should be a way to reduce the complexity.
Refactor DoorControl (or a similar switch-case-based state machine) to use State
Pattern and investigate the cyclomatic complexity before and after. Also discuss
how this change affects testing? Does it make testing of state transitions harder og
easier?




                                       Page 2 of 2

