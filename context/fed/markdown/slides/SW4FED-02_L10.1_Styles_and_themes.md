# L10 – Styles and themes

## Metadata

- **Lektion:** L10 – Styles and themes
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L10/Styles and themes.pdf (35 slides)
- **Emner dækket:**
  - Hvorfor styles: konsistent look and feel uden gentagen markup
  - `Style`, `Setter`, `TargetType` og `x:Key`
  - Explicit vs. implicit styles
  - Scope: control-level, page-level, app-level
  - `ResourceDictionary` og merged dictionaries
  - Style-hierarki og præcedens
  - Light mode / dark mode og `AppThemeBinding`
  - Dynamic styles med `DynamicResource`
  - Custom themes og skift af tema i runtime
  - Triggers: property, data, event, multi
  - Visual State Manager og `CommonStates`

---

## 1. Styles — problemet

Alle controls har properties, der kan tilpasses for at ændre deres udseende, for eksempel `TextColor` og `FontSize`. Men at anvende disse ændringer hver gang man tilføjer en control er ikke bare arbejdstungt — det introducerer også risiko for menneskelige fejl, som kan underminere den konsistens, vi stræber efter.

### Kodeeksempel — uden styles

```xml
<Label Text="These labels"
       HorizontalOptions="Center"
       VerticalOptions="Center"
       FontSize="18" />
<Label Text="are not"
       HorizontalOptions="Center"
       VerticalOptions="Center"
       FontSize="18" />
<Label Text="using styles"
       HorizontalOptions="Center"
       VerticalOptions="Center"
       FontSize="18" />
```

For at undgå dette problem kan vi bruge styles.

## 2. Hvad er styles?

Styles er samlinger af tilpasninger for en specifik control-type. En app kan styles ved at bruge `Style`-klassen til at gruppere en samling af property-værdier i ét objekt, som derefter kan anvendes på flere visuelle elementer. Det hjælper med at reducere gentagen markup, gør det lettere at ændre en apps udseende og opretholder konsistens.

## 3. Hvordan definerer man en style?

Hvert `Style`-objekt indeholder en samling af ét eller flere `Setter`-objekter, hvor hver `Setter` har en `Property` og en `Value`. `Property` er navnet på den bindable property på det element, styles anvendes på. `Value` er den værdi, der sættes på propertyen.

### Kodeeksempel

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiStockTake.UI.Pages.ReportPage"
             Title="ReportPage">
    <ContentPage.Resources>
        <Style x:Key="labelStyle" TargetType="Label">
            <Setter Property="HorizontalOptions" Value="Center" />
            <Setter Property="VerticalOptions" Value="Center" />
            <Setter Property="FontSize" Value="18" />
        </Style>
    </ContentPage.Resources>
```

## 4. Hvordan anvender man en style?

For at anvende en `Style` skal target-objektet være et `VisualElement`, der matcher `TargetType`-værdien på den pågældende `Style`.

### Kodeeksempel

```xml
<Label Text="Demonstrating an explicit style" Style="{StaticResource labelStyle}" />
```

## 5. Explicit eller implicit

En **explicit** `Style` defineres ved at angive både en `TargetType` og en `x:Key`-værdi. En explicit style anvendes ved at sætte target-elementets `Style`-property til `x:Key`-referencen.

```xml
<Style x:Key="labelStyle" TargetType="Label">
    <Setter Property="HorizontalOptions" Value="Center" />
    <Setter Property="VerticalOptions" Value="Center" />
    <Setter Property="FontSize" Value="18" />
</Style>
```

En **implicit** `Style` defineres ved kun at angive en `TargetType`. Styles anvendes så automatisk på alle elementer af den type inden for scope.

```xml
<Style TargetType="Label">
    <Setter Property="FontSize" Value="24" />
</Style>
```

## 6. Scope

Valget af, hvor man definerer en `Style`, bestemmer hvor den kan bruges — altså dens scope.

- `Style`-instanser defineret på **control-level** kan kun anvendes på den pågældende control og dens children.
- `Style`-instanser defineret på **page-level** kan kun anvendes på siden og dens children.
- `Style`-instanser defineret på **app-level** kan anvendes overalt i appen — det vil sige i appens `Resources`-collection.

### Kodeeksempel — App.xaml

```xml
<Application.Resources>
  <ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
      <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
      <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
    </ResourceDictionary.MergedDictionaries>
  </ResourceDictionary>
</Application.Resources>
```

## 7. Resource dictionaries

Et `ResourceDictionary` er et repository for ressourcer, der bruges af en .NET MAUI-app. Typiske ressourcer, der gemmes i et `ResourceDictionary`, er styles, control templates, data templates, konvertere og farver.

### Kodeeksempel — Styles.xaml

```xml
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <Style TargetType="ActivityIndicator">
        <Setter Property="Color" Value="{AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource White}}" />
    </Style>
```

## 8. Style-hierarki

Control-property-værdier anvendes hierarkisk:

1. App-level styles anvendes som default.
2. Page-level styles overskriver app-level styles.
3. En explicit style har forrang frem for en implicit style.
4. Værdier sat direkte på controls overskriver alle styles.

## 9. Light mode og dark mode

Tilpasselige farvetemaer eller paletter er almindelige i mange apps, men selv dem der ikke tilbyder det, tilbyder næsten altid en mulighed for at skifte mellem light mode og dark mode. Understøttelse af light og dark modes er indbygget i templates i .NET MAUI.

## 10. AppThemeBinding

`AppThemeBinding` lader dig reagere på systemets light og dark modes ved at angive en forskellig værdi for hver mode.

### Kodeeksempel

```xml
<Style TargetType="ActivityIndicator">
    <Setter Property="Color" Value="{AppThemeBinding
            Light={StaticResource Primary},
            Dark={StaticResource White}}" />
</Style>
```

Markup extension'en `AppThemeBinding` kan bruges hvor som helst — ikke kun til at sætte farver.

Slidesene viser MauiStockTake kørende i light mode (venstre) og dark mode (højre), hvor de samme styles giver to forskellige farvesæt.

## 11. Dynamic styles

Apps kan reagere på style-ændringer dynamisk i runtime ved at bruge dynamic resources. Hvor `StaticResource` udfører ét enkelt dictionary-opslag, opretholder `DynamicResource` et link til dictionary-nøglen. Hvis den dictionary-post, der er associeret med nøglen, erstattes, anvendes ændringen på det visuelle element.

### Kodeeksempel

```xml
<StackLayout>
  <SearchBar Placeholder="SearchBar demonstrating dynamic styles"
              Style="{DynamicResource SearchBarStyle}" />
</StackLayout>
```

`SearchBar` kan derefter få sin `Style`-definition opdateret i kode:

```csharp
Resources["SearchBarStyle"] = Resources["greenSearchBarStyle"];
```

Brug markup extension'en `StaticResource`, hvis du ikke har brug for at ændre app-temaet i runtime.

## 12. Custom themes

Man kan gå videre end light og dark modes og tilbyde fulde temaer for sine apps. Man kan udskifte farvepaletten i runtime, og ved brug af `DynamicResource` opdateres views automatisk til at afspejle de nye farver.

Der er dog en begrænsning: **`AppThemeBinding` og `DynamicResource` virker ikke sammen.**

Vi kan omgå denne begrænsning ved at bruge et **theme**, som er et resource dictionary med en kombineret farvepalet og et sæt styles bundtet sammen. Fordi et theme ikke afhænger af `DynamicResource`, kan styles i temaet bruge `AppThemeBinding`.

## 13. Custom theme — sådan gør man

- Opret en ny mappe inde i `Resources`-mappen kaldet `Themes`, og tilføj to nye resource dictionaries, ét kaldet `DefaultTheme` og et andet kaldet `SandyTheme` (eller hvad du nu foretrækker).
  - Sørg for at bruge templaten frem for bare at oprette en XAML-fil.
  - Når man bruger templaten, inkluderes også en C# code-behind, hvilket er påkrævet for at det virker.
- Kopiér indholdet fra `Colors.xaml` og indsæt det i `DefaultTheme`-filen.
- Kopiér alle styles fra `Styles.xaml` og indsæt dem i `DefaultTheme`-filen efter farvedefinitionerne og før den afsluttende `ResourceDictionary`-tag.
- Gentag processen for at oprette Sandy-temaet — kopiér de samme farver og styles ind i `SandyTheme.xaml`, og modificér derefter farveværdierne i `SandyTheme`.

## 14. Ændring af App.xaml

I `App.xaml` erstattes de to eksisterende dictionaries med det nye default-tema.

### Kodeeksempel

```xml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MauiStockTake.UI"
             x:Class="MauiStockTake.UI.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Themes/DefaultTheme.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

## 15. DefaultTheme.xaml

### Kodeeksempel

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                    x:Class="MauiStockTake.UI.Resources.Themes.DefaultTheme">
    <Color x:Key="Primary">#215377</Color>
    <Color x:Key="Secondary">#8dacb9</Color>
    <Color x:Key="PrimaryBackground">#8dacb9</Color>
    . . .

   <Style TargetType="ActivityIndicator">
       <Setter Property="Color" Value="{AppThemeBinding Light={StaticResource
               Primary}, Dark={StaticResource PrimaryDark}}" />
   </Style>
   <Style TargetType="IndicatorView">
       <Setter Property="IndicatorColor" Value="{AppThemeBinding
             Light={StaticResource Gray200}, Dark={StaticResource Gray500}}"/>
   . . .
```

## 16. Skift af tema i kode

Temaet skiftes ved at rydde `MergedDictionaries` og tilføje det nye tema-dictionary.

### Kodeeksempel

```csharp
public partial class App : Application
{
    public static Theme Theme { get; set; } = Theme.Default;
```

```csharp
private void ThemeMenuItem_Clicked(object sender, EventArgs e)
{
    if (App.Theme == Theme.Default)
    {
         App.Theme = Theme.Sandy;
         ThemeMenuItem.Text = "Switch to Default Theme";
         ICollection<ResourceDictionary> mergedDictionaries =
                  Application.Current.Resources.MergedDictionaries;
    if (mergedDictionaries != null)
         {
             mergedDictionaries.Clear();
             mergedDictionaries.Add(new SandyTheme());
    }
    }
    else
    {
         App.Theme = Theme.Default;
         ThemeMenuItem.Text = "Switch to Sandy Theme";
```

<!-- uklart i kilden: slide 21 er beskåret, else-grenen er ikke fuldt gengivet -->

Slidesene viser en flyout med et theme-switching menu item, og at custom themes stadig reagerer på dark og light mode.

## 17. Responding to visual state changes — overblik

I .NET MAUI er der to måder, hvorpå vi kan gøre vores UI dynamisk som reaktion på state-ændringer:

- **Triggers** — lader dig udtrykke actions deklarativt i XAML, som ændrer udseendet af controls baseret på events eller dataændringer.
- **Visual state manager** — giver en struktureret måde at foretage visuelle ændringer i brugergrænsefladen baseret på events eller dataændringer.

## 18. Triggers

Man kan tildele en trigger direkte til en controls `Triggers`-collection, eller tilføje den til et page-level eller app-level resource dictionary, så den anvendes på flere controls. Triggers bruger `Setter` til at definere den property og værdi, man vil styre — præcis som i en style.

Triggers findes i flere varianter:

- Property triggers
- Data triggers
- Event triggers
- Multi-triggers
- EnterActions og ExitActions
- State triggers

## 19. Property triggers

En trigger, der anvender property-værdier eller udfører actions, når den angivne property opfylder en angivet betingelse.

### Kodeeksempel

```xml
<Entry Placeholder="Enter name">
  <Entry.Triggers>
    <Trigger TargetType="Entry" Property="IsFocused" Value="True">
    <Setter Property="BackgroundColor" Value="Yellow" />
    <!-- Multiple Setter elements are allowed -->
    </Trigger>
  </Entry.Triggers>
</Entry>
```

## 20. Anvend en trigger via en style

Triggers kan også tilføjes til en `Style`-deklaration.

### Kodeeksempel

```xml
<ContentPage.Resources>
  <Style TargetType="Entry">
    <Style.Triggers>
      <Trigger TargetType="Entry" Property="IsFocused" Value="True">
        <Setter Property="BackgroundColor" Value="Yellow" />
        <!-- Multiple Setter elements are allowed -->
      </Trigger>
    </Style.Triggers>
  </Style>
</ContentPage.Resources>
```

## 21. Data triggers

En `DataTrigger` repræsenterer en trigger, der anvender property-værdier eller udfører actions, når de bundne data opfylder en angivet betingelse.

### Kodeeksempel

```xml
<Entry x:Name="entry"
       Text=""
       Placeholder="Enter text"
       />
<Button Text="Save">
  <Button.Triggers>
    <DataTrigger TargetType="Button"
                 Binding="{Binding Source={x:Reference entry}, Path=Text.Length}"
                 Value="0">
      <Setter Property="IsEnabled" Value="False" />
      <!-- Multiple Setter elements are allowed -->
    </DataTrigger>
  </Button.Triggers>
</Button>
```

## 22. Event triggers

Repræsenterer en trigger, der anvender et sæt actions som reaktion på et event. I modsætning til `Trigger` har `EventTrigger` intet begreb om afslutning af state, så actions bliver ikke rullet tilbage, når den betingelse, der rejste eventet, ikke længere er sand.

### Kodeeksempel

```xml
<EventTrigger Event="TextChanged">
    <local:NumericValidationTriggerAction />
</EventTrigger>
```

En trigger action-implementation skal implementere den generiske klasse `TriggerAction<T>`, hvor den generiske parameter svarer til typen af den control, triggeren anvendes på.

```csharp
public class NumericValidationTriggerAction : TriggerAction<Entry>
{
  protected override void Invoke(Entry entry)
  {
     double result; bool isValid = Double.TryParse(entry.Text, out result);
     entry.TextColor = isValid ? Colors.Black : Colors.Red; } }
```

## 23. Multi-triggers

En `MultiTrigger` repræsenterer en trigger, der anvender property-værdier eller udfører actions, når et sæt af betingelser er opfyldt. Alle betingelser skal være sande, før `Setter`-objekterne anvendes.

### Kodeeksempel

```xml
<Entry x:Name="email" Text="" />
<Entry x:Name="phone" Text="" />
<Button Text="Save">
  <Button.Triggers>
    <MultiTrigger TargetType="Button">
      <MultiTrigger.Conditions>
        <BindingCondition Binding="{Binding Source={x:Reference email},
                                    Path=Text.Length}"
                          Value="0" />
        <BindingCondition Binding="{Binding Source={x:Reference phone},
                                    Path=Text.Length}"
                          Value="0" />
      </MultiTrigger.Conditions>
      <Setter Property="IsEnabled" Value="False" />
      <!-- multiple Setter elements are allowed -->
    </MultiTrigger>
  </Button.Triggers>
</Button>
```

## 24. Visual State Manager

Med Visual State Manager er de visuelle states inden for en visual state group altid gensidigt udelukkende — på ethvert tidspunkt er kun én state i hver gruppe den aktuelle state.

.NET MAUI's Visual State Manager definerer en visual state group ved navn `CommonStates` med følgende visuelle states:

- `Normal`
- `Disabled`
- `Focused`
- `Selected`
- `PointerOver`

Man kan tilknytte Visual State Manager-markup til et enkelt view, eller definere det i en style, hvis det gælder flere views.

## 25. Definér visual states på et view

### Kodeeksempel

```xml
<Entry FontSize="18">
  <VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="CommonStates">
      <VisualState x:Name="Normal">
        <VisualState.Setters> <Setter Property="BackgroundColor" Value="Lime" />
      </VisualState.Setters>
    </VisualState>
    <VisualState x:Name="Focused">
      <VisualState.Setters>
        <Setter Property="FontSize" Value="36" />
      </VisualState.Setters>
    </VisualState>
    <VisualState x:Name="Disabled">
      <VisualState.Setters>
        <Setter Property="BackgroundColor" Value="Pink" />
      </VisualState.Setters>
    </VisualState>
    <VisualState x:Name="PointerOver">
      <VisualState.Setters>
        <Setter Property="BackgroundColor" Value="LightBlue" />
      </VisualState.Setters>
     </VisualState> </VisualStateGroup> </VisualStateManager.VisualStateGroups>
```

## 26. Definér visual states i en style

### Kodeeksempel

```xml
<Style TargetType="Entry">
  <Setter Property="FontSize" Value="18" />
    <Setter Property="VisualStateManager.VisualStateGroups">
      <VisualStateGroupList>
        <VisualStateGroup x:Name="CommonStates">
          <VisualState x:Name="Normal">
            <VisualState.Setters>
              <Setter Property="BackgroundColor" Value="Lime" />
            </VisualState.Setters>
          </VisualState>
          <VisualState x:Name="Focused">
            <VisualState.Setters>
              <Setter Property="FontSize" Value="36" />
                <Setter Property="BackgroundColor" Value="Lime" />
              </VisualState.Setters>
            </VisualState>
          <VisualState x:Name="Disabled">
            <VisualState.Setters>
              <Setter Property="BackgroundColor" Value="Pink" />
```

<!-- uklart i kilden: slide 33 er beskåret, de afsluttende tags mangler -->

## 27. Visual states i .NET MAUI

| Klasse | States |
| --- | --- |
| `Button` | `Pressed` |
| `CarouselView` | `DefaultItem`, `CurrentItem`, `PreviousItem`, `NextItem` |
| `CheckBox` | `IsChecked` |
| `CollectionView` | `Selected` |
| `ImageButton` | `Pressed` |
| `RadioButton` | `Checked`, `Unchecked` |
| `Switch` | `On`, `Off` |
| `VisualElement` | `Normal`, `Disabled`, `Focused`, `PointerOver` |

## 28. References & Links

- .NET MAUI in Action af Matt Goldman
- Style apps using XAML — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/styles/xaml
- Theme an app — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/theming
- Triggers — https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/triggers#state-triggers
- Visual states — https://learn.microsoft.com/en-us/dotnet/maui/user-interface/visual-states
