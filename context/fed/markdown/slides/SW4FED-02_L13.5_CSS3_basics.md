# L13 – CSS3 basics

## Metadata

- **Lektion:** L13 – CSS3 Basics (Cascading Style Sheets)
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED CSS3 basics.pdf (38 slides)
- **Emner dækket:**
  - Hvad CSS er og fordelene ved det
  - Fire steder at placere CSS i et HTML-dokument
  - The Cascade og specificity
  - CSS-syntaks: selector og declaration
  - Inline, embedded og external CSS
  - Farver: hex, rgb/rgba, color names, hsl/hsla
  - Tekst-properties og `font-size`-enheder
  - Selectors: element, class, id, contextual, attribute
  - Pseudo-classes og pseudo-elements
  - CSS-validering

---

## 1. Overview of CSS

Cascading Style Sheets (CSS) leverer funktionaliteten af style sheets (og meget mere) til webudviklere. Det er et fleksibelt, cross-platform, standards-based sprog udviklet af W3C.

Se hvad der er muligt med CSS på http://www.csszengarden.com

## 2. Fordele ved CSS

- Større kontrol over typografi og page layout.
- Style er adskilt fra struktur.
- Styles kan gemmes i et separat dokument og associeres med websiden.
- Potentielt mindre dokumenter.
- Lettere site maintenance.

## 3. Fire steder til CSS i et HTML-dokument

**Inline Styles**

- I body-sektionen.
- HTML `style`-attributten.
- Gælder kun for det specifikke element.

**Embedded Styles**

- I head-sektionen.
- HTML `style`-elementet.
- Gælder for hele websidedokumentet.

**External Styles**

- Separat tekstfil med filendelsen `.css`.
- Associeres med et HTML `link`-element i head-sektionen af en webside.

**Imported Styles**

- Ligner External Styles.
- Vi koncentrerer os om de tre andre typer styles.

## 4. The "Cascade"

Den mere specifikke regel vinder. Hvis to eller flere regler har samme specificity, vinder den, der optræder sidst.

## 5. CSS Syntax

Style sheets består af **Rules**, som beskriver den styling, der skal anvendes. Hver rule indeholder en **Selector** og en **Declaration**.

### Kodeeksempel

Konfigurér en webside til at vise blå tekst og gul baggrund.

```css
body { color:    blue;
        background-color:     yellow; }
```

Dette kunne også skrives med hexadecimale farveværdier som vist nedenfor.

```css
body { color:    #0000FF;
        background-color:     #FFFF00; }
```

## 6. Inline CSS

Eksempel: konfigurér den røde tekst i overskriften og en grå baggrund i overskriften.

### Kodeeksempel

```html
<h1 style="color:#FF0000;background-color:#cccccc">This
is displayed as a red heading with gray background</h1>
```

Advarsel: Generel brug af inline styles er ineffektivt og upraktisk at vedligeholde. Men der er nogle situationer, hvor de er praktiske.

## 7. Embedded CSS

- Konfigureres i header-sektionen af en webside.
- Bruger HTML-elementet `<style>`.
- Gælder for hele websidedokumentet.
- Style-deklarationerne står mellem det åbnende og lukkende `<style>`-tag.

### Kodeeksempel

Konfigurér en webside med hvid tekst på sort baggrund.

```html
<head>
     <title>CSS demo</title>
     <style>
          body { background-color: #000000;
                   color: #FFFFFF;
                }
     </style>
</head>
```

## 8. External Style Sheets

- CSS style rules ligger i en tekstfil adskilt fra HTML-dokumenterne.
- Den eksterne style sheet-tekstfil har endelsen `.css` og indeholder kun style rules.
- Et HTML `link`-element bruges til at associere den eksterne style sheet-fil med websiden.
- Flere websider kan associeres med den samme eksterne style sheet-fil.

### Kodeeksempel

```html
<head>
    <title>CSS demo</title>
    <link rel="stylesheet" href="site.css">
</head>
<body>
    <p>Dette er en demo af eksternt stylesheet</p>
    <h2>Dette er en heading 2</h2>
</body>
```

`site.css`:

```css
body {background-color:#E6E6FA;
      color:#f93c53;
      font-family:Arial, sans-serif;
      font-size:120%; }
h2 { color: #003366; }
```

## 9. Common Formatting Properties

- `background-color`
- `color`
- `font-family`
- `font-size`
- `font-style`
- `font-weight`
- `line-height`
- `margin`
- `text-align`
- `text-decoration`
- `width`

## 10. span Element Example

### Kodeeksempel

CSS:

```html
<style>
.companyname { font-weight: bold;
               font-family: Georgia, "Times New Roman",
                            serif;
               font-size: 1.25em;
             }
 </style>
```

HTML:

```html
<p>Your needs are important to us at <span
class="companyname">Acme Web Design</span>.
We will work with you to build your Web site.</p>
```

## 11. Centering Page Content med CSS og div

### Kodeeksempel

```css
#wrapper { margin-left: auto;
           margin-right: auto;
           width:80%;
           color: blue;
           background-color: #F0F0F0;}
```

```html
<body>
    <div id="wrapper">
        <h1>Heading</h1>
        <p>Some fancy text</p>
    </div>
</body>
```

## 12. Using Color on Web Pages

Computerskærme viser farve som intensiteter af rødt, grønt og blåt lys. Værdierne for rød, grøn og blå varierer fra 0 til 255, men udtrykkes ofte i hexadecimale tal, hvor `#` bruges til at angive en hexadecimal værdi.

Der er flere alternativer til at angive en farve:

- RGB i hex eller decimal.
- Color name — http://www.w3schools.com/colors/colors_names.asp
- HSL (kun i decimal).

## 13. HSL og HSLA

HSL står for hue, saturation og lightness.

- **Hue** er en grad på farvehjulet fra 0 til 360. 0 er rød, 120 er grøn, 240 er blå.
- **Saturation** er en procentværdi. 0 % betyder en gråtone, 100 % er den fulde farve.
- **Lightness** er en procentværdi. 0 % er sort, 100 % er hvid.

## 14. Different Options for Color

### Kodeeksempel

```css
.color1 {
    background-color: maroon;
    color: white;
}
.color2 {
    background-color: #800;
    color:#FFF;
}
.color3 {
    background-color: #800000;
    color: #FFFFFF;
}
.color4 {
    background-color: rgb(128,0,0);
    color: rgb(255,255,255);
}
.color5 {
    background-color: rgba(128,0,0,1.0);
    color: rgba(255,255,255,1.0);
}
.color6 {
    background-color: hsl(0,100%,13%);
    color:hsl(0,100%,100%);
}
.color7 {
    background-color: hsla(0,100%,13%,1.0);
    color: hsla(0,100%,100%,1.0);
}
```

1. Color name
2. Shorthand hexadecimal
3. Hexadecimal color value
4. RGB Decimal color value
5. RGB Decimal color value with transparency
6. HSL Decimal color value
7. HSL Decimal color value with transparency

## 15. Making Color Choices

Hvordan vælger man et farveskema?

- **Monochromatic** — http://meyerweb.com/eric/tools/color-blend
- **Vælg ud fra et fotografi eller andet billede** — http://www.colr.org
- **Start med en yndlingsfarve** og brug et af følgende sites til at vælge de øvrige farver:
  - http://colorsontheweb.com/colorwizard.asp
  - http://kuler.Adobe.com
  - http://colorschemedesigner.com/
- **Web Color Palette** — http://webdevfoundations.net/color

## 16. Configuring Text with CSS

CSS-properties til at konfigurere tekst:

- `font-weight` — konfigurerer fedheden af tekst.
- `font-style` — konfigurerer tekst til italic style.
- `font-size` — konfigurerer størrelsen af teksten.
- `font-family` — konfigurerer skrifttypen (typeface) for teksten.

## 17. The font-size Property

| Absolute-size keywords | em (eller ex) | rem | Percentage | px |
| --- | --- | --- | --- | --- |
| xx-small | .5em | .5rem | 50% | 8px |
| x-small | .6em | .6rem | 60% | 10px |
| small | .75em | .75rem | 75% | 12px |
| medium | 1em | 1rem | 100% | 16px |
| large | 1.15em | 1.15rem | 115% | 18px |
| x-large | 1.5em | 1.5rem | 150% | 24px |
| xx-large | 2em | 2rem | 200% | 32px |
| xxx-large | | | | |

- **em/ex/percentage:** Font size er relativ til parentens font size.
- **rem:** Font size er relativ til størrelsen af den font, der bruges af `<html>` (root)-elementet.

Accessibility Recommendation: Brug `rem` (eller `em` / percentage) font sizes — de kan nemt forstørres i alle browsere af brugerne.

Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/font-size

## 18. The font-family Property

Ikke alle har de samme fonte installeret på deres computer. Konfigurér en liste af fonte og inkludér et generisk family name.

### Kodeeksempel

```css
p {font-family: Arial, Verdana, sans-serif;}
```

| Font Family Category | Beskrivelse | Almindelige typeface-navne |
| --- | --- | --- |
| `serif` | Har små udsmykninger (serifs) i enden af bogstavstregerne | Times New Roman, Georgia, Palatino |
| `sans-serif` | Har ikke serifs | Arial, Tahoma, Helvetica, Verdana |
| `monospace` | Fixed-width font | Courier New, Lucida Console |
| `cursive` | Håndskrevet stil | Lucida Handwriting, Brush Script, Comic Sans MS |
| `fantasy` | Overdreven stil | Jokerman, Impact, Papyrus |

## 19. Hvilke enheder skal man bruge hvornår

- **px** — brug til: hairline borders, værdier til CSS shadow displacement og når man laver fixed-width designs. Brug ikke til: typografi.
- **rem og em** — brug til: typografi og elementer relateret til typografi (for eksempel margins). Foretræk `rem`.
- **%** — brug til: at lave responsive billeder og containere.
- **vh, vw** — brug til: relative length units baseret på viewport.
- **cqh, cqw** — brug til: relative length units baseret på container.
- **pt** — brug til: print stylesheets. Brug ikke til: noget andet.
- **cm og in** — brug til: print stylesheets, især page margins. Brug ikke til: noget andet.

CSS har absurd mange forskellige length units. Se dem alle — med forklaring — her: https://developer.mozilla.org/en-US/docs/Web/CSS/length

## 20. CSS Selectors — overblik

CSS style rules kan konfigureres for:

- element selector
- class selector
- id selector
- Contextual Selector

## 21. Element Selector

Element-selectoren vælger alle elementer med det angivne elementnavn.

### Kodeeksempel

```html
<style>
body { color: blue;
       background-color:   yellow;
}
</style>
```

## 22. class Selector

- Anvender en CSS-regel på en bestemt "class" af elementer på en webside.
- Associerer ikke styles til et specifikt HTML-element.
- Konfigureres med `.classname`.

### Kodeeksempel

Koden opretter en class kaldet "new" med rød, kursiv tekst.

```html
<style>
.new { color: #FF0000;
       font-style: italic;
     }
</style>
```

Anvend class'en:

```html
<p class="new">This text is red and in italics</p>
```

## 23. id Selector

- Anvender en CSS-regel på ÉT element på en webside.
- Konfigureres med `#idname`.

### Kodeeksempel

Koden opretter et id kaldet "new" med rød, stor, kursiv tekst.

```html
<style>
#new { color: #FF0000;
       font-size:2em;
       font-style: italic;
    }
</style>
```

Anvend id'et:

```html
<p id="new">This text is red, large, and in italics</p>
```

## 24. CSS Contextual Selector

Angiver et element i konteksten af dets container (parent) element. Kaldes også descendant selector eller relational selector.

Eksemplet konfigurerer en grøn tekstfarve kun for anchor tags placeret inden i `footer`-id'et.

### Kodeeksempel

```html
<style>
  #footer a { color: #00ff00; }
</style>
```

Fordelen ved contextual selectors er, at de reducerer antallet af classes og id'er, man skal anvende i HTML'en.

## 25. Contextual Selector — oversigt

- **Descendant combinator (E F)** — rammer ethvert element F, som er en descendant (child, grandchild, great-grandchild osv.) af et element E.
- **Child combinator (E > F)** — matcher ethvert element F, som er et direkte child af elementet E. Yderligere indlejrede elementer ignoreres.
- **Next sibling selector (E + F)** — matcher ethvert element F, som deler samme parent som E og kommer direkte efter E i markuppen.
- **Following sibling selector (E ~ F)** — matcher ethvert element F, som deler samme parent som et E og kommer efter det i markuppen.

### Kodeeksempel

```css
main > div {
  float: left;
  overflow: hidden;
}
```

## 26. Attribute Selectors

Tillader at matche elementer baseret på deres attributter.

- `E[attr]` — matcher ethvert element E, der har attributten `attr`, uanset attributtens værdi.
- `E[attr=val]` — matcher ethvert element E, der har attributten `attr` med den præcise værdi `val`.
- `E[attr|=val]` — matcher ethvert element E, hvis attribut `attr` enten har værdien `val` eller begynder med `val-`. Eksempel: `p[lang|="en"]`
- `E[attr~=val]` — matcher ethvert element E, hvis attribut `attr` inden for sin værdi har det fulde ord `val`, omgivet af whitespace. Eksempel: `.info[title~=more]`
- `E[attr^=val]` — matcher ethvert element E, hvis attribut `attr` starter med værdien `val`.

Obs: Der er flere attribute selectors end vist her.

## 27. Pseudo-classes

En pseudo-class bruges til at definere en særlig state for et element.

### Kodeeksempel

```css
selector:pseudo-class {
    property:value;
}
```

## 28. Anchor Pseudo-classes

### Kodeeksempel

```css
a:link {
    color: #FF0000;
}

a:visited {
    color: #00FF00;
}

a:hover {
    color: #FF00FF;
}

a:active {
    color: #0000FF;
}
```

Rækkefølgen er: unvisited link (`:link`), visited link (`:visited`), mouse over link (`:hover`), selected link (`:active`).

Obs: Rækkefølgen betyder noget!

## 29. Pseudo-classes for attributter, brugerinteraktion og form control state

- `:enabled`
- `:disabled`
- `:checked`
- `:indeterminate`
- `:target`
- `:default`
- `:valid`
- `:invalid`
- `:in-range`
- `:out-of-range`
- `:required`
- `:optional`
- `:read-only`
- `:read-write`

## 30. Structural Pseudo-classes

Gør det muligt at ramme elementer baseret på deres placering i markuppen.

### Kodeeksempel

Match ethvert `<p>`-element, som er første child af et vilkårligt element.

```css
p:first-child {
    color: blue;
}
```

Match det første `<li>`-element i alle `<ul>`-elementer.

```css
ul li:first-child {
    color: blue;
}
```

## 31. Flere structural pseudo-classes

- `:root`
- `E:nth-child(n)`
- `E:nth-last-child(n)`
- `E:nth-of-type(n)`
- `E:nth-last-of-type(n)`
- `E:first-child`
- `E:last-child`
- `E:only-child`
- `E:only-of-type`
- `E:empty`
- `E:not(exception)`

`nth-child` rammer det n'te child uanset type. Angiv en background color for hvert `<p>`-element, som er det andet p-element hos sin parent:

```css
p:nth-of-type(2) {
      background: #ff0000;
}
```

`odd` og `even` er keywords, der kan bruges til at matche child-elementer:

```css
p:nth-of-type(odd) {
     background: #ff0000;
}
```

Med en formel `(an + b)`, hvor `a` repræsenterer en cycle size, `n` er en tæller (starter ved 0), og `b` er en offset-værdi:

```css
p:nth-of-type(3n+1) {
      background: #ff0000;
}
```

Obs: Der er flere structural pseudo-classes end vist her.

## 32. Pseudo-elements

Tillader at ramme tekst, som er en del af dokumentet, men som ellers ikke kan rammes i document tree.

- `::first-letter` — matcher det første bogstav i en text node.
- `::first-line` — matcher den første linje i en text node.
- `::before` — indsætter noget før indholdet af hvert valgt element.
- `::after` — indsætter noget efter indholdet af hvert valgt element.
- `::selection` — matcher brugervalgt eller fremhævet tekst.

### Kodeeksempel

```css
p::first-letter {
       font-size: 150%;
   }
```

```css
p::after {
    content: " - Remember this";
}
```

## 33. W3C CSS Validation

- http://jigsaw.w3.org/css-validator/

## 34. References & Links

- Web Development and Design Foundations with HTML5
- A Guide To CSS Debugging — https://www.smashingmagazine.com/2021/10/guide-debugging-css/
- Specificity Calculator — https://specificity.keegan.st/
- CSS3 Click Chart — http://css3clickchart.com
- A visual CSS editor — http://enjoycss.com/
