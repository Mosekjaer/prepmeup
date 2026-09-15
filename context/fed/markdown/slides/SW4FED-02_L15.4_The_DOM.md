# The DOM

## Metadata

- **Lektion:** L15 – The DOM (How JavaScript interacts with a Web Page)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L15/FED The DOM.pdf (36 slides)
- **Emner dækket:**
  - The DOM og document-objektet
  - Navigation i DOM-træet: parentNode, childNodes, siblings, nodeType
  - Find elementer: getElementsByTagName, getElementById, getElementsByClassName, querySelector
  - Ændring af dokumentet: document.write, innerHTML, appendChild/insertBefore/replaceChild/removeChild
  - Oprettelse af nodes og arbejde med attributes
  - Browser events, addEventListener og event objects
  - Event bubbling, stopPropagation og preventDefault
  - Unobtrusive JavaScript og form validation
  - Ændring af style fra JavaScript
  - Web workers

---

## Agenda

Forelæsningen dækker fire hovedområder: the DOM, ændring af dokumentet, browser events og ændring af style fra JavaScript.

## 1. The document

Ethvert window object har en `document`-property, som indeholder et object, der repræsenterer det dokument, der vises i vinduet. Dette object indeholder for eksempel en property `location` med information om dokumentets URL:

```javascript
alert(document.location.href);
```

Sætter man `document.location.href` til en ny URL, kan det bruges til at få browseren til at loade et andet dokument.

## 2. Document Object Model (DOM)

DOM'en er en hierarkisk struktur. Ethvert tag i dokumentet er repræsenteret i denne model og kan slås op og interageres med. Slidet viser en portion af DOM-træet som et diagram, hvor `html` er roden med `head` og `body` som børn, og hvor hvert HTML-element bliver en node med sine egne childNodes.

## 3. DOM Example

Eksempeldokumentet, som resten af forelæsningen refererer til:

```html
<html>
  <head>
    <title>Alchemy for beginners</title>
    <script type="text/javascript" src="js/chapter/dom.js"></script>
    <script type="text/javascript" src="js/FunctionalTools.js"></scrip
    <style type="text/css">
       td, th {border: 1px solid black; padding: 3px;}
       table {border-collapse: collapse;}
    </style>
  </head>
  <body><h1>Chapter 1: Equipment</h1>
    <p>This is what an <em>alchemists' bottle</em> looks like:</p>
    <img src="img/florence_flask.png" alt="a fat bottle"
id="picture"/></body>
</html>
```

<!-- uklart i kilden: det andet script-tag er afskåret i slidet ("</scrip") -->

## 4. Navigating the DOM

`document.body` peger på body-delen af dokumentet. Forbindelserne mellem nodes er tilgængelige som properties på node-objekterne:

- Ethvert DOM-object har en `parentNode`-property, som refererer til det object, det er indeholdt i, hvis noget.
- Disse parents har også links tilbage til deres børn — i et pseudo-array kaldet `childNodes`. Der er desuden links kaldet `firstChild` og `lastChild`.
- Endelig findes properties kaldet `nextSibling` og `previousSibling`, som peger på de nodes, der ligger "ved siden af" en node — altså nodes, der er børn af den samme parent.

## 5. nodeType

For at finde ud af, om en node repræsenterer et simpelt stykke tekst eller en egentlig HTML-node, kan man kigge på dens `nodeType`-property. Den indeholder et tal: 1 for regular nodes og 3 for text nodes.

```javascript
function isTextNode(node) {
    return node.nodeType == 3;
}

show(isTextNode(document.body));
show(isTextNode(document.body.firstChild.firstChild));
```

## 6. Finding Elements – TagName

Man kan tilgå en node ved hjælp af `childNodes` og en række `nextSibling` og så videre. Det kan fungere, men det er omstændeligt og let at ødelægge. Alle element nodes har i stedet nogle meget bekvemme hjælpefunktioner til at finde element nodes:

```javascript
var link = document.body.getElementsByTagName("a")[0];
console.log(link.href);
```

Kaldet henter alle anchor nodes i dokumentet, og `[0]` plukker den første ud.

## 7. Finding Elements – id attribute

Giv de elementer, du har brug for adgang til, et `id`-attribut og brug `getElementById`:

```javascript
var picture = document.getElementById("picture");
alert(picture.src);
picture.src = "img/ostrich.png";
```

Fordi `document.getElementById` er et latterligt langt navn for en meget almindelig operation, er det blevet en konvention blandt JavaScript-programmører aggressivt at forkorte det til `$`:

```javascript
function $(id) {
    return document.getElementById(id);
}
alert($("picture"));
```

## 8. Finding Elements – class name

`getElementsByClassName` søger gennem indholdet af en element node og henter alle elementer, der har den givne string i deres `class`-attribut. Den fungerer tilsvarende `getElementsByTagName`.

## 9. querySelector

`querySelector`-methoden er nyttig, hvis man vil have et specifikt, enkelt element. Den returnerer kun det første matchende element eller `null`, hvis ingen elementer matcher. `querySelectorAll`-methoden returnerer et array-lignende object indeholdende alle de elementer, den matcher.

Begge methods tager en selector string med samme syntaks som brugt i CSS3:

```javascript
var nodes = document.querySelectorAll("p .animal");
```

## 10. document.write

`document.write` skriver noget HTML til dokumentet. Bruges den på et fuldt loadet dokument, erstatter den hele dokumentet. Men hvis et script kalder den, mens dokumentet loades, indsættes den skrevne HTML i dokumentet på det sted, hvor det script-tag, der udløste kaldet, står. Det er en simpel måde at tilføje dynamiske elementer til en side:

```html
</head>
<body>
    <h1>The time</h1>
    <p>The time is
      <script type="text/javascript">
           var time = new Date();
           document.write(time.getHours() + ":" + time.getMinutes());
       </script>
    </p>
</body>
</html>
```

## 11. innerHTML

`innerHTML`-propertyen kan bruges til at hente HTML-teksten inde i en node, uden tags for selve noden:

```javascript
alert(document.body.innerHTML);
```

Sætter man `innerHTML` på en node eller `nodeValue` på en text node, ændres dens indhold:

```javascript
document.body.firstChild.nextSibling.innerHTML =
  "Chapter 1: The deep significance of the bottle";
```

```javascript
document.body.firstChild.nextSibling.firstChild.nodeValue =
  "Chapter 1: The deep significance of the bottle";
```

## 12. Changing the Child Nodes

Element nodes har en række methods, der kan bruges til at ændre deres indhold: `appendChild`, `insertBefore`, `replaceChild` og `removeChild`.

```html
<p>One</p>
<p>Two</p>
<p>Three</p>

<script>
  var paragraphs = document.body.getElementsByTagName("p");
  document.body.insertBefore(paragraphs[2], paragraphs[0]);
</script>
```

Vigtig pointe fra slidet: en node kan kun eksistere ét sted i dokumentet. Indsætter man en eksisterende node et nyt sted, flyttes den — den kopieres ikke.

## 13. Creating nodes

`createElement()` opretter en ny type1-node, og `createTextNode()` opretter en ny type3-node:

```javascript
var secondHeader = document.createElement("h1");
var secondTitle = document.createTextNode("Chapter 2: Deep
magic");
secondHeader.appendChild(secondTitle);
document.body.appendChild(secondHeader);

var newImage = document.createElement("img");
newImage.setAttribute("src", "../img/ostrich.png");
document.body.appendChild(newImage);
```

<!-- uklart i kilden: string-literalen "Chapter 2: Deep magic" er ombrudt over to linjer i slidet -->

## 14. Attributes

Nogle element-attributter kan tilgås gennem en property med samme navn på elementets DOM-object:

```javascript
var link = document.body.getElementsByTagName("a")[0];
link.href = "http://ece.au.dk";
```

Men HTML tillader, at man sætter hvilket som helst attribut på nodes, og til det bruger man `setAttribute` og `getAttribute`:

```html
<p data-classified="secret">The launch code is 00000000.</p>

<script> var paras = document.body.getElementsByTagName("p");
if (paras[0].getAttribute("data-classified") == "secret")
  paras[0].parentNode.removeChild(paras[0]);
```

<!-- uklart i kilden: script-blokken er ikke lukket i slidet -->

## 15. JavaScript and Events

Events er handlinger foretaget af den besøgende på websiden:

- klik (`click`)
- placering af musen på et element (`mouseover`)
- fjernelse af musen fra et element (`mouseout`)
- loading af siden (`load`)
- unloading af siden (`unload`)
- og så videre

`addEventListener`-funktionen registrerer sit andet argument til at blive kaldt, hver gang den event, der beskrives af det første argument, indtræffer:

```javascript
addEventListener("click", function() {
    console.log("You clicked!");
  });
```

Slidet viser en tabel over event-navne og deres tilsvarende event handlers:

| Event | Event Handler |
| --- | --- |
| click | onclick |
| load | onload |
| mouseover | onmouseover |
| mouseout | onmouseout |
| submit | onsubmit |
| unload | onunload |

## 16. Obtrusive JavaScript

JavaScript kan konfigureres til at udføre handlinger, når events indtræffer, ved at kode event-navnet som et attribut på et HTML-tag. Værdien af event-attributtet indeholder JavaScript-koden. Eksempel: vis en alert box, når musen placeres over et hyperlink:

```html
<a href="home.htm"
   onmouseover="alert('Click to go home')">
   Home
</a>
```

Slidet markerer dette som obtrusive JavaScript — ikke anbefalet.

## 17. Unobtrusive JavaScript

I stedet foretages event subscriptions i JavaScript, adskilt fra markup'en:

```html
...
<body>
  <!-- button has no event wire up code -->
  <a id="homelink" href="home.htm">Home</a>

    <script>
       document.getElementById("homelink")
                .addEventListener("mouseover", function () {
             alert('Click to go home');
       });
    </script>
</body>
```

## 18. Event objects

Event handler-funktioner får et argument med: event-objektet. Det giver yderligere information om eventen, og den information, der er gemt i et event object, varierer alt efter event-typen:

```html
<button>Click me any way you want</button>
<script>
 var button = document.querySelector("button");
 button.addEventListener("mousedown", function(event) {
   if (event.which == 1)
      console.log("Left button");
   else if (event.which == 2)
      console.log("Middle button");
   else if (event.which == 3)
      console.log("Right button");
  });
</script>
```

## 19. Event Bubbling

En uhåndteret event vil "boble" gennem DOM-træet:

- Klikker man på et link i et paragraph, kaldes eventuelle handlers på linket først.
- Hvis der ikke er sådanne handlers — eller hvis disse handlers ikke indikerer, at de er færdige med at håndtere eventen —
- prøves handlers for paragraph'et, som er linkets parent.
- Derefter får handlers for `document.body` en tur.
- Endelig, hvis ingen JavaScript-handlers har taget sig af eventen, håndterer browseren den.

Eventen siges at propagere udad, væk fra den node, hvor den skete. En event handler kan kalde `stopPropagation`-methoden på event-objektet for at forhindre handlers "længere oppe" i at modtage eventen.

## 20. Default actions

Mange events har en default action knyttet til sig — klikker man for eksempel på et link, bliver man ført til linkets target. De fleste JavaScript event handlers kaldes, før default-adfærden udføres. Hvis handleren ikke ønsker, at den normale adfærd skal ske, bruges `preventDefault`-methoden på event-objektet.

Afhængigt af browseren kan nogle events dog ikke opsnappes. I Chrome kan tastaturgenveje til at lukke den aktuelle tab (Ctrl-W eller Command-W) ikke håndteres af JavaScript.

```javascript
var link = document.querySelector("a");
link.addEventListener("click", function(event) {
  // DoSomeThing();
  event.preventDefault();
});
```

## 21. Mouse, Touch, and Pointer Events

Slidet viser en sammenligningstabel mellem Mouse Events, Touch Events og Pointer Events på tværs af kriterier som understøttelse af mus, single-touch, multi-touch, pen/Kinect og andre enheder, over/out/enter/leave-events og hover, asynkron panning/zooming-initiering til hardwareacceleration, W3C-specifikation samt brugbarhed på tværs af browsere på henholdsvis mobile og desktop-enheder.

<!-- uklart i kilden: tabellens celleværdier er ikke læsbare i den udtrukne tekst; kun kolonnen for Pointer Events viser fragmenterne "Yes", "Partly" og "No" uden entydig rækketilknytning -->

Reference: https://developer.mozilla.org/en-US/docs/Web/API/Pointer_events

## 22. Form Validation

Det er almindeligt at bruge JavaScript til at validere formularinformation, før den sendes til webserveren. Typiske spørgsmål: er navnet indtastet? Er e-mailadressen i det korrekte format? Er telefonnummeret i det korrekte format?

Man bruger `""` eller `null` til at tjekke, om et formularfelt har information:

```javascript
if (document.forms[0].userName.value == "" ) {
       alert("Name field cannot be empty.");
       return false;
     } // end if
```

## 23. CSS attributes fra JavaScript

Hvordan sætter man et CSS-attribut på en html-node i DOM'en? Man sætter det ganske enkelt gennem `style`-attributtet. For eksempel til at sætte bredden på et element:

```javascript
var obj = document.getElementById("myId");
obj.style.width = "100px";
```

## 24. CSS Properties with Hyphens

JavaScript tillader ikke bindestreger i navne, så "camelCase" bruges i stedet. CSS-propertyen `background-color` tilgås derfor som `backgroundColor` fra JavaScript:

```javascript
var obj = document.getElementById("myId");
obj.style.backgroundColor = "#ff0000";
```

## 25. Unofficial Attributes

Uofficielle style-attributter som `-webkit-background-size` kan sættes med denne syntaks:

```javascript
var obj = document.getElementById("myId");
obj.style["-webkit-background-size"] = "400px"
```

## 26. Separate Behaviour and Style

De foregående eksempler blander style og behaviour. Det gør sitet eller app'en sværere at vedligeholde, da designeren så skal arbejde med både CSS- og JavaScript-filerne.

En renere tilgang er at definere alle de forskellige styles i CSS-filen — for eksempel som forskellige classes — og derefter ændre elementets class-navn fra JavaScript:

```javascript
var obj = document.getElementById("myId");
obj.className = "newClass";
```

Bemærk: `class` i HTML bliver til `className` i JavaScript.

## 27. Web workers

JavaScript, der kører i browseren, er hovedsageligt et single threaded environment (ES5). Men JavaScript har visse multithreading-egenskaber:

- `xmlHttpRequest`-objektet kan bruges til at sende asynkrone requests til en webserver (ajax).
- Web workers bruges til at spawne en ny tråd.

En web worker er et isoleret JavaScript-environment, der kører sideløbende med hovedprogrammet for et dokument. Web workers og hovedprogrammet interagerer via message passing ved brug af `postMessage()`-methoden og `message`-eventen.

Mere detaljeret information: https://developer.mozilla.org/en-US/docs/Web/API/Web_Workers_API/Using_web_workers

## 28. Web Worker Example

Hovedprogrammet:

```javascript
var squareWorker = new Worker("code/squareworker.js");

squareWorker.addEventListener("message", function(event) {
  console.log("The worker responded:", event.data);
});

squareWorker.postMessage(10);
squareWorker.postMessage(24);
```

`code/squareworker.js`:

```javascript
addEventListener("message", function(event) {
  postMessage(event.data * event.data);
});
```

## 29. References and Links

- Eloquent JavaScript af Marijn Haverbeke — http://eloquentjavascript.net
- Wikipedia har en fremragende oversigt — http://en.wikipedia.org/wiki/DOM_events
- How to Write Accessible JavaScript — https://medium.com/dailyjs/4-javascript-techniques-for-building-accessible-web-interfaces-348f820c157f
