---
title: "SW4BAD: DML — Data Manipulation Language"
source: "SW4BAD - DML .pdf"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# DML — Data Manipulation Language

Data Manipulation Language (DML) is the subset of SQL operations used to **insert, delete, and update** data in a database. DML statements modify stored data but not the schema or database objects (that is DDL's job).

Running example — the `Movies` table:

| TITLE             | YEAR | GENRE     | LENGTH |
|-------------------|------|-----------|--------|
| Enola Holmes 2    | 2022 | Adventure | 129    |
| A Star Is Born    | 2018 | Musical   | 136    |
| The Life Of Brian | 1979 | Comedy    | 93     |

## SELECT

```sql
-- All columns, all rows
SELECT * FROM Movies;

-- Specific columns
SELECT Title, Year FROM Movies;

-- System metadata works the same way (SQL Server)
SELECT name FROM sys.Databases;
```

## INSERT

Insert full rows (values in column order):

```sql
INSERT INTO Movies
VALUES
('Enola Holmes 2', 2022, 'Adventure', 129),
('A Star Is Born', 2018, 'Musical', 136),
('The Life Of Brian', 1979, 'Comedy', 93);
```

Insert only selected columns — the remaining columns become NULL (or their default):

```sql
INSERT INTO Movies (Title, Year)
VALUES
('Enola Holmes 2', 2022),
('A Star Is Born', 2018),
('The Life Of Brian', 1977);
```

## INSERT conflicts

Typical errors when inserting:

- **"The INSERT statement conflicted with the FOREIGN KEY constraint"** — you forgot to add data in the table pointed to by the foreign key first.
- **"Violation of PRIMARY KEY constraint ... Cannot insert duplicate key in object ... The duplicate key value is ..."** — you are trying to add a duplicate value into a primary key column.

## UPDATE

```sql
-- Update a specific row
UPDATE Movies
SET GENRE = 'Drama'
WHERE Title = 'A Star is Born';

-- WARNING: without WHERE the update applies to ALL rows
UPDATE Movies
SET GENRE = 'Drama';
```

The `WHERE <boolean condition>` clause filters which rows the command applies to — if omitted, it applies to all rows. `WHERE` works with other commands too:

```sql
SELECT * FROM Movies
WHERE Title = 'A Star is Born';
```

## DELETE

```sql
DELETE FROM Movies
WHERE Title = 'A Star Is Born';
```

## UPDATE and DELETE conflicts

Errors like:

- "The UPDATE statement conflicted with the REFERENCE constraint ..."
- "The UPDATE statement conflicted with the FOREIGN KEY ..."
- "The DELETE statement conflicted with the REFERENCE constraint ..."

mean **referential integrity found a problem**. By default SQL Server raises an error and the update/delete action on the row in the parent table is rolled back. You can alter your tables to change the default behavior — e.g. cascade the updates or deletes (`ON UPDATE CASCADE` / `ON DELETE CASCADE`).

## Customize query results

```sql
-- Order results by a column (default ASC)
SELECT *
FROM Movies
ORDER BY Title [DESC|ASC];

-- Distinct values
SELECT DISTINCT Title
FROM Movies;

-- Values in a range
SELECT Title
FROM Movies
WHERE Year BETWEEN 2021 AND 2023;

-- Values in a set
SELECT *
FROM Movies
WHERE Genre IN ('comedy', 'action');

-- Pattern matching (% = wildcard)
SELECT *
FROM Movies
WHERE Title LIKE ('A Star %');
```

## Nested queries (subqueries)

Example schema — `StarsIn(MovieTitle, MovieYear, StarName)` and `MovieStar(Name, Address, Gender, Birthdate)`:

| MOVIE TITLE       | MOVIE YEAR | STAR NAME    |
|-------------------|------------|--------------|
| Enola Holmes 2    | 2022       | Millie Bobby |
| A Star Is Born    | 2018       | Lady Gaga    |
| The Life Of Brian | 1979       | John Cleese  |

| NAME         | ADDRESS  | GENDER | BIRTHDATE  |
|--------------|----------|--------|------------|
| Millie Bobby | Illinois | F      | 19/02/2004 |
| Lady Gaga    | Malibu   | B      | 28/03/1986 |
| John Cleese  | London   | M      | 27/10/1939 |

Find all names and addresses of stars who were active in 2022:

```sql
SELECT Name, Adress
FROM MovieStar
WHERE Name IN (SELECT StarName
               FROM StarsIn
               WHERE MovieYear = 2022);
```

## Aggregates

SQL can compute functions on the results:

```sql
SELECT MIN(year) FROM movies;
SELECT MAX(year) FROM movies;
SELECT COUNT(*) FROM movies;
SELECT AVG(rating) FROM movies;
SELECT SUM(rating) FROM movies;
```

Example — `SUM` adds the values in the designated column:

```sql
SELECT SUM(sales) FROM cookie_sales
WHERE first_name = 'Nicole';
```

## GROUP BY

- Groups rows that have the same values into summary rows.
- Often used with aggregate functions.
- Solves queries like: "Find the sales for each entity...".

```sql
SELECT first_name, SUM(sales)
FROM cookie_sales
GROUP BY first_name
ORDER BY SUM(sales);
```

## Joins

### Cartesian join (CROSS JOIN)

Also called Cartesian product, cross product, cross join, or "no join". Returns every row from one table crossed with every row from the second.

```sql
SELECT t.toy, b.boy
FROM toys AS t
CROSS JOIN boys AS b;
```

- 5 toys × 4 boys = 20 result rows — every possible combination.
- Aliases (`AS`) can be used for columns or tables. `t.toy` means: table (alias) before the dot, column after it.
- Avoid cross joining large tables — the result set explodes and you risk hanging your machine.

### INNER JOIN

An INNER JOIN combines the records from two tables using comparison operators in a condition. Columns are returned only where the joined rows match the condition.

```sql
SELECT mc.last_name, mc.first_name, p.profession
FROM my_contacts AS mc
INNER JOIN profession AS p
ON mc.prof_id = p.prof_id;
```
