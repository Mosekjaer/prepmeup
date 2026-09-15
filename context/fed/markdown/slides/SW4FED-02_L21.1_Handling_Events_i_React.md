# L21 – Handling Events i React

## Metadata

- **Lektion:** L21 – Handling Events in React
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L21/Handling Events in React.pdf (19 slides)
- **Emner dækket:**
  - Syntaksforskelle mellem DOM-events og React-events
  - Pitfall: pass funktionen, kald den ikke
  - Konventioner for navngivning af event handlers
  - Inline event handlers
  - React event objects (synthetic events) og deres properties
  - Event phases: capture, target, bubbling — også i React
  - `*Capture`-postfix til capture listeners
  - Default actions og `e.preventDefault()`
  - Event handler generators
  - Lytning på DOM-events uden for React (window, document)

---

## 1. Syntaksforskelle

At håndtere events med React elements ligner meget at håndtere events på DOM-elementer. Der er dog nogle syntaksforskelle:

- React-events navngives med **camelCase** i stedet for lowercase.
- Med JSX sender du en **funktion** som event handler, ikke en string.

I HTML:

```html
<button onclick="activateLasers()">
  Activate Lasers
</button>
```

I React:

```jsx
<button onClick={activateLasers}>
  Activate Lasers
</button>
```

React tilføjer og fjerner korrekt event listeneren, efterhånden som komponenten mounter og unmounter.

## 2. Pitfall

Funktioner sendt til event handlers skal **passes**, ikke **kaldes**.

Korrekt — at sende en funktion:

```jsx
<button onClick={handleClick}>
  Click me
</button>
```

Forkert — at kalde en funktion:

```jsx
<button onClick={handleClick()}>
  Click me
</button>
```

I det forkerte tilfælde kører `handleClick` med det samme under rendering, og dens returværdi bliver sat som handler.

## 3. Konventioner

Event handler-funktioner:

- Defineres som regel inde i dine komponenter.
- Har navne, der starter med `handle`, efterfulgt af navnet på eventet:
  - `onClick={handleClick}`
  - `onMouseEnter={handleMouseEnter}`

## 4. Inline event handlers

Du kan definere en event handler inline i JSX'en. Det er praktisk for korte funktioner.

```jsx
<button onClick={() => {
  alert('You clicked me!');
}}>
```

## 5. Eksempel — videoafspiller

Bemærk at eksemplet er skrevet i TypeScript (bemærk typeannotationen `<HTMLVideoElement>`).

### Kodeeksempel

```jsx
import { useState, useRef } from "react";

export function ShowVideo() {
  const VIDEO_SRC = "//images-assets.nasa.gov/video/One Small Step/One Small Step~orig.mp4";
  const [isPlaying, setPlaying] = useState(false);
  const onPlay = () => setPlaying(true);
  const onPause = () => setPlaying(false);
  const onClickPlay = () => video.current?.play();
  const onClickPause = () => video.current?.pause();
  const video = useRef<HTMLVideoElement>(null);

  return (
    <section>
      <video
        ref={video}
        src={VIDEO_SRC}
        controls
        width="480"
        onPlay={onPlay}
        onPause={onPause}
      />
      <button onClick={isPlaying ? onClickPause : onClickPlay}>
        {isPlaying ? "Pause" : "Play"}
      </button>
    </section>
  );
}
```

Demo: https://reactquickly.dev/browse/ch08/rq08-video-player/try

## 6. Information flow

Du kan kun bruge React til at lytte efter events, der er understøttet af React — men næsten alle DOM-events er det.

## 7. React event objects

En React event handler tilføjes ikke direkte til noget DOM-objekt. Den bliver kaldt af React med et **React event object**, når React opdager, at et event af den givne type skete på det objekt.

React's **synthetic events** har et API, der er baseret på den standard API-model, som er defineret i HTML-specifikationen.

### Kodeeksempel

```jsx
import { useState, useRef } from 'react';

export function EventObject() {
  const [counter, setCounter] = useState(0);
  const increment = useRef<HTMLButtonElement>(null);

  function onClick(evt: React.MouseEvent<HTMLButtonElement>): void {
    const delta = evt?.target === increment.current ? 1 : -1;
    setCounter(value => value + delta);
  };

  return (
    <section>
      <h1>Value: {counter}</h1>
      <button ref={increment} onClick={onClick}>Increment</button>
      <button onClick={onClick}>Decrement</button>
    </section>
  );
}
```

Pointen: én og samme handler bruges til begge knapper, og `evt.target` afgør, hvilken knap der blev klikket.

## 8. Event object properties

Event objects har altid:

- En `target`-property
- En `type`-property

Nogle event objects har ekstra properties, der er specifikke for de event-typer, der dispatchede dem.

Et mouse event object har:

- `clientX` (i viewport-koordinater)
- `clientY` (i viewport-koordinater)
- `pageX` (relativt til hele dokumentet)
- `pageY` (relativt til hele dokumentet)
- `ctrlKey`
- `shiftKey`
- `button`
- osv.

## 9. Event phases og propagation

Hvert event i React bobler op gennem alle noderne i document-træet over det.

Vi kan bruge dette trick til at placere vores focus listeners på de to sections og blur listeneren på selve formen — i stedet for på hvert enkelt input-felt.

### Kodeeksempel

```jsx
const FOCUS_NONE = 0;
const FOCUS_USER = 1;
const FOCUS_REQUEST = 2;

const [focus, setFocus] = useState(FOCUS_NONE);
const onUserInfoFocus = () => setFocus(FOCUS_USER);
const onRequestFocus = () => setFocus(FOCUS_REQUEST);
const onBlur = () => setFocus(FOCUS_NONE);
```

```jsx
<form onBlur={onBlur}>
  <h1>Contact</h1>
  <fieldset
    onFocus={onUserInfoFocus}
    style={getStyle(focus === FOCUS_USER)}
  >
    <legend>User</legend>
    <Field label="Name">
      <input />
    </Field>
    <Field label="Email">
      <input type="email" />
    </Field>
  </fieldset>

  <fieldset
    onFocus={onRequestFocus}
    style={getStyle(focus === FOCUS_REQUEST)}
  >
    <legend>Request</legend>
    <Field label="Subject">
      <input />
    </Field>
    <Field label="Body">
      <textarea />
    </Field>
  </fieldset>
</form>
```

Demo: EventPropagation

## 10. Capture og bubbling phases i HTML

Et event i browseren gennemløber tre faser. Eksemplet i slidesene bruger et træ `window → document → html → body → header → nav → button`:

**1. Capture phase**

- a) Capture event dispatched på `window`
- b) Capture event dispatched på `document`
- c) Capture event dispatched på `<html>`-elementet
- d) Capture event dispatched på `<body>`-elementet
- e) Capture event dispatched på `<header>`-elementet
- f) Capture event dispatched på `<nav>`-elementet

**2. Target phase**

- a) Target event (registreret som capture listener) dispatched på `<button>`-elementet
- b) Target event (registreret som bubble listener) dispatched på `<button>`-elementet

**3. Bubbling phase**

- a) Bubble event dispatched på `<nav>`-elementet
- b) Bubble event dispatched på `<header>`-elementet
- c) Bubble event dispatched på `<body>`-elementet
- d) Bubble event dispatched på `<html>`-elementet
- e) Bubble event dispatched på `document`
- f) Bubble event dispatched på `window`

Vil du tilføje en capture listener i JavaScript:

```javascript
element.addEventListener("click", onClick, { capture: true });
```

## 11. Håndtering af event phases i React

I React, som i JavaScript, er default at tilføje events som **bubble listeners**:

```jsx
<main onClick={onClickHandler}>
...
</main>
```

Vil du tilføje en capture event listener, skal du postfixe eventet med `*Capture`:

```jsx
<main onClickCapture={handler1} onClick={handler4}>
  <button onClickCapture={handler2} onClick={handler3} />
</main>
```

Rækkefølgen bliver her handler1 → handler2 → handler3 → handler4. Capture handlers er sjældne!

## 12. Default actions og hvordan man forhindrer dem

Browsere har default actions som konsekvens af nogle events. Det meste af tiden vil du som udvikler gerne have, at disse default actions sker — men nogle gange vil du ikke.

### Kodeeksempel

```jsx
<form>
  <button onClick={onClick}>
    Click me</button>
</form>
```

```jsx
const onClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    console.log("Button was pressed");
  }
```

At trykke på denne knap reloader siden. Det skyldes, at en button inde i en form får formen til at submitte, og når en form submitter, bliver variablerne inde i formen sendt til formens target URL. Dette sker, selv hvis formen ikke har nogen inputs, og selv hvis formen ikke har en eksplicit target URL.

Sådan forhindres default actions: kald `e.preventDefault()` på det event object, der sendes til event handleren.

Demo: DefaultAction

## 13. Event handler generators

Har du mange event handler-funktioner, der kun varierer en smule, kan du generalisere dem til en **event handler generator** — en funktion, der returnerer en handler.

### Kodeeksempel

```jsx
import { useState } from 'react';

export function EventHandlerGenerator() {
  const [counter, setCounter] = useState(0);
  const update = (delta: number) => () => setCounter((c) => c + delta);

  return (
    <>
      <h1>Event handler generator</h1>
      <h3>Value: {counter}</h3>
      <button onClick={update(1)}>Increment</button>
      <button onClick={update(-1)}>Decrement</button>
    </>
  );
}
```

Bemærk at `update(1)` her **skal** kaldes i JSX'en — den returnerer selve handleren. Det er altså ikke pitfall'en fra afsnit 2.

## 14. Lytning til DOM-events (uden for React)

Du får brug for at gå uden om React's event system, når:

- Du vil lytte efter events på `window`- eller `document`-objektet.
- Du vil lytte efter events på HTML-elementer, der ikke er direkte inkluderet inde i React-applikationen. Det kunne fx være `body`, som aldrig kan være inde i din React-applikation, men også bare et element uden for React-applikationens kontrol.
- Du vil lytte efter events på non-DOM-objekter, som en request, socket eller et vilkårligt andet JavaScript-objekt.
- Du vil lytte efter et **enkelt** event på et bestemt objekt, men er ligeglad med mere end én forekomst af eventet.
- Du vil betinget lytte efter et event på et objekt.

## 15. Lyt efter event på window-objektet

### Kodeeksempel

```jsx
import { useState, useEffect } from "react";

function getWindowSize() {
  return `${window.innerWidth}x${window.innerHeight}`;
}

export function WindowSize() {
  const [size, setSize] = useState(getWindowSize());

  useEffect(() => {
    const onResize = () => setSize(getWindowSize());
    console.log('useEffect called')
    window.addEventListener("resize", onResize);
    return () => window.removeEventListener("resize", onResize);
  }, [setSize]);

  return <h1>Window size: {size}</h1>;
}
```

Cleanup-funktionen (`return () => window.removeEventListener(...)`) er væsentlig — uden den ophobes listeners ved hver render.

## 16. References & Links

- React Quickly, Second Edition, kapitel 8, Morten Barklund og Azat Mardan, Manning
- React events:
  - https://react.dev/learn/responding-to-events
  - https://react.dev/reference/react-dom/components/common
- MouseEvent: https://developer.mozilla.org/en-US/docs/Web/API/MouseEvent
