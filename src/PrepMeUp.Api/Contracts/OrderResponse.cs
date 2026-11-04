namespace PrepMeUp.Api.Contracts;

public record OrderResponse(DateTimeOffset PlannedAt, DateTimeOffset? SentAt);
