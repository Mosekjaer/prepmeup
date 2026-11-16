# Acceptance tests (Postman)

`prepmeup-fr-02.postman_collection.json` runs the API part of the FR-02 acceptance test (appendix 6.1). `prepmeup-fr-05.postman_collection.json` runs the API part of FR-05 (see the end of this file). The collections hold no tokens or passwords.

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

The list must contain exactly the three seeded orders, so use a database without other planned orders (`make db-reset`) or remove them first. Marking an order as sent (step 2 of the scenario) is not part of the collection until FR-06 exists.
