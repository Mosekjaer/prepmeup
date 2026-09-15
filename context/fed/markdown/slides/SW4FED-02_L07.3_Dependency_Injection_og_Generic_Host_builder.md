# Dependency Injection og Generic Host builder pattern i .NET MAUI

## Metadata

- **Lektion:** L07.3 – Dependency Injection and Generic Host builder pattern in .NET MAUI
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L07/Dependency Injection and Generic Host builder pattern.pdf (25 slides)
- **Emner dækket:**
  - Generic Host Builder Pattern og `MauiProgram.cs`
  - Hvad en dependency er, og hvorfor den er et problem
  - Constructor injection og setter injection
  - IoC-containere og Inversion of Control
  - DI-containeren i .NET MAUI
  - Service lifetimes: Singleton, Transient (og Scoped)
  - Registrering af services i `MauiProgram`
  - Custom extension methods til DI
  - Consuming services via constructor injection
  - App lifecycle-states og cross-platform lifecycle events

---

## 1. Generic Host Builder Pattern

.NET MAUI bruger det samme generic host builder pattern, som bruges i andre .NET-projekttyper.

Mønstret giver en ensartet måde at konfigurere og bygge applikationen i .NET — Microsofts tilgang til at indkapsle en applikations krav (konfiguration). For eksempel dependency injection, logging og fonts.

`MauiProgram.cs` initialiserer .NET MAUI-applikationen og bruger Generic Host Builder.

### .NET Generic Host

En host er et objekt, der indkapsler en apps resources og lifetime-funktionalitet:

- Dependency injection (DI)
- Logging
- Configuration
- App shutdown
- `IHostedService`-implementeringer

Hovedgrunden til at samle alle appens indbyrdes afhængige resources i ét objekt er lifetime management — kontrol over app startup og graceful shutdown.

## 2. MauiProgram.cs

`CreateMauiApp` er hovedindgangspunktet i en MAUI-applikation. Metoden `MauiApp.CreateBuilder()` lader dig bootstrappe (konfigurere) din applikation. `MauiProgram`-klassen kalder `CreateMauiApp`-metoden for at oprette et `MauiApp`-builder-objekt.

```csharp
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

        return builder.Build();
    }
}
```

Den underliggende `MauiApp`-type:

```csharp
namespace Microsoft.Maui.Hosting
{
    public sealed class MauiApp : IDisposable
    {
        public IServiceProvider Services { get; }
        public IConfiguration Configuration { get; }

        public static MauiAppBuilder CreateBuilder(bool useDefaults = true);
        ...
    }
}
```

## 3. Hvad er en dependency?

En dependency mellem en source class X og en target class Y opstår, når hver instans af X har brug for en instans af Y.

Problemet er, at X skal vide, hvordan Y-instansen skal oprettes:

```csharp
Class X {
  IY yref;
  public X(){
    yref = new Y();   // Ingen injection her!
  }
  ...
```

## 4. Dependency Injection

Et objekt bør ikke instantiere de objekter, det afhænger af. I stedet bør disse objekter sendes ind "udefra".

De to hovedtyper af injection:

**Constructor injection:**

```csharp
Class X {
  IY yref;
  public X(IY ay)
  {
    yref = ay;
  }
  . . .
```

**Setter injection:**

```csharp
Class X {
  IY yref;
  public X(){
    …
  }
  public void SetY(IY ay)
  {
    yref = ay;
  }
  . . .
```

### Dependency Injection by-hand

En application initializer injicerer en `IY`-implementering ind i en `X`-instans.

Constructor injection:

```csharp
Class App {
  X x;

  public static Main()
  {
    x = new X(new Y());
  }
. . .
```

Setter/property injection:

```csharp
Class App {
  X x;

  public static Main()
  {
    x = new X();
    x.SetY(new Y());
  }
. . .
```

## 5. DI med IoC-containere

I stedet for at hardcode dependencies i hånden lister en komponent blot de nødvendige services, og et DI-framework — kaldet en IoC container — leverer dem.

En IoC container er en komponent, der er ansvarlig for object management og konfigurerer object-grafen. Containere gør det muligt at konfigurere objekter via containeren i stedet for via klientapplikationen.

Ved at specificere dependencies som interface-typer muliggør dependency injection en afkobling af de konkrete typer fra den kode, der afhænger af dem.

### Inversion of Control

Inversion of Control er et princip, som frameworks bruger til at lade udviklere udvide frameworket eller bygge applikationer med det.

Grundidéen er, at frameworket er bevidst om programmørens objekter og foretager invocations på dem. Det er det modsatte af at bruge et API, hvor udviklerens kode foretager kaldene til API-koden.

Frameworks inverterer altså kontrollen: det er ikke udviklerkoden, der har styringen, men frameworket, der foretager kaldene baseret på en stimulus.

IoC kaldes også Hollywood-princippet: *"don't call us, we'll call you"*.

### Inversion of Control Container

IoC-containeren holder en liste af registreringer og mappings mellem interfaces/abstrakte typer og de konkrete typer, der implementerer eller udvider dem.

En Inversion of Control Container bruger IoC-princippet til at styre klassers:

- creation
- destruction
- lifetime
- configuration
- dependencies

Dermed behøver klasser ikke selv at skaffe og konfigurere de klasser, de afhænger af. Det reducerer coupling i systemet dramatisk og forenkler reuse og testability — en container faciliterer testability ved at tillade, at dependencies mockes.

### Dependency Injection Container

Dependency injection-containere reducerer koblingen mellem objekter ved at levere en facilitet til at instantiere klasseinstanser og styre deres lifetime baseret på containerens konfiguration.

Under objektoprettelse injicerer containeren de dependencies, objektet kræver. Er de dependencies ikke oprettet endnu, opretter containeren dem og resolver deres dependencies først.

Fordelene ved at bruge en dependency injection container:

- Containeren fjerner behovet for, at en klasse selv skal lokalisere sine dependencies og styre deres lifetimes.
- Containeren tillader mapping af implementerede dependencies uden at påvirke klassen.
- Containeren faciliterer testability ved at tillade, at dependencies mockes.
- Containeren øger maintainability ved at gøre det let at tilføje nye klasser til appen.

## 6. Dependency Injection i .NET MAUI

.NET MAUI bruger samme generic host builder pattern som andre .NET-projekttyper, f.eks. console- eller ASP.NET Core-applikationer. Den indeholder derfor en indbygget DI-container.

I en .NET MAUI-app, der bruger MVVM, bruges en dependency injection container typisk til:

- At registrere og resolve views
- At registrere og resolve view models
- At registrere services og models og injicere dem i view models

### Registrering af dependencies

I .NET MAUI er `MauiProgram`-klassen stedet, hvor man registrerer komponenter — views, view models og services — til dependency injection.

Alle komponenter, der registreres via `Services`-propertyen, leveres til dependency injection-containeren, når `MauiAppBuilder.Build`-metoden kaldes.

## 7. Service lifetimes i .NET MAUI

Afhængigt af applikationens behov kan services registreres med forskellige lifetimes. Generelt: Transient, Scoped og Singleton — men **Scoped er ikke meningsfuldt i MAUI**, kun i ASP.NET.

| Metode | Beskrivelse |
|---|---|
| `AddSingleton` | Opretter en enkelt instans af objektet, som består i hele applikationens levetid |
| `AddTransient` | Opretter en ny instans af objektet, hver gang der bliver anmodet om det under resolution. Transient-objekter har ingen foruddefineret lifetime, men følger typisk deres hosts lifetime |

### Singleton kontra Transient i Shell-apps

I en Shell-app i .NET MAUI registreres pages, der tilføjes til Shell via XAML eller registreres for routing, automatisk i DI med singleton scope.

For pages uden for Shell og andre dependencies som services og ViewModels skal vi registrere dem manuelt. Shell registrerer automatisk pages som singleton, mens pages uden for Shell bør registreres manuelt som transient.

Hvorfor? I en Shell-app forventer brugeren at kunne navigere hurtigt mellem sider og forventer som regel at se den samme instans af en side, når der navigeres frem og tilbage. Den mest effektive brug af resources er derfor at registrere siden som singleton og lade den blive i memory i appens levetid.

### Hvornår Transient og hvornår Singleton

| Dependency-type | Scope | Begrundelse |
|---|---|---|
| Pages | Transient | Vi bør forvente en ny instans af en side, hver gang vi anmoder om en. At persistere værdier på tværs af forskellige instanser af en side kan give problemer, så det er bedre at persistere state andetsteds og bruge transient-instanser af pages |
| ViewModels | Transient | Ligesom pages bør ViewModels være transient, så vi ved, at vi har en ren instans, hver gang vi får en ny ViewModel |
| Services | Singleton | Vi bør kun forvente én instans af en service i appens levetid. At instantiere flere kopier af en database eller en service, der kommunikerer med et API, er spild af resources og kan føre til datakonflikter. Services bør være singletons og kan derfor være der, hvor vi persisterer app-wide state. Disse singleton-instanser kan injiceres i ViewModels |

Dette er generel vejledning, ikke hugget i sten.

## 8. Registrering af services til DI i MauiProgram

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        })
        .UsePageResolver();

    builder.Services.AddSingleton<IBrowser, AuthBrowser>();
    builder.Services.AddSingleton<IAuthService, AuthService>();
    builder.Services.AddTransient<LoginPage>();
    builder.Services.AddApiClientServices(new ApiClientOptions
    {
        BaseUrl = Constants.BaseUrl
    });
```

Her registreres `AuthBrowser` som implementering af `IBrowser` med singleton scope, og `LoginPage` registreres med transient scope.

## 9. Custom extension method

`AddApiClientServices` er en custom extension method på `IServiceCollection`, som samler registreringen af API-klientens services ét sted:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApiClientServices(this IServiceCollection
                                         services, ApiClientOptions options)
    {
        services.AddSingleton(options);

        services.AddSingleton<AuthHandler>();

        services.AddHttpClient(AuthHandler.AUTHENTICATED_CLIENT)
            .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());

        services.AddSingleton<IInventoryService, InventoryService>();
        services.AddSingleton<IProductService, ProductService>();

        return services;
    }
}
```

## 10. Consuming services

Vi kan konsumere dependencies fra service collectionen i .NET MAUI via constructor injection:

```csharp
public AuthService(IBrowser browser)
{
```

```csharp
public LoginPage(IAuthService authService)
{
    InitializeComponent();
    _authService = authService;
}
```

## 11. App lifecycle

Slidesene viser et state-diagram over app lifecycle states og events:

- Den grå oval angiver, at appen ikke er loadet i memory.
- De lyseblå ovaler angiver, at appen er i memory.
- Tekst på buerne angiver events, som raises af .NET MAUI, og som giver notifikationer til den kørende app.

### Cross-platform lifecycle events

| Event | Beskrivelse | Handling |
|---|---|---|
| `Created` | Raises efter det native vindue er oprettet. På dette tidspunkt har cross-platform-vinduet en native window handler, men vinduet er måske ikke synligt endnu | |
| `Activated` | Raises når vinduet er blevet aktiveret og er eller bliver det fokuserede vindue | |
| `Deactivated` | Raises når vinduet ikke længere er det fokuserede vindue. Vinduet kan dog stadig være synligt | |
| `Stopped` | Raises når vinduet ikke længere er synligt. Der er ingen garanti for, at en app genoptages fra denne state, da den kan blive termineret af operativsystemet | Afbryd forbindelsen til langvarige processer, eller annullér pending requests, der kan forbruge enhedens resources |
| `Resumed` | Raises når en app genoptages efter at have været stopped. Dette event raises ikke første gang appen startes og kan kun raises, hvis `Stopped`-eventet tidligere er raised | Subscribe til nødvendige events, og refresh indhold på den synlige side |
| `Destroying` | Raises når det native vindue destrueres og deallokeres. Det samme cross-platform-vindue kan bruges mod et nyt native vindue, når appen genstartes | Fjern eventuelle event subscriptions, du har knyttet til det native vindue |

### Eksempel: subscribe til Window lifecycle events

For at subscribe til Window lifecycle events overrides `CreateWindow`-metoden i `App`-klassen, så man kan oprette en `Window`-instans og subscribe til dens events:

```csharp
namespace MyMauiApp
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
      Window window = base.CreateWindow(activationState);
      window.Created += (s, e) =>
      {
        // Custom logic
      };
      return window;
    }
  }
}
```

## 12. Referencer og links

- *.NET MAUI in Action*
- Dependency Injection in .NET MAUI:
  - https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection#introduction-to-dependency-injection
  - https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection?view=net-maui-10.0
- App lifecycle in .NET MAUI — https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle?view=net-maui-10.0
