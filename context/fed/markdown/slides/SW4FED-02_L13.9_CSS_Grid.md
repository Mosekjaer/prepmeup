# L13 – CSS Grid

## Metadata

- **Lektion:** L13 – CSS Grid
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED CSS Grid.pdf (11 slides)
- **Emner dækket:**
  - Normal flow, og hvordan man bryder ud af den (floats og positioning)
  - `display: grid` og forskellen på 1D (flex) og 2D (grid) layout
  - Grid-terminologi: grid container, tracks, lines, cells, areas, inline/block axis
  - `grid-template-columns`, `fr`-enheden og `grid-gap`
  - Line-based positioning med `grid-column` / `grid-row`
  - Named areas med `grid-template-areas` og `grid-area`
  - Responsivt layout ved at omdefinere grid areas i media queries

---

## 1. Normal Flow

På en HTML-webside uden CSS til at ændre layoutet vises elementerne i **normal flow**.

Illustrationen viser en kasse med grå tekstlinjer i tre afsnit. En vandret dobbeltpil over kassen er markeret **Inline Direction** — den retning ord lægges ud i (venstre mod højre på dansk/engelsk). En lodret dobbeltpil i venstre side er markeret **Block Direction** — den retning blokke (afsnit) stables i, altså oppefra og ned. Normal flow er altså: inline-indhold flyder langs inline-aksen, og blokke stables langs block-aksen.

## 2. Moving away from normal flow

**Floats** bruges til at skubbe en boks til venstre eller højre, så indhold kan flyde omkring den.

```css
.item { float: left; }
```

Illustrationen viser en tekstblok hvor en grå firkantet boks er placeret i øverste venstre hjørne, og teksten flyder rundt om den — først til højre for boksen, og derefter i fuld bredde under boksen.

**Positioning** bruges til at flytte et element væk fra dets plads i normal flow.

```css
.item {
  position: relative;
  bottom: 50px;
}
```

Illustrationen viser samme tekstblok, men her er den grå boks flyttet 50px opad i forhold til hvor den ville stå. Teksten flyder *ikke* omkring den — den plads boksen oprindeligt optog, står nu tom, og boksen overlapper teksten ovenover. Det er den vigtige forskel: `position: relative` flytter kun den visuelle repræsentation, pladsen i flowet reserveres stadig.

## 3. CSS grid

```css
display: grid;
```

- Definerer et **to-dimensionelt** grid-baseret layoutsystem, optimeret til user interface design.
- Børnene af en grid container kan placeres i vilkårlige slots i et fordefineret fleksibelt eller fixed-size layout-grid (rækker og kolonner).
- Grid Layout er optimeret til 2-dimensionelle layouts: dem hvor man ønsker alignment af indhold i **begge** dimensioner.

Slidet sammenligner to layouts nederst:

- **Flex layout example:** to rækker af kasser. Øverste række har tre kasser, nederste har fem — kasserne i de to rækker har ingen fælles lodrette linjer, fordi hver flex-linje fordeler pladsen for sig selv.
- **Grid layout example:** kasserne i de to rækker deler de samme kolonnegrænser, så der er alignment både vandret og lodret. Det er den forskel 2D-layout giver.

## 4. Eksempel: simpelt grid

```html
<body>
  <div class="container">
    <div>1</div>
    <div>2</div>
    <div>3</div>
    <div>4</div>
    <div>5<br>has more content.</div>
  </div>
</body>
```

```css
.container {
  width: 100%;
  border: 5px solid rgb(111, 41, 97);
  border-radius: .5em;
  padding: 10px;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  grid-gap: 20px;
}

.container>div {
  padding: 10px;
  background-color: rgba(111, 41, 97, .3);
  border: 2px solid rgba(111, 41, 97, .5);
}
```

Browser-screenshottet viser resultatet: en lilla-omkranset container hvor de fem divs fordeles i tre lige brede kolonner (`1fr 1fr 1fr`). Første række indeholder 1, 2 og 3; anden række indeholder 4 og 5, mens tredje celle i anden række er tom. Div nr. 5 har mere indhold og er derfor højere — og fordi de er i samme række, **strækkes div 4 til samme højde** automatisk. Der er 20px luft mellem alle celler fra `grid-gap`.

## 5. Grid terminology

- **Grid Container** er det element du har sat `display: grid` på.
- Et grid har altid to akser:
  - **Inline Axis** løber i den retning ord lægges ud på siden.
  - **Block Axis** løber i den retning blokke lægges ud.

Diagrammet viser et 3×3-grid med annoterede begreber:

| Begreb | Hvad pilen på illustrationen peger på |
|---|---|
| **Column track** | Lodret pil ned gennem en hel kolonne — pladsen mellem to tilstødende lodrette grid lines |
| **Row track** | Vandret pil hen gennem en hel række — pladsen mellem to tilstødende vandrette grid lines |
| **Grid Line** | De stiplede skillelinjer mellem tracks (pilen peger på en vandret linje). Linjerne er nummererede og er det man positionerer efter |
| **Grid Cell** | Én enkelt kasse i grid'et — skæringen mellem én række og én kolonne |
| **Grid Area** | Celler der tilsammen udgør et komplet rektangel (kan spænde over flere rækker og kolonner) |

## 6. Line-based positioning

Man placerer items ved at angive hvilken **grid line** de starter og slutter på: `grid-column: <start> / <end>`. Slidet markerer eksplicit med pile at det første tal er **start** og det andet er **end**.

```html
<div class="container">
  <div class="one">1</div>
  <div class="two">2</div>
  <div class="three">3</div>
  <div class="four">4</div>
  <div class="five">5</div>
</div>
```

```css
.one {
  grid-column: 1 / 4;
  grid-row: 1;
}
.two {
  grid-column: 1 / 3;
  grid-row: 2;
}
.three {
  grid-column: 2 / 4;
  grid-row: 2 / 5;
}
.four {
  grid-column: 1;
  grid-row: 4;
}
.five {
  grid-column: 3;
  grid-row: 4 / 5;
}
```

Browser-screenshottet viser resultatet: item 1 spænder over alle tre kolonner i øverste række (linje 1 til 4). Item 2 fylder de to første kolonner i række 2, og item 3 starter i kolonne 2 og strækker sig ned gennem række 2–4 — den overlapper altså item 2's celle i kolonne 2, og den vindes af det senere item i kilden. Item 4 sidder alene i kolonne 1, række 4, og item 5 i kolonne 3, række 4. Resultatet er et layout med tomme huller — celler ingen har krævet, står blot tomme.

## 7. Positioning with named areas

Man giver hvert item et navn og beskriver derefter layoutet som værdien af `grid-template-areas`.

```html
<div class="container">
  <div class="one">1</div>
  <div class="two">2</div>
  <div class="three">3</div>
  <div class="four">4</div>
  <div class="five">5</div>
</div>
```

```css
.container {
  width: 100%;
  border: 5px solid rgb(111, 41, 97);
  border-radius: .5em;
  padding: 10px;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  grid-auto-rows: minmax(50px, auto);
  grid-gap: 20px;
  grid-template-areas:
    "a a a"
    "b c c"
    ". . d"
    "e e d";
}
.one {
  grid-area: a;
}
.two {
  grid-area: b;
}
```

(Slidet viser kun `.one` og `.two`; `.three`, `.four` og `.five` får tilsvarende `grid-area: c`, `d` og `e`.)

Bemærk to ting i syntaksen: hver streng i `grid-template-areas` er én række, og hvert navn i strengen er én kolonne. Et **punktum** (`.`) betyder en tom celle. `grid-auto-rows: minmax(50px, auto)` giver rækkerne mindst 50px højde, men lader dem vokse efter indholdet.

Browser-screenshottet viser layoutet: item 1 (`a`) i fuld bredde øverst; item 2 (`b`) i kolonne 1 og item 3 (`c`) spændende over kolonne 2–3 i anden række; item 4 (`d`) i kolonne 3, spændende over de to nederste rækker; item 5 (`e`) over kolonne 1–2 i nederste række. De to punktummer i tredje række giver et tydeligt tomt hul under item 2 og 3.

## 8. Redefining grid areas with media queries

Den samme HTML kan lægges helt anderledes ud på forskellige skærmstørrelser — man ændrer bare `grid-template-areas` (og kolonnerne) i en media query. HTML'en røres ikke.

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
```

```css
.container {
  display: grid;
  grid-gap: 1em;
  grid-template-areas:
    "header"
    "sidebar"
    "content"
    "sidebar2"
    "footer";
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
  .container {
    grid-gap: 20px;
    grid-template-columns: 120px auto 120px;
    grid-template-areas:
      "header header header"
      "sidebar content sidebar2"
      "footer footer footer";
  }
}
```

<!-- de sidste linjer af 800px-blokken er delvist skjult på slide 9; footer-rækken fremgår af skærmbillederne på slide 10 -->

Slide 10 viser tre browservinduer i forskellige bredder, som demonstrerer de tre trin:

1. **Smallest (under 500px, én kolonne):** alle fem blokke stablet lodret i rækkefølgen Header, Sidebar, Content, Sidebar 2, Footer — hver i fuld bredde.
2. **Medium (mindst 500px, to kolonner 20% / auto):** Header i fuld bredde øverst. Derefter Sidebar i den smalle venstre kolonne med Content ved siden af i den brede. Sidebar 2 spænder over begge kolonner, og Footer ligeså nederst.
3. **Bred (mindst 800px, tre kolonner 120px / auto / 120px):** Header i fuld bredde. Midterrækken har Sidebar til venstre, Content i midten (det brede felt) og Sidebar 2 til højre. Footer i fuld bredde nederst.

I alle tre skærmbilleder ligger Sidebar og Content som mørke bokse, mens Header, Sidebar 2 og Footer er lysegrå — det gør det let at se at det er de samme elementer der bare bliver ompositioneret.

## 9. References & Links

- Getting Started With CSS Layout — https://www.smashingmagazine.com/2018/05/guide-css-layout/
- Introducing the CSS Grid Layout — http://www.sitepoint.com/introducing-the-css-grid-layout/
- CSS Grid Layout (MDN) — https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Grid_Layout
- Grid by Example — https://gridbyexample.com/
- CSS Grid for UI Layouts — https://hacks.mozilla.org/2018/02/css-grid-for-ui-layouts/
- W3C CSS Grid Layout — https://www.w3.org/TR/css-grid-1/
- Grid Critters — https://gridcritters.com/
- Responsive grid magazine layout in just 20 lines of CSS — https://css-tricks.com/responsive-grid-magazine-layout-in-just-20-lines-of-css/
