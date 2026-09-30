using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure; // AddInfrastructure() and AddApplicaiton()

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Composition root. This is the only place Api is allowed to know Infrastructure.
// TODO: builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration); // Here, PrepMeUpContext and e.g. IItemRepository become know to the app

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var keycloak = builder.Configuration.GetSection("Keycloak");
var authority = keycloak["Authority"];

if (!string.IsNullOrWhiteSpace(authority))
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.Audience = keycloak["Audience"];
            options.RequireHttpsMetadata = keycloak.GetValue("RequireHttpsMetadata", true);

            var metadataAddress = keycloak["MetadataAddress"];
            if (!string.IsNullOrWhiteSpace(metadataAddress))
            {
                options.MetadataAddress = metadataAddress;
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authority,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });
}

builder.Services.AddAuthorization();

const string ClientCorsPolicy = "clients";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options => options.AddPolicy(ClientCorsPolicy, policy => policy
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

// Apply any pending EF Core migrations on startup, if a real database is
// configured. Runs inside the api container, which is the only thing that
// can reach the db service (its port is not exposed to the host). Skipped
// when no connection string is set, e.g. in the integration test host.
if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("PrepMeUpDb")))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
    dbContext.Database.Migrate();

    // Seed the same items the PMU-9 spike hardcoded, so the endpoint has
    // real data to return the first time the table is created.
    if (!dbContext.Items.Any())
    {
        dbContext.Items.AddRange(
            new Item { Id = Guid.NewGuid(), Name = "torch" },
            new Item { Id = Guid.NewGuid(), Name = "water" },
            new Item { Id = Guid.NewGuid(), Name = "radio" });
        dbContext.SaveChanges();
    }
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "PrepMeUp API v1"));
}

app.UseCors(ClientCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();

/// <summary>Exposed so the integration tests can boot the real pipeline.</summary>
public partial class Program;
