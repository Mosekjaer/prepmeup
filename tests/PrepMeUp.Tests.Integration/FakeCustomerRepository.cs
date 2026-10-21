using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Integration;

public class FakeCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<string, Customer> _customers = [];

    public Task<Customer?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_customers.GetValueOrDefault(keycloakId));
    }

    public Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        _customers.Add(customer.KeycloakId, customer);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
