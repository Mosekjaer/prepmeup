# L15 – Functions i JavaScript

## Metadata

- **Lektion:** L15 – Functions and Scope in JavaScript
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L15/FED Functions in js.pdf (29 slides)
- **Emner dækket:**
  - Function Declaration og syntaks
  - Pure functions vs. functions med side effects
  - Kald af funktioner og typeløse parametre
  - `arguments`-objektet
  - Function Expression og self invoking function expression
  - Scope: lokalt scope, nested functions, "static" variabler
  - Blocks og `let`
  - Closures
  - Hoisting med fire konkrete quizspørgsmål

---

## 1. Agenda

- Functions
- Scope
- Hoisting

## 2. Function Declaration

En **Function Declaration** definerer en named function variable uden at kræve variable assignment. Function Declarations optræder som standalone statements.

ECMA 5 definerer syntaksen som:

```text
function Identifier ( FormalParameterListopt ) { FunctionBody }
```

### Kodeeksempel

```javascript
function bar() {
    return 3;
}

console.log(bar()); //3
```

## 3. Pure Functions

Pure functions returnerer altid den samme værdi, når de gives de samme argumenter, og har aldrig side effects.

### Kodeeksempel

```javascript
function add(a, b) {
  return a + b;
}

console.log(add(2, 3));     // 5
```

- Navnene på en funktions argumenter er tilgængelige som variabler inde i den.
- Pure functions er nemme at tænke over, og de er nemme at genbruge.
- Et `return`-statement uden et expression efter sig får funktionen til at returnere `undefined`.

## 4. Functions with Side Effects

Funktioner med side effects behøver ikke at indeholde et `return`-statement. Hvis intet `return`-statement mødes, returnerer funktionen `undefined`.

### Kodeeksempel

```javascript
function yell(message) {
  alert(message + "!!");
}
console.log(yell("Wow"));
```

```javascript
var count = 0;
function inc(step) {
  count += step;
  return count;
}
```

Bemærk: I JavaScript starter funktionsnavne ikke med stort begyndelsesbogstav.

## 5. Kald af en funktion

De formelle parametre er typeløse, så man kan sende enhver slags værdi til en funktion — men funktionen virker måske ikke med alle slags værdier! Og antallet af argumenter behøver ikke at matche antallet af formelle parametre.

### Kodeeksempel

```javascript
function add(a, b) {
  return a + b;
}
console.log(add('2', '3'));                 // 23
console.log(add(true, false));              // 1
console.log(add(4));                        // NaN
console.log(add(5,6,7,8));                  // 11
```

## 6. The Arguments Object

Inde i en funktion kan argumenterne også tilgås gennem `arguments`-objektet. Det giver adgang til alle argumenter via indices.

### Kodeeksempel

```javascript
function add() {
   var sum = 0;
   for (i=0; i < arguments.length; i++) {
     sum += arguments[i];
   }
   return sum;
};

console.log(add(1,2,3));           // 6
```

## 7. Function Expression

Når keywordet `function` bruges et sted, hvor et expression forventes, behandles det som et expression, der producerer en function-værdi. Funktionsnavnet er valgfrit.

### Kodeeksempel

```javascript
const add = function addFunc(a, b)
{
    return a + b;
};
console.log(add(5, 5));
```

```javascript
const add = function (a, b) {
    return a + b;
};
console.log(add(5, 5));
console.log(typeof add);
// prints function
```

ECMA 5 definerer syntaksen som:

```text
function Identifieropt ( FormalParameterListopt ) { FunctionBody }
```

## 8. Self Invoking Function Expression

Function Expressions må ikke starte med `function` — derfor parenteserne omkring den self invoking function expression.

### Kodeeksempel

```javascript
(function sayHello() {
    alert("hello!");
})();
```

## 9. Scope — aka environment

Funktioner i JavaScript har **lexical scoping**.

## 10. Funktioner danner et lokalt scope

Variablerne i dette lokale environment er kun synlige for koden inde i funktionen. Variablerne er ikke tilgængelige udefra funktionen.

### Kodeeksempel

```javascript
var str = "top-level";

function printVariable() {
    console.log("inside printVariable, str holds '" + str + "'.");
}

function test() {
    var str = "local";
    console.log ("inside test, str holds '"+ str + "'.");
    printVariable();
}

test();
```

Output:

```text
inside test, str holds 'local'.
inside printVariable, str holds 'top-level'.
```

## 11. Nested Functions scope

Når en funktion defineres inde i en anden funktion, baseres dens lokale environment på det lokale environment, der omgiver den — i stedet for på top-level environment'et.

### Kodeeksempel

```javascript
var variable = "top-level";

function parentFunction() {
    var variable = "local";

    function childFunction() {
        console.log(variable);
    }

    childFunction();
}
parentFunction();
```

Output: `local`

## 12. "Static" Variables

Enhver variabel brugt i en funktion, som ikke er eksplicit defineret med `var`, `let` eller `const`, antages at tilhøre et ydre scope — muligvis det globale objekt.

### Kodeeksempel

```javascript
function printVariable() {
    console.log("inside printVariable, myStr holds '" + myStr + "'.");
}

function test() {
    myStr = "Static";
    console.log("inside test, myStr holds '"+ myStr + "'.");
    printVariable();
}

test();
```

Output:

```text
inside test, myStr holds 'Static'.
inside printVariable, myStr holds 'Static'.
```

## 13. Brug altid var, let eller const!

Der er ingen namespaces eller class scopes i JavaScript, så alle "static" variabler ryger i det globale scope (environment). Og hvis man ved et uheld bruger et variabelnavn, der allerede er der, overskriver man det. På den måde kan man blokere for adgang til vigtig funktionalitet!

### Kodeeksempel — gør ikke dette

```javascript
function foo() {
    a = 2;
    b = "Hello";

    return 7 * a;
}

console.log(foo());
console.log(a);
console.log(b);
```

## 14. Blocks

I modsætning til de andre sprog i C-familien producerer en blok af kode (mellem tuborgklammer) IKKE et nyt lokalt environment. Funktioner er de eneste ting, der skaber et nyt scope.

### Kodeeksempel — med var

```javascript
var something = 1;
{
    var something = 2;
    console.log ("Inside: " + something);
}

console.log("Outside: " + something);
```

Output: `Inside: 2` og `Outside: 2`.

### Kodeeksempel — med let

```javascript
let nothing = 1;
{
    let nothing = 2;
    console.log ("Inside: " + nothing);
}

console.log("Outside: " + nothing);
```

Output: `Inside: 2` og `Outside: 1`.

## 15. Closure

En funktion defineret inde i en anden funktion bevarer adgang til det environment, der eksisterede i den funktion på det tidspunkt, hvor den blev defineret.

### Kodeeksempel

```javascript
var variable = "top-level";

function parentFunction() {
    var variable = "local";
    function childFunction() {
        console.log(variable);
    }
    return childFunction;
}

var child = parentFunction();
child();
```

Output: `local`

### Kodeeksempel — function factory

```javascript
function makeAddFunction(amount) {
    function add(number) {
        return number + amount;
    }
    return add;
}

var addTwo = makeAddFunction(2);
var addFive = makeAddFunction(5);
console.log(addTwo(1) + addFive(1));
```

Output: `9`

## 16. Hoisting — Question 1

Hvad bliver alerted? 3, 8 eller TypeError?

```javascript
function foo() {
    function bar() {
        return 3;
    }
    return bar();

    function bar() {
        return 8;
    }
}
alert(foo());
```

## 17. Hoisting — Question 2

Hvad bliver alerted? 3, 8 eller TypeError?

```javascript
function foo() {
    var bar = function() {
        return 3;
    }
    return bar();

    var bar = function() {
        return 8;
    }
}
alert(foo());
```

## 18. Hoisting — Question 3

Hvad bliver alerted? 3, 8 eller TypeError?

```javascript
alert(foo());

function foo() {
    var bar = function() {
        return 3;
    }
    return bar();

    var bar = function() {
        return 8;
    }
}
```

## 19. Hoisting — Question 4

Hvad bliver alerted? 3, 8 eller TypeError?

```javascript
function foo() {
    return bar();

    var bar = function() {
        return 3;
    }

    var bar = function() {
        return 8;
    }
}

alert(foo());
```

## 20. Hvad er Hoisting?

Function declarations og function variables flyttes altid ("hoistes") til toppen af deres JavaScript scope af JavaScript-interpreteren.

## 21. Question 1 Revisited

Når en function declaration hoistes, løftes hele funktionskroppen med. Så efter interpreteren er færdig med koden i Question 1, kører den mere sådan her:

### Kodeeksempel — original

```javascript
function foo() {
    function bar() {
        return 3;
    }
    return bar();

    function bar() {
        return 8;
    }
}
alert(foo());
```

### Kodeeksempel — efter hoisting

```javascript
function foo() {
    function bar() {
        return 3;
    }
    function bar() {
        return 8;
    }

    return bar();
}
alert(foo());
```

Svaret er altså **8** — den anden `bar` overskriver den første, før `return bar()` køres.

## 22. Question 2 Revisited

Venstre side (`var bar`) er en **Variable Declaration**. Variable Declarations hoistes — men deres Assignment Expressions gør ikke. Så når `bar` hoistes, sætter interpreteren initialt `var bar = undefined`.

### Kodeeksempel — efter hoisting

```javascript
function foo() {
    //a declaration for each function expression
    var bar = undefined;
    var bar = undefined;
    //first Function Expression is executed
    bar = function () {
        return 3;
    }
    // Function created by first Function Expression is invoked
    return bar();
    // second Function Expression unreachable
} alert(foo());
```

Svaret er **3**.

## 23. Question 3 Revisited

Når en function declaration hoistes, løftes hele funktionskroppen med.

### Kodeeksempel — original

```javascript
alert(foo());

function foo() {
    var bar = function() {
        return 3;
    }
    return bar();

    var bar = function() {
        return 8;
    }
}
```

### Kodeeksempel — efter hoisting

```javascript
//Hoisting
function foo() {
    //Hoisting
    var bar = undefined;
    var bar = undefined;
    bar = function () {
        return 3;
    }
    return bar();
    // second expression unreachable
}
alert(foo());
```

Svaret er **3** — selve `foo` er hoisted, så kaldet før definitionen virker.

## 24. Question 4 Revisited

`var bar` er ikke en funktion, når den returneres.

### Kodeeksempel — original

```javascript
function foo() {
    return bar();

    var bar = function() {
        return 3;
    }

    var bar = function() {
        return 8;
    }
}

alert(foo());
```

### Kodeeksempel — efter hoisting

```javascript
function foo() {
    //Hoisting
    var bar = undefined;
    var bar = undefined;

    return bar();
    // Both expressions unreachable
}

alert(foo());
```

Svaret er **TypeError** — `bar` er `undefined` på kaldstidspunktet.

## 25. References and Links

- Eloquent JavaScript af Marijn Haverbeke — http://eloquentjavascript.net
- The JavaScript guru: Douglas Crockfords blog — http://www.crockford.com/javascript/
- Function Declarations vs. Function Expressions (Hoisting) — http://javascriptweblog.wordpress.com/2010/07/06/function-declarations-vs-function-expressions/
