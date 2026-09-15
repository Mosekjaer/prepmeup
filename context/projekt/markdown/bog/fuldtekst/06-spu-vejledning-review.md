# Vejledning i review

> **Fuldtekst-udgave.** Mekanisk konverteret fra kompendiets LaTeX-transskription med pandoc (`latex → gfm`). TikZ-figurer er erstattet af en placeholder; figurbeskrivelser og mermaid-gengivelser findes i den strukturerede udgave i [`../`](../README.md). Ingen redaktionel bearbejdning.


*Stephen Biering-Sørensen, Finn Overgaard Hansen, Susanne Klim og Preben Thalund Madsen. Struktureret Program-Udvikling (SPU), Teknisk Forlag, ISBN 87-571-1046-8. Vejledning i review, pp 215–236.*

## Indhold

|                                     |     |
|:------------------------------------|----:|
| . Indledning                        | 217 |
| Reviewets komponenter               | 218 |
| Reviewets faser                     | 219 |
| . Planlægning                       | 220 |
| Fastsættelse af tidspunkt           | 220 |
| Udvælgelse af deltagere             | 221 |
| Klargøring af dokumentet            | 223 |
| Fremfinding af materiale            | 223 |
| Indkaldelse                         | 224 |
| . Formøde                           | 225 |
| Reviewets formål                    | 225 |
| Deltagernes roller                  | 225 |
| Overordnet gennemgang af produktet  | 226 |
| Overordnet gennemgang af dokumentet | 226 |
| . Forberedelse                      | 227 |
| . Reviewmøde                        | 228 |
| Reviewmødets forløb                 | 229 |
| Andre forløb                        | 229 |
| Reviewernes opgaver                 | 231 |
| Reviewlederens opgaver              | 232 |
| Referentens opgaver                 | 232 |
| Tilhørernes opgaver                 | 232 |
| . Efterbehandling                   | 233 |
| Referat                             | 233 |
| Opfølgning                          | 233 |
| Registrering af tidsforbrug         | 233 |
| . Variationer af reviewteknikken    | 234 |
| Uformelle reviews                   | 234 |
| Korrekturlæsning                    | 234 |
| Tekniske gennemgange                | 235 |
| Kodegranskning                      | 235 |
| Inspektioner                        | 235 |
| . Vejledningens hovedpunkter        | 236 |
| Litteraturliste                     | 237 |

## Indledning

Et review er et møde, hvor folk uden for projektet (“reviewere”) fremlægger deres kritik af et dokument.

Dokumenter, der kan reviewes, er f.eks.

- kravspecifikationer

- testspecifikationer

- program- og procesdesign

- modulspecifikationer

- kildetekster

dvs. det meste skriftlige materiale inden for projektet. Også ikke-tekniske dokumenter, som f.eks. projektgrundlag, kan reviewes. I *Vejledning i struktureret programudvikling* beskrives, hvornår i udviklingsforløbet der holdes review. Review benyttes som regel ved milepæle i programudviklingen, f.eks. når man er nået til slutningen af en fase.

Formålet med et review er dels at finde *fejl og mangler* ved dokumentet, dels at påpege hvilke dele af dokumentet, der er gode (*fortræffeligheder*).

Nogle gange kan formålet endvidere være at godkende eller forkaste dokumentet. Hvis dokumentet bliver forkastet, må det gennemgå en opretning, hvor de påpegede fejl rettes, og derpå undergå et nyt review. Mere normalt er det dog, at reviewet ikke ender med en sådan “karaktergivning”, men blot konstaterer eventuelle fejl og mangler, og overlader beslutningen om dokumentets videre skæbne til projektgruppen.

*Figur: FRISKE ØJNE* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Grunden til at man med reviewteknikken kan finde flere fejl, end man ellers ville kunne, er, at der bliver set på dokumentet med *friske øjne*. Det er som regel lettere at finde fejl i andres dokumenter end i sit eget!

Et biprodukt ved at holde reviews er *uddannelseseffekten*. Dels bliver reviewerne uddannet ved at se andres måde at gøre tingene på, dels bliver forfatteren uddannet ved at blive gjort opmærksom på problemer, der ikke var tænkt på.

Der findes flere varianter af reviews, som alle kan anvendes i projektarbejdet. Det drejer sig f.eks. om:

- uformelle reviews

- korrekturlæsning

- tekniske gennemgange

- kodegranskning

- inspektioner

Denne vejledning vil hovedsageligt behandle formelle reviews, men de andre typer vil blive gennemgået i kapitel 7.

De efterfølgende 5 kapitler handler om hver sin fase af det formelle review:

- planlægning

- formøde

- forberedelse

- reviewmøde

- efterbehandling

Resten af dette kapitel omhandler dels *reviewets komponenter* dels *reviewets faser*.

### Reviewets komponenter

I dette afsnit gennemgås kort de forskellige komponenter, der indgår i et review, samt hvilken funktion de har. Det drejer sig om

- materialet

- deltagerne og

- resultatet

#### Materialet

For at et review kan gennemføres, skal der være noget skriftligt materiale til stede, nemlig

- et dokument, som er den specifikation eller den kildetekst, der skal reviewes

- noget baggrundsmateriale, som er den information, der er nødvendig for at forstå dokumentet

- en standard for dokumentets udformning og indhold, som giver retningslinjer for hvordan dokumentet skal være udformet. Denne standard er dog ikke en absolut nødvendighed.

- checklister (hvis de findes, og hvis de ikke er en del af standarden for dokumentets udformning og indhold).

#### Deltagerne

De roller, der er involveret i et review, er følgende:

- forfatteren, som er den person (eller de personer), der har skrevet dokumentet

- reviewerne, som er de personer, der skal gennemgå dokumentet med henblik på at finde fejl, mangler og fortræffeligheder

- reviewlederen, som er den person, der sørger for de praktiske arrangementer omkring reviewet, dvs. indkaldelse, kopiering og uddeling af materiale samt mødeledelse på formøde og reviewmøde

- referenten, som noterer de kritikpunkter, der rejses på reviewmødet. Normalt er forfatteren selv referent

- tilhørerne, som typisk vil være personer fra projektgruppen, der gerne vil overvære reviewet. Også personer fra andre projekter kan overvære et review, for at blive fortrolige med teknikken, inden de måske selv skal deltage i et review.

#### Resultatet

Det synlige resultat af et review er referatet, som indeholder reviewernes kritik af dokumentet. Referatet kan også indeholde reviewmødets *godkendelse* eller *forkastelse* af dokumentet. Dette referat skrives af referenten.

### Reviewets faser

Et review kan inddeles i følgende fem faser:

- *planlægning*, hvor deltagerne udvælges og materialet gøres klar

- *formøde*, hvor forfatteren præsenterer dokumentet for reviewerne, hvor reviewets formål præciseres og hvor forløbet fastlægges

- *forberedelse*, hvor reviewerne hver for sig kritisk gennemgår dokumentet

- *reviewmøde*, hvor reviewerne fremlægger deres kritik

- *efterbehandling*, hvor der udgives referat, og hvor dokumentet rettes med hensyn til de fejl og mangler, reviewerne har påpeget.

*Figur: Reviewets faser* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

## Planlægning

I planlægningsfasen skal følgende aktiviteter udføres:

- fastsættelse af tidspunkt for review

- udvælgelse af deltagere

- klargøring af dokument

- fremfinding af materiale

- indkaldelse

Disse aktiviteter beskrives i de følgende afsnit.

### Fastsættelse af tidspunkt

Det skal både planlægges hvilken dato, reviewet skal holdes, hvornår på dagen, det skal foregå, og hvorlænge.

#### Dato

Reviews skal være med i planen fra starten, men det nøjagtige tidspunkt fastsættes af forfatteren. Det er nemlig vigtigt, at dokumentet ikke sendes til review *før det er færdigt*. Man opnår intet ved at reviewe et halvfærdigt dokument – højst irritation over, at der findes for mange fejl og mangler, som forfatteren sikkert allerede kender!

Lad være med at udskyde et review, fordi der “ikke er tid lige nu”. Projektet gør sig selv en bjørnetjeneste ved at arbejde videre med et dokument, der indeholder fejl.

Derimod kan forfatteren godt arbejde videre med dokumentets efterfølger, mens reviewet er i forberedelsesfasen (efterfølgeren er f.eks. kildeteksten, hvis dokumentet er modulspecifikationen). Man skal da blot huske at rette de fejl, reviewerne påpeger, også i dokumentets efterfølger.

#### Klokkeslet

Det er en god ide, at placere reviewet først på formiddagen, f.eks. kl. 9.30 til 11.30. Det er som regel det tidspunkt, folk er mest oplagte.

*Figur: Klokkeslet* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

#### Reviewmødets varighed

Et reviewmøde bør *aldrig vare længere end 2 timer*! Erfaringen viser, at efter 2 timer falder koncentrationsevnen væsentligt. Hvis dokumentet er for stort til, at man kan nå det hele igennem på 2 timer, er det bedre at dele reviewet over flere dage.

Kommer reviewerne udenbys fra, kan man naturligvis dispensere fra denne regel. Man må da holde gode pauser med passende mellemrum i stedet.

### Udvælgelse af deltagere

Deltagerne bør findes så tidligt som muligt, dvs. helst allerede når reviewet planlægges i projektplanen. På denne måde kan de indpasse deltagelsen i deres egne tidsplaner.

Der må *ikke deltage personer fra ledelsen*. Dette punkt er vigtigt, idet det er *dokumentet*, der skal reviewes, ikke *forfatteren*. Deltager repræsentanter fra ledelsen, vil det næppe kunne undgås, at der fokuseres mere på forfatteren, end der ellers ville være blevet. Dette har den naturlige konsekvens, at forfatteren stiller sig i forsvarsposition fremfor at lægge sit dokument åbent frem. I yderste konsekvens vil ingen godvilligt gøre sit dokument til genstand for et review.

Ved review af kravspecifikationer kan repræsentanter for kunden, salgsafdelingen eller andre relevante afdelinger være gode reviewere.

*Figur: Reviewets deltagere* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

#### Reviewlederen

Den første person, der skal udvælges, er *reviewlederen*. Reviewlederen udpeges af ledelsen i samarbejde med projektgruppen. Reviewlederen kan f.eks. være en anden af projektdeltagerne, men det er bedst, hvis det er en person uden for projektgruppen, da en sådan har lettere ved at være neutral.

Reviewlederen er reviewets “praktiske gris” og skal derfor være en god organisator. Reviewlederen skal også være en god mødeleder, dvs. have styrke til at afbryde begyndende skænderier og personlighed til at kunne skabe en god stemning omkring reviewet.

#### Reviewerne

Reviewerne findes af reviewlederen i samarbejde med ledelsen.

De personer, der skal være reviewere, skal

- være teknisk kompetente inden for området

- have diplomatisk sans

- kunne være i stue sammen

For at alle software-folk i firmaet (med de nævnte kvalifikationer) får lejlighed til at være reviewere, kan man f.eks. indføre en “vagtordning”, således at 4 personer har “review-vagten” i f.eks. 3 måneder. Det betyder, at reviewlederen aldrig har problemer med at finde reviewere.

En anden måde at gribe sagen an på er, at reviewerne udpeges for *et helt projekt ad gangen*. Det er altså de samme personer, der er reviewere af såvel kravspecifikationer som program- og procesdesign, modulspecifikationer osv. Dette indebærer den fordel, at reviewerne ikke behøver bruge tid på at sætte sig ind i dokumentets omgivelser *hver gang*, men kan nøjes med at gøre det første gang.

Endelig kan man finde reviewerne *fra gang til gang*. Dette betyder et større arbejde for reviewlederen, men til gengæld kan man “håndplukke” personer, med interesse for netop det emneområde, det pågældende dokument dækker. Hvis en anden end forfatteren skal arbejde videre med dokumentet i den næste fase, vil vedkommende være en god kandidat som reviewer, da man er mere motiveret for at finde fejl og mangler, hvis man ved, at man selv skal overtage dokumentet.

Normalt vil man have to reviewere, men i enkelte tilfælde kan man have flere. Dette kan f.eks. være tilfældet, hvis man reviewer en kravspecifikation, hvor man gerne vil have flere synspunkter repræsenteret. Det er dog ikke en god ide med mere end 3–4 reviewere. Dels falder reviewernes engagement (“De andre finder nok fejlene!”), dels bliver det svært at overholde de to timer, reviewmødet højst må vare, hvis for mange skal have taletid. Og ofte vil to grundige reviewere alligevel finde fejlene, manglerne og fortræffelighederne.

#### Referenten

*Referenten* kan f.eks. være en af projektdeltagerne, men i praksis vil forfatteren selv være en god referent.

#### Tilhørerne

Tilhørerne må naturligvis dukke op af sig selv, men de skal vide, at reviewet holdes, så det skal annonceres ordentligt. Personer, der ikke har deltaget i et review før, bør inviteres specielt til at være tilhørere, så de kan lære teknikken.

### Klargøring af dokument

Dokumentet skal være færdigt til review og i overensstemmelse med standarden for det pågældende dokument. Bemærk at det er *forfatteren*, der bestemmer, hvornår dokumentet er færdigt til review. Som nævnt tidligere, opnår man intet ved at forsøge at tvinge et review igennem af et dokument, der ikke er klart.

Som en del af denne klargøring til review kan forfatteren fremstille en liste af spørgsmål eller særligt kritiske områder i dokumentet, som reviewerne skal overveje.

Kan det lade sig gøre at udskrive dokumentet med linienumre (eller kopiere det på papir med linienumre), vil det lette reference til specifikke linier på selve reviewmødet.

En typisk størrelse for et dokument, der ønskes reviewet, er 20–30 sider. Dette svarer til, hvad man kan nå igennem på de to timer, reviewmødet højst må vare. Vælger man at dele reviewmødet over flere dage, kan man naturligvis godt reviewe større dokumenter.

### Fremfinding af materiale

Alt det baggrundsmateriale, der er nødvendigt under læsningen af dokumentet, skal findes frem. Det kan f.eks. dreje sig om

- en overordnet beskrivelse på introducerende niveau af det produkt, hvor dokumentet indgår

- forgængerdokumentet (hvis dokumentet, der skal reviewes, er en modulspecifikation, så er forgængerdokumentet lig med procesdesignet)

- skrifter, der henvises til fra dokumentet, eller som er nødvendige for forståelsen af dokumentet. Det kan f.eks. dreje sig om beskrivelser af fælles datastrukturer eller grænseflader.

Ud over dette baggrundsmateriale, skal man også sørge for, at reviewerne får den standard, dokumentet følger, hvis en sådan findes. Endelig skal man sørge for, at reviewerne får eventuelle checklister for dokumentets udformning og indhold, hvis de findes. Ofte vil disse checklister være en del af standarden for dokumentet.

Det er forfatteren, der fremfinder baggrundsmaterialet.

### Indkaldelse

Der skal indkaldes til såvel formøde som reviewmøde i god tid.

*Figur: Indkaldelse* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Sammen med indkaldelsen til *formødet* bør udleveres den overordnede beskrivelse af produktet (hvis en sådan findes). Så har reviewerne en chance for at sætte sig ind i problemstillingen på forhånd.

De reviewere, der ikke har deltaget i et review før, bør også have udleveret denne *Vejledning i review* sammen med indkaldelsen til formødet.

Det er reviewlederen, der er ansvarlig for, at deltagerne bliver indkaldt, og at materialet bliver kopieret.

Af hensyn til eventuelle tilhørere, bør indkaldelsen annonceres offentligt, f.eks. på opslagstavler.

Har man kendskab til personer, der kunne være interesserede i dokumentet, bør de have en speciel invitation til at deltage i reviewet som tilhørere.

Følgende skal kopieres til reviewerne:

- dokument (evt. i to eksemplarer, hvoraf det ene bruges til markering af korrekturrettelser og det andet bruges til de andre kommentarer)

- baggrundsmateriale

- checklister

- spørgsmål til reviewerne

- evt. denne *Vejledning i review*

Det udleverede materiale kan f.eks. samles i et ringbind med fanebladsforside, så det er let for reviewerne at finde rundt i.

## Formøde

Formålet med formødet er dels at reviewerne skal få et klart billede af, hvad der forventes af dem, dels at dokumentet skal introduceres for deltagerne. Formødet kan f.eks. forløbe efter følgende dagsorden:

*Figur: Dagsorden for formøde* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

### Reviewets formål

Reviewlederen indleder med at fortælle hvad det er, der skal reviewes, og hvad formålet med reviewet er. Man kan evt. fortælle lidt om baggrunden, hvilke reviews, der tidligere har været holdt (dvs. dokumentets forgængere) osv.

Under dette punkt udleveres også ringbindet med dokument, baggrundsmateriale og dokument-standard.

### Deltagernes roller

Det er vigtigt, at reviewerne er helt klar over, hvad der forventes af dem. Hvis der derfor er “nye” reviewere med, bør der bruges noget tid på at diskutere selve reviewteknikken og specielt reviewernes “rolle-beskrivelse” (se senere i kapitlet om selve reviewmødet).

Reviewlederen bør også fortælle om dagsordenen for selve reviewmødet, så reviewerne kan tilrettelægge deres fremlæggelse efter det. Det kan f.eks. være, at reviewlederen finder det hensigtsmæssigt, at tage alle kommentarer til et afsnit i dokumentet på én gang. Eller det kan være, at reviewerne skal aflevere deres væsentlige kritikpunkter skriftligt af hensyn til referenten. Alt dette skal de vide på forhånd.

### Overordnet gennemgang af produktet

For at give reviewerne en fornemmelse af den sammenhæng, dokumentet indgår i, fortæller en person fra projektgruppen (gerne forfatteren selv) om selve produktet. Dette punkt kan reviewerne evt. have forberedt sig på, hvis de har fået udleveret en overordnet introduktion til produktet sammen med indkaldelsen til formødet.

Punktet er selvfølgelig overflødigt, hvis reviewerne er “faste reviewere” på projektet, og dermed har kendskab til produktet fra tidligere reviews.

### Overordnet gennemgang af dokumentet

For at lette reviewernes tilegnelse af selve dokumentet, gennemgås det af forfatteren (på overordnet plan). Man kan f.eks. fortælle om formål, struktur, kommunikation med andre moduler osv.

Forfatteren bør også gennemgå baggrundsmaterialet, for at reviewerne kan få et overblik over, hvor de kan finde hvad.

Har forfatteren forberedt nogle *spørgsmål til dokumentet*, som reviewerne særligt skal have opmærksomheden henledt på, gennemgås de på dette tidspunkt.

## Forberedelse

I denne fase skal reviewerne hver for sig studere dokumentet med henblik på at finde logiske fejl, mangler, afvigelser fra standarden og fortræffeligheder ved dokumentet. Dette er *reviewets vigtigste fase*.

*Længden* af forberedelsesfasen er afhængig af flere faktorer, f.eks.

- dokumentets kompleksitet

- baggrundsmaterialets omfang og

- reviewernes erfaring

En typisk forberedelsestid vil være 3–6 minutter pr. side (se f.eks. /Skogstad86/).

Under dette arbejde kan reviewerne f.eks. benytte de checklister for “almindeligt forekommende fejl”, der bør indgå i standarden for det pågældende dokument.

Det er en god ide, at dele kommentarerne i tre grupper:

- *Trykfejl, stavefejl og andre korrekturrettelser.* Disse kan f.eks. noteres med rødt direkte i dokumentet og udleveres til forfatteren ved reviewmødets start.

- *Afvigelser fra standarden* (f.eks. forkert side-/afsnitsnummerering eller udeladte afsnit). Disse vil typisk vedrøre dokumentets form, og derfor blive gennemgået først på reviewmødet. Det er derfor lettest, hvis de er skilt ud for sig selv.

- *Logiske fejl og mangler samt fortræffeligheder.* Logiske fejl kan f.eks. være uopfyldelige krav i en kravspecifikation eller muligheder, man har overset i et programdesign. Afsløringen af denne type fejl er naturligvis en af reviewernes fornemmeste opgaver. En god hjælp til at finde manglerne, er de uddelte checklister. Fortræffeligheder ved dokumentet kan f.eks. være en god måde at løse et bestemt problem på, en overskuelig struktur, etc. Det er vigtigt at påpege fortræffelighederne, så de ikke forsvinder ved en eventuel rettelse af dokumentet.

## Reviewmøde

I dette kapitel gennemgås dels hvordan selve reviewmødet forløber, dels hvilke roller, de forskellige deltagere har under mødet.

### Reviewmødets forløb

Et reviewmøde kan struktureres på mange måder. Den måde, der gennemgås i dette afsnit, er blot en enkelt. Der er heller ingen grund til at stå stift på dagsordenen. Hvis f.eks. en deltager undervejs finder på en kommentar, som egentlig hører hjemme under et punkt, der allerede er behandlet, så bør kommentaren tages med alligevel.

Reviewmødets forløb kan inddeles i følgende punkter:

- Korrekturfejl

- Kommentarer til dokumentets form

- Generelle kommentarer til dokumentet

- Detaljeret gennemgang af dokumentet

- Evt. konklusion (godkendelse/forkastelse)

Under hele mødet noterer referenten de punkter, der rejses af reviewerne.

#### Korrekturfejl

Korrekturfejl (stavefejl, trykfejl o.lign.) kan behandles hurtigt ved at reviewerne udleverer deres korrekturrettelser skriftligt til forfatteren (f.eks. markeret med rødt i en kopi af dokumentet).

#### Kommentarer til dokumentets form

Disse kommentarer drejer sig om, at dokumentet afviger fra den standard, det skal følge. Dette behøver ikke nødvendigvis at være dokumentets fejl, det kan også være standardens fejl. I så fald skal standarden laves om! Det skal dog understreges, at en eventuel diskussion om standarden *ikke* skal foregå på reviewmødet.

#### Generelle kommentarer

Reviewerne fremkommer med deres generelle (overordnede) kommentarer til dokumentet. De kan komme med én kommentar ad gangen på skift, eller de kan skiftes til at komme med alle deres kommentarer på én gang.

#### Detaljeret gennemgang

Reviewerne kommer med deres detaljerede kommentarer, dvs. afvigelser fra standarden, logiske fejl, mangler samt fortræffeligheder. Som tidligere nævnt kan de skiftes til at komme med en kommentar, eller hver reviewer kan komme med alle sine kommentarer på én gang.

Dette kan f.eks. foregå side for side eller funktion for funktion.

#### Konklusion

Hvis reviewet skal ende med en konklusion, skal dette punkt behandles som det sidste på reviewmødet.

Reviewerne skal være klar over, at de forpligter sig både ved at godkende dokumentet og ved at forkaste det. Men selv om reviewerne således bliver gjort medansvarlige for dokumentets korrekthed, fritager det naturligvis ikke forfatteren for ansvar! Evt. kan dokumentet godkendes med kommentarer.

Husk at det er lige så slemt, at godkende et dårligt dokument, som det er at forkaste et godt dokument!

### Andre forløb

Den struktur, der er beskrevet ovenfor, behandler hele dokumentet under ét. Først de generelle kommentarer og så de detaljerede.

Man kan i stedet behandle dokumentet afsnit for afsnit, og lade reviewerne komme med først generelle kommentarer til det pågældende afsnit og derefter detaljerede kommentarer til det pågældende afsnit.

Man kan også gennemgå dokumentet side for side eller afsnit for afsnit, og lade reviewerne bestemme hvad de vil sige, altså uden at bede dem komme med generelle kommentarer først, men lade dem komme med kommentarerne i den rækkefølge, de selv vil.

Endelig kan man strukturere reviewmødets forløb efter de udleverede spørgsmål.

### Reviewernes opgaver

I /Freedman82/ findes følgende gode råd for reviewere:

- Vær forberedt

- Vær solidarisk

- Tal pænt

- Giv også positiv kritik

- Påpege, ikke løse

- Undgå diskussion om stil

- Kun tekniske emner

Den dybere mening med disse råd er:

#### Vær forberedt

Hvis du ikke har haft tid til at forberede dig grundigt, så sig det til reviewlederen *inden* reviewmødet, så mødet kan blive udsat. Lad være med at møde op og prøve at “bluffe dig igennem”. I bedste fald bliver du gennemskuet, og de andre deltagere bliver sure over, at du har spildt deres tid. I værste fald lykkes det for dig, og de fejl, du skulle have fundet i dokumentet, bliver ikke opdaget.

#### Vær solidarisk

Lad være med at være “bedrevidende” over for forfatteren. Ideen med reviewformen er netop, at man skiftes til at reviewe hinandens dokumenter. Derfor er det måske dig, der er forfatter, næste gang. Prøv derfor at fokusere på dokumentet frem for på forfatteren.

#### Tal pænt

Husk på, at forfatteren er i en meget sårbar position. Det er ikke alle, der synes, det er rart, at få sat sine fejl og mangler til skue for andre. Tænk derfor på hvordan du kan formulere din kritik så diplomatisk som muligt. F.eks. hedder det ikke:

- Det er da for dumt, at der ikke er taget højde for overløb. Enhver kan da se med et halvt øje, at programmet går i indeksfejl, når kataloget bliver fuldt.

I stedet kan man f.eks. sige:

- Jeg tror nok, der er et problem med overløb. Er det ikke rigtigt, at programmet går i indeksfejl, hvis kataloget løber fuldt?

Så vil forfatteren bladre lidt i sin specifikation, og sige “nåh jo, det har jeg da vist ikke tænkt på”, og han vil være glad over at have fået uddybet en fejl, i stedet for at være sur over, at han er blevet gjort til grin.

*Figur: Tal pænt* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Som regel bliver spørgsmål formuleret med “Hvorfor …” opfattet mere negativt end spørgsmål, der starter med “Jeg forstår ikke helt …”.

#### Giv også positiv kritik

Prøv også at finde positiv kritik. Det lyder banalt, men negativ kritik glider som regel bedre ned, hvis man også bliver rost:

- Jeg kan godt lide den overskuelige måde dette modul er beskrevet på, men er der ikke et problem med …

Du behøver ikke indlede hver negativ kritik med noget positivt.

Den positive kritik (dvs. fortræffelighederne) er også forebyggende overfor ændringer af de dele, der vitterligt er gode.

#### Påpege, ikke løse

Husk på, at det er din opgave som reviewer at påpege problemer, men du skal *ikke løse* dem. Det er forfatterens opgave, og han/hun skal have lov til at gøre det på sin egen måde. Desuden tillader reviewmødets korte tid heller ikke, at man går ind i diskussioner om alternative måder at gøre tingene på.

Hvis reviewerne har forslag til *andre* måder at løse problemerne på – og hvis forfatteren er interesseret – kan de f.eks. mødes med forfatteren efter reviewmødet.

#### Undgå diskussion om stil

Ting kan laves på mange måder, og du er måske ikke altid enig i den måde, forfatteren har valgt. Spørgsmålet er imidlertid ikke, om du kan lide forfatterens stil, men om det, der står, er korrekt. Prøv derfor at se bort fra, at dokumentet måske ikke lige er lavet, som du ville have lavet det. Koncentrer dig i stedet om, hvorvidt det opfylder specifikationerne og overholder standarden.

Standarden kan dog også omhandle stilen, og i så fald er det et emne, der kan diskuteres på reviewet!

Bemærk at hvis et dokument er svært at vedligeholde, så er det *ikke* et spørgsmål om stil.

#### Kun tekniske emner

Reviewets opgave er at finde tekniske problemer i dokumentet. Derimod er betingelserne, hvorunder dokumentet er frembragt (f.eks. tidsplaner og bemanding), irrelevante. Det er dokumentet, der skal reviewes.

### Reviewlederens opgaver

Det er reviewlederens ansvar, at

- reviewet bliver struktureret bedst muligt

- reviewets praktiske afvikling forløber bedst muligt

- ingen brænder inde med kommentarer

- diskussionen ikke løber af sporet

Det er derimod ikke reviewlederens ansvar, at

- alle kommer til tiden

- alle er forberedte

Det er *alle deltageres ansvar*! Men naturligvis kan reviewlederen godt være “indpisker” og minde deltagerne om mødet lidt i forvejen, hvis erfaringen viser, at det er nødvendigt!

I et review med afsluttende godkendelse/forkastelse er det derimod reviewlederens *pligt*, at være opmærksom på, om alle deltagere er forberedte, eller om nogen forsøger at “bluffe”. Vær f.eks. opmærksom på, om én hele tiden snakker de andre efter munden (“ja, det mener jeg også!”), eller hele tiden kommer med generelle kommentarer. Vær også opmærksom på, om én “læser foran” og finder på kommentarer undervejs. I denne situation må reviewlederen afbryde reviewet, og notere i referatet, at reviewet ikke har kunnet gennemføres af den og den grund. Der må derefter fastsættes et nyt tidspunkt for reviewet.

Det er imidlertid *kun* i denne situation, reviewlederen skal være vagthund! I et almindeligt review uden karaktergivning, kan det ødelægge en ellers god stemning, hvis alle føler sig overvågede.

### Referentens opgaver

Referentens opgave er klar og entydig, nemlig at

- referere reviewernes kritikpunkter så objektivt som muligt

- få referatet udgivet så hurtigt som muligt

Det sidste er ikke det mindst vigtige!

I nogle situationer kan det være en god ide, at lave et “offentligt referat”, dvs. referenten skriver referatet på overheads eller på flip-over, så alle deltagere kan se, hvad der bliver skrevet i referatet mens det bliver skrevet. På denne måde undgår man, at referenten misforstår eller overhører væsentlige punkter.

Referenten kan også læse svære passager højt.

En anden måde at sikre, at referenten får det hele med, er, at reviewerne afleverer deres kritikpunkter skriftligt til referenten, som så kan sammenskrive dem.

Selv om forfatteren selv er referent, fritager det ikke for at lave et godt og fyldigt referat.

### Tilhørernes opgaver

Tilhørernes fornemmeste opgave er, at *holde mund* og i det hele taget forstyrre reviewet så lidt som muligt.

Mener en tilhører at have et væsentligt bidrag til diskussionen, kan man naturligvis godt henvende sig til reviewlederen og bede om at måtte komme med en kommentar. Men det kan virke meget irriterende på de andre deltagere, hvis tilhørerne hele tiden blander sig.

## Efterbehandling

### Referat

Som nævnt er det vigtigt, at referatet bliver udgivet så hurtigt som muligt.

*Figur: Referat* — [Figur: TikZ-tegning, se .tex / figurbeskrivelse i den strukturerede udgave]

Referatet bør naturligvis indeholde alle reviewernes kritikpunkter og også de positive!

Er reviewet afsluttet med en konklusion, skal denne også med i referatet, gerne i indledningen.

Referatet *distribueres* til deltagerne i reviewet (så reviewerne kan få lejlighed til at kontrollere, at de ikke er blevet misforstået). I et review med godkendelsesfunktion, skal referatet også distribueres til ledelsen.

### Opfølgning

De fejl og mangler, reviewerne har påpeget, skal naturligvis overvejes og eventuelt rettes. Det normale er, at forfatteren blot retter fejlene, og så sker der ikke mere.

Der kan imidlertid være rejst så alvorlige kritikpunkter (eller så dybtgående rettelser), at dokumentet skal gennem et nyt review.

Ved review af kravspecifikationer er det ofte ledelsen, der beslutter dette, men normalt er det projektgruppen eller forfatteren selv, der beslutter, at et fornyet review er nødvendigt.

### Registrering af tidsforbrug

Til brug for projektplanlægning af fremtidige reviews er det vigtigt at vide, hvor lang tid, deltagerne har brugt på reviewet.

Reviewlederen bør derfor indsamle deltagernes tidsforbrug. Det kan eventuelt medtages i referatet.

Et typisk tidsforbrug i et review, med en reviewleder, to reviewere og en forfatter, er (målt i persontimer):
``` math
20 + 0.16 \cdot \text{AntalSider}
```

idet der her er regnet med:

|             |               |
|:------------|--------------:|
| Planlægning | 4 persontimer |
| Formøde     | 5 persontimer |
| Reviewmøde  | 8 persontimer |
| Opfølgning  | 3 persontimer |

og forberedelsen er sat til 5 minutter pr. reviewer pr. side.

## Variationer af reviewteknikken

Der findes en række variationer af reviewteknikken, som man kan bruge alt efter situation og temperament.

### Uformelle reviews

Den eneste væsentlige forskel på et formelt og et uformelt review er, at det uformelle review holdes inden for projektgruppen, og altså ikke involverer personer udefra.

I det uformelle review er det også tilladt at komme med løsningsforslag. Hvor det formelle review altid holdes på et færdigt dokument, der altså formodes at være fejlfrit, holdes det uformelle review ofte på dokumenter, der ikke er helt færdige.

### Korrekturlæsning

Dette er ikke et egentligt “review”. Forfatteren uddeler dokumentet til en eller flere personer, som læser det igennem og kommenterer det. Kommentarerne kan enten afleveres skriftligt, men bedre er det at supplere med en mundtlig uddybning.

Denne teknik er ikke helt så effektiv, som reviewet, men kan bruges, hvis det er svært at gennemføre et rigtigt review.

### Tekniske gennemgange

Et review holdes altid *efter* dokumentet er færdigt, hvorimod en teknisk gennemgang (walk-through) er en metode, forfatteren kan benytte, under *fremstillingen* af dokumentet. Naturligvis kan en teknisk gennemgang også træde i stedet for et review, og altså holdes efter, dokumentet er færdigt.

Tekniske gennemgange holdes typisk inden for projektgruppen, og der må gerne fremsættes løsnings-/ændringsforslag.

Den tekniske gennemgang foregår ved at forfatteren gennemgår sit dokument (eller typisk en mindre del af det) på tavlen med et par andre projektdeltagere som tilhørere.

Tekniske gennemgange kræver heller ikke forberedelse af deltagerne, da materialet bliver gennemgået på mødet.

### Kodegranskning

Review af kildetekster kaldes ofte kodegranskning. Kodegranskning er en slags “skrivebordstest” af den pågældende kildetekst, hvor revieweren (“granskeren”) gennemspiller koden med tænkt input.

### Inspektioner

En inspektion er en endnu mere formel udgave af reviewteknikken, som bl.a. omfatter klassificering og optælling af fejl, oplæsning af hele dokumentet på reviewmødet, speciel uddannelse af reviewlederen osv.

Denne teknik, som blev “opfundet” af Michael F. Fagan i 1976 er beskrevet i /Fagan76/, /Fagan86/ og /Skogstad86/.

## Vejledningens hovedpunkter

Denne vejledning har beskrevet, hvorledes man gennemfører et review.

Reviewet er blevet opdelt i faserne planlægning, formøde, forberedelse, reviewmøde og efterbehandling, og det er blevet beskrevet, hvad der udføres i de forskellige faser.

Under planlægningen er det specielt vigtigt, at dokumentet er klart til review, og at der vælges kompetente reviewere.

Under reviewmødet er det vigtigt, at reviewerne fremlægger deres kommentarer til dokumentet på en høflig måde, og at mødet forløber i en positiv stemning.
