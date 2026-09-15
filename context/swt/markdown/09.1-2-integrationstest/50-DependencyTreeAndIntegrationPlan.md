---
title: "DependencyTreeAndIntegrationPlan"
source: "csfiles/home_dir/IntegrationTest/DependencyTreeAndIntegrationPlan.pdf"
modul: "Lektion 09.1+2: Integrationstest"
pages: 4
type: "slides"
vision: "done"
---
# DependencyTreeAndIntegrationPlan

<!-- side 1 -->

        Button                                         Door




                             User Interface




                         Cook Controller




Light            PowerTube                    Timer           Display




                                              Output

**Figur:** Afhængighedstræ (dependency tree) for mikrobølgeovnen, tegnet som bokse forbundet med streger. Hierarkiet oppefra og ned:

<!-- side 2 -->

            Button                                                     Door




                     3
                                                           3

        3                            User Interface                                   3




                                            2                      2
                                                                                          Bottom-Up
                     2       2


                                     Cook Controller




                                     1                         1                  1


                                                                              1
Light                    PowerTube                         Timer                          Display




                                                       F                          F
                                 F

                                                           Output

**Figur:** Samme afhængighedstræ som side 1, men annoteret med integrationsrækkefølge for **Bottom-Up** (etiketten Bottom-Up står til højre i billedet). Tallet ved hver kant angiver, i hvilket integrationstrin den forbindelse tages i brug; `F` betyder fake/stub, der aldrig erstattes.

<!-- side 3 -->

            Button                                                     Door




                     1
                                                           1

        1                            User Interface                                   1




                                            2                      2
                                                                                          Top-Down
                     2       3


                                     Cook Controller




                                     3                         3                  3


                                                                              3
Light                    PowerTube                         Timer                          Display




                                                       F                          F
                                 F

                                                           Output

**Figur:** Samme afhængighedstræ, annoteret med integrationsrækkefølge for **Top-Down** (etiketten Top-Down står til højre). Tallene er nu spejlvendt i forhold til side 2:

<!-- side 4 -->

Top down



         Button    Door      User        Light   Display   Cook         Power   Timer   Output
                             Interface                     Controller   Tube
 1       T         T         X           S       S         S
 2       T         T         X           X       X         X            S       S       S
 3       T         T         X           X       X         X            X       X/S     S
 Final   T         T         X           X       X         X            X       X       X




Bottom up – leaving out Output




         Button    Door      User        Light   Display   Cook         Power   Timer   Output
                             Interface                     Controller   Tube
 1                           S                   X         T            X       X       S
 2       S         S         T           X       X         X            X       X       S
 3       T         T         X           X       X         X            X       X       S
 Final   T         T         X           X       X         X            X       X       X

**Figur:** To integrationsplan-tabeller (den udtrukne tekst har flettet kolonneoverskrifterne). Kolonnerne er i begge tabeller: Button, Door, User Interface, Light, Display, Cook Controller, Power Tube, Timer, Output. Rækkerne er trin 1, 2, 3 og Final. Celleværdier: T = testdriver/test, X = rigtig (integreret) komponent, S = stub, tom = indgår ikke i trinnet.

