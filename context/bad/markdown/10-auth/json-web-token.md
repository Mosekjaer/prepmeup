---
title: JSON Web Token
source: JSON Web Token.pdf
course_week: 10
topic: Autentifikation og autorisation
---

# JSON Web Token

## What is a JSON Web Token?

- JWT is an open standard (RFC 7519) that defines a compact and self-contained way to securely transmit information between parties as a JSON object.
- The information can be verified and trusted because it is digitally signed:
  - JWTs can be signed using a secret (HMAC + SHA256), or
  - a public/private key pair using RSA or ECDSA (Elliptic Curve).
- **Compact**: because of its size it can be sent through a URL, a POST parameter, or inside an HTTP header.
- **Self-contained**: the payload contains all required information about the user, avoiding repeated database queries.

## The JWT structure

A JWT consists of three parts separated by dots (`.`):

1. Header
2. Payload
3. Signature

## JWT header (aka JOSE header)

Typically two parts: the token type (JWT) and the hashing algorithm (e.g. HMAC SHA256 or RSA):

```json
{
    "alg": "HS256",
    "typ": "JWT"
}
```

This JSON is Base64Url-encoded to form the first part of the JWT.

## JWT payload

- The payload contains the **claims** — statements about an entity (typically the user) plus additional metadata.
- Three types of claims: reserved, public and private.

### Reserved claims

Predefined claims — not mandatory, but recommended; provide a set of useful, interoperable claims:

- `iss` (issuer)
- `exp` (expiration time)
- `sub` (subject)
- `aud` (audience)
- `iat` (issued at)
- `nbf` (not before)
- `jti` (JWT ID)

### Public claims

- Can be defined at will by those using JWTs.
- To avoid collisions, they should be defined in the IANA JSON Web Token Registry or as a URI containing a collision-resistant namespace.

### Private claims

Custom claims created to share information between parties that agree on using them. Example payload:

```json
{
    "sub": "1234567890",
    "name": "John Doe",
    "admin": true
}
```

This JSON is Base64Url-encoded to form the second part of the JWT.

## Signature

To create the signature, take the encoded header, the encoded payload, a secret, and the algorithm specified in the header — and sign that. With HMAC SHA256:

```
HMACSHA256(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload),
  secret)
```

The signature verifies that the sender of the JWT is who it says it is and that the message wasn't changed along the way.

## The output JWT

The output is three Base64 strings separated by dots, easily passed in HTML and HTTP environments: the encoded header and payload, signed with a secret.

## How do JSON Web Tokens work?

- The user logs in with their credentials; a JWT is returned and must be saved locally (typically in local storage).
- Whenever the user wants to access a protected route or resource, the app sends the JWT, typically in the `Authorization` header using the Bearer schema:

```
Authorization: Bearer <token>
```

## JWS vs JWE

A JWT is usually complemented with a signature or encryption, handled in their own specs:

- **JSON Web Signature (JWS)** — allows a JWT to be validated against modifications.
- **JSON Web Encryption (JWE)** — makes sure the content of the JWT is only readable by certain parties.

Most JWTs are just signed. The most common algorithms:

- HMAC + SHA256 — the most common algorithm for signed JWTs
- RSASSA-PKCS1-v1_5 + SHA256 — asymmetric with private/public key pair
- ECDSA + P-256 + SHA256 — asymmetric with private/public key pair

## JWT vulnerabilities

Some libraries have had critical vulnerabilities allowing attackers to bypass the verification step.

### Trying to verify a token

- To verify a signature, we first need to know which algorithm was used — the `alg` field in the header tells us.
- But we haven't validated the token yet, which means we haven't validated the header!
- Awkward position: to validate the token, we must allow attackers to select which method we use to verify the signature.

### The "none" algorithm

- The `none` algorithm is intended for situations where the integrity of the token has already been verified.
- It is one of only two algorithms that are mandatory to implement (the other being HS256).
- Some libraries treated tokens signed with the `none` algorithm as valid tokens with a verified signature.
- The result: anyone could create their own "signed" tokens with any payload, allowing arbitrary account access on some systems.

### The "none" algorithm check

Most implementations now have a basic check: if a secret key was provided, token verification fails for tokens using the `none` algorithm.

```
verify(clientToken, serverHMACSecretKey)
```

## Asymmetric signing algorithms

- The JWT spec also defines asymmetric signing algorithms (based on RSA or ECDSA).
- Tokens are created and signed using a **private key**, but verified using the corresponding **public key**.
- Publish the public key but keep the private key secret: only you can sign tokens, but anyone can check if a token is correctly signed.

### JWT library implementations

Typical library implementation (sometimes called "decode"):

```
verify(string token, string verificationKey)
// returns payload if valid token, else throws an error
```

- HMAC systems: `verificationKey` is the server's secret signing key — `verify(clientToken, serverHMACSecretKey)`
- Asymmetric systems: `verificationKey` is the public key — `verify(clientToken, serverRSAPublicKey)`

Unfortunately, an attacker can abuse this.

### Abuse of asymmetric signing algorithms

If a server expects a token signed with RSA but actually receives a token signed with HMAC, it will treat the public key as an HMAC secret key.

Hacker's how-to:

1. Grab your favourite JWT library and choose a payload for your token
2. Get the public key used on the server as a verification key
3. Sign your token using the public key as an HMAC key:

```
forgedToken = sign(tokenPayload, 'HS256', serverRSAPublicKey)
```

### Recommendation

The server and client should already know what algorithm was used to sign tokens; it's not safe to let attackers provide this value. **Never use the algorithm specified in the header of the JWT!**

```
verify(string token,
       string algorithm,
       string verificationKey)
```

## References & links

- JSON Web Tokens: http://jwt.io/
- Epoch & Unix Timestamp Conversion Tools: https://www.epochconverter.com/
- JWT signing algorithms overview: https://auth0.com/blog/json-web-token-signing-algorithms-overview/
- RFC 7519: JSON Web Token (JWT): https://tools.ietf.org/html/rfc7519
- RFC 7518: Cryptographic Algorithms for Digital Signatures and MACs: https://tools.ietf.org/html/rfc7518#section-3
- Critical vulnerabilities in JWT libraries: https://auth0.com/blog/2015/03/31/critical-vulnerabilities-in-json-web-token-libraries/
- JWT implementation for .NET: https://github.com/jwt-dotnet/jwt
