# L14 – Bootstrap

## Metadata

- **Lektion:** L14 – Bootstrap (One framework, every device)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L14/FED Bootstrap.pdf (23 slides)
- **Emner dækket:**
  - Hvad Bootstrap er og dets historie
  - Måder at hente Bootstrap: download, NuGet, npm, CDN
  - Brug af v4 og v5 via CDN
  - Komponenter der kræver jQuery, Bootstrap JS og Popper.js
  - Ændringerne i Bootstrap 5
  - Containers: `container` vs. `container-fluid`
  - Grid-systemet og breakpoints/tiers
  - Responsive utilities
  - Fonts, headings og icons
  - Components og Cards
  - Customizing Bootstrap med Sass

---

## 1. Hvad er Bootstrap?

- Bootstrap er et CSS- og JavaScript-UI-framework.
- Det er designet til at hjælpe dig, der ikke er designer-type, med at balancere dine layouts og designs.
- Bootstrap skalerer nemt og effektivt dine websites og applikationer med én enkelt kodebase.

Det blev skrevet af to af Twitters senior developers for at sikre et konsistent look and feel på tværs af alle de projekter, de lavede til Twitter.

- I august 2010 udgivet som open source-projekt på GitHub.
- Siden februar 2012 har det været det mest stjernemarkerede udviklingsprojekt på GitHub.

## 2. Hvordan får man fat i det?

- Download den seneste stabile version fra http://getbootstrap.com
- Brug NuGet.
- Brug npm.
- Brug et CDN.

Aktuel version i slidesene er v5.1.3.

### Kodeeksempel — CDN (Bootstrap 3)

```html
<!-- Latest compiled and minified CSS -->
<link rel="stylesheet"
href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">

<!-- Optional theme -->
<link rel="stylesheet"
href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css">

<!-- Latest compiled and minified JavaScript -->
<script
src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js">
</script>
```

## 3. How to Use — V. 4.x

Når man kun har brug for at inkludere Bootstraps kompilerede CSS eller JS, kan man bruge Bootstrap CDN.

### Kodeeksempel — CSS only

```html
<link rel="stylesheet"
href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css"
integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4"
crossorigin="anonymous">
```

### Kodeeksempel — JS, Popper.js og jQuery

```html
<script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo"
crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"
integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ"
crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"
integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm"
crossorigin="anonymous"></script>
```

## 4. Hvilke komponenter kræver jQuery, Bootstrap JS og Popper.js?

- **Alerts** for dismissing.
- **Buttons** for toggling states og checkbox/radio-funktionalitet.
- **Carousel** for alle slide behaviors, controls og indicators.
- **Collapse** for toggling af content-synlighed.
- **Dropdowns** for displaying og positioning (kræver også Popper.js).
- **Modals** for displaying, positioning og scroll behavior.
- **Navbar** for at udvide Collapse-plugin'et med responsiv adfærd.
- **Tooltips og popovers** for displaying og positioning (kræver også Popper.js).
- **Scrollspy** for scroll behavior og navigationsopdateringer.

### Kodeeksempel — Modal

```html
<div class="modal" tabindex="-1" role="dialog">
 <div class="modal-dialog" role="document">
  <div class="modal-content">
   <div class="modal-header">
     <h5 class="modal-title">Modal title</h5>
    <button type="button" class="close"
                  data-dismiss="modal" aria-label="Close">
      <span aria-hidden="true">&times;</span>
    </button>
   </div>
   <div class="modal-body">
    <p>Modal body text goes here.</p>
   </div>
   <div class="modal-footer">
    <button type="button" class="btn btn-primary">Save changes</button>
    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
   </div>
```

<!-- uklart i kilden: slide 5 er beskåret i højre side, de afsluttende tags mangler -->

## 5. Ændringerne i Bootstrap 5

- jQuery blev fjernet.
- Skift til Vanilla JavaScript.
- Responsive Font Sizes.
- Drop af support for Internet Explorer 10 og 11.
- Ændring af måleenheden for gutter width.
- Card Decks fjernet.
- Navbar Optimization.
- Custom SVG icon library.

Bootstrap-teamet tager store skridt for at gøre frameworket lightweight, simpelt, nyttigt og hurtigere til udviklerens fordel.

## 6. How to Use — V. 5.x

### Kodeeksempel — CSS

```html
<!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css"
      rel="stylesheet" integrity="sha384-eOJMYsd53ii+scO/bJGFsiCZc+5NDVN2yr8+0RDqr0Ql0h+rP48ckxlpbzKgwra6"
crossorigin="anonymous">
```

### Kodeeksempel — JS og Popper.js

```html
<!-- Optional JavaScript; choose one of the two! -->
<!-- Option 1: Bootstrap Bundle with Popper -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/js/bootstrap.bundle.min.js"
integrity="sha384-JEW9xMcG8R+pH31jmWH6WWP0WintQrMb4s7ZOdauHnUtxwoG2vI5DkLtS3qm9Ekf"
crossorigin="anonymous"></script>
<!-- Option 2: Separate Popper and Bootstrap JS -->
<!-- <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.1/dist/umd/popper.min.js"
integrity="sha384-SR1sx49pcuLnqZUnnPwx6FCym0wLsk5JZuNx2bPPENzswTNFaQU1RDvt3wT4gWFG"
crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/js/bootstrap.min.js"
integrity="sha384-j0CNLUeiqtyaRmlzUHCPZ+Gy5fQu0dQ6eZ/xAww941Ai1SxSY+0EQqNXNE6DZiVc"
crossorigin="anonymous"></script> -->
```

## 7. Bootstrap layout — overblik

Bootstrap-layoutet består af:

- wrapping containers
- a powerful grid system
- a flexible media object
- responsive utility classes

## 8. Containers

Containers er påkrævet, når man bruger grid-systemet. Der er to valg:

- `class="container"` — fixed-width container. Dens `max-width` ændres ved hvert breakpoint.
- `class="container-fluid"` — fluid-width. 100 % bred hele tiden. Denne foretrækkes.

### Kodeeksempel

```html
<div class="container">
  <!-- Content here -->
</div>
```

```html
<div class="container-fluid">
  <!-- Content here -->
</div>
```

## 9. The grid

Grid'et skaber page layouts med en serie af rows og columns, som huser indholdet. Blokke af indhold skabes ved at angive antallet af kolonner, man ønsker at spænde over.

## 10. The Grid System

- Tre lige brede kolonner startende ved desktops og skalerende til large desktops: `.col-md-4` `.col-md-4` `.col-md-4`. På mobile enheder, tablets og derunder stables kolonnerne automatisk.
- Tre kolonner af varierende bredde: `.col-md-3` `.col-md-6` `.col-md-3`.
- To kolonner med to nestede kolonner: `.col-md-8` og `.col-md-4`, hvor `.col-md-8` indeholder `.col-md-6` og `.col-md-6`.

Grid-kolonner skal lægge sammen til tolv for én enkelt horisontal blok.

## 11. Fem (fire) tiers af grids

De centrale breakpoints i grid-systemet.

**V4**

| | Extra small devices (portrait phones) | Small devices (landscape phones) | Medium devices (tablets) | Large devices (Desktops) | Extra large devices (large desktops) |
| --- | --- | --- | --- | --- | --- |
| Bootstrap name | xs | sm | md | lg | xl |
| Class prefix | `.col-` | `.col-sm-` | `.col-md-` | `.col-lg-` | `.col-xl-` |
| Container min-width | None (auto) | 576px | 768px | 992px | 1200px |

**V3**

| | Extra small devices (Phones) | Small devices (Tablets) | Medium devices (Desktops) | Large devices (Desktops) |
| --- | --- | --- | --- | --- |
| Bootstrap name | xs | sm | md | lg |
| Class prefix | `.col-xs-` | `.col-sm-` | `.col-md-` | `.col-lg-` |
| Container min-width | None (auto) | 750px | 970px | 1170px |

## 12. Use of Grids

Man kan blande mobile, tablet og desktop i den samme markup ved at kombinere flere class-prefixer på det samme element.

- Række 1: `.col-xs-12 .col-md-8` og `.col-xs-6 .col-md-4`
- Række 2: `.col-xs-6 .col-md-4`, `.col-xs-6 .col-md-4`, `.col-xs-6 .col-md-4`
- Række 3: `.col-xs-6` og `.col-xs-6`

## 13. Responsive utilities

| Klasse | Extra small devices Phones (<768px) | Small devices Tablets (≥768px) | Medium devices Desktops (≥992px) | Large devices Desktops (≥1200px) |
| --- | --- | --- | --- | --- |
| `.visible-xs-*` | Visible | Hidden | Hidden | Hidden |
| `.visible-sm-*` | Hidden | Visible | Hidden | Hidden |
| `.visible-md-*` | Hidden | Hidden | Visible | Hidden |
| `.visible-lg-*` | Hidden | Hidden | Hidden | Visible |
| `.hidden-xs` | Hidden | Visible | Visible | Visible |
| `.hidden-sm` | Visible | Hidden | Visible | Visible |
| `.hidden-md` | Visible | Visible | Hidden | Visible |
| `.hidden-lg` | Visible | Visible | Visible | Hidden |

## 14. Fonts

- For at få moderne, smukke fonte med nul latency bruger Bootstrap v4 **system UI fonts**.
- Denne `font-family` anvendes på `<body>` og arves automatisk globalt gennem hele Bootstrap.
- Fallback er `"Helvetica Neue", Arial, sans-serif !default;`

Reference: https://www.smashingmagazine.com/2015/11/using-system-ui-fonts-practical-guide/

## 15. Headings

Styles for headings 1 til 6. I version 4 er størrelserne:

| Heading | Størrelse |
| --- | --- |
| h1 | Semibold 2.5rem (40px) |
| h2 | Semibold 2rem (32px) |
| h3 | Semibold 1.75rem (28px) |
| h4 | Semibold 1.5rem (24px) |
| h5 | Semibold 1.25rem (20px) |
| h6 | Semibold 1rem (16px) |

## 16. Icons

- Bootstrap v3 inkluderede Glyphicons.
- I Bootstrap v4 skal man selv inkludere icons, for eksempel Font Awesome.
- Søg efter icons her: https://glyphsearch.com
- Bootstrap v5:
  - Gratis, high quality, open source icon library med over 1.300 icons.
  - Inkludér dem som du vil — SVG'er, SVG sprite eller web fonts.
  - Brug dem med eller uden Bootstrap i ethvert projekt.
  - https://icons.getbootstrap.com/

## 17. Components

Over et dusin genbrugelige komponenter med kodeeksempler til copy-paste. Se https://getbootstrap.com/docs/4.1/examples/ eller https://getbootstrap.com/docs/5.0/components/buttons/

- Navs
- Navbar
- Breadcrumbs
- Modal
- Collapse
- Tooltip
- med flere

## 18. Cards i Bootstrap 4

Et card er en fleksibel og udvidelig content container. Det inkluderer muligheder for headers og footers, en bred vifte af indhold, contextual background colors og kraftfulde display-muligheder.

### Kodeeksempel

```html
<div class="container">
  <div class="row">
     <div class="card">
       <div class="card-body">
          <h3>I am the body of a very basic card</h3>
       </div>
     </div>
  </div>
</div>
```

Reference: http://getbootstrap.com/docs/4.1/components/card/

## 19. Use of Card

### Kodeeksempel

```html
<div class="row">
 <div class="card">
  <div class="card-header">Header</div>
  <img class="card-img-top"
       src="images/css-is-awesome.jpg">
  <div class="card-body">
   <h3 class="card-title">
    I am the body of a not-so-simple card
   </h3>
   <h4 class="card-subtitle">
    I am the subtitle
   </h4>
   <p class="card-text">
    I am plain text
   </p>
   <a href="#" class="card-link">First link</a>
   <a href="#" class="card-link">Second link</a>
   <a href="#" class="card-link">Third link</a>
  </div>
  <div class="card-footer">Footer</div>
 </div>
</div>
```

## 20. Card decks

Card decks bruges i Bootstrap 4 til at gruppere flere cards. De er ikke nødvendige i v5.

### Kodeeksempel

```html
<div class="card-deck">
  <div class="card">
    <img
   ...
```

<!-- uklart i kilden: slide 21 er beskåret -->

## 21. Customizing Bootstrap

Kompilér Bootstrap med din egen asset pipeline ved at downloade Bootstraps source Sass-, JavaScript- og dokumentationsfiler. Denne mulighed kræver noget ekstra tooling:

- Sass compiler (Libsass eller Ruby Sass understøttes) til at kompilere din CSS.
- Autoprefixer til CSS vendor prefixing.

Download sass-kilderne: https://getbootstrap.com/docs/4.1/getting-started/download/

## 22. References & Links

- http://getbootstrap.com/
- W3schools — http://www.w3schools.com/bootstrap/default.asp
- Design Systems Handbook — https://www.designbetter.co/design-systems-handbook
- Alternatives to Bootstrap:
  - Material Design — https://material.io/design/
  - Material Design Lite — https://getmdl.io/index.html
  - Bulma — https://bulma.io/
  - Foundation — http://foundation.zurb.com/
  - Semantic UI — http://semantic-ui.com/usage/layout.html
  - Flexbox — https://scotch.io/tutorials/a-visual-guide-to-css3-flexbox-properties og https://css-tricks.com/snippets/css/a-guide-to-flexbox/
  - CSS Grid Layout — http://www.sitepoint.com/introducing-the-css-grid-layout/
