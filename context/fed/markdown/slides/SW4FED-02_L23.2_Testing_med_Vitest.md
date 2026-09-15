# L23 – FED Testing med Vitest

## Metadata

- **Lektion:** L23 – React Testing With Vitest (A Vite-native testing framework)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L23/FED testing with Vitest.pdf (38 slides)
- **Emner dækket:**
  - Hvorfor Vitest — delt konfiguration med Vite, Jest-kompatibel API
  - Opsætning: install, navngivning af testfiler, npm-scripts, coverage
  - Test suites, test cases og assertions (`describe`, `test`/`it`, `expect`)
  - JSDOM som browser-emulering til test af React-komponenter
  - React Testing Library: `render`, `screen`, `debug`
  - Query-varianter: `getBy*`, `queryBy*`, `findBy*` og `*AllBy*`
  - `fireEvent` til simulering af brugerinteraktion
  - Snapshot testing
  - API mocking med Mock Service Worker (msw), `setupServer` vs. `setupWorker`

---

## 1. Hvorfor Vitest?

Vitest er et Vite-native testing framework.

- Bruger den samme konfiguration som ens app (gennem `vite.config.js`) og deler dermed en fælles transformation pipeline under dev, build og test
- Leverer den samme API som Jest — Jest har været det ledende test framework for JavaScript i nogle år
- Inkluderer de mest almindelige features, der kræves ved opsætning af unit tests: mocking, snapshots, coverage
- Vitest sigter mod at positionere sig som det foretrukne Test Runner-valg for Vite-projekter, og som et solidt alternativ selv for projekter, der ikke bruger Vite

## 2. Tilføj Vitest til dit projekt

Det anbefales, at man installerer en kopi af vitest i sin `package.json` som en development dependency.

### Kodeeksempel

```bash
npm install -D vitest
```

## 3. Opret en test

Testfiler skal indeholde `.test.` eller `.spec.` i deres filnavn.

### Kodeeksempel

```typescript
// sum.ts
export function sum(a: number, b: number): number {
  return a + b
}
```

```typescript
// sum.test.ts
import { expect, test } from 'vitest'
import { sum } from './add'

test('adds 1 + 2 to equal 3', () => {
  expect(sum(1, 2)).toBe(3)
})
```

## 4. Kør testen

For at eksekvere testen tilføjes følgende sektion til `package.json`:

### Kodeeksempel

```json
"scripts": {
  "dev": "vite",
  "build": "tsc -b && vite build",
  "lint": "eslint .",
  "preview": "vite preview",
  "test": "vitest",
  "coverage": "vitest run --coverage"
},
```

Kør alle tests:

```bash
npm run test
```

Vitest starter i watch mode: hver gang man gemmer en fil, køres testene igen. `npm run coverage` genererer en coverage-rapport.

<!-- Slide 6 og 7 viser skærmbilleder af terminal-output fra hhv. npm run test og npm run coverage -->

## 5. IDE Integration

Vitest leverer en officiel extension til Visual Studio Code, der forbedrer testoplevelsen: https://marketplace.visualstudio.com/items?itemName=vitest.explorer

## 6. Hvor skal testfiler placeres?

- `*.test.*` / `*.spec.*`-filerne kan placeres i vilkårlig dybde under `src`-topniveaumappen
- Det anbefales at placere testfilerne ved siden af den kode, de tester, så relative imports bliver kortere
- Nogle vælger dog at placere alle deres tests i en "mirror"-mappe kaldet `__tests__` eller bare `tests`

## 7. Hvad Vitest tilbyder

- En test runner: `vitest`
- Test suites: `describe`
- Test cases: `it` eller `test`
- Assertions: `expect` — inklusive snapshot testing
- Mocking og spies

### Kodeeksempel — en test suite

```javascript
// tests/TestSuite.test.js

import { describe, expect, test, it } from 'vitest'

describe('true is truthy and false is falsy', () => {
  test('true is truthy', () => {
    expect(true).toBe(true);
  });

  it('false is falsy', () => {
    expect(false).toBe(false);
  });
});
```

## 8. Test af React-komponenter — JSDOM

React-komponenter er designet til at køre i et browser-miljø. For at teste dem skal man levere et browser-miljø. For hurtigere tests bruger vi jsdom til at emulere browserens miljø.

Installér jsdom som dev dependency:

```bash
npm install -D jsdom
```

Og inkludér det i Vite-konfigurationsfilen, `vite.config.ts`:

```typescript
/// <reference types="vitest" />
/// <reference types="vite/client" />
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
  },
})
```

## 9. React Testing Library

React Testing Library bygger oven på DOM Testing Library ved at tilføje API'er til at arbejde med React-komponenter. Den er velegnet til unit, integration og end-to-end testing af React-komponenter og -applikationer.

Dens primære ledende princip er: *"The more your tests resemble the way your software is used, the more confidence they can give you."*

Installér React Testing Library sammen med dens peer dependency `testing-library/dom` som dev dependency:

```bash
npm install -D @testing-library/react @testing-library/dom
```

## 10. Initialisering af testmiljøet

Hvis man har brug for global opsætning før testene køres, tilføjes en `src/tests/setup.ts` til projektet:

### Kodeeksempel

```typescript
import { expect, afterEach } from 'vitest';
import { cleanup } from '@testing-library/react';
import * as matchers from "@testing-library/jest-dom/matchers";

// This adds jest-dom's custom assertions
expect.extend(matchers);

afterEach(() => {
  cleanup();
});
```

## 11. Opdatér Vites konfigurationsfil

Inkludér den nye test setup-fil i Vites konfigurationsfil, og gør alle imports fra Vitest globale, så man ikke behøver at foretage disse imports (fx `expect`) manuelt i hver fil.

### Kodeeksempel

```typescript
/// <reference types="vitest" />
/// <reference types="vite/client" />
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: './src/tests/setup.ts',
  },
})
```

Sørg for at TypeScript kender de globale typer. I `tsconfig.app.json` tilføjes:

```json
{
    "compilerOptions": {
      "types": ["vitest/globals"],
      "tsBuildInfoFile": "./node_modules/.tmp/tsconfig.app.tsbuildinfo",
      "target": "ES2020",
      "useDefineForClassFields": true,
      "lib": ["ES2020", "DOM", "DOM.Iterable"],
      "module": "ESNext",
      "skipLibCheck": true,
      // Code missing
```

<!-- Resten af tsconfig-eksemplet er ikke vist på sliden -->

## 12. "Smoke test"

En smoke test verificerer, at en komponent renderer uden at kaste en fejl. Tests som denne giver meget værdi for meget lidt indsats.

### Kodeeksempel

```jsx
import { render } from '@testing-library/react';
import App from './App';

it('renders without crashing', () => {
  render(<App />);
});
```

Anbefaling: start med simple smoke tests for dine komponenter.

## 13. Testing med React Testing Library

RTL's `render`-funktion tager vilkårlig JSX som argument og renderer den som output. Man tilgår outputtet med `screen`. For at overbevise sig selv om, at det er der, kan man bruge RTL's `debug`-funktion — også praktisk, når man skriver sine assertions.

### Kodeeksempel

```jsx
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders Vite dev link', () => {
  render(<App />);
  const linkElement = screen.queryByAltText('Vite logo');
  expect(linkElement).toBeInTheDocument();
  // use screen.debug(); to see what is rendered
});
```

## 14. Selektion af elementer

- `getByText`
- `getByRole` — bruges typisk til at hente elementer via `aria-label`-attributter. Der findes dog også implicitte roles på HTML-elementer — fx `button` for et `button`-element
- `getByLabelText` — `<label for="search" />`
- `getByPlaceholderText` — `<input placeholder="Search" />`
- `getByAltText` — `<img alt="profile" />`
- `getByDisplayValue` — `<input value="JavaScript" />`
- `getByTestId` — `<sometag data-testid="xyz" />`

### Kodeeksempel

```javascript
//screen.getByRole('');
expect(screen.getByRole('textbox'))
    .toBeInTheDocument();
```

## 15. Search variants

- **`getBy*`** — det, man skal bruge i de fleste tilfælde
- **`queryBy*`** — bruges, når man asserterer, at et element *ikke* er der:

```javascript
expect(screen.queryByText(/Searches for JavaScript/)).toBeNull();
```

- **`findBy*`** — bruges til asynkrone elementer, som vil være der på et tidspunkt

### Kodeeksempel — findBy

```jsx
export default function User() {
  const [search, setSearch] = React.useState('');
  const [user, setUser] = React.useState(null);
  React.useEffect(() => {
    const loadUser = async () => {
      const user = await getUser();
      setUser(user);
    };
    loadUser();
  }, []);
  return (
    <div>
      {user ? <p>Signed in as {user.name}</p> : null}

      <Search value={search} onChange={handleChange}>
        Search:
      </Search>

      <p>Searches for {search ? search : '...'}</p>
    </div>
  );
}
```

```jsx
import React from 'react';
import { render, screen } from '@testing-library/react';
import User from './User';

describe('User', () => {
  test('renders User component', async () => {
    render(<User />);
    expect(screen.queryByText(/Signed in as/)).toBeNull();
    expect(await screen.findByText(/Signed in as/)).toBeInTheDocument();
  });
});
```

## 16. Flere elementer

Hvordan asserterer man, hvis der er flere elementer — fx en liste i en React-komponent? Alle search variants kan udvides med ordet `All`:

- `getAllBy*`
- `queryAllBy*`
- `findAllBy*`

## 17. Assertive functions fra React Testing Library

- `toBeDisabled`
- `toBeEnabled`
- `toBeEmpty`
- `toBeEmptyDOMElement`
- `toBeInTheDocument`
- `toBeInvalid`
- `toBeRequired`
- `toBeValid`
- `toBeVisible`
- `toContainElement`
- `toContainHTML`
- `toHaveAttribute`
- `toHaveClass`
- `toHaveFocus`
- `toHaveFormValues`
- `toHaveStyle`
- `toHaveTextContent`
- `toHaveValue`
- `toHaveDisplayValue`
- `toBeChecked`
- `toBePartiallyChecked`
- `toHaveDescription`

## 18. Fire event

Vi kan bruge RTL's `fireEvent`-funktion til at simulere interaktioner fra en slutbruger. Bemærk Arrange/Act/Assert-strukturen.

### Kodeeksempel

```jsx
import { render, screen, fireEvent } from '@testing-library/react';
import User from './User';

test('User component handles change event', async () => {
  // Arrange
  render(<User />);
  // Assert before event
  const inputElement = screen.getByRole('textbox');
  expect(inputElement).toHaveValue('');
  // Act
  fireEvent.change(screen.getByRole('textbox'), {
    target: { value: 'JavaScript' },
  });
  // Assert after event
  expect(await screen.findByRole('textbox')).toHaveValue('JavaScript');
});
```

Der er brug for et `await` for at undgå en warning i konsollen.

## 19. Snapshot testing

Snapshot tests er et meget nyttigt værktøj, når man vil sikre sig, at outputtet af ens funktioner ikke ændrer sig uventet.

- Det er en feature i Vitest/Jest, der lader en teste render-outputtet af sine komponenter på en unik måde
- Ved første kørsel opretter Vitest en snapshot-fil. Vitest laver JSON-outputs for tests og gemmer dem i specielt navngivne mapper. Disse bør tilføjes til version control sammen med al anden kode
- Ved efterfølgende testkørsler sammenligner Vitest/Jest ganske enkelt det renderede output med det tidligere snapshot
- Hvis der findes en forskel, ved man, at testen fejlede og skal justeres — eller at output-snapshottet skal opdateres (tryk blot `u`)

### Kodeeksempel

```jsx
import { render } from '@testing-library/react';
import Content from './Content';

test('Content snapshot', () => {
  const mockPost = {
    content: 'I am learning to test React components'
  };
  const component = render(<Content post={mockPost} />);
  expect(component).toMatchSnapshot();
});
```

Komponenten under test:

```tsx
export default function Content(props: { post: { content: string; }; }) {
    const { post } = props;
    return <p className="content">{post.content}</p>;
};
```

Den genererede snapshot-fil:

```javascript
// Vitest Snapshot v1, https://vitest.dev/guide/snapshot.html

exports[`<Content/> > snapshot 1`] = `
{
  "asFragment": [Function],
  "baseElement": <body>
   <div>
    <p
     class="content"
    >
     I am learning to test React components
    </p>
```

<!-- Snapshot-outputtet er afkortet på sliden -->

## 20. API mocking med MSW

Brug Mock Service Worker (msw) til at mocke et API-request, som vores test-objekt (SUT — System Under Test) foretager.

### Hvordan MSW virker

- MSW bruger Service Worker API'et til at intercepte requests
- Modsat konventionel request client stubbing tillader Service Workeren os at intercepte requests *efter*, at de er dispatchet af applikationen — dvs. efter `window.fetch` er færdig med sit arbejde. Sådanne requests sker faktisk, er observerbare i netværkstrafikken og besvares med mocks på browser-niveau. Applikationen ved end ikke, at der er et mocked API involveret
- Fordi interceptionen sker "højere" i request-kæden, er MSW ligeglad med, hvilke request clients man bruger — om det er native `window.fetch`, axios eller Apollo. Alt resulterer i en netværks-entry, og alt bliver interceptet af Service Workeren uden stubs og patches
- Der er ganske enkelt ikke en bedre, renere måde at intercepte og styre sin HTTP-kommunikation i browseren

### Installation

```bash
npm install -D msw@latest
```

## 21. Worker vs. Server

- **`setupWorker()`** — bruges, hvis ens tests kører i en rigtig browser
- **`setupServer()`** — bruges, hvis ens tests kører i Node. Den fungerer som en bro, der anvender de samme request handlers i Node.js, hvor Service Workers ikke kan køre. Vitest kører i Node, IKKE i en rigtig browser

## 22. Beskriv API'et

Man beskriver netværket ved hjælp af Request handlers:

- `http.get()` for REST API
- `graphql.query()`
- Osv.

En request handler er ansvarlig for at intercepte et request og håndtere dets response, som kan være:

- Et mocked response
- En kombination af det rigtige response og en mock
- Ingenting — hvis man kun vil overvåge trafikken uden at påvirke den

### Kodeeksempel

```javascript
const server = setupServer(
  http.get('/greeting', () => {
     return HttpResponse.json({greeting: 'hello there'})
  }),
)
```

## 23. Integrér msw-mock i Vitest

### Kodeeksempel

```javascript
beforeAll(() => server.listen())
afterEach(() => server.resetHandlers())
afterAll(() => server.close())
```

Placér disse linjer efter `setupServer`, men før test cases.

## 24. Skriv testen — success

```jsx
test('loads and displays greeting', async () => {
  // Arrange
  render(<Fetch url="/greeting" />)

  // Act
  fireEvent.click(screen.getByText('Load Greeting'))

  await screen.findByRole('heading')

  // Assert
  expect(screen.getByRole('heading')).toHaveTextContent('hello there')
  expect(screen.getByRole('button')).toBeDisabled()
})
```

## 25. Skriv testen — fail

Her overskrives handleren for én enkelt test med `server.use`, så endpointet returnerer HTTP 500.

```jsx
test('handles server error', async () => {
  server.use(
    http.get('/greeting', () => {
      return new HttpResponse(null, {status: 500})
    }),
  )

  render(<Fetch url="/greeting" />)

  fireEvent.click(screen.getByText('Load Greeting'))

  await screen.findByRole('alert')

  expect(screen.getByRole('alert')).toHaveTextContent('Oops, failed to fetch!')
  expect(screen.getByRole('button')).not.toBeDisabled()
})
```

## 26. References & Links

- Vitest — https://vitest.dev/
- React Testing Library — https://www.robinwieruch.de/react-testing-library/ og https://testing-library.com/docs/react-testing-library/intro/
- Expect — https://jestjs.io/docs/expect
- Mocking API calls – MSW — https://mswjs.io/docs/getting-started
- Ultimate Guide to Visual Testing with Playwright — https://www.browsercat.com/post/ultimate-guide-visual-testing-playwright
