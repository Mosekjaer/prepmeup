# L19 – Promises og async/await

## Metadata

- **Lektion:** L19 – Promises and async await
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L19/FED Promises.pdf (18 slides)
- **Emner dækket:**
  - Hvad en Promise er, og executor-funktionens rolle
  - `then()` og `catch()`
  - Promise chaining
  - `Promise.all()` og `Promise.allSettled()`
  - Komplet eksempel med `XMLHttpRequest` (callee og caller)
  - `async` functions
  - `await` og concurrency
  - Error handling med try/catch i async functions

---

## 1. Hvad er en Promise?

En Promise repræsenterer en operation, der endnu ikke er færdig, men som forventes i fremtiden.

### Syntaks

```javascript
new Promise( /* executor */ function(resolve, reject) {
 ...
});
```

- **Executor-funktionen** eksekveres øjeblikkeligt af Promise-implementationen, som stiller `resolve`- og `reject`-funktionerne til rådighed.
- `resolve` og `reject` er bundet til promisen, og at kalde dem fulfiller eller rejecter promisen.
- Executoren forventes at igangsætte noget asynkront arbejde og derefter, når det er færdigt, enten kalde `resolve` (for at resolve promisens endelige værdi) eller kalde `reject` (for at rejecte den, hvis der opstod en fejl).

Du kan vælge de navne, du har lyst til, for de to parametre:

```javascript
new Promise(function(succes, fail) { ... });
```

## 2. Promise.prototype.then()

`then()`-metoden returnerer en Promise og tager to argumenter:

1. Callback-funktion for succes
2. Callback-funktion for fejltilfælde

### Syntaks

```javascript
p.then(onFulfilled, onRejected);
```

- **onFulfilled** — en funktion, der kaldes, når promisen er fulfilled (succes). Denne funktion har ét argument: fulfillment-værdien.
- **onRejected** — en funktion, der kaldes, når promisen er rejected (error). Denne funktion har ét argument: rejection reason.

## 3. Brug af then-metoden

### Kodeeksempel

```javascript
var p1 = new Promise(function (resolve, reject) {
    // Do some (asynchronous) work – e.g. make an ajax call
    resolve("Success!");
    // or
    // reject ("Error!");
});

p1.then(function (value) {
    console.log(value); // Success!
}, function (reason) {
    console.log(reason); // Error!
});
```

## 4. Promise.prototype.catch()

`catch()`-metoden returnerer en Promise og håndterer kun rejected tilfælde. Den opfører sig identisk med at kalde:

```javascript
Promise.prototype.then(undefined, onRejected)
```

### Syntaks

```javascript
p.catch(onRejected);
```

`onRejected` er en funktion, der kaldes, når promisen er rejected. Den har ét argument: rejection reason.

## 5. Brug af catch

### Kodeeksempel

```javascript
var p1 = new Promise(function (resolve, reject) {
    resolve('Success');
});

p1.then(function (value) {
    console.log(value); // "Success!"
    return Promise.reject('oh, no!');
}).catch(function (e) {
    console.log(e); // "oh, no!"
}).then(function () {
    console.log('after a catch the chain is restored');
}, function () {
    console.log('Not fired due to the catch');
});
```

Pointen er, at kæden **genoprettes** efter et `catch` — den efterfølgende `then` kører success-grenen, ikke fejlgrenen.

## 6. Beregningsforløbet for en promise

### Kodeeksempel

```javascript
console.log('Starting Execution');
const promise = rp('http://example.com/'); // Returns a Promise
promise.then(result => console.log(result));
console.log("Can't know if promise has finished yet...");
```

Den eneste måde at schedule kode efter en promise er at specificere en callback via `then`-metoden. Koden efter promise-oprettelsen kører videre med det samme — man kan ikke vide, om promisen er færdig endnu.

## 7. Chaining

### Kodeeksempel

```javascript
var p2 = new Promise(function (resolve, reject) {
    resolve(1);
});

p2.then(function (value) {
    console.log(value); // 1
    return value + 1;
}).then(function (value) {
    console.log(value); // 2
});

p2.then(function (value) {
    console.log(value); // 1
});
```

Bemærk forskellen: den kædede `then` modtager returværdien fra den foregående `then` (altså 2), mens en separat `then` direkte på `p2` stadig får promisens oprindelige værdi (1).

## 8. Promise.all(iterable)

`all`-funktionen returnerer en ny promise, som fulfilles med et **array af fulfillment-værdier** for de indsendte promises — eller rejectes med reason'en fra den første indsendte promise, der rejecter.

Den resolver alle elementer i det indsendte iterable til promises, mens den kører denne algoritme.

## 9. Promise.allSettled

`allSettled` venter på, at **alle** promises er settled. Den tager et array af Promises og returnerer først, når promisene er settled — enten rejected eller resolved.

### Kodeeksempel

```javascript
const promise1 = Promise.resolve(42);
const promise2 = Promise.reject("Some error occured");
const promises = [promise1, promise2];

Promise.allSettled(promises)
    .then(results => console.log(`Here are are your promises results`, results))
    .catch(err => console.log(`Catch ${err}`));
```

Forskellen til `Promise.all` er væsentlig: `all` fejler straks ved første rejection, mens `allSettled` altid venter på alle og rapporterer hver enkelt status.

## 10. Eksempel — the Callee

Her indpakkes den callback-baserede `XMLHttpRequest` i en Promise. Hele `function (succeed, fail) {...}` er executor-funktionen.

### Kodeeksempel

```javascript
function get(url) {
    return new Promise(function (succeed, fail) {
        var req = new XMLHttpRequest();
        req.open("GET", url, true);
        req.addEventListener("load", function () {
            if (req.status < 400)
                succeed(req.responseText);
            else
                fail(new Error("Request failed: " +
                                req.statusText));
        });
        req.addEventListener("error", function () {
            fail(new Error("Network error"));
        });
        req.send(null);
    });
}
```

## 11. Eksempel — the Caller

`get(...)` returnerer en Promise, som kalderen håndterer med `then`.

### Kodeeksempel

```javascript
get("files/data.txt").then(function (text) {
    // Resolve / succeed
    console.log("data.txt: " + text);
}, function (error) {
    // Reject / fail
    console.log ("Failed to fetch data.txt: " + error);
});
```

## 12. async functions

En `async` funktion er en genvej til at definere en funktion, der returnerer en promise.

### Kodeeksempel — resolve

```javascript
function f() {
    return Promise.resolve('TEST');
}

// asyncF is equivalent to f!
async function asyncF() {
    return 'TEST';
}
```

### Kodeeksempel — reject

```javascript
function f() {
  return Promise.reject('Error');
}

// asyncF is equivalent to f!
async function asyncF() {
  throw 'Error';
}
```

Altså: et `return` i en async funktion svarer til `Promise.resolve`, og et `throw` svarer til `Promise.reject`.

## 13. await

- `await` kan kun bruges inde i `async` functions.
- Den lader os synkront vente på en promise.
- Hvis vi bruger promises uden for async functions, skal vi stadig bruge `then`-callbacks.

### Kodeeksempel

```javascript
async function f(){
    // response will evaluate as the resolved value of the promise
    const response = await fetch('./api/some.json');
    console.log(response);
}

// We can't use await outside of async function.
// We need to use then callbacks ....
f().then(() => console.log('Finished'));
```

## 14. Spawn HTTP-kald og kør dem concurrently

Et vigtigt mønster: hvis man `await`'er hvert kald med det samme, kører de sekventielt. Ved først at starte kaldene (uden `await`) og derefter `await`'e de gemte promises, kører de samtidigt.

### Kodeeksempel

```javascript
// Encapsulate the solution in an async function
async function solution() {
  // Wait for the first HTTP call and print the result
  console.log(await fetch('http://example.com/1'));

  // Spawn the HTTP calls without waiting for them - run them concurrently
  const call2Promise = fetch('http://example.com/2'); // Does not wait!
  const call3Promise = fetch('http://example.com/3'); // Does not wait!

  // After they are both spawn - wait for both of them
  const response2 = await call2Promise;
  const response3 = await call3Promise;
  console.log(response2);
  console.log(response3);
}

// Call the async function
solution().then(() => console.log('Finished'));
```

## 15. Error Handling

Hvis en promise, vi `await`'er, fejler, resulterer det i en exception inde i async-funktionen. Vi kan bruge standard try/catch til at håndtere det.

### Kodeeksempel

```javascript
async function f() {
  try {
      const promiseResult = await Promise.reject('Error');
  } catch (e){
      console.log(e);
  }
}
```

## 16. References & Links

- ES6 Promises in Depth: https://ponyfoo.com/articles/es6-promises-in-depth
- MDN Promise: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise
- Await and Async Explained with Diagrams and Examples: http://nikgrozev.com/2017/10/01/async-await/
- What You Need to Know About Asynchronous Programming in JavaScript: https://medium.com/swlh/what-you-need-to-know-about-asynchronous-programming-in-javascript-894f90a97941
