# Pages og navigation i .NET MAUI

## Metadata

- **Lektion:** L06.1 – Pages and Navigation
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L06/Pages and Navigation.pdf (23 slides)
- **Emner dækket:**
  - De fire page-typer i .NET MAUI
  - `Page` base class og dens fælles properties
  - Page lifecycle-metoder: `OnAppearing`, `OnDisappearing` m.fl.
  - `ContentPage` og dens `Content`-property
  - De fire navigationsparadigmer
  - `NavigationPage` og hierarkisk navigation med Push/Pop
  - `FlyoutPage` og flyout-navigation
  - `TabbedPage` og tabbed navigation

---

## 1. Pages

.NET MAUI-apps består af én eller flere pages. En page optager sædvanligvis hele skærmen eller vinduet, og hver page indeholder typisk mindst ét layout.

.NET MAUI indeholder følgende pages:

| Page | Beskrivelse |
|---|---|
| `ContentPage` | Viser et enkelt view og er den mest almindelige page-type |
| `FlyoutPage` | En page, der styrer to relaterede sider med information: en flyout page, der præsenterer items, og en detail page, der præsenterer detaljer om items på flyout-siden |
| `NavigationPage` | Giver en hierarkisk navigationsoplevelse, hvor man kan navigere gennem pages frem og tilbage efter behov |
| `TabbedPage` | Består af en række pages, som kan navigeres via tabs øverst eller nederst på siden, hvor hvert tab loader page-indholdet |

## 2. Page base class

I .NET MAUI er en page en klasse, der arver fra `Page` base class.

`Page` har flere properties relateret til at rendere et UI på skærmen, samt nogle lifecycle-metoder og events. Den har også properties, som arves af og kan være nyttige i de afledte page-typer.

Afhængigt af det navigationsparadigme, man bruger til at præsentere siden, kan disse properties gøre forskellige ting — eller i nogle tilfælde ingenting.

Alle disse properties er bindable, hvilket betyder, at man kan tildele dem værdier direkte eller binde dem til properties af tilsvarende typer i en binding context.

### Fælles page properties

| Property | Beskrivelse |
|---|---|
| `IconImageSource` | Angiver et billede, der skal vises som sidens ikon |
| `BackgroundImageSource` | Angiver et billede, der vises i baggrunden |
| `Padding` | Angiver hvor langt fra et views grænse dets interne elementer kan vises |
| `Title` | Sætter sidens titel |
| `MenuBarItems` | En collection af typen `MenuBarItem` til at vise en menu |

## 3. Page lifecycle-metoder

`Page` base class stiller flere metoder til rådighed, som kaldes af event handlers som reaktion på page lifecycle events.

Lifecycle events er knyttet til sidens livscyklus, i modsætning til et direkte svar på en ekstern handling som en brugerinteraktion eller en push notification. Mange af disse lifecycle events udspringer dog i sidste ende af en brugerinteraktion eller en ekstern stimulus.

### OnAppearing

`OnAppearing`-metoden kaldes, når en page vises. Det er en vigtig metode, da den bør udføre al page-initialiseringslogik.

- Constructors: bør kun bruges til at sætte initielle værdier på fields.
- `OnAppearing`: til alt andet, der skal ske, når en page vises.

Typisk use case: indlæsning af data. Dataindlæsning er som regel en asynkron operation, da data hentes enten fra en database eller et web API — og man kan ikke have en asynkron constructor.

### OnDisappearing

Siden forsvinder, når `OnDisappearing`-metoden kaldes.

Typisk use case: den er nyttig for sider, der styrer en form for state — til at unsubscribe fra events og messages, og til at rydde op i resources, der ikke længere er nødvendige, når siden ikke længere er synlig.

### Kodeeksempel: async i constructor kontra OnAppearing

Variant 1 — async kald i constructoren via en separat task-metode:

```csharp
public MainPage()
{
    InitializeComponent();
    _database = new Database();
    _ = Initialize(); //Async call in constructor

    TodosCollection.ItemsSource = Todos;
}

private async Task Initialize()
{
    //Async data fetch
    var todos = await _database.GetTodos();

    foreach(var todo in todos)
    {
        Todos.Add(todo);
        ..
    }
    ..
}
```

Variant 2 — async i `OnAppearing`-lifecycle-metoden:

```csharp
public MainPage()
{
    InitializeComponent();
    _database = new Database();
}

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

**Hvornår bruger man hvad?** Variant 1 (async i constructor med separat task-metode) giver hurtig rendering og bruges, når dataindlæsningen ikke er kritisk. Variant 2 (`OnAppearing`) bruges, når dataene er essentielle og vigtige i appen, og man vil styre dataindlæsning og rendering med lifecycle-operationen.

Eksemplerne stammer fra MauiToDo-appen (ver. 2), hvor den venstre variant er den oprindelige, og den højre erstatter den med `OnAppearing`.

### Øvrige lifecycle-metoder

**`OnNavigatedTo` og `OnNavigatedFrom`** udfører samme funktion som `OnAppearing` og `OnDisappearing`. Forskellen er timing: `OnNavigatedTo`/`OnNavigatedFrom` kaldes **før** siden vises eller forsvinder, mens `OnAppearing`/`OnDisappearing` kaldes **efter**. `OnAppearing` og `OnDisappearing` passer sandsynligvis bedst til de fleste use cases.

**`OnSizeAllocated`** kaldes, hver gang sidens størrelse ændres. Nyttig til at bygge responsive UI og til at ændre størrelse eller position på elementer som reaktion på ændringer i sidens størrelse.

**`BackButtonPressed`** kaldes, når back-knappen bruges enten på .NET MAUI-navigationsbaren eller i Android. Metoden bør bruges til at supplere, ikke til at ændre adfærd. Vil man ændre adfærden for back-knappen for Android-brugere, skal man override `OnBackPressed`-metoden i `MainActivity`-klassen i Android-platformsmappen.

## 4. ContentPage

`ContentPage` viser et enkelt view, som ofte er et layout såsom et `Grid` eller et `StackLayout`, og er den mest almindelige page-type.

- `ContentPage` definerer en `Content`-property af typen `View`, som definerer det view, der repræsenterer sidens indhold.
- `Content`-propertyen er backed af et `BindableProperty`-objekt, hvilket betyder, at den kan være target for data bindings og kan styles.
- `ContentPage` arver de bindable properties `Title`, `IconImageSource`, `BackgroundImageSource`, `IsBusy` og `Padding` fra `Page`-klassen.

### Kodeeksempel

XAML:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.MainPage">
    ...
</ContentPage>
```

Code-behind:

```csharp
public partial class MainPage : ContentPage
{
}
```

En `ContentPage` har en `Content`-property, som man kan tildele et `View` — altså et hvilket som helst layout eller control. I slide-eksemplet tildeles en `ScrollView` til `Content`-propertyen; den wrapper et `VerticalStackLayout`, som indeholder alle controls. `ScrollView` sikrer, at indhold, der ikke passer på skærmen, stadig er tilgængeligt, og `VerticalStackLayout` gør det muligt at tilføje flere controls til siden.

## 5. Navigation i .NET MAUI

For at bygge en mere kompleks app med flere sider og navigation understøtter .NET MAUI navigation til multipage-apps, hvor man kan navigere gennem sider frem og tilbage.

.NET MAUI understøtter disse navigationsparadigmer:

- Hierarchical navigation
- Tabbed navigation
- Flyout navigation
- Route-based navigation

De tre første hovedparadigmer understøttes hver af en subclass af `Page`, som er specifikt konfigureret til at understøtte paradigmet. Shell understøtter tabbed og flyout navigation samt route-based navigation.

## 6. NavigationPage: hierarkisk navigation

`NavigationPage` bruges til at levere hierarkisk navigation ved at vise `ContentPage`s i en navigation stack.

- Elementer kan pushes (tilføjes) på stakken og poppes (fjernes) fra stakken.
- I `NavigationPage`s tilfælde er de elementer, der pushes eller poppes, pages.
- Hierarkisk navigation er et godt valg, hvis appen skal starte med en menuskærm.
- **Vigtigt:** `NavigationPage` er inkompatibel med .NET MAUI Shell-apps, og der kastes en exception, hvis man forsøger at bruge `NavigationPage` i en Shell-app.
- Ideel til en hierarkisk struktur, f.eks. hvor en menu går ned i hierarkiet og vender tilbage til hovedsektionen.

I hierarkisk navigation kan en side pushes på stakken og poppes fra stakken efter last-in, first-out-mønstret.

### Forskellen fra de andre paradigmer

Hierarkisk navigation adskiller sig på ét afgørende punkt:

- **I de andre paradigmer** er sider bundet til en UI-control som et tab eller et flyout item, og et tap eller klik resulterer i, at siden vises automatisk af frameworket.
- **I hierarkisk navigation** pusher man sider på navigation stacken programmatisk. Man bruger f.eks. en `Button` til at navigere til en side.

### Kodeeksempel

Push af en ny side med `PushAsync`-metoden på `Navigation`-klassen, med en instans af page-typen som parameter:

```csharp
await Navigation.PushAsync(new InputPage());
```

Pop af siden fra stakken, så man navigerer tilbage til den forrige side:

```csharp
await Navigation.PopAsync();
```

## 7. Flyout navigation og FlyoutPage

Med flyout navigation glider (eller "flyver") en menu ud fra siden af skærmen og præsenterer en liste af sider, der kan vises i hovedvisningsområdet. Brugeren vælger en side, som så optager hovedvisningsområdet, mens menuen glider væk igen.

`FlyoutPage` understøtter flyout-navigationsparadigmet og har to child properties af typen `Page`:

- **`Flyout`**: bruges til menuen og definerer flyout-siden.
- **`Detail`**: den valgte side tildeles hertil og vises på skærmen. Definerer den detail page, der vises for det valgte item i flyout-siden.

### Layout-adfærd

En `FlyoutPage` har to layout behaviors:

- **Popover layout**
  - Apps, der kører på telefoner, bruger altid denne adfærd.
  - Detail-siden dækker eller dækker delvist flyout-siden.
- **Split layout**
  - Apps på tablets eller desktops kan bruge denne adfærd.
  - Flyout-siden vises til venstre og detail-siden til højre.
  - Windows bruger den som default.

Man kan bruge en `ContentPage` til at bygge en fuldt tilpasset flyout-menu og tildele den til `Flyout`-propertyen på `FlyoutPage`. Vær dog varsom med at introducere ukendte, tilpassede paradigmer eller UX.

### FlyoutPage properties

`FlyoutPage` skal forsynes med en collection af typen `FlyoutPageItem`, som brugeren kan vælge fra.

`FlyoutPageItem` har en property kaldet `TargetType`, som man tildeler den ønskede side. Når en bruger vælger et af disse `FlyoutPageItem`s, tildeles det valgte items `TargetType`-property til `Detail`-propertyen på `FlyoutPage`, hvorved siden vises i appens hovedvisningsområde.

## 8. Tabbed navigation og TabbedPage

Med tabbed navigation bruges tabs til at give adgang til de forskellige sider i appen.

En `TabBar` vises med en collection af labeled icons:

- Hvert ikon repræsenterer en side i appen, som brugeren kan vælge.
- Den valgte side vises i hovedvisningsområdet.
- På iOS vises TabBar'en i bunden af skærmen.
- På Windows og Android vises TabBar'en i toppen, hvilket minder mere om de tabs, man kender fra webbrowsere.

I .NET MAUI muliggør `TabbedPage` det tabbede navigationsparadigme. `TabbedPage` tillader flere children, som hver især skal nedarve fra `Page`. Disse sider vises automatisk som tabs med ikon og tekst taget fra sidens `Icon`- og `Title`-properties.

## 9. Referencer og links

- *.NET MAUI in Action*, Matt Goldman
- ContentPage — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/contentpage
- FlyoutPage — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/flyoutpage
- NavigationPage — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage
- TabbedPage — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/tabbedpage
