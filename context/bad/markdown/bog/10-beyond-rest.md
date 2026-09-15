---
title: Beyond REST
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 10
---

# 10. Beyond REST

**This chapter covers**

- Overview of some notable REST drawbacks
- GraphQL introduction (pros and cons)
- GraphQL implementation with HotChocolate
- Overview of gRPC Remote Procedure Call (gRPC)
- gRPC implementation with Grpc.AspNetCore
- Other REST alternatives

As we’ve known since chapter 1, the REST architectural paradigm has been the most popular web API architecture for several years, thanks mostly to its scalability, flexibility, and portability, not to mention the huge adoption of the JavaScript Object Notation (JSON) data-exchange format by the IT world. But if we could take a closer look at all the REST web APIs in use nowadays, we’d likely see that only a fraction of them are RESTful. Most REST implementations adhere to only some of the guiding constraints defined by Roy Fielding which we reviewed in chapter 3 and have implemented throughout this book. Nonetheless, all of them have been developed with the REST standard in mind.

This indisputable success is more than justified. When it comes to designing and implementing a web service, REST is almost always a great choice, especially if we’re looking for simplicity, versatility, and performance—the top reasons why it has replaced SOAP almost everywhere. But it’s also important to understand that REST is not the only choice we have or even the best possible alternative in any given circumstance.

In this chapter, we’ll take a comprehensive look at two alternatives: Facebook’s GraphQL and Google Remote Procedure Call (gRPC). Both provide a different way of implementing web APIs and offer some advantages over the REST standard in specific scenarios. We’ll see how we can implement them in ASP.NET Core with the help of some third-party NuGet packages, and we’ll use them to interact with our board-game-related data as we do with our current REST-based approach.

## 10.1 REST drawbacks

Before considering the alternative approaches, let’s acknowledge some well-known downsides and limitations of the REST standard. Rest assured that I’m not going to reveal some major blocking problems that could affect any web API, regardless of its use. The drawbacks I’m about to introduce may have little or no impact in most scenarios, depending on how our data is structured and how our clients are supposed to use it.

But in several circumstances, these limitations could result in nontrivial performance problems, as well as force us to design our endpoints and data-retrieval logic in a suboptimal way. The best way to understand what I mean is to take a close look at the data-fetching capabilities of the MyBGList web API that we’ve been working on— from the client point of view.

### 10.1.1 Overfetching

Whenever we want to retrieve the data of some board games, we need to issue a GET request to the /BoardGames endpoint (with some optional input parameters). By performing this call, we obtain a JSON-serialized RestDTO object containing the JSON representation of some BoardGame entities (paged, sorted, and/or filtered, depending on the input parameters) fetched from the underlying SQL database. We know this behavior all too well, because we’ve called the /BoardGames endpoint many times during our test rounds, and we also know that each of these entities contains several properties.

What if we need only some of them? We may want to populate a list of board games in which we display only each game’s Title and possibly a hyperlink to each game’s detail page, for which we also require the game’s Id. But our current endpoint doesn’t provide a mechanism that lets us choose the properties we need to fetch; we need to get them all. This scenario is an example of overfetching, which means that our clients are often forced to retrieve more data than they need.

NOTE If we think about it, we can see how a certain amount of overfetching will inevitably be present in all our endpoints, because all of them are designed to return the whole entity and all its properties. Unless the client requires all of them, every data-retrieval request received by our web API will ultimately result in some unnecessary data traffic.

This much unnecessary data traffic might have a negligible affect on most small services, but it can become a serious performance hit for large-scale, enterprise-level web APIs. To mitigate this problem, we could adopt some countermeasures, such as the following: Exclude some entity properties from the JSON outcome by decorating them with the [JsonIgnore] data attribute, which works the way its name implies. The attribute works like an on/off switch for all requests of all clients, however, so those “excluded” properties won’t be available to anyone. Create alternative endpoints with DTO classes containing only a fraction of the entity’s properties, ideally matching the client’s needs. This approach, however, will inevitably complicate our web API codebase, eventually leading to code bloat, disorganization, and maintenance troubles, not to mention usability problems due to the presence of multiple endpoints returning similar yet not identical data. Refactor our current endpoint(s) to allow the client to select the properties it wants to retrieve (and/or exclude those it doesn’t need). Implementing this workaround, however, will greatly increase the complexity of our action method’s source code, as well as the public interface of our endpoints. Furthermore, we’ll be forced to abandon our strongly typed entities and data- transfer objects (DTOs) in favor of anonymous objects or dynamic types unless we want to conditionally set their properties or leave them null. Both approaches can lead to interface errors, undesirable behaviors, and source code that’s full of bad practices and hard to maintain.

There’s no easy way to avoid overfetching as long as we’re using a RESTful web API.

### 10.1.2 Underfetching

Let’s go back to the list of board games we want to populate. That list is supposed to contain the Title of each board game, which is also a hyperlink to an endpoint showing the details for that single board game. Currently, we don’t have an endpoint that returns a single board game, but we could easily implement one. Let’s do that.

The action plan is simple: we need to receive the board game Id and return a RestDTO object containing the matching board game (if any). Open the /Controller/ BoardGamesController.cs file, and paste the code in the following listing right below the existing BoardGames method.

**Listing 10.1 BoardGamesController.cs file: GetBoardGame method [HttpGet("{id}")] [ResponseCache(CacheProfileName = "Any-60")] public async Task<RestDTO<BoardGame?>> GetBoardGame(int id) { _logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "GetBoardGame method started.");**

```csharp
    BoardGame? result = null;
    var cacheKey = $"GetBoardGame-{id}";
    if (!_memoryCache.TryGetValue<BoardGame>(cacheKey, out result))
    {
        result = await _context.BoardGames.FirstOrDefaultAsync(bg => bg.Id
➥ == id);
        _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
    }

    return new RestDTO<BoardGame?>()
    {
        Data = result,
        PageIndex = 0,
        PageSize = 1,
        RecordCount = result != null ? 1 : 0,
        Links = new List<LinkDTO> {
            new LinkDTO(
                Url.Action(
                    null,
                        "BoardGames",
                        new { id },
                        Request.Scheme)!,
                    "self",
                    "GET"),
             }
        };
}
```

This code doesn’t contain anything new: we’re accepting a board game’s Id as input, using it to retrieve the board-game entry from the database, and returning it to the caller (if it does exist) within a RestDTO object. Let’s test it. Launch our app in Debug mode, locate the new GET /BoardGames/{id} endpoint in the SwaggerUI, expand the panel, and click the Try It Out button to execute it. Set the id input value to 30 to receive the following response object:

{ "id": 30, "name": "Dark Tower", "year": 1981, "minPlayers": 1, "maxPlayers": 4, "playTime": 90, "minAge": 10, "usersRated": 1261, "ratingAverage": 6.82, "bggRank": 2534, "complexityAverage": 1.81, "ownedUsers": 1655, "createdDate": "2022-04-04T09:14:43.6922169", "lastModifiedDate": "2022-04-04T09:14:43.6922169", "boardGames_Domains": null, "boardGames_Mechanics": null }

Again, we’re receiving all the properties of the resulting board-game entry, which is fine for our current needs, because now we need that data to create a detail view. Notice, however, that the last two properties—boardGames_Domains and boardGames_ Mechanics—are set to null. Does that mean that the Dark Tower board game doesn’t belong to any domain and doesn’t implement any mechanic? No. Those properties are null simply because the data is stored in different database tables and linked through a many- to-many relationship (as we learned in chapters 4 and 5). That data isn’t part of the BoardGame entity unless we explicitly include it in the query—a typical example of underfetching.

NOTE Those properties are null even when we retrieve a list of board games by using the GET /BoardGames endpoint, because we’re not explicitly including the related entities there as well. We didn’t for a good reason: we’d end up overfetching and creating a considerable performance affect, considering that this endpoint might be used to return a considerable number of board games.

Again, we have several ways to work around this problem, such as the following:

Add the missing data to the existing endpoint by explicitly using the Include() method provided by Entity Framework Core (EF Core). This approach could be viable for endpoints that return a single entity, such as the new one we added, but it can’t be done for those that return multiple results, for obvious performance reasons. We could minimize this drawback by linking the Include behavior to the presence of some new input parameters we might add, but this approach would eventually lead to the complexity and maintenance problems resulting from overfetching. Create alternative endpoints that return aggregate data to match the client’s actual needs, such as /GetBoardGamesWithRelatedData. Again, this approach is the same one we could use to deal with overfetching, but it has the same drawbacks: code bloat, maintenance, and usability problems. Create additional endpoints that return the missing data and that the client can call right after the existing one (or in a parallel thread) to retrieve everything it needs, such as /GetDomainsByBoardGameId and /GetMechanicsByBoardGameId. This approach is typically more elegant, reusable, and tidy than the alternative- endpoints approach. But it forces clients to perform several HTTP requests (creating a additional overhead and a performance hit), as well as do a lot of additional work to combine/aggregate the data coming from the various endpoints.

As with overfetching, there’s no easy way to get rid of underfetching problems when working with a RESTful web API. The only thing we can do is find an acceptable compromise among the number of endpoints, their intrinsic complexity (which often can be measured in terms of input parameters), and the specific requirements of the various clients.

This explains why the RESTful API and most general-purpose Software as a Service (SaaS) platforms that need to accommodate thousands of clients with many different needs (PayPal, MailChimp, TrustPilot, Google Analytics, and so on) often end up being surprisingly complex, with lots of endpoints and input parameters. That’s also why the undisputable dominance of the REST API architecture has been challenged in the past few years by some modern alternatives. In the next sections, we’re going to review some of those alternatives and see how we can implement them with ASP.NET Core.

## 10.2 GraphQL

Let’s start our journey with GraphQL, a query language for APIs created by Facebook in 2012 for internal use and then open-sourced in 2015. After its public release, the language gained a lot of popularity throughout the web developer community and was quickly followed by several implementations for most popular programming languages, including JavaScript, Go, PHP, Java, Python, and Ruby.

GraphQL’s success is due mostly to its great flexibility; it allows clients to receive data by using a single declarative query to a single endpoint. In other words, each client can ask for the data fields it needs and receive them (and only them) within a single request/response cycle, avoiding the risk of overfetching and underfetching without having to perform multiple HTTP requests at the same time.

### 10.2.1 GraphQL advantages

To better understand the differences between REST and GraphQL, take a look at figure 10.1. Notice that REST forces us to deal with multiple API endpoints, each of them retrieving data from our database management system (DBMS) by using our implementation strategy and sending a nonconfigurable JSON-serialized result set, eventually leading to a certain amount of overfetching and/or underfetching. As we learned earlier, we can mitigate these problems by adding endpoints. That solution, however, will inevitably require our clients to perform multiple round trips, as well as add complexity and increase development time. Conversely, GraphQL gives us the chance to deal with a single endpoint that will accept a single, standardized data-retrieval query, process and execute it against the DBMS by using its internal engine, and return a combined set of JSON data, avoiding overfetching, underfetching, and multiple round trips.
Figure 10.1 REST and GraphQL comparison

NOTE Both the query and the resulting data will have the same JSON structure, generated by the GraphQL runtime using the standards given by the GraphQL specs. We’ll learn more about these specs (and how to use them) in a short while.

This approach is particularly beneficial for developers (and for the web API codebase in general), as it requires only a single endpoint. Furthermore, because the data is retrieved via a standardized query syntax, there’s no need to implement filtering, sorting, and ordering features manually, based on custom input parameters. All the hard work is done by the underlying GraphQL runtime, assuming that the clients adopt the GraphQL specs.

NOTE The GraphQL specs are available at https://spec.graphql.org. For additional info regarding GraphQL, code samples, use cases, and FAQs, check out the official website at https://graphql.org.

Here’s another notable advantage for developers: because clients can define the exact data they need from the server, the data model can be changed and improved over time with the risk of client-side incompatibilities, possibly even without releasing a new API version. Being able to add fields to the model without breaking the frontend code is a great added value for most microservices, especially if they feed data to multiple clients built with different frameworks (such as websites and mobile apps).

### 10.2.2 GraphQL drawbacks

Although GraphQL has many advantages over traditional REST APIs, it also has some key disadvantages. We can group these disadvantages into three categories: caching, performance, and complexity. Caching problems As we learned in chapter 8, RESTful APIs can use the built-in caching features provided by the HTTP protocol to implement client, intermediate, and/or server-side response caching. GraphQL comes with no native caching support, so we’d need to implement our own standard or rely on third-party libraries. Moreover, implementing an efficient cache mechanism won’t be an easy task, because all requests are addressed to a single endpoint that’s supposed to return highly customized (hence, often different) data sets.

Performance problems Despite their well-known overfetching and underfetching problems, RESTful APIs are typically fast when it comes to issuing a single request to a given endpoint. As long as the data model is thin and/or the number of required round trips is low, the REST architecture can provide great performance and minimal server-side overhead.

Although GraphQL is also meant to be efficient, its underlying engine will likely require some additional overhead. Furthermore, because the data-retrieval queries are issued by the clients, there’s an increased risk of performance loss due to time-consuming or nonoptimized requests. We can mitigate this problem to some extent by implementing (or adopting) a highly efficient GraphQL runtime. But processing custom, client-made queries and executing them against the DBMS is a complex job that can easily lead to unexpected scenarios (circular references, nested loops, nonindexed column lookups, and so on) that could slow or even crash the server. Complexity problems RESTful APIs are easy to learn and implement, thanks mostly to the built-in support provided by most development frameworks, as well as the overall simplicity of the underlying HTTP-based approach and its widespread adoption. GraphQL often comes with no built-in support, has a steeper learning curve, and is still unknown to most developers. Furthermore, it still lacks some important features out of the box— caching, file uploads, and error handling, for example—that are likely to require additional work. Implementing these features (or finding viable workarounds) won’t be as easy as with REST, because community support is still relatively limited. The increased complexity also affects clients. Writing a GraphQL query is much harder than calling a typical REST endpoint, especially if we want to avoid overfetching and underfetching, which is one of the main reasons to use it.

Final words With all its pros and cons, GraphQL provides a viable solution to the most relevant drawbacks of REST APIs. Although it’s not suitable for all scenarios, it can be a valid alternative for dealing with big scalable projects that have rapidly changing data models, as long as the developers are up to the task. In the following sections, we accept the challenge by implementing a full-featured GraphQL API using ASP.NET Core to handle our MyBGList data model.

### 10.2.3 Implementing GraphQL

Because ASP.NET Core comes with no built-in GraphQL support, we have to identify and choose a suitable third-party library that does the heavy lifting unless we want to implement this feature by ourselves. We need a component that’s able to perform the following steps:

1. Expose a GraphQL API endpoint that clients will use to send their queries.
2. Process the incoming requests (with GraphQL queries) against our MyBGList data model.

3. Retrieve the requested data from the underlying DBMS.
4. Provide a suitable HTTP response with the resulting data.

Ideally, steps 2 and 3 (the GraphQL runtime tasks) should be performed with EF Core, which would allow us to preserve our existing data model and everything we’ve done so far. It’s evident that implementing all these requirements from scratch isn’t a good idea; that approach would require a considerable amount of work (and source code), not to mention knowledge that we don’t have (yet). That’s why we’ll opt for the third-party-libraries route.

Choosing the GraphQL library As of today, we have two main third-party alternatives to choose between: GraphQL.NET (https://github.com/graphql-dotnet/graphql- dotnet) and HotChocolate (https://github.com/ChilliCream/hotchocolate). Both packages are open source (MIT-licensed), production-ready, and well-regarded in the ASP.NET developer community. Because we have to make a choice, we’ll go with HotChocolate. It provides a couple of convenient features for our scenario, such as the following: A built-in GraphQL integrated development environment (IDE) called BananaCakePop, which we can use to test the API endpoint without installing a dedicated client, as we’ve done with SwaggerUI Native EF Core support, which is precisely what we’re looking for

Now that we’ve made our choice, we can move to installation.

Installing HotChocolate As always, to install the HotChocolate NuGet packages, we can use the Visual Studio’s NuGet graphical user interface (GUI), the Package Manager console window, or the .NET command-line interface (CLI). Here are the commands to install them by using the .NET CLI:

dotnet add package HotChocolate.AspNetCore --version 12.15.2 dotnet add package HotChocolate.AspNetCore.Authorization --version 12.15.2 dotnet add package HotChocolate.Data.EntityFramework --version 12.15.2

NOTE This version is the latest stable version as of this writing. I strongly suggest using that version to avoid breaking changes, incompatibility problems, and the like.

As we might easily guess, the first package contains the GraphQL services and middleware. The second and third packages contain some useful extension sets that we can use to integrate ASP.NET Core Authorization and EF Core seamlessly into HotChocolate, which will allow us to reuse a lot of work (and source code).

When the package installation is done, try to build the project. If the Implicit Using feature is enabled (the default for most VS2022 C# templates), the RequestDTO object could raise some compiler errors due to an ambiguous reference to the [DefaultValue] attribute. To fix that error, replace the Using statement

using System.ComponentModel;

with

using DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute;

which resolves the ambiguity and makes the errors disappear.

TIP For further info on the Implicit Using feature, introduced with C# version 10, check out http://mng.bz/71mx.

Setting up the GraphQL schema Now that we’ve installed HotChocolate, the first thing need we need to do is set up the GraphQL schema, which defines how we want to expose data to our client and the create, read, update, and delete (CRUD) operations we want to allow. The GraphQL schema is typically composed of one or more root operation types that allow clients to perform read, write, and subscription tasks:

Query—Exposes all the possible data-retrieval queries that we want to make available to our clients. This class serves as a centralized access layer of our data model, with one or more methods corresponding to the various ways to retrieve them. Mutation—Allows clients to perform Insert, Update, and/or Delete operations on our data model. Subscription—Enables a real-time messaging mechanism that clients can use to subscribe to various events and be notified of their occurrence.

The Query root type must always be present, whereas Mutation and Subscription are optional. Furthermore, Query and Mutation root types are stateless, whereas the Subscription type must be stateful because it needs to preserve the document, variables, and context over the lifetime of each active subscription.

TIP For a detailed overview of Query, Mutation, and Subscription root operation types, see http://mng.bz/rdMZ.

To keep things simple, we’ll implement only the Query and Mutation root operation types. These types will allow us to mimic the CRUD functionalities provided by the RESTful approach we’ve been working with.

Adding the Query type Let’s start with the Query. In Visual Studio’s Solution Explorer window, create a new /GraphQL/ top-level folder in our MyBGList project; then add a new Query.cs file to it. We need to create a public method for each entity in our data model that we want our clients to access by using GraphQL, mapping it to its corresponding DbSet. The following listing shows how.

**Listing 10.2 /GraphQL/Query.cs file**

```csharp
using MyBGList.Models;

namespace MyBGList.GraphQL
{
    public class Query
    {
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
         public IQueryable<BoardGame> GetBoardGames(   ❶
             [Service] ApplicationDbContext context)
             => context.BoardGames;

         [Serial]
         [UsePaging]
         [UseProjection]
         [UseFiltering]
         [UseSorting]
         public IQueryable<Domain> GetDomains(         ❷
             [Service] ApplicationDbContext context)
             => context.Domains;

         [Serial]
         [UsePaging]
         [UseProjection]
         [UseFiltering]
         [UseSorting]
         public IQueryable<Mechanic> GetMechanics(     ❸
             [Service] ApplicationDbContext context)
             => context.Mechanics;
    }
}
```

❶ Entry point for BoardGames
❷ Entry point for Domains
❸ Entry point for Mechanics

Two things in this code are worth noting:

Each method is mapped to an EF Core DbSet, returning it as an IQueryable object that GraphQL runtime will use under the hood to retrieve the records from the DBMS. Each method has some data annotation attributes that we’ve never seen before. As we can easily guess, those attributes are part of HotChocolate and allow us to use some powerful built-in features without having to implement them explicitly.

The following list describes the purpose of each attribute:

[Serial]—This attribute configures the GraphQL runtime to execute certain tasks in serial mode rather than in parallel mode, as required by our current ApplicationDbContext implementation. This approach will likely have some performance effects, but it’s an acceptable choice for our sample scenario because it allows us to reuse our source code without refactoring it. [UsePaging]—This attribute adds pagination middleware that allows the GraphQL runtime to paginate results by using the Cursor Connections Specification, a standardized pagination approach adopted by GraphQL specs. [UseProjection]—This attribute adds dedicated middleware that will project the incoming GraphQL queries to database queries through IQueryable objects.

[UseFiltering]—This attribute adds filtering middleware that allows the requesting clients to use filters, which will be translated into Language Integrated Query (LINQ) queries (using the IQueryable object) and then to DBMS queries (using EF Core) by the GraphQL runtime. The available filters will be inferred automatically by the runtime (and shown to the clients) by looking at the IQueryable entity types, as SwaggerUI does with the available REST endpoints. [UseSorting]—This attribute adds sorting middleware that allows the requesting clients to sort results by using a sorting argument, which will be translated into LINQ queries and then into DBMS queries under the hood.

Before continuing, it might be wise to understand why we’re using the [Serial] attribute. When we use the AddDbContext<T> command to register our ApplicationDbContext as a scoped service, a single instance of the class is created and used for the entirety of any given HTTP request. This approach is a good one for typical REST APIs because each endpoint is expected to perform a single DBMS query and return a resulting result set. That’s not the case with GraphQL; the runtime will likely have to perform multiple sets of queries and combine the resulting data into a single aggregate response. These queries, per the runtime’s default behavior, are executed in parallel for performance reasons, making our DbContext crash with one of the following exceptions (depending on its current state):

A second operation started on this context before a previous operation was completed Cannot access a disposed object

Both exceptions are due to the fact that our current DbContext implementation isn’t thread-safe. The [Serial] attribute provides a workaround by forcing the runtime to resolve the queries one after another (serial mode). Alternatively, we could replace our current DbContext registration technique with an approach based on the AddDbContextFactory extension method, registering a factory instead of a single scoped instance. That said, using the attribute, as we did, is a viable workaround for our sample project. The AddDbContextFactory approach would likely be the preferred route for production-level GraphQL APIs.

HotChocolate data attributes For further information about HotChocolate data attributes, check out the following pages of the official docs:

http://mng.bz/Vp6O http://mng.bz/xdYY http://mng.bz/Zo4a http://mng.bz/AlR7 http://mng.bz/2awm

Now that we’ve added our Query root operation type, we’re ready to deal with the Mutation type.

Adding the Mutation type Add a new Mutation.cs file in the /GraphQL/ folder; this folder is where we’ll put the source code for the Mutation root operation type. We’re going to create a public method for the Update and Delete tasks of our BoardGames, Domains, and Mechanics entities (six methods total). This time, the implementation part will be slightly more complex because we’re going to use the DTO objects that we created in chapter 5 instead of the raw entity classes, which forces us to map the properties we want to update manually.
NOTE The source code of the /GraphQL/Mutation.cs file is long. For that reason, I’ve split it into three parts—one for each affected entity. The full source code is available in the GitHub repository for this chapter.

The following listing provides the first part of the source code, containing the namespace and class declaration as well as the Update and Delete methods for the BoardGame entity.

**Listing 10.3 /GraphQL/Mutation.cs file: BoardGame methods using HotChocolate.AspNetCore.Authorization; using Microsoft.EntityFrameworkCore; using MyBGList.Constants; using MyBGList.DTO; using MyBGList.Models;**

```csharp
namespace MyBGList.GraphQL
{
    public class Mutation
    {
        [Serial]
        [Authorize(Roles = new[] { RoleNames.Moderator })]
        public async Task<BoardGame?> UpdateBoardGame(                    ❶
            [Service] ApplicationDbContext context, BoardGameDTO model)
        {
            var boardgame = await context.BoardGames
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();
            if (boardgame != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    boardgame.Name = model.Name;
                if (model.Year.HasValue && model.Year.Value > 0)
                    boardgame.Year = model.Year.Value;
                boardgame.LastModifiedDate = DateTime.Now;
                context.BoardGames.Update(boardgame);
                await context.SaveChangesAsync();
            }
            return boardgame;
        }

        [Serial]
       [Authorize(Roles = new[] { RoleNames.Administrator })]
       public async Task DeleteBoardGame(                                 ❷
           [Service] ApplicationDbContext context, int id)
       {
           var boardgame = await context.BoardGames
               .Where(b => b.Id == id)
               .FirstOrDefaultAsync();
           if (boardgame != null)
           {
               context.BoardGames.Remove(boardgame);
               await context.SaveChangesAsync();
           }
       }
```

❶ Updates a BoardGame
❷ Deletes a BoardGame

The code in the following listing contains the Update and Delete methods for the Domain entity, which are almost identical to their BoardGame counterparts.

**Listing 10.4 /GraphQL/Mutation.cs file: Domain methods**

```csharp
       [Serial]
       [Authorize(Roles = new[] { RoleNames.Moderator })]
       public async Task<Domain?> UpdateDomain(                       ❶
           [Service] ApplicationDbContext context, DomainDTO model)
       {
           var domain = await context.Domains
               .Where(d => d.Id == model.Id)
               .FirstOrDefaultAsync();
           if (domain != null)
           {
               if (!string.IsNullOrEmpty(model.Name))
                   domain.Name = model.Name;
               domain.LastModifiedDate = DateTime.Now;
               context.Domains.Update(domain);
               await context.SaveChangesAsync();
           }
           return domain;
       }

       [Serial]
       [Authorize(Roles = new[] { RoleNames.Administrator })]
       public async Task DeleteDomain(                                ❷
            [Service] ApplicationDbContext context, int id)
        {
            var domain = await context.Domains
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
            if (domain != null)
            {
                context.Domains.Remove(domain);
                await context.SaveChangesAsync();
            }
        }
```

❶ Updates a Domain
❷ Deletes a Domain

The following listing shows the methods for the Mechanic entity, which follows the same approach.

**Listing 10.5 /GraphQL/Mutation.cs file: Mechanic methods [Serial] [Authorize(Roles = new[] { RoleNames.Moderator })] public async Task<Mechanic?> UpdateMechanic(                     ❶ [Service] ApplicationDbContext context, MechanicDTO model) { var mechanic = await context.Mechanics .Where(m => m.Id == model.Id) .FirstOrDefaultAsync(); if (mechanic != null) { if (!string.IsNullOrEmpty(model.Name)) mechanic.Name = model.Name; mechanic.LastModifiedDate = DateTime.Now;**

```csharp
                context.Mechanics.Update(mechanic);
                await context.SaveChangesAsync();
            }
            return mechanic;
        }

        [Serial]
        [Authorize(Roles = new[] { RoleNames.Administrator })]
        public async Task DeleteMechanic(                                ❷
            [Service] ApplicationDbContext context, int id)
        {
            var mechanic = await context.Mechanics
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
            if (mechanic != null)
            {
                context.Mechanics.Remove(mechanic);
                await context.SaveChangesAsync();
            }
        }
    }
}
```

❶ Updates a Mechanic
❷ Deletes a Mechanic

This implementation is similar to what we did in the BoardGamesController, DomainsControllers, and MechanicsController when we implemented the Update and Delete methods in chapter 5. But it’s important to understand one thing: the [Authorize] data attribute that we’re using to restrict the access of the GraphQL’s Update and Delete methods to moderators and administrators, respectively, is part of the HotChocolate.AspNetCore.Authorization namespace. Therefore, it differs from the one we used in our controllers (which are part of the Microsoft.AspNetCore.Authorization namespace).

NOTE The two attributes have the same name, but they accept a different Roles parameter type. The built-in Microsoft attribute wants a string, whereas the one provided by HotChocolate requires a string array. Otherwise, the attributes provide the same functionalities (that is, restricting a method to certain roles, claims, and/or policies, as we learned in chapter 9). Again, we used the [Serial] attribute to ensure that the Mutation tasks will be executed in serial mode, for the reasons I explained earlier. With this class, our GraphQL schema is ready. Now we need to add the required services and middleware to our HTTP pipeline by updating the Program.cs file.

Adding Services and Middleware Open the Program.cs file, and locate the part where we added the ApplicationDbContext as a service by using the builder.Services.AddDbContext method. We’re going to add the GraphQL services right after those code lines, as follows.

**Listing 10.6 Program.cs file: GraphQL services**

```csharp
using MyBGList.GraphQL;                 ❶

// [...]

builder.Services.AddGraphQLServer()     ❷
     .AddAuthorization()                ❸
     .AddQueryType<Query>()             ❹
     .AddMutationType<Mutation>()       ❺
     .AddProjections()                  ❻
     .AddFiltering()                    ❼
     .AddSorting();                     ❽
```

❶ Adds the root types namespace
❷ Registers the GraphQL server as a service
❸ Adds authorization support
❹ Binds the Query root type
❺ Binds the Mutation root type
❻ Adds projections functions
❼ Adds filtering functions
❽ Adds sorting capabilities

Let’s switch to middleware. Scroll down a bit, and add the following line of code right below the app.UseAuthorization() method:

app.MapGraphQL();

The middleware accepts various configuration settings. But the default behaviors are good enough for our scenario, including the path of the GraphQL endpoint, which is /graphql.

This section concludes our HotChocolate setup and configuration task. It’s time to see whether what we’ve done works and learn how to use GraphQL to interact with our data.

### 10.2.4 Working with GraphQL

Among the many good reasons why we picked HotChocolate is that it comes with BananaCakePop, a neat GraphQL client that we can use to interact with the GraphQL API we set up by using a convenient, web-based interface. To access the client’s main dashboard, launch the project in Debug mode, and point your browser to GraphQL’s default endpoint: https://localhost:40443/graphql. If everything goes well, we should see the page shown in figure 10.2.
Figure 10.2 BananaCakePop’s main dashboard

Click the Create Document button; confirm the default connection settings in the pop-up window that appears; and click the Apply button to open a new tab, which we’ll use to write our first GraphQL queries.

Query test Let’s start with a simple query that retrieves the top three board games, sorted by Id (in ascending order):

```csharp
query {
  boardGames(order: { id: ASC }, first:3 ) {
    nodes {
      id
      name
      year
    }
  }
}
```

Copy and paste the query into the Operations tab (left part of the screen), and click the Run button to send it to the server. If everything works as it should, we should see the following outcome in the Response tab (right part of the screen):

{ "data": { "boardGames": { "nodes": [ { "id": 1, "name": "Die Macher", "year": 1986 }, { "id": 2, "name": "Dragonmaster", "year": 1981 }, { "id": 3, "name": "Samurai", "year": 1998 } ] } } } It works! Notice that the response’s nodes array contains only the Id, Name, and Year properties for each board-game entry, matching the properties that we included in the query. This result demonstrates that we’re receiving only what we asked for, without any overfetching. Let’s perform another test with a more complex query:

```csharp
query {
  boardGames(order: { id: ASC }, first:3 ) {
    nodes {
      id
      name
      year
      boardGames_Domains {
         domain {
           name
         }
      }
      boardGames_Mechanics {
         mechanic {
           name
         }
      }
    }
  }
}
```

This time, we’re asking for not only some board-game properties, but also for some info about their domains and mechanics. In other words, we’re using the many-to-many relationships that we defined in chapter 4 to fetch data from three different entities (corresponding to three different database tables) within a single request.

Cut and paste the preceding query into BananaCakePop, and click Run to see the outcome. If everything goes as expected, we should see the domains and mechanics info for each board game, as shown in figure 10.3.
Figure 10.3 Requesting board games, domains, and mechanics together

Again, we can see that the response contains all the fields we asked for (and no more) for the three related entities. There’s no need for additional requests or round trips because we don’t have an underfetching problem to address.

Mutation test Let’s complete this testing round with a mutation. This task is a bit trickier than the preceding one, because the Mutation class methods are protected by the [Authorize] data attribute. To call those methods without getting a 401 - Unauthorized HTTP status code, we need to retrieve the bearer token that we set up in chapter 9 and use it within our request.

To keep things simple, we won’t implement the login functionalities in GraphQL. We’ll perform the login from the SwaggerUI, as we did in chapter 9, and retrieve the JSON Web Token (JWT) from there. For reasons of space, I won’t show how to do that here; feel free to check out chapter 9 for further guidance.

WARNING Be sure to perform the login with the TestModerator or TestAdministrator user, as we need an account with the Moderator role to perform our mutation tests.

After retrieving the token, go back to the main BananaCakePop dashboard, and click the plus (+) button near the top-right corner of the screen to open a new tab (A in figure 10.4). This time, before closing the Connection Settings pop-up window by clicking Apply, select the Authorization tab, select the Bearer type, and paste the JWT in the Token text box within the form, as shown in figure 10.4.
Figure 10.4 BananaCakePop Authorization settings

Next, click Apply, and cut/paste the following mutation into the Operations tab. This mutation is expected to change the Name of a BoardGame entity with an Id equal to 1 from "Die Macher" to "Die Macher (v2)" and return the updated name:

```csharp
mutation {
  updateBoardGame(model: {
      id:1
      name:"Die Macher (v2)"
     }) {
       name
    }
}
```

Click Run again to execute the code against the GraphQL server. If everything goes well, we should receive the following response:

```csharp
{
    "data": {
      "updateBoardGame": {
        "name": "Die Macher (v2)"
      }
    }
}
```

This response shows that the mutation worked well, because the returned name corresponds to the updated one.

WARNING Don’t forget that you have only 5 minutes to use the JWT from the moment you retrieve it by using the /Login endpoint, because we set its expiration time to 300 seconds in chapter 9. If you need more time, feel free to increase that value.

To further confirm that our update has been performed, open a third tab (no authentication required this time), and run the following query to retrieve the Name of the board game with an Id equal to 1:

```csharp
query {
  boardGames(where: { id: { eq: 1 } }) {
    nodes {
      name
    }
  }
}
```

This query will allow us to receive the updated name:

```csharp
{
    "data": {
      "boardGames": {
        "nodes": [
          {
            "name": "Die Macher (v2)",
          }
        ]
      }
    }
}
```

That’s it for GraphQL. Now let’s move on to gRPC.

## 10.3 Google Remote Procedure Call

gRPC is a language-agnostic, cross-platform, open source, high- performance Remote Procedure Call architecture designed by Google in 2015 to ensure high-speed communication among microservices. Its main architectural differences with REST are that gRPC uses the HTTP/2 protocol for transport instead of HTTP/1.1, and it uses protocol buffers (protobuf) as the data-exchange format instead of JSON.

### 10.3.1 gRPC pros

Most of the advantages of gRPC are related to performance:

Lightweight—The framework can run on almost any hardware (as long as HTTP/2 is supported). Low latency—The handshake optimizations granted by the HTTP/2 protocol ensures latency considerably lower than that of HTTP/1.1-based approaches such as REST and GraphQL. Low network use—The protobuf binary serialization ensures reduced network use (considerably lower than REST and GraphQL). Better communication model—Unlike REST and GraphQL, both of which are limited to the HTTP/1.1 request-response communication model, gRPC takes advantage of the bidirectional, streaming, multiplexing communication features granted by HTTP/2.

Thanks to these benefits, gRPC APIs are reportedly seven to ten times faster than REST APIs, according to a well-known test published by Ruwan Fernando in 2019. The full article, including test methodology and results, is available at http://mng.bz/1M8n.

Otherwise, gRPC features most of the relevant benefits of REST and GraphQL. It’s platform- and language-agnostic, has multiplatform support, has software development kits (SDKs) available for many programming languages and development frameworks, and has a well-established (and growing) developer community.

### 10.3.2 gRPC cons

Like REST and GraphQL, gRPC has some notable drawbacks, such as the following:

Complexity—Implementing a gRPC-based API is a lot more difficult and much slower than implementing an equivalent REST API. The complex use of the HTTP/2 protocol and the efforts required to implement the protobuf-based messaging system create a steep learning curve for most developers, even when they’re using third-party libraries that do most of the hard work. Compatibility—Most clients are unable to support gRPC out of the box, including browsers, because they don’t expose built-in APIs to control HTTP/2 requests with JavaScript. This problem has been mitigated by the introduction of gRPC-Web, an extension that makes gRPC compatible with HTTP/1.1, but this workaround still lacks a good level of support. (Most client-side frameworks don’t support it.) Readability—Because protobuf compresses gRPC messages before the transmission phase, humans can’t read the data exchanged by clients and servers without using dedicated tools. This fact is a huge downside compared with human-readable formats such as JSON and Extensible Markup Language (XML), as it makes it difficult to perform important development tasks such as writing requests manually, debugging, logging/analyzing the transferred data, and inspecting payloads. Caching—gRPC responses are hard to cache through intermediaries such as reverse proxies and/or content-delivery networks (CDNs). Furthermore, the gRPC specs don’t provide standards, best practices, and/or guidelines for caching. This problem could be less serious in most gRPC-friendly scenarios, such as real-time data streaming microservices, but it’s a nontrivial drawback for most web APIs, which is a major barrier to widespread adoption. Overfetching and underfetching—The gRPC architecture is based on requests accepting strongly typed input parameters and returning a predefined data structure, like REST. It doesn’t provide native support for a declarative query language, and it doesn’t have a runtime that projects the received input values to IQueryable objects or DBMS queries. As a result, it’s subject to the same overfetching and underfetching problems as most RESTful APIs, even if the streaming and bidirectional communication capabilities granted by HTTP/2 minimize the number of required round trips.

All in all, we can say that gRPC is a great alternative to REST for specific use cases that require lightweight services, great performance levels, real-time communication, and low-bandwidth usage. But the lack of browser support strongly limits its use, especially if our web API aims to provide data to external websites, services, and apps that may unable to handle it properly.

Next, we’ll see how to implement gRPC with ASP.NET Core. Unlike what we did with REST and GraphQL, this time we’ll have to implement both the server and the client.

### 10.3.3 Installing the NuGet packages

ASP.NET Core provides gRPC support through the Grpc.AspNetCore NuGet package, developed and maintained by Google. The package contains the following libraries:

Grpc.AspNetCore.Server—A gRPC server for .NET Grpc.Tools—The code-generation toolset Google.Protobuf—The protobuf serialization library

As always, the package can be installed by using the NuGet Package Manager GUI, the Package Manager console, or the dotnet CLI with the following command:

> dotnet add package Grpc.AspNetCore --version 2.50.0

### 10.3.4 Implementing the gRPC Server

Let’s start with the server. Here’s a list of the tasks that we’ll need to do:

Add a protobuf file. gRPC uses protocol buffer (protobuf) files to describe its interface definition language (IDL) and messaging structure. For that reason, we’re going to need to set one up. Update the project file. After adding the protobuf file, we’ll need to set it up within our project file so that the .NET compiler can use it autogenerate some base classes (stubs) for our gRPC server and client. Implement the gRPC service. We’re going to extend the autogenerated gRPC server base class to create our own service, which will interact with our data by using EF Core. Set up the project file. As soon as the gRPC server is ready, we’ll need to update our Program.cs file to add it to the HTTP pipeline, as well as register the main gRPC services in the service collection.

Adding the protobuf file The first thing to do is add a protobuf file, which will contain the message and service descriptions for our gRPC implementation. Start by creating a new /gRPC/ folder in our project’s root folder, which will contain our protobuf file and our gRPC server class. Then right-click the folder, and add a new protocol buffer file, calling it grpc.proto (lowercase is recommended). You can use the "proto" query to search for it among the available templates, as shown in figure 10.5.

Figure 10.5 Add a new protocol buffer file with Visual Studio.

Next, we can use the new grpc.proto file to describe the interface that our gRPC server and client will use to exchange data. The syntax to use is called "proto3" (protobuf version 3) and is different from C# yet conceptually similar.
TIP For reasons of space, I won’t explain the "proto3" syntax in detail. For a reference guide, check out http://mng.bz/Pxjv.

Listing 10.7 shows how we can use the "proto3" syntax to define a single GetBoardGame method accepting a BoardGameRequest input type and returning a BoardGameResponse output type.

**Listing 10.7 /gRPC/grpc.proto file**

```csharp
syntax = "proto3";

option csharp_namespace = "MyBGList.gRPC";

package Grpc;

service Grpc {
    rpc GetBoardGame (BoardGameRequest) returns (BoardGameResponse);   ❶
}

message BoardGameRequest {                                             ❷
  int32 id = 1;
}

message BoardGameResponse {                                            ❸
  int32 id = 1;
  string name = 2;
  int32 year = 3;
}
```

❶ Data-retrieval method
❷ Input type definition
❸ Output type definition

Notice that the BoardGameRequest and BoardGameResponse types closely resemble the C# DTO classes that we’ve used since chapter 5 to process our requests. They’re DTOs as well.
NOTE The numbers placed after the equal sign of each type’s property aren’t their default values (as would be the case C#), but their index. In the case of the BoardGameResponse type, the id property has an index of 1, name has 2, and year has 3. The protobuf serialization library will use these index values to serialize and compress the actual values.

Setting up the project file Now that we have our protobuf file, we need to set it up in our MyBGList project file so that the .NET compiler will locate and compile it. First, from Solution Explorer, right-click the project’s root node, and choose Edit Project File from the contextual menu to open the MyBGList.csproj file. Next, add the following reference near the end of the file, before the </Project> closing tag:

<ItemGroup> <Protobuf Include="gRPC/grpc.proto" /> </ItemGroup>

This reference tells the .NET compiler to code-generate the client and server stubs.

TIP For additional info on the protobuf/gRPC code-generation features and their integration into the .NET building phase, see http://mng.bz/JlBa.

Now that we’ve laid out the basis, we can implement the service that our gRPC server will use to accept and process the incoming request. Implementing the GrpcService Create a new /gRPC/GrpcService.cs file, which will host our gRPC service implementation. This time, we’ll be able to use C# syntax. (Yay!) We’re going to create the single GetBoardGame data-retrieval method that we defined in the grpc.proto file, with its input and output types, to allow our clients to fetch some sample board-game data. The following listing shows how.

**Listing 10.8 /gRPC/GrpcService.cs file**

```csharp
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using MyBGList.Models;

namespace MyBGList.gRPC
{
   public class GrpcService : Grpc.GrpcBase                           ❶
   {
        private readonly ApplicationDbContext _context;               ❷

        public GrpcService(ApplicationDbContext context)
        {
            _context = context;                                       ❷
        }

        public override async Task<BoardGameResponse> GetBoardGame(   ❸
            BoardGameRequest request,
            ServerCallContext scc)
        {
            var bg = await _context.BoardGames                        ❹
                .Where(bg => bg.Id == request.Id)
                .FirstOrDefaultAsync();
            var response = new BoardGameResponse();
            if (bg != null)
            {
                response.Id = bg.Id;
                response.Name = bg.Name;
                response.Year = bg.Year;
            }
            return response;                                          ❺
        }
    }
}
```

❶ Extends the codegen service base class
❷ Injects the ApplicationDbContext
❸ Implements the GetBoardGame method
❹ Retrieves board-game data by using EF Core
❺ Returns the BoardGameResponse type

This code should be easy to understand. Notice that it looks like one of our controllers, which isn’t a surprise because it’s meant to do roughly the same jobs under the hood: accepting a request, retrieving the data, and returning a response. Now that we have our service, we can map it to our Program.cs file and register the gRPC main services to make our server accept the incoming calls.

Setting up the Program file Let’s start by registering the gRPC main services in the service collection. Open the Program.cs file, locate the part where we added the GraphQL services (AddGraphQLServer), and add the following line right below it to enable gRPC:

builder.Services.AddGrpc();

Next, scroll down to the part where we added the GraphQL endpoint mapping (MapGraphQL), and add the following line below it to map our new service in the ASP.NET routing pipeline:

app.MapGrpcService<GrpcService>(); Our gRPC server-side implementation is complete. Now we need a sample client to test it.

### 10.3.5 Implementing the gRPC client

Implementing a gRPC client with ASP.NET Core typically requires creating a new project with the same NuGet packages and a new protobuf file similar to the one we defined for our server (as client and server protobuf files need to share interfaces). This approach would decouple the client and server’s source code, ensuring that each component will fulfill its role while relying on its own separate, independent codebase. But it would also require more time and coding effort.

To keep things simple, we’ll create a new GrpcController that will act as a client. This approach will allow us to reuse the existing /gRPC/grpc.proto file without having to copy it or move it to a separate project. At the end of the day, we’ll have a gRPC server and client in the same ASP.NET Core project, which is perfectly fine for testing purposes.

Adding the GrpcController Create a new /Controllers/GrpcController.cs file to contain our gRPC client implementation. We need to define a single action method that will instantiate the gRPC client, perform the request to the gRPC service endpoint, and return the results. In other words, the action method will act as a wrapper for the actual gRPC client. The added value of this approach is that we’ll be able to deal with readable input and output values, as the serialization and compression tasks will happen within the action method. The following listing provides the source code.

**Listing 10.9 /Controllers/GrpcController.cs file**

```csharp
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using MyBGList.gRPC;

namespace MyBGList.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class GrpcController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<BoardGameResponse> GetBoardGame(int id)
        {
            using var channel = GrpcChannel
                  .ForAddress("https://localhost:40443");            ❶
              var client = new gRPC.Grpc.GrpcClient(channel);        ❷
              var response = await client.GetBoardGameAsync(         ❸
                                new BoardGameRequest { Id = id });
              return response;                                       ❹
         }
     }
}
```

❶ Sets up the gRPC channel
❷ Instantiates the gRPC client
❸ Performs the client-to-server call
❹ Returns the received response

As we can see, the controller’s implementation is thin. We don’t have services to inject—not even our ApplicationDbContext. But we’re supposed to retrieve our data from the gRPC server instead of using EF Core. Now that we have our client, we can test the gRPC data-retrieval flow. Testing it Launch the project in Debug mode, and access the SwaggerUI dashboard. Locate the /Grpc/GetBoardGame/{id} endpoint, handled by the GrpcController’s GetBoardGame action method that wraps our gRPC client. Expand the endpoint’s panel, click the Try It Out button, enter 1 in the Id text box, and click Execute to perform the request (figure 10.6).

Figure 10.6 Executing the /Grpc/GetBoardGame endpoint that wraps the gRPC client

Right after executing this code, check the Server Response panel below to see the resulting JSON output. Notice that we’re receiving the board game id, name, and year data in the endpoint’s response body: { "id": 1, "name": "Die Macher (v2)", "year": 1996, }

Those values were fetched by the underlying gRPC client through a connection established with the gRPC server, so our sample gRPC implementation is working properly.

### 10.3.6 Adding Authorization support

Before moving on to the next topic, let’s see how our sample gRPC implementation handles the ASP.NET Core authorization framework. The gRPC services fully support the [Authorize] attribute we learned to use in chapter 9, so we already know what to do. In the following section, we’ll use our acquired knowledge to perform the following tasks:

1. Define a new gRPC endpoint that will allow our clients to update board-game data by using gRPC.
2. Implement the new endpoint in our existing GrpcService, using the [Authorize] attribute to limit its use to users who have the Moderator role.
3. Update the GrpcController by adding a new action method that will act as a wrapper for the client that calls the new gRPC endpoint.

Defining the new endpoint Open the /gRPC/grpc.proto file, and add the new UpdateBoardGame gRPC endpoint in the following way (new lines in bold):

// ... existing code

```csharp
service Grpc {
  rpc GetBoardGame (BoardGameRequest) returns (BoardGameResponse);
  rpc UpdateBoardGame (UpdateBoardGameRequest) returns (BoardGameResponse);
}
```

// ... existing code

The new method accepts a new input type, which we also need to define within the protobuf file. We can do that below the BoardGameRequest definition (new lines in bold):

// ... existing code

```csharp
message BoardGameRequest {
  int32 id = 1;
}
```

```csharp
message UpdateBoardGameRequest {
  int32 id = 1;
  string name = 2;
}
```

// ... existing code

Now that we’ve defined the interface, we can implement it in our GrpcService.

Implementing the new endpoint Open the /gRPC/GrpcService.cs file, and add the method in listing 10.10 right below the existing one. The code features the same data-retrieval and update logic that we’ve used several times.
**Listing 10.10 /gRPC/GrpcService.cs file: UpdateBoardGame method**

```csharp
using Microsoft.AspNetCore.Authorization;   ❶
using MyBGList.Constants;                   ❶

// ... existing code

[Authorize(Roles = RoleNames.Moderator)]     ❷
public override async Task<BoardGameResponse> UpdateBoardGame(
    UpdateBoardGameRequest request,
    ServerCallContext scc)
{
    var bg = await _context.BoardGames
        .Where(bg => bg.Id == request.Id)
        .FirstOrDefaultAsync();
    var response = new BoardGameResponse();
    if (bg != null)
    {
        bg.Name = request.Name;
        _context.BoardGames.Update(bg);
        await _context.SaveChangesAsync();
        response.Id = bg.Id;
        response.Name = bg.Name;
        response.Year = bg.Year;
    }
    return response;
}
```

❶ Required namespaces
❷ Role-based access control

To keep things simple, our sample method will update only the board game’s Name data. But any additional field can be added to the interface without much effort. Now that the server-side part is ready, we can move to the controller that wraps our gRPC client.

Updating the GrpcController The most convenient thing to do is add another wrapper that will instantiate a dedicated gRPC client for the new method. Open the /Controllers/GrpcController.cs file, and add another action method (as in the following listing) right below the existing one (relevant lines in bold).

**Listing 10.11 /Controllers/GrpcController.cs file: UpdateBoardGame method**

```csharp
using Grpc.Core;                                        ❶

// ... existing code

[HttpPost]
public async Task<BoardGameResponse> UpdateBoardGame(
    string token,                                       ❷
    int id,
    string name)
{
    var headers = new Metadata();                       ❸
    headers.Add("Authorization", $"Bearer {token}");    ❹

    using var channel = GrpcChannel
        .ForAddress("https://localhost:40443");
    var client = new gRPC.Grpc.GrpcClient(channel);
    var response = await client.UpdateBoardGameAsync(
                        new UpdateBoardGameRequest {
                            Id = id,
                            Name = name
                        },
                         headers);                      ❺
    return response;
}
```

❶ Required namespace
❷ Accepts the bearer token as input value
❸ Creates a headers metadata object
❹ Adds the Authorization header with the token
❺ Appends the headers metadata to the request

The new action method accepts the bearer token among input values, along with the id of the board game we want to update and the new name that we want to assign to it. The implementation is straightforward and should pose no problems.

Testing the new endpoint To test the new gRPC endpoint, we need to retrieve a bearer token, which we can do by using our existing REST /Account/Login endpoint.

WARNING Be sure to perform the login with the TestModerator or TestAdministrator user, as we need an account with the Moderator role to perform our mutation tests.

As soon as we have the token, we can call the /UpdateBoardGame endpoint from the SwaggerUI, filling in all the relevant form fields (figure 10.7).
Figure 10.7 Calling /UpdateBoardGame from the SwaggerUI

If everything is working as expected, we should receive the following response body:

{ "id": 1, "name": "Die Macher (v3)", "year": 1986 }

This response means that the name change has been applied. This task concludes our gRPC implementation sample.
TIP For more information on the ASP.NET Core gRPC integration, see https://docs.microsoft.com/en-us/aspnet/core/grpc.

## 10.4 Other REST alternatives

Facebook’s GraphQL and gRPC aren’t the only modern alternatives to REST. Several other promising data exchange technologies, architectures, and standards are gaining more attention from the developer community. The following sections briefly mention the most notable ones, as well as documenting current .NET Core support for each one.

### 10.4.1 Newline Delimited JSON (NDJSON)

NDJSON is a simple yet effective solution for storing, transferring, and streaming multiple JSON objects delimited by a newline separator (\n) or a return plus a newline separator pair (\r\n). The technique takes advantage of the fact that the JSON format doesn’t allow newline characters within primitive values and doesn’t require them elsewhere. (Most JSON formatters suppress them by default, because they have no purpose other than making the formatted data easier for humans to read.) ASP.NET support is granted by some third-party NuGet packages, such as Ndjson.AsyncStreams by Tomasz Pe˛czek. The package can be downloaded at https://github.com/tpeczek/Ndjson.AsyncStreams.

### 10.4.2 Falcor

Falcor is a JSON-based data platform developed by Netflix and open- sourced in 2015 (under the Apache 2.0 license). Falcor operates as a layer between the client and the server, providing a single endpoint that clients can use to send JSON-formatted queries and receive exactly the data they need. The overall concept is similar to GraphQL, but instead of having a schema and static types, Falcor uses a single huge virtual JSON object that represents the data model and a specialized router that fetches the values requested by clients to one or more backend services.

Like GraphQL, Falcor prevents clients from problems such as overfetching, underfetching, and multiple round trips to gather the required data. ASP.NET support has been initially granted by the Falcor.NET NuGet Package (https://github.com/falcordotnet/falcor.net), developed by Craig Smitham for the .NET Framework 4.x around 2015. But the implementation is still in developer-preview status, was never ported to ASP.NET Core, and hasn’t received updates for many years. For additional information on Falcor, check out the project’s official page on GitHub at https://netflix.github.io/falcor.

### 10.4.3 Thrift

Apache Thrift is an IDL and binary communication protocol developed by Facebook and released in 2020 as an open source project in the Apache Software Foundation. Like gRPC, it’s based on a proprietary descriptor file that contains the interfaces for services and DTOs. The compiler uses the description file to generate the source code for the destination language (such as C#), obtaining the codegen stubs that can be extended to implement the server and client. Thrift can operate over HTTP/2, HTTP 1.1/1.0, and WebSocket, and it supports several data transmission protocols, including binary and JSON.

ASP.NET Core support is granted through the Apache Thrift IDL compiler (available for Windows and Linux) and the Thrift C# library, which can be compiled directly from the Thrift source code or obtained in a precompiled version through several third-party NuGet packages, such as ApacheThrift, apache-thrift- netcore, thrift-csharp, Apache.Thrift, and Tnidea.Thrift.

For additional information about Thrift, check out the project’s official page at https://thrift.apache.org. For a comprehensive coverage of the Apache Thrift framework, see Randy Abernethy’s Programmer’s Guide to Apache Thrift at http://mng.bz/wPOa.

## 10.5 Exercises

The following exercises emulate some task assignments given by our product owner and addressed to the MyBGList development team —in other words, to us.

NOTE The solutions to the exercises are available on GitHub in the /Chapter_10/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 10.5.1 Write a new GraphQL query

Write a new GraphQL query that fetches the following data from the GraphQL endpoint (and only that data), respecting all the following requirements:

Retrieve only the board games with a Name starting with "War". Order the results by their names, in ascending order. Get only the top ten results. For each board game, retrieve the following fields: Id, Name, Year, MinPlayers, MaxPlayers, and PlayTime. For each board game, also retrieve the Id and Name of each related domain and mechanic.

TIP Feel free to check the GraphQL Query and Mutation syntax official reference at https://graphql.org/learn/queries.

NOTE Be sure to test the query by using the BananaCakePop web client to ensure that it works.

### 10.5.2 Fetch GraphQL data for a mutation

Write a new GraphQL query that fetches the Id of the board game with a Name equal to "Axis & Allies" and "Year" equal to
2004. (One game should match both conditions.) After retrieving the Id, write a new GraphQL mutation to change its name to "Axis & Allies: Revised".

NOTE Again, feel free to check the GraphQL Query and Mutation syntax official reference (see section 10.5.1). Be sure to execute the mutation with the BananaCakePop web client (retrieving and using a suitable bearer token ) to ensure that the changes will be saved in the DBMS. Then roll back all the changes by setting the board game’s Name back to its previous value.

### 10.5.3 Implement new gRPC server features

Improve the gRPC protobuf file to support the following methods: GetDomain, UpdateDomain, GetMechanic, and UpdateMechanic. For each method, define the required input and output type, following the same pattern we used to implement the BoardGameRequest, UpdateBoardGameRequest, and BoardGameResponse types. Be sure to use the [Authorize] attribute to make the UpdateDomain and UpdateMechanic methods accessible only to users with the Moderator role.

### 10.5.4 Add new gRPC client wrappers

Improve the GrpcController file by adding the following action methods: GetDomain, UpdateDomain, GetMechanic, and UpdateMechanic. Each action method must act like a wrapper for a gRPC client performing the call to the gRPC server, following the same pattern we used to implement the GetBoardGame and UpdateBoardGame action methods. Again, use the [Authorize] attribute to limit access to the update methods to the Moderator role.

### 10.5.5 Test the new gRPC features

Use the SwaggerUI to execute the new gRPC wrappers we created in section 10.5.4 to ensure that they work. At the same time, ensure that all the update methods are accessible only to users with the Moderator role.

Summary The REST architectural style has been the most popular web API architecture for years, thanks mostly to its scalability, flexibility, and portability, which led to worldwide success and enthusiastic adoption among the developer community. But it’s not the only choice for implementing a web API or even the best alternative in every circumstance. REST has a lot of benefits, yet it also has undeniable drawbacks such as overfetching and underfetching. These drawbacks can easily lead to nontrivial performance problems, forcing developers to define many additional endpoints and forcing clients to perform multiple round trips to fetch the data they need. GraphQL is a query language for APIs created by Facebook as a workaround for some known REST limitations. Its success is due mostly to its great flexibility; it allows clients to receive highly personalized data sets by using a single declarative query to a single endpoint, avoiding the risk of underfetching and overfetching without the need for multiple round trips and/or many endpoints. It’s well supported by ASP.NET Core thanks to several open source NuGet packages, such as HotChocolate. gRPC is a high-performance RPC architecture designed to ensure high-speed communication among microservices. Unlike REST and GraphQL, it uses the HTTP/2 protocol instead of HTTP/1.1, and protocol buffers instead of JSON. Despite being considerably faster than REST and GraphQL, it has some drawbacks (such as complexity, poor readability, and lack of some relevant features) that limit its adoption to some specific use cases. gRPC can be used in any ASP.NET Core app thanks to the Grpc.AspNetCore NuGet package. GraphQL and gRPC aren’t the only alternatives to REST. Several other promising data exchange solutions are available: Newline Delimited JSON (NDJSON), a simple yet effective solution for storing, transferring, and streaming multiple JSON objects delimited by newlines Falcor, a JSON-based data platform developed by Netflix that offers features similar to GraphQL but has a different underlying architecture Thrift, an interface definition language and binary communication protocol developed by Facebook that works like gRPC and has a similar architecture
