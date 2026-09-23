using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Application;

namespace PrepMeUp.Infrastructure;

// Dette layers (infrastructures) dependency injection
public static class DependencyInjection
{
    // Note: the 'this' mean you can call the method below as builder.Services.AddInfrastructure(...)
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PrepMeUpDb"); // get from appsettings.json

        // When any class needs a DbContext, the program provides PrepMeUpDbContext
        services.AddDbContext<PrepMeUpDbContext>(options => options.UseSqlServer(connectionString));
        
        // Add IItemRepository to services
        // -> When a controller needs IItemRepository needs it for its constructor, the program provides ItemRepository and instantiates automatically
        services.AddScoped<IItemRepository, ItemRepository>();

        return services;
    }
}
