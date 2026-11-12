using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

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
        var response = await PostSubscriptionAsync(NewSub());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task PostSubscription_Onboarded_ReturnsCreatedWithActiveSubscriptionAndFirstOrder()
    {
        var sub = await OnboardAsync();

        var response = await PostSubscriptionAsync(sub);
        var body = await response.Content.ReadFromJsonAsync<SubscriptionResponse>(JsonOptions);

        Assert.That(
            (response.StatusCode, response.Headers.Location?.OriginalString, body?.Status, body?.Orders.Single().PlannedAt),
            Is.EqualTo((HttpStatusCode.Created, "/subscription", SubscriptionStatus.Active, body?.StartedAt)));
    }

    [Test]
    public async Task PostSubscription_ValidLines_ReturnsCreatedWithTheLinesOnTheFirstOrder()
    {
        var sub = await OnboardAsync();
        var packageTypes = await GetActivePackageTypesAsync(sub);
        var request = new { lines = new[] { new { packageTypeId = packageTypes[0].Id, quantity = 2 }, new { packageTypeId = packageTypes[1].Id, quantity = 1 } } };

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, request);
        var body = await response.Content.ReadFromJsonAsync<SubscriptionResponse>(JsonOptions);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(
                body?.Orders.Single().Lines.Select(line => (line.PackageTypeId, line.PackageTypeName, line.Quantity)),
                Is.EqualTo(new[]
                {
                    (packageTypes[0].Id, packageTypes[0].Name, 2),
                    (packageTypes[1].Id, packageTypes[1].Name, 1)
                }));
        });
    }

    [Test]
    public async Task PostSubscription_NoLines_ReturnsBadRequest()
    {
        var sub = await OnboardAsync();

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, new { lines = Array.Empty<object>() });

        await AssertNotSubscribedAsync(sub, response, HttpStatusCode.BadRequest);
    }

    [TestCase(0)]
    [TestCase(-1)]
    public async Task PostSubscription_QuantityBelowOne_ReturnsBadRequest(int quantity)
    {
        var sub = await OnboardAsync();
        var packageTypes = await GetActivePackageTypesAsync(sub);
        var request = new { lines = new[] { new { packageTypeId = packageTypes[0].Id, quantity } } };

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, request);

        await AssertNotSubscribedAsync(sub, response, HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task PostSubscription_UnknownPackageType_ReturnsBadRequestAndCreatesNoSubscription()
    {
        var sub = await OnboardAsync();
        var request = new { lines = new[] { new { packageTypeId = Guid.NewGuid(), quantity = 1 } } };

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, request);
        var subscription = await SendAsync(HttpMethod.Get, "/subscription", sub);

        Assert.That(
            (response.StatusCode, subscription.StatusCode),
            Is.EqualTo((HttpStatusCode.BadRequest, HttpStatusCode.NotFound)));
    }

    [Test]
    public async Task PostSubscription_ReplacedPackageType_ReturnsBadRequest()
    {
        var sub = await OnboardAsync();
        var replaced = PackageType.Create(
            "Replaced test type",
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 3)]);
        replaced.MarkReplaced();
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            dbContext.PackageTypes.Add(replaced);
            await dbContext.SaveChangesAsync();
        }

        var request = new { lines = new[] { new { packageTypeId = replaced.Id, quantity = 1 } } };
        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, request);

        await AssertNotSubscribedAsync(sub, response, HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task PostSubscription_SamePackageTypeTwice_ReturnsBadRequest()
    {
        var sub = await OnboardAsync();
        var packageTypes = await GetActivePackageTypesAsync(sub);
        var request = new { lines = new[] { new { packageTypeId = packageTypes[0].Id, quantity = 1 }, new { packageTypeId = packageTypes[0].Id, quantity = 2 } } };

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, request);

        await AssertNotSubscribedAsync(sub, response, HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task PostSubscription_LinesMissing_ReturnsBadRequestAndCreatesNoSubscription()
    {
        var sub = await OnboardAsync();

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub, new { });

        await AssertNotSubscribedAsync(sub, response, HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task PostSubscription_NoBody_ReturnsUnsupportedMediaTypeAndCreatesNoSubscription()
    {
        var sub = await OnboardAsync();

        var response = await SendAsync(HttpMethod.Post, "/subscription", sub);

        await AssertNotSubscribedAsync(sub, response, HttpStatusCode.UnsupportedMediaType);
    }

    [Test]
    public async Task PostSubscription_Onboarded_SendsStatusAsText()
    {
        var sub = await OnboardAsync();

        var response = await PostSubscriptionAsync(sub);

        Assert.That(await response.Content.ReadAsStringAsync(), Does.Contain("\"status\":\"Active\""));
    }

    [Test]
    public async Task PostSubscription_AlreadySubscribed_ReturnsConflict()
    {
        var sub = await OnboardAsync();
        await PostSubscriptionAsync(sub);

        var second = await PostSubscriptionAsync(sub);

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
        var created = await PostSubscriptionAsync(sub);

        var response = await SendAsync(HttpMethod.Get, "/subscription", sub);

        var createdBody = await created.Content.ReadAsStringAsync();
        Assert.That(
            (response.StatusCode, await response.Content.ReadAsStringAsync()),
            Is.EqualTo((HttpStatusCode.OK, createdBody)));
        Assert.That(createdBody, Does.Contain("\"packageTypeName\"").And.Contain("\"quantity\":1"));
    }

    private static string NewSub() => Guid.NewGuid().ToString();

    private async Task AssertNotSubscribedAsync(string sub, HttpResponseMessage response, HttpStatusCode expected)
    {
        var subscription = await SendAsync(HttpMethod.Get, "/subscription", sub);

        Assert.That((response.StatusCode, subscription.StatusCode), Is.EqualTo((expected, HttpStatusCode.NotFound)));
    }

    private async Task<HttpResponseMessage> PostSubscriptionAsync(string sub)
    {
        var packageTypes = await GetActivePackageTypesAsync(sub);
        var request = new { lines = new[] { new { packageTypeId = packageTypes[0].Id, quantity = 1 } } };
        return await SendAsync(HttpMethod.Post, "/subscription", sub, request);
    }

    private async Task<List<PackageTypeResponse>> GetActivePackageTypesAsync(string sub)
    {
        var response = await SendAsync(HttpMethod.Get, "/package-types", sub);
        return (await response.Content.ReadFromJsonAsync<List<PackageTypeResponse>>(JsonOptions))!;
    }

    private async Task<string> OnboardAsync()
    {
        var sub = NewSub();
        var body = new SaveCustomerRequest("Malthe", null, new AddressContract("Finlandsgade 22", "8200", "Aarhus N"));
        var response = await SendAsync(HttpMethod.Put, "/user", sub, body);
        response.EnsureSuccessStatusCode();
        return sub;
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string path, string sub, object? body = null)
    {
        var request = new HttpRequestMessage(method, path);
        var token = TestJwt.Create(TestJwt.TrustedKey, ["household"], additionalClaims: new() { ["sub"] = sub, ["email"] = "malthe@example.com" });
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body);
        }

        return _client.SendAsync(request);
    }
}
