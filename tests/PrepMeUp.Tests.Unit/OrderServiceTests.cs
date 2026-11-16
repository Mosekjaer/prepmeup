using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class OrderServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 20, 9, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset StartedAt = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    private IOrderRepository _orderRepository = null!;
    private OrderService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(Now);
        _sut = new OrderService(_orderRepository, clock);
    }

    [Test]
    public async Task MarkAsSentAsync_PlannedOrder_SetsSentAtFromClockAndSaves()
    {
        var orderId = Guid.NewGuid();
        var order = NewPlannedOrder();
        _orderRepository.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);

        var result = await _sut.MarkAsSentAsync(orderId, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(MarkAsSentResult.Sent));
            Assert.That(order.SentAt, Is.EqualTo(Now));
        });
        await _orderRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task MarkAsSentAsync_UnknownOrder_ReturnsNotFoundAndSavesNothing()
    {
        var orderId = Guid.NewGuid();
        _orderRepository.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns((Order?)null);

        var result = await _sut.MarkAsSentAsync(orderId, CancellationToken.None);

        Assert.That(result, Is.EqualTo(MarkAsSentResult.NotFound));
        await _orderRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task MarkAsSentAsync_AlreadySent_ReturnsNotPlannedAndSavesNothing()
    {
        var orderId = Guid.NewGuid();
        var order = NewPlannedOrder();
        var firstSentAt = StartedAt.AddDays(1);
        order.MarkAsSent(firstSentAt);
        _orderRepository.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);

        var result = await _sut.MarkAsSentAsync(orderId, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(MarkAsSentResult.NotPlanned));
            Assert.That(order.SentAt, Is.EqualTo(firstSentAt));
        });
        await _orderRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task MarkAsSentAsync_ConflictWhileSaving_ReturnsNotPlanned()
    {
        var orderId = Guid.NewGuid();
        _orderRepository.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(NewPlannedOrder());
        _orderRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new OrderConflictException()));

        var result = await _sut.MarkAsSentAsync(orderId, CancellationToken.None);

        Assert.That(result, Is.EqualTo(MarkAsSentResult.NotPlanned));
    }

    private static Order NewPlannedOrder() =>
        Subscription.Start(Guid.NewGuid(), StartedAt, [NewLine("Normal - Adult", 1)]).Orders.Single();

    private static OrderLine NewLine(string packageTypeName, int quantity) =>
        new(
            PackageType.Create(
                packageTypeName,
                [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]),
            quantity);
}
