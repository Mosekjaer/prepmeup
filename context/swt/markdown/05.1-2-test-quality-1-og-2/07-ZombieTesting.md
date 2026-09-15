---
title: "ZombieTesting"
source: "ZombieTesting.pdf"
modul: "Lektion 05.1+2: Test Quality 1 og 2"
pages: 14
type: "slides"
vision: "done"
---
# ZombieTesting

<!-- side 1 -->

WHAT TO TEST?
-A ZOMBIE GUIDE


AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   ZOMBIE TESTING
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

    Originally a guide line to select the right test cases
    Nowadays a guide line for Test Driven Development
    http://blog.wingman-sw.com/archives/677
AARHUS                                                     SWT        PETER HØGH MIKKELSEN
UNIVERSITY                                            FEBRUARY 2026   ZOMBIE TESTING
T ECH N IC AL SC IEN C ES

**Figur:** Billede fra James W Grennings plakat "TDD Guided by ZOMBIES" (copyright 2016). Til venstre et foto af en menneskemængde udklædt som zombier med den påskrevne tekst "Not these zombies!" på skrå hen over. Til højre et gitter, der forklarer akronymet: en venstre kolonne med de tre rækker Zero, One og Many, hvor det store blå felt ved siden af er mærket "Simple Scenarios" og "Simple Solutions" — de tre første bogstaver hører altså til de simple tilfælde. Nederst en række med tre lodret satte kolonner: "Boundaries Behaviors" (B), "Interfaces" (I) og "Exercise Exceptions" (E). Bemærk at plakatens udlægning af bogstaverne er lidt bredere end sliden bagefter: B står for både Boundaries og Behaviors, og E for både Exercise og Exceptional behavior.

<!-- side 3 -->

ZOMBIE
 Z - Zero
 O – One
 M – Many
 B – Boundaries
 I – Interfaces
 E – Exceptional Behavior




 AARHUS                               SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   ZOMBIE TESTING
 T ECH N IC AL SC IEN C ES

<!-- side 4 -->

Z - ZERO
  Zero input or zero output or zero actions:
    • Test on the correct state of a newly created object – ZERO calls
    • Test with the empty input for collection – ZERO elements
         • Make tests that should return the empty collection – ZERO elements




  AARHUS                                              SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                 FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 5 -->

O - ONE
  One input or One output or One action:
   • Test on the correct state of a newly created object after one call of each method
   • Test input with a collection with: – ONE element
         • Make tests that should return a collection with exactly: – ONE element




  AARHUS                                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                  FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 6 -->

M - MANY
  Many inputs or Many outputs or Many actions:
   • Test on the correct state of an object after several calls of a each method
   • Test on the correct state of an object after calls of several mixed methods
         • Test input with a collection with: – 2 or more elements
         • Make tests that should return a collection with: – 2 or more elements




  AARHUS                                               SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                  FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 7 -->

B - BOUNDARIES
  Use BVA (Boundary Value Analysis) for selecting the correct parameters for tests – both
  valid and invalid
  Use EP (Equivalence Partitions) to keep this to a workable level




  AARHUS                                          SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                             FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 8 -->

I - INTERFACES
  Test all methods
  Test all overloaded versions of methods
    • Different number of parameters
    • Different types of parameters
  Test all thrown exceptions
  Use Coverage to be sure
  Exercise all called interfaces (Interaction-based testing and Integration testing) to
  dependencies
  Test all events used from dependencies
  Test all events UUT provides




  AARHUS                                           SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                              FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

E – EXCEPTIONAL BEHAVIOR
  This is not just Exceptions as defined in the specifications
  This is about robustness
  What happens on faulty
         • Input?
             • (BVA)
         • Call sequence? (any dependencies on order?)
         • Dependencies (timeout, unexpected result, exception, …)?




  AARHUS                                             SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 10 -->

TEST DRIVEN DEVELOPMENT
                            TEST(CircularBuffer, is_empty_after_creation) {                   Write Test   ZERO – Instantiate buffer
                              CHECK_TRUE(CircularBuffer_IsEmpty(buffer)); }                                and check it’s empty

                                 CircularBuffer * CircularBuffer_Create(void) {                                                    Write App
                                   CircularBuffer * self = (CircularBuffer *)calloc(1, sizeof(CircularBuffer));
                                   return self; }

                                 bool CircularBuffer_IsEmpty(CircularBuffer * self) {
                                   return true; }

index                             TEST(CircularBuffer, is_not_empty_after_put) { Expand Test                  ONE –
                                    CircularBuffer_Put(buffer, 42);                                           Put one element and
                                    CHECK_FALSE(CircularBuffer_IsEmpty(buffer)); }                            check buffer not empty
 1
42
1
                                    bool CircularBuffer_IsEmpty(CircularBuffer * self) {                    Update App
                                    return self->index == self->outdex; }

                                    void CircularBuffer_Put(CircularBuffer * self, int value) {
                                      self->index++; }


     AARHUS                                                                SWT    PETER HØGH MIKKELSEN
     UNIVERSITY                                                   FEBRUARY 2026   ZOMBIE TESTING
     T ECH N IC AL SC IEN C ES

<!-- side 11 -->

GRENNING’S BLOG:
  “What have we accomplished so far with help from ZOMBIES?” (After Zero and One) “To
  the novice, “you are testing nothing!”. Sure enough, but I think I’ve accomplished a several
  important things:
  •The interface is nearly complete, and we can see where it is going. If it was inconvenient
  to use, we’d know already!
  •The the code is proving to be testable.
  •A lot of complier syntax has been tamed for our needs.
  •Several important boundary conditions have been captured in tests we are confident in.
  •I can devote less of my brain to those boundary cases as I define the rest of the behaviors
  for the CircularBuffer. The tests will tell me if my code stops following the behaviors
  defined in the test scenarios.
  •We have explored a specific mechanism that the CircularBuffer can use to report that it is
  empty or not empty. Saving the value has nothing to do with determining IsEmpty().”
                              http://blog.wingman-sw.com/tdd-guided-by-zombies
  AARHUS                                                SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                   FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 12 -->

TEST DRIVEN DEVELOPMENT
                                     TEST(CircularBuffer, put_get_is_fifo) {      Expand Test
           index                       CircularBuffer_Put(buffer, 41);
                                       CircularBuffer_Put(buffer, 42);                                         MANY -
  1 4243
 41                                    CircularBuffer_Put(buffer, 43);                                         Add several items
  1                                    LONGS_EQUAL(41, CircularBuffer_Get(buffer));                            to buffer and check
                                       LONGS_EQUAL(42, CircularBuffer_Get(buffer));
                                       LONGS_EQUAL(43, CircularBuffer_Get(buffer)); }
outdex
                                      void CircularBuffer_Put(CircularBuffer * self, int value) {                         Update App          _Get() must also be
                                        self->values[self->index] = value;                                                                    implemented…
                                        self->index++ }                                                                                       Increments outdex

                                        TEST(CircularBuffer, force_a_buffer_wraparound){                           Expand Test
   index                                CircularBuffer * buffer = CircularBuffer_Create(2);
                                        CircularBuffer_Put(buffer, 1);
                                        CircularBuffer_Put(buffer, 2);                                                               BOUNDARY -
      31 2                              CircularBuffer_Get(buffer);                                                                  Variable capacity,
       1                                CircularBuffer_Put(buffer, 3);                                                               check
                                        LONGS_EQUAL(2, CircularBuffer_Get(buffer));                                                  boundaries
      outdex                            LONGS_EQUAL(3, CircularBuffer_Get(buffer));
                                        CHECK_TRUE(CircularBuffer_IsEmpty(buffer));
                                        CircularBuffer_Destroy(buffer); }
         AARHUS                                                                  SWT    PETER HØGH MIKKELSEN
         UNIVERSITY                                                     FEBRUARY 2026   ZOMBIE TESTING
         T ECH N IC AL SC IEN C ES

<!-- side 13 -->

TEST DRIVEN DEVELOPMENT
                              void CircularBuffer_Put(CircularBuffer * self, int value) {                     Update App
                                self->count++;
                                self->values[self->index] = value;
                                self->index = nextIndex(self, self->index); }


                               TEST(CircularBuffer, put_to_full_fails) {                               Expand Test
                                 CircularBuffer * buffer = CircularBuffer_Create(1);
                                 CHECK_TRUE(CircularBuffer_Put(buffer, 1));                                           EXCEPTION -
                                 CHECK_FALSE(CircularBuffer_Put(buffer, 2));                                          Add too many items
                                 CircularBuffer_Destroy(buffer); }                                                    to buffer and check
                                                                                                                      return value
                               TEST(CircularBuffer, get_from_empty_returns_default_value) {
                                 LONGS_EQUAL(DEFAULT_VALUE, CircularBuffer_Get(buffer)); }

                                 bool CircularBuffer_Put(CircularBuffer * self, int value) {                         Update App
                                   if (CircularBuffer_IsFull(self))
                                     return false;
                                   self->count++;
                                   self->values[self->index] = value;
                                   self->index = nextIndex(self, self->index);
                                   return true; }
  AARHUS                                                                 SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                                    FEBRUARY 2026   ZOMBIE TESTING
  T ECH N IC AL SC IEN C ES

<!-- side 14 -->

AARHUS
UNIVERSITY

