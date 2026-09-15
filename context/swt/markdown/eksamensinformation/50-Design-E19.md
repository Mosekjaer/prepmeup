---
title: "Design"
source: "csfiles/home_dir/Eksamensinformation/E19/Design.pdf"
modul: "Eksamensinformation"
pages: 1
type: "slides"
vision: "done"
---
# Design

<!-- side 1 -->

Display                                     IControlCdPlayer                      TrayInterface




IDisplay                                    ControlCdPlayer                       ITrayInterface


                                       1




           3

                                                                                 NewCdTrackEvent
IButton                     << event >> << 2 events >>         IDriveInterface
                                                                                      Args




                        3


               Button                        DriveInterface                         EventArgs

**Figur:** Klassediagram (UML) for CD-afspilleren. Bokse i tre niveauer.

