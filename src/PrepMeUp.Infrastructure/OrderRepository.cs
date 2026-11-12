using Microsoft.EntityFrameworkCore;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class OrderRepository(PrepMeUpDbContext dbContext) : IOrderRepository
{
    public async Task<IReadOnlyList<PlannedOrder>> GetPlannedAsync(CancellationToken cancellationToken)
    {
        var rows = await (
                from order in dbContext.Set<Order>()
                join subscription in dbContext.Subscriptions on order.SubscriptionId equals subscription.Id
                join customer in dbContext.Customers on subscription.CustomerId equals customer.Id
                where order.SentAt == null
                select new //creating anonymous object for each row. Reason: we need to fetch street, postal code and city seperately
                {
                    order.Id,
                    order.PlannedAt,
                    customer.Name,
                    customer.Address.Street,
                    customer.Address.PostalCode,
                    customer.Address.City,
                    Lines = order.Lines
                        .Select(line => new PlannedOrderLine(line.PackageType.Name, line.Quantity))
                        .ToList()
                })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows
            .OrderBy(row => row.PlannedAt)
            .Select(row => new PlannedOrder(
                row.Id,
                row.PlannedAt,
                row.Name,
                new Address(row.Street, row.PostalCode, row.City), //combining street, postalcode and city into address
                row.Lines))
            .ToList();
    }
}
