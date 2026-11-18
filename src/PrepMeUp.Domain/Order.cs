using System.Linq.Expressions;

namespace PrepMeUp.Domain;

public class Order
{
    public static Expression<Func<Order, bool>> IsPlannedExpression { get; } = order => order.SentAt == null;
    private static readonly Func<Order, bool> IsPlannedCheck = IsPlannedExpression.Compile();
    private readonly List<OrderLine> _lines = [];

    public Guid Id { get; init; }
    public Guid SubscriptionId { get; private set; }
    public DateTimeOffset PlannedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; } // Type DateTimeOffset -> avoid having to deal with time zones
    public IReadOnlyList<OrderLine> Lines => _lines;
    public bool IsPlanned => IsPlannedCheck(this);

    private Order()
    {
    }

    internal Order(DateTimeOffset plannedAt, IEnumerable<OrderLine> lines)
    {
        PlannedAt = plannedAt;
        _lines = lines.ToList();
    }
    
    public void MarkAsSent(DateTimeOffset sentAt)
    {
        if (!IsPlanned)
        {
            throw new InvalidOperationException("Only a planned order can be marked as sent.");
        }

        SentAt = sentAt;
        ExpiresAt = sentAt.AddYears(1); // AddYears() is a .NET built-in function that takes leap year into account.
    }
}
