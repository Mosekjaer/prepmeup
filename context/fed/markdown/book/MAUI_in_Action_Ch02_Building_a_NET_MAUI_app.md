# Kapitel 2 – Building a .NET MAUI app

## Metadata

- **Kapitel:** 2 – Building a .NET MAUI app
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L01
- **Hovedemner:**
  - Værktøjer: Visual Studio 2022 (Windows/Mac) vs. .NET CLI
  - .NET MAUI project templates (`maui`, `mauiblazor`, `mauilib`, `mauipage-xaml`, `blankmaui` m.fl.)
  - Oprettelse af app fra template via GUI og `dotnet new`
  - Kørsel og debugging pr. målplatform (`dotnet build -t:Run -f:net7.0-ios`)
  - Single project solutions og multitargeting (modsat Xamarin.Forms)
  - App-anatomi: `MauiProgram.CreateMauiApp` → `App` → `MainPage`
  - Hostbuilder-mønstret og `UseMauiApp<App>()`
  - Platform-specifikke entry points: `AppDelegate` (iOS/macOS), `MainActivity` (Android)
  - XAML-struktur: namespaces, `x:Class`, `ScrollView`/`VerticalStackLayout`/`Image`/`Label`/`Button`
  - Accessibility (a11y) og semantic properties
  - .NET Hot Reload og dens begrænsninger

---

## Indledning

Enhver .NET-udvikler kan bygge mobil- eller desktop-UI-apps med .NET MAUI. Der er en lille læringskurve omkring UI- og markup-specifik syntaks og design patterns. Har man erfaring med et web-UI-framework (især Angular), føles .NET MAUI hurtigt komfortabelt, men forudgående erfaring er ikke nødvendig.

## 2.1 Saying "Aloha, World!" with .NET MAUI

I dette afsnit bygges den første .NET MAUI-app: *Aloha, World!*

> **Which option should I choose?**
> Valget mellem Visual Studio og .NET CLI er op til én selv. Visual Studio (både macOS og Windows) inkluderer avancerede udviklingsfeatures, man ikke får med CLI'en: XAML Live Preview giver real-time feedback på designændringer, og der er kraftfuld IntelliSense og IntelliCode samt modne test- og debugging-features. Det er også let at vælge målplatform via drop-down på Run-knappen.
>
> .NET CLI er mere kompliceret her, og — vigtigere — **.NET CLI understøtter ikke targeting af Windows** (man kan stadig bygge og køre MAUI-apps på Windows fra en kommandoprompt, men skal bruge MSBuild frem for .NET CLI).
>
> GUI'ens primære fordel er discoverability. Men en øvet CLI-bruger opnår markante produktivitetsgevinster. Følgende tager et sekund eller to:
>
> ```bash
> mkdir AlohaWorld
> cd AlohaWorld
> dotnet new blankmaui
> ```
>
> Det tager .NET CLI ca. 0,4 sekunder at bygge en ny MAUI-app fra template; det tilsvarende i Visual Studio kan tage betydeligt længere. Angular CLI har fx mange analoger til .NET CLI. Vælger man CLI, skal man stadig bruge en editor — Visual Studio Code er et populært valg, men en MAUI-app er i bund og grund en samling tekstfiler (indtil `dotnet build` kompilerer dem).

Man skal sikre sig, at .NET MAUI-workloaden er installeret via Visual Studio-installeren eller .NET CLI. Bogen bruger desuden en *blank* .NET MAUI project template, som skal installeres separat.

> **NOTE** .NET MAUI understøtter teknisk .NET 6 og frem, men .NET 7 er minimumsversionen i bogen. Vælg .NET 7 i wizards (og .NET 8, når den udkommer).

### 2.1.1 Visual Studio 2022

Visual Studio er Microsofts primære førstepartsværktøj til MAUI-udvikling. Version 2022 er minimumskravet og har indbygget support for features som XAML Live Preview og .NET Hot Reload. Visual Studio findes til Windows og macOS i editions fra gratis Community til Enterprise — MAUI virker med alle, og alt i bogen kan gøres med Community.

**Mac:** Åbn Visual Studio 2022 og klik New. Scroll ned til **Multiplatform**-sektionen i listen til venstre, klik **App** for at få MAUI-templates frem, og vælg **Blank .NET MAUI Template** (**Figure 2.1**). Klik Continue. Angiv `AlohaWorld` som projektnavn; solution-navnet sættes som standard til det samme. **Figure 2.2** viser de filer og mapper, der vises i Solution Explorer efter oprettelsen.

**Windows:** Åbn Visual Studio og klik **Create a New Project**. Vælg **Blank .NET MAUI Template** fra listen. Man kan filtrere via drop-downs for sprog, platform og projekttype, men det hurtigste er fritekst-søgefeltet — **Figure 2.3** viser, hvordan søgeordet `maui` indsnævrer listen til .NET MAUI-projekter. Klik Next, vælg mappe, angiv `AlohaWorld` som projektnavn, og klik Create. **Figure 2.4** viser den færdige solution i Solution Explorer.

### 2.1.2 .NET CLI overview

Da .NET MAUI er en .NET-workload frem for en ekstern package, virker project templates på samme måde som alle andre .NET-templates og accepterer de samme inputs og switches. De er dokumenteret hos Microsoft og kan findes via online help ved at tilføje `--help` efter enhver `dotnet`-kommando.

For at se de tilgængelige templates:

```bash
dotnet new list
```

MAUI-templates ligger belejligt øverst i listen.

**Table 2.1 — .NET MAUI templates:**

| Template name | Short name | Beskrivelse |
| --- | --- | --- |
| .NET MAUI App | `maui` | Hovedtemplaten til nye .NET MAUI-apps. |
| .NET MAUI Blazor App | `mauiblazor` | Ny MAUI-app der bruger Blazor til at definere sit UI. |
| .NET MAUI Class Library | `mauilib` | Nyt class library til deling af kode mellem MAUI-projekter. |
| .NET MAUI ContentPage (C#) | `mauipage-csharp` | Ny application page med UI defineret deklarativt i C#. |
| .NET MAUI ContentPage | `mauipage-xaml` | Ny application page med UI i XAML markup (plus en tilsvarende C# code-behind-fil). |
| .NET MAUI ContentView (C#) | `mauiview-csharp` | Ny content view (genbrugelig UI-komponent til brug i MAUI-sider) med UI i C#. |
| .NET MAUI ContentView | `mauiview-xaml` | Ny content view med UI i XAML markup (plus code-behind). |
| .NET MAUI ResourceDictionary (XAML) | `mauidict-xaml` | Ny resource dictionary i XAML. Lader dig definere og navngive colors, styles og templates til genbrug i hele appen. |
| Blank .NET MAUI template | `blankmaui` | Ekstra template leveret af forfatteren. Samme som default-templaten (`maui`), bortset fra at default bruger Shell (dækkes først i kapitel 5). Denne bruger ikke Shell og er lettere at arbejde med i de første kapitler. |

.NET CLI har konventioner, der gør den let at bruge. Bruger man `dotnet new` med en template uden at angive andre parametre, oprettes en ny solution med alle default-optioner og med den indeholdende mappes navn som solution-navn.

> **Overriding default values**
> Man kan overskrive default-adfærden med ekstra command-line-parametre — fx et andet output directory eller solution-navn:
>
> ```bash
> dotnet new blankmaui -n HelloWorld -o C:\code\HelloWorld
> ```
>
> Se `dotnet new --help` eller dokumentationen for alle switches.

### 2.1.3 .NET CLI in action

Opret en mappe `AlohaWorld`, naviger ind i den, og kør:

```bash
dotnet new blankmaui
```

Ved succes vises meddelelsen:

```
The template "Blank .NET MAUI template" was created successfully.
```

**Figure 2.5** viser de genererede solution-filer listet i terminalen.

## 2.2 Running and debugging your app

> **Single project solutions**
> Kommer man fra Xamarin.Forms, vil man bemærke en forskel. I Xamarin.Forms havde man ét projekt pr. platform (fx `MyApp` til delt logik og UI, `MyApp.Android` til Android og `MyApp.iOS` til iOS) og satte projektet for målplatformen som startup project. **I .NET MAUI har man en single project solution og bruger multitargeting til at vælge, hvor appen skal køre.** Man kan tilføje class libraries eller andre projekter, men det er ikke nødvendigt for at target forskellige platforme.

### 2.2.1 Visual Studio for Windows

Brug drop-down'en på Run-knappen i toolbaren. Fra menuen vælges **Framework**-undermenuen og derefter `Net7.0-windows[din windows build-version]` som target (**Figure 2.6**).

> **Windows Developer Mode**
> Man skal aktivere Developer Mode på Windows. Developer Mode lader én køre usignerede apps; som standard blokeres eksekverbare filer, der ikke er signeret af en betroet autoritet. MAUI-apps under udvikling er usignerede, så Developer Mode er påkrævet — men overvej at slå det fra igen, når det ikke aktivt bruges, af sikkerhedshensyn. Det gøres i Settings-appen.

Klik Run-knappen for at køre appen på Windows. **Figure 2.7** viser AlohaWorld kørende på Windows — klik **Click Me**-knappen for at se ændringerne.

### 2.2.2 Visual Studio for Mac

Nær øverste venstre hjørne er en Run-knap (**Figure 2.8**), og til højre for den profilen (Release eller Debug) og derefter target (som standard **My Mac**). Klikker man **My Mac**, vises alle de target devices, appen kan køre på: den Mac man kører Visual Studio på, samt aktuelt understøttede iOS device simulators.

Lad My Mac være valgt, og klik Run. **Figure 2.9** viser AlohaWorld kørende på macOS.

### 2.2.3 .NET CLI

Naviger til solution-mappen og kør Run-kommandoen svarende til målplatformen. **Målplatformen er den platform, appen skal køre på — ikke den platform, den er udviklet på.**

```bash
dotnet build -t:Run -f:net7.0-[target platform]
```

**Table 2.2 — .NET MAUI target platforms:**

| Operating system | Target platform | Noter |
| --- | --- | --- |
| macOS | Mac Catalyst | Mac Catalyst er en bro, der lader apps bygget til iOS køre på macOS. .NET MAUI bruger Mac Catalyst til at køre apps på macOS. Man kan kun target macOS, når man udvikler på macOS. |
| iOS | iOS | Man kan target iOS fra både macOS og Windows (men en macOS-computer er påkrævet for at publicere MAUI-apps til App Store). |
| Android | Android | Man kan target Android fra både macOS og Windows. |

> **NOTE** Windows er en målplatform for .NET MAUI, men mangler i table 2.2, fordi man ikke kan bygge MAUI-apps til Windows med .NET CLI. Hold dig til Visual Studio — eller brug MSBuild, hvis du vil bruge en terminal.

For at køre appen på iOS:

```bash
dotnet build -t:Run -f:net7.0-ios
```

**Figure 2.10** bryder kommandoen ned:

- `dotnet` — navnet på den eksekverbare, operativsystemet skal køre; her .NET CLI.
- `build` — kommandoen, .NET CLI skal udføre. Den bygger projektet eller solution, og .NET CLI forventer en `.csproj`- eller `.sln`-fil i den aktuelle mappe (ekstra parametre kan angive et andet projekt).
- `-t:Run` — fortæller .NET CLI, at solution skal køres efter build er færdig.
- `-f:` — angiver, at der følger en framework-parameter, som skal bruges som målplatform. Alle frameworks bruger `net7.0-` som prefix, da .NET MAUI kræver .NET 6 som minimum.

## 2.3 Anatomy of a .NET MAUI app

I et normalt C#-program leder .NET efter entry point'et: en statisk metode kaldet `Main`, der returnerer enten `int` eller `void` og har enten ingen parametre eller en enkelt parameter af typen `string[]` kaldet `args`. **Figure 2.11** viser dette flow: et almindeligt C#-program starter i `Main` i Program.cs og eksekverer kode, indtil det afslutter og returnerer enten `null` eller en exit code.

I en .NET MAUI-app forventer .NET derimod en metode, der returnerer et objekt af typen `MauiApp`. Den ligger i **MauiProgram.cs**, oprettet af templaten, og indeholder en statisk metode `CreateMauiApp`, der bruger det generiske hostbuilder-mønster med returtypen `MauiApp`.

**Figure 2.12** og **Figure 2.13** viser flowet: en .NET MAUI-app starter i `CreateMauiApp` i MauiProgram.cs, som launcher en instans af `App`-klassen, som viser den `Page`, der er tildelt `MainPage`-propertyen på `App`. Kort sagt: **MauiProgram er entry point, som launcher App, som viser MainPage.**

`CreateMauiApp` bygger `MauiApp`-objektet med en version af .NET host builder, som ASP.NET Core- og Blazor-udviklere vil genkende. En extension method `UseMauiApp` kaldes på hostbuilderen med `App` som type-parameter. `UseMauiApp` forventer en type, der implementerer `IApplication`-interfacet, og `App`-klassen nedarver `Application`-klassen, som implementerer dette interface.

### Listing 2.1 MauiProgram.cs

```csharp
namespace AlohaWorld;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()          // ① Statisk metode med returtype MauiApp
    {
        var builder = MauiApp.CreateBuilder();     // ② Hostbuilder-mønstret
        builder
            .UseMauiApp<App>()                     // ③ Forventer en type der implementerer IApplication
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();                    // ④ Hostbuilderen kører Build() og returnerer resultatet
    }
}
```

① `CreateMauiApp` er en statisk metode med returtypen `MauiApp`.
② I en .NET MAUI-app bruger vi .NET hostbuilder-mønstret.
③ `.UseMauiApp` extension method forventer en type-parameter, hvis type implementerer `IApplication`-interfacet.
④ Hostbuilderen eksekverer `Build()` og returnerer resultatet.

`App`-klassen har et member kaldet `MainPage`. I `App`-konstruktoren tildeles dette member en værdi af typen `ContentPage` (som nedarver `Page`-baseklassen). .NET MAUI viser derefter den side til brugeren, når appen er færdig med at loade.

Bemærk navneforvirringen: den type, vi instantierer, hedder også `MainPage`, men det er klassenavnet — i `App`-klassen er `MainPage` et member af typen `Page`. `Page` er den type i .NET MAUI, der bruges til at definere sider i en applikation. **En page er et fuldskærms-view, og et view er noget der vises på skærmen.**

Det væsentlige: for at starte appen skal vi have et objekt af typen `Page` (det behøver ikke hedde `MainPage` — det kan hedde hvad som helst, men hedder `MainPage` i alle templates), som vi sætter som værdien af `MainPage`-memberet i `App`-klassen.

### Listing 2.2 App.xaml.cs

```csharp
namespace AlohaWorld;

public partial class App : Application     // ① App nedarver Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new MainPage();         // ② Tildeler en ContentPage til Page-propertyen
    }
}
```

① `App`-klassen nedarver `Application`-klassen.
② `Application`-baseklassen leverer en property af typen `Page` kaldet `MainPage`. I `App`-klassen tildeler vi en `ContentPage` kaldet `MainPage` til denne property, og .NET MAUI viser denne side ved load.

### Platform-specifikke entry points

På Windows er ovenstående proces præcis. Men på macOS, iOS og Android forventer operativsystemets SDK'er specifikke entry points til at starte en app, og .NET MAUI giver instanser af disse, som man kan tilpasse ved behov: **`AppDelegate`-klassen** for macOS og iOS, og **`MainActivity`-klassen** for Android. **Figure 2.14** viser, hvordan disse platform-specifikke entry points sætter det almindelige program-eksekveringsflow i gang.

Filerne ligger i mappen **Platforms** og en undermappe navngivet efter det relevante OS. .NET MAUI leverer dem til OS'et som entry point og loader `MauiProgram` bag kulisserne — **Figure 2.15** viser dem i Solution Explorer. For det meste behøver man ikke røre dem, når man bygger MAUI-apps.

### MainPage.xaml's struktur

**Figure 2.16** nummererer de layouts og views, der bruges i default-appen: 1. `ScrollView`, 2. `VerticalStackLayout`, 3. `Image`, 4. `Label`, 5. `Label`, 6. `Button`.

Views (layouts og controls) i MainPage.xaml uden properties:

```xml
<ContentPage>
    <ScrollView>                     <!-- 1 -->
        <VerticalStackLayout>        <!-- 2 -->
            <Image />                <!-- 3 -->
            <Label />                <!-- 4 -->
            <Label />                <!-- 5 -->
            <Button />               <!-- 6 -->
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

Første child-element i content page'en er en **`ScrollView`** — en UI-container, der lader brugeren scrolle for at se indhold, der er for stort til at være på skærmen:

```xml
<ContentPage>
    <ScrollView>                     <!-- 1 -->
    ...
```

Inde i `ScrollView` er en **`VerticalStackLayout`**. Denne layout-komponent lader dig arrangere views sekventielt og vertikalt (det første øverst, det næste under, osv.):

```xml
...
    <VerticalStackLayout>            <!-- 2 -->
    ...
```

Inde i `VerticalStackLayout` er et image, to labels og en button:

```xml
...
    <VerticalStackLayout>            <!-- 2 -->
        <Image .../>                 <!-- 3 -->
        <Label .../>                 <!-- 4 -->
        <Label .../>                 <!-- 5 -->
        <Button .../>                <!-- 6 -->
    </VerticalStackLayout>
...
```

XAML (eXtensible Application Markup Language) er blot XML med nogle custom elementnavne, vi bruger til at bygge .NET MAUI-UI'er. Vi forbruger disse elementnavne ved at importere de relevante namespaces:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AlohaWorld.MainPage"
             ...
```

- Linje 1 henter `http://schemas.microsoft.com/dotnet/2021/maui`-schemaet ind som **default namespace**. Det indeholder de fleste af de tag-navne, vi bruger.
- Linje 2 henter `http://schemas.microsoft.com/winfx/2009/xaml`-schemaet ind og tildeler det til `x`-namespacet. Det indeholder tags til *metadata* om vores UI frem for UI'et selv.
- Linje 3 bruger `Class`-tagget fra `x`-namespacet til at associere XAML-markup-filen med en C#-klasse. Den tilhørende code-behind-klassefil ses i Solution Explorer.

> **All views are classes**
> Alle views i .NET MAUI-apps er C#-klasser. Det inkluderer `ContentPage`-views (komplette applikationssider) såvel som controls (ting vist på en side). Når du bruger en XAML-fil, bruger du XAML-markup til at fortælle view'et, hvordan UI'et skal vises — men view'et selv er stadig en klasse.
>
> Alt hvad du kan i XAML, kan du i C#, så du kunne bygge din app helt uden XAML. **Du kan have et .NET MAUI-view, der er en C#-fil uden en XAML-fil, men du kan ikke have et XAML-view uden C#.**

### Listing 2.3 MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AlohaWorld.MainPage">

    <ScrollView>
        <VerticalStackLayout Spacing="25" Padding="30,0"
                             VerticalOptions="Center">

            <Image
                Source="dotnet_bot.png"
                SemanticProperties.Description="Cute dot net bot waving hi to you!"
                HeightRequest="200"
                HorizontalOptions="Center" />

            <Label
                Text="Hello, World!"
                SemanticProperties.HeadingLevel="Level1"
                FontSize="32"
                HorizontalOptions="Center" />

            <Label
                Text="Welcome to .NET Multi-platform App UI"
                SemanticProperties.HeadingLevel="Level2"
                SemanticProperties.Description="Welcome to dot net Multi platform App U I"
                FontSize="18"
                HorizontalOptions="Center" />

            <Button
                x:Name="CounterBtn"
                Text="Click me"
                SemanticProperties.Hint="Counts the number of times you click"
                Clicked="OnCounterClicked"
                HorizontalOptions="Center" />

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
```

① `ContentPage`-tagget omslutter alle andre UI-elementer. Sætter også default namespace (`xmlns`) til standard .NET MAUI-schemaet.
② Sætter `x`-namespacet til XAML metadata-schemaet.
③ Associerer XAML-filen med den klasse, den svarer til.
④ En `ScrollView`-control indeholder den `VerticalStackLayout`, der definerer resten af layoutet. Ved at omslutte `VerticalStackLayout` i en `ScrollView` kan vi scrolle indhold, der ikke passer på skærmen.
⑤ `VerticalStackLayout`-tagget med properties for **padding** (afstand mellem `VerticalStackLayout`-grænsen og de elementer, den indeholder) og **spacing** (afstanden mellem rækker).
⑥ Et `Image`, der deklarerer en `Source`-property, som fortæller .NET MAUI, hvor billedet skal hentes. Det kan være en URL eller en embedded resource.
⑦ `Label`s der deklarerer deres `Text`-properties.
⑧ Semantic properties bruges af assistive technologies til at give beskrivende information om elementer på skærmen.
⑨ En `Button` der definerer sin `Text`-property (teksten på knappen) og den event handler, knappens `Clicked`-event delegerer til. Knappen har en `Name`-deklaration (fra `x:`-namespacet). At navngive elementer lader os tilgå dem fra kode eller med binding-referencer i XAML (mere i kapitel 3).

> **Accessibility in .NET MAUI apps**
> Accessibility (forkortet a11y) er en kritisk overvejelse. Vi bruger ofte visuelle cues til at få meta-information om ting på skærmen — fx kan overskriftsstørrelse indikere vigtighed eller hierarki. En synshandicappet bruger har ikke adgang til disse cues.
>
> Web Content Accessibility Guidelines (WCAG) 2.1-standarden giver omfattende vejledning i at lave apps, der både ser godt ud og er tilgængelige. Assistive technologies som screen readers kan hjælpe synshandicappede brugere og undertiden erstatte visuel brug af skærmen helt. For disse brugere er det vigtigt at levere metadata, som screen readers kan fortolke.
>
> **Semantic properties** i .NET MAUI gør netop dette. I listing 2.3 bruges semantic properties til at deklarere heading level for en `Label` over for en screen reader. Det gør det muligt for screen reader'en ikke bare at læse indholdet af `Label`, men også fortælle brugeren, hvor i hierarkiet den ligger.

## 2.4 Seeing real-time changes with Hot Reload

Indtil nu har appen heddet AlohaWorld, men står der "Hello, World!". Følgende trin viser, hvordan man ændrer et par ting i appen *mens den kører* og ser ændringerne afspejlet i realtid uden at genstarte. Tricket hedder **Hot Reload** og er i øjeblikket kun understøttet i Visual Studio og Visual Studio for Mac — .NET CLI understøtter ikke Hot Reload for MAUI-apps.

**Figure 2.17** viser Dotnet Bot, den officielle .NET-maskot. På https://mod-dotnet-bot.net kan man bygge sin egen version og downloade den via Share-knappen. Filen skal omdøbes, så den overholder .NET MAUI's filnavnskrav: **filnavnet må kun indeholde små bogstaver og underscores.**

### 2.4.1 Visual Studio for Windows

1. Åbn filen MainPage.xaml.
2. Find `Text`-propertyen på det `Label`-element, der har værdien `'Hello, World!'`. Ret værdien til `'Aloha, World!'`.
3. Skift tilbage til den kørende app. Din label skulle nu vise "Aloha, World!".
4. Højreklik **Resources**-mappen i Solution Explorer, og vælg **Add Existing Item**.
5. Vælg den custom Dotnet Bot, du downloadede.
6. Højreklik billedet i Resources-mappen og vælg **Properties**.
7. I Properties-vinduet skal **Build Action** være sat til `MauiImage`.
8. Tilbage i MainPage.xaml er `Source`-propertyen på `Image`-elementet sat til `dotnet_bot` på linje 13. Ret den til filnavnet (inklusive extension) på den fil, du importerede.
9. Gem ændringerne til MainPage.xaml.

**Figure 2.18** viser **Hot Reload**-knappen til højre for Run-knappen i toolbaren (den med den røde flamme). Klikker man den, advarer Visual Studio om, at ændringerne ikke kan anvendes med Hot Reload, og at der skal rebuildes. **Figure 2.19** viser resultatet: "Aloha, World!".

Hot Reload-knappen er også en drop-down. Udvider man den, findes **Hot Reload on File Save**, som automatisk anvender Hot Reload (for understøttede ændringer) ved gem. Det svarer til at køre `dotnet watch run` for en ikke-MAUI .NET-app med .NET CLI.

> **Why does Hot Reload need to rebuild sometimes?**
> Hot Reload lader os ændre kode, mens en app kører, uden at stoppe debugging og rebuilde. Men det har begrænsninger.
>
> Når vi bygger en applikation med .NET, kompileres den til Common Language Runtime (CLR)-kode. Det inkluderer en slags fortegnelse over alle vores klasser og deres public members (metoder og properties) og resources. **Denne fortegnelse kan ikke ændres at runtime**, så vi kan godt ændre koden *inde i* en metode, men vi kan ikke tilføje eller fjerne klasser, metoder eller resources (fx billeder) uden en rebuild.

### 2.4.2 Visual Studio for Mac

1. Åbn filen MainPage.xaml.
2. Find `Text`-propertyen på det `Label`, der har værdien `'Hello, World!'`. Ret den til `'Aloha, World!'`.
3. Skift tilbage til den kørende app. Labelen skulle nu vise "Aloha, World!" (**Figure 2.20**).
4. Tilbage i Visual Studio: højreklik **Resources**-mappen i Solution Explorer og vælg **Add > Existing files**.
5. Vælg den custom Dotnet Bot, du downloadede.
6. Vælg filen, og vælg i **Add File to Folder**-dialogen at kopiere filen til mappen.
7. Udvid Resources-mappen, højreklik det tilføjede billede og vælg **Properties**.
8. Under **Build**-sektionen sættes **Build Action**-drop-down'en til `MauiImage`.
9. Tilbage i MainPage.xaml rettes `Source`-propertyen på `Image`-elementet fra `dotnet_bot` (linje 13) til filnavnet inklusive extension.
10. Gem ændringerne til MainPage.xaml.
11. Skift tilbage til den kørende app. Dotnet Bot'en er nu væk.
12. Stop appen og start den igen. Nu vises din custom Dotnet Bot.

**Figure 2.20** viser pointen: tekstændringen fra "Hello, World!" til "Aloha, World!" ses uden genstart, men billedet kræver genstart. Årsagen er den samme som i sidebaren ovenfor og gælder både macOS og Windows.

## Summary

- Man kan oprette en .NET MAUI-app fra en template leveret af .NET via .NET CLI, Visual Studio for Mac eller Visual Studio for Windows.
- Man kan bygge og køre en .NET MAUI-app på enhver af målplatformene (iOS, Android, macOS eller Windows) — også fra CLI'en eller begge Visual Studio-varianter.
- For at vælge platform bruges drop-down-selectoren i Run-knappen.
- Man kan ændre sin app, mens den kører. .NET Hot Reload anvender ændringerne i realtid uden genstart.
- Man kan bygge UI'et i en .NET MAUI-app med enten XAML eller C#. XAML er den mest udbredte tilgang.
- Man kan bruge det velkendte hostbuilder-mønster i .NET MAUI-apps.
