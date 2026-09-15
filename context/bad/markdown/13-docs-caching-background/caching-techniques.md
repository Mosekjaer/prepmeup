---
title: Caching Techniques
source: Caching techniques.pdf
course_week: 12-13
topic: API-dokumentation, background services og caching
---

# Caching Techniques — "Cache is King!"

## What is caching?

The term *cache* describes a hardware component or a software mechanism that can be used to store data so that future requests that require such data can be served faster and without being retrieved from scratch.

## Benefits of caching

- Performance
- Lower latency
- Less CPU overhead
- Reduced bandwidth use
- Decreased costs
- Lower carbon footprint

## The downside of caching

- Adds some complexity
- Might cause unwanted side effects — like serving outdated data

## What to cache?

A RESTful web API should use:

- HTTP response caching for static assets and frequently called endpoints that return the same response
- An in-memory and/or distributed server-side caching strategy to relieve the burden on the DBMS

A well-crafted web application often adopts a combination of all these techniques to deal with the needs of each of its various sections in the best possible way. Caches can live at every layer: client, proxy server, web server (response cache / in-memory cache) and a distributed cache next to the database.

## Caching and REST

Cacheability is one of the six guiding constraints of representational state transfer (REST):

> Cache constraints require that the data within a response to a request be implicitly or explicitly labeled as cacheable or non-cacheable. If a response is cacheable, then a client cache is given the right to reuse that response data for later, equivalent requests.
> — Roy Fielding's REST dissertation, 5.1.4

## Cache-Control via [ResponseCache]

- The `[ResponseCache]` attribute in ASP.NET Core is a convenient way to set the `cache-control` HTTP response header's directives, required by the HTTP 1.1 caching specifications (RFC 7234)
- These directives are honored by clients and intermediate proxies, fulfilling the REST constraint's requirements

```csharp
[HttpGet(Name = "GetBoardGames")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
public async Task<RestDTO<BoardGame[]>> Get(
    [FromQuery] RequestDTO<BoardGameDTO> input)
{ /* ... */ }

[HttpPost(Name = "UpdateBoardGame")]
[ResponseCache(NoStore = true)]
public async Task<RestDTO<BoardGame?>> Post(BoardGameDTO model)
{ /* ... */ }
```

## Common Cache-Control directives

| Directive | ResponseCache property | Action |
|---|---|---|
| `public` | `Location=Any` | A cache may store the response |
| `private` | `Location=Client` | The response must not be stored by a shared cache. A private cache may store and reuse the response |
| `max-age` | `Duration` | The client doesn't accept a response whose age is greater than the specified number of seconds. Examples: `max-age=60` (60 seconds), `max-age=2592000` (1 month) |
| `no-cache` | `Location=None` | On requests: a cache must not use a stored response to satisfy the request; the origin server regenerates the response and the middleware updates the stored response in its cache. On responses: the response must not be used for a subsequent request without validation on the origin server |
| `no-store` | `NoStore` | On requests: a cache must not store the request. On responses: a cache must not store any part of the response |

## Setting the cache-control header manually

Use the `Response.Headers` collection provided by the `HttpContext`:

```csharp
app.MapGet("/cache/test/1",
    [EnableCors("AnyOrigin")]
        (HttpContext context) =>
    {
        context.Response.Headers["cache-control"] =
            "no-cache, no-store";
        return Results.Ok();
    });
```

The `[ResponseCache]` attribute is generally a better alternative unless we have specific caching needs that aren't supported by the high-level abstraction it provides.

## Default caching directive

- If we forget to set the `[ResponseCache]` attribute somewhere, the corresponding response will have no cache-control header — unless you add a default
- Not a direct violation of the REST constraint requirements, but widely considered a data-security vulnerability, because it leaves clients (and intermediate proxies) free to choose whether to cache the received content, which could contain sensitive information
- To avoid this problem, define a default caching directive

## Implementing a no-cache default behavior

Add custom middleware to the app's HTTP pipeline that sets the cache-control header using strongly typed headers:

```csharp
// Adds a default cache-control directive
app.Use((context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            NoCache = true,
            NoStore = true
        };
    return next.Invoke();
});
```

## Cache profiles

- Enable us to centralize Cache-Control directives to keep the codebase more DRY and easier to maintain
- A Controller Middleware configuration option that sets up predefined caching directives, applied via a name-based reference instead of repeating them

Create cache profiles in `Program.cs` (in the `AddControllers` configuration):

```csharp
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("NoCache",
        new CacheProfile() { NoStore = true });
    options.CacheProfiles.Add("Any-60",
        new CacheProfile() { Location = ResponseCacheLocation.Any, Duration = 60 });
    options.CacheProfiles.Add("Any-1hour",
        new CacheProfile() { Location = ResponseCacheLocation.Any, Duration = 3600 });
    options.CacheProfiles.Add("Client-1day",
        new CacheProfile() { Location = ResponseCacheLocation.Client, Duration = 86400 });
});
```

Use a cache profile — replace explicit caching values with a profile reference:

```csharp
// Before:
[HttpGet(Name = "GetBoardGames")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]

// After:
[HttpGet(Name = "GetBoardGames")]
[ResponseCache(CacheProfileName = "Any-60")]
public async Task<RestDTO<BoardGame[]>> Get(
    [FromQuery] RequestDTO<BoardGameDTO> input)
{ /* ... */ }
```

## Add Cache-Control for static files

- Best practices recommend specifying an explicit expiration time for content several years out, to ensure the browser can reuse content without making conditional HTTP requests to revalidate it with the server
- If the resource changes, change the name of the resource
- In ASP.NET Core you can specify cache-control for static files:

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseStaticFiles(new StaticFileOptions()
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
        }
    });
}
```

Useful durations: 10 minutes = 600, 1 hour = 3600, 24 hours = 86400, 30 days = 2592000.

## Different caching strategies

- No caching (Cache-Control)
- The Expires header
- Conditional GET (by the browser)

## Cache-Control values (browser view)

The HTTP response header `Cache-Control` has 3 possible values:

- `Cache-Control: no-cache` — tells the browser to never use a cached version of a resource without first checking the ETag value
- `Cache-Control: no-store` — tells the browser (and all intermediary network devices) not to store the resource in its cache
- `Cache-Control: max-age=3600` (or another value) — tells the browser the number of seconds this resource is valid in a cache

## Add an Expires header

- When an HTTP request is sent, the browser checks if it has a copy of that page in the cache, based on the URL
- If there is, it checks the page for freshness
- A page is fresh if the HTTP response `Expires` header value is less than the current datetime
- The Expires response header takes this form:

```
Expires: Sat, 01 Dec 2018 16:00:00 GMT
```

## Conditional GET

- **Using If-Modified-Since and Last-Modified**
  - The browser sends a request adding an `If-Modified-Since` header, based on the `Last-Modified` header value from the currently cached page
  - This tells the server to only return a response body (the page content) if the resource has been updated since that date
  - Otherwise, the server returns a `304 Not Modified` response
- **Using If-None-Match and ETag**
  - The web server can send an `ETag` header — the identifier of a resource
    - Every time the resource changes, the ETag should change as well (like a checksum)
  - The browser sends an `If-None-Match` header containing one (or more) ETag values
  - If none match, the server returns the fresh version of the resource; otherwise a `304 Not Modified` response

## Server-side response caching

ASP.NET Core has a built-in component that can store HTTP responses in a dedicated internal cache repository and serve them from that cache instead of re-executing the controller's (or minimal API's) methods.

### Response-caching middleware

- Allows the ASP.NET Core app to cache its own responses according to the same caching directives specified in the response headers by the `[ResponseCache]` attribute or by any other means
- The middleware runs on the same server that hosts the ASP.NET Core application itself
  - Not a trivial problem: a main benefit of intermediate caching services (reverse proxies, CDNs) is that they're located elsewhere to avoid burdening the web server with additional overhead
- The response-caching middleware will cache only HTTP responses using GET or HEAD methods and resulting in a `200 OK` status code — it ignores any other responses, including error pages

### Enable server-side response caching

In `Program.cs`:

```csharp
builder.Services.AddResponseCaching();
```

```csharp
app.UseResponseCaching();
```

### Configuring the middleware

Part of local memory is reserved for cached responses, which could negatively affect app performance. To prevent memory-shortage problems, fine-tune the middleware's caching strategies with the `ResponseCachingOptions` class:

- `MaximumBodySize` — maximum cacheable size for the response body, in bytes (default 64 * 1024 * 1024 = 64 MB)
- `SizeLimit` — size limit for the response-cache middleware, in bytes (default 100 * 1024 * 1024 = 100 MB)

```csharp
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 32 * 1024 * 1024;
    options.SizeLimit = 50 * 1024 * 1024;
});
```

### The client reload problem

- All the HTTP response caching techniques so far implement standard HTTP caching semantics
- They don't only follow the response cache headers; they also honor the *request* cache headers, including the cache-control headers set by web browsers on reload:
  - Browser reload: `cache-control: max-age=0`
  - Browser force reload: `cache-control: no-cache`
- As a result, all clients are perfectly able to bypass this kind of cache
- This makes the server vulnerable to distributed denial-of-service (DDoS) attacks

## In-memory caching

- A .NET caching feature that allows storing arbitrary data in local memory
- Can be injected as a service and exposes convenient Get and Set methods
- The `IMemoryCache` instance is a singleton object that stores arbitrary data as key-value pairs

### Setting up the in-memory cache

In `Program.cs`:

```csharp
builder.Services.AddMemoryCache();
```

The extension method supports an additional overload accepting a `MemoryCacheOptions` object with settings such as:

- `ExpirationScanFrequency` — a TimeSpan defining the length of time between successive scans for expired items
- `SizeLimit` — the maximum size of the cache, in bytes
- `CompactionPercentage` — the amount to compact the cache by when the SizeLimit value is exceeded

### Inject the IMemoryCache interface

```csharp
public class BoardGamesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BoardGamesController> _logger;
    private readonly IMemoryCache _memoryCache;

    public BoardGamesController(
        ApplicationDbContext context,
        ILogger<BoardGamesController> logger,
        IMemoryCache memoryCache)
    {
        _context = context;
        _logger = logger;
        _memoryCache = memoryCache;
    }
}
```

### Using the in-memory cache

Goal: cache the board-game array returned by EF Core in the `Get` method for 30 seconds.

1. Create a `cacheKey` string variable based on the GET parameters
2. Check the `_memoryCache` instance to see whether a cache entry for that key is present
3. If a cache entry is present, use it instead of querying the database; otherwise retrieve the data using EF Core and set the cache entry with an absolute expiration of 30 seconds

```csharp
BoardGame[]? result = null;
var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";   // Create a cacheKey
if (!_memoryCache.TryGetValue<BoardGame[]>(cacheKey, out result))         // Check the _memoryCache instance
{
    query = query
        .OrderBy($"{input.SortColumn} {input.SortOrder}")
        .Skip(input.PageIndex * input.PageSize)
        .Take(input.PageSize);
    result = await query.ToArrayAsync();
    _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));           // Absolute expiration of 30 seconds
}
return new RestDTO<BoardGame[]>()
{
    Data = result,
    // ...
};
```

## Distributed caching

- In-memory caching is a major problem for apps running on multiple servers
- The solution is to replace the local in-memory cache with a shared caching repository that can be accessed by multiple servers — a *distributed cache*

### Benefits of distributed caching

- **Consistent cached data** — all servers retrieve them from the same source
- **Independent lifetime** — it doesn't depend on any web server (won't reset whenever a server restarts)
- **Performance benefits** — typically implemented by a third-party service, and won't consume a web server's local memory

### Distributed cache with SQL Server

- Install the NuGet package `Microsoft.Extensions.Caching.SqlServer`
- Configure the SQL Server cache service in `Program.cs`:

```csharp
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "AppCache";
});
```

- The distributed caching tasks will be performed against a new, dedicated database table called `AppCache`

### Creating the AppCache DB table

Install and execute the sql-cache tool with the dotnet CLI from the project's root folder — the version number must match the .NET version (patch included) installed on the system:

```bash
dotnet tool install --global dotnet-sql-cache --version 7.0.11
```

Then execute (replace `{connectionString}` with the value of the DefaultConnection key):

```bash
dotnet sql-cache create "{connectionString}" dbo AppCache
```

Example:

```bash
dotnet sql-cache create "Server=localhost;Database=MyBGList;User Id=SA;Password=YourStrong@Passw0rd;MultipleActiveResultSets=True;TrustServerCertificate=True" dbo AppCache
```

The command creates a new `[AppCache]` DB table with the following columns:

- `Id`: PK, nvarchar(449), not null
- `Value`: varbinary(max), not null
- `ExpiresAtTime`: datetimeoffset(7), not null
- `SlidingExpirationInSeconds`: bigint, null
- `AbsoluteExpiration`: datetimeoffset(7), null

Alternatively, create the table manually using SQL Server Management Studio (SSMS).

### Adding the DistributedCacheExtensions

- The `IDistributedCache` interface doesn't come with the handy generic type methods — `Get<T>`, `Set<T>`, `TryGetValue<T>` — that `IMemoryCache` has; it provides only Get and Set methods for string and byte array values
- We can create an extension method helper class implementing the same methods and extending `IDistributedCache`:
  - `TryGetValue<T>` — accepting a cacheKey string value and an out parameter of type T
  - `Set<T>` — accepting a cacheKey string value, a value of type T, and a TimeSpan representing the absolute expiration relative to the current time

Create a new `/Extensions/` directory and a `DistributedCacheExtensions.cs` file within it:

```csharp
public static class DistributedCacheExtensions
{
    public static bool TryGetValue<T>(
        this IDistributedCache cache,
        string key,
        out T? value)
    {
        value = default;
        var val = cache.Get(key);
        if (val == null) return false;
        value = JsonSerializer.Deserialize<T>(val);
        return true;
    }

    public static void Set<T>(
        this IDistributedCache cache,
        string key,
        T value,
        TimeSpan absoluteExpirationRelativeToNow)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
        cache.Set(key, bytes, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow
        });
    }
}
```

### Injecting the IDistributedCache interface

Add references to the required namespaces at the top of the controller file, and inject an `IDistributedCache` instance in the constructor via dependency injection:

```csharp
using Microsoft.Extensions.Caching.Distributed;
using MyBGList.Extensions;
using System.Text.Json;
```

### Using the DistributedCache

```csharp
public async Task<RestDTO<Mechanic[]>> Get(
    [FromQuery] RequestDTO<MechanicDTO> input)
{
    var query = _context.Mechanics.AsQueryable();
    if (!string.IsNullOrEmpty(input.FilterQuery))
        query = query.Where(b => b.Name.Contains(input.FilterQuery));
    var recordCount = await query.CountAsync();

    Mechanic[]? result = null;
    var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";
    if (!_distributedCache.TryGetValue<Mechanic[]>(cacheKey, out result))
    {
        query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);
        result = await query.ToArrayAsync();
        _distributedCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
    }

    return new RestDTO<Mechanic[]>()
    {
        Data = result,
        // ...
    };
}
```

## Redis

- The open source, in-memory data store used by millions of developers as a database, cache, streaming engine, and message broker
- Keeps the dataset in memory for fast access, but can also persist all writes to permanent storage to survive reboots and system failures

## Caching server — NGINX

- NGINX caching and cache clustering are powerful tools that can exponentially improve the performance of your website
- NGINX is the leading web server and reverse proxy server for high-performance websites, in use by more than 50% of the top 100,000 websites

### Microcaching

- NGINX allows you to increase server capacity by taking repetitive tasks away from the upstream servers. Even for content which appears to be uncacheable (the front page of a blogging site, for example), there's merit in *microcaching* — caching content on the NGINX proxy just for a second or so
- When a hundred users request the same content in the same second, NGINX reduces that down to a single request to the origin server. NGINX serves content back to ninety-nine of those users from its cache, with a promise that content is never more than one second out of date

## References & Links

- Building Web APIs with ASP.NET Core, chapter 8
- Response caching in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response
- SqlServerCacheOptions class: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.sqlserver.sqlservercacheoptions
