---
title: "MicrowaveSeqTotal"
source: "MicrowaveDiagramsPdf.zip/MicrowaveSeqTotal.pdf"
modul: "11.1-12.2-obligatorisk-opgave-iii"
pages: 1
type: "diagram"
vision: "done"
---

# MicrowaveSeqTotal

<!-- side 1 -->

PowerButton :              TimeButton :          Start-CancelButton
                        : Door                                                                                                    : UserInterface                : Display                      : Light    : CookController                    : Timer   : PowerTube               : Output
                                                         Button                     Button                   : Button



                                                                                                                                      Ready
       Opens Door
                                                                       <<event>> OnDoorOpened()

                                                                                                                                                                 TurnOn()

                                                                                                                                                                                                                                           LogLine()
       Closes Door
                                                                                                                                   DoorIsOpen
                                                                       << event>> OnDoorClosed()

                                                                                                                                                                 TurnOff()

                                                                                                                                                                                                                                           LogLine()
                                                                                                                                      Ready

loop                                                                                                                                     [until power set]
                     Press Power Button

                                                                                      <<event>> OnPowerPressed()

                                                                                                                                                ShowPower()

                                                                                                                                                                                                                              LogLine()

                                                                                                                                    SetPower




loop                              Press Time Button                                                                                       [until time set]

                                                                                                   <<event>> OnTimePressed()

                                                                                                                                                    ShowTime()

                                                                                                                                                                                                                              LogLine()

                                                                                                                                     SetTime



                                           Press Start-Cancel Button

                                                                                                            <<event>> OnStartCancelPressed()

                                                                                                                                                                 TurnOn()
                                                                                                                                                                                                                                           LogLine()
                                                                                                                                                                             StartCooking()
                                                                                                                                                                                                                               Start()

                                                                                                                                                                                                                                           TurnOn()
                                                                                                                                                                                                                                                                       LogLine()
                                                                                                                                     Cooking
                                                                                                                                                                 loop                                               [until time expired]

                                                                                                                                                                                                                    << event>> OnTimerTick()
                                                                                                                                                                                              ShowTime()
                                                                                                                                                                                                                      get TimeRemaining


                                                                                                                                                                                                                                   LogLine()

                                                                                                                                                                                                                  <<event>> OnTimerExpired()


                                                                                                                                                                                                                                           TurnOff()

                                                                                                                                                                                                                                                                       LogLine()
                                                                                                                                                                             CookingIsDone()




                                                                                                                                                      Clear()

                                                                                                                                                                                                                              LogLine()

                                                                                                                                                                 TurnOff()
                                                                                                                                                                                                                                           LogLine()

                                                                                                                                      Ready

**Figur:** Sekvensdiagram for det samlede hovedforløb i Microwave Oven. Livliner fra venstre mod højre: en aktør (strichmand, brugeren), `: Door`, `PowerButton : Button`, `TimeButton : Button`, `Start-CancelButton : Button`, `: UserInterface`, `: Display`, `: Light`, `: CookController`, `: Timer`, `: PowerTube`, `: Output`.

Tilstandsmarkeringer vises som afrundede bokse på `: UserInterface`-livlinen undervejs: `Ready`, `DoorIsOpen`, `Ready`, `SetPower`, `SetTime`, `Cooking` og til sidst `Ready`.

Beskedforløb i kronologisk orden:

1. Aktør → `Door`: `Opens Door`
2. `Door` → `UserInterface`: `<<event>> OnDoorOpened()`
3. `UserInterface` → `Light`: `TurnOn()`
4. `Light` → `Output`: `LogLine()` — tilstand: `DoorIsOpen`
5. Aktør → `Door`: `Closes Door`
6. `Door` → `UserInterface`: `<<event>> OnDoorClosed()`
7. `UserInterface` → `Light`: `TurnOff()`
8. `Light` → `Output`: `LogLine()` — tilstand: `Ready`

**loop `[until power set]`:**
9. Aktør → `PowerButton`: `Press Power Button`
10. `PowerButton` → `UserInterface`: `<<event>> OnPowerPressed()`
11. `UserInterface` → `Display`: `ShowPower()`
12. `Display` → `Output`: `LogLine()` — tilstand: `SetPower`

**loop `[until time set]`:**
13. Aktør → `TimeButton`: `Press Time Button`
14. `TimeButton` → `UserInterface`: `<<event>> OnTimePressed()`
15. `UserInterface` → `Display`: `ShowTime()`
16. `Display` → `Output`: `LogLine()` — tilstand: `SetTime`

17. Aktør → `Start-CancelButton`: `Press Start-Cancel Button`
18. `Start-CancelButton` → `UserInterface`: `<<event>> OnStartCancelPressed()`
19. `UserInterface` → `Light`: `TurnOn()`, derefter `Light` → `Output`: `LogLine()`
20. `UserInterface` → `CookController`: `StartCooking()`
21. `CookController` → `Timer`: `Start()`
22. `CookController` → `PowerTube`: `TurnOn()`, derefter `PowerTube` → `Output`: `LogLine()` — tilstand: `Cooking`

**loop `[until time expired]`** (indlejret, spænder over `Display`, `CookController` og `Timer`):
23. `Timer` → `CookController`: `<<event>> OnTimerTick()`
24. `CookController` → `Timer`: `get TimeRemaining` (stiplet returpil tilbage)
25. `CookController` → `Display`: `ShowTime()`
26. `Display` → `Output`: `LogLine()`

27. `Timer` → `CookController`: `<<event>> OnTimerExpired()`
28. `CookController` → `PowerTube`: `TurnOff()`, derefter `PowerTube` → `Output`: `LogLine()`
29. `CookController` → `UserInterface`: `CookingIsDone()`
30. `UserInterface` → `Display`: `Clear()`, derefter `Display` → `Output`: `LogLine()`
31. `UserInterface` → `Light`: `TurnOff()`, derefter `Light` → `Output`: `LogLine()` — tilstand: `Ready`

Bemærk mønstret: alle `Output`-kald er `LogLine()`, og `Output` modtager kun — den kalder aldrig videre. Knapper og dør kommunikerer udelukkende via events opad til `UserInterface`.
