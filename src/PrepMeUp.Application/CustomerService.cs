using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public class CustomerService(ICustomerRepository customerRepository)
{
    public Task<Customer?> GetAsync(string keycloakId, CancellationToken cancellationToken)
        => customerRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);

    public async Task SaveDetailsAsync(
    string keycloakId,
    string email,
    string name,
    string? phone,
    Address address,
    CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);

        if (customer is null)
        {
            await customerRepository.AddAsync(new Customer(keycloakId, name, email, phone, address), cancellationToken);
        }
        else
        {
            customer.UpdateDetails(name, phone, address);
        }

        await customerRepository.SaveChangesAsync(cancellationToken);
    }
}
