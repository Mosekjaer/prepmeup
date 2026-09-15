# npm – Node Package Manager

## Metadata

- **Lektion:** L15 – npm (Node Package Manager)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L15/FED npm.pdf (12 slides)
- **Emner dækket:**
  - Hvad et JavaScript package er
  - Package-infrastruktur og NPM som service plus program
  - Hvad Node.js er
  - Installation af Node.js og npm samt verifikation af versioner
  - `npm install`, lokalt og globalt
  - `package.json` og `npm init`
  - Tilføjelse af moduler og dependencies-listen
  - `.gitignore` og `node_modules`
  - Alternative package managers: Yarn og pnpm

---

## 1. Hvad er et JavaScript package?

Et package i JavaScript er en klump kode, der kan distribueres — altså kopieres og installeres. Det kan indeholde et eller flere modules og har information om, hvilke andre packages det afhænger af. Et package kommer sædvanligvis med dokumentation, der forklarer, hvad det gør, så folk der ikke har skrevet det, alligevel kan bruge det.

## 2. Package infrastructure

Vi har brug for et sted at gemme og finde packages, og en bekvem måde at installere og opgradere dem på.

I JavaScript-verdenen leveres denne infrastruktur af Node Package Manager, NPM (https://npmjs.org).

## 3. NPM

NPM er to ting:

- En online service, hvor man kan downloade (og uploade) packages.
- Et program (bundlet med Node.js), der hjælper med at installere og administrere dem.

Der er mere end to millioner forskellige packages tilgængelige på NPM.

## 4. Hvad er Node.js?

Node.js er et open source, cross-platform runtime environment til udvikling af serverapplikationer. Node.js-applikationer skrives i JavaScript.

Node.js leverer en event-driven arkitektur og et non-blocking I/O API designet til at optimere en applikations throughput og skalerbarhed til real-time webapplikationer.

Node.js indeholder et indbygget bibliotek, der lader applikationer fungere som webserver uden software som Apache HTTP Server, Nginx eller IIS. Nginx bruges dog ofte som proxyserver foran Node.

## 5. Installing Node.js and npm

På Windows og Mac downloader man en installer fra Node.js-websitet: https://nodejs.org/en/

Slidet noterer, at man måske allerede har node og npm på sit system.

På Linux findes komplette instruktioner til installation her: https://nodejs.org/en/download/package-manager/

## 6. Verifying installation

Når Node og npm er installeret, kan man tjekke de versioner, man har, med et par terminalkommandoer:

```bash
node --version
npm --version
```

## 7. Use of npm

npm-moduler hentes over internettet fra det offentlige package registry, der vedligeholdes på http://npmjs.org. Moduler installeres gennem `npm install`:

```bash
npm install moduleName
```

For at installere et modul eller værktøj globalt bruges `–g` — i en shell som administrator (sudo):

```bash
npm install moduleName –g
```

## 8. package.json

I enhver JavaScript-applikation bør der ligge en fil i applikationens rodmappe kaldet `package.json`. Den indeholder metadata om projektet og refererer de packages, som projektet afhænger af.

Brug kommandoen `npm init` til at oprette en `package.json`-fil til din applikation. Kommandoen prompter dig for en række ting såsom navn og version på din applikation.

```json
{
    "name": "mean",
    "version": "0.1.0",
    "description": "Demo project",
    "main": "index.js",
    "scripts": {
      "test": "test”
    },
    "author": "Poul Ejnar",
    "license": "ISC"
}
```

<!-- uklart i kilden: slidet viser et typografisk højre-anførselstegn efter "test", hvilket ikke er gyldig JSON -->

## 9. To Add a Module

For at tilføje Express-modulet til et Node-projekt:

```bash
npm install express
```

Node-moduler installeres og tilføjes til dependencies-listen i `package.json`-filen:

```json
{
    "name": "mean",
    "version": "0.1.0",
    "description": "Demo project",
    "main": "index.js",
    "scripts": {
      "test": "test"
    },
    "author": "Poul Ejnar",
    "license": "ISC",
    "dependencies": {
      "express": "^4.13.3"
}
```

Slidet annoterer `^4.13.3` med bemærkningen "Use newest patch" — caret-notationen tillader altså opdateringer inden for samme major-version.

<!-- uklart i kilden: de afsluttende krøllede parenteser for dependencies-objektet og rodobjektet mangler i slidet -->

## 10. .gitignore

Node-moduler installeres i mappen `node_modules`. Den mappe skal IKKE vedligeholdes af git.

Slidet anbefaler at bruge `.gitignore`-filen fra Facebook: https://github.com/facebook/react/blob/main/.gitignore

```bash
.DS_STORE
node_modules
scripts/flow/*/.flowconfig
.flowconfig
*~
*.pyc
.grunt
_SpecRunner.html
__benchmarks__
build/
remote-repo/
coverage/
.module-cache
# more stuff
```

## 11. Alternative package managers

**Yarn** — safe, stable, reproducible projects. Yarn garanterer, at en installation, der virker nu, fortsat vil virke på samme måde i fremtiden. https://yarnpkg.com/

**Pnpm** — pnpm bruger et content-addressable filsystem til at gemme alle filer fra alle module-mapper på disken. Alle filerne gemmes ét enkelt sted på disken. Når packages installeres, hard-linkes deres filer fra dette ene sted uden at forbruge yderligere diskplads. Man sparer gigabytes af plads på disken og får langt hurtigere installationer. https://pnpm.io/
