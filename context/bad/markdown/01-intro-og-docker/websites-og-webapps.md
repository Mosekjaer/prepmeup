---
title: Web Sites and Web Apps
source: WebSitesAndWebApps.pdf
course_week: 1
topic: Intro til backend + Docker
---

# Web Sites and Web Apps

An introduction to the world of web-site and web-app development.

## Agenda

- The basic Web architecture: the origins of the WWW, HTML, URL, HTTP, the web server, the browser
- Web enhancements: dynamic HTML generation, CSS, Java applets, JavaScript, Ajax / Web API
- Web dev tools

## The origins of the WWW

WWW was invented by Tim Berners-Lee (a physicist) at CERN in 1989–1992. Main purpose: **hypertext across the Internet** (replacing FTP).

Five constituents:

| Constituent | Role |
|---|---|
| HTML | Mark-up language for hypertext |
| URL | Notation for locating files on servers |
| HTTP/HTTPS | High-level protocol for data/file transfers |
| Web server | Sends data/a file as an HTTP response when requested |
| Browser | Receives HTML documents and renders them as visible pages |

## The origin of HTML

- HTML is an acronym for HyperText Mark-up Language.
- HTML 1.0 was a simplification of SGML (Standard Generalized Markup Language) with the addition of the Link element.

| Year | Version |
|---|---|
| 1991 | HTML Tags, an informal CERN document, first mentioned in public |
| 1992 | HTML 1.0 — first informal draft of the HTML standard (Tim Berners-Lee proposal) |
| 1995 | HTML 2.0 published as IETF RFC 1866 |
| 1996 | The HTML standard is now developed by W3C |
| 1997 | HTML 3.2 published as a W3C Recommendation; the Browser War ends |
| 1997/98 | HTML 4.0 — style sheets are introduced (CSS) |
| 2000–02 | XHTML 1.0 published as a W3C Recommendation (an XML version of HTML 4.01) |
| 2008 | HTML5 published as a Working Draft by the W3C |
| 2014 | HTML5 published as a W3C Recommendation |
| 2016 | HTML5.1 published as a W3C Recommendation |
| 2017 | HTML5.2 published as a W3C Recommendation |

HTML is now defined by the **WHATWG** (Web Hypertext Application Technology Working Group), an organization formed by people working on the most popular web browsers — the "HTML Living Standard" (https://whatwg.org/).

## HTML

HTML describes the logical structure of a document and uses tags `<tag>` to structure the text:

```html
<html>
  <head>
    <title>SW4FED</title>
  </head>
  <body>
    <h1>Front end</h1>
    <h2>SW4FED</h2>
    <p>Til Web-applikationer anvendes <b>HTML</b>,
       CSS og Javascript.</p>
  </body>
</html>
```

## Uniform Resource Locator — URL

A Web resource is located by a URL:

```
http://www.ece.au.dk:1234/path/file.html?x=2&y=7
└──┬─┘ └─────┬──────┘└─┬─┘└──────┬─────┘└──┬───┘
scheme     server     port      path      query
```

- Default ports: 80 (http), 443 (https).
- A relative URL: `path/file.htm`
- Fragment identifier: `https://www.ece.au.dk/path/file.html/#section4` — `#section4` is the fragment id.

## URI

- URLs are a subset of the more general concept of Uniform Resource Identifiers (URIs).
- The general URI syntax is very flexible: `scheme:scheme-specific-part`
- Many schemes are defined besides `http`: `ftp`, `file`, `mailto`, `imap`, `https`, `dict`, `geo`, ...
- The official register of schemes is maintained by IANA: http://www.iana.org/assignments/uri-schemes.html

## URL rules

- `/` implies a hierarchical structure.
- `?` separates the queryable resource from the query string.
- `#` separates a fragment identifier from the URI.
- Special symbols are escaped with the notation `%NN` (NN is the character's hexadecimal code), e.g. `%20` is space.

## HTTP — HyperText Transfer Protocol

- Client-Server model following a **Request-Response** pattern.
- Version 1.1 (RFC 2068) from 1997.
- HTTP/2 (RFC 7540) from May 2015.
- Current version is HTTP/3: on 6 June 2022, IETF published HTTP/3 as a Proposed Standard in RFC 9114. It is already supported by 75% of running web browsers and, according to W3Techs, 26% of the top 10 million websites.

Diagram (described): a browser client sends an *HTTP Request* to a web server; the server answers with an *HTTP Response*.

## HTTP/2

- Specification published as RFC 7540 in May 2015, developed by the httpbis working group of the IETF.
- Goals:
  - **Asynchronous connection multiplexing** — a bottleneck of HTTP/1.1 implementations is that HTTP relies on multiple connections for concurrency.
  - **Header compression** — reduces overhead.
  - **Server push** — allows the server to supply data it knows a web browser will need.
- Result: page load speedups ranging from 11.81% to 47.7%.
- Backwards compatible with the semantics of HTTP/1.1 — what changed is how the data is framed and transported between client and server.

## HTTP/3

- Both HTTP/1.1 and HTTP/2 use TCP as their transport; HTTP/3 uses **QUIC**.
- QUIC is a transport-layer network protocol using user-space congestion control over UDP.
- The switch to QUIC aims to fix a major problem of HTTP/2 called **head-of-line blocking**: because the parallel nature of HTTP/2's multiplexing is not visible to TCP's loss recovery mechanisms, a lost or reordered packet causes all active transactions to stall regardless of whether that transaction was impacted by the lost packet.
- Because QUIC provides native multiplexing, lost packets only impact the streams where data has been lost.

Ref: https://en.wikipedia.org/wiki/HTTP/3

## Network layers

Both browser and server run the same stack:

| Layer | Protocol |
|---|---|
| Application layer | HTTP |
| Transport layer | TCP or QUIC+UDP |
| Internet layer | IP |
| Link layer | MAC (Ethernet, WiFi, ...) |

## HTTP verbs

- The client submits an HTTP request message to the server; the server returns a response message.
- The response contains completion status information about the request and may also contain requested content (e.g. an HTML document) in its message body.
- HTTP is a simple protocol with only a few request methods (verbs):

| Verb | Meaning |
|---|---|
| GET | Fetch an existing resource |
| POST | Create a new resource |
| PUT | Update an existing resource |
| DELETE | Delete an existing resource |

...and a few others.

## HTTP status codes

The client initiates requests with URLs and verbs; the server responds with status codes and message payloads. Extract:

- **1xx: Informational messages**
- **2xx: Successful**
  - 200 OK
  - 201 Created
  - 204 No Content
- **3xx: Redirection**
  - 301 Moved Permanently
  - 302 Found / Moved Temporarily
  - 304 Not Modified
- **4xx: Client Error**
  - 400 Bad Request
  - 401 Unauthorized
  - 403 Forbidden
  - 404 Not Found
- **5xx: Server Error**
  - 500 Internal Server Error
  - 503 Service Unavailable

## Web client

- Connected to the Internet when needed.
- Usually a web browser (such as Chrome, Firefox, Edge or Safari).
- Uses HTTP/HTTPS.
- Requests web pages, files or data from the server.
- Renders the received HTML on the screen.

## Web server

- Continually connected to the Internet.
- Runs web server software (such as Apache, Internet Information Server, Kestrel or Node.js).
- Uses HTTP.
- Receives requests for web pages.
- Responds to requests and transmits status code, web page, and associated files or data.

## Web enhancements

### Dynamic web pages

- Dynamic web pages are web sites generated at the time of access by a user, or that change as a result of interaction with the user.
- A program running on the web server (server-side scripting) is used to change the web content sent back to the client.
- Typical server-side languages: PHP, JSP, Perl, Ruby, C# (Razor), Java, Go and JavaScript.

### Dynamic HTML generation

- Dynamic web pages usually consist of a static part (HTML) and a dynamic part — code that generates HTML.
- The code can generate HTML based on variables in a template, or on code.
- The generated text can come from a database, dramatically reducing the number of pages in a site.

Example: a real estate agent with 500 houses for sale. In a static web site, the agent would have to create 500 web pages. In a dynamic website, the agent could connect a single dynamic web page to a database table of 500 records.

### Client-side scripting

- Client-side scripting changes interface behaviors within a specific web page in response to mouse or keyboard actions, or at specified timing events.
- The dynamic behavior occurs within the presentation; the client-side content is generated on the user's local computer system.
- The client-side scripting language is JavaScript.

### Ajax / Web API

- HTTP is not just for serving up web pages — it is also a powerful platform for building APIs that expose services and data.
- HTTP services can reach a broad range of clients: browsers, SPAs, mobile devices, embedded systems and traditional desktop applications.
- All modern browsers have a global `fetch` method for initiating asynchronous resource requests (AJAX calls).

## Web dev tools

### Browsers

- Web applications are typically developed to target all/most browsers on different platforms.
- Test your pages/apps in different browsers and platforms: install all the common browsers on your development machine and use Internet services to visualize your pages on the remaining browsers and platforms.

### Editor or IDE?

- On Windows: MS Visual Studio, Visual Studio Code, or a simple editor like Notepad++.
- All platforms (Windows, Mac, Linux): Visual Studio Code, JetBrains WebStorm, Brackets, Sublime Text, Atom, ...

### Validation

Some IDEs have integrated validation of HTML and CSS; otherwise use online services:

- HTML validation: http://validator.w3.org/ , http://html5.validator.nu , http://lint.brihten.com/html
- CSS validation: http://jigsaw.w3.org/css-validator/
- JavaScript validation: https://eslint.org/

### Debugging

- Most browsers have a debugging aid built in (press F12 or choose Inspect).
- Google Chrome's DevTools are probably the best: https://developers.google.com/chrome-developer-tools/
- Sometimes it is useful to use different browsers for debugging.

### Testing environment

- For static web pages you only need browsers.
- If you use a framework like ASP.NET, PHP etc., you need a local web server for test and debugging.
- If you use Visual Studio it will install IIS Express locally, or you can use Kestrel.

## References & links

- Wikipedia
- Web Development and Design Foundations with HTML5: http://webdevfoundations.net/
