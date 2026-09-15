# Vejledning i softwaretest

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Stephen Biering-Sørensen, Finn Overgaard Hansen, Susanne Klim og Preben Thalund Madsen. Struktureret Program-Udvikling (SPU), Teknisk Forlag, ISBN 87-571-1046-8. Vejledning i softwaretest, pp 171–207.*

## Indhold

|                                               |     |
|:----------------------------------------------|----:|
| . Indledning                                  | 172 |
| Hvorfor teste?                                | 172 |
| Hvad er systematisk test?                     | 173 |
| . Generelt om test                            | 174 |
| Testbehov                                     | 174 |
| Testplanlægning                               | 175 |
| Testteknikker                                 | 177 |
| Testbemanding                                 | 180 |
| . SPU-tests                                   | 181 |
| Modultests                                    | 181 |
| Integrationstests                             | 182 |
| Accepttest                                    | 187 |
| . Testforberedelse og -udførelse              | 190 |
| Specificer testemner                          | 190 |
| Design testen                                 | 192 |
| Implementer testcases og testomgivelser       | 193 |
| Kør testen                                    | 193 |
| Evaluer testforløbet                          | 194 |
| . Udvælgelse af effektive testcases           | 195 |
| Design af testcases til blackboxtests         | 195 |
| Design af testcases til whiteboxtests         | 198 |
| Strategi for udvælgelse af testdata           | 199 |
| . Testdokumentation                           | 200 |
| Referencedokumenter                           | 201 |
| Indholdsfortegnelse for en testspecifikation  | 201 |
| Indholdsfortegnelse for en testrapport        | 204 |
| Testdokumentationens udvikling gennem faserne | 206 |
| . Debugging                                   | 208 |
| . Testværktøjer                               | 209 |
| . Vejledningens hovedpunkter                  | 210 |
| Litteraturliste                               | 211 |

## Indledning

Formålet med denne vejledning er at sætte softwareudviklere i stand til at planlægge og udføre systematisk softwaretest. Ifølge denne vejledning ligger den første testplanlægning ved projektets begyndelse, således at ideerne fra vejledningen bør tages op allerede ved projektetableringen. Vejledningen lægger vægt på at testarbejdet opdeles. Svarende til SPU-modellen er testarbejdet overordnet opdelt i modultest, integrationstest og accepttest. Hver af disse tests er igen detaljeret opdelt i testaktiviteterne forberedelse, kørsel og evaluering. Testaktiviteter og dokumentation er beskrevet generelt, dvs. at uanset hvilken type test man skal udvikle og udføre, er arbejdsgangen og dokumentationsstrukturen den samme.

Kapitel 2 handler generelt om testbehov, testplanlægning, testteknikker og testbemanding. Kapitel 3 beskriver SPU’s modultest, integrationstest og accepttest. Kapitel 4 beskriver testforberedelse og testudførelse. Kapitel 5 gennemgår udvælgelse af effektive testcases til blackbox- og whiteboxtest. Kapitel 6 beskriver testdokumentationen.

Kapitel 7 summerer ganske kort nogle enkle principper for fejlfinding og fejlretning, og kapitel 8 giver eksempler på testværktøjer.

### Hvorfor teste?

Følgende facts er vigtige for at forstå softwaretest:

- Mennesker laver fejl. Derfor skal tests udvikles med det formål at jagte fejl. Jo flere fejl man finder jo bedre.

- Det tager tid at finde fejl, og test skal inkluderes i projektplanlægningen med realistiske tider. På større softwareprojekter har ca. 50% af udviklingstiden været brugt til test. Systematisk testplanlægning er med til at nedsætte dette tal.

- Fejl skal findes så tidligt som muligt. Det er dårlig økonomi at udskyde alle testaktiviteter til de sidste faser.

- Udviklere er blinde over for egne fejl. Det er derfor hensigtsmæssigt at bemande et projekt, således at ingen skal teste sit eget program alene.

- Selv gode tests kan ikke redde et dårligt program. Programkvaliteter skal indbygges løbende.

### Hvad er systematisk test?

Systematisk test kan beskrives ved hjælp af følgende punkter:

- Gør dig klart, hvad du skal teste.

- Gør dig klart, hvorfor du tester. Hvor vigtigt er det, at det færdige produkt ikke fejler?

- Opdel testarbejdet i mindre, overskuelige aktiviteter.

- Udvælg omhyggeligt testdata til hver enkelt test, og forudsig resultaterne.

- Vær ikke alene om både at planlægge og køre testen.

- Læg vægt på, at tests skal kunne gentages.

- Undgå at sidde foran skærmen for “…lige at prøve noget!” Vær opmærksom på forskellen mellem debugging og test. Test er nøje planlagte programkørsler, som skal finde fejl. Når der er fundet en fejl, anvendes debugging til at finde ud af, hvor fejlen er.

- Kør en passende mængde af tests. Tag resultaterne med ind til skrivebordet. Kontroller dem ved at sammenligne med det forudsagte resultat. Planlæg næste trin.

- Sæt arbejdet i system ved at anvende fortrykte formularer, ringbind med fanebladsinddeling, og eventuelt en database til at gemme testdata, resultater, o.lign.

- Lad testresultaterne have en form, som er direkte brugbar i en testrapport.

## Generelt om test

I dette kapitel beskrives nogle grundlæggende ting om softwaretest. Det er:

- *Testbehov*. Hvilke grunde kan projektgruppen have til at ville investere resurser i testarbejde?

- *Testplanlægning*. Hvordan er opdelingen af testarbejdet i SPU-sammenhæng?

- *Testteknikker*. Hvordan tester man i praksis?

- *Testbemanding*. Hvem skal teste?

### Testbehov

Det tager tid og energi at foretage en systematisk og grundig test. Det er derfor vigtigt at vurdere behovet for test i det enkelte projekt. Hvilke resurser der skal bruges, afhænger af projektets størrelse, programmets kompleksitet, produktets levetid, og af hvor kritiske fejl i det færdige system vil være.

Hvordan gik det ved afleveringen af det sidste system?

> “Vi havde lovet at aflevere den 15. september. Det gjorde vi, så vi måtte rejse over og rette alle de fejl, der blev fundet bagefter!”
>
> “Det betød ikke noget, om det varede en måned mere eller mindre.”
>
> “Kunden ville ikke betale, før systemet virkede, som han mente, der var aftalt.”

Er en fejl kritisk i det endelige system?

> “Ja, du godeste, der er jo menneskeliv på spil!”
>
> “Ja, produktionsstop koster 50.000 kr. pr. time.”
>
> “Ja, tænk på firmaets gode rygte og markedsandel.”
>
> “Nej, den retter vi bare.”

Vil en eventuel fejl være svær (dyr!!) at rette?

> “Ja, vores system er installeret på Nordpolen.”
>
> “Ja, vi sælger 50.000 apparater om året, og vi kan ikke rette, når først apparatet er på markedet.”
>
> “Nej, udstyret står jo i vores laboratorium.”

Skal der bygges/testes videre på programmet?

> “Ja, vi forventer at sælge et stort antal af dette program i mange variationer.”
>
> “Ja, og vi ønsker ikke, at Søren skal hænge på vedligeholdelse i al fremtid.”
>
> “Nej, det er kun et demo-program.”

### Testplanlægning

I et projektforløb kan testarbejdet opdeles i modultest, modulintegration, procesintegration og accepttest.

Man skal ikke vente til testfaserne med at tænke på test. Forberedelsen bør i vid udstrækning finde sted tidligt i projektet, fordi testplanlægningen hænger sammen med de specifikationer og designbeslutninger, der træffes undervejs.

*Figur: SPU’s “V”-model for testaktiviteter.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

“V”-modellen på figur 1.1 viser, hvorledes testfaserne er knyttet sammen med de egentlige udviklingsfaser:

- accepttesten forberedes ud fra kravspecifikationen,

- procesintegrationen forberedes ud fra programdesignet,

- modulintegrationen forberedes ud fra procesdesignet,

- modultesten forberedes ud fra moduldesignet.

*Figur: Testaktiviteter i projektets faser.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Figur 1.2 viser testaktiviteterne, som er knyttet til projektets faser. Der er mange fordele ved at begynde testarbejdet tidligt. Man finder uklarheder i specifikationerne, man får bedre projektplanlægning, og testomgivelser og hjælpeprogrammer kan udvikles i tide.

### Testteknikker

Testteknikkerne afhænger af, hvilket niveau der testes på, og hvor meget man ved om programmet. Ved test af et modul kan man ofte styre input direkte og observere alle resultater. Ved integrationstest og accepttest kan programmet kun testes gennem de grænseflader, der er synlige i det samlede system.

#### Testomgivelser

Til testbrug er det hensigtsmæssigt at have tastatur, skærm, printer og disk-faciliteter. Det er imidlertid vidt forskelligt, hvilke faciliteter der findes i det miljø, programmet skal køre i. Undertiden må man derfor bygge testomgivelser specielt til formålet.

*Figur: Eksempel 1. Eksisterende testfaciliteter i PM.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Til forskellige testtyper har man forskellige ønsker til sine testomgivelser. Nogle tests kræver præcis styring af alle input, andre kræver realistiske omgivelser. Der er i høj grad tale om at afbalancere ønsket om effektiv test mod den tid, det tager at bygge testomgivelserne.

#### Interaktive testkørsler

En testkørsel kan designes, så testeren selv påtrykker input og kontrollerer output. Testeren logger resultaterne enten med papir og blyant eller ved at udskrive skærmbilleder, hvis det er muligt. Fordelen ved den interaktive metode er, at den ofte er hurtig at etablere. Ulempen er, at gentagelse af testen let bliver upræcis.

#### Automatiske testkørsler

Hvis testdata ligger på filer, kan programmet køres automatisk. Resultaterne kan også opsamles på filer og sammenlignes med forventede resultater. Automatiske tests er især nyttige, når mange testcases skal gentages efter rettelser.

#### Test-instrumentering

Test-instrumentering består i at indsætte ekstra kode, som kun bruges under test. Instrumenteringen kan f.eks. udskrive variable, tælle hvor ofte bestemte programdele gennemløbes eller stoppe programmet ved bestemte betingelser:

    IF TEST1 THEN <udskriv en enkelt parameter>;
    IF TEST3 THEN <udskriv en hel tabel>;

#### Debuggere

Der findes mange symbolske debuggere, hvor man kan gennemløbe programmet instruktion for instruktion, sætte stoppunkter og undersøge variable. Debuggere er gode til fejlfinding, når en test allerede har afsløret, at programmet fejler.

### Testbemanding

Den der har skrevet et program, er ikke nødvendigvis den bedste til at teste det alene. Udviklere overser let egne fejl, fordi de ved, hvad de mente, programmet skulle gøre. Test bør derfor bemandes, så andre end programmøren selv er med til at planlægge og kontrollere testen.

I små projekter kan dette være vanskeligt, men også her kan man opnå en forbedring ved at lade en kollega gennemgå testspecifikationen, testdata og resultater. I større projekter bør testansvaret placeres tydeligt i projektplanen.

## SPU-tests

Denne vejledning omhandler fire typer test, nemlig:

- modultest,

- modulintegration,

- procesintegration,

- accepttest.

Disse beskrives i de følgende afsnit.

### Modultests

Modultesten har til formål at vise, om et enkelt modul virker i overensstemmelse med sin modulspecifikation. Modulet testes så vidt muligt isoleret fra resten af programmet. Det betyder, at der ofte skal bygges en testdriver, som kalder modulet under test, og stubbe, som erstatter de moduler, det testede modul selv kalder.

*Figur: Modultestomgivelser.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Det er et stort arbejde at modulteste alle moduler. Testomfanget må derfor planlægges ud fra modulets kompleksitet, risikoen ved fejl, og hvor let modulet kan testes senere i det samlede system.

### Integrationstests

Der er principielt to forskellige aktiviteter i integrationstest. Dels skal allerede testede enheder sættes sammen, dels skal man teste grænsefladerne mellem dem. Integrationstest er derfor ikke en gentagelse af modultesten, men en test af samspillet.

Det er en vigtig del af integrationsarbejdet at planlægge, hvordan integrationen kan foregå på den mest hensigtsmæssige måde. Principielt er der to forskellige måder at integrere på:

##### Trinvis integration.

Enheder tilføjes én efter én eller i små grupper. Hvert integrationstrin testes, før man går videre. Fordelen er, at fejl kan lokaliseres til det senest tilføjede område.

##### Samlet integration.

Efter individuel test i isolerede omgivelser samles alle enheder på én gang. Denne form kaldes ofte “BIG BANG”-test. Den kan virke hurtig, men hvis der findes fejl, kan de være vanskelige at lokalisere.

#### Modulintegration

Grundlaget for modulintegrationen er moduloversigten. Et modulhierarki kan integreres bottom-up, top-down eller ved en kombination.

*Figur: Et eksempel på et modulhierarki, som skal trinvis integreres.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Bottom-up-integration går ud på at starte med de nederste moduler i hierarkiet og derefter tilføje moduler højere oppe. Der anvendes drivere til at kalde de moduler, som endnu ikke kan kaldes af deres rigtige overordnede modul.

*Figur: To integrationstrin i en bottom-up-integration af modulhierarkiet i figur 4.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Top-down-test går ud på at starte med toppen af hierarkiet og erstatte de endnu manglende underordnede moduler med stubbe. Fordelen er, at programstrukturen og de øverste funktioner hurtigt kan demonstreres.

*Figur: Et integrationstrin i en top-down-integration af modulhierarkiet i figur 4.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

I praksis vil man ofte vælge et kompromis mellem de to metoder. Valget afhænger blandt andet af:

- programmets struktur,

- hvilke moduler der bliver færdige først,

- om hardwaren er tilgængelig,

- testfaciliteterne,

- om der er funktioner, som er særligt vigtige at få afprøvet tidligt.

#### Procesintegration

Før processerne integreres, bør de enkelte processer være modultestet og modulintegreret. Procesintegration går ud på at samle processerne og kontrollere, at kommunikationen og den tidsmæssige samordning virker.

*Figur: Eksempel 2. Procesintegrationsplan for Patientmonitorsystemet, som indgår i programdokumentationen.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

*Figur: Eksempel 3. Integrationstest af driver IT6 med den nødvendige driver og de nødvendige stubbe.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Accepttest

Accepttesten er den test, som skal vise, om systemet opfylder kravspecifikationen. Den bør planlægges med udgangspunkt i kravspecifikationen og de aftaler, der er indgået med kunden.

Hvis kunden ikke ønsker selv at deltage i udarbejdelsen af accepttesten, bør projektgruppen alligevel formulere testen sådan, at den kan bruges som grundlag for afleveringen. Det er klart, at projektgruppen selv anvender accepttesten til at undersøge, om produktet er klar til aflevering.

*Figur: Det er nødvendigt med et aftalegrundlag, som kan sikre at afleveringen af produktet kan ske til alles tilfredshed.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Foruden den funktionelle test omfatter accepttesten test eller demonstration af de ønskede kvalitetsfaktorer:

- *Stress-test*. Virker programmet under de værst tænkelige påvirkninger?

- *Volumentest*. Kan programmet håndtere maksimalt input gennem lang tid?

- *Brugertest*. Er systemet tilpas brugervenligt?

- *Sikkerhedstest*. Kan man snyde systemet? Er data sårbare?

- *Test af ydeevne*. Opfylder systemet krav til svar-tider, også ved spidsbelastninger?

- *Lagertest*. Bruger programmet mere lager end estimeret? Hvor mange procent af lageret er i alt i brug?

- *Konfigurationstest*. Skal systemet kunne fungere i forskellige konfigurationer (anden lagerstørrelse, anden inputenhed, etc.)?

- *Kompatibilitetstest*. Skal det nye program kunne køre på forskellige datamater? Skal det erstatte et gammelt program i alle henseender?

- *Pålidelighedstest*. Er der specielle krav til pålidelighed? Kan programmet køre i normal drift, f.eks. 48 timer uden fejl?

- *Fejlbehandlingstest*. Kommer systemet korrekt ud af enhver fejlsituation (strømsvigt, fejl på ydre enheder, etc.)?

## Testforberedelse og -udførelse

Modultest, integrationstest og accepttest bygges trinvist op af de samme typer aktiviteter. Forberedelsen ligger så tidligt som muligt i udviklingsforløbet, mens udførelsen ligger i den egentlige testfase.

| **Testtype**      | **Referencedokument**                                   |
|:------------------|:--------------------------------------------------------|
| Accepttest        | Kravspecifikation                                       |
| Procesintegration | Programdesign (kap. 2, 3 og 4 i Programdokumentationen) |
| Modulintegration  | Procesdesign (afsnit 6.4 ff. i Programdokumentationen)  |
| Modultest         | Modulspecifikation                                      |

### Specificer testemner

- **IDENTIFICER** testemner, dvs. de forskellige krav, der skal testes ud fra en grundig gennemlæsning af grundspecifikationen med test for øje.

- **IDENTIFICER** uklarheder, modstridende krav og krav, som er vanskelige eller umulige at teste.

- **SPECIFICER** testemnerne ved at udarbejde en nummereret liste, hvor hvert testemne kan spores tilbage til grundspecifikationen.

> **Eksempel 4.** Udfra modulspecifikationen til *IndlæsPatientVærdier* fås følgende testemner til black-box-testen:
>
> “Funktion”s-afsnittet giver følgende funktioner, der skal testes:
>
> TE1:  
> der indlæses patientværdier fra stuekoncentrator,
>
> TE2:  
> der kan optræde alarm ved 1., 2., 3. eller 4. læsning,
>
> TE3:  
> tidspunktet registreres,
>
> TE4:  
> patientværdierne skal skaleres.
>
> “Input”-afsnittet giver følgende testemner:
>
> TE5:  
> der kan vælges forskellige sengenumre (1–6),
>
> TE6:  
> der kan vælges hvilke patientværdier, der skal læses (TEMP, PULS, BPSYS og BPDIA),
>
> TE7:  
> der kan vælges hvor mange patientværdier, der skal læses (1–4).
>
> “Output”-afsnittet giver følgende testemner:
>
> TE8:  
> der returneres læste patientværdier med tidsangivelse (1–250, 19.1–44.0),
>
> TE9:  
> der returneres alarmtype (ingen alarm, LINIEALARM1–3),
>
> TE10:  
> modulet selv er en boolsk funktion, der returnerer med alarm-status (TRUE, FALSE).

### Design testen

- **STRUKTURER** testen. Er listen af testemner så omfattende, at testen bør opdeles i flere selvstændige dele?

- **DESIGN** testomgivelser. Hvorledes påtrykkes testinput, og hvorledes observeres testresultaterne?

- **IDENTIFICER** vanskeligheder, som har betydning for projektplanlægningen, f.eks. særlige hjælpeprogrammer eller særligt udstyr.

- **DESIGN** testcases, dvs. udvælg testinput og forventet output for hver test.

*Figur: Eksempel 5. Et eksempel på at testomgivelserne kan opbygges enten til interaktive testkørsler eller til automatiske testkørsler.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Implementer testcases og testomgivelser

- **IDENTIFICER** nye testemner, hvis implementationen afslører forhold, som ikke var synlige i grundspecifikationen.

- **UDFYLD** testcases med konkrete inputdata og forventede resultater.

- **SUPPLER** med testcases valgt ud fra viden om kodens struktur, hvis der er tale om whiteboxtest.

- **BESKRIV** hvorledes testomgivelserne skal opbygges, og hvilke hjælpeprogrammer, drivere og stubbe der skal anvendes.

- **CHECK** at alle nødvendige testfaciliteter er til stede.

- **BEGYND** på testrapporten.

### Kør testen

- **KØR** testen som beskrevet i testspecifikationen og registrer resultaterne. Hvis der findes disk- og printfaciliteter på systemet, logges resultaterne og printes ved lejlighed.

- **NOTER** afvigelser fra den planlagte testkørsel. Hvis operatøren taster forkert, strømmen svigter, eller et hjælpeprogram ikke virker, skal det fremgå af testrapporten.

- **REGISTRER** fundne fejl i problem-/ændringsrapporter, så de kan spores og behandles systematisk.

### Evaluer testforløbet

Efter testkørslen sammenlignes de faktiske resultater med de forventede. Evalueringen skal blandt andet svare på:

- Blev testen kørt som planlagt?

- Er resultaterne i overensstemmelse med det forventede?

- Dækker testen de specificerede testemner?

- Skal der rettes fejl og køres nye tests?

- Skal testspecifikation eller testomgivelser ændres?

## Udvælgelse af effektive testcases

Det er normalt umuligt at teste alle tænkelige kombinationer af inputdata. Testarbejdet består derfor i at udvælge testcases, som har stor sandsynlighed for at afsløre fejl. Der anvendes især to synsvinkler: blackboxtest og whiteboxtest.

*Figur: Blackboxtest contra whiteboxtest.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Design af testcases til blackboxtests

Blackboxtest kaldes også funktionel test. Testen designes ud fra specifikationen uden at tage hensyn til programmets indre struktur.

*Figur: Blackboxtest – funktionel afprøvning* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

En systematisk blackboxtest kan designes således:

1.  Find alle testemner i specifikationen.

2.  Opdel input i ækvivalensklasser, som forventes at blive behandlet ens.

3.  Vælg både gyldigt og ugyldigt input.

4.  Vælg grænseværdier, fordi fejl ofte findes ved grænser.

5.  Gæt på fejl ud fra erfaring med tilsvarende programmer.

6.  Husk specielle værdier, f.eks. maximumværdi, minimumværdi, én over grænsen, én under grænsen, tallet nul som parameter, ASCII-tegnene Null og Blank samt tomme elementer.

7.  Udarbejd de konkrete testcases.

8.  Angiv det forventede resultat for hver testcase.

9.  Kontroller at alle testemner er dækket.

| **Testemne** | **Arbejdstrin** | **Gyldigt input** | **Ugyldigt input** |
|:---|:---|:---|:---|
|  |  |  |  |
| Puls | Klassedeling | $`0<x\leq250`$ (T1) | $`x=0`$ (T2); $`250<x<256`$ (T3); intet nummer – ikke mulig |
| Puls | Grænseværdi | $`x=1`$ (T4); $`x=2`$ (T5); $`x=250`$ (T6); $`x=249`$ (T7) | $`x=0`$ (T2); $`x=-1`$ ikke mulig; $`x=251`$ (T8); $`x=255`$ (T9) |
| Puls | Fejlgætning | blank gyldig | Null ikke mulig; blank ikke mulig |
|  |  |  |  |
| Valg af patientværdi | Klassedeling | Fra 0000 til 1111 (T1) | ikke mulig |
| Valg af patientværdi | Grænseværdi | (T2); 0001 (T3); 1110 (T4); 1111 (T5) | ikke mulig |
| Valg af patientværdi | Fejlgætning | (T6); 0010 (T7); 0100 (T8); 1000 (T9) |  |

**Eksempel 6.** Et eksempel på første trin i designet af tests til “Puls” og “Valg af patientværdi”.

### Design af testcases til whiteboxtests

Whiteboxtest kaldes også strukturel afprøvning. Testen designes ud fra viden om programmets indre struktur. Formålet er at sikre, at væsentlige dele af koden faktisk gennemløbes under testen.

*Figur: Whiteboxtest – strukturel afprøvning* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Ved whiteboxtest kan man f.eks. undersøge, om alle instruktioner er udført mindst én gang, om alle grene i en beslutning er taget, og om alle simple betingelser i sammensatte udtryk har haft både sand og falsk værdi:

    IF A AND B THEN ...
    IF X > 0 THEN ...

Man bør ikke erstatte blackboxtesten med whiteboxtesten. Whiteboxtesten supplerer de funktionelle tests, fordi den kan afsløre, at dele af programmet ikke er berørt af de valgte testdata.

Testdækning kan måles automatisk vha. et værktøj, som registrerer hvilke instruktioner, grene og betingelser, der gennemløbes under kørsel af programmet med de anvendte testcases.

### Strategi for udvælgelse af testdata

En praktisk strategi for udvælgelse af testdata er først at designe blackboxtests ud fra specifikationen. Derefter suppleres med whiteboxtests ud fra programteksten. Hvis der findes fejl, rettes de, og relevante tests gentages. Testdata skal gemmes, så testen kan gentages efter ændringer.

## Testdokumentation

Testdokumentationen skal gøre det muligt at planlægge, gennemføre, gentage og vurdere testen. I et projekt kan der være mange testspecifikationer og testrapporter.

*Figur: Testdokumentation for et projekt.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Et projekt kan således indeholde:

- testspecifikationer og testrapporter for modultests,

- testspecifikationer og testrapporter for modulintegration,

- testspecifikationer og testrapporter for procesintegration,

- 1 accepttestspecifikation plus accepttestrapport.

I SPU-sammenhæng er dokumentationen organiseret således, at kun accepttestdokumentationen fremstår som selvstændige dokumenter, mens de øvrige dokumenter indgår som afsnit i programdokumentationen.

Indholdsfortegnelse for testspecifikationer og testrapporter er beskrevet i afsnit 6.2 og 6.3.

### Referencedokumenter

Enhver test har et dokument som reference. Referencedokumentet er det dokument, testen kontrollerer mod:

- Kravspecifikationen er reference for accepttesten.

- Programdesignet er reference for procesintegration.

- Procesdesignet er reference for modulintegration.

- Modulspecifikationen er reference for modultesten.

### Indholdsfortegnelse for en testspecifikation

En testspecifikation beskriver, hvad der skal testes, hvordan testen skal designes, hvordan den skal implementeres, og hvordan den skal køres.

*Figur: Indholdsfortegnelse til en testspecifikation* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

#### Indledning

Her angives, hvorledes dette dokument og den test, der beskrives heri, hænger sammen med de øvrige aktiviteter i projektet.

##### Formål.

Her præciseres formålet med den test, som er specificeret i testspecifikationen.

##### Referencer.

Enhver test har et udviklingsdokument som baggrund. I dette afsnit identificeres den grundspecifikation, der har været udgangspunkt for denne testspecifikation.

##### Testens omfang og begrænsninger.

Her angives, hvilke programmer, moduler eller lignende, som denne testspecifikation dækker, og hvilke den ikke dækker, hvis der kan være tvivl om det.

##### Godkendelse.

Her angives hvem der er ansvarlig for frigivelse eller aflevering af det testede program og hvilke kriterier der anvendes for at afgøre, om en test er bestået eller ej. Ved at opstille f.eks. det godkendelseskriterium, at denne testspecifikation skal være kørt igennem fejlfrit, undgår man at afleveringen af et program udelukkende bestemmes af, at en bestemt dato nås.

*Figur: Det er desværre ofte tidspunktet mere end objektive godkendelseskriterier, der bestemmer, når et produkt skal afleveres.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

#### Testemner

Testemner er alle de ting, der skal testes. Man kunne kalde kapitlet for “testens kravspecifikation”, idet det identificerer hvad teksten skal omfatte. Kapitlet udfyldes gennem analyse af grundspecifikationen identificeret i afsnit 1.2. Det mest praktiske er at opstille testemnerne nummereret på listeform. For hvert testemne skal der refereres tilbage til et enkelt-afsnit i grundspecifikationen.

#### Testdesign

Dette kapitel beskriver overordnet, hvorledes testen designes, således at alle testemnerne testes på den mest hensigtsmæssige måde. Beskrivelsen består af en nedbrydning til velafgrænsede enkelt-tests, som kan specificeres uafhængigt og udføres i vilkårlig eller veldefineret rækkefølge.

For hver test beskrives enten verbalt eller ved hjælp af en tegning af testomgivelserne, hvordan testen tænkes udført, så godt som det er muligt på planlægningstidspunktet, hvor ingen eller kun meget få implementationsdetaljer er kendt. Af hensyn til projektplanlægningen er det imidlertid vigtigt at kunne identificere, hvilke testprogrammer og hvilket hjælpeudstyr, der er behov for (stubbe, drivere, ekstra printer, patientsimulator, etc.).

Ud fra en blackbox-analyse af de testemner, som den enkelte test omfatter, beskrives overordnet for hver test de forskellige testcases, som skal anvendes i testen omfattende både gyldigt og ugyldigt input.

#### Testimplementation

Svarende til testdesignet beskrevet i kapitel 3 angives for hhv. gyldigt og ugyldigt input nøjagtigt, hvordan input ser ud (Patient: PETER HANSEN, Patientnummer 637). Ligeledes skal der angives, hvordan det forventede output ser ud. I mange tilfælde vil man enkelt kunne afgøre, om det faktiske resultat er i overensstemmelse med det forventede. I andre tilfælde, f.eks. hvor resultaterne godt kan variere inden for visse tolerancer, må dette angives.

I det omfang, hvor testdata opbevares på selvstændige datafiler, vil kapitel 4 kunne begrænses til en reference til eller kopi af disse filer.

De testcases, som tilføjes med det formål at nå et bestemt testdækningskriterium, mærkes separat, således at formålet med alle testcases er sporbart.

#### Udførelse af test

For hver test defineret i kapitel 3 angives en brugsanvisning for afvikling af testen. En sådan brugsanvisning skal indeholde:

- Hvordan bygges testomgivelserne op?

- Hvordan startes testen?

- Hvordan køres testen?

- Hvordan observeres og opsamles resultaterne?

- Hvordan afbrydes testen midlertidigt og hvordan genstartes?

- Hvordan afsluttes testen normalt?

#### Bilag

I bilaget kunne være beskrevet nogle af de faciliteter, der benyttes specielt i denne test, f.eks. beskrivelse af og kildetekst til testprogrammer.

### Indholdsfortegnelse for en testrapport

Testrapporten behøver ikke nødvendigvis at være et selvstændigt dokument. Den vil kunne udformes som en kopi af testspecifikationen, påført testdato, kommentarer og konklusion. Indholdsfortegnelsen for en egentlig testrapport er følgende:

*Figur: Indholdsfortegnelse til en testrapport* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

#### Indledning

##### Reference.

Reference til testspecifikation angives.

##### Identifikation.

Identifikation af den testede software, nu også med faktisk versionsnummer, opbevaringsmedium osv.

De faktiske testomgivelser identificeres så præcist, at de kan genetableres.

#### Testresultater

I dette kapitel registreres testresultaterne. Det vil være forskelligt fra system til system, hvorledes resultaterne kan præsenteres, så det overlades til den enkelte at lave en rimelig opdeling og formatering af resultater. Husk at for den enkelte test refereres tilbage til et underpunkt i testspecifikationen.

#### Afvigelser og kommentarer

##### Afvigelser fra normal afvikling.

Afvigelser i afvikling af testkørslen, som f.eks. sekvenser, der er kørt om på grund af trivielle operatørfejl.

##### Problem/ændringsrapporter.

Detekterede fejl påvist ved uoverensstemmelse mellem det forventede og det opnåede resultat registreres. En fortrykt formular til dette formål findes i *Vejledning i Konfigurationsstyring*. En kopi af problem-/ændringsrapporten indsættes i dette afsnit.

#### Konklusion

Hvordan gik testen? Er den testede software i orden, eller skal alt laves om? Kan man eventuelt frigive en midlertidig version?

### Testdokumentationens udvikling gennem faserne

Som beskrevet i vejledningen udføres testaktiviteter løbende gennem udviklingsprojektet, og testdokumentationen udarbejdes tilsvarende. På følgende figur er vist sammenhængen mellem testaktiviteterne (beskrevet i kapitel 4) og testdokumentationen (beskrevet ovenfor i kapitel 5). For alle integrationstests og modultests indgår testdokumentationen i programdokumentationen, mens accepttestdokumentationen er selvstændige dokumenter på lige fod med kravspecifikationen.

*Figur: Tidsmæssig sammenhæng mellem testaktiviteter og udarbejdelsen af testdokumentationen.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

