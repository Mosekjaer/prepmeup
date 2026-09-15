---
title: Entity Framework — Advanced Topics
source: EF Core Advanced.pdf
course_week: 5-6
topic: EF Core + LINQ + mapping
---

# Entity Framework — Advanced Topics

Agenda: LINQ evaluation, seeding the database, existing-database scaffolding, transactions, inserting many records, stored procedures, table valued parameters, logging.

## LINQ is lazy evaluated

A LINQ query is **not executed when you define it** — only when you iterate over it. Lazy evaluation is powerful, since no unnecessary work is done… unless you do things wrong.

**Lazy operators** — return `IEnumerable<T>` and wait:

- `Where()`, `Select()`, `Take()`, `Skip()`, `OrderBy()`, `GroupBy()`

**Terminal operators** — execute instantly:

- `ToList()`, `ToArray()`, `Count()`, `First()`, `Single()`, `Any()`

### A common pitfall

```csharp
var evens = numbers.Where(n => n % 2 == 0);
Console.WriteLine(evens.Count());
Console.WriteLine(evens.Count());
// This query runs twice.
```

The right way:

```csharp
var evens = numbers.Where(n => n % 2 == 0).ToList();
```

`yield return` works like an internal state machine in `IEnumerable<T>`.

## IEnumerable<T> vs IQueryable<T>

EF uses `IQueryable` — the LINQ query is converted to an **expression tree** instead of a delegate.

- With `IEnumerable<T>`: `.Where(u => u.IsActive)` is compiled to IL code and runs **in memory**.
- With `IQueryable<T>` (EF): `.Where(u => u.IsActive)` is **translated to SQL**.

| Feature | IEnumerable | IQueryable |
|---|---|---|
| Executes in | Memory | Database |
| Lambda type | `Func<T, bool>` | `Expression<Func<T, bool>>` |
| Who runs the logic? | .NET runtime | Query provider (EF) |
| Can translate to SQL? | No | Yes |
| Uses yield internally? | Often yes | No |

## Seeding the database

- **Definition**: populating a database with initial data.
- **Implementation**: in EF Core, seeding can be done within the `OnModelCreating` method.
- **Use cases**: reference data, default users, etc.
- **Migrations**: seeding works with migrations to ensure the database is populated when updated.
- Seed data during application startup, or through migrations for consistency.

## Existing database scaffolding

Scaffolding generates C# models and a `DbContext` from an existing database (reverse-engineering the schema).

Requirements:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Command (EF Core CLI):

```bash
dotnet ef dbcontext scaffold "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer
```

Considerations: supports customization after generation; use `--data-annotations` for attributes or `--context` to specify the DbContext name.

## Transactions

A transaction is a sequence of operations performed as a single logical unit of work. EF Core support: `DbContext.Database.BeginTransaction()`, then `transaction.Commit()` or `transaction.Rollback()`.

```csharp
using (var transaction = context.Database.BeginTransaction())
{
    try
    {
        // Perform database operations
        context.SaveChanges();
        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
    }
}
```

## Inserting many records

- This is where EF can struggle.
- Default is `.AddRange()`.
- For better performance, consider third-party libraries (e.g. `EFCore.BulkExtensions`).
- Or use stored procedures and table valued parameters.

## Stored procedures

Precompiled SQL statements stored in the database. In EF Core, execute them with `FromSqlRaw` or `ExecuteSqlRaw`:

```csharp
var result = context.YourEntities.FromSqlRaw("EXEC YourStoredProcedure");
```

Considerations: better for complex queries or database-side logic; can return entities or perform non-query operations.

## Table Valued Parameters

A SQL Server feature, **not natively supported by EF** — used via raw SQL:

```sql
CREATE TYPE MyTableType AS TABLE
(
    Id INT,
    Name NVARCHAR(100)
);

CREATE PROCEDURE MyStoredProcedure
    @MyTableParam MyTableType READONLY
AS
BEGIN
    -- Your logic here
END
```

```csharp
using (var context = new MyDbContext())
{
    var myTableParam = new DataTable();
    myTableParam.Columns.Add("Id", typeof(int));
    myTableParam.Columns.Add("Name", typeof(string));

    myTableParam.Rows.Add(1, "Name1");
    myTableParam.Rows.Add(2, "Name2");

    var parameter = new SqlParameter
    {
        ParameterName = "@MyTableParam",
        SqlDbType = SqlDbType.Structured,
        TypeName = "dbo.MyTableType",
        Value = myTableParam
    };

    await context.Database.ExecuteSqlRawAsync(
        "EXEC MyStoredProcedure @MyTableParam", parameter);
}
```

## Logging

- Purpose: track the application's behavior, diagnose issues, help debugging.
- EF Core logging: use the built-in logging capabilities with `Microsoft.Extensions.Logging`; configure logging at startup.
