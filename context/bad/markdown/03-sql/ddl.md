---
title: "SW4BAD: DDL #2 — Constraints, Keys og Auto Increment"
source: "SW2BAD - DDL #2.pdf"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# DDL #2 — Constraints

## Primary key constraints

Two equivalent ways to declare a composite primary key — inline table constraint vs. named constraint:

```sql
CREATE TABLE Movies (
  title NVARCHAR(100),
  year INT,
  genre NVARCHAR(20),
  length INT,
  PRIMARY KEY (title, year)
);
```

```sql
CREATE TABLE Movies (
  title NVARCHAR(100),
  year INT,
  genre NVARCHAR(20),
  length INT,
  CONSTRAINT PK_Movies PRIMARY KEY (title, year)
);
```

Single-column primary key declared inline:

```sql
CREATE TABLE MovieStar (
  name NVARCHAR(100) PRIMARY KEY,
  address NVARCHAR(200),
  gender CHAR,
  birthdate DATE
);
```

## Foreign key constraints

Example schema — `StarsIn` references `MovieStar`:

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

Inline foreign key:

```sql
CREATE TABLE StarsIn (
  MovieTitle NVARCHAR(100),
  MovieYear INT,
  StarName NVARCHAR(100) FOREIGN KEY REFERENCES MovieStar (NAME)
);
```

Named constraint variant:

```sql
CREATE TABLE StarsIn (
  MovieTitle NVARCHAR(100),
  MovieYear INT,
  StarName NVARCHAR(100),
  CONSTRAINT FK_StarsInStarName FOREIGN KEY (StarName) REFERENCES MovieStar (NAME)
);
```

## Referential integrity

Problem: changes to the **referenced** (parent) relation, e.g. `MovieStar.Name`:

```sql
DELETE FROM MovieStar
WHERE Name = 'Milie Bobby';

UPDATE MovieStar
SET Name = 'M.B.'
WHERE Name = 'Milie Bobby';
```

If these succeeded blindly, `StarsIn` would contain a `StarName` ('Millie Bobby') that no longer exists in `MovieStar` — a dangling foreign key.

**Notice:** Changes in the *child* ("pointing to") relation are not a problem:

```sql
DELETE FROM StarsIn
WHERE StarName = 'Milie Bobby';
```

The foreign key constraint is not violated by deleting a referencing row.

### Solution: manual or automated actions

Referential actions on the foreign key:

- **NO ACTION** (default) — reject violating modifications.
- **CASCADE**
  - `ON DELETE CASCADE` — deletes the whole referencing row in `StarsIn`.
  - `ON UPDATE CASCADE` — updates the FK `StarName` in `StarsIn` as well.
- **SET NULL** — modifications/deletes to the referenced relation set the FK `StarName` to NULL.
- **SET DEFAULT** — modifications/deletes to the referenced relation set the FK `StarName` to its default value.

```sql
CREATE TABLE StarsIn (
  MovieTitle NVARCHAR(100),
  MovieYear INT,
  StarName NVARCHAR(100) FOREIGN KEY REFERENCES MovieStar (NAME) ON UPDATE CASCADE
);
```

## Other constraints

### CHECK

- Inline: restricts the values of the column.
- As table-level constraint: creates a restriction among values (can reference multiple columns).

```sql
CREATE TABLE Movies (
  title NVARCHAR(100),
  year INT,
  genre NVARCHAR(20) NOT NULL,
  length INT CHECK (length >= 0),
  CONSTRAINT CHK_Birthdate CHECK (DATEDIFF(year, birthdate, GETDATE()) > 18)
);
```

### DEFAULT

Used to set a default value for a column; the value is set on new inserts when none is supplied.

```sql
CREATE TABLE MovieStar (
  name NVARCHAR(100),
  address NVARCHAR(200) DEFAULT 'TBA',
  gender CHAR,
  birthdate DATE DEFAULT GETDATE()
);
```

## Auto increment (IDENTITY)

Auto-increment generates a unique number when a new record is inserted into a table.

- `IDENTITY(1,1)` — starts at 1, increments by 1.
- `IDENTITY(10,2)` — starts at 10, increments by 2.

```sql
CREATE TABLE Goods (
  GoodID INT IDENTITY(1,1),
  Name VARCHAR(128),
  CONSTRAINT PK_Goods PRIMARY KEY (GoodID)
);
```
