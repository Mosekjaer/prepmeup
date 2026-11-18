using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionServicePlanNextOrdersTests
{
    private static readonly DateTimeOffset StartedAt = new(2026, 11, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset SentAt = new(2026, 11, 18, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ReorderDate = new(2027, 11, 4, 12, 0, 0, TimeSpan.Zero);

    private ISubscriptionRepository _subscriptionRepository = null!;
    private IClock _clock = null!;
    private SubscriptionService _sut = null!;
    private PackageType _adultPackage = null!;

    [SetUp]
    public void SetUp()
    {
        _subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        _clock = Substitute.For<IClock>();
        _clock.UtcNow.Returns(ReorderDate);
        _sut = new SubscriptionService(
            Substitute.For<ICustomerRepository>(),
            _subscriptionRepository,
            Substitute.For<IPackageTypeRepository>(),
            _clock);
        _adultPackage = PackageType.Create(
            "Normal - Adult",
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]);
    }

    [Test]
    [Description("S1: if one subscription is ready for its next order, it gets a new order")]
    public async Task PlanNextOrdersAsync_OneSubscriptionReady_SubscriptionGetsNewOrder()
    {
        var subscription = NewSubscriptionWithSentOrder();
        _subscriptionRepository.GetActiveAsync(Arg.Any<CancellationToken>()).Returns([subscription]);

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        Assert.That(subscription.Orders, Has.Count.EqualTo(2));
    }

    [Test]
    [Description("S2: if one subscription is ready, the changes are saved once")]
    public async Task PlanNextOrdersAsync_OneSubscriptionReady_SavesOnce()
    {
        _subscriptionRepository.GetActiveAsync(Arg.Any<CancellationToken>()).Returns([NewSubscriptionWithSentOrder()]);

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        await _subscriptionRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    [Description("S3: if the subscription is not ready (one second before the reorder date), it gets no new order")]
    public async Task PlanNextOrdersAsync_SubscriptionNotReady_SubscriptionGetsNoNewOrder()
    {
        var subscription = NewSubscriptionWithSentOrder();
        _subscriptionRepository.GetActiveAsync(Arg.Any<CancellationToken>()).Returns([subscription]);
        _clock.UtcNow.Returns(ReorderDate.AddSeconds(-1));

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        Assert.That(subscription.Orders, Has.Count.EqualTo(1));
    }

    [Test]
    [Description("S4: if no subscription is ready, nothing is saved")]
    public async Task PlanNextOrdersAsync_NoSubscriptionReady_DoesNotSave()
    {
        _subscriptionRepository.GetActiveAsync(Arg.Any<CancellationToken>()).Returns([NewSubscriptionWithSentOrder()]);
        _clock.UtcNow.Returns(ReorderDate.AddSeconds(-1));

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    [Description("S5: if two of three subscriptions are ready, the method returns 2")]
    public async Task PlanNextOrdersAsync_TwoOfThreeReady_ReturnsTwo()
    {
        var notReady = NewSubscriptionWithSentOrder();
        notReady.PlanNextOrder(StartedAt);
        _subscriptionRepository.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns([NewSubscriptionWithSentOrder(), notReady, NewSubscriptionWithSentOrder()]);

        var planned = await _sut.PlanNextOrdersAsync(CancellationToken.None);

        Assert.That(planned, Is.EqualTo(2));
    }

    private Subscription NewSubscriptionWithSentOrder()
    {
        var subscription = Subscription.Start(Guid.NewGuid(), StartedAt, [new OrderLine(_adultPackage, 2)]);
        subscription.Orders.Single().MarkAsSent(SentAt);
        return subscription;
    }
}
