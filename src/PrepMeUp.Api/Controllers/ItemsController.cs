using Microsoft.AspNetCore.Mvc;

namespace PrepMeUp.Api.Controllers;

[ApiController] // marks this class as a controller ASP.NET Core should route requests to
[Route("items")] // sets the base URL path for this controller: /items.
public class ItemsController : ControllerBase // Controllerbase is the base class for API controllers
{
    [HttpGet] // this method answers GET requests
    public IActionResult GetItems() => Ok(new[] { "Ida x2", "Marie", "Malthe" });
}
