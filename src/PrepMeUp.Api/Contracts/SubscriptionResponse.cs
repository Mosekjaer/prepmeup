using PrepMeUp.Domain;

namespace PrepMeUp.Api.Contracts;

public record SubscriptionResponse(SubscriptionStatus Status, DateTimeOffset StartedAt, IReadOnlyList<OrderResponse> Orders)
{
    public static SubscriptionResponse From(Subscription subscription) => new(
        subscription.Status,
        subscription.StartedAt,
        subscription.Orders.Select(order => new OrderResponse(order.PlannedAt, order.SentAt)).ToList());
}
