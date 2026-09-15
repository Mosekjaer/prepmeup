# Kapitel 7 — Pages and navigation

## Metadata

- **Kapitel:** 7 — Pages and navigation
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L06
- **Hovedemner:**
  - `ContentPage` og dens `Content`-property som den grundlæggende byggesten for en page
  - Fælles page-properties: `IconImageSource`, `BackgroundImageSource`, `Padding`, `Title`, `MenuBarItems`
  - Page lifecycle-metoder: `OnAppearing`, `OnDisappearing`, `OnNavigatedTo`/`OnNavigatedFrom`, `OnSizeAllocated`, `BackButtonPressed`
  - De tre navigation paradigms: hierarchical (`NavigationPage`), flyout (`FlyoutPage`), tabbed (`TabbedPage`)
  - Shell som samlet beskrivelse af app'ens navigation hierarchy i XAML
  - Shell flyout: `FlyoutHeader`, `FlyoutItem`, `MenuItem`, `Shell.MenuItemTemplate`
  - Shell tabs: `TabBar`, `Tab`, `ShellContent`, hierarkisk tab-placering (bund vs. top)
  - Route-based navigation: `Route`-property, `Routing.RegisterRoute`, `Shell.Current.GoToAsync`
  - Overførsel af data via query parameters og navigation state (`Dictionary<string, object>`) med `QueryPropertyAttribute`
  - Case-projektet MauiStockTake: problemdomæne, sider og opsætning

---

## Introduktion

Indtil nu har alle eksempel-apps i bogen bestået af en enkelt page. Det fungerer fint for små apps og faktisk også for adskillige kommercielle apps, men så snart en app vokser, bliver det nødvendigt at dele den op i flere pages — enten for at adskille forskellige områder af app'en eller for logisk at gruppere funktionalitet. Kapitlet gennemgår de navigation paradigms, .NET MAUI understøtter, og hvordan man navigerer mellem pages.

Kapitlets fire hovedtemaer er: hvordan man bryder en app op i pages med `ContentPage`, hvordan man navigerer mellem pages, hvordan Shell forenkler organiseringen af pages, og hvordan man sender data med, når man navigerer.

## 7.1 ContentPage

`ContentPage` er den vigtigste page-type. De øvrige page-typer er reelt bare containere, der leverer forskellige navigation paradigms — altså forskellige måder at præsentere en `ContentPage` på.

`ContentPage` har én public property af typen `View`, som hedder `Content`. Det er dette indhold, page'en renderer på skærmen. I XAML tildeles `Content` ved at skrive XAML-elementet mellem åbnings- og lukketagget `<ContentPage>...</ContentPage>`. I C# tildeles det som enhver anden property.

### Listing 7.1 ContentPage.xaml

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MayApp.MyPage">
<!—Your page's content goes here -->
</ContentPage>
```

① I XAML tildeler man content til page'en ved at skrive det mellem `ContentPage`-taggene.

Man kan også eksplicit angive `Content`-propertyen med `<ContentPage.Content>... </ContentPage.Content>`-tags og lægge sine views ind i den. Det er dog ikke nødvendigt: en `View`, der tilføjes mellem `<ContentPage>`-taggene, bliver automatisk tildelt `Content`-propertyen.

### Listing 7.2 ContentPage.xaml.cs

```csharp
namespace MyApp;
public partial class MyPage : ContentPage
{
public MyPage()
{
InitializeComponent();
Content = new VerticalStackLayout();
}
}
```

① I C# kan man tildele en `View` direkte til `Content`-propertyen på en `ContentPage`.

Når man bygger en page i en .NET MAUI-app, vælger man én af de to tilgange — listing 7.1 og 7.2 ville ikke være fra samme app.

### UI i C#

Bogen fokuserer primært på XAML-tilgangen, men dykker ned i C# hvor det er nødvendigt for at deklarere eller manipulere UI. Er man interesseret i C#-deklareret UI, findes der dokumentation hos Microsoft samt en del community-ressourcer, herunder markup extensions i Community Toolkit, der giver et fluent API til at skrive UI i C#. Bogen anbefaler Gerald Versluis' video på YouTube (`https://www.youtube.com/watch?v=nCNh9G-Q688`) som udgangspunkt.

Alle layouts og controls i .NET MAUI nedarver fra base-klassen `View`, så de kan alle tildeles `Content`-propertyen på en `ContentPage`. Det er almindeligt at tildele enten et layout eller en `ScrollView` (kapitel 4).

Figur 7.1 illustrerer pointen: en `ContentPage` har en `Content`-property, som man kan tildele en `View` — hvilket vil sige ethvert layout eller control. I template-genererede sider er det en `ScrollView`, der tildeles `Content`; den wrapper en `VerticalStackLayout`, som indeholder alle controls. `ScrollView` sikrer, at indhold der ikke kan være på skærmen stadig er tilgængeligt, og `VerticalStackLayout` gør det muligt at tilføje flere controls til page'en.

Eftersom alle controls også nedarver fra `View`, kan man i princippet tildele f.eks. en enkelt `Button` direkte til page'ens `Content`.

### Listing 7.3 A page with a single control as its Content

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="AlohaWorld.MainPage">
<Button Text="Click me"
FontAttributes="Bold"
HorizontalOptions="Center" />
</ContentPage>
```

Figur 7.2 viser resultatet: en `ContentPage` med en enkelt `View` — her en `Button` — tildelt `Content` ved at placere den mellem `ContentPage`'s åbnings- og lukketags. Det er en gyldig, men ikke særligt fornuftig, tilgang. Man bør holde sig til at tildele enten et layout eller en `ScrollView` til sine pages' `Content`.

## 7.2 Common page properties

I .NET MAUI er en page en klasse, der nedarver base-klassen `Page` (eller, i tilfældet `TabbedPage`, en collection af `Page`s). `Page`-base-klassen har flere properties relateret til at rendere UI på skærmen samt en række lifecycle-metoder og events.

Base-klassen har desuden nogle properties, som arves af og kan være nyttige i de afledte page-typer. Afhængigt af hvilket navigation paradigm der bruges til at præsentere page'en, kan disse properties gøre noget forskelligt — eller i nogle tilfælde slet ingenting. Alle properties er bindable, så man kan enten tildele værdier direkte eller binde dem til properties af tilsvarende typer i en binding context.

> **NOTE** `IsBusy`-propertyen dækkes ikke, fordi den ikke virker pålideligt og ikke bør
> bruges. Det er trivielt at tilføje sin egen `ActivityIndicator`; gør det i stedet, som
> vi har set i MauiTodo og MauiMovies.

### 7.2.1 IconImageSource

`IconImageSource` lader dig angive et billede, der vises som page'ens ikon. Hvis page'en præsenteres af enten en `FlyoutPage` eller en `TabbedPage`, vises billedet som page'ens ikon i henholdsvis flyout-menuen eller tab bar'en.

### 7.2.2 Background Images

`BackgroundImageSource` lader dig angive et billede, der vises i baggrunden. Layouts er som standard transparente, så alle områder af page'en, der ikke er dækket af et control, viser baggrundsbilledet.

Image-properties kan angive, hvordan billedet skal vises (filled, tiled osv.). Til et tiled baggrundsbillede er `BackgroundImageSource` et godt valg. Men vil man fitte eller fylde billedet ud, giver `BackgroundImageSource` uforudsigelige resultater.

Vil man have et filled eller fitted baggrundsbillede, får man langt mere pålidelige resultater ved at bruge et `Grid` som parent layout for page'en, placere et `Image` i række og kolonne 0 og sætte den ønskede `Aspect`-property. Man behøver ikke engang at definere rækker eller kolonner eller angive placeringen: et `Grid` har som standard én række og én kolonne, og views i et `Grid` ligger som standard i række og kolonne 0.

Eksemplet tilføjer et baggrundsbillede (Hollywood-skiltet fra Pexels.com) til MauiMovies.

### Listing 7.4 MainPage.xaml with background image

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Name="MoviePage"
x:Class="MauiMovies.MainPage">
<Grid>
<Image Source="hollywood.jpg"
Aspect="AspectFill"
Opacity="0.4"/>

<VerticalStackLayout Spacing="10" Padding="30">
...
</VerticalStackLayout>
</Grid>
</ContentPage>
```

① Wrapper den eksisterende `VerticalStackLayout` i et `Grid`
② Tilføjer et `Image` (række og kolonne 0 er begge default) og sætter source til
baggrundsbilledet
③ Sætter `Aspect` til `AspectFill`, så billedet fylder skærmen eller vinduet ud, mens
aspect ratio bevares
④ Sætter opacity til 0.4, så billedet ikke dominerer UI'en

Figur 7.3 viser resultatet: et baggrundsbillede tilføjet ved at tildele både billedet og page'ens layout til et `Grid` med én række og én kolonne, som fylder hele page'en. Denne tilgang lader dig pålideligt fitte eller fylde billedet — noget der giver uforudsigelige resultater med `BackgroundImageSource`. Til at tile et baggrundsbillede er `BackgroundImageSource` derimod mere pålidelig.

### 7.2.3 Padding

`Page` har en `Padding`-property af typen `Thickness`. Alle views har en boundary, og `Padding` angiver, hvor tæt på den boundary elementer inde i viewet må komme.

`Padding` adskiller sig fra `Margin` ved, at `Padding` repræsenterer pladsen *inden for* et views boundary, mens `Margin` svarer til pladsen *uden for* boundary'en. Sætter man page'ens `Padding`, sikrer man et mellemrum mellem page'ens ydre boundary og alle child items, page'en viser.

Figur 7.4 illustrerer forskellen med to buttons på en page: pilen mellem dem repræsenterer `Margin` og angiver, hvor langt views er fra andre views; pilen der peger indad repræsenterer `Padding` og angiver, hvor tæt på et views boundary dets interne elementer må vises.

### 7.2.4 Title

`Page` har en `Title`-property af typen `string`. `Title` bruges til at vise, hvilken page i app'en der aktuelt er fremme, og vises i app'ens toolbar eller navigation bar.

Figur 7.5 viser MauiTodo-app'en, hvor `MainPage`'s `Title` er sat i XAML, og titlen optræder i navigation bar'en.

> **NOTE** I eksemplet i figur 7.5 er `MainPage` pakket ind i en `NavigationPage`. Laver
> man kun XAML-ændringen, ser man ikke det samme resultat. Hvordan man gør det, kommer
> senere i kapitlet.

I en single-page-app er app'ens og page'ens titler formentlig de samme, så det er ikke nødvendigvis nødvendigt at vise `Title`. På grund af begrænsningerne i, hvordan denne property vises i navigation bar'en, er det faktisk formentlig at foretrække at bruge en `Label` (eller en grafik) til at vise app'ens eller page'ens titel som en del af page'ens indhold — som gjort i FindMe og MauiTodo.

I en multi-page-app er det derimod en hurtig og velkendt måde for brugeren at se, hvor i app'en de befinder sig. Bruger man et af kapitlets navigation paradigms, vil `Title` automatisk blive vist i navigation bar'en.

### 7.2.5 MenuBarItems

`MenuBarItems` er en collection af typen `MenuBarItem` og bruges på desktop-platformene (Windows og macOS) til at vise en menu. Menuen følger hver platforms standard-paradigme: på Windows øverst i vinduet, på macOS øverst på skærmen. `MenuBarItems` renderes ikke på mobile target-platforme (iOS og Android).

En menu er en velkendt interaktionsmetode, der går tilbage til de tidligste grafiske brugergrænseflader. I en mobil-app skal UI'et forenkles for at tage højde for både mindre skærme og fingre, som er mere grovkornede input-devices end mus og pointer. Men i en desktop-applikation er en menu et godt valg — især i line-of-business-applikationer eller enhver app med et rigt udvalg af features, som ville være svære at proppe ind på én skærm som knapper. Menuer behandles i kapitel 10.

## 7.3 Common page lifecycle methods

`Page`-base-klassen leverer flere metoder, som kaldes af event handlers som reaktion på page lifecycle events. Lifecycle events er knyttet til page'ens livscyklus, i modsætning til et direkte svar på en ekstern handling som brugerinteraktion eller en push-notifikation. Mange af disse lifecycle events udspringer i sidste ende af en brugerinteraktion eller et eksternt stimulus — `OnAppearing` kaldes f.eks., når en page kommer frem, men en page kommer kun frem, fordi en bruger har navigeret til den. Metoden er dog knyttet til page'ens lifecycle event, ikke til brugerinteraktionen.

Fuld dækning af alle lifecycle-metoder findes i .NET MAUI-dokumentationen (`http://mng.bz/e1GP`).

### 7.3.1 OnAppearing

`OnAppearing` kaldes, når en page kommer frem. Metoden er vigtig, fordi det er her, man skal udføre sin page-initialiseringslogik. Ligesom enhver anden klasse har `Page` og dens afledte typer constructors, og det kan være fristende at lægge initialiseringslogik der. Men det er ikke det, constructors er til: i en .NET MAUI `Page` bør constructors kun bruges til at sætte initielle værdier på fields.

Alt andet, der skal ske når page'en kommer frem, bør kaldes fra `OnAppearing`. Et typisk use case er indlæsning af data, hvilket normalt er en asynkron operation — data hentes enten fra en database eller et web-API — og man kan ikke have en asynkron constructor.

I MauiTodo skrev vi vores egen `Initialize`-metode til dette, men logikken er en ideel kandidat til `OnAppearing`. Åbn MauiTodo-app'en, rediger `MainPage.xaml.cs`, override `OnAppearing` og flyt logikken fra `Initialize` derind. Derefter kan `Initialize` og kaldet til den slettes. Behold kaldet til `base.OnAppearing()`, og gør metoden `async`.

### Listing 7.5 The OnAppearing method in MainPage.xaml.cs

```csharp
protected override async void OnAppearing()
{
base.OnAppearing();

var todos = await _database.GetTodos();

foreach (var todo in todos)
{
Todos.Add(todo);
}
}
```

① `OnAppearing` kommer fra base-klassen, så vi overrider den i vores klasse for at få
adgang til lifecycle-eventet.
② Kalder base-metoden, da vi stadig vil have base-klassens lifecycle-event-logik udført
③ Den resterende kode er kopieret fra `Initialize`-metoden.

Ikke vist i listing 7.5 er fjernelsen af `Initialize`-metoden og kaldet til den i constructoren (`_ = Initialize()`) — husk at slette dem. Kører man MauiTodo nu, indlæses to-do-elementerne stadig fra databasen, når page'en kommer frem.

### 7.3.2 OnDisappearing

`OnDisappearing` kaldes, når page'en forsvinder. Den er nyttig til pages, der forvalter en form for state, til at unsubscribe fra events og messages, og til at rydde op i ressourcer, der ikke længere er nødvendige, når page'en ikke er synlig.

### 7.3.3 OnNavigatedTo and OnNavigatedFrom

`OnNavigatedTo` og `OnNavigatedFrom` udfører i praksis samme funktion som henholdsvis `OnAppearing` og `OnDisappearing`, men med én afgørende forskel: `OnNavigatedTo` og `OnNavigatedFrom` kaldes *før* page'en kommer frem eller forsvinder, hvor `OnAppearing` og `OnDisappearing` kaldes *efter*.

`OnAppearing` er derfor et godt valg til at indlæse data: når page'en er synlig, kan man vise en `ActivityIndicator` (eller en egen loading-indikator) og signalere til brugeren, at noget sker i baggrunden. Udførte man samme handling i `OnNavigatedTo`, ville app'en se ud til at gå i stå, mens den ventede på, at data blev indlæst.

`OnAppearing` og `OnDisappearing` passer bedst til de fleste use cases. Men i scenarier hvor man skal udføre en hurtig, synkron operation før en page kommer frem eller forsvinder, er `OnNavigatedTo` og `OnNavigatedFrom` det bedre valg.

### 7.3.4 OnSizeAllocated

`OnSizeAllocated` kaldes, hver gang page'ens størrelse ændres. Det gælder både når page'en først indlæses, når orienteringen skifter, eller når et vindue ændrer størrelse. Metoden tager to parametre af typen `double` for bredde og højde.

Metoden er nyttig til at bygge responsivt UI og til at ændre størrelse eller position på elementer som reaktion på ændringer i page-størrelsen. I forrige kapitel så vi, hvordan `AbsoluteLayout` kan bruge proportional positionering til at placere en floating action button (FAB) ni tiendedele nede og henover skærmen eller vinduet. `OnSizeAllocated` lader dig skrive mere sofistikerede regler for placering og størrelse af views — f.eks. at ændre både størrelse og margin på FAB'en ved bestemte breakpoints. Det adskiller sig fra proportional størrelse og positionering ved, at man kan skrive logik, der ud fra en række skærm- eller vinduesstørrelser bestemmer, hvilken af et sæt faste størrelser og positioner der skal anvendes. Et eksempel findes i kapitlets kodemappe.

### 7.3.5 BackButtonPressed

`BackButtonPressed` kaldes, når enten back-knappen i .NET MAUI's navigation bar bruges, eller — på Android — når Android OS' back-knap bruges. Den er nyttig, hvis man vil køre kode, når eventet indtræffer.

> **NOTE** `BackButtonPressed` bør bruges til at *supplere*, ikke *ændre*, back-knappens
> opførsel. Vil man ændre back-knappens opførsel for Android-brugere, skal man override
> `OnBackPressed` i `MainActivity`-klassen i Android-platform-mappen.

I modsætning til de øvrige lifecycle-metoder, som alle er `void`, har `BackButtonPressed` returtypen `bool`. Men da metoden kaldes af en event handler, er returtypen ligegyldig. Man kan f.eks. ikke returnere `false` i stedet for at returnere et kald til base-metoden (hvilket ville være default-opførslen) for at annullere navigation.

## 7.4 Navigation in .NET MAUI

Nogle apps fungerer fint med en enkelt page, men de bliver typisk mere interessante, når de rummer nok funktionalitet til at retfærdiggøre flere pages. Resten af bogen bygger MauiStockTake, en mere kompleks app, som har brug for flere pages og dermed navigation.

.NET MAUI understøtter tre hovedparadigmer for navigation: **hierarchical navigation**,
**tabbed navigation** og **flyout navigation**. Hvert paradigme understøttes af en subclass
af `Page`, konfigureret specifikt til formålet. Derudover har .NET MAUI **Shell**, som understøtter både tabbed og flyout navigation samt route-based navigation. Route-based navigation er især nyttig, hvis man vil understøtte deep linking — altså lade host-OS'et navigere til bestemte dele af app'en via en unik URL.

### 7.4.1 Scenario: The MauiStockTake app

Mildred ejer Mildred's Surf Shack, en populær surfbutik nær stranden. Hun sælger surfbrætter og surfudstyr og udlejer surfbrætter til turister. Hun bruger sine salgstal hver uge til at bestille hos leverandører, så hun altid har nok af forbrugsvarer som voks. Én gang i kvartalet tæller hun sit lager op for at sikre, at formlen fungerer.

Hver kvartal beder Mildred to medarbejdere om at komme ind om natten og hjælpe med optællingen. Hver af dem bruger et clipboard, skriver sit navn øverst og noterer, hvad de ser. Processen virker, men har problemer. For det første noterer folk lagerbeholdning inkonsistent, hvilket forværrer problem nummer to: det er svært og tidskrævende for Mildred at afstemme sine og medarbejdernes noter til sidst. Endelig tager processen hele natten, og medarbejderne bryder sig lige så lidt om at arbejde natten igennem, som Mildred bryder sig om at betale overtid.

Du ejer Beach Bytes, et app-udviklingsfirma beliggende lige ved siden af butikken. Mildred tror, du kan hjælpe hende med en app, der gør optællingsprocessen mere effektiv.

### 7.4.2 Features of the MauiStockTake app

MauiStockTake handler om tre hovedområder (tabel 7.1):

| Problemområde | Beskrivelse |
| --- | --- |
| **Products** | Medarbejdere noterer produktnavne inkonsistent og tager nogle gange fejl. For at undgå det vil Mildred have, at app'en slår produkter op i hendes katalog frem for at lade medarbejderne indtaste detaljer vilkårligt. Hun vil også have rapportering på producenter, så hun kan se, hvor hun bruger flest penge på lager, og forhåbentlig forhandle mængderabatter. |
| **Inventory** | Registrering af lagerbeholdning er app'ens kerneadfærd. Mildred skal kunne tælle op, hvor mange hun har af hvert produkt. |
| **Staff** | Mildred vil dele arbejdet op i tre områder og tildele hvert område til et teammedlem (inklusive sig selv). Hun har brug for at vide, hvem der registrerer hvad, for at gøre fejlfinding lettere. Ved store mængder vil hun måske rotere teamet, så hvert område tælles af mindst to personer for at fange fejl tidligt. |

Hvert af disse områder får en page i app'en. Figur 7.6 viser en mock-up af product-page'en: den indeholder et søgefelt, hvor resultaterne udfylder en dropdown, en stepper til at angive hvor mange enheder der blev talt, og en Add-knap til at føje optællingen til stocktake'et. Product-page'en bliver app'ens hjerte: et lookup-felt, hvor medarbejderne kan indtaste et produktnavn og vælge det rette resultat fra listen — vi ved altså, at vi har brug for en product lookup-feature.

Figur 7.7 viser en mock-up af en login-page. Mildred vil kunne identificere, hvem der har indtastet hver lageroptælling, så der skal være en måde for medarbejderne at logge ind på — eller i det mindste indtaste deres navn. Det betyder, at der skal være en user service.

Endelig skal resultaterne af optællingen kunne gennemses på hver enhed og synkroniseres til en upstream-service, så de kan konsolideres i én samlet rapport. Til begge dele skal der være en inventory service, som aggregerer optællingerne, viser resultaterne for brugeren og synkroniserer dem opstrøms. Figur 7.8 viser en mock-up af en reports-page til at se de aggregerede optællinger.

For at rumme de mange pages skal der vælges et navigation paradigm.

### 7.4.3 Hierarchical navigation

I .NET MAUI bruges `NavigationPage` til hierarchical navigation ved at vise `ContentPage`s i en **navigation stack**. "Stack" har her sin standardbetydning: elementer kan pushes (tilføjes) på stakken og poppes (fjernes) fra den. I en `NavigationPage` er elementerne pages.

Figur 7.9 illustrerer det: i hierarchical navigation kan en page pushes på stakken (til venstre) eller poppes fra stakken (til højre) efter et last-in, first-out-mønster.

Den senest pushede (last-in) page på stakken er den, brugeren ser på skærmen. Enhver klasse afledt af `Page` kan pushes på en navigation stack, inklusive alle kapitlets page-typer. Man kan altså kombinere navigation paradigms, men det kan forvirre brugerne, så pas på med ukendte kombinationer.

Hierarchical navigation er et godt valg, hvis app'en skal starte med en menu-skærm. MauiStockTake kunne bruge denne tilgang ved at starte med en menu-page med knapper til input-page og report-page. Et tryk på en knap ville pushe den relevante page på stakken. Med `NavigationPage` vises der en navigation bar øverst med en back-knap, som popper den aktuelle page og fører dig ét skridt tilbage i stakken.

Figur 7.10 viser, hvordan hierarchical navigation kunne fungere i MauiStockTake: app'en starter med en menu-page med to knapper svarende til de tilgængelige pages. Et tryk pusher den tilsvarende page på navigation stack'en; et tryk på back-knappen (chevron'en øverst til venstre) popper page'en og fører dig tilbage til den forrige.

Hierarchical navigation adskiller sig fra de øvrige paradigmer på ét afgørende punkt: i de andre paradigmer er pages bundet til et UI-control som en tab eller et flyout item, og et tryk/klik får frameworket til automatisk at vise page'en. I hierarchical navigation pusher man derimod pages på navigation stack'en **programmatisk**.

I figur 7.10 bruges en `Button` til at navigere til en page. I kode ville vi bruge `PushAsync`-metoden på `Navigation`-klassen og sende en instans af den ønskede page-type med som parameter:

```csharp
await Navigation.PushAsync(new InputPage());
```

For at poppe page'en fra stakken og navigere tilbage til den forrige page bruger vi `PopAsync`:

```csharp
await Navigation.PopAsync();
```

Bogen bruger ikke hierarchical navigation, men det er meget udbredt og formentlig det, du vil bruge, hvis din app har en menu-page eller landing page.

### 7.4.4 Flyout navigation

I flyout navigation glider (eller "flyver") en menu ud fra siden af skærmen og præsenterer en liste over pages, der kan vises i hovedområdet. Brugeren vælger en page, som så optager hovedområdet, mens menuen glider tilbage. Figur 7.11 viser paradigmet anvendt på MauiStockTake: i en `FlyoutPage` viser en menu (flyout) en liste over pages, og den valgte page vises på skærmen.

I .NET MAUI understøttes flyout navigation af `FlyoutPage`. `FlyoutPage` har to child-properties af typen `Page`: `Flyout`, som bruges til menuen, og `Detail`, som den valgte page tildeles og vises i.

Man kan bruge en `ContentPage` til at bygge en fuldt tilpasset flyout-menu og tildele den til `Flyout`-propertyen. `ContentPage` giver stor fleksibilitet, men man bør være varsom med at introducere ukendte UX-paradigmer for sine brugere.

Inde i sin `FlyoutPage` skal man levere en collection af typen `FlyoutPageItem`, som brugeren kan vælge fra. `FlyoutPageItem` har en property `TargetType`, som man tildeler den ønskede page. Når brugeren vælger et `FlyoutPageItem`, tildeles det valgte items `TargetType` til `FlyoutPage`'s `Detail`-property, hvorved den vises i app'ens hovedområde.

### 7.4.5 Tabbed navigation

I tabbed navigation bruges tabs til at give adgang til app'ens forskellige pages. En `TabBar` vises med en collection af mærkede ikoner, hvert repræsenterende en page, brugeren kan vælge. Den valgte page vises i hovedområdet. Som standard vises `TabBar` nederst på skærmen på iOS. På Windows og Android vises den øverst, hvilket minder mere om de tabs, man kender fra webbrowsere.

Figur 7.12 viser paradigmet i MauiStockTake: en tab bar nederst på skærmen med en collection af mærkede ikoner, hvert repræsenterende en page. Brugeren trykker på et ikon, og den page, det repræsenterer, vises på skærmen.

"Tab" er en skeuomorf metafor, der ikke altid holder (ligesom floppy disk-ikonet) — det er sjældent at se egentlige faner. De fleste UI-designs foretrækker nu simple ikoner vist i en bar i bunden, i stil med iOS' default.

I .NET MAUI muliggør `TabbedPage` tabbed navigation. I modsætning til `ContentPage`, som har en enkelt child via `Content`, tillader `TabbedPage` flere children, som hver skal nedarve fra `Page`. Disse pages vises automatisk som tabs efter platformens default-tilgang, med ikon og tekst taget fra page'ens `Icon`- og `Title`-properties.

## 7.5 Introducing Shell

I .NET MAUI-apps er **Shell** en enkel måde at beskrive app'ens page navigation hierarchy i XAML. Den giver en simpel måde at strukturere hele app'en på ved hjælp af flyouts og tabs.

Er app'ens design baseret på flyout navigation eller tabbed navigation (eller begge), kan Shell være et bedre valg end de page-typer, der understøtter paradigmerne. Shell giver tre hovedfordele:

- **Hele app'en beskrevet i én fil** — Shell-filen er et one-stop shop, hvor man kan se,
hvordan hele app'en er organiseret navigationsmæssigt. Det er let at se, hvordan app'ens pages hænger sammen, og lige så let at rokere rundt på dem.
- **Dependency resolution** — .NET MAUI bruger generic host builder-mønstret, som
inkluderer .NET's indbyggede DI-container. Med Shell bliver alle dependencies, der constructor-injiceres i dine pages, automatisk resolvet — præcis som controller- dependencies i en ASP.NET Core-app.
- **Route-based navigation** — Med Shell kan hver page nås via en URL, hvilket giver to
fede features. For det første forenkler det deep linking, så app'en kan åbnes eksternt direkte på en bestemt page (særlig nyttigt til push-notifikationer). For det andet kan man bruge query parameters, altså sende data til sine pages som del af URL'en.

### Komplekse query parameters i .NET MAUI

Shell blev oprindeligt introduceret i Xamarin.Forms version 4 og indeholdt query parameters. I Xamarin.Forms var Shell query parameters begrænset til primitive typer — man kunne let sende en `string`, `int` eller `bool`, men ikke et mere komplekst objekt (medmindre man brugte et workaround som at serialisere til JSON). Ville man route fra f.eks. en produktlisteside til en produktdetaljeside, måtte man sende produktets `id`, og detaljesiden (eller dens ViewModel) var ansvarlig for at slå detaljerne op.

Med .NET MAUI lader Shell dig sende objekter direkte som parameter, så man i eksemplet ovenfor kan route til produktdetaljesiden og sende hele produktet med.

Shell lader dig kombinere flyout- og tabbed navigation-paradigmerne på den måde, der passer app'en bedst.

### 7.5.1 The flyout menu

Shell leverer en flyout-menu, der kan bruges til navigation samt til at give adgang til features eller funktionalitet, man forventer at kunne nå overalt i app'en. Den har en tilpasselig header og footer og lader dig tilføje flyout items til navigation og menu items til at eksekvere funktionalitet.

Figur 7.13 opsummerer strukturen: flyout-menuen i Shell består af en tilpasselig header og footer, flyout items til at navigere i app'en, og menu items til at eksekvere kode.

Headeren er let at tilpasse. Man kan skrive XAML-UI direkte i `AppShell.xaml`-filen, eller man kan bruge en control template. Det samme gælder footeren, som også kan bruge control templates eller importerede views.

#### Menu items

Flyout-menuen i Shell understøtter både menu items og flyout items, så man kan tildele funktionalitet til items i flyouten i stedet for udelukkende at lade dem navigere til en page i app'en (tænk f.eks. på en logout-knap).

Figur 7.14 viser en logout-knap tilføjet til Shell-flyout-menuen. Det er et menu item, fordi det bruges til at eksekvere kode (logikken der logger brugeren ud) frem for at navigere til en page. `MenuItem`-typen har både en `Clicked` event handler og en `Command`, og begge kan bruges til at kalde den kode, der skal køre, når brugeren trykker eller klikker på menu item'et. Figuren viser samtidig, hvordan et `FlyoutItem` i Shell er konfigureret til at vise en sektion af app'en: sektionen indeholder to tabs — en Home-tab og en Settings-tab — som brugeren kan skifte imellem inden for denne Shell-sektion. Åbner man menuen, afsløres andre flyout items, som giver adgang til andre Shell-sektioner.

#### Flyout Items

Flyout items giver adgang til pages eller grupper af pages i app'en. `FlyoutItem`-typen tillader flere children, så man kan tilføje enten en enkelt page eller en collection af pages.

Med Shell inddeles app'en i **Shell Sections**. Hver Shell Section kan indeholde en eller flere pages eller collections af pages organiseret i tabs.

### 7.5.2 Tabs

Tabs organiseres hierarkisk i Shell. I modsætning til `TabbedPage` vil de tabs, der ligger øverst i hierarkiet (dem man tilføjer først), i Shell **altid** blive vist nederst på skærmen, uanset platform. Pages kan indlejres i en tab, som så selv bliver tabbet individuelt; tabs på dette niveau i hierarkiet vises **øverst** på skærmen.

Figur 7.15 illustrerer det: Flyout Item 2 navigerer til en Shell Section med to tabs, tilgængelige via et Balloons-ikon og et Cake-ikon, begge vist nederst på skærmen. Inde i Cake-tab'en ligger tre yderligere tabs — Birthday, Wedding og Christmas — som giver adgang til indholdet et niveau længere nede og vises øverst på skærmen.

For store og komplekse applikationer er denne evne til at partitionere app'en i sektioner med Shell et stærkt værktøj til at organisere indhold logisk, samtidig med at brugerne let kan navigere til de dele, de har brug for.

### 7.5.3 Getting started with MauiStockTake

Nu bygges det store projekt. MauiStockTake fortsætter gennem resten af bogen, efterhånden som services, arkitektur og designmønstre gennemgås. Dette kapitel handler om navigation og brugen af Shell til at bygge app'ens struktur.

#### Downloading the Starter Project

MauiStockTake har brug for et backend-API til at slå produkter op og til at sende lageroptællinger til. Hos Beach Bytes står en kollega for den del, så du kan koncentrere dig om client-app'en. En prototype af MauiStockTake-API'et kan hentes fra kapitlets chapter-start-mappe.

Løsningen er baseret på Jason Taylors Clean Architecture (CA)-template (`https://tinyurl.com/2p9ceh4x`). .NET MAUI integrerer med enhver arkitektur, man ønsker i sin .NET-løsning; forfatterens tilgang til .NET MAUI med CA gennemgås i hans talk (`https://www.youtube.com/live/K9ryHflmQJE`). Man kan også bare ignorere det og køre løsningen. Når løsningen kører, kan man gå videre til at tilføje .NET MAUI-projektet.

#### Adding the .NET MAUI App

Indtil nu har bogen brugt `blankmaui`-templaten, fordi default-templaten har Shell indbygget. Man kan godt arbejde med den uden at bruge Shell, men det er lettere at lære nogle fundamentale .NET MAUI-principper, før man blander Shell ind i det.

##### Skal jeg bruge Shell til alle apps?

Shell passer godt til apps, der bruger flyout, tabbed navigation eller begge dele, men i nogle scenarier er det ikke det bedste valg. Line-of-business desktop-applikationer har typisk ikke denne navigationsstil, som traditionelt hører til web eller mobil. I apps med kun en enkelt page — som dem bogen har bygget indtil nu — eller apps med hierarchical navigation er Shell unødvendigt. Endelig lader Shell dig tilpasse flyouten i vid udstrækning, men **du kan slet ikke tilpasse tab bar'en**, så hvis dit visuelle design ikke kan rummes inden for Shells rammer, skal du vælge en anden løsning. For mange apps, inklusive MauiStockTake, er Shell dog perfekt.

MauiStockTake bruger både tabs og en flyout, så her bruges default-templaten. Tilføj et nyt .NET MAUI-projekt til MauiStockTake-løsningen og kald det `MauiStockTake.UI`. Default-templaten hedder bare ".NET MAUI App" (figur 7.16: default-templaten inkluderer Shell out of the box; `blankmaui`-templaten er præcis den samme, bare uden Shell).

> **NOTE** Visual Studio-projektmapper er ikke mappet 1:1 til filsystemets mapper. Du skal
> eksplicit ændre placeringen, så projektet ligger i `Presentation`-mappen på filsystemet.

Templaten skaber en .NET MAUI-app, der på dette tidspunkt er næsten identisk med Aloha, World-projektet, men med nogle vigtige forskelle:

Den første forskel er, at projektet indeholder `AppShell.xaml` og `AppShell.xaml.cs`. Det er Shell-filerne, der definerer app'ens struktur og hierarki.

Den næste forskel er, hvordan app'en bootstrapper sin main page. I Aloha, World og alle tidligere projekter blev `MainPage`-propertyen i `App.xaml.cs` tildelt app'ens main page. I `MauiStockTake.UI` tildeles i stedet en ny instans af `AppShell`-klassen.

Åbner man `AppShell.xaml.cs`, ser man, at rod-noden er af typen `Shell` og har en række attributter, man kender fra `ContentPage`. Den har ét child-element af typen `ShellContent`, som har tre properties tildelt:

- **`Title`** — sætter page-titlen. Titlen vises i navigation bar'en, når page'en er aktiv,
og i tab bar'en, når page'en er tilgængelig som en tab.
- **`ContentTemplate`** — det faktiske indhold, der skal vises. Det er af typen
`DataTemplate`, så man kunne bygge indhold direkte i `AppShell.xaml` og tildele det. Det er dog mere effektivt at tildele en `ContentPage`, som det er gjort her, fordi page'en først indlæses, når der navigeres til den. Når man begynder at regne en pages dependencies med, som i næste kapitel, kan det løbe op i en del overhead. I templaten er `MainPage`-typen tildelt fra `local`-namespacet, og af Shell-attributterne fremgår det, at `MauiStockTake.UI`-rod-namespacet er tilføjet som `local` XML-namespace, hvor `MainPage`-klassen ligger.
- **`Route`** — dette `ShellContent` har fået tildelt routen `MainPage`. Det betyder, at vi
ved Shell-navigation let kan komme til denne page via routen.

`MainPage.xaml` bruges ikke i app'en, så dette item i Shell skal erstattes med de pages, vi vil bruge. Vi kender allerede de nødvendige pages, selvom det visuelle og funktionelle design ikke er færdigt — det er nok til at komme i gang med Shell.

Sidste opsætningsskridt: C# 10 introducerede **global using**-statements, som gør det muligt at markere et `using`-statement i en vilkårlig fil som `global`, hvorved det namespace, det importerer, bliver tilgængeligt i alle andre filer i samme projekt. Det bruges i .NET MAUI-projekter til at reducere boilerplate for namespaces, man refererer gentagne gange.

Tilføj en fil kaldet `GlobalUsings.cs` i projektets rod. Er der templated kode i den, så slet den. Filen holdes tom indtil videre, og namespaces tilføjes undervejs.

#### Adding the InputPage and ReportPage

App'en har to funktionelle pages (input og reports) samt en login-page, der kun bruges, første gang brugeren starter app'en. En to-siders app egner sig godt til en tab bar, men der tilføjes også en flyout-menu, som kan vise information om app'en og brugeren samt en logout-knap.

Opret en mappe i `MauiStockTake.UI` kaldet `Pages`. Tilføj en .NET MAUI `ContentPage` (XAML) kaldet `InputPage`, en anden kaldet `LoginPage` og en tredje kaldet `ReportPage`.

Fjern alt indhold fra hver page. I `InputPage` og `ReportPage` tilføjes en `VerticalStackLayout` med en enkelt `Label`, hvis `Text` sættes til henholdsvis "Input Page" og "Report Page". Det samme i `LoginPage`, men med en knap under `Label`'en, hvis `Text` er "Login".

##### Listing 7.6 InputPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.Pages.InputPage"
Title="InputPage">
<VerticalStackLayout>
<Label Text="Input Page"
VerticalOptions="CenterAndExpand"
HorizontalOptions="CenterAndExpand" />
</VerticalStackLayout>
</ContentPage>
```

##### Listing 7.7 LoginPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.Pages.LoginPage"
Title="LoginPage">
<VerticalStackLayout>
<Label Text="Login Page"
VerticalOptions="CenterAndExpand"
HorizontalOptions="CenterAndExpand" />
<Button Text="Login"
HorizontalOptions="Center"
VerticalOptions="Center" />
</VerticalStackLayout>
</ContentPage>
```

##### Listing 7.8 ReportPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.ReportPage"
Title="ReportPage">
<VerticalStackLayout>
<Label Text="Report Page"
VerticalOptions="CenterAndExpand"
HorizontalOptions="CenterAndExpand" />
</VerticalStackLayout>
</ContentPage>
```

Da der refereres til disse pages flere steder i app'en, tilføjes et global using-statement for namespacet i `GlobalUsings.cs`:

```csharp
global using MauiStockTake.UI.Pages;
```

#### Importing Required Assets

Der skal bruges et ikon til hver page i tab bar'en samt nogle menu item-ikoner til flyouten og et header-billede. I bogens online-ressourcer til kapitlet ligger fem billeder:

- `icon_input.svg`
- `icon_login.svg`
- `icon_logout.svg`
- `icon_report.svg`
- `surfshack_logo.jpeg`

Hent dem og kopiér dem ind i `Resources/Images`-mappen i `MauiStockTake.UI`-projektet.

#### Adding the TabBar

Før tabs tilføjes, ryddes boilerplate-koden væk. Åbn `AppShell.xaml` og slet det eksisterende `ShellContent`:

```xml
<ShellContent
Title="Home"
ContentTemplate="{DataTemplate local:MainPage}"
Route="MainPage" />
```

`MainPage` bruges ikke i app'en, så `MainPage.xaml` og `MainPage.xaml.cs` kan også slettes. Med et rent udgangspunkt tilføjes tab bar'en:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell ...>
<TabBar>
</TabBar>
</Shell>
```

Tab bar'en skal vise tabs for `InputPage` og `ReportPage`, så der tilføjes en `Tab` for hver, og hver `Tab` får en `Title` og et `Icon`.

Husk fra afsnit 7.5.2, at en `Tab` kan have flere `ShellContent`-children, hvilket ville vise top-tabs inden i bund-tab'en for hver page. Det gøres ikke her — der tilføjes ét `ShellContent`-child pr. `Tab`, og en page tildeles `ContentTemplate`-propertyen. Til det skal der bringes et XML-namespace ind, som repræsenterer `Pages`-namespacet i app'en.

##### Listing 7.9 AppShell.xaml

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
x:Class="MauiStockTake.Maui.AppShell"
xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
xmlns:pages="clr-namespace:MauiStockTake.Maui.Pages"
xmlns:local="clr-namespace:MauiStockTake.Maui">
<TabBar>
<Tab Title="Input"
Icon="icon_input.svg">
<ShellContent ContentTemplate="{DataTemplate pages:InputPage}" />
</Tab>
<Tab Title="Reports"
Icon="icon_report.svg">
<ShellContent ContentTemplate="{DataTemplate pages:ReportPage}" />
</Tab>
</TabBar>
</Shell>
```

① Tilføjer en `Tab` og sætter `Title` til en beskrivende titel for page'en
② Sætter `Icon` til den relevante grafikfil, vi importerede
③ Tilføjer et `ShellContent`, sætter `ContentTemplate` via `DataTemplate`-markup extension
og tildeler en af app'ens pages

På dette tidspunkt er app'ens overordnede visuelle struktur defineret. Start app'en og bekræft, at den kører. Figur 7.17 viser MauiStockTake-shell'en kørende på Android: på dette stadie gør `InputPage` og `ReportPage` ikke andet end at vise en label med deres navn, men man kan skifte mellem dem via tabs og dermed verificere, at pages oprettes korrekt, og at routes er registreret.

Verificér, at ikonerne vises, og at man kan skifte mellem de to pages. Label og titel skal ændre sig afhængigt af, hvilken page man er på. Slet derefter `MainPage.xaml` og `MainPage.xaml.cs`.

#### Adding the Flyout

Med en `TabBar` kan brugeren navigere mellem app'ens to hovedsider, når de er logget ind. Men vi vil også have en `Flyout` — ikke til navigation, men til at logge ud af app'en og vise en header med information om app'en.

Først aktiveres flyouten ved at sætte Shell'ens `FlyoutBehavior`-property til `Flyout`:

```xml
<Shell
x:Class="MauiStockTake.Maui.AppShell"
...
FlyoutBehavior="Flyout">
```

Dernæst tilføjes et `MenuItem` til Shell'en med `Text` sat til "Logout" og `IconImageSource` sat til filnavnet på det importerede logout-ikon. Tilføj dette før det lukkende `</Shell>`-tag:

```xml
<MenuItem Text="Logout"
IconImageSource="icon_logout.png" />
```

`MenuItem` har både et event og en `Command`, man kunne koble op, men funktionaliteten er ikke nødvendig endnu. Kører man app'en nu, ser `MenuItem`'et lidt uslebent ud, så `Shell.MenuItemTemplate` angives for at justere layoutet. Inde i tagget kan man angive en `DataTemplate` og tilføje sit layout — samme proces som ved collections. Hvert `MenuItem` har en `Icon`- og en `Text`-property, som der kan bindes til i layoutet.

##### Listing 7.10 Shell.MenuItemTemplate

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell ...>

<Shell.MenuItemTemplate>
<DataTemplate>
<Grid ColumnDefinitions="0.2*,0.8*">
<Image Source="{Binding Icon}"
Margin="35,0,0,0"
HeightRequest="45" />
<Label Grid.Column="1"
Text="{Binding Text}"
Margin="10,0,0,0"
VerticalTextAlignment="Center" />
</Grid>
</DataTemplate>
</Shell.MenuItemTemplate>
...
</Shell>
```

Det er ikke det mest sofistikerede layout — det ligner faktisk default-layoutet, blot lidt mere forfinet. Man kan være så kreativ, man vil, og selvom app'en kun har ét `MenuItem`, vil templaten gælde for alle, hvis der er flere.

Før app'en køres, tilføjes flyout-headeren. Man kan tildele et vilkårligt layout til `Shell.FlyoutHeader`. Her bruges et `Grid` til at placere controls, med et `Image` med app'ens logo klippet cirkulært og en `Label` nedenunder med app'ens titel.

##### Listing 7.11 The Shell Flyout header

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell ...>
<Shell.FlyoutHeader>
<Grid RowDefinitions="20,*,*"
Padding="20">
<Image Grid.Row="1"
Source="surfshack_logo.jpeg"
WidthRequest="100"
HeightRequest="100"
HorizontalOptions="Center"
VerticalOptions="Center">
<Image.Clip>
<EllipseGeometry Center="50,50"
RadiusX="50"
RadiusY="50"/>
</Image.Clip>
</Image>
<Label Grid.Row="2"
HorizontalOptions="Center"
VerticalOptions="Center"
Text="MauiStockTake"/>
</Grid>
</Shell.FlyoutHeader>
...
</Shell>
```

Kører man app'en nu, ses hamburger-menuen i øverste venstre hjørne. Trykker eller klikker man på den, flyver flyouten ud. Figur 7.18 viser resultatet: øverst ses `Image`'et, klippet med `EllipseGeometry`, og nedenunder `Label`'en med app'ens titel — de to udgør tilsammen flyout-headeren. Under dem ses Logout-menu item'et, som bruger det importerede ikon og er styled via `Shell.MenuTemplate`.

### 7.5.4 Routes and navigation

App'en har nu to pages, som let kan nås via tab bar'en. Med Shell vil de fleste pages blive tilgået på den måde — enten via en `Tab` eller et flyout item. Selvom forenklet navigation er default-adfærden med Shell (og hovedårsagen til, at Shell blev introduceret), er det stadig muligt at navigere programmatisk, hvis man har brug for det.

Til programmatisk navigation med Shell bruges `GoToAsync`:

```csharp
await Shell.Current.GoToAsync("myroute");
```

Med Shell bruger man `GoToAsync` med en **route** frem for en instans af en page (som i hierarchical navigation). For at kunne bruge routen skal den være registreret, hvilket er let med `Tab` eller `FlyoutItem`, da begge har en `Route`-property. Vi opretter routes for de to pages, vi har indtil videre:

```xml
<TabBar>
<Tab Title="Input"
Icon="icon_input.svg"
Route="input">
<ShellContent ContentTemplate="{DataTemplate Pages:InputPage}" />
</Tab>
<Tab Title="Reports"
Icon="icon_report.svg"
Route="reports">
<ShellContent ContentTemplate="{DataTemplate Pages:ReportPage}" />
</Tab>
</TabBar>
```

De pages, vi har registreret routes for indtil nu, er let tilgængelige via tab bar'en, så routes er ikke særligt nyttige her. Men vi har en anden page, som hverken kan nås via tabs eller flyout: login-page'en.

Da der ikke er en tab eller et flyout item til `LoginPage`, skal `GoToAsync` bruges for at nå den. Og vi skal kunne registrere en route uden at angive `Route`-propertyen på en `Tab` eller et `FlyoutItem`. Det gøres med `RegisterRoute`-metoden på `Routing`-klassen. Den letteste anvendelse er at sende to parametre: først en `string` til routen, dernæst typen af den page, routen skal repræsentere.

Åbn `AppShell.xaml.cs` og registrér en route for `LoginPage` i constructoren.

##### Listing 7.12 The updated AppShell.xaml.cs constructor

```csharp
public AppShell()
{
InitializeComponent();
Routing.RegisterRoute("login", typeof(LoginPage));
}
```

Med routen registreret kan vi let navigere til `LoginPage` med `GoToAsync`, selvom den hverken har en tilsvarende `Tab` eller et `FlyoutItem`. `LoginPage` tages op igen i kapitel 8 om authentication.

### 7.5.5 Route parameters

Routing i Shell minder om web'en ved, at man kan sende query parameters med. I stedet for blot at bruge `GoToAsync` med en route kan man også sende værdier med.

Figur 7.19 viser `Navigation.GoToAsync`-metoden med en route og en parameter angivet. I eksemplet bruges string interpolation til at sende værdien af en string som parameter til `mypage`. Man er ikke tvunget til at bruge string interpolation — man kunne have hardcodet værdien i URL'en. Man er heller ikke begrænset til strings: man kan også sende
**navigation state**, som er en dictionary af `<string, object>`, og bruge nøglen i den
modtagende page (eller page'ens binding context) til at hente den tilsvarende værdi.

For at se det i praksis tilføjes en produktdetaljeside til app'en. Først tilføjes en klasse, der repræsenterer produkter. I `MauiStockTake.UI` tilføjes en `Models`-mappe og heri en ny fil `Product.cs` med en klasse `Product` med fire properties:

- `id` af typen `int`
- `Name` af typen `string`
- `ManufacturerId` af typen `int`
- `ManufacturerName` af typen `string`

Tilføj dernæst en XAML `ContentPage` kaldet `ProductPage` til `Pages`-mappen. Registrér så en route for den i constructoren i `AppShell.xaml.cs`, lige under linjen hvor `LoginPage`- routen registreres:

```csharp
Routing.RegisterRoute("productdetails", typeof(ProductPage));
```

Vi kunne bruge denne page til at modtage rigtige produktoplysninger fra vores API og vise detaljer, men indtil videre bruges den blot til at teste, at Shell og routing er sat rigtigt op. Vi tilføjer en `Button` til `InputPage` med teksten "Go to product" og kobler den til en event handler i code-behind-filen. Her instantierer vi et mock-produkt, der repræsenterer app'en selv, lægger det i en dictionary og navigerer til `ProductPage` med dictionary'en som navigation state-parameter.

##### Listing 7.13 The updated InputPage content

```xml
<VerticalStackLayout Spacing="50">
<Label
Text="Input Page"
VerticalOptions="Center"
HorizontalOptions="Center" />
<Button Text="Go to product"
WidthRequest="200"
Clicked="Button_Clicked"/>
</VerticalStackLayout>
```

##### Listing 7.14 The Go to Product button event handler

```csharp
private async void Button_Clicked(object sender, EventArgs e)
{
var product = new Product { Name = "MauiStockTake", ManufacturerName =
 "BeachBytes" };
var pageParams = new Dictionary<string, object>
{
{ "Product", product }
};
await Shell.Current.GoToAsync("productdetails",
 pageParams);
}
```

① Bruger `GoToAsync` med routen registreret i `AppShell.xaml.cs` (`productdetails`) og en
dictionary af `<string, object>` som navigation state

Kører man app'en nu, ses "Go to Product"-knappen på `InputPage`, og trykker man på den, kommer man til `ProductPage`. Men for at `ProductPage` kan vise noget af den data, vi sender med, skal den sættes op til at modtage query properties.

Det gøres med `QueryPropertyAttribute` på den page, der skal modtage parametre (eller på page'ens binding context — mere om det i kapitel 9). `QueryPropertyAttribute` har en constructor, der tager to argumenter: først navnet på den property på page'en, der svarer til query property'en, dernæst navnet på query-parameteren, altså nøglen på elementet i dictionary'en.

Figur 7.20 illustrerer det: `QueryPropertyAttribute` bruges til at dekorere en page eller en pages binding context, så page'en kan modtage URL-parametre eller navigation state via URL-baseret navigation i Shell.

Brugte vi en string, som i eksemplet i figur 7.19, ville attributten se sådan ud:

```csharp
[QueryProperty("myPagesString", "myvalue")]
```

Her skal vi sende en query parameter kaldet `myvalue`, som bindes til en string kaldet `myPagesString` på den page, vi navigerer til. Man kan naturligvis også bruge `nameof` for at undgå tastefejl. Man kan tilføje så mange `QueryPropertyAttribute`s til sine pages, som man har brug for.

Til `ProductPage` skal vi tilføje et `Product` kaldet `Product`, der matcher nøglen i navigation state'en. Vi vil vise produktnavn og producentnavn, så vi skal bruge string-properties til disse samt backing fields, og vi skal kalde `OnPropertyChanged` i setteren.

##### Listing 7.15 ProductPage.xaml.cs updated for navigation state parameters

```csharp
namespace MauiStockTake.Maui.Pages;
[QueryProperty(nameof(Product), nameof(Product))]
public partial class ProductPage : ContentPage
{
public ProductPage()
{
InitializeComponent();
BindingContext = this;
}
Product _product;
public Product Product
{
get { return _product; }
set
{
_product = value;
ProductName = _product.Name;
ManufacturerName = _product.ManufacturerName;
}
}
string _productName;
public string ProductName
{
get => _productName;
set
{
_productName = value;
OnPropertyChanged();
}
}
string _manufacturerName;
public string ManufacturerName
{
get => _manufacturerName;
set
{
_manufacturerName = value;
OnPropertyChanged();
}
}
}
```

Sidste skridt er at tilføje `Label`s og bindings til XAML'en. Vi kan også centrere `VerticalStackLayout`'et og tilføje lidt spacing.

##### Listing 7.16 The updated ProductPage.xaml

```xml
<VerticalStackLayout Spacing="50"
VerticalOptions="Center">
<Label Text="{Binding ProductName}"
FontSize="Title"
VerticalOptions="Center"
HorizontalOptions="Center" />
<Label Text="{Binding ManufacturerName}"
FontSize="Subtitle"
VerticalOptions="Center"
HorizontalOptions="Center" />
</VerticalStackLayout>
```

Kører man app'en nu, ses "Go to Product"-knappen med det samme. Trykker man på den, kommer man til `ProductPage` og ser de detaljer, der blev sendt med i `GoToAsync`. Figur 7.21 viser begge skærmbilleder: i `InputPage` til venstre bruges en `Button` til programmatisk at navigere til `ProductPage` med `GoToAsync`. Et produkt sendes med i en dictionary, og `ProductPage` læser det via elementets nøgle og tildeler properties til strings, som `Label`s i XAML'en er bundet til.

## Summary

- Du kan bruge `ContentPage` til at bygge navigérbare dele af dine apps.
- `ContentPage` har en `Content`-property, som du kan tildele et layout eller et control.
Tildel et layout, så du kan tilføje flere controls.
- Almindelige page lifecycle-metoder lader dig eksekvere kode som en del af app'ens flow
frem for at kræve brugerinteraktion. Det kan bruges til at indlæse data, når en page kommer frem, eller udføre andre handlinger, brugeren ikke selv skal initiere.
- Du kan bruge tabbed navigation, hierarchical navigation eller flyout navigation i .NET
MAUI-apps. Eller du kan kombinere dem.
- Du kan bruge Shell til at forenkle opbygningen af en app med flere pages. Shell lader dig
definere navigation hierarchy og arkitektur for app'ens pages i XAML.
- Shell giver automatisk navigation for flyout items og tabs, men du kan navigere
programmatisk ved at tildele routes til pages.
- Du kan sende data med i navigation i Shell via query strings eller en dictionary af
`<string, object>`, der repræsenterer navigation state.
