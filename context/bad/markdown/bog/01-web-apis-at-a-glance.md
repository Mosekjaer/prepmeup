---
title: Web APIs at a glance
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 1
---

# 1. Web APIs at a glance

**This chapter covers**

- Web API overview and real-world case samples
- Types of web APIs and their pros and cons
- ASP.NET Core overview
- Main ASP.NET Core architectural principles

Almost all applications need data, especially web-based applications, in which large numbers of clients interact with a centralized entity— typically, an HTTP-based service—to access information and possibly update or manipulate them. In this book, we’ll learn how to design and develop a specific type of HTTP-based service that has the sole purpose of providing data to these clients, allowing them to interact with the information they require in a uniform, structured, and standardized way: a web API.

In the first section of this chapter, we’ll see the distinctive characteristics of a web API and learn how it can be applied to several real-world scenarios. In the second section, we’ll get familiar with ASP.NET Core, the web framework we’ll be using to create web APIs throughout this book.

## 1.1 Web APIs

An application programming interface (API) is a type of software interface that exposes tools and services that computer programs use to interact with each other and exchange information. The connection required to perform such exchanges is established by means of common communication standards (protocols), given sets of available operations (specifications), and data exchange formats (JSON, XML, and the like).

From that definition, we can easily see that the main purpose of an API is to allow parties to communicate by using a common syntax (or abstraction) that simplifies and standardizes what happens under each hood. The overall concept is similar to that of real-world interfaces, which also provide a common “syntax” to allow different parties to operate. A perfect example is the plug socket, an abstraction used by all national electrical systems to allow household appliances and electronic devices to interact with the power supply through given voltage, frequency, and plug type standards.

Figure 1.1 shows the main components that form an electrical grid: an interconnected network for generating, transmitting, and delivering electricity from producers to residential consumers. As we can see, each component handles electricity in a different way and communicates with the others by using various “protocols” and “adapters” (cable lines, transformers, and so on) with the ultimate goal of bringing it to people’s houses. When the residential units are connected to the grid, the electricity can be used by home appliances (TV sets, ovens, refrigerators, and so on) through a secure, protected, easy-to-use interface: the AC power plug. If we think about how these sockets work, we can easily understand how much they simplify the technical aspects of the underlying power grid. Home appliances don’t have to know how such a system works as long as they can deal with the interface.
Figure 1.1 A common example of an interface: the AC power plug socket

A web API is the same concept brought to the World Wide Web: an interface accessible through the web that exposes one or more plugs (the endpoints), which other parties (the clients) can use to interact with the power supply (the data) by using common communication protocols (HTTP) and standards (JSON/XML formats).

NOTE Throughout this book, the term web API will be used interchangeably to mean both the interface and the actual web application.

### 1.1.1 Overview

Figure 1.2 illustrates the purpose of a web API within a typical service-oriented architecture (SOA) environment. SOA is an architectural style built around the separation of responsibility of various independent services that communicate over a network.
Figure 1.2 The role of a web API in a typical SOA-based environment

As we can see, the web API plays a key role because it is responsible for retrieving the data from the underlying database management system (DBMS) and making it available to the services:

Web app 1 (such as a React informative website), which fetches data from the web API to display it to end users through HTML pages and components Mobile app 1 (such as an Android app) and mobile app 2 (which can be the iOS port of the same app), which also fetches data and show them to end users through their native user interfaces (UIs) Web app 2 (such as a PHP management website), which accesses the web API to allow administrators to interact with and possibly modify the data Cloud service 1 (such as a data warehouse), which periodically pulls data from the web API to feed its internal repository (to persist some access logs, for example) Cloud service 2 (such as machine learning software), which periodically retrieves some test data from the web API and uses it to perform predictions and provide useful insights

What we’ve just described is an interesting scenario that can help us understand the role of a web API within a rather common service- based ecosystem. But we’re still discussing an impersonal, theoretical approach. Let’s see how we can adapt the same architecture to a specific real-world scenario.

### 1.1.2 Real-world example

In this section, we instantiate the abstract concept depicted in figure 1.1 into a concrete, credible, and realistic scenario. Everyone knows what a board game is, right? We’re talking about tabletop games with dice, cards, playing pieces, and stuff like that. Suppose that we work in the IT department of a board-gaming club. The club has a database of board games that includes gaming info such as Name, Publication Year, Min-Max players, Play Time, Minimum Age, Complexity, and Mechanics, as well as some ranking stats (number of ratings and average rating) given by club members, guests, and other players over time. Let’s also assume that the club wants to use this database to feed some web-based services and applications, such as the following:

An end-user website—Accessible to everyone, to showcase the board games and their ratings, as well as provide some additional features to registered users (such as the ability to create lists) A mobile app—Also publicly accessible, with the same features as the website and an optimized UI/UX (user experience) interface for smartphones and tablets A management portal—Available to a restricted list of authorized users, that allows those users to perform Create, Read, Update, and Delete (CRUD) operations in the database and carry out other administration-based tasks A data analysis service—Hosted on a third-party platform

As we can easily guess, a great way to fulfill such requirements would be to implement a dedicated web API. This approach allows us to feed all these services without exposing the database server—and its underlying data model—to any of them. Figure 1.3 shows the architecture.
Figure 1.3 Several board-game-related web applications and services fed by a single MyBGList web API

Again, the web API is the playmaker, fetching the data source containing all the relevant data (MyBGList DBMS) and making it available to the other services:

MyBGList website—A ReactJS website entirely hosted on a content-delivery network (following the Jamstack approach) where users can browse the board-game catalog and add games to a set of predefined lists (Own, Want to Try, Want to Buy, and so on), as well as add their own custom lists MyBGList mobile app—A React Native app available for Android and iOS that allows users to perform the same actions that are available on the MyBGList website (browse and add to lists) MyBGList management portal—An ASP.NET web application hosted on a dedicated virtual machine (VM) server within a secure private cloud that system administrators can access to add, update, and delete board games, as well as perform maintenance-based tasks MyBGList insights—A Software as a Service (SaaS) service that periodically pulls data from the web API to feed its internal repository and to perform logging, monitoring, performance analysis, and business intelligence tasks

This web API is the one we’ll be working with throughout the following chapters.

What is Jamstack? Jamstack (JavaScript, API, and Markup) is a modern architecture pattern based on prerendering the website as static HTML pages and loading the content using JavaScript and web APIs. For further information regarding the Jamstack approach, see https://jamstack.org. For a comprehensive guide to developing standards-based static websites using the Jamstack approach, check out The Jamstack Book: Beyond Static Sites with JavaScript, APIs, and Markup, by Raymond Camden and Brian Rinaldi (https://www.manning.com/books/the-jamstack-book).

### 1.1.3 Types of web APIs

Now that we know the general picture, we can spend some time exploring the various architectures and messaging protocols available to web API developers nowadays. As we’ll be able to see, each type has characteristics, pros, and cons that make it suitable for different applications and businesses.

What you won’t find in this book For reasons of space, I’ve intentionally restricted this book’s topics to HTTP web APIs, skipping other Application-layer protocols such as Advanced Message Queuing Protocol (AMQP). If you’re interested in knowing more about message-based applications through AMQP, I suggest reading RabbitMQ in Depth, by Gavin M. Roy (https://www.manning.com/books/rabbitmq-in-depth).

Before looking at the various architectures, let’s briefly summarize the four main scopes of use in which each web API commonly falls:

Public APIs—Public APIs are also called open APIs (not to be confused with the OpenAPI specification). As the name suggests, this term refers to APIs that are meant to be available for use by any third party, often without access limitations. These APIs typically involve no authentication and authorization (if they’re free to use), or they employ a key or token authentication mechanism if they need to identify the caller for various reasons (such as applying per-call costs). Furthermore, because their endpoints are usually accessible from the World Wide Web, public APIs often use various throttling, queueing, and security techniques to avoid being crippled by a massive number of simultaneous requests, as well as denial of service (DoS) attacks. Partner APIs—The APIs that fall into this category are meant to be available only to specifically selected and authorized partners, such as external developers, system integrator companies, whitelisted external internet protocols (IPs), and the like. Partner APIs are used mostly to facilitate business-to-business (B2B) activities. An example would be an e-commerce website that wants to share its customer data with a third-party business partner that needs to feed its own customer relationship management (CRM) and marketing automation systems. These APIs typically implement strong authentication mechanisms and IP restriction techniques to prevent them from being accessed by unauthorized users; they’re definitely not meant to be consumed by end users or “public” clients such as standard websites. Internal APIs—Also known as private APIs, these APIs are meant for internal use only, such as to connect different services owned by the same organization, and are often hosted within the same virtual private network, web farm, or private cloud. An internal API, for example, can be used by the internal enterprise resource planning software to retrieve data from various internal business sources (payroll, asset management, project management, procurement, and so on), as well as create high- level reports and data visualizations. Because these APIs are not exposed to the outside, they often implement mild authentication techniques, depending on the organization’s internal security model and its performance and load-balancing tweaks. Composite APIs—Sometimes referred to as API gateways, these APIs combine multiple APIs to execute a sequence of related or interdependent operations with a single call. In a nutshell, they allow developers to access several endpoints at the same time. Such an approach is used mostly in microservice architecture patterns, in which executing a complex task can require the completion of a chain of subtasks handled by several services in a synchronous or asynchronous way. A composite API acts mostly as an API orchestrator, ensuring that the various subtasks required to perform the main call are successful and capable of returning a valid result (or invalidating the whole process if they aren’t). A common use scenario ensures that multiple third-party services fulfill their respective jobs, such as when we need to delete a user record (and all its personal info) permanently in our internal database and in several external data sources without creating atomicity problems, such as leaving potentially sensitive data somewhere. Because the orchestrated calls can be of multiple types (public, partner, and/or internal), composite APIs often end up being hybrids, which is why they’re generally considered to represent a different API type.

### 1.1.4 Architectures and message protocols

To fulfill its role of enabling data exchange among parties, a web API needs to define a clear, unambiguous set of rules, constraints, and formats. This book deals with the four most-used web API architectural styles and message protocols: REST, RPC, SOAP, and GraphQL. Each has distinctive characteristics, trade-offs, and supported data interchange formats that make it viable for different purposes. REST Representational state transfer (REST) is an architectural style specifically designed for network-based applications that use standard HTTP GET, POST, PATCH, PUT, and DELETE request methods (first defined in the now-superseded RFC 2616, https://www.w3.org/Protocols/rfc2616/rfc2616.xhtml) to access and manipulate data. More specifically, GET is used to read data, POST to create a new resource or perform a state change, PATCH to update an existing resource, PUT to replace an existing resource with a new one (or create it if it doesn’t exist), and DELETE to erase the resource permanently. This approach does not require additional conventions to allow the parties to communicate, which makes it easy to adopt and fast to implement.

REST is much more, however. Its architectural paradigm relies on six guiding constraints that, if implemented correctly, can greatly benefit several web API properties:

Client-server approach—RESTful APIs should enforce the Separation of Concerns principle by keeping the UI and data storage concerns apart. This approach is particularly suitable for the World Wide Web, where clients (such as browsers) have a separate role from web applications and don’t know anything about how they were implemented. It’s important to understand that separating these concerns not only improves their portability, but also keeps them simpler to implement and maintain, thus improving the simplicity, scalability, and modifiability of the whole system. Statelessness—The server should handle all communications between clients without keeping, storing, or retaining data from previous calls (such as session info). Also, the client should include any context-related info, such as authentication keys, in each call. This approach reduces the overhead of each request on the server, which can significantly improve the performance and scalability properties of the web API, especially under heavy load. Cacheability—The data exchanged between the client and the server should use the caching capabilities provided by the HTTP protocol. More specifically, all HTTP responses must contain the appropriate caching (or noncaching) info within their headers to optimize client workload while preventing them from serving stale or outdated content by mistake. A good caching strategy can have a huge affect on the scalability and performance properties of most web APIs. Layered system—Putting the server behind one or more intermediary HTTP services or filters—such as forwarders, reverse proxies, and load balancers—can greatly improve the overall security aspects of a web API, as well as its performance and scalability properties. Code on demand (COD)—COD is the only optional REST constraint. It allows the server to provide executable code or scripts that clients can use to adopt custom behavior. Examples of COD include distributed computing and remote evaluation techniques, in which the server delegates part of its job to clients or needs them to perform certain checks locally by using complex or custom tasks, such as verifying whether some applications or drivers are installed. In a broader sense, such a constraint also refers to the capability—rare in the early days of REST yet common in most recent JavaScript-powered web apps —to fetch from the server the source code required to build and load the application, as well as problem further REST calls. COD can improve the performance and scalability of a web API, but at the same time, it reduces overall visibility and poses nontrivial security risks, which is why it is flagged as optional. Uniform interface—The last REST constraint is the most important one. It defines the four fundamental features that a RESTful interface must have to enable clients to communicate with the server without the server knowing anything about how they work, keeping them decoupled from the underlying implementation of the web API. These features are Identification of resources—Each resource must be univocally identified through its dedicated, unique universal resource identifier (URI). The URI https://mybglist.com/api/games/11, for example, identifies a single board game. Manipulation of resources through representations—Clients must be able to perform basic operations on resources by using the resource URI and the corresponding HTTP method, without the need for additional info. To read board game 11’s data, for example, a client should make a GET request to https://mybglist.com/api/games/11. If the client wants to delete that data, it should make a DELETE request to that same URI, and so on. Self-descriptive messages—Each sender’s message must include all the information the recipient requires to understand and process it properly. This requirement is valid for the client and server and is quite easy to implement by means of the HTTP protocol. Both requests and responses are designed to include a set of HTTP headers that describe the protocol version, method, content type, default language, and other relevant metadata. The web API and connecting clients need only ensure that the headers are set properly. HATEOAS (Hypermedia as the Engine of Application State) —The server should provide useful information to clients, but only through hyperlinks and URIs (or URI templates). This requirement decouples the server from its clients because clients require little to no knowledge of how to interact with the server beyond a generic understanding of hypermedia. Furthermore, server functionality can evolve independently without creating backward-compatibility problems.

A web API that implements all these constraints can be described as RESTful.

Introducing Roy Fielding, the undisputed father of REST The six REST constraints are described by Roy Fielding, one of the principal authors of the HTTP specification and widely considered to be the father of REST, in his dissertation Architectural Styles and the Design of Network-based Software Architectures, which earned him a doctorate in computer science in 2000. The original text of Fielding’s dissertation is available in the University of California publications archive at http://mng.bz/19ln.

The widely accepted rules and guidelines, the proven reliability of the HTTP protocol, and the overall simplicity of implementation and development make REST the most popular web API architecture used nowadays. For that reason, most of the web API projects we’ll develop throughout this book use the REST architectural style and follow the RESTful approach with the six constraints introduced in this section. SOAP Simple Object Access Protocol (SOAP) is a messaging protocol specification for exchanging structured information across networks by means of Extensible Markup Language (XML). Most SOAP web services are implemented by using HTTP for message negotiation and transmission, but other Application-layer protocols, such as Simple Mail Transfer Protocol (SMTP), can be used as well.

The SOAP specification was released by Microsoft in September 1999, but it didn’t become a web standard until June 24, 2003, when it finally achieved W3C Recommendation status (SOAP v1.2, https://www.w3.org/TR/soap12). The accepted specification defines the SOAP messaging framework, consisting of the following:

SOAP Processing Model—Which defines the rules for processing a SOAP message SOAP Extensibility Model—Which introduces SOAP features and SOAP modules SOAP Underlying Protocol binding framework—Which describes the rules for creating bindings with an underlying protocol that can be used for exchanging SOAP messages between SOAP nodes

The main advantage of SOAP is its extensibility model, which allows developers to expand the base protocol with other official message- level standards—WS-Policy, WS-Security, WS-Federation, and the like—to perform specific tasks, as well as create their own extensions. The resulting web service can be documented in Web Services Description Language (WSDL), an XML file that describes the operations and messages supported by the various endpoints. This approach is also a flaw, however, because the XML used to make requests and receive responses can become extremely complex and difficult to read, understand, and maintain. This problem has been mitigated over the years by many development frameworks and IDEs, including ASP.NET and Visual Studio, which started to provide shortcuts and abstractions to ease the development experience and generate the required XML code automatically. Even so, the protocol has several disadvantages compared with REST:

Fewer data formats—SOAP supports only XML, whereas REST supports a greater variety of formats, including JavaScript Object Notation (JSON), which offers faster parsing and is definitely easier to work with, and comma-separated values (CSV), a great JSON alternative for huge data sets in which bandwidth optimization is a major concern. Worse client support—Both modern browsers and frontend frameworks have been optimized to consume REST web services, which typically provide better compatibility. Performance and caching problems—SOAP messages are typically sent via HTTP POST requests. Because the HTTP POST method is nonidempotent, it’s not cached at the HTTP level, which makes its requests much harder to cache than those of RESTful counterparts. Slower and harder to implement—A SOAP web service is generally harder to develop, especially if it must be integrated with an existing website or service. Conversely, REST typically can be implemented as a drop-in functionality of the existing code without the need to refactor the whole client-server infrastructure. For all these reasons, the general consensus today when it comes to the REST-versus-SOAP debate is that REST web APIs are the preferred way to go unless there are specific reasons to use SOAP. These reasons might include hardware, software, or infrastructure limitations, as well as requirements of existing implementations, which is why we won’t be using SOAP within this book.

GraphQL The supremacy of REST was questioned in 2015 with the public release of GraphQL, an open source data query and manipulation language for APIs developed by Facebook. The main differences between the two approaches are related not to the architectural styles, but to their different ways of sending and retrieving data.

As a matter of fact, GraphQL follows most of the RESTful constraints, relies on the same Application-layer protocol (HTTP), and adopts the same data format (JSON). But instead of using different endpoints to fetch different data objects, it allows the client to perform dynamic queries and ask for the concrete data requirements of a single endpoint.

I’ll try to explain this concept with an actual example taken from the board-game web API scenario. Suppose that we want to retrieve the names and unique IDs of all the users who gave positive feedback on the Citadels board game. When we’re dealing with typical REST APIs, fulfilling such a task would require the following operations:
1. A web API request to the full-text search endpoint—To retrieve a list of board games with a name equal to "Citadels". To keep things simple, let’s assume that such a call returns a single result, allowing us to get our target board game’s info, including its unique ID, which is what we’re looking for.
2. Another web API request to the feedback endpoint—To retrieve all the feedback received from that user’s unique ID. Again, let’s assume that the feedback ranges from 1 (“worst gaming experience I’ve ever had”) to 10 (“best game ever”).
3. A client-side iteration (such as a foreach loop)—To cycle through all the retrieved feedback and retrieve the user ID of those with a rating equal to or greater than 6.
4. A third web API request to the user endpoint—To retrieve the users who correspond to those unique IDs.

The complete request/response cycle is represented graphically in figure 1.4.
Figure 1.4 HTTP request-response cycle in REST

That plan is feasible, yet it undeniably involves a huge amount of work. Specifically, we would have to perform multiple round-trips (three HTTP requests) and a lot of over-fetching—the Citadels game info, the data of all feedback, and the data of all users who gave a positive rating—to get some names. The only workaround would be to implement additional API endpoints to return what we need or to ease some of the intermediate work. We could add an endpoint to fetch the positive feedback for a given board game ID or maybe even for a given board game name, including the full-text query in the underlying implementation. If we’re up for it, we could even implement a dedicated endpoint, such as api/positiveFeedbacksByName, to perform the whole task.

This approach, however, would undeniably affect backend development time and add complexity to our API, and wouldn’t be versatile enough to help in similar, nonidentical cases. What if we want to retrieve negative feedback instead or positive feedback for multiple games or for all the games created by a given author? As we can easily understand, overcoming such problems may not be simple, especially if we need a high level of versatility in terms of client-side data fetching requirements.

Now let’s see what happens with GraphQL. When we take this approach, instead of thinking in terms of existing (or additional) endpoints, we focus on the query we need to send to the server to request what we need, as we do with a DBMS. When we’re done, we send that query to the (single) GraphQL endpoint and get back precisely the data we asked for, within a single HTTP call and without fetching any field we don’t need. Figure 1.5 shows a GraphQL client- to-server round trip.
Figure 1.5 HTTP request-response cycle in GraphQL

As we can see, the performance optimizations aren’t limited to the web API. The GraphQL approach doesn’t require client-side iteration, because the server already returns the precise result we’re looking for.

The GraphQL specification is available at https://spec.graphql.org. I talk extensively about GraphQL in chapter 10, showing you how to use it alongside REST to achieve specific query-oriented goals.

## 1.2 ASP.NET Core

Now that we’ve seen what web APIs are and how we can use them to exchange data within web applications and services across a network, it’s time to introduce the framework that we’ll use to create them throughout this book. That framework is ASP.NET Core, a high- performance, cross-platform, open source web development framework introduced by Microsoft in 2016 as the successor to ASP.NET.

In the following sections, I briefly cover its most distinctive aspects: the overall architecture, the request/response pipeline management, the asynchronous programming pattern, the routing system, and so on.

NOTE In this book, we’re going to use .NET 6.0 and the ASP.NET Core Runtime 6.0.11, the latest generally available version at this writing and the ninth installment released so far, following .NET Core 1.0, 1.1, 2.0, 2.1, 2.2, 3.0, 3.1, and .NET 5.0. Each version introduced several updates, improvements, and additional features, but for the sake of simplicity, I’ll review the resulting characteristics shipped with the latest version. Furthermore, starting with .NET 5, all even- numbered .NET releases, including .NET 6, are granted with long- term support (LTS) status, meaning that they’ll be supported for many years to come, as opposed to odd-numbered versions, which have shorter time frames. Currently, the support period is three years for even-numbered versions and 18 months for odd-numbered versions (http://mng.bz/JVpa).

### 1.2.1 Architecture

I’ve chosen to use ASP.NET Core for our web API projects because the new Microsoft Framework enforces several modern architectural principles and best practices that allow us to build lightweight, highly modular apps with a high level of testability and maintainability of the source code. Such an approach naturally guides developers to build (or adopt) applications composed of discrete and reusable components that perform specific tasks and communicate through a series of interfaces made available by the framework. These components are called services and middleware, and they’re registered and configured in the web application’s Program.cs file, which is the app’s entry point.

NOTE If you’re coming from an older ASP.NET Core version, such as 3.1 or 5.0, you may wonder what happened to the Startup class (and its corresponding Startup.cs file), which used to contain the services and middleware configuration settings. This class was removed from the minimal hosting model introduced with .NET 6, which merged the Program and Startup classes into a single Program.cs file.

Services Services are components that an application requires to provide its functionalities. We can think of them as app dependencies, because our app depends on their availability to work as expected. I use the term dependencies here for a reason: ASP.NET Core supports the Dependency Injection (DI) software design pattern, an architectural technique that allows us to achieve inversion of control between a class and its dependencies. Here’s how the whole services implementation, registration/configuration, and injection process works in ASP.NET Core: Implementation—Each service is implemented by means of a dedicated interface (or base class) to abstract the implementation. Both the interface and the implementation can be provided by the framework, created by the developer, or acquired from a third party (GitHub, NuGet packages, and the like). Registration and configuration—All services used by the app are configured and registered (using their interfaces) in the built-in IServiceProvider class, which is a service container. The actual registration and configuration process happens within the Program.cs file, where the developer can also choose a suitable lifetime (Transient, Scoped, or Singleton). Dependency injection—Each service can be injected into the constructor of the class where it’s meant to be used. The framework, through the IServiceProvider container class, automatically makes available an instance of the dependency, creating a new one or possibly reusing an existing one, depending on the configured service’s lifetime, as well as disposing of it when it’s no longer needed.

Typical examples of service interfaces provided by the framework include IAuthorizationService, which can be used to implement policy-based permissions, and IEmailService, which can be used to send email messages.

Middleware Middleware is a set of components that operates at the HTTP level and can be used to handle the whole HTTP request processing pipeline. If you remember the HttpModules and HttpHandlers used by ASP.NET before the advent of ASP.NET Core, you can easily see how middleware plays a similar role and performs the same tasks. Typical examples of middleware used in ASP.NET Core web applications include HttpsRedirectionMiddleware, which redirects non-HTTPS requests to an HTTPS URL, and AuthorizationMiddleware, which use the authorization service internally to handle all authorization tasks at the HTTP level.

Each type of middleware can pass the HTTP request to the next component in the pipeline or provide an HTTP response, thus short- circuiting the pipeline itself and preventing further middleware from processing the request. This type of request-blocking middleware is called terminal middleware and usually is in charge of processing the main business logic tasks of the various app’s endpoints.

WARNING It’s worth noting that middleware is processed in registration order (first in, first out). Always add terminal middleware after nonterminal middleware in the Program.cs file; otherwise, the file won’t work.

A good example of terminal middleware is StaticFileMiddleware, which ultimately handles the endpoint URLs pointing to static files, as long as they are accessible. When such a condition happens, it sends the requested file to the caller with an appropriate HTTP response, thus terminating the request pipeline. The HttpsRedirectionMiddleware that I mentioned briefly earlier is also terminal middleware, because it ultimately responds with an HTTP-to-HTTPS redirect to all non-HTTPS requests.
NOTE Both StaticFileMiddleware and HttpsRedirectionMiddleware will terminate the HTTP request only when certain circumstances are met. If the requested endpoint doesn’t match their activation rules (such as a URL pointing to a nonpresent static file for the former or already in HTTPS for the latter), they pass it to the next middleware present in the pipeline without taking action. For this reason, we could say that they are potentially terminal middleware to distinguish them from middleware that always ends the request pipeline when the HTTP request reaches it.

### 1.2.2 Program.cs

Now that I’ve introduced services and middleware, we can finally take a look at the Program.cs file, which is executed at the start of the application:

var builder = WebApplication.CreateBuilder(args);   ❶

// Add services to the container.

```csharp
builder.Services.AddControllers();                  ❷
// Learn more about configuring Swagger/OpenAPI
// at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();         ❷
builder.Services.AddSwaggerGen();                   ❷
```

var app = builder.Build();                          ❸

```csharp
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                               ❹
    app.UseSwaggerUI();                             ❹
}
```

app.UseHttpsRedirection();                          ❹

```csharp
app.UseAuthorization();                             ❹
app.MapControllers();                                   ❹
```

app.Run();                                              ❺

❶ Creates the WebApplicationBuilder factory class
❷ Registers and configures services
❸ Builds the WebApplication object
❹ Registers and configures nonterminal and potentially terminal middleware
❺ Registers and configures terminal middleware

By analyzing this code, we can easily see that the file is responsible for the following initialization tasks:

Instantiating the web application Registering and configuring the services Registering and configuring the middleware

More precisely, the web application is instantiated by the WebApplicationBuilder factory class, creating a WebApplication object. This instance is stored in the app local variable, which is used to register and configure the required services and middleware.

NOTE Both services and middleware are registered and configured by dedicated extension methods, a convenient way to shortcut the setup process and keep the Program.cs file as concise as possible. Under ASP.NET Core naming conventions, services are configured mostly by using extension methods with the Add prefix; middleware prefixes are Use, Map, and Run. The only difference that concerns us, at least for now, is that the Run delegate is always 100% terminal and the last one to be processed. We’ll talk more about these conventions and their meaning when we experiment with Minimal APIs later. For the sake of simplicity, I split middleware into two categories: potentially terminal and terminal. The difference should be clear at this point. Potentially terminal middleware ends the HTTP request pipeline only if it matches certain rules, whereas terminal middleware always does (so no wonder it’s the last of the pile).

### 1.2.3 Controllers

In ASP.NET Core, a controller is a class used to group a set of action methods (also called actions) that handle similar HTTP requests. From such a perspective, we could say that controllers are containers that can be used to aggregate action methods that have something in common: routing rules and prefixes, services, instances, authorization requirements, caching strategies, HTTP-level filters, and so on. These common requirements can be defined directly on the controller class, thus avoiding the need to specify them for each action. This approach, enforced by some powerful built-in naming and coding conventions, helps to keep the code DRY and simplifies the app’s architecture.

NOTE DRY is an acronym for Don’t Repeat Yourself, a well-known principle of software development that helps us remember to avoid redundancy, pattern repetition, and anything else that could result in a code smell. It’s the opposite of WET (Write Everything Twice or Write Every Time).

Starting with ASP.NET Core, controllers can inherit from two built-in base classes:

ControllerBase, a minimal implementation without support for views Controller, a more powerful implementation that inherits from ControllerBase and adds full support for views

As we can easily understand, the Controller base class is meant to be used in web applications that adopt the Model-View-Controller (MVC) pattern, where they’re meant to return the data coming from business logic (typically handled by dependency-injected services). In a typical ASP.NET Core web application, the resulting data is returned to the client through the views, which handle the app’s data Presentation layer by using client-side markup and scripting languages such as HTML, CSS, JavaScript, and the like (or server- side rendered syntaxes such as Razor). When dealing with web APIs, we generally don’t use views because we want to return JSON or XML data (no HTML) directly from the Controller. Given such a scenario, inheriting from the ControllerBase base class is the recommended approach in our specific case. Here’s a sample web API Controller that handles two types of HTTP requests:

A GET request to /api/Sample/ to receive a list of items

A GET request to /api/Sample/{id} to receive a single item with the specified {id}, assuming that it’s a unique integer value acting as a primary key A DELETE request to /api/Sample/{id} to delete the single item with the specified {id}

```csharp
[ApiController]                                  ❶
[Route("api/[controller]")]                      ❷
public class SampleController : ControllerBase
{
    public SampleController()
    {
    }
     [HttpGet]                                    ❸
     public string Get()
     {
         return "TODO: return all items";
     }
```

```csharp
     [HttpGet("{id}")]                          ❹
     public string Get(int id)
     {
         return $"TODO: return the item with id #{id}";
     }
```

```csharp
     [HttpDelete("{id}")]                       ❺
     public string Delete(int id)
     {
         return $"TODO: delete the item with id #{id}";
     }
}
```

❶ Adds API-specific behaviors
❷ Default routing rules
❸ Action to handle HTTP GET to /api/Sample/
❹ Action to handle HTTP GET to /api/Sample/{id}
❺ Action to handle HTTP DELETE to /api/Sample/{id}

As we can see by looking at this code, we were able to fulfill our given tasks with few lines of code by taking advantage of some useful built-in conventions:

A centralized /api/Sample/ routing prefix valid for all action methods, thanks to the [Route("api/[controller]")] attribute applied at the controller level Automatic routing rules for all the implemented HTTP verbs (including the required parameters) thanks to the [HttpGet] and [HttpDelete] attributes applied to the corresponding action methods Automatic routing mapping using the ControllerMiddleware’s default rules, because we applied the Controller suffix to the class name, as long as the app.MapControllers() extension method is present in the Program.cs file

NOTE Furthermore, because we’ve used the [ApiController] attribute, we also need some additional conventions specific to web APIs that automatically return certain HTTP status codes, depending on the action type and result. We’ll talk more about them in chapter 6, when we delve into error handling.

### 1.2.4 Minimal APIs

Controllers with built-in conventions are great ways to implement web API business logic with few lines of code. In ASP.NET Core 6, however, the framework introduced a minimal paradigm that allows us to build web APIs with even less ceremony. This feature is called minimal APIs, and despite its young age, it’s getting a lot of attention from new and seasoned ASP.NET developers because it often allows for a much smaller and more readable codebase.

The best way to explain what Minimal API is about is to perform a quick code comparison between a controller-based approach and the new kid on the block. Here’s how the same HTTP requests that we handled with the SampleController can be handled by Minimal APIs with some minor updates to the Program.cs file:

```csharp
// app.MapControllers();                         ❶
app.MapGet("/api/Sample",                        ❷
   () => "TODO: return all items");              ❷
app.MapGet("/api/Sample/{id}",                           ❷
    (int id) => $"TODO: return the item with id #{id}"); ❷
app.MapDelete("/api/Sample/{id}",                        ❷
    (int id) => $"TODO: delete the item with id #{id}"); ❷
```

❶ Removes this line (and the SampleController class)
❷ Adds these lines instead

As we can see, all the implementation is transferred to the Program.cs file without the need for a separate Controller class. Also, the code can be further simplified (or DRYfied), such as by adding a prefix variable that eliminates the need to repeat "/api/Sample" multiple times. The advantage is visible in terms of code readability, simplicity, and overhead reduction.

TIP The business logic of any Minimal API method can be put outside the Program.cs file; only the Map methods are required to go there. As a matter of fact, moving the actual implementation to other classes is almost always good practice unless we’re dealing with one-liners.

The Minimal APIs paradigm isn’t likely to replace controllers, but it will definitely ease the coding experience of most small web API projects (such as microservices) and attract most developers who are looking for sleek approaches and shallow learning curves. For all these reasons, we’ll often use it alongside controllers throughout this book.

### 1.2.5 Task-based asynchronous pattern

Most performance problems that affect web applications are due to the fact that a limited number of threads needs to handle a potentially unlimited volume of concurrent HTTP requests. This is especially true when, before responding to these requests, these threads must perform some resource-intensive (and often noncacheable) tasks, being blocked until they eventually receive the result. Typical examples include reading or writing data from a DBMS through SQL queries, accessing a third-party service (such as an external website or web API), and performing any other task that requires a considerable amount of execution time.

When a web application is forced to deal with a lot of these requests simultaneously, the number of available threads quickly decreases, leading to degraded response times and, eventually, to service unavailability (HTTP 503 and the like).

NOTE This scenario is the main goal of most HTTP-based denial-of- service (DoS) attacks, in which the web application is flooded by requests to make the whole server unavailable. Thread-blocking, noncached HTTP requests are goldmines for DoS attackers because they have a great chance of starving the thread pool quickly.

A general rule of thumb in dealing with resource-intensive tasks is to cache the resulting HTTP response aggressively, as stated by REST constraint 3 (cacheability). In several scenarios, however, caching is not an option, such as when we need to write into a DBMS (HTTP POST) or read mutable or highly customizable data (HTTP GET with a lot of parameters). What do we do in those cases?

The ASP.NET Core framework allows developers to deal with this
problem efficiently by implementing the C# language-level
asynchronous model, better known as the Task-based Asynchronous
Pattern (TAP). The best way to understand how TAP works is to see it
in action. Take a look at the following source code:
[HttpGet]
public async Task<string> Get()            ❶
{
    return await Task.Run(() => {          ❷
        return "TODO: return all items";
    });
}

❶ async
❷ await

As we can see, we applied some minor updates to the first action method of the SampleController that we worked with earlier to implement TAP. The new pattern relies on using the async and await keywords to handle a Task in a nonblocking way:

The async keyword defines the methods returning a Task. The await keyword allows the calling thread to start the newly implemented Task in a nonblocking way. When the Task is completed, the thread continues the code execution, thus returning the resulting comma-separated string.

It’s worth noting that because we used the await keyword inside the Get() action method, we had to mark that method async as well. That’s perfectly fine; it means that the method will be awaited by the calling thread instead of blocking it, which is what we want.

Our minimal implementation won’t have any benefit in terms of performance. We definitely don’t need to await for something as trivial as a literal TODO string. But it should give us an idea of the benefits that the async/await pattern will bring the web application when that sample Task.Run is replaced by something that requires the server to perform some actual work, such as retrieving real data from a DBMS. I’ll talk extensively about asynchronous data retrieval tasks in chapter 4, which introduces Microsoft’s most popular data access technology for .NET: Entity Framework Core.

Summary Web APIs are HTTP-based services that any software application can use to access and possibly manipulate data. Web APIs are designed to work by using common communication standards (protocols), given sets of available operations (specifications), and data exchange formats (JSON, XML, and the like). Web APIs are commonly split among four possible scopes of use: Public or open—When anyone has (or can acquire) access Partner—When access is restricted to business associates Internal or private—When access is limited to the organization’s network Composite—When they orchestrate multiple public, partner, and/or internal API calls To fulfill their role, web APIs require a uniform set of rules, constraints, and formats that depend on the chosen architectural style or messaging protocol. The most-used standards nowadays are REST, SOAP, gRPC, and GraphQL: Each of which has distinctive characteristics, tradeoffs, and supported formats. This book focuses mostly on REST, but chapter 10 is dedicated to GraphQL. ASP.NET Core is the high-performance, cross-platform, open source web development framework that we will use to design and develop web APIs throughout this book: ASP.NET Core’s modern architectural principles allow developers to build lightweight, highly modular apps with a great level of testability and maintainability. ASP.NET Core architecture allows developers to customize the app’s capabilities and workloads by using discrete and reusable components to perform specific tasks. These components fall into two main categories, both of which are registered and configured in the app’s entry point (the Program.cs file):

Services—Required to provide functionalities and instantiated through dependency injection Middleware—Responsible for handling the HTTP request pipeline ASP.NET Core supports two paradigms for building web APIs: controllers, which offer great versatility and a full set of supported features, and minimal APIs, a simplified approach that allows us to write less code while reducing overhead: The minimal API approach, introduced in .NET 6, can be a great way to build simple web APIs in a time-efficient fashion and is great for developers who are looking for a sleek, modern programming approach. ASP.NET Core web apps can overcome most performance problems due to resource-intensive, thread-blocking calls by using TAP, an asynchronous programming model that allows the main thread to start tasks in a nonblocking way and resume execution upon their completion.
