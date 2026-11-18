using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Application;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class SubscriptionReadyForNextOrderTests
{
    private static readonly Address Home = new("Finlandsgade 22", "8200", "Aarhus N");
    private static readonly DateTimeOffset StartedAt = new(2026, 11, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Now = new(2027, 11, 4, 12, 0, 0, TimeSpan.Zero);

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
    [Description("Of five subscriptions, the database only returns the one that is active, has no order waiting and whose latest sent order expires within 14 days")]
    public async Task GetReadyForNextOrderAsync_FiveSubscriptions_ReturnsOnlyTheReadyOne()
    {
        using var factory = CreateFactory();
        Guid readyCustomerId;
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            var packageType = await dbContext.PackageTypes.FirstAsync(type => type.Status == PackageTypeStatus.Active);

            var ready = AddSubscription(dbContext, "ready", packageType);
            ready.Orders.Single().MarkAsSent(new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));
            readyCustomerId = ready.CustomerId;

            var fifteenDaysLeft = AddSubscription(dbContext, "fifteen-days-left", packageType);
            fifteenDaysLeft.Orders.Single().MarkAsSent(new DateTimeOffset(2026, 11, 19, 12, 0, 0, TimeSpan.Zero));

            var paused = AddSubscription(dbContext, "paused", packageType);
            paused.Orders.Single().MarkAsSent(new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));
            paused.Pause();

            var orderWaiting = AddSubscription(dbContext, "order-waiting", packageType);
            orderWaiting.Orders.Single().MarkAsSent(new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));
            orderWaiting.PlanNextOrder(new DateTimeOffset(2027, 11, 1, 12, 0, 0, TimeSpan.Zero));

            AddSubscription(dbContext, "never-sent", packageType);

            await dbContext.SaveChangesAsync();
        }

        using var readScope = factory.Services.CreateScope();
        var subscriptions = await readScope.ServiceProvider.GetRequiredService<ISubscriptionRepository>()
            .GetReadyForNextOrderAsync(Now, CancellationToken.None);

        Assert.That(subscriptions.Select(subscription => subscription.CustomerId), Is.EqualTo(new[] { readyCustomerId }));
    }

    private static Subscription AddSubscription(PrepMeUpDbContext dbContext, string keycloakId, PackageType packageType)
    {
        var customer = new Customer(keycloakId, keycloakId, $"{keycloakId}@example.com", null, Home);
        dbContext.Customers.Add(customer);
        var subscription = Subscription.Start(customer.Id, StartedAt, [new OrderLine(packageType, 2)]);
        dbContext.Subscriptions.Add(subscription);
        return subscription;
    }

    private SqliteApiFactory CreateFactory()
    {
        var factory = new SqliteApiFactory(_connection);
        factory.EnsureDatabaseCreated();
        return factory;
    }
}
