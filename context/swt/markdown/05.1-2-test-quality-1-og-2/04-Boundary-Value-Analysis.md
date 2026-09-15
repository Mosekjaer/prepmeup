---
title: "Boundary-Value-Analysis"
source: "Boundary-Value-Analysis.pdf"
modul: "Lektion 05.1+2: Test Quality 1 og 2"
pages: 15
type: "slides"
vision: "done"
---
# Boundary-Value-Analysis

<!-- side 1 -->

BOUNDARY VALUE
ANALYSIS
& EQUIVALENCE PARTITIONS

AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

BOUNDARY VALUE ANALYSIS
   • Boundary Value Analysis (BVA)
                 – A test design technique to identify the necessary and sufficient set of test
                   cases
                 – Identify input values at which the output(s) change either in value or
                   behavior or validity. These are boundary values
                 – Test input on and on either side of the boundary values




 AARHUS                                                      SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                         FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
 T ECH N IC AL SC IEN C ES

<!-- side 3 -->

BOUNDARY VALUE ANALYSIS
 Example:                         bool IsPositive(int number)                                                              𝑛𝑢𝑚𝑏𝑒𝑟 ∈ ℕ0
        • What test cases would you define?




                             -8   -7   -6   -5   -4   -3   -2   -1               1        2       3       4    5   6   7     8
                                                                       0




 AARHUS                                                                   SWT        PETER HØGH MIKKELSEN
 UNIVERSITY                                                      FEBRUARY 2026       BOUNDARY VALUE ANALYSIS
 T ECH N IC AL SC IEN C ES

<!-- side 4 -->

EQUIVALENCE PARTITIONS
  An equivalence partition (EP) is a set of inputs that result in the same
  output, category or behavior
  • An invalid output or undefined category is also
     an equivalence partition.
  • EPs are often limited by boundaries, if so do BVA first!
  • Test with at least one input value from each EP!
    • This is necessary!
  • Don’t test with all values of an EP, but only one or a few
    • This is sufficient!



  AARHUS                                    SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                       FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

**Figur:** Til højre et Venn-diagram med to overlappende cirkler: venstre cirkel "Culinary Vegetables" (indeholder billeder af radise og broccoli), højre cirkel "Botanical Fruits" (pære og appelsiner). I snitmængden ligger en tomat — eksempel på et input der tilhører to kategorier samtidig, dvs. den diskrete grænse mellem frugt og grønt er tvetydig.

<!-- side 5 -->

EQUIVALENCE PARTITIONS
  Example:                         bool IsPositive(int number)                                                              𝑛𝑢𝑚𝑏𝑒𝑟 ∈ ℕ0
         • What test cases would you define?


                                   EP1: (false)                                                     EP2: (true)

                              -8   -7   -6   -5   -4   -3   -2   -1               1        2       3       4    5   6   7     8
                                                                        0




  AARHUS                                                                   SWT        PETER HØGH MIKKELSEN
  UNIVERSITY                                                      FEBRUARY 2026       BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 6 -->

EQUIVALENCE PARTITIONS AND BVA
  Programming errors often occur at boundaries!
    • If (x >100)
       • Nummerical boundary: Should it be > or >=?

         • If(typeof(tomato).IsSubclassOf(typeof(Fruit)))
            • Discrete boundary: Did we check if its a vegetable too?

  In particular for nummerically defined partitions, EP must be complemented
  by BVA!
  For discrete sets, we can stick with just EP

  AARHUS                                         SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                            FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 7 -->

EPS AND BVA - EXAMPLE
     Write a method string Calendar.Month2Semester(uint month):
      • Given a month 1-12, return either ”Spring” or ”Fall”
      • Spring semester starts in FEB and ends in JUL
      • Fall semester starts in AUG and ends in JAN

     What are the EPs? Boundary values? Test cases?

    0                1           2     3     4     5     6     7          8          9          10      11    12    13    14       ...
                  JAN           FEB   MAR   APR   MAY   JUN   JUL      AUG         SEP         OCT      NOV   DEC

 Illegal           Fall                      Spring                                            Fall                      Illegal




    AARHUS                                                             SWT    PETER HØGH MIKKELSEN
    UNIVERSITY                                                FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
    T ECH N IC AL SC IEN C ES

<!-- side 8 -->

EPS AND BVA - SOLUTION
 Write a method string Calendar.Month2Semester(uint month):
  • Given a month 1-12, return either ”Spring” or ”Fall”
   • Spring semester starts in FEB and ends in JUL
   • Fall semester starts in AUG and ends in JAN

 What are the EPs? Boundary values? Test cases?

      0                 1      2     3     4     5     6       7             8         9          10     11    12    13    14       ...
                     JAN      FEB   MAR   APR   MAY   JUN    JUL            AUG      SEP         OCT     NOV   DEC

 Illegal             Fall                  Spring                                                 Fall                    Illegal




  AARHUS                                                             SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                                FEBRUARY 2026    BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

DYNAMIC AND COMBINED BOUNDARIES
 Example:           void Regulate()
        • The boundary can be set – which tests should be used

                             EPheater1                                                     EPheater2

                                            EPwindow1                                                  EPwindow2

                             EPcombined1                  EPcombined2                                  EPcombined3
                                                   -




                                    low-1   low   low+1 low+2               high-1 high high+1


 AARHUS                                                              SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                                 FEBRUARY 2026    BOUNDARY VALUE ANALYSIS
 T ECH N IC AL SC IEN C ES

<!-- side 10 -->

BVA – COMBINED, BUT DIFFERENT INPUTS

                                                                                                                 90
                                     0


                                 12
                                               1
                                                   2                                                  High

    -90                                                90
                                                            +15                    0      Level

                                                                                                      Low
                                                            -15                                                         ”Missile, 1 o’clock low”

                                                                                                                 -90
                              -180       179
                                                                                                            Elevation

                              Azimuth
  AARHUS                                                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                      FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

**Figur:** To grafiske visninger af et radarsystems inputrum set fra flyets perspektiv.

<!-- side 11 -->

EPS AND BVA – YOUR TURN
  Perform BVA on the system and decide what test data you will use for azimuth and elevation
  Inputs are integers
  What would be different if inputs were floating point with high resolution?


                                           0                                                               90

                                           12
                                                     1

                                                           2
                                                                                                    High

                              -90                                        90       0     Level


                                                                                                    Low




                                    -180       179                                                         -90

  AARHUS                                                          SWT     PETER HØGH MIKKELSEN
                                                                                                      Elevation
  UNIVERSITY                        Azimuth              FEBRUARY 2026    BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

**Figur:** Samme to figurer som side 10 (azimuth-cirkel med sektorerne 12/1/2 og akserne 0, 90, -90, -180/179; elevation-siden med High/Level/Low og akserne 90, 0, -90), men uden det røde målpunkt og uden talebobblen — opgavens udgangspunkt, hvor de studerende selv skal finde EP'er og grænseværdier.

<!-- side 12 -->

EPS AND BVA – YOUR TURN
                              Azimuth     -200     -180                         -15            0        +15                                +179   +200



                              Elevation              -100 -90                   -15            0        +15                  +90 +100



                                                                 0                                                                  90

                                                                 12
                                                                           1

                                                                                   2
                                                                                                                             High

                                             -90                                                   90       0      Level


                                                                                                                             Low




                                                          -180       179                                                            -90

  AARHUS                                                                                SWT        PETER HØGH MIKKELSEN
                                                                                                                               Elevation
  UNIVERSITY                                              Azimuth              FEBRUARY 2026       BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

**Figur:** Øverst to vandrette talakser med de foreslåede testværdier afsat over hver akse.

<!-- side 13 -->

DIMENSIONAL MULTIPLICATION
                                                               Azimuth

                                     -200 -180 …           0           …            …     +179 +180 200
                              -100
                              -90
                              …
                 Elevation
                              0
                              …
                              90
                              100


  For multidimensional input data, it is even more important that the number of test values is
  reduced.
  Because the number of values per dimension is multiplied with that of the other dimensions!

  AARHUS                                                SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                   FEBRUARY 2026    BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 14 -->

EP AND BVA
  BVA and EPs are black box test tools – we only consider input and expected
  output (from specification/documentation etc)
   • but uses general knowledge about how programs are constructed

  EPs reduce the number of tests through analysis
    • Helps us not to make too many tests!

  BVA helps to select those tests in a way that makes it more probable that
  errors are found – as there is often programming errors at the boundaries.
    • Helps us to make enough tests!


  AARHUS                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                      FEBRUARY 2026   BOUNDARY VALUE ANALYSIS
  T ECH N IC AL SC IEN C ES

<!-- side 15 -->

AARHUS
UNIVERSITY

