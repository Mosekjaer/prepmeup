# Kapitel 10 — Styles, themes, and multiplatform layouts

## Metadata

- **Kapitel:** 10 — Styles, themes, and multiplatform layouts
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L10
- **Hovedemner:**
  - Styles som samlinger af `Setter`s med `TargetType`, og hvordan de sikrer konsistent look and feel
  - Implicit style vs. explicit style (nøgle via `x:Key`) og style-hierarkiet app → page → control
  - Resource dictionaries: `Colors.xaml`, `Styles.xaml` og merged dictionaries i `App.xaml`
  - Brushes: `Background` (Brush) vs. `BackgroundColor` (Color), `LinearGradientBrush` med `GradientStop`
  - Themes og light/dark mode via `AppThemeBinding`, samt runtime-temaskift med merged dictionaries
  - `StaticResource` vs. `DynamicResource` og deres indbyrdes begrænsninger
  - Triggers: property triggers, data triggers og forskellen til event triggers
  - Visual state (`VisualStateManager`), visual state groups og de fem common states
  - Multiplatform: device idiom, `DeviceInfo.Current.Idiom`, compiler directives og `OnIdiom`-markup extension
  - Desktop-features: `MenuBarItems`, `MenuFlyoutItem` og flere vinduer via `Window` + `OpenWindow`

---

## Introduktion

Kapitlet bygger videre på idéen fra kapitel 1 og 5 om en konsistent UX på tværs af
platforme. Uanset om man går efter "pixel perfect" eller blot at appen skal føles som
*din* app overalt, er visuel konsistens kritisk for, at brugerne forstår hvordan appen
virker. Primær- og sekundærfarver hjælper brugeren med at forstå formålet med en knap,
og det samme gør genkendelige knapformer — fx en cirkulær floating action button, der
signalerer en tilføjende handling. Omvendt skaber for mange forskellige knapformer og
-farver forvirring.

**Figur 10.1** viser to versioner af den samme UI, hvor brugeren kan trykke på en knap
for at få 25 % rabat. I venstre version ligner knappen alle andre knapper i appen, og
brugeren kan straks se, hvad der skal gøres. I højre version er knappen erstattet af en
stjerne, som visuelt er mere iøjnefaldende, men som ikke er i tråd med appens
designsprog og på ingen måde signalerer, at den kan trykkes på. Pointen er, at selv små
variationer i præsentationen af controls skaber forvirring — og at venstre eksempel
stadig har et problem, nemlig at to identiske knapper udvander call to action; kuponknappen
burde have haft en secondary style.

Kapitlet dækker derfor tre ting: styles til konsistent udseende, themes (særligt light og
dark mode, som brugere ofte vil kunne vælge eller synkronisere med OS'et), og hvordan man
udnytter de UX-forventninger brugere har til forskellige device paradigms (desktop vs.
mobil).

---

## 10.1 Creating a consistent look and feel

MauiStockTake skal afspejle surf-looket i Mildreds forretning, så appens styles opdateres
til at matche. Tilgangen er den "consistent but not identical" UI, der i .NET MAUI opnås
med styles.

### 10.1.1 Styles

Alle controls i .NET MAUI har properties, der kan tilpasses. Nogle hører til selve
kontrollen — fx `TextColor` på `Label` og `Entry` (bemærk at flere controls kan have
properties med samme navn) — mens andre, som `HorizontalOptions` og `WidthRequest`,
arves fra baseklasserne `View` og `VisualElement`.

Man *kan* sætte disse properties direkte på hver enkelt control, men det er både
arbejdskrævende og introducerer risiko for menneskelige fejl, hvilket underminerer den
konsistens man søger. Løsningen er **styles**.

En style er en samling af tilpasninger for en bestemt control-type. .NET MAUI-templates
har allerede indbyggede styles — det er derfor en `Button` ser nogenlunde ens ud på hver
platform.

#### Styling buttons

Både default-template og `blankmaui`-template definerer et sæt styles. I `App.xaml`
indlæses to ekstra filer i appens resource dictionary, begge fra `Resources/Styles`-mappen:
`Colors.xaml` (farver brugt i hele appen) og `Styles.xaml` (default styles, som igen
bruger farverne fra `Colors.xaml`). Fordi de indlæses i appens resource dictionary,
gælder de for hele appen.

Først opdateres farverne. I `Colors.xaml` ændres `Primary` og `Secondary`, og
`PrimaryBackground` og `SecondaryBackground` tilføjes.

**Listing 10.1 Color definitions to add to the Colors.xaml resource dictionary**

```xml
<Color x:Key="Primary">#215377</Color>
<Color x:Key="Secondary">#8dacb9</Color>
<Color x:Key="PrimaryBackground">#8dacb9</Color>
<Color x:Key="SecondaryBackground">#b4bcc7</Color>
```

Kører man appen nu, ses ændringen i `Button`-farven med det samme. De to
baggrundsfarver er til styling af `Page` og bruges senere.

I `Styles.xaml` findes den style, der har `TargetType` sat til `Button`. **Styles skal
definere en `TargetType`** og kan derefter bruge `Setter`s til at definere, hvordan
properties på denne target type skal se ud. For en style med `TargetType="Button"` kan
man bruge en `Setter` for enhver property på en `Button`.

`Button`-stylen har en `Setter` for `BackgroundColor`, der bruger `AppThemeBinding`:
i light mode sættes `BackgroundColor` til `Primary` fra `Colors.xaml`, i dark mode til
hvid.

```xml
<Setter Property="BackgroundColor" Value="{AppThemeBinding Light={
➥ StaticResource Primary}, Dark={StaticResource White}}" />
```

To ændringer skal laves for at matche Mildreds brand guidelines:

1. Tilføj en `Setter` for `HeightRequest` med værdien 50, så alle knapper i appen har
   samme højde.
2. Ændr `CornerRadius`-setteren fra 8 til 25 — halvdelen af højden, hvilket giver
   perfekt afrundede kanter på hver `Button`.

Derudover opdateres `Setter` for `MinimumWidthRequest` til værdien 100. Alt efter IDE
eller editor vil Intellisense foreslå property-navnene undervejs.

**Figur 10.2** viser Login-knappen på `LoginPage` med den nye styling. Pointen er, at
knappen ikke er tilpasset direkte — appens styles er blevet anvendt og giver den ønskede
visuelle stil "gratis".

#### Styling pages

Pages er ikke lige så tilpasningsvenlige som andre views, men har alligevel properties man
kan ønske at gøre konsistente, fx `Padding`. For MauiStockTake laves en gradient-baggrund,
der passer til det beachy surf-tema.

I `Styles.xaml` findes stylen med `TargetType="Page"`. Den eksisterende `Setter` for
`BackgroundColor` slettes og erstattes af en `Setter` for `Background`.

> **What is the difference between `Background` and `BackgroundColor`?**
>
> Baseklassen `VisualElement`, som alle views nedarver fra, har både en `Background`- og
> en `BackgroundColor`-property. De ligner hinanden ved begge at sætte baggrunden på et
> view, men adskiller sig ved, at `BackgroundColor` er af typen `Color`, mens `Background`
> er af typen `Brush`.
>
> Brushes lader dig "male" et område (fx baggrunden på et view). Ud over en enkelt farve
> (med `SolidColorBrush`, som i `Background`-tilfældet funktionelt er identisk med at sætte
> `BackgroundColor`) har du også `LinearGradientBrush` og `RadialGradientBrush`.
>
> Vil du blot have en enkelt farve, kan du sætte `Background` med en `SolidColorBrush`,
> men den simplere tilgang er at sætte `BackgroundColor`. Gradient brushes bruges igen i
> næste kapitel.

Fordi `Background` er af typen `Brush`, og der skal tildeles en gradient, kan man ikke
bruge den inline `Value`-property på `Setter`en. I stedet tilføjes den eksplicit med tags,
så en `LinearGradientBrush` kan tildeles `Value` med `StartPoint` og `EndPoint`. Disse er
af typen `Point` og består af en x- og y-værdi mellem 0 og 1, som repræsenterer en andel
af viewets bredde og højde.

**Figur 10.3** illustrerer punkter i et view: hvert punkt defineres af x og y udtrykt som
en brøkdel (mellem 0 og 1) af viewets bredde (x) og højde (y). Øverste venstre hjørne er
`0,0`, midten er `0.5,0.5`, og nederste højre er `1,1`.

Gradienten på siden skal løbe fra nederst til venstre til øverst til højre, så `StartPoint`
sættes til `0,1` og `EndPoint` til `1,0`. `StartPoint` og `EndPoint` definerer gradientens
**akse** — linjen fra start til slut, som gradienten løber langs. Derudover skal der
defineres **gradient stops**, som angiver farver på positioner langs aksen. `GradientStop`
har to properties: `Color` (farven på positionen) og `Offset` (en fraktionel position langs
aksen fra 0 til 1).

Man kan angive lige så mange gradient stops man vil; her bruges kun to, baseret på
`PrimaryBackground` (offset 0) og `SecondaryBackground` (offset 1).

**Listing 10.2 The background setter for the Page style**

```xml
<Setter Property="Background">
<Setter.Value>
<LinearGradientBrush StartPoint="0,1" EndPoint="1,0">
<GradientStop Color="{StaticResource
➥ PrimaryBackground}” Offset=”0”/>
<GradientStop Color=”{StaticResource
➥ SecondaryBackground}" Offset="1"/>
</LinearGradientBrush>
</Setter.Value>
</Setter>
```

**Figur 10.4** viser `LoginPage`, der automatisk arver `Background`-property'en fra
stylen. Gradienten er ikke sat på siden selv, men på en style, der targeter `Page`-typen
og er importeret på app-niveau.

#### Style hierarchy

Styles tilføjes til en **resource dictionary**, som enten kan være appens resource
dictionary (som med de indbyggede styles) eller en pages resource dictionary. Control
properties anvendes hierarkisk: først styles i appens resource dictionary, derefter dem i
pagens resource dictionary, og til sidst properties sat eksplicit på selve kontrollen.

**Figur 10.5** illustrerer dette hierarki: app-level styles anvendes som default,
page-level styles overskriver app-level styles, og værdier sat direkte på controls
overskriver enhver style.

Med andre ord gælder: styles med **snævrere scope** (defineret på et mere granulært
niveau) har forrang over bredt scopede styles. Ud over scope gælder, at en **explicit
style** har forrang over en **implicit style**.

#### Implicit vs. explicit styles

De styles der er set indtil nu er **implicit styles**. En implicit style defineres mod en
target type, og ethvert view af den type, som hierarkisk er inden for stylens scope,
anvender den.

En **explicit style** defineres næsten identisk, bortset fra at den tilføjer en **key** —
en identifier som ressourcen kan refereres med. Et view anvender ikke en explicit style,
medmindre stylen eksplicit tildeles via `Style`-property'en.

Count-labelen er lidt kedelig og svær at læse, så den forbedres med en explicit style.

> **NOTE** I en rigtig app ville det give langt mere mening at sætte disse properties
> direkte på viewet. Værdien af en style opstår, når den anvendes på mere end én control.

I `Styles.xaml` oprettes en ny explicit style med `TargetType="Label"` og key
`CountLabelStyle`. Count-labelen har allerede `FontSize`, `HorizontalOptions` og
`VerticalOptions` defineret, så disse flyttes ind i stylen, og til sidst tilføjes
`TextColor` sat til appens `Primary`-farve.

**Listing 10.3 The CountLabelStyle style**

```xml
<Style TargetType="Label" x:Key="CountLabelStyle">
<Setter Property="FontSize" Value="64"/>
<Setter Property="HorizontalOptions" Value="Center"/>
<Setter Property="VerticalOptions" Value="Center"/>
<Setter Property="TextColor" Value="{StaticResource Primary}"/>
</Style>
```

Derefter fjernes de nu overflødige properties fra count-labelen, og stylen tildeles:

```xml
<Label Grid.Row="2"
Text="{Binding Count}"
Style="{StaticResource CountLabelStyle}"/>
```

**Figur 10.6** viser resultatet: en explicit style styrer labelens vandrette og lodrette
position, skriftstørrelse og tekstfarve. Disse tilpasninger kan anvendes på ethvert view
med matchende target type (her `Label`) ved at bruge `Style`-property'en og referere til
den explicit style.

Én ting passer stadig ikke ind: tab-baren har hvid baggrund. I `Styles.xaml` findes stylen
for `Shell` og `Setter` for `Shell.TabBarBackgroundColor`; `Value` slettes og erstattes af
`{StaticResource PrimaryBackground}`.

**Figur 10.7** viser, at tab-barens baggrund nu er konsistent med resten af appen og
styres af en `Setter` i appens styles.

---

### 10.1.2 Themes

Udviklere er vant til at kunne tilpasse tema eller udseende i de værktøjer de bruger.
Visual Studio Code — verdens mest populære udviklerværktøj ifølge Stack Overflows
developer survey (http://mng.bz/OxWo) — tillader omfattende customization, og themes er
blandt de mest downloadede extensions.

Tilpasselige farvetemaer er udbredte, men selv apps uden dem tilbyder næsten altid at
skifte mellem light mode og dark mode. **Figur 10.8** gengiver en Twitter-afstemning om
farvetemaer i apps; svarene indikerer overvældende, at light- og dark mode-understøttelse
bør prioriteres. Det er ikke en videnskabelig undersøgelse, men signalet er tydeligt.

#### Light mode and dark mode

Mildred og hendes team tæller ofte lager om aftenen, så dark mode-understøttelse er
relevant for at undgå unødig øjenbelastning. Understøttelsen er indbygget i .NET MAUI's
templates: opretter man et nyt projekt med default- eller `blankmaui`-template (eller
åbner et projekt fra tidligere kapitler), skifter farverne allerede, når enheden skifter
mellem dark og light mode.

Dark mode-farverne opdateres i `Colors.xaml`.

**Listing 10.4 Dark mode color definitions for MauiStockTake**

```xml
<Color x:Key="PrimaryDark">#7b98aa</Color>
<Color x:Key="PrimaryDarkBackground">#141d31</Color>
<Color x:Key="SecondaryDarkBackground">#212f51</Color>
```

Markup extension'en **`AppThemeBinding`** lader dig reagere på systemets light og dark
mode ved at angive forskellige værdier for hver mode. I MauiStockTake bruges
`AppThemeBinding` til at styre light/dark mode for:

- Page background
- Activity indicator
- Button
- Flyout background
- Tab bar
- Navigation (title) bar

**ActivityIndicator:** bruger i dag `Primary` i light mode og `White` i dark mode;
dark mode-farven ændres til `PrimaryDark`.

**Button:** samme ændring i `Setter` for `Background`. For `TextColor`-setteren fjernes
`AppThemeBinding` helt, og `Value` sættes til `White` som `StaticResource` — knappens
`Background` er ikke længere hvid i dark mode, og hvid tekst kontrasterer lige så godt
mod `PrimaryDark` som mod `Primary`.

**Page:** `Setter` for `Background` har to gradient stops, som hver refererer til en farve
i resource dictionary'en. De kunne blot skiftes til `AppThemeBinding`, men bogen går et
skridt videre og gør hele gradient-brushen til en ressource. Hele `LinearGradientBrush`
klippes ud fra `<Setter.Value>`-taggene og indsættes i `Colors.xaml` (hvor der i forvejen
er `SolidColorBrush`-ressourcer). Den får en key, `BackgroundGradient`, og gradient stops
opdateres til `AppThemeBinding` med de eksisterende light mode-farver plus de nye dark
mode-baggrundsfarver fra listing 10.4.

**Listing 10.5 The BackgroundGradient resource**

```xml
<LinearGradientBrush x:Key="BackgroundGradient"
➥ StartPoint="0,1" EndPoint="1,0">
<GradientStop Color="{AppThemeBinding Light={

①

➥ StaticResource PrimaryBackground}, Dark={
➥ StaticResource PrimaryDarkBackground}}" Offset=
➥ "0"/>

②

<GradientStop Color="{AppThemeBinding Light={
➥ StaticResource SecondaryBackground}, Dark={
➥ StaticResource SecondaryDarkBackground}}” Offset=
➥ "1"/>
</LinearGradientBrush>

②
```

① Gradient-brushen har nu en key defineret, så den kan refereres i hele appen.
② Gradient stops er opdateret til at bruge `AppThemeBinding`, så light mode- og dark
mode-baggrundsressourcerne bruges tilsvarende.

Nu hvor gradient-baggrunden er en defineret ressource, kan `Setter` for `Background` på
`Page`-stylen forenkles til et enkelt selvlukkende tag:

```xml
<Setter Property="Background" Value="{StaticResource BackgroundGradient}"/>
```

**Tab bar:** `AppThemeBinding` sættes tilbage på `Shell.TabBarBackgroundColor` med
`PrimaryBackground` til light og `PrimaryDarkBackground` til dark:

```xml
<Setter Property="Shell.TabBarBackgroundColor" Value="{AppThemeBinding
➥ Light={StaticResource PrimaryBackground}, Dark={StaticResource
➥ PrimaryDarkBackground}}” />
```

**Flyout background:** en simpel tilføjelse af en `Setter` for `Shell.FlyoutBackgroundColor`:

```xml
<Setter Property=”Shell.FlyoutBackgroundColor” Value=”{AppThemeBinding
➥ Light={StaticResource PrimaryBackground}, Dark={StaticResource
➥ PrimaryDarkBackground}}"/>
```

**Navigation bar:** sidste `Shell`-ændring er `Setter` for `BackgroundColor`, som sætter
baggrunden på navigationsbaren med menuknap og titel. Den har allerede `AppThemeBinding`;
`Primary` beholdes til light mode, mens dark mode-værdien ændres fra `Gray950` til
`SecondaryDarkBackground`. Det får både top- og bundbar til at flyde sammen med
baggrundsgradienten — og fordi gradienten løber diagonalt, ses en let kant, som giver en
fin UI-effekt.

Til sidst opdateres den explicit style for `CounterLabel`, så `TextColor`-setteren bruger
`AppThemeBinding` med `Primary` til light mode og `PrimaryDark` til dark mode.

**Figur 10.9** viser MauiStockTake i light mode (venstre) og dark mode (højre). Fordi
`AppThemeBinding` bruges, vælges denne mode ikke inde i appen, men er bundet til OS'ets
light- eller dark-tema.

Der er langt flere styles i `Styles.xaml`, der kan tilpasses, og `AppThemeBinding` kan
bruges alle steder — ikke kun til farver. Man kunne fx bruge en anden version af
firmalogoet på login-skærmen i dark mode eller endda have forskellige tekstværdier på
labels.

#### Custom themes

Man kan gå videre end light og dark mode og tilbyde fulde themes. Figur 10.8 viser, at
dette ikke er nær så efterspurgt som light/dark mode — omend afstemningen langt fra er
rigoristisk videnskabelig og blev afholdt i en app, der faktisk tilbyder tilpasselige
themes.

Indtil nu er `StaticResource` blevet brugt til at referere farver i appens merged resource
dictionary. Som navnet antyder, betragtes værdien som statisk og ændres ikke ved runtime.
Bruger man derimod modstykket **`DynamicResource`**, opdateres UI'et ved runtime, når
kildeværdien ændres. Dermed kan man udskifte farvepaletten ved runtime, og views der
bruger stylen opdaterer automatisk.

**Figur 10.10** viser en app med to farvepaletter og ét sæt styles. Stylene bruger
`DynamicResource`, så farvepaletten kan udskiftes ved runtime, og views der anvender
stylen opdateres automatisk i realtid.

Microsofts dokumentation dækker denne tilgang til theming (http://mng.bz/Y1vK). Der er dog
en **begrænsning: `AppThemeBinding` og `DynamicResource` virker ikke sammen.** Det fungerer
fint til brugervalgte farveskemaer (kun med `DynamicResource`), men tillader ikke binding
til OS'ets light/dark mode-skift.

Løsningen er at bruge et **theme** — en resource dictionary med en kombineret farvepalet
*og* et sæt styles bundtet sammen. Fordi et theme ikke afhænger af `DynamicResource`, kan
stylene i temaet bruge `AppThemeBinding`.

**Figur 10.11** viser et theme, der indeholder både farver og styles. Vil man skifte tema,
udskifter man hele temaet frem for kun farvepaletten. Fordi det bruger `StaticResource`
frem for `DynamicResource`, kan det også bruge `AppThemeBinding` og understøtte light og
dark mode.

Mildred har bedt om et **Sandy**-tema ved siden af default-temaet, og begge skal
understøtte automatisk light/dark mode i respons på OS-ændringer.

Der oprettes en ny mappe `Themes` inde i `Resources`-mappen med to nye resource
dictionaries: `DefaultTheme` og `SandyTheme`. I Visual Studio bruges templaten
".NET MAUI Resource Dictionary (XAML)", eller via .NET CLI:

```csharp
dotnet new maui-dict-xaml -na MauiStockTake.UI.Resources.Themes -n
➥ SandyTheme
```

> **WARNING** Sørg for at bruge templaten frem for blot at oprette en XAML-fil. Templaten
> inkluderer også en C# code-behind, som kritisk nok kalder `InitializeComponent()` i
> constructoren. Det er påkrævet ifølge den tidligere linkede dokumentation.

Themes bygges ved at kombinere de eksisterende farvepaletter og styles: alt mellem
`<ResourceDictionary..>...</ResourceDictionary>`-taggene i `Colors.xaml` (altså alle
farve- og brush-definitioner) kopieres øverst i `DefaultTheme.xaml` mellem
`ResourceDictionary`-taggene der. Derefter kopieres alle styles fra `Styles.xaml` ind i
`DefaultTheme.xaml` efter farvedefinitionerne og før det lukkende `ResourceDictionary`-tag.
Samme proces gentages for `SandyTheme.xaml`. De to temaer er nu identiske, så Sandy-temaets
farver opdateres.

**Listing 10.6 The colors to update for the Sandy theme**

```xml
<Color x:Key="Primary">#6b433b</Color>
<Color x:Key="PrimaryDark">#d9ceb6</Color>
<Color x:Key="Secondary">#ede8dd</Color>
<Color x:Key="PrimaryBackground">#cdb08a</Color>
<Color x:Key="SecondaryBackground">#dadccb</Color>
<Color x:Key="PrimaryDarkBackground">#251f13</Color>
<Color x:Key="SecondaryDarkBackground">#453b24</Color>
```

Default-temaet sættes ved at fjerne `Colors.xaml` og `Styles.xaml` fra resource
dictionary'en i `App.xaml` og erstatte dem med det nye tema:

```xml
<ResourceDictionary Source="Resources/Themes/DefaultTheme.xaml" />
```

Kører man appen nu, skal alt se ud som før — der er ikke ændret noget kvalitativt, kun
kombineret styles og farver i et tema.

For at lade brugeren skifte tema tilføjes et `MenuItem` til `Shell`. Ikonet
`icon_palette.svg` hentes fra kapitlets mappe i bogens onlineressourcer og importeres i
`Resources/Images` i `MauiStockTake.UI`. Der findes allerede et `MenuItem` til logout;
det nye tilføjes i `AppShell.xaml` før logout-menupunktet.

**Listing 10.7 The Change Theme menu item**

```xml
<MenuItem IconImageSource="icon_palette.png"
x:Name="ThemeMenuItem"
Clicked="ThemeMenuItem_Clicked"/>
```

`IconImageSource` er sat, der er tilføjet en event handler, og menupunktet har fået et
navn. `Text` er *ikke* sat, fordi den skal ændre sig afhængigt af det aktuelle tema.
Startværdien sættes i `AppShell.xaml.cs`-constructoren:

```csharp
ThemeMenuItem.Text = "Switch to Sandy Theme";
```

For at holde styr på hvilket tema der er i brug, tilføjes en enum med alle tilgængelige
temaer. Der er kun to nu, men flere kan let tilføjes senere; tilføjes der flere, er det en
god idé med en settings-side, hvor brugeren kan vælge — den nuværende tilgang tillader kun
at toggle mellem to. Filen `Theme.cs` tilføjes i `Helpers`-mappen i `MauiStockTake.UI`.

**Listing 10.8 The Theme enum**

```csharp
namespace MauiStockTake.UI.Helpers;
public enum Theme
{
Default,
Sandy
}
```

Til at spore det aktuelle tema tilføjes en static property til `App`-klassen i
`App.xaml.cs`:

```csharp
public static Theme Theme { get; set; } = Theme.Default;
```

Det giver en static instans af `Theme`-enummen med default-værdien allerede sat. Sidste
skridt er event handleren `ThemeMenuItem_Clicked` i `AppShell.xaml.cs`, som gør følgende:

1. Tjekker den statiske enum for at se, hvilket tema der er i brug.
2. Sætter enummen til den anden værdi.
3. Opdaterer menupunktets tekst.
4. Henter den merged resource dictionary fra `App`-klassen.
5. Rydder dictionary'en.
6. Indlæser det alternative tema.

**Listing 10.9 The ThemeMenuItem_Clicked event handler**

```csharp
private void ThemeMenuItem_Clicked(object sender, EventArgs e)
{
if (App.Theme == Theme.Default)
{
App.Theme = Theme.Sandy;
ThemeMenuItem.Text = "Switch to Default Theme";
ICollection<ResourceDictionary>
➥ mergedDictionaries = Application.Current.
➥ Resources.MergedDictionaries;
if (mergedDictionaries != null)
{

①

mergedDictionaries.Clear();

②
③

mergedDictionaries.Add(new SandyTheme());
}
}
else
{
App.Theme = Theme.Default;
ThemeMenuItem.Text = "Switch to Sandy Theme";

ICollection<ResourceDictionary> mergedDictionaries = Application.
➥ Current.Resources.MergedDictionaries;
if (mergedDictionaries != null)
{
mergedDictionaries.Clear();
mergedDictionaries.Add(new DefaultTheme());
}
}
}
```

① XAML-filerne tilføjet i `App.xaml` ligger i en merged dictionary; her tilføjes en
reference til den.
② Rydder den nuværende merged dictionary for at fjerne det aktuelle tema.
③ Indlæser det ønskede tema i den merged dictionary.

**Figur 10.12** viser flyouten med det nye tema-skift-menupunkt. Teksten skifter afhængigt
af det aktuelt valgte tema, og Sandy-temaet reagerer på light mode (venstre) og dark mode
(højre) præcis som default-temaet gør.

Resultatet er to temaer, der begge bruger `AppThemeBinding` til at understøtte light og
dark mode, og som kan skiftes ud ved runtime. Tilgangen er en anelse grovkornet — har man
ikke brug for både light/dark mode *og* flere farvepaletter, er man bedre tjent med enten
`AppThemeBinding` eller `DynamicResource` alene. Men denne tilgang giver begge dele, hvilket
ingen af de andre gør.

---

## 10.2 Responding to state changes

UI'et bygget indtil nu har været relativt statisk: når det først er defineret, ændrer det
sig ikke. I nogle tilfælde vil man have UI'et til at være mere dynamisk og ændre sig som
respons på ændringer i appens tilstand — det er det, der menes med **UI behavior**.
Kapitel 7 brugte en `Switch` som eksempel, hvor baggrundsfarven kunne ændres alt efter
dens state. I .NET MAUI findes der flere måder at gøre UI'et dynamisk på.

> **Orientation changes**
>
> Den mest effektive måde at reagere på orientationsændringer er at designe appen
> responsivt, altså bruge layouts der tilpasser sig forskellige orienteringer og
> skærmstørrelser — det kan gøres med de layouts .NET MAUI stiller til rådighed.
>
> Vil man eksplicit ændre dele af en side afhængigt af orientering, er den bedste tilgang
> `OnSizeAllocated`-metoden nævnt i kapitel 7. Man overrider metoden og sammenligner de
> indkomne højde- og breddeværdier for at bestemme orienteringen.
>
> Hvad man gør derfra er op til én selv: skjule eller vise forskellige views, ændre
> skriftstørrelser osv. En anden tilgang er at sætte værdien af en enum og binde properties
> i XAML til den, så de kan reagere på orienteringsændringer. Fordelen er, at det også
> virker ved window resizing. Anvendt på MauiCalc-appen fra kapitel 5 kunne man vise et
> scientific layout på mobil i landscape og på desktop når vinduet er bredere end højt
> (eller bredere end en minimumsbredde), og et basic mode-layout på mobil i portrait eller
> på desktop med et højere frem for bredere vindue.

### 10.2.1 Triggers

I `InputViewModel` i MauiStockTake findes en valideringskontrol i `AddCount`-metoden, som
sikrer at brugeren har valgt et produkt før en optælling kan indsendes. Det er en god
kontrol, der forhindrer at gemme dårlige data, men to ting kan forbedres: (1) tilføje en
kontrol der forhindrer indsendelse af en optælling på nul, og (2) i stedet for at vente på
at brugeren indsender dårlige data og så vise en dialog, kan submit-knappen simpelthen
deaktiveres indtil data er i orden. Begge dele opnås med **triggers**.

I .NET MAUI lader triggers dig definere i XAML, hvordan UI'et skal reagere på events eller
state-ændringer. Triggers bruger `Setter` til at definere property og værdi, præcis som i
en style. Der findes flere slags triggers, men de to mest anvendte er:

- **Property triggers** — lader dig definere en ændring i udseendet af en control, når en
  af kontrollens egne properties opfylder en bestemt betingelse. En property trigger ville
  passe godt til det tidligere nævnte `Switch`-eksempel.
- **Data triggers** — lader dig definere en ændring i udseendet af en control, når en
  property på en hvilken som helst control i viewet, eller en property i binding context,
  har en bestemt værdi. Data triggers bruges til UX-ændringerne i MauiStockTake.

Det vigtige at bemærke er, at disse triggers **anvender ændringen når betingelsen er
opfyldt og ruller den tilbage, når betingelsen ikke længere er opfyldt**.

> **NOTE** Disse triggers adskiller sig fra **event triggers**, som *ikke* ruller ændringen
> tilbage. Læs mere om hele spektret af triggers og deres forskelle i dokumentationen:
> http://mng.bz/GyDR.

Først fjernes den eksisterende kontrol: i `InputViewModel`s `AddCount`-metode slettes
`if`-blokken, der tjekker om `SelectedProduct` er null. Derefter tilføjes to data triggers
i `InputPage.xaml`.

Både `Stepper` og `Button` udvides, så de ikke længere er selvlukkende tags, og for hver
deklareres dens `Triggers`-collection. Der tilføjes en data trigger med en `Setter`, som
sætter `IsEnabled` til `False`, til begge.

Med en trigger skal man — ligesom med en style — definere en target type, så den er
forskellig for hver. Man skal desuden angive en **binding** (kildedata som data triggeren
er bundet til) og en **value** (betingelsen som den bundne property skal opfylde for at
aktivere triggeren). For `Stepper` er det ViewModellens `SelectedProduct`-property, for
`Button` er det `Count`-property'en.

`Count` er allerede bundet til UI'et (en `Label` viser dens værdi), så den har allerede et
eksplicit backing field, og dens setter kalder `OnPropertyChanged`. Det samme skal gøres
for `SelectedProduct`.

**Listing 10.10 The updated SelectedProduct declaration**

```csharp
private ProductDto _selectedProduct;
public ProductDto SelectedProduct
{
get => _selectedProduct;
set
{
_selectedProduct = value;
OnPropertyChanged();

①
②

③

}
}
```

① Tilføjer et backing field til `SelectedProduct`
② Udvider property'en til at definere getter og setter
③ Kalder i setteren `OnPropertyChanged`-metoden for at opdatere UI'et

Nu hvor der findes en property, der rejser et property changed-event, kan UI'et bindes til
den. Data triggeren på `Stepper` sætter `IsEnabled` til false og bindes til
`SelectedProduct`, så den aktiveres når værdien er null. Dermed er `Stepper` deaktiveret
indtil et produkt er valgt.

Fordi der bindes til en værdi, der kan være null (og altid vil være det når bindingen
sættes), bruges en **binding fallback** til at sætte startværdien når den bundne property
er null. Binding fallbacks er beskrevet i dokumentationen (http://mng.bz/zXzX), men er ikke
essentielle her.

**Listing 10.11 The updated Stepper code in InputPage.xaml**

```xml
<Stepper Grid.Row="3"
HorizontalOptions="Center"
VerticalOptions="Center"
Value="{Binding Count}">
<Stepper.Triggers>

①

<DataTrigger TargetType="Stepper"
②
Binding="{Binding SelectedProduct,

③
④
⑤
⑥

➥ TargetNullValue=''}"
Value="">
<Setter Property="IsEnabled"
Value="False" />
</DataTrigger>
</Stepper.Triggers>
</Stepper>
```

① Deklarerer `Stepper`ens `Triggers`-collection
② Tilføjer en `DataTrigger` med `TargetType` `Stepper`
③ Binder `DataTrigger`en til `SelectedProduct`-property'en i binding context og giver den
en `TargetNullValue` binding fallback på `' '` (ingenting)
④ Sætter data triggerens value-betingelse til `" "` (ingenting)
⑤ Tilføjer en `Setter` til `DataTrigger`en for `IsEnabled`-property'en
⑥ Sætter `Value` for setterens property til `False`

Kører man appen nu, er `Stepper` på `InputPage` deaktiveret, indtil man søger et produkt og
vælger et fra listen.

Data triggeren til `Button` er næsten identisk; forskellene er target type, binding og
value. Target type bliver `Button` i stedet for `Stepper`, bindingen bliver `Count`, og
fordi `Count` er af typen `int` med default-værdien 0, bliver værdien i data triggeren 0.

**Listing 10.12 The updated Button code in InputPage.xaml**

```xml
<Button Grid.Row="4"
Text="Add count"
Command="{Binding AddCountCommand}">
<Button.Triggers>
<DataTrigger TargetType="Button"
Binding="{Binding Count}"

①

Value="0">
<Setter Property="IsEnabled"
Value="False"/>
</DataTrigger>
</Button.Triggers>
</Button>
```

① Bindingen til `Button`ens data trigger er simpel og binder blot til `Count`-property'en.
② Værdien af `Count` skal være 0 for at aktivere denne trigger.

Med disse ændringer er `Stepper` deaktiveret indtil et produkt er valgt, og `Button` til at
tilføje optællingen er deaktiveret indtil `Count` er et andet tal end 0 — og dermed også
indtil et produkt er valgt (et produkt *skal* være valgt for at kunne ændre `Count` og
aktivere knappen).

Triggers kan mere end at aktivere og deaktivere controls. Fordi de bruger `Setter`s ligesom
en style, kan de ændre ethvert aspekt af en control eller et view. Og ligesom en style kan
en trigger have flere `Setter`s og dermed angive en betingelse, der ændrer flere aspekter
af en control.

---

### 10.2.2 Visual state manager

Triggers er en god måde at ændre udseendet af controls dynamisk som respons på events eller
dataændringer, og man kan kombinere `Setter`s til at tilpasse en control ud fra forskellige
betingelser. Nogle gange kan man logisk gruppere `Setter`s i en trigger for at definere
udseendet under et bestemt sæt betingelser — men i det tilfælde er det mere effektivt at
definere en **visual state**.

Visual states håndteres i .NET MAUI af **visual state manager (VSM)**, et indbygget værktøj
der automatisk ændrer udseendet af et view som respons på den visual state, viewet befinder
sig i. Der er fem indbyggede **common states**:

- `Normal`
- `Disabled`
- `Focused`
- `Selected`
- `PointerOver`

Disse er allerede set i praksis: normal er hvordan controls ser ud det meste af tiden, og
disabled er hvordan en control ser ud når `IsEnabled` er sat til false (som i forrige
afsnit). Selected-tilstanden er set brugt i `CollectionView` flere gange, bl.a. i MauiTodo
og i produktsøgeresultaterne i MauiStockTake.

Selected-tilstanden for produktsøgeresultaternes `CollectionView` er en visuel anomali:
den passer ikke til Mildreds brand guidelines og ser malplaceret ud i alle fire varianter
af appens temaer. **Figur 10.13** viser den nuværende selected state: baggrunden på et
valgt element bliver en orange nuance. Den høje kontrast gør den let at skelne, men farven
harmonerer ikke med appens tema eller brand.

Selvom triggers er alsidige, giver det i det mindste en logisk udfordring at bruge dem til
at ændre udseendet af det valgte element i en collection — VSM gør problemet langt lettere.

VSM anvendes i en `Style`, hvor VSM bruges som setterens property frem for en property på
en control. Fordi det er indlejret i en `Style`, gælder samme hierarki som i afsnit 10.1.1.
Det er baggrundsfarven på `VerticalStackLayout` (det layout der bruges i
`CollectionView`ens data template) der skal ændres, så scopet skal sættes derefter.

I `InputPage.xaml` tilføjes en resource dictionary på den `CollectionView`, der viser
produktsøgeresultaterne, og heri en style med `TargetType="VerticalStackLayout"`. I stylen
tilføjes en `Setter` med property sat til `VisualStateManager.VisualStateGroups`.

**Visual state groups** bruges til at organisere sæt af visual states i VSM. .NET MAUI
leveres med en indbygget visual state group ved navn `CommonStates`, som definerer de fem
ovennævnte states. **Visual states i en gruppe er gensidigt udelukkende** — kun én visual
state i en gruppe kan være aktiv ad gangen. Et view kan fx ikke være både `Normal` og
`Selected`.

Visual states i *forskellige* grupper kan derimod være aktive samtidigt, så man kan
definere flere visual state groups efter behov. Custom visual states er beskrevet i
dokumentationen: http://mng.bz/0KBl.

Inde i `Setter`en i stylen deklareres først visual state group-listen. Kun `CommonStates`
findes (der er ikke defineret custom states), så den indlejres i listen. I gruppen defineres
`Normal` og `Selected`. For `Normal` får VSM besked på ikke at gøre noget, fordi VSM — i
modsætning til en trigger — **ikke ruller ændringerne tilbage**, når staten ikke længere er
aktiv. For `Selected` tilføjes en `Setter` for `BackgroundColor` sat til `Secondary`. Bemærk
at denne `Setter` er indlejret i en `Style` der targeter `VerticalStackLayout`, så de
tilgængelige properties er dem på den angivne target type.

**Listing 10.13 The CollectionView in InputPage.xaml with VSM**

```xml
<CollectionView ...>
<CollectionView.Resources>
<Style TargetType="VerticalStackLayout">
<Setter Property="VisualStateManager.
➥ VisualStateGroups">
<VisualStateGroupList>
<VisualStateGroup x:Name=
➥ "CommonStates">
<VisualState x:Name=”Normal" />
<VisualState x:Name="Selected">

①
②
③
④
⑤
⑥
⑦
⑧

<VisualState.Setters>
<Setter Property="BackgroundColor"
Value="{
➥ StaticResource Secondary}"/>
</VisualState.Setters>

⑨

</VisualState>
</VisualStateGroup>
</VisualStateGroupList>
</Setter>
</Style>
</CollectionView.Resources>
<CollectionView.ItemsLayout>
...
</CollectionView.ItemsLayout>
<CollectionView.ItemTemplate>
...
</CollectionView.ItemTemplate>
</CollectionView>
```

① Definerer `CollectionView`ens resources
② Tilføjer en `Style`-ressource til `CollectionView` med target type `VerticalStackLayout`
③ Tilføjer en `Setter` til stylen med VSM's Visual State Groups som property
④ Definerer Visual State Group-listen
⑤ Definerer `CommonStates` Visual State Group
⑥ Tilføjer `Normal` Visual State, men angiver ingen `Setter`s
⑦ Definerer `Selected` Visual State
⑧ Definerer visual statens `Setters`-collection
⑨ Tilføjer en `Setter` for `BackgroundColor` på target typen og sætter den til `Secondary`
static resource

**Figur 10.14** viser resultatet for default-temaet i light mode: VSM er brugt til at
opdatere stylen for `VerticalStackLayout` — det layout der bruges i data templaten på
produktsøgeresultaternes `CollectionView`. For `Selected`-tilstanden sættes
`BackgroundColor` til `Secondary` static resource, hvilket giver et konsistent look frem
for defaulten. Man ser den relevante `Secondary`-farve for det tema man kører.

Fordi VSM bruger styles, kan `InputPage` ryddes op ved at flytte stylen ud af
`CollectionView`ens resources og ind i tema-filerne. Den skal ikke gælde for *alle*
`VerticalStackLayout`s, så den gøres til en explicit style med en key — hvilket samtidig
giver den fordel, at den kan genbruges i andre `CollectionView`s i appen.

Hele `<Style...>...</Style>` klippes ud af `CollectionView`ens resources og indsættes i
hver af tema-filerne (gerne efter gradient-brushen). Den får key'en `ProductSelector`.

I modsætning til triggers rulles ændringer anvendt ved indtræden i en visual state **ikke**
tilbage, når betingelserne ikke længere er opfyldt. Derfor skal `Normal`-tilstanden
defineres ud over enhver state man vil tilpasse. Vil man ikke tilpasse `Normal` (altså
bevare kontrollens standardudseende), angiver man blot `Normal`-staten uden setters.

**Listing 10.14 The VerticalStackLayout VSM Style in the theme files**

```xml
<Style TargetType="VerticalStackLayout" x:Key=
➥ "ProductSelector">

①

<Setter Property="VisualStateManager.VisualStateGroups">
<VisualStateGroupList>
<VisualStateGroup x:Name="CommonStates">
<VisualState x:Name="Normal" />
<VisualState x:Name="Selected">
<VisualState.Setters>
<Setter Property="BackgroundColor"
Value="{StaticResource Secondary}"/>
</VisualState.Setters>
</VisualState>
</VisualStateGroup>
</VisualStateGroupList>
</Setter>
</Style>
```

① Tilføjer `ProductSelector`-key'en til stylen kopieret fra `CollectionView`

Derefter fjernes `<CollectionView.Resources>...</CollectionView.Resources>` og alt derimellem
fra `InputPage.xaml`, og den explicit style tildeles til `VerticalStackLayout` i
`CollectionView`ens data template.

**Listing 10.15 The final CollectionView in InputPage.xaml**

```xml
<CollectionView ...>
<CollectionView.ItemsLayout>
...
</CollectionView.ItemsLayout>
<CollectionView.ItemTemplate>
<DataTemplate>
<VerticalStackLayout Style="{

①

➥ StaticResource ProductSelector}">
...
</VerticalStackLayout>
</DataTemplate>
</CollectionView.ItemTemplate>
</CollectionView>
```

① Resources er fjernet fra `CollectionView`.
② Tilføjer den explicit style til `VerticalStackLayout`. Stylen bruger VSM.

Ved at flytte stylen og gøre den explicit er UI-koden ryddet op og gjort genbrugelig,
hvilket hjælper med at fastholde et konsistent look and feel gennem hele appen.

---

## 10.3 Multiplatform Apps

.NET MAUI-apps kan køre på et stort spektrum af enheder: laptops og desktops, telefoner,
tablets, ure og TV. Nogle gange er en app målrettet én kategori, men ofte skal den fungere
på tværs af mange **device idioms**.

Forskellige device idioms har forskellige UX-paradigmer. Touch- og swipe-gestures er
allestedsnærværende på mobil, og selvom man ofte kan opnå det samme med et museklik og
træk, er det ikke nødvendigvis lige så implicit opdageligt for desktopbrugere, som
forventer at interagere med appen på andre måder.

Der findes flere tilgange til at give forskellig UX for forskellige idioms:

- **`DeviceInfo.Current.Idiom`** — henter det aktuelle idiom i kode. Returnerer en custom
  struct, der reelt er en enum (og kan behandles som sådan) med værdierne `Phone`, `Tablet`,
  `Desktop`, `TV`, `Watch` eller `Unknown`. Output kan bruges til idiom-afhængig logik eller
  UI-manipulation. Brady Stroud har et eksempel, der bruger idiom til at loade et andet view
  på desktop end på alle andre platforme: https://github.com/bradystroud/MauiMail.
- **Compiler directives** — understøtter ikke forskellige idioms, men lader dig angive en
  platform (bemærk at `DeviceInfo.Current.Platform` giver platformen). Se David Ortinaus
  WeatherTwentyOne-sample: https://github.com/davidortinau/WeatherTwentyOne. Denne tilgang
  giver ikke bare en mere granulær oplevelse, men forhindrer også kode, der kun er beregnet
  til én platform, i at blive kompileret til andre. Det er nyttigt for apps som Verinote
  (nævnt i kapitel 1), hvor funktionalitet på desktop ikke skal være tilgængelig på mobil —
  en person, der forsøger at omgå sikkerhedsforanstaltninger, kan ikke tilgå funktionalitet,
  der simpelthen ikke er der.

Afsnittet fokuserer på layout og på, hvordan man tilpasser appens udseende til forskellige
platforme i XAML.

### 10.3.1 Adding the report page

Når inventaret hentes fra API'et, er der mange informationer, som kan være nyttige for en
desktopbruger, men som ville overvælde en lille skærm. Skærmpladsen skal udnyttes optimalt
per platform, så rapportsiden skal opføre sig forskelligt på forskellige device idioms.

#### Adding the ReportViewModel

MVVM-mønstret følges fortsat, så funktionaliteten lægges i en ViewModel. Ved at følge
ViewModel first-tilgangen (sidebaren "View First vs. ViewModel First" i kapitel 9) kan
funktionaliteten bygges — og endda testes — før UI'et overhovedet tilføjes.

I `ViewModels`-mappen tilføjes klassen `ReportViewModel`, som nedarver `BaseViewModel`. Den
skal have en `ObservableCollection` af typen `InventoryItemDto` at binde UI'et til, og
`IInventoryService` injiceres i constructoren, så API'et kan kaldes for det aktuelle
inventar.

En `Init`-metode kaldes fra `ReportPage`s `OnAppearing`, og en `Refresh`-metode kalder
`IInventoryService`, rydder og genudfylder `ObservableCollection` og sætter
`IsLoading`-property'en fra `BaseViewModel` efter behov.

I `Init` tjekkes et flag `initialized` med default-værdi false: er det true, returneres
blot; ellers kaldes `Refresh`. Dermed kan `Refresh` genbruges senere i afsnittet og
eventuelt til pull-to-refresh. Teknikken kan være nyttig for andre sider, så
`initialized`-boolen tilføjes til `BaseViewModel` og markeres `protected` (så den kan tilgås
fra afledte typer).

**Listing 10.16 ReportViewModel.cs**

```csharp
using System.Collections.ObjectModel;
using MauiStockTake.Shared.Inventory.Queries;
namespace MauiStockTake.UI.ViewModels;
public class ReportViewModel : BaseViewModel
{
private readonly IInventoryService _inventoryService;
public ObservableCollection<InventoryItemDto> Inventory { get; set; }
➥ = new();
public ReportViewModel(IInventoryService inventoryService)
{
_inventoryService = inventoryService;
IsLoading = true;
}
public async Task Init()
{
if (initialized)
return;
initialised = true;
await Refresh();
}
private async Task Refresh()
{
IsLoading = true;
Inventory.Clear();
var inventory = await _inventoryService.GetInventory();
foreach (var item in inventory)
{
Inventory.Add(item);
}
IsLoading = false;
}
}
```

ViewModellen registreres i service collection, så den kan injiceres i rapportsiden. I
`MauiProgram.cs`, efter linjen der registrerer `InputViewModel`:

```csharp
builder.Services.AddTransient<ReportViewModel>();
```

#### Adding the ReportPage UI

`ReportPage` findes allerede i MauiStockTake, men er tom. Før UI'et tilføjes, tilføjes et
felt til `ReportViewModel` i code-behind; i constructoren injiceres ViewModellen, sidens
`Navigation`-property tildeles ViewModellens `Navigation`-property, og ViewModellen sættes
som binding context. Derefter overrides `OnAppearing`, gøres async, og ViewModellens
`Init` kaldes.

**Listing 10.17 ReportPage.xaml.cs**

```csharp
namespace MauiStockTake.UI.Pages;
public partial class ReportPage : ContentPage
{
private readonly ReportViewModel _viewModel;
public ReportPage(ReportViewModel viewModel)
{
InitializeComponent();
viewModel.Navigation = Navigation;
_viewModel = viewModel;
BindingContext = _viewModel;
}
protected override async void OnAppearing()
{
base.OnAppearing();
await _viewModel.Init();
}
}
```

Fordi `ReportPage` ikke længere har en default-constructor, skal den registreres til
dependency injection. I `MauiProgram.cs`, efter linjen der registrerer `InputPage`:

```csharp
builder.Services.AddTransient<ReportPage>();
```

Hovedlayoutet på siden er en `CollectionView` bundet til ViewModellens
`Inventory`-property, pakket ind i et `Grid`, så en `ActivityIndicator` bundet til den
nedarvede `IsLoading`-property kan lægges ovenpå. `CollectionView`ens `ItemsLayout` sættes
eksplicit til `LinearItemsLayout`, så `ItemSpacing` kan sættes til 30 og give lidt luft
mellem elementerne. I `DataTemplate` bruges en `Border` og `Shadow` til at give en
kort-lignende effekt, og et `Grid` lægger produktnavn og aktuelt antal ud.
`AppThemeBinding` bruges til farverne på kortet, så de virker med begge temaer i både light
og dark mode.

**Listing 10.18 ReportPage.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.Pages.ReportPage"
Title="ReportPage">
<Grid>
<ActivityIndicator HorizontalOptions="Center"
VerticalOptions="Center"
IsEnabled="True"
IsRunning="True"
IsVisible="{Binding IsLoading}"/>
<CollectionView HorizontalOptions="Center"
Margin="30"
ItemsSource="{Binding Inventory}">
<CollectionView.ItemsLayout>
<LinearItemsLayout ItemSpacing="30"
Orientation="Vertical"/>
</CollectionView.ItemsLayout>
<CollectionView.ItemTemplate>

<DataTemplate>
<Border StrokeShape="RoundRectangle 10"
Stroke="Transparent"
BackgroundColor="{AppThemeBinding Light={
➥ StaticResource PrimaryBackground}, Dark={StaticResource
➥ PrimaryDarkBackground}}">
<Grid ColumnDefinitions="4*, *"
Margin="20">
<Label Grid.Column="0"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"
FontSize="24"
Text="{Binding ProductName}"/>
<Label Grid.Column="1"
FontSize="24"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"
HorizontalTextAlignment="Center"
Text="{Binding Count}"/>
</Grid>
<Border.Shadow>
<Shadow Brush="{AppThemeBinding Light={StaticResource
SecondaryBackground}, Dark={StaticResource
➥ SecondaryDarkBackground}}"
Offset="-5,-5"
Radius="10"
Opacity="0.8"/>
</Border.Shadow>
</Border>
</DataTemplate>
</CollectionView.ItemTemplate>
</CollectionView>
</Grid>
</ContentPage>
```

Der er intet nyt her — lignende card views er set i andre apps, og `AppThemeBinding` blev
gennemgået tidligere i kapitlet. Det giver et basalt layout, der virker godt på mobil.

**Figur 10.15** viser `ReportPage` på Android: kortlayoutet fungerer på en lille skærm og
præsenterer kun det minimalt nødvendige, nemlig produktnavn og aktuelt lagerantal.

Layoutet fungerer på en telefon, men har to problemer på desktop (eller tablet): for det
første ser det forfærdeligt ud, og for det andet er den minimale mængde information, der er
skåret ned til for mobil, ikke en effektiv udnyttelse af skærmpladsen.

---

### 10.3.2 Multiplatform layouts

`DeviceInfo`-API'et kan give information om den aktuelle platform eller idiom (og faktisk
langt mere). Det er velegnet i C#-kode; i XAML kan man i stedet bruge markup extensions som
**`OnIdiom`**. Med `OnIdiom` kan man angive forskellige værdier for forskellige platforme
direkte i XAML, for enhver property på ethvert view.

**Figur 10.16** viser `OnIdiom` i brug: markup extension'en angiver forskellige værdier
afhængigt af device idiom — i eksemplet tildeles forskellige strenge til en labels
`Text`-property afhængigt af, om appen kører på desktop eller telefon.

Ligesom `DeviceIdiom`-structen lader `OnIdiom` dig angive værdier for `Phone`, `Tablet`,
`Desktop`, `Watch` eller `TV`, men i stedet for `Unknown` har den `Default`. Man behøver
altså ikke angive en værdi for hvert idiom; man kan angive sin standardværdi og kun ændre
den for det idiom, man vil tilpasse.

Først opdateres spacing. I `LinearItemsLayout`-taggen ændres 30 til 10 på desktop, 30 på
telefon og en default på 20:

```xml
<LinearItemsLayout ItemSpacing="{OnIdiom Desktop=10, Phone=30, Default=20}"
```

`Margin` på `Grid` ændres fra 20 til en default på 0 og 20 på telefon:

```xml
Margin="{OnIdiom Phone=20, Default=0}">
```

Dernæst ændres `BackgroundColor` på `Border`, så den kun viser den bundne farve på telefon.
Den nuværende `AppThemeBinding` pakkes ind som værdi for `Phone`, og default sættes til
`Transparent`:

```xml
BackgroundColor="{OnIdiom Phone={AppThemeBinding Light={StaticResource
➥ PrimaryBackground}, Dark={StaticResource PrimaryDarkBackground}},
➥ Default=Transparent}">
```

Det giver et bedre desktop-layout, men ideelt skal informationen vises i tabelform med
flere data. `OnIdiom` bruges til at angive et forskelligt antal kolonner: to på telefon som
hidtil, men fem på desktop, så produktnavn, producentnavn, antal, tælledato og hvem der
talte kan vises.

På telefon (to kolonner) skal anden kolonne vise antallet, men på desktop kan antallet
flyttes længere hen, og producentnavnet vises i anden kolonne — ved at angive en forskellig
binding afhængigt af idiom. De ekstra kolonner tilføjes ved at lægge de nødvendige `Label`s
i de rigtige kolonner og bruge `OnIdiom` til at sætte `IsVisible` afhængigt af, om der er
tale om telefon eller desktop.

Da informationen nu vises som en tabel, tilføjes headers via `CollectionView.Header`-property'en:
et `Grid` med fem kolonner med samme definitioner som desktop-visningen i data templaten.
`OnIdiom` gør hele `Grid`et usynligt på telefon og synligt på desktop. Inde i det `Grid`
tilføjes fem `Label`s, næsten identiske med dem i data templaten — forskellene er, at
`FontAttributes` sættes til bold, og at kolonneoverskrifterne er hardcodede.

Sidste finish til desktop er en data trigger på den `Label`, der viser tælledatoen. Da
værdien er en `DateTime`, har DTO'en default-værdien `DateTime.Minimum`, hvis ingen eksplicit
værdi returneres fra databasen. Det sker, når der ikke er registreret optællinger for det
element, så en data trigger sætter `Text`-property'en til `No stock counted` i det tilfælde.

> **NOTE** Man kunne opnå samme resultat ved at binde data triggeren til `Count`-property'en
> og bruge 0 som værdi. Men denne tilgang demonstrerer data triggers alsidighed.

**Listing 10.19 The final ReportPage.xaml code**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage ...>
<Grid>
<ActivityIndicator .../>
<CollectionView ...>
<CollectionView.ItemsLayout>
<LinearItemsLayout ItemSpacing="{OnIdiom Desktop=10, Phone=

①

➥ 30, Default=20}"
Orientation=”Vertical”/>
</CollectionView.ItemsLayout>
<CollectionView.Header>
<Grid IsVisible="{OnIdiom Phone=False,

②

➥ Desktop=True}"
ColumnDefinitions="2*, 2*, *, 2*, 2*">
<Label Grid.Column="0"
Text="Product"
FontSize="24"
FontAttributes="Bold"/>
<Label Grid.Column="1"
Text="Manufacturer"
FontSize="24"
FontAttributes="Bold"/>
<Label Grid.Column="2"
Text="Count"
FontSize="24"
FontAttributes="Bold"/>
<Label Grid.Column="3"
Text="Counted By"
FontSize="24"
FontAttributes="Bold"/>
<Label Grid.Column="4"
Text="Counted On"
FontSize="24"

FontAttributes="Bold"/>
</Grid>
</CollectionView.Header>
<CollectionView.ItemTemplate>
<DataTemplate>
<Border StrokeShape="RoundRectangle 10"
Stroke="Transparent"
BackgroundColor="{OnIdiom Phone={AppThemeBinding Light=
{StaticResource PrimaryBackground}, Dark={StaticResource PrimaryDarkBackground}},
Default=

③

➥ Transparent}">
<Grid ColumnDefinitions="{
➥ OnIdiom Phone='4*, *', Desktop='2*, 2*, *, 2*, 2*'}"
Margin="{OnIdiom Phone=20, Default=0}">

④
⑤

<Label Grid.Column="0"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"
FontSize="24"
Text="{Binding ProductName}"/>
<Label Grid.Column="1"
FontSize="24"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"
HorizontalText
➥ Alignment="{OnIdiom Phone=Center}"

⑥

Text="{OnIdiom
➥ Phone={Binding Count}, Desktop={Binding ManufacturerName}}"/>

⑦

<Label Grid.Column="2"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"
FontSize="24"
Text="{Binding Count}"
IsVisible="{OnIdiom
➥ Phone=False, Desktop=True}"/>

⑧

<Label Grid.Column="3"
FontSize="24"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"
Text="{Binding CountedByName}"
IsVisible="{OnIdiom Phone=False, Desktop=True}"/>
<Label Grid.Column="4"
FontSize="24"
TextColor="{AppThemeBinding Light={StaticResource
Primary}, Dark={StaticResource PrimaryDark}}"

Text="{Binding CountedAt}"
IsVisible="{OnIdiom Phone=False, Desktop=True}">
<Label.Triggers>
<DataTrigger TargetType="Label"
Binding="{Binding CountedAt}"
Value
➥ ="1/1/0001">

⑨

<Setter Property="Text"
Value="No stock counted"/>
</DataTrigger>
</Label.Triggers>
</Label>
</Grid>
<Border.Shadow>
<Shadow Brush="{AppThemeBinding Light={StaticResource
SecondaryBackground}, Dark={
➥ StaticResource SecondaryDarkBackground}}"
Offset="-5,-5"
Radius="10"
Opacity="0.8"/>
</Border.Shadow>
</Border>
</DataTemplate>
</CollectionView.ItemTemplate>
</CollectionView>
</Grid>
</ContentPage>
```

① Bruger `OnIdiom` til at angive item spacing for forskellige idioms
② Hele header-`Grid`et kan gøres synligt på desktop og skjult på telefon. Dermed kræves
ingen yderligere idiom-specificitet for controls i dette view.
③ `Border`en er der for at give card view på telefon. På desktop er det tabulært, så
baggrunden kan gøres transparent.
④ På telefon har vi kun to kolonner, men på desktop har vi fem.
⑤ `Grid`et har brug for en margin på telefon til card view. På desktop er det en tabelrække,
så marginen er ikke nødvendig.
⑥ På telefon skal count-labelen være centreret. På desktop skal den være venstrejusteret
(default) for at matche header og øvrige kolonner.
⑦ På desktop bliver værdien i anden kolonne producentens navn (da antallet ligger i næste
kolonne). På telefon vises kun produktnavn og antal, så antallet skal ligge i anden kolonne.
⑧ De resterende kolonner er kun synlige på desktop og skjult på telefon.
⑨ En data trigger kan bruges til at give en alternativ værdi, når `DateTime` er sat til
defaulten `DateTime.Minimum`.

**Figur 10.17** viser `ReportPage` kørende på Windows. `OnIdiom` bruges til at ændre måden
informationen vises på, hvilket giver yderligere information og et tabulært layout
sammenlignet med det simple card view på telefoner. Kører man appen igen på Android eller
iOS, ses ingen ændring — den ser ud som i figur 10.15.

Ser man på de eksempler, der blev linket til i starten af afsnittet, kan man se, hvordan
man loader forskellige views for forskellige idioms og platforme. Med tilgangen her kan
displayet i stedet tilpasses forskellige platforme og idioms — alt sammen inde i det samme
view.

> **Exercise**
>
> Der mangler en sidste ændring på rapportsiden: `CollectionView` reagerer ikke på
> temaskift, så den skal have besked om at refreshe når temaet er ændret. Det kan gøres
> med `WeakReferenceMessenger` i MVVM Community Toolkit. Læs om det i dokumentationen
> (http://mng.bz/KedZ) eller i James Montemagnos video (https://youtu.be/vD17OetzGXc).
> Løsningen ligger også i chapter-complete-mappen.

> **WARNING** Kommer man fra Xamarin.Forms, kan man være fristet til at bruge
> `MessagingCenter` til øvelsen; men `MessagingCenter` er deprecated og fjernes fra
> .NET MAUI i .NET 8. Brug `WeakReferenceMessenger` i stedet.

---

### 10.3.3 Features of desktop apps

At levere et desktop-specifikt layout er det vigtigste enkelttiltag for god desktop-UX. Det
giver appen et professionelt præg, der adskiller den fra "lift and shifted" mobil-apps, som
blot er portet til desktop.

Man kan gå videre ved at udnytte de UX-paradigmer, brugere forventer på desktop-OS'er:
grundlæggende **menuer** og **vinduer**.

#### Menus

Menuer tilføjes til pages ved at angive sidens `MenuBarItems`-collection. Fordi menulinjen
kun vises på desktop, behøver man **ikke** bruge `OnIdiom` (eller anden teknik) til at
skjule den på mobil eller andre idioms.

Menuer tilføjes på topniveau ved at angive et `MenuBarItem`, og disse menuer kan bestå af
`MenuFlyoutItem`s, som viser tekst og tilbyder en handling, eller `MenuFlyoutSubItem`s, som
lader dig gruppere `MenuFlyoutItem`s i undermenuer.

I `ReportPage.xaml` tilføjes `ContentPage.MenuBarItems`-taggene før `Grid`et og et
`MenuBarItem` med `Text` sat til `Help`. Heri tilføjes et enkelt `MenuFlyoutItem` med `Text`
sat til `About`. `MenuFlyoutItem` har både et `Clicked`-event og en `Command`-property;
begge kan bruges til at eksekvere en handling, enten via en event handler i code-behind
eller en command i binding context.

**Listing 10.20 The ReportPage.xaml menu**

```xml
<ContentPage.MenuBarItems>
<MenuBarItem Text="Help">
<MenuFlyoutItem Text="About"

①
②
③

Command="{Binding
➥ ShowAboutPageCommand}"/>
</MenuBarItem>
</ContentPage.MenuBarItems>

④
```

① Definerer `ContentPage`ens `MenuBarItems`-collection
② Et `MenuBarItem` er en topniveau-menu (typisk File, Edit osv.).
③ Et `MenuFlyoutItem` er et enkeltniveau-menupunkt med en eksekverbar handling (man kan
tilføje et `MenuFlyoutSubItem` i stedet for at oprette en undermenu).
④ `MenuFlyoutItem` understøtter Commands (kun .NET 7+) og event handlers.

**Figur 10.18** viser `ReportPage` med Help-menuen tilføjet øverst til venstre ved siden af
titellinjen. Menuen har et enkelt menupunkt, About. Klikker man på det nu, sker der intet —
det ordnes i næste afsnit.

#### Windows

Når en .NET MAUI-app startes på Windows eller macOS, oprettes et initielt vindue, og appens
`MainPage` indlæses i dette vindue. I `blankmaui`-templaten blev en content page tildelt
denne property, og i default-templaten er `Shell` tildelt den og bliver dermed indholdet af
appens hovedvindue.

Man kan også oprette **yderligere vinduer** og tildele sine egne pages til dem programmatisk.
I `Pages`-mappen i MauiStockTake tilføjes en ny XAML-page, `AboutPage`. Indholdet skal være
en `VerticalStackLayout` med spacing 30, der indeholder tre `Label`s. Alle tre skal være
centreret og have horizontal text alignment. Første `Label`s tekst er `Welcome to
MauiStockTake`, `v1.0` på den anden og `Copyright Mildred's Surf Shack 2023` på den tredje.
De tildeles skriftstørrelserne `Header`, `Title` og `Large`.

Derefter tilføjes en metode til `ReportViewModel`, der viser vinduet. Der oprettes en ny
instans af typen `Window`, og en `ContentPage` sendes til dens constructor (eller tildeles
direkte til `Page`-property'en). Det er en god idé at sætte `Title` samt `Height` og `Width`.
Til sidst kaldes `Application.Current.OpenWindow` med `Window`-instansen.

**Listing 10.21 The ShowAboutPage method in ReportViewModel**

```csharp
public void ShowAboutPage()
{
var newWindow = new Window(new AboutPage())
{
Title = "About",
Width = 300,
Height = 300
};
Application.Current.OpenWindow(newWindow);
}
```

`ShowAboutPageCommand`-property'en (af typen `ICommand`) tilføjes til `ReportViewModel`:

```csharp
public ICommand ShowAboutPageCommand { get; set; }
```

Og i constructoren tildeles metoden via en ny `Command`-instans:

```csharp
ShowAboutPageCommand = new Command(ShowAboutPage);
```

Vinduer er en fast bestanddel af desktop-apps, men understøttes også på iOS og Android
(afhængigt af systemet). Det ovenstående virker fint på Windows og Android, men der kræves
et par ekstra skridt for at aktivere multiwindowing på macOS og iOS.

Først skal der oprettes en **`SceneDelegate`**. En Scene i iOS og Mac Catalyst er en kørende
instans af appen og er ansvarlig for at håndtere appens vinduer og UI.

**Listing 10.22 The SceneDelegate.cs file**

```csharp
using Foundation;
namespace MauiStockTake.UI.Platforms.[iOS/MacCatalyst];

①

[Register("SceneDelegate")]
public class SceneDelegate : MauiUISceneDelegate
{
}
```

① Fjern de kantede parenteser og behold blot det relevante platform-namespace

Sidste skridt er at opdatere `info.plist` for at erklære, at appen bruger flere scenes.
Nøglen og dictionary'en tilføjes til `info.plist` i både iOS- og MacCatalyst-platform-mapperne.

**Listing 10.23 The multiwindow key to add to info.plist**

```xml
<key>UIApplicationSceneManifest</key>
<dict>
<key>UIApplicationSupportsMultipleScenes</key>
<true/>
<key>UISceneConfigurations</key>
<dict>
<key>UIWindowSceneSessionRoleApplication</key>
<array>
<dict>
<key>UISceneConfigurationName</key>
<string>__MAUI_DEFAULT_SCENE_CONFIGURATION__</string>
<key>UISceneDelegateClassName</key>
<string>SceneDelegate</string>
</dict>
</array>
</dict>
</dict>
```

**Figur 10.19** viser `AboutPage` for MauiStockTake i et nyt vindue. Det er blot en
`ContentPage` som alle andre `ContentPage`s i appen, men den er tildelt `Page`-property'en
på en instans af `Window`-klassen, og instansen kan derefter vises med `OpenWindow`-metoden.

---

## Summary

- Styles er samlinger af `Setter`s, der ændrer værdierne af properties på en control.
- Styles kan være implicit eller explicit. Med implicit styles kan man anvende visuelle
  ændringer på alle instanser af en control i appen. Med explicit styles kan man begrænse
  ændringerne til specifikke instanser.
- Et hierarki bestemmer den endelige fremtoning af en control. App-brede styles anvendes
  først, men page-specifikke styles har forrang. Layout- eller control-specifikke styles har
  højere prioritet end page-styles, og explicit styles overskriver enhver anden style.
  Property-værdier sat direkte på en control overskriver enhver værdi i en style.
- Man kan binde til enhedens light- eller dark mode-indstilling med `AppThemeBinding`. Det
  gør det let at levere light- og dark mode-visninger.
- Man kan bruge `DynamicResource` til at skifte temaer og farver ved runtime.
- Data triggers lader dig reagere på ændringer i appens tilstand ved runtime. Man kan ændre
  enhver property på ethvert layout eller control som respons på en state- eller
  værdiændring hvor som helst i appen.
- `VisualStateManager` lader dig gruppere `Setter`s i en visual state. Visual states samles
  i visual state groups. Fem states kaldet common states er indbygget i .NET MAUI
  (`Normal`, `Selected`, `Disabled`, `Focused` og `PointerOver`), men man kan også definere
  sine egne.
- .NET MAUI-apps kan køre på en række forskellige enheder, herunder laptops og desktops,
  telefoner, TV og ure. Hvilken af disse appen kører på kaldes **device idiom**.
- Der er flere måder at tilpasse appen til forskellige platforme. Man kan bruge compiler
  directives til at styre hvilken kode der kompileres til hvilke platforme, og
  `DeviceInfo`-API'et kan give den aktuelle platform eller idiom ved runtime.
- `OnIdiom`-markup extension'en lader dig definere forskellige views og layouts for
  forskellige platforme, alt sammen inden for den samme XAML-fil.
- Forskellige UX-paradigmer er udbredte på forskellige idioms. Med .NET MAUI kan man have
  menuer og multiwindow i sine apps for at imødekomme desktop-brugere.
