using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly DateTimeOffset StartedAt = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    [Test]
    public void Start_ValidCustomer_IsActiveWithCustomerAndStartTime()
    {
        var subscription = Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]);

        Assert.That(
            (subscription.Status, subscription.CustomerId, subscription.StartedAt),
            Is.EqualTo((SubscriptionStatus.Active, CustomerId, StartedAt)));
    }

    [Test]
    public void Start_ValidCustomer_HasExactlyOneOrder()
    {
        var subscription = Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]);

        Assert.That(subscription.Orders, Has.Count.EqualTo(1));
    }

    [Test]
    public void Start_ValidCustomer_FirstOrderIsPlannedAtStartAndNotSent()
    {
        var order = Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]).Orders.Single();

        Assert.That((order.PlannedAt, order.SentAt), Is.EqualTo((StartedAt, (DateTimeOffset?)null)));
    }

    private static OrderLine NewLine(string packageTypeName, int quantity) =>
        new(
            PackageType.Create(
                packageTypeName,
                [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]),
            quantity);

    [Test]
    public void Start_WithTwoLines_FirstOrderHasThoseLinesPlannedAtStartAndNotSent()
    {
        var adult = NewLine("Normal - Adult", 2);
        var child = NewLine("Normal - Child", 1);

        var order = Subscription.Start(CustomerId, StartedAt, [adult, child]).Orders.Single();

        Assert.Multiple(() =>
        {
            Assert.That(order.Lines, Is.EqualTo(new[] { adult, child }));
            Assert.That((order.PlannedAt, order.SentAt), Is.EqualTo((StartedAt, (DateTimeOffset?)null)));
        });
    }

    [Test]
    public void Start_NoLines_Throws()
    {
        Assert.That(() => Subscription.Start(CustomerId, StartedAt, []), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Start_SamePackageTypeTwice_Throws()
    {
        var packageType = NewLine("Normal - Adult", 1).PackageType;

        Assert.That(
            () => Subscription.Start(CustomerId, StartedAt, [new OrderLine(packageType, 1), new OrderLine(packageType, 2)]),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Start_EmptyCustomerId_Throws()
    {
        Assert.That(() => Subscription.Start(Guid.Empty, StartedAt, [NewLine("Normal - Adult", 1)]), Throws.TypeOf<ArgumentException>());
    }
}
