using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Integration;

public class SubscriptionEndpointTests
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    private SqliteConnection _connection = null!;
    private SqliteApiFactory _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _factory = new SqliteApiFactory(_connection);
        _factory.EnsureDatabaseCreated();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        _connection.Dispose();
    }

    [Test]
    public async Task PostSubscription_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.PostAsync("/subscription", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task PostSubscription_NotOnboarded_ReturnsNotFound()
    {
        var response = await SendAsync(HttpMethod.Post, "/subscription", NewSub());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task PostSubscription_Onboarded_ReturnsCreatedWithActiveSubscriptionAndFirstOrder()
    {
        var sub = await OnboardAsync();

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub);
        var body = await response.Content.ReadFromJsonAsync<SubscriptionResponse>(JsonOptions);

        Assert.That(
            (response.StatusCode, response.Headers.Location?.OriginalString, body?.Status, body?.Orders.Single().PlannedAt),
            Is.EqualTo((HttpStatusCode.Created, "/subscription", SubscriptionStatus.Active, body?.StartedAt)));
    }

    [Test]
    public async Task PostSubscription_Onboarded_SendsStatusAsText()
    {
        var sub = await OnboardAsync();

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub);

        Assert.That(await response.Content.ReadAsStringAsync(), Does.Contain("\"status\":\"Active\""));
    }

    [Test]
    public async Task PostSubscription_AlreadySubscribed_ReturnsConflict()
    {
        var sub = await OnboardAsync();
        await SendAsync(HttpMethod.Post, "/subscription", sub);

        var second = await SendAsync(HttpMethod.Post, "/subscription", sub);

        Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task GetSubscription_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/subscription");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetSubscription_NotSubscribed_ReturnsNotFound()
    {
        var sub = await OnboardAsync();

        var response = await SendAsync(HttpMethod.Get, "/subscription", sub);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetSubscription_AfterSubscribe_ReturnsOkWithSameSubscription()
    {
        var sub = await OnboardAsync();
        var created = await SendAsync(HttpMethod.Post, "/subscription", sub);

        var response = await SendAsync(HttpMethod.Get, "/subscription", sub);

        Assert.That(
            (response.StatusCode, await response.Content.ReadAsStringAsync()),
            Is.EqualTo((HttpStatusCode.OK, await created.Content.ReadAsStringAsync())));
    }

    private static string NewSub() => Guid.NewGuid().ToString();

    private async Task<string> OnboardAsync()
    {
        var sub = NewSub();
        await SendAsync(HttpMethod.Put, "/user", sub, new { Name = "Malthe", Address = "Finlandsgade 22" });
        return sub;
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, string sub, object? body = null)
    {
        var request = new HttpRequestMessage(method, path);
        var token = TestJwt.Create(TestJwt.TrustedKey, ["household"], additionalClaims: new() { ["sub"] = sub });
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body);
        }

        return _client.SendAsync(request);
    }
}
