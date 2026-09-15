---
title: HTTPS
source: 86 HTTPS.pdf
course_week: 10-11
topic: Release, deployment og security
---

# HTTPS

## What and Why?

- HTTPS consists of communication over Hypertext Transfer Protocol (HTTP)
  - within a connection encrypted by Transport Layer Security (TLS)
  - or its predecessor, Secure Sockets Layer (SSL)
- HTTPS is not a separate protocol, but refers to use of ordinary HTTP over an encrypted SSL/TLS connection
- The main motivation for HTTPS is authentication of the visited website and protection of the privacy and integrity of the exchanged data
  - protects against man-in-the-middle attacks

## Difference from HTTP

- HTTPS URLs begin with `https://` and use port 443 by default
- HTTP URLs begin with `http://` and use port 80 by default
- HTTP is not encrypted and is vulnerable to man-in-the-middle and eavesdropping attacks, which can let attackers gain access to website accounts and sensitive information, and modify webpages to inject malware or advertisements
- HTTPS is designed to withstand such attacks and is considered secure against them
  - with the exception of older, deprecated versions of SSL

## What is encrypted?

- Because HTTPS piggybacks HTTP entirely on top of TLS, the entirety of the underlying HTTP protocol can be encrypted:
  - The request URL (but not the IP address and port)
  - Query parameters
  - Headers
  - Cookies
  - Body
- Because host addresses and port numbers are necessarily part of the underlying TCP/IP protocols, HTTPS cannot protect their disclosure

## Network layering (encapsulation)

TLS/HTTPS lives at the application layer; each layer below wraps the data:

- Application layer: Data (application data)
- Transport layer: TCP/UDP header + Data = TCP segment or UDP packet
- Network layer: IP header + TCP/UDP header + Data = IP datagram
- Data link layer: Frame header + IP header + TCP/UDP header + Data + Frame trailer = network frame
- Physical network

## Other advantages

- Ability to use JavaScript browser APIs, i.e. location APIs, microphone APIs, and storage APIs
- Search engine indexing
- Industry heading this way
- Increased app performance: HTTP/2 and HTTP/3, gRPC

## Certificate authorities

- Web browsers know how to trust HTTPS websites based on certificate authorities (CA) that come pre-installed in their software
  - based upon the X.509 standard
- Certificate authorities are in this way trusted by web browser creators to provide valid certificates
- Examples of certificate authorities:
  - IdenTrust
  - Let's Encrypt
  - GoDaddy
  - And others (https://en.wikipedia.org/wiki/Certificate_authority)
- A certificate uses two parts, public and private key
  - But also consists of one or more certificates from a third party — a CA
- Browsers are hardcoded to trust CAs

## Trustworthy HTTPS

A user should trust an HTTPS connection to a website if and only if all of the following are true:

- The user trusts that the browser software correctly implements HTTPS with correctly pre-installed certificate authorities
- The user trusts the certificate authority to vouch only for legitimate websites
- The website provides a valid certificate, which means it was signed by a trusted authority
- The certificate correctly identifies the website (e.g., when the browser visits `https://example.com`, the received certificate is properly for `example.com` and not some other entity)
- The user trusts that the protocol's encryption layer (SSL/TLS) is sufficiently secure against eavesdroppers

## Server setup

- To prepare a web server to accept HTTPS connections, the administrator must create a public key certificate for the web server
- This certificate must be signed by a trusted certificate authority for the web browser to accept it without warning
  - The authority certifies that the certificate holder is the operator of the web server that presents it (owns the domain name)

## Acquiring certificates

- Authoritatively signed certificates may be free or cost between 8 USD and 70 USD per year (in 2012–2014)
- Let's Encrypt (launched in April 2016) provides free and automated SSL/TLS certificates to websites

## HTTPS in ASP.NET

- ASP.NET has middleware to set up and enforce the usage of HTTPS communication with your application
- Websites should be able to handle HTTP as well as HTTPS
- APIs should reject HTTP and only accept HTTPS
- Use security headers to enforce HTTPS communication, for instance HSTS

## HSTS

- HSTS (HTTP Strict Transport Security) is a web security policy mechanism to mitigate protocol downgrade attacks and cookie hijacking
- HSTS effectively forces the client's browser to direct all traffic through HTTPS
  - a "secure or not at all" ideology!

## Use HSTS with HTTPS redirection

Flow:

1. The browser sends an initial request over HTTP (`http://myapp.com/path`)
2. The app immediately redirects the browser (`307 Temporary Redirect`) to send an HTTPS request to the same path
3. The browser resends the request over HTTPS (`https://myapp.com/path`)
4. The app responds and includes an HSTS header (`strict-transport-security`) in the response
5. Thanks to the HSTS header, all subsequent requests are sent over HTTPS. If a user attempts to make an HTTP request, the browser automatically aborts it and makes an HTTPS request instead

## References & Links

- https://letsencrypt.org/
- https://hstspreload.org/
- https://en.wikipedia.org/wiki/HTTPS
- https://www.youtube.com/watch?v=UwJX32UOJyU
- https://www.youtube.com/watch?v=E5FEqGYLL0o
- https://www.darknetdiaries.com/episode/3
