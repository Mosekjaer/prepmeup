# L19 – React: fetching data med useEffect

## Metadata

- **Lektion:** L19 – React Fetching data
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L19/FED React fetching data.pdf (14 slides)
- **Emner dækket:**
  - Opsætning af `json-server` som lokal mock-backend
  - Fetch af data inde i et `useEffect`-hook
  - Håndtering af den første render, hvor data endnu ikke findes
  - `async`/`await` inde i en effect (og hvorfor effect-callbacken ikke må være async)
  - Helper-funktion til fetch med fejlkontrol
  - State-objekt med `isLoading` og `error`
  - Fetch med axios
  - At trigge data fetching fra input-felt og fra knap
  - Fremtiden: Suspense, React Router loaders
  - Caching-libraries: SWR og React Query

---

## 1. Opsætning af en JSON server

For at sætte en lokal http-server op, der bruger en simpel json-fil som datalager:

Installér npm-pakken `json-server` som globalt værktøj:

```bash
npm install -g json-server
```

Placér datafilen i root-mappen og initialisér den med noget data. Den skal være en gyldig json-fil.

Start serveren:

```bash
json-server --watch db.json --port 4001 --delay 2000
```

Kommandoen skal køres i root-mappen. `db.json` er datafilen, portnummeret vælger du selv, og `--delay` kan tilføjes, så man kan nå at se sin spinner.

## 2. Fetch af data inde i et useEffect-hook

React kalder effect-funktioner **efter** rendering, så data er ikke tilgængeligt ved den første render. Der er to strategier:

- Sæt en tom liste af users som initialværdi, eller
- Returnér alternativt UI — en ny `Spinner`-komponent — for loading-tilstanden.

### Kodeeksempel

```jsx
const [users, setUsers] = useState(null);

useEffect(() => {
  fetch("http://localhost:4001/users")
    .then(resp => resp.json())
    .then(data => setUsers(data));
}, []);   // To avoid an infinite loop!
```

```jsx
if (users === null) {
  return <Spinner />
}
return (
  <select>
    {users.map(u => (
      <option key={u.id}>{u.name}</option>
    ))}
  </select>
);
```

Det tomme dependency-array `[]` er kritisk: uden det ville hver `setUsers` udløse en ny render, som ville køre effecten igen — en uendelig løkke.

## 3. UserPicker — fuld kode

### Kodeeksempel

```jsx
import { useState, useEffect } from "react";
import Spinner from "./ui/Spinner";

export function UserPicker() {
  const [users, setUsers] = useState(null);

  useEffect(() => {
    fetch("http://localhost:4001/users")
      .then(resp => resp.json())
      .then(data => setUsers(data));
  }, []);

  if (users === null) {
    return <Spinner />
  }

  return (
    <select>
      {users.map(u => (
        <option key={u.id}>{u.name}</option>
      ))}
    </select>
  );
}
```

## 4. Brug af async og await i en effect

Effect-callbacks er **synkrone** for at forhindre race conditions. Vil du bruge `await`, skal du lægge den async funktion **inde i** effecten og kalde den derfra.

### Kodeeksempel — med .then

```jsx
const [users, setUsers] = useState(null);

useEffect(() => {
  fetch("http://localhost:4001/users")
    .then(resp => resp.json())
    .then(data => setUsers(data));
}, []);
```

### Kodeeksempel — med async/await

```jsx
useEffect(() => {
  async function getUsers() {
    const resp = await fetch(url);
    const data = await (resp.json());
    setUsers(data);
  }
  getUsers();
}, []);
```

Man må altså ikke skrive `useEffect(async () => {...})` — effecten ville da returnere en Promise i stedet for en cleanup-funktion.

## 5. En helper-funktion til at fetche data

### Kodeeksempel

```javascript
export default function getData (url) {

    return fetch(url)
      .then(resp => {

       if (!resp.ok) {
         throw Error("There was a problem fetching data.");
       }

       return resp.json();
     });
}
```

## 6. Et state-objekt til at håndtere loading- og error-tilstande

For at kunne vise loading-indikatoren og fejlbeskeden tilføjer vi to properties til state'en: `isLoading` og `error`.

### Kodeeksempel — den fulde initial state

```javascript
const initialState = {
  group: "Rooms",
  bookableIndex: 0,
  hasDetails: true,
  bookables: [],
  isLoading: true,
  error: false
}
```

### Kodeeksempel — effecten

```jsx
const [state, setState] = useState(initialState);

useEffect(() => {
  setState({
    ...state,
    isLoading: true,
    error: false,
    bookables: []
  });

  getData("http://localhost:4001/bookables")
    .then(bookables =>
      setState({
        ...state,
        isLoading: false,
        bookables
      })
    )
    .catch(error =>
      setState({
        ...state,
        isLoading: false,
        error,
      })
    );
}, []);
```

Bemærk brugen af spread-operatoren i hver `setState` — objekter i state skal kopieres, ikke muteres.

## 7. Brug axios til at fetche data

### Kodeeksempel

```jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';

export function UseAxios() {
  const [data, setData] = useState({ hits: [] });

  useEffect(() => {
    const fetchData = async () => {
      const result = await axios(
        'https://hn.algolia.com/api/v1/search?query=react',
      );

      setData(result.data);
    };
    fetchData();
  }, []);

  return (
    <ul>
      {data.hits.map(item => (
        <li key={item.objectID}>
          <a href={item.url}>{item.title}</a>
        </li>
      ))}
    </ul>
  );
}
```

## 8. Hvordan trigger man data fetching?

Brug et input-felt til at fortælle API'et, hvilket emne vi er interesserede i. Ved at lægge `query` i dependency-arrayet kører effecten igen, hver gang query'en ændres — altså ved hvert tastetryk.

### Kodeeksempel

```jsx
const [data, setData] = useState({ hits: [] });
const [query, setQuery] = useState('react');

useEffect(() => {
  const fetchData = async () => {
    const result = await axios(
      `http://hn.algolia.com/api/v1/search?query=${query}`,
    );

    setData(result.data);
  };

  fetchData();
}, [query]);

return (
  <Fragment>
    <input
       type="text"
       value={query}
       onChange={event => setQuery(event.target.value)}
    />
    {/* ... */}
  </Fragment>
);
```

## 9. Hvordan trigger man data fetching med en knap?

Løsningen er to state-variabler: `query` følger input-feltet ved hvert tastetryk, mens `search` kun opdateres ved klik på knappen. Kun `search` ligger i dependency-arrayet, så fetch sker først ved klik.

### Kodeeksempel

```jsx
const [data, setData] = useState({ hits: [] });
const [query, setQuery] = useState('react');
const [search, setSearch] = useState('react');

useEffect(() => {
  const fetchData = async () => {
    const result = await axios(
       `http://hn.algolia.com/api/v1/search?query=${search}`,
    );

    setData(result.data);
  };
  fetchData();
}, [search]);

return (
  <Fragment>
    <input
       type="text"
       value={query}
       onChange={event => setQuery(event.target.value)}
    />
    <button type="button" onClick={() => setSearch(query)}>
       Search
    </button>
    {/* ... */}
  </Fragment>
);
```

## 10. Fremtiden

- React Hooks er ikke tiltænkt data fetching i React. I stedet vil en feature kaldet **Suspense** stå for det.
- Suspense lader komponenter "vente" på noget, før de renderer.
- MEN Suspense er først for nylig blevet released med React 18.

Man kan nu bruge **loaders** med React Router: https://reactrouter.com/en/main/start/tutorial#loading-data

## 11. SWR

SWR står for **stale-while-revalidate** — en HTTP cache invalidation-strategi. SWR dækker dig på alle aspekter af hastighed, korrekthed og stabilitet, så du kan bygge bedre oplevelser.

### Kodeeksempel

```jsx
import useSWR from 'swr'

const fetcher = (url) => fetch(url).then((res) => res.json());

function Profile() {
  const { data, error, isLoading } = useSWR('/api/user', fetcher)

  if (error) return <div>failed to load</div>
  if (isLoading) return <div>loading...</div>
  return <div>hello {data.name}!</div>
}
```

Eksempel: https://swr.vercel.app/examples/basic

## 12. React Query

- React Query kommer med indbygget query caching.
- React Query håndterer også state management og indbygget error-handling af queries.

### Kodeeksempel

```jsx
const retrievePosts = async () => {
  const response = await axios.get(
    "https://jsonplaceholder.typicode.com/posts",
  );
  return response.data;
};

const DisplayPosts = () => {
  const {
    data: posts,
    error,
    isLoading,
  } = useQuery("postsData", retrievePosts);

  if (isLoading) return <div>Fetching posts...</div>;
  if (error) return <div>An error occurred: {error.message}</div>;

  return (
    <ul>
      {posts.map((post) => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
};
```

## 13. References & Links

- "React Hooks in Action" af John Larsen
- How to fetch data in React with Hooks: https://www.robinwieruch.de/react-hooks-fetch-data
- How to post data: https://jasonwatmore.com/post/2020/02/01/react-fetch-http-post-request-examples
- Axios: https://axios-http.com/docs/intro
- React + Axios - Interceptor to Set Auth Header for API Requests if User Logged In: https://jasonwatmore.com/post/2021/09/25/react-axios-interceptor-to-set-auth-header-for-api-requests-if-user-logged-in
- Loaders with React Router: https://reactrouter.com/en/main/start/tutorial#loading-data
- Caching:
  - SWR: https://swr.vercel.app/
  - React Query: https://refine.dev/blog/react-query-guide/
