using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Application;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class OrderRepositoryTests
{
    private static readonly Address Home = new("Finlandsgade 22", "8200", "Aarhus N");
    private static readonly DateTimeOffset Now = new(2026, 12, 1, 10, 0, 0, TimeSpan.Zero);

    private SqliteConnection _connection = null!;
    private SqliteApiFactory _factory = null!;
    private IServiceScope _scope = null!;
    private PrepMeUpDbContext _dbContext = null!;
    private IOrderRepository _repository = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _factory = new SqliteApiFactory(_connection);
        _factory.EnsureDatabaseCreated();
        _scope = _factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        _repository = _scope.ServiceProvider.GetRequiredService<IOrderRepository>();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
        _scope.Dispose();
        _factory.Dispose();
        _connection.Dispose();
    }

    [Test]
    public async Task GetPlanned_EmptyDatabase_ReturnsEmptyList()
    {
        var orders = await _repository.GetPlannedAsync(CancellationToken.None);

        Assert.That(orders, Is.Empty);
    }

    [Test]
    public async Task GetPlanned_OnePlannedOrder_ReturnsAllFieldsMapped()
    {
        var packageTypes = (await PlannedOrderSeed.ActivePackageTypesAsync(_dbContext))
            .OrderByDescending(type => type.Name, StringComparer.Ordinal)
            .ToList();
        var plannedAt = new DateTimeOffset(2026, 12, 1, 10, 0, 0, TimeSpan.FromHours(1));
        var order = await PlannedOrderSeed.AddOrderAsync(
            _dbContext, "sub-1", "Malthe", Home, plannedAt, (packageTypes[0], 2), (packageTypes[1], 1));

        var orders = await _repository.GetPlannedAsync(CancellationToken.None);

        var planned = orders.Single();
        Assert.Multiple(() =>
        {
            Assert.That(planned.OrderId, Is.EqualTo(order.Id));
            Assert.That(planned.PlannedAt, Is.EqualTo(plannedAt));
            Assert.That(planned.CustomerName, Is.EqualTo("Malthe"));
            Assert.That(planned.Address, Is.EqualTo(Home));
            Assert.That(
                planned.Lines,
                Is.EqualTo(new[]
                {
                    new PlannedOrderLine(packageTypes[1].Name, 1),
                    new PlannedOrderLine(packageTypes[0].Name, 2)
                }));
        });
    }

    [Test]
    public async Task GetPlanned_SentOrder_IsLeftOut()
    {
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(_dbContext))[0];
        var planned = await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-1", "Planned", Home, Now, (packageType, 1));
        var sent = await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-2", "Sent", Home, Now, (packageType, 1));
        await PlannedOrderSeed.MarkSentAsync(_dbContext, sent, Now);

        var orders = await _repository.GetPlannedAsync(CancellationToken.None);

        Assert.That(orders.Select(order => order.OrderId), Is.EqualTo(new[] { planned.Id }));
    }

    [Test]
    public async Task GetPlanned_OrdersInThePastAndFuture_ReturnsBoth()
    {
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(_dbContext))[0];
        var past = await PlannedOrderSeed.AddOrderAsync(
            _dbContext, "sub-1", "Past", Home, DateTimeOffset.UtcNow.AddDays(-30), (packageType, 1));
        var future = await PlannedOrderSeed.AddOrderAsync(
            _dbContext, "sub-2", "Future", Home, DateTimeOffset.UtcNow.AddDays(30), (packageType, 1));

        var orders = await _repository.GetPlannedAsync(CancellationToken.None);

        Assert.That(orders.Select(order => order.OrderId), Is.EquivalentTo(new[] { past.Id, future.Id }));
    }

    [Test]
    public async Task GetPlanned_SavedInMixedOrder_ReturnsSortedByPlannedAt()
    {
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(_dbContext))[0];
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-1", "Third", Home, Now.AddSeconds(2), (packageType, 1));
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-2", "First", Home, Now, (packageType, 1));
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-3", "Second", Home, Now.AddSeconds(1), (packageType, 1));

        var orders = await _repository.GetPlannedAsync(CancellationToken.None);

        Assert.That(orders.Select(order => order.CustomerName), Is.EqualTo(new[] { "First", "Second", "Third" }));
    }

    [Test]
    public async Task GetPlanned_DifferentOffsets_ReturnsSortedByInstant()
    {
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(_dbContext))[0];
        var earlierInstant = new DateTimeOffset(2026, 12, 1, 10, 0, 0, TimeSpan.FromHours(2));
        var laterInstant = new DateTimeOffset(2026, 12, 1, 9, 30, 0, TimeSpan.Zero);
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-1", "Later", Home, laterInstant, (packageType, 1));
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-2", "Earlier", Home, earlierInstant, (packageType, 1));

        var orders = await _repository.GetPlannedAsync(CancellationToken.None);

        Assert.That(orders.Select(order => order.CustomerName), Is.EqualTo(new[] { "Earlier", "Later" }));
    }

    [Test]
    public async Task GetPlanned_SamePlannedAt_ReturnsBothInTheSameOrderEveryTime()
    {
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(_dbContext))[0];
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-1", "A", Home, Now, (packageType, 1));
        await PlannedOrderSeed.AddOrderAsync(_dbContext, "sub-2", "B", Home, Now, (packageType, 1));

        var first = await _repository.GetPlannedAsync(CancellationToken.None);
        var second = await _repository.GetPlannedAsync(CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(first, Has.Count.EqualTo(2));
            Assert.That(second.Select(order => order.OrderId), Is.EqualTo(first.Select(order => order.OrderId)));
        });
    }

        [Test]
    public async Task GetById_UnknownId_ReturnsNull()
    {
        var order = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.That(order, Is.Null);
    }

}
