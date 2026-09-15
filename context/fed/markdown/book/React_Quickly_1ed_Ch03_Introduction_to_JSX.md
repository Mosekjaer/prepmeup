# Kapitel 3 — Introduction to JSX

## Metadata

> **⚠️ Udgave-advarsel:** Denne bog er *React Quickly* **1. udgave (©2017, Azat Mardan)**. Kursusbeskrivelsen foreskriver **2. udgave** (Barklund & Mardan). 1. udgave er skrevet før React Hooks og bruger class components, lifecycle-metoder og Webpack. Kursets React-lektioner (L16–L27) er hook-baserede og bruger Vite, Vitest og Redux Toolkit. **Ved konflikt mellem denne bog og slidesene er slidesene autoritative.**

- **Kapitel:** 3 — Introduction to JSX
- **Bog:** React Quickly, 1. udgave — Azat Mardan, Manning (©2017)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L16/L17
- **Hovedemner:**
  - Hvad JSX er: syntactic sugar for `React.createElement()`, ikke en template engine
  - Fordele ved JSX: læsbarhed (DX), lettere samarbejde med designere, mindre kode at skrive
  - Oprettelse af elements med JSX, herunder komponenter (store forbogstaver)
  - Udskrivning af variabler og JS-expressions med curly braces `{}`
  - Arbejde med props/attributes i JSX, inkl. spread-operatoren `{...this.props}`
  - Komponentmetoder kaldt fra JSX
  - if/else i JSX: variabel, expression, ternary operator, IIFE
  - Kommentarer i JSX
  - Opsætning af JSX-transpiler med Babel CLI
  - React/JSX gotchas: lukkende slash, special characters, `data-`-attributes, `style`-objekt, `className`/`htmlFor`, boolean attribute-værdier

---

## Introduktion

Dette kapitel dækker:

- Forståelse af JSX og dets fordele
- Opsætning af JSX-transpilers med Babel
- Kendskab til React- og JSX-gotchas

Indtil nu har bogen dækket, hvordan man opretter elements og components, så man kan bruge custom elements og organisere sine UI'er bedre. Man har brugt JavaScript til at oprette React-elements i stedet for at arbejde med HTML. Men der er et problem. Se på denne kode og prøv at afgøre, hvad der sker:

```javascript
render() {
return React.createElement(
'div',
{ style: this.styles },
React.createElement(
'p',
null,
React.createElement(
reactRouter.Link,
{ to: this.props.returnTo },
'Back'
)
),
this.props.children
);
}
```

Kunne du se, at der er tre elements, at de er nestede, og at koden bruger en component fra React Router? Hvor læsbar er koden sammenlignet med almindelig HTML? React-teamet er enige i, at det ikke er sjovt at læse (eller skrive) en bunke `React.createElement()`-statements. JSX er løsningen på det problem.

> **NOTE** Kildekoden til eksemplerne i dette kapitel ligger på www.manning.com/books/react-quickly og https://github.com/azat-co/react-quickly/tree/master/ch03 (i mappen `ch03` i GitHub-repositoriet https://github.com/azat-co/react-quickly).

---

## 3.1 Hvad er JSX, og hvad er fordelene?

JSX er en JavaScript-extension, der leverer **syntactic sugar** (sukkerglasur) for funktionskald og objektkonstruktion — særligt `React.createElement()`. Det kan ligne en template engine eller HTML, men det er det ikke. JSX producerer React-elements, samtidig med at man beholder JavaScripts fulde kraft.

JSX er en god måde at skrive React-components på. Fordelene omfatter:

- **Forbedret developer experience (DX)** — Koden er lettere at læse, fordi den er mere veltalende takket være en XML-lignende syntaks, der er bedre til at repræsentere nestede, deklarative strukturer.
- **Mere produktive teammedlemmer** — Ikke-specialiserede udviklere (fx designere) kan lettere ændre koden, fordi JSX ligner HTML, som de allerede kender.
- **Færre håndledsskader og syntaksfejl** — Udviklere har mindre kode at skrive (mindre sukkerglasur), hvilket betyder færre fejl.

Selvom JSX ikke er *påkrævet* for React, passer det godt ind og anbefales stærkt af både forfatteren og React-skaberne. Den officielle "Introducing JSX"-side siger: "We recommend using [JSX] with React."

For at demonstrere JSX' veltalenhed er dette koden til at oprette `HelloWorld` og et `a`-link-element:

```jsx
<div>
<HelloWorld/>
<br/>
<a href="http://webapplog.com">Great JS Resources</a>
</div>
```

Det svarer til følgende JavaScript:

```javascript
React.createElement(
"div",
null,
React.createElement(HelloWorld, null),
React.createElement("br", null),
React.createElement(
"a",
{ href: "http://webapplog.com" },
"Great JS Resources"
)
)
```

Og hvis man bruger Babel v6 (et af værktøjerne til JSX), bliver JS-koden denne:

```javascript
"use strict";
React.createElement(
"div",
null,
" ",
React.createElement(HelloWorld, null),
" ",
React.createElement("br", null),
" ",
React.createElement(
"a",
{ href: "http://webapplog.com" },
"Great JS Resources"
),
" "
);
```

*Kursusnote: Kurset bruger Vite, som håndterer JSX-transformationen via esbuild/SWC. Babel-opsætningen i dette kapitel er ikke længere den anbefalede vej, men den underliggende transformation til `React.createElement()`-kald (eller den nyere automatic JSX runtime) er den samme.*

I bund og grund er JSX et lille sprog med XML-lignende syntaks, men det har ændret måden, folk skriver UI-components på. Tidligere skrev udviklere HTML — og JS-kode til controllers og views — på en MVC-lignende måde, hvor man sprang mellem forskellige filer. Det stammede fra separation of concerns i internettets tidlige dage. Den tilgang tjente webbet godt, dengang det bestod af statisk HTML, lidt CSS og en smule JS til at få tekst til at blinke.

Sådan er det ikke længere; i dag bygger vi højinteraktive UI'er, og JS og HTML er tæt koblede for at implementere funktionalitet. React reparerer det brudte **separation of concerns (SoC)**-princip ved at samle beskrivelsen af UI'et og JS-logikken; og med JSX ligner koden HTML og er lettere at læse og skrive.

JSX kompileres af forskellige transformers (værktøjer) til standard ECMAScript (se figur 3.1). JavaScript *er* ECMAScript, men JSX er ikke en del af specifikationen og har ingen defineret semantik.

**Figur 3.1** — JSX transpileres til almindelig JavaScript:

```
1. JSX  →  2. Transpiler  →  3. JS  →  4. Browser
```

> **NOTE** Ifølge https://en.wikipedia.org/wiki/Source-to-source_compiler: "A source-to-source compiler, transcompiler, or transpiler is a type of compiler that takes the source code of a program written in one programming language as its input and produces the equivalent source code in another programming language."

Man kan spørge: "Hvorfor overhovedet bruge JSX?" Når man ser hvor kontraintuitiv JSX-kode ser ud til at begynde med, er det ikke overraskende, at mange udviklere bliver afskrækket. For eksempel viser denne JSX, at der er vinkelparenteser i JavaScript-koden, hvilket ser bizart ud i starten:

```jsx
ReactDOM.render(<h1>Hello</h1>, document.getElementById('content'))
```

Det, der gør JSX fantastisk, er genvejene til `React.createElement(NAME, ...)`. I stedet for at skrive det funktionskald igen og igen kan man bruge `<NAME/>`. Og som sagt: jo mindre man skriver, jo færre fejl laver man. Med JSX er DX lige så vigtigt som user experience (UX).

Hovedgrunden til at bruge JSX er, at mange finder kode med vinkelparenteser (`< >`) lettere at læse end kode med mange `React.createElement()`-statements (selv når de er aliased). Og når man først har vænnet sig til at tænke på `<NAME/>` ikke som XML, men som et alias for JavaScript-kode, kommer man over den oplevede mærkelighed.

### Sidebar: Alternative genveje

For at være retfærdig findes der nogle få alternativer til JSX, når det handler om at undgå at skrive verbose `React.createElement()`-kald. Et af dem er at bruge aliasset `React.DOM.*`. I stedet for at oprette et `<h1/>`-element med

```javascript
React.createElement('h1', null, 'Hey')
```

vil følgende også være tilstrækkeligt og kræver mindre plads og tid:

```javascript
React.DOM.h1(null, 'Hey')
```

Man har adgang til alle standard-HTML-elements i `React.DOM`-objektet, som man kan inspicere som ethvert andet objekt:

```javascript
console.log(React.DOM)
```

Man kan også skrive `React.DOM` og trykke Enter i Chrome DevTools-konsollen. (Bemærk at `React.DOM` og `ReactDOM` er to helt forskellige objekter og ikke må forveksles eller bruges i flæng.)

Et andet alternativ, som den officielle React-dokumentation anbefaler i situationer, hvor JSX er upraktisk (fx når der ikke er nogen build-proces), er at bruge en kort variabel. For eksempel kan man oprette en variabel `E`:

```javascript
const E = React.createElement
E('h1', null, 'Hey')
```

*Kursusnote: `React.DOM.*` er fjernet fra moderne React, og kurset bruger JSX gennemgående med Vite. Betragt disse alternativer som historisk kontekst.*

JSX skal transpileres (eller kompileres, som det ofte kaldes) til almindelig JavaScript, før browsere kan eksekvere koden. De forskellige metoder gennemgås i afsnit 3.3.

---

## 3.2 Forståelse af JSX

Lad os undersøge, hvordan man arbejder med JSX. Man kan læse afsnittet som reference, eller — hvis man foretrækker at have kodeeksemplerne kørende på sin computer — har man følgende muligheder:

- Opsæt en JSX-transpiler med Babel på computeren som vist i afsnit 3.3.
- Brug online-tjenesten Babel REPL (https://babeljs.io/repl), som transpilerer JSX til JavaScript i browseren.

Anbefalingen er at læse om hovedkoncepterne i JSX først og derefter lave Babel-opsætningen.

### 3.2.1 Oprettelse af elements med JSX

At oprette `ReactElement`-objekter med JSX er ligetil. I stedet for at skrive følgende JavaScript (hvor `name` er en streng — `h1` — eller et component class-objekt — `HelloWorld`)

```javascript
React.createElement(
name,
{key1: value1, key2: value2, ...},
child1, child2, child3, ..., childN
)
```

kan man skrive denne JSX:

```jsx
<name key1=value1 key2=value2 ...>
<child1/>
<child2/>
<child3/>
...
<childN/>
</name>
```

I JSX-koden kommer attributes og deres værdier (fx `key1=value1`) fra andet argument til `createElement()`. Her er "Hello World" i JavaScript (`ch03/helloworld/index.html`).

**Listing 3.1 — Hello World i JavaScript**

```javascript
ReactDOM.render(
React.createElement('h1', null, 'Hello world!'),
document.getElementById('content')
)
```

JSX-versionen er meget mere kompakt (`ch03/hello-world-jsx/js/script.jsx`).

**Listing 3.2 — Hello World i JSX**

```jsx
ReactDOM.render(
<h1>Hello world!</h1>,
document.getElementById('content')
)
```

Man kan også gemme objekter oprettet med JSX-syntaks i variabler, fordi JSX blot er en syntaktisk forbedring af `React.createElement()`. Dette eksempel gemmer referencen til `Element`-objektet i en variabel:

```jsx
let helloWorldReactElement = <h1>Hello world!</h1>
ReactDOM.render(
helloWorldReactElement,
document.getElementById('content')
)
```

### 3.2.2 Arbejde med JSX i components

Det foregående eksempel brugte JSX-tagget `<h1>`, som også er et standard-HTML-tagnavn. Når man arbejder med components, bruger man samme syntaks. Den eneste forskel er, at **component class-navnet skal starte med stort bogstav**, som i `<HelloWorld/>`.

**Listing 3.3 — Oprettelse af en HelloWorld-klasse i JSX**

```jsx
class HelloWorld extends React.Component {
render() {
return (
<div>
<h1>1. Hello world!</h1>
<h1>2. Hello world!</h1>
</div>
)
}
}
ReactDOM.render(
<HelloWorld/>,
document.getElementById('content')
)
```

*Kursusnote: Kurset bruger funktionelle komponenter (`function HelloWorld() { return (...) }`) og hooks i stedet for `class ... extends React.Component`. JSX-syntaksen i return-værdien er identisk.*

Kan man læse listing 3.3 lettere end følgende JavaScript-kode?

```javascript
class HelloWorld extends React.Component {
render() {
return React.createElement('div',
null,
React.createElement('h1', null, '1. Hello world!'),
React.createElement('h1', null, '2. Hello world!'))
}
}
ReactDOM.render(
React.createElement(HelloWorld, null),
document.getElementById('content')
)
```

> **NOTE** At se vinkelparenteser i JavaScript-kode kan være mærkeligt for erfarne JavaScript-udviklere. Parenteserne er den primære kontrovers omkring JSX og en af de hyppigste indvendinger — derfor dykker bogen ned i JSX tidligt.

Bemærk parenteserne efter `return` i JSX-koden i listing 3.3; man **skal** inkludere dem, hvis man ikke skriver noget på samme linje efter `return`. Hvis man starter sit øverste element, `<div>`, på en ny linje, skal man sætte parenteser `()` omkring det. Ellers vil JavaScript afslutte `return` med ingenting (automatic semicolon insertion). Denne stil er:

```jsx
render() {
return (
<div>
</div>
)
}
```

Alternativt kan man starte sit øverste element på samme linje som `return` og undgå de nødvendige `()`. Dette er også gyldigt:

```jsx
render() {
return <div>
</div>
}
```

En ulempe ved den anden tilgang er den reducerede synlighed af det åbnende `<div>`-tag: det er let at overse i koden. Valget er ens eget; bogen bruger begge stilarter.

### 3.2.3 Udskrivning af variabler i JSX

Når man sammensætter components, vil man have dem til at være smarte nok til at ændre view'et baseret på noget kode. For eksempel ville det være nyttigt, hvis en "current date-time"-component brugte en aktuel dato og tid, ikke en hardkodet værdi.

Når man arbejder med ren JavaScript-React, må man ty til konkatenering (`+`) eller — hvis man bruger ES6+/ES2015+ — string templates markeret med backtick og `${varName}`. Det officielle navn for denne feature er **template literal**. For at bruge en property i tekst i en `DateTimeNow`-component i almindelig JavaScript-React ville man skrive:

```javascript
class DateTimeNow extends React.Component {
render() {
let dateTimeNow = new Date().toLocaleString()
return React.createElement(
'span',
null,
`Current date and time is ${dateTimeNow}.`
)
}
}
```

Omvendt kan man i JSX bruge **curly braces `{}`**-notation til at udskrive variabler dynamisk, hvilket reducerer kodemængden væsentligt:

```jsx
class DateTimeNow extends React.Component {
render() {
let dateTimeNow = new Date().toLocaleString()
return <span>Current date and time is {dateTimeNow}.</span>
)
}
}
```

Variablerne kan være properties, ikke kun lokalt definerede variabler:

```jsx
<span>Hello {this.props.userName}, your current date and time is
 {dateTimeNow}.</span>
```

Desuden kan man eksekvere JavaScript-**expressions** eller vilkårlig JS-kode inde i `{}`. For eksempel kan man formatere en dato:

```jsx
<p>Current time in your locale is
 {new Date(Date.now()).toLocaleTimeString()}</p>
```

Nu kan man omskrive `HelloWorld`-klassen i JSX ved brug af de dynamiske data, som JSX gemmer i en variabel (`ch03/hello-world-class-jsx`).

**Listing 3.4 — Udskrivning af variabler i JSX**

```jsx
let helloWorldReactElement = <h1>Hello world!</h1>
class HelloWorld extends React.Component {
render() {
return <div>
{helloWorldReactElement}
{helloWorldReactElement}
</div>
}
}
ReactDOM.render(
<HelloWorld/>,
document.getElementById('content')
)
```

### 3.2.4 Arbejde med properties i JSX

Element-properties defineres med **attribute-syntaks**. Man bruger `key1=value1 key2=value2…`-notation inde i JSX-tagget til at definere både HTML-attributes og React component-properties. Det svarer til attribute-syntaks i HTML/XML.

Hvis man skal sende properties videre, skriver man dem i JSX, som man ville i normal HTML. Man renderer også standard-HTML-attributes ved at sætte element-properties (jf. afsnit 2.3). For eksempel sætter denne kode standard-HTML-attributten `href` for anchor-elementet `<a>`:

```jsx
ReactDOM.render((
<div>
<a href="http://reactquickly.co">Time for React?</a>
<DateTimeNow userName='Azat'/>
</div>
),
document.getElementById('content')
)
```

Her renderer `href` en standard-HTML-attribut, og `userName` sætter en værdi for en property på componenten.

Hardkodede værdier for attributes er ikke fleksible. Hvis man vil genbruge link-componenten, skal `href` ændres, så den afspejler en anden adresse hver gang. Det kaldes **dynamisk værdisætning** modsat hardkodning. Man bruger curly braces (`{}`) inde i vinkelparenteser (`<>`) til at sende dynamiske property-værdier til elements.

Antag, at man bygger en component, der skal linke til brugerkonti. `href` og `title` skal være forskellige og ikke hardkodede. En dynamisk component `ProfileLink` renderer et link `<a>` ved hjælp af properties `url` og `label` til henholdsvis `href` og `title`. I `ProfileLink` sender man properties videre til `<a>` med `{}`:

```jsx
class ProfileLink extends React.Component {
render() {
return <a href={this.props.url}
title={this.props.label}
target="_blank">Profile
</a>
}
}
```

Hvor kommer property-værdierne fra? De defineres, når `ProfileLink` oprettes — altså i den component, der opretter `ProfileLink`, dvs. dens parent:

```jsx
<ProfileLink url='/users/azat' label='Profile for Azat'/>
```

Fra det foregående kapitel: når React renderer standard-elements (`<h>`, `<p>`, `<div>`, `<a>` osv.), renderer React alle attributes fra HTML-specifikationen og udelader alle andre attributes, der ikke er en del af specifikationen. Det er ikke en JSX-gotcha; det er Reacts adfærd.

Men nogle gange vil man tilføje custom data som en attribut. Et almindeligt mønster er at lægge informationen i DOM-elementet som en attribut. Dette eksempel bruger attributterne `react-is-awesome` og `id`:

```html
<li react-is-awesome="true" id="320">React is awesome!</li>
```

At gemme data i custom HTML-attributes i DOM'en betragtes generelt som et **antipattern**, fordi man ikke vil have DOM'en til at være sin database eller front-end data store. At hente data fra DOM'en er langsommere end fra et virtuelt/in-memory store.

I tilfælde hvor man *skal* gemme data som elementers attributes, og man bruger JSX, skal man bruge præfikset `data-NAME`. For at rendere `<li>`-elementet med værdien af `this.reactIsAwesome` i en attribut kan man skrive:

```jsx
<li data-react-is-awesome={this.reactIsAwesome}>React is awesome!</li>
```

Hvis `this.reactIsAwesome` er `true`, bliver den resulterende HTML:

```html
<li data-react-is-awesome="true">React is awesome!</li>
```

Men hvis man forsøger at sende en ikke-standard HTML-attribut til et standard-HTML-element, renderes attributten ikke (jf. afsnit 2.3). For eksempel denne kode

```jsx
<li react-is-awesome={this.reactIsAwesome}>React is orange</li>
```

og denne kode

```jsx
<li reactIsAwesome={this.reactIsAwesome}>React is orange</li>
```

producerer begge kun følgende:

```html
<li>React is orange</li>
```

Da custom elements (component classes) ikke har indbyggede renderers og er afhængige af standard-HTML-elements eller andre custom elements, er `data-`-problemstillingen ikke vigtig for dem. De får alle attributes som properties i `this.props`.

Apropos component classes — dette er koden fra Hello World (afsnit 2.3) skrevet i almindelig JavaScript:

```javascript
class HelloWorld extends React.Component {
render() {
return React.createElement(
'h1',
this.props,
'Hello ' + this.props.frameworkName + ' world!!!'
)
}
}
```

I `HelloWorld`-componenten sender man properties videre til `<h1>` uanset hvilke properties der er. Hvordan gør man det i JSX? Man vil ikke sende hver property individuelt, fordi det er mere kode; og når en property skal ændres, får man tæt koblet kode, der også skal opdateres. Forestil dig at skulle sende hver property manuelt — og hvad hvis man har to eller tre niveauer af components at sende igennem? Det er et antipattern. **Gør ikke dette:**

```jsx
class HelloWorld extends React.Component {
render() {
return <h1 title={this.props.title} id={this.props.id}>
Hello {this.props.frameworkName} world!!!
</h1>
}
}
```

Send ikke properties individuelt, når hensigten er at sende dem alle; JSX tilbyder en **spread**-løsning, der ser ud som ellipser, `...` (`ch03/jsx/hello-js-world-jsx`).

**Listing 3.5 — Arbejde med properties**

```jsx
class HelloWorld extends React.Component {
render() {
return <h1 {...this.properties}>
Hello {this.props.frameworkName} world!!!
</h1>
}
}
ReactDOM.render(
<div>
<HelloWorld
id='ember'
frameworkName='Ember.js'
title='A framework for creating ambitious web applications.'/>,
<HelloWorld
id='backbone'
frameworkName= 'Backbone.js'
title= 'Backbone.js gives structure to web applications...'/>
<HelloWorld
id= 'angular'
frameworkName= 'Angular.js'
title= 'Superheroic JavaScript MVW Framework'/>
</div>,
document.getElementById('content')
)
```

Med `{...this.props}` kan man sende hver property videre til barnet.

### Sidebar: Ellipser i ES6+/ES2015+ — rest, spread og destructuring

Apropos ellipser findes der lignende operatorer i ES6+, kaldet **destructuring**, **spread** og **rest**. Det er en af grundene til, at Reacts JSX bruger ellipser.

Hvis man nogensinde har skrevet en JavaScript-funktion med et variabelt eller ubegrænset antal argumenter, kender man `arguments`-objektet. Problemet er, at `arguments` ikke er et rigtigt array. Man skal konvertere det til et array, hvis man vil bruge funktioner som `sort()` og `map()`. Denne `request`-funktion konverterer `arguments` med `call()`:

```javascript
function request(url, options, callback) {
var args = Array.prototype.slice.call(arguments, request.length)
var url = args[0]
var callback = args[2]
// ...
}
```

Findes der en bedre måde i ES6 til at tilgå et ubestemt antal argumenter som et array? Ja — **rest parameter**-syntaksen, defineret med ellipser (`…`). Følgende er ES6-funktionssignaturen med rest-parameteren `callbacks`, som bliver et rigtigt array:

```javascript
function(url, options, ...callbacks) {
var callback1 = callbacks[0]
var callback2 = callbacks[1]
// ...
}
```

> I rest-arrayet er den første parameter den, der ikke har et navn: fx er callback'et på index 0, ikke 2 som i ES5's `arguments`. Desuden vil andre navngivne argumenter efter rest-parameteren give en syntaksfejl.

Rest-parametre kan **destructures**, dvs. udtrækkes til separate variabler:

```javascript
function(url, options, ...[error, success]) {
if (!url) return error(new Error('ooops'))
// ...
success(data)
}
```

Hvad med **spread**? Kort sagt tillader spread én at udfolde argumenter eller variabler følgende steder:

- **Funktionskald** — fx `push()`-metoden: `arr1.push(…arr2)`
- **Array-literals** — fx `array2 = [...array1, x, y, z]`
- **`new`-funktionskald (constructors)** — fx `var d = new Date(...dates)`

I ES5 skulle man bruge `apply()`, hvis man ville bruge et array som argumenter til en funktion:

```javascript
function request(url, options, callback) {
// ...
}
var requestArgs = ['http://azat.co', {...}, function(){...}]
request.apply(null, requestArgs)
```

I ES6 kan man bruge spread-parameteren:

```javascript
function request(url, options, callback) {
// ...
}
var requestArgs = ['http://azat.co', {...}, function(){...}]
request(...requestArgs)
```

Spread-operatorens syntaks ligner rest-parameterens, men rest bruges i en funktionsdefinition/-deklaration, og spread bruges i kald og literals.

### 3.2.5 Oprettelse af React component-metoder

Som udvikler er man fri til at skrive vilkårlige component-metoder, fordi en React-component er en klasse. For eksempel kan man oprette en helper-metode `getUrl()`:

```javascript
class Content extends React.Component {
getUrl() {
return 'http://webapplog.com'
}
render() {
...
}
}
```

`getUrl()`-metoden er ikke sofistikeret, men pointen er: man kan oprette sine egne vilkårlige metoder, ikke kun `render()`. Helper-metoder kan indeholde genbrugelig logik, og man kan kalde dem hvor som helst i componentens andre metoder, inklusive `render()`.

Hvis man vil udskrive returværdien fra den custom metode i JSX, bruger man `{}` ligesom med variabler. Husk at kalde metoden med `()`.

**Listing 3.6 — Kald af en component-metode for at hente en URL**

```jsx
class Content extends React.Component {
getUrl() {
return 'http://webapplog.com'
}
render() {
return (
<div>
<p>Your REST API URL is:
<a href={this.getUrl()}>
{this.getUrl()}
</a>
</p>
</div>
)
}
}
...
```

Klassemetoden kaldes altså inde i curly braces. Når man bruger metoden i listing 3.6, ser man `http://webapplog.com` som returværdi i linket i paragraffen `<p>` (figur 3.2).

Disse metoder er vigtige som fundament for Reacts event handlers.

*Kursusnote: I funktionelle komponenter defineres tilsvarende helpers som almindelige funktioner inde i (eller uden for) komponentfunktionen og kaldes uden `this.`.*

### 3.2.6 if/else i JSX

Ligesom med dynamiske variabler har udviklere brug for at komponere components, så de kan ændre view baseret på resultatet af **if/else**-betingelser.

Lad os starte med et simpelt eksempel, der renderer elements i en component class afhængigt af en betingelse. Fx bestemmes linktekst og URL af værdien `user.session`. Sådan kan det kodes i ren JS:

```javascript
...
render() {
if (user.session)
return React.createElement('a', {href: '/logout'}, 'Logout')
else
return React.createElement('a', {href: '/login'}, 'Login')
}
...
```

Man kan bruge en lignende tilgang og skrive det om med JSX:

```jsx
...
render() {
if (this.props.user.session)
return <a href="/logout">Logout</a>
else
return <a href="/login">Login</a>
}
...
```

Antag, at der er andre elements, såsom en `<div>`-wrapper. I ren JS ville man skulle oprette en variabel eller bruge en expression eller en ternary operator (også kaldet Elvis-operatoren), fordi man ikke kan bruge en `if`-betingelse inde i `<div>`'ens `createElement()`. Idéen er, at man skal have værdien ved runtime.

#### Sidebar: Ternary operators

Følgende ternary-betingelse virker sådan, at hvis `userAuth` er `true`, sættes `msg` til `welcome`. Ellers bliver værdien `restricted`:

```javascript
let msg = (userAuth) ? 'welcome' : 'restricted'
```

Dette statement svarer til følgende:

```javascript
let session = ''
if (userAuth) {
session = 'welcome'
} else {
session = 'restricted'
}
```

I nogle tilfælde er ternary-operatoren (`?`) en kortere version af if/else. Men der er en stor forskel, hvis man prøver at bruge ternary-operatoren som en **expression** (hvor den returnerer en værdi). Denne kode er gyldig JS:

```javascript
let msg = (userAuth) ? 'welcome' : 'restricted'
```

Men if/else virker ikke, fordi det ikke er en expression, men et statement:

```javascript
let msg = if (userAuth) {'welcome'} else {'restricted'} // Not valid
```

Man kan udnytte denne egenskab ved ternary-operatoren til at få en værdi fra den ved runtime i JSX.

For at demonstrere de tre forskellige stilarter (variabel, expression og ternary operator), se følgende almindelige JavaScript-kode, før den konverteres til JSX:

```javascript
// Approach 1: Variable
render() {
let link
if (this.props.user.session)
link = React.createElement('a', {href: '/logout'}, 'Logout')
else
link = React.createElement('a', {href: '/login'}, 'Login')
return React.createElement('div', null, link)
}
// Approach 2: Expression
render() {
let link = (sessionFlag) => {
if (sessionFlag)
return React.createElement('a', {href: '/logout'}, 'Logout')
else
return React.createElement('a', {href: '/login'}, 'Login')
}
return React.createElement('div', null, link(this.props.user.session))
}
// Approach 3: Ternary operator
render() {
return React.createElement('div', null,
(this.props.user.session) ? React.createElement('a', {href: '/logout'},
 'Logout') : React.createElement('a', {href: '/login'}, 'Login')
)
}
```

Ikke dårligt, men lidt klodset. Med JSX kan `{}`-notationen udskrive variabler og eksekvere JS-kode. Lad os bruge det til at opnå bedre syntaks:

```jsx
// Approach 1: Variable
render() {
let link
if (this.props.user.session)
link = <a href='/logout'>Logout</a>
else
link = <a href='/login'>Login</a>
return <div>{link}</div>
}
// Approach 2: Expression
render() {
let link = (sessionFlag) => {
if (sessionFlag)
return <a href='/logout'>Logout</a>
else
return <a href='/login'>Login</a>
}
return <div>{link(this.props.user.session)}</div>
}
// Approach 3: Ternary operator
render() {
return <div>
{(this.props.user.session) ? <a href='/logout'>Logout</a> :
 <a href='/login'>Login</a>}
</div>
}
```

Ser man nærmere på expression/function-stilen (Approach 2: en funktion uden for JSX før `return`), kan man finde et alternativ. Man kan definere den samme funktion med et **immediately invoked function expression (IIFE)** inde i JSX. Det lader én undgå en ekstra variabel (som `link`) og eksekvere if/else ved runtime:

```jsx
render() {
return <div>{
(sessionFlag) => {
if (sessionFlag)
return <a href='/logout'>Logout</a>
else
return <a href='/login'>Login</a>
}(this.props.user.session)
}</div>
}
```

Her defineres en IIFE, som kaldes med en parameter.

Man kan bruge de samme principper til at rendere ikke bare hele elements (`<a>` i eksemplerne), men også tekst og værdier af properties. Alt man skal gøre er at bruge en af tilgangene inde i curly braces. Man kan fx tilpasse URL og tekst uden at duplikere koden til element-oprettelse. Forfatterens foretrukne tilgang, fordi den kun bruger ét `<a>`:

```jsx
render() {
let sessionFlag = this.props.user.session
return <div>
<a href={(sessionFlag)?'/logout':'/login'}>
{(sessionFlag)?'Logout':'Login'}
</a>
</div>
}
```

Her oprettes en lokal variabel til at gemme session-boolean-værdien (mindre kode og bedre performance), ternary-operatoren renderer forskellige URL'er baseret på `sessionFlag`, og ternary-operatoren renderer også forskellig tekst.

Som man kan se, findes der — modsat template engines — ingen speciel syntaks til disse betingelser i JSX; man bruger bare JavaScript. Oftest bruger man en ternary operator, fordi den er en af de mest kompakte stilarter. Opsummeret kan man bruge disse muligheder for if/else-logik i JSX:

- Variabel defineret uden for JSX (før `return`) og udskrevet med `{}` i JSX
- Expression (funktion der returnerer en værdi) defineret uden for JSX (før `return`) og kaldt i `{}` i JSX
- Conditional ternary operator
- IIFE i JSX

Forfatterens tommelfingerregel: brug if/else uden for JSX (før `return`) til at generere en variabel, som du udskriver i JSX med `{}`. Eller drop variablen og udskriv resultatet af Elvis-operatoren (`?`) eller expressions med `{}` i JSX:

```jsx
class MyReactComponent extends React.Component {
render() {
// Not JSX: Use a variable and if/else or ternary
return (
// JSX: Print result of ternary or expression with {}
)
}
}
```

### 3.2.7 Kommentarer i JSX

Kommentarer i JSX fungerer på samme måde som kommentarer i almindelig JavaScript. For at tilføje JSX-kommentarer kan man wrappe standard-JavaScript-kommentarer i `{}`:

```jsx
let content = (
<div>
{/* Just like a JS comment */}
</div>
)
```

Eller man kan bruge kommentarer sådan her:

```jsx
let content = (
<div>
<Post
/* I
am
multi
line */
name={window.isLoggedIn ? window.name : ''} // We are inside of JSX
/>
</div>
)
```

Før man kan fortsætte, skal man forstå, at JSX skal kompileres for at et projekt kan fungere. Browsere kan ikke køre JSX — de kan kun køre JavaScript, så man skal transpilere JSX til normal JS (figur 3.1).

---

## 3.3 Opsætning af en JSX-transpiler med Babel

For at eksekvere JSX skal man konvertere det til almindelig JavaScript-kode. Processen kaldes **transpilation** (fra compilation og transformation), og flere værktøjer kan gøre arbejdet. Anbefalede måder:

- **Babel command-line interface (CLI)-værktøj** — Pakken `babel-cli` leverer en kommando til transpilation. Denne tilgang kræver mindst opsætning og er lettest at komme i gang med.
- **Node.js- eller browser-JavaScript-script (API-tilgang)** — Et script kan importere pakken `babel-core` og transpilere JSX programmatisk (`babel.transform`). Det giver low-level-kontrol og fjerner abstraktioner og afhængigheder til build-værktøjer og deres plugins.
- **Build-værktøj** — Et værktøj som Grunt, Gulp eller Webpack kan bruge Babel-pluginnet. Det er den mest populære tilgang.

Alle bruger Babel på den ene eller anden måde. Babel er hovedsageligt en ES6+/ES2015+-compiler, men den kan også konvertere JSX til JavaScript. React-teamet stoppede faktisk udviklingen af deres egen JSX-transformer og anbefaler Babel.

*Kursusnote: Kurset bruger Vite som build-værktøj. Vite håndterer JSX-transformationen automatisk for `.jsx`-filer uden manuel Babel-opsætning, og dev-serveren har hot module replacement indbygget. Hele dette afsnit er derfor primært historisk kontekst — kend princippet (JSX skal transpileres), ikke Babel-CLI-kommandoerne.*

### Sidebar: Kan jeg bruge noget andet end Babel 6?

Selvom der findes forskellige værktøjer til at transpilere JSX, er det mest anvendte — og det React-teamet anbefalede på den officielle React-website pr. august 2016 — Babel (tidligere 5to6). Historisk vedligeholdt React-teamet `react-tools` og `JSXTransformer` (transpilation i browseren); men siden version 0.13 har teamet anbefalet Babel og stoppet udviklingen af `react-tools` og `JSXTransformer`.

Til in-browser runtime-transpilation har Babel version 5.x `browser.js`, som er en klar-til-brug-distribution. Man kan lægge den i browseren som `JSXTransformer`, og den vil konvertere enhver `<script>`-kode til JS (brug `type="text/babel"`). Den seneste Babel-version med `browser.js` er 5.8.34, og man kan inkludere den direkte fra CDN (https://cdnjs.com/libraries/babel-core/5.8.34).

Babel 6.x skiftede til ikke at have default presets/configs (såsom JSX) og fjernede `browser.js`. Babel-teamet opfordrer udviklere til at lave deres egne distributioner eller bruge Babel-API'et. Der findes også et `babel-standalone`-bibliotek (https://github.com/Daniel15/babel-standalone), men man skal stadig fortælle det, hvilke presets/configs der skal bruges.

Traceur (https://github.com/google/traceur-compiler) er et andet værktøj, man kan bruge som erstatning for Babel.

Endelig ser TypeScript (www.typescriptlang.org) ud til at understøtte JSX-kompilering via `jsx-typescript` (https://github.com/fdecampredon/jsx-typescript), men det er en helt ny toolchain og et nyt sprog (et supersæt af almindelig JavaScript).

Man kan formentlig bruge `JSXTransformer`, Babel v5, `babel-standalone`, TypeScript og Traceur med bogens eksempler (bogen bruger React v15). TypeScript og Traceur bør være relativt sikre valg. Men bruger man noget andet end Babel 6 til bogens eksempler, gør man det på eget ansvar.

Ved at bruge Babel til React får man ekstra ES6/ES2015-features ved blot at tilføje en ekstra konfiguration og et modul til ES6. Den sjette iteration af ECMAScript-standarden har utallige forbedringer og er for det meste tilgængelig i alle moderne browsere. Men ældre browsere har svært ved at fortolke den nye ES6-kode. Og hvis man vil bruge ES7, ES8 eller ES27, har nogle browsere måske ikke alle features implementeret endnu.

For at løse forsinkelsen i browsernes ES6- eller ES.Next-implementering kommer Babel til undsætning. Dette afsnit dækker den anbefalede tilgang, som bruges i de næste kapitler — Babel CLI — fordi den involverer minimal opsætning og ikke kræver kendskab til Babels API.

For at bruge Babel CLI (http://babeljs.io) skal man have Node v6.2.0, npm v3.8.9, `babel-cli` v6.9.0 og `babel-preset-react` v6.5.0. Andre versioner er ikke garanteret at virke med bogens kode på grund af Node- og React-udviklingens hurtige tempo.

Hvis man skal installere Node og npm, er den letteste måde at downloade installeren (én for både Node og npm) fra den officielle website: http://nodejs.org.

Hvis man tror, man har værktøjerne installeret, eller er usikker, kan man tjekke versionerne af Node og npm med disse shell/terminal-kommandoer:

```bash
node -v
npm -v
```

Man skal have Babel CLI og React-presettet **lokalt**. At bruge Babel CLI globalt (`-g` ved npm-installation) frarådes, fordi man kan løbe ind i konflikter, når ens projekter er afhængige af forskellige versioner af værktøjet. Her er en kort version af instruktionerne fra appendiks A:

1. Opret en ny mappe, fx `ch03/babel-jsx-test`.
2. Opret en `package.json`-fil i den nye mappe og indtast et tomt objekt `{}` i den, eller brug `npm init` til at generere filen.
3. Definér dine Babel-presets i `package.json` (brugt i bogen og forklaret i næste afsnit) eller `.babelrc` (ikke brugt i bogen).
4. Valgfrit: udfyld `package.json` med information som projektnavn, licens, GitHub-repository osv.
5. Installér Babel CLI og React-presettet lokalt med `npm i babel-cli@6.9.0 babel-preset-react@6.5.0 --save-dev` for at gemme afhængighederne i `devDependencies` i `package.json`.
6. Valgfrit: opret et npm-script med en af Babel-kommandoerne beskrevet nedenfor.

### Sidebar: Babel ES6-preset

I det uheldige tilfælde, at man skal understøtte en ældre browser som IE9, men stadig vil skrive i ES6+/ES2015+, kan man tilføje transpileren `babel-preset-es2015`. Den konverterer ES6 til ES5-kode. Installér biblioteket:

```bash
npm i babel-preset-es2015 --save-dev
```

Tilføj det derefter til presets-konfigurationen ved siden af `react`:

```json
{
"presets": ["react", "es2015"]
}
```

Forfatteren anbefaler ikke at bruge denne ES2015-transpiler, hvis man ikke behøver at understøtte ældre browsere, af flere grunde. For det første vil man køre gammel ES5-kode, som er mindre optimeret end ES6-kode. For det andet tilføjer man en ekstra afhængighed og mere kompleksitet. For det tredje: hvis de fleste fortsætter med at køre ES5-kode i browseren, hvorfor gad browser-teams og JavaScript-udviklere så overhovedet ES6?

For at gentage, hvad der står i appendiks A, skal man bruge en `package.json`-fil med mindst dette preset:

```json
{
...
"babel": {
"presets": ["react"]
},
...
}
```

Derefter bør denne kommando (fra den nyoprettede projektmappe) virke til at tjekke versionen:

```bash
$ ./node_modules/.bin/babel --version
```

Efter installation udstedes en kommando til at behandle `js/script.jsx` (JSX) til `js/script.js` (JavaScript):

```bash
$ ./node_modules/.bin/babel js/script.jsx -o js/script.js
```

Kommandoen er lang, fordi man bruger en sti til Babel. Man kan gemme kommandoen i en `package.json`-fil for at bruge en kortere version: `npm run build`. Åbn filen i din editor og tilføj denne linje til `scripts`:

```json
"build": "./node_modules/.bin/babel js/script.jsx -o js/script.js"
```

Man kan automatisere kommandoen med watch-optionen (`-w` eller `--watch`):

```bash
$ ./node_modules/.bin/babel js/script.jsx -o js/script.js -w
```

Babel-kommandoen holder øje med ændringer i `script.jsx` og kompilerer til `script.js`, når man gemmer den opdaterede JSX. Når det sker, viser terminalen følgende:

```
change js/script.jsx
```

Efterhånden som man akkumulerer flere JSX-filer, bruger man kommandoen med `-d` (`--out-dir`) og mappenavne til at kompilere JSX-kildefiler (`source`) til mange almindelige JS-filer (`build`):

```bash
$ ./node_modules/.bin/babel source --d build
```

Ofte er det bedre for en front-end-apps performance at have én fil at loade end mange filer, fordi hver request tilføjer forsinkelse. Man kan kompilere alle filer i `source`-mappen til én enkelt JS-fil med `-o` (`--out-file`):

```bash
$ ./node_modules/.bin/babel src -o script-compiled.js
```

Afhængigt af sti-konfigurationen på computeren kan man måske køre `babel` i stedet for `./node_modules/.bin/babel`. I begge tilfælde eksekverer man lokalt. Har man en ældre `babel-cli` installeret globalt, sletter man den med `npm rm -g babel-cli`.

Hvis man ikke kan køre `babel`, når man installerer `babel-cli` lokalt i sit projekt, kan man overveje at tilføje et af disse sti-statements til sin shell-profil: `~/.bash_profile`, `~/.bashrc` eller `~/.zsh`, afhængigt af ens shell, hvis man er på POSIX (Unix, Linux, macOS og lignende).

Dette shell-statement tilføjer en sti — så man kan starte lokalt installerede npm CLI-pakker uden at skrive stien — hvis der findes `./node_modules/.bin` i den aktuelle mappe:

```bash
if [ -d "$PWD/node_modules/.bin" ]; then
PATH="$PWD/node_modules/.bin"
fi
```

Shell-scriptet tjekker, om der findes en `./node_modules/.bin`-mappe i den aktuelle mappe i dit terminal-bash-miljø, og tilføjer så mappen til stien, så npm CLI-værktøjer som Babel, Webpack osv. kan kaldes ved navn.

Man kan vælge at få stien sat hele tiden, ikke kun når der er en undermappe. Dette shell-statement tilføjer altid stien `./node_modules/.bin` til din `PATH`-miljøvariabel:

```bash
export PATH="./node_modules/.bin:$PATH"
```

Bonus: denne indstilling lader dig også køre ethvert npm CLI-værktøj lokalt med blot dets navn, ikke stien og navnet.

> **TIP** For fungerende eksempler på Babel-`package.json`-konfigurationer, åbn projekterne i `ch03`-mappen i bogens kildekode. `package.json`-filen i `ch03` har npm build-scripts for hvert projekt (undermappe), der kræver kompilering, medmindre projektet har sin egen `package.json`.

Når man kører et build-script — fx `npm run build-hello-world` — kompilerer det JSX fra `ch03/PROJECT_NAME/jsx` til almindelig JavaScript og lægger den kompilerede fil i `ch03/PROJECT_NAME/js`. Alt man skal gøre er at installere de nødvendige afhængigheder med `npm i` (det opretter en `ch03/node_modules`-mappe), tjekke om et build-script findes i `package.json`, og så køre `npm run build-PROJECT_NAME`.

---

## 3.4 React- og JSX-gotchas

Dette afsnit dækker nogle edge cases. Der er nogle få gotchas at være opmærksom på, når man bruger JSX.

For eksempel kræver JSX, at man har en **lukkende slash** (`/`) enten i det lukkende tag eller — hvis man ikke har nogen children og bruger et enkelt tag — i slutningen af det enkelte tag. Dette er korrekt:

```jsx
<a href="http://azat.co">Azat, the master of callbacks</a>
<button label="Save" className="btn" onClick={this.handleSave}/>
```

Dette er **ikke** korrekt, fordi slashene mangler:

```jsx
<a href="http://azat.co">Azat<a>
<button label="Save" className="btn" onClick={this.handleSave}>
```

Omvendt er HTML mere fejltolerant. De fleste browsere ignorerer den manglende slash og renderer elementet fint uden den.

Der er også andre forskelle mellem HTML og JSX.

### 3.4.1 Special characters

**HTML entities** er koder, der viser specialtegn såsom copyright-symboler, em-tankestreger, anførselstegn osv. Her er nogle eksempler:

```html
&copy;
&mdash;
&ldquo;
```

Man kan rendere disse koder som en hvilken som helst streng i `<span>` eller i string-attributten `<input>`. Dette er statisk JSX (tekst defineret i kode uden variabler eller properties):

```jsx
<span>&copy;&mdash;&ldquo;</span>
<input value="&copy;&mdash;&ldquo;"/>
```

Men hvis man vil udskrive HTML entities **dynamisk** (fra en variabel eller property) med `<span>`, får man kun det direkte output (`&copy;&mdash;&ldquo;`), ikke specialtegnene. Følgende kode virker altså ikke:

```jsx
// Anti-pattern. Will NOT work!
var specialChars = '&copy;&mdash;&ldquo;'
<span>{specialChars}</span>
<input value={specialChars}/>
```

React/JSX auto-escaper farlig HTML, hvilket er praktisk sikkerhedsmæssigt (security by default). For at udskrive specialtegn skal man bruge en af disse tilgange:

- Bryd dem op i flere strenge ved at udskrive et array; fx `<span>{[<span>&copy;&mdash;&ldquo;</span>]}</span>`. Man kan også sætte `key`, som `key="specialChars"`, for at undertrykke en advarsel om manglende key.
- Kopiér specialtegnet direkte ind i kildekoden (sørg for at bruge et UTF-8-tegnsæt).
- Escape specialtegnet med `\u` og brug et unicode-nummer.
- Konvertér fra en character code til et character number med `String.fromCharCode(charCodeNumber)`.
- Brug den interne metode `__html` til farligt at sætte inner HTML (ikke anbefalet).

For at illustrere den sidste tilgang (som sidste udvej):

```jsx
var specialChars = {__html: '&copy;&mdash;&ldquo;'}
<span dangerouslySetInnerHTML={specialChars}/>
```

React-teamet har åbenbart humoristisk sans, siden de kalder en property `dangerouslySetInnerHTML`.

### 3.4.2 data-attributes

Afsnit 2.3 dækkede properties på en ikke-JSX-måde, men lad os se på, hvordan man opretter custom attributes i HTML én gang til (denne gang med JSX). Hovedsageligt vil React saligt ignorere enhver ikke-standard HTML-attribut, man tilføjer til components. Det er ligegyldigt, om man bruger JSX eller native JavaScript — det er Reacts adfærd.

Men nogle gange vil man sende yderligere data med DOM-noder. Det er et antipattern, fordi ens DOM ikke bør bruges som database eller local storage. Vil man alligevel oprette custom attributes og få dem renderet, bruger man præfikset `data-`.

Dette er en gyldig custom `data-object-id`-attribut, som React vil rendere i view'et (HTML bliver den samme som denne JSX):

```jsx
<li data-object-id="097F4E4F">...</li>
```

Hvis inputtet er følgende React/JSX-element, vil React ikke rendere `object-id`, fordi det ikke er en standard-HTML-attribut (HTML kommer til at mangle `object-id`, modsat denne JSX):

```jsx
<li object-id="097F4E4F">...</li>
```

### 3.4.3 style-attributten

`style`-attributten i JSX fungerer anderledes end i almindelig HTML. Med JSX skal man sende et **JavaScript-objekt** i stedet for en streng, og CSS-properties skal skrives i camelCase. For eksempel:

- `background-image` bliver til `backgroundImage`.
- `font-size` bliver til `fontSize`.
- `font-family` bliver til `fontFamily`.

Man kan gemme JavaScript-objektet i en variabel eller rendere det inline med dobbelte curly braces (`{{...}}`). De dobbelte braces er nødvendige, fordi det ene sæt er til JSX og det andet til JavaScript-objekt-literalen.

Antag at man har et objekt med denne skriftstørrelse:

```javascript
let smallFontSize = {fontSize: '10pt'}
```

I sin JSX kan man bruge `smallFontSize`-objektet:

```jsx
<input style={smallFontSize} />
```

Eller man kan nøjes med en større skrift (30 point) ved at sende værdierne direkte uden en ekstra variabel:

```jsx
<input style={{fontSize: '30pt'}} />
```

Et andet eksempel på at sende styles direkte — denne gang sættes en rød border på `<span>`:

```jsx
<span style={{borderColor: 'red',
borderWidth: 1,
borderStyle: 'solid'}}>Hey</span>
```

Alternativt vil følgende border-værdi også virke:

```jsx
<span style={{border: '1px red solid'}}>Hey</span>
```

Hovedgrunden til, at klasser ikke er uigennemsigtige strenge men JavaScript-objekter, er, at React kan arbejde hurtigere med dem, når det anvender ændringer på views.

### 3.4.4 class og for

React og JSX accepterer enhver attribut, der er en standard-HTML-attribut, **undtagen `class` og `for`**. Disse navne er reserverede ord i JavaScript/ECMAScript, og JSX konverteres til almindelig JavaScript. Brug `className` og `htmlFor` i stedet. Har man fx en klasse `hidden`, kan man definere den i en `<div>` sådan her:

```jsx
<div className="hidden">...</div>
```

Skal man oprette et label til et form-element, bruger man `htmlFor`:

```jsx
<div>
<input type="radio" name={this.props.name} id={this.props.id}>
</input>
<label htmlFor={this.props.id}>
{this.props.label}
</label>
</div>
```

### 3.4.5 Boolean attribute-værdier

Sidst men ikke mindst er nogle attributter (såsom `disabled`, `required`, `checked`, `autofocus` og `readOnly`) kun specifikke for form-elements. Det vigtigste at huske er, at attributværdien skal sættes i JavaScript-expressionen (dvs. inde i `{}`) og ikke sættes i strenge.

Brug fx `{false}` for at aktivere inputtet:

```jsx
<input disabled={false} />
```

Men brug ikke en `"false"`-værdi, fordi den vil bestå truthy-tjekket (en ikke-tom streng er truthy i JavaScript) og rendere inputtet som disabled (`disabled` bliver `true`):

```jsx
<input disabled="false" />
```

#### Sidebar: Truthiness

I JavaScript/Node oversættes en **truthy**-værdi til `true`, når den evalueres som en Boolean; fx i et `if`-statement. Værdien er truthy, hvis den ikke er falsy. Og der findes kun seks falsy-værdier:

- `false`
- `0`
- `""` (tom streng)
- `null`
- `undefined`
- `NaN` (not a number)

Strengen `"false"` er en ikke-tom streng, som er truthy og oversættes til `true`. Derfor får man `disabled=true` i HTML.

Hvis man udelader værdien, antager React, at værdien er `true`:

```jsx
<input disabled />
```

De efterfølgende kapitler bruger JSX udelukkende. Men at kende den underliggende almindelige JavaScript, som browsere kører, er en god færdighed at have i værktøjskassen.

---

## 3.5 Quiz

1. For at udskrive en JavaScript-variabel i JSX, hvilken af følgende bruger man? `=`, `<%= %>`, `{}`, eller `<?= ?>`
2. `class`-attributten er ikke tilladt i JSX. Sandt eller falsk?
3. Standardværdien for en attribut uden værdi er `false`. Sandt eller falsk?
4. Inline-`style`-attributten i JSX er et JavaScript-objekt og ikke en streng som andre attributter. Sandt eller falsk?
5. Hvis man har brug for if/else-logik i JSX, kan man bruge det inde i `{}`. For eksempel er `class={if (!this.props.admin) return 'hide'}` gyldig JSX-kode. Sandt eller falsk?

---

## 3.6 Opsummering

- JSX er blot syntactic sugar for React-metoder som `createElement`.
- Man bør bruge `className` og `htmlFor` i stedet for standard-HTML-attributterne `class` og `for`.
- `style`-attributten tager et JavaScript-objekt, ikke en streng som i normal HTML.
- Ternary operators og IIFE er de bedste måder at implementere if/else-statements på.
- Udskrivning af variabler, kommentarer og HTML entities samt kompilering af JSX-kode til native JavaScript er nemt.
- Der findes flere valgmuligheder for at omdanne JSX til almindelig JavaScript; kompilering med Babel CLI kræver minimal opsætning sammenlignet med at konfigurere build-processering med et værktøj som Gulp eller Webpack eller at skrive Node/JavaScript-scripts til Babel-API'et.

---

## Quiz-svar

1. Man bruger `{}` til variabler og expressions.
2. **Sandt.** `class` er et reserveret eller specielt JavaScript-statement. Derfor bruger man `className` i JSX.
3. **Falsk.** Det anbefales at bruge `attribute_name={false/true}` til at sætte boolean-værdierne eksplicit.
4. **Sandt.** `style` er et objekt af performance-årsager.
5. **Falsk.** For det første er `class` ikke en korrekt attribut. Og i stedet for `if return` (ikke gyldigt) bør man bruge en ternary operator.
