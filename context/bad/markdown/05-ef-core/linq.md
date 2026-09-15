---
title: LINQ — Language-INtegrated Query
source: LINQ.pdf
course_week: 5-6
topic: EF Core + LINQ + mapping
---

# LINQ — Language-INtegrated Query

## What is LINQ?

- LINQ is a uniform programming model for any kind of data. It enables you to query and manipulate data with a consistent model independent of data sources.
- LINQ defines a set of method names (called **standard query operators** / standard sequence operators), along with translation rules from **query expressions** to expressions using these method names, lambda expressions, and anonymous types.

```csharp
var adultNames = from person in people
                 where person.Age >= 18
                 select person.Name;
```

## Why use LINQ?

Query expressions benefit from what was previously available only to imperative code:

- Rich metadata
- Compile-time syntax checking
- Static typing
- IntelliSense

Net result: a better understanding of the intent of what the code is doing.

## Foundation — language features enabling LINQ

- Generics
- Anonymous methods
- Implicit typing of local variables
- Lambda expressions and expression trees
- Extension methods

## Expression trees

- Expression trees represent code in a tree-like data structure where each node is an expression.
- When a lambda expression is assigned to a variable of type `Expression<TDelegate>`, the compiler emits code to build an expression tree representing the lambda.
- The C# compiler can only generate expression trees from *expression lambdas* (single-line lambdas).

```csharp
Expression<Func<int, bool>> myLambda = num => num < 5;
```

### The use of expression trees

Both LINQ to Objects and LINQ to EF start with C# code and end with query results. The ability to execute the code **remotely** (as LINQ to EF does) comes through expression trees: `IEnumerable` executes locally, while `IQueryable<>` hands the expression tree to a provider (e.g. EF) that translates it to SQL.

## Extension methods

- Extension methods "add" methods to existing types without creating a new derived type, recompiling, or modifying the original type.
- They are a special kind of static method, but are called as if they were instance methods on the extended type.
- The first parameter specifies which type the method operates on, preceded by the `this` modifier.
- They are only in scope when you explicitly import their namespace with a `using` directive.

```csharp
public static class MyExtensions
{
    public static int WordCount(this String str) { ... }
}

string s = "Hello Extension Methods";
int i = s.WordCount();
```

## LINQ foundation

- LINQ works on a **sequence** of elements: an instance of a class implementing `IEnumerable<T>` (local query) or `IQueryable<T>` (remote query).
- A LINQ query operator works on an input sequence and produces some output value — an output sequence or a single scalar value.
- The standard query operators are implemented as extension methods in the static `System.Linq.Enumerable` class, defined almost entirely in terms of `IEnumerable<T>`.
- Every `IEnumerable<T>`-compatible information source gets the standard query operators simply by adding `using System.Linq;`.

The standard query operators allow **traversal, filter, and projection** operations to be expressed in a direct yet declarative way in any .NET-based language.

The developer is free to use named methods, anonymous methods, or lambda expressions with query operators. Lambdas provide the most direct and compact syntax — and crucially, they can be compiled as either **code or data**, allowing them to be processed at runtime by optimizers, translators, and evaluators.

## LINQ example — query vs method syntax

```csharp
string[] names = { "Burke", "Connor", "Frank", "Everett", "Albert", "George" };

var query = from s in names
            where s.Length == 5
            orderby s
            select s.ToUpper();

foreach (string item in query)
    Console.WriteLine(item);

// Is equivalent to
IEnumerable<string> query2 = names
    .Where(s => s.Length == 5)
    .OrderBy(s => s)
    .Select(s => s.ToUpper());

foreach (string item in query2)
    Console.WriteLine(item);
```

## Query syntax

### from

Every query expression starts by stating the source of a sequence of data:

```csharp
from element in source
```

The `element` part is just an identifier; `source` is a normal expression.

### select

Query expressions always end with either a `select` clause or a `group` clause. The `select` clause is known as a **projection**. Minimal (useless) query:

```csharp
var query = from name in names
            select name;
```

### OrderBy

`OrderBy` and `OrderByDescending` take a key extraction function producing the value used to sort:

```csharp
var s1 = names.OrderBy(s => s);
var s2 = names.OrderByDescending(s => s);
var s3 = names.OrderBy(s => s.Length);
var s4 = names.OrderByDescending(s => s.Length);
```

They also accept an optional comparison function that imposes a partial order over the keys.

### ThenBy

To allow multiple sort criteria, `OrderBy`/`OrderByDescending` return `OrderedSequence<T>` rather than `IEnumerable<T>`. Two operators are defined only on `OrderedSequence<T>`: `ThenBy` and `ThenByDescending`, which apply an additional (subordinate) sort criterion.

```csharp
var s1 = names.OrderBy(s => s.Length).ThenBy(s => s);
```

### Reverse

`Reverse` enumerates a sequence and yields the same values in reverse order. Unlike `OrderBy`, it doesn't consider the actual values — it relies solely on the order in which the underlying source produces them.

## LINQ extensibility

Third parties can augment the set of standard query operators with new domain-specific operators: LINQ to SQL, LINQ to Entities, LINQ to XML, LINQ to Google, LINQ to CSV, LINQ to Twitter, …

## References

- [LINQ: .NET Language-Integrated Query (Don Box, Anders Hejlsberg)](https://learn.microsoft.com/en-us/previous-versions/dotnet/articles/bb308959(v=msdn.10))
- [C# guide — LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
- [101 LINQ Samples](http://linqsamples.com/)
- [C# in Depth](http://csharpindepth.com/)
- Free book: LINQ Succinctly
