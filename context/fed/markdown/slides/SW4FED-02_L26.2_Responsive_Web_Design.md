# L26 – Responsive Web Design

## Metadata

- **Lektion:** L26 – Responsive Web Design
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L26/Responsive Web Design.pdf (38 slides)
- **Emner dækket:**
  - Mobile Web Design og de tre tilgange til mobil-web
  - Begrænsninger på mobil og designteknikker
  - Viewport meta tag, telefon- og SMS-links
  - Performance: rendering under ét sekund, optimeret sidestruktur
  - Responsive Web Design: mål og de tre byggeklodser
  - CSS3 media queries, breakpoints og retina-queries
  - Flexbox vs. CSS Grid, grid areas med media queries
  - Mobile first med Tailwind og dets breakpoint-præfikser
  - CSS styling til print

---

## 1. Agenda

- Mobile Web Design
- Responsive Web Design
- CSS Styling for Print

## 2. Smartphones dominerer webbet

Det er simpelthen utænkeligt at forestille sig et webudviklingsfirma, der undlader at lave et responsivt site. Sliden viser statistik over platform market share (desktop/mobile/tablet) fra https://gs.statcounter.com/platform-market-share/desktop-mobile-tablet.

Blandt mobile browsere konkurrerer Safari med Chrome Mobile — se https://gs.statcounter.com/browser-market-share/mobile/denmark.

## 3. Tre tilgange til mobil-web

1. **Separate URLs** (også kendt som mobile-friendly)
   - Opret et separat website hostet inden for det nuværende domæne, målrettet mobilbrugere, som fx `m.whitehouse.gov`, `m.jyllands-posten.dk`, `mobil.bold.dk`

2. **Dynamic serving** (også kendt som adaptive)
   - Sites, der dynamisk serverer alle enheder på det samme sæt URLs, men hvor hver URL serverer forskellig HTML og CSS afhængigt af, om user agent er en desktop eller en mobil enhed

3. **Responsive web design**
   - Brug CSS til at konfigurere det nuværende website til visning på både mobil, tablets og desktop
   - Dette er Googles anbefalede konfiguration

Reference: https://developers.google.com/webmasters/smartphone-sites/details

### Mobilstrategi brugt af forskellige websites

| Configuration | Same URLs | Same HTML | Used by |
| --- | --- | --- | --- |
| Responsive web design | Yes | Yes | 52% |
| Dynamic serving (adaptive) | Yes | No | 5% |
| Separate URLs (mobile-friendly) | No | No | 16% |
| Not mobile friendly | Yes | Yes | 27% |

Statistikken er fra marts 2017 (Smashing Magazine).

## 4. Mobile Web Design — begrænsninger

- Lille skærmstørrelse
- Touch og ingen mus

Og måske også:

- Lav båndbredde
- Begrænsede fonts
- Pris per kilobyte
- Begrænset processor og hukommelse

## 5. Designteknikker til mobil-web

- Single column design
- Undgå floats, tables, frames
- Beskrivende page title
- Beskrivende heading-tags
- Optimér billeder
- Beskrivende alt-tekst til billeder
- Fjern unødvendige billeder
- Navigation i lister
- `rem`, `em` eller procent som font size-enheder
- Almindelige font-typefaces
- God kontrast mellem tekst- og baggrundsfarver
- Tilbyd et "Skip to Content"-hyperlink
- Tilbyd et "Back to Top"-hyperlink

De to sidste hvor det er relevant.

## 6. Viewport Meta Tag

Standardadfærden for de fleste mobile enheder er at zoome ud og skalere websiden.

Viewport Meta Tag blev skabt som en Apple-extension til at konfigurere visning på mobile enheder. Det konfigurerer bredde og initial scale af browserens viewport.

### Kodeeksempel

```html
<meta name="viewport" content="width=device-width, initial-scale=1.0">
```

Læs mere: https://developer.mozilla.org/en-US/docs/Mozilla/Mobile/Viewport_meta_tag

## 7. Telefon- og SMS-hyperlinks

**Telephone Scheme** — mange mobile browsere initierer et telefonopkald, når linket klikkes:

```html
<a href="tel:888-555-5555">Call 888-555-5555</a>
```

**SMS Scheme** — mange mobile browsere initierer en tekstbesked til telefonnummeret, når linket klikkes:

```html
<a href="sms:888-555-5555">Text 888-555-5555</a>
```

## 8. Gør dine mobile sider hurtigere

Vi ved, at en brugers tankeproces typisk afbrydes efter blot ét sekunds ventetid, hvilket får brugeren til at blive disengageret.

Så som minimum: "above the fold"-indholdet på en webside bør renderes på under ét sekund.

### Rendering på under ét sekund

Hvis vi estimerer 3G-netværkets round trip time til 250 ms:

- 4G RTT er 55 ms (20–170 afhængigt af operatør og placering)
- 5G RTT er typisk 25 ms (10–100 afhængigt af operatør og placering)
- Cable RTT er 21 ms

Under antagelse af ingen blokerende ekstern JavaScript eller CSS pådrager vi os tre round trips til DNS, TCP og request/response, i alt 750 ms (75 ms for 5G). Plus 100 ms backend-tid bringer det os til 850 ms (200 ms for 5G).

Så længe render-blocking JavaScript og CSS er inlinet, og størrelsen af den initiale HTML-payload holdes til et minimum (fx under 60 kB komprimeret), bør tiden til at parse og rendere være et godt stykke under 100 ms, hvilket bringer os til 950 ms — lige under vores ét-sekunds-mål.

Brug Lighthouse i Chrome til at måle.

Referencer:
- http://calendar.perfplanet.com/2012/make-your-mobile-pages-render-in-under-one-second/
- http://calendar.perfplanet.com/2013/network-latency-4g/

## 9. Optimeret sidestruktur

```html
<html>
<head>
  <style>
    .main { ... }
    .leftnav { ... }
    /* ..any other styles needed for the initial render here */
  </style>
  <script>
    // Any script needed for initial render here.
    // Ideally, there should be no JS needed for the initial render
  </script>
</head>
<body>
  <div class="topnav"> Perhaps there is a top nav bar here. </div>
  <div class="main">
      Here is my content. </div>
  ...
  <link rel="stylesheet" href="my_leftover.css">
  <script src="my_leftover.js"></script>
</body>
</html>
```

Scriptet kan flyttes op i toppen med `defer`:

```html
<script defer src="my_leftover.js"></script>
```

## 10. Speed Summary

For at få din mobile webside til at rendere på under ét sekund bør du:

- Holde server backend-tiden til at generere HTML på et minimum (under 100 ms)
- Undgå HTTP redirects for HTML-hovedressourcen
- Undgå at loade blokerende ekstern JavaScript og CSS før den initiale render
- Inline præcis den JavaScript og CSS, der er nødvendig for den initiale render
- Forsinke eller async-loade al JavaScript og CSS, der ikke er nødvendig for den initiale render, med `defer`
- Holde HTML-payloaden, der kræves for at rendere det initiale indhold, under 60 kB komprimeret

## 11. "Mobile friendly" websites

Krav:

- Tekst er læsbar uden zooming
- Indhold er dimensioneret, så horisontal og vertikal scrolling ikke er nødvendig
- Links er placeret langt nok fra hinanden til nem tapping

Selvom det ikke er et krav at have en mobilversion af sine sider for at få sit indhold inkluderet i Googles søgeresultater, anbefales det meget kraftigt. Guide: https://developers.google.com/search/mobile-sites

Test af mobilvisning: brug browserens developer tools (tryk F12, inspect eller "undersøg").

## 12. Responsive Web Design

Responsive web design er en webdesign-tilgang, der sigter mod at udforme sites, som giver en optimal viewing experience — nem læsning og navigation med et minimum af resizing, panning og scrolling — på tværs af et bredt udvalg af enheder (fra mobiltelefoner til desktop-computerskærme). (Wikipedia)

### Hvorfor Responsive Design

- At bruge en enkelt URL for et stykke indhold gør det lettere for brugerne at interagere med, dele og linke til indholdet, og en enkelt URL hjælper Googles algoritmer med at tildele indekseringsegenskaber for indholdet
- Ingen redirection er nødvendig for at brugere kan komme til den device-optimerede visning, hvilket reducerer loading time. Desuden er user agent-baseret redirection fejlbehæftet og kan forringe sitets user experience
- Det sparer ressourcer for både dit site og Googles crawlere

### Målet for Responsive Design

1. Siderne skal rendere læsbart ved enhver skærmopløsning — fra 320 px og op
2. Vi markerer ét sæt indhold op, som kan vises på enhver enhed — kun én version af hver html-fil
3. Vi bør aldrig vise en horisontal scrollbar, uanset vinduesstørrelse — fra 320 px og op

### Hvordan?

Et site designet med RWD tilpasser layoutet til viewing-miljøet ved at bruge:

1. CSS3 media queries
2. Fluid, proportion-based grid layout — eller brug Flexbox
3. Flexible images

## 13. CSS3 Media Queries

Media queries bestemmer den mobile enheds capability, såsom skærmopløsning, og dirigerer browseren til styles konfigureret specifikt til disse capabilities.

### Kodeeksempel — via link-element

```html
<link href="lighthousemobile.css" rel="stylesheet"
      media="only screen and (max-width: 640px)">
```

### Kodeeksempel — som @media-regel i CSS

```css
@media screen and (max-width: 640px){
  .column {
    float: none;
  }
}
```

### Retina — media query til high-resolution display

```css
@media screen and (min-resolution: 2dppx) {
  /* CSS goes here */
}
```

Læs mere: https://developer.mozilla.org/en-US/docs/Web/CSS/Media_Queries/Using_media_queries

## 14. Media Queries "Magic Numbers"

Man behøver ikke gætte de magiske tal i media queries — søg på nettet, og man finder fungerende media queries, fx https://css-tricks.com/snippets/css/media-queries-for-standard-devices/. Eller google "Responsive web design". Eller brug Bootstrap, Tailwind eller lignende.

### Responsive — Bootstrap

Man behøver ikke genopfinde hjulet — brug twitter bootstrap (http://getbootstrap.com/). Bootstrap blev lavet til ikke kun at se godt ud og opføre sig godt i de nyeste desktop-browsere, men også i tablet- og smartphone-browsere via responsiv CSS. Gratis HTML-snippets til Bootstrap: http://bootsnipp.com/

Eller brug css grid og/eller flexbox: https://css-tricks.com/snippets/css/a-guide-to-flexbox/

## 15. Flexbox eller CSS Grid

- **Brug flexbox** hvis man kun har brug for at kontrollere layout per række *eller* kolonne
- **Brug css grid** hvis man har brug for at kontrollere per række *og* kolonne

Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Grid_Layout/Relationship_of_Grid_Layout

## 16. Redefinering af grid areas med media queries

### Kodeeksempel — markup

```html
<div class="container">
<div class="box header">Header</div>
<div class="box sidebar">Sidebar</div>
<div class="box sidebar2">Sidebar 2</div>
<div class="box content">Content
<br /> More content than we had before so this column is now quite tall.</div>
<div class="box footer">Footer</div>
</div>
```

### Kodeeksempel — CSS

```css
.sidebar {
  grid-area: sidebar;
}
.sidebar2 {
  grid-area: sidebar2;
}
.content {
  grid-area: content;
}

.container {
  display: grid;   grid-gap: 1em;
  grid-template-areas:
    "header"
    "sidebar"
    "content"
    "sidebar2"
    "footer"
}

@media only screen and (min-width: 500px) {
  .container {
    grid-template-columns: 20% auto;
    grid-template-areas:
      "header header"
      "sidebar content"
      "sidebar2 sidebar2"
      "footer footer";
  }
}

@media only screen and (min-width: 800px) {
  .container {     grid-gap: 20px;
    grid-template-columns: 120px auto 120px;
    grid-template-areas:
      "header header header"
```

<!-- Den sidste media query er afkortet på sliden; slide 29 viser det renderede resultat -->

## 17. Mobile first med Tailwind

### Breakpoints

| Breakpoint prefix | Minimum width | CSS |
| --- | --- | --- |
| `sm` | 40rem (640px) | `@media (width >= 40rem) { ... }` |
| `md` | 48rem (768px) | `@media (width >= 48rem) { ... }` |
| `lg` | 64rem (1024px) | `@media (width >= 64rem) { ... }` |
| `xl` | 80rem (1280px) | `@media (width >= 80rem) { ... }` |
| `2xl` | 96rem (1536px) | `@media (width >= 96rem) { ... }` |

Dette virker for hver utility class i frameworket.

### Working mobile-first

Tailwind bruger et mobile-first breakpoint-system:

- Uprefixede utilities (som `uppercase`) træder i kraft på alle skærmstørrelser, mens prefixede utilities (som `md:uppercase`) kun træder i kraft ved det angivne breakpoint og opefter
- For at style noget til mobil skal man bruge den uprefixede version af en utility — ikke `sm:`-versionen

```html
<!-- This will center text on mobile, and left align it on
screens 640px and wider -->
<div class="text-center sm:text-left"></div>
```

### Kodeeksempel — responsivt kort i Tailwind

```html
<div class="mx-auto max-w-md overflow-hidden rounded-xl bg-white shadow-md md:max-w-2xl">
  <div class="md:flex">
    <div class="md:shrink-0">
       <img
         class="h-48 w-full object-cover md:h-full md:w-48"
         src="/img/building.jpg"
         alt="Modern building architecture"
       />
    </div>
    <div class="p-8">
       <div class="text-sm font-semibold tracking-wide text-indigo-500 uppercase">Company retreats</div>
       <a href="#" class="mt-1 block text-lg leading-tight font-medium text-black hover:underline">
         Incredible accommodation for your team
       </a>
       <p class="mt-2 text-gray-500">
         Looking to take your team away on a retreat to enjoy awesome food and take in some sunshine? We have a list of
         places to do just that.
       </p>
    </div>
  </div>
</div>
```

## 18. CSS Styling for Print

- Opret et eksternt style sheet med konfigurationerne til browser-visning
- Opret et andet eksternt style sheet med konfigurationerne til print
- Forbind begge eksterne style sheets til websiden med to `<link>`-elementer

```html
<link rel="stylesheet" href="wildflower.css" type="text/css" media="screen">
<link rel="stylesheet" href="wildflowerprint.css" type="text/css" media="print">
```

### Print Styling Best Practices

**Skjul ikke-essentielt indhold:**

```css
#nav { display: none; }
```

**Konfigurér font size og farve til print** — brug `pt` som font size-enhed og en mørk tekstfarve.

**Kontrollér page breaks:**

```css
.newpage { page-break-before: always; }
```

**Print URLs for hyperlinks:**

```css
#sidebar a:after { content: " (" attr(href) ") "; }
```

### Mere om print

- Designing For Print With CSS — https://www.smashingmagazine.com/2015/01/designing-for-print-with-css/
- Generér en pdf-fil — https://www.html-to-pdf.net/

## 19. References & Links

- "Web Development and Design Foundations with HTML5"
- "HTML5 & CSS3 for the Real World"
- Logical Breakpoints For Your Responsive Design (brug em) — http://www.smashingmagazine.com/2013/03/01/logical-breakpoints-responsive-design/
- Responsive design — https://developers.google.com/web/fundamentals/design-and-ux/responsive/ og http://www.smashingmagazine.com/2013/05/06/new-defaults-web-design/
- The Front-End Checklist — https://frontendchecklist.io/
- https://tailwindcss.com/
