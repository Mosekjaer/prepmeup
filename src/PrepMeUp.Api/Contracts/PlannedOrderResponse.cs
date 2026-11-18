using PrepMeUp.Application;

namespace PrepMeUp.Api.Contracts;

public record PlannedOrderResponse(
    Guid OrderId,
    DateTimeOffset PlannedAt,
    string CustomerName,
    AddressContract Address,
    IReadOnlyList<PlannedOrderLineResponse> Lines)
{
    public static PlannedOrderResponse From(PlannedOrder order) => new(
        order.OrderId,
        order.PlannedAt,
        order.CustomerName,
        new AddressContract(order.Address.Street, order.Address.PostalCode, order.Address.City),
        order.Lines.Select(line => new PlannedOrderLineResponse(line.PackageTypeName, line.Quantity)).ToList());
}

public record PlannedOrderLineResponse(string PackageTypeName, int Quantity);