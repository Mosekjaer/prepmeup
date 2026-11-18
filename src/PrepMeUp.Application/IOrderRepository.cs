using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public interface IOrderRepository
{
    Task<IReadOnlyList<PlannedOrder>> GetPlannedAsync(CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}