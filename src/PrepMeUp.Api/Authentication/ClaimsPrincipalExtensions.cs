using System.Security.Claims;

namespace PrepMeUp.Api.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static string? GetKeycloakId(this ClaimsPrincipal principal)
    {
        var keycloakId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return string.IsNullOrWhiteSpace(keycloakId) ? null : keycloakId;
    }
}
