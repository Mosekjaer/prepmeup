using System.Linq.Expressions;

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

    public void Pause()
    {
        Status = SubscriptionStatus.Paused;
    }

    public static Expression<Func<Subscription, bool>> NextOrderShouldBePlannedExpression(DateTimeOffset now)
    {
        var latestAllowedExpiry = now.AddDays(Order.ReorderDaysBeforeExpiry); // The latest expiry-time we include this time around.

        return subscription =>
            // Check 1: the subscription is active
            subscription.Status == SubscriptionStatus.Active
            // Check 2: no order is waiting to be sent
            && subscription.Orders.All(order => order.SentAt != null)
            // Check 3: the latest sent order has reached its reorder date (14 days before it expires)
            && subscription.Orders.Max(order => order.ExpiresAt) <= latestAllowedExpiry;
    }

    public bool NextOrderShouldBePlanned(DateTimeOffset now) =>
        NextOrderShouldBePlannedExpression(now).Compile()(this);

    public void PlanNextOrder(DateTimeOffset now)
    {
        // If subscription isn't even active -> trow exception
        if (Status != SubscriptionStatus.Active)
        {
            throw new InvalidOperationException("Only an active subscription can get a next order.");
        }

        // If there is already a pending order -> trow exception
        if (_orders.Any(order => order.IsPlanned))
        {
            throw new InvalidOperationException("The subscription already has an order that is not sent.");
        }

        var previousOrder = _orders.MaxBy(order => order.SentAt)!;  // Look into this subscriptions own order-list and get last sent one.
        var lines = previousOrder.Lines.Select(line => new OrderLine(line.PackageType, line.Quantity));
        _orders.Add(new Order(now, lines)); // Add orderline-objects from previous order to this new one
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
