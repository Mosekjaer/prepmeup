# Kapitel 5 – Layouts

## Metadata

- **Kapitel:** 5 – Layouts
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L04
- **Hovedemner:**
  - De indbyggede layouts i .NET MAUI og hvornår man bruger hvilket
  - `Grid` som det mest kraftfulde layout: `RowDefinitions`, `ColumnDefinitions`, `RowSpacing`, `ColumnSpacing`
  - Attached properties `Grid.Row`, `Grid.Column`, `Grid.RowSpan`, `Grid.ColumnSpan`
  - Row/column sizing: proportional (`*`), relativt proportional (`4*`) og absolut (DIUs)
  - Registrering af custom fonts i `MauiProgram` og `MauiFont` build action
  - Farver i `ResourceDictionary` (Styles.xaml) og `{StaticResource}`
  - `ScrollView`: orientering, nested scrolling views, "pixel-perfect"-myten
  - `HorizontalStackLayout` og `VerticalStackLayout` samt `Spacing` som layoutværktøj
  - `FlexLayout` som .NET MAUI's pendant til CSS flexbox (wrapping)
  - Kombination af layouts til at bygge vilkårlige UI'er

---

## Indledning

Layout er det vigtigste aspekt af et UI-design. UI-design består fundamentalt af tre aspekter: **layout, typografi og farve**. Man kan skabe et flot UI i sort-hvid med kun ét typeface, men fancy typografi og pæne farver redder ikke et dårligt layout.

> "Layout is the most critical piece of a website's design. It is quite literally the foundation for the rest of the pieces that will eventually be added to the website."
> — *Design for Developers*, Stephanie Stimac, Manning 2022

Layouts inkluderet i .NET MAUI:

- `Grid`
- `HorizontalStackLayout`
- `VerticalStackLayout`
- `FlexLayout`
- `BindableLayout`
- `AbsoluteLayout`

Dette kapitel behandler de tre første grundigt plus `ScrollView`; de sidste tre behandles i kapitel 6.

> **RelativeLayout: Where has it gone?**
> Kommer du fra Xamarin.Forms, kender du måske `RelativeLayout`. Det er inkluderet i .NET MAUI af hensyn til kompatibilitet med Xamarin.Forms, men **du bør ikke bruge det i .NET MAUI-apps**, fordi det kan være dyrt at beregne (flere CPU-cycles) og resultere i langsommere apps.
>
> Du kan opnå de samme resultater som et `RelativeLayout` ved at bruge et `Grid`. Migrerer du fra Xamarin.Forms, bør du overveje at opdatere dit layout til at bruge et `Grid` i stedet.

`ScrollView` er inkluderet i kapitlet, selvom det teknisk set er en control og ikke et layout. Grunden er, at man normalt bruger det som en del af, hvordan man laver sit UI-layout, frem for til at vise noget for brugeren eller få feedback. Kombineret med at den viser child controls, opfører den sig i praksis som et layout.

## 5.1 Grid

`Grid` er det mest kraftfulde layout i .NET MAUI, og de fleste UI-layouts implementeres med et `Grid`. Som navnet antyder, lader `Grid` dig arrangere child-elementer i **rows og columns**.

Det er nemt at forestille sig en tabel, når man hører "rows og columns", men det ville være en begrænsende måde at tænke på `Grid`. **Et `Grid` bruges ikke til at vise rows og columns** — der findes bedre muligheder i .NET MAUI til at vise tabulære data. I stedet bruger et `Grid` rows og columns til at *arrangere dine views på skærmen*. Kender du designværktøjet Figma, er konceptet det samme som de layout grids, Figma og de fleste andre designværktøjer bruger.

### 5.1.1 Grid basics

Man kunne tro, at brug af et `Grid` betyder at følge kvadrater som på ternet papir. Sådan fungerer det ikke i .NET MAUI. **For at bruge et `Grid` beslutter du, hvor mange rows og columns dit view skal opdeles i, og størrelserne på disse rows og columns.**

Kender du det klassiske tre-kolonne-layout fra webdesign, kender du allerede konceptet grid system: man laver layout i rows og columns i stedet for at bruge et fast-størrelse grid til at konstruere sit UI.

**Figure 5.1** viser en hurtig skitse af designet til en calculator-app: en "skærm" øverst til at vise de tal, brugeren indtaster, samt resultatet, og derunder rækker af knapper til tal og matematiske operatorer. Tænker vi på designet som et grid, har det tydeligvis **fem rows og fire columns**. Row- og column-numre starter ved 0, så skærmen ville være i row 0, og knapperne i rows 1–4 og columns 0–3.

### 5.1.2 Building MauiCalc

Opret en ny blank .NET MAUI-app kaldet **MauiCalc**. Åbn `MainPage.xaml` og slet alt mellem `<ContentPage>...</ContentPage>`-taggene.

#### Listing 5.1 The MauiCalc MainPage with the grid added

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiCalc.MainPage">
    <Grid>
    </Grid>
</ContentPage>
```

① Tilføjer et `Grid`-tag til siden.

Row definitions har en **`Height`**-property, og column definitions har en **`Width`**-property. Der er **to måder** at definere rows og columns på.

Den første er at tilføje definitionerne som XAML-elementer inde i `Grid`'et:

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

Her har `Grid`'et to rows og to columns, og i stedet for at angive en bredde eller højde bruger vi en **asterisk (`*`)**, som betyder proportional sizing — de fordeles ligeligt.

Der findes dog en meget enklere måde: at angive row- og column-definitionerne direkte som del af `Grid`-deklarationen:

```xml
<Grid RowDefinitions="*,*"
      ColumnDefinitions="*,*">
</Grid>
```

Det giver samme resultat, men er langt lettere at deklarere.

Vi kan også angive row- og column-spacing med **`RowSpacing`** og **`ColumnSpacing`**. Som standard er der ingen afstand mellem rows og columns i et `Grid`, men med disse properties kan vi angive et mellemrum, specificeret i DIUs.

#### Listing 5.2 Row and column definitions inline

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiCalc.MainPage">
    <Grid ColumnDefinitions="*,*,*,*"
          RowDefinitions="*,*,*,*,*"
          RowSpacing="2"
          ColumnSpacing="2">
    </Grid>
</ContentPage>
```

① Tilføjer column definitions for fire columns.
② Tilføjer row definitions for fem rows.
③ Angiver row spacing på 2.
④ Angiver column spacing på 2.

### Placering af controls i et Grid

For at placere controls i et `Grid` bruger vi **attached properties** kaldet **`Grid.Row`** og **`Grid.Column`**. De kan bruges i enhver control eller ethvert layout til at positionere dem, hvor vi vil have dem. Grid rows og columns starter ved 0 — for en row øverst i `Grid`'et, for en column længst til venstre.

**Figure 5.2** viser et `Grid`-layout med fire rows og fire columns og et view positioneret i `Grid.Row` 0, `Grid.Column` 0 (øverste venstre hjørne).

**Figure 5.3** viser et `Grid`-layout med fem rows og tre columns med et view positioneret i `Grid.Row` 2, `Grid.Column` 1 — altså i midten.

```xml
<Button Grid.Row="2"
        Grid.Column="1"
        Text="Click me!" />
```

**Figure 5.4** viser MauiCalc-designet med et nummereret grid lagt over, hvilket gør det let at afgøre hvilken row og column hver knap skal tildeles. Fx skal knappen for tallet 1 være i row 3, column 0, og knappen for 9 i row 1, column 2. Ingen af knapperne er i row 0, som er reserveret til skærmen.

#### Listing 5.3 MainPage with the buttons added

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiCalc.MainPage">
    <Grid ColumnDefinitions="*,*,*,*"
          RowDefinitions="*,*,*,*,*"
          RowSpacing="2"
          ColumnSpacing="2">

        <!-- Row 1 -->
        <Button Grid.Row="1"
                Grid.Column="0"
                CornerRadius="0"
                Text="7"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="1"
                Grid.Column="1"
                CornerRadius="0"
                Text="8"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="1"
                Grid.Column="2"
                CornerRadius="0"
                Text="9"
                Clicked="Button_Clicked"/>

        <!-- Row 2 -->
        <Button Grid.Row="2"
                Grid.Column="0"
                CornerRadius="0"
                Text="4"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="2"
                Grid.Column="1"
                CornerRadius="0"
                Text="5"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="2"
                Grid.Column="2"
                CornerRadius="0"
                Text="6"
                Clicked="Button_Clicked"/>

        <!-- Row 3 -->
        <Button Grid.Row="3"
                Grid.Column="0"
                CornerRadius="0"
                Text="1"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="3"
                Grid.Column="1"
                CornerRadius="0"
                Text="2"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="3"
                Grid.Column="2"
                CornerRadius="0"
                Text="3"
                Clicked="Button_Clicked"/>

        <!-- Row 4 -->
        <Button Grid.Row="4"
                Grid.Column="0"
                CornerRadius="0"
                Text="."
                Clicked="Button_Clicked"/>
        <Button Grid.Row="4"
                Grid.Column="1"
                CornerRadius="0"
                Text="0"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="4"
                Grid.Column="2"
                CornerRadius="0"
                Text="="
                Clicked="Button_Clicked"/>

        <!-- Column 3 (Operator buttons) -->
        <Button Grid.Row="1"
                Grid.Column="3"
                CornerRadius="0"
                Text="+"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="2"
                Grid.Column="3"
                CornerRadius="0"
                Text="-"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="3"
                Grid.Column="3"
                CornerRadius="0"
                Text="X"
                Clicked="Button_Clicked"/>
        <Button Grid.Row="4"
                Grid.Column="3"
                CornerRadius="0"
                Text="/"
                Clicked="Button_Clicked"/>

    </Grid>
</ContentPage>
```

① Tilføjer en `Button` til `Grid`'et og placerer den i row 1.
② Placerer knappen i column 0.
③ Sætter `CornerRadius` til 0 (vi vil have firkantede hjørner).
④ Sætter `Text` til 7 (den øverste venstre talknap).
⑤ Sætter `Clicked` til en event handler kaldet `Button_Clicked`.
⑥–⑪ Videre placering i de øvrige rows og den højre operator-column.

To ting at bemærke: for det første har hver knap sit `Clicked`-event delegeret til den *samme* event handler `Button_Clicked` — at tilføje en handler pr. knap ville være uhåndterligt. For det andet begynder vi at placere knapper på row 1 i stedet for row 0, som er reserveret til skærmen.

### ColumnSpan og RowSpan

Vi vil have skærmen til at fylde hele den øverste row frem for kun én column. Til det bruger vi **`ColumnSpan`**, som lader dig deklarere, at dit view — selvom det starter i en bestemt column — skal spænde over det angivne antal columns. MauiCalc har fire columns, så skærmen placeres i `Grid.Column` 0 med `ColumnSpan` 4.

Det samme trick virker for rows: vil du have et view til at optage mere end én vertikal row, bruger du **`RowSpan`**.

#### Listing 5.4 The LCD screen row

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiCalc.MainPage">
    <Grid ColumnDefinitions="*,*,*,*"
          RowDefinitions="*,*,*,*,*"
          RowSpacing="2"
          ColumnSpacing="2">

        <Label Grid.Row="0"
               Grid.Column="0"
               Grid.ColumnSpan="4"
               FontSize="72"
               Padding="20"
               TextColor="Black"
               HorizontalTextAlignment="End"
               x:Name="LCD"/>

        <!-- Buttons omitted for brevity -->

    </Grid>
</ContentPage>
```

① Tilføjer en `Label` til `Grid`'et og placerer den i row 0.
② Placerer `Label`'en i column 0.
③ Angiver `ColumnSpan` 4, så den fylder hele bredden af vores fire-column-`Grid`.
④ Sætter `FontSize` til 72; vi vil have store tal.
⑤ Giver `Label`'en en padding-værdi på 20 DIUs, så der er et mellemrum mellem tallene og skærmens kanter.
⑥ Sætter tekstfarven til sort.
⑦ Sætter `HorizontalTextAlignment` til `End`; tal på en lommeregnerdisplay kommer ind fra højre side.
⑧ Giver `Label`'en et navn, så vi kan sætte dens `Text`-property i kode.

#### Listing 5.5 The Button_Clicked method

```csharp
private void Button_Clicked(object sender, EventArgs e)
{
}
```

① Tilføjer en metode kaldet `Button_Clicked` med den standard event handler-signatur.

Slet derefter den eksisterende `OnCounterClicked`-metode, hvorefter appen kan kompilere og køre. **Figure 5.5** viser MauiCalc kørende på Windows med knapperne i et grid i rows 1–4 og et talvisningsfelt i row 0 spredt over alle fire columns.

### Fonts og farver

Nu hvor layoutet er på plads, kan vi tænke på de to andre aspekter: farve og typografi. Vi giver skærmen en mere LCD-agtig baggrundsfarve og en LCD-styled font. Skeuomorfisme er måske gået af mode, men virker stadig i visse scenarier.

Placér fontfilen `LCD.ttf` i `Resources/Fonts`-mappen. For at registrere en font tilføjer vi den til **`ConfigureFonts`**-extension method på `MauiAppBuilder`. `IFontCollection` sendes allerede ind i lambda-metoden, så vi kalder blot `fonts.AddFont` med filnavnet og et alias.

#### Listing 5.6 Adding a font registration to MauiProgram

```csharp
namespace MauiCalc;

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
                fonts.AddFont("LCD.ttf", "LCD");                 // ①
            });

        return builder.Build();
    }
}
```

① Tilføjer en font-registrering i `MauiProgram`, registrerer `LCD.ttf` og giver den aliaset `LCD`.

> **Registering fonts**
> Du definerer hvilke filer i dit projekt der er fonts ved at sætte deres **`BuildAction`-property til `MauiFont`**. Det kan gøres via Properties-panelet i Visual Studio eller ved at registrere det i `.csproj`-filen:
>
> ```xml
> <MauiFont Include="[your font file]" />
> ```
>
> Kigger du i `MauiCalc.csproj` nu, ser du allerede en entry der matcher dette, men med et wildcard for hele `Resources/Fonts`-mappen. Vil du tilføje en font til din .NET MAUI-app, kopierer du blot fontfilen til denne mappe, hvorefter den er tilgængelig for registrering ved filnavn i `MauiProgram`-configuration builderen.
>
> Kommer du fra Xamarin.Forms, vil du bemærke den slående forskel i hvor let dette er sammenlignet med den gamle måde.

I stedet for en foruddefineret farve tilføjer vi den specifikke farve til `ResourceDictionary` i `Resources/Styles.xaml`.

#### Listing 5.7 The LCD color in Styles.xaml

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <Color x:Key="LcdBackgroundColor">#D1E0BA</Color>

    <!-- code omitted for brevity -->
</ResourceDictionary>
```

① Tilføjer en farve med hex-værdien `D1E0BA` og giver den key'en `LcdBackgroundColor`, så vi kan referere den ved navn.

#### Listing 5.8 The LCD styling in MainPage

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage ...>
    <Grid ...>
        <Label Grid.Row="0"
               Grid.Column="0"
               Grid.ColumnSpan="4"
               BackgroundColor="{StaticResource LcdBackgroundColor}"
               FontFamily="LCD"
               FontSize="72"
               Padding="20"
               TextColor="Black"
               HorizontalTextAlignment="End"
               VerticalTextAlignment="End"
               x:Name="LCD"
               Text="456789"/>

        <!-- code omitted for brevity -->
    </Grid>
</ContentPage>
```

① Sætter `BackgroundColor` på `Label`-controlen til den farve, vi definerede i Styles.xaml.
② Sætter `FontFamily` på `Label`'en til den font, vi definerede i MauiProgram.cs.
③ `Text`-propertyen er sat til nogle tal, så vi kan få en fornemmelse af hvordan det ser ud ved kørsel.

**Figure 5.6** viser MauiCalc på Windows med LCD-font og baggrundsfarve anvendt på `Label`'en for et mere lommeregner-agtigt udseende.

### Beregningslogikken

Slet `Text`-propertyen fra LCD-`Label`-controlen, og opdater `MainPage.xaml.cs`:

#### Listing 5.9 The full code for MainPage.xaml.cs

```csharp
namespace MauiCalc;

public partial class MainPage : ContentPage
{
    public string CurrentInput { get; set; } = String.Empty;              // ①
    public string RunningTotal { get; set; } = String.Empty;              // ②
    private string selectedOperator;                                      // ③

    string[] operators = { "+", "-", "/", "X", "=" };                     // ④
    string[] numbers = { "0", "1", "2", "3", "4",
                         "5", "6", "7", "8", "9", "." };                  // ⑤

    bool resetOnNextInput = false;                                        // ⑥

    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;                                       // ⑦
        var thisInput = btn.Text;                                         // ⑧

        if (numbers.Contains(thisInput))                                  // ⑨
        {
            if (resetOnNextInput)                                         // ⑩
            {
                CurrentInput = btn.Text;                                  // ⑪
                resetOnNextInput = false;                                 // ⑫
            }
            else
            {
                CurrentInput += btn.Text;                                 // ⑬
            }

            LCD.Text = CurrentInput;                                      // ⑭
        }
        else if (operators.Contains(thisInput))                           // ⑮
        {
            var result = PerformCalculation();                            // ⑯

            if (thisInput == "=")                                         // ⑰
            {
                CurrentInput = result.ToString();                         // ⑱
                LCD.Text = CurrentInput;                                  // ⑲

                RunningTotal = String.Empty;                              // ⑳
                selectedOperator = String.Empty;                          // ㉑
                resetOnNextInput = true;                                  // ㉒
            }
            else
            {
                RunningTotal = result.ToString();                         // ㉓
                selectedOperator = thisInput;                             // ㉔
                CurrentInput = String.Empty;                              // ㉕
                LCD.Text = CurrentInput;                                  // ㉖
            }
        }
    }

    private double PerformCalculation()
    {
        double currentVal;
        double.TryParse(CurrentInput, out currentVal);                    // ㉗

        double runningVal;
        double.TryParse(RunningTotal, out runningVal);                    // ㉘

        double result;

        switch (selectedOperator)                                         // ㉙
        {
            case "+":
                result = runningVal + currentVal;
                break;
            case "-":
                result = runningVal - currentVal;
                break;
            case "X":
                result = runningVal * currentVal;
                break;
            case "/":
                result = runningVal / currentVal;
                break;
            default:
                result = currentVal;
                break;
        }

        return result;                                                    // ㉚
    }
}
```

① Deklarerer en string-variabel til det aktuelle tal, brugeren indtaster.
② Deklarerer en string-variabel til den løbende sum.
③ Deklarerer en privat string-variabel til den valgte aritmetiske operator.
④ Deklarerer et array af strings med de tilgængelige operatorer at sammenligne brugerens valg mod.
⑤ Deklarerer et array af strings med de tal, brugeren kan vælge.
⑥ Deklarerer en boolean, der afgør om skærmen nulstilles ved næste knaptryk.
⑦ Caster `sender` i `Button_Clicked`-event handleren til `Button`, så vi kan læse dens `Text`-property.
⑧ Tildeler knappens `Text`-værdi til en midlertidig variabel.
⑨ Tjekker om den trykkede knap er et tal.
⑩ Tjekker om displayet skal nulstilles (fx hvis sidste tryk var lig med eller en operator).
⑪ Hvis displayet nulstilles, sættes `CurrentInput` til det netop trykkede tal.
⑫ Sikrer at displayet ikke nulstiller igen, hvis brugeren indtaster endnu et tal.
⑬ Hvis displayet ikke nulstilles (brugeren tilføjer cifre), appendes det trykkede ciffer.
⑭ Sætter `Text` på LCD-`Label`'en til `CurrentInput`.
⑮ Tjekker om den trykkede knap er en aritmetisk operator.
⑯ Kalder `PerformCalculation` og tildeler resultatet til en midlertidig variabel.
⑰ Tjekker om brugeren har trykket lig med eller en anden operator, så vi ved om den løbende sum skal nulstilles efter opdatering af displayet.
⑱ Sætter `CurrentInput` til resultatet af beregningen.
⑲ Opdaterer LCD-`Label`'ens `Text` til at vise resultatværdien.
⑳ Nulstiller den løbende sum, da brugeren har trykket lig med.
㉑ Nulstiller den valgte operator.
㉒ Sætter displayet til at nulstille ved næste knaptryk.
㉓ Sætter den løbende sum til resultatet af beregningen.
㉔ Sætter den valgte operator til den trykkede knaps operator, så den kan bruges til at beregne den løbende sum.
㉕ Sætter `CurrentInput` til tom.
㉖ Sætter LCD-`Label`'ens `Text` til `CurrentInput`.
㉗ Caster `CurrentInput`, som er en string, til en `double` så vi kan udføre aritmetik.
㉘ Caster `RunningTotal` til en `double` af samme grund.
㉙ Tjekker hvilken operator brugeren har valgt, så vi kan udføre den rette operation.
㉚ Returnerer resultatet af beregningen.

### 5.1.3 Row and column sizing

Kører man appen på en telefon (Android eller iOS), virker layoutet af knapperne ikke lige så pænt som på desktop. **Figure 5.7** viser problemet: fordi rows og columns er proportionale, ser knapperne ubehageligt aflange ud på en enhed i portræt-orientering.

At løse dette er et designspørgsmål snarere end et teknisk problem. **Figure 5.8** viser et opdateret design for MauiCalc, der holder knapperne samme størrelse uafhængigt af skærmens størrelse.

Vi har hidtil gjort alle rows og columns til en lige andel af den samlede tilgængelige højde eller bredde, men der er to andre måder.

**Den første er stadig proportional sizing, men med et forhold frem for en lige andel.** Det gøres ved at sætte et tal foran asterisken. Eksempel: vil vi have den første column til at fylde halvdelen af skærmen, den anden en tredjedel og den sidste en sjettedel, og for rows: den første 1/12, den midterste 3/12 og den sidste 8/12:

```xml
ColumDefinitions="3*, 2*, 1*"
RowDefinitions="1*, 3*, 8*"
```

<!-- "ColumDefinitions" står sådan i råteksten; korrekt navn er ColumnDefinitions -->

**Det er vigtigt at huske, at dette er relative proportioner, ikke DIUs.** Row-definitionerne kan derfor også skrives som

```xml
RowDefinitions="4*, 12*, 32*"
```

og give præcis samme resultat. **Figure 5.9** illustrerer dette `Grid` med relativt proportionerede rows og columns.

Vi bruger samme logik i MauiCalc: skærmen skal fylde den øverste halvdel af den tilgængelige plads, og knapperne resten. Der er fem rows — én til skærmen og fire til knapper. Skærm-rowen skal derfor være fire gange så høj som en knap-row og vil dermed fylde halvdelen af den vertikale plads. Det opnås ved at sætte skærm-rowens højde til `4*` og lade alle knap-rows være `*` (det er ikke nødvendigt at skrive `1*`).

#### Listing 5.10 Updated Grid with proportional screen and rows

```xml
<Grid ColumnDefinitions="*,*,*,*"
      RowDefinitions="4*,*,*,*,*"
      RowSpacing="2"
      ColumnSpacing="2">
```

① Højden på den første row er ændret fra `*` til `4*`, og givet at der er fire andre rows med højde `*`, fylder den nu halvdelen af `Grid`'ets tilgængelige højde.

**Figure 5.10** viser den opdaterede app på Android — en stor forbedring, der åbner plads øverst til mere information som den løbende sum eller en memory-funktion. **Figure 5.11** viser samme layout på Windows, hvor vi nu har det omvendte problem: knapperne er strakt horisontalt. Og et vindue på et desktop-OS kan resizes, hvilket kan give resultatet i **Figure 5.12** (vinduet strakt horisontalt).

For at nå det skitserede design fra figure 5.8 skal vi kombinere proportional sizing med **absolute sizing**. Absolute sizing er let: i stedet for en proportion angiver man en værdi i DIUs. Dermed kan vi gøre hver knap præcis kvadratisk.

#### Listing 5.11 MauiCalc with fixed height and width buttons

```xml
<Grid ColumnDefinitions="100,100,100,100"
      RowDefinitions="*,100,100,100,100"
      HorizontalOptions="Center"
      RowSpacing="2"
      ColumnSpacing="2">
```

① Alle columns er nu angivet som 100 DIUs brede.
② Knap-rows er alle angivet som 100 DIUs høje, mens skærmen får lov at tage den resterende tilgængelige højde.
③ Sætter `HorizontalOptions` til `Center`, så knapperne og skærmen vises i midten af vinduet.

**Figure 5.13** viser MauiCalc på Windows med fast højde og bredde på knapperne. Det matcher nu det tilsigtede design og giver et konsistent udseende på tværs af desktop og mobil, men der er plads til forbedring.

**Figure 5.14** viser FancyCalc — samme app med små UI-ændringer (baggrundsfarve, LCD-tekstfarve, corner radius på knapperne), men **layoutet er uændret**. Bortset fra LCD-tekstfarven ligger forskellene mellem MauiCalc og FancyCalc i `Styles.xaml`-filen.

## 5.2 ScrollView

`ScrollView` er en control, der lader sit child-indhold scrolle. Den er nyttig, når man har indhold af ubestemt længde og skal præsentere mere indhold, end der kan være på skærmen.

**Figure 5.15** viser forskellen: til venstre er en stor mængde tekst i en `VerticalStackLayout` — indholdet er for langt til skærmen, så brugeren kan kun læse de første linjer. Til højre vises samme tekst i en `ScrollView`, så brugeren kan swipe op og ned for at se hele teksten.

Som standard er `ScrollView`'ets orientering (scrolleretningen) **vertikal**. Man kan også angive **horizontal**, **both** eller **neither**. `Both` kan være nyttigt til at vise et billede i fuld skala og lade brugeren panorere rundt i det. Det kan også være en nyttig accessibility-feature: nogle brugere foretrækker stor skrift, og at kunne scrolle teksten både vertikalt og horisontalt kan være at foretrække frem for at scrolle vertikalt gennem mange linjer med kun ét-to ord.

> **Take care with nested scrolling views**
> Vær forsigtig med at neste scrollende views i hinanden, da det kan gøre UI'et svært eller umuligt at bruge. Et oplagt eksempel er at *ikke* placere én `ScrollView` inde i en anden. Mere subtile eksempler: en `CollectionView` inde i en `ScrollView`, eller en `ScrollView` i et bottom sheet.
>
> Når brugeren swiper op eller ned, kan resultatet blive uventet, fordi din app ikke nødvendigvis scroller det view, de forventer. En mere præcis advarsel er derfor: **brug ikke overlappende control gestures.**
>
> Undtagelsen er scrollende views, hvor scrolleretningen er **vinkelret**. Har du fx en side der scroller vertikalt, kan du inkludere en horisontalt scrollende `CollectionView` i siden, da der ikke er tvetydighed mellem scrolleretningerne.

En populær trend er at bruge `ScrollView` som omsluttende container for hele sider. Fordi enheder findes i mange forskellige former og størrelser, er det blevet umuligt for designere at skabe et UI, der ser ens ud på alle skærm-aspect ratios. Dybden af problemet illustreres af design-trenden væk fra "pixel-perfect"-designs og mod design systems.

> **NOTE** "Pixel-perfect-myten" er en god grund til, at default-tilgangen i .NET MAUI (consistent but not identical, jf. kapitel 1) ofte fører til meget bedre UI og UX. Det betyder ikke, at man ikke kan opnå et pixel-perfect UI med .NET MAUI, men det er værd at overveje, om en konsistent oplevelse er vigtigere.

**Figure 5.16** illustrerer valget: et mobil-UI-design skabt til en lang skærm (venstre). Skal samme UI renderes på en skærm med et andet aspect ratio, der gør enheden mindre "høj", har man to muligheder. Den første er at *presse* indholdet sammen, så det hele passer på skærmen (midten). Den anden er at omslutte indholdet i en `ScrollView` (højre), så indholdet fremstår i samme størrelse som det oprindelige design, og brugeren kan scrolle for at se det, der ikke er plads til.

En fjerde mulighed ville være at designe et responsivt UI med plads til, at visuelle elementer kan repositioneres uden at kompromittere selve elementerne. Den tilgang virker op til et punkt, men har sine grænser (behandles i kapitel 10).

## 5.3 HorizontalStackLayout and VerticalStackLayout

De to stack layouts er de simpleste layouts og formentlig de letteste til hurtige prototyper eller trivielle apps. `VerticalStackLayout` og `HorizontalStackLayout` fungerer på samme måde bortset fra deres orientering. De har en collection af typen `View` kaldet **`Children`**, og views tilføjes til stakken og renderes på skærmen **i den rækkefølge de tilføjes**. Man kan tilføje enhver control eller ethvert layout til dem.

Man kan kombinere `VerticalStackLayout` og `HorizontalStackLayout` til at skabe næsten ethvert UI.

**Figure 5.17** viser CellBoutique genfortolket med kort til produkterne frem for en `CarouselView`: kortene vises i en vertikal stack, og hvert kort bruger en vertikal og en horisontal stack til at layoute sit indhold. Her er `CarouselView` erstattet af en `CollectionView` (man kunne også bruge en `VerticalStackLayout` inde i en `ScrollView` og hardcode produkterne), og kort-templaten er bygget udelukkende med `VerticalStackLayout` og `HorizontalStackLayout`.

**Figure 5.18** nedbryder det: hvert kort er selv en `HorizontalStackLayout`, som arrangerer sine children horisontalt fra venstre mod højre. Children er en `VerticalStackLayout` til produkttitel og -beskrivelse samt en `Label` til prisen.

**Figure 5.19** viser det første child view: en `VerticalStackLayout` bruges til at arrangere produkttitel og -beskrivelse over hinanden, fra top til bund.

### Spacing

Vi kan faktisk fjerne alle borders fra disse kort og alene forlade os på layout og stadig have et anstændigt UI. Det opnås med **spacing**. `HorizontalStackLayout` og `VerticalStackLayout` har en **`Spacing`**-property, som tilføjer et mellemrum mellem child views i en stack.

**Spacing er et af — hvis ikke *det* — vigtigste aspekter af layout.** Det hjælper med at definere hierarki i dit UI og relationer mellem forskellige elementer.

**Figure 5.20** viser dette UI uden borders og shadows, alene med spacing til at gruppere elementer. Prisen er flyttet til venstre, fordi den uden borderen ser ud til at flyde uden reference — at flytte den til venstre definerer bedre relationen mellem prisen og de øvrige elementer. Afstanden mellem kort er øget, og forskellen i spacing mellem hver gruppe af elementer og elementerne *inden i* hver gruppe efterlader ingen tvivl om, hvilken pris, titel og beskrivelse der hører sammen.

## 5.4 FlexLayout

Kender du webteknologier og CSS, har du utvivlsomt hørt om **flexible box layout**, ofte bare kaldet flexbox. I .NET MAUI har vi et lignende layout kaldet **`FlexLayout`**.

`FlexLayout` har mange ligheder med `HorizontalStackLayout` og `VerticalStackLayout`, men med vigtige forskelle. **Den vigtigste forskel er, at med de to stack layouts vil noget, der ikke passer på skærmen, simpelthen ikke blive renderet og går tabt for UI'et — mens `FlexLayout` wrapper items til næste row eller column**, afhængigt af den angivne retning.

**Figure 5.21** viser forskellen: øverst arrangerer en `HorizontalStackLayout` child-items, og de items der ikke passer på skærmen er simpelthen ikke synlige. Nederst arrangerer et `FlexLayout` med `direction` sat til `row` de samme child-items, og items der ikke passer, wrappes til næste row.

`FlexLayout` er næsten en direkte oversættelse af CSS flexbox og har lignende properties til at arrangere child-items i enten rows eller columns og angive spacing og alignment. To forskellige måder at bruge `FlexLayout` på behandles i kapitel 6.

## Summary

- `Grid` er et kraftfuldt layout, og du kan bruge det til at skabe næsten ethvert UI. `Grid` bruges til at arrangere child views i rows og columns.
- Du kan bruge en `ScrollView` til at gøre mere indhold, end der er plads til på skærmen, tilgængeligt for brugerne. Du kan bruge `ScrollView` til en scrollende sektion af en side eller endda wrappe hele siden i en `ScrollView` for at imødekomme forskellige skærmstørrelser.
- `HorizontalStackLayout` og `VerticalStackLayout` er simple layouts til at arrangere child views efter hinanden, vertikalt eller horisontalt. De fleste simple layouts kan opnås med en kombination af disse.
- Du kan kombinere layouts i .NET MAUI, og i virkelige apps vil du sandsynligvis gøre det. At kombinere dem er den bedste måde at bygge de UI'er, dine designere giver dig.
