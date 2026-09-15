# L11 – Custom controls

## Metadata

- **Lektion:** L11 – Custom controls
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L11/Custom controls.pdf (12 slides)
- **Emner dækket:**
  - Tre måder at bygge eller tilpasse controls i .NET MAUI
  - Componentization og `ContentView`
  - Eksempel: custom stepper med redigerbart værdifelt
  - Layout og code-behind for en custom control
  - Brug af custom control fra en `ContentPage`
  - Bindable properties og `BindableObject`
  - `BindableProperty.Create` og navngivningskonvention

---

## 1. Tre måder at tilpasse controls

I .NET MAUI har man tre måder at bygge eller tilpasse controls på:

- Bundle controls into a reusable component — samle controls i en genbrugelig komponent.
- Customize the platform implementations that come in the box — tilpasse de platform-implementationer, der følger med.
- Draw your own controls and graphics — tegne sine egne controls og grafik med biblioteket `Microsoft.Maui.Graphics`.

## 2. Componentization

Componentization er en kernefunktion i ethvert moderne UI-framework. Selvom man kan bygge hele sit UI med de elementære controls, der er i frameworket, er en mere effektiv tilgang at kombinere disse controls til genbrugelige komponenter.

I .NET MAUI bygges disse genbrugelige komponenter med `ContentView`.

## 3. Eksempel — hvorfor en custom stepper

En `Label` og en `Stepper` bruges til henholdsvis at vise den nuværende tælling og lade brugeren indtaste den. Alle andre steder, hvor denne funktionalitet er nødvendig, er man nødt til at tilføje begge controls igen.

En almindelig løsning på dette problem er at tilbyde et input-felt med en `Stepper` indbygget. Men det er ikke særligt touchvenligt og derfor ikke et godt valg til mobil.

I stedet kan vi lave en custom stepper control med et redigerbart værdifelt.

## 4. Opbygning af layoutet til den custom stepper

- Tilføj en mappe kaldet `Controls`.
- Højreklik på mappen `Controls` og vælg **Add new item**.
- Vælg **.NET MAUI ContentView (XAML)**.

## 5. Custom control layout

Layoutet er et `Grid` med tre kolonner: minus-knap, `Entry` til værdien og plus-knap.

### Kodeeksempel

```xml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="CustomControlsDemo.Controls.MyStepper">
    <Grid ColumnDefinitions="50,120,50">
        <Button Grid.Column="0"
                Text="-"
                Clicked="MinusButton_Clicked"
                x:Name="MinusButton"
                VerticalOptions="Center"
                HorizontalOptions="Center"
                MinimumWidthRequest="50"/>
        <Entry Grid.Column="1"
                x:Name="ValueEntry"
                FontSize="42"
                HorizontalTextAlignment="Center"
                TextChanged="ValueEntry_TextChanged"
                VerticalOptions="Center"
                HorizontalOptions="Center"/>
        <Button Grid.Column="2"
```

<!-- uklart i kilden: slide 6 er beskåret, plus-knappen og de afsluttende tags mangler -->

## 6. Custom control implementation

Code-behind arver fra `ContentView`, initialiserer værdien til `"0"` og håndterer knapklik samt tekstændringer.

### Kodeeksempel

```csharp
public partial class MyStepper : ContentView
{
    public MyStepper()
    {
        InitializeComponent();
        ValueEntry.Text = "0";
    }

    public int Value { get; set; }

    private void MinusButton_Clicked(object sender, EventArgs e)
    {
        Value--;
        ValueEntry.Text = Value.ToString();
    }

    private void ValueEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(e.NewTextValue, out var value))
        {
            Value = value;
        }
```

<!-- uklart i kilden: slide 7 er beskåret, de afsluttende tuborgklammer mangler -->

## 7. Brug af den custom control

Namespace for `Controls`-mappen erklæres i XAML, hvorefter controllen kan bruges som ethvert andet element.

### Kodeeksempel

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:CustomControlsDemo.Controls"
             x:Class="CustomControlsDemo.MainPage">

    <ScrollView>
        <VerticalStackLayout Spacing="25"
                             Padding="30,0"
                             VerticalOptions="Center">
            <controls:MyStepper HorizontalOptions="Center"
                                VerticalOptions="Center"
                                />
```

Demo — branch: `customcontrol-v1`.

## 8. Bindable properties

Et kernebegreb i .NET MAUI er **bindable properties**. Det er en udvidelse af de klasse-properties, man kender fra .NET (altså et public klassemedlem med en getter og setter), men med yderligere funktionalitet, der blandt andet muliggør data binding.

Target for en binding skal arve fra baseklassen `BindableObject` — og `ContentView` er en efterkommer af `BindableObject`. Target-propertyen for en binding skal desuden være en bindable property.

`Value`-propertyen i `MyStepper`-controllen er en property, men ikke en bindable property. Det kan vi lave om på.

## 9. Tilføjelse af Value som bindable property

- Bindable properties backer almindelige properties (i stedet for felter).
- Navnet på en bindable property skal matche navnet på den property, den backer, med suffikset `Property`.
- I vores custom stepper har vi en property kaldet `Value`, så den bindable property, der backer den, skal hedde `ValueProperty`.
- Den statiske `Create`-metode på klassen `BindableProperty` bruges til at oprette bindable properties, som skal oprettes med modifiers `static` og `readonly`.
- Getter og setter på den almindelige `Value`-property skal kalde henholdsvis `GetValue` og `SetValue`. Disse metoder er nedarvet fra baseklassen `BindableObject`.

## 10. Bindable property implementation

### Kodeeksempel

```csharp
public partial class MyStepper : ContentView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(int),
        typeof(MyStepper),
        defaultBindingMode:BindingMode.TwoWay);

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
```

Demo — branch: `customcontrol-v2`.

## 11. References & Links

- .NET MAUI in Action af Matt Goldman
