using Microsoft.EntityFrameworkCore;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly PrepMeUpDbContext _dbContext;

    public SubscriptionRepository(PrepMeUpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Subscription?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return _dbContext.Subscriptions
            .Include(subscription => subscription.Orders)
            .SingleOrDefaultAsync(subscription => subscription.CustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        await _dbContext.Subscriptions.AddAsync(subscription, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
