# Kapitel 13 — React routing

## Metadata

> **⚠️ Udgave-advarsel:** Denne bog er *React Quickly* **1. udgave (©2017, Azat Mardan)**. Kursusbeskrivelsen foreskriver **2. udgave** (Barklund & Mardan). 1. udgave er skrevet før React Hooks og bruger class components, lifecycle-metoder og Webpack. Kursets React-lektioner (L16–L27) er hook-baserede og bruger Vite, Vitest og Redux Toolkit. **Ved konflikt mellem denne bog og slidesene er slidesene autoritative.**

- **Kapitel:** 13 — React routing
- **Bog:** React Quickly, 1. udgave — Azat Mardan, Manning (©2017)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L17
- **Hovedemner:**
  - Hvorfor client-side routing er nødvendigt i single-page applications (SPA)
  - Semantiske URL'er, deling af links, browser-historik og SEO
  - Implementering af en naiv router fra bunden med `hashchange` og lifecycle-metoder
  - React Router: `<Router>`, `<Route>`, `<Link>` og JSX-baseret route-hierarki
  - Nested routes og layout-komponenter via `this.props.children`
  - Hash history vs. browser history (HTML5 `pushState`) og krav til serveren
  - URL parameters (`/posts/:id`) og adgang via `props.params`
  - Adgang til router via `contextTypes` eller HOC'en `withRouter`
  - Programmatisk navigation med `router.push(URL)`
  - Videregivelse af data til routes via ekstra props på `<Route>`
  - Alternativ: routing med Backbone's `Router.extend()`

---

## Introduktion

Tidligere ændrede URL'en sig sjældent i single-page applications. Der var ingen grund til at gå til serveren, takket være browser rendering — kun en del af siden ændrede indhold. Den tilgang havde nogle uheldige konsekvenser:

- At genindlæse browseren førte dig tilbage til sidens oprindelige form.
- Et klik på browserens Back-knap kunne føre dig til et helt andet website, fordi browserens history-funktion kun registrerede én enkelt URL for det site, du var på. Der var ingen URL-ændringer, der afspejlede din navigation mellem indhold.
- Du kunne ikke dele en præcis side på sitet med dine venner.
- Søgemaskiner kunne ikke indeksere sitet, fordi der ikke var distinkte URL'er at indeksere.

I dag har vi browser URL routing. **URL routing** lader dig konfigurere en applikation til at acceptere request-URL'er, der ikke mapper til fysiske filer. I stedet kan du definere URL'er, der er semantisk meningsfulde for brugerne, som kan hjælpe med search-engine optimization (SEO), og som kan afspejle applikationens state. Fx kunne en URL for en side, der viser produktinformation, være:

```
https://www.manning.com/books/react-quickly
```

Dette mappes bag kulisserne til én enkelt side, der viser produktet med ID `react-quickly`. Efterhånden som du browser rundt mellem produkter, kan URL'en ændre sig, og både browseren og søgemaskinerne kan interagere med produktsiderne, som man forventer.

Vil du undgå fulde page reloads, kan du bruge en hash (`#`) i dine URL'er, sådan som disse velkendte sites gør:

```
https://mail.google.com/mail/u/0/#inbox
https://en.todoist.com/app?v=816#agenda%2Foverdue%2C%20today
https://calendar.google.com/calendar/render?tab=mc#main_7
```

URL routing er et krav for en brugervenlig, veldesignet web-app. Uden specifikke URL'er kan brugere ikke gemme eller dele links uden at miste applikationens state — det gælder både en single-page application (SPA) og en traditionel web-app med server rendering.

I dette kapitel bygges et simpelt React-website, og der gennemgås et par forskellige muligheder for at implementere routing i det: først en router skrevet fra bunden, dernæst React Router, og til sidst Backbone's router.

---

## 13.1 Implementering af en router fra bunden

Selvom der findes færdige biblioteker til routing i React, starter kapitlet med at implementere en simpel router for at vise, hvor let det er. Projektet hjælper også med at forstå, hvordan andre routere virker under motorhjelmen.

Målet er tre sider, der skifter sammen med URL'en, når man navigerer rundt. Der bruges hash-URL'er (`#`) for at holde det simpelt; non-hash URL'er kræver særlig serverkonfiguration. Siderne er:

- **Home** — `/` (tom URL-path)
- **Accounts** — `/#accounts`
- **Profile** — `/#profile`

Implementeringen består af en router-komponent (`router.jsx`), en mapping og en HTML-side. Router-komponenten tager information fra URL'en og opdaterer websiden tilsvarende. Trinene er:

1. Skriv **mappingen** mellem den indtastede URL og den ressource, der skal vises (React-elementer eller komponenter). Mapping er app-specifik, og der skal en ny mapping til for hvert nyt projekt.
2. Skriv **router-biblioteket** fra bunden. Det tilgår den requestede URL og tjekker URL'en mod mappingen (trin 1). Router-biblioteket bliver én enkelt `Router`-komponent i `router.jsx`. Denne `Router` kan genbruges uden ændringer i forskellige projekter.
3. Skriv **eksempel-appen**, som bruger `Router`-komponenten fra trin 2 og mappingen fra trin 1.

Der bruges JSX til at oprette React-elementer til markup'en. `Router` behøver naturligvis ikke være en React-komponent — den kunne være en almindelig funktion eller klasse. Men at bruge en React-komponent forstærker de koncepter, bogen har gennemgået, såsom event lifecycles og udnyttelse af Reacts rendering og DOM-håndtering. Derudover kommer implementeringen tættere på React Routers egen implementering.

### 13.1.1 Opsætning af projektet

Strukturen for projektet (som kan kaldes en simpel eller naiv router) er:

```
/naive-router
  /css
    bootstrap.css
    main.css
  /js
    bundle.js
  /jsx
    app.jsx
    router.jsx
  /node_modules
  index.html
  package.json
  webpack.config.js
```

Først installeres dependencies. De ligger i `package.json`; kopiér dependencies samt babel-config og scripts, og kør `npm install` (`ch13/naive-router/package.json`).

**Listing 13.1 Opsætning af udviklingsmiljøet**

```javascript
{
  "name": "naive-router",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "build": "./node_modules/.bin/webpack -w"
  },
  "author": "Azat Mardan",
  "license": "MIT",
  "babel": {
    "presets": [
      "react"
    ]
  },
  "devDependencies": {
    "babel-core": "6.18.2",
    "babel-loader": "6.2.4",
    "babel-preset-react": "6.5.0",
    "webpack": "2.4.1"
    "react": "15.5.4",
    "react-dom": "15.5.4"
  },
  "dependencies": {
  }
}
```

Kommentarerne i bogens listing forklarer: build-scriptet gemmes som et npm-script for bekvemmelighed; `babel`-feltet fortæller Babel hvilke presets der skal bruges (React til JSX her; ES6+ er valgfrit); og Webpack v2.4.1 installeres lokalt (anbefalet).

*Kurset bruger Vite som build-tool i stedet for Webpack + babel-loader — se slides L16/L17.*

Webpack skal have sin egen konfigurationsfil, `webpack.config.js`. Nøglen er at konfigurere kilden (`entry`) og den ønskede destination (`output`). Man skal også angive loaderen.

**Listing 13.2 webpack.config.js**

```javascript
module.exports = {
  entry: './jsx/app.jsx',
  output: {
    path: __dirname + '/js/',
    filename: 'bundle.js'
  },
  module: {
    loaders: [
      {
        test: /\.jsx?$/,
        exclude: /(node_modules)/,
        loader: 'babel-loader'
      }
    ]
  }
}
```

`entry` definerer filen, der starter bundlingen (typisk hovedfilen, der indlæser de andre filer), `output.path` definerer stien til de bundlede filer, `filename` definerer navnet på den bundlede fil, som bruges i `index.html`, og `loader` specificerer den loader, der udfører JSX-transformationen (og ES6+ hvis nødvendigt).

### 13.1.2 Oprettelse af route-mappingen i app.jsx

Først oprettes en mapping med et mapping-objekt, hvor nøglerne er URL-fragmenter, og værdierne er indholdet af de individuelle sider. En mapping tager en værdi og binder/forbinder den til en anden værdi. Her mapper nøglen (URL-fragmentet) til JSX. Man kunne lave en separat fil for hver side, men her holdes de alle i `app.jsx`.

**Listing 13.3 Route mapping (app.jsx)**

```jsx
const React = require('react')
const ReactDOM = require ('react-dom')
const Router = require('./router.jsx')

const mapping = {
  '#profile': <div >Profile (<a href="#">home</a>)</div>,
  '#accounts': <div >Accounts (<a href="#">home</a>)</div>,
  '*': <div>Dashboard<br/>
    <a href="#profile">Profile</a>
    <br/>
    <a href="#accounts">Accounts</a>
  </div>
}

ReactDOM.render(
  <Router mapping = {mapping}/>,
  document.getElementById('content')
)
```

Her bruges CommonJS `require()` til at importere moduler med Webpack-bundling, et route mapping-objekt mapper routes til individuelle sider, og mappingen sendes videre til `Router` som prop.

### 13.1.3 Oprettelse af Router-komponenten i router.jsx

Kort sagt skal `Router` tage information fra URL'en (`#profile`) og mappe den til JSX ved hjælp af `mapping`-proppen. URL'en tilgås via `window.location.hash` fra browser-API'et:

```jsx
const React = require('react')

module.exports = class Router extends React.Component {
  constructor(props) {
    super(props)
    this.state = {hash: window.location.hash}
    this.updateHash = this.updateHash.bind(this)
  }
  render() {
    ...
  }
}
```

Dernæst skal der lyttes efter URL-ændringer med `hashchange`. Hvis man ikke lytter efter nye URL'er, virker routeren kun én gang: når hele siden genindlæses, og `Router`-elementet oprettes. De bedste steder at tilknytte og fjerne listeners for `hashchange` er lifecycle-metoderne `componentDidMount()` og `componentWillUnmount()`:

```javascript
updateHash(event) {
  this.setState({hash: window.location.hash})
}
componentDidMount() {
  window.addEventListener('hashchange', this.updateHash, false)
}
componentWillUnmount() {
  window.removeEventListener('hashchange', this.updateHash, false)
}
```

*Kurset bruger function components med hooks: `useEffect` med en cleanup-funktion erstatter `componentDidMount`/`componentWillUnmount`, og `useState` erstatter `this.state`/`setState` — se slides L16.*

> **componentDidMount() og componentWillUnmount()**
>
> Kapitel 5 behandler lifecycle events, men her er en genopfriskning. `componentDidMount()` affyres, når et element er mounted og optræder i den rigtige DOM-node (man kan sige, at elementet har en rigtig DOM-node). Derfor er dette det sikreste sted at tilknytte events, der integrerer med andre DOM-objekter, og også at lave AJAX/XHR-kald (ikke brugt her).
>
> På den anden side er `componentWillUnmount()` det bedste sted at fjerne event listeners; elementet bliver unmounted, og man skal fjerne alt det, man har oprettet uden for elementet (såsom en event listener på `window`). At efterlade mange event listeners hængende uden de elementer, der oprettede og brugte dem, er dårlig praksis: det fører til performance-problemer såsom memory leaks.

I `render()` bruges `if/else` til at se, om der er et match mellem den aktuelle URL-værdi (`this.state.hash`) og nøglerne/attributterne/propertierne i `mapping`-proppen. Hvis ja, tilgås `mapping` igen for at hente indholdet af den individuelle side (JSX). Hvis nej, falder man tilbage til `*` for alle andre URL'er, inklusive den tomme værdi (forsiden). Her er den komplette kode (`ch13/naive-router/jsx/router.jsx`).

**Listing 13.4 Implementering af en URL router**

```jsx
const React = require('react')

module.exports = class Router extends React.Component {
  constructor(props) {
    super(props)
    this.state = {hash: window.location.hash}
    this.updateHash = this.updateHash.bind(this)
  }
  updateHash(event) {
    this.setState({hash: window.location.hash})
  }
  componentDidMount() {
    window.addEventListener('hashchange', this.updateHash, false)
  }
  componentWillUnmount() {
    window.removeEventListener('hashchange', this.updateHash, false)
  }
  render() {
    if (this.props.mapping[this.state.hash])
      return this.props.mapping[this.state.hash]
    else
      return this.props.mapping['*']
  }
}
```

Konstruktøren tildeler en initiel URL hash-værdi, `updateHash` fodrer nye URL hash-værdier ind i state, og `render()` renderer indholdet svarende til URL-hashen.

Endelig inkluderes CSS-filen og `bundle.js` i `index.html` — den fil Webpack producerer, når man kører `npm run build` (som igen kører `./node_modules/.bin/webpack -w`):

```html
<!DOCTYPE html>
<html>
  <head>
    <link href="css/bootstrap.css" type="text/css" rel="stylesheet"/>
    <link href="css/main.css" type="text/css" rel="stylesheet"/>
  </head>
  <body>
    <div id="content" class="container"></div>
    <script src="js/bundle.js"></script>
  </body>
</html>
```

Kør bundleren for at få `bundle.js`, og åbn websiden i en browser. Klik på links ændrer både URL'en og indholdet af siden.

At bygge sin egen router med React er altså ligetil; man kan bruge lifecycle-metoder til at lytte efter ændringer i hashen og rendere det passende indhold. Men selvom det er en brugbar mulighed, bliver tingene mere komplekse, hvis man har brug for **nested routes**, bruger route parsing (udtrækning af **URL parameters**) eller bruger "pæne" URL'er uden `#`. Man kunne bruge en router fra Backbone eller et andet front-end, MVC-lignende framework — men der findes en løsning designet specifikt til React (hint: den bruger JSX).

---

## 13.2 React Router

React er fremragende til at bygge UI'er, og kan også bruges til at implementere simpel URL routing fra bunden, som `router.jsx` viste.

Men til mere sofistikerede SPA'er er der brug for flere features. Fx er det almindeligt at sende en **URL parameter** for at betegne et individuelt element frem for en liste af elementer: fx `/posts/57b0ed12fa81dea5362e5e98`, hvor `57b0ed12fa81dea5362e5e98` er et unikt post-ID. Man kunne udtrække denne URL parameter med et regulært udtryk; men før eller siden, hvis applikationen vokser i kompleksitet, ender man med at genopfinde eksisterende implementeringer af front-end URL routing.

> **Semantiske URL'er**
>
> Semantiske eller pæne URL'er sigter mod at forbedre brugervenligheden og tilgængeligheden af et website eller en web-app ved at afkoble den interne implementering fra UI'et. En ikke-semantisk tilgang kunne bruge query strings og/eller script-filnavne. Den semantiske vej bruger derimod kun path'en på en måde, der hjælper brugere med at fortolke strukturen og manipulere URL'erne. Eksempler:
>
> | Ikke-semantisk (okay) | Semantisk (bedre) |
> |---|---|
> | `http://webapplog.com/show?post=es6` | `http://webapplog.com/es6` |
> | `https://www.manning.com/books/react-quickly?a_aid=a&a_bid=5064a2d3` | `https://www.manning.com/books/react-quickly/a/5064a2d3` |
> | `http://en.wikipedia.org/w/index.php?title=Semantic_URL` | `https://en.wikipedia.org/wiki/Semantic_URL` |

Store frameworks som Ember, Backbone og Angular har routing indbygget. Når det gælder routing og React, er **React Router** (`react-router`) en færdig, off-the-shelf-løsning. Afsnit 13.4 dækker en Backbone-implementering og illustrerer, hvor pænt React spiller sammen med dette MVC-lignende framework.

React Router er ikke en del af det officielle React core-bibliotek. Det kom fra communityet, men er modent og populært nok til at en tredjedel af React-projekter bruger det. Det er standardvalget for de fleste React-udviklere.

Syntaksen for React Router bruger JSX, hvilket er endnu et plus, fordi det tillader mere læsbare hierarkiske definitioner end et mapping-objekt. Ligesom den naive `Router`-implementering har React Router en `Router` React-komponent (React Router inspirerede bogens implementering). Trinene er:

1. Opret en mapping, hvor URL'er oversættes til React-komponenter (som bliver til markup på en webside). I React Router opnås dette ved at sende `path`- og `component`-properties samt ved at neste `Route`. Mappingen laves i JSX ved at deklarere og neste `Route`-komponenter. Denne del skal implementeres for hvert nyt projekt.
2. Brug React Routers `Router`- og `Route`-komponenter, som udfører magien med at ændre views efter ændringer i URL'er. Denne del implementerer man ikke selv, men man skal installere biblioteket.
3. Render `Router` på en webside ved at mounte den med `ReactDOM.render()` som et almindeligt React-element. Denne del skal implementeres for hvert nyt projekt.

Der bruges JSX til at oprette en `Route` for hver side, og de nestes enten i en anden `Route` eller i `Router`. `Router`-objektet placeres i `ReactDOM.render()`-funktionen som ethvert andet React-element:

```jsx
ReactDOM.render((
  <Router ...>
    <Route ...>
      <Route ../>
      ...
    </Route>
    <Route .../>
  </Router>
), document.getElementById('content'))
```

Hver `Route` har mindst to properties: `path`, som er det URL-mønster, der skal matche for at trigge denne route; og `component`, som henter og renderer den nødvendige komponent. En `Route` kan have flere properties, såsom event handlers og data. De vil være tilgængelige i `props.route` i den pågældende route-komponent. Sådan sender man data til route-komponenter.

*Kurset bruger React Router v6+: `<Route element={<Home />} />` i stedet for `component`-proppen, og `<Routes>` i stedet for at neste `<Route>` direkte i `<Router>` — se slides L17.3.*

Som illustration betragtes et eksempel på en SPA med routing til nogle få sider: About, Posts (som en blog), en individuel Post, Contact Us og Login. De har forskellige paths og renderes fra forskellige komponenter:

- **About** — `/about`
- **Posts** — `/posts`
- **Post** — `/post`
- **Contact** — `/contact`

About-, Posts-, Post- og Contact Us-siderne bruger samme layout (`Content`-komponenten) og renderes inde i den. Her er den indledende React Router-kode (ikke den komplette, endelige version):

```jsx
<Router>
  <Route path="/" component={Content} >
    <Route path="/about" component={About} />
    <Route path="/about/company" .../>
    <Route path="/about/author" .../>
    <Route path="/posts" component={Posts} />
    <Route path="/posts/:id" component={Post}/>
    <Route path="/contact" component={Contact} />
  </Route>
</Router>
```

Interessant nok kan man neste routes for at genbruge layouts fra parents, og deres URL'er kan være uafhængige af nestingen. Fx er det muligt at have en nested `About`-komponent med URL'en `/about`, selvom "parent"-layout-routen `Content` bruger `/app`. `About` får stadig `Content`-layoutet (implementeret via `this.props.children` i `Content`):

```jsx
<Router>
  <Route path="/app" component={Content} >
    <Route path="/about" component={About} />
    ...
```

Med andre ord behøver `About` ikke den nestede URL `/app/about`, medmindre man ønsker det. Det giver mere fleksibilitet i forhold til paths og layouts.

*Kurset bruger React Router v6+, hvor nested routes normalt får relative paths og layoutet renderer `<Outlet />` i stedet for `this.props.children` — se slides L17.3.*

Til navigation implementeres en menu. Menuen og headeren renderes fra `Content` og genbruges på About-, Posts-, Post- og Contact Us-siderne. Når man navigerer til `/about`, renderes About-siden, menuknappen bliver aktiv, URL'en afspejler at man er på About-siden ved at vise `/#/about`, og teksten "Node.University" afspejler indholdet af `About`-komponenten.

### 13.2.1 React Routers JSX-stil

Der bruges JSX til at oprette `Router`-elementet og de `Route`-elementer, der nestes i det (og i hinanden). Hvert element (`Router` eller `Route`) har mindst to properties, `path` og `component`, som fortæller routeren URL-path'en og den React-komponentklasse, der skal oprettes og renderes. Det er muligt at have yderligere custom properties/attributter til at sende data; den tilgang bruges til at sende et `posts`-array.

Nu importeres React Router-objekterne og bruges i `ReactDOM.render()` til at definere routing-adfærden (`ch13/router/jsx/app.jsx`). Ud over About, Posts, Post og Contact Us oprettes en Login-side.

**Listing 13.5 Definition af Router**

```jsx
const ReactRouter = require('react-router')
let { Router,
  Route,
  Link
} = ReactRouter

ReactDOM.render((
  <Router history={hashHistory}>
    <Route path="/" component={Content} >
      <Route path="/about" component={About} />
      <Route path="/posts" component={Posts} posts={posts}/>
      <Route path="/posts/:id" component={Post} posts={posts}/>
      <Route path="/contact" component={Contact} />
    </Route>
    <Route path="/login" component={Login}/>
  </Router>
), document.getElementById('content'))
```

*Kurset bruger React Router v6+: route-træet defineres typisk med `createBrowserRouter([...])` og `<RouterProvider router={router} />` (eller `<BrowserRouter>` + `<Routes>`), og `history`-proppen findes ikke længere — se slides L17.3.*

Den sidste route, `Login` (`/login`), lever uden for `Content`-routen og har ikke menuen (som ligger i `Content`). Alt, der ikke har brug for det fælles interface i `Content`, kan holdes uden for `Content`-routen. Denne adfærd bestemmes af hierarkiet af nestede routes.

`Post`-komponenten renderer blogpost-information baseret på post-slug'en (en del af URL'en — tænk ID), som den får fra URL'en (fx `/posts/http2`) via variablen `props.params.id`. Ved at bruge en speciel syntaks med kolon i `path`, fortæller man routeren, at den skal parse den værdi og lægge den i `props.params`.

`Router` sendes til `ReactDOM.render()`-metoden. Bemærk, at der sendes `history` til `Router`. Fra og med version 2 af React Router **skal** man levere en history-implementering. Der er to valg: at bruge den history, der er bundlet med React Router, eller at bruge en standalone history-implementering.

### 13.2.2 Hash history

**Hash history** bygger, som navnet antyder, på hash-symbolet `#`, som er måden man navigerer på siden uden at genindlæse den; fx `router/#/posts/http2`. De fleste SPA'er bruger hashes, fordi de skal afspejle ændringer i kontekst inde i appen uden at forårsage et fuldt refresh (en request til serveren). Det samme blev gjort i den håndskrevne router.

> **NOTE** Den korrekte term for en hash er *fragment identifier*.

I dette eksempel bruges også hashes, som kommer standalone fra `history`-biblioteket. Biblioteket importeres, initialiseres og sendes til React Router.

Man skal sætte `queryKey` til `false`, når man initialiserer `history`, fordi man vil deaktivere den irriterende query string (fx `?_k=vl8reh`), som er der som default for at understøtte ældre browsere og overføre states under navigation:

```jsx
const ReactRouter = require('react-router')
const History = require('history')
let hashHistory = ReactRouter.useRouterHistory(History.createHashHistory)({
  queryKey: false
})

<Router history={hashHistory}/>
```

For at bruge den bundlede hash history importeres den fra React Router sådan:

```jsx
const { hashHistory } = require('react-router')

<Router history={hashHistory} />
```

*Kurset bruger React Router v6+, hvor `hashHistory` er erstattet af `createHashRouter` (eller `<HashRouter>`), og history-objektet ikke længere sendes som prop — se slides L17.3.*

Man kan bruge en anden history-implementering med React Router, hvis man foretrækker det. Gamle browsere elsker hash history, men det betyder, at man ser `#`-hashtagget. Har man brug for URL'er uden hash-tegn, kan man det også. Man skal blot skifte til **browser history** og implementere nogle server-modifikationer, som er simple, hvis man bruger Node som HTTP-server-backend. For at holde projektet simpelt bruges hash history her, men browser history gennemgås kort.

### 13.2.3 Browser history

Et alternativ til hash history er browserens HTML5 `pushState`-history. Fx kunne en browser history-URL være `router/posts/http2` frem for `router/#/posts/http2`. Browser history-URL'er kaldes også *real URLs*.

Browser history bruger almindelige, ufragmenterede URL'er, så hver request trigger en server-request. Derfor kræver denne tilgang noget server-side konfiguration. Typisk bør SPA'er bruge fragmenterede/hash-URL'er, især hvis man skal understøtte ældre browsere, fordi browser history kræver en mere kompleks implementering.

Browser history bruges på samme måde som hash history. Man importerer modulet, sætter det ind, og konfigurerer til sidst serveren til at servere den samme fil (ikke filen fra SPA'ens routing).

Browser-implementeringer kommer fra en standalone custom package (som `history`) eller fra implementeringen i React Router (`ReactRouter.browserHistory`). Efter import af browser history-biblioteket anvendes det på `Router`:

```jsx
const { browserHistory } = require('react-router')

<Router history={browserHistory} />
```

*Kurset bruger React Router v6+, hvor `browserHistory` er erstattet af `createBrowserRouter` (eller `<BrowserRouter>`) — se slides L17.3.*

Dernæst skal serveren modificeres til at svare med den samme fil uanset URL'en. Dette eksempel er blot én mulig implementering; det bruger Node.js og Express:

```javascript
const express = require('express')
const path = require('path')
const port = process.env.PORT || 8080
const app = express()

app.use(express.static(__dirname + '/public'))
app.get('*', function (request, response){
  response.sendFile(path.resolve(__dirname, 'public', 'index.html'))
})

app.listen(port)
console.log("server started on port " + port)
```

Grunden til den nødvendige server-side adfærd er, at når man skifter til rigtige URL'er uden hash-tegnet, begynder de at ramme HTTP-serveren. Serveren skal servere den samme SPA JavaScript-kode til hver eneste request. Fx skal requests til `/posts/57b0ed12fa81dea5362e5e98` og `/about` begge resolve til `index.html`, ikke `posts/57b0ed12fa81dea5362e5e98.html` eller `about.html` (som formentlig vil give 404: Not Found).

Fordi hash history er den foretrukne måde at implementere URL routing på, når der er behov for understøttelse af ældre browsere — og for at holde eksemplet simpelt uden at skulle implementere backend-serveren — bruges hash history i resten af kapitlet.

*Bemærk: Vites dev-server (som kurset bruger) håndterer denne SPA-fallback automatisk; i produktion skal serveren stadig konfigureres til at servere `index.html` for alle paths.*

### 13.2.4 React Router-udviklingsopsætning med Webpack

Når man arbejder med React Router, er der biblioteker, der skal bruges og importeres, samt JSX-kompileringen, der skal køre. Her ses udviklingsopsætningen for React Router med Webpack.

Følgende listing viser `devDependencies` fra `package.json` (`ch13/router/package.json`). Nye pakker er `history` og `react-router`. Brug de præcise versioner, der er vist; ellers kan man ikke være sikker på, at koden kører.

**Listing 13.6 Dependencies til Webpack v1, React Router v2.6, React v15.2 og JSX**

```javascript
{
  ...
  "devDependencies": {
    "babel-core": "6.11.4",
    "babel-loader": "6.2.4",
    "babel-preset-react": "6.5.0",
    "history": "2.1.2",
    "react": "15.2.1",
    "react-dom": "15.2.1",
    "react-router": "2.6.0",
    "webpack": "1.12.9"
  }
}
```

Ud over `devDependencies` skal `package.json` have en Babel-konfiguration. Det anbefales også at tilføje npm-scripts:

```javascript
{
  ...
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "build": "./node_modules/.bin/webpack -w",
    "i": "rm -rf ./node_modules && npm cache clean && npm install"
  },
  "babel": {
    "presets": [
      "react"
    ]
  },
  ...
}
```

Bemærk, at fordi JSX konverteres til `React.createClass()`, skal man importere og definere `React` i filer, der bruger JSX — selv når de ikke bruger React direkte. Fx ser det i listing 13.7 ud som om `About`-komponenten (som er stateless — altså en funktion) ikke bruger React. Men når koden transpileres, vil den bruge React i form af `React.createElement()`-kald. I kapitel 1 og 2 var React defineret som en global `window.React`; men med en modulær, ikke-global tilgang er den ikke det. Derfor skal `React` defineres eksplicit (`ch13/router/jsx/about.jsx`).

**Listing 13.7 Eksplicit definition af React**

```jsx
const React = require('react')

module.exports = function About() {
  return <div>
    <a href="http://Node.University" target="_blank">Node.University</a>
    is home to top-notch Node education which brings joy to JavaScript
    engineers.
  </div>
}
```

*Bemærk: med den moderne JSX transform (React 17+, som Vite bruger) er dette `import React` ikke længere nødvendigt.*

Resten af filerne og projektet som helhed bruger denne struktur:

```
/router
  /css
    bootstrap.css
    main.css
  /js
    bundle.js
    bundle.js.map
  /jsx
    about.jsx
    app.jsx
    contact.jsx
    content.jsx
    login.jsx
    post.jsx
    posts.jsx
  /node_modules
  index.html
  package.json
  posts.js
  webpack.config.js
```

`bundle.js` og `bundle.js.map` er den bundlede (sammenkædede) fil og dens source map, til bedre debugging. `posts.js` indeholder data for blogposts, såsom URL'er, titler og tekst.

`index.html` er minimal, fordi den kun inkluderer den bundlede fil.

**Listing 13.8 index.html**

```html
<!DOCTYPE html>
<html>
  <head>
    <link href="css/bootstrap.css" type="text/css" rel="stylesheet"/>
    <link href="css/main.css" type="text/css" rel="stylesheet"/>
  </head>
  <body>
    <div id="content" class="container"></div>
    <script src="js/bundle.js"></script>
  </body>
</html>
```

`webpack.config.js` skal som minimum have et entry-point `app.jsx`, `babel-loader` og source maps (`ch13/router/webpack.config.js`).

**Listing 13.9 Konfiguration af Webpack**

```javascript
module.exports = {
  entry: './jsx/app.jsx',
  output: {
    path: __dirname + '/js/',
    filename: 'bundle.js'
  },
  devtool: '#sourcemap',
  stats: {
    colors: true,
    reasons: true
  },
  module: {
    loaders: [
      {
        test: /\.jsx?$/,
        exclude: /(node_modules)/,
        loader: 'babel-loader'
      }
    ]
  }
}
```

`devtool` sættes for at få den korrekte mapping til ens JSX-kildekode, ikke den transpilerede.

### 13.2.5 Oprettelse af en layout-komponent

`Content`-komponenten, som er defineret som en parent `Route`, fungerer som layout for `About`-, `Posts`-, `Post`- og `Contact`-komponenterne.

Først importeres React og `Link` fra React Router. Sidstnævnte er en speciel komponent til at rendere navigationslinks. `Link` er en speciel wrapper omkring `<a>`; den har nogle magiske attributter, som det normale anchor-tag ikke har, såsom `activeClassName="active"`, der tilføjer klassen `active`, når denne route er aktiv.

`Content`-komponentens struktur ser sådan ud, med udeladelse af enkelte dele:

```jsx
const React = require('react')
const {Link} = require('react-router')

class Content extends React.Component {
  render() {
    return (
      <div>
      ...
      </div>
    )
  }
}
...
module.exports = Content
```

I `render()` bruges Twitter Bootstrap UI-biblioteket til at deklarere menuen med de rigtige klasser. Menuen kan oprettes med færdiglavede CSS-klasser:

```jsx
<div className="navbar navbar-default">
  <ul className="nav nav-pills navbar-nav ">
    <li ...>
      <Link to="/about" activeClassName="active">
        About
      </Link>
    </li>
    <li ...>
      <Link to="/posts" activeClassName="active">
        Blog
      </Link>
    </li>
    ...
  </ul>
</div>
```

Man tilgår `isActive()`-metoden, som returnerer `true` eller `false`. På den måde bliver et aktivt menulink visuelt anderledes end de øvrige links:

```jsx
<li className={(this.context.router.isActive('/about'))? 'active': ''}>
  <Link to="/about" activeClassName="active">
    About
  </Link>
</li>
```

Bemærk `activeClassName`-attributten på `Link`. Når man sætter denne attribut til en værdi, anvender `Link` klassen på et aktivt element (det valgte link). Men man skal sætte stilen på `<li>`, ikke kun på `Link`. Derfor tilgås også `router.isActive()`.

*Kurset bruger React Router v6+: `<Link>` findes stadig, men `activeClassName` er erstattet af `<NavLink className={({isActive}) => ...}>`, og `router.isActive()` er erstattet af `useLocation`/`useMatch` — se slides L17.3.*

Efter `Content`-klassens definition defineres et statisk felt/attribut `contextTypes`, som muliggør brugen af `this.context.router`. Bruger man ES2017+/ES8+, kan man have understøttelse af statiske felter, men det er ikke tilfældet i ES2015/ES6 eller ES2016/ES7. ES2017/ES8-standarden var ikke endelig, da bogen blev skrevet, og havde heller ikke denne feature. Tjek den aktuelle liste over færdige proposals, eller overvej at bruge ES Next (samling af stage 0-proposals).

Denne statiske attribut bruges af React Router sådan, at hvis den kræves, populerer React Router `this.context` (hvorfra man kan tilgå `router.isActive()` og andre metoder):

```javascript
Content.contextTypes = {
  router: React.PropTypes.object.isRequired
}
```

At have `contextType` og `router` sat til required giver adgang til `this.context.router.isActive('/about')`, som igen fortæller, hvornår netop denne route er aktiv.

Her er den fulde implementering af `Content`-layoutet.

**Listing 13.10 Komplet Content-komponent**

```jsx
const React = require('react')
const {Link} = require('react-router')

class Content extends React.Component {
  render() {
    return (
      <div>
        <h1>Node.University</h1>
        <div className="navbar navbar-default">
          <ul className="nav nav-pills navbar-nav ">
            <li className={(this.context.router.isActive('/about'))?
              'active': ''}>
              <Link to="/about" activeClassName="active">
                About
              </Link>
            </li>
            <li className={(this.context.router.isActive('/posts'))?
              'active': ''}>
              <Link to="/posts" activeClassName="active">
                Blog
              </Link>
            </li>
            <li className={(this.context.router.isActive('/contact'))?
              'active': ''}>
              <Link to="/contact" activeClassName="active">
                Contact Us
              </Link>
            </li>
            <li>
              <Link to="/login" activeClassName="active">
                Login
              </Link>
            </li>
          </ul>
        </div>
        {this.props.children}
      </div>
    )
  }
}

Content.contextTypes = {
  router: React.PropTypes.object.isRequired
}

module.exports = Content
```

I listingen tilgås Router og dens metode for at tjekke den aktive route, `Link` bruges til at oprette et navigationslink, `{this.props.children}` renderer child routes (defineret i `app.jsx`), og `contextTypes` definerer, at komponenten skal have et `router`-objekt i konteksten.

`children`-udtrykket gør det muligt at genbruge menuen på hver subroute (route nestet i `/`-routen), såsom `/posts`, `/post`, `/about` og `/contact`:

```jsx
{this.props.children}
```

*Kurset bruger React Router v6+: `<Outlet />` erstatter `this.props.children` som placeholder for nested routes — se slides L17.3.*

---

## 13.3 React Router-features

For at lære mere om React Routers features og patterns ses her på en anden måde at tilgå en router fra child-komponenter på, hvordan man navigerer programmatisk inde i dem, hvordan man parser URL parameters, og hvordan man sender data videre.

### 13.3.1 Adgang til router med higher-order-komponenten withRouter

At bruge `router` gør det muligt at navigere programmatisk og tilgå den aktuelle route, blandt andet. Det er godt at inkludere adgang til `router` i sine komponenter.

Man har set, hvordan man tilgår `router` fra `this.context.router` ved at sætte den statiske klasseattribut `contextTypes`:

```javascript
Content.contextTypes = {
  router: React.PropTypes.object.isRequired
}
```

På en måde bruger man valideringsmekanismen til at definere API'et; altså at komponenten skal have routeren. `Content`-komponenten brugte denne tilgang.

Men `context` afhænger af Reacts context, som er en eksperimentel tilgang, og hvis brug frarådes af React-teamet. Heldigvis findes der en anden vej — nogle vil hævde den er simplere og bedre: **`withRouter`**.

`withRouter` er en higher-order component (HOC; mere om dem i kapitel 8), som tager en komponent som argument, injicerer `router` og returnerer en anden HOC. Fx kan man injicere `router` i `Contact` sådan:

```jsx
const {withRouter} = require('react-router')
...
<Router ...>
  ...
  <Route path="/contact" component={withRouter(Contact)} />
</Router>
```

Når man ser på `Contact`-komponentens implementering (en funktion), er `router`-objektet tilgængeligt fra propertierne (argument-objektet til funktionen):

```jsx
const React = require('react')

module.exports = function Contact(props) {
  // props.router - GOOD!
  return <div>
  ...
  </div>
}
```

Fordelen ved `withRouter` er, at den virker med almindelige, stateful React-klasser såvel som med stateless funktioner.

> **NOTE** Selvom der ikke er nogen direkte (synlig) brug af React, skal man kalde `require` på React, fordi denne kode konverteres til kode med `React.createElement()`-statements, der afhænger af `React`-objektet.

*Kurset bruger React Router v6+: HOC'en `withRouter` findes ikke længere — man bruger i stedet hooks som `useNavigate`, `useLocation` og `useParams` direkte i function components — se slides L17.3.*

### 13.3.2 Programmatisk navigation

En populær anvendelse af `router` er at navigere programmatisk: at ændre URL'en (location) inde fra sin kode baseret på logik frem for brugerhandlinger. Antag fx en app, hvor brugeren skriver en besked i en kontaktformular og indsender den. Baseret på server-svaret navigerer appen til en Error-side, en Thank-you-side eller en About-side.

Når man har `router`, kan man navigere programmatisk ved at kalde `router.push(URL)`, hvor `URL` skal være en defineret route-path. Fx kan man navigere til About fra Contact efter 1 sekund.

**Listing 13.11 Kald af router.push() til navigation**

```jsx
const React = require('react')

module.exports = function Contact(props) {
  setTimeout(()=>{props.router.push('about')}, 1000)
  return <div>
    <h3>Contact Us</h3>
    <input type="text" placeholder="your email" className="form-control"
      ></input>
    <textarea type="text" placeholder="your message" className="form-control">
      </textarea>
    <button className="btn btn-primary">send</button>
  </div>
}
```

*Kurset bruger React Router v6+: `const navigate = useNavigate()` og derefter `navigate('/about')` i stedet for `props.router.push('about')` — se slides L17.3.*

Programmatisk navigation er en vigtig feature, fordi den lader dig ændre applikationens state.

### 13.3.3 URL parameters og andre route-data

Som vist giver `contextTypes` og `router` adgang til objektet `this.context.router`. Det er en instans af `<Router/>` defineret i `app.jsx`, og det kan bruges til at navigere, hente den aktive path og så videre. På den anden side er der anden interessant information i `this.props`, og der kræves ingen statisk attribut for at tilgå den:

- `history` (deprecated i v2.x; brug `context.router`)
- `location`
- `params`
- `route`
- `routeParams`
- `routes`

Objekterne `this.props.location` og `this.props.params` indeholder data om den aktuelle route, såsom path-navn, URL parameters (navne defineret med kolon `:`) og så videre.

Her bruges `params.id` i `post.jsx` til `Post`-komponenten sammen med `Array.find()` til at finde den post, der svarer til en URL-path som `router/#/posts/http2` (`ch13/router/jsx/post.jsx`).

**Listing 13.12 Rendering af post-data**

```jsx
const React = require('react')

module.exports = function Product(props) {
  let post = props.route.posts.find(element=>element.slug ==
    props.params.id)
  return (
    <div>
      <h3>{post.title}</h3>
      <p>{post.text}</p>
      <p><a href={post.link} target="_blank">Continue reading...</a></p>
    </div>
  )
}
```

Posten findes ved sin `slug`-property.

*Kurset bruger React Router v6+: `const { id } = useParams()` i stedet for `props.params.id` — se slides L17.3.*

Når man navigerer til Posts-siden, er der en liste af posts. Route-definitionen er som en påmindelse:

```jsx
<Route path="/posts" component={Posts} posts={posts}/>
```

Et klik på en post navigerer til `#/posts/ID`. Den side genbruger `Content`-komponentens layout.

### 13.3.4 Videregivelse af properties i React Router

Man har ofte brug for at sende data til nestede routes, og det er let at gøre. I eksemplet skal `Posts` have data om posts. I listing 13.13 tilgår `Posts` en property, der blev sendt med i `<Route/>` i `app.jsx`: `posts`, fra filen `posts.js`. Det er muligt at sende vilkårlige data til en route som en attribut; fx `<Route path="/posts" component={Posts} posts={posts}/>`. Man kan derefter tilgå dataene i `props.route`; fx er `props.route.posts` en liste af posts.

**Listing 13.13 Posts-implementering med data fra props.route**

```jsx
const {Link} = require('react-router')
const React = require('react')

module.exports = function Posts(props) {
  return <div>Posts
    <ol>
      {props.route.posts.map((post, index)=>
        <li key={post.slug}><Link
          to={`/posts/${post.slug}`} >{post.title}</Link></li>
      )}
    </ol>
  </div>
}
```

`props.route.posts` tilgår en attribut defineret i route-deklarationen.

*Kurset bruger React Router v6+: vilkårlige props sættes ikke på `<Route>` — data gives enten direkte i `element={<Posts posts={posts} />}` eller via loaders og `useLoaderData()` — se slides L17.3.*

Værdien af disse data kan naturligvis være en funktion. På den måde kan man sende event handlers til stateless komponenter og kun implementere dem i hovedkomponenten, såsom `app.jsx`.

Nu er alle større dele på plads, og projektet kan startes. Det gøres ved at køre et npm-script (`npm run build`) eller ved at bruge `./node_modules/.bin/webpack -w` direkte. Vent på at build'et bliver færdigt, og man ser noget i stil med:

```
> router@1.0.0 build /Users/azat/Documents/Code/react-quickly/ch13/router
> webpack -w

Hash: 07dc6eca0c3210dec8aa
Version: webpack 1.12.9
Time: 2596ms
        Asset     Size  Chunks             Chunk Names
    bundle.js   976 kB       0  [emitted]  main
bundle.js.map  1.14 MB       0  [emitted]  main
    + 264 hidden modules
```

Åbn i et nyt vindue din foretrukne statiske server, og naviger til lokationen i din browser. Prøv at gå til `/` og `/#/about`; den præcise URL afhænger af, om du kører din statiske server fra samme mappe eller en parent-mappe.

---

## 13.4 Routing med Backbone

Når man har brug for routing til en single-page application, er det ligetil at bruge React sammen med andre routing- eller MVC-lignende biblioteker. Backbone er fx et af de mest populære front-end frameworks med indbygget front-end URL routing. Backbone-routeren kan bruges til at rendere React-komponenter ved at gøre følgende:

- Definere en router-klasse med `routes`-objektet som en mapping fra URL-fragmenter til funktioner
- Rendere React-elementer i metoderne/funktionerne i Backbone `Router`-klassen
- Instantiere og starte Backbone `Router`-objektet

Projektstrukturen er:

```
/backbone-router
  /css
    bootstrap.css
    main.css
  /js
    bundle.js
    bundle.map.js
  /jsx
    about.jsx
    app.jsx
    contact.jsx
    content.jsx
    login.jsx
    post.jsx
    posts.jsx
  /node_modules
    ...
  index.html
  package.json
  posts.js
  webpack.config.js
```

`package.json` inkluderer Backbone v1.3.3 ud over de sædvanlige mistænkte, såsom Webpack v2.4.1, React v15.5.4 og Babel v6.11:

```javascript
{
  "name": "backbone-router",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "build": "./node_modules/.bin/webpack -w",
    "i": "rm -rf ./node_modules && npm cache clean && npm install"
  },
  "author": "Azat Mardan",
  "license": "MIT",
  "babel": {
    "presets": [
      "react"
    ]
  },
  "devDependencies": {
    "babel-core": "6.11.4",
    "babel-loader": "6.4.1",
    "babel-preset-react": "6.5.0",
    "backbone": "1.3.3",
    "jquery": "3.1.0",
    "react": "15.5.4",
    "react-dom": "15.5.4",
    "webpack": "2.4.1"
  }
}
```

Hovedlogikkens kilde er i `app.jsx`, hvor alle tre ovennævnte opgaver udføres:

```jsx
const Backbone = require ('backbone')
// Include other libraries

const Router = Backbone.Router.extend({
  routes: {
    ''      : 'index',
    'about' : 'about',
    'posts' : 'posts',
    'posts/:id' : 'post',
    'contact' : 'contact',
    'login': 'login'
  },
  ...
})
```

Når `routes`-objektet er defineret, kan man definere metoderne. Værdierne i `routes` skal bruges som metodenavne:

```jsx
// Include libraries

const Router = Backbone.Router.extend({
  routes: {
    ''      : 'index',
    'about' : 'about',
    'posts' : 'posts',
    'posts/:id' : 'post',
    'contact' : 'contact',
    'login': 'login'
  },
  index: function() {
    ...
  },
  about: function() {
    ...
  }
  ...
})
```

Hvert URL-fragment mapper til en funktion. Fx trigger `#/about` funktionen `about`. Man kan altså definere disse funktioner og rendere sine React-komponenter i dem. Dataene sendes med som en property (`router` eller `posts`):

```jsx
const {render} = require ('react-dom')
// ...

const Router = Backbone.Router.extend({
  routes: {
    ...
  },
  index: function() {
    render(<Content router={router}/>, content)
  },
  about: function() {
    render(<Content router={router}>
      <About/>
    </Content>, content)
  },
  posts: function() {
    render(<Content>
      <Posts posts={posts}/>
    </Content>, content)
  },
  post: function(id) {
    render(<Content>
      <Post id={id} posts={posts}/>
    </Content>, content)
  },
  contact: function() {
    render(<Content>
      <Contact />
    </Content>, content)
  },
  login: function() {
    render(<Login />, content)
  }
})

let router = new Router()
Backbone.history.start()
```

Her bruges destructuring til at importere og definere `render()` fra `ReactDOM.render()`. `Content` oprettes med `About` indeni — routeren kan sendes med som property. Nødvendige data sendes til `Post`, såsom en URL parameter (`id`) og `posts`-data. `Login` renderes uden `Content`. Til sidst instantieres `Router`, og browser-historikken startes.

`content`-variablen er en DOM-node (som deklareres før routeren):

```javascript
let content = document.getElementById('content')
```

Sammenlignet med React Router-eksemplet får nestede komponenter som `Post` deres data ikke i `props.params` eller `props.route.posts`, men i `props.id` og `props.posts`. Efter forfatterens mening betyder det mindre magi — hvilket altid er godt. Til gengæld får man ikke den deklarative JSX-syntaks og må bruge en mere imperativ stil.

Dette eksempel giver et forspring, hvis man har et Backbone-system eller planlægger at bruge Backbone. Og selv hvis man ikke planlægger at bruge Backbone, viser det endnu en gang, at React er fremragende til at arbejde sammen med andre biblioteker.

---

## 13.5 Quiz

1. Man skal levere en history-implementering til React Router v2.x (den version dette kapitel dækker), fordi den som default ikke bruger en. Sandt eller falsk?
2. Hvilken history-implementering er bedre understøttet af ældre browsere: hash history eller browser HTML5 `pushState`-history?
3. Hvad skal man implementere for at få adgang til `router`-objektet i en route-komponent, når man bruger React Router v2.x?
4. Hvordan tilgår man URL parameters i en route-komponent (stateless eller stateful), når man bruger React Router v2.x?
5. React Router kræver brug af Babel og Webpack. Sandt eller falsk?

---

## 13.6 Opsummering

- Man kan implementere routing med React på en naiv måde ved at lytte efter `hashchange`.
- React Router leverer JSX-syntaksen til at definere et routing-hierarki: `<Router><Route/></Router>`.
- Nested routes behøver ikke have nestede URL'er relativt til deres parent routes; path og nestethed er uafhængige.
- Man kan bruge hash history uden tokens ved at sætte `queryKey` til `false`.
- Man skal inkludere React (`require('react')`), når man bruger JSX, selv om der ikke er nogen synlig brug af React, fordi JSX konverteres til `React.createElement()`, som har brug for `React`.

---

## 13.7 Quiz-svar

1. **Sandt.** Version 1.x af React Router indlæste en history-implementering som default; men i version 2.x skal man selv levere et bibliotek, enten fra en standalone-pakke eller en, der er bundlet med router-biblioteket.
2. **Hash history** er bedre understøttet af ældre browsere.
3. Den statiske klasseattribut `contextTypes`, med `router` som et required objekt.
4. Fra `props.params` eller `props.routeParams`.
5. **Falsk.** Man kan bruge det rent og/eller med andre build-værktøjer såsom Gulp og Browserify.

---

## Forskelle mod kursets React Router-version

Bogen bruger React Router v2.x (v3/v4-æraens API). Kursets slides (L17.3) bruger moderne React Router (v6+). Nedenfor er de API-forskelle, kapitlet faktisk berører.

| Bogens API (React Router v2.x) | Moderne ækvivalent (React Router v6+) |
|---|---|
| `<Router history={hashHistory}>` med `history`-prop | `createHashRouter([...])` + `<RouterProvider>` (eller `<HashRouter>`) |
| `<Router history={browserHistory}>` | `createBrowserRouter([...])` + `<RouterProvider>` (eller `<BrowserRouter>`) |
| `hashHistory` / `browserHistory` fra `react-router` | Ingen history-objekter som props; router-typen vælges via `create*Router` |
| `useRouterHistory(History.createHashHistory)({queryKey: false})` fra `history`-pakken | Bortfaldet; ingen `queryKey`-håndtering |
| `<Route path="/about" component={About} />` | `<Route path="/about" element={<About />} />` |
| `<Route>` nestet direkte i `<Router>` | `<Route>` nestet i `<Routes>` (eller route-objekter i `createBrowserRouter`) |
| `{this.props.children}` i layout-komponenten | `<Outlet />` |
| `props.params.id` | `useParams()` → `const { id } = useParams()` |
| `props.route.posts` (custom props på `<Route>`) | Props gives direkte i `element={<Posts posts={posts} />}`, eller data via loader + `useLoaderData()` |
| `props.router.push('about')` | `useNavigate()` → `navigate('/about')` |
| `withRouter(Contact)` HOC | Hooks direkte i komponenten: `useNavigate`, `useLocation`, `useParams` |
| `this.context.router` + `Content.contextTypes = { router: React.PropTypes.object.isRequired }` | Bortfaldet; brug hooks (`useNavigate`, `useLocation`) |
| `this.context.router.isActive('/about')` | `<NavLink>` med `className={({isActive}) => ...}`, eller `useMatch()` |
| `<Link to="/about" activeClassName="active">` | `<NavLink to="/about" className={({isActive}) => isActive ? 'active' : ''}>` |
| `React.PropTypes` | Egen `prop-types`-pakke; kurset bruger typisk TypeScript i stedet |
| Class components + `componentDidMount`/`componentWillUnmount` | Function components + `useEffect` med cleanup |
| Webpack + `babel-loader` + `bundle.js` | Vite (dev-server med SPA-fallback og ESM) |
