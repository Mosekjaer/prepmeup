# Kapitel 1 — Meeting React

## Metadata

> **⚠️ Udgave-advarsel:** Denne bog er *React Quickly* **1. udgave (©2017, Azat Mardan)**. Kursusbeskrivelsen foreskriver **2. udgave** (Barklund & Mardan). 1. udgave er skrevet før React Hooks og bruger class components, lifecycle-metoder og Webpack. Kursets React-lektioner (L16–L27) er hook-baserede og bruger Vite, Vitest og Redux Toolkit. **Ved konflikt mellem denne bog og slidesene er slidesene autoritative.**

- **Kapitel:** 1 — Meeting React
- **Bog:** React Quickly, 1. udgave — Azat Mardan, Manning (©2017)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L16
- **Hovedemner:**
  - Hvad React er: et UI-component-bibliotek bygget på composable UIs i ren JavaScript
  - Problemet React løser: store applikationer med data der ændrer sig over tid
  - Declarative vs. imperative programmeringsstil
  - Component-based architecture (CBA) uden templates og DSL'er
  - Virtual DOM, DOM diffing og reconciliation
  - Fordele: simplicity, hastighed, testbarhed, økosystem
  - Ulemper: ikke et fuldt framework, JSX-tærskel, one-way binding
  - React i en single-page application (SPA)-arkitektur
  - React Core vs. ReactDOM og alternative rendering targets
  - Første React-kode: Hello World uden JSX

---

## Introduktion

Kapitlet åbner med en historisk ramme: da forfatteren begyndte med webudvikling i starten af 2000'erne, var HTML plus et server-side sprog som Perl eller PHP nok, og `alert()`-bokse var debug-værktøjet. Efterhånden som internettet er modnet, er kompleksiteten i at bygge websites steget dramatisk. Websites er blevet til web applications med komplekse user interfaces, forretningslogik og datalag, der skal ændres og opdateres over tid — ofte i realtid.

Mange JavaScript template-biblioteker er forsøgt brugt til at løse problemerne med komplekse UIs, men de kræver stadig, at udviklere holder fast i den gamle separation of concerns, som splitter style (CSS), data og struktur (HTML) og dynamiske interaktioner (JavaScript). Den opdeling matcher ikke moderne behov. (Husker du overhovedet termen DHTML?)

React tilbyder i stedet en tilgang, der strømliner front-end-udvikling. React er et kraftfuldt UI-bibliotek, som store firmaer som Facebook, Netflix og Airbnb har taget til sig. I stedet for at definere en engangs-template til dine UIs, lader React dig skabe genbrugelige UI components i JavaScript, som du kan bruge igen og igen på tværs af dine sites.

Har du brug for en captcha-kontrol eller date picker? Så definér med React en `<Captcha />`- eller `<DatePicker />`-component, som du kan tilføje til din formular: en simpel drop-in component med al funktionalitet og logik til at kommunikere med backenden. Har du brug for en autocomplete-boks, der asynkront forespørger en database, når brugeren har tastet fire eller flere bogstaver? Definér en `<Autocomplete charNum="4"/>` component, der laver den asynkrone forespørgsel. Du kan selv vælge, om den har et tekstfelt som UI, eller om den slet intet UI har og i stedet bruger et andet custom form-element — måske `<Autocomplete textbox="..." />`.

Tilgangen er ikke ny. Composable UIs har eksisteret længe, men React er det første til at bruge ren JavaScript uden templates til at gøre det muligt. Og den tilgang har vist sig lettere at vedligeholde, genbruge og udvide.

React er et godt bibliotek til UIs og bør være en del af dit front-end-værktøjssæt, men det er ikke en komplet løsning på al front-end-webudvikling. Kapitlet ser på fordele og ulemper ved React og på, hvordan det kan passe ind i en eksisterende web-development-stak.

Bogens del 1 fokuserer på React's primære koncepter og features; del 2 ser på biblioteker omkring React (a.k.a. React stack eller "React and friends"). Begge dele demonstrerer både greenfield- og brownfield-udvikling. *Brownfield* er et projekt med legacy-kode og eksisterende systemer, mens *greenfield* er et projekt uden legacy-kode eller -systemer.

---

## 1.1 What is React?

React defineres i bogen som et **UI component library**. UI components skabes i React med JavaScript, ikke med et særligt template-sprog. Denne tilgang kaldes at skabe **composable UIs**, og den er fundamental for React's filosofi.

React UI components er i høj grad selvindeholdte, concern-specifikke blokke af funktionalitet. Der kunne fx være components til date-picker, captcha, adresse og postnummer-felter. Sådanne components har både en visuel repræsentation og dynamisk logik. Nogle components kan endda tale med serveren på egen hånd: en autocomplete-component kan fx hente sin autocompletion-liste fra serveren.

### Sidebemærkning: User interfaces

I bred forstand er et user interface alt, der faciliterer kommunikation mellem computere og mennesker. Tænk på et hulkort eller en mus — begge er UIs. Inden for software taler ingeniører om graphical user interfaces (GUIs), som blev banebrydende med tidlige personlige computere som Mac og PC. Et GUI består af menuer, tekst, ikoner, billeder, kanter og andre elementer. Web-elementer er en snæver delmængde af GUI'et: de bor i browsere, men der findes også elementer til desktop-applikationer i Windows, OS X og andre operativsystemer. Når bogen nævner et UI, menes der altid et web-GUI.

**Component-based architecture (CBA)** — ikke at forveksle med web components, som blot er én af de nyeste implementeringer af CBA — eksisterede før React. Sådanne arkitekturer er generelt lettere at genbruge, vedligeholde og udvide end monolitiske UIs. Det, React bringer til bordet, er brugen af ren JavaScript (uden templates) og en ny måde at anskue komposition af components på.

---

## 1.2 The problem that React solves

Hvilket problem løser React? Ser man på de seneste års webudvikling, er problemerne med at bygge og styre komplekse web-UIs til front-end-applikationer tydelige — og React blev primært født for at adressere netop dem. Tænk på store web-apps som Facebook: en af de mest smertefulde opgaver ved at udvikle sådanne applikationer er at styre, hvordan views ændrer sig som respons på dataændringer.

Bogen citerer React's officielle website: *"We built React to solve one problem: building large applications with data that changes over time."*

Historien bag: I en diskussion på React Podcast nævnes det, at React's skaber — **Jordan Walke** — løste et problem hos Facebook: at have flere datakilder, der opdaterede et autocomplete-felt. Dataene kom asynkront fra en backend. Det blev mere og mere kompliceret at afgøre, hvor nye rækker skulle indsættes for at kunne genbruge DOM-elementer. Walke besluttede at generere feltets repræsentation (DOM-elementerne) på ny hver gang. Løsningen var elegant i sin simpelhed: **UIs som funktioner**. Kald dem med data, og du får renderede views forudsigeligt.

Det viste sig senere, at det er ekstremt hurtigt at generere elementer i hukommelsen, og at den egentlige flaskehals er rendering i DOM'en. React-teamet fandt på en algoritme, der undgår unødvendig DOM-smerte. Det gjorde React meget hurtigt (og billigt performance-mæssigt). React's fremragende performance kombineret med en udvikler-venlig, component-based arkitektur er en vindende kombination.

React løste Facebooks oprindelige problem, og mange store firmaer var enige i tilgangen. React kom ud af Facebook og bruges nu ikke kun af Facebook, men også af Instagram, PayPal, Uber, Sberbank, Asana, Khan Academy, HipChat, Flipboard og Atom, for at nævne nogle få. De fleste af disse applikationer brugte oprindeligt noget andet (typisk template engines med Angular eller Backbone), men skiftede til React og er meget glade for det.

---

## 1.3 Benefits of using React

Hvert nyt bibliotek eller framework hævder at være bedre end sine forgængere på et eller andet punkt. I begyndelsen havde vi jQuery, og det var milevidt bedre til at skrive cross-browser-kode end native JavaScript. Husk, at et enkelt AJAX-kald krævede mange linjer kode for at tage højde for Internet Explorer og WebKit-lignende browsere. Med jQuery kræver det kun ét kald: fx `$.ajax()`. Dengang blev jQuery kaldt et framework — men ikke længere. Nu er et framework noget større og mere kraftfuldt.

Tilsvarende med Backbone og siden Angular: hver ny generation af JavaScript-frameworks har bragt noget nyt. React er ikke unikt i det. Det nye er, at React udfordrer nogle af de kernekoncepter, de fleste populære front-end-frameworks bruger — for eksempel idéen om, at man behøver templates.

Bogens liste over fordele ved React versus andre biblioteker og frameworks:

- **Simpler apps** — React har en CBA med ren JavaScript; en declarative stil; og kraftfulde, udvikler-venlige DOM-abstraktioner (og ikke kun DOM, men også iOS, Android og så videre).
- **Fast UIs** — React leverer fremragende performance takket være sin **virtual DOM** og smart-reconciliation-algoritme, som i øvrigt gør det muligt at teste uden at starte en headless browser.
- **Less code to write** — React's store community og enorme økosystem af components giver udviklere adgang til en bred vifte af biblioteker og components. Det er vigtigt, når man overvejer, hvilket framework man skal udvikle i.

### 1.3.1 Simplicity

Begrebet *simplicity* i datalogi værdsættes højt af både udviklere og brugere. Det er ikke det samme som *ease of use*. Noget simpelt kan være svært at implementere, men til gengæld ender det med at være mere elegant og effektivt. Og ofte ender noget "let" med at blive komplekst. Simplicity er tæt beslægtet med KISS-princippet (keep it simple, stupid). Kernen er, at simplere systemer fungerer bedre.

React's tilgang tillader simplere løsninger via en dramatisk bedre webudviklings-oplevelse. Forfatteren sammenligner skiftet til React med skiftet fra ren, framework-løs JavaScript til jQuery.

I React opnås denne simplicity gennem følgende features:

- **Declarative over imperative style** — React foretrækker declarative stil over imperative ved at opdatere views automatisk.
- **Component-based architecture using pure JavaScript** — React bruger ikke domain-specific languages (DSLs) til sine components, kun ren JavaScript. Og der er ingen opdeling, når man arbejder på samme funktionalitet.
- **Powerful abstractions** — React har en forenklet måde at interagere med DOM'en på, hvilket lader dig normalisere event handling og andre interfaces, så de virker ens på tværs af browsere.

#### Declarative over imperative style

Declarative stil betyder, at udviklere skriver *hvordan det skal være*, ikke hvad der trin for trin skal gøres (imperative). Hvorfor er declarative bedre? Fordelen er, at declarative stil reducerer kompleksitet og gør din kode lettere at læse og forstå.

Betragt dette korte JavaScript-eksempel, som illustrerer forskellen. Antag at du skal lave et array (`arr2`), hvis elementer er resultatet af at fordoble elementerne i et andet array (`arr`). Du kan bruge en `for`-løkke til at iterere over arrayet og bede systemet om at gange med 2 og skabe et nyt element (`arr2[i]=`):

```javascript
var arr = [1, 2, 3, 4, 5],
arr2 = []
for (var i=0; i<arr.length; i++) {
arr2[i] = arr[i]*2
}
console.log('a', arr2)
```

Resultatet af dette snippet, hvor hvert element ganges med 2, printes på konsollen sådan:

```javascript
a [2, 4, 6, 8, 10]
```

Dette illustrerer imperative programmering, og det virker — indtil det ikke virker, på grund af kodens kompleksitet. Det bliver for svært at forstå, hvad slutresultatet er ment at være, når man har for mange imperative statements. Heldigvis kan man skrive samme logik i declarative stil med `map()`:

```javascript
var arr = [1, 2, 3, 4, 5],
arr2 = arr.map(function(v, i){ return v*2 })
console.log('b', arr2)
```

Outputtet er `b [2, 4, 6, 8, 10]`; variablen `arr2` er den samme som i det forrige eksempel. Hvilket snippet er lettest at læse og forstå? Efter forfatterens ydmyge mening: det declarative.

Se på følgende imperative kode til at hente en nested værdi i et objekt. Udtrykket skal returnere en værdi baseret på en streng som `account` eller `account.number`, sådan at disse statements printer `true`:

```javascript
var profile = {account: '47574416'}
var profileDeep = {account: { number: 47574416 }}
console.log(getNestedValueImperatively(profile, 'account') === '47574416')
console.log(getNestedValueImperatively(profileDeep, 'account.number')
➥ === 47574416)
```

Denne imperative stil fortæller bogstaveligt systemet, hvad det skal gøre for at få resultatet:

```javascript
var getNestedValueImperatively = function getNestedValueImperatively
➥ (object, propertyName) {
var currentObject = object
var propertyNamesList = propertyName.split('.')
var maxNestedLevel = propertyNamesList.length
var currentNestedLevel
for (currentNestedLevel = 0; currentNestedLevel < maxNestedLevel;
➥ currentNestedLevel++) {
if (!currentObject || typeof currentObject === 'undefined')
➥ return undefined
currentObject = currentObject[propertyNamesList[currentNestedLevel]]
}
return currentObject
}
```

Sammenlign med declarative stil (fokuseret på resultatet), som reducerer antallet af lokale variable og dermed forenkler logikken:

```javascript
var getValue = function getValue(object, propertyName) {
return typeof object === 'undefined' ? undefined : object[propertyName]
}
var getNestedValueDeclaratively = function getNestedValueDeclaratively(object,
➥ propertyName) {
return propertyName.split('.').reduce(getValue, object)
}
console.log(getNestedValueDeclaratively({bar: 'baz'}, 'bar') === 'baz')
console.log(getNestedValueDeclaratively({bar: { baz: 1 }}, 'bar.baz')=== 1)
```

De fleste programmører er trænet til at kode imperativt, men som regel er den declarative kode simplere. I dette eksempel gør færre variable og statements den declarative kode lettere at gennemskue ved første øjekast.

Og React? React tager samme declarative tilgang, når man komponerer UIs. Først beskriver React-udviklere UI-elementer i declarative stil. Derefter, når der sker ændringer i de views, disse UI-elementer genererer, tager React sig af opdateringerne.

Bekvemmeligheden ved React's declarative stil skinner især igennem, når du skal foretage ændringer i view'et. De kaldes ændringer af den interne **state**. Når state ændrer sig, opdaterer React view'et tilsvarende.

> **NOTE (bogen):** State dækkes i kapitel 4.

Under motorhjelmen bruger React en **virtual DOM** til at finde forskellene (delta'et) mellem det, der allerede er i browseren, og det nye view. Denne proces kaldes **DOM diffing** eller **reconciliation** af state og view (at bringe dem tilbage til overensstemmelse). Det betyder, at udviklere ikke behøver bekymre sig om eksplicit at ændre view'et; alt de skal gøre, er at opdatere state, og view'et bliver automatisk opdateret efter behov.

Med jQuery ville du omvendt skulle implementere opdateringer imperativt. Ved at manipulere DOM'en kan udviklere programmatisk ændre websiden eller dele af websiden (det mest sandsynlige scenarie) uden at gen-rendere hele siden. DOM-manipulation er det, du gør, når du kalder jQuery-metoder.

Nogle frameworks, såsom Angular, kan udføre automatiske view-opdateringer. I Angular kaldes det **two-way data binding**, hvilket grundlæggende betyder, at views og models har tovejs-kommunikation/synkronisering af data mellem sig.

jQuery- og Angular-tilgangene er ikke gode, af to grunde. Tænk på dem som to yderpunkter. På det ene yderpunkt gør biblioteket (jQuery) ingenting, og udvikleren skal implementere alle opdateringer manuelt. På det andet yderpunkt gør frameworket (Angular) alt.

jQuery-tilgangen er fejlbehæftet og kræver mere arbejde. Direkte manipulation af den rigtige DOM fungerer også fint med simple UIs, men den er begrænsende, når man har mange elementer i DOM-træet, fordi det er sværere at gennemskue resultatet af imperative funktioner end af declarative statements.

Angular-tilgangen er svær at ræsonnere om, fordi ting med two-way binding hurtigt kan løbe ud af kontrol. Man indsætter mere og mere logik, og pludselig opdaterer forskellige views models, og de models opdaterer andre views. Ja, Angular-tilgangen er noget mere læsbar end imperative jQuery (og kræver mindre manuel kodning), men der er et andet problem: Angular hviler på templates og et DSL, der bruger `ng`-direktiver (for eksempel `ng-if`).

#### Component-based architecture using pure JavaScript

Component-based architecture eksisterede før React kom på banen. Separation of concerns, loose coupling og code reuse er kernen i tilgangen, fordi den giver mange fordele; softwareingeniører, inklusive webudviklere, elsker CBA. Byggestenen i CBA i React er **component-klassen**. Som med andre CBA'er er hovedfordelen genbrug af kode (du kan skrive mindre kode).

> *Kurset bruger i stedet funktionelle komponenter — se slides L17.1. "Component class" som byggesten er 1.-udgave-terminologi; i moderne React er byggestenen en funktion, der returnerer JSX.*

Det, der manglede før React, var en implementering af arkitekturen i ren JavaScript. Når man arbejder med Angular, Backbone, Ember eller de fleste andre MVC-lignende front-end-frameworks, har man én fil til JavaScript og en anden til template'en. (Angular bruger termen *directives* for components.) Der er flere problemer ved at have to sprog (og to eller flere filer) til én component.

Adskillelsen af HTML og JavaScript fungerede godt, dengang HTML skulle renderes på serveren, og JavaScript kun blev brugt til at få tekst til at blinke. I dag håndterer **single page applications (SPAs)** komplekst brugerinput og udfører rendering i browseren. Det betyder, at HTML og JavaScript er tæt koblede funktionelt. For udviklere giver det mere mening ikke at skulle adskille HTML og JavaScript, når man arbejder på ét stykke af projektet (en component).

Betragt denne Angular-kode, som viser forskellige links afhængigt af værdien af `userSession`:

```html
<a ng-if="user.session" href="/logout">Logout</a>
<a ng-if="!user.session" href="/login">Login</a>
```

Du kan læse den, men du kan være i tvivl om, hvad `ng-if` tager: en Boolean eller en streng. Og vil den skjule elementet eller slet ikke rendere det? I Angular-tilfældet kan du ikke være sikker på, om elementet skjules ved `true` eller `false`, medmindre du er bekendt med, hvordan netop dette `ng-if`-direktiv fungerer.

Sammenlign det med følgende React-kode, som bruger JavaScripts `if`/`else` til betinget rendering. Det er helt klart, hvad værdien af `user.session` skal være, og hvilket element (logout eller login) der renderes, hvis værdien er `true`. Hvorfor? Fordi det bare er JavaScript:

```javascript
if (user.session) return React.createElement('a', {href: '/logout'}, 'Logout')
else return React.createElement('a', {href: '/login'}, 'Login')
```

Templates er nyttige, når man skal iterere over et array af data og printe en property. Vi arbejder med lister af data hele tiden. Her er en `for`-løkke i Angular, hvor direktivet hedder `ng-repeat`:

```html
<div ng-repeat="account in accounts">
{{account.name}}
</div>
```

Et af problemerne med templates er, at udviklere ofte skal lære endnu et sprog. I React bruger du ren JavaScript, hvilket betyder, at du ikke behøver lære et nyt sprog. Her er et eksempel på at komponere et UI for en liste af kontonavne med ren JavaScript:

```javascript
accounts.map(function(account) {
return React.createElement('div', null, account.name)
})
```

Annotationerne i bogen: `accounts.map(...)` er en almindelig JavaScript-metode, der tager et iterator-udtryk som parameter, og iterator-udtrykket returnerer en `<div>` med kontoens navn.

Forestil dig, at du laver ændringer i listen af konti. Du skal vise kontonummeret og andre felter. Hvordan ved du, hvilke felter kontoen har ud over `name`? Du skal åbne den tilsvarende JavaScript-fil, som kalder og bruger template'en, og dér finde `accounts` for at se dens properties. Så det andet problem med templates er, at logikken om dataene og beskrivelsen af, hvordan de skal renderes, er adskilt.

Det er meget bedre at have JavaScript og markup ét sted, så du ikke skal skifte mellem filer og sprog. Det er præcis, hvordan React fungerer.

> **NOTE (bogen):** Separation of concerns er generelt et godt mønster. Kort sagt betyder det adskillelse af forskellige funktioner såsom dataservice, view-lag og så videre. Når man arbejder med template-markup og tilhørende JavaScript-kode, arbejder man på én funktionalitet. Derfor er to filer (.js og .html) ikke separation of concerns.

Vil du eksplicit sætte metoden, hvormed elementer i den renderede liste holdes styr på (for eksempel for at sikre, at der ikke er dubletter), kan du bruge Angulars `track by`:

```html
<div ng-repeat="account in accounts track by account._id">
{{account.name}}
</div>
```

Vil du tracke efter et indeks i arrayet, er der `$index`:

```html
<div ng-repeat="account in accounts track by $index">
{{account.name}}
</div>
```

Men hvad er denne magiske `$index`? I React bruger du et argument fra `map()` som værdi for `key`-attributten:

```javascript
accounts.map(function(account, index) {
return React.createElement('div', {key: index}, account.name)
})
```

Bogens annotationer: der bruges en array-elementværdi (`account`) og dens index leveret af `Array.map()`, og der returneres et React-element `<div/>` med en attribut `key` med værdien `index` og indre tekst sat til `account.name`.

Det er værd at bemærke, at `map()` ikke er eksklusivt for React. Du kan bruge det med andre frameworks, fordi det er en del af sproget. Men `map()`s declarative natur gør det og React til et perfekt par.

Bundlinjen: bruger et framework et DSL, skal du lære dets magiske variable og metoder. I React kan du bruge ren JavaScript. Bruger du React, kan du tage din viden med til det næste projekt, selv hvis det ikke er i React. Bruger du derimod en X template engine (eller et Y framework med en indbygget DSL template engine), er du låst inde i det system og må beskrive dig selv som X/Y-udvikler. Din viden er ikke overførbar til projekter, der ikke bruger X/Y.

Opsummeret handler pure JavaScript component-based architecture om at bruge diskrete, velindkapslede, genbrugelige components, der sikrer bedre separation of concerns baseret på funktionalitet, uden behov for DSLs, templates eller directives.

Forfatteren tilføjer en observation fra arbejdet med mange udviklerhold: React har en bedre, fladere, mere gradvis læringskurve sammenlignet med MVC-frameworks og template engines med særlig syntaks — for eksempel Angular-direktiver eller Jade/Pug. Grunden er, at de fleste template engines i stedet for at bruge JavaScripts kraft bygger abstraktioner med deres eget DSL og på en måde genopfinder ting som en `if`-betingelse eller en `for`-løkke.

#### Powerful abstractions

React har en kraftfuld abstraktion over dokumentmodellen. Med andre ord skjuler det de underliggende interfaces og leverer normaliserede/syntetiserede metoder og properties. Når du for eksempel opretter en `onClick`-event i React, modtager event handleren ikke et native, browser-specifikt event-objekt, men et **synthetic event**-objekt, som er en wrapper omkring native event-objekter. Du kan forvente samme opførsel fra synthetic events uanset hvilken browser koden kører i. React har også et sæt synthetic events til touch events, som er gode til at bygge web-apps til mobile enheder.

Et andet eksempel på React's DOM-abstraktion er, at du kan rendere React-elementer på serveren. Det kan være praktisk for bedre search engine optimization (SEO) og/eller forbedret performance.

### 1.3.2 Speed and testability

Ud over de nødvendige DOM-opdateringer kan dit framework udføre unødvendige opdateringer, hvilket gør performance i komplekse UIs endnu værre. Det bliver særligt mærkbart og smertefuldt for brugerne, når man har mange dynamiske UI-elementer på siden.

React's **virtual DOM** eksisterer derimod kun i JavaScript-hukommelsen. Hver gang der sker en dataændring, sammenligner React først forskellene ved hjælp af sin virtual DOM; kun når biblioteket ved, at der er sket en ændring i renderingen, opdaterer det den rigtige DOM.

Figur 1.1 i bogen viser flowet på højt niveau (beskrevet her i tekst, da figuren ikke kan gengives):

1. **Render** — en component renderes; ReactElement / ReactNode / ReactComponent findes i React's virtual DOM, DOMNode i den rigtige DOM.
2. **State changes (`setState`)** — state ændrer sig.
3. **Smart diffing algorithm (reconciliation)** — virtual DOM identificerer "dirty" components, der er påvirket af state-ændringerne.
4. **Rerender only affected elements** — kun de påvirkede elementer gen-renderes i den rigtige DOM.

Figurteksten lyder: *"Once a component has been rendered, if its state changes, it's compared to the in-memory virtual DOM and rerendered if necessary."*

I sidste ende opdaterer React kun de dele, der er absolut nødvendige, så den interne state (virtual DOM) og view'et (den rigtige DOM) er ens. Hvis der for eksempel er et `<p>`-element, og du ændrer teksten via componentens state, opdateres kun teksten (det vil sige `innerHTML`), ikke selve elementet. Det giver bedre performance end at gen-rendere hele sæt af elementer eller — endnu værre — hele sider (server-side rendering).

> **NOTE (bogen):** Er du til algoritmer og Big O, forklarer to artikler godt, hvordan React-teamet fik vendt et O(n³)-problem til et O(n)-problem: "Reconciliation" på React's website og "React's Diff Algorithm" af Christopher Chedeau.

Den tilføjede fordel ved virtual DOM er, at du kan lave unit testing uden headless browsere som PhantomJS. Der findes et Jasmine-lag kaldet **Jest**, som lader dig teste React components direkte fra kommandolinjen.

> *Kurset bruger Vitest til unit-test af React-komponenter, ikke Jest — se kursets test-lektioner.*

### 1.3.3 Ecosystem and community

React understøttes af udviklerne bag Facebook såvel som deres kolleger hos Instagram. Som med Angular og nogle andre biblioteker giver et stort firma bag teknologien en solid testgrund (den er deployet til millioner af browsere), tryghed om fremtiden og øget bidragshastighed.

React-communitiet er stort. Det meste af tiden behøver udviklere ikke selv implementere ret meget kode. Bogen lister disse community-ressourcer:

- Liste over React components: `https://github.com/brillout/awesome-react-components` og `http://devarchy.com/react-components`
- Sæt af React components, der implementerer Google Material Design-specifikationen: `http://react-toolbox.com`
- Material Design React components: `www.material-ui.com`
- Samling af React components til Office- og Office 360-oplevelser med Office Design Language: `https://github.com/OfficeDev/office-ui-fabric-react`
- Opinioneret katalog over open source JS-pakker (mest React): `https://js.coach`
- Katalog over React components: `https://react.rocks`
- Khan Academy React components: `https://khan.github.io/react-components`
- Register over React components: `www.reactjsx.com`

Forfatterens anekdotiske erfaring med open source er, at markedsføringen af open source-projekter er lige så vigtig for udbredelse og succes som selve koden. Har et projekt en dårlig hjemmeside, mangler dokumentation og eksempler og har et grimt logo, vil de fleste udviklere ikke tage det seriøst — især nu, hvor der er så mange JavaScript-biblioteker. Heldigvis har React et godt teknisk ry i ryggen.

---

## 1.4 Disadvantages of React

Næsten alt har ulemper. Det gælder også React, men den fulde liste afhænger af, hvem man spørger. Nogle af forskellene, som declarative versus imperative, er i høj grad subjektive og kan være både fordele og ulemper. Bogens liste over React-ulemper:

- React er ikke et fuldblods, schweizerkniv-agtigt framework. Udviklere skal parre det med et bibliotek som **Redux** eller **React Router** for at opnå funktionalitet, der kan sammenlignes med Angular eller Ember. Det kan også være en fordel, hvis man har brug for et minimalistisk UI-bibliotek at integrere i sin eksisterende stak.
- React er ikke så modent som andre frameworks. React's core-API ændrer sig stadig, om end kun lidt efter 0.14-udgivelsen; best practices for React (samt økosystemet af components, plug-ins og add-ons) er stadig under udvikling.
- React bruger en noget ny tilgang til webudvikling, og **JSX** og **Flux** (ofte brugt med React som databibliotek) kan virke skræmmende for begyndere. Der mangler best practices, gode bøger, kurser og ressourcer til at mestre React.
- React har kun **one-way binding**. Selvom one-way binding er bedre til komplekse apps og fjerner en masse kompleksitet, vil nogle udviklere (især Angular-udviklere), der er vant til two-way binding, komme til at skrive lidt mere kode. Bogen forklarer forskellen i kapitel 14 om arbejde med data.
- React er ikke *reactive* (som i reactive programming og arkitektur, der er mere event-drevet, resilient og responsiv) ud af boksen. Udviklere skal bruge andre værktøjer som Reactive Extensions (RxJS) til at komponere asynkrone datastrømme med Observables.

> *Bemærk til kurset: 1. udgaves "umodenhed"-punkt er forældet. Kursets stak bruger Redux Toolkit til state management og Vite som build-værktøj, hvor bogen bruger Flux/Redux og Webpack.*

---

## 1.5 How React can fit into your web applications

React-biblioteket i sig selv, uden React Router eller et databibliotek, kan i mindre grad sammenlignes med frameworks (som Backbone, Ember og Angular) og i højere grad med biblioteker til at arbejde med UIs, som template engines (Handlebars, Blaze) og DOM-manipulationsbiblioteker (jQuery, Zepto). Faktisk har mange hold udskiftet traditionelle template engines som Underscore i Backbone eller Blaze i Meteor med React, med stor succes.

Du kan bruge React til blot en del af dit UI. Sig, at du har en formular til at indlæse en ansøgning på en webside bygget med jQuery. Du kan gradvist begynde at konvertere denne front-end-app til React ved først at konvertere by- og stats-felterne, så de udfyldes automatisk baseret på postnummeret. Resten af formularen kan blive ved med at bruge jQuery. Vil du fortsætte, kan du konvertere resten af formularens elementer fra jQuery til React, indtil hele siden er bygget på React. Med samme tilgang har mange hold integreret React med Backbone, Angular eller andre eksisterende front-end-frameworks.

React er **back-end agnostisk** i front-end-udviklingens forstand. Du behøver ikke stole på en Node.js-backend eller MERN (MongoDB, Express.js, React.js og Node.js) for at bruge React. Det er fint at bruge React med enhver anden backend-teknologi som Java, Ruby, Go eller Python. React er trods alt et UI-bibliotek. Du kan integrere det med enhver backend og ethvert front-end-databibliotek.

Opsummeret bruges React oftest i disse scenarier:

- Som UI-bibliotek i React-relaterede stak-SPA'er, såsom React + React Router + Redux
- Som UI-bibliotek (V'et i MVC) i ikke-fuldt React-relaterede stak-SPA'er, såsom React + Backbone
- Som en drop-in UI component i enhver front-end-stak, såsom en React autocomplete-input-component i en jQuery + server-side rendering-stak
- Som et server-side template-bibliotek i en ren thick-server (traditionel) web-app eller i en hybrid eller isomorphic/universal web-app, såsom en Express-server der bruger `express-react-views`
- Som UI-bibliotek i mobilapps, såsom en React Native iOS-app
- Som UI-beskrivelsesbibliotek til forskellige rendering targets

React spiller pænt sammen med andre front-end-teknologier, men bruges mest som en del af en single-page-arkitektur, fordi SPA synes at være den mest fordelagtige og populære tilgang til at bygge web-apps.

I nogle ekstreme scenarier kan du endda bruge React kun på serveren som en slags template engine. Der findes for eksempel et `express-react-views`-bibliotek, som renderer view'et server-side ud fra React components. Denne server-side rendering er mulig, fordi React lader dig bruge forskellige rendering targets.

### 1.5.1 React libraries and rendering targets

I versioner 0.14 og opefter delte React-teamet biblioteket i to pakker: **React Core** (`react`-pakken på npm) og **ReactDOM** (`react-dom`-pakken på npm). Dermed gjorde vedligeholderne det klart, at React er på vej mod at blive ikke bare et bibliotek til web, men et universelt (undertiden kaldet isomorphic, fordi det kan bruges i forskellige miljøer) bibliotek til at beskrive UIs.

I version 0.13 havde React for eksempel en `React.render()`-metode til at montere et element til en websides DOM-node. I versioner 0.14 og opefter skal du inkludere `react-dom` og kalde `ReactDOM.render()` i stedet.

At community'et har skabt flere pakker til at understøtte forskellige rendering targets gjorde det logisk at adskille komposition af components fra render-logikken. Nogle af disse moduler:

- Renderer til `blessed` terminal-interfacet: `http://github.com/Yomguithereal/react-blessed`
- Renderer til ART-biblioteket: `https://github.com/reactjs/react-art`
- Renderer til `<canvas>`: `https://github.com/Flipboard/react-canvas`
- Renderer til 3D-biblioteket three.js: `https://github.com/Izzimach/react-three`
- Renderer til virtual reality og interaktive 360-oplevelser: `https://facebook.github.io/react-vr`

Ud over understøttelsen af disse biblioteker gør adskillelsen af React Core fra ReactDOM det lettere at dele kode mellem React og React Native (brugt til native mobil-udvikling på iOS og Android). I bund og grund skal du, når du bruger React til webudvikling, som minimum inkludere React Core og ReactDOM.

Derudover findes yderligere React-utility-biblioteker i React og npm. (Før React v15.5 var nogle af dem en del af React som React add-ons.) Disse utility-biblioteker lader dig udvide funktionalitet, arbejde med immutable data (`immutability-helper`) og udføre testing.

Endelig bruges React næsten altid sammen med **JSX** — et lille sprog, der lader udviklere skrive React-UIs mere elegant. Du kan transpile JSX til almindeligt JavaScript ved hjælp af Babel eller et lignende værktøj.

Der er altså megen modularitet — funktionaliteten omkring React er delt i forskellige pakker. Det giver dig magt og valgfrihed, hvilket er godt. Ingen monolit eller opinioneret bibliotek dikterer den eneste mulige måde at implementere ting på.

### 1.5.2 Single-page applications and React

Et andet navn for SPA-arkitektur er **thick client**, fordi browseren, som klient, holder mere logik og udfører funktioner såsom rendering af HTML, validering, UI-ændringer og så videre.

Bogens figur 1.2 viser et fugleperspektiv af en typisk SPA-arkitektur med en bruger, en browser og en server. Trinnene er:

1. Brugeren taster en URL i browseren for at åbne en ny side.
2. Browseren sender en URL-forespørgsel til serveren.
3. Serveren svarer med statiske assets såsom HTML, CSS og JavaScript. I de fleste tilfælde er HTML'en bare et skelet af websiden. Ofte er der en "Loading ..."-besked og/eller en roterende spinner-GIF.
4. De statiske assets inkluderer JavaScript-koden til SPA'en. Når den er indlæst, laver koden yderligere forespørgsler efter data (AJAX/XHR-requests).
5. Dataene kommer tilbage i JSON, XML eller et andet format.
6. Når SPA'en har modtaget dataene, kan den rendere den manglende HTML. Med andre ord sker UI-rendering i browseren ved at SPA'en hydrerer templates med data.
7. Når browser-renderingen er færdig, erstatter SPA'en "Loading …"-beskeden, og brugeren kan arbejde med siden.
8. Brugeren ser en færdig webside og kan interagere med den, hvilket udløser nye forespørgsler fra SPA'en til serveren, og cyklussen i trin 2–6 fortsætter. På dette stadie kan browser-routing forekomme, hvis SPA'en implementerer det: navigation til en ny URL udløser ikke en ny sideindlæsning fra serveren, men snarere en SPA-rerender i browseren.

I SPA-tilgangen sker det meste rendering af UIs altså i browseren. Kun data rejser til og fra browseren. Modsat en **thick-server**-tilgang, hvor al rendering sker på serveren. (Her betyder rendering "at generere HTML fra templates eller UI-kode", ikke at tegne HTML'en i browseren, hvilket undertiden kaldes painting eller drawing af DOM'en.)

MVC-lignende arkitektur er den mest populære tilgang, men ikke den eneste. React kræver ikke, at du bruger en MVC-lignende arkitektur; men for enkelhedens skyld antager bogen det. Figur 1.3 viser de mulige dele: et navigator- eller routing-bibliotek fungerer som en slags controller i MVC-paradigmet; det dikterer hvilke data der skal hentes, og hvilken template der skal bruges. Navigator/controller laver en forespørgsel for at få data og hydrerer/udfylder derefter templates (views) med disse data for at rendere UI'et i form af HTML. UI'et sender actions tilbage til SPA-koden: klik, mouse hovers, tastetryk og så videre.

I en SPA-arkitektur fortolkes og behandles data i browseren (browser rendering) og bruges af SPA'en til at rendere yderligere HTML eller til at ændre eksisterende HTML. Det giver flotte, interaktive webapplikationer, der kan måle sig med desktop-apps. Angular.js, Backbone.js og Ember.js er eksempler på front-end-frameworks til at bygge SPA'er.

> **NOTE (bogen):** Forskellige frameworks implementerer navigators, data og templates forskelligt, så figur 1.3 gælder ikke alle frameworks. Den illustrerer snarere den mest udbredte separation of concerns i en typisk SPA.

React's plads i SPA-diagrammet er i **Templates**-blokken. React er et **view-lag**, så du kan bruge det til at rendere HTML ved at forsyne det med data. React gør naturligvis meget mere end en typisk template engine. Forskellen mellem React og andre template engines som Underscore, Handlebars og Mustache ligger i måden, du udvikler UIs, opdaterer dem og styrer deres states. State dækkes i bogens kapitel 4. Indtil videre kan du tænke på **state** som data, der kan ændre sig, og som er relateret til UI'et.

### 1.5.3 The React stack

React er ikke et fuldblods front-end-JavaScript-framework. React er minimalistisk. Det påtvinger ikke en bestemt måde at gøre ting på som data modeling, styling eller routing (det er non-opinionated). Derfor skal udviklere parre React med et routing- og/eller modelleringsbibliotek.

Et projekt, der allerede bruger Backbone.js og Underscore.js template engine, kan for eksempel skifte til Underscore for React og beholde eksisterende datamodeller og routing fra Backbone. (Underscore har også utilities, ikke kun template-metoder. Du kan bruge disse Underscore-utilities med React som en løsning til en klar declarative stil.)

Andre gange vælger udviklere at bruge **React stack**, som består af data- og routing-biblioteker skabt specifikt til React:

- **Data-model libraries and back ends** — RefluxJS, Redux, Meteor og Flux
- **Routing library** — React Router
- **Collection of React components to consume the Twitter Bootstrap library** — React-Bootstrap

Økosystemet af biblioteker til React vokser hver dag. React's evne til at beskrive composable components (selvindeholdte dele af UI'et) hjælper med genbrug af kode. Der er mange components pakket som npm-moduler. Nogle populære eksempler fra bogen:

- Datepicker component: `https://github.com/Hacker0x01/react-datepicker`
- Sæt værktøjer til rendering og validering af formularer: `https://github.com/prometheusresearch/react-forms`
- WAI-ARIA-compliant autocomplete (combo box) component: `https://github.com/reactjs/react-autocomplete`

Så er der **JSX**, som formodentlig er det hyppigste argument imod at bruge React. Er du bekendt med Angular, har du allerede måttet skrive en masse JavaScript i din template-kode. Det skyldes, at almindelig HTML i moderne webudvikling er for statisk og næsten ubrugelig alene. Forfatterens råd: giv React fordelen af tvivlen, og giv JSX en fair chance.

JSX er en lille syntaks til at skrive React-objekter i JavaScript ved brug af `<>` som i XML/HTML. React passer godt sammen med JSX, fordi udviklere bedre kan implementere og læse koden. Tænk på JSX som et minisprog, der kompileres til native JavaScript. JSX køres altså ikke i browseren, men bruges som kildekode til kompilering. Her er et kompakt snippet skrevet i JSX:

```jsx
if (user.session)
return <a href="/logout">Logout</a>
else
return <a href="/login">Login</a>
```

Selv hvis du indlæser en JSX-fil i din browser med runtime-transformer-biblioteket, der kompilerer JSX til native JavaScript on the fly, kører du stadig ikke JSX; du kører JavaScript i stedet. I den forstand ligner JSX CoffeeScript. Man kompilerer disse sprog til native JavaScript for at få bedre syntaks og features end almindeligt JavaScript.

Forfatteren indrømmer, at det kan se bizart ud at have XML blandet ind i JavaScript-kode, og det tog ham et stykke tid at vænne sig til. Og ja, brugen af JSX er valgfri. Af de to grunde dækkes JSX først i bogens kapitel 3.

> *Kurset bruger JSX fra første færd sammen med Vite, som håndterer JSX-transformationen — ikke runtime-transformeren fra 1. udgave.*

---

## 1.6 Your first React code: Hello World

Første React-kode er det klassiske Hello World-eksempel. Der bruges ikke JSX endnu, kun almindeligt JavaScript. Projektet printer en "Hello world!!!"-overskrift (`<h1>`) på en webside.

### Sidebemærkning: Learning React first without JSX

Selvom de fleste React-udviklere skriver i JSX, kører browsere kun standard JavaScript. Derfor er det gavnligt at kunne forstå React-kode i ren JavaScript. En anden grund til at starte med plain JS er at vise, at JSX er valgfrit, om end de facto-standardsproget for React. Endelig kræver preprocessing af JSX noget tooling. Bogen vil have læseren i gang med React så hurtigt som muligt uden at bruge for meget tid på setup; al nødvendig setup til JSX sker i kapitel 3.

Projektets mappestruktur er simpel. Den består af to JavaScript-filer i `js`-mappen og én HTML-fil, `index.html`:

```
/hello-world
/js
react.js
react-dom.js
index.html
```

De to filer i `js`-mappen er React-biblioteket version 15.5.4: `react-dom.js` (web browser DOM renderer) og `react.js` (React Core-pakken). Først skal du downloade React Core og ReactDOM. Bogen anbefaler at bruge filerne fra bogens kildekode på `www.manning.com/books/react-quickly` og `https://github.com/azat-co/react-quickly/tree/master/ch01/hello-world`, fordi det er den mest pålidelige og letteste tilgang uden afhængighed af andre services eller værktøjer.

> **WARNING (bogen):** Før version 0.14 var de to biblioteker bundtet sammen. For version 0.13.3 behøvede man for eksempel kun `react.js`. Bogen bruger React og React DOM version 15.5.4 (den nyeste da bogen blev skrevet), medmindre andet er noteret. Til de fleste projekter i del 1 skal du bruge to filer: `react.js` og `react-com.js`. I kapitel 8 skal du bruge `prop-types`, som var en del af React indtil version 15.5.4, men nu er et separat modul.

> *Kurset bruger en moderne React-version installeret via npm og bundlet med Vite — ikke `<script>`-tags med lokale kopier af react.js og react-dom.js. Eksemplet her er stadig instruktivt, fordi det viser hvad JSX kompilerer ned til.*

Efter du har placeret React-filerne i `js`-mappen, opretter du `index.html` i `hello-world`-projektmappen. Denne HTML-fil er applikationens entry point (det er den, du skal åbne i browseren).

Koden til `index.html` er simpel og starter med at inkludere bibliotekerne i `<head>`. I `<body>`-elementet oprettes en `<div>`-container med ID'et `content` og et `<script>`-element (dér hvor appens kode skal placeres senere):

**Listing 1.1 — Loading React libraries and code (index.html)**

```html
<!DOCTYPE html>
<html>
<head>
<script src="js/react.js"></script>
<script src="js/react-dom.js"></script>
</head>
<body>
<div id="content"></div>
<script type="text/javascript">
...
</script>
</body>
</html>
```

Bogens annotationer til listingen: det første `<script>` importerer React-biblioteket, det andet importerer ReactDOM-biblioteket, `<div id="content">` definerer et tomt `<div>`-element til at montere React-UI'et, og det sidste `<script>` er dér, React-koden til Hello World-view'et starter.

Hvorfor ikke rendere React-elementet direkte i `<body>`-elementet? Fordi det kan føre til konflikt med andre biblioteker og browser-extensions, der manipulerer document body. Forsøger du at hæfte et element direkte på body, får du denne advarsel:

```
Rendering components directly into document.body is discouraged...
```

Det er endnu en god ting ved React: det har gode advarsels- og fejlbeskeder.

> **NOTE (bogen):** React's advarsels- og fejlbeskeder er ikke en del af production-buildet, for at reducere støj, øge sikkerheden og minimere distributionsstørrelsen. Production-buildet er den minificerede fil fra React Core-biblioteket: for eksempel `react.min.js`. Udviklingsversionen med advarsler og fejlbeskeder er den uminificerede version: for eksempel `react.js`.

Ved at inkludere bibliotekerne i HTML-filen får du adgang til de globale React- og ReactDOM-objekter: `window.React` og `window.ReactDOM`. Du får brug for to metoder fra disse objekter: én til at skabe et element (React) og én til at rendere det i `<div>`-containeren (ReactDOM).

For at skabe et React-element skal du kalde `React.createElement(elementName, data, child)` med tre argumenter, som betyder følgende:

- **`elementName`** — HTML som en streng (for eksempel `'h1'`) eller en custom component-klasse som et objekt (for eksempel `HelloWorld`; se afsnit 2.2)
- **`data`** — Data i form af attributter og properties (properties dækkes senere); for eksempel `null` eller `{name: 'Azat'}`
- **`child`** — Child-element eller indre HTML/tekstindhold; for eksempel `Hello world!`

**Listing 1.2 — Creating and rendering an h1 element (index.html)**

```javascript
var h1 = React.createElement('h1', null, 'Hello world!')
ReactDOM.render(
h1,
document.getElementById('content')
)
```

Bogens annotationer: første linje skaber og gemmer i en variabel et React-element af typen `h1`; `ReactDOM.render()` renderer `h1`-elementet i det rigtige DOM-element med ID `"content"`.

Denne listing henter et React-element af typen `h1` og gemmer referencen til objektet i variablen `h1`. Variablen `h1` er ikke en egentlig DOM-node; det er snarere en instantiering af React `h1`-componenten (elementet). Du kan navngive den, som du vil: `helloWorldHeading`, for eksempel. React leverer med andre ord en abstraktion over DOM'en.

> **NOTE (bogen):** Variabelnavnet `h1` er vilkårligt. Du kan kalde variablen hvad som helst (såsom `bananza`), så længe du bruger samme variabel i `ReactDOM.render()`.

Når elementet er skabt og gemt i `h1`, renderer du det til DOM-noden/elementet med ID `content` via `ReactDOM.render()`-metoden. Foretrækker du det, kan du flytte `h1`-variablen ind i render-kaldet. Resultatet er det samme, blot uden en ekstra variabel:

```javascript
ReactDOM.render(
React.createElement('h1', null, 'Hello world!'),
document.getElementById('content')
)
```

> *Kurset bruger `createRoot` fra `react-dom/client` i stedet for `ReactDOM.render()`, som er fjernet i moderne React — se slides L16.*

Åbn nu `index.html`-filen serveret af en statisk HTTP-webserver i din foretrukne browser. Bogen anbefaler en opdateret version af Chrome, Safari eller Firefox. Du bør se "Hello world!"-beskeden på siden.

Figur 1.5 i bogen viser Elements-fanen i Chrome DevTools med `<h1>`-elementet valgt. Man kan observere attributten `data-reactroot`; den indikerer, at elementet blev renderet af ReactDOM.

En hurtig note: du kan abstrahere React-koden (listing 1.2) ud i en separat fil i stedet for at skabe elementer og rendere dem med `ReactDOM.render()` inde i `index.html` (listing 1.1). Du kan for eksempel oprette `script.js` og kopiere `h1`-elementet og `ReactDOM.render()`-kaldet derover. Derefter skal du i `index.html` inkludere `script.js` efter `<div>`'en med ID `content`, sådan her:

```html
<div id="content"></div>
<script src="script.js"></script>
```

### Sidebemærkning: Local dev web server

Det er bedre at bruge en lokal webserver end at åbne en `index.html`-fil direkte i browseren, fordi dine JavaScript-apps med en webserver kan lave AJAX/XHR-requests. Du kan se, om det er en server eller en fil, ved at kigge på URL'en i adresselinjen. Starter adressen med `file`, er det en fil; starter den med `http`, er det en server. Du får brug for denne feature til fremtidige projekter. Typisk lytter en lokal HTTP-webserver efter indkommende requests på `127.0.0.1` eller `localhost`.

Du kan bruge en hvilken som helst open source-webserver, såsom Apache, MAMP, eller — forfatterens favoritter, fordi de er skrevet i Node.js — `node-static` eller `http-server`. For at installere `node-static` eller `http-server` skal du have Node.js og npm installeret.

Antaget at du har Node.js og npm på din maskine, kør `npm i -g node-static` eller `npm i -g http-server` i din terminal eller kommandoprompt. Naviger derefter til mappen med kildekoden, og kør `static` eller `http-server`. I forfatterens tilfælde startes `static` fra `react-quickly`-mappen, så stien til Hello World i browserens URL-linje bliver: `http://localhost:8080/ch01/hello-world/`.

> *Kurset bruger `npm run dev` med Vites dev-server i stedet for `node-static`/`http-server`.*

Tillykke — du har nu implementeret din første React-kode.

---

## 1.7 Quiz

1. Den declarative programmeringsstil tillader ikke mutation af gemte værdier. Det er "dette er hvad jeg vil have" versus den imperative stils "dette er hvordan man gør det". Sandt eller falsk?
2. React components renderes ind i DOM'en med hvilken af følgende metoder? (Pas på, det er et drilsk spørgsmål.) `ReactDOM.renderComponent`, `React.render`, `ReactDOM.append` eller `ReactDOM.render`
3. Du skal bruge Node.js på serveren for at kunne bruge React i din SPA. Sandt eller falsk?
4. Du skal inkludere `react-com.js` for at kunne rendere React-elementer på en webside. Sandt eller falsk?
5. Problemet React løser, er at opdatere views baseret på dataændringer. Sandt eller falsk?

### Quiz answers

1. **Sandt.** Declarative er en "hvad jeg vil have"-stil, og imperative er en "sådan gør man det"-stil.
2. **`ReactDOM.render`.**
3. **Falsk.** Du kan bruge enhver backend-teknologi.
4. **Sandt.** Du skal bruge ReactDOM-biblioteket.
5. **Sandt.** Det er det primære problem, React løser.

> *Note til spørgsmål 2 og 4: I moderne React (18+) er `ReactDOM.render` erstattet af `createRoot(...).render(...)` fra `react-dom/client`. Kursets svar ville være et andet — se slides L16.*

---

## 1.8 Summary

Bogens opsummering af kapitlet:

- React er declarative; det er kun et view- eller UI-lag.
- React bruger components, som du bringer til eksistens med `ReactDOM.render()`.
- React component-klasser skabes med `class` og dens obligatoriske `render()`-metode.
- React components er genbrugelige og tager immutable properties, som er tilgængelige via `this.props.NAME`.
- Du bruger ren JavaScript til at udvikle og komponere UIs i React.
- Du behøver ikke bruge JSX (en XML-lignende syntaks for React-objekter); JSX er valgfrit, når man udvikler med React.
- Sammenfattende definition af React: React for web består af bibliotekerne React Core og ReactDOM. React Core er et bibliotek rettet mod at bygge og dele composable UI components med JavaScript og (valgfrit) JSX på en isomorphic/universal måde. ReactDOM-biblioteket, som man bruger til at arbejde med React i browseren, har metoder til DOM-rendering såvel som til server-side rendering.

> *Punkterne om `class`, den obligatoriske `render()`-metode og `this.props.NAME` er 1.-udgave-terminologi. Kurset bruger i stedet funktionelle komponenter, hvor komponenten selv er en funktion, der modtager `props` som parameter og returnerer JSX — se slides L17.1. `ReactDOM.render()` er ligeledes erstattet af `createRoot`.*

---

## Kurskoblinger (L16)

Kernebegreberne fra dette kapitel, der bærer direkte over i kursets L16, uafhængigt af udgave:

- **Component** som byggesten for UI, og idéen om composable UIs.
- **Declarative** beskrivelse af UI: du beskriver hvordan det skal se ud givet data, ikke hvilke DOM-operationer der skal udføres.
- **Virtual DOM** og **reconciliation**: React sammenligner ny og gammel beskrivelse og opdaterer kun de nødvendige dele af den rigtige DOM.
- **State** som data, der kan ændre sig og som view'et afhænger af; ændring af state udløser rerender.
- **Props** som immutable input til en component.
- **One-way data flow** modsat Angulars two-way binding.
- **SPA**-arkitekturen: browseren henter statiske assets én gang, henter derefter data via HTTP og renderer UI'et lokalt.
- **JSX** som syntaks, der kompileres til `React.createElement`-kald (eller den moderne jsx-runtime). Bogens plain-JS-eksempler viser præcis, hvad JSX kompilerer til — nyttigt til at forstå, hvorfor `key`-attributten hører til på elementet i en `map()`.

Det der ikke bærer over: class components, `React.createClass`, lifecycle-metoder, `ReactDOM.render()`, Flux, `<script>`-tag-baseret setup og Webpack. Kurset bruger funktionelle komponenter med hooks, `createRoot`, Redux Toolkit, Vite og Vitest.
