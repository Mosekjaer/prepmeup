using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly DateTimeOffset StartedAt = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    [Test]
    public void Start_ValidCustomer_IsActiveWithCustomerAndStartTime()
    {
        var subscription = Subscription.Start(CustomerId, StartedAt);

        Assert.That(
            (subscription.Status, subscription.CustomerId, subscription.StartedAt),
            Is.EqualTo((SubscriptionStatus.Active, CustomerId, StartedAt)));
    }

    [Test]
    public void Start_ValidCustomer_HasExactlyOneOrder()
    {
        var subscription = Subscription.Start(CustomerId, StartedAt);

        Assert.That(subscription.Orders, Has.Count.EqualTo(1));
    }

    [Test]
    public void Start_ValidCustomer_FirstOrderIsPlannedAtStartAndNotSent()
    {
        var order = Subscription.Start(CustomerId, StartedAt).Orders.Single();

        Assert.That((order.PlannedAt, order.SentAt), Is.EqualTo((StartedAt, (DateTimeOffset?)null)));
    }

    [Test]
    public void Start_EmptyCustomerId_Throws()
    {
        Assert.That(() => Subscription.Start(Guid.Empty, StartedAt), Throws.TypeOf<ArgumentException>());
    }
}
