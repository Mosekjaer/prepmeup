---
title: "I4SWT Vintereksamensopgave 19-20 Endelig"
source: "csfiles/home_dir/Eksamensinformation/E19/I4SWT Vintereksamensopgave 19-20 Endelig.pdf"
modul: "Eksamensinformation"
pages: 7
type: "slides"
vision: "ingen-grafik"
---
# I4SWT Vintereksamensopgave 19-20 Endelig

<!-- side 1 -->

Indledning
Denne tekst definerer eksamensopgaven i faget I4SWT/ST4SWT, Software Test, for indeværende
eksamenstermin på diplomingeniørstudiet i Informations- og Kommunikationsteknologi hhv.
Sundhedsteknologi på Ingeniørhøjskolen Aarhus Universitet.

Eksamensopgaven ligger til grund for den mundtlige eksamen i faget. Med udgangspunkt i opgavens
besvarelse vil du blive eksamineret i fagets indhold.

Opgavens besvarelse (”afleveringen”) skal bestå af netop 2 dele:

    •   En journal i PDF-format, hvor du besvarer de spørgsmål der er givet i de enkelte delopgaver.

    •   En implementering i en Microsoft Visual Studio-solution, som indeholder implementeringen
        af det system og de tests, der efterspørges i opgaven.
Dit navn og studienummer skal fremgår i headeren eller footeren på hver side i din journal.

Det vil være en fordel, hvis du foretager en Build/Clean solution command inden du zipper din VS
solution, så fylder den mindre, også som ZIP fil.

Journalen afleveres som hoveddokument på Digital Eksamen og implementeringen skal samles og
afleveres i netop 1 samlet ZIP-fil som bilag på Digital Eksamen.

Det er nok at navngive filerne SWTJournal.pdf og SWTKode.zip, da Digital Eksamen sætter jeres
navn foran, når vi downloader dem.

Formålet med opgaven er at give dig mulighed for at demonstrere din kunnen og viden inden for
fagets læringsmål. Der lægges derfor vægt på at opgavens løsning demonstrerer din erhvervede viden
inden for fagets forskellige emner.

Opgaven er inddelt i et antal delopgaver, som bygger på hinanden. Det fremgår af hver enkelt
delopgave hvad der skal afleveres i journalen hhv. implementeringen af opgaven. Al implementering
skal finde sted i den Microsoft Visual Studio solution der afleveres.

Til hjælp for løsningen udleveres der et enkelt bilag, som findes på Digital Eksamen.

Bemærk:
   • Du må ikke anvende et offentligt tilgængeligt Git repository i forbindelse med løsningen af
     denne opgave. Det indebærer risikoen for at din opgavebesvarelse bliver synlig for andre
     deltagere i denne eksamen, hvilket ikke er tilladt.

    •   Du må ikke oprette et Jenkins job på den i undervisningen anvendte CI-server (eller nogen
        anden CI-server) i forbindelse med løsningen af denne opgave. Det indebærer risikoen for at
        din opgavebesvarelse bliver synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt.




Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19

<!-- side 2 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19
1 Simpel CD Afspiller
Denne opgave omhandler design, implementering og test af software til en CD afspiller, der er bygget
op af nogle elektroniske/mekaniske blokke, der styres af en indbygget computer, der programmeres i
C#.

En principskitse af enheden er vist herunder (det er et hardware blokdiagram i uformel notation):


                   Display



                                          Information
                                                          Computer                                  Styresignal                CD Tray


                                      Knaptryk
           User
                                                                             Styresignal



                  Knappanel




                                                        Status Information



                                                                                           CD Playback Drive      CD Data   Audio Circuits




                                        Figur 1: Principskitse af CD afspilleren


CD afspilleren er udstyret med et simpelt brugerinterface, som skal fungere på følgende velkendte
måde.

Der er 3 knapper:

                  Play/Pause, der igangsætter afspilningen fra den aktuelle position af CD’en hvis
                  afspilning ikke er i gang, og sætter afspilningen på pause, men lader CD drevet bevare
                  den aktuelle position.

                  Stop, der stopper afspilningen, hvis den er i gang, og lader CD drevet gå i neutral
                  position, som hvis en CD lige er blevet indsat. Dvs. den aktuelle position glemmes.

                  Eject, der åbner og lukker CD skuffen (Tray), alt efter den aktuelle position. Hvis CD
                  skuffen lukket, åbnes skuffen så en CD kan indsættes eller tages ud. Hvis afspilning
                  eller pause er i gang, stoppes denne før skuffen åbnes. Hvis skuffen er åben, lukkes den
                  igen, med eller uden CD i. Der sker ikke yderligere automatiske aktiviteter.

Der er ingen Skip knap, der kunne hoppe til næste track. Det er en meget simpel CD afspiller.

Der er et display:

                  Displayet kan være blankt, det kan vise diverse status informationer, og det kan vise et
                  tal, der angiver tracknummeret for det aktuelt afspillede track på CD’en.




                                                                                                                                             Side 2 af 7

<!-- side 3 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19
Et løseligt udkast til et design (ikke et fuldt formelt UML klassediagram) er givet nedenfor:



                                Information
            Display




                        3
                            Button Events
                                                                              Tray Commands
             Button                                 ControlCDPlayer                           TrayInterface




                                            Drive Commands            Drive Events




                                                     DriveInterface




                                                 Figur 2: Designskitse




                                                                                                         Side 3 af 7

<!-- side 4 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19
Systemets forventede opførsel er beskrevet nedenfor i nogle sekvensdiagrammer og et tilstandsdiagram
for klassen ControlCdPlayer.


                        Eject : Button   PlayPause : Button   Stop : Button                 display : Display                   : ControlCDPlayer                         tray : TrayIF        drive : DriveIF




  User presses Eject
                                                                                                                                      Ready
                                                                EjectPressed() << event >>

                                                                                                                                                      Open()




  User inserts CD and
                                                                                                                                    TrayOpen
  presses Eject

                                                                EjectPressed() << event >>

                                                                                                                                                      Close()




                                                                                                                                      Ready
  User presses
  Play/Pause
                                                                               PlayPausePressed() << event >>

                                                                                                                                                                       Start()




                                                                                                                                     Playing


                                                                                                                                                                NewCdTrack(1) << event >>

                                                                                                                ShowNumber(1)




                                                                              loop      [while more tracks]                     [while more tracks]       NewCdTrack(n) << event >>

                                                                                                                ShowNumber(n)




                                                                                                                                                                EndOfCd() << event >>


                                                                                                                 Write(”End”)




                                                                                                                                      Ready

  User can now press
  Eject and remove
  the CD, not shown




                                                   Figur 3 Sekvensdiagram for en normal fuld afspilning af en CD


På sekvensdiagrammet i Figur 3 set forløbet af en normal indsætning og afspilning af en CD. Her kan
ses nogle af de tiltænkte events og metodekald, der er imellem systemets klasser.

Det kan antages, at de synkrone kald til TrayIF og DriveIF først vil returnere, når den underliggende
hardware er færdig med sin aktivitet, dvs. fx TrayIF.Open() returnerer først, når CD skuffen (Tray) er
kørt helt ud og står stille.

Det kan antages, at DriveIF sender et NewCdTrack event med det aktuelle tracknummer, når
DriveIF.Start() er blevet kaldt, uanset om afspilningsdelen (CD Playback Drive) var i neutral eller pause
position, således at ControlCdPlayer ikke behøver at holde styr på dette imellem forskellige
Play/Pause/Stop aktioner.




                                                                                                                                                                                            Side 4 af 7

<!-- side 5 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19

                       Eject : Button   PlayPause : Button      Stop : Button                  :Display             : ControlCDPlayer                 : TrayIF      : DriveIF




  User presses Eject                                                                                                     Playing
                                                                  EjectPressed() << event >>

                                                                                                                                                 Stop()




                                                                                                                                        Open()



                                                                                                          Clear()




  User Can now                                                                                                         Tray Open
  remove the CD




                                                             Figur 4 Sekvensdiagram for Eject mens der spilles


På Figur 4 ses et eksempel på et forløb, der er anderledes end det normale afspilningsforløb, nemlig
hvis brugeren trykker på Eject knappen under afspilning.

Andre lignende forløb for tryk på Eject eller Stop knapperne i andre tilstande af afspilningssystemet,
kan udledes af nedenstående tilstandsdiagram.




                                                                                                                                                                 Side 5 af 7

<!-- side 6 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19


                                                                                     EjectPressed()
                                                                 /display.Clear()    /tray.Close(),                              tray Open
                                                                                display.Write(“Ready”)




                                                                                                               EjectPressed()
                                                                                                               /tray.Open(),
                                                                          Ready                                display.Clear()




                                                                                                   EndOfCd()
                                           PlayPausePressed()
                                                                      NewCdTrack(nr)         /display.Write(”End”)
                                              /drive.Start()
                                                                 /display.ShowNumber(nr)



                           StopPressed()/
                             drive.Stop(),                                                                           EjectPressed()
                      display.Write(”Stopped”)                                                                       /drive.Stop(),
                                                                         Playing
                                                                                                                      tray.Open(),
                                                                                                                     display.Clear()



                                                                    PlayPausePressed()
                                            PlayPausePressed()        /drive.Pause(),
                                               /drive.Start()     display.Write(”Pause”)
         StopPressed()/
           drive.Stop(),
    display.Write(“Stopped”)                                                                                         EjectPressed()
                                                                                                                     /drive.Stop(),
                                                                                                                      tray.Open(),
                                                                         Paused                                      display.Clear()




                                            Figur 5: Tilstandsdiagram for CD Afspilning


På Figur 5 ses et tilstandsdiagram med transitions med triggere og aktiviteter for ControlCdPlayer
klassen.

For at begrænse denne opgaves størrelse, er der ikke beskrevet fejlsituationer, fx hvis der ikke er
nogen CD i skuffen, eller hvis der opstår en læsefejl, osv. Det skal du heller ikke tage højde for i dit
design, implementation og test. Dette kunne nemt indføres ved at tilføje flere events for DriveIF og
flere transitions på tilstandsdiagrammet.

Så tilstandsdiagrammet kan betragtes som en komplet beskrivelse af systemets opførsel, i modsætning
til sekvensdiagrammerne, der kun viser udvalgte dele af denne.

På den næste side følger delopgave 1 til 9.




                                                                                                                             Side 6 af 7

<!-- side 7 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Vinter 2019-20
Prøve: I4SWT/ST4SWT
Dato: 2019-12-19
1.1   Delopgave 1
Vis i din journal et testbart design (UML klassediagram i korrekt notation) for softwaren til CD-
afspilleren, med udgangspunkt i designudkastet i Figur 2. Redegør ligeledes for, hvad du mener der
gør dit design testbart.
1.2   Delopgave 2
Gør implementationen af klassen ControlCdPlayer færdig, med udgangspunkt i dit designforslag,
sekvens- og tilstandsdiagrammerne ovenfor og det udleverede, delvise forslag til source code.

Et udkast til dele af implementeringen af klassen ControlCdPlayer er givet i filen
ControlCdPlayer.Handout.cs, som du er velkommen til at arbejde videre med, ligesom filen
NewCdTrackEventArg.Handout.cs kan bruges til inspiration for eventet NewCdTrack mellem
DriveIf og ControlCdPlayer.

OBS! Disse filer er udkast, og kan ikke umiddelbart compilere, før du har færdiggjort og tilpasset dem
til dit eget design og din øvrige kode.
1.3   Delopgave 3
Implementér nogle af de unit tests (mindst 3), du finder nødvendige for at teste klassen
ControlCdPlayer. Vis resultatet af kørslen af disse unit tests i din journal og redegør for
opbygningen af den test suite, der knytter sig til klassen. Hvilke slags test har du lavet?
1.4   Delopgave 4
Vis i din journal eksempler på, hvor du i dine unit tests har draget nytte af det testbare design, du
specificerede i Delopgave 1.
1.5   Delopgave 5
Brug coverage beregning til demonstrere, hvor langt du er kommet i Delopgave 3 med at
implementere et passende antal tests for din klasse. Vis resultatet af denne coverage beregning i din
journal, og redegør for hvilken type af coverage beregning du bruger, og hvilke konklusioner du kan
drage af den.
1.6   Delopgave 6
Vis i din journal eksempler fra dine unit test, hvor du bruger et isolation framework. Redegør
endvidere for fordele og ulemper ved brugen af et sådant framework.
1.7   Delopgave 7
Lad Visual Studio udføre beregning af nogle Software Quality Metrics. Vis disse i din journal. Med
speciel fokus på Cyclomatic Complexity og Maintenance Index, redegør for hvad de kan sige om din
kode, hvorfor de er som de er, og om de for lige netop din kode giver anledning til at lave nogle
ændringer.
1.8   Delopgave 8
Antag at systemets øvrige klasser er implementeret og unit testet. Vis i din journal et dependency-træ
til brug for integrationstests af systemet. Redegør endvidere for, hvordan du finder klassernes
indbyrdes afhængigheder.
1.9   Delopgave 9
Vis i din journal en plan for integrationstesten af dit system med udgangspunkt i dit dependency-træ
fra ovenstående delopgave. Redegør i din journal for, hvilke integrationstest-strategier du har anvendt
i udfærdigelsen af din testplan.




                                                                                               Side 7 af 7

