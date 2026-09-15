---
title: "SWTSommer22Endelig"
source: "SWTSommer22Endelig.pdf"
modul: "Eksamensinformation"
pages: 10
type: "slides"
vision: "ingen-grafik"
---
# SWTSommer22Endelig

<!-- side 1 -->

Indledning
Denne tekst definerer eksamensopgaven i faget SW4SWT/ST4SWT, Software Test, for indeværende
eksamenstermin på diplomingeniørstudiet i Informations- og Kommunikationsteknologi hhv.
Sundhedsteknologi, på Institut for Elektronik og Computerteknologi..

Eksamensopgaven ligger til grund for den mundtlige eksamen i faget. Med udgangspunkt i opgavens
besvarelse vil du blive eksamineret i fagets indhold.

Opgavens besvarelse (”afleveringen”) skal bestå af netop 2 dele:

    •   En journal i PDF-format, hvor du besvarer de spørgsmål der er givet i de enkelte delopgaver.

    •   En implementering i en Microsoft Visual Studio-solution, som indeholder implementeringen
        af det system og de tests, der efterspørges i opgaven.
Dit navn og studienummer skal fremgå i headeren eller footeren på hver side i din journal.

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
og test skal finde sted i den Microsoft Visual Studio solution der afleveres af dig.

Der er ingen bilag til denne opgave.

Bemærk:
   • Du må ikke anvende et offentligt tilgængeligt Git repository i forbindelse med løsningen af
     denne opgave, fx på GitHub.com. Det indebærer risikoen for at din opgavebesvarelse bliver
     synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt. Det er tilladt at bruge et
     ”private” repository uden adgang for andre end dig.

    •   Du må ikke oprette et Jenkins job på den i undervisningen anvendte CI-server (eller nogen
        anden CI-server) i forbindelse med løsningen af denne opgave. Det indebærer risikoen for at
        din opgavebesvarelse bliver synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt.




Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Eksamen Sommer 2022
Prøve: SW4SWT/ST4SWT/
Dato: 2022-06-01

<!-- side 2 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
1 Parkeringssystem
Dette system styrer betaling for parkering i et parkeringsanlæg ved hjælp af aflæsning af bilernes
nummerplader og betaling med betalingskort.

Opgaven består at programmere og teste de dele af dette system som angives nedenfor. Den nøjagtige
udformning af brugerinterfacet er ikke en del af opgaven, og der lægges op til at passende
boundaryklasser abstraherer disse dele bort, så det er registrering af biler og af betaling, der er
fokuspunkterne.

Her følger en simpel principskitse.


                                                                                 Intelligent
                                                                             Nummerpladelæser ved
                                                                                  indkørsel

                                                 Nummerplade
                                                                             Styring af indkørselsbom

                                                     Styresignaler

                                                                                 Intelligent
                                                        Nummerplade
                                                                             Nummerpladelæser ved
                         Styrecomputer                                            udkørsel
                                                   Tekst, Pris, Kortnummer
                                                                               Intelligent Kortlæser
                                                     Styresignaler
                         Internetkommunikation

                                                                             Styring af udkørselsbom


                  WiFi modul med forbindelse
                        til Internettet

                Figur 1: Principskitse af Parkeringssystemets mekaniske og elektroniske komponenter




                     Opgaven fortsætter på næste side.

                                                                                                        Side 2 af 10

<!-- side 3 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
De vigtigste Use Cases beskrives nedenfor:




                                             Indkørsel


    Nummerpladelæser ved indkørsel                                         Bom ved indkørsel




                                             Udkørsel
                                                                                Kortlæser


    Nummerpladelæser ved udkørsel



                                                                            Betalingsserver




                                                                           Bom ved udkørsel


                                        Figur 2 Use Case diagram for systemet

Her følger fully dressed versioner af de to Use Cases:

 Navn:                               Indkørsel
 Mål                                 At køretøjet er kommet ind i parkeringsanlægget
 Initiering                          Nummerpladen på et køretøj er blevet registreret af Nummerplade-
                                     læseren ved indkørslen
 Aktører                             Primær: Nummerpladelæser ved indkørsel
                                     Sekundær: Bom ved indkørsel
                                     Offstage: Køretøj og fører
 Antal samtidige forekom-            En
 ster
 Prækondition                        Systemet er operationelt, og der er plads i anlægget
 Postkondition                       Køretøjet er blevet lukket ind i parkeringsanlægget og dets num-
                                     merplade er blevet registreret.
 Hovedscenarie                           1. Nummerpladelæseren har registreret en nummerplade (evt.
                                             ved hjælp af føreren) og sender nummerpladens tekst til sy-
                                             stemet.
                                         2. Systemet gemmer nummerpladetekst og ankomsttidspunkt
                                         3. Systemet sender besked til Bom ved Indkørsel, at den skal
                                             lukke ét køretøj ind.
 Udvidelser/undtagelser                  Ingen, disse behandles af de eksterne aktører




                          Opgaven fortsætter på næste side.
                                                                                               Side 3 af 10

<!-- side 4 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
 Navn:                           Udkørsel
 Mål                             At parkeringen er blevet betalt og køretøjet er kommet ud af parke-
                                 ringsanlægget
 Initiering                      Nummerpladen på et køretøj er blevet registreret af Nummerplade-
                                 læseren ved udkørslen
 Aktører                         Primær: Nummerpladelæser ved udkørsel
                                 Sekundær: Kortlæser, Betalingsserver, Bom ved udkørsel
                                 Offstage: Køretøj og fører
 Antal samtidige forekom-        En
 ster
 Prækondition                    Systemet er operationelt, og køretøjet er blevet registreret ved ind-
                                 kørslen
 Postkondition                   At parkeringen er blevet betalt og køretøjet er kommet ud af parke-
                                 ringsanlægget
 Hovedscenarie                       1. Nummerpladelæseren har registreret en nummerplade (evt.
                                         ved hjælp af føreren) og sender nummerpladens tekst til sy-
                                         stemet.
                                     2. Systemet finder nummerpladen og ankomsttidspunkt i de
                                         gemte indkørselshændelser.
                                     [Extention 1: Nummerpladen findes ikke blandt de registrerede
                                     indkørte køretøjer]
                                     3. Systemet udregner den skyldige parkeringspris
                                     4. Systemet sender prisen til Kortlæseren og afventer at et kor-
                                         rekt kort er blevet registreret af denne
                                     5. Kortlæseren har registreret et korrekt betalingskort og sender
                                         dets kortnummer til Systemet.
                                     6. Systemet sender anmodning om debitering på kortet til Beta-
                                         lingsserveren.
                                     7. Systemet modtager godkendelse af betalingen
                                     8. Systemet sender besked til Bom ved Udkørsel, at den skal
                                         lukke ét køretøj ud.
 Udvidelser/undtagelser              Mange exceptions/extensions er blevet behandlet af de eksterne
                                     aktører, så der er ikke så mange at behandle i denne use case.
                                     Her er kun medtaget 1 exception, som dette system har direkte
                                     indflydelse på og som ikke kunne have været klaret af de eks-
                                     terne intelligente aktører.

                                 [Extention 1: Nummerpladen findes ikke blandt de registrerede ind-
                                 kørte køretøjer]
                                                1. Prisen sættes til den højeste pris ud fra pristabel-
                                                   len.
                                                2. UC fortsætter fra punkt 4.

Der skal ikke tages hensyn til yderligere fejlscenarier eller extensions, f.eks. fejl fundet ved
indbyggede diagnosekredsløb, el. lign. Der skal ikke implementeres et brugerinterface.

Et løseligt udkast til et softwaredesign (ikke et fuldt formelt UML diagram) er givet nedenfor. Mulige
klasser og deres relationer og interaktioner er skitseret. Forbindelserne symboliseret med et lyn-
symbol tænkes implementeret med C# events eller en anden implementation af Observer pattern.
Dette er et oplæg, og andre forbindelser er tilladte, når blot de beskrives, og implementeres og testes,
hvor det er relevant.


                    Opgaven fortsætter på næste side.
                                                                                              Side 4 af 10

<!-- side 5 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01




                                                     Nummerpladelæser                       Bomstyring




                                   NummerpladeLæst




                                                         Indkørsel


                                                                                         Beregningsmodul

              Parkeringsliste


                                                         Udkørsel

                                                                                            Kortlæser




                                      NummerpladeLæst
                                                                                         Betalingsserver




                                                     Nummerpladelæser                      Bomstyring




                                Figur 3: Designskitse for Parkeringssystemets software

De foreslåede klasser er som følger:

Indkørsel er controlklassen for use casen Indkørsel. Klassens opgaver fremgår af ovenstående
beskrivelse af use casen, og af nedenstående sekvensdiagram.

Udkørsel er controlklassen for use casen Udkørsel. Klassens opgaver fremgår af ovenstående
beskrivelse af use casen, og af nedenstående sekvensdiagram, og består bl.a. af udførsel af betalingen
via Betalingsserveren.

Nummerpladelæser er en boundaryklasse, der står for interfacet til et eksternt intelligent system, der
kan aflæse en nummerplade ved hjælp af et kamera og kan bede føreren af bilen om at indtaste
nummeret, hvis det ikke kan bestemmes ud fra billedet. De to nummerpladelæsere er ens og har
samme interface, så der er to instanser af den samme klasse. De kan give et event, når en ny
nummerplade er aflæst.

Bomstyring er en boundaryklasse, der står for styringen af en bom. De to bomme fungerer på samme
måde, så der er to instanser af den samme klasse.



                      Opgaven fortsætter på næste side.
                                                                                                           Side 5 af 10

<!-- side 6 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
Parkeringsliste er en hjælpeklasse, der kan gemme ankomsttidspunktet knyttet til en bestemt
nummerplade og slå det op, når bilen med den samme nummerplade skal køre ud, og returnere det.
Den kan gemme data for alle biler, der kan stå i parkeringsanlægget, og om de er kørt ud og har betalt.
Nummerpladen repræsenteres med en tekst/string, da den kan indeholde både tal og bogstaver, også i
udenlandske kombinationer, ankomsttidspunktet kan registreres som time- og minuttal. Data for en
ankomst kan returneres som en simpel type – et Data Transfer Object – DTO, som er en
domæneklasse. Hvis nummerpladen ikke kan findes ved udkørsel, kan der returneres en null værdi,
eller metoden kan smide en exception, eller det kan angives som en property i DTO, efter dit
designvalg.

BeregningsModul er en hjælpeklasse, der ud fra de registrerede ankomst- og udkørselstider kan
beregne prisen for parkeringen ud fra nedenstående tabel og metode. Der antages at der regnes ud fra
time- og minuttal for ankomst og udkørsel, dato kan ignoreres, prisen beregnes ud fra nedenstående
beskrivelse.

Kortlæser er en boundaryklasse, der står for interfacet til et eksternt intelligent system, der kan vise et
skyldigt beløb og læse et betalingskort, samt give andre instruktioner til brugeren, indtil et korrekt
kortet er korrekt læst.

Betalingsserver er en boundaryklasse, der kan anvende systemets WiFi forbindelse til at
kommunikere med et eksternt betalingssystem, der kan debitere brugerens kort med den beregnede
pris. Ved denne anvendelse og disse beløbsstørrelser er det ikke nødvendigt med en PIN-kode.

Nedenfor følger en række sekvensdiagrammer, der illustrerer de forskellige Use Cases’ hovedscenarier
og exceptions.


                                                IndNPL :
        : Indkørsel                           Nummerplade-                              IndBS : Bomstyring     : ParkeringsListe
                                                  læser




                      IndkommendeNummerPlade(nr)

                                                           RegistrerIndkørsel(nr, time, minut)



                                                   ÅbenBom()




                                           Figur 4 Sekvensdiagram for use case Indkørsel




                               Opgaven fortsætter på næste side.




                                                                                                             Side 6 af 10

<!-- side 7 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01

                                           UdNPL :
             : Udkørsel                  Nummerplade-           UdBS : Bomstyring            : ParkeringsListe   : Beregningsmodul    : Kortlæser   : BetalingsServer
                                            læser




                      UdkørendeNummerPlade(nr)




      alt                   [ankomsttidspunkt registreret]




                          [ankomsttidspunkt ikke registreret]




                      pris sættes til maksimum pris



                                                                           AnmodKortBetaling(pris)



                                                                                kortnummer




                                                  Figur 5 Sekvensdiagram for use case Udkørsel




                                        Opgaven fortsætter på næste side.




                                                                                                                                     Side 7 af 10

<!-- side 8 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
Her følger reglerne og en pristabel for parkering:

Det antages at der ikke parkeres mere end 24 timer. Det kan ske, at perioden for parkeringen strækker
sig henover midnat (kl. 00:00), så udkørselstidspunktet ser ud som det er før ankomsttidspunktet.
Eksempelvis vil indkørsel kl 21:00 og udkørsel kl 01:00 give en varighed på 4 timer. Systemet skal
tage højde for denne situation.

Der anvendes et ur med tidspunkter der går fra 00:00 til 23:59, som kan hentes fra systemet.

Tabel over prisen for parkering:

 Parkeringsvarighed                 Pris
 (timer:minutter), værdierne
 er inklusive i intervallerne
 00:00 til 02:00                    0 kr.
 0 til 120 minutter
 02:01 til 03:00                    25 kr.
 121-180 minutter
 03:01 til 04:00                    50 kr.
 181-240 minutter
 Følgende yderligere varigheder     75 – 175 kr.
 lægger 25 kr. til per påbegyndt
 time/60 minutter op til og med
 09:00/540 minutter
 09:01-23:59                        200 kr.
 541-1439 minutter

200 kr. er altså den maksimale pris, som også trækkes, hvis ankomsttidspunktet ikke er blevet
registreret for den pågældende nummerplade.

På de næste sider følger delopgave 1 til 9.




                 Opgaven fortsætter på næste side.




                                                                                         Side 8 af 10

<!-- side 9 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
1.1     Delopgave 1
Vis i din journal et testbart design for softwaren til Parkeringssystemet som et eller flere UML klasse-
og sekvensdiagrammer i korrekt notation, med udgangspunkt i designudkastet i Figur 3. Redegør for,
hvad du mener der gør dit design testbart.

Du skal medtage al den ovenfor beskrevne funktionalitet, de nødvendige boundaryklasser og
interfaces, og de domæne- og hjælpeklasser du finder nødvendige i dit design, for at gøre det testbart.
Hvor der mangler detaljer, finder du selv på nogen, eller udelader dem, med en kommentar på
diagrammet eller i journalen. Det er tilladt med et design, der afviger fra skitsen i Figur 3, men det skal
implementere den beskrevne funktionalitet (som observerbart udenfor systemet) og være et modulært
og testbart design.

Det er tilladt at tilføje flere klasser hvis de nødvendige for designet og testbarheden.

Det er ikke nødvendigt med et multitrådet design for at implementere systemet, men det er tilladt, hvis
det fremgår af designet og den videre løsning af delopgaverne.
1.2     Delopgave 2
Du skal implementere klasserne Udkørsel og Beregningsmodul, med udgangspunkt i dit testbare
design, sekvensdiagrammer og specifikationer ovenfor, og antagelserne nedenfor. Du skal også
definere interfaces til afhængighederne, så klasserne kan testes. Alternativt kan du implementere
den/de klasser som i dit design udfører de ovenfor beskrevne funktioner for udkørslen.

Følgende antagelser kan bruges for at gøre implementationen simplere:
    • At åbne bommen for gennemkørsel er en synkron aktivitet, hvor der ventes på at bommen er
       lukket igen, inden boundaryklassens metode returnerer.
    • At anmode om kort til betaling er en synkron aktivitet, hvor der ventes på at Kortlæseren har
       afsluttet kommunikationen med brugeren og evt. Betalingsserveren, inden boundaryklassen
       returnerer.
    • At anmode om debitering for prisen er en synkron aktivitet, hvor der ventes på at
       Betalingsserveren har svaret tilbage, inden boundaryklassens metode returnerer.
    • Der vil ikke være nogen parkeringer, der varer 24 timer eller mere.

Hint:
   •     Konverter klokkeslettene i timer og minutter til minutter siden midnat, så det er nemmere at
         beregne parkeringstiden.
1.3     Delopgave 3
Implementér de unit tests, du finder nødvendige for at teste klasserne. Vis resultatet af kørslen af disse
unit tests i din journal og redegør for opbygningen af den test suite, der knytter sig til klasserne. Hvilke
slags test har du lavet?
1.4     Delopgave 4
Vis i din journal eksempler på, hvor og hvordan du i dine unit tests har draget nytte af det testbare
design, du specificerede i Delopgave 1.
1.5     Delopgave 5
Brug coverage beregning til at demonstrere, hvor langt du er kommet i Delopgave 3 med at
implementere et passende antal tests for dine klasser. Gør rede for, hvordan du har suppleret coverage
beregning for at finde de nødvendige og tilstrækkelige test cases.
1.6     Delopgave 6
Vis i din journal eksempler fra dine unit test, hvor du bruger eller kunne bruge et isolation framework.
Redegør endvidere for fordele og ulemper ved brugen af et sådant framework.

        Opgaven fortsætter på næste side.
                                                                                              Side 9 af 10

<!-- side 10 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22
Prøve: SW4SWT/ST4SWT
Dato: 2022-06-01
1.7   Delopgave 7
Hvilke elementer af Black Box testing og White Box testing er der i dine tests?
1.8   Delopgave 8
Antag at systemet til parkeringsanlægget skal udvides med to nye features. Antag endvidere at udvik-
lingen af disse features skal understøttes af et Git workflow, hvor forskellige udviklere kan arbejde pa-
rallelt på de to features. Redegør for hvordan et sådant workflow kan understøttes af Git. Hvad er en
fornuftig branching strategi og hvordan vil du vha. Git få integreret mellem branches?
1.9   Delopgave 9
I et rigtigt projekt bruger man ofte Continuous Integration. Beskriv med dine egne ord: Hvilke elemen-
ter indgår der i en opsætning af dette værktøj? Hvordan arbejder de sammen, og hvilke fordele giver
det at bruge det?




                                                                                          Side 10 af 10

