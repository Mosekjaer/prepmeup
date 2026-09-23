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
    [ProducesResponseType<IEnumerable<ItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<ItemResponse>> GetItems() =>
        Ok(new[] { new ItemResponse("Ida x2"), new ItemResponse("Marie"), new ItemResponse("Malthe") });
}
