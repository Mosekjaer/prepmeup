---
title: "SWT-IntroductionHandin3"
source: "SWT-IntroductionHandin3.pdf"
modul: "Lektion 10.1 + 10.2: Hand-in 3"
pages: 14
type: "slides"
vision: "ingen-grafik"
---
# SWT-IntroductionHandin3

<!-- side 1 -->

INTRODUCTION TO
MICROWAVE OVEN WITH
NEW FEATURES USING A GIT
BASED WORKFLOW

AARHUS                        SWT    PETER HØGH MIKKELSEN
UNIVERSITY           05 APRIL 2024   INTRODUCTION TO HAND-IN 3
TECHNICAL SCIENCES

<!-- side 2 -->

MICROWAVE OVEN
 •      This exercise will use a finished and tested software for a Microwave Oven
 •      It is described in the material made available for lecture 09.1
 •      You will add new features, using the workflows described in the lectures 10.1 + 2
 •      The new features should be tested and old unit tests should be updated if necessary
 •      You must also make a dependency tree and an integration plan – but integration tests
        are not needed




 All details:
     https://gitlab.au.dk/au-ece-swt/sw4swt-student/handins/microwaveoven-handin



 AARHUS                                            SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
 TECHNICAL SCIENCES

<!-- side 3 -->

THE MICROWAVE OVEN CLASS DIAGRAM

                                                   Display                                                   Output




                      Button   3



                                   UserInterface                                                  CookController



                      Door



                                                             Light                                   Timer            PowerTube



 AARHUS                                                        SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                            05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
 TECHNICAL SCIENCES

<!-- side 4 -->

THE MICROWAVE OVEN STM DIAGRAM
                                                                                                                 DoorIsClosed/Turn Off Light


                                                                                               Ready            DoorOpens/Turn On Light        Door is Open



                                                                                                               Start-Cancel Button Pressed/
                                                                                                                      Reset Values,
                                                                              Press Power Button/Display Power         Clear Display
                                                                                                                                     DoorIsOpened/
                                                                                                                                     Reset Values,
                                                                                                                                     Clear Display,
                                                                  Press Power Button/                                                Turn On Light
                                                                   Increase Power,           Set Power
                                                                    Display Power

               Start-Cancel Button Pressed/   Cooking Finished/
                      Stop Cooking,            Reset Values,
                      Reset Values,            Clear Display,                        TimeButtonPressed/Display Time
                      Clear Display,            TurnOffLight
                       TurnOffLight
                                                                                                                                                  DoorIsOpened/
                                                                                                                                                  Reset Values,
                                                            TimeButtonPressed/                                                                    Clear Display,
                                                              Increase Time,                  Set Time                                            Turn On Light
                                                               Display Time
                                                                                                                                                                   DoorIsOpened/
                                                                                                                                                                   Stop Cooking,
                                                                                                                                                                   Reset Values,
                                                                      Start-Cancel Button Pressed/Start Cooking, Turn On Light                                      Clear Display




                                                                                              Cooking


 AARHUS                                                                                       SWT        PETER HØGH MIKKELSEN
 UNIVERSITY                                                                           05 APRIL 2024      SW4SWT INTRODUCTION TO HAND-IN 3
 TECHNICAL SCIENCES

<!-- side 5 -->

NEW FEATURES TO ADD
  Mandatory features


                      Add a buzzer to sound at the end of cooking and perhaps
                      at other times
                      Make the power of the power tube configurable
                      Allow changing the cooking time during cooking


  Optional features
                      Add a button for seconds to allow short times
                      Change input of time to use a progressive time scale: 5 s,
                      10 s, 20, …, 1 min, 2 min, …
                      A feature of your own choice


 AARHUS                                             SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                 05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
 TECHNICAL SCIENCES

<!-- side 6 -->

MANDATORY WORKFLOW
 For each feature
   • Work in parallel on different feature branches
   • Create a feature branch for each feature
   • Merge/integrate often from the main branch, remember to fetch first
   • Commit often in the feature branch

 When integrating the feature into the main branch
  • Fetch and Merge or Rebase from main one last time and solve problems, test and
    commit
  • Create a merge-request
  • Get the approval of the merge-request through a code review



 AARHUS                                        SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                            05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
 TECHNICAL SCIENCES

<!-- side 7 -->

DELIVERABLES
                                                                                               GitLab Repo

                                                                Merge-
                                  Pipeline                      request
                                                                    Merge-
                                  withPipeline
                                       test                      from
                                                                   request
                                                                        Merge-
                                    and
                                     withPipeline
                                           test                 feature
                                                                     from
                                                                        request
                                                                            Merge-
     Documentation
       Report as PDF              coverage
                                        and
                                         withPipeline
                                               test            branch  tofrom
                                                                    feature
                                                                 main       request
                                     coverage
                                            and
                                             with test            branch   tofrom
                                                                        feature
                                         coverage                    main
                                                                      branch   to
                                                and                         feature
                                                                         main
                                                                                                                Zipped
                                             coverage                     branch to
                                                                                                                Release
                                                                             main                                         Feedback Fruits


                       GitLab repo with main branch and feature branches




  AARHUS                                                             SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                                 05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
  TECHNICAL SCIENCES

<!-- side 8 -->

DELIVERABLES
  In FeedbackFruits you must submit a release of your repo containing:
       • ./README.md: Group number, Group members and study numbers, URL to your GitLab repo
       • ./docs/README.md: Documentation as required in exercise description
       • ./: Sln, projects, gitignore, ci scripts etc.


  In your GitLab repo, the following must be avaliable:
       • All application, class library and unit test source code
       • Main branch, feature branches and all pull-requests
       • Pipeline
       • Do not clean up, do not delete the feature branches! We need to see the history!!!


  Check the how the hand-in is evaluated on the hand-ins gitlab:
       • https://gitlab.au.dk/au-ece-swt/sw4swt-student/handins/microwaveoven-handin


  AARHUS                                                         SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                             05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
  TECHNICAL SCIENCES

<!-- side 9 -->

MAIN POINTS
  •        Using feature branches
  •        Using git functions for a transparent history
  •        Using merge-request for a safer feature release
  •        Using CI to follow progress and verify feature
  •        Working in parallel in a structured workflow
  •        Practice testable design, coding and testing




  AARHUS                                    SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                        05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
  TECHNICAL SCIENCES

<!-- side 10 -->

EXERCISE PLAN
  Start:                    08.04.2026 14:15

  Hand-in Deadline:         23.04.2026 23:59
       • Journal + all merge-requests processed

  Peer Review Deadline : 30.04.2026 23:59




  AARHUS                                           SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                               05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
  TECHNICAL SCIENCES

<!-- side 11 -->

TIME ALLOTTED
  Time allotted for the exercise:
    • Lectures in study weeks 11 and 12. See lecture plan on Brightspace
    • All normal preparation time for those lectures

  Support and resources
    • Jacob and TAs will be present during normal lecture times

  Feedback:
    • Summary/key points: Lesson 14.1 (at the latest).




  AARHUS                                          SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                              05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
  TECHNICAL SCIENCES

<!-- side 12 -->

TIPS AND TRICKS
  It is absolutely necessary that you have set up your GitLab repo, your local repos, and a
  pipeline before you make any new feature!!!
  As further tips and tricks are found – see them on Brightspace!




  AARHUS                                          SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                              05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
  TECHNICAL SCIENCES

<!-- side 13 -->

QUESTIONS?




 AARHUS                       SWT     PETER HØGH MIKKELSEN
 UNIVERSITY           05 APRIL 2024   SW4SWT INTRODUCTION TO HAND-IN 3
 TECHNICAL SCIENCES

<!-- side 14 -->

AARHUS
UNIVERSITY

