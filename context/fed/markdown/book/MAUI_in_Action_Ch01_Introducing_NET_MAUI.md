# Kapitel 1 – Introducing .NET MAUI

## Metadata

- **Kapitel:** 1 – Introducing .NET MAUI
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L01
- **Hovedemner:**
  - Historikken bag cross-platform udvikling: Java/JVM, .NET Framework, Mono, Xamarin, Xamarin.Forms
  - Hvad .NET MAUI er: én kodebase, fire platforme (Windows, macOS, iOS, Android)
  - .NET MAUI-arkitekturen lag for lag (OS → runtime → BCL → platform-bindings → MAUI)
  - MAUI som .NET-workload, ikke et separat SDK
  - Cross-platform vs. "native" apps — MAUI-apps *er* native binaries
  - Binær app vs. web app: multithreading, encryption, hardware- og platform-API-adgang
  - Abstraktion af native UI: "consistent but not identical" på tværs af platforme
  - .NET MAUI i .NET-økosystemet: delt kode på tværs af full-stack
  - Udviklingsparadigmer: XAML + MVVM, C#-UI, MVU, .NET MAUI Blazor

---

## 1.1 How did we get here?

Drømmen om *write once, run anywhere* (WORA) begyndte for alvor i 1996 med Sun Microsystems' første version af Java. Før Java kunne udviklere kun skrive kode mod de API'er, operativsystemet stillede til rådighed. Java var ikke bare et nyt sprog — det var også en runtime med sit eget sæt API'er, så udvikleren kunne ignorere målplatformen. Sun leverede en runtime (Java Virtual Machine, JVM) til stort set alle operativsystemer, og udvikleren byggede dermed ikke en Windows-, Unix-, Linux- eller Mac-applikation, men en *Java*-applikation.

Microsoft startede sin egen rejse kort efter med den første offentlige version af .NET Framework i 2000. Det var ikke cross-platform, men paradigmet lignede: udvikleren skrev ikke længere kode mod Windows-API'er, men brugte .NET Base Class Library (BCL) til at skrive kode mod .NET-API'erne. Ligesom JVM'en var .NET Framework en runtime installeret uafhængigt af operativsystemet.

**Figure 1.1** illustrerer netop dette skift: legacy-applikationer bygger direkte oven på operativsystemets API'er, mens Java og .NET indskyder deres egne API'er og en runtime, der skjuler platform-API'erne.

Problemet var, at .NET Framework kun kørte på Windows. Det ændrede sig i 2016 med .NET Core, som strippede de centrale Windows-afhængigheder ud og blev en reelt portabel runtime, der kan sendes med koden og køre på Windows, Mac og Linux.

Men UI-applikationer manglede stadig i billedet. .NET Core-applikationer er command-line-only (inklusive webservere og andre services). .NET Framework leverede UI-platforme til Windows — først Windows Forms, senere Windows Presentation Foundation (WPF) — og selvom disse blev portet til .NET Core og senere versioner, er de stadig Windows-only.

Uden for Microsoft tog rejsen mod cross-platform UI i .NET sit eget liv. Inden for et år efter .NET Frameworks første version blev .NET-specifikationen en åben standard. Åbne standarder driver det moderne web og muliggør konkurrerende eller komplementære runtimes — fordi HTML, JavaScript og CSS er åbne standarder, kan enhver bygge en browser. Åbningen af .NET-standarden gjorde det muligt for Miguel de Icaza (dengang hos Novell) at udgive **Mono**, en open source .NET-compiler til Linux.

Ved udgangen af 2000'erne var iOS og Android — og især deres distributionsplatforme, App Store og Google Play — veletablerede, og enhver diskussion om cross-platform UI blev domineret af mobil. iOS og Android bruger forskellige sprog og paradigmer, og de fleste udviklere foretrækker ikke at skrive og vedligeholde flere versioner af deres software.

Mono blev portet til iOS som MonoTouch og til Android som MonoDroid. Disse udviklede sig til **Xamarin**, som leverede både en .NET-compiler til iOS og Android og en komplet abstraktion af iOS- og Android-API'erne i .NET. Med Xamarin skulle man stadig lære iOS- og Android-API'erne, men koden blev skrevet i C# i stedet for Objective-C, Swift eller Kotlin. Ikke-UI-kode kunne deles mellem de to platforme.

I 2014 introducerede Xamarin **Xamarin.Forms**, som gav et API til at skrive cross-platform UI-kode i XAML (eXtensible Application Markup Language) — markup-sproget oprindeligt introduceret i WPF. Det gjorde det muligt at dele både business logic og UI på tværs af iOS, Android og Universal Windows Platform (UWP). Som de øvrige abstraktioner er XAML-UI-koden netop en abstraktion: når appen kompileres til iOS, oversættes XAML til iOS' native UI, og når der kompileres til Android, kompileres XAML til native Android UI-kode.

Xamarin blev opkøbt af Microsoft i 2016. Xamarin.Forms 5 er den sidste version. I stedet kommer .NET MAUI, som Microsoft beskriver som den næste evolution af Xamarin.Forms. MAUI deler meget DNA med Xamarin, men er en helt ny platform bygget fra bunden.

### Tidslinje (opsummering af afsnittet)

| Årstal | Begivenhed | Betydning for WORA |
| --- | --- | --- |
| 1996 | Sun udgiver første version af **Java** | Første seriøse WORA-forsøg: eget sprog *og* runtime (JVM) med egne API'er på næsten alle OS'er |
| 2000 | Første offentlige version af **.NET Framework** | Samme paradigme som Java (BCL i stedet for Windows-API'er), men Windows-only |
| ~2001 | .NET-specifikationen bliver en **åben standard** | Muliggør konkurrerende/komplementære runtimes |
| — | Miguel de Icaza (Novell) udgiver **Mono** | Open source .NET-compiler til Linux |
| Slut-2000'erne | iOS og Android + App Store/Google Play etableres | Cross-platform-diskussionen domineres af mobil |
| — | **MonoTouch** (iOS) og **MonoDroid** (Android) → **Xamarin** | .NET-compiler *og* komplet .NET-abstraktion af iOS-/Android-API'erne; delt ikke-UI-kode |
| 2014 | **Xamarin.Forms** | Cross-platform *UI* i XAML; delt business logic **og** UI på tværs af iOS, Android og UWP |
| 2016 | **.NET Core** | Reelt portabel runtime (Windows/Mac/Linux), men command-line-only |
| 2016 | Microsoft opkøber **Xamarin** | Xamarin.Forms 5 bliver sidste version |
| — | **.NET MAUI** | Næste evolution af Xamarin.Forms; helt ny platform bygget fra bunden |

## 1.2 What is .NET MAUI?

.NET Multiplatform App UI (MAUI) er et framework fra Microsoft til at bygge cross-platform UI-applikationer, der targeter **Windows, macOS, iOS og Android**. Med én kodebase kan man bygge en applikation, der understøtter alle platformene og deler 100 % af koden mellem dem. Al logik skrives i et .NET-sprog, og UI'et defineres i enten XAML eller ens .NET-sprog efter eget valg.

> **.NET MAUI development languages**
> Man kan skrive .NET MAUI-apps i C# og bruge enten XAML eller C# til at definere sit UI. Det er teknisk muligt at bruge andre sprog, men de er ikke officielt understøttet. Bogen bruger C# til logik og XAML til UI, da det er de mest udbredte tilgange.

**Figure 1.2** viser arkitekturen af en .NET MAUI-applikation. Pointen i figuren er, at MAUI er bygget *bottom-up*: hver platform leverer API'er, og der findes en .NET runtime pr. platform (WinRT på Windows, Mono på alt andet) bygget oven på disse API'er. Hvert lag leverer API'er, som bruges til at bygge API'erne i laget ovenover. Din egen kode skrives derimod *top-down*: du skriver en .NET MAUI-app, og arkitekturen indkapsler den for de underliggende lag.

Lagene nedefra og op:

1. **Målplatformens operativsystem** — Android, iOS, macOS eller Windows.
2. **.NET runtime** — Mono for Android, iOS og macOS; WinRT for Windows.
3. **.NET BCL** — den første abstraktion. Giver adgang til alle de sprogfeatures, vi forventer (lister, generics osv.), som ikke er en del af .NET's primitiver. Fra .NET 5 og frem er ".NET" (uden Core eller Framework) blevet den nye standard og har også afløst .NET Standard; target frameworks hedder nu fx `net7.0` i stedet for `netcoreapp` eller `netstandard`. BCL'et er tilgængeligt på alle platforme.
4. **Platform-specifikke abstraktioner** — .NET for Android og .NET for iOS er næste iteration af Xamarin.Android og Xamarin.iOS. Det er bindings til platform-API'erne med de samme typer og namespaces, som Objective-C-, Swift-, Java- eller Kotlin-udviklere bruger. .NET for Mac er nyt, men fungerer på samme måde, og WinUI-API'et bruges til Windows. Alt i platformens API er med — fra simple layouts og controls som knapper og tekstfelter til mere avancerede API'er som ARKit på iOS og ARCore på Android.
5. **.NET MAUI** — den sidste abstraktion. Et unified API med UI-elementer, der er fælles for alle understøttede platforme: views (layouts, buttons, text, entry fields), navigations-API'er og meget mere. Man får også adgang til fælles hardware-features som Bluetooth, location services og device storage.

Samme lagdeling set pr. platform:

| Platform | .NET runtime (lag 2) | Platform-abstraktion (lag 4) |
| --- | --- | --- |
| Android | Mono | .NET for Android (næste iteration af Xamarin.Android) |
| iOS | Mono | .NET for iOS (næste iteration af Xamarin.iOS) |
| macOS | Mono | .NET for Mac |
| Windows | WinRT | WinUI |

Filosofien bag at bygge en MAUI-app er top-down:

1. Byg en cross-platform applikation ved at skrive .NET MAUI-kode (i stedet for fx iOS- eller Android-kode).
2. Man *kan* skrive platform- eller OS-specifik kode i sin applikation, men man behøver ikke.
3. .NET MAUI kompilerer koden til målplatformen. Man behøver ikke forstå hvordan for at bygge en MAUI-app, men en god forståelse af platformene er en fordel: man kan bedre fejlsøge OS- eller platformspecifikke fejl, og man åbner hele spektret af platform-API'er, ikke kun dem der er eksponeret i top-level MAUI-wrappers.

.NET MAUI er mere end blot næste version af Xamarin.Forms. Hvor Xamarin var et SDK, man installerede uafhængigt af .NET, er MAUI en **workload** — altså en del af .NET på linje med ASP.NET eller console app-udvikling. Det demonstrerer Microsofts commitment til MAUI som en kernedel af .NET.

## 1.3 Cross-platform vs. "native" apps

Når man beslutter sig for at bygge en applikation, skal man stille nogle spørgsmål:

- Bygger man en installerbar, native binary executable — eller en web app?
- Hvis web app: bruger man et single-page application (SPA) framework (fx Angular eller Blazor), eller et traditionelt server-genereret page framework (fx ASP.NET Core eller PHP)?
- Hvis installerbar app: bygger man én app pr. platform, eller én app der kører overalt?

**Figure 1.3** skitserer denne beslutningsproces: forudsat at du er .NET-udvikler, er den største beslutning, om du vil bygge en web app eller en installerbar/eksekverbar app. Vælger du executable, er .NET MAUI et oplagt valg.

Det er vigtigt at aflive en myte: **apps bygget med .NET MAUI *er* native apps.** Enhver app skrevet i MAUI kompileres til en native binary executable for hver målplatform — på samme måde som havde den været skrevet i Swift på iOS eller Kotlin på Android (.NET-kode er dog stadig just-in-time [JIT]-kompileret som standard, men man kan aktivere ahead-of-time [AOT]).

Den større beslutning er derfor snarere binær applikation vs. web app. Argumenter for en binær applikation:

- **Multithreading** — applikationer i en webbrowser kan kun bruge én tråd ad gangen. I en binær app på multicore-hardware kan trådene reelt køre samtidig, så baggrundsprocesser ikke låser UI'et.
- **Encryption** — webapplikationer bruger kryptering til at kommunikere med backend-services, men man kan ikke sikkert gemme data offline i en browser. At kunne kryptere data *at rest* såvel som *in motion* kan være afgørende.
- **Adgang til device hardware-features** — mange hardware-features er tilgængelige i browsere nu (kamera, location services, endda Bluetooth). Andre, som telefoni eller SMS, er svære eller umulige at tilgå fra en browser-app. Konsistent og pålidelig adgang er langt lettere med en installeret binær app.
- **Adgang til platform-API'er** — fx ARKit på iOS eller ARCore på Android.

Måske det stærkeste argument er dog **branding**: en tilstedeværelse i app-butikkerne betragtes som kritisk for de fleste virksomheder og giver brugerne en tillid, som en web app alene måske ikke har.

**Figure 1.4** viser Verinote, en app bygget i Xamarin.Forms og under opgradering til .NET MAUI. Den er designet til politi og regulerede brancher og lader brugere i felten optage noter med fotos, lydoptagelser og skitser, som synkroniseres til en cloud-service. Den skal virke både offline og online, så data caches lokalt indtil cloud-servicen kan nås. På grund af informationernes følsomme karakter krypterer Verinote cachede data (og alle data i motion) og bruger platform-leveret biometrisk autentifikation. Netop derfor kunne den ikke være bygget som web app.

**Figure 1.5** viser AR-appen My Very Hungry Caterpillar, som placerer larven fra Eric Carles bog i brugerens rum. Sådanne oplevelser er forenklet af Apples og Googles API'er og ville være ekstremt vanskelige eller umulige i en browser.

Har man valgt en binær, installerbar app, er næste beslutning: flere versioner (én pr. platform) eller én cross-platform kodebase? Som regel giver det mening at bruge et cross-platform framework.

En populær tilgang er at bygge en web app og pakke den ind i en installerbar binary — fx Ionic til Angular-apps eller Electron til alt web-baseret. Det giver fuld adgang til native platform-API'er, men den centrale begrænsning er, at man stadig bruger en web view til at rendere og køre koden, med alle web-appens performance- og threading-begrænsninger.

Alternativet er én kodebase, der bygges som native app pr. målplatform. Det er .NET MAUI's tilgang, ligesom React Native og Flutter. Fordelene inkluderer multithreading og andre performance-gevinster samt fuld adgang til alle native platform-API'er — i MAUI's tilfælde garanteret på udgivelsesdagen. Med web-app-wrappers er man ofte afhængig af plugins uden garanti for, at de features man har brug for er tilgængelige.

Den afgørende forskel mellem .NET MAUI og andre frameworks i kategorien er, at **UI'et man bygger i MAUI er en abstraktion af platformens native UI**. Kører en MAUI-app på iOS, ligner den en iOS-applikation; kører den på Windows, ligner den en Windows-applikation. **Figure 1.6** viser en .NET MAUI `DatePicker` i den samme applikation på Android (venstre) og Windows (højre) — begge versioner kører fra samme kode uden ekstra modifikationer.

Man kan naturligvis også vælge et fuldt custom UI, der ser ens ud alle steder. Det kræver ikke mere arbejde i MAUI end i andre frameworks. Men at bygge en applikation, der er *konsistent men ikke identisk* på hver platform, kræver slet ingen ekstra indsats i MAUI — at opnå det samme i React Native eller Flutter ville kræve flere implementeringer af samme control, én pr. platform.

**Figure 1.7** viser Microsoft Word på macOS (øverst) og Windows (nederst). Hver version forbliver konsistent med sin platform, men bevarer produktets brand og UX. Navigation og UX er lige så velkendt for brugeren af den ene platform som den anden.

Alle de tilgængelige muligheder er gode og modne. Valget afhænger typisk af, hvad der ligger inden for ens komfortzone — hvem der bakker frameworket op, og hvilket sprog det bruger. Elsker man Microsoft og er erfaren C#-udvikler, er .NET MAUI det oplagte valg; er man Google-fan og fortrolig med Dart, vælger man nok Flutter.

## 1.4 .NET MAUI and the .NET ecosystem

At skrive en cross-platform applikation i .NET MAUI er en god mulighed for .NET-udviklere. Man bruger sit foretrukne sprog og udviklerværktøjer og bygger videre på de færdigheder, man allerede har. Man har adgang til de samme ressourcer som i andre .NET-projekter: eksisterende supportnetværk, packages og patterns (dog med nogle nye patterns at lære til mobiludvikling). Da MAUI-apps er .NET-projekter, er mange — hvis ikke alle — af ens foretrukne NuGet-packages tilgængelige (om de er egnede til mobiludvikling er en anden sag), ligesom mange packages er skræddersyet til mobil og cross-platform UI-udvikling.

**Figure 1.8** viser pointen: man kan bygge full-stack cloud-, web- og desktop/mobil-applikationer, hvor alle komponenterne deler én kodebase.

Bygger man en full-stack løsning, får man fordelen af at kunne dele kode mellem lagene. Eksempel: en chat-app med et ASP.NET Core web API med SignalR i Azure, et Blazor web-UI og et .NET MAUI mobil- og desktop-UI. Her kan logikken og connectivity, der forbinder UI'et til web-API'et, deles på tværs af Blazor- og MAUI-apps, og i nogle tilfælde kan de samme NuGet-packages bruges i både klient og server. Ændrer man API'et, opdaterer man klientkoden ét sted, og ændringen afspejles automatisk i alle klient-UI-applikationer.

Fordi MAUI er en kernedel af .NET og en workload frem for et SDK, kan man bruge alle sine velkendte udviklingsværktøjer. **Virker det med .NET, virker det med .NET MAUI.** Det inkluderer Visual Studio (Mac eller Windows), Visual Studio Code og .NET CLI (Visual Studio giver dog den mest gennemarbejdede oplevelse), build- og DevOps-værktøjer, NuGet-packages og alt andet. .NET MAUI er ikke et add-on; det *er* .NET — og det gælder for ens færdigheder lige så meget som for ens værktøjer.

## 1.5 .NET MAUI development paradigms

XAML er de facto-valget til at bygge applikationer i .NET MAUI. Microsoft skabte XAML til WPF, men det er siden brugt til Silverlight (Silverlight var netop et XAML-renderer-plugin til webbrowsere), Windows Phone, UWP, Xamarin.Forms og nu .NET MAUI.

XAML er et godt valg for de fleste. Det er et XML-baseret markup-sprog til at definere et UI — på samme måde som HTML eller den rene XML, der bruges til at bygge Android-UI'er — så det er typisk hurtigt at lære for folk der kommer fra Angular, ren HTML eller Android, og især for dem med XAML-erfaring fra WPF eller Xamarin.Forms. De forskellige "flavors" af XAML kan drille — der er subtile forskelle mellem WPF, Xamarin.Forms og .NET MAUI XAML — men forskellene er lette at lære, og XAML IntelliSense i Visual Studio 2022 gør det endnu lettere.

### MVVM

Komplementært til XAML er **Model-View-ViewModel (MVVM)**-mønstret. **Figure 1.9** beskriver rollerne:

- **Model** — repræsentationen af det problem, appen løser. Består af entiteter og services og indeholder business logic.
- **ViewModel** — repræsenterer *state* for din View og indeholder logik til at interagere med Model. ViewModel reagerer på events i View, sender data til Model, og ændrer View'ets state, når Model leverer information der kræver det.
- **View** — dit XAML-definerede UI. Indeholder alt hvad appen har brug for for at vise ting på skærmen ifølge dit design, inklusive UI controls og eventuel kode til at ændre hvordan de vises.

MVVM behandles i dybden i kapitel 9.

### MVU

XAML er ikke det eneste valg. Man kan også deklarere sit UI i kode frem for markup: man instantierer klassen for den UI control eller view, man vil vise, og angiver dens properties.

Ud over at deklarere UI i kode kan man bruge **Model-View-Update (MVU)**-paradigmet i stedet for MVVM (på skrivetidspunktet er MVU-support eksperimentel i .NET MAUI). MVU, også kendt som *the Elm Architecture*, adskiller sig fra MVVM på to afgørende punkter: **Model er immutable, og data flyder kun i én retning.** Man kan derfor ikke ændre Model som svar på UI-ændringer (brugerinput), for det ville bryde begge regler. I stedet flyder ændringer i View til en **Update**, som genererer en ny Model, og View ændrer sig derefter som svar på den nye Model.

**Figure 1.10** viser dette flow: Model repræsenterer hele applikationens state; ændringer i Model sendes til View, som ændrer hvad der vises; ændringer i View dispatches til en Update-funktion, som genererer en ny Model.

MVU vil være genkendeligt for folk med baggrund i native iOS-udvikling (Objective-C eller Swift) eller React-udviklere (React's virtual DOM er en version af MVU-mønstret). I .NET MAUI skal man hente et bibliotek ind for at understøtte mønstret — **Comet** til C# og **Fabulous** til F#. Xamarin.Forms havde tættere kobling mellem API'et og de underliggende platforme, hvilket gjorde den slags mønstre sværere. I MAUI giver abstraktionerne en ren adskillelse mellem lagene (jf. figure 1.2), så forskellige implementeringer kan indsættes i ethvert lag. Det gør MVU-bibliotekerne til "first-class citizens" i MAUI-økosystemet, og Microsoft bakker fuldt op om dem.

MVU dækkes ikke i bogen, da fokus er på kernemåden at gøre tingene på.

### .NET MAUI Blazor

.NET MAUI giver også en tredje mulighed: at bygge sit UI med **Blazor**. Blazor er et SPA-framework fra Microsoft, der lader dig bygge applikationer, som kører client-side i webbrowsere med .NET frem for JavaScript eller TypeScript.

Tilgangen minder om web-app-wrapper-løsningerne fra afsnit 1.3, og på mange måder er den det: man skriver en web app i et SPA-framework (Blazor) og bruger en wrapper (.NET MAUI) til at pakke den web app ind i en installerbar binary executable på tværs af platforme.

.NET MAUI Blazor bruger en web view til at rendere UI'et, ligesom Electron, Ionic eller Cordova. **Den afgørende forskel er, at C#-koden i en .NET MAUI Blazor-app køres som .NET managed code** — ligesom i en XAML MAUI-app — frem for at blive kørt af scripting-motoren i web view'et, som er tilfældet med Cordova, Ionic eller Electron. Derudover får man i en MAUI Blazor-app adgang til alle de platform-API'er, der er eksponeret via .NET-abstraktioner (jf. figure 1.2).

Med .NET MAUI kan man bygge en full-stack applikation med .NET i hvert lag: ASP.NET Core til API'et, Blazor til web-appen og .NET MAUI til mobil- og desktop-klienter. Vælger man .NET MAUI Blazor, kan man endda placere sine views i et Razor class library og dele Blazor-kompatible UI'er på tværs af web-, mobil- og desktop-projekter.

Bogen fokuserer dog på .NET MAUI-udvikling med XAML og MVVM-mønstret, fordi formålet er at lære .NET MAUI, ikke Blazor — og fordi XAML med MVVM er de facto-valget til MAUI-apps.

### Opsummering af de tre paradigmer

| | **XAML + MVVM** | **MVU** | **.NET MAUI Blazor** |
| --- | --- | --- | --- |
| UI defineres i | XAML markup (eller C#) | C#/F# i kode | Razor-komponenter |
| Rendering | Native platform-UI | Native platform-UI | Web view |
| Model | Mutable | **Immutable** | — |
| Dataflow | To-vejs via ViewModel | **Én retning**: View → Update → ny Model → View | — |
| Understøttelse | De facto-valget; fuldt understøttet | Eksperimentel i MAUI; kræver **Comet** (C#) eller **Fabulous** (F#) | Understøttet |
| Kendt fra | WPF, Xamarin.Forms | Elm Architecture, React (virtual DOM), native iOS | Blazor SPA |
| Dækkes i bogen | **Ja** | Nej | Nej |

Grunden til at MVU-bibliotekerne kan være "first-class citizens" i MAUI er, at abstraktionerne giver en ren adskillelse mellem lagene (jf. figure 1.2), så forskellige implementeringer kan indsættes i ethvert lag — i modsætning til Xamarin.Forms, hvor koblingen til de underliggende platforme var tættere.

## Summary

- .NET MAUI er en cross-platform, write once, run anywhere (WORA) UI-applikationsplatform. Man bygger én .NET MAUI-app, og den kører på flere platforme uden yderligere modifikation.
- Man kan skrive native apps med .NET MAUI. .NET MAUI-apps *er* native apps.
- Man kan bygge apps i .NET MAUI med funktionelle, performance- og sikkerhedsmæssige fordele over web apps.
- Man kan bruge hele .NET-økosystemet til at bygge .NET MAUI-apps. Det inkluderer alle ens foretrukne NuGet-packages og ens eksisterende færdigheder som .NET-udvikler.
- Man kan skrive .NET MAUI app-UI'er i XAML, C#, F# eller Blazor (bogen bruger XAML).
