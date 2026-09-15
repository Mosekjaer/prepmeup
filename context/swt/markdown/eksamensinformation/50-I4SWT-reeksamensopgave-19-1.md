---
title: "I4SWT reeksamensopgave 19-1"
source: "csfiles/home_dir/Eksamensinformation/I4SWT reeksamensopgave 19-1.pdf"
modul: "Eksamensinformation"
pages: 8
type: "slides"
vision: "ingen-grafik"
---
# I4SWT reeksamensopgave 19-1

<!-- side 1 -->

EKSAMEN
Kursus:                      I4SWT/ST4SWT – Software Test - Hjemmeeksamen


Eksamensdato:                2019-08-08 kl. 9:00


Varighed:                    24 timer


Underviser:                  Frank B. Jakobsen


Eksamenstermin:              Sommer 2019 Reeksamen




Praktiske informationer:

Digital eksamen
Opgaven tilgås og afleveres gennem den digitale eksamensportal.

Opgavebesvarelsen skal afleveres i PDF og ZIP-format

Husk at uploade og aflevere i Digital eksamen. Du vil modtage en elektronisk afleveringskvittering,
straks du har afleveret.

Husk at aflevere til tiden, da der ellers skal indsendes dispensationsansøgning.

Husk angivelse af navn og studienummer på alle sider, samt i dokumenttitel/filnavn.

Der er et enkelt bilag, som findes på Digital Eksamen

Hjælpemidler:
Alle hjælpemidler må benyttes, herunder internettet som opslagsværktøj, men opgaven er en
personlig opgave.




       Aarhus Universitet Ingeniørhøjskolen – IKT/ST
       Eksamenstermin: Reeksamen Sommer 2019
       Prøve: I4SWT/ST4SWT
       Dato: 2019-08-08

<!-- side 2 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Reeksamen Sommer 2019
Prøve: I4SWT/ST4SWT
Dato: 2019-08-08

Indledning
Denne tekst definerer eksamensopgaven i faget I4SWT/ST4SWT, Software Test, for
indeværende reeksamenstermin på diplomingeniørstudiet i Informations- og
Kommunikationsteknologi hhv. Sundhedsteknologi på Ingeniørhøjskolen Aarhus Universitet.

Eksamensopgaven ligger til grund for den mundtlige eksamen i faget. Med udgangspunkt i
opgavens besvarelse vil du blive eksamineret i fagets indhold.

Opgavens besvarelse (”afleveringen”) skal bestå af netop 2 dele:

    •   En journal i PDF-format, hvor du besvarer de spørgsmål der er givet i de enkelte
        delopgaver.

    •   En implementering i en Microsoft Visual Studio-solution, som indeholder
        implementeringen af det system og de tests, der efterspørges i opgaven.
Dit navn og studienummer skal fremgår i headeren eller footeren på hver side i din journal.

Det vil være en fordel, hvis du foretager en Build/Clean solution command inden du zipper din
VS solution, så fylder den mindre, også som ZIP fil.

Journalen afleveres som hoveddokument på Digital Eksamen og implementeringen skal samles
og afleveres i netop 1 samlet ZIP-fil som bilag på Digital Eksamen.

Det er nok at navngive filerne SWTJournal.pdf og SWTKode.zip, da Digital Eksamen sætter
jeres navn foran, når vi downloader dem.

Formålet med opgaven er at give dig mulighed for at demonstrere din kunnen og viden inden
for fagets læringsmål. Der lægges derfor vægt på at opgavens løsning demonstrerer din
erhvervede viden inden for fagets forskellige emner.

Opgaven er inddelt i et antal delopgaver, som bygger på hinanden. Det fremgår af hver enkelt
delopgave hvad der skal afleveres i journalen hhv. implementeringen af opgaven. Al
implementering skal finde sted i den Microsoft Visual Studio solution der afleveres.

Til hjælp for løsningen udleveres der et enkelt bilag, som findes på Digital Eksamen.

Bemærk:
   • Du må ikke anvende et offentligt tilgængeligt Git repository i forbindelse med løsningen
     af denne opgave. Det indebærer risikoen for at din opgavebesvarelse bliver synlig for
     andre deltagere i denne eksamen, hvilket ikke er tilladt.

    •   Du må ikke oprette et Jenkins job på den i undervisningen anvendte CI-server (eller
        nogen anden CI-server) i forbindelse med løsningen af denne opgave. Det indebærer
        risikoen for at din opgavebesvarelse bliver synlig for andre deltagere i denne eksamen,
        hvilket ikke er tilladt.




                                                                                        Page 2 of 8

<!-- side 3 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Reeksamen Sommer 2019
Prøve: I4SWT/ST4SWT
Dato: 2019-08-08

1 Selvforsynende og autonom enhed til styring af kameraer i arktiske
  områder
Denne opgave omhandler design, implementering og test af software til en enhed, der skal styre
et antal kameraer i den periode, hvor insekter tænkes at være aktive i de arktiske områder.
Styringen omfatter to overordnede opgaver: At tænde og slukke kameraerne og at styre
temperaturen for batteriet, kameraerne og enheden selv:

    •          Kameraerne er tændt i en sæson, der varer et par måneder hvert år (se senere). Når
               sæsonen begynder, tænder enheden for kameraerne, som så løbende tager billeder af
               insekterne. Selve fotograferingen er ikke del af denne opgave. Når sæsonen slutter, skal
               enheden slukke for kameraerne igen.

    •          Enheden (og dens batteri) er placeret udendørs i en isoleret kasse. Af hensyn til
               enheden, særligt dens batteri og det TFT-display, der er integreret i enheden, skal
               temperaturen holdes over et vist niveau hele året. Reguleringen af temperaturen foregår
               ved, at enheden tænder og slukker for et varmelegeme i kassen (se senere).

Bemærk, at den løbende opladning af enhedens batteri varetages af solceller. Denne del af
systemet er ikke en del af denne opgave.

En principskitse af enheden er vist herunder:



    Display
                                                                                       Styresignal                                   Kamera
                                                                                                                                  Kamera
                                                                                                                     Styret
                                      Information               Computer                             Kamerarelæ    Spænding
                                                                                                                               Kamera
                                                                                 Styresignal


                                                    Knaptryk




   Knappanel

                                                                                  Spænding

                                         Temperaturmåling         Spænding                            Varmerelæ




                                                                                  Spænding
                                                                                                         Styret
                                                                                                       Spænding




                    Temperaturføler                                 Batteri                          Varmelegeme




                                                                  Ladespænding



    Isoleret
     kasse




                                                                    Solceller




                                                               Figur 1: Principskitse af enheden




                                                                                                                              Page 3 of 8

<!-- side 4 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Reeksamen Sommer 2019
Prøve: I4SWT/ST4SWT
Dato: 2019-08-08
Måden, som enheden skal opfylde sine to overordnede opgaver på, er således:

Temperaturstyring:
   1. Temperaturen i kassen måles løbende.
   2. Hvis temperaturen kommer under 0 °C tænder enheden for et varmelegeme gennem et
      relæ.
   3. Først hvis temperaturen er over +2 °C igen, slukker enheden for varmelegemet.
   4. Når varmen tændes eller slukkes, skal begivenheden logges til en fil inkl. timestamp
      (dato og klokkeslæt), den aktuelle temperatur og en beskrivende tekst for begivenheden.
      Af hensyn til pladsforbruget på computeren, skal der selvfølgelig kun logges, når der
      sker en nødvendig begivenhed.

Kamerastyring:
   1. Kameraerne tændes den 15. maj hvert år.
   2. Kameraerne slukkes igen den 31. juli hvert år
   3. Når kameraerne tændes eller slukkes, skal begivenheden logges til en fil inkl. timestamp
      (dato og klokkeslæt) samt en beskrivende tekst for begivenheden.

Enheden er udstyret med et simpelt brugerinterface, som skal fungere som følger:

Når en ansvarlig medarbejder tilser enheden, skal han/hun ved tryk på en knap kunne udlæse
passende information om systemets status på displayet. På displayet vises der en udskrift
indeholdende mindst følgende information:

    1. Hvor mange dage, der er tilbage af optageperioden (dvs. frem til 31. juli) som 0 eller
       positivt tal. Der vises et -1 (minus et), hvis det nuværende tidspunkt er uden for
       optageperioden.
    2. Den aktuelle temperatur i kassen.
    3. Den aktuelle tilstand af temperaturstyringen: Varme tændt eller slukket

Yderligere detaljer for brugerinterfacet er ikke en del af denne opgave.

Et løseligt udkast til et design (ikke et UML klassediagram) er givet nedenfor:




                                                                                     Page 4 of 8

<!-- side 5 -->

         Aarhus Universitet Ingeniørhøjskolen – IKT/ST
         Eksamenstermin: Reeksamen Sommer 2019
         Prøve: I4SWT/ST4SWT
         Dato: 2019-08-08



                                                                                                                         On/Off Commands
                                           GetDaysRemaining                              Kamerastyring                                                   Kamerarelæ

                                                                                                                             Log entries




                    Brugerinterface                                                                         GetTemperature


                                                                                         IsHeatOn
                                                                                                                      TemperatureChanged
                                                                                                                            Events

                          RequestStatus
                                                                                      Temperaturstyring                                              Temperaturmåler




                                                                                                             Log entries

                                                                               On/Off Commands




                                                                                           Varmerelæ                                          Log File




                                                                                   Figur 2: Designskitse




         TemperaturMaaler                   TemperaturStyring                           Varmerelæ


                                                                                                                                   Log File

                                                 Slukket



                     TemperatureChanged(Temp)



opt          [Temp < 0]                                             TurnOn()

                                                                                   Logentry(Time, action)



                                                 Tændt




      loop                        [Temp <= 2]

                     TemperatureChanged(Temp)




                                                                    TurnOff()

                                                                                   Logentry(Time, Action)



                                                 Slukket




                                                                Figur 3: Sekvensdiagram for Temperaturstyringen




                                                                                                                                                              Page 5 of 8

<!-- side 6 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Reeksamen Sommer 2019
Prøve: I4SWT/ST4SWT
Dato: 2019-08-08




                                                                                       /varmeRelæ.TurnOff()


                                                                                       Slukket




                                                                                                                                temperatureChanged [temp > 2]
         temperatureChanged[temp < 0]                                                                                           /varmeRelæ.TurnOff(), logEvent
         /varmeRelæ.TurnOn(), logEvent




                                                                                        Tændt




                                        Figur 4: Tilstandsdiagram for Temperaturstyringen




                            User Interface                       TemperaturMaaler                           TemperaturStyring         Kamerastyring


  User




                                             GetTemperature()



                                             temperature : int

                                                                    IsHeatOn()



                                                                 heatingIsOn : bool


                                                                                      GetDaysRemaing()




                                                                                      daysRemaining : int




                                              Figur 5 Sekvensdiagram for visning af status


På de næste sider følger delopgave 1 til 11.




                                                                                                                                                      Page 6 of 8

<!-- side 7 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Reeksamen Sommer 2019
Prøve: I4SWT/ST4SWT
Dato: 2019-08-08

1.1   Delopgave 1
Vis i din journal et testbart design (UML klassediagram) for softwaren til enheden, med
udgangspunkt i designudkastet i Figur 2. Redegør ligeledes for, hvad du mener der gør dit
design testbart.


1.2   Delopgave 2
Implementér klassen Temperaturstyring, med udgangspunkt i dit designforslag, sekvens- og
tilstandsdiagrammerne ovenfor og det udleverede, delvise forslag til source code.

Et løseligt udkast til dele af implementeringen af klassen TemperaturStyring er givet i filen
TemperaturStyring.Handout.cs.

1.3   Delopgave 3
Implementér de unit tests, du finder nødvendige for at teste klassen Temperaturstyring. Vis
resultatet kørslen af disse unit tests i din journal og redegør for opbygningen af den test suite,
der knytter sig til klassen.

1.4   Delopgave 4
Vis i din journal eksempler på, hvor du i dine unit tests har draget nytte af det testbare design,
du specificerede i Delopgave 1.

1.5   Delopgave 5
Vis i din journal eksempler på anvendelsen af en adfærdsbaseret test og en tilstandsbaseret test
og redegør for forskellen.

1.6   Delopgave 6
Vis i din journal eksempler fra din unit test, hvor du bruger et isolation framework. Redegør
endvidere for fordele og ulemper ved brugen af et sådant framework.

1.7   Delopgave 7
I forbindelse med unit test af klassen Temperaturstyring vil det være naturligt at gennemføre
en Boundary Value Analysis (BVA) for at finde interessante test cases. Redegør for, hvordan en
BVA kan hjælpe dig med at finde sådanne test cases, og vis resultatet af din gennemførte BVA
for temperaturstyringen. Redegør i din journal endvidere for, hvordan resultatet af din BVA
anvendes i din unit test af klassen.

1.8   Delopgave 8
Implementér og test de dele af dit design, der håndterer skrivning til en log-fil. Vis i din journal
hvordan du har båret dig ad med at teste dette, og dokumentér i din journal, hvilke overvejelser
dette giver anledning til.

1.9   Delopgave 9
Antag at systemets øvrige klasser er implementeret og unit testet. Vis i din journal et
dependency-træ til brug for integrationstests af systemet. Redegør endvidere for, hvordan du
finder klassernes indbyrdes afhængigheder.

                      Der er flere delopgaver på næste side.




                                                                                         Page 7 of 8

<!-- side 8 -->

Aarhus Universitet Ingeniørhøjskolen – IKT/ST
Eksamenstermin: Reeksamen Sommer 2019
Prøve: I4SWT/ST4SWT
Dato: 2019-08-08

1.10 Delopgave 10
Vis i din journal en plan for integrationstesten af dit system med udgangspunkt i dit
dependency-træ fra ovenstående delopgave. Redegør i din journal for, hvilken integrationstest-
strategi du har anvendt i udfærdigelsen af din testplan.

1.11 Delopgave 11
Vis i din journal en skitse af, hvorledes en Continuous Integration server (CI-server) kunne
anvendes i udviklingen af softwaren til enheden. Redegør endvidere for, hvilke opgaver du ville
lade en CI-server varetage, og hvilke fordele og ulemper du ser i brugen af en CI-server.




                                                                                    Page 8 of 8

