---
title: "MicrowaveSeqExtensions"
source: "MicrowaveDiagramsPdf.zip/MicrowaveSeqExtensions.pdf"
modul: "11.1-12.2-obligatorisk-opgave-iii"
pages: 1
type: "diagram"
vision: "done"
---

# MicrowaveSeqExtensions

<!-- side 1 -->

PowerButton :               TimeButton :     Start-CancelButton
             : Door                                                                                       : UserInterface               : Display            : Light   : CookController                : Timer    : PowerTube               : Output
                                     Button                      Button              : Button



Opens Door
                                                                                                             Cooking
                                                    <<event>> OnDoorOpened()

                                                                                                                                                    Stop()

                                                                                                                                                                                           Stop()


                                                                                                                                                                                                      TurnOff()

                                                                                                                                                                                                                                LogLine()
                                                                                                                             Clear()

                                                                                                                                                                                          LogLine()


                                                                                                           DoorIsOpen




                                                                                                             Cooking

                      Presses Start-Cancel Button

                                                                                    <<event>> OnStartCancelPressed()

                                                                                                                                                    Stop()

                                                                                                                                                                                           Stop()


                                                                                                                                                                                                      TurnOff()

                                                                                                                             Clear()                                                                                            LogLine()

                                                                                                                                                                                          LogLine()


                                                                                                                            TurnOff()                                                                 LogLine()



                                                                                                              Ready

**Figur:** Sekvensdiagram for de to afbrydelses-scenarier (extensions) i Microwave Oven — begge starter i tilstanden `Cooking`. Samme livliner som hoveddiagrammet: aktør, `: Door`, `PowerButton : Button`, `TimeButton : Button`, `Start-CancelButton : Button`, `: UserInterface`, `: Display`, `: Light`, `: CookController`, `: Timer`, `: PowerTube`, `: Output`. De to scenarier er adskilt af en tyk vandret stiplet linje.

**Scenarie 1 — døren åbnes under madlavning** (starttilstand `Cooking`):

1. Aktør → `Door`: `Opens Door`
2. `Door` → `UserInterface`: `<<event>> OnDoorOpened()`
3. `UserInterface` → `CookController`: `Stop()`
4. `CookController` → `Timer`: `Stop()`
5. `CookController` → `PowerTube`: `TurnOff()`, derefter `PowerTube` → `Output`: `LogLine()`
6. `UserInterface` → `Display`: `Clear()`, derefter `Display` → `Output`: `LogLine()`

Sluttilstand: `DoorIsOpen`. Bemærk at lyset ikke slukkes — det skal netop være tændt, når døren står åben.

**Scenarie 2 — Start-Cancel trykkes under madlavning** (starttilstand `Cooking`):

1. Aktør → `Start-CancelButton`: `Presses Start-Cancel Button`
2. `Start-CancelButton` → `UserInterface`: `<<event>> OnStartCancelPressed()`
3. `UserInterface` → `CookController`: `Stop()`
4. `CookController` → `Timer`: `Stop()`
5. `CookController` → `PowerTube`: `TurnOff()`, derefter `PowerTube` → `Output`: `LogLine()`
6. `UserInterface` → `Display`: `Clear()`, derefter `Display` → `Output`: `LogLine()`
7. `UserInterface` → `Light`: `TurnOff()`, derefter `Light` → `Output`: `LogLine()`

Sluttilstand: `Ready`.

Forskellen mellem de to scenarier er udelukkende det sidste skridt: ved annullering slukkes lyset (`Light.TurnOff()`) og systemet går til `Ready`; ved døråbning forbliver lyset tændt og systemet går til `DoorIsOpen`.
