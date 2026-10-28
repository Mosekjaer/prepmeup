using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
