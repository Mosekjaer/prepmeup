namespace PrepMeUp.Application;

public class OrderService(IOrderRepository orderRepository)
{
    public Task<IReadOnlyList<PlannedOrder>> GetPlannedAsync(CancellationToken cancellationToken)
    {
        return orderRepository.GetPlannedAsync(cancellationToken);
    }
}