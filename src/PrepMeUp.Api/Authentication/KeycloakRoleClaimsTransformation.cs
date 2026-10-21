using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace PrepMeUp.Api.Authentication;

public class KeycloakRoleClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var realmAccess = principal.FindFirst("realm_access");
        if (principal.Identity?.IsAuthenticated != true || realmAccess is null)
        {
            return Task.FromResult(principal);
        }

        var roleClaims = ReadRoles(realmAccess.Value)
            .Where(role => !principal.IsInRole(role))
            .Select(role => new Claim(ClaimTypes.Role, role))
            .ToList();

        if (roleClaims.Count > 0)
        {
            principal.AddIdentity(new ClaimsIdentity(roleClaims));
        }

        return Task.FromResult(principal);
    }

    private static IEnumerable<string> ReadRoles(string realmAccessJson)
    {
        try
        {
            using var document = JsonDocument.Parse(realmAccessJson);

            if (!document.RootElement.TryGetProperty("roles", out var roles)
                || roles.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            return roles
                .EnumerateArray()
                .Where(role => role.ValueKind == JsonValueKind.String)
                .Select(role => role.GetString()!)
                .Distinct()
                .ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
