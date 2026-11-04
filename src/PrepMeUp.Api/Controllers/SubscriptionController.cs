using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("subscription")]
[Authorize]
public class SubscriptionController(SubscriptionService subscriptionService) : ControllerBase
{
    [HttpGet(Name = "Subscription_GetSubscription")]
    [ProducesResponseType<SubscriptionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubscriptionResponse>> GetSubscription(CancellationToken cancellationToken)
    {
        var subscription = await subscriptionService.GetAsync(KeycloakId, cancellationToken);
        if (subscription is null)
        {
            return NotFound();
        }

        return Ok(SubscriptionResponse.From(subscription));
    }

    [HttpPost(Name = "Subscription_Subscribe")]
    [ProducesResponseType<SubscriptionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SubscriptionResponse>> Subscribe(CancellationToken cancellationToken)
    {
        var result = await subscriptionService.SubscribeAsync(KeycloakId, cancellationToken);

        switch (result)
        {
            case SubscribeResult.CustomerNotFound:
                return NotFound();
            case SubscribeResult.AlreadySubscribed:
                return Conflict();
        }

        var subscription = await subscriptionService.GetAsync(KeycloakId, cancellationToken);
        return Created("/subscription", SubscriptionResponse.From(subscription!));
    }

    private string KeycloakId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
