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
    private SubscriptionService _sut = null!;
    private PackageType _adultPackage = null!;

    [SetUp]
    public void SetUp()
    {
        _subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(ReorderDate);
        _sut = new SubscriptionService(
            Substitute.For<ICustomerRepository>(),
            _subscriptionRepository,
            Substitute.For<IPackageTypeRepository>(),
            clock);
        _adultPackage = PackageType.Create(
            "Normal - Adult",
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]);
    }

    [Test]
    [Description("S1: the repository is asked for the subscriptions that are ready at the clock's now")]
    public async Task PlanNextOrdersAsync_Called_AsksRepositoryWithClockNow()
    {
        _subscriptionRepository.GetReadyForNextOrderAsync(Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>()).Returns([]);

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        await _subscriptionRepository.Received(1).GetReadyForNextOrderAsync(ReorderDate, Arg.Any<CancellationToken>());
    }

    [Test]
    [Description("S2: if the repository returns one ready subscription, it gets a new order")]
    public async Task PlanNextOrdersAsync_OneSubscriptionReady_SubscriptionGetsNewOrder()
    {
        var subscription = NewSubscriptionWithSentOrder();
        ReturnsReady(subscription);

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        Assert.That(subscription.Orders, Has.Count.EqualTo(2));
    }

    [Test]
    [Description("S3: if the repository returns one ready subscription, the changes are saved once")]
    public async Task PlanNextOrdersAsync_OneSubscriptionReady_SavesOnce()
    {
        ReturnsReady(NewSubscriptionWithSentOrder());

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        await _subscriptionRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    [Description("S4: if the repository returns no subscriptions, nothing is saved")]
    public async Task PlanNextOrdersAsync_NoSubscriptionReady_DoesNotSave()
    {
        ReturnsReady();

        await _sut.PlanNextOrdersAsync(CancellationToken.None);

        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    [Description("S5: if the repository returns two ready subscriptions, the method returns 2")]
    public async Task PlanNextOrdersAsync_TwoSubscriptionsReady_ReturnsTwo()
    {
        ReturnsReady(NewSubscriptionWithSentOrder(), NewSubscriptionWithSentOrder());

        var planned = await _sut.PlanNextOrdersAsync(CancellationToken.None);

        Assert.That(planned, Is.EqualTo(2));
    }

    private void ReturnsReady(params Subscription[] subscriptions) =>
        _subscriptionRepository.GetReadyForNextOrderAsync(Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>())
            .Returns(subscriptions);

    private Subscription NewSubscriptionWithSentOrder()
    {
        var subscription = Subscription.Start(Guid.NewGuid(), StartedAt, [new OrderLine(_adultPackage, 2)]);
        subscription.Orders.Single().MarkAsSent(SentAt);
        return subscription;
    }
}
