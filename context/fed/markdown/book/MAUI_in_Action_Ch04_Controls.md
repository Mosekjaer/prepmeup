# Kapitel 4 – Controls

## Metadata

- **Kapitel:** 4 – Controls
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L03
- **Hovedemner:**
  - Definitionen af *view*: controls, layouts og pages i .NET MAUI's hierarki
  - Cross-platform controls som abstraktioner af native platform-implementeringer
  - Controls til at vise information (`Label`, `ProgressBar`, `ActivityIndicator`)
  - Controls til at modtage input (`Entry`, `Editor`, `CheckBox`, `DatePicker`, `Slider`, `Stepper`, `TimePicker`, `RadioButton`, `Picker`)
  - Controls til at modtage commands (`Button`, `ImageButton`, `SearchBar`) og `ICommand`
  - Grafik: `Image`, `Shapes`, `GraphicsView`
  - Templated views til collections: `CollectionView`, `ListView`, `CarouselView`, `TableView`
  - Fælles properties: `Height`/`Width` vs. `HeightRequest`/`WidthRequest`, device-independent units (DIUs)
  - Control modifiers: clipping (`Clip`, geometrier), `Border`, `Shadow`
  - Gesture recognizers, `RefreshView` og `SwipeView`

---

## Indledning

**Controls** er views, der enten direkte renderer noget på skærmen (som et `Image` eller en `Label`) eller modtager input fra en bruger (som en `CheckBox` eller `DatePicker`). Cirka 30 controls leveres med .NET MAUI ud af boksen og dækker alle gængse use cases for UI-applikationer.

Derudover leverer .NET MAUI Community Toolkit samt gratis og kommercielle UI kits en række yderligere controls og stiliserede eller tilpasselige versioner af de indbyggede controls. Man kan naturligvis også bygge sine egne controls (kapitel 11).

Nogle properties er fælles på tværs af alle controls, og der findes supplerende controls, som forfatteren kalder **control modifiers**, der kan anvendes på enhver control.

## 4.1 What do we mean by "views"?

Views i et UI er de ting, der vises på skærmen. I modsætning til andre dele af en app (fx en service) er views det, brugerne ser og interagerer med.

I en .NET MAUI-app findes **tre typer view: controls, layouts og pages**. **Figure 4.1** viser hierarkiet: alt på skærmen i en UI-app er et view og skal være indeholdt i en page (som, da den er på skærmen, også er et view). En page fylder hele skærmen eller vinduet og kan navigeres til eller instantieres. **I .NET MAUI kan en page kun indeholde ét child** — i figurens tilfælde en `VerticalStackLayout`, som viser sine child-items vertikalt. Layouts kan indeholde et vilkårligt antal child-items, som kan være controls (fx `Button` eller `Label`) eller andre layouts (fx `Grid` eller `HorizontalStackLayout`).

En .NET MAUI-app ved ikke, hvordan den direkte renderer et layout eller en control, men den ved, hvordan den viser en page på skærmen. En page ved til gengæld, hvordan den renderer et layout, og layouts ved, hvordan de renderer controls. **Figure 4.2** illustrerer dette med et eksempel: en app renderer en home page med et `Grid`-layout som child. `Grid`'et har fem child-items, herunder en `VerticalStackLayout` i midten, som har tre controls som children: et `Image`, en `Button` og en `Entry`.

> **Important note on terms**
> I den officielle .NET MAUI-dokumentation bruges **control** som paraplybetegnelse for alt, hvad man ser på skærmen — inklusive pages, layouts og interaktive/visnings-elementer som buttons og images — mens **view** bruges om selve disse elementer.
>
> For at gøre konceptet lettere at forstå har forfatteren byttet om og brugt branchens gængse definitioner: **view** som paraplybetegnelse og **controls** om de interaktive og visningsmæssige elementer (definitionen af pages er uændret). Det er vigtigt at kende de officielle termer, især når man slår op i dokumentationen.

## 4.2 Cross-platform controls

.NET MAUI leverer ud af boksen en samling controls, der dækker et bredt spektrum af use cases. Disse controls er **abstraktioner af deres native platform-implementeringer**: vil man modtage tekstinput fra en bruger, bruger man en `Entry`, som ser ud som en iOS-control på iOS, en Windows-control på Windows osv., uden at skulle implementere den samme control én gang pr. platform. Dette er "consistent, but not identical"-tilgangen fra kapitel 1.

### 4.2.1 Displaying information

Disse controls præsenterer information for brugeren og modtager ikke input.

**Table 4.1 — Controls til at vise information:**

| Control | Primary data type | Primary data type name | Use case |
| --- | --- | --- | --- |
| `Label` | String | `Text` | Viser tekst på skærmen. Vi har brugt `Label`s i hver app indtil nu; det er næsten umuligt at bygge en app uden dem. |
| `ProgressBar` | Double | `Progress` | Viser en værdi udtrykt som en brøkdel. Bruges typisk til at indikere fremdrift, fx hvor meget af en fil der er downloadet. |
| `ActivityIndicator` | Bool | `IsRunning` | Viser at noget sker. Forskellig fra en `ProgressBar` ved kun at vise, at en aktivitet foregår, frem for at indikere færdiggørelsesstatus. |

### 4.2.2 Accepting input

Hver af disse controls har en property, der repræsenterer brugerens valgte værdi. **Alle disse properties er bindable**, så man kan enten bruge data binding eller læse værdien direkte i code-behind.

**Table 4.2 — Controls til at modtage input:**

| Control | Value property | Primary data type name | Use case |
| --- | --- | --- | --- |
| `Entry` | `Text` | String | Lader brugeren indtaste tekst |
| `Editor` | `Text` | String | Lader brugeren indtaste tekst over flere linjer |
| `CheckBox` | `IsChecked` | Bool | Giver en yes/no-, true/false- eller on/off-option |
| `DatePicker` | `Date` | DateTime | Lader brugeren vælge en dato |
| `Slider` | `Value` | Double | Lader brugeren vælge en værdi mellem et minimum og maksimum (som standard mellem 0 og 1) |
| `Stepper` | `Value` | Double | Øger eller mindsker et tal med et angivet beløb (1 som standard) |
| `TimePicker` | `Time` | TimeSpan | Lader brugeren vælge et tidspunkt |
| `RadioButton` | `IsChecked` | Bool | Lader brugeren vælge én option fra en gruppe viste options ved at afkrydse en boks |
| `Picker` | `SelectedItem` | Object | Lader brugeren vælge én option fra en gruppe listede options ved at plukke den fra en liste |

Disse controls har mange flere properties end de nævnte — de er lette at opdage via IntelliSense.

### 4.2.3 Accepting commands

Disse controls adskiller sig fra input-controls ved, at de strengt taget fortæller appen, at brugeren **vil gøre noget**, i modsætning til at brugeren leverer en værdi.

Hver af disse controls har en bindable property af typen **`ICommand`**, som kan bruges til at eksekvere en action som svar på brugerinteraktion. De har også event handlers, der kan bruges i code-behind uden binding.

**Table 4.3 — Controls der initierer en action eller command:**

| Control | `ICommand` property name | Event handler | Use case |
| --- | --- | --- | --- |
| `Button` | `Command` | `Clicked` | En simpel control, brugeren kan tappe eller klikke for at initiere en action. Bruger tekst til at formidle sin intention. |
| `ImageButton` | `Command` | `Clicked` | Samme som `Button`, men bruger et billede i stedet for tekst til at formidle intentionen. |
| `SearchBar` | `SearchCommand` | `SearchButtonPressed`, `TextChanged` (nedarvet fra Input) | Præsenterer en genkendelig søgeboks med tekstinput og et forstørrelsesglas-ikon. Både `SearchCommand` og `SearchButtonPressed` er knyttet til forstørrelsesglasset. Enhver af commands eller event handlers kan kobles til kode, der søger eller filtrerer en liste. |

*Note:* `ICommand`-propertyen på hver control er bindable, eller man kan bruge event handleren i stedet. I den officielle dokumentation grupperes yderligere to controls i denne kategori: `RadioButton` og `SwipeView`, som bogen dækker under control modifiers.

### 4.2.4 Displaying graphics

.NET MAUI tilbyder tre måder at præsentere grafik på.

**Table 4.4 — Controls til grafik:**

| Control | Use case |
| --- | --- |
| `Image` | Viser billeder. `Source`-propertyen kan modtage billeder fra en fil, en resource, en URL eller en `Stream` og udfyldes automatisk via statiske konstruktorer (man behøver kun angive `Source`, og typen udledes). |
| `Shapes` | Lader dig tegne simple geometriske former på skærmen eller rendere komplekse billeder med en SVG-kompatibel `Path`-control. Nyttigt til hurtigt at placere en simpel form på siden. |
| `GraphicsView` | Eksponerer et canvas, der lader dig tegne simple eller komplekse billeder med brushes og paths. `GraphicsView` er nyt i .NET MAUI og en del af MAUI.Graphics-biblioteket. |

## 4.3 Displaying lists and collections

Den mest effektive måde at vise en collection af data på er at definere en **template** for hvert item i collection'en og lade appen rendere collection'en ud fra din template. **Figure 4.3** viser princippet: templated views tager en template og en collection af data og renderer hvert item i collection'en efter templaten.

Nøgle-propertyerne på disse controls er **`ItemsSource`** og **`ItemTemplate`**:

- `ItemsSource` er en bindable property, der kan bindes til en collection af typen `IEnumerable<T>` (eller enhver nedarvet collection-type). Det gør det let at sætte bindingerne i XAML; man kan ikke direkte tildele en collection til propertyen i kode, men man kan bruge `SetBinding`-metoden nedarvet fra `BindableObject`.
- `ItemTemplate` definerer, hvordan hvert item præsenteres. Man tildeler en `DataTemplate` til `ItemTemplate`-propertyen.

Ud over statisk at definere én item-template kan man bruge en **data template selector** til at vælge forskellige templates at runtime baseret på hvert items properties.

`TableView` adskiller sig fra de øvrige tre ved ikke at have en `ItemsSource`-property — den bruger hverken template eller data source, men child views tilføjes statisk.

### 4.3.1 CollectionView

`CollectionView` er uden tvivl arbejdshesten i .NET MAUI-apps.

`CollectionView` understøtter flere layout-typer. **Figure 4.4** viser to af dem: vertical list og horizontal list; `CollectionView` understøtter også vertical-grid og horizontal-grid layouts. **Vertical list er default**, så det layout fås ved blot at instantiere en `CollectionView` uden layout-parametre. Figuren viser desuden, at `CollectionView` scroller automatisk og giver et hårdt stop, når man når enden af collection'en.

`CollectionView` har en bindable `ItemsSource`-property og en `DataTemplate`, der definerer, hvordan hvert item præsenteres. Derudover lader `CollectionView` dig specificere, om brugere kan vælge ét eller flere items, via de bindable properties **`SelectedItem`** og **`SelectedItems`**.

### 4.3.2 ListView

`ListView` er konceptuelt lig `CollectionView`, men fokuseret på at præsentere items med en af et sæt foruddefinerede templates: `TextCell`, `ImageCell`, `EntryCell` og `SwitchCell`. Hvor `DataTemplate` i `CollectionView` kan være et vilkårligt layout af eget design, skal en `DataTemplate` i `ListView` referere en af disse celletyper. En yderligere celletype, **`ViewCell`**, lader dig dog definere et custom layout.

Forskelle:

- `CollectionView` understøtter **multiple selection**; `ListView` understøtter kun **single selection**.
- `ListView` understøtter **context actions**, som `CollectionView` ikke gør — men man kan bruge `SwipeView`-modifier-controlen til at tilføje den funktionalitet til items i en `CollectionView`.

Alt hvad man kan opnå med `ListView`, kan man også med `CollectionView`, som giver bedre fleksibilitet. `CollectionView` blev introduceret som en mere moden version af `ListView`, og selvom `ListView` stadig findes, er **`CollectionView` det, man bør bruge i .NET MAUI**.

### 4.3.3 CarouselView and IndicatorView

`CarouselView` er faktisk en extension af `CollectionView` og bygget oven på den. Den binder til en collection og renderer hvert item efter en template i en horisontalt eller vertikalt scrollende sekvens.

Forskelle:

- `CarouselView` defaulter til **horisontal orientering**, hvilket hjælper med at fokusere på ét item ad gangen, hvor fokus i en `CollectionView` (eller `ListView`) er selve collection'en.
- Hovedforskellen er, at `CarouselView` **kan vende tilbage til begyndelsen**, når man når enden af collection'en, og dermed vise items i en karrusel frem for en endimensionel liste. **Figure 4.5** viser dette: i modsætning til en `CollectionView` kan den konfigureres til at loope, den kan orienteres både vertikalt og horisontalt, og — modsat en fysisk karrusel — kan man scrolle i begge retninger.
- `CarouselView` tillader ikke egentlig selection af items, men tilbyder en bindable property **`CurrentItem`**, som kan bruges til at afgøre det aktuelt fokuserede item.

#### CellBoutique-eksemplet

`Models`-mappen indeholder en klasse `Product` med fire properties:

- `Title` — produktets navn
- `Description` — en kort beskrivelse
- `Price` — produktets pris
- `Image` — en URL der peger på et billede af produktet

I `MainPage.xaml.cs` er `OnAppearing`-metoden overridet; i den udfyldes en `ObservableCollection` af typen `Product` med nogle items. I konstruktoren er sidens binding context sat til sig selv, så vi kan binde til sidens properties i XAML.

#### Listing 4.1 Updated MainPage.xaml from the CellBoutique app

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="CellBoutique.MainPage">

    <CarouselView ItemsSource="{Binding Products}"
                  Margin="10">

        <CarouselView.ItemsLayout>
            <LinearItemsLayout Orientation="Horizontal"
                               ItemSpacing="60"/>
        </CarouselView.ItemsLayout>

        <CarouselView.ItemTemplate>
            <DataTemplate>
                <VerticalStackLayout Spacing="20">

                    <Image Source="{Binding Image}"
                           WidthRequest="400"
                           HeightRequest="500"
                           Aspect="AspectFill"
                           HorizontalOptions="Center"/>

                    <HorizontalStackLayout Spacing="20">
                        <VerticalStackLayout WidthRequest="200"
                                             Spacing="10">
                            <Label Text="{Binding Title}"
                                   FontAttributes="Bold" />
                            <Label Text="{Binding Description}"
                                   LineBreakMode="WordWrap"/>
                        </VerticalStackLayout>
                        <Label FontAttributes="Bold"
                               HorizontalTextAlignment="End"
                               Text="{Binding Price, StringFormat='{0:c}'}"/>
                    </HorizontalStackLayout>

                </VerticalStackLayout>
            </DataTemplate>
        </CarouselView.ItemTemplate>

    </CarouselView>
</ContentPage>
```

① Tilføjer `CarouselView` og binder dens `ItemsSource` til `Products`-propertyen på binding context.
② Tilføjer en margin på 10 for at give afstand mellem `CarouselView` og sidens kant.
③ Specificerer `CarouselView.ItemsLayout`-propertyen.
④ Specificerer en `LinearItemsLayout` og sætter `Orientation` til `Horizontal`; dette er defaults, men skal sættes når man angiver `ItemsLayout`.
⑤ Sætter `ItemSpacing` på `LinearItemsLayout` til 60 for at give plads mellem items.
⑥ Definerer `CarouselView`'ets `ItemTemplate`.
⑦ Tilføjer et layout og nogle controls bundet til properties på `Product`-typen.

> **ImageSource in .NET MAUI**
> `Image`-controlen har en property `Source` af typen `ImageSource`. `ImageSource` har fire metoder til at vise et billede fra forskellige kilder: **`FromFile`, `FromUrl`, `FromResource` og `FromStream`.**
>
> `Source` er en bindable property, så man kan enten sætte værdien direkte eller bruge en binding. Når man bruger `Image`-controlen i XAML, er .NET MAUI smart nok til at vide, hvilken type `ImageSource` der bruges, og viser billedet derefter. Vil man bruge egne produktfotos, importeres de til `Resources/Images`-mappen, og `Image`-propertyen på `Product`-entries ændres — det lokale billede vises da i stedet uden andre kodeændringer.

**Figure 4.6** viser CellBoutique med `CarouselView` på Windows: items vises i en scrollbar karrusel, så listen gentager sig i et loop. På desktop-OS navigerer man med scrollhjul eller piletaster. **Figure 4.7** viser samme app på Android, hvor man bruger en swipe-gesture.

### 4.3.4 TableView

I modsætning til de øvrige controls tilføjes `TableView`'s child-items direkte frem for at blive renderet automatisk fra en source-liste. **`TableView` har ikke en `ItemsSource`-property.**

Ligesom `ListView` skal man i `TableView` bruge specifikke celletyper (**Figure 4.8**):

- **`TextCell`** — viser to `Label`s: en primær og en sekundær
- **`ImageCell`** — som en `TextCell`, men tilføjer et billede
- **`SwitchCell`** — viser tekst og en `Switch`
- **`EntryCell`** — viser en `Label` og redigerbar tekst
- **`ViewCell`** — viser et custom layout

**Figure 4.8** pointerer, at `TableView` viser en scrollbar liste af statiske, ikke-templatede items. Hvert item kan være forskelligt og urelateret og indtastes manuelt.

Dokumentationen foreslår fire use cases: en menu, en formular, præsentation af data og settings. Den bedste use case er en **settings-side konsistent med det native OS' settings-app** (fx som Metas Messenger eller Microsofts Outlook).

## 4.4 Common properties and control modifiers

Dette afsnit dækker fælles modifiers:

- Clipping
- Borders
- Shadows
- Gesture recognizers
- `SwipeView`
- `RefreshView`

samt de to mest brugte properties fælles for alle views: `Height` og `Width`.

### 4.4.1 Height and Width

Alle views har `Height`- og `Width`-properties. **Disse properties er read-only** og bruges til at *hente* et views højde eller bredde. Det er nyttigt, hvis man bygger et responsivt layout eller skal justere størrelse eller position af ét visuelt element som svar på et andet.

Da de er read-only, sætter man i stedet **`HeightRequest`** og **`WidthRequest`** (fra `VisualElement`-baseklassen, som alle controls og layouts nedarver). Disse værdier angives i **device-independent units (DIUs)**.

> **Device-independent units**
> At arbejde direkte med pixels er upraktisk i moderne UI-udvikling, fordi hver skærm har forskellig opløsning — og værre: selv skærme med samme opløsning kan have vidt forskellige størrelser. Det er ikke ualmindeligt at finde en mobiltelefon med en 1080p-skærm (eller endda 4K) på 6 tommer eller mindre — samme opløsning som et 55- eller 85-tommers TV.
>
> Problemet er løst med **device-independent units (DIUs)**. De har lidt forskellige navne på forskellige platforme (device-independent pixels, density-independent pixels eller points), men de fleste platforme følger samme sizing-konvention, groft svarende til **160 units pr. tomme eller 64 units pr. centimeter**. Giver man en size-værdi på 2 til spacing i et grid, bliver afstanden ca. 1/32 cm eller 1/80 tomme, uanset skærmens størrelse eller opløsning.
>
> Bemærk ordene *groft* og *ca.* — systemet er ikke perfekt (se noten om pixel-perfect-myten i kapitel 5), men tæt nok på i de fleste tilfælde. **Ser man en height-, width- eller thickness-værdi i en .NET MAUI-app, og størrelsen ikke er proportional (angivet med en asterisk `*`), er størrelsen i DIUs.**

Når en layout-beregning trigges (fx når en side loader), beregner en metode kaldet `GetSizeRequest` layout-bounds ud fra en kombination af faktorer, herunder den requestede højde og bredde og andre constraints som parent-elementets højde og bredde. `HeightRequest` og `WidthRequest` er gode nok til at give den ønskede størrelse — så længe man forstår, at de begrænses af andre faktorer, er det sikkert at tænke på dem som at sætte højde og bredde.

### 4.4.2 Clipping

`Clip`-propertyen findes på `VisualElement`-baseklassen, så den kan angives på enhver control eller layout, og er af typen `Geometry`.

Der er tre foruddefinerede "simple" geometrier i .NET MAUI: **`EllipseGeometry`, `LineGeometry` og `RectangleGeometry`**.

Vi tilføjer et `Clip` til `Image` i `DataTemplate` i CellBoutique for at gøre hvert billede til en ellipse. Vi reducerer billedets højde og bredde, så hele ellipsen er synlig, sætter `Center` til x- og y-værdier svarende til halvdelen af bredden og højden, og gør det samme for `RadiusX` og `RadiusY`.

#### Listing 4.2 Adding ellipse clipping to CellBoutique

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="CellBoutique.MainPage">

    <CarouselView ItemsSource="{Binding Products}"
                  Margin="10">
        <CarouselView.ItemsLayout>
            <LinearItemsLayout Orientation="Horizontal"
                               ItemSpacing="60"/>
        </CarouselView.ItemsLayout>
        <CarouselView.ItemTemplate>
            <DataTemplate>
                <VerticalStackLayout Spacing="20">

                    <Image Source="{Binding Image}"
                           WidthRequest="300"
                           HeightRequest="400"
                           Aspect="AspectFill"
                           HorizontalOptions="Center">
                        <Image.Clip>
                            <EllipseGeometry Center="150,200"
                                             RadiusX="150"
                                             RadiusY="200"/>
                        </Image.Clip>
                    </Image>

                    <HorizontalStackLayout Spacing="20">
                        <VerticalStackLayout WidthRequest="200"
                                             Spacing="10">
                            <Label Text="{Binding Title}"
                                   FontAttributes="Bold" />
                            <Label Text="{Binding Description}"
                                   LineBreakMode="WordWrap"/>
                        </VerticalStackLayout>
                        <Label FontAttributes="Bold"
                               HorizontalTextAlignment="End"
                               Text="{Binding Price, StringFormat='{0:c}'}"/>
                    </HorizontalStackLayout>

                </VerticalStackLayout>
            </DataTemplate>
        </CarouselView.ItemTemplate>
    </CarouselView>
</ContentPage>
```

① Reducerer billedets bredde, så hele ellipsen kan vises i karrusellen.
② Reducerer billedets højde af samme grund.
③ Ændrer det selvlukkende `Image`-tag til en eksplicit `<Image...>...</Image>`-deklaration.
④ Specificerer billedets `Clip`-property.
⑤ Tilføjer en `EllipseGeometry` med `Center` i 150, 200 (halvdelen af bredden og højden).
⑥ Sætter `RadiusX` til halvdelen af bredden.
⑦ Sætter `RadiusY` til halvdelen af højden.

**Figure 4.9** viser CellBoutique på Android med billeder klippet med `EllipseGeometry`.

Man er ikke begrænset til simple geometrier. Man kan også bruge en **composite geometry**, hvor man kombinerer mere end én geometri. I stedet for at tildele en geometri til `Clip`, tildeler man en **`GeometryGroup`**, hvori man kan angive så mange geometrier man vil.

Eksempel: marketingchefen beder om, at billederne vises i en snemandsform i juleperioden. Det opnås ved at kombinere to ellipse-geometrier.

#### Listing 4.3 CellBoutique with a composite geometry clipping the images

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="CellBoutique.MainPage">

    <CarouselView ItemsSource="{Binding Products}"
                  Margin="10">
        <CarouselView.ItemsLayout>
            <LinearItemsLayout Orientation="Horizontal"
                               ItemSpacing="60"/>
        </CarouselView.ItemsLayout>
        <CarouselView.ItemTemplate>
            <DataTemplate>
                <VerticalStackLayout Spacing="20">

                    <Image Source="{Binding Image}"
                           WidthRequest="300"
                           HeightRequest="400"
                           Aspect="AspectFill"
                           HorizontalOptions="Center">
                        <Image.Clip>
                            <GeometryGroup>
                                <EllipseGeometry Center="150,100"
                                                 RadiusX="100"
                                                 RadiusY="100"/>
                                <EllipseGeometry Center="150,250"
                                                 RadiusX="150"
                                                 RadiusY="150"/>
                            </GeometryGroup>
                        </Image.Clip>
                    </Image>

                    <HorizontalStackLayout Spacing="20">
                        <VerticalStackLayout WidthRequest="200"
                                             Spacing="10">
                            <Label Text="{Binding Title}"
                                   FontAttributes="Bold" />
                            <Label Text="{Binding Description}"
                                   LineBreakMode="WordWrap"/>
                        </VerticalStackLayout>
                        <Label FontAttributes="Bold"
                               HorizontalTextAlignment="End"
                               Text="{Binding Price, StringFormat='{0:c}'}"/>
                    </HorizontalStackLayout>

                </VerticalStackLayout>
            </DataTemplate>
        </CarouselView.ItemTemplate>
    </CarouselView>
</ContentPage>
```

① Opdaterer `Clip`-propertyen til at være en `GeometryGroup`.
② Tilføjer en `EllipseGeometry` med centrum halvvejs på tværs og en fjerdedel nede — snemandens "hoved".
③ Sætter `RadiusX` til 100.
④ Sætter `RadiusY` til 100.
⑤ Tilføjer endnu en `EllipseGeometry` med centrum halvvejs på tværs og 5/8 nede — snemandens "krop".
⑥ Sætter `RadiusX` til 150 (lidt bredere end hovedet).
⑦ Sætter `RadiusY` til 150 (lidt højere end hovedet).

**Figure 4.10** viser CellBoutique på Windows med billederne klippet af en composite geometry bestående af to ellipser: en større nederst til "kroppen" og en mindre øverst til "hovedet".

Ud over simple og composite geometrier kan man skabe en vilkårlig form med en `Path`.

### 4.4.3 Borders

Et af de erklærede mål for .NET MAUI var "borders everywhere". Man wrapper simpelthen enhver view i en **`Border`** og specificerer `Stroke`-, `StrokeThickness`- og `StrokeShape`-properties.

- **`Stroke`** er en `Brush`, hvilket betyder man kan definere en linear gradient, en radial gradient eller en enkelt farve (angivet via `Colors`-enum, en hex-værdi eller en static/dynamic resource).
- **`StrokeThickness`** definerer, hvor tyk borderen bliver, i DIUs.
- **`StrokeShape`** definerer formen på borderen. Man kan bruge en foruddefineret `Ellipse`, `Rectangle` eller `RoundRectangle` (et rektangel med afrundede hjørner), men også mere komplekse former via `Path`-, `Polygon`- og `Polyline`-implementeringerne af `IShape`-interfacet (den type `StrokeShape` forventer), eller endda en simpel linje.

> **Brushes**
> En `Brush` kan bruges flere steder i .NET MAUI, inklusive `Stroke`-propertyen på en `Border` og til view-baggrunde. Der findes tre typer: **`SolidColor`, `LinearGradientBrush` og `RadialGradientBrush`.** `SolidColor` er default, hvilket er grunden til at man kan angive én farve via enum, hex eller resource-reference. `LinearGradientBrush` dækkes i kapitel 7.

**Figure 4.11** viser resultatet i MauiTodo: `Border`ens `StrokeShape` er `RoundRectangle` med en `CornerRadius` på 10, `Stroke` er `Primary`-farvens `StaticResource`, og `StrokeThickness` er 3. Brugen af borders betyder også, at vi kan reducere afstanden mellem items i collection'en.

#### Listing 4.4 Borders in MauiTodo

```xml
<DataTemplate>
    <Border Stroke="{StaticResource Primary}"
            StrokeThickness="3"
            Padding="5"
            Margin="0,10">

        <Border.StrokeShape>
            <RoundRectangle CornerRadius="10"/>
        </Border.StrokeShape>

        <Grid WidthRequest="325"
              ColumnDefinitions="1*, 5*"
              RowDefinitions="Auto, 25"
              x:Name="TodoItem">

            <CheckBox VerticalOptions="Center"
                      HorizontalOptions="Center"
                      Grid.Column="0"
                      Grid.Row="0" />

            <Label Text="{Binding Title}"
                   FontAttributes="Bold"
                   LineBreakMode="WordWrap"
                   HorizontalOptions="StartAndExpand"
                   FontSize="Medium"
                   Grid.Row="0"
                   Grid.Column="1"/>

            <Label Text="{Binding Due, StringFormat='{0:dd MMM yyyy}'}"
                   VerticalOptions="End"
                   Grid.Column="1"
                   Grid.Row="1"/>

        </Grid>
    </Border>
</DataTemplate>
```

① Tilføjer en `Border` omkring `Grid`'et og sætter `Stroke` til `Primary`-farvens `StaticResource`.
② Sætter `StrokeThickness` til 3.
③ Flytter `Padding`-propertyen fra `Grid`'et til `Border`en og reducerer den fra 10 til 5.
④ Flytter `Margin`-propertyen fra `Grid`'et til `Border`en og reducerer de vertikale margins fra 20 til 10.
⑤ Definerer `Border`ens `StrokeShape`-property.
⑥ Sætter `StrokeShape` til den foruddefinerede `RoundRectangle` med en `CornerRadius` på 10.

De fire hjørner behøver ikke have samme radius: man kan angive fire individuelle værdier adskilt af mellemrum, svarende til øverst-venstre, øverst-højre, nederst-højre og nederst-venstre.

Der findes også en kortform for `StrokeShape`, hvor propertyen angives direkte i `Border`-tagget:

#### Listing 4.5 Simplified StrokeShape

```xml
<Border Stroke="Black"
        StrokeThickness="3"
        StrokeShape="RoundRectangle 10"
        Padding="10">
    <Grid>
        ...
    </Grid>
</Border>
```

① `Border.StrokeShape`-tagget er erstattet med en inline shorthand-property.

**Figure 4.12** viser MauiTodo kørende på Windows med borders og reduceret spacing.

### 4.4.4 Shadows

`Shadow`-propertyen findes på `VisualElement`-baseklassen, som enhver control og ethvert layout nedarver. `Shadow` kræver fire properties:

- **`Brush`** — angiver skyggens farve, kan sættes med `Colors`-enum'en.
- **`Opacity`** — angiver hvor uigennemsigtig skyggen er, med en værdi mellem 0 og 1.
- **`Radius`** — definerer skyggens radius, angivet med et tal i DIUs.
- **`Offset`** — angiver skyggens offset (konceptuelt lyskildens position). `Offset` er af typen `Point` og kræver derfor et værdipar (også i DIUs) for x- og y-koordinater.

I MauiTodo tilføjer vi en `Shadow` til `Border`en, da den er det yderste view i ethvert kort.

#### Listing 4.6 Border with a Shadow

```xml
<Border Stroke="{StaticResource Primary}"
        StrokeThickness="3"
        StrokeShape="RoundRectangle 10"
        Padding="10">

    <Border.Shadow>
        <Shadow Brush="Black"
                Offset="20,20"
                Radius="40"
                Opacity="0.8" />
    </Border.Shadow>

    <Grid ...>
        ...
    </Grid>
</Border>
```

① Tilføjer et tag til at specificere `Shadow`-propertyen på `Border`en.
② Tilføjer en `Shadow` med en `Black` `Brush`.
③ Sætter `Offset` til 20,20.
④ Giver skyggen en `Radius` på 40.
⑤ Sætter `Opacity` til 0.8.

**Figure 4.13** viser MauiTodo kørende på Android med en `Shadow` tilføjet `Border`en.

### 4.4.5 Gesture recognizers

.NET MAUI understøtter fem gesture recognizers:

- Tap
- Pan
- Swipe
- Pinch
- Drag and drop

Bortset fra pinch har disse gestures analoge interaktioner, der kan opnås med en mus eller tilsvarende cursor-enhed. `View`-baseklassen har en collection kaldet **`GestureRecognizers`**, og da alle controls og layouts nedarver den, kan disse gestures tilføjes ethvert view i .NET MAUI.

### 4.4.6 RefreshView

Pull-to-refresh: man trækker ned fra toppen af skærmen og slipper for at opdatere indholdet.

**`RefreshView`** er en wrapper, der kan placeres omkring ethvert andet view for at tilføje pull-to-refresh-funktionalitet, inklusive scrollende views som `ScrollView` og de list- og collection-views, vi så tidligere. Den lader dig scrolle til toppen, før refresh-funktionaliteten aktiveres, hvilket forhindrer den i at forstyrre scrollende views.

`RefreshView` har to bindable properties:

- **`Command`** — logikken der skal eksekveres for at opdatere view'et.
- **`IsRefreshing`** — en boolean, du sætter til `false`, når logikken er færdig.

Når brugeren aktiverer refresh, vises en `ActivityIndicator` (indbygget i `RefreshView`, ikke en du selv definerer), indtil `IsRefreshing` sættes til `false`.

### 4.4.7 SwipeView

Et udbredt UI-paradigme er brugen af en swipe til at afsløre yderligere funktionalitet — fx i Outlook mobile, hvor man kan swipe på et item i sin indbakke for at afsløre konfigurerbare handlinger som delete og archive.

I .NET MAUI bruger man **`SwipeView`** til at tilføje denne funktionalitet til ethvert view. Ligesom `Border` wrapper `SwipeView` andre views.

`SwipeView` har fire collections af `SwipeItems`: **`LeftItems`, `RightItems`, `TopItems` og `BottomItems`**. Man tilføjer `SwipeItem`s til disse collections, som afsløres afhængigt af swipe-retningen. En property kaldet **`Mode`** med to muligheder, **`Execute`** og **`Reveal`**, afgør, hvad der sker når brugeren swiper.

I MauiTodo tilføjer vi et `SwipeItem` til `LeftItems` til at slette to-do item'et og et til `RightItems`, der markerer item'et som done.

#### Listing 4.7 Adding SwipeView

```xml
<DataTemplate>
    <SwipeView>

        <SwipeView.LeftItems>
            <SwipeItems Mode="Execute">
                <SwipeItem Text="Delete"
                           IconImageSource="delete"
                           BackgroundColor="Tomato"/>
            </SwipeItems>
        </SwipeView.LeftItems>

        <SwipeView.RightItems>
            <SwipeItems Mode="Execute">
                <SwipeItem Text="Done"
                           IconImageSource="check"
                           BackgroundColor="LimeGreen"/>
            </SwipeItems>
        </SwipeView.RightItems>

        <Border ...>
            <Border.Shadow>
                ...
            </Border.Shadow>
            <Grid ...>
                ...
            </Grid>
        </Border>

    </SwipeView>
</DataTemplate>
```

① Åbner et `SwipeView`-tag til at wrappe view'et.
② Definerer `SwipeView`'ets `LeftItems`-collection.
③ Tilføjer en collection af `SwipeItems` med `Mode` sat til `Execute`.
④ Tilføjer et `SwipeItem` og sætter `Text` til Delete.
⑤ Sætter `IconImageSource` til navnet på filen med delete-ikonet.
⑥ Sætter `BackgroundColor` til `Tomato`.
⑦ Definerer `SwipeView`'ets `RightItems`-collection.
⑧ Tilføjer en collection af `SwipeItems` med `Mode` sat til `Execute`.
⑨ Tilføjer et `SwipeItem` og sætter `Text` til Done.
⑩ Sætter `IconImageSource` til navnet på filen med check-ikonet.
⑪ Sætter `BackgroundColor` til `LimeGreen`.

**Figure 4.14** viser resultatet med `Mode="Execute"`: når brugeren begynder at swipe, afsløres handlingen, og fuldføres swipe-gesturen, eksekveres handlingen.

Alternativet er **`Reveal`**, som i stedet for at eksekvere viser de tilgængelige handlinger; brugeren skal derefter eksplicit eksekvere en handling med et tap eller klik. **Figure 4.15** viser MauiTodo med `Mode="Reveal"`. **`Reveal` er den bedre mulighed, når man vil vise flere handlinger.**

For at eksekvere handlingerne kan man bruge en event handler kaldet **`Invoked`** eller de bindable properties **`Command`** og **`CommandParameter`**.

#### Listing 4.8 SwipeItem_Invoked method

```csharp
private async void SwipeItem_Invoked(object sender, EventArgs e)     // ①
{
    var item = sender as SwipeItem;                                  // ②
    await App.Current.MainPage
        .DisplayAlert(item.Text, $"You invoked the {item.Text} action.", "OK");   // ③
}
```

① Tilføjer en metode med den standard event handler-signatur.
② Caster `sender` til en midlertidig variabel af typen `SwipeItem`.
③ Viser en alert, der kvitterer for den swipe-handling, brugeren har foretaget.

#### Listing 4.9 DataTemplate with the Invoked event handler

```xml
<DataTemplate>
    <SwipeView>

        <SwipeView.LeftItems>
            <SwipeItems Mode="Reveal">
                <SwipeItem Text="Delete"
                           IconImageSource="delete"
                           BackgroundColor="Tomato"
                           Invoked="SwipeItem_Invoked"/>
            </SwipeItems>
        </SwipeView.LeftItems>

        <SwipeView.RightItems>
            <SwipeItems Mode="Reveal">
                <SwipeItem Text="Done"
                           IconImageSource="check"
                           BackgroundColor="LimeGreen"
                           Invoked="SwipeItem_Invoked"/>
            </SwipeItems>
        </SwipeView.RightItems>

        <Border ...>
            <Border.Shadow>
                ...
            </Border.Shadow>
            <Grid ...>
                ...
            </Grid>
        </Border>

    </SwipeView>
</DataTemplate>
```

① Sætter `Invoked`-propertyen på `SwipeItem` til `SwipeItem_Invoked`-event handleren.

**Figure 4.16** viser MauiTodo på Android: at eksekvere en swipe-handling (swipe item'et til siden og tappe det afslørede ikon) kalder event handleren, som parser `sender` (her et `SwipeItem`) og viser en alert med sender'ens `Text`-property.

Disse alerts er placeholders for funktionaliteten til at markere et to-do item som done eller slette det. Vi kan endnu ikke implementere den funktionalitet — i kapitel 9 ser vi hvorfor, og hvordan MauiTodo kan refaktoreres til MVVM-mønstret for at løse problemet.

## Summary

- Views er en apps visuelle komponenter. De omfatter **pages** (navigerbare sektioner af en app), **layouts** (styrer hvordan ting arrangeres på skærmen) og **controls** (viser noget til eller får noget fra brugeren).
- Du kan bruge en række indbyggede controls i .NET MAUI til at vise information til en bruger eller modtage input fra dem.
- .NET MAUI leverer flere controls, der kan binde til en data source og bruge en template til at rendere hvert item i data source'en til skærmen.
- Du kan modificere udseendet af enhver control eller ethvert layout til at inkludere borders og shadows og klippe formen af et layout eller en control med en geometry.
- Du kan tilføje recognizers for gestures, fx tap og swipe, til views i .NET MAUI for at tilføje velkendte UX-paradigmer til dine apps.
