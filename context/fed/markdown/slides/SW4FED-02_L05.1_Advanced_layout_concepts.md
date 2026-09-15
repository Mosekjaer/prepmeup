# Avancerede layout-koncepter

## Metadata

- **Lektion:** L05.1 – Advanced layout concepts
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L05/Advanced layout concepts.pdf (17 slides)
- **Emner dækket:**
  - "Thinking in grids" — at nedbryde et design til nested Grids
  - Proportionale row- og column-definitioner
  - BindableLayout som collection view på et vilkårligt layout
  - MauiMovies-appens layout med FlexLayout og BindableLayout
  - Installation og registrering af .NET MAUI Community Toolkit
  - Popup-sider med CommunityToolkit.Maui
  - Åbning af en popup med `ShowPopupAsync`

---

## 1. Thinking in grids

Grid er et kraftfuldt layout, der kan meget mere end blot at vise firkanter og rektangler.

Første skridt i at bygge et design er at identificere det **øverste Grid**. Eksempeldesignet i slidesene kan brydes ned i tre rows:

- en header-sektion
- en body-sektion
- en bio-sektion

Og den øverste row har tre columns.

### Implementering af det øverste Grid

Man kan tilnærme de relative størrelser ud fra designbilledet — eller man har måske fået designet fra UX-teamet i et design collaboration tool og kender de præcise relative proportioner. I dette tilfælde svarer rows til `2*`, `9*` og `3*`.

Kun den øverste row bruger columns, men det er ikke et problem: de to øvrige rows sættes til `ColumnSpan="3"`. Navigationskontrollerne placeres så i column 0 og 2 i row 0. Column-definitionerne er `*, 5*, *`.

### Developer details-sektionen

Den indre sektion har to lige store columns:

- Billedet fylder alle tre rows i første column og får desuden `ColumnSpan="2"`, så billedet kan overlappe andre items.
- De resterende items ligger i anden column: skills summary i første row, QR-ikonet i anden, og de sociale interaktionsknapper i tredje.

De relative column-bredder er lette: de deler bredden ligeligt, så begge er `*`. For rows kan man ud fra designet aflæse de relative højder `5*`, `2*` og `*`.

### Kodeeksempel

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             >
    <ContentPage.Content>
        <Grid RowDefinitions="2*, 9*, 3*"
              ColumnDefinitions="*, 5*, *">
            <Grid Grid.Row="1"
                Grid.Column="0"
                Grid.ColumnSpan="3"
                ColumnDefinitions="*, *"
                RowDefinitions="5*, 2*, *"
            >
        </Grid>
    </ContentPage.Content>
</ContentPage>
```

<!-- uklart i kilden: det indre Grid mangler sit lukkende tag i slidet -->

## 2. BindableLayout

`BindableLayout` er en static class, der kan attaches til et hvilket som helst layout og lade det generere sit eget indhold. Den forvandler ethvert layout til et collection view.

Med BindableLayout kan man angive en `ItemsSource`-property for et layout og derefter bruge en `DataTemplate` til at rendere hvert item i collectionen — præcis som med `CollectionView`.

Det fungerer godt med `HorizontalStackLayout` og `VerticalStackLayout` og især godt med `FlexLayout`. Det giver ikke meget mening at bruge BindableLayout med `Grid`, da views i et Grid har brug for Row- og Column-properties.

## 3. Layout for MauiMovies-appen

Wireframen for MauiMovies viser:

- En liste af trending movies med poster, titel og rating.
- Øverst på skærmen en liste af genres, som brugeren har valgt at filtrere efter.
- Brugeren kan tappe på denne liste for at ændre sit valg.

### Implementering af wireframen

Hovedlayoutet for siden er et `VerticalStackLayout`, som arrangerer child views lodret fra top til bund.

Det første view, der tilføjes, er et `FlexLayout`, der viser de valgte genres. Derefter tilføjes endnu et `FlexLayout`, der viser den filtrerede liste af film.

Vi bruger altså BindableLayout med to FlexLayouts i MauiMovies: ét til genre-listen og ét til film-listen.

### Kodeeksempel: genre-listen

```xml
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
```

## 4. Popup-side med .NET MAUI Community Toolkit

.NET MAUI Community Toolkit har et brugbart popup view. Første skridt er at installere NuGet-pakken `CommunityToolkit.Maui` i projektet.

Før den kan bruges, skal den registreres i `MauiProgram.cs` ved at kæde `UseMauiCommunityToolkit()` på builderen:

```csharp
builder
    .UseMauiApp<App>()
    .UseMauiCommunityToolkit();
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    })
```

<!-- Bemærk: slidet har et semikolon efter UseMauiCommunityToolkit(), hvilket ville bryde chainingen. Gengivet som i kilden. -->

**Obs — versionsmatch:**

- Til .NET version 8 bruges CommunityToolkit.Maui version 9
- Til .NET version 9 bruges CommunityToolkit.Maui version 11
- Til .NET version 10 bruges CommunityToolkit.Maui version 14

### Oprettelse af popup-siden

Den nemmeste måde at lave en Popup page på er at tilføje en .NET MAUI `ContentPage` via templaten og modificere den.

Åbn `GenreListPopup.xaml.cs` og skift den nedarvede type fra `ContentPage` til `Popup`. Du skal bruge namespacet `CommunityToolkit.Maui.Views`. Der vil nu være fejl på grund af mismatch med XAML'en, så skift over til `GenreListPopup.xaml` og ret den til.

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
```

<!-- kodeeksemplet er afkortet i slidet -->

### Definition af popup'en i XAML

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

## 5. Åbning af en popup

`TapGestureRecognizer` binder til et `ICommand` på view modellen:

```xml
<TapGestureRecognizer NumberOfTapsRequired="1"
                      Command="{Binding ChooseGenres}"/>
```

Og kommandoen kalder en async metode, der viser popup'en med `ShowPopupAsync` og bruger resultatet:

```csharp
public ICommand ChooseGenres => new Command(async () => await ShowGenreList());

private async Task ShowGenreList()
{
    var genrePopup = new GenreListPopup(_genreList);

    var selected = await this.ShowPopupAsync(genrePopup);

    if ((bool)selected)
    {
        Genres.Clear();
        foreach (var genre in _genreList)
        {
            // Code missing
}
```

<!-- kodeeksemplet er ufuldstændigt i slidet ("Code missing" er skrevet af underviseren) -->

Slidesene afsluttes med screenshots af den kørende app og af popup'en i brug.

## 6. Referencer og links

- *.NET MAUI in Action*
- .NET MAUI Community Toolkit — https://github.com/CommunityToolkit/Maui
- .NET MAUI Community Toolkit documentation, Microsoft Learn
