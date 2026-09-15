---
title: "Database Systems: The Complete Book — Ch. 2: The Relational Model of Data"
source: Database Systems - The complete book - second edition - Chapter 2 - Copy.pdf
course_week: 2
topic: Databaser intro + datamodellering (forberedelseslæsning)
---

# Chapter 2: The Relational Model of Data

> **Note:** This is a structured study-notes rendition of the chapter (Garcia-Molina, Ullman & Widom, 2nd ed., pp. 17–36), not a verbatim transcription. All definitions, schemas, tables, and SQL examples are preserved; the surrounding prose is condensed. The provided extract covers sections 2.1–2.3 (data models, relational basics, and SQL DDL). The chapter's remaining sections on relational algebra (2.4) and constraints (2.5) are not in the extract.

The chapter introduces the relational model — the two-dimensional table, or "relation." It covers data models in general, relational terminology, and the data-definition part of SQL. (The full chapter continues with relational algebra as both a query language and a constraint language.)

## 2.1 An Overview of Data Models

### 2.1.1 What is a Data Model?

A **data model** is a notation for describing data or information. It has three parts:

1. **Structure of the data.** Analogous to arrays/structs in C or objects in Java, but at a higher level of abstraction. In-computer data structures are sometimes called a *physical data model*; database data models sit higher and are called *conceptual models*.
2. **Operations on the data.** Unlike general programming languages, database models allow only a limited set of operations: *queries* (retrieve information) and *modifications* (change the database). This limitation is a strength, not a weakness — it lets programmers describe operations at a very high level while the DBMS implements them efficiently. Conventional-language compilers cannot, e.g., swap a bubblesort for a quicksort; a DBMS optimizer effectively can make such algorithmic improvements.
3. **Constraints on the data.** Ways to state what the data is allowed to be — from simple ("a day of the week is an integer between 1 and 7", "a movie has at most one title") to complex constraints (covered later in Ch. 7).

### 2.1.2 Important Data Models

The two preeminent models today:

1. **The relational model**, including object-relational extensions — present in all commercial DBMS's; subject of this chapter.
2. **The semistructured-data model**, including XML and related standards — an added feature of most relational DBMS's (covered from Ch. 11).

### 2.1.3 The Relational Model in Brief

The relational model is based on tables. Example relation (Fig. 2.1):

| title              | year | length | genre  |
|--------------------|------|--------|--------|
| Gone With the Wind | 1939 | 231    | drama  |
| Star Wars          | 1977 | 124    | sciFi  |
| Wayne's World      | 1992 | 95     | comedy |

- **Structure:** looks like an array of structs (column headers = field names, rows = struct values), but that is only one possible physical implementation — and not the normal one. Relations are typically too large for main memory; much of database systems research concerns implementing tables that live on disk.
- **Operations:** the *relational algebra* — table-oriented operations, e.g., "give me all rows where genre = comedy."
- **Constraints:** e.g., require that the genre value comes from a fixed list, or (incorrectly, as it turns out) that no two rows share the same title.

### 2.1.4 The Semistructured Model in Brief

Semistructured data resembles trees or graphs rather than tables. The main manifestation is **XML**: hierarchically nested tagged elements, where tags (like HTML's) define the role of each piece of data, much as column headers do in a relation. The same movie data as XML (Fig. 2.2):

```xml
<Movies>
    <Movie title="Gone With the Wind">
        <Year>1939</Year>
        <Length>231</Length>
        <Genre>drama</Genre>
    </Movie>
    <Movie title="Star Wars">
        <Year>1977</Year>
        <Length>124</Length>
        <Genre>sciFi</Genre>
    </Movie>
    <Movie title="Wayne's World">
        <Year>1992</Year>
        <Length>95</Length>
        <Genre>comedy</Genre>
    </Movie>
</Movies>
```

- **Operations:** follow paths in the implied tree — from an element to its nested subelements, and so on (e.g., from `<Movies>` to each `<Movie>` to its `<Genre>` to find comedies).
- **Constraints:** data types of tag values (must `<Length>` be an integer?), and which tags may nest within which (must every `<Movie>` contain a `<Length>`? can a movie have more than one genre?).

### 2.1.5 Other Data Models

- **Object-relational model:** adds object-oriented features to relations — (1) values can have structure rather than being elementary types, and (2) relations can have associated methods. Analogous to C structs → C++ objects. (Section 10.3.)
- **Purely object-oriented database models:** the relation is just one structure among many. (Section 4.9.)
- **Legacy models, now out of use:**
  - *Hierarchical model* — tree-oriented, but operated at the physical level, preventing conveniently high-level programming.
  - *Network model* — graph-oriented, also physical-level. Both it and the hierarchical/semistructured family actually allow full graphs; the network model built graph generality in directly rather than favoring trees.

### 2.1.6 Comparison of Modeling Approaches

Semistructured models look more flexible than relations, yet the relational model is preferred in DBMS's. Why: databases are large, so efficiency of access and modification matter greatly, as does programmer productivity. Both are achieved by a model that:

1. Provides a simple, limited, yet reasonably versatile way to structure data, and
2. Provides a limited, yet useful, collection of operations.

These limitations become features: they make languages like SQL possible, where a few lines do the work of thousands of lines of C (or hundreds under the network/hierarchical models), and where the strongly limited operation set lets queries be optimized to run as fast or faster than hand-written alternatives.

## 2.2 Basics of the Relational Model

The relational model represents data as a two-dimensional table called a **relation**. Running example, the relation `Movies` (Fig. 2.3):

| title              | year | length | genre  |
|--------------------|------|--------|--------|
| Gone With the Wind | 1939 | 231    | drama  |
| Star Wars          | 1977 | 124    | sciFi  |
| Wayne's World      | 1992 | 95     | comedy |

Rows represent movies; columns represent properties of movies.

### 2.2.1 Attributes

The columns are named by **attributes** — here `title`, `year`, `length`, `genre`. An attribute appears at the top of its column and describes the meaning of the entries below it (e.g., `length` holds each movie's length in minutes).

### 2.2.2 Schemas

The relation name plus its set of attributes is the **schema** of the relation, written as the name followed by a parenthesized attribute list:

```
Movies(title, year, length, genre)
```

The attributes in a schema are a **set**, not a list — but to talk about relations we fix a "standard" order, taken to be the order in which the attributes were introduced.

A **database** in the relational model is one or more relations. The set of schemas for its relations is the **relational database schema** (or just *database schema*).

> **Naming convention:** relation names start with a capital letter, attribute names with a lower-case letter. When discussing relations in the abstract, single capitals are used for both, e.g., `R(A, B, C)`.

### 2.2.3 Tuples

The rows (excluding the header) are **tuples**. A tuple has one component per attribute. Written in isolation, a tuple uses parentheses and comma-separated components:

```
(Gone With the Wind, 1939, 231, drama)
```

In isolation the attributes don't appear, so the relation the tuple belongs to must be indicated; components follow the standard attribute order of the schema.

### 2.2.4 Domains

Each component of each tuple must be **atomic** — an elementary type such as integer or string. Record structures, sets, lists, arrays, or anything decomposable into smaller components is not permitted.

Each attribute has an associated **domain** — a particular elementary type — and every tuple component must belong to the domain of its column. Domains can be written in the schema by appending a colon and a type:

```
Movies(title:string, year:integer, length:integer, genre:string)
```

### 2.2.5 Equivalent Representations of a Relation

Relations are **sets** of tuples, not lists — tuple order is immaterial (the three tuples of Fig. 2.3 can be listed in any of their six orders). Attributes can also be reordered, provided the columns (and hence every tuple's components) are permuted the same way. Example — another presentation of the same relation `Movies` (Fig. 2.4):

| year | genre  | title              | length |
|------|--------|--------------------|--------|
| 1977 | sciFi  | Star Wars          | 124    |
| 1992 | comedy | Wayne's World      | 95     |
| 1939 | drama  | Gone With the Wind | 231    |

The two tables are different *presentations* of the same relation.

### 2.2.6 Relation Instances

Relations change over time: tuples are inserted (new movies), updated (corrections), and deleted. Schema changes are possible but rare and expensive — millions of tuples may need rewriting, and appropriate values for a newly added attribute may be hard or impossible to supply for existing tuples.

A set of tuples for a given relation is an **instance** of that relation. A conventional DBMS maintains only one version — the set of tuples in the relation *now* — called the **current instance**. (Databases that keep historical versions are called *temporal databases*.)

### 2.2.7 Keys of Relations

The most fundamental constraint: a set of attributes forms a **key** for a relation if no two tuples in any instance may agree on all attributes of the key.

**Example 2.1.** `Movies` has key `{title, year}`: no two movies should share both title and year. `title` alone is not a key (remakes exist — three different movies named *King Kong*, each in a different year), and `year` alone obviously isn't (many movies per year).

Key attributes are indicated by underlining; conventionally written:

```
Movies(title, year, length, genre)     -- key: title, year (underlined in the book)
```

Important points:

- A key is a statement about **all possible instances**, not one instance. In the tiny Fig. 2.3 instance no two tuples share a genre, but a larger instance would have many dramas and comedies — so `genre` is not a key.
- Real-world databases often use **artificial keys** rather than trusting attributes outside their control: employee IDs, Social Security numbers, student IDs, driver's license numbers, automobile registration numbers. Several key choices can coexist (e.g., employees with both an employee ID and an SSN); nothing is wrong with that.

### 2.2.8 An Example Database Schema

The running movie database schema (Fig. 2.5; keys underlined in the book, noted here in comments):

```
Movies(
    title:string,          -- key (with year)
    year:integer,          -- key (with title)
    length:integer,
    genre:string,
    studioName:string,
    producerC#:integer
)
MovieStar(
    name:string,           -- key
    address:string,
    gender:char,
    birthdate:date
)
StarsIn(
    movieTitle:string,     -- key (all three attributes together)
    movieYear:integer,     -- key
    starName:string        -- key
)
MovieExec(
    name:string,
    address:string,
    cert#:integer,         -- key
    netWorth:integer
)
Studio(
    name:string,           -- key
    address:string,
    presC#:integer
)
```

What each relation means:

- **Movies** — extends the earlier example. Key: `title` + `year`. New attributes: `studioName` (owning studio) and `producerC#` (integer identifying the producer; see `MovieExec`).
- **MovieStar** — stars. Key: `name`. Person names are normally not safe as keys, but the book adopts the convenient fiction that movie-star names are unique (a star would never reuse another star's name). A more conventional approach would invent a serial number — as is done for executives. New data types appear: `gender` is a single character (`M`/`F`), `birthdate` is of type "date."
- **StarsIn** — connects movies to their stars and vice versa. Movies are represented by the key of `Movies` (title + year) under the names `movieTitle`/`movieYear`; stars by the key of `MovieStar` as `starName`. **All three attributes are needed for the key**: e.g., a star appearing in two movies in one year gives two tuples agreeing on `movieYear` and `starName` but differing in `movieTitle`.
- **MovieExec** — movie executives: name, address, net worth. Key: invented "certificate numbers" (`cert#`), unique integers assigned to each executive — producers (referenced from `Movies`) and studio presidents (referenced from `Studio`) alike.
- **Studio** — studios: name, address, and the certificate number of the president (`presC#`). Key: `name` (no two studios share a name). The president is assumed to appear in `MovieExec`.

### 2.2.9 Exercises for Section 2.2

Banking database instances (Fig. 2.6):

**The relation Accounts:**

| acctNo | type     | balance |
|--------|----------|---------|
| 12345  | savings  | 12000   |
| 23456  | checking | 1000    |
| 34567  | savings  | 25      |

**The relation Customers:**

| firstName | lastName | idNo    | account |
|-----------|----------|---------|---------|
| Robbie    | Banks    | 901-222 | 12345   |
| Lena      | Hand     | 805-333 | 12345   |
| Lena      | Hand     | 805-333 | 23456   |

- **Exercise 2.2.1:** For Fig. 2.6, identify: (a) the attributes of each relation; (b) the tuples; (c) the components of one tuple from each; (d) each relation schema; (e) the database schema; (f) a suitable domain for each attribute; (g) another equivalent presentation of each relation.
- **Exercise 2.2.2:** Give additional examples of attributes created for the purpose of serving as keys (cf. Section 2.2.7).
- **Exercise 2.2.3:** How many different ways (counting orders of tuples and attributes) can a relation instance be presented, if it has: (a) three attributes and three tuples (like `Accounts`); (b) four attributes and five tuples; (c) *n* attributes and *m* tuples?

## 2.3 Defining a Relation Schema in SQL

**SQL** (pronounced "sequel") is the principal language for describing and manipulating relational databases. The current standard is **SQL-99**; commercial DBMS's implement something similar but not identical. SQL has two aspects:

1. The **Data-Definition** sublanguage — declaring database schemas (this section; more in Ch. 7, especially constraints).
2. The **Data-Manipulation** sublanguage — querying and modifying the database (Ch. 6).

The split mirrors conventional languages (C/Java also have data-declaring portions and executable-code portions).

### 2.3.1 Relations in SQL

SQL distinguishes three kinds of relations:

1. **Stored relations (tables)** — exist in the database; can be queried and modified. The ordinary kind.
2. **Views** — relations defined by a computation; not stored, but constructed (wholly or partly) when needed. (Section 8.1.)
3. **Temporary tables** — constructed by the SQL processor during query/modification execution, then thrown away; never declared.

The `CREATE TABLE` statement declares the schema of a stored relation: name, attributes, data types, and optionally one or more keys. (It also supports many constraint forms and index declarations, deferred to later chapters.)

### 2.3.2 Data Types

Primitive SQL data types (every attribute must have one):

1. **Character strings**, fixed or varying length. `CHAR(n)` — fixed-length string of up to *n* characters; `VARCHAR(n)` — string of up to *n* characters. The difference is implementation-dependent: typically `CHAR` pads short strings to *n* characters, `VARCHAR` uses an endmarker or length count. SQL coerces reasonably between string types — e.g., `'foo'` stored into a `CHAR(5)` component becomes `'foo  '` (two trailing blanks). Note: SQL strings use *single* quotes, not double quotes.
2. **Bit strings**, fixed or varying length: `BIT(n)` (length exactly *n*) and `BIT VARYING(n)` (length up to *n*).
3. **`BOOLEAN`** — logical values: `TRUE`, `FALSE`, and (to George Boole's surprise) `UNKNOWN`.
4. **Integers**: `INT` / `INTEGER` (synonyms); `SHORTINT` may allow fewer bits, implementation-dependent (like `int` vs. `short int` in C).
5. **Floating-point numbers**: `FLOAT` / `REAL` (synonyms); `DOUBLE PRECISION` for higher precision (distinctions as in C). Fixed-decimal reals: `DECIMAL(n,d)` — *n* decimal digits with the decimal point *d* positions from the right (e.g., `0123.45` fits `DECIMAL(6,2)`). `NUMERIC` is nearly a synonym for `DECIMAL`, with possible implementation-dependent differences.
6. **Dates and times**: types `DATE` and `TIME` — essentially character strings of a special form, coercible to/from string types when the string "makes sense" as a date or time.

> **Dates and times in SQL (standard representation):**
> A date value is the keyword `DATE` followed by a quoted string `'yyyy-mm-dd'` — e.g., `DATE '1948-05-14'`. Single-digit months/days are zero-padded.
> A time value is the keyword `TIME` and a quoted string: two digits for the hour (24-hour clock), colon, two digits for minutes, colon, two digits for seconds, optionally a decimal point and fractional seconds — e.g., `TIME '15:00:02.5'` (two and a half seconds past 3 PM).

### 2.3.3 Simple Table Declarations

Simplest form: `CREATE TABLE`, the relation name, then a parenthesized comma-separated list of attribute names and types.

**Example 2.2.** Declaring `Movies` (Fig. 2.7):

```sql
CREATE TABLE Movies (
    title      CHAR(100),
    year       INT,
    length     INT,
    genre      CHAR(10),
    studioName CHAR(30),
    producerC# INT
);
```

The length choices (100 for title, 10 for genre, 30 for studio name) are arbitrary trade-offs — too small and long values get truncated.

**Example 2.3.** Declaring `MovieStar` (Fig. 2.8), illustrating both major string types plus `CHAR(1)` and `DATE`:

```sql
CREATE TABLE MovieStar (
    name      CHAR(30),
    address   VARCHAR(255),
    gender    CHAR(1),
    birthdate DATE
);
```

`name` is a fixed-length 30-character string (blank-padded / truncated as needed); `address` is variable-length up to 255. (Why 255? A single byte holds 0–255, so a varying string of up to 255 bytes needs just one byte for the count — though commercial systems generally support longer strings.) `gender` holds a single letter `M` or `F`; `birthdate` naturally gets type `DATE`.

### 2.3.4 Modifying Relation Schemas

Delete an entire relation `R` (schema and all tuples):

```sql
DROP TABLE R;
```

More commonly, modify an existing relation's schema with `ALTER TABLE` plus the relation name, then most importantly:

1. `ADD` followed by an attribute name and data type, or
2. `DROP` followed by an attribute name.

**Example 2.4.**

```sql
ALTER TABLE MovieStar ADD phone CHAR(16);
```

`MovieStar` now has five attributes; existing tuples get the special **null value** `NULL` for `phone` since no numbers are known. And:

```sql
ALTER TABLE MovieStar DROP birthdate;
```

removes `birthdate` from the schema and deletes that component from every existing tuple.

### 2.3.5 Default Values

A **default value** appears in a column when no other value is known. Anywhere an attribute and type are declared, append `DEFAULT` and a value — `NULL`, a constant, or certain system-provided values (e.g., the current time).

**Example 2.5.** Using `?` for unknown gender and the earliest possible date for unknown birthdate:

```sql
gender    CHAR(1) DEFAULT '?',
birthdate DATE    DEFAULT DATE '0000-00-00'
```

Or giving the new `phone` attribute a default at ADD time:

```sql
ALTER TABLE MovieStar ADD phone CHAR(16) DEFAULT 'unlisted';
```

### 2.3.6 Declaring Keys

Two ways to declare a key in `CREATE TABLE`:

1. Declare one attribute to be a key on its own line in the schema.
2. Add a separate declaration item stating that a particular attribute or set of attributes forms the key.

A multi-attribute key **must** use method (2); a single-attribute key may use either. Two keywords indicate keyness:

- `PRIMARY KEY`
- `UNIQUE`

Effect of declaring attribute set *S* a key of relation *R* (either keyword): two tuples of *R* cannot agree on all attributes of *S*, unless one of them is `NULL`; any insert/update violating this is rejected by the DBMS. Additional difference: with `PRIMARY KEY`, attributes of *S* may **not** be `NULL`; with `UNIQUE`, `NULL` is permitted. A DBMS may make further distinctions.

**Example 2.6.** `name` as key of `MovieStar`, inline (Fig. 2.9):

```sql
CREATE TABLE MovieStar (
    name      CHAR(30) PRIMARY KEY,
    address   VARCHAR(255),
    gender    CHAR(1),
    birthdate DATE
);
```

Or as a separate declaration (Fig. 2.10):

```sql
CREATE TABLE MovieStar (
    name      CHAR(30),
    address   VARCHAR(255),
    gender    CHAR(1),
    birthdate DATE,
    PRIMARY KEY (name)
);
```

In both, `UNIQUE` could replace `PRIMARY KEY` — then multiple tuples could have `NULL` names, but no other duplicates.

**Example 2.7.** A multi-attribute key requires the separate-declaration style. `Movies`, key `(title, year)` (Fig. 2.11):

```sql
CREATE TABLE Movies (
    title      CHAR(100),
    year       INT,
    length     INT,
    genre      CHAR(10),
    studioName CHAR(30),
    producerC# INT,
    PRIMARY KEY (title, year)
);
```

As usual, `UNIQUE` may replace `PRIMARY KEY`.

### 2.3.7 Exercises for Section 2.3

**Exercise 2.3.1** introduces a running example: a products database with four relations:

```
Product(maker, model, type)
PC(model, speed, ram, hd, price)
Laptop(model, speed, ram, hd, screen, price)
Printer(model, color, type, price)
```

- `Product`: manufacturer, model number, and type (PC, laptop, or printer). Model numbers are assumed unique across all manufacturers and product types (unrealistic; a real database would encode the manufacturer in the model number).
- `PC`: per PC model — processor speed (GHz), RAM (MB), hard-disk size (GB), price.
- `Laptop`: same, plus screen size (inches).
- `Printer`: per printer model — whether it prints color (boolean), process type (laser or ink-jet, typically), price.

Task (as far as the extract goes): write suitable declarations, starting with (a) a schema for relation `Product`. *(The extract ends here, mid-exercise; the remainder of the exercise list and sections 2.4–2.5 are not included in the source PDF excerpt.)*
