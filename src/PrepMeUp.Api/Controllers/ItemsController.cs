using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("items")]
[Authorize] // A client must be logged in to make this request
public class ItemsController(IItemRepository itemRepository) : ControllerBase
{
    [HttpGet(Name = "Items_GetItems")]
    [ProducesResponseType<IEnumerable<ItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItems(CancellationToken cancellationToken)
    {
        var items = await itemRepository.GetAllAsync(cancellationToken);
        return Ok(items.Select(item => new ItemResponse(item.Id, item.Name)));
    }
}
