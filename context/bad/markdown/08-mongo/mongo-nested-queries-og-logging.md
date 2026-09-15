---
title: Mongo and C# — Nested Queries and Logging
source: Mongo - nested queries and logging.pdf
course_week: 8-9
topic: MongoDB
---

# Mongo and C#: Nested Queries and Logging

Agenda: using logging with Mongo and Serilog, queries in nested documents from ASP.NET, tips and tricks.

## Logging using Mongo and Serilog

### Installation

From the NuGet package manager, install the following packages:

- `Serilog`
- `Serilog.AspNetCore`
- `Serilog.Settings.Configuration`
- `Serilog.Sinks.MongoDB`

### Change appsettings.json

```json
"Serilog": {
    "MinimumLevel": {
        "Default": "Debug",
        "Override": {
            "Microsoft": "Warning",
            "System": "Warning"
        }
    },
    "WriteTo": [
        {
            "Name": "MongoDBBson",
            "Args": {
                "databaseUrl": "mongodb://localhost:27017/booklogs",
                "collectionName": "log",
                "cappedMaxSizeMb": "50",
                "cappedMaxDocuments": "1000"
            }
        }
        // Add other sinks here if desired...
    ]
},
"AllowedHosts": "*"
```

### Modify Program.cs

```csharp
using Serilog;

// ...after builder is created...

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});
```

This reads the configuration from appsettings / environment variables.

### Adding logging to a controller

Shown here together with a service (using dependency injection, as in the book exercise):

```csharp
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(BooksService booksService, ILogger<BooksController> logger)
    {
        _booksService = booksService;
        _logger = logger;
    }
}
```

### Log an HTTP call — example using GET

```csharp
[HttpGet]
public async Task<List<Book>> GetAll()
{
    var timestamp = new DateTimeOffset(DateTime.UtcNow);
    var logInfo = new { Operation = "Get", Timestamp = timestamp };

    _logger.LogInformation("Get called {@LogInfo} ", logInfo);

    // actual code for doing the operation here...
}
```

The `{@LogInfo}` destructuring operator stores the anonymous object as structured data. The result can be seen in the log collection in Compass — the `LogInfo` object (Operation, Timestamp) is added by our application under the log entry's Properties.

## Queries in nested documents from ASP.NET

### Querying the log — setting it up

- You need to create new endpoints, so you need a new controller using WebAPI to map this to HTTP GET methods.
- You should also create a new service for this (just like the BookService), and remember to register it in `Program.cs`:

```csharp
builder.Services.AddSingleton<LogService>();
```

- You need to create a new Mongo database and a collection for this controller/service to use (you can configure it in appsettings.json — just like for the BookService / REST API example in the previous exercise).

### Map Mongo data to a C# class

Map each nesting "level" that you need to a class. `[BsonIgnoreExtraElements]` means only the fields we want are extracted — the rest of the data is ignored:

```csharp
[BsonIgnoreExtraElements]
public class LogEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String? Id { get; set; }

    [BsonElement("Level")]
    public String Level { get; set; } = "";

    [BsonElement("Properties")]
    public LogProperties Properties { get; set; }
}
```

### Example log endpoint — all logs

In the controller:

```csharp
[HttpGet]
public async Task<List<LogEntry>> Get()
{
    return await _logService.GetAsync();
}
```

In the service:

```csharp
public async Task<List<LogEntry>> GetAsync()
{
    // Get all log entries
    var s = _logsCollection.Find(_ => true);
    var count = s.CountDocuments();
    Console.WriteLine("number of documents found: " + count);
    return await s.ToListAsync();
}
```

### Example log endpoint — filter on operation

In the controller:

```csharp
[HttpGet("{operation}")]
public async Task<List<LogEntry>> GetByOperation(string operation)
{
    Console.WriteLine("operation = " + operation);
    return await _logService.GetAsync(operation);
}
```

In the service:

```csharp
public async Task<List<LogEntry>> GetAsync(string operation)
{
    var query = _logsCollection.Find(x =>
        x.Level.Equals("Information") &&
        x.Properties.logInfo.Operation.ToUpper().Equals(operation.ToUpper()));
    return await query.ToListAsync();
}
```

Notice we can use strongly typed dot notation (with code completion!) into the nested document — because we mapped to strongly typed classes. In this case we also make sure case sensitivity is not an issue, so `get` = `GET` results in a match.

## Tips and tricks

- **TRIPLE check configuration strings** — connection strings, collection names, database names etc. It is very easy to make spelling mistakes or forget an "s".
- **Think about what your queries to the log should do before you choose the data you store in the log.** For instance, if you want to query logs from a specific day (given a year, month and day, return all logs on that day), you need to store the log timestamp in a way that supports that query easily.
- It can be a good idea to give the various GET methods in the same controller different method names (i.e. not call them all just `Get`).

## Authentication

- User secrets in DOTNET: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows
- Authentication/credentials in MongoDB (if a password is set on the database): https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/authentication/
