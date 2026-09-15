# L23 – React Styling

## Metadata

- **Lektion:** L23 – React Styling
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L23/React Styling.pdf (25 slides)
- **Emner dækket:**
  - De to indbyggede styling-metoder i React: CSS style sheets og `style`-prop
  - `className` i JSX i stedet for `class`
  - Inline styles som JS-objekt
  - CSS Modules (`.module.css`), scoping og `composes`
  - Styled-components og tagged template literals
  - CSS-in-JS: runtime vs. zero-runtime, fordele og ulemper
  - MUI (Material UI): installation, Roboto-font, ikoner, `CssBaseline`
  - Tailwind CSS som utility-first alternativ

---

## 1. Hvordan styler man en webside med React?

React understøtter i sig selv — uden yderligere værktøjer — to måder at style elementer på:

- Med CSS style sheets
- Med `style`-proppen (inline style)

Alt derudover kommer fra tredjepartsbiblioteker.

## 2. Regular CSS

CSS style sheets virker præcis som ved rent HTML-markup. Den største forskel er, at man skal bruge `className`-proppen i stedet for `class`.

I JSX tager både `id`-proppen og `className`-proppen enten en streng eller et expression, der evaluerer til en streng:

### Kodeeksempel

```jsx
//JSX
<div
  id="side-bar"
  className={isSmall ? 'side-bar--small' : 'side-bar'}
/>
```

Det resulterende DOM:

```html
<div id="side-bar" class="side-bar"/>
```

## 3. style-proppen — inline styles

`style`-proppen tager et JS-objekt med CSS-properties og værdier og oversætter det i sidste ende til inline styles på elementet. Bemærk de dobbelte krøllede parenteser: det ydre par er JSX-expression, det indre er selve objektet.

### Kodeeksempel

```jsx
// JSX
<div style={{ color:'red'}}>
  Red Text
</div>
```

```html
//DOM
<div style="color:red;">
  Red Text
</div>
```

## 4. Brug af tredjepartsbiblioteker

At vedligeholde CSS i stor skala kan blive meget udfordrende, så React-communityet har bygget værktøjer, der forbedrer developer experience ved CSS at scale. De største er formentlig:

- CSS modules
- CSS-in-JS (mere specifikt styled-components og emotion)
- Tailwind

## 5. CSS Modules

Enhver CSS-fil, der ender på `.module.css`, betragtes som en CSS modules-fil. Når man importerer sådan en fil, behandles klasserne i CSS-filen som properties på et JS-objekt.

### Kodeeksempel

```jsx
import styles from './Button.module.css';
<button className={styles.error}>Error Button</button>
```

```html
//DOM
<div class="error-5xyn87oq5x"/>
```

Hashen tilføjes til klassen for at sikre, at den er unik i vores app.

Det er typisk, at hver komponent har sin egen CSS-fil tilknyttet, når man bruger CSS modules. Featuren er tilgængelig i Vite-projekter.

## 6. CSS Modules — composes

CSS modules tillader også, at man kombinerer flere klasser gennem `composes`-nøgleordet:

### Kodeeksempel

```css
/* styles.modules.css */
.btn {
    width: 90px;
    height: 40px;
    padding: 10px 20px;
}

.submit {
    composes: btn;
    background-color: green;
    color:#FFFFFF
}
```

Man kan også komponere styles fra et andet CSS-modul:

```css
.submit {
    composes: primary from "./colors.css"
    background-color: green;
}
```

## 7. Naming convention

Vite understøtter CSS Modules side om side med almindelige stylesheets ved hjælp af navnekonventionen `[name].module.css`.

CSS Modules tillader scoping af CSS ved automatisk at oprette et unikt klassenavn af formatet `[filename]_[classname]__[hash]`.

## 8. Styled-Components

Styled-components opfylder samme mål som CSS modules, men griber det anderledes an. Det bruger syntaksen tagged template literals — funktioner, der kaldes med en template literal-streng. Funktionen parser template literalen og kan agere derefter.

### Kodeeksempel

```jsx
import styled from 'styled-components';

const Layout = styled.div`
   display: grid;
`;
```

```html
//DOM
<div class="sc-bZQynM iEpgro" />
```

Et mere fuldstændigt eksempel:

```jsx
import styled from 'styled-components';

const MyTitle = styled.div`
  color: blue,
  background: yellow
`

<MyTitle>My First CSS-in-JS React component!</MyTitle>
```

Dette ville blive renderet af browseren som:

```html
<style>
.hash999s99 {
  background-color: yellow;
  color: blue;
}
</style>
<div class="hash999s99">My First CSS-in-JS React component!</div>
```

## 9. Styled-Components — pros

- Styled components kan være en meget pæn måde at organisere vores React-komponenter på
- Vi behøver ikke gøre vores JSX-kode beskidt med masser af `div`/`span`-elementer
- Vi kan simpelthen rendere komponenterne med deres egne styles
- Da denne tilgang ikke har nogen inline styles, bliver koden let at læse
- Endnu vigtigere: vi kan ændre vores CSS når som helst uden at bekymre os om, hvorvidt det påvirker en anden komponent

## 10. Styled-Components — cons

Sam Magura, staff software engineer hos Spot og aktiv maintainer af CSS-in-JS-biblioteket Emotion, har beskrevet, hvorfor Spot forlod runtime CSS-in-JS-biblioteket Emotion til fordel for Sass modules:

- Runtime overhead
- Payload overhead
- Server rendering-problemer

Magura sammenlignede rendering-tiden for en komponent i Spots kodebase implementeret med runtime CSS-in-JS-biblioteket Emotion med en implementering med Sass modules:

- Sass modules: 27,7 ms
- Emotion: 54 ms

Reference: https://www.infoq.com/news/2022/10/prefer-build-time-css-js

## 11. Alternativer

CSS-in-JS refererer til et mønster, hvor CSS-regler produceres gennem JavaScript i stedet for at være defineret i eksterne CSS-filer. To under-mønstre eksisterer side om side:

- **Runtime CSS-in-JS**-biblioteker, såsom Emotion eller Styled-components, modificerer styles dynamisk ved runtime, for eksempel ved at injicere style-tags i dokumentet
- **Zero-runtime CSS-in-JS** er et mønster, der fremmer at udtrække al CSS på build time

Populære build-time CSS-in-JS-biblioteker inkluderer Linaria, Astroturf og vanilla-extract. I 2021 introducerede Facebook stylex, deres eget build-time CSS-in-JS-bibliotek (https://stylexjs.com/docs/learn/).

## 12. MUI

MUI er et robust, customizable og tilgængeligt bibliotek af foundational og advanced components.

### Installation

For at installere og gemme i `package.json`-dependencies:

```bash
npm install @mui/material @emotion/react @emotion/styled
```

Eller hvis man vil bruge styled-components som styling engine:

```bash
npm install @mui/material @mui/styled-engine-sc styled-components
```

## 13. Roboto-fonten

MUI blev designet med Roboto-fonten i tankerne:

```html
<link rel="stylesheet"
  href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
/>
```

Man kan installere den, hvis man foretrækker det:

```bash
npm install @fontsource/roboto
```

Derefter kan den importeres i entry-pointet:

```javascript
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
```

Man behøver ikke bruge denne font.

## 14. Ikoner

### Font icons

For at bruge font `Icon`-komponenten skal man først tilføje Material icons-fonten:

```html
<link
  rel="stylesheet"
  href="https://fonts.googleapis.com/icon?family=Material+Icons"
/>
```

Som alternativ kan man bruge React icons: https://react-icons.github.io/react-icons/

### SVG icons

For at bruge prebuilt SVG Material icons, såsom dem der findes i icons-demoerne, skal man først installere pakken `@mui/icons-material`:

```bash
npm install @mui/icons-material
```

## 15. Brug af MUI

MUI-komponenter virker isoleret. De er self-supporting og injicerer kun de styles, de har brug for at vise. Man kan bruge enhver af komponenterne, som demonstreret i dokumentationen.

### Kodeeksempel

```jsx
import './App.css';
import Button from '@mui/material/Button';

function App() {
  return (
    <Button variant="contained">Hello World</Button>
  );
}

export default App;
```

## 16. CssBaseline

MUI leverer en valgfri `CssBaseline`-komponent. Den retter nogle inkonsistenser på tværs af browsere og enheder, samtidig med at den giver lidt mere opinionated resets til almindelige HTML-elementer.

### Kodeeksempel

```jsx
import * as React from 'react';
import CssBaseline from '@mui/material/CssBaseline';

export default function MyApp() {
  return (
    <React.Fragment>
      <CssBaseline enableColorScheme/>
      {/* The rest of your application */}
    </React.Fragment>
  );
}
```

## 17. React Templates og læringsressourcer

Et udvalg af gratis React-templates, der hjælper med at komme i gang med at bygge sin app: https://mui.com/getting-started/templates/

Læringsmateriale findes på https://mui.com/getting-started/learn/:

- **Introduction to MUI** — en videoserie, der dækker alle de vigtige MUI-komponenter
- **Customize MUI for your project** — hvordan man tilpasser MUI til sin virksomheds identitet (design system) og produkter
- **Meet MUI — your new favorite user interface library** — et blogindlæg, der guider gennem at bygge en Todo MVC, mens det dækker vigtige MUI-koncepter
- **Learn React & MUI** — videoserie om de vigtige MUI-komponenter
- **Getting Started With MUI For React** — blogindlæg, der guider gennem at bygge en simpel card list
- **Elegant UX in React with MUI** — blogindlæg om vigtige MUI-koncepter

## 18. Tailwind

Tailwind CSS er et utility-first CSS framework fyldt med klasser som `flex`, `pt-4`, `text-center` og `rotate-90`, der kan komponeres til at bygge et hvilket som helst design direkte i ens markup.

Installation:

- https://tailwindcss.com/docs/installation/using-vite
- Video: https://www.youtube.com/watch?v=sHnG8tIYMB4

Tailwind Plus (https://tailwindcss.com/plus) er et bibliotek af 500+ professionelt designede, ekspert-udformede component examples, man kan droppe ind i sine Tailwind-projekter og tilpasse frit.

## 19. References & Links

- How to Style React Components Using CSS Modules — https://www.makeuseof.com/react-components-css-modules-style/
- CSS modules with React — https://create-react-app.dev/docs/adding-a-css-modules-stylesheet
- How To Use Styled-Components In React — https://styled-components.com/docs og https://www.smashingmagazine.com/2020/07/styled-components-react/
- The React UI library – MUI — https://mui.com/
- Styling Best Practices Using React — https://non-traditional.dev/styling-best-practices-using-react-c37b96b8be9c
- Best Practices for Styling React Components — https://www.pluralsight.com/guides/best-practices-styling-react-components
- Style React Components: 7 Ways Compared — https://www.sitepoint.com/react-components-styling-options/
