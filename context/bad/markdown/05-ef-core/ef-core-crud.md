---
title: CRUD in ASP.NET Web API and Entity Framework
source: EF Core CRUD.pdf
course_week: 5-6
topic: EF Core + LINQ + mapping
---

# CRUD in ASP.NET Web API and Entity Framework

Agenda: dependency injection and interfaces, repositories, LINQ queries, data transfer objects, HTTP status codes, controllers and endpoints, paging/sorting/filtering.

Shortcut tip: `Ctrl + .` (quick action) — refactor code, generate constructors, initialize private fields, extract interfaces, rename files and namespaces.

## Dependency injection & interfaces

Why use interfaces?

- Decouple your application; increase testability
- Promote SOLID principles (single responsibility, dependency inversion)
- Improved flexibility and extensibility (supports design patterns, easier to swap implementations)
- Reduces boilerplate (simplified object management, centralized configuration)
- Consistent, scalable application architecture; built-in support in .NET Core

How to register:

```csharp
services.AddTransient<ILibraryContext, LibraryContext>();
services.AddScoped<ILibraryContext, LibraryContext>();
services.AddSingleton<ILibraryContext, LibraryContext>();
```

### Service lifetimes

**Transient (short-lived)** — a new instance every time the service is requested.

- Use case: lightweight, stateless services (data transformation utilities/helpers)
- Pros: no shared state, clean independent services
- Cons: can hurt performance if overused with resource-heavy services

**Scoped (request-scoped)** — one instance per scope (per web request in ASP.NET Core).

- Use case: services that maintain state for a single request and are disposed afterward
- Example: database contexts (`DbContext` in Entity Framework)
- Pros: isolated per request, good for multi-user scenarios
- Cons: not shared across requests — can be inefficient if the service is expensive to create

**Singleton (application-wide)** — one instance created on first request, shared for the entire application.

- Use case: shared services like caching or logging; configuration services
- Pros: efficient for expensive reusable objects (HTTP clients, config readers)
- Cons: holds state for the whole application lifecycle — shared state must be managed carefully

Summary: Transient = stateless and lightweight; Scoped = request-level state; Singleton = long-lived shared instances.

## Repositories

EF Core already offers a rich data access API (through `DbSet<>` and LINQ). Why use the repository pattern anyway?

- **Decoupling** — keeps business logic isolated from EF Core, allowing easier replacement of EF Core or the data source
- **Unit testing** — easier to mock the data access layer, avoiding direct dependencies on EF Core
- **Separation of concerns** — separates data access logic from business logic

### Generic repository example

```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
}
```

Specialized repository on top of the generic one:

```csharp
public interface ICarRepository : IRepository<Car>
{
    Task<Car> GetCarByDriverIdAsync(int driverId);
}

public class CarRepository : Repository<Car>, ICarRepository
{
    private readonly DbContext _context;

    public CarRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Car> GetCarByDriverIdAsync(int driverId)
    {
        return await _context.Set<Car>()
            .FirstOrDefaultAsync(car => car.DriverId == driverId);
    }
}
```

## LINQ queries

Language Integrated Query — unified query syntax.

Query syntax (SQL-like):

```csharp
var query = from num in numbers
            where num > 2
            select num;
```

Method syntax (fluent API):

```csharp
var query = numbers.Where(num => num > 2);
var filtered = list.Where(x => x > 3);
var squares = list.Select(x => x * x);
var sorted = list.OrderBy(x => x);
var first = list.FirstOrDefault(x => x > 5);
var anyEven = list.Any(x => x % 2 == 0);
var groups = list.GroupBy(x => x % 2 == 0);
```

Combined:

```csharp
var filteredIds = list.Where(x => x.Number > 3).Select(x => x.Id);
```

Why use LINQ? Readable (declarative), flexible (queries in-memory collections, databases, XML, …), powerful (filtering, sorting, grouping in one unified model), reduced boilerplate (no loops and verbose conditionals).

## Data Transfer Objects (DTOs)

A DTO is a simple object used to transfer data between layers or across the network. It typically contains no business logic, just properties. Purpose: transport data efficiently between systems, especially in distributed architectures (APIs, microservices).

Why use DTOs?

- Separation of concerns
- Security
- Performance
- Data shape customization
- Maintainability

## HTTP status codes

See [MDN — HTTP status codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status).

## Controllers and actions (endpoints)

- **Controller**: a class responsible for handling HTTP requests and returning HTTP responses. Inherits from `ControllerBase` or `Controller`. Acts as intermediary between the client and the business logic/service layer. Naming convention: ends with "Controller" (e.g. `ProductsController`).
- **Action**: a method inside a controller that responds to an HTTP request. Each action corresponds to a specific HTTP verb (GET, POST, PUT, DELETE, …) and returns data (JSON, XML) or status codes.

```csharp
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllBooks()
    {
        var books = _bookService.GetAll();
        return Ok(books);
    }

    [HttpPost]
    public IActionResult AddBook(BookDto book)
    {
        var createdBook = _bookService.Add(book);
        return CreatedAtAction(nameof(GetAllBooks),
            new { id = createdBook.Id }, createdBook);
    }
}
```

Key features of controllers:

- **Routing** — attribute routing (`[Route("api/[controller]")]`) or convention-based routing (patterns defined in startup)
- **HTTP verbs** — actions map to HTTP methods via `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`, …
- **Model binding** — automatically binds request data (query parameters, request body) to action method parameters
- **Return types** — `Ok()` (200 with data), `Created()` (201), `NotFound()` (404), `BadRequest()` (400)

Why use controllers and actions? Separation of concerns (business logic out of controllers), scalability (multiple endpoints per resource), RESTful architecture (HTTP verbs map naturally to CRUD).

## Paging, sorting, and filtering

### Paging

Split large datasets into smaller pages using `Skip(int count)` and `Take(int count)`:

```csharp
int pageNumber = 1;
int pageSize = 10;
var pagedResults = context.Products
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

### Sorting

Order results with `OrderBy()` (ascending) / `OrderByDescending()`:

```csharp
var sortedResults = context.Books
    .OrderBy(p => p.Title)
    .ToList();
```

### Filtering

Restrict retrieved data with `Where()`:

```csharp
var results = context.Books
    .Where(p => p.Author.Name == "Patrick Rothfuss")
    .ToList();
```
