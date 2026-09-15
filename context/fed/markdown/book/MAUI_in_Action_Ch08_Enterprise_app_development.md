# Kapitel 8 — Enterprise app development

## Metadata

- **Kapitel:** 8 — Enterprise app development
- **Bog:** .NET MAUI in Action — Matt Goldman, Manning
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Relateret lektion:** L07
- **Hovedemner:**
  - Flytning af business logic ud af code-behind og ind i services
  - Definition af krav via interfaces (dependency inversion) og mock-implementationer
  - Authentication med OAuth2/OIDC: IdentityServer, `WebAuthenticator`, `OidcClient`
  - Registrering af custom URL schemes pr. platform (Android, iOS, macOS, Windows)
  - Generic host builder pattern og den indbyggede DI-container i .NET MAUI
  - Service lifetimes: transient for pages og ViewModels, singleton for services
  - Constructor injection og `PageResolver`-pakken til DI-opløst navigation
  - Consuming web services: REST, autogenererede NSwag-klienter, DTOs
  - Delegating handlers og `IHttpClientFactory` til named `HttpClient`-instanser
  - Full-stack arkitektur: Clean Architecture, cross-cutting concerns og code sharing

---

.NET MAUI er ofte valgt, fordi det passer ind i en eksisterende .NET-stack. Mange af de patterns og praksisser, man kender fra andre .NET-projekttyper, kan bruges direkte i .NET MAUI-apps. Dette kapitel gennemgår abstraktion af logik til interfaces og services, dependency injection (DI), authentication, samt hvordan man forenkler lokal udvikling af full-stack cloud-baserede løsninger med .NET MAUI-klienter. Konkret ses der på, hvordan `MauiStockTake.UI` slotter ind i en eksisterende .NET API-løsning, og hvordan kode deles mellem lagene.

---

## 8.1 Moving logic to services

`MauiStockTake`-appen fra kapitel 7 har en side- og navigationsstruktur defineret i sin Shell, men gør endnu ikke noget. Der mangler funktionalitet til at søge efter produkter og til at tælle lagerbeholdning.

Indtil nu har al funktionalitet ligget i code-behind-filerne til XAML-siderne. Det er ikke en god idé af især to grunde:

- **Det kobler UI'et tæt til business logic.** Business logic i code-behind på en `Page` gør applikationen skrøbelig. Ændringer i UI'et kan påvirke business logic — og omvendt.
- **Det tillader ikke code re-use.** Logik skrevet i en `Page` kan ikke bruges af andre sider. Hvis flere sider har brug for produktinformation, skal de hver især implementere logikken selv.

I `MauiStockTake` implementeres funktionaliteten i stedet i **services**. Det løser begge problemer: business logic fjernes fra UI'et (koblingen brydes), og logikken kan bruges, hvor der er brug for den, uden at skulle reimplementeres.

Før implementeringen defineres kravene — ved at fastslå, hvilken funktionalitet appen har brug for, og derefter skrive **interfaces**, der beskriver denne funktionalitet.

### 8.1.1 Defining requirements

Det første, brugeren skal kunne, når appen åbnes, er at logge ind. Derfor tilføjes en login-`Button` til `LoginPage`. Samtidig ændres layoutet fra `VerticalStackLayout` til `FlexLayout` for bedre at arrangere views, og teksten på `Label` ændres fra "Login Page" til "MauiStockTake". Mellem `Label` og `Button` placeres appens logo (samme `surfshack_logo.jpeg` som i flyout'et). Endelig pakkes hele layoutet i et `Grid` med en `ActivityIndicator`, så brugeren kan se, at der sker noget, mens login foregår.

**Listing 8.1 The updated LoginPage.xaml**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
x:Class="MauiStockTake.UI.Pages.LoginPage"
Title="LoginPage">
<Grid>
<FlexLayout JustifyContent="SpaceAround"
Direction="Column"
AlignItems="Center"
HorizontalOptions="Center"
VerticalOptions="Center">
<Image Source="surfshack_logo.jpeg"
WidthRequest="200"
HeightRequest="200"
HorizontalOptions="Center"
VerticalOptions="Center">
<Image.Clip>
<EllipseGeometry Center="100,100"
RadiusX="100"
RadiusY="100"/>
</Image.Clip>
</Image>
<Label Text="MauiStockTake"
FontSize="Title"
VerticalOptions="Center"
HorizontalOptions="Center" />
<Button Text="Login"
HorizontalOptions="Center"
VerticalOptions="Center"
Clicked="LoginButton_Clicked"
x:Name="LoginButton"/>
</FlexLayout>
<ActivityIndicator x:Name="LoggingIn"
IsRunning="True"
IsVisible="false"/>
</Grid>
</ContentPage>
```

① `Direction` sættes til `Column`, så child-elementerne arrangeres lodret.
② `AlignItems` sættes til `Center`, så child-elementerne centreres langs main-aksen (retningen af flex-flowet).

Login-`Button` har en `Clicked`-event handler defineret, som skal tilføjes til `LoginPage.xaml.cs`.

**Listing 8.2 The LoginButton_Clicked method**

```csharp
private void LoginButton_Clicked(object sender, EventArgs e)
{
}
```

Dermed er det første krav defineret: der er en login-knap, altså er der et krav om funktionalitet, der lader brugeren logge ind. Når man bruger **dependency inversion principle** (se "MVVM for SOLID apps" i næste kapitel), defineres krav dér, hvor de *konsumeres* — ikke dér, hvor de leveres. Derfor starter man med at definere et interface, der beskriver den funktionalitet, `LoginPage` har brug for.

**Figure 8.1** illustrerer pointen: `LoginPage` har en login-knap, knappen udløser en event handler i code-behind, og event handleren håndterer *ikke* selv login-processen — den kalder i stedet en `Login`-metode i en authentication service.

I `MauiStockTake.UI`-projektet oprettes en ny mappe `Services`, og heri et interface `IAuthService.cs`. Implementationen skal lade brugeren logge ind, så der tilføjes en metode `LoginAsync`. Der er brug for at vide, om login lykkedes, så metoden returnerer en `bool` (dette kunne håndteres mere elegant, men er tilstrækkeligt her). Login-processen kommunikerer med REST-API'et, og den kommunikation skal ske på en background thread, så UI'et ikke fryser mens der ventes på svar — derfor er returtypen en `Task`.

**Listing 8.3 The IAuthService interface**

```csharp
namespace MauiStockTake.UI.Services;
public interface IAuthService
{
    Task<bool> LoginAsync();
}
```

Namespacet skal bruges mange steder i appen, så det tilføjes til `GlobalUsings.cs`:

```csharp
global using MauiStockTake.UI.Services;
```

Appen kan endnu ikke køres, fordi to problemer mangler at blive løst: der findes ingen implementation af interfacet, og selvom der er registreret en route til `LoginPage`, har brugeren ingen måde at navigere derhen.

En rigtig implementation kommer i næste afsnit, men indtil videre oprettes en **mock-implementation**. I `Services`-mappen oprettes klassen `MockAuthService`, som implementerer `IAuthService` og blot returnerer et `Task`-resultat på `true`.

**Listing 8.4 MockAuthService**

```csharp
namespace MauiStockTake.UI.Services;
public class MockAuthService : IAuthService
{
    public Task<bool> LoginAsync() => Task.FromResult(true);
}
```

> **Mocking interfaces**
> Mock-implementationen af `IAuthService` sikrer, at man ikke bliver blokeret af manglende rigtig implementation. Ved at definere kravene i interfaces undgår man at blive blokeret, mens man bygger et UI, der afhænger af funktionalitet, som endnu ikke findes — man kan levere en mock i mellemtiden.
> En mock hjælper ikke med noget, der reelt afhænger af, at brugeren er logget ind (fx authenticated API-kald). Men den gør det muligt at bygge, køre og teste UI'et, og den er også nyttig til unit testing senere.

Med mock-implementationen på plads kan `LoginButton_Clicked` få funktionalitet. I `LoginPage.xaml.cs` tilføjes et felt til `IAuthService`, og i konstruktøren tildeles en ny instans af `MockAuthService`. I event handleren deaktiveres login-knappen først, så brugeren ikke kan trykke igen, mens login er i gang (irrelevant for mock'en, men god praksis). Derefter kaldes `Login`-metoden i authentication servicen; ved succes sendes brugeren ind i appen, ellers vises en advarsel.

**Listing 8.5 LoginPage.xaml.cs**

```csharp
namespace MauiStockTake.UI.Pages;
public partial class LoginPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginPage()
    {
        InitializeComponent();
        _authService = new MockAuthService();
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        LoginButton.IsEnabled = false;
        LoggingIn.IsVisible = true;

        var loggedIn = await _authService.LoginAsync();

        LoggingIn.IsVisible = false;
        if (!loggedIn)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Something went wrong logging you in. Please try again.", "OK");
            LoginButton.IsEnabled = true;
        }
        else
        {
            // TODO: navigate back to the app
        }
    }
}
```

① Tilføjer et felt til `IAuthService`-interfacet.
② Tildeler en ny instans af `MockAuthService` til feltet.
③ I event handleren deaktiveres login-knappen.
④ `ActivityIndicator` gøres synlig, så brugeren ser, at login er i gang.
⑤ `LoginAsync` kaldes på `IAuthService`, og resultatet gemmes i en midlertidig variabel.
⑥ Hvis login ikke lykkes, vises en advarsel, `ActivityIndicator` skjules, og knappen aktiveres igen.

TODO-kommentaren står, hvor login lykkes. Problemet er, at `LoginPage` slet ikke vises endnu, så der er intet at gå tilbage fra. Login er det første, brugeren skal gøre, så siden bør vises automatisk ved appstart. Der er registreret en route til `LoginPage`, men den route skal fjernes — siden skal fremkomme som del af appens **lifecycle**, ikke være navigerbar.

Løsningen er `OnStart` lifecycle-metoden kombineret med hierarkisk navigation, der pusher `LoginPage` som en **modal page** på navigation stacken. Modsat en `NavigationPage` giver en modal page ikke navigations-UI (dvs. ingen back-knap), hvilket signalerer til brugeren, at handlingerne på siden bør fuldføres, før man navigerer væk. Det passer godt til login. På Android findes der naturligvis en hardware- eller OS-leveret back-knap, så brugeren kan stadig lukke siden — men signalet er der. Funktionaliteten kan overrides, hvis nødvendigt.

Navigation stacken styres af en property kaldet `Navigation`, som `Page` arver fra basisklassen `NavigableElement`. Appens navigation stack videregives by reference til den side, der er tildelt appens `MainPage`-property, så den vej kan bruges til at kalde navigationsmetoder.

**Listing 8.6 The OnStart functionality to add to App.xaml.cs**

```csharp
protected override async void OnStart()
{
    base.OnStart();
    await MainPage.Navigation.PushModalAsync(
        new LoginPage());
}
```

① Kalder `PushModalAsync` på `Navigation`-propertyen på appens `MainPage` og sender en ny instans af `LoginPage` med.

Når brugeren er logget ind, kan siden poppes for at vende tilbage til Shell'en. `LoginPage` arver også `NavigableElement` og har derfor selv en `Navigation`-property med en reference til appens navigation stack. Derfor kan `PopModalAsync` kaldes derfra. TODO-kommentaren i `LoginPage.xaml.cs` erstattes med:

**Listing 8.7 The code to add to LoginPage.xaml.cs in place of the TODO**

```csharp
await Navigation.PopModalAsync();
```

Kører man appen nu, popper `LoginPage` automatisk op. Trykker man på login-knappen, forsvinder siden, og man er tilbage i app-Shell'en.

**Figure 8.2** viser resultatet: til venstre er `LoginPage` pushed som modal side på navigation stacken automatisk ved appstart — bemærk, at appen ikke leverer en back-knap. Klikkes der på Login, poppes `LoginPage` programmatisk, og den underliggende Shell kommer til syne.

### 8.1.2 Implementing the authentication service

`MauiStockTake`-API'et bruger **IdentityServer**, et OpenID Connect (OIDC)-kompatibelt framework til ASP.NET Core-applikationer. OIDC er en udvidelse af OAuth2 og lader brugere authenticate via deres webbrowser og få et token, der giver adgang til beskyttede ressourcer. Der skal derfor bygges en authentication service i .NET MAUI-appen, som kan logge brugere ind via IdentityServer og hente et **JSON web token (JWT)**, der kan bruges til at authenticate API-kald.

> **Identity, authentication, and authorization**
> Identity er et enormt emne og uden for bogens scope, men vigtigt at have kendskab til. Nogle apps kræver hverken authentication eller brugerkonti, men det er et lille mindretal.
> Man behøver ikke bruge IdentityServer. Det er blevet stadig mere populært at outsource identity til en cloud-baseret identity provider (IDP) som Azure Active Directory B2C eller Auth0. IdentityServer bruges her, fordi det er inkluderet i ASP.NET Core-templates og derfor ikke kræver oprettelse hos en tredjepart. De fleste cloud-IDP'er tilbyder veldokumenterede klientpakker eller SDK'er, som ofte også håndterer noget af den funktionalitet, der her bygges manuelt — fx sikker opbevaring af refresh tokens.
> Outsourcer man identity, er det lettere at komme i gang med de dokumenterede klientpakker; men vil man bruge IdentityServer, virker fremgangsmåden her — og den fungerer også med enhver OIDC-kompatibel IDP, inklusive de fleste kommercielle cloud-tilbud.

Med OAuth2-authentication sendes brugernavn og password ikke fra appen til IDP'en. I stedet åbner appen en webbrowser på IDP'ens login-side. Metoden anses for mere sikker, da appen aldrig får adgang til brugerens password; brugeren logger ind direkte hos IDP'en og bliver derefter redirected tilbage til appen med en **code**, der kan veksles til et access token.

I implementationen af `IAuthService` bruges en kombination af den indbyggede `WebAuthenticator` i .NET MAUI og NuGet-pakken `IdentityModel.OidcClient`. `OidcClient` er lavet af de samme folk som IdentityServer og forenkler parsingen af det OAuth2-svar, IdentityServer returnerer.

**Figure 8.3** beskriver flowet i `MauiStockTake` i seks trin: (1) `LoginAsync` kaldes på `OidcClient`-pakken. (2) `OidcClient` kalder `WebAuthenticator` og viser login-siden på IdentityServer. (3) Brugeren logger ind på IdentityServer. (4) IdentityServer returnerer resultatet via `WebAuthenticator`. (5) Det parsede resultat returneres til `OidcClient`. (6) `OidcClient` udtrækker JWT'et af resultatet og returnerer det til kalderen af `LoginAsync`.

`OidcClient` bruges til at parse et OIDC-svar, men leverer ikke selv en måde at sende brugeren til IDP'ens login-side. Når man opretter en `OidcClient`-instans, skal man levere en implementation af `OidcClient`s `IBrowser`-interface, som definerer en metode til netop dette. Der bygges derfor en implementation, som bruger `WebAuthenticator` til at gennemføre login og sende resultatet tilbage til `OidcClient`, der udtrækker de nødvendige tokens.

**NOTE** Der skal bruges det fuldt kvalificerede navn for `IBrowser`, da .NET MAUI også har et interface ved navn `IBrowser` med et andet formål.

Når man bruger et OAuth2 login-flow med en webbrowser, angives en **redirect URI** som del af requestet. Efter succesfuld authentication sender IDP'en brugeren tilbage til denne URI sammen med en authorization code, der kan veksles til tokens. Her skal brugeren ikke redirectes til et website, men til appen. Det gøres ved at registrere et **custom URL scheme** hos operativsystemet, så OS'et ved, at alle URL'er med det scheme skal sendes til appen.

`http://` og `https://` er velkendte URL schemes. Åbner man en URL, håndteres den af den applikation, der er registreret hos OS'et for det pågældende scheme — for HTTP og HTTPS er det standardbrowseren. Med et custom scheme kan appen associeres, så enhver URL, der begynder med det scheme, åbnes i appen.

**Figure 8.4** viser pointen: ved authentication mod en IDP leveres en redirect URL, og ved succes sendes svaret tilbage til den. For en webapplikation er redirecten typisk adressen på den webapplikation, hvor authentication-requestet stammer fra. For en mobil- eller desktop-app bruger URL'en et custom scheme, som OS'et har associeret med appen, så svaret fra IDP'en sendes til appen.

Applikationen bruger `auth.com.mildredsurf.stocktake://callback` som redirect URI. Her er `auth.com.mildredsurf.stocktake` selve schemet, som skal registreres på alle målplatforme. Processen er lidt forskellig pr. platform.

#### Android URL registration

Registrering af et custom URL scheme på Android kræver to trin. Først oprettes en `Activity`, der modtager web-callbacket. Dernæst tilføjes en intent til manifestet, som tillader at åbne browseren.

Først `Activity`'en. Tilføj filen `WebCallbackActivity.cs` i `Platforms/Android`.

**Listing 8.8 WebCallbackActivity.cs**

```csharp
using Android.App;
using Android.Content;
using Android.Content.PM;
namespace MauiStockTake.UI.Platforms.Android;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
DataScheme = "auth.com.mildredsurf.stocktake",
DataHost = "callback")]
public class WebCallbackActivity : WebAuthenticatorCallbackActivity
{
}
```

Dernæst fortælles Android, at appen bruger **custom tabs**. Custom tabs ligger et sted mellem en embedded webview og at skifte til en anden browser-app. Koden tilføjes til `AndroidManifest.xml` (i `Platforms/Android`-mappen) før det lukkende `</manifest>`-tag.

**Listing 8.9 AndroidManifest.xml**

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
<uses-sdk android:minSdkVersion="21" android:targetSdkVersion="30" />
<application android:allowBackup="true" android:icon="@mipmap/appicon"
android:roundIcon="@mipmap/appicon_round" android:supportsRtl="true"></application>
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<queries>
<intent>
<action
android:name="android.support.customtabs.action.CustomTabsService" />
</intent>
</queries>
</manifest>
```

#### iOS and Mac custom URL registration

Processen for iOS og macOS er den samme; trinene gentages i platform-mapperne for både iOS og Mac Catalyst.

Først registreres det custom URL scheme i `info.plist` (findes i `Platforms/iOS` og `Platforms/MacCatalyst`). Bruger man Visual Studio, åbnes filen med plist-editoren, hvorefter man går til fanen Advanced, folder noden **URL Types** ud og klikker **Add URL type**.

**Table 8.1 The values to add to info.plist**

| Field | Value |
| --- | --- |
| Identifier | Auth |
| URL Schemes | auth.com.mildredsurf.stocktake |
| Role | Viewer |

Bruger man ikke Visual Studio — eller vil man tilføje værdierne manuelt — åbnes `info.plist` i en teksteditor. Inde i `<dict>...</dict>`-taggene tilføjes en ny entry med nøglen `CFBundleURLTypes`. Værdien er et array, der også indeholder en dictionary. Tilføjes før det lukkende `</dict>`-tag.

**Listing 8.10 Custom URL definition in Info.plist for iOS and macOS**

```xml
<key>CFBundleURLTypes</key>
<array>
<dict>
<key>CFBundleURLName</key>
<string>Auth</string>
<key>CFBundleURLSchemes</key>
<array>
<string>auth.com.mildredsurf.stocktake</string>
</array>
<key>CFBundleTypeRole</key>
<string>Viewer</string>
</dict>
</array>
```

Husk at gentage trinene i **både** `iOS`- og `MacCatalyst`-platformmapperne.

#### Windows custom URL scheme registration

På Windows opdateres `Package.appxmanifest`. Åbn filen i en teksteditor (i Visual Studio: højreklik og vælg View Code, eller marker filen i Solution Explorer og tryk F7). Find `Applications`-noden og `Application`-noden med id'et `App`. Koden indsættes umiddelbart efter det åbnende `<Application ...>`-tag.

**Listing 8.11 Custom URL definition in Package.appxmanifest for Windows**

```xml
<Extensions>
<uap:Extension Category="windows.protocol">
<uap:Protocol Name="auth.com.mildredsurf.stocktake">
<uap:DisplayName>Auth</uap:DisplayName>
</uap:Protocol>
</uap:Extension>
</Extensions>
```

#### Defining the IdentityServer values

Når man interagerer med en OAuth2-IDP, skal nogle værdier være defineret: URI'en på IDP'en, client ID (som repræsenterer en client defineret i IDP'en — her `MauiStockTake`-appen), de scopes, der anmodes om adgang til, og redirect URI'en. Alle værdier gøres til konstanter, og klassen gøres static, så de er lette at tilgå overalt i appen.

Tilføj en ny klasse `Constants` i roden af `MauiStockTake.UI`. URL'erne i koden er ngrok-URL'er genereret til test (se appendiks A); de skal erstattes med ens egne.

**Listing 8.12 Constants.cs**

```csharp
namespace MauiStockTake.UI;
public static class Constants
{
    public const string BaseUrl = "https://8284-159-196-124-207.ngrok.io";

    public const string RedirectUri = "auth.com.mildredsurf.stocktake://callback";

    public const string AuthorityUri = "https://8284-159-196-124-207.ngrok.io";

    public const string ClientId = "com.mildredsurf.stocktake";

    public const string Scope = "openid profile offline_access MauiStockTake.WebUIAPI";
}
```

① `BaseUrl` bruges som base-adresse for kald til API'et.
② `RedirectUri` sendes til IdentityServer med login-requestet, så et succesfuldt login kan returneres til appen. Bemærk, at URL-schemet er det, der blev registreret tidligere.
③ `AuthorityUri` bruges til authentication-requests. Her er den identisk med API-URI'en, fordi API'et kører med IdentityServer indbygget.
④ `ClientId` skal matche et gyldigt client ID i IDP'en.
⑤ `Scope` indeholder et space-adskilt array af scopes, som alle skal være gyldige for IDP'en.

#### Add the AuthBrowser

`AuthBrowser`-klassen implementerer `IBrowser`-interfacet fra `OidcClient`-pakken, som derfor installeres først. Installer `IdentityModel.OidcClient` i `MauiStockTake.UI`. Opret dernæst mappen `Helpers` med klassen `AuthBrowser`.

Da .NET MAUI også har et `IBrowser`-interface, skal det fuldt kvalificerede navn inklusive namespace bruges. `IBrowser` definerer metoden `InvokeAsync`, som skal implementeres.

Metoden returnerer typen `BrowserResult`, der har en enkelt string-property `Response` med de værdier, IDP'en returnerede, formateret så `OidcClient` kan parse dem. I `InvokeAsync` kaldes `WebAuthenticator`, som leverer værdierne fra IDP'en i en dictionary kaldet `Properties`. En hjælpemetode læser disse og formaterer dem til den string, `OidcClient` forventer.

**Listing 8.13 The AuthBrowser class**

```csharp
using IdentityModel.OidcClient.Browser;
namespace MauiStockTake.UI.Helpers;
public class AuthBrowser : IdentityModel.OidcClient.Browser.IBrowser
{
    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(Constants.RedirectUri));

        return new BrowserResult()
        {
            Response = ParseAuthenticationResult(authResult)
        };
    }

    private string ParseAuthenticationResult(WebAuthenticatorResult result)
    {
        string code = result?.Properties["code"];
        string scope = result?.Properties["scope"];
        string state = result?.Properties["state"];
        string sessionState = result?.Properties["session_state"];
        return $"{Constants.RedirectUri}#code={code}&scope={scope}&state={state}&session_state={sessionState}";
    }
}
```

① Erklærer, at klassen implementerer `IBrowser` fra `OidcClient`-pakken.
② Implementerer `InvokeAsync` defineret i `IBrowser`.
③ Kalder `AuthenticateAsync` på `WebAuthenticator` for at åbne en browser på IDP'ens login-side. `OidcClient` kalder metoden og sender options med, der inkluderer IDP-URL'en, men redirect URI'en tages fra `Constants`.
④ Returnerer en instans af den `BrowserResult`-type, `OidcClient` forventer.
⑤ Tildeler en string til `Response`-propertyen på `BrowserResult`, dannet ved at sende resultatet fra `WebAuthenticator` til `ParseAuthenticationResult`.
⑥ Definerer `ParseAuthenticationResult`, som returnerer en string og tager et `WebAuthenticatorResult`.
⑦ Returnerer en formateret string med værdier fra `Properties`-dictionary'en på `WebAuthenticatorResult`.

Dermed er `AuthBrowser` færdig, og der findes nu en måde at åbne en webbrowser på IDP'ens login-side og returnere et formateret resultat, `OidcClient` kan behandle.

**NOTE** På skrivetidspunktet virker `WebAuthenticator` endnu ikke på Windows, men pakken `WinUIEx` indeholder sin egen `WebAuthenticator`, der kan bruges i stedet.

#### Adding the AuthService

I `Services`-mappen oprettes klassen `AuthService`, som implementerer `IAuthService` — der lige nu definerer en enkelt metode, `LoginAsync`.

`LoginAsync` implementeres ved hjælp af `OidcClient`. Når klienten instantieres, kræver konstruktøren nogle options, som bygges af en kombination af værdier fra `Constants` og `IBrowser`-implementationen.

`OidcClient` bruger `WebAuthenticator` til at starte en browsersession og opfange den returnerede session state. Den centrale del til authentication mod API'et er **access token**. Foreløbig tjekkes resultatet blot for fejl: er der ingen, er login lykkedes, der er et access token, og metoden returnerer `true` — ellers `false`.

**Listing 8.14 The AuthService class**

```csharp
using IdentityModel.OidcClient;
using MauiStockTake.UI.Helpers;
namespace MauiStockTake.UI.Services;
public class AuthService : IAuthService
{
    private readonly OidcClientOptions _options;

    public AuthService()
    {
        _options = new OidcClientOptions
        {
            Authority   = Constants.AuthorityUri,
            ClientId    = Constants.ClientId,
            Scope       = Constants.Scope,
            RedirectUri = Constants.RedirectUri,
            Browser     = new AuthBrowser()
        };
    }

    public async Task<bool> LoginAsync()
    {
        var oidcClient = new OidcClient(_options);

        var loginResult = await oidcClient.LoginAsync(new LoginRequest());

        if (loginResult.IsError)
        {
            // TODO: inspect and handle error
            return false;
        }

        return true;
    }
}
```

① Erklærer, at klassen implementerer `IAuthService`.
② Tilføjer et felt til at gemme `OidcClientOptions`.
③ I konstruktøren tildeles en ny instans af `OidcClientOptions` til feltet med værdier fra `Constants` og `AuthBrowser` som `IBrowser`-implementation.
④ Implementerer `LoginAsync` som async.
⑤ Opretter en ny `OidcClient`-instans med de gemte options.
⑥ Kalder `LoginAsync` på `OidcClient` og gemmer resultatet i en midlertidig variabel.
⑦ Tjekker om resultatet er en fejl; er det tilfældet returneres `false`, ellers fortsættes til `return true`.

Nu hvor der findes en rigtig implementation af `IAuthService`, opdateres konstruktøren i `LoginPage` code-behind, så `MockAuthService` erstattes:

```csharp
_authService = new AuthService();
```

Følger man opsætningen i appendiks A, kan API'et nu køres, og ngrok (eller en anden tunnel) kan give en offentligt routbar URL. Sørg for, at URL'en står i `Constants` for både authority og base URL.

Kører man appen, pushes login-siden automatisk som modal. Klikkes der på login-knappen, åbnes på iOS og Android et browservindue inde i appen, mens Windows og macOS åbner standardbrowseren på IdentityServers login-side. API'et opretter automatisk en default-konto med brugernavnet `administrator@localhost` og password `Administrator1!`; man kan logge ind med disse eller registrere en konto.

Efter login forsvinder browservinduet på iOS og Android; på macOS og Windows spørger browseren om tilladelse til at åbne `MauiStockTake`-appen (den skal godkendes). Derefter er man tilbage i appen, og login-siden poppes af stacken præcis som med `MockAuthService`.

Sætter man et breakpoint i `AuthService` på linjen, der tjekker `loginResult`, kan man inspicere resultatet og se et access token, et ID token og et refresh token.

---

## 8.2 Using the generic host builder and dependency injection

.NET MAUI bruger det samme **generic host builder pattern**, som bruges i andre .NET-projekttyper som console- eller ASP.NET Core-applikationer. Derfor indeholder MAUI også en indbygget **DI-container**.

**NOTE** Er man ikke fortrolig med DI, anbefales *Dependency Injection Principles, Practices, and Patterns* af Steven van Deursen og Mark Seemann (Manning, 2019).

I `LoginPage` code-behind og i `AuthService` "newes" der en række dependencies op for login-processen, som burde kunne resolves fra DI-containeren (`IBrowser`-implementationen i `AuthService` og `AuthService` i `LoginPage`).

Før services eller dependencies kan konsumeres fra DI-containeren, skal de registreres — og til registrering skal deres **scope** bestemmes. I en ASP.NET Core-applikation findes en klart defineret HTTP request pipeline, hvor hvert request har et scope, hvilket ofte kan afgøre valget.

I en UI-app fungerer det anderledes. Da der ikke er nogen HTTP request pipeline, giver `AddScoped` ikke mening — der findes ingen requests at scope efter. Tilbage står to muligheder: **transient** eller **singleton**.

I en Shell-app i .NET MAUI registreres sider, der tilføjes til Shell'en via XAML eller registreres til routing (fx `InputPage`, `ReportPage` og `ProductPage`), automatisk i DI med **singleton**-scope. Men sider uden for Shell (fx `LoginPage`) og andre dependencies som services og ViewModels skal registreres manuelt.

**Table 8.2 Scope lifetimes for different dependencies in a .NET MAUI app**

| Dependency type | Scope | Reasoning |
| --- | --- | --- |
| Pages | Transient | Man bør forvente en ny instans af en side, hver gang der anmodes om en. At persistere værdier på tværs af forskellige instanser af en side kan give problemer, så det er bedre at persistere state andetsteds og bruge transient sideinstanser. |
| ViewModels | Transient | Ligesom sider bør ViewModels være transient, så man ved, at man får en ren instans hver gang. |
| Services | Singleton | Man bør kun forvente én instans af en service i appens levetid. At instantiere flere kopier af en database eller en service, der kommunikerer med et API, er spild af ressourcer og kan føre til datakonflikter. Services bør være singletons og kan derfor være dér, hvor app-omfattende state persisteres. Disse singleton-instanser kan injectes i ViewModels. |

> **Why are we scoping pages differently to Shell?**
> Shell registrerer automatisk sider som singleton, mens anbefalingen her er at registrere sider manuelt som transient. Spørgsmålet handler reelt om UX og brugerforventninger.
> I en Shell-app forventer brugeren at navigere hurtigt mellem sider og typisk se den samme instans af en side frem og tilbage. Den mest effektive ressourceudnyttelse er derfor at registrere siden som singleton og holde den i hukommelsen i appens levetid.
> Med andre navigationsparadigmer, som hierarkisk (og windowed, der behandles i kapitel 10), kan en enkelt instans af en side eller dens state være problematisk. Brugeren kan forvente en frisk instans hver gang, og at holde sjældent brugte elementer i hukommelsen hele appens levetid er ineffektivt.
> Facebook-appen er et eksempel: home-fanen viser newsfeed'et, og hvis det genindlæste, hver gang man skiftede fane, ville det være dårlig UX. Men trykker man på en profil i feed'et, pushes den hierarkisk på stacken — og her giver en frisk instans hver gang mening, ellers ville siden holde forældede data fra tidligere besøgte profiler.

Med host builder-patternet kan pages, ViewModels og services registreres som i en ASP.NET Core- eller console-app. Men i .NET MAUI kan man også registrere andre ting.

### 8.2.1 Registering resources, services, and other dependencies

Der er allerede erfaring med at registrere resources: i kapitel 4 blev en LCD-font tilføjet til `MauiCalc`, og i kapitel 6 blev en icon-font registreret til Outlook-replikaen.

I `MauiProgram`-klassen ses, at metoden `CreateMauiApp` returnerer en `MauiApp`. Metoden kaldes af frameworket for at oprette en instans af appen til hver målplatform. Inde i metoden bruges generic host builder-patternet til at generere den `MauiApp`, der returneres.

Extension-metoden `ConfigureFonts` gør det muligt at registrere fonte. Har man tidligere brugt Xamarin.Forms, vil man værdsætte, hvor meget enklere registrering er i .NET MAUI. Andre extension-metoder bruges til at registrere handlers og animationer (begge behandles i kapitel 11).

Der skal ikke registreres fonte i `MauiStockTake`, men der er dependencies. Host builderen i `MauiProgram` er en midlertidig variabel `builder` af typen `MauiAppBuilder`. Den har en property `Services` af typen `IServiceCollection`, som bruges til at registrere dependencies, og som frameworket bruger til at resolve dem.

Arbejder man baglæns, ses det, at `AuthBrowser` ingen eksterne dependencies har. Den implementerer et interface defineret af `OidcClient` og bruger `WebAuthenticator` fra .NET MAUI. Derfor registreres denne implementation først.

I `MauiProgram.cs`, efter `ConfigureFonts`-kaldet og før builderen returneres, registreres `AuthBrowser` som implementation af `IBrowser` med singleton-scope:

```csharp
builder.Services.AddSingleton<IBrowser, AuthBrowser>();
```

De nødvendige namespaces skal med, men tilføjer man linjen som den er, opstår en fejl på grund af det duplikerede `IBrowser`-navn i .NET MAUI og `OidcClient`. Løsningen er enten det fuldt kvalificerede navn eller — for læsbarhedens skyld — en alias-erklæring øverst i filen:

```csharp
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
```

Næste led i kæden er `AuthService`, som implementerer `IAuthService` og også registreres som singleton:

```csharp
builder.Services.AddSingleton<IAuthService, AuthService>();
```

De fleste sider styres af Shell, men `LoginPage` er en modal navigationsside, som derfor registreres manuelt med transient scope:

```csharp
builder.Services.AddTransient<LoginPage>();
```

**Listing 8.15 MauiProgram.cs with the dependency registrations**

```csharp
using MauiStockTake.UI.Helpers;
using MauiStockTake.UI.Pages;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
namespace MauiStockTake.UI;
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
            });

        builder.Services.AddSingleton<IBrowser, AuthBrowser>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddTransient<LoginPage>();

        return builder.Build();
    }
}
```

### 8.2.2 Consuming services

Dependencies kan konsumeres fra service collection i .NET MAUI via **constructor injection**, præcis som i enhver anden .NET-applikation. `AuthService` har en dependency på `IBrowser`, men den er kun synlig ved at læse koden, da den ikke er constructor-injected. Nu hvor den er registreret, kan den injectes og tildeles `_options`-feltet.

I `AuthService` ændres konstruktøren, så den tager en injected instans af `IBrowser` og tildeler den til `Browser`-propertyen på `_options`. Det samme `using`-alias som i `MauiProgram` skal tilføjes for at løse tvetydigheden mellem de to `IBrowser`-interfaces.

**Listing 8.16 The updated AuthService constructor**

```csharp
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
...
public AuthService(IBrowser browser)
{
    _options = new OidcClientOptions
    {
        Authority   = Constants.AuthorityUri,
        ClientId    = Constants.ClientId,
        Scope       = Constants.Scope,
        RedirectUri = Constants.RedirectUri,
        Browser     = browser
    };
}
```

Ændringen af `AuthService`-konstruktøren giver en fejl i `LoginPage`-konstruktøren. Der tildeles i øjeblikket en `new AuthService()`-instans til `_authService`-feltet, men `AuthService` har ikke længere en default-konstruktør — den har nu en synlig dependency på `IBrowser`. Man kunne ændre til `_authService = new AuthService(new AuthBrowser)`, men det er bedre blot at injecte `IAuthService` i `LoginPage`. Så bliver service collection ansvarlig for at levere en fuldt resolvet `IAuthService`-implementation, og man overholder dependency inversion principle ved at afhænge af de krav, `LoginPage` definerer, snarere end af en konkret implementation.

**Listing 8.17 The updated LoginPage constructor**

```csharp
public LoginPage(IAuthService authService)
{
    InitializeComponent();
    _authService = authService;
}
```

Ændringen af `LoginPage`-konstruktøren giver til gengæld en fejl i `App.xaml.cs` i `OnStart`, hvor en ny instans af `LoginPage` sendes til `PushModalAsync` — igen en afhængighed af en default-konstruktør, der ikke længere findes.

Problemet kan løses på flere måder. Man kunne tilføje en static property af typen `IServiceCollection` til `MauiProgram` og tildele `Services`-propertyen fra `MauiAppBuilder`, hvorefter den kunne kaldes overalt for at resolve dependencies — men det er **service locator-antipatternet**. Alternativt kunne `LoginPage` injectes i `App`-klassen, gemmes i et felt og sendes til `PushModalAsync`.

En bedre løsning er dog NuGet-pakken **PageResolver** (skrevet af bogens forfatter). Med den kan man navigere til sider *by type*, med siden som type-argument. Plugin'et navigerer derefter til en fuldt resolvet instans af siden med alle dens dependencies.

Installer `Goldie.MauiPlugins.PageResolver` i `MauiStockTake.UI`. Pakken skal bruges flere steder, så den tilføjes til `GlobalUsings`:

```csharp
global using Maui.Plugins.PageResolver;
```

Nu kan koden i `App.xaml.cs` bruge den forenklede navigationsmetode og sende `LoginPage` som type-parameter uden at bekymre sig om dependencies:

```csharp
await MainPage.Navigation.PushModalAsync<LoginPage>();
```

Sidste trin er at registrere `PageResolver` med generic host builderen, som giver den service collection til at resolve dependencies. I `MauiProgram.cs` tilføjes `UsePageResolver()` til det fluent `UseMauiApp`-kald.

**Listing 8.18 The updated UseMauiApp method**

```csharp
builder
    .UseMauiApp<App>()
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    })
    .UsePageResolver();
```

Alt er nu på plads til automatisk at resolve og konsumere alle `LoginPage`s dependencies. Funktionaliteten er uændret i forhold til før, men koden er nu mere vedligeholdelsesvenlig.

---

## 8.3 Consuming web services

Mange apps er selvforsynende og har ingen eksterne dependencies, men enterprise-apps og de fleste succesfulde consumer-apps kommunikerer med et API. Der findes mange teknologier til det — REST, SignalR, GraphQL og gRPC. `MauiStockTake` bruger REST, da det stadig er mest udbredt.

Med REST repræsenterer API'et ressourcer, og klienter interagerer med dem via HTTP-verberne (GET, POST, PUT, PATCH og DELETE). Payloads sendes typisk i JSON-format, hvilket lader API'er og klienter kommunikere uden hensyn til, hvilken teknologi hver af dem bruger bag kulisserne.

I en .NET-applikation kan man bruge en extension-metode i namespacet `System.Net.Http.Json` til at kalde et REST-endpoint via `HttpClient` og deserialisere JSON-svaret til en .NET-type:

```csharp
var product = await _httpClient.GetFromJsonAsync<Product>("product");
```

Her er `_httpClient` en `HttpClient`-instans med defineret `BaseUrl` (hvilket gør `"product"` til en route).

Tilgangen er god og er allerede brugt et par gange (i Outlook-replikaen og `MauiMovies`). For større applikationer kan den blive svær at vedligeholde, men i `MauiStockTake` bruges **Clean Architecture (CA)**, som anvender **NSwag** til automatisk at generere klienter for hver ressource.

> **Autogenerated clients**
> NSwag er et bekvemt værktøj til at generere klient-klassebiblioteker til .NET-API'er. For et trivielt API som `MauiStockTake` er det formentlig ikke nødvendigt, men det bruges her, fordi det er indbygget i CA-templaten. Efterhånden som løsninger vokser i kompleksitet, og antallet af routes stiger, vokser værdien af en autogenereret klient eksponentielt.
> Foretrækker man det ikke, kan man skrive sine egne metoder mod specifikationen. API-specifikationen kan udforskes ved at køre WebAPI-projektet og tilføje `/api` til URL'en, hvilket åbner Swagger UI.

Selve REST-klientimplementationerne er allerede på plads i `MauiStockTake`-løsningen. Dette afsnit handler om at wire dem op i .NET MAUI-appen.

### 8.3.1 Adding the client project

`MauiStockTake`-løsningen har et klassebiblioteksprojekt `MauiStockTake.Client`. I `Helpers`-mappen ligger en autogenereret fil med klient-klasser til REST-endpoints. I `Services`-mappen findes services til produkt- og inventory-ressourcerne sammen med interfaces, der kan injectes, og som udgør en brugbar wrapper omkring klienterne.

`MauiStockTake.Client` skal tilføjes som dependency på `MauiStockTake.UI`. I Visual Studio: højreklik **Dependencies** under `MauiStockTake.UI` og vælg **Add Project Reference...**, sæt flueben ved `MauiStockTake.Client` og klik OK. Åbner man `MauiStockTake.UI.csproj`, ses følgende linjer tilføjet:

```xml
<ItemGroup>
<ProjectReference Include="..\MauiStockTake.Client\MauiStockTake.Client.csproj" />
</ItemGroup>
```

Bruger man ikke Visual Studio, kan de tilføjes manuelt.

`MauiStockTake.Client` afhænger af et andet projekt i løsningen ved navn `Shared`, som indeholder **data transfer objects (DTOs)** brugt af både API'et og klientprojektet. Et af dem hedder `ProductDto` og er næsten identisk med den `Product`-klasse, der blev oprettet i `Models`-mappen i `MauiStockTake.UI`. Den klasse er ikke længere nødvendig, så både `Product`-klassen og `Models`-mappen kan slettes.

Sletningerne giver fejl i `ProductPage.xaml.cs` og `InputPage.xaml.cs`. Løsningen: fjern `using`-statementet for `Models`-namespacet i hver side og erstat det med `using` for `MauiStockTake.Shared.Products`. Opdater derefter i begge code-behind-filer alle forekomster af `Product` til `ProductDto`. Byg `MauiStockTake.UI` og sikr, at der ikke er fejl.

### 8.3.2 Using a delegating handler

`MauiStockTake`-API'et forventer, at requests inkluderer et access token som header; requests til beskyttede routes uden token bliver ikke autoriseret. Det er ligetil at tilføje headeren til en `HttpRequestMessage`, men det bliver hurtigt uoverskueligt, hvis det skal gøres ved hvert eneste API-kald.

I stedet kan man bruge en **delegating handler**. En delegating handler kan associeres med en `HttpClient`-instans og modificere hvert eneste HTTP-request fra den klient, så man ikke selv skal gøre det ved hvert kald.

I `MauiStockTake.Client` er en delegating handler allerede sat op. I `Authentication`-mappen ligger klassen `AuthHandler`, som nedarver fra basisklassen `DelegatingHandler` og overrider `SendAsync`. Logikken i metoden er simpel: tilføj en header til requestet med access token'et og kald derefter base-metoden.

Klassen har også to yderligere medlemmer. Det første er en static string, der skal indeholde access token'ets værdi. Den overridede `SendAsync` bruger den til at hæfte på requestet, men værdien bliver i øjeblikket ikke sat.

Ser man tilbage på `AuthService` i `MauiStockTake.UI`, returnerer `LoginAsync` blot `true` ved succes. Koden opdateres, så access token'et tildeles den static string i `AuthHandler`. Da det er en static string, kan den refereres via klassenavn og medlemsnavn uden en instans.

**Listing 8.19 The updated LoginAsync method of AuthService**

```csharp
using MauiStockTake.Client.Authentication;
...
public async Task<bool> LoginAsync()
{
    var oidcClient = new OidcClient(_options);
    var loginResult = await oidcClient.LoginAsync(new LoginRequest());
    if (loginResult.IsError)
    {
        // TODO: inspect and handle error
        return false;
    }

    AuthHandler.AuthToken = loginResult.AccessToken;

    return true;
}
```

Den delegating handler er nu klar til at modificere ethvert HTTP-request, så det inkluderer access token'et. Tilbage står at give API-klientklasserne adgang til en `HttpClient`-instans med denne handler tilknyttet.

### 8.3.3 Using IHttpClientFactory

Microsoft leverer NuGet-pakken `Microsoft.Extensions.Http`, som muliggør brug af `HttpClient` med DI (pakken er allerede installeret i `MauiStockTake.Client`). Via en extension-metode i pakken kan man kalde `AddHttpClient` for at tilføje en singleton-instans af `HttpClient` til service collection.

Metoden kan kaldes flere gange for at tilføje flere `HttpClient`-instanser, hver med sit eget formål og refereret med et unikt navn (man kan også registrere `HttpClient`-instanser for specifikke typer). Når man har brug for en `HttpClient`, injecter man interfacet `IHttpClientFactory` i sin klasse og kalder dens `CreateClient`-metode for at resolve den navngivne instans fra service collection.

I `AuthHandler` findes en `const string` ved navn `AUTHENTICATED_CLIENT`. Dens formål er at give et navn, som kan refereres overalt i appen, til den `HttpClient`-instans, der tilføjer token'et til HTTP-requests.

I roden af `MauiStockTake.Client` ligger filen `DependencyInjection` med metoden `AddApiClientServices`. I metoden tilføjes den delegating handler til service collection:

```csharp
services.AddSingleton<AuthHandler>();
```

Umiddelbart efter registreres en `HttpClient`-instans, navngivet med konstanten fra `AuthHandler`-klassen og med `AuthHandler` tilføjet til sin handler chain:

```csharp
services.AddHttpClient(AuthHandler.AUTHENTICATED_CLIENT)
    .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());
```

Koden registrerer en `HttpClient`-instans i service collection, som kan konsumeres overalt i appen, og som har en handler tilknyttet, der hæfter access token'et på ethvert HTTP-request lavet med klienten.

> **Chaining delegating handlers**
> Man kan gøre lige så meget, man vil, for at manipulere `HttpRequestMessage` i en delegating handler, før base-metoden kaldes for at sende den. Her er der kun én modifikation (at hæfte access token'et på), men der kan være tilfælde med behov for flere — fx specifikke headers afhængigt af bestemte kriterier.
> Alle ændringer kan laves i én delegating handler, men det er bedre at oprette specifikke handlers til hver modifikation. `AddHttpMessageHandler` kan kaldes flere gange for at kæde handlers sammen.
> Det overholder single responsibility principle bedre og giver mere fleksibilitet. Handlers behandles i den rækkefølge, de tilføjes, så man kan også styre rækkefølgen af modifikationerne.

I `Services`-mappen i `MauiStockTake.Client` ligger tre filer:

- `BaseService`
- `InventoryService`
- `ProductService`

`BaseService` indeholder en basisklasse, der forventer `IHttpClientFactory` injected. Basisservicen har et felt til en `HttpClient`-instans. Konstruktøren anmoder om en instans af den navngivne klient — med navnet defineret i `AuthHandler` — og tildeler den til feltet:

```csharp
public BaseService(IHttpClientFactory httpClientFactory, ApiClientOptions options)
{
    _httpClient = httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);
    _baseUrl = options.BaseUrl;
}
```

Konstruktøren forventer også en klasse `ApiClientOptions`, som bruges til at udfylde et base URL-felt.

Da alle services bruger det samme token, nedarver `InventoryService` og `ProductService` fra basisklassen. Deres konstruktører skal også tage de dependencies, basisklassen kræver, og videresende dem til base-konstruktøren, men de behøver ikke selv anmode om `HttpClient`-instansen fra `IHttpClientFactory`.

Ser man på konstruktøren i enten `InventoryService` eller `ProductService`, bruger de denne authenticated klient til at oprette en instans af den REST-ressource-specifikke klient, hver service anvender. Ved at bruge en managed `HttpClient`-instans får man et access token til at lave authenticated requests mod ethvert endpoint i API'et uden at skulle håndtere det ved hvert enkelt request.

### 8.3.4 Adding the remaining MauiStockTake services

I `MauiStockTake.UI` er det allerede vist, hvordan services og dependencies wires op i `IServiceCollection`. I `MauiProgram` er `AuthService` registreret som implementation af `IAuthService`.

`MauiStockTake.Client` har også interfaces defineret samt implementationer. Disse dependencies er allerede registreret i `DependencyInjection`-klassen, så det eneste, der skal gøres for at bruge dem i `MauiStockTake.UI`, er at tilføje den eksisterende dependency-registrering.

`AddApiClientServices` er en extension-metode på `IServiceCollection` og kan derfor registreres i `MauiProgram`. Registreringen af service-interfaces og deres implementationer sker allerede i metoden, så et kald i `MauiProgram` gør disse services tilgængelige i appen.

Da det er en extension-metode, kaldes den på en eksisterende `IServiceCollection` i stedet for at få den sendt ind. Metoden forventer dog også en parameter af typen `ApiClientOptions`. I `MauiProgram` kaldes extension-metoden på `Services`-propertyen på `builder`-variablen, og der sendes en ny `ApiClientOptions`-instans med `BaseUrl` fra `Constants`-klassen.

**Listing 8.20 Registering the client in MauiProgram**

```csharp
builder.Services.AddApiClientServices(new ApiClientOptions
{
    BaseUrl = Constants.BaseUrl
});
```

Med `MauiStockTake.Client` registreret kan service-interfaces injectes i klasser og bruges til at tale med API'et. Authentication håndteres af `AuthHandler`, og services er wired op til at bruge en `HttpClient`-instans, der anvender denne handler. I næste kapitel færdiggøres selve stock-taking-funktionaliteten ved hjælp af disse services.

---

## 8.4 Full-stack app architecture

`MauiStockTake` er ikke en selvstændig mobil- og desktopapplikation. Den er del af en større løsning, der omfatter cloud/web-komponenter og en database, og den kræver authentication for at sikre kommunikationen mellem .NET MAUI-appen og cloud-API'et. Dette afsnit handler om at organisere løsningen, så code sharing og effektivitet maksimeres.

### 8.4.1 Project Organization

`MauiStockTake`-API'et er baseret på **Clean Architecture (CA)**-templaten og følger clean architecture-principperne, men de code-sharing-teknikker, kapitlet dækker, gør det muligt at integrere et .NET MAUI-UI i enhver full-stack .NET-arkitektur.

**Figure 8.5** viser organiseringen af API-projekterne: med CA peger alle dependencies **indad**. Core (Domain og Application) indeholder entiteter og business logic. Application afhænger af Domain, og Domain har ingen dependencies. Infrastructure og Presentation afhænger af Application — og .NET MAUI-appen er en del af Presentation.

CA-patternet anvendt på backend-API'et er uden for bogens scope. Det vigtige er ikke arkitekturens detaljer, men at man følger principperne og et design pattern for at maksimere code re-use på tværs af hele stacken.

> **You don't have to use Clean Architecture**
> CA bruges i `MauiStockTake`, fordi det giver en logisk, struktureret måde at organisere kode i løsningen på, og fordi strukturen illustrerer, hvordan kode kan deles mellem API'et og .NET MAUI-appen.
> Der findes masser af alternative arkitekturer; nogle passer måske bedre til ens scenarie, eller man bryder sig simpelthen ikke om CA (det gør mange ikke). **Vertical slice architecture** er særligt i vælten lige nu. Minimal APIs vinder også frem.
> Uanset om man bruger CA eller en anden arkitektur, er principperne for code sharing på tværs af stacken de samme, og teknikkerne her virker lige så godt med andre arkitekturer.

Løsningen er arrangeret i mapper, der repræsenterer CA's lag. I `Presentation`-mappen findes:

- **WebAPI**
- **MauiStockTake.Maui** (tilføjet i forrige kapitel)
- **Client**

#### WebAPI

`WebAPI` er et ASP.NET Core-projekt, der leverer REST-controllers og endpoints, som lader omverdenen kommunikere med business logic og data i API'et. .NET MAUI-appen kommunikerer med dette over HTTP for at interagere med resten af løsningen.

#### MauiStockTake.UI

`MauiStockTake.Maui` er .NET MAUI-projektet, der blev tilføjet i forrige kapitel. Her bygges den app, Mildred og hendes team bruger til at optælle lagerbeholdning.

#### MauiStockTake.Client

`MauiStockTake.Client` er et klassebibliotek med typer og logik, der kan bruges i et .NET UI-projekt til at interagere med API'et. Det indeholder DTOs og services samt noget autogenereret klientkode.

Alt i projektet er nødvendigt for at få klientappen til at virke, men det er boilerplate-kode, som ikke er en del af .NET MAUI-appen i sig selv. Ville man i fremtiden tilføje en Blazor-webapp, kunne Blazor-appen genbruge det samme klassebibliotek og undgå at duplikere kode, der gør det samme.

### 8.4.2 Sharing code between projects in the solution

I CA er det en streng regel, at dependencies peger indad. Det er også en streng regel, at Domain ikke har eksterne dependencies. Typisk er Application en del af Core sammen med Domain, og Application må gerne have dependencies — bare ikke sådanne, der peger udad.

Dele af løsningen, der løber vinkelret på dependency-flowet, kaldes **cross-cutting concerns**. I `MauiStockTake` er der cross-cutting concerns i form af DTOs, som kræves af Application-projektet, WebAPI-projektet og .NET MAUI-appen (sidstnævnte får dem dog via API-klientprojektet).

**Figure 8.6** viser pointen: `MauiStockTake` er en full-stack-løsning skrevet i C#, og der er derfor ingen grund til at duplikere kode — den kan deles på tværs af stacken. Kode delt på denne måde kaldes cross-cutting concerns.

I `MauiStockTake` ligger disse DTOs i et projekt kaldet `Shared` (i en solution-mappe kaldet `Common`). `Shared` er dependency for Application-projektet, WebAPI-projektet og `MauiStockTake.Client`. Med denne tilgang kan kode deles ubesværet mellem front- og backend.

Fordelene er betydelige. Det overholder **don't repeat yourself**-princippet, hvilket ikke bare gør livet lettere, men også giver sikkerhed for, at ændringer ét sted i løsningen slår igennem øjeblikkeligt andre steder. Ændrer man strukturen på en DTO, er den samme DTO allerede i brug overalt, så eventuelle breaking changes bliver straks synlige. Den slags ændringer er langt vanskeligere, når lagene i stacken udvikles uafhængigt.

Da der bygges en full-stack .NET-løsning, kan man gå et skridt videre. Beslutter man sig senere for at tilføje et web-UI, kan man bruge Blazor, og `MauiStockTake.Client`-pakken kan bruges i Blazor-UI'et også. Det giver en full-stack cloud-, mobil-, desktop- og webløsning, der maksimerer code re-use på tværs af hele stacken.

### 8.4.3 Sharing code between solutions

Som selvstændig løsning er `MauiStockTake` allerede velarkitektureret til maksimal code re-use. Ofte er den software, man bygger, dog del af et **enterprise-økosystem**. Enterprise-logik udgør ofte kernen i sådanne økosystemer og leverer logik og typer, der er relevante på tværs af hele virksomheden — og dermed adskilt fra domæne- og problemspecifikke typer.

Forestil man sig en suite af applikationer i en virksomhed, hver med sit eget forretningsområde. De ville alle have egne krav, men sandsynligvis dele noget fælles funktionalitet — det mest oplagte eksempel er brugerhåndtering og authentication.

Ud over business logic er det vigtigt, at disse applikationer bevarer en konsistent UX, så de føles som del af en sammenhængende helhed — især for eksternt vendte produkter. .NET MAUI gør det nemt.

**Figure 8.7** viser et eksempel: `MauiStockTake`-appen bliver del af et økosystem, hvis Mildred også introducerer en kundeportal, som blandt andet kan kræve en indikation af produkttilgængelighed. Kundeportalen ville sandsynligvis have sit eget API, der kunne forespørge `MauiStockTake`-API'et om denne information. Kundeportalen kunne bygges i Blazor, hvilket ville tillade, at dens controls abstraheres tilbage til et Razor class library. Det class library kunne bruges af .NET MAUI Blazor til også at levere kundeportalen som app. Controls i `MauiStockTake`-appen kunne tilsvarende abstraheres tilbage til et .NET MAUI class library og deles med fremtidige apps, hvilket ville bevare den visuelle konsistens i Mildreds brand.

I det scenarie har Mildreds virksomhed en suite af applikationer, både kundevendte og interne, som ikke bare kan dele business logic (hvor relevant), men også UI og UX. Hele økosystemet er brudt ned i moduler, der kan deles på tværs af løsninger. En almindelig tilgang er at bundle disse moduler som NuGet-pakker og hoste dem på et privat feed. GitHub tilbyder NuGet-pakkehosting (offentlig og privat), og mange andre produkter kan integrere med ens foretrukne DevOps- eller CI/CD-platform.

På det punkt har man nået code sharing-nirvana i et .NET-enterprise-økosystem: business logic og UI delt på tværs af hele virksomheden — i skyen, på desktop og i browseren.

---

## Summary

- At flytte logik ud af UI'et og ind i services gør det muligt at dele logikken på tværs af en løsning.
- Man kan definere krav til sit UI ved at oprette et interface og derefter skrive en service, der implementerer det. Det er et eksempel på **dependency inversion principle**.
- `WebAuthenticator` i .NET MAUI gør det nemt at authenticate via OAuth. Den åbner browsersessionen og returnerer token-svaret til koden.
- Man kan registrere et **custom URL scheme** hos OS'et for sin app, hvilket lader en webbrowser (eller enhver anden applikation) route til appen via en URL.
- .NET MAUI bruger det **generic host builder pattern**, der bruges på tværs af alle .NET-applikationstyper.
- Via host builderen kan man registrere fonte til brug i sine .NET MAUI-apps.
- Generic host builder-patternet giver adgang til den indbyggede services collection. Den bruges til at registrere dependencies og injecte dem i klasser via deres konstruktører.
- Man kan oprette en **delegating handler**, der automatisk hæfter en header med et access token på ethvert HTTP-request lavet af en `HttpClient`.
- Man kan registrere en navngiven `HttpClient`-instans sammen med en delegating handler i services collection. `IHttpClientFactory` kan injectes i enhver klasse og bruges til at få adgang til den navngivne `HttpClient`-instans.
- Med .NET MAUI er det nemt at dele kode på tværs af hele stacken. DTOs kan placeres i et delt projekt, som både front- og backend har adgang til.
- Man kan endda dele business logic. I `MauiStockTake` kunne klientpakken fx også bruges af et Blazor-UI.
- Deling af logik og UI på tværs af en virksomhed er også nemt med ASP.NET Core, .NET MAUI og Blazor, med velkendte CI/CD- og DevOps-værktøjer.
