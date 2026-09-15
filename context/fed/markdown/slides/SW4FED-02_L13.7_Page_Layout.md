# L13 – Page Layout med HTML5 & CSS3

## Metadata

- **Lektion:** L13 – Page Layout
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED Page Layout.pdf (23 slides)
- **Emner dækket:**
  - CSS Box Model: content, padding, border, margin
  - Margin- og padding-syntaks (1-4 værdier)
  - `box-sizing: content-box` vs. `border-box`
  - Normal flow
  - `position: relative` og `position: absolute`
  - `float`, `clear` og `overflow` til at afslutte floats
  - `display`-property (none, block, inline, flex, grid)
  - Navigationslister (vertikal og horisontal) med `<ul>`
  - Valg mellem class og id, navngivning
  - CSS-debugging og ARIA roles

---

## Agenda

- The CSS Box Model
- Positioning with CSS
- Navigation lists

## 1. The Box Model

Diagrammet på slidet viser fire koncentriske kasser: yderst **margin**, derindenfor **border**, derindenfor **padding**, og inderst den hvide **content**-boks. Kasserne er annoteret på alle fire sider med `top`, `right`, `bottom` og `left` — hver af de fire lag kan altså sættes uafhængigt på hver af de fire sider.

- **Content** — tekst og webside-elementer i containeren.
- **Padding** — området mellem content og border.
- **Border** — mellem padding og margin.
- **Margin** — bestemmer den tomme plads mellem elementet og tilstødende elementer.

### Box model in Action

Skærmbilledet (Firefox, "Examples of the Box Model") viser to elementer med callouts der peger på hver del af boksmodellen. Øverst et `<h1>` med lyseblå baggrund og tynd ramme; nedenunder et `<div>` med kraftig blå baggrund og en sort 5 pixel border. Callout-teksterne peger på margin (det tomme område udenfor rammen, hvor sidens baggrund skinner igennem), border (den sorte streg), padding (afstanden mellem rammen og teksten) og content (selve teksten). Teksten i div'en forklarer selv: elementet har lyseblå baggrund, browserens default padding (som er ingen padding), og en sort 5 pixel border — og det tomme område hvor sidens baggrund skinner igennem mellem dette element og elementet ovenover er et eksempel på margin.

## 2. Configure Margin with CSS

`margin`-propertyen konfigurerer tom plads mellem elementet og tilstødende elementer. Relaterede properties: `margin-top`, `margin-right`, `margin-bottom`, `margin-left`.

Syntaks-eksempler — bemærk hvordan antallet af værdier bestemmer betydningen:

```css
h1 { margin: 0; }                    /* alle fire sider */
h1 { margin: 20px 10px; }            /* først top og bottom, dernæst left og right */
h1 { margin: 10px 30px 20px; }       /* top, left og right, bottom */
h1 { margin: 20px 30px 0 30px; }     /* top, right, bottom, left */
```

Samme syntaks gælder for `padding`.

## 3. Overriding box-sizing

Default-værdien for `box-sizing` er **`content-box`**. Med default box-sizing bliver den faktisk renderede bredde bredere end den bredde du satte, så snart elementet får padding eller border:

```
Actual width = width + border-left + border-right + padding-left + padding-right
```

Slidet illustrerer problemet med en håndtegnet grøn kasse med tyk grøn ramme og teksten "Sometimes I have a `border: 6px solid green;`". Ved siden af står den grubende kommentar: "Now my width is… uhm, 25% + 12px I guess? I can tell you one thing, four of me won't fit on a row." Pointen er at et element med `width: 25%` plus border ikke længere passer fire gange på en række.

### Løsningen: border-box

Med `box-sizing: border-box` presser padding og border sig **ind i** boksen i stedet for at udvide den. Resultatet er en boks med præcis den bredde du satte, og som du kan regne med.

```css
*, *:before, *:after {
      box-sizing: border-box;
}
```

Normalize og Bootstrap gør dette for dig.

Illustrationen viser fire kasser (grøn, blå, lilla og gul) med vidt forskellig border- og padding-tykkelse — alligevel er alle fire nøjagtig lige brede og sidder side om side på én række. Undertekst: "Four of me can sit in a row no matter what border and padding we have. Life = easy street."

## 4. Positioning with CSS — Normal Flow

**Normal flow** er browserens visning af elementer i den rækkefølge de er kodet i webside-dokumentet.

Slidet viser tre illustrationer:

1. Et browservindue med to bokse — "This is the first box." (lyseblå med stiplet ramme) og under den "This is the second box." (hvid med tynd ramme). De stables lodret i kilderækkefølgen.
2. Et browservindue hvor den anden boks i stedet er **inde i** den første ("This is the outer box." med "This is the inner box." indeni) — nesting følger også normal flow.
3. Akse-diagrammet med **Inline Direction** (vandret dobbeltpil over kassen) og **Block Direction** (lodret dobbeltpil i venstre side): inline-indhold flyder vandret, blokke stables lodret.

## 5. Relative Positioning

Ændrer et elements placering **i forhold til** hvor det ellers ville optræde.

```css
h1 {
    background-color: #cccccc;
    padding: 5px;
    color: #000000;
}
#myContent {
    position: relative;
    left: 30px;
    font-family: Arial, sans-serif;
}
```

Skærmbilledet viser en side med overskriften "Relative Positioning" på grå baggrund, og under den et afsnit der er rykket 30 pixels ind fra venstre kant i forhold til sin normale plads. Teksten i afsnittet siger det selv: "This paragraph uses CSS relative positioning to be placed 30 pixels in from the left side."

## 6. Absolute Positioning

Angiver præcist et elements placering i browservinduet.

```css
h1 {
    background-color: #cccccc;
    padding: 5px;
    color: #000000;
}
#content {
    position: absolute;
    left: 200;
    top: 100;
    font-family: Arial, sans-serif;
    width: 300;
}
```

<!-- bemærk: værdierne left/top/width står uden enhed på slidet — det er teknisk set ugyldig CSS, men gengivet som i kilden -->

Skærmbilledet viser overskriften "Absolute Positioning" øverst, og et smalt tekstblok placeret 200 pixels inde fra venstre og 100 pixels ned fra toppen af browservinduet, med en bredde på 300 pixels. Teksten forklarer nøjagtigt det.

## 7. float Property

Elementer der ser ud til at "flyde" i højre eller venstre side af enten browservinduet eller et andet element konfigureres ofte med `float`-propertyen.

```css
h1 {
    background-color: #cccccc;
    padding: 5px;
    color: #000000;
}
p {
    font-family: Arial, sans-serif;
}
#yls {
    float: right;
    margin: 0 0 5px 5px;
    border: solid;
}
```

Skærmbilledet ("Wildflowers") viser en grøn overskrift, og under den et afsnit tekst hvor et billede af en gul Lady Slipper-orkidé ligger i højre side med en ramme omkring, og teksten flyder omkring den til venstre. Marginen `0 0 5px 5px` giver luft under og til venstre for billedet, så teksten ikke klistrer op ad det.

## 8. clear Property

`clear` er nyttig til at "clear" eller terminere en float. Værdier: `left`, `right` og `both`.

Slidet viser to skærmbilleder af samme side:

1. **Uden clear:** overskriften `<h2>` "Be Green When Enjoying Wildflowers" vises i normal flow — den lægger sig ved siden af det floatede billede, hvilket ser forkert ud (den brydes over to linjer ved siden af blomsterbilledet).
2. **Med `clear: left;` på h2:** nu vises h2-teksten **efter** det floatede billede, altså på en linje for sig selv under billedet, i fuld bredde.

## 9. overflow Property

`overflow` er tænkt til at konfigurere visningen af elementer på en webside. Men den er også nyttig til at "clear" eller terminere en float **før** slutningen af et container-element. Værdier: `auto`, `hidden` og `scroll`.

Slidet viser problemet og løsningen:

1. **Uden overflow:** container-div'ens gule baggrund strækker sig ikke så langt som man ville forvente — det floatede billede stikker ud under baggrunden, fordi et floatet element ikke bidrager til forældrens højde. Callout: "The background does not extend as far as you'd expect."
2. **Med `overflow: auto;` på div'en** der indeholder billedet og afsnittet: nu strækker baggrunden sig hele vejen ned omkring billedet, og h2-teksten vises efter det floatede billede.

## 10. Display Property

`display` konfigurerer **hvordan** og **om** et element vises.

| Værdi | Effekt |
|---|---|
| `display: none;` | Elementet vises ikke. |
| `display: block;` | Elementet renderes som et block element — også selvom det er et inline element, som fx et hyperlink. |
| `display: inline;` | Elementet renderes som et inline element — også selvom det er et block element, som fx et `<li>`. |
| `display: flex;` | Har sin egen præsentation (se Flexbox-slidesene). |
| `display: grid;` | Har sin egen præsentation (se CSS Grid-slidesene). |

## 11. Navigation lists — vertikal navigation

```html
<div id="leftcolumn">
   <ul>
      <li><a href="index.html">Home</a></li>
      <li><a href="menu.html">Menu</a></li>
      <li><a href="directions.html">Directions</a></li>
      <li><a href="contact.html">Contact</a></li>
   </ul>
</div>
```

CSS fjerner list marker og understregning:

```css
#leftcolumn ul { list-style-type: none; }
#leftcolumn a  { text-decoration: none; }
```

Slidet viser to små skærmbilleder ved siden af hinanden. **Før CSS:** de fire links står under hinanden med sorte punkttegn (bullets) foran og blå understreget tekst. **Efter CSS:** samme fire links står stadig lodret under hinanden, men uden bullets og uden understregning — de ligner nu et rent menu-panel.

## 12. Navigation lists — horisontal navigation

```html
<nav>
   <ul>
      <li><a href="index.html">Home</a></li>
      <li><a href="menu.html">Menu</a></li>
      <li><a href="directions.html">Directions</a></li>
      <li><a href="contact.html">Contact</a></li>
   </ul>
</nav>
```

CSS fjerner list marker, fjerner understregning, tilføjer padding og konfigurerer list items til inline display:

```css
nav ul { list-style-type: none; }
nav a  { text-decoration: none;
         padding-right: 10px; }
nav li { display: inline; }
```

Skærmbilledet viser resultatet: de fire links "Home Menu Directions Contact" står nu på **én vandret linje** i en blå bjælke, uden bullets og uden understregning, med 10px luft mellem hvert link fra `padding-right`. Nøglen er `display: inline` på `<li>`, som ellers er block-elementer.

## 13. Deciding to Configure a class or id

**Brug en class:**

- Hvis stilen kan gælde for mere end ét element på en side
- Brug `.`-notation (punktum) i stylesheetet
- Brug `class`-attributten i HTML'en

**Brug et id:**

- Hvis stilen kun er specifik for ét enkelt element på en side
- Brug `#`-notation i stylesheetet
- Brug `id`-attributten i HTML'en

### Navngivning af class eller id

Et class- eller id-navn bør være **beskrivende for formålet** — fx `nav`, `news`, `footer`.

Dårlige navnevalg: `redText`, `bolded`, `blueborder` — de beskriver udseendet, ikke formålet, og bliver misvisende så snart designet ændres.

## 14. CSS Debugging Tips

- Tjek manuelt for syntaksfejl.
- Brug W3C CSS Validator til at finde syntaksfejl: http://jigsaw.w3.org/css-validator/
- Konfigurér midlertidige baggrundsfarver.
- Konfigurér midlertidige borders.
- Brug CSS-kommentarer til at finde det uventede: `/* the browser ignores this code */`
- Forvent ikke at dine sider ser præcis ens ud i alle browsere — medmindre du bruger normalize.css.
- Vær tålmodig!

## 15. ARIA Roles

Som en del af Web Accessibility Initiative (WAI) definerer Accessible Rich Internet Applications Suite (ARIA) en måde at gøre webindhold og webapplikationer mere tilgængelige på.

Det bruges til at forbedre tilgængeligheden af dynamisk indhold og avancerede user interface controls udviklet med Ajax, HTML, JavaScript og relaterede teknologier.

ARIA roles virker nu i mange browsere og skærmlæsere. Når de ikke gør, er de harmløse.

```html
<header role="banner">
<nav role="navigation">
```

Mere info: http://www.webteacher.ws/2010/10/14/aria-roles-101/

## 16. References & Links

- normalize.css — http://necolas.github.io/normalize.css/
- Modern normalize — https://github.com/sindresorhus/modern-normalize
