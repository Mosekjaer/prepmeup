# Note – Brug af SQLite i MAUI apps

## Metadata

- **Lektion:** L03 – Lokal datalagring i MAUI
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing
- **Kilde:** Brightspace-note "Brug af SQLite i MAUI apps"
- **Emner dækket:**
  - Hvad SQLite er, og hvorfor den bruges på en device
  - Valg mellem SQLite-net-pcl og Microsoft.EntityFrameworkCore.Sqlite
  - Installation af NuGet-pakker
  - Kryptering med sqlite-net-sqlcipher
  - SQLiteNetExtensions til relationer
  - SQLiteStudio som database tool
  - Håndtering af exceptions omkring databasen

---

## 1. Hvorfor bruge SQLite på en device?

SQLite er en databasemotor. Dvs. det er ikke en selvstændig app; snarere er det et bibliotek, som softwareudviklere integrerer i deres apps. Som sådan tilhører den familien af indlejrede databaser. Det er den mest udbredte databasemotor, da den bruges utroligt mange steder. SQLite blev designet til at kunne bruges uden at installere et databasestyringssystem eller kræve en databaseadministrator.

Mange programmeringssprog har bindinger til SQLite-biblioteket. Det følger generelt PostgreSQL-syntaks, men gennemtvinger ikke typekontrol som standard.

Du kan læse mere om SQLite her: <https://en.wikipedia.org/wiki/SQLite> og på den officielle side: <https://www.sqlite.org/>

## 2. Valg af library

I .NET-verdenen er der 2 forskellige NuGet-pakker (der er flere, men 2 som er mest relevante), som kan bruges til at tilgå en SQLite-database fra en MAUI app: **SQLite-net-pcl** og **Microsoft.EntityFrameworkCore.Sqlite**.

### SQLite-net-pcl

Er den mindste, så din app kan blive lille og effektiv. Ulemperne ved denne pakke er, at den har en meget tynd wrapper (simpel ORM), men simple CRUD-operationer er hurtige og lette at programmere. Bemærk at den ikke understøtter relationer (foreign keys). Den funktionalitet skal du selv implementere på applikationsniveau. Der findes dog en overbygning til denne pakke, som implementerer en mini-ORM med en-til-mange og mange-til-mange relationer. **Men API'et er ikke, som det du lærer i SW4BAD.**

### Microsoft.EntityFrameworkCore.Sqlite

Denne pakke er noget større, men fordelen ved den er, at den implementerer EF Core, så du kan bruge LINQ-udtryk til at kommunikere med databasen på samme måde, som du lærer i SW4BAD.

## 3. Installation

### Installation af SQLite-net-pcl

```bash
dotnet add package sqlite-net-pcl
```

Hvis du vil kryptere databasen:

```bash
dotnet add package sqlite-net-sqlcipher
```

Læs her hvordan SQLite bruges: <https://github.com/praeclarum/sqlite-net>

### Extension til relationer

```bash
dotnet add package SQLiteNetExtensions.Async
```

Læs her hvordan den bruges: <https://bitbucket.org/twincoders/sqlite-net-extensions/src/master/>

### Installation af EntityFrameworkCore.Sqlite

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

## 4. Database tools

**SQLiteStudio** er en open source desktop-applikation til at browse og editere en SQLite-databasefil. Du kan se hvor din applikation lagrer databasefilen, og så åbne den i SQLiteStudio.

Download herfra: <https://sqlitestudio.pl/>

Bemærk, at du kun kan bruge SQLiteStudio (og tilsvarende tools), hvis du **IKKE** krypterer databasen. Derfor anbefales det ikke at kryptere databasen, mens man udvikler appen.

Hvis du skal bruge SQLiteStudio til at undersøge, hvad der er i databasen, så får du brug for stien til databasen. Det er derfor smart at udskrive den i konsollen.

## 5. Exceptions

Husk at interaktion med databaser ofte giver anledning til exceptions. Og da MAUI nogle gange skjuler den underliggende exception og bare stopper med en "unhandled exception", er det vigtigt at fange database-exceptions og udskrive dem, som vist i det efterfølgende kodeeksempel fra `03-MauiTodo-demo`.

### Kodeeksempel: Database-klassen

```csharp
internal class Database
{
    private readonly SQLiteAsyncConnection _connection;

    public Database()
    {
        var dataDir = FileSystem.AppDataDirectory;
        var databasePath = Path.Combine(dataDir, "PERsMauiTodo.db");
        SQLiteConnectionString? dbOptions = null;

        // Delete if you need a fresh start
        //if (File.Exists(databasePath))
        //{
        //    File.Delete(databasePath);
        //}

#if DEBUG
        // Don't use encryption when run in debug mode (development)
        Console.WriteLine($"Database path: {databasePath}");
        dbOptions = new SQLiteConnectionString(databasePath, true);
#else
        // Use encryption when run in production
        string _dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;

        if (string.IsNullOrEmpty(_dbEncryptionKey))
        {
            Guid g = new Guid();
            _dbEncryptionKey = g.ToString();
            SecureStorage.SetAsync("dbKey", _dbEncryptionKey);
        }
        dbOptions = new SQLiteConnectionString(databasePath, true, key: _dbEncryptionKey);
#endif
        try
        {
            _connection = new SQLiteAsyncConnection(dbOptions);
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"SQLiteException: {ex.Message}");
            Console.WriteLine($"SQLiteException: {ex.InnerException}");
            Console.WriteLine($"SQLiteException: {ex.StackTrace}");
        }
        if (_connection == null)
        {
            Console.WriteLine("Database connection is wrong");
            _connection = new SQLiteAsyncConnection("");
        }
        _ = Initialise();
    }

    private async Task Initialise()
    {
        try
        {
            await _connection.CreateTableAsync<TodoItem>();
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"SQLiteException: {ex.Message}");
            Console.WriteLine($"SQLiteException: {ex.InnerException}");
            Console.WriteLine($"SQLiteException: {ex.StackTrace}");
        }
    }
}
```

Bemærk mønsteret: i DEBUG køres uden kryptering, så databasen kan inspiceres med SQLiteStudio, mens produktionsbuildet henter (eller genererer) en nøgle i `SecureStorage` og bruger den til at kryptere databasefilen.

Se også den tilsvarende `Database`-klasse fra lab 02 i [kode/SW4FED-02_Lab02_MauiTodo_kode.md](../kode/SW4FED-02_Lab02_MauiTodo_kode.md).
