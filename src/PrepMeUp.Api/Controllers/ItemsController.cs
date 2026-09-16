using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("items")]
[Authorize]
public class ItemsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetItems() => Ok(new[] { "Ida x2", "Marie", "Malthe" });
}
