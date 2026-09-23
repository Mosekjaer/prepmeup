# @prepmeup/api-client

Generated from the API's OpenAPI document. Never edit `src/generated/` by hand.

## Regenerate

1. Start the API on port 5001, where `nswag.json` expects it: `docker compose up` from the repo root
   (serves `/openapi/v1.json` in Development).
2. From `clients/`: `npm run generate:api-client`.
3. Commit `src/generated/` with the contract change. CI typechecks against it and cannot generate it.

The generator is configured in [nswag.json](nswag.json) and runs through the repo-local
`nswag` tool in `.config/dotnet-tools.json`.
