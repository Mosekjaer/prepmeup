# L17 – React Routing (react-router)

## Metadata

- **Lektion:** L17 – React Routing
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L17/FED React Router.pdf (18 slides)
- **Emner dækket:**
  - React Router som de facto standard (routing er ikke en del af React selv)
  - De tre modes: Declarative, Data, Framework — kurset bruger Declarative
  - Scaffolding og versionskrav for React Router v7
  - De fire komponentkategorier: Routers, Route matchers, Navigation, Outlet
  - `<BrowserRouter>`, `<Routes>`, `<Route>`
  - `<Link>` og `<NavLink>`
  - "No match"-route med wildcard `*`
  - `<Navigate>` og `useNavigate`
  - Child routes og `<Outlet>`
  - URL-parametre og `useParams`
  - Lazy loading, code splitting, `React.lazy` og `<Suspense>`

---

## 1. Routing i React

Routing er **ikke** en del af det officielle React-library. Men de facto standard routing-library til React er `react-router`.

## 2. Vælg en mode

React Router er en multi-strategy router til React. Der er tre primære måder — "modes" — at bruge den på:

- **Declarative**
- **Data**
- **Framework**

Features i hver mode er additive: at bevæge sig fra Declarative til Data til Framework tilføjer blot flere features, på bekostning af arkitektonisk kontrol.

**Anbefalet mode i SW4FED er Declarative.** Bruger man React i semesterprojektet, kan man vælge anderledes.

## 3. Scaffold et nyt projekt til Declarative mode

Dette er den anbefalede fremgangsmåde for SW4FED.

```bash
npx create-vite@latest

npm i react-router
```

React Router v7 kræver minimum følgende versioner:

- `node@20`
- `react@18`
- `react-dom@18`

## 4. Scaffold til Framework mode (IKKE anbefalet i SW4FED)

```bash
npx create-react-router@latest app-name
```

Eller:

```bash
npm create vite@latest
```

og vælg React Router.

Efter oprettelse af et Framework mode-projekt skal man generere typer:

```bash
npm run typecheck
```

Begge dele er markeret som **ikke anbefalet** for SW4FED.

## 5. Primary Components

Der er fire primære kategorier af komponenter i React Router:

- **Routers** — fx `<createBrowserRouter>`, `<BrowserRouter>`, `<HashRouter>` m.fl.
- **Route matchers** — fx `<Route>` og `<Routes>`
- **Navigation** — fx `<Link>`, `<NavLink>` og `<Navigate>`
- **Outlet for children** — `<Outlet>` som placeholder for child routes

Plus en række custom hooks. Importér dem du bruger fra `react-router`.

## 6. Render BrowserRouter

Render en `<BrowserRouter>` omkring din applikation for at aktivere React Router i Declarative mode. `<BrowserRouter>` er den declarative router, som **ikke** understøtter data-API'et.

### Kodeeksempel — main.jsx

```jsx
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router'
import './index.css'
import App from './App.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
)
```

Vigtigt om SPA-hosting: din web server skal servere den **samme side** på alle de URL'er, der håndteres client-side af React Router. Ellers får brugeren en 404 ved direkte navigation til en underside.

## 7. Route Matchers

Når en `<Routes>` renderes, søger den gennem sine `<Route>`-children for at finde én, hvis `path` matcher den aktuelle URL. Når den finder én, renderer den den `<Route>` og ignorerer alle andre. Hvis ingen `<Route>` matcher, renderer `<Routes>` ingenting.

### Kodeeksempel

```jsx
function App() {
  return (
    <div className="App">
      <Navbar />
      <Routes>
        <Route path="/" element={<Default />} />
        <Route path="/home" element={<Home />} />
        <Route path="/fea1" element={<Fea1 />} />
        <Route path="/fea2" element={<Fea2 />} />
        <Route path="*" element={<Unknown />} />
      </Routes>
    </div>
  );
}
```

`"*"` er et wildcard, der matcher alt.

## 8. Navigation

Brug en `<Link>`-komponent til at lave links i din applikation. Hvor end du renderer en `<Link>`, bliver et anchor-element (`<a>`) renderet i dit HTML-dokument.

`<NavLink>` er en speciel type `<Link>`, der kan style sig selv som "active", når dens `to`-prop matcher den aktuelle location.

### Kodeeksempel

```jsx
<Link to="/home">Home</Link>

<NavLink to="/feature1">
  Feature 1
</NavLink>
```

```css
.active {
  color: #00f;
  background-color: chocolate;
}
```

## 9. En "No match"-route

`"*"` matcher kun, når ingen andre routes gør.

### Kodeeksempel

```jsx
<Routes>
  <Route path="/fea1" element={<Fea1 />} />
  <Route path="/fea2" element={<Fea2 />} />
  <Route path="*" element={<Unknown />} />
</Routes>
```

```jsx
export function Unknown() {
  return (
    <main>
      <p>You have entered an unknown route.</p>
    </main>
  );
}
```

## 10. Navigate

Et `<Navigate>`-element ændrer den aktuelle location, når det renderes. Det er en komponent-wrapper omkring `useNavigate`-hooket.

### Kodeeksempel

```jsx
import { Navigate } from 'react-router';

export function Default() {
  return (
    <Navigate to="/home" replace={true} />
  );
}
```

## 11. Child routes

Fremgangsmåden er:

- Flyt layoutet ud i en `Layout`-komponent.
- Gør `Fea1` og `Fea2` til children af app-routen.
- Indsæt en `<Outlet />` i `Layout`-komponenten — den fungerer som placeholder, hvor den matchende child route renderes.

### Kodeeksempel

```jsx
function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Layout />} >
          <Route index element={<Home />} />
          <Route path="/fea1" element={<Fea1 />} />
          <Route path="/fea2" element={<Fea2 />} />
          <Route path="*" element={<Unknown />} />
        </Route>
      </Routes>
    </div>
  )
}
```

```jsx
export function Layout() {
  return (
    <>
      <h1>Using child routes demo</h1>
      <Navbar />
      <Outlet />
      <footer>
        <p>Hello world of SPA routing</p>
      </footer>
    </>
  )
}
```

## 12. URL parameters

Params er placeholders i URL'en, der begynder med et kolon, fx `:id`.

### Kodeeksempel — routes i App.jsx

```jsx
<Route path="streaming" element={<Streaming />} >
  <Route path=":id" element={<StreamingChild />} />
</Route>
```

### Kodeeksempel — parent-komponenten

```jsx
export function Streaming() {
  return (
    <div>
      <h2>Streaming service</h2>
      <ul>
        <li>
          <Link to="netflix">Netflix</Link>
        </li>
        <li>
          <Link to="HBO">HBO</Link>
        </li>
      </ul>
      <Outlet />
    </div>
  )
}
```

### Kodeeksempel — child-komponenten med useParams

```jsx
export function StreamingChild() {
  // We can use the `useParams` hook here to access
  // the dynamic pieces of the URL.
  let { id } = useParams();
  return (
    <div>
      <h3>ID: {id}</h3>
    </div>
  );
}
```

## 13. Programmatisk navigation

Nogle gange vil man sende brugeren til en anden side — for eksempel efter et form submit. Det gøres med `useNavigate`-hooket.

### Kodeeksempel

```jsx
import {
  useNavigate
} from "react-router-dom";

export function SomeForm() {
  const navigate = useNavigate();

  function handleSubmit(event) {
    let form = event.currentTarget;
    let name = form.elements[0].value;
    alert('A name was submitted: ' + name);
    event.preventDefault();
    navigate("/");
  }

  return (
    <>
      <h2>Programmatic navigation</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Name:
          <input type="text" />
        </label>
        <input type="submit" value="Submit" />
      </form>
    </>
  );
}
```

## 14. Lazy Loading

Normalt kommer en client-side rendered React-applikation som **ét** bundle fra web-serveren. Når man aktiverer lazy loading, splittes bundlet op i mindre bundles.

Når en bruger besøger en specifik del af applikationen, lazy-loades kun den del on demand. Termen for denne optimering er **code splitting**.

Code splitting forbedrer opstartstiden for den første rendering.

## 15. React.lazy

React tilbyder et top-level API til lazy loading kaldet `React.lazy`.

### Kodeeksempel

```jsx
import { Suspense, lazy } from "react";
import {
  Routes,
  Route,
} from 'react-router';
import { Layout } from './Layout';
import { Home } from './Home';

const Fea1 = lazy(() => import('./Fea1'));     // For default exports
const Fea2 = lazy(() =>                        // For named exports
  import("./Fea2").then((module) => ({
    default: module.Fea2,
  }))
);
```

Bemærk forskellen: default exports kan lazy-loades direkte, mens named exports skal mappes om til en `default`-nøgle.

## 16. Brug af Suspense

`<Suspense>` lader dig vise en fallback, indtil dens children er færdige med at loade.

### Kodeeksempel

```jsx
<Route index element={<Home />} />
<Route path="/fea1" element={
    <Suspense fallback={<>Fea1 is loading...</>}>
      <Fea1 />
    </Suspense>
  }
/>
<Route path="/fea2" element={
    <Suspense fallback={<>Fea2 is loading...</>}>
      <Fea2 />
    </Suspense>
  }
/>
```

## 17. References & Links

- ReactRouter: https://reactrouter.com/en/main
- Declarative API: https://reactrouter.com/start/declarative/routing
- Lazy load: https://www.robinwieruch.de/react-router-lazy-loading/
- Auth Example: https://github.com/remix-run/react-router/tree/dev/examples/auth
- Styling Link components: https://stackoverflow.com/questions/37669391/how-to-get-rid-of-underline-for-link-component-of-react-router
