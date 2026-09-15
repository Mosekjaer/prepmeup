---
title: SW4BAD - Building Background Tasks and Services
source: SW4BAD - BackgroundServices.pdf
course_week: 12-13
topic: API-dokumentation, background services og caching
---

# SW4BAD: Building Background Tasks and Services

Contents: Background services, implementing background tasks in .NET, scoped services, deploying background tasks, Quartz.NET.

## Building background tasks and services

- ASP.NET Core apps only create tasks in response to requests
- Background services have no direct interaction with users
  - Run in the background
  - Typically process items from a queue or perform long-running processes
- Can be implemented in various ways:
  - `IHostedService`
  - `BackgroundService`
- You have already used multiple background services:
  - Kestrel runs as an `IHostedService`
  - ASP.NET apps are hosted in a Kestrel service

## Use cases

- **Batching email**
  - Send out order confirmations from a webshop — the user interface is not busy waiting for the mail to go through
  - Send out newsletters once each day/week/month
- **Compute daily profits**
  - Load data from day sales, upload dashboard
- **Caching data from external sources** (e.g. exchange rates)
  - Improve response time for our APIs
  - Consider what data is cacheable — not all data are good candidates

## Implementation with a background service

Approach:

- Use the `IHostedService` interface
  - Makes it easier to run async/await and enforces best-practice patterns
  - Abstract approach
- Run background tasks with timers
  - Simple schedule: run X after time Y has elapsed
  - Use `Task.Delay()` to specify the timespan
- Register background services in ConfigureServices
  - Just as with any other service
  - Use `AddHostedService()`

## Implement the background task

A `BackgroundService` that calls a remote HTTP API and caches the result (from ASP.NET Core in Action, listing 34.2):

```csharp
// Derives from BackgroundService to create a task that runs
// for the lifetime of your app
public class ExchangeRatesHostedService : BackgroundService
{
    private readonly IServiceProvider _provider;   // Injects an IServiceProvider so you
    private readonly ExchangeRatesCache _cache;    // can create instances of the typed client
                                                   // _cache: a simple cache for exchange rates
    public ExchangeRatesHostedService(
        IServiceProvider provider, ExchangeRatesCache cache)
    {
        _provider = provider;
        _cache = cache;
    }

    // You must override ExecuteAsync to set the service's behavior.
    // The CancellationToken passed as an argument is triggered when
    // the application shuts down.
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        // Keeps looping until the application shuts down
        while (!stoppingToken.IsCancellationRequested)
        {
            // Creates a new instance of the typed client
            // so that the HttpClient is short-lived
            var client = _provider
                .GetRequiredService<ExchangeRatesClient>();

            // Fetches the latest rates from the remote API
            string rates = await client.GetLatestRatesAsync();
            _cache.SetRates(rates);   // Stores the rates in the cache

            // Waits for 5 minutes (or for the application to shut down)
            // before updating the cache
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
```

## Register the task in Program.cs

Registering an `IHostedService` with the DI container (listing 34.3):

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ExchangeRatesClient>();       // Registers the typed client as before
builder.Services.AddSingleton<ExchangeRatesCache>();         // Adds the cache object as a singleton so it is shared throughout your app
builder.Services.AddHostedService<ExchangeRatesHostedService>();  // Registers ExchangeRatesHostedService as an IHostedService
```

## Scoped services

- Background services that implement `IHostedService` are created once when your application starts
  - That means they are by necessity singletons, as there will be only a single instance of the class
  - Any dependencies added to a service must have a lifetime that is equal to or longer than that of the service itself
  - But sometimes it would be preferable to have short-lived services...
- Scoped services:
  - The ASP.NET framework creates a new container scope every time a new request is received
  - Create container scopes with `IServiceCollection.CreateScope()`

Consuming scoped services from an `IHostedService` (listing 34.5):

```csharp
// BackgroundService is registered as a singleton.
public class ExchangeRatesHostedService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public ExchangeRatesHostedService(IServiceProvider provider)
    {
        // The injected IServiceProvider can be used to retrieve
        // singleton services or to create scopes.
        _provider = provider;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Creates a new scope using the root IServiceProvider
            using (IServiceScope scope = _provider.CreateScope())
            {
                // The scope exposes an IServiceProvider that can be
                // used to retrieve scoped components.
                var scopedProvider = scope.ServiceProvider;

                // Retrieves the scoped services from the container
                var client = scope.ServiceProvider
                    .GetRequiredService<ExchangeRatesClient>();

                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                // Fetches the latest rates, and saves using EF Core
                var rates = await client.GetLatestRatesAsync();

                context.Add(rates);
                await context.SaveChanges(rates);
            }  // Disposes of the scope with the using statement

            // Waits for the next iteration.
            // A new scope is created on the next iteration.
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
```

## The IHost implementation

- Background tasks can be created in two ways:
  - Classes implementing `BackgroundService` in ASP.NET Core projects
  - Stand-alone .NET applications
- The .NET Core generic `IHost` abstraction provides logging, configuration, and dependency injection abstractions
  - If your application doesn't need to handle HTTP requests, there's no reason to use ASP.NET Core
- HTTP functionality is not added by default
  - Add the package `Microsoft.Extensions.Http` if you need to fetch data using HTTP

## Headless worker services

- Worker services are .NET console applications that use the generic `IHost` but don't include the ASP.NET Core libraries for handling HTTP requests
  - Intended for long-running processes
  - Called *headless* because they do not feature a UI
- Can be deployed like any other .NET application:
  - Running in an isolated container
  - Framework-dependent, where the .NET runtime is installed on the host machine
  - Self-contained, where the .NET runtime is bundled with the application

Example worker service:

```csharp
using SystemdService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
```

```csharp
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
```

## Running as a (Windows) service

- Commonly registered as a service with the operating system:
  - Windows Service on Microsoft-based systems
  - systemd on Unix-based systems
- It's also very common to run applications in the cloud using Docker containers or dedicated platform services like Azure App Service
- To deploy as a Windows Service:
  - Install NuGet package: `Microsoft.Extensions.Hosting.WindowsServices`
  - Call `UseWindowsService()`
  - Publish the app (`dotnet publish -c Release`)
  - Deploy into the services:

```bash
sc create "My Test Service" BinPath="C:\path\to\MyService.exe"
```

```csharp
using WindowsService;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseWindowsService()
    .Build();

host.Run();
```

- You can manage the service from the Services control panel in Windows
- You can install an ASP.NET Core application the same way — add the call to `UseWindowsService()` and install your ASP.NET Core app

## Scheduling libraries

- All the background tasks seen so far repeat a task on an interval indefinitely, from the moment the application starts
- Sometimes you want more control of this timing
- Excellent libraries already provide this functionality. Two of the most well known in the .NET space:
  - Hangfire (www.hangfire.io)
  - Quartz.NET (www.quartz-scheduler.net)

## Quartz.NET

- More control over when specific tasks are scheduled
- Quartz.NET main concepts:
  - **Jobs** — background tasks that implement the logic
  - **Triggers** — control when Jobs will run
  - **Job Factory** — creates instances of Jobs
  - **Scheduler** — keeps track of application triggers
- Clustering workers (distributed locking):
  1. The trigger schedule indicates that a job is due to run
  2. All instances of the application attempt to obtain a lock (in a shared database of job locks) to run the job
  3. Only a single instance receives the lock on the job — that instance executes the job

## References

- ASP.NET Core in Action, second edition, by Andrew Lock (Manning)
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services
- https://www.quartz-scheduler.net/documentation/
