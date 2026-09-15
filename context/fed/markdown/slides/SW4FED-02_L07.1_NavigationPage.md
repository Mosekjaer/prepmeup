# NavigationPage

## Metadata

- **Lektion:** L07.1 – NavigationPage
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L07/NavigationPage.pdf (10 slides)
- **Emner dækket:**
  - Repetition af page-typerne i .NET MAUI
  - Skift fra Shell til NavigationPage i `App`
  - NavigationPage-properties og -events
  - Modeless navigation med `PushAsync` / `PopAsync`
  - Navigationsbarens opbygning
  - Manipulation af navigation stacken: `InsertPageBefore`, `RemovePage`
  - Modal navigation med `PushModalAsync` / `PopModalAsync`

---

## 1. Pages — repetition

.NET MAUI-apps består af én eller flere pages. En page optager sædvanligvis hele skærmen eller vinduet, og hver page indeholder typisk mindst ét layout.

| Page | Beskrivelse |
|---|---|
| `ContentPage` | Viser et enkelt view og er den mest almindelige page-type |
| `FlyoutPage` | Styrer to relaterede sider: en flyout page, der præsenterer items, og en detail page, der præsenterer detaljer om items på flyout-siden |
| `NavigationPage` | Giver en hierarkisk navigationsoplevelse, hvor man kan navigere gennem pages frem og tilbage efter behov |
| `TabbedPage` | En række pages, som navigeres via tabs øverst eller nederst på siden, hvor hvert tab loader page-indholdet |

## 2. Fra Shell til NavigationPage

`NavigationPage` er inkompatibel med .NET MAUI Shell-apps, og der kastes en exception, hvis man forsøger at bruge `NavigationPage` i en Shell-app. Man skifter derfor `MainPage` ud i `App`-klassen:

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //MainPage = new AppShell();
        MainPage = new NavigationPage(new MainPage());
    }
}
```

## 3. NavigationPage-properties

`NavigationPage` giver en hierarkisk navigationsoplevelse, hvor man kan navigere gennem sider frem og tilbage. Den leverer navigation som en last-in, first-out (LIFO) stack af `Page`-objekter.

| Property | Beskrivelse |
|---|---|
| `BarBackground` | Angiver navigationsbarens baggrund som en `Brush` |
| `BarBackgroundColor` | Angiver navigationsbarens baggrundsfarve |
| `BackButtonTitle` | Repræsenterer teksten på back-knappen. Dette er en attached property |
| `BarTextColor` | Angiver farven på teksten i navigationsbaren |
| `CurrentPage` | Repræsenterer den side, der ligger øverst på navigation stacken. Read-only |
| `HasNavigationBar` | Angiver om der er en navigationsbar på `NavigationPage`'en |
| `HasBackButton` | Angiver om navigationsbaren indeholder en back-knap |
| `IconColor` | Definerer baggrundsfarven for ikonet i navigationsbaren. Attached property |
| `RootPage` | Repræsenterer rodsiden i navigation stacken. Read-only |
| `TitleIconImageSource` | Definerer ikonet, der repræsenterer titlen i navigationsbaren. Attached property |
| `TitleView` | Definerer det view, der kan vises i navigationsbaren. Attached property |

## 4. NavigationPage-events

`NavigationPage`-klassen definerer også tre events:

| Event | Beskrivelse |
|---|---|
| `Pushed` | Raises når en side pushes på navigation stacken |
| `Popped` | Raises når en side poppes fra navigation stacken |
| `PoppedToRoot` | Raises når den sidste non-root page poppes fra navigation stacken |

Alle tre events modtager `NavigationEventArgs`-objekter, som definerer en read-only `Page`-property. Den henter den side, der blev poppet fra navigation stacken, eller den nyligt synlige side på stakken.

## 5. Modeless navigation

.NET MAUI understøtter modeless page navigation. En modeless page bliver på skærmen og forbliver tilgængelig, indtil man navigerer til en anden side.

En `NavigationPage` bruges typisk til at navigere gennem en stak af `ContentPage`-objekter. Når en side navigerer til en anden, pushes den nye side på stakken og bliver den aktive side:

```csharp
await Navigation.PushAsync(new InputPage());
```

Når den anden side vender tilbage til den første, poppes en side fra stakken, og den nye øverste side bliver aktiv:

```csharp
await Navigation.PopAsync();
```

Den aktive side kan poppes fra navigation stacken ved at trykke på Back-knappen på en enhed, uanset om det er en fysisk knap på enheden eller en on-screen-knap.

### Navigationsbaren

En `NavigationPage` består af en navigationsbar, hvor den aktive side vises under navigationsbaren. Slidet viser et diagram over navigationsbarens hovedkomponenter: back-knap, valgfrit ikon og titel.

Et valgfrit ikon kan vises mellem back-knappen og titlen.

Navigationsmetoderne eksponeres af `Navigation`-propertyen på alle `Page`-afledte typer. Disse metoder giver mulighed for at pushe sider på navigation stacken, poppe sider fra stakken og manipulere stakken.

## 6. Manipulation af navigation stacken

`Navigation`-propertyen på en `Page` eksponerer en `NavigationStack`-property, hvorfra siderne i navigation stacken kan hentes.

Selvom .NET MAUI opretholder adgangen til navigation stacken, stiller `Navigation`-propertyen metoderne `InsertPageBefore` og `RemovePage` til rådighed til at manipulere stakken ved at indsætte eller fjerne sider.

- `InsertPageBefore`-metoden indsætter en angivet side i navigation stacken før en eksisterende angivet side.
- `RemovePage`-metoden fjerner den angivne side fra navigation stacken.

<!-- Slidet viser metodesignaturerne som billeder; koden er ikke udtrukket i råteksten -->

## 7. Modal navigation

.NET MAUI understøtter modal page navigation. En modal page opfordrer brugeren til at fuldføre en selvstændig opgave, som man ikke kan navigere væk fra, før opgaven er fuldført eller annulleret. En modal page kan være hvilken som helst af de page-typer, .NET MAUI understøtter.

For at vise en side modalt skal appen pushe den på **modal stacken**, hvor den bliver den aktive side:

```csharp
await MainPage.Navigation.PushModalAsync<LoginPage>();
```

For at vende tilbage til den forrige side skal appen poppe den aktuelle side fra modal stacken, hvorefter den nye øverste side bliver aktiv:

```csharp
await Navigation.PopModalAsync();
```

Den aktive side kan poppes fra navigation stacken ved at trykke på Back-knappen på en enhed, uanset om det er en fysisk knap eller en on-screen-knap.

## 8. Referencer og links

- *.NET MAUI in Action*
- NavigationPage — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage
