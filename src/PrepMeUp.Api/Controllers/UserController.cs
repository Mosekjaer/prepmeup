using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Api.Authentication;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController(CustomerService customerService) : ControllerBase
{
    [HttpGet(Name = "User_GetCustomer")]
    [ProducesResponseType<CustomerResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(CancellationToken cancellationToken)
    {
        var keycloakId = User.GetKeycloakId();
        if (keycloakId is null)
        {
            return Unauthorized();
        }

        var customer = await customerService.GetAsync(keycloakId, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(new CustomerResponse(
            customer.Name,
            customer.Email,
            customer.Phone,
            new AddressContract(customer.Address.Street, customer.Address.PostalCode, customer.Address.City)));
    }

    [HttpPut(Name = "User_SaveCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SaveCustomer(SaveCustomerRequest request, CancellationToken cancellationToken)
    {
        var keycloakId = User.GetKeycloakId();
        if (keycloakId is null)
        {
            return Unauthorized();
        }

        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        var phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone;
        var address = new Address(request.Address.Street, request.Address.PostalCode, request.Address.City);

        await customerService.SaveDetailsAsync(keycloakId, email, request.Name, phone, address, cancellationToken);
        return NoContent();
    }
}
