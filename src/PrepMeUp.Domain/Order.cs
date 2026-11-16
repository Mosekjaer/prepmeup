using System.Linq.Expressions;

namespace PrepMeUp.Domain;

public class Order
{
    public static Expression<Func<Order, bool>> IsPlannedExpression{ get; } = order => order.SentAt == null;
    private static readonly Func<Order, bool> IsPlannedCheck = IsPlannedExpression.Compile();
    private readonly List<OrderLine> _lines = [];

    public Guid Id { get; init; }
    public Guid SubscriptionId { get; private set; }
    public DateTimeOffset PlannedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
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
}
