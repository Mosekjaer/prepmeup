---
title: Web APIs in ASP.NET Core
source: Web APIs in ASPNET Core.pdf
course_week: 4
topic: Web APIs + REST + Dapper
---

# Web APIs in ASP.NET Core

Contents: Web app vs Web API, the ASP.NET Core app framework, dependency injection, services and middleware, minimal APIs.

## The World Wide Web

Based on HTTP protocol methods: GET, POST, DELETE, PATCH. HTTP itself works on top of TCP/IP:

- Addresses: `localhost` / `192.168.0.1`
- URLs: `https://www.google.com/search?q=SW4BAD`

## The board games example (MyBGList)

A running example of one backend serving many clients:

- **MyBGList web API** — the playmaker, fetching the data source (MyBGList DBMS) and making it available to the other services.
- **MyBGList website** — a ReactJS site where users browse the board-game catalog and add games to predefined lists (Own, Want to Try, Want to Buy, …) plus custom lists.
- **MyBGList mobile app** — a React Native app with the same actions as the website.
- **MyBGList management portal** — an ASP.NET web application for system administrators (add/update/delete board games, maintenance tasks).
- **MyBGList insights** — a SaaS service that periodically pulls data from the web API for logging, monitoring, performance analysis, and BI.

## Web app vs Web APIs

We categorize apps into:

1. **HTML web application** — generates HTML pages as responses for users' requests (typical website).
2. **Web APIs** — designed for consumption by another machine or in code (returns JSON or XML).
3. **Hybrid apps** — both an HTML web application and an API.

Web APIs enable data exchange among all the parties. Different routes provide access to different resources, e.g. `https://mybglist.com/api/search` and `https://mybglist.com/api/feedbacks`.

Example flow: (1) the web app queries the search endpoint for board games named "Citadels"; (2) another request to the feedback endpoint retrieves all feedback for that user's unique ID; (3) a client-side iteration finds user IDs with rating >= 6; (4) a third request to the user endpoint retrieves the corresponding users.

## Demo setup

Dependencies: .NET Core 10, Visual Studio Code, C# Dev Kit extension, REST Client extension.

```bash
mkdir WeatherExample
dotnet new webapi -controllers -f net8.0
dotnet run
# Try in browser: https://localhost:5000/weatherforecast
```

## ASP.NET Core app framework

Applications are composed of discrete and reusable components that perform specific tasks and communicate through framework interfaces:

- **Services**
- **Middleware**

All components are registered and configured in the application's `Program.cs` file.

## Services and Dependency Injection

Services are the components an application requires to provide its functionality — think of them as app dependencies.

ASP.NET Core features the **Dependency Injection** design pattern:

- **Implementation** — dedicated interface or base class (the actual code)
- **Registration and configuration** — the service container `IServiceProvider`, where all services used by the app must be configured and registered
- **Dependency injection** — service instances are created automatically by the `IServiceProvider` and given to the constructors in your code

### Services example (DI in action)

```csharp
string RegisterUser(string username, EmailSender emailSender)
{
    emailSender.SendEmail(username);
    return $"Email sent to {username}!";
}
```

Instead of creating the dependencies implicitly, they are injected directly — the handler is easy to read and understand. The DI container creates the complete dependency graph and provides an instance of `EmailSender` to `RegisterUser`: `RegisterUser` depends on `EmailSender`; `EmailSender` depends on `MessageFactory` and `NetworkClient`; `NetworkClient` depends on `EmailServerSettings` — all resolved by the DI container.

## Middleware and endpoints

Middleware is the set of components operating at the HTTP level, handling the whole HTTP request processing pipeline:

- Each component can pass the request on to the next component in the pipeline, and can perform actions before and after the next component.
- Or it can short-circuit and return the response directly — known as **terminal middleware** or an **endpoint**.

## Program.cs file

Executed at the start of the application, responsible for:

- Instantiating the web application
- Registering and configuring the services
- Registering and configuring the middleware

The web application is instantiated by the `WebApplicationBuilder` factory class, creating a `WebApplication` object, stored in the `app` local variable.

```csharp
var builder = WebApplication.CreateBuilder(args);   // Creates the WebApplicationBuilder factory class

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();          // Registers and configures services
builder.Services.AddSwaggerGen();

var app = builder.Build();                           // Builds the WebApplication object

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();                           // Registers and configures nonterminal and
app.UseAuthorization();                              // potentially terminal middleware
app.MapControllers();

app.Run();                                           // Registers and configures terminal middleware
```

## Controllers

Controllers are classes used to group a set of **action methods** (actions) that handle similar HTTP requests. They aggregate action methods that have something in common: routing rules and prefixes, services, instances, authorization requirements, caching strategies, …

Controllers can inherit from two built-in base classes:

- **`ControllerBase`** — minimal implementation without support for views. Meant for web APIs where the result is JSON or XML, not HTML.
- **`Controller`** — inherits from `ControllerBase` and adds full support for views. Meant for MVC apps with data presentation.

### Controller example

Web API controller handling: GET `/api/Sample/` (list of items), GET `/api/Sample/{id}` (one item), DELETE `/api/Sample/{id}` (delete one item).

```csharp
[ApiController]                      // Adds API-specific behaviors
[Route("api/[controller]")]          // Default routing rules
public class SampleController : ControllerBase
{
    public SampleController()
    {
    }

    [HttpGet]                        // Action to handle HTTP GET to /api/Sample/
    public string Get()
    {
        return "TODO: return all items";
    }

    [HttpGet("{id}")]                // Action to handle HTTP GET to /api/Sample/{id}
    public string Get(int id)
    {
        return $"TODO: return the item with id #{id}";
    }

    [HttpDelete("{id}")]             // Action to handle HTTP DELETE to /api/Sample/{id}
    public string Delete(int id)
    {
        return $"TODO: delete the item with id #{id}";
    }
}
```

Code conventions to notice:

- Centralized `/api/Sample/` routing prefix for all action methods via `[Route("api/[controller]")]` at controller level.
- Automatic routing rules for all implemented HTTP verbs (incl. required parameters) via `[HttpGet]`/`[HttpDelete]` attributes on the action methods.
- Automatic routing mapping via the ControllerMiddleware's default rules — because the class name has the `Controller` suffix and `app.MapControllers()` is present in `Program.cs`.

## Route vs endpoint

A **route plus an HTTP method** (GET, POST, PUT, PATCH, DELETE) establishes an **endpoint**. For the same route (`https://mybglist.com/api/Sample/3`) we may have several endpoints: if associated with GET it runs the `Get(int id)` action; if associated with DELETE it runs the `Delete(int id)` action.

## Minimal APIs

Think of it as implementing your controllers "inline" in `Program.cs`. Replaces `app.MapControllers()` (and the `SampleController` class) with:

```csharp
// app.MapControllers();
app.MapGet("/api/Sample",
    () => "TODO: return all items");
app.MapGet("/api/Sample/{id}",
    (int id) => $"TODO: return the item with id #{id}");
app.MapDelete("/api/Sample/{id}",
    (int id) => $"TODO: delete the item with id #{id}");
```

## Task-based Asynchronous Pattern (TAP)

Most performance problems affecting web applications occur because a limited number of threads must handle a potentially unlimited volume of concurrent HTTP requests.

The usage of TAP:

- The `async` keyword defines methods returning a `Task`.
- The `await` keyword allows the calling thread to start the newly implemented Task in a nonblocking way. When the Task completes, the thread continues execution.

Necessary in compute-intensive tasks: retrieving data from a DBMS, accessing third-party systems, …

```csharp
[HttpGet]
public async Task<string> Get()      // async
{
    return await Task.Run(() => {    // await
        return "TODO: return all items";
    });
}
```

Compare with the synchronous version:

```csharp
[HttpGet]
public string Get()
{
    return "TODO: return all items";
}
```
