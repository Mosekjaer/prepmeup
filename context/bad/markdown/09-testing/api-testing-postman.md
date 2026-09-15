---
title: API testing with Postman
source: Api testing with Postman.pdf
course_week: 8
topic: Testing af Web APIs
---

# API testing with Postman

## Postman API testing — overview

Main workflow for using Postman for API testing per request:

- **Pre-request script** — runs before the request is executed. Use it to set up things or get state before the request.
- **Test script** — the code for the test(s), executed after the request is sent.

## Collection scripts

- You can also add "global" scripts on a collection of requests.
- Both the collection-level pre-request script and the collection-level test script run before any individual scripts for a given request.
- Collection pre-request script: runs before every request in the collection. Collection test script: runs after every request in the collection.

## Run a test collection

- The "Run collection" action is easy to miss in the UI (via the collection's menu).
- **Run configuration**: choose the run order of the requests and corresponding tests. Tests should be independent and order should not matter, but in practice it may be convenient to choose a certain run order if some script depends on results of another request.
- The run configuration also includes performance (load test) settings for the whole collection.

## Writing tests

Tests are written in JavaScript using the `pm` object (Postman) and the Chai.js assertion library. Many built-in snippets are available to get started.

### Example snippet — response code

Code goes in the test tab:

```javascript
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});
// HTTP OK Code = 200

pm.test("Status code is 404", function () {
    pm.response.to.have.status(404);
});
// HTTP Not Found Code = 404 - useful for testing something does not exist in api/database!
```

### Tests — using variables

```javascript
// In pre-request for getall
pm.collectionVariables.set("nr_of_games", 3)

// In Test – testing number of returned games
pm.test("Board Games collection equal to " + pm.collectionVariables.get("nr_of_games"), function () {
    pm.expect(pm.response.json().recordCount).to.eql(
        pm.collectionVariables.get("nr_of_games"));
});

// notice recordCount / json().length gets number of elements in array
```

### Tests — for a specific resource

Request: `https://localhost:7052/api/BoardGames/643912`

```javascript
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Found Acquire board game", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.Name).to.eql("Acquire");
});
```

## Running requests — POST example

Example: posting a new board game.

In the pre-request script, log the current number of games and set a variable before doing the POST:

```javascript
pm.sendRequest("https://localhost:7052/boardgames", function (err, response) {
    pm.collectionVariables.set("games_in_collection", response.json().length)
    console.log(response.json()); // using the postman log
});
```

In the test script — don't hardcode expected values, use the variables:

```javascript
pm.test("Check board game was inserted - id exists", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.Id).exist
    // save ID for later usage (see cleanup later)
    pm.collectionVariables.set("id_inserted", jsonData.Id)
});

pm.test("Games collection size increased by 1", function () {
    // get all games to find new size
    pm.sendRequest("https://localhost:7052/boardgames", function (err, response) {
        pm.expect(response.json().length).to.eq(
            pm.collectionVariables.get("games_in_collection") + 1); // size should increase by 1
    });
});
```

## Running requests — DELETE example (cleanup)

```javascript
const id = pm.collectionVariables.get("id_inserted")
const deleteRequest = {
    url: 'https://localhost:7052/Boardgames/' + id,
    method: 'DELETE',
    header: {
        'Content-Type': 'text/plain'
    },
    /* If you want, you can add parameters (json) to your request:
    body: {
        mode: 'raw',
        raw: JSON.stringify({ key: 'this is json' })
    } */
};
pm.sendRequest(deleteRequest, (error, response) => {
    // console.log(response.json());
});
```

## Sharing scripts and tests

- Everything (requests and tests) can be exported as JSON and put in version control.
- Or shared directly with a team via Postman's Team setup.
- Automated tests should run as part of CI.

## PostBot (AI test generation, beta)

Using AI PostBot to generate tests:

1. Run a request
2. Go to the Tests tab
3. Click the PostBot icon
4. Use PostBot

More info: https://learning.postman.com/docs/getting-started/basics/about-postbot/

## Using the Postman console

- Use the PM console to debug tests (`console.log(...)` output ends up here).
- The console shows an overview and details of all requests.
- Documentation: https://learning.postman.com/docs/sending-requests/troubleshooting-api-requests/

## More info

- Authorization and requests in Postman: https://learning.postman.com/docs/sending-requests/authorization/authorization/
