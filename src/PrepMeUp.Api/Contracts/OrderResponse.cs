namespace PrepMeUp.Api.Contracts;

public record OrderResponse(
    DateTimeOffset PlannedAt,
    DateTimeOffset? SentAt,
    DateTimeOffset? ExpiresAt,
    IReadOnlyList<OrderLineResponse> Lines);

public record OrderLineResponse(Guid PackageTypeId, string PackageTypeName, int Quantity);
