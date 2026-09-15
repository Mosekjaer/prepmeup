---
title: Model Validation and Error Handling in ASP.NET Core
source: Model Validation and Error handling.pdf
course_week: 6
topic: Data validation
---

# Model Validation and Error Handling in ASP.NET Core

Once model binding is complete, validation occurs.

## What is model validation?

- Model validation is the process of ensuring the data received by the application is suitable for binding to the model.
- When this is not the case, it provides useful error information to the user.

## How is it used?

- Controllers check the outcome of the validation process.
- Validation is performed automatically after the model binding process and is usually supplemented with custom validation in a controller class or by using validation attributes.
- Validation rules may be:
  - Defined in attributes on the model classes
  - Defined in attributes on action parameters
  - Explicitly implemented in the action method

## Default model validation

```csharp
[HttpGet(Name = "GetBoardGames")]
public async Task<RestDTO<BoardGame[]>> Get(
    int pageIndex = 0,
    int pageSize = 10,
    string? sortColumn = "Name",
    string? sortOrder = "ASC",
    string? filterQuery = null
    )
```

- `pageIndex` and `pageSize` are expected to be of type integer.
- If we try to pass even one of them with an incompatible value, such as `https://localhost:40443/BoardGames?pageIndex=test`, the application responds with **HTTP 400 - Bad Request** — without even starting to execute the `BoardGamesController`'s `Get` action method.

## Built-in validation attributes

| Attribute | Description |
|---|---|
| `BindRequired` | Useful to ensure form data is complete |
| `Compare` | Ensures that two properties must have the same value |
| `CreditCard` | The data field value is a credit card number |
| `EmailAddress` | Validates an email address |
| `MaxLength` | Specifies the maximum length of array or string data |
| `Phone` | Validates the property has a telephone format |
| `Range` | Validates the property value falls within the given range |
| `RegularExpression` | Validates that the data matches the specified regular expression |
| `Required` | Makes a property required |
| `StringLength` | Validates that a string property has at most the given maximum length |
| `Url` | Validates the property has a URL format |

All the validation attributes support specifying a custom error message by setting a value for the `ErrorMessage` property.

Attribute reference: https://docs.microsoft.com/da-dk/dotnet/api/system.componentmodel.dataannotations

## Validation rules using attributes

Use DataAnnotations to apply validation attributes to the model class:

```csharp
using System.ComponentModel.DataAnnotations;

public class Appointment
{
    [Required]
    [Display(Name = "name")]
    public string ClientName { get; set; }

    [UIHint("Date")]
    [Required(ErrorMessage = "Please enter a date")]
    public DateTime Date { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool TermsAccepted { get; set; }
}
```

## Explicitly validating a model

The most direct way of validating a model is to do so in the action method:

```csharp
[HttpPost]
public ViewResult MakeBooking(Appointment appt) {
    if (string.IsNullOrEmpty(appt.ClientName)) {
        ModelState.AddModelError(nameof(appt.ClientName), "Please enter your name");
    }
    if (ModelState.GetValidationState("Date")
        == ModelValidationState.Valid && DateTime.Now > appt.Date) {
        ModelState.AddModelError(nameof(appt.Date), "Please enter a date in the future");
    }
    if (!appt.TermsAccepted) {
        ModelState.AddModelError(nameof(appt.TermsAccepted), "You must accept the terms");
    }
    if (ModelState.IsValid) {
        return SomeData;
    }
    else {
        return BadRequest();
    }
}
```

## Custom property validation attribute

Some validation rules are specific to your business:

```csharp
public class MustBeTrueAttribute : Attribute, IModelValidator {
    public bool IsRequired => true;
    public string ErrorMessage { get; set; } = "This value must be true";

    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context) {
        bool? value = context.Model as bool?;
        if (!value.HasValue || value.Value == false) {
            return new List<ModelValidationResult> {
                new ModelValidationResult("", ErrorMessage)
            };
        }
        else {
            return Enumerable.Empty<ModelValidationResult>();
        }
    }
}
```

## Error handling

Error handling is the process of anticipating, detecting, classifying, and managing application errors that might happen within the program execution flow.

## Default error response

Calling `https://localhost:40443/BoardGames?pageIndex=test` gives **HTTP 400 - Bad Request** with this body:

```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-a074ebace7131af6561251496331fc65-ef1c633577161417-00",
    "errors": {
        "pageIndex": ["The value 'string' is not valid."]
    }
}
```

This is built-in functionality of the `[ApiController]` attribute that decorates our controllers. The attribute drives the model binding and validation lifecycle: on invalid `ModelState` it automatically returns a 400 response before the action method runs.

## Configuring the ApiController's behavior

You can suppress the automatic 400 response in `Program.cs`:

```csharp
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

// Just before
var app = builder.Build();
```

**Caution:** You will seldom (read: never) need to do this!

## A custom HTTP status code

With the automatic filter suppressed, you can return custom status codes based on the validation errors:

```csharp
if (!ModelState.IsValid) {
  var details = new ValidationProblemDetails(ModelState);
  details.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id
                                      ?? HttpContext.TraceIdentifier;
  if (ModelState.Keys.Any(k => k == "PageSize")) {
    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.2";
    details.Status = StatusCodes.Status501NotImplemented;
    return new ObjectResult(details) {
      StatusCode = StatusCodes.Status501NotImplemented
    };
  }
  else {
    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    details.Status = StatusCodes.Status400BadRequest;
    return new BadRequestObjectResult(details);
  }
}
```

This approach forces the action method's return type to change from `Task<RestDTO<Domain[]>>` to `Task<ActionResult<RestDTO<Domain[]>>>`.

## References & links

- Building Web APIs with ASP.NET Core, chapter 6
- Pro ASP.NET Core
- Model validation: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
