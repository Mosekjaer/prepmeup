using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace PrepMeUp.Tests.Integration;

public static class TestJwt
{
    public static RsaSecurityKey TrustedKey { get; } = NewKey();
    public static RsaSecurityKey UntrustedKey { get; } = NewKey();

    public static string Create(RsaSecurityKey signingKey, string[]? roles, DateTime? expires = null,
        string issuer = AuthenticatedApiFactory.Issuer, string audience = AuthenticatedApiFactory.Audience,
        Dictionary<string, object>? additionalClaims = null)
    {
        var expiry = expires ?? DateTime.UtcNow.AddMinutes(5);
        var claims = new Dictionary<string, object>(additionalClaims ?? []);
        if (roles is not null)
        {
            claims["realm_access"] = new Dictionary<string, object> { ["roles"] = roles };
        }

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            NotBefore = expiry.AddMinutes(-10),
            Expires = expiry,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256),
            Claims = claims
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    private static RsaSecurityKey NewKey() => new(RSA.Create(2048));
}
