# L02.1 – Making .NET MAUI apps interactive

## Metadata

- **Lektion:** L02 – Making .NET MAUI apps interactive
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L02/Lectures/MAUI Making App Interactive.pdf (27 slides)
- **Emner dækket:**
  - Cross-platform development og abstraktioner af common features
  - Platforms-mappen og platform specifics
  - App permissions og metadata-filer pr. platform
  - Deklaration af permissions i Android, iOS og Windows
  - Muligheder for lokal datalagring i .NET MAUI
  - SecureStorage og platformenes underliggende krypterings-API'er
  - SQLite og SQLCipher via sqlite-net NuGet-pakker
  - Datamodel med PrimaryKey og AutoIncrement
  - Forbindelse til SQLite-database, tabeloprettelse og CRUD
  - async/await

---

## 1. Cross-platform development i .NET MAUI

.NET MAUI leverer cross-platform development og separerer disse cross-platform API'er i forskellige funktionalitetsområder — dels platform-specific development, dels shared functionality i ét enkelt projekt.

To spørgsmål bærer emnet: hvordan .NET MAUI bruger OS- og device-features til platform-specifics, og hvordan .NET MAUI leverer abstraktioner af common features til shared functionality.

## 2. Abstraktioner af common features

.NET MAUI leverer abstraktioner af common features, så man kan tilgå dem på alle platforme fra én enkelt codebase.

Adgang til common features for device og OS i .NET MAUI er simpelt. Men implementationerne for hver platform er forskellige og kan ændre sig i operativsystemerne med platform specifics.

.NET Essentials leverer et enkelt cross-platform API, der virker med enhver .NET MAUI-applikation, og som kan tilgås fra shared code uanset hvordan user interfacet er lavet.

## 3. Implementering af platform specifics

Et .NET MAUI-app-projekt indeholder en `Platforms`-mappe, hvor hver child-mappe repræsenterer en platform, som .NET MAUI kan targette. Mapperne indeholder for hver platform de platform-specifikke ressourcer og den kode, der starter appen på den pågældende platform.

På build-tidspunktet inkluderer build-systemet kun koden fra hver mappe, når der bygges til netop den platform. Når du fx bygger til Android, bliver filerne i `Platforms/Android` bygget ind i app-pakken, mens filerne i de øvrige `Platforms`-mapper ikke bliver det.

Det gør det muligt at strukturere sit .NET MAUI-projekt, så man ikke behøver placere sin platform-kode i child-mapper under `Platforms`.

Geolocation og sharing bruges i slidesene som eksempler på, hvordan man tilgår features på tværs af enhver understøttet platform.

## 4. App permissions

Det er nødvendigt at bede brugeren om permission, når en app skal tilgå brugerens specifikke oplysninger — fx brugerens location.

Brugerens location er et eksempel på en permission-required feature. Man skal deklarere, at man vil bede om tilladelsen, og på alle platforme skal brugeren eksplicit give samtykke til, at appen tilgår fx deres location.

Hver platform har en specifik metadata-fil, som deklarerer information om appen, som OS'et og distributionsplatformene har brug for. Deklaration af krævede permissions foregår i appens metadata og skal derfor gøres på en platform-specifik måde.

Afhængigt af target-platformen indeholder denne information enten en deklaration af de permissions, appen kræver, eller en deklaration af hvilke capabilities (netværksadgang, location osv.) appen bruger, hvorfra de krævede permissions udledes.

## 5. Metadata-filer pr. platform

App-metadata-filer er alle XML og kan let ændres i en hvilken som helst teksteditor. Man har dog ofte adgang til et grafisk værktøj med visuelle editorer. I Visual Studio kan man højreklikke på metadata-filen (manifest), vælge Open With og derefter XML (Text) Editor i dialogen.

| Platform | Filnavn | Placering |
| --- | --- | --- |
| Android | AndroidManifest.xml | `[Your app]/Platforms/Android` |
| iOS | Info.plist | `[Your app]/Platforms/iOS` |
| macOS | Info.plist | `[Your app]/Platforms/macOS` |
| Windows | package.appxmanifest | `[Your app]/Platforms/Windows` |

## 6. Deklaration af permission i Android-metadata

Åbn `FindMe > Platforms > Android > AndroidManifest.xml` — kan åbnes med XML (Text) Editor ved at højreklikke filen i Visual Studio.

Inde i `<manifest>...</manifest>`-taggene er der tre child-elementer: `uses-sdk`, `application` og `uses-permission`.

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
   <application android:allowBackup="true"
                android:icon="@mipmap/appicon"
                android:roundIcon="@mipmap/appicon_round"
                android:supportsRtl="true">
   </application>
   <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
   <uses-permission android:name="android.permission.INTERNET" />
   <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
   <uses-feature android:name="android.hardware.location" android:required="false" />
   <uses-feature android:name="android.hardware.location.gps" android:required="false" />
</manifest>
```

`uses-permission`- og `uses-feature`-linjerne er dér, hvor permissions — fx location — deklareres.

## 7. Deklaration af permission i iOS-metadata

Åbn iOS-versionen af `Info.plist`. "Plist" er kort for property list. Listen af properties er indkapslet i en dictionary af key-value-par kaldet `<dict>...</dict>`.

Tilføj key-value-parret sidst i dictionary'en i `Info.plist`:

```xml
<dict>
...
...
  <key>NSLocationWhenInUseUsageDescription</key>
  <string>We need your location in order to share it.</string>
</dict>
```

## 8. Deklaration af permission i Windows-metadata

Åbn `Package.appxmanifest`. Nederst i filen tilføjes følgende linje for at bede om tilladelse til at tilgå brugerens location:

```xml
<DeviceCapability Name="location" />
```

Windows-appen vil derefter bede om brugerens location permission.

## 9. Local data og local database

Lokale data skal gemmes i en lokal database. Lokale data kan variere fra "simple" settings over caching af filer/data til et fuldt datasæt.

.NET MAUI leverer flere muligheder for at gemme data lokalt på en enhed:

- **File system** — gemmer løse filer direkte på enheden via filsystemadgang.
- **Database** — gemmer data i en fil optimeret til adgang.
- **Preferences** — gemmer data i key-value-par.
- **Secure Storage** — gemmer data i key-value-par ligesom Preferences, men i en sikker placering på enheden.

## 10. SecureStorage

Den væsentligste feature, der adskiller en .NET MAUI-app, er `SecureStorage`. .NET MAUI leverer `SecureStorage` som en abstraktion.

`SecureStorage` bruger en abstraktion af hver platforms underliggende API til encrypted data management. På macOS og iOS bruger `SecureStorage` KeyChain. På Android bruger den Keystore.

`SecureStorage` bruger key-value-par.

En cryptographic key hentes fra og forvaltes af OS'et til at kryptere værdien af de gemte data, og den er kun tilgængelig for appen — i de fleste tilfælde understøttet af en hardware encryption chip.

`SecureStorage` bruger altså en cryptographic key leveret og forvaltet af OS'et til at kryptere værdien af en entry gemt via Preferences API'et.

## 11. SQLite og SQLCipher

Database-mulighederne i .NET MAUI-apps er betydeligt bredere og mere varierede.

SQLite er en meget udbredt open source database engine og en letvægts cross-platform database, der kan embeddes direkte i applikationer som en lokal database.

.NET MAUI-apps anvender SQLite til at gemme og hente data-objekter ved at tilføje sqlite-net NuGet-pakken, som er en implementation af SQLite.

SQLCipher håndterer kryptering og dekryptering af data ved at bruge SecureStorage API'et og lade OS'et forvalte database encryption key'en.

### Anvendelse af SQLite i en .NET MAUI-app

SQLite tilføjes via SQLite-net-pakkerne: `sqlite-net-pcl` og `sqlite-net-sqlcipher`. SQLite-net-pakkerne giver mulighed for at definere mapping-information i model-klasserne.

I Visual Studio: project -> Manage NuGet package.

Hvis man implementerer en Android-app, kræves en ekstra Android-specifik pakke: tilføj `SQLitePCLRaw.provider.dynamic cdecl` i `[yourappname].csproj`.

Database encryption key'en vil være en GUID, som bliver krypteret og gemt af OS'et og tilgået med .NET MAUI SecureStorage API'et. Selve dataene — fx Todo items — bliver krypteret med SQLCipher.

## 12. Definér datamodel

Definér data model-klasser, der repræsenterer de dataelementer, der skal danne en SQLite database context. Fx `TodoItem`-modelklassen (`TodoItem.cs`, udleveret på Brightspace):

```csharp
public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Due { get; set; }
    public bool Done { get; set; } = false;
}
```

For at kunne bruge auto-increment i en database er det nødvendigt at have både primary key- og auto-increment-attributter.

`PrimaryKey`-attributten identificerer hver entity entydigt — her `Id`-propertyen. `AutoIncrement`-attributten inkrementerer automatisk værdien af entity'en — her `Id` — hver gang et nyt `Id` indsættes i databasen.

## 13. Forbindelse til en SQLite-database

For at forbinde til en SQLite-database bruges:

- `SQLiteConnectionString`-metoden: en helper-metode, en connection string til at forbinde til en database ved at inkludere stien til datafilen.
- `SQLiteAsyncConnection`-metoden: en asynkron metode, der verificerer at en databasetabel eksisterer og opretter den, hvis den ikke gør.
- `Key`: den nøgle, der bruges til at kryptere og dekryptere databasen med SQLCipher.

```csharp
var dataDir = FileSystem.AppDataDirectory;
var databasePath = Path.Combine(dataDir, "[yourappname].db");

string _dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;

if (string.IsNullOrEmpty(_dbEncryptionKey))
{
    Guid g = Guid.NewGuid();
    _dbEncryptionKey = g.ToString();
    SecureStorage.SetAsync("dbKey", _dbEncryptionKey);
}

var dbOptions = new SQLiteConnectionString(databasePath, true, key: _dbEncryptionKey);

_connection = new SQLiteAsyncConnection(dbOptions);
```

Koden stammer fra `Database.cs`, udleveret på Brightspace.

## 14. Opret en tabel i databasen

`CreateTableAsync`-metoden opretter en tabel, der svarer til modelklassen. SQLite genererer tabellen ud fra modelklassens definition, fx `TodoItem`.

Sørg for, at tabellen er oprettet i databasen, før du begynder at læse fra eller skrive til den. Det vil sige, at `CreateTableAsync` skal være gennemført før CRUD-operationer.

```csharp
private async Task Intialise()
{
    await _connection.CreateTableAsync<TodoItem>();
}
```

## 15. Udnyt SQLite-funktionaliteterne

Implementér SQLite-funktionalitet til Create, Read, Update og Delete mod SQLite-databasen.

```csharp
public async Task<List<TodoItem>> GetTodos()
{
     return await _connection.Table<TodoItem>().ToListAsync();
}
public async Task<TodoItem> GetTodo(int id)
{
     var query = _connection.Table<TodoItem>().Where(t => t.Id == id);
     return await query.FirstOrDefaultAsync();
}
public async Task<int> Addtodo(TodoItem item)
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
```

`GetTodos` er read all, `GetTodo` er read specific, `Addtodo` er create, `DeleteTodo` er delete og `UpdateTodo` er update. Koden stammer fra `Database.cs`, udleveret på Brightspace.

## 16. Async og await

`async`/`await` er en C#-feature, der gør det muligt at skrive asynkron kode — én form for multithreading.

Asynkron programmering blokerer ikke main thread. Hvis operationen er en long-running operation, sikrer asynkron programmering, at appen ikke låser (fryser).

Med `async`/`await` sikrer man, at appen ikke blokeres fra at fortsætte med at køre — den kan arbejde på andre tasks (threads), mens databasetabellen oprettes, eller mens items skrives, opdateres, læses eller slettes.

## 17. References & Links

- *.NET MAUI in Action* af Matt Goldman
- .NET MAUI Platform Specifics and Platform integration:
  https://learn.microsoft.com/en-us/dotnet/maui/windows/platform-specifics/
  https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/
- .NET MAUI Permissions: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/appmodel/permissions?tabs=windows
- .NET MAUI Local database: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite
