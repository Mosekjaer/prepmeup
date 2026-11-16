using Microsoft.EntityFrameworkCore;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public static class PlannedOrderSeed
{
    public static async Task<IReadOnlyList<PackageType>> ActivePackageTypesAsync(PrepMeUpDbContext dbContext)
    {
        return await dbContext.PackageTypes.Where(type => type.Status == PackageTypeStatus.Active).ToListAsync();
    }

    public static async Task<Order> AddOrderAsync(
        PrepMeUpDbContext dbContext,
        string keycloakId,
        string customerName,
        Address address,
        DateTimeOffset plannedAt,
        params (PackageType PackageType, int Quantity)[] lines)
    {
        var customer = new Customer(keycloakId, customerName, $"{keycloakId}@example.com", null, address);
        dbContext.Customers.Add(customer);
        var subscription = Subscription.Start(
            customer.Id,
            plannedAt,
            lines.Select(line => new OrderLine(line.PackageType, line.Quantity)).ToList());
        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync();

        return subscription.Orders.Single();
    }

    public static async Task MarkSentAsync(PrepMeUpDbContext dbContext, Order order, DateTimeOffset sentAt)
    {
        dbContext.Entry(order).Property(nameof(Order.SentAt)).CurrentValue = sentAt;
        await dbContext.SaveChangesAsync();
    }
}
