---
title: Tips - Troubleshooting
source: Tips - troubleshooting.html
course_week: 12-13
topic: API-dokumentation, background services og caching
---

# Tips — Troubleshooting

## JSON serialization: "cycles in the output"

If you run into a problem saying there are cycles in the output (object cycle / reference loop when serializing entities with navigation properties), you can ignore the loops like this:

1. Install the package `Microsoft.AspNetCore.Mvc.NewtonsoftJson` using the NuGet Package Manager
   - https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson/

2. In `Startup.cs` or `Program.cs`, add the service (put it after the line `builder.Services.AddSwaggerGen();`):

```csharp
services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
```
