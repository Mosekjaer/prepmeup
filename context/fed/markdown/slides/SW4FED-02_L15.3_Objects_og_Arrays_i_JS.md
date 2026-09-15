# Objects og Arrays i JavaScript

## Metadata

- **Lektion:** L15 – Objects and Arrays in js
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L15/FED Objects and Arrays in js.pdf (19 slides)
- **Emner dækket:**
  - Properties og de to måder at tilgå dem på
  - Objects som referenceable containere af name/value-par
  - Dot notation kontra subscript notation
  - Chaining og optional chaining (`?.`)
  - Nullish coalescing (`??`) kontra `||`
  - `in`-operatoren
  - Date objects og deres `get...`-methods
  - Arrays i JavaScript: literal notation, `length`, `push`/`pop`/`join`
  - Typed arrays fra ECMAScript 2015

---

## 1. Properties

Rigtig mange JavaScript-værdier har andre værdier knyttet til sig. Disse associationer kaldes properties. Enhver string har for eksempel en property `length`, som refererer til et tal — antallet af karakterer i den pågældende string.

Properties kan tilgås på to måder, og de er ækvivalente her:

```javascript
var text = "purple haze";
console.log(text["length"]);
console.log(text.length);
```

Bemærk at properties på en string-værdi ikke kan ændres.

## 2. Objects

Objects har deres eget sæt af members i form af properties. Man kan frit modificere dem, fjerne dem eller tilføje nye:

```javascript
var cat = {colour: "grey", name: "Spot",
           size: 46};
cat.size = 47;
console.log(cat.size);
delete cat.size;
console.log(cat.size);
console.log(cat);
```

Nøgleordet `delete` skærer en property af objektet. Forsøger man at læse en property, der ikke eksisterer, får man værdien `undefined` — ikke en fejl.

Mere præcist er et object en referenceable container af name/value-par, sædvanligvis implementeret som en hash-table. Navnene er strings — eller andre elementer såsom tal, der konverteres til strings. Værdierne kan være hvilken som helst af datatyperne, inklusive andre objects. Nye members kan tilføjes til ethvert object på ethvert tidspunkt ved assignment.

Arrays og functions er implementeret som særlige slags objects.

## 3. Accessing Properties

Dot notation kan bruges, når subscriptet er en string-konstant i form af en lovlig identifier. På grund af en fejl i sprogdefinitionen kan reserverede ord ikke bruges i dot notation, men de kan godt bruges i subscript notation:

```javascript
var thing = {"gabba gabba": "hey"};
console.log(thing["gabba gabba"]);

thing["if"] = "funny";
console.log(thing["if"]);
```

Property-navnet `gabba gabba` indeholder et mellemrum og er derfor kun tilgængeligt via subscript notation. Det samme gælder det reserverede ord `if`.

## 4. Chaining

Hvis en af propertyerne selv er et object, kan man fortsætte med at "chaine" property-navne ved blot at tilføje endnu et punktum:

```javascript
const book = {
    title: "Eloquent JavaScript",
    author: {
        firstName: "Marijn",
        lastName: "Haverbeke"
    }
}
console.log(book.author.firstName); // Marijn
```

## 5. Optional Chaining `?.`

Men hvad nu hvis vores book kommer fra et book API, der ikke garanterer at have information om author? Med optional chaining kan man tilføje et `?` før punktummet. Det forsøger at hente værdien, og findes der ingen værdi, returneres `undefined` i stedet for at kaste en fejl:

```javascript
// Using optionel chaining
const authorName = book?.author?.firstName !== undefined
    ? book.author.firstName
    : 'Unknown'
```

## 6. Nullish Coalescing

I JavaScript findes der to måder, hvorpå tomhed eller ikke-eksistens er defineret: det kan enten være `undefined` eller `null`.

Da både `undefined` og `null` betragtes som falsy i JavaScript, var et af tricksene til at levere fallbacks at bruge `||`-operatoren, som returnerer den første truthy værdi:

```javascript
const authorName = book?.author?.firstName || 'Unknown'
console.log(authorName);
```

Problemet er, at `||` også slår til på legitime falsy værdier som tom string eller `0`. Det nullish coalescing-operatoren gør, er at evaluere den første værdi og kun bruge fallback'en, hvis værdien er "nullish" — med andre ord `null` eller `undefined`:

```javascript
const authorName = book?.author?.firstName ?? 'Unknown'
console.log(authorName);
```

## 7. In-operatoren

Operatoren `in` kan bruges til at teste, om et object har en bestemt property:

```javascript
var set = {"Spot": true};
// Add "White Fang" to the set
set["White Fang"] = true;
console.log("Spot" in set);
// Prints true
```

## 8. Date Objects

JavaScript har et indbygget Date object. Date-objektet kan gemme et klokkeslæt såvel som en dato og har en constructor til at lave nye date objects:

```javascript
var when = new Date(1980, 1, 1);
console.log(when);
```

Date-constructoren kan bruges på forskellige måder:

```javascript
console.log(new Date());
console.log(new Date(1980, 1, 1));
console.log(new Date(2007, 2, 30, 8, 20, 30));
```

Månedsnumrene går fra 0 til 11, hvilket kan være forvirrende — især da dagsnumrene starter fra 1.

Slidet noterer i en sidebemærkning, at der arbejdes på at tilføje et nyt date time API kaldet Temporal API til JavaScript, som adresserer mange af de eksisterende problemer med Date-objektet. På slidets tidspunkt er det kun understøttet i Firefox (se Temporal – JavaScript | MDN). Indtil da kan man bruge Luxon, et moderne letvægtsbibliotek til parsing, validering, manipulation og formatering af datoer i JavaScript: https://moment.github.io/luxon/api-docs/index.html

## 9. Date objects har en række get...-methods

Indholdet af et Date object kan inspiceres med en række `get...`-methods:

```javascript
var today = new Date();
console.log("Year: ", today.getFullYear(), ", month: ",
       today.getMonth(), ", day: ", today.getDate());
console.log("Hour: ", today.getHours(), ", minutes: ",
       today.getMinutes(), ", seconds: ", today.getSeconds());
console.log("Day of week: ", today.getDay());
```

## 10. Arrays i JavaScript

Arrays i JavaScript er implementeret som hashtable-objects. Det gør dem meget velegnede til sparse array-anvendelser. Værdierne findes via en key, ikke via en offset — det gør JavaScript-arrays meget bekvemme at bruge, men ikke velegnede til numerisk analyse (for langsomt).

Når man konstruerer et array, behøver man ikke deklarere en størrelse. Arrays vokser automatisk, meget lig en C#-collection. Der er to måder at lave et nyt tomt array på:

```javascript
var myArray = [];
var myArray = new Array();
```

## 11. Array Initializing

Arrays har en literal notation, der ligner den for objects:

```javascript
myList = ['oats', 'peas', 'beans', 'barley'];

monthLengths = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

slides = [
    { url: 'slide0001.html', title: 'Looking Ahead' },
    { url: 'slide0008.html', title: 'Forecast' },
    { url: 'slide0021.html', title: 'Summary' }
];

console.log('Number of days in february: ' + monthLengths[1]);
```

## 12. Arrays Are Not Typed

Arrays kan indeholde numbers, strings, booleans, objects, functions og arrays. Man kan blande strings, numbers og objects i det samme array. Det første index i et array er sædvanligvis nul.

## 13. length

Når et nyt element tilføjes til et array, og subscriptet er et heltal, der er større end den nuværende værdi af `length`, ændres `length` til subscriptet plus én. Det er en bekvemmelighedsfeature, som gør det let at bruge en for-løkke til at gennemgå elementerne i et array:

```javascript
myList = ['oats', 'peas', 'beans', 'barley'];

console.log('The length of myList is ' + myList.length);

myList[27] = 'banana';

console.log('The length of myList is ' + myList.length);
```

Efter tildelingen til index 27 rapporterer `length` værdien 28, selvom de mellemliggende pladser aldrig er blevet fyldt ud.

## 14. Arrays Specialities

Methoden `push` kan bruges til at tilføje værdier til et array. Methoden `pop` tager det sidste element af og returnerer det. `join` bygger én stor string ud fra et array af strings — parameteren, den får, indsættes mellem værdierne i arrayet:

```javascript
var mack = [];
mack.push("Mack");
mack.push("the");
mack.push("Knife");
console.log(mack.join(" "));
console.log(mack.pop());
console.log(mack);
```

## 15. Typed Arrays

Typed arrays er nye i JavaScript med ECMAScript 2015 og præsenterer en array-lignende visning af en underliggende binær data buffer:

```javascript
// create a TypedArray with a size in
// bytes
const arr = new Int8Array(8);
arr[0] = 32;
```

Slidet indeholder en oversigtstabel over de tilgængelige typed array-typer med værdiområde, størrelse i bytes, beskrivelse, Web IDL-type og den ækvivalente C-type:

| Type | Value Range | Size in bytes | Description | Web IDL type | Equivalent C type |
| --- | --- | --- | --- | --- | --- |
| Int8Array | -128 to 127 | 1 | 8-bit two's complement signed integer | byte | int8_t |
| Uint8Array | 0 to 255 | 1 | 8-bit unsigned integer | octet | uint8_t |
| Uint8ClampedArray | 0 to 255 | 1 | 8-bit unsigned integer (clamped) | octet | uint8_t |
| Int16Array | -32768 to 32767 | 2 | 16-bit two's complement signed integer | short | int16_t |
| Uint16Array | 0 to 65535 | 2 | 16-bit unsigned integer | unsigned short | uint16_t |
| Int32Array | -2147483648 to 2147483647 | 4 | 32-bit two's complement signed integer | long | int32_t |
| Uint32Array | 0 to 4294967295 | 4 | 32-bit unsigned integer | unsigned long | uint32_t |
| Float32Array | 1.2x10^-38 to 3.4x10^38 | 4 | 32-bit IEEE floating point number (7 significant digits e.g. 1.1234567) | unrestricted float | float |
| Float64Array | 5.0x10^-324 to 1.8x10^308 | 8 | 64-bit IEEE floating point number (16 significant digits e.g. 1.123...15) | unrestricted double | double |
| BigInt64Array | -2^63 to 2^63-1 | 8 | 64-bit two's complement signed integer | bigint | int64_t (signed long long) |
| BigUint64Array | 0 to 2^64-1 | 8 | 64-bit unsigned integer | bigint | uint64_t (unsigned long long) |

## 16. References and Links

- Eloquent JavaScript af Marijn Haverbeke — http://eloquentjavascript.net
- The definitive guide to JavaScript Dates — https://flaviocopes.com/javascript-dates/
- Parse, validate, manipulate, and display dates and times in JavaScript — https://momentjs.com/
