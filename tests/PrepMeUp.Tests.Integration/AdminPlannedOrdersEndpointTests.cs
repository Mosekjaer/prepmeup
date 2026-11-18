using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class AdminPlannedOrdersEndpointTests
{
    private const string Path = "/admin/orders/planned";
    private static readonly Address Home = new("Finlandsgade 22", "8200", "Aarhus N");
    private static readonly DateTimeOffset Now = new(2026, 12, 1, 10, 0, 0, TimeSpan.Zero);

    private SqliteConnection _connection = null!;
    private SqliteApiFactory _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _factory = new SqliteApiFactory(_connection);
        _factory.EnsureDatabaseCreated();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        _connection.Dispose();
    }

    [Test]
    public async Task GetPlannedOrders_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync(Path);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetPlannedOrders_HouseholdRole_ReturnsForbidden()
    {
        var response = await GetWithTokenAsync(TestJwt.Create(TestJwt.TrustedKey, ["household"]));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task GetPlannedOrders_AdminAndNoOrders_ReturnsEmptyArray()
    {
        var response = await GetWithTokenAsync(AdminToken());

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(json.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Array));
            Assert.That(json.RootElement.GetArrayLength(), Is.Zero);
        });
    }

    [Test]
    public async Task GetPlannedOrders_AdminWithOrdersOfOtherCustomers_ReturnsPlannedOnesSorted()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(dbContext))[0];
            await PlannedOrderSeed.AddOrderAsync(dbContext, "sub-1", "Second", Home, Now.AddDays(1), (packageType, 1));
            await PlannedOrderSeed.AddOrderAsync(dbContext, "sub-2", "First", Home, Now, (packageType, 1));
            var sent = await PlannedOrderSeed.AddOrderAsync(dbContext, "sub-3", "Sent", Home, Now, (packageType, 1));
            await PlannedOrderSeed.MarkSentAsync(dbContext, sent, Now);
        }

        var response = await GetWithTokenAsync(AdminToken());

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(
                json.RootElement.EnumerateArray().Select(order => order.GetProperty("customerName").GetString()),
                Is.EqualTo(new[] { "First", "Second" }));
        });
    }

    [Test]
    public async Task GetPlannedOrders_Admin_ReturnsExpectedJsonShape()
    {
        string packageTypeName;
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(dbContext))[0];
            packageTypeName = packageType.Name;
            await PlannedOrderSeed.AddOrderAsync(
                dbContext, "sub-1", "Malthe", Home, new DateTimeOffset(2026, 12, 1, 10, 0, 0, TimeSpan.FromHours(1)), (packageType, 2));
        }

        var response = await GetWithTokenAsync(AdminToken());

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var order = json.RootElement[0];
        var address = order.GetProperty("address");
        var line = order.GetProperty("lines")[0];
        Assert.Multiple(() =>
        {
            Assert.That(order.GetProperty("orderId").ValueKind, Is.EqualTo(JsonValueKind.String));
            Assert.That(order.GetProperty("plannedAt").GetString(), Does.StartWith("2026-12-01T10:00:00").And.EndsWith("+01:00"));
            Assert.That(order.GetProperty("customerName").GetString(), Is.EqualTo("Malthe"));
            Assert.That(address.GetProperty("street").GetString(), Is.EqualTo("Finlandsgade 22"));
            Assert.That(address.GetProperty("postalCode").GetString(), Is.EqualTo("8200"));
            Assert.That(address.GetProperty("city").GetString(), Is.EqualTo("Aarhus N"));
            Assert.That(line.GetProperty("packageTypeName").GetString(), Is.EqualTo(packageTypeName));
            Assert.That(line.GetProperty("quantity").ValueKind, Is.EqualTo(JsonValueKind.Number));
            Assert.That(line.GetProperty("quantity").GetInt32(), Is.EqualTo(2));
        });
    }

    private static string AdminToken() => TestJwt.Create(TestJwt.TrustedKey, ["admin"]);

    private Task<HttpResponseMessage> GetWithTokenAsync(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, Path);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client.SendAsync(request);
    }
}
