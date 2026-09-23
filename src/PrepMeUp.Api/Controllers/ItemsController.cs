using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Api.Contracts;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("items")]
[Authorize]
public class ItemsController : ControllerBase
{
    [HttpGet(Name = "Items_GetItems")]
    [ProducesResponseType<IEnumerable<ItemsResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<ItemsResponse>> GetItems() =>
    Ok(new[] { new ItemsResponse("Ida x2"), new ItemsResponse("Marie"), new ItemsResponse("Malthe") });

}
