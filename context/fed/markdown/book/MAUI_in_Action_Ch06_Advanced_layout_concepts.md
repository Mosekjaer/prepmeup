# Kapitel 6 – Advanced layout concepts

## Metadata

- **Kapitel:** 6 – Advanced layout concepts
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L05
- **Hovedemner:**
  - "Thinking in grids": at nedbryde komplekse designs til `Grid`-rows og -columns
  - `BindableLayout` som statisk klasse, der gør ethvert layout til et collection view
  - `BindableLayout.ItemsSource` og `BindableLayout.ItemTemplate`
  - `FlexLayout` med `JustifyContent` og `Wrap` (CSS flexbox-pendant)
  - Kald af REST-API med `HttpClient.GetFromJsonAsync<T>` og deserialisering til modelklasser
  - .NET MAUI Community Toolkit `Popup` og `ShowPopupAsync`
  - `ICommand`, `Command<T>`, `CommandParameter` og `Source`-markup extension i bindings
  - `TapGestureRecognizer`
  - `AbsoluteLayout` med `LayoutBounds` (`Rect`) og `LayoutFlags` (proportional vs. absolut)
  - Font icons: registrering af icon-fonts og brug af glyph-koder i XAML
  - Praktisk UI challenge: replikering af Outlook Inbox-UI'et med kombinerede layouts

---

## Indledning

Vi har set på `Grid`, `HorizontalStackLayout`, `VerticalStackLayout` og `FlexLayout`, og næsten ethvert UI kan opnås med disse. Men der er flere layout-muligheder i .NET MAUI.

Før vi går til nye layouts, genbesøger vi `Grid` og ser, hvordan det anvendes på mere komplekse layouts. I kapitel 5 så vi det brugt på et UI, der naturligt lægger sig til et grid-mønster, men det er også ofte det bedste layout til at arrangere ting, der *ikke* umiddelbart ligner et grid.

Derefter ser vi på, hvordan `BindableLayout` kan konvertere ethvert layout til et collection view, og hvordan `FlexLayout` er en ideel kandidat til den behandling. Til sidst behandles `AbsoluteLayout`, som kan være et kraftfuldt værktøj, når man skal ramme et præcist UI-mål.

## 6.1 Thinking in grids

Når det gælder det overordnede side- eller skærmlayout, er `Grid` den bedste mulighed i de fleste scenarier.

**SSW Rewards** er en app udviklet til at drive engagement i udviklercommunity'et. Brugere kan optjene point og bytte dem til præmier, og appen har en profiles-sektion med billede og bio af medarbejdere. Den er open source.

**Figure 6.1** viser designet til profiles-siden. Det ligner ikke umiddelbart et `Grid`, men `Grid`-layoutet bruges til at implementere designet. Når man har udviklet .NET MAUI-apps et stykke tid, begynder man at se rows og columns i sådanne designs — næsten som Neo, der ser verden som grøn kode-regn i The Matrix.

**Figure 6.2** viser første nedbrydningsskridt: identificér det øverste grid. Designet kan brydes ned i **tre rows** — en header-sektion, en body-sektion og en bio-sektion — og den øverste row har **tre columns**.

Sidens tre hovedsektioner:

- **Header** — indeholder navn, titel og nogle navigationsknapper.
- **Main details** — indeholder et billede, en skills summary, en QR-indikator (viser om brugeren har scannet denne udvikler og optjent point) og nogle social interaction-knapper.
- **Footer** — indeholder en kort bio.

Fra designværktøjet kendes de præcise relative proportioner af de tre rows: **`2*, 9*, 3*`**.

Kun den øverste row bruger columns, men det er intet problem, da vi kan sætte `ColumnSpan` på de to resterende rows til 3, så de fylder alle tre columns. Navigationskontrollerne placeres i columns 0 og 2 af row 0. Column-definitionerne fra designet er **`*, 5*, *`**.

**Figure 6.3** viser nedbrydningen af developer details-sektionen: to lige brede columns, hvor billedet fylder alle tre rows af den første column (og har `ColumnSpan` 2, så billedet kan overlappe andre elementer). De resterende elementer ligger i anden column: skills summary i første row, QR-ikonet i anden og social interaction-knapperne i tredje. Row-højderne er **`5*, 2*, *`**.

### Listing 6.1 The Grid layout for the SSW Rewards People page

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             BackgroundColor="{StaticResource PeopleBackground}"
             xmlns:controls="clr-namespace:SSW.Rewards.Controls"
             xmlns:converters="clr-namespace:SSW.Rewards.Converters"
             x:Class="SSW.Rewards.Pages.PeoplePage">
    <ContentPage.Content>
        <Grid RowDefinitions="2*, 9*, 3*"
              ColumnDefinitions="*, 5*, *">

            <Grid Grid.Row="1"
                  Grid.Column="0"
                  Grid.ColumnSpan="3"
                  ColumnDefinitions="*, *"
                  RowDefinitions="5*, 2*, *">
            </Grid>

    </ContentPage.Content>
</ContentPage>
```

<!-- Kildens listing viser kun Grid-definitionerne og mangler afsluttende </Grid>-tags; gengivet som i bogen -->

Kun `Grid`-definitionerne vises; det er en kompleks side, og at inkludere alle views, converters, helpers og bindings ligger uden for afsnittets scope. Den fulde kildekode findes på GitHub: https://github.com/SSWConsulting/SSW.Rewards

## 6.2 BindableLayout

**`BindableLayout` er ikke selv et layout.** Det er en **statisk klasse**, der kan attaches til ethvert layout, hvilket gør layoutet i stand til at generere sit eget indhold. I praksis **gør det ethvert layout til et collection view**.

Med `BindableLayout` kan vi angive en `ItemsSource`-property for et layout og derefter bruge en `DataTemplate` til at rendere hvert item i collection'en — præcis som med `CollectionView` — som var de hardcodede child views af det layout, vi attacher `BindableLayout` til. Det fungerer godt med `HorizontalStackLayout` eller `VerticalStackLayout` og **særligt godt med `FlexLayout`**.

**Det giver ikke megen mening at bruge `BindableLayout` med `Grid`**, da views i et `Grid` har brug for `Row`- og `Column`-properties for at arrangere sig korrekt.

`CollectionView` tilbyder funktionalitet, man ikke får med `BindableLayout` — fx muligheden for at vælge ét eller flere items understøttet af bindable properties. **`BindableLayout` er en god mulighed, når du blot skal bruge en data source til at rendere indhold; `CollectionView` er bedre, når din bruger skal interagere med selve collection'en** frem for kun med items i den.

### MauiMovies

Vi bygger en app, der giver filmanbefalinger baseret på, hvad der trender, og på valgte genrer.

**Figure 6.4** viser hoved-UI-layoutet: en liste af trending film med poster, titel og rating. Øverst er en liste af de genrer, brugeren har valgt at filtrere efter — brugeren kan tappe på listen for at ændre sit valg.

**Figure 6.5** viser, at brugeren kan tappe på "chips" med de valgte genrer for at se listen og ændre sit valg.

**Figure 6.6** viser, at brugeren kan tappe eller klikke på en af filmene for at se mere information. En popup viser poster, titel, rating, genrer og den detaljerede beskrivelse.

Vi bruger `BindableLayout` med **to `FlexLayout`s**: ét til genrelisten og ét til filmlisten.

### 6.2.1 Creating the MauiMovies MainPage

Appen bygges med det gratis API fra https://www.themoviedb.org, hvor man kan anmode om en API-nøgle fra account settings. Opret en ny .NET MAUI-app fra `blankmaui`-templaten kaldet **MauiMovies**.

Først skal vi have typer til at deserialisere data fra API'et. (I Visual Studio kan man kopiere JSON og bruge Edit | Paste Special til automatisk konvertering til C#-klasser; alternativt https://json2csharp.com.)

#### Listing 6.2 The MovieResult class

```csharp
namespace MauiMovies;

public class MovieResult
{
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string poster_path { get; set; }
    public bool video { get; set; }
    public double vote_average { get; set; }
    public string overview { get; set; }
    public string release_date { get; set; }
    public int vote_count { get; set; }
    public int id { get; set; }
    public bool adult { get; set; }
    public string backdrop_path { get; set; }
    public string title { get; set; }
    public List<int> genre_ids { get; set; }
    public double popularity { get; set; }
    public string media_type { get; set; }
}
```

#### Listing 6.3 The TrendingMovies class

```csharp
namespace MauiMovies;

public class TrendingMovies
{
    public int page { get; set; }
    public List<MovieResult> results { get; set; }
    public int total_pages { get; set; }
    public int total_results { get; set; }
}
```

#### Listing 6.4 The Genre class

```csharp
namespace MauiMovies;

public class Genre
{
    public int id { get; set; }
    public string name { get; set; }
}
```

#### Listing 6.5 The GenreList class

```csharp
namespace MauiMovies;

public class GenreList
{
    public List<Genre> genres { get; set; }
}
```

`UserGenre` subklasser `Genre`, men tilføjer en `Selected`-property.

#### Listing 6.6 The UserGenre class

```csharp
namespace MauiMovies;

public class UserGenre : Genre
{
    public bool Selected { get; set; }
}
```

Åbn `MainPage.xaml` og slet alt mellem `<ContentPage...>...</ContentPage>`. Åbn `MainPage.xaml.cs` og slet `count`-fieldet og `OnCounterClicked`-metoden. Herefter tilføjes:

1. Et field til API-nøglen.
2. Et field til base-URI'en for API'et.
3. Et field til de top-20 trending film.
4. Et field til listen af genrer.
5. Et field til `HttpClient`.
6. En loading-property til at vise en `ActivityIndicator`, mens vi venter på data.
7. En property til de genrer, brugeren har valgt.
8. En property til film filtreret efter genre.

#### Listing 6.7 The updated MainPage.xaml.cs with the fields and properties

```csharp
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MauiMovies;

public partial class MainPage : ContentPage
{
    string _apiKey = "[your API key]";
    string _baseUri = "https:/ /api.themoviedb.org/3/";

    private TrendingMovies _movieList;
    private GenreList _genres;

    public ObservableCollection<Genre> Genres { get; set; } = new();
    public ObservableCollection<MovieResult> Movies { get; set; } = new ();

    public bool IsLoading { get; set; }

    private readonly HttpClient _httpClient;

    public MainPage()
    {
        InitializeComponent();
    }
}
```

Dernæst opdateres sidens konstruktor: vi sætter sidens binding context til sig selv, så UI'et kan binde til de netop oprettede properties, og instantierer `HttpClient` med API'ets base-URI.

#### Listing 6.8 The MainPage.xaml.cs constructor

```csharp
public MainPage()
{
    InitializeComponent();
    BindingContext = this;
    _httpClient = new HttpClient { BaseAddress = new Uri(_baseUri) };
}
```

Sidste skridt før UI'et er den indledende dataindlæsning. Vi overrider sidens `OnAppearing`-metode og gør den `async`.

#### Listing 6.9 The MainPage.xaml.cs OnAppearing method

```csharp
protected override async void OnAppearing()
{
    base.OnAppearing();

    IsLoading = true;
    OnPropertyChanged(nameof(IsLoading));

    _genres = await _httpClient
        .GetFromJsonAsync<GenreList>($"genre/movie/list?api_key={_apiKey}&language=en-US");   // ①

    _movieList = await _httpClient
        .GetFromJsonAsync<TrendingMovies>($"trending/movie/week?api_key={_apiKey}&language=en-US");  // ②

    IsLoading = false;
    OnPropertyChanged(nameof(IsLoading));
}
```

① Bruger `HttpClient` til at hente en liste af filmgenrer fra API'et og deserialisere den til `_genres`-fieldet.
② Bruger `HttpClient` til at hente en liste af trending film og deserialisere den til `_movieList`-fieldet.

**Figure 6.7** viser layout-nedbrydningen for MauiMovies: hovedlayoutet for siden er en `VerticalStackLayout`, som arrangerer child views vertikalt fra top til bund. Det første view er et `FlexLayout`, der viser de valgte genrer. Derefter tilføjes endnu et `FlexLayout` til den filtrerede filmliste.

I `MainPage.xaml` tilføjes en `VerticalStackLayout` med `Spacing` 10 og `Padding` 30. Det første element er genre-selection-boksen: endnu en `VerticalStackLayout` med en `Label` ("Genres") og et `FlexLayout` under, som viser de valgte genrer via `BindableLayout` som "chips" eller pills.

`Border` bruges til to ting: at wrappe hele genre-selection-boksen i en pæn border og at give chip-effekten. Til sidst tilføjes en gesture recognizer, så vi kan vise en popup, når brugeren tapper på boksen.

#### Listing 6.10 MainPage.xaml with the genre selection box

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiMovies.MainPage">

    <VerticalStackLayout Spacing="10" Padding="30">

        <Border Stroke="Black"
                StrokeThickness="2"
                StrokeShape="RoundRectangle 5">

            <VerticalStackLayout Padding="20">

                <VerticalStackLayout.GestureRecognizers>
                    <TapGestureRecognizer NumberOfTapsRequired="1"
                                          Command="{Binding ChooseGenres}"/>
                </VerticalStackLayout.GestureRecognizers>

                <Label Text="Genres:"/>

                <FlexLayout BindableLayout.ItemsSource="{Binding Genres}"
                            JustifyContent="SpaceEvenly"
                            Wrap="Wrap">
                    <BindableLayout.ItemTemplate>
                        <DataTemplate>
                            <Border Stroke="CadetBlue"
                                    StrokeShape="RoundRectangle 15">
                                <Label Text="{Binding name}"
                                       TextColor="White"
                                       Padding="10"
                                       BackgroundColor="CadetBlue"/>
                            </Border>
                        </DataTemplate>
                    </BindableLayout.ItemTemplate>
                </FlexLayout>

            </VerticalStackLayout>
        </Border>

    </VerticalStackLayout>
</ContentPage>
```

① Binder `TapGestureRecognizer`'ens `Command`-property til en property kaldet `ChooseGenres`.
② Tilføjer et `FlexLayout` til at vise listen af valgte genrer og gør det til et `BindableLayout` med `ItemsSource` bundet til `Genres`-propertyen i code-behind.

#### Listing 6.11 The movies layout to add to MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiMovies.MainPage">

    <VerticalStackLayout Spacing="10" Padding="30">
        ... [Code omitted for clarity]...

        <ScrollView>
            <FlexLayout BindableLayout.ItemsSource="{Binding Movies}"
                        JustifyContent="SpaceEvenly"
                        Wrap="Wrap">
                <BindableLayout.ItemTemplate>
                    <DataTemplate>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </FlexLayout>
        </ScrollView>

    </VerticalStackLayout>
</ContentPage>
```

① Tilføjer et `FlexLayout`, gør det til et `BindableLayout` og binder `ItemsSource` til `Movies`-collection'en.

Til `DataTemplate` bruger vi en `VerticalStackLayout` til at arrangere poster, titel og rating. Titel og rating placeres i en `HorizontalStackLayout` under posteren, og posteren får en shadow for at give UI'et dybde.

#### Listing 6.12 The movies layout to add to MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiMovies.MainPage">

    <VerticalStackLayout Spacing="10" Padding="30">
        ... [Code omitted for clarity]...

        <ScrollView>
            <FlexLayout BindableLayout.ItemsSource="{Binding Movies}"
                        JustifyContent="SpaceEvenly"
                        Wrap="Wrap">
                <BindableLayout.ItemTemplate>
                    <DataTemplate>
                        <VerticalStackLayout WidthRequest="200"
                                             Margin="30"
                                             Spacing="10">

                            <VerticalStackLayout.GestureRecognizers>
                                <TapGestureRecognizer NumberOfTapsRequired="1"
                                                      Command="{Binding ShowMovies}"/>
                            </VerticalStackLayout.GestureRecognizers>

                            <Image Source="{Binding poster_path}"
                                   Aspect="AspectFit">
                                <Image.Shadow>
                                    <Shadow Brush="Black"
                                            Opacity="0.6"
                                            Offset="5,5"
                                            Radius="20"/>
                                </Image.Shadow>
                            </Image>

                            <HorizontalStackLayout>
                                <Label Text="{Binding title}"
                                       FontAttributes="Bold"
                                       WidthRequest="150"
                                       LineBreakMode="TailTruncation"/>
                                <Label HorizontalOptions="EndAndExpand"
                                       HorizontalTextAlignment="End"
                                       WidthRequest="50"
                                       Text="{Binding vote_average, StringFormat='{0:N1} ⭐'}"/>
                            </HorizontalStackLayout>

                        </VerticalStackLayout>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </FlexLayout>
        </ScrollView>

    </VerticalStackLayout>
</ContentPage>
```

`poster_path`-propertyen fra API'et er kun en delvis URL, så vi skal hydrere den med en base-URL for billeder (hentet fra API'ets `/Configuration`-endpoint). Vi tilføjer også en metode til at opdatere `Movies`-collection'en med den filtrerede filmliste, samt de to `ICommand`s, UI'et binder til.

#### Listing 6.13 MainPage.xaml.cs with the logic to load movies to the UI

```csharp
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace MauiMovies;

public partial class MainPage : ContentPage
{
    string _apiKey = "[YOUR API KEY HERE]]";
    string _baseUri = "https:/ /api.themoviedb.org/3/";
    string _imageBaseUrl = "https:/ /image.tmdb.org/t/p/w500";        // ①

    TrendingMovies _movieList;
    GenreList _genres;

    public ObservableCollection<UserGenre> Genres { get; set; } = new();
    public ObservableCollection<MovieResult> Movies { get; set; } = new();

    public ICommand ChooseGenres { get; set; }
    public ICommand ShowMovie { get; set; }

    public bool IsLoading { get; set; }

    HttpClient _httpClient;

    List<UserGenre> _genreList { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUri) };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        IsLoading = true;
        OnPropertyChanged(nameof(IsLoading));

        _genres = await _httpClient.GetFromJsonAsync<GenreList>(
            $"genre/movie/list?api_key={_apiKey}&language=en-US");

        _movieList = await _httpClient.GetFromJsonAsync<TrendingMovies>(
            $"trending/movie/week?api_key={_apiKey}&language=en-US");

        foreach (var movie in _movieList.results)
        {
            movie.poster_path = $"{_imageBaseUrl}{movie.poster_path}";
        }

        foreach (var genre in _genres.genres)
        {
            _genreList.Add(new UserGenre
            {
                id = genre.id,
                name = genre.name,
                Selected = false
            });
        }

        LoadFilteredMovies();

        IsLoading = false;
        OnPropertyChanged(nameof(IsLoading));
    }

    private void LoadFilteredMovies()
    {
        Movies.Clear();

        if (_genreList.Any(g => g.Selected))
        {
            var selectedGenreIds = _genreList.Where(g => g.Selected).Select(g => g.id);

            foreach (var movie in _movieList.results)
            {
                if (movie.genre_ids.Any(id => selectedGenreIds.Contains(id)))
                {
                    Movies.Add(movie);
                }
            }
        }
        else
        {
            foreach (var movie in _movieList.results)
            {
                Movies.Add(movie);
            }
        }
    }
}
```

① Tilføjer et field til base-URL'en for billeder fra API'et. Værdien er hentet fra TMDB API'ets configuration-endpoint.

**Figure 6.8** viser MauiMovies på Windows: `FlexLayout` bruges til at arrangere filmene og wrappe hver film til næste linje, hvis den ikke passer. Filmene kommer fra en `ObservableCollection`, og `BindableLayout` binder collection'en til `FlexLayout`'et. Hver film renderes efter en template med poster, titel og rating.

### 6.2.2 Creating the popup pages

Vi bruger popups til genre-selection-listen og filmdetaljerne. .NET MAUI Community Toolkit har en pæn popup view til det.

Installér NuGet-packagen **`CommunityToolkit.Maui`** i MauiMovies-projektet, og registrér den i `MauiProgram.cs` ved at kæde `UseCommunityToolkit()` på builderen:

```csharp
builder.UseMauiApp<App>().ConfigureFonts().UseMauiCommunityToolkit();
```

> **NOTE** Font-registreringerne er udeladt her for at gøre koden lettere at vise. Fjern dem ikke!

Den letteste måde at oprette popup-siderne på er at tilføje en .NET MAUI `ContentPage` fra templaten og tilpasse den. Tilføj en ny side via **.NET MAUI ContentPage (XAML)**-templaten og kald den `GenreListPopup`.

Åbn `GenreListPopup.xaml.cs` og skift den nedarvede type fra `ContentPage` til `Popup`, og hent `CommunityToolkit.Maui.Views`-namespacet ind. Der kommer fejl på grund af mismatch med XAML'en, så skift over til `GenreListPopup.xaml`:

- Hent Community Toolkit XAML-namespacet ind og giv det aliaset `mct`.
- Skift typen fra `ContentPage` til `mct:Popup`.
- Slet `Title`-propertyen (som `ContentPage`-templaten inkluderer, men `Popup` ikke har).
- `Popup` lader os definere størrelsen i XAML, så den kan tilføjes i åbningstagget.
- Slet alt indhold fra templaten.

#### Listing 6.14 GenreListPopup.xaml.cs

```csharp
using CommunityToolkit.Maui.Views;              // ①

namespace MauiMovies;

public partial class GenreListPopup : Popup     // ②
{
    public GenreListPopup()
    {
        InitializeComponent();
    }
}
```

① Henter `CommunityToolkit.Maui.Views`-namespacet ind.
② Opdaterer klassen til at nedarve fra `Popup` i stedet for `ContentPage`.

#### Listing 6.15 GenreListPopup.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<mct:Popup xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
           xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
           xmlns:mct="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
           Size="600,600"
           x:Class="MauiMovies.GenreListPopup">
</mct:Popup>
```

① Skifter typen fra `ContentPage` til `mct:Popup`.
② Henter .NET MAUI Community Toolkit-namespacet ind (til understøttelse af `Popup`-typen).
③ Sætter popup'ens `Size` til 600 (bredde) × 600 (højde).

Vi opdaterer konstruktoren til at tage en liste af typen `UserGenre`, tilføjer en `ObservableCollection` til at holde de modtagne `UserGenre`s og binde til i UI'et, og sætter popup'ens binding context til sig selv.

#### Listing 6.16 The ObservableCollection and constructor

```csharp
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace MauiMovies;

public partial class GenreListPopup : Popup
{
    public ObservableCollection<UserGenre> Genres { get; set; }

    public GenreListPopup(List<UserGenre> Genres)
    {
        BindingContext = this;
        this.Genres = new ObservableCollection<UserGenre>(Genres);
        InitializeComponent();
    }
}
```

Vi har brug for to event handlers. Den første håndterer, når et valg ændres i genrelisten. Vi bruger event arguments til at hente de aktuelt valgte items, itererer gennem `Genres`-collection'en og markerer hvert items `Selected`-property.

**Bemærk: dette er noget vi kan gøre med `CollectionView`, men ikke ville kunne med `BindableLayout` attached til et andet layout.**

#### Listing 6.17 The CollectionView_SelectionChanged method

```csharp
private bool _selectionHasChanged = false;

private void CollectionView_SelectionChanged(
    object sender, SelectionChangedEventArgs e)
{
    _selectionHasChanged = true;

    var selectedItems = e.CurrentSelection;

    foreach (var genre in Genres)
    {
        if (selectedItems.Contains(genre))
        {
            genre.Selected = true;
        }
        else
        {
            genre.Selected = false;
        }
    }
}
```

Den anden event handler lukker popup'en, når brugeren klikker en knap. `Popup`-baseklassen har en `Close()`-metode, som også kan returnere en værdi til den side, der kaldte popup'en, så den ved, om brugeren har ændret sit valg.

I listing 6.18 bruger vi desuden en property nedarvet fra `Popup` kaldet **`ResultWhenUserTapsOutsideOfPopup`** til også at returnere værdien, når brugeren lukker popup'en ved at tappe udenfor frem for at bruge Confirm-knappen.

#### Listing 6.18 The logic for indicating whether the selection has changed

```csharp
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace MauiMovies;

public partial class GenreListPopup : Popup
{
    // remaining code omitted

    Public GenreListPopup(List<UserGenre> Genres)
    {
        // remaining code omited
        ResultWhenUserTapsOutsideOfPopup = _selectionHasChanged;
    }

    // remaining code omitted

    private void Button_Clicked(object sender, EventArgs e) =>
        Close(_selectionHasChanged);
}
```

<!-- "Public" med stort P og "omited" står sådan i råteksten -->

#### Listing 6.19 GenreListPopup.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<mct:Popup xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
           xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
           xmlns:mct="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
           x:Class="MauiMovies.GenreListPopup">

    <VerticalStackLayout Spacing="10"
                         Padding="10">

        <Button Text="Confirm"
                Clicked="Button_Clicked"/>

        <CollectionView ItemsSource="{Binding Genres}"
                        SelectionMode="Multiple"
                        SelectionChanged="CollectionView_SelectionChanged">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Label Text="{Binding name}"/>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

    </VerticalStackLayout>
</mct:Popup>
```

① Delegerer `SelectionChanged`-eventet til den event handler, vi oprettede.

**Da genrerne sendes by reference, afspejles brugerens valg her i `MainPage`, når vi vender tilbage til den.**

Den sidste UI-del er endnu en popup til filmdetaljer. Brug samme proces til at tilføje en popup kaldet `MovieDetailsPopup`.

#### Listing 6.20 MovieDetailsPopup.xaml.cs

```csharp
using CommunityToolkit.Maui.Views;

namespace MauiMovies;

public partial class MovieDetailsPopup : Popup
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string PosterUrl { get; set; }
    public List<string> Genres { get; set; } = new();
    public double Rating { get; set; }

    public MovieDetailsPopup(MovieResult movie, List<Genre> genres)
    {
        Size = new Size(600, 600);

        Title = movie.title;
        Description = movie.overview;
        PosterUrl = movie.poster_path;
        Rating = movie.vote_average;

        foreach (var id in movie.genre_ids)
        {
            Genres.Add(genres.Where(
                g => g.id == id).Select(g => g.name).FirstOrDefault());
        }

        BindingContext = this;
        InitializeComponent();
    }
}
```

`MovieResult` indeholder kun en liste af genre-ID'er, så vi sender også listen af genrer ind og tildeler de relevante navne ud fra ID'erne.

#### Listing 6.21 MovieDetailsPopup.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<mct:Popup xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
           xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
           xmlns:mct="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
           x:Class="MauiMovies.MovieDetailsPopup">

    <VerticalStackLayout Padding="20"
                         Spacing="20">

        <Image Source="{Binding PosterUrl}"
               HeightRequest="300"
               Aspect="AspectFit"/>

        <FlexLayout JustifyContent="SpaceBetween">
            <Label Text="{Binding Title}"
                   FontAttributes="Bold"/>
            <Label HorizontalTextAlignment="End"
                   Text="{Binding Rating, StringFormat='{0:N1} ⭐'}"/>
        </FlexLayout>

        <FlexLayout BindableLayout.ItemsSource="{Binding Genres}"
                    JustifyContent="SpaceEvenly"
                    Wrap="Wrap">
            <Label Text="{Binding .}"/>
        </FlexLayout>

        <Label Text="{Binding Description}"/>

    </VerticalStackLayout>
</mct:Popup>
```

① Tilføjer et `FlexLayout` til genrerne, gør det til et `BindableLayout` og binder `ItemsSource` til listen af genrenavne.
② Vi behøver ikke en `DataTemplate`, da vi blot vil vise en `Label` pr. item — og fordi hvert item er en string, kan vi binde `Label`'ens `Text`-property til selve item'et (`{Binding .}`).

### 6.2.3 Showing the popup pages

I `MainPage.xaml.cs` tilføjes en async-metode med returtypen `Task` kaldet `ShowGenreList`. I den instantieres en ny `GenreListPopup` med `_genreList` som konstruktorparameter. Popup'en vises med extension methoden **`ShowPopupAsync`** fra .NET MAUI Community Toolkit. Vi awaiter resultatet og sikrer, at det er `True` (hvilket det er, hvis brugeren har ændret sit valg før lukning).

Er resultatet `True`, ryddes `Genres`-collection'en og genudfyldes med den opdaterede `_genreList`, hvorefter `LoadFilteredMovies` kaldes.

#### Listing 6.22 The new code in MainPage.xaml.cs

```csharp
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace MauiMovies;

public partial class MainPage : ContentPage
{
    // ...

    public ICommand ChooseGenres => new Command(async () => await ShowGenreList());

    // ...

    private async Task ShowGenreList()
    {
        var genrePopup = new GenreListPopup(_genreList);           // ①

        var selected = await this.ShowPopupAsync(genrePopup);      // ②

        if ((bool)selected)
        {
            Genres.Clear();

            foreach (var genre in _genreList)
            {
                if (genre.Selected)
                {
                    Genres.Add(new Genre
                    {
                        name = genre.name
                    });
                }
            }

            LoadFilteredMovies();
        }
    }
}
```

① Instantierer en ny `GenreListPopup` og sender `_genreList` til dens konstruktor.
② Viser popup'en ved at kalde `ShowPopupAsync`-extension methoden med `GenreListPopup`-instansen og awaiter resultatet.

**Figure 6.9** viser MauiMovies på Windows med popup'en vist af `ShowPopupAsync`. En `CollectionView` viser listen af genrer, og at vælge en eller flere trigger `SelectionChanged`-eventet. At lukke popup'en — enten ved at tappe Confirm eller ved at tappe på baggrunden udenfor — returnerer `_selectionChanged`-værdien.

**Figure 6.10** viser resultatet: brugerens genrevalg vises i Genres-boksen, og kun film der matcher kriterierne vises.

#### Listing 6.23 The final changes to MainPage.xaml.cs

```csharp
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace MauiMovies;

public partial class MainPage : ContentPage
{
    ...

    public ICommand ShowMovie => new Command<MovieResult>((movie) => ShowMovieDetails(movie));   // ①

    ...

    private void ShowMovieDetails(MovieResult movie)
    {
        var moviePopup = new MovieDetailsPopup(movie, _genres.genres);
        this.ShowPopup(moviePopup);
    }
}
```

① Opdaterer `ShowMovie`-`ICommand`'en med en expression body til at være en `Command` typed til `MovieResult`, som kalder `ShowMovieDetails`-metoden.

Til sidst skal `TapGestureRecognizer`'en på `VerticalStackLayout` for hver film opdateres. **Den binding, vi har sat på `Command`-propertyen, virker ikke:** vi har bundet til `ShowMovie`-metoden, men den findes på `MainPage`-klassen — ikke på `MovieResult`-klassen, som er binding context for item'et i det bindable layout.

Vi løser det med **`Source`-markup extension** og en reference til siden. Da `Command` nu er typed til `MovieResult`, som vi også skal bruge til `ShowMovieDetails`, har vi brug for en **`CommandParameter`** — den er let, da den blot sættes til bindingen til selve item'et.

#### Listing 6.24 The final changes to MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Name="MoviePage"
             x:Class="MauiMovies.MainPage">

    <VerticalStackLayout Spacing="10" Padding="30">
        ...
        <ScrollView>
            <FlexLayout BindableLayout.ItemsSource="{Binding Movies}"
                        JustifyContent="SpaceEvenly"
                        Wrap="Wrap">
                <BindableLayout.ItemTemplate>
                    <DataTemplate>
                        <VerticalStackLayout WidthRequest="200"
                                             Margin="30"
                                             Spacing="10">

                            <VerticalStackLayout.GestureRecognizers>
                                <TapGestureRecognizer
                                    NumberOfTapsRequired="1"
                                    Command="{Binding Source={x:Reference MoviePage}, Path=ShowMovie}"
                                    CommandParameter="{Binding .}"/>
                            </VerticalStackLayout.GestureRecognizers>
        ...
        </VerticalStackLayout>
</ContentPage>
```

① Opdaterer bindingen for denne `Command` med `Source`-markup extension til at referere siden, og `Path` til at pege på `ShowMovie`-propertyen.
② Binder `CommandParameter`-propertyen til selve item'et, som er en `MovieResult`.

**Figure 6.11** viser MauiMovies på Windows, hvor brugeren har tappet på en af filmene for at få `MovieDetailsPopup` frem.

## 6.3 Absolute layout

`AbsoluteLayout` lader dig bruge eksplicitte værdier til at positionere views på skærmen. Det kan være nyttigt, men man skal være forsigtig: alles skærm har forskellig højde og bredde, så selv med eksplicitte værdier kan du ikke garantere et views absolutte position.

**Figure 6.12** viser konsekvensen: en `BoxView` er tilføjet med absolut position og absolut størrelse. Den fylder næsten hele skærmen på en mindre enhed, men kun et lille hjørne øverst til venstre på en større skærm.

`AbsoluteLayout` bruger to attached properties, **`LayoutBounds`** og **`LayoutFlags`**, så et view kan positionere sig inden i et `AbsoluteLayout`.

- `LayoutBounds` er af typen **`Rect`** og har derfor fire properties: `x`, `y`, `width` og `height`. De første to specificerer view'ets øverste venstre position i `AbsoluteLayout`'et; de sidste to definerer størrelsen. Default-værdierne er **`0, 0, Auto, Auto`**, hvilket betyder, at tilføjer du et view som child til et `AbsoluteLayout` uden `LayoutBounds`, placeres det øverst til venstre og størrelsesbestemmes automatisk efter sit indhold.
- **Trods navnet bliver `AbsoluteLayout` mere nyttigt, når du bruger det til proportional størrelse og positionering.** `LayoutFlags`-propertyen er en enum, der lader dig angive, hvilke `LayoutBounds` der er proportionale og hvilke der er absolutte. Du kan deklarere at `x`, `y` eller begge (position) er proportionale; at `height`, `width` eller begge (size) er proportionale; at alle værdier er proportionale; eller at ingen er.

**Figure 6.13** viser dette: position og størrelse af `BoxView`'en er proportionale i forhold til `AbsoluteLayout`'et, hvilket gør det lettere at opnå den ønskede effekt (her en `BoxView` der fylder det meste af skærmen).

Opret en ny .NET MAUI-app fra `blankmaui`-templaten kaldet **MauiFab**, og wrap hele indholdet af `MainPage` i et `AbsoluteLayout`.

#### Listing 6.25 MainPage.xaml in MauiFab

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="FabMaui.MainPage">
    <AbsoluteLayout>
        <ScrollView>
            ...
        </ScrollView>
    </AbsoluteLayout>
</ContentPage>
```

① Et `AbsoluteLayout` er tilføjet som sidens `Content`-property, med `ScrollView`'et nu tilføjet som child.

Vi tilføjer en simpel **floating action button (FAB)**: en `Button` med højde og bredde 100 og en corner radius på 50 for at gøre den cirkulær. Vi sætter `LayoutFlags` til `PositionProportional`, så den altid vises i samme relative position, og `LayoutBounds` med x og y begge på 0.9. Da positionen er proportional, er 0.9 = 9/10 hen over (for x) og ned (for y), så den placeres nederst til højre.

#### Listing 6.26 MainPage.xaml with the FAB added

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="FabMaui.MainPage">
    <AbsoluteLayout>
        <ScrollView>
            ...
        </ScrollView>

        <Button CornerRadius="50"
                Text="Fab!"
                FontSize="Large"
                AbsoluteLayout.LayoutBounds="0.9,0.9,100,100"
                AbsoluteLayout.LayoutFlags="PositionProportional"/>

    </AbsoluteLayout>
</ContentPage>
```

① Sætter knappens `LayoutBounds`: 0.9 for x, 0.9 for y, 100 for bredde og 100 for højde.
② Sætter knappens `LayoutFlags` til `PositionProportional`, så positionen er proportional i forhold til `AbsoluteLayout`'et, mens højde og bredde er absolutte.

**Figure 6.14** viser resultatet: indholdet af `MainPage` er wrappet i et `AbsoluteLayout`, som lader os placere en FAB 90 % nede og hen over skærmen — men nu er resten af indholdet placeret øverst til venstre.

**Figure 6.15** viser, at resizing af vinduet flytter FAB'en i forhold til det øvrige indhold, men bevarer dens position relativt til `AbsoluteLayout`'et (som resizer med vinduet).

Grunden til at `ScrollView`'et nu ligger øverst til venstre er, at det er inden i et `AbsoluteLayout` uden angivne `LayoutBounds` og derfor antager default-værdierne 0 for x og y og automatisk størrelse.

Du vil formentlig bruge `AbsoluteLayout` sjældnere end andre layouts, da det meste, du kan opnå med det, også er muligt med andre layouts. Men det er et kraftfuldt værktøj at have.

## 6.4 Putting it all together

Vi sætter os i stolen som UI-ingeniør hos Microsoft, der har modtaget designet til Microsoft Outlook mobile-appen, og ser hvordan vi kan bruge de lærte layouts til at arrangere de kendte controls og implementere designet. **Figure 6.16** viser Microsoft Outlook kørende på iOS.

Vi bygger ikke nogen af Outlooks funktionalitet — vi genskaber blot UI'et på Inbox-skærmen for at skærpe vores layout-færdigheder.

> **UI challenges**
> En UI challenge er, hvor du tager et eksisterende UI-design og bygger det i .NET MAUI. En almindelig tilgang er at replikere UI'et fra en velkendt eksisterende app (som her). En anden er at finde et sjovt og interessant koncept-design og bygge det; Dribbble.com bruges ofte som inspiration.
>
> UI challenges er en populær måde at holde sine UI-færdigheder skarpe. De hjælper dig med at identificere og løse UI-problemer og udbygge de UI-værktøjer, du har i din mentale værktøjskasse.

### 6.4.1 Adding the app's shared resources

Opret en ny .NET MAUI-app fra `blankmaui`-templaten kaldet **OutlookClone**.

Outlook bruger Microsofts **Fluent Design system**, som inkluderer et ikon-sæt (https://github.com/microsoft/fluentui-system-icons). I OutlookClone bruger vi **fonts, der indeholder alle symbolerne** — altså font icons.

Importér `FluentSystemIcons-Filled.ttf` og `FluentSystemIcons-Regular.ttf` til `Resources/Fonts`-mappen og registrér dem i `MauiProgram.cs`.

#### Listing 6.27 MauiProgram.cs

```csharp
namespace OutlookClone
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentFilled");    // ①
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentRegular");  // ②
                });

            return builder.Build();
        }
    }
}
```

① Registrerer `FluentSystemIcons-Filled.ttf`-fontassetet med navnet `FluentFilled`.
② Registrerer `FluentSystemIcons-Regular.ttf`-fontassetet med navnet `FluentRegular`.

Fontsene er nu registreret som applikationsdækkende resources. Vi har også brug for andre applikationsdækkende resources: **farver**. Vi kunne angive farverne individuelt for hver control, men det er besværligt og unødvendigt. Vi registrerer dem i `Resources/Styles/Colors.xaml`.

#### Listing 6.28 Colors.xaml

```xml
<ResourceDictionary ...>
    <Color x:Key="Primary">#0878d3</Color>
    <Color x:Key="Secondary">#f1edec</Color>
    <Color x:Key="Tertiary">#717171</Color>
</ResourceDictionary>
```

① Opdaterer "Primary"-farven med den angivne hex-værdi.
② Opdaterer "Secondary"-farven.
③ Opdaterer "Tertiary"-farven.

Hver af disse farver er nu tilgængelige som `StaticResource` overalt i appen.

### 6.4.2 Defining the UI as a Grid

**Figure 6.17** viser Outlook Inbox-UI'et nedbrudt som rows i et `Grid` (status bar og safe area ignoreres). Der er **fire rows**:

- Øverste row: titel og search.
- Næste row: focused inbox-switch og filter-knappen.
- Nederst: tab bar.
- Mellem anden og fjerde row: listen af beskeder, som fylder al resterende plads.

I en virkelig app ville vi bruge Shell eller en `TabbedPage` til fanerne (kapitel 7), men her bygger vi dem direkte ind i siden, da vi kun replikerer UI'et.

Ved måling giver en tilstrækkelig approksimation row-højderne **50, 40, `*` og 80**.

#### Listing 6.29 MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
    </Grid>
</ContentPage>
```

① Tilføjer et `Grid` med row-højderne 50, 40, `*` og 80.

### 6.4.3 Creating the title bar with FlexLayout

**Figure 6.18** viser tilgangen: vi bruger et `FlexLayout` til den øverste row med `JustifyContent="SpaceBetween"` til at placere child views i hver sin side. Til venstre en `HorizontalStackLayout` med to `Label`s (én til ikonet og én til titlen), og til højre en `Label` til search-ikonet.

Ved brug af font icons bruger vi **glyphs** frem for ASCII- eller Unicode-tegn.

**Table 6.1 — Glyphs:**

| Icon | Font | Glyph code |
| --- | --- | --- |
| Home | `FluentFilled` | `fa38` |
| Search | `FluentRegular` | `fb26` |
| Filter | `FluentRegular` | `f408` |
| Mail | `FluentFilled` | `f513` |
| Calendar | `FluentRegular` | `03de` |

At bruge disse fonts i XAML er let: sæt `FontFamily`-propertyen på en `Label` til den ønskede font og `Text`-propertyen til glyphens kode. **Ved brug af koderne i XAML skal de præfixes med `&#x` og afsluttes med `;`.**

#### Listing 6.30 MainPage.xaml with the top row layout added

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">

        <FlexLayout Grid.Row="0"
                    HorizontalOptions="FillAndExpand"
                    VerticalOptions="FillAndExpand"
                    BackgroundColor="{StaticResource Primary}"
                    JustifyContent="SpaceBetween">

            <HorizontalStackLayout Margin="5,0,0,0"
                                   Spacing="10">
                <Label Text="&#xfa38;"
                       FontFamily="FluentFilled"
                       TextColor="{StaticResource Primary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="StartAndExpand"
                       VerticalTextAlignment="Center"
                       BackgroundColor="White"
                       VerticalOptions="Center"
                       WidthRequest="30"
                       HeightRequest="30"
                       FontSize="Large"/>
                <Label Text="Inbox"
                       VerticalTextAlignment="Center"
                       HorizontalOptions="StartAndExpand"
                       TextColor="White"
                       FontAttributes="Bold"
                       FontSize="Large"/>
            </HorizontalStackLayout>

            <Label Text="&#xfb26;"
                   FontFamily="FluentRegular"
                   TextColor="White"
                   VerticalOptions="Center"
                   HorizontalOptions="EndAndExpand"
                   HorizontalTextAlignment="End"
                   WidthRequest="40"
                   FontSize="Large"
                   Margin="0,0,5,0"/>

        </FlexLayout>
    </Grid>
</ContentPage>
```

Home-ikonet ville være firkantet, men i Outlook er det rundt. Vi kan let **klippe** `Label`'en rund.

#### Listing 6.31 The updated home icon

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        <FlexLayout ...>
            <HorizontalStackLayout ...>

                <Label Text="&#xfa38;"
                       FontFamily="FluentFilled"
                       TextColor="{StaticResource Primary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="StartAndExpand"
                       VerticalTextAlignment="Center"
                       BackgroundColor="White"
                       VerticalOptions="Center"
                       WidthRequest="30"
                       HeightRequest="30"
                       FontSize="Large">
                    <Label.Clip>
                        <EllipseGeometry RadiusX="15"
                                         RadiusY="15"
                                         Center="15,15"/>
                    </Label.Clip>
                </Label>

                <Label .../>
            </HorizontalStackLayout>
            <Label .../>
        </FlexLayout>
    </Grid>
</ContentPage>
```

### 6.4.4 Creating the filter bar with FlexLayout

**Figure 6.19** viser anden row: også et `FlexLayout` med `SpaceBetween`. Til venstre focused inbox-switch-controlen (her erstattet med en standard `Switch` og en `Label`, da den originale er en custom control), til højre en `HorizontalStackLayout` med to `Label`s: én til ikonet og én til ordet "Filter".

#### Listing 6.32 The second row of the Grid

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">

        <FlexLayout ...>
            ...
        </FlexLayout>

        <FlexLayout Grid.Row="1"
                    HorizontalOptions="FillAndExpand"
                    VerticalOptions="FillAndExpand"
                    BackgroundColor="{StaticResource Primary}"
                    Padding="20,5"
                    JustifyContent="SpaceBetween">

            <HorizontalStackLayout Margin="5,0,0,0">
                <Label Text="Focused"
                       TextColor="White"
                       VerticalOptions="Center"/>
                <Switch/>
            </HorizontalStackLayout>

            <HorizontalStackLayout Margin="0,0,5,0"
                                   Spacing="10">
                <Label Text="&#xf408;"
                       FontFamily="FluentRegular"
                       TextColor="White"
                       VerticalOptions="Center"
                       HorizontalOptions="EndAndExpand"
                       HorizontalTextAlignment="End"
                       WidthRequest="40"
                       FontSize="Large"/>
                <Label Text="Filter"
                       TextColor="White"
                       VerticalOptions="Center"
                       HorizontalOptions="EndAndExpand"
                       HorizontalTextAlignment="End"/>
            </HorizontalStackLayout>

        </FlexLayout>

    </Grid>
</ContentPage>
```

① Sætter `JustifyContent` til `SpaceBetween`, så child-items placeres i begyndelsen og slutningen af rowen.

**Figure 6.20** viser OutlookClone-header-sektionen: to rows arrangeret med et `Grid`, hvor rowene selv bruger `FlexLayout` og `HorizontalStackLayout` til at positionere elementer.

> **NOTE** Slet al kode fra `MainPage.xaml.cs` bortset fra konstruktoren, før du kører appen.

### 6.4.5 Using Grid to create a FAB

**Figure 6.21** viser tredje row: en `CollectionView` er et oplagt valg til beskedforhåndsvisningerne, men der er også en FAB-knap nederst til højre. Den kan ligge i samme row (der er kun én column), og vi bruger `HorizontalOptions` og `VerticalOptions` til at placere den i slutningen.

Da vi bruger en `Button`, som har en `CornerRadius`-property, behøver vi ikke clipping for at gøre den rund. Den får også en `Shadow` for at efterligne den rigtige app.

#### Listing 6.33 The FAB

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        <FlexLayout ...>
            ...
        </FlexLayout>
        <FlexLayout ...>
            ...
        </FlexLayout>

        <Button Grid.Row="2"
                BackgroundColor="{StaticResource Primary}"
                HorizontalOptions="EndAndExpand"
                VerticalOptions="EndAndExpand"
                Margin="20"
                HeightRequest="60"
                WidthRequest="60"
                CornerRadius="30"
                FontSize="30"
                Text="+">
            <Button.Shadow>
                <Shadow Brush="Black"
                        Offset="5,5"
                        Radius="10"
                        Opacity="0.5"/>
            </Button.Shadow>
        </Button>

    </Grid>
</ContentPage>
```

### 6.4.6 Building a tab bar with Grid

**Figure 6.22** viser sidste row, tab baren: endnu et `Grid` med **tre columns**. I hver column bruges en `VerticalStackLayout` med to `Label`s som child-items: én til tab-ikonet og én til tab-labelen.

#### Listing 6.34 The tab bar

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        <FlexLayout ...>
            ...
        </FlexLayout>
        <FlexLayout ...
            ...
        </FlexLayout>
        <Button ...>
            ...
        </Button>

        <Grid Grid.Row="3"
              HorizontalOptions="FillAndExpand"
              VerticalOptions="FillAndExpand"
              ColumnDefinitions="*,*,*"
              Padding="5"
              BackgroundColor="{StaticResource Secondary}">
        </Grid>

    </Grid>
</ContentPage>
```

#### Listing 6.35 The first tab

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        <FlexLayout ...>
            ...
        </FlexLayout>
        <FlexLayout ...
            ...
        </FlexLayout>
        <Button ...>
            ...
        </Button>

        <Grid ...>
            <VerticalStackLayout HorizontalOptions="Center"
                                 Grid.Column="0">
                <Label Text="&#xf513;"
                       FontFamily="FluentFilled"
                       TextColor="{StaticResource Primary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Center"
                       VerticalTextAlignment="Center"
                       VerticalOptions="Center"
                       WidthRequest="30"
                       HeightRequest="30"
                       FontSize="30"/>
                <Label Text="Email"
                       TextColor="{StaticResource Primary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Center"
                       VerticalTextAlignment="Center"
                       VerticalOptions="Center"
                       FontSize="11"/>
            </VerticalStackLayout>
        </Grid>

    </Grid>
</ContentPage>
```

De to øvrige faner er kopier med ændret column, glyph og tekstfarve.

#### Listing 6.36 The final two tabs

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        <FlexLayout ...>
            ...
        </FlexLayout>
        <FlexLayout ...
            ...
        </FlexLayout>
        <Button ...>
            ...
        </Button>

        <Grid ...>
            <VerticalStackLayout ...>
                ...
            </VerticalStackLayout>

            <VerticalStackLayout HorizontalOptions="Center"
                                 Grid.Column="1">
                <Label Text="&#xfb26;"
                       FontFamily="FluentRegular"
                       TextColor="{StaticResource Tertiary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Center"
                       VerticalTextAlignment="Center"
                       VerticalOptions="Center"
                       WidthRequest="30"
                       HeightRequest="30"
                       FontSize="30"/>
                <Label Text="Search"
                       TextColor="{StaticResource Tertiary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Center"
                       VerticalTextAlignment="Center"
                       VerticalOptions="Center"
                       FontSize="11"/>
            </VerticalStackLayout>

            <VerticalStackLayout HorizontalOptions="Center"
                                 Grid.Column="2">
                <Label Text="&#x03de;"
                       FontFamily="FluentRegular"
                       TextColor="{StaticResource Tertiary}"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Center"
                       VerticalTextAlignment="Center"
                       VerticalOptions="Center"
                       WidthRequest="30"
                       HeightRequest="30"
                       FontSize="30"/>
                <Label Text="Calendar"
                       TextColor="{StaticResource Tertiary}"
                       FontSize="11"
                       HorizontalTextAlignment="Center"
                       HorizontalOptions="Center"
                       VerticalTextAlignment="Center"
                       VerticalOptions="Center"/>
            </VerticalStackLayout>
        </Grid>

    </Grid>
</ContentPage>
```

**Figure 6.23** viser OutlookClone med layoutet færdigt, blot uden beskederne.

### 6.4.7 Populating dummy data

Vi bygger ikke en email-klient, så vi simulerer data med et API, der returnerer tilfældige citater fra The Simpsons: https://thesimpsonsquoteapi.glitch.me. Vi bruger navnet som afsender, billedet som avatar, og citatet som både besked og emne.

#### Listing 6.37 The Simpson class

```csharp
namespace OutlookClone;

public class Simpson
{
    public string quote { get; set; }
    public string character { get; set; }
    public string image { get; set; }
    public string characterDirection { get; set; }
}
```

#### Listing 6.38 MainPage.xaml.cs

```csharp
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace OutlookClone
{
    public partial class MainPage : ContentPage
    {
        private string contentUri = "https:/ /thesimpsonsquoteapi.glitch.me/quotes?count=20";

        public ObservableCollection<Simpson> Simpsons = new();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var httpClient = new HttpClient();
            var jsonResponse = await httpClient.GetFromJsonAsync<List<Simpson>>(contentUri);
            jsonResponse.ForEach(s => Simpsons.Add(s));
        }
    }
}
```

### 6.4.8 Displaying the messages with CollectionView

Tilføj en `CollectionView` til row 2 af sidens top-level `Grid`. **Den skal tilføjes *før* `Button`en i samme row** — ellers renderes `CollectionView`'et oven på knappen. (Man kan også justere z-index på elementerne, men for et simpelt UI som dette er det lettere at tilføje dem i den rigtige rækkefølge.)

#### Listing 6.39 MainPage.xaml with the CollectionView added

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        <FlexLayout ...>
            ...
        </FlexLayout>
        <FlexLayout ...>
            ...
        </FlexLayout>

        <CollectionView Grid.Row="2"
                        x:Name="MessageCollection"
                        HorizontalOptions="Fill"
                        VerticalOptions="Fill">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

        <Button ...>
            ...
        </Button>
        <Grid ...>
            ...
        </Grid>
    </Grid>
</ContentPage>
```

**Figure 6.24** beskriver message-templaten: et `Grid` med **tre columns** (første og sidste med bredde 50, den midterste tager resten) og **tre rows**. Avataren ligger i første column og row med `RowSpan` 3 (der er intet andet i første column). Afsenderens navn i første row, anden column. Emnet i anden row, anden column. Preview af beskedteksten i tredje row, anden column, spændende ind i tredje column. Tidspunkt eller dag i første row, tredje column.

#### Listing 6.40 MainPage.xaml with the message template

```xml
<CollectionView ...>
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="50,*,50"
                  RowDefinitions="25,20,40"
                  HorizontalOptions="Fill"
                  VerticalOptions="Fill"
                  Padding="10,5,20,5">

                <Image WidthRequest="40"
                       HeightRequest="40"
                       Grid.RowSpan="3"
                       VerticalOptions="Start"
                       HorizontalOptions="Start"
                       Aspect="AspectFill"
                       Source="{Binding image}">
                    <Image.Clip>
                        <EllipseGeometry RadiusX="20"
                                         RadiusY="20"
                                         Center="20,20"/>
                    </Image.Clip>
                </Image>

                <Label Grid.Row="0"
                       Grid.Column="1"
                       Text="{Binding character}"
                       FontSize="18"
                       FontAttributes="Bold"
                       TextColor="Black"/>

                <Label Grid.Row="1"
                       Grid.Column="1"
                       Text="{Binding quote}"
                       LineBreakMode="TailTruncation"
                       VerticalOptions="Start"
                       TextColor="Black"/>

                <Label Grid.Row="2"
                       Grid.Column="1"
                       Grid.ColumnSpan="2"
                       Text="{Binding quote}"
                       LineBreakMode="WordWrap"
                       VerticalOptions="Start"
                       TextColor="{StaticResource Tertiary}"/>

                <Label Grid.Row="0"
                       Grid.Column="2"
                       Text="Saturday"
                       FontSize="12"
                       TextColor="{StaticResource Tertiary}"/>

            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

Til sidst tilføjes en `ActivityIndicator` til sidens top-level `Grid` i row 2 for at vise, at data loader.

#### Listing 6.41 MainPage.xaml with the ActivityIndicator

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="OutlookClone.MainPage">
    <Grid RowDefinitions="50,40,*,80">
        ...
        <ActivityIndicator Grid.Row="2"
                           Color="{StaticResource Primary}"
                           IsRunning="True"
                           IsEnabled="True"
                           VerticalOptions="Center"
                           HorizontalOptions="Center"
                           x:Name="LoadingIndicator"/>
    </Grid>
</ContentPage>
```

#### Listing 6.42 The complete MainPage.xaml.cs

```csharp
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace OutlookClone
{
    public partial class MainPage : ContentPage
    {
        private string contentUri = "https:/ /thesimpsonsquoteapi.glitch.me/quotes?count=20";

        public ObservableCollection<Simpson> Simpsons = new();

        public MainPage()
        {
            InitializeComponent();
            MessageCollection.ItemsSource = Simpsons;
        }

        protected override async void OnAppearing()
        {
            LoadingIndicator.IsVisible = true;

            base.OnAppearing();

            var httpClient = new HttpClient();
            var jsonResponse = await httpClient.GetFromJsonAsync<List<Simpson>>(contentUri);
            jsonResponse.ForEach(s => Simpsons.Add(s));

            LoadingIndicator.IsVisible = false;
        }
    }
}
```

**Figure 6.25** viser den færdige OutlookClone-app.

Der er flere ting, man kunne gøre videre med appen: tilføje `SwipeView` til beskederne, gøre fanerne til en templated control (kapitel 8), eller — hvis man er ambitiøs — replikere den originale focused inbox-switch.

## Summary

- `Grid` er et alsidigt layout, du kan bruge til næsten ethvert UI. Du kan bygge komplekse layouts med et `Grid`, som SSW.Rewards eller endda Microsoft Outlook — ikke kun simple rows og columns som MauiCalc.
- Med `BindableLayout` kan du bruge en data source til at tilføje child-items til ethvert layout via en `DataTemplate`, ligesom med `CollectionView`.
- `FlexLayout` er et godt layout at bruge med `BindableLayout`. `HorizontalStackLayout` og `VerticalStackLayout` er også gode kandidater — men `Grid` er ikke et fornuftigt valg til dette.
- Med `AbsoluteLayout` får du præcis kontrol over, hvor ting placeres på skærmen, og hvordan de størrelsesbestemmes. Inden i et `AbsoluteLayout` kan du bruge proportional eller absolut sizing og positionering til at arrangere views.
