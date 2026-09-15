---
title: Data validation and error handling
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 6
---

# 6. Data validation and error handling

**This chapter covers**

- Overview of model binding and data validation
- Built-in and custom validation attributes
- ModelState validation approaches
- Error and exception handling techniques

For simplicity, up to this point we’ve assumed that the data coming from clients is always correct and adequate for our web API’s endpoints. Unfortunately, this is not always the case: whether we like it or not, we often have to deal with erroneous HTTP requests, which can be caused by several factors (including malicious attacks) but always occur because our application is facing unexpected or unhandled behavior.

In this chapter, we’ll discuss a series of techniques for handling unexpected scenarios during the client-server interaction. These techniques rely on two main concepts:

Data validation—A set of methods, checks, routines, and rules to ensure that the data coming into our system is meaningful, accurate, and secure and therefore is allowed to be processed Error handling—The process of anticipating, detecting, classifying, and managing application errors that might happen within the program execution flow

In the upcoming sections, we’ll see how we can put them into practice within our code.

## 6.1 Data validation

We know from chapter 1 that the primary purpose of a web API is to enable different parties to interact by exchanging information. In later chapters, we saw how to implement several HTTP endpoints that can be used to create, read, update, and delete data. Most (if not all) of these endpoints require an input of some sort from the calling client. As an example, consider the parameters expected by the GET /BoardGames endpoint, which we greatly improved in chapter 5:

pageIndex—An optional integer value to set the starting page of the board games to return pageSize—An optional integer value to set the size of each page sortColumn—An optional string value to set the column to sort the returned board games sortOrder—An optional string value to set the sort order filterQuery—An optional string value that, if present, will be used to return only board games with a Name that contains it

All these parameters are optional. We’ve chosen to allow their absence—in other words, to accept incoming requests without them —because we could easily provide suitable default values in case they’re not explicitly provided by the caller. For that reason, all the following HTTP requests will be handled in the same way and therefore will provide the same outcome (until the default values change):

https://localhost:40443/BoardGames https://localhost:40443/BoardGames? pageIndex=0&pageSize=10 https://localhost:40443/BoardGames? pageIndex=0&pageSize=10&sortColumn=Name&sortOrder=AS C

At the same time, we require some of these parameters’ values to be compatible with a given .NET type instead of being raw strings. That’s the case with pageIndex and pageSize, whose values are expected to be of type integer. If we try to pass even one of them with an incompatible value, such as the HTTP request https://localhost:40443/BoardGames?pageIndex=test, our application will respond with an HTTP 400 - Bad Request error without even starting to execute the BoardGamesController’s Get action method:

{ "type":"https://tools.ietf.org/html/rfc7231#section-6.5.1", "title":"One or more validation errors occurred.", "status":400, "traceId":"00-a074ebace7131af6561251496331fc65-ef1c633577161417-00", "errors":{ "pageIndex":["The value 'string' is not valid."] } }

We can easily see that by allowing and/or rejecting such requests, we’re already performing some sort of data validation activity on those parameters by actively checking two important acceptance criteria:

Providing an undefined value for each parameter is OK because we have server-defined fallbacks (the action method’s default values). Providing a noninteger value for pageIndex and pageSize is not OK because we expect them to be of an integer type.

We’re obviously talking about an implicit activity, because the null- checking and fallback-to-default tasks are conducted by the framework under the hood, without our having to write anything. Specifically, we’re taking advantage of the ASP.NET Core’s model binding system, which is the mechanism that automatically takes care of all that.

### 6.1.1 Model binding

All the input data coming from HTTP requests—request headers, route data, query strings, form fields, and so on—is transmitted by means of raw strings and received as such. The ASP.NET Core framework retrieves these values and converts them from strings to .NET types automatically, saving the developer from the tedious, error-prone manual activity. Specifically, the model binding system retrieves the input data from the HTTP request query string and/or body and converts them to strongly typed method parameters. This process is performed automatically upon each HTTP request, but it can be configured with a set of attribute-based conventions according to the developer’s requirements.

Let’s see what model binding does under the hood. Consider the HTTP GET request https://localhost:40443/BoardGames? pageIndex=2&pageSize=50, which is routed to our BoardGamesController’s Get action method:

public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, int pageSize = 10, string? sortColumn = "Name", string? sortOrder = "ASC", string? filterQuery = null)

The model binding system performs the following tasks:

Identifies the presence of the pageIndex and pageSize GET parameters Retrieves their raw string values ("2" and "50"), converts them to integer types (2 and 50), and assigns the converted values to the corresponding action method’s properties Identifies the absence of the sortColumn, sortOrder, and filterQuery GET parameters, and assigns a null value to the corresponding action method’s properties so that the respective default values will be used instead

In a nutshell, the main purpose of the model binding system is to convert a given (raw string) source to one or more expected (.NET typed) targets. In our example, the raw GET parameters issued by the URL are the model binding’s sources, and the action method’s typed parameters are the targets. The targets can be simple types (integer, bool, and the like) or complex types (such as data-transfer objects [DTOs]), as we’ll see later.

### 6.1.2 Data validation attributes

In addition to performing standard type conversion, model binding can be configured to perform several data validation tasks by using a set of built-in data annotation attributes included in the System.ComponentModel.DataAnnotation namespace. Here’s a list of the most notable of those attributes: [CreditCard]—Ensures that the given input is a credit card number [EmailAddress]—Ensures that the given string input has an email address format [MaxLength(n)]—Ensures that the given string or array input has a length smaller than or equal to the specified value [MinLength(n)]—Ensures that the given string or array input has a length equal to or greater than the specified value [Range(nMin, nMax)]—Ensures that the given input falls between the minimum and maximum specified values [RegularExpression(regex)]—Ensures that the given input matches a given regular expression [Required]—Ensures that the given input has a non-null value [StringLength]—Ensures that the given string input doesn’t exceed the specified length limit [Url]—Ensures that the given string input has a URL format

The best way to learn how to use these validation attributes is to
implement them within our MyBGList web API. Suppose that we
want (or are asked) to limit the page size of our GET
/BoardGames endpoint to a maximum value of 100. Here’s how
we can do that by using the [Range] attribute:
         public async Task<RestDTO<BoardGame[]>> Get(
             int pageIndex = 0,
             [Range(1, 100)] int pageSize = 10,   ❶
             string? sortColumn = "Name",
             string? sortOrder = "ASC",
             string? filterQuery = null)

❶ Range validator (1 to 100)

NOTE This change request is credible. Accepting any page size without limits means allowing potentially expensive data-retrieval requests, which could result in HTTP response delays, slowdowns, and performance drops, thus exposing our web application to denial- of-service (DoS) attacks.

This change would cause the URL https://localhost:40443/BoardGames?pageSize=200 to return an HTTP 400 - Bad Request status error instead of the first 200 board games. As we can easily understand, data annotation attributes can be useful whenever we want to put some boundaries around the input data without implementing the corresponding checks manually. If we don’t want to use the [Range] attribute, we could obtain the same outcome with the following code:

```csharp
if (pageSize < 0 || pageSize > 100) {
  // .. do something
}
```

That "something" could be implemented in various ways, such as raising an exception, returning an HTTP error status, or performing any other suitable error handling action. Manual approaches can be hard to maintain, however, and they’re often prone to human error. For that reason, the best practice in working with ASP.NET Core is to adopt the centralized interface provided by the framework as long as we can use it to achieve what we need to do.

DEFINITION This approach is known as aspect-oriented programming (AOP), a paradigm that aims to increase the modularity of the source code by adding behavior to existing code without modifying the code itself. The data annotation attributes provided by ASP.NET are a perfect example because they allow the developer to add functionalities without cluttering the code.

### 6.1.3 A nontrivial validation example

The [Range(1, 100)] validator that we used to limit page size was easy to pull off. Let’s try a more difficult change request. Suppose that we want (or are asked) to validate the sortOrder parameter, which currently accepts any string, to accept only a value that can be considered valid for its specific purpose, which is either "ASC" or "DESC". Again, this change request is more than reasonable. Accepting an arbitrary string value for a parameter such as sortOrder, which is used programmatically to compose a LINQ expression with Dynamic LINQ, could expose our web application to dangerous vulnerabilities, such as SQL injections or LINQ injections. For that reason, providing our app a validator for these “dynamic” strings is a security requirement that we should take care of.

TIP For additional info regarding this topic, check out the StackOverflow thread at http://mng.bz/Q8w4.

Again, we could easily implement this change request by taking a programmatic approach, making the following “manual check” within the action method itself: if (sortOrder != "ASC" && sortOrder != "DESC") { // .. do something }

But we have at least two other ways to achieve the same outcome with the built-in validator interface provided by ASP.NET Core: using the [RegularExpression] attribute or implementing a custom validation attribute. In the upcoming sections, we’ll use both techniques.

Using the RegularExpression attribute The RegularExpressionAttribute class is one of the most useful and most customizable data annotation attributes because it uses the power and flexibility of regular expressions. The [RegularExpression] attribute relies on the .NET regular expression engine, represented by the System.Text.RegularExpressions namespace and its Regex class. This engine accepts regular expression patterns written using Perl 5-compatible syntax and uses them against the input string to determine matches, retrieve occurrences, or replace the matching text, depending on the method being used. Specifically, the attribute calls the IsMatch() method internally to determine whether the pattern finds a match in the input string, which is precisely what we need in our scenario.

Regular expressions Regular expressions (also known as RegEx and RegExp) are standardized patterns used to match character combinations in strings. The technique originated in 1951 but became popular only during the late 1980s, thanks to the worldwide adoption of the Perl language (which has featured a regex library since 1986). Also, in the 1990s, the Perl Compatible Regular Expression (PCRE) library was adopted by many modern tools (such as PHP and Apache HTTP Server), becoming a de facto standard.

Throughout this book, we will rarely use regular expressions and only to a basic extent. To find out more about the topic, check the following website, which provides some insightful tutorials, examples, and a quick-start guide: https://www.regular- expressions.info.

Here’s a suitable RegEx pattern that we can use to check for the presence of either the ASC or DESC string:

ASC|DESC

This pattern can be used within a [RegularExpression] attribute in our BoardGamesController’s Get action method in the following way:

public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, [Range(1, 100)] int pageSize = 10, string? sortColumn = "Name", [RegularExpression("ASC|DESC")] string? sortOrder = "ASC", string? filterQuery = null)

Afterward, all incoming requests containing a sortOrder parameter value different from "ASC" and "DESC" will be considered to be invalid, resulting in an HTTP 400 - Bad Request response.

Using a Custom Validation Attribute If we don’t want to fulfill our change request by using the [RegularExpression] attribute, we can achieve the same outcome with a custom validation attribute. All the existing validation attributes extend the ValidationAttribute base class, which provides a convenient (and overridable) IsValid() method that performs the actual validation tasks and returns a ValidationResult object containing the outcome. To implement our own validation attribute, we need to perform the following steps:

1. Add a new class file, which will contain our custom validator’s source code.
2. Extend the ValidationAttribute base class.

3. Override the IsValid method with our own implementation.
4. Configure and return a ValidationResult object containing the outcome.

Adding the SortOrderValidator class file In Visual Studio’s Solution Explorer, create a new /Attributes/ folder in the MyBGList project’s root. Then right-click the folder, and add a new SortOrderValidatorAttribute.cs class file to generate an empty boilerplate within a new MyBGList.Attributes namespace. Now we’re ready to implement our custom validator.

Implementing the SortOrderValidator The following listing provides a minimal implementation that checks the input string against the "ASC" and "DESC" values, returning a successful result only if an exact match of one of them occurs.

**Listing 6.1 SortOrderValidatorAttribute**

```csharp
using System.ComponentModel.DataAnnotations;

namespace MyBGList.Attributes
{
    public class SortOrderValidatorAttribute : ValidationAttribute
    {
        public string[] AllowedValues { get; set; } =
            new[] { "ASC", "DESC" };
        public SortOrderValidatorAttribute()
            : base("Value must be one of the following: {0}.") { }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue)
                && AllowedValues.Contains(strValue))
                return ValidationResult.Success;

            return new ValidationResult(
                FormatErrorMessage(string.Join(",", AllowedValues))
            );
        }
    }
}
```

The code is easy to read. The input value is checked against an array of allowed string values (the AllowedValues string array) to determine whether it’s valid. Notice that if the validation fails, the resulting ValidationResult object is instantiated with a convenient error message that will give the caller some useful contextual info about the failed check. Default text for this message is defined in the constructor, but we can change it by using the public ErrorMessage property provided by the ValidationAttribute base class in the following way:

[SortOrderValidator(ErrorMessage = "Custom error message")]

Also, we set the AllowedValues string array property as public, which gives us the chance to customize these values in the following way:

[SortOrderValidator(AllowedValues = new[] { "ASC", "DESC", "OtherString" })]

TIP Customizing the allowed sorting values could be useful in some edge-case scenarios, such as replacing SQL Server with a database management system (DBMS) that supports a different sorting syntax. That’s why we defined a set accessor for that property.

Now we can go back to our BoardGamesController’s Get method and replace the [RegularExpression] attribute that we added earlier with our new [SortOrderValidator] custom attribute:

using MyBGList.Attributes;

// ...

public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, [Range(1, 100)] int pageSize = 10, string? sortColumn = "Name", [SortOrderValidator] string? sortOrder = "ASC", string? filterQuery = null)

Implementing the SortColumnValidator Before going further, let’s implement another custom validator to fix an additional security issue in the going BoardGamesControlle’s Get action method: the sortColumn parameter. Again, we must deal with an arbitrary, user-provided string parameter used to build a LINQ expression tree dynamically, which could expose our web application to some LINQ injection attacks. The least we can do to prevent these kinds of threats is to validate that string accordingly.

This time, however, the “allowed” values are determined by the properties of the [BoardGame] database table, which is represented in our codebase by the BoardGame entity. We can take either of two approaches:

Use fixed strings to hardcode all the BoardGame entity’s property names and proceed as we did with the "ASC" and "DESC" values. Find a way to check the entity’s property names dynamically against the given input string.

The first approach is easy to pull off by using a [RegularExpression] attribute or a custom attribute similar to the SortOrderValidator we created. This solution could be quite hard to maintain in the long run, however, especially if we plan to add more properties to the BoardGame entity. Furthermore, it won’t be flexible enough to use with entities such as Domains, Mechanics, and so on unless we pass the whole set of “valid” fixed strings as a parameter each time. The dynamic approach could be the better choice, especially considering that we can make it accept an EntityType property, which we can use to pass the entity type to check. Then it would be easy to use LINQ to iterate through all the EntityType’s properties to check whether one of them matches the input string. The following listing shows how we can implement this approach in a new SortColumnValidatorAttribute.cs file.

**Listing 6.2 SortColumnValidatorAttribute**

```csharp
using System.ComponentModel.DataAnnotations;

namespace MyBGList.Attributes
{
    public class SortColumnValidatorAttribute : ValidationAttribute
    {
        public Type EntityType { get; set; }

        public SortColumnValidatorAttribute(Type entityType)
            : base("Value must match an existing column.")
        {
            EntityType = entityType;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (EntityType != null)
            {
                var strValue = value as string;
                if (!string.IsNullOrEmpty(strValue)
                    && EntityType.GetProperties()
                        .Any(p => p.Name == strValue))
                    return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
```

As we can see, the core part of the IsValid() method’s source code relies on the GetProperties() method, which returns an array of PropertyInfo objects corresponding to the type’s properties.

WARNING The IsValid() method, as we have implemented it, will consider any property valid for sorting purposes, as long as it exists: although such an approach might work in our specific scenario, it’s hardly the most secure choice when dealing with entities with private properties, public properties that contain personal or sensitive data, and the like. To better understand this potential problem, consider having a User entity with a Password property containing the password hash. We wouldn’t want to allow clients to sort a list of users by using that property, right? These kinds of problems can be solved by tweaking the preceding implementation to exclude some properties explicitly or (better) by enforcing the good practice of always using DTOs instead of entity classes when interacting with clients unless we’re 100 percent sure that the entity’s data won’t pose any threat.

The technique of programmatically reading/inspecting the code’s metadata that we used here is known as reflection. It is supported in most programming frameworks through a set of dedicated libraries or modules. In .NET, this approach is available through the classes and methods provided by the System.Reflection namespace.

TIP For additional info about the reflection technique, check out the following guide: http://mng.bz/X57E.

Now that we have our new custom validation attribute, we can use it in our BoardGamesController’s Get method in the following way: public async Task<RestDTO<BoardGame[]>> Get( int pageIndex = 0, [Range(1, 100)] int pageSize = 10, [SortColumnValidator(typeof(BoardGameDTO))] string? sortColumn = "Name", [SortOrderValidator] string? sortOrder = "ASC", string? filterQuery = null)

Notice that we used the BoardGameDTO instead of the BoardGame entity for the SortColumnValidator’s EntityType parameter, thus following the single-responsibility principle introduced in chapter 5. Using DTOs instead of entity types whenever we exchange data with clients is a good practice that will greatly increase the security posture of our web application. For that reason, I suggest always following this practice, even if it requires additional work.

### 6.1.4 Data validation and OpenAPI

Thanks to the introspection activity of the Swashbuckle middleware, the criteria followed by the model binding system are documented automatically in the autogenerated swagger.json file, which represents the OpenAPI specification file for our web API’s endpoints (chapter 3). We can check out such behavior by executing the URL https://localhost:40443/swagger/v1/swagger.json and then looking at the JSON content of the file. Here’s an excerpt containing the first two parameters of the GET /BoardGames endpoint:

{
    "name": "PageIndex",   ❶
    "in": "query",
    "schema": {
     "type": "integer",    ❷
     "format": "int32",    ❸
     "default": 0          ❹
     }
},
{
     "name": "PageSize",      ❺
     "in": "query",
     "schema": {
       "maximum": 100,
       "minimum": 1,
         "type": "integer",   ❻
         "format": "int32",   ❼
         "default": 10        ❽
     }
}

❶ Parameter name
❷ Parameter type
❸ Parameter format
❹ Parameter default value
❺ Parameter name
❻ Parameter type
❼ Parameter format
❽ Parameter default value

As we can see, everything worth noting about our parameters is documented there. Ideally, the clients that consume our web API will use this information to create a compatible user interface that can be used to interact with our data in the best possible way. A great example is the SwaggerUI, which uses the swagger.json file to create the input forms that we can use to test our API endpoints. Execute the URL https://localhost:40443/swagger/index.xhtml, expand the GET/BoardGames endpoint panel by using the right handle, and check the list of parameters on the Parameters tab (figure 6.1).
Figure 6.1 GET /BoardGames endpoint parameter info

The type, format, and default value info for each parameter are well documented. If we click the Try It Out button, we can access the edit mode of that same input form, where we can fill the text boxes with actual values. If we try to insert some clearly invalid data, such as strings instead of integers, and click the Execute button, the UI won’t perform the call; instead, it shows the errors we need to correct (figure 6.2).
Figure 6.2 GET /BoardGames input form with invalid data

No request to the GET /BoardGame endpoint has been made (yet). The input errors were detected by the client-side validation techniques built by the SwaggerUI in the parameter info retrieved from the swagger.json file. All this happens automatically, without our having to code (almost) anything; we’re using the framework’s built-in features to their full extent.

Should we rely on client-side validation? It’s important to understand that the client-side validation features of SwaggerUI are useful only for improving user experience and preventing a useless round trip to the server. Those features serve no security purpose, because they can be easily bypassed by any user who has some minimal HTML and/or JavaScript knowledge.

The same can be said of all client-side validation controls, rules, and checks. They’re useful for enhancing our application’s presentation layer and blocking invalid requests without triggering their server-side counterparts, thus improving the overall performance of the client app, but they can’t ensure or protect the integrity of our data. So we can’t—and shouldn’t—rely on client-side validation. In this book, because we’re dealing with a web API, which represents the server-side companion of any client- server model we could imagine, we must always validate all the input data, regardless of what the client does.

Built-in validation attributes Most of the built-in validation attributes are natively supported by Swashbuckle, which automatically detects and documents them in the swagger.json file. If we look at our swagger.json file now, we’ll see that the [Range] attribute is documented in the following way:

{
    "name": "pageSize",
    "in": "query",
    "schema": {
      "maximum": 100,         ❶
        "minimum": 1,        ❷
        "type": "integer",
        "format": "int32",
        "default": 10
    }
}

❶ Range attribute min value
❷ Range attribute max value

Here’s how the [RegularExpression] attribute is documented:

{
    "name": "sortOrder",
    "in": "query",
    "schema": {
        "pattern": "ASC|DESC",   ❶
        "type": "string",
        "default": "ASC"
    }
}

❶ RegularExpression attribute’s RegEx pattern

Clients can also use this valuable information to implement additional client-side validation rules and functions.

Custom validation attributes Unfortunately, custom validators aren’t natively supported by Swashbuckle, which is hardly a surprise, because there’s no chance that Swashbuckle would know how they work. But the library exposes a convenient filter pipeline that hooks into the swagger.json file generation process. This feature allows us to create our own filters, add them to the pipeline, and use them to customize the file’s content.
NOTE The Swashbuckle’s filter pipeline is covered extensively in chapter 11. In this section, I provide only a small preview of this feature by introducing the IParameterFilter interface, because we need it to fulfill our current needs. For additional info about the interface, check out chapter 11 and/or the following URL: http://mng.bz/ydNe.

In a nutshell, here’s what we need to do if we want to add our custom validation attribute’s info to the swagger.json file:

1. Create a new filter class implementing the IParameterFilter interface for each of our custom validation attributes. Swashbuckle will call and execute this filter before creating the JSON block for all the parameters used by our controller’s action methods (and Minimal API methods).
2. Implement the IParameterFilter interface’s Apply method so that it detects all parameters decorated with our custom validation attribute and adds relevant info to the swagger.json file for each of them.

Let’s put this plan into practice, starting with the [SortOrderValidator] attribute.

Adding the SortOrderFilter In Visual Studio’s Solution Explorer panel, create a new /Swagger/ folder, right-click it, and add a new SortOrderFilter.cs class file. The new class must implement the IParameterFilter interface and its Apply method to add a suitable JSON key to the swagger.json file, like the built-in validation attributes. The following listing shows how we can do that.

**Listing 6.3 SortOrderFilter**

```csharp
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using MyBGList.Attributes;

namespace MyBGList.Swagger
{
    public class SortOrderFilter : IParameterFilter
    {
        public void Apply(
            OpenApiParameter parameter,
            ParameterFilterContext context)
        {
            var attributes = context.ParameterInfo?
                .GetCustomAttributes(true)
                   .OfType<SortOrderValidatorAttribute>();   ❶

              if (attributes != null)
              {
                   foreach (var attribute in attributes)     ❷
                   {
                       parameter.Schema.Extensions.Add(
                           "pattern",
                           new OpenApiString(string.Join("|",
                               attribute.AllowedValues.Select(v => $"^{v}$")))
                           );
                   }
              }
         }
     }
}
```

❶ Checks whether the parameter has the attribute
❷ If the attribute is present, acts accordingly

Notice that we’re using the "pattern" JSON key and a RegEx pattern as a value—the same behavior used by the [RegularExpression] built-in validation attribute. We did that to facilitate the implementation of the client-side validation check, assuming that the client will already be able to provide RegEx support when it receives the info (which happens to be “compatible” with our validation requirements). We could use a different key and/or value type, leaving the implementation details to the clients. Next, let’s create another filter for our second custom validation attribute.

Adding the SortColumnFilter Add a new SortColumnFilter.cs class file within the /Swagger/ folder. This class will be similar to the SortOrderFilter class, with some minor differences: this time, we’ll have to retrieve the names of the EntityType properties instead of the AllowedValues string array, which require some additional work. The following listing provides the source code.

**Listing 6.4 SortColumnFilter**

```csharp
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using MyBGList.Attributes;

namespace MyBGList.Swagger
{
    public class SortColumnFilter : IParameterFilter
    {
        public void Apply(
            OpenApiParameter parameter,
            ParameterFilterContext context)
        {
            var attributes = context.ParameterInfo?
                .GetCustomAttributes(true)
                .OfType<SortColumnValidatorAttribute>();   ❶
            if (attributes != null)
            {
                foreach (var attribute in attributes)      ❷
                {
                    var pattern = attribute.EntityType
                        .GetProperties()
                           .Select(p => p.Name);
                       parameter.Schema.Extensions.Add(
                           "pattern",
                           new OpenApiString(string.Join("|",
                               pattern.Select(v => $"^{v}$")))
                           );
                   }
              }
          }
      }
}
```

❶ Checks whether the parameter has the attribute
❷ If the attribute is present, acts accordingly

Again, we used the "pattern" key and a RegEx pattern for the value, because even this validator is compatible with a RegEx-based client-validation check. Now we need to hook these filters to the Swashbuckle’s middleware in the Program.cs file so that they’ll be taken into account when the Swagger file is generated.

Binding the IParameterFilters Open the Program.cs file, and add the namespace corresponding to our newly implemented filters at the top:

using MyBGList.Swagger;

Scroll down to the line where we add the Swashbuckle’s Swagger Generator middleware to the pipeline, and change it in the following way:

```csharp
builder.Services.AddSwaggerGen(options => {
      options.ParameterFilter<SortColumnFilter>();    ❶
      options.ParameterFilter<SortOrderFilter>();     ❷
});
❶ Adds the SortColumnFilter to the filter pipeline
❷ Adds the SortOrderFilter to the filter pipeline
```

Now we can test what we did by running our project in Debug mode and looking at the autogenerated swagger.json file, using the same URL as before (https://localhost:40443/swagger/v1/swagger.json). If we did everything correctly, we should see the sortOrder and sortColumn parameters with the "pattern" key present and filled in according to the validator’s rules:

{
     "name": "sortColumn",
     "in": "query",
     "schema": {
       "type": "string",
       "default": "Name",
         "pattern": "^Id$|^Name$|^Year$"       ❶
     }
},
{
     "name": "sortOrder",
     "in": "query",
     "schema": {
       "type": "string",
       "default": "ASC",
         "pattern": "^ASC$|^DESC$"             ❷
     }
}

❶ SortColumnValidator’s RegEx pattern
❷ SortOrderValidator’s RegEx pattern

It’s important to understand that implementing custom validators can be a challenge—and an expensive task in terms of time and source- code lines. In most scenarios, we won’t need to do that, because the built-in validation attributes accommodate all our needs. But being able to create and document them can make a difference whenever we deal with complex or potentially troublesome client-defined inputs.

### 6.1.5 Binding complex types

Up to this point, we’ve always worked with our action methods by using simple type parameters: integer, string, bool, and the like. This approach is a great way to learn how model binding and validation attributes work and is often the preferred approach whenever we deal with a small set of parameters. Several scenarios, however, can greatly benefit from using complex-type parameters such as DTOs, especially considering that the ASP.NET Core model binding system can handle them as well.

When the model binding’s target is a complex type, each type property is treated as a separate parameter to bind and validate. Each property of the complex type acts like a simple type parameter, with great benefits in terms of code extensibility and flexibility. Instead of having a potentially long list of method parameters, we can box all the parameters into a single DTO class. The best way to understand the advantages is to put them into practice by replacing our current simple type parameters with a single, comprehensive complex type.

Creating a RequestDTO class In Visual Studio’s Solution Explorer panel, right-click the /DTO/ folder, and add a new RequestDTO.cs class file. This class will contain all the client-defined input parameters that we’re receiving in the BoardGamesController’s Get action method; all we have to do is to create a property for each of them, as shown in the following listing.

**Listing 6.5 RequestDTO.cs file using MyBGList.Attributes; using System.ComponentModel; using System.ComponentModel.DataAnnotations;**

```csharp
namespace MyBGList.DTO
{
    public class RequestDTO
    {
          [DefaultValue(0)]                                   ❶
          public int PageIndex { get; set; } = 0;

          [DefaultValue(10)]                                  ❶
          [Range(1, 100)]                                     ❷
          public int PageSize { get; set; } = 10;

          [DefaultValue("Name")]                              ❶
          [SortColumnValidator(typeof(BoardGameDTO))]         ❸
          public string? SortColumn { get; set; } = "Name";

          [DefaultValue("ASC")]                               ❶
          [SortOrderValidator]                                ❸
          public string? SortOrder { get; set; } = "ASC";

          [DefaultValue(null)]                                ❶
          public string? FilterQuery { get; set; } = null;
     }
}
```

❶ Default value attributes
❷ Built-in validation attributes
❸ Custom validation attributes

Notice that we’ve decorated each property with a [DefaultValue] attribute. This attribute enables the Swagger generator middleware to create the "default" key in the swagger.json file, because it won’t be able to see the initial value that we set by using the convenient C# inline syntax. Luckily, this attribute is supported and provides a good workaround. Now that we have the RequestDTO class, we can use it to replace the simple type parameters of the BoardGamesController’s Get method in the following way:

```csharp
[HttpGet(Name = "GetBoardGames")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
public async Task<RestDTO<BoardGame[]>> Get(
    [FromQuery] RequestDTO input)               ❶
{
    var query = _context.BoardGames.AsQueryable();
    if (!string.IsNullOrEmpty(input.FilterQuery))
        query = query.Where(b => b.Name.Contains(input.FilterQuery));
    query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);
```

return new RestDTO<BoardGame[]>() { Data = await query.ToArrayAsync(), PageIndex = input.PageIndex, PageSize = input.PageSize, RecordCount = await _context.BoardGames.CountAsync(), Links = new List<LinkDTO> { new LinkDTO( Url.Action( null, "BoardGames", new { input.PageIndex, input.PageSize }, Request.Scheme)!, "self", "GET"), } }; }

❶ The new complex-type parameter

In this code, we use the [FromQuery] attribute to tell the routing middleware that we want to get the input values from the query string, thus preserving the former behavior. But we could have used any of the other available attributes:

[FromQuery]—To get values from the query string [FromRoute]—To get values from route data [FromForm]—To get values from posted form fields [FromBody]—To get values from the request body [FromHeader]—To get values from HTTP headers [FromServices]—To get values from an instance of a registered service [FromUri]—To get values from an external URI

Being able to switch among parameter binding techniques by using an attribute-based approach is another convenient feature of the framework. We’ll use some of these attributes later.

We had to use the [FromQuery] attribute explicitly because the default method for complex-type parameters is to get the values from the request body. We also had to replace all the parameters’ references within the source code with the properties of the new class. Now the “new” implementation looks more sleek and DRY (Don’t Repeat Yourself principle) than the preceding one. Moreover, we have a flexible, generic DTO class that we can use to implement similar GET-based action methods in the BoardGamesController, as well as in other controllers that we want to add in the future: DomainsController, MechanicsControllers, and so on. Right?

Well, no. If we take a better look at our current RequestDTO class, we see that it’s not generic at all. The problem lies in the [SortColumnValidator] attribute, which requires a type parameter. As we can see by looking at the source code, this parameter is hardcoded to the BoardGameDTO type:

[SortColumnValidator(typeof(BoardGameDTO))]

How can we work around the problem? On first thought, we might be tempted to pass that parameter dynamically, maybe using a generic <T> type. This approach would require changing the RequestDTO’s class declaration to

public class RequestDTO<T>

which would allow us to use it in our action methods in the following way:

[FromQuery] RequestDTO<BoardGameDTO> input

Then we’d change the validator this way:

[SortColumnValidator(typeof(T))]

Unfortunately, that approach wouldn’t work. In C#, attributes that decorate a class are evaluated at compile time, but a generic <T> class won’t receive its final type info until runtime. The reason is simple: because some attributes could affect the compilation process, the compiler must be able to define them in their entirety at compile time. As a result, attributes can’t use generic type parameters.

Generic attribute type limitations in C# According to Eric Lippert (a former Microsoft engineer and member of the C# language design team), this limitation was added to reduce complexity in both the language and compiler code for a use case that doesn’t add much value. His explanation (paraphrased) can be found in this StackOverflow answer given by Jon Skeet: http://mng.bz/Mlw8.

For additional info regarding this topic, check out the Microsoft guide to C# generics at http://mng.bz/Jl0K.

This behavior could change in the future, because the .NET community often asks the C# language design team to reevaluate it.

If attributes can’t use generic types, how can we work around the problem? The answer isn’t difficult to guess: if the mountain won’t go to Mohammed, Mohammed must go to the mountain. In other words, we need to replace the attribute approach with another validation technique that’s supported by the ASP.NET framework. Luckily, such a technique happens to exist, and it goes by the name IValidatableObject.

Implementing the IValidatableObject The IValidatableObject interface provides an alternative way to validate a class. It works like a class-level attribute, meaning that we can use it to validate any DTO type regardless of its properties and in addition to all property-level validation attributes it contains. The IValidatableObject interface has two major advantages over the validation attributes:

It isn’t required to be defined at compile time, so it can use generic types (which allows us to overcome our problem). It’s designed to validate the class in its entirety, so we can use it to check multiple properties at the same time and perform cross- validation and any other tasks that require a holistic approach.

Let’s use the IValidatableObject interface to implement the sort-column validation check in our current RequestDTO class. Here’s what we need to do:

1. Change the RequestDTO’s class declaration so that it can accept a generic <T> type.

2. Add the IValidatableObject interface to the RequestDTO type.
3. Implement the Validate method of the IValidatableObject interface so that it will fetch the generic <T> type’s properties and use their names to validate the SortColumn property.

The following listing shows how we can implement these steps.

**Listing 6.6 RequestDTO.cs file (version 2)**

```csharp
using MyBGList.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBGList.DTO
{
   public class RequestDTO<T> : IValidatableObject   ❶
   {
       [DefaultValue(0)]
       public int PageIndex { get; set; } = 0;

        [DefaultValue(10)]
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
         [DefaultValue("Name")]
         public string? SortColumn { get; set; } = "Name";

         [SortOrderValidator]
         [DefaultValue("ASC")]
         public string? SortOrder { get; set; } = "ASC";

         [DefaultValue(null)]
         public string? FilterQuery { get; set; } = null;

         public IEnumerable<ValidationResult> Validate(       ❷
             ValidationContext validationContext)
         {
             var validator = new SortColumnValidatorAttribute(typeof(T));
             var result = validator
                 .GetValidationResult(SortColumn, validationContext);
             return (result != null)
                 ? new [] { result }
                 : new ValidationResult[0];
         }
    }
}
```

❶ Generic Type and IValidatableObject interface
❷ Validate method implementation

In this code, we see that the Validate method implementation presents a plot twist: we’re using the SortColumnValidator under the hood! The main difference is that this time, we’re using it as a “standard” class instance rather than a data annotation attribute, which allows us to pass the generic type as a parameter.

It almost feels like cheating, right? But that’s not the case; we’re recycling what we’ve already done. We can do that thanks to the fact that the GetValidationResult method exposed by the ValidationAttribute base class is defined as public, which allows us to create an instance of the validator and call it to validate the SortColumn property. Now that we have a generic DTO class to use in our code, open the BoardGamesControllers.cs file, scroll down to the Get method, and update its signature in the following way:

public async Task<RestDTO<BoardGame[]>> Get( [FromQuery] RequestDTO<BoardGameDTO> input)

The rest of the method’s code doesn’t require any change. We’ve specified the BoardGameDTO as a generic-type parameter so that the RequestDTO’s Validate method will check its properties against the SortColumn input data, ensuring that the column set by the client to sort the data is valid for that specific request.

Adding DomainsController and MechanicsController Now is a good time to create a DomainsController and a MechanicsController, replicating all the features that we’ve implemented in the BoardGamesController so far. Doing this will allow us to make a proper test of our generic RequestDTO class and our IValidatableObject flexible implementation. We also need to add a couple of new DTOs, DomainDTO class and MechanicDTO, which will be along the lines of the BoardGameDTO class.

For reasons of space, I’m not listing the source code for these four files here. That code is available in the /Chapter_06/ folder of this book’s GitHub repository, in the /Controllers/ and /DTO/ subfolders. I strongly suggest that you try to implement them without looking at the GitHub files, because this is a great chance to practice everything you’ve learned so far.

Testing the new controllers When the new controllers are ready, we can check them thoroughly by using the following URL endpoints (for the GET method),

https://localhost:40443/Domains/ https://localhost:40443/Mechanics/

as well as the SwaggerUI (for the POST and DELETE methods).

NOTE Whenever we delete a domain or a mechanic, the cascading rules that we set for these entities in chapter 4 also remove all its references to the corresponding many-to-many lookup table. All board games will lose their relationship with that specific domain or mechanic (if they had it). To recover, we need to delete all the board games and then reload them by using the SeedController’s Put method.

Updating the IParameterFilters Before we go further, we need to do one last thing. Now that we’ve replaced our simple type parameters with a DTO, the SortOrderFilter and SortColumnFilter won’t be able to locate our custom validators anymore. The reason is simple: their current implementation is looking for them by using the GetCustomAttributes method of the context.ParameterInfo object, which returns an array of the attributes applied to the parameter handled by the filter. Now this ParameterInfo contains a reference of the DTO itself, meaning that the preceding method will return the attributes applied to the whole DTO class—not to its properties.

To fix the problem, we need to extend the attribute lookup behavior so that it also checks the attributes assigned to the given parameter’s properties (if any). Here’s how we can update the SortOrderFilter’s source code to do that:

var attributes = context.ParameterInfo
    .GetCustomAttributes(true)
    .Union(                                                   ❶
        context.ParameterInfo.ParameterType.GetProperties()
        .Where(p => p.Name == parameter.Name)
        .SelectMany(p => p.GetCustomAttributes(true))
    )
    .OfType<SortOrderValidatorAttribute>();

❶ Retrieves the parameter’s properties custom attributes

Notice that we used a Union LINQ extension method to produce a single array containing the custom attributes assigned to the ParameterInfo object itself, as well as those assigned to a property of that ParameterInfo object with the name of the parameter that the filter is currently processing (if any). Thanks to this new implementation, our filters will be able to find the custom attributes assigned to any complex-type parameter’s property as well as to simple type parameters, ensuring full backward compatibility.

The SortOrderFilter has been fixed, but what about the SortColumnFilter? Unfortunately, the fix isn’t so simple. The [SortColumnValidator] attribute isn’t applied to any property, so there’s no way that the SortColumnFilter can find it. We might think that we could work around the problem by adding the attribute to the IValidatableObject’s Validate method and then tweaking the filter’s lookup behavior to include methods other than properties. But we already know that this workaround would fail; the attribute would still require a generic type parameter that can’t be set at compile time. For reasons of space, we won’t fix this problem now; we’ll postpone this task until chapter 11, when we’ll learn other API documentation techniques involving Swagger and Swashbuckle.

TIP Before proceeding, be sure to apply the preceding patch to the SortColumnFilter as well. The source code to add is identical, because both filters use the same lookup strategy. This patch may seem to be useless, because the SortColumnFilter isn’t working (and won’t work for a while), but keeping our classes up to date is good practice, even if we’re not actively using or counting on them.

Our data validation journey is over, at least for now. In the next section, we’ll learn how to handle validation errors and program exceptions.

## 6.2 Error handling

Now that we’ve implemented several server-side data validation checks, we’ve created a lot of additional failing scenarios for our web API, in addition to those normally provided in case of blatantly invalid HTTP requests. Each piece of missing, malformed, incorrect, or otherwise invalid input data, according to and limited by our validation rules that determine a model-binding failure, will be rejected by our web API with an HTTP 400 - Bad Request error response. We experienced this behavior at the start of this chapter when we tried to pass a string value to the pageIndex parameter instead of a numeric 1. The HTTP 400 status wasn’t the only response from the server, however. We also got an interesting response body, which deserves another look:

{ "type":"https://tools.ietf.org/html/rfc7231#section-6.5.1", "title":"One or more validation errors occurred.", "status":400, "traceId":"00-a074ebace7131af6561251496331fc65-ef1c633577161417-00", "errors":{ "pageIndex":["The value 'string' is not valid."] } }

As we can see, our web API doesn’t only tell the client that something went wrong; it also provides contextual information about the error, including the parameter with the rejected value, using the HTTP API response format standard defined at https://tools.ietf.org/html/rfc7807, which appeared briefly in chapter 2. All this work is performed automatically by the framework under the hood; we don’t have to do anything. This work is a built-in functionality of the [ApiController] attribute that decorates our controllers.

### 6.2.1 The ModelState object

To understand what the [ApiController] attribute is doing for us, we need to take a step back and review the whole model binding and validation system lifecycle. Figure 6.3 illustrates the flow of the various steps performed by the framework within a typical HTTP request.
Figure 6.3 Model binding and validation lifecycle with the [ApiController] attribute

Our point of interest starts right after the HTTP request arrives, and the routing middleware invokes the model binding system, which performs two relevant tasks in sequence: Binds the input values to the action method’s simple type and/or complex-type parameters. If the binding process fails, an HTTP error 400 response is returned immediately; otherwise, the request goes to the next phase. Validates the model by using the built-in validation attributes, the custom validation attributes, and/or the IValidatableObject. The results of all the validation checks are recorded in the ModelState object, which eventually becomes valid (no validation errors occurred) or invalid (one or more validation errors occurred). If the ModelState object ends up being valid, the request is handled by the action method; otherwise, an HTTP error 400 response is returned.

The important lesson is that both the binding errors and the validation errors are handled by the framework (using an HTTP 400 error response) without even calling the action method. In other words, the [ApiController] attribute provides a fully automated error- handling management system. This approach can be great if we don’t have specific requirements, but what if we want to customize something? In the following sections, we’ll see how to do that.

### 6.2.2 Custom error messages

The first thing we may want to do is define some custom error messages instead of the default ones. Let’s start with the model binding errors.

Customizing the model binding errors To change the default model binding error messages, we need to modify the settings of the ModelBindingMessageProvider, which can be accessed from the ControllersMiddleware’s configuration options. Open the Program.cs file, locate the builder .Services.AddControllers method, and replace the current parameterless implementation in the following way (new lines in bold):

```csharp
builder.Services.AddControllers(options => {
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        (x) => $"The value '{x}' is invalid.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        (x) => $"The field {x} must be a number.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (x, y) => $"The value '{x}' is not valid for {y}.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
        () => $"A value is required.");
});
```

For simplicity, this sample changes only three of the many available messages.

Customizing the model validation errors Changing the model validation error messages is easy because the ValidationAttribute base class comes with a convenient ErrorMessage property that can be used for that purpose. We used it when we implemented our own custom validators. The same technique can be used for all built-in validators:

[Required(ErrorMessage = "This value is required.")] [Range(1, 100, ErrorMessage = "The value must be between 1 and 100.")] By doing that, however, we’re customizing the error messages, not the ModelState validation process itself, which is still performed automatically by the framework.

### 6.2.3 Manual model validation

Suppose that we want (or are asked) to replace our current HTTP 400 - Bad Request with a different status code in case of some specific validation failures, such as an HTTP 501 - Not Implemented status code for an incorrect pageSize integer value (lower than 1 or bigger than 100). This change request can’t be handled unless we find a way to check the ModelState manually (and act accordingly) within the action method. But we know that we can’t do that, because the ModelState validation and error handling process is handled by the framework automatically thanks to the [ApiController] functionalities. In case of an invalid ModelState, the action method won’t even come into play; the default (and unwanted) HTTP 400 error will be returned instead.

The first solution that might come to mind would be to get rid of the [ApiController] attribute, which would remove the automatic behavior and allow us to check the ModelState manually, even when it’s invalid. Would that approach work? It would. Figure 6.4 shows how the model binding and validation lifecycle diagram work without the [ApiController] attribute.
Figure 6.4 Model binding and validation lifecycle without the [ApiController] attribute

As we can see, now the action method will be executed regardless of the ModelState status, allowing us to inspect it, see what went wrong, and act accordingly, which is precisely what we want. But we shouldn’t commit to such a harsh workaround, because the [ApiController] attribute gives our controller several other functionalities that we may want to preserve. Instead, we should disable the automatic ModelState validation feature, which we can do by tweaking the default configuration settings of the [ApiController] attribute itself.

Configuring the ApiController’s behavior Open the Program.cs file, and locate the line where we instantiate the app local variable:

var app = builder.Build();

Place the following lines of code right above it:

```csharp
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);
```

var app = builder.Build();

This setting suppresses the filter that automatically returns a BadRequestObjectResult when the ModelState is invalid. Now we can achieve what we want for all our controllers without removing the [ApiController] attribute, and we’re ready to implement our change request by conditionally returning an HTTP 501 status code.

Implementing a custom HTTP status code For simplicity, suppose that the change request affects only the DomainsController. Open the /Controllers/DomainsController.cs file, and scroll down to the Get action method. Here’s what we need to do:

1. Check the ModelState status (valid or invalid).

2. If the ModelState is valid, preserve the existing behavior.
3. If the ModelState isn’t valid, check whether the error is related to the pageSize parameter. If that’s the case, return an HTTP 501 status code; otherwise, stick to the HTTP 400.

And here’s how we can pull it off:

```csharp
[HttpGet(Name = "GetDomains")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
public async Task<ActionResult<RestDTO<Domain[]>>> Get(                   ❶
    [FromQuery] RequestDTO<DomainDTO> input)
{
     if (!ModelState.IsValid)                                             ❷
     {
         var details = new ValidationProblemDetails(ModelState);
         details.Extensions["traceId"] =
              System.Diagnostics.Activity.Current?.Id
                 ?? HttpContext.TraceIdentifier;
         if (ModelState.Keys.Any(k => k == "PageSize"))
         {
              details.Type =
                   "https://tools.ietf.org/html/rfc7231#section-6.6.2";
              details.Status = StatusCodes.Status501NotImplemented;
              return new ObjectResult(details) {
                   StatusCode = StatusCodes.Status501NotImplemented
              };
         }
         else
         {
              details.Type =
                   "https://tools.ietf.org/html/rfc7231#section-6.5.1";
              details.Status = StatusCodes.Status400BadRequest;
              return new BadRequestObjectResult(details);
         }
     }
```

     // ... code omitted ...                                              ❸
}
❶ New return value (ActionResult<T>)
❷ Steps to perform if ModelState is invalid
❸ Code omitted for reasons of space (unchanged)

We can easily check the ModelState status by using the IsValid property, which returns true or false. If we determine that the ModelState isn’t valid, we check for the presence of the "PageSize" key in the error collection and create a UnprocessableEntity or a BadRequest result to return to the client. The implementation requires several lines of code because we want to build a rich request body documenting the error details, including the reference to the RFC documenting the error status code, the traceId, and so on.

This approach forced us to change the action method’s return type from Task<RestDTO<Domain[]>> to Task<ActionResult<RestDTO<Domain[]>>>, because now we need to handle two different types of responses: an ObjectResult if the ModelState validation fails and a JSON object in case it succeeds. The ActionResult is a good choice because thanks to its generic-type support, it can handle both types.

Now we can test the new behavior of the DomainsController’s Get action method. This URL should return an HTTP 501 status code: https://localhost:40443/Domains?pageSize=101. This one should respond with an HTTP 400 status code: https://localhost:40443/Domains?sortOrder=InvalidValue.

Because we also need to check the HTTP status code, not the response body alone, be sure to open the browser’s Network tab (accessible through Developer Tools in all Chrome-based browsers) before executing the URL—a quick, effective way to see the status code of each HTTP response in real time.

An unexpected regression bug All is well so far—except for a nontrivial regression error that we unintentionally caused in all our controllers! To check out what I’m talking about, try to execute the two “invalid” URLs in the preceding section against the BoardGamesController’s Get method:

https://localhost:40443/BoardGames?pageSize=101 https://localhost:40443/BoardGames?sortOrder=invalidValue

The first URL returns 101 board games, and the second throws an unhandled exception due to a syntax error in dynamic LINQ. What happened to our validators?

The answer should be obvious: they still work, but because we disabled the [ApiController]’s automatic ModelState validation feature (and HTTP 400 response), all of our action methods are executed even if some of their input parameters are invalid, with no manual validation to fill the gap except the DomainsController’s Get action method! Our BoardGamesController and MechanicsController, as well as all DomainsController action methods other than Get, are no longer protected from insecure input. Don’t panic, though; we can fix the problem. Again, we may be tempted to remove the [ApiController] attribute from the DomainsController and work around our regression bug with no further hassle. Unfortunately, this approach won’t do the trick; it will prevent the bug from affecting the other controllers but won’t solve the problem for the DomainsController’s other action methods. Also, we’d lose the other useful features of [ApiController], which was why we didn’t get rid of it to begin with.

Think about what we did: disabled a feature of the [ApiController] for the entire web application because we didn’t want it to trigger for a single controller’s action method. That was the mistake. The idea was good; we need to refine the scope.

Implementing an IActionModelConvention filter We can obtain what we want by using the convenient ASP.NET Core filter pipeline, which allows us to customize the behavior of the HTTP request/response lifecycle. We’ll create a filter attribute that checks for the presence of the ModelStateInvalidFilter within a given action method and removes it. This setting will have the same effect as the configuration setting that we placed in the Program.cs file, but it will happen only for the action method that we’ll choose to decorate with that filter attribute. In other words, we’ll be able to disable the ModelState autovalidation feature conditionally (opt-out, default-in) instead of having to shut it down for everyone (default-out).

Let’s put that theory into practice. Create a new ManualValidationFilterAttribute.cs class file in the /Attributes/ folder, and fill it with the source code in the following listing.

**Listing 6.7 ManualValidationFilterAttribute**

```csharp
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MyBGList.Attributes
{
    public class ManualValidationFilterAttribute
        : Attribute, IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            for (var i = 0; i < action.Filters.Count; i++)
            {
                if (action.Filters[i] is ModelStateInvalidFilter
                    || action.Filters[i].GetType().Name ==
                        "ModelStateInvalidFilterFactory")
                {
                    action.Filters.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
```

Sadly, the ModelStateInvalidFilterFactory type is
marked as internal, which prevents us from checking for the filter
presence by using a strongly typed approach. We must compare the
Name property with the literal name of the class. This approach is
hardly ideal and might cease to work if the name changes in future
releases of the framework, but for now, it will do the trick. Now that
we have the filter, we need to apply it to our
DomainsController’s Get action method like any other
attribute:
[HttpGet(Name = "GetDomains")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
[ManualValidationFilter]                                               ❶
public async Task<ActionResult<RestDTO<Domain[]>>> Get(
    [FromQuery] RequestDTO<DomainDTO> input)

❶ The new ManualValidationFilter attribute

Now we can delete (or comment out) the application-wide settings that caused the regression bug in the Program.cs file:

```csharp
// Code replaced by the [ManualValidationFilter] attribute
// builder.Services.Configure<ApiBehaviorOptions>(options =>
//    options.SuppressModelStateInvalidFilter = true);
```

We’ve reenabled the automatic ModelState validation feature for all our controllers and methods, leaving in manual state only the single action method for which we’ve implemented a suitable fallback. We’ve found a way to fulfill our change request while fixing our unexpected bug—and without giving up anything. Furthermore, everything we did here helped us to gain experience and raised our awareness of the ASP.NET Core request/response pipeline, as well as the underlying model binding and validation mechanism. Our manual ModelState validation overview is over, at least for now.

### 6.2.4 Exception handling

The ModelState object isn’t the only source of application errors that we might want to handle. Most application errors we’ll experience with our web API will be due not to client-defined input data, but to unexpected behavior of our source code: null-reference exceptions, DBMS connection failures, data-retrieval errors, stack overflows, and so on. All these problems will likely raise exceptions, which (as we’ve known since chapter 2) will be caught and handled by the DeveloperExceptionPageMiddleware (if the corresponding application setting is true) and the ExceptionHandlingMiddleware (if the setting is false).

In chapter 2, when we implemented the UseDeveloperExceptionPage application setting, we set it to false in the generic appsettings.json file and to true in the appsettings.Development.json file. We used this approach to ensure that the DeveloperExceptionPageMiddleware would be used only when the app is executed in a development environment. This behavior is clearly visible in the code section of the Program.cs file where we add the ExceptionHandlingMiddleware to the pipeline:

```csharp
if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
     app.UseDeveloperExceptionPage();
else
     app.UseExceptionHandler("/error");
```

Let’s disable this development override temporarily so that we can focus on how our web API is handling exceptions when dealing with actual clients (in other words, in production). Open the appSettings.Development.json file, and change the value of the UseDeveloperExceptionPage setting from true to false:

"UseDeveloperExceptionPage": false Now our application will adopt the production error handling behavior even in development, allowing us to check what we’re doing while updating it. In chapter 2, we set the ExceptionHandlingMiddleware’s error handling path to the "/error" endpoint, which we implemented by using the following Minimal API MapGet method in the Program.cs file:

```csharp
app.MapGet("/error",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
    Results.Problem());
```

Our current implementation consists of a single line of code that returns a ProblemDetails object, which results in an RFC 7807- compliant JSON response. We tested this behavior in chapter 2 by implementing and executing the /error/test endpoint, which throws an exception. Let’s execute it again to take another look at it:

{ "type":"https://tools.ietf.org/html/rfc7231#section-6.6.1", "title":"An error occurred while processing your request.", "status":500 }

This simple yet effective response clearly shows that we’ve set up a decent exception-handling strategy for our production environment. Every time something goes wrong, or when we want to raise an exception within our code manually, we can be sure that the calling client will receive an HTTP 500 error status code together with a standard (and RFC 7807-compliant) response body.

At the same time, we can see that the overall outcome isn’t informative. We’re telling the client only that something went wrong by returning an HTTP 500 status code and a minimal response body that explains the error in human-readable form.

We’re facing the same scenario that we experienced with the [ApiController]’s ModelState validation, an automatic behavior that might be convenient for most scenarios but could be limiting if we need to customize it further. We may want to return different status codes, depending on the exception being thrown. Or we may want to log the error somewhere and/or send an email notification to someone (depending on the exception type and/or context).

Fortunately, the ExceptionHandlingMiddleware can be configured to do all those things, and many more, with relatively few lines of code. In the following sections, we’ll take a better look at the ExceptionHandlingMiddleware (we only scratched its surface in chapter 2, after all) and see how we can use it to its full extent.

Working with the ExceptionHandlingMiddleware The first thing we can do to customize the current behavior is to provide the ProblemDetails object with some additional details regarding the exception, such as its Message property value. To do that, we need to retrieve two objects:

The current HttpContext, which can be added as a parameter in all Minimal API methods An IExceptionHandlerPathFeature interface instance, which allows us to access the originating exception in a convenient handler

Here’s how we can do that (relevant code in bold):

```csharp
app.MapGet("/error",
    [EnableCors("AnyOrigin")]
      [ResponseCache(NoStore = true)] (HttpContext context) =>           ❶
      {
          var exceptionHandler =
                context.Features.Get<IExceptionHandlerPathFeature>();    ❷
```

            // TODO: logging, sending notifications, and more            ❸

```csharp
            var details = new ProblemDetails();
            details.Detail = exceptionHandler?.Error.Message;            ❹
            details.Extensions["traceId"] =
                System.Diagnostics.Activity.Current?.Id
                  ?? context.TraceIdentifier;
            details.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            details.Status = StatusCodes.Status500InternalServerError;
            return Results.Problem(details);
      });
```

❶ Adds the HttpContext
❷ Retrieves the Exception handler
❸ Performs other error-related management tasks
❹ Sets the Exception message

After we perform this upgrade, we can launch our app and execute the /error/test endpoint to get a much more detailed response body:

{
    "type":"https://tools.ietf.org/html/rfc7231#section-6.6.1",
    "title":"An error occurred while processing your request.",
    "status":500,
    "detail":"test",                                                    ❶
    "traceId":"00-7cfd2605a885fbaed6a2abf0bc59944e-28bf94ef8a8c80a7-00" ❶
}
❶ New JSON data

Notice that we’re manually instantiating a ProblemDetails object instance, configuring it for our needs, and then passing it to the Results.Problem method overload, which accepts it as a parameter. We could do much more than simply configure the ProblemDetails object’s properties, however. We could also do the following:

Return different HTTP status codes depending on the exception’s type, as we did with the ModelState manual validation in the DomainsController’s Get method

Log the exception somewhere, such as in our DBMS Send email notifications to administrators, auditors, and/or other parties

Some of these possibilities are covered in the upcoming chapters.

WARNING It’s important to understand that the exception-handling middleware will reexecute the request using the original HTTP method. The handler endpoint—in our scenario, the MapGet method handling the /error/ path—shouldn’t be restricted to a limited set of HTTP methods, because it would work only for them. If we want to handle exceptions differently based on the original HTTP method, we can apply different HTTP verb attributes to multiple actions with the same name. We could use [HttpGet] to handle GET exceptions only and [HttpPost] to handle POST exceptions only, for example.

Exception handling action Instead of delegating the exception handling process to a custom endpoint, we can use the UseExceptionHandler method’s overload, which accepts an Action<IApplicationBuilder> object instance as a parameter. This approach allows us to obtain the same level of customization without specifying a dedicated endpoint. Here’s how we can use the implementation that we currently have within our Minimal API’s MapGet method using that overload:

```csharp
app.UseExceptionHandler(action => {
    action.Run(async context =>
    {
        var exceptionHandler =
            context.Features.Get<IExceptionHandlerPathFeature>();
```

```csharp
            var details = new ProblemDetails();
            details.Detail = exceptionHandler?.Error.Message;
            details.Extensions["traceId"] =
                System.Diagnostics.Activity.Current?.Id
                  ?? context.TraceIdentifier;
            details.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            details.Status = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(
                System.Text.Json.JsonSerializer.Serialize(details));     ❶
      });
});
```

❶ JSON-serializing the ProblemDetails object

As we can see, the source code is almost identical to the MapGet method’s implementation. The only real difference is that here, we need to write the response body directly into the HTTP response buffer; we must take care of serializing the ProblemDetails object instance to an actual JSON-formatted string. Now that we’ve had some practice with the various error handling approaches offered by the framework, we’re ready to apply this knowledge to the topic of chapter 7: application logging.

## 6.3 Exercises

It’s time to challenge ourselves with the usual list of hypothetical task assignments given by our product owner. The solutions to the exercises are available on GitHub in the /Chapter_06/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 6.3.1 Built-in validators

Add a built-in validator to the Name property of the DomainDTO object so that it will be considered valid only if it’s not null, not empty, and contains only uppercase and lowercase letters (without digits, spaces, or any other characters). Examples of valid values are "Strategy", "Family", and "Abstract". Examples of invalid values are "Strategy Games", "Children's", "101Guides", "", and null.

In case of an invalid value, the validator should emit the error message "Value must contain only letters (no spaces, digits, or other chars)". The DomainsController’s Post method, which accepts a DomainDTO complex type as a parameter, can be used to test the HTTP response containing the validation outcome.

### 6.3.2 Custom validators

Create a [LettersOnly] validator attribute, and implement it to fulfill the same specifications given in section 6.3.1, including the error message. The actual value check should be performed using either regular expressions or string manipulation techniques, depending on whether the custom UseRegex parameter is set to true or false (default). When the custom validator attribute is ready, apply it to the Name property of the MechanicDTO object, and test it with both the UseRegex parameter values available by using the MechanicsController’s Post method.

### 6.3.3 IValidatableObject

Implement the IValidatableObject interface to the DomainDTO object, and use its Valid method to consider the model valid only if the Id value is equal to 3 or if the Name value is equal to "Wargames". If the model is invalid, the validator should emit the error message "Id and/or Name values must match an allowed Domain." The DomainsController’s Post method, which accepts a DomainDTO complex type as a parameter, can be used to test the HTTP response containing the validation outcome.

### 6.3.4 ModelState validation

Apply the [ManualValidatonFilter] attribute to the DomainsController’s Post method to disable the automatic ModelState validation performed by the [ApiController]. Then implement a manual ModelState validation to return the following HTTP status codes conditionally whenever ModelState isn’t valid:

HTTP 403 - Forbidden—If the ModelState is invalid because the Id value isn’t equal to 3 and the Name value isn’t equal to "Wargames" HTTP 400 - Bad Request—If the ModelState is invalid for any other reason

If the ModelState is valid, the HTTP request must be processed as normal.

### 6.3.5 Exception handling

Modify the current /error endpoint behavior to return the following HTTP status code conditionally, depending on the type of exception being thrown:

HTTP 501 - Not Implemented—For the NotImplementedException type HTTP 504 - Gateway Timeout—For the TimeoutException type HTTP 500 - Internal Server Error—For any other exception type

To test the new error handling implementation, create two new MapGet methods, using Minimal API and implement them so that they throw an exception of the corresponding type: /error/test/501 for the HTTP 501 - Not Implemented status code /error/test/504 for the HTTP 504 - Gateway Timeout status code

Summary Data validation and error handling allow us to handle most unexpected scenarios during the interaction between client and server, reducing the risk of data leaks, slowdowns, and other security and performance problems. The ASP.NET Core model binding system is responsible for handling all the input data coming from HTTP requests, including converting them to .NET types (binding) and checking them against our data validation rules (validating). We can assign data validation rules to input parameters and complex-type properties by using built-in or custom data annotation attributes. Furthermore, we can create cross- validation checks in complex types by using the IValidatableObject interface. The ModelState object contains the combined result of the data-validation checks performed against the input parameters. ASP.NET Core allows us to use it in two ways: Processing it automatically (thanks to the [ApiController]’s autovalidation features). Checking its values manually, which allows us to customize the whole validation process and the resulting HTTP response. Application-level errors and exceptions can be handled by using the ExceptionHandlingMiddleware. This middleware can be configured to customize the error handling experience according to our needs, such as Returning different HTTP status codes and/or human- readable info depending on the exception’s type. Logging the exceptions somewhere (DBMS, text file, event registry, and so on). Sending email notifications to the interested parties (such as system administrators).
