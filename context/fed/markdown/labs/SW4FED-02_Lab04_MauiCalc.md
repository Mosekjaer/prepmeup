# Lab 04 – MauiCalc

## Metadata

- **Lektion:** L04 – Lab 04, FED MAUI Lab 04 MauiCalc App
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L04/Lab4/FED Lab 04 MauiCalc.pdf
- **Emner dækket:**
  - Grid med rows, columns, RowSpacing og ColumnSpacing
  - Placering af Buttons med Grid.Row og Grid.Column
  - ColumnSpan til en Label over hele øverste row
  - Registrering af custom font (LCD) i MauiProgram
  - Farve i ResourceDictionary og StaticResource-binding
  - Delt event handler til alle knapper
  - Beregningslogik i code-behind

---

## Formål

At få erfaring med Layouts.

## Forudsætninger

Du har læst kapitel 5 i *MAUI in Action*.

## Overordnet opgavebeskrivelse

Lav en lommeregner: MauiCalc-appen.

MauiCalc laves for at øve og demonstrere forståelse af Layouts.

**Note:** MauiCalc-appen har 16 Buttons og 1 Label med LCD-font og baggrundsfarve `#D1E0BA`.

## Step 1: Opret et nyt .NET MAUI-projekt kaldet MauiCalc

1. Opret et nyt .NET MAUI-projekt kaldet MauiCalc i Visual Studio.
2. Åbn `MainPage.xaml` og slet alt mellem `<ContentPage...>` og `</ContentPage>`-taggene.
3. Slet den præfabrikerede `ScrollView` og alt inde i den. Der bør kun være `<ContentPage...>` `</ContentPage>`-taggene tilbage.
4. Slet den eksisterende `OnCounterClicked`-metode i `MainPage.xaml.cs`.

## Step 2: Tilføj et Grid til siden

1. Åbn `MainPage.xaml`.
2. Tilføj row- og column-definitioner:
   - Grid'et har i alt 4 columns og 5 rows med proportional størrelse.
   - `RowSpacing` og `ColumnSpacing` sat til 2.

## Step 3: Tilføj Buttons i XAML

1. Åbn `MainPage.xaml` og tilføj alle Buttons inde i Grid'et.
   - Buttons til tal (0, 1, 2, ..., 9) og operatorer (+, -, /, *, =).
   - Placér Buttons fra row 1 og nedefter.
     - Note: den øverste row (row 0) skal være fri til at vise tallene fra brugerens tryk og resultatet efter beregninger.
     - Lommeregneren skal have i alt 16 knapper.
   - Tilføj et `Clicked`-event på hver knap med event handler-navnet `Button_Clicked`.
     - Event handleren skal også ligge i `MainPage.xaml.cs`.

### Kodeeksempel: en enkelt knap

```xml
<Button Grid.Row="1"
        Grid.Column="0"
        CornerRadius="0"
        Text="+"
        Clicked="Button_Clicked"/>
```

Husk at give den rigtige row og column, den rigtige tekst, og event handler-navnet.

Knapperne fordeles over rows 1–4 og columns 0–3, altså 16 celler i alt.

## Step 4: Tilføj en Label i den øverste row

1. Åbn `MainPage.xaml`.
2. Tilføj en Label i den øverste row til at vise beregningsresultaterne:
   - Labelen skal vise de tal, brugeren indtaster, samt resultatet.
   - Brug `ColumnSpan` til at spænde den øverste row over 4 columns: placér den i `Grid.Column="0"` med `ColumnSpan="4"`.
   - Sæt tekstjusteringen til `End` — både `HorizontalTextAlignment` og `VerticalTextAlignment`.
   - Husk at give Labelen et navn, f.eks. `LCD`, så den kan bruges fra code-behind.

### Kodeeksempel

```xml
<Label Grid.Row="0"
   Grid.Column="0"
   Grid.ColumnSpan="4"
   FontSize=".."
   Padding=".."
   TextColor=".."
   HorizontalTextAlignment="End"
   VerticalTextAlignment="End"
   x:Name="LCD"/>
```

Værdierne for `FontSize`, `Padding` og `TextColor` kan justeres efter behov.

## Step 5: Registrér en ny font (LCD) og brug den i appen

1. Download en LCD-fontfil (`LCD.ttf` fra BrightSpace).
2. Placér filen i mappen `Resources/Fonts`.
3. Tilføj fonten til appen — åbn `MauiProgram.cs` og brug `AddFont`-metoden:

```csharp
fonts.AddFont("LCD.ttf", "LCD");
```

4. Sæt farve til LCD-fonten. Brug farvekoden `#D1E0BA` og tilføj farven i `ResourceDictionary` i `Resources/Styles.xaml`:

```xml
<Color x:Key="LcdBackgroundColor">#D1E0BA</Color>
```

5. Opdatér Labelen, så den bruger LCD-fonten, og sæt dens `BackgroundColor` til den definerede farve:

```xml
<Label ...
       ...
       BackgroundColor="{StaticResource LcdBackgroundColor}"
       FontFamily="LCD"
       ... />
```

## Step 6: Implementér code-behind og event handleren Button_Clicked

### Felter i MainPage.xaml.cs

```csharp
public partial class MainPage : ContentPage
{
    //Declare a string variable to store the current number is entered
    public string CurrentInput { get; set; } = String.Empty;
    //Declare a string variable to store the running total after currently calculated
    public string RunningTotal { get; set; } = String.Empty;

    //Declare a private string variable to store the selected operator
    private string selectedOperator;

    //Declare an array of operators
    string[] operators = { "+", "-", "/", "X", "=" };
    //Declare an array of the available numbers
    string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };
    //Declare a Boolean to determine whether the screen will reset the next time the user presses a Button

    bool resetOnNextInput = false;
```

### Logik i Button_Clicked

```csharp
//Cast sender to Button type to read its text
var btn = sender as Button;
//Assigns the value of the Button's Text property to a temporary variable
var thisInput = btn.Text;

if (numbers.Contains(thisInput))
{
    if (resetOnNextInput)
    {
        CurrentInput = btn.Text;
        resetOnNextInput = false;
    }
    else
    {
        CurrentInput += btn.Text;
    }
    LCD.Text = CurrentInput;
}
else if (operators.Contains(thisInput))
{
     //PerformCalculation method to calculate operation(+,-,/,=)
     var result = PerformCalculation();

     if (thisInput == "=")
     {
         CurrentInput = result.ToString();
         LCD.Text = CurrentInput;
         RunningTotal = String.Empty;
         selectedOperator = String.Empty;

         resetOnNextInput = true;
     }
     else
     {
         RunningTotal = result.ToString();
         selectedOperator = thisInput;
         CurrentInput = String.Empty;
         LCD.Text = CurrentInput;
     }
}
```

## Step 7: Implementér beregningslogikken (PerformCalculation)

Implementér metoden, der udfører beregninger, når operator-knapperne (+, -, *, /, =) klikkes. Selve de fire regnearter skal du selv udfylde.

```csharp
private double PerformCalculation()
{
    double currentVal;
    //Cast CurrentInput from string to double to perform arithmetic operations on it
    double.TryParse(CurrentInput, out currentVal);

    double runningVal;
    //Casts the RunningTotal from string to double to perform arithmetic operations on it
    double.TryParse(RunningTotal, out runningVal);

    double result;

    /* ToDo: Implement the mathematic operation code by the selected operator*/
    switch (selectedOperator)
    {
        case "+":
            result = /* Do it yourself */;
            break;
        case "-":
            result = /* Do it yourself */;
            break;
        case "X":
            result = /* Do it yourself */;
            break;
        case "/":
            result = /* Do it yourself */;
            break;
        default:
            result = currentVal;
            break;
    }
    return result;
}
```
