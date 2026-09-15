# Kapitel 3 — Making .NET MAUI apps interactive

## Metadata

- **Kapitel:** 3 — Making .NET MAUI apps interactive
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L02
- **Hovedemner:**
  - App permissions deklareret i platform-specifikke metadata-filer (AndroidManifest.xml, Info.plist, Package.appxmanifest)
  - Adgang til OS- og device-features via .NET MAUI-abstraktioner: Geolocation og Share
  - Runtime permission-håndtering med `Permissions.CheckStatusAsync` / `RequestAsync`
  - Event handlers i code-behind (`Clicked`, delegate-signatur `(object sender, EventArgs e)`)
  - Lokal datapersistering: `Preferences` vs. `SecureStorage`, SQLite + SQLCipher
  - Data binding: source property, target property, `BindableObject`, binding context
  - Binding context nedarves fra parent til child i view-hierarkiet
  - View-to-view bindings med `{x:Reference}` og `{Binding}`
  - `CollectionView`, `ItemsSource`, `ItemTemplate` og `DataTemplate`
  - `ObservableCollection<T>` og automatiske UI-opdateringer ved ændringer i collection

---

## Indledning

I kapitel 2 blev den første .NET MAUI-app bygget og kørt, men app'en gjorde reelt ingenting. Click Me-knappen, der tæller et tal op på skærmen, illustrerer dog kernemekanismen: man kan hente og sætte værdier på UI-elementer fra kode.

At tælle en tæller op er ikke særlig spændende, og det kan man også gøre med en web-app. Derfor udforsker kapitlet de device capabilities, som gør det interessant at bygge mobil- og desktop-applikationer med .NET MAUI.

Kapitlet viser, hvordan **data binding** bruges til at binde værdier og commands fra UI'et til koden, og hvordan klassen bag et view let kan manipulere sit view. Undervejs bygges et par eksempel-apps, som samtidig demonstrerer, hvordan data persisteres lokalt på enheden, og hvordan almindelige device- og OS-features tilgås gennem .NET MAUI-API'erne.

---

## 3.1 Using OS and device features

En af grundene til at vælge en mobil- eller desktop-app frem for en web-app er den lette adgang til device- og OS-features. Nogle af disse features kan tilgås fra web-apps, andre kan ikke — men i .NET MAUI er adgangen simpel.

**Figur 3.1** illustrerer pointen: .NET MAUI leverer abstraktioner over almindelige features (her location og sharing), så man kan tilgå dem på alle platforme fra én fælles kodebase.

.NET MAUI giver adgang til fælles features på tværs af alle understøttede platforme; en udtømmende liste findes i Microsofts dokumentation. Dette afsnit fokuserer på to: **geolocation** og **sharing**. Som demonstration bygges app'en **FindMe!**, der henter brugerens location og lader dem dele den.

**Figur 3.2** viser FindMe!-app'en kørende på Android: brugeren indtaster sit navn og klikker på en knap, hvorefter de præsenteres for flere muligheder for at dele deres location med en ven.

iOS, Windows, macOS og Android har alle et geolocation-API, der kan bruges til at hente information om brugerens position. På desktop approksimeres positionen ud fra flere datapunkter, herunder IP-adresse og Wi-Fi-information; på en mobil enhed kombineres dette med GPS-data for en mere præcis position. Med .NET MAUI behøver man ikke kende de underliggende mekanismer — kun at de tilgås via en fælles abstraktion.

**Figur 3.3** viser lagdelingen: .NET MAUI's geolocation-API er en abstraktion over hver platforms individuelle API, som igen er en abstraktion over forskellige teknikker til fysisk at lokalisere brugeren.

På alle platforme skal brugeren eksplicit give samtykke til, at app'en tilgår deres location. På iOS og Android skal app'en desuden deklarere, hvilke permissions den vil bede om, som en del af sin descriptive metadata. .NET MAUI leverer metoder til både at anmode om samtykke og til at verificere, at brugeren har givet samtykke til location-adgang, mens app'en er i brug (**figur 3.4**). Deklarationen af påkrævede permissions sker i app'ens metadata frem for i kode og skal derfor gøres på en specifik måde for hver platform.

Den anden feature, kapitlet bruger, er **sharing**. Alle fire understøttede OS'er leverer et sharing-API, der — når det aktiveres — beder brugeren om at vælge en target-applikation. Dette kan være email, SMS eller enhver app, der har registreret en sharing-capability hos OS'et (sociale medie-apps eksponerer næsten altid en share-capability). Fordi OS'et har et sharing-API, hvor brugeren selv vælger destinationen, behøver man ikke understøtte hver enkelt delingsmekanisme i hver app på hvert OS; i .NET MAUI bruges én fælles abstraktion, der giver adgang til sharing på alle målplatforme (**figur 3.5**).

> **NOTE** At registrere sin egen app som *sharing target* dækkes ikke her — .NET MAUI leverer ikke et unified API til at registrere et sharing target, så man vil skulle skrive OS-specifik kode for hver platform (dog abstraheret ind i .NET). Bogen berører platform-specifik kode undervejs, med lidt mere dybde i kapitel 11, men kernefokus er delt funktionalitet, der ikke kræver OS-specifik kode. Indtil videre kan man altså dele *fra* sin app, men ikke *til* sin app.

Kom i gang: opret et nyt .NET MAUI-projekt kaldet **FindMe** med `blankmaui`-templaten. Før der skrives UI- eller logikkode, skal det deklareres, hvilke permissions der vil blive bedt om — her adgang til brugerens location.

Hver platform har en specifik metadata-fil, som deklarerer den information om app'en, OS'et og distributionsplatformene (fx iOS App Store og Microsoft Store) har brug for. Afhængigt af målplatformen indeholder denne information enten en deklaration af de permissions, app'en kræver, eller en deklaration af hvilke capabilities (netværksadgang, location osv.) app'en bruger, hvorudfra de nødvendige permissions udledes.

**Tabel 3.1 — Navne og placering af app-metadatafiler pr. platform**

| Platform | Filnavn | Placering |
| --- | --- | --- |
| Android | `AndroidManifest.xml` | `[Your app]/Platforms/Android/` |
| iOS | `Info.plist` | `[Your app]/Platforms/iOS/` |
| macOS | `Info.plist` | `[Your app]/Platforms/macOS/` |
| Windows | `package.appxmanifest` | `[Your app]/Platforms/Windows/` |

**Figur 3.6** viser de samme metadata-filer i projektstrukturen. Mønstret er let at få øje på: alle filerne ligger i en mappe under `Platforms`, navngivet efter hver platform. Denne mappe indeholder også andre platform-specifikke filer.

Metadata-filerne er alle XML, så de kan let redigeres i en vilkårlig teksteditor. Afhængigt af IDE kan man have adgang til grafiske editorer for velkendte værdier i filerne, men den letteste vej er at redigere XML'en direkte — og det anbefales at blive fortrolig med den fremgangsmåde.

> **TIP** I Visual Studio kan man højreklikke på manifest-filerne, vælge Open With og derefter XML (Text) Editor i dialogen.

> **Where can I learn about platform-specific metadata files?**
> .NET MAUI-dokumentationen indeholder information om de platform-specifikke metadata-filer og er et godt sted at starte. Som .NET MAUI-udvikler vil man før eller siden få brug for at grave ned i platform-specifikke detaljer — ikke kun for metadata-filerne. Bogen berører platform-specifikt stof hist og her og går lidt mere i dybden i kapitel 8, men det vigtige er at kunne finde information selv om platform-specifikke API'er og implementeringsdetaljer og oversætte dem til .NET MAUI-scenarier. Målet er at være dækkende frem for udtømmende og at gøre læseren selvhjulpen. "Other Online Resources" forrest i bogen er et godt udgangspunkt.

---

## 3.1.1 Android metadata

Åbn `FindMe > Platforms > Android > AndroidManifest.xml`. Inde i `<manifest>...</manifest>`-taggene er der tre child-elementer: `uses-sdk`, `application` og `uses-permission`.

- `uses-sdk` deklarerer, hvilken minimum Android API-version der kræves for at køre app'en (fx hvis man vil bruge Android-features introduceret i en bestemt version — der er også et minimumskrav for at kunne submitte apps til Google Play), samt target-versionen.
- `application` indeholder metadata om, hvordan app'en præsenteres og håndteres af Android-OS'et. Her deklareres hvor app-ikonet findes (både standard og round icon), om right-to-left understøttes (til sprog som arabisk eller hebraisk), og om app'en skal inkluderes i en device-backup (man kan fx udelade den, hvis app'en indeholder følsomme data).
- `uses-permission` er det mest interessante her. Det deklarerer, at app'en vil bede om brugerens tilladelse til at tilgå enhedens netværksstatus-information, angivet med det Android API-specifikke namespace.

Først deklareres de permissions, brugeren skal spørges om. Følgende listing viser de permissions, der skal deklareres (de tilføjede står i fed i bogen).

**Listing 3.1 Permissions to add to AndroidManifest.xml**

```xml
<uses-permission android:name="android.permission.
➥ ACCESS_NETWORK_STATE" />

<uses-permission android:name="android.permission.
➥ ACCESS_COARSE_LOCATION" />

<uses-permission android:name="android.permission.
➥ ACCESS_FINE_LOCATION" />
```

① Beder brugeren om tilladelse til at tilgå netværksstatus
② Beder om adgang til brugerens *coarse location* (position approksimeret ud fra tilgængelig information)
③ Beder om adgang til brugerens *fine location* (evalueret mere præcist ud fra alle tilgængelige data, inkl. GPS)

Ud over at deklarere de permissions, der spørges om, skal de Android API-features, app'en bruger, også deklareres. Det specificeres samtidig, at app'en godt nok bruger disse features, men at de ikke er påkrævede for at installere app'en. Tilføj følgende features efter permissions.

**Listing 3.2 Features to add to AndroidManifest.xml**

```xml
<uses-feature android:name="android.hardware.location"
➥ android:required="false" />
<uses-feature android:name="android.hardware.location.
➥ Gps" android:required="false" />
```

① Bruger location-featuren (approksimeret med Wi-Fi, IP-adresse osv.), men blokerer ikke app'en, hvis den ikke er tilgængelig
② Bruger GPS til at forbedre location-nøjagtigheden, men blokerer ikke app'en, hvis den ikke er tilgængelig

Når disse er tilføjet, skal `AndroidManifest.xml` se således ud.

**Listing 3.3 AndroidManifest.xml full code**

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
<uses-sdk android:minSdkVersion="21" android:targetSdkVersion="31" />
<application android:allowBackup="true" android:icon="@mipmap/appicon"
➥ android:roundIcon="@mipmap/appicon_round" android:supportsRtl="true">
➥ </application>
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-feature android:name="android.hardware.location" android:required
➥ ="false" />
<uses-feature android:name="android.hardware.location.gps" android:
➥ required="false" />
</manifest>
```

Det er alle de ændringer, der skal laves i Android-manifestet.

---

## 3.1.2 iOS metadata

Åbn iOS-versionen af `Info.plist`. *Plist* er kort for *property list* (Apples term i stedet for manifest), og listen af properties er indkapslet i en dictionary af key-value-par kaldet `<dict>...</dict>`.

Processen for at deklarere behovet for brugerens location er lidt anderledes på iOS: i stedet for at deklarere både permission og feature, deklarerer man *årsagen* til, at man har brug for en permission. Når det er gjort, er kravene til både feature og permission implicitte. Tilføj key-value-parret fra listing 3.4 til slutningen af dictionaryen i `Info.plist`.

**Listing 3.4 Key-value pair to add to the iOS Info.plist file**

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>We need your location in order to share it.</string>
```

① Nøglen for dette element i dictionaryen, med den genkendte key der beskriver, hvorfor vi har brug for brugerens location, mens app'en er i brug
② Værdien i dictionaryen for denne key, angivet som en string med den begrundelse, vi har givet

Luk og gem filen, og lav derefter den samme ændring i **Mac Catalyst**-versionen af `Info.plist`.

> **Why is the Mac platform called Mac Catalyst?**
> En betydelig del af Apples omsætning kommer fra app-salg, hvoraf det meste sker i iOS App Store. For at øge variationen og volumen af apps i Mac App Store introducerede Apple Catalyst-programmet, der gør det muligt for iOS-udviklere at pakke deres apps til Mac. Fra 2021 markeres alle apps submitted til iOS App Store som standard også til release i Mac App Store — udviklere skal aktivt fravælge det. Mange udviklere sætter pris på bekvemmeligheden.
>
> .NET MAUI udnytter Mac Catalyst-programmet og bruger Catalyst-platformen til at pakke apps til Mac. Resultatet for udviklere og brugere er det samme, som hvis app'en var skrevet specifikt til Mac, men under motorhjelmen betyder det, at .NET MAUI kan levere en abstraktion over iOS' UI SDK (UIKit) i stedet for også at skulle levere en abstraktion over macOS' UI SDK (AppKit). Situationen kan ændre sig i fremtiden, hvis Apple forener SDK'erne; indtil da giver Catalyst en bekvem måde for .NET MAUI at pakke apps til Mac.

---

## 3.1.3 Windows metadata

Den sidste platform, der skal specificeres permissions for, er Windows. Åbn `Package.appxmanifest` i Windows-platformmappen. Det er endnu en XML-fil ligesom på de andre platforme. Mod bunden af filen findes en `Capabilities`-node. Inde i denne node, under eventuelle eksisterende capabilities, tilføjes følgende linje:

```xml
<DeviceCapability Name="location" />
```

Det er alt, der skal til for at fortælle Windows-apps, at vi vil bede om brugerens tilladelse. Platform-opsætningen er dermed komplet.

---

## 3.2 The FindMe! UI

Åbn `MainPage.xaml`, som indeholder UI-definitionen for siden. Der skal laves nogle få ændringer for at tilpasse UI'et til FindMe:

1. Det andet element i `VerticalStackLayout` er en `Label` med `Text` sat til "Hello, world!". Ændr teksten til **"Find me!"**.
2. Den næste `Label` fungerer som undertitel (selvom den semantisk er sat til samme title level) med teksten "Welcome to the dot net Multi Platform App UI." Ændr teksten til **"Enter your name, then click the button to share your location."** Bemærk at teksten også er indtastet i en property kaldet `SemanticProperties.Description`; ændr værdien her, så den matcher det, der blev indtastet i `Text`-propertyen.

> **Semantic properties**
> Semantic properties er beskrivende værdier, som assistive technologies såsom skærmlæsere bruger til at hjælpe folk med at bruge app'en. Fx kan en synshandicappet person ikke nødvendigvis skelne farver, så typiske konventioner som en rød label til at indikere fejl eller et grønt flueben til at indikere succes vil ikke virke for alle brugere. Farve og især kontrast kan bruges til at forbedre læsbarheden, men man kan ikke basere sig på én enkelt teknik.
>
> Accessibility (eller *a11y*, som det ofte forkortes) er et stort emne, som kræver investering af tid. Microsoft leverer en række ressourcer på området (nævnt kort i kapitel 2), inklusive dokumentation og videoer, og meget af det er specifikt for .NET MAUI. Et godt sted at starte er Accessibility-siden i Fundamentals-sektionen af den officielle .NET MAUI-dokumentation. Det er essentielt at blive fortrolig med dette.

Det næste UI-element er en `Button`. Før den tilføjes en `Entry`-kontrol med følgende snippet:

```xml
<Entry
Placeholder="Enter your name"
SemanticProperties.Hint="Enter your name to be used when sharing location"
HorizontalOptions="Center"
x:Name="UsernameEntry"/>
```

① Elementtypen er `Entry` frem for `Label`. En `Entry` er et enkeltlinjes brugerinput-felt.
② I stedet for `Text`-propertyen, som blev sat på `Label`, sættes en `Placeholder`-værdi (`Entry` har også en `Text`-property, som bruges senere i kapitlet). Værdien vises, når brugeren endnu ikke har indtastet sin egen værdi.
③ UI-elementet får et navn, så det kan refereres fra C#-koden. Der findes ingen Microsoft style guide for XAML-navnekonventioner, men man bør etablere sin egen.

> **.NET MAUI naming conventions**
> Microsoft leverer coding convention-retningslinjer for hvert af sine .NET-sprog, og der findes XAML-syntakskonventioner for Universal Windows Platform (UWP), men ikke for .NET MAUI (eller Xamarin.Forms). Alligevel er nogle best practices blevet de facto-standarder i branchen. Man bør gøre sig fortrolig med dem, men i sidste ende må man selv og/eller sit team definere den bedste tilgang for produktet.
>
> I bogen bruges **PascalCase** til alle public properties og **underscored `_camelCase`** til private fields. For XAML-properties bruges PascalCase med konventionen `[property or action][ElementType]`: fx `LoginButton` eller `UsernameEntry`. Konventionerne gør det lettere for enhver, der gennemgår koden — inklusive én selv — at forstå, hvad en property er til, og hvad den gør.

Ændr værdien af `Clicked`-propertyen på `Button`-elementet. Den refererer i øjeblikket til en metode i code-behind-filen kaldet `OnCounterClicked`; ændr den til `OnFindMeClicked`. Metoden findes ikke endnu, men oprettes om lidt. Semantic hint skal også opdateres; ændr `SemanticProperties.Hint`-værdien til "Presents apps available to share your name and location via."

Til sidst kan Dotnet Bot erstattes med et billede, der passer bedre til scenariet — fx noget der ligner det almindelige location pin-ikon eller en globus. Tilføj billedet til `Resources/Images`-mappen på samme måde som i kapitel 2, og opdatér `Source`-propertyen på `Image`-elementet i UI-griddet til det nye filnavn.

Med UI-ændringerne på plads skal logikken bygges. Når brugeren klikker på find me-knappen, kaldes en **event handler**. Denne event handler tjekker, at de nødvendige permissions er givet, og kalder derefter en metode, der deler brugerens location. Metoden henter brugerens navn fra UI'et, henter deres location fra location-API'et og deler til sidst positionen via share-API'et.

**Figur 3.7** viser flowet: når brugeren klikker på Find Me-knappen, raises en event, og `OnFindMeClicked`-event handleren i code-behind trigges (1). Den bruger .NET MAUI's permissions-API'er til at verificere, at brugeren har givet app'en tilladelse til at tilgå location (2). Derefter kaldes `ShareLocation`-metoden (3), som henter brugerens navn fra `UsernameEntry`-feltet i UI'et (4) og positionen fra Location-API'et (5) og deler navn og position via Share-API'et (6).

Åbn `MainPage.xaml.cs`. Når positionen deles, sendes et link til at åbne positionen i Bing Maps. Bing tilbyder et URL-format, der understøtter at åbne en position med latitude- og longitude-koordinater, så der tilføjes et field til at holde base-URL'en. Tilføj dette øverst i `MainPage`-klassen:

```csharp
string _baseUrl = "https:/ /bing.com/maps/default.aspx?cp=";
```

Der skal også bruges en variabel til at holde brugerens navn fra den `Entry`, der blev tilføjet i XAML'en. Tilføj denne property efter `_baseURL`-fieldet:

```csharp
public string UserName { get; set; }
```

Tilføj derefter metoden, der udfører det egentlige arbejde med at hente brugerens navn og dele deres position, i slutningen af `MainPage`-klassen.

**Listing 3.5 ShareLocation method**

```csharp
private async Task ShareLocation()
{
UserName = UsernameEntry.Text;

var locationRequest = new GeolocationRequest(
➥ GeolocationAccuracy.Best);

var location = await Geolocation.GetLocationAsync(
➥ locationRequest);
await Share.RequestAsync(new ShareTextRequest
{
Subject = "Find me!",
Title =

"Find me!",

Text = $"{UserName} is sharing their location with you",
Uri = $"{_baseUrl}{location.Latitude}~{location.Longitude}"

});

}
```

① Henter den tekst, brugeren har indtastet, og tildeler den til `UserName`-variablen
② Opretter en geolocation request, der specificerer, at vi vil have den bedst tilgængelige nøjagtighed
③ Bruger geolocation requesten til at hente brugerens nuværende position
④ Opretter en ny sharing request, der specificerer, at det er tekst, vi vil dele (vi kan også dele filer)
⑤ Sætter subject og title på requesten til "Find me!". Det er op til target-applikationen at bestemme, hvordan informationen håndteres.
⑥ Sætter `Text`-propertyen på share requesten til en besked med brugerens navn
⑦ Sætter `Uri`-propertyen på share requesten til en Bing Maps-formuleret URL med brugerens koordinater

Nu skal metoden kaldes. UI'et er allerede fortalt at kalde metoden `OnFindMeClicked` i XAML'en, så den tilføjes og bruges til at kalde `ShareLocation`. `OnCounterClicked`-metoden kan fjernes, da den ikke længere bruges, og `count`-fieldet kan også slettes.

**Listing 3.6 OnFindMeClicked method**

```csharp
private async void OnFindMeClicked(object sender, EventArgs e)
{
var permissions = await Permissions.

➥ CheckStatusAsyn<Permissions.LocationWhenInUse>();

if (permissions == PermissionStatus.Granted)
{

await ShareLocation();
}
else
{
await App.Current.MainPage.DisplayAlert(
➥ "Permissions Error", "You have not granted the app

➥

permission to access your location.", "OK");

var requested = await Permissions.RequestAsync<
➥ Permissions.LocationWhenInUse>();
if (requested == PermissionStatus.Granted)
{
await ShareLocation();

}
else
{
if (DeviceInfo.Platform == DevicePlatform.iOS ||
DeviceInfo.Platform == DevicePlatform.MacCatalyst)
{
await App.Current.MainPage.DisplayAlert(
➥ "Location Required", "Location is required to
➥ share it. Please enable location for this app
➥

in Settings.", "OK");
}
else
{

await App.Current.MainPage.DisplayAlert("Location Required",
➥ "Location is required to share it. We'll ask again next time.", "OK");
}
}
}
}
```

<!-- uklart i kilden: `CheckStatusAsyn<...>` mangler tilsyneladende et 'c' (skal være CheckStatusAsync), men er gengivet ordret fra kilden -->

① `Clicked`-propertyen på en knap er en **delegate**, så den kræver en metode med den rigtige signatur (to parametre, én af typen `object`, den anden af typen `EventArgs`).
② Først skal permission-status for brugerens location hentes, mens app'en er i brug.
③ Tjekker permission-status for at se, om vi har fået tilladelse til at tilgå brugerens location
④ Hvis vi har tilladelse, kaldes `ShareLocation`-metoden fra listing 3.5
⑤ Hvis vi ikke har tilladelse til at tilgå brugerens location, vises en alert
⑥ Anmoder eksplicit om brugerens tilladelse til at tilgå deres location
⑦ Tjekker permission-status igen, efter vi har anmodet om den
⑧ Hvis vi har tilladelse, deles brugerens position
⑨ Hvis vi ikke har tilladelse, skal vi afgøre hvilken platform brugeren er på, da man på iOS og macOS ikke kan spørge brugeren om permissions mere end én gang.
⑩ Hvis de er på macOS eller iOS, vises en alert om, at vi har brug for tilladelse til deres location, og at de skal aktivere det i Settings
⑪ Viser en sidste alert om, at vi har brug for brugerens tilladelse for at dele deres position, og at vi spørger igen næste gang

Dermed er al koden til FindMe-app'en skrevet. **Figur 3.8** viser den færdige FindMe-app kørende på macOS.

---

## 3.3 Persisting data on your user's device

Forrige afsnit handlede om at tilgå device- og OS-features, der ikke er tilgængelige for web-apps. Det holder til dels, men eksemplet — adgang til brugerens location — kan faktisk også gøres i en web-app (selvom sharing ikke aktuelt understøttes konsistent af web-browsere). Dette afsnit handler om **datalagring**. Det kan også gøres i en web-app, men med begrænsninger, man ikke har i .NET MAUI.

Den væsentligste skelnende feature mellem en .NET MAUI-app og en web-app (specifikt en single-page application) er **secure storage**. Web-apps er begrænsede af de permissions og API'er, browsere tilbyder, og kan ikke gemme krypterede data på en måde, der både er sikker og reversibel. Hvis en web-app vil tilgå krypterede data, skal den gemme nøglen i browserens cache, som er sårbar over for en række angreb. Platform-executable apps har derimod fuld adgang til alle de API'er, OS'et eller platformen stiller til rådighed, og er kun begrænset af de permissions, brugeren har givet dem.

**Tabel 3.2 — Sammenligning af datalagringsmuligheder i web-apps og .NET MAUI-apps**

| Storage feature | Web app | .NET MAUI app |
| --- | --- | --- |
| **Files** | En bruger kan åbne og gemme filer. Filsystemadgang er inkonsistent mellem browsere og OS'er, så adgang til specifikke filer kan ikke garanteres. App'en kan ikke åbne en fil uden brugeren. | En bruger kan åbne og gemme filer. Der leveres abstraktioner over almindelige placeringer, hvilket betyder, at en udvikler kan forvente et konsistent resultat på tværs af platforme. App'en kan åbne en fil uden en bruger, så data nødvendige for app'ens drift kan indlæses i baggrunden. |
| **Preferences** | Kan gemmes i browser-cachen, men skal serialiseres til et struktureret tekstformat som JSON. Kan også bruge Web Storage API. | En fælles abstraktion leveres til app-konfigurationsplaceringen på hver platform. Data gemmes som key-value-par. |
| **Encryption** | Web-apps kan ikke gemme krypterede data sikkert. Der findes måder at kryptere data i en web-app, men hvis app'en også skal kunne læse dataene, skal den gemme nøglen, som dermed eksponeres. | En fælles abstraktion leveres, som lader dig gemme data i en krypteret placering med en nøgle, der administreres af OS'et. |
| **Structured data** | Web-apps kan bruge IndexedDB API, et objektorienteret database-API tilgængeligt i moderne browsere. | .NET-økosystemet gør utallige muligheder tilgængelige, herunder Entity Framework (EF) Core. Enhver database-engine tilgængelig i .NET Standard kan bruges i en .NET MAUI-app. |

.NET MAUI leverer featuren **`SecureStorage`**. `SecureStorage` bruger key-value-par ligesom `Preferences`; men i modsætning til `Preferences` hentes en kryptografisk nøgle fra OS'et til at kryptere værdien af de gemte data.

**Figur 3.9** illustrerer forskellen: `SecureStorage` bruger en kryptografisk nøgle leveret og administreret af OS'et til at kryptere værdien af en post, der gemmes via `Preferences`-API'et.

`SecureStorage` bruger en abstraktion over hver platforms underliggende API til håndtering af krypterede data. På macOS og iOS bruger `SecureStorage` fx **KeyChain**, mens den på Android bruger **Keystore**. Den kryptografiske nøgle administreres af OS'et og er kun tilgængelig for app'en, og i de fleste tilfælde er den understøttet af en hardware-krypteringschip.

> **Encryption with .NET MAUI apps**
> Hardwarebaseret kryptering er garanteret på iOS og macOS, da alle macOS- og iOS-enheder leveres med en indbygget krypteringschip. På Android — især hvis enheden er en tablet eller telefon — er hardwarekryptering meget sandsynlig, men ikke garanteret. De fleste moderne Windows-laptops har en Trusted Platform Module (TPM)-chip, der leverer hardwarekryptering, mens forholdsvis få desktops har det. Windows 11's TPM-krav vil sandsynligvis forskyde denne balance fremover.
>
> Dette er nyttig information, men ikke strengt nødvendig for at bygge apps med .NET MAUI. .NET MAUI leverer `SecureStorage` som en abstraktion, så uanset om hardware- eller softwarebaseret kryptering bruges, er API'et i koden det samme.

Mens kryptering er den klare differentiator mellem web-apps og native apps, er struktureret datalagring også mere alsidig i .NET MAUI-apps. IndexedDB i moderne browsere er kraftfuld, men er stadig en samling af key-value-par — om end en indekseret samling med hurtig søgning og fremhentning. Udvalget af databasemuligheder i .NET MAUI-apps er markant bredere og mere varieret og omfatter object-relational mappers (ORM'er), relationelle databaser og masser af NoSQL-muligheder.

I resten af afsnittet bygges **MauiTodo**, en simpel to-do-app til at oprette en liste af opgaver. Der bruges en database til at gemme to-do-items, så de persisteres mellem brug; og fordi der skal kunne gemmes fortrolige to-do-items, krypteres databasen.

**Figur 3.10** viser den overordnede arkitektur: MauiTodo bruger **SQLite** til at gemme data. Dataene krypteres med **SQLCipher**. Databasens krypteringsnøgle er en GUID, som selv krypteres og gemmes af OS'et og tilgås med .NET MAUI's `SecureStorage`-API.

Der bruges SQLite, en populær open source database-engine. Man kunne bruge EF Core SQLite-provideren, men bogen bruger pakken **SQLite-net**, en implementering af SQLite designet specifikt til mobilapps. Derudover tilføjes **SQLCipher** til at kryptere databasens indhold. SQLCipher håndterer kryptering og dekryptering af dataene, mens `SecureStorage`-API'et lader OS'et administrere databasens krypteringsnøgle.

Start med at oprette et nyt .NET MAUI-projekt kaldet **MauiTodo** med `blankmaui`-templaten. Definér derefter datamodellen: tilføj en mappe kaldet `Models`, og tilføj en ny klassefil `TodoItem.cs`.

**Listing 3.7 TodoItem class**

```csharp
using System;
namespace MauiTodo.Models
{
public class TodoItem
{
public int Id { get; set; }
public string Title { get; set; }
public DateTime Due { get; set; }
public bool Done { get; set; } = false;
}
}
```

Det er et **plain-old CLR object (POCO)** med nogle properties, der repræsenterer et typisk to-do-item. Næste skridt er at tilføje database-funktionaliteten til at gemme og hente to-do-items. Tilføj NuGet-pakkerne **`sqlite-net-pcl`** og **`sqlite-net-sqlcipher`**.

> **Consuming NuGet packages**
> En af de ting, der gør .NET MAUI til et fremragende værktøj for .NET-udviklere, er adgangen til .NET-økosystemet. En væsentlig komponent er mængden af biblioteker tilgængelige som NuGet-pakker.
>
> Enhver NuGet-pakke, man er fortrolig med og kan lide at bruge, er tilgængelig i .NET MAUI-apps, forudsat at den ikke har en afhængighed af en specifik platform. Eksempler kunne være Windows Compatibility Pack og bredere set nogle pakker populære blandt Xamarin.Forms-udviklere, som har en Xamarin.Forms-afhængighed. Dem virker ikke i .NET MAUI-apps.
>
> Kommer man fra Xamarin.Forms, er en stor fordel ved .NET MAUI **single-project solution**. For at bruge en NuGet-pakke skal den kun importeres én gang, frem for én gang til det delte projekt og én gang pr. platform.

På Android kræves en yderligere NuGet-pakke, `SQLitePCLRaw.provider.dynamic_cdecl`; men denne pakke giver utilsigtede sideeffekter på andre platforme. Pakken kan tilføjes udelukkende til Android med en **MSBuild condition** i `.csproj`-filen. Tilføj følgende til `MauiTodo.csproj`, lige efter den `ItemGroup`, der indeholder de eksisterende package references.

**Listing 3.8 Android-specific package references**

```xml
<ItemGroup
Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) ==
'android'">
<PackageReference Include="SQLitePCLRaw.provider.dynamic_cdecl" Version="2.1.4"
/>
</ItemGroup>
```

Da dette er eneste gang, teknikken bruges, går bogen ikke i dybden med hvordan den virker. Tilgangen er dog allerede anvendt i `.csproj`-filen af templaten. Specifikationerne for MSBuild conditions findes i dokumentationen på http://mng.bz/x4Z6.

Nu skal `TodoItem`-klassen opdateres, så SQLite ved, at `Id`-propertyen er primærnøgle og skal auto-inkrementeres af databasen. Tilføj `PrimaryKey`- og `AutoIncrement`-attributterne fra SQLite-namespacet til `Id`-propertyen:

```csharp
[PrimaryKey, AutoIncrement]
public int Id { get; set; }
```

Opret en mappe kaldet `Data`, og i den en ny klassefil `Database.cs`. I denne klasse tilføjes metoder til at tilføje, hente og slette to-do-items fra en SQLite-database med en datafil i en særlig placering til brugerdata. Placeringen er forskellig på hver platform, men .NET MAUI's abstraktion sætter stien automatisk. Databasen initialiseres også med en krypteringsnøgle, som er en GUID, og GUID'en krypteres og gemmes sikkert med `SecureStorage`.

SQLite kræver en **connection string** for at forbinde til en database. Her indeholder den stien til datafilen og definerer også, hvordan `DateTime`-værdier gemmes. Da SQLCipher bruges, angives også nøglen til at kryptere og dekryptere databasen. SQLite-net indeholder helper-metoder, der genererer connection stringen ud fra de angivne værdier, og SQLCipher har yderligere extension-metoder, der dækker krypteringsnøglen (**figur 3.11**).

SQLite indeholder en letvægts-ORM, der lader os interagere med databasen via POCO'er frem for at skrive SQL-statements hver gang, der skal skrives eller læses data. De samme POCO'er kan bruges som tabeldefinitioner. **Figur 3.12** viser, at SQLite's ORM lader os definere tabeller med POCO'er, og at der leveres en asynkron metode til at verificere, om en tabel eksisterer, og oprette den hvis ikke.

Vi har defineret en model med en C#-type, der repræsenterer to-do-items, og lader SQLite generere tabellerne ud fra denne definition. Vi vil være sikre på, at tabellen er oprettet i databasen, før vi begynder at læse fra eller skrive til den. Men vi ønsker ikke at blokere app'en, mens tabellen sættes op. Heldigvis er SQLite-nets metode til at oprette tabellen asynkron; men da vi ikke kan have async-konstruktører, indkapsles kaldet, der opretter tabellen, i sin egen async-metode. Denne metode kaldes så fra konstruktøren med en **discard (`_`)**, som kører metoden i en tråd uden at blokere UI'et.

> **Async/await in .NET MAUI apps**
> Som UI-udvikler er det vigtigt at sikre, at langvarige (eller potentielt fejlende) operationer ikke låser app'en. Det er en dårlig brugeroplevelse, og man mister brugere. Som .NET-udvikler er man måske allerede fortrolig med async/await og asynkron programmering.
>
> Under alle omstændigheder er det værd at bruge tid på at lære, hvordan disse patterns bedst anvendes i UI-udvikling. Bogen viser flere eksempler undervejs og fremhæver specifikke patterns, tips eller faldgruber. Vil man lære mere, findes der masser af udmærkede ressourcer online (særligt fra Brandon Minnick og Brian Lagunas). En anden god ressource er SSW Rules, en gratis ressource fra forfatterens arbejdsgiver.

Når tabellen er oprettet, kan vi fortsætte med at bruge POCO'er og ORM'en til at interagere med den. At tilføje items er så simpelt som at kalde `_connection.InsertAsync(item)`; ORM'en er intelligent nok til at udlede, hvilken tabel `item` skal indsættes i, ud fra dens type. At forespørge en tabel er også simpelt med **LINQ** og et **lambda expression**.

**Listing 3.9 Database class**

```csharp
using MauiTodo.Models;
using SQLite;
namespace MauiTodo.Data
{
public class Database
{
private readonly SQLiteAsyncConnection _connection;
public Database()
{
var dataDir = FileSystem.AppDataDirectory;
var databasePath = Path.Combine(dataDir, "MauiTodo.db");
string _dbEncryptionKey = SecureStorage.

➥ GetAsync("dbKey").Result;

if (string.IsNullOrEmpty(_dbEncryptionKey))
{
Guid g = new Guid();
_dbEncryptionKey = g.ToString();

SecureStorage.SetAsync("dbKey", _dbEncryptionKey);

}
var dbOptions = new SQLiteConnectionString(
➥ databasePath, true, key: _dbEncryptionKey);

_connection = new SQLiteAsyncConnection(dbOptions);

_ = Initialise();

}
private async Task Initialise()
{
await _connection.CreateTableAsync<TodoItem>();

}
public async Task<List<TodoItem>> GetTodos()
{
return await _connection.Table<TodoItem>().ToListAsync();

}
public async Task<TodoItem> GetTodo(int id)
{
var query = _connection.Table<TodoItem>().
➥ Where(t => t.Id == id);
return await query.FirstOrDefaultAsync();

}
public async Task<int> AddTodo(TodoItem item)
{
return await _connection.InsertAsync(item);
}
public async Task<int> DeleteTodo(TodoItem item)
{
return await _connection.DeleteAsync(item);
}
public async Task<int> UpdateTodo(TodoItem item)
{
return await _connection.UpdateAsync(item);

}
}
}
```

① `FileSystem.AppDataDirectory`-helperen returnerer application data-stien for den platform, app'en kører på.
② Forespørger `SecureStorage` efter en værdi med nøglen "dbKey". Her refererer "key" til *identifikatoren* for det sikkert gemte item (som i key-value-par), ikke den kryptografiske nøgle, der sikrer det. Da `SecureStorage`-API'et kun er async, skal `.Result` kaldes, fordi vi ikke kan await'e kaldet i klassens konstruktør.
③ Opretter en ny GUID, hvis databasens krypteringsnøgle er tom
④ Da ingen krypteringsnøgle blev returneret fra `SecureStorage`, sættes værdien til den nye GUID
⑤ Bruger en helper-metode fra SQLite-net til at oprette en connection string til databasens datafil. Helperen tager en sti, en boolean der afgør om `DateTime`-værdier gemmes som ticks, og en krypteringsnøgle til at kryptere og dekryptere databasen.
⑥ Sætter et read-only field til at holde den asynkrone databaseforbindelse
⑦ "Throw away"-kald til en async-metode. Vi skal returnere en konstrueret klasse med en initialiseret forbindelse, men behøver ikke vente på, at database-enginen initialiserer tabellerne.
⑧ SQLite-net indeholder en letvægts-ORM, så en tabel kan initialiseres ud fra en model. `CreateTableAsync` tager en typeparameter, der fungerer som model for tabellen, og opretter en tabel baseret på modellen, hvis en sådan ikke allerede findes.
⑨ Med ORM'en kan vi bruge en typeparameter til at anmode om en tabel med et design, der matcher den angivne type. Vi await'er resultatet og caster det til en `List`.
⑩ Bruger et LINQ lambda expression til at oprette en query-definition, med en type til at repræsentere et tabeldesign
⑪ Await'er queryets resultat, caster det til den angivne type og returnerer dette awaitable kald
⑫ ORM'en har en `InsertAsync`-metode, der er intelligent nok til at afgøre, hvilken tabel den angivne parameter skal indsættes i, baseret på parameterens type

Vi har nu en database i app'en, der sikkert kan gemme hemmelige to-do-items og leverer metoder til at gemme og hente dem. Næste skridt er at bygge et UI til at vise og indtaste to-do-items samt kode, der forbinder UI'et til databasen.

> **LINQ and lambda expressions**
> Da .NET MAUI-apps er .NET-apps, kan vi bruge alt fra C#-værktøjskassen. Man er sandsynligvis allerede fortrolig med LINQ, især hvis man arbejder med EF Core; hvis ikke, er det værd at investere tid i (start med Microsofts dokumentation), da det er nyttigt i flere scenarier. Disse scenarier handler ikke kun om databaser; LINQ virker også med collections, og collections er kernen i næsten alle apps.
>
> Lambda expressions lader dig deklarere en anonym funktion. Det betyder, at du kan bede din kode om at eksekvere anden kode og returnere resultatet uden at skulle deklarere den anden kode som en metode. Lambda expressions er utroligt kraftfulde sammen med LINQ, men også nyttige i en række andre scenarier.
>
> Alle .NET-kompetencer er overførbare til .NET MAUI-apps. Er du .NET-udvikler, er du .NET MAUI-udvikler!

I `MainPage.xaml` laves nu nogle UI-ændringer: sidetitlen opdateres, der tilføjes et tekstfelt hvor brugeren kan angive titlen på nye to-do-items, en date picker til forfaldsdato, en knap til at bekræfte tilføjelsen, og til sidst en måde at vise listen af to-do-items fra databasen.

Slet `ScrollView` og alt indeni i XAML'en. Tilbage står `<ContentPage...>...</ContentPage>`-taggene. Den fjernede `ScrollView` er et layout, og den erstattes med en anden slags layout: et **`Grid`** (layouts dækkes i kapitel 4).

**Listing 3.10 UI for the MauiTodo app**

```xml
<Grid RowDefinitions="1*, 1*, 1*, 1*, 8*"
MaximumWidthRequest="400"
Padding="20">

<Label Grid.Row="0"
Text="Maui Todo"
SemanticProperties.HeadingLevel="Level1"
SemanticProperties.Description="Maui Todo"
HorizontalTextAlignment="Center"
FontSize="Title"/>
<Entry Grid.Row="1"
HorizontalOptions="Center"
Placeholder="Enter a title"
SemanticProperties.Hint="Title of the new todo item"
WidthRequest="300"
x:Name="TodoTitleEntry" />
<DatePicker Grid.Row="2"
WidthRequest="300"
HorizontalOptions="Center"
SemanticProperties.Hint="Date the todo item is due"
x:Name="DueDatepicker" />
<Button Grid.Row="3"
Text="Add"
SemanticProperties.Hint="Adds the todo item to the database"
WidthRequest="100"
HeightRequest="50"
HorizontalOptions="Center"
Clicked="Button_Clicked"/>
<ScrollView Grid.Row="4">
<Label HorizontalTextAlignment="Center"
SemanticProperties.Description="The list of todo items
➥ in the database"
x:Name="TodosLabel" />
</ScrollView>
</Grid>
```

① Tilføjer et grid layout med fem rækker
② Tilføjer en label med sidetitlen
③ Tilføjer en `Entry`, hvor brugeren kan indtaste titlen på et nyt to-do-item
④ Tilføjer en `DatePicker`. En date picker viser platformens standardkontrol til at vælge en dato og returnerer den valgte værdi som en `DateTime`.
⑤ Tilføjer en knap, som brugeren kan klikke på for at tilføje et nyt to-do-item
⑥ Tilføjer en `ScrollView` inde i griddet på den femte række (row 4). Vi vil have en `ScrollView`, fordi den fyldes med de to-do-items, brugeren tilføjer, hvilket kan strække sig ud over siden.
⑦ Tilføjer en label, der viser listen af to-do-items, brugeren har tilføjet

Vi har nu defineret et simpelt UI til at indtaste og liste brugerens to-do-items. Alle kontroller er kendte bortset fra den nye `DatePicker`, som lader brugeren vælge en dato med en grafisk vælger frem for at indtaste den i et tekstfelt.

Dernæst tilføjes kode i code-behind:

- Properties og fields, der holder en instans af databasen, værdierne af to-dos i databasen, og de værdier brugeren vil tilføje til et nyt to-do-item.
- Opdatering af klassens konstruktør og en metode til at initialisere siden ved load.
- En metode, der reagerer på knapklik og tilføjer et nyt to-do til databasen.

**Listing 3.11 Code for the MauiTodo main page**

```csharp
using MauiTodo.Data;
using MauiTodo.Models;
namespace MauiTodo;
public partial class MainPage : ContentPage
{

string _todoListData = string.Empty;

readonly Database _database;

public MainPage()
{
InitializeComponent();
_database = new Database();

_ = Initialize();

}
private async Task Initialize()
{
var todos = await _database.GetTodos();

foreach (var todo in todos)
{
_todoListData += $"{todo.Title} - {todo.
➥ Due:f}{Environment.NewLine}";
}
TodosLabel.Text = _todoListData;

}
private async void Button_Clicked(object sender,EventArgs e)
{
var todo = new TodoItem
{
Due = DueDatepicker.Date,
Title = TodoTitleEntry.Text
};

var inserted = await _database.AddTodo(todo);

if (inserted != 0)
{

_todoListData += $"{todo.Title} - {todo.Due:f}{Environment.NewLine}";
TodosLabel.Text = _todoListData;

TodoTitleEntry.Text = String.Empty;

DueDatepicker.Date = DateTime.Now;
}
}
}
```

① Indeholder værdierne af de to-do-items, vi vil vise på skærmen
② Gemmer en instans af vores database-klasse
③ I konstruktøren oprettes en instans af database-klassen og tildeles `_database`-fieldet.
④ Bruger discard-variablen til at kalde `Initialize`-metoden
⑤ Henter listen af to-do-items i databasen
⑥ Tilføjer titel og dato for hvert to-do-item (med `:f`-syntaksen til at formatere det til en culture-formateret string) samt en ny linje til `_todoListData`-fieldet
⑦ Tildeler værdien af `_todoListData`-fieldet til `Text`-propertyen på `TodosLabel` i UI'et
⑧ I button clicked-delegaten oprettes en ny instans af `TodoItem`-klassen, hvor tekstværdien fra `Entry` i UI'et tildeles `Title`-propertyen, og datoværdien fra date pickeren tildeles `Due`-propertyen
⑨ Tilføjer det nyoprettede to-do-item til databasen
⑩ Tjekker `Id`-værdien på to-do-itemet for at sikre, at det blev indsat
⑪ Tilføjer det nye to-do-item til listen
⑫ Opdaterer værdien af `TodosLabel` i UI'et
⑬ Nulstiller de to inputkontroller, klar til et nyt to-do

Dermed er MauiTodo-app'en færdig indtil videre. **Figur 3.13** viser MauiTodo kørende på Windows: når man klikker på Add-knappen, henter en event handler værdierne fra `Entry` og `DatePicker` og bruger dem til at oprette et nyt to-do-item med titel og forfaldsdato. Derefter tilføjes det til listen af to-do-items på skærmen.

---

## 3.4 Data binding: Connecting the UI to the code

Indtil videre har vi brugt code-behind til at hente værdier fra og tildele værdier til properties på UI-elementer. Ved at inkludere skemaet `http://schemas.microsoft.com/winfx/2009/xaml` i XAML-filen kan vi bruge `x:...` til at give navne til UI-elementer (fx `x:Name="TodoTitleEntry"`) og referere til dem ved navn i koden. Det lader os bruge `TodoTitleEntry.Text` til at læse værdien af tekst-propertyen på UI-elementet `TodoTitleEntry`.

XAML er et markup-sprog, men de UI-elementer, vi bruger i XAML, er stadig klasser. Når vi tilføjer et `<Entry ... />` til vores view, instantierer vores view (også en klasse) ved runtime et objekt af typen `Entry`. Kigger man på `Entry`-klassens properties, findes en property kaldet `Text` af typen `string`, hvor den værdi, brugeren indtastede, hentes.

At kunne tilgå properties på XAML-elementer fra kode er i sig selv kraftfuldt og giver en pæn måde at hente og sætte værdier fra UI'et. Men en bedre måde er **data binding**. Data binding lader os binde en property til en anden property, sådan at når den ene ændres, ændres den anden også (**figur 3.14**: en property på en klasse, der nedarver fra `BindableObject`, kan bindes til en property af samme type).

Data binding forbinder en **source property** og en **target property** (**figur 3.15**). Target-propertyen skal tilhøre et objekt, der nedarver basisklassen **`BindableObject`** (alle indbyggede UI-kontroller i .NET MAUI nedarver `BindableObject`). Target-propertyen bindes til sourcen, så det er på target'et, bindingen sættes. Source-propertyen kan tilhøre et objekt af enhver type, men skal matche target-propertyens type. Er target- og source-propertyerne ikke af samme type, kan en **value converter** bruges — value converters dækkes i kapitel 8.

**Figur 3.15** viser desuden, at hvert view (target object) kan have ét enkelt **binding context** (source object). Properties på target-viewet bindes til properties af samme type på source-objektet.

Når bindingen er sat, kan properties på det view (target) bindes til sourcens properties ved blot at referere til property-navnet. Individuelle properties på kontrollen kan stadig bindes vilkårligt til enhver property på ethvert andet objekt, men det kræver eksplicit referering; sætter man binding context for en kontrol, kan alle kontrollens properties bindes til alle sourcens properties blot ved at bruge property-navnet.

> **Controls vs. views**
> Nogle gange kan det virke, som om termerne *view* og *control* bruges i flæng. I nogle sammenhænge kan de det; men for at undgå forvirring: views findes i tre kategorier — **pages**, **layouts** og **controls**:
>
> - **Page** — En særlig slags view, der (som regel) fylder hele skærmen og kan navigeres til. En `Page` indeholder ét child layout, som igen kan indeholde flere andre layouts.
> - **Layout** — Et view brugt til at arrangere elementer på skærmen. Eksempler set indtil videre er `ScrollView` (teknisk set en control, men den opfører sig som et layout) og `Grid`.
> - **Control** — Et view, der enten eksplicit viser noget på skærmen eller modtager brugerinput. En `Label` er en control, det samme er en `Entry`.

Binding context **nedarves** og kaskaderer fra parent til child-komponenter i et view. Binding contexts kan defineres eksplicit for ethvert view, men definerer man ikke eksplicit et binding context for en kontrol i en `ContentPage`, bliver binding context det samme som `ContentPage`'ens. Tilføjer man fx et layout eller en collection til en `ContentPage`, arver layoutet `ContentPage`'ens binding context, og det samme gør alle dets child controls. Sætter man eksplicit binding context på layoutet eller collectionen, arver child controls det i stedet.

**Figur 3.16** illustrerer dette: binding context for en side er sat til sidens code-behind. `Title` og `Button` arver dette binding context og binder til properties på `ContentPage`-code-behind. Binding context for `VerticalStackLayout` er sat til en objekt-property i code-behind. `Subtitle` og `Button` inde i dette layout arver dette binding context og binder til properties på objektet. Pointen er, at binding context kan sættes på ethvert niveau og nedarves af child-komponenter fra det punkt og nedad.

Views kan også bindes til andre views. Fx har både `Label` og `Entry` en property kaldet `Text` af typen `string`, så disse properties kan bindes (**figur 3.17**). Ved at binde de to properties kan den ene automatisk opdatere den anden.

---

## 3.4.1 View-to-view bindings

View-to-view bindings kan sættes i XAML og gør det muligt at binde properties i et view uden nogen indgriben i code-behind. Opret et nyt .NET MAUI-projekt kaldet **Bindings**, og slet `ScrollView` og dens indhold i `MainPage.xaml`. Tilføj følgende kode i stedet (inden for `ContentPage`-taggene).

**Listing 3.12 Markup for view-to-view bindings**

```xml
<VerticalStackLayout VerticalOptions="Center"
HorizontalOptions="Center"
Spacing="20"
WidthRequest="200">

<Label FontSize="Title"
BindingContext="{x:Reference TextEntry}"
Text="{Binding Text}"
HorizontalTextAlignment="Center"/>
<Entry x:Name="TextEntry"
Placeholder="Enter some text..." />
</VerticalStackLayout>
```

① Placerer vores kontroller inde i et `VerticalStackLayout`. Layouts dækkes i kapitel 4.
② Sætter binding context for dette view (her `Label`-kontrollen) til et andet view ved navn `TextEntry`. `x:Reference`-syntaksen refererer til andre views på samme side ved navn.
③ Sætter binding source (den property på binding context, som target-propertyen skal bindes til) til `Text`-propertyen på binding context
④ Denne `Entry` er navngivet `TextEntry`, så den kan refereres ved navn i andre views

Før projektet køres, slettes `count`-fieldet og `OnCounterClicked`-metoden fra code-behind. Kør derefter projektet og begynd at skrive tekst i `Entry`'en.

**Figur 3.18** viser resultatet: `Text`-propertyen på en `Label` er bundet til `Text`-propertyen på en `Entry`. Når `Text` ændres på `Entry`'en, ændres `Label`'en automatisk.

Bemærk hvordan labelens tekst ændrer sig, mens man skriver. I tidligere eksempler brugte vi code-behind til at hente properties fra UI-kontroller og derefter sætte properties på andre kontroller. Med data binding peger vi den ene property på den anden, og resten klares for os.

View-to-view binding er nyttig, når man har properties af samme type på to indbyrdes relaterede views. I eksemplet opdateres en `Label` med den tekst, der indtastes i en `Entry`. Tilsvarende adfærd fra virkelige apps kunne være at opdatere numre på en mockup af et kreditkort, mens brugeren indtaster dem. Et andet nyttigt eksempel er at ændre zoom-niveauet på et billede eller kort som reaktion på en slider. Den funktionalitet tilføjes nu til Bindings-app'en.

Tilføj de to kontroller fra næste listing under `Entry`'en i `VerticalStackLayout`.

**Listing 3.13 Controls to add to the Bindings app**

```xml
<Slider x:Name="ZoomSlider" />

<Image Source="dotnet_bot.png"
WidthRequest="300"
HorizontalOptions="Center"

BindingContext="{x:Reference ZoomSlider}"
Scale="{Binding Value}" />
```

① Vi tilføjer en `Slider`-kontrol og navngiver den `ZoomSlider`, så vi kan referere til den andre steder.
② Tilføjer Dotnet Bot-billedet igen
③ Sætter binding context for image-kontrollen til `ZoomSlider`. Det betyder, at alle properties på `Image`-kontrollen, vi binder, får deres værdier fra `ZoomSlider`-objektet.
④ Sætter `Scale`-propertyen på `Image`. Vi bruger en binding frem for en eksplicit værdi, og bindingen er til en property kaldet `Value`. Binding context er sat til `ZoomSlider`, så `Scale`-propertyen på dette `Image` er bundet til `ZoomSlider.Value`.

**Figur 3.19** viser Bindings-app'en med `Scale`-propertyen på et `Image` bundet til `Value`-propertyen på en `Slider`. Flytter man slideren frem og tilbage, reagerer billedets skala i realtid.

Man kunne have gjort dette uden data binding, men processen ville have været langt mere omstændelig: man skulle oprette en metode, der kaldes af `Slider`'ens `ValueChanged`-event, og med hver ændring i værdien sætte `Scale`-propertyen på `Image`.

---

## 3.4.2 Collections and bindings in code

Data binding er en utroligt kraftfuld feature i .NET MAUI, og den gør langt mere end blot at fjerne boilerplate-kode. Den åbner en række muligheder, vi ikke ville have uden den.

En af de mest kraftfulde anvendelser er at sætte en binding source for en **`CollectionView`**, via propertyen **`ItemsSource`**. En `CollectionView` viser en samling af items, fx en `List<T>`, på skærmen. Collections eller lister er kernen i mange apps, og med .NET MAUI kan man bruge data binding til at definere, hvordan hvert enkelt item i samlingen skal vises.

Den mest kraftfulde feature i `CollectionView` er brugen af en **`DataTemplate`**. En `DataTemplate` er definitionen af, hvordan ting vises på skærmen. I det nuværende eksempel bruges en `Label`, som kun kan vise tekst. Med en `DataTemplate` kan man definere et vilkårligt layout og binde dele af layoutet til properties på sit binding context.

Mens en `DataTemplate` definerer, hvordan et item vises, er en **`ItemTemplate`** den specifikke `DataTemplate`, der er i brug for `CollectionView`'en. `ItemTemplate` er en property på en `CollectionView`, og en `DataTemplate` tildeles den. `DataTemplate`s kan defineres inline (som i følgende eksempel), men kan også være selvstændige, genbrugelige views — det dækkes senere i bogen.

Frem for at arve hele den collection, `CollectionView`'en er bundet til, er binding context for en `ItemTemplate` **selve itemet**. Det betyder, at for en samling af objekter med en string-property kaldet `Name` kan man sætte bindingen for `Text`-propertyen på en `Label` i `ItemTemplate` til `Name`, og den ønskede værdi vises.

Ser vi tilbage på MauiTodo-app'en, kan visningen af to-do-items forbedres væsentligt. Lige nu er der en `Label`, der viser titlen på hvert to-do-item i databasen adskilt af et newline-tegn. Det er primitivt og ret begrænset; vi kan fx ikke vise forfaldsdatoen eller formatere forskellige items baseret på deres properties (overdue, completed osv.).

App'en opdateres til at bruge en `CollectionView`. Det lader os definere et specifikt layout for hvert item i to-do-listen, og vi binder `CollectionView`'en i kode til en **`ObservableCollection`** af `TodoItem`s. En `ObservableCollection` er en generisk collection ligesom en `List`, bortset fra at den automatisk er koblet op gennem XAML-enginen til at levere en notifikation, når items tilføjes eller fjernes, uden at vi skal gøre noget ekstra i koden. Det opdaterer automatisk `CollectionView`'en i UI'et, hvis vi tilføjer eller fjerner to-do-items.

Start med UI'et. I `MainPage.xaml` slettes `ScrollView` på række 4 af griddet og dens indhold, og erstattes med denne `CollectionView`.

**Listing 3.14 To-dos CollectionView**

```xml
<CollectionView Grid.Row="4"
x:Name="TodosCollection">
<CollectionView.ItemTemplate>
<DataTemplate>
<Grid WidthRequest="350"
Padding="10"
Margin="0,20"
ColumnDefinitions="2*, 5*"
RowDefinitions="Auto, 50"
x:Name="TodoItem">
<CheckBox VerticalOptions="Center"
HorizontalOptions="Center"
Grid.Column="0"
Grid.Row="0" />

<Label Text="{Binding Title}"
FontAttributes="Bold"
LineBreakMode="WordWrap"
HorizontalOptions="StartAndExpand"
FontSize="Large"
Grid.Row="0"
Grid.Column="1"/>
<Label Text="{Binding Due, StringFormat=
➥ '{0:dd MMM yyyy}'}"
Grid.Column="1"
Grid.Row="1"/>
</Grid>
</DataTemplate>
</CollectionView.ItemTemplate>
</CollectionView>
```

① Tilføjer en `CollectionView` og placerer den i række 4 af griddet. Giver `CollectionView`'en navnet `TodosCollection`, så vi kan referere til den i kode.
② Inde i `CollectionView`'en tilføjes et element til at definere `ItemTemplate`-propertyen
③ Tilføjer en `DataTemplate`. En `DataTemplate` definerer, hvordan hvert item i samlingen præsenteres, og tildeles `ItemTemplate`-propertyen på `CollectionView`'en ved at være nested som direkte child.
④ Bruger et `Grid` til item-layoutet
⑤ Griddet har to kolonner: én på 2/7 af bredden og én på 5/7 af bredden.
⑥ Griddet har to rækker: én med en højde der tilpasser sig indholdet, og én med fast højde på 50.
⑦ Tilføjer en `CheckBox` i første kolonne, første række
⑧ Tilføjer en `Label` i anden kolonne, første række. Binder `Text`-propertyen på denne `Label` til `Title`-propertyen på itemet.
⑨ Tilføjer en `Label` i anden kolonne, anden række. Binder `Text`-propertyen til `Due`-propertyen på itemet, og da `Due` er en `DateTime`, angives en formateringsregel.

Vi har tilføjet en `CollectionView` og defineret, hvordan hvert item i samlingen skal vises, men vi har ikke bundet den til en collection. Det gøres nu i koden.

Åbn `MainPage.xaml.cs`, og øverst i klassen (før de private member-definitioner) tilføjes en `ObservableCollection` af `TodoItem`s:

```csharp
public ObservableCollection<TodoItem> Todos { get; set; } = new();
```

`ObservableCollection` ligger i namespacet `System.Collections.ObjectModel`, så tilføj det nødvendige using-statement øverst i filen. I konstruktøren sættes derefter `ItemsSource`-propertyen på `CollectionView`'en til `ObservableCollection`'en:

```csharp
TodosCollection.ItemsSource = Todos;
```

Nu ryddes den kode væk, der ikke længere bruges. Slet den private `_todoListData`-string og hver linje, der refererer til den: tildelingen i `foreach`-loopet i `Initialise`-metoden, tildelingen af værdien til `Label`'en i `Initialise`, tilføjelsen i `Button_Clicked` og tildelingen til `Label`'en i `Button_Clicked`.

Dernæst tilføjes logik til at initialisere collectionen og opdatere den, når en bruger tilføjer et nyt to-do-item. I `foreach`-loopet i `Initialise`-metoden tilføjes to-do-itemet til `ObservableCollection`'en:

```csharp
Todos.Add(todo);
```

Til sidst tilføjes den samme logik i `Button_Clicked`-metoden i `if`-statementet, hvor vi tidligere tilføjede itemet til strengen:

```csharp
Todos.Add(todo);
```

Opsummeret: vi har tilføjet en `CollectionView` til UI'et og defineret en data template, der specificerer, hvordan items i `CollectionView`'en skal vises. Vi har tilføjet en `ObservableCollection` — en særlig type collection, der sender notifikationer, når dens indhold ændres — til code-behind og bundet den som source for de items, `CollectionView`'en viser. Og vi har tilføjet kode, der tilføjer to-do-items til `CollectionView`'en, når siden initialiseres, og når en bruger tilføjer et nyt to-do-item.

**Figur 3.20** viser MauiTodo med en `CollectionView` kørende på Windows.

---

## 3.4.3 ItemsSource bindings in XAML

At bruge kode til at sætte binding contexts er kraftfuldt, men da data binding er det, der giver XAML sine superkræfter, bør vi også se på, hvordan man sætter `ItemsSource` for `CollectionView`'en **i XAML**. Vi kan sætte binding context for ethvert view direkte i XAML, som vi så i afsnit 3.4.2, hvor binding context for én kontrol blev sat til properties på en anden. Lad os refaktorere MauiTodo, så al binding sker i XAML.

Åbn `MainPage.xaml.cs` og fjern i konstruktøren den linje, der blev tilføjet i forrige afsnit til at sætte items source:

```csharp
TodosCollection.ItemsSource = Todos;
```

I stedet sættes bindingen op i XAML. Åbn `MainPage.xaml`, og tilføj i `<ContentPage...>`-åbningstagget et navn (så den kan refereres) og et binding context. Bind derefter `ItemsSource`-propertyen på `CollectionView`'en til `Todos`-`ObservableCollection`'en i binding context.

**Listing 3.15 MainPage.xaml ContentPage tag changes**

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiTodo.MainPage"
BackgroundColor="{DynamicResource SecondaryColor}"
x:Name="PageTodo"
BindingContext="{x:Reference PageTodo}">

...

<CollectionView Grid.Row="4"
ItemsSource="{Binding Todos}"
x:Name="TodosCollection">
```

① Bruger `Name`-attributten fra XAML-namespacet (aliased til `x`) til at specificere et navn for siden
② Sætter binding context for siden. Brug det navn, der blev oprettet for siden, som reference, og sæt dermed sidens binding context til sig selv.
③ Binder `ItemsSource`-propertyen på `CollectionView`'en til `Todos`-propertyen på binding context

Kør app'en igen. Den bør køre uden problemer med samme resultat (svarende til figur 3.19). Selvom der ikke er nogen synlig ændring i UI'et, er forbedringen af koden betydelig. Tidligere havde vi bindingen for `ItemsSource` sat i kode og alle øvrige bindings sat i XAML. Nu er XAML'en ansvarlig for at sætte alle bindings. Det bliver endnu vigtigere, når vi lærer om **MVVM-mønstret** i kapitel 9 og ser, hvordan denne tilgang hjælper med at opretholde en ren separation of concerns i koden.

---

## Summary

- Man kan tilgå almindelige cross-platform device- og OS-features via .NET MAUI's API'er. De giver adgang til ting som location, sharing, storage og meget mere.
- Man skal anmode brugeren om tilladelse, før man tilgår privatlivsfølsomme features såsom location, og disse permissions kan anmodes om ved runtime. I app-metadata, defineret i en fil specifik for hver platform, deklarerer man hvilke permissions man vil anmode om, og — for Android — hvilke features man vil bruge.
- Som en del af .NET giver MAUI adgang til hele NuGet-økosystemet. Det betyder, at man kan bruge sine eksisterende .NET-kompetencer og de pakker, man allerede kender, til at bygge .NET MAUI-apps.
- Man kan gemme data lokalt til offline-brug, hvilket løfter en .NET MAUI-app ud over en webside. Man kan persistere key-value-par med `Preferences`-API'et og gemme krypterede key-value-par med `SecureStorage`-API'et.
- Man kan gemme mere komplekse data end key-value-par, herunder billeder, videoer eller lydoptagelser, via filsystemet. Kombineret med NuGet kan man bruge en database som SQLite til at gemme og hente komplekse datastrukturer og arbejde helt offline.
- Man kan bruge kode til at hente og sætte værdier på elementer i UI'et. Men med data binding forbinder man simpelthen properties mellem UI-elementer indbyrdes eller mellem UI-elementer og kode. Når de er bundet, afspejles ændringer i source-propertyen automatisk i target'et.
- Man repræsenterer lister af komplekse modeller på skærmen med en `CollectionView`. En `CollectionView` har en property kaldet `ItemsSource`, der kan bindes til en `List` af items med data binding. `CollectionView` har også en property kaldet `ItemTemplate`, som man tildeler en `DataTemplate`, der definerer hvordan hvert item i samlingen skal vises.

---

## Noter til L02

Kapitlet dækker L02's kerneemner, men bemærk følgende afgrænsninger i forhold til kursets pensum:

- **`INotifyPropertyChanged`** nævnes ikke eksplicit i kapitel 3. Kapitlet opnår automatiske UI-opdateringer via `ObservableCollection<T>` (som implementerer `INotifyCollectionChanged`) og via view-to-view bindings på `BindableProperty`-baserede kontroller. Selve `INotifyPropertyChanged`-interfacet, der er nødvendigt for at en almindelig POCO-property kan notificere UI'et, behandles først i forbindelse med MVVM-mønstret i kapitel 9. <!-- uklart i kilden: interfacet omtales ikke ved navn i dette kapitel -->
- **Binding modes** (`OneWay`, `TwoWay`, `OneWayToSource`, `OneTime`) nævnes heller ikke eksplicit i kapitlet. Two-way binding demonstreres implicit i view-to-view-eksemplerne, hvor `Entry.Text` som source opdaterer `Label.Text` som target, men `Mode`-propertyen på `Binding` diskuteres ikke. <!-- uklart i kilden -->
- **Value converters** nævnes som løsningen på type-mismatch mellem source og target, men henvises til kapitel 8.
- Event handling dækkes gennem `Clicked`-eventen og delegate-signaturen `(object sender, EventArgs e)`, samt `ValueChanged` som det alternativ, data binding gør overflødigt.
