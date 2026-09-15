# Kapitel 11 — Beyond the basics: Custom controls

## Metadata

- **Kapitel:** 11 — Beyond the basics: Custom controls
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L11
- **Hovedemner:**
  - Templated controls (genbrugelige komponenter) bygget med `ContentView`
  - Code-behind med event handlers til at drive et custom control internt
  - UX-forbedringer i custom controls: input-begrænsning, `Keyboard`-typer, validering
  - Bindable properties: `BindableProperty.Create`, `GetValue`/`SetValue`, navnekonvention
  - Default values og `propertyChanged`-delegate (change handler) på bindable properties
  - Skjul af nedarvede bindable properties med `new`-keyword (`IsEnabledProperty`)
  - `DataTrigger` på custom controls og XML-namespaces i XAML
  - Handler-arkitektur: abstraction → virtual view → handler → native view, `PlatformView`
  - Override af handler mappings: `PrependToMapping`, `ModifyMapping`, `AppendToMapping`
  - Platformspecifik tilpasning med compiler-direktiver (Android, iOS/MacCatalyst, Windows)
  - Deling af controls via .NET MAUI class libraries og NuGet

---

## Introduktion

.NET MAUI leveres med nok indbyggede controls til at bygge stort set enhver UI. Funktionelt er der meget lidt, man ikke kan lave alene med standardkontrollerne, og de er meget fleksible via styles og styling-properties. Men nogle gange skal man et skridt videre.

I .NET MAUI findes der grundlæggende tre veje til at bygge eller tilpasse controls:

1. Bundle eksisterende controls sammen til en genbrugelig komponent (templated control).
2. Tilpasse de platformsspecifikke implementeringer, der følger med i boksen (handlers).
3. Tegne sine egne controls og grafik med `Microsoft.Maui.Graphics`.

> **NOTE** `Microsoft.Maui.Graphics` er et kraftfuldt bibliotek, der kan lave avanceret billedgenerering og -manipulation. At tegne sine egne controls er kun en lille delmængde af, hvad det kan.

Kapitlet behandler de to første tilgange: først bygges et eget control ved at genbruge de indbyggede controls, og derefter vises, hvordan man ændrer den måde, .NET MAUI som standard viser de indbyggede controls på.

---

## 11.1 Using ContentView

Komponentisering er en kernefunktion i alle moderne UI-frameworks. Man *kan* bygge hele sin UI af de elementære controls, men det er mere effektivt at kombinere dem til genbrugelige komponenter.

Tilgangen kendes fra Blazor, Angular, React, Flutter og mange andre. Terminologien varierer, men det er som regel en variation af "reusable components". I .NET MAUI bygges disse genbrugelige komponenter med **`ContentView`**.

Eksemplet er MauiStockTake. På input-siden skal brugeren kunne registrere, hvor mange enheder af et bestemt item vedkommende har talt, og antallet skal vises. Funktionaliteten leveres i dag ved at kombinere to controls.

**Figur 11.1** viser pointen: en `Label` og en `Stepper` bruges til henholdsvis at vise den aktuelle optælling og at lade brugeren indtaste den. Overalt hvor den funktionalitet skal bruges, må man tilføje *begge* controls igen — altså gentagelse.

Der er samtidig et UX-problem: en `Stepper` er upraktisk for store tal. Forestil dig at indtaste 546 med kun en stepper.

**Figur 11.2** viser den klassiske web-løsning: et input-felt med indbygget stepper, hvor brugeren kan taste et tal og bruge op/ned-pile til at øge og sænke værdien. Den løsning fungerer godt i formularer på web og kan fungere på desktop, men den er ikke særlig touch-venlig og er derfor et dårligt valg på mobil.

**Figur 11.3** viser i stedet det templated control, der bygges i kapitlet: to `Button`s og en `Entry`. Knapperne øger og sænker værdien af en bundet property, og `Entry` lader brugeren redigere værdien direkte.

Man kunne bygge disse controls direkte ind i UI'et hver gang. At droppe en `Entry` og et par `Button`s ind på en side er ikke voldsomt arbejde, men et templated control gør UI'et genbrugeligt og sparer en for at løse problemet forfra hver gang. Og jo mere kompleks UI'en er, jo mere værdi ligger der i at gøre den til en genbrugelig komponent.

> **TIP** Hold øje med muligheder for at bundle dele af din UI til genbrug. Don't repeat yourself!

### 11.1.1 Building the custom stepper layout

Der tilføjes en mappe `Controls` i `MauiStockTake.UI`, og heri en **.NET MAUI ContentView (XAML)** ved navn `MildredStepper`. Det kan gøres fra kontekstmenuen i Visual Studio eller via .NET CLI.

Layoutet er et `Grid` med én række og tre kolonner (én til hver `Button` og én til `Entry` i midten). Knap-kolonnerne har bredden 50, `Entry`-kolonnen 120 (så der er plads til større tal). Alt centreres lodret og vandret, og tallet bruger stor skriftstørrelse (42 point) — samme størrelse som den `Label`, der viser optællingen i dag.

Alle tre controls skal have event handlers, så en value-property kan sættes. Til sidst skal `MinimumWidthRequest` for knapperne sættes til 50. `MinimumWidthRequest` er sat til 100 i temaet (fra default-`Style`), så den skal overrides, for at knapperne kan være i deres kolonner.

**Listing 11.1 The custom stepper layout**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.Controls.MildredStepper">
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
Text="+"
Clicked="PlusButton_Clicked"
x:Name="PlusButton"
VerticalOptions="Center"
HorizontalOptions="Center"
MinimumWidthRequest="50"/>
</Grid>
</ContentView>
```

Da både knapper og `Entry` har event handlers, skal de tilføjes i code-behind. Der skal bruges en property til at gemme værdien; når `TextChanged` fyrer, hentes `Text`-værdien fra `Entry`, parses til en `int` og tildeles property'en. Når en af knapperne klikkes, øges eller sænkes værdien, castes til `string` og tildeles `Text`-property'en på `Entry`.

**Listing 11.2 The MildredStepper.xaml.cs file**

```csharp
namespace MauiStockTake.UI.Controls;
public partial class MildredStepper : ContentView
{
public int Value { get; set; }
public MildredStepper()
{
InitializeComponent();
ValueEntry.Text = "0";
}
private void MinusButton_Clicked(object sender, EventArgs e)
{
Value--;
ValueEntry.Text = Value.ToString();
}
private void PlusButton_Clicked(object sender, EventArgs e)
{
Value++;
ValueEntry.Text = Value.ToString();
}
private void ValueEntry_TextChanged(object sender,
 TextChangedEventArgs e)
{
if (int.TryParse(e.NewTextValue, out var value))
{
Value = value;
}
}
}
```

Nu kan det custom stepper-control bruges på input-siden. Når man tilføjer ting til sine XAML-filer, som ikke er en del af standard .NET MAUI-controls og markup extensions, skal der tilføjes et **XML namespace** (præcis som det blev gjort for Behaviors i .NET MAUI Community Toolkit).

Der er i dag afsat to `Grid`-rækker til funktionaliteten (én til `Label` og én til `Stepper`). Med det nye control er der kun brug for én række, så row definitions og rækkeplaceringen af "add count"-knappen skal justeres (før række 4, nu række 3).

**Listing 11.3 The updated InputPage.xaml file**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.Pages.InputPage"
xmlns:controls="clr-namespace:

 MauiStockTake.UI.Controls"
Title="InputPage">
<Grid Padding="20"
RowDefinitions="*, 3*, 4*, 1*">
<SearchBar .../>

<CollectionView Grid.Row="1" ...>
...
</CollectionView>
<ActivityIndicator Grid.Row="1" .../>
<controls:MildredStepper Grid.Row="2"
HorizontalOptions="Center"
VerticalOptions="Center"/>
<Button Grid.Row="3"
...
</Button>
</Grid>
</ContentPage>
```

Annoteringerne i listingen:

1. Tilføjer et XML namespace, der peger på appens `Controls`-namespace med det custom stepper-control.
2. Justerer row definitions. Før var der fem rækker; række 2 og 3 havde `2*` og `2*`, som er slået sammen til én række på `4*`.
3. Tilføjer det custom stepper-control i række 2, refereret via XML-namespacet øverst i filen, centreret lodret og vandret.
4. Flytter `Button` fra række 4 til række 3 (række 4 er ikke længere defineret).

Kører man appen nu, er `Label` og `Stepper` erstattet af det nye control. Plus- og minus-knapperne øger og sænker optællingen, og man kan klikke direkte ind i `Entry` og redigere tallet.

Men: den `Stepper` og `Label`, der er erstattet, havde bindings til ViewModel'en — det har det custom control ikke. Det løses i afsnit 11.2. Først et par UX-forbedringer.

### 11.1.2 Improving the custom stepper's UX

To små ændringer forbedrer UX markant.

**Negative tal.** Det er i dag muligt at indtaste et negativt tal, både via `Entry` og via knapperne. I `MinusButton_Clicked` tilføjes et tjek, der sikrer, at værdien er større end 0, før den sænkes:

```csharp
private void MinusButton_Clicked(object sender, EventArgs e)
{
if (Value > 0)
{
Value--;
ValueEntry.Text = Value.ToString();
}
}
```

Når tallet rammer 0, gør minus-knappen ikke længere noget. Samme princip anvendes i `Entry`-event handleren: før tallet parses, tjekkes om den nye værdi begynder med `"-"`. Gør den det, sættes `Text` tilbage til den nuværende værdi, og handleren afsluttes. Koden tilføjes **inde i event handleren, før den nye værdi parses**:

```csharp
if (e.NewTextValue.StartsWith("-"))
{
ValueEntry.Text = Value.ToString();
return;
}
```

Det forhindrer brugere i ved et uheld at registrere en negativ stock count.

> **Sidebar: Validation**
>
> Normalt validerer man al brugerinput i en UI-app. Det, der er lavet her, er *næsten* validering — men ikke helt, af én grund: der gives ingen feedback til brugeren.
>
> Validering kan tage mange former: sikre at en indtastet værdi er et tal i et bestemt interval, tjekke at den findes på en godkendt liste, verificere at det er en e-mailadresse osv. Ved validering forhindres brugeren ikke i at indtaste et ugyldigt tal — brugeren får i stedet at vide, at indtastningen er ugyldig, og forhindres i at submitte formularen.
>
> Den sidste del er allerede på plads (add count-knappen er disabled), men der gives ingen feedback. .NET MAUI har mange muligheder for validering. Man kunne udvide det eksisterende med en `Label`, der viser en advarsel og kun er synlig, når valideringen fejler.
>
> Yderligere validering kan laves med bindable properties (næste afsnit). Men det bedste sted at starte er .NET MAUI Community Toolkit, som indeholder en håndfuld almindelige valideringer som behaviors, der er nemme at implementere.

**Rigtigt tastatur.** Når man redigerer værdien i det custom stepper, får man det almindelige tastatur. Her ønskes kun tal, så brugeren bør få et numerisk tastatur. Det gøres ved at sætte `Keyboard`-property'en på `Entry` i `MildredStepper.xaml` til `Numeric`. Kører man appen igen, vises et numerisk keypad ved redigering af feltet.

> **Sidebar: Improving input UX in .NET MAUI**
>
> Man kan i mange tilfælde forbedre UX ved at give brugeren det rigtige tastatur. Skal brugeren indtaste tal, brug det numeriske tastatur. E-mail-tastaturet giver hurtig adgang til `@` og almindelige top-level domains. Tastaturtyperne er: `Default`, `Chat`, `Email`, `Numeric`, `Plain`, `Telephone`, `Text` og `Url`.
>
> Input-UX kan forbedres på andre måder. For et password-felt kan man sætte `IsPassword` til `true`, hvorved input maskeres til asterisker, mens brugeren skriver.
>
> .NET MAUI Community Toolkit indeholder en `MaskedBehavior`, som kan sættes på en `Entry`, så input matcher et bestemt mønster. `MaskedBehavior` er særligt nyttig til kreditkortnumre eller specifikke telefonnummerformater.
>
> Bemærk, at dette udelukkende er UX-forbedringer; en bruger kan stadig omgå dem, hvilket er grunden til, at de skal kombineres med de nævnte valideringsteknikker.

Det custom stepper-control er nu næsten færdigt. Det sidste, der mangler, er en måde at få værdien ud af controllet og ind i den omkringliggende side eller layout.

---

## 11.2 Bindable properties

Et kernebegreb i .NET MAUI er **bindable properties**. De er en udvidelse af de klasse-properties, man kender fra .NET (en public class member med getter og setter), men med ekstra funktionalitet, der blandt andet muliggør data binding.

> **NOTE** Bindable properties tilbyder langt mere funktionalitet; læs dokumentationen for at få et indtryk af alt, hvad de kan.

I kapitel 3 blev det slået fast, at data binding sker fra en **source** til en **target**.

**Figur 11.4** viser pointen: en binding går fra et source-objekt (binding context) til et target-objekt. Target-objektet (view'et) skal nedarve `BindableObject`, og target-property'en skal være en bindable property.

Target for en binding skal altså nedarve baseklassen `BindableObject`. Det custom stepper er en `ContentView`, som er en efterkommer af `BindableObject` — den del er i orden. Men target-property'en skal være en bindable property (source for en binding må gerne være en almindelig property, som det er set med ViewModels indtil nu).

Tidligere blev `Value`-property'en på en `Stepper` og `Text`-property'en på en `Label` (targets) bundet til properties i en ViewModel (sources). `Stepper.Value` og `Label.Text` *er* bindable properties. `Value`-property'en i `MildredStepper` er en property, men ikke en bindable property — hvilket betyder, at dette **ikke** ville virke:

```xml
<controls:MildredStepper Value="{Binding Count}">
```

Funktionaliteten er nødvendig, så `Value` skal gøres til en bindable property. Controllets indre virkemåde pakkes ind, og en værdi eksponeres udadtil via en bindable property.

**Figur 11.5** viser pointen: de to `Button`s og den `Entry`, der udgør det custom stepper, manipulerer alle — eller manipuleres af — værdier i code-behind. Men én enkelt property eksponeres eksternt som en bindable property, så man kan hente og sætte den med en binding.

Det custom stepper er et simpelt eksempel, men denne "wrapping" er endnu mere værdifuld i komplekse scenarier.

**Figur 11.6** illustrerer et sådant scenarie: et custom control med input-felter til fornavn, efternavn og fødselsdato. Felterne sætter direkte værdier på properties i code-behind, men der eksponeres én enkelt bindable property, som pakker alle tre properties ind i ét data transfer object (DTO) — fx en `PersonDTO`.

### 11.2.1 Adding the Value property

At tilføje bindable properties til et control i .NET MAUI kræver, at man følger nogle konventioner:

- Den statiske `Create`-metode på klassen `BindableProperty` bruges til at oprette bindable properties.
- De skal oprettes med modifierne `static` og `readonly`.
- Bindable properties backer almindelige properties (i stedet for felter).
- **Navnekonvention:** navnet på en bindable property skal matche navnet på den property, den backer, med suffikset `Property`. Da controllet har en property `Value`, skal den bindable property hedde `ValueProperty`.

`Create`-metoden har tre påkrævede parametre:

- Navnet på den property, den backer
- Return-typen (dvs. typen på den property, den backer)
- Declaring type (dvs. typen på det custom control, som den bindable property tilhører)

**Figur 11.7** opsummerer dette visuelt: den statiske `Create`-metode på typen `BindableProperty` bruges til at oprette instanser af bindable properties. De skal være `public`, `static` og `readonly`. `Create` kræver navnet på property'en, typen på property'en og typen på det control, property'en tilhører. Navnet på den bindable property skal matche property-navnet med `Property` tilføjet.

Der er dog ét skridt mere, før bindingen virker: getter og setter på den almindelige `Value`-property skal kalde henholdsvis `GetValue` og `SetValue`. Disse metoder er nedarvet fra baseklassen `BindableObject`.

- `GetValue` tager én parameter: navnet på den bindable property. Den returnerer `object`, så resultatet skal castes til property'ens type (`int` i tilfældet `Value`).
- `SetValue` tager to parametre: navnet på den bindable property og den værdi, der skal tildeles.

**Listing 11.4 The BindableProperty and updated property**

```csharp
public static readonly BindableProperty ValueProperty = BindableProperty.Create(
nameof(Value),
typeof(int),
typeof(MildredStepper));
public int Value
{
get => (int)GetValue(ValueProperty);

set => SetValue(ValueProperty, value);

}
```

Annoteringer:

1. Bruger `GetValue` i getteren til at hente værdien fra den bindable property. Return-typen er `object`, så den skal castes til property'ens type.
2. I setteren bruges `SetValue` til at tildele den indkomne værdi til den bindable property.

Med den bindable property på plads er det custom stepper klar til brug. På input-siden tilføjes bindingen fra `Value`-property'en på `MildredStepper` til `Count`-property'en i sidens binding context.

**Listing 11.5 The MildredStepper with the binding added**

```xml
<controls:MildredStepper Grid.Row="2"
HorizontalOptions="Center"
VerticalOptions="Center"
Value="{Binding Count}"/>
```

Kører man appen nu, vises det custom stepper på siden. Man kan øge og sænke optællingen med plus- og minus-knapperne og redigere værdien direkte. Man bør kunne registrere en stock count og på reports-siden se, at optællingen er gemt med værdien fra det custom stepper.

**Figur 11.8** viser `InputPage` med det custom `MildredStepper`-control: til venstre er optællingen ændret med plus/minus-knapperne, til højre er værdien redigeret direkte, og det numeriske tastatur gør det nemt for brugeren at indtaste den rigtige datatype.

### 11.2.2 Adding the IsEnabled property

Som figur 11.8 viser, er det custom stepper enabled som standard, og en ændring af optællingen aktiverer efterfølgende add count-knappen. Det betyder, at man kan submitte en optælling uden at have valgt et produkt — og få en fejl.

Før det custom control blev indført, blev `IsEnabled` på standard-`Stepper` styret af en `DataTrigger`, som disablede controllet, når `SelectedProduct` i binding context var `null`, og enablede det, når et produkt var valgt.

> Når man bruger custom controls til at forbedre UX, er det vigtigt ikke at kompromittere eksisterende funktionalitet.

#### Adding the bindable property

`MildredStepper` er af typen `ContentView`, som allerede har en `IsEnabled`-property nedarvet fra baseklassen `VisualElement`. Man kan binde til den fra det indeholdende view og hente/sætte dens værdi, men den understøtter ikke den funktionalitet, der er brug for til data triggeren. Man kunne tilføje en anden property med et andet navn, men det ville bryde med den eksisterende konvention i .NET MAUI-controls.

Løsningen er **`new`-keywordet**: den bindable property tilføjes som enhver anden bindable property, og selvom den allerede findes på en baseklasse, kan `new`-modifieren skjule den nedarvede member. Der findes også allerede en `IsEnabled`-property, som den bindable property backer, så den kan genbruges og refereres i `Create`-metoden uden at skulle tilføjes igen.

**Listing 11.6 The new bindable property for the IsEnabled property**

```csharp
public static new readonly BindableProperty
 IsEnabledProperty = BindableProperty.Create(
nameof(IsEnabled),
typeof(bool),
typeof(MildredStepper));
```

Annoteringer:

1. Bruger `Create` til at oprette en instans af `BindableProperty`, men tilføjer `new`-modifieren ud over `static` og `readonly`.
2. Bruger den eksisterende base-`IsEnabled`-property.
3. Sætter return-typen og declaring type.

Indtil videre er det eneste, der er ændret, tilføjelsen af `new`-keywordet, og funktionelt adskiller denne bindable property sig ikke fra den nedarvede. For at få funktionaliteten til data triggeren tilbage kræves to ændringer:

- **Ændr default-værdien** — default for `bool` er `false`, men data triggeren skal kunne ændre værdien til `true`. Husk, at data triggers ruller deres ændringer tilbage, når betingelsen ikke længere er opfyldt. Hvis triggeren sætter `IsEnabled` til `false`, ville værdien — når `SelectedProduct` ikke længere er `null` — rulle tilbage til default-værdien, som også er `false`. Den skal rulle tilbage til `true`.
- **Tilføj en change handler** — når værdien af `IsEnabled` ændres, skal `Button`s og `Entry` inde i det templated control enables eller disables programmatisk. Med en change handler kan man inspicere den nye og den gamle værdi og reagere derefter.

#### Adding default values

Primitive typer i .NET har alle en default-værdi (fx `false` for `bool`, `0` for `int`). Med en almindelig property kan man overskrive typens default og tildele en default-værdi til instansen, enten ved deklarationen eller i en konstruktør. Med bindable properties skal default-værdien i stedet tildeles i `Create`-metoden — som **fjerde argument**.

**Listing 11.7 The bindable property declaration with a default value provided**

```csharp
public static new readonly BindableProperty IsEnabledProperty =
 BindableProperty.Create(
nameof(IsEnabled),
typeof(bool),
typeof(MildredStepper),
true);
```

Annotering:

1. Sætter default-værdien af `IsEnabled` til `true`. Default for `bool` er `false`, men den skal være `true`, så data triggeren kan sætte den til `false`, baseret på at `SelectedProduct` er `null`.

#### Adding a change handler

`BindableProperty.Create` gør det muligt at angive en delegate, der kaldes, når property'ens værdi ændres — parameteren hedder `propertyChanged`. Indtil nu er der brugt positionelle argumenter i `Create`, men `propertyChanged` er ikke det næste argument i rækkefølgen, så det skal angives som **named argument**.

Delegaten skal være en **static** metode med en bestemt signatur og tre parametre:

- Et `BindableObject`, som er det kaldende templated control — her den instans af `MildredStepper`, som `BindableProperty`-instansen tilhører.
- Et `object`, der repræsenterer den gamle værdi (værdien før ændringen).
- Et `object`, der repræsenterer den nye værdi (værdien efter ændringen).

Parametrene skal dække enhver bindable property på ethvert bindable object, så de skal castes til de specifikke typer, en given property kræver. Det er også en god idé at sikre, at typerne er korrekte — for det bindable object kan man gøre begge dele på én gang (type-check med pattern matching).

At have både gammel og ny værdi betyder, at man kan sammenligne dem og handle derefter. Her er kun den nye værdi interessant: den castes til `bool` og sættes på den tilsvarende property på det bindable object. Der er ikke behov for at tjekke værdien — `IsEnabled` på de enkelte controls (`Entry` og de to `Button`s) sættes simpelthen til den modtagne værdi.

**Listing 11.8 The OnIsEnabledChanged method**

```csharp
private static void OnIsEnabledChanged(BindableObject
 bindable, object oldValue, object newValue)

{
if (bindable is MildredStepper mildredStepper)
{

mildredStepper.IsEnabled = (bool)newValue;

mildredStepper.ValueEntry.IsEnabled =
 mildredStepper.IsEnabled;
mildredStepper.PlusButton.IsEnabled =

 mildredStepper.IsEnabled;
mildredStepper.MinusButton.IsEnabled =

 mildredStepper.IsEnabled;
}
}
```

Annoteringer:

1. Tilføjer `OnIsEnabledChanged` og gør den `static`. Den skal tage en `BindableObject`-parameter, en `object`-parameter til den gamle værdi og en `object`-parameter til den nye værdi.
2. Tjekker, at det bindable object, metoden har modtaget, er den rigtige type, og caster det til en variabel.
3. Caster den nye værdi til den rigtige type (her `bool`) og tildeler den til den tilsvarende property på det bindable object (her `IsEnabled`).
4. Sætter `IsEnabled` på child-controls til `IsEnabled` på det bindable object, hvilket disabler eller enabler dem efter behov.

Til sidst tildeles metoden til `IsEnabledProperty` i `Create`-metoden via et named argument.

**Listing 11.9 The IsEnabledProperty declaration with the propertyChanged delegate**

```csharp
Public static new readonly BindableProperty IsEnabledProperty =
 BindableProperty.Create(
nameof(IsEnabled),
typeof(bool),
typeof(MildredStepper),
true,
propertyChanged: OnIsEnabledChanged);
```

Annotering:

1. Tildeler `OnIsEnabledChanged` til `propertyChanged`-parameteren via et named argument.

Dermed er `IsEnabled`-bindable-property'en og `MildredStepper`-controllet færdige. Sidste skridt er at tilføje data triggeren tilbage på input-siden.

#### Adding the DataTrigger

Processen for at tilføje data triggeren til det custom stepper er den samme som for standard-`Stepper`: controllets `Triggers`-collection defineres, og der tilføjes en `DataTrigger` med `TargetType`, `Binding` og `Value`. Derefter tilføjes en `Setter` med `Property` og `Value`.

Forskellen er, at controllet ikke ligger i standard-XAML-namespacet, så XML-namespacet skal med, både når `Triggers`-collection defineres, og i `TargetType`. Binding, value og setter er identiske med dem, der blev brugt til standard-`Stepper`.

**Listing 11.10 MildredStepper with the DataTrigger added**

```xml
<controls:MildredStepper Grid.Row="2"
HorizontalOptions="Center"
VerticalOptions="Center"
Value="{Binding Count, Mode=TwoWay}">

<controls:MildredStepper.Triggers>
<DataTrigger TargetType="controls:MildredStepper"

Binding="{Binding SelectedProduct, TargetNullValue=''}"
Value="">
<Setter Property="IsEnabled"
Value="False" />
</DataTrigger>
</controls:MildredStepper.Triggers>
</controls:MildredStepper>
```

Annotering:

1. Inkluderer namespacet, når controllet refereres i definitionen af triggers og target type.

**Figur 11.9** viser resultatet: det custom stepper disables af en `DataTrigger`, når `SelectedProduct` i binding context er `null`. Søger man efter og vælger et produkt, enables stepperen, og man kan sætte en optælling og submitte den.

> **Sidebar: An easier way to create bindable properties**
>
> I tidligere kapitler blev det vist, hvordan interfacet `INotifyPropertyChanged` bruges til at fortælle UI'et, at properties i dets binding context har ændret sig. Processen er mere kompleks end i visse andre UI-frameworks, men den kan forenkles med source generators i MVVM Community Toolkit: i stedet for at skrive både property, felt og `PropertyChanged`-invokation i setteren, deklarerer man blot feltet og dekorerer det med en attribut.
>
> At oprette bindable properties er betydeligt mere omstændeligt, og .NET MAUI Community Toolkit har desværre ikke en tilsvarende source generator til bindable properties (der er dog på skrivetidspunktet et åbent forslag og en spec). Der findes til gengæld en pakke, der gør præcis dette: `https://github.com/rrmanzano/maui-bindableproperty-generator`.
>
> Anbefalingen er at fortsætte med at skrive bindable properties manuelt, indtil man kan gøre det uden at slå op i bogen eller dokumentationen — for at sikre en grundig forståelse af, hvordan de virker. Når man kan det, kan pakken være en betydelig tidsbesparelse.

---

## 11.3 Modifying platform controls with handlers

Det nye custom stepper giver en betydelig UX-forbedring, især ved indtastning af store tal. Efter en testrunde foretrækker Mildreds personale funktionaliteten frem for standard-`Stepper`, men UI'et er upopulært. Mildreds designere beder om, at `Entry` i midten af stepperen gøres mindre iøjnefaldende.

**Figur 11.10** sammenligner, hvordan det custom stepper ser ud på tværs af de understøttede platforme: det er ens på hver platform, men med variationer. På alle platforme er `Entry` prominent, hvilket gør den nem at opdage, men grim — særligt når kun knapperne bruges. Hver platform har sin egen tilgang til at rendere en `Entry` og dermed til, hvordan stepperen renderes.

I forrige kapitel blev det vist, hvordan styles og control-properties kan ændre udseendet af out-of-the-box-controls — men der er **ingen property i .NET MAUI, der styrer borderen på en `Entry`**. I den slags situationer kan man override den måde, .NET MAUI-controlabstraktionen implementeres på målplatformene, og det gøres ved at tilpasse controllets **handler**.

### 11.3.1 Handler architecture

De cross-platform controls, der bruges i .NET MAUI-apps, repræsenteres af **abstraktioner** — hver er i bund og grund en konceptuel definition af en UI-control. **Virtual views** implementerer disse abstraktioner som de controls, vi bruger i .NET MAUI-apps, og **handlers** mapper abstraktionerne til **native views**, dvs. deres specifikke implementeringer på hver platform. Hver handler har en property ved navn **`PlatformView`**, der repræsenterer den native control.

Handlers er limen, der binder de cross-platform controls sammen med de platformsspecifikke implementeringer, og de er den kerneteknologi, der gør det muligt at skrive cross-platform apps i .NET MAUI.

**Figur 11.11** viser arkitekturen: cross-platform controls beskrives af interfaces, virtual views implementerer dem som controls i .NET MAUI's UI-lag, og handlers mapper abstraktionerne til platformsspecifikke implementeringer.

Handlers definerer mappings i en **dictionary**, der beskriver, hvordan de cross-platform properties anvendes på hver platform. Eksempel: en `Button` i .NET MAUI har flere properties, man kan ændre, herunder `BackgroundColor`. En handler mapper .NET MAUI's `BackgroundColor`-property, som er af typen `Microsoft.Maui.Graphics.Color`, til den platformsspecifikke property, som på iOS og macOS fx er af typen `UIKit.UIColor`.

> **NOTE** Der findes én handler pr. control pr. platform. Fx er der én `Button`-handler til Android, én til iOS, én til Windows og én til Mac Catalyst.

Hver platform implementerer UI-controls forskelligt, men når man bygger .NET MAUI-apps, er man normalt ikke optaget af de platformsspecifikke implementeringsdetaljer. Nogle gange har man dog brug for finere kontrol over, hvordan UI-elementer vises, og så skal de overrides.

Man kan også skrive sine **egne** handlers for at lave sine egne cross-platform controls og få adgang til platformsspecifikke controls, der ikke er eksponeret i .NET MAUI. For det custom stepper er det kun nødvendigt at ændre eksisterende mappings.

### 11.3.2 Overriding handler mappings

Hver handler har en **mapper**, og hver mapper stiller tre metoder til rådighed til at override mappings:

- **`PrependToMapping`** — ændringerne anvendes *før* handlerens default-mappings. Godt valg, hvis man tilføjer mappings, der ikke allerede er defineret.
- **`ModifyMapping`** — ændrer eksisterende mappings defineret i handleren.
- **`AppendToMapping`** — ændringerne anvendes *efter* default-mappings. Det betyder, at ændringerne her har forrang over defaults.

> **Sidebar: When do I use each method?**
>
> `PrependToMapping` er nyttig, hvis du vil mappe properties, der ikke allerede mappes af default-mappings, men du ikke vil have, at dine mappings overrider noget i defaults.
>
> `ModifyMapping` er nyttig, hvis du har en dyb forståelse af den eksisterende mapping-dictionary og vil ændre den måde, default-mappings er defineret på.
>
> `AppendToMapping` giver den største sikkerhed for, at dine tilpasninger bliver anvendt, og er den metode, du bør bruge i næsten alle tilfælde. Det er usandsynligt, at du får brug for en af de andre metoder i de fleste tilfælde.

Alle tre metoder har de samme to parametre:

1. **En nøgle (key).** Mappings defineres i dictionaries. Bruger man `ModifyMapping`, skal man bruge nøglen for den eksisterende default-mapping (i .NET MAUI er disse defineret som navnet på property'en på det relevante interface). For de to andre metoder kan man bruge en vilkårlig string-nøgle.
2. **En `Action`,** som får to argumenter overleveret: den handler, mapperen tilhører (dvs. den platformsspecifikke instans af handleren), og det view, handleren svarer til.

**Figur 11.12** viser et eksempel på at tilføje en handler-mapping til et view: koden åbner et lambda-udtryk til at appende mappings til handleren for cross-platform-controllet `Entry`. Mappings for dette control til platformsspecifikke controls ligger i `EntryHandler` i namespacet `Microsoft.Maui.Handlers`. Der bruges `AppendToMapping`-metoden, som ligger på handlerens `Mapper`-property. Nøglen for denne mapping-profil er `RemoveBorder` — havde man i stedet brugt `ModifyMapping`, skulle man have brugt en eksisterende nøgle i mapping-dictionary'en. `Action`'en får to argumenter: handler-instansen og det view, der tilpasses.

**Hvor skal handler-mappings ligge?** Der er flere muligheder:

- I `MauiProgram` som del af appens startup-logik.
- I de relevante platform-mapper.
- Sammen med det control, man ændrer (den tilgang, kapitlet bruger).

Vigtigt at huske: når handler-modifikationerne først er eksekveret, gælder de for **alle instanser af controllet i hele appen**, og hvor man placerer logikken, afgør *hvornår* den eksekveres. Lægger man den i `MauiProgram`, eksekveres den, før nogen views renderes, og ændringerne gælder for alle instanser fra appens start. Lægger man den andetsteds, eksekveres den, når den kodesti nås — hvorefter alle instanser af det control, handleren står for, ændres.

Vil man ikke anvende en modifikation på alle instanser af et control, kan man **subclasse** det og i sin handler-logik tjekke, om det påvirkede view er en instans af baseklassen eller af ens subclass. Det er den tilgang, kapitlet tager.

Forfatterens foretrukne tilgang ved et subclasset control er at holde handler-mappings i **konstruktøren**. Det sikrer, at handler-logikken eksekveres, hver gang en instans af controllet renderes, og holder al rendering-logik for det custom control ét sted. Skulle alle instanser af et control ændres, ville handler-mappings ligge i `MauiProgram`; skulle et control kun ændres på én platform, ville handler-logikken ligge i den pågældende platform-mappe.

### 11.3.3 Implementing custom handler logic

For det custom stepper skal `Entry`-controllet ændres, men mapping-dictionary'en i `EntryHandler` gælder alle instanser af `Entry`. Anvendes mappings direkte, rammer de bredt — hvilket ikke er ønsket. I stedet subclasses `Entry`-typen, og i `AppendToMapping` tjekkes, at ændringen kun anvendes på den ønskede type.

Der oprettes en klasse `BorderlessEntry` i `Controls`-mappen i `MauiStockTake.UI`, som subclasser `Entry`. Der tilføjes en `private void`-metode `ModifyEntry`, som kaldes fra konstruktøren. Inde i `ModifyEntry` kaldes `AppendToMapping` på `EntryHandler`s `Mapper` med nøglen `RemoveBorder`.

**Listing 11.11 The BorderlessEntry class**

```csharp
namespace MauiStockTake.UI.Controls;
public class BorderlessEntry : Entry
{
public BorderlessEntry()
{
ModifyEntry();

}
private void ModifyEntry()
{
Microsoft.Maui.Handlers.EntryHandler.Mapper.
AppendToMapping("RemoveBorder", (handler, view) =>
{
});
}
}
```

Annoteringer:

1. Subclasser `Entry`-controllet, så vi har vores egen version at tilpasse.
2. Kalder `ModifyEntry` fra konstruktøren.
3. Kalder `AppendToMapping` på `Mapper`-property'en af `EntryHandler` og giver mappingen nøglen `RemoveBorder`; navngiver handler- og view-argumenterne, der sendes til lambda-udtrykket.

På dette tidspunkt kunne man begynde at tilføje sine tilpasninger. Men da de anvendes på `EntryHandler`, vil de — så snart en instans af `BorderlessEntry` konstrueres — gælde alt, hvad `EntryHandler` står for, altså **alle** instanser af `Entry`.

Derfor tilføjes et conditional check inde i mapping-logikken.

**Listing 11.12 The conditional check in the mapping code**

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveBorder",
 (handler, view) =>
{
if (view is BorderlessEntry)
{
}

});
```

Annotering:

1. Tilføjer et tjek, der pakker al vores tilpasningslogik ind i en conditional, så tilpasningerne kun anvendes på vores custom subclassede control og ikke alle instanser af `Entry`.

Det sidste, der skal på plads, før tilpasningerne kan skrives, er **compiler-direktiver**, så logikken for hver handler-instans (dvs. hver platformsspecifik handler) kan holdes adskilt. Der findes andre måder — fx partial classes med modifikationerne i platform-mapperne — men denne tilgang holder al logik for hvert custom control ét sted.

**Listing 11.13 The compiler directives to isolate platform logic**

```csharp
#if ANDROID

#elif WINDOWS

#elif IOS || MACCATALYST

#endif
```

Annoteringer:

1. Kode i denne del af `#if`-blokken kompileres kun til Android og indgår ikke i builds til andre platforme.
2. Kode i denne del kompileres kun til Windows.
3. Kode i denne del kompileres kun til iOS og Mac Catalyst. Mac Catalyst og iOS bruger samme handler-logik.

#### Android

Lambda-udtrykket får to argumenter: det view, der tilpasses, og den platformsspecifikke handler-instans. Handler-instansen har property'en `PlatformView`, som giver adgang til den native control, handleren mapper til. På Android er det en **`AppCompatEditText`**-widget.

For at fjerne borders og understregningen på Android skal `Background` på `AppCompatEditText` sættes til `null`, og `SetBackgroundColor` kaldes med `Android.Graphics.Color.Transparent`. Det fuldt kvalificerede navn på farven bruges, fordi der allerede er compiler-direktiver inde i mapperen; begynder man også at tilføje dem til `using`-statements, bliver koden rodet.

**Listing 11.14 The Android-specific customizations**

```csharp
handler.PlatformView.Background = null;
handler.PlatformView.SetBackgroundColor(Android.

Graphics.Color.Transparent);
```

Annoteringer:

1. `PlatformView` er her `AppCompatEditText`, som har en `Background`-property, der skal sættes til `null`.
2. `AppCompatEditText` har en `SetBackgroundColor`, der tager en `Android.Graphics.Color` som argument. Her sættes den til `Transparent` med fuldt kvalificeret navn, så Android-specifikke `using`-statements ikke trækkes ind i delt kode.

> **Sidebar: How do I know what changes to make to platform-specific mappings?**
>
> At arbejde med handlers er en leg, især sammenlignet med den renderer-arkitektur, det erstatter i Xamarin.Forms. Vanskeligheden ligger i at vide, *hvilke* native controls der mappes til, og *hvilke* properties på dem der skal ændres.
>
> Man opbygger færdigheden, efterhånden som man udvikler sig som .NET MAUI-udvikler, særligt i takt med at man lærer målplatformene bedre at kende. Med tiden får man måske sit eget kuraterede bibliotek af platform-tilpasninger. Der er dog et par metoder til at finde ud af, hvad der skal ændres:
>
> - **IntelliSense.** I en IDE som Visual Studio viser IntelliSense de tilgængelige properties og metoder. Nogle gange er det indlysende, hvilke værdier de tager; ellers kan man slå dem op i fx Android-dokumentationen. Man kan også holde musen over `PlatformView` eller dens properties i editoren for at se, hvilken native control der er i brug, og så slå dens properties op eller finde guides til den specifikke tilpasning. Derefter skal det blot oversættes til en mapping i handleren — hvilket normalt er lettere end første del.
> - **Community-materiale.** De fleste modifikationer, man får brug for, er allerede veldokumenteret af communityet. Selv hvis man ikke finder noget .NET MAUI-specifikt, finder man det næsten helt sikkert til Xamarin.Forms. Xamarin.Forms bruger en anden arkitektur, men at oversætte renderers til handlers er som regel en simpel opgave. Der findes faktisk et eksempel på netop denne `BorderlessEntry` oversat fra en Xamarin.Forms-renderer til en .NET MAUI-handler, linket i appendiks B.
>
> .NET MAUI-communityet er en af de bedste ting ved .NET MAUI-udvikling — aktivt og fuld af folk, der deler viden. Når man skal tilpasse en handler, finder ens foretrukne søgemaskine næsten altid et relevant blogindlæg, en video eller en diskussion. Og ellers kan man altid række ud til communityet.
>
> Med tiden begynder man selv at regne tingene ud med en kombination af IntelliSense, platform-dokumentation og voksende erfaring. Når det sker, så overvej at dokumentere sine fund — communityet vil takke, og man takker måske også sig selv, når man vender tilbage til det senere.

Koden fjerner `Entry`-chromet, hvilket ser godt ud, når stepper-knapperne bruges. Men så mangler de sædvanlige UX-cues, en bruger forventer, når tekstfelter redigeres. Før de øvrige platforme håndteres, tilføjes derfor en `Border` omkring `Entry` i det custom stepper. `StrokeThickness` sættes til 0, så den normalt ikke er synlig, og en data trigger viser den, når `Entry` har fokus (dvs. når brugeren tapper eller klikker i den). Markup extension'en **`OnPlatform`** bruges, så den kun vises på Android.

**Listing 11.15 The conditional border to add to MildredStepper.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView ...>
<Grid ...>
...
<Border Grid.Column="1"
Stroke="{StaticResource Primary}"
BackgroundColor="Transparent"
Margin="10"
StrokeThickness="0"
StrokeShape="RoundRectangle 5">
<Border.Triggers>
<DataTrigger TargetType="Border"
Binding="{Binding Source={x:Reference
 ValueEntry}, Path=IsFocused}"
Value="True">
<Setter Property="StrokeThickness"
Value="{OnPlatform Android=1}"/>
</DataTrigger>
</Border.Triggers>
<Entry x:Name="ValueEntry" .../>
</Border>
...
</Grid>
</ContentView>
```

Dermed er Android-delen færdig.

#### macOS and iOS

I kapitel 3 blev det slået fast, at .NET MAUI-apps kører på macOS via **Catalyst**, som kører iOS-apps på macOS og samtidig giver adgang til macOS-features, når det er nødvendigt. Derfor er tilpasningerne til iOS og macOS de samme, og de kan ligge i samme conditional compiler-blok.

På iOS og macOS er `PlatformView` en `UITextView`. Dens `BackgroundColor` sættes til `UIColor.Clear` i `UIKit`-namespacet og dens `BorderStyle` til `UITextBorderStyle.None`, også i `UIKit`. Derudover skal `BorderWidth` fra baseklassen `UIView`, som `UITextView` nedarver, sættes til 0.

**Listing 11.16 The macOS and iOS customizations**

```csharp
handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
handler.PlatformView.Layer.BorderWidth = 0;
handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
```

#### Windows

`PlatformView` på Windows er en `TextBox` i namespacet `Microsoft.UI.Xaml.Controls`. Den har en `Background`-property, der skal sættes til `null`, samt `BorderThickness` og `FocusVisualMargin`, som skal sættes til en ny instans af `Microsoft.UI.Xaml.Thickness`. Konstruktøren tager en `int`, der definerer tykkelsen — her sendes 0 ind.

**Listing 11.17 The Windows customizations**

```csharp
handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
handler.PlatformView.Background = null;
handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
```

`BorderlessEntry`-klassen er nu færdig, og selvom den fjerner borderen fra `Entry` på Windows, vises fokus-understregningen stadig. Den kan **ikke** fjernes via en handler; i stedet skal resource dictionary'en i `App.xaml` i Windows-platform-mappen ændres. Windows' `App.xaml` har en rodnode `maui:MauiWinUIApplication`; koden tilføjes mellem disse tags.

**Listing 11.18 The code to add to App.xaml in the Windows platform folder**

```xml
<maui:MauiWinUIApplication.Resources>
<Thickness x:Key="TextControlBorderThemeThickness">0</Thickness>
<Thickness x:Key="TextControlBorderThemeThicknessFocused">0</Thickness>
</maui:MauiWinUIApplication.Resources>
```

Disse properties eksponeres ikke via `PlatformView`, så det er det eneste sted, ændringen kan laves. Det introducerer dog et nyt problem: da ændringen ikke anvendes på en specifik subclass, bliver **enhver** `Entry` nu helt borderless på Windows og mister fokus-understregningen. `Entry` bruges ganske vist kun som del af andre controls i MauiStockTake, men på Windows bruger `SearchBar` (øverst på input-siden) `TextBox`-controllet under motorhjelmen — og bliver derfor påvirket.

Problemet kunne løses ved at subclasse WinUI's `TextBox`-control i Windows-platform-mappen, anvende den Windows-specifikke style-ændring på subclassen frem for base-`TextBox`, og til sidst oprette en helt ny mapping (i stedet for at ændre den eksisterende), der mapper den subclassede `BorderlessEntry` til den subclassede `TextBox`-derivat.

Men da `Entry` ikke bruges andre steder, er det ikke nødvendigt for MauiStockTake. En lettere tilgang er at reparere `SearchBar`: der tilføjes en `Border` omkring den, og `OnPlatform` bruges til at give borderen en `StrokeThickness` på 0 på alle andre platforme.

> **Sidebar: Making tradeoffs**
>
> I MauiStockTake er der ingen eksplicitte instanser af `Entry`-controllet, så det er ikke et stort problem at ofre borders og understregning. Kompromiset kan accepteres for at opnå den ønskede effekt på et control, vi rent faktisk bruger.
>
> Efterhånden som appen vokser, vil `Entry` sandsynligvis blive brugt andre steder. Når det sker, står man med et valg: enten rulle ændringen tilbage på Windows og acceptere, at det custom stepper ikke matcher designet 100 % (men måske er tæt nok på) — mod at bruge `Entry` som den kommer ud af boksen; eller lave et custom entry-control med fx egen border eller understregnings-fokuseffekt, som så bruges overalt i appen i stedet for standard-controllet.
>
> I større apps er sidstnævnte tilgang formentlig det, man ender med alligevel. Mange virksomheder har deres egen branding og style guides og vil have, at alle deres controls, på tværs af alle apps og platforme, ser ud på en bestemt måde. I .NET MAUI-apps laver man måske controls netop for at opnå dette og deler dem via control libraries.

**Listing 11.19 The Border workaround for SearchBar in InputPage.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage ...>
<Grid ..>
<Border StrokeShape="RoundRectangle 5"

Stroke="{StaticResource Primary}"
BackgroundColor="Transparent"
StrokeThickness="{OnPlatform WinUI=1, Default=0}">
<SearchBar .../>
</Border>
...
</Grid>
</ContentPage>
```

Dermed er det borderless entry-control færdigt; det skal blot droppes ind i det custom stepper.

### 11.3.4 Updating the custom stepper

Det custom stepper har en `Entry` i midterkolonnen af sit `Grid`. Fordi `BorderlessEntry` er en subclass af `Entry`, kan den droppes direkte ind som erstatning (jf. **Liskov Substitution Principle**, nævnt i sidebaren "MVVM for SOLID apps" i kapitel 9). Det eneste, der skal gøres først, er at tilføje et XML-namespace i XAML'en, så det custom control kan refereres.

**Listing 11.20 MildredStepper.xaml updated to use the new BorderlessEntry**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns:controls="clr-namespace:
MauiStockTake.UI.Controls" ...>
<Grid ...>
...

<controls:BorderlessEntry

x:Name="ValueEntry"
Keyboard="Numeric"
FontSize="42"
HorizontalTextAlignment="Center"

TextChanged="ValueEntry_TextChanged"
VerticalOptions="Center"
HorizontalOptions="Center"/>
...
</Grid>
</ContentView>
```

Annoteringer:

1. Tilføjer et XML-namespace for `Controls`-namespacet i `MauiStockTake.UI`-projektet.
2. Erstatter standard-`Entry`-controllet med `BorderlessEntry`-controllet i `controls`-XML-namespacet.

**Figur 11.13** viser resultatet: det custom stepper med `BorderlessEntry`, her på Android. Til venstre er tallet ændret med stepper-knapperne; til højre redigeres tallet direkte, og en border vises.

På Windows er der en hover-effekt (kaldet `PointerOver` i `VisualStateManager`, jf. kapitel 10) og en fokus-effekt. De kunne også fjernes, men de bidrager til controllets discoverability, så de beholdes (og deres fravær på de andre platforme begrædes).

---

## 11.4 Creating and sharing control libraries

Man kan lave custom controls med templated controls, ved at tegne controls med `Microsoft.Maui.Graphics`-biblioteket, ved at tilpasse native controls med handler mappings — eller ved kombinationer af det hele. Ofte bruges disse custom controls til at leve op til en virksomheds branding, style guide eller ligefrem dens design language system. Når det er tilfældet, er det nyttigt at kunne genbruge controls på tværs af flere apps — og selv når det ikke er tilfældet, udvikler man nogle gange et control (eller et sæt) som er værd at genbruge.

I kapitel 8 blev deling af kode inden for en solution behandlet, men også deling af kode på tværs af en virksomhed blev nævnt. Bygger man .NET MAUI-apps, giver det mening at dele genbrugelige controls i **control libraries**. Hvis Mildred's Surf Shack ville introducere flere apps, ville det give mening at flytte det custom stepper ud i et control library, der kan deles med de andre apps.

> **Sidebar: Naming controls**
>
> Det er almindeligt at se controls med navne som `CustomStepper`. Det navn er ikke særlig beskrivende, og det er bedre at bruge mere meningsfulde navne. Fx er det nemt at se alene ud fra navnet, hvad `BorderlessEntry` er, og hvad den gør.
>
> Det er også almindeligt, at udviklere opkalder controls efter sig selv eller deres virksomhed — fx `GoldieEntry` eller `SSWButton`. Det er fint, når man bygger og kuraterer (eller deler) sit eget control library, men bygger man apps for eller på vegne af en anden virksomhed, er det bedre at navngive controls på en måde, der er meningsfuld for dem — som i eksemplet `MildredStepper`.
>
> `MildredStepper` fortæller måske ikke noget om de specifikke tilpasninger, men det fortæller, at det er en variation af et `Stepper`-control, som passer til Mildreds brand. Det navn er bedre end `CustomStepper`, fordi det er mere specifikt og derfor mindre tilbøjeligt til at kollidere med controls i andre libraries. Det er også bedre end et app-specifikt navn som `StockTakeStepper` — særligt hvis det flyttes til et control library og bruges i mere end én app hos Mildred's Surf Shack.

**Figur 11.14** viser komponenterne i enterprise-app-økosystemet hos Mildred's Surf Shack (en opdateret version af code-sharing-diagrammet fra kapitel 8). Boksen "**.NET MAUI Control Library**" er fremhævet: det ville give mening at flytte det custom stepper (og eventuelle andre custom controls) derind, så det kan bruges både af stock-taking-appen og af andre apps i virksomheden.

Deling af controls med .NET MAUI er ligetil. Skulle `MildredStepper` deles med andre apps, ville første skridt være at oprette et **.NET MAUI class library**. Der findes en .NET-template til det, som kan bruges fra Visual Studio eller .NET CLI — den svarer til et almindeligt class library, men med alle .NET MAUI-dependencies allerede sat op.

**Figur 11.15** viser new project-dialogen for .NET MAUI class library-templaten: et class library med alle .NET MAUI-dependencies wired up, inklusive platform-mapperne, hvor man kan lægge platformsspecifik kode.

Man ville derefter flytte (eller genskabe) `MildredStepper`-controllet i dette library og tilføje en dependency på det i MauiStockTake-appen. Namespacet skulle opdateres, men ellers ville der ikke være nogen forskel.

.NET gør det også nemt at dele class libraries på tværs af en virksomhed. Hele biblioteket kan pakkes med **NuGet**, hvilket kan konfigureres som del af buildet.

**Figur 11.16** viser pointen: at oprette en NuGet-pakke ud fra et class library — også et .NET MAUI class library — kan konfigureres som del af build-processen.

Når projektet er bygget, kan man kopiere det til en delt mappe, som kan tilføjes som en NuGet-kilde. En bedre tilgang er dog at få NuGet-pakken oprettet og distribueret som del af sine CI/CD-pipelines. Er det et control library, man deler offentligt, er det bedste sted `www.nuget.org`. I andre scenarier kan GitHub hoste både private og offentlige NuGet-feeds, hvilket også gælder Azure DevOps og flere andre formålsbyggede NuGet-løsninger.

---

## Summary

- Templated controls i .NET MAUI lader dig "komponentisere" views.
- Templated controls oprettes med `ContentView`-templaten.
- Custom controls bør altid forbedre UX. Fjerner eller overrider du funktionalitet, skal du enten tilføje den igen eller erstatte den med noget bedre.
- Validering — og især meningsfuld feedback om fejlslagne input — forbedrer din apps UX markant.
- Templated controls bruger bindable properties til at muliggøre data binding mellem det indeholdende view og deres interne data.
- Bindable properties oprettes med den statiske metode `BindableProperty.Create`. De properties, de backer, skal bruge `GetValue` og `SetValue` i deres getter og setter til at hente og sætte værdier på den tilsvarende bindable property.
- Bindable properties skal følge navnekonventionen `[property den backer]Property`; fx backer `IsEnabledProperty` en klasse-property ved navn `IsEnabled`.
- .NET MAUI mapper cross-platform controls til native controls via handlers. Handlers indeholder dictionaries af property mappings, der mapper properties på cross-platform controls til de native platform-controls.
- .NET MAUI class libraries er en fremragende måde at dele controls mellem apps i en virksomhed eller med communityet.
