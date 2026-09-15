---
title: REST Principles
source: REST principals.pdf
course_week: 4
topic: Web APIs + REST + Dapper
---

# REST Principles

Lecture (SW4BAD, 16 Feb 2026, Jacob Brinth Assenholm). Agenda: What is REST, the 6 guiding constraints, CORS, configuring CORS in ASP.NET, reverse proxies, CDN services, API versioning with OpenAPI/Scalar.

## What is REST?

REST = **RE**presentational **S**tate **T**ransfer. A REST API is an API that conforms to the design principles of the REST architectural style. At its core, REST revolves around the idea of **resources** — any piece of information like a user, product, document, or collection of items.

## The 6 guiding constraints

1. **Client/Server** — separation of concerns.
2. **Stateless** — each request must contain all information needed to fulfill it.
3. **Cacheable** — responses declare cache rules so clients/proxies can reuse responses safely.
4. **Uniform Interface** — consistent URL paths, standardized HTTP methods, and status codes.
5. **Layered System** — a client can't assume it is talking directly to the origin server; there may be proxies, gateways, CDNs.
6. **Code-on-demand** (optional).

## Client/Server

UI/client concerns are separated from data/server concerns. The API returns JSON (server), while a SPA/mobile app handles UI (client).

Request/response flow:

1. **Client sends request** — a client application (mobile app, browser, or another server) sends a request to a specific URL with an HTTP method, headers with metadata, and sometimes a request body.
2. **Server processes request** — the server validates the request, confirms the client is authenticated and authorized, then processes it (retrieve, create, or update a resource).
3. **Server sends response** — includes an HTTP status code (e.g. 200 OK, 404 Not Found, 401 Unauthorized) plus the requested data in the body.
4. **Data transfer** — the transferred data is a *representation* of the state of the resource. JSON is the most common format: lightweight, human-readable, easy to parse.

## Statelessness

Every request must contain all the information the server needs to process it. The server stores no client context or session state between requests. This makes REST APIs highly scalable, because any server can handle any request.

## Cacheable

Responses should be defined as cacheable or non-cacheable. When cacheable, a client or intermediary can reuse the response for subsequent identical requests — improving performance and reducing server load.

## Uniform Interface

This is IMPORTANT (where most APIs fail):

- Use resources (**nouns**), not actions (verbs)
- Standard HTTP methods: GET, POST, PUT, PATCH, DELETE
- Status codes: 200, 201, 204, 400, 404, 409, 422
- Content negotiation (typically JSON)

## Layered System

A client can't assume it is talking directly to the origin server. There may be proxies, gateways, CDNs in between.

## Reverse Proxy

Typical responsibilities:

- TLS termination
- Load balancing
- Request buffering
- Header rewriting
- Rate limiting / WAF
- Observability (logs/trace IDs)

## CDN — Content Delivery Network

- Geographically distributed group of servers
- Caches website content closer to end-users
- Significantly reduces latency and speeds up page load times
- Improves performance, enhances security against DDoS attacks, reduces bandwidth costs

## CORS — Cross-Origin Resource Sharing

- CORS is a protocol that allows servers to receive requests from different domains.
- Cross-domain requests in JavaScript are restricted by the **same-origin policy** — a security standard enforced by the browser: scripts loaded on one domain can only request resources originating from the same domain.
- A cross-origin request is usually from one domain to another, but can also be a different protocol (http vs https) or a different port on the same server.
- Applies when using `fetch` or `XMLHttpRequest` to make AJAX requests.

## Setting up CORS in ASP.NET Web API

Done in `Program.cs` during startup:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("https://localhost:5173") // the host of our frontend app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// ...
app.UseCors("Frontend");
```

## A note about middleware ordering

When setting up the API in `Program.cs`, the order of **services** doesn't really matter — but the order of **middleware** matters, since requests are piped through them: middleware1 → middleware2 → middleware3.

General rules:

- Exception handlers go first.
- CORS must run before endpoints:
  ```csharp
  app.UseCors();
  app.MapControllers();
  ```
- Authentication before Authorization:
  ```csharp
  app.UseAuthentication();
  app.UseAuthorization();
  ```
- Forwarded Headers must be EARLY (reverse proxy scenario):
  ```csharp
  app.UseForwardedHeaders();
  app.UseHttpsRedirection();
  ```
- Static files usually before auth (if public):
  ```csharp
  app.UseStaticFiles();
  ```

## API versioning

We version our API so we don't break it for existing clients. Typical reasons:

- Breaking response shape (rename/remove fields, change types)
- Breaking request contract (new required field, different validation rules)
- Behavior changes clients rely on (sorting, defaults, auth rules)

Common versioning styles:

- URL segment: `/api/v1/authors`
- Query string: `/api/authors?api-version=1.0`
- Header: `X-Api-Version: 1.0`

### Good practices

- Keep old versions working for a defined period
- Communicate deprecation (docs + headers like "sunset" policies)
- Make versioning visible in OpenAPI so clients know what exists
