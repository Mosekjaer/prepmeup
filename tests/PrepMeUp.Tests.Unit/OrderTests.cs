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

    [Test]
    public void MarkAsSent_PlannedOrder_SetsSentAtAndIsNoLongerPlanned()
    {
        var order = NewPlannedOrder();
        var sentAt = StartedAt.AddDays(1);

        order.MarkAsSent(sentAt);

        Assert.That(order.SentAt, Is.EqualTo(sentAt));
        Assert.That(order.IsPlanned, Is.False);
    }

    [Test]
    public void MarkAsSent_AlreadySent_ThrowsAndKeepsFirstSentAt()
    {
        var order = NewPlannedOrder();
        var firstSentAt = StartedAt.AddDays(1);
        order.MarkAsSent(firstSentAt);

        Assert.That(() => order.MarkAsSent(firstSentAt.AddDays(1)), Throws.InvalidOperationException);
        Assert.That(order.SentAt, Is.EqualTo(firstSentAt));
    }

    [Test]
    public void MarkAsSent_SameTimestampTwice_Throws()
    {
        var order = NewPlannedOrder();
        var sentAt = StartedAt.AddDays(1);
        order.MarkAsSent(sentAt);

        Assert.That(() => order.MarkAsSent(sentAt), Throws.InvalidOperationException);
    }

    [Test]
    public void IsPlannedExpression_SentOrder_DoesNotMatchOrder()
    {
        var order = NewPlannedOrder();
        order.MarkAsSent(StartedAt.AddDays(1));

        Assert.That(Order.IsPlannedExpression.Compile()(order), Is.False);
    }


    private static OrderLine NewLine(string packageTypeName, int quantity) =>
        new(
            PackageType.Create(
                packageTypeName,
                [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]),
            quantity);

    private static Order NewPlannedOrder() =>
       Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]).Orders.Single();

}
