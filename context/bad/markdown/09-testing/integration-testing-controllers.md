---
title: ASP.NET Core Integration Testing
source: Integration Testing Controllers.pdf
course_week: 8
topic: Testing af Web APIs
---

# ASP.NET Core Integration Testing

Integration tests are tests that exercise multiple components at the same time.

## Integration tests in ASP.NET Core

- Integration tests ensure that an app's components function correctly at a level that includes the app's supporting infrastructure, such as the database, file system, and network.
- ASP.NET Core supports integration tests using:
  - A unit test framework with a test web host
  - An in-memory test server

## Integration tests vs. unit tests

Integration tests:

- Use the actual components that the app uses in production
- Require more code and data processing
- Take longer to run

## Separate unit tests from integration tests

Put unit tests and integration tests in different projects. Separating them:

- Helps ensure that infrastructure testing components aren't accidentally included in the unit tests
- Allows control over which set of tests are run

## The important parts

- A test project contains and executes the tests, and has a reference to the SUT (**S**ystem **U**nder **T**est).
- The test project creates a test web host for the SUT and uses a test server client to handle requests and responses with the SUT.
- A test runner executes the tests and reports the results.

## Create an integration test project

From the command line, create a folder for the integration test project, change into it, and run:

```
dotnet new xunit3
```

## Integration test prerequisites

- Change the project type in the integration test project's `.csproj` to the Web SDK:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
```

- Manually change the target framework from .NET 8.0 to 10.0:

```xml
<TargetFramework>net10.0</TargetFramework>
```

- The `xunit.runner.json` is explicitly included in the `.csproj`, but the Web SDK already includes it automatically as a `Content` item. Remove the explicit `<Content>` entry and use `<None>` with `CopyToOutputDirectory` instead, which avoids the conflict:

```xml
<ItemGroup>
  <None Update="xunit.runner.json" CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

## Configure web host

- The test web host is usually configured differently than the app's normal web host — for example, a different database or different app settings might be used for the tests.
- Infrastructure components, such as the test web host and the in-memory test server (`TestServer`), are provided by the `Microsoft.AspNetCore.Mvc.Testing` package:

```
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 10.0.5
```

## The steps

The usual Arrange, Act, and Assert test steps:

1. The SUT's web host is configured
2. A test server client is created to submit requests to the app
3. **Arrange**: the test app prepares a request
4. **Act**: the client submits the request and receives the response
5. **Assert**: the actual response is validated as pass or fail against an expected response
6. The process continues until all tests are executed
7. The test results are reported

## TestHost

- The `Microsoft.AspNetCore.TestHost` library from the ASP.NET Core team can be used to write integration tests for a controller.
- Use this package if you want the possibility to configure the middleware pipeline differently from the real app.

```
dotnet add package Microsoft.AspNetCore.TestHost --version 10.0.5
```

## Reference your app from your test project

Right-click the integration test project, select "Add Project Reference", and select the web project. This adds:

```xml
<ItemGroup>
  <ProjectReference Include="..\ExchangeRates\ExchangeRates.csproj" />
</ItemGroup>
```

## TestHost example

```csharp
public class StatusMiddlewareTestHostTests
{
    [Fact]
    public async Task StatusMiddlewareReturnsPong()
    {
        var hostBuilder = new HostBuilder()
          .ConfigureWebHost(webHost =>
          {
              webHost.UseTestServer();
              webHost.Configure(app =>
                app.UseMiddleware<StatusMiddleware>()
              );
          });

        IHost host = await hostBuilder.StartAsync();
        HttpClient client = host.GetTestClient();

        // Act
        var response = await client.GetAsync("/ping");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("pong", content);
    }
}
```

## WebApplicationFactory

- The `WebApplicationFactory` class (from the `Microsoft.AspNetCore.Mvc.Testing` NuGet package) runs an in-memory version of your real application.
- It uses `TestServer` behind the scenes, but with your app's real configuration, DI service registration, and middleware pipeline.

### Using WebApplicationFactory — setup

- Test classes implement a class fixture interface (`IClassFixture`) to indicate the class contains tests and to provide shared object instances across the tests in the class.
- `CreateClient()` creates an `HttpClient` that automatically follows redirects and handles cookies.

```csharp
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
}
```

### Using WebApplicationFactory — a test

```csharp
[Fact]
public async Task ConvertReturnsExpectedValue()
{
    // Arrange
    HttpClient client = _factory.CreateClient();
    var model = new ExchangeInputModel
    {
        Value = 6,
        ExchangeRate = 3,
        DecimalPlaces = 2,
    };
    var content = JsonContent.Create(model);

    // Act
    var response = await client.PutAsync("/api/currency", content);

    // Assert
    response.EnsureSuccessStatusCode();
    var responseValue = await response.Content.ReadAsStringAsync();

    Assert.Equal("2", responseValue);
}
```

## In-memory database for integration tests

- SQLite is a relational database with an in-memory mode, where the database stays in memory — much faster and easier to create and use for testing.
- EF Core migrations are tailored to a specific database, so you can't run migrations created for SQL Server or PostgreSQL against a SQLite database.
- Consequently, always use `EnsureCreated()` with SQLite tests, which creates the database without running migrations.

### SQLite in-memory example

```csharp
var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();
var options = new DbContextOptionsBuilder<AppDbContext>()
              .UseSqlite(connection)
              .Options;
using (var context = new AppDbContext(options))
{
    context.Database.EnsureCreated();
    context.Recipes.AddRange(
        new Recipe { RecipeId = 1, Name = "Recipe1" },
        new Recipe { RecipeId = 2, Name = "Recipe2" },
        new Recipe { RecipeId = 3, Name = "Recipe3" });
    context.SaveChanges();
}
```

## References & links

- ASP.NET Core in Action, 3rd edition, chapter 35 and 36, by Andrew Lock
- Integration tests in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests
- VS Code unit testing: https://code.visualstudio.com/docs/csharp/testing
- xUnit: https://xunit.net/
