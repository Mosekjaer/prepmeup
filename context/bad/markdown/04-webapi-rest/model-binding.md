---
title: Model Binding in ASP.NET Core
source: Model Binding.pdf
course_week: 4
topic: Web APIs + REST + Dapper
---

# Model Binding in ASP.NET Core

## What is model binding?

Model binding maps data from HTTP requests to action method parameters. Parameters may be simple types (strings, integers, floats) or complex types.

```csharp
[Route("[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    public async Task<IActionResult> Edit(int? id) { ... }
    public async Task<IActionResult> Create(Department department) { ... }
}
```

## Binding targets

Model binding tries to find values for:

- Parameters of the controller action method that a request is routed to
- Public properties of a controller, if specified by attributes

`[BindProperty]` — applied to a public property of a controller:

```csharp
public class DepartmentsController : ControllerBase
{
    [BindProperty] public Instructor? Instructor { get; set; }
}
```

`[BindProperties]` — applied to a controller class to target all its public properties:

```csharp
[BindProperties]
public class DepartmentsController : ControllerBase
{
    public Instructor? Instructor { get; set; }
}
```

## Binding sources

Model binding gets data as key-value pairs from these sources in an HTTP request:

1. Form fields
2. The request body (for controllers that have the `[ApiController]` attribute)
3. Route data
4. Query string parameters
5. Uploaded files

If the default source is not correct, use an attribute to specify the source:

```csharp
public class Instructor
{
    [FromQuery(Name = "Note")]
    public string? NoteFromQueryString { get; set; }
}

public class DepartmentsController : ControllerBase
{
    public void OnGet([FromHeader(Name = "Accept-Language")] string language) { ... }
}
```

## How model binding works

By default, Web API binds parameters with these rules:

- For **simple types**, Web API tries to get the value from the URI. Simple types include the .NET primitives (int, bool, double, …), plus `TimeSpan`, `DateTime`, `Guid`, `decimal`, and `string`, plus any type with a type converter that can convert from a string.
- For **complex types**, Web API tries to read the value from the message body.

The default model binders look for simple data values in this order:

1. Routing variables
2. Query strings

Example — `http://localhost:21393/api/pets/2?DogsOnly=true`:

```csharp
[HttpGet("{id}")]
public ActionResult<Pet> GetById(int id, bool dogsOnly)
```

`id = 2` comes from the route, `dogsOnly = true` from the query string; values are converted to int and bool.

### Routing variables vs query strings

`http://localhost:21393/api/pets/3?id=1`

```csharp
[HttpGet("{id}")]
public ActionResult<Pet> GetById(int id) {
    result = id;
}
```

What is the value of `result`? Routing variables take priority over query strings — `result = 3`.

## Body as binding source

When a JavaScript client sends JSON data to an API controller, `[FromBody]` specifies that the request body should be decoded and used as a model binding source. This is the default for ApiControllers.

```javascript
let response = await fetch("/Home/Body", {
    method: "POST",
    body: JSON.stringify({
        firstName: "Bob",
        lastName: "Smith"
    }),
    headers: new Headers({
        'Content-Type': 'application/json'
    })
});
```

```csharp
[HttpPost]
public Person Body([FromBody] Person model)
{
    return model;
}
```

## Handling missing data

Options:

- Cope with default values provided by the model binding system
- Assign default values to the action method parameters
- Make the parameters optional

Safe coding:

```csharp
[HttpGet("{id}")]
public ActionResult<Pet> GetById(int? id)
{
    Pet pet;
    if (id.HasValue && (pet = repository[id.Value]) != null)
    {
        return pet;
    }
    else
    {
        return NotFound();
    }
}
```

## Binding complex types

When the action method parameter is a complex type (any type that cannot be parsed from a single string value), the model binding process uses reflection to get the target type's public properties and binds each of them in turn.

```csharp
[HttpPost]
public IActionResult Create(Person model)
{
    return model;
}

public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Address HomeAddress { get; set; }
    public bool IsApproved { get; set; }
    public Role Role { get; set; }
}
```

Nested complex types are bound recursively:

```csharp
public class Address
{
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}
```

### Default constructor required

All model classes must have a **default constructor** for the model binding process to succeed. A class with only `public Person(int Id) { PersonId = Id; }` will break binding.

## Selective binding (over-post protection)

To guard against overpost attacks, restrict binding to a list of named properties:

```csharp
[HttpPost]
public ViewResult CreateDb([Bind("PersonId, FirstName, LastName")] Person person)
{
    repository.Add(person);
    return View("Index", person);
}
```

Alternatively, create a DTO with just the used properties. You may use the strongly typed syntax: `nameof(AddressSummary.City)`.

### Bind attribute on the model

Apply `[Bind]` to the model class itself for a more widespread effect:

```csharp
[Bind(nameof(FirstName), nameof(LastName))]
public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Address HomeAddress { get; set; }
    public bool IsApproved { get; set; }
    public Role Role { get; set; }
}
```

### The BindNever attribute

Exclude properties explicitly:

```csharp
public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [BindNever]
    public DateTime BirthDate { get; set; }
    public Address HomeAddress { get; set; }
    [BindNever]
    public bool IsApproved { get; set; }
    [BindNever]
    public Role Role { get; set; }
}
```

## Binding to arrays and collections

The default model binder supports array parameters:

```html
@model string[]
<form asp-action="Names" method="post">
    @for (int i = 0; i < 3; i++)
    {
      <div class="form-group">
        <label>Name @(i + 1):</label>
        <input id="names" name="names" class="form-control" />
      </div>
    }
    <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

```csharp
public ViewResult Names(string[] names)
{
    return View(names ?? new string[0]);
}
```

Binding to collection classes:

```csharp
public ViewResult Names2(List<string> names)
{
    return View("Names", names ?? new List<string>());
}
```

Binding to collections of complex types:

```csharp
public ViewResult Address(IList<Person> people)
```

## References

- Building Web APIs with ASP.NET Core, chapter 6
- Pro ASP.NET Core
- [Model binding docs](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0)
