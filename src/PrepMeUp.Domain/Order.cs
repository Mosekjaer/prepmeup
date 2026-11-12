namespace PrepMeUp.Domain;

public class Order
{
    private readonly List<OrderLine> _lines = [];

    public Guid Id { get; init; }
    public Guid SubscriptionId { get; private set; }
    public DateTimeOffset PlannedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public IReadOnlyList<OrderLine> Lines => _lines;

    private Order()
    {
    }

    internal Order(DateTimeOffset plannedAt, IEnumerable<OrderLine> lines)
    {
        PlannedAt = plannedAt;
        _lines = lines.ToList();
    }
}
