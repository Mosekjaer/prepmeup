---
title: Working with data
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 4
---

# 4. Working with data

**This chapter covers**

- Understanding the core concepts of a database management
- system (DBMS)
- Introducing Entity Framework Core (EF Core)
- Building a data model and binding it to entity classes
- Deploying a SQL Server database using on-premise and cloud
- hosting models
- Setting up the database context and structure using EF Core

Up to this point, we have been working on our web API using sample data created on demand by a static procedure. Such an approach was certainly useful, as it allowed us to focus on the fundamental concepts of a REST architecture while keeping our source code as simple as possible. In this chapter, we’re going to get rid of that “fake” data set and replace it with a proper data source managed by means of a database.

In the first section, we’ll briefly review the key elements and distinctive properties of a database management system (DBMS), as well as the benefits of storing our data there. Right after that, we’ll see how we can interact with that system using Entity Framework Core (EF Core), a lightweight, extensible data access technology that allows us to work with most database services using .NET objects. More precisely, we’ll learn how to use the two main development approaches provided by EF Core to generate a data model from an existing database schema (database-first) or create a data model manually and generate the database schema from that (code-first), providing a concrete scenario for both.

To achieve these goals, we will also need to provide ourselves with a real DBMS. For this reason, we will spend some valuable time analyzing some of the many alternatives available today: installing a database server on our development machine (on-premise) or relying on a cloud-hosted DBMS service using one of the many cloud computing providers available (such as Microsoft Azure).

As soon as we have a database up and running, we’ll learn how to improve our web API project to retrieve and persist relational data by using the object-relational mapping (ORM) capabilities provided by EF Core.

Database schema and data model Before continuing, it might be useful to clarify the meaning of some relevant terms that we’ll be using throughout this book, because they are often used in a similar, sometimes interchangeable way.

We’ll use database schema for the database structure, intended to be the sum of all tables, collections, fields, relationships, views, indexes, constraints, and the like, and data model (or model) for the abstraction EF Core uses to interact with that structure. This means that the database schema is a logical representation of the database, whereas the data model is made of several C# classes working together, using a standardized set of conventions and rules. These distinctions will help us better understand the differences of the various operational contexts that we’ll work with in this chapter and the following chapters.

## 4.1 Choosing a database

A database management system is a software program specifically designed for creating and managing large sets of structured data. Those data sets are generally called databases. For the sake of simplicity, I’ll take for granted that you already know the main advantages of using a DBMS to manage a large quantity of information: improved efficiency; access versatility; data categorization; normalization capabilities; support for atomicity, consistency, isolation, and durability (ACID) transactions; and several undeniable benefits in terms of data security coming from the increased confidentiality, integrity, and availability that these solutions natively provide.

NOTE Confidentiality, integrity, and availability are the three pillars of the CIA triad, a widely used security model that summarizes the key properties that any organization should consider before adopting a new technology, improving the existing IT infrastructure, or developing a project.

Since chapter 1, we’ve taken for granted the use of a database to store all our board-game-related data. The reason for that choice is quite simple: we not only want to benefit from all the advantages described earlier, but also aim to reproduce a suitable situation for developing a web API. Having to deal with a DBMS is likely to be the most common task for any backend web developer for years to come.

In our scenario, however, we are playing the role not only of software developers, but also of high-level IT infrastructure architects. We need to do something before starting to code: choose which DBMS to use for storing and delivering our data.

The following sections briefly review some alternative approaches we can follow, as well as their pros and cons. For the sake of simplicity, our analysis will be limited to the fundamental aspects of each option, as well as the reasons that will determine our decisions.

### 4.1.1 Comparing SQL and NoSQL

The first choice we’re called on to make concerns the type of database we want to adopt. The two most popular choices today are SQL-type databases, also known as relational database management systems (RDBMS) and NoSQL-type (or nonrelational) databases.

DEFINITION Conversely from what we might think when looking at the acronym, NoSQL doesn’t stand for No SQL; it stands for Not Only SQL. Most NoSQL databases support variants of Structured Query Language (SQL), even if their data-retrieval API typically relies on different (JSON-like) syntax. Because SQL is typically used with relational databases, however, this term is conventionally adopted in reference only to relational databases, whereas NoSQL defines all non-relational approaches. For the sake of simplicity, we’ll adopt these conventions throughout this book.

The main difference between SQL and NoSQL databases is their data storage model, which also affects the data-retrieval techniques and, eventually, their scaling capabilities and usage scenarios. The following sections shed some light on these aspects.

Data storage model Without delving too much into the characteristics of each individual product, we can summarize the data storage model concept as follows: In SQL databases—The data is modeled by using tables with a fixed schema of rows and columns, with each table representing a single record type: the Users table, the Roles table, the BoardGames table, the Comments table, and so on. In NoSQL databases—The data is modeled by using collections of documents with a flexible schema of key/value pairs, with each document typically representing a record type together with all its related info: the Users documents (including the roles, if any), the Boardgames documents (including the comments, if any), and so on.

We immediately see that the two approaches handle relationships between entities in different ways. In SQL databases, the relationships between table records are typically implemented by means of primary keys that univocally identify each record of any given type, which can be added in the tables that require such references and then used to retrieve them. In our concrete scenario, for example, the Comments table would likely require the BoardGameID column to reference each comment to a given unique board game. Such logic will produce a well-structured, highly normalized, and (often) rather complex database schema made of fixed tables.

In NoSQL databases, most of the parent-child relationships between entities are embedded in the parent document, resulting in fewer collections. Key-based relationships are still used to handle entities that need to be referenced in multiple documents (a typical example being the users collection), but for the most part, the database schema doesn’t rely on them. This approach favors a loosely structured, denormalized, and (often) rather simple database schema made by collections of flexible documents.

The differences don’t stop at the relationship level. Because we must deal with fixed tables in SQL databases, we’re forced to determine and declare each table’s schema before inserting data. NoSQL’s collections don’t have this requirement because they don’t require their documents to have the same schema.

To better understand these concepts, take a look at a few figures. The first one (figure 4.1) depicts a possible data schema for our concrete scenario built with a table-oriented, relational, SQL database in mind: a BoardGames table and two related tables (Domains and Mechanics) linked to the main table by means of many-to- many relationships through two junction tables.
Figure 4.1 MyBGList database schema diagram for a table- oriented SQL database

Figure 4.2 shows a database schema that can host the same data by using a document-oriented, NoSQL alternative approach: a single boardGames document collection with two embedded collections of mechanic and domain documents.

NOTE In figure 4.2 (and its preceding text) we wrote the document and property names using camelCase because it’s the preferred naming convention for most NoSQL database engines.
Figure 4.2 MyBGList database schema diagram for a document- oriented NoSQL database

Both models are rather easy to understand at first glance because we’re working with a simple, demonstrative data structure. But we can easily see that if we had to deal with many additional “child” entities, the complexity levels of the two models would likely rise at different paces. The SQL schema would likely require additional tables (and junction tables), as well as an increasing number of JOINs for retrieving their relevant info, thus requiring more development effort.

The overall complexity of the database schema (and data retrieval queries) is only one of the many factors that we should consider when choosing between a SQL and a NoSQL approach. Another huge factor is their scaling capabilities, as we’ll see in the next section.

Scaling In general terms, we can say that SQL databases are designed to run on a single server because they need to maintain the integrity of their tables, indexes, and constraints. They are designed to scale vertically by increasing their server’s size, not to scale horizontally following a distributed computing approach; even the clustering techniques introduced to give them horizontal scaling capabilities are often subject to consistency problems, performance bottlenecks, and other drawbacks, not to mention added complexity.

Conversely, NoSQL databases are designed for scaling horizontally on distributed systems, which means having multiple servers working together and sharing the workload. This design is probably their most important advantage over their SQL counterparts, because it makes them better suited to host huge amounts of data, more reliable, and more secure. As long as we can count on multiple NoSQL nodes, we won’t have a single point of failure, thus ensuring a higher availability level.

DEFINITION Vertical scaling, also known as scale-up, refers to increasing the processing power of a single node (database server). Horizontal scaling, also known as scale-out, is based on adding nodes to form a cluster; after the cluster has been created, it can be scaled by adding or removing nodes to accommodate the workload requirements.

The huge advantage of NoSQL’s horizontal scaling, however, is reduced by the advent of several modern cloud-based SQL (or hybrid) DBMS services, which greatly reduce the end-user costs of vertical scaling. Moreover, those solutions often include many clustering and redundancy techniques that effectively mitigate the risk of a single point of failure.

Support Another important aspect to consider is the level of support for the development stack we’ve chosen to adopt. SQL databases have a strong edge because they have a decades-long presence in the market. Most NoSQL solutions are still behind in terms of available drivers, accessors, connectors, tooling, and cross-platform support— at least when taking the .NET ecosystem into account. Furthermore, most NoSQL databases use different APIs with no shared standards, such as the ones in relational databases (such as SQL syntax)—a fact that has slowed the widespread adoption of these technologies.

Usage scenarios All in all, we can say that the intrinsic characteristics of SQL databases make them ideal for hosting a set of normalized data, when all the record types are supposed to have the same structure, and most of them need to be referenced several times and in multiple places. When we’re dealing with this kind of data, most of the advantages of the NoSQL approach likely won’t come into play.

Conversely, whenever we have a huge amount of data, or when we’re forced to handle records that require a mutable structure and several parent-child relationships, the flexibility and scaling capabilities provided by a NoSQL database often make it a suitable choice (assuming that our development stack supports it).

### 4.1.2 Making a choice

Now that we have all the required info, let’s put on the clothes of our MyBGList IT team and choose between the two approaches. Suppose that our club is handling the board-game data by using a comma-separated-values (CSV) file containing all the relevant fields (ID, name, year published, and so on), which we need to import within a DBMS to enable our web API to interact with it. We already know the data structure we’re dealing with, because we saw it in the two database schema diagrams, which can be considered a couple of proofs of concept we made before making the decision. Should we go with SQL or NoSQL? Here’s a list of arguments that could help us make a reasoned choice:

By looking at the data, we can say that we are dealing—and will always have to deal—with a rather small data set. We can reasonably expect that the number of board games won’t exceed a few thousand over the whole lifetime of our web API. Considering our overall architecture, performance won’t likely be a problem. We will serve mostly small sets of data to a limited number of peers, and we’ll definitely make extensive use of caching and paging techniques to reduce the server load. With such premises, horizontal scaling won’t be required any time soon. All our record types are supposed to have the same data structure. Some of them will likely change in the future, but we will probably always want to keep it consistent between all the record types available. For example, sooner or later we might want to add additional details to the authors entities (title, job, address, and so on), but as soon as we do that we would want these new columns to be available for all authors’ records, including the existing ones. Last but not least, we need to consider the web development stack we’ve chosen to adopt. As a matter of fact, ASP.NET Core provides support for both SQL and NoSQL databases. But although drivers and libraries for the most popular RDBMS software products (SQL Server, MySQL, and more) have been shipped as built-in features for decades, the NoSQL alternative is currently supported only by Azure Cosmos DB (Microsoft’s proprietary NoSQL database hosted on Azure) or third-party providers (such as those for MongoDB and RavenDB). The same goes for EF Core, the most popular ASP.NET data access technology, which currently doesn’t provide native NoSQL support. Even the EF Core Azure Cosmos DB provider works only with the SQL API.

For all these reasons, our final choice will eventually be the SQL (relational) database approach, which seems to be the most suitable option for our concrete scenario. This decision comes with the added value of allowing us to use EF Core (more about which later in this chapter).

Next, we need to choose a specific RDBMS product. For the sake of simplicity, we won’t spend too much time comparing the various available options offered by the market. Let’s start by narrowing the list to the main RDBMSes supported by EF Core, together with their respective owners and the maintainer/vendor of the most popular database provider required to support each of them (table 4.1). A comprehensive list of all the available DBMS and database providers is available at https://docs.microsoft.com/en-us/ef/core/providers.

Table   4.1   DBMS,        owners,           and   database          providers
maintainers/vendors

   DBMS product           Owner/developer           Database provider
                                                    maintainer/vendor
SQL Server              Microsoft                  EF Core Project
SQLite                  Dwayne Richard Hipp        EF Core Project
Cosmos DB (SQL API)     Microsoft                  EF Core Project
PostgreSQL              PostgreSQL GDG             Npgsql Development
                                                   Team
MySQL                   Oracle Corporation         Pomelo Foundation
                                                   Project
Oracle DB               Oracle Corporation         Oracle Corporation

As we can see, each DBMS product is supported by the means of a database provider created and maintained by either the EF Core project or a third-party development team. The providers shown in table 4.1 provide a good level of support and are updated continuously by their maintainer to match the latest versions of EF Core, which makes them ideal for most production scenarios.

Taking all those factors into account, because we want to explore most of the features offered by ASP.NET Core and EF Core, a reasonable choice for demonstrative purposes would be Microsoft SQL Server, which will also allow us to experience both on-premise and cloud-based hosting models. We’re going to pick that DBMS.

NOTE Because EF Core provides a common interface for all those relational database engines, we’ll even be able to reconsider our decision and switch to a different product during our development phase without significant problems.

## 4.2 Creating the database

Now that we’ve chosen our DBMS product, we can create our SQL database and populate it with our board-game data. Because we want to follow our concrete scenario, the first thing we need to do is provide ourselves with a suitable data set (such as an XML or CSV file) that we can use to emulate our initial condition: a list of board games with their most relevant info (name, year published, mechanics, domains, and so on). When we have that data set, we’ll be able to perform the following tasks:

1. Install a local SQL Server instance on our development machine.
2. Create the MyBGList database using either a graphical user interface (GUI) tool or raw SQL queries.
3. Set up and configure EF Core to access the MyBGList database.
4. Use EF Core’s database-first feature to create our entities.
5. Populate the database by using EF Core directly from our web API.

### 4.2.1 Obtaining the CSV file

For the sake of simplicity, instead of creating a CSV file from scratch, we’re going to get one from Kaggle, an online community of data scientists that hosts several publicly accessible data sets for education and training purposes. We’ll use Larxel’s Board Games, a CSV file hosting a data set of approximately 20,000 board games scraped from the BoardGamesGeek website. Download the compressed data set and then unpack it to extract the bgg_dataset.csv file.

Larxel’s board game credits and references The dataset is published under Creative Commons Attribution 4.0 International license, which allows us to use it by giving credit to the authors.

Dataset URL: https://www.kaggle.com/andrewmvd/board-games

Maintainer: Larxel, https://www.kaggle.com/andrewmvd

Citation: Dilini Samarasinghe, July 5, 2021, “BoardGameGeek Dataset on Board Games,” IEEE Dataport, doi: https://dx.doi.org/10.21227/9g61-bs59

License: CC BY 4.0, https://creativecommons.org/licenses/by/4.0
TIP Kaggle requires (free) registration to download the file: if you’re not willing to register, use the /Data/bgg_dataset.csv file provided in the book’s GitHub repository starting from chapter 4.

Before moving on to the next step, it could be useful to open the file using a text editor (or a GUI tool that can handle CSV format, such as Microsoft Excel) and take a good look at it to ensure that it contains the field we need. Figure 4.3 shows an excerpt of the first ten lines and eight columns of the file. As we can see, the file contains all the columns we need.

Figure 4.3 Excerpt of the bgg_dataset.csv file

NOTE The actual CSV file content may vary because it’s updated frequently by its maintainer. The version used in this book is version 2.0 (the latest at this writing) and can be found in the GitHub repository for this chapter for reference purposes.

### 4.2.2 Installing SQL Server

We chose to install a local instance of SQL Server instead of using a cloud-based solution (such as the SQL database provided by Azure) for practical reasons:

SQL Server provides two free editions that we can use: Developer, which comes with the full functionality of the commercial editions but can be used only for testing, educational, and teaching purposes, and Express, which has several size and performance limitations but can be used in production for small-scale applications. We can use SQL Server Management Studio (SSMS) to its full extent, as most of its GUI-related features—such as the visual table designer and the modeling tools—might not work when we’re dealing with a cloud-hosted SQL database (depending on the product).

TIP As an alternative to installing a local instance on their development machines, Docker users can consider one of the many SQL Server containers available on DockerHub. Here’s the link to the official SQL Server Docker image, released and maintained by Microsoft: https://hub.docker.com/_/microsoft-mssql-server.

Regardless of the choice we make, there’s no need to worry; we’ll always be able to migrate our on-premise SQL Server database to a cloud-hosted Azure SQL database service whenever we want to, with minimal effort.

SQL Server Express and LocalDB The SQL Server Express installation instructions are detailed in the appendix. In case we don’t want to install a local SQL server instance on our development machine, we could take an alternative route by using SQL Server Express LocalDB—a lightweight SQL instance provided by Visual Studio that offers the same T-SQL language, programming surface, and client-side providers as the regular SQL Server Express without the need to install or configure (almost) anything. Such a solution can be great during development, because it immediately gives us a database with no additional work. But it comes with a huge set of limitations that make it unsuitable for production use, which is one of the goals we want to achieve with this book. For that reason, I suggest avoiding this “convenient” shortcut and sticking with a regular SQL Server edition instead.

When SQL Server installation is complete, we can connect to the newly installed instance by using one of the following free management tools:

SQL Server Management Studio (SSMS) Azure Data Studio (ADS)

Both software applications allow us to connect to a SQL Server database and manage its contents (tables, users, agents, and so on), as well as perform queries and scripts. SSMS is available only for Windows and has a lot of features that can be used from the GUI. ADS has a portable, multiplatform, lightweight design and provides a rather minimal interface that allows us to perform only SQL queries (at least, for now).

TIP Starting with version 18.7, SSMS includes ADS as an internal module, accessible from the Tools menu.

For this book, we’re going to use SSMS, because it provides a more graceful learning curve for SQL novices. But ADS might be a great alternative for two groups of people: seasoned SQL developers who prefer to avoid the GUI-based approach and perform everything through SQL queries and scripts, and Linux users, because both SQL Server and ADS can be installed in Linux.

### 4.2.3 Installing SSMS or ADS

SSMS can be installed through the SQL Server installation wizard’s additional components (SQL Server Management Tools section) or downloaded as a standalone package at http://mng.bz/pdKw. To use ADS, download it at http://mng.bz/OpBa. Both tools are easy to install and set up via the installation wizard and require no specific settings.

NOTE In the following sections, we’re going to use SSMS to connect to SQL Server and create the database structure via its unique UI- based approach. All the SQL queries required to create the MyBGList database tables can be found in the GitHub repository for this chapter.

### 4.2.4 Adding a new database

Right after completing the installation process, launch SSMS. The Connect to Server pop-up window should appear. Because we’re connecting to a locally hosted SQL Server instance, we’re going to use the following settings:

Server type—Database engine Server name—<MACHINENAME>\SQLEXPRESS (or .\SQLEXPRESS ; a single dot can be used as an alias for the local machine name) Authentication—Windows authentication or SQL Server authentication (depending on whether you chose Windows Authentication or Mixed Mode during the installation phase) Username and password—Leave them blank for Windows authentication, or use the sa account’s credentials for Mixed Mode/SQL Server authentication

Next, click the Connect button to establish a connection with the SQL Server instance. From here, we can finally create our database by right-clicking the Databases folder in the left tree view and then selecting the New Database option (figure 4.4).

Figure 4.4 Adding a new SQL database in SSMS The New Database modal window opens, allowing us to give our new database a name and configure its core settings. For the sake of simplicity, we’ll give it the MyBGList name and keep the default settings, as shown in figure 4.5.

Figure 4.5 Creating the MyBGList database in SSMS Click the OK button to create the database, which will be added to the Databases folders as a new child node, with several subfolders: Diagrams, Tables, Views, and so on.

Introducing logins and users Now that we have the MyBGList database, we need to create a set of credentials (username and password) that can be put in a connection string to make our web API access it. To do that, we need to perform two tasks:

Add a login to authenticate into our SQL Server local instance. Add a user to authorize the login into our MyBGList database.

The individual logins-and-users approach ensures a good security posture, because it gives us precise, granular control of who can access each database and to what extent. It’s important to understand that logins must be added to the SQL Server instance and therefore affect the whole instance, whereas users pertain only to a specific database—in our case, MyBGList. For that reason, the same login could be theoretically linked to different users. That said, in our specific scenario, we’ll create a login and a user with the same name and then link them.

Adding the SQL Server login Because the Login entry must be added at the instance level, we need to use SQL Server’s main Security folder—not the folder of the same name inside the MyBGList database. When we locate this folder, we right-click it and choose New Login from the contextual menu to access the Login—New modal window, shown in figure 4.6.

Figure 4.6 Adding the MyBGList login in SSMS As we can see, the page is split into multiple pages. Figure 4.6 shows the General page, where we need to define the login name, the authentication type, the password, and so on. Here’s a list of the settings that we’ll take for granted throughout the rest of the book:

Login Name—MyBGList

Authentication—SQL Server Authentication Password—MyS3cretP4$$ (or any other password you want to use) Enforce Password Policy—Enabled (unless you want to choose a weak password) Enforce Password Expiration—Disabled (unless you want the password to expire) Default Database—MyBGList

All the other settings can be left at their defaults.

WARNING If we chose Windows authentication during the SQL Server installation phase (see the appendix), we need to enable the authentication mixed mode at the SQL Server instance level; otherwise, we won’t be able to log in.

Adding the MyBGList database user Next, we switch to the User Mapping page. We can use this page as a shortcut to create an individual user for each existing database and have that user linked automatically to the login entry we’re about to add. That’s great, because it’s precisely what we need to do now. Select the MyBGList database in the right panel, create the MyBGList user, and select the db_owner role in the Database Role Membership panel, as shown in figure 4.7. The db_owner role is important, because it’s required to perform create, read, update, and delete (CRUD) operations as well as create tables.
Figure 4.7 Adding the MyBGList user and setting the role membership in SSMS

All the other settings can be left at their default values. Click the OK button to add a new MyBGList login to the SQL Server instance and a new (linked) MyBGList User to the MyBGList database at the same time.

WARNING As always, feel free to change the Login and User settings, if you like. Be sure to remember your changes later, when we’ll put them in a connection string to access our MyBGList database.

This section concludes the initial setup of our SQL database. Now we need to create the BoardGames, Domains, and Mechanics tables that will host our data, as well as the BoardGames_Domains and BoardGames_Mechanics junction tables to handle their many-to-many relationships.

## 4.3 EF Core

Now that we have a real data source accessible through a DBMS, it’s time to introduce the software component that we’ll be using to interact with it: EF Core, a set of technologies designed to help ASP.NET Core developers interact with a supported DBMS source in a standardized, structured way. This behavior is achieved through a set of ORM techniques that allow us to work with data at a higher abstraction level, using the C# programming language instead of having to write actual SQL queries.
NOTE The EF Core version we’re going to use in this book is the latest iteration of a project released more than a decade ago. The first version, called Entity Framework and included with .NET Framework 4.5 SP1, was released on August 11, 2008, and was followed by no fewer than 13 subsequent versions in 13 years. Not until version 4.1, which introduced Code First support, did the project establish itself in the ASP.NET developer communities, overcoming strong initial criticism due to many bugs, performance problems, and antipattern architectural choices. Since the introduction of EF Core 1.0, released under Apache License 2.0 and featuring a completely rewritten codebase as well as cross-platform compatibility, the component gained a lot of popularity and is now widely adopted for ASP.NET projects of all sizes.

### 4.3.1 Reasons to use an ORM

Before getting straight to the code, it could be wise to spend a couple of minutes answering the following question: do we really need an ORM? (We’ve chosen the SQL database approach, so why can’t we use standard SQL code?) The whole idea of using a programming language such as C# to interact with an RDBMS instead of its built-in API (SQL queries) may seem odd. But assuming that we choose a good ORM, we have a lot to gain by taking this approach. Among other things, we can

Reduce the development time and size of the code base Standardize our data retrieval strategies and thus reduce the number of mistakes Take advantage of several advanced RDBMS features out of the box (transactions, pooling, migrations, seeds, streams, security measures, and so on) without having to learn how to use them with bare SQL commands Minimize the number of string-manipulation methods, functions, and techniques required to write dynamic SQL commands programmatically

Consider the following SQL query:

UPDATE users SET notes = "this user has been disabled" WHERE id = 4;

If we want to assign the value set to the notes column dynamically by using C#, we could be tempted to write something like this:

var notes = "this user has been disabled";

// ... other code

$"UPDATE users SET notes = "{notes}" WHERE id = 4;";

This technique, however, will greatly increase the chance of a SQL injection unless we make sure that the notes variable content is escaped properly and/or completely under our control. To address this risk, we’d be forced to patch the dynamic query by using SQL parameters. When we use such a technique, the query gets executed through a system-stored procedure that separates the actual SQL commands from the array of parameters, keeping their values “isolated” from the execution context and eliminating the risk of injection. Here’s how we could write the preceding SQL query to adopt this approach: SqlDataAdapter myCommand = new SqlDataAdapter( "UPDATE users SET notes = @notes", conn); SQLParameter parm = myCommand.SelectCommand.Parameters.Add("@notes", SqlDbType.VarChar, 11); Parm.Value = notes;

Alternatively, we could implement other viable (or “not-so-viable”) countermeasures, such as creating a dedicated stored procedure for that task or escaping the notes variable manually. Inevitably, we’d end up putting such countermeasures in some helper class or method, in a desperate effort to centralize them—which would likely pose additional security risks in the future, because those workarounds might become outdated or less secure as time passes and new vulnerabilities are discovered. Here’s how we can write the same query with EF Core (assuming that we’ve configured it properly):

```csharp
var user = DbContext.Users.Where(u => u.Id == 4);
user.Notes = "this user has been disabled";
DbContext.SaveChanges();
```

We wrote some C# code here. The ORM will take care of everything, including the anti-injection countermeasures, and automatically generate the SQL required to interact with the database without our having to do (or know) anything about it.

NOTE Don’t get me wrong: SQL is a powerful language, and any developer who’s experienced enough to use it to its full extent will probably be able to use it quickly and effectively to write any kind of query without an ORM. This level of theoretical and practical SQL knowledge isn’t common among web developers, however. Most of them will have a much better learning curve if they’re given the chance to interact with data in the same language they’re using to write the backend code (C# in the preceding examples)—especially if they can count on a software component that handles most of the complex work automatically and transparently.

It’s important to understand that delegating the most complex SQL tasks to an ORM component doesn’t mean that software developers won’t need some SQL knowledge or that they don’t need to learn anything about it anymore. The purpose of an ORM is not to throw SQL out of the picture, but to free developers from the need to reinvent the wheel with SQL, giving them more time to focus on what really matters: the source code.

It’s also worth noting that most ORMs, including EF Core, even give the developers the chance to write queries to handle specific tasks, as well as analyze the ORM-generated queries. We’ll make extensive use of these features throughout this book. Besides convenience and security benefits, notable advantages of ORM include the following:

Seamless DBMS switch—Abstracting the database and its SQL syntax makes it easy to switch among the various supported DBMS products. Because we’re not writing the queries manually, everything should work out of the box without our having to do anything other than reconfigure the ORM and change the connection string. Query optimization—The queries that the ORM creates automatically are generally more optimized than those written by the average software developer, often resulting in a performance gain. But the outcome depends on the ORM implementation, the database structure, and the query type. Improved readability—ORM-based statements are typically much more readable than parametrized SQL queries, as well as more succinct and easier to maintain. Source control—The ORM-related source code (and all its history) will be tracked and versioned automatically by the source control system, together with the rest of the code. This isn’t always the case when we use stored procedures, which typically aren’t tracked and are often changed on the fly by a database administrator directly on the database server(s).

As with any abstraction mechanism, some inevitable tradeoffs accompany the benefits of using an ORM, including the following:

Performance problems—Using an ORM adds overhead to the whole system, which almost always results in a performance hit. Often, the query optimization benefit that we talked about earlier compensates for this drawback, but the actual effect depends on the given scenario. Knowledge gap—Adopting an ORM to address advanced (or less-advanced) SQL problems that we don’t want to solve by ourselves could eventually take away most of our interest in learning and studying SQL, thus making us weaker developers in that portion of the stack. Opaque data access layer—Because the ORM “hides” the SQL queries, software developers won’t have the chance to inspect low-level data retrieval techniques merely by looking at the codebase. The only way to analyze the actual queries would be to launch the project in Debug mode and inspect the ORM- accessible properties (assuming that they’re available) or have them logged somewhere. Additional work—Adopting an ORM and learning how to use it properly aren’t easy tasks and often take a great deal of effort for all developers involved—perhaps even more than getting used to the raw SQL syntax. Limited database support—Most ORMs, including EF Core, come with limited database support, which inevitably reduces our options when we make the decision to use it.

Despite these drawbacks, adopting an ORM in data-oriented applications is widely considered to be good practice and will greatly help us optimize the development of our RESTful web API in terms of time, source-code size, security, and reliability.

### 4.3.2 Setting up EF Core

Without further ado, let’s see how we can add EF Core to our existing code. Here’s a list of tasks that we need to complete to achieve this result:

1. Install EF Core and the dotnet-ef command-line interface (CLI) tools, using the .NET Core CLI.
2. Explore the EF Core data modeling approaches, code-first and database-first, and their corresponding migrations and scaffolding tools.
3. Create a data model, following the EF Core conventions and rules for each approach.
4. Review the data model, and adjust it to our needs.

Installing the EF Core packages and tools As always, the required NuGet packages can be installed within Visual Studio, using the NuGet Package Manager or the Package Manager Console, or from the command line, using the .NET Core CLI. For simplicity, this time we’ll use the .NET Core CLI, which requires us to open a command prompt, navigate to our project’s root folder, and type the following commands:

> dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.11 > dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.11

Right after that, let’s also install the dotnet-ef CLI tools, a set of command-line features that we can use to perform design-time development tasks. Because we plan to use the EF Core’s data modeling features, we’re going to need these tools soon enough. Without leaving the command line, type the following command:

> dotnet tool install --global dotnet-ef --version 6.0.11

The --global switch ensures that the tools will be installed globally, allowing us to use them for all projects. To limit their use to this project only, remove that switch before executing the command. Next, run the following command to ensure that the dotnet-ef CLI tools have been installed properly:

> dotnet ef

If we did everything correctly, we should see ASCII output containing the EF logo and the installed version of the CLI tools. Now we can move to our next task.

EF Core data modeling techniques The EF Core data model can be created by means of two approaches:

Code-first—Create the C# entity classes manually, and use them to generate the database schema, keeping it in sync with the source code by means of the EF CLI’s migrations tool. Database-first—Generate the C# entity classes by reverse engineering them from an existing database schema, and keep them in sync with the database structure by using the EF CLI’s scaffolding tool.

As we can see, the two techniques are based on different premises. When we use code-first, the EF Core data model is the “source of truth,” meaning that all the change management tasks are always made there and then “replicated” to the database by means of the migrations tool. That approach is great if we don’t have an existing database up and running, because it allows us to create the database schema from scratch and use C# to manage it without the need for specific SQL knowledge or database modeling tools. Conversely, database-first takes for granted that the source of truth is played by the database schema, so whenever we want to change our database’s structure, we have to use raw SQL queries or database modeling tools such as SSMS and then replicate those changes to our C# entity classes using the scaffolding tool. This approach is often preferable when we have an existing database, maybe inherited from an existing application that our ASP.NET Core project is meant to replace.

Taking our concrete scenario into account, we currently have an empty database without any schema. So adopting the code-first approach might be the most logical choice.

### 4.3.3 Creating the DbContext

Let’s start by creating a dedicated folder where we’ll put everything related to our EF Core data model. In Visual Studio’s Solution Explorer, right-click the MyBGList node, and create a new /Models/ folder in the project’s root.

The first class we need to create there is our application’s DbContext, which represents the operational context between ASP.NET Core and the database—in other words, a database session abstraction that allows us to perform CRUD operations programmatically by using our C# code.

The best thing we can do to get the most from our application’s DbContext class is to create it by following the EF Core conventions. We can easily do that by inheriting the DbContext base class, as shown in the following listing.

**Listing 4.1 ApplicationDbContext.cs file**

```csharp
using Microsoft.EntityFrameworkCore;

namespace MyBGList.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO: custom code here
        }
    }
}
```

This code is the boilerplate we’re going to use as a basis to create our data model. Both the constructor and the OnModelCreating method are required by the DbContext base class we’re inheriting from.

We placed a TODO comment inside the OnModelCreating method because that’s the place where we can configure our model by using the ModelBuilder API (also known as the Fluent API), one of the three configuration methods made available by EF Core. The other two supported methods are conventions and data annotations. Before starting to code, let’s take a minute to see how the three configuration methods work:

Fluent API—The most powerful data modeling configuration method because it takes precedence over the other two, overriding their rules when they collide. Data annotations—A set of attributes that can be used to add metadata to the various entity classes, allowing us to specify individual configuration settings for each. Data annotations override conventions and are overridden by Fluent API. Conventions—A set of naming standards hard-baked into EF Core that can be used to handle the mapping between the Model and the database schema automatically, without the need to use the Fluent API or data annotations.

Each configuration method can be used independently to obtain the same outcome. In this book, we’ll use the Fluent API for modelwide settings, data annotations for entity-specific settings, and conventions whenever we can. This approach will allow us to learn gradually how to use each technique without creating useless conflicts that would result in code bloat.

Our first entity It’s time to create our first entity. In EF Core, an entity is a C# class representing a business object—in other words, an abstraction of a database record type. For that reason, each entity class has a structure that closely matches a corresponding database table. That’s expected, because we’ll use those entities to perform all CRUD operations and even to create our database schema, because we’re committed to the code-first approach.

Let’s use this knowledge to create an entity that can represent our BoardGame database table. We already know the structure of this table because we have the CSV file and even a database schema diagram (figure 4.8) that we can use as references for the various fields/columns we need to include.
Figure 4.8 The MyBGList database schema diagram

The following listing shows the source code for our BoardGame entity class, which we can put in a new /Models/BoardGame.cs file.

**Listing 4.2 BoardGame.cs file**

```csharp
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models
{
    [Table("BoardGames")]       ❶
    public class BoardGame
    {
        [Key]                    ❷
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public int Year { get; set; }

        [Required]
        public int MinPlayers { get; set; }

        [Required]
        public int MaxPlayers { get; set; }

        [Required]
        public int PlayTime { get; set; }

        [Required]
        public int MinAge { get; set; }

        [Required]
        public int UsersRated { get; set; }

        [Required]
        [Precision(4, 2)]
        public decimal RatingAverage { get; set; }

        [Required]
        public int BGGRank { get; set; }

        [Required]
        [Precision(4, 2)]
        public decimal ComplexityAverage { get; set; }

        [Required]
        public int OwnedUsers { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
```

❶ Database table name
❷ Database table’s primary key
We did nothing special here. The class is mimicking the fields in the CSV file, because those fields are what we want to store in the database and retrieve with our web API. The only things missing are the Domains and Mechanics info, which will be stored in different database tables and therefore abstracted by means of their own EF Core entity types.

Before creating those classes, let’s review the various attributes that we’ve added to the BoardGame entity type by using the data annotations configuration method:

[Table("BoardGames")]—This attribute instructs EF Core to use the specified name for the database table related to this entity. Without it, the table name will be given by sticking to the EF Core conventions—that is, the class name will be used instead. [Key]—As its name implies, this attribute tells EF Core to set this field as the table’s primary key. [Required]—Again, the attribute’s name explains everything. These fields will be marked as required and won’t accept a null value. [MaxLength(200)]—This attribute can be applied to the string and byte[] properties to specify the maximum character (or byte) length for its value. The assigned size will also set the size of the corresponding database column. [Precision(4,2)]—This attribute configures the precision of the data for that given field. It’s used for mostly decimal types to define the number of allowed digits. The number before the comma defines the precision (the total number of digits in the value); the number after the comma represents the scale (the number of digits after the decimal point). By taking all that into account, we can see how our value of 4,2 indicates that we expect numbers with two digits before and two digits after the decimal point.

NOTE For additional info about data annotation attributes and a comprehensive list of them, read the official docs at http://mng.bz/Y6Da.

Now that we have a proper BoardGame entity class, we can delete the root-level BoardGame.cs file that we created in chapter 2; we don’t need it anymore. Then we need to add a reference to the MyBGList.Models namespace at the top of the BoardGamesController.cs file so that the controller will be able to find (and use) the new model instead of the deleted dummy class:

using MyBGList.Models;

Adding other entities Let’s create the Domain entity. The code for this class is much simpler. The Domain record type requires only a few fields: a unique Id and Name, plus the CreationDate and LastModifiedDate. Create a new /Models/Domain.cs file, and fill it with the code in the following listing.

**Listing 4.3 Domain.cs file**

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models
{
    [Table("Domains")]
    public class Domain
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
```

Nothing is new here. This entity is essentially a subset of the previous one. The same can be said of the Mechanic entity, which has the same fields as the previous one. The following listing shows the code that we can put in a new /Models/Mechanic.cs file.

**Listing 4.4 Mechanic.cs file**

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBGList.Models
{
    [Table("Mechanics")]
    public class Mechanic
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
    }
}
```

Now we have the three main entities that will populate our data source. We need to add the required junction entities to JOIN them.

Junction entities As I said earlier, we’re dealing with two many-to-many relationships here, because each Domain (and Mechanic) can be referenced to zero, one, or multiple BoardGames, and vice versa. Because we’ve opted for an RDBMS, we need to create two additional entities that EF Core will use to abstract (and create) the required junction tables. We’ll call them BoardGames_Domains and BoardGames_Mechanics, respectively, as we did with the database schema diagram (figure 4.8).

Let’s start with the BoardGames_Domains entity. Create a new /Models/BoardGames_ Domains.cs file, and fill its content with the source code in the following listing.

**Listing 4.5 BoardGames_Domains.cs file using System.ComponentModel.DataAnnotations;**

```csharp
namespace MyBGList.Models
{
    public class BoardGames_Domains
    {
        [Key]
        [Required]
        public int BoardGameId { get; set; }

          [Key]
          [Required]
          public int DomainId { get; set; }

          [Required]
          public DateTime CreatedDate { get; set; }
    }
}
```

This time, we didn’t use the [Table("<name>")] attribute. The conventions naming standards are good enough for this table.

The same approach is required for the BoardGames_Mechanics entity, which features the same fields. The following listing contains the source code for the new /Models/BoardGames_Mechanics.cs file that we need to create.

**Listing 4.6 BoardGames_Mechanics.cs file**

```csharp
using System.ComponentModel.DataAnnotations;

namespace MyBGList.Models
{
    public class BoardGames_Mechanics
    {
        [Key]
        [Required]
        public int BoardGameId { get; set; }

          [Key]
          [Required]
          public int MechanicId { get; set; }
         [Required]
         public DateTime CreatedDate { get; set; }
    }
}
```

As we can see by looking at this code, these two entity classes have two properties with the [key] attribute, meaning that we want their respective database tables to have a composite primary key. This approach is a common one used to deal with many-to-many junction tables, as each mapped relationship is almost always meant to be unique.

Composite primary keys are supported in EF Core, but they require an additional setting that only Fluent API support. Let’s add that setting to our code before proceeding. Open the ApplicationDbContext.cs file, and add the following code to the existing OnModelCreating method, replacing the TODO comment:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
```

modelBuilder.Entity<BoardGames_Domains>() .HasKey(i => new { i.BoardGameId, i.DomainId });

```csharp
        modelBuilder.Entity<BoardGames_Mechanics>()
            .HasKey(i => new { i.BoardGameId, i.MechanicId });
}
```

We can use the OnModelCreating method override to further configure and/or customize the model after all the default conventions defined in the entity types have been applied. Notice how we used the HasKey method to configure the composite primary key of the [BoardGames_Domains] and [BoardGames_Mechanics] tables.

Now that we’ve created these junction entities, a couple of questions arise. How can we tell EF Core to use the values that we’ll store, using them to JOIN the BoardGames records with Domains and Mechanics? More important, is there a way to create a direct reference to these relationships from the BoardGames entity? The following sections answer these questions, starting with the second one.

Navigation properties In EF Core, the term navigation property describes an entity property that directly references a single related entity or a collection of entities. To better understand this concept, consider the following two navigation properties, which we could add to the BoardGames_Domains entity class:

```csharp
public BoardGame? BoardGame { get; set; }
public Domain? Domain { get; set; }
```

Each of these (nullable) properties is intended to be a direct reference to one of the entities handled by the junction. This technique can be useful, allowing us to handle the JOIN between tables transparently by using standard properties. But if we want EF Core to fill these properties properly, we need to make our DbContext aware of the corresponding relationships. In other words, we need to configure it by defining some rules. Because these rules affect multiple entities, we’ll use the Fluent API to set them up. First, however, we need to add all the required navigation properties to our entities.

Let’s start by adding the preceding properties to the BoardGames_Domains entity class. Then add the following two properties to the BoardGames_Mechanics entity class:

```csharp
public BoardGame? BoardGame { get; set; }
public Mechanic? Mechanic { get; set; }
```

Now we need to reciprocate these references in the main entity classes. Open the /Models/BoardGame.cs file, and add the following properties after the last line of code:

```csharp
public ICollection<BoardGames_Domains>? BoardGames_Domains { get; set; }
public ICollection<BoardGames_Mechanics>? BoardGames_Mechanics { get; set; }
```

Do the same in the /Models/Domain.cs file

public ICollection<BoardGames_Domains>? BoardGames_Domains { get; set; }

and in the /Models/Mechanic.cs file:

public ICollection<BoardGames_Mechanics>? BoardGames_Mechanics { get; set; }

Now that all entities have their navigation properties applied, we can use the Fluent API to configure them. Before proceeding with the code, let’s briefly recap what we need to do: Define a one-to-many relationship between the BoardGame entity and BoardGames_Domains entity (using the BoardGameId foreign key), and define another one-to-many relationship between the Domain entity and BoardGames_Domains entity (using the DomainId foreign key). Define a one-to-many relationship between the BoardGame entity and BoardGames_Mechanics entity (using the BoardGameId foreign key), and define another one-to-many relationship between the Mechanic entity and BoardGames_ Mechanics entity (using the MechanicId foreign key). Set up a cascade behavior for all these relationships so that all the junction records will be deleted when one of the related main records is deleted.

Relational database concepts and references This plan, as well as this part of the chapter, takes for granted that you have the required RDBMS knowledge to handle many-to-many relationships and that you understand concepts such as foreign keys, constraints, and cascading rules. In case of problems, the following links can help you understand these topics better:

http://mng.bz/GReJ http://mng.bz/zmZA Let’s put the plan into practice. Open the /Models/ApplicationDbContext.cs file, and put the following lines (marked in bold) in the OnModelCreating method, below the HasKey methods that we added earlier to configure the composite primary keys:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
```

modelBuilder.Entity<BoardGames_Domains>() .HasKey(i => new { i.BoardGameId, i.DomainId });

```csharp
    modelBuilder.Entity<BoardGames_Domains>()
        .HasOne(x => x.BoardGame)
        .WithMany(y => y.BoardGames_Domains)
        .HasForeignKey(f => f.BoardGameId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
```

```csharp
    modelBuilder.Entity<BoardGames_Domains>()
        .HasOne(o => o.Domain)
        .WithMany(m => m.BoardGames_Domains)
        .HasForeignKey(f => f.DomainId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
```

modelBuilder.Entity<BoardGames_Mechanics>() .HasKey(i => new { i.BoardGameId, i.MechanicId });

```csharp
    modelBuilder.Entity<BoardGames_Mechanics>()
        .HasOne(x => x.BoardGame)
        .WithMany(y => y.BoardGames_Mechanics)
        .HasForeignKey(f => f.BoardGameId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
```

modelBuilder.Entity<BoardGames_Mechanics>() .HasOne(o => o.Mechanic) .WithMany(m => m.BoardGames_Mechanics) .HasForeignKey(f => f.MechanicId) .IsRequired() .OnDelete(DeleteBehavior.Cascade); } This code should be quite easy to understand. We wrote down our plan using the Fluent API, thus ensuring that EF Core will be able to fill all the navigation properties defined in our entities properly whenever we want to enforce such behavior. Now that we’ve set up the relationships between our entities, we need to make them accessible through our ApplicationDbContext class.

Defining the DbSets In EF Core, entity types can be made available through the DbContext by using a container class known as DbSet. In a nutshell, each DbSet represents a homogenous set of entities and allows us to perform CRUD operations for that entity set. In other words, if the entity class represents a single record within a database table, we can say that the DbSet represents the database table itself.

DbSets are typically exposed by the DbContext through public properties, one for each entity. Here’s an example that represents a DbSet for the BoardGame entity:

public DbSet<BoardGame> BoardGames => Set<BoardGame>();

Now that we know the back story, we’re ready to switch back to the /Models/ ApplicationDbContext.cs file and add a DbSet for each of our entities at the end of the class in the following way (new lines marked in bold):

using Microsoft.EntityFrameworkCore;

```csharp
namespace MyBGList.Models
{
    public class ApplicationDbContext : DbContext
    {
```

// ... existing code

```csharp
        public DbSet<BoardGame> BoardGames => Set<BoardGame>();
        public DbSet<Domain> Domains => Set<Domain>();
        public DbSet<Mechanic> Mechanics => Set<Mechanic>();
        public DbSet<BoardGames_Domains> BoardGames_Domains
            => Set<BoardGames_Domains>();
        public DbSet<BoardGames_Mechanics> BoardGames_Mechanics =>
Set<BoardGames_Mechanics>();
    }
}
```

With that code, we can consider our EF Core data model to be ready. All we have to do now is to add the ApplicationDbContext class to our app’s services container so that we can start using it.

### 4.3.4 Setting up the DbContext

In this section, we’ll set up and configure an instance of our brand- new ApplicationDbContext class in our MyBGList web API project. As we should know at this point, this step must be performed within the Program.cs file. Before we jump to the code, however, it may be wise to spend a couple of minutes reviewing the role of a DbContext instance within an ASP.NET Core web application, which also determines its lifecycle. To do that, we need to understand the concept of a unit of work.

Unit of work The best definition of unit of work is given by Martin Fowler, the software engineer who introduced the dependency injection design pattern in his Catalog of Patterns of Enterprise Application architecture (http://mng.bz/0yQv):

[A unit of work] maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.

In a nutshell, a unit of work is a set of CRUD operations performed against a database during a single business transaction. From a RDBMS point of view, we can think of it as a transaction, with the sole difference being that it’s handled at the application level instead of the database level.

In most ASP.NET Core web applications, a DbContext instance is meant to be used for a single unit of work that gets executed within a single HTTP request/response lifecycle. Although this approach is not required, it’s widely acknowledged as good practice to ensure the atomicity, consistency, isolation, and durability (ACID) properties of each transaction. Here’s a typical usage scenario:

1. The web API receives an HTTP GET request for a list of board games, which is handled by a controller (BoardGamesController) and a dedicated action method (Get).

2. The action method obtains a DbContext instance through dependency injection and uses it to perform a READ operation (resulting in a SELECT query) to retrieve the requested records.

3. The DbContext retrieves the (raw) data from the BoardGames database table and uses it to create a set of entities, which it returns to the calling action method.
4. The action method receives the resulting entities and returns them to the caller through a JSON-serialized data transfer object (DTO).
5. The DbContext instance gets disposed.

This example depicts a typical unit of work that performs a single read-only operation. Let’s see what happens when we need to read and write data, such as when dealing with an HTTP PUT or POST request. For simplicity, we’ll assume that such requests come from an authorized source such as a web-based management interface, using a secure token (or IP address) that was checked beforehand.

6. The web API receives an HTTP PUT request meant to globally replace the term "war" with the term "conflict" in all the board game’s Mechanics. The request is handled by a dedicated Rename action method in the MechanicsController, which accepts two parameters: oldTerm ("war") and newTerm ("conflict").
7. Again, the action method obtains a DbContext instance through dependency injection and uses it to perform a READ operation (resulting in a SELECT query) to retrieve the records to modify, using the oldTerm parameter passed by the request. The resulting records are used to create a set of Mechanic entities, which the DbContext returns to the action method, tracking them for changes.
8. The action method receives the resulting Mechanic entities and changes their names in a foreach cycle, using the newTerm parameter. These changes are tracked by the DbContext instance, because we’re still operating within the same unit of work.
9. After the end of the foreach cycle, the action methods call the DbContext’s SaveChanges method to save the batch rename job.
10. EF Core detects the changes and persists them to the database.
11. The action method returns a successful response to the caller (in JSON format), typically with some details on what happened: number of modified records, task duration, and so on.
12. The DbContext instance is disposed.

Now that we know how the DbContext instance is meant to work, we’re ready to set it up within our Program.cs file.

Configuring the DbContext With the following lines of code, which we can put in the Program.cs file, we’ll register our ApplicationDbContext class as a scoped service in the ASP.NET Core service provider and configure it to use a SQL Server database with a given connection string:

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")) ); By following this pattern, the connection string is meant to be added in our appsettings .json file—or in the appsettings.Development.json file, because we’ll likely be using a different connection string in production. Because a SQL Server connection string contains the login user credentials (username and password) in clear text, however, doing that during development would likely expose our source code to vulnerability problems. If we plan to push our code to a GitHub repository, for example, everyone who has access to it will be able to see and use those credentials. What can we do to prevent that situation?

Securely storing the connection string Securing the connection string, as well as any secrets we may have to use while working on our web API, is a critical requirement in software development. In our specific scenario, to deal with our connection-string problem efficiently, we can consider the following approaches:

Put the connection string in the appsettings.Development.json file, and configure it to be excluded/ignored by our source control provider so that it won’t be uploaded with the rest of the code. Put the connection string in an environment variable and put a reference to its name within the appsettings file, or use this reference directly from the source code, skipping the appsettings approach. Use Visual Studio’s Secret Manager feature to store the connection string (and its credentials) securely in a separate, protected place. All these methods are viable enough. But excluding a configuration file from source control management may have other negative effects. What if we lose this file and are unable to recover it? Currently, the file contains only a few lines, but it might grow in the future, possibly containing other configuration settings that could be difficult to retrieve.

As for the environment-variable pattern, I don’t recommend using it. This approach will be harder to maintain as the number of keys to secure increases, which is likely to happen in almost all apps. Because we’re using Visual Studio, the Secret Manager feature will help us solve all credential-related problems throughout the rest of the book—at least, during development.

Introducing Secret Manager One of the best aspects of the Secret Manager feature is that it can be used from within the Visual Studio GUI. All we need to do is to right-click to the project’s root folder in Solution Explorer and then choose the Manage User Secrets option from the contextual menu (figure 4.9).

Figure 4.9 Accessing the Visual Studio Secret Manager feature As soon as we select that option, Visual Studio adds a UserSecretsId element within a PropertyGroup of our web API project’s configuration file (the MyBGList.csproj file). Here’s what the element looks like:

<UserSecretsId> cfbd4e7a-6cc3-470c-8bd3-467c993c69e6 </UserSecretsId>

By default, the inner text of that UserSecretsId element is a globally unique identifier (GUID). But this text is arbitrary and can be changed as long as it’s unique to the project (unless we want more projects to share the same secrets). Right after adding the UserSecretsId value to our project, Visual Studio will automatically use it to generate an empty secrets.json file in the following folder:

C:\Users\<UserName>\AppData\Roaming\Microsoft ➥\UserSecrets\<UserSecretsId>\secrets.json

When that’s done, Visual Studio automatically opens that file for us from within the GUI in edit mode, where we can store our secrets securely. All the key/value pairs that we put in the secrets.json file override the corresponding key/value pairs in any appsettings*.json file (or are appended to them if they don’t exist there), which means that if we put our connection string within the secrets.json file, it will be treated (and fetched by the source code) as though it’s in the appsettings.json file, regardless of the environment. Let’s take advantage of this feature and put our connection string in the secrets.json file in the following way: { "ConnectionStrings": { "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=MyBGList; User Id=MyBGList;Password=MyS3cretP4$$; Integrated Security=False;MultipleActiveResultSets=True; TrustServerCertificate=True" } }

NOTE This connection string contains some common compatibility settings that ensure a good level of SQL Server and ASP.NET Core interoperability and should work in most scenarios. Be sure to change the sample password (MyS3cretP4$$) to the one that was used to create the Login user.

As we can see, we’ve created the same key/value pair conventional structure used by the appsettings.json file, which is good practice, because we’ll likely need to put these values in a remotely- hosted appsettings.Production.json file when we deploy our app in a production environment (chapter 12). Also, the structure we’ve used matches the ASP.NET Core conventions of the GetConnectionString method that we used in the Program.cs file to retrieve the connection-string value, so we’re good to go.

NOTE For reasons of space, we won’t delve further into Visual Studio’s Secret Manager feature. For additional info about it and other usage scenario samples, check out http://mng.bz/KlGO.

Now that our ApplicationDbContext class has been configured properly, we can use it to create our database structure.

### 4.3.5 Creating the database structure

The code-first approach that we’ve chosen to adopt relies on generating the database schema from the source code, using the EF CLI’s migrations tool. As always, it could be wise to briefly review the concept of migrations.

Introducing migrations The EF Core migrations tool was introduced with the goal of keeping the database schema in sync with the EF Core model by preserving data. Every time we need to create or update the schema, we can use the tool to compare the existing database structure with our current data model and generate a C# file containing the required set of SQL commands to update it. This autogenerated file is called a migration, and per the EF Core CLI default settings, it’s created in a /Migrations/ folder.

The current state of the model is stored in another autogenerated file called <DbContext>ModelSnapshot.cs. That file is created in that same /Migrations/ folder with the initial migration and gets updated with each subsequent migration. The snapshot file allows EF Core to calculate the changes required to synchronize the database structure with the data model.

NOTE As we can easily guess, the migration tool knows what to do, and which database to check and/or update, by fetching our ApplicationDbContext and the connection string that we defined.

Adding the initial migration In our current scenario, because our MyBGList database is still empty, we need to create the initial migration, which will be used to create the database schema from scratch. To do that, open a command prompt window, navigate to the MyBGList project’s root folder, and type the following command:

> dotnet ef migrations add Initial

If we did everything correctly, we should see the following output:

Build started... Build succeeded. info: Microsoft.EntityFrameworkCore.Infrastructure[10403] Entity Framework Core 6.0.11 initialized 'ApplicationDbContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer:6.0.11' with options: None Done. To undo this action, use 'ef migrations remove'

That "Done" message at the end means that the initial migration was created successfully. To confirm that, we can check for the presence of a new /Migrations/ folder with the two autogenerated files in it, as shown in figure 4.10. Now that our initial migration is ready, we need to apply it to our database.
Figure 4.10 The new /Migrations/ folder with the autogenerated initial migration and snapshot files

Updating the database It’s important to understand that when we add a migration, we only create the autogenerated files that will update our database structure according to our current data model. The actual SQL queries won’t be executed until we apply that migration by using the database update EF Core CLI command. To apply our Initial migration to our MyBGList database, run the following command:

> dotnet ef database update Initial

TIP In this specific case, the Initial parameter could be omitted, because the database update command, when executed without a specific migration name, always updates the database up to the most recent migration. Specifying a name can be useful in some cases, such as reverting to a previous migration, which is a great way to roll back unwanted changes applied to the database schema.

After executing the command, we should see output containing the actual SQL queries run against the MyBGList database by the EF migrations tool, followed by another "Done" confirmation message to inform us that everything went well. As soon as we see that outcome, we can launch SQL Server Management Studio and connect to our MyBGList database to check what happened. If we did everything correctly, we should see the five database tables that correspond to our entities and all their expected columns, as well as the relationships, indexes, and foreign keys that match our data model (figure 4.11).
Figure 4.11 The database tables generated by the EF Core Migration tool

Now that our database structure is ready, we need to learn how to use our ApplicationDbContext class within our web API to interact with it. In chapter 5, we’ll acquire this knowledge by accomplishing several data-related tasks, including the following:

Importing the board game’s CSV file, which we downloaded from Kaggle, into the MyBGList database Refactoring our BoardGamesController’s Get action method to serve the actual data through the BoardGame entities instead of the sample data we’re currently using

## 4.4 Exercises

As always, the following exercises emulate some task assignments given by our product owner and addressed to the MyBGList development team—in other words, to us.

TIP The solutions to the exercises are available on GitHub in the /Chapter_ 04/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 4.4.1 Additional fields

Add the following properties to the BoardGames entity:

AlternateNames (string, max length 200, not required) Designer (string, max length 200, not required) Flags (int, required)

Furthermore, add the following properties to the Domains and Mechanics entities:

Notes (string, max length 200, not required) Flags (int, required)

### 4.4.2 One-to-many relationship

Add a new Publisher entity that will be used to list all the available board-game publisher (one for each BoardGame). The new entity is meant to have the Publishers table name and the following properties:

Id (primary key, int, not null) Name (string, max length 200, required) CreatedDate (datetime, required) LastModifiedDate (datetime, required)

Next, create a one-to-many relationship between this entity and the BoardGame entity, adding the PublisherId property (int, required) to the BoardGame entity. Remember to add the navigation properties to the two entities, as well as to define the foreign keys, cascading rules, and DbSet<Publisher> in the ApplicationDbContext class, using the Fluent API.

### 4.4.3 Many-to-many relationship

Add a new Category entity to list all the available categories (one or more) for each BoardGame. The new entity is meant to have the Categories table name and the following properties:

Id (primary key, int, not null) Name (string, max length 200, required) CreatedDate (datetime, required) LastModifiedDate (datetime, required)

Next, create a many-to-many relationship between this entity and the BoardGame entity, adding a BoardGames_Categories junction entity with the minimum number of required properties. Remember to add the navigation properties to the three entities, as well as to define the foreign keys, cascading rules, and DbSets for the Category and the BoardGames_Categories entities in the ApplicationDbContext class, using the Fluent API.

### 4.4.4 Creating a new migration

Using the EF Core Migration tool, create a new Chapter4_Exercises migration containing all the changes performed during the previous tasks. This task will also check the updated data model for consistency, ensuring that the preceding exercises have been done properly.

### 4.4.5 Applying the new migration

Apply the new Chapter4_Exercises migration, using the EF Core Migration tool. Then inspect the MyBGList database structure to ensure that all the new tables and columns were created.

### 4.4.6 Reverting to a previous migration

Roll back the database schema to its previous state by applying the Initial migration, using the EF Core Migration tool. Then inspect the MyBGList database structure to ensure that all the new tables and columns were removed.

Summary It’s time to replace the fake sample data we’ve used so far with a proper data source managed by a DBMS. First, however, it’s strongly advisable to review the various SQL and NoSQL alternatives and pick a suitable one for our scenario. After careful analysis, installing a local instance of the Microsoft SQL Server Express edition seems to be a viable choice for development purposes. It’s also recommended to install SQL Server Management Studio (SSMS), a data modeling tool that allows us to perform various management tasks (creating databases, tables, and so on) by using the GUI. We’re going to access our SQL database from our web API by using EF Core, a set of technologies designed to help developers interact with a DBMS source through a set of ORM techniques. Using an ORM instead of writing raw SQL queries might have some drawbacks, but it offers strong benefits in terms of security, flexibility, optimizations, readability, and maintainability. After installing EF Core, we must create our ApplicationDbContext class, which represents the operational context between ASP.NET Core and the database. We also need a set of entities, C# classes that represent the tables of our database and allow us to perform CRUD operations directly from the code. To allow the ApplicationDbContext to interact with the database, we need to instantiate it with a connection string, which we can store securely in our development environment using Visual Studio’s Secret Manager feature. As soon as we set up the connection string, we can use the EF Core migrations tool to create our database structure from our current EF Core data model.
