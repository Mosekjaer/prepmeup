namespace PrepMeUp.Application;

public class OrderService(IOrderRepository orderRepository, IClock clock)
{
    public Task<IReadOnlyList<PlannedOrder>> GetPlannedAsync(CancellationToken cancellationToken)
    {
        return orderRepository.GetPlannedAsync(cancellationToken);
    }

    public async Task<MarkAsSentResult> MarkAsSentAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null)
        {
            return MarkAsSentResult.NotFound;
        }

        if (!order.IsPlanned)
        {
            return MarkAsSentResult.NotPlanned;
        }

        order.MarkAsSent(clock.UtcNow);

        try
        {
            await orderRepository.SaveChangesAsync(cancellationToken);
        }
        catch (OrderConflictException)
        {
            return MarkAsSentResult.NotPlanned;
        }

        return MarkAsSentResult.Sent;
    }
}