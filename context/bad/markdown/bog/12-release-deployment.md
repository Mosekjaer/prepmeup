---
title: Release and deployment
source: Building Web APIs with ASP.NET Core (De Sanctis, Manning)
chapter: 12
---

# 12. Release and deployment

**This chapter covers**

- Getting the web API ready for release
- Adopting a security by design approach
- Setting up a content-delivery network (CDN)
- Creating a Windows Server virtual machine (VM) in Microsoft
- Azure
- Installing and configuring Internet Information Services (IIS)
- Deploying the web API to Windows Server with Visual Studio

We’re close to the end of our journey. Our ASP.NET Core web API project is mature enough to be classified as a potentially shippable product (PSP), a definition used by most Agile methodologies to indicate that the application, although still incomplete, has reached a level of maturity solid enough to be shown to product owners, beta testers, and/or end users.

It’s important to understand that this status doesn’t mean the app is ready to be delivered to customers. Potentially shippable is a state of confidence from a development perspective; actual shipping is a business decision that should take other data into account.

In this chapter, we’ll suppose that the IT project manager assigned to our development team, after carefully reviewing the results of our work, has decided that our web API is ready for its first release. To comply with this decision, we need to publish it somewhere to enable the interested parties—ideally, product owners and testers—to access and evaluate it.

After a brief introductory part, where we’ll perform some required steps to ensure that our ASP.NET Core application is ready for the deployment phase, we’ll learn how to publish our project to a Windows Server machine. ASP.NET Core is platform agnostic, so although we’ll target a Windows OS, we could use the same process, with minor differences, to deploy our app on a Linux machine, a Docker container, or any other publishing target.

WARNING The topics covered in this chapter concern different skills from those required in the previous chapters. Most of the tasks are typically assigned to deployment engineers and/or system administrators, not to software—at least, in large organizations. But being able to release, deploy, and publish a project is a required learning path for anyone who aims to become a full-stack developer or to work on a small, DevOps-oriented team—a rather common scenario for small companies and startups.

## 12.1 Prepublishing tasks

In this section, we’ll learn how to get our ASP.NET Core web API ready to publish in a production environment. Here’s what we’re about to do:

1. Define the overall security approach that will guide us through implementation of the various required tasks.
2. Determine the domain name that we’ll use to make our app publicly available on the World Wide Web.
3. Set up a content-delivery network (CDN) to achieve Transport Layer Security (TLS) and caching capabilities, as well as protect our web API from distributed denial of service (DDoS) attempts, brute-force attacks, and other web-related threats.
4. Configure our project to make it ready for production.

We’ll also review our current caching and security settings to ensure that the app is ready to be accessed by external users (including potentially malicious parties) while minimizing the risks of request- based attacks, data breaches, and other harmful scenarios.

### 12.1.1 Considering security

The depth and scope of the prepublishing review we’re about to perform are strictly determined by our scenario. We’re bringing into production a (yet) unknown web API, which is meant to be accessed by a limited panel of users and beta testers, and which doesn’t contain restricted or sensitive data. Based on these premises, we can reasonably expect that we won’t be subject to sophisticated attacks, at least not for some time.

This expectation, however, must not lead us to take the risk of attack lightly. According to the 2022 Imperva Bad Bot Report (http://mng.bz/v1Dx), 27.7 percent of internet traffic is made up of automated threats. These bots can be instructed to attack specific websites, as well as to scan the internet for potentially vulnerable IP addresses and/or TCP ports. When a suitable target is found, these bots will likely carry out some automated attacks on it and signal the outcome to their “master”—a person or organization that might orchestrate even worse malicious activities. For that reason, whenever we’re commissioned to bring one of our web-based projects into production, we should adopt a security by design approach: to take the most secure route to fulfill each of our tasks as long as we maintain a decent cost/benefit (and/or risk) tradeoff. Now that we’ve defined our overall security approach, let’s deal with the first of our prepublishing tasks: obtaining a suitable domain name.

### 12.1.2 Choosing a domain name

In information technology, a domain name is a string of text that uniquely maps to a numeric Internet Protocol (IP) address, which can be used to access a web resource (typically, a website). We can think of it as a human-readable alias of the IP address we want to connect to, which is also machine-readable and understood by all web browsers and web servers.

NOTE For reasons of space, I’ll take for granted that you know most of the basic networking and TCP/IP-related concepts that are used throughout this section. If not, I strongly suggest improving your knowledge by reading HTTP/2 in Action, by Barry Pollard (Manning; https://www.manning.com/books/http2-in-action).

For deployment purposes, we’ll register a dedicated mybglist.com domain so that we’ll be able to create custom subdomains and use them to create public endpoints for our users. I won’t guide you through the domain-purchase process; I take for granted that you’ll complete it before continuing to the next section. If you don’t want to purchase a dedicated domain, you’re free to use a subdomain and follow the same instructions, replacing the subdomain we’re going to use in our deployment sample (win- 01.mybglist.com) with your chosen subdomain.

### 12.1.3 Setting up a CDN

Now that we’ve determined our domain name, we have another important decision to make: whether to set up a direct binding to the IP address of our web server or set up an intermediate layer (such as a CDN service). Each option has advantages and drawbacks. Configuring a CDN will inevitably require additional configuration efforts and will complicate our overall architecture, but it will also grant us valuable caching and security benefits.

TIP In the unlikely case that you don’t want (or are unable) to set up a CDN, you can skip this section and go to the next one.

Based on what we learned in chapter 8, which was about intermediate caching, we already know that adding a CDN service to our network stack will increase the performance of our web API. But we should take into account some relevant security benefits, assuming that we want to stick to the security-by-design approach that we chose to adopt. Those benefits may vary depending on the CDN service and/or the subscription plan, but some of them are usually available, such as the following:

IP address masking—The CDN acts as a proxy, hiding the IP address of the origin server (the web server hosting our web API). All requests go to the CDN first and then are forwarded to our origin server. Web Application Firewall (WAF)—CDNs typically use a dedicated WAF that checks the incoming HTTP requests and filters potentially malicious traffic based on a given set of rules, blacklists, and other industry-standard security measures. DDoS protection—Most CDNs automatically detect and mitigate DDoS attacks by using rate-limiting rules and other advanced techniques.

These features alone, which would be difficult (and costly) for any software development team to implement, are more than enough to make the CDN worth the effort. For that reason, we’re going to set it up. Before choosing a provider, however, let’s consider the result we want to achieve.

Using the edge-origin pattern In chapter 3, which introduced CDNs, I briefly mentioned the edge- origin architectural pattern, an infrastructure in which a proxy or CDN service (the edge) publicly serves the content by taking it from a source web server (the origin) that isn’t exposed directly. Figure 12.1 illustrates this concept.
Figure 12.1 Edge-origin architectural pattern

As we can see, the end user points the browser to our web API’s public (sub)domain, which is handled by the CDN service (edge). The CDN serves the content back, taking it from its intermediate cache or requesting it from the web server (origin) using its dedicated (and not exposed) IP address.

That process is precisely what we want to implement for our web API. With that in mind, let’s start by choosing a CDN service provider.

Choosing a CDN service provider In general terms, there are four main features to look for when choosing a CDN service provider:

Capability—A good CDN should have a decent network size (number of servers) and distribution (worldwide coverage). More servers will likely lead to less buffering, greater redundancy, and more scalability, and a good geographic distribution will ensure the same level of services for all members of our potential audience, regardless of where they’re connecting from. Performance—Performance is often related to (and mostly dependent on) capability. The most important key metrics to measure include Domain Name System (DNS) response time, connect time, wait time, overall latency, packet loss rate, and bandwidth throughput. Reliability—When it’s set up, the CDN becomes the only gateway of our web application. If something bad happens to it or to its configuration settings, our users will be unable to connect. For that reason, we need to ensure that the service guarantees near- 100 percent uptime/availability and good (and fast) customer support. Pricing—A good CDN should offer an affordable, transparent pricing model. Most CDNs use modular subscription plans based on custom negotiated contracts, depending on the customer’s bandwidth and feature needs. Another common model relies on use, with a per-GB pricing that typically goes down as volume rises. Some CDNs even offer free plans for small-scale projects. Any of these approaches can be viable, depending on specific needs and—most important—the expected (or known) traffic volume of the web application.

Because we have a small-scale project with a limited budget, we should choose a CDN provider with a great cost-benefit ratio. Starting from this premise, our choice will be Cloudflare, an extremely popular CDN service provided by Cloudflare, Inc. since 2010. According to a W3Techs.com survey (http://mng.bz/41EB), Cloudflare CDN is used by 19.1 percent of websites worldwide, with a CDN market share of 79.7 percent. This popularity is easily explained by the fact that Cloudflare is one of the few CDN services to offer an unlimited and unrestricted free plan, which makes it perfect for our needs. Another great reason to choose Cloudflare is the fact that it provides two free SSL/TLS certificates: The edge certificate—The edge certificate will be generated automatically and used by the CDN itself when serving the content to our visitors, ensuring full HTTPS support for our web API. The origin certificate—We can manually generate and install the origin certificate on the web server(s) hosting the web API to ensure an encrypted connection between the CDN (edge) and the host (origin) server.

In the following sections, we’ll learn how to create a Cloudflare account and set up this service.

Creating a Cloudflare account The first thing we need to do is create a Cloudflare account. I won’t go into details here, because the registration process is straightforward. At the end of it, perform the login by using the credentials (email and password) that we set up. Then click the Add Site button (figure 12.2), which takes us to a page where we can add our domain.
Figure 12.2 Adding a domain to Cloudflare

We can’t add a subdomain here, however. If we plan to use a subdomain of an existing domain instead of a dedicated domain, we still need to type the “parent” domain in this text box unless it’s already configured with Cloudflare. We’ll be given the chance to choose domain configuration options later. Right after we add the domain and click the Add Site button, we’ll be prompted to select a subscription plan (figure 12.3).
Figure 12.3 Choosing a Cloudflare subscription plan

The paid plans (Pro, Business, and Enterprise) are stuffed with interesting features. But because we’re aiming for a budget-friendly solution, we’ll start with the free plan, which guarantees the core features we require (at least for the time being).

The free plan requires us to configure Cloudflare as the primary DNS provider for our domain, so we’ll use the Cloudflare nameservers and manage our DNS records on Cloudflare. This configuration option is known as Full Setup. In case we want to use a subdomain of an existing domain that we don’t want to move to Cloudflare, we should consider purchasing the Business or Enterprise plan instead. These plans include the CNAME set-up feature, also known as Partial Setup. We’re going to choose the Full Setup option, which allows us to select the free plan.

TIP For more information about the Full Setup and Partial (CNAME) Setup configuration options, see http://mng.bz/Q86m and http://mng.bz/X5yY.

After we click Continue, Cloudflare performs a scan of the existing DNS records of our domain (which will take a few seconds). These records will be used to populate a DNS management panel. Cloudflare will try to copy (or clone) the current DNS configuration of our domain to get ready to handle it by using its own nameservers. Unsurprisingly, we’ll eventually have to update our domain’s existing nameservers to use Cloudflare’s nameservers.

Updating the nameservers After the DNS scan, we’ll access a gridlike view showing the various DNS records currently registered for our domain in Cloudflare. But that configuration won’t become active until we update the nameservers currently used by our domain with the one provided by Cloudflare. The required steps are summarized on Cloudflare’s Overview page, accessible through the menu on the left side of the page (figure 12.4).

Figure 12.4 Cloudflare’s nameservers setup guide The nameservers update process is straightforward and shouldn’t pose significant problems. We need to access the domain management dashboard of the domain service provider that we used to purchase the domain and replace the content of a couple of text boxes with the values provided by Cloudflare.

TIP First, it may be wise to do a detailed check to ensure that the automatic DNS scan performed by Cloudflare fetched all the existing A, CNAME, TXT, and MX records and to add the missing ones (if any) in the Cloudflare DNS management panel. If we fail to do that, the nameservers update process could compromise some of the existing domain services; they would no longer be resolved.

Next, we’ll need to wait for DNS propagation (the timeframe it takes for DNS changes to be updated across the internet). This process can take up to 72 hours, depending on several factors, including the time-to-live (TTL) values of our existing DNS records.

When the process completes, Cloudflare will send us an email notification saying that our domain is active on Cloudflare. If we don’t want to wait, we can click the Check Nameservers button at the end of the nameserver setup guide (partly visible in figure 12.4) to perform a real-time check.

NOTE For reasons of space, we’ve only scratched the surface of Cloudflare’s features and capabilities. To learn more about the service, read the official docs at https://developers.cloudflare.com.

While we wait for the nameserver update to propagate, we can proceed to the next step: deploying our ASP.NET Core web API by using Visual Studio’s publishing capabilities.

### 12.1.4 Fine-tuning our APP

In this section, we’ll take care of the following tasks:

1. Get our project ready for production by reviewing the appsettings.json file(s) and the Program.cs file.
2. Improve the web API’s security posture by adding some specific HTTP response headers (better known as security headers) that will help mitigate or prevent some common attacks.
3. Create a static API documentation interface (client) instead of exposing our Swagger definition file and SwaggerUI directly.

Reviewing the appsettings.json file(s) Our project’s appsettings.json files should be OK for production because we adopted the good practice of creating multiple versions of them, one for each environment we need to support. We have the following “cascading” files:

appsettings.json—Contains the default settings appsettings.Development.json—Contains development-specific settings and overrides (for all developers) secrets.json—Contains development-specific settings and overrides (for our own private development environment)

Ideally, we need to create a new appsettings.Production.json file containing the equivalent of the secrets.json files for the production environment. Adding this file in our Visual Studio project (and/or sharing it among developers) would be a bad idea, however, because it could easily end up in some publicly accessible repositories (such as GitHub) and/or unprotected storage devices.

For that reason, we’ll create the appsettings.Production.json file directly on the production server during the deployment phase. This approach may not always be practical, depending on the overall complexity of that file and the working practices of the IT development team, but it’s viable enough in our scenario. We’ll see how to do that when we deploy our web API in production. For now, we have nothing to do with these files.

Reviewing the Program.cs file As we’ve known since chapter 2, the Program.cs file allows us to set up middleware for each available runtime environment (development, staging, and production) by using the app.Environment property. Two good examples are the Swagger and SwaggerUI middleware, configured to be available only in development:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

Hiding Swagger in staging production environments is good security practice. Publicly documenting our API might give potential attackers some valuable hints on ways to abuse our endpoints. We can lessen these risks by implementing a restrictive Cross-Origin Resource Sharing (CORS) policy (chapter 3), as well as adding HTTP security headers that could mitigate or prevent other attacks.

Using HTTP security headers As we know all too well, each time a browser navigates to a website, it issues an HTTP Request to the web server hosting that site. The server typically responds with an HTTP Response, which usually contains a status code, some response headers and often some content in HTML, JavaScript Object Notation (JSON), or other browser-compatible formats.

Another thing we know well is that the response headers contain useful info that the client will use to handle several tasks, such as content caching, language and localization, and character encoding. Some of this (meta)data can affect the website’s security posture, as it controls the browser’s behavior with regard to the received content. It can force the client/browser to communicate over HTTPS only, for example, or force the browser to block any FRAME, IFRAME, or other source (SRC) content from third-party servers.

The response headers that contain these client policies are called security headers. Adding them to our web applications is widely considered to be one of the most convenient security best practices in terms of cost-benefit ratio, for the following reasons:

They’re easy to implement. It’s easy to understand how they work (and what attacks they help mitigate). They require minimum configuration changes. They have a negligible effect on performance.

Security headers are required nowadays to pass any penetration test, vulnerability scan, risk assessment, data security checklist, and decent IT audit and/or certification process. Here’s a list of the most important HTTP security headers that we might consider implementing in our web API project:

HTTP Strict Transport Security (HSTS)—Tells the web browser to access the web server over HTTPS only, ensuring that each connection will be established only through secure channels. X-Frame-Options—Protects against clickjacking (http://mng.bz/51y4) by preventing FRAMEs and IFRAMEs from specific sources (such as different web servers) from loading on your site. X-XSS-Protection—Protects against cross-site scripting (XSS) by enabling a specific filter built into most modern browsers. Although XSS filtering is active by default in most modern browsers, it’s advisable to enable (and configure) it explicitly to strengthen our website even more. X-Content-Type-Options—Prevents the browser from downloading, viewing, and/or executing a response that differs from the expected and declared content type, thus reducing the risk of executing malicious software or downloading harmful files. Content security policy—Prevents attacks such as XSS and other code-injection-based attacks by defining which content the browser should and shouldn’t load. Referrer policy—Determines whether the URL of the web page that linked to the requested resource has to be sent along with the request (using the Referer HTTP Header). The default behavior, if no value is specified, is to strip that header (and hence the referrer info) when going from a page using HTTPS to a page using unsecure HTTP and leaving it in all other cases.

Now that we know what these security headers are and what they do, let’s see how to implement them. As we might guess, the best place to handle this task is the Program.cs file. Open that file, scroll down to this point,

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

and replace it with the code shown in lithe following listing.

**Listing 12.1 Program.cs file: HTTP security headers**

```csharp
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}
else
{
     // HTTP Security Headers
    app.UseHsts();                                               ❶
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Frame-Options",
            "sameorigin");                                       ❷
        context.Response.Headers.Add("X-XSS-Protection",
            "1; mode=block");                                    ❸
        context.Response.Headers.Add("X-Content-Type-Options",
            "nosniff");                                          ❹
          context.Response.Headers.Add("Content-Security-Policy",
              "default-src ' self' ;");                             ❺
          context.Response.Headers.Add("Referrer-Policy",
              "strict-origin");                                     ❻
          await next();
    });
}
```

❶ Strict-Transport-Security header
❷ X-Frame-Options header
❸ X-XSS-Protection header
❹ X-Content-Type-Options header
❺ Content-Security-Policy header
❻ Referrer-Policy header

Notice that the HTTP Strict Transport Security (HSTS) header has its own built-in middleware; the others require a manual implementation using custom middleware. We chose to add the header only in nondevelopment environments—a wise choice, because some of headers (especially HSTS) might create some inconvenient problems when we run our app on localhost.

NOTE I won’t cover security headers in detail. We’ll add them by using their most common settings, which are good enough to protect our MyBGList web API. Plenty of websites offer information about how they work and how to configure them. A good place to start is the OWASP HTTP Security Response Headers Cheat Sheet at http://mng.bz/ydDJ.

Although the security headers we implemented can prevent or mitigate some attacks, exposing Swagger in a nondevelopment environment would still be risky. The best thing we can do to comply with the security-by-design approach is to keep adding Swagger middleware in the development environment only. Unfortunately, this approach will also prevent our audience from accessing the API documentation we worked with in chapter 11, including the SwaggerUI. Is there a way to preserve what we did there without exposing Swagger directly? Luckily, several code- generator tools can seamlessly create static, HTML-based web user interfaces from Swagger JSON definition files. In the next section, we’ll learn how to use one of them.

TIP First, be sure to inspect the Program.cs file thoroughly for other middleware that shouldn’t be used in nondevelopment environments, and use the app.Environment property to prevent them from being added to the HTTP pipeline. Our sample project has none of this middleware, but it’s a good practice to perform this check before the publishing phase.

Creating a static API documentation interface The first thing we must do to create static API documentation interface is choose a code-generator tool. Plenty of those tools are available, made, developed, and maintained by the Swagger team or third parties. For simplicity, we’ll limit our choices to the following:

Swagger Codegen—An open source command-line interface (CLI) tool available on GitHub at https://github.com/swagger- api/swagger-codegen. Swagger Editor—A web-based, open source online editor providing a Generate Client feature that uses Swagger Codegen to achieve the same results. This tool is available at https://editor.swagger.io. SwaggerHub—An integrated API design and documentation platform that includes the preceding tools and a lot of additional configuration and productivity features. It’s available at https://swagger.io/tools/swaggerhub.

The first two options are free, and the third one requires a subscription plan (with a 14-day free trial available). All things considered, the Swagger Editor is the most practical free choice, as it doesn’t require us to install anything. To use it, navigate to https://editor.swagger.io, and wait for the main page to load (figure 12.5).
Figure 12.5 Swagger Editor’s home page

The editor already contains a sample Swagger definition file (left panel), which is rendered in real time to a HTML-based user interface (UI; right panel). To replace the sample file with ours, perform the following steps:
1. Launch the MyBGList project in Debug mode, and access the SwaggerUI main dashboard.
2. Click the https://localhost:40443/swagger/v1/swagger.json link below the title to access our web API’s Swagger definition file, and save that file locally.
3. Go back to the Swagger Editor’s home page.
4. Choose File > Import File Option to import the saved swagger.json file.
5. When the file is loaded, choose Generate Client > HTML2 to download a zip archive containing a single HTML file.

That HTML file is our autogenerated, HTML-based UI. To see what it looks like, unzip and execute it in a browser (figure 12.6).
Figure 12.6 Static API documentation generated by Swagger Editor using Swagger Codegen

Not bad, right? Now that we have this static HTML file, we can choose how to release it to our audience—via email, a partner-only website, or any other secure method—without having to expose our Swagger definition file and/or the SwaggerUI directly. Alternative API documentation tools The static documentation we’ve obtained by using the Swagger Editor and Swagger Codegen is one of many options for creating a suitable reference for our web API. Several other open source and commercial tools can help us to achieve the same result, offering different UI/UX templates, additional customization features, and so on. I also encourage you to try the additional Generate Client options provided by the Swagger Editor, which include more advanced (and visually appealing) clients built with C#, TypeScript, Angular, PHP, Dart, Go, Scala, and the like.

We’re ready to move to our next topic: choosing a suitable .NET publishing mode.

### 12.1.5 Understanding the .NET publishing modes

The first thing we need to learn is that .NET applications can be published in two modes:

Framework-dependent—This publishing mode requires the presence of the .NET Framework, which must be installed and available on the target system. In other words, we’ll build a .NET application that can run only on a hosting server that has been configured properly to support it. Self-contained—This publishing mode doesn’t rely on the presence of .NET components on the target system. All components, including the .NET libraries and runtime, will be included in the production build. If the hosting server supports .NET, the app will run in isolated mode, separating itself from other .NET applications. Self-contained deployment (SCD) builds include an executable file (an .exe file on Windows platforms) as well as a dynamic link library (DLL) file containing the application’s runtime.

NOTE The framework-dependent mode produces a platform-specific executable and a cross-platform binary (in the form of a DLL file). The self-contained mode produces a platform-specific executable only.

Let’s try to understand the pros and cons of each publishing mode.

Framework-dependent mode The framework-dependent mode grants the developer several advantages:

Small package size—The deployment bundle is small because it contains only the app’s runtime and the third-party dependencies. .NET itself won’t be there because we expect it to be present on the target machine by design. Latest version—Per its default settings, the framework- dependent mode will always use the latest runtime and framework version installed on the target system, with all the latest security patches. Improved performance in multihosting scenarios—If the hosting server has multiple .NET apps installed, the shared resources will enable us to save some storage space and, most important, reduce memory use.

This publishing mode also has several weaknesses:

Reduced compatibility—Our app will require a .NET runtime compatible with the one used by our app (or later). If the hosting server is stuck to a previous version, our app won’t be able to run. Stability problems—If the .NET runtime and/or libraries change their behavior (in other words, if they have breaking changes or reduced compatibility for security or licensing reasons), our app would potentially be affected by these changes.

Self-contained mode Using the self-contained mode has two big advantages that could easily outweigh the disadvantages in some scenarios:

Full control of the published .NET version—Regardless of what is installed on the hosting server (or what will happen to it in the future) No compatibility problems—Because all the required libraries are provided within the bundle

Unfortunately, there are also some relevant disadvantages:

Platform dependency—Providing the runtime with the production package requires the developer to select the target building platforms in advance. Increased bundle size—The additional presence of the runtime resources will eventually take its toll in terms of disk space requirements—a heavy hit if we plan to deploy multiple SCD .NET Core apps to a single hosting server, as each of them will require a significant amount of disk space.

The bundle-size problem was mitigated in .NET Core 3.0 with the introduction of the app trimming feature (also called assembly linker), which trims the unused assemblies. This approach was further improved in subsequent .NET versions; assemblies were cracked open and purged of the types and members not used by the application, further reducing the size.

TIP For further info about the .NET app trimming feature, check out this post by Sam Spencer (program manager, .NET Core team): http://mng.bz/MlK2.

Choosing a publishing mode As we can guess, both publishing modes can be viable depending on several factors, such as how much control we have of the production server, how many ASP.NET Core apps we plan to publish, the target system’s hardware/software/size capabilities, and so on. As a general rule, as long as we have access to the production server (and permission to install and update system packages such as .NET and ASP.NET Core), the framework-dependent mode is often an ideal choice. Conversely, if we host our apps on a cloud-hosting provider that doesn’t have our desired .NET runtime, the self-contained mode probably would be a more logical choice (if not the only way to go). Available disk space and memory size will also play major roles, especially if we plan to publish multiple apps.
NOTE The requirement to install and update the packages on the server manually should no longer be a hindrance, because all .NET updates are now released through the regular Microsoft Update channel, as explained in this post by Jamshed Damkewala (principal engineering manager, .NET): http://mng.bz/aMzJ.

Now we must make a decision. For our MyBGList project, we’re going to use the framework-dependent publishing mode, because we plan to have full access to all the hosts where we’ll release and deploy our web API. But we still need to choose where we’re going to perform those release and deployment tasks.

Choosing the release and deployment target Let’s go back to the scenario introduced at the start of this chapter: our IT project manager asked us to release and deploy our web API to a public server to make it publicly accessible on the World Wide Web. To comply with this request, we need to make some important decisions, carefully evaluating the pros and cons of each option. Here’s a breakdown of the most relevant questions we should ask before proceeding, assuming that we’re a member of a full-stack development team or the deployment manager of a structured IT team:

Should we use a physical or virtualized server? Should we host the server on our own data center (assuming that we have one) or on an external data center, or should we use a cloud service platform? If we choose the cloud, should we opt for an Infrastructure as a Service (IaaS) environment, which would allow us to set up, access, and configure the web server(s) directly, or should we use a Platform as a Service (PaaS) alternative, in which all the machine, OS, and runtime-related tasks are handled and managed by the service provider? If we opt for the server (or service) OS, should we pick Windows or Linux?

As always, the answer to each question depends on several factors: the budget; the skills, know-how, and composition of the IT team; and the performance we expect. Luckily, regardless of the choices we make, the publishing, release, and deployment tasks won’t change much. Physical and virtualized servers work in the same way, and .NET and ASP.NET Core, due to their cross-platform foundation, work well on both Windows and Linux systems.

After taking all those questions into account, we decide to release and deploy our web API to a Windows Server virtual machine (VM). In the following sections, we’ll create and configure those hosts, starting with the Windows VM.

## 12.2 Creating a Windows VM server

Creating a Windows VM on the cloud is easy these days, thanks to the fact that most cloud service providers platforms—such as Azure, Amazon Web Services (AWS), and Google Cloud Platform—offer a wide set of fully managed, GUI-based configuration tools that allow users to create a VM within minutes. This UI-based approach has greatly simplified setup of a VM, allowing any willing developer or IT enthusiast, even with limited technical know-how, to create and manage a web server host. But this hyper-simplification has also increased performance, stability, and security risks due to an incorrect configuration of the underlying machine, OS, services, and core apps, including the web server software itself.

For reasons of space, we will not delve into the subject of system hardening, a collection of tools, techniques, and security best practices to minimize the vulnerabilities of a given infrastructure, server, or service. But we’ll reduce the risks of creating a weak VM by choosing a cloud platform that grants good levels of performances, reliability, and security. We’ll create our VM in Microsoft Azure, the public cloud platform managed and operated by Microsoft.

Reasons for choosing Azure We opted for Azure not only for security reasons, but also because it fully integrates with .NET, .NET Core, and Visual Studio publishing features, which will greatly simplify our deployment. You’re free to choose any other suitable physical, virtualized, or cloud- based alternative. Most release and deployment techniques explained in this chapter will also work for any other infrastructure, platform, or solution.

In the following sections, we’re going to set up a brand-new VM on the Azure platform and then perform the release and deployment tasks on it. Here’s what we’ll do:

1. Create a new VM in Azure, using the Windows 2022 Datacenter Edition template and configure it to accept inbound calls to the required TCP ports.
2. Configure the VM by downloading and/or installing all the necessary services and runtimes to host our ASP.NET Core web API, including Internet Information Services (IIS).
3. Publish the MyBGList web API and deploy it to the web server we’ve set up.
4. Test the MyBGList web API, using a remote client/browser.

NOTE If you already have a production-ready Windows Server, you’re free to skip the VM setup sections and go directly to the publishing and deployment topics.

### 12.2.1 Accessing Azure

To use the Azure platform, we need to create a new account or use an existing account. The good part about creating a new account is that it comes with a welcome offer that grants $200 in credits, more than enough to do everything we’re about to do in this chapter.

NOTE If you’ve read chapter 7 and followed the required steps to implement the ApplicationInsights logging provider, you should have an Azure account (and the credits). In case you didn’t, point your browser to https://azure.microsoft.com/en-us/free and click the Start Free button.

When you have an account, navigate to https://portal.azure.com to access the Azure administration portal, where we’re going to spend some of our $200 credit to create our new VM.

### 12.2.2 Creating and setting up the Windows VM

On the main Azure administration portal dashboard, click the Virtual Machines icon, as shown in figure 12.7 (or just search for virtual machine in the top search bar).

Figure 12.7 Create a new VM in Azure.

On the next page, click Create (near the top-left corner of the page), and select the Azure Virtual Machine option to access a new page called Create a Virtual Machine, which is where most of the magic happens. The Create a Virtual Machine page is a wizardlike form that we use to create and configure our VM. The various configuration settings are grouped in several panels, each one dedicated to a specific set of options:

Basics—Subscription type, VM name, deployment region, image, login credentials, and so on Disks—Number and capacity of drives that will be assigned to the VM Networking—Network-related configuration settings Management—Antimalware settings, autoshutdown capabilities, backup, OS updates, and the like Monitoring—Diagnostic and system monitoring features Advanced—Agents, scripts, extensions, and other advanced settings Tags—Key/value pairs that can be used to assign Azure resources to different logical groups

Most of the default settings are good enough for tutorial purposes. For the sake of simplicity, we’ll change something here and there. Here’s a short list of what we need to change in the Basics tab:

Subscription—Select the active subscription that you want to use for this VM. If you have a new free account, you should have your free trial subscription available. Resource group—Create a new resource group with a suitable name. (We’ll use Manning in our screenshots and samples.)

VM name—Choose a suitable name to assign to the VM. (We’ll use MyBGList-Win-01.)

Region—Choose the region closest to your geographical position. Availability options—No infrastructure redundancy is required. Image—We’ll choose Windows Server 2022 Datacenter: Azure Edition - Gen 2. VM architecture—Choose X64. Run with Azure Spot discount—Select Yes to create the VM by using the Azure Spot feature, which allows us to take advantage of Azure’s unused capacity at a significant cost saving. But because these VMs can be evicted at any time when Azure needs the capacity back, such a feature should be used only for short-term testing purposes. If we want to create a permanent, production-like VM, we should choose No and create a standard pay-as-you-go machine. Note that the Azure Spot option isn’t available for all VM series and/or sizes (including the B2ms that we’re going to use in this tutorial). Size—Choose Standard B2ms (2 vCPU, 4 GB memory) or anything similar. Feel free to choose a different size if you’re willing to spend more. B2ms is an entry-level machine featuring a limited set of resources that will suffice for this deployment sample. It won’t perform well in production, however. Administrator account—Select the Password authentication type and then create a suitable username and password set. Remember to write these credentials in a secure place because we’ll need them to access our machine in a while.

TIP For additional info about the Azure Spot feature, check out http://mng.bz/gJDR.

Click the Next button at the end of the form to move to the Disk tab, where we need to configure the OS disk type settings. Select Standard HDD if you want to save credits, as it is the cheapest available choice and good enough for our tutorial. For nontutorial purposes, however, it’s strongly advisable to choose a solid-state disk. All other settings, including the Advanced options panel near the end of the page, can be left at their default values. Next, move to the Networking tab and change the following settings:

Virtual network—Create a new one called <ResourceGroupName>-vnet (should be the default option). Public inbound ports—Choose Allow Selected Ports and then select the HTTP (80) and HTTPS (443) ports, leaving ports 22 and 3389 closed for security reasons.

Skip the Management tab (the default settings are fine), and access the Monitoring tab, where we’re given the option to disable the Diagnostics > Boot Diagnostics settings. Doing that is not recommended for production servers, but it can be a good idea for our tutorial purposes (to save some credits). Leave all other settings in the next tabs at their defaults, and go straight to the Review + Create tab (figure 12.8), where we’ll review our configuration settings and confirm them by clicking Create in the bottom-left corner of the screen.
Figure 12.8 Reviewing the VM settings

Check out the VM settings to ensure that they’re OK; then click Create to start the VM deployment process, which will likely take a few minutes. Eventually the page will refresh, informing us that the VM deployment is complete and giving us the chance to access its main configuration panel by clicking to the Go to Resource button, which is precisely what we’re going to do.

### 12.2.3 Working with the VM public IP address

The VM configuration panel is where we can review and change all the VM’s main settings. The panel is split into several pages, all accessible from the left menu. If we accessed it from the Go to Resource button, we should be looking at the Overview page (figure 12.9), which is a dashboard showing a summary of the current settings. From there we should be able to retrieve the VM’s public IP address, which is located in the right part of the screen.

Figure 12.9 Setting a unique DNS name

Adding an Azure DNS name While we’re here, we configure a unique DNS name that will resolve to that IP address by clicking the DNS Name option below it. We’ll gain access to a new settings page where we’ll be able to input an optional DNS name label, as shown in figure 12.10.

Figure 12.10 Setting a unique DNS name

As we can see, we used the lowercase VM name (mybglist- win-01) to set the DNS name label. This will create the following Azure-managed unique subdomain, which will resolve to our VM’s IP address: mybglist-win- 01.westeurope.cloudapp.azure.com.

But we don’t want to use this subdomain to access our web API directly. We want to have our HTTP incoming request be handled by the Cloudflare CDN that we set up earlier. For that reason, in the next section we’re going to configure our VM’s public IP address on our CDN by creating a dedicated A record (and subdomain) for it.

Adding a new A record Open another tab in the browser (without closing the VM Overview page) and use it to access Cloudflare (https://www.cloudflare.com). Log in with the account we created earlier; select the relevant domain (mybglist.com in our sample); and click the DNS page on the left menu to access the domain’s DNS configuration settings, as shown in figure 12.11.

Figure 12.11 Cloudflare DNS management page Click the Add Record button to create a new record, specifying the following options, as shown in figure 12.12:

Type—A Name—win-01 IPv4 address—Azure VM public IP address (20.123.146.191 in our sample)

Proxy status—Proxied TTL—Auto

Figure 12.12 Cloudflare Add Record settings for win- 01.mybglist.com

These settings create a new win-01.mybglist.com subdomain that will point to the IP address of our Windows VM, like the Azure unique DNS name that we created earlier.

Azure DNS name vs. custom subdomain There is an important difference between how the Azure DNS name and our custom win-01.mybglist.com subdomain will work in practice: the Azure-managed subdomain will resolve directly to our VM server’s IP, and the Cloudflare subdomain will not because we activated Cloudflare’s proxying (and IP masking) feature. More precisely, the win-01.mybglist.com subdomain will resolve to Cloudflare’s proprietary IP, which will internally forward all requests to our VM’s public IP without exposing it to the public. Such a difference might seem trivial, but it could be relevant in terms of security as long as we use only the Cloudflare subdomain, keeping the Azure one hidden from public eyes.

Moreover, notice that we intentionally chose to not use the www.mybglist.com domain name for this deployment tutorial. We preferred to use a subdomain that uniquely identifies the Windows VM that we’ll create later, so that we’ll be able to create additional VMs/hosts in future editions of this book. You’re free to use www (or any other suitable subdomain name) instead.

Click the Save button to persist the newly created A record to the domain DNS configuration so that we can test it.

Testing the A record The new subdomain will start working immediately, because our domain’s DNS configuration is already handled by Cloudflare and the internal proxying feature doesn’t need to propagate. But even if our Windows VM is already up and running, it doesn’t have a web server listening to the HTTP (80) and/or HTTPS (443) TCP ports.

If we try to navigate to http://win-01.mybglist.com/ or https://win-01.mybglist.com/ with our browser, we hit a 522 - Connection timed out error (see figure 12.13). Notice that the error is clearly coming from Cloudflare, meaning that the connection problem occurs between the CDN (working) and the Host (error), as shown by the informative icons displayed by the error page. There’s no need to worry. The error will disappear as soon as we configure a web server to handle the incoming HTTP requests. First, let’s fulfill another important task in Cloudflare: create an SSL/TLS certificate that we will configure to our origin server.

Figure 12.13 Cloudflare returning a 522 - Connection timed out error

### 12.2.4 Creating an SSL/TLS origin certificate

One of the main reasons why we chose Cloudflare as our CDN service provider is that it grants two free SSL/TLS certificates for our domain, one for the edge server (valid for everyone) and one for the origin server (valid only for Cloudflare servers). The edge certificate is automatically generated, renewed, and served by Cloudflare; the origin certificate must be created manually and then installed on the origin hosts.

Why do we need an origin certificate? Setting up the origin certificate is optional. We need it only if we want Cloudflare to connect to the origin server using HTTPS, thus using the Full SSL/TLS encryption mode instead of the default one (which uses HTTP). However, if we want to adhere to our security-by-design approach, we definitely need to switch to that encryption mode to ensure that all transferred data will be always encrypted from the origin server to the end users’ browsers, and vice versa.

Because we’re going to configure our newly created Windows VM, now is a good time to generate the origin certificate. From Cloudflare domain’s configuration page, expand the SSL/TLS panel in the left menu and then select the Origin Server option to access the Origin Certificates section (see figure 12.14).
Figure 12.14 Create an origin certificate in Cloudflare.

Click the Create Certificate button to start the generation process. We access a dedicated form, which we can fill with the following values:

Generate private key and CSR with Cloudflare—Yes. Private key type—RSA (2048). Hostnames—Leave the default values. Choose how long before your certificate expires—15 years.

Then click the Create button to generate the certificate. After a couple of seconds, we access a page with two text boxes containing, respectively, our origin certificate and its private key. Copy and paste the content of each text box into a text editor of your choice (such as Windows Notepad) and then save both in a safe local folder using the following names:

mybglist.com-origin.pem—For the origin certificate mybglist.con-origin.key—For the private key

We’re going to need these files when we configure our Windows VM to be the origin host. Before moving on to that, let’s switch the Cloudflare SSL/TLS encryption mode from Flexible to Full to take advantage of what we did.

### 12.2.5 Setting Cloudflare Encryption Mode to Full

From Cloudflare domain’s configuration page, expand the SSL/TLS panel in the left menu and then select the Overview option to access the SSL/TLS Encryption Mode section (figure 12.15). Then change the encryption mode from Flexible to Full.
Figure 12.15 Cloudflare’s SSL/TLS Encryption Mode settings

The Full encryption mode ensures that Cloudflare will always use (and require) the origin certificate when connecting to our origin host, thus granting full encryption capabilities to our architecture’s HTTP stack. We need to configure that host, which is what we’re about to do.

## 12.3 Configuring the Windows VM server

Now that we have created our Windows VM server, we need to connect to it. Azure gives us several ways to do that, the most secure of them being Azure Bastion, a fully managed service providing secure and seamless Remote Desktop Protocol (RDP) and Secure Shell Protocol (SSH) access to VMs without any exposure through public IP addresses.

TIP For additional info about Azure Bastion features and pricing, check out http://mng.bz/eJjJ.

The only “bad” thing about Azure Bastion is that it’s a paid service with an hourly rate that might be too costly for our actual scenario. For the purpose of this chapter we’ll use a standard RDP connection by (securely) opening the 3389 TCP port on our Azure VM.

NOTE To open the 3389 TCP port for inbound traffic on Azure, follow the instructions detailed in the appendix, using 3389 for the port number and RDP for the service name. It’s important to remember that the 3389 TCP port is a common target for network-related attacks performed by malicious third parties, as it’s used by a protocol (RDP) that allows attackers to take control of the VM remotely. Keeping that port open for everyone is hardly good practice in terms of security. To mitigate these risks, it’s strongly advisable to restrict the inbound rule to the IP address(es) of our development machine(s), as explained in the appendix.

Now that we have the 3389 TCP port open, we can connect with our new server using the RDC tool from our local Windows-based development machine. Type the public IP address of the Azure VM that we retrieved earlier and click Connect to initiate an RDP session with our remote host. Once connected, the first thing we must do is install and configure IIS, a flexible, secure, and manageable HTTP server that we’ll use to serve our ASP.NET Core web API over the web.

NOTE For reasons of space, we’re not going to explain how IIS works or explore its functionalities. We’ll use the settings strictly required to host our app. For additional information regarding IIS, check out https://www.iis.net/overview.

Together with IIS, we will install Web Deploy, an extensible client- server tool that can be used to publish and deploy web apps directly from Visual Studio, thus avoiding the need to implement other data- transfer alternatives (such as FTP). Last, but not least, we’re going to install a local instance of SQL Server to host our MyBGList web API’s production database.

### 12.3.1 Installing IIS

To install IIS, follow the instructions detailed in the appendix. Be sure to do that in the Windows VM using the established RDP connection, not on your local development machine.

### 12.3.2 Installing the ASP.NET Core hosting bundle

When IIS has been installed, we can proceed with downloading and installing the ASP.NET Core Windows Hosting Bundle, a convenient bundle that includes the .NET runtime, the ASP.NET Core runtime, and the ASP.NET Core IIS module. That’s all we need to run our ASP.NET Core web API using the framework-dependent publishing mode.

TIP The instructions for installing the ASP.NET Core Windows hosting bundle are detailed in the appendix.

After the installation, be sure to restart the IIS service by following the appendix instructions.

### 12.3.3 Installing the Web Deploy component

Web Deploy (formerly msdeploy) is a IIS-related component that greatly simplifies the deployment of web applications to IIS servers. For our release and deployment tutorial, we’re going to use it to create a one-click publishing profile that will allow us to deploy our MyBGList web API from Visual Studio to the VM server in a fast and seamless way (as we’ll see in a short while).

It’s important to understand that Web Deploy is not the only deployment option offered by Visual Studio; it’s the one we have chosen for this book. If you prefer to use a more “classic” deployment approach based on a different data transfer method/protocol such as FTP, XCOPY, RoboCopy, SCP, and the like, feel free to skip the Web Deploy installation. You will also need to create a different Visual Studio publishing profile (and deal with the file upload tasks manually).

TIP Web Deploy installation instructions are detailed in the appendix. After installing it, we also need to open the 8172 TCP port for inbound traffic, which is required by the IIS management service (and Web Deploy).

### 12.3.4 Opening the 8172 TCP port

The 8172 TCP port must be open at OS level (using the Windows Firewall) and network level (using the Azure VM configuration panel). The Windows Firewall configuration should be OK, assuming we have already installed IIS with the management service optional role feature, as it’s automatically handled by that component during the installation process. We need to open that port on Azure.

TIP To open the 8172 TCP port for inbound traffic on Azure, follow the instructions detailed in the appendix, using 8172 for the port number and Web Deploy for the service name.

With that done, we can move to the next step: configuring IIS to host our web API.

### 12.3.5 Configuring IIS

Let’s start by launching the IIS Manager app, a management tool that gets installed with IIS and can be used to configure most IIS settings through an intuitive, easy-to-use GUI (figure 12.16).
Figure 12.16 The IIS Manager main dashboard

The first thing to do here is install the SSL/TLS origin certificate that we got from Cloudflare a short while ago. However, the PEM/KEY certificate format that we have saved to our local disk cannot be directly imported to IIS. To do that, we need to convert them to a PFX (PKCS#12) file.

Converting the PEM certificate to PFX To quickly convert our PEM Certificate to a PFX file we suggest using OpenSSL, an open source command-line tool that can be used to perform a wide variety of cryptography tasks, such as creating and handling certificates and related files. Most information about OpenSSL, including the updated GitHub link to the project’s source code, can be retrieved from its official website (https://www.openssl.org). However, the Windows binaries (and installer packages), which is what we need to install it, are available at the following third-party URL, courtesy of Shining Light Productions (http://mng.bz/pdDP).

More precisely, we need to download the Win64 OpenSSL v3.0.5 Light edition. The light edition is lightweight (5MB) and contains only the tool’s basic features, which are more than enough for our purposes.

The installation process is straightforward; we’ll be asked only to copy the required DLLs to the Windows folder or to a separate folder (both will work) and to make an optional donation to the authors. After installing it, the tool will be available at the following path:

C:\Program Files\OpenSSL-Win64\bin

To use it, open a command prompt, navigate to the folder where we put the .pem and .key files, and execute the following one-line command:

"C:\Program Files\OpenSSL-Win64\bin\openssl.exe" pkcs12 -export -out mybglist_origin.pfx -in mybglist_origin.pem -inkey mybglist_origin.key
TIP In case you installed the tool on a different path and/or used different filenames, change the command accordingly.

The tool will ask us to enter an export password. Choose one (and remember it, as we’ll need it in a short while); then confirm it by typing it again, and click Enter to generate the mybglist_origin.pfx file.

TIP Now we can safely delete the .pem and .key files from our local machine, as we don’t need them anymore.

Now that we have the .pfx file, we can install it on our Windows VM’s IIS.

Installing the SSL/TLS origin certificate Go back to the Windows VM desktop. Then copy the mybglist_origin.pfx file from our local machine to the remote VM, using the RDP copy/paste capabilities.

Switch to (or open) the IIS Manager main dashboard, locate the Server Certificates icon, and click it to access that section. Next, click the Import link button in the Actions menu on the right to access the Import Certificate modal window (figure 12.17).
Figure 12.17 The IIS Manager’s Import Certificate window

Fill the form with the following values, shown in figure 12.17:

Certificate file (.pfx)—The .pfx file path on the remote VM

Password—The export password we chose earlier Select Certificate Store—Web hosting Allow this certificate to be exported—Yes

Click OK to import the certificate.
TIP Again, we can safely delete the .pfx file from our local machine and from the remote server right after importing it. We can always export it from the IIS Manager certificate store if we ever need to.

With that done, we can finally configure our ASP.NET Core web API to IIS.

Adding the MyBGList website to IIS Keeping the IIS Manager GUI open, locate the Sites folder node in the left tree view; right-click it and choose the Add Website option to access the modal window, which we will use to add a new website entry for our MyBGList web API. Fill the form with the following values:

Site name—MyBGList Content directory Application pool—MyBGList Physical path—C:\inetpub\MyBGList Binding Type—https IP Address—All unassigned Port—443 Host name—win-01.mybglist.com

Require Server Name Indication—Yes SSL Certificate—CloudFlare origin certificate Start Website immediately—Yes After we click OK, a new MyBGList website entry should appear in the left tree view, right below the default website.

TIP Let’s take this chance to disable the default website (right-click, Manage Website, Stop), as we don’t need it.

We’re almost done. We need to start the web management service.

Starting the web management service In the IIS Manager GUI, left-click the main server’s root note (it should be called MyBGList-Win-01, like the VM name), locate the Management Service icon in the central panel, and click it to access another configuration window (figure 12.18).
Figure 12.18 The IIS Manager’s Management Service window

From here, we need to do the following:

1. Activate the Enable Remote Connections check box.
2. Start the service by clicking the Start button to the right.

Web Management Service startup mode The Web Management Service is configured with a manual startup mode, meaning that it won’t automatically start when the VM boots (or reboots). If we plan to consistently use the Web Deploy component to publish and deploy our app, we might think of changing the startup behavior of this service, as it’s required to be running for that feature to work. This can be done using the Control Panel > Administrative Tools > Services app.

Now that IIS has been properly configured, we can deal with the database.

### 12.3.6 Creating the production database

In chapter 4 we hosted our development database on a local instance of SQL Server installed on our development machine; more precisely, we opted for that route instead of using a cloud-based solution such as the SQL database provided by Azure. Now that we are releasing our web API in production, we are faced with that same choice: on- premise or cloud-based?

For reasons of space, in this deployment tutorial we’re going to pick the on-premise option again so that we can replicate what we have already learned and got used to. However, readers who want to challenge themselves with a different alternative (such as the previously mentioned Azure SQL Database service) are strongly encouraged to do that.

NOTE For additional info, documentation, and installation guides about the Azure SQL Database service and other Azure SQL-related available products, check out https://learn.microsoft.com/en- us/azure/azure-sql.

Now that we have chosen our path, here’s what we need to do:

1. Install SQL Server.
2. Install SQL Server Management Studio (SSMS).
3. Create a new instance of the MyBGList database.

All of these tasks can be easily dealt with by repeating the same steps we took in chapter 4.

Should we install SSMS on the VM server? Technically speaking, we could avoid installing the SQL Server Management Studio tool on the server and connect to the remote database using the SSMS local instance that we installed on our development machine. However, this would require us to open the 1433 TCP port (as we did with 3389 and 8172 earlier), which would lead to additional security problems. In case we want to do that, we should definitely restrict the inbound rule to the IP address(es) of our development machine(s), as explained in the appendix.

When creating the MyBGList database, be sure to also create its login and user structure, as we’re going to need them for the production connection string.

NOTE For obvious security reasons, be sure to set a different password for the production login. Using the same passwords for development and production environments is a bad practice that could lead to severe data breaches.

Once the MyBGList production database is up and running, we can finally create the appsettings.Production.json file on the Windows VM.

### 12.3.7 Creating the appsettings.Production.json file

When we added the MyBGList website entry to IIS, we were asked to specify a physical path, which we filled with C:\inetpub\MyBGList earlier; that’s the filesystem path where IIS expects our web API artifacts to be deployed, including the appsettings.json files.

Open that folder using the remote VM’s File Explorer (or command prompt), and create a new appsettings.Production.json file within it using a text editor of your choice (such as Windows Notepad). That’s the file that will be used by our ASP.NET Core web API to override the appsettings.json values in the production environment.

Ideally, the appsettings.Production.json file should contain the same keys and values that we currently have in our local secrets.json file, with the sole difference of the ConnectionStrings’s DefaultConnection value, which we must replace with the values required to connect to our MyBGList production database. If we followed the same naming conventions that we used when creating the development database, we’ll have to change the user ID and/or password values, leaving the rest as is.

WARNING For the sake of simplicity, we’re not going to change the Azure Application Insight Connection String and the Redis Configuration values, because they are not that relevant to our sample tutorial scenario. But assuming we want to use those services, we should definitely create different instances for development and production environments as we did with the MyBGList database.

We can quickly deal with this task by using the RDP client to copy and paste the content of our local secrets.json file to the appsettings.Production.json file and then perform the required value change(s) on the new file and then save it to disk. While doing that, ensure that the appsettings.Production.json file has the correct extension (.json and not .json.txt, which is a common problem when using Notepad).

With this last step, our Windows VM configuration is complete. Now it’s finally time to publish, release, and deploy our web API. However, before doing this, we need to learn more about the Visual Studio built-in publishing capabilities and how this feature actually works.

## 12.4 Publishing and deploying

In this section we’ll learn how to publish our app. More specifically, we’ll create the output artifacts (framework libraries, project binaries, static contents, and the like) that will then be deployed to the production server(s) to release our app over the World Wide Web. Here’s what we’re going to do in detail:

1. Choose a suitable .NET publishing mode among the four available ones, which will be done after learning the pros and cons of each of them.
2. Create a Windows publishing profile using Visual Studio’s publishing capabilities to publish and deploy our web API project on the production server.

Are we ready? Let’s start!

### 12.4.1 Introducing Visual Studio publish profiles

The Publish feature is one of the greatest advantages of Visual Studio (and VS Code) over the other IDE alternatives: a built-in set of capabilities that allow developers to build, publish, and sometimes even deploy a web application directly from the GUI, thus greatly simplifying the whole release and deployment process.

The publish feature is basically a wizard that guides the developer to build a profile, a set of configuration settings about the project and the deployment target. Once created, the profile is saved to a dedicated .pubxml file on a dedicated /Properties/PublishProfiles/ folder, which is created together with the first profile (if it doesn’t exist already). Such a file/folder structure allows applications to have multiple publishing profiles, meaning that we can pre-configure its release and deployment settings over several different locations. To create a new publish profile, right-click the MyBGList project’s root node in Solution Explorer and select Publish.

TIP Alternatively, we can select the Publish MyBGList option from the Build section of the menu. Once we do that, we’ll be greeted by a modal window that will ask us to select one of several available publish targets, as shown in figure 12.19.
Figure 12.19 Visual Studio Publish feature Each available target provides a short description explaining how it works. The important thing to notice here is that most targets will create a profile that will perform the publishing and deployment tasks together, meaning that our app will also automatically go live at the end of the publishing process. However, a few of them (such as the folder target) allow us to only publish the artifacts locally, thus requiring us to manually perform the deploy using different techniques.

Publish and deploy requirements As we can easily expect, the “publish and deploy” profile targets require the hosting server to support (or be compatible with) a specific deployment module, package, or component, be it a FTP server, the Web Deploy component that we have installed a short while ago, or other services supported by Visual Studio. This requirement is quite easy to fulfill when dealing with Windows-based hosts Microsoft services (such as the Azure App Service). However, it can be difficult for Linux servers, where these techniques are not supported. For that reason, publishing profiles for Linux hosts typically targets a local folder or an FTP server preinstalled on the host.

Because we installed the Web Deploy component to our Windows VM server, our best option here would be creating a publishing profile based on the Azure target. Let’s do this.

### 12.4.2 Creating an Azure VM publish profile

From the Visual Studio’s Publish modal windows, choose the Azure target, select the Azure Virtual Machine option from the next screen (figure 12.20), and click Next to proceed.
Figure 12.20 Publishing from Visual Studio to an Azure VM In the following panel we’ll be asked to select (and/or connect) an Azure account and a Subscription name. After completing that part with our relevant info, we should be able to select our Windows VM, as shown in figure 12.21.
Figure 12.21 Selecting the Windows VM where we want to publish and deploy our app

Click the Finish button to create the publish profile, which will be saved in the /Properties/PublishProfiles/ folder. From now on, if we right-click the MyBGList root node in the Solution Folder’s tree view and select the Publish option again, Visual Studio will open a dedicated MyBGList: Publish tab (figure 12.22) that can be used to visually edit and configure the publishing profile we have created. Let’s take the chance to do that, because we need to change a couple of default settings that Visual Studio took for granted while creating the profile.

### 12.4.3 Configuring the publish profile

From the MyBGList: Publish tab, click the Show All settings link button (or More Actions > Edit) to access a modal containing the publish profile’s configuration settings, as shown in figure 12.22.
Figure 12.22 Visual Studio’s Publish Profile configuration settings

From there, change the following settings: Connections Site name—MyBGList (it must match the name of the website entry that we created on IIS). User name—Insert the VM Administrator’s user name. Password—Insert the VM Administrator’s password (it will make sense only if you activate the Save Password check box below). Save password—Choose Yes if you want to save it and No if you prefer to type it every time. Destination URL—https://win-01.mybglist.com. Validate Connection—Click this button to ensure that the connection can be performed successfully with these parameters. Settings Configuration—Release Target Framework—net6.0 Deployment Mode—Framework-dependent Target Runtime—win-x64

Right after doing that, click Save, we can finally hit the Publish button near the top-right of the MyBGList: Publish tab to finally start the publish, deployment, and post-deployment testing tasks.

### 12.4.4 Publishing, deployment, and testing

If everything goes well, the publishing (and deployment) process will successfully complete without problems. Once it completes, we should receive a Publish Succeeded on <CurrentDateTime> message with an Open Site link button that we can click to launch the https://win-01.mybglist.com website.

Let’s click the button to see if our web API is working. If we did everything correctly, we should be welcomed by... an HTTP 400 - Not Found error page.

We might be surprised by this error, but if we think about it, we can see how it’s absolutely normal. When we were launching our project in Debug mode (and from a development environment), we had the SwaggerUI dashboard configured as the startup page in the launchSettings.json file. Now that we are in production, there is no such file and no SwaggerUI either, as we have chosen to disable it for security reasons.

However, we still have some GET endpoints that we can call with our browser to test that the app is working, such as the following:

https://win-01.mybglist.com/error/test https://win-01.mybglist.com/cod/test

The error/test endpoint should work without problems, showing the same RFC7231-compliant JSON error we implemented in chapter
6. Conversely, the cod/test endpoint that we added in chapter 3 will likely output a blank page. Where’s the JavaScript alert pop-up that is expected to appear? To answer this question, press Ctrl+Shift+I (or F12) to access the browser’s developer console and take a look at the red message that should be there:

Refused to execute inline script because it violates the following Content Security Policy directive: "default-src ' self' ". Either the ‘unsafe-inline’ keyword, a hash (‘sha256-j3kW1ylRRXx1+pINyR/6EW435UHoxKSlU6fhd5xVSSk=‘), or a nonce (' nonce-...' ) is required to enable inline execution. Note also that ' script-src' was not explicitly set, so ' default-src' is used as a fallback.

As we can see, the JavaScript alter window was negated by the content security policy that we set with one of our HTTP security headers at the start of this chapter. That’s great news, as now we have the visual proof that they work! Let’s take this chance to slightly relax that policy to allow this (and only this) script to be accepted (and executed) by the requesting browsers.

Updating the content-security policy Open the Program.cs file and scroll down to the HTTP security headers block. Locate the line of code that adds the Content- Security-Policy header and change its value in the following way:

context.Response.Headers.Add("Content-Security-Policy", "default-src ' self' ; script-src ' self' ' nonce-23a98b38c' ;");

The nonce that we have set here is a random alphanumeric identifier that instructs cloud service providers to only allow scripts having that same nonce in their <script> element. Unsurprisingly, this also means we need to set that same nonce value in our script. To do that, open the Program.cs file, scroll down to the Minimal API method that handles the cod/test endpoint, and update its current implementation in the following way (new code is marked in bold):

app.MapGet("/cod/test",
    [EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
    Results.Text("<script nonce='23a98b38c'>" +    ❶
❶ Adds the nonce value

WARNING From a security perspective, enabling inline scripts negates the whole purpose of the content security policy. For that reason, I strongly recommend avoiding doing what we did here in a real production web app unless we really need these kinds of code on demand endpoints.

Once done, execute again the publish profile to redeploy the web app to the production server, then try again to call the cod/test endpoint. The JavaScript alert pop-up should now appear without problems.

Checking the logs table The second thing to do is test if the logging capabilities of our app are properly working. As we’ve known since chapter 7, our web API is expected to log its events to the [MyBGList].[LogEvents] database table using the Serilog SQL Server sink.

Let’s open that table using the VM server’s SSMS tool to ensure that it exists (it should have been created automatically by Serilog) and that it contains some startup-related log entries. If we can confirm these assumptions, it means that the logging feature is working as expected.

While we are there, let’s also take the chance to thoroughly check the existing log entries to ensure that the app is running without errors. As we learned in chapter 7, keeping this table under control will be a crucial activity to prevent or promptly react to most runtime and application-related problems that might affect the production environment over time. Creating the database structure The next thing we should do is to create the board-game-related tables, as they are still missing from the MyBGList production database. There are several methods to do that, including

Generating a SQL script from the development machine using the dotnet-ef CLI, and then execute the scripts on the production DB Creating a migration bundle from the development machine using the dotnet-ef CLI, and then execute them on the production machine Using the dotnet-ef CLI directlyfrom the production machine Using a programmatic approach, such as the dbContext.Database.Migrate() helper method from within the Program.cs file

The following article explains these alternatives in detail, highlighting the pros and cons of each: http://mng.bz/Y6M7.

In a nutshell, SQL script and migration bundle (introduced in .NET 6) are the recommended approaches, as they offer some unique advantages without significative downsides. More specifically, SQL scripts can be reviewed by an experienced DBA that could fine-tune the raw SQL commands to fit production-specific requirements and/or prevent accidental data losses before applying them; however, they require a dedicated SQL management tool (such as SSMS) to execute them. Conversely, migration bundles are self-contained executables that can be launched in production without any software requirement (no SQL management tool, NET SDK, or the dotnet- ef CLI).

TIP Alternatively, we could avoid any of these approaches and copy the whole development database structure in production using the SSMS import/export tools. This approach would basically have the same effect of the EF Core SQL script method that we will adopt here.

For our specific scenario, because we have installed the SSMS tool on the server, we’re going to use the SQL script approach. To generate the script, execute the following tasks:

1. Open a command prompt from the development machine.
2. Navigate to the project’s root folder (the one containing the Program.cs file).
3. Execute the following dotnet-ef CLI command:

> dotnet ef migrations script -o ./Migrations/script.sql

After doing that, we’ll end up with a script.sql file in the project’s /Migrations/ folder. We just have to open that file, copy its content, paste it to a new SSMS query window and execute it there to re-create our database model structure in the MyBGList production database—minus the [AppCache] table, that we have created without relying to EF Core migrations and hence requires a separate script.

Creating the AppCache table The quickest way to generate the SQL script for the [AppCache] table is to use the SSMS tool installed on our development machine. From the tree view on the left, right-click on the [AppCache] table, then Select Script Table As, Create To, New Query Editor Window to generate the script in a new tab. Once done, copy and paste the script to a new query window on the SSMS tool installed on the production server, then execute it to create this last table. Now that we have re-created the whole DB structure on our production server, we just need to populate it with actual data.

Populating the production database Our MyBGList production database is almost ready, except it’s empty. To populate it, we have two choices:

Copy the data from the development database. Import the data source (the board games CSV file) from scratch.

The former approach could be doable, as long as we are 100 percent sure we want the exact same data that we currently have in our development database—including all the insert, delete, and update tests we did throughout this book; in practical terms, this is often a bad practice, as we would likely end up with altered info—not to mention some sensitive data that we hardly want to deploy to production, such as the passwords of the users that we created in chapter 9. For that reason, we will proceed with a brand-new CSV data import using our SeedController. More precisely, here’s what we need to do:

1. From Visual Studio’s Solution Explorer, select the /Data/bgg_dataset.csv file by left-clicking it.
2. From the Properties window, change the Copy to Output Directory value from Do Not Copy to Copy Always or Copy if Newer (both will work), as shown in figure 12.23.

Figure 12.23 Changing the Copy to Output Directory settings from the Visual Studio Properties window

Doing this will ensure that the /Data/bgg_dataset.csv file will be copied in the MyBGList web app’s production folder, thus allowing the SeedController’s BoardGameData method to find and use it to populate the database. We have to temporarily comment out the [Authorize] attribute that we placed on that controller in chapter 9 to be able to call it, as it is currently restricted to administrators.

After having temporarily commented and/or enabled what we need, we can execute the publishing profile again to re-publish and redeploy the web app; once done, we can perform the seeding tasks. Right after that, we can take the chance to also create some users using the POST /account/register endpoint, as well as assign them to the roles of our choice using the POST /Seed/AuthData endpoint, possibly changing its current implementation to match our new users’ names.

Once we do that, we will be able to perform as many code and configuration changes as we want, republishing our project at will to have them instantly deployed in production until we are done. That’s a great way to experience the advantages of the one-click deployment mechanic we’ve worked so hard to implement.

How to test our endpoints without SwaggerUI Calling non-GET endpoints (such as the PUT /Seed/BoardGameData and the POST /Seed/AuthData) in production can be a rather difficult task, considering we don’t have the SwaggerUI client available; to do that, I suggest using Postman (https://www.postman.com), a great web API client platform that allows to easily execute all kinds of HTTP requests. Alternatively, we could choose to temporarily enable the SwaggerUI in production; there won’t be security problems in doing that, assuming we’ll promptly disable it after performing the seeding tasks. However, if we opt for that route, we will also have to temporarily comment out the SeedController’s [ApiExplorerSettings] attribute which we added in chapter 11 to prevent that controller from being ignored by SwaggerUI.

When we are done with the database seeding tasks, we shouldn’t forget to roll back the temporary changes that we did. More precisely, we need to

1. Delete the /Data/bgg_dataset.csv file from the remote VM’s production folder (it should be c:\inetpub\MyBGList\).
2. Uncomment SeedController’s [Authorize] and/or [ApiExplorerSettings] attributes.
3. Disable the SwaggerUI in production in the Program.cs file (if we chose to temporarily enable it to execute the seed/ endpoints).

Doing this will restore our app’s security posture back to its former, production-ready state.

### 12.4.5 Final thoughts

Now that our MyBGList app has been published and deployed in production together with its database, our journey through ASP.NET Core web API development has finally come to an end. Rest assured, through these chapters we dedicated ourselves to laying down a solid foundation and learning the core concepts, while scratching only the surface of several long and complex topics that would have probably deserved much more time and space. Still, we have plenty of reasons to be satisfied with what we did; the results obtained and the lessons learned will definitely help us to build even better web APIs in the future. Most importantly, we hope you enjoyed this book. Many thanks for reading it!

Summary Before thinking about publishing, releasing, and deploying our web API, it’s important to define the overall security approach we want to stick to, as it will determine the whole architectural design of the production infrastructure. Choosing the domain name, the presence of intermediate proxies (such as a CDN service), and the SSL/TLS posture, are some of the decisions we must make before committing to the publishing tasks. A good practice when dealing with these choices is to adopt a security by design approach, meaning that we should always take the most secure route to fulfill our tasks while keeping a decent cost/benefit (and/or risk) tradeoff. Before publishing and deploying our web API, we need to ensure that it’s ready for release. Reviewing the configuration files, removing the unnecessary (or potentially vulnerable) middleware, and adding additional security countermeasures (such as the HTTP security headers) can greatly help to minimize the risk of malicious attacks, DDoS attempts, and data breaches. Using a content-delivery network (CDN) to serve our content instead of directly exposing our web hosting server has several advantages, not only in terms of caching and performance yet also for security reasons. Among other things, most CDNs allow us to implement a secure edge-origin architectural pattern based upon a full encryption mode from our web hosting server to the end users’ browsers, thus further improving the overall security posture of our infrastructure. Creating a VM on the cloud is a rather easy task, especially if we can count on the powerful GUI-based configuration tools provided by most market-leading cloud platforms (such as Azure) that allows us to build and configure a VM in few minutes. However, before doing that it’s important to acquire the required know-how to avoid some common security pitfalls, especially when dealing with some network-related tasks such as opening the TCP ports required by services like RDP and Web Deploy. Visual Studio allows to easily release and deploy a web app thanks to its built-in publishing features and GUI-based, wizard- like tools that can greatly simplify the whole release and deployment process. As for the database, the dotnet-ef CLI offers a wide set of alternatives to replicate the development database structure in production. Last but not least, the data seeding tasks can be handled by our SeedController, which can be temporarily tweaked to work in production by taking advantage of the one-click deployment capabilities of the Visual Studio publish profile.

appendix A.

A.1 Installing SQL Server The SQL Server installer for Windows can be downloaded from http://mng.bz/GRmN. This page also provides detailed instructions for installing SQL Server on Linux, with specific guides for RHEL, Ubuntu, and SUSE.

NOTE For this book, we’re going to install the Express edition, which comes with no usage limitation. If you want to use the Developer edition, download it instead; the editions behave in the same way for all our needs.

Installing SQL Server on Windows is a wizard-guided, straightforward process that should pose no problems. Be sure to allow the installation tool to update the machine’s firewall rules or prevent it from doing that, depending on whether you want the database management system (DBMS) to be accessible to the outside. When the first part of the installation completes, the wizard will ask some additional questions that can be answered in the following way:

Instance Name—I suggest setting it to SQLExpress, the name we’ll be using throughout this book. Instance ID—I suggest setting it to SQLEXPRESS for the same reason. If you want to change it, be sure to remember your choice. We’re going to need this value when we write our connection string. Authentication Mode—Choose one of the following options: Windows authentication—If we want unrestricted access to the database engine only from the local machine (using our Windows credentials). This option is better for security purposes but lacks some versatility because it won’t allow us to administer the database remotely. Mixed Mode—To enable the SQL Server system administrator (the sa user) and set a password for it.

SQL Server installation references If you need a more comprehensive guide to installing the SQL Server local instance on a Windows or Linux machine, take a look at the following tutorials:

http://mng.bz/zmDZ http://mng.bz/0yJz

A.2 Installing Internet Information Services Internet Information Services (IIS) is an optional Windows component, part of the software shipped with the OS but typically not installed by default. The IIS installation process can be started in one of the following ways:

Opening the Server Manager and selecting Add Roles and Features Opening the Control Panel, selecting Program and Features, and turning Windows features on and off

Both paths will take us to the Add Roles and Features wizard, which we need to follow to the Server Roles panel, where we can select Web Server (IIS) (figure A.1).
Figure A.1 Adding the Web Server (IIS) role

Ensure that the Include Management Tools check box is checked, and click Add Features to continue. A.3 Installing the IIS Management Service Advance the wizard up to the Role Services tab, and select the Management Service check box before continuing (as shown in
Figure A.2). This service is required for the Web Deploy component to work.
Figure A.2 Installing the IIS Management Service Click Next, and complete the wizard, leaving the other options at their default values until reaching the Confirmation tab. Then click the Install button and wait for the IIS installation to complete.

A.4 Installing the ASP.NET Core Windows hosting bundle The ASP.NET Core Windows hosting bundle is available as a free download at http://mng.bz/Klzn. Be sure to pick the ASP.NET Core 6.0.11 Runtime—Windows Hosting Bundle installer package for Windows x64, as shown in figure A.3.

NOTE It’s strongly advisable to install the .NET runtime after installing IIS because the package bundle will perform some modifications to the IIS default configuration settings.
Figure A.3 Downloading the ASP.NET Core Hosting Bundle for Windows

The installation process is straightforward and should pose no problems, but it’s strongly advisable to restart the IIS service after it completes to ensure that it will load the newly added settings. To do this, open a command prompt window with administrative rights and execute the following console commands:

> net stop w3svc /y > net start w3svc These commands will stop and restart the World Wide Web publishing service.

A.5 Installing Web Deploy To install the Web Deploy component, download the installer from http://mng.bz/91E8. The installation process is straightforward and should pose no problems. Be sure to select the Complete installation option to ensure that all the required components and prerequisites are installed as well, as the required IIS Deployment Handler feature is not selected by default.

NOTE We can download the installer directly on the virtual machine (VM) server or download it locally and then copy/paste it using the RDP client.

A.6 Adding an inbound port rule in Azure From the Azure Portal, access the VM configuration panel, and navigate to the Networking page (figure A.4).
Figure A.4 Azure VM’s inbound port rules

On that page, we see the inbound and outbound port rules currently configured for our Azure VM; as shown in the figure, the Windows VM created in chapter 12 starts with TCP ports 80 (HTTP) and 443 (HTTPS) open to inbound traffic for anyone, and nothing more. To add a TCP port to that list, click the Add inbound port rule and set up a new rule with the following settings:

Source—Your development machine’s public IP address or mask (Any is not recommended for security reasons) Source port ranges—*(Asterisk—meaning that we will accept any source port) Destination—Any Service—Custom Destination port ranges—The number of the port we want to open for inbound traffic (3389 for RDP, 8172 for the management service used by Web Deploy) Protocol—TCP Action—Allow Priority—Leave the default number Name—The name of the service (RDP or Web Deploy)

WARNING Publicly exposing a TCP port such as 3389 or 8172 to the internet is a nontrivial security problem that is recommended only for short-lived testing purposes. For production environments, it’s highly recommended to keep those ports closed or restrict their source to the IP addresses of a few authorized deployment machines.

When ready, click the Add button to add the new rule, thus making the TCP port accessible from the internet.
