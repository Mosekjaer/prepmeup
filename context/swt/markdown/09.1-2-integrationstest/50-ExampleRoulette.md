---
title: "ExampleRoulette"
source: "csfiles/home_dir/IntegrationTest/ExampleRoulette.pdf"
modul: "Lektion 09.1+2: Integrationstest"
pages: 2
type: "dokument"
vision: "n/a"
---
# ExampleRoulette

<!-- side 1 -->

I dette eksempel findes Dependency Tree til et Roulette Game.

Til dette brug ses nedenfor et klassediagram, der viser klasserne i den færdige design, og et sekvensdiagram, der vises
det typiske forløb.

Den foreslåede løsning ligger separat som en zip fil med en Visual Studio solution.




1    Class Diagram


                              UI
                                                Handles

                                                     RouletteGame
                                                      Exception

                                    1     Generates

                                            1                           1
                         RouletteGame                                              Roulette

                                                                   Uses
          1
                                                        Standard
      Output                                          FieldFactory
                                                                            Generates
                                                                                         37               1



                                    *                                               Field             Randomizer



                              Bet




    OddEvenBet                FieldBet              ColorBet




                                                          Page 1 of 2

<!-- side 2 -->

2          Sequence Diagram

                       UI                                                                                                        FieldFactory

                                                                                                                       Randomizer

                                               Roulette(FieldFactory, Randomizer)                Roulette

                                                                                        Output                  CreateFields()
                                                                                                                                                Field
              RouletteGame(Roulette, Output) RouletteGame


                               OpenBets()                                                                           Fields
    loop
                                                         Report(”Bets now open”)

       loop    DoBet                                                      Bet
                               Bet(Name, Amount, Bet)

                               PlaceBet(Bet)

                                CloseBets()
                                                        Report(”Bets now closed”)

                              SpinRoulette()
                                                            Report(”Spinning...”)
                                                                            Spin()                            Next()

                                                                          GetResult()
                                                                                                            Number
                                                                           WinningField
                                                                Report(”Result: ...”)

                                 PayUp()
                                                                          GetResult()


                                                                      WinningField
                                                 WonAmount(WinningField)

                                                                   alt
                                                                                                     getColor()


                                                                                                        color

                                                                                                   getNumber()


                                                                                                      number

                                                                                                       Even


                                                                                                       parity



                                                 Report(”User just won 200$ on a ... bet”)




                                                                     Page 2 of 2

