---
title: "SW4BAD: Transactions — ACID"
source: "SW2BAD - Transactions.pdf"
course_week: 2-3
topic: SQL, transaktioner og normalisering
---

# Transactions

Contents: ACID; serialized and parallelized examples.

## The classic example

Send 100 DKK from one account to another. You need to:

1. Read balance from account A (1000 DKK)
2. Write balance to account A (900 DKK)
3. Read balance from account B (300 DKK)
4. Write balance to account B (400 DKK)

**What happens if the process gets interrupted in the middle?** Money disappears or appears from nowhere. That is the problem transactions solve.

## ACID — what we want

### Atomicity

Either **all** operations of the transaction are reflected properly in the database, or **none** are.

### Consistency

Execution of a transaction in isolation (i.e., with no other transaction executing concurrently) preserves the consistency of the database.

### Isolation

Even though multiple transactions may execute concurrently, the system guarantees that, for every pair of transactions Ti and Tj, it appears to Ti that either Tj finished execution before Ti started, or Tj started execution after Ti finished. Each transaction is unaware of other transactions executing concurrently in the system.

### Durability

After a transaction completes successfully, the changes it has made to the database persist, even if there are system failures.

## Transactions in SQL

Treat multiple statements as a **single unit of work**. If the transaction is successful all changes are committed; otherwise all changes are cancelled.

Explicit transaction — starts with `BEGIN TRANSACTION`, ends with:

- `COMMIT` — close the transaction, persist the changes.
- `ROLLBACK` — undo all the changes.

```sql
BEGIN TRANSACTION
  UPDATE Account SET balance = balance + 100
  WHERE accNo = 1
  UPDATE Account SET balance = balance - 100
  WHERE accNo = 2
COMMIT
```

With error handling:

```sql
BEGIN TRANSACTION
  UPDATE Account SET balance = balance + 100
  WHERE accNo = 1
  UPDATE Account SET balance = balance - 100
  WHERE accNo = 2
IF (something went wrong)
  ROLLBACK
ELSE
  COMMIT
```

**Autocommit** — each single statement is its own transaction. All statements run outside explicit transactions are atomic transactions on their own.

## How the properties are achieved

### Durability

Three kinds of storage:

- **Volatile** — e.g. main memory, cache memory.
- **Non-volatile** — e.g. hard drive, CD-ROM.
- **Stable** — theoretically impossible; practically approximated with several instances of non-volatile storage.

To achieve durability, we use stable storage.

### Atomicity

Rollback or commit. But how to roll back? **Logging** — the DBMS logs operations so they can be undone.

### Isolation

Works trivially when transactions are executed serially — but we usually want concurrency.

Example schedules with T1 (transfer 50 from A to B) and T2 (transfer 10% of A to B):

**Serial schedule** (T1 then T2) — always correct:

```text
T1                    T2
read(A)
A := A - 50
write(A)
read(B)
B := B + 50
write(B)
commit
                      read(A)
                      temp := A * 0.1
                      A := A - temp
                      write(A)
                      read(B)
                      B := B + temp
                      write(B)
                      commit
```

**Interleaved but serializable schedule** — equivalent to the serial one, therefore correct:

```text
T1                    T2
read(A)
A := A - 50
write(A)
                      read(A)
                      temp := A * 0.1
                      A := A - temp
                      write(A)
read(B)
B := B + 50
write(B)
commit
                      read(B)
                      B := B + temp
                      write(B)
                      commit
```

**Non-serializable schedule** — T2 reads A *after* T1 has computed the new value but *before* T1 writes it; T2's write(A) is then overwritten by T1's write(A):

```text
T1                    T2
read(A)
A := A - 50
                      read(A)
                      temp := A * 0.1
                      A := A - temp
                      write(A)
                      read(B)
write(A)
read(B)
B := B + 50
write(B)
commit
                      B := B + temp
                      write(B)
                      commit
```

The result is not equivalent to any serial execution — updates are lost, and the sum A + B is no longer preserved. Concurrency control must prevent such schedules.
