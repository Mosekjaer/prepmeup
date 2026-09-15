---
title: "SWTSommer22ReGodkendt"
source: "SWTSommer22ReGodkendt.pdf"
modul: "Eksamensinformation"
pages: 9
type: "slides"
vision: "ingen-grafik"
---
# SWTSommer22ReGodkendt

<!-- side 1 -->

Indledning
Denne tekst definerer eksamensopgaven i faget SW4SWT/ST4SWT, Software Test, for indeværende
eksamenstermin på diplomingeniørstudiet i Informations- og Kommunikationsteknologi hhv.
Sundhedsteknologi, på Institut for Elektronik og Computerteknologi.

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

Der er eet bilag, nemlig sekvensdiagrammet i Figur 4 som PDF fil. Der er ingen kodebilag.

Bemærk:
   • Du må ikke anvende et offentligt tilgængeligt Git repository i forbindelse med løsningen af
     denne opgave, fx på GitHub.com. Det indebærer risikoen for at din opgavebesvarelse bliver
     synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt. Det er tilladt at bruge et
     ”private” repository uden adgang for andre end dig.

    •   Du må ikke oprette et Jenkins job på den i undervisningen anvendte CI-server (eller nogen
        anden CI-server) i forbindelse med løsningen af denne opgave. Det indebærer risikoen for at
        din opgavebesvarelse bliver synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt.




Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Reeksamen Sommer 2022
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15

<!-- side 2 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
1 Bibliotekssystem
I denne opgave arbejdes der med dele af et Bibliotekssystem, der bl.a. administrerer bogbestanden,
lånere og udlån og tilbagelevering af bøger (og andet materiale).

Et Use Case Diagram kunne se ud som følger:



                                                                 BiblioteksSystem



                                                                       Check Bog Ud


    Låner                    Medarbejder
                                                                                                           BogDatabase




                                                                        Aflever Bog




     Betalingssystem


                                                                      Other Use Case                       LånerDatabase
                                                                          Other Use Case
                                                                              Other Use Case
                             BogScanner
                                                                                      Other Use Case




                                      Figur 1 Use Case diagram for Bibliotekssystem

De forskellige aktører forklares her:

Medarbejderen er den primære bruger af systemet, og den primære aktør for de fleste Use Cases.

Låneren vil kun være indirekte/off stage aktør gennem medarbejderen, og fx give bøger til
indscanning og aflevering til denne og få bøger, der er scannet ind til udlån af denne. Der er ikke
indført selvbetjening på dette bibliotek.

BogDatabasen og LånerDatasen er sekundære aktører, der indeholder den information, der er
nødvendig for at udføre de forskellige Use Cases kernefunktionalitet.

Use Casen Aflever Bog indeholder en funktion, der kan udregne en bøde for en for sent afleveret bog.
Denne bøde skal afregnes med et kasseapparat eller anden betalingsløsning, der ikke er den del af
denne opgave. Betalingssløsningen er derfor en indirekte/off stage aktør. Den er endnu ikke integreret
i bibliotekssystemet.

Opgaven består at programmere og teste de dele af dette system som angives nedenfor. Den nøjagtige
udformning af brugerinterfacet er ikke en del af opgaven, og der lægges op til at passende
boundaryklasser abstraherer disse dele bort, så det er registrering af aflevering, der er fokuspunkterne.


                       Opgaven fortsætter på næste side.
                                                                                                       Side 2 af 9

<!-- side 3 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
Her følger en simpel principskitse.



                                                           BogScanner




                                                                                  BogDatabase




                                        Medarbejderens
                                             PC

                      Printer
                                                                                 LånerDatabase


                Figur 2: Principskitse af Bibliotekssystemets mekaniske og elektroniske komponenter




                  Opgaven fortsætter på næste side.




                                                                                                      Side 3 af 9

<!-- side 4 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
Her følger en fully dressed version af Use Casen Aflever Bog:

 Navn:                           Aflever bog
 Mål                             Låneren har fået afleveret sin bog
 Initiering                      Denne Use Case er valgt på Brugerinterfacet og en bog scannes på
                                 Bogscanneren
 Aktører                         Primær: Bogscanner og Medarbejder
                                 Sekundær: Bogdatabase og LånerDatabase
                                 Offstage: Låner og Betalingssystem
 Antal samtidige forekom-        En
 ster
 Prækondition                    Denne Use Case er valgt på Brugerinterfacet
 Postkondition                   Bogen er registreret som afleveret, og status for en evt. bøde er re-
                                 gistreret.
 Hovedscenarie                        1. Bogscanneren registrerer et id på en bog
                                      2. Systemet slår bogen op i Bogdatabasen og finder låneren og
                                          den dato, hvor bogen er lånt (checket ud)
                                      3. Lånetiden beregnes
                                      4. En bøde beregnes ud fra lånetiden
                                      [Extention 1: Den normale lånetid er ikke overskredet]
                                      5. Systemet viser den skyldige bøde
                                      6. Medarbejderen modtager den opkrævede bøde ved hjælp af
                                          betalingssystemet.
                                      [Extention 2: Låneren (eller den person som afleverer bogen for
                                      låneren) kan ikke betale]
                                      7. Medarbejderen registrerer i Bibliotekssystemet, at bøden er
                                          betalt.
                                      8. Status for bøden registreres i LånerDatabasen
                                      9. Bogen registreres som afleveret i Bogdatabasen
                                      10. Der udskrives en kvitteringsbon for modtagelse af bogen,
                                          der også indeholder information om en evt. betalt eller skyl-
                                          dig bøde
 Udvidelser/undtagelser          [Extention 1: Den normale lånetid er ikke overskredet]. Bøden be-
                                 regnes derfor til 0 (nul) kr.
                                                  1. UC fortsætter fra punkt 9.

                                 [Extention 2: Låneren (eller den person som afleverer bogen for lå-
                                 neren) kan ikke betale]
                                                1. Medarbejderen registrerer i Bibliotekssystemet at
                                                   bøden ikke er betalt.
                                                2. UC fortsætter fra punkt 8.


Der skal ikke tages hensyn til yderligere fejlscenarier eller extensions, f.eks. at bogen aldrig er checket
ud eller at id for bogen ikke kan scannes eller ikke kendes.

Use Casen Check Bog Ud kan tænkes at forhindre at en låner må checke bøger ud, hvis låneren har
udestående bøder, derfor skal det registreres i LånerDatabasen. Selve Use Casen Check Bog Ud skal
ikke implementeres i denne aflevering.



                   Opgaven fortsætter på næste side.

                                                                                              Side 4 af 9

<!-- side 5 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
Et løseligt udkast til et softwaredesign (ikke et fuldt formelt UML diagram) er givet nedenfor. Mulige
klasser og deres relationer og interaktioner er skitseret. Forbindelserne symboliseret med et lyn-
symbol tænkes implementeret med C# events eller en anden implementation af Observer pattern.
Dette er et oplæg, og andre forbindelser er tilladte, når blot de beskrives, og implementeres og testes,
hvor det er relevant.


                                                            BogScanner




                                   BogIdentifikation Læst

                                                                                                 BødeModul




                 BrugerInterface                            AfleverBog

                                                                                                BogDatabase




                                                              Printer                           LånerDatabase




                                       Figur 3: Designskitse for Bibliotekssystemets software

De foreslåede klasser er som følger:

BrugerInterface er en boundaryklasse, der via skærm, mus og tastatur kommunikerer med
medarbejderen så denne kan arbejde med Bibliotekssystemet.

AfleverBog er controlklassen for use casen Aflever Bog. Klassens opgaver fremgår af ovenstående
beskrivelse af use casen, og af nedenstående sekvensdiagrammer.

Bogscanner er en boundaryklasse, der står for interfacet til et eksternt intelligent system, der kan
scanne en bogs identifikation – det er ikke vigtigt om dette foregår ved hjælp af stregkode, RFID eller
andre principper.

BødeModul er en hjælpeklasse, der ud fra en overskridelse af lånetiden kan beregne bøden for
overskredet lånetid ud fra nedenstående tabel og metode.

BogDatabase er en boundaryklasse, der giver et interface til databasen over alle bibliotekets bøger.
For hver bog gemmes bogens identifikation, så den kan genkendes når den scannes, aktuel
udlånsstatus, sidste udcheckningsdato og låner, samt de almindelige biblioteksmæssige data som
forfatter, titel, genre, osv. Der kan være eksemplarer af en bog, dette håndteres af den identifikation,
der er givet bogen, så de enkelte eksemplarer kan skelnes fra hinanden. Identifikationen anvendes som
primær nøgle til disse data for hver bog/eksemplar. Data for en bog kan returneres som en simpel
klasse – et Data Transfer Object – DTO, som er en domæneklasse.



                Opgaven fortsætter på næste side.
                                                                                                                Side 5 af 9

<!-- side 6 -->

      Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
      Eksamen: Sommer 22 Reeksamen
      Prøve: SW4SWT/ST4SWT/I4SWT
      Dato: 2022-08-15
      LånerDatabase er en boundaryklasse, der giver et interface til databasen over alle bibliotekets lånere.
      For hver låner gemmes personlige identifikationsdata som CPR-nummer, navn, adresse og evt.
      udestående ubetalte bøder med størrelse og data for bødens oprettelse. CPR-nummer anvendes som
      primær nøgle til disse data.

      Printer er en boundaryklasse, der giver et interface til en printer, der kan udskrive kvittering og
      bødestatus for aflevering af bogen.

      Nedenfor følger et sekvensdiagram, der illustrerer de forskellige forløb af Use Casen Aflever Bog med
      hovedscenarie og extentions.

      Sekvensdiagrammet kan findes som bilag på Digital Eksamen, da det er ret stort.


              : AfleverBog                              : BrugerInterface                                 : Bogscanner                             : BogDatabase                             : BødeModul   : LånerDatabase   : Printer




                                                     BogScannet(id : BogId)



                                                                                FindBog(id)




                                                                       { låner : CPR, låneDato : Dato }


                             udregn lånetid


                                                                                                  UdregnBøde(lånetid)




                                                                                                          bøde : double


opt                                                                                                                         [bøde > 0]

                                 OpkrævBøde(bøde)
                                                                               Medarbejderen bruger
                                                                               Betalingssystemet og giver besked
                                                                               via BrugerInterface om resultatet.


        alt                                                                                                                [bøde ikke betalt]

                                  BødeIkkeBetalt()                                                              BødeIkkeBetalt(lånerId: CPR, bøde: double)



                                                                                                                             [bøde betalt]

                                    BødeBetalt()
                                                                                                                  BødeBetalt(lånerId: CPR, bøde: double)




                                                                              BogAfleveret(id)



                                                                                                                            UdskrivKvittering(id : BogId, bøde: double, bødeStatus : bool)




                                                                               Figur 4 Sekvensdiagram for use case Aflever Bog




                                                            Opgaven fortsætter på næste side.




                                                                                                                                                                                                           Side 6 af 9

<!-- side 7 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
Her følger reglerne og en tabel for bøder i forbindelse med for sen aflevering af en bog:

I dette Bibliotekssystem beregnes lånetiden i kalenderdage, og der gives ikke en ekstra dag når en
måned har 31 dage. Den normale og gratis lånetid er 30 kalenderdage.

Bibliotekssystemet anvender PC’ens indbyggede ur, som kan antages at være korrekt indstillet hvad
angår dato og klokkeslæt.

Tabel over bøder:

 Lånetid                            Bøde
 Fra 0 til 30 dage, inkl.           0 kr.
 Fra 31 til 45 dage, inkl.          20 kr.
 Fra 45 til 75 dage, inkl.          50 kr.
 Over 75 dage                       200 kr.

200 kr. er altså den maksimale pris, når bogen afleveres gennem denne Use Case for
Bibliotekssystemet.

Bemærk at i denne tabel er det lånetiden og ikke overskridelsen, der er input.

På de næste sider følger delopgave 1 til 9.


                             Opgaven fortsætter på næste side.




                                                                                            Side 7 af 9

<!-- side 8 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
1.1     Delopgave 1
Vis i din journal et testbart design for softwaren til Bibliotekssystemet for Use Casen Aflever Bog som
et eller flere UML klasse- og uddybende sekvensdiagrammer i korrekt notation, med udgangspunkt i
designudkastet i Figur 3. Redegør for, hvad du mener der gør dit design testbart.

Du skal medtage al den ovenfor beskrevne funktionalitet, de nødvendige boundaryklasser og
interfaces, og de domæne- og hjælpeklasser du finder nødvendige i dit design, for at gøre det testbart.
Hvor der mangler detaljer, finder du selv på nogen, eller udelader dem, med en kommentar på
diagrammet eller i journalen. Det er tilladt med et design, der afviger fra skitsen i Figur 3, men det skal
implementere den beskrevne funktionalitet (som er observerbar udenfor systemet) og være et
modulært og testbart design.

Det er tilladt at tilføje flere klasser hvis de er nødvendige for designet og testbarheden.

Du kan se bort for Use Casen Check Bog Ud og de andre Use Cases, der kunne være i systemet.

Det er ikke nødvendigt med et multitrådet design for at implementere systemet, men det er tilladt, hvis
det fremgår af designet og den videre løsning af delopgaverne.
1.2     Delopgave 2
Du skal implementere klasserne AfleverBog og BødeModul, med udgangspunkt i dit testbare
design, sekvensdiagrammer og specifikationer ovenfor, og antagelserne nedenfor. Du skal også
definere interfaces til afhængighederne, så klasserne kan testes. Alternativt kan du implementere
den/de klasser, som i dit design udfører de ovenfor beskrevne funktioner for Aflever Bog.

Følgende antagelser kan bruges for at gøre implementationen simplere:
    • Der afleveres kun én bog og beregnes kun én bøde af gangen. Der gives en bøde for hver for
       sent afleveret bog
    • Udlånstiden er altid 30 kalenderdage, uanset månedernes længde
    • DTO – domæneklassen for en bog behøver kun at indeholde de informationer, der er vigtige
       for use casen Aflever Bog

Hint:
   •     Den indbyggede C#-type DateTime har funktioner og operatorer der kan bruges til at udregne
         varigheden i dage mellem to tidspunkter.
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
                                                                                               Side 8 af 9

<!-- side 9 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Sommer 22 Reeksamen
Prøve: SW4SWT/ST4SWT/I4SWT
Dato: 2022-08-15
1.7   Delopgave 7
Hvilke elementer af Black Box testing og White Box testing er der i dine tests?
1.8   Delopgave 8
Hvad er statisk analyse af software? Skriv 5 linier, du kan bruge som udgangspunkt til at beskrive
dette emne til den mundtlige del af eksamen.
1.9   Delopgave 9
I et rigtigt projekt bruger man ofte Continuous Integration. Beskriv med dine egne ord: Hvilke elemen-
ter indgår der i en opsætning af dette værktøj? Hvordan arbejder de sammen, og hvilke fordele giver
det at bruge det?




                                                                                           Side 9 af 9

