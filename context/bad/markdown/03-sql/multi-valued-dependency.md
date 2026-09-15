---
title: "Multi-valued Dependency"
source: "Multi valued dpendency.html"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# Multi-valued dependency (flerværdiet afhængighed)

## Definition

If an attribute A determines a **set of values** in an attribute B, seen over several tuples, it is said that B is multi-value dependent on A.

Notation: **A →→ B** (A "pluralizes" B), alternatively written **A -> -> B** or **A ->> B**.

## Example: relation "Kunde"

| ID | Navn              | Amt          | Kommune   |
|----|-------------------|--------------|-----------|
| 2  | Alfa              | Århus        | Hadsten   |
| 5  | DSB               | Århus        | Århus     |
| 9  | Højbjerg Maskiner | Århus        | Silkeborg |
| 78 | Spritkompagniet   | Nordjyllands | Aalborg   |
| 12 | Kurts Møbler      | Århus        | Hadsten   |
| 8  | Pias Persienner   | Århus        | Silkeborg |
| 15 | Kurts Burger      | Fyn          | Assens    |

Here **Amt →→ Kommune** applies: Amt designates a specific selection (set) of Kommune values. It is inconceivable to find, for example, **Nordjyllands** and **Assens** in the same row — Assens does not lie in Nordjyllands Amt.

**Amt →→ Navn** does *not* apply if the same name cannot exist more than once. If the same Navn *can* occur multiple times, it can also occur in multiple Amter — and then the MVD would hold.

Key point: whether a multi-valued dependency holds is decided by the **domain rules**, not just by the data currently in the table.
