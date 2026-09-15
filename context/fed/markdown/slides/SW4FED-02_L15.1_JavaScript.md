# L15 – JavaScript (The Basics)

## Metadata

- **Lektion:** L15 – JavaScript, The Basics
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L15/FED JavaScript.pdf (53 slides)
- **Emner dækket:**
  - Hvorfor JavaScript, karakteristika og historie
  - Values, types og `typeof`
  - `number`, `string`, `boolean`
  - Expressions, statements og variabler
  - Garbage collection og environment
  - Operatorer, comparison rules, `||`, `??`, `?.`
  - Typerne `function` og `object`
  - Control flow: `if`, `?:`, `switch`, `while`, `do while`, `for`, `for in`, `break`, `continue`
  - JavaScript på en webside: inline, `<script>`, ekstern fil, `defer`
  - `window.open`, `prompt`, accessibility, debugging og unit test

---

## 1. Agenda

- Introduction
- Values, Variables and Types
- Control flow
- JavaScript On a Web Page

## 2. Hvorfor JavaScript?

JavaScript er webbens programmeringssprog. Alle webbrowsere har en JavaScript-interpreter, der kan eksekvere JavaScript.

JavaScript bruges til:

- At skabe dynamiske websider.
- At validere user input i browseren.
- Ajax — sende data til og hente data fra serveren asynkront.
- At skabe webapplikationer.

## 3. Karakteristika

JavaScript er et **multi-paradigm** sprog, der understøtter imperative, object-oriented og functional programming styles. Det er et dynamisk sprog, det er weakly typed og understøtter duck typing — typer er associeret med værdier, ikke med variabler.

De centrale designprincipper i JavaScript er hentet fra:

- **Self** — prototype-based approach to objects
- **Scheme** — functional programming
- **C** — imperative programming og syntaks
- **Java** — navne og navngivningskonventioner
- **Perl** — regular expressions

## 4. Historie

- JavaScript blev oprindeligt udviklet hos Netscape af Brendan Eich, som blev hjemme i to uger for at omskrive Mocha til den kodebase, der blev kendt som SpiderMonkey — den første JavaScript-engine.
- Netscape ville have et letvægts, fortolket sprog, som kunne komplementere Java ved at appellere til ikke-professionelle programmører, ligesom Microsofts VB.
- Første udgivelse med Netscape Navigator 2.0 i 1995.
- Microsoft introducerede JavaScript-understøttelse (JScript) i Internet Explorer 3.0 i 1996.
- Netscape indsendte JavaScript til Ecma International til standardisering i 1996 — og det officielle navn er nu **ECMAScript**.

## 5. Versioner

| År | JavaScript | JScript | ECMAScript |
| --- | --- | --- | --- |
| 1995 | 1.0 | | |
| 1996 | 1.1 | 1.0 | |
| 1997 | 1.2 | 2.0 | 1.0 |
| 1998 | 1.3 | 3.0 | 2.0 |
| 1999 | 1.4 | 4.0, 5.0 | |
| 2000 | 1.5 | 5.5 | 3.0 |
| 2005 | 1.6 | | |
| 2006 | 1.7 | | |
| 2008 | 1.8 | | |
| 2009 | | | 5.0 |
| 2011 | | | 5.1 |
| 2015 | 2.0 | | 6.0 (aka ES2015) |
| 2016 | | | ES2016 |
| 2017 | | | ES20xy (ny hvert år) |

## 6. Values

I JavaScript er data opdelt i ting kaldet **values**. Nogle mulige værdier:

```javascript
27
3.1415927
"Hello World"
true
```

Enhver værdi har en type.

## 7. Types

Der er 7 typer af værdier:

- `number` — primitiv type, value type-semantik
- `string` — primitiv type, value type-semantik
- `boolean` — primitiv type, value type-semantik
- `Symbol` (ny i ECMAScript 6) — primitiv type
- `object` — reference type-semantik
- `function` — reference type-semantik
- `Undefined` — ikke rigtig en type, det er et specielt ord ligesom `null`

Bemærk: der er ingen `char`-type, og der er kun én number-type.

`typeof`-operatoren producerer en string-værdi, der navngiver typen af den værdi, man giver den.

```javascript
typeof 4.5
```

Reference: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Data_structures

## 8. number

Numbers i JavaScript har 64 bits (IEEE-754 Double).

- 144 lagres som `0100000001100010000000000000000000000000000000000000000000000000`
- `3.1415927` er også af typen `number`.
- 1 bit bruges til fortegnet og 11 bruges til at gemme positionen af decimalpunktet i tallet. Det efterlader 52 bits.
- Ethvert heltal mindre end 2^52 (som er mere end 10^15) passer sikkert i et JavaScript number.
- Beregninger med heltal, der passer i 52 bits, er garanteret altid præcise. Beregninger med brøktal er generelt ikke præcise (men normalt gode nok).

### Kodeeksempel

```javascript
alert(0.94 - 0.01); // displays 0.9299999999999999
```

Brug metoden `toFixed()` til at afrunde tal, når de formateres til output.

```javascript
alert((0.94 - 0.01).toFixed(2)); // displays 0.93
```

JavaScript har de normale aritmetiske operationer, for eksempel:

```javascript
115 * 4 - 4 + 88 / 2
```

## 9. string

Strings skrives ved at omslutte deres tegn med anførselstegn.

```javascript
"Patch my boat with chewing gum."
```

`\` bruges som escape-tegn, som vi er vant til. `+`-operatoren kan bruges til at lime to strings sammen.

```javascript
"con" + "cat" + "e" + "nate"
```

Strings er lavet af unicode-tegn, og strings er **immutable**.

## 10. boolean

Har de sædvanlige to værdier: `true` og `false`.

```javascript
3>2
```

producerer værdien `true`.

## 11. Expressions

Et stykke kode, der producerer en værdi, kaldes et **expression**. Enhver værdi, der skrives direkte, er et expression.

```javascript
"Patch my boat with chewing gum."
42
2.998e8
2 * 10 + 7
33 % 5
typeof(2.4)
```

## 12. Statements

Et program bygges som en liste af statements. De fleste statements slutter med et semikolon `;`. Den simpleste slags statement er et expression med et semikolon efter.

```javascript
42;
2 * 10 + 7;
!false;
```

I nogle tilfælde tillader JavaScript, at man udelader semikolonnet i slutningen af et statement. I andre tilfælde skal det være der, ellers sker der mærkelige ting. Reglerne for, hvornår det sikkert kan udelades, er komplekse og sære — så udelad ikke nogen semikoloner! (Der er faktisk kun 2 tilfælde, hvor et semikolon er nødvendigt, så nogle programmører bruger slet ikke semikoloner.)

## 13. Variables

Ordet `var` eller `let` bruges til at oprette en ny variabel. Efter `var` følger variablens navn.

### Kodeeksempel

```javascript
var caught = 5 * 5;
let name = "John";
```

- Variablen selv er typeløs og er i stand til at holde enhver type værdi (værdien har en type — ikke variablen).
- Variabelnavne kan være næsten ethvert ord, men de må ikke indeholde mellemrum.
- Cifre kan være del af variabelnavne — `catch22` er et gyldigt navn — men navnet må ikke starte med et ciffer.
- Tegnene `$` og `_` kan bruges i navne, som var de bogstaver, så `$_$` er et gyldigt variabelnavn.
- Et variabelnavn kan bruges som et expression.

## 14. Variabler er referencer til en værdi

### Kodeeksempel

```javascript
let myRef;
console.log(typeof myRef);            // prints undefined
myRef = 22;
console.log(typeof myRef);            // prints number
myRef = "Hello";
console.log(typeof myRef);            // prints string
myRef = 5 > 3;
console.log(typeof myRef);            // prints boolean
```

`typeof` giver dig typen af den værdi, variablen holder fast i på det pågældende tidspunkt.

## 15. Garbage Collection

JavaScript har garbage collection. Al lagerplads optaget af værdier og variabler genindvindes automatisk, når de ikke længere refereres. Hvordan dette virker (GC-algoritmen) er ikke en del af standarden, men afhænger af implementationen i JavaScript-engine'n.

## 16. Operators

JavaScript har de sædvanlige operatorer.

**Binary arithmetic operators**

- `+` Addition
- `-` Subtraction
- `*` Multiplication
- `/` Division
- `%` Modulus

**Comparison**

- `==` Equal
- `!=` Not equal
- `>` Greater than
- `>=` Greater than or equal to
- `<` Less than
- `<=` Less than or equal to
- `===` Identical (equal og af samme type)
- `!==` Not identical

De virker generelt som forventet, men nogle gange er automatisk type coercion roden til overraskende adfærd. Og `||` og `&&` virker anderledes end i et typisk C-afledt sprog — undtagen når de bruges på boolean-værdier.

## 17. Comparison Rules

- Tallet `0`, `NaN`, den tomme string `""`, `null`, `undefined` og selvfølgelig `false` tæller alle som false.
- `null == undefined` producerer `true`.
- `NaN == NaN` er `false`! Brug funktionen `isNaN()`.
- Når man sammenligner værdier, der har forskellige typer, forsøger JavaScript at konvertere den ene værdi til den andens type — men når `null` eller `undefined` optræder, giver det kun `true`, hvis begge sider er `null` eller `undefined`.

### Kodeeksempel

Disse printer alle `true`:

```javascript
log(null == undefined);
log(false == 0);
log("" == 0);
log("5" == 5);
```

Disse printer alle `false`:

```javascript
log(null === undefined);
log(false === 0);
log("" === 0);
log("5" === 5);
```

## 18. Operator ||

På booleans virker den, som vi er vant til.

```javascript
var a = false;
let b = true;
console.log(a || b); // normal boolean OR -> prints true
```

Ellers: hvis `a` er true, returneres `a`, ellers returneres `b`.

```javascript
let a = null;
let b = "b";
console.log(a || b); // if a is true, return a, otherwise return b
```

Typisk brug af `||`-operatoren:

```javascript
let input = prompt("What is your name?");
alert("Well hello " + (input || "dear"));
```

## 19. Nullish Coalescing Operator ??

"The nullish coalescing operator (??) is a logical operator that returns its right-hand side operand when its left-hand side operand is null or undefined, and otherwise returns its left-hand side operand." — Kilde: MDN

### Kodeeksempel

```javascript
let score = 0;
let pass = score || 60;

console.log(pass);
// Prints 60
```

```javascript
let score = 0;
let pass = score ?? 60;

console.log(pass);
// Prints 0
```

```javascript
let score;
let pass = score ?? 60;

console.log(pass);
// Prints 60
```

## 20. Optional Chaining Operator ?.

"Shorter and simpler expressions when accessing chained properties when the possibility exists that a reference may be missing." — Kilde: MDN

Hvis propertyen eksisterer, returnerer operatoren dens værdi. Hvis propertyen ikke eksisterer, returnerer operatoren `undefined`.

## 21. Environment

Samlingen af variabler og deres værdier, der eksisterer på et givet tidspunkt, kaldes **environment**.

- Når et program starter op, er dette environment ikke tomt — det indeholder altid en række standardvariabler.
- Når din browser loader en side, skaber den et nyt environment og knytter disse standardværdier til det.
- De variabler, der oprettes og modificeres af programmer på den side, overlever, indtil browseren går til en ny side.
- Det er muligt at give næsten enhver variabel i environment'et en ny værdi. Det kan være nyttigt, men også farligt!

## 22. Typen 'function'

En funktion er et stykke program pakket ind i en værdi. I et browser-environment holder variablen `alert` en funktion, der viser et lille dialogvindue med en besked.

```javascript
alert("Your hair is on fire!");
```

Ethvert expression, der producerer en function-værdi, kan kaldes ved at sætte parenteser efter det.

```javascript
prompt("Tell us everything you know.", "...");
```

## 23. Konvertering af string til number

Funktionen `Number` konverterer en værdi til et tal.

### Kodeeksempel

```javascript
let theNumber = Number(prompt("Pick a number", ""));
console.log("Your number is the square root of " +
      (theNumber * theNumber));
```

Der findes tilsvarende funktioner kaldet `String` og `Boolean`, som konverterer værdier til de typer.

## 24. Typen object

- Objekter er entiteter, der har en identitet (de er kun lig med sig selv), og som mapper property-navne til værdier.
- Objekter er en samling af (key, value)-par (en dictionary), hvor key'en er af typen string, og værdien kan være enhver type.
- Objekter har en **prototype chain**.

## 25. Hvordan opretter man objekter?

Man kan oprette objekter på to forskellige måder.

**1. Object literal**

```javascript
var myObject = {member1: 'value 1', 'my number': 27};
console.log(myObject.member1);
console.log(myObject['my number']);

var b = {};
b.x = 42;
console.log(b.x);
```

**2. Constructor** — via en constructor function

```javascript
var anObject = new Object();
```

## 26. Reserved Words

Keywords som `var` og `while` samt en række ord, der er "reserved for future use", kaldes **reserved words**. Et reserved word kan ikke bruges som:

- Et navn i literal object notation
- Et member-navn i dot notation
- Et function argument
- En `var`
- En global variabel
- Et statement label

Listen af reserverede ord:

```text
abstract
boolean break byte
case catch char class const continue
debugger default delete do double
else enum export extends
false final finally float for function
goto
if implements import in instanceof int interface
long
native new null
package private protected public
return
short static super switch synchronized
this throw throws transient true try typeof
var volatile void
while with
```

## 27. if

Har den sædvanlige syntaks og adfærd.

### Kodeeksempel

```javascript
var day = 1;
if (day > 7)
  day = 1;
```

```javascript
var day = 1;
if (day > 7)
  day = 1;
else
  day++;
```

## 28. Conditional Operator ?

Ligner `if`-statementet.

### Kodeeksempel

```javascript
result = condition ? expression : alternative;
```

```javascript
var a = 6;
var x = 5;
var res = (x > 7) ? a : 5 * a;
console.log(res);
```

## 29. switch

Har den sædvanlige syntaks og adfærd — bortset fra at man også kan switche på strings.

### Kodeeksempel

```javascript
switch (day) {
   case 1:
     console.log ("Monday");
     break;
   case 2:
     console.log ("Thusday");
     break;
   default:
     console.log ("Wow");
     break;
 }
```

## 30. while

Har den sædvanlige syntaks og adfærd. Tuborgklammer (`{` og `}`) bruges til at gruppere statements i blokke.

### Kodeeksempel

```javascript
var currentNumber = 0;
while (currentNumber <= 12) {
  console.log(currentNumber);
  currentNumber = currentNumber + 2;
}
```

```javascript
var txt = "false";
var i = 0;
while (txt) { //any non empty string evaluates to true
  console.log(i);
  i += 1;
  if (i > 5)
    txt = "";
  }
```

## 31. do while

Har den sædvanlige syntaks og adfærd.

### Kodeeksempel

```javascript
var i = 1;
do {
  console.log(i);
  i += 2;
} while (i <= 7)
```

## 32. for

Har den sædvanlige syntaks og adfærd.

### Kodeeksempel

```javascript
for (var i = 0; i <= 5; i++)
  console.log(i);
```

```javascript
for (var i = 0; i <= 10; i = i + 2) {
  console.log(i);
  }
```

```javascript
for (var counter = 0; counter < 20; counter++) {
  if (counter % 4 == 0)
    console.log(counter);
  else
    console.log("(" + counter + ")");
}
```

## 33. for in

Itererer gennem alle enumerable properties på et objekt.

### Kodeeksempel

```javascript
for (var property_name in some_object) {
   //statements using some_object[property_name];
 }

var myObject = {member1: 'value 1', 'my number': 27};

for (var prop in myObject) {
   console.log(myObject[prop]);
}

console.log("++++++++"); // window ~ this ~ this.window
for (var p in window)
  console.log(p);
```

## 34. break

Har den sædvanlige syntaks og adfærd.

### Kodeeksempel

```javascript
for (var current = 30; ; current++) {
  if (current % 7 == 0)
    break;
}
console.log(current);
```

## 35. continue

Springer til næste iteration af løkken.

### Kodeeksempel

```javascript
for (var i = 0; i < 10; i++) {
  if (i % 3 != 0)
    continue;
  console.log (i, " is divisible by three.");
}
```

## 36. JavaScript på en webside

JavaScript-statements kan kodes på en webside ved hjælp af tre forskellige teknikker:

- Placér JavaScript-kode som en del af et HTML-element.
- Placér JavaScript-kode mellem `<script>`-tags.
- Placér JavaScript-kode i en separat fil og tilføj en reference i `src`-attributten på et `<script>`-tag.

## 37. JavaScript-kode som del af et HTML-element

HTML-elementer har event-attributter, der tager JavaScript-kode som deres værdi.

### Kodeeksempel

```html
<button onclick="alert('Boom!');">
DO NOT PRESS
</button>
```

Her er `alert('Boom!');` en eventhandler skrevet i JavaScript. Det virker, men er dårlig stil — det blander presentation med logic.

## 38. JavaScript: Using The script Element

`script`-elementet er et container tag og kan placeres i enten head- eller body-sektionen af en webside. Det anbefales at placere det som det sidste statement før det lukkende body-tag.

### Kodeeksempel

```html
<body>
  . . .
  <script>
    alert("Welcome to Our Site");
  </script>
</body>
```

## 39. JavaScript i en separat fil

Brug `src`-attributten til at referere en ekstern JavaScript-fil.

```html
<script src="url" ></script>
```

- Indlæsningen og behandlingen af siden pauser, mens browseren henter og eksekverer filen.
- Indholdet mellem `<script src="url">` og `</script>` bør være tomt.
- `defer`-attributten fortæller browseren, at den ikke skal vente på scriptet. I stedet fortsætter browseren med at behandle HTML'en og bygge DOM'en. Scriptet loades "i baggrunden" og kører derefter, når DOM'en er fuldt bygget. Ved flere scripts kører de i samme rækkefølge, som de blev angivet.

```html
<script defer src="url" ></script>
<script defer src="url2" ></script>
```

Dette er mere effektivt.

## 40. JavaScript på en webside — environment og sandbox

JavaScript arbejder med de objekter, der er associeret med en webside, ved brug af environment-variabler som `window` og `document`.

Det eksekverer i en **sandbox** — browsere begrænser i høj grad de ting, et JavaScript-program må gøre. Det kan ikke modificere noget, der ikke er relateret til den webside, det er indlejret i.

## 41. Almindelige anvendelser af JavaScript

- Vise en message box
- Redigere og validere form-information
- Modal dialogs
- Beregninger
- Animationer
- AJAX-kald
- Indsætte, modificere eller slette DOM-elementer

## 42. The open Method

`open` tager en URL som argument og åbner et nyt vindue, der viser den URL.

### Kodeeksempel

```javascript
var perry = window.open("http://www.pbfcomics.com/257/");
```

Fordi `open` er en metode på `window`-objektet, kan `window.`-delen udelades.

```javascript
var perry = open("http://www.pbfcomics.com/257/");
```

Et åbnet vindue kan lukkes med dets `close`-metode.

```javascript
perry.close();
```

Værdien returneret af `window.open` er et nyt window. Dette er det globale objekt for det script, der kører i det vindue, og indeholder alle standardtingene som `Object`-constructoren og `Math`-objektet. Men hvis man prøver at kigge på dem, vil de fleste browsere (formentlig) ikke lade én.

Undtagelsen fra denne regel er sider åbnet på samme domæne. Når et script, der kører på en side fra `myDomain.net`, åbner en anden side på samme domæne, kan det gøre alt, hvad det vil, med den side.

## 43. Prompts

`prompt()`-metoden viser en besked og accepterer en værdi fra brugeren.

### Kodeeksempel

```javascript
myName = prompt("prompt message");
```

Værdien indtastet af brugeren gemmes i variablen `myName`.

## 44. JavaScript og Accessibility

Forvent ikke, at JavaScript altid fungerer for enhver besøgende. Nogle kan have JavaScript deaktiveret, og nogle kan være fysisk ude af stand til at klikke med en mus.

Sørg for en måde, hvorpå dit site kan bruges, hvis JavaScript ikke fungerer — for eksempel plain text links og e-mail-kontaktinfo.

## 45. JavaScript Debugging

- Tjek syntaksen i statements — vær meget opmærksom på store og små bogstaver, mellemrum og anførselstegn.
- Verificér at du har gemt siden med dine seneste ændringer.
- Verificér at du tester den nyeste version af siden (refresh eller reload siden).
- Hvis du får en fejlbesked, så brug de fejlbeskeder, browseren viser (tryk F12 / inspect — debug mode).
- Brug direktivet `"use strict";`
- Brug ESLint — https://eslint.org/

## 46. Hvordan unit tester man JavaScript-kode?

- **Mocha** (med Chai som assertion library) og **Jasmine** er to populære testframeworks til JavaScript.
- **Jest** er et nyt populært testframework til unit tests — https://jestjs.io/. Testing with Jest: from zero to hero — https://blog.logrocket.com/testing-with-jest-from-zero-to-hero-85ce0e9cc953
- **Selenium** er en web driver, der ofte bruges til integrationstest — den bruger en browser til rent faktisk at rendere og loade siden, simulere brugerinteraktioner og tjekke resultatet.

## 47. References and Links

- Eloquent JavaScript af Marijn Haverbeke — http://eloquentjavascript.net
- The Modern JavaScript Tutorial — https://javascript.info/
- The JavaScript guru: Douglas Crockfords blog — http://www.crockford.com/javascript/ og http://javascript.crockford.com/survey.html
- Reference — https://developer.mozilla.org/en-US/docs/JavaScript/Reference
- ECMA-262 ECMAScript Language Specification — http://www.ecma-international.org/memento/TC39-M.htm
- http://en.wikipedia.org/wiki/JavaScript
- http://en.wikipedia.org/wiki/JavaScript_syntax
- http://en.wikipedia.org/wiki/ECMAScript

**JavaScript free Books**

- Learning JavaScript Design Patterns — http://www.addyosmani.com/resources/essentialjsdesignpatterns/book/
- Speaking JavaScript - An In-Depth Guide for Programmers — http://speakingjs.com/es5/index.html
- JavaScript Allongé — https://leanpub.com/javascript-allonge/read
- JavaScript Spessore — https://leanpub.com/javascript-spessore/read

**Style guides (how to write JavaScript)**

- Idiomatic Style Manifesto — https://github.com/rwaldron/idiomatic.js
- Google — https://google.github.io/styleguide/jsguide.html
- Airbnb — https://github.com/airbnb/javascript
