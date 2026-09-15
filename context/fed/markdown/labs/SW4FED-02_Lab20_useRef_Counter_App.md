# Lab 20 – useRef: Counter App

## Metadata

- **Lektion:** L20 – React hooks (useRef)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Jung Min Kim (Jenny)
- **Kilde:** Brightspace-note "Understand useRef hook" (eksporteret som "Untitled - Copy (3).html")
- **Emner dækket:**
  - Forskellen mellem `useState` og `useRef`
  - Hvornår man bruger hvad
  - Kombination af `useState`, `useEffect` og `useRef`
  - Tælling af renders uden at trigge re-render
  - Midlertidig besked med `setTimeout` i `useEffect`

---

## Understand useRef hook

Create a **Counter App** that helps you understand how `useRef` works differently from `useState`, and when to use each.

(optional) You will also use `useEffect` to handle side effects, like showing a temporary message.

## Create a Counter App that demonstrates how useState causes renders and useRef does not

- Understand the difference between `useState` vs. `useRef`
- When to use what
- Use all 3 hooks you have learned by now:
  - `useState`
  - `useEffect`
  - **`useRef`**

## Create a component called Counter

### What to build in the Counter component

- **State Counter (using `useState`)**
  - Create a button that increments
  - Every time the button is clicked:
    - state value increases
    - a message appears (e.g. "state count was updated"), then this message disappears after 2 seconds
- **Ref Counter (using `useRef`)**
  - Create a second button that increments a counter using **`useRef`**
  - In some way, you show how many times it has been clicked
- **Rendering Track (using `useRef`)**
  - Create two render counters using `useRef`
    - One render counter that increments **without** using `useEffect`
    - Another render counter that increments **with** using `useEffect`
  - Use both `useRef` to track how many times the component has **rendered** (not clicked)
  - These counters increase every time the component re-renders

## Optional

Add a temporary message (e.g. "State count was updated") using `useEffect`, that appears every time (with `setTimeout`) the state counter updates and disappears after 2 seconds.
