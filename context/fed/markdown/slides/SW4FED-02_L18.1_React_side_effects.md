# L18 – React Hooks: side effects (useEffect)

## Metadata

- **Lektion:** L18 – React side effects
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L18/FED React side effects.pdf (11 slides)
- **Emner dækket:**
  - Hvad en side effect er i React-forstand
  - `useEffect` som erstatning for lifecycle methods
  - Effects uden cleanup
  - Effects med cleanup (unsubscribe, `clearInterval`)
  - Dependency-array og kontrol af hvornår en effect kører
  - Effect kun ved mount (tomt dependency-array)
  - Fordele ved `useEffect` og forskellen til `useLayoutEffect`
  - Flere effects til at adskille concerns

---

## 1. Hvad er en side effect?

React betragter alt, der interagerer med noget andet end den virtuelle DOM, som en **side effect**.

Eksempler:

- At sætte page title imperativt
- At arbejde med timers som `setInterval` eller `setTimeout`
- At måle bredde, højde eller position af elementer i DOM'en
- Manuelt at ændre DOM'en
- At logge beskeder til konsollen eller en anden service
- At sætte eller hente værdier i local storage
- At fetche og sende data, eller at subscribe/unsubscribe til services

## 2. useEffect

React tilbyder `useEffect`-hooket, så vi bedre kan kontrollere side effects og integrere dem i vores komponenters lifecycles.

- `useEffect` tilføjer muligheden for at udføre side effects fra en function component.
- I en class-baseret komponent bruger man i stedet lifecycle methods (som i Angular).
- `useEffect` tjener samme formål som `componentDidMount`, `componentDidUpdate` og `componentWillUnmount` i React-classes — men samlet i ét enkelt API.

Når du kalder `useEffect`, fortæller du React, at den skal køre din "effect"-funktion **efter** ændringer er flushed til DOM'en.

Effects kan valgfrit også specificere, hvordan der skal "ryddes op" efter dem, ved at returnere en funktion.

## 3. Kør side effects efter hver render

Dokumentets title er ikke en del af document body og bliver ikke renderet af React. Men titlen er tilgængelig via `document`-propertyen på `window`:

```javascript
document.title = "Bonjour";
```

At række ud efter et browser-API på denne måde regnes som en side effect. Derfor skal kaldet wrappes i `useEffect`-hooket:

```jsx
useEffect(() => {
  document.title = "Bonjour";
});
```

React kører effect-funktionen inde i `useEffect`-hooket efter **hver** render, når browseren har repainted siden, og opdaterer page title som ønsket.

## 4. Effects Without Cleanup

Network requests, manuelle DOM-mutationer og logging er almindelige eksempler på effects, der ikke kræver cleanup.

### Kodeeksempel

```jsx
import React, { useState, useEffect } from "react";

export default function Counter(props) {
  const [count, setCount] = useState(0);

  useEffect(() => {
    document.title = `You clicked ${count} times`;
  });

  function handleClick() {
    setCount(count + 1);
    console.log('Demo3: ' + count);
  }

  return (
    <div>
      <h2> Demo 3 </h2>
      <p>The title is updated by use of the useEffect hook!</p>
      <button onClick={handleClick}> Increment {count}
      </button>
    </div>
  );
}
```

Bemærk: der er **intet dependency-array**, så effecten kører efter hver render.

## 5. Effects With Cleanup

Når du subscriber til et event, skal du også unsubscribe for at undgå memory leaks.

Hvis din effect returnerer en funktion, kører React den, når det er tid til at rydde op. Hvis `useEffect`-funktionen trigges igen — fx under en rerender — kører cleanup-funktionen **først**.

### Kodeeksempel

```jsx
import React, { useState, useEffect } from "react";

export default function Timer(props) {
  const [time, setTime] = useState(Date());

  useEffect(() => {
    console.log("New Effect");
    const intervalID = setInterval(() => {
      const newTime = Date();
      setTime(newTime);
    }, 1000);
    // Specify how to clean up after this effect:
    return () => clearInterval(intervalID);
  });

  return (
    <div>
      <div> Timer {props.name} </div>
      <div> It is {time} </div>
    </div>
  );
}
```

## 6. useEffect Dependencies — styring af hvornår en effect kører

`useEffect`'s andet argument er et **array af dependencies**. React kører effecten igen, hvis blot én af dem er forskellig fra sidste render.

### Kodeeksempel

```jsx
import React, { useState, useEffect } from 'react';

export default function UserStorage() {
  const [user, setUser] = useState("Sanjiv");

  useEffect(() => {
    const storedUser = window.localStorage.getItem("user");
    if (storedUser) {
      setUser(storedUser);
    }
  }, []);

  useEffect(() => {
    window.localStorage.setItem("user", user);
  }, [user]);

  return (
    <select value={user} onChange={e => setUser(e.target.value)}>
      <option>Jason</option>
      <option>Akiko</option>
      <option>Clarisse</option>
      <option>Sanjiv</option>
    </select>
  );
}
```

Her illustreres begge former: den første effect har tomt dependency-array og kører kun ved mount (læser fra local storage). Den anden har `[user]` og kører hver gang `user` ændres (skriver til local storage).

## 7. Kør en effect kun når en komponent mounter

- `useEffect`'s andet argument er et array af dependencies.
- React kører effecten igen, selv hvis blot én af dem er forskellig.
- Default er, at **alt** er en dependency, så effecten altid kører.
- Sender vi et **tomt array** ind, svarer det til at fortælle React, at effecten kun skal køre allerførste gang.

### Kodeeksempel

```jsx
import React, { useState, useEffect } from "react";

export default function Timer(props) {
  const [time, setTime] = useState(Date());

  useEffect(() => {
    console.log("New Effect");
    const intervalID = setInterval(() => {
      const newTime = Date();
      setTime(newTime);
    }, 1000);
    return () => clearInterval(intervalID);
  }, []);

  return (
    <div>
      <div> Timer {props.name} </div>
      <div> It is {time} </div>
    </div>
  );
}
```

Forskellen fra afsnit 5 er alene det tomme `[]` til sidst: intervallet oprettes én gang ved mount og ryddes op ved unmount, i stedet for at blive genskabt ved hver render.

<!-- Slide 9 i kilden indeholder ingen tekst (formodentlig et diagram eller screenshot). -->

## 8. Fordele ved useEffect

- Effects, der er schedulet med `useEffect`, blokerer ikke browseren fra at opdatere skærmen. Det får app'en til at føles mere responsiv.
- Når du har brug for, at noget sker **synkront**, findes der et separat `useLayoutEffect`-hook med et API identisk med `useEffect`. `useLayoutEffect` køres efter React har opdateret DOM'en, men før browseren repainter.
- React anvender hver effect brugt af komponenten i den rækkefølge, de blev specificeret. Brug **flere effects til at adskille concerns**.

## 9. References & Links

- "React Hooks in Action" af John Larsen
- A Complete Guide to useEffect: https://overreacted.io/a-complete-guide-to-useeffect/
