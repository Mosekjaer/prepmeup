using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public class SubscriptionService(
    ICustomerRepository customerRepository,
    ISubscriptionRepository subscriptionRepository,
    IClock clock)
{
    public async Task<Subscription?> GetAsync(string keycloakId, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        return await subscriptionRepository.GetByCustomerIdAsync(customer.Id, cancellationToken);
    }

    public async Task<SubscribeResult> SubscribeAsync(string keycloakId, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        if (customer is null)
        {
            return SubscribeResult.CustomerNotFound;
        }

        if (await subscriptionRepository.GetByCustomerIdAsync(customer.Id, cancellationToken) is not null)
        {
            return SubscribeResult.AlreadySubscribed;
        }

        await subscriptionRepository.AddAsync(Subscription.Start(customer.Id, clock.UtcNow), cancellationToken);
        await subscriptionRepository.SaveChangesAsync(cancellationToken);

        return SubscribeResult.Subscribed;
    }
}
