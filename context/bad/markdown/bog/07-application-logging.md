---
title: Application logging
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 7
---

# 7. Application logging

**This chapter covers**

- Application logging origin and purposes
- Logging techniques using the ILogger interface
- Differences between unstructured and structured logging
- Implementing an alternative logging framework with Serilog

The term logging, when used in an IT context, defines the process of keeping track of all the events occurring within the application (and their context information) in a structured, semistructured, and/or unstructured format, and outputting them to a dedicated view and/or storage channel. Such a channel is often called secondary to distinguish it from the primary output mechanism the software uses to communicate with the end user: the user interface (UI).

The primary purpose of logging is to keep track of the various interactions between the software and its users: state changes, access to internal resources, event handlers that trigger in response to user actions, exceptions thrown by internal modules, and so on. Because this activity monitoring task is performed while the application is running, each log entry is typically shown (or recorded) with a timestamp value representing the moment when the logged event occurred.

In this chapter, after a brief overview of the concept and importance of logging, we’ll learn how to create a structured logging mechanism for our web API project by using the ILogger interface, as well as some third-party components that implement it.

## 7.1 Application logging overview

In computer science, to log refers to the sequential and chronological recording of the operations carried out by the system (as they are performed). These operations can be carried out by a plurality of actors: users, system administrators, automated scheduled tasks, and tasks originated by the system itself.

By extension, the term log also refers to the file (or set of files) in which these records are stored. When someone asks us to “check out the logs,” we know that logs means the log files. Why do we use this verb, though? Where does it come from?

### 7.1.1 From boats to computers

When electronics didn’t exist, the term log was used only to refer to a piece of wood. But its use as a synonym for recording didn’t start with computers or information technology; it originated around the 18th century. In the nautical jargon of the time, log referred to a piece of wood attached to a rope that was left to float off the ship. The rope had a series of knots that the sailors would count to measure the speed of the ship at any moment; the speed was determined by the number of knots that were above the water. This rudimentary yet effective measuring instrument is the reason why the speed of boats is still expressed in knots today. This technique was far from perfect, however. Sailors knew that the speed of the boat could have been influenced by several internal and external factors, such as sail or engine performance, atmospheric conditions, and wind strength and direction. For this reason, the log- based knot measurement task known as logging was repeated at regular intervals. Each logging activity was recorded in a special register along with other relevant information, such as time, wind, and weather—the context. This register, commonly called a logbook, was the first example of a proper log file. As we can see, this unique measurement technique provided not only the name, but also the foundation for the modern concept of logging.

The adoption of the logbook concept in computer jargon took place at the beginning of the 1960s and led to the introduction of related definitions, including login and logout. From that moment on, the importance of logging in the IT industry increased exponentially, leading to the birth of a new market sector (log management) that was worth more than $1.2 billion in 2022 and grows year after year. This expansion was clearly determined by the growing importance of the IT security, compliance, audit, monitor, business intelligence, and data analysis frameworks, which strongly rely on system and application logs.

### 7.1.2 Why do we need logs?

We may wonder what a logging system can do for our web API or why we should care about implementing it. The best way to address this concern is to enumerate the most important aspects of an application that can realistically benefit from a logging system: Stability—Log records allow us to detect and investigate bugs and/or unhandled exceptions promptly, facilitating the process of fixing them and minimizing the application’s downtime. Security—The presence of a logging system helps us determine whether the system has been compromised and, if so, to what extent. The log records also allow us to identify the vulnerabilities found by the attacker and the malicious actions the attacker was able to perform by exploiting those vulnerabilities. Business continuity—The logging system can make system administrators aware of abnormal behaviors before they become critical and/or can inform them promptly about crashes via alarms, email messages, and other real-time notification-based alert processes. Compliance requirements—Most international IT security regulations, standards, and guidelines require precise logging policies. This approach has been further strengthened in recent years by the introduction of the General Data Protection Regulation (GDPR) on the territory of the European Union and other data protection regulations elsewhere.

It’s important to understand that the act of “reading” the logs (and acting accordingly) doesn’t have to be performed by human beings. A good logging practice always requires both manual checks and a monitoring system that can be configured to apply some preset remediations automatically in case of common incidents (restarting defective or nonfunctioning services, launching maintenance scripts, and so on). Most modern IT security software provides a highly automatable security operation center (SOC) framework and/or a security, orchestration, automation, and response (SOAR) integrated solution. All that considered, we can easily see that log management is not only a useful tool for keeping web (and nonweb) applications under control, but also an essential requirement to guarantee their overall reliability as well as a fundamental asset in IT security.

## 7.2 ASP.NET logging

Now that we’ve acknowledged the importance of logging, let’s use the tools of the .NET framework to manage this important aspect of our application. These tools are the standardized, general-purpose logging APIs made available through the ILogger interface, which allows us to record the events and activities we want to log through a series of built-in and/or third-party logging providers.

Technically speaking, the ILogger API isn’t part of .NET; it’s located in an external Microsoft.Extensions.Logging NuGet package published and maintained by Microsoft. This package, however, is implicitly included in all ASP.NET Core applications, including the ASP.NET Core web API project template we’ve used to create our MyBGList app. We can easily confirm that fact by looking at the BoardGamesController’s source code.

Open the Controllers/BoardGamesController.cs file, and search for a reference to the ILogger interface. We should find it among the private properties of the controller’s class:

private readonly ILogger<BoardGamesController> _logger;

As we can see, the interface accepts a generic type (TCategoryName). The name of the type will be used to determine the log category, a string value that will help us categorize the log entries coming from the various classes.

If we scroll down a bit farther, we see that the ILogger<BoardGamesController> object instance is obtained within the class constructor by the standard dependency injection (DI) pattern, which we should be used to by now:

```csharp
public BoardGamesController(
    ApplicationDbContext context,
    ILogger<BoardGamesController> logger)
    {
        _context = context;
        _logger = logger;
}
```

Because all our controllers have been derived from this codebase, that same _logger private property (and injection pattern) should be present in our DomainsController and MechanicsController as well. We can already use the ILogger API. To demonstrate, let’s take a couple of minutes to perform a simple test.

### 7.2.1 A quick logging test

Keeping the BoardGamesController.cs file open, scroll down to the Get action method’s implementation, and add the following single line of code (in bold):

```csharp
public async Task<RestDTO<BoardGame[]>> Get(
    [FromQuery] RequestDTO<BoardGameDTO> input)
    {
        _logger.LogInformation("Get method started.");   ❶
```

// ... rest of code omitted
❶ Our first logging attempt

If we look at the suggestions given by Visual Studio’s IntelliSense feature while writing this code, we notice that the API provides several logging methods: LogTrace, LogDebug, LogInformation (the one we used), LogWarning, LogError, and LogCritical, all of which correspond to the various log levels available. These methods are shortcuts for the generic Log method, which allows us to specify a LogLevel value explicitly. We can use this value to set the severity level of each log entry, choosing among several options defined by the LogLevel enumeration class.

Launch the project in Debug mode; open the Visual Studio Output window by choosing View > Output; and execute the BoardGamesController’s Get method via either SwaggerUI or https://localhost:40443/BoardGames. If everything goes as planned, immediately before receiving the HTTP response, we should see the following log entry in the Output window (along with several other lines):

MyBGList.Controllers.BoardGamesController: Information: Get method started.

That’s the result of our first logging attempt. As we can see, the log entry contains the category (the controller’s fully qualified class name), the chosen LogLevel (Information), and the log message that we specified when implementing it.

If we look at the other lines in the Output window, we can see a couple of other logs related to EntityFrameworkCore. We shouldn’t be surprised, because we’re using the same logging API that all other .NET and ASP.NET Core middleware and services use, along with any third-party package that has adopted it. That fact is great to know, because every configuration tweak we apply to the logging behavior of our application will affect not only our custom log entries, but also the logging capabilities provided by our components. Now that we’ve experienced how the ILogger API works in practice, let’s take a couple of steps back to review its core concepts, starting with the LogLevel enumeration class.

### 7.2.2 Log levels

The LogLevel enum type is part of the Microsoft.Extensions.Logging namespace and defines the following available levels, in order of severity:

Trace (0)—Information related to the application’s internal activities and useful only for low-level debugging or system administration tasks. This log level is never used, because it can easily contain confidential data (such as configuration info, use of encryption keys, and other sensitive information that shouldn’t be viewed or recorded by anyone). Debug (1)—Development-related information (variable values, stack trace, execution context, and so on) that’s useful for interactive analysis and debugging purposes. This log level should always be disabled in production environments, as the records might contain information that shouldn’t be disclosed. Information (2)—Informative messages that describe events or activities related to the system’s normal behavior. This log level usually doesn’t contain sensitive or nondisclosable information, but it’s typically disabled in production to prevent excessive logging verboseness, which could result in big log files (or tables). Warning (3)—Information about abnormal or unexpected behaviors, as well as any other event or activity that doesn’t alter the normal flow of the app. Error (4)—Informative messages about noncritical events or activities that have likely halted, interrupted, or otherwise hindered the standard execution flow of a specific task or activity. Critical (5)—Informative messages about critical events or activities that prevent the application from starting or that determine an irreversible and/or unrecoverable application crash. None (6)—No information is logged. Typically, this level is used to disable logging.

### 7.2.3 Logging configuration

We stumbled upon these LogLevel values in chapter 2, when we looked at our appSettings files for the first time. Now it’s time to explain their meaning. Open the appsettings.json file, and check out the root-level "Logging" key, shown in the following code snippet (nonrelevant parts omitted):

```csharp
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
      }
  }
```

These settings configure the logging behavior of the ILogger API for our application. More precisely, they specify the minimum level to log for each category, based on the numeric value of the LogLevel enumeration class, from Trace (0) to None (6).

As we can see, the Default category, which represents the default fallback setting, is set to Information, so it will log Information and higher levels (thus including Warning, Error, and Critical). Another setting, however, overrides these default rules for the Microsoft.AspNetCore category, which has been configured to log only Warning and higher levels. The purpose of this override is to exclude the Information level for the Microsoft.AspNetCore category, cutting out the logging of several unnecessary pieces of information regarding that namespace.

In more general terms, the category settings follow a simple set of rules based on cascading and specificity concepts (the most specific one is the one that will be used):

Each category setting also applies to all its nested (child) categories. The logging settings for the Microsoft.AspNetCore category setting, for example, will also apply to all the categories starting with Microsoft.AspNetCore.* (such as Microsoft.AspNetCore.Http) unless they’re overridden by specific rules. The settings applied to a specific category always override those configured for any top-level category pertaining to that same namespace, as well as the Default fallback category. If we add a settings key for the Microsoft.AspNetCore.Http category with a value of Error, for example, we cut out the logging of all the Warning log events for that category, thus overriding the settings for the Microsoft.AspNetCore parent category.

It’s important to remember that we can set up environment-specific logging configuration settings by using the appsettings. <EnvironmentName>.json files, as we did in chapter 2 when we created the UseDeveloperExceptionPage setting. This approach allows us to have verbose logging behavior for our Development environment while enforcing a more restrictive (and confidential) approach in our Production environment.

Suppose that we want (or are asked) to limit the logging verboseness for our Production environment while increasing the granularity of the Development logs. Here’s what we need to do:

Production—Log only Warning and higher levels for all categories except MyBGList, where we want to log the Information level as well. Development—Log Information and higher levels for all categories except Microsoft.AspNetCore, where we’re interested only in Warning and higher levels, and MyBGList, where we want to log everything at Debug level and higher. The first thing to do is to open the appsettings.json file, which is where we’ll put the Production environment settings, and update it with the code in bold (nonrelevant part omitted):

```csharp
 "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "MyBGList": "Information"
    }
  }
```

Then we can proceed with the appsettings.Development.json file, which we need to update in the following way (nonrelevant part omitted):

"Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning", "MyBGList": "Debug" } }

Now that we’re familiar with the logging configuration settings, we may want to understand where all these log records go, because until now, we’ve seen them only in the Output window. To learn more, we need to know about logging providers.

### 7.2.4 Logging providers

A logging provider is a component that stores or displays logs. It receives the log records sent by the ILogger API based on the configuration settings—persists them somewhere. The following sections provide an overview of the built-in logging providers—those provided by the framework. How many are there, what do they do, and how can we configure them? Then we’ll review some third-party logging providers, which we can use to extend the capabilities of the ILogger API.

Built-in logging providers Here’s a list of the default logging providers shipped by the .NET framework through the Microsoft.Extensions.Logging namespace:

Console—Outputs the logs to the console. This provider allows us to view the log messages within the console window that ASP.NET Core opens to launch the MyBGList.exe process when we execute our app. Debug—Outputs the logs by using the WriteLine method provided by the System.Diagnostics.Debug class. When a debugger is connected, this provider allows us to view the log messages in the Visual Studio Output window, as well as store them in log files or registers (depending on the OS and the debugger settings). EventSource—Outputs the logs as runtime event traces so that they can be fetched by an event source platform such as Event Tracing for Windows or Linux Trace Toolkit next-gen. EventLog—Writes the logs in the Windows Event Log (available only for Windows operating systems). Three more logging providers can send logs to various Microsoft Azure data stores:

AzureAppServicesFile—Writes the logs to a text file within an Azure App Service filesystem (which needs to be set up and configured beforehand) AzureAppServicesBlob—Writes the logs to blob storage within an Azure Storage account (which needs to be set up and configured beforehand) ApplicationInsights—Writes the logs to an Azure Application Insights service (which needs to be set up and configured beforehand)

WARNING The Azure providers aren’t part of the runtime libraries and must be installed as additional NuGet packages. Because these packages are maintained and shipped by Microsoft, however, they’re considered to be among the built-in providers.

Multiple logging providers can be attached to the ILogger API (enabled) at the same time, allowing us to store and/or display our logs in several places simultaneously. This is precisely what happens with the configuration shipped with all the ASP.NET Core web app templates, including the one we used to create our MyBGList web API project, which adds the following logging providers by default: Console, Debug, EventSource, and EventLog (Windows only).

The configuration line responsible for these default settings is the WebApplication.CreateBuilder method called at the start of the Program.cs file. If we want to change this behavior, we can use a convenient ILoggingBuilder interface instance made available by the WebApplicationBuilder object returned by that method.

Suppose that we want to remove the EventSource and EventLog providers while keeping the other ones. Open the Program.cs file, locate the preceding method, and add the following lines (in bold) that call:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Logging
    .ClearProviders()      ❶
    .AddSimpleConsole()    ❷
    .AddDebug();           ❸
```

❶ Removes all registered logging providers
❷ Adds the Console logging provider
❸ Adds the Debug logging provider

Before we could get rid of those logging providers, we had to remove all the preconfigured ones (using the ClearProviders method). Then we added back only those that we want to keep.

Configuring the providers Most built-in logging providers can be configured programmatically via a dedicated overload of their add method that accepts an options object. Here’s how we could configure the console provider to timestamp its log entries, using HH:mm:ss format and the UTC time zone:

```csharp
builder.Logging
    .ClearProviders()
    .AddSimpleConsole(options =>     ❶
    {
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss ";
        options.UseUtcTimestamp = true;
    })
    .AddDebug();
```

❶ Option-based configuration added

We can check the new behavior immediately by launching our app in Debug mode, executing the BoardGamesController’s Get method where we put our custom log message, and looking at the command prompt window hosting the MyBGList.exe process. If we did everything correctly, we should find our log message with its new look:

15:36:52 info: MyBGList.Controllers.BoardGamesController[0] Get method started.

The HH:mm:ss timestamp is now clearly visible at the beginning of the line. But the option-based configuration method isn’t the recommended way to configure the logging providers for the latest versions of .NET. Unless we have specific needs, it may be better to use a more versatile approach that uses the "Logging" section of the appsettings.json files—the same section that we used to configure the LogLevel.

Let’s switch from the option-based configuration settings to this new method. The first thing to do is roll back the changes we made in the AddSimpleConsole method, reverting it to its parameterless overload in the following way:

builder.Logging
    .ClearProviders()
      .AddSimpleConsole()     ❶
      .AddDebug();

❶ Option-based configuration removed

Then we can open the appsettings.json file and add the following Console section block (nonrelevant part omitted):

  "Logging": {
    "LogLevel": {
       "Default": "Warning",
       "MyBGList": "Information"
    },
      "Console": {               ❶
        "FormatterOptions": {
          "SingleLine": true,
          "TimestampFormat": "HH:mm:ss ",
          "UseUtcTimestamp": true
        }
      }
  }

❶ Console logging provider using the appsettings.json file

We can use the same technique to override the LogLevel for specific logging providers. Here’s how we could limit the verboseness of the MyBGList category by setting its LogLevel to Warning for the console logging provider only:

```csharp
  "Logging": {
    "LogLevel": {
       "Default": "Warning",
       "MyBGList": "Information"
    },
    "Console": {
        "LogLevel": {             ❶
           "MyBGList": "Warning"
        },
        "FormatterOptions": {
           "TimestampFormat": "HH:mm:ss ",
           "UseUtcTimestamp": true
        }
      }
  }
```

❶ LogLevel settings override for the Console provider

As we can see by looking at this code, we can override the settings specified in the generic LogLevel section by creating a new LogLevel subsection inside the logging provider configuration. We can confirm that the override works by executing our app from scratch (or hot-reloading it) and looking at the same command prompt window as before. We shouldn’t see our custom log message, because it belongs to a LogLevel that we’re not logging for that category anymore.

WARNING The appsettings-based configuration approach can be convenient, as it allows us to customize the logging behavior of all our providers, and/or to set environment-specific rules, without having to update the Program.cs file. But this approach also requires some practice and study, because each provider has specific configuration settings. The Debug logging provider, for example, doesn’t have the TimestampFormat and UseUctTimestamp settings, so those values would be ignored if we tried to use them within a LogLevel:Debug section. This aspect will become evident when we configure third-party logging providers.

Let’s remove the Logging:Console:LogLevel section from our appsettings.json file (in case we’ve added it) so that it won’t hinder our upcoming tests.

### 7.2.5 Event IDs and templates

If we look again at the log message written in the MyBGList.exe console window by the console logging provider, we notice the presence of a numeric value within square brackets after the name of the class. That number, which happens to be [0] (zero) in our custom logging message, represents the event ID of the log. We can think of it as contextual info that we can use to classify a set of events that have something in common, regardless of their category. Suppose that we want (or are asked) to classify all logs related to our BoardGamesController’s Get method with an event ID of
50110. The following sections show how we can implement that task.

Setting custom event IDs To adopt such a convention, we need to replace our current implementation, switching to the LogInformation’s method overload that accepts a given event ID as its first parameter (updated code in bold):

_logger.LogInformation(50110, "Get method started.");

If we launch our app in Debug mode and check the MyBGList.exe console window, we see the event ID in place of the previous value of 0:

15:43:15 info: MyBGList.Controllers.BoardGamesController[50110] Get method started.

Classifying our log messages can be useful, because it allows us to group them together (or filter them) whenever we need to perform some checks and/or audit activities. Having to set these numbers manually each time could be inconvenient, however, so let’s create a CustomLogEvents class to define them in a centralized place.

In Visual Studio’s Solution Explorer window, create a new /Constants/ root folder, and add a new CustomLogEvents.cs class file inside it. Then fill the new file with the content of the following listing.

**Listing 7.1 CustomLogEvents class**

```csharp
namespace MyBGList.Constants
{
    public class CustomLogEvents
    {
        public const int BoardGamesController_Get = 50110;
        public const int BoardGamesController_Post = 50120;
        public const int BoardGamesController_Put = 50130;
        public const int BoardGamesController_Delete = 50140;

        public const int DomainsController_Get = 50210;
        public const int DomainsController_Post = 50220;
        public const int DomainsController_Put = 50230;
        public const int DomainsController_Delete = 50240;

        public const int MechanicsController_Get = 50310;
        public const int MechanicsController_Post = 50320;
        public const int MechanicsController_Put = 50330;
        public const int MechanicsController_Delete = 50340;
    }
}
```

Now we can go back to our BoardGamesController class and replace the numeric literal value with the constant that we created to reference it:

using MyBGList.Constants;                                          ❶

// ... nonrelevant code omitted

_logger.LogInformation(CustomLogEvents.BoardGamesController_Get,   ❷
    "Get method started.");
❶ New required namespace
❷ New constant instead of a literal value

The new implementation is much more readable and less error- prone, because we don’t have to type the numbers for each event ID within the code (and run the risk of typing them wrong).

NOTE Not all logging providers display the event ID, and not all of them put it in square brackets at the end of the line. The Debug provider doesn’t show it, and other structured providers (such as the Azure ones) persist it in a specific column that we can choose to display or not.

Using message templates The ILogger API supports a template syntax that we can use to build log messages, employing a string-formatting technique similar to the one provided by the string.Format C# method. Instead of using numbers to set the placeholders, however, we can use a name. Instead of doing this

string.Format("This is a {0} level log", logLevel);

we can do this:

_logger.LogInformation("This is a {logLevel} level log", logLevel);

The difference between the two approaches is that the latter is more human-readable; we immediately understand what the placeholder is for. If we need to use multiple placeholders in our template, we put their corresponding variables in the correct order, as in the following example:

_logger.LogInformation( "This is a {logLevel} level log of the {catName} category.", logLevel, categoryName);

We deliberately used the {catName} placeholder for the categoryName parameter to clarify that each placeholder will receive its value from the parameter that corresponds to its order, not to its name.

### 7.2.6 Exception logging

The ILogger API can also be used to log exceptions, thanks to some dedicated overloads provided by all the logging extension methods that accept an Exception? as a parameter. This feature can be convenient for our MyBGList application because we’re handling exceptions through a centralized endpoint.

In chapter 6, being able to log our exceptions was one of the main reasons that led us to implement a centralized logging handling approach through the ExceptionHandlerMiddleware. Now the time has come to turn this capability into something real.

Open the Program.cs file and scroll down to the Minimal API’s MapGet method that handles the /error route. We need to provide an ILogger instance that we can use to log the exception’s details. To obtain the ILogger instance, we could think about injecting an ILogger interface instance by using dependency injection in the following way: app.MapGet("/error", [EnableCors("AnyOrigin")] [ResponseCache(NoStore = true)] (HttpContext context, ILogger logger)

Then we could use the logger variable within the MapGet method’s implementation to perform our logging tasks. But because we’re in the Program.cs file, we can use the default ILogger instance provided by the WebApplication object instead. This instance is accessible through the Logger property of the app local variable—the same variable that we use to add our middleware to the request pipeline. Here’s how we can take advantage of this property to implement our exception logging change request:

```csharp
// Minimal API
app.MapGet("/error",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] (HttpContext context) =>
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
```

```csharp
          app.Logger.LogError(          ❶
              exceptionHandler?.Error,
              "An unhandled exception occurred.");
```

```csharp
          return Results.Problem(details);
    });
```

❶ Exception logging While we’re here, we could create a new event ID for this specific task so that we’ll be able to filter all the error log entries related to exceptions. Switch to the /Constants/CustomLogEvents.cs file, and add the following constant at the beginning of the class, right above the existing ones:

public const int Error_Get = 50001;

Now we can switch to the Program.cs file and change the LogError method implementation with the override accepting an eventId parameter:

using MyBGList.Constants;

// ... nonrelevant code omitted

app.Logger.LogError(
    CustomLogEvents.Error_Get,     ❶
    exceptionHandler?.Error,
    "An unhandled exception occurred.");

❶ Custom Event ID

Now that we’ve gained confidence with the ILogger API, we’re ready to talk about third-party logging providers, which allow us to persist these logs in structured data stores, such as our database management system (DBMS). First, however, we’ll spend some valuable time examining the difference between structured and unstructured logging.

## 7.3 Unstructured vs. structured logging

All the built-in logging providers that we’ve briefly reviewed (except for the Azure ApplicationInsights provider, which we’ll talk about later) have a common characteristic: they store (or show) the log information by using raw strings. In other words, the log records have the appearance of textual data that gets stored (or shown) in an unstructured way. The following list summarizes the differences between unstructured, semistructured, and structured data:

Unstructured data—Raw data that isn’t organized with a predefined data model, be it a database schema, a spreadsheet file with columns, a structured XML/JSON file, or anything else that allows splitting the relevant parts of the recorded data among fields—in other words, a plain text record. Semistructured data—Data that doesn’t reside in a DBMS but comes with some organizational properties that make it easier to analyze, parse, and/or process the content (such as to seed an actual DBMS). A good example of semistructured data is the board game CSV file that we used in chapter 5 to seed our MyBGList database. Structured data—Data that has been organized into an addressable repository, such as a DBMS, and that is ready for effective analysis without our having to parse it. A typical example of structured data is a data set of records such as those that have populated our MyBGList tables since chapter 5. We can say without doubt that the log records we’ve played with until now—thanks to the console’s built-in provider—belong to the unstructured family. Is that a bad thing? Not necessarily, provided that we’re fully aware of the benefits and drawbacks of this logging strategy.

### 7.3.1 Unstructured logging pros and cons

The unstructured logging approach undoubtedly has relevant advantages, including accessibility. We need a console or a text editor to view these records and fully understand their content. Furthermore, because the records are typically stored one after another, the log-reviewing phase is often rather quick and convenient, especially when we need to access the latest entries, maybe because we know that the event that triggered the log we’re looking for occurred a short while ago.

Unfortunately, when things become more complicated, these benefits tend to disappear. If we need to retrieve a specific log entry without knowing when the relevant event occurred, or even whether it occurred, we might have a hard time finding it. Our only tools would be text-based search tools such as the Notepad Find feature and the Linux grep command. This task can become even more troublesome when these log files reach critical mass, which can happen quickly if we activate particularly verbose LogLevels. We all know how difficult it is to access and browse those gigantic files, not to mention perform search operations on them. Retrieving specific information from thousands of unstructured log entries can easily become a frustrating task; it’s definitely not a quick and convenient way to get the job done.
NOTE This problem is so well known in the IT ecosystem that it resulted in the birth of several log management tools that ingest unstructured log records and then aggregate, parse, and/or normalize them by using standard or user-defined patterns, rules, or schemas. These tools include Graylog, LogDNA, Splunk, and NewRelic.

In short, what’s really lacking in the unstructured logging produced by most built-in logging providers is a feature that lets us filter those records in a practical way through one or more parameters, such as log level, event ID, transaction number, and a start/end date and time —in other words, to query them. We can overcome this limitation by adding a structured logging provider—a logging provider that allows us to write these records to a service that allows structured data storage, such as a DBMS.

### 7.3.2 Structured logging advantages

Following are the main benefits of a structured logging strategy:

We don’t need to rely on text-editor tools to read logs in manual mode. We don’t need to write code to parse or process the logs in automatic mode. We can query the log records by using relevant fields, as well as aggregate them with external data. We could issue a JOIN query, for example, to extract only the logs triggered by user actions together with some relevant user data (such as user ID, name, and email address). We can extract and/or convert the logs in other formats, possibly including only the relevant information and/or omitting information that shouldn’t be disclosed. Performance benefits, thanks to the indexing features of the DBMS, make the retrieval process more efficient even when we’re dealing with a large number of records.

These benefits are a direct consequence of the fact that the log record is stored by using a predefined data structure. Some indirect benefits are also worth considering, determined by the fact that any structured logging provider is meant to rely on a data storage service that will ultimately store the data. These benefits vary depending on what type of data storage we choose. We could store our logs in various ways:

Within our existing MyBGList database so that we can use EF Core to access it and keep all our application-relevant data in a single, centralized place In a separate database on the same DBMS instance (SQL Server) so that we can keep the log records logically separated from the board-game data while still being able to access them through EF Core to a certain extent In an external repository located elsewhere (such as a third-party service or DBMS) so that the log records are completely independent of the application’s infrastructure and therefore more resilient against failures, leakage, tampering, and so on

As we can see, all these options are significant. We could even think about mixing those logging strategies, because the ILogger interface supports multiple logging providers, and most third-party providers support multiple output destinations.
TIP We could also keep the built-in logging providers that we configured earlier—Console and Debug—unless we want to remove or replace them. This approach would allow us to set up structured and unstructured logging at the same time.

That’s enough theory. Let’s see how we can add these valuable tools to our toolbox.

### 7.3.3 Application Insights logging provider

As I said earlier, the only built-in alternative that we could use to store event logs in a structured format is the Application Insights logging provider. In this section, we’ll briefly see how we can set up this logging provider for our MyBGList web API. This task requires us to create an account in Microsoft Azure, the cloud provider that hosts the Application Insights service and makes it available. First, however, I’ll briefly introduce Azure and Application Insights and discuss the benefits that cloud-based services such as Application Insights can bring to modern web applications such as our MyBGList web API.

Introducing Microsoft Azure Microsoft Azure (often referred to as MS Azure or simply Azure) is a public cloud computing platform owned and maintained by Microsoft. It was announced in 2008, formally released in 2010 as Windows Azure, and renamed in 2014. The platform relies on a huge network of Microsoft data centers worldwide. It offers more than 600 services provided through Software as a Service (SaaS), Platform as a Service (PaaS), and Infrastructure as a Service (IaaS) delivery models, using large-scale virtualization technologies. It follows the subscription-based approach used by its most notable competitors: Amazon Web Services (AWS) and Google Cloud Platform (GCP).

Azure services can be managed via a set of APIs that are directly accessible via the web and/or managed class libraries available for various programming languages. At the end of 2015, Microsoft also released the Azure Portal, a web-based management interface allowing users to manage most of the services in a visual graphical user interface (GUI). The release of the Azure Portal greatly helped Azure increase its market share, which (according to Canalys) reached 21 percent in the first quarter of 2022, with AWS at 33 percent and GCP at 8 percent.

TIP The Canalys report is available at http://mng.bz/pdqK.

Introducing Application Insights Among the services made available by Azure is Azure Monitor, a comprehensive solution that can be used to collect and analyze logs, audit trails, and other performance-related output data from any supported cloud-based and on-premises environment or service, including web applications. The Azure Monitor feature dedicated to ingesting, monitoring, and analyzing the log and informative status messages generated by live web applications is called Application Insights.

NOTE We could think of Application Insights as a Google Analytics of some sort. Instead of monitoring our web application’s page views and sessions, however, it’s meant to monitor and analyze its logs and status messages. The presence of the Application Insights provider among the built-in logging providers is also the main reason why we’ve chosen to deal with Azure instead of AWS and GCP.

Creating the Azure instance Now that we know the basics of Azure and Application Insights, we can proceed with the service setup. To use the Application Insights service, we need to have a valid Azure account. Luckily, the service offers a basic pricing plan that has almost no charge until our log data becomes substantial.

TIP To create a free account, go to https://azure.microsoft.com/en- us/free.

The first thing to do is log into the Azure Portal. Then use the search text box at the top of the main dashboard to find and access the Application Insights service. Click the Create button in the top-left corner of the screen to create a new Application Insights instance, as shown in figure 7.1.
Figure 7.1 Creating a new instance

The creation process is simple and requires us only to set up a few parameters:

Subscription—The Azure subscription to create this instance in. (If you don’t have a subscription, you’ll be asked to create a free one for a 30-day trial.) Resource Group—The Azure resource group to create this instance in. Name—The instance name. Region—The geographic region in which to create the instance. Resource Mode—Choose Classic if you want this Application Insights instance to have its own environment or Workspace- Based if you want to integrate it with an existing Log Analytics workspace. For simplicity, choose Classic (figure 7.2).
Figure 7.2 Configuration settings

Click Review + Create to review these settings and then click Create to finalize the instance deployment process. As soon as the deployment is complete, click Go to Resource, which takes us to our new Application Insights instance’s management dashboard, where we can retrieve the connection string (figure 7.3).
Figure 7.3 Retrieving the Application Insights connection string

We’re going to need that value when we configure the Application Insights logging provider so that it will be able to send log events to that instance. Let’s store it in an appropriate place.

Storing the connection string A great place to store the Application Insights connection string for development purposes is the secrets.json file, which we’ve been using since chapter 4. In the Visual Studio Solution Explorer window, right-click the project’s root node and choose Manage User Secrets from the contextual menu. Here’s the section block that we can add to the existing content, below the ConnectionStrings key (nonrelevant parts removed):

```csharp
  "Azure": {
    "ApplicationInsights": {
          "ConnectionString": "<INSERT_CONNECTION_STRING_HERE>"   ❶
      }
  }
```

❶ Puts the connection string here

Now we know where we’ll retrieve the connection string when we configure the logging provider. First, however, we must install it.

Install the NuGet packages Now that we have the MyBGList Application Insights instance in Azure and the connection string available, we can install the Application Insights logging provider NuGet packages. As always, we can use Visual Studio’s NuGet GUI, the Package Manager Console window, or the .NET command-line interface (CLI). Here are the commands to install them by using the .NET CLI:

> dotnet add package Microsoft.Extensions.Logging.ApplicationInsights --version 2.21.0

> dotnet add package Microsoft.ApplicationInsights.AspNetCore
➥    --version 2.21.0

NOTE The version specified in the example is the latest stable version available at this writing. I strongly suggest using that version as well to avoid having to deal with breaking changes, incompatibility problems, and the like. The first package contains the provider itself, and the second is required to configure it by using the appsettings.json file, as we did with the Console logging provider earlier. When the packages are installed, we can configure the provider.

Configuring the logging provider The configuration part takes place in the Program.cs file of our app. Open that file, locate the part where we defined the logging providers, and add the new one to the loop in the following way:

builder.Logging
  .ClearProviders()
  .AddSimpleConsole()
  .AddDebug()
  .AddApplicationInsights(                     ❶
    telemetry => telemetry.ConnectionString =
      builder
        .Configuration["Azure:ApplicationInsights:ConnectionString"],
      loggerOptions => { });

❶ Adds the Application Insights logging provider

The new configuration line will activate the Application Insights logging provider for our MyBGList web API. All we need to do now is to see whether it works.

Testing the Application Insights event logs Launch the project in Debug mode, and navigate to the /BoardGames endpoint to trigger some event logs. Then go back to the main dashboard of the Application Insights service in Azure, and choose Investigate > Transaction from the right menu. If we did everything correctly, we should see our application’s event logs as TRACE event types, as shown in figure 7.4.

Figure 7.4 Application logs shown as TRACE event types

As we can see, the event logs are accessible in a structured format. We can even create some queries by using the GUI, such as filtering the entries with a severity level equal to or greater than a given value. That’s a great improvement over unstructured logging! This technique has a major limitation, however: the logging provider we added, as its name clearly implies, is limited to Azure’s Application Insights service. We won’t be able to store these structured logs anywhere else, such as within our existing SQL Server database or any other DBMS. To achieve this capability, we have to use a third- party logging provider that provides support for these kinds of output destinations.

## 7.4 Third-party logging providers

This section introduces Serilog, an open source logging library for .NET applications available on NuGet that allows us to store our application logs in several popular DBMSes (including SQL Server and MariaDB), as well as third-party services.

NOTE We chose Serilog over other alternatives because it’s one of the most popular third-party logging providers available, with more than 480 million downloads and 1,000 stars on GitHub at this writing, as well as being open source (Apache 2.0 license).

After seeing how Serilog works, we’ll see how to install it in our MyBGList web API project and make it work along with the built-in logging providers we already have.

### 7.4.1 Serilog overview

The first thing we must understand is that Serilog isn’t only a logging provider that implements the Microsoft.Extensions.Logging.ILogger interface along with the other logging providers. It’s a full-featured logging system that can be set up to work in two different ways:

As a logging API—Replaces the .NET logging implementation (including the ILogger interface) with its own native interface As a logging provider—Implements the Microsoft extensions logging API (and extends it with several additional features) instead of replacing it

The main difference between these two architectural approaches is that the first one requires setting up a dependency on the Serilog interface in all our codebases, thus replacing the ILogger interface we’ve used up to now. Although this requirement isn’t necessarily bad, I think that keeping the Microsoft logging API is generally a better choice because it’s a better fit with the modular structure of a typical ASP.NET Core web app, ensuring a more flexible system. For that reason, we’re going to follow the second approach. Regardless of how we choose to set it up, Serilog provides two main advantages over most other logging providers:

Enrichers—A set of packages that can be used to add additional info automatically to log events (ProcessId, ThreadId, MachineName, EnvironmentName, and so on) Sinks—A selection of output destinations, such as DBMSes, cloud-based repositories, and third-party services

Because Serilog was built with a modular architecture, all enrichers and sinks are available via dedicated NuGet packages that we can install along with the library core package(s) whenever we need them.

### 7.4.2 Installing Serilog

For simplicity, we’ll take for granted that in our scenario, we want to use Serilog to store our log events in our SQL Server database. We need to install the following packages:

Serilog.AspNetCore—Includes the core Serilog package, integration into the ASP.NET Core configuration and hosting infrastructure, some basic enrichers and sinks, and the middleware required to log the HTTP requests Serilog.Sinks.MSSqlServer—The sink for storing event logs in SQL Server

To do that, we can use the NuGet GUI or the following commands in the Package Manager console:

> dotnet add package Serilog.AspNetCore --version 6.0.1 > dotnet add package Serilog.Sinks.MSSqlServer --version 6.0.0

TIP The versions are the latest stable versions available at the time of this writing. I strongly suggest using them as well to avoid having to deal with breaking changes, incompatibility problems, and the like.

### 7.4.3 Configuring Serilog

Configuration takes place in the Program.cs file of our app. We’re going to use the UseSerilog extension method, provided by the Serilog.AspNetCore package, which will set Serilog as the main logging provider and set up the SQL Server sink with some basic settings. Open the Program.cs file, and add the following lines of code below the builder.Logging configuration settings we added earlier:

builder.Logging                                     ❶
  .ClearProviders()
  .AddSimpleConsole()
  .AddDebug()
  .AddApplicationInsights(
    telemetry => telemetry.ConnectionString =
      builder
        .Configuration["Azure:ApplicationInsights:ConnectionString"],
      loggerOptions => { });

builder.Host.UseSerilog((ctx, lc) => {               ❷
    lc.ReadFrom.Configuration(ctx.Configuration);
    lc.WriteTo.MSSqlServer(
       connectionString:
         ctx.Configuration.GetConnectionString("DefaultConnection"),
       sinkOptions: new MSSqlServerSinkOptions
       {
         TableName = "LogEvents",
         AutoCreateSqlTable = true
       });
    },
    writeToProviders: true);

❶ Built-in logging provider configuration
❷ Serilog configuration

This code requires us to add the following namespace references at the top of the file:

```csharp
using Serilog;
using Serilog.Sinks.MSSqlServer;
```

Notice that we specified the same SQL Server connection string that we’re using with EF Core. We did that because in this scenario, we want to use the same MyBGList database we’re already using to store the board-game-related data.
NOTE Theoretically speaking, we could enforce a separation-of- concerns approach and create a dedicated logging database. Both approaches are perfectly viable, although they have pros and cons that could make them more or less viable in various circumstances. For simplicity, we’ll assume that keeping everything in a single database is a valid choice for our scenario.

If we look at the code, we see that we named the table that will host the log records ("LogEvents"). Moreover, we configured the SQL Server sink to autocreate it in case it doesn’t exist. This feature is a great built-in feature of that sink because it allows us to delegate the task without having to create the table manually, which would involve the risk of choosing the wrong data types. The autogenerated table will have the following structure:

[Id] [int] IDENTITY(1,1) NOT NULL [Message] [nvarchar](max) NULL [MessageTemplate] [nvarchar](max) NULL [Level] [nvarchar](max) NULL [TimeStamp] [datetime] NULL [Exception] [nvarchar](max) NULL [Properties] [nvarchar](max) NULL

Last but not least, we set the writeToProviders parameter to true. This option ensures that Serilog will pass the log events not only to its sinks, but also to the logging providers registered through the Microsoft.Extensions.Logging API, such as the built- in providers we configured earlier. This setting defaults to false because the Serilog default behavior is to shut down these providers and replace them with the equivalent Serilog sinks. We don’t want to enforce this behavior, however, because we want to see how the built-in and third-party logging providers can be configured to work side by side. The configuration part is over, for the most part; now it’s time to test what we’ve done so far.

### 7.4.4 Testing Serilog

Again, launch the MyBGList project in Debug mode, and execute the /BoardGame endpoint to trigger some log events. This time, instead of looking at the command-prompt output, we need to launch SQL Server Management Studio (SSMS) and connect to the MyBGList database. If everything worked properly, we should see a brand-new LogEvents table filled with structured event logs, as shown in figure 7.5.
Figure 7.5 LogEvents table filled with event logs

If we take a closer look at the log events recorded in the table (in the bottom-right part of figure 7.5), we even see the log record related to the “custom” log event entry that we added to the BoardGameController. Our test was a success. We have a structured logging engine that records our log entries within our database.

### 7.4.5 Improving the logging behavior

In this section, we’ll see how we can further improve our Serilog- based structured logging behavior with some of the many features provided by the library: adding columns, configuring the minimum logging level, customizing the log messages by using the Serilog template syntax, enriching our logs, and adding sinks.

Adding columns If we take a closer look at the log entries that fill the LogEvents table by using SSMS (figure 7.5), we notice that something is missing: columns to store the log record’s source context (the namespace of the class that originated it) or the EventId. The reason is simple: instead of recording these values in dedicated columns, Serilog stores them in the Properties column. If we look at that column’s value for our custom log entry, we see that both values are there, wrapped in an XML structure together with other properties (nonrelevant parts omitted):

  <propertykey='EventId'>                       ❶
    <structure type=''>
      <property key='Id'>
        50110                                   ❷
      </property>
    </structure>
  </property>
  <property key='SourceContext'>                ❶
    MyBGList.Controllers.BoardGamesController   ❷
  </property>

❶ Property name
❷ Property value

Notice that EventId is a complex property because it can contain values of different types (int, string, and so on), whereas the SourceContext property hosts a simple string (the namespace of the class that originated the log record). Having these values stored within this XML semistructured format could be good enough for most scenarios. But Serilog also allows us to store these properties in their own individual columns. These features are enabled through an optional columnOptions parameter that can be specified in the configuration settings, so we can add a collection of AdditionalColumns and map them to these properties.

Suppose that we want to store the SourceContext property in a dedicated column. Here’s how we can use the columnOptions parameter to do that:

builder.Host.UseSerilog((ctx, lc) => {
    lc.ReadFrom.Configuration(ctx.Configuration);
    lc.WriteTo.MSSqlServer(
        connectionString:
            ctx.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "LogEvents",
            AutoCreateSqlTable = true
        },
        columnOptions: new ColumnOptions()          ❶
        {
            AdditionalColumns = new SqlColumn[]
            {
                 new SqlColumn()                    ❷
                 {
                     ColumnName = "SourceContext",
                     PropertyName = "SourceContext",
                     DataType = System.Data.SqlDbType.NVarChar
                 }
             }
        }
        );
    },
    writeToProviders: true);

❶ Configures the columnOptions optional parameter
❷ Adds a column for the SourceContext property

Now we need to delete the LogEvents table from the MyBGList database so that Serilog will be able to regenerate it with the new SourceContext column. Then we need to launch the MyBGList project in Debug mode, visit the /BoardGame endpoint to trigger the log records, and view the result in SSMS.

If everything worked as expected, we should see the new SourceContext column with the various namespaces, as shown in figure 7.6. (Don’t worry if you can’t read the text; the figure shows only where to find the new column.)

Figure 7.6 LogEvents table with the new SourceContext column Now that we can look at these namespaces, we notice another problem. Why are we logging those log records with an Information level coming from the Microsoft.AspNetCore namespace? If we look at the appsettings.Development.json file, we see that we opted them out, using the Logging:LogLevel configuration key:

"Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning "MyBGList": "Debug" } },

The bold line clearly shows that we excluded all event logs with a LogLevel lower than Warning, which includes Information logs. If that’s the case, why are we still logging them?

The answer is simple. Although Serilog can be configured via the appsettings .json file(s), like the built-in logging providers, it uses its own configuration section, which also replaces the Logging section used by the default logger implementation.

NOTE The Logging:LogLevel section that we’ve had in our appsettings.json file(s) since chapter 2 is now useless, to the point that we could remove it (unless we want to disable Serilog and roll back to the default logger). That said, we’ll keep that section for reference purposes.

The new configuration section is called “Serilog” and has a slightly different syntax. In the next section, we’ll see how we can configure it. Configuring the minimumLevel To mimic the same LogLevel behavior that we configured in the Logging section, we need to add a new top-level “Serilog” section to the appsettings.json file below the existing “Logging” section. Here’s how:

```csharp
  "Logging": {
       // omissis...
  },
  "Serilog": {
     "MinimumLevel": {
        "Default": "Warning",
        "Override": {
          "MyBGList": "Information"
        }
     }
  }
```

Here is the corresponding section for the appsettings.Development.json file:

```csharp
  "Logging": {
       // omissis...
  },
  "Serilog": {
     "MinimumLevel": {
        "Default": "Information",
        "Override": {
          "Microsoft.AspNetCore": "Warning",
          "MyBGList": "Debug"
        }
     }
  }
```

The MinimumLevel could also be configured programmatically in
the Program.cs file (option-based approach) in the following way:
builder.Host.UseSerilog((ctx, lc) => {
      lc.MinimumLevel.Is(Serilog.Events.LogEventLevel.Warning);    ❶
      lc.MinimumLevel.Override(                                    ❷
          "MyBGList", Serilog.Events.LogEventLevel.Information);

// ... omissis ...

❶ Default MinimumLevel value
❷ Overrides value for specific sources

As we can see, we can configure a default behavior for all log event sources and then some namespace-based overrides, like the default logger settings.

WARNING To determine the log level, Serilog doesn’t use the Microsoft.Extensions.Logging.LogLevel enum. It uses the proprietary Serilog.Events.LogEventLevel enum, which features slightly different names: Verbose, Debug, Information, Warning, Error, and Fatal. The most relevant difference between the two enums is the absence of Trace and None in the Serilog counterpart, as well as the Critical level, which has been renamed Fatal.

The settings specified in the MinimumLevel section will be applied to all sinks. If we need to override them for a specific sink, we can use the restrictedToMinimumLevel setting in the appsettings.json files (configuration-based approach) in the following way:

```csharp
  "Serilog": {
    "WriteTo": [{
      "Name": "MSSqlServer",
        "Args": {
              "restrictedToMinimumLevel": "Warning",    ❶
          }
        }]
  }
❶ Sets up a minimum LogEventLevel for this sink
```

Or we can use the Program.cs file (option-based approach) in the following way:

lc.WriteTo.MSSqlServer(
    restrictedToMinimumLevel:                      ❶
        Serilog.Events.LogEventLevel.Information

// ... omissis ...

❶ Sets up a minimum LogEventLevel for this sink

For our sample project, we’ll keep the default behavior for all sinks. Therefore, we won’t use the restrictedToMinimumLevel setting within our code.

Message template syntax Now that we’ve restored the logging level configuration, we can explore another great Serilog feature: the extended message template syntax. The default logger syntax provides an overload that allows us to use the standard .NET composite formatting feature, meaning that instead of writing this

_logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "Get method started at " + DateTime.Now.ToString("HH:mm"));

we could write something like this

_logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "Get method started at {0}", DateTime.Now.ToString("HH:mm"));

or this: _logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "Get method started at {0:HH:mm}", DateTime.Now);

The .NET composing formatting feature is powerful, but it has major drawbacks in terms of readability, especially when we have a lot of placeholders. For this reason, this feature is often neglected in favor of string interpolation (introduced in C# version 6), which provides a more readable, convenient syntax for formatting strings. Here’s how we can implement the same logging entry as before by using string interpolation:

_logger.LogInformation(CustomLogEvents.BoardGamesController_Get, $"Get method started at {DateTime.Now:HH:mm}");

TIP For additional info about the .NET composing formatting feature, check out http://mng.bz/eJBq. For additional info regarding the C# string interpolation feature, see http://mng.bz/pdZw.

Serilog extends the .NET composing formatting feature with a message template syntax that not only fixes the readability problem, but also provides additional advantages. The fastest way to understand how the improved syntax works is to see how we could use it to write the previous log entry:

_logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "Get method started at {StartTime:HH:mm}.", DateTime.Now);

As we can see, the message template syntax allows us to use string- based placeholders instead of numeric ones, which improves readability. But that’s not all: all the placeholders will also be treated (and stored) as properties automatically, so we’ll find them in the Properties column (within the XML structure). This convenient feature can be useful for performing query-based lookups within the log table, because all these values would be recorded in a semistructured (XML) fashion.

TIP We could even store the values in dedicated columns by using the columnOptions configuration parameter, as we did with SourceContext earlier, thus having them recorded in a structured way.

Thanks to this powerful placeholder-to-property feature, writing log messages with the Serilog’s message template syntax is generally preferable to writing them with the C# string interpolation feature.

Adding enrichers Now that we know the basics of Serilog’s templating features, we can give our log records additional information regarding the application’s context by using some of the enrichers provided by Serilog. Suppose that we want to add the following info to our logs:

The name of the executing machine (equivalent to %COMPUTERNAME% for Windows systems or $HOSTNAME for macOS and Linux systems) The unique ID of the executing thread

To fulfill this request, we could write our log message(s) in the following way:

_logger.LogInformation(CustomLogEvents.BoardGamesController_Get,
    "Get method started [{MachineName}] [{ThreadId}].",            ❶
    Environment.MachineName,                                       ❷
    Environment.CurrentManagedThreadId);                           ❸

❶ Adds placeholders
❷ Retrieves the MachineName
❸ Retrieves the ThreadId

This approach retrieves these values and—thanks to the placeholder- to-property feature provided by the message template syntax— records them in the log record’s Properties column. But we would be forced to repeat this code for every log entry. Furthermore, those values will also be present in the Message column, whether we want them there or not.

The purpose of Serilog’s enrichers is to achieve the same outcome in a transparent way. To implement them, we need to install the following NuGet packages:

> dotnet add package Serilog.Enrichers.Environment --version 2.2.0 > dotnet add package Serilog.Enrichers.Thread --version 3.1.0

Then we can activate them, modifying the Serilog configuration in the Program.cs file in the following way (nonrelevant part omitted):

```csharp
builder.Host.UseSerilog((ctx, lc) => {
    lc.ReadFrom.Configuration(ctx.Configuration);
    lc.Enrich.WithMachineName();                    ❶
    lc.Enrich.WithThreadId();                       ❷
```

❶ Adds the Environment enricher
❷ Adds the Thread enricher

Now the MachineName and ThreadId properties will be created automatically in all our log records. Again, we can choose between keeping them in the Properties columns (semistructured) or store them in a structured format by adding a couple of columns, as we did for SourceContext. Adding other Sinks Before completing our Serilog journey, let’s add another sink. Suppose that we want to write our log events to a custom text file. We can implement this requirement easily by using Serilog.Sinks.File, a sink that writes log events to one or more customizable text files. As always, the first thing to do is install the relevant NuGet package:

> dotnet add package Serilog.Sinks.File --version 5.0.0

Next, open the Program.cs file, and add the sink to the Serilog configuration in the following way:

```csharp
builder.Host.UseSerilog((ctx, lc) => {
    lc.ReadFrom.Configuration(ctx.Configuration);
    lc.Enrich.WithMachineName();
    lc.Enrich.WithThreadId();
     lc.WriteTo.File("Logs/log.txt",               ❶
         rollingInterval: RollingInterval.Day);    ❷
```

// ... non-relevant parts omitted ...

❶ Adds the sink, specifying a file path and name
❷ Configures the sink

These settings will instruct the sink to create a log.txt file in the /Logs/ folder (creating it if it doesn’t exist) with a rolling interval of one day. The rolling interval is the interval at which the sink will create a new file to store the logs. An interval of one day means that we’ll have a single file for each day.

NOTE The rolling interval also influences the filenames, because the sink—per its default behavior—will timestamp them accordingly. Our log filenames will be log<yyyyMMdd>.txt, such as log20220518.txt, log20220519.txt, and the like.

All we need to do to test the sink is launch our project in Debug mode, wait for it to load, and then check out the project’s root folder. If everything went as expected, we should find a new /Logs/ folder containing a log<yyyyMMdd>.txt file with the logs formatted in the following way:

2022-05-18 04:07:57.736 +02:00 [INF] Now listening on: https://localhost:40443 2022-05-18 04:07:57.922 +02:00 [INF] Now listening on: http://localhost:40080 2022-05-18 04:07:57.934 +02:00 [INF] Application started. Press Ctrl+C to shut down. 2022-05-18 04:07:57.939 +02:00 [INF] Hosting environment: Development

The log entries are written by means of the default output template provided by the sink. If we want to customize the default template, we can use the outputTemplate configuration property. Suppose that we want to include the MachineName and ThreadId properties with which we enriched our logs a short while ago. Here’s how we can achieve that:

    lc.WriteTo.File("Logs/log.txt",
        outputTemplate:                               ❶
            "{Timestamp:HH:mm:ss} [{Level:u3}] " +
            "[{MachineName} #{ThreadId}] " +
            "{Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day);

❶ Defines a custom output template

The custom template results in the following outcome: 04:35:11 [INF] [PYROS #1] Now listening on: https://localhost:40443 04:35:11 [INF] [PYROS #1] Now listening on: http://localhost:40080 04:35:11 [INF] [PYROS #1] Application started. Press Ctrl+C to shut down. 04:35:11 [INF] [PYROS #1] Hosting environment: Development

As we can see, now the MachineName and ThreadId property values are present in each log entry.

TIP For reasons of space, I won’t dig further into Serilog. To find additional info about it, as well as its enrichers, sinks, and message template syntax, check out the library official wiki at https://github.com/serilog/serilog/wiki.

## 7.5 Exercises

It’s time to challenge ourselves with the usual list of hypothetical task assignments given by our product owner.

NOTE The solutions to the exercises are available on GitHub in the /Chapter_07/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 7.5.1 JSON console logging

Replace the built-in simple console logging provider with the built-in JSON console logging provider.

### 7.5.2 Logging provider configuration

Set the JSON console logging provider’s TimeStampFormat to log only hours and minutes (without seconds), using the UTC time zone and the options-based approach.

### 7.5.3 Exception logging’s new property

In the exception handler Minimal API method, add a new errorMessage custom property to the error log message, using Serilog’s message template syntax. The new property must contain the exception’s Message value.

### 7.5.4 New Serilog enricher

Enrich the current log configuration by adding the ThreadName property. Then modify Serilog’s file sink so that the ThreadName value will be written right after the ThreadId value, separated by a space.

### 7.5.5 New Serilog sink

Add another file sink below the existing one, with the following requirements:

Filename—/Logs/errors.txt

Output template—Same as the existing file sink Log level to record—Error and Fatal only Rolling interval—Daily

Summary Logging is the process of keeping track of all the events occurring within the application in a structured, semistructured, and/or unstructured format and sending them to one or more display and/or storage channels. The term originated in the nautical field and has been used in IT since the 1960s. Application logging allows us to detect and fix bugs, unhandled exceptions, errors, and vulnerabilities, thus improving the stability and security of our apps. Moreover, it greatly helps us identify abnormal behaviors before they become critical, which can be crucial for business continuity. Furthermore, it’s a requirement for most international IT security regulations, standards, and guidelines. The .NET Framework provides standardized, general-purpose logging APIs through the Microsoft.Extensions.Logging package and the ILogger interface, which allows us to record the events and activities we want to log through a series of built-in and/or third- party logging providers. Most of the .NET built-in providers store log events by using raw strings (unstructured format). Although this logging technique has some benefits, it’s far from ideal, especially for dealing with a large amount of log records. When that’s the case, switching to a structured format is often wise, as it allows us to query those records for relevant info instead of having to parse and/or process them. Adopting a structured logging strategy in .NET is possible thanks to the Azure Application Insights logging provider, available through an external NuGet package maintained by Microsoft. Application Insights works only within the Azure ecosystem, however, which can be a hindrance in terms of accessibility and customization. A great alternative way to implement structured logging in .NET is Serilog, an open source logging library for .NET applications available on NuGet. Serilog can be used to store application logs in several popular DBMSes, as well as third-party services and other destinations. The storage destination(s) can be defined by using a modular architecture built around sinks—sets of output handlers that can be installed and configured independently. Other notable Serilog features include Additional columns, which can be used to add other structured properties to the log event table. Enrichers, which extend the log data with other context info. A powerful message template syntax, which allows us to customize the log messages and properties in a convenient, readable way.
