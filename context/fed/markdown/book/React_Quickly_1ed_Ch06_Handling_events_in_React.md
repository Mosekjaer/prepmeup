# Kapitel 6 — Handling events in React

## Metadata

> **⚠️ Udgave-advarsel:** Denne bog er *React Quickly* **1. udgave (©2017, Azat Mardan)**. Kursusbeskrivelsen foreskriver **2. udgave** (Barklund & Mardan). 1. udgave er skrevet før React Hooks og bruger class components, lifecycle-metoder og Webpack. Kursets React-lektioner (L16–L27) er hook-baserede og bruger Vite, Vitest og Redux Toolkit. **Ved konflikt mellem denne bog og slidesene er slidesene autoritative.**

- **Kapitel:** 6 — Handling events in React
- **Bog:** React Quickly, 1. udgave — Azat Mardan, Manning (©2017)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L21
- **Hovedemner:**
  - DOM events i React: deklarativ binding via JSX-attributter i camelCase (`onClick`, `onMouseOver`)
  - Event handlers som class-metoder og `this`-binding med `bind()` i constructor eller i `render()`
  - Understøttede event-typer i React v15 (mouse, keyboard, clipboard, form, focus, touch, UI, wheel, selection, image, animation, transition)
  - Capture phase vs. target phase vs. bubbling phase — og `...Capture`-suffikset
  - Event delegation under motorhjelmen: React binder ét listener på `document`, ikke på hver DOM-node
  - `SyntheticEvent`: cross-browser wrapper, dens API, event pooling og `persist()`
  - Events kombineret med `state` — interaktive komponenter der opdaterer sig selv
  - Event handlers sendt videre som props: presentational/dumb vs. container/smart components
  - Dataudveksling mellem søskendekomponenter via en fælles parent
  - DOM events som React *ikke* understøtter (fx `resize`) — håndteret via lifecycle-metoder
  - Integration med andre biblioteker (jQuery UI Slider) — tight vs. loose coupling

---

## Introduktion

Indtil nu har bogen renderet UI'er uden nogen brugerinteraktion — altså ren visning af data. Eksemplet var et ur, der ikke tager imod brugerinput, fx valg af tidszone.

I praksis er UI'er sjældent statiske. Elementerne skal være smarte nok til at reagere på brugerhandlinger: klik, træk med musen, tastetryk. Dette kapitel viser, hvordan man håndterer **events** i React. Kapitel 7 bygger videre på det og anvender viden om events på webformularer og deres elementer. React understøtter kun et bestemt sæt events; kapitlet viser også, hvordan man arbejder med de events, React *ikke* understøtter.

> **NOTE** Kildekoden til eksemplerne i dette kapitel ligger på
> `https://www.manning.com/books/react-quickly` og
> `https://github.com/azat-co/react-quickly/tree/master/ch06`
> (i `ch06`-mappen i GitHub-repoet `https://github.com/azat-co/react-quickly`).
> Demoer findes på `http://reactquickly.co/demos`.

---

## 6.1 Working with DOM events in React

React-elementer gøres responsive over for brugerhandlinger ved at definere **event handlers** for de handlinger. Man definerer event handleren (funktionsdefinitionen) som værdien af en element-attribut i JSX — eller som en element-property i almindelig JavaScript, når `createElement()` kaldes direkte uden JSX.

Attributnavne for events er standard W3C DOM event-navne i **camelCase**, fx `onClick` eller `onMouseOver`:

```jsx
onClick={function() {...}}
```

eller

```jsx
onClick={() => {...}}
```

Man kan fx definere en event listener, der udløses ved klik på en knap; i listeneren logges `this`-konteksten. **Event objectet** er en forbedret udgave af det native DOM event object — det kaldes `SyntheticEvent`:

```jsx
<button onClick={(function(event) {
  console.log(this, event)
}).bind(this)}>
Save
</button>
```

`bind()` er nødvendig, for at man i event handler-funktionen får en reference til instansen af klassen (React-elementet); uden binding er `this` lig `null` (i strict mode). Man behøver **ikke** binde konteksten til klassen med `bind(this)` i disse tilfælde:

- Når man ikke har brug for at referere til klassen via `this`
- Når man bruger den ældre stil, `React.createClass()`, i stedet for den nyere ES6+ class-stil — `createClass()` autobinder nemlig for én
- Når man bruger fat arrows (`(){}`)

> *Kursusnote: Kurset bruger funktionelle komponenter og hooks. Der findes ingen `this` i en funktionel komponent, så hele problemstillingen med `bind(this)`, autobinding og `createClass()` bortfalder. Selve event-syntaksen (`onClick={...}`, camelCase, funktionsdefinition som værdi) er derimod uændret. Se slides L21.1.*

Man kan gøre det pænere ved at bruge en **class-metode** som event handler (her kaldet `handleSave()`) til `onClick`-eventet. Betragt en `SaveButton`-komponent, som ved klik udskriver værdien af `this` og `event`, men bruger en class-metode:

**Listing 6.1 — Declaring an event handler as a class method** (`ch06/button/jsx/button.jsx`)

```jsx
class SaveButton extends React.Component {
  handleSave(event) {
    console.log(this, event)
  }
  render() {
    return <button onClick={this.handleSave.bind(this)}>
      Save
    </button>
  }
}
```

Bogens annotation: `bind()` returnerer en funktionsdefinition, og det er den, der sendes videre til `onClick`. *Figur 6.1: Et klik på knappen udskriver værdien af `this`: `SaveButton`.*

> *Kursusnote: I kursets funktionelle komponenter skrives det samme som en almindelig funktion i komponentkroppen — `function handleSave(event) {...}` — og videregives direkte som `onClick={handleSave}` uden nogen binding. Se slides L21.1.*

Man kan desuden binde en event handler til klassen i klassens **constructor**. Funktionelt er der ingen forskel; men bruger man samme metode mere end én gang i `render()`, kan man reducere duplikering med constructor-binding. Samme knap, nu med constructor-binding:

```jsx
class SaveButton extends React.Component {
  constructor(props) {
    super(props)
    this.handleSave = this.handleSave.bind(this)
  }
  handleSave(event) {
    console.log(this, event)
  }
  render() {
    return <button onClick={this.handleSave}>
      Save
    </button>
  }
}
```

Annotationerne: `this.handleSave = this.handleSave.bind(this)` binder `this`-konteksten til klassen, så `this` i event handleren refererer til klassen, og `onClick={this.handleSave}` videregiver funktionsdefinitionen. Forfatterens anbefaling er constructor-binding, fordi den eliminerer duplikering og samler al binding ét sted.

> *Kursusnote: Constructor-binding er det klassiske 2017-mønster. I kursets funktionelle komponenter findes hverken constructor eller `this`, så mønstret er irrelevant der. Bevar eksemplet som historisk kontekst — se slides L21.1.*

### Tabel 6.1 — DOM events understøttet af React v15

Bemærk brugen af camelCase i event-navnene, for konsistens med øvrige attributnavne i React.

| Event group | Events supported by React |
| --- | --- |
| Mouse events | `onClick`, `onContextMenu`, `onDoubleClick`, `onDrag`, `onDragEnd`, `onDragEnter`, `onDragExit`, `onDragLeave`, `onDragOver`, `onDragStart`, `onDrop`, `onMouseDown`, `onMouseEnter`, `onMouseLeave`, `onMouseMove`, `onMouseOut`, `onMouseOver`, `onMouseUp` |
| Keyboard events | `onKeyDown`, `onKeyPress`, `onKeyUp` |
| Clipboard events | `onCopy`, `onCut`, `onPaste` |
| Form events | `onChange`, `onInput`, `onSubmit` |
| Focus events | `onFocus`, `onBlur` |
| Touch events | `onTouchCancel`, `onTouchEnd`, `onTouchMove`, `onTouchStart` |
| UI events | `onScroll` |
| Wheel events | `onWheel` |
| Selection events | `onSelect` |
| Image events | `onLoad`, `onError` |
| Animation events | `onAnimationStart`, `onAnimationEnd`, `onAnimationIteration` |
| Transition events | `onTransitionEnd` |

React understøtter altså flere typer normaliserede events. Sammenlignet med listen over standard-events på `https://developer.mozilla.org/en-US/docs/Web/Events` er dækningen omfattende, og flere kommer til. Dokumentation: `http://facebook.github.io/react/docs/events.html`.

---

## 6.1.1 Capture and bubbling phases

React er **declarative**, ikke imperative. Det fjerner behovet for at manipulere objekter direkte, og man tilknytter ikke events til sin kode, som man ville gøre med jQuery (fx `$('.btn').click(handleSave)`). I stedet deklarerer man eventet i JSX som en attribut (fx `onClick={handleSave}`). Ved mouse events kan attributnavnet være et hvilket som helst af de understøttede events fra tabel 6.1. Attributtens værdi er event handleren.

Vil man fx definere et mouse-hover event, bruger man `onMouseOver`; hover viser "mouse is over" i konsollen, når markøren føres over `<div>`'ens røde ramme:

```jsx
<div
  style={{border: '1px solid red'}}
  onMouseOver={()=>{console.log('mouse is over')}} >
  Open DevTools and move your mouse cursor over here
</div>
```

Events som `onMouseOver` udløses i **bubbling phase** (bobler op). Der findes også en **capture phase** (trickle down), som går forud for bubbling- og target-faserne. Rækkefølgen er:

1. **Capture phase** — fra `window` og ned til target-elementet
2. **Target phase**
3. **Bubbling phase** — eventet rejser op ad træet tilbage til `window`

Distinktionen mellem faser bliver vigtig, når man har det samme event på både et element og dets ancestor(s). I bubbling mode fanges og håndteres eventet først af det inderste element (target) og propageres derefter til de ydre elementer (ancestors, startende med targets parent). I capture mode fanges eventet først af det yderste element og propageres derefter indad.

For at registrere en event listener til **capture phase** tilføjer man `Capture` til event-navnet: i stedet for `onMouseOver` bruger man `onMouseOverCapture`. Det gælder alle event-navnene i tabel 6.1. *Figur 6.2 illustrerer capture-, target- og bubbling-faserne ned og op gennem `Window` → `Document` → `<html>` → `<body>` → `<table>` → `<tbody>` → `<tr>` → `<td>`.* Antag et `<div>` med både et regulært (bubbling) event og et capture event, defineret med henholdsvis `onMouseOver` og `onMouseOverCapture`:

**Listing 6.2 — Capture event followed by bubbling event** (`ch06/mouse-capture/jsx/mouse.jsx`)

```jsx
class Mouse extends React.Component {
  render() {
    return <div>
      <div
        style={{border: '1px solid red'}}
        onMouseOverCapture={((event)=>{
          console.log('mouse over on capture event')
          console.dir(event, this)}).bind(this)}
        onMouseOver={((event)=>{
          console.log('mouse over on bubbling event')
          console.dir(event, this)}).bind(this)} >
        Open DevTools and move your mouse cursor over here
      </div>
    </div>
  }
}
```

> *Kursusnote: Eksemplet bruger en class component og `.bind(this)` på arrow-funktionerne (i praksis overflødigt, da fat arrows allerede binder leksikalsk). Kurset bruger funktionelle komponenter, hvor `this`-problematikken ikke findes; `onMouseOverCapture`/`onMouseOver`-syntaksen er identisk. Se slides L21.1.*

Containeren har en 1 pixel bred rød ramme og lidt tekst, så man ved, hvor markøren skal føres hen. Hvert `mouseover`-event logger event-typen samt selve event objectet (skjult under `Proxy` i DevTools på grund af `console.dir()`). Ikke overraskende logges **capture-eventet først** (*figur 6.3*) — den adfærd kan bruges til at stoppe propagation og sætte prioriteter mellem events. Det er vigtigt at forstå, hvordan React implementerer events, fordi events er hjørnestenen i UI'er; kapitel 7 går dybere.

---

## 6.1.2 React events under the hood

Events fungerer anderledes i React end i jQuery eller ren JavaScript, hvor man typisk placerer event listeneren direkte på DOM-noden; det giver problemer med at fjerne og tilføje events i løbet af UI'ets livscyklus. Eksempel: en liste af konti, hvor konti kan fjernes, redigeres og tilføjes, og hvert `<li>` er unikt identificeret ved ID:

```html
<ul id="account-list">
  <li id="account-1">Account #1</li>
  <li id="account-2">Account #2</li>
  <li id="account-3">Account #3</li>
  <li id="account-4">Account #4</li>
  <li id="account-5">Account #5</li>
  <li id="account-6">Account #6</li>
</ul>
```

Ved hyppige ændringer bliver eventhåndteringen besværlig. En bedre tilgang er **én** event listener på en parent (`account-list`), som lytter efter bubbled-up events (et event bobler højere op i DOM-træet, hvis intet fanger det lavere nede). Internt holder React styr på events tilknyttet højere elementer og target-elementer i en **mapping**, så React kan spore target fra parent (`document`). *Figur 6.4: Et DOM-event (1) bobler op til sine ancestors (2–3), hvor det fanges af en regulær (bubbling-stage) React event listener (4), fordi events i React fanges ved roden (`Document`).*

Lad os se **event delegation** i praksis med `Mouse`-komponenten fra listing 6.2, som har et `<div>` med `mouseover`. Åbner man DevTools, vælger `data-reactroot`-elementet i Elements-/Inspector-fanen og skriver `$0` i konsollen, får man en reference til `<div>`'en. Interessant nok har denne DOM-node **ingen** event listeners. Man kan tjekke det med den globale `getEventListeners()`-metode:

```javascript
getEventListeners($0)
```

Resultatet er et tomt objekt `{}` — React har **ikke** tilknyttet event listeners til `reactroot`-noden `<div>` (*figur 6.5*). Alligevel logges udsagnene ved hover; eventet fanges tydeligvis. Gentager man proceduren med `<div id="content">` eller det rødrammede `<div>` (barn af `reactroot`), er resultatet det samme. Undersøg i stedet `document`:

```javascript
getEventListeners(document)
```

Bingo: `Object {mouseover: Array[1]}` (*figur 6.6*). React har tilknyttet event listeneren til den ultimative parent, `document`-elementet — **ikke** til en individuel node som `<div>` eller et element med `data-reactroot`. Man kan fjerne eventet igen fra konsollen:

```javascript
getEventListeners(document).mouseover[0].remove()
```

Nu vises "mouse is over" ikke længere. Event listeneren på `document` er væk, hvilket illustrerer, at React tilknytter events til `document`, ikke til hvert enkelt element. Det gør React hurtigere, især ved lister — modsat jQuery, hvor events tilknyttes individuelle elementer. Har man flere elementer med samme event-type, fx to `mouseover`, er de tilknyttet ét event og håndteres af Reacts interne mapping til det korrekte barn (target-element), jf. *figur 6.7*. Information om target-noden (hvor eventet opstod) fås fra event objectet.

> *Kursusnote: Detaljen om at React binder til `document` er specifik for React v15/16/17. Fra React 18 tilknyttes event listeners til rod-containeren fra `createRoot()` i stedet for `document`. Selve princippet — ét delegeret listener i roden i stedet for ét pr. node — er uændret. Se slides L21.*

---

## 6.1.3 Working with the React SyntheticEvent event object

Browsere kan afvige i deres implementering af W3C-specifikationen (`www.w3.org/TR/DOM-Level-3-Events`), så det **event object**, der sendes til event handleren, kan have forskellige properties og metoder. Det giver cross-browser-problemer: for at få target-elementet i IE8 skulle man tilgå `event.srcElement`, mens Chrome, Safari og Firefox bruger `event.target`:

```javascript
var target = event.target || event.srcElement
console.log(target.value)
```

Situationen er bedre i 2016 end i 2006, men cross-browser-problemer er stadig dårlige: brugerne bør have samme oplevelse på tværs af browsere, og typisk må man tilføje `if/else`-kode for at kompensere for forskelle i browser-API'er samt teste mere. Reacts løsning er en **wrapper** omkring browserens native events, som gør events konsistente med W3C-specifikationen uanset browser. Under motorhjelmen bruger React sin egen klasse til syntetiske events: `SyntheticEvent`, hvis instanser sendes til event handleren. For at få adgang til objektet tilføjer man et argument `event` til handler-funktionen:

**Listing 6.3 — Event handler receiving a synthetic event** (`ch06/mouse/jsx/mouse.jsx`)

```jsx
class Mouse extends React.Component {
  render() {
    return <div>
      <div
        style={{border: '1px solid red'}}
        onMouseOver={((event)=>{
          console.log('mouse is over with event')
          console.dir(event)})} >
        Open DevTools and move your mouse cursor over here
      </div>
    </div>
  }
}
```

Annotationerne: `event`-argumentet defineres, og `console.dir(event)` logger `SyntheticEvent`-objektet interaktivt (*figur 6.8*).

Som tidligere kan handler-koden flyttes til en komponentmetode eller en selvstændig funktion, fx `handleMouseOver()` refereret fra `render()` med `{this.handleMouseOver.bind(this)}`. `bind()` er nødvendig for at overføre den rigtige værdi af `this`; med fat-arrow- og `createClass()`-syntaks sker det automatisk, men ikke med `class`. Bruger man ikke `this` i metoden, kan man nøjes med `onMouseOver={this.handleMouseOver}`. Navnet `handleMouseOver()` er arbitrært (modsat navnene på lifecycle events, kapitel 5), men konventionen i React er at præfikse med `handle` for at skelne fra en almindelig class-metode og inkludere enten event-navnet (`mouseOver`) eller operationen (`save`).

**Listing 6.4 — Event handler as a class method; binding in `render()`**

```jsx
class Mouse extends React.Component {
  handleMouseOver(event) {
    console.log('mouse is over with event')
    console.dir(event.target)
  }
  render(){
    return <div>
      <div
        style={{border: '1px solid red'}}
        onMouseOver={this.handleMouseOver.bind(this)} >
        Open DevTools and move your mouse cursor over here
      </div>
    </div>
  }
}
```

> *Kursusnote: `this.handleMouseOver.bind(this)` inde i `render()` er class component-mønstret. I kursets funktionelle komponenter defineres `handleMouseOver` som en almindelig funktion i komponentkroppen og videregives direkte som `onMouseOver={handleMouseOver}` — ingen binding, ingen `this`. Se slides L21.1.*

Eventet har samme properties og metoder som de fleste native browser-events: `stopPropagation()`, `preventDefault()`, `target`, `currentTarget`. Kan man ikke finde en native property eller metode, tilgås det native browser-event med `nativeEvent`:

```javascript
event.nativeEvent
```

### SyntheticEvent-interfacet (React v15.x)

Et udvalg af attributter og metoder på Reacts `SyntheticEvent`-interface:

- **`currentTarget`** — `DOMEventTarget` for det element, der fanger eventet (kan være et target eller target'ets parent)
- **`target`** — `DOMEventTarget`, det element, hvor eventet blev udløst
- **`nativeEvent`** — `DOMEvent`, det native browser event object
- **`preventDefault()`** — Forhindrer default-adfærden, fx et link eller en form-submit-knap
- **`isDefaultPrevented()`** — En boolean, der er `true`, hvis default-adfærden blev forhindret
- **`stopPropagation()`** — Stopper propagation af eventet
- **`isPropagationStopped()`** — En boolean, der er `true`, hvis propagation blev stoppet
- **`type`** — En streng med tag-navnet
- **`persist()`** — Fjerner det syntetiske event fra pool'en og tillader, at referencer til eventet bevares af brugerkode
- **`isPersistent`** — En boolean, der er `true`, hvis `SyntheticEvent` blev taget ud af pool'en

`target` indeholder DOM-noden for det objekt, hvor eventet **skete** — ikke hvor det blev fanget, som `currentTarget` gør (`https://developer.mozilla.org/en-US/docs/Web/API/Event/target`). Ofte har man brug for teksten i et input-felt; den fås fra `event.target.value`.

### Event pooling og nullificering

Det syntetiske event **nullificeres** (bliver utilgængeligt), når event handleren er færdig. Derfor kan man ikke uden videre gemme referencen i en variabel for at tilgå den senere eller asynkront i en callback. Eksempel, hvor referencen gemmes i en global `e`:

**Listing 6.5 — Nullifying a synthetic event** (`ch06/mouse-event/jsx/mouse.jsx`)

```jsx
class Mouse extends React.Component {
  handleMouseOver(event) {
    console.log('mouse is over with event')
    window.e = event // Anti-pattern
    console.dir(event.target)
    setTimeout(()=>{
      console.table(event.target)
      console.table(window.e.target)
    }, 2345)
  }
  render() {
    return <div>
      <div
        style={{border: '1px solid red'}}
        onMouseOver={this.handleMouseOver.bind(this)}>
        Open DevTools and move your mouse cursor over here
      </div>
    </div>
  }
}
```

Annotationerne: event objectet og dets attributter bruges i metoden, men som standard kan man **ikke** bruge et event i en asynkron callback eller via `window.e`. Man får en advarsel om, at React genbruger det syntetiske event af performance-hensyn (*figur 6.9*):

```
This synthetic event is reused for performance reasons. If you're seeing this,
you're accessing the property `target` on a released/nullified synthetic
event. This is set to null.
```

Har man brug for at beholde eventet, efter handleren er færdig, bruger man `event.persist()`; så genbruges og nullificeres objektet ikke. React **syntetiserer** (normaliserer) altså browser-eventet med en cross-browser wrapper omkring de native event objects, så events opfører sig identisk i stort set alle browsere. I de fleste tilfælde har man alle de native metoder på React-eventet, inklusive `event.stopPropagation()` og `event.preventDefault()`. Har man alligevel brug for det native event, ligger det i `event.nativeEvent` — og så skal man selv håndtere de cross-browser-forskelle, man støder på.

> *Kursusnote: Event pooling — det at `SyntheticEvent` nullificeres efter handleren — blev fjernet i React 17. `event.persist()` findes stadig, men gør ingenting i moderne React, og advarslen ovenfor optræder ikke længere. Resten af `SyntheticEvent`-API'et (`target`, `currentTarget`, `preventDefault()`, `stopPropagation()`, `nativeEvent`) er uændret. Se slides L21.*

---

## 6.1.4 Using events and state

At bruge `state` sammen med events — at ændre en komponents state som reaktion på et event — giver interaktive UI'er, der reagerer på brugerhandlinger. Man kan fange vilkårlige events og ændre views baseret på dem og på app-logikken, hvilket gør komponenterne **selvindeholdte**: de behøver ingen ekstern kode eller repræsentation. Eksempel (*figur 6.10*): en knap med et label, der har en tæller startende ved 0, hvor hvert klik inkrementerer tallet. Man implementerer:

- **`constructor()`** — `this.state` sættes, fordi tælleren skal sættes til 0, før den kan bruges i view'et.
- **`handleClick()`** — Event handler, der inkrementerer tælleren.
- **`render()`** — Render-metode, der returnerer knappens JSX.

`click()`-metoden adskiller sig ikke fra andre React-komponentmetoder (jf. `getUrl()` i kapitel 3 og `handleMouseOver()` ovenfor), bortset fra at man manuelt skal binde `this`-konteksten. `handleClick()` sætter `counter`-state til den nuværende værdi inkrementeret med 1:

**Listing 6.6 — Updating state as a result of a click action** (`ch06/onclick/jsx/content.jsx`)

```jsx
class Content extends React.Component {
  constructor(props) {
    super(props)
    this.state = {counter: 0}
  }
  handleClick(event) {
    this.setState({counter: ++this.state.counter})
  }
  render() {
    return (
      <div>
        <button
          onClick={this.handleClick.bind(this)}
          className="btn btn-primary">
          Don't click me {this.state.counter} times!
        </button>
      </div>
    )
  }
}
```

Annotationerne: initial state `counter` sættes til 0; `handleClick` øger `counter` med 1; `onClick` tilknytter event listeneren til `handleClick`-triggeren; og `{this.state.counter}` viser værdien af state-tælleren.

> *Kursusnote: Kurset bruger `useState` i funktionelle komponenter til præcis denne slags state, og undgår `this.setState({counter: ++this.state.counter})`, som muterer state direkte (et anti-mønster selv i class components). Selve koblingen event → state-ændring → re-render er den samme. Se slides L21.1.*

### Invocation vs. definition

Selvom `this.handleClick()` er en metode i listing 6.6, **invokerer** man den ikke i JSX, når den tildeles `onClick` — der er ingen parenteser efter `this.handleClick` inde i tuborgklammerne. Man skal videregive en **funktionsdefinition**, ikke invokere den; funktioner er first-class citizens i JavaScript, og her sendes definitionen som værdi til `onClick`. Omvendt **invokeres** `bind()`, fordi den giver den rigtige værdi af `this` — men `bind()` returnerer selv en funktionsdefinition, så man ender stadig med en funktionsdefinition som `onClick`-værdi. Husk desuden, at `onClick` ikke er en rigtig HTML-attribut, men syntaktisk ligner enhver anden JSX-deklaration (`className={btnClassName}`, `href={this.props.url}`).

Analogt til `onClick` og `onMouseOver` kan man bruge alle de DOM-events, React understøtter. I bund og grund definerer man view'et og en event handler, der ændrer state; man modificerer **ikke** repræsentationen imperativt. Det er styrken ved den deklarative stil. Næste afsnit viser, hvordan man sender event handlers og andre objekter videre til børneelementer.

---

## 6.1.5 Passing event handlers as properties

Scenarie: man har en knap, som er en stateless komponent med kun styling. Hvordan tilknytter man en event listener, så knappen kan udløse kode? Properties er **immutable** og sendes fra parent-komponenter til deres børn — og fordi funktioner er first-class citizens i JavaScript, kan en property i et børneelement *være* en funktion og bruges som event handler. Løsningen er altså at sende event handleren som **property** til den stateless komponent og bruge propertyen dér. Vi opdeler funktionaliteten fra det forrige eksempel i to komponenter: `ClickCounterButton` (dum/stateless) og `Content` (smart/stateful).

### Presentational/Dumb vs. container/smart components

Dumb og smart components kaldes også henholdsvis **presentational** og **container** components. Dikotomien hænger sammen med statelessness og statefulness, men er ikke altid præcis det samme.

Presentational-komponenter har som regel ingen states og kan være stateless- eller function components — dog ikke altid, for man kan have behov for state, der vedrører selve præsentationen. De bruger ofte `this.props.children` og renderer DOM-elementer. Container/smart components beskriver derimod, hvordan tingene *virker*, uden DOM-elementer: de har states, bruger typisk higher-order component-mønstre og forbinder sig til datakilder. At kombinere de to er best practice — det holder tingene rene og giver bedre separation of concerns.

---

Når koden køres, øges tælleren ved hvert klik. Visuelt er intet ændret i forhold til figur 6.10, men internt er der nu en ekstra komponent, `ClickCounterButton` (stateless og stort set logikløs), ud over `Content`, som stadig har al logikken. `ClickCounterButton` har ikke sin egen `onClick`-handler (ingen `this.handler` eller `this.handleClick`) — den bruger den handler, forælderen sender ned i `this.props.handler`. Det er en fordel, fordi knappen dermed er en genbrugelig, stateless presentational component.

**Listing 6.7 — Stateless button component** (`ch06/onclick-props/jsx/click-counter-button.jsx`)

```jsx
class ClickCounterButton extends React.Component {
  render() {
    return <button
      onClick={this.props.handler}
      className="btn btn-danger">
      Increase Volume (Current volume is {this.props.counter})
    </button>
  }
}
```

Komponenten er, med forfatterens ord, dummere end *Dumb & Dumber* — men det er netop det gode ved arkitekturen: den er simpel og let at forstå (*figur 6.11*). `ClickCounterButton` bruger også `counter`-propertyen, renderet med `{this.props.counter}`. Properties leveres til børn med standard attribut-syntaks, `name=VALUE`, i JSX-deklarationen i forælderens render (her `Content`):

```jsx
<div>
  <ClickCounterButton
    counter={this.state.counter}
    handler={this.handleClick}/>
</div>
```

`counter` i `ClickCounterButton` er en property og dermed immutable; i `Content`-forælderen er den en state og dermed mutable (properties vs. state: se kapitel 4). Navnene kan afvige, men ens navne hjælper med at se, at data hænger sammen på tværs af komponenter. Den initiale `counter` sættes til 0 i `Content`, hvor event handleren også defineres — barnet udløser altså eventet på forælderen:

**Listing 6.8 — Passing an event handler as a property** (`ch06/onclick-props/jsx/content.jsx`)

```jsx
class Content extends React.Component {
  constructor(props) {
    super(props)
    this.handleClick = this.handleClick.bind(this)
    this.state = {counter: 0}
  }
  handleClick(event) {
    this.setState({counter: ++this.state.counter})
  }
  render() {
    return (
      <div>
        <ClickCounterButton
          counter={this.state.counter}
          handler={this.handleClick}/>
      </div>
    )
  }
}
```

Annotationen: konteksten bindes i constructor'en, så man kan bruge `this.setState()`, som refererer til instansen af denne `Content`-klasse.

> *Kursusnote: Constructor-bindingen i linjen `this.handleClick = this.handleClick.bind(this)` er kun nødvendig, fordi dette er en class component. I kursets funktionelle komponenter defineres `handleClick` i komponentkroppen og sendes direkte videre som `handler={handleClick}`. Selve mønstret — event handler defineret i forælderen, sendt ned som prop, brugt af barnet — er identisk i moderne React. Se slides L21.1.*

Funktioner er first-class citizens i JavaScript og kan sendes videre som variabler eller properties. Spørgsmålet er nu, hvor logik som event handlers skal ligge — i et barn eller en forælder?

---

## 6.1.6 Exchanging data between components

I det forrige eksempel lå click-event handleren i forældreelementet. Man *kan* placere den i barnet, men forælderen gør det muligt at udveksle information mellem børnekomponenter. Vi bruger igen knappen, men fjerner tællerværdien fra `render()` og lægger den i en selvstændig komponent `Counter`. Der bliver altså tre komponenter: `ClickCounterButton`, `Content` og `Counter` (*figur 6.12*). Knappen og teksten har hver properties, som er states i `Content`-forælderen, og de skal kommunikere for at tælle klik — men via `Content`, ikke direkte, da direkte kommunikation ville skabe tæt kobling. `ClickCounterButton` forbliver stateless, som de fleste React-komponenter bør være: bare properties og JSX.

**Listing 6.9 — Button component using an event handler from `Content`**

```jsx
class ClickCounterButton extends React.Component {
  render() {
    return <button
      onClick={this.props.handler}
      className="btn btn-info">
      Don't touch me with your dirty hands!
    </button>
  }
}
```

Man kan også skrive `ClickCounterButton` som en funktion i stedet for en klasse, hvilket forenkler syntaksen en smule:

```jsx
const ClickCounterButton = (props) => {
  return <button
    onClick={props.handler}
    className="btn btn-info">
    Don't touch me with your dirty hands!
  </button>
}
```

> *Kursusnote: Denne funktionsvariant er præcis den komponentform, kurset bruger overalt — bemærk fraværet af `this` og af binding. Se slides L21.1.*

Den nye komponent `Counter` viser `value`-propertyen, som er tælleren (navnene kan variere — man behøver ikke altid bruge `counter`):

```jsx
class Counter extends React.Component {
  render() {
    return <span>Clicked {this.props.value} times.</span>
  }
}
```

Endelig forældrekomponenten, som leverer de to properties — event handleren og tælleren. Render-delen opdateres tilsvarende, mens resten af koden er uændret:

**Listing 6.10 — Passing an event handler and state to two components** (`ch06/onclick-parent/jsx/content.jsx`)

```jsx
class Content extends React.Component {
  constructor(props) {
    super(props)
    this.handleClick = this.handleClick.bind(this)
    this.state = {counter: 0}
  }
  handleClick(event) {
    this.setState({counter: ++this.state.counter})
  }
  render() {
    return (
      <div>
        <ClickCounterButton handler={this.handleClick}/>
        <br/>
        <Counter value={this.state.counter}/>
      </div>
    )
  }
}
```

Tommelfingerreglen: læg eventhåndteringslogikken i **forælderen eller wrapper-komponenten**, hvis der er brug for interaktion mellem børnekomponenter. Vedrører eventet kun børnekomponenterne selv, er der ingen grund til at forurene komponenterne højere oppe i kæden.

> *Kursusnote: Dette er "lifting state up" — samme princip som i moderne React, blot udtrykt med `this.state`/`setState` i stedet for `useState`. Se slides L21.*

---

## 6.2 Responding to DOM events not supported by React

Tabel 6.1 listede de events, React understøtter. Hvad så med dem, React *ikke* understøtter? Antag et skalerbart UI, der skal ændre størrelse med vinduet (`resize`-eventet) — det event er ikke understøttet. Løsningen er en React-feature, man allerede kender: **lifecycle events**. Eksemplet er radioknapper. Standard HTML-radioknapper skalerer dårligt og inkonsistent på tværs af browsere, så forfatteren implementerede i sin tid hos DocuSign skalerbare CSS-radioknapper (`http://mng.bz/kPMu`) i jQuery, hvor knapperne skaleres ved at manipulere deres CSS. Nu laves samme UI i React (*figur 6.13*). `resize` understøttes ikke af React — dette virker **ikke**:

```jsx
...
render() {
  return <div>
    <div onResize={this.handleResize}
      className="radio-tagger"
      style={this.state.taggerStyle}>
...
```

Løsningen er komponentens **lifecycle events**: listing 6.11 tilføjer `resize`-listeners til `window` i `componentDidMount()` og fjerner dem igen i `componentWillUnmount()`, så intet efterlades, når komponenten er væk fra DOM'en. At efterlade event listeners hængende efter komponentens fjernelse er en glimrende måde at introducere **memory leaks**, som kan crashe appen — og koste søvnløse, Red Bull-drevne nætter med debugging.

**Listing 6.11 — Using lifecycle events to listen to DOM events** (`ch06/radio/jsx/radio.jsx`)

```jsx
class Radio extends React.Component {
  constructor(props) {
    super(props)
    this.handleResize = this.handleResize.bind(this)
    let order = props.order
    let i = 1
    this.state = {
      outerStyle: this.getStyle(4, i),
      innerStyle: this.getStyle(1, i),
      selectedStyle: this.getStyle(2, i),
      taggerStyle: {top: order*20, width: 25, height: 25}
    }
  }
  getStyle(i, m) {
    let value = i*m
    return {
      top: value,
      bottom: value,
      left: value,
      right: value,
    }
  }
  componentDidMount() {
    window.addEventListener('resize', this.handleResize)
  }
  componentWillUnmount() {
    window.removeEventListener('resize', this.handleResize)
  }
  handleResize(event) {
    let w = 1+ Math.round(window.innerWidth / 300)
    this.setState({
      taggerStyle: {top: this.props.order*w*10, width: w*10, height: w*10},
      textStyle: {left: w*13, fontSize: 7*w}
    })
  }
  ...
```

Annotationerne: styles gemmes i state; `getStyle()` skaber forskellige styles ud fra en bredde og en multiplikator; `componentDidMount()` tilknytter den ikke-understøttede event listener til `window`, og `componentWillUnmount()` fjerner den igen; `handleResize()` implementerer selve "magien" ud fra den nye skærmstørrelse.

> *Kursusnote: `componentDidMount()`/`componentWillUnmount()`-parret svarer i kursets funktionelle komponenter til et `useEffect` med tom dependency-array, hvor cleanup-funktionen fjerner listeneren. Mønstret — tilknyt ved mount, fjern ved unmount for at undgå memory leaks — er identisk. Se slides L21.*

Hjælpefunktionen `getStyle()` abstraherer noget af stylingen, fordi `top`, `bottom`, `left` og `right` gentages med forskellige værdier afhængigt af vinduets bredde; den tager værdien og multiplikatoren `m` og returnerer pixels (tal i Reacts CSS bliver til pixels). Resten er ligetil: `render()` bruger states og properties til at rendere fire `<div/>`-elementer, hver med en style defineret i `constructor()`.

**Listing 6.12 — Using state values for styles to resize elements**

```jsx
...
  render() {
    return <div>
      <div className="radio-tagger" style={this.state.taggerStyle}>
        <input type="radio" name={this.props.name} id={this.props.id}>
        </input>
        <label htmlFor={this.props.id}>
          <div className="radio-text" style={this.state.textStyle}>
            {this.props.label}</div>
          <div className="radio-outer" style={this.state.outerStyle}>
            <div className="radio-inner" style={this.state.innerStyle}>
              <div className="radio-selected"
                style={this.state.selectedStyle}>
              </div>
            </div>
          </div>
        </label>
      </div>
    </div>
  }
}
```

Pointen med eksemplet er, at man med lifecycle events kan lave **custom event listeners** — her på `window`. Det ligner den måde, Reacts egne event listeners fungerer på (React tilknytter events til `document`). Husk at fjerne de custom listeners i unmount-eventet. Om de skalerbare radioknapper og deres jQuery-implementering: blogindlæg på `http://mng.bz/kPMu`, demo på `http://jsfiddle.net/DSYz7/8`. Det bringer os til integration af React med andre UI-biblioteker.

---

## 6.3 Integrating React with other libraries: jQuery UI events

React leverer standard-DOM-events; men hvad hvis man skal integrere med et bibliotek, der bruger ikke-standardiserede events? Antag jQuery-komponenter, der bruger `slide` (slider-kontrolelementet), og at man vil integrere en React-widget i sin jQuery-app. Alle de DOM-events, React ikke leverer, kan tilknyttes via lifecycle events `componentDidMount` og `componentWillUnmount`: man tilknytter listeneren ved mount og afkobler ved unmount. Afkoblingen (oprydningen) er vigtig, så ingen listeners skaber konflikter eller performance-problemer ved at hænge rundt som forældreløse — **orphaned event handlers** er handlers uden de DOM-noder, som skabte dem, og dermed potentielle memory leaks.

Eksempel: man skal implementere lydstyrkekontroller i en ny version af en webafspiller (tænk Spotify eller iTunes) og tilføje et label og knapper oven i den gamle jQuery-slider (`http://plugins.jquery.com/ui.slider`). Målet er et label med en numerisk værdi og to knapper, der sænker og hæver værdien med 1. Delene skal spille sammen begge veje: træk i slider-pin'en opdaterer værdi og knaptekster, og klik på en knap flytter pin'en. Altså ikke bare en slider, men en samlet widget (*figur 6.14*).

### 6.3.1 Integrating buttons

Der er mindst to muligheder: (1) tilknytte events for jQuery Slider i en React-komponent, og (2) bruge `window`. Vi starter med den første, brugt til knapperne.

> **NOTE** Denne tilgang til integration af knapper er **tightly coupled**. Objekterne afhænger af hinanden. Generelt bør man undgå tæt koblede mønstre. Den anden, mere løst koblede mulighed implementeres til labels, efter denne tilgang er gennemgået.

Ved et `slide`-event på jQuery-slideren (en værdiændring) skal knapteksterne opdateres. Man tilknytter en listener til slideren i `componentDidMount` og udløser `handleSlide` på React-komponenten, som opdaterer state (`sliderValue`) ved hver ændring. `SliderButtons` implementerer det:

**Listing 6.13 — Integrating with a jQuery plug-in via its events** (`ch06/slider/jsx/slider-buttons.jsx`)

```jsx
class SliderButtons extends React.Component {
  constructor(props) {
    super(props)
    this.state = {sliderValue: 0}
  }
  handleSlide(event, ui) {
    this.setState({sliderValue: ui.value})
  }
  handleChange(value) {
    return ()=> {
      $('#slider').slider('value', this.state.sliderValue + value)
      this.setState({sliderValue: this.state.sliderValue + value})
    }
  }
  componentDidMount() {
    $('#slider').on('slide', this.handleSlide)
  }
  componentWillUnmount() {
    $('#slider').off('slide', this.handleSlide)
  }
  ...
```

Annotationerne: initialværdien sættes til 0; jQuery sender to argumenter — et jQuery-event og `ui`-objektet med den aktuelle værdi, som bruges til at opdatere state; `handleChange()` følger **Factory Function**-mønstret til `-1`- og `+1`-knapperne, bruger en jQuery-metode til at sætte den nye værdi og opdaterer state; `componentDidMount()` tilknytter listeneren, `componentWillUnmount()` fjerner den. `render()` har to knapper med `onClick`-events, en dynamisk `disabled`-attribut (så værdier under 0 eller over 100 undgås) samt Twitter Bootstrap-klasser:

**Listing 6.14 — Rendering slider buttons** (`ch06/slider/jsx/slider-buttons.jsx`)

```jsx
...
  render() {
    return <div>
      <button disabled={(this.state.sliderValue<1)?true:false}
        className="btn default-btn"
        onClick={this.handleChange(-1)}>
        1 Less ({this.state.sliderValue-1})
      </button>
      <button disabled={(this.state.sliderValue>99) ? true : false}
        className="btn default-btn"
        onClick={this.handleChange(1)}>
        1 More ({this.state.sliderValue+1})
      </button>
    </div>
  }
})
```

Annotationerne: ternær operator disabler knapperne, når værdien er under 1 eller over 99; `this.handleChange` invokeres med `-1` for at få en funktion ud af funktionsfabrikken; Bootstrap-klasser anvendes via `className`; og den næste værdi renderes som knap-label. Resultatet er, at knapperne disables uden for intervallet 0–100 — ved værdien 0 er Less-knappen disabled (*figur 6.15*). At trække i slideren ændrer knapteksterne og aktiverer/deaktiverer dem efter behov, og takket være kaldet til slideren i `handleChange()` flytter et klik på knapperne slideren. Dernæst Value-labelet, `SliderValue`.

### 6.3.2 Integrating labels

Ovenfor blev jQuery kaldt direkte fra React-metoder. Man kan i stedet **afkoble** jQuery og React ved at bruge et andet objekt til at fange events — et **loosely coupled** mønster, som ofte er at foretrække, fordi komponenterne ikke behøver kende detaljerne i hinandens implementering. `SliderValue` ved altså ikke, hvordan man kalder en jQuery-slider, hvilket gør det lettere senere at udskifte Slider med Slider 2.0 med et andet interface. Det implementeres ved at **dispatche events til `window`** i jQuery-events og definere `window`-listeners i React-komponentens lifecycle-metoder:

**Listing 6.15 — Integrating with a jQuery plug-in via `window`** (`ch06/slider/jsx/slider-value.jsx`)

```jsx
class SliderValue extends React.Component {
  constructor(props) {
    super(props)
    this.handleSlide = this.handleSlide.bind(this)
    this.state = {sliderValue: 0}
  }
  handleSlide(event) {
    this.setState({sliderValue: event.detail.ui.value})
  }
  componentDidMount() {
    window.addEventListener('slide', this.handleSlide)
  }
  componentWillUnmount() {
    window.removeEventListener('slide', this.handleSlide)
  }
  render() {
    return <div className="" >
      Value: {this.state.sliderValue}
    </div>
  }
}
```

Annotationerne: `slide`-listeneren tilknyttes `window` for at udløse `handleSlide()`, og fjernes igen for at undgå orphan event handlers og memory leaks. Derudover skal man **dispatche et custom event** — i den første tilgang (`SliderButtons`) var det unødvendigt, fordi man brugte pluginets eksisterende events. Dispatcherne kan implementeres sammen med koden, der opretter jQuery-slider-objektet, i et script-tag i `index.html`:

**Listing 6.16 — Setting up event listeners on a jQuery UI plug-in** (`ch06/slider/index.html`)

```javascript
let handleChange = (e, ui)=>{
  var slideEvent = new CustomEvent('slide', {
    detail: {ui: ui, jQueryEvent: e} )
  })
  window.dispatchEvent(slideEvent)
}
$( '#slider' ).slider({
  'change': handleChange,
  'slide': handleChange
})
```

Annotationerne: der oprettes en event handler for jQuery-slideren, som dispatcher custom events med jQuery-data om den aktuelle værdi; eventet dispatches til `window`; slideren oprettes ud fra containeren med ID `slider`; og der tilknyttes listeners på `change` (programmatisk) og `slide` (UI).

Når koden køres, virker både knapper og value-label problemfrit. Der blev brugt to tilgange: én loosely coupled og én tightly coupled. Sidstnævnte er kortere at implementere, men førstnævnte er at foretrække, fordi den gør koden lettere at ændre senere. Som integrationen viser, kan React arbejde fint sammen med andre biblioteker ved at lytte til events i `componentDidMount()`. React er meget **un-opinionated**, og den nemme integration er en stor fordel: udviklere kan skifte til React gradvist i stedet for at omskrive en hel applikation fra bunden.

---

## 6.4 Quiz

1. Vælg den korrekte syntaks for event-deklarationen: `onClick=this.doStuff`, `onclick={this.doStuff}`, `onClick="this.doStuff"`, `onClick={this.doStuff}` eller `onClick={this.doStuff()}`
2. `componentDidMount()` udløses ikke under server-side rendering af den React-komponent, hvor den er deklareret. Sandt eller falsk?
3. En måde at udveksle information mellem børnekomponenter på er at flytte objektet op til børnenes forælder. Sandt eller falsk?
4. Man kan som standard bruge `event.target` asynkront og uden for event handleren. Sandt eller falsk?
5. Man kan integrere med tredjepartsbiblioteker og events, som React ikke understøtter, ved at opsætte event listeners i komponentens lifecycle events. Sandt eller falsk?

---

## 6.5 Summary

- `onClick` bruges til at fange muse- og trackpad-klik.
- JSX-syntaksen for event listeners er `<a onNAME={this.METHOD}>`.
- Bind event handlers med `bind()` i `constructor()` eller i JSX, hvis du vil bruge `this` i event handleren som værdien af komponentklassens instans.
- `componentDidMount()` udløses kun i browseren. `componentWillMount()` udløses både i browseren og på serveren.
- React understøtter de fleste af de standardiserede HTML DOM-events ved at levere og bruge synthetic event objects.
- `componentDidMount()` og `componentWillUnmount()` kan bruges til at integrere React med andre frameworks og med events, React ikke understøtter.

> *Kursusnote: Punkterne om `bind()`, `componentDidMount()` og `componentWillMount()` er class component-specifikke. I kursets funktionelle komponenter håndteres mount/unmount med `useEffect` og cleanup, og binding er ikke et emne. Punkterne om `onClick`, JSX-syntaksen og `SyntheticEvent` gælder uændret. Se slides L21.1.*

---

## 6.6 Quiz answers

1. `onClick={this.doStuff}` er korrekt, fordi kun funktionsdefinitionen må sendes til `onClick` — ikke invokationen (mere præcist: resultatet af invokationen).
2. Sandt. `componentDidMount()` eksekveres kun for React i browseren, ikke for server-side React. Derfor bruger udviklere `componentDidMount()` til AJAX/XHR-requests. Se kapitel 5 for en genopfriskning af component lifecycle events.
3. Sandt. At flytte data op i komponenternes træhierarki gør, at man kan sende dem videre til forskellige børnekomponenter.
4. Falsk. Objektet genbruges, så man kan ikke bruge det i en asynkron operation, medmindre `persist()` kaldes på `SyntheticEvent`.
5. Sandt. Component lifecycle events er et af de bedste steder til dette, fordi de lader én udføre forberedelsen, før en komponent er aktiv, og før den fjernes.
