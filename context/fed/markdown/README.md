# SW4FED-02 Front-end udvikling — AI-kontekstnoter

Indeks over konverteret kursusmateriale til brug som AI-kontekst.

## 1. Kursusfakta

| | |
| --- | --- |
| **Kursus** | SW4FED-02 Front-end udvikling |
| **ECTS** | 5 |
| **Semester** | 4. semester, efterårssemester 2026 |
| **Uddannelse** | Diplomingeniøruddannelsen i softwareteknologi, Aarhus Universitet |
| **Institut** | Institut for Elektro- og Computerteknologi |
| **Undervisere** | Poul Ejnar Rovsing (kursusansvarlig), Jung Min Kim ("Jenny") |
| **Undervisningssprog** | Dansk, delvist engelsk |
| **Omfang** | 56 timer over 14 uger |

## 2. Kursusindhold

Introduktion til .NET, C#, XAML og MAUI. Lokal database fra MAUI apps. Controls and Layouts. Pages and navigation. Generic host builder pattern og dependency injection. MVVM-arkitekturen. HTML5 og CSS3. JavaScript og TypeScript. React til Single Page Applications. Side effects og datahentning fra Web API. Application state på klienten. Test af React-komponenter.

## 3. Læringsmål

Efter kurset skal den studerende kunne:

1. Redegøre for principperne i .NET frameworket og dets overordnede arkitektur samt beskrive og anvende programmeringssproget C#.
2. Anvende kontroller og layout panels til opbygning af grafiske brugergrænseflader.
3. Anvende navigering mellem pages i en applikation.
4. Anvende dependency injection og det generiske "host builder pattern" til at registrere services og resourcer.
5. Kunne anvende MVVM-arkitekturen til at implementere applikationer med en grafisk brugergrænseflade ved brug af C# og XAML.
6. Tilgå data i en database på enheden og tilgå remote data via et web api.
7. Redegøre for arkitekturen for en webapplikation.
8. Designe og implementere webapplikationer med grafisk brugergrænseflade med HTML5, CSS og JavaScript eller TypeScript.
9. Anvende et klientside-bibliotek til udvikling af webapplikationer.
10. Anvende komponenter til opbygning af en webapplikation.
11. Anvende client-side routing i en webapplikation.

## 4. Eksamen

24-timers individuel hjemmeopgave (alle hjælpemidler tilladt, inklusive generativ AI og GitHub Copilot) efterfulgt af 20 minutters mundtlig eksamination med ekstern censur, 7-trinsskala. Karakteren vægter primært den mundtlige del. Forudsætning for eksamen: godkendelse af to obligatoriske opgaver.

## 5. Mappestruktur

| Mappe | Indhold |
| --- | --- |
| `slides/` | Forelæsningsslides, navngivet efter lektionsnummer (L01–L26) |
| `labs/` | Opgavetekster til øvelserne |
| `book/` | Lærebogskapitler fra *.NET MAUI in Action* og *React Quickly* |
| `noter/` | Brightspace-noter og praktiske vejledninger; billeder i `noter/assets/` |
| `kode/` | Kildekodefiler, der hører til labs og eksamensbilag |
| `eksamen/` | Gamle eksamenssæt samt eksamensforberedelse |

Derudover: [Vejledende lektionsplan](README_lektionsplan.md) — uge-for-uge plan og litteraturliste.

## 6. Indeks

### Slides

**Lektion 1 — Introduktion, XML, XAML, C#, MAUI**
- [Introduktion til kurset](slides/SW4FED-02_L01.1_Introduktion_til_kurset.md)
- [XML Essentials](slides/SW4FED-02_L01.2_XML_Essentials.md)
- [XAML](slides/SW4FED-02_L01.3_XAML.md)
- [C# Introduction](slides/SW4FED-02_L01.4_Csharp_Introduction.md)
- [Copilot i kurset](slides/SW4FED-02_L01.5_Copilot_i_kurset.md)
- [MAUI introduction](slides/SW4FED-02_L01.6_MAUI_introduction.md)

**Lektion 2 — Interaktivitet og data binding**
- [MAUI: Making App Interactive](slides/SW4FED-02_L02.1_MAUI_Making_App_Interactive.md)
- [Data Binding Basics](slides/SW4FED-02_L02.2_Data_Binding_Basics.md)
- [Events i C#](slides/SW4FED-02_L02.3_Events_i_Csharp.md)

**Lektion 3 — Controls og typesystem**
- [Controls](slides/SW4FED-02_L03.1_Controls.md)
- [.NET typesystem](slides/SW4FED-02_L03.2_NET_typesystem.md)

**Lektion 4 — .NET-arkitektur og layouts**
- [.NET Architecture](slides/SW4FED-02_L04.1_NET_Architecture.md)
- [Layouts](slides/SW4FED-02_L04.2_Layouts.md)

**Lektion 5 — Advanced layout, async og HTTP**
- [Advanced layout concepts](slides/SW4FED-02_L05.1_Advanced_layout_concepts.md)
- [async/await](slides/SW4FED-02_L05.2_async_await.md)
- [HttpClient](slides/SW4FED-02_L05.3_HttpClient.md)
- [Garbage Collection](slides/SW4FED-02_L05.4_Garbage_Collection.md)

**Lektion 6 — Pages og navigation**
- [Pages and Navigation](slides/SW4FED-02_L06.1_Pages_and_Navigation.md)
- [Shell and Routes](slides/SW4FED-02_L06.2_Shell_and_Routes.md)

**Lektion 7 — NavigationPage, DI og authentication**
- [NavigationPage](slides/SW4FED-02_L07.1_NavigationPage.md)
- [Enterprise App development](slides/SW4FED-02_L07.2_Enterprise_App_development.md)
- [Dependency Injection og Generic Host builder](slides/SW4FED-02_L07.3_Dependency_Injection_og_Generic_Host_builder.md)
- [Authentication i MAUI](slides/SW4FED-02_L07.4_Authentication_i_MAUI.md)

**Lektion 8 — MVVM**
- [The MVVM Pattern](slides/SW4FED-02_L08.1_The_MVVM_Pattern.md)

**Lektion 9 — MVVM Community Toolkit og Preferences**
- [MVVM Community Toolkit](slides/SW4FED-02_L09.1_MVVM_Community_Toolkit.md)
- [Preferences](slides/SW4FED-02_L09.2_Preferences.md)

**Lektion 10 — Styles og themes**
- [Styles and themes](slides/SW4FED-02_L10.1_Styles_and_themes.md)

**Lektion 11 — Custom controls**
- [Custom controls](slides/SW4FED-02_L11.1_Custom_controls.md)

**Lektion 13 — Web, HTML5 og CSS3**
- [Websites og Web apps](slides/SW4FED-02_L13.1_Websites_og_Web_apps.md)
- [Config Server for static files](slides/SW4FED-02_L13.2_Config_Server_for_static_files.md)
- [Serve static files i VS Code](slides/SW4FED-02_L13.2b_Serve_static_files_VS_Code.md)
- [HTML5 basics](slides/SW4FED-02_L13.3_HTML5_basics.md)
- [HTML5 structural elements](slides/SW4FED-02_L13.4_HTML5_structural_elements.md)
- [CSS3 basics](slides/SW4FED-02_L13.5_CSS3_basics.md)
- [Graphics i HTML og CSS](slides/SW4FED-02_L13.6_Graphics_i_HTML_og_CSS.md)
- [Page Layout](slides/SW4FED-02_L13.7_Page_Layout.md)
- [Flexbox](slides/SW4FED-02_L13.8_Flexbox.md)
- [CSS Grid](slides/SW4FED-02_L13.9_CSS_Grid.md)

**Lektion 14 — Web design, tabeller, forms og Bootstrap**
- [Web Design](slides/SW4FED-02_L14.1_Web_Design.md)
- [Tables i HTML](slides/SW4FED-02_L14.2_Tables_i_HTML.md)
- [Forms i HTML](slides/SW4FED-02_L14.3_Forms_i_HTML.md)
- [Bootstrap](slides/SW4FED-02_L14.4_Bootstrap.md)

**Lektion 15 — JavaScript og DOM**
- [JavaScript](slides/SW4FED-02_L15.1_JavaScript.md)
- [Functions i JS](slides/SW4FED-02_L15.2_Functions_i_JS.md)
- [Objects og Arrays i JS](slides/SW4FED-02_L15.3_Objects_og_Arrays_i_JS.md)
- [The DOM](slides/SW4FED-02_L15.4_The_DOM.md)
- [npm](slides/SW4FED-02_L15.5_npm.md)

**Lektion 16 — React-introduktion**
- [React Overview](slides/SW4FED-02_L16.1_React_Overview.md)

**Lektion 17 — Komponenter, state og routing**
- [Functional components](slides/SW4FED-02_L17.1_Functional_components.md)
- [React useState Hook](slides/SW4FED-02_L17.2_React_useState_Hook.md)
- [React Router](slides/SW4FED-02_L17.3_React_Router.md)

**Lektion 18 — Side effects, JSON og klientlagring**
- [React side effects](slides/SW4FED-02_L18.1_React_side_effects.md)
- [JSON](slides/SW4FED-02_L18.2_JSON.md)
- [WebStorage og Cookies](slides/SW4FED-02_L18.3_WebStorage_og_Cookies.md)

**Lektion 19 — Promises og datahentning**
- [Promises](slides/SW4FED-02_L19.1_Promises.md)
- [Fetch](slides/SW4FED-02_L19.2_Fetch.md)
- [React fetching data](slides/SW4FED-02_L19.3_React_fetching_data.md)

**Lektion 20 — Modules og useRef**
- [Modules](slides/SW4FED-02_L20.1_Modules.md)
- [React useRef Hook](slides/SW4FED-02_L20.2_React_useRef_Hook.md)

**Lektion 21 — Events i React**
- [Handling Events i React](slides/SW4FED-02_L21.1_Handling_Events_i_React.md)

**Lektion 22 — React Forms**
- [React Forms](slides/SW4FED-02_L22.1_React_Forms.md)
- [React Forms med kommentarer](slides/SW4FED-02_L22.2_React_Forms_med_kommentarer.md)

**Lektion 23 — Styling og test**
- [React Styling](slides/SW4FED-02_L23.1_React_Styling.md)
- [Testing med Vitest](slides/SW4FED-02_L23.2_Testing_med_Vitest.md)

**Lektion 24 — Application state**
- [React: managing application state](slides/SW4FED-02_L24.1_React_managing_application_state.md)
- [Kørsel af demoer](slides/SW4FED-02_L24.2_Koersel_af_demoer.md)

**Lektion 25 — TypeScript**
- [TypeScript](slides/SW4FED-02_L25.1_TypeScript.md)

**Lektion 26 — Redux og responsivt design**
- [React Redux](slides/SW4FED-02_L26.1_React_Redux.md)
- [Responsive Web Design](slides/SW4FED-02_L26.2_Responsive_Web_Design.md)

### Labs

- [Lab 01 — Hello MAUI](labs/SW4FED-02_Lab01_Hello_MAUI.md)
- [Lab 02 — MauiTodo](labs/SW4FED-02_Lab02_MauiTodo.md)
- [Lab 03 — SelectImages](labs/SW4FED-02_Lab03_SelectImages.md)
- [Lab 04 — MauiCalc](labs/SW4FED-02_Lab04_MauiCalc.md)
- [Lab 05 — SelectImages v2](labs/SW4FED-02_Lab05_SelectImages_v2.md)
- [Lab 06 — MauiShell app](labs/SW4FED-02_Lab06_MauiShell_app.md)
- [Lab 07 — El-spotpriser](labs/SW4FED-02_Lab07_El_spot_priser.md)
- [Lab 08 — ToDoApp v2](labs/SW4FED-02_Lab08_ToDoApp_v2.md)
- [Lab 09 — Hacker News browser](labs/SW4FED-02_Lab09_Hacker_News_browser.md)
- [Lab 10 — Implicit styles](labs/SW4FED-02_Lab10_Implicit_styles.md)
- [Lab 11 — Pagination control](labs/SW4FED-02_Lab11_Pagination_control.md)
- [Lab 14 — JavaJam 03](labs/SW4FED-02_Lab14_JavaJam_03.md)
- [Lab 15 — JavaScript](labs/SW4FED-02_Lab15_JavaScript.md)
- [Lab 16 — Introduction to React](labs/SW4FED-02_Lab16_Introduction_to_React.md)
- [Lab 17 — React Router: client-side routing](labs/SW4FED-02_Lab17_React_Router_client_side_routing.md)
- [Lab 18 — useEffect](labs/SW4FED-02_Lab18_useEffect.md)
- [Lab 19 — Fetching data fra React](labs/SW4FED-02_Lab19_Fetching_data_fra_React.md)
- [Lab 20 — useRef Counter App](labs/SW4FED-02_Lab20_useRef_Counter_App.md)
- [Lab 21 — Simple draw canvas](labs/SW4FED-02_Lab21_Simple_draw_canvas.md)
- [Lab 22 — Form og POST data](labs/SW4FED-02_Lab22_Form_og_Post_data.md)
- [Lab 23 — Responsivt design](labs/SW4FED-02_Lab23_Responsivt_design.md)
- [Lab 24 — Teachers context](labs/SW4FED-02_Lab24_Teachers_context.md)
- [Lab 27 — Vitest](labs/SW4FED-02_Lab27_Vitest.md)

### Bog: .NET MAUI in Action

- [Kapitel 1 — Introducing .NET MAUI](book/MAUI_in_Action_Ch01_Introducing_NET_MAUI.md)
- [Kapitel 2 — Building a .NET MAUI app](book/MAUI_in_Action_Ch02_Building_a_NET_MAUI_app.md)
- [Kapitel 3 — Making apps interactive](book/MAUI_in_Action_Ch03_Making_apps_interactive.md)
- [Kapitel 4 — Controls](book/MAUI_in_Action_Ch04_Controls.md)
- [Kapitel 5 — Layouts](book/MAUI_in_Action_Ch05_Layouts.md)
- [Kapitel 6 — Advanced layout concepts](book/MAUI_in_Action_Ch06_Advanced_layout_concepts.md)
- [Kapitel 7 — Pages and navigation](book/MAUI_in_Action_Ch07_Pages_and_navigation.md)
- [Kapitel 8 — Enterprise app development](book/MAUI_in_Action_Ch08_Enterprise_app_development.md)
- [Kapitel 9 — The MVVM Pattern](book/MAUI_in_Action_Ch09_The_MVVM_Pattern.md)
- [Kapitel 10 — Styles, themes and multiplatform](book/MAUI_in_Action_Ch10_Styles_themes_multiplatform.md)
- [Kapitel 11 — Custom controls](book/MAUI_in_Action_Ch11_Custom_controls.md)

### Bog: React Quickly (1. udgave — se forbehold)

- [Kapitel 1 — Meeting React](book/React_Quickly_1ed_Ch01_Meeting_React.md)
- [Kapitel 3 — Introduction to JSX](book/React_Quickly_1ed_Ch03_Introduction_to_JSX.md)
- [Kapitel 6 — Handling events in React](book/React_Quickly_1ed_Ch06_Handling_events_in_React.md)
- [Kapitel 13 — React routing](book/React_Quickly_1ed_Ch13_React_routing.md)

### Noter

- [Course Contacts](noter/SW4FED-02_Note_Course_Contacts.md)
- [Double path problem](noter/SW4FED-02_Note_Double_path_problem.md)
- [.gitignore til .NET-projekter](noter/SW4FED-02_Note_gitignore_til_NET_projekter.md)
- [MAUI-program kører ikke](noter/SW4FED-02_Note_MAUI_program_koerer_ikke.md)
- [Om undervisningen](noter/SW4FED-02_Note_Om_undervisningen.md)
- [Preferences i MAUI](noter/SW4FED-02_Note_Preferences_i_MAUI.md)
- [SQLite i MAUI apps](noter/SW4FED-02_Note_SQLite_i_MAUI_apps.md)
- [VS Code setup til MAUI](noter/SW4FED-02_Note_VS_Code_setup_til_MAUI.md)

### Kode

- [Lab 02 — MauiTodo, kildekode](kode/SW4FED-02_Lab02_MauiTodo_kode.md)
- [Lab 09 — HackerNewsResponse](kode/SW4FED-02_Lab09_HackerNewsResponse.md)
- [Eksamen sommer 2024 — bilag 1](kode/SW4FED-02_Eksamen_2024_Sommer_bilag1.md)

### Eksamen

- [Eksamen sommer 2024](eksamen/SW4FED-02_Eksamen_2024_Sommer.md)
- [Eksamen vinter 2024](eksamen/SW4FED-02_Eksamen_2024_Vinter.md)
- [Eksamen sommer 2025](eksamen/SW4FED-02_Eksamen_2025_Sommer.md)
- [Eksamen V25-26, ordinær](eksamen/SW4FED-02_Eksamen_V25-26_ordinaer.md)
- [Eksamensforberedelse](eksamen/SW4FED-02_Eksamensforberedelse.md)

## 7. Vigtige forbehold

### Lærebøgerne

- **.NET MAUI in Action** (Matt Goldman, Manning): kapitel 1–11 er konverteret. Kapitel 12 (*Deploying apps to production with GitHub Actions*) er udeladt, da deployment ikke er i kursets emneliste.
- **React Quickly**: ⚠️ **den tilgængelige PDF er 1. udgave (©2017, Azat Mardan), men kursusbeskrivelsen foreskriver 2. udgave (Barklund & Mardan).** 1. udgave er skrevet før React Hooks og bruger class components, lifecycle-metoder og Webpack. Kursets React-lektioner er hook-baserede og bruger Vite, Vitest og Redux Toolkit. Derfor er kun fire kapitler konverteret (1 Meeting React, 3 Introduction to JSX, 6 Handling events, 13 React routing), hvor 1. udgave stadig er fagligt gyldig. **Ved konflikt mellem bogen og slidesene er slidesene autoritative.**

### Lektionsnumre

Den vejledende lektionsplan og slide-mappernes lektionsnumre stemmer ikke overens. Planen placerer Styling i lektion 25, Responsive design i 26 og Redux Toolkit i 27; slidesene har Styling og Vitest under L23, Managing application state under L24, og Redux samt Responsive Web Design under L26. **Gå efter slide-mappernes numre når du leder efter materiale.** Se [README_lektionsplan.md](README_lektionsplan.md).

### Kildetro gengivelse

Materialet er konverteret fra PDF med tekstudtrækning, suppleret med visuel læsning af sidebilleder for de mest diagram- og screenshottunge slides. Hvor kilden indeholder trykfejl, ukompilérbar kode eller afkortede kodeeksempler, er dette **bevaret som i kilden** og markeret med en HTML-kommentar eller `<!-- uklart i kilden -->`. Der er ikke opdigtet kode for at "lukke" ufuldstændige eksempler.

## 8. Kilder

Materialet er genereret fra 29 Brightspace-eksportzips (87 PDF'er, 1543 slides) samt de to lærebøger. Originalfilerne ligger uændret i mappen over denne (`../`).
