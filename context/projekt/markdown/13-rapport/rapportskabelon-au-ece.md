# AU ECE rapportskabelon (report_template) — struktur og vejledende tekst

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | Materiale til SW4PRJ4-02 Projekt 4 (Brightspace "Materiale" → report_template.pdf) |
| **Kursus** | SWISE-01 Indledende System Engineering / SW4PRJ4-02 Projekt 4 |
| **Kilde** | `report_template.pdf` (44 sider; LaTeX-skabelonen fra https://gitlab.au.dk/au-ece-prj/student/rapport-eksempel rendret som PDF, dateret 19. august 2026) |
| **Type** | skabelon |
| **Emner dækket** | Rapportstruktur baseret på V-modellen/IMRAD; vejledende tekst (kursiv i originalen) for hvert kapitel og afsnit; gennemgående eksempel "Drinkbot 4000" (webklient + backend + skænkningsapplikation); brug af AI (AI Fluency-framework); krav til rapport og bilag; sidebudget; tabeller over relevante sektioner/diagrammer for SW-, Embedded SW- og Electronics-projekter; eksempler på kodeudsnit, tabeller og roteret helsidesfigur |

---

> Konvention i denne fil: Skabelonens **vejledende tekst** (kursiv i PDF'en, står under hver overskrift) gengives som almindelig brødtekst. Skabelonens **eksempler** (grå bokse "Eksempel: …" om det fiktive system Drinkbot 4000) gengives i blockquote-bokse markeret *Eksempel*. Sidehenvisninger er til PDF'ens trykte sidetal.

## Forside

Aarhus Universitet, Institut for Elektro- og Computerteknologi. Felter: *Min rapporttitel*; *Fag/Projekt - Gruppenummer*; **Forfattere**: *Dit navn - studienummer* (×2); **Vejleder**: *Hendes Navn*; *Antal anslag: 124*; dato (19. AUGUST 2026).

> Forside

## Abstract

An academic abstract typically outlines four elements relevant to the completed work:

- The research focus (statement of the problem(s)/specific gap in existing research/research issue(s) addressed);
- The research methods (experimental research, case studies, questionnaires, etc) used to solve the problem;
- The major results/findings of the research; and
- The main conclusions and recommendations (i.e., how the work answers the proposed research problem).

(Wikipedia)

> s. 1

## Læsevejledning

Rapportens struktur er en variant af IMRAD tilpasset ingeniørprojekter. Strukturen benytter V-modellen, som vist i Figur 1. I forbindelse med rapporten er modellen udvidet med de akademiske dele, nemlig: *Indledning, Metode, Diskussion* og *Konklusion*.

*Figur 1: Rapportstruktur baseret på V-Modellen.* Venstre ben (Construction, nedad) og højre ben (Evaluation, opad) med vandrette pile mellem parrene:

| Venstre ben (Construction) | Relation | Højre ben (Evaluation) |
|---|---|---|
| Introduction | Answer project questions → | Conclusion |
| Process and Methods | Evaluate chosen process and methods → | Discussion |
| User Requirements | Validate User requirements → | User Acceptance Testing |
| System Design (Architecture) | Verify architecture and internal interfaces → | System Integration Testing |
| Unit Design (Subsystem Design) | Verify Design Units → | Unit Test (Subsystem Design) |

Øverst: "Provides input to" (pil fra Introduction-siden mod Conclusion-siden).

I grove træk beskriver rapporten projektets forløb, fra idé, over krav, til arkitektur, design, test og evaluering.

Når du skriver rapporten så tænk det som at du skriver hvert afsnit, som stod du på netop det sted i projektet. I indledning og metode afsnittene beskriver du hvad du agter at gøre og hvordan. I krav beskriver du hvilke krav du vil opfylde. I arkitektur beskriver du hvordan du vil lave din arkitektur, baseret på kravene. I design beskriver du hvordan du designer og implementerer løsningens komponenter. I testafsnittene tester du og kigger tilbage på resultaterne. Diskussionsafsnittet kigger tilbage på projekter og diskuterer konsekvenserne af de valg i tog undervejs og til sidst konkluderer du på projektet, hvordan det gik i forhold til de mål som du satte op indledningsvist. **Pas derfor på med i metodeafsnittet at skrive "vi brugte Scrum…", da det skuer tilbage. I stedet skal intentionen beskrives.** Hvordan det gik med Scrum kan diskuteres til sidst, hvor der netop skues tilbage.

Det bliver en lidt plandrevet beskrivelse af projektet, også selvom man har arbejdet ved hjælp af en iterativ metode, men det giver en naturlig opbygning af argumenter og evaluering af resultater, som man kender fra IMRAD. Har man arbejdet iterativt, præsenterer man blot det endelige resultat i rapporten. Ønsker man at diskutere hvordan et resultat er fremkommet sprint-for-sprint, kan dette gøres i diskussionsafsnittet.

> s. 2

## Indholdsfortegnelse (skabelonens struktur)

1. Introduktion — 1.1 Baggrund; 1.2 Eksisterende arbejde; 1.3 Systemskitse; 1.4 Problemformulering/Hypotese
2. Udviklingsproces og metode — 2.1 Udviklingsproces; 2.2 Udviklingsmetoder; 2.3 Risikohåndtering; 2.4 Brug af AI (2.4.1 Uddelegering af arbejde; 2.4.2 Fremgangsmåde; 2.4.3 Bedømmelse af genererede output; 2.4.4 Ansvarlighed og transparens)
3. Brugerkrav — 3.1 Introduktion; 3.2 Systemkontekst og aktører; 3.3 Funktionelle krav (3.3.1 Elicitering og analyse; 3.3.2 Specifikation); 3.4 Ikke-funktionelle krav (3.4.1 Elicitering og analyse; 3.4.2 Specifikation); 3.5 Systemets domænemodel
4. Systemarkitektur — 4.1 Introduktion; 4.2 Systemets ydre grænser og relationer; 4.3 Analyse af arkitektur; 4.4 Systemets struktur og grænseflader (4.4.1 Interaktion mellem delsystemer; 4.4.2 Interfaces og protokoller; 4.4.3 Datamodeller og persistering)
5. Design og implementering — 5.1 Introduktion; 5.2 Delsystem \<delsystem navn\> (5.2.1 Introduktion; 5.2.2 Design og implementering af \<sw komponent navn\>; 5.2.3 … \<hw komponent navn\>; 5.2.4 … \<database komponent navn\>; 5.2.5 Tests og resultater; 5.2.6 Delkonklusion)
6. Integrationstest — 6.1 Introduktion; 6.2 Teststrategi; 6.3 Integrationstest af use case 1 (6.3.1 Opstilling; 6.3.2 Resultater); 6.4 Diskussion; 6.5 Delkonklusion
7. Brugeraccepttest — 7.1 Introduktion (7.1.1 Opstilling; 7.1.2 Resultater); 7.2 Diskussion; 7.3 Delkonklusion
8. Diskussion — 8.1 Problemstilling og projektets løsning; 8.2 Proces og metode; 8.3 Brugerkrav; 8.4 Arkitektur; 8.5 Design
9. Konklusion — 9.1 Fremtidigt arbejde
- Bibliografi
10. Appendiks — 10.1 Krav til rapport og supplerende materiale (10.1.1 Antal tegn; 10.1.2 Supplerende materiale); 10.2 Sidebudget for rapport; 10.3 Relevante rapportsektioner og diagrammer (10.3.1 Software Engineering-projekt; 10.3.2 Embedded Software Engineering-projekt; 10.3.3 Electronics Engineering-projekt); 10.4 Code Listing Examples; 10.5 Table examples; 10.6 Rotated Full-page Image Example

Skabelonen har desuden separate lister over Figurer, Tabeller og Kodeudsnit.

> s. 3–6

---

## Kapitel 1 — Introduktion

Dette kapitel introducerer baggrund og motivation for projektet og skitserer det problem, der behandles i denne rapport. Det præsenterer relevant eksisterende arbejde og giver et overordnet konceptuelt overblik over det foreslåede system for at støtte læserens forståelse. Endelig defineres problemformuleringen, der fastlægger projektets omfang og mål. **(2-4 sider)**

### 1.1 Baggrund

Her introduceres projektets kontekst og motivation. Afsnittet beskriver domænet, relevante tendenser og det underliggende problemområde, der skaber behovet for en løsning. Formålet er at give læseren tilstrækkelig kontekst til at forstå, hvorfor projektet er relevant.

### 1.2 Eksisterende arbejde

Her gennemgås relevante eksisterende løsninger, forskning mm. inden for domænet. Afsnittet opsummerer eksisterende fremgangsmåder, disses styrker og begrænsninger og placerer projektet i forhold til disse. Formålet er at demonstrere indsigt i state-of-the-art og begrunde behovet for den foreslåede løsning.

### 1.3 Systemskitse

Her præsenteres en overordnet konceptuel beskrivelse af det foreslåede system. Afsnittet skitserer hovedideen, nøglekomponenterne og hvordan systemet adresserer det identificerede problem. Formålet er at give læseren en intuitiv forståelse af løsningen før den detaljerede analyse. Det anbefales at vise en figur, f.eks. et rigt billede. Hold det funktionelt og undgå detaljer som vi på dette tidspunkt ikke har viden om. F.eks. teknologi som Raspberry Pi, React mm.

### 1.4 Problemformulering/Hypotese

Her defineres det problem, projektet adresserer. Afsnittet angiver omfang, mål og eventuelle antagelser eller begrænsninger. Problemformuleringen bør være klar, præcis og fungere som fundament for de efterfølgende krav, for design og evaluering. Afsnittet bør basere sig direkte på Baggrund og Eksisterende arbejde.

> s. 7

## Kapitel 2 — Udviklingsproces og metode

Dette kapitel beskriver den valgte projektmodel og de udviklingsmetoder, der styrer analyse, design, implementering og test af systemet. Formålet er at sikre en struktureret og systematisk udviklingsproces. **(1-2 sider)**

### 2.1 Udviklingsproces

Her beskrives den valgte projektmodel, herunder strategien for at bevæge sig fra den indledende idé til det endelige resultat, og hvordan modellen er tilpasset projektets kontekst. Eksempler kan være stage-gate, vandfald, iterativ, spiral, agil eller hybride modeller.

> **Eksempel: Udviklingsproces**
>
> Baseret på projektets formulering og kontekst er dette projekt karakteriseret ved følgende:
> - Brugerinteraktion med systemet er afgørende og skal adresseres tidligt i processen
> - Domænet er ikke fuldt forstået og skal undersøges tidligt for at klarlægge, hvad der reelt kræves
>
> For at imødekomme dette, anvendes en stage-gate-model inspireret af UP, da den tillader os at styrke fokus på domæneundersøgelse i starten af projektet og derefter fokusere på iterativ udvikling. Følgende faser vil blive brugt:
>
> - **FASE I: Informationsindsamling og Interviews.** I denne fase undersøges eksisterende litteratur og der udvælges en række interessenter…
> - **FASE II: Kravanalyse og arkitekturudkast.** …
> - **FASE III: Delsystemudvikling, integration og test.** …
> - **FASE IV: Indkøring og overlevering.** …

### 2.2 Udviklingsmetoder

Her beskrives de specifikke udviklingsmetoder og teknikker, der anvendes for at nå projektets mål. Afsnittet kan inkludere metoder til requirements engineering (f.eks. FURPS+), udvikling (f.eks. UML), undersøgelse (f.eks. litteratursøgning), koordinering (f.eks. Task board) og mere.

> **Eksempel: Udviklingsmetoder**
>
> Til projektets faser anvendes følgende metoder:
>
> …
>
> - **Fase II: Krav** — Funktionelle krav beskrives ved hjælp af Use Cases, og ikke-funktionelle krav udledes ved hjælp af FURPS [ref]. I forbindelse med kravene modelleres systemet i brugerens domæne med en domænemodel.
> - **Fase II: Arkitekturudkast** — Den indledende systemarkitektur bygges på basis af Domænemodellen og modelleres med UML komponent- og deployment-, og systemsekvensdiagrammer, således at delsystemer kan bygges på basis af arkitekturen.
> - **FASE III: Delsystemudvikling og test.** I denne fase anvendes en iterativ arbejdsmetode til implementering af komponenter indenfor det enkelte delsystem. Parallelle arbejdsprocesser understøttes ved hjælp af Versionsstyring, Git, og feature-branches …
>
> …

### 2.3 Risikohåndtering

Her beskrives hvordan risici og prioritering af krav håndteres gennem projektets levetid. Dette kan f.eks. være ved hjælp af en risikomatrice, der benyttes til at prioritere krav i løbet af projektet. Risici kan være af forskellig karakter: Arbejdsmæssige, kravsmæssige (er krav F1.2 mulig at opfylde eller måle?) og tekniske. Tekniske risici kan eksistere både på arkitektur- (kan arkitekturen rumme projektets løsning?) og på designniveau (kan valgte implementering opfylde performance krav?). Risikomatrice og kravsprioritering håndteres typisk i et separat dokument/tabel/etc. som opdateres løbende i projektets forløb. Krav kan prioriteres nummerisk eller ved hjælp af MoSCoW.

> **Eksempel: Risikohåndtering**
>
> Identificerede risici dokumenteres og vurderes i en risikomatrice, der vedligeholdes i et regneark og løbende opdateres i projektet. Særlig opmærksomhed rettes mod arkitekturkritiske risici, da de har direkte indflydelse på centrale designbeslutninger. De fremhævede arkitektur-risici anvendes i Kapitel 4 som baggrund for valg af systemarkitektur.

### 2.4 Brug af AI

Brugen af AI beskrives ved hjælp af 4 metrikker baseret på *Framework for AI Fluency*. Der forefindes undervisningsmateriale (videoer mm.) omkring frameworket på denne side: https://aifluencyframework.org/. Indholdet fra dette afsnit kan kopieres ind i AU's AI Deklaration, som **SKAL** afleveres som bilag til projektet, såfremt AI har været anvendt.

#### 2.4.1 Uddelegering af arbejde

"Delegation refers to the ability to identify when and how to use AI tools and modalities effectively in creative and problem-solving processes. It involves understanding the capabilities and limitations of various AI technologies and making informed decisions about when to use AI for automation, augmentation, or independent agent-mediated experiences." [1]

#### 2.4.2 Fremgangsmåde

"Description encompasses the skills needed to effectively communicate ideas, requirements, constraints, and other aspects of creative visions to AI systems. It involves crafting clear, specific, and well-structured prompts (using a wide range of prompting techniques) and other elements that guide and enable AI tools to produce desired behaviors and outputs." [1]

#### 2.4.3 Bedømmelse af genererede output

"Discernment involves the critical evaluation of AI-generated outputs, understanding their quality, relevance, potential biases, and other salient characteristics. It also includes the ability to iterate and refine the collaborative process with AI tools." [1]

#### 2.4.4 Ansvarlighed og transparens

"Diligence refers to the responsible use of AI, including ethical considerations, transparency about AI use, and taking accountability for the final products created with AI assistance." [1]

> **Eksempel: Brug af AI**
>
> **Uddelegering af arbejde** — AI bruges til at effektivisere opgaver, hvor gruppen allerede har viden, men begrænses på områder, hvor læring er et mål i sig selv.
> - Backend: AI bruges primært til inspiration til den indledende arkitektur. Gruppen står selv for implementering, tilpasning og omstrukturering.
> - Frontend: Da GUI ikke er et centralt læringsmål, bruges AI i høj grad til udvikling af brugergrænsefladen.
>
> **Fremgangsmåde** — Der formuleres detaljerede prompts og der stilles opklarende spørgsmål. Antagelser minimeres og AI promptes eksplicit til at beskrive hvert trin i sin proces. Dette giver bedre kontrol over retningen og gør det muligt at opdage misforståelser, før der genereres output.
>
> **Bedømmelse af genererede output** — AI'ens arbejdsproces evalueres løbende, ligesom det endelige resultat. Output skal kunne forklares og forstås, før det anvendes. Samtalerne holdes korte og nulstilles om nødvendigt undervejs for at undgå hallucinationer og fejlagtige output. Dette kan opstå ved lange samtaler.
>
> **Ansvarlighed og transparens** — Der anvendes flere AI-værktøjer:
> - Claude Code: Bruges til kodearbejde direkte i terminalen.
> - Gemini: Bruges til inspiration til den indledende struktur.
> - Figma: Bruges til hurtig GUI-prototyping.
> - Claude Design: Bruges til videreudvikling af GUI efter import af Figma-designs.
>
> Der prioriteres sikker og ansvarlig AI-anvendelse. AI får ikke adgang til følsomme filer som .env, og alt genereret indhold gennemgås og valideres, før det anvendes.

> s. 8–10

## Kapitel 3 — Brugerkrav

Dette kapitel beskriver kravene til systemet fra brugerperspektiv. Det indleder med at præsentere konteksten for systemet og dets brugere. Herefter udledes, analyseres og specificeres de funktionelle og ikke-funktionelle krav. Endelig indeholder kapitlet en model af systemet beskrevet i brugerens termer. Dette kapitel bør være læsbart for en ikke-teknisk domæneekspert. Rapporten bør præsentere udvalgte krav og henvise til et separat kravspecifikationsdokument for resten. Hvis krav eller prioriteter har ændret sig i løbet af projektet, kan dette diskuteres i diskussionskapitlet. Husk at kravene skal være specifikke nok til at man kan udvikle en løsning på basis af disse, men ikke så specifikke, at der tages beslutninger om teknologi og design, med mindre at eksterne krav dikterer dette. **(2-4 sider)**

### 3.1 Introduktion

*(tom i skabelonen)*

### 3.2 Systemkontekst og aktører

Dette afsnit beskriver systemets kontekst fra et bruger- og domæneperspektiv. Det identificerer aktørerne, der interagerer med systemet, og det miljø, som systemet opererer i. Formålet er at forstå, hvem der bruger systemet, og hvem systemet interagerer med. Et aktør-kontekstdiagram kan være nyttigt her.

> **Eksempel: Systemkontekst og aktører**
>
> Systemet skal kunne opstilles ved ikke-betjente udskænkningssteder, f.eks. i cafeteriet i den lokale sportshal. Systemet har to aktører:
>
> - **Kunden** kan således forventes at være i alderen 15-99 år, selvstændig, og i stand til at bestille og betale en drik i almindelig betjent café.
> - **Administrator** som redigerer drikke og vedligeholder systemet

### 3.3 Funktionelle krav

#### 3.3.1 Elicitering og analyse

Funktionelle krav udledes ved at analysere interaktionerne mellem aktører og systemet. Forklar hvordan kravene er udledt (f.eks. interview), og vis en oversigt (diagram eller korte beskrivelser).

#### 3.3.2 Specifikation

På basis af analysen beskrives systemets adfærd fra et brugerperspektiv. Kravene struktureres som Use Cases, User stories, eller på listeform. Kravene bør være nummererede.

> **Eksempel: Funktionelle krav — Elicitering og analyse**
>
> Den primære aktør, brugeren, forventes i stand til at bestille og betale en drik i almindelig betjent café. Bestillingsflow skal således minde om det som man kender fra betjente caféer: Vis menukort, vælg drik, betal og modtag bestilling. Da systemet skal køre autonomt, er det vigtigt at det løbende monitorerer bestilling og opskænkning, og agerer på anomali. F.eks. at opfyldning stopper straks, hvis glasset fjernes eller overfyldes.
>
> **Specifikation**
>
> - **UC1: Bestil drik** — En kunde tilgår Drinkbot 4000 via webklienten og får vist en liste over tilgængelige drikke. Kunden vælger en drik og afgiver en bestilling, hvorefter den samlede pris vises. Kunden identificerer sig ved at føre sin mobiltelefon med aktivt kreditkort hen til RFID-læseren. Systemet validerer betalingen hos banken, og ved godkendelse igangsættes opskænkning af den valgte drik.
> - **UC2: Skænk drik** — Systemets vægt måler vægten af det tomme glas og aktiverer den relevante doseringsenhed. Drik påfyldes, indtil den ønskede mængde er opnået baseret på løbende vægtmålinger, hvorefter opskænkningen stoppes.

### 3.4 Ikke-funktionelle krav

#### 3.4.1 Elicitering og analyse

Ikke-funktionelle krav identificeres ud fra kvalitetskrav og begrænsninger i problemdomænet. Disse omfatter *illities* som f.eks. ydeevne, skalerbarhed og pålidelighed, der påvirker systemets design (f.eks. Kameraet skal filme persons gang (2 m/s) ad 10 meter sti ⇒ krav: filmklip længde skal være minimum 5 sekunder. Der må maksimalt være fejl i 1 sekund af visning af en 2-timers film ⇒ Oppetid = 1 − 1/(2·60·60) = 99.99% af tiden).

#### 3.4.2 Specifikation

De konkrete krav. Kan være grupperede, og skal være nummererede.

> **Eksempel: Ikke-funktionelle krav — Elicitering og analyse**
>
> Systemet skal kunne opstilles f.eks. i cafeterier i den lokale sportshal. Ved arrangementer med 200 deltagere skal alle kunne få skænket en drik hver time. En drik er typisk 0,5 liter, dvs. den skal kunne servere 100 liter i timen og håndtere 200 transaktioner. Tidsbudgettet ser således ud:
>
> - T_kunde = T_betjening + T_kortvalidering + T_skænkning (3.1)
> - T_kunde,max = 60 [min] / 200 [pers] = 0,3 [min] = 20 [sek] (3.2)
> - T_skænkning = T_kunde − T_betjening + T_kortvalidering (3.3) *(sic — som trykt i skabelonen)*
> - Antages: (3.4)
> - T_betjening = 12 [s], T_kortvalidering = 5 [s] (3.5)
> - T_skænkning = 20 − 12 − 5 = 3 [sek] (3.6)
>
> **Specifikation**
>
> - **NF1: Brugervenlighed** — 9 ud af 10 slutbrugere skal kunne betjene systemet på 12 sekunder (+/- 1 s), med henblik på at få leveret en drik.
> - **NF2: Performance** — Systemet skal validere kreditkort indenfor 5 sekunder, antaget at eksternt pengeinstitut overholder standard XYZ.
> - **NF3: Performance** — Systemet skal kunne skænke en 0,5 liter drik indenfor 3 sekunder.
> - **NF4: Performance** — Drik skal skænkes med en præcision på 2 ml (+/- 0.5 ml).
> - …

### 3.5 Systemets domænemodel

Dette afsnit beskriver systemets funktionelle enheder og deres relationer fra et brugerperspektiv. Det er den sidste brugerorienterede del. Herefter bliver det teknisk i de følgende kapitler. En UML-domænemodel er nyttig her. Husk at domænemodellen udledes direkte fra kravene og beskriver funktionalitet, data og ydre grænser for systemet som helhed, men at den ikke forholder sig til teknologi og interne interfaces! Det kommer først i arkitektur og design!

> **Eksempel: Domænemodel**
>
> Systemets domænemodel i Figur 3.1 viser systemets vigtigste entiteter, fysiske såvel som abstrakte. Det er valgt at `Ordre` både har ansvar for den valgte drik og for selve skænkningen.
>
> *Figur 3.1: Eksempel: domænemodel ("Domain Model IT4: With Stereotypes").* Aktør **kunde** samt begreberne Webklient, Ordre (attributter: RFID, Drik, Vægtmåling, Beløb), Katalog, Drik (attributter: Pris, doseringsenhed), Glas, og «boundary»-klasserne RFID Læser, Vægt, Bank, Doseringsenhed. Navngivne, rettede associationer:
>
> | Fra | Association | Til |
> |---|---|---|
> | kunde | interagerer med | Webklient |
> | kunde | lægger Mobil på | «boundary» RFID Læser |
> | kunde | stiller | Glas |
> | Webklient | opretter / starter | Ordre |
> | Webklient | vælger Drik fra | Katalog |
> | Katalog | viser | Webklient |
> | Ordre | læser RFID | «boundary» RFID Læser |
> | Ordre | måler mængde | «boundary» Vægt |
> | Ordre | styrer | «boundary» Doseringsenhed |
> | Ordre | validerer med | «boundary» Bank |
> | Ordre | indeholder | Drik |
> | Katalog | har | Drik |
> | Drik | påfyldes med | «boundary» Doseringsenhed |
> | Vægt | vejer | Glas |
> | Doseringsenhed | fylder | Glas |

> s. 11–14

## Kapitel 4 — Systemarkitektur

Dette kapitel præsenterer systemets overordnede arkitektur fra et strukturelt perspektiv. Det definerer først systemets grænser og relationer til eksterne systemer, hvorefter den interne struktur og grænseflader beskrives sammen med centrale teknologivalg. **(2-4 sider)**

### 4.1 Introduktion

*(tom i skabelonen)*

### 4.2 Systemets ydre grænser og relationer

Dette afsnit identificerer, hvad der er systemet, hvor dets grænser går og hvad dets eksterne relationer er. Dette er de ydre grænser for den arkitektur, som vi giver flere detaljer i de følgende afsnit. Relevante diagrammer i dette afsnit kan være et simpelt, overordnet UML Package-diagram baseret på domænemodellen, der fremhæver systemet og dets grænser, eller et System Context Diagram, hvis C4 anvendes.

> **Eksempel: Systemets ydre grænser og relationer**
>
> Systemets kontekst er præciseret i Figur 4.1. Kunde og Admin tilgår systemets software via en Webklient, men har også begge to behov for at tilgå de tilsluttede hardwarekomponenter. Softwaresystemet tilgår Banken via sin Backend Service.
>
> *Figur 4.1: Eksempel: et detaljeret context diagram ("Detailed Context Diagram").* Aktørerne **Admin** og **Kunde** har pile til pakken **SW System** (indeholder Webklient og Backend) og til pakken **Internal Boundaries** (RFIDReader, Vægt, Doseringsenhed). Backend har pil til pakken **External Boundaries** (Bank).

### 4.3 Analyse af arkitektur

Dette afsnit identificerer arkitektur-kritiske risici og foreslår arkitekturelle løsninger som reducerer disse. Identifikationen af risici kan basere sig på at (a) finde ikke-funktionelle krav, som den indledende domænemodelbaserede arkitektur ikke opfylder, og (b) identificere arkitekturkritiske teknologier (softwareframeworks, grænseflader mm.). Kritiske risici reduceres ved tilpasning af arkitekturen, grænseflader eller ved valg af teknologi. Analysen indeholder typisk argumenter baseret på litteratur eller små eksperimenter (dokumenteret i bilag). Den resulterende arkitektur præsenteres i det næste afsnit.

> **Eksempel: Analyse af arkitektur**
>
> **Skænkningshastighed**
> De ikke-funktionelle krav NF3 om hurtig opskænkning og NF4 om præcision, stiller krav til reguleringsloopet ved dosering af drikken. For at kunne understøtte disse, skal reguleringssløjfen kunne reagere indenfor kort tid:
>
> - T_skænkning,max = 3 [sek] (4.1)
> - V_fejl,max = 0,002 [l] (4.2)
> - Flow_min = 0,5 [l] / 3 [s] = 0,17 [l/s] (4.3)
> - T_it,max = 0,002 [l] / 0,17 [l/s] = 12 [ms] (4.4)
>
> En multithreaded Linux applikation på en Raspberry Pi vil have svært ved at imødekomme dette krav og vil dermed udgøre en høj teknisk risiko. Forsøg med en microcontroller uden OS har vist gode resultater (Bilag X), og systemet opdeles derfor i to delsystemer, Backend og Skænkning, hvor den sidste del afvikler den tidskritiske regulering af doseringen. Den tekniske risiko reduceres herved til et acceptabelt niveau.

### 4.4 Systemets struktur og grænseflader

Her præsenteres den valgte arkitektur, som resultat af analysen i forrige afsnit. Arkitekturen beskriver systemets overordnede struktur, og de delsystemer som den består af. Den suppleres med beskrivelser af grænseflader, protokoller og datamodeller, der deles af delsystemerne. Med viden om den overordnede struktur, fælles datamodeller og hvordan der kommunikeres mellem delsystemerne, har vi nu et solidt grundlag for at udvikle delsystemerne i næste kapitel. Nyttige diagrammer er lagdelte UML Package-diagrammer, UML Deployment-diagram eller et C4 Container-diagram.

> **Eksempel: Systemets struktur og grænseflader**
>
> Systemets arkitektur er baseret på inputtene fra vores analyse af de ikke-funktionelle krav. Kravene til opskænkningshastighed har resulteret i en distribueret løsning med to separate delsystemer, som vist i Figur 4.2.
>
> *Figur 4.2: Eksempel: arkitektur (package diagram) — "Package Diagram IT2: Backend divided into two Apps".* Pakker: **WebClient** (WebClient); **Backend Application** (SalgsService + underpakke Boundary: RFIDAdapter, SkænkningsAdapter, BankGateway); **Skænkningsapplikation** (SkænkningsStyring + underpakke Boundary: SkænkningsAdapter, VægtAdapter, DoseringsenhedAdapter); **External Systems / Boundaries** (RFIDReader, Bank, Skænkningsinterface, Vægt, Doseringsenhed). Adapterne har afhængigheder ned til de tilsvarende eksterne boundaries; begge applikationers SkænkningsAdapter peger på Skænkningsinterface. Note på diagrammet: backend-app'en er opdelt i to apps pga. realtidskrav til doseringsloop; de kommunikerer via et "skænkningsinterface".
>
> Delsystemerne deployeres som vist i Figur 4.2 *(sic; menes 4.3)*, således at `Backend applikation` og `Webserver` afvikles på en Linux-baseret embedded enhed, skænkningsapplikationen afvikles på en microcontroller og endelig afvikles `Web Client` i en browser på brugerens PC eller mobile enhed. Detaljer om designvalg kan findes i de respektive designafsnit i kapitlerne X.x og Y.y.
>
> *Figur 4.3: Eksempel: allokering af arkitektur (deployment diagram) — "Deployment Diagram IT2: System Allocated to Devices".* Noder: **Mobile Device (Browser)** med WebClient — «protocol» HTTP/TCP/IP → **Raspberry Pi 12** (Raspberry Pi OS) med «Web Server» Apache (index.html, main.js) — REST → «artifact» Backend Application. Fra Raspberry Pi: «protocol» Bankprotokol/TCP/IP → **Bank System** (Bank); «protocol» RFID-protokol/I2C → **RFID Reader** (RFID); «protocol» skænkningsprotokol/UART → **Arduino** (Skænkningsapplikation). Fra Arduino: «analog» 5V → **Vægt Sensor** (Vægt); «digital» IO → **Doserings Device** (Doseringsenhed). Note på diagrammet om at de specifikke devices/noder er "bedste bud nu" og opdateres senere.

#### 4.4.1 Interaktion mellem delsystemer

Her beskrives, hvordan delsystemerne interagerer og udveksler data under drift. Gerne med et UML System Sequence-diagram for hver relevant Use Case / User Story.

> **Eksempel: Interaktion mellem delsystemer**
>
> Med udgangspunkt i UC1 beskriver Figur 4.4 interaktionen mellem de to delsystemer og grænseflader. `Backend App` og `Skænknings App` kommunikerer via `Skænkningsinterface` som er markeret med grøn.
>
> *Figur 4.4: Eksempel: systemsekvensdiagram ("System Sequence Diagram").* Lifelines: User, «process» Web Client, «process» Backend App, «boundary» RFID Reader, «boundary» Bank, «boundary» Skænknings Interface (grøn), «process» Skænknings App, «boundary» Vægt, «boundary» Doseringsenhed. Forløb (læst fra det lille render): User vælger drik og starter køb i Web Client → Web Client sender POST /sale til Backend App → drikliste hentes/vises → User holder mobil mod læser → Backend læser RFID hos RFID Reader → Backend validerer kort hos Bank → OK/fejl. `alt [betaling OK]`: Backend sender Start Skænkning over Skænknings Interface til Skænknings App; `loop [indtil drik er skænket]`: Skænknings App måler vægt (Vægt) og skænker drik (Doseringsenhed); status returneres til Backend; HTTP success til Web Client. `[betaling fejl]`: HTTP error. Afslutning: `alt [betaling og skænkning OK]`: "Værsgo, din drik er klar!" til User; `[fejl]`: "Fejl under bestilling af drik".

#### 4.4.2 Interfaces og protokoller

Her beskrives detaljer om de grænsefladeteknologier og protokoller, som er nævnt i UML Deployment-diagram eller C4 Container-diagram.

> **Eksempel: Interfaces og protokoller**
>
> **Skænkningsinterface**
> På basis af analysen benytter dette interface et UART fysisk lag, med en baud rate på 115200 og en protokol som beskrevet i Figur 4.5.
>
> *Figur 4.5: Eksempel: protokol (Skænkningsinterface) — "Skænkningsprotokol".* Ramme: `preamble 0x55 | payload | postamble 0x88`. Payload er en af tre pakketyper:
>
> | type | length | data |
> |---|---|---|
> | start (0x00) | 2 [bytes] | mængde [g] |
> | stop (0x01) | 0 [byte] | «» |
> | status (0x02) | 1 [byte] | okay (0x12) \| dead (0x34) |
>
> **Webinterface**
> Interfacet mellem `Webclient` og `Backend` benytter sig af REST over HTTPS. Ved et salg sendes en HTTP/POST fra `Webclient`, hvorpå der modtages en response fra `Backend`. Formatet af JSON-beskederne er vist i henholdsvis Figur 4.6 og Figur 4.7.
>
> *Figur 4.6: Eksempel: protokol (REST sale POST) — "Sale JSON (POST /sale)":* `sale { drinkId: integer, amount: double, rfid: string, timestamp: string (ISO-8601) }`.
>
> *Figur 4.7: Eksempel: protokol (REST sale Response) — "Sale Response JSON (OK)":* `sale { status: string ("OK" | "FAILED"), orderId: int, drink: { id: int, name: string, pricePerLiter: double }, amount: double (liters), totalPrice: double, timestamp: string (ISO-8601) }`, `error { code: int, message: string }`.

#### 4.4.3 Datamodeller og persistering

Hvis relevant beskriver dette afsnit datamodeller, persisteringsmetoder, og teknologier som anvendes på tværs af arkitekturen (f.eks. SQL/NoSQL/filbaseret). Relevante diagrammer kan f.eks. være et ER-diagram.

> **Eksempel: Datamodeller og persistering**
>
> **Logging**
> Logs gemmes som tekstfiler og anvender følgende format:
>
> ```
> dd:mm:åå tt:mm:ss - <log niveau> - <klassenavn::funktion> - <besked>
> ```

> s. 15–21

## Kapitel 5 — Design og implementering

Dette kapitel beskriver design og implementering af systemets delsystemer. Det præciserer hvordan indholdet i arkitekturen realiseres gennem software- og/eller hardwarekomponenter, herunder designbeslutninger, modeller og implementeringsdetaljer. Kapitlet danner grundlaget for den efterfølgende test og evaluering af systemet. Bemærk at kapitlet bruger en top-down approach, hvor hvert delsystem fra arkitekturen beskrives i separate afsnit, som der efterfølgende bores ned i og åbnes op. Hvis et delsystem har flere underniveauer (f.eks. Container-Component-Class), kan disse om nødvendigt bores ned i gennem yderligere underafsnit. **(4-10 sider)**

### 5.1 Introduktion

*(tom i skabelonen)*

### 5.2 Delsystem \<delsystem navn\>

#### 5.2.1 Introduktion

Dette afsnit introducerer et specifikt delsystem og de komponenter, det består af. I det følgende er givet eksempler på indhold til komponenter af forskellig type. Vær opmærksom på at du alene skal designe dette delsystems interaktion med andre delsystemer udfra det som du kan læse i arkitekturen! Dvs. ingen detaljer om andre delsystemer i dette afsnit!!

**5.2.1.1 Struktur af delsystem** — Her viser du hvad som delsystemet består af og hvordan det er internt forbundet. Beskrivelsen skal kunne stå alene og må ikke indeholde komponenter og logik fra andre delsystemer, vi kender kun interfacet til dem (via arkitekturen). For software-delsystemer med flere abstraktionsniveauer kan du bruge et C4 Component Diagram eller et UML Component Diagram og bore ned i disse i underafsnit. For et simplere system med færre abstraktionsniveauer kan du nøjes med et UML Class Diagram. Hardware kan præsenteres ved hjælp af et overordnet SysML Internal Block Diagram, eventuelt med visning af software-allokering.

**5.2.1.2 Interaktion mellem komponenter** — Her vises hvordan delsystemets komponenter interagerer, gerne med udgangspunkt i Use Cases/User Stories, og ved hjælp af UML Sequence Diagrams. Til software, der er mere dataflow-orienteret, f.eks. algoritmer, functional programs eller DSP-kode, som ikke er objektorienteret, kan du benytte UML Activity Diagrams.

#### 5.2.2 Design og implementering af \<sw komponent navn\>

Afsnittet introducerer en specifik SW-komponent, dens design, implementering og enhedstest. Nyttige diagrammer her er UML Class Diagrams, UML Sequence Diagrams, UML State Diagrams, UML Activity Diagrams.

#### 5.2.3 Design og implementering af \<hw komponent navn\>

Afsnittet introducerer en specifik HW-komponent, dens design, implementering og enhedstest, herunder beregninger, PCB Layout, EMC-målinger mm. Nyttige diagrammer her er UML State Diagrams, Hardware Schematic Diagram.

#### 5.2.4 Design og implementering af \<database komponent navn\>

Eksempel på en specifik komponenttype, som beskrives med specifikke modeller: Databaseskema (tabeller, PK/FK, begrænsninger), og som har specifikke overvejelser om fysisk design, indeksering, performanceovervejelser, ORM-modeller, migrationer.

#### 5.2.5 Tests og resultater

Resultater fra enhedstests, og integrationstests mellem komponenter, inklusive testmetrikker, hvis tilgængelige.

#### 5.2.6 Delkonklusion

Eventuel opsummering på delsystem.

> **Eksempel: Backend Delsystem**
>
> **Introduktion**
> Backend delsystemet har ansvaret for afvikling af salg og kommunikation med Skænkningsenheden.
>
> **Struktur af delsystem**
> Det centrale ansvar for koordineringen mellem klasserne er placeret i `SalgsService`, hvilket fremgår af klassediagrammet i Figur 5.1. Håndteringen af REST-endpoints er placeret i en separat klasse, `RestController`, som kommunikerer med `SalgsService` via events. Denne opdeling reducerer koblingen mellem komponenterne og øger samhørigheden.
>
> *Figur 5.1: Eksempel: klassediagram ("Class Diagram of Backend App UC1 + UC2", pakke Backend Application).* Klasser (læst fra lille render; detaljer omtrentlige):
>
> | Stereotype | Klasse | Attributter | Operationer |
> |---|---|---|---|
> | «boundary» | RESTController | | +opretSalg() «POST /sale», +hentKatalog() «GET /drinks» |
> | «control» | SalgsService | -ordrer[]: Ordre, -katalog: Katalog, -bank: BankGateway, -skænkningAdapter: SkænkningsAdapter | +salg(Drik), +hentKatalog() |
> | «boundary» | RFIDAdapter | -fdDescriptor: int | +læsRFID(): string |
> | «domain» | Ordre | -rfid: string, -drik: Drik, -vægt: double, -beløb: double | |
> | «boundary» | BankGateway | | validerKort(rfid: string): int |
> | «domain» | Katalog | -drikListe: vector\<Drik\> | +tilføjDrik(drik: Drik), +fjernDrik(drik: Drik), +listDrik(): vector\<Drik\> |
> | «boundary» | SkænkningsAdapter | -fdDescriptor: int | +startSkænkning(ordre: Ordre): int, +stopSkænkning(ordre: Ordre): int, +getStatus(): int |
> | «domain» | Drik | -pris: double, -lagerbeholdning: double, -doseringsenhed: int | |
>
> RESTController er associeret til SalgsService; SalgsService er associeret til RFIDAdapter, Ordre, BankGateway, Katalog og SkænkningsAdapter; Ordre og Katalog er associeret til Drik.
>
> **Interaktion mellem komponenter**
> Som vist i Figur 5.2 interagerer `SalgsService` via adapterklasser med eksterne devices og aktører.
>
> *Figur 5.2: Eksempel: sekvensdiagram ("Sequence Diagram Use Case 1: Bestil Drik").* Lifelines: User, WebClient, RESTController, SalgsService, RFIDAdapter, BankGateway, SkænkningsAdapter, «interface» RFIDReader, «interface» SkænkningsApp, «interface» Bank. Forløb: User vælger drik + start køb → WebClient: POST /sale(drikId) → RESTController: salg(drikId) → SalgsService; User holder mobil mod læser; SalgsService: læsRFID() → RFIDAdapter → read (I2C) → RFIDReader → rfid retur; SalgsService: validerKort(rfid) → BankGateway → request validation (SSL) → Bank → OK / fejl. `alt [betaling ok]`: SalgsService: startSkænkning() → SkænkningsAdapter → start (UART) → SkænkningsApp; `loop [indtil færdig]`: getStatus() → status (UART); status retur; success → HTTP response → "Værsgo, din drik er klar!". `[betaling fejl]`: fejl → HTTP response.
>
> **Design og implementering af SalesService**
> `SalesService` implementerer logikken beskrevet i `UC1` og `UC2`. Designet er baseret på state machinen vist i Figur 5.3.
>
> *Figur 5.3: Eksempel: statediagram ("State Machine Diagram – SalgsService (UC1: Bestil Drik)").* Læst fra lille render:
>
> ```mermaid
> stateDiagram-v2
>     [*] --> Idle
>     Idle --> VenterPaaRFID : salg(Drik)
>     VenterPaaRFID : Venter på RFID
>     VenterPaaRFID : entry / RFIDAdapter.læsRFID()
>     VenterPaaRFID --> ValidererBetaling : RFID modtaget
>     ValidererBetaling : Validerer betaling
>     ValidererBetaling : entry / BankGateway.validerKort(rfid)
>     ValidererBetaling --> SkaenkerDrik : betaling OK
>     ValidererBetaling --> Fejl : betaling afvist
>     SkaenkerDrik : Skænker drik
>     SkaenkerDrik : entry / SkænkningsAdapter.startSkænkning()
>     SkaenkerDrik --> SkaenkerDrik : getStatus() [status != færdig]
>     SkaenkerDrik --> FuldfoererOrdre : status == færdig
>     FuldfoererOrdre : Fuldfører ordre
>     FuldfoererOrdre : entry / opret Ordre, opdater lager, returnér succes
>     Fejl : entry / returnér fejl
>     FuldfoererOrdre --> Idle
>     Fejl --> Idle
> ```
>
> **Design og implementering af hardware**
> Den overordnede struktur for hardwaren er vist i Figur 5.4 og består af tre overordnede komponenter: Processor board, RFID læser og en strømforsyning.
>
> *Figur 5.4: Eksempel: Intern blokdiagram ("IBD Backend Enhed").* Parts: **Power Adapter 21W** — port USB C (5V) → **Processor Module (Raspberry Pi 12)** med ports USB C, WIFI, 3.3V (exp/3V3), I2C (exp/i2c-2), UART (exp/uart1). Processor Module → **RFID Læser (Boxxon XQ21)** via 3.3V og I2C; Processor Module → **Skænkningsenhed** via UART.
>
> **Processorboard** — Der er valgt en Raspberry Pi 12 fordi … (analyse her eller i bilag).
>
> **RFID Læser** — Jfr. krav NF47, skal RFID læseren kunne læse kort af typen qi43 fra en afstand af maksimalt 3 centimeter. Ud fra undersøgelsen af læsere (bilag TT), er det valgt at anvende *Boxxon XQ21*, som opfylder givne krav og som kan tilsluttes Processorboardet via I2C.
>
> **Strømforsyning** — Strømforsyningen er valgt ud fra krav (ref) til salgsområde og baseret på målinger af strømforbrug (Bilag ZZ). Målingerne viste at under maksimal belastning kan delsystemet bruge op mod 15 Watt. (ref) anbefaler desuden at anvende en 21 W power adapter. Strømforsyning xyz21 eller tilsvarende kan derfor anvendes.
>
> **Tests og resultater**
> For at verificere implementeringen af backend-delsystemet er der udført enhedstests samt interne integrationstests mellem backend-komponenterne. Testene fokuserer på funktionaliteten i `SalgsService` og dets interaktion med de tilhørende boundary-klasser.
>
> *Enhedstest af SalgsService* — Enhedstestene verificerer den centrale forretningslogik i `SalgsService`, herunder oprettelse af ordrer, håndtering af katalogdata og validering af salgsflowet. Eksterne afhængigheder såsom `BankGateway` og `SkænkningsAdapter` er erstattet med mock-objekter for at isolere testene til backend-logikken.
>
> *Tabel 5.1: Eksempel: Resultater af enhedstest for backend*
>
> | Test | Forventet resultat | Status |
> |---|---|---|
> | Opret salg med gyldig drik | Ordre oprettes korrekt | ✓ |
> | Hent katalog | Liste af tilgængelige drikke returneres | ✓ |
> | Salg med ugyldigt RFID | Salg afvises | ✓ |
> | Salg ved afvist betaling | Skænkning initieres ikke | ✓ |
>
> *Intern integrationstest* — Der er udført integrationstest mellem backend-komponenterne for at verificere, at de interne interfaces fungerer korrekt. Testene omfatter blandt andet kommunikationen mellem `RESTController` og `SalgsService` samt integration mellem `SalgsService`, `BankGateway` og `RFIDAdapter`.
>
> *Tabel 5.2: Eksempel: Resultater af intern integrationstest i backend*
>
> | Interface | Test | Status |
> |---|---|---|
> | RESTController → SalgsService | Salgsrequest videresendes korrekt | ✓ |
> | SalgsService → RFIDAdapter | RFID-data modtages korrekt | ✓ |
> | SalgsService → BankGateway | Valideringsresultat håndteres korrekt | ✓ |
> | SalgsService → Katalog | Drikdata kan hentes korrekt | ✓ |
>
> *Testmetrikker* — Der er gennemført i alt 8 automatiserede tests for backend-delsystemet. Alle tests bestod. Testdækningen for de centrale klasser i backend var:
> - SalgsService: 92% line coverage
> - Katalog: 100% line coverage
> - RESTController: 85% line coverage
>
> **Delkonklusion**
> Backend delsystemet er opbygget med en Raspberry Pi 12, en XQ21 RFID læser, har et UART interface til Doseringssystemet og kommunikerer med Bankens API over Raspberry'ens WIFI forbindelse. Softwaren er centreret omkring `SalgsService` som håndterer salg og medierer information mellem Webklient og Doseringssystemet. Der er gennemført manuel end-to-end test med brug af simulerede udgaver af Bank API og Doseringssystem. Testresultaterne viser, at backendens centrale funktionalitet og interne komponentrelationer fungerer som forventet.

> s. 22–26

## Kapitel 6 — Integrationstest

Dette kapitel verificerer den valgte arkitektur og interaktionen mellem delsystemerne, med resultaterne af integrationstests. Der er tale om tekniske tests, som giver os sikkerhed for at interne krav er opfyldt. **(1-2 sider)**

### 6.1 Introduktion

*(tom i skabelonen)*

### 6.2 Teststrategi

Beskrivelse af integrationstest-strategi: Top-down, bottom-up, use-case drevet mm.

### 6.3 Integrationstest af use case 1

#### 6.3.1 Opstilling

Beskrivelse af forudsætningerne for at gennemføre testen. Hvilken opsætning? Værktøjer?

#### 6.3.2 Resultater

Beskrivelse af resultaterne?

### 6.4 Diskussion

Diskussion af resultaterne af integrationstestene.

### 6.5 Delkonklusion

Hvad kan konkluderes ud fra resultaterne?

> **Eksempel: Integrationstest af delsystemerne Webclient, Backend og Skænkningssystem**
>
> **Teststrategi**
> Integrationstesten fokuserer på kommunikationen mellem systemets tre primære delsystemer: Webclient, Backend og Skænkningssystemet. Interne integrationer i de enkelte delsystemer betragtes som verificeret i tidligere testkapitler. Testene udføres med en top-down tilgang og verificerer de definerede interfaces mellem komponenterne.
>
> **Testopstilling**
> Testen udføres på den samlede hardwareplatform bestående af en Raspberry Pi med Backend Application og en Arduino med Skænkningsapplikationen. Webklienten anvendes som input til backendens REST-interface, mens kommunikationen mellem backend og skænkningsapplikationen foregår via det definerede skænkningsinterface over UART.
>
> **Resultater**
> Resultaterne i Tabel 6.1 viser at alle integrationstestene er gennemført med godkendt resultat.
>
> *Tabel 6.1: Eksempel: Resultat af integrationstest mellem systemets delsystemer*
>
> | Interface | Test | Resultat |
> |---|---|---|
> | WebClient → Backend Application | REST-kald med salgsrequest modtages og behandles | Godkendt |
> | Backend Application → Skænkningsapplikation | Startkommando over skænkningsinterface modtages korrekt | Godkendt |
> | Skænkningsapplikation → Backend Application | Statusmeddelelser returneres efter endt skænkning | Godkendt |
> | Skænkningsapplikation → Doseringsenhed | Kommandoer til aktivering og stop videresendes korrekt | Godkendt |
> | Backend Application → WebClient | Resultat fra backend returneres som HTTP-respons | Godkendt |
>
> **Diskussion**
> Integrationstesten verificerer, at kommunikationen mellem systemets delsystemer fungerer via de definerede interfaces. Testene viser, at Backend kan initiere en skænkning, modtage status fra Skænkningsapplikationen og kommunikere resultatet tilbage til webklienten. De interne funktioner i de enkelte delsystemer er ikke omfattet af denne test.
>
> **Delkonklusion**
> Integrationstesten viser at interfaces mellem delsystemerne fungerer som specificeret og at de definerede interfaces og protokoller er tilstrækkelige til de nuværende behov.

> s. 27–28

## Kapitel 7 — Brugeraccepttest

Dette kapitel beskriver valideringen af systemet i forhold til brugerkravene. Det skitserer opstillingen af brugeraccepttesten og præsenterer resultaterne, idet det vurderes, om systemet opfylder sit tiltænkte formål. **(1-2 sider)**

### 7.1 Introduktion

#### 7.1.1 Opstilling

Hvad er forudsætningerne for at gennemføre testen? Opsætning? Værktøjer?

#### 7.1.2 Resultater

Hvad er resultaterne?

### 7.2 Diskussion

Diskussion af resultaterne af accepttestene.

### 7.3 Delkonklusion

Hvad kan konkluderes ud fra resultaterne?

> **Eksempel: Brugeraccepttest af Drinkbot 4000**
>
> **Introduktion**
> Brugeraccepttesten evaluerer systemet som en samlet løsning. Modsat tidligere enheds- og integrationstests fokuserer denne test på brugerens oplevelse og systemets evne til at opfylde de opstillede krav. Testen tager udgangspunkt i kravene beskrevet i kapitel K.
>
> **Opstilling**
> Testen blev udført på den komplette Drinkbot 4000 prototype og udført sammen med en repræsentant fra YY. Tidsmålinger blev foretaget med en ekstern timer, mens doseringspræcisionen blev evalueret ved hjælp af vægtmålinger.
>
> **Resultater**
>
> *Tabel 7.1: Eksempel: Resultater fra brugeraccepttest*
>
> | Krav | Test | Resultat | Status |
> |---|---|---|---|
> | UC1 | Bruger bestiller drik gennem webklient og gennemfører betaling | Bestilling gennemført | ✓ |
> | UC2 | Systemet skænker valgt drik baseret på vægtmåling | Drik skænkes korrekt | ✓ |
> | NF1 | 10 brugere gennemfører en bestilling | 9/10 indenfor 12 sekunder | (✓) |
> | NF2 | Tid fra RFID-identifikation til betalingsresultat måles | 2,8 sekunder | ✓ |
> | NF3 | Tid for opskænkning af 0,5 liter måles | 2,6 sekunder | ✓ |
> | NF4 | Doseringspræcision måles ved gentagne skænkninger | Afvigelse ±0,4 ml | ✓ |
>
> **Diskussion**
> Testresultaterne viser, at Drinkbot 4000 opfylder de opstillede brugerkrav. Brugeren kan gennemføre hele processen fra valg af drik til færdig opskænkning, og systemets performance ligger indenfor de specificerede grænser. De funktionelle krav blev verificeret gennem komplette brugerflows, mens de ikke-funktionelle krav blev evalueret gennem tidsmålinger og kvantitative målinger af doseringspræcision. Testen af NF1 var begrænset af kun at være gennemført med en enkelt bruger og kan derfor ikke regnes for 100% gyldig.
>
> **Delkonklusion**
> Brugeraccepttesten viser, at systemet opfylder det tiltænkte formål som en automatisk drikkevareløsning. Alle testede krav blev opfyldt, og der blev ikke observeret fejl under gennemførelsen af de komplette brugerflows. Testen af NF1 var dog begrænset af antallet af forskellige brugere. Resultaterne indikerer, at den valgte arkitektur og implementering understøtter de funktionelle krav samt de opstillede krav til brugervenlighed og performance.

> s. 29–30

## Kapitel 8 — Diskussion

Dette kapitel diskuterer projektets resultater. I forbindelse med et udviklingsprojekt udgør resultater såvel krav, arkitektur, design og tests. Det giver derfor mening at diskutere bredere end blot at diskutere accepttestresultater. Man kan diskutere oppe- eller nedefra. Man kan f.eks. diskutere problemstilling versus accepttest, resultaterne af at anvende den valgte projektmodel og metode, resultaterne af at benytte den valgte arkitektur, resultaterne af designbeslutninger mm. Kapitlet diskuterer systemet i forhold til kravene og reflekterer over designbeslutninger, systemets ydeevne og begrænsninger. Diskussionen giver en kritisk vurdering af den samlede løsning. **(1-3 sider)**

### 8.1 Problemstilling og projektets løsning

Hvordan besvarer løsningen projektets problemstilling? Rammer den sit mål, eller er der gået teknik-creep i den? Har I måtte justere undervejs?

### 8.2 Proces og metode

Diskutér hvordan det er gået med den valgte proces og metode. Hvad har fungeret godt for projektet og ikke? Har I måtte justere?

### 8.3 Brugerkrav

Diskutér kravene. Var de velformulerede og testbare? Hvordan er de blevet prioriteret? Har nogle vist sig urealistiske?

### 8.4 Arkitektur

Har den valgte arkitektur kunne rumme design og implementering? Har den begrænsninger? Har I måtte ændre den markant undervejs?

### 8.5 Design

Gav designvalg anledning til begrænsninger/muligheder? Er det holdbare valg som understøtter fremtidigt arbejde?

> **Eksempel: Diskussion**
>
> Dette kapitel diskuterer projektets samlede resultater. Diskussionen vurderer ikke kun, om systemet fungerer, men også hvilke konsekvenser de valgte løsninger har haft for udvikling, vedligeholdelse og fremtidig udvidelse.
>
> **Problemstilling og projektets løsning**
> Den udviklede løsning adresserer problemstillingen beskrevet i kapitel I ved at kombinere en webbaseret brugerflade med en backend-applikation og en separat skænkningsapplikation. Under projektet blev der foretaget en afgrænsning af systemets funktionalitet. Eksempelvis blev fokus fastholdt på én central salgsproces fremfor at udvide systemet med yderligere brugerfunktionalitet. Dette har begrænset de tekniske risici og sikret fokus på de centrale krav. En udfordring ved løsningen er dog, at den samlede brugeroplevelse afhænger af flere eksterne komponenter såsom betalingssystem, RFID-læser og doseringshardware. Dette betyder, at systemets kompleksitet primært ligger i integrationen mellem komponenterne frem for i den enkelte softwarekomponent.
>
> **Proces og metode**
> Den valgte projektmodel har fungeret hensigtsmæssigt i projektet. Den indledende analysefase gav mulighed for at identificere centrale risici, før implementeringen blev påbegyndt. Særligt opdelingen mellem backend og skænkningsapplikationen blev identificeret som en vigtig arkitekturbeslutning tidligt i processen. Den iterative udviklingsproces i fase III gjorde det muligt løbende at afprøve designbeslutninger gennem implementering og test. Dette var særligt relevant for hardwareintegration, hvor den faktiske opførsel først kunne verificeres på den fysiske prototype. En begrænsning ved processen var, at hardwareafhængigheder medførte ventetid i udviklingen af softwarekomponenter. En tidligere etablering af mocks eller simulatorer kunne have reduceret denne afhængighed. Samlet set har den valgte proces understøttet projektets karakter, hvor både softwarearkitektur og fysisk integration skulle udvikles parallelt.
>
> **Brugerkrav**
> De funktionelle krav har fungeret godt som grundlag for både design og test. UC1 og UC2 beskriver tydeligt de centrale brugerinteraktioner og kunne direkte anvendes som udgangspunkt for sekvensdiagrammer og brugeraccepttest. De ikke-funktionelle krav har ligeledes været vigtige for arkitekturen, især kravene relateret til performance og præcision. Kravene medførte eksempelvis behovet for at separere skænkningsapplikationen fra backend-applikationen. Kravene har dog generelt været tilstrækkeligt konkrete til at understøtte designvalg og efterfølgende test.
>
> **Arkitektur**
> Den valgte arkitektur med en opdeling mellem backend og skænkningsapplikation har vist sig egnet til systemets krav. Opdelingen reducerer koblingen mellem forretningslogik og realtidsstyring, hvilket gør det muligt at udvikle og teste de to dele uafhængigt. Under udviklingen viste det sig, at arkitekturen kunne rumme de nødvendige designændringer uden større omstrukturering. Den oprindelige opdeling mellem delsystemerne blev derfor bevaret gennem hele projektet. En begrænsning ved arkitekturen er, at ændringer i kommunikationsprotokollen kræver koordinering mellem begge applikationer. Et mere avanceret kommunikationslag kunne i fremtiden reducere denne afhængighed.
>
> **Design**
> Flere designbeslutninger har haft betydning for systemets muligheder og begrænsninger. Anvendelsen af adapterklasser omkring eksterne enheder har bidraget til en lavere kobling og gjort det muligt at udskifte hardware uden at ændre den centrale forretningslogik. De valgte designprincipper vurderes derfor som hensigtsmæssige for den nuværende løsning og giver mulighed for fremtidig udvidelse, eksempelvis flere drikke, flere doseringsenheder eller alternative betalingsmetoder.

> s. 31–32

## Kapitel 9 — Konklusion

Dette kapitel afslutter rapporten ved at opsummere de vigtigste fund og projektets bidrag. Det reflekterer over, hvordan problemformuleringen er blevet adresseret, og fremhæver de centrale resultater af arbejdet. **(1-2 sider)**

### 9.1 Fremtidigt arbejde

Dette afsnit skitserer åbninger for fremtidigt arbejde i form af potentielle forbedringer og udvidelser. Den identificerer begrænsninger i den nuværende løsning og foreslår retninger for videre udvikling og forfining.

> **Eksempel: Konklusion**
>
> Dette kapitel opsummerer projektets resultater og vurderer, hvorvidt den udviklede løsning opfylder projektets formål og opstillede krav. Afslutningsvis beskrives relevante muligheder for fremtidigt arbejde.
>
> **Konklusion**
> Projektets formål var at udvikle et automatiseret drikkevaresystem, der gør det muligt for en bruger at bestille en drik via en webklient, gennemføre betaling og få drikken automatisk opskænket. Der er udviklet en samlet løsning bestående af en webklient, en backend-applikation og en separat skænkningsapplikation. Den valgte arkitektur har vist sig velegnet til systemets krav. Backend håndterer forretningslogikken, mens skænkningsapplikationen varetager den tidskritiske styring af doseringsprocessen. Denne opdeling har reduceret koblingen mellem systemets dele og gjort det muligt at udvikle og teste delsystemerne uafhængigt. Gennem enhedstests, integrationstests og brugeraccepttest er det verificeret, at systemets centrale funktionalitet fungerer som forventet. Brugeraccepttesten viste, at løsningen opfylder de opstillede krav til funktionalitet, betalingshåndtering, opskænkningstid og doseringspræcision. På baggrund af projektets resultater vurderes det derfor, at den udviklede løsning opfylder projektets formål om at realisere et fungerende automatiseret drikkevaresystem.
>
> **Fremtidigt arbejde**
> Selvom den udviklede prototype opfylder de opstillede krav, er der flere muligheder for videreudvikling. En naturlig udvidelse er at forbedre systemets skalerbarhed ved at understøtte flere doseringsenheder og flere samtidige brugere. Dette vil kræve yderligere udvikling af backendens håndtering af ordrer samt en mere avanceret kommunikationsarkitektur mellem delsystemerne. På hardware-siden kan doseringssystemet forbedres gennem mere præcise ventiler og sensorer, hvilket kan reducere variationen i doseringsresultaterne. Desuden kan skænkningsalgoritmen optimeres for at håndtere forskellige drikketyper med forskellige fysiske egenskaber. Endelig kan brugerinteraktionen videreudvikles med funktioner såsom brugerprofiler, historik over tidligere køb og mulighed for fjernadministration af drikkekataloget. Den nuværende arkitektur vurderes dog at danne et godt fundament for disse udvidelser, da ansvarsfordelingen mellem systemets delkomponenter allerede er tydeligt defineret.

> s. 33

## Bibliografi

- [1] Rick Danaik og Joseph Feller. *Artificial Intelligence at Ringling: Framework for AI Fluency*. Accessed: 2026-07-01. URL: https://ringling.libguides.com/ai/framework.
- [2] Steve Klabnik m.fl. *The Rust Programming Language*. Accessed: 2025-12-29. 2025. URL: https://doc.rust-lang.org/stable/book/.

> s. 34

## Kapitel 10 — Appendiks

### 10.1 Krav til rapport og supplerende materiale

En aflevering i forbindelse med et projekt består af en rapport (PDF) samt en række bilag, typisk pakket sammen i en zip-fil. Rapport og bilag afleveres som angivet i forbindelse med projektet på f.eks. Wiseflow eller Brightspace, hvor rapporten afleveres som den primære *Hovedopgave*, mens bilag afleveres som det sekundære *Supplerende materiale*.

#### 10.1.1 Antal tegn

Omfanget af projektrapporten må ikke overstige **30 normalsider** tekst. En normalside består af 2400 tegn med mellemrum, dvs. projektrapporten må fylde **72.000 tegn**. Forside, resumé/abstract, indholdsfortegnelse, referenceliste, bilagsfortegnelse, tegn i figurer og tabeller samt bilag tæller ikke med i de 72.000 tegn.

#### 10.1.2 Supplerende materiale

Rapporten kan suppleres med bilag med tekniske eller procesmæssige detaljer, som der ikke er plads til i hovedrapporten. Bilagene henvender sig til en teknisk kyndig på samme faglige niveau som forfatter og med disse samt rapporten, bør vedkommende være i stand til at kunne videreudvikle og/eller vedligeholde projektets produkt.

Eksempel på tekniske bilag:
- Kravspecifikation med alle funktionelle, ikke-funktionelle krav, samt prioritering.
- Risikomatrice som opdateres i løbet af projektet
- Analyserapporter som beskriver analyser og eksperimenter anvendt i forbindelse med arkitektur og design.
- Testdokumenter, herunder integrationstests, udført accepttest, usability-studier etc.
- Implementeringsspecifikke filer herunder kildekode, diagrammer, printudlæg, beregninger etc.
- Datakilder, herunder træningsdata, testdata mm.

Eksempel på procesrelaterede bilag:
- Procesbeskrivelse
- Samarbejdsaftale
- Mødereferater
- Tidsplaner, herunder den oprindelige og den endelige
- Logbøger

> s. 35–36

### 10.2 Sidebudget for rapport

*Tabel 10.1: Sidebudget for rapport*

| Kapitel | Sider |
|---|---|
| Introduktion | 2-4 |
| Udviklingsproces og metode | 1-2 |
| Brugerkrav | 4-6 |
| Systemarkitektur | 3-4 |
| Design og Implementering | 6-10 |
| Systemintegrationstest | 1-2 |
| Brugeraccepttest | 1-4 |
| Diskussion | 1-2 |
| Konklusion | 1-2 |
| **Total** | **30** |

> s. 37

### 10.3 Relevante rapportsektioner og diagrammer

#### 10.3.1 Software Engineering-projekt

*Tabel 10.2: Relevant sections and diagrams for Software Engineering Projects*

| Kapitel | Sektion | Relevante figurer |
|---|---|---|
| Introduction | Background | |
| Introduction | Existing Work | |
| Introduction | Conceptual Overview of System | Rich Picture |
| Introduction | Problem Statement | |
| Methodology and Development Process | Project Model | |
| Methodology and Development Process | Development Methods | |
| User Requirements | Actors and Use Case Context | UML Actor-context diag. |
| User Requirements | Requirements Elicitation and Analysis | |
| User Requirements | Requirements Specification | UML Use Case diag. |
| User Requirements | Requirements Prioritization | MoSCoW Matrix |
| User Requirements | Domain Model of the System | UML Domain Model |
| System Architecture | System Context and Scope | C4 Context Diag. |
| System Architecture | Technology and Risk Assessment | Risk Matrix |
| System Architecture | System Structure and Interfaces | C4 Container Diag., sys-level UML SD, ER diag. |
| Subsystem Design and Implementation | Subsystem X - Design and Implementation | C4 Comp diag of subsys., UML CD, SD, STM, code |
| Subsystem Design and Implementation | Subsystem X - Test and Results | Unit and local integr. test results and metrics |
| System Integration Testing | Integration Tests for Use Case Y | |
| User Acceptance Testing | Test Setup | |
| User Acceptance Testing | Results and Discussion | |
| Evaluation and Discussion | Discussion of Test Results | |
| Evaluation and Discussion | Discussion of System Architecture and Design | |
| Conclusion | Conclusion | |
| Conclusion | Future Work | |

> s. 38

#### 10.3.2 Embedded Software Engineering-projekt

*Tabel 10.3: Relevant sections and diagrams for Embedded Software Engineering Projects*

| Kapitel | Sektion | Relevante figurer |
|---|---|---|
| Introduction | Background | |
| Introduction | Existing Work | |
| Introduction | Conceptual Overview of System | Rich Picture |
| Introduction | Problem Statement | |
| Methodology and Development Process | Project Model | |
| Methodology and Development Process | Development Methods | |
| User Requirements | Actors and Use Case Context | UML Actor-context diag. |
| User Requirements | Requirements Elicitation and Analysis | |
| User Requirements | Requirements Specification | UML Use Case diag. |
| User Requirements | Requirements Prioritization | MoSCoW Matrix |
| User Requirements | Domain Model of the System | UML Domain Model |
| System Architecture | System Context and Scope | SysML BDD |
| System Architecture | Technology and Risk Assessment | Risk Matrix |
| System Architecture | System Structure and Interfaces | UML Deployment diag., sys-level UML SD + HW I/F and protocol desc. |
| Subsystem Design and Implementation | Subsystem X - SW Design and Implementation | Subsys UML Comp diag., SysML IBD, UML CD, SD, STM, (ACT), code |
| Subsystem Design and Implementation | Subsystem X - HW Design and Implementation | Analysis, schematics, PCB Layout |
| Subsystem Design and Implementation | Subsystem X - SW Test and Results | Unit test results and metrics |
| Subsystem Design and Implementation | Subsystem X - HW Test and Results | Test design and results |
| Subsystem Design and Implementation | Subsystem X - HW/SW Test and Results | Local HW/SW integration tests and results |
| System Integration Testing | Integration Tests for Use Case Y | |
| User Acceptance Testing | Test Setup | |
| User Acceptance Testing | Results and Discussion | |
| Evaluation and Discussion | Discussion of Test Results | |
| Evaluation and Discussion | Discussion of System Architecture and Design | |
| Conclusion | Conclusion | |
| Conclusion | Future Work | |

> s. 39

#### 10.3.3 Electronics Engineering-projekt

*Tabel 10.4: Relevant sections and diagrams for Electronics Engineering Projects*

| Kapitel | Sektion | Relevante figurer |
|---|---|---|
| Introduction | Background | |
| Introduction | Conceptual Overview of System | Rich Picture |
| Introduction | Problem Statement | |
| Methodology and Development Process | Project Model | |
| Methodology and Development Process | Development Methods | |
| User Requirements | Actors and Use Case Context | UML Actor-context diag. |
| User Requirements | Requirements Elicitation and Analysis | |
| User Requirements | Requirements Specification | UML Use Case diag. |
| User Requirements | Requirements Prioritization | MoSCoW Matrix |
| System Architecture | System Context and Scope | SysML BDD |
| System Architecture | Technology and Risk Assessment | Risk Matrix |
| System Architecture | System Structure and Interfaces | Sys-level SysML IBD, UML SD + HW I/F and protocol desc. |
| Subsystem Design and Implementation | Subsystem X - SW Design and Implementation | Subsys SysML IBD, UML ACT, STM, (CD), (SD), code |
| Subsystem Design and Implementation | Subsystem X - HW Design and Implementation | Analysis, schematics, PCB Layout |
| Subsystem Design and Implementation | Subsystem X - SW Test and Results | Unit test results and metrics |
| Subsystem Design and Implementation | Subsystem X - HW Test and Results | Test design and results |
| Subsystem Design and Implementation | Subsystem X - HW/SW Test and Results | Local HW/SW integration tests and results |
| System Integration Testing | Integration Tests for Use Case Y | |
| User Acceptance Testing | Test Setup | |
| User Acceptance Testing | Results and Discussion | |
| Evaluation and Discussion | Discussion of Test Results | |
| Evaluation and Discussion | Discussion of System Architecture and Design | |
| Conclusion | Conclusion | |
| Conclusion | Future Work | |

> s. 40

### 10.4 Code Listing Examples

Skabelonen demonstrerer fem listing-varianter (unboxed, boxed med header/caption, bash, side-by-side, two-row) — referencer til dem skrives som "Kodeudsnit 10.1" osv.

*Kodeudsnit 10.1: Unboxed code listing*

```cpp
int main() {
    std::vector<int> v = {1,2,3};
    int* p = &v[0]; // Set ptr
    v.push_back(4); // May reallocate
    std::cout << *p << "\n"; // UB
}
```

*Kodeudsnit 10.2: Boxed Code Listing* (header "Boxed Code Listing Header", caption under: "Vector push-back in C++") — samme C++-kode som 10.1.

*Kodeudsnit 10.3: Bash Listing Example* (header "Bash Listing Example Header", kommentar under: "Below comment"; efterfulgt af "This is a citation: [2]")

```bash
au276283@d43582 classicthesis % git checkout -b main
Switched to a new branch 'main'
au276283@d43582 classicthesis % git push --set-upstream originAU main
Enumerating objects: 848, done.
Counting objects: 100% (848/848), done.
Delta compression using up to 8 threads
```

*Kodeudsnit 10.4: Boxed Side-by-Side Code Listing* (header "Boxed Side-by-Side Code Listing Header"; undertitler "Left Title" / "Right Title") — venstre: C++-koden fra 10.1 (uden kommentaren "Set ptr"); højre:

```rust
fn main() {
    let mut v = vec![1, 2, 3];
    let p = &v[0];
    v.push(4);
    println!("{p}");
}
```

*Kodeudsnit 10.5: Boxed Two Row Code Listing* (header "Boxed Two-Row Code Listing Header"; "Upper Section" = C++-koden, "Lower Section" = Rust-koden).

> s. 41–42

### 10.5 Table examples

*Tabel 10.5: Table example with footnote* (refereres som "See Tabel 10.5")

| feature | no_std | std |
|---|---|---|
| heap (dynamic memory) | * | ✓ |
| collections (Vec, BTreeMap, etc) | ** | ✓ |
| stack overflow protection | × | ✓ |
| runs init code before main | × | ✓ |
| libstd available | × | ✓ |
| libcore available | ✓ | ✓ |
| writing firmware, kernel, or bootloader code | ✓ | × |

\* possible with an allocator (alloc) — \*\* many collections require alloc

> s. 42

### 10.6 Rotated Full-page Image Example

*Figur 10.1: Figure Example full-page landscape.* Et roteret helsides UML-lignende klassediagram for et Rust-projekt "flopsy-rs" med modulerne wifi («struct» WifiManager med members `stack: Option<Stack<'static>>`, `controller: Option<Rc<SharedWifiController>>`, `credential_manager: Rc<SharedCredetialManager>` og functions `new(flash: FLASH<'static>) -> Self`, `init(rng: &Rng, spawner: &Spawner, wifi_station: WifiDevice<'static>, wifi_controller: WifiController<'static>, ble_controller: ExternalController<BleConnector<'static>, 20>) -> Self`, `wait_until_ready(stack: &Stack<'_>) -> Ipv4Cidr`, `scan(max_results: usize) -> Result<Vec<AccessPointInfo>, WifiError>`; «type alias» SharedWifiController og SharedCredetialManager; «struct» WifiController), mqtt («struct» MqttSessionBuffers med rx_buffer/tx_buffer `&[u8; 4096]` og tx_buffer_mqtt/rx_buffer_mqtt `&[u8; 1024]`; «struct» MqttManager med publish_channel/receive_channel), message_manager («mod» message_manager_mod med `route_message(sender: EspNowSenderHandle, packet_id: u64, payload: Vec<u8>) -> ()`), wifi_cred («struct» WifiCredentialsManager med `map_storage`, `new(flash) -> Result<Self, WifiCredentialsError>`, `get_credentials()`, `store_credentials(wifi_credentials: WifiCredentials)`; «struct» WifiCredentials med `ssid: String`, `password: String`, `serialize_into`, `deserialize_from`, derives Debug/Clone/Serialize/Deserialize), mac («struct» MacAddress `[u8; 6]` med `get()`, `as_bytes()`, `to_colon_str()`, `fmt()`), sensor («struct» Shtc3Sensor med `dev: shtcx::ShtC3<I2C>`, `normal_delay: Duration`), ble («mod» ble_mod med `creds_receiver()` og Embassy-task `ble_task(controller)`; «struct» Server med `provisioning: ProvisioningService`; «struct» ProvisioningService med `ssid`, `password`, `done: bool`), esp_now_manager («mod» med `init(esp_now, spawner) -> (EspNowManagerHandle, EspNowSenderHandle)`, `send_broadcast(sender, payload: &[u8]) -> Result<(), EspNowError>`) og board («struct» Board med `sensor`, `ble_controller`, `wifi_controller`, `wifi_interfaces`, `storage: FLASH<'static>`). Formålet i skabelonen er alene at vise, hvordan en stor figur sættes roteret på en hel side.

> s. 43
