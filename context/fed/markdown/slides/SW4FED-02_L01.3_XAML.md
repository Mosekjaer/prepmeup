# L01.3 – XAML (eXtensible Application Markup Language)

## Metadata

- **Lektion:** L01 – XAML
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L01/FED XAML.pdf (10 slides)
- **Emner dækket:**
  - Hvad XAML er, og forholdet til MAUI
  - XML namespaces i XAML og hvorfor de er nødvendige
  - De to standard-namespaces (MAUI og `x`)
  - `clr-namespace:` — namespace mapping URI-syntaks
  - `x:Class` og generering af klasser
  - Properties som ukvalificerede attributter
  - `ContentPropertyAttribute` og child content
  - Håndtering af flere children (collection content property)

---

## 1. Hvad er XAML?

XAML er et XML-baseret sprog til at skabe træer af .NET-objekter.

Det bruges til at bygge user interfaces i MAUI — og i WPF og andre Microsoft-frameworks.

Selvom XAML er stærkt associeret med MAUI, er de to ting adskilte. Man behøver ikke bruge XAML for at skrive en MAUI-applikation, og det er muligt at bruge XAML til andre teknologier.

## 2. Namespaces

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AlohaWorld.MainPage"
             >
```

XAML er afhængig af XML namespaces for at bestemme betydningen af elementer.

Mange klassenavne er tvetydige. Der findes fx fire forskellige klasser ved navn `Control` i .NET class library. .NET Framework har et namespace-system, der bruges til disambiguering, og standard-XML har også et namespace-system til samme formål. XAML bruger XML namespaces til at repræsentere .NET namespaces.

Men der er ikke en en-til-en-korrespondance mellem XML namespaces i XAML og .NET namespaces.

## 3. Standard XAML namespaces

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AlohaWorld.MainPage"
             >
```

Det **første** namespace angiver typer, der er en del af MAUI-frameworket. Dette ene XML namespace omfatter flere .NET namespaces. Der er intet kolon efter `xmlns`-tagget, hvilket gør dette til default namespace for elementet — altså alt mellem `<ContentPage>` og `</ContentPage>`.

Det **andet** namespace repræsenterer forskellige XAML utility features, som ikke er specifikke for MAUI — fx muligheden for at repræsentere type-objekter eller en null-reference. Det er et specielt namespace, i den forstand at ikke alt i det svarer til en type. Det andet namespace er associeret med prefixet `x`.

På linje 3 bruges `Class`-tagget fra `x`-namespacet til at associere XAML-markup-filen med en C#-klasse.

## 4. Namespace mapping URI-syntaks

XAML understøtter også en måde at referere til typer i namespaces, hvor `XmlnsDefinitionAttribute` ikke er brugt:

```xml
<Grid xmlns:local="clr-namespace:MyProject"
      xmlns:mylib="clr-namespace:MyLibraryNS;assembly=MyLibrary">

  <!-- MyProject.MyLocalType in local assembly -->
  <local:MyLocalType />

  <!-- MyLibraryNamespace.MyLibraryType in MyLibrary assembly -->
  <mylib:MyLibraryType />
</Grid>
```

Hvis en XML namespace-URI begynder med `clr-namespace:`, bliver den ikke behandlet som en simpel opaque identifier, sådan som namespace-URI'er normalt bliver. XAML-compileren parser i stedet URI'en for at udtrække .NET-namespacet og eventuelt et assembly-navn.

## 5. Generering af klasser

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AlohaWorld.MainPage"
             >
```

I `x:Class="AlohaWorld.MainPage"` er `AlohaWorld` namespacet og `MainPage` klassen.

`x:Class`-attributten er et signal til XAML-compileren om, at den skal generere en klassedefinition baseret på denne XAML-fil. Attributten bestemmer navnet på den genererede klasse, og klassen vil nedarve fra typen af root-elementet.

## 6. Properties

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AlohaWorld.MainPage"
             BackgroundColor="AliceBlue"
             >
```

`BackgroundColor`-attributten har ingen namespace-kvalifikator. I XAML svarer ukvalificerede attributter som regel til properties på det .NET-objekt, elementet refererer til.

Attributten betyder, at når en instans af den genererede klasse `AlohaWorld.MainPage` konstrueres, skal den sætte sin egen `BackgroundColor` property til `"AliceBlue"`. Det svarer til følgende kode:

```csharp
this.BackgroundColor = "AliceBlue";
```

## 7. Children og ContentProperty

```csharp
[Microsoft.Maui.Controls.ContentProperty("Content")]
public class ContentPage : Microsoft.Maui.Controls.TemplatedPage
```

```xml
<ContentPage >
    <ScrollView >
    </ScrollView>
</ContentPage>
```

Når man forsøger at angive nested content, kræver XAML-compileren, at parent-typen (her `ContentPage`) eller dens baseklasse er annoteret med `ContentPropertyAttribute`. Attributten fortæller XAML-compileren navnet på den property, der skal indeholde child content.

I dette eksempel vil compileren sørge for, at der oprettes et `ScrollView`-objekt, som tildeles `Content`-propertyen på `ContentPage`:

```csharp
ScrollView sv = new ScrollView ();
MainPage.Content = sv;
```

## 8. Håndtering af flere children

Elementer, der kan indeholde flere children — fx layouts — udpeger simpelthen en property med en collection-type som content property:

```csharp
[Microsoft.Maui.Controls.ContentProperty("Children")]
public class VerticalStackLayout : Microsoft.Maui.Controls.StackBase
```

```xml
<VerticalStackLayout …>
    <Image … />
    <Label … />
    <Label … />
    <Button … />
</VerticalStackLayout>
```

`ContentProperty("Children")` fortæller os, hvordan XAML-compileren tilføjer børnene inde i `VerticalStackLayout`:

```csharp
VerticalStackLayout v = new VerticalStackLayout();
Image i = new Image(); Label l = new Label();
...
v.Children.Add(i);
v.Children.Add(l);
```

## 9. References & Links

- XAML overview: http://en.wikipedia.org/wiki/Extensible_Application_Markup_Language
- XAML definition: http://download.microsoft.com/download/0/A/6/0A6F7755-9AF5-448B-907D-13985ACCF53E/%5BMS-XAML%5D.pdf
