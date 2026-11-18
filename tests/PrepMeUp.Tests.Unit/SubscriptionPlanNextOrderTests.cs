using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionPlanNextOrderTests
{
    private static readonly DateTimeOffset StartedAt = new(2026, 11, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset SentAt = new(2026, 11, 18, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Now = new(2027, 11, 4, 12, 0, 0, TimeSpan.Zero);

    private PackageType _adultPackage = null!;
    private PackageType _childPackage = null!;
    private Subscription _subscription = null!;

    [SetUp]
    public void SetUp()
    {
        _adultPackage = NewPackageType("Normal - Adult");
        _childPackage = NewPackageType("Normal - Child");
        _subscription = Subscription.Start(
            Guid.NewGuid(),
            StartedAt,
            [new OrderLine(_adultPackage, 2), new OrderLine(_childPackage, 1)]);
        _subscription.Orders.Single().MarkAsSent(SentAt);
    }

    [Test]
    [Description("P1: calling PlanNextOrder once gives the subscription exactly one more order")]
    public void PlanNextOrder_CalledOnce_AddsOneOrder()
    {
        _subscription.PlanNextOrder(Now);

        Assert.That(_subscription.Orders, Has.Count.EqualTo(2));
    }

    [Test]
    [Description("P2: the new order is not sent and is planned at now")]
    public void PlanNextOrder_CalledOnce_NewOrderIsNotSentAndPlannedAtNow()
    {
        _subscription.PlanNextOrder(Now);

        var newOrder = _subscription.Orders.Last();
        Assert.That((newOrder.SentAt, newOrder.PlannedAt), Is.EqualTo(((DateTimeOffset?)null, Now)));
    }

    [Test]
    [Description("P3: the new order has the same package types and quantities as the latest sent order")]
    public void PlanNextOrder_CalledOnce_NewOrderHasSamePackageTypesAndQuantities()
    {
        _subscription.PlanNextOrder(Now);

        var newOrder = _subscription.Orders.Last();
        Assert.That(
            newOrder.Lines.Select(line => (line.PackageType, line.Quantity)),
            Is.EqualTo(new[] { (_adultPackage, 2), (_childPackage, 1) }));
    }

    [Test]
    [Description("P4: after the next order is sent, calling PlanNextOrder again adds yet another order")]
    public void PlanNextOrder_PreviousNextOrderSent_AddsAnotherOrder()
    {
        _subscription.PlanNextOrder(Now);
        _subscription.Orders.Last().MarkAsSent(Now.AddDays(7));

        _subscription.PlanNextOrder(Now.AddYears(1));

        Assert.That(_subscription.Orders, Has.Count.EqualTo(3));
    }

    [Test]
    [Description("P5: calling PlanNextOrder before the first order is sent throws")]
    public void PlanNextOrder_NoSentOrder_Throws()
    {
        var subscription = Subscription.Start(Guid.NewGuid(), StartedAt, [new OrderLine(_adultPackage, 2)]);

        Assert.That(() => subscription.PlanNextOrder(Now), Throws.InvalidOperationException);
    }

    [Test]
    [Description("P6: calling PlanNextOrder twice without sending the order in between throws on the second call")]
    public void PlanNextOrder_PlannedOrderExists_Throws()
    {
        _subscription.PlanNextOrder(Now);

        Assert.That(() => _subscription.PlanNextOrder(Now), Throws.InvalidOperationException);
    }

    [Test]
    [Description("P7: calling PlanNextOrder on a paused subscription throws")]
    public void PlanNextOrder_SubscriptionPaused_Throws()
    {
        _subscription.Pause();

        Assert.That(() => _subscription.PlanNextOrder(Now), Throws.InvalidOperationException);
    }

    [Test]
    [Description("P8: a replaced package type throws and no order is added (not supported until FR-11)")]
    public void PlanNextOrder_PackageTypeReplaced_ThrowsUntilReplacementIsSupported()
    {
        _adultPackage.MarkReplaced();

        Assert.That(() => _subscription.PlanNextOrder(Now), Throws.ArgumentException);
        Assert.That(_subscription.Orders, Has.Count.EqualTo(1));
    }

    private static PackageType NewPackageType(string name) =>
        PackageType.Create(
            name,
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]);
}
