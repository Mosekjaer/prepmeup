---
title: Web API Testing overview
source: Web API Testing overview.pdf
course_week: 8
topic: Testing af Web APIs
---

# How to test a Web API?

Overview, types and benefits of automated API testing.

## The test pyramid (C. Horsdal's version)

What to test in a microservices system? Horsdal's version of the test pyramid has three levels:

- **System tests (top level)** — tests that span the complete system of microservices, usually implemented through the GUI. Aka end-to-end tests.
- **Service tests (middle level)** — tests that work against one, but only one, complete microservice. Aka API testing.
- **Unit tests (bottom level)** — tests that test one small piece of functionality in a microservice. Unit tests call code in the microservice under test in-process and usually involve only part of a microservice. Aka integration tests.

## Selected types of API tests

- **Functional testing** — does the API work as expected?
- **Load testing** — can the API handle a certain volume (API calls per second or other)?
- **Security testing** — makes sure the API is secure from unwanted usage.
- **Fuzz testing** — test using invalid or random input parameters to make sure the API can handle these situations.

More types exist; the lesson focuses on functional testing.

## Load testing

Example requirement: "The server can handle 100 clients simultaneously." A client/tester tool sends many concurrent requests against the server. Postman can be used for load testing (performance runs).

## Security testing

Two main concerns covered:

- **SQL injection** — malicious input in a query parameter, e.g. an input that turns a filtered query into one returning ALL users in the database. To prevent this, sanitize input. EF Core does this by parameterizing all input (input is never executed as SQL).
- **Authorization** — restricted access to endpoints (also a separate topic in SW4BAD).

## Fuzz testing

Typical fuzz inputs:

1. **Boundary values** — (-1), 0, 1000000000
2. **Special characters** — including emoji
3. **Long strings** — excessively long inputs beyond typical limits
4. **Invalid formats** — e.g. invalid dates

## Selected benefits of API testing

- API testing can usually be done early in the project — it does not depend on the UI being ready (fits TDD).
- Solid foundation — if you know your API works, the rest of the application becomes easier to test.
- Easier to make incremental changes to the API — some tests may need redefining, but the rest should still run.
- Tests can be written independently of the application: the implementation is "invisible" behind the API, so a test framework like Postman works no matter whether the backend is JavaScript, Java or C#.

## End-to-end tests with Cypress

For end-to-end/system tests through the GUI, Cypress is an option: https://docs.cypress.io/guides/end-to-end-testing/testing-your-app

## References & links

- Microservices in .NET, Second Edition (ISBN-13: 9781617297922) by Christian Horsdal Gammelgaard
- Test of a Web API by Nichlaes H. Sørensen
