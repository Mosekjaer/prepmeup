---
title: "SW4BAD: Introduction to the Relational Model"
source: "SW4BAD - Relational Model.pdf"
course_week: 2
topic: Databaser intro + datamodellering
---

# Introduction to the Relational Model

Contents: tables, relations, schema, keys, referential integrity, functional dependencies, Data Definition Language.

## Data and tables

Running example: a movie streaming service. Develop an idea of what kinds of data you want to store and ways of categorizing it — what is similar? Give each common bit of data a label that describes the category of information: **Title**, **Year**, **Genre**, **Length**.

## The relational model

The relational model gives us a single way to represent data: as a two-dimensional table called a **relation**.

Terminology:

- **attribute** = column (interchangeable)
- **row** = record = tuple (interchangeable)

Example — the Movies relation:

| TITLE | YEAR | GENRE | LENGTH |
|---|---|---|---|
| Enola Holmes 2 | 2022 | Adventure | 129 |
| A Star Is Born | 2018 | Musical | 136 |
| The Life Of Brian | 1979 | Comedy | 93 |

## Schema

The name of a relation and the set of attributes for a relation is called the **schema** for that relation. We show it with the relation name followed by a parenthesized list of its attributes:

```text
Movies(title, year, genre, length)
```

Attribute **order does not matter** — `Movies(title, year, length, genre)` is also valid.

## Tuples

The rows of a relation, other than the header row containing the attribute names, are called **tuples**. A tuple has one component for each attribute of the relation — even if null. The tuple for row 3 above is written:

```text
(The Life Of Brian, 1979, Comedy, 93)
```

## Schema with domains

Each attribute can be given a domain (type):

```text
Movies(title: string, year: integer, genre: string, length: integer)
```

## Database schema

A database schema is the collection of relation schemas. Example — the StreamDB database with relations `Movies(title, year, genre, length)`, `MovieStar(name, address, gender, birthdate)`, and `StarsIn(movie title, movie year, star name)` linking them (plus `MovieExec` and `Studio` in the full example):

| MOVIE TITLE | MOVIE YEAR | STAR NAME |
|---|---|---|
| Enola Holmes 2 | 2022 | Millie Bobby |
| A Star Is Born | 2018 | Lady Gaga |
| The Life Of Brian | 1979 | John Cleese |

*StarsIn relation*

| NAME | ADDRESS | GENDER | BIRTHDATE |
|---|---|---|---|
| Millie Bobby | Illinois | F | 19/02/2004 |
| Lady Gaga | Malibu | B | 28/03/1986 |
| John Cleese | London | M | 27/10/1939 |

*MovieStar relation*

## Primary key

A **primary key** is a set of attributes (columns) that uniquely specify a tuple (row) in a relation (table).

- A primary key is a column in your table that makes each record unique
- The primary key column has to be designated as such when you create the table

Primary key rules:

- **A primary key can't be NULL** — if it's null, it can't be unique, because other records can also be NULL
- **The primary key must be given a value when the record is inserted** — inserting without a primary key risks NULL primary keys and duplicate rows

### Choosing a primary key for Movies

Is `title` enough? No — add the 1976 version of *A Star Is Born* and `title` is duplicated:

| TITLE | YEAR | GENRE | LENGTH |
|---|---|---|---|
| Enola Holmes 2 | 2022 | Adventure | 129 |
| A Star Is Born | 2018 | Musical | 136 |
| The Life Of Brian | 1979 | Comedy | 93 |
| A Star Is Born | 1976 | Musical | 139 |

The primary key is the composite **(title, year)**.

Primary keys are indicated by underlining the attributes in the schema (underline shown here as markup):

```text
Movies(title, year, genre, length)
       ^^^^^  ^^^^  (underlined = primary key)
```

## Foreign key

A **foreign key** is a set of attributes in a table that refers to the primary key of another table. The foreign key links the two tables. It has to be designated as such when you create the table.

Foreign key rules:

- The referenced relation must exist
- The referenced attribute must be part of the primary key of the referenced relation
- Data type and size of both keys must be the same

In the Movies database: `StarsIn(movie title, movie year, star name)` has foreign keys — `(movie title, movie year)` referencing `Movies(title, year)` and `star name` referencing `MovieStar(name)`.

## Referential integrity

The **referential integrity rule** requires that for every foreign key instance that exists in a table, the row (and thus the key instance) of the parent table associated with that foreign key instance must also exist.

The DBMS automatically checks constraints and issues errors at **CUD** operations (Create/Update/Delete):

- **R** (read) operations are fine because there is no change to the data
- Checked constraints include: primary key rules, foreign key rules, the referential integrity rule

## Functional dependency

A **functional dependency** specifies the association between two sets of attributes (columns), where one attribute (column or columns) determines the value of another attribute (column or columns).

Denoted **X → Y**: the attribute set on the left side of the arrow, X, is called the **determinant**; Y is called the **dependent**.

Example — in the table below we have `Address → Country`:

| NAME | ADDRESS | COUNTRY | BIRTHDATE |
|---|---|---|---|
| Millie Bobby | Illinois | USA | 19/02/2004 |
| Lady Gaga | Malibu | USA | 28/03/1986 |
| John Cleese | London | UK | 27/10/1939 |

Think of it as a mathematical function between columns/attributes — e.g. f(x) = x * 10 gives a column X and a column F(X) where X determines F(X) (1 → 10, 3 → 30, 5 → 50).

## Data Definition Language (DDL)

### CREATE / DROP DATABASE

`CREATE` adds a database, a table, and other database objects. `DROP` removes database objects.

```sql
CREATE DATABASE streamdb;
DROP DATABASE streamdb;
```

Note: **system databases** (e.g. `master`, `model` in SQL Server) are created by default in the DBMS; `CREATE DATABASE streamdb` adds `streamdb` alongside them and `DROP` removes it again.

### CREATE / DROP TABLE

Remember the schema `Movies(title: string, year: integer, genre: string, length: integer)` — the mapping is easy, but note the SQL types:

```sql
CREATE TABLE movies (
  title CHAR(100),
  year INT,
  genre CHAR(20),
  length INT);
```

```sql
DROP TABLE movies;
```

In the real world schemas can get very complex — keys and all that jazz:

```sql
CREATE TABLE movies (
  title CHAR(100),
  year INT,
  genre CHAR(20),
  length INT,
  PRIMARY KEY (title, year)
);
```

Hint — set which database the DBMS runs commands against:

```sql
USE streamDB;
```

If you create the table `movies` and it succeeds but you cannot see it, it was probably created in another DB... like `master`.

### ALTER

`ALTER` changes a database, a table, and other database objects (course focus: tables). Useful when a schema changes.

Adding a column:

```sql
ALTER TABLE movie ADD DIRECTOR CHAR(50);
```

Removing a column:

```sql
ALTER TABLE movie DROP COLUMN DIRECTOR;
```
