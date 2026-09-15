---
title: "SW4BAD: SQL Connections — Microsoft.Data.SqlClient"
source: "SW2BAD - SQL Connections.pdf"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# SQL Connections — programmatic database access from .NET

Contents: using `SqlConnection` as a driver; advanced DDL.

## Microsoft.Data.SqlClient

- Until now the DBMS was accessed via a GUI (Azure Data Studio). Now we take the programmatic approach.
- Import **Microsoft.Data.SqlClient** — an open-source driver to the database.
- One among several driver technologies: ODBC, OLE, ADO.NET, ...

## Add the package

.NET (console):

```bash
mkdir SqlConnectionExample
cd SqlConnectionExample
dotnet new webapi
dotnet add package Microsoft.Data.SqlClient
dotnet run
```

Or via package manager: `PM> Install-Package Microsoft.Data.SqlClient`

## SqlConnection

Creates a connection to the database. You need to set up:

- the **connection string** — configuration to access your database.

You use the connection to build a `SqlCommand` by passing the query string (commands) you want to run:

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    SqlCommand command = new SqlCommand(queryString, connection);
    command.Connection.Open();
    // ...
}
```

## Full example

```csharp
String connectionString = "Data Source=127.0.0.1,1433;Database=Movies;User Id=SA;Password=<YourStrongP@ssword>;TrustServerCertificate=True";

String queryString =
    @"SELECT Title, Year
      FROM Movies;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    SqlCommand command = new SqlCommand(queryString, connection);
    connection.Open();

    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine(String.Format("{0}, {1}", reader[0], reader[1]));
        }
    }
}
```

Key pieces:

- `SqlConnection` — the open connection (dispose with `using`).
- `SqlCommand` — the SQL statement bound to the connection.
- `SqlDataReader` — forward-only reader over the result set; `reader.Read()` advances one row, columns accessed by index (`reader[0]`, `reader[1]`).
