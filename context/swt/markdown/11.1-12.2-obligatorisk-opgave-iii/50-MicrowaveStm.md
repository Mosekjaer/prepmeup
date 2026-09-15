---
title: "MicrowaveStm"
source: "MicrowaveDiagramsPdf.zip/MicrowaveStm.pdf"
modul: "11.1-12.2-obligatorisk-opgave-iii"
pages: 1
type: "diagram"
vision: "done"
---

# MicrowaveStm

<!-- side 1 -->

DoorIsClosed/Turn Off Light


                                                                               Ready           DoorOpens/Turn On Light        Door is Open



                                                                                                Start-Cancel Button Pressed/
                                                                                                       Reset Values,
                                                               Press Power Button/Display Power         Clear Display
                                                                                                                      DoorIsOpened/
                                                                                                                      Reset Values,
                                                                                                                      Clear Display,
                                                   Press Power Button/                                                Turn On Light
                                                    Increase Power,          Set Power
                                                     Display Power

Start-Cancel Button Pressed/   Cooking Finished/
       Stop Cooking,            Reset Values,
       Reset Values,            Clear Display,                        TimeButtonPressed/Display Time
       Clear Display,            TurnOffLight
        TurnOffLight
                                                                                                                                 DoorIsOpened/
                                                                                                                                 Reset Values,
                                             TimeButtonPressed/                                                                  Clear Display,
                                               Increase Time,                 Set Time                                           Turn On Light
                                                Display Time
                                                                                                                                                  DoorIsOpened/
                                                                                                                                                  Stop Cooking,
                                                                                                                                                  Reset Values,
                                                       Start-Cancel Button Pressed/Start Cooking, Turn On Light                                    Clear Display




                                                                              Cooking

**Figur:** Tilstandsmaskine (UML state machine) for Microwave Oven med fem tilstande tegnet som afrundede rektangler: `Ready`, `Door is Open`, `Set Power`, `Set Time` og `Cooking`. Starttilstand er en udfyldt sort cirkel med pil ned i `Ready`. Der er ingen sluttilstand.

Transitioner, angivet som `trigger / action`:

- Start → `Ready` (initial transition, ingen trigger)
- `Ready` → `Door is Open`: `DoorOpens / Turn On Light`
- `Door is Open` → `Ready`: `DoorIsClosed / Turn Off Light`
- `Ready` → `Set Power`: `Press Power Button / Display Power`
- `Set Power` → `Set Power` (selvtransition): `Press Power Button / Increase Power, Display Power`
- `Set Power` → `Ready`: `Start-Cancel Button Pressed / Reset Values, Clear Display`
- `Set Power` → `Door is Open`: `DoorIsOpened / Reset Values, Clear Display, Turn On Light`
- `Set Power` → `Set Time`: `TimeButtonPressed / Display Time`
- `Set Time` → `Set Time` (selvtransition): `TimeButtonPressed / Increase Time, Display Time`
- `Set Time` → `Door is Open`: `DoorIsOpened / Reset Values, Clear Display, Turn On Light`
- `Set Time` → `Cooking`: `Start-Cancel Button Pressed / Start Cooking, Turn On Light`
- `Cooking` → `Ready`: `Start-Cancel Button Pressed / Stop Cooking, Reset Values, Clear Display, TurnOffLight`
- `Cooking` → `Ready`: `Cooking Finished / Reset Values, Clear Display, TurnOffLight`
- `Cooking` → `Door is Open`: `DoorIsOpened / Stop Cooking, Reset Values, Clear Display`

Hovedflowet går lodret ned ad midten: `Ready` → `Set Power` → `Set Time` → `Cooking`. `Door is Open` ligger til højre og kan nås fra alle fire øvrige tilstande; alle veje ud af den fører tilbage til `Ready`.
