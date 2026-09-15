# L14 – Tabeller i HTML

## Metadata

- **Lektion:** L14 – Tables in HTML
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L14/FED Tables in HTML.pdf (16 slides)
- **Emner dækket:**
  - `<table>`, `<tr>`, `<td>`, `<th>`, `<caption>`
  - `colspan` og `rowspan`
  - Accessibility: `<th id>` og `<td headers>`
  - Styling af tabeller med CSS i stedet for HTML-attributter
  - `border-collapse`, padding, `text-align`
  - CSS structural pseudo-classes og zebra-striping
  - Table row groups: `<thead>`, `<tbody>`, `<tfoot>`

---

## Agenda

- Tables in HTML
- Styling Tables with CSS

## 1. HTML Table

Tabeller bruges på websider til at organisere **tabulær information** — opbygget af rækker og kolonner, ligesom et regneark.

| Element | Formål |
|---|---|
| `<table>` | Indeholder tabellen |
| `<tr>` | Indeholder en tabelrække (table row) |
| `<td>` | Indeholder data for en tabelcelle. `<td>` er containere og kan indeholde alle slags HTML-elementer som tekst, billeder, lister osv. |
| `<caption>` | Konfigurerer en beskrivelse af tabellen |

## 2. HTML Table Example

```html
<table border="1">
   <caption>Students</caption>
   <tr>
        <td>Casper</td>
        <td>Andersen</td>
        <td>20103276</td>
    </tr>
    <tr>
        <td>Benjamin</td>
        <td>Blankholm</td>
        <td>201206087</td>
    </tr>
    <tr>
        <td>Jens</td>
        <td>Brendstrup</td>
        <td>201270480</td>
    </tr>
</table>
```

På slidet er `border="1"` markeret med en håndtegnet pil og teksten **"Use css!"** — HTML's `border`-attribut er forældet, man skal bruge CSS i stedet.

Skærmbilledet af den renderede tabel viser overskriften "Students" (fra `<caption>`) centreret over tabellen, og derunder et 3×3-gitter med tynde rammer om hver celle. Alle celler har samme typografi — der er ingen visuel forskel på fornavn, efternavn og id, fordi alle er `<td>`.

## 3. HTML Table – `<th>`

```html
<table border="1">
   <caption>Students</caption>
   <tr>
        <th>Name</th>
        <th>Family name</th>
        <th>Id</th>
    </tr>
    <tr>
        <td>Casper</td>
        <td>Andersen</td>
        <td>20103276</td>
    </tr>
    <tr>
        <td>Benjamin</td>
        <td>Blankholm</td>
        <td>201206087</td>
    </tr>
    . . .
</table>
```

Den renderede tabel viser nu en overskriftsrække med **Name**, **Family name** og **Id** i fed og **centreret** — det er browserens default-styling af `<th>`. Datarækkerne under er stadig venstrestillet normal tekst.

## 4. HTML Common Table Cell Attributes

- `colspan`
- `rowspan`

Brug CSS til at konfigurere de fleste tabelcelle-egenskaber i stedet for HTML-attributter. `colspan` og `rowspan` er strukturelle og hører derfor stadig hjemme i HTML'en.

## 5. HTML colspan Attribute

```html
<table border="1">
   <caption>Students</caption>
   <tr>
        <th colspan="2">Name</th>
        <th>Id</th>
    </tr>
    <tr>
        <td>Casper</td>
        <td>Andersen</td>
        <td>20103276</td>
    </tr>
    <tr>
        <td>Benjamin</td>
        <td>Blankholm</td>
        <td>201206087</td>
    </tr>
    . . .
</table>
```

Den renderede tabel viser effekten tydeligt: overskriftsrækken har nu kun **to** celler, hvor "Name" strækker sig hen over de to første kolonner (fornavn og efternavn) som én bred celle, mens "Id" står over den tredje kolonne. Datarækkerne har stadig tre celler hver.

## 6. Accessibility and Tables

- Brug table header elements (`<th>`-tags) til at angive kolonne- eller rækkeoverskrifter.
- Brug `caption`-elementet til at give tabellen en tekstlig titel eller beskrivelse.

**Komplekse tabeller:** associér tabelcelle-værdier med deres tilsvarende headers:

- `id`-attributten på `<th>`-tagget
- `headers`-attributten på `<td>`-tagget

Det gør at en skærmlæser kan læse op hvilken kolonne en given værdi hører til, også når tabellen er for kompleks til at udlede det af positionen alene.

### Table Accessibility Example

```html
<table border="1">
    <caption>Students</caption>
    <tr>
        <th id="name">Name</th>
        <th id="famName">Family name</th>
        <th id="id">Id</th>
    </tr>
    <tr>
        <td headers="name">Casper</td>
        <td headers="famName">Andersen</td>
        <td headers="id">20103276</td>
    </tr>
    <tr>
        <td headers="name">Benjamin</td>
        <td headers="famName">Blankholm</td>
        <td headers="id">201206087</td>
    </tr>
...
```

## 7. Using CSS to Style a Table

Oversigt over hvilken CSS-property der erstatter hvilken gammel HTML-attribut:

| HTML-attribut | CSS-property |
|---|---|
| `align` | Justér en tabel: `table { width: 75%; margin: auto; }`. Justér inde i en tabelcelle: `text-align` |
| `bgcolor` | `background-color` |
| `cellpadding` | `padding` |
| `cellspacing` | `border-spacing` eller `border-collapse` |
| `height` | `height` |
| `valign` | `vertical-align` |
| `width` | `width` |
| `border` | `border`, `border-style`, `border-spacing` og `border-collapse` |
| (findes ikke i HTML) | `background-image` |

## 8. Simple Style Example

Slidet viser to trin, hver med kode til venstre og den renderede tabel til højre.

**Trin 1:**

```css
table, th, td {
    border: 1px solid black;
    border-collapse: collapse;
}
```

Renderet: tabellen har nu **enkelte** streger mellem cellerne i stedet for den dobbelte "3D"-ramme man ellers får — det er `border-collapse: collapse` der lægger tilstødende cellers borders oveni hinanden. Cellerne sidder tæt sammen uden luft omkring teksten, og `<th>`-teksten er stadig centreret og fed.

**Trin 2:**

```css
table, th, td {
     border: 1px solid black;
     border-collapse: collapse;
}
th, td {
     padding: 5px;
}
th {
     text-align: left;
}
```

Renderet: den samme tabel er nu mere luftig — 5px padding giver afstand mellem cellernes tekst og stregerne, og overskrifterne **Name**, **Family name** og **Id** er nu **venstrestillede** (stadig fede) og flugter med dataene under dem. Det er mærkbart lettere at læse.

## 9. CSS Structural Pseudo-classes

| Pseudo-class | Formål |
|---|---|
| `:first-of-type` | Gælder det første element af den angivne type |
| `:first-child` | Gælder det første child af et element |
| `:last-of-type` | Gælder det sidste element af den angivne type |
| `:last-child` | Gælder det sidste child af et element |
| `:nth-of-type(n)` | Gælder det "n'te" element af den angivne type. Værdier: et tal, `odd` eller `even` |

### Zebra Stripe a Table

```css
tr:nth-of-type(even) { background-color: #eaeaea; }
```

Hver anden række får lysegrå baggrund, hvilket gør brede tabeller langt lettere at følge vandret med øjet.

## 10. Table Row Groups

| Element | Indhold |
|---|---|
| `<thead>` | table head rows |
| `<tbody>` | table body rows |
| `<tfoot>` | table footer rows |

```html
<table> <caption>Time Sheet</caption>
  <thead>
    <tr>
      <th id="day">Day</th>
      <th id="hours">Hours</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td headers="day">Monday</td>
      <td headers="hours">4</td>
    </tr>
    <tr>
    …
  </tbody>
  <tfoot>
    <tr>
      <td headers="day">Total</td>
      <td headers="hours">18</td>
    </tr>
  </tfoot>
</table>
```

<!-- uoverensstemmelse i kilden: kodeeksemplet skriver Total = 18, men den renderede tabel på samme slide viser Total = 28, hvilket svarer til summen 4+8+8+5+3 -->

Den renderede tabel "Time Sheet" viser strukturen visuelt: en kraftig blå overskriftsrække med **Day** og **Hours** (thead), fem hvide/lyseblå skiftevis stribede datarækker Monday 4, Tuesday 8, Wednesday 8, Thursday 5, Friday 3 (tbody), og nederst en kraftig blå fed række **Total 28** (tfoot). Foroven står caption'en "Time Sheet" i fed, større skrift.

## 11. Table Row Groups – CSS

```css
th {
    text-align: left;
}

table caption {
    font-weight: bold;
    font-size: larger;
}

table thead tr {
    background-color: #0026ff;
    font-size: larger;
}

table tfoot tr {
    background-color: #0026ff;
    font-size: larger;
    font-weight: bold;
}

table tr:nth-of-type(even) {
    background-color: #d4d9f3;
}
```

Pointen er at row groups giver præcise "kroge" at style efter: `thead` og `tfoot` kan få deres egen kraftige blå baggrund og større skrift uafhængigt af body-rækkerne, som får zebra-striber via `:nth-of-type(even)`.

## 12. References & Links

<!-- slidet "References & Links" er tomt i kilden -->
