---
title: "SWTSommer22ReBilag"
source: "SWTSommer22ReBilag.pdf"
modul: "Eksamensinformation"
pages: 1
type: "slides"
vision: "ingen-grafik"
---
# SWTSommer22ReBilag

<!-- side 1 -->

            : AfleverBog                              : BrugerInterface                                 : Bogscanner                             : BogDatabase                             : BødeModul   : LånerDatabase   : Printer




                                                   BogScannet(id : BogId)



                                                                              FindBog(id)




                                                                     { låner : CPR, låneDato : Dato }


                           udregn lånetid


                                                                                                UdregnBøde(lånetid)




                                                                                                        bøde : double


opt                                                                                                                       [bøde > 0]

                               OpkrævBøde(bøde)
                                                                             Medarbejderen bruger
                                                                             Betalingssystemet og giver besked
                                                                             via BrugerInterface om resultatet.


      alt                                                                                                                [bøde ikke betalt]

                                BødeIkkeBetalt()                                                              BødeIkkeBetalt(lånerId: CPR, bøde: double)



                                                                                                                           [bøde betalt]

                                  BødeBetalt()
                                                                                                                BødeBetalt(lånerId: CPR, bøde: double)




                                                                            BogAfleveret(id)



                                                                                                                          UdskrivKvittering(id : BogId, bøde: double, bødeStatus : bool)

