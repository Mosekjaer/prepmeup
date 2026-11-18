using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using PrepMeUp.Api.BackgroundServices;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class NextOrderPlannerTests
{
    private static readonly Address Home = new("Finlandsgade 22", "8200", "Aarhus N");

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
    [Description("When the planner starts, a subscription whose order expires in 13 days gets its next order saved in the database")]
    public async Task StartAsync_SubscriptionReady_NextOrderIsSaved()
    {
        using var factory = new SqliteApiFactory(_connection);
        factory.EnsureDatabaseCreated();
        Guid subscriptionId;
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            var packageType = await dbContext.PackageTypes.FirstAsync(type => type.Status == PackageTypeStatus.Active);
            var customer = new Customer("sub-1", "Malthe", "malthe@example.com", null, Home);
            dbContext.Customers.Add(customer);
            var sentAt = DateTimeOffset.UtcNow.AddYears(-1).AddDays(13);
            var subscription = Subscription.Start(customer.Id, sentAt.AddDays(-7), [new OrderLine(packageType, 2)]);
            subscription.Orders.Single().MarkAsSent(sentAt);
            dbContext.Subscriptions.Add(subscription);
            await dbContext.SaveChangesAsync();
            subscriptionId = subscription.Id;
        }

        var planner = new NextOrderPlanner(
            factory.Services.GetRequiredService<IServiceScopeFactory>(),
            NullLogger<NextOrderPlanner>.Instance);
        await planner.StartAsync(CancellationToken.None);
        var orderCount = await WaitForOrderCountAsync(factory, subscriptionId, expected: 2);
        await planner.StopAsync(CancellationToken.None);

        Assert.That(orderCount, Is.EqualTo(2));
    }

    private static async Task<int> WaitForOrderCountAsync(SqliteApiFactory factory, Guid subscriptionId, int expected)
    {
        var count = 0;
        for (var attempt = 0; attempt < 50 && count < expected; attempt++)
        {
            await Task.Delay(100);
            using var scope = factory.Services.CreateScope();
            count = await scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>().Orders
                .CountAsync(order => order.SubscriptionId == subscriptionId);
        }

        return count;
    }
}
