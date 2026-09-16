# @prepmeup/api-client

Generated from the API's OpenAPI document. Never edit `src/generated/` by hand.

## Regenerate

1. Start the API: `dotnet run --project src/PrepMeUp.Api` (serves `/openapi/v1.json` in Development).
2. From `clients/`: `npm run generate:api-client`.
3. Uncomment the `export * from './generated/api'` line in `src/index.ts` on the first run.

The generator is configured in [nswag.json](nswag.json) and runs through the repo-local
`nswag` tool in `.config/dotnet-tools.json`.
