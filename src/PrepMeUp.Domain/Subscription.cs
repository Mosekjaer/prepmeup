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

    public static Subscription Start(Guid customerId, DateTimeOffset startedAt, IEnumerable<OrderLine> lines)
    {
        var lineList = lines.ToList();
        if (lineList.Count == 0)
        {
            throw new ArgumentException("An order needs at least one line.", nameof(lines));
        }

        if (lineList.Select(line => line.PackageType.Id).Distinct().Count() != lineList.Count)
        {
            throw new ArgumentException("A package type can only appear once in an order.", nameof(lines));
        }

        return Start(customerId, startedAt, new Order(startedAt, lineList));
    }

    private static Subscription Start(Guid customerId, DateTimeOffset startedAt, Order firstOrder)
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
        subscription._orders.Add(firstOrder);

        return subscription;
    }
}
