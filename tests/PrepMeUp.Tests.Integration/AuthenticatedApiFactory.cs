using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace PrepMeUp.Tests.Integration;

public class AuthenticatedApiFactory : WebApplicationFactory<Program>
{
    public const string Issuer = "https://keycloak.test/realms/prepmeup";
    public const string Audience = "prepmeup-api";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Keycloak:Authority", Issuer);
        builder.UseSetting("Keycloak:Audience", Audience);

        builder.ConfigureServices(services => services.PostConfigure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.Configuration = new OpenIdConnectConfiguration();
                options.TokenValidationParameters.IssuerSigningKey = TestJwt.TrustedKey;
            }));
    }
}
