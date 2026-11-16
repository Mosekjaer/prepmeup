namespace PrepMeUp.Application;

public interface IOrderRepository
{
    Task<IReadOnlyList<PlannedOrder>> GetPlannedAsync(CancellationToken cancellationToken);
}