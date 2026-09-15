# L14 – Formularer i HTML5

## Metadata

- **Lektion:** L14 – Forms in HTML5
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L14/FED Forms in HTML.pdf (44 slides)
- **Emner dækket:**
  - Formularens rolle: HTML-siden og den server-side behandling
  - `<form>`-elementets attributter: `action`, `method`, `name`, `id`
  - GET vs. POST og HTTP verb safety (idempotens)
  - Form controls: text, textarea, submit, reset, password, checkbox, radio, hidden
  - `<select>`, `<option>`, `<datalist>`, image button, `<button>`
  - HTML5 input-typer: email, url, tel, search, range, number, date, file
  - Accessibility: `<label>`, `<fieldset>`, `<legend>`, `placeholder`, `tabindex`, `accesskey`, `title`
  - Client-side validation: `required`, `pattern`, `maxlength`, `min`/`max`/`step`
  - Styling af formularer med CSS (transitional vs. "pure" CSS)

---

## Agenda

- Overview
- Form controls
- Accessibility & Forms
- Client-side validation
- Styling a Form

## 1. Overview of Forms

Formularer bruges overalt på nettet til at indtaste information.

Skærmbilledet viser en simpel demo-formular i browseren: overskriften "Simple Form in HTML5", derunder labelen "Name" med et tomt tekstfelt ved siden af, "Email" med et tomt tekstfelt, og nederst to knapper — "Send" (submit) og "Nulstil" (reset, browserens danske default-tekst). Bemærk at reset-knappens tekst kommer fra browserens sprogindstilling, ikke fra HTML'en.

## 2. Using Forms — de to halvdele

**1. The HTML form**

- Modtager input fra brugeren.
- Sender data til serveren når brugeren trykker på submit-knappen.
- Som default får denne datasending siden til at genindlæse efter afsendelsen, men med JavaScript kan man ændre den opførsel.

**2. The server-side processing**

- Når serveren modtager formulardata, kalder den den specificerede `action` (en metode eller et script) der behandler dataene.
- Det kunne fx:
  - Opdatere en database
  - Skrive til en fil
  - Sende e-mail
  - Eller udføre en anden form for behandling på serveren

## 3. Sample Form HTML

```html
<form id="demoform" action="AddToNewsletter" method="post">
   <label for="name">Name</label>
   <input type="text" id="name" name="name" /><br /><br />
   <label for="email">Email</label>
   <input type="text" id="email" name="email" /> <br /><br />
   <input type="submit" value="Send" id="submit"> <input type="reset">
</form>
```

Skærmbilledet under koden viser den renderede formular: labelen "Name" står til venstre for sit tekstfelt, "Email" tilsvarende under, og de to knapper "Send" og "Nulstil" nederst. Bemærk at `value="Send"` styrer submit-knappens tekst, mens reset-knappen uden `value` får browserens default-tekst.

## 4. HTML form element

Attributter:

| Attribut | Betydning |
|---|---|
| `action` | Angiver det server-side program eller script der vil behandle dine formulardata — sti-delen af URL'en |
| `method` | `get` (default) — formulardata sendes i URL'en. `post` — mere sikkert, formulardata sendes i request body |
| `name` | Identificerer formularen på serveren og klienten |
| `id` | Identificerer formularen på klienten |

## 5. GET, POST, and HTTP Verb Safety

En vigtig skelnen mellem disse verber er, at en **GET**-operation ikke bør ændre noget på serveren.

- Lidt mere abstrakt: en GET-operation resulterer ikke i en tilstandsændring på serveren.
- Du kan udføre en GET-operation på de samme ressourcer så mange gange du vil, og de ressourcer ændrer sig ikke.
- En GET bør være **idempotent**.

En **POST**-request ændrer noget på serveren hver gang du udfører operationen.

### GET vs. POST i praksis

**GET:** Når du foretager en søgning med en søgemaskine som Google, udfylder du en formular der består af én tekstboks, og klikker på søgeknappen. Browseren udfører en GET-operation, hvor værdien du indtastede sendes med som en del af URL'en. At bruge GET til denne type formular er fint, fordi en søgeoperation ikke ændrer nogen ressourcer på serveren — den henter blot information.

**POST:** Overvej så processen med at bestille noget online. Du udfylder ordredetaljerne og klikker submit. Denne operation vil være en POST-request, fordi operationen vil resultere i ændringer på serveren, som en ny ordre-record, en ændring i dine kontooplysninger, og måske mange andre ændringer.

## 6. Form controls

### Input Text box

```html
<input type="text" name="name" placeholder="Enter your name"/>
```

Accepterer tekstinformation. Attributter: `name`, `id`, `size`, `maxlength`, `value`, `placeholder`.

Slidet fremhæver i en note: **du skal give feltet et `name`, for at dets indhold bliver sendt til serveren når formularen submittes.** Uden `name` sendes feltet slet ikke — `id` alene er ikke nok.

### textarea – Scrolling Text Box

```html
<textarea> </textarea>
```

Konfigurerer en scrollende tekstboks. Attributter: `name`, `id`, `cols`, `rows`.

Skærmbilledet viser en "Sample Scrolling Text Box" med labelen "Comments:" og under den en flerlinjet boks der indeholder teksten "Enter your comments here". I højre side af boksen er der en lodret scrollbar med pile op og ned — det er præcis dét der adskiller en textarea fra en almindelig text input: den kan rumme og scrolle flere linjer.

### input Submit Button

```html
<input type="submit">
```

Submitter formularinformationen — svarer til en enter-knap. Når der klikkes:

- Trigger `action`-metoden på `<form>`-tagget.
- Sender formulardataene (name=value-parret for hvert formularelement) til webserveren.

Attributter: `name`, `id`, `value`.

### input Reset Button

```html
<input type="reset">
```

Nulstiller formularfelterne til deres initielle værdier. Attributter: `name`, `id`, `value`.

### input Password box

```html
<input type="password">
```

Accepterer tekstinformation der skal skjules, mens den indtastes. Attributter: `name`, `id`, `maxlength`, `value`.

### input Check box

```html
<input type="checkbox">
```

Tillader brugeren at vælge ét eller flere elementer fra en gruppe af foruddefinerede items. Attributter: `name`, `id`, `checked`, `value`.

Skærmbilledet viser en "Sample Check Box" med teksten "Choose the browsers you use:" og tre uafhængige afkrydsningsfelter under hinanden: Internet Explorer, Firefox og Opera. Alle tre kan markeres samtidig — det er hele pointen med checkboxes.

### input Radio Button

```html
<input type="radio" name="color" value="Black">
```

Tillader brugeren at vælge **præcis én** fra en gruppe af foruddefinerede items.

- Hver radio button i en gruppe får **samme `name`** og en **unik `value`**.
- Der er altid ét element checked. Det første item er det der er checked som default.

```html
<input type="radio" name="color" value="yellow">
<input type="radio" name="color" value="red">
<input type="radio" name="color" value="blue">
```

Attributter: `name`, `id`, `checked`, `value`. Det fælles `name` er dét der binder knapperne sammen til én gruppe, hvor kun én kan være valgt.

### input Hidden form data

```html
<input type="hidden">
```

Denne form control vises ikke på websiden.

- Hidden form fields kan tilgås af både client-side og server-side scripting.
- Bruges nogle gange til at indeholde information der er nødvendig, mens den besøgende bevæger sig fra side til side.

Attributter: `name`, `id`, `value`.

### Select List

```html
<select></select>
```

Konfigurerer en select list. Også kendt som: Select Box, Drop-Down List, Drop-Down Box og Option Box. Tillader brugeren at vælge ét eller flere items fra en liste af foruddefinerede valg.

Attributter: `name`, `id`, `size`, `multiple`.

`multiple` er en Boolean attribut der angiver at flere options kan vælges i listen. Hvis den ikke er angivet, kan kun én option vælges ad gangen.

### Options in a Select List

```html
<label>Select your choice: </label>
<select name="select_example" >
  <option value="home">Home</option>
  <option value="products" selected>Products</option>
  <option value="services">Services</option>
  <option value="about">About</option>
  <option value="contact">Contact</option>
</select>
```

Attributter: `value`, `selected`.

Skærmbilledet viser den renderede kontrol: en lille dropdown ved siden af teksten "Select your choice:" hvor der står **"Products"** med en lille pil ned. Det er `selected`-attributten der gør at Products vises som forvalgt i stedet for det første element (Home).

### `<datalist>` element

`<datalist>`-elementet repræsenterer listen af `<option>`-elementer der skal foreslås, når man udfylder et `<input>`-felt.

```html
<label for="color">Favorite Color:</label>
<input type="text" name="color" id="color" list="colors" />
<datalist id="colors">
   <option value="red">
   <option value="green">
   <option value="blue">
   <option value="yellow">
   <option value="pink">
   <option value="black">
</datalist>
```

To skærmbilleder viser adfærden:

1. **Tomt felt, klikket:** hele listen af forslag foldes ud under tekstfeltet — red, green, blue, yellow, pink, black — og "yellow" er fremhævet fordi musen holder over den.
2. **Efter at have skrevet "r":** kun forslaget "red" vises under feltet.

Forskellen fra `<select>` er altså at brugeren stadig kan skrive en fri værdi — `<datalist>` **foreslår** kun, den begrænser ikke.

### Input Image Button

```html
<input type="image">
```

Submitter formularen. Når der klikkes: trigger `action`-metoden på form-tagget og sender formulardataene til webserveren.

Attributter: `name`, `id`, `src` (definerer billedets kilde), `alt` (definerer alternativ tekst), `height`, `width`.

### Button Element

```html
<button type="button"></button>
```

Et container-tag. Når der klikkes, afhænger dets funktion af værdien af `type`-attributten. Kan indeholde en kombination af tekst, billeder og medier. De bruges til programmatisk at gøre noget, ved hjælp af JavaScript.

Attributter: `type="submit"`, `"reset"` eller `"button"`, samt `name`, `id`, `alt`, `value`.

## 7. HTML5 input-typer

| Type | Betydning |
|---|---|
| `<input type="email">` | Accepterer tekstinformation i e-mail-adresseformat — **og validerer** |
| `<input type="url">` | Accepterer tekstinformation i URL-format — og validerer |
| `<input type="tel">` | Accepterer tekstinformation i telefonnummerformat — og validerer |
| `<input type="search">` | Accepterer søgetermer |

Skærmbilledet på email-slidet viser valideringen i aktion: i formularen "Join Our Newsletter" er der skrevet "Dr. Morris" i E-mail-feltet, feltet er markeret med rød ramme, og browseren viser en gul tooltip: **"Please enter an email address."** Formularen submittes ikke. Valideringen er altså gratis — den følger af `type="email"` alene.

### Slider Control

```html
<label for="myChoice">
Choose a number between 1 and 100:</label><br>
Low <input type="range" name="myChoice" id="myChoice"> High
```

Skærmbilledet viser den renderede kontrol: teksten "Choose a number between 1 and 100:", og under den ordet "Low", en vandret skydebane med et lille rundt greb placeret cirka i midten, og ordet "High" i højre side. Der vises ingen talværdi — det er en ren visuel skyder.

### Spinner Control

```html
<label for="myChoice">Choose a number between 1 and 10:</label>
<input type="number" name="myChoice" id="myChoice"
    min="1" max="10">
```

Skærmbilledet viser et smalt tekstfelt med tallet "3" indtastet og et lille par op/ned-pile i højre kant af feltet. Til forskel fra slideren ser man her den præcise værdi og kan taste den ind eller klikke sig op og ned.

### Calendar Control

```html
<label for="myDate">Choose a Date</label>
<input type="date" name="myDate" id="myDate" />
<input type="submit" /> <input type="reset" />
```

Slidet viser **tre** skærmbilleder af den samme `type="date"`-kontrol i tre forskellige browsere — og det er hele pointen: udseendet er browserens ansvar, ikke dit.

1. Én browser viser et felt med formatet `dd-mm-åååå` og en udfoldet kalender for "april 2018" med ugedagene man–sø og den 13. markeret.
2. En anden viser formatet `mm/dd/yyyy` og en helt anden vælger: tre lodrette rullehjul med dag, måned og år, hvor "13", "april" og "2018" er fremhævet, med et flueben og et kryds nederst.
3. Den tredje viser igen `dd/mm/åååå` og en udfoldet månedskalender i en mere moderne stil, med weekend-dage i rødt.

Bemærk at knapteksterne også varierer ("Indsend"/"Nulstil", "Submit Query"/"Reset", "Send forespørgsel"/"Nulstil") efter browser og sprog.

### File uploads

Du kan indlæse filer fra din lokale computer og sende dem til serveren:

```html
<input type="file" name="secret-documents">
```

Du kan vedhæfte flere filer:

```html
<input type="file" name="secret-documents" multiple>
```

Du kan angive en eller flere tilladte filtyper:

```html
<input type="file" name="secret-documents" accept=".jpg, .jpeg, .png">
```

## 8. Accessibility & Forms

Emner: Label Element, Fieldset Element, Legend Element, Placeholder attribute, Tabindex Attribute, Accesskey Attribute.

### Label element

`<label></label>` associerer en tekstlabel med en form control. To forskellige formater:

```html
<label>Email: <input type="text" name="email" id="email"></label>
```

Eller:

```html
<label for="email">Email: </label>
<input type="text" name="CustEmail" id="email">
```

I den første form omslutter labelen selve input-feltet; i den anden binder `for`-attributten labelen til feltets `id`. Begge dele gør at et klik på labelteksten sætter fokus i feltet, og at en skærmlæser kan læse feltets betydning op.

### Fieldset and Legend Elements

**The Fieldset Element** — container tag, skaber en visuel gruppe af formularelementer på en webside.

**The Legend Element** — container tag, skaber en tekstlabel inde i fieldset'et.

```html
<fieldset><legend>Customer Information</legend>
   <label>Name:
   <input type="text" name="Name" id="Name"></label>
   <br><br>
   <label>Email:
   <input type="text" name="Email" id="Email"></label>
 </fieldset>
```

Skærmbilledet viser resultatet: en tynd ramme omkring de to felter, hvor teksten "Customer Information" er indlejret i selve rammens øverste kant (rammen brydes omkring teksten). Inde i rammen står "Name:" og "Email:" med hvert sit tekstfelt.

### placeholder attribute

`placeholder`-attributten på `<input>`- og `<textarea>`-elementer giver brugeren et hint om hvad der kan indtastes i feltet.

- Placeholder-teksten må ikke indeholde carriage returns eller line-feeds.

```html
<input type="text" id="name" name="name"
       placeholder="Please enter your name"/>
```

Bemærk at en placeholder **ikke** erstatter en label — den forsvinder så snart brugeren begynder at skrive.

### tabindex attribute

En attribut der kan bruges på form controls og anchor tags. Ændrer default-tab-rækkefølgen. Tildel en numerisk værdi.

```html
<input type="text"
       name="CustEmail"
       id="CustEmail"
       tabindex="1"
>
```

### accesskey attribute

En attribut der kan bruges på form controls og anchor tags. Skaber en "hot-key"-kombination der placerer fokus på komponenten. Tildel værdien af et tastaturbogstav. På Windows bruges CTRL plus "hot-key" til at flytte cursoren.

```html
<input type="text" name="CustEmail" id="CustEmail"
       accesskey="E" />
```

### title attribute

Hvis `title`-attributten er sat på `<input>`-elementet, bruges dens værdi som **tooltip**. Hvis valideringen fejler, erstattes denne tooltip-tekst med den tilhørende fejlbesked.

```html
<input type="email" title="Please, provide an e-mail" />
```

Skærmbillederne viser begge tilstande: i det ene svæver tooltip'en "Please, provide an e-mail" under Name-feltet som et hint; i det andet vises den samme tekst i en gul/orange kasse ved siden af Email-feltet efter et mislykket submit — nu som fejlbesked.

## 9. Client-side validation

> Selvom denne funktionalitet **ikke** erstatter server-side validation — som stadig er nødvendig af hensyn til sikkerhed og dataintegritet — kan client-side validation understøtte en bedre brugeroplevelse.

### required attribute

`required`-attributten på elementer angiver at der **skal** angives en værdi.

```html
<input type="email" required />
```

Skærmbilledet viser effekten: der klikkes submit med et tomt Name-felt, feltet får en rød ramme, og browseren viser en boble med teksten **"This is a required field"**. Formularen sendes ikke afsted.

### pattern attribute

`pattern`-attributten på `<input>`-elementet begrænser værdien til at matche et bestemt regulært udtryk.

```html
<input type="password" id="password" name="password"
       required pattern="[\w]{8,}" />
```

Skærmbilledet viser en formular hvor Password-feltet indeholder tre tegn, har rød ramme, og en tooltip siger: **"You must use this format: min 8 alphanumerics"** — teksten kommer fra `title`-attributten. Det regulære udtryk `[\w]{8,}` betyder "mindst 8 word-tegn".

### maxlength attribute

`maxlength`-attributten på `<input>`- og `<textarea>`-elementer begrænser det maksimale antal tegn brugeren kan indtaste.

### min, max, and step attributes

- `min` og `max` på `<input>`-elementet begrænser de minimum- og maksimumværdier der kan indtastes.
- `step` på `<input>`-elementet begrænser granulariteten af de værdier der kan indtastes.
  - Kan kun bruges sammen med `min` og `max`.

```html
<input type="number" name="a-number" min="10" max="50" step="5">
```

Her kan brugeren altså kun vælge 10, 15, 20, … 50.

## 10. Styling a Form

### Format a Form with a Table (gammel metode)

```html
<form method="get">
<table border="0">
  <tr>
    <td align="right">Name:</td>
    <td><input type="text" name="fmail" id="fmail"></td>
  </tr>
  <tr>
    <td align="right">E-mail:</td>
    <td><input type="text" name="email" id="email"></td>
  </tr>
  <tr>
    <td align="right" valign="top">Comments:</td>
    <td><textarea name="comments" id="comments" rows="4"
                  cols="40"></textarea></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><input type="submit" value="Contact">
        &nbsp; <input type="reset"></td>
  </tr>
</table>
</form>
```

Slidet er stemplet med en rød, skrå håndskrift-note: **"Old approach – Do not use!"** Skærmbilledet viser dog at resultatet ser pænt ud: labels højrestillet i venstre kolonne, felterne pænt flugtende i højre kolonne, og de to knapper nederst under felterne. Problemet er semantisk — en tabel skal bruges til tabulære data, ikke til layout.

### Using CSS to Style a Form – Transitional Approach

Brug en tabel til at formatere formularen, men konfigurér styles i stedet for HTML table-attributter.

```css
table { background-color: #eaeaea;
        border-style: none;
        width: 20em;
        font-family: Arial, sans-serif; }
th { font-weight: normal;
     text-align: right;
     vertical-align: top; }
td, th {padding: 5px; }
```

Skærmbilledet viser en "Contact Us"-formular på lysegrå baggrund med Arial-skrift: labels "Name:", "E-mail:" og "Comments:" højrestillet, felterne pænt flugtende, og en Submit-knap nederst. `th` bruges nu til labels med `font-weight: normal` (så de ikke er fede) og `vertical-align: top` (så "Comments:" flugter med toppen af den høje textarea).

### Using CSS to Style a Form – "Pure" CSS Approach

- Brug **ikke** en tabel til at formatere formularen.
- Brug CSS `float` og `display: block`.

```css
form { background-color:#eaeaea; width: 350px;
       font-family: Arial, sans-serif; padding: 10px;
}
label { float: left; width: 100px; display: block;
        clear: left; text-align: right; padding-right: 10px;
        margin-top: 10px;
}
input, textarea { margin-top: 10px; display: block; }
#mySubmit { margin-left: 110px; }
```

Slidet viser en skematisk skitse af resultatet: en `form`-kasse der indeholder tre rækker med et lille "label"-felt til venstre og et bredere felt til højre (to "text box"-felter og én "scrolling text box"), samt en "submit button" nederst der er rykket ind så den flugter med felterne, ikke med labels.

Mekanikken: `label` gøres til en block på 100px bredde der floater til venstre med højrestillet tekst; `clear: left` sikrer at hver ny label starter på en ny linje; `input` og `textarea` gøres til blocks så de lander til højre for den floatede label. Submit-knappen har ingen label og skubbes derfor manuelt ind med `margin-left: 110px`.

## 11. References & Links

- The HTML handbook af Flavio Copes — https://flaviocopes.com/page/ebooks-links/
- "Web Development and Design Foundations with HTML5" af Terry Felke-Morris, sixth edition, ISBN-13: 9780273774501
- Forms in HTML (MDN) — https://developer.mozilla.org/en-US/docs/Learn/Forms
- How To Build An Awesome Form — https://medium.com/@kubachrzecijanek/how-to-build-an-awesome-form-1e9b2c1bd00d
