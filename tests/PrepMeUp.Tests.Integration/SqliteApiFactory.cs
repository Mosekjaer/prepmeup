using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class SqliteApiFactory(SqliteConnection connection) : AuthenticatedApiFactory
{
    public void EnsureDatabaseCreated()
    {
        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>().Database.EnsureCreated();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<PrepMeUpDbContext>>();
            services.AddDbContext<PrepMeUpDbContext>(options => options.UseSqlite(connection));
        });
    }
}
