# Layouts i .NET MAUI

## Metadata

- **Lektion:** L04.2 – Layouts in .NET MAUI
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Layouts.pdf (35 slides)
- **Emner dækket:**
  - Layout som fundament for UI-design
  - Grid: RowDefinitions, ColumnDefinitions, proportional sizing
  - Grid.Row / Grid.Column og RowSpan / ColumnSpan
  - Fonts og typografi, registrering af custom fonts (LCD)
  - ScrollView og scrollbare sider
  - HorizontalStackLayout og VerticalStackLayout
  - FlexLayout kontra StackLayout
  - BindableLayout som introduktion

---

## 1. Hvad layout er

Layout er det vigtigste aspekt af et UI-design. UI-design består grundlæggende af tre aspekter: layout, typography og color (plus flere). Layout er fundamentet, som resten af elementerne bygges oven på.

> "Layout is the most critical piece of a website's design. It is quite literally the foundation for the rest of the pieces that will eventually be added to the website."
> — *Design for Developers*, Stephanie Stimac, Manning 2022

## 2. Layouts i .NET MAUI

.NET MAUI indeholder følgende layouts:

- `Grid`
- `HorizontalStackLayout`
- `VerticalStackLayout`
- `FlexLayout`
- `BindableLayout` (behandles i næste lektion)
- `AbsoluteLayout`

Denne lektion fokuserer på Grid, stack layouts og FlexLayout, plus `ScrollView`.

## 3. Grid

`Grid` er det mest kraftfulde layout i .NET MAUI, og de fleste UI-layouts implementeres med et Grid.

Grid arrangerer child elements i rows og columns. Det er vigtigt at forstå, at et Grid **ikke** er en tabel — Grid er ikke til at vise rækker og kolonner af data. Grid bruger rows og columns til at placere views på skærmen.

### Rows og columns

`RowDefinitions` og `ColumnDefinitions` definerer Grid'ets række- og kolonnekarakteristik:

- Et Grid bør indeholde én `RowDefinition` og `ColumnDefinition`-samling.
- `RowDefinitions`: det samlede antal rows i Grid'et.
- `ColumnDefinitions`: det samlede antal columns i Grid'et.

**Proportional sizing:** ved at bruge asterisk (`*`) angives, at størrelsen er proportional — altså ligeligt fordelt.

`RowSpacing` og `ColumnSpacing` angiver et mellemrum mellem hver row og column. Som default har Grid-rows og -columns intet mellemrum.

`Grid.Row` og `Grid.Column` bruges til at placere controls i Grid'et. Det kan være en hvilken som helst control eller layout.

### Kodeeksempel: to måder at definere rows og columns

Den udførlige form:

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="*" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>
</Grid>
```

Den forkortede form giver samme resultat:

```xml
<Grid RowDefinitions="*,*"
      ColumnDefinitions="*,*">
</Grid>
```

### Kodeeksempel: Grid med 4 columns og 5 rows

Proportionalt størrelsessat, med row- og column-spacing på 2:

```xml
<Grid ColumnDefinitions="*,*,*,*"
      RowDefinitions="*,*,*,*,*"
      RowSpacing="2"
      ColumnSpacing="2">
</Grid>
```

## 4. Grid.Row og Grid.Column

Child views kan placeres i specifikke celler. Enhver control eller layout kan placeres i en Grid-celle. Cellerne indekseres fra 0, så et 4x5-grid har `Grid.Row` fra 0 til 4 og `Grid.Column` fra 0 til 3.

### Kodeeksempel

```xml
<Button Grid.Row="3"
        Grid.Column="0"
        Text="+" />
```

```xml
<Button Grid.Row="3"
        Grid.Column="2"
        Text="=" />
```

## 5. RowSpan og ColumnSpan

**RowSpan** får et view til at optage mere end én lodret row. Brug `Grid.RowSpan` til at angive, hvor mange rows viewet skal spænde over.

**ColumnSpan** angiver, at viewet starter i en bestemt column, men skal spænde over det angivne antal columns. Skal den øverste row f.eks. spænde over 4 columns, placeres viewet i `Grid.Column="0"` med `ColumnSpan="4"`.

### Kodeeksempel: Label over hele første row

Her tilføjes en Label i hele første row, altså spændt over 4 columns, med tekstjustering sat til `End`:

```xml
<Label Grid.Row="0"
       Grid.Column="0"
       Grid.ColumnSpan="4"
       HorizontalTextAlignment="End"
       VerticalTextAlignment="End" />
```

## 6. Fonts og typografi i .NET MAUI

Som default bruger .NET MAUI-apps Open Sans-fonten på hver platform. Yderligere fonts kan registreres til brug i en app.

Alle controls med properties til at vise tekst kan få ændret font appearance:

- `FontFamily`
- `FontAttributes`
- `FontSize`
- `FontAutoScalingEnabled` (bool) — afspejler operativsystemets tekstskaleringspræferencer. Default er `true`.

Disse properties er backed af `BindableProperty`-objekter, hvilket betyder, at de kan være targets for data bindings og kan styles.

Microsofts Typography Group forsker i og udvikler fonts og fontteknologier og understøtter udviklingen af TrueType og OpenType fonts hos uafhængige type vendors. I en .NET MAUI-app kan TrueType-format (TTF) og OpenType font (OTF) tilføjes til appen og refereres ved filnavn eller alias, med registrering i `CreateMauiApp`-metoden i `MauiProgram`-klassen.

### Registrering af en font

Fremgangsmåden for f.eks. en LCD-font:

1. Download fontfilen (f.eks. `LCD.ttf` fra BrightSpace).
2. Placér filen i mappen `Resources/Fonts` i appen.
3. Kald `AddFont`-metoden for at tilføje fonten til appen.

### Kodeeksempel

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("...");
            fonts.AddFont("LCD.ttf", "LCD");   // Tilføj den nye font til appen
        });

    return builder.Build();
}
```

### Farve til fonten

Man kan tilknytte en specifik farve. Farven tilføjes til `ResourceDictionary` i `Resources/Styles.xaml`:

```xml
<Color x:Key="LcdBackgroundColor">#D1E0BA</Color>
```

### Brug af den registrerede font i MainPage

Registrerede fonts konsumeres ved at sætte `FontFamily`-propertyen. `BackgroundColor` sættes til den farve, vi definerede i `Styles.xaml`:

```xml
<Label ..
       BackgroundColor="{StaticResource LcdBackgroundColor}"
       FontFamily="LCD"
       ..
       x:Name="LCD"/>
```

## 7. Proportional højde på skærm og rows

For at det ser bedre ud på en telefon eller desktop, når layoutet strækkes, kan første row gives en større andel. Ændres højden på første row fra `*` til `4*`, og de fire øvrige rows har højden `*`, optager første row halvdelen af Grid'ets tilgængelige højde.

```xml
<Grid ColumnDefinitions="*,*,*,*"
      RowDefinitions="4*,*,*,*,*"
      RowSpacing="2"
      ColumnSpacing="2">
```

### Fast højde og bredde

Alternativt kan man angive faste størrelser i DIU'er. Her er rows til knapperne alle specificeret som 100 DIU høje, mens skærmen (første row) får den resterende tilgængelige højde. `HorizontalOptions="Center"` sørger for, at knapper og skærm står i midten af vinduet:

```xml
<Grid ColumnDefinitions="100,100,100,100"
      RowDefinitions="*,100,100,100,100"
      HorizontalOptions="Center"
      RowSpacing="2"
      ColumnSpacing="2">
```

## 8. ScrollView

`ScrollView` er en control, der lader sit childs indhold blive scrollet. Den er nyttig med indhold af ubestemt længde, hvor der skal præsenteres mere indhold, end der kan være på skærmen.

- Som default er ScrollViewets orientation vertikal, altså scrollretningen.
- Retningen kan også sættes til horisontal, til begge, eller til ingen af delene.
- Den kan scrolle både lodret og vandret.
- Den kan have nested scrolling views. Vær dog forsigtig med at neste scrolling views inden i hinanden — det kan gøre UI'et svært eller i nogle tilfælde umuligt at bruge. Hvis man ender med nested scrolling views, bør designet gentænkes, så scrolling views kun optræder på fixed pages.

En populær tendens er at bruge ScrollView som omsluttende container for pages. Det er dog umuligt at lave et UI, der ser ens ud på alle skærm-aspect ratios, fordi enheder findes i mange forskellige former og størrelser.

Slidesene sammenligner tre designtilgange: (A) det oprindelige design, (B) designet komprimeret så det passer til en mindre skærm, og (C) designet der scroller for at rumme indhold, der ikke kan være på den mindre skærm. Man vælger den tilgang, der passer bedst — scroll-varianten vinder frem, og ScrollView i .NET MAUI gør den let at implementere.

## 9. HorizontalStackLayout og VerticalStackLayout

`HorizontalStackLayout` og `VerticalStackLayout` er de simpleste layouts i .NET MAUI og formentlig de nemmeste til hurtige prototyper eller trivielle apps.

De to virker på samme måde bortset fra orienteringen:

- `VerticalStackLayout` organiserer child views i en endimensionel lodret stak.
- `HorizontalStackLayout` organiserer child views i en endimensionel vandret stak.

De har en collection af typen `View` kaldet `Children`. Views tilføjes til stakken og renderes på skærmen i den rækkefølge, de tilføjes. Man kan tilføje enhver control eller ethvert layout til et stack layout.

### Kodeeksempel: kort i cellboutique-appen

I cellboutique-appen vises kortene i en lodret stak, og hvert kort bruger et vertikalt og et horisontalt stack layout til at layoute sit indhold. Card-templaten er bygget udelukkende med VerticalStackLayout og HorizontalStackLayout. Selve kortet er et HorizontalStackLayout, som arrangerer sine children vandret fra venstre mod højre:

```xml
<HorizontalStackLayout Spacing="20"
                       Padding="10">
    <VerticalStackLayout WidthRequest="220"
                         Spacing="10">
        <Label Text="{Binding Title}"
               FontAttributes="Bold"/>
        <Label Text="{Binding Description}"
               LineBreakMode="WordWrap"/>
    </VerticalStackLayout>
    <Label Text="{Binding Price, StringFormat='{0:c}'}"
           FontAttributes="Bold"/>
</HorizontalStackLayout>
```

## 10. FlexLayout

I webteknologier og CSS findes et koncept kaldet flexible box layout, ofte bare kaldet flexbox. I .NET MAUI har vi et tilsvarende layout kaldet `FlexLayout`.

`FlexLayout` wrapper items til næste row eller column afhængigt af den angivne `Direction`, hvis noget ikke kan være på skærmen. Det er den største forskel til stack layouts: stack layouts renderer ikke items, der ikke er plads til — de går tabt for UI'et.

FlexLayout er næsten en direkte oversættelse af CSS flexbox og har lignende properties, så man kan arrangere child items i enten rows eller columns og angive spacing og alignment.

### Kodeeksempel

```xml
<FlexLayout
     Direction="Column"
     AlignItems="Center" >
```

I sammenligningen på slidesene: med `HorizontalStackLayout` er items, der ikke passer på skærmen, ikke synlige. Med `FlexLayout` wrappes items, der ikke passer, til næste row.

## 11. BindableLayout

`BindableLayout` gør det muligt for en hvilken som helst layout-klasse at generere sit indhold ved at binde til en collection af items, med mulighed for at sætte hvert items udseende med en `DataTemplate`.

Properties:

- `ItemsSource`: angiver den collection af `IEnumerable`-items, layoutet skal vise.
- `ItemTemplate`: angiver den `DataTemplate`, der skal anvendes på hvert item i den viste collection.
- `ItemTemplateSelector`: angiver den `DataTemplateSelector`, der bruges til at vælge en `DataTemplate` for et item at runtime.

## 12. Referencer og links

- *.NET MAUI in Action*, Matt Goldman
- Fonts in .NET MAUI — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/fonts
- ScrollView — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/scrollview
- FlexLayout — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/flexlayout
- BindableLayout — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/bindablelayout
