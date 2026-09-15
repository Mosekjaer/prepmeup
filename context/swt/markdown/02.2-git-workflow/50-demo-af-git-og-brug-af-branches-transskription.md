---
title: "Video: demo af git og brug af branches"
source: "demo af git og brug af branches.mp4"
type: "video-transskription"
varighed: "16:39"
sprog: "da"
---

# Video: demo af git og brug af branches

Automatisk transskription (faster-whisper, dansk). Videofilen ligger i `assets/video/demo af git og brug af branches.mp4`.

**[00:01]** Hej, jeg har en lille video omkring basal brug af feature-branches i Magid og GitLab fra Visual Studio Så feature-branches er en ganske fin måde at udvikle nye features på, og så merge dem ind i sin master-branche In i GitLab efter at man har testet sin kode virker, og andre måske har lavet et review på sin kode. Så hvordan gør man det?

**[00:33]** Jo, her er et simpel eksempel. Så vi skal starte med at lave en ny branch, og den skal jo så baseres på vores main eller master branch. Så lad os prøve at skifte branch, det gør man hernede. Og lige nu kan jeg se, at jeg står i en anden feature branch her, Nu skifter jeg lige til min master branch. Sådan. Så er vores gode checker ud, hedder det, og så står jeg altså nu i en master branch her.

**[01:06]** Skal jeg lige sikre mig, at jeg har den nyeste version, så jeg kan lige starte med at gå heroppe, og så kan man lave en bod. Jeg kunne også godt lave en patch, og så lave en merge, men når nu er master branchen, så har man nok ikke ændret i den lokalt. Så man kan lave en bod her, lige til at se om der er sket nogle ændringer, og det var der så ikke. Så jeg har den ydse version af den, der ligger op i gitlab, ellers vi får mine ændringer ind.

**[01:39]** Det er fint. Så kan jeg begynde at lave en ny branch. Så går jeg op over en git her, det er en new branch, og lad os sige, jeg vil lave fakultætsfunktionen. Så er det en god idé at måske kalde min branch noget med feature. Og så siger jeg backhaul.t-function, måske. Så den skal altså baseret på master branchen, det er ligesom den, vi tager udgangspunkt i.

**[02:10]** Og så vil jeg gerne check den out, det vil sige at skifte den, så jeg begynder at arbejde i den med det samme. Så har jeg faktisk fået en ny branch, nu kan jeg se, at der er 3 branches. Og jeg er altså i gang med at arbejde i min faculty-function her. Så kan jeg jo gå i gang med at gå ud. Så er der en start lige med en kommentar her, så kommer vi lige den.

**[02:50]** Så nu har jeg jo så fået ændret min branch her. Der er en forskel på mastern og min nye feature her. Den er dog jo ikke cometet i nu. Eller for den særl skyl er den heller ikke bevendt buss til Hitler. Så det kan jeg lige starte med at gøre. Så kan jeg se, at der er altså sket en ændring i en kædeblætter her. Så skriver vi lige en kommentar. Det er jo mit comet.

**[03:22]** Og så siger jeg fint nok, jeg vil gerne comet det hele her, der er kun en ændring. Og så kan vi lige så godt pushe det med det samme til GitLab. Ja, så har vi altså fået den pushet til min GitLab, vi kan lige gode at tjekke. Og så lige refresher heroppe. I gradglædet kan I se, at der er en eller andet branchet her.

**[03:53]** Og så kan I se, at der har kommet en ny branch heroppe. Og ja, man kan også se, at der står nogle tal her, der står noget med 0.1 behind master, og det vil sige, at den er opdateret i forhold til mastern. Og så står der sådan en commit forhjænger. Det er fordi, at der er noget nyt gået i den her, som ikke er i mastern. Nu kan jeg så begynde at udvikle. Eller hvordan skal jeg sgu i det andre kunne begynde at udvikle på den? Men det kunne være, at jeg så begyndte at kode lidt her, og jeg siger, at det er nok. Det skal være en eller anden empty der, og der siger, at vi kan have fakulti her funktionen, og den tager jo så en int, eller en i her, og så kan jeg så begynde at kode den, og giver jeg ikke lige at implementere den her.

**[04:48]** Men lige nu har vi bare en dommi-metode her, som bliver tøn at nå. Og så tænker jeg nok, men jeg vil da godt lige smide det på. Så er der bare lige en kommentar. Og igen ser jeg, at vi nok det pusher ved lige til GitLab. Ja, så er den pludselig til GitLab. Vi kunne jo godt lige tjekke her.

**[05:26]** Like just now, så er vi pludselig to commits foran. Og lige nu er der så også en pipeline som faktisk kører hos, Hvor se fil og altså kompiler og bilder og faktisk køre vores tester. Så det kunne jeg selvfølgelig godt vente på, hvis jeg ville. Men lad os nu sige, at jeg begåde den her funktion færdig,

**[05:57]** og så på den tidspunkt vil jeg gerne have den her funktion over i mastern. Så lad os nu se, hvor er færdig med at kode, så kunne man jo lige tjekke, at den vil virke med den nye master. Det kunne jo godt være, at nogen havde opdateret mastern med nogle andre features, mens jeg sidder og koder. Så er en god idé, inden man forsøger at mødes den ind her. Vi har gitlapped det lige at tjekke om den virker med eventuelt nye kode fra mastern.

**[06:29]** Så det kan jo lige prøve at gøre. Så jeg går lige over i min master branch igen. Ja, og så vil jeg lige sikre mig, at jeg har den nyeste god for mig sådan. Der var så ikke nogen ting, fordi der ikke nogen, der lige har endet dem i mellemtiden her, så der ikke nogen Changes. Men hvis der nu var nogle Changes, så lyder jeg godt lige at møtte de her Changes over i den fide.

**[07:07]** Så kan jeg sidde og arbejde på at se, om det nu også virker med de ting, der har skete i mastern Og det kan man jo så gøre ved at skifte til sin branch her Den man er i gang med at sidde og udvikle på Og så kan jeg så højre glede min mastern Og så kan jeg sige merge into current branch Altså den, jeg sidder og arbejder på, ind i min feature her. Jeg kan godt lige prøve at gøre det.

**[07:40]** Så siger jeg, ja. Og der ser jeg sgu ikke rigtig noget, fordi den allerede up to date, jeg har allerede det nyeste fra mastern ind i min feature brand. Så er jeg i hvert fald sikker på, at så må der ikke være nogen problemer med naturligt nye ting i mastern her. Ja, men ved en tidspunkt så skal den jo merges over i mastern, så den er den min nye funktionalitet kommer med i mastern. Og det vil man så typisk gøre via gitlab her, og det kan man gøre via det, der er merge requests.

**[08:13]** At jeg gerne requester, at mit nye feature, som jeg nu har testet, skal følge testen også, at den kan køre min test lokalt og så videre og så videre. Men lad os nu sige, at det var fint, at jeg gerne ville merge den over. Så kan man altså lave et merge request nogle steder. For eksempel på GitHub bliver det betegnet som et pro request. Så ja, det hedder lidt forskelligt, men det handler altså om at merge en feature over i en typisk master branch. Så lad os nu sige, at jeg gør det, så vil jeg gerne prøve at lave en ny merge request her.

**[08:51]** Sådan her, så nu gætter den på, at jeg gerne vil mørge min faktisk funktion inden mastern, for det er det nyeste, men ellers så kan man endere de branches, der skal mørges ind i mastern ved at trække her. Så kan man skrive et eller andet siddel her, så jeg siger min Fidrelde don, eller merge, eller hvad man nu har lyst til at kalde den her.

**[09:25]** Og så kommer man lige at skrive et eller andet her, som hvad vi dyver skal være mærksom på, at kage lige koden igennem. Et sættervarer, hvad man nu har lyst til at skrive her, og så kan man jo lige vælge at sige, at man vil gerne at sejne den pr. en lande, som skal udkende den her merge request. Det kunne jo være, hvis man havde en skrum, der har stil af en eller anden sted for at håndtere de her ting, men ja, det kan jo tydeligt være nogle af de andre i grupperne.

**[10:04]** Nu skal jeg have partiet mig selv, og hvis jeg gerne vil, kunne jeg også sætte en reviewer på, som skulle gå koden igennem. Det kunne jeg da også godt sætte mig selv på, hvis jeg havde lyst til det, det er et vold, man så selvfølgelig ikke gør. Og så igen her kan man vælge nogle forskellige options. Man kan vælge om man skal delete den her brand, når merch request er ekseptet. Det kunne man jo godt gøre, men det ville jeg nok typisk ikke gøre her i en skole opgave her i rar, der lige har alle de her branches her.

**[10:37]** Så kan man også vælge, og hvis jeg nu havde mange commits, eller så nu siger jeg 17 commits, og dem vil jeg helst ikke alle sammen have ind i min master branch, jeg vil gerne have én commit ind, ligesom. Så kan man vælge at sige, så squash dem, så kommer der bare én commit ind. Nu skal jeg fint nok, så gav det en rart rart ting at gøre, det havde jeg synes jeg. Høber, at jeg kan gøre det. Men så siger jeg, create merge request. Så er der altså kommet en merge request.

**[11:09]** Den er så lige undersegnet til mig selv, kan man sige. Og ja, så kan jeg lige prøve at give koden igennem her. Det er selvfølgelig, at gå ind og så måske begynde at give noget af koden igennem, men lafter bare sige, at det er okay, så det er thumbs up. Og så kan jeg lige prøve at bruge den her måske. Nu kan vi også se, at faktisk den er past, som den er altså kompeilet, og alle mine test er sådan set kørt.

**[11:44]** Jeg kan også se mine testcoverage her, den er så gået lidt ned Det er jo så fordi jeg ikke har lavet nogle nye test til den nye feature her Så det er ikke klart, der skal jeg også lave nogle test og sådan noget Men alldeles ser det jo meget godt ud, det her, ikke? Jeg kan jo lige sige proof, den her proof er bare mig Det er mig, der er reviewern her, ikke? Så det er jo fint nok Og så kan jeg sige, det ser jo godt ud, ellers hvis der ikke er Hvis du er ready to merge her, så vil der komme nogle warnings og nogle eventuelle merge-konflikter,

**[12:18]** man skal løse her. Så siger jeg, fint nok. Det er vi sånne set klar til. Så blev den sånne set merge fede ind, og så kan jeg se, så kører pipelineen lige igennem nu, går andet i gang på master branchen, og det er jo fordi, der sker nogle ændringer på master branchen, og så kører vi at se iscript.io igen, og det burde jo så selvfølgelig ikke på galt, fordi jeg selvfølgelig testede lokalt, at alle mine test kan køre.

**[12:52]** Det skal man lige huske selvfølgelig inden man bekyrner prøve at møde sådan noget ind i sin master. Jeg har ikke så testet at tænke, at det kører lokalt også, at testene kan køre. Ja, så kunne jeg, hvis jeg ville kunne jeg lige skrive en anden kommentar, hvis jeg havde lødt det det. Ja, alt gik fint, alt godt. Hvad han hedder at have lyst til, til det her, sådan der, og så går man flere kommentarer med sådan noget, og lyst til det her. Men nu er den altså merged ind, og nu begynder den lige så at køre de her, på et skalige stage, på mastern her.

**[13:32]** Vi kunne jo for eksempel være hedder det prøve at gå og kigge på brandsen her, og så kan vi se, at ja, den her faktisk blev mødt ind, og nu kan vi se, at det gik også godt, og så kunne jeg selvfølgelig gå ind og kigge på den nye corbels her, den ville selvfølgelig være lidt lavere. Og nu kan jeg også se, at den her funktion her, altså den skal jo så, nu kan jeg jo princippet godt slet den, hvis jeg vil.

**[14:20]** Men det er meget godt at beholde, der står også, der står Møgst her, så den er altså Møgstet ind i master. Ja, og så kan jeg andre jo tjekke ud den her master her og arbejde videre på den nye branches her. Det sidste man så lige skal sække, så det er så følge også, at man selv får den nye master. Den ligger jo kun på our remote repository på GitLab her. Så der skal så lige gå tilbage til Visual Studio her.

**[14:55]** Så kan vi lige skifte til master branchen her, og så kan vi lige tørre for at se, at jeg vil faktisk gerne have den nye kod, når jeg ser noget skiftet til mastern. Ja, der er jo ikke nogen fakultetsfunktioner, jeg kan se her, der er ikke noget short der, og det er så fordi jeg ikke har fået den nye master op fra GitLab. Så det kan jeg jo sige fint, den vil jeg gerne lige have fået en ny kode, der står en Master Branch.

**[15:29]** Så ser vi på, ja. Og så kan jeg se dem til at opdate siger til en kommet, og så har jeg så fået de nye kommets, der har fået, som der kom fra den branch, vi har merget ind. Ja, og så kunne jeg gå gang med at lave, hvis jeg skulle lave en ny feature, så kan vi igen tage det samme år, altså at lave nye brands baseret på den opdateret master her.

**[16:04]** Selvfølgelig kan der nogle gange kommet en merge konflikt, og hvis flere har opdateret noget i mastern eller flere sidder og laver nogle ting på den samme feature, som ikke er rigtig kompetivt, men andre, så må man jo prøve at løse dem, når de er opstår. Men det var altså en lille guide til, hvordan man meget simpelthen kan arbejde med i et simplificerede gitflow, det måske vi kalder git hopflow, som man har i en master, og så kan man have forskellige feature branches.
