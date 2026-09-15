---
title: "DoorControlExercise"
source: "csfiles/home_dir/FakesAndIsolation/DoorControlExercise.pdf"
modul: "Lektion 04.1: Test Types and Fake Types"
pages: 3
type: "dokument"
vision: "n/a"
---
# DoorControlExercise

<!-- side 1 -->

                                                                                                          DoorControlExercise.docx
                                                                                                                       2023-09-13



Fakes Exercise – Using stubs and mocks to test a Door Control System

In this exercise, you will create the core component of a Door Control system, which, by means of some sort of
identification, controls the opening and closing of a door. You will test the components of the system using fakes, both
stubs and mocks, as appropriate to ensure that the system works as advertised. Then – as always – you will set up a
repository and a pipeline which must run build and unit tests.


A Door Control system controls the opening and closing of a door. When a user requests that the door be opened, he
identifies himself with an ID (by entering a code, swiping a card, performing a retina scan, …). The system consults a
user validation mechanism to check if the user may be granted access. If so, the system notifies the user, opens the
door, and waits for it to close again before any more requests can be handled. The sequence diagram below depicts
the main scenario, Entry Granted:


                : DoorControl                          : Door                          : UserValidation        : EntryNotification




                 Door Closed


    RequestEntry(id)
                       ValidateEntryRequest(id)

                                                                                  OK

                                    Open()



                                                  NotifyEntryGranted(id)


                Door Opening

                                 DoorOpened()


                                    Close()



                 Door Closing


                                  DoorClosed()




                 Door Closed




                                                  Figure 1: Main scenario: “Entry Granted”




                                                                Page 1 of 3

<!-- side 2 -->

                                                                                                                    DoorControlExercise.docx
                                                                                                                                 2023-09-13



Furthermore, two exception scenarios have been identified: Entry Denied and Door Breached, the latter when the
door is opened without the prior consent of the system. The sequence diagram for each of these two exceptions is
shown below:


                : DoorControl                               : Door                     : UserValidation                  : EntryNotification




                 Door Closed



    RequestEntry(id)

                          ValidateEntryRequest(id)

                                                                             Not OK



                                                     NotifyEntryDenied(id)




                                      Figure 2: Sequence diagram for exception scenario “Entry denied”




                          : DoorControl                                : Door                             : Alarm




                           Door Closed

                                              DoorOpened()


                                                  Close()


                                          RaiseAlarm()




                         Door Breached




                                     Figure 3: Sequence diagram for exception scenario ”Door breached”




                                                                     Page 2 of 3

<!-- side 3 -->

                                                                                                     DoorControlExercise.docx
                                                                                                                  2023-09-13



Exercise 1:
Analyse the system to make sure you understand each of the given scenarios:
    • Draw a UML State Machine Diagram (STM) state chart for class DoorControl
    • Draw a class diagram for the complete system
    • Check and change the class diagram as necessary to make the design testable

Exercise 2:
Design, implement and test class DoorControl according to your testable design from the previous question, so that it
satisfies the sequence given in Figure 1. You should not implement any other classes than DoorControl - use fakes as
necessary to satisfy the dependencies of DoorControl.

NOTE: Think about what you want to test! What type of tests are typically important for control classes?

NOTE: It is important that you think about how you can test class DoorControl without exposing any class-internal
variables or properties only for the sake of testing. For example, it is tempting but wrong to make public the
variable or property currentState in DoorControl which holds the current state (DoorOpen, DoorClosed, …) in the
STM for DoorControl. If you do so, you are making a white-box test, and merely renaming the states will break your
test suite.

HINT: How to act on DoorControl with DoorOpened() and DoorClosed()? It is not the responsibility of a fake for
Door to act, fakes should be as simple as possible! Simply call DoorOpened() and DoorClosed() on UUT from you
test code.

Exercise 3:
If you did not already do so, place your solution under version control, publish it to GitLab and setup a pipeline, that
will build and run the tests.

Exercise 4:
Working in parallel as you prefer, extend your implementation and test of class DoorControl so that it can also handle
the exceptions given in Figure 2 and Figure 3. Again, test your solution using fakes. Share your results using GitLab and
check that your tests are running on in the pipeline.




                                                        Page 3 of 3

