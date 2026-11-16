using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class OrderTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly DateTimeOffset StartedAt = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    [Test]
    public void IsPlanned_NewOrder_ReturnsTrue()
    {
        var order = Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]).Orders.Single();

        Assert.That(order.IsPlanned, Is.True);
    }

    [Test]
    public void IsPlannedExpression_NewOrder_MatchesOrder()
    {
        var order = Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]).Orders.Single();

        Assert.That(Order.IsPlannedExpression.Compile()(order), Is.True);
    }

    private static OrderLine NewLine(string packageTypeName, int quantity) =>
        new(
            PackageType.Create(
                packageTypeName,
                [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]),
            quantity);
}
