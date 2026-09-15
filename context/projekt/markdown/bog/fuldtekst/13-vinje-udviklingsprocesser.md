# Udviklingsprocesser (Vinje)

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Poul Staal Vinje, “Projektledelse af systemudvikling”, Nyt Teknisk Forlag, 3. udgave.  
Kapitel 5.8–5.9, s. 100–103 (Projektmodel, udviklingsmetodik).  
Kapitel 6.3, s. 119–131 (Udviklingsstrategier).  
Kapitel 10.1–10.7, s. 259–277 (Styringsprocessen — RUP).*

## 5.8 Projektmodel

### Formål

En projektmodel opfylder forskellige behov for projektlederen og -deltagerne. Derfor optræder den både i dette kapitel om projektlederens værktøjer og i kataloget over produktivitetsfremmende foranstaltninger. Det samme gælder udviklingsmetodik i dette kapitel.

Formålet er at planlægge projektforløbet på grundlag af en generel model. Definitionen på en projektmodel er:

> *En projektmodel er en teoretisk og forenklet model af et projektforløb.*

Den er teoretisk fordi modellen aldrig vil være identisk med et forløb i praksis. Der vil altid være en række afvigelser i det konkrete projekt i forhold til standardforløbet. En model er desuden forenklet, fordi den kun indeholder aktiviteter, der forekommer i flertallet af projekter, og kun beskriver dem kortfattet og generelt.

På trods af teori og forenklinger er en projektmodel et nyttigt værktøj. Den giver væsentlige fordele inden for:

- udvikling,

- kommunikation,

- dokumentation.

Udviklingsmæssigt bruges modellen som grundlag for aktivitetsplanlægning og fremgangsmåde i projektet. Projektplanen kan i vid udstrækning bygges over de forud definerede aktiviteter. De overordnede faser, de fleste hovedaktiviteter og en del af detailaktiviteterne fås direkte fra modellen.

Projektet bruger således ingen tid på at definere rammerne, et minimum af tid på det gængse og sædvanlige og den meste tid på det usædvanlige og projektspecifikke.

Kommunikationsmæssigt spares der megen tid. Når en interessent modtager et oplæg fra en projektgruppe, er det praktisk at vide, hvad oplægget kan forventes at indeholde, dvs. hvori specifikation af kravene til svartider og tilgængelighed fastlægges endeligt i dette oplæg eller i et kommende oplæg.

Modellen anvendes til løbende at dokumentere i et standardiseret udseende.

### Kreativitet og standardisering

En model går ikke ud over kreativiteten. En model fjerner planløshed og øger sikkerheden for at alle aktiviteter bliver husket. Erfarne projektledere følger ofte egne retningslinier, der kan være både solide og effektive. Der synes måske derfor ikke at være brug for en model. Problemet opstår når medarbejderne forlader organisationen.

### Modeltyper

Der er et antal modeller til rådighed som støtter IT-projekter. Der findes gennemprøvede modeller som er rettet mod udvikling af administrative systemer, der findes modeller som er specielt rettet mod anvendelse af bestemte udviklingsmetodikker, og der er modeller, der støtter integrationsprojekter, hvor udvikling er en mindre del af arbejdet.

Til et almindeligt målstyret projekt finder der mange “færdige” vandalfaldsmodeller, bygget op med faserne:

- Foranalyse

- Analyse

- Design

- Konstruktion

- Test.

Hver fase er detaljeret i 10–20 hovedaktiviteter, og under dem flere hundrede detailaktiviteter i hver fase.

Hvis der er brug for en cyklisk (iterativ) model, kan man for eksempel se på DSDM (Dynamic Systems Development Method). Den er skræddersyet til et iterativt forløb med brug af prototyper. Den indeholder følgende faser:

- Business Study

- Functional Model Iteration

- Design and Build Iteration

- Implementation.

*Modellens faser gennemføres flere gange, med stigende detaljeringsgrad.*

### Sammensatte modeller

### Gennemførelse

I forbindelse med opstarten vurderes aktiviteterne i modellen:

- Hvilke aktiviteter er overflødige i dette projekt?

- Hvilke aktiviteter skal tilføjes i dette projekt?

Den reviderede model bruges derefter som:

- Skelet for aktivitetsplanlægningen

- Grundlag for tidsestimeringen

- Grundlag for kalenderfast sættelsen

- Skelet for bemanding.

En gennemarbejdet model indeholder også forslag til reviews og milepæle.

### Resultat og anvendelse

Modellen giver i sig selv en grundlæggende systematik. Desuden fungerer den som skelet for mange andre aktiviteter, der er knyttet til bestemte tidspunkter eller opgaver i forløbet. Det kan være opfølgning på økonomi, kvalitetsstyring, godkendelser af styregruppen, udarbejdelse af uddannelsesmateriale eller fremstilling af brugervejledninger.

Modellen kan udbygges med formelle udviklingsmetodikker. Hvis projektet skal bruge E/R- eller OO-analyse, kan de nødvendige aktiviteter placeres i modellen. Resultater og dokumentation integreres med den øvrige systemdokumentation og bliver en del af den samlede dokumentation efter afslutningen af en fase.

## 5.9 Udviklingsmetodik

### Formål

Dette afsnit er kun relevant for udviklingsprojekter og for eksempel ikke for integrations- og implementeringsprojekter.

Udviklingsmetodikker har til formål at understøtte en systematisk analyse- og designproces. Fejl i disse faser er de dyreste at rette og de mest afgørende for interessenternes tilfredshed.

Teknikkerne kan være enkeltstående eller integrerede, det sidste for eksempel som Rational Unified Process med værktøjsstøtte.

### Valg af metodik

En god udviklingsmetodik giver produkter, der kan læses og forstås af interessenterne.

Desuden skal en metodik indeholde værktøjer til flere formål. Den bør være værktøjer til den kreative udredende fase, andre værktøjer, der dokumenterer udredningen i form af grafer og modeller, og værktøjer og teknikker til verifikation og test af resultatet. Det er en fordel hvis metodikken er automatiseret.

Grundlæggelse skal der vælges mellem at bruge en totalmetode eller en kombination af flere forskellige. I sidste tilfælde kan valg af metodikker gøres mere situationsbestemt. Totalmetoderne købes typisk med en udviklingsmodel og kan anvendes integreret. For de fleste organisationer vil en løsning med flere mere eller mindre uafhængige metodikker være et naturligt valg. En totalmetode kræver ofte en tilsvarende totaloplæsning af udviklingsafdelingens aktiviteter. Uddannelse af hele IT-afdelingens personale er omfattende og forudsætter hele medarbejdernes medvirken og motivation.

Projektlederen bør selv vælge nogle metodikker at arbejde med i projektet, hvis organisationen ikke har valgt nogen som standard.

### Gennemførelse

Anvendelsen af en metodik kan ske i et samarbejde mellem interessenterne og projektgruppen. Det forudsætter at alle har mulighed for at afsætte tid. Direkte medvirken forudsætter også en uddannelse i den valgte metodik.

### Værktøjsstøtte

Fordelen ved automatisering er markant. Det er dog vigtigt at metodikken er formel og anerkendt i litteraturen. Automatisering er ikke garanti for systematisk indførelse, men når en metodik er valgt og uddannelsen er gennemført, kan værktøjer understøtte den praktiske brug. Metodikker bygger ofte på modeller og grafiske fremstillinger, der uden automatisering kræver den tid at vedligeholde manuelt.

### Resultat og anvendelse

Metoderne indeholder normalt en fremgangsmåde og en eller flere diagrammeringsteknikker. For eksempel til at dokumentere data-, proces- og

## 6.3 Strategier

### Indledning

Strategierne dokumenterer projektets overordnede styring af forløbet.

De godkendte strategier påvirker projektets organisation, fremgangsmåde i projektet, sammensætningen af projektdeltagere og valget af værktøjer.

Valg af strategi baseres på forandrings-, system- og projektmålene.

Projektledere med IT baggrund mangler ofte erfaring i strategisk planlægning og er ikke altid bevidste om behovet. Projektledere med en relevant videregående uddannelse er bevidste om behovet, men mangler tilstrækkelig indsigt i naturen af systemudviklingsprojekter til at vælge de rette strategier.

Der er for strategisk planlægning et eksempel på et uddannelsesbehov, der skal tilgodeses uanset projektlederens baggrund. Det er desuden et eksempel på, at det er en fordel at rekruttere projektledere med forskellig uddannelsesmæssig baggrund.

### Styr på projektet

Projektlederen skal styre forløbet og ikke omvendt. Strategier er et middel til at sikre kontrollen.

Strategierne fastlægger fremgangsmåden i projektet ved at tage udgangspunkt i målene og øger sandsynligheden for at målene realiseres.

Strategier giver projektarbejdsformen spændstighed og dynamik. De giver aktiviteterne mening og definerer aktiviteternes indbyrdes sammenhæng.

Strategierne udarbejdes af projektlederen og godkendes af styregruppen/liniechefen, projektdeltagerne og nøgleinteressenterne. Strategierne giver fordel er, at der etableres en fælles opfattelse, og at der skabes nogle fælles forventninger til styringen af forløbet. Strategierne har også en positiv effekt i selve forløbet, idet behovet for løbende at få truffet beslutninger mindskes. Strategierne fungerer som overordnede retningslinjer for projektlederens handlerum.

Et godt eksempel på strategiernes betydning for at realisere et mål er teststrategien defineret for det at teste. Et mål om “0-fejl” skal bakkes op af en strategi, der beskriver hvornår, hvem og hvornår, der gøres noget i projektet for at nå målet.

### Indholdets omfang

En strategi er et dokument og en del af projektets formelle dokumentation. Dokumentet beskriver mål og midler, det sidstnævnte i form af de taktiske tiltag der sikrer at målet nås.

Den generelle model for virksomhedsplanlægning arbejder med tre niveauer: det strategiske, det taktiske og det operationelle niveau. De samme niveauer bruges til at planlægge projektet. Det er kun naturligt, da et projekt kan betragtes om en minivirksomhed, blot med begrænset levetid.

Det er praktisk at beskrive et mål og de dertil hørende taktiske tiltag i samme fysiske dokument. Strategien dokumenterer således resultatet af de to øverste planlægningsniveauer. Projektplanen dokumenterer det operationelle niveau i form af de planlagte aktiviteter. Projektets fysiske planlægning består således af et antal strategier og af en projektplan med de operationelle aktiviteter.

Beskrivelse af en strategi fylder sjældent mere end et par sider tekst. Den skal være væsentlig, konkret og kortfattet.

### Taktisk planlægning

Strategiske mål realiseres ved hjælp af taktiske tiltag inden for fire områder:

*Opgaver:*  
Hvilke aktiviteter skal udføres for at nå målet?

*Aktører:*  
Hvilke typer af medarbejdere skal udføre aktiviteterne?

*Organisation:*  
Hvilke styrende relationer skal der etableres mellem aktørerne?

*Teknologi:*  
Hvilke metoder, teknikker og værktøjer er nødvendige?

Til de fire spørgsmål er der sædvanligvis flere mulige svar, og i valget af den taktiske løsning får planen det specifikke præg, der passer til projektet.

De fire områder kan gennemgås og afdækkes i fri rækkefølge. I praksis er der behov for at analysere dem i flere omgange. Analysen af et element skaber behov for at behandle de øvrige. Bearbejdningen af *opgaver* kan

### Strategiske områder

De strategiske mål kan grupperes og samles fysisk i en eller flere strategier. En strategi vil dække et væsentligt område af projektet eller produktet.

I et systemudviklingsprojekt vil der normalt være behov for tre strategier:

- En udviklingsstrategi, der sætter mål for og beskriver udviklingsforløbet

- En teststrategi, der sætter mål for og beskriver afprøvningsforløbet

- En strategi for indføring, der sætter mål for og beskriver implementeringen.

### Udviklingsstrategier

Følgende grundtyper af udviklingsstrategier gennemgås i det følgende:

- Vandfald

- Delleveringer

- Eksperimentel eller cyklisk udvikling

- Versionsvis udvikling

- “Rapid Application Development” (RAD)

- “Rapid Application Prototyping” (RAP)

- Genbrug.

Den sidste strategi kan synes ude af niveau med de andre. Umiddelbart er genbrug et taktisk middel til at øge produktiviteten. Men i forbindelse med at øge mængden af det mulige genbrug er det fornuftigt at beskrive det som et strategisk mål.

#### Vandfald

Denne velkendte strategi bygger på at gøre en hel fase af arbejdet færdigt, før den næste startes. Foranalysen gøres færdig, derefter starter projektet forfra, men denne gang med analyse, og så videre med de andre faser. Det er altså ikke muligt at gå i retning. Man kan ikke kan gå imod strømmen, opgaven er veldefineret og tidsspilde. Strategien benyttes, når opgaven er veldefineret og velkendt. Forløbet skal have en kort varighed, dvs. mindre end 3 måneder, inden for et kendt og veldefineret forretningssjø, udviklingsmetodik, platformen og testmiljøet etc. Hvis disse betingelser ikke er opfyldt, er denne strategi ikke den rette.

*Figur: Vandfald med overlappende faser.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Anvendelighed:  
Små og korte projekter, Kendt forretningsområde, Kendt teknologi, Veldefinerede mål som projektgrundlag.

Fordele:  
Let af styre, Let at planlægge, Billig når den er anvendelig.

Ulemper:  
Sårbar overfor ændringer i projektets grundlag, Maksimal sårbar overfor risici.

Anbefalinger:  
Stram styring af ændringer, Overlap mellem faser, Inspektion af mål og kravspecifikation.

Strategien er let at planlægge og udføre. Figur 6.3 er praktisk taget en groupplan. Ulempen er primært dens sårbarhed over for ændringer i kravene. Der er brug for en stram og formel styring af ændringer, således at tidligere faser produkter overlap melemmer faserne, således som det fremgår af figur 6.3. Det giver mulighed for at åbne næste fase, således om det fremgår af værende fase afsluttes. Formålet er at se, om der er overraskelser, der peger på at projektet skal blive lidt længere i den aktuelle fase, for denne lukkes endeligt.

#### Delleveringer

Man kan have brug for at udvikle og levere systemet i flere fysiske delleveringer. Strategien bruges når nogle dage har der større betydning andre. Det er brugernes eller leverandørorganisationens behov, der styrer opdeling og udvælgelse. Forløbet drives altså frem af forretningsmæssige eller markedsmæssige behov. Hvis der er en stram plan, fordi “Time-to-Market, leveres lang tid for det vigtigst for kunder/markedet, leveres lang tid for det vigtigst for kunder/markedet. Omvendt kommer de mere afgørende dele senere, end det ellers ville have været muligt.

*Figur: Delleveringer.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Anvendelighed:  
Lange projekter, Kort Time-to-Market, Stramme tidsplaner.

Fordele:  
Antallet af leverancer kan styres, Indholdet af leverancerne kan styres, Det mindst brugbare levereres aldrig, konstant nedprioritering.

Ulemper:  
Forvaltningsbehov parallelt med udvikling, Ressourcer til styring af ændringer.

Anbefalinger:  
Lad interessenterne vælge opsplitning, Brug faste intervaller for levering, fx 3 eller 6 mdr.

Den største ulempe er, at der opstår et vedligeholdelsesbehov efter første delvering. Den er i sagens natur den vigtigste, og der skal etableres forvaltnings- og supportgrupper.

Projektlederen skal ikke bestemme antallet og indholdet af deliveringerne. Overlad det til køberen eller salgsafdelingen. Definér til gengæld en formel og aftalt leveringsplan, for eksempel med leverancer hvert 3. eller 6. måned.

Den enkelte delvering køres som et isoleret vandfald, men således at den første skaber den overordnede arkitektur og bereder vejen for de efterfølgende.

#### Eksperimental strategi/cyklisk udvikling

Vi forlader nu den målstyrede systemudvikling til fordel for det eksperimentelle. I en ægte eksperimental strategi arbejdes der med et ukendt antal versioner. Planlægning er vanskelig, fordi der mangler naturlige milepæle.

Det kan være nødvendigt at eksperimentere når der arbejdes med nye forretningsområder eller ny teknologi. Strategien kan få mange til at bakke, men er den glimt en form for den optimale løsning ikke kan besluttes, men kun kan findes ved at lære mere. Figur 6.5 viser at der kan gås rundt i forløbet et ukendt antal gange.

Strategien er langt hen ad vejen “naturlig” for udviklere og slutbrugere. Men de er heller ikke begrænset af budgetter eller leveringsdatoer. Og håndtering af ændringer er en smal sag, hvor det i andre strategier er en risiko.

*Figur: Eksperimentel/Cyklisk.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Anvendelighed:  
Søge-lære processer, Nyt forretningsområde, Ny teknologi, Ved uklare krav eller mål.

Fordele:  
“Naturlig” for udviklere og brugere, Ændringskrav kan absorberes.

Ulemper:  
Ingen planlægning, Ingen opfølgning, Testen skal gentages, og i sit fulde omfang til sidst.

Anbefalinger:  
Brug et prototypeværktøj, Brugerne skal være aktive deltagere i projektet, Annoncering af lukketider, Strategien skal være ønsket af interessenterne.

Kompetente brugere, med tid og evner til at deltage konkret i projektet, styrker forløbet.

#### Versionsvis udvikling

Denne strategi kan bruges til at give interessenterne indflydelse undervejs, men uden at det bliver egentlig eksperimentelt. Der udvikles prototyper/versioner i en søge-lære-proces, men det aftales fra start hvor mange versioner der udvikleles. Hver version omfatter i princippet hele det nye system i stadig stigende detaljeringsgrad. Versionerne kan ikke sættes i drift, de kan kun bruges til at afgøre det videre forløb. Figur 6.6 viser et forløb med fire versioner.

Fordele:  
Brug et prototypeværktøj, Brug/køb af bestemte versioner, Planlægges med kendte milepæle, andre interessenter forbereder sig på formelle godkendelsesreviews.

Ulemper:  
Kræver stort brugerengagement når en version skal vurderes.

Anbefalinger:  
Vær omhyggelig i valget af kompetente og motiverede brugere, Sæt tid af til ændringer efter en version.

Fordelen i forhold til en ægte eksperimental strategi er aftalen om det specifikke antal versioner. Den første kan være en ren prototype på brugergrænseflade, den næste en version der tydeliggør, hvilke data der bliver tilgængelige, og derefter et par stykker mere der viser den færdige funktionalitet. Versionerne planlægges med kendte milepæle, og brugere og andre interessenter forbereder sig på formelle godkendelsesreviews.

Det bringer ulempen på bane. Der er brug for koordinering af mange menneskers indsats, og der er behov for at planlægge hvordan en version gøres tilgængelig for brugerne. Men det holder ikke i teorien end i praksis. Det kræver meget af projektlederen at sørge for interessenternes medvirken.

#### Rapid Application Development (RAD)

Når det handler om kort leveringstid og overholdelse af datoer, er RAD det rigtige valg.

RAD bygger på at samle de rigtige personer på de rigtige tidspunkter og i det nødvendige tidsrum. Der anvendes workshops af 1–3 ugers varighed, hvor deltagerne arbejder sammen på opgaven.

De første workshops i et RAD-forløb involverer beslutningstagen. Derved spares den tid ved at fjerne den del af processen, hvor de egentlige beslutningstager skal vurdere og projektgruppe arbejde. I stedet sættes de til at beskrive det, de gerne vil have. Den første workshop beslutter, hvilke data/informationen systemet skal håndtere, for eksempel i form af “Business Objects”. Derefter kan det være andre beslutningstager, der på næste workshop fastlægger kravene til funktionalitet, fulgt op med 3–5 ugers samarbejde mellem slutbrugere og udviklere til at designe systemets udseende. Til sidst er der brug for 4–15 ugers konstruktionsarbejde. Figur 6.7 viser et forløb på ca. et halvt år.

*Figur: Rapid Application Development.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Anvendelighed:  
Overholdelse af leveringsdato, Projekter med tidspres, Ikke fuldt specificerede mål.

Fordele:  
Høj produktivitet, Involverer beslutningstager, Forcerer nødvendige kompromiser og beslutninger.

Ulemper:  
Isolerede løsninger, Stort forbrug af ressourcer udenfor projektet, Afhængig af rette ressourcer, Planlægning bliver personafhængig.

Anbefalinger:  
Nøje udvælgelse af deltagerne, Brug værktøjer i hele forløbet, Anvend Time-Boxes og milepæle.

Det giver høj produktivitet at arbejde så fokuseret og intenst. Der er omvendt en risiko for at arbejde med skyklapper på, og der er risiko for manglende integration med andre systemer og forretningsområder.

#### Rapid Application Prototyping (RAP)

RAP er i sin grundstruktur Versionsvis udvikling. Men RAP lægger mere vægt på det prototypeuddviklende element og ikke mindre i forbindelsen med at udvikle og vurdere prototyper. RAP er meget der hedder DSDM, eller andre “Agile” metoder til hurtigt at komme i gang med fremstillede prototyper. DSDM er en metode/model, der også bygger på en veldefineret strategi, nemlig et dynamisk samarbejde mellem udviklere og interessenter støttet af en række navngivne prototyper. DSDM ser sig selv som et RAD-koncept, men med de prototypeuddviklende element, der står centralt. Et rigtigt RAD-forløb skal sammenlignes med et at afryre en kanon mod et mål. Der er ingen trinvis forfining og afredede modeller.

Det er nemt at definere og beskrive emner teoretisk, fx modeller og randers, er de i nogle tilfælde — som for eksempel de praktiske tilbud fra leversamme produkt. Det er helt fint, så længe begge dele er velegnende i forhold til systemet der skal udvikles, og arbejdsprocessen det skal udvikles under.

#### Genbrug

Genbrug bruges til to formål:

- Strategisk at skabe nye komponenter til fremtidens systemer

- Taktisk at genbruge/købe allerede udviklede komponenter.

Det er to meget forskellige formål. De har dog det tilfælles, at det skal sluttes tidligt i projektet. Og at bruger/køber/markedet skal indstille sig på, at løsningen ikke er 100% skræddersyet.

Anvendelse af genbrug spiller især en rolle som produktivitetsmiddel. Skabelonen af genbrugelige komponenter skaber fremtidens produktivitet, men kan koste tid i det skabende projekt.

Genbruget kan omfatte forskellige produkter: design, kode, databeskrivelser, Design Patterns, Use Cases, testdata, planer, varigheder i form af realiserede estimater, kravspecifikationer, dokumentation, risikoanalyser (pas på med det) eller noget helt tolvt.

Genbrug er ofte et supplerende element i en af de andre strategier. Men det er nødvendigt at give det en strategisk status for at det tager det taktiske apparat på plads. Ellers risikerer det hele at blive skønne tanker i stedet for skøn virkelighed.

*Figur: Genbrug.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Anvendelighed:  
Alle typer projekter, undtagen “Mission critical”, Stram Time-to-Market (kun anvendelse, ikke skabelse af genbrug), Stram økonomi (kun anvendelse, ikke skabelse af genbrug).

Fordele:  
Høj produktivitet, Standardisering.

Ulemper:  
“80%’s løsninger”, Vanskelige beslutninger i starten af projektet.

Anbefalinger:  
Opbyg en kritisk masse af muligt genbrug, Fokuser meget på emnet fra start til slut, må ikke drukne.

### Teststrategi

Målsætningen for testens effektivitet fastlægges på grundlag af købers behov for korrekthed og pålidelighed, konkurrenternes formåen og evner på pålidelighed. Fejlfri produkter er altid at foretrække, men da der er en pris på det meste her i verden, må projektlederen tage udgangspunkt i, hvor mange penge der bevilges til test.

Projektlederen kan dog som minimum lade være med at lægge op til en rimelig test til sidst øjeblik, lade som om det ikke ville koste at planlægge at bruge mange erfaringer og lade være med at opstille et budget til test.

Der er tre basisstrategier: den traditionelle med at vente til sidst, V-modellen der planlægger tidligst muligt, og “0-fejl” der udføres tidligst muligt.

#### Den traditionelle

Testen både planlægges og udføres sidst i udviklingsforløbet. Strategien er velegnet til mindre opgaver på velkendte forretningsområder. Fordelen er at testen kan baseres på et færdigt system. Ulempen er at det er dyrere og omkostningseffektivt, fordi der ikke finder fejl af anden type end den tilpas omstillede og fine pindede billeder og uddata.

I praksis betyder det at testaktiviteterne først optræder på projektplanen efter konstruktionsfasen.

#### V-modellen

Testen kan planlægges tidligt, det er det selv et udtryk for, at objekt/data/funktionsmodellerne er forståelige og konsistente. Accepttesten planlægges detaljeret efter foranalysen, bruger- og systemtesten planlægges detaljeret efter analysefasen, og integrationstesten planlægges detaljeret efter designfasen. Komponenttesten ligger dog uændret i forhold til den traditionelle strategi. Testens praktiske udførelse er altså uændret i forhold til den traditionelle, dvs. den ligger sidst i forløbet. Denne strategi er grundlaget for IEEE’s standarder for test. Det betyder at testfaserne planlægges modsat den rækkefølge, de udføres i. Den største forskel for testerne er, at planlægningen baseres på de tidlige modeller af systemet, ikke på det færdige system. System- og brugertesten baseres på objekt/data/funktionsmodeller, og for OO-systemer på tre grundmodeller: objektmodellen, der fokuserer på klassehierarki og relationer, kommunikationsmodellen, der viser “messages”, og den funktionelle model, der viser Events/Use Cases. V-modellen udvides med reviews og inspektioner, og afhængigt af hvor tidligt og hvor omfattende, bevæger projektet sig over mod “0-fejl”.

#### 0-fejl

Kaldes også “zero-defect”. I denne strategi hindres fejlene i overhovedet at opstå. Verifikation og validering udføres parallelt med udviklingen. Ulempen er at prisen; den er høj. Resultatet er til gengæld et fejlfrit, stabilt og vedligeholdelsesvenlightigt system. Strategien sørger også for at leveringsdatoer overholdes. Projektlederen vil ikke opleve at testindsatsen forkortes fordi udviklingen forsinkes, og leveringsdatoen alligevel skal overholdes. Udfordringen i denne strategi er at kunne teste i hver udviklingsmæssig fase i en grad af detaljering og pålidelighed, der finder alle fejl. Produktet skal være fejlfrit på ethvert givet tidspunkt. Det betyder at testerne, herunder brugerne/køberne, skal indstille sig på, at det er en rigtig test, der udføres. De skal behandle den seriøst og bruge testscenarier, der er lige så gennemarbejdede som i en traditionel test på et færdigt system.

Strategien kræver mere omfattende brug af prototyper, formelle metoder og løbende inspektioner og verifikationer.

### Opsummering af teststrategier

Den korte udgave af teststrategier lyder således: I et traditionelt testforløb både planlægges og udføres test, som man gjorde det i gamle dage, se figur 6.9. V-modellen er den for tiden mest anvendte eller i det mindste den mest ønskede. I et V-forløb planlægges testens faser i modsat rækkefølge af den traditionelle, men udførelsen er som vanligt. Den udvidede V-model forsynes med et passende antal reviews og inspektioner. Der “lånes” så meget fra “0-fejl”-strategien som budgettet tillader. Den ægte “0-fejl”-strategi er et opgør med fortiden, idet både planlægning og gennemførelse af test sker i “modsat” rækkefølge af vanetænkningen.

Vær opmærksom på at man får et traditionelt forløb, hvis der ikke gøres noget aktivt. Se meget mere om teststrategier i litteraturlistens ‘Struktureret test’.

Betaesten er kun vist skematisk. Nogle foretrækker betaesten før accepttest, andre efter. Den er vist uden planlægning, men det er selvfølgelig tilladt at planlægge sin betatest ordentligt. Det er bare ikke så afgørende i strategisk sammenhæng.

## 10.1 Indledning

Projektlederen kan anlægge flere indfaldsvinkler, når det drejer sig om at vælge hvilke styringsprocesser der skal vælges til et projekt.

Den ene indfaldsvinklen er en veldefineret tilgang kontra en “Agile” tilgang. Hvis man vælger en veldefineret indfaldsvinkel tages der udgangspunkt i, at projektet kan planlægges med rolle-, proces- og resultater, der er kendt på forhånd. Den bedste model for en sådan tilgang er Rational Unified Process (RUP). Den eksisterer et Extreme Programming (XP). I det tilfælde lægges der mindre vægt på det formelle, og der satses i højere grad på kommunikation.

“Agile” betyder behændig eller smidig og bruges om den type udviklingsmetoder, som XP er et udtryk for. Der er andre agile metoder end XP, for eksempel:

**SCRUM,**  
der supplerer XP fordi den i højere grad sigter på ledelsesprocessen og er velegnet til projekter med ustabile krav, konflikter interesser og krav om effektiv levering af fejlfri software.

**Crystal Clear,**  
er et medlem af en familie af processer. Alastair Cockburn, der er kendt fra sit arbejde med “Use Cases”, er den drivende kraft.

**ASD,**  
Adaptive Software Development, sigter på at tilpasse sig til situationen, gerne ved selvorganisering på tværs af den konkrete organisation, og på tværs af virtuelle teams.

**DSDM,**  
Dynamic Systems Development Method, er en gammel kendning. Den er ikke ud-bygget med de teknikker og mekanismer, der gør den rigtigt “Agile”. Dens væsentligste bidrag er “Prototyping” og “Timeboxing”.

Fælles for “Agile” metoder er, at de er iterative, at de er drevet af konkrete brugerønsker, at de er “Timeboxed”, at de har til formål at levere fejlfrit software, at de er drevet af risici, ved at kende og forholde sig til dem, og at de er tolerante over for ændringer. Endelig må vi sige, at de er generelt kommunikative, ved at mennesker er i dialog, snarere end at der kommunikeres ved udarbejdelse af dokumenter.

URL: www.agilealliance.com

## 10.2 RUP — konceptet

Rational Unified Process (RUP) fra Rational, nu en del af IBM, er en projektmodel. Den er et eksempel på en formel udvikling, der bygger på et veldefineret udviklingsforløb.

RUP giver projektledere mange fordele. Den har en stærk styring af krav, fokuserer på at styre risici, den er iterativ og støttes med værktøjer. Værktøjerne er integrerede og kan give sporbarhed fra krav til testcases og retur.

RUP er moderne med hensyn til test. Det er løbende verifikation og validering, men det er på den anden side også nødvendigt, når der satses på et iterativt forløb.

Som andre moderne modeller er den dokumenteret elektronisk og tilgængelig med en browser. Skabeloner er lette at nå via links, og umiddelbart anvendelige.

RUP støttes direkte af Unified Modeling Language (UML), der visuelt understøtter det system der udvikles. UML modellerne kan både bruges til at kommunikere med forretningssiden og indbyrdes mellem udviklere.

Vi har altså at gøre med en samlet model af udviklingsprocessen, RUP, der går hånd i hånd med en samlet model af systemet, UML. De kan bruges hver for sig med stor nytte, og sammen med endnu større udbytte.

## 10.3 RUP i praksis

RUP består af:

- Faser, hvoraf der er 4.

- Processer, kaldet “Workflows”, hvoraf der er 9.

- Roller, hvoraf der er 33.

- Aktiviteter, og dem er der mange af.

- Produkter, kaldet “Artifacts”, hvoraf der er 63.

- Skabeloner, kaldet “Templates”, og dem er der mange af.

- Retningslinier, kaldet “Guidelines”, og dem er der mange af.

Figur 10.1 viser RUP, dens faser og workflows.

*Figur: Rational Unified Process.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

De fire faser hedder “Inception”, “Elaboration”, “Construction” og “Transition”.

Når projektlederen beslutter at en fase kan lukkes, fordi en eller flere iterationer har frembragt et grundlag for den næste fase, afholdes et review. Det kan være i form af en workshop, der formelt afgår om milepælen er nået. Afslutningen af en fase repræsenterer en vigtig milepæl i projektet.

Faserne er tidsmæssige forløb. Langs med faserne gennemfører projektet et antal iterationer. Projektets egne produktionsplan er derfor en iterationsplan. En iteration udføres det arbejde, der defineres af “workflows”. Samtlige “workflows” kan være aktive i en iteration.

Iterationer har meget forskellig længde. I mindre projekter varer de få uger, i større projekter kan iterationer vare 2–4 måneder.

Hele processen drives af Use Cases (brugsmønstre), der er modelleret i en Use Case Model. Use Cases eksisterer begrebsmæssigt både i RUP og UML. Figur 10.2 viser en Use Case model, og sidst i dette kapitel er et eksempel på en Use Case. Projektlederen kan med fordel bruge Use Cases som det drivende element i både udvikling og test. Til udvikling repræsenterer de et aftalegrundlag med omverdenen, og til test er de grundlag for at definere testscenarier.

*Figur: Use Case Model.* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

De engelske betegnelser på “Workflows” er oversat i denne tekst:

| **RUP**                           | **Dansk**                         |
|:----------------------------------|:----------------------------------|
| Business Modeling                 | Forretningsanalyse                |
| Requirements                      | Kravstyring                       |
| Analysis & Design                 | Analyse og design                 |
| Implementation                    | Implementering                    |
| Test                              | Test                              |
| Deployment                        | Udrulning                         |
| Configuration & Change Management | Konfiguration- og ændringsstyring |
| Project Management                | Projektledelse                    |
| Environment                       | Miljø                             |

## 10.4 Faserne

### Fase: Inception

Inception betyder begyndelse, så det er jo et relativt generelt navn. Men indholdet er meget specifikt. Fasens formål er at etablere en vision for produktet, og det gøres meget konkret, blandt andet ved hjælp af det dokument der netop hedder “Vision”. Det beskriver omfang, indhold og økonomi for det produkt man ønsker og den lange række andre væsentlige forhold om produktet.

Visionen har et afsnit der dokumenterer de fleste af de aktiviteter, der udføres i “Inception”:

- Problembeskrivelse

- Afgrænsning af produktets omfang

- Indholdet

- Prioritering af funktionalitet

- Risikoanalyse

- Overordnet arkitektur.

Ud over visionen fremstilles der en liste over de centrale Use Cases, de første prototyper, accepttestskriterier, et estimat på det samlede tidsforbrug for hele projektet og et budget.

Fasen kan i mange tilfælde gennemføres i en enkelt iteration, men det kan fint være i flere. Til at drive arbejdet i fasen bruges disse processer i RUP’en:

- Forretningsanalyse

- Kravstyring

- Analyse og design

- Test

- Projektledelse

- Miljø.

Opdelingen i iterationer allerede i denne første fase er en stor fordel. Det gør det muligt at levere mere information om systemet ud af projektgruppen og dermed reducere risikoen for at bevæge sig i den forkerte retning.

#### Milepæl: Inception er slut

Milepælen sikrer at nøgleinteressenterne kan godkende “Visionen”, estimater og budget, og checker at nøgleinteressenterne er indbyrdes enige. Milepælen skal formelt sagt sikre, at målet er det rigtige.

### Fase: Elaboration

“Elaboration” betyder at sætte detaljer på. Så det er jo også et passende generelt navn. Men fasen skal styres særdeles håndfast af projektlederen. I elaboreringsprocessen er der mange muligheder for misforståelser, der ser viser sig som fejl i produktet, når systemet bruges.

Fasen drives af arbejdet med Use Cases. Når fasen er slut skal 80% af de identificerede Use Cases have være skrevet så detaljeret, at 80% ikke er risiko for misforståelser. Det skal desuden være de vigtigste Use Cases, der ikke indebærer risici. Risici er i det hele taget i centrum for processen i denne fase.

Use Cases skal også foreligge i en Use Case model, der viser sammenhængen og aktørerne.

Til at drive arbejdet i fasen bruges især disse processer i RUP’en:

- Kravstyring

- Analyse og design

- Implementering

- Test

- Projektledelse

- Miljø.

Fasen omfatter arbejde med de centrale modeller i UML: Use Cases, klassemodeller, sekvensdiagrammer, aktivitetsdiagrammer og samarbejdsdiagrammer. Udfordringen i at komme helskindet igennem elaborering er langt større end i “Inception”. Mænden af dokumentation er vokset. Det er svært at mange af dokumentationen til at holde én metode, eller med og mange interessenter og aktører. Inspektioner og reviews skal give projektgruppen opmærksomhed på banen.

Projektlederen skal sørge for at de egenskaberne er kontrollerbare, at kravene til projektet er opfyldt, eller i det mindste at man nu ville kunne konstatere om de funktionelle egenskaber? Egenskaberne omfatter de vigtigste forhold som robusthed, brugervenlighed og performance. Det er fristende at vente til konstruktionsfasen med at kontrollere dem, men da kan det være for sent. Processen “Kravstyring” indeholder de nødvendige trin og retningslinier til at specificere kravene til egenskaberne. Projektlederen skal sørge for at de egenskaber.

Det er en god idé at bruge risikoanalyse til kvalitetsstyring. Med udgangspunkt i de mulige risici kan reviews, inspektioner og demonstrationer, dels ved hjælp af prototyper, bruges til at checke om der er opstået fejl eller misforståelser.

#### Milepæl: Elaboration er slut

Milepælen sikrer at interessenterne er enige om at indholdet er det rigtige. Milepælen skal afgøre, at der er sammenhængende dokumentation, der er både er udviklet og dokumenteret på den rigtige måde.

### Fase: Construction

Konstruktionsfasen skal sørge for at levere koden, tabeller, ASP’en og de andre fysiske dele. Målet er at levere noget hurtigt muligt, der kan testes af brugere. Konstruktionen sker på grundlag af diagrammerne og modellerne, der er dokumenteret ved hjælp af UML.

Der er brug for at tænke både horisontalt og vertikalt. Det betyder at projektlederen skal afgøre hvor detaljeret, der skal arbejdes på langs af projektmodellens faser, og hvor omfatende iterationerne skal være. Et godt råd er at detaljere den foregående elaboreringsfase maksimalt. At detaljere modellerne til det yderste for tages fat på konstruktion. For således at have fjernet risici, og reducere konstruktionsfasen til det simplest mulige. Men for ikke at ende op i en vandfaldsankegang, er der derfor brug for at arbejde med korte iterationer.

Samtlige workflows kan være aktive, afhængig af iterationernes dybde og bredde, men disse workflows vil naturligt være aktive:

- Implementering

- Test

- Udrulning.

De færdige dele skal testes med de gængse testformer, herunder komponent- og integrationstest. Når iterationerne har leveret et sammenhængende produkt kan der også gennemføres en systemtest. Derefter kan der arrangeres alfatest, hvor brugere tester i udviklernes miljø. Projektlederen skal desuden holde øje med hvornår der er betakandidater, for yderligere at give brugerne mulighed for at anvende systemet i det endelige miljø.

#### Milepæl: Construction er slut

Milepælen skal sikre, at systemets kvalitet er god nok som betakandidat. Milepælen skal sikre, at det ikke vil være spild af brugernes tid.

### Fase: Transition

Fasens formål er implementering. Når et antal iterationer har afsluttet konstruktion, gennemføres en eller flere “Transition” iterationer. Det kan omfatte en samlet alfatest, samt en beta- og accepttest. Før, under eller efter disse afsluttende test, kan der foretages en produktmodning i form af afpudsninger, fejlrettelser, dokumentation, konverteringer og integration til afgørende systemer. Implementeringen kan også omfatte aktiviteter uden for egentlig systemtest, som for eksempel uddannelse, opdatering af helpdesk og de andre aktiviteter, der er nødvendige, når produktet skal bruges i praksis.

Igen kan samtlige workflows teoretisk være aktive, alene på grund af rettelser og produktmodning, men de centrale er:

- Implementering

- Test

- Udrulning.

#### Milepæl: Transition

Hele projektet er færdigt, når den oprindelige “Vision” er opfyldt, og en godkendt accepttest dokumenterer det.

## 10.5 Workflows

Workflows er det bærende element i en udviklingsmodel som RUP, der baserer sig på veldefinerede processer. Workflows binder roller, aktiviteter og “Artifacts” sammen. Den glimrende oversigt i RUP, der både viser sammenhængen med udgangspunkt i rollerne, og der med udgangspunkt i “Artifacts”.

Til hvert workflow er der desuden et antal vejledninger, der er en uvurderlig hjælp i gennemførelsen af aktiviteterne i workflowet. Endelig er der skabeloner til at dokumentere resultaterne, der produceres i workflowet.

Alle workflows er teoretisk mulige at bruge i hver fase. Det er kun “Udrulning” der ikke er aktiv i “Inception”. Men i praksis har hver proces sin tyngde i forhold til en eller flere faser.

### Workflow: Forretningsanalyse

Forretningsanalysen kan være begrænset til videreudvikling af systemer indenfor et eksisterende forretningsområde, eller det kan være i lidt større bredt anlagt forretningsanalyse af et forretningsområde, eller yderligere en analyse af hele forretningen. Det er tilsvarende de forskellige veje igennem workflowet, svarende til ambitionsniveauet.

I alle tilfælde bruges resultater af forretningsanalysen som grundlag for at definere kravene til IT-systemerne.

Til dette workflow er der 35 vejledninger til rådighed. Der er mange, men der skal være støtte til forretningsmodellering, fremstilling af en Use Case model, og andre omfattende aktiviteter.

Analysen kan dokumenteres ved hjælp af UML diagrammerne:

- Use Case Model

- Use Cases

- Klassemodel

- Objektmodel

- Samarbejdsdiagrammer (Collaboration Diagrams)

- Sekvensdiagrammer

- Aktivitetsdiagrammer

- Tilstandsdiagrammer.

### Workflow: Kravstyring

Kravstyring er grundstruktur for projektet. I alle tilfælde bruges resultater af forretningsanalysen som grundlag for at definere kravene til IT-systemerne.

Kompositionsdelen, som indeholder et større projekt, projektet fremstillet, testens faktiske resultater og vækst i sammensætningen. Kompositionsdelen er i alle tilfælde en vigtig del af projektet i de basale relationer og aktiviteter.

### Workflow: Analyse og Design

Dette workflow bestemmer aktiviteter inden for de væsentligste om-råder. Det er de aktiviteter, der skal til for at sætte brugerne i stand til at bruge produktet.

### Workflow: Implementering

Dette workflow bestemmer aktiviteter inden for de væsentligste om-råder: Det er de aktiviteter, der skal til for at sætte brugerne i stand til at bruge produktet. Der er ikke altid nok med et system, det kan også være nødvendig for at tage uddannelse af, og næsten altid er brug for supportfunktioner. Det andet element i en god implementering er produktionslignende miljø, og betafasen lever der tester i det endelige produktionsmiljø.

### Workflow: Test

Test er et gennemgående workflow, der er en aktivitet siden at tages i alt fald. Workflowet slutter af, at kvaliteten er på det ønskede niveau: på det allerede niveau, at alle tests er udført og at de er defineret på grundlag af Use Cases.

### Workflow: Udrulning

Dette workflow bestemmer aktiviteter inden for de væsentligste om-råder. Det er en selvejende “Deployment Plan”.

### Workflow: Konfiguration- og ændringsstyring

Det er et nøjagtigt styreret er sammenhæng med velfungerende produkt.

### Workflow: Projektledelse

Workflow er oplagt til projekter, der giver konkrete planer for projektet. Planerne er her en realistisk form for projekterne.

### Workflow: Miljø

Det er vigtigt at miljøet understøtter projektets gennemførelse.

## 10.6 Rollerne

Hver rolle i RUP er beskrevet med hensyn til hvilke artifacts rollen styrer og producerer. Rollerne er afgørende for at disse processer i løbet af projektet.

Det er projektlederens rolle, og deres til rollen tilknyttede opgaver, ansvaret og kompetencer, der gives et workflow fuldt indblik.

Det er både fordele og ulemper ved roller. Fordelene er bedre styring af aktiviteterne, bedre brug af vejledningerne, tydeligere ansvar og en klar fordeling af arbejdet. Roller i RUP er desuden velbeskrevne og umiddelbart til at anvende.

Med “ulemperne” er der tale om, at ikke alle roller er rette for alle mennesker, og at man med erfaring opdager at roller er for begrænsende, og ikke altid beskrivende for den samlede kompetence man ønsker at bringe i spil.

## 10.7 Artifacts

Artifacts er alle de produkter der fremstilles i projektet: modeller, dokumenter, kode, og alt under håndtering. Tracking tilpasningerne er altid støttende roller, der er nødvendigt for at kunne, dels en del af projektet, dels for at bidrage andre til projektet og anvende hjælp fra dem til slut.

RUP’s faktiske arbejde starter når en artifact produceres. Projektets tidsmæssige plan viser hvornår de kan forventes.

RUP’en indeholder et »Artifact Overview« med de væsentligste dokumentationsenheder, og sammenhængen imellem dem:

- Interessenternes ønsker

- Vision

- Forretningsmodel

- Risici

- Kravspecifikation, herunder en Use Case model og supplerende krav til egenskaber

- Begreber – forklaringer

- Udviklingsplan

- Udrulningsplan

- Arkitektur, software

- Analysemodel

- Designmodel

- Implementeringsmodel

- Testplan.

## 10.8 Use Cases

Use Cases er scenarier, udbygget med bestemte nøjere definerede oplysninger, og i et standardiseret format. Formelle scenarier, kan man kalde dem. På dansk bruges ofte »Brugsmønster«.

En Use Case skal beskrive tre ting:

- et hændelsesforløb således som det vil foregå, når systemet bruges forretningsmæssigt, for eksempel: »kunde beder om levering af vare«.

- brugerens reaktion på hændelsen, for eksempel: »brugeren aktiverer skærmbillede til levering af vare«.

- den forventede reaktion fra systemet på det brugeren gør, for eksempel: »viser skærmbillede xxx med customiseret brugersætning«.

En Use Case svarer til en enkelt forretningsmæssig hændelse. Den omfatter og beskriver alle varianter inden for »hændelsen«.
