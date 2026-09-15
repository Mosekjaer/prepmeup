---
title: API documentation
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 11
---

# 11. API documentation

**This chapter covers**

- Identifying potential users of a web API
- Applying API documentation best practices in Swagger and
- Swashbuckle
- Using Extensible Markup Language (XML) documentation and
- Swashbuckle annotations
- Customizing the swagger.json file by using Swashbuckle’s
- filter pipeline

In chapter 1, when we tried to define an application programming interface (API), we referred to it as a software platform that exposes tools and services that different computer programs can use to interact by exchanging data. Starting from this definition, we could say that the purpose of an API (including a web API) is to create a common place where independent and often-unrelated systems can meet, greet, and communicate by using a commonly accepted standard. These “actors” are mostly computer programs—such as websites, mobile apps, and microservices—that are implemented by other developers. For that reason, whoever takes on the task of designing, creating, and releasing a web API must acknowledge the existence and needs of a new type of user: the third-party developer, which brings us to this chapter’s topic.

In modern software development, documenting an interface, middleware, a service, or any product that’s intended to be a means to an end isn’t considered to be an option anymore: it’s a design requirement as long as we want to increase or speed its adoption. It’s also the quickest possible way to make an interested third party able to fully understand the value of our work. This aspect has become so important in recent years that it favored the definition of a new design field: developer experience (DX), the user experience from a developer’s point of view. By taking DX into account, I’ll dedicate this chapter to identifying best practices for API documentation and show how we can put them in practice with the help of the many tools made available by ASP.NET Core.

## 11.1 Web API potential audience

The technical documentation of a product is useful only to the extent that it meets the needs and expectations of those who read it. For this reason, the first thing to do is identify our web API potential audience: the stakeholders who are expected to choose and/or use it. When referring to them, I usually break them into three main types, using names taken from construction slang.

### 11.1.1 Prospectors

Prospectors are passionate developers and IT enthusiasts who are willing to give our web API a try without a compelling need beyond personal interest, knowledge gaining, testing/reviewing purposes, and so on. This group is important if we want our web API to be a general-purpose product that we intend to release to the public (or part of it); their feedback will likely have an immediate effect on the developer community, possibly bringing in contractors and builders (see sections 11.1.2 and 11.1.3, respectively).

### 11.1.2 Contractors

Contractors are the IT analysts, solution architects, and backend designers who take responsibility for creating products, solving problems, or addressing potential challenges that our web API could help them deal with. Although typically, they don’t get down to implementation, they often serve as decision-makers, since they have authority, handle the budget, and/or possess the required know-how to suggest, choose, or dictate which components to use (unless they let builders choose them).

### 11.1.3 Builders

Builders are software developers who choose (or are instructed) to use our web API to solve a specific problem. They represent the most technical part of our audience and can be difficult to satisfy, because dealing with our API is part of their working assignment, and they often have limited time to get the job done. Builders will have to learn to work with our web API practically; they’re the third-party developers I mentioned earlier.

After reading those descriptions, it may seem obvious to think that our documentation should focus on builders, who are the end users of our web API. This premise is valid. Most of the API documentation best practices we’ll deal with in this chapter will take this approach into account. But we shouldn’t forget about the other two audience types, as the success of our project may also depend on them.

## 11.2 API documentation best practices

Developers are peculiar types of users. They’re analytical, precise, and demanding, especially if we consider that they typically want to use our API to achieve major goals: implementing requirements, solving problems, and so on. Whenever they find themselves unable to achieve their goals due to poorly written documentation, there’s a high chance they’ll think that the API isn’t good enough—and brutal as it may sound, they’ll be right. At the end of the day, APIs are only as good as their documentation, which inevitably has a huge influence on adoption and maintainability.

What do we mean by good documentation, and how can we achieve it? No single answer is valid in all cases. But a few good practices can help us to find a viable way to achieve what we want, such as the following:

Adopting an automated description tool—So that our documentation won’t become stale or outdated if we forget to update it along with our web API’s source code Describing endpoints and input parameters—So that our audience not only acknowledges their existence, but also learns their purposes and how to use them Describing responses—So that our audience knows what to expect when calling each endpoint and how to handle the outcome Adding request and response samples—To save our audience a huge amount of development time Grouping endpoints into sections—To better separate users’ different scopes, purposes, and roles Excluding reserved endpoints—To prevent users from being aware of their existence and/or trying to call them Emphasizing authorization requirements—To let our audience distinguish between publicly accessible operations and those that are restricted to authorized users Customizing the documentation context—Such as choosing the appropriate names, icons, and metadata to help users find the required info

The following sections talk more about these concepts and show how to implement them in our MyBGList web API.

### 11.2.1 Adopt an automated description tool

If we want to satisfy a third-party developer, we must ensure that our API documentation will always be updated. Nothing is more frustrating than dealing with missing specifications, nonexistent operations or endpoints, wrong parameters, and the like. Outdated documentation will make our users think our API is broken, even if it’s not.

NOTE A poorly (or wrongly) documented web API is technically broken, because a third party has no chance to see that it works as expected. That’s the reason behind our initial statement that API documentation is a design requirement, not an option or an add-on. This concept is true even for internal APIs that are expected to be used only by internal developers, because the lack of proper documentation will eventually affect new employees, potential partners, maintenance tasks, handover processes, outsourcers, and so on. The need to automate the documentation process is particularly strong for RESTful APIs, because the REST architectural standard doesn’t provide a standardized mechanism, schema, or reference for that purpose. This is the main reason behind the success of Open API (formerly known as Swagger), an open source specification for automated API documentation released by SmartBear Software in 2011 with the purpose of solving that problem.

We’ve known about Swagger/OpenAPI since chapter 2, because Visual Studio’s ASP.NET Core web API template, which we used to create our MyBGList project, includes the services and middleware of Swashbuckle, a set of services, middleware, and tools for implementing OpenAPI within ASP.NET Core. We’ve also experienced its autodiscovery and description capabilities, which granted us a code-generated OpenAPI 3.0 description file (swagger.json) and an interactive, web-based API client (SwaggerUI) that we’ve used to test our endpoints. Because we’re already using Swashbuckle, we could say that we’re set. In the following sections, however, we’re going to extend its capabilities to meet our needs.

### 11.2.2 Describe endpoints and input parameters

If we look at our SwaggerUI main dashboard, we see that our current “documentation” consists merely of a list of endpoints and their input variables, without a single description of what each method does. Our audience will have to infer the endpoint’s use, as well as the purpose of each request header and/or input parameter, from the names, which isn’t the best possible way to showcase, valorize, or promote our work.

NOTE If our API adheres to the RESTful good practice of using HTTP verbs to identify the action type, it’ll provide other useful hints about the use of each endpoint—at least to users who have the required know-how.

Conversely, we should adopt a standardized way to create a concise yet relevant description of each endpoint and its input variables. This practice will not only save builders time, but also give prospectors and contractors a better grip on how the API works and what it can do. Swashbuckle provides two ways to add custom descriptions to our endpoints and input parameters:

Use an Extensible Markup Language (XML) documentation file autogenerated by the .NET compiler from the standard triple- slash, XML-formatted comments that we can add inside our C# classes. Use the [SwaggerOperation] data attribute, provided by the optional Swashbuckle .AspNetCore.Annotations NuGet package.

Each technique has benefits and downsides. In the next section, we’ll see how to implement both techniques.

### 11.2.3 Add XML documentation support

The XML documentation approach can be useful, convenient, and fast to implement if we’ve already added comments in our source code by using the triple-slash syntax provided by C#. I’m talking about a neat C# feature that allows developers to create code-level documentation by writing special comment fields indicated by triple slashes. This feature is also used by some integrated development environments (IDEs), such as Visual Studio, that automatically generate XML elements to describe various code parts, such as <summary> (for methods), <param> (for input parameters), and <returns> (for return values).

NOTE For additional info regarding C# XML documentation comments, check out http://mng.bz/qdy6. For a complete reference on the supported XML tags, see http://mng.bz/7187.

The best way to learn how to use this feature is to put it in practice. Open the /Controllers/AccountController.cs file, locate the Register action method, position the cursor above it (and all its attributes), and type the slash (/) character above it three times. As soon as you add the third slash, Visual Studio should generate the following XML comment boilerplate:

/// <summary> /// /// </summary> /// <param name="input"></param> /// <returns></returns> [HttpPost] [ResponseCache(CacheProfileName = "NoCache")] public async Task<ActionResult> Register(RegisterDTO input)

Notice that the autogenerated XML structure identifies the name of the RegisterDTO input parameter of the action method. Now that we have the boilerplate, let’s fill it. Here’s how we can document our AccountController’s Register endpoint: /// <summary> /// Registers a new user. /// </summary> /// <param name="input">A DTO containing the user data.</param> /// <returns>A 201 - Created Status Code in case of success.</returns>

Right after that, scroll down to the Login action method, and do the same thing. Here’s a suitable description we can use to document it:

/// <summary> /// Performs a user login. /// </summary> /// <param name="input">A DTO containing the user's credentials.</param> /// <returns>The Bearer Token (in JWT format).</returns>

Save and close AccountController. Next, tell the compiler to use the XML comments we’ve added, as well as any other comment of that type that’s present in our code, to generate an XML documentation file.

Generating XML documentation files To enable this feature, we need to update our MyBGList project’s configuration file. Right-click the project’s root node in the Solution Explorer window, and choose the Edit Project File option from the contextual menu to open the MyBGList.csproj file. Next, add the following code at the bottom of the file, below the <ItemGroup> block that we added in chapter 10 to include the protobuf file:

// ... existing code

<ItemGroup> <Protobuf Include="gRPC/grpc.proto" /> </ItemGroup> <PropertyGroup> <GenerateDocumentationFile>true</GenerateDocumentationFile> <NoWarn>$(NoWarn);1591</NoWarn> </PropertyGroup>

// ... existing code

Now the compiler will generate the XML documentation file whenever we build the project.

Overcoming the CS1591 warnings The <NoWarn> element that we used in the preceding code will suppress the CS1591 warnings that the GenerateDocumentationFile switch will raise for any public types and members without a three-slash comment. We’ve chosen to shut them down globally in our sample project because we don’t need that advice, but it could be useful to keep it up if we want to ensure that we comment/document everything.

For further info on the GenerateDocumentationFile switch, see http://mng.bz/mJdn.

The last thing we need to do is configure Swashbuckle to fetch the XML documentation file’s contents.

Configuring Swashbuckle To read our project’s XML documentation file, Swashbuckle needs to know its full path and file name that corresponds to our project’s name (with an .xml extension). Instead of writing it manually, we can determine and build this path automatically by using Reflection, a C# technique that allows us to retrieve metadata on types at runtime. This programmatic approach is generally preferable, as it ensures better code maintainability than using literal strings, so we’ll opt for it. Open the Program.cs file, locate the AddSwaggerGen() method, and add the following code within its configuration block (new lines in bold):

using System.Reflection;                                           ❶

// ... existing code

```csharp
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename =                                              ❷
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(System.IO.Path.Combine(             ❸
        AppContext.BaseDirectory, xmlFilename));
```

// ... existing code

❶ Required namespace
❷ Builds the XML documentation filename
❸ Assembles the XML documentation file full path

Notice that in this code, we’re using Reflection to build an XML filename that matches the project’s name and then using it to construct the XML file’s full path. Next, we’ll test what we’ve done to see whether it works.

Testing the XML documentation Run the project in Debug mode, and look at the SwaggerUI main dashboard, where we should see the same descriptive strings that we used in the three-slash comments (figure 11.1).
Figure 11.1 XML documentation fetched by Swashbuckle and used in SwaggerUI

Notice that the summary appears right after the endpoint definition. The description is shown inside the endpoint’s expandable panel.

Evaluating XML documentation pros and cons Being able to translate all our code-level comments to API documentation automatically allows us to kill two birds with one stone. If we’re used to writing comments to describe our classes and methods (a developer’s good practice), we can reuse a lot of work. Furthermore, this approach can be particularly useful for internal developers, because they’ll be able to read our API documentation directly from the source code without even having to look at the swagger.json file and/or SwaggerUI.

But this notable benefit could easily become a downside. If we want to keep the internal source code documentation (for internal developers) separate from the public API documentation (for third- party developers/end users), for example, we could find this approach to be limited, not to mention that involves potential risk of an involuntary data leak. Code-level comments are typically considered to be confidential by most developers, who could likely use them to keep track of internal notes, warnings, known problems/bugs, vulnerabilities, and other strictly reserved data that shouldn’t be released to the public. To overcome such a problem, we might think about using the [SwaggerOperation] data attribute alternative, which provides better separation of concerns between internal comments and API documentation, along with some neat additional features that we may want to use.

### 11.2.4 Work with Swashbuckle annotations

In addition to XML documentation, Swashbuckle provides an alternative, attribute-based feature for adding custom descriptions to our web API endpoints. This feature is handled by an optional module called Swashbuckle.AspNetCore.Annotations, available with a dedicated NuGet package. In the following sections, we’ll learn how to install and use it.

Installing the NuGet package As always, to install the Swashbuckle annotations’ NuGet packages, we can use Visual Studio’s NuGet graphical user interface (GUI), the Package Manager console window, or the .NET command-line interface (CLI). Here are the commands to install them in the .NET CLI:

dotnet add package Swashbuckle.AspNetCore.Annotations --version 6.4.0

After installation, we’ll be able to use some data annotation attributes to enhance our API documentation. We’ll start with [SwaggerOperation], which allows us to set up custom summaries, descriptions, and/or tags for our controller’s action methods, as well as Minimal API methods.

Using the [SwaggerOperation] attribute Because our AccountController’s action methods have already been documented via XML, this time we’re going to use the BoardGamesController. Open the /Controllers/ BoardGamesController.cs file, and add the attributes to the four existing action methods as shown in listing 11.1 (new lines in bold).

**Listing 11.1 /Controllers/BoardGamesControllers.cs file: Adding annotations**

```csharp
using Swashbuckle.AspNetCore.Annotations;                               ❶

// ... existing code

[HttpGet(Name = "GetBoardGames")]
[ResponseCache(CacheProfileName = "Any-60")]
[SwaggerOperation(                                                       ❷
    Summary = "Get a list of board games.",                              ❸
    Description = "Retrieves a list of board games " +                   ❹
    "with custom paging, sorting, and filtering rules.")]
public async Task<RestDTO<BoardGame[]>> Get(

// ... existing code

[HttpGet("{id}")]
[ResponseCache(CacheProfileName = "Any-60")]
[SwaggerOperation(                                                       ❷
    Summary = "Get a single board game.",                                ❸
    Description = "Retrieves a single board game with the given Id.")]   ❹
public async Task<RestDTO<BoardGame?>> Get(int id)

// ... existing code

[Authorize(Roles = RoleNames.Moderator)]
[HttpPost(Name = "UpdateBoardGame")]
[ResponseCache(CacheProfileName = "NoCache")]
[SwaggerOperation(                                                       ❷
    Summary = "Updates a board game.",                                   ❸
    Description = "Updates the board game's data.")]                     ❹
public async Task<RestDTO<BoardGame?>> Post(BoardGameDTO model)

// ... existing code

[Authorize(Roles = RoleNames.Administrator)]
[HttpDelete(Name = "DeleteBoardGame")]
[ResponseCache(CacheProfileName = "NoCache")]
[SwaggerOperation(                                                       ❷
    Summary = "Deletes a board game.",                                   ❸
    Description = "Deletes a board game from the database.")]            ❹
public async Task<RestDTO<BoardGame?>> Delete(int id)

// ... existing code
```

❶ Required namespace
❷ [SwaggerOperation] attribute
❸ Adds the endpoint summary
❹ Adds the endpoint description

Now that we know how to use Swashbuckle annotations to describe our operations, let’s do the same with their input parameters. Using the [SwaggerParameter] attribute To set a description for input parameters, we can use the [SwaggerParameter] attribute, which is the counterpart to Swashbuckle annotations of the XML documentation’s <param> tag. But whereas the XML tag must be defined at method level and then bound to its corresponding parameter by means of the name attribute, the [SwaggerParameter] annotation must be defined above the parameter it’s meant to describe.

To understand how it works, let’s implement it. While keeping the BoardGamesController.cs file open, locate the Get() method, and add a [SwaggerParameter] to the existing input parameter in the following way (new lines in bold):

public async Task<RestDTO<BoardGame[]>> Get(
    [FromQuery]
    [SwaggerParameter("A DTO object that can be used " +   ❶
        "to customize the data-retrieval parameters.")]
    RequestDTO<BoardGameDTO> input)

❶ Adds the [SwaggerParameter]

Now that the descriptive attributes have been set, we need to enable the Swashbuckle annotations feature globally by updating our Swagger configuration.

Enabling annotations
To enable Swashbuckle annotations, open the Program.cs file,
and add the following configuration setting to the existing
AddSwaggerGen() method:
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();     ❶

// ... existing code

❶ Enables the Swashbuckle annotations feature

Adding Minimal API support The [SwaggerOperation] attribute, as well as the whole Swashbuckle annotations feature, works even with Minimal API methods. Let’s add some of those methods to the loop. Keeping the Program.cs file open, scroll down to the three Minimal API methods that we implemented in chapter 9 to test the ASP.NET Core authorization capabilities. Then add the [SwaggerOperation] attribute to them as shown in the following listing (new lines in bold).

**Listing 11.2 Program.cs file: Adding annotations to Minimal API methods**

```csharp
using Swashbuckle.AspNetCore.Annotations;                  ❶

// ... existing code

app.MapGet("/auth/test/1",
[Authorize]
[EnableCors("AnyOrigin")]
[SwaggerOperation(                                         ❷
    Summary = "Auth test #1 (authenticated users).",       ❸
    Description = "Returns 200 - OK if called by " +       ❹
    "an authenticated user regardless of its role(s).")]
[ResponseCache(NoStore = true)] () =>

// ... existing code

app.MapGet("/auth/test/2",
[Authorize(Roles = RoleNames.Moderator)]
[EnableCors("AnyOrigin")]
[SwaggerOperation(                                                  ❷
    Summary = "Auth test #2 (Moderator role).",                     ❸
    Description = "Returns 200 - OK status code if called by " +    ❹
    "an authenticated user assigned to the Moderator role.")]
[ResponseCache(NoStore = true)] () =>

// ... existing code

app.MapGet("/auth/test/3",
[Authorize(Roles = RoleNames.Administrator)]
[EnableCors("AnyOrigin")]
[SwaggerOperation(                                                  ❷
    Summary = "Auth test #3 (Administrator role).",                 ❸
    Description = "Returns 200 - OK if called by " +                ❹
    "an authenticated user assigned to the Administrator role.")]
[ResponseCache(NoStore = true)] () =>

// ... existing code
```

❶ Required namespace
❷ [SwaggerOperation] attribute
❸ Adds the endpoint summary
❹ Adds the endpoint description

Now we’re ready to test what we’ve done.

Testing annotations To test our new annotations, run our project in Debug mode, and take a look at the SwaggerUI main dashboard, where we should be able to see them (figure 11.2).
Figure 11.2 OpenAPI annotations added via the [SwaggerOperation] attribute

As we can see, the overall result is much like what we achieved by using the XML documentation approach. But there are some notable differences between what we can document with each technique. XML documentation, for example, allows us to describe examples (using the <example> element), which currently isn’t supported by Swashbuckle annotations. At the same time, the Swashbuckle annotations feature can be extended with custom schema filters to support virtually any documentation option mentioned in the Swagger/OpenAPI specifications. In the following sections, we’ll use both approaches in a complementary way to get the most out of them.

### 11.2.5 Describe responses

The same descriptive approach used for endpoints and input parameters should be also applied to our web API responses. This approach applies not only to the returned JavaScript Object Notation (JSON) data, but also to HTTP status codes (which should always be used according to their meaning) and relevant response headers (if any).

Again, to describe our responses, we can use the <response> XML documentation tag or the dedicated [SwaggerResponse] Swashbuckle annotation attribute. In the following sections, we’ll take both of these approaches.

Using XML documentation As we did earlier with the <param> tag, which can be used multiple times to describe each input parameter, we can create a <response> tag for any HTTP Status Code returned by the method. Each XML <response> tag requires a code attribute (to determine the HTTP Status Code of the response it describes) and a text-based value containing the actual description. To test it, again open the /Controllers/AccountController.cs file, and append the following <response> tags to the existing XML documentation comment block for the Register method (new lines in bold):

/// <summary>
/// Registers a new user.
/// </summary>
/// <param name="input">A DTO containing the user data.</param>
/// <returns>A 201 - Created Status Code in case of success.</returns>
/// <response code="201">User has been registered</response>   ❶
/// <response code="400">Invalid data</response>               ❷
/// <response code="500">An error occurred</response>          ❸

❶ HTTP Status Code 201 description
❷ HTTP Status Code 400 description
❸ HTTP Status Code 500 description

Next, scroll down to the Login method, and append the following <response> tags there as well:

/// <summary>
/// Performs a user login.
/// </summary>
/// <param name="input">A DTO containing the user's credentials.</param>
/// <returns>The Bearer Token (in JWT format).</returns>
/// <response code="200">User has been logged in</response>       ❶
/// <response code="400">Login failed (bad request)</response>    ❷
/// <response code="401">Login failed (unauthorized)</response>   ❸

❶ HTTP Status Code 200 description
❷ HTTP Status Code 400 description
❸ HTTP Status Code 401 description

To test what we’ve done, launch the project in Debug mode, access the SwaggerUI main dashboard, and expand the Account/Register and Account/Login endpoints. If we did everything properly, we should see our response description, as shown in figure 11.3.
Figure 11.3 Response descriptions for the /Account/Login endpoint

Now that we know how to obtain this outcome by using XML documentation comments, let’s see how to achieve the same thing with the [SwaggerResponse] data annotation attribute.

Using Swashbuckle annotations The [SwaggerResponse] attribute, like its <response> XML tag counterpart, can be added multiple times to the same method for the purpose of describing all the results, HTTP status codes, and response types that the affected method might send back to the client. Furthermore, it requires two main parameters:

The HTTP status code for the response to describe The description we want to show

The best way to learn how to use it is to see it in action. Open the Program.cs file, scroll down to the /auth/test/1 Minimal API endpoint, and add a new [SwaggerResponse] attribute to describe its unique response in the following way:

app.MapGet("/auth/test/1",
    [Authorize]
    [EnableCors("AnyOrigin")]
    [SwaggerOperation(
        Summary = "Auth test #1 (authenticated users).",
        Description = "Returns 200 - OK if called by " +
        "an authenticated user regardless of its role(s).")]
    [SwaggerResponse(StatusCodes.Status200OK,
        "Authorized")]                                  ❶
    [SwaggerResponse(StatusCodes.Status401Unauthorized,
        "Not authorized")]                              ❷

❶ HTTP Status Code 201 description
❷ HTTP Status Code 401 description

Notice that we used the StatusCodes enum provided by the Microsoft.AspNetCore .Http namespace, which allows us to specify the HTTP status codes by using a strongly typed approach.

NOTE One advantage of using the attribute-based method is that it grants us all the benefits provided by C# and ASP.NET Core features, including—yet not limited to—strongly typed members. As an example, we could specify different descriptions for different languages and/or cultures by using ASP.NET Core’s built-in localization support (which I don’t cover in this book for reasons of space).

To test the attribute, launch the project in Debug mode, access the SwaggerUI dashboard, and check for the presence of the preceding descriptions in the Responses section of the /auth/test/1 endpoint’s SwaggerUI panel (figure 11.4).

Figure 11.4 Response descriptions for the /auth/test/1 endpoint

Not bad. Most of our endpoints, however, don’t emit only an HTTP status code; in case of a successful request, they also return a JSON object with a well-defined, predetermined structure. Wouldn’t it be nice to describe those return types to our API users as well, to let them know what to expect? To achieve such a goal, we need to add some samples to these descriptions. In the next section, we’ll see how.

### 11.2.6 Add request and response samples

Ideally, each API operation should include a request and response sample so that users will understand how each one is expected to work. As we already know, our beloved SwaggerUI takes care of that task for the request part; it shows an Example Value tab containing a sample input data-transfer object (DTO) in JSON format whenever we use it, as shown in figure 11.5.

Figure 11.5 Response sample for the /Account/Register endpoint

To the right of the Example Value tab is a neat Schema tab showing the object’s schema and a lot of useful information, such as the maximum size, nullability, and underlying type of each field. Unfortunately, this automatic feature doesn’t always work for JSON response types, requiring some manual intervention.

NOTE Sometimes, the SwaggerUI manages to autodetect (and show an example of) the response type. If we expand the GET /BoardGames endpoint’s SwaggerUI panel, for example, the RestDTO<BoardGame> object is shown properly in the Responses section. Sadly, when the method has multiple return types, this convenient feature often fails to autodetect most of them. The method described in the next section takes care of those scenarios.

Let’s see how we can tell SwaggerUI to show a response sample whenever we want it. The [ProducesResponseType] attribute comes with the Microsoft.AspNetCore.Mvc namespace and isn’t part of Swashbuckle. But because we configured the component to take annotations into account, SwaggerUI will use it to determine the response type(s) of each method and act accordingly.

The main parameters to use with the [ProducesResponseType] attribute are the response type and the status code returned by the method. Again, because endpoints can return different response types and status codes, it can be added to each method multiple times. We already know that SwaggerUI is unable to autodetect the return types of the /Account/Register and /Account/Login endpoints, which makes them the perfect candidates for this attribute.

Open the /Controller/AccountController.cs file, and locate the Register action method. Then add the following attributes below the existing ones, right before the method’s declaration (new lines in bold):

[HttpPost]
[ResponseCache(CacheProfileName = "NoCache")]
[ProducesResponseType(typeof(string), 201)]                   ❶
[ProducesResponseType(typeof(BadRequestObjectResult), 400)]   ❷
[ProducesResponseType(typeof(ProblemDetails), 500)]           ❸

❶ HTTP Status Code 201 description
❷ HTTP Status Code 400 description
❸ HTTP Status Code 500 description Do the same with the Login action method, using the following attributes:

[HttpPost]
[ResponseCache(CacheProfileName = "NoCache")]
[ProducesResponseType(typeof(string), 200)]                   ❶
[ProducesResponseType(typeof(BadRequestObjectResult), 400)]   ❷
[ProducesResponseType(typeof(ProblemDetails), 401)]           ❸

❶ HTTP Status Code 200 description
❷ HTTP Status Code 400 description
❸ HTTP Status Code 401 description

To test what we’ve done, launch the project in Debug mode, and check out the Responses section of the /Account/Register and /Account/Login endpoints panels in SwaggerUI to ensure that they look like those in figure 11.6.
Figure 11.6 JSON samples for the /Account/Register return types

The screenshot depicted in figure 11.6 has been cropped because the JSON representation of the BadRequestObjectResult returned in case of HTTP Status Code 400 is long. But the figure should give us an idea of what we’ve done. Now that we know how to force SwaggerUI to provide a sample of our response types, we’re ready to master another good practice: endpoint grouping.

### 11.2.7 Group endpoints into sections

If a web API has lots of endpoints, it can be useful to group them into sections corresponding to their role/purpose. In our scenario, it could be wise to group the authentication endpoints, those that operate on the board-game entities, and so on. We could say that we’ve already done that because we used a controller for each of these groups, following the ASP.NET Core default behavior in this matter. As we’ve known since chapter 1, ASP.NET Core controllers allow us to group a set of action methods that have a common topic, meaning, or record type. We adopted this convention in our MyBGList scenario by using the BoardGamesController for the endpoints related to board games, the DomainsController for the domain-based endpoints, and so on.

This approach is enforced automatically by our current Open API implementation. If we look at our SwaggerUI dashboard, we see that the API endpoints handled by action methods pertaining to the same controller are grouped as shown in figure 11.7.
Figure 11.7 SwaggerUI group names for endpoints handled by controllers

As we can guess, these groups are generated automatically by Swashbuckle. This trick is performed by adding the controller’s name to the swagger.json file via the tags property, which is designed to handle grouping tasks.
TIP For additional information about Swagger’s tags property, check out http://mng.bz/5178.

To check, open the swagger.json file by clicking the hyperlink below the SwaggerUI main title, or navigate to https://localhost:40443/swagger/v1/swagger.json. The tags property for the /Account/Register endpoint is located near the beginning of the file:

{
    "openapi": "3.0.1",
    "info": {
       "title": "MyBGList",
       "version": "1.0"
    },
    "paths": {
      "/Account/Register": {   ❶
        "post": {
          "tags": [            ❷
             "Account"
          ],

❶ /Account/Register endpoint description
❷ "Account" tag taken from controller’s name

Unfortunately, this automatic behavior doesn’t work for Minimal API methods because they don’t belong to a controller. The only thing Swashbuckle can do is list them all in a generic group with the name of the app (MyBGList, in our scenario), as shown in figure 11.8.
Figure 11.8 Generic group for endpoints handled by Minimal API

The result of such a fallback behavior isn’t bad. But because our current Minimal API endpoints handle different sets of related tasks, we may want to find a better way to group them.

If we want to improve Swashbuckle’s default tagging behavior, we can use the Tags property provided by the [SwaggerOperation] attribute to override it. Let’s test it. Suppose that we want to group the three endpoints, starting with the /auth/ segment, in a new SwaggerUI section called "Auth". Open the Program.cs file; locate those methods; and make the following change to their existing [SwaggerOperation] attribute, starting with the /auth/test/1 endpoint (new lines in bold):

app.MapGet("/auth/test/1",
    [Authorize]
    [EnableCors("AnyOrigin")]
    [SwaggerOperation(
         Tags = new[] { "Auth" },    ❶
         Summary = "Auth test #1 (authenticated users).",
         Description = "Returns 200 - OK if called by " +
         "an authenticated user regardless of its role(s).")]

❶ Adds the Tags property

Do the same with the /auth/test/2 and /auth/test/3 methods, and then run the project in Debug mode to see the new Auth group (figure 11.9).
Figure 11.9 New Auth group for authorization-related endpoints

We can use the same technique to override Swashbuckle’s default behavior for the action methods belonging to a controller. Whenever the Tags parameter is present with a custom value, Swashbuckle will always use it to populate the swagger.json file instead of falling back to the controller’s or action method’s name.

NOTE This override feature can be handy if we want to customize the endpoint’s group names instead of using the controller names. It’s important to keep in mind, however, that this level of customization violates one of the most important development best practices enforced by ASP.NET Core: the convention over configuration design paradigm, which aims to limit the number of decisions that a developer is required to make, as well as the amount of source code, without losing flexibility. For that reason, I strongly suggest adhering to the ASP.NET Core grouping and tagging conventions for controllers, leaving the Tags property customization practice for the Minimal API methods and a limited amount of exceptions.

### 11.2.8 Exclude reserved endpoints

The ApiExplorer service Swashbuckle uses to find all the controller’s action methods and Minimal API methods in our project’s source code automatically and describe them in the swagger.json file, is a great feature in most cases. But we may want to hide some methods (or whole controllers) that we don’t want to show to our audience.

In our current scenario, this case could apply to the SeedController, which contains a couple of methods meant to be called and known only by administrators. It could be wise to exclude these operations from the swagger.json file, which will also take them out of the SwaggerUI.

To achieve this result, we can use the [ApiExplorerSettings] attribute, which contains a useful IgnoreApi property. This attribute can be applied to any controller, action method, or Minimal API method. Let’s use it to exclude our SeedController from the swagger.json file. Open the /Controllers/SeedController.cs file, and apply the attribute to the class declaration in the following way:

[Authorize(Roles = RoleNames.Administrator)]
[ApiExplorerSettings(IgnoreApi = true)]           ❶
[Route("[controller]/[action]")]
[ApiController]
public class SeedController : ControllerBase

❶ Excludes the controller from the swagger.json file

To test what we did, run the project in Debug mode; navigate to the SwaggerUI main dashboard; and confirm that the whole Seed section, which was present when we visited that page earlier, isn’t visible anymore.

WARNING It’s important to understand that the IgnoreApi = true setting will only prevent the controller and its action methods from being included in the swagger.json file; it doesn’t prevent users from calling (and potentially executing) it. That’s why we also restricted it to administrators by using the [Authorize] attribute in chapter 9.

Up to this point, we’ve learned how to configure the content of the swagger.json file and the resulting SwaggerUI layout by working on individual methods, using either XML documentation or data annotation attributes. In the next section, we’ll see how to perform these kinds of changes with a more structured and centralized approach, based on the use of Swashbuckle’s filters.

## 11.3 Filter-based Swagger customization

As we know from chapter 6, Swashbuckle exposes a convenient filter pipeline that hooks into the swagger.json file-generation process, allowing us to create and add our own filters to customize the file’s content according to our needs. To implement a filter, all we need to do is extend one of the built-in interfaces made available by Swashbuckle, each of them providing a convenient Apply method for customizing the autogenerated file. Here’s a comprehensive list of the filter interfaces made available by Swashbuckle:

IDocumentFilter—To customize the whole swagger.json file IOperationFilter—To customize operations/endpoints IParameterFilter—To customize the operations’ query string input parameters IRequestBodyFilter—To customize the operations’ request body input parameters ISchemaFilter—To customize the input parameters’ default schema

We used this feature in chapter 6 when we added a SortColumnFilter and SortOrderFilter (extending the IParameterFilter interface) to provide SwaggerUI some regular-expression-based patterns to validate some input parameters. Swashbuckle uses those filters, which we implemented in the /Swagger/ folder and then added to the Swashbuckle’s filter pipeline in the Program.cs file, to add the pattern JSON key selectively to all parameters decorated with the [SortColumnValidator] and [SortOrderValidator] custom attributes. What we did was a simple yet perfect example of how the filter pipeline works.

In this section, we’ll learn how to use the other filter interfaces provided by Swashbuckle to further configure our autogenerated swagger.json file, thus also updating the SwaggerUI accordingly. As always, we’ll assume that we’ve been asked to implement some credible new-feature requests.

### 11.3.1 Emphasizing the authorization requirements

In chapter 9, when we learned how to use the [Authorize] attribute, we added a security definition and a security requirement to our existing Swagger configuration settings. We did that to make the Authorize button appear in the SwaggerUI, which now allows us to set a bearer token and test our authorization-restricted endpoints. But this addition had a secondary effect that we deliberately overlooked at the time: it also added an odd padlock icon next to all our endpoints, as shown in figure 11.10.
Figure 11.10 The padlock icons in SwaggerUI

When clicked, those icons make the Authorization pop-up window appear, like the Authorize button near the top-right corner of the page that we used several times in chapter 9. The padlock icon is always shown as open, however, regardless of the endpoint’s authorization requirements, which isn’t the behavior we expect. Ideally, we want that padlock icon to appear only next to endpoints that require authorization of some sort. The next section explains how to achieve this result.

Before delving into the source code, let’s see how the padlock icon feature works under the hood. The SwaggerUI renders those icons automatically if the endpoint has a security requirement of some sort —in other words, if it requires some level of authorization. This information is taken from the swagger.json file, which assigns a security property to those endpoints:

```csharp
"security": [{
        "Bearer": [ ]
    }]
```

In chapter 9, when we configured Swagger to support our token- based authorization mechanism, we added a global security requirement to the swagger.json file-generator service, using a dedicated configuration option in our project’s Program.cs file:

// ...existing code

```csharp
options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Name = "Bearer",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        new string[]{}
    }
});
```

// ...existing code

Thanks to this global security requirement, the security property is being set on all our endpoints because they’re considered to be protected by a token-based authorization scheme—even if they aren’t. To patch this behavior, we need to replace that global requirement with a specific rule that will trigger only for methods that are restricted by such a scheme.

The most effective approach is to create a custom filter by using the IOperationFilter interface, which can extend the swagger.json generator service to provide additional info (or modify the existing/default info) for the affected operations. In our scenario, we want a filter that can set the same security requirements that we currently assign to all operations, but only for those with an [Authorize] attribute applied. To implement this requirement, create a new AuthRequirementFilter.cs class file inside the /Swagger/ root-level folder, and fill its content with the source code in the following listing.

**Listing 11.3 /Swagger/AuthRequirementFilter.cs file**

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyBGList.Swagger
{
    internal class AuthRequirementFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            if (!context.ApiDescription                                 ❶
                .ActionDescriptor
                .EndpointMetadata
                .OfType<AuthorizeAttribute>()
                .Any())
                return;                                                 ❷

            operation.Security = new List<OpenApiSecurityRequirement>   ❸
            {
                new OpenApiSecurityRequirement
                {
                    {
                           new OpenApiSecurityScheme
                           {
                               Name = "Bearer",
                               In = ParameterLocation.Header,
                               Reference = new OpenApiReference
                               {
                                   Type=ReferenceType.SecurityScheme,
                                   Id="Bearer"
                               }
                           },
                           new string[]{}
                      }
                  }
             };
         }
     }
}
```

❶ Checks for the [Authorize] attribute
❷ If not present, does nothing
❸ If present, secures the operation

As we can see, our new operation filter internally performs the same tasks that are currently being done in the Program.cs file. The only difference is that it skips the operations that don’t have the [Authorize] attribute, because we don’t want them to have any security requirement documented in the swagger.json file (or a padlock icon).

Now that we have our AuthRequirementFilter, we need to update the Swagger generator configuration options to use it instead of the global scale requirement we currently have. Open the Program.cs file; scroll down to the AddSwaggerGen method; and replace the existing AddSecurityRequirement statement with a new AddOperationFilter statement, as shown in the following code listing. (The previous code lines are commented out; new code lines are bold.)
**Listing 11.4 Program.cs file: AddSwaggerGen configuration update**

```csharp
using MyBGList.Swagger;                                           ❶

// ... existing code...

//options.AddSecurityRequirement(new OpenApiSecurityRequirement   ❷
//{
//    {
//        new OpenApiSecurityScheme
//        {
//            Name = "Bearer",
//            In = ParameterLocation.Header,
//            Reference = new OpenApiReference
//            {
//                Type=ReferenceType.SecurityScheme,
//                Id="Bearer"
//            }
//        },
//        new string[]{}
//    }
//});
options.OperationFilter<AuthRequirementFilter>();                 ❸

// ... existing code...
```

❶ Required namespace
❷ Previous code to remove
❸ New code to add

TIP In the GitHub repository for this chapter, I’ve commented out the previous code lines instead of deleting them.

To test what we did, we can launch the project in Debug mode and take another look at the same endpoints that previously had a padlock icon (figure 11.11). As we can see, the padlock icon has disappeared for the publicly accessible endpoints, but it’s still there for those that require authorization of some sort. Our custom IOperationFilter allowed us to do what we wanted to do.
Figure 11.11 The new behavior of padlock icons in SwaggerUI

### 11.3.2 Changing the application title

Suppose that we want to change the application’s title in the SwaggerUI, which is currently set to MyBGList—the same name as the ASP.NET Core project, per Swashbuckle’s default behavior. If we look at the swagger.json file, we can see that the JSON property hosting that value is called title and is part of a parent info property set at document level:

{ "openapi": "3.0.1", "info": { "title": "MyBGList Web API", "version": "1.0" },

This means that if we want to override it, we need to create a custom filter that allows us to customize the document-level parameters of the swagger.json file. The most effective way to achieve our goal is to create a custom DocumentFilter (by extending the IDocumentFilter interface) and add it to the filters pipeline. Create a new CustomDocumentFilter.cs file in the /Swagger/ root-level folder, and fill it with the content of the following listing.

**Listing 11.5 /Swagger/CustomDocumentFilter.cs file**

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyBGList.Swagger
{
    internal class CustomDocumentFilter : IDocumentFilter
    {
        public void Apply(
            OpenApiDocument swaggerDoc,
            DocumentFilterContext context)
        {
              swaggerDoc.Info.Title = "MyBGList Web API";   ❶
          }
      }
}
```

❶ Sets a custom title

Then hook the file to the Swashbuckle’s filter pipeline by updating the Program.cs file in the following way (new lines in bold):

options.OperationFilter<AuthRequirementFilter>();    ❶
options.DocumentFilter<CustomDocumentFilter>();      ❷
❶ Existing filter
❷ New filter

To test what we did, launch the project in Debug mode, and check out the new title of the SwaggerUI dashboard (figure 11.12).

Figure 11.12 SwaggerUI title changed with the CustomDocumentFilter

Not bad. Let’s see what we can do with the IRequestBodyFilter interface.

### 11.3.3 Adding a warning text for passwords

Suppose that we want to set up a custom warning text for our users whenever they need to send a password to our web API. By looking at our current endpoint, we can easily determine that for the time being, such a warning would affect only the AccountController’s Register and Login methods. By taking that fact into account, we could insert this message into the operation’s Summary or Description property, using either the XML documentation comments (section 11.2.3) or the [SwaggerOperation] attribute (section 11.2.4), as we learned to do earlier. Alternatively, we could work at parameter level by using the <param> XML tag or the [SwaggerParameter] attribute.

Both approaches have a nontrivial downside. If we add endpoints that accept a password in the future, we’ll have to repeat the XML tag or data annotation attribute there as well, which would mean replicating a lot of code—unless we forget to do that, because such a method would be error-prone.

To overcome such problems, it would be better to find a way to centralize this behavior by creating a new filter and adding it to Swashbuckle’s pipeline. We need to determine which filter interface to extend among the available ones. Ideally, the IRequestBodyFilter interface would be a good choice, considering that we want to target a specific parameter with a name equal to "password", which currently comes (and will likely always come) with POST requests. Let’s proceed with this approach. Create a new PasswordRequestFilter.cs file in the /Swagger/ root folder, and fill it with the code in the following listing.

**Listing 11.6 /Swagger/PasswordRequestFilter.cs file**

```csharp
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyBGList.Swagger
{
    internal class PasswordRequestFilter : IRequestBodyFilter
    {
        public void Apply(
            OpenApiRequestBody requestBody,
            RequestBodyFilterContext context)
          {
               var fieldName = "password";                           ❶

               if (context.BodyParameterDescription.Name
                   .Equals(fieldName,
                         StringComparison.OrdinalIgnoreCase)         ❷
                     || context.BodyParameterDescription.Type
                     .GetProperties().Any(p => p.Name
                         .Equals(fieldName,
                             StringComparison.OrdinalIgnoreCase)))   ❸
               {
                     requestBody.Description =
                         "IMPORTANT: be sure to always use a strong password " +
                         "and store it in a secure location!";
               }
          }
     }
}
```

❶ Input parameter name
❷ Name check (primitive type)
❸ Property check (complex type)

As we can see by looking at this code, we check whether the input parameter name is equal to "password" (for primitive types) or contains a property with that name (for complex types, such as DTOs). Now that we have the filter, we need to register it in Swashbuckle’s filter pipeline in the following way, below the AuthRequirementFilter and CustomDocumentFilter that we added earlier:

```csharp
options.OperationFilter<AuthRequirementFilter>();          ❶
options.DocumentFilter<CustomDocumentFilter>();            ❶
options.RequestBodyFilter<PasswordRequestFilter>();        ❷
```

❶ Existing filters
❷ New filter As always, we can test what we did by executing our project in Debug mode and checking the SwaggerUI for the expected changes (figure 11.13).

Figure 11.13 The new description added by the PasswordRequestFilter

The changes seem to work. Thanks to such an approach, our password warning message will cover our two existing endpoints and any future endpoints that accept a password parameter in their request body.

NOTE If we want to extend our coverage to query string parameters, we need to add another filter that extends the IParameterFilter interface and does the same job, and then register it in the Program.cs file by using the ParameterFilter helper method. All that’s left to do now to complete our filters overview is the ISchemaFilter interface.

### 11.3.4 Adding custom key/value pairs

Let’s take another look at the SortColumnFilter and SortOrderFilter classes that we implemented in chapter 6. Extending the IParameterFilter interface was a good idea, because we only had to handle some specific input parameters coming from the query string. In other words, we wanted to add the pattern key to the JSON schema of those parameters in the swagger.json file, taking the value from the same data annotation attribute—[SortColumnAttribute] or [SortOrderAttribute]—used to identify them.

Suppose that we want to extend that approach to implement a new filter that’s capable of adding any arbitrary JSON key (and value) to any property, whether it’s a request parameter, a response parameter, or anything else. In this section, we’ll achieve this goal by implementing the following:

A custom data annotation attribute, which will allow us to set one or more custom JSON Key and Value pairs to any property A custom SchemaFilter that extends the ISchemaFilter interface, adding those Key and Value pairs to all parameters, responses, and properties that have those data annotation attributes applied to them

The ISchemaFilter interface is the perfect choice to deal with this task, as it’s specifically designed to postmodify the JSON schema generated by Swashbuckle’s SwaggerGen service for every input and output parameter and complex types exposed by controller actions and Minimal API methods. Now that we’ve chosen our route, let’s put it into practice.

Implementing the CustomKeyValueAttribute In Visual Studio’s Solution Explorer panel, right-click the /Attributes/ folder in the MyBGList project’s root, and add a new CustomKeyValueAttribute.cs class file with two string properties: Key and Value. The following listing provides the source code for the new class.

**Listing 11.7 CustomKeyValueAttribute**

```csharp
namespace MyBGList.Attributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Parameter,
        AllowMultiple = true)]
    public class CustomKeyValueAttribute : Attribute
    {
        public CustomKeyValueAttribute(string? key, string? value)
        {
            Key = key;
            Value = value;
        }

        public string? Key { get; set; }

        public string? Value { get; set; }
    }
}
```

Notice that we’ve decorated our new class with the [AttributeUsage] attribute, which allows us to specify the use of the attribute. We did that for two important reasons: To allow the attribute to be applied to properties and parameters, using the AttributeTargets enum. To allow the attribute to be applied multiple times because the AllowMultiple property is set to true. This setting is required because we want the chance to apply multiple [SwaggerSchema] attributes (thus setting multiple custom key/value pairs) to a single property or parameter.

Now that we have the attribute, we’re ready to implement the filter that will handle it.

Implementing the CustomKeyValueFilter Add a new CustomKeyValueFilter.cs class file within the /Swagger/ folder. The new class must implement the ISchemaFilter interface and its Apply method, which is where we’ll handle the [CustomKeyValue] attribute lookup and JSON key/value pair insertion process. The following listing shows how.

**Listing 11.8 CustomKeyValueFilter**

```csharp
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyBGList.Attributes
{
    public class CustomKeyValueFilter : ISchemaFilter
    {
        public void Apply(
            OpenApiSchema schema,
            SchemaFilterContext context)
        {
            var caProvider = context.MemberInfo
                ?? context.ParameterInfo
                as IcustomAttributeProvider;            ❶
               var attributes = caProvider?
                   .GetCustomAttributes(true)
                     .OfType<CustomKeyValueAttribute>();    ❷

               if (attributes != null)                     ❸
               {
                   foreach (var attribute in attributes)
                   {
                       schema.Extensions.Add(
                           attribute.Key,
                           new OpenApiString(attribute.Value)
                           );
                   }
               }
          }
     }
}
```

❶ Determines whether we’re dealing with a property or a parameter
❷ Checks whether the parameter has the attribute(s)
❸ If one or more attributes are present, acts accordingly

This code should be simple to understand. We’re checking the context provided by the ISchemaFilter interface by using Language Integrated Query (LINQ) to determine whether our property or parameter has one or more [CustomKeyValue] attributes applied and act accordingly. All we need to do now is add the new filter to Swashbuckle’s filter pipeline. As always, we can update the Program.cs file in the following way:

```csharp
options.OperationFilter<AuthRequirementFilter>();   ❶
options.DocumentFilter<CustomDocumentFilter>();     ❶
options.RequestBodyFilter<PasswordRequestFilter>(); ❶
options.SchemaFilter<CustomKeyValueFilter>();       ❷
```

❶ Existing filters
❷ New filter Now that our two classes are ready and the filter has been registered, we can test the [CustomKeyValue] attribute by applying it to a property of one of our existing DTOs. Let’s pick the LoginDTO used by the AccountController’s Login action method. Open the /DTO/LoginDTO.cs file, and apply a couple of these attributes to the existing UserName property in the following way:

```csharp
[Required]
[MaxLength(255)]
[CustomKeyValue("x-test-1", "value 1")]   ❶
[CustomKeyValue("x-test-2", "value 2")]   ❶
public string? UserName { get; set; }
```

❶ First CustomKeyValue attributes

Next, run the project in Debug mode, access the SwaggerUI dashboard, and click to the swagger.json file link below the main title (figure 11.14) to open it in a new tab.

Figure 11.14 swagger.json file URL Use the browser’s Search feature to look for the "x-test-" string within the swagger.json file. If we did everything properly, we should see two entries of this string in the JSON schema of the LoginDTO’s username property, as shown in the following listing.

**Listing 11.9 swagger.json file (LoginDTO schema)**

```json
      "LoginDTO": {
        "required": [
           "password",
           "userName"
        ],
        "type": "object",
        "properties": {
           "userName": {
             "maxLength": 255,
             "minLength": 1,
             "type": "string",
              "x-test-1": "value 1",   ❶
               "x-test-2": "value 2"   ❶
            },
            "password": {
               "minLength": 1,
               "type": "string"
            }
        }
```

❶ Custom key/value pairs

So far, so good. Let’s perform another test to ensure that the same logic will work for a standard GET parameter of a primitive type. Open the /Controllers/BoardGamesController.cs file, scroll down to the Get action method accepting a single id parameter of int type, and add a [CustomKeyValue] attribute to that parameter in the following way:

[HttpGet("{id}")]
[ResponseCache(CacheProfileName = "Any-60")]
[SwaggerOperation(
    Summary = "Get a single board game.",
    Description = "Retrieves a single board game with the given Id.")]
public async Task<RestDTO<BoardGame?>> Get(
    [CustomKeyValue("x-test-3", "value 3")]   ❶
    int id
    )

❶ Adds a new [CustomKeyValue] attribute

Next, run the project in Debug mode, access the swagger.json file contents as we did earlier, and check again for the presence of the of the "x-test-" string within it. This time, we should locate three entries, the last of which is the one we added (see the following listing).

**Listing 11.10 swagger.json file (/BoardGames/{id} endpoint schema) "/BoardGames/{id}": { "get": { "tags": [ "BoardGames" ], "summary": "Get a single board game.", "description": "Retrieves a single board game with the given Id.", "parameters": [ { "name": "id", "in": "path", "required": true, "schema": { "type": "integer", "format": "int32", "x-test-3": "value 3"   ❶ } } ],**

```json
```

❶ Custom key/value pair

Our custom key/value feature seems to be working well. This last task concludes our journey through Swashbuckle’s filter pipeline and our API documentation overview. The only thing left to do now is learn to deploy our web API project to production, which is the topic of chapter 12.

## 11.4 Exercises

It’s time to challenge ourselves with a new streak of hypothetical task assignments given by our product owner. As always, dealing with these tasks will greatly help us memorize and remember the concepts covered and the techniques learned throughout this chapter.

NOTE The solutions of the exercises are available on GitHub in the /Chapter_11/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 11.4.1 Use XML documentation

Describe the GET /Domains endpoint in the following way, using the XML documentation approach:

Summary—Gets a list of domains Description—Retrieves a list of domains with custom paging, sorting, and filtering rules Parameter—A DTO object that can be used to customize some retrieval parameters Returns—A RestDTO object containing a list of domains

TIP The description can be added with the <remarks> XML element.

### 11.4.2 Use Swashbuckle annotations

Describe the GET /Mechanics endpoint in the following way, using the Swashbuckle annotations approach:

Summary—Gets a list of mechanics Description—Retrieves a list of mechanics with custom paging, sorting, and filtering rules Parameter—A DTO object that can be used to customize some retrieval parameters Returns—A RestDTO object containing a list of mechanics

### 11.4.3 Exclude some endpoints

Use the [ApiExplorerSettings] attribute to hide the following endpoints from the swagger.json file:

POST /Domains DELETE /Domains

Then ensure that these endpoints are also excluded from the SwaggerUI dashboard.

### 11.4.4 Add a custom filter

Extend the IRequestBodyFilter interface to implement a new UsernameRequestFilter that will add the following description to any input parameter with a name equal to "username". Then register the new filter within Swashbuckle’s filter pipeline, and test it in the SwaggerUI dashboard by checking the sername parameter used by the POST Account/Login and POST Account/Register endpoints.

WARNING Be sure to remember your username, as you’ll need it to perform the login!

### 11.4.5 Add custom key/value pairs

Use the [CustomKeyValue] attribute to add the following key/value pairs to the existing id parameter of the DELETE Mechanics endpoint:

Key: x-test-4, Value: value 4 Key: x-test-5, Value: value 5

Then check for the presence of the new properties in the endpoint’s JSON schema within the swagger.json file.

Summary Well-written documentation can greatly increase or speed the adoption of a web API. For that reason, it’s important to identify the API documentation best practices and learn to follow them, using ASP.NET Core built-in and third-party tools. Identifying the potential audience for our web API—the stakeholders who are expected to choose and/or use it—can help us write compelling documentation. Ideally, we need to satisfy Early adopters who are eager to try what we did (prospectors). IT solution architects who aim to evaluate our work (contractors). Software developers who will be asked to implement our web API’s endpoints (builders). Focusing on the needs of the builders without forgetting the other two audience groups (prospectors and contractors) is almost always the way to go. Developers are analytical, precise, and demanding. To meet their expectations, it’s important to adhere to some well-known documentation best practices widely adopted by the IT industry, including adopting an automated description tool. describing endpoints, input parameters, and responses. providing request and response samples. grouping endpoints into sections. emphasizing the authorization requirements. customizing the documentation context. The Swagger/OpenAPI framework provides a standardized approach for documenting and describing APIs, using a common language that everyone can understand. We can use Swagger to create the documentation for our web API thanks to Swashbuckle: a set of services, middleware, and tools that allows us to implement the OpenAPI specification within ASP.NET Core while following the best practices that we identified earlier. Swashbuckle exposes a convenient set of data attributes, as well as a powerful filter pipeline, that can be used to postmodify the autogenerated swagger.json file, thus customizing the API documentation to suit our needs. Swashbuckle’s features allow us to improve the description of operations, input parameters, and output parameters, as well as add custom key/value pairs to the existing JSON schemas.
