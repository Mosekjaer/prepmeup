using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;

namespace PrepMeUp.Tests.Integration;

public class UserEndpointTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var repository = new FakeCustomerRepository();
        _factory = new AuthenticatedApiFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services => services.AddSingleton<ICustomerRepository>(repository)));
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetUser_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/user");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetUser_NotOnboarded_ReturnsNotFound()
    {
        var response = await SendAsync(HttpMethod.Get, NewSub());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task PutUser_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("/user", new { Name = "Malthe", Address = "Finlandsgade 22" });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task PutUser_FirstOnboarding_ReturnsNoContentAndCustomerCanBeRead()
    {
        var sub = NewSub();

        var put = await SendAsync(HttpMethod.Put, sub, new { Name = "Malthe", Address = "Finlandsgade 22" });
        var customer = await GetCustomerAsync(sub);

        Assert.That(put.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(customer, Is.EqualTo(new CustomerResponse("Malthe", "Finlandsgade 22")));
    }

    [Test]
    public async Task PutUser_Twice_KeepsTheLatestDetails()
    {
        var sub = NewSub();

        await SendAsync(HttpMethod.Put, sub, new { Name = "Malthe", Address = "Finlandsgade 22" });
        var second = await SendAsync(HttpMethod.Put, sub, new { Name = "Malthe Winther", Address = "Nørrebrogade 1" });
        var customer = await GetCustomerAsync(sub);

        Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(customer, Is.EqualTo(new CustomerResponse("Malthe Winther", "Nørrebrogade 1")));
    }

    [TestCase("", "Finlandsgade 22")]
    [TestCase("Malthe", "")]
    public async Task PutUser_BlankField_ReturnsBadRequest(string name, string address)
    {
        var response = await SendAsync(HttpMethod.Put, NewSub(), new { Name = name, Address = address });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase(101, 1)]
    [TestCase(1, 201)]
    public async Task PutUser_FieldOverMaxLength_ReturnsBadRequest(int nameLength, int addressLength)
    {
        var body = new { Name = new string('a', nameLength), Address = new string('a', addressLength) };

        var response = await SendAsync(HttpMethod.Put, NewSub(), body);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetUser_AnotherUserIsOnboarded_ReturnsNotFound()
    {
        await SendAsync(HttpMethod.Put, NewSub(), new { Name = "Malthe", Address = "Finlandsgade 22" });

        var response = await SendAsync(HttpMethod.Get, NewSub());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private static string NewSub() => Guid.NewGuid().ToString();

    private async Task<CustomerResponse?> GetCustomerAsync(string sub)
    {
        var response = await SendAsync(HttpMethod.Get, sub);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<CustomerResponse>() : null;
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string sub, object? body = null)
    {
        var request = new HttpRequestMessage(method, "/user");
        var token = TestJwt.Create(TestJwt.TrustedKey, ["household"], additionalClaims: new() { ["sub"] = sub });
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body);
        }

        return _client.SendAsync(request);
    }
}
