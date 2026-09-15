---
title: Exploring GraphQL APIs
source: Exploring GrahpQL APIs.pdf
course_week: 11-12
topic: GraphQL, gRPC og remote APIs
---

# Exploring GraphQL APIs

## Agenda

- Using GraphQL's in-browser IDE to test GraphQL requests
- Exploring the fundamentals of sending GraphQL data requests
- Exploring read and write example operations from the GitHub GraphQL API
- Exploring GraphQL's introspective features

## The GraphiQL editor

- GraphiQL (with an *i* before the QL, pronounced "graphical") is an open source web application (written with React.js and GraphQL) that can be run in a browser
- Try it on the Star Wars service: https://graphql.org/swapi-graphql/ or https://swapi-graphql.eskerda.vercel.app/
- Press Ctrl-Space to get an autocompletion list

## The structure of a GraphQL request

A request consists of:

- **Document**
  - Queries
  - Mutations
  - Subscriptions
  - Fragments
- **Variables**
- **Meta-information**

If the request document contains more than one operation, a GraphQL request must include information about which operation to execute.

## Example GraphQL request

Document:

```graphql
query GetEmployees($active: Boolean!) {
  allEmployees(active: $active) {
    ...employeeInfo
  }
}

query FindEmployee {
  employee(id: $employeeId) {
    ...employeeInfo
  }
}

fragment employeeInfo on Employee {
  name
  email
  startDate
}
```

Variables:

```json
{
  "active": true,
  "employeeId": 42
}
```

Meta-information:

```
operationName="GetEmployees"
```

## Three types of operations

- **Query operations** — a read-only fetch
- **Mutation operations** — a write followed by a fetch
- **Subscription operations** — a request for real-time data updates

## Example GraphQL mutation

```graphql
mutation RateStory {
  addRating(storyId: 123, rating: 5) {
    story {
      averageRating
    }
  }
}
```

Adds a new five-star rating record for a story and then retrieves the new average rating of that same story. Note this is a write followed by a read — all GraphQL mutation operations follow this concept.

## Example GraphQL subscription

```graphql
subscription StoriesRating {
  allStories {
    id
    averageRating
  }
}
```

Instructs the GraphQL server to open a socket connection with the client, send story IDs along with their average ratings, and keep doing that when the information changes on the server.

## Fields

- A field always appears within a pair of curly brackets — a *selection set*
- A field can describe:
  - A scalar value
  - An object (contains another selection set to customize the information needed)
  - A list of objects
- Scalar types: `Int`, `String`, `Float`, `Boolean`, `ID`
- Scalar types are leaf values

Example:

```graphql
{
  lukeSkywalker {
    email
    birthday {
      month
      year
    }
    friends {
      name
    }
  }
}
```

## Root fields

- *Root field* refers to the first-level fields in a GraphQL operation
- The root fields in an operation usually represent information that is globally accessible to your application and its current user
- Root fields are also often used to access certain types of data referenced by a unique identifier:

```graphql
{
  user(id: 42) {
    fullName
  }
}
```

- References to a currently logged-in user are often named `viewer` or `me`:

```graphql
{
  me {
    username
    fullName
  }
}
```

## Customizing fields with arguments

- Fields in a GraphQL operation are similar to functions — they map input to output
- Just like functions, we can pass any GraphQL field a list of argument values

## Identifying a single record to return

For an API field representing a single record, the argument value you pass to identify that record must be a unique value:

```graphql
query UserInfo {
  user(email: "jane@doe.name") {   # user = root field, email = field argument
    firstName
    lastName
    username
  }
}
```

## Node interface

- With a Node interface, you can look up any node in the data graph by its unique global system-wide ID
- Then, based on what that node is, you can use an inline fragment to specify the properties on that node that you are interested in seeing in the response

## Introspective queries

- Introspective queries can be used to answer questions about the API schema
- Introspective queries start with a root field that's either `__type` or `__schema` — aka *meta-fields*
- Fields with names that begin with double underscore characters are reserved for introspection support

Example:

```graphql
{
  __schema {
    types {
      name
      description
    }
  }
}
```

## References & Links

- GraphQL in Action (book)
- Trying the Star Wars service: https://graphql.org/swapi-graphql
- Exploring GitHub: https://docs.github.com/en/graphql/overview/explorer
- GraphiQL.app (works with any GraphQL API service): https://github.com/skevy/graphiql-app
