# Lab 14 – JavaJam 03 (table og form)

## Metadata

- **Lab-nummer:** Lab 14 – JavaJam 03
- **Kursus:** Front-end udvikling (SW4FED-02), 4. semester diplomingeniør softwareteknologi, Aarhus Universitet
- **Relateret lektion:** L14 – Web design, tabeller, formularer og Bootstrap
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L14/Lab14 JavaJam 03.pdf (2 sider)
- **Formål:** At opnå erfaring med brug af `table`- og `form`-elementerne i den gennemgående JavaJam-case (en kaffebar-hjemmeside), herunder styling af tabel og formular med CSS.
- **Emner dækket:**
  - `table`-elementet i HTML
  - Styling af tabeller med CSS
  - Omkodning af eksisterende side (menu.html) til tabelbaseret indhold
  - `form`-elementet i HTML
  - `action` og `method` (GET) på en formular
  - Styling af formular med CSS
  - Tilføjelse af udleveret fil (acknowledge.html) til projektet

---

## 1. Formål

At opnå erfaring med brug af `table`- og `form`-elementerne.

## 2. Forudsætninger

At du har lavet tidlige JavaJam-opgaver og læst om tables og forms.

## 3. Opgaven

### Delopgave 1

Omkod siden `menu.html` til at bruge et `table`-element, og brug CSS til at style tabellen, så menu-siden ser ud som på figur 1.

**Figur 1 – menu**

Figuren viser et browservindue på `localhost:26568/Home/Menu` med den færdige menu-side:

- Øverst en banner-header med logoet "JavaJam Coffee House" i en dekorativ skrifttype, på brun baggrund.
- I venstre side en lys navigationskolonne med fire links: Home, Menu, Music, Jobs.
- I hovedområdet en tabel med tre rækker og to kolonner. Venstre kolonne indeholder kaffenavnet i fed (**Just Java**, **Cafe au Lait**, **Iced Cappuccino**), højre kolonne beskrivelsen plus pris:
  - *Just Java* — "Regular house blend, decaffeinated coffee, or flavor of the day." Endless Cup $2.00
  - *Cafe au Lait* — "House blended coffee infused into a smooth, steamed milk." Single $2.00 Double $3.00
  - *Iced Cappuccino* — "Sweetened espresso blended with icy-cold milk and served in a chilled glass." Single $4.75 Double $5.75
- Rækkerne har skiftevis baggrundsfarve (lys beige / lysere gul), og tabellen fylder hovedområdets bredde.
- Nederst en footer med "Copyright © 2018 JavaJam Coffee House" og mailadressen JavaJam@nowhere.com som link.

### Delopgave 2

Tilføj siden **Jobs**, som består af en `form`, som er stylet med brug af CSS.

Forms `action` skal gå til filen `"acknowledge.html"`, og dens `method` skal være `get`.

Download filen `"acknowledge.html"` fra Brightspace og tilføj den til projektet.

## 4. Tekniske noter

Formularens attributter skal sættes præcis som angivet i opgaven:

```html
<form action="acknowledge.html" method="get">
  <!-- felter -->
</form>
```

Tabellen på menu-siden bygges med de sædvanlige tabelelementer og styles efterfølgende med CSS, f.eks.:

```css
table {
  border-collapse: collapse;
}
```

<!-- uklart i kilden: opgaveteksten angiver ikke konkrete felter i formularen eller konkrete CSS-regler/kolonner i tabellen — kun at resultatet skal svare til figur 1. -->
