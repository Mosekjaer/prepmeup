---
title: "DependencyTree"
source: "csfiles/home_dir/Eksamensinformation/E19/DependencyTree.pdf"
modul: "Eksamensinformation"
pages: 1
type: "slides"
vision: "done"
---
# DependencyTree

<!-- side 1 -->

                                           Button




                                    ControlCdPlayer




                                    U

DriveInterface                                      TrayInterface   Display




                 U




                     NewCdTrackEventArgs

**Figur:** Dependency-diagram (afhængighedstræ) for en CD-afspiller, tegnet som seks rektangulære bokse forbundet med rette linjer. `ControlCdPlayer` er roden i midten. Fra `ControlCdPlayer` går en lodret linje op til `Button`, og fra `Button` løber en linje til højre, ned langs siden og tilbage ind i `ControlCdPlayer` — altså en tovejs- eller tilbagekaldsrelation mellem de to. Fra bunden af `ControlCdPlayer` udgår fire linjer i vifte ned til fjerde niveau: skråt til venstre `DriveInterface`, næsten lodret ned til `NewCdTrackEventArgs` (linjen er mærket `U`), skråt ned til `TrayInterface` og længst til højre `Display`. `DriveInterface` har desuden sin egen linje ned til `NewCdTrackEventArgs`, også mærket `U`, samt en linje fra sin venstre side, der løber ud til venstre, op langs kanten og tilbage ind i `ControlCdPlayer`. Mærket `U` markerer de afhængigheder, der går via event-argument-klassen `NewCdTrackEventArgs`, som både `ControlCdPlayer` og `DriveInterface` bruger. `TrayInterface` og `Display` er blade uden yderligere afhængigheder.

