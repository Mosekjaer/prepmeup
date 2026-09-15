---
title: "Coverage"
source: "Coverage-latest.pdf"
modul: "Lektion 05.1+2: Test Quality 1 og 2"
pages: 18
type: "slides"
vision: "done"
---
# Coverage

<!-- side 1 -->

TEST COVERAGE


AARHUS                                SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  SEPTEMBER 2025   TEST COVERAGE
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

TEST COVERAGE


 We want a measure of how “good” our test is.
        • One measure: How much application code is “covered” by tests?


 Test coverage measure quality of test (not of the actual product)




 AARHUS                                       SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                         SEPTEMBER 2025   TEST COVERAGE
 T ECH N IC AL SC IEN C ES

<!-- side 3 -->

TEST COVERAGE
 The process of determining which areas of a program that are
 exercised by given set of test cases.

 Using that knowledge to systematically expand test cases to test
 “untouched” parts of the program.

 Having a quantitative measure of code coverage

 Many different kinds of coverage exist


 AARHUS                                SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  SEPTEMBER 2025   TEST COVERAGE
 T ECH N IC AL SC IEN C ES

<!-- side 4 -->

LINE COVERAGE (AKA. STATEMENT COVERAGE)

  Most frequently used - not necessarily best, but easiest
                              Source lines reached by test
         • 100 ∗                                             %
                                  Source lines in code



  Disadvantages
         • White-box test
                                                                                                         Did we test for zero times?
         • Insensitive to control structures:                                                                 Once? Several?


                                 int* p = 0;                        int array[MAX];
                                 if (condition)                     for(int i=0; i<MAX; i++)
                                     p = &variable;                 {
                                 *p = 117;                              array[i] = i-1;      // ar[i] = i-1
                                                                    }
                                   Did we test for condition
                                        being false?
  AARHUS                                                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                     SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

**Figur:** To kodebokse vist side om side som billede (den udtrukne tekst har flettet dem sammen kolonnevis). Venstre boks:
```c
int* p = 0;
if (condition)
    p = &variable;
*p = 117;
```
Mørk callout-boks under den: Did we test for condition being false? — pointen er, at én test med condition = true giver 100 % line coverage, mens false-grenen giver nulpointer-dereference.

<!-- side 5 -->

BRANCH COVERAGE (AKA DECISION COVERAGE)

  Measures to which extent all decision points (if, while, switch etc.)
  have been fully exercised

  Also includes “asynchronous branches” such as exception and
  interrupt handlers

  Fairly simple to calculate, but without the problems of statement
  coverage.



  AARHUS                                 SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                   SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 6 -->

BRANCH COVERAGE (AKA DECISION COVERAGE)

  Branch coverage also has its deficiencies

                              if (amount > 100 || someCode() == 0) // Was someCode() called?
                                statement1;
                              else
                                statement2;




  In languages with short circuit boolean operators (C, C#, C++,
  Java, …) we have problems when trying to exercise all code



  AARHUS                                                         SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                           SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 7 -->

THE 238776 OTHER MEASURES
  Attempts to strengthen decision coverage
         • condition coverage (all conditions tested as false/true)
         • multiple condition coverage (all combinations of all conditions –
           exponential increases…)
         • condition/decision coverage
         • modified condition/decision coverage

  Other measures
         • relational operator coverage (<=, >=,<,>)
         • Function, call, loop coverage
         • and so on … and so forth…

  AARHUS                                          SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                            SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 8 -->

PRACTICAL EXPERIENCES
  Test coverage works best for new projects where it can be
  applied from the start

  Make the goal 100% coverage, nothing less.
         • If coverage is 95%, you really know nothing of the remaining 5%
         • If 95% is ok, then 90% is also fine … so is 85% … naahhh – let’s make it 80%

  If something cannot be covered, make an informed decision to
  exclude it from coverage
         • Keep the goal at 100%

  AARHUS                                           SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                             SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

UPS OF CODE COVERAGE



 Simple, objective (repeatable) measure for quality of tests

 Helps to decide where to spend test effort (adding test cases etc.)

 Helps to detect trends (increasing or decreasing coverage in
 specific parts of the code) and hence take appropriate action

  AARHUS                                SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 10 -->

DOWNS OF CODE COVERAGE



 100% coverage is no guarantee of zero defects

 Very tool dependent (expensive, non-portable)

 Test coverage says nothing of omission errors

 Test coverage observations takes time
 AARHUS                                SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  SEPTEMBER 2025   TEST COVERAGE
 T ECH N IC AL SC IEN C ES

<!-- side 11 -->

  COVERAGE WITH VISUAL STUDIO 2026




Collect
                                                                             Select the class and method
coverage
                                                                             to have the code coverage
                                                                             highlighted

Results per
method




         AARHUS                                SWT    PETER HØGH MIKKELSEN
         UNIVERSITY                  SEPTEMBER 2025   TEST COVERAGE
         T ECH N IC AL SC IEN C ES

**Figur:** Skærmbillede af Visual Studio 2026 (mørkt tema) med tre røde annoteringspile ind i UI'et.

<!-- side 12 -->

COVERAGE WITH COVERLET AND FINE
CODE COVERAGE
Install the Fine Code Coverage (FCC) Visual Studio Extension from
Extensions/Manage Extensions. Restart Visual Studio
Make sure that the NuGet package coverlet.collect is installed in the test project
Use the built in Test/Run All Test command – this will use the Microsoft test runner, and
collect coverage automatically
Wait until tests have been run and coverage has been generated
Make FCC's result window visible with the View/Other Windows/Fine Code Coverage
command if you cannot find the results
coverlet and FCC gives a fairly good branch coverage, without being perfect (it can
make mistakes)

   AARHUS                                        SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                          SEPTEMBER 2025   TEST COVERAGE
   T ECH N IC AL SC IEN C ES

<!-- side 13 -->

CODE COVERAGE WITH A CI INSTALLATION
  Make sure that the NuGet packages ReportGenerator and coverlet.collect
  are installed in the test project
  Use the .gitlab-ci.yml script given in this lecture as your CI script, commit and
  push. It should run a combined line and branch coverage (like Fine Code
  Coverage)
  You see the line coverage in the Jobs list under CI/CD
  You can see the timeline for your coverage under Analytics/Repository
  To see the line-by-line coverage, you must download the archived files as a
  ZIP file, unzip it and open index.html.




  AARHUS                                      SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                        SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 14 -->

# See https://docs.gitlab.com/ee/ci/testing/unit_test_report_examples.html
# Complete reference here: https://docs.gitlab.com/ee/ci/yaml/index.html
image: mcr.microsoft.com/dotnet/sdk:10.0
build-and-test:
stage: test
                                                                      See: https://gitlab.au.dk/au-ece-swt/sw4swt-
variables:
                                                                      student/tools/-
GIT_STRATEGY: clone                                                   /blob/main/gitlab/GitLabCIWithCoverage.gitlab-
script:                                                               ci.yml?ref_type=heads
# Build and run tests
- 'dotnet clean'
- 'dotnet build'
# dotnet test will dump coverage.cobertura.xml in tests/MyThing.Tests/TestResults/<number>/coverage.cobertura.xml
- 'dotnet test --collect:"XPlat Code Coverage" --logger:"junit;MethodFormat=Class;FailureBodyFormat=Verbose"'
# Create Report using ReportGenerator: https://reportgenerator.io/usage
# When invoking nuget package directly it will execute from sln root folder
# The report path is therefore set to ./*/*/*/*/coverage.cobertura.xml
- 'dotnet ~/.nuget/packages/reportgenerator/*/tools/net8.0/ReportGenerator.dll -
reports:./*/*/*/*/coverage.cobertura.xml -targetdir:coveragereport "-reporttypes:Html;TextSummary"'
# Display Coverage Summary (used by coverage regex to display coverage in GitLab)
- 'cat ./coveragereport/Summary.txt'


     AARHUS                                                SWT    PETER HØGH MIKKELSEN
                                                                                           Continued….
     UNIVERSITY                                  SEPTEMBER 2025   TEST COVERAGE
     T ECH N IC AL SC IEN C ES

<!-- side 15 -->

# Display Coverage Summary (used by coverage regex to display coverage in GitLab)
- 'cat ./coveragereport/Summary.txt'
# Regex expression applied to job log to subtract number to be displayed in GitLab
coverage: '/Line coverage: (\d+.\d+)/'
# coverage: '/Branch coverage: (\d+)/'
artifacts:
when: always
untracked: true
paths:                                                                          See: https://gitlab.au.dk/au-ece-swt/sw4swt-
- coveragereport                                                                student/tools/-
                                                                                /blob/main/gitlab/GitLabCIWithCoverage.gitlab-
reports:
                                                                                ci.yml?ref_type=heads
junit:
- ./**/TestResults.xml
coverage_report:
coverage_format: cobertura
path: ./**/coverage.cobertura.xml




     AARHUS                                                SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                                  SEPTEMBER 2025   TEST COVERAGE
     T ECH N IC AL SC IEN C ES

<!-- side 16 -->

CODE COVERAGE FROM TERMINAL
 What is done in the CI script, can also be done in your local terminal:

dotnet test --collect:"XPlat Code Coverage" --
logger:"junit;MethodFormat=Class;FailureBodyFormat=Verbose"'
dotnet ~/.nuget/packages/reportgenerator/*/tools/net10.0/ReportGenerator.dll -
reports:./*/*/*/*/coverage.cobertura.xml -targetdir:coveragereport "-reporttypes:Html;TextSummary"



 Which is also done with this script:

  ./build_coverage_report.sh


https://gitlab.au.dk/au-ece-swt/sw4swt-student/tools/-/blob/main/dotnet/scripts/build-coverage-report.sh?ref_type=heads


  Reports can be found inside the test project folder


     AARHUS                                                  SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                                    SEPTEMBER 2025   TEST COVERAGE
     T ECH N IC AL SC IEN C ES

<!-- side 17 -->

ALWAYS DO COVERAGE ON CI BUILDS?

  Running coverage calculation takes time because extra code is
  executed in the UUT

  Depending on project complexity one must consider whether to
  run coverage on
    • Each push?
    • Nightly build?


  AARHUS                                SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  SEPTEMBER 2025   TEST COVERAGE
  T ECH N IC AL SC IEN C ES

<!-- side 18 -->

AARHUS
UNIVERSITY

