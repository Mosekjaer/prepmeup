---
title: "ECSBeforeAndAfter"
source: "csfiles/home_dir/DesignForTestability/ECSBeforeAndAfter.pdf"
modul: "Lektion 03.1+2: Design For Testability"
pages: 4
type: "slides"
vision: "done"
note: "delt mellem flere moduler"
---
# ECSBeforeAndAfter

<!-- side 1 -->

                                      ECS

         - threshold : int
         - _tempSensor : TempSensor
         - _heater : Heater

         + ECS (int)
         + Regulate() : void
         + SetThreshold(int) : void
         + GetThreshold() : int
         + GetCurTemp() : int
         + RunSelfTest() : bool




         TempSensor                                   Heater


+ GetTemp() : int                           + TurnOn() : void
+ RunSelfTest() : bool                      + TurnOff() : void
                                            + RunSelfTest() : bool

**Figur:** UML-klassediagram, "før"-versionen (uden design for testability). Tre kasser: `ECS` øverst i midten, `TempSensor` nederst til venstre, `Heater` nederst til højre. Fra `ECS` udgår to heltrukne linjer med åben pilespids skråt ned til henholdsvis `TempSensor` og `Heater` — direkte associationer til de konkrete klasser. Der findes ingen interfaces, så `ECS` er hårdt bundet til de konkrete afhængigheder; konstruktoren `ECS(int)` tager kun tærskelværdien, hvilket betyder at `ECS` selv må instantiere sine afhængigheder. Det er præcis dét, der gør klassen umulig at isolere i en unit-test.

<!-- side 2 -->

                                                                                    ECS

                                                      - threshold : int
                                                      - _tempSensor : ITempSensor
                                                      - _heater : IHeater

                                                      + ECS (ITempSensor, Iheater, int)
                                                      + Regulate() : void
                                                      + SetThreshold(int) : void
                                                      + GetThreshold() : int
                                                      + GetCurTemp() : int
                                                      + RunSelfTest() : bool




                                 ITempSensor                                                                                 IHeater

                         + GetTemp() : int                                                                         + TurnOn() : void
                         + RunSelfTest() : bool                                                                    + TurnOff() : void
                                                                                                                   + RunSelfTest() : bool




         TempSensor                               FakeTempSensor                                    Heater                                      FakeHeater


+ GetTemp() : int                           + GetTemp() : int                             + TurnOn() : void                             + TurnOn() : void
+ RunSelfTest() : bool                      + RunSelfTest() : bool                        + TurnOff() : void                            + TurnOff() : void
                                                                                          + RunSelfTest() : bool                        + RunSelfTest() : bool

**Figur:** UML-klassediagram, "efter"-versionen (design for testability). Fire lag:
- Øverst `ECS`, hvis felter nu er `- _tempSensor : ITempSensor` og `- _heater : IHeater`, og hvis konstruktor er `ECS(ITempSensor, IHeater, int)` — dependency injection.
- Midterlag: `ITempSensor` (venstre) og `IHeater` (højre). Fra `ECS` går to heltrukne linjer med åben pilespids skråt ned til hvert interface (association/dependency).
- Nederste lag: `TempSensor` og `FakeTempSensor` under `ITempSensor`; `Heater` og `FakeHeater` under `IHeater`. Fra hver af de fire konkrete klasser går en **stiplet** linje op med **hul trekantpil** til det interface, den implementerer (realization).

<!-- side 3 -->

             : ECS                          : TempSensor   : Heater




Regulate()



                           GetTemp()



                             temp



                     alt      [ temp < threshold]

                                              TurnOn()



                              [else]

                                              TurnOff()

**Figur:** UML-sekvensdiagram for `Regulate()`. Tre livliner med stiplede lodrette linjer: `: ECS`, `: TempSensor`, `: Heater` (navnene understreget, som instanser).

<!-- side 4 -->

                : ECS                    : TempSensor     : Heater




RunSelfTest()



                        RunSelfTest()



                        tresult : bool

                                         RunSelfTest()




                                         hresult : bool

**Figur:** UML-sekvensdiagram for `RunSelfTest()`. Samme tre livliner: `: ECS`, `: TempSensor`, `: Heater`.

