using PrepMeUp.Application;

namespace PrepMeUp.Api.BackgroundServices;

public class NextOrderPlanner(IServiceScopeFactory scopeFactory, ILogger<NextOrderPlanner> logger) : BackgroundService
{
    public static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var planned = await scope.ServiceProvider.GetRequiredService<SubscriptionService>()
                    .PlanNextOrdersAsync(stoppingToken);
                logger.LogInformation("Planned {Count} next orders", planned);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                logger.LogError(exception, "Planning next orders failed");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}
