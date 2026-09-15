---
title: Mapster — object to object mappers
source: Mapster.pdf
course_week: 5-6
topic: EF Core + LINQ + mapping
---

# Mapster — object to object mappers

Writing mapping methods is a machine job. Do not waste your time — let Mapster do it.

## Why?

On the backend it is often required to map from one model to another: entity → DTO and DTO → entity.

Tools that help with this:

- **AutoMapper** — widely used (been around almost forever), many features, 715 million NuGet downloads.
- **Mapster** — newer and smarter, faster, easy to set up, 32 million NuGet downloads.

## Mapster vs AutoMapper (benchmark)

| Method | Mean | Allocated |
|---|---|---|
| AutoMapper_SimpleMapping | 327.11 us | 109 KB |
| Mapster_SimpleMapping | 250.13 us | 109 KB |
| AutoMapper_ListOrArrayMapping | 245.02 us | 133 KB |
| Mapster_ListOrArrayMapping | 234.22 us | 125 KB |
| AutoMapper_NestedMapping | 149.77 us | 117 KB |
| Mapster_NestedMapping | 68.51 us | 117 KB |
| AutoMapper_FlattenedMapping | 162.06 us | 137 KB |
| Mapster_FlattenedMapping | 86.60 us | 137 KB |
| AutoMapper_CustomPropertyMapping | 226.93 us | 90 KB |
| Mapster_CustomPropertyMapping | 49.07 us | 39 KB |
| AutoMapper_ReverseMapping | 150.28 us | 117 KB |
| Mapster_ReverseMapping | 55.12 us | 55 KB |
| AutoMapper_AttributeMapping | 351.38 us | 117 KB |
| Mapster_AttributeMapping | 274.45 us | 117 KB |

Mapster is faster than AutoMapper in all cases; memory footprints are either the same or Mapster's is smaller. (Source: [code-maze.com/automapper-vs-mapster-dotnet](https://code-maze.com/automapper-vs-mapster-dotnet/))

## Installation

```bash
dotnet add package Mapster
```

## Basic usage

Mapping to a new object:

```csharp
var destObject = sourceObject.Adapt<Destination>();
```

Mapping to an existing object:

```csharp
Destination destObject;
sourceObject.Adapt(destObject);
```

Mapster copies public properties and fields with the same name from source to destination. It supports type casting between primitive types — the properties may use different data types, e.g. `DateTime` ↔ `string`.

## Example — manual vs Mapster

```csharp
// Manual mapping
public static ExerciseDto FromEntityExercise(Entities.Exercise eExercise)
{
    var exercise = new ExerciseDto();
    exercise.Description = eExercise.Description;
    exercise.Name = eExercise.Name;
    exercise.Sets = eExercise.Sets;
    exercise.Repetitions = eExercise.Repetitions;
    exercise.Time = eExercise.Time;
    return exercise;
}

// With Mapster
using Mapster;
public static ExerciseDto FromEntityExercise(Entities.Exercise eExercise)
{
    var exercise = eExercise.Adapt<ExerciseDto>();
    return exercise;
}
```

## Nested type mapping

Mapster supports nested type mapping with no special syntax required:

```csharp
public class Person
{
    public long PersonId { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Address? Address { get; set; }
}

public class PersonDto
{
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DateOfBirth { get; set; }   // note: string vs DateTime is fine
    public Address? Address { get; set; }
}

public class Address
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
}

var pp = DemoData.CreatePerson();
var p = pp.Adapt<PersonDto>();
```

## List or array mapping

Specify the destination type param as `List<TDestination>`, `TDestination[]` or similar:

```csharp
List<UserDto> destinationList = sourceList.Adapt<List<UserDto>>();
```

## Custom property mapping

Use the static `TypeAdapterConfig` class to explicitly declare custom mapping:

```csharp
public class UserDto
{
    public string FullName { get; set; } = null!;
}

TypeAdapterConfig<Person, UserDto>
    .NewConfig()
    .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

var u = pp.Adapt<UserDto>();
Console.WriteLine($"UserDto: {u.FullName}");
```

## Attribute mapping

Use attribute mapping when properties have different names in source and destination (other attributes exist too):

```csharp
using Mapster;

public class UserDto
{
    public string FullName { get; set; } = null!;
    [AdaptMember("DateOfBirth")]
    public DateTime? Born { get; set; }
}
```

## Queryable extensions

Mapster provides extensions to map queryables (projection at the SQL level):

```csharp
using (MyDbContext context = new MyDbContext())
{
    // Build a Select expression from the DTO
    var destinations = context.Sources.ProjectToType<Destination>().ToList();

    // Versus creating by hand
    var destinations = context.Sources.Select(c => new Destination {
        Id = c.Id,
        Name = c.Name,
        Surname = c.Surname,
        // ....
    })
    .ToList();
}
```

## Generating models & mappers — Mapster.Tool

Mapster.Tool can generate DTOs from entity models and explicit mappings, which has some advantages:

```csharp
[AdaptTo("[name]Dto"), GenerateMapper]
public class Student {
    // ...
}

// Generated:
public class StudentDto {
    // ...
}
public static class StudentMapper {
    public static StudentDto AdaptToDto(this Student poco) { ... }
    public static StudentDto AdaptTo(this Student poco, StudentDto dto) { ... }
    public static Expression<Func<Student, StudentDto>> ProjectToDto => ...;
}
```

## References

- [Mapster](https://github.com/MapsterMapper/Mapster)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [AutoMapper vs Mapster](https://code-maze.com/automapper-vs-mapster-dotnet/)
- [Mapster.Tool](https://github.com/MapsterMapper/Mapster/wiki/Mapster.Tool)
