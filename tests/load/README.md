# Load test (k6)

`nfr-05.js` runs the NFR-05 acceptance test (appendix 6.1): 10 virtual users call `GET /user` with a customer token for 1 minute. The test passes when the 90th percentile of the response time is under 1 second and no request failed. k6 marks a failed threshold with a cross and exits with a code other than 0.

## Prerequisites

- A customer who has completed onboarding, so `GET /user` answers 200.
- k6, or Docker with the `grafana/k6` image.
- Run it from a machine outside the server, against the deployed API.

## Run

1. Get an access token for the customer. Sign in through the administrator interface or the customer app in a browser, open the developer tools and copy the token from the Authorization header of a request to the API. A token is valid for 15 minutes.
2. Start the test with the token and the address of the deployed API:
   ```
   k6 run -e BASE_URL=https://<api-address> -e TOKEN=<token> tests/load/nfr-05.js
   ```
   With Docker:
   ```
   docker run --rm -i -e BASE_URL=https://<api-address> -e TOKEN=<token> grafana/k6 run - < tests/load/nfr-05.js
   ```
3. Copy the summary that k6 prints at the end to the actual result in appendix 6.1. The lines to look at are `http_req_duration` (p(90)) and `http_req_failed`.

The token is passed on the command line and never saved in a file.
