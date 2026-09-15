---
title: ASP.NET Core Unit Testing
source: Unit Testing Controllers.pdf
course_week: 8
topic: Testing af Web APIs
---

# ASP.NET Core Unit Testing

> "Unit testing is a tool and not a religion… But if you are not testing at all, then you are probably letting users find your bugs and you are officially a bad person" — Adam Freeman

## What are unit tests?

- When unit testing controller logic, only the contents of a single action is tested — not the behavior of its dependencies or of the framework itself.
- Tests that cover the interactions among components that collectively respond to a request are handled by integration tests.

## Choose a test framework

A range of unit test packages is available. The course uses **xUnit.net**:

- Free, open-source unit testing tool for the .NET Framework
- Written by the original inventor of NUnit v2
- The latest technology for unit testing C# and other .NET languages
- Works with ReSharper, CodeRush, TestDriven.NET and Visual Studio
- Part of the .NET Foundation, licensed under Apache 2
- Used by Microsoft to write unit tests for ASP.NET

## Test projects

- Test projects are console apps that contain several tests.
- A test is typically a method that evaluates whether a given class in your app behaves as expected.
- The test project typically has dependencies on at least three components:
  - The .NET Test SDK
  - A unit testing framework, such as xUnit, NUnit, Fixie, or MSTest
  - A test-runner adapter for the chosen framework, so tests can be run via `dotnet test`
- These dependencies are normal NuGet packages.

## Install the .NET SDK templates

xUnit ships templates for creating new projects; they must be installed before use:

```
dotnet new install xunit.v3.templates
```

## Create a test project with xUnit

Create a folder for the test project, change into it, and create the project:

```
dotnet new xunit3
```

- The template creates a console project and adds the required testing NuGet packages to the `.csproj` file.
- There is no `Program` class or `static void Main` in the generated project — it looks like a class library, because the test SDK automatically injects a `Program` class at build time.

## Change to .NET 10

You must manually change the project file from .NET 8.0 to 10.0:

```xml
<PropertyGroup>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
  <OutputType>Exe</OutputType>
  <RootNamespace>ExchangeRates_Tests</RootNamespace>
  <TargetFramework>net10.0</TargetFramework>
  <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
</PropertyGroup>

<ItemGroup>
  <Content Include="xunit.runner.json" CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>

<ItemGroup>
  <Using Include="Xunit" />
</ItemGroup>
```

## An example xUnit unit test

- In xUnit, a test is a method on a public class, decorated with a `[Fact]` attribute.
- The method should be public, with no method arguments.
- The method is `void` — or it can be an async method returning `Task`.
- The method resides inside a public, non-static class.

```csharp
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
    }
}

public class UnitTest2
{
    [Fact]
    public async Task TestA()
    {
        await Task.Delay(1000);
    }
}
```

## Running tests

Run tests from the command line with:

```
dotnet test
```

## Reference your app from your test project

Recommended project structure: right-click the test project and select "Add Project Reference", then select the web project under test. This adds the following to the test project's `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="..\ExchangeRates\ExchangeRates.csproj" />
</ItemGroup>
```

## Unit testing API controllers

- Unit tests are all about isolating behavior; you want to test only the logic contained in the component itself, separate from the behavior of any dependencies.
- Controllers generally shouldn't contain business logic themselves — instead, they should call out to other services.
- Think of controllers as orchestrators, serving as the intermediary between the HTTP interfaces your app exposes and your business logic services. This is called a **thin controller**.

## The API controller under test

```csharp
[ApiController]
[Route("api/currency")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyConverter _converter;

    public CurrencyController(ICurrencyConverter converter)
    {
        _converter = converter;
    }

    [HttpGet]
    public ActionResult<decimal> Convert(ExchangeInputModel model)
    {
        return _converter.ConvertToGbp(
            model.Value,
            model.ExchangeRate,
            model.DecimalPlaces);
    }
}
```

## Why use Moq

- When an object depends on another object, you need a test double of the dependency to isolate the object under test from the rest of the app.
- You can write a test double (stub) manually, use Fakes, or use a mocking framework like **Moq** (installed via NuGet).

## Mocking ICurrencyConverter

```csharp
Mock<ICurrencyConverter> mock = new();
mock.Setup(m => m.ConvertToGbp(It.IsAny<decimal>(),
                               It.IsAny<decimal>(),
                               It.IsAny<int>()))
                               .Returns(3);
var controller = new CurrencyController(mock.Object);
```

## Moq features — the methods of the It class

| Method | Description |
|---|---|
| `Is<T>(predicate)` | Specifies values of type T for which the predicate returns true |
| `IsAny<T>()` | Specifies any value of the type T |
| `IsInRange<T>(min, max, kind)` | Matches if the parameter is between the defined values and of type T. The final parameter is a value from the `Range` enumeration, either `Inclusive` or `Exclusive` |
| `IsRegex(expr)` | Matches a string parameter if it matches the specified regular expression |

Examples:

```csharp
mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v < 0)))
    .Throws<System.ArgumentOutOfRangeException>();
mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100)))
    .Returns<decimal>(total => (total * 0.9M));
mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100,
    Range.Inclusive))).Returns<decimal>(total => total - 5);
```

## Writing unit tests (arrange/act/assert)

- Tests are defined as methods annotated with the `[Fact]` or `[Theory]` attribute.
- Within the method body, methods from the `Assert` class compare the expected result with what actually happened.

```csharp
public class ExchangeRatesUnitTests
{
    [Fact]
    public void Convert_ReturnsValue()
    {
        // arrange
        Mock<ICurrencyConverter> mock = new();
        mock.Setup(m => m.ConvertToGbp(It.IsAny<decimal>(),
                                       It.IsAny<decimal>(),
                                       It.IsAny<int>()))
                                       .Returns(3);
        var controller = new CurrencyController(mock.Object);
        var model = new ExchangeInputModel
        {
            Value = 1,
            ExchangeRate = 3,
            DecimalPlaces = 2,
        };

        // act
        var result = controller.Convert(model);

        // assert
        Assert.Equal(3, result.Value);
    }
}
```

## xUnit.net Assert methods

- `Equal(expected, result)` / `NotEqual(expected, result)`
- `True(result)` / `False(result)`
- `IsType(expected, result)` / `IsNotType(expected, result)`
- `IsNull(result)` / `IsNotNull(result)`
- `InRange(result, low, high)` / `NotInRange(result, low, high)`
- `Throws(exception, expression)`

## Should we unit test controllers?

- If your controllers are fat, unit tests may add value.
- If your controllers are thin, like the `ExchangeRatesController`, they will not add much value for the effort.
- Controllers are the interface between the ASP.NET Core framework and your application — so integration tests or Postman tests will add more value.

## References & links

- ASP.NET Core in Action, 3rd edition, chapter 35 and 36, by Andrew Lock
- Unit testing C# in .NET Core using dotnet test and xUnit: https://docs.microsoft.com/da-dk/dotnet/core/testing/unit-testing-with-dotnet-test
- Testing controller logic in ASP.NET Core: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing
- VS Code unit testing: https://code.visualstudio.com/docs/csharp/testing
- Live Unit Testing in Visual Studio: https://docs.microsoft.com/da-dk/visualstudio/test/live-unit-testing-start?tabs=csharp
- xUnit: https://xunit.net/
- Moq: https://github.com/Moq/moq4/wiki/Quickstart
