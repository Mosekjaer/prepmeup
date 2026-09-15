# L20 – Modules og packages i JavaScript

## Metadata

- **Lektion:** L20 – Modules
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L20/FED Modules.pdf (14 slides)
- **Emner dækket:**
  - Hvorfor moduler: organisering og global namespace pollution
  - ES modules i ES2015 — scripts vs. modules i browseren
  - Multiple named exports og import-former
  - Single default export
  - Dynamic imports (ES2020)
  - JavaScript før ES Modules: funktioner som namespaces
  - Objects som export interface
  - En minimal og en forbedret `require`-implementation
  - CommonJS og problemet med CommonJS i browseren (Browserify/bundlers)

---

## 1. Hvorfor moduler?

Store programmer har brug for en større organisationsenhed end individuelle funktioner. Og på grund af manglen på namespaces har JavaScript brug for en mekanisme, der kan minimere forureningen af det globale namespace.

Moduler deler programmer op i klynger af kode og hjælper med at strukturere store programmer samt minimere pollution af det globale namespace.

## 2. ES modules

ES modules bringer et officielt, standardiseret modulsystem til JavaScript.

## 3. Modules i ES2015

- I ES2015 er modules understøttet i JavaScript.
- ES6-modules gemmes i filer. Der er præcis ét modul per fil og én fil per modul.
- I browsere skelnes mellem scripts og modules:

| | Scripts | Modules |
|---|---|---|
| HTML element | `<script>` | `<script type="module">` |
| Default mode | non-strict | strict |
| Top-level variables are | global | local to module |
| Value of `this` at top level | `window` | `undefined` |
| Executed | synchronously | asynchronously |
| Has import statement | No | Yes |
| Programmatic imports | Yes | Yes |
| File extension | `.js` | `.js` |

Denne feature er implementeret i alle moderne browsere.

## 4. Multiple named exports

### Kodeeksempel — modulet

```javascript
//------ lib.js ------
export const sqrt = Math.sqrt;
export function square(x) {
  return x * x;
}
export function diag(x, y) {
  return sqrt(square(x) + square(y));
}
```

### Kodeeksempel — import af udvalgte items

```javascript
//------ main.js ------
import { square, diag } from 'lib.js';
console.log(square(11)); // 121
console.log(diag(4, 3)); // 5
```

### Kodeeksempel — import af hele modulet

```javascript
//------ myModule.js ------
import * as lib from 'lib.js';
console.log(lib.square(11)); // 121
console.log(lib.diag(4, 3)); // 5
```

I html-filen:

```html
<script type="module" src="~/js/myModule.js"></script>
```

## 5. Single default export

Der kan være ét enkelt default export. Det kan være en funktion, en class, et objekt eller hvad som helst andet.

### Kodeeksempel — objekt

```javascript
//------ lib1.js ------
export default {
  field1: value1,
  field2: value2
};
```

```javascript
//------ main2.js ------
import lib1 from 'lib1.js';
Console.log(lib1.field1);
```

### Kodeeksempel — funktion

```javascript
//------ lib2.js ------
export default function () {
 ···
}
```

```javascript
//------ main2.js ------
import myFunc from 'lib2.js';
myFunc();
```

### Kodeeksempel — class

```javascript
//------ lib3.js ------
export default class {
 ···
}
```

```javascript
//------ main3.js ------
import MyClass from 'lib3.js';
const inst = new MyClass();
```

Bemærk: ved default export vælger den importerende fil selv navnet — der er ingen krøllede parenteser omkring det.

## 6. Dynamic Imports

Importér dependencies kun, når du har brug for dem. Det forbedrer applikationens performance.

### Kodeeksempel

```javascript
if (calculations) {
    const calculator = await import('./calculator.js');
    const result = calculator.add(num1, num2);

    console.log(result);
}
```

Introduceret i ES2020.

## 7. JavaScript før ES Modules — funktioner som namespaces

Funktioner var det eneste i JavaScript, der skabte et nyt scope — før introduktionen af moduler.

### Kodeeksempel

```javascript
var names1 = ["Sunday", "Monday", "Tuesday", "Wednesday",
             "Thursday", "Friday", "Saturday"];
function dayName1(number) {
    return names1[number];
}
console.log(dayName1(1));

var dayName2 = function () {
    var names2 = ["Sunday", "Monday", "Tuesday", "Wednesday",
                 "Thursday", "Friday", "Saturday"];
    return function (number) {
        return names2[number];
    };
}();
console.log(dayName2(3));
```

I den anden variant er `names2` skjult inde i en straks-eksekveret funktion, så den ikke forurener det globale scope.

## 8. Objects som Export Interface

Når vi vil eksportere mere end én funktion, kan vi bruge et objekt til at holde alle de funktioner og variabler, vi vil eksportere.

### Kodeeksempel

```javascript
(function (exports) {
    var names = ["Sunday", "Monday", "Tuesday", "Wednesday",
                 "Thursday", "Friday", "Saturday"];

    exports.name = function (number) {
        return names[number];
    };
    exports.number = function (name) {
        return names.indexOf(name);
    };
})(this.weekDay = {});

console.log(weekDay.name(5));
console.log(weekDay.number("Saturday"));
```

## 9. Frigørelse fra det globale scope

Vi kan lave et system, der lader ét modul direkte bede om interface-objektet fra et andet modul, uden at gå gennem det globale scope.

### Kodeeksempel — en minimal implementation af require

```javascript
function require(name) {
    var code = new Function("exports", getFile(name));
    var exports = {};
    code(exports);
    return exports;
}
```

### Kodeeksempel — weekDay.js

```javascript
var names = ["Sunday", "Monday", "Tuesday", "Wednesday",
             "Thursday", "Friday", "Saturday"];
exports.name = function (number) {
    return names[number];
};

exports.number = function (name) {
    return names.indexOf(name);
};
```

### Kodeeksempel — brugen

```javascript
var weekday = require("weekDay");
console.log(weekday.name(3));
```

## 10. En forbedret require

Denne version tilføjer caching (så et modul kun indlæses én gang) og et `module`-objekt, så et modul kan erstatte hele sit exports-objekt.

### Kodeeksempel

```javascript
function require(name) {
    if (name in require.cache)
        return require.cache[name];

    var code = new Function("exports, module", readFile(name));
    var exports = {}, module = { exports: exports };
    code(exports, module);

    require.cache[name] = module.exports;
    return module.exports;
}
require.cache = Object.create(null);
```

Denne stil af modulsystem kaldes **CommonJS modules**. Det er indbygget i Node.js-systemet.

## 11. CommonJS i browseren

- At læse en fil (et modul) fra nettet er meget langsommere end at læse den fra harddisken.
- Mens et script kører i browseren, kan intet andet ske på den website, det kører på.

For at overvinde problemet med langsom loading af moduler i browseren med CommonJS kan man køre **Browserify** på sin kode, før man serverer den på en webside — http://browserify.org/ — eller bruge et af de andre bundling-værktøjer.

## 12. References & Links

- ES2015 Modules:
  - https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Modules
  - https://hacks.mozilla.org/2018/03/es-modules-a-cartoon-deep-dive/
  - https://hacks.mozilla.org/2015/08/es6-in-depth-modules/
- Eloquent JavaScript: http://eloquentjavascript.net/10_modules.html
- CommonJS / require:
  - http://requirejs.org/docs/commonjs.html
  - http://www.sitepoint.com/understanding-requirejs-for-effective-javascript-module-loading/
