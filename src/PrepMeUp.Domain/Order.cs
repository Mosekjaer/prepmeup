namespace PrepMeUp.Domain;

public class Order
{
    public Guid Id { get; init; }
    public Guid SubscriptionId { get; private set; }
    public DateTimeOffset PlannedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }

    internal Order(DateTimeOffset plannedAt)
    {
        PlannedAt = plannedAt;
    }
}
