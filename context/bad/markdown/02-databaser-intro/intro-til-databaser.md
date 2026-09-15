---
title: "SW4BAD: Introduction to Databases"
source: "SW4BAD - Intro To Databases.pdf"
course_week: 2
topic: Databaser intro + datamodellering
---

# Introduction to Databases

Contents: Introduction to Databases, Database Management Systems (DBMS), Connecting to the DBMS, SQL.

## What is a database?

Databases = Data + bases — it is about data.

The DIKW pyramid distinguishes:

- **Data** — a given/fact/signal/symbol (observed)
- **Information** — the meaning inferred from the data
- **Knowledge** and **Wisdom** — increasingly ambiguous/philosophical, not covered in this course

**Definition (by the book):** a database is *data stored in a computer (system) and a description of the data, designed to meet the information needs of an organization*. It is data organization with a purpose.

## Example organizations

Restaurant, university, supermarket, stock market, medical system, personal registry system (CPR no), airline, air transportation authority, railway company, carsharing app, SKAT, ... Each deals with data samples whose information value serves the organization's needs.

## Where databases fit in a system

The database sits behind a backend system: client devices (phones, laptops, screens) connect over the web to a backend that provides an API, and the backend uses the database. It can also just be an app running on a single machine — your laptop.

In your education: SW4BAD covers the backend/database side; SW4FED covers the frontend/client side.

## Summary in software imagery

The tempting mental model:

- A database is a cylinder
- The cylinder contains rectangles (tables)
- The cylinder is inside a computer
- Clients use the web to connect to the backend system that provides an API
- Users are given access to data via clients

Almost right — but rectangles (tables) are only for **relational** databases. There are alternative data structures, e.g. JSON, XML, ... This course starts with relational databases anyway.

## Database Management System (DBMS)

In standard applications a database is an **abstraction on top of the filesystem**. Instead of saving data in files we have a Database Management System (DBMS) — an application managing data.

> DBMS = Database server = Database engine

Instead of read/write-to-file operations it provides standard operations:

- **CRUD** — Create, Read, Update and Delete operations
- Access over network
- Manages multiple simultaneous connections to data

## Finding the DBMS on your machine

The DBMS is yet another application. Depending on the setup:

- **Local application** — visible in Task Manager as e.g. `sqllite.exe`, `sqlservr.exe`, `mongodb.exe`, ...
- **Container** — you find `docker` in Task Manager; inside the Linux container runs e.g. `sqlservr` (mssql-server) or `mongod`

## Connecting to the DBMS

We use the database via the **client-server pattern**:

- Client in this course: **Azure Data Studio**
- Alternative: a shell client such as `sqlcmd`

## Programming "relational" DBMS — SQL

**SQL** (pronounced "sequel") is the principal language used to describe and manipulate relational databases. It provides commands to interact with the database.

It is a standard, but most commercial DBMS adapt it — for SQL Server, look for **Transact-SQL** (T-SQL).

Aspects of SQL:

- **DDL** — the Data-Definition sublanguage for declaring database structures
- **DML** — the Data-Manipulation sublanguage for querying (asking questions about) databases and modifying the database
- ...

## Summary

- A database is data stored in a computer (system) and a description of the data designed to meet the information needs of an organization
- A database is also a running server
- We connect to the database server through the network using a client
- Instead of read/write on files, databases provide standardized CRUD operations allowing multiple connections at the same time
- CRUD operations are standardized in SQL commands
