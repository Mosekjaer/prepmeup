---
title: Key Concepts — database keys
source: Master/L5/KeyConcepts.png
course_week: 5-6
topic: EF Core + LINQ + mapping
---

# Key Concepts — database keys

Diagram of how the key concepts in relational databases relate.

**A key** — purpose: to determine a row uniquely.

## Conceptual keys

- **Super key** — *every* set of attributes that determines a row uniquely.
- **Candidate keys** — the *minimal* sets of attributes that determine a row uniquely. Every candidate key is a super key.

## Relational DB design

Supports relational algebra's demand for uniqueness (the Set clause).

- **Primary key** — chosen from the candidate keys. It is the determinant.
- **Secondary key** — the remaining candidate keys not chosen as primary key.
- **Foreign key** — points to a primary key (in another table).

Relationships in the diagram:

- Candidate keys *are* super keys.
- Primary key and secondary key *are chosen from* the candidate keys.
- Primary key *is determinant*.
- Foreign key *points to* primary key.
