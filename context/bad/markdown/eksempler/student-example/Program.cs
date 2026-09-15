using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Added for at kunne vise scalar endpoint
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//Added for at vi kan bruge Controllers
builder.Services.AddControllers();



var app = builder.Build();

//Added for at kunne vise scalar endpoint
app.MapOpenApi("/openapi/v1.json");
app.MapScalarApiReference();

//Added for at vi kan bruge Controllers
app.MapControllers();

app.Run();
