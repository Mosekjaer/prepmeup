# L13 – Flexbox

## Metadata

- **Lektion:** L13 – Flexbox
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED Flexbox.pdf (16 slides)
- **Emner dækket:**
  - Hvad Flexbox er, og hvorfor det bruges (én-dimensionelt layout)
  - Flex container og flex items, main axis / cross axis
  - `flex-direction` og `flex-wrap`
  - `justify-content` (main axis) og `align-items` (cross axis)
  - `align-self` som override pr. item
  - `order`, `flex-grow`, `flex-basis`, `flex-shrink`
  - `flex`-shorthand og `flex-flow`-shorthand
  - Responsivt navigations-eksempel med media queries

---

## 1. What and Why

Flexbox er en CSS3-layoutmetode designet til **én-dimensionelt layout** (enten en række eller en kolonne, ikke begge på én gang). Den er fleksibel og float-fri og kan tilpasse sig forskellige skærmstørrelser og display devices.

Browseren får lov at ændre bredde eller højde på elementer, så de bedst udfylder den plads der er til rådighed:

- Elementer kan udvides (expand) til at fylde al ledig plads.
- Elementer kan skrumpe (shrink) for at undgå overflow.

Med Flexbox kan designeren lægge elementer ud lodret eller vandret, så de fylder al den plads der stilles til rådighed, eller mindst mulig plads. Man kan også lave elementer med ens højde eller bredde med ganske få linjer CSS.

## 2. Flex Layout Terminology

En **flex container** er den boks der genereres af et element med `display: flex` eller `display: inline-flex`. Børnene af en flex container kaldes **flex items** og lægges ud efter flex-layoutmodellen.

Slidet viser et akse-diagram: en grå container med to items (1 og 2) placeret side om side. Igennem containeren løber en vandret rød pil, **main axis**, fra **main start** i venstre side til **main end** i højre side. Vinkelret på den løber en lodret rød pil, **cross axis**, fra **cross start** i toppen til **cross end** i bunden. Længden af containeren langs main axis kaldes **main size** (grøn måltapper over boksen), og udstrækningen langs cross axis kaldes **cross size** (grøn måltapper i venstre side). Pointen er, at Flexbox' properties altid refererer til akserne — ikke til "venstre/højre" — og at akserne skifter retning når `flex-direction` ændres.

```css
.container {
  flex-direction: row | row-reverse | column | column-reverse;
}
```

## 3. Flex Container

Man definerer en **containing block** som container for de fleksible items — enten en inline eller en block-level flex container.

```html
<div class="container">
    <p>One</p>
    <p>Two</p>
    <p>Three</p>
    <p>Four</p>
    <p>Five</p>
</div>
```

```css
.container {
    display: inline-flex;
}
```

Eller

```css
.container {
    display: flex;
}
```

```css
.container > p {
    background-color: coral;
    margin: 0.2em;
}
```

Browser-screenshottet på slidet viser resultatet: de fem afsnit "One Two Three Four Five" står nu vandret side om side i koralfarvede kasser med lidt luft imellem — i stedet for under hinanden, som `<p>`-elementer normalt ville stå i normal flow.

## 4. flex-direction

`flex-direction` definerer den akse, som flex items følger efter hinanden langs. Slidet viser fire kasser med de samme fem items ("One" … "Five") under hver sin værdi:

| Værdi | Visuel effekt på illustrationen |
|---|---|
| `row` (default) | Items vandret fra venstre mod højre: One, Two, Three, Four, Five |
| `row-reverse` | Items vandret, men omvendt rækkefølge: Five, Four, Three, Two, One |
| `column` | Items lodret under hinanden ovenfra og ned: One, Two, Three, Four, Five |
| `column-reverse` | Items lodret, men nedefra og op i kilderækkefølgen — altså vist Five, Four, Three, Two, One fra toppen |

## 5. flex-wrap

`flex-wrap` styrer om flex containeren er single-line, multiline, eller multi-lined hvor hver ny linje kommer visuelt *før* den forrige. Mulige værdier: `nowrap`, `wrap` og `wrap-reverse`.

Illustrationen viser tre containere med samme fem items i en container der er for smal til dem alle:

| Værdi | Visuel effekt |
|---|---|
| `nowrap` | Alle fem items presses sammen på én linje; teksten bliver klemt/afskåret ("One Two Thre Four Five" overlapper hinanden) |
| `wrap` | Fire items på første linje (One, Two, Three, Four), og "Five" wrapper ned på en ny linje under dem |
| `wrap-reverse` | Samme opdeling, men den nye linje lægges *over* den første: "Five" står øverst, og One, Two, Three, Four står nederst |

## 6. justify-content

`justify-content` definerer justeringen langs **main axis**. Den hjælper med at fordele den ekstra ledige plads der er tilovers, når alle flex items på en linje enten er ufleksible eller er fleksible men har nået deres maksimale størrelse.

```css
.container {
  display: flex;
  border: 1px solid red;
  background-color: aquamarine;
  justify-content: space-evenly;
}
```

Illustrationen viser fire aquamarine-farvede containere med fem koralfarvede items:

| Værdi | Visuel effekt |
|---|---|
| `space-evenly` | Lige store mellemrum overalt — også mellem kanten og første/sidste item |
| `space-around` | Hvert item har lige meget plads omkring sig, så mellemrummet ved kanterne er halvt så stort som mellemrummene mellem items |
| `flex-end` | Alle fem items samlet klumpet i højre side, al tom plads til venstre |
| `center` | Alle fem items samlet i midten, tom plads fordelt lige i begge sider |

## 7. align-items

`align-items` definerer default-opførslen for hvordan flex items lægges ud langs **cross axis** på den aktuelle linje.

```css
.container {
  height: 25vh;
  display: flex;
  border: 1px solid red;
  background-color: aquamarine;
  justify-content: center;
  align-items: stretch;
}
```

Illustrationen sammenligner to værdier i en container med `height: 25vh`:

| Værdi | Visuel effekt |
|---|---|
| `stretch` (default) | De fem koral-items strækkes ud i fuld højde af containeren — høje, smalle søjler fra top til bund |
| `center` | Items beholder deres egen (lille) højde og placeres lodret centreret midt i den høje container |

## 8. align-self

Man kan overskrive `align-items`-værdien for enkelte flex items ved at sætte `align-self` på et enkelt flex item.

```css
.container2 {
    display: flex;
    -ms-flex-flow: row nowrap;
    -webkit-flex-flow: row nowrap;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-start;
}

.flexItem {
    background-color: coral;
}

.flexItem:first-of-type {
    align-self: stretch;
}
```

Browser-screenshottet nederst viser effekten: containeren har `align-items: flex-start`, så alle items ligger op mod toppen med kun deres egen tekstshøjde. Men det første item ("One") har `align-self: stretch` og er derfor det eneste der strækkes hele vejen ned gennem containerens højde som en høj koral-søjle. Items med mere indhold ("Three" gentaget 3 gange, "Five" gentaget 4 gange) er højere end de øvrige, men stadig top-justerede.

## 9. order

Man kan give hvert (eller nogle) flex item en `order`-property med en heltalsværdi.

- Browseren viser items i **stigende** rækkefølge efter `order`-værdien i stedet for HTML-kilderækkefølgen.
- Default-værdien af `order` er `0`.
- Hvis to eller flere items har samme `order`-værdi, bruger browseren HTML-kilderækkefølgen mellem dem.

```css
.flexItem:first-of-type {
    order: 1;
}

.flexItem:last-of-type {
    order: -1;
}
```

Screenshottet viser resultatet: HTML-kilderækkefølgen er One, Two, Three, Four, Five, men da "Five" har `order: -1` (lavest) står den nu længst til venstre, og da "One" har `order: 1` (højest) står den længst til højre. De tre midterste beholder deres indbyrdes rækkefølge (Two, Three, Four), fordi de alle har default `order: 0`. Visuel rækkefølge på skærmen: Five, Two, Three, Four, One.

## 10. flex-grow

`flex-grow` definerer hvor meget et flex item skal vokse, hvis der er plads. Det er en **enhedsløs** værdi pr. flex item, der angiver den proportion det pågældende item skal optage af den ledige plads.

```css
.flexItem:first-of-type {
    flex-grow: 2;
}

.flexItem:last-of-type {
    flex-grow: 4;
}
```

Screenshottet viser en række med fem items. "One" (grow 2) er tydeligt bredere end de tre midterste items (default `flex-grow: 0`, som beholder deres indholdsbredde), og "Five" (grow 4) er den klart bredeste — cirka dobbelt så bred som "One". Den ledige plads fordeles altså i forholdet 2:0:0:0:4.

## 11. flex-basis

`flex-basis` definerer default-bredden af hvert flex item. Mulige værdier:

- `auto` (default)
- En længdeværdi som `200px`, `10em` eller `30%`

## 12. flex-shrink

`flex-shrink` definerer et flex items evne til at skrumpe, hvis det er nødvendigt.

- Mulige værdier: et heltal uden enheder.
- Hvis der ikke er plads nok til alle flex items, vil de items med den **største** `flex-shrink`-værdi skrumpe først.

## 13. flex – shorthand på items

`flex` er shorthand for `flex-grow`, `flex-shrink` og `flex-basis` kombineret.

- Andet og tredje parameter (`flex-shrink` og `flex-basis`) er valgfrie.
- Shorthand-notationen sætter de øvrige værdier intelligent.

```css
.container > p {
    background-color: coral;
    margin: 0.2em;
    padding: 0.2em;
    flex: 1;
}
```

```html
<p>One</p>
<p>Two</p>
<p style="flex: 2">Three</p>
<p>Four</p>
<p>Five</p>
```

Illustrationen viser resultatet: alle fem koral-kasser fylder én linje i den aquamarine container. Fire af dem er lige brede (`flex: 1`), mens "Three" er cirka dobbelt så bred, fordi den har `flex: 2` sat inline.

## 14. Eksempel: responsiv navigation

Forestil dig et højrestillet navigationselement øverst på websitet, som skal være centreret på mellemstore skærme og enkelt-kolonne på små enheder.

```css
.navigation {
  display: flex;
  flex-flow: row wrap;
  justify-content: flex-end;
}

@media all and (max-width: 800px) {
  .navigation {
    justify-content: space-around;
  }
}

@media all and (max-width: 600px) {
  .navigation {
    flex-flow: column wrap;
    padding: 0;
  }
}
```

Slidet viser tre skærmbilleder af den samme blå navigationsbar:

1. **Bred skærm:** Home, About, Products, Contact klumpet sammen i højre side (`justify-content: flex-end`).
2. **Under 800px:** de fire links fordelt jævnt ud over hele bredden med luft omkring hvert (`space-around`).
3. **Under 600px:** de fire links stablet lodret under hinanden i hver sin fuldbredde-blok (`flex-flow: column wrap`).

Bemærk at `flex-flow` er shorthand for `flex-direction` og `flex-wrap`.

## 15. References & Links

- A Complete Guide to Flexbox — https://css-tricks.com/snippets/css/a-guide-to-flexbox/
- Flexbox Patterns — http://www.flexboxpatterns.com/home
- Flexbox Zombies — https://mastery.games/post/flexboxzombies2/
