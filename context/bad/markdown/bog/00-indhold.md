---
title: Indholdsfortegnelse
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 0
---

# Building Web APIs with ASP.NET Core — indhold

## Part 1 Getting started

### [1. Web APIs at a glance](01-web-apis-at-a-glance.md)

- **1.1 Web APIs**
  - Overview
  - Real-world example
  - Types of web APIs
  - Architectures and message protocols
- **1.2 ASP.NET Core**
  - Architecture
  - Program.cs
  - Controllers
  - Minimal APIs
  - Task-based asynchronous pattern

### [2. Our first web API project](02-foerste-web-api-projekt.md)

- **2.1 System requirements**
  - .NET SDK
  - Integrated development environment
- **2.2 Installing Visual Studio**
- **2.3 Creating the web API project**
- **2.4 MyBGList project overview**
  - Reviewing launchSettings.json
  - Configuring the appsettings.json
  - Playing with the Program.cs file
  - Inspecting the WeatherForecastController
  - Adding the BoardGameController
- **2.5 Exercises**
  - launchSettings.json
  - appsettings.json
  - Program.cs
  - BoardGame.cs
  - BoardGameControllers.cs

### [3. RESTful principles and guidelines](03-restful-principper.md)

- **3.1 REST guiding constraints**
  - Client-server approach
  - Statelessness
  - Cacheability
  - Layered system
  - Code on demand
  - Uniform interface
- **3.2 API documentation**
  - Introducing OpenAPI
  - ASP.NET Core components
- **3.3 API versioning**
  - Understanding versioning
  - Should we really use versions?
  - Implementing versioning
- **3.4 Exercises**
  - CORS
  - Client-side caching
  - COD
  - API documentation and versioning

## Part 2 Basic concepts

### [4. Working with data](04-working-with-data.md)

- **4.1 Choosing a database**
  - Comparing SQL and NoSQL
  - Making a choice
- **4.2 Creating the database**
  - Obtaining the CSV file
  - Installing SQL Server
  - Installing SSMS or ADS
  - Adding a new database
- **4.3 EF Core**
  - Reasons to use an ORM
  - Setting up EF Core
  - Creating the DbContext
  - Setting up the DbContext
  - Creating the database structure
- **4.4 Exercises**
  - Additional fields
  - One-to-many relationship
  - Many-to-many relationship
  - Creating a new migration
  - Applying the new migration
  - Reverting to a previous migration

### [5. CRUD operations](05-crud-operations.md)

- **5.1 Introducing LINQ**
  - Query syntax vs. method syntax
  - Lambda expressions
  - The IQueryable<T> interface
- **5.2 Injecting the DbContext**
  - The sync and async methods
  - Testing the ApplicationDbContext
- **5.3 Seeding the database**
  - Setting up the CSV file
  - Installing the CsvHelper package
  - Creating the BggRecord class
  - Adding the SeedController
  - Reading the CSV file
  - Executing the SeedController
- **5.4 Reading data**
  - Paging
  - Sorting
  - Filtering
- **5.5 Updating and deleting data**
  - Updating a BoardGame
  - Deleting a BoardGame
- **5.6 Exercises**
  - Create
  - Read
  - Update
  - Delete

### [6. Data validation and error handling](06-validation-error-handling.md)

- **6.1 Data validation**
  - Model binding
  - Data validation attributes
  - A nontrivial validation example
  - Data validation and OpenAPI
  - Binding complex types
- **6.2 Error handling**
  - The ModelState object
  - Custom error messages
  - Manual model validation
  - Exception handling
- **6.3 Exercises**
  - Built-in validators
  - Custom validators
  - IValidatableObject
  - ModelState validation
  - Exception handling

## Part 3 Advanced concepts

### [7. Application logging](07-application-logging.md)

- **7.1 Application logging overview**
  - From boats to computers
  - Why do we need logs?
- **7.2 ASP.NET logging**
  - A quick logging test
  - Log levels
  - Logging configuration
  - Logging providers
  - Event IDs and templates
  - Exception logging
- **7.3 Unstructured vs. structured logging**
  - Unstructured logging pros and cons
  - Structured logging advantages
  - Application Insights logging provider
- **7.4 Third-party logging providers**
  - Serilog overview
  - Installing Serilog
  - Configuring Serilog
  - Testing Serilog
  - Improving the logging behavior
- **7.5 Exercises**
  - JSON console logging
  - Logging provider configuration
  - Exception logging’s new property
  - New Serilog enricher
  - New Serilog sink

### [8. Caching techniques](08-caching.md)

- **8.1 Caching overview**
- **8.2 HTTP response caching**
  - Setting the cache-control header manually
  - Adding a default caching directive
  - Defining cache profiles
  - Server-side response caching
  - Response caching vs. client reload
- **8.3 In-memory caching**
  - Setting up the in-memory cache
  - Injecting the IMemoryCache interface
  - Using the in-memory cache
- **8.4 Distributed caching**
  - Distributed cache providers overview
  - SQL Server
  - Redis
- **8.5 Exercises**
  - HTTP response caching
  - Cache profiles
  - Server-side response caching
  - In-memory caching
  - Distributed caching

### [9. Authentication and authorization](09-auth.md)

- **9.1 Basic concepts**
  - Authentication
  - Authorization
- **9.2 ASP.NET Core Identity**
  - Installing the NuGet packages
  - Creating the user entity
  - Updating the ApplicationDbContext
  - Adding and applying a new migration
  - Setting up the services and middleware
  - Implementing the AccountController
- **9.3 Authorization settings**
  - Adding the authorization HTTP header
  - Setting up the [authorize] attribute
  - Testing the authorization flow
- **9.4 Role-based access control**
  - Registering new users
  - Creating the new roles
  - Assigning users to roles
  - Adding role-based claims to JWT
  - Setting up role-based auth rules
  - Testing the RBAC flow
  - Using alternative authorization methods
- **9.5 Exercises**
  - Adding a new role
  - Creating a new user
  - Assigning a user to roles
  - Implementing a test endpoint
  - Testing the RBAC flow

### [10. Beyond REST](10-beyond-rest.md)

- **10.1 REST drawbacks**
  - Overfetching
  - Underfetching
- **10.2 GraphQL**
  - GraphQL advantages
  - GraphQL drawbacks
  - Implementing GraphQL
  - Working with GraphQL
- **10.3 Google Remote Procedure Call**
  - gRPC pros
  - gRPC cons
  - Installing the NuGet packages
  - Implementing the gRPC Server
  - Implementing the gRPC client
  - Adding Authorization support
- **10.4 Other REST alternatives**
  - Newline Delimited JSON (NDJSON)
  - Falcor
  - Thrift
- **10.5 Exercises**
  - Write a new GraphQL query
  - Fetch GraphQL data for a mutation
  - Implement new gRPC server features
  - Add new gRPC client wrappers
  - Test the new gRPC features

## Part 4 Toward production

### [11. API documentation](11-api-documentation.md)

- **11.1 Web API potential audience**
  - Prospectors
  - Contractors
  - Builders
- **11.2 API documentation best practices**
  - Adopt an automated description tool
  - Describe endpoints and input parameters
  - Add XML documentation support
  - Work with Swashbuckle annotations
  - Describe responses
  - Add request and response samples
  - Group endpoints into sections
  - Exclude reserved endpoints
- **11.3 Filter-based Swagger customization**
  - Emphasizing the authorization requirements
  - Changing the application title
  - Adding a warning text for passwords
  - Adding custom key/value pairs
- **11.4 Exercises**
  - Use XML documentation
  - Use Swashbuckle annotations
  - Exclude some endpoints
  - Add a custom filter
  - Add custom key/value pairs

### [12. Release and deployment](12-release-deployment.md)

- **12.1 Prepublishing tasks**
  - Considering security
  - Choosing a domain name
  - Setting up a CDN
  - Fine-tuning our APP
  - Understanding the .NET publishing modes
- **12.2 Creating a Windows VM server**
  - Accessing Azure
  - Creating and setting up the Windows VM
  - Working with the VM public IP address
  - Creating an SSL/TLS origin certificate
  - Setting Cloudflare Encryption Mode to Full
- **12.3 Configuring the Windows VM server**
  - Installing IIS
  - Installing the ASP.NET Core hosting bundle
  - Installing the Web Deploy component
  - Opening the 8172 TCP port
  - Configuring IIS
  - Creating the production database
  - Creating the appsettings.Production.json file
- **12.4 Publishing and deploying**
  - Introducing Visual Studio publish profiles
  - Creating an Azure VM publish profile
  - Configuring the publish profile
  - Publishing, deployment, and testing
  - Final thoughts

