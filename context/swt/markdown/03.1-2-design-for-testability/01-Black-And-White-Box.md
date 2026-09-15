---
title: "Black And White Box"
source: "Black And White Box.pdf"
modul: "Lektion 03.1+2: Design For Testability"
pages: 10
type: "slides"
vision: "done"
---
# Black And White Box

<!-- side 1 -->

BLACK & WHITE BOX
TESTING


AARHUS                           SW4SWT     PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   BLACK AND WHITE BOX TESTING
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

BLACK AND WHITE




 AARHUS                           SW4SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   BLACK & WHITE BOX TESTING
 T ECH N IC AL SC IEN C ES

<!-- side 3 -->

COMPLETE TESTS VS. ROBUST TESTS

  If we know what is inside the unit, we can test it much better!
  If we can reach inside the unit, we can make more complete tests!

  VS

  If we base our tests on what is inside the unit, the tests will break when our
  code is changing/maintained!




  AARHUS                                 SW4SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                        FEBRUARY 2026   BLACK & WHITE BOX TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 4 -->

BLACK BOX TEST TECHNIQUES VS. WHITE
BOX TEST TECHNIQUES
                     Black Box Testing
                     • We treat the Unit we are testing as a ”black box”.
                     • We can give input to this box and observe output and compare to expected
                       values (from a specification or definition)
                     • We don’t use knowledge of the internal code structure or how the
                       function/method/algorithm is implemented in details



                     White Box Testing
                     • We CAN see ”inside the box” and design tests based on the code structure.
                     • These type tests are not really good unit tests!!! but can be useful for other
                       types of tests (an example on next slide)

 AARHUS                                                    SW4SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                                           FEBRUARY 2026   BLACK & WHITE BOX TESTING
 T ECH N IC AL SC IEN C ES

<!-- side 5 -->

ONE EXAMPLE OF A WHITE BOX TEST TECHNIQUE


                                  Branch testing – all code
Simple example! In general,       branches traversed
there may be many more            input a,b
branches in a
program/function – In which       if (a+b > 50) {
case, we need more test               print(”Large”)
cases to traverse all branches.   }
                                  if (a+b < 50) {
                                    print(”small”)
                                  }



    AARHUS                                     SW4SWT     PETER HØGH MIKKELSEN
    UNIVERSITY                            FEBRUARY 2026   BLACK & WHITE BOX TESTING
    T ECH N IC AL SC IEN C ES

**Figur:** Til højre et flowchart over branch testing-eksemplet, med nummererede kanter (1–8) for de otte kant-segmenter der skal gennemløbes. Struktur oppefra: afrundet boks "Read A and B" → kant 1 → beslutningsraute "A+B > 50". Fra rauten går kant 2 til højre til boksen "Print Large", som via kant 4 løber ned og ind i den elliptiske knude "End If"; kant 3 går fra rauten direkte ned (false-grenen) til samme "End If". Fra "End If" går kant 5 ned til den næste beslutningsraute "A+B < 50". Fra denne går kant 6 til højre til boksen "Print Small", som via kant 8 løber ned til den nederste ellipse "End If"; kant 7 er false-grenen direkte fra rauten ned til samme "End If". Nummereringen viser, at fuld branch coverage kræver testcases, der dækker både true- og false-grenen i begge if-sætninger.

<!-- side 6 -->

HOW (NOT!) TO TEST!




          The interface and external behavior are defined by the contract (requirements).
          Your test must guarantee conformance to the contract, not conformance to the code!
          Aim for black box testing!!
  AARHUS                                         SW4SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                FEBRUARY 2026   BLACK & WHITE BOX TESTING
  T ECH N IC AL SC IEN C ES

**Figur:** Tegneserie (Andy Glover, cartoontester.blogspot.com, 2010) i sort/hvid med overskriften "HOW TO PASS ALL YOUR TESTS — NO BUGS, NO COMPLAINTS, NO MORE RE-TESTING". Fire strichmandsfelter forbundet af pile, hvert med en person ved en computer: (1) WRITE CODE → (2) EXECUTE CODE → (3) WRITE TESTS FROM VIEWING THE EXECUTION → (4) EXECUTE TESTS → "Voila!". Over sidste felt er skrevet med rød håndskrift "Don't OFC!" — pointen er, at det er en anti-opskrift: at udlede tests af den observerede eksekvering tester koden mod sig selv i stedet for mod kontrakten.

<!-- side 7 -->

BLACK BOX UNIT TEST
                                                       Interact                             Observe
                                   Act                                                                      (External)
                                                                                                       State/Value based
                                                                                                               tests

                                                                                             As long as the interface to
                                         Unit Under                                          the UUT and the interfaces
                                                                                             used by the UUT remain
                                            Test                                             unchanged – then the
                                                                                             tests are safe!

                                                                                                       Interaction based
                              Observe                                                React                    tests


                                         Mock             Stub                  and/or Mock
  AARHUS                                             SW4SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                    FEBRUARY 2026   BLACK & WHITE BOX TESTING
  T ECH N IC AL SC IEN C ES

**Figur:** Diagram over black box unit test. I midten en stor sort boks med hvid tekst "Unit Under Test". Fra oven ind i boksen: en pil mærket "Act" (med hånd-ikon) og en pil mærket "Interact" (med hånd-ikon); ud fra toppen en stiplet pil op til et øje-symbol mærket "Observe". Denne øverste vej hører til det blå felt til højre: "(External) State/Value based tests".

<!-- side 8 -->

DESIGN VS. TEST
  To do proper unit testing
         • As "Black Box" as possible – Do NOT rely on internal code of the UUT
         • As independent as possible from other programmers/units
         • Unit tests should not depend on other unit tests, but be independent
         • Unit tests should be able to run in any order


  When designing code, consider how you will test it!!
         • This will improve encapsulation and make it easier to write proper tests




  AARHUS                                            SW4SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                   FEBRUARY 2026   BLACK & WHITE BOX TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

QUESTIONS?




 AARHUS                           SW4SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   BLACK & WHITE BOX TESTING
 T ECH N IC AL SC IEN C ES

<!-- side 10 -->

AARHUS
UNIVERSITY

