using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController(ICustomerRepository customerRepository, CustomerService customerService) : ControllerBase
{
    [HttpGet(Name = "User_GetCustomer")]
    [ProducesResponseType<CustomerResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByKeycloakIdAsync(KeycloakId, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(new CustomerResponse(customer.Name, customer.Address));
    }

    [HttpPut(Name = "User_SaveCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SaveCustomer(SaveCustomerRequest request, CancellationToken cancellationToken)
    {
        await customerService.SaveDetailsAsync(KeycloakId, request.Name, request.Address, cancellationToken);
        return NoContent();
    }

    private string KeycloakId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
