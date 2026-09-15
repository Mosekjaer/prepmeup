---
title: "Continuous-Integration"
source: "Continuous-Integration.pdf"
modul: "Lektion 02.1: Continuous Integration"
pages: 15
type: "slides"
vision: "done"
---
# Continuous-Integration

<!-- side 1 -->

CONTINUOUS INTEGRATION
(GITLAB)


AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   SW4SWT – CONTINUOUS INTEGRATION
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

AGILE DEVELOPMENT CYCLE

                                                                                    Feedback
                 Specify
                 Design
                 Implement
                 Test


   Agile developement provides functional increments, that can be integrated
   continuously, this process is also known as Continuous Integration



  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 3 -->

A CI DEFINITION

                              A software development practice where members
                               of a team integrate their work frequently, usually
                               each person integrates at least daily – leading to
                              multiple integrations per day. Each integration is
                              verified by an automated build (including test) to
                              detect integration errors as quickly as possible (…)

                                                                                                     Martin Fowler, 2006




  AARHUS                                                    SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                       FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 4 -->

THE PRINCIPLE




  AARHUS                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

**Figur:** Flowdiagram over CI-cyklussen: fra boksen "Developers" går pilen "Code check-in" til et versionsstyringsrepository (serverikon i midten). Fra repositoriet går "Code check-out" til boksen "Build tool" øverst til højre, og "Code commit" til "CI server" nederst til højre. Mellem CI server og Build tool løber to lodrette pile: "Start build" opad og "Build status" nedad. Fra CI server går en pil til beslutningsromben "CI status?", som dels sender "Send notification" tilbage til Developers, dels rapporterer videre til boksen "Clients, testers, team leaders, etc."

<!-- side 5 -->

BUILD AUTOMATION & TEST
                                                                                                        Workstation 1

                Build server

                                                                                                           Build application
                                                              Code
                                                                                                           + ”Green Zone” tests
                                                            repository


                                                                                                        Workstation 2
 Fast integration:             Nightly:
 Build application             Build application
 + ”Green Zone” tests          + ”Green Zone” tests
                               + Coverage
                               + Integration tests
                               + Code inspection
                                                                                                           Build application
                               + Reporting
                                                                                                           + ”Green Zone” tests
                               +…

   AARHUS                                                      SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                                         FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
   T ECH N IC AL SC IEN C ES

**Figur:** Topologidiagram: en central blå cylinder "Code repository". Til venstre en tyk pil fra repositoriet ind i et rack-billede mærket "Build server" (envejs — build-serveren henter kode). Til højre to dobbeltpile til hhv. "Workstation 1" og "Workstation 2" (laptop-ikoner), som hver har en tekstboks "Build application + ”Green Zone” tests". Under build-serveren står to konfigurationsbokse: "Fast integration" (Build application + ”Green Zone” tests) og "Nightly" (Build application + ”Green Zone” tests + Coverage + Integration tests + Code inspection + Reporting + …). Diagrammet viser altså at udviklere kører den hurtige testmængde lokalt, mens build-serveren kører en gradueret mængde builds.

<!-- side 6 -->

BENEFITS OF CI
  No ”integration hell” at the ”end” of a project
  Never more than hours from working (deployable) build
  Rapid feedback
  Several graded builds (continuous, nightly, …)
  Metrics for code and test quality never out of date
  Certified and controlled runtime environment (on CI server)




  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 7 -->

STEPS OF A TYPICAL CI JOB
  For basic building and testing these are the typical steps to specify:
      1. Pull the project from a given SCM (ex. Git) repository (and branch)
      2. Build the binaries from scratch
      3. Run tests and/or other tools
      4. Publish the results for presentation

    Options for triggering and schedule can be configured




  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 8 -->

TRIGGER SEQUENCE




 AARHUS                               SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
 T ECH N IC AL SC IEN C ES

**Figur:** UML-sekvensdiagram med livslinjerne (venstre mod højre): User (aktør), Local Files, Local Repo, GitLab Git Server, GitLab CI Server, GitLab Runner. Beskeder i rækkefølge: User → Local Files: "Code and Test"; Local Files → Local Repo: "commit"; Local Repo → GitLab Git Server: "push"; GitLab Git Server → GitLab CI Server: "Trigger"; GitLab CI Server → GitLab Runner: "Run job" (runneren får en aktiveringsbjælke); GitLab Runner → GitLab Git Server: "pull"; GitLab Runner → sig selv: "build and run tests" (selvkald); GitLab Runner ⇢ GitLab CI Server: "Test Results" (stiplet retursvar); GitLab CI Server ⇢ User: "Test Report" (stiplet).

<!-- side 9 -->

THE SETUP IN SWT
                                                                                                       Student workstation
 GitLab CI server                           GitLab git server
                                    https://gitlab.au.dk/user/repository

                                                                                                        Build application
                                                                                                        + ”Green Zone” tests

                                                                                                       Student workstation
                              GitLab runners
                              Ex: gitrunner12-2.uni.au.dk

                                                Test Image                                              Build application
                                                   Repo                                                 + ”Green Zone” tests
                                               (Ex Microsoft)

  AARHUS                                                      SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                         FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

**Figur:** Infrastrukturdiagram for kursets opsætning. Tre servertårne og to laptops: "GitLab CI server" (venstre) har dobbeltpil til "GitLab git server" (midten, https://gitlab.au.dk/user/repository) og dobbeltpil nedad til gruppen "GitLab runners" (Ex: gitrunner12-2.uni.au.dk, tegnet som flere tårne). Fra runnerne går en envejspil til en sky mærket "Test Image Repo (Ex Microsoft)", hvorfra container-images hentes. Fra git-serveren går dobbeltpile ud til to "Student workstation"-laptops, som hver har tekstboksen "Build application + ”Green Zone” tests".

<!-- side 10 -->

GITLAB CI/CD PIPELINE




  AARHUS                               SWT    PETER HØGH MIKKELSEN              From:
  UNIVERSITY                  FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

**Figur:** Arkitekturdiagram over GitLabs CI/CD-pipeline med signaturforklaring nederst: stiplet linje = data flow, fuld linje = process flow. Venstre kolonne "Pipeline triggers" (alle med linje til CreatePipelineService): Git push, Web API, Web UI, Merge Request (created/updated), Merge Train, Pipeline Schedule, Subscription to upstream project, Edit CI/CD settings (AutoDevops), GitHub PR event (created/updated), samt "Bridge job execution" der går til CreateCrossProject-PipelineService (to be renamed). En Actor-figur står ved trigger-listen. CreatePipelineService modtager data fra "gitlab-ci.yml Processor (DSL + Linter)" og sender videre til ProcessPipelineService; CreateCrossProject-PipelineService har pilen "create downstream pipeline" op til CreatePipelineService og linjen "when bridge job becomes pending" tilbage til Bridge job execution. En Actor øverst udfører "Retry pipeline / Retry job / Play manual job" mod ProcessPipelineService. ProcessPipelineService skriver "changes to jobs status" til cylinderen Database, som også forbindes med RegisterJobService, Test Reports (dokumentsymbol) og ObjectStorage (sky). Til højre kassen "Runner API Gateway (api/v4/runner)" med endpointsene register runner, delete runner, verify runner, Request a job, Update a job, Append log, Authorize artifact, Upload artifact, Download artifact; gatewayen forbindes til RegisterJobService/Database/ObjectStorage og til højre til "Shared / Group / Project Runners": Runner 1 … Runner N.

<!-- side 11 -->

PREREQUISITES FOR OUR CI JOBS
  The NuGet package JunitXml.TestLogger must be added to the test project because the
  JUnit report format is used on GitLab. (CLI: dotnet add package JunitXml.TestLogger)
                              # From testproject.csproj

                              <ItemGroup>
                              <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.3.2" />
                              <PackageReference Include="NUnit" Version="3.13.3" />
                              <PackageReference Include="NUnit3TestAdapter" Version="4.2.1" />
                              <PackageReference Include="NUnit.Analyzers" Version="3.3.0" />
                              <PackageReference Include="coverlet.collector" Version="3.1.2" />
                              <PackageReference Include="JunitXml.TestLogger" Version="2.1.10" />
                              </ItemGroup>

  A file containing the script on the next page must be placed in the solution folder of your
  project
    • The name of the script file must be ".gitlab-ci.yml"

  See more detals in today's exercise
  AARHUS                                                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                  FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 12 -->

PIPELINE SPECIFICATION: .GITLAB-CI.YML
  # See https://docs.gitlab.com/ee/ci/testing/unit_test_report_examples.html
  image: mcr.microsoft.com/dotnet/sdk:10.0
  build-and-test:
        stage: test                                                    This must match your project’s
                                                                                .NET version
        variables:
               GIT_STRATEGY: clone
        script:
               - 'dotnet clean’
               - 'dotnet build’
               - 'dotnet test --logger:"junit;MethodFormat=Class;FailureBodyFormat=Verbose”’
        artifacts:
               when: always
               reports:
                     junit:
                              - ./**/TestResults.xml

  AARHUS                                                        SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                           FEBRUARY 2026    SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 13 -->

STEPS OF OUR GITLAB CI JOB
   1. Invoke a runner with a virtual machine with the dotnet SDK
   2. Pull the project from the repository
   3. Clean, build the binaries from scratch
       1. Install NuGet packages in .csproj (Nunit, JUniXml a.o.)
   4. Run tests
   5. Publish the results for presentation

    See the job log on GitLab (It’s a terminal dump, you can read it ☺)




  AARHUS                                     SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                        FEBRUARY 2026   SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 14 -->

TEST RUNNER

                                                           Calculator.Test.Unit.dll
                                                               [TestFixture]
                              Calculator.dll      uses                                                 uses          Nunit.Framework.dll
                                                                   [Test]
                                                                  [Setup]...


                                                                             Search for and run tests

                                                                                                                        Needs    Test Runners
                                   Nunit3-                                              dotnet test             NUnit3Adapter
                                console from   Resharper     Test Explorer               from the          JunitXml.TestLogger
                               command line     from VS          in VS                command line
                                                                                                                                    and more
                                  (Jenkins)                                            (e.g. GitLab)




                                                                         Test Results



  AARHUS                                                                 SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                                    FEBRUARY 2026    SW4SWT - CONTINUOUS INTEGRATION
  T ECH N IC AL SC IEN C ES

<!-- side 15 -->

AARHUS
UNIVERSITY

