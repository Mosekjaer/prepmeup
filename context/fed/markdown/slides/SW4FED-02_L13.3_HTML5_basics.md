# L13 – HTML5 Basics

## Metadata

- **Lektion:** L13 – HTML5 Basics (Web Development with HTML5)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED HTML5 basics.pdf (35 slides)
- **Emner dækket:**
  - Hvad HTML er, W3C og WHATWG, HTML5 som Living Standard
  - XHTML, well-formedness og void elements
  - Doctype, head- og body-sektion
  - Headings, paragraphs, line break, blockquote
  - Phrase elements (`<b>`, `<em>`, `<i>`, `<mark>`, `<small>`, `<strong>`, `<sub>`, `<sup>`)
  - Lister: unordered, ordered og description lists
  - Special characters og whitespace-entities
  - `<div>` vs. `<span>`, block vs. inline elements
  - Anchor-elementet: absolutte/relative links, `target`, fragments, mailto
  - Validering og opsætning af en simpel node-baseret http-server

---

## 1. What is HTML?

- HTML er et akronym for **Hyper Text Mark-up Language**.
- Det er det sæt af markup-symboler eller koder der placeres i en fil beregnet til visning på en webbrowser-side.
- World Wide Web Consortium (http://w3c.org) sætter standarderne for HTML og relaterede sprog.
  - Men standarden udvikles nu af **Web Hypertext Application Technology Working Group**, også kendt som **WHATWG**: https://whatwg.org/
  - Selve specifikationen: https://html.spec.whatwg.org/multipage/

## 2. HTML Elements

- Hver markup-kode repræsenterer et **HTML element**.
- Hvert element har et formål.
- De fleste elementer kodes som et par af tags: et **opening tag** og et **closing tag**.
  - Tags omsluttes af vinkelparenteser, `<` og `>`.

```html
<html>
  <head>
    <title>I4GUI</title>
  </head>
  <body>
    <h1>GUI programmering</h1>
    <h2>I4GUI</h2>
    <p>Til Web-applikationer anvendes <b>HTML</b>,
       CSS og Javascript.</p>
  </body>
</html>
```

## 3. What is XHTML?

**eXtensible HyperText Markup Language** er en dialekt af HTML. XHTML bruger:

- elementerne og attributterne fra HTML
- syntaksen fra XML (eXtensible Markup Language)

Skal være **well-formed**:

- Brug lowercase
- Brug opening- og closing-tags: `<body>   </body>`
- Luk stand-alone tags med speciel syntaks: `<hr />`

## 4. What is HTML5?

- Nyeste anbefalede version af HTML/XHTML — HTML 5.2: W3C Recommendation, december 2017.
- Understøttet af moderne browsere: Safari, Google Chrome, Firefox, Edge.
- Er beregnet til at være bagudkompatibel.
- Tilføjer nye elementer.
- Tilføjer ny funktionalitet:
  - Validate form data
  - Native video og audio
  - Og mere!

Slidet fremhæver i en boks: **HTML5 er nu en "Living Standard"** — altså ikke længere versionerede udgivelser, men en løbende opdateret specifikation hos WHATWG.

## 5. Document Type Definition

Doctype-statementet placeres øverst i et webside-dokument og identificerer den version af HTML dokumentet indeholder.

HTML5 DTD:

```html
<!DOCTYPE html>
```

## 6. Example HTML5 Web Page

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Page Title Goes Here</title>
    <meta charset="utf-8">
</head>
<body>
    <p>
       Body text and more HTML5 tags go here
    </p>
</body>
</html>
```

Til sammenligning viser slidet også XHTML5-varianten, som tilføjer et `xmlns`-namespace og lukker void-elementer eksplicit:

```html
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
</body>
</html>
```

## 7. Head Sections

Head-sektionen indeholder information der beskriver webside-dokumentet.

```html
<head>
   head section info goes here
</head>
```

HTML head-elementer:

| Tag | Beskrivelse |
|---|---|
| `<title>` | Definerer titlen på et dokument |
| `<base>` | Definerer en default-adresse eller et default-target for alle links på en side |
| `<link>` | Definerer relationen mellem et dokument og en ekstern ressource |
| `<meta>` | Definerer metadata om et HTML-dokument |
| `<script>` | Definerer et client-side script |
| `<style>` | Definerer style-information for et dokument |

## 8. Body Section

Body-sektionen indeholder tekst og elementer der vises i webside-dokumentet.

```html
<body>
   body section info goes here
</body>
```

## 9. The Heading Element

```html
<body>
    <h1>Heading Level 1</h1>
    <h2>Heading Level 2</h2>
    <h3>Heading Level 3</h3>
    <h4>Heading Level 4</h4>
    <h5>Heading Level 5</h5>
    <h6>Heading Level 6</h6>
</body>
```

Skærmbilledet ved siden af koden viser de seks overskrifter renderet i browseren: alle er fede, men med **aftagende skriftstørrelse** fra `h1` (klart størst) ned til `h6` (mindre end normal brødtekst). Der er også lodret luft (margin) over og under hver overskrift.

Google anbefaler **maks. én `<h1>` pr. side!**

## 10. Paragraph Element

```html
<p> …paragraph goes here… </p>
```

- Grupperer sætninger og tekstafsnit sammen.
- Konfigurerer en tom linje over og under afsnittet.
  - Dette kan ændres med en CSS-style.

## 11. Line Break Element

Line Break-elementet er et **stand-alone** eller **void** tag:

- HTML-syntaks: `<br>`
- XHTML-syntaks: `<br />`

```html
…text goes here <br>
This starts on a new line…
```

Får det næste element eller den næste tekst til at blive vist på en ny linje.

## 12. Void Elements

Betegnelsen **void elements** bruges om elementer der skal være tomme. Sådanne elementer inkluderer blandt andre `br`, `hr`, `link` og `meta`.

| Skrivemåde | Gyldighed |
|---|---|
| `<br>` | Et void element i HTML-syntaks. Dette er **ikke** tilladt i XHTML-syntaksen |
| `<br/>` | Et void element med den self-closing tag-syntaks der er kompatibel med både HTML og XHTML |
| `<br></br>` | Et void element med den XHTML-only syntaks med eksplicit end tag. Dette er **ikke** tilladt for void elements i HTML-syntaksen |

## 13. Blockquote Element

Blockquote-elementet indrykker en tekstblok med henblik på særlig fremhævelse.

```html
<blockquote>
   …text goes here…
</blockquote>
```

## 14. Paragraph and Blockquote Demo

```html
<body>
<h6>Heading Level 6</h6>
<p>
    …paragraph goes here…
    This text continue on the same line<br />
    but this text is on a line below.
</p>
<p>Next paragraph appears here</p>
   <blockquote>
     This text is indented.
   </blockquote>
<p>       this is not </p>
</body>
```

Browser-skærmbilledet viser præcis hvad hvert element gør:

- `<h6>` "Heading Level 6" står øverst i lille fed skrift.
- Det første afsnit vises som løbende tekst: "…paragraph goes here… This text continue on the same line" fortsætter på samme linje selvom kilden har linjeskift der (whitespace i HTML kollapser), og først ved `<br />` bryder teksten til "but this text is on a line below."
- "Next paragraph appears here" står som et nyt afsnit med tom linje over.
- Blockquote-teksten "This text is indented." er tydeligt **indrykket** fra venstre margen.
- Det sidste afsnit "this is not" står igen ude ved venstre margen — de mange mellemrum i kilden har ingen effekt.

## 15. Phrase Elements

Angiver konteksten og betydningen af teksten.

| Element | Eksempel | Anvendelse |
|---|---|---|
| `<b>` | **bold text** | Tekst der ikke har ekstra vigtighed, men styles med fed skrift efter brug og konvention |
| `<em>` | *emphasized text* | Får tekst til at blive fremhævet i forhold til anden tekst; vises normalt i kursiv |
| `<i>` | *italicized text* | Tekst der ikke har ekstra vigtighed, men styles i kursiv efter brug og konvention |
| `<mark>` | mark text (vist med gul overstregning på slidet) | Tekst der er fremhævet for let at kunne refereres |
| `<small>` | small text | Juridiske forbehold og noter ("fine print") vist med lille font-size |
| `<strong>` | **strong text** | Stor vigtighed; får tekst til at skille sig ud fra omgivende tekst; vises normalt i fed |
| `<sub>` | sub text (sænket) | Viser et subscript som lille tekst under baseline |
| `<sup>` | sup text (hævet) | Viser et superscript som lille tekst over baseline |

Bemærk parvist: `<b>` og `<strong>` ser ens ud, men `<strong>` bærer betydning (semantik), mens `<b>` kun er visuel. Samme forhold mellem `<i>` og `<em>`.

## 16. HTML Lists

- Unordered List
- Ordered List
- Description List (tidligere kaldet en definition list)

### Unordered List

Viser en bullet, eller list marker, før hvert element i listen.

- `<ul>` indeholder den uordnede liste.
  - `type`-attributten bestemmer typen af bullet point.
  - Default type er `disc` (men afhænger af browseren).
- `<li>` indeholder et element i listen.

```html
<ul>
   <li>TCP</li>
   <li>IP</li>
   <li>HTTP</li>
   <li>FTP</li>
 </ul>
```

Renderet i browseren vises de fire punkter under hinanden, hver med en sort udfyldt cirkel (disc) foran, og hele listen er indrykket fra venstre margen.

### Ordered List

Viser et nummererings- eller bogstavsystem til at opremse informationen i listen.

- `<ol>` indeholder den ordnede liste.
  - `type`-attributten bestemmer listens nummereringsskema, default er tal.
- `<li>` indeholder et element i listen.

```html
<ol>
   <li>Apply to school</li>
   <li>Register for course</li>
   <li>Pay tuition</li>
   <li>Attend course</li>
</ol>
```

Renderet vises de fire punkter nummereret "1. Apply to school", "2. Register for course", "3. Pay tuition", "4. Attend course" — browseren tildeler tallene automatisk, de står ikke i HTML'en.

### Description List

Nyttig til at vise en liste af termer og beskrivelser, eller en liste af FAQ og svar.

- `<dl>` indeholder description list'en.
- `<dt>` indeholder en term/frase/sætning. Konfigurerer tom plads over og under teksten.
- `<dd>` indeholder en beskrivelse af termen/frasen/sætningen.
  - Indrykker teksten.
  - Konfigurerer tom plads over og under teksten.

```html
<dl>
   <dt>IP</dt>
        <dd>Internet Protocol</dd>
    <dt>TCP</dt>
         <dd>Transmission Control Protocol</dd>
</dl>
```

Renderet ser det sådan ud: "IP" står ude ved venstre margen, og under den er "Internet Protocol" indrykket. Derefter står "TCP" ude ved margenen med "Transmission Control Protocol" indrykket under. Der er ingen bullets eller numre — kun indrykningen adskiller term fra beskrivelse.

## 17. Special Characters

Viser specialtegn som anførselstegn, copyright-symbol osv.

| Tegn | Kode |
|---|---|
| © | `&copy;` |
| < | `&lt;` |
| > | `&gt;` |
| & | `&amp;` |

Indsæt whitespace af forskellig bredde:

| Kode | Betydning |
|---|---|
| `&nbsp;` | Non breaking space |
| `&ensp;` | Bredden af to normale mellemrum |
| `&emsp;` | Cirka fire normale mellemrum |

`&lt;` og `&gt;` er nødvendige netop fordi `<` og `>` ellers ville blive tolket som tags.

## 18. Div Element

- Konfigurerer et strukturelt blok-område eller en "division" på en webside med tom plads over og under.
- Kan indeholde andre block display-elementer, inklusive andre div-elementer.

```html
<div>Home Services Contact</div>
```

## 19. span Element

**Formål:**

- Konfigurerer et specielt formateret område der vises **in-line** med andre elementer, fx inde i et afsnit.
- Bruges hovedsageligt til at ændre stilen på ord.

Der er **ingen** ekstra tom plads over eller under et span — det er inline display.

```html
<p>Here are some text <span class="highlight">and
here comes some important text</span>. Now we are
back to some plain text again.</p>
```

## 20. Block elements vs inline elements

**Block elements**

- Fx: `div`, `p`, heading-elementer, lister og list items.
- Block elements tillader, når de placeres på siden, ikke andre elementer ved siden af sig. De optager containerens fulde bredde.
- Nogle block elements (fx `div`) kan indeholde andre block elements.

**Inline elements**

- Fx: `a`, `span` og `img`.
- Kan sidde ved siden af andre inline elements.

Bemærk at vi med CSS kan ændre defaulten for hvert element:

- Sætte et `p`-tag til at være inline.
- Sætte et `span` til at være et block element.

## 21. Anchor Element

- Angiver en hyperlink-reference (`href`) til en fil.
- Teksten mellem `<a>` og `</a>` vises på websiden.

```html
<a href="contact.html">Contact Us</a>
```

`href`-attributten angiver filnavnet eller URL'en.

### Absolute & Relative Hyperlinks

**Absolut link** — link til andre websites:

```html
<a href="http://au.dk">Aarhus University</a>
```

**Relativt link** — link til sider på dit eget site:

```html
<a href="index.htm">Home</a>
```

Slidet fremhæver: **No http!!!** — et relativt link må ikke have protokol og domæne foran, ellers er det ikke relativt længere.

### Opening Link in a New Window

`target`-attributten på anchor-elementet åbner et link i et nyt browservindue eller en ny browserfane.

```html
<a href="http://au.dk" target="_blank">Aarhus University</a>
```

### Linking to Fragments

Et link til en del af en webside — også kaldet named fragments eller fragment ids. To komponenter:

1. Elementet der identificerer det navngivne fragment af websiden. Dette kræver `id`-attributten:

```html
<div id="top"> ….. </div>
```

2. Anchor-tagget der linker til det navngivne fragment af websiden. Dette bruger `href`-attributten med et `#` foran:

```html
<a href="#top">Back to Top</a>
```

### E-Mail Hyperlink

Starter automatisk det default mailprogram der er konfigureret for browseren. Hvis der ikke er konfigureret en browser-default, vises en besked.

```html
<a href="mailto:me@gmail.com">me@gmail.com</a>
```

## 22. Write Valid HTML

- Tjek din kode for syntaksfejl!
- Valid kode ⇒ mere konsistent browser-visning.
- Brug en editor med validering.
- Eller brug W3C XHTML Validation Tool: http://validator.w3.org

## 23. HTML5 Taxonomy & Status (forældet oversigtsdiagram)

Slide 32 viser et stort cirkulært taxonomi-diagram med titlen "HTML5 — Taxonomy & Status (October 2014)". Diagrammet består af tre koncentriske ringe:

- Inderst **Initial WHATWG HTML5 specification** med bobler som HTML5 Markup, Canvas 2D, Audio Video, Web Messaging, Web Sockets, Web Workers, Drag and Drop og Microdata.
- Midterringen **W3C HTML5 specification** med Web SQL, Web Storage, HTML+RDFa og HTML 5.1.
- Yderringen **HTML5 & related technologies** med CSS3, MathML 3.0, SVG, Selectors L1, Navigation/User Timing, RDFa, Geo Location, WAI-ARIA, Touch Events, Animation Timing, WOFF 1.0, Media Capture, Indexed Database, File API, Battery Status, XmlHTTPRequest 1, Device Orientation, JavaScript og WebGL.

Farvekoden på boblerne angiver status: grøn = Recommendation/Proposed, olivengrøn = Candidate Recommendation, orange = Last Call, rød = Working Draft, magenta = Non-W3C Specifications, grå = Deprecated or inactive.

Slidet er markeret med en stor rød ellipse: **"This picture is outdated!"** Pointen er altså ikke at lære diagrammet udenad, men at forstå at HTML5 er en paraply over mange separate specifikationer med forskellig modenhed — og at statusbilledet fra 2014 for længst er overhalet, netop fordi HTML5 nu er en Living Standard.

## 24. Foretræk MDN-dokumentation

Slide 33 viser et Google-søgeresultat for "localstorage". Øverst i resultatlisten står `https://developer.mozilla.org` med "Window.localStorage - Web APIs | MDN", og under den flere W3Schools-resultater. En stor callout peger på MDN-resultatet: **"Whenever you google something web related you should prefer the links to developer.mozilla.org"**.

<!-- callout'en staver domænet "developer.mozille.org"; det korrekte er developer.mozilla.org -->

## 25. Setup your Web Server

Til udvikling kan vi bruge en simpel node-baseret http-server.

Installation i en PowerShell med administratorrettigheder:

```bash
npm install http-server -g
```

Kørsel fra projektmappen:

```bash
http-server
```

Terminal-output som vist på slidet:

```
Starting up http-server, serving ./
Available on:
  http://172.21.236.241:8080
  http://169.254.80.80:8080
  http://172.16.80.1:8080
  http://10.24.128.217:8080
  http://127.0.0.1:8080
Hit CTRL-C to stop the server
```

Serveren serverer altså den aktuelle mappe (`./`) på port 8080 — `http://127.0.0.1:8080` er den adresse man selv bruger lokalt, mens de øvrige er maskinens netværksinterfaces.

## 26. References & Links

- "Web Development and Design Foundations with HTML5" af Terry Felke-Morris
- Web Fundamentals — Googles site dedikeret til at hjælpe udviklere med at bygge bedre web apps: https://developers.google.com/web (vises i Chrome)
- Validation tool: http://validator.w3.org
