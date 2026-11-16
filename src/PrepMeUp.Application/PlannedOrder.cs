using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public record PlannedOrder(
    Guid OrderId,
    DateTimeOffset PlannedAt,
    string CustomerName,
    Address Address,
    IReadOnlyList<PlannedOrderLine> Lines);

public record PlannedOrderLine(string PackageTypeName, int Quantity);