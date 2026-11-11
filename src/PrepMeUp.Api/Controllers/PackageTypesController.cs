using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;

namespace PrepMeUp.Api.Controllers;

[ApiController]
[Route("package-types")]
[Authorize]
public class PackageTypesController(PackageTypeService packageTypeService) : ControllerBase
{
    [HttpGet(Name = "PackageTypes_GetActivePackageTypes")]
    [ProducesResponseType<IEnumerable<PackageTypeResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PackageTypeResponse>>> GetActivePackageTypes(CancellationToken cancellationToken)
    {
        var packageTypes = await packageTypeService.GetActiveAsync(cancellationToken);
        return Ok(packageTypes.Select(PackageTypeResponse.From));
    }
}
