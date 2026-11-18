using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PrepMeUp.Tests.Integration;

public class StartupConfigurationTests
{
    private const string ConnectionString = "Server=db,1433;Database=PrepMeUp;User Id=sa;Password=unused";

    [Test]
    public void Production_WithoutKeycloakAuthority_RefusesToStart()
    {
        using var factory = CreateFactory("Production", authority: "", connectionString: ConnectionString);

        Assert.That(() => factory.CreateClient(),
            Throws.InvalidOperationException.With.Message.Contains("Keycloak:Authority"));
    }

    [Test]
    public void Production_WithoutConnectionString_RefusesToStart()
    {
        using var factory = CreateFactory("Production", authority: AuthenticatedApiFactory.Issuer, connectionString: "");

        Assert.That(() => factory.CreateClient(),
            Throws.InvalidOperationException.With.Message.Contains("ConnectionStrings:PrepMeUpDb"));
    }

    [Test]
    public async Task Development_WithoutRequiredValues_Starts()
    {
        using var factory = CreateFactory("Development", authority: "", connectionString: "");
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private static WebApplicationFactory<Program> CreateFactory(string environment, string authority, string connectionString) =>
        new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(environment);
            builder.UseSetting("Keycloak:Authority", authority);
            builder.UseSetting("ConnectionStrings:PrepMeUpDb", connectionString);
        });
}
