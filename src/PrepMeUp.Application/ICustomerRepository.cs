using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public interface ICustomerRepository
{
    Task<Customer?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
