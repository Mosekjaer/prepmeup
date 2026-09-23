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
    [HttpGet]
    public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
    {
        var items = await itemRepository.GetAllAsync(cancellationToken);
        var response = items.Select(item => new ItemResponse(item.Id, item.Name));
        return Ok(response);
    }
}