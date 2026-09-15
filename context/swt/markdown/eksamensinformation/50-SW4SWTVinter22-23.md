---
title: "SW4SWTVinter22-23"
source: "SW4SWTVinter22-23.pdf"
modul: "Eksamensinformation"
pages: 10
type: "slides"
vision: "ingen-grafik"
---
# SW4SWTVinter22-23

<!-- side 1 -->

Indledning
Denne tekst definerer eksamensopgaven i faget SW4SWT/ST4SWT, Software Test, for indeværende
eksamenstermin på diplomingeniørstudiet i Informations- og Kommunikationsteknologi hhv.
Sundhedsteknologi, på Institut for Elektronik og Computerteknologi..

Eksamensopgaven ligger til grund for den mundtlige eksamen i faget. Med udgangspunkt i opgavens
besvarelse vil du blive eksamineret i fagets indhold.

Opgavens besvarelse (”afleveringen”) skal bestå af netop 2 dele:

       En journal i PDF-format, hvor du besvarer de spørgsmål der er givet i de enkelte delopgaver.

       En implementering i en Microsoft Visual Studio-solution, som indeholder implementeringen
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

Der er et bilag til denne opgave, filen BeregnYdelser.cs som kan hentes på Digital Eksamen i en
ZIP-fil.

Bemærk:
    Du må ikke anvende et offentligt tilgængeligt Git repository i forbindelse med løsningen af
     denne opgave, fx på GitHub.com. Det indebærer risikoen for at din opgavebesvarelse bliver
     synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt. Det er tilladt at bruge et
     ”private” repository uden adgang for andre end dig.

       Du må ikke oprette et Jenkins job på den i undervisningen anvendte CI-server (eller nogen
        anden CI-server) i forbindelse med løsningen af denne opgave. Det indebærer risikoen for at
        din opgavebesvarelse bliver synlig for andre deltagere i denne eksamen, hvilket ikke er tilladt.




Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12

<!-- side 2 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
1 Banksystem med forskellige Use Cases.
Denne opgave omhandler design, implementering og test af software til et banksystem, der
understøtter forskellig ekspeditionsmuligheder, bl.a. godkendelse og optagelse af et mindre banklån.

Et Use Case Diagram ser ud som følger.



                                        BankEkspeditionsSystem



                                                  Beregn Ydelser


    Medarbejder                                                                         Renteserver
                                                       uses



                                                   Godkend Lån




                                          Other Use Case
                                              Other Use Case
                                                  Other Use Case
      Kunde                                                                             Kontoserver
                                                      Other Use Case




                                   Figur 1 Use Case diagram for Banksystem

De forskellige aktører forklares her:

Medarbejderen er den primære bruger af systemet, og den primære aktør for de fleste Use Cases.

Kunden vil kun være indirekte/off stage aktør gennem medarbejderen.

Renteserveren og Kontoserveren er sekundære aktører, der indeholder den information, der er
nødvendig for at udføre de forskellige Use Cases kernefunktionalitet.

Renteserveren kan levere den aktuelle rente for banklån, ud fra dagens kurser på obligationer og andre
informationer. I denne opgave leverer den renten per måned for korte banklån, op til 10 år/120
måneder.

Kontoserveren kan checke status og udføre transaktioner på Kundens konto, fx hæve, indsætte eller
oplyse saldo.

Use Casen Godkend Lån kontrollerer og godkender en låneansøgning fra kunden på et kort banklån,
ud fra kundens månedlige indkomst og faste udgifter, lånets størrelse og den månedlige ydelse ud fra
den aktuelle rente på banklån af denne type.


                    Opgaven fortsætter på næste side.
                                                                                         Side 2 af 10

<!-- side 3 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
Use Casen Beregn Ydelser holder øje med den aktuelle rente, viser denne rente på Medarbejderens
brugerinterface og kan dermed beregne ydelsen på et lån ud fra størrelse, længde og den aktuelle rente.

Opgaven består at programmere og teste de dele af dette system som angives nedenfor. Den nøjagtige
udformning af brugerinterfacet er ikke en del af opgaven, og der lægges op til at passende
boundaryklasser abstraherer disse dele bort.

Her følger en simpel principskitse.




                                                                                 Renteserver




                                      Bankmedarbejderens
                                             PC

                      Printer
                                                                                 Kontoserver
                  Figur 2: Principskitse af Banksystemets mekaniske og elektroniske komponenter




                       Opgaven fortsætter på næste side.




                                                                                                  Side 3 af 10

<!-- side 4 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
Her følger en fully dressed Use Case beskrivelse for Godkend Lån:

 Navn:                          Godkend Lån
 Mål                            Kunden kan søge og få udbetalt et lån
 Initiering                     Kunden møder frem med et ønske om at låne nogle penge, og Bank-
                                medarbejderen aktiverer denne funktion i systemet
 Aktører                        Primær: Bankmedarbejder
                                Sekundær: Kontoserver
                                Indirekte: Kunde
 Antal samtidige forekom-       En
 ster
 Prækondition                   Kunden har en konto i banken
                                Systemet er operationelt, og der er forbindelse til de sekundære ak-
                                tører
 Postkondition                  Kunden har fået godkendt det ønskede lån og kundens konto er kre-
                                diteret lånebeløbet, eller kunden har ikke fået godkendt sit lån, og
                                der er ikke blevet sat penge ind på kontoen
 Hovedscenarie                      1. Bankmedarbejderen vælger Godkend Lån funktionen gen-
                                         nem brugerinterfacet.
                                    2. Kunden præsenterer sit ønske om lån og sine oplysninger:
                                         Beløb, varighed i måneder, faste indtægter per måned og fa-
                                         ste udgifter per måned
                                    3. Bankmedarbejderen indtaster disse informationer
                                    4. Systemet udregner den månedlige ydelse for lånet ved at
                                         bruge use casen Beregn Ydelser
                                    5. Systemet sammenligner lånets ydelse med kundens rådig-
                                         hedsbeløb per måned
                                    [Extension 1: Lånets ydelse overskrider 10% af rådighedsbeløbet
                                    og kan dermed ikke godkendes]
                                    6. Systemet udskriver et lånedokument, der underskrives af
                                         kunden
                                    7. Bankmedarbejderen modtager kontrakten
                                    8. Kunden meddeler hvilket kontonummer, skal udbetales på
                                    9. Bankmedarbejderen indtaster dette kontonummer og udløser
                                         udbetalingen af lånet
                                    10. Systemet opdaterer kontoen med lånebeløbet
 Udvidelser/undtagelser         [Extension 1: Lånets ydelse overskrider 10% af rådighedsbeløbet og
                                kan dermed ikke godkendes]
                                    1. Systemet skriver en forklarende fejlmeddelelse på skærmen
                                    2. Use case afsluttes, en ny låneansøgning kræver at Use Casen
                                         genstartes

Der skal ikke tages hensyn til eller implementeres andre extensions, end de ovenfor nævnte.




                      Opgaven fortsætter på næste side.




                                                                                        Side 4 af 10

<!-- side 5 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
Følgende yderligere specifikationer gælder for de 2 use cases.

Der regnes overalt med månedlig rente og månedlige ydelser, der trækkes fra kundens konto.

For at få godkendt et banklån gennem denne simple og hurtige ansøgningsprocedure, skal kunden
have nok overskud – rådighedsbeløb – når de faste månedlige udgifter er betalt af de faste månedlige
indtægter. Ydelsen på det nye lån må højst udgøre 10% af rådighedsbeløbet.

Hvis dette er opfyldt, er låneansøgningen godkendt og pengene udbetales med det samme, Hvis det
ikke er opfyldt, skal man begynde forfra med et andet beløb og/eller en anden løbetid. Renten afgøres
helt af den aktuelle værdi fra Renteserveren, og ændres kun, hvis der er kommet en anden værdi fra
den.

Som eksempel: har kunden en fast månedlig indtægt efter skat på 24000 kr. og faste udgifter på 14000
kr, er rådighedsbeløbet 10000 kr. Så må ydelsen på lånet højst være 1000 kr. om måneden, men ikke
over.

Lånet skal betales over højst 10 år (120 måneder).

Det er denne regel som Godkend Lån skal implementere.

Ydelsen per måned uderegnes af annuitetsformlen r / (1 - (1 + r)-n), hvor r er rentesatsen i kommatal og
n er antallet af terminer, og formlen angiver ydelsen per lånt krone. Det er denne beregning som
Beregn Ydelser skal udføre, og dette er allerede programmeret i bilaget. Men den allerede
programmerede beregning skal testes, og det er en del af denne opgave.

Som eksempel, hvis man har lånt 10000 kr. til 1,5% om måneden (18% om året nominelt), over 60
måneder (5 år) skal man betale 253,93 kr. per måned. Dette ville blive godkendt for ovenstående
kundeeksempel. Dette lyder dyrt, men korte lån er dyre, og inflationen er også på vej op, og dermed
renten.

Her er en række testeksempler, der kan bruges. Overvej, om der skal flere til.

 Lånebeløb            Rente per måned     Antal måneder          Ydelse per måned
                                                                 (afrundet)
 10000                0,015 (1,5%)        60 (5 år)              253,93
 50000                0,0025 (0,25%)      120 (10 år)            482,80
 100000               0,005 (0,5%)        120 (10 år)            1110,21




                      Opgaven fortsætter på næste side.




                                                                                          Side 5 af 10

<!-- side 6 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
Et løseligt udkast til et softwaredesign (ikke et fuldt formelt UML diagram) er givet nedenfor. Mulige
klasser og deres relationer og interaktioner er skitseret. Forbindelserne symboliseret med et lyn-
symbol tænkes implementeret med C# events eller en anden implementation af Observer pattern.
Dette er et oplæg, og andre forbindelser er tilladte, når blot de beskrives, og implementeres og testes,
hvor det er relevant.


                                                                    Ny Rente Event
                                       BeregnYdelser




                                                                                     Renteserver
        Display
                                                                                      Interface




                                        GodkendLån

                                                                                       Printer




                                           Anden Use Case
                                         Anden Use Case                              Kontoserver
     UserInterface                     Anden Use Case                                 Interface




                                Figur 3: Designskitse for Banksystemets software

De foreslåede klasser er som følger:

UserInterface er en boundaryklasse, der via skærm, mus og tastatur kommunikerer med
medarbejderen så denne kan arbejde med Banksystemet.

Display er en boundaryklasse til skærmen, der også kan benyttes af andre funktioner end UserInter-
face.

GodkendLån er controlklassen for use casen Godkend Lån. Klassens opgaver fremgår af ovenstående
beskrivelse af use casen, og af nedenstående sekvensdiagrammer.

BeregnYdelser er en controlklasse for use casen Beregn Ydelser og en utilityklasse til brug for andre
use cases. Den overvåger det aktuelle renteniveau og er til rådighed for beregning af låneydelser. Dette
er ikke et optimalt design ud fra Single Responsibility princippet, men det skal der ikke rettes op på i
denne opgave.

RenteserverInterface og KontoserverInterface er boundaryklasser, der står for interfacet til de
eksterne systemer, der blev beskrevet ovenfor.

Printer er en boundaryklasse, der giver et interface til en printer, der kan udskrive kvitteringer og
andre dokumenter.



                     Opgaven fortsætter på næste side.


                                                                                                   Side 6 af 10

<!-- side 7 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
Nedenfor følger et sekvensdiagram, der illustrerer de forskellige forløb af Use Casen Godkend Lån
med hovedscenarie og extention.


                 : UserInterface                            : GodkendLån                           : BeregnYdelser                                                  : Kontoserver-
                                                                                                                                         : Display   : Printer
                                                                                                                                                                       Interface




                         Ansøg(beløb, varighed, indtægt, udgifter)
                                                                       BeregnYdelse(beløb, varighed)



                                                                                  ydelse



                  alt                                                                                                [ydelse for stor]




                                           false



                                                                                                                       [ydelse OK]




                                              true
   Kunde underskriver              FrigivLån(kontonummer)
   og vælger
   kontonummer




                                                              Figur 4 Sekvensdiagram for use case Godkend Lån




                                               Opgaven fortsætter på næste side.




                                                                                                                                                     Side 7 af 10

<!-- side 8 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
Use Casen Beregn Ydelser går ud på at systemet løbende modtager den aktuelle renteværdi for små,
korte banklån. Hver gang en ny renteværdi modtages, opdateres den linie på displayet, som viser
renten.

Forbindelsen fra RenteserverInterfacet til BeregnYdelser tænkes implementeret som et C#
event eller en anden implementation af Observer Pattern.

BeregnYdelser vil således altid have den aktuelle rente til rådighed, og kan beregne ydelsen for et
lån.

Her ses et sekvensdiagram, der illustrerer dette.

                                                                              : Renteserver-
                                     : BeregnYdelser                                                           : Display
                                                                                 Interface




                                               HandleNyRente(lånerente) << event >>




   Kald fra andre
   klasser


              BeregnYdelse(beløb, varighed)
                                                       Beregn ydelsen ud
                                                       fra sidst modtagne
                                                       lånerente

                         ydelse



                                                          Figur 5 Sekvensdiagram for Use Case Beregn Ydelser


På de næste sider følger delopgave 1 til 9.


                                                        Opgaven fortsætter på næste side.




                                                                                                                           Side 8 af 10

<!-- side 9 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
1.1   Delopgave 1
Vis i din journal et testbart design for softwaren til Banksystemet, der inkluderer Use Cases Godkend
Lån og Beregn Ydelser som et eller flere UML klassediagrammer i korrekt notation, med
udgangspunkt i designudkastet i Figur 3. Hvis forløbet i dit design afviger fra de ovenfor viste
sekvensdiagrammer, skal der også være sekvensdiagrammer, der viser de relevante dele af
interaktionen mellem klasserne.

Redegør for, hvad du mener der gør dit design testbart.

Du skal medtage al den ovenfor beskrevne funktionalitet, de nødvendige boundaryklasser og
interfaces, og de domæne- og hjælpeklasser du finder nødvendige i dit design, for at gøre det testbart.
Hvor der mangler detaljer, finder du selv på nogen, eller udelader dem, med en kommentar på
diagrammet eller i journalen. Det er tilladt med et design, der afviger fra skitsen i Figur 3, men det skal
implementere den beskrevne funktionalitet (som observerbart udenfor systemet) og være et modulært
og testbart design.

Det er tilladt at tilføje flere klasser hvis du finder dem nødvendige for designet og testbarheden.

Du kan se bort fra andre Use Cases, der kunne være i systemet.

Det er ikke nødvendigt med et multitrådet design for at implementere systemet, men det er tilladt, hvis
det fremgår af designet og den videre løsning af delopgaverne.
1.2   Delopgave 2
Du skal implementere klassen GodkendLån og og færdiggøre BeregnYdelser (som kan findes i
bilaget i filen BeregnYdelser.cs) med udgangspunkt i dit testbare design, sekvensdiagrammer og
specifikationer ovenfor. Du skal også definere interfaces til afhængighederne, så klasserne kan testes.
Alternativt kan du implementere den/de klasser som i dit design udfører de ovenfor beskrevne
funktioner for Godkend Lån og Beregn Ydelser.
1.3   Delopgave 3
Implementér de unit tests, du finder nødvendige for at teste klasserne. Vis resultatet af kørslen af disse
unit tests i din journal og redegør for opbygningen af den test suite, der knytter sig til klasserne. Hvilke
slags test har du lavet?
1.4   Delopgave 4
Vis i din journal eksempler på, hvor og hvordan du i dine unit tests har draget nytte af det testbare
design, du specificerede i Delopgave 1.
1.5   Delopgave 5
Brug coverage beregning til at demonstrere, hvor langt du er kommet i Delopgave 3 med at
implementere et passende antal tests for dine klasser. Gør rede for, hvordan du har suppleret coverage
beregning for at finde de nødvendige og tilstrækkelige test cases.
1.6   Delopgave 6
Hvilke elementer af Black Box testing og White Box testing er der i dine tests?
1.7   Delopgave 7
Hvad er forskellen mellem unit test og integrationstest? Skriv 5 linjer, du kan bruge som udgangspunkt
til at beskrive dette emne til den mundtlige del af eksamen.
1.8   Delopgave 8
Hvad er dynamisk analyse af software? Skriv 5 linjer, du kan bruge som udgangspunkt til at beskrive
dette emne til den mundtlige del af eksamen.


                Opgaven fortsætter på næste side.
                                                                                              Side 9 af 10

<!-- side 10 -->

Aarhus Universitet Institut for Elektro- og Computerteknologi – SW/ST
Eksamen: Vinter 22-23
Prøve: SW4SWT/ST4SWT
Dato: 2023-01-12
1.9   Delopgave 9
I et rigtigt projektteam bruger man ofte et workflow, der bl.a. anvender git, for at kunne arbejde struk-
tureret, når flere skal arbejde parallelt. Beskriv med dine egne ord: Hvilke elementer indgår der i en
sådan proces? Hvordan arbejder de sammen, og hvilke fordele giver det at bruge det?




                                                                                           Side 10 af 10

