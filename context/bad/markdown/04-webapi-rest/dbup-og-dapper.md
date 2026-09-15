---
title: DbUp & Dapper — an alternative to Entity Framework Core
source: DbUp & Dapper.pdf
course_week: 4
topic: Web APIs + REST + Dapper
---

# DbUp & Dapper

An alternative to Entity Framework Core (SW4BAD, 12 Feb 2026, Jacob Brinth Assenholm).

Learning goals: explain why database schema versioning is necessary, understand how schema drift breaks applications, use DbUp to version and apply SQL migrations, use Dapper for lightweight data access, understand how database schema relates to Web API models, build a small example using DbUp + Dapper together.

## The core problem: keeping database and code in sync

In modern Web API development we typically have:

- Database tables
- Backend models (DTOs / entities)
- API contracts (JSON responses)

These must match. If they don't, things break — often at runtime.

## What is schema drift?

Schema drift happens when:

- One developer changes the DB locally
- Production DB is changed manually
- Different environments have different schemas
- SQL scripts are not tracked in version control

Consequences: runtime errors, missing columns, incorrect data types, broken deployments, impossible rollbacks.

## Database migration tools

We want our database to behave like code: version controlled, repeatable builds, CI/CD friendly, reviewable changes.

## What is DbUp?

DbUp is a **database migration runner for .NET**:

- Runs SQL scripts in order (you define the order)
- Tracks which scripts already ran
- Stores history inside the database
- Works great in CI/CD pipelines
- Keeps schema synchronized across environments

### Transitions, not state

One approach to upgrading existing databases is to "diff" the old state with the new state and manually modify the diff until it works — the "hope and pray" strategy ("leave it to the DBA" for production). DbUp instead models the database as a series of transitions (migration scripts).

### Why not just run SQL scripts manually?

Manual SQL causes: human error, missed scripts, wrong execution order, no history tracking.

DbUp solves this by running scripts automatically, tracking applied scripts, and failing fast if something is wrong.

### How DbUp works conceptually

DbUp:

1. Checks the history table
2. Finds scripts not yet executed
3. Runs them in order
4. Logs success

Scripts live in a folder in the project, numbered to define order, e.g.:

```
scripts/
  0001 - Initial create.sql
  0002 - Alter Books add price.sql
  0003 - Seed data.sql
```

History is stored in a `SchemaVersions` table in the database:

```sql
SELECT TOP (1000) [Id]
      ,[ScriptName]
      ,[Applied]
  FROM [DbUp_demo_dapper].[dbo].[SchemaVersions]
```

Result rows look like `database.scripts.0001 - Initial create.sql | 2026-02-06 14:14:15.730`, one per applied script.

### Benefits

- Safe deployments
- Easier rollbacks (sort of…)
- Reproducible environments
- Better team collaboration
- Works well in Docker + CI/CD

### Drawbacks

- Still kinda manual: you need to update both model and schema
- Requires knowledge of SQL
- Manual setup to be part of CI/CD pipeline

## Dapper — what is it?

Dapper is a **micro ORM** (object-relational mapper), open source. It allows you to:

- Execute raw SQL queries
- Map results to objects
- Execute stored procedures

### Micro ORM vs 'real' ORM

| Feature | Micro ORM (Dapper) | ORM (EF Core) |
|---|---|---|
| Map queries to objects | Yes | Yes |
| Caching results | No | Yes |
| Change tracking | No | Yes |
| SQL generation | No | Yes |
| Identity management | No | Yes |
| Association management | No | Yes |
| Lazy loading | No | Yes |
| Unit of work support | No | Yes |
| Database migrations | No | Yes |

### Use Dapper when

- You want full SQL control
- Performance matters
- You don't need change tracking

## Dapper query methods

### Query (multiple rows) — `Query<T>`

- Returns multiple rows mapped to objects; most common read operation
- Maps columns → properties by name
- Returns `IEnumerable<T>`
- Typical use: get lists, search results, table reads (get all users, all products in category, all orders for customer)

### Query single row — `QuerySingle<T>`

- Expects **exactly one** row
- Throws if 0 rows or more than 1 row returned
- Use when data must exist and the result must be unique (e.g. get user by ID if guaranteed to exist)

### `QuerySingleOrDefault<T>`

- Returns the object if found, `null` if not found
- Throws if more than one row returned
- Use when the record may or may not exist but should still be unique (e.g. login lookup by username)

### `QueryFirst<T>`

- Returns the first row; throws if no rows
- Use when you only care about the first result and the query might return many

### `QueryFirstOrDefault<T>`

- Returns the first row or `null`
- Does NOT care if multiple rows exist
- Use for "give me first match if one exists"

### Execute (write operations) — `Execute()`

- Used for INSERT, UPDATE, DELETE
- Returns the number of rows affected
- Use cases: create new record, update status, delete by ID

### Scalar values — `ExecuteScalar<T>()`

- Returns a single value: first column of first row only
- Use for counts, identity values, calculated values
- Examples: `COUNT(*)`, `SCOPE_IDENTITY()`, `MAX(Date)`

### Async variants

| Scenario | Method |
|---|---|
| Many rows | `QueryAsync` |
| Exactly one row | `QuerySingleAsync` |
| Maybe one row | `QuerySingleOrDefaultAsync` |
| First row only | `QueryFirstAsync` |
| Insert/Update/Delete | `ExecuteAsync` |
| One value | `ExecuteScalarAsync` |

## Parameter handling

Dapper uses anonymous objects for parameters:

```csharp
var sql = "SELECT * FROM Books WHERE BookId = @BookId";

var result = await conn.QueryAsync<Book>(sql, new { BookId = bookId });
```

This provides:

- Protection against SQL injection
- A clean syntax
- Easy mapping

## Works with advanced SQL

- Stored procedures
- Transactions
- Views
- Table Valued Parameters

## Performance notes

- Extremely fast (near raw ADO.NET)
- Minimal overhead
- No change tracking
- No automatic schema generation

## Takeaways

- With DbUp, the database schema becomes part of your application code
- DbUp keeps schema consistent across environments
- Dapper provides fast, simple data access
- Models, SQL schema, and API contracts must stay aligned
