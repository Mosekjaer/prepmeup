using System.Net;
using System.Net.Http.Headers;

namespace PrepMeUp.Tests.Integration;

public class AdminEndpointTests
{
    private AuthenticatedApiFactory _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new AuthenticatedApiFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetAdminPing_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/admin/ping");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAdminPing_TokenIsNotAJwt_ReturnsUnauthorized()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/ping");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-jwt");

        var response = await _client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAdminPing_SignedWithUntrustedKey_ReturnsUnauthorized()
    {
        var token = TestJwt.Create(TestJwt.UntrustedKey, ["admin"]);

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAdminPing_ExpiredToken_ReturnsUnauthorized()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["admin"], DateTime.UtcNow.AddMinutes(-5));

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAdminPing_WrongAudience_ReturnsUnauthorized()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["admin"], audience: "another-api");

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAdminPing_WrongIssuer_ReturnsUnauthorized()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["admin"], issuer: "https://other.test/realms/other");

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetAdminPing_HouseholdRole_ReturnsForbidden()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["household"]);

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetAdminPing_NoRoles_ReturnsForbidden()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, null);

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetAdminPing_AdminRole_ReturnsOk()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["admin"]);

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAdminPing_HouseholdAndAdminRoles_ReturnsOk()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["household", "admin"]);

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAdminPing_AdminRoleWithDifferentCase_ReturnsForbidden()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, ["Admin"]);

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetAdminPing_AdminOnlyAsResourceAccessRole_ReturnsForbidden()
    {
        var resourceAccess = new Dictionary<string, object>
        {
            ["prepmeup-admin"] = new Dictionary<string, object> { ["roles"] = new[] { "admin" } }
        };
        var token = TestJwt.Create(
            TestJwt.TrustedKey,
            null,
            additionalClaims: new Dictionary<string, object> { ["resource_access"] = resourceAccess });

        var response = await GetWithToken("/admin/ping", token);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    private Task<HttpResponseMessage> GetWithToken(string path, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client.SendAsync(request);
    }
}
