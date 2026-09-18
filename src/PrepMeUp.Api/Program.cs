using Serilog;

// Create builder object
var builder = WebApplication.CreateBuilder(args);

// Use Serilog library to log
// Read logs, Run in terminal: dotnet run --project 
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Composition root. This is the only place Api is allowed to know Infrastructure.
// TODO: builder.Services.AddApplication();
// TODO: builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers(); // enable controller based routing styles
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi(); // generates the OpenAPI/Swagger schema describing endpoints
builder.Services.AddHealthChecks(); // egisters the health-check feature (what powers /health)

// Set up CORS (Cross-Origin Resource Sharing)
const string ClientCorsPolicy = "clients";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options => options.AddPolicy(ClientCorsPolicy, policy => policy
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()));

// Turns builder (L4) into an app
var app = builder.Build();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "PrepMeUp API v1"));
}

app.UseCors(ClientCorsPolicy);
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();

/// <summary>Exposed so the integration tests can boot the real pipeline.</summary>
public partial class Program;
