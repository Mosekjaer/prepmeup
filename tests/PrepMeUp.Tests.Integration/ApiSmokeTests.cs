using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PrepMeUp.Tests.Integration;

/// <summary>
/// Boots the real API pipeline with <see cref="WebApplicationFactory{TEntryPoint}"/> and checks
/// that it answers. Replace the database registration here once Infrastructure is wired up.
/// </summary>
public class ApiSmokeTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task HealthEndpoint_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
