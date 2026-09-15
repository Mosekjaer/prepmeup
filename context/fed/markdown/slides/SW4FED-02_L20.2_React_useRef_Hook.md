# L20 – React useRef Hook

## Metadata

- **Lektion:** L20 – React Hooks: useRef
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L20/FED React useRef Hook.pdf (7 slides)
- **Emner dækket:**
  - Opdatering af state uden at udløse et re-render
  - Forskellen på `useState` og `useRef`
  - `useRef`-objektets `current`-property
  - Brug af `useRef` til at holde et timer-handle og annullere `setInterval`
  - Brug af `useRef` til at holde referencer til DOM-elementer (knap og formularfelt)

Undertitel på titelsliden: *How the useRef hook helps you manage state in your components.*

---

## 1. Opdatering af state uden at forårsage et re-render

Med **`useState`**-hooket udløser et kald til en state-værdis updater-funktion normalt et re-render.

Med **`useRef`**-hooket kan vi opdatere vores værdi **uden** en tilsvarende ændring af UI'et.

Slide 2 sætter de to varianter op mod hinanden i to kodebokse:

```javascript
const [count, setCount] = useState(1);
const incCount = () => setCount(c => c + 1);
```

```javascript
const ref = useRef(1);
const incRef = () => ref.current++;
```

En taleboble peger på ref-varianten med teksten: **"The value is persisted between calls to the function"**.

Forskellen er altså ikke, om værdien overlever mellem renders — det gør begge — men om en ændring *notificerer* React. `setCount` gør, `ref.current++` gør ikke.

## 2. Kald af useRef

```javascript
const ref = useRef(1);
```

- `useRef`-funktionen returnerer et **objekt med en `current`-property**.
- Hver gang React kører komponentkoden, vil hvert kald til `useRef` returnere **det samme ref-objekt** for netop det kald.
- Du kan persistere state-værdier ved at tildele dem til refs' `current`-properties:

```javascript
const incRef = () => ref.current++;
```

Bemærk at argumentet til `useRef` kun er **startværdien** — den bruges udelukkende ved første render og ignoreres ved efterfølgende renders.

## 3. useRef til at håndtere timer-annullering

Et af de klassiske brugsscenarier: `setInterval` returnerer et handle, som skal gemmes et sted, hvor det overlever re-renders — men uden selv at udløse et. Det er præcis en ref.

```javascript
const [count, setCount] = useState(0);
const incCount = () => setCount(c => c + 1);
const timerRef = useRef(null);

useEffect(() => {
    timerRef.current = setInterval(() => {
        incCount()
    }, 1000);
    return stopCounter;
}, []);

function stopCounter() {
    clearInterval(timerRef.current);
}
```

Og knappen, der stopper tælleren:

```jsx
<button
     className="btn"
     onClick={stopCounter}
>
     Stop counter
</button>
```

To ting at bemærke:

- `useEffect` med det tomme dependency array `[]` kører kun ved mount, så intervallet oprettes én gang.
- `return stopCounter;` gør `stopCounter` til effektens **cleanup-funktion** — den kaldes automatisk ved unmount, så timeren ikke lækker. Samme funktion genbruges som knappens `onClick`.

## 4. At holde referencer til DOM-elementer: en knap

Brug `useRef`-hooket til at gemme en reference til en knap.

```typescript
const nextButtonRef = useRef<HTMLInputElement>(null);

function changeBookable (selectedIndex) {
    dispatch({
      type: "SET_BOOKABLE",
      payload: selectedIndex
    });
    nextButtonRef.current.focus();
  }
```

Og JSX'en, hvor referencen kobles på elementet med `ref`-attributten:

```jsx
<button
    className="btn"
    onClick={nextBookable}
    ref={nextButtonRef}
    autoFocus
>
```

Pilene på slidet peger på de tre steder, der hænger sammen: deklarationen af `nextButtonRef`, kaldet `nextButtonRef.current.focus()`, og `ref={nextButtonRef}` i JSX'en. Når React monterer elementet, sætter den selv `ref.current` til det underliggende DOM-element — derfor kan `.focus()` kaldes direkte på det.

## 5. At holde referencer til DOM-elementer: et formularfelt

Brug `useRef`-hooket til at gemme en reference til et formularfelt.

```typescript
const textboxRef = useRef<HTMLInputElement>(null);

function goToDate () {
  dispatch({
    type: "SET_DATE",
    payload: textboxRef.current.value
  });
}
```

```jsx
<input
   type="text"
   ref={textboxRef}
   placeholder="e.g. 2020-09-02"
   defaultValue="2022-06-24"
 />
```

Taleboblen på slidet forklarer: **`textboxRef.current.value` is the text in the text box.**

Dette er mønstret for et *uncontrolled* input: feltet får en `defaultValue` frem for en `value`-binding, og værdien læses først, når man har brug for den — via ref'en. Det sparer et re-render pr. tastetryk, som en controlled komponent ville koste.

## 6. Referencer og links

- *React Hooks in Action* af John Larsen
