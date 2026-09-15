---
title: Working with data in .NET — EF Core and SQL databases (intro)
source: EF intro.pdf
course_week: 5-6
topic: EF Core + LINQ + mapping
---

# EF Core intro — working with data in .NET

Agenda: right tool for the job, connection strings, ORMs, data modeling, adding EF Core to your project, migrations, keys, properties, relationships.

## Choosing the right tool for the job

**SQL** (MSSQL, MySQL, PostgreSQL, Oracle):

- Vertical scaling (and its challenges)
- Data consistency (ACID: Atomicity, Consistency, Isolation, Durability)
- Normalization

**NoSQL** (MongoDB, Cassandra, Redis):

- Horizontal scaling
- Eventual consistency
- Data partitioning
- Scaling flexibility

## User secrets (connection strings)

Keep connection strings out of source control with the user-secrets CLI:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "qwer1234"
```

Creates a file at `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`.

In `Program.cs`:

```csharp
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];
```

## Object-Relational Mapper (ORM)

Benefits of using ORMs:

- **Abstraction** — no need to write raw SQL queries
- **Productivity** — increases development speed by reducing boilerplate
- **Maintainability** — easier to manage changes in both code and database
- **Security** — reduces the risk of SQL injection vulnerabilities

Popular ORM frameworks in .NET: Entity Framework (EF Core), Dapper.

Considerations: performance, complex queries, learning curve.

## Data modeling approaches

- **Code first** — database is created from the domain model (C# classes). Define classes and relations in code; EF generates the schema. Full control over domain model and database generation; easy to evolve as the model and application grow.
- **Database first** — when the database already exists. Entity models are generated from the database schema (scaffolding of entity classes and DbContext).
- **Model first** — design the data model in a visual designer; EF generates both code and schema. Rarely used.

## Terminology mapping

| Relational database | Object-oriented language |
|---|---|
| Table | Class |
| Column | Property |
| Unique row | Object |
| Rows | Collection of objects |
| Foreign key | Reference |
| SQL — e.g. `WHERE` | .NET LINQ — e.g. `Where(...)` |

## Adding EF Core to your project

Create a new web API project, then install NuGet packages:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Add a context class that inherits from `DbContext`:

```csharp
public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions options) : base(options)
    {
    }
}
```

## Model class

Represents entities/tables. Contains properties with public get/set. Primary key naming convention in EF is either `Id` or `<class>Id`:

```csharp
public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Nationality { get; set; }
}
```

Add it to the DbContext:

```csharp
public DbSet<Author> Authors { get; set; }
```

## Registering the DbContext

In `Program.cs`:

```csharp
builder.Services.AddDbContext<LibraryContext>(options =>
{
    options.UseSqlServer(builder.Configuration["your-secret-key"]);
});
```

## Migrations

Migrations manage changes to your model. They are versioned and contain `Up` and `Down` methods.

```bash
# Install the EF CLI tool
dotnet tool install --global dotnet-ef

# Create a migration
dotnet ef migrations add 0001-InitialCreate

# Apply migrations
dotnet ef database update

# Revert to a previous migration
dotnet ef database update PreviousMigrationName
```

## Keys and other attributes

```csharp
[Table("Forfattere")]
public class Author
{
    [Key]
    public int SomethingId { get; set; }

    [Required]
    [StringLength(30)]
    [Column("FullName", TypeName = "nvarchar(30)")]
    public string Name { get; set; }

    [NotMapped]
    public string Nationality { get; set; }

    [ForeignKey]
    public int SomeId { get; set; }
}
```

## Relationships

### 1 : 1

```csharp
public class Fanclub
{
    public int Id { get; set; }
    public Author Author { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }
    public Fanclub Fanclub { get; set; }
}
```

With Fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Author>()
        .HasOne(a => a.Fanclub)
        .WithOne(f => f.Author)
        .HasForeignKey<Fanclub>();

    base.OnModelCreating(modelBuilder);
}
```

### 1 : many

```csharp
public class Author
{
    public int AuthorId { get; set; }
    public ICollection<Fanclub> Fanclub { get; set; }
}

public class Fanclub
{
    public int Id { get; set; }
    public Author Author { get; set; }
}
```

With Fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Fanclub>()
        .HasOne(f => f.Author)
        .WithMany(a => a.Fanclub);

    base.OnModelCreating(modelBuilder);
}
```

### many : many

```csharp
public class Author
{
    public int AuthorId { get; set; }
    public ICollection<Fanclub> Fanclub { get; set; }
}

public class Fanclub
{
    public int FanclubId { get; set; }
    public ICollection<Author> Author { get; set; }
}
```

With Fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Fanclub>()
        .HasMany(f => f.Author)
        .WithMany(a => a.Fanclub)
        .UsingEntity(x => x.ToTable("AuthorFanclub"));

    base.OnModelCreating(modelBuilder);
}
```

### many : many (enriched join entity)

When the join table carries extra data (here `Members`), model it explicitly:

```csharp
public class Author
{
    // ...
    public ICollection<AuthorFanclub> AuthorFanclub { get; set; }
}

public class Fanclub
{
    // ...
    public ICollection<AuthorFanclub> AuthorFanclub { get; set; }
}

public class AuthorFanclub
{
    public int AuthorId { get; set; }
    public int FanclubId { get; set; }
    public Author Author { get; set; }
    public Fanclub Fanclub { get; set; }
    public int Members { get; set; }
}
```

With Fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<AuthorFanclub>()
        .HasOne(af => af.Author)
        .WithMany(a => a.AuthorFanclub);

    modelBuilder.Entity<AuthorFanclub>()
        .HasOne(af => af.Fanclub)
        .WithMany(f => f.AuthorFanclub);

    base.OnModelCreating(modelBuilder);
}
```
