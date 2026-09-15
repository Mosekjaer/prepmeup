# L13 – Graphics i HTML og CSS

## Metadata

- **Lektion:** L13 – Graphics in HTML and CSS
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L13/FED Graphics in HTML and CSS.pdf (39 slides)
- **Emner dækket:**
  - `<hr>`, CSS `border` og individuelle border-sider
  - Block vs. inline display-elementer
  - `padding` og shorthand-notation
  - CSS3 rounded corners, `box-shadow`, `text-shadow`, `opacity`, gradients
  - HTML5 `<meter>` og `<progress>`
  - `<img>`, `srcset`, `<figure>`/`<figcaption>`, `<picture>`
  - Grafiktyper: GIF, JPEG, PNG, AVIF
  - Accessibility og images, image links, thumbnails
  - Image optimization og filnavngivning
  - CSS `background-image` og `background-repeat`
  - Image maps og favicon

---

## 1. Horizontal Rule Element

Et horizontal rule-element adskiller områder på en side med en linje tværs over siden. I HTML5 indikerer det et **thematic break**. Det kodes som et void tag: `<hr/>`.

### Kodeeksempel

```html
<p>
 Our expert designers are creative and eager to work with you.
 Take advantage of the power of Web 2.0!
</p>
<hr />
<div>Copyright &copy; 2012 Terry</div>
```

## 2. CSS border

Konfigurerer en border omkring et HTML-element. Som default har borderen en width sat til 0 og vises ikke.

Man kan sætte:

- `border-width: 2px` (eller `thin`, `medium`, `thick`)
- `border-color:` enhver gyldig farve
- `border-style:` `solid`, `dashed`, `dotted`, `double` … eller `none`
- `border-radius: 5px` (1-4 værdier i px, em eller %)
- `border: 1px solid #00f0f0`

Man kan også sætte de enkelte border-sider individuelt.

## 3. Block / Inline Elements

- **Block display element** — default width af elementets indhold strækker sig til browser-marginen (eller den angivne width). For eksempel: `h2 { border: 2px solid #ff0000; }`
- **Inline display element** — borderen omslutter tæt elementets indhold. For eksempel: `a { border: 2px solid #0000ff; }`

## 4. Individual Border Sides

Brug CSS til at konfigurere en linje på en eller flere sider af et element:

- `border-bottom`
- `border-left`
- `border-right`
- `border-top`

### Kodeeksempel

```css
h2 { border-bottom: 0.1em solid #d26416 }
```

## 5. Padding

CSS `padding`-propertyen konfigurerer tomt rum mellem indholdet af HTML-elementet og borderen. Sat til `0px` som default.

### Kodeeksempel

```css
h3 {
       border: 2px solid #ff0000;
       padding: 5px;
   }
```

## 6. Padding on Specific Sides

Brug CSS til at konfigurere padding på en eller flere sider af et element:

- `padding-top: 30px`
- `padding-right: 10px`
- `padding-bottom: 5px`
- `padding-left: 20px`

Eller brug shorthand-notationer:

- `padding: 10px 5px` (første: top og bund, anden: venstre og højre)
- `padding: 30px 10px 5px 20px` (top, right, bottom og left)

### Kodeeksempel

```css
h4 {
        border: 2px solid #00ff00;
        width: 90%;
        background-color: #cccccc;
        padding: 30px 10px 5px 20px;
    }
```

## 7. CSS3 Rounded Corners

`border-radius`-propertyen konfigurerer den horisontale radius og vertikale radius af hjørnet. Numeriske værdier med enhed (pixel, em eller percentage).

Browser vendor proprietary properties:

- `-webkit-border-radius` (til Safari og Chrome)
- `-moz-border-radius` (til Firefox)
- `border-radius` (W3C-syntaks)

### Kodeeksempel

```css
h1 { -webkit-border-radius: 15px;
     -moz-border-radius: 15px;
      border-radius: 15px;
}
```

## 8. CSS3 box-shadow Property

`box-shadow`-propertyen gør det muligt at kaste en drop shadow fra rammen af næsten ethvert element. Man konfigurerer horizontal offset, vertical offset, blur radius, spread radius og en gyldig farveværdi.

### Kodeeksempel

```css
/* Keyword values */
box-shadow: none;

/* offset-x | offset-y | color */
box-shadow: 60px -16px teal;

/* offset-x | offset-y | blur-radius | color */
box-shadow: 10px 5px 5px black;

/* offset-x | offset-y | blur-radius | spread-radius | color */
box-shadow: 2px 2px 2px 1px rgba(0, 0, 0, 0.2);

/* inset | offset-x | offset-y | color */
box-shadow: inset 5em 1em gold;
```

```css
#wrapper {box-shadow: 5px 5px 5px #828282;}
```

Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/box-shadow

## 9. CSS3 text-shadow Property

Konfigurerer horizontal offset, vertical offset, blur radius og en gyldig farveværdi.

### Kodeeksempel

```css
h1 {
       text-shadow: 5px 2px 2px #666;
```

## 10. CSS3 opacity Property

Konfigurerer opaciteten af baggrundsfarven. Opacity-området er 0 (completely transparent) til 1 (completely opaque).

### Kodeeksempel

```css
h1{ background-color: #FFFFFF;
    opacity: 0.6; }
```

## 11. CSS3 Gradients

En gradient er en glidende blanding af nuancer fra én farve til en anden. CSS3 definerer to typer gradienter:

- **Linear Gradients** — går ned/op/venstre/højre/diagonalt.
- **Radial Gradients** — defineret ved deres centrum.

### Kodeeksempel — lineær gradient fra top til bund

```css
background: -webkit-linear-gradient(red, green); /* For Safari 5.1 to 6.0 */
background: -o-linear-gradient(red, green); /* For Opera 11.1 to 12.0 */
background: -moz-linear-gradient(red, green); /* For Firefox 3.6 to 15 */
background: linear-gradient(red, green); /* Standard syntax */
```

Flere eksempler: http://www.w3schools.com/css/css3_gradients.asp

## 12. HTML5 Meter Element

Viser en visuel måler af en numerisk værdi inden for et kendt område. Repræsenterer et element, hvis range er kendt — det vil sige, at det har definerede minimum- og maksimumværdier.

### Kodeeksempel

```html
<meter value="14417" min="0" max="14417">14417</meter>14,417 Total Visits<br>
<meter value="7000" min="0" max="14417">7000</meter> 7,000 Firefox<br>
<meter value="3800" min="0" max="14417">3800</meter> 3,800 Internet Explorer<br>
<meter value="2062" min="0" max="14417">2062</meter> 2,062 Chrome<br>
<meter value="1043" min="0" max="14417">1043</meter> 1,043 Safari<br>
<meter value="312" min="0" max="14417">312</meter> &nbsp;&nbsp; 312 Opera<br>
<meter value="200" min="0" max="14417">200</meter> &nbsp;&nbsp; 200 other<br>
```

## 13. HTML5 Progress Element

Viser en bar, der afbilder en numerisk værdi inden for et angivet område. `meter`-elementet har seks attributter: `value`, `max`, `min`, `high`, `low` og `optimum`.

### Kodeeksempel

```html
<h1>Your Task is in Progress</h1>
<p>
   Status: <progress max="100" value="37">
   <span>37</span>%
   </progress>
</p>
```

Slidesene viser, at Edge og Chrome renderer progress-baren forskelligt.

## 14. Img — HTML Image Element

Konfigurerer grafik på en webside.

### Kodeeksempel

```html
<img src="image.gif" alt="some text"
     height="100" width="100">
```

- `src`-attributten — filnavnet på grafikken (url).
- `alt`-attributten — konfigurerer alternate text content (beskrivelse).
- `height`-attributten — højden af grafikken i pixels.
- `width`-attributten — bredden af grafikken i pixels.

`height` og `width` sættes ofte i CSS i stedet.

## 15. The srcset Attribute

For at understøtte responsive images har HTML-standarden tilføjet `srcset`-attributten.

### Kodeeksempel

```html
<img src="defaultImage.png"
     srcset="image-1x.png 1x, image-2x.png 2x,
             image-3x.png 3x, image-4x.png 4x">
```

- På browsere uden `srcset`-understøttelse bruges værdien af `src`-attributten som image source (`defaultImage`).
- På regular resolution displays bruges `1x`-varianten af `srcset` (`image-1x`).
- På displays med 2 device pixels pr. CSS pixel bruges `2x`-varianten (`image-2x`).
- Tilsvarende findes et `3x`- og et `4x`-billede. Og man kan også have en `1.5x`-version.

## 16. Figure og Figcaption Elements

`figure`-elementet indeholder en enhed af indhold, som er selvstændigt — såsom et billede — sammen med ét valgfrit `figcaption`-element.

### Kodeeksempel

```html
<figure>
 <img src="lighthouseisland.jpg" width="250"
    height="355"
    alt="Lighthouse Island">
   <figcaption>
   Island Lighthouse, Built in 1870
   </figcaption>
</figure>
```

## 17. Responsive Images med picture

`<picture>`-elementet muliggør progressiv understøttelse, da man kan liste image sources i den rækkefølge, man ønsker dem indlæst, og browseren indlæser den første, den understøtter. Det lader også forfatteren deklarativt styre eller give hints til user agenten om, hvilken image resource der skal bruges, baseret på screen pixel density, viewport size, image format og andre faktorer.

### Kodeeksempel

```html
<picture>
      <source media="(min-width: 800px)" srcset="head.jpg, head-2x.jpg 2x">
      <source media="(min-width: 450px)" srcset="head-small.jpg, head-small-2x.jpg 2x">
      <img src="head-fb.jpg" srcset="head-fb-2x.jpg 2x" >
</picture>
```

## 18. Types of Graphics

Grafiktyper, der ofte bruges på websider: GIF, JPG og PNG.

**GIF — Graphics Interchange Format**

- Bedst til line art og logoer.
- Maksimum 256 farver.
- Én farve kan konfigureres som transparent.
- Kan animeres.
- Bruger lossless compression.
- Kan interlaces.

**JPEG — Joint Photographic Experts Group**

- Bedst til fotografier.
- Op til 16,7 millioner farver.
- Bruger lossy compression.
- Kan ikke animeres.
- Kan ikke gøres transparent.
- Progressive JPEG minder om interlaced display.

**PNG — Portable Network Graphic**

- Understøtter millioner af farver.
- Understøtter flere niveauer af transparens (men browsere gør ikke — så begræns til én transparent farve til webvisning).
- Understøtter interlacing.
- Bruger lossless compression.
- Kombinerer det bedste fra GIF og JPEG.

## 19. AVIF

En superkomprimeret billedtype. AVIF giver betydelig filstørrelsesreduktion for billeder sammenlignet med JPEG eller WebP:

- ~50 % besparelse sammenlignet med JPEG.
- ~20 % besparelse sammenlignet med WebP.

Formatet blev udviklet af Alliance for Open Media i samarbejde med Google, Cisco og Xiph.org.

### Kodeeksempel

```html
<picture>
  <source srcset="img/photo.avif" type="image/avif">
  <source srcset="img/photo.webp" type="image/webp">
  <img src="img/photo.jpg" alt="Description of Photo">
</picture>
```

## 20. Accessibility og Images

**Anbefalet**

- Hvis dit sites navigation bruger image links til hovednavigationen, så tilbyd simple tekstlinks i bunden af siden.

**Påkrævet**

- Konfigurér `alt`-attributten.
  - Alternate text content, der formidler betydningen/hensigten med billedet.
  - IKKE filnavnet på billedet.
  - Brug `alt=""` til rent dekorative billeder.

## 21. Image Links

For at lave et image hyperlink bruges et anchor-element til at indeholde et image-element.

### Kodeeksempel

```html
<a href="index.html"><img src="home.gif"
height="19" width="85" alt="Home"></a>
```

Browsere tilføjer automatisk en border til image links. Konfigurér CSS for at fjerne borderen:

```css
img {border-style:none; }
```

## 22. Thumbnail Image

Et lille billede konfigureret til at linke til en større version af samme billede.

### Kodeeksempel

```html
<a href="big.jpg"><img src="small.jpg"
alt="country road" width="200" height="100"></a>
```

## 23. Image Optimization

Processen med at skabe et billede med den laveste filstørrelse, som stadig renderer et billede af god kvalitet — altså at balancere billedkvalitet og filstørrelse.

Image optimization består i at:

- Reducere filstørrelsen på billedet.
- Reducere dimensionerne på billedet til den faktiske bredde og højde af billedet på websiden.

Image editing tools:

- Squoosh web app — https://squoosh.app/
- GIMP
- Adobe Photoshop
- Paint.NET — http://www.getpaint.net/
- http://pixlr.com/editor

## 24. Valg af navne til billedfiler

- Brug udelukkende små bogstaver.
- Brug ikke tegnsætningssymboler og mellemrum.
- Ændr ikke filendelserne (skal være `.gif`, `.jpg`, `.jpeg` eller `.png`).
- Hold filnavnene korte, men beskrivende.
  - `i1.gif` er formentlig for kort.
  - `myimagewithmydogonmybirthday.gif` er for langt.
  - `dogbday.gif` er nok tilpas.

## 25. Organizing Your Site

- Placér billeder i deres egen mappe.
- Kod stien til filen i `src`-attributten.

### Kodeeksempel

```html
<img src="images/home.gif" alt="Home"
height="100" width="200">
```

## 26. CSS background-image

Konfigurerer et background image. Som default gentages background images.

### Kodeeksempel

```css
body { background-image: url(background.gif); }
```

## 27. CSS background-repeat Property

`background-repeat` styrer, hvordan baggrundsbilledet gentages — for eksempel `repeat`, `repeat-x`, `repeat-y` og `no-repeat`.

### Kodeeksempel — brug af background-repeat med `trilliumbullet.gif`

```css
h2 {
  color: #5c743d;
  font-family: Georgia, "Times New Roman", serif;
  padding-left: 30px;
  background-color: #d5edb3;
  background-image: url(trilliumbullet.gif);
  background-repeat: no-repeat;
}
```

## 28. CSS3 Multiple Background Images

### Kodeeksempel

```css
body { background-color: #f4ffe4;
       color: #333333;
       background-image: url(trilliumgradient.png);
       background: url(trilliumfoot.gif)
                    no-repeat bottom right,
                    url(trilliumgradient.png);
}
```

## 29. Image Map

- `map`-elementet definerer selve mappet.
- `area`-elementet definerer et specifikt område på et map. Det kan sættes til et rectangle, circle eller polygon via attributterne `href`, `shape` og `coords`.

### Kodeeksempel

```html
<map name="boat" id="boat">
 <area href="http://www.doorcountyvacations.com"
       shape="rect"
       coords="24, 188, 339, 283" alt="Door County Fishing">
</map>
<img src="fishingboat.jpg" usemap="#boat" alt="Door County" width="416" height="350">
```

## 30. Favorites Icon

Et kvadratisk billede associeret med en webside. Normalt navngivet `favicon.ico` og placeret i root-mappen. Det kan vises i browserens adressebar, faneblad eller favorites/bookmarks-liste.

### Kodeeksempel

```html
<link rel="icon" href="favicon.ico" type="shortcut icon">
```

## 31. Guidelines for Using Images

- Genbrug billeder.
- Overvej billedets filstørrelse i forhold til billedkvalitet.
- Overvej billedets load time.
- Brug passende opløsning.
- Angiv dimensioner.
- Vær opmærksom på lysstyrke og kontrast.

## 32. Images and Accessibility

- Stol ikke på farve alene. Nogle besøgende kan have farveopfattelsesnedsættelser. Brug høj kontrast mellem baggrund og tekstfarve.
- Tilbyd et tekstækvivalent for non-text-elementer — brug `alt`-attributten på dine image-elementer.
- Hvis dit sites navigation bruger image links, så tilbyd simple tekstlinks i bunden af siden.

## 33. References & Links

- "Web Development and Design Foundations with HTML5" af Terry Felke-Morris
- W3schools — http://www.w3schools.com
- Check — http://caniuse.com
