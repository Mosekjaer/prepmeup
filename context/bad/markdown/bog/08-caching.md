---
title: Caching techniques
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 8
---

# 8. Caching techniques

**This chapter covers**

- Caching overview
- HTTP response caching (client-side, intermediate, and server-
- side)
- In-memory caching
- Distributed caching (using SQL Server or Redis)

In information technology, the term cache describes a hardware component or a software mechanism that can be used to store data so that future requests that require such data can be served faster and—most important—without being retrieved from scratch. Good caching practices often result in performance benefits, lower latency, less CPU overhead, reduced bandwidth use, and decreased costs.

Based on this definition, we can understand that adopting and implementing a caching strategy can create many invaluable optimization advantages. These advantages are especially important for web applications and services (including web APIs), which often have to deal with recurring requests targeting the same resources, such as the same HTML page(s) or JSON result(s) accessed by multiple users. But introducing a caching mechanism also adds some complexity to our code and might easily cause unwanted side effects when we don’t implement it properly.

Chapter 3 introduced the concept of caching, talking about server- side, client-side, and intermediate caching. This chapter expands those concepts and puts them in action with some implementation strategies. We’ll start with a brief overview of the main benefits and downsides that caching can introduce. Then we’ll learn how to use several caching mechanisms, natively provided by .NET and ASP.NET Core, that can implement caching on various levels:

Client-side and intermediate HTTP response caching—Through HTTP response headers Server-side HTTP response caching—Using the ResponseCachingMiddleware In-memory caching—Using the IMemoryCache interface Distributed caching—Using the IDistributedCache interface

## 8.1 Caching overview

The first question we should ask ourselves before starting to code is what we should cache to begin with. This question isn’t easy to answer, because choosing what to cache strongly depends on how our web application is meant to work—to put it even more clearly, what kind of client interactions (requests) it’s meant to handle. Consider some common scenarios:

A static website, which mostly relies on HTML, CSS, JS, and images, should typically focus on HTTP caching, possibly at the browser level and/or via a content-delivery network (CDN) because the server doesn’t do much more than serve static resources. In other words, it requires client-side and/or intermediate HTTP response caching. A dynamic website, such as a WordPress blog, should focus on client-side and/or intermediate HTTP response caching, as it serves several static resources, and most of its HTML content (past blog articles) isn’t subject to frequent changes. Because the web pages are built using PHP and data retrieved from a database management system (DBMS), however, it could benefit from server-side HTTP response, as well as in-memory and/or distributed caching for some expensive and/or highly recurrent DBMS queries. A RESTful web API, such as our MyBGList project, should use HTTP response caching to optimize the most frequently accessed GET endpoints and/or those that come with the default parameters. But because we can reasonably expect that most clients will configure their requests to retrieve only specific data (or only the data they’re allowed to fetch, as we’ll see in chapter 9), a good in-memory and/or distributed server-side caching strategy to relieve the burden on our DBMS could be even more relevant.

DEFINITION A content-delivery network (CDN) is a geographically distributed network of proxy servers that can be used as a service to provide high availability and increase the performance of the content.

In a nutshell, when developing our caching strategy, we should always consider what we can reasonably expect from the clients and how these requests affect our web application’s architecture. As a general rule, we should always aim to cache at the highest level we can get away with: HTTP response caching for static assets and frequently called endpoints that return the same response, in-memory or distributed caching to reduce DBMS calls. That said, a well-crafted web application often adopts a combination of all the preceding techniques to deal with the needs of each of its various sections in the best possible way.

## 8.2 HTTP response caching

As we’ve known since chapter 1, cacheability is one of the six guiding constraints of representational state transfer (REST):

Cache constraints require that the data within a response to a request be implicitly or explicitly labeled as cacheable or non-cacheable. If a response is cacheable, then a client cache is given the right to reuse that response data for later, equivalent requests. (Roy Fielding’s REST dissertation, 5.1.4; http://mng.bz/qdPK)

This small quote summarizes what we need to do to implement a proper HTTP response caching strategy. We already did that in chapter 3, when we introduced the [ResponseCache] attribute (part of the Microsoft.AspNetCore.Mvc namespace) and implemented it in several controller and Minimal API methods. Here’s what we used in our controller’s Get methods to label their responses as cacheable :

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]

And we have labeled several other responses as noncacheable :

[ResponseCache(NoStore = true)]

The [ResponseCache] attribute is a convenient way to set the cache-control HTTP response header’s directives, required by the HTTP 1.1 caching specifications (RFC 7234). These directives are honored by clients and intermediate proxies, fulfilling the REST constraint’s requirements. Now, because we have used this header in all our controller’s methods, we can proudly say that our MyBGList web API is already compliant with the cacheability constraint. Yay! Next, let’s see how we can improve our current implementation.

### 8.2.1 Setting the cache-control header manually

The first question we should ask is whether we could set the cache-control header manually instead of relying on the [ResponseCache] attribute. We can do this easily by using the Response.Headers collection provided by the HttpContext, which happens to be writable. To demonstrate, add a new Minimal API method to the Program.cs file that uses it (listing 8.1).

**Listing 8.1 Program.cs file: /cache/test/1 endpoint using Minimal API app.MapGet("/cache/test/1", [EnableCors("AnyOrigin")] (HttpContext context) => { context.Response.Headers["cache-control"] =   ❶ "no-cache, no-store";                     ❶ return Results.Ok(); });**

```csharp
```

❶ Sets the cache-control header

This method handles the /cache/test/1 endpoint, adding a cache-control header’s directive identical to the one added by the [ResponseCache(NoStore = true)] that we’ve used elsewhere. Setting such directives programmatically would be rather verbose, inconvenient, and hard to maintain, however. The [ResponseCache] attribute may be a better alternative unless we have specific caching needs that aren’t supported by the high- level abstraction it provides.

### 8.2.2 Adding a default caching directive

Our REST cacheability constraint compliance relies on applying the [ResponseCache] attribute to all our controller and Minimal API methods. If we forget to use it somewhere, the corresponding response will have no cache-control header. We can easily test it by creating a second Minimal API caching test method without specifying any caching strategy:

```csharp
app.MapGet("/cache/test/2",
    [EnableCors("AnyOrigin")]
    (HttpContext context) =>
    {
        return Results.Ok();
    });
```

Add this method to the Program.cs file, right below code listing 8.1. Then perform the following tasks:

1. Launch the project in Debug mode.
2. Open the browser’s Development Tools panel by pressing F12.
3. Select the Network tab.
4. Navigate to the /cache/test/2 endpoint.
5. Check out the HTTP response headers on the Network tab. We won’t find any cache-control header for that response. Although this behavior isn’t considered to be a direct violation of the constraint requirements, it’s widely considered to be a data-security vulnerability because it leaves clients (and intermediate proxies) free to choose whether to cache the received content, which could contain sensitive information (passwords, credit cards, personal data, and so on). To avoid this problem, it may be wise to define a default caching directive that kicks in every time we don’t use the [ResponseCache] attribute but is overwritten wherever that attribute is present—in other words, a fallback mechanism.

Implementing a no-cache default behavior Suppose that we want to implement a no-cache default directive for our MyBGList web API that should kick in whenever the [ResponseCache] attribute isn’t explicitly set. We can add to our app’s HTTP pipeline custom middleware that will set the cache- control header by using the programmatic approach we used a while ago. Open the Program.cs file, scroll down to the line where we added the AuthorizationMiddleware, and add the code in the following listing below that line.

**Listing 8.2 Program.cs file: Custom caching middleware**

```csharp
App.UseAuthorization();

app.Use((context, next) =>     ❶
{
    context.Response.Headers["cache-control"] =
        "no-cache, no-store";
    return next.Invoke();
});
```

❶ Adds a default cache-control directive

This task is the first time we’ve implemented custom middleware—an easy task thanks to the Use extension method. We specify what to do with the HttpContext (and/or other injected services) and then pass the HTTP request to the next component in the pipeline.

NOTE We’ve implemented nonblocking middleware, because we’re passing the HTTP request to the subsequent component instead of terminating the pipeline (and providing an HTTP response), as blocking middleware would do. We saw this difference in chapter 1, which introduced blocking and nonblocking middleware.

Testing the no-cache fallback behavior To test our new caching fallback custom middleware, launch the project in Debug mode, and repeat the test that we performed a moment ago. This time, we should find the cache-control header with the no-cache, no-store fallback directive that we set up.

Now let’s check whether the fallback strategy is overwritten by the [RequestCache] attribute (when present). Repeat the test, this time using the /BoardGames GET endpoint instead. If everything works as expected, we should find the cache-control header directive specified by the [RequestCache] attribute that we used for the BoardGamesController’s Get action method,

cache-control: public,max-age=60 which confirms that our no-cache fallback behavior works as expected.

Using strongly typed headers Now that we know that our custom middleware works, let’s refine it. A potential weakness that we can spot by looking at the code is that we’re taking a literal approach to setting the response header—an approach that’s subject to human error. To improve this aspect, we can use the GetTypedHeaders method provided by the HttpContext .Response object, which allows access to the response headers via a strongly typed approach. The following listing shows how we can update our current implementation by replacing those strings with strongly typed values.

**Listing 8.3 Program.cs file: Custom caching middleware (updated)**

```csharp
app.Use((context, next) =>
{
      context.Response.GetTypedHeaders().CacheControl =                ❶
              new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
              {
                 NoCache = true,                                      ❷
                  NoStore = true                                      ❷
              };
      return next.Invoke();
});
```

❶ Uses the GetTypedHeaders method
❷ Configures the cache settings using strongly typed values

The new code not only looks better, but also prevents us from having to type the headers manually, eliminating the risk of mistyping them.

### 8.2.3 Defining cache profiles

Thanks to the no-cache fallback behavior that we’ve set, we don’t have to add a [ResponseCache] attribute to all our controller and Minimal API methods that we don’t want our clients (and intermediate proxies) to cache. But we still have to duplicate these attributes whenever we want to configure different caching behavior. A good example is the Get methods of our BoardGamesController, DomainsController, and MechanicsController, which repeat the [ResponseCache] attribute multiple times:

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]

This approach is far from ideal, especially considering that the number of our controllers will likely increase over time. We likely want to centralize these directives to keep our codebase more DRY (Don’t Repeat Yourself) and easier to maintain. We can achieve this goal by using cache profiles. This convenient ControllerMiddleware configuration option allows us to set up some predefined caching directives and then apply them by using a convenient name-based reference instead of repeating them. Let’s test this neat feature by adding two cache profiles with the following names and behaviors:

NoCache—Use whenever we want to prevent any client or intermediate proxy from caching the response. Any-60—Use when we want to tell everyone (clients and intermediate proxies) to cache the response for 60 seconds.

Open the Program.cs file, locate the line where we added the ControllerMiddleware by means of the AddControllers extension method, and add the options in the following listing at the end of the existing configuration settings.

**Listing 8.4 Program.cs file: Cache profiles**

```csharp
builder.Services.AddControllers(options => {

      // ... non-relevant code omitted ...

      options.CacheProfiles.Add("NoCache",          ❶
          new CacheProfile() { NoStore = true });
      options.CacheProfiles.Add("Any-60",          ❷
          new CacheProfile()
          {
              Location = ResponseCacheLocation.Any,
              Duration = 60
          });
});
```

❶ Adds the "NoCache" profile
❷ Adds the "Any-60" profile

Now that we have two centralized cache profiles, we can apply them throughout our controllers. Open the BoardGamesController.cs file, and scroll down to the Get action method. Then change the current implementation of the [ResponseCache] attribute, replacing the caching values with a reference to the "Any-60" caching profile in the following way:

[HttpGet(Name = "GetBoardGames")]
[ResponseCache(CacheProfileName = "Any-60")]        ❶
public async Task<RestDTO<BoardGame[]>> Get(

❶ Adds a reference to the cache profile

To test what we’ve done, perform the following steps:

1. Launch the project in Debug mode.
2. Open the browser’s Network tab.
3. Navigate to the /GetBoardGames endpoint.
4. Check out the cache-control header value.

If we did everything correctly, we should see the same directive that we had before:

cache-control: public,max-age=60

The only difference is that now the values are taken from the "Any- 60" cache profile, meaning that we successfully centralized them. Now we can apply this technique to all our controllers, replacing the values of all the existing [ResponseCache] attributes with the name of the corresponding cache profile:

[ResponseCache(CacheProfileName = "NoCache")]     ❶
[ResponseCache(CacheProfileName = "Any-60")]      ❷

❶ Use this to prevent caching.
❷ Use this to enforce 60-second caching for anyone.

TIP Technically speaking, instead of using the "NoCache" cache profile, we could remove the attribute and let our no-cache fallback behavior take care of the action methods we don’t want to cache. But we won’t do that. The fallback is meant to act as a safety net against human errors, not to handle our standard caching settings.

### 8.2.4 Server-side response caching

When used alone, the [ResponseCache] attribute will take care of setting the appropriate caching HTTP headers depending on the parameters being set for it. In other words, it handles HTTP response caching only for clients and intermediate proxies. But ASP.NET Core has a built-in component that can store the HTTP responses in a dedicated internal cache repository and serve them from that cache instead of reexecuting the controller’s (or Minimal API’s) methods. The name of this component is response-caching middleware. In the following sections, we’ll see how we can add it to our application’s pipeline and use it to implement server-side HTTP request caching.

Understanding the response-caching middleware The response-caching middleware allows our ASP.NET Core app to cache its own responses according to the same caching directives specified in the response headers by the [ResponseCache] attribute or by any other means. So we can say that it performs its caching job like an intermediate reverse proxy or a CDN service except that it runs on the server side instead of being an intermediate layer.

NOTE The middleware runs on the same server that hosts the ASP.NET Core application itself—not a trivial problem, because one of the main benefits of intermediate caching services (such as reverse proxies and CDNs) is that they’re located elsewhere to avoid burdening the web server with additional overhead. This fact is important to consider when we choose whether to use this middleware, as we’re going to see in a short while.

The fact that the response-caching middleware uses the HTTP response headers also means that when it’s enabled, it seamlessly stacks on what we’ve already done. We don’t have to specify additional caching settings or profiles unless we want to.
TIP The response-caching middleware will cache only HTTP responses using GET or HEAD methods and resulting in a 200 - OK status code. It ignores any other responses, including error pages.

Adding services and middleware Setting up the response-caching middleware in our MyBGList web API requires us to add two lines to the app’s Program.cs file. The first line adds the required services to the service collection. We can do this after all the existing services, right before the app’s building phase, as shown in the following code snippet:

builder.Services.AddResponseCaching();    ❶

var app = builder.Build();

❶ Adds the response-caching middleware services

Scroll down to the line where we added the Cross-Origin Resource Sharing (CORS) middleware. Then add the response-caching middleware, using the convenient extension method in the following way:

app.UseCors();

app.UseResponseCaching();    ❶

❶ Adds the response-caching middleware

NOTE To work, the CORS middleware must be called after the response-caching middleware.

Configuring the middleware Thanks to these changes, our MyBGList web API is equipped with a server-side HTTP response caching mechanism. Also, we’re reserving part of local memory to store the results of our cached responses, which could negatively affect the performance of our app. To prevent memory-shortage problems, we can fine-tune the middleware’s caching strategies by changing its default settings, using the following public properties provided by the ResponseCachingOptions class:

MaximumBodySize—Specifies the maximum cacheable size for the response body, in bytes. The default value is 64 * 1024 * 1024 bytes (64 MB). SizeLimit—Represents the size limit for the response-cache middleware, in bytes. The default value is 100 * 1024 * 1024 bytes (100 MB). UseCaseSensitivePaths—If set to true, caches the responses by using case-sensitive paths. The default value is false.

Here’s how we can use these properties to halve the default caching size limits:

```csharp
builder.Services.AddResponseCaching(options =>
{
      options.MaximumBodySize = 32 * 1024 * 1024;   ❶
      options.SizeLimit = 50 * 1024 * 1024;         ❷
});
```

❶ Sets max response body size to 32 MB
❷ Sets max middleware size to 50 MB
TIP For further information on the response-caching middleware, see http://mng.bz/GRDV.

### 8.2.5 Response caching vs. client reload

All the HTTP response caching techniques that we’ve reviewed so far implement standard HTTP caching semantics. Although this approach is great from a RESTful perspective, it has an important consequence that can easily be seen as a downside. These techniques don’t only follow the response cache headers; they also honor the request cache headers, including the cache-control headers set by web browsers when their user issues a reload and/or force-reload action:

cache-control: max-age=0     ❶
cache-control: no-cache      ❷

❶ Cache header set by a browser’s reload
❷ Cache header set by a browser’s force reload

As a result, all clients are perfectly able to bypass this kind of cache. This situation is great from clients’ perspective because they can force our app to serve them fresh content whenever they want to. But it also means that the cache won’t benefit us in terms of performance (and overhead reduction) if we receive several of these requests in a short period, including those generated by malicious attempts such as distributed denial-of-service (DDoS) attacks.

Unfortunately, ASP.NET doesn’t provide a way to overcome this problem. The intended response-caching behavior can’t be overridden or set up to ignore the HTTP request header. If we want to overcome the client’s will and take control of what should and shouldn’t be cached, we must rely on alternative approaches, such as using a reverse proxy, a third-party caching package (such as AspNetCore.CacheOutput), or any other component or service that can be set up to ignore the HTTP request headers at will.

Otherwise, we might think of integrating our response-caching strategies with other caching techniques that aren’t based on HTTP headers. The following sections introduce some of those techniques and show how we can implement them alongside what we’ve done so far.

## 8.3 In-memory caching

As its name implies, in-memory caching is a .NET caching feature that allows us to store arbitrary data in local memory. The mechanism is based on the IMemoryCache interface, which can be injected as a service (following the standard dependency injection pattern that we should be used to by now) and exposes some convenient Get and Set methods to interact with the cache by retrieving and storing data.

NOTE Technically speaking, even the server-side HTTP response caching strategy based on the response-caching middleware we implemented earlier uses the IMemoryCache interface to store its caching data.

The IMemoryCache interface is part of the Microsoft.Extensions.Caching.Memory namespace, maintained by Microsoft and shipped with a dedicated NuGet package included in most ASP.NET Core templates.
WARNING The IMemoryCache interface and its Microsoft.Extensions.Caching.Memory namespace shouldn’t be confused with System.Runtime.Caching, a different NuGet package that provides an alternative in-memory cache implementation made available through a different class with a similar name (MemoryCache). Although both implementations provide similar functionalities, the IMemoryCache interface works natively with the ASP.NET Core dependency injection design pattern; thus, it’s the recommended approach for ASP.NET Core applications.

In the following sections, we’ll see how we can implement in-memory caching in our MyBGList web API and explore some typical use scenarios.

### 8.3.1 Setting up the in-memory cache

Again, the first thing to do is set up and configure the MemoryCache services in the service collections. Open the Program.cs file, and add the following line after all the existing services, right before the app’s building phase:

builder.Services.AddMemoryCache();   ❶

var app = builder.Build();

❶ Adds the MemoryCache service

The extension method supports an additional overload to accept a MemoryCacheOptions object that can be used to configure some caching settings, such as the following: ExpirationScanFrequency—A TimeSpan value that defines the length of time between successive scans for expired items SizeLimit—The maximum size of the cache, in bytes CompactionPercentage—The amount to compact the cache by when the SizeLimit value is exceeded

For simplicity, we’ll stick with the default values.

### 8.3.2 Injecting the IMemoryCache interface

Now that we’ve enabled the MemoryCache service, we can inject an IMemoryCache interface instance into our controllers. We’ll do that in the BoardGamesController. Open the BoardGamesController.cs file, and add a reference to the interface’s namespace at the top of the file:

using Microsoft.Extensions.Caching.Memory;

Right after that reference, add a new private, read-only _memoryCache property below the constructor, in the following way:

private readonly IMemoryCache _memoryCache;

This property will host the IMemoryCache instance; we need to inject it into in the constructor, as we did with the ApplicationDbContext in chapter 5. The following listing shows how.
**Listing 8.5 BoardGamesController: IMemoryCache**

```csharp
public BoardGamesController(
    ApplicationDbContext context,
    ILogger<BoardGamesController> logger,
     IMemoryCache memoryCache)                ❶
{
     _context = context;
     _logger = logger;
     _memoryCache = memoryCache;              ❷
}
```

❶ Injects the IMemoryCache
❷ Stores the instance in the local variable

That’s all we need to do to give our app in-memory cache capability. In the next section, we’ll learn how to use it.

### 8.3.3 Using the in-memory cache

The IMemoryCache instance is a singleton object that can be used to store arbitrary data as key-value pairs. The great thing about it is that it can store all objects, even nonserializable ones. It can store a collection of model entities retrieved from the database by using Entity Framework Core (EF Core), for example. The following sections define a practical scenario based on this behavior.

Implementing an in-memory caching strategy Suppose that we want to cache the board-game array returned by EF Core within our current BoardGamesController’s Get method for 30 seconds. Because that method can be called by using multiple parameters that will affect the returned data, we also need to find a way to create a different cache key for each request with different parameters to avoid the risk of returning wrong data. Here’s what we need to do:

1. Create a cacheKey string variable based on the GET parameters.
2. Check out the _memoryCache instance to see whether a cache entry for that key is present.
3. If a cache entry is present, we use it instead of querying the database; otherwise, retrieve the data by using EF Core, and set the cache entry with an absolute expiration of 30 seconds.

The following listing shows how we can pull off this plan.

**Listing 8.6 BoardGamesController’s Get method**

```csharp
[HttpGet(Name = "GetBoardGames")]
[ResponseCache(CacheProfileName = "Any-60")]
public async Task<RestDTO<BoardGame[]>> Get(
    [FromQuery] RequestDTO<BoardGameDTO> input)
{
  _logger.LogInformation(CustomLogEvents.BoardGamesController_Get,
      "Get method started.");

  var query = _context.BoardGames.AsQueryable();                           ❶
  if (!string.IsNullOrEmpty(input.FilterQuery))                            ❷
    query = query.Where(b => b.Name.Contains(input.FilterQuery));

  var recordCount = await query.CountAsync();                              ❸

  BoardGame[]? result = null;                                              ❹
  var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";   ❺
  if (!_memoryCache.TryGetValue<BoardGame[]>(cacheKey, out result))        ❻
  {
      query = query                                                        ❼
        .OrderBy($"{input.SortColumn} {input.SortOrder}")
        .Skip(input.PageIndex * input.PageSize)
      .Take(input.PageSize);
      result = await query.ToArrayAsync();
      _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
  }
    return new RestDTO<BoardGame[]>()
    {
      Data = result,                                               ❽
      PageIndex = input.PageIndex,
      PageSize = input.PageSize,
      RecordCount = recordCount,
      Links = new List<LinkDTO> {
        new LinkDTO(
          Url.Action(
            null,
            "BoardGames",
            new { input.PageIndex, input.PageSize },
            Request.Scheme)!,
          "self",
          "GET"),
      }
    };
}
```

❶ Retrieves the IQueryable
❷ Applies the filterQuery (if present)
❸ Retrieves the record count from the database
❹ Declares the local variable to store board-game data
❺ Creates a unique cache key by using the RequestDTO
❻ Checks the in-memory cache for the presence of the cache key
❼ If no cached data exists, retrieves it from DB (and caches it)
❽ Sets the cached (or newly retrieved) data in the RestDTO

Notice that the new implementation, despite adding some complexity to the code, is still strongly based on the previous one. We added only the part to create the cache key and the “cache-or-query” logic to use EF Core conditionally (only if a suitable cache entry isn’t present) instead of always using it.

As for the cacheKey definition, we created a unique string by concatenating the RequestDTO<BoardGame> type name and its JavaScript Object Notation (JSON) serialized representation. The type name is retrieved with the built-in GetType method, and the serialization is obtained by the Serialize method of the JsonSerializer static class, which is part of the System.Text.Json namespace. (Remember to add its reference at the top of the file.) That method creates a JSON string from the input data-transfer object (DTO). By concatenating these two strings, we’ve provided ourselves a simple yet effective way to create a unique key for each request.

The rest of the code is straightforward and should be easy to understand. The “cache-or-query” logic relies on the convenient IMemoryCache’s TryGetValue<T> method, which returns true if the cache entry exists (while conditionally setting the output parameter with the retrieved value) and false if it doesn’t.

TIP In this example, the <T> generic type could be omitted when using the TryGetValue method, because it will be inferred automatically from the result local variable. We chose to specify it explicitly to clarify the cache-retrieval process.

Testing the in-memory cache The best thing we can do to test our new caching strategy is place a couple of breakpoints within the source code: one on the line where we call the TryGetValue method, and another one on the line that (if no cached data is found) will execute the data-retrieval process from the database by using EF Core (figure 8.1).
Figure 8.1 Setting up the breakpoints to test the in-memory cache

Ideally, we can expect the first breakpoint to be hit every time the server receives the request (that is, when the HTTP response cache doesn’t kick in), and the second one should be hit only when the app has nothing in cache for the incoming request, which should happen only once every 30 seconds for each request with the same set of parameters. Conversely, if the cache entry is present, that part of the code shouldn’t be executed.

When the breakpoints have been placed, we can launch our project in Debug mode and call the /BoardGames GET endpoint without parameters, so that the default parameters will be used. Now, because this request has never been called before, the IMemoryCache instance should have no cache entries for it. As a result, both breakpoints should be hit in sequence. (Press F5 or click the Continue button when the first breakpoint activates to stop on the second one, as shown in figure 8.2.)
Figure 8.2 Both breakpoints are being hit because there’s nothing in cache for this request.

The fact that the first breakpoint has been hit is hardly a surprise; we know that it’s expected to be hit on every request. The important thing to notice is that the second breakpoint is being hit as well. The controller will query the DBMS by using EF Core to retrieve the entries for the first time, as we expect it to do.

So far, so good. Press F5 or click Continue a second time to hop off the second breakpoint and resume the app’s execution; then wait for the controller to return its response and for the browser to show the JSON outcome. When the JSON result with the board-games data fills the browser’s window, press Ctrl+F5 (while focusing the browser) to clear the cache, thus issuing a forced page reload. Again, the first breakpoint should be hit, because the forced reload should invalidate both the client-side and server-side HTTP response caches and reach the controller’s Get method. But if we press F5 or click Continue within 30 seconds of the previous request, this time the second breakpoint isn’t likely to be hit, and we’ll receive a second response, identical to the previous one, without having to query the DBMS.

If we repeat this test after 30 seconds or more, the in-memory cache should expire, meaning that both breakpoints will be hit again. This simple yet effective test is more than enough to confirm that our in- memory caching implementation works as expected. Now that we’ve tested the in-memory cache mechanism, we’re ready to move to distributed caching.

## 8.4 Distributed caching

In-memory caching has several advantages over HTTP caching in terms of management and customization, because we have full control of what to cache and what not to cache. But this technique also has some unavoidable downsides, most of which depend on the fact that the cache storage resides in the server’s local memory.

Although this situation isn’t a problem when our web app is running on a single server, it becomes a major problem for apps running on a server farm (that is, on multiple servers). In this scenario, each cached entry will be available only within the single server that set it up; any other server won’t be able to retrieve it and will have to create its own. As a result, each server will have its own local in-memory cache with different expiration times. This behavior, besides being a waste of memory, can lead to cache inconsistencies, because the same user might navigate across different servers that could respond with different cached data (having stored it at different times).

The best thing we can do to avoid these drawbacks is to replace the local in-memory cache mechanism with a shared caching repository that can be accessed by multiple servers—in other words, a distributed cache. Switching from a local cache to a distributed cache provides the following advantages:

Consisted cached data—Because all servers retrieve them from the same source Independent lifetime—Because it doesn’t depend on any web server (won’t reset whenever we need to restart it) Performance benefits—Because it’s typically implemented by a third-party service and won’t consume a web server’s local memory

NOTE Although the first advantage affects only apps that are running on multiple servers, the other two advantages are likely to benefit even one-server app scenarios.

In the following sections, we’ll set up a distributed cache mechanism in our MyBGList project, starting with the in-memory caching implementation we already have.

### 8.4.1 Distributed cache providers overview

The first thing to do when we want to set up a distributed caching strategy is choose the cache provider—the external service that will host the cached data. When we’re dealing with ASP.NET Core web apps, the challenge usually boils down to the following two major alternatives: A DBMS (such as SQL Server)—The most obvious choice, especially considering that most web apps (including our MyBGList web API) already use it for data storage purposes, so most of the required stuff is already set up A distributed key-value store (such as Redis or NCache)— Another popular choice, because those storage services are typically fast (Redis can perform up to 110K SETs and 81K GETs per second), easy to set up and maintain, and can scale out at will

With that in mind, we’ll implement a distributed caching strategy by using a popular provider for both SQL Server and Redis. Each can be easily installed by adding a reference to its own NuGet package, which implements the IDistributedCache interface (part of the built-in Microsoft.Extensions.Caching.Distributed namespace). This interface, like ILogger, IMemoryCache, and many others that we’ve used in previous chapters, provides a common set of methods that we can use to set up, configure, and manage the cache provider.

### 8.4.2 SQL Server

To use the SQL Server distributed cache provider, we need to install the NuGet package Microsoft.Extensions.Caching.SqlServer. As always, we can use Visual Studio’s NuGet graphical user interface (GUI), Visual Studio’s Package Manager console, or the command- line interface (CLI). I’ll take for granted that you know how to do that. Then open the Program.cs file, and set up the distributed caching provider, using the standard, service-based approach that you should be used to by now.

Configuring the SQL Server cache service In the Program.cs file, scroll down to the point where we added the in-memory cache by using the AddMemoryCache extension method, and add the lines in listing 8.7 immediately below it.

**Listing 8.7 Program.cs file: Distributed SQL Server cache service**

```csharp
builder.Services.AddDistributedSqlServerCache(options =>                    ❶
{
    options.ConnectionString =
          builder.Configuration.GetConnectionString("DefaultConnection");   ❷
      options.SchemaName = "dbo";                                           ❷
      options.TableName = "AppCache";                                       ❷
});
```

❶ Adds the SQL Server distributed cache provider
❷ Configures the provider’s settings

Notice that the configuration process is loosely similar to what we did with Serilog in chapter 7. We’re “recycling” the same connection string that we set up for EF Core in chapter 4 to use the same MyBGList database. But the distributed caching tasks will be performed against a new, dedicated database table called "AppCache".

NOTE For reasons of space, I won’t bother explaining the other configuration options. They’re self-explanatory, and we don’t need to change them for demonstration purposes. For additional info on the SqlServerCacheOptions class and its properties, see http://mng.bz/zmzr.

Creating the AppCache DB table This table doesn’t exist in our database yet. To create it, we must install and then execute a specific sql-cache command, using the dotnet CLI in the project’s root folder. Here’s how we can install the sql-cache command:

dotnet tool install --global dotnet-sql-cache -version 6.0.11

WARNING The version number here must match the .NET version (patch included) installed on the system. Otherwise the sql-cache create command that we’re about to use will likely return an error.

Then execute the command in the following way:

dotnet sql-cache create "{connectionString}" dbo AppCache

Be sure to replace the {connectionString} placeholder with the value of the DefaultConnection key (including the username/password credentials) defined in the secrets.json file. Also be sure to replace any double backslash (such as in localhost\\MyBGList) with a single backslash (localhost\MyBGList); otherwise, the CLI command will fail. When executed correctly, the sql-cache command should create a new [AppCache] DB table in our MyBGList database with the following columns: Id: PK, nvarchar(449), not null Value: varbinary(max), not null ExpiresAtTime: datetimeoffset(7), not null SlidingExpirationInSeconds: bigint, null AbsoluteExpiration: datetimeoffset(7), null

Alternatively, we could create the table manually by using SQL Server Management Studio (SSMS).

Adding the DistributedCacheExtensions Unfortunately, the IDistributeCache interface doesn’t come with the handy generic-type methods—Get<T>, Set<T>, and TryGetValue<T>—that we appreciated in the IMemoryCache interface; it provides only Get and Set methods to handle string and byte array values. Because we want to store strongly typed entity objects retrieved by the DBMS, using those methods as they are would force us to convert them to byte arrays, thus resulting in more code lines.

For that reason, we’ll create our first extension method helper class, in which we’ll implement the same methods provided by the IMemoryCache interface and extend the IDistributedCache interface with them. Here are the methods we’re going to create:

TryGetValue<T>, accepting a cacheKey string value and an out parameter of T type Set<T>, accepting a cacheKey string value, a value of T type, and a TimeSpan value representing the absolute expiration relative to the current time

In the project’s root folder, create a new /Extensions/ directory and a new DistributedCacheExtensions.cs file within it. Then fill the new file with the content of the following listing.

**Listing 8.8 DistributedCacheExtensions.cs file**

```csharp
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace MyBGList.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static bool TryGetValue<T>(     ❶
            this IDistributedCache cache,
            string key,
            out T? value)
        {
            value = default;
            var val = cache.Get(key);
            if (val == null) return false;
            value = JsonSerializer.Deserialize<T>(val);
            return true;
        }

        public static void Set<T>(             ❷
            this IDistributedCache cache,
            string key,
            T value,
            TimeSpan absoluteExpirationRelativeToNow)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
            cache.Set(key, bytes, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow
            });
        }
    }
}
```

❶ TryGetValue<T> implementation
❷ Set<T> implementation

Notice that both extension methods perform a conversion between the byte[] type (required by the IDistributedCache interface’s default methods) and a JSON serialization of the cache value, which is passed as a T generic type. Thanks to these methods, we provided the IDistributedCache interface the same methods that we used when implementing the IMemoryCache interface, so we can use a similar approach to implement it.

Injecting the IDistributedCache interface Now that we have the service enabled, the [AppCache] table ready, and our convenient extension methods, we can inject an IDistributedCache interface instance into one of our controllers, as we did with the IMemoryCache interface earlier. This time we’ll use the MechanicsController. Open the MechanicsController.cs file, and add a reference to the required namespaces at the top of the file:

```csharp
using Microsoft.Extensions.Caching.Distributed;
using MyBGList.Extensions;
using System.Text.Json;
```

Then add a new private, read-only _distributedCache property right below the constructor, in the following way:

private readonly IDistributedCache _distributedCache; Now we need to inject an IDistributedCache instance into the constructor by using the dependency injection pattern, as we’ve already done several times. The following listing shows how.

**Listing 8.9 MechanicsController: IDistributedCache**

```csharp
public MechanicsController(
    ApplicationDbContext context,
    ILogger<BoardGamesController> logger,
      IDistributedCache distributed)              ❶
{
      _context = context;
      _logger = logger;
      _distributedCache = distributedCache;       ❷
}
```

❶ Injects the IDistributedCache
❷ Stores the instance in the local variable

All that’s left now is the implementation part in the MechanicsController’s Get method. The source code is provided in the following listing.

**Listing 8.10 MechanicsController’s Get method**

```csharp
[HttpGet(Name = "GetMechanics")]
[ResponseCache(CacheProfileName = "Any-60")]
public async Task<RestDTO<Mechanic[]>> Get(
    [FromQuery] RequestDTO<MechanicDTO> input)
{
  var query = _context.Mechanics.AsQueryable();
  if (!string.IsNullOrEmpty(input.FilterQuery))
    query = query.Where(b => b.Name.Contains(input.FilterQuery));

    var recordCount = await query.CountAsync();

    Mechanic[]? result = null;                                              ❶
    var cacheKey =
      $"{input.GetType()}-{JsonSerializer.Serialize(input)}";               ❷
    if (!_distributedCache.TryGetValue<Mechanic[]>(cacheKey, out result))   ❸
    {
        query = query
          .OrderBy($"{input.SortColumn} {input.SortOrder}")
          .Skip(input.PageIndex * input.PageSize)
                    .Take(input.PageSize);
            result = await query.ToArrayAsync();                             ❹
            _distributedCache.Set(cacheKey, result, new TimeSpan(0, 0, 30)); ❹
    }

    return new RestDTO<Mechanic[]>()
    {
        Data = result,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize,
        RecordCount = recordCount,
        Links = new List<LinkDTO> {
          new LinkDTO(
            Url.Action(
              null,
              "Mechanics",
              new { input.PageIndex, input.PageSize },
              Request.Scheme)!,
            "self",
            "GET"),
        }
    };
}
```

❶ Declares the result variable
❷ Creates the cache key from the GET parameters
❸ Checks the cache for the existence of the key
❹ If cache isn’t present, retrieves the data and sets it

As we can see, the actual IDistributedCache implementation is almost identical to what we did in the BoardGamesController’s Get method for the IMemoryCache interface. Such optimization could be done thanks to the extension methods provided by the DistributedCacheExtensions class, which handle the byte[] conversion tasks internally, allowing us to focus on the caching set and retrieval logic in the action method’s source code. Now we need to test what we’ve done. Testing the distributed cache To test our distributed cache implementation, we can perform the same tasks that we did when we tested the IMemoryCache. Obviously, we won’t be able to test it by using multiple web servers, but that’s not an problem: as long as we can prove that the data is stored and retrieved properly on our third-party service (SQL Server) by our single-server app, we can take for granted that the same behavior will work in a multiserver scenario.

Let’s proceed with the test. Open the MechanicsController.cs file, and place a couple breakpoints within the source code: one on the line where we call the TryGetValue method and another one on the first line between the curly brackets, as we did with the BoardGamesController.cs file (refer to figure 8.1). When the breakpoints have been placed, launch the project in Debug mode, and call the /Mechanics GET endpoint without parameters so that the default parameters will be used.

Because that request is the first in the past 30 seconds, the IDistributedCache instance should find no valid cache entries, so both breakpoints should be hit (refer to figure 8.2). Wait for the JSON outcome to show up in the browser; then press Ctrl+F5 (while focusing the browser) to issue a forced page reload within the next 30 seconds. If everything works as expected, this time only the first breakpoint will be hit because the actual data will be loaded from the distributed cache.

Wait 30 seconds or more to make the distributed cache expire; then refresh the page again. This time, both breakpoints should be hit again, proving that the retrieval-and-expiration logic works and concluding our distributed cache test.

### 8.4.3 Redis

Implementing a Redis-based distributed cache requires us to perform three major tasks:

1. Set up a Redis server we can use to test the distributed cache mechanism.
2. Install the NuGet package required to handle the Redis distributed cache provider.
3. Change the distributed cache settings in the Program.cs file, replacing the SQL Server provider with the Redis provider.

In the following sections, we’ll take care of all those tasks.

NOTE The implementation in the MechanicsController.cs file won’t require any change because we’re changing the underlying provider, not the IDistributedCache uniform interface that will handle the caching behavior.

Setting up a Redis server The quickest thing we can do to provide ourselves a Redis server is to create a free account in Redis Enterprise Cloud, the fully managed cloud service offered by Redis on its official website. To obtain it, perform the following steps:

1. Visit the https://redis.com website.
2. Click the Try Free button to access the subscription form.
3. Create an account by using Google or GitHub or by filling out the subscription form.
4. Select a cloud vendor and region to create the Redis instance (figure 8.3).
5. Click the Let’s Start Free button to activate the free plan.
Figure 8.3 Selecting the cloud vendor and region in Redis Cloud

The website creates our Redis database instance and shows its entry in the service’s main dashboard. Click the new instance to access its configuration settings, where we can take note of the public endpoint (figure 8.4) we’ll use to access it.
Figure 8.4 Retrieving the Redis database’s public endpoint

Next, we need to scroll farther down that page’s Configuration tab until we reach the Security panel, where we can retrieve the default user password (figure 8.5). The Public Endpoint and Default User Password values are all we need to assemble the Redis connection string.

Figure 8.5 Retrieving the Redis database’s default user password

Adding the Redis connection string Because the Redis connection settings are reserved data, we’re going to store them securely, using Visual Studio’s User Secrets feature, which we used in previous chapters. On the Solution Explorer panel, right-click the project’s root node, and choose the Manage User Secrets option from the contextual menu to access the secrets.json file for our MyBGList project. Add the following section to the JSON content below the "Azure" section that we added in chapter 7:

```csharp
  "Redis": {
    "ConnectionString": "<public endpoint>,password=<default password>"
  }
```

Replace the <public endpoint> and <default password> placeholders with the values retrieved from the Redis Cloud dashboard panel. Now that we have the Redis storage service, as well as the connection string required to connect to it, we’re ready to install the NuGet package and configure the distributed cache service by using the Redis provider.

Installing the NuGet package The NuGet package we need to use the Redis distributed cache provider is Microsoft .Extensions.Caching.StackExchangeRedis. When we’re done installing it, we can switch to the Program.cs file to set up and configure the service.

Configuring the Redis cache service
Open the Program.cs file, and locate the point where we set up
and configured the SQL Server distributed cache provider. Comment
out the SQL Server settings, and replace them with the following
Redis-related code:
builder.Services.AddStackExchangeRedisCache(options =>      ❶
{
    options.Configuration =
         builder.Configuration["Redis:ConnectionString"];   ❷
});

❶ Adds the Redis distributed cache provider
❷ Configures the provider’s connection settings

NOTE Again, I won’t delve into the other configuration options; the default settings are viable for this sample, demonstrative scenario. For additional info regarding the RedisCacheOptions class and its properties, check out http://mng.bz/0yBm.

If we did everything correctly, the new distributed cache provider should work with the same implementation that we set up in the MechanicsController; we don’t have to change anything because the IDistributedCache interface is the same.

To test the new provider, we can perform the same two-breakpoints procedure that we used to test our previous SQL Server distributed cache implementation. For reasons of space, I won’t mention it again. That said, it could be wise to repeat the test to ensure that everything works as expected.

TIP For further information regarding the ASP.NET Core distributed cache, see http://mng.bz/Kld4.

Our application cache journey is over. Rest assured that we’ve only scratched the surface of a complex topic; to explore it further, check out the official Microsoft guides that I’ve referenced throughout the chapter.

## 8.5 Exercises

The time has come to challenge ourselves with a cache-related list of hypothetical task assignments given by our product owner. As always, dealing with these tasks will greatly help us memorize and remember the concepts covered and the techniques learned throughout this chapter.

TIP The solutions to the exercises are available on GitHub in the /Chapter_08/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 8.5.1 HTTP response caching

Change the HTTP response cache behavior of the MechanicsController’s Get method to make it comply with the following requirements:

It must be cacheable only by clients (private). The cache must expire after 2 minutes.

### 8.5.2 Cache profiles

Add a new cache profile with the same settings specified in the preceding exercise, give it the name Client-120. Then apply the new cache profile to the BoardGamesController’s Get method, replacing the existing cache profile.

### 8.5.3 Server-side response caching

Change the response-caching services settings in the following way:

Set the largest cacheable size for the response body to 128 MB. Set the size limit for the response-cache middleware to 200 MB. Set the request paths as case-sensitive.

### 8.5.4 In-memory caching

Currently, the RecordCount property of the RestDTO object returned by the BoardGamesController’s Get method is always retrieved from the DBMS. Change the current implementation to cache it as well, along with the result variable. To achieve this result without altering the existing caching strategy, consider the following approach:

1. Declare a new dataTuple variable of Tuple type.

2. Use it to group the recordCount and the result variables.
3. Cache the new dataTuple variable instead of the result variable, applying the set-and-retrieval logic to both variables grouped within it.

TIP To find out more about the C# Tuple type and learn how to use it to group multiple data elements, check out the guide at http://mng.bz/918a.

### 8.5.5 Distributed caching

Change the name of the SQL Server distributed cache provider’s caching table from [AppCache] to [SQLCache] at application level. Then create the new table in the DBMS, using the sql- cache CLI command. Don’t delete the previous table so that you can revert the changes after completing the exercise.

Summary When developing a caching strategy, we should always consider what we can expect from clients and how their requests can affect our web application. With that in mind, we should aim to cache at the highest level we can get away with. In ASP.NET Core, we can do this by using several caching mechanisms provided by the framework, including HTTP response cache (client-side and server-side) In-memory cache Distributed cache HTTP response caching is useful mostly for static assets and frequently called endpoints that return the same response, whereas in-memory and distributed caching greatly reduce DBMS calls and the overhead caused by heavy-lifting business logic tasks. Adopting a wise, balanced combination of all these techniques is typically the way to go when developing web applications. In ASP.NET Core, HTTP response caching is typically implemented by the [ResponseCache] attribute, which provides a high-level abstraction of the HTTP response caching headers. The [ResponseCache] attribute settings can be defined inline and/or centralized within the app’s main configuration file through the cache profiles feature. If used alone, the [ResponseCache] attribute will take care of client-side response caching only. When coupled with the response-caching middleware, it also handles the server-side response-caching behavior of the web app. In-memory caching gives the developer more control of the caching data because it doesn’t honor the HTTP request cache headers, so the clients can’t refresh or invalidate it. It also has several other advantages: It can be used to store objects of any type, including entities retrieved from the DBMS. It’s easy to set up, configure, and implement in any ASP.NET Core app thanks to the built-in IMemoryCache interface. It’s great for dealing with web apps hosted on a single server. In multiple-server scenarios, in-memory caching could lead to performance drawbacks and cache inconsistency problems. Distributed caching is an effective way to prevent that problem, because it relies on an external service that makes the cache storage accessible simultaneously by all the web servers without affecting their local memory. In ASP.NET Core, distributed caching can be set up and configured by using a DBMS (such as SQL Server) or key- value storage (such as Redis). Each supported distributed cache provider comes with dedicated NuGet packages that implement the built-in IDistributedCache interface internally, allowing us to choose the provider we like most (and/or switching providers at will) without changing the app’s implementation logic.
