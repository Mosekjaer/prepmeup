---
title: "IntroductionHandinTwo"
source: "IntroductionHandinTwo.pdf"
modul: "Lektion 06.1-07.2: Obligatorisk Handin 2"
pages: 14
type: "slides"
vision: "ingen-grafik"
---
# IntroductionHandinTwo

<!-- side 1 -->

INTRODUCTION TO
MOBILE CHARGING STATION
HAND-IN

AARHUS                               SWT    PETER HØGH MIKKELSEN
UNIVERSITY                  FEBRUARY 2026   HAND-IN TWO
T ECH N IC AL SC IEN C ES

<!-- side 2 -->

MOBILE CHARGING STATION – FOR ONE
DEVICE
                              Door


                                                                                                Display




                                                                                                 RFID
                                                                                                reader


                                     USBCharger                                          Lock

  AARHUS                                                   SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                      FEBRUARY 2026   HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 3 -->

USE CASE DIAGRAM


                              User
                                                Load and Remove




                                                    << uses >>
                             RFID tag



                                                  Charge Device




                             Device




 AARHUS                                          SWT     PETER HØGH MIKKELSEN
 UNIVERSITY                             FEBRUARY 2026    HAND-IN TWO
 T ECH N IC AL SC IEN C ES

<!-- side 4 -->

DESIGN SKETCH
                                          Door Open/Close                                             RFID Detected
                                               Events                                                     Event


                               Door     Door Lock and Unlock           StationControl                                            RfidReader




                                           Station Messages                             Log entries


                                                                 Charger Commands




                                         Charging Messages
                              Display                                  ChargeControl                                  Log File




                                                         USB Commands                          Current Event




                                                                        USBCharger
  AARHUS                                                                SWT     PETER HØGH MIKKELSEN
  UNIVERSITY                                                   FEBRUARY 2026    HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 5 -->

BOUNDARIES OF THE SYSTEM
                                                 Door Open/Close                                              RFID Detected
                                                      Events                                                      Event


                                                                                                                                                      Reads
                                      Door     Door Lock and Unlock       StationControl                                                 RfidReader


                                                                                                                                                              RFID tag
                                                  Station Messages                              Log entries


                                                                      Charger Commands

                              User
                                                Charging Messages
                                     Display                               ChargeControl                                      Log File




                                                                USB Commands                           Current Event




                                                                           USBCharger




                                                                                  Charges and
                                                                                   Measures
                                                                             Device
  AARHUS                                                            SWT                PETER HØGH MIKKELSEN
  UNIVERSITY                                               FEBRUARY 2026               HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 6 -->

HARDWARE AND BOUNDARIES
 Since we don't have the hardware – solutions for simulating boundary interaction must be present
   • For the display, use the console
   • For the door, USB charger and RFID reader, add methods
   • They must be tested

 Use these in the App, see proposal for a main loop
 See also proposal for USB Charger Simulator
 Interfaces to the internal of the system should be as if the hardware existed
 A GUI App is allowed, but will not give extra credit
 Other designs are allowed, but should be testable, tested and fulfill the same functionality. Ask
 first!



  AARHUS                                            SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026   HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 7 -->

EXERCISE PLAN
  Start:                      04.03.2026 14:15                 Introduction



  Deadline:                   20.03.2026 23:59                  Complete system + journal (md text)

  Peer feedback:              25.03.2026 23:59




  AARHUS                                            SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026   HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 8 -->

TIME ALLOTTED
  Time allotted for the exercise:
    • Most Lectures in study weeks 6, 7 and 8. See lecture plan on Brightspace
    • All normal preparation time for those lectures
    • There will be theoretical lectures in study weeks 7 & 8, but also time for assistance with the
      handin
  Support and resources
    • Some of us will be present during normal lecture times in the classrooms
    • Use the opportunity to have your design reviewed as early as possible
    • Feel free to send an e-mail with material to be reviewed

  Feedback:
    • Per-group feedback (written) on Brightspace
    • Summary/key points: Lesson 10.2 (at the latest).


  AARHUS                                             SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                                FEBRUARY 2026   HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 9 -->

HAND-IN
  On FeedbackFruits you must hand-in: Zipped release of your repository containing:

  ./README.md :
        • Table of participants (names, student numbers)
        • Link to GitLab repository
        • Journal as required in the specification and the Rubrics
  ./docs/README.md:
     • Documentation and diagram as described in the hand-in
  .gigitnore
  .gitlab-ci.yml
  Solution and projects (one subfolder per project)


 AARHUS                                               SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                  FEBRUARY 2026   HAND-IN TWO
 T ECH N IC AL SC IEN C ES

<!-- side 10 -->

MAIN POINTS
  Testable Design
  Unit Tests and Coverage
  Test Quality
  Collaboration in the team
  Continuous Integration
  Reflections/observations on what you learned




  AARHUS                                       SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                          FEBRUARY 2026   HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 11 -->

TIPS AND TRICKS
  Read the specification carefully and all the way through
  Use what you have learned in the lecture on Design for Testability
  Events are your friends – loose coupling
  If a thing is hard to test or control for testing – maybe you should encapsulate it in its own
  class
  The simulators for the hardware is not the same as a fake – the simulators must also be
  tested (except UsbChargerSimulator)

  Fork the hand-out at gitlab.au.dk to your own group repo and work from there.




  AARHUS                                            SWT    PETER HØGH MIKKELSEN
  UNIVERSITY                               FEBRUARY 2026   HAND-IN TWO
  T ECH N IC AL SC IEN C ES

<!-- side 12 -->

QUESTIONS?




 AARHUS                               SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                  FEBRUARY 2026   HAND-IN TWO
 T ECH N IC AL SC IEN C ES

<!-- side 13 -->

BATTERY CHARGER SIMULATOR
                                                                                                                         System.EventArgs




                                                         <<interface>>
                                                          IUsbCharger
                             + CurrentValueEvent : event EventHandler<CurrentEventArgs>
                             +/- CurrentValue : double << property>>
                             +/- Connected : bool <<property>>                                                           CurrentEventArgs

                             + StartCharge() : void                                                            + Current : double << property>>
                             + StopCharge() : void




                                                      UsbChargerSimulator
                                                                                                    Only extensions to
                                                                                                    the interface are
                                                                                                    shown
                                         + SimulateConnected(connected : bool) : void
                                         + SimulateOverload(overload : bool) : void




 AARHUS                                                                                  SWT    PETER HØGH MIKKELSEN
 UNIVERSITY                                                                     FEBRUARY 2026   HAND-IN TWO
 T ECH N IC AL SC IEN C ES

<!-- side 14 -->

AARHUS
UNIVERSITY

