# Lab 02 – MauiTodo: Database og TodoItem

## Metadata

- **Lektion:** L02 – Lokal datalagring med SQLite
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing
- **Kilde:** `L02/Lab/Database.cs` og `L02/Lab/TodoItem.cs` (udleveret kode til FED Lab 02 MauiTodo)
- **Emner dækket:**
  - Model-klasse til en todo-post
  - SQLite-forbindelse via `SQLiteAsyncConnection`
  - Krypteringsnøgle gemt i `SecureStorage`
  - Asynkron oprettelse af tabel
  - CRUD-metoder mod SQLite

---

## Kontekst

Dette er den udleverede kode til **lab 02 (MauiTodo)** – todo-appen der bruges som gennemgående eksempel i MAUI-delen af kurset. To klasser i namespacet `MauiTodo.Models`:

- **`TodoItem`** er modelklassen. Den er en ren POCO med et `Id`, en `Title`, en forfaldsdato `Due` og et `Done`-flag der defaulter til `false`. `Id` bliver primærnøgle i SQLite-tabellen.
- **`Database`** er data access-laget. Konstruktøren finder appens datamappe via `FileSystem.AppDataDirectory`, henter (eller genererer og gemmer) en krypteringsnøgle i `SecureStorage`, og åbner en krypteret `SQLiteAsyncConnection`. Tabellen oprettes asynkront i `Intialise()`, som konstruktøren kalder uden at afvente (`_ = Intialise()`), fordi en konstruktør ikke kan være `async`. Resten af klassen er de fem CRUD-operationer, som alle er `async` og returnerer `Task`.

Bemærk at denne version altid krypterer databasen. I underviserens demo-version (se [noter/SW4FED-02_Note_SQLite_i_MAUI_apps.md](../noter/SW4FED-02_Note_SQLite_i_MAUI_apps.md)) er kryptering slået fra i DEBUG, så databasefilen kan inspiceres med SQLiteStudio. Bemærk også stavefejlen `Intialise` og metodenavnet `Addtodo`, som er i den udleverede kode.

## TodoItem.cs

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

## Database.cs

```csharp
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTodo.Models
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _connection;

        public Database()
        {
            var dataDir = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(dataDir, "MauiTodo.db");

            string _dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;

            if (string.IsNullOrEmpty(_dbEncryptionKey))
            {
                Guid g = Guid.NewGuid();
                _dbEncryptionKey = g.ToString();
                SecureStorage.SetAsync("dbKey", _dbEncryptionKey);
            }

            var dbOptions = new SQLiteConnectionString(databasePath, true, key: _dbEncryptionKey);

            _connection = new SQLiteAsyncConnection(dbOptions);
            _ = Intialise();
        }

        private async Task Intialise()
        {
            await _connection.CreateTableAsync<TodoItem>();
        }

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

    }
}
```
