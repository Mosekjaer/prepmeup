---
title: "Coverage - Exercise"
source: "csfiles/home_dir/TestQuality/Coverage/Coverage - Exercise.pdf"
modul: "Lektion 05.1+2: Test Quality 1 og 2"
pages: 2
type: "dokument"
vision: "n/a"
---
# Coverage - Exercise

<!-- side 1 -->

                                                                                                   Coverage - Exercise.docx
                                                                                                                27-09-2024




1     Lab Exercise: Using code coverage with your project

In this exercise you will get code coverage tools up and running for your project, inspect and improve coverage for
your project.

1.1    Local code coverage

Use one of you prior projects, Calculator, ECS or DoorControl and inspect the code coverage for the tests you have
performed so far. There are different coverage tools dependent on your IDE.

Visual Studio + Resharper
    • Select Cover from either:
    • click on the "bubble" and select Cover All
    • right click on the test project or the solution in Solution Explorer and select "Cover Unit Tests" very far down
         in the menu
    • Select the Unit Test submenu in the Resharper menu and choose one of the "Cover…" commands
    • Make Resharper's Unit Test Coverage window visible by selecting it from the Windows submenu in the
         Resharper Menu, if you cannot find the results

Visual Studio + Coverlet + Fine Code Coverage
    • Install the Fine Code Coverage (FCC) Visual Studio Extension from Extensions/Manage Extensions. Restart
         Visual Studio
    • Make sure that the NuGet package coverlet.collect is installed in the test project
    • Use the built in Test/Run All Test command – this will use the Microsoft test runner, and collect coverage
         automatically
    • Wait until tests have been run and coverage has been generated
    • Make FCC's result window visible with the View/Other Windows/Fine Code Coverage command if you cannot
         find the results
    • coverlet and FCC gives a fairly good branch coverage, without being perfect (it can make mistakes)

JetBrains Rider + dotCover
    • Dot Cover is installed per default in Rider, but not always enabled
    • Enable dotCover
    • In the test menu, press the drop-down menu for the testing button and choose “Cover All”
    • Results will be shown in a separate window

Visual Studio Code + Coverlet + JunitXml.TestLogger + ReportGenerator
    • Make sure that the following nuget packages are installed in the testproject:
             o NUnit
             o NUnit3TestAdapter
             o JunitXml.TestLogger
             o Microsoft.NET.Test.Sdk
             o (NSubstitute, if used)
             o coverlet.collector
             o ReportGenerator
    • Run tests using VS Codes Test Runner or CLI with dotnet test and check this is successful
    • Collect coverage with CLI: dotnet test --collect:"XPlat Code Coverage" --
         logger:"junit;MethodFormat=Class;FailureBodyFormat=Verbose
    • Build a report with: dotnet
         ~/.nuget/packages/reportgenerator/*/tools/net8.0/ReportGenerator.dll -
         reports:$(pwd)/*/*/*/coverage.cobertura.xml -targetdir:coveragereport "-
         reporttypes:Html;TextSummary"
    • Open the index.html file in the coveragereport subfolder of your testproject and view the coverage. Clicking
         on a class and function, you can dig into sections covered or not.

When you can check your coverage locally, you can continue to have coverage added to your CI script.


                                                       Page 1 of 2

<!-- side 2 -->

                                                                                                         Coverage - Exercise.docx
                                                                                                                      27-09-2024



1.2       Code coverage on CI server

Code coverage estimation can be performed on the CI server upon push, daily actions or other actions configured on
the server. We’ll do it for every push for now. You will have to update your .gitlab-ci.yml script to add coverage
analysis and a such has been prepared for you on the course’s Gitlab. Pick the GitLabCIWithCoverage.gitlab-ci.yml and
put it in the root of your project (where your .sln is located). Remember to rename it to .gitlab-ci.yml.
The new pipeline file will build, run the tests, and when running the test, it will observe the tested code and will
observe whether it has been run – i.e. covered – and finally calculate a coverage percentage and generate a report
with line-by-line coverage display. The report will be stored as a job artifact in Gitlab and can be downloaded as a ZIP
file. It must be unzipped and then opened with a browser to see it.


Before going through the following steps, it is assumed that the solution is already connected to a GitLab
repository.

      •    Make sure that the following nuget packages are installed in the testproject:
              o NUnit
              o NUnit3TestAdapter
              o JunitXml.TestLogger
              o Microsoft.NET.Test.Sdk
              o (NSubstitute, if used)
              o coverlet.collector
              o ReportGenerator

      •    They may already be installed by the test project template, but make sure they are there, nevertheless. If
           they are not, make sure it is so before the next steps. The next step should be taken by the same person.

      •    If you have not done so already, add the new pipeline file to the same folder as the solution file (.sln) like in
           the previous exercises. Commit and push.

      •    The new pipeline should now run on GitLab. Correct problems until it does.

      •    When the pipeline is finished, then under the CI/CD-Jobs page, a coverage percentage should be shown in its
           own column. A full report can be downloaded: When downloading from the Jobs page, it should immediately
           make an "artifacts.zip" file available for download. When downloading from the Pipelines page, select the
           "archive" download from the drop down list. On the Analytics/Repository page there is a time graph for the
           coverage percentage, but it may take several runs before it is updated.

      •    This ZIP file should be unzipped to a new folder. Go to the index.html in one of the folders below and open it
           with a browser. It should now be possible to see the report. Click on a class to see the code. Green lines have
           been covered, other colors means it is not covered by your tests, or only partially, if there is a branch on the
           line.

      •    Everybody else in the team make a pull.

      •    Now, one from your team shall create a change in the project. Build and Test. Commit the change to the local
           repository. Finally do a sync: Pull – Push to your remote GitLab repository.

      •    Goto the GitLab server. After a few seconds (10-15), a new run of the pipeline should start automatically.

      •    The new job should run the unit tests again, and at the same time update the coverage report.


1.3       Update your tests

Inspect the report and locate uncovered areas. Some uncontrollable boundaries may be hard to obtain full coverage
for, make informed decisions on what to include and what not. Strive for 100% coverage.


                                                           Page 2 of 2

