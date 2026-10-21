using Microsoft.EntityFrameworkCore;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class CustomerRepository : ICustomerRepository
{
    private readonly PrepMeUpDbContext _dbContext;

    public CustomerRepository(PrepMeUpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Customer?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken)
    {
        return _dbContext.Customers.SingleOrDefaultAsync(customer => customer.KeycloakId == keycloakId, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _dbContext.Customers.AddAsync(customer, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
