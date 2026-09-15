# L01.1 – Introduktion til kurset SW4FED

## Metadata

- **Lektion:** L01 – Introduktion til kurset
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L01/FED Introduktion til kurset.pdf (19 slides)
- **Emner dækket:**
  - Præsentation af undervisere
  - Formål med kurset og baggrund for oprettelsen
  - Hvorfor C# er valgt som sprog
  - Sprogpopularitet (TIOBE, Stack Overflow, GitHub Octoverse, jobtrends)
  - Kursusbeskrivelse, ECTS og eksamensform
  - Obligatoriske opgaver
  - Læringsmål
  - Vejledende lektionsplan
  - Undervisningsform
  - Lærebøger til MAUI, HTML/CSS/JS og React

---

## 1. Hvad kurset handler om

SW4FED er front-end development: programmering af Graphical User Interfaces. Kurset dækker to spor. Først app-udvikling med C#, .NET og MAUI. Derefter web-sites og web-apps med HTML, CSS, JavaScript og React.

Agendaen for første lektion er præsentation af undervisere og kursus, dernæst C# og MAUI.

## 2. Undervisere

**"Jenny" Jung Min Kim**
Kontor: Bygning 5123 – rum 424. E-mail: jmk@ece.au.dk.
Uddannelse: Civilingeniør fra Aarhus University, B.Sc.EE in Electronic Engineering, kandidat M.Sc.EE in Computer Engineering.
Erfaring: 20 års industriel erfaring som softwareingeniør i internationale firmaer — Software Engineer i Korea (1998), Danmark (1999–2003), Senior Research Engineer / Project Lead i Korea (2003–2007), Software Engineer i Danmark (2008–2014 og 2015–2019).

**Poul Ejnar Rovsing**
Kontor: Bygning 5123 – rum 412. E-mail: per@ece.au.dk.
Uddannelse: Dataingeniør fra Aarhus Teknikum (nuværende ECE).
Erfaring: Har undervist i Windows-programmering med C# og .NET siden Visual Studio .NET blev lanceret, og har arbejdet med hjemmesider siden WWW's barndom.

## 3. Hvorfor er SW4FED oprettet?

Grafiske brugergrænseflader og webudvikling er udbredt anvendt i industrien.

Det overordnede mål er todelt. Dels at få erfaring med et konkret framework til udvikling af installerbare applikationer med grafiske brugergrænseflader til computere, tablets og mobiltelefoner. Dels at få erfaring med et konkret framework til udvikling af klient-applikationer med grafiske brugergrænseflader til web-browsere.

De fornødne forkundskaber er til stede: godt kendskab til objektorienteret programmering i C++, C# eller Java.

## 4. Hvorfor C#?

C# kan anvendes både på front-end og back-end. Bemærk dog forbeholdet: browsere understøtter kun JavaScript.

C# er let at lære for både C++- og Java-programmører, og sproget har taget noget af det bedste fra C++, Visual Basic, Java og Delphi — plus omkring 25 andre sprog.

## 5. Hvilke sprog anvendes?

Slidesene viser tre forskellige opgørelser af sprogpopularitet.

TIOBE Programming Community Index er en indikator for programmeringssprogs popularitet. Placeringen januar 2021: 1) C, 2) Python, 3) Java, 4) C++, 5) C#, 6) Visual Basic.NET, 7) JavaScript, 8) PHP, 9) R, 10) Groovy.

Stack Overflow Survey 2020 opgør Programming, Scripting and Markup Languages. <!-- uklart i kilden: selve tallene fra diagrammet er ikke udtrukket -->

Jobtrends viser hyppighed af termer i danske it-jobannoncer pr. 25-07-2019: sikkerhed 22,9 %, drift 20,1 %, web 19,2 %, cloud 18,6 %, javascript 16,0 %, sql 14,6 %, .net 14,4 %, scrum 13,5 %, java 10,9 %, c# 10,4 %.

GitHub Octoverse rangerer sprogene efter antallet af unikke bidragsydere til offentlige eller private repoer på GitHub. Kilde: https://octoverse.github.com/

IEEE Spectrum Top Programming Languages 2021: https://spectrum.ieee.org/top-programming-languages-2021

## 6. Kursusbeskrivelse

SW4FED Front-end development ligger på 4. semester for SW-studerende og giver 5 ECTS-point.

Evaluering: mundtlig eksamen. Der er en 24-timers take home-eksamen, hvor den studerende individuelt skal løse en konkret problemstilling. Løsningen demonstreres og fremlægges på 15 (20) minutter. Karakter efter 7-trinsskalaen med ekstern censur. Brug af GAI som fx GitHub Copilot er tilladt.

## 7. Obligatoriske opgaver

Der er 2 obligatoriske opgaver, som skal godkendes for at blive indstillet til eksamen. De obligatoriske opgaver skal laves i grupper af 2–4 studerende.

## 8. Læringsmål

Efter kurset skal den studerende kunne:

- Redegøre for principperne i .NET-frameworket og dets overordnede arkitektur samt beskrive og anvende programmeringssproget C#.
- Anvende kontroller og layout panels til opbygning af grafiske brugergrænseflader.
- Anvende navigering mellem pages i en applikation.
- Anvende dependency injection og det generiske "host builder pattern" til at registrere services og resourcer.
- Kunne anvende MVVM-arkitekturen til at implementere applikationer med en grafisk brugergrænseflade ved brug af programmeringssprogene C# og XAML.
- Tilgå data i en database på enheden og tilgå remote data via et web API.
- Redegøre for arkitekturen for en webapplikation.
- Designe og implementere webapplikationer med en grafisk brugergrænseflade med brug af HTML5, CSS og JavaScript eller TypeScript.
- Anvende et client side-bibliotek til udvikling af web-applikationer.
- Anvende komponenter til opbygning af en webapplikation.
- Anvende client-side routing i en webapplikation.

## 9. Vejledende lektionsplan

| Uge | Emne |
| --- | --- |
| 1 | Introduction to C# and MAUI / Making apps interactive and events in C# |
| 2 | Controls and .NET architecture / Layouts |
| 3 | Advanced layout concepts / Pages and navigation |
| 4 | Dependency Injection and Generic Host builder pattern / The MVVM pattern |
| 5 | The MVVM pattern / Styles, themes, and multi-platform layouts |
| 6 | Custom controls / Assignment 1 |
| 7 | HTML5 and CSS3 / Forms and tables in HTML |
| 8 | Basic JavaScript and the DOM / React basics, JSX |
| 9 | useState Hook and routing / useEffect and lifecycle |
| 10 | Fetch – calling a WebAPI / TypeScript |
| 11 | Handling events in React, useRef + custom hooks / Forms |
| 12 | Testing React components / Managing application state |
| 13 | Styling React / Responsive design |
| 14 | Redux intro / Evaluering + om eksamen |

## 10. Undervisningsform

Kursustiden er fordelt med ca. 50 % til forelæsninger og ca. 50 % til praktisk programmering.

Undervisningen er fordelt på 2 dage om ugen à 2 × 45 min, og består af forelæsning, spørgsmål, diskussioner, eksempler og programmering på egen laptop.

## 11. Lærebøger

**MAUI:** *.NET MAUI in Action* af Matt Goldman, ISBN 9781633439405, udgivet 2023 af Manning Publications Inc. https://www.manning.com/books/dot-net-maui-in-action

**HTML, CSS og JavaScript:**
*The HTML handbook* af Flavio Copes — https://flaviocopes.com/page/ebooks-links/
*The CSS handbook* af Flavio Copes — https://flaviocopes.com/page/ebooks-links/
*The Modern JavaScript Tutorial* — https://javascript.info/ (opdateres løbende)

**React:** *React Quickly, Second Edition* af Morten Barklund og Azat Mardan, ISBN 9781633439290, udgivet 2023 af Manning. https://www.manning.com/books/react-quickly-second-edition

Slidesene slutter med et citat af Napoleon Hill: "It takes half your life before you discover life is a do-it-yourself project."
