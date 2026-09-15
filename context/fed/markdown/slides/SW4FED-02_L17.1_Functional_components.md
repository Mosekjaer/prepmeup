# L17 – Functional components (recap)

## Metadata

- **Lektion:** L17 – Functional components
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L17/FED Functional components.pdf (6 slides)
- **Emner dækket:**
  - Hvad en functional component er
  - Anbefalet syntaks vs. alternativ syntaks
  - Destructuring af properties
  - Default values via destructuring
  - Pass-through properties med rest-syntaks (`...rest`)

---

## 1. Hvad er en functional component?

Functional components er funktioner, der tager et `props`-argument og returnerer en beskrivelse af deres UI ved hjælp af JSX.

- Funktionsnavnet skal starte med **stort begyndelsesbogstav**.
- `props`-argumentet er valgfrit.

### Kodeeksempel — anbefalet syntaks

```jsx
function Welcome(props) {
  return <h1>Hello, {props.name}</h1>;
}
```

### Kodeeksempel — alternativ syntaks

```jsx
const App = function() {
  const Menu = () => { return <ul /> };
  return (
    <main>
      <h1>Menu options:</h1>
      <Menu />
    </main>
  );
}
```

## 2. Destructuring properties

Vi kan bruge destructuring, når vi tager imod objekt-argumenter i en function component. Det gør koden kortere, fordi man slipper for at skrive `props.` foran hver værdi.

### Kodeeksempel — uden destructuring

```jsx
function MenuItem(props) {
  return (
    <li>
      <a href={props.href}
         title={props.label}>
        {props.label}
      </a>
    </li>
  );
}
```

### Kodeeksempel — med destructuring i parameterlisten

```jsx
function MenuItem({ href, label }) {
  return (
    <li>
      <a href={href} title={label}>
        {label}
      </a>
    </li>
  );
}
```

Alternativt kan man destructure inde i funktionskroppen:

```jsx
function MenuItem(props) {
  const { href, label } = props;
  // ...
}
```

## 3. Default values

En ekstra fordel ved destructured properties er, at man samtidig kan angive default values. I eksemplet nedenfor får `target` default-værdien `"_self"`, når parenten ikke sender den med.

### Kodeeksempel

```jsx
function Menu() {
  return (
    <ul>
      <MenuItem label="Home" href="/"/>
      <MenuItem label="About" href="/about/" />
      <MenuItem label="Blog" href="/blog" target="_blank" />
    </ul>
  );
}

function MenuItem({ label, href, target="_self" }) {
  return (
    <li>
      <a href={href} title={label} target={target}>
        {label}
      </a>
    </li>
  );
}
```

## 4. Pass-through properties

Når man destructurer et objekt, kan man bruge **rest-syntaksen** — tre punktummer — til at angive et objekt, som får tildelt alle de "left-over" properties, der ikke allerede er blevet tildelt.

Det er nyttigt, når en komponent skal videresende vilkårlige attributter (fx `className`, `id`, `target`) til det underliggende HTML-element uden at kende dem på forhånd.

### Kodeeksempel

```jsx
function Menu() {
  return (
    <ul>
      <MenuItem label="Home" href="/" className="logo" />
      <MenuItem label="About" href="/about/" id="about-link" />
      <MenuItem label="Blog" href="/blog" target="_blank" id="blog-link" />
    </ul>
  );
}

function MenuItem({ label, href, ...rest }) {
  return (
    <li>
      <a href={href} title={label} {...rest}>
        {label}
      </a>
    </li>
  );
}
```

Bemærk de to anvendelser af `...`: i parameterlisten samler den de resterende props op i objektet `rest`, og i JSX'en (`{...rest}`) spredes objektet ud som attributter på `<a>`-elementet.

## 5. References & Links

- "React Quickly", Second Edition, af Morten Barklund og Azat Mardan
