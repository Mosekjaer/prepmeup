---
title: Calling Remote APIs with IHttpClientFactory
source: Calling Remote APIs.pdf
course_week: 11-12
topic: GraphQL, gRPC og remote APIs
---

# Calling Remote APIs with IHttpClientFactory

It's very common for your backend to interact with third-party services by consuming their APIs — backends are often split up in microservices.

## Server-to-server communication

- **HTTP to a REST API**
  - Easy to implement
- **gRPC**
  - Builds on top of HTTP/2 but typically provides much higher performance than traditional RESTful APIs
  - Reduced network usage with Protobuf binary serialization
- **Message queues**
  - Also known as asynchronous messaging
  - Makes your data temporarily persistent, reducing the chance of errors that may occur when different parts of the system are offline

## HttpClient — using-statement (anti-pattern)

In .NET we use the `HttpClient` class for calling HTTP APIs. Wrapping the HttpClient in a `using` statement means it is disposed of at the end of the block:

```csharp
[HttpGet("values")]
public async Task<string> GetRates()
{
    using (HttpClient client = new HttpClient())
    {
        client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
        var response = await client.GetAsync("latest");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
```

**HttpClient is special, and you shouldn't use it like this!**

## Socket exhaustion

If you're making a lot of requests in a short interval, creating/disposing HttpClients per request can quickly lead to socket exhaustion (sockets linger in TIME_WAIT after dispose).

## HttpClient — singleton (also problematic)

A static/singleton HttpClient solves socket exhaustion but introduces a different problem, primarily around DNS:

```csharp
public class ValuesController : ControllerBase
{
    private static readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri("https://api.exchangeratesapi.io")
    };

    [HttpGet("values")]
    public async Task<string> GetRates()
    {
        var response = await _client.GetAsync("latest");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
```

If the DNS record of a service you're calling changes during the lifetime of your application, a singleton HttpClient will keep calling the old service.

## IHttpClientFactory

Use `IHttpClientFactory` to implement resilient HTTP requests:

- HTTP clients are designed to be short-lived
- Takes advantage of HttpClient handler rotation
- Addresses the issues with the original HttpClient class
- Configurable and works with Dependency Injection (DI)
- Provides extensions for Polly-based middleware
  - To take advantage of delegating handlers in HttpClient
  - Polly is a transient-fault-handling library that helps developers add resiliency to their applications, using pre-defined policies in a fluent and thread-safe manner

## HttpMessageHandlers

- Each HttpClient contains a pipeline of `HttpMessageHandler`s
- `IHttpClientFactory` separates the lifetime of the `HttpClient` from the underlying `HttpClientHandler`

## Benefits of using IHttpClientFactory

- Provides a central location for naming and configuring logical HttpClient objects
  - e.g. configure a client (Service Agent) pre-configured to access a specific microservice
- Codifies the concept of outgoing middleware via delegating handlers in HttpClient and implements Polly-based middleware to take advantage of Polly's policies for resiliency
- HttpClient already has the concept of delegating handlers that can be linked together for outgoing HTTP requests
  - You can register HTTP clients into the factory and use a Polly handler for Retry, CircuitBreakers, and so on
- Manages the lifetime of HttpMessageHandler to avoid the problems that occur when managing HttpClient lifetimes yourself

## 4 ways to use IHttpClientFactory

- Basic usage (`CreateClient`)
- Named Clients
- Typed Clients
- Generated Clients

## Register IHttpClientFactory

Register by calling `AddHttpClient` in `Program.cs`:

```csharp
// Add services to the container.
builder.Services.AddHttpClient();
```

Inject `IHttpClientFactory` in the controller:

```csharp
public class BasicController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BasicController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
}
```

## Basic usage / CreateClient

```csharp
// GET: api/Basic
[HttpGet]
public async Task<IEnumerable<GitHubBranch>> GetGitHubBranches()
{
    var httpRequestMessage = new HttpRequestMessage(
        HttpMethod.Get,
        "https://api.github.com/repos/dotnet/AspNetCore.Docs/branches")
    {
        Headers =
        {
            { HeaderNames.Accept, "application/vnd.github.v3+json" },
            { HeaderNames.UserAgent, "HttpRequestsSample" }
        }
    };

    var httpClient = _httpClientFactory.CreateClient();
    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

    if (httpResponseMessage.IsSuccessStatusCode)
    {
        using var contentStream =
            await httpResponseMessage.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync
            <IEnumerable<GitHubBranch>>(contentStream);
    }
}
```

## Named Client

- Use when the app requires many distinct uses of HttpClient
- Isolates the configuration of the client

Configure the client in `Program.cs`:

```csharp
// Add a named client
builder.Services.AddHttpClient("GitHub", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.github.com/");

    // using Microsoft.Net.Http.Headers;
    // The GitHub API requires two headers.
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/vnd.github.v3+json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "HttpRequestsSample");
});
```

To use a named client, pass its name into `CreateClient`:

```csharp
public class NamedClientController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NamedClientController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: api/NamedClient
    [HttpGet]
    public async Task<IEnumerable<GitHubBranch>> Get()
    {
        var httpClient = _httpClientFactory.CreateClient("GitHub");
        var httpResponseMessage = await httpClient.GetAsync(
            "repos/dotnet/AspNetCore.Docs/branches");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync
                <IEnumerable<GitHubBranch>>(contentStream);
        }
        else return Enumerable.Empty<GitHubBranch>();
    }
}
```

## Typed Clients

- Provide the same capabilities as named clients without the need for "magic strings" as keys
- Provides IntelliSense and compiler help when consuming clients
- Encapsulates the logic for interacting with the remote API
  - Single location for configuration and usage
- A typed client accepts an `HttpClient` parameter in its constructor

```csharp
public class GitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com/");

        // The GitHub API requires two headers.
        _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Accept, "application/vnd.github.v3+json");
        _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent, "HttpRequestsSample");
    }

    public async Task<IEnumerable<GitHubBranch>?> GetAspNetCoreDocsBranchesAsync() =>
        await _httpClient.GetFromJsonAsync<IEnumerable<GitHubBranch>>(
            "repos/dotnet/AspNetCore.Docs/branches");
}
```

Register the typed client in `Program.cs`:

```csharp
builder.Services.AddHttpClient<GitHubService>();
```

The typed client (service) can be injected and consumed directly:

```csharp
public class TypedClientController : ControllerBase
{
    private readonly GitHubService _gitHubService;

    public TypedClientController(GitHubService gitHubService)
    {
        _gitHubService = gitHubService;
    }

    // GET: api/TypedClientController
    [HttpGet]
    public async Task<IEnumerable<GitHubBranch>?> Get()
    {
        return await _gitHubService.GetAspNetCoreDocsBranchesAsync();
    }
}
```

## What is Polly?

- Polly is a zero-dependency, lightweight library (NuGet package) that can work anywhere .NET can run
  - Polly helps you navigate the unreliable network
- Polly provides resilience strategies in fluent-to-express policies such as:
  - Retry
  - WaitAndRetry
  - CircuitBreaker
  - Bulkhead Isolation
  - Timeout
  - Fallback

## Installation

Install the `Microsoft.Extensions.Http.Polly` NuGet package:

```bash
dotnet add package Polly
```

| Package | About |
|---|---|
| `Polly.Core` | The core abstractions and built-in strategies |
| `Polly.Extensions` | Telemetry and dependency injection support |
| `Polly.RateLimiting` | Integration with System.Threading.RateLimiting APIs |
| `Polly.Testing` | Testing support for Polly libraries |
| `Polly` | The legacy API exposed by versions of Polly before version 8 |

## Handle transient faults

`AddTransientHttpErrorPolicy` allows a policy to be defined to handle transient errors. Policies configured with it handle the following responses:

- `HttpRequestException`
- HTTP 5xx (server error)
- HTTP 408 Request Timeout

## To add a policy to a Named client

This example creates a policy which will handle typical transient faults, retrying the underlying HTTP request up to 3 times if necessary:

```csharp
// Add a WaitAndRetryAsync policy
builder.Services.AddHttpClient("GitHubWithPolly", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.github.com/");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/vnd.github.v3+json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "HttpRequestsSample");
})
.AddTransientHttpErrorPolicy(policyBuilder =>
    policyBuilder.WaitAndRetryAsync(
        3, retryNumber => TimeSpan.FromMilliseconds(600)));
```

## To add a policy to a Typed client

```csharp
// Register a typed client
builder.Services.AddHttpClient<GitHubService>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(new[] {
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1)
        })
    );
```

## Exponential backoff

Retry a specified number of times, using a function to calculate the duration to wait between retries based on the current retry attempt:

```csharp
Policy
    .Handle<SomeExceptionType>()
    .WaitAndRetry(5, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    );
```

With a callback executed before each retry (e.g. for logging):

```csharp
Policy
    .Handle<SomeExceptionType>()
    .WaitAndRetry(
        5,
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        (exception, timeSpan, retryCount, context) => {
            // Add logic to be executed before each retry, such as logging
        }
    );
```

## Creating a custom HttpMessageHandler

Many services require you to attach an API key to an outgoing request so the request can be tied to your account. Instead of manually adding this header for every request, configure a custom `HttpMessageHandler` to automatically attach the header.

Steps:

1. Create a custom handler by deriving from the `DelegatingHandler` base class
2. Override the `SendAsync()` method to provide your custom behavior; call `base.SendAsync()` to execute the remainder of the handler pipeline
3. Register your handler with the DI container. If your handler does not require state, register it as a singleton service; otherwise register it as a transient service
4. Add the handler to one or more of your named or typed clients by calling `AddHttpMessageHandler<T>()` on an `IHttpClientBuilder`

The order in which you register handlers dictates the order in which they are added to the HttpClient handler pipeline.

## Create a custom handler

```csharp
public class ApiKeyMessageHandler : DelegatingHandler
{
    private readonly AppSettings _settings;

    public ApiKeyMessageHandler(IOptions<AppSettings> settings)
    {
        _settings = settings.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-API-KEY", _settings.ApiKey);
        HttpResponseMessage response =
            await base.SendAsync(request, cancellationToken);
        return response;
    }
}
```

Register with DI in `Program.cs`:

```csharp
builder.Services.AddTransient<ApiKeyMessageHandler>();
```

Add the handler to a client:

```csharp
// Register a typed client
builder.Services.AddHttpClient<GitHubService>()
    .AddHttpMessageHandler<ApiKeyMessageHandler>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(new[] {
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1)
        })
    );
```

## References & Links

- IHttpClientFactory consumption patterns: https://docs.microsoft.com/da-dk/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#consumption-patterns
- Microservices architecture e-book (PDF): https://dotnet.microsoft.com/download/e-book/microservices-architecture/pdf
- Polly: https://docs.microsoft.com/da-dk/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#use-polly-based-handlers og https://github.com/App-vNext/Polly
- Implement resilient applications: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/
- Reliability patterns: https://docs.microsoft.com/da-dk/azure/architecture/framework/resiliency/reliability-patterns
