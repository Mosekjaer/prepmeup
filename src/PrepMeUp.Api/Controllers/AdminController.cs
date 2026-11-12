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
    //bruges til at tjekke om kaldet kommer fra en admin, og dermed om tokenet er validt. Kan bruges til at tjekke om admin er på browser, eller om en household er logget ind.
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
}
