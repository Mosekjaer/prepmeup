---
title: "Normalization — 1NF, 2NF, 3NF og BCNF"
source: "Normalization.pdf"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# Normalization

"Structuring a relational database in order to reduce data redundancy and improve data integrity."

Each normal form establishes a set of rules that enhances the database schema against:

- redundancy
- contradicting data / inconsistency
- bad design and known anomalies:
  - insert anomalies
  - update anomalies
  - delete anomalies

## First Normal Form (1NF)

- Each table has a **primary key**.
- Using row order to convey information is not permitted.
- Mixing data types within the same column is not permitted.
- Repeating groups are not permitted.
- Each column contains **one and only one value** (atomic values).

## Superkey, candidate key, primary key

- A **superkey** is a set of one or more attributes which, taken collectively, allows us to identify uniquely a row in a table.
- Any superset of the attributes of a superkey is also a superkey.
- A **candidate key** is a superkey that is not reducible to another (smaller) superkey — i.e. minimal.
- A **primary key** is chosen among the available candidate keys.

Running example — the unnormalized `Report` table (primary key: report_no, author_id):

| report_no | editor | dept_no | dept_name | dept_addr | author_id | author_name | author_addr |
|-----------|--------|---------|-----------|-----------|-----------|-------------|-------------|
| 4216      | woolf  | 15      | design    | argus1    | 53        | mantei      | cs-tor      |
| 4216      | woolf  | 15      | design    | argus1    | 44        | bolton      | mathrev     |
| 4216      | woolf  | 15      | design    | argus1    | 71        | koenig      | mathrev     |
| 5789      | koenig | 27      | analysis  | argus2    | 26        | fry         | folkstone   |
| 5789      | koenig | 27      | analysis  | argus2    | 38        | umar        | prise       |
| 5789      | koenig | 27      | analysis  | argus2    | 71        | koenig      | mathrev     |

Here {report_no, author_id} is the minimal superkey, thus a candidate key, thus the primary key. Example of multiple candidates: add an author_ssn column — then {report_no, author_ssn} would also be a candidate key.

## Functional dependency (recap)

A functional dependency specifies the association between two sets of attributes (columns) where one attribute (or set of columns) determines the value of another. Denoted **X → Y**, where X (left side) is the **determinant** and Y is the **dependent**.

Example table where Address → Country:

| NAME         | ADDRESS  | COUNTRY | BIRTHDATE  |
|--------------|----------|---------|------------|
| Millie Bobby | Illinois | USA     | 19/02/2004 |
| Lady Gaga    | Malibu   | USA     | 28/03/1986 |
| John Cleese  | London   | UK      | 27/10/1939 |

Think of it as a mathematical function between columns/attributes, e.g. f(x) = x * 10: every X value maps to exactly one F(X) value (1 → 10, 3 → 30, 5 → 50).

## Functional dependencies for Report

Suppose we were given the following dependencies:

- report_no → editor, dept_no
- dept_no → dept_name, dept_addr
- author_id → author_name, author_addr

Plus **transitive** functional dependencies. Given that:

- report_no → dept_no
- dept_no → dept_name

we know:

- report_no → dept_name

and similarly:

- report_no → dept_addr

## Partial functional dependencies

Functional dependencies on **part of the primary key**. With primary key {report_no, author_id}:

- report_no → editor, dept_no
- author_id → author_name, author_addr

Both determinants are proper parts of the primary key — these are partial dependencies.

## Anomalies in the unnormalized table

### Insert anomaly

If a new editor is to be added to the table, it can only be done if the new editor is editing a report: both the report number and editor must be known to add a row, because you cannot have a primary key with a NULL value in most relational databases.

### Update anomaly

report_no, editor, and dept_no are duplicated for each author of the report. If the editor of a report changes, several rows must be updated — miss one and the data is inconsistent.

### Delete anomaly

If a report is removed, all rows associated with that report must be deleted. This has the side effect of deleting the information that associates an author_id with author_name and author_addr. A delete causes us to lose unrelated data.

## Second Normal Form (2NF)

- It is in First Normal Form.
- Each non-key attribute in the table must be dependent on the **entire** primary key = **no partial dependencies**.

Decomposition of `Report` guided by the FDs:

**Report 1** — report_no → editor, dept_no (and transitively dept_no → dept_name, dept_addr):

| report_no | editor | dept_no | dept_name | dept_addr |
|-----------|--------|---------|-----------|-----------|
| 4216      | woolf  | 15      | design    | argus 1   |
| 5789      | koenig | 27      | analysis  | argus 2   |

**Report 2** — author_id → author_name, author_addr:

| author_id | author_name | author_addr |
|-----------|-------------|-------------|
| 53        | mantei      | cs-tor      |
| 44        | bolton      | mathrev     |
| 71        | koenig      | mathrev     |
| 26        | fry         | folkstone   |
| 38        | umar        | prise       |
| 71        | koenig      | mathrev     |

**Report 3** — the relationship {report_no, author_id}; no FD on the pair (report_no, auth_id → _):

| report_no | author_id |
|-----------|-----------|
| 4216      | 53        |
| 4216      | 44        |
| 4216      | 71        |
| 5789      | 26        |
| 5789      | 38        |
| 5789      | 71        |

### Anomalies still present in 2NF

- **Delete anomaly:** if we delete a report (rows from Report 1 and Report 3), we have the side effect of deleting the association between dept_no, dept_name, and dept_addr.
- **Update anomaly** is also present for the same reason (dept_name/dept_addr duplicated per report in the same department).

Cause: the transitive dependency report_no → dept_no → dept_name, dept_addr still lives inside Report 1.

## Third Normal Form (3NF)

- It is in Second Normal Form.
- For every nontrivial functional dependency X → A, either:
  1. X is a superkey, **or**
  2. A is a member of a candidate key.

= **No transitive dependencies on the primary key.**

Report 1 is split to remove the transitive dependency:

**Report 11** — report_no → editor, dept_no:

| report_no | editor | dept_no |
|-----------|--------|---------|
| 4216      | woolf  | 15      |
| 5789      | koenig | 27      |

**Report 12** — dept_no → dept_name, dept_addr:

| dept_no | dept_name | dept_addr |
|---------|-----------|-----------|
| 15      | design    | argus 1   |
| 27      | analysis  | argus 2   |

Together with Report 2 (author_id → author_name, author_addr) and Report 3 (report_no, author_id) the schema is now in 3NF.

## Boyce-Codd Normal Form (BCNF)

If for every functional dependency X → A, **X is a superkey**.

Removes option 2 from Third Normal Form = no dependencies "ending in the key".

Example — `ClientInterview` (primary key: clientNo, interviewDate):

| clientNo | interviewDate | interviewTime | staffNo | roomNo |
|----------|---------------|---------------|---------|--------|
| CR76     | 13-May-05     | 10.30         | SG5     | G101   |
| CR56     | 13-May-05     | 12.00         | SG5     | G101   |
| CR74     | 13-May-05     | 12.00         | SG37    | G102   |
| CR56     | 1-Jul-05      | 10.30         | SG5     | G102   |

Functional dependencies:

- clientNo, interviewDate → interviewTime, staffNo, roomNo  *(candidate key)*
- staffNo, interviewDate, interviewTime → clientNo  *(candidate key)*
- roomNo, interviewDate, interviewTime → staffNo, clientNo  *(candidate key)*
- staffNo, interviewDate → roomNo  *(NOT a superkey)*

The last dependency's determinant {staffNo, interviewDate} is not a superkey, but its dependent roomNo is part of a candidate key — so `ClientInterview` is in 3rd normal form, **but not in BCNF**.
