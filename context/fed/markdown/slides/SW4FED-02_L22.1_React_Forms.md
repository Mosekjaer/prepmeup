# L22 – React Forms

## Metadata

- **Lektion:** L22 – React Forms
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L22/FED React Forms.pdf (18 slides)
- **Emner dækket:**
  - Forms i React — ingen særlige API'er, blot komponenter, state, props og events
  - Controlled components vs. uncontrolled components
  - Event handlers: `onChange`, `onClick`, `onSubmit`
  - Properties på et synthetic event
  - Filtered input og masked input
  - Håndtering af flere inputs med `name`-attributten
  - Form validation
  - React Hook Form
  - MUI (Material-UI)

---

## 1. Forms i React

Der findes **ikke** et særligt sæt API'er til forms.

- Men det er på vej i version 19.
- Og `react-router-dom` har en `Form`-control, man kan bruge nu.

At arbejde med forms i React er blot mere af det, vi har set indtil nu i React: **komponenter**. Man bruger komponenter, state, props og events til at lave forms.

MEN HTML form-elementer fungerer en smule anderledes end andre DOM-elementer i React, fordi form-elementer naturligt holder noget intern state.

De to måder at håndtere dette:

- **Controlled forms**
- **Uncontrolled forms**

## 2. Controlled Components / forms

I HTML holder form-elementer som `<input>`, `<textarea>` og `<select>` typisk deres egen state og opdaterer den baseret på brugerinput.

I React holdes mutable state typisk i komponenternes state-property og opdateres kun med `useState` eller `useReducer`.

Vi kan kombinere de to ved at lade React-state være **"single source of truth"**. Så kontrollerer den React-komponent, der renderer en form, også hvad der sker i den form ved efterfølgende brugerinput.

Et input form-element, hvis værdi styres af React på denne måde, kaldes en **"controlled component"**.

## 3. Controlled form

State holder de aktuelle værdier.

### Kodeeksempel

```jsx
import { useState } from "react";

export function ControlledForm() {
  const initialState = { value: '' };
  const [state, setState] = useState(initialState);

  function handleChange(event) {
    setState({ value: event.target.value });
  }

  function handleSubmit(event) {
    alert('A name was submitted: ' + state.value);
    event.preventDefault();
  }

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Name:
        <input type="text" value={state.value} onChange={handleChange} />
      </label>
      <input type="submit" value="Submit" />
    </form>
  );
}
```

Nøglen er kombinationen `value={state.value}` + `onChange={handleChange}` — uden `onChange` ville feltet være read-only.

## 4. Uncontrolled form

Her bruges en `ref` til at hente form-værdier fra DOM'en.

### Kodeeksempel

```jsx
import { useRef } from "react";

export function UncontrolledForm() {
  const inputRef = useRef();

  function handleSubmit(event) {
    alert('A name was submitted: ' + inputRef.current.value);
    event.preventDefault();
  }

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Name:
        <input type="text" ref={inputRef} />
      </label>
      <input type="submit" value="Submit" />
    </form>
  );
}
```

## 5. Events

Almindelige event handlers:

**onChange**

- Fyres når et input-element ændrer sig.
- Du kan tilgå den nye værdi af form-elementet med `event.target.value`.

**onClick**

- Fyres når et element bliver klikket.

**onSubmit**

- Fyres fra en submit-knap.
- Håndterer du submit-kaldet manuelt, skal du huske `event.preventDefault();`

## 6. Properties og metoder på et synthetic event i React

| Property | Return type |
|---|---|
| `bubbles` | boolean |
| `cancelable` | boolean |
| `currentTarget` | DOMEventTarget |
| `defaultPrevented` | boolean |
| `eventPhase` | number |
| `isTrusted` | boolean |
| `nativeEvent` | DOMEvent |
| `preventDefault()` | |
| `isDefaultPrevented()` | boolean |
| `stopPropagation()` | |
| `isPropagationStopped()` | boolean |
| `target` | DOMEventTarget |
| `timeStamp` | number |
| `type` | string |

## 7. Filtered input

Hvis du ikke opdaterer state-værdien, når data er indtastet, opdateres input-feltet ikke. Det kan du bruge til kun at tillade visse inputs — altså til at filtrere inputtet.

### Kodeeksempel

```jsx
export function HexColor() {
  const [color, setColor] = useState("BADA55");
  const onChange = (evt) =>
    setColor(evt.target.value.replace(/[^0-9a-f]/gi, "").toUpperCase());

  return (
    <form style={{ display: "flex" }}>
      <label>
        Hex color:
        <input value={color} onChange={onChange} style={inputStyle}/>
      </label>
      <span style={outputStyle} />
    </form>
  );
}
```

Regexpet `[^0-9a-f]` fjerner alt, der ikke er et gyldigt hex-ciffer.

## 8. Masked input

Eksempel: ticket numbers er defineret som tre alfanumeriske tegn, efterfulgt af en bindestreg, efterfulgt af tre alfanumeriske tegn — fx `R1S-T2U`.

Vi vil:

- vise tegnene i uppercase, uanset om brugeren indtaster dem sådan
- tilføje en bindestreg efter de første tre tegn
- begrænse inputtet til kun 7 tegn i alt

### Kodeeksempel

```jsx
export function TicketNumber() {
  const [ticketNumber, setTicketNumber] = useState("");

  const onChange = (evt) => {
    const [first = "", second = ""] = evt.target.value
      .replace(/[^0-9a-z]/gi, "")
      .slice(0, 6)
      .match(/.{0,3}/g);
    const value = first.length === 3 ? `${first}-${second}` : first;
    setTicketNumber(value.toUpperCase());
  };

  const isValid = ticketNumber.length === 7;

  return (
    <form style={{ display: "flex" }}>
      <label>
        Ticket number:
        <input
          value={ticketNumber}
          onChange={onChange}
          placeholder="E.g. R1S-T2U"
        />
      </label>
      <span>{isValid ? "✓" : "✗"}</span>
    </form>
  );
}
```

## 9. Handling Multiple Inputs

Når du skal håndtere flere controlled input-elementer, kan du tilføje en `name`-attribut til hvert element og lade handler-funktionen vælge, hvad den skal gøre, baseret på værdien af `event.target.name`.

## 10. Multiple Inputs demo

### Kodeeksempel

```jsx
const initialState = {
  isGoing: true,
  numberOfGuests: 2
};

const [state, setState] = useState(initialState);

function handleInputChange(event) {
  const target = event.target;
  const value = target.type === 'checkbox' ? target.checked : target.value;
  const name = target.name;

  setState(state => {
    return {
      ...state,
      [name]: value
    };
  });
}
```

```jsx
<form onSubmit={handleSubmit}>
  <label>
    Is going:
    <input
      name="isGoing"
      type="checkbox"
      checked={state.isGoing}
      onChange={handleInputChange} />
  </label>
  <br />
  <label>
    Number of guests:
    <input
      name="numberOfGuests"
      type="number"
      value={state.numberOfGuests}
      onChange={handleInputChange} />
  </label>
  <input type="submit" value="Submit" />
</form>
```

To detaljer værd at bemærke: checkboxes bruger `checked` i stedet for `value`, og `[name]: value` er computed property names — nøglen bestemmes ved runtime.

## 11. Form validation

Spørgsmålene man skal stille sig:

- Hvad er datakravene for applikationen?
- Baseret på disse constraints, hvordan kan du hjælpe dine brugere til at levere meningsfulde data?
- Er der måder, hvorpå du kan eliminere inkonsistenser i de data, brugerne leverer?

For at tilføje validation og sanitization til din komponent skal du hooke ind i update-processen. Til det formål skriver du general-purpose validation- og sanitization-funktioner.

## 12. Basic validation

Bemærk: dette eksempel er fra slidesene skrevet som en **class component** — kurset bruger ellers hooks.

### Kodeeksempel

```jsx
class CreatePost extends Component {
    constructor(props) {
        super(props);
        this.state = {
           content: '',
           valid: false,
        };
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handlePostChange = this.handlePostChange.bind(this);
    }

    handlePostChange(event) {
        const content = event.target.value;
        this.setState(() => {
           return {
              content,
              valid: content.length <= 280
           };
        });
    }

    handleSubmit() {
        if (!this.state.valid) {
            return;
        }
        const newPost = {
            content: this.state.content,
        };
        console.log(newPost);
    }
}
```

## 13. React Hook Form

- Performant, fleksible og udvidelige forms med letanvendelig validation.
- React Hook Form er et lillebitte library uden nogen dependencies.
- Det omfavner uncontrolled elements, men kan arbejde med controlled elements.
- React Hook Form har gjort det let at integrere med eksterne UI component libraries.

Installation:

```bash
npm install react-hook-form
```

## 14. React Hook Form demo

### Kodeeksempel

```jsx
import { useForm } from 'react-hook-form';

export function ReactHookFormDemo() {
  const { register, handleSubmit } = useForm();
  const onSubmit = (data) => {
    alert(JSON.stringify(data));
  };

  return (
    <div className="react-hooks-form">
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label htmlFor="firstName">First Name</label>
          <input placeholder="bill" {...register("firstName")} />
        </div>
        <div>
          <label htmlFor="isDeveloper">Is an developer?</label>
          <input
            type="checkbox"
            value="yes"
            {...register("isDeveloper")}
          />
        </div>
        <input type="submit" />
      </form>
    </div>
  );
}
```

`register("firstName")` returnerer et objekt med props (`name`, `onChange`, `ref` m.fl.), som spredes ud på inputtet — deraf `{...register(...)}`.

## 15. MUI (Material-UI)

React-komponenter, der implementerer Googles Material Design.

### Kodeeksempel

```jsx
import React from "react";
import ReactDOM from "react-dom";
import Button from "@mui/material/Button";

function App() {
    return (
        <Button variant="contained"
                color="primary">
            Hello World
        </Button>
    );
}
ReactDOM.render(<App />, document.querySelector("#app"));
```

Demo: https://codesandbox.io/s/4j7m47vlm4

## 16. References & Links

- "React Quickly, Second Edition" af Morten Barklund og Azat Mardan, Manning
- "React Hooks in Action" af John Larsen, Manning
- "React in Action" af Mark Tielens Thomas, Manning
- Forms documentation: https://reactjs.org/docs/forms.html
- Material UI:
  - https://mui.com/
  - Learn React & Material UI - #1 Intro: https://www.youtube.com/watch?v=xm4LX5fJKZ8
  - Learn React & Material UI - #8 Forms (Part 1): https://www.youtube.com/watch?v=L6HC1bqrLRQ
- React Hook Form: https://react-hook-form.com/get-started
- React-router-dom's form: https://reactrouter.com/en/main/components/form
