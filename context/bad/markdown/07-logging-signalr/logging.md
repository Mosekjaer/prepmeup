---
title: Logging in ASP.NET Core
source: Logging.pdf
course_week: 7
topic: Logging + SignalR
---

# Logging in ASP.NET Core

## What is logging?

- Logging, in an IT context, is the process of keeping track of all the events occurring within the application in a structured, semi-structured, or unstructured format, and outputting them to a dedicated view and/or storage channel.
- The primary purpose is to keep track of the various interactions between the software and its users:
  - State changes
  - Access to internal resources
  - Event handlers that trigger in response to user actions
  - Exceptions thrown by internal modules
- Each log entry is typically recorded with a timestamp representing the moment the logged event occurred.

Without logs, a developer investigating a support case only has the user's information — and the user may not know it all. With logs, the developer can look up what actually happened, e.g.:

```
[2024-07-03 14:45:19] userId: 5 added -1 item
[2024-07-03 14:45:19] Exception: invalid argument
```

## Why do we need logs?

- **Stability** — Log records allow us to detect and investigate bugs and/or unhandled exceptions promptly, facilitating fixes and minimizing downtime.
- **Security** — A logging system helps determine whether the system has been compromised and to what extent, and identifies the vulnerabilities and malicious actions the attacker performed.
- **Business continuity** — Logs can make system administrators aware of abnormal behavior before it becomes critical and inform them promptly about crashes via alarms, email messages, and other real-time notification-based alert processes.
- **Compliance requirements** — Most international IT security regulations, standards, and guidelines require precise logging policies, further strengthened by GDPR.

## Built-in logging

ASP.NET Core includes a generic logging interface that you can plug into. It is used throughout the ASP.NET Core framework code itself, as well as by third-party libraries, and you can easily use it to create logs in your own code.

## How to create log messages

Inject `ILogger<T>` via constructor injection:

```csharp
public RecipeController(
    RecipeService service,
    UserManager<ApplicationUser> userService,
    IAuthorizationService authService,
    ILogger<RecipeController> log)
{
    _service = service;
    _userService = userService;
    _authService = authService;
    _log = log;
}

public IActionResult Index()
{
    var models = _service.GetRecipes();
    _log.LogInformation("Loaded {RecipeCount} recipes", models.Count);
    return models;
}
```

The exact format of the log message varies from provider to provider.

## Log levels

Every log message has a log level, defined by the `LogLevel` enum (from most to least severe):

| Level | Use |
|---|---|
| `LogLevel.Critical` | Disastrous errors that may leave the app unable to function |
| `LogLevel.Error` | Unhandled errors and exceptions that don't affect other requests |
| `LogLevel.Warning` | Unexpected conditions that you can work around, such as handled exceptions |
| `LogLevel.Information` | For tracking normal application flow |
| `LogLevel.Debug` | For tracking detailed information, especially during development |
| `LogLevel.Trace` | For very detailed, sensitive information — rarely used |

## Each log record includes up to six common elements

- **Log level** — how important the log is, defined by the `LogLevel` enum.
- **Event category** — may be any string, but typically the name of the class creating the log. For `ILogger<T>`, the full name of type `T` is the category.
- **Message** — the content of the log message. Can be a static string or contain placeholders for variables, indicated by braces `{}`, substituted with the provided parameter values.
- **Parameters** — if the message contains placeholders, they are associated with the provided parameters.
- **Exception** — you can pass the exception object to the logging function along with the message and other parameters.
- **EventId** — an optional integer identifier for the error, used to quickly find similar logs. Defaults to 0.

## Changing log verbosity with filtering

ASP.NET Core can filter out log messages before they are written, based on a combination of three things:

- The log level of the message
- The category of the logger (who created the log)
- The logger provider (where the log will be written)

Example rules:

- The default minimum log level is `Information` — if no other rules apply, only logs at `Information` or above are written.
- For categories starting with `Microsoft`, the minimum log level is `Warning` — filters out "noisy" framework messages.
- For the console provider, the minimum log level is `Error` — lower levels are not written to console, though they may be written by other providers.

### The log filtering configuration section of appsettings.json

```json
{
    "Logging": {
        "IncludeScopes": false,
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Warning"
        },
        "Debug": {
            "LogLevel": {
                "Default": "Information"
            }
        },
        "Console": {
            "LogLevel": {
                "Default": "Error"
            }
        }
    }
}
```

The top-level `LogLevel` section holds the rules applied when there are no applicable provider-specific rules.

## Formatting messages and capturing parameter values

Whenever you create a log entry, you must provide a message. Including a placeholder and a parameter value effectively creates a key-value pair, which some logging providers can store as additional information associated with the log.

```csharp
// Correct: structured logging with placeholder
_log.LogInformation("Loaded {RecipeCount} recipes", models.Count);

// Avoid: string interpolation loses the structured key-value pair
_log.LogInformation($"Loaded {models.Count} recipes");
```

## Structured or semantic logging

- Structured (semantic) logging attaches additional structure to log messages to make them more easily searchable and filterable.
- Rather than storing only text, it stores contextual information, typically as key-value pairs.

```csharp
_log.LogInformation("User {UserId} loaded recipe {RecipeId}", 123, 456);
```

This creates the parameters `UserId=123` and `RecipeId=456`. Structured logging providers can store these values in addition to the formatted message `"User 123 loaded recipe 456"`, making it easy to search the logs for a particular `UserId` or `RecipeId`.

## The ASP.NET Core logging abstractions

- `ILoggerProvider`s are registered with the `ILoggerFactory` using extension methods (e.g. `AddConsole()`, `AddEventLog()`).
- `ILoggerProvider`s are used to create loggers that write to a specific destination (e.g. `ConsoleLoggerProvider` creates a logger writing to the console, `EventLogLoggerProvider` one writing to the Windows Event Log).
- Calling `CreateLogger` on the `ILoggerFactory` calls `CreateLogger` on each provider and creates an `ILogger` that wraps each logger implementation.
- Calling `Log()` on the `ILogger` writes the message to each of the wrapped logger destinations — e.g. `ConsoleLogger` → `Console.Write()`, `DebugLogger` → `Debug.Write()`, `FileLogger` → `File.AppendText()`.

## Controlling where logs are written

Log providers available out of the box:

- **Console provider** — writes messages to the console. NOT for production.
- **Debug provider** — writes messages to the debug window when debugging in Visual Studio / VS Code.
- **EventLog provider** — writes to the Windows Event Log. Only available when targeting Windows.
- **EventSource provider** — writes using Event Tracing for Windows or Linux Trace Toolkit next-gen.
- **Azure App Service provider** — writes to text files or blob storage when running in Azure. Automatically added when running an ASP.NET Core app in Azure.

There are also many third-party logging provider implementations, such as NLog, Loggr, and Serilog.

## Adding a new logging provider

To add a third-party logging provider in ASP.NET Core:

1. Add the logging provider NuGet package to the solution (e.g. `NetEscapades.Extensions.Logging.RollingFile`, a simple file logging provider — for more control over file format, use Serilog instead).
2. Add the logging provider via the logging builder:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Logging
    .ClearProviders()
    .AddSimpleConsole()
    .AddDebug();
```

For production, a more established logging provider such as NLog or Serilog is recommended.

## Serilog overview

Serilog is a full-featured logging system that can be set up in two different ways:

- **As a logging API** — replaces the .NET logging implementation (including the `ILogger` interface) with its own native interface.
- **As a logging provider** — implements the Microsoft extensions logging API (and extends it with several additional features) instead of replacing it.

With ASP.NET Core you typically replace the default `ILoggerFactory` with a custom factory containing a single logging provider, `SerilogLoggerProvider`. You can integrate with Serilog while still using the `ILogger` abstractions in your application code. Serilog supports up to 67 different sinks, including Elasticsearch (full list: https://github.com/serilog/serilog/wiki/Provided-Sinks).

## Serilog advantages

- **Enrichers** — packages that automatically add additional info to log events (ProcessId, ThreadId, MachineName, EnvironmentName, etc.).
- **Sinks** — a selection of output destinations, such as DBMSes, cloud-based repositories, and third-party services.

Serilog has a modular architecture: all enrichers and sinks are available via dedicated NuGet packages installed alongside the core package(s) as needed.

## Installing Serilog (with SQL Server sink)

To store log events in a SQL Server database, install:

- `Serilog.AspNetCore` — includes the core Serilog package, integration into the ASP.NET Core configuration and hosting infrastructure, some basic enrichers and sinks, and the middleware required to log HTTP requests.
- `Serilog.Sinks.MSSqlServer` — the sink for storing event logs in SQL Server.

## Configuring Serilog

Configuration takes place in `Program.cs`:

```csharp
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);

    lc.WriteTo.File("Logs/log.txt",
        outputTemplate:
            "{Timestamp:HH:mm:ss} [{Level:u3}] " +
            "[{MachineName} #{ThreadId}] " +
            "{Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day);

    lc.WriteTo.MSSqlServer(
        connectionString:
            ctx.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "LogEvents",
            AutoCreateSqlTable = true
        });
},
    writeToProviders: true);
```

## Improving the logging behavior — adding columns

```csharp
lc.WriteTo.MSSqlServer(
    connectionString:
        ctx.Configuration.GetConnectionString("DefaultConnection"),
    sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "LogEvents",
        AutoCreateSqlTable = true
    },
    columnOptions: new ColumnOptions()
    {
        AdditionalColumns = new SqlColumn[]
        {
            new SqlColumn()
            {
                ColumnName = "SourceContext",
                PropertyName = "SourceContext",
                DataType = System.Data.SqlDbType.NVarChar
            }
        }
    }
);
```

## Configuring the minimum level

Serilog can be configured via `appsettings.json` like the built-in providers, but it uses its own configuration section, which replaces the `Logging` section used by the default logger implementation:

```json
"Serilog": {
    "MinimumLevel": {
        "Default": "Information",
        "Override": {
            "Microsoft.AspNetCore": "Warning",
            "MyBGList": "Debug"
        }
    }
}
```

Serilog does not use the `Microsoft.Extensions.Logging.LogLevel` enum. It uses the proprietary `Serilog.Events.LogEventLevel` enum with slightly different names: `Verbose`, `Debug`, `Information`, `Warning`, `Error`, and `Fatal`.

## Adding enrichers

Add additional context info using Serilog's enrichers. Install these NuGet packages:

- `Serilog.Enrichers.Environment`
- `Serilog.Enrichers.Thread`

Then activate them:

```csharp
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
    lc.Enrich.WithMachineName();
    lc.Enrich.WithThreadId();
    lc.WriteTo.File("Logs/log.txt", /* ... */);
});
```

## Structured logging: creating searchable, useful logs

- For a user-friendly structured logging frontend, Seq (https://getseq.net) is a great option.
- Seq is installed on a server or your local machine and collects structured log messages over HTTP, providing a web interface for viewing and analyzing your logs.
- Or use the equivalent from your cloud provider (e.g. Application Insights).

## References & links

- Building Web APIs with ASP.NET Core, chapter 7
- ASP.NET Core in Action, Andrew Lock, chapter 16
- Application Insights: https://docs.microsoft.com/en-us/azure/application-insights/app-insights-overview
- The Art of Logging: https://www.codeproject.com/articles/42354/the-art-of-logging
- Logging in ASP.NET Core: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
- Serilog: https://github.com/serilog/serilog
- Seq: https://getseq.net/
- NLog: https://github.com/NLog
- Log4Net: https://logging.apache.org/log4net/
