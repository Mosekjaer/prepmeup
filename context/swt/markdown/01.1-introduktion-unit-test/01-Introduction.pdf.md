---
title: "Introduction.pdf"
source: "Introduction.pdf"
modul: "Lektion 01.1: Introduktion + Unit Test"
pages: 20
type: "slides"
vision: "done"
---
# Introduction.pdf

<!-- side 1 -->

SW4SWT INTRODUCTION


AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   SW4SWT INTRODUCTION
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

LECTURERS
 Peter Høgh Mikkelsen
 phm@ece.au.dk
 Lokale 5123-424
 Helsingforsgade 10



 Jacob Brinth Assenholm
 jba@ece.au.dk
 Lokale 5123-424
 Helsingforsgade 10



  AARHUS                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 3 -->

TEACHING ASSISTANTS



                   Oliver Kok                                              202107339@post.au.dk
                   Morten Husted                                           202109169@post.au.dk
                   Oliver Vestergaard Schousboe                            202008211@post.au.dk




  AARHUS                                             SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 4 -->

OVERORDNEDE MÅL – KVALITET!

                                   Fokus ligger på at sætte den
                                     studerende i stand til at
                              kvalitetssikre sin egen programkode,
                                og kvalitetssikre større systemer,
                                 som udvikles af flere udviklere.




  AARHUS                                          SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                             FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 5 -->

LÆRINGSMÅL
         Anvende et unit test framework til kvalitetssikring af programkode
         Identificere afhængigheder i software og anvende designteknikker til at reducere disse.
         Beskrive og anvende et isolation framework til isolering af programenheder.
         Beskrive, sammenligne og anvende udvalgte typer af code coverage til kvalitetssikring af tests.
         Foretage grænseværdianalyse til kvalitetssikring af tests
         Anvende udvalgte værktøjer til kvalitetssikring af programkode og tests
         Anvende versionsstyringsværktøjer til versionskontrol og sikring af programkode
         Beskrive anvendelsen af versionsstyringsværktøjer i en moderne udviklingsproces
         Beskrive en Continuos Integration proces
         Anvende et Continuos Integration værktøj til automatisk build, test og integration af programkode
         Beskrive, planlægge og udføre integrationstest
         Beskrive og anvende værktøjer til specifikation og gennemførelse af system- og accepttest.



 AARHUS                                                   SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                      FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 6 -->

CONCEPTS
    Unit test
    Design for testability
    Continuous integration
    Isolation frameworks
    Kvalitetssikring af programkode og tests
    Integrationstest
    System- og accepttest
    Versionsstyring, branching, releasestyring




 AARHUS                                            SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                               FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 7 -->

THE MASTER PLAN                                                                                                              Altid Check Brightspace
                                                                                                                             For den opdaterede plan!
                              U/V uge                      Emne                  Emne
                                        Opsætning og testøvelse                  Git Workflow 1
                                1
                                        Introduktion til I4SWT + Unit Tests      Exceptions (selvstudie)
                                        A-A-A, Velstrukturerede test
                                        Continuous Integration                   Continuous Integration
                                2
                                        Introduktion Handin 1                    Arbejde Handin 1
                                        Design for Testability,                  Design for Testability
                                        Black box/White Box                      Arbejde Handin 1
                                3
                                        Interfaces (selvstudie), Arbejde Handin
                                                                                 Aflevering fredag
                                        1
                                        Fakes
                                4                                                Fakes and Isolation Frameworks
                                        Types of test and fakes
                                        Test Quality 1
                                5                                                Test Quality 2
                                        Coverage, BVA/EP, Zombie
                                        Introduktion Handin 2
                                6                                                Arbejde Handin 2
                                         Events og Test af events
                                7       Arbejde Handin 2                         Arbejde Handin 2
                                        Efterårsferie
                                        Software Quality Analysis                Software Quality Analysis
                                8
                                        Arbejde Handin 2                         Arbejde Handin 2 Aflevering fredag
                                        Integrationstest – patterns              Integrationstest – plan, implementations
                                9
                                        Mikrobølgeovnen                          review hand-in 2
                                        Git Workflow 2 – branches og releases,
                                10                                               Git Workflow 2 – branches og releases, CI
                                        CI
                                11      Arbejde Handin 3                         Arbejde Handin 3
                                                                                 Arbejde Handin 3
                                12      Arbejde Handin 3
                                                                                 Aflevering fredag
                                13      Gæsteforelæsning                         System-/Accepttest
                                        Feedback Handin 3
                                14                                               Repetition/Spørgetime
  AARHUS                                Slutevaluering, eksamensinfo           SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                           FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 8 -->

READING MATERIAL
  Slides

  Supplementary Reading

  Binder: ”Testing Object-Oriented Systems: Models, Patterns, and Tools” (1
  chapter) (Made available on Brightspace)

  Various papers, videos, etc.




  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

MANDATORY HAND-INS
 1. CI-kørekort
    • Introduktion – 4/2
    • Deadline – 13/2
                                                                   Alle datoer er
 2 . Anden obligatoriske aflevering                                med forbehold
    • Introduktion – 4/3                                           Datoerne på
    • Deadline – 20/3
                                                                   Brightspace
 3. Tredje obligatoriske aflevering                                gælder!
    • Introduktion – 8/4
    • Deadline – 24/4




 AARHUS                                        SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                           FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 10 -->

GROUPS/TEAMS
 All mandatory Hand-Ins are done in groups of 3-4
 Sign up for groups on Brightspace
 Groups must be formed by 4/2
 Change of group between Hand-Ins is allowed
  • Be sure to do it on Brightspace as soon as possible
  • Tell the teachers!
 Each group uses a GitLab.au.dk account with repositories




 AARHUS                                         SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                            FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 11 -->

EXAM
 All Mandatory Hand-ins must be approved to get access to the final exam

 Final exam is a 2-step process:
        • 24 hour work phase with a fixed system:
           • Design, code, test, answer questions
        • 20 minute oral exam with external examiner
                  • 2-3 days later
                  • Based on the 24 hour work phase, with possible discussions of the complete
                    theoretical background




 AARHUS                                                     SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                        FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 12 -->

EXPECTATIONS
 Make an effort!
  • Prepare and get an overview in advance
  • The interesting discussions are in class, not in the book!

 The process is the result!




 AARHUS                                           SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                              FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 13 -->

WHAT IS SOFTWARE TEST?




  AARHUS                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 14 -->

ONE DEFINITION OF SOFTWARE TEST


                              A systematic and objective verification of a
                              software unit’s state and behavior, based on
                                   objective and predefined criteria.




  AARHUS                                                SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                   FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 15 -->

WHAT IS SOFTWARE TEST?
  Systematic, objective verification
         • Measurable, repeatable – anybody can repeat the test and get identical results
         • Automated
  Objective criteria
    • Measurable, undisputable – anybody can measure the extent to which the testing has
      been carried out with no room for interpretation
  Predefined criteria
         • The set of criteria used to select…
            • which parts of the software to test
            • how to test them
            • how to measure the results
         • … should be determined before the testing begins

  AARHUS                                              SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                 FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 16 -->

THE REAL WORLD
 Fact: We will have to make real-world compromises

                                                      Cost


                                       Quality                       Functionality


 We must make testing cost-effective
       •            Pick the low-hanging fruits first.
       •            Test the most critical areas
       •            Automate the tests!

  AARHUS                                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                      FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

**Figur:** Trekantsdiagram med tre mørkeblå ellipser: Cost øverst, Quality nederst til venstre, Functionality nederst til højre. Alle tre par er forbundet med dobbeltpile (Cost↔Quality, Cost↔Functionality, Quality↔Functionality), altså et trade-off hvor ændring af én størrelse påvirker de to andre.

<!-- side 17 -->

THE V-MODEL
  Testing at different layers for different purposes



                              Requirements                                                           Accept testing

                              System specification                                              System testing

                                     System design                                  Integration testing

                                       Component design                    Unit test


                                                     Implement component



  AARHUS                                                          SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                             FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

**Figur:** V-model tegnet som to vandrette bånd: et lyst øverste bånd med de to øverste abstraktionsniveauer og et mørkere nederste bånd med de tre nederste. Venstre side (nedstigende udviklingsgren, forbundet af en skrå pil ned mod bunden): Requirements → System specification → System design → Component design → Implement component. Højre side (opstigende testgren, forbundet af en skrå pil op fra Implement component): Unit test → Integration testing → System testing → Accept testing. Vandrette dobbeltpile parrer hvert niveau med sit testniveau: Requirements ↔ Accept testing, System specification ↔ System testing, System design ↔ Integration testing, Component design ↔ Unit test. Pilene angiver, at hvert designniveau verificeres af sit tilsvarende testniveau.

<!-- side 18 -->

TWO DIFFERENT WAYS OF DOING THE JOB
                                • A single large cycle (waterfall)
         Specify                                                                         Feedback
         Design
         Implement
         Test

                              • Several smaller cycles (iterative)
                                                                                         Feedback
                              Specify
                              Design
                              Implement
  AARHUS
                              Test                         SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                      FEBRUARY 2026   SW4SWT INTRODUCTION
  T ECH N IC AL SC IEN C ES

<!-- side 19 -->

THE MODERN TEST MANTRA

                                                                    Test early
                                                                    Test often
                                                                    Test enough




 AARHUS                               SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   SW4SWT INTRODUCTION
 T ECH N IC AL SC IEN C ES

<!-- side 20 -->

AARHUS
UNIVERSITY

