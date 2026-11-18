using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public class SubscriptionService(
    ICustomerRepository customerRepository,
    ISubscriptionRepository subscriptionRepository,
    IPackageTypeRepository packageTypeRepository,
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

    public async Task<SubscribeResult> SubscribeAsync(
        string keycloakId,
        IReadOnlyList<SubscribeLine> lines,
        CancellationToken cancellationToken)
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

        if (lines.Select(line => line.PackageTypeId).Distinct().Count() != lines.Count)
        {
            return SubscribeResult.DuplicatePackageType;
        }

        var packageTypes = await packageTypeRepository.GetAllAsync(cancellationToken);
        var orderLines = new List<OrderLine>();
        foreach (var line in lines)
        {
            var packageType = packageTypes.SingleOrDefault(candidate => candidate.Id == line.PackageTypeId);
            if (packageType is null || packageType.Status != PackageTypeStatus.Active)
            {
                return SubscribeResult.PackageTypeNotAvailable;
            }

            orderLines.Add(new OrderLine(packageType, line.Quantity));
        }

        await subscriptionRepository.AddAsync(Subscription.Start(customer.Id, clock.UtcNow, orderLines), cancellationToken);
        await subscriptionRepository.SaveChangesAsync(cancellationToken);

        return SubscribeResult.Subscribed;
    }

    // Method that fetches ALL active subscriptions and plans next orders for those needed.
    public async Task<int> PlanNextOrdersAsync(CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        // Get active subscriptions that soon needs nex order (checks are handled in repository)
        var subscriptions = await subscriptionRepository.GetReadyForNextOrderAsync(now, cancellationToken);

        foreach (var subscription in subscriptions)
        {
            subscription.PlanNextOrder(now);
        }

        if (subscriptions.Count > 0)
        {
            await subscriptionRepository.SaveChangesAsync(cancellationToken);
        }

        return subscriptions.Count;
    }

}
