namespace PrepMeUp.Api.Contracts;

public record OrderResponse(DateTimeOffset PlannedAt, DateTimeOffset? SentAt, IReadOnlyList<OrderLineResponse> Lines);

public record OrderLineResponse(Guid PackageTypeId, string PackageTypeName, int Quantity);
