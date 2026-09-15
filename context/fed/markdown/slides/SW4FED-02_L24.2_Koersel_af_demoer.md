# L24 – Kørsel af demoer

## Metadata

- **Lektion:** L24 – How to run demos in L24 and find other demos
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L24/How to run demos in L24 and find other demos.pdf (6 slides)
- **Emner dækket:**
  - Hentning og opsætning af demoer fra Brightspace
  - Hvordan man finder andre demoer via git-branches i VS Code
  - Installation og kørsel af `json-server`
  - Portkonfiguration (4001) i useCallback/Bookable-demoen
  - README.md som kilde til server-kommandoen
  - Kørsel af server og frontend i to separate terminaler

---

## 1. To-do før du kører demoerne

- Download demoen fra **Brightspace**
- Åbn **Visual Studio Code**, og husk **`npm i`**
- Lecture 24 kræver, at man kører **serveren først**
- Se hvordan man finder andre demoer fra GitHub (afsnit 2)
- Alle øvrige demoer i L24 kan køre **uden** server
- Men **useCallback-demoen** kræver også, at `json-server` kører (afsnit 3)
  - Du skal åbne **2 terminaler**: én til serveren, en anden til det normale `npm start`

## 2. Sådan finder du andre demoer fra GitHub

Slide 3 viser fremgangsmåden med et VS Code-screenshot og tre gule callouts.

Demoerne ligger som **git-branches** i det samme repository. Fremgangsmåden er:

1. **Klik** på branch-indikatoren i statuslinjen nederst til venstre i VS Code (den viser den aktuelle branch, på screenshottet `usememo*`).
2. **"It will popup like this"** — VS Code åbner en quick-pick øverst med feltet *"Select a ref to checkout"*.
3. I feltet: **"Type in to find the right demo"** — skriv navnet på den demo, du leder efter, for at filtrere.
4. **"Or / also can see the recent demo from the lists"** — de senest brugte branches står allerede i listen.

Branch-listen på screenshottet viser demoernes navne — nyttigt som oversigt over, hvad der findes:

```
Create new branch…
Create new branch from…
Checkout detached…
passing-shared-state   1c4f365a
usecontext-counter     11e37eeb
usecontext-simple      78593207
usememo                41c31857
usecallback            d8b07f71
master                 d8b07f71
```

Det tilhørende projekt hedder `app-state-demo` og indeholder bl.a. `App.js`, `App.test.js`, `index.css`, `index.js`, `db.json`, `package.json` og `README.md`.

## 3. useCallback – Bookable-demoen kræver json-server

Sådan kører du `json-server`:

### 1) Installér json-server

```bash
npm i -g json-server
```

Terminal-prompten på slidet viser, at kommandoen køres fra projektmappen:

```
FED_Lab_React\app-state-demo\app-state-demo> npm i -g json-server
```

### 2) Kør json-server

```bash
json-server --watch db.json --port 4001
```

```
FED_Lab_React\app-state-demo\app-state-demo> json-server --watch db.json --port 4001
```

### 3) Kontrollér porten

Sørg for, at din port stemmer med **4001** i `UserPicker`, `UsersList` og `BookableList`.

Eksemplet på slidet viser, hvor i koden porten står — en pil peger på `4001` i fetch-URL'en:

```jsx
export default function UserPicker () {
  const [users, setUsers] = useState(null);

  useEffect(() => {

    fetch("http://localhost:4001/users")
```

Hvis serveren kører på en anden port end den, komponenterne fetcher fra, fejler kaldene — det er den klassiske fælde her.

## 4. Tjek README.md og kør serveren i terminalen

Hvis du har brug for at køre en server til appen, viser `README.md` hvordan server-kommandoen køres:

```bash
json-server --watch db.json --port 4001
```

README-filen på slidet indeholder:

```markdown
## Preperation
The usecallback demo requeres a http-server running on port 4001:
You can install a server with:
    npm install -g json-server
And then you can start it in a terminal with:
    json-server --watch db.json --port 4001 --delay 1000
```

Bemærk at README-varianten tilføjer `--delay 1000`, som forsinker svarene med ét sekund. Det er bevidst: forsinkelsen gør loading-tilstande synlige i demoen, så man kan se effekten af den asynkrone datahentning.

Kommandoen køres i terminalen fra projektmappen:

```
\app-state-demo2> json-server --watch db.json --port 4001
```

## 5. Kør frontend som sædvanligt (npm start) i en anden terminal

- Åbn en **ny terminal** i den samme Visual Studio Code-app
- Kør derefter frontend som sædvanligt:

```bash
npm start
```

- Og appen vil fungere sammen med serveren

Pointen er, at begge processer skal køre samtidig: `json-server` blokerer sin terminal, så frontenden skal have sin egen.
