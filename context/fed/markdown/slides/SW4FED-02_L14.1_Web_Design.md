# L14 – Web Design

## Metadata

- **Lektion:** L14 – Web Design
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L14/FED Web Design.pdf (41 slides)
- **Emner dækket:**
  - Design i forhold til sitets formål og målgruppe
  - Website-organisering: hierarkisk, lineær og random ("web"/wiki)
  - For flad vs. for dyb hierarki, information chunking, Three Click Rule
  - Design principles: repetition, contrast, proximity, alignment
  - Accessibility, WCAG 2.0 og POUR-principperne
  - Skrivning for web, læsbar tekst, farvevalg og grafik-best practices
  - Navigation design, wireframes og page layout (ice/jello/liquid)
  - Load time, screen resolution og browser compatibility
  - Web fonts og prototyping-værktøjer
  - Usability-lektioner fra Steve Krugs "Don't Make Me Think"

---

## 1. Introduktion

Titelslidet viser et foto af en vifte af spidse farveblyanter i mange farver — et billede på designvalg. Under står Shaker-filosofiens citat:

> "Don't make something unless it is both necessary and useful; but if it is both necessary and useful, don't hesitate to make it beautiful."
>
> — Shaker philosophy

## 2. Overall Design Is Related to the Site Purpose

Slidet viser to skærmbilleder af meget forskellige websites, med teksten "Consider the target audience of these sites":

- **NASA's forside:** mørk baggrund, et stort fotografi af en satellit i rummet, dominerende visuelt indhold, billed-baserede navigationsfliser (Shuttle & Station, Solar System, Beyond Earth, Commercial Space, Universe, Earth). Designet er visuelt og inspirerende — det henvender sig til et bredt, nysgerrigt publikum.
- **U.S. Bureau of Labor Statistics, Consumer Price Index:** lys baggrund, tætpakket tekst, mange links, tabeller og talrækker i sidebaren, formel typografi. Designet er informationstungt og funktionelt — det henvender sig til fagfolk der skal finde specifikke data.

Pointen: det overordnede design skal følge af sitets formål og målgruppe. Ingen af de to designs ville fungere for det andet sites publikum.

## 3. WebSite Organization

- Hierarchical
- Linear
- Random (kaldes nogle gange Web Organization)

### Hierarchical Organization

- En klart defineret home page
- Navigationslinks til de store site-sektioner
- Bruges ofte til kommercielle og corporate websites

Diagrammet viser et træ: øverst **Home**, som forgrener sig til tre bokse — **About**, **Contact** og **Products**. Fra **Products** går der en gren videre ned til **Category 1** og **Category 2**. Det er den klassiske to-niveau site-struktur.

### Hierarchical & Shallow

Pas på at organiseringen ikke er for **flad**.

- For mange valg ⇒ et forvirrende og mindre brugbart website.
- **Information Chunking** — forskning af Nelson Cowan: voksne kan typisk holde omkring fire items eller chunks af items i korttidshukommelsen.
- Vær opmærksom på antallet af store navigationslinks.
- Prøv at gruppere navigationslinks visuelt i grupper med højst omkring fire links.

Diagrammet illustrerer problemet: fra **Home** går der grene ud til **tolv** bokse på én række — alt for mange valg på samme niveau. Kun to af de tolv har selv underpunkter (én boks har ét barn, en anden har tre). Strukturen er altså bred og flad, og brugeren skal skanne tolv muligheder ad gangen.

### Hierarchical & Deep

Pas på at organiseringen ikke er for **dyb**.

- Det resulterer i mange "clicks" for at drille ned til den ønskede side.

**User Interface "Three Click Rule"** — en besøgende bør kunne komme fra en hvilken som helst side på dit site til en hvilken som helst anden side med maksimalt tre hyperlinks.

Diagrammet viser den modsatte fejl: fra **Home** går der kun fire grene ud, men strukturen fortsætter i **seks–syv niveauer** nedad i en lang, smal kæde. For at nå de nederste bokse skal man klikke sig igennem alle mellemniveauerne.

### Linear Organization

- En række sider der udgør en tutorial, tour eller præsentation.
- Sekventiel gennemsyn.

Diagrammet viser fem bokse på en vandret linje forbundet af pile der alle peger fremad: **Home Page → Lesson 1 → Lesson 2 → Lesson 3 → Summary**. Der er kun én vej gennem indholdet.

### Random Organization

- Kaldes nogle gange "Web" Organization (eller Wiki).
- Der er som regel ingen klar sti gennem sitet.
- Kan bruges til kunstneriske eller konceptuelle sites.
- Bruges typisk ikke til kommercielle sites.

Diagrammet viser omkring et dusin bokse spredt uden nogen hierarkisk orden, forbundet på kryds og tværs af linjer i alle retninger. Der er ingen top, ingen "Home"-boks der dominerer, og ingen entydig rækkefølge — nettets struktur snarere end et træ.

## 4. Design Principles

| Princip | Betydning |
|---|---|
| **Repetition** | Gentag visuelle elementer gennem hele designet |
| **Contrast** | Tilføj visuel spænding og træk opmærksomhed |
| **Proximity** | Gruppér relaterede elementer |
| **Alignment** | Justér elementer for at skabe visuel enhed |

Slidet illustrerer principperne med et skærmbillede af delstaten Tennessees officielle website (TN.gov): den blå navigationsbjælke i venstre side gentager samme knapform hele vejen ned (repetition), det store farvebillede kontrasterer de blå og hvide flader (contrast), links er samlet i tydelige grupper med overskrifter som "Directories" og "Featured Sites" (proximity), og alle blokke flugter i et klart kolonneraster (alignment).

## 5. Design to Provide for Accessibility

Tim Berners-Lee:

> "The power of the Web is in its universality. Access by everyone regardless of disability is an essential aspect."

Hvem har gavn af øget accessibility?

- En person med et fysisk handicap
- En person med en langsom internetforbindelse
- En person med en gammel, forældet computer

Juridisk krav i USA: **Section 508**. Standard: **WCAG 2.0**.

### WCAG 2.0 og POUR

Web Content Accessibility Guidelines 2.0:

- http://www.w3.org/TR/WCAG20/Overview
- http://www.w3.org/WAI/WCAG20/quickref

Baseret på fire principper (**POUR**):

1. **Perceivable** — indhold skal kunne opfattes.
2. **Operable** — interface-komponenter i indholdet skal kunne betjenes.
3. **Understandable** — indhold og kontroller skal være forståelige.
4. **Robust** — indhold skal være robust nok til at virke med nuværende og fremtidige user agents, inklusive assistive technologies.

## 6. Writing for the Web

- Undgå lange tekstblokke
- Brug bullet points
- Brug headings og subheadings
- Brug korte afsnit

### Design "Easy to Read" Text

- Brug almindelige fonte: Arial, Helvetica, Verdana, Times New Roman
- Brug passende tekststørrelse: `medium`, `1em`, `100%`
- Brug stærk kontrast mellem tekst og baggrund
- Brug kolonner i stedet for brede områder med vandret tekst

### More Text Design Considerations

- Vælg omhyggeligt teksten i hyperlinks:
  - Undgå "click here"
  - Link nøgleord eller fraser, ikke hele sætninger
- "Chek yur spellin" (Check your spelling) — slidet demonstrerer selv pointen med bevidste stavefejl.

## 7. Making Color Choices

Hvordan vælger man et farveskema?

- **Monochromatic** — http://meyerweb.com/eric/tools/color-blend
- **Vælg ud fra et fotografi eller andet billede** — http://www.colr.org
- **Start med en yndlingsfarve** og brug et af disse sites til at vælge de øvrige:
  - http://paletton.com/
  - http://colrd.com/
  - http://www.colorsontheweb.com/Color-Tools/Color-Wizard

### Use of Color — fire målgrupper

Slide 16 viser fire skærmbilleder omkring overskriften "Use of Color", hver med sin målgruppe:

| Målgruppe | Farvebrug på skærmbilledet |
|---|---|
| **Appealing to Kids & Preteens** (H.I.P. Pocket Change) | Kraftig gul baggrund, primærfarver, runde former, tegneseriegrafik og legende typografi |
| **Appealing to Everyone** (National Park Service) | Mørkegrøn/brun paletten, stort naturfotografi, neutral og rolig — plus synlige "Text Size"-kontroller til tilgængelighed |
| **Appealing to Young Adults** (Underated Rock) | Næsten helt sort baggrund med hvid tekst og fotos af guitarer — dramatisk, høj kontrast, minimalt |
| **Appealing to Older Adults** (NIH SeniorHealth) | Lys hvid baggrund, meget stor og enkel typografi, blå/hvid palette, og eksplicitte kontroller øverst til "Text Size", "Contrast: on/off" og "Speech: on/off" |

Pointen er at farvevalg ikke er smag alene — det er en målgruppebeslutning, og for ældre brugere er høj kontrast og læsbarhed vigtigere end visuel effekt.

## 8. Graphic Design Best Practices

**Del 1:**

- Vær forsigtig med store grafikfiler!
- Brug `alt`-attributten til at levere beskrivende alternativ tekst.
- Sørg for at dit budskab kommer igennem, også hvis billeder ikke vises.
  - Hvis du bruger billeder til navigation, så læg almindelige tekstlinks nederst på siden.

**Del 2:**

- Vælg farver fra web-paletten, hvis der er brug for konsistens på tværs af ældre Windows/Mac-platforme.
- Brug **anti-aliased** tekst i billeder.
- Brug kun nødvendige billeder.
- Genbrug billeder.
- Mål: billedets filstørrelse skal være så lille som muligt.

Slidet demonstrerer anti-aliasing visuelt med to ord i stor skrift: ordet "Antialiased" har bløde, jævne kanter hvor bogstavernes kurver glider over i baggrunden, mens ordet "Aliased" har tydeligt takkede, pixelerede trappekanter. Nederst er der en ironisk gul callout ved et lavopløst foto af en hund: **"Do you really need to see a photo of a dog right now?"** — pointen om at fjerne unødvendige billeder.

## 9. Navigation Design

Gør dit site let at navigere:

- Sørg for tydeligt mærket navigation samme sted på hver side.
- Mest almindeligt — hen over toppen eller ned langs venstre side.

Overvej:

- Navigation Bars
- Breadcrumb Navigation
- Brug af grafik til navigation
- Dynamic Navigation
- Site Map
- Site Search Feature
- "Skip to Content"-hyperlink

## 10. Wireframe

En wireframe er en skitse eller blueprint af en webside. Den viser strukturen af de basale sideelementer, inklusive:

- Logo
- Navigation
- Content
- Footer

Illustrationen viser en konkret wireframe: øverst en bred kasse med teksten "Branding / Site Logo", derunder en vandret navigationsbjælke med fem faner (Home, Services, Products, About, Contact). Hovedområdet er delt i tre kolonner — venstre kolonne med "Heading", et lille billedplaceholder og en punktliste; midterkolonnen med et stort billedplaceholder (en simpel bjerg-og-sol-ikon), en "Subheading" og brødtekst; højre kolonne med to "Links"-blokke. Nederst en bred bjælke med "Page footer area". Bemærk at wireframen bruger lorem ipsum og grå placeholder-ikoner — den handler om struktur, ikke indhold eller farve.

## 11. Web Page Design – Page Layout

- Placér den vigtigste information "above the fold".
- Brug tilstrækkeligt "white" eller blank space.
- Brug et interessant page layout.

Slidesene viser en progression gennem tre wireframes af samme side:

**Usable, men lidt kedelig (slide 21):** logo øverst, navigationsbjælke, og derefter alt indhold i **én enkelt fuldbredde-kolonne** — heading, brødtekst, subheading, mere brødtekst, punktliste, og footer nederst. Callout: "This is usable, but a little boring."

**Better (slide 22, øverst):** samme sidehoved, men indholdsområdet er nu delt i **tre lige brede kolonner** med tekst. Callout: "Columns make the page more interesting and it's easier to read this way." Kortere linjelængder er nemmere at læse.

**Best (slide 22, nederst):** indholdet er delt i **kolonner af forskellig bredde** — en smal venstrekolonne med heading, lille billede og punktliste; en bredere midterkolonne med et stort billede og en subheading; en smal højrekolonne med links. Der er billeder spredt ind mellem tekstblokkene. Callout: "Columns of different widths interspersed with graphics and headings create the most interesting, easy to read page."

## 12. Page Layout Design Techniques

| Teknik | Beskrivelse |
|---|---|
| **Ice Design** | Også kendt som rigid eller fixed design. Fast bredde, som regel op mod venstre margen |
| **Jello Design** | Sideindholdet typisk centreret. Ofte konfigureret med en fast eller procentvis bredde, fx 80% |
| **Liquid Design** | Siden udvides til at fylde browseren ved alle opløsninger |

## 13. Web Page Design – Load Time

- Hold øje med load time på dine sider.
- Prøv at begrænse websidedokument og tilhørende medier til under 100K på forsiden.

## 14. Web Page Design – Screen Resolution

- Test ved forskellige skærmopløsninger.
  - Mest udbredte: 1024x768, 1280x800, 1366x768 og 1920x1080.
- Design så det ser godt ud ved forskellige skærmopløsninger:
  - Centreret sideindhold, sat til enten fast eller procentvis bredde.
  - Eller brug Liquid Design.

Skærmbilledet viser City of Fresno's website vist i et bredt browservindue: sideindholdet er centreret i en kasse i midten med tydelige blå margener i begge sider — jello design i praksis. Sidens indhold flyder altså ikke ud i fuld skærmbredde, men bevarer en læsbar linjelængde.

## 15. Web Page Design – Browser Compatibility

- Websider ser **IKKE** ens ud i alle de store browsere.
- Test med nuværende og nylige versioner af:
  - Chrome
  - Firefox
  - Safari
  - Edge
- Design så det ser OK ud i almindeligt brugte browsere, og implementér nye teknologier i moderne browsere ⇒ **Progressive Enhancement**.

## 16. Web Design Best Practices Checklist

- Page Layout
- Browser Compatibility
- Navigation
- Color and Graphics
- Multimedia
- Content Presentation
- Functionality
- Accessibility

## 17. Web Fonts

- Bestemte fonte fungerer bedst i overskrifter, mens andre læses godt i afsnit.
- Nogle font-familier er store nok til at inkludere internationale skrifter og specialtegn.
- Og hvis fonten findes i en række forskellige styles (som kursiv eller small caps) og vægte (fra hairline til ultra-black), giver den flere værktøjer til at finjustere designet, efterhånden som projektet tager form.

Læs mere: Choosing Web Fonts: A Beginner's Guide — https://design.google/library/choosing-web-fonts-beginners-guide/

## 18. Tools

- **Pencil** — et open source GUI prototyping-værktøj tilgængeligt for ALLE platforme: http://pencil.evolus.vn/Downloads.html
- **Balsamiq** — trial er fuldt funktionel i 30 dage; derefter skal der købes licens for at gemme sit arbejde: https://balsamiq.com/
- **Sketch** — et digitalt design-toolkit bygget til at hjælpe dig med at skabe dit bedste arbejde, fra tidlige idéer til endelige assets: https://www.sketchapp.com/

<!-- slidet staver "Balsamic"; produktet hedder Balsamiq -->

## 19. Usability

> "A user interface is like a joke. If you have to explain it, it's not that good."

### Don't Make Me Think

**A Common Sense Approach to Web Usability, 2nd Edition**, af Steve Krug.

Opsummering: http://www.uxbooth.com/articles/10-usability-lessons-from-steve-krugs-dont-make-me-think/

De følgende slides gennemgår bogens hovedpointer:

**Usability Means…**
Usability betyder at sikre at noget fungerer godt, og at en person med gennemsnitlig evne eller erfaring kan bruge det til dets tiltænkte formål uden at blive håbløst frustreret.

**Web applications should explain themselves**
Så vidt det overhovedet er menneskeligt muligt bør en webside være selvindlysende, når jeg ser på den. Obvious. Self-explanatory.

**Don't Make Me Think**
Som regel kan folk ikke lide at gruble over hvordan man gør ting. Hvis de der bygger et site ikke bekymrer sig nok om at gøre tingene indlysende, kan det underminere tilliden til sitet og dets udgivere.

**Don't waste my time**
Meget af vores webbrug er motiveret af ønsket om at spare tid. Derfor har webbrugere tendens til at opføre sig som hajer — de skal blive ved med at bevæge sig, ellers dør de.

**Users still cling to their back buttons**
Der er ikke den store straf for at gætte forkert. I modsætning til brandslukning er straffen for at gætte forkert på et website bare et klik eller to på back-knappen. Back-knappen er den mest brugte feature i webbrowsere.

**We're creatures of habit**
Hvis vi finder noget der virker, holder vi fast i det. Når vi først har fundet noget der virker — uanset hvor dårligt — har vi tendens til ikke at lede efter en bedre måde. Vi bruger en bedre måde hvis vi snubler over den, men vi leder sjældent efter den.

**No Time for Small Talk**
"Happy talk" er som small talk — indholdsløst, dybest set bare en måde at være selskabelig på. Men de fleste webbrugere har ikke tid til small talk; de vil direkte til sagen. Du kan — og bør — eliminere så meget happy talk som muligt.

**Don't lose search**
Nogle mennesker (search-dominant users) vil næsten altid lede efter en søgeboks når de kommer ind på et site. Det er måske de samme mennesker der leder efter den nærmeste ekspedient, så snart de træder ind i en butik.

**We form mental site-maps**
Når vi vender tilbage til noget på et website, må vi — i stedet for at støtte os til en fysisk fornemmelse af hvor det er — huske hvor det er i det begrebsmæssige hierarki og gå vores skridt tilbage.

**Make it easy to go home**
At have en home-knap synlig hele tiden giver den betryggelse at uanset hvor faret vild jeg måtte blive, kan jeg altid starte forfra — som at trykke på en Reset-knap eller bruge et "Get out of Jail free"-kort.
