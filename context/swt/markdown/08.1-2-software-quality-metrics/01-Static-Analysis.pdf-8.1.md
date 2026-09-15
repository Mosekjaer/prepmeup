---
title: "Static Analysis.pdf (8.1)"
source: "Static Analysis.pdf"
modul: "Lektion 08.1+2: Software Quality Metrics"
pages: 29
type: "slides"
vision: "done"
---
# Static Analysis.pdf (8.1)

<!-- side 1 -->

SOFTWARE QUALITY
METRICS - STATIC ANALYSIS


AARHUS                            SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  MARCH 2026   STATIC ANALYSIS
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

CONTENTS
• Intro to software quality metrics /static analysis
• Complexity of programs
• Code style
• Static analysis tools / the compiler.
• Hard problems / impossible problems.
• Code reviews.




    AARHUS                              SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                    MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 3 -->

SOFTWARE METRICS CLASSIFICATION
                                            Quantitative                                                                Qualitative

                         Lines of Code (LOC), Cyclomatic Complexity,                                Code Readability, Usability, and Documentation
Static




                                     Maintainability Index,                                                            Quality
                                         Defect Density
                                Metrics based on static analysis                                      Metrics based on code review and inspection

                            Execution Time, Memory Usage, and Code                                   User Satisfaction, Crash Management, and Risk
Dynamic




                                            Coverage                                                                     Exposure

                       Metrics derived from profiling during program                               Metrics based on user feedback, data collection
                                         execution                                                            and post-mortem analysis



                                      "Software Metrics: A Rigorous and Practical Approach" by Norman Fenton and James Bieman



          AARHUS                                                                   SWT    PETER HØGH MIKKELSEN
          UNIVERSITY                                                         MARCH 2026   STATIC ANALYSIS
          T ECH N IC AL SC IEN C ES

<!-- side 4 -->

PROGRAM COMPLEXITY
        Accidental Complexity
       • Property of development limitations
       • Inadequate design
       • Inadequate development language and tools
       • Undesirable

        Essential Complexity
       • Property of the problem
       • Sophistic business logic
       • Non-functional requirements: scalability, fault-tolerance
       • Unavoidable

 AARHUS                                                                    SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                                          MARCH 2026   STATIC ANALYSIS
 T ECH N IC AL SC IEN C ES
                             Frederic Brooks, “No Silver Bullet”, 1986

<!-- side 5 -->

CONSEQUENCE OF ESSENTIAL COMPLEXITY
• Essential complexity is unavoidable
• Tools cannot distinguish between accidental and essential complexity
• We cannot mandate policy on how much complexity we will allow
• Use quality measures as indicator:
  • Reduce accidental complexity
  • Test essential complexity




    AARHUS                            SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                  MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 6 -->

CYCLOMATIC COMPLEXITY
                                                                                                    Control-Flow Graph (CFG)

• Quantifies the complexity of a program by
                                                                                   void func()
  counting the number of linearly                                                  {
  independent paths through the source




                                                                                                                         https://en.wikipedia.org/wiki/Cyclomatic_complexity
                                                                                    action0;
  code                                                                              while (cond1)
                                                                                    {
                                                                                     action1;       while(cond1)
                                                                                     action0;
• For structured programs, complexity:
                                                                                    }
  M = number of decision points + 1 (here: 2)                                       if (cond2)        If (cond2)
                                                                                    {
                                                                                     action2;
• Note! Compound boolean expressions                                                }
  are counted as one, ex: if(0 < x && x < 10))                                      action3;
                                                                                   }



     AARHUS                                                  SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                                        MARCH 2026   STATIC ANALYSIS
     T ECH N IC AL SC IEN C ES
                                 Thomas McCabe, 1976

**Figur:** Til højre et control-flow-graph (CFG) for koden i den midterste kodeboks, tegnet som cirkelnoder forbundet med pile. Rød udfyldt node øverst = entry, mørkeblå udfyldt node nederst = exit. Fra entry går en pil ned til en hvid node (`action0`). Herfra pil ned til den node, der er mærket `while(cond1)`. Denne while-node har en pil ud til højre til en sidenode (løkkekroppen `action1; action0;`), og fra sidenoden går en pil tilbage op til noden over while-noden — dvs. løkkekanten lukker cyklussen. Fra while-noden går en pil ned til noden mærket `If (cond2)`. If-noden har en pil skråt ned til højre til en sidenode (then-grenen `action2`), og både if-noden og then-noden har pile ned/tilbage til en fælles samlingsnode (`action3`). Fra samlingsnoden en pil ned til exit. To beslutningspunkter (while og if) giver M = 2 + 1 = 3 efter node/kant-formlen, mens tommelfingerreglen «decision points + 1» i slidens tekst tæller 2.

<!-- side 7 -->

CYCLOMATIC COMPLEXITY                                                       CFG

•   M= Complexity
•   E = Number of edges of the graph.
•   N = Number of nodes of the graph.




                                                                                  https://en.wikipedia.org/wiki/Cyclomatic_complexity
•   P = Number of connected components (unconnected sub-graphs)

• M = E − N + 2P
• Here: M = 9 – 8 + 2*1 = 3

•   1 - 10: Simple procedure, little risk
•   11 - 20: More complex, moderate risk
•   21 - 50: Complex, high risk
•   > 50: Untestable code, very high risk

    AARHUS                                    SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                          MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

**Figur:** Samme CFG som på foregående side gentages til højre, nu uden kode ved siden af: rød entry-node → hvid node → node med udgående sideløkke (pil ud til højre og tilbage op) → node → node med sidegren til højre og pile tilbage til samlingsnoden → mørkeblå exit-node. Grafen har 8 noder og 9 kanter, hvilket er tallene, der indsættes i M = E − N + 2P = 9 − 8 + 2·1 = 3.

<!-- side 8 -->

GRAPH EXAMPLES
            Multiple Connected Graphs                               Fully Connected Graphs




                                                                                                      https://www.geeklawblog.com/2016/01/throwing-bodies-at-proble.html
            E=4
            N=4
                                     E=22
                                     N=22




                                    E=10
                                    N=10                                                   KN:E
          P=3


       M = E − N + 2P                                      K12:66 : M = 66 – 12 +1 = 55 =>
       M = (4+22+10) - (4+22+10) + 2*3 =>                  K12:66 : Untestable! (Very High Risk)
       M= 6 => SIMPLE! (Low Risk)
                                                           Could be a result very high coupling and
       Could be caused by multiple threads                 high degrees at each decision point
       with independent control loops
 AARHUS                                            SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                  MARCH 2026   STATIC ANALYSIS
 T ECH N IC AL SC IEN C ES

**Figur:** To grafeksempler side om side. Venstre, «Multiple Connected Graphs»: tre adskilte delgrafer af grønne knuder forbundet med rette streger. Øverst til venstre en lille firkant med E=4, N=4; i midten et stort forgrenet netværk mærket E=22, N=22; nederst et aflangt netværk med en trekant i sig mærket E=10, N=10. Antallet af sammenhængskomponenter står som P = 3. Højre, «Fully Connected Graphs»: et 2×2-panel med fire komplette grafer, hver med overskrift over sig — `K₇: 21`, `K₈: 28`, `K₁₁: 55` og `K₁₂: 66` — tegnet som blå knuder placeret på en cirkel, hvor hver knude er forbundet til alle øvrige, så billedet fyldes af krydsende diagonaler. Notationen `K_N:E` forklarer, at tallet efter kolon er antal kanter.

<!-- side 9 -->

CYCLOMATIC COMPLEXITY AND TESTING
The value is useful for assing test cases:
  • M is an upper bound for the number of (white box) test cases required for full
    branch coverage!
  • M is a lower bound for the number of paths through the control-flow graph (CFG)
    => lower bound of (white box) test cases to support full path coverage


                       branch coverage ≤ cyclomatic complexity ≤ number of paths


 • Support selection of test cases with Boundary Value Analysis and ZOMBIE


    AARHUS                                             SWT    PETER HØGH MIKKELSEN   https://en.wikipedia.org/wiki/Cyclomatic_compl
    UNIVERSITY                                   MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 10 -->

TESTING EXAMPLE
We have two branch points => combine to two tests:                                                                           k=0

Tests:        while(true) + if(true) while(false) + if(false)
=> Full branch coverage! (Lower bound, bug not found!)
                                                                                                                                   Bug: k set
                                                                                                                                   wrongly
We have four individual paths through the graph:                                                                  while(cond1)
Tests:       while(true) + if(true) while(true) + if(false)
                                     while(false) + if(true)    while(false) + if(false)
                                                                                                                    If (cond2)
=> Full path coverage! (Upper bound, bug found!)
                                                                                                                                   Use k


In practice we use the complexity, 3, and pick paths that cover most :
Tests:                               while(true) + if(false)
                                     while(false) + if(false)   => Bug found
                                     while(true) + if(true)
         AARHUS                                                                     SWT    PETER HØGH MIKKELSEN
         UNIVERSITY                                                           MARCH 2026   STATIC ANALYSIS
         T ECH N IC AL SC IEN C ES

**Figur:** Til højre samme CFG som på side 6-7, men annoteret med fejlens placering. Rød entry-node er mærket `k=0`. Sidenoden på while-løkkens højre gren er mærket «Bug: k set wrongly», og sidenoden på if-grenen er mærket «Use k». Fejlen opstår altså kun, hvis løkkekroppen udføres (while(true)) og derefter `k` bruges i if-grenen (if(true)) — den kombination mangler i det første testsæt, hvorfor branch coverage ikke afslører fejlen. Testkombinationerne i venstre side står i tre rammede tabeller: den første med to celler i én række, den anden med fire celler i 2×2, den tredje med tre celler under hinanden.

<!-- side 11 -->

REDUCING CYCLOMATIC COMPLEXITY
• Static Analysis returns a high CC, what to do?
  • Refactor code into smaller sub-functions
  • Replace conditionals with polymorphism fx using design patterns:
     • Strategy
     • State
     • Factory Method
     • Template Method
     • Command
     • Chain of Responsibility



    AARHUS                                 SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                       MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 12 -->

REDUCING CYCLOMATIC COMPLEXITY
switch (bird.type) {
 case 'EuropeanSwallow’:
  return "average";
 case 'AfricanSwallow’:
  return (bird.numberOfCoconuts > 2) ? "tired" : "average";
 case 'NorwegianBlueParrot’:
  return (bird.voltage > 100) ? "scorched" : "beautiful";
 default:
  return "unknown";           class EuropeanSwallow : Bird {
                               string plumage() { return "average” }}

                                            class AfricanSwallow : Bird {
    Strategy Pattern                         string plumage() {
                                              return (this.numberOfCoconuts > 2) ? "tired" : "average"; }}

                                            class NorwegianBlueParrot : Bird {
                                             string plumage() {
  https://www.refactoring.com/catalog/rep     return (this.voltage > 100) ? "scorched" : "beautiful"; }}
    laceConditionalWithPolymorphism.html

       AARHUS                                                SWT    PETER HØGH MIKKELSEN
       UNIVERSITY                                      MARCH 2026   STATIC ANALYSIS
       T ECH N IC AL SC IEN C ES

**Figur:** Two mørkeblå kodebokse med syntaksfarvning, hvor den nederste ligger delvist oven på den øverste. Mellem dem en tyk mørkeblå vinkelpil (går ned og drejer mod højre) med teksten «Strategy Pattern» til venstre — refaktoreringens retning fra switch-koden til klassehierarkiet. Øverste boks (før):
```
switch (bird.type) {
 case 'EuropeanSwallow’:
  return "average";
 case 'AfricanSwallow’:
  return (bird.numberOfCoconuts > 2) ? "tired" : "average";
 case 'NorwegianBlueParrot’:
  return (bird.voltage > 100) ? "scorched" : "beautiful";
 default:
  return "unknown";
```
Nederste boks (efter):
```
class EuropeanSwallow : Bird {
 string plumage() { return "average” }}

<!-- side 13 -->

REDUCING CYCLOMATIC COMPLEXITY
switch (bird.type) {                                class EuropeanSwallow : Bird {
 case 'EuropeanSwallow’:                             string plumage() { return "average” }}
  return "average";
 case 'AfricanSwallow’:                             class AfricanSwallow : Bird {
  return (bird.numberOfCoconuts > 2) ? "tired" :     string plumage() {
"average";                                             return (this.numberOfCoconuts > 2) ? "tired" : "average";
 case 'NorwegianBlueParrot’:                        }}
  return (bird.voltage > 100) ? "scorched" :
"beautiful";                                        class NorwegianBlueParrot : Bird {
 default:                                            string plumage() {
  return "unknown";                                   return (this.voltage > 100) ? "scorched" : "beautiful"; }}



              M=13-10+2=5                                           M=5-5+2=2              M=5-5+2=2   M=2-3+2=1




                                                                                       +               +           Total = 5
                                                                                                                   Individual ≤ 2




      AARHUS                                             SWT    PETER HØGH MIKKELSEN
      UNIVERSITY                                   MARCH 2026   STATIC ANALYSIS
      T ECH N IC AL SC IEN C ES

**Figur:** Samme to kodebokse som på side 12, nu placeret side om side (switch til venstre, klasserne til højre), og under hver af dem de tilhørende control-flow-grafer tegnet med røde entry-noder, hvide mellemnoder, mørkeblå exit-noder og mørkeblå pile. Venstre graf (switch'en, M=13−10+2=5): fra rød entry én pil ned til en hvid node, derfra tre pile ud i vifte til tre hvide noder på næste niveau, disse forgrener videre ned til fire hvide noder, og alle nederste noder samles med pile i den mørkeblå exit-node. En tyk mørkeblå højrepil i midten markerer refaktoreringen. Højre side: tre små separate grafer adskilt af store `+`-tegn. De to første er identiske (M=5−5+2=2): rød entry → hvid node → to hvide noder i forgrening → mørkeblå exit. Den tredje er en ren kæde (M=2−3+2=1): rød entry → hvid node → mørkeblå exit. Til højre står konklusionen `Total = 5`, `Individual ≤ 2` — den samlede kompleksitet er uændret, men fordelt på metoder er hver enkelt langt lavere.

<!-- side 14 -->

OTHER COMPLEXITY METRICS
Halstead Complexity
 • Use diversity as a complexity metric
 • Count distinct operations and operands
 • http://en.wikipedia.org/wiki/Halstead_complexity_measures



                                There are other types of complexity, but
                                cyclomatic complexity is by far the most
                                        used and useable one.
    AARHUS                                            SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                                  MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 15 -->

VISUAL STUDIO COMPLEXITY METRICS
         1.           Lines of code (LOC)
         2.           Class Coupling (classes that are referenced)
         3.           Depth of Inheritance
         4.           Cyclomatic Complexity (CC)
         5.           Maintainability Index:
                        𝑀 = 𝑚𝑎𝑥(0, 171 − 5.2 ∗ ln 𝐻𝑎𝑙𝑠𝑡𝑒𝑎𝑑 𝑉𝑜𝑙𝑢𝑚𝑒 − 0.23 ∗ 𝐶𝐶 − 16.2 ∗ ln 𝐿𝑂𝐶                                                                            ∗ 100/171)

                           Index value                          Color                      Meaning
                           0-9                                  Red                        Low maintainability of code
                           10-19                                Yellow                     Moderate maintainability of code
                           20-100                               Green                      Good maintainability of code
https://learn.microsoft.com/en-us/visualstudio/code-quality/code-metrics-maintainability-index-range-and-meaning?view=visualstudio
  https://learn.microsoft.com/en-us/visualstudio/code-quality/code-metrics-maintainability-index-range-and-meaning?view=visualstudio

                AARHUS                                                                                                                     SWT    PETER HØGH MIKKELSEN
                UNIVERSITY                                                                                                           MARCH 2026   STATIC ANALYSIS
                T ECH N IC AL SC IEN C ES

<!-- side 16 -->

COMPLEXITY METRICS SUMMARY

         Use metrics as indicators

        • Identify accidental complexity
        • Improve test coverage for essential complexity
        • Estimate work effort

         Do not use as policy

        • Essential complexity is unavoidable
        • Cannot set project rule of complexity < K

  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                         MARCH 2026   STATIC ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 17 -->

DEFECT DENSITY
Measures the number of defects relative to software size:

Defect Density = Number of Defects / Size of Software (KLOC)

Purpose:
   Evaluate software quality
   Compare quality between projects
   Track quality improvements


    AARHUS                             SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                   MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 18 -->

DEFECT DETECTION (STATIC)
Purpose: To locate defects found before testing

Such as:
 • Memory Leaks
 • Type violations
 • Stereotypical mistakes
 • Security problems



    AARHUS                            SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                  MARCH 2026   STATIC ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 19 -->

DEFECT DETECTION EXAMPLES
•   Is there an input that leads to a null pointer exception, division-by-zero, or
    arithmetic overflow?
•   Are all variables initialized before they are read?
•   Are nullable objects checked before usage?
•   Can input values from untrusted users flow unchecked to file system operations
    (injection)?
•   Does the program contain dead code?
•   Is the value of some expression the same at every iteration of a loop, making code
    inefficient?

Static Analysis tools can locate many of these defects!

     AARHUS                                 SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                       MARCH 2026   STATIC ANALYSIS
     T ECH N IC AL SC IEN C ES

<!-- side 20 -->

COMPILER’S STATIC CODE ANALYSIS
All modern compilers give warnings – some important, some less important.

It takes experience to know which warnings you can ignore.
It should be an informed decision to ignore a warning!
-You may be implementing sub-optimal, unsafe code, that will take much effort to
refactor later

The more type-strong the language is, the more checks the compiler can run!

The compiler is your friend! :-)

     AARHUS                                SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                      MARCH 2026   STATIC ANALYSIS
     T ECH N IC AL SC IEN C ES

<!-- side 21 -->

OTHER STATIC CODE ANALYSIS TOOLS
•   Lint - original qualitative static analysis tool
•   .NET Code Analysis (built into .NET SDK)
    • Apply more rules by adding this to project file:
       •             <AnalysisLevel>latest-Recommended</AnalysisLevel>
•   Resharper
•   SonarQube
•   Coverity
•   List of tools for .NET, including C#

•   All tools are limited to knowledge found in the code and thus cannot find
    wrong or incomplete business logic etc.
     AARHUS                                            SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                                  MARCH 2026   STATIC ANALYSIS
     T ECH N IC AL SC IEN C ES

<!-- side 22 -->

HARD PROBLEM EXAMPLE
Does the following terminate on every integer input n ?

                                  while (n > 1)
                                  {
                                    if (n % 2 == 0) // if n is even, divide it by two
                                      n = n / 2;
                                    else // if n is odd, multiply by three and add one
                                      n = 3 * n + 1;
                                  }


Collatz Conjecture (1937): “yes”
As of 2020, has been checked for all inputs up to 2 68
As of 2021, no proof that conjecture is true.


      AARHUS                                                     SWT    PETER HØGH MIKKELSEN
      UNIVERSITY                                           MARCH 2026   STATIC ANALYSIS
      T ECH N IC AL SC IEN C ES

<!-- side 23 -->

EVEN WORSE - IMPOSSIBILITY!
•   It has been mathematically proven that there is no general method – for all
    possible programs – that will determine that a given program will terminate on a
    given input
•   Turing's Halting Theorem from 1937!

•   Rice showed (1953) that (informally) all interesting questions about input/output
    behavior of programs is undecidable (impossible).

•   So, we can never guarantee 100% bug free software….we must test as complete
    as we can.


     AARHUS                                  SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                        MARCH 2026   STATIC ANALYSIS
     T ECH N IC AL SC IEN C ES

<!-- side 24 -->

SOFTWARE METRICS CLASSIFICATION
                                               Quantitative                                                                 Qualitative

                   Lines of Code (LOC), Cyclomatic Complexity,                                             Code Readability, Usability, and
Static




                                  Defect Density                                                              Documentation Quality
                                      Metrics based on static analysis                                   Metrics based on code review and inspection

                      Execution Time, Memory Usage, and Code                                           User Satisfaction, Crash Management, and Risk
Dynamic




                                      Coverage                                                                             Exposure

                  Metrics derived from profiling during program                                  Metrics based on user feedback, data collection and
                                    execution                                                                   post-mortem analysis



                                            "Software Metrics: A Rigorous and Practical Approach" by Norman Fenton and James Bieman



          AARHUS                                                                         SWT    PETER HØGH MIKKELSEN
          UNIVERSITY                                                               MARCH 2026   STATIC ANALYSIS
          T ECH N IC AL SC IEN C ES

<!-- side 25 -->

CODE READABILITY
  Ensure consistent coding style
   • Indentation
   • Placement of keywords, brackets, etc.
   • Naming convention (Vars, classes, namespaces, modules etc)
   • Language Guidelines

  Typically supported by IDE or formatting tool
   • Automated formatting while typing
   • Can be enforced during Git commit


  AARHUS                            SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  MARCH 2026   STATIC ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 26 -->

CODING STYLE ADVICE
Companies have coding styles that may not be your favorite – but there should be
standard, otherwise it will be difficult to read code if style changes all the time
from file to file.
   1. Follow the existing coding style
   2. Get acquainted with several coding styles
   3. See: https://learn.microsoft.com/en-
        us/dotnet/csharp/fundamentals/coding-style/coding-conventions
   4. See: https://learn.microsoft.com/en-
        us/dotnet/csharp/fundamentals/coding-style/identifier-names




   AARHUS                                 SWT    PETER HØGH MIKKELSEN
   UNIVERSITY                       MARCH 2026   STATIC ANALYSIS
   T ECH N IC AL SC IEN C ES

<!-- side 27 -->

CODE REVIEW & INSPECTION
•   Qualitative assessment of code
•   “Manual” static analysis by others (person or AI?)
•   Review: informal
•   Inspection: formal (ex. procedure + check list)
•   Git tools has good support for code reviews (ex. Merge Request)
•   Focus on: Correctness / Robustness / Readability / Security
•   Conformance to design requirements (Patterns, data models etc.)
•   Examples:
    • https://en.wikipedia.org/wiki/Fagan_inspection
    • ISO/IEC 20246 and industry specific standards (health, energy..)
     AARHUS                            SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                  MARCH 2026   STATIC ANALYSIS
     T ECH N IC AL SC IEN C ES

<!-- side 28 -->

COST OF FIXING DEFECTS




                                                                  DoD Developer ’s Guidebook for
                                                                  Software Assurance
  AARHUS                            SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                  MARCH 2026   STATIC ANALYSIS
  T ECH N IC AL SC IEN C ES

**Figur:** Søjlediagram med titlen «Defect Removal Time by Phase». Y-akse «Minutes», 0–1600 med gitterlinjer for hver 200. X-akse «Removal Phase» med seks kategorier og mørkeblå søjler med dataetiket over hver: Design Review 5, Design Inspect 22, Code Review 2, Code Inspect 25, Unit Test 32, System Test 1405. De fem første søjler er så lave, at de næsten flugter med aksen, mens System Test-søjlen når helt op omkring 1400 — omkostningen ved at finde fejlen sent er ca. 40-280 gange højere end i review- og inspektionsfaserne. Kilde anført: DoD Developer's Guidebook for Software Assurance.

<!-- side 29 -->

AARHUS
UNIVERSITY

