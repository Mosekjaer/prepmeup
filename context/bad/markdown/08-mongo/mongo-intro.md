---
title: Mongo Intro — NoSQL / Document Databases
source: Mongo-intro.pdf
course_week: 8-9
topic: MongoDB
---

# Mongo Intro

Agenda: NoSQL / document databases, Mongo intro, Mongo schema design, working with Mongo.

## NoSQL

- A different category of database that **N**ot **O**nly supports **SQL**.
- Each with a different data model and different performance characteristics.
- Types of NoSQL databases: **document**, **graph**, **key-value**, **columnar**. The focus here is document databases.

## Document databases

- Breaks 1st normal form: each row can contain more than one value (e.g. a list of phone numbers).
- Designed to make it easier (and faster) to scale horizontally on different nodes/servers.
- Possible because:
  - No JOIN operation (when getting all phone numbers, for instance)
  - Sharding

### Document database structure

- No schema.
- Database → Collections → Document(s), e.g. "Users" (collection) → User (document) → social media account (embedded document).
- Embedded data.

### Design consequences

- No longer a single "given way" of designing a database — many choices!
- In many cases data is denormalized (there can even be multiple copies of the same data — a big no-no in standard SQL).
- Embed all (or most) related data into a single document (BSON in MongoDB).
- Retrieve or write document(s) in a single operation.
- Fewer queries needed in general.

### Trade-off: faster reads vs. (potentially) longer writes

**Faster reads** — joins not (necessarily) needed. Example, "get author and written books" in one document:

```json
{"id": "1",
   "name": "O'Reilly",
   "books": [
      { "title": "Learning python" },
      { "title": "Jenkins 2 - up & running" },
      { "title": "Head First Kotlin" },
      { "title": "Mastering Ethereum" }
   ]
}
```

**(Potentially) longer writes** — an update (e.g. "update book's title") applies to multiple places, and there are no foreign key constraints, since the same book may exist both embedded in the author document and as its own document in a Books collection.

## MongoDB — a document database

- Another popular example of a document database is Google's Firestore.
- MongoDB is to NoSQL/document databases what MSSQL is to SQL/relational databases — an implementation.
- A free-to-use document database.
- Stores a list of documents in collections.
- Uses BSON types (binary representation of JSON): arrays of values, objects, null, etc.
- Allows documents to be up to 16 MB.

## MongoDB — example data

```json
{ "_id": ObjectId("624d83ab9225bfaa18640a1f"),
    "firstName": "Thomas",
    "lastName": "Andersen",
    "addresses": [
        { "line1": "100 Some Street",
          "line2": "Unit 1",
          "city": "Seattle",
          "state": "WA",
          "zip": 98012
        }
    ],
    "contactDetails": [
        { "email": "thomas@andersen.com" },
        { "phone": "+1 555 555-5555", "extension": 5555 }
    ]
}
```

- `_id` is the key of the document.
- `addresses` is an array — multiple addresses could be embedded here.
- `contactDetails` is an array with two elements, each with its own key(s)/value(s).

## MongoDB — terminology

- A database consists of one or more collections (≈ tables).
- Documents are stored in collections.
- A MongoDB document is like a tuple (list of something).
- Consists of key:value pairs.
- Values can be: other documents, arrays, and simple types.

## MongoDB — BSON types (not all shown)

| Type | Number | Alias | Notes |
|---|---|---|---|
| Double | 1 | "double" | |
| String | 2 | "string" | |
| Object | 3 | "object" | |
| Array | 4 | "array" | |
| Binary data | 5 | "binData" | |
| Undefined | 6 | "undefined" | Deprecated |
| ObjectId | 7 | "objectId" | |
| Boolean | 8 | "bool" | |
| Date | 9 | "date" | |
| Null | 10 | "null" | |

Full list: https://www.mongodb.com/docs/manual/reference/bson-types/

## MongoDB features

- **Performance** — fewer joins, as more data can be embedded in a single document; keys (sub-documents).
- **Query language** — CRUD and data searches.
- **Availability** — via replica sets: automatic failover, data redundancy.
- **Scalability** — sharding.

Why backups are important: https://jyllands-posten.dk/indland/ECE10312652/fakta-hackerangreb-kostede-maersk-over-en-milliard-kroner/

## MongoDB — sharding

- **Shard** — contains a subset of the data.
- **Mongos** — query router.
- **Config servers** — metadata and configurations.
- Uses shard key(s) to distribute data; all data must contain this key.
- Gives: distributed reads/writes, storage, high availability — at the cost of complexity and infrastructure.
- Sharding allows for horizontal scaling.

### Sharding vs. replication

- Sharding: data is split across nodes (each shard holds a subset).
- Replication: the same data is copied across nodes (primary + secondaries).

## Mongo — schema design

There are no hard rules — unlike in SQL databases. But some guidelines:

1. Start with an E/R diagram.
2. Follow with an actual design implementation in Mongo.
3. Exemplify how documents can look — with some sample data.

### Actual design

Possibilities:

- Embed data in the same collection, or
- Links between data (referencing)

Optimize for fewer roundtrips:

- What data belongs together?
- What data is being used together?
- What data is being updated together?

### Embedded data

Embedded data is data nested in another object inside the same collection. When to embed?

- There is a contained relationship (e.g. motor in car).
- There is a one-to-few relationship (1 person, few phone numbers).
- Embedded data changes infrequently.
- Embedded data will not grow beyond some upper bound (e.g. number of email addresses or phone numbers for a person).
- Embedded data is often queried together.

### Referencing

There is no concept of foreign keys as in the SQL world:

- Links are considered weak links.
- Not enforced by the DB.
- Maintained by application(s).

Reference when:

- One-to-many relationships
- Many-to-many relationships
- Related data changes frequently
- Data can be unbounded

### Embedded example: 1-N

```json
// Books embedded in Publisher document (here O'Reilly)
{ "id": "1",
    "name": "O'Reilly",
    "books": [
        { "title": "Learning python" },
        { "title": "Jenkins 2 - up & running" },
        { "title": "Head First Kotlin" },
        { "title": "Mastering Ethereum" }
    ]
}
```

### Referenced example: 1-N

```json
// In publisher collection - here just arbitrary ids 1, 2, 3...
{
    "id": "1",
    "name": "O'Reilly",
    "books": [1, 2, 3, 12, 15, 19, 25, 26, 27, 49, 50]
}

// In Books collection
{ "id": "1", "name": "Learning python" }
{ "id": "2", "name": "Jenkins 2 - up & running" }
{ "id": "3", "name": "Head First Kotlin" }
{ "id": "50", "name": "Mastering Ethereum" }
```

### 1-N — some data embedded (alternative)

```json
// In publisher collection
{ "id": "1", "name": "O'Reilly" }

// In Books collection
{ "id": "1", "name": "Learning python", "publisher": "O'Reilly" }
{ "id": "2", "name": "Jenkins 2 - up & running", "publisher": "O'Reilly" }
{ "id": "3", "name": "Head First Kotlin", "publisher": "O'Reilly" }
{ "id": "50", "name": "Mastering Ethereum", "publisher": "O'Reilly" }
```

### N-N — referenced

```json
// In Author collection
{ "id": "a1", "name": "J.K. Rowling", "books": ["b1", "b7", "b8", "b9"] }
{ "id": "a2", "name": "Amanda Berlin", "books": ["b10", "b14"] }
{ "id": "a3", "name": "Lee Brotherston", "books": ["b10", "b11"] }

// In Book collection
{ "id": "b1", "name": "Harry Potter and the Philosophers Stone", "authors": ["a1"] }
{ "id": "b7", "name": "Harry Potter and the Goblet of Fire", "authors": ["a1"] }
{ "id": "b10", "name": "Defensive Security Handbook", "authors": ["a3", "a2"] }
```

What happens if we delete a book or an author? That needs to be handled in application code.

### N-N — embedding some data

Alternative: merge data together based on application usage. Example: author name with book — since author names don't change very often:

```json
// In Books collection
{
    "id": "b1",
    "title": "Harry Potter and the Philosophers Stone",
    "author_name": "J.K. Rowling",
    "authors": ["a1"]
}

// In Author collection
{ "id": "a1", "name": "J.K. Rowling", "books": ["b1", "b7", "b8", "b9"] }
```

## Working with Mongo

Ways of interacting with Mongo:

- Compass (UI)
- MongoDB shell (`mongosh` tool)
- From application code (like ASP.NET etc.)

### Inserting data (using mongo shell)

Use an existing database / create a new one:

```javascript
use myNewDB
```

Create a new collection named `myNewCollection1` and insert an element (`db` refers to the current database):

```javascript
db.myNewCollection1.insertOne( { x: 1 } )
```

Insert-many example (into the inventory collection):

```javascript
db.inventory.insertMany([
  { item: "journal", qty: 25, status: "A", size: { h: 14, w: 21, uom: "cm" }, tags: [ "blank", "red" ] },
  { item: "notebook", qty: 50, status: "A", size: { h: 8.5, w: 11, uom: "in" }, tags: [ "red", "blank" ] },
  { item: "paper", qty: 10, status: "D", size: { h: 8.5, w: 11, uom: "in" }, tags: [ "red", "blank", "plain" ] },
  { item: "planner", qty: 0, status: "D", size: { h: 22.85, w: 30, uom: "cm" }, tags: [ "blank", "red" ] },
  { item: "postcard", qty: 45, status: "A", size: { h: 10, w: 15.25, uom: "cm" }, tags: [ "blue" ] }
]);

// MongoDB adds an _id field with an ObjectId value if the field is not present in the document
```

### Selection examples

```javascript
db.inventory.find( {status: "D"})
// or {qty: 0, status: "D"}
// or {tags: "red"}
// or {size: { h: 14, w: 21, uom: "cm"}}
db.inventory.find({}) // returns all
```

What to return from the collection (called a projection):

```javascript
db.inventory.find( { }, { item: 1, status: 1 } )
```

`1` means include this field, `0` means exclude this field in output. The above returns all documents in inventory — but only with the item and status fields.

```javascript
db.inventory.find( { }, { status: 0 } ) // all fields except status
```

A projection cannot mix includes and excludes, except for `_id`.

### Sorting and limit

```javascript
db.inventory.find({})
  .sort({qty: 1}) // -1 descending
  .limit(20)
// sort ascending (1) by qty field - take first 20 documents

db.inventory.find({})
  .skip(20*page)
  .limit(20)
// skip first 20*page documents, then limit to 20

db.inventory.find({"item": /book$/})
// find documents from inventory where item matches the regex
```

Doc refs:
- https://www.mongodb.com/docs/manual/reference/operator/aggregation/skip/
- https://www.mongodb.com/docs/manual/reference/operator/aggregation/sort/

### Alternative selection methods

- `db.collection.find()`
- `db.collection.findOne()`
- `db.collection.aggregate()`
- `db.collection.countDocuments()`
- `db.collection.estimatedDocumentCount()`
- `db.collection.count()`
- `db.collection.distinct()`

More on collections: https://www.mongodb.com/docs/manual/reference/method/js-collection/

### _id — identifier

All documents in a collection must have a unique `_id`. If omitted, `_id` is generated by these rules:

- a 4-byte value representing the seconds since the Unix epoch,
- a 5-byte random value, and
- a 3-byte counter, starting with a random value.

```json
{ "_id": ObjectId("5099803df3f4948bd2f98391"),
  "name": { "first": "Alan", "last": "Turing" }
}
```

## Key points

- **Not Only SQL.**
- **Flexibility:** flexible schema designs, dynamic data structures.
- **Scalability:** horizontal scalability.
- **Variety:** different database types (document-based, key-value, columnar, and graph), each suitable for specific use cases.
- MongoDB is an example of a document database.
