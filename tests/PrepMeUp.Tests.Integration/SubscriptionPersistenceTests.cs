using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Application;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class SubscriptionPersistenceTests
{
    private const string Email = "malthe@example.com";
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
    public async Task Subscribe_ApiRestarted_SubscriptionAndFirstOrderAreKept()
    {
        using (var factory = CreateFactory())
        using (var scope = factory.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await services.GetRequiredService<CustomerService>()
                .SaveDetailsAsync("sub-1", Email, "Malthe", null, Home, CancellationToken.None);
            var activeType = (await services.GetRequiredService<IPackageTypeRepository>().GetAllAsync(CancellationToken.None))
                .First(type => type.Status == PackageTypeStatus.Active);
            await services.GetRequiredService<SubscriptionService>()
                .SubscribeAsync("sub-1", [new SubscribeLine(activeType.Id, 1)], CancellationToken.None);
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
    public async Task Subscribe_ApiRestarted_OrderLinesAreKept()
    {
        Guid adultId, childId;
        using (var factory = CreateFactory())
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            var packageTypes = await dbContext.PackageTypes.Where(type => type.Status == PackageTypeStatus.Active).ToListAsync();
            (adultId, childId) = (packageTypes[0].Id, packageTypes[1].Id);
            var customer = new Customer("sub-1", "Malthe", Email, null, Home);
            dbContext.Customers.Add(customer);
            dbContext.Subscriptions.Add(Subscription.Start(
                customer.Id,
                DateTimeOffset.UtcNow,
                [new OrderLine(packageTypes[0], 2), new OrderLine(packageTypes[1], 1)]));
            await dbContext.SaveChangesAsync();
        }

        using var restarted = CreateFactory();
        using var restartedScope = restarted.Services.CreateScope();
        var subscription = await restartedScope.ServiceProvider.GetRequiredService<SubscriptionService>()
            .GetAsync("sub-1", CancellationToken.None);

        Assert.That(
            subscription?.Orders.Single().Lines.Select(line => (line.PackageType.Id, line.Quantity)),
            Is.EquivalentTo(new[] { (adultId, 2), (childId, 1) }));
    }

    [Test]
    public async Task SaveChanges_SecondSubscriptionForSameCustomer_IsRejectedByDatabase()
    {
        using var factory = CreateFactory();
        var customer = new Customer("sub-1", "Malthe", Email, null, Home);
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            dbContext.Customers.Add(customer);
            var packageType = await dbContext.PackageTypes.FirstAsync(type => type.Status == PackageTypeStatus.Active);
            dbContext.Subscriptions.Add(Subscription.Start(customer.Id, DateTimeOffset.UtcNow, [new OrderLine(packageType, 1)]));
            await dbContext.SaveChangesAsync();
        }

        using var otherScope = factory.Services.CreateScope();
        var otherDbContext = otherScope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var otherPackageType = await otherDbContext.PackageTypes.FirstAsync(type => type.Status == PackageTypeStatus.Active);
        otherDbContext.Subscriptions.Add(Subscription.Start(customer.Id, DateTimeOffset.UtcNow, [new OrderLine(otherPackageType, 1)]));

        Assert.That(() => otherDbContext.SaveChangesAsync(), Throws.InstanceOf<DbUpdateException>());
    }

    [Test]
    public async Task Database_SamePackageTypeTwiceInOneOrder_IsRejected()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var (orderId, orderedTypeId, _) = await AddSubscriptionWithOneLineAsync(dbContext);

        Assert.That(
            () => dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO OrderLines (Id, OrderId, PackageTypeId, Quantity) VALUES ({Guid.NewGuid()}, {orderId}, {orderedTypeId}, 1)"),
            Throws.InstanceOf<SqliteException>());
    }

    [Test]
    public async Task Database_QuantityBelowOne_IsRejected()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var (orderId, _, otherTypeId) = await AddSubscriptionWithOneLineAsync(dbContext);

        Assert.That(
            () => dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO OrderLines (Id, OrderId, PackageTypeId, Quantity) VALUES ({Guid.NewGuid()}, {orderId}, {otherTypeId}, 0)"),
            Throws.InstanceOf<SqliteException>());
    }

    private static async Task<(Guid OrderId, Guid OrderedTypeId, Guid OtherTypeId)> AddSubscriptionWithOneLineAsync(PrepMeUpDbContext dbContext)
    {
        var packageTypes = await dbContext.PackageTypes.Where(type => type.Status == PackageTypeStatus.Active).ToListAsync();
        var customer = new Customer("sub-1", "Malthe", Email, null, Home);
        dbContext.Customers.Add(customer);
        var subscription = Subscription.Start(customer.Id, DateTimeOffset.UtcNow, [new OrderLine(packageTypes[0], 1)]);
        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync();

        return (subscription.Orders.Single().Id, packageTypes[0].Id, packageTypes[1].Id);
    }

    private SqliteApiFactory CreateFactory()
    {
        var factory = new SqliteApiFactory(_connection);
        factory.EnsureDatabaseCreated();
        return factory;
    }
}
