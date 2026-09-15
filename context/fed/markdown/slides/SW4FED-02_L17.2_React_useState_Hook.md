# L17 – React Hooks: useState

## Metadata

- **Lektion:** L17 – React Hooks / useState
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L17/FED React useState Hook.pdf (19 slides)
- **Emner dækket:**
  - Hvad hooks er, og hvorfor de findes
  - React-arkitektur og hvordan hooks knytter sig til den virtuelle DOM
  - Import-måder for React og hooks
  - Hvorfor almindelige variabler ikke trigger re-render
  - `useState` — grundlæggende brug
  - Funktionel opdatering (previous state)
  - Objekter i state og spread-operatoren
  - Lazy initial state
  - Flere state-variabler i samme komponent
  - Deling af state mellem parent og children (lifting state)
  - Rules of Hooks
  - Oversigt over de indbyggede hooks

---

## 1. Hvad er Hooks?

Hooks er funktioner, der lader dig "hooke ind i" React's state- og lifecycle-features fra function components.

- Hooks virker **ikke** inde i classes.
- Hooks lader dig bruge state og andre React-features, som lifecycle methods, uden at skrive en class.
- Hooks betyder, at du altid kan bruge funktioner, i stedet for konstant at skifte mellem funktioner, classes, higher-order components og render props. Det giver renere og kortere kode.
- Hooks blev tilføjet til React i version 16.8.

## 2. React-arkitektur

React's hovedopgave er at vedligeholde og opdatere et virtual DOM-træ i hukommelsen.

Enhver funktion, der returnerer JSX og bliver renderet af ReactDOM, er en komponent og vil have tilsvarende noder i den virtuelle DOM. Og præcis som en DOM-node holder hver node i dette træ noget intern state og properties som focus eller input.

React Hooks er måden, hvorpå function components hooker ind i React-runtimen for at gemme eller hente data, eller for at trigge funktioner på det rigtige tidspunkt. Hooks er specifikke for komponenten og bliver destrueret og oprettet sammen med komponenten.

## 3. React hooks i praksis

Tilføjelsen af React Hooks betyder, at du nu kan bruge function components til at håndtere state og side effects.

### Kodeeksempel

```jsx
function MyComponent (props) {
  // Use local state.
  const [value, setValue] = useState(initialValue);
  const [state, dispatch] = useReducer(reducer, initialState);

  useEffect(() => {
    // Perform side effect.
  });

  return (
    <p>{value} and {state.message}</p>
  );
}
```

## 4. Måder at importere React og bruge hooks

Alle de følgende virker, men den **named import** er den, kurset anbefaler.

```javascript
// global
window.React.useState()

// CommonJS
const React = require('react')
React.useState()

// ESModules default import
import React from 'react'
React.useState()

// ESModules named import  <-- Use this
import {useState} from 'react'
useState()

// ESModules namespace import
import * as React from 'react'
React.useState()
```

## 5. Hvordan ved React, at state har ændret sig?

Bare fordi du ændrer værdien af en variabel inde i din komponent-funktion, betyder det ikke, at React opdager det.

### Kodeeksempel — det der IKKE virker

```jsx
window.count = 0;

export function NotWorkingCounter() {
  return (
    <div>
      <p>You clicked {window.count} times</p>
      <button onClick={() => window.count++}>
        Click me
      </button>
    </div>
  );
}
```

Vi har brug for en måde at ændre den værdi på — en slags **updater function**, der trigger React til at kalde komponenten med den nye værdi og få det opdaterede UI. Det er præcis det, `useState`-hooket er til for.

### Kodeeksempel — det der virker

```jsx
export function SimpleCounter() {
  const [count, setCount] = useState(0);
  return (
    <div>
      <p>You clicked {count} times</p>
      <button onClick={() => setCount(count + 1)}>
        Click me
      </button>
    </div>
  );
}
```

## 6. useState

`useState`-hooket initialiserer en variabel, der er specifik for denne komponent, i React-runtimen.

`useState` returnerer et array med to værdier:

- Den **første** værdi er den aktuelle state-værdi.
- Den **anden** værdi er en funktion, der opdaterer state-værdien i React-runtimen og trigger en rerender.

### Kodeeksempel

```jsx
import React, { useState } from "react";

export default function Counter(props) {
  const [count, setCount] = useState(0);

  function handleClick() {
    setCount(count + 1);
  }

  return (
    <div>
      <h2>useState demo</h2>
      <button onClick={handleClick}>
        Increment {count}
      </button>
    </div>
  );
}
```

## 7. Brug den forrige state, når du sætter den nye

For at sikre at du har den nyeste state, når du sætter nye værdier baseret på gamle, sender du en **funktion** som argument til updater-funktionen.

### Kodeeksempel — dette virker ikke som forventet

```jsx
function step2() {
  setCount(count + 1);
  setCount(count + 1);      // Don't do this!
}
```

Begge kald bruger den samme, indfangne værdi af `count`, så tælleren stiger kun med 1.

### Kodeeksempel — korrekt funktionel opdatering

```jsx
export function BetterCounterV2() {
  const [count, setCount] = useState(0);

  function step2() {
    setCount(state => state + 1);
    setCount(state => state + 1);
  }

  return (
    <div style={{padding: "10px"}}>
      <h2>Better step2 useState demo</h2>
      <p>You clicked {count} times</p>
      <button onClick={() => step2()}>
        Click me
      </button>
      &nbsp;
      <button onClick={() => setCount(0)}>
        Reset counter
      </button>
    </div>
  );
}
```

Du sender altså en funktion til `setState`, som tager den forrige state som argument og returnerer den nye state.

## 8. Objekter i state

Objekter i state kræver særlig omhu. Du skal kopiere alle properties over fra det gamle objekt ved hjælp af **spread-operatoren**, når du sætter en ny property-værdi. Ellers mister du de øvrige felter.

### Kodeeksempel

```jsx
const [fruits, setFruits] = useState({ type: "", count: 0 });

function changeType(e) {
  setFruits(state => {
    return {
      ...state,              // kopiér forrige state
      type: e.target.value,  // overskriv med ny værdi
      count: 0,
    }
  });
}
```

```jsx
<p>You picked {fruits.count} {fruits.type}</p>
<button onClick={() => setFruits(state => { return {
    ...state,
    count: state.count + 1 } })}>
  Pick another
</button>
```

## 9. Brug en funktion til at beregne initialværdien (lazy initial state)

Nogle gange skal en komponent lave noget arbejde for at beregne en initialværdi for et stykke state. `useState`-hooket accepterer en funktion som argument — en **lazy initial state**.

React eksekverer funktionen kun **første gang** komponenten renderes, og bruger funktionens returværdi som initial state.

### Kodeeksempel

```jsx
function untangle (aFrayedKnot) {
    // perform expensive untangling manoeuvers
    return nugget;
}

function ShinyString ({tangledWeb}) {
    const [shiny, setShiny] = useState(() => untangle(tangledWeb));
    // use shiny value and allow new shiny values to be set
}

function ShinyComponent ({tangledWeb}) {
    const [shiny, setShiny] = useState(untangle(tangledWeb));
    // use shiny value and allow new shiny values to be set
}
```

Forskellen er vigtig: i `ShinyComponent` bliver `untangle` kaldt **hver gang** komponenten renderer (men værdien bruges kun første gang) — spild af arbejde. I `ShinyString` bliver funktionen kun kaldt én gang.

## 10. Deklarér flere state-variabler

Du kan bruge State Hooket mere end én gang i den samme komponent. React antager, at hvis du kalder `useState` mange gange, gør du det i **samme rækkefølge** ved hver render.

### Kodeeksempel

```jsx
function ExampleWithManyStates() {
  // Declare multiple state variables!
  const [age, setAge] = useState(42);
  const [fruit, setFruit] = useState('banana');
  const [todos, setTodos] = useState([{ text: 'Learn Hooks' }]);
  // ...
}
```

## 11. Sharing state — send shared state til child-komponenter

Når forskellige komponenter bruger de samme data til at bygge deres UI, er den mest eksplicitte måde at dele de data på at sende dem som en **prop** fra parent til children.

Parent-komponenten holder state og sender den til child-komponenterne som en prop — en attribut i JSX'en.

### Kodeeksempel

```jsx
export default function Colors() {
  const availableColors = ["skyblue", "goldenrod", "teal", "coral"];
  const [color, setColor] = useState(availableColors[0]);

  return (
    <div className="colors">
      <ColorPicker colors={availableColors} color={color} setColor={setColor} />
      <ColorChoiceText color={color} />
      <ColorSample color={color} />
    </div>
  );
}
```

## 12. Modtagelse af props

Når React kalder komponenten, sender den som komponentens første argument et objekt, der indeholder alle de props, parenten har sat. Man destructurer typisk props direkte i parameterlisten.

### Kodeeksempel

```jsx
export default function ColorChoiceText({ color }) {
  return color ? (
    <p>The selected color is {color}!</p>
  ) : (
    <p>No color has been selected!</p>
  );
}
```

### Kodeeksempel — med default value

```jsx
export default function ColorSample({ color = 'white' }) {
  return (
    <div className="colorSample" style={{ background: color }} />
  )
}
```

## 13. Send data fra child til parent

En child-komponent kan modtage en **updater function** fra en parent som en prop og derefter bruge updater-funktionen til at ændre state på parenten. Sådan flyder data "opad" i React, selvom data-bindingen i sig selv er one-way.

### Kodeeksempel

```jsx
const availableColors = ["skyblue", "goldenrod", "teal", "coral"];
const [color, setColor] = useState(availableColors[0]);

return (
  <div className="colors">
    <ColorPicker colors={availableColors} color={color} setColor={setColor} />
    {/* ... */}
  </div>
);
```

```jsx
export default function ColorPicker({ colors = [], color, setColor }) {
  // ...
  // onClick={() => setColor(c)}
}
```

## 14. Component concepts — recap

- Komponenter er funktioner, der tager props og returnerer en beskrivelse af deres UI ved hjælp af JSX.
- React invoker komponenterne. Som funktioner kører komponenterne deres kode og slutter så.
- Nogle variabler kan overleve i closures skabt af event handlers. Andre destrueres, når funktionen slutter.
- Vi kan bruge hooks til at bede React om at håndtere state for os — fx `useState`. React kan så sende komponenterne de nyeste værdier og updater-funktioner til dem.
- Ved at bruge updater-funktionen giver vi React besked om ændrede værdier. React kører så komponenten igen for at få den nyeste beskrivelse af UI'et.

## 15. Rules of Hooks

**Kald kun hooks på top level**

- Kald ikke hooks inde i loops, conditions eller nestede funktioner.
- Fordi hooks skal kaldes i samme rækkefølge ved hver render.

**Kald kun hooks fra React-funktioner**

- Kald hooks fra React function components.
- Kald hooks fra custom hooks.

**Hooks navngives `use*`**

- Funktioner, der ikke er hooks, må ikke starte med `use`.

## 16. De indbyggede hooks i React

**Basic Hooks:**

- `useState`
- `useEffect`
- `useContext`

**Additional Hooks:**

- `useReducer`
- `useCallback`
- `useMemo`
- `useRef`
- `useImperativeHandle`
- `useLayoutEffect`
- `useDebugValue`
- …

## 17. References & Links

- "React Quickly", Second Edition, af Morten Barklund og Azat Mardan
- "React Hooks in Action" af John Larsen
- Introducing Hooks: https://reactjs.org/docs/hooks-intro.html
- Why We Switched to React Hooks: https://blog.bitsrc.io/why-we-switched-to-react-hooks-48798c42c7f
- Relearning React with React Hooks: https://betterprogramming.pub/relearning-react-3db1be5a3567
- Should I useState or useReducer? https://kentcdodds.com/blog/should-i-usestate-or-usereducer
- useState lazy initialization and function updates: https://kentcdodds.com/blog/use-state-lazy-initialization-and-function-updates
- Using React in Visual Studio Code (se hvordan man debugger fra VS Code): https://code.visualstudio.com/docs/nodejs/reactjs-tutorial
