# Lab 18 – Simpel brug af useEffect-hooket

## Metadata

- **Lektion:** L18 – Lab 18
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L18/FED Lab18 - useEffect.pdf (1 side)
- **Emner dækket:**
  - `useEffect`-hooket
  - Tilknytning af event handler til `resize`-eventet på `window`
  - Cleanup af event listener
  - Imperativ opdatering af `document.title` som side effect
  - Breakpoints og responsive størrelseskategorier

---

## 1. Opgave

Lav en app, der opdaterer document title, efterhånden som vinduet resizes. Titlen i browserens tab skal sige **"Small"**, **"Medium"** eller **"Large"** afhængigt af vinduets størrelse.

Når komponenten loader, skal du tilknytte en event handler til `resize`-eventet.

Du kan læse om `resize`-eventet her: https://developer.mozilla.org/en-US/docs/Web/API/Window/resize_event

## 2. Foreslåede breakpoints

De foreslåede breakpoints er **767** og **1199** (pixels bredde):

- Under eller lig 767 → "Small"
- Mellem 768 og 1199 → "Medium"
- 1200 og derover → "Large"

## 3. Vink til løsningen

Opgaven kombinerer to side effects fra forelæsningen: at lytte på et browser-event (subscribe/unsubscribe) og at sætte `document.title` imperativt.

Husk at effecten skal returnere en cleanup-funktion, der fjerner event listeneren igen med `removeEventListener` — ellers ophobes listeners, og du får et memory leak. Et tomt dependency-array (`[]`) sikrer, at listeneren kun tilknyttes ved mount.

<!-- Lab-teksten i kilden indeholder ingen kodeeksempler; ovenstående vink er en sammenfatning af de teknikker, forelæsningen L18 gennemgår. -->
