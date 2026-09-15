---
title: ASP Security (HTTPS) — week 11
source: week 11 - ASP Security.pdf
course_week: 10-11
topic: Release, deployment og security
---

# ASP Security — HTTPS (week 11)

Note: this deck ("week 11 - ASP Security") is an earlier version of the same HTTPS deck as `86 HTTPS.pdf` (PDF title: "HTTPS"). Content is identical apart from the network-layering slide and the Darknet Diaries reference, which only appear in the later version. See `https.md` for the full annotated version. The substance is repeated here so this file stands alone.

## What and Why?

- HTTPS consists of communication over HTTP within a connection encrypted by Transport Layer Security (TLS) — or its predecessor, Secure Sockets Layer (SSL)
- HTTPS is not a separate protocol, but refers to use of ordinary HTTP over an encrypted SSL/TLS connection
- The main motivation for HTTPS is authentication of the visited website and protection of the privacy and integrity of the exchanged data
  - protects against man-in-the-middle attacks

## Difference from HTTP

- HTTPS URLs begin with `https://`, port 443 by default; HTTP URLs begin with `http://`, port 80 by default
- HTTP is not encrypted and is vulnerable to man-in-the-middle and eavesdropping attacks — attackers can gain access to website accounts and sensitive information, and modify webpages to inject malware or advertisements
- HTTPS is designed to withstand such attacks and is considered secure against them, with the exception of older, deprecated versions of SSL

## What is encrypted?

- Because HTTPS piggybacks HTTP entirely on top of TLS, the entirety of the underlying HTTP protocol can be encrypted:
  - the request URL (but not the IP address and port), query parameters, headers, cookies, body
- Host addresses and port numbers are necessarily part of the underlying TCP/IP protocols, so HTTPS cannot protect their disclosure

## Other advantages

- Ability to use JavaScript browser APIs: location APIs, microphone APIs, storage APIs
- Search engine indexing
- Industry heading this way
- Increased app performance: HTTP/2 and HTTP/3, gRPC

## Certificate authorities

- Browsers trust HTTPS websites based on certificate authorities (CA) that come pre-installed in their software — based upon the X.509 standard
- CAs are trusted by web browser creators to provide valid certificates
- Examples: IdenTrust, Let's Encrypt, GoDaddy, and others (https://en.wikipedia.org/wiki/Certificate_authority)
- A certificate uses two parts, public and private key — but also consists of one or more certificates from a third party, a CA
- Browsers are hardcoded to trust CAs

## Trustworthy HTTPS

A user should trust an HTTPS connection to a website if and only if all of the following are true:

- The user trusts that the browser software correctly implements HTTPS with correctly pre-installed certificate authorities
- The user trusts the certificate authority to vouch only for legitimate websites
- The website provides a valid certificate, signed by a trusted authority
- The certificate correctly identifies the website (e.g. the certificate for `https://example.com` is properly for `example.com` and not some other entity)
- The user trusts that the protocol's encryption layer (SSL/TLS) is sufficiently secure against eavesdroppers

## Server setup

- To accept HTTPS connections, the administrator must create a public key certificate for the web server
- The certificate must be signed by a trusted certificate authority for the browser to accept it without warning
  - The authority certifies that the certificate holder is the operator of the web server that presents it (owns the domain name)

## Acquiring certificates

- Authoritatively signed certificates may be free or cost 8–70 USD per year (2012–2014 figures)
- Let's Encrypt (launched April 2016) provides free and automated SSL/TLS certificates

## HTTPS in ASP.NET

- ASP.NET has middleware to set up and enforce HTTPS communication with your application
- Websites should handle HTTP as well as HTTPS
- APIs should reject HTTP and only accept HTTPS
- Use security headers to enforce HTTPS communication, for instance HSTS

## HSTS

- HSTS is a web security policy mechanism to mitigate protocol downgrade attacks and cookie hijacking
- HSTS effectively forces the client's browser to direct all traffic through HTTPS — a "secure or not at all" ideology!

## Use HSTS with HTTPS redirection

1. The browser sends an initial request over HTTP (`http://myapp.com/path`)
2. The app immediately redirects (`307 Temporary Redirect`) the browser to send an HTTPS request to the same path
3. The browser resends the request over HTTPS (`https://myapp.com/path`)
4. The app responds and includes an HSTS header (`strict-transport-security`) in the response
5. Thanks to the HSTS header, all subsequent requests are sent over HTTPS. If a user attempts an HTTP request, the browser automatically aborts it and makes an HTTPS request instead

## References & Links

- https://letsencrypt.org/
- https://hstspreload.org/
- https://en.wikipedia.org/wiki/HTTPS
- https://www.youtube.com/watch?v=UwJX32UOJyU
- https://www.youtube.com/watch?v=E5FEqGYLL0o
