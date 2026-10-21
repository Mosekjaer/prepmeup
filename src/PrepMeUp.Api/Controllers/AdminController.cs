using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrepMeUp.Api.Controllers;


[ApiController]
[Route("admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    //bruges til at tjekke om kaldet kommer fra en admin, og dermed om tokenet er validt. Kan bruges til at tjekke om admin er på browser, eller om en household er logget ind.
    [HttpGet("ping", Name = "Admin_Ping")]
    public IActionResult Ping() => Ok();
}
