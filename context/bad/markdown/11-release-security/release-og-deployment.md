---
title: Release and Deployment
source: Release and Deployment.pdf
course_week: 10-11
topic: Release, deployment og security
---

# Release and Deployment

## dotnet publish

- Builds a project for deployment
- Produces assembly files, dependencies and the .NET runtime (optional)
- Output located in `bin/publish`
- Possible to deploy directly to Azure with Visual Studio

## ASP.NET Core hosting model

- An ASP.NET Core app is running in a web server in a console application
  - Typically a Kestrel instance
- Kestrel provides the HTTP functionality
  - Receives requests and returns responses
  - Passes any request to the application itself to generate a response
  - The request passes through the middleware pipeline

Kestrel used as an edge (Internet-facing) web server:

```
Internet <--HTTPS--> [Kestrel] <--HttpContext--> [App code]
                     (inside the ASP.NET Core app)
```

## CDN as a security layer

- IP address masking
  - Acts as a proxy, thus hiding the IP address of the origin server
- Web Application Firewall (WAF)
- DDoS protection

(Building Web APIs with ASP.NET Core, Manning 2022, p. 409)

## Kestrel with a reverse proxy

Request flow through a reverse proxy (IIS/NGINX/Apache — or YARP: Yet Another Reverse Proxy):

1. HTTP request is made to the server and is received by the reverse proxy
2. Request is forwarded by IIS/NGINX/Apache to ASP.NET Core
3. ASP.NET Core web server (Kestrel) receives the HTTP request and passes it to the middleware
4. Request is processed by the application, which generates a response
5. Response passes through the middleware back to the web server
6. Web server forwards the response to the reverse proxy
7. HTTP response is sent to the browser

## Why use a reverse proxy?

- When Kestrel is used as an edge server without a reverse proxy, sharing of the same IP address and port among multiple processes is unsupported
- A reverse proxy:
  - Can limit the exposed public surface area of the apps that it hosts
  - Provides an additional layer of configuration and defense-in-depth cybersecurity
  - Performance (caching)
  - Simplifies load balancing and secure communication (HTTPS) configuration
    - Only the reverse proxy server requires the X.509 certificate for the public domain(s)
    - That server can communicate with the app's servers on the internal network using plain HTTP or HTTPS with locally managed certificates
      - Internal HTTPS increases security but adds significant overhead
- Downside: increased complexity

## Configure ASP.NET Core to work with proxy servers

Proxy servers often obscure information about the request before it reaches the app:

- When HTTPS requests are proxied over HTTP, the original scheme (HTTPS) is lost and must be forwarded in a header
- Because an app receives a request from the proxy and not its true source on the Internet or corporate network, the originating client IP address must also be forwarded in a header

## Forwarded headers

By convention, proxies forward information in HTTP headers:

| Header | Description |
|---|---|
| `X-Forwarded-For` (XFF) | Holds information about the client that initiated the request and subsequent proxies in a chain of proxies. May contain IP addresses and, optionally, port numbers |
| `X-Forwarded-Proto` (XFP) | The value of the originating scheme, HTTP or HTTPS |
| `X-Forwarded-Host` (XFH) | The original value of the Host header field. Usually, proxies don't modify the Host header |
| `X-Forwarded-Prefix` | The original base path requested by the client. Useful for applications to correctly generate URLs, redirects, or links back to the client |

The Forwarded Headers Middleware (`ForwardedHeadersMiddleware`) reads these headers and fills in the associated fields on `HttpContext`.

## Deployments

- **Runtime/framework-dependent**
  - Small deployments
  - Cross-platform
  - Uses the latest patched runtime
  - Requires .NET to be installed on the host
  - .NET may change
- **Self-contained**
  - Bundles .NET framework and application together
  - Platform-dependent
  - Full control over .NET version
  - Larger deployments
  - Harder to update the .NET version

## Preparing your application

It's important to take security into account before releasing your app — releasing opens up for external traffic. You should review:

- `appsettings.json`
- `Program.cs`

## Fine-tuning our app — appsettings

- Default settings: `appsettings.json`
- Development settings: `appsettings.Development.json`
- Production settings: `appsettings.Production.json`
- {Environment} settings: `appsettings.{environment}.json`
- `ASPNETCORE_ENVIRONMENT` decides which settings to use
  - Default is `Production`

## Use of different DB servers

- `appsettings.Development.json`
  - `connectionstring = "Server=localhost;Database=...;"`
- `appsettings.Production.json`
  - `connectionstring = "Server=servicename;Database=...;"`

## Fine-tuning our app — secrets

Production SECRETS should only be located on the production server:

- As a file
- As environment variables
- As whatever the hosting platform / cloud services provide

## Fine-tuning our app — Program.cs

Some features / middleware help us during development but are potential threats in production:

```csharp
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts(); // HTTP Security Headers
}

app.UseHttpsRedirection();
```

Other examples: developer exception pages.

## Use Docker for deployment

Docker Compose example with API + SQL Server, connection string and HTTPS certificate injected via environment variables:

```yaml
services:
  api:
    container_name: api
    image: poulejnar/sw4fed-modelsapi:v2
    ports:
      - "8080:8080"
      - "8081:8081"
    depends_on:
      feddb:
        condition: service_healthy
    environment:
      Connectionstrings__DefaultConnection: "Server=feddb;Database=FED-Assignment2;User Id=SA;Password=YourStrong@Passw0rd;MultipleActiveResultSets=True;TrustServerCertificate=True"
      ASPNETCORE_HTTPS_PORTS: 8081
      ASPNETCORE_Kestrel__Certificates__Default__Password: StrongPassw0rd!
      ASPNETCORE_Kestrel__Certificates__Default__Path: /https/ModelsApi.pfx
  feddb:
    container_name: feddb
    image: mcr.microsoft.com/mssql/server
    # More stuff
```

## References & Links

- Building Web APIs with ASP.NET Core, 2022 Manning Publications
- Kestrel web server in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
- When to use Kestrel with a reverse proxy: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/when-to-use-a-reverse-proxy
- Configure ASP.NET Core to work with proxy servers and load balancers: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer
