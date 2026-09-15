---
title: Our first web API project
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 2
---

# 2. Our first web API project

**This chapter covers**

- Reviewing the system requirements
- Choosing and installing the integrated development environment
- (IDE)
- Creating a new ASP.NET web API project
- Configuring the project’s startup and settings files
- Debugging, testing, and improving our first project

In this chapter, we’ll create the MyBGList web API, the cornerstone of the service-oriented architecture concrete scenario that we introduced in chapter 1. More specifically, we’ll put on the shoes of the software development team in charge of creating and setting up the project. We’ll have to make some high-level decisions, such as choosing the integrated development environment (IDE) to adopt and then switch to a more practical approach, such as getting our hands on the source code for the first time, as well as debugging and testing it to ensure that it works as expected.

By the end of the chapter, you’ll be able to test your knowledge by solving some wrap-up exercises on the topics and concepts covered. The main goal is to create a working ASP.NET web API project, which you’ll expand and improve in the following chapters.

## 2.1 System requirements

Because we’ve already chosen to develop our web API with ASP.NET Core, it could be useful to recap the system requirements— what we need to install to start our development journey.

### 2.1.1 .NET SDK

The most important tool to obtain is the .NET software development kit (better known as .NET SDK), which contains the .NET command- line interface (.NET CLI), the .NET libraries, and the three available runtimes:

ASP.NET Core runtime, required to run ASP.NET Core apps Desktop runtime, required to run WPF and Windows Forms apps .NET runtime, which hosts a set of low-level libraries and interfaces required by the preceding two runtimes

NOTE The .NET SDK is available as a standalone package for Windows, Linux, and macOS at the following URL: https://dotnet.microsoft.com/download.

### 2.1.2 Integrated development environment

From a strict point of view, installing the .NET SDK is all we need to do to start building any kind of .NET app, including our ASP.NET Core web API. But writing source code using the built-in tools available in Windows, Linux, or macOS—such as the default text editor and the command-line terminal—is far from ideal. The days when an experienced developer could proudly say that they could do anything by using Notepad, Emacs, or vim are long gone. As a matter of fact, modern IDEs are packed with a ton of useful features that will undeniably boost the productivity of any software developer who’s willing to learn how to take advantage of them productively. I’m not talking only about syntax highlighting, code completion, and other “cosmetic” features. What makes modern IDEs great is their ability to provide tools and extensions that allow the development team to standardize and automate some processes, thus favoring a DevOps- oriented approach: task runners, package managers, code inspectors, integrated source control, automatic deployment system, secure credentials storage, syntax highlighting, and much more.

For all these reasons, because we want to put ourselves in the shoes of the MyBGList software development team, we’re going to build our web API using an IDE (or a code editor with advanced features). Microsoft provides two alternatives:

Visual Studio—Visual Studio is a comprehensive .NET development solution for Windows and macOS that includes compilers, code completion tools, graphical designers, and a lot of useful features to enhance the software development experience and productivity. Visual Studio Code—Visual Studio Code is a lightweight open source code editor for Windows, Linux, and macOS. Unlike Visual Studio, which focuses strongly on .NET development, this product embraces a framework-neutral, language-agnostic approach. But it provides a rich ecosystem of extensions to add support for most development frameworks and programming languages, including ASP.NET Core and C#.

NOTE A lot of non-Microsoft alternatives are worth mentioning, such as Rider by JetBrains and Brackets by Adobe. For the sake of simplicity, however, we’ll restrict our analysis to the Microsoft development tools.

Both Visual Studio and Visual Studio Code are viable for creating, configuring, and maintaining an ASP.NET web API project. Because we want to explore the .NET and ASP.NET Core frameworks to their full extent while we work on our task, however, we’re going to choose Visual Studio. Specifically, we’ll be using Visual Studio 2022, which is the latest version available at the time of this writing.

NOTE If you want to use Visual Studio Code or another editor, don’t worry. All the code samples in this book, as well as all the content available in the GitHub repository, will work in those editors too.

## 2.2 Installing Visual Studio

Visual Studio 2022 is available in three editions, each with a specific set of supported features:

Community Edition has all the required features to create, develop, debug, test, and publish all kinds of .NET apps. This edition is free to use, but it can be used only for open source projects, academic research, and classroom learning, as well as by nonenterprise organizations with no more than five developers. Professional Edition has all the features of Community Edition and has less restrictive paid licensing. Enterprise Edition has all the features of the other two editions, plus some advanced architectural-level design, validation, analysis, and testing tools.
NOTE For the purposes of this book, you’re entitled to download and install Community Edition at the following URL: https://visualstudio.microsoft.com.

The Visual Studio installation process is divided into three phases:

1. Download and install the Visual Studio Installer, which acts as an installation and update-management tool for the whole Visual Studio family.
2. Choose the workloads, individual components, and language packs. Each workload contains a group of components required for the framework, programming language, and platform we want to use. In our scenario, because we want to develop an ASP.NET Core web API, we need to install only the ASP.NET and web development workload (see figure 2.1) without additional individual components—at least for the time being. The language pack depends on how we want to set the language used by the graphical user interface (GUI).
3. Install Visual Studio 2022, along with the selected workloads, components, and language packs.
Figure 2.1 Adding the ASP.NET and web development workload to the Visual Studio installation

NOTE If you’re reading this book for the first time, installing the English language pack might be a good choice so that the IDE commands always match the samples and screenshots.

## 2.3 Creating the web API project

When we have the .NET SDK and Visual Studio 2022 installed on our system, we can start creating our first web API project. Perform the following steps:

1. Launch Visual Studio 2022.
2. Select the Create a New Project option.
3. Use the search box to find the ASP.NET Core web API project template, as shown in figure 2.2.
Figure 2.2 Finding the ASP.NET Core web API project template in Visual Studio

Configuring the template is rather easy. We give our project a name— MyBGList, in our scenario—and accept the other default settings, as shown in figure 2.3. Be sure to select the .NET 6.0 framework, which is the latest version available at the time of this writing.
Figure 2.3 Configuring the ASP.NET Core web API project

WARNING If you want to use a different .NET version, you’re free to do that. Keep in mind, however, that some of the source code samples in this book might require some changes to work with framework updates and/or breaking changes.

As soon as you click the Create button, Visual Studio generates the source code for our new MyBGList project, adds it to a solution with the same name, and opens it in the IDE. We’re all set!

Before continuing, let’s quickly check that everything works. Press the F5 key (or click the Run button in the topmost toolbar) to launch the project in Debug mode. If we did everything correctly, Visual Studio should start our default browser automatically, pointing to https:/ /localhost:<someRandomPort> and showing the page displayed in figure 2.4.
Figure 2.4 MyBGList project first run

As we can see, the Visual Studio web API template that we used to create our project provides a neat start page that mentions Swagger, a convenient tool that describes the structure of our API. I’ll introduce Swagger later in this chapter. Now we’re ready to take a better look at our project.

Before we start coding, it might be worth discussing projects and solutions and the roles they’re meant to play within the Visual Studio ecosystem. In a nutshell, we can say that A project is a group of source-code files that are typically compiled into an executable, library, or website together with some (required) configuration files and a bunch of (optional) content files such as icons, images, and data files. The MyBGList project includes a small set of .cs and .json files generated by Visual Studio to create our ASP.NET web API, using the configuration settings that we choose. A solution is a container that we can use to organize one or more related projects. When we open a solution, Visual Studio automatically opens all the projects that the solution contains. In our scenario, the solution contains only our MyBGList web API project and shares its name.

In the following chapters, we’ll add other projects to our solution. Specifically, we’ll create some class libraries that we’ll want to keep separate from the web API project so we can use them elsewhere. Such an approach increases the reusability of our code while granting us the chance to keep our projects logically grouped and accessible within the same IDE window.

## 2.4 MyBGList project overview

Now that we’ve learned the basics of Visual Studio, let’s spend some valuable time reviewing the autogenerated source code of our brand- new web API project. The default ASP.NET Core web API template provides a minimal yet convenient boilerplate that can be useful for understanding the basic structure of a typical project, which is precisely what we need to do before we start coding our board-game- related web API. Let’s start by taking a look at the project’s file structure. In the Solution Explorer window, we see that our project contains some important files:

launchSettings.json (inside the /Properties/ folder)—Containing the settings for launching the project in development mode appsettings.json—Containing the app’s configuration settings for all the environments, and the nested appsettings.Development.json, containing the settings specific to the development environment only Program.cs—The bootstrap class introduced in chapter 1 WeatherForecastController.cs—A sample controller showing how to serve some dummy weather-forecast data WeatherForecast.cs—A minimal data-transfer object (DTO) class that the controller uses to serve the sample weather forecast JavaScript Object Notation (JSON) data, using a strongly typed approach

In the following section, we’ll briefly review all these files to understand their purposes and the roles they play within the boilerplate. While we perform code review, we’ll also make some small yet significant updates of some of the default behaviors to better suit our needs. Eventually, we’ll replace the boilerplate files related to weather forecasts with our own board-game-themed classes.

### 2.4.1 Reviewing launchSettings.json

The first file to look at is launchSettings.json, located in the /Properties/ folder. As the name suggests, this file contains some configuration settings related to how our project should be launched. It’s important to understand, however, that this file—along with all the settings included—will be used only within the local development machine. In other words, it won’t be deployed with our app when we publish our project on a production server. As we can see by opening it, the configuration settings are split into three main sections (or JSON keys):

"$schema"—Pointing to a URL that describes the schema used by the file "iisSettings"—Containing some base configuration settings for the IIS Express web server "profiles"—Which is split into two subsections: "IIS Express"—Containing other settings for the IIS Express web server "MyBGList"—Containing settings specific to the Kestrel web server

If you don’t know what IIS Express and Kestrel are, let’s quickly recap the backstory. ASP.NET Core provides built-in support for two web servers that can be used for local development purposes:

IIS Express—A lightweight version of the Internet Information Services (IIS) Web Server, available since Windows XP and Visual Studio 2012 Kestrel—An open source, cross-platform, event-driven, asynchronous HTTP server implementation, introduced with the first version of ASP.NET Core

NOTE In Visual Studio 2010 and earlier, the default web server for development purposes was the ASP.NET Development Server, better known as Cassini.

We can choose the web server to use when running our app in Visual Studio by clicking the arrow handler to the right of the Start button. This handler is the button with the green arrow on the Visual Studio top-level toolbar, as shown in figure 2.5.
Figure 2.5 The Visual Studio Start button

The option with the app’s name—MyBGList in our scenario— corresponds to Kestrel. As we can see, we can also choose the web browser to use, as well as some other options that we can skip for now.

The iisSettings and profiles sections of the launchSettings.json file contain the configuration settings for IIS Express and Kestrel. For each server, we can choose the HTTP and HTTPS port to use, the launch URL, the environment variables to set before launching the app, and so on.

As figure 2.5 shows, if we click the Start button (or choose Debug > Start Debugging or press F5) now, we’ll see the SwaggerUI page, which handles the swagger endpoint, configured in the launchUrl option for both browsers. Notice that the SwaggerUI page shown in figure 2.4 appears regardless of the web server we choose because the browsers have been configured to use that endpoint. The only things that visibly change are the local TCP ports used to establish the HTTPS connection because Visual Studio randomly determines them when creating the project. Let’s take the chance to normalize those ports.

NOTE We’ll use the SwaggerUI page and its contents extensively in later chapters while implementing our sample web API. Also, chapter 11 discusses Swagger in depth.

Open the launchSettings.json file, and change its contents as shown in the following listing. The updated lines and values are boldfaced.
**Listing 2.1 Modified launchSettings.json file**

```json
{
    "$schema": "https://json.schemastore.org/launchsettings.json",
    "iisSettings": {
      "windowsAuthentication": false,
      "anonymousAuthentication": true,
      "iisExpress": {
            "applicationUrl": "http://localhost:40080",                        ❶
            "sslPort": 40443                                                   ❶
        }
    },
    "profiles": {
       "MyBGList": {
         "commandName": "Project",
         "dotnetRunMessages": true,
         "launchBrowser": true,
            "launchUrl": "swagger",                                             ❷
            "applicationUrl": "https://localhost:40443;http://localhost:40080", ❸
            "environmentVariables": {                                           ❹
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        },
        "IIS Express": {
           "commandName": "IISExpress",
           "launchBrowser": true,
            "launchUrl": "swagger",                                            ❺
            "environmentVariables": {                                          ❻
              "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    }
}
```

❶ IIS Express local URLs and TCP ports to use for HTTP and HTTPS
❷ Kestrel local URLs and TCP ports to use for HTTP and HTTPS
❸ Kestrel starting page
❹ Kestrel environment variables
❺ IIS Express starting page
❻ IIS Express environment variables

As we can see, we’ve set the 40080 (HTTP) and 40443 (HTTPS) TCP ports for both IIS Express and Kestrel. This simple tweak ensures that the URLs referenced in this book will work properly within our local codebase, regardless of the web server we want to use.

The rest of the settings specified in the built-in launchSettings.json file are good enough for the time being, so we can leave them as they are. Before closing the file, however, let’s take a good look at the "environmentVariables" sections, which contain a single ASPNETCORE_ENVIRONMENT environment variable for both IIS Express and Kestrel.

### 2.4.2 Configuring the appsettings.json

Let’s move to the appsettings.json files, which store the application configuration settings in JSON key-value pairs. If we look at the autogenerated code of our MyBGList web API project, we can see that Visual Studio created two instances of the file:

appsettings.json appsettings.Development.json

Before seeing how these files are meant to work, it could be useful to explore the concept of runtime environments in ASP.NET Core.

Runtime environments Web application development typically involves at least three main phases: Development, in which software developers perform their debug sessions Staging, in which a selected group of users (or testers) performs internal and/or external tests Production, in which the app is made available to end users

These phases, by .NET conventions, are called environments, and they can be set using the DOTNET_ENVIRONMENT and/or ASPNETCORE_ENVIRONMENT environment variables in the app’s execution context. Whenever we launch our application, we can choose which runtime environment to target by setting that environment variable accordingly.

TIP If we remember the "environmentVariables" section of the launchSettings.json file, we already know how the ASPNETCORE_ENVIRONMENT variable can be set in our local development machine. We’ll learn how to do that in a production server in chapter 12, when we deploy our web API.

The appsettings files Now that we know all that, we can easily understand the purpose of those two appsettings files:

appsettings.json is meant to store configuration settings that will be used by all the runtime environments unless they’re overridden—or complemented by—specific environment-specific files. appsettings.Development.json is one of those files. The settings put there will be used by the development environment only (and ignored by any other environment), overriding and/or integrating the settings present in the appsettings.json “generic” file.

WARNING The environment-specific file will be read after the generic version, thus overriding any key-value pair present there. In other words, if we run our app using the development environment, every key-value pair present in the appsettings.Development.json will be added to the key-value pairs present in the appsettings.json files, replacing them if they’re already set.

If we look inside those two appsettings files, we’ll see a bunch of log-related settings (within the Logging JSON key), which we can ignore for now. We’ll have the chance to play with them in chapter 7 when we talk about logging techniques. What we can do now instead is add a new key/value pair that can help us later. Open the appsettings.json file, and add the following line (in bold) to the existing JSON:

```csharp
{
    "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
    },
    "AllowedHosts": "*",
    "UseDeveloperExceptionPage": false
}
```

This new configuration setting will give us some practice with the appsettings file(s) and also allow us to switch between implementation techniques that we’ll put in place in a short while.

### 2.4.3 Playing with the Program.cs file

Let’s move to the Program.cs file, which we saw briefly in chapter
1. We already know that this file is executed at the start of the application to register and configure the required services and middleware to handle the HTTP request-and-response pipeline.

As a matter of fact, the default Program.cs file created by the ASP.NET web API template is identical to what we saw in chapter 1, so we won’t find anything new there. By briefly reviewing it, we clearly see the services and middleware that our web API is meant to use, as shown in the following listing.

**Listing 2.2 Program.cs file**

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();              ❶
// Learn more about configuring Swagger/OpenAPI at
https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();    ❷
builder.Services.AddSwaggerGen();              ❷

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                          ❷
    app.UseSwaggerUI();                        ❷
}

app.UseHttpsRedirection();                     ❸
app.UseAuthorization();                   ❹

app.MapControllers();                     ❺

app.Run();
```

❶ Controllersservice and middleware
❷ Swagger services and middleware
❸ HTTP to HTTPS redirection middleware
❹ ASP.NET Core authorization middleware
❺ Controllersservice and middleware

While we’re here, let’s take the chance to add some useful middleware that will help us handle errors and exceptions better.

Exception handling Locate the following code within the Program.cs file:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

Replace it with the following (changes marked in bold):

```csharp
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
     app.UseDeveloperExceptionPage();
}
else
{
     app.UseExceptionHandler("/error");
}
```

As we can see, we’ve added new middleware to the HTTP pipeline. The first addition will be included only if the app is run in the development environment; the second addition will be present in only the staging and production environments. Here’s what this middleware does in detail:

DeveloperExceptionPageMiddleware—As its name implies, this middleware captures synchronous and asynchronous exceptions from the HTTP pipeline and generates an HTML error page (the developer exception page), which contains useful information regarding the exception, such as stack trace, query string parameters, cookies, and headers. This information might expose configuration settings or vulnerabilities to a potential attacker. For that reason, we’re using it only in the development environment, so that this useful, yet potentially harmful information will be available for the developer’s eyes only. ExceptionHandlingMiddleware—This middleware also handles HTTP-level exceptions but is better suited to nondevelopment environments, because it sends all the relevant error info to a customizable handler instead of generating a detailed error response and automatically presenting it to the end user.

Now, although the DeveloperExceptionPageMiddleware works straight out of the box and doesn’t require any additional work, the ExceptionHandlingMiddleware requires us to implement a dedicated handler. As we can see by looking at our code, we’ve passed the /error string parameter, meaning that we want to handle these errors with a dedicated HTTP route, which we need to implement.

As we already know from chapter 1, we have two ways to do that: with a Controller or with the Minimal API. Let’s see both of them and then pick the most effective one.

Using a Controller Let’s start with the Controller-based approach. From Visual Studio’s Solution Explorer, perform the following steps:

1. Right-click the Controllers folder of our MyBGList project, and choose Add > Controller. A pop-up window opens, asking us to select the controller we want to add.
2. Navigate to the Common > API node in the tree view on the left, select the API Controller - Empty option, and click the Add button.
3. Name the new controller ErrorController.cs, and click OK to create it.

We’ll see our new ErrorController.cs file’s content: an empty class that we can use to add our action methods—specifically, the action method that we need to handle the /error/ route where our ExceptionHandlingMiddleware will forward the HTTP errors. Here’s a minimal implementation of the action method we need:

using Microsoft.AspNetCore.Mvc;

```csharp
namespace MyBGList.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
         [Route("/error")]              ❶
         [HttpGet]                      ❷
         public IActionResult Error()
         {
             return Problem();          ❸
         }
    }
}
```

❶ The HTTP route to handle
❷ The HTTP method to handle
❸ The HTTP response to return to the caller

The Problem() method that we’re returning is a method of the ControllerBase class (which our ErrorController extends) that produces a ProblemDetail response—a machine- readable standardized format for specifying errors in HTTP API responses based on RFC 7807 (https://tools.ietf.org/html/rfc7807). In a nutshell, it’s a JSON file containing some useful info regarding the error: title, detail, status, and so on. Because we’re calling the Problem() method without specifying any parameter, however, these values will be set automatically by ASP.NET Core, using default values taken from the exception that has been thrown.

Using Minimal API Let’s see how we can pull off the same outcome by using Minimal APIs. In Visual Studio’s Solution Explorer, open the Program.cs file, and add the following code right before the app.MapControllers() method:

app.MapGet("/error", () => Results.Problem()); That’s it. As a matter of fact, Minimal API seems to be the clear winner of this match, because it allows us to achieve the same result as our ErrorController with a one-liner without having to create a dedicated file. This outcome shouldn’t be a surprise: This scenario is a perfect example of the dead-simple routing actions in which Minimal API shines, whereas Controllers are better suited to complex tasks.

In the following chapters, we’ll see a lot of scenarios in which the Controller-based approach will take its revenge. For the time being, we may as well delete the ErrorController.cs file and keep the Minimal API one-liner in the Program.cs file. First, though, let’s spend a couple of minutes discussing what might happen if we keep ErrorController.cs in place.

Routing conflicts Controllers and Minimal API can exist in the same project without problems, so the developer can get the best of both worlds. But they should be configured to handle different routes. What happens if they share an endpoint?

If we remember what we learned in chapter 1, we already know the answer: the middleware that comes first in the Program.cs file handles the HTTP request first and likely terminates it, thus preventing the other from coming into play. Such behavior is perfectly fine and won’t cause any significant problem in the HTTP lifecycle, aside from the waste of having a useless implementation within our project’s codebase. In our current scenario, because we put the Minimal API’s app.MapGet() method right before the app.MapControllers() method, the “dead code” victim would be our ErrorController.cs file. If everything is fine, why should we delete that controller? Can’t we just leave it there?

The best way to answer that question is to press F5 again and execute our app before deleting the ErrorController.cs file.
Figure 2.6 shows what we should get.
Figure 2.6 SwaggerUI error 500 (due to routing conflicts)

As we can see, the previously working SwaggerUI page displays a fetch error, due to the fact that its data source (the autogenerated swagger.json file used internally to build the UI) is returning an HTTP 500 error. If we copy that URL (https:/ /localhost:40443/ swagger/v1/swagger.json) and paste it in our web browser’s address bar, we can see the actual error:

SwaggerGeneratorException: Conflicting method/path combination "GET error" for actions - MyBGList.Controllers.ErrorController.Error (MyBGList),HTTP: GET /error. Actions require a unique method/path combination for Swagger/OpenAPI 3.0. Use ConflictingActionsResolver as a workaround.

This error message brings us to the root of the problem: we have two handlers for the same method/path combination (GET /error), which prevents Swagger from working properly. To fix the problem, we can do one of two things:

Remove one of the “duplicate” handlers (the Controller’s Error() action or the Minimal API’s MapGet() method). Set up a ConflictingActionsResolver to instruct Swagger how to handle duplicate handlers.

In this scenario, deleting the ErrorController.cs file or removing it from the project is the better thing to do, because we don’t want to keep redundant code anyway. But in case we wanted to keep it for some reason, we could instruct Swagger to deal with the situation by changing the SwaggerGeneratorMiddleware configuration within the Program.cs file in the following way (updated code marked in bold):

```csharp
builder.Services.AddSwaggerGen(opts =>
    opts.ResolveConflictingActions(apiDesc =>   apiDesc.First())
    );
```

We’re telling Swagger to resolve all conflicts related to duplicate routing handlers by always taking the first one found and ignoring the others. This approach is highly discouraged, however, because it can hide potential routing problems and lead to unexpected results. No wonder the error message calls it a workaround!

WARNING In more general terms, always address routing conflicts by removing the redundant (or wrong) action handler. Setting up the framework (or the middleware) to automatically “resolve” conflicts is almost always bad practice unless the developers are experienced enough to know what they’re doing.

For that reason, the best thing we can do before proceeding is delete the ErrorController.cs file or exclude it from the project (right-click it and then choose Exclude from Project from the contextual menu in Solution Explorer) so that Swagger won’t have the chance to find any dupes.

Testing it Now that we’ve configured the developer exception page for the development environment and the Error() action for the production environment, we need to emulate an actual error. The quickest way is to add another action method (or Minimal API) that throws an exception. If we still had our ErrorController, we could implement the action method in the following way:

```csharp
[Route("/error/test")]
[HttpGet]
public IActionResult Test()
{
    throw new Exception("test");
}
```

Because we chose to delete or exclude the ErrorController from the project, however, we can put the following Minimal API one- liner in the Program.cs file right below the other MapGet() method that we added:

app.MapGet("/error/test", () => { throw new Exception("test"); });

Then click the Start button (or press the F5 key) and point the web browser to https://localhost:40443/error/test. If we did everything correctly, we should see the developer exception page generated by DeveloperExceptionPageMiddleware, as shown in figure 2.7.
Figure 2.7 Testing DeveloperExceptionPageMiddleware

This outcome is expected because we’re executing our app in the development environment, as specified by the ASPNETCORE_ENVIRONMENT variable in the launchSettings .json file. If we want to test ExceptionHandlerMiddleware, all we need to do is to change the value of the variable from Development to Production.

Alternatively, we can make good use of the UseDeveloperExceptionPage key that we added to our appsettings.json file. Implementing this setting will allow us to switch between the developer exception page and the ExceptionHandler without having to change the runtime environment of our app. Open the Program.cs file, and replace the code

```csharp
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
     app.UseDeveloperExceptionPage();
}
else
{
     app.UseExceptionHandler("/error");
}
```

with this code:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))   ❶
    app.UseDeveloperExceptionPage();                                 ❷
else
    app.UseExceptionHandler("/error");                               ❸
❶ Retrieves that literal value from the appsettings.json file(s)
❷ If TRUE, uses the DeveloperExceptionPageMiddleware
❸ If FALSE, uses the ExceptionHandlerMiddleware instead

Now ExceptionHandlerMiddleware will be used instead of DeveloperExceptionPageMiddleware, because the value of the UseDeveloperExceptionPage key was set to false in the appsetting.json file. We can immediately press F5, navigate to the https://localhost:40443/error/test URL, and receive the ProblemDetail JSON response in all its glory:

{ "type":"https://tools.ietf.org/html/rfc7231#section-6.6.1", "title":"An error occurred while processing your request.", "status":500 }

NOTE This JSON output is viable enough in our scenario (for now) because it doesn’t expose potentially exploitable info regarding our app. We can further customize the output. We could replace the generic “an error occurred” title with the actual Exception message, provide different status codes for different kinds of errors, and so on, using the optional parameters supported by the method’s overloads.

Now that we’ve completed this series of tests, we should reenable DeveloperExceptionPageMiddleware for the development environment. We could open the appsettings .json file and change the UseDeveloperExceptionPage value from false to true, but that wouldn’t be the right thing to do. We want to be sure that such a potentially insecure page could be seen only by developers, remember? For that reason, the proper way to reenable it is to perform the following steps:
1. Open the appsettings.Development.json file.

2. Add a UseDeveloperExceptionPage key (which doesn’t exist in this file).
3. Set the key’s value to true.

Here’s what the updated file will look like (new line in bold):

```csharp
{
    "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
    },
    "UseDeveloperExceptionPage": true
}
```

Now the value we put in the appsettings.Development.json file will overwrite the value in the appsettings.json file whenever our app is launched in the development runtime environment—which is precisely what we want.

### 2.4.4 Inspecting the WeatherForecastController

The next file to look at is WeatherForecastController.cs, which we can find in the /Controllers/ folder.

NOTE By ASP.NET convention, all controller classes must reside in the project’s root-level /Controllers/ folder and inherit from the Microsoft.AspNetCore.Mvc.Controller base class. Controllers, as we know from chapter 1, are used in ASP.NET Core to define and group actions that handle HTTP requests (mapped through routing) and return HTTP responses accordingly. If we look at the source code of the WeatherForecastController, we can see that it makes no exception. This sample Controller is meant to handle an HTTP GET request to the /WeatherForecast route and return an HTTP response containing an array of five JSON objects containing some randomly generated Date, TemperatureC, and Summary property values:

using Microsoft.AspNetCore.Mvc;

```csharp
namespace MyBGList.Controllers
{
   [ApiController]                                                     ❶
   [Route("[controller]")]                                             ❷
   public class WeatherForecastController : ControllerBase
   {
          private static readonly string[] Summaries = new[] {
           "Freezing", "Bracing", "Chilly", "Cool",
           "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
       };
```

        private readonly ILogger<WeatherForecastController> _logger;   ❸

```csharp
        public WeatherForecastController
            (ILogger<WeatherForecastController> logger)
        {
            _logger = logger;                                          ❸
        }
```

```csharp
        [HttpGet(Name = "GetWeatherForecast")]                         ❹
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
           }
      }
}
```

❶ Adds API-specific behaviors
❷ Default routing rules
❸ ILogger instance (instantiated through Dependency Injection)
❹ Action to handle HTTP GET to /WeatherForecast

Here’s what we get if we try to execute this code:

[{ date: "2021-12-03T02:04:31.5766653+01:00", temperatureC: 0, temperatureF: 32, summary: "Warm" }, { date: "2021-12-04T02:04:31.5770138+01:00", temperatureC: 23, temperatureF: 73, summary: "Freezing" }, { date: "2021-12-05T02:04:31.5770175+01:00", temperatureC: 40, temperatureF: 103, summary: "Freezing" }, { date: "2021-12-06T02:04:31.5770178+01:00", temperatureC: 47, temperatureF: 116, summary: "Cool" }, { date: "2021-12-07T02:04:31.577018+01:00", temperatureC: 36, temperatureF: 96, summary: "Mild" }]

The returned object is a JSON representation of the C# WeatherForecast class, defined in the WeatherForecast.cs file in our project’s root folder. As we can see by looking at its source code, it’s a POCO class containing some properties that can easily be serialized into JSON output:

```csharp
namespace MyBGList
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
```

public int TemperatureC { get; set; }

public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

```csharp
        public string? Summary { get; set; }
    }
}
```

NOTE POCO stands for Plain Old CLR Object—in other words, a normal class without dependencies, attributes, infrastructure concerns, special types, or other responsibilities.

The WeatherForecastController and WeatherForecast sample classes can be useful for understanding how an ASP.NET controller works, but they don’t fit well in our concrete scenario. We don’t need to know anything about temperatures or forecasts when dealing with a board-game-related API. For that reason, we’re going to delete those files or exclude them from the project, as we did with the ErrorController.cs file earlier, and replace them with a more pertinent sample.

### 2.4.5 Adding the BoardGameController

Let’s start with the POCO class that will take the place of the previous WeatherForecast. In Visual Studio’s Solution Explorer, perform the following steps (listing 2.3):

1. Delete the existing WeatherForecast.cs file.

2. Right-click the MyBGList project’s root folder, choose Add > New Item from the contextual menu, and create a new BoardGame.cs class file.
3. Fill the new file with a POCO class hosting some board-game data.

**Listing 2.3 BoardGame.cs file**

```csharp
namespace MyBGList
{
    public class BoardGame
    {
        public int Id { get; set; }

         public string? Name { get; set; }

         public int? Year { get; set; }
     }
}
```

This new class is still a sample, yet it’s more consistent with our chosen scenario! Let’s do the same with the controller. In Visual Studio’s Solution Explorer, perform the following steps (listing 2.4):

1. Navigate to the /Controllers/ folder, and delete the existing WeatherForecastController.cs file.

2. Right-click the /Controllers/ folder, choose Add > Controller from the contextual menu, create a new API Controller - Empty, and name the new file BoardGamesController.cs.
3. Remove the /api/ prefix, because we don’t need it.
4. Add a new action method to return an array of board-game data, using the BoardGame POCO class we created.

**Listing 2.4 BoardGamesController.cs file**

```csharp
using Microsoft.AspNetCore.Mvc;

namespace MyBGList.Controllers
{
     [Route("[controller]")]                   ❶
     [ApiController]
     public class BoardGamesController : ControllerBase
     {
         private readonly ILogger<BoardGamesController> _logger;

         public BoardGamesController(ILogger<BoardGamesController> logger)
         {
             _logger = logger;
         }

         [HttpGet(Name = "GetBoardGames")]     ❷
         public IEnumerable<BoardGame> Get()
         {
             return new[] {
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
             };
         }
     }
}
```

❶ Updated Route pattern
❷ New Get method
That’s it. Our new BoardGamesController will handle the /BoardGames route and respond with a JSON array containing some relevant sample info on three highly acclaimed board games released in the past 45 years or so.

The minimal behavior of our BoardGamesController could be easily handled by Minimal API with a few lines of code. Here’s a code snippet that we can put in the Program.cs file to obtain the same output from the Get() action method:

app.MapGet("/BoardGames", () => new[] { new BoardGame() { Id = 1, Name = "Axis & Allies", Year = 1981 }, new BoardGame() { Id = 2, Name = "Citadels", Year = 2000 }, new BoardGame() { Id = 3, Name = "Terraforming Mars", Year = 2016 } });

This example is only a sample JSON response that mimics more complex behavior, which often includes nontrivial data retrieval. In the following chapters, we’re going to get the board-game data from a database management system (DBMS) using Entity Framework Core and possibly even update it for users. When we’re dealing with these kinds of actions, the controller-based approach becomes convenient, possibly even more convenient than Minimal API. For that reason, this time we’re going to keep the controller instead of replacing it.

## 2.5 Exercises

The best way to build confidence with ASP.NET and Visual Studio is to practice using various tools as soon as we learn how they work. This section provides some useful exercises that allow us to further customize our first web API project, using the skills we learned in this chapter. Each exercise is meant to modify a single file, but by completing all the exercises, we will be able to achieve a consistent overall goal.

Suppose that we need to configure our new MyBGList web API for a selected group of internal testers, which will be able to access our development machine through a given set of TCP ports. Here’s the full backlog of the specifics we need to ensure:

1. Testers can use only the 55221 and 55222 TCP ports.
2. Testers can only use the Kestrel web server.
3. The web browser used to test the app should start with the JSON list of the BoardGames returned by the BoardGamesController’s Get() action method.
4. Testers, like developers, must be allowed to access the SwaggerUI page but not the developer exception page, which should be unavailable to them.
5. Testers need to retrieve two additional fields for each board game: MinPlayers and MaxPlayers. These fields should contain the number of minimum and maximum players supported by the board game.
TIP If you’re feeling bold, stop reading here, and start the exercise without further help (hard mode). If you’re less confident about what you’ve learned so far, you can read the following sections, which provide general guidance on all the relevant steps without giving the solution away (relaxed mode). The solutions to all the given exercises are available on GitHub in the /Chapter_02/Exercises/ folder. To test them, replace the relevant files in your MyBGList project with those in that folder, and run the app.

### 2.5.1 launchSettings.json

The first thing we need to do is ensure that testers will be able to access the local machine through the given TCP ports. It would be wise to set up a dedicated runtime environment for them to use. The staging environment seems to be the perfect choice because it allows us to define some specific configuration settings without altering the configuration for the production and development environments, which we’re likely going to need. We can perform these tasks by updating the launchSettings.json file and configuring the Kestrel launch settings for the MyBGList project in the following way:

1. Use TCP port 55221 for HTTP and 55222 for HTTPS.
2. Set the runtime environment to Staging.
3. Set the starting endpoint URL to the route handled by the BoardGamesController’s Get() action method so that the web browser will automatically show that page when the app starts. We don’t need to change the settings for IIS Express because testers won’t be using it. These tasks fulfill items 1, 2, and 3 in our backlog.

### 2.5.2 appsettings.json

The next things to do are to create the settings file for the staging runtime environment and define some default behaviors, valid for all the environments, that we can override conditionally whenever we need to. Here’s how we can pull off all those tasks:

1. Add a new UseSwagger configuration setting to the MyBGList app settings, valid for all the runtime environments, with the value of False.

2. Add that same UseSwagger configuration setting in the existing configuration file to the development environment, with the value of True.

3. Create a new configuration file for the staging environment, and override the settings as follows:

UseDeveloperExceptionPage: false UseSwagger: true

These tasks won’t affect anything yet, but they fulfill item 4 of our specifications.

TIP Setting the UseDeveloperExceptionPage to false for the staging environment could be redundant because that value is already set in the generic appsettings.json file. Because we’re talking about a page containing potentially confidential information, however, explicitly denying access in a given environment won’t hurt.

### 2.5.3 Program.cs

Now that we have the proper app setting variables, we can use them to conditionally add (or skip) the relevant middleware, depending on the app’s runtime environment. We need to open the Program.cs file and change the current initialization strategy of SwaggerMiddleware and SwaggerUIMiddleware to ensure that they’ll be used only if UseSwagger is set to True. By doing that, we fulfill item 4 of our backlog.

### 2.5.4 BoardGame.cs

To implement item 5 of our backlog, we need to add two new properties to the existing BoardGame POCO class. As for the type to use, the most suitable choice is a nullable int, as we used for the Year property; we can’t be sure that such info will always be available for all board games.

### 2.5.5 BoardGameControllers.cs

Adding these properties to the BoardGame class won’t be enough to show them properly in the JSON file unless we want them always to be null. Because we’re currently dealing with sample data, the only thing we can do is update our BoardGameController’s Get() method and set fixed values manually. This task is enough to fulfill item 5 of our backlog and complete the exercise.

All we have left to do is select Kestrel as the startup web server, click the Start button (or press F5) to launch our web API project, and see what happens. If we did everything correctly, our web browser should automatically call the https://localhost :55221/boardgames endpoint and show the following JSON response:

[{ "id":1, "name":"Axis & Allies", "year":1981, "minPlayers":2, "maxPlayers":5 }, { "id":2, "name":"Citadels", "year":2000, "minPlayers":2, "maxPlayers":8 }, { "id":3, "name":"Terraforming Mars", "year":2016, "minPlayers":1, "maxPlayers":5 }]

If we achieved that result, we’re ready to move on.

Summary To create ASP.NET Core apps, we need to download and install the .NET Core SDK, the .NET Core runtime, and the ASP.NET Core runtime (unless you chose an IDE that automatically does those things, such as Visual Studio). We should also provide ourselves with a suitable IDE such as Visual Studio: a comprehensive .NET development solution for Windows and macOS that we’ll use throughout this book. Visual Studio can dramatically boost our productivity and help us standardize our development processes thanks to built-in features such as task runners, package managers, integrated source control, and syntax highlighting. Visual Studio includes a lot of useful templates that we can use as boilerplate for creating our apps, including the ASP.NET Core web API template, which is the perfect choice to start our MyBGList web API project. Visual Studio’s ASP.NET Core web API template comes with a small group of autogenerated files: A startup file to set up services and middleware (Program.cs)

A settings file to launch the app with the development web server (launchSettings.json)

A set of configuration files that store the app’s environment- specific settings (appsettings.json)

A POCO class to emulate a data object (WeatherForecast.cs)

A Controller class that can respond to simple HTTP requests with some sample JSON data (WeatherForecastController.cs) After a brief review of the template files and some minor code changes to understand how each of them works, we can start replacing the built-in weather forecast sample classes with some board-game-related classes. Before going further, it may be useful to test our acquired knowledge with some exercises, simulating a series of requests (backlog items) from the client. Depending on our confidence, we can try to fulfill them without any suggestions or follow some high-level guidance. Regardless of the chosen complexity level, performing such exercises is a great way to test our current skills and prepare for the topics yet to come.
