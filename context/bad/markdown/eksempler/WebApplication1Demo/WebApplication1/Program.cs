using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Domain.DTOs;

var builder = WebApplication.CreateBuilder(args);


//add dependency injection for the SchoolContext
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Creates the database if not exists
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchoolContext>();
    context.Database.Migrate(); //Applies pending migrations and creates the database if not exists
    DbInitializer.Initialize(context);
}

app.MapGet("/", (SchoolContext dbcontext) =>
{
    var courses = dbcontext.Courses;

    return courses.Select(c => new { c.Title, c.Credits, c.Enrollments.Count }).ToList();
});

app.MapGet("/include", (SchoolContext dbcontext) =>
{
    var courses = dbcontext.Courses.Include(c => c.Enrollments);

    return courses.Select(c => new CourseDto
        { Title = c.Title, Credits = c.Credits, CountOfEnrollments = c.Enrollments.Count }).ToListAsync();
});

app.Run();