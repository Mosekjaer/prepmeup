---
title: Mongo 2 — Query and Transactions
source: mongo2 - query and transactions.pdf
course_week: 8-9
topic: MongoDB
---

# Mongo 2: Query and Transactions

Agenda: querying (and projection/aggregation), schema changes, transactions/consistency.

## Querying in Mongo

Queries can be run from DB Compass (UI) or from C# via the MongoDB driver.

### Example data used

```csharp
public class Book {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string BookName { get; set; }

    [BsonElement("Price")]
    public decimal Price { get; set; }

    [BsonElement("Category")]
    public string Category { get; set; }

    [BsonElement("Formats")]
    public String[] Formats { get; set; }
    // and more...
}
```

### MongoDB driver

Install the MongoDB driver via NuGet in your IDE (e.g. Rider, Visual Studio).

### Querying using the MongoDB driver

```csharp
var client = new MongoClient(connectionString);
var database = client.GetDatabase(bookStoreDB);
IMongoCollection<Book> books = database.GetCollection<Book>("books");

books.Find(book => true).ToList(); // get all books
books.Find<Book>(book => book.Id == id).FirstOrDefault(); // specific id
```

### Projection — only return specific data

```csharp
public IEnumerable<string> GetFormats(string id) {
    var projection = Builders<Book>.Projection.Include(b => b.Formats); // only get the formats
    var bson = _books.Find<Book>(book => book.Id == id).Project(projection).FirstOrDefault();
    var array = bson.GetElement("Formats").Value.AsBsonArray;
    return array.Select(str => str.AsString);
}
```

Reference: https://www.mongodb.com/docs/manual/tutorial/project-fields-from-query-results/

### Aggregation

- Works like a pipeline.
- Consists of a set of stages: Project, Sort, Match, Group, Limit, ...

```csharp
var results = db.GetCollection<ZipEntry>
    .Aggregate()
    .Group(x => x.State, g =>
        new { State = g.Key, TotalPopulation = g.Sum(x => x.Population) })
    .Match(x => x.TotalPopulation > 20000)
    .ToList();   // g.Key is the ID of the group - in this case State
```

This should result in the following being sent to the server (what you would input in Compass):

```javascript
[{ $group : { _id : '$state', TotalPopulation: { $sum : '$pop' } } },
 { $match : { TotalPopulation : { $gt : 20000 } } }]
```

More examples with zip codes: https://www.mongodb.com/docs/manual/tutorial/aggregation-zip-code-data-set/
Group doc: https://www.mongodb.com/docs/manual/reference/operator/aggregation/group/

### Other CRUD operations

```csharp
// books = database.GetCollection<Book>("books");
public Book Create(Book book)
{
    _books.InsertOne(book);
    return book;
}

public void Update(string id, Book bookIn)
{
    _books.ReplaceOne(book => book.Id == id, bookIn);
}

public void Remove(Book bookIn) // based on id
{
    _books.DeleteOne(book => book.Id == bookIn.Id);
}
```

## Using LINQ with Mongo (the easy way to query)

### Data examples

```csharp
public class Person {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name = "";

    [BsonElement("Adress")]
    public string Address { get; set; } = "";

    [BsonElement("Owns")]
    public List<String> Owns { get; set; } = new List<String>();
}
```

```csharp
[BsonIgnoreExtraElements]
public class Pet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name = "";

    [BsonElement("Years")]
    public int Years = 0;

    [BsonElement("Race")]
    public string Race = "";

    [BsonElement("Weight")]
    public double Weight = 0.0;

    [BsonElement("Owner")]
    public String Owner = "";
}
```

### Example queries using LINQ

```csharp
public List<Person> GetAllPersons()
{
    var query = from p in persons.AsQueryable()
                select p;
    Console.WriteLine("persons found: " + query.ToList().Count);
    return query.ToList();
}
```

```csharp
public List<Pet> GetPetsAboveWeight(int weight)
{
    var query = from pet in pets.AsQueryable()
                where pet.Weight >= weight
                select pet;
    return query.ToList();
}
```

### Example query — join

```csharp
var innerJoinResult = person.AsQueryable().Join(
    pets.AsQueryable(),          // inner join A and B
    person => person.Name,       // from each itemA take the Name
    pet => pet.Owner,            // from each itemB take the Owner
    (person, pet) => new Pet()   // when they match make a new object
    {                            // where you only select the properties you want
        Name = pet.Name,
        Owner = person.Name,
        Race = pet.Race,
        Years = pet.Years,
        Weight = pet.Weight
    });

return innerJoinResult.ToList();
```

Note: you can return new types — using `new YourType() { ... }` — or anonymous types just using `new { ... }`, and then mix and match from both join collections.

### Example query — method syntax with SelectMany

```csharp
public List<PetEmbedded> GetAllPets()
{
    var query = personspets.AsQueryable().SelectMany(p => p.Owns);
    return query.ToList();
}

// Notice use of SelectMany instead of Select...
// SelectMany will flatten multiple lists to one list
```

`SelectMany` produces one flat list:

```json
[
    { "Name": "kingkong", "Years": 10, "Race": "gorilla", "Weight": 300 },
    { "Name": "Mickey", "Years": 10, "Race": "mouse", "Weight": 2 },
    { "Name": "Donald", "Years": 5, "Race": "Duck", "Weight": 4 }
]
```

where `Select` would produce a list of lists:

```json
[
    [ { "Name": "kingkong", "Years": 10, "Race": "gorilla", "Weight": 300 } ],
    [ { "Name": "Mickey", "Years": 10, "Race": "mouse", "Weight": 2 },
      { "Name": "Donald", "Years": 5, "Race": "Duck", "Weight": 4 } ]
]
```

### Useful references

- Many query examples from C#: https://www.mongodb.com/docs/manual/tutorial/query-documents/
- Using LINQ with Mongo: https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/linq/#overview
- Mapping classes to data with C#: https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/serialization/class-mapping/

## Handling schema changes

In NoSQL this is often done in application code — updating/altering entities when read.

Algorithm:

1. Read data.
2. If data is of an old version:
   - update data to the new version in the application,
   - save the new data to the DB.

Note: in relational databases, schema changes are handled with migrations instead.

### Example: change "age" to "birthday" field

1. Server reads the old document from the database:

```json
{
    "_id": ObjectId("1234567890"),
    "username": "john_doe",
    "email": "john@example.com",
    "age": 30
}
```

2. The application detects the old `age` field, computes `birthday`, removes `age`, and writes the updated document back:

```json
{
    "_id": ObjectId("1234567890"),
    "username": "john_doe",
    "email": "john@example.com",
    "birthday": "01/01/1990"
}
```

### Update schema in C#

```csharp
class Model : ISupportInitialize
{
    [BsonExtraElements] // maps extra elements not in model
    public IDictionary<string, object> ExtraElements { get; set; }

    public void BeginInit() { }

    public void EndInit()
    {
        object oldValue;
        if (!ExtraElements.TryGetValue("OldField", out oldValue))
        {
            return; // no OldField to update in data.
        }
        var value = (string)oldValue;
        ExtraElements.Remove("OldField");
        // Set new field/values in document if needed.
    }
}
```

## Consistency / Transactions

### Why transactions — examples

- Deletion in a 1-N relationship
- Adjusting quantity of an inline object
- Keeping consistency

In a relational system:

```sql
BEGIN TRANSACTION;
DELETE FROM orders WHERE id='11223';
DELETE FROM order_items WHERE order_id='11223';
COMMIT;
```

Or with FOREIGN KEY plus ON DELETE CASCADE.

### Removing data in a 1-N relationship (Mongo)

Option 1/2 — delete from both collections (order then order_items, or the reverse):

```javascript
db.orders.remove({'_id': '1123'})
db.order_items.remove({'order_id': '11223'})
```

Option 3 — or embed order_items in the order:

```json
{
    "_id": "11223",
    "items": [
        { "sku": "...", ... },
        { "sku": "...", ... }
    ]
}
```

### Potential issues with embedding

```json
{ "_id": "11223", "total": 500.94,
    "items": [
        { "sku": "123", "price": 55.11, "qty": 2 },
        { "sku": "...", ... }
    ]
}
```

Reading and changing `qty` in memory introduces a race condition. Use `db.collection.update()` to update multiple values in the document atomically — you still need to check that no changes have been made by another process:

```javascript
db.orders.update(
    { '_id': order_id, 'items.sku': sku },
    { '$inc': { 'total': total_update, 'items.$.qty': qty } }
)
```

### Consistency between documents

1. Update each document with `db.collection.update()` — an exception between updates can lead to missing data (in a bank?).
2. Emulate transactions in the data model:
   - Create a "transaction" collection.
   - A transaction can be in 'new', 'committed' or 'rollback' state.
   - Clean up accordingly.

Or use the sessions and transactions API (new).

### Sessions and transactions

The recommended way of using a session:

```csharp
var sessionOptions = new ClientSessionOptions { ... };
using (var session = client.StartSession(sessionOptions, cancellationToken))
{
    // execute some operations passing the session as an argument to each operation
}
```

With a transaction:

```csharp
using (var session = client.StartSession())
{
    session.StartTransaction();
    // execute operations using the session
    session.CommitTransaction(); // if an exception is thrown before reaching here the transaction is not committed
}
```

### Eventual consistency

- Accept that all data is not always up to date (across documents) — as long as it will be at some point.
- This is a common strategy in the NoSQL world.
- Use cases: fine for a chat application; not for a bank.
