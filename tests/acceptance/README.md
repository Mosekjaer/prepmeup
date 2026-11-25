# Acceptance tests (Postman)

`prepmeup-fr-02.postman_collection.json` runs the API part of the FR-02 acceptance test (appendix 6.1). `prepmeup-fr-03.postman_collection.json`, `prepmeup-fr-05.postman_collection.json`, `prepmeup-fr-06.postman_collection.json` and `prepmeup-fr-19.postman_collection.json` run the API part of FR-03, FR-05, FR-06 and FR-19 (see the end of this file). The collections hold no tokens or passwords.

## Prerequisites

- The stack runs locally: `make up` (needs `MSSQL_SA_PASSWORD` and `KEYCLOAK_ADMIN_PASSWORD` in `.env`).
- A test user is registered at http://localhost:8082/realms/prepmeup/account.
- Use a fresh database (`make db-reset`) or a user without a subscription, since `POST /subscription` answers `409 Conflict` for a second run.

## Run

1. Import the collection in Postman and open its Authorization tab. Press **Get New Access Token**, sign in as the test user and press **Use Token**. The token is valid for 15 minutes and is not saved in the file.
2. Run the folder **Before restart** (requests 1 to 4). The first order is created with two package types, which request 2 reads from `GET /package-types`.
3. Restart the database, wait until it is healthy, and restart the API:
   ```
   docker compose restart db
   docker compose restart api
   ```
   The API runs the migrations when it starts, so it must start after the database.
4. Get a new token if the old one has expired, and run the folder **After restart** (requests 5 and 6). The tests compare with the values saved in step 2, including the order lines.

## FR-05: next orders to send

`prepmeup-fr-05.postman_collection.json` checks that the administrator sees the planned orders sorted by date, and that a customer is refused. The orders need dates relative to today and one sent order, which cannot be created through the API, so they are put into the database with `seed-fr-05.sql`.

### Prerequisites

- The stack runs locally (`make up`), on a database where the migrations have run.
- A user with the admin role and a user without it (a customer) are registered in Keycloak.

### Run

1. Load the test data. The script deletes and recreates the four test customers (key `fr05-...`) and leaves other data alone, so it can be run again before every run:
   ```
   docker compose cp tests/acceptance/seed-fr-05.sql db:/tmp/seed-fr-05.sql
   docker compose exec db sh -c '/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -d PrepMeUp -b -i /tmp/seed-fr-05.sql'
   ```
   The script ends by listing the four orders: planned orders for A (today+9), B (today+2) and C (today+5), and a sent order for D.
2. Import the collection in Postman. Press **Get New Access Token** on the Authorization tab, sign in as the administrator and press **Use Token**. Run the folder **As administrator**.
3. Get a new token, sign in as the customer, and run the folder **As customer**.

The list must contain exactly the three seeded orders, so use a database without other planned orders (`make db-reset`) or remove them first. Marking an order as sent (step 2 of the scenario) is tested in the FR-06 collection below.

## FR-06: mark an order as sent

`prepmeup-fr-06.postman_collection.json` checks that the administrator can mark a planned order as sent, that the order then leaves the list of planned orders, and that the same order cannot be sent twice. It also checks that an unknown order gives `404`, and that a request without a token (`401`) or with a customer token (`403`) is refused. The collection uses the same test data as FR-05 (`seed-fr-05.sql`): order A is planned and gets sent by the test, B and C stay planned, and D is already sent.

### Prerequisites

The same as for FR-05: the stack runs locally (`make up`) on a database where the migrations have run, and an administrator and a customer are registered in Keycloak. The API must be built from the current code, because the endpoint `POST /admin/orders/{orderId}/shipment` is new (`docker compose up -d --build api`).

### Run

1. Load the test data again before every run, because the test sends order A and the order cannot be put back through the API. The commands are the ones in step 1 of FR-05.
2. Import the collection in Postman. Press **Get New Access Token** on the Authorization tab, sign in as the administrator and press **Use Token**. Run the folder **As administrator** (requests 1 to 5) in order, because request 2 and 3 depend on request 1.
3. Run the folder **Without a token** (request 6). It sends no token.
4. Get a new token, sign in as the customer, and run the folder **As customer** (request 7). Order B stays planned.

Run the FR-05 collection before FR-06, or load the test data again between them, since FR-05 expects the three planned orders A, B and C.

## FR-03: default package types

`prepmeup-fr-03.postman_collection.json` checks that exactly the two default package types are listed, that a replaced package type is hidden, that each has contents, and that the contents cannot be changed through the API.

### Prerequisites

- A fresh database (`make db-reset`), so no administrator has created a package type.
- A user with the admin role is registered in Keycloak.

### Run

1. Load the test data. It adds one package type with the status replaced, called `FR-03 Replaced`, and can be run again:
   ```
   docker compose cp tests/acceptance/seed-fr-03.sql db:/tmp/seed-fr-03.sql
   docker compose exec db sh -c '/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -d PrepMeUp -b -i /tmp/seed-fr-03.sql'
   ```
   The script ends by listing all package types: the replaced one and the two default ones.
2. Import the collection, get a token as the administrator and run requests 1 to 4 in order. Request 3 uses the id that request 1 saves, and request 4 compares with the contents from request 1.

## FR-19: next order 14 days before expiry

`prepmeup-fr-19.postman_collection.json` checks that the back-end plans a next order for an active subscription whose order expires within 14 days, with the same package types and quantities, and only once.

### Prerequisites

The same as for FR-05. The API plans next orders when it starts and then every 5 minutes.

### Run

1. Load the test data. The script deletes and recreates the three test customers (key `fr19-...`), so it can be run again before every run:
   ```
   docker compose cp tests/acceptance/seed-fr-19.sql db:/tmp/seed-fr-19.sql
   docker compose exec db sh -c '/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -d PrepMeUp -b -i /tmp/seed-fr-19.sql'
   ```
   Each customer has one sent order with 2 adult packages and 1 child package. C15 expires today+15, C14 and CP expire today+14, and CP's subscription is paused.
2. Make the back-end check the expiry dates: `docker compose restart api`, or wait 5 minutes.
3. Import the collection, get a token as the administrator and run requests 1 and 2.
4. Restart the API again (or wait 5 minutes) and run request 3. C14 must still have exactly one planned order.

The collection only looks at orders for customers whose name starts with `FR-19`. The planned order for C14 does show up in the FR-05 list, so load the FR-05 test data on a database without it, or run FR-19 after FR-05 and FR-06.
