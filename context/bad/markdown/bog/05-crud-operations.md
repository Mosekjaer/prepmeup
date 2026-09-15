---
title: CRUD operations
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 5
---

# 5. CRUD operations

**This chapter covers**

- Performing SELECT, INSERT, UPDATE, and DELETE queries
- with EF Core
- Handling requests using the HTTP GET, POST, PUT, and
- DELETE methods
- Implementing paging, sorting, and filtering using EF Core
- Using data-transfer objects (DTOs) to exchange JavaScript
- Object Notation (JSON) data with the client

In chapter 4, we dedicated ourselves to the installation, setup, and configuration of all the prerequisites for interacting with a database management system (DBMS), and we learned the means, techniques, and reasons to do that. Now that we have a database context and an object-relational mapper (ORM) up and running, we’re ready to perform the typical data-related tasks: create, read, update, and delete (CRUD) operations, which are handled by Insert, Select, Update, and Delete SQL queries, respectively. Performing these operations with Entity Framework Core (EF Core) allows our MyBGList web API application to interact with the DBMS we set up in chapter 4.

In this chapter, we’ll take advantage of our data model to perform several read and write operations required by our concrete scenario, using the EF Core’s ApplicationDbContext class. These tasks will help us develop the skills required to interact with our database throughout the rest of the book.

This chapter takes for granted that you already know the basic data manipulation language (DML) operations supported by most DBMSes and how they work. No specific SQL knowledge is required, however, because all these tasks will be handled by the Language Integrated Query (LINQ) component and the IQueryable<T> extension methods that it provides.

## 5.1 Introducing LINQ

Microsoft introduced LINQ in 2007 with the release of .NET Framework 3.5. The purpose of LINQ is to extend the programming language (C#, in our scenario) by allowing the use of query expressions—expressive statements that loosely resemble SQL syntax—to conveniently fetch, process, and manipulate data from any data source in a uniform, standardized, and consistent way. Any C# collection-type class can be queries in LINQ, as long as it implements the generic IEnumerable<T> interface.

TIP This brief introduction recaps the core principles and features of LINQ. For additional info, I suggest reading the official documentation at http://mng.bz/91Yr.

The main advantage of LINQ is rather obvious: it allows developers to use a single unified query language to deal with different types of raw data sources (XML, DBMS, CSV, and more)—as long as an ORM is capable of “mapping” them to Enumerable<T> types. Being able to manipulate our data with C# objects instead of SQL queries is one of the main reasons why we’re using EF Core, after all. We’ll deal with those objects mostly by using LINQ standard methods, as well as an additional set of LINQ-based extension methods provided by EF Core.

### 5.1.1 Query syntax vs. method syntax

The best way to learn LINQ is to use it, which we’re going to do often from now on. First, however, here’s a quick overview of LINQ syntaxes. I use the plural form here because we can choose between two approaches: a declarative query syntax, which loosely resembles SQL queries, and a fluent method syntax, which looks more like standard C# methods. The best way to learn how these syntaxes work is to see them in action. Consider the following example:

var data = new [] { new BoardGame() { Id = 1, Name = "Axis & Allies", Publisher = "Milton Bradley", Year = 1981 }, new BoardGame() { Id = 2, Name = "Citadels", Publisher = "Hans im Glück", Year = 2000 }, new BoardGame() { Id = 3, Name = "Terraforming Mars", Publisher = "FryxGames", Year = 2016 } };

This array isn’t new to us; it’s the same one we’ve used to populate the RestDTO <BoardGame[]> return object of the BoardGameController’s Get() method since chapter 2. Suppose that you want to fetch all the board games published in 2000 from this array, using LINQ. Here’s how we could do that by using query syntax:

var boardgames = from bg in data where bg.Year == 2000 select bg;

The syntax in bold looks rather like a typical SQL query. The statement will return an IEnumerable<BoardGame> containing all the board games that match the where condition (or an empty collection). Now let’s perform the same task by using method syntax:

var boardgames = data.Where(bg => bg.Year == 2000);

Although the two statements will accomplish the same thing, the syntactical difference is quite evident. Query syntax, as its name suggests, looks like a SQL query, whereas method syntax is much like standard C# code, with lambda expressions used as method parameters.

NOTE The Where() method used in this example is one of the many standard LINQ operators that can be used to form the LINQ pattern. Those operators provide several querylike capabilities: filtering, projection, aggregation, sorting, and more. For additional info about them, check out http://mng.bz/jmpe.

If you already know what lambda expressions are and how to use them, feel free to skip the next section. Otherwise, keep reading.

### 5.1.2 Lambda expressions

A lambda expression is a C# expressive operator that can be used to create an anonymous function in the following way:

(input) => { body }

The left part of the expression—the input—can be used to specify one or more input parameters. The right part—the body—contains one or more statements that return the function’s expected result. The input and the body are connected by the => lambda declaration operator. Here’s a typical lambda expression:

(a, b) => { Console.WriteLine($"{a} says hello to {b}"); }

Whenever we have a single input and/or statement, we can omit the corresponding parentheses:

a => Console.WriteLine($"{a} says hello");

In case there are no input parameters, we can use empty parentheses in the following way:

() => Console.WriteLine($"Somebody says hello");

Although lambda expressions aren’t specific to LINQ, they’re used often with LINQ because of their versatility. Starting with C# 10 and ASP.NET 6, they got several syntactical improvements—attributes, explicit return types, natural delegate type inferring, and so on—that greatly improved their use with the Minimal API’s Map methods. We took advantage of these enhancements in chapter 3, when we applied the Cross-Origin Resource Sharing (CORS) and cache attributes in the Program.cs file’s MapGet() from chapter 2: app.MapGet("/error", [EnableCors("AnyOrigin")] [ResponseCache(NoStore = true)] () => Results.Problem());

Lambda expressions play a pivotal role in ASP.NET Core, and we’re going to use them extensively throughout this book.

TIP For further info on lambda expressions, check out the official documentation at http://mng.bz/WA2W.

### 5.1.3 The IQueryable<T> interface

Now that we’ve introduced LINQ and the method syntax we’re going to use, let’s see how the LINQ queries built with EF Core are converted to a database-specific query language (such as SQL). The whole process relies on the IQueryable<T> interface provided by the System.Linq namespace, which can be used to create an in- memory-representation of the query that we want to execute against our database. Here’s an example of how we can provide a IQueryable<BoardGame> instance from an instance of our ApplicationDbContext, which is supposedly referenced by the _context local variable:

var query = _context.BoardGames;

All we did was create a reference to the existing BoardGames property that we set up in chapter 4. This property is of DbSet<BoardGame> type: however, the DbSet<T> type implements the IQueryable<T> interface, thus exposing all the interface’s public methods (and extension methods) due to polymorphism.

DEFINITION In object-oriented programming, the term polymorphism describes accessing objects of different types through the same interface. I don’t delve into this topic, taking for granted that you already know it. For further info regarding polymorphism in C#, take a look at http://mng.bz/81OD.

The query variable holds an in-memory representation of a query that will retrieve all records present in the [BoardGames] database table. In our scenario (SQL Server), it will be converted to the following SQL query:

SELECT * FROM BoardGames;

In most cases, we don’t want to access the whole sequence, but a portion of it. This scenario is one in which LINQ comes into play, thanks to the IQueryable<T> extension methods that it provides. We could add the Where() operator

var query = _context.BoardGames.Where(b => b.Year == 2020);

which will be converted to the following SQL query:

SELECT * FROM BoardGames WHERE Year = 2020;

Because LINQ operators can be chained, we can use many of them together in the following way:

```csharp
var query = _context.BoardGames
    .Where(b => b.Year == 2020)
    .OrderBy(b => b.Name);
```

Alternatively, we can achieve the same result by adding them through separate lines:

```csharp
var query = _context.BoardGames.Where(b => b.Year == 2020);
query = query.OrderBy(b => b.Name);
```

Both approaches result in the following SQL query:

SELECT * FROM BoardGames WHERE Year = 2020 ORDER BY Name;

The query representation built with the IQueryable<T> interface and LINQ operators is called an expression tree. It’s important to understand that expression trees are converted to queries and sent to the database only when we execute the IQueryable<T>, which happens only when the results are consumed by the source code—in other words, when we use an operator that returns something that can be created or evaluated only by querying the database. To better understand this concept, consider this example:

```csharp
var query = _context.BoardGames.Where(b => b.Year == 2020);
var bgArray = query.ToArray();
```

The first line of code creates an IQueryable<T> instance and builds an expression tree without the need to query the database. The second line of code, however, asks for an array of all the board games fetched with the preceding query, which can’t be returned without executing such a query to the database and putting the resulting records in a new instance of the BoardGame[] type.

Our discussion of LINQ and IQueryable<T> ends here. We’ll learn how to make good use of these powerful syntaxes while we work on our app’s source code. First, we’ll focus on using our ApplicationDbContext class inside our existing MyBGList app.

## 5.2 Injecting the DbContext

As we learned in chapter 4, each DbContext instance is meant to be used for a single unit of work that gets executed within a single HTTP request/response lifecycle. The best way to accomplish this task is to have an ApplicationDbContext instance injected into each controller that needs it so that the corresponding action methods will be able to use it.

A great example is our existing BoardGameController's Get() method, which is still using some dummy, manually generated board-game data instead of fetching it from the DBMS. Here’s what we’re going to do to replace this mock behavior with the real deal:

Update the BoardGameController's constructor to obtain an ApplicationDbContext instance using dependency injection. Create a private variable that will locally store a reference to the ApplicationDbContext instance and make it accessible by the controller’s action methods through its lifecycle. Change the Get() method implementation, replacing the dummy data with an actual data retrieval query against our MyBGList database, performed by using the IQueryable<T> extension methods.

The following listing shows how we can pull off that plan (new and modified lines in bold).

**Listing 5.1 BoardGameController.cs file**

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DTO;
using MyBGList.Models;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;    ❶

        private readonly ILogger<BoardGamesController> _logger;

        public BoardGamesController(
            ApplicationDbContext context,                  ❷
            ILogger<BoardGamesController> logger
        )
        {
            _context = context;                            ❷
            _logger = logger;
        }

        [HttpGet(Name = "GetBoardGames")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public RestDTO<BoardGame[]> Get()
        {
            var query =   _context.BoardGames;             ❸

            return new RestDTO<BoardGame[]>()
            {
                Data = query.ToArray(),                    ❹
                Links = new List<LinkDTO> {
                     new LinkDTO(
                         Url.Action(null, "BoardGames",
                             null, Request.Scheme)!,
                         "self",
                         "GET"),
                 }
            };
        }
    }
}
```

❶ Adds the ApplicationDbContext instance
❷ Adds the ApplicationDbContext instance
❸ Creates the IQueryable<T> expression tree
❹ Executes the IQueryable<T>

As we can see, the new code is much thinner and more readable. That’s expected, because we got rid of the manually generated board games and replaced them with a couple of lines of code: the first one to create the IQueryable<T> expression tree and the second to execute it, using the ToArray() method introduced earlier. This method tells EF Core to perform the following tasks:

Perform a Select query against the database to fetch all the records of the [Boardgames] table.

Enumerate the resulting records to create an array of BoardGame entities. Return the resulting array to the caller.

Before going forward, let’s delve into our current implementation to see whether we can improve it further.

### 5.2.1 The sync and async methods

The ToArray() method that we used to retrieve the BoardGame array operates in a synchronous way: the calling thread is blocked while the SQL query is executed in the database. Let’s replace this method with its asynchronous counterpart provided by EF Core. This change will make our BoardGameController’s source code compliant with the async/await pattern enforced by ASP.NET Core, which is generally considered to be a best practice in terms of performance benefits. First, we need to perform the following updates to the Get() method:

1. Replace the method’s signature (and return value) to make it async.
2. Change the ToArray() method to its ToArrayAsync() counterpart.

Here’s how the new code looks (updated code in bold):

```csharp
public async Task<RestDTO<BoardGame[]>> Get()
{
    var query = _context.BoardGames;
```

```csharp
     return new RestDTO<BoardGame[]>()
     {
         Data = await query.ToArrayAsync(),
         Links = new List<LinkDTO> {
             new LinkDTO(
                 Url.Action(null, "BoardGames", null, Request.Scheme)!,
                 "self",
                 "GET"),
         }
     };
}
```

As we can see, to use the ToArrayAsync() method and the await operator, we had to change not only the Get() method’s signature (adding the async modifier), but also its return value (with a Task<TResult> type). In other words, we had to convert the Get() method from synchronous to asynchronous. All these changes will be handled by ASP.NET Core transparently because the async/await pattern is natively supported by the framework. Now we’re ready to test what we’ve done.

Task-based Asynchronous Pattern (TAP) Following the .NET standard, EF Core provides an asynchronous counterpart for any synchronous method that performs I/O operations, with the former always ending with the Async suffix. In our scenario, because we want to optimize our web service performances, performing the DB-related tasks by using the .NET’s Task-based Asynchronous Pattern (TAP) is generally a good idea. This approach allows the system to free the main thread for other tasks, such as handling other incoming calls, while waiting for the asynchronous result, thus improving the efficiency of the web application. For additional info on the async/await pattern, check out the following URLs:

http://mng.bz/ElwR http://mng.bz/Nmwd

### 5.2.2 Testing the ApplicationDbContext

To test our new Get() method, we can visit the following URL, the same one we’ve used many times to retrieve our manually generated list of board games: https://localhost:40443/BoardGames. Sadly, this time we get a rather disappointing JSON response; the data array is empty (figure 5.1).
Figure 5.1 BoardGameController’s Get() method returning an empty “data” array

That result is hardly a surprise. We’ve replaced our dummy data with a real data-retrieval query against a database table, which happens to be empty. To have such an array populated again, we need to seed our database, using the CSV data set that we downloaded in chapter 4.

## 5.3 Seeding the database

It’s time to implement the first part of the CRUD acronym: create the data within the DBMS. As soon as we do, our BoardGameController is able to read the data correctly, paving the way for the remaining Update and Delete tasks.

SQL Server Management Studio, the management tool we learned to use in chapter 4, provides some built-in data import features that work with a lot of structured and semistructured formats, including CSV files. But this tool isn’t ideal when we need to fill multiple tables from a single file, which is precisely our goal. For that reason, to fulfill this requirement, we’ll implement a dedicated Controller class (which we’ll call SeedController) and an action method that will read the CSV file programmatically and create all the corresponding records within our database tables, using EF Core. Here’s what we’ll have to do:

1. Move (or copy) the bgg_dataset.csv file that we downloaded from Kaggle in chapter 4 to a dedicated folder within our ASP.NET Core project.
2. Install a third-party NuGet package that will allow us to read our CSV file and map its content to an IEnumerable<T> collection of C# objects.
3. Create a BggRecord Plain Old CLR Object (POCO) class that the third-party package will use to map the CSV records to C# objects and make them available through an IEnumerable<BggRecord> collection.
4. Add a SeedController to host the action method that will read the CSV file content and feed the database.
5. Implement the CSV reading process, using the third-party package.
6. Iterate through the resulting IEnumerable<BggRecord> collection, and use the data to create the entities and add them to their corresponding MyBGList database tables with EF Core.

### 5.3.1 Setting up the CSV file

The first thing we must do is put the bgg_dataset.csv file containing the board-game-related data in a location that will be accessible by our ASP.NET Core app. We create a new /Data/ subfolder in the MyBGList project’s root folder and move (or copy) the file there.

This folder and its content won’t be accessible by outside users because we didn’t add StaticFilesMiddleware to the application’s pipeline in the Program.cs file. But we’ll be able to access the folder programmatically by using ASP.NET Core from any Controller using the built-in I/O interfaces.

### 5.3.2 Installing the CsvHelper package

Now that we can access the CSV file, we need to find a way to read it in an efficient (and effortless) way. Theoretically speaking, we could implement our own CSV parser, which would also be a great way to improve our C# I/O programming skills. Taking that route, however, would be like reinventing the wheel, because a lot of community- trusted third-party packages can perform this task.

NOTE The “build versus buy” debate is a never-ending dilemma in the IT decision-making process, and software development is no exception. As a general rule, if we don’t have specific requirements for customization, performance, security, and/or backward compatibility, using a community-trusted component developed by a third party instead of implementing something on our own is often reasonable, as long as the costs can be covered and/or the license allows it. We’ll often do the same in this book because it allows us to focus on our main topic. For that reason, in this scenario we’ll handle this task with CsvHelper, an open source .NET library for reading and writing CSV files developed by Josh Close. This library will be our ORM for our bgg_dataset.csv file because we’re going to use it to map each record in that file to a C# POCO class.

NOTE CsvHelper is dual-licensed under MS-PL and Apache 2, so it can be used for free even for commercial purposes. As for its community trust level, the 3.6K stars on GitHub and the 81M NuGet downloads speak for themselves. To see the source code, check out the project’s GitHub repository at https://github.com/JoshClose/CsvHelper.

To install the package, type the following line in the Package Manager console:

Install-Package CsvHelper -Version 30.0.1

To use the dotnet CLI (from the project’s root folder), type

> dotnet add package CsvHelper --version 30.0.1

Alternatively, we can use the NuGet GUI provided by Visual Studio by right-clicking the MyBGList project in the Solution Explorer window and choosing Manage NuGet Packages from the contextual menu. As soon as we’ve got the CsvHelper package installed, we can move to the next step: create the C# POCO class that the library will use to map the CSV records.

### 5.3.3 Creating the BggRecord class

Call the class BggRecord, and place it in a new subfolder inside the existing /Models/ folder—the one that contains our EF Core entities. Create a new /Models/Csv/ sub-folder, right-click it in the Solution Explorer window, and choose Add > New Item > Class (or press Shift+Alt+C) to add a new class file to the project. Call the new file BggRecord.cs, and open it for editing.

The CsvHelper library will use this class to map the contents of each record (row) in the bgg_dataset.csv file. We can use various techniques to configure the mapping process, such as using dedicated map types or a [Name] data attribute provided by the library. For simplicity, we’re going to use the data attributes approach: add a property for each CSV record field, and decorate it with the name of the corresponding header column name.

We already know the structure of the CSV file, because we used it to design our DB schema in chapter 4. Let’s review these header columns by opening the file with a text reader (such as Notepad) and looking at the first line:

```csharp
ID;Name;Year Published;Min Players;Max Players;
Play Time;Min Age;Users Rated;Rating Average;
BGG Rank;Complexity Average;Owned Users;
Mechanics;Domains
```

From these semicolon-separated values, we can infer the list of the properties we need to add to the BggRecord class. More precisely, we need to create a property for each value, possibly using exactly the same name to ensure that the mapping process will work. When using that name isn’t possible, perhaps due to space characters (which can’t be used for C# property names), we can use the [Name] attribute to enforce the mapping. The following listing shows how the resulting class will eventually look.

**Listing 5.2 BggRecord.cs file**

```csharp
using CsvHelper.Configuration.Attributes;

namespace MyBGList.Models.Csv
{
    public class BggRecord
    {
        [Name("ID")]
        public int? ID { get; set; }

        public string? Name { get; set; }

        [Name("Year Published")]
        public int? YearPublished { get; set; }

        [Name("Min Players")]
        public int? MinPlayers { get; set; }

        [Name("Max Players")]
        public int? MaxPlayers { get; set; }

        [Name("Play Time")]
        public int? PlayTime { get; set; }

        [Name("Min Age")]
        public int? MinAge { get; set; }

        [Name("Users Rated")]
        public int? UsersRated { get; set; }

        [Name("Rating Average")]
        public decimal? RatingAverage { get; set; }

        [Name("BGG Rank")]
        public int? BGGRank { get; set; }

        [Name("Complexity Average")]
        public decimal? ComplexityAverage { get; set; }

        [Name("Owned Users")]
        public int? OwnedUsers { get; set; }

        public string? Mechanics { get; set; }

        public string? Domains { get; set; }
     }
}
```

The class is similar to the BoardGame entity that we created in chapter 4, which is hardly a surprise, because most of the CSV record data is stored in the [BoardGames] database table. But the BggRecord class is expected to contain some additional info that we want to store in different tables.

Also, we intentionally defined all the properties as nullable because we can’t rely on the correctness of the CSV file that we’re using as a data source. In other words, we want our ORM to map the CSV record to this object even if some fields are missing instead of throwing an exception or skipping the whole line.

WARNING Our bgg_dataset.csv file has some missing fields. Some of these fields, such as UsersRated and OwnedUsers, can be replaced by a zero default value; others, such as ID and Name, require us to skip the whole record. We’ll see how to deal with both scenarios later.

Now that we have the C# class that will be used to map the CSV records, we can create the controller where the whole CSV-importing and database-seeding process will take place.

### 5.3.4 Adding the SeedController

To create the SeedController, perform the following steps:

1. Add a new SeedController.cs file in the /Controllers/ folder, using the Visual Studio Add > Controller feature and the API Controller - Empty template.
2. Change the [Route] attribute default value from [Route("api/[controller]")] to [Route(" [controller]")] so that the new controller’s routing pattern will be consistent with the one used by the BoardGamesController.
3. Modify the default constructor by injecting an ApplicationDbContext instance using dependency injection, and store its reference in a local private variable as we did with the BoardGameController.

The following listing shows how the resulting SeedController class should look.

**Listing 5.3 SeedController.cs file**

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBGList.Models;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

         private readonly ILogger<SeedController> _logger;

         public SeedController(
             ApplicationDbContext context,
             ILogger<SeedController> logger)
         {
             _context = context;
             _logger = logger;
         }
     }
}
```

We also added the same ILogger interface instance that we have in the BoardGamesController. We’re definitely going to use it later on.

In addition to the ApplicationDBContext and ILogger, we need to request a third instance in this controller through dependency injection: the IWebHostEnvironment, an interface that provides information about the web hosting environment the ASP.NET Core web application is running in. We’re going to need this interface to determine the web application’s root path, which will be required to load the CSV file. The following listing shows the improved SeedController class.

**Listing 5.4 SeedController.cs file (version 2)**

```csharp
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.Models;
using MyBGList.Models.Csv;
using System.Globalization;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;   ❶

        private readonly ILogger<BoardGamesController> _logger;

        public SeedController(
            ApplicationDbContext context,
            IWebHostEnvironment env,                 ❷
            ILogger<BoardGamesController> logger)
        {
             _context = context;
             _env = env;                       ❸
             _logger = logger;
         }
    }
}
```

❶ Adds a local variable
❷ Injects the IWebHostEnvironment
❸ Assigns the injected instance

We’ve already added the using directives for all the namespaces that we’ll need later. Now that the IWebHostEnvironment is available, we can use it to determine the CSV file’s path, load it into a StreamReader object, and pass it to a CsvHelper’s CsvReader class instance that will read the file content. In other words, we’re ready to read our bgg_dataset.csv file.

### 5.3.5 Reading the CSV file

It’s time to see how we can use the CsvHelper library to read our CSV file. The whole importing-and-seeding task will be handled by a dedicated action method, so we’ll be able to launch it whenever we want to populate our database. Because we’re talking about an idempotent task, configuring this method so that it will accept only HTTP PUT requests seems to be the most logical choice.

DEFINITION Idempotency is a property of HTTP methods that ensures that executing subsequent identical requests will have the same effect on the server as executing the first one. Per RFC 7231, the following HTTP methods are considered to be idempotent: GET, HEAD, OPTIONS, TRACE, PUT, and DELETE. All server applications should implement the idempotent semantic correctly, as clients might expect it. For that reason, because we’re dealing with a data seeding task, which will affect our database only the first time it’s called, we should make sure that the process will be triggered only by an idempotent HTTP method such as PUT.

Here’s a brief list of what our Put() action method is expected to do:

Instantiate a CsvConfiguration object to set up some CSV properties (culture settings, delimiter, and so on). Create a StreamReader pointing to the CSV file.

Create an instance of the CsvReader class, which will perform the CSV parsing and map each record to a BggRecord object. Iterate the resulting IEnumerable<BggRecord> collection, create the EF Core entities retrieving the relevant data from each BggRecord entry, and persist them into the database (if they don’t exist yet).

The actual implementation of the SeedController’s Put method takes several lines of code. For readability, we’ll review it by splitting it into smaller blocks, following the four comments placed inside: Setup, Execute, Save, and Recap. As always, the full source code is in the GitHub repository for this chapter. Before reviewing the code, let’s briefly review the attributes we used to decorate the method:

[HttpPut(Name = "Seed")] [ResponseCache(NoStore = true)] As I’ve explained before, we used the [HttpPut] attribute to ensure that the action method will handle only put requests, because it’s handling an idempotent call. We also disabled the cache, using the [ResponseCache] attribute that we used earlier. Now let’s move to the first part of the code, which is the Setup block:

```csharp
var config = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
{
    HasHeaderRecord = true,
    Delimiter = ";",
};
using var reader = new StreamReader(
    System.IO.Path.Combine(_env.ContentRootPath, "Data/bgg_dataset.csv"));
using var csv = new CsvReader(reader, config);
var existingBoardGames = await _context.BoardGames
    .ToDictionaryAsync(bg => bg.Id);
var existingDomains = await _context.Domains
    .ToDictionaryAsync(d => d.Name);
var existingMechanics = await _context.Mechanics
    .ToDictionaryAsync(m => m.Name);
var now = DateTime.Now;
```

Here is where we prepare the objects that the CsvReader (the CSV parser component provided by the CsvHelper library) requires to perform its job:

A CsvConfiguration object containing all the relevant info to read the CSV file: the culture to use for the decimal separator, the field delimiter, and so on. A StreamReader object built against the bgg_dataset.csv file, with the filesystem path determined thanks to the IWebHostingModel instance that we injected into the controller’s constructor (and stored in the _env local variable). An instance of the CsvReader object itself. Three dictionaries that will contain the BoardGame, Domain, and Mechanic records are already present in the database, which we’re going to use to avoid creating duplicates in case of multiple runs (ensuring idempotence). These dictionaries will be empty the first time we run the method, because the database is empty. A DateTime object representing the current date and time, which will be used to assign a value to the CreatedDate and LastModifiedDate properties of the EF Core entities we’re going to create.

The StreamReader and the CsvReader objects have been declared with a using declaration, convenient C# syntax that ensures the correct use of IDisposable (and IAsyncDisposable) objects by disposing of them at the end of the scope. In this case, the end of the scope is the end of the method. Because these two objects implement the IDisposable interface, and we want to be sure that both of them will be released from memory at the end of the process, declaring them with the using keyword is the right thing to do. Let’s proceed with the subsequent Execute block:

var records = csv.GetRecords<BggRecord>(); var skippedRows = 0; foreach (var record in records) { if (!record.ID.HasValue || string.IsNullOrEmpty(record.Name) || existingBoardGames.ContainsKey(record.ID.Value)) { skippedRows++; continue; } var boardgame = new BoardGame() { Id = record.ID.Value, Name = record.Name, BGGRank = record.BGGRank ?? 0, ComplexityAverage = record.ComplexityAverage ?? 0, MaxPlayers = record.MaxPlayers ?? 0, MinAge = record.MinAge ?? 0, MinPlayers = record.MinPlayers ?? 0, OwnedUsers = record.OwnedUsers ?? 0, PlayTime = record.PlayTime ?? 0, RatingAverage = record.RatingAverage ?? 0, UsersRated = record.UsersRated ?? 0, Year = record.YearPublished ?? 0, CreatedDate = now, LastModifiedDate = now, }; _context.BoardGames.Add(boardgame);

if (!string.IsNullOrEmpty(record.Domains)) foreach (var domainName in record.Domains .Split(',', StringSplitOptions.TrimEntries) .Distinct(StringComparer.InvariantCultureIgnoreCase)) { var domain = existingDomains.GetValueOrDefault(domainName); if (domain == null) { domain = new Domain() { Name = domainName, CreatedDate = now, LastModifiedDate = now }; _context.Domains.Add(domain); existingDomains.Add(domainName, domain); } _context.BoardGames_Domains.Add(new BoardGames_Domains() { BoardGame = boardgame, Domain = domain, CreatedDate = now }); }

```csharp
if (!string.IsNullOrEmpty(record.Mechanics))
    foreach (var mechanicName in record.Mechanics
        .Split(',', StringSplitOptions.TrimEntries)
        .Distinct(StringComparer.InvariantCultureIgnoreCase))
    {
        var mechanic = existingMechanics.GetValueOrDefault(mechanicName);
        if (mechanic == null)
        {
               mechanic = new Mechanic()
               {
                   Name = mechanicName,
                   CreatedDate = now,
                   LastModifiedDate = now
               };
               _context.Mechanics.Add(mechanic);
               existingMechanics.Add(mechanicName, mechanic);
           }
           _context.BoardGames_Mechanics.Add(new BoardGames_Mechanics()
           {
               BoardGame = boardgame,
               Mechanic = mechanic,
               CreatedDate = now
           });
       }
}
```

As its name clearly implies, this block is where most of the magic happens. The overall logic is summarized in the following key points:

We use the GetRecords() method of the CsvReader object to obtain the IEnumerable<BggRecord> class, which is stored in a local records variable.

We iterate the IEnumerable with a foreach block to cycle through all the mapped record objects. If the objects lack a required field (ID and/or Name) or have been added already, we skip the whole record, using the continue statement (and count the skip by using the skippedRows counter); otherwise, we keep going. We use the BggRecord data to create an instance of the BoardGame entity and add it to our ApplicationDbContext’s DbSet<BoardGame>. We split the BggRecords’s Domains field with the comma character, because we know that the domains related to each board game are referenced by a comma-separated string containing their names. For each of these names, we check whether we have already created a corresponding Domain entity using the GetValueOrDefault()method in the existingDomains dictionary, which uses the domain name as the key to optimizing the lookup performances. If we find an existing Domain entity, we use it; otherwise, we create a new one and add it to the DbSet<Domains> and to the existingDomains dictionary so that it will be found next time. Next, we create a new BoardGame_Domains entity to add a many-to-many relationship between the newly added BoardGame and each new or existing Domain. We repeat the preceding step for the mechanics, using the Mechanic, the existingMechanics dictionary, the DbSet<Mechanics>, and the BoardGame_ Mechanics entities.

It’s important to understand that we’re not persisting anything in the database yet. All the preceding work is performed in-memory, with EF Core tracking all the added and/or related entities. The relationships are held using temporary key values that EF Core assigns automatically when it adds the entities, using the Add() method. These temporary values will be eventually converted to actual values when EF Core persists the entities in the database. This discussion brings us to the Save block: await _context.SaveChangesAsync();

As we can see, we’re just calling an async method that will persist all changes performed in the current execution context (the ApplicationDbContext instance) to the underlying database. There’s not much more to say about this block, at least for now, but we’ll have to improve it further in a short while. Now let’s move to the last part of the code, which is the Recap block:

return new JsonResult(new { BoardGames = _context.BoardGames.Count(), Domains = _context.Domains.Count(), Mechanics = _context.Mechanics.Count(), SkippedRows = skippedRows });

This block is also simple to read. We set up a JsonResult containing some relevant info about the outcome of our data-seeding task. We do that by using a C# anonymous type, which is a convenient way to encapsulate a set of read-only properties in a single object without defining a type explicitly. We used this technique in previous chapters, so there’s nothing to add.

Our SeedController.Put() method is ready. We have to launch it to see whether it works as expected. (Major spoiler: it doesn’t.)

### 5.3.6 Executing the SeedController

Until this moment, we’ve executed our controllers (and Minimal API) methods by using the browser’s address bar. We’ve done that because all the methods we’ve created so far are meant to handle HTTP GET requests. That’s not the case for our SeedController’s Put() method, which requires an HTTP PUT. To execute it, we need an HTTP client of some sort. Here are a couple of suitable alternatives that we could use:

Postman—An API platform for building and using APIs, available as an online web application or as a Windows app cURL—A command-line tool for getting or sending data, using URL syntax

Both would do the job, but the Postman web app can’t reach the localhost-based address where our MyBGList web application currently runs. For simplicity, we’ll opt for a third option that we already have available: the SwaggerUI, which includes a built-in testing platform that we can use to interact with all our API resources. Furthermore, because the UI is generated automatically from our swagger.json file, we don’t have to input the actual URIs manually. All our API endpoints are already there, ready to be launched.

To access the SwaggerUI, type the following URL in a browser’s address bar: https:// localhost:40443/swagger/index.xhtml. Locate the PUT /Seed endpoint, and expand the HTML panel, using the handle on the right, to expose the Try It Out button, shown in figure 5.2.
Figure 5.2 Trying the PUT /Seed endpoint

Click Try It Out to access the Execute panel, which consists only of an Execute button because the endpoint doesn’t require parameters. Click Execute to start the seeding process (figure 5.3).
Figure 5.3 Executing the seeding process

The process will likely take a few minutes; we’re reading approximately 20K CSV records, after all. Eventually, the Execute panel is updated with a black, console-like text box showing the outcome (figure 5.4).
Figure 5.4 The PUT /Seed endpoint returning an error

As we can see, the seeding process failed (HTTP 500 response). The error message is shown in the response body, which is clearly visible in the text box. By reading the first lines, we should be able to identify the problem that caused the error:

Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details. ---> Microsoft.Data.SqlClient.SqlException (0x80131904): Cannot insert explicit value for identity column in table 'BoardGames' when IDENTITY_INSERT is set to OFF. The problem lies in a SqlException thrown by the EF Core’s SQL Client when trying to write to the BoardGames table. It seems that our SQL Server database doesn’t allow us to set an explicit value in an identity column.

If we retrace the steps we took when we created our BoardGame entity, we can easily explain the error: we decorated the BoardGame.Id property by using the [Key] data annotation attribute because we wanted EF Core to create the corresponding DB table by using the Id column as its primary key. But the autogenerated migration script also assigns the Identity property to this column; its value will be automatically assigned (and autoincremented) by the database itself because the IDENTITY_INSERT flag is set to OFF by default. The error is clearly due to the fact that we’re trying to assign an explicit value taken from the CSV record to the Id property manually instead of accepting the default value and having the database handle it. Now that we understand the root cause of our problem, we have two ways to fix it:

Modify the Execute block of the SeedController.Put() method, removing the BoardGame.Id property assignment, thus leaving it to its default value. Modify the Save block of the SeedController.Put() method, altering the database’s IDENTITY_INSERT flag to allow the manual insertion of identity values.

If we choose the first approach, we will lose the board game’s existing IDs, which is not an option in our given scenario. The IDs are relevant info that we want to preserve by design. For that reason, we’ll choose the other approach. Here’s how we can refactor the Save block of the SeedController.Put() method to set the IDENTITY_INSERT flag value to ON right before saving our entities and then set it back to OFF:

```csharp
// SAVE
using var transaction = _context.Database.BeginTransaction();
_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames ON");
await _context.SaveChangesAsync();
_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames OFF");
transaction.Commit();
```

As we can see by looking at this code, we’ve wrapped the three sets of commands—setting the flag to ON, inserting the entities, and setting the flag back to OFF—within a single transaction, ensuring that they’ll be processed in an atomic manner. Because the IDENTITY_INSERT flag is specific to SQL Server, we had to perform the command by using the ExecuteSqlRaw() method, which executes a raw SQL command directly against the underlying database. In other words, we’re bypassing the ORM, because EF Core doesn’t provide a “managed” way to do that. That’s not surprising, though, because we’re working around a problem that is specific to the SQL Server engine.

Now that we’ve fixed our identity-related problem, we can launch the project, connect to the SwaggerUI, and launch the PUT /Seed endpoint again. This time, we should end up with an HTTP 200 response and the following response body:

{ "boardGames": 20327, "domains": 8, "mechanics": 182, "skippedRows": 16 }

The seeding task was a success. Our database is filled with 8 domains, 182 mechanics, and more than 20K board games, with only 16 skipped CSV records with missing ID and/or Name. For simplicity, we’ll ignore those “incorrect” records for now. We’ll come up with a way to review (and possibly fix) them in the exercises at the end of this chapter.

## 5.4 Reading data

Now that our database has been filled with data, we can execute the BoardGameController’s BoardGames() method—with the changes we applied at the beginning of this chapter—and see what has changed. If everything went according to our plan, we should see a lot of board games instead of the empty array we got last time.

Check it out by executing the following URL in a browser: https://localhost:40443/BoardGames. The outcome should be a huge JSON list of board games (figure 5.5).
Figure 5.5 A long list of board games

This outcome clearly demonstrates that the second part of the CRUD operation—read—has been fulfilled. If we look at the scroll bar on the right, however, we see a potential problem with this output. A JSON object containing so many unordered records would likely be difficult for a typical client to use unless we provide a way to split the board- game data into discrete parts, sort it into a given (or configurable) order, and/or list only those board games that match certain conditions. In other words, we need to implement paging (or pagination), sorting, and filtering processes.

### 5.4.1 Paging

In a client-server programming context, the purpose of paging is to allow the client app to display a limited number of results on each page or view. We can use two techniques to achieve this result:

Client-side paging—The client gets all the data from the server and uses the result to create a multipage UI that users can browse one page at a time. Server-side paging—The client creates a multipage UI that users can browse one page at a time. Each accessed page is populated by asking the server for only the relevant subset of the data.

If we apply this logic to our specific context, in which our web API plays the role of the server, we see that client-side paging requires an endpoint capable of returning all data in a single shot, with the client handling the actual job. In other words, we’re already set!

Conversely, server-side paging requires some additional work. We need to improve our /BoardGames endpoint to return only the subset of board games that the user is supposed to see. This work is what we’re going to do next, assuming that our web client requires this approach.

PageIndex, PageSize, RecordCount To determine the subset of board games to return, our web API needs to know three values:

PageIndex—The zero-based index of the page. When we adopt this naming convention, the first page has an index of 0, the second page has an index of 1, and so on. This value is almost always an input parameter, as it’s meant to be passed by the client with the HTTP request. PageSize—The number of records contained on each page. This value is often an input parameter, because most clients allow the user to configure the number of items to display for each page. Sometimes, however, the server wants to force, override, limit, and/or restrict this number to control the size (and the performance hit) of the response. RecordCount—The total number of records, which can also be used to calculate the total number of available pages. This value is always calculated by the server, because the client has no way of knowing it in advance. But clients often need it, along with the actual data, to calculate the total number of pages (or records) to display to the user. A typical usage scenario for the RecordCount value is the page navigation UI component, which is commonly displayed as a list of clickable numbers and/or an array of PREV, NEXT, FIRST, and/or LAST buttons.

NOTE The PageIndex parameter is sometimes called PageNumber. When the latter name is used, the parameter typically adopts a one- based indexing convention: the first page is marked as 1, the second as 2, and so on. To keep things simple, because both C# and SQL Server work with zero-based indexes, we’ll adopt zero-based indexing (and the PageIndex naming convention) throughout this book.

With all that in mind, we’re now ready to refactor our GET /BoardGames method to take these values into account. We’ll do the following things:

1. Add the pageIndex and pageSize input parameters, assuming that we want to give our clients the chance to determine both of them.
2. Change the EF Core implementation to return only the subset of records delimited by the pageIndex and pageSize values.
3. Calculate the RecordCount value and return it to the client along with the subset of requested data.

The following listing shows how we can pull off these tasks.

**Listing 5.5 BoardGameController.Get() method (with paging) [HttpGet(Name = "GetBoardGames")] [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)] public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, int pageSize = 10) { var query = _context.BoardGames .Skip(pageIndex * pageSize) .Take(pageSize);**

```csharp
    return new RestDTO<BoardGame[]>()
    {
        Data = await query.ToArrayAsync(),
        PageIndex = pageIndex,
        PageSize = pageSize,
        RecordCount = await _context.BoardGames.CountAsync(),
        Links = new List<LinkDTO> {
            new LinkDTO(
              Url.Action(
                  null,
                  "BoardGames",
                  new { pageIndex, pageSize },
                  Request.Scheme)!,
              "self",
              "GET"),
         }
    };
}

As we can see, the paging stuff is handled by the Skip() and
Take() EF Core extension methods, which make good use of the
new input parameters; as their names imply, they skip a number of
```

records equal to the pageIndex multiplied by the pageSize and then take a number of records equal to the pageSize. Because the pageIndex is zero-based, this logic works even for the first page, which requires a skip of zero records.

NOTE We’ve set default values for the PageIndex and PageSize parameters (0 and 10, respectively) to ensure that our implementation will work even if the client’s HTTP request doesn’t set them explicitly. Whenever this happens, our API endpoint will serve only the first page of ten records.

Furthermore, we added the PageIndex, PageSize, and RecordCount properties to the RestDTO so that the client will receive them. While we were there, we updated the value of the Links property, adding a reference to the new pageIndex and pageSize variables, so that the property will keep matching the "self" endpoint URL. If we tried to build our project now, we would undoubtedly get a runtime error, because the PageIndex, PageSize, and RecordCount properties aren’t present in the RestDTO class. Let’s add them, as shown in the following listing, before proceeding.

**Listing 5.6 RestDTO.cs file (with paging properties)**

```csharp
namespace MyBGList.DTO
{
    public class RestDTO<T>
    {
        public T Data { get; set; } = default!;

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public int? RecordCount { get; set; }

        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
```

Now we can run our improved web API and perform a quick paging test.

Testing the paging process Here are the URLs that we can use to retrieve the first three pages of board games, with each page holding five records:

https://localhost:40443/BoardGames?pageIndex=0&pageSize=5 https://localhost:40443/BoardGames?pageIndex=1&pageSize=5 https://localhost:40443/BoardGames?pageIndex=2&pageSize=5

If we did everything correctly, we should receive a much smaller (and more readable) JSON object for each response, because the data array now contains a subset of five board games instead of the whole stack. We still have no way to sort the result of these “pages” in a given order, however. In the next section, we’ll close this gap.

### 5.4.2 Sorting

Like paging, the sorting process can be implemented in two ways:

Using the DBMS—The ORM appends a sorting statement (such as ORDER BY) to the query when asking the database for the data and receives the records already ordered. Programmatically—The ORM retrieves the unordered records and then sorts them in memory on the server side or even on the client side (assuming that we opted for client-side paging or no paging).

The DBMS approach is often preferable for performance reasons because it uses the indexes and caching strategies provided by the database engine. More important, it eliminates the need to fetch all the records from the database—a required task in sorting them programmatically. For that reason, we’re going to follow this route.

NOTE If we think about it for a moment, we see that the performance pitfalls of the programmatic sorting approach closely resemble those of client-side paging. In both cases, the actor performing the job needs to retrieve all the records, even when the goal is to return (or show) only some of them. This disadvantage is particularly evident when programmatic sorting is used in conjunction with paging; these unoptimized scenarios will likely take place in each page view.

The EF Core extension method we can use to delegate the sorting process to the database engine is called OrderBy() and can be used in the following way:

```csharp
var results = await _context.BoardGames
                        .OrderBy(b => b.Name)
                        .ToArrayAsync();
```

The problem with this approach is that the OrderBy() extension method provided by EF Core requires a lambda expression as a parameter. No overloads accept a (dynamic) string value. If we want to allow the client to specify a sort column, we’ll be forced to implement a cumbersome (and ugly) conditional statement, with a condition for each column. Here’s a brief example (viewer discretion advised):

```csharp
var query = _context.BoardGames.AsQueryable();
switch (orderBy)
{
    case "Name":
        query = query.OrderBy(b => b.Name);
        break;
    case "Year":
        query = query.OrderBy(b => b.Year);
        break;
```

// TODO: add other cases (1 for each column we want to sort)

```csharp
}
Data = await query.ToArrayAsync();
```

This approach isn’t a good way to deal with our task. We need to find a way to apply that sorting statement dynamically, possibly using a string representation because that input value is likely to come from a client.

Luckily, we can achieve this result with another open source NuGet package designed with the precise intent to allow string-based query operations through LINQ providers. This package is Dynamic LINQ, developed by the ZZZ Projects community and released under the Apache-2.0 license. To install it, execute the following command in Visual Studio’s Package Manager console:

Install-Package System.Linq.Dynamic.Core -Version 1.2.23

If you prefer to use the dotnet CLI (from the project’s root folder), execute this command:

> dotnet add package System.Linq.Dynamic.Core --version 1.2.23

When the package has been installed, go back to the BoardGameController.cs file, and add the following using declaration to the top to make the Dynamic LINQ extension methods available for use:

using System.Linq.Dynamic.Core;

Now we can update the Get() method by adding a new orderBy input value that we can use to sort our records dynamically, using the DBMS engine. The following listing shows the improved method (new/updated code in bold).

**Listing 5.7 BoardGameController.Get() method (with paging and sorting) [HttpGet(Name = "GetBoardGames")] [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)] public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, int pageSize = 10, string? sortColumn = "Name") { var query = _context.BoardGames .OrderBy(sortColumn) .Skip(pageIndex * pageSize) .Take(pageSize);**

```csharp
    return new RestDTO<BoardGame[]>()
    {
        Data = await query.ToArrayAsync(),
        PageIndex = pageIndex,
        PageSize = pageSize,
        RecordCount = await _context.BoardGames.CountAsync(),
        Links = new List<LinkDTO> {
            new LinkDTO(
                Url.Action(
                    null,
                    "BoardGames",
                    new { pageIndex, pageSize },
                    Request.Scheme)!,
                "self",
                "GET"),
        }
    };
}
```

We’ve added a sortColumn string parameter for the client, which is consumed by the “string-based” overload of the OrderBy() extension method provided by Dynamic LINQ. The OrderBy() method instructs EF Core to add an ORDER BY [sortColumn] statement to the underlying database query and then apply skip-and- take paging to the ordered results.

WARNING In listing 5.7, we’re passing the sortColumn GET parameter to the OrderBy() extension method without checking it properly. This approach is rather unsafe, as it’s likely to lead to unexpected runtime errors and might even pose the risk of SQL injections (if the ORM isn’t equipped to prevent them). I’ll talk more about these kinds of problems and how to deal with them in chapter 6, which introduces data validation and exception handling.

Adding a sort order Before testing what we’ve done, let’s improve our code further by adding a configurable sorting order. Sorting can be applied in two ways: ascending and descending. In general terms, ascending means smallest to largest (0-9, A-Z, and so on), whereas descending means largest to smallest (9-0, Z-A, and the like).

In most DBMSes, including SQL Server, the order can be set by the ASC and DESC keywords, with ASC being the default. In our current implementation, the OrderBy() method always sorts our records in ascending order, regardless of the column set with the sortColumn input parameter.

Suppose that we want to improve this behavior by allowing the client to choose the sort order instead of taking it for granted. Luckily, thanks to the Dynamic LINQ library, we can easily do that, because the OrderBy() overload we’re using allows us to specify an optional, space-separated ASC or DESC keyword right after the column name. All we have to do is add a new sortOrder input parameter to our GET /BoardGames method:

public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, int pageSize = 10, string? sortColumn = "Name", string? sortOrder = "ASC")

We append it to the column name in the existing OrderBy() string parameter in the following way:

.OrderBy($"{sortColumn} {sortOrder}") Now the client can set the sort column and the sort order. As always, we provided the new parameter with a suitable default value ("ASC") that will be used whenever the client doesn’t use that feature.

Testing the sorting process To test our sort, we can use the following URLs:

https://localhost:40443/BoardGames?sortColumn=Name to retrieve the first ten records, sorted by Name (ascending)

https://localhost:40443/BoardGames?sortColumn=Year to retrieve the first ten records, sorted by Year (ascending) https://localhost:40443/BoardGames? sortColumn=Year&sortOrder=DESC to retrieve the first ten records, sorted by Year (descending)

Next, we’ll implement the last (but not least) read-related feature.

### 5.4.3 Filtering

Filtering doesn’t need an introduction. We use it while surfing the web, because filtering is a pivotal feature of any search engine, as well as Wikipedia, Twitter, and any web (or nonweb) application that provides “browsable” data to users. Like sorting, filtering can be implemented by using the DBMS or programmatically, with the same caveats. For the same reasons, we’ll take the DBMS-driven approach. Adding a filter logic Implementing a DBMS-driven filtering process in our existing GET /BoardGame endpoint is a rather easy task, especially considering that we have only a single string parameter to deal with: the board game’s Name. Here’s what we need to do:

1. Add a filterQuery input parameter, which the client(s) will use to specify the actual lookup query.
2. Use the filterQuery parameter in a new Where() operator that will instruct EF Core to retrieve only records with a matching Name from the database.

The following listing shows the implementation (new/updated code in bold).

**Listing 5.8 BoardGameController.Get() method (with paging, sorting, and filtering) [HttpGet(Name = "GetBoardGames")] [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)] public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, int pageSize = 10, string? sortColumn = "Name", string? sortOrder = "ASC", string? filterQuery = null)                                   ❶ { var query = _context.BoardGames.AsQueryable();                ❷ if (!string.IsNullOrEmpty(filterQuery))                       ❸ query = query.Where(b => b.Name.Contains(filterQuery)); var recordCount = await query.CountAsync();                   ❹ query = query .OrderBy($"{sortColumn} {sortOrder}") .Skip(pageIndex * pageSize) .Take(pageSize); return new RestDTO<BoardGame[]>() { Data = await query.ToArrayAsync(), PageIndex = pageIndex, PageSize = pageSize, RecordCount = recordCount, Links = new List<LinkDTO> { new LinkDTO( Url.Action( null, "BoardGames", new { pageIndex, pageSize }, Request.Scheme)!, "self", "GET"), } }; }**

```csharp
```

❶ Adds the filterQuery parameter
❷ Handles the DbSet as an IQueryable object
❸ Conditionally applies the filter
❹ Determines the record count

As we can see, this time we had to set a NULL default value for our new filterQuery parameter, because providing a “default” filter wouldn’t make much sense. For that reason, we had to find a way to add the Where() operator conditionally so that it would be applied to the expression tree only if the filter query is null or empty. Two more things are worth noting:

We had to use the AsQueryable() method to cast the DbSet<BoardGame> explicitly to an IQueryable<BoardGame> interface so that we could “chain” the extension methods to build the expression tree the way we did. We added a recordCount local variable and used it to pull the record count from the database sooner so that we could take the filter parameter into account before performing the paging tasks.

Testing the filtering process We can use these URLs to test our brand-new filter behavior:

https://localhost:40443/BoardGames?filterQuery=diplomacy to retrieve the top ten board games sorted by Name (ascending), with a Name containing "diplomacy" (spoiler: only five) https://localhost:40443/BoardGames?filterQuery=war to retrieve the first ten board games sorted by Name (ascending) with a Name containing "war"

## 5.5 Updating and deleting data

Now that we’ve learned how to create and read data, we’re ready to tackle the last two parts of the CRUD acronym: update and delete. Those operations, like create, typically require elevated privileges to be used because they’ll likely make permanent changes in the data source. I discuss this topic in chapter 9, which introduces authentication and authorization. For now, we’ll stick to a sample implementation that allows us to learn the basics.

### 5.5.1 Updating a BoardGame

The first decision we should make is which HTTP method to use to perform the Update process. Should we use an idempotent HTTP method, as we did with SeedController.Put()? The answer depends on how we plan to implement the Update process:

If we can ensure that multiple identical Update operations will have the same outcome on the server, we can use an idempotent HTTP method such as PUT, which (according to RFCs 2616 and 7241) is meant to be used to create or replace a given, targeted resource by using a standardized intent. If we can’t guarantee the prior result, we should use a nonidempotent HTTP method such as POST, which (according to the same RFCs) is meant to be used to create a new resource or otherwise interact with an existing resource by using a nonstandardized intent.

In our scenario, we can already answer this question. Because we intentionally created a LastModifiedDate column in our BoardGames table (and entity), we’ll likely want to update it every time an update is performed on a record so we can keep track of it. Our Update operation will be nonidempotent because it will have a different, unique effect on our database each time it’s executed. For that reason, we’re going to use the HTTP POST method. Now that we’ve settled that matter, we can move to implementation.

Creating the BoardGameDTO The first thing to do is create a DTO that will allow authorized clients to send the board-game data to update. Reasons to use a DTO Theoretically speaking, instead of creating a DTO, we could use one of our existing classes: the EF Core’s BoardGame entity that we’ve worked with since chapter 4 or the BggRecord entity that we created for the CsvHelper library. Using entities for DTOs, however, is widely considered to be bad practice: it breaks the single- responsibility principle, which states that every module, class, or function should have responsibility for a single part of that program’s functionality.

Entity classes are meant to be object wrappers for database tables and views. Directly using them to “configure” the JSON data output for our client-side app isn’t a good idea, for several reasons. An entity might contain a lot of data that the user and/or the client-side app should never be able to see or transmit, such as password hashes, personal data, and so on. Masking or ignoring these properties would force the developer to overcomplicate the source code, eventually leading to a confusing codebase. In this book, we’ll adopt the good practice of creating DTO objects whenever we need to transmit data from and to our clients within our API endpoints.

For simplicity, we’ll implement a minimal BoardGameDTO class with three properties:

Id, which will be used to locate the board-game record to update Name and Year, assuming that they’re the only fields we want our clients to be able to update

Let’s create a new /DTO/BoardGameDTO.cs file with the source code shown in the following listing.

**Listing 5.9 BoardGameDTO.cs file**

```csharp
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBGList.DTO
{
     public class BoardGameDTO
     {
         [Required]
         public int Id { get; set; }

         public string? Name { get; set; }

         public int? Year { get; set; }
     }
}
```

Now that we have the DTO class, let’s see how to make good use of it.

Adding the Post method The next step of the plan involves opening our BoardGameController.cs file and adding a new Post() action method that will do the following things:

1. Accept a JSON input with the same structure as the BoardGameDTO class.
2. Check for the existence of a board-game record with the given Id.
3. If the record is found, update its Name and/or Year accordingly, and also update the LastModifiedDate field to keep track of the Update operation.

4. Save the updated record in the DBMS.
5. Return a RestDTO object containing the relevant info to the client.

The following listing shows how we can implement all those features.
**Listing 5.10 BoardGameController’s Post method**

```csharp
[HttpPost(Name = "UpdateBoardGame")]
[ResponseCache(NoStore = true)]
public async Task<RestDTO<BoardGame?>> Post(BoardGameDTO model)
{
    var boardgame = await _context.BoardGames
        .Where(b => b.Id == model.Id)
        .FirstOrDefaultAsync();
    if (boardgame != null)
    {
        if (!string.IsNullOrEmpty(model.Name))
            boardgame.Name = model.Name;
        if (model.Year.HasValue && model.Year.Value > 0)
            boardgame.Year = model.Year.Value;
        boardgame.LastModifiedDate = DateTime.Now;
        _context.BoardGames.Update(boardgame);
        await _context.SaveChangesAsync();
    };

    return new RestDTO<BoardGame?>()
    {
        Data = boardgame,
        Links = new List<LinkDTO>
        {
            new LinkDTO(
                    Url.Action(
                         null,
                         "BoardGames",
                         model,
                         Request.Scheme)!,
                    "self",
                    "POST"),
        }
    };
}
```

Now we can test what we’ve done by using the SwaggerUI.

Testing the update process Press F5 or click the Run button to execute the MyBGList project, which should automatically launch the browser with the SwaggerUI’s main dashboard URL in the address bar: https://localhost:40443/swagger/index.xhtml. Locate the POST /BoardGames endpoint; expand its panel by using the right handle; and click Try It Out, as we did with the PUT /Seed endpoint earlier. This time, the SwaggerUI asks us to fill out the request body, because this method requires parameters (figure 5.6).

Figure 5.6 Trying the POST /BoardGames endpoint Fill out the JSON request with the following values to change the Name of the Risk board game (Id 181) to “Risk!” and the publishing Year from 1959 to 1980:

{ "id": 181, "name": "Risk!", "year": 1980 }

Before executing the request, it could be wise to place a breakpoint at the start of the Post() method to check how the server will handle this request: once done, click the Execute button to perform the test. If everything goes as expected, we should end up with an HTTP 200 response and a RestDTO<BoardGame> object containing the BoardGame entity, with the newly updated Name, Year, and LastModifiedDate in its data property.

Now that we have proof that our method works, we can revert the changes by executing the POST /BoardGames endpoint a second time, with the following values in the JSON request body:

{ "id": 181, "name": "Risk", "year": 1959 }

This code reverts the Risk board game to its original state except for the LastModifiedDate, which permanently keeps track of what we did.

### 5.5.2 Deleting a BoardGame

Unlike the Update process, which requires a nonidempotent HTTP method (in our scenario) because we had to deal with the LastModifiedDate, the Delete operation is almost always implemented with an idempotent approach. The reason is simple: assuming that the record to delete is targeted (typically by means of the Id primary key), executing the deletion multiple times against the same target (Id) should have the same effect on the server as executing it once. For that reason, we’ll use the HttpDelete method, which (not surprisingly) is among the idempotent ones.

Adding the Delete method Let’s start with the usual plan. Here’s what our new Delete() method should do:

1. Accept an integer-type input containing the Id of the board game we want to delete.
2. Check for the existence of a board-game record with the given Id.
3. If the record is found, delete it, and persist the changes in the DBMS.
4. Return a RestDTO object containing the relevant info to the client.

The following listing shows the implementation.

**Listing 5.11 BoardGameController’s Delete method**

```csharp
[HttpDelete(Name = "DeleteBoardGame")]
[ResponseCache(NoStore = true)]
public async Task<RestDTO<BoardGame?>> Delete(int id)
{
    var boardgame = await _context.BoardGames
        .Where(b => b.Id == id)
        .FirstOrDefaultAsync();
    if (boardgame != null)
    {
        _context.BoardGames.Remove(boardgame);
        await _context.SaveChangesAsync();
    };

    return new RestDTO<BoardGame?>()
    {
        Data = boardgame,
        Links = new List<LinkDTO>
        {
            new LinkDTO(
                    Url.Action(
                         null,
                         "BoardGames",
                         id,
                         Request.Scheme)!,
                    "self",
                    "DELETE"),
        }
    };
}
```

The source code looks loosely like the Update method, with less work to do; all we have to do is check for the Id key and act accordingly. Let’s test whether the method works as expected by using the SwaggerUI.

Testing the delete process Launch the MyBGList project, and have the browser point again to the SwaggerUI main dashboard’s URL: https://localhost:40443/swagger/index.xhtml. Locate the DELETE /BoardGames endpoint, expand its panel by using the handle, and click Try It Out. This time, we’re asked to specify a single Id parameter. Pay attention now: we’re going to delete a board game from our database! But don’t worry—we’ll have it back shortly after performing the test.

TIP As always, it’s strongly advisable to put a breakpoint at the start of the Delete method to keep track of what happens.

Let’s use the Risk board game we updated (and reverted) earlier. Type 181 in the Id text box and click Execute to proceed. If everything goes the way it should, we’ll receive an HTTP 200 response with the targeted board-game info in the RestDTO’s data property—a sign that the board game has been found (and deleted).

To double-check, click Execute again. This time, the RestDTO’s data property is null, confirming that the board game with Id 181 doesn’t exist in the database. Now that the test is over, we may want to get the Risk board game back. We don’t want to lose it, right? Luckily, we already have a convenient method that can bring it back.

Reseeding the database If we think back to how we implemented the SeedController.Put() method, we should remember that it uses an existingBoardGames dictionary filled with existing records to identify and skip them by using the Id key, thus averting duplicate entries. Thanks to that dictionary, if we execute that method multiple times, all the records present in the CSV file will be skipped —except the Risk board game with Id 181, because it’s not present anymore. In other words, all we need to do to add the Risk board game back to the database is execute the SeedController.Put() method. Let’s try that approach. Launch the MyBGList project again, access the SwaggerUI main dashboard, and execute the PUT /Seed endpoint. If everything works the way it should, we should have our Risk board game back in (almost) no time.

This chapter concludes our journey through CRUD operations. In the following chapters, we switch to error handling, data validation, and logging.

## 5.6 Exercises

The following exercises emulate some task assignments given by our product owner and addressed to the MyBGList development team (us).

TIP The solutions to the exercises are available on GitHub in the /Chapter_05/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 5.6.1 Create

Modify the SeedController’s Put() method so that it accepts an optional Id parameter of an integer type. If such a parameter is present, the method will add only the board game that has that Id, skipping all the others; otherwise, it acts like it already does.

### 5.6.2 Read

Change the filtering behavior of the BoardGamesController’s Get() method. Instead of returning the board games with a Name that contains the filterQuery, the method should return board games with a Name that starts with the filterQuery.

### 5.6.3 Update

Improve the capabilities of the BoardGamesController’s Post method so that it can also update the following columns of the [BoardGames] table: MinPlayers, MaxPlayers, PlayTime, and MinAge. To implement this change properly, we also need to update the BoardGameDTO type.

### 5.6.4 Delete

Modify the BoardGamesController’s Delete() method, replacing the current id integer parameter with a new idList string parameter, which clients will use to specify a comma-separated list of Id keys instead of a single Id. The improved method should perform the following tasks:

1. Ensure that every single id contained in the idList parameter is of an integer type.

2. Delete all the board games that match one of the given id keys.
3. Return a RestDTO<BoardGame[]?> JSON object containing all the deleted board games in the data array. Summary CRUD operations strongly rely on the LINQ component provided by ASP.NET Core, which allows us to deal with different types of raw data sources by using a single, unified query language. To use LINQ effectively, we need to gain some knowledge of its syntax and method parameters and operators, as well as the interfaces where its extension methods apply: IEnumerable<T>, which is implemented by most C# object collections. IQueryable<T>, which is used to build expression trees that will eventually be translated into database queries and executed against the DBMS. To access our ApplicationDbContext within our controllers, we need to inject it by using dependency injection. The injection can be done at the constructor level, where we can also assign the injected instance to a local property that will hold its reference for the controller’s lifecycle, thus being accessible by all the action methods. When we have the ApplicationDbContext available, we can implement a create task that will seed our SQL Server database from a CSV data source of board games. The seeding process can be implemented by using the CsvHelper NuGet package, which acts like an ORM for CSV files. The CsvHelper library will help us map the CSV records to DTO classes, which can be used to create the BoardGame entities and add them to the database. As soon as the create process is ready, we can execute it to feed our database with actual records. Then we’ll be able to implement the read, update, and delete operations as well. Each operation requires its own set of input parameters, HTTP method, and LINQ operators.
