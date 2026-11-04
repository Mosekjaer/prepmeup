namespace PrepMeUp.Domain;

public class Subscription
{
    private readonly List<Order> _orders = [];

    public Guid Id { get; init; }
    public Guid CustomerId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public IReadOnlyList<Order> Orders => _orders;

    private Subscription()
    {
    }

    public static Subscription Start(Guid customerId, DateTimeOffset startedAt)
    {
        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("customerId is required.", nameof(customerId));
        }

        var subscription = new Subscription
        {
            CustomerId = customerId,
            Status = SubscriptionStatus.Active,
            StartedAt = startedAt
        };
        subscription._orders.Add(new Order(startedAt));

        return subscription;
    }
}
