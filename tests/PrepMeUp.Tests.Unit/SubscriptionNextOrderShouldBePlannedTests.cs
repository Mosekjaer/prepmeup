using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionNextOrderShouldBePlannedTests
{
    private static readonly DateTimeOffset StartedAt = new(2026, 11, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset SentAt = new(2026, 11, 18, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ReorderDate = new(2027, 11, 4, 12, 0, 0, TimeSpan.Zero);

    private PackageType _adultPackage = null!;
    private Subscription _subscription = null!;

    [SetUp]
    public void SetUp()
    {
        _adultPackage = PackageType.Create(
            "Normal - Adult",
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]);
        _subscription = Subscription.Start(Guid.NewGuid(), StartedAt, [new OrderLine(_adultPackage, 2)]);
        _subscription.Orders.Single().MarkAsSent(SentAt);
    }

    [Description("Boundary: one second before and right on the reorder date of the latest sent order")]
    [TestCase(-1, false)]
    [TestCase(0, true)]
    public void NextOrderShouldBePlanned_AroundReorderDate_ReturnsExpected(int secondsFromReorderDate, bool expected)
    {
        var now = ReorderDate.AddSeconds(secondsFromReorderDate);

        Assert.That(_subscription.NextOrderShouldBePlanned(now), Is.EqualTo(expected));
    }

    [Test]
    [Description("The first order is not sent yet, so there is no sent order to count from")]
    public void NextOrderShouldBePlanned_NoSentOrder_ReturnsFalse()
    {
        var subscription = Subscription.Start(Guid.NewGuid(), StartedAt, [new OrderLine(_adultPackage, 2)]);

        Assert.That(subscription.NextOrderShouldBePlanned(ReorderDate), Is.False);
    }

    [Test]
    [Description("An order is already waiting to be sent")]
    public void NextOrderShouldBePlanned_OrderWaitingToBeSent_ReturnsFalse()
    {
        _subscription.PlanNextOrder(ReorderDate);

        Assert.That(_subscription.NextOrderShouldBePlanned(ReorderDate), Is.False);
    }

    [Test]
    [Description("Only the latest sent order counts: the old order has expired, but the latest has not reached its reorder date")]
    public void NextOrderShouldBePlanned_OldOrderExpiredButLatestNotDue_ReturnsFalse()
    {
        _subscription.PlanNextOrder(ReorderDate);
        _subscription.Orders.Last().MarkAsSent(new DateTimeOffset(2027, 11, 10, 12, 0, 0, TimeSpan.Zero));

        var now = new DateTimeOffset(2027, 11, 20, 12, 0, 0, TimeSpan.Zero);

        Assert.That(_subscription.NextOrderShouldBePlanned(now), Is.False);
    }

    [Test]
    [Description("A paused subscription")]
    public void NextOrderShouldBePlanned_SubscriptionPaused_ReturnsFalse()
    {
        _subscription.Pause();

        Assert.That(_subscription.NextOrderShouldBePlanned(ReorderDate), Is.False);
    }
}
