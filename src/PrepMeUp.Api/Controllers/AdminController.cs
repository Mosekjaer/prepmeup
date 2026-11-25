using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Application;
using PrepMeUp.Api.Contracts;

namespace PrepMeUp.Api.Controllers;


[ApiController]
[Route("admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController(OrderService orderService) : ControllerBase
{
    // Used to check whether the call comes from an admin, and so whether the token is valid. Can be used to check whether an admin is in the browser, or whether a household user is logged in.
    [HttpGet("ping", Name = "Admin_Ping")]
    public IActionResult Ping() => Ok();

    [HttpGet("orders/planned", Name = "Admin_GetPlannedOrders")]
    [ProducesResponseType<IEnumerable<PlannedOrderResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<PlannedOrderResponse>>> GetPlannedOrders(CancellationToken cancellationToken)
    {
        var orders = await orderService.GetPlannedAsync(cancellationToken);
        return Ok(orders.Select(PlannedOrderResponse.From));
    }

    [HttpPost("orders/{orderId:guid}/shipment", Name = "Admin_MarkOrderShipped")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> MarkOrderShipped(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await orderService.MarkAsSentAsync(orderId, cancellationToken);

        return result switch
        {
            MarkAsSentResult.Sent => NoContent(),
            MarkAsSentResult.NotFound => NotFound(),
            MarkAsSentResult.NotPlanned => Conflict(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }

}
