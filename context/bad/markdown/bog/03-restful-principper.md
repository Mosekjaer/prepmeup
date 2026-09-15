---
title: RESTful principles and guidelines
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 3
---

# 3. RESTful principles and guidelines

**This chapter covers**

- Reviewing the six REST guiding constraints
- Setting up and configuring CORS and caching techniques in
- ASP.NET Core
- Understanding the role of reverse proxies and CDN services
- Implementing code on demand with a use-case example
- Adopting a uniform interface with HATEOAS capabilities
- Adding API documentation and versioning with
- Swagger/OpenAPI

Now that we have a minimal web API boilerplate up and running, we’re ready to review the representational state transfer (REST) properties and constraints briefly introduced in chapter 1 and see how they can be implemented in ASP.NET Core. Specifically, we’ll add some built-in and third-party services and middleware to our existing MyBGList project to achieve true RESTful status. The chapter introduces concepts such as separation of concerns, state management, caching, idempotency, API versioning, Hypermedia as the Engine of Application State (HATEOAS), and Cross-Origin Resource Sharing (CORS). Rest assured that we won’t dwell too much on theory. This chapter shows how to put these topics into practice in ASP.NET Core.

By the end of the chapter, you’ll be able to test your knowledge by solving some wrap-up exercises on the concepts and programming techniques discussed here. The main goal is to understand the differences between a REST-based web application and a true RESTful web API, paving the way for what comes next.

## 3.1 REST guiding constraints

Let’s start by revisiting the six REST guiding constraints. For the sake of simplicity, we’ll follow the same order used in chapter 1.

### 3.1.1 Client-server approach

To enforce this constraint, we must separate the concerns of the server from those of the clients. In practical terms, our web API

Can respond only to requests initiated by the clients, thus being unable to make requests on its own Has no constraints or dependencies on the location, environment, technology, architecture, and underlying implementation of the individual clients, thus is able to grow, evolve, experience change, and even be rebuilt from scratch without knowing anything

If we look at our existing web API project, we can see that this approach is mostly already in place. Controllers and Minimal APIs are meant only to return standard HTTP responses; they don’t need to know anything about the requesting client. The default ASP.NET Core settings, however, enforce an HTTP security mechanism that might restrict some clients’ locations and/or technologies. This mechanism is a fundamental topic for any web developer.

Cross-origin resource sharing CORS is an HTTP header-based mechanism proposed in 2004 to allow safe cross-origin data requests by VoiceXML browsers. Later, it was formalized as a working draft by the WebApps Working Group of the World Wide Web Consortium (W3C), with participation by the major browser vendors, which started to implement it around 2009; then the draft was accepted as a W3C recommendation in January
2014. The current CORS specification, implemented in all major browsers, is included in the Fetch Living Standard of the Web Hypertext Technology Working Group (WHATWG).

DEFINITION WHATWG is a community of people who are interested in evolving HTML and related technologies. The working group was founded in 2004 by representatives of Apple Inc., the Mozilla Foundation, and Opera Software.

The purpose of CORS is to allow browsers to access resources by using HTTP requests initiated from scripts (such as XMLHttpRequest and Fetch API) when those resources are located in domains other than the one hosting the script. In the absence of such a mechanism, browsers would block these “external” requests because they would break the same-origin policy, which allows them only when the protocol, port (if specified), and host values are the same for both the page hosting the script and the requested resource.

TIP For additional info regarding the same-origin policy, check out http://mng.bz/gJaG.

To understand the concept, let’s create a practical example using our current web API scenario. Suppose that we want to use our MyBGList web API, which we previously published to the mybglist-api.com domain, to feed a board-gaming web app created with a JavaScript framework such as Angular and located in a different domain (such as webapp.com). The client’s implementation relies on the following assumptions:

The browser client performs an initial HTTP GET request to the webapp.com/ index.xhtml page, which loads the Angular app. The webapp.com/index.xhtml page returns an HTTP 200 - OK response containing the HTML content, which includes two additional references: A <script> element pointing to a /app.js file, which contains the web app’s Angular JavaScript code. A <link> element pointing to a /style.css file, which contains the web app’s UI styles. When retrieved, the /app.js file autostarts the Angular app, which performs an XMLHttpRequest to the mybglist- api.com/BoardGames endpoint, handled by our MyBGList web API’s BoardGamesController, to retrieve a list of board games to display.

Figure 3.1 shows these requests.
Figure 3.1 Same-origin and cross-origin client-server interactions

As we can see, the first HTTP request—the webapp.com/index.xhtml page—points to the webapp.com domain. This initial request defines the origin. The subsequent two requests—/app.js and /style.css—are sent to the same origin as the first one, so they are same-origin requests. But the last HTTP request—mybglist-api.com/BoardGames —points to a different domain, violating the same-origin policy. If we want the browser to perform that call, we can use CORS to instruct it to relax the policy and allow HTTP requests from external origins.

NOTE It’s important to understand that the same-origin policy is a security mechanism controlled by the browser, not by the server. In other words, it’s not an HTTP response error sent by the server because the client is not authorized; it’s a block applied by the client after completing an HTTP request, receiving the response, and checking the headers returned with it to determine what to do. Starting from this assumption, we can easily understand that the CORS settings sent by the server have the function of telling the client what to block and what not to block.

All modern browsers use two main techniques to check for CORS settings and apply them: simple mode, which uses the same HTTP request/response used to fetch the accessed resource, and preflight mode, which involves an additional HTTP OPTIONS request. The next section briefly explains the differences between these approaches.

A practical guide to CORS For additional info about CORS from both the server and client perspectives, check out CORS in Action: Creating and Consuming Cross-Origin APIs, by Monsur Hossain (https://www.manning.com/books/cors-in-action).

Simple requests vs. preflight requests Whenever a client-side script initiates an HTTP request, the browser checks whether it meets a certain set of requirements—such as using standard HTTP methods, headers, and content—that can be used to define it as simple. If all these requirements are met, the HTTP request is processed as normal. CORS is handled by checking the Access-Control-Allow-Origin header and ensuring that it complies with the origin of the script that issued the call. This approach is a simple request.

NOTE The requirements that define a simple request are documented at http://mng.bz/wyKa.

In case the HTTP request doesn’t meet any of the requirements, the browser puts it on hold and automatically issues a preemptive HTTP OPTIONS request before it. The browser uses this preflight request to determine the exact CORS capabilities of the server and act accordingly.

As we can easily understand, an important difference between a simple request and a preflight request is that the former directly asks for (and receives) the content we want to fetch, whereas the latter asks the server for permission before issuing the request.

Preflight requests provide an additional security layer because CORS settings are checked and applied beforehand. A preflight request uses three headers to make the server aware of the characteristics of the subsequent request:

Access-Control-Request-Method—The HTTP method of the request Access-Control-Request-Headers—A list of custom headers that will be sent with the request Origin—The origin of the script initiating the call

The server, if configured to handle this kind of request, will answer with the following HTTP response headers to indicate whether the subsequent HTTP request will be allowed:

Access-Control-Allow-Origin—The origin allowed to make the request (or the * wildcard if any origin is allowed). This is the same header used by simple requests. Access-Control-Allow-Headers—A comma- separated list of allowed HTTP headers. Access-Control-Allow-Methods—A comma- separated list of allowed HTTP methods. Access-Control-Max-Age—How long the results of this preflight request can be cached (in seconds).

The browser checks the preflight request’s response values to determine whether to issue the subsequent HTTP request.

NOTE When implementing a web app by using a client-side framework such as Angular or ReactJS, software developers typically don’t need to delve into the technical details of CORS; the browser transparently takes care of all that and communicates with the server in the appropriate way. Because we’re dealing with the server-side part of the story, however, we need to know what to allow and what not to allow. That’s enough theory. Let’s see how we can implement CORS in our web API project.

Implementing CORS In ASP.NET Core, CORS can be set up via a dedicated service, which gives us the chance to define a default policy and/or various named policies. As always, such a service must be added in the service container in the Program.cs file.

We can use different policies to allow CORS for specific origins, HTTP headers (for preflight requests, explained earlier) and/or methods. If we want to allow cross-origin requests for all origins, headers, and methods, we could add the service by using the following settings:

```csharp
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(cfg => {
         cfg.AllowAnyOrigin();
         cfg.AllowAnyHeader();
         cfg.AllowAnyMethod();
    }));
```

Configuring CORS this way, however, means disabling the same- origin policy for all the endpoints that will adopt the default policy, thus posing nontrivial security problems. For that reason, it would be safer to define a more restrictive default policy, leaving the relaxed settings for a named policy. The following code shows how we can achieve that goal by defining two policies:

A default policy that accepts every HTTP header and method from a restricted set of known origins, which we can safely set whenever we need to An "AnyOrigin" named policy that accepts everything from everyone, which we can use situationally for a limited set of endpoints that we want to make available for any client, including a client we are not aware of

Here’s how we can improve the snippet in the Program.cs file:

```csharp
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(cfg => {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
    options.AddPolicy(name: "AnyOrigin",
        cfg => {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
    });
```

The value that we pass to the WithOrigins() method will be returned by the server within the Access-Control-Allow- Origin header, which indicates to the client which origin(s) should be considered valid. Because we used a configuration setting to define the origins to allow for the default policy, we need to set them up. Open the appSettings.json file, and add a new "AllowedOrigins" key, as shown in the following listing.

**Listing 3.1 appSettings.json file**

```json
{
    "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
    },
    "AllowedHosts": "*",
    "AllowedOrigins": "*",
    "UseDeveloperExceptionPage": false
}
```

In this example, we used a literal value of "*"—a value that can be used as a wildcard to allow any origin to access the resource when the request has no credentials. If the request is set to allow credentials such as cookies, authorization headers, or TLS client certificates, the "*" wildcard can’t be used and would result in an error.

WARNING Such behavior, which requires both the server and the client to acknowledge that it’s OK to include credentials in requests and to specify a specific origin, is enforced to reduce the chance of Cross-Site Request Forgery (CSRF) vulnerabilities in CORS. The reason is simple: requests with credentials are likely used to read restricted data and/or perform data updates, so they require additional security measures.

Now that we’ve defined our CORS policies, we need to learn how to apply them.

Applying CORS ASP.NET Core gives us three ways to enable CORS:

The CORS middleware Endpoint routing The [EnableCors] attribute

CORS middleware is the simplest technique to use, as it applies the selected CORS policy to all the app’s endpoints (Controllers, Minimal APIs, and the like). In our scenario, it can be useful to globally set the default policy that we defined earlier. Set it up by adding the following line of code to our Program.cs file right before the Authorization middleware:

// ...

app.UseCors();

app.UseAuthorization();

// ...

We have to put that code there for a reason: the order in which middleware components are added to the Program.cs file defines the order in which they are invoked. If we want the CORS middleware to be applied to the endpoints handled by our controllers and Minimal APIs, as well as taken into account by our global authorization settings, we need to add it before any of those endpoints.

NOTE For additional info on middleware order and best practices, read the following page of the ASP.NET Core official documentation: http://mng.bz/qop6.

In case we want to apply the "AnyOrigin" named policy to all our endpoints instead of the default policy, we could add the CORS middleware by specifying the policy name in the following way:

// ...

app.UseCors("AnyOrigin");

// ... This approach would make no sense, however, because that named policy is clearly meant to be applied only in some edge-case scenarios when we want to relax the same-origin policy for all origins. That’s definitely not the case for our BoardGamesController’s action methods unless we’re OK with letting any web app consume it without restrictions. But this approach might be acceptable for the /error and /error/test routes, currently handled by Minimal APIs. Let’s apply the "AnyOrigin" named policy to them by using the endpoint routing method:

// ...

```csharp
// Minimal API
app.MapGet("/error", () => Results.Problem())
    .RequireCors("AnyOrigin");
app.MapGet("/error/test", () => { throw new Exception("test"); })
    .RequireCors("AnyOrigin");
```

// ...

As we can see, endpoint routing allows us to enable CORS on a per- endpoint basis by using the RequireCors() extension method. This approach is good for nondefault named policies because it gives us better control in choosing the endpoints that support them. If we want to use it for controllers instead of Minimal API methods, however, we won’t have the same level of granularity, because it could be applied only globally (to all controllers):

// ...

```csharp
app.MapControllers()
    .RequireCors("AnyOrigin");
```

// ... Furthermore, enabling CORS by using the RequireCors() extension method currently doesn’t support automatic preflight requests, for the reason explained at https://github.com/dotnet/aspnetcore/issues/20709.

For all these reasons, endpoint routing currently isn’t the suggested approach. Luckily, the same granularity is granted by the third and last technique allowed by ASP.NET Core: the [EnableCors] attribute, which is also the Microsoft-recommended way to implement CORS on a per-endpoint basis. Here’s how we can implement it in our Minimal API method, replacing the previous endpoint routing technique based on the RequireCors() extension method:

// ...

```csharp
// Minimal API
app.MapGet("/error", [EnableCors("AnyOrigin")] () =>
    Results.Problem());
app.MapGet("/error/test", [EnableCors("AnyOrigin")] () =>
    { throw new Exception("test"); });
```

// ...

To use the attribute, we also need to add a reference to the Microsoft.AspNetCore .Cors namespace at the start of the Program.cs file in the following way:

using Microsoft.AspNetCore.Cors;

A big advantage of the [EnableCors] attribute is that it can be assigned to any controller and/or action method, allowing us to implement our CORS named policies in a simple, effective way.
NOTE Using the [EnableCors] attribute without specifying a named policy as a parameter will apply the default policy, which would be redundant in our scenario because we’ve already added it on a global basis by using CORS middleware. Later, we’ll use such an attribute to override the default policy with a named policy whenever we need to.

### 3.1.2 Statelessness

The statelessness constraint is particularly important in RESTful API development because it prevents our web API from doing something that most web applications do: store some of the client’s info on the server and retrieve it on subsequent calls (using a session cookie or a similar technique). In more general terms, enforcing a statelessness approach means that we have to restrain ourselves from using convenient ASP.NET Core features such as session state and application state management, as well as load-balancing techniques such as session affinity and sticky sessions. All the session-related info must be kept entirely on the client, which is responsible for storing and handling them on its own side. This capability is typically handled by frontend state management libraries such as Redux, Akita, ngrx, and Elf.

In our current scenario, because we used a minimal ASP.NET Core web API template without authentication support, we can say that we’re already compliant. Our current project doesn’t have the required services and middleware (available mostly through the built- in Microsoft.AspNetCore.Session namespace) to enable session state. This fact also means that we won’t be able to benefit from these convenient techniques if we need to authenticate specific calls and/or restrict some endpoints to authorized clients, such as adding, updating, or deleting board games or reading some reserved data. Whenever we want to do those things, we have to provide the client with all the required information to create and maintain a session state on its side. We’ll learn how to do that in chapter 9, which introduces JSON Web Token (JWT), token-based authentication, and other RESTful techniques.

Session state and application state, as well as any data stored by the server to identify requests, interactions, and context information related to clients, has nothing to do with resource state or any other states related to the response returned by the server. These types of states are not only allowed within a RESTful API but also constitute a required constraint, as discussed in the next section.

### 3.1.3 Cacheability

The term cache, when used in an IT-related context, refers to a system, component, or module meant to store data to make it available for further requests with less effort. When dealing with HTTP-based web applications, caching is typically intended to store frequently accessed (requested) content in various places within the request-response lifecycle. Within this context, most available caching techniques and mechanisms can be divided into three main groups:

Server-side caching (also known as application caching)—A caching system that saves data to key/value stores by using either a built-in service or a third-party provider such as Memcached, Redis, Couchbase, managed abstraction layers such as Amazon ElastiCache, or a standard database management system (DBMS) Client-side caching (also known as browser caching or response caching)—A caching mechanism defined in the HTTP specifications that relies on several HTTP response headers (including Expires, Cache-Control, Last-Modified, and ETag) that can be set from the server to control the caching behavior of the clients Intermediate caching (also known as proxy caching, reverse- proxy caching, or content-delivery network [CDN] caching)—A response-caching technique that stores cached data by using dedicated services (proxies) and/or relies on third-party services optimized for accelerated and geographically distributed content distribution (CDN providers), following the same HTTP header- based rules as client-side caching

In the following sections, we’ll set up and configure these caching methods by using some convenient ASP.NET Core features. We’ll also explore most of them in chapter 8.

Server-side caching Being able to store frequently accessed data in high-performance storage (such as system memory) is traditionally considered to be a valid implementation technique for a web application, because it allows us to preserve a data provider—a DBMS, a network resource, or the filesystem—from being stressed by a high number of simultaneous requests. But because most of these data sources now come with their own caching features and mechanisms, the idea of building a centralized, application-level cache is less and less attractive, especially when dealing with highly decentralized architectural styles such as service-oriented architecture (SOA) and microservices. In general terms, we can say that using several caching methods provided by different services is preferable, because it allows us to fine-tune the caching requirements for each data source, often leading to better overall performances.

That said, there are several scenarios in which server-side caching might be a viable option. Because we plan to store our board-game data in a dedicated DBMS, we could think about caching the results of some frequently used performance-heavy database queries, assuming that the retrieved data doesn’t change too often over time. We’ll talk more about that topic in chapter 8, which introduces some database optimization strategies based on the Microsoft.Extensions.Caching.Memory NuGet package; until then, we’ll focus on client-side caching.

Client-side caching Unlike server-side caching, which is used mostly to store data retrieved from backend services and DBMS queries, client-side caching focuses on the content served by web applications: HTML pages, JSON outputs, JavaScript, CSS, images, and multimedia files. That kind of content is often called static because it’s typically stored in the filesystem using standard text or binary files. This definition isn’t always formally correct, however, because most HTML and JSON content is retrieved dynamically and then rendered on the fly by the server right before being sent along with the HTML response. That’s the case with the JSON data sent by our web API.

A major advantage of client-side caching is that, as its name implies, it requires no server-side activity because it fully satisfies the HTTP request to which it applies. This behavior can lead to tremendous advantages in terms of performance, latency, and bandwidth optimization and greatly reduces the server-side load. These benefits are clearly stated in the HTTP/1.1 specifications (RFC 2616 Section 13, “Caching in HTTP,” https://www.rfc-editor.org/rfc/rfc2616#section- 13):

The goal of caching in HTTP/1.1 is to eliminate the need to send requests in many cases and to eliminate the need to send full responses in many other cases. The former reduces the number of network round-trips required for many operations; [...] The latter reduces network bandwidth requirements; [...]

For all these reasons, we can say that client-side caching is the most important caching technique to implement when we work with any HTTP-based application, especially if we’re dealing with a RESTful interface.

Response caching As we already know, response caching is controlled by HTTP headers that specify how we want the client—as well as any intermediate proxy, CDN, or another service—to cache each HTTP response. The most relevant is Cache-Control, which can be used to specify several caching directives explaining who can cache the response (public, private, no-cache, no-store), the cache duration (max-age), stale-related info (max-stale, must-revalidate) and other settings. Ideally, such directives are honored by all the caching systems and services along the request/response chain.

ASP.NET Core gives us the chance to configure these headers (and their directives) by using the [ResponseCache] attribute, which can be applied to any controller or Minimal API method. The [ResponseCache] attribute can be configured by using the following properties:

Duration—Determines the max-age value of the Cache- Control header, which controls the duration (in seconds) for which the response is cached. Location—Determines who can cache the response: Any if both clients and proxies are allowed to, Private to allow clients only, or None to disable it. These values respectively set the public, private, or no-cache directive in the Cache-Control header. NoStore—When set to true, sets the Cache-Control header value to no-store, thus disabling the cache. This configuring is typically used for error pages because they typically contain unique info for the specific request that raised the error—info that would make no sense to cache.

TIP The [ResponseCache] attribute is part of the Microsoft.AspNetCore.Mvc namespace. For that reason, we need to add a reference in the Program.cs file (for Minimal APIs) and/or in the controller files where we want to use it.

Let’s see how we can implement this attribute in our existing method, starting with the Minimal API error handlers in the Program.cs file. Because we’re dealing with error response messages, we can use this opportunity to use the NoStore property to prevent anyone from caching them in the following way:

```csharp
// ...
app.MapGet("/error",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
    Results.Problem());
app.MapGet("/error/test",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
    { throw new Exception("test"); });
```

// ...

We can use a different approach with our BoardGamesController because it’s meant to return a list of board games that we may want to cache for a reasonable amount of time. Here’s how we can set up a public cache with a max-age of 60 seconds for that response:

// ...

[HttpGet(Name = "GetBoardGames")] [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)] public IEnumerable<BoardGame> Get()

// ...

That’s enough for now. We’ll define further response-caching rules whenever we add other action methods.

Intermediate caching From a web development perspective, intermediate caching is similar to client-side caching, because both techniques store data outside the server following the rules specified in the HTTP response headers. For that reason, the implementation that we’ve pulled off will work seamlessly for both of them, assuming that we set the Location property to Public, as explained earlier. The main differences between the two caching methods are set on the architectural level. The term intermediate means that cached resources are stored in a server (or services) located between the client and the server instead of on the former’s local drive. This approach involves three important concepts:

Each cached resource can be used to serve multiple clients. The intermediate cache is also a shared cache, because the same cached response can be issued by several HTTP requests coming from different peers. The intermediate caching server must sit between the client and the server so that it can answer each incoming call by serving a cached response (without calling the server) or forwarding it to the server (and possibly caching it for further requests). The whole caching mechanism is not visible to clients, to the point that they’re mostly unable to tell whether the response comes from the original server or from the cache (unless the owner wants to explicitly make them aware).

Figure 3.2 shows how intermediate caching is intended to work and how it can “stack” with client caching. These concepts are true for proxies, reverse proxies, and CDN services, regardless of where they’re physically (or logically) located.
Figure 3.2 Client-side caching and intermediate caching at a glance

From a technical point of view, we can see that the intermediate caching service works like the browser’s local cache, sitting between requests and responses, and acting according to the HTTP headers. The scalability performance gains are considerably higher, however, especially when we’re dealing with multiple simultaneous requests for the same content. Such benefits are of utmost importance for most web applications and deserve the added complexity of setting up and configuring a dedicated server or service.

WARNING When implementing intermediate caching, we are basically creating “cached copies” of some URL-related content (HTML pages, JSON data, binary files, and so on) that typically become accessible regardless of any authentication and authorization logic used by the server to render that content in the first request/response cycle. As a general rule, intermediate caching services should be used to cache only publicly available content and resources, leaving restricted and personal data to private caching approaches (or no-cache) to prevent potentially critical data security problems.

### 3.1.4 Layered system

This constraint further employs the separation-of-concerns principle already enforced by the client-server approach by applying it to various server components. A RESTful architecture can benefit greatly from a service-distributed approach, in which several (micro)services work together to create a scalable and modular system. We’ve embraced such a pattern in our concrete scenario, because our board-game API is part of a wider SOA ecosystem with at least two server-side components: the web API itself, which handles the incoming HTTP requests and the whole data exchange process with the third parties, and the DBMS, which provides the data securely. Ideally, the web API and the DBMS can be deployed on different servers located within the same web farm or even different server farms.

Starting from that setting, we can further expand the layered system concept by adding two additional, decentralized architectural components: a reverse-proxy server, which will be installed in a virtual machine under our control, and a CDN service hosted by a third-party provider such as Cloudflare. Figure 3.3 shows the updated architectural SOA diagram with the new components.
Figure 3.3 Updated MyBGList SOA using CDN and reverse proxy

The web API still plays the same pivotal role, but we’ve put two additional layers between the API server and the clients, which will definitely increase the overall performance and availability of our system under heavy load:

The reverse proxy will allow us to implement an efficient intermediate caching mechanism, as well as pave the way for several load balancing techniques (such as the edge-origin pattern) to improve the horizontal scalability of our app. The CDN service will add an intermediate caching layer, lower network latency for most countries and regions, and contribute to cost savings by reducing the server’s bandwidth use.

We’ll actively implement this plan in chapter 12 when we deploy our app in production.

### 3.1.5 Code on demand

When Roy Fielding wrote his dissertation on REST, JavaScript was still in its early days, the AJAX acronym didn’t exist, and the XMLHttpRequest model was unknown to most developers. It wasn’t easy to imagine that over the next few years, we would witness a real revolution following the advent of thousands of JavaScript-powered libraries (JQuery) and web frameworks (Angular, React, Vue.js, and more).

The code on demand (COD) optional constraint is easier to understand now than it was before. In a nutshell, we can describe it as the ability of a RESTful API to return executable code, such as JavaScript, to provide the client with additional capabilities. It’s precisely what happens when we use a <script> element within an HTML page to retrieve a combined JavaScript file containing, say, an Angular app. As soon as the file is downloaded and executed, the browser loads an app that can render UI components, interact with the user, and even perform further HTTP requests to get additional content (or code).

In our given scenario, we’ll hardly have the chance to adhere to this optional constraint, because we’ll want to return mostly JSON data. Before putting this topic aside, however, I’ll show how we can implement COD by using ASP.NET Core and Minimal APIs with only a few lines of source code.

Suppose that our development team has been asked to provide an endpoint that can be used to check whether the calling client supports JavaScript and provide visual proof of the result. That scenario is a perfect chance to develop an endpoint that can return some JavaScript COD.

We’ll set up a /cod/test/ route that will respond with some HTML containing a <script> element, with the JavaScript code to render a visual alert in case of success and a <noscript> tag to render a text-only message in case of failure. Here’s a Minimal API method that we can add to our Program.cs file right below the other MapGet methods that we’ve added:

app.MapGet("/cod/test", [EnableCors("AnyOrigin")] [ResponseCache(NoStore = true)] () => Results.Text("<script>" + "window.alert('Your client supports JavaScript!" + "\\r\\n\\r\\n" + $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" + "\\r\\n" + "Client time (UTC): ' + new Date().toISOString());" + "</script>" + "<noscript>Your client does not support JavaScript</noscript>", "text/html")); As we can see, we also used the [EnableCors] and [ResponseCache] attributes to set up some CORS and caching rules. We want to enable CORS (because we used the "AnyOrigin" named policy), but we don’t want the HTTP response to be cached by clients or proxies.

TIP The [ResponseCache] attribute requires adding the following reference at the top of the Program.cs file: using Microsoft.AspNetCore.Mvc.

To test what we did, launch the project in Debug mode and navigate to the following URL, which corresponds to our newly added endpoint: https://localhost:40443/cod/test. If we did everything properly, and if our browser supports JavaScript, we should get the “visual” result shown in figure 3.4.
Figure 3.4 COD testing with JavaScript enabled

The alert window rendered by the sample JavaScript code we sent to the client is a good demonstration of COD because it shows both server-computed and client-computed data. While we’re here, we might as well test the <noscript> outcome as well. Press Ctrl+Shift+I (or F12) to access the browser’s developer console; then click the cog icon to access the Settings page, and select the Disable JavaScript check box (figure 3.5).
Figure 3.5 Disabling JavaScript in Microsoft Edge

WARNING These commands assume that we’re using a Chromium- based browser, such as Microsoft Edge or Google Chrome. Different browsers/engines require different methods. In Mozilla Firefox, for example, we can turn off JavaScript by typing about:config in the search bar, accepting the disclaimer, and changing the javascript.enabled toggle value from true to false.

Right after that, without closing the Settings window, press F5 to issue a reload of the /cod/test/ endpoint. Because JavaScript is now disabled (temporarily), we should see the negative outcome shown in figure 3.6.
Figure 3.6 COD testing with JavaScript disabled

### 3.1.6 Uniform interface

The RESTful constraint I’ve left for last is also the one that often takes the longest to implement properly, as well as being arguably the most difficult to understand. Not surprisingly, the best definition of a uniform interface is given by Roy Fielding in section 5.1.5 of his dissertation “Architectural Styles and the Design of Network-based Software Architectures” (http://mng.bz/eJyq), in which he states The central feature that distinguishes the REST architectural style from other network- based styles is its emphasis on a uniform interface between components. By applying the software engineering principle of generality to the component interface, the overall system architecture is simplified and the visibility of interactions is improved. Implementations are decoupled from the services they provide, which encourages independent evolvability.

This statement implies that a RESTful web API, along with the data, should provide the clients with all the actions and resources they need to retrieve related objects, even without knowing them in advance. In other words, the API must not only return the requested data, but also make the client aware of how it works and how it can be used to request further data. But how can we make the client understand what to do?

Adopting a uniform interface is what we can do. We can think of a uniform interface as being standardized, structured, machine-friendly self-documentation included with any HTTP response, explaining what has been retrieved and (most important) what to do next.

NOTE Fielding’s statement and the whole uniform interface concept have long been misunderstood and underestimated by many web developers, who preferred to focus on the other REST constraints. Such behavior started to become common in the 2000s, eventually pushing Fielding to revamp the subject in a famous 2008 post on his personal blog (http://mng.bz/7ZA7).

REST specifications don’t force developers to adopt a specific interface. But they do provide four guiding principles that we should follow to implement a viable uniform interface approach: Identification of resources—Each individual resource must be univocally identified, such as by using universal resource identifiers (URIs) and represented by a standard format (such as JSON). Manipulation through representations—The representation sent to the client should contain enough info to modify or delete the resource, as well as add further resources of the same kind if the client has permission to do so. Self-descriptive messages—The representation sent to the client should contain all the required info to process the received data. We can do this by adding relevant info with JSON (which HTTP method to use, which MIME type to expect, and so on), as well as using HTTP headers and metadata (for caching info, character sets, and the like). HATEOAS—Clients should be able to interact with the application without any specific knowledge beyond a generic understanding of hypermedia. In other words, manipulation through representation should be handled (and documented) only by means of standard descriptive links.

To understand these concepts better, let’s adapt them to our current scenario. Our BoardGamesController currently returns the following JSON structure:

[ { "id": <int>, "name": <string>, "year": <int> } ] This structure means that whenever our clients request a list of board games, our web API responds with the objects they’re looking for . . . and nothing more. What it should do instead—assuming that we want to adopt the uniform interface REST constraint—is to return those objects as well as some descriptive links to inform the clients how they can alter those resources and request further resources of the same kind.

Implementing HATEOAS Our current BoardGamesController’s Get() action method currently returns a dynamic object containing an array of objects created with the BoardGame Plain Old CLR Object (POCO) class (the BoardGame.cs file). We made that choice in chapter 2, using a handy C# feature (anonymous types) to serve data quickly and effectively with our ASP.NET Core web API. If we want to adopt a uniform interface to return data and descriptive links, however, we need to switch to a more structured approach.

NOTE For more info regarding C# anonymous types, see the official docs at http://mng.bz/m2MW.

We want to replace the current anonymous type with a base data- transfer object (DTO) class that can contain one or more records of a generic type, as well as the descriptive links that we want to provide the client. A generic type is a placeholder for a specific type that can be defined whenever an instance of that object is declared and instantiated. A perfect example is the built-in List<T> type, which we can use in C# to declare and instantiate a constructed type list by specifying a specific type argument inside the angle brackets: var intList = new List<int>(); var stringList = new List<string>(); var bgList = new List<BoardGame>();

This snippet creates three separate type-safe objects with a single class definition, thanks to the fact that the class accepts a generic type. That’s precisely why we need to create our uniform interface DTO.

DEFINITION DTOs should be familiar to most web developers. In a nutshell, DTOs are POCO classes that can be used to expose relevant info about that object only to the requesting client. The basic idea is to decouple the response data from the data returned by the Data Access layer. I talk more about this concept in chapter 4 when we replace our code sample with an actual data provider powered by SQL Server and Entity Framework Core.

Create a new /DTO/ folder in the project’s root. We’ll put all the DTOs in that folder from now on. Then use Visual Studio’s Solution Explorer to add two new files:

LinkDTO.cs—The class that will host our descriptive links RestDTO.cs—The class containing the data and the links that will be sent to the client

Both classes have a simple structure. The RestDTO class is a container for the <T> generic type, which will host the actual data, and the LinkDTO class will contain the descriptive links. The following listing shows the code for the LinkDTO class.

**Listing 3.2 LinkDTO.cs file namespace MyBGList.DTO { public class LinkDTO { public LinkDTO(string href, string rel, string type) { Href = href; Rel = rel; Type = type; }**

```csharp
          public string Href { get; private set; }

          public string Rel { get; private set; }

          public string Type { get; private set; }
      }
}
```

Listing 3.3 shows the code for the RestDTO class.

**Listing 3.3 RestDTO.cs file**

```csharp
namespace MyBGList.DTO
{
    public class RestDTO<T>
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();

          public T Data { get; set; } = default!;
      }
}
```

C generic classes, methods, and type parameters For additional info regarding C# generic classes, methods, and type parameters, see the following official docs:

http://mng.bz/5mV8 http://mng.bz/69xp

Now that we have these two classes, we can refactor our BoardGamesController’s Get() action method to use them, replacing our existing implementation. We want to change the existing anonymous type with the RestDTO class and use the LinkDTO class to add the descriptive links in a structured way. Open the BoardGamesController.cs file, and perform these updates in the following way:

public RestDTO<BoardGame[]> Get()           ❶
{
    return new RestDTO<BoardGame[]>()       ❷
    {
        Data = new BoardGame[] {
            new BoardGame() {
                Id = 1,
                Name = "Axis & Allies",
                Year = 1981
            },
            new BoardGame() {
                Id = 2,
                Name = "Citadels",
                Year = 2000
            },
            new BoardGame() {
                Id = 3,
                Name = "Terraforming Mars",
                Year = 2016
            }
        },
         Links = new List<LinkDTO> {      ❸
             new LinkDTO(
                 Url.Action(null, "BoardGames", null, Request.Scheme)!,
                 "self",
                 "GET"),
         }
    };
}

❶ Changes the return value
❷ Changes the anonymous type with RestDTO
❸ Adds the descriptive links

We’ve made three main changes:

Replaced the previous IEnumerable<BoardGame> return value with a new RestDTO<BoardGame[]> return value, which is what we’re going to return now. Replaced the previous anonymous type, which contained only the data, with the new RestDTO type, which contains the data and the descriptive links. Added the HATEOAS descriptive links. For the time being, we support a single board-game-related endpoint; therefore, we’ve added it by using the "self" relationship reference.

NOTE This HATEOAS implementation requires us to add our descriptive links manually to each action method. We did that for the sake of simplicity, because it’s a great way to understand the logic of what we’ve done so far. But if we adopt a common development standard throughout all our controllers and action methods, we could automatically fill the RestDTO.Links property by using a helper or factory method and a bunch of parameters.

If we run our project now and call the /BoardGames/ endpoint, here’s what we’ll get:

{ "data": [ { "id": 1, "name": "Axis & Allies", "year": 1981 }, { "id": 2, "name": "Citadels", "year": 2000 }, { "id": 3, "name": "Terraforming Mars", "year": 2016 } ], "links": [ { "href": "https://localhost:40443/BoardGames", "rel": "self", "type": "GET" } ] }

Our uniform interface is up and running. We need to adhere to it from now on, improving and extending it as necessary so that the client always knows what to do with our given data.

NOTE For the sake of simplicity, we won’t refactor our existing Minimal API methods to use the RestDTO type, as those methods are meant only for testing purposes. We’ll remove them from our codebase as soon as we learn how to handle actual errors properly.

Classes or records? Instead of creating our DTOs by using the standard C# class type, we could have used the relatively new C# record type (introduced in C# 9). The main difference between these two class types is that records use value-based equality, meaning that two record variables are considered to be equal if all their field values are equal. Conversely, two class variables are considered to be equal only if they have the same type and refer to the same object. C# record types can be a great replacement of standard class types for defining DTOs, because value-based equality can be useful for dealing with object instances representing JSON data. Because we won’t need such a feature in our scenario, we created our first DTOs by using “good old” class types.

TIP For further information on C# record types, I strongly suggest checking out the following Microsoft Docs tutorial: http://mng.bz/GRYD.

This section concludes our journey through the REST constraints. Now we’ve got all the required knowledge to stick to these best practices throughout the rest of the book. The following sections introduce a couple of topics that aren’t strictly related to any of the REST constraints but can be useful for improving the overall usability and readability of our web API:

API documentation, intended to expose the use of our API in a human-readable way for developers and nondevelopers API versioning, which keeps a history of the various releases and performs breaking changes without disrupting existing integrations with the clients

These two features, when implemented correctly, can greatly improve the effectiveness of a web API because they directly affect development and consumption time, increasing customer satisfaction and reducing overall cost. Implementing each feature requires a different set of efforts and tasks, which I cover in the following sections.

## 3.2 API documentation

Web-based products tend to evolve quickly, not only during the development phase, but also between subsequent deployment cycles after the product is released. Continuous (and inevitable) improvement is a proven, intrinsic, almost ontological characteristic of technology, and the main reason why Agile methodologies are frequently used to deal with it. Fixing bugs, adding new features, introducing security requirements, and dealing with performance and stability problems are tasks that any web project will have to deal with to succeed.

Web APIs are no exception. Unlike standard websites or services, however, they don’t have a user interface that can show changes and make users aware of them. If a WordPress blog adds a comments section to posts, for example, there’s a high chance that visitors will see the new feature, learn how it works, and start using it right away. If we do the same with our web API, maybe adding a brand-new CommentsController with a set of endpoints, that’s not likely going to happen. Chances are low that the various developers who work with the clients and services that interact with our data will notice the new feature. The same would happen if we make some changes or improvements to an existing feature. If that blog chooses to allow visitors to alter their comments after they’ve been sent, a new Edit button will be enough. If we add an Edit() action method to a controller, however, no one will know unless we find a way to make the interested parties aware of the fact. That’s why we should find a way to document our API.

In an ever-changing environment, this activity comes with a cost. Maintaining and updating this documentation for our development team, as well as any external third party that expects to consume our API and/or integrate with it, can become difficult and expensive unless we find a way to do the work automatically, with minimal effort. That task is precisely what OpenAPI (previously known as Swagger) can do.

### 3.2.1 Introducing OpenAPI

The OpenAPI specification is a JSON-based interface description language for documenting, consuming, and visualizing RESTful web services and APIs. It was known as Swagger until 2015, when it was donated to the OpenAPI initiative by the Swagger team. OpenAPI has become the most used, most widely acknowledged documentation standard for RESTful APIs.

NOTE For additional info about Swagger and OpenAPI, check out Designing APIs with Swagger and OpenAPI, by Joshua S. Ponelat and Lukas L. Rosenstock (http://mng.bz/neaV).

The main purpose of OAS is to define a standardized contract between the server and the client, allowing the latter to understand how the former works and to interact with its available endpoints without accessing the source code. Furthermore, such a “contract” is intended to be language agnostic and human readable, allowing both machines and humans to understand what the API is supposed to do. From this perspective, OpenAPI is closely related to uniform interface and HATEOAS, discussed earlier in this chapter. Because it can be used to serve many purposes, it’s advisable to treat it as a separate topic.

### 3.2.2 ASP.NET Core components

The .NET framework provides two OpenAPI implementations that can be used in any ASP.NET Core web application: Swashbuckle and NSwag. For the sake of simplicity, we’ll use Swashbuckle, which is shipped with the Visual Studio ASP.NET Core web API template that we used to create our MyBGList project.

In chapter 2, when we created the web API project with Visual Studio, we chose to keep the Enable OpenAPI support check box selected. That choice allowed us to get the Swashbuckle services and middleware up and running. Check it out by opening the Program.cs file and looking at the following lines of code:

// ...

builder.Services.AddSwaggerGen();      ❶

// ...

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                  ❷
    app.UseSwaggerUI();                ❸
}
```

❶ Swagger JSON generator service
❷ Swagger middleware to serve the generated JSON
❸ Swagger middleware to enable the user interface

The Swagger generator service creates a swagger.json file that describes all the endpoints available within our web API using the OpenAPI specification. The first middleware exposes such a JSON file by using a configurable endpoint (the default is /swagger/v1/swagger.json), and the second enables a handy user interface that allows users to see and browse the documentation. To see the swagger.json file, launch the app in Debug mode and navigate to https://localhost:40443/swagger/v1/swagger.json. To access the SwaggerUI (which uses the JSON file as a documentation source), navigate to https://localhost:40443/swagger.

As we can see by looking at the UI main page, the JSON file was created with OAS version 3.0 (per the Swagger Generator default settings) and takes for granted that we’re dealing with version 1.0 of our API. Most of the default settings, including the URL of the endpoint serving the swagger.json file, can be changed by using the service and middleware options. We can switch from OAS 3.0 to 2.0, add swagger.json files, include custom stylesheet files in the index.xhtml page, and so on.

A few words about Swashbuckle For the sake of simplicity, I won’t dive into Swashbuckle’s configuration settings for now. I’ll talk more about them in chapter 11. For additional info, I suggest looking at the official docs:

http://mng.bz/vX0m http://mng.bz/49r5

In our current scenario, the default settings are good enough, at least for the time being. As we can see by looking at figure 3.7, the SwaggerUI already includes all the endpoints (and return types) that we’ve added since we started to work with our project—and will automatically keep doing that for those we’ll add throughout the rest of the book.
Figure 3.7 SwaggerUI main page for the MyBGList web API project The SwaggerUI already includes all the controller and Minimal API endpoints that we’ve added since we started to work with our project —as well as their DTO return types—and will keep doing that for everything we’ll add throughout the rest of the book.

Each endpoint and schema type shown by the UI can be expanded; use the right handle to see the full documentation info. The detail level is incredibly high, which is why the middleware that exposes the info was set up in the Program.cs file within a conditional block that makes it available only in a development environment. The Visual Studio template is acting conservatively here because it doesn’t know whether we want to share such info publicly. That way, we’ll be able to benefit from the SwaggerUI during the whole development phase without the risk of sharing our whole endpoint structure in case of a “reckless” deploy. We have no reason to change this convenient behavior for the time being, because we don’t plan to publish our app any time soon. All of Swashbuckle’s goodies are already set up, and we don’t need to do anything.

## 3.3 API versioning

If we take another look at the SwaggerUI, we can notice the Select a Definition drop-down list in the top-right corner of the screen (figure 3.8).

Figure 3.8 SwaggerUI’s API version selector We can use this list to switch between the Swagger documentation files with which we’re feeding the UI. Because we’re using only the default file, which has the project’s name and is version 1.0 by default, this list contains a single MyBGList v1 entry. We can generate (and add) multiple swagger.json files and feed them all to the UI so that we’ll be able to handle and document different versions at the same time. Before I explain how to achieve this result, it could be wise to spend a couple of minutes explaining what API versioning is and, most important, why we should consider it.

### 3.3.1 Understanding versioning

In software development, versioning a product means assigning a unique version number to each release. That’s a common practice for software bundles as well as middleware packages and libraries, as it allows developers and users to track each version of the product. Applying this concept to web applications could be considered to be odd, because apps are typically published to a single static URI (a hostname, domain, or folder), and any new version overrides the previous one.

As mentioned earlier, web applications, including web APIs, are meant to evolve throughout their whole lifecycle. Each time we apply a change and deploy it in production, there’s a risk that a feature that worked correctly will stop working. Regression bugs, interface modifications, type mismatches, and other backward-incompatibility problems can result in a breaking change. The word breaking isn’t metaphorical, because there’s a high chance that the existing system integrations with the clients, as well as with any third party that consumes the API, will break. Adopting a versioning system and applying it to our APIs can reduce the risk. Instead of applying the updates directly to the API, forcing all clients to use it immediately, we can publish the new version in a new location, accessible by means of various techniques (such as URIs or headers) without removing or replacing the old version(s). As a result, the latest version and the old version(s) are online simultaneously, giving clients the choice to adopt the new version immediately or stick with the previous one until they’re ready to embrace the change.

That scenario sounds great, right? Unfortunately, it’s not so simple.

### 3.3.2 Should we really use versions?

API versioning can help us and our clients mitigate the effect of breaking changes, but it also adds complexity and cost. Here’s a brief list of the most significant drawbacks:

Increased complexity—The whole point of adopting a versioning strategy is to keep multiple versions of the same API online in a production environment at the same time. This strategy will likely lead to a massive increase in the number of available endpoints, which means having to deal with a lot of additional source code. Conflict with DRY—Chapter 1 introduced the Don’t Repeat Yourself (DRY) principle. If we want to adhere to this principle, we should have a single, unambiguous, and authoritative representation for each component of our code. API versioning often points in the opposite direction, which is known as Write Everything Twice or Write Every Time (WET). Security and stability problems—Adding features isn’t the only reason to update a web API. Sometimes, we’re forced to do that to fix bugs, performance problems, or security flaws that could negatively affect our system or to adopt new standards that are known to be more stable and secure. If we do that while keeping the old versions available, we leave the door open to these kinds of troubles. In the worst-case scenario, a malicious third party could exploit our web API by using a bug that we fixed years ago because a vulnerable endpoint is still available. Wrong mindset—This drawback is more subtle than the previous ones, yet relevant. Relying on API versioning could affect the way that we think about and evolve our app. We might be tempted to refactor our system in backward-compatible ways so that old versions will still be able to use it or be compatible with it. This practice could easily affect not only our approach to the source code, but also other architectural layers of our project: database schemas, design patterns, business logic, data retrieval strategies, DTOs structure, and so on. We could give up adopting the new version of a third-party library because its new interface would be incompatible with an older version of our API that we want to keep online because some clients are still actively using it—and they’re doing that because we allow them to.

Most of these drawbacks and others were pointed out by Roy Fielding on a couple of occasions. In August 2013, during a talk at the Adobe Evolve conference, he offered some advice on how to approach API versioning in RESTful web service (figure 3.9) by using a single word: don’t. Roy Fielding’s thoughts on API versioning The 46 slides Roy Fielding used throughout his talk can be downloaded at http://mng.bz/Qn91. A year later, he gave a more verbose explanation of that concept in a long interview with InfoQ, available at http://mng.bz/Xall.

Figure 3.9 Advice on API versioning

I’m not saying that API versioning is always bad practice. In some scenarios, the risk of losing backward compatibility is so high that these drawbacks are more than acceptable. Suppose that we need to perform major updates on a web API that handles online payments— an API that’s actively used by millions of websites, services, and users worldwide. We definitely wouldn’t want to force breaking changes on the existing interface without giving users a sort of grace period to adapt. In this case, API versioning might come to the rescue (and save a lot of money).

That example doesn’t apply to our MyBGList web API, however. Our RESTful API is meant to evolve, and we want our clients to embrace that same path, so we won’t give them any excuse to be stuck in the past. For this reason, I’ll explain how to implement a new version of our existing API and then follow a different path.

### 3.3.3 Implementing versioning

Suppose that our development team has been asked to introduce a simple yet breaking change to our existing BoardGamesController for a new board-games-related mobile app, such as renaming the Data property of our RestDTO type to Items. From a code-complexity perspective, implementing this requirement would be a no-brainer; we’re talking about a single line of updated code. But some websites use the current API, and we don’t want to jeopardize the system integration that they’ve struggled so much to pull off. Given this scenario, the best thing we can do is set up and release a new version of our API while keeping the current one available.

API versioning techniques REST doesn’t provide any specifications, guidelines, or best practices for API versioning. The most common approaches rely on the following techniques:

URI versioning—Using a different domain or URL segment for each version, such as api.example.com/v1/methodName QueryString versioning—A variant of URI versioning, using a GET parameter instead, such as api.example.com/methodName?api-version=1.0 Route versioning—Another variant of URI versioning, using a different route, such as api.example.com/methodName-v1 Media Type versioning—Using the standard Accept HTTP header to indicate the version, such as Accept: application/json;api-version=2.0 Header versioning—Using a custom HTTP header to indicate the version, such as Accept-Version: 2.0

Segment-based URI versioning, which separates the various versions by using unique URL segments (also known as path segments) representing the version ID, is by far the most common approach. Here’s an example taken from the PayPal APIs:

https://api-m.paypal.com/v1/catalogs/products https://api-m.paypal.com/v2/checkout/orders

WARNING These URLs aren’t publicly accessible and won’t load unless you have a valid access token; they’re provided for reference purposes only. These URLs were taken from https://developer.paypal.com, which contains the up-to-date API documentation and recommended endpoints for the various services. As we can see, the Orders API currently uses version 2, and the Catalog Products API (at this writing) is still stuck on version 1. As I said earlier, API versioning can help us deal with breaking changes gracefully, even on an endpoint basis, because all the versions that we choose to keep online can be used simultaneously and are (or should be) guaranteed to work. At the same time, the increase in complexity is evident. We’re going to adopt that same segment-based URI versioning approach to fulfill our assigned task.

Formats and conventions The most widely used versioning format for many years was semantic versioning, also known as SemVer, which can be summarized in the following way: MAJOR.MINOR.PATCH. The most recent SemVer version (which, not surprisingly, adopts its own conventions) is 2.0.0. These numbers must be changed according to the following rules:

MAJOR—When we make backward-incompatible API changes MINOR—When we add functionality in a backward-compatible manner PATCH—When we make backward-compatible bug fixes

The SemVer specification also allows the use of prerelease labels, metadata, and other extensions. For simplicity, we’ll stick to the basics, which are more than enough for our current needs. For additional info, see the official specification at http://mng.bz/yaj7. ASP.NET Core API versioning Now that we know what we want to do, we can finally move to the implementation part. But because we don’t want to be stuck to the versioning system we’re going to implement for the rest of the book, we’re going to create a separate project and put everything there. Here’s what we need to do to create such a clone:

1. In Visual Studio’s Solution Explorer, right-click the MyBGList solution and choose Add > New Project from the contextual menu.
2. Select the same ASP.NET Core web API template that we used in chapter 2, with the same settings.
3. Give the new project a distinctive name, such as MyBGList_ApiVersion.
4. Delete the /Controller/ folder and the root files of the new project.
5. Copy the /Controller/ folder, the /DTO/ folder, and the root files of the MyBGList project—the one we’ve been working with up to now—to the new project.

Now we have a clean copy of our project that we can use to play with API versioning without messing with the other codebase.

TIP Rest assured that there are several alternatives—and arguably better—ways to do the same thing. If we’re using a version-control system such as Git, for example, we could create an ApiVersioning branch of the existing project instead of adding a new one. Because this exercise is for demonstrative purposes only, however, we want to have both codebases accessible at the same time. For further references, you can find the MyBGList_ApiVersion project in the book’s GitHub repository for chapter 2.

Setting up the versioning services The most effective way to implement a SemVer-based API versioning system in ASP.NET Core relies on installing the following NuGet packages:

Microsoft.AspNetCore.Mvc.Versioning Microsoft.AspNetCore.Mvc.Versioning.ApiExp lorer

To install them, right-click the MyBGList project’s root node, and choose Manage NuGet Packages from the contextual menu. Then use the search box to find these packages and install them. Alternatively, open the Visual Studio’s Package Manager console, and type the following command:

PM> Install-Package Microsoft.AspNetCore.Mvc.Versioning -Version 5.0.0 PM> Install-Package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer - ➥ Version 5.0.0

In case you prefer to use the dotnet command-line interface, here’s the command to issue from the project’s root folder:

> dotnet add package Microsoft.AspNetCore.Mvc.Versioning --version 5.0.0 > dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer -- ➥ version 5.0.0
NOTE At the time of this writing, version 5.0.0, which was released in February 2021 for .NET 5, is the latest for both packages. This version is mostly compatible with .NET 6, with some minor pitfalls in Minimal API support (as we’ll see later).

The Microsoft.AspNetCore.Mvc.Versioning namespace includes a lot of useful features that implement versioning with a few lines of code. These features include the [ApiVersion] attribute, which we can use to assign one or more versions to any controller or Minimal API method, and the [MapToApiVersion] attribute, which allows us to do the same for the controller’s action methods. Before using these features, however, we need to set up a couple of required versioning services. Furthermore, because we’re using OpenAPI, we’ll need to alter the configuration of the existing middleware so that Swashbuckle can recognize and work with the versioned routes. As always, all these tasks need to be done in the Program.cs file. Open that file, and add the following lines below the CORS service configuration:

// ...

```csharp
builder.Services.AddApiVersioning(options => {
      options.ApiVersionReader = new UrlSegmentApiVersionReader();   ❶
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.DefaultApiVersion = new ApiVersion(1, 0);
});
```

```csharp
builder.Services.AddVersionedApiExplorer(options => {
      options.GroupNameFormat = "'v'VVV";                            ❷
      options.SubstituteApiVersionInUrl = true;                      ❸
});
```

// ...

❶ Enables URI versioning
❷ Sets the API versioning format
❸ Replaces the {apiVersion} placeholder with version number

The new code also requires the following namespace reference, which can be added to the top of the file:

```csharp
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
```

The two services that we’ve added, as well as their configuration settings, give our web API the necessary info to set up URI versioning. In a nutshell, we’re telling the service that we want to use URI versioning, define the versioning format to use, and replace the {version:apiVersion} placeholder with the actual version number. This convenient feature allows us to set up the “versioned” routes for our controllers and Minimal API methods dynamically, as we’ll see in a short while.

TIP In case we want to use an alternative versioning technique, such as QueryString or HTTP headers, we can replace the UrlSegmentApiVersionReader or combine it with one of the other version readers: QueryStringApiVersionReader, HeaderApiVersionReader, and/or MediaTypeApiVersionReader.

Updating the Swagger configuration Now we need to configure the Swagger Generator service to create a JSON documentation file for each version we want to support. Suppose that we require version 1.0 (the existing one) and version 2.0 (the new one), which we want to configure to use the /v1/ and /v2/ URL fragments, respectively: builder.Services.AddSwaggerGen(options => { options.SwaggerDoc( "v1", new OpenApiInfo { Title = "MyBGList", Version = "v1.0" }); options.SwaggerDoc( "v2", new OpenApiInfo { Title = "MyBGList", Version = "v2.0" }); });

This code requires the following namespace reference:

using Microsoft.OpenApi.Models;

Right after that, we need to make sure that the SwaggerUI will load the swagger.json files. Scroll down to the SwaggerUI middleware, and add the following configuration settings:

```csharp
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(
        $"/swagger/v1/swagger.json",
        $"MyBGList v1");
    options.SwaggerEndpoint(
        $"/swagger/v2/swagger.json",
        $"MyBGList v2");
});
```

Last but not least, because we’ve chosen URI versioning, we need to alter the existing routes for our controllers and Minimal APIs to support the /v1/ and /v2/ URL fragments properly. To do that, we can take advantage of the {version:ApiVersion} placeholder made available by the versioning services, which will be replaced by the actual version number used in the HTTP request, because we’ve set the SubstituteApiVersionInUrl option to true in the Program.cs file.

Adding versioning to Minimal API Let’s apply versioning plan to the Minimal APIs, because they’re in the Program.cs file. Scroll down to them, and update the existing code in the following way:

```csharp
app.MapGet("/v{version:ApiVersion}/error",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
    Results.Problem());
```

```csharp
app.MapGet("/v{version:ApiVersion}/error/test",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
    { throw new Exception("test"); });
```

app.MapGet("/v{version:ApiVersion}/cod/test", [ApiVersion("1.0")] [ApiVersion("2.0")] [EnableCors("AnyOrigin")] [ResponseCache(NoStore = true)] () => Results.Text("<script>" + "window.alert('Your client supports JavaScript!" + "\\r\\n\\r\\n" + $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" + "\\r\\n" + "Client time (UTC): ' + new Date().toISOString());" + "</script>" + "<noscript>Your client does not support JavaScript</noscript>", "text/html"));

As we can see, we’re using the [ApiVersion] attribute to assign one or more version numbers to each method. Furthermore, we’re changing the existing route to prepend the version number by using the {version:ApiVersion} placeholder. The placeholder will be replaced by the actual version number specified by the URL fragment included in the HTTP request: /v1/ or /v2/, depending on which version the client wants to use.
NOTE Because our task assignment doesn’t require these Minimal API methods to behave differently in versions 1.0 and 2.0, the best thing we can do to optimize our source code and keep it as DRY as possible is to configure them to handle both versions. Doing that will ensure that all these methods will be executed regardless of the URL fragment used by the client, without our having to duplicate them.

Folder and namespace versioning Now that we’ve adapted the Minimal API methods to our new versioned approach, we can switch to our BoardGamesController. This time, we’ll be forced to duplicate our source code to a certain extent; our task assignment requires such a controller (and its action method) to behave differently in version 1 and version 2, because it’s expected to return a different DTO. Here’s what we need to do:

1. In the /Controllers/ root directory, create two new /v1/ and /v2/ folders.

2. Move the BoardGamesController.cs file to the /v1/ folder, and put an additional copy of that same file in the /v2/ folder. This action immediately raises a compiler error in Visual Studio, because now we have a duplicate class name.
3. To fix the error, change the namespace of the two controllers from MyBGList.Controllers to MyBGList.Controllers.v1 and MyBGList.Controllers.v2, respectively.
4. In the /DTO/ root directory, create two new /v1/ and /v2/ folders.
5. Move the LinkDTO.cs and RestDTO.cs files to the /v1/ folder, and put an additional copy of the RestDTO.cs file in the /v2/ folder. Again, this action raises a compiler error.
6. To fix the error, replace the existing namespace with MyBGList.DTO.v1 and MyBGList.DTO.v2, and change the namespace of the /v1/LinkDTO.cs file from MyBGList.DTO to MyBGList.DTO.v1. This action raises a couple more errors in the two BoardGamesControllers.cs files, because both have an existing using reference to MyBGList.DTO: change—to MyBGList.DTO.v1 for the v1 controller and to MyBGList.DTO.v2 for v2. The v2 controller won’t be able to find the LinkDTO class anymore, as we didn’t create a v2 version for that class to keep our codebase as DRY as possible.
7. Fix the error by adding an explicit reference and changing the LinkDTO references in the v2 controller to DTO.v1.LinkDTO:

Links = new List<DTO.v1.LinkDTO> {     ❶
     new DTO.v1.LinkDTO(               ❶
         Url.Action(null, "BoardGames", null, Request.Scheme)!,
         "self",
         "GET"),
}

❶ Adds an explicit reference to the v1 namespace After all these file-copy and namespace-rename tasks, we should end up with the structure shown in figure 3.10.

Figure 3.10 MyBGList_ApiVersion project structure

As we can see, we created two new “instances” of the types that we need to alter in version 2: RestDTO, which contains the property we’ve been asked to rename, and BoardGamesController, which is the action method that will serve it. Now we have everything we need to implement the required changes.

Updating the v2 Let’s start with the RestDTO.cs file. We need to change the instance that we put in the /v2/ folder, because we want the other one to preserve its current structure and behavior. Open the /DTO/v2/RestDTO.cs file, and rename the Data property to Items in the following way:

public T Items { get; set; } = default!;

That’s it. Now we can finally change version 2 of BoardGamesController to handle the new property (and route). Open the /Controllers/v2/BoardgamesController.cs file. Start with the breaking change that we’ve been asked to implement, which now boils down to updating a single line of code:

// ...

Items = new BoardGame[] {

// ...

Now we need to explicitly assign this BoardGamesController to version 2 and modify its route to accept the path segment that corresponds to that version. Both tasks can use the [Route] and

```csharp
[ApiVersion] attributes, as follows:
// ...
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class BoardGamesController : ControllerBase
```

// ...

We have to do the same thing for the v1 controller, which we still need to assign to version 1:

// ...

```csharp
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class BoardGamesController : ControllerBase
```

// ...

As we can see, we used the {version:apiVersion} placeholder here as well. We could have used a literal string, because each controller is univocally bound to a single version:

[Route("v1/[controller]")]

Using the placeholder whenever possible is definitely good practice, however, as it allows us to configure a controller to handle multiple versions, as we did with the Minimal API methods.

Testing API versioning Now we can finally test what we’ve done so far. Run the MyBGList_ApiVersion project in Debug mode, and type https://localhost:40443/swagger in the browser to access the SwaggerUI. If we did everything correctly, we should see the new /v1/BoardGame endpoint, shown in figure 3.11.

Figure 3.11 SwaggerUI with API versioning If we look at the Select a Definition drop-down list in the top-right corner of the screen, we can see that we can switch to MyBGList v2, which contains the /v2/BoardGames endpoint. Our API versioned project is working as expected—at least, for the most part.

WARNING Truth be told, something is missing from the picture. If we compare figure 3.11 with the SwaggerUI main page of our previous, nonversioned MyBGList project, we see that the Minimal API methods are missing from the drop-down list, and they’re not listed in the swagger.json file. The VersionedApiExplorer that we’re using now to locate all the app’s available endpoints (part of the second NuGet package that we installed) was released in February 2021, before the introduction of .NET 6 and Minimal APIs, so it isn’t able to detect their presence. Remember what I said about the lack of some .NET 6 support? That’s the pitfall I was talking about. Luckily, the problem affects only Swashbuckle. Our Minimal API methods will still work as expected for both versions of our API; they’re not present in the Swagger documentation files, though, and can’t be shown in the SwaggerUI. Ideally, this minor drawback will be fixed when the NuGet packages are updated to fully support all .NET 6 and Minimal API features.

## 3.4 Exercises

Before moving to chapter 4, spend some time testing your knowledge of the topics covered in this chapter by doing some wrap-up exercises. Each exercise emulates a task assignment given by a product owner, which we’ll have to implement by playing the role of the MyBGList development team. The project we’ll be working on is MyBGList_ApiVersion, which has the greatest functionality and is the most complete.

TIP The solutions to the exercises are available on GitHub in the /Chapter_03/Exercises/ folder. To test them, replace the relevant files in your MyBGList_ApiVersion project with those in that folder, and run the app.

### 3.4.1 CORS

Create a new CORS policy that accepts only cross-origin requests performed by using the HTTP GET method (with any origin and header). Call it "AnyOrigin_GetOnly", and assign it to the Minimal API method that handles the <ApiVersion>/cod/test route for any API version.

### 3.4.2 Client-side caching

Update the existing caching rules for the BoardGamesController’s Get() method (API version 2 only) in the following way:

Ensure that the response will set the Cache-Control HTTP header to private. Change the max-age to 120 seconds.

Then switch to the API v1 and disable the cache by setting the Cache-Control HTTP header to no-store.

### 3.4.3 COD

Create a new CodeOnDemandController (version 2 only) with no constructor, without ILogger support, and with two action methods handling the following routes:

/v2/CodeOnDemand/Test—This endpoint must return the same response as the Minimal API method that currently handles the <apiVersion>/cod/test route, with the same CORS and caching settings. Use the ContentResult return value for the action method and the Content() method to return the response text. /v2/CodeOnDemand/Test2—This endpoint must return the same response as the Test() action method, with the same CORS and caching settings. Furthermore, it needs to accept an optional addMinutes GET parameter of integer type. If this parameter is present, it must be added to the server time before it’s sent to the client so that the server time (UTC) value shown in the alert window rendered by the script can be altered by the HTTP request.

Both action methods must be configured to accept only the HTTP GET method.

### 3.4.4 API documentation and versioning

Add a new version (version 3) to our API with the following requirements: No support for any existing Minimal API route/endpoint No support for any BoardGameController route/endpoint Support for the /v3/CodeOnDemand/Test2 route/endpoint only (see the preceding exercise), but the addMinutes GET parameter must be renamed minutesToAdd without affecting version 2

The new version must also have its own swagger.json documentation file and must be shown in the SwaggerUI like the other ones.

Summary Understanding the REST constraints and their effect through the whole project’s lifecycle, from overall architecture to low-level implementation efforts, is a required step for building HTTP APIs that can deal efficiently with the challenges posed by an ever- changing entity like the web. ASP.NET Core can greatly help with implementing most RESTful constraints because it comes with several built-in features, components, and tools that enforce the REST best practices and guidelines. CORS is an HTTP-header-based mechanism that can be used to allow a server to relax a browser security setting by allowing it to load resources from one or more external (third-party) origins. CORS can be implemented in ASP.NET Core thanks to a built-in set of services and middleware, which allows us to use attributes to define default rules and custom policies for controllers and/or Minimal API methods. Response caching techniques can bring substantial advantages to a web API in terms of performance, latency, and bandwidth optimization. Response caching can be implemented with a given set of HTTP headers that can be sent with the response to influence the client (browser cache), as well as intermediate parties such as proxy servers and/or CDN services. The HTTP headers are easy to set in ASP.NET Core by using the [ResponseCache] attribute.

Adopting a uniform interface to standardize the data sent by the web API using hypermedia can help the client become aware of what it can do next, with huge advantages in terms of evolvability. Although REST doesn’t provide specific guidance on how to create a uniform interface, ASP.NET Core can handle this requirement by implementing a response DTO to return not only the requested data, but also a structured set of links and metadata that the client can use to understand the overall logic and possibly request further data. Unlike most web applications, web APIs don’t have a user interface that can make users aware of the frequent changes they’ll likely experience. This limitation is addressed by the introduction of standardized description languages such as OpenAPI (formerly Swagger), which can be fetched by readers and parsers to show the API documentation in human-readable fashion. Swagger/OpenAPI can be implemented in ASP.NET Core by using Swashbuckle, an open source project that provides a set of services and middleware to generate Swagger documents for web APIs. API versioning is the process of iterating different versions of your API and keeping them available at the same time. Adopting an API versioning procedure could bring some stability and reliability to our project, because it allows us to introduce breaking changes gracefully without forcing every client to adapt or cease working. It also comes with some nontrivial drawbacks that could outweigh the benefits in some scenarios. ASP.NET Core provides a wide range of API versioning functionalities through two Microsoft-maintained NuGet packages that can help minimize the complexity that inevitably comes with this approach.
