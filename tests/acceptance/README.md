# Acceptance tests (Postman)

`prepmeup-fr-02.postman_collection.json` runs the API part of the FR-02 acceptance test (appendix 6.1). The collection holds no tokens or passwords.

## Prerequisites

- The stack runs locally: `make up` (needs `MSSQL_SA_PASSWORD` and `KEYCLOAK_ADMIN_PASSWORD` in `.env`).
- A test user is registered at http://localhost:8082/realms/prepmeup/account.
- Use a fresh database (`make db-reset`) or a user without a subscription, since `POST /subscription` answers `AlreadySubscribed` for a second run.

## Run

1. Import the collection in Postman and open its Authorization tab. Press **Get New Access Token**, sign in as the test user and press **Use Token**. The token is valid for 15 minutes and is not saved in the file.
2. Run the folder **Before restart** (requests 1 to 3).
3. Restart the database, wait until it is healthy, and restart the API:
   ```
   docker compose restart db
   docker compose restart api
   ```
   The API runs the migrations when it starts, so it must start after the database.
4. Get a new token if the old one has expired, and run the folder **After restart** (requests 4 and 5). The tests compare with the values saved in step 2.
