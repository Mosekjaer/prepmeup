using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PrepMeUp.Application;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class SubscriptionPersistenceTests
{
    private SqliteConnection _connection = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    [TearDown]
    public void TearDown()
    {
        _connection.Dispose();
    }

    [Test]
    public async Task Subscribe_ApiRestarted_SubscriptionAndFirstOrderAreKept()
    {
        using (var factory = CreateFactory())
        using (var scope = factory.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await services.GetRequiredService<CustomerService>()
                .SaveDetailsAsync("sub-1", "Malthe", "Finlandsgade 22", CancellationToken.None);
            await services.GetRequiredService<SubscriptionService>().SubscribeAsync("sub-1", CancellationToken.None);
        }

        using var restarted = CreateFactory();
        using var restartedScope = restarted.Services.CreateScope();
        var subscription = await restartedScope.ServiceProvider.GetRequiredService<SubscriptionService>()
            .GetAsync("sub-1", CancellationToken.None);

        Assert.That(
            (subscription?.Status, subscription?.Orders.Single().PlannedAt),
            Is.EqualTo((SubscriptionStatus.Active, subscription?.StartedAt)));
    }

    [Test]
    public async Task SaveChanges_SecondSubscriptionForSameCustomer_IsRejectedByDatabase()
    {
        using var factory = CreateFactory();
        var customer = new Customer("sub-1", "Malthe", "Finlandsgade 22");
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            dbContext.Customers.Add(customer);
            dbContext.Subscriptions.Add(Subscription.Start(customer.Id, DateTimeOffset.UtcNow));
            await dbContext.SaveChangesAsync();
        }

        using var otherScope = factory.Services.CreateScope();
        var otherDbContext = otherScope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        otherDbContext.Subscriptions.Add(Subscription.Start(customer.Id, DateTimeOffset.UtcNow));

        Assert.That(() => otherDbContext.SaveChangesAsync(), Throws.InstanceOf<DbUpdateException>());
    }

    private WebApplicationFactory<Program> CreateFactory()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IDbContextOptionsConfiguration<PrepMeUpDbContext>>();
                services.AddDbContext<PrepMeUpDbContext>(options => options.UseSqlite(_connection));
            }));

        using var scope = factory.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>().Database.EnsureCreated();

        return factory;
    }
}
