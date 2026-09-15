---
title: GraphQL Introduction
source: GrapQL Introduction.pdf
course_week: 11-12
topic: GraphQL, gRPC og remote APIs
---

# GraphQL Introduction

## REST drawbacks

- **Over fetching**
  - You get more data than you need
  - When you make a call to a REST endpoint you get all the properties of the item, even if you just need the title and id
- **Under fetching**
  - You don't get all the data you need
  - Often you need data from different tables to appear on the same page, which typically requires several calls to the back-end

## GraphQL's origins & why use it

- GraphQL comes from Facebook
- Using a traditional REST API structure, the newsfeed was making many calls to multiple API endpoints in order to get all the data it needed
- Along the way, the API calls were also over-fetching extra data that the newsfeed didn't need
- Additionally, upon receipt, the frontend engineers still had to parse through the data to find the fields they wanted
- Facebook engineers wondered: "What if we could write a query language so that we can specify all the information we need in a single API request?"
- GraphQL is the result of that effort
- GraphQL maps the relationships between objects in your database, creating a graph; then a query language was designed for traversing that map of relationships — hence the name GraphQL
- Facebook's mobile apps have been powered by GraphQL since 2012

## GraphQL is a specification, not an implementation

- Facebook open-sourced GraphQL as a specification
- It can be implemented in any programming language — as long as the implementation parses queries, schema, etc. in the specified way, it will play nice with any other GraphQL application
- Publicly released in 2015
- November 2018: the GraphQL project moved from Facebook to the newly established GraphQL Foundation, hosted by the non-profit Linux Foundation

## Overview

- GraphQL is not a data storage service; it's an interface to one or many (a GraphQL layer sits between clients and data sources)

## Graph data structure

- A graph is a non-linear data structure consisting of nodes and edges
  - The nodes are sometimes also referred to as vertices
- The edges are lines or arcs that connect any two nodes in the graph

## Advantages

- The unique thing about GraphQL is that the documentation (the schema) is part of how you create the API service
  - You cannot have out-of-date documentation
- GraphQL decouples clients from servers and allows both to evolve and scale independently
  - This enables faster iteration in both frontend and backend products
- Efficiency
  - The client asks the GraphQL service a single complex question and gets a single response with precisely what the client needs

## The Cons

- **Complexity problems**
  - Building a GraphQL API can be more intensive than building a REST API
  - The flexibility and richness of the query language adds complexity that may not be worthwhile for simple APIs
  - The benefits in speed and usability may make up for it in complex or high-performance applications — but only if you have a complex API and clients with different needs
- **Caching problems**
  - GraphQL comes with no native caching support, so you need to implement your own standard or rely on third-party libraries
  - Implementing an efficient cache mechanism won't be easy, because all requests are addressed to a single endpoint that's supposed to return highly customized (hence, often different) data sets

## The GraphQL language

Query:

```graphql
{
  flight(id: "1234") {
    origin
    destination
  }
}
```

Response:

```json
{
  "data": {
    "flight": {
      "origin": "DFW",
      "destination": "MKE"
    }
  }
}
```

With nested objects:

```graphql
{
  flight(id: "1234") {
    origin
    destination
    passengers {
      name
    }
  }
}
```

```json
{
  "data": {
    "flight": {
      "origin": "DFW",
      "destination": "MKE",
      "passengers": [
        { "name": "Luke Skywalker" },
        { "name": "Han Solo" },
        { "name": "R2-D2" }
      ]
    }
  }
}
```

Because GraphQL interprets data as a graph, we can traverse it the other direction, too:

```graphql
{
  person(name: "Luke Skywalker") {
    passport_number
    flights {
      id
      date
      origin
      destination
    }
  }
}
```

```json
{
  "data": {
    "person": {
      "passport_number": 78120935,
      "flights": [
        { "id": "1234", "date": "2019-05-24", "origin": "DFW", "destination": "MKE" },
        { "id": "2621", "date": "2019-07-05", "origin": "MKE", "destination": "DFW" }
      ]
    }
  }
}
```

## To create a GraphQL API

1. Pick a framework to implement your GraphQL server — you can use ASP.NET Core
2. Define a schema so GraphQL knows how to route incoming queries
3. Construct an endpoint:
   1. Expose a GraphQL API endpoint that clients will use to send their queries
   2. Process the incoming requests (with GraphQL queries) against your data model
   3. Retrieve the requested data from the underlying DBMS
   4. Provide a suitable HTTP response with the resulting data
4. Write a client-side query that fetches data

## Install GraphQL

For ASP.NET Core you have two main third-party alternatives:

- **GraphQL.NET** — https://github.com/graphql-dotnet/graphql-dotnet
- **HotChocolate** — https://github.com/ChilliCream/hotchocolate
  - Provides a couple of convenient features:
    - A built-in GraphQL IDE called Banana Cake Pop, used to test the API endpoint without installing a dedicated client (like SwaggerUI for REST)
    - Native EF Core support

## Hot Chocolate Server

- Hot Chocolate is an open-source GraphQL server for the Microsoft .NET platform, compliant with the newest GraphQL October 2021 spec + drafts — compatible with all GraphQL-compliant clients like Strawberry Shake, Relay, Apollo Client and other GraphQL clients and tools
- You can use Hot Chocolate Server as:
  - Stand-alone ASP.NET Core GraphQL server
  - Serverless Azure Function or AWS Lambda that serves up a GraphQL server
  - GraphQL Gateway for a federated data graph that pulls all your data sources together to create the one source of truth

## Installing HotChocolate

Install the HotChocolate NuGet packages — all `HotChocolate.*` packages need to have the same version:

```bash
dotnet add package HotChocolate.AspNetCore
dotnet add package HotChocolate.AspNetCore.Authorization
dotnet add package HotChocolate.Data.EntityFramework
```

If the Implicit Usings feature is enabled (the default for most VS2022 C# templates), the RequestDTO object could raise compiler errors due to an ambiguous reference to the `[DefaultValue]` attribute. To fix, replace:

```csharp
using System.ComponentModel;
```

with:

```csharp
using DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute;
```

## GraphQL schema basics

- Your GraphQL server uses a schema to describe the shape of your data graph
- The schema defines a hierarchy of types with fields that are populated from your back-end data stores
- The schema specifies exactly which queries and mutations are available for clients to execute against your data graph
- The GraphQL specification includes a human-readable schema definition language (SDL) that you use to define your schema and store it as a string
- The schema is not responsible for defining where data comes from or how it's stored

## Supported types

- Scalar types
- Object types
- The Query type
- The Mutation type
- Input types
- Enum types

## API requirements

A way to start thinking about a GraphQL schema is to look at it from the point of view of the UIs you'll be building — what data operations will they require?

## Setting up the GraphQL schema

The GraphQL schema defines how we want to expose data to our client and the CRUD operations we want to allow. It is typically composed of one or more root operation types:

- **Query** — exposes all the possible data-retrieval queries we want to make available to clients
- **Mutation** — allows clients to perform Insert, Update, and/or Delete operations (optional)
- **Subscription** — enables a real-time messaging mechanism that clients can use to subscribe to various events and be notified of their occurrence (optional)

## Adding the query type

- Create a new `/GraphQL/` top-level folder in the project
- Add a new `Query.cs` file to this folder
- Create a public method for each entity in the data model that clients should be able to access via GraphQL
  - Mapping it to its corresponding DbSet
  - Each method is mapped to an EF Core DbSet, returning it as an `IQueryable` object that the GraphQL runtime uses under the hood to retrieve the records from the DBMS
  - Each method has data annotation attributes from HotChocolate that enable powerful built-in features without implementing them explicitly

## Query implementation

```csharp
using MyBGList.Data;
using MyBGList.Models;

namespace MyBGList_Chap10.GraphQL
{
    public class Query
    {
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<BoardGame> GetBoardGames(
            [Service] ApplicationDbContext context)
            => context.BoardGames;
    }
}
```

## The purpose of each attribute

- `[Serial]` — configures the GraphQL runtime to execute certain tasks in serial mode rather than parallel mode, as required by the current `ApplicationDbContext` implementation. Likely has some performance effects, but acceptable because it allows reusing the source code without refactoring
- `[UsePaging]` — adds pagination middleware allowing the GraphQL runtime to paginate results using the Cursor Connections Specification, a standardized pagination approach adopted by GraphQL specs
- `[UseProjection]` — adds middleware that projects the incoming GraphQL queries to database queries through `IQueryable` objects
- `[UseFiltering]` — adds filtering middleware that lets requesting clients use filters, translated into LINQ queries and then DBMS queries by the GraphQL runtime. The available filters are inferred automatically from the `IQueryable` entity types (like SwaggerUI does with REST endpoints)
- `[UseSorting]` — adds sorting middleware that lets requesting clients sort results using a sorting argument, translated into LINQ queries and then DBMS queries under the hood

## Why we're using the [Serial] attribute

- When we use `AddDbContext<T>` to register `ApplicationDbContext` as a scoped service, a single instance of the class is created and used for the entirety of any given HTTP request
- Without `[Serial]`, the GraphQL runtime will likely perform multiple sets of queries and combine the resulting data into a single aggregate response
- These queries are, per the runtime's default behavior, executed in parallel for performance reasons — making our DbContext crash
- The preferred route for production-level GraphQL APIs is to replace the DbContext registration with `AddDbContextFactory`, registering a factory instead of a single scoped instance

## Adding the mutation type

- Add a new `Mutation.cs` file in the `/GraphQL/` folder
- Create a public method for the Update and Delete tasks of the BoardGames, Domains, and Mechanics entities
- Use the DTO objects instead of the raw entity classes — which forces us to map the properties we want to update manually
- The `[Authorize]` attribute restricting access of the GraphQL Update and Delete methods to moderators and administrators is part of the `HotChocolate.Authorization` namespace
  - It differs from the one used in controllers, which is part of `Microsoft.AspNetCore.Authorization`
  - The two attributes have the same name but accept a different `Roles` parameter type; otherwise they provide the same functionality

## DeleteBoardGame mutation

```csharp
using MyBGList.Data;
using MyBGList.DTO;
using MyBGList.Models;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBGList.Constants;

namespace MyBGList.GraphQL
{
    public class Mutation
    {
        [Serial]
        [Authorize(Roles = new[] { RoleNames.Administrator })]
        public async Task DeleteBoardGame(
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
    }
}
```

## UpdateBoardGame mutation

```csharp
[Serial]
[Authorize(Roles = new[] { RoleNames.Moderator })]
public async Task<BoardGame?> UpdateBoardGame(
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
```

## Add services

```csharp
builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();
```

## Add middleware

Add the GraphQL middleware after `UseAuthorization`:

```csharp
app.UseAuthorization();

app.MapGraphQL();
```

- The middleware accepts various configuration settings, but the defaults are good enough for this scenario — including the path of the GraphQL endpoint, which is `/graphql`

## Try it — Banana Cake Pop

- HotChocolate comes with Banana Cake Pop, a GraphQL client to interact with the GraphQL API via a convenient web-based interface
- Launch the project in Debug mode and point the browser to GraphQL's default endpoint: `https://localhost:<portno>/graphql` (e.g. `https://localhost:7233/graphql/`)

## Query test

Click "Create Document", paste the query into the Operations tab (left) and click Run:

```graphql
query {
  boardGames(order: { id: ASC }, first: 3) {
    nodes {
      id
      name
      year
    }
  }
}
```

Result appears in the Response tab (right):

```json
{
  "data": {
    "boardGames": {
      "nodes": [
        { "id": 1, "name": "Die Macher", "year": 1986 }
      ]
    }
  }
}
```

## Complex query

```graphql
query {
  boardGames(order: { id: ASC }, first: 3) {
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

Response (excerpt):

```json
{
  "data": {
    "boardGames": {
      "nodes": [
        {
          "id": 1,
          "name": "Die Macher",
          "year": 1986,
          "boardGames_Domains": [
            { "domain": { "name": "Strategy Games" } }
          ],
          "boardGames_Mechanics": [
            { "mechanic": { "name": "Hand Management" } },
            { "mechanic": { "name": "Simultaneous Action Selection" } }
          ]
        }
      ]
    }
  }
}
```

## Mutation test

- To call mutation methods without getting a 401 Unauthorized HTTP status code, retrieve the bearer token and use it in the request
- Perform the login from SwaggerUI and retrieve the JSON Web Token (JWT) from there
  - You can easily implement login in GraphQL if you need to
- In the Banana Cake Pop dashboard, click connection settings (top right), select the Authorization tab, select the Bearer type and paste the JWT in the Token text box

## Mutation example

```graphql
mutation {
  updateBoardGame(model: { id: 1, name: "Die Macher (v2)" }) {
    name
  }
}
```

Or test it with Postman.

## Hot Chocolate v15

Hot Chocolate provides a set of templates to quickly get started:

```bash
dotnet new install HotChocolate.Templates
```

Create a new Hot Chocolate GraphQL server project:

```bash
dotnet new graphql --name GettingStarted
```

Run the scaffolded project:

```bash
dotnet run --no-hot-reload
```

## Links & References

- https://graphql.org/
- GraphQL intro: https://graphql.github.io/learn/
- ChilliCream GraphQL Platform: https://github.com/ChilliCream/graphql-platform
- Popular server-side frameworks, client libraries, services and tools: https://graphql.org/community/tools-and-libraries/
- The Apollo Data Graph Platform: https://www.apollographql.com/
