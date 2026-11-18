using System.Net;
using Microsoft.Data.Sqlite;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;
using System.Text.Json;

namespace PrepMeUp.Tests.Integration;

public class AdminMarkOrderShippedEndpointTests
{
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
    public async Task MarkShipped_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.PostAsync($"/admin/orders/{Guid.NewGuid()}/shipment", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task MarkShipped_HouseholdRole_ReturnsForbidden()
    {
        var response = await PostWithTokenAsync(Guid.NewGuid(), TestJwt.Create(TestJwt.TrustedKey, ["household"]));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    private Task<HttpResponseMessage> PostWithTokenAsync(Guid orderId, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/admin/orders/{orderId}/shipment");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client.SendAsync(request);
    }

    [Test]
    public async Task MarkShipped_AdminAndPlannedOrder_ReturnsNoContentAndSetsSentAt()
    {
        var orderId = await SeedPlannedOrderAsync();

        var response = await PostWithTokenAsync(orderId, AdminToken());

        Assert.Multiple(async () =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(await SentAtAsync(orderId), Is.Not.Null);
        });
    }

    private static string AdminToken() => TestJwt.Create(TestJwt.TrustedKey, ["admin"]);

    private async Task<Guid> SeedPlannedOrderAsync(string keycloakId = "sub-1")
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(dbContext))[0];
        var order = await PlannedOrderSeed.AddOrderAsync(dbContext, keycloakId, "Malthe", Home, Now, (packageType, 1));
        return order.Id;
    }

    private async Task<DateTimeOffset?> SentAtAsync(Guid orderId)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var order = await dbContext.Orders.AsNoTracking().SingleAsync(row => row.Id == orderId);
        return order.SentAt;
    }

    [Test]
    public async Task MarkShipped_AdminAndPlannedOrder_OrderIsNoLongerInPlannedList()
    {
        var orderId = await SeedPlannedOrderAsync();

        await PostWithTokenAsync(orderId, AdminToken());

        var plannedOrderIds = await PlannedOrderIdsAsync();
        Assert.That(plannedOrderIds, Does.Not.Contain(orderId));
    }

    private async Task<List<Guid>> PlannedOrderIdsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/orders/planned");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken());
        var response = await _client.SendAsync(request);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.EnumerateArray()
            .Select(order => order.GetProperty("orderId").GetGuid())
            .ToList();
    }

    [Test]
    public async Task MarkShipped_AdminAndUnknownOrder_ReturnsNotFound()
    {
        var response = await PostWithTokenAsync(Guid.NewGuid(), AdminToken());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task MarkShipped_AdminAndAlreadySentOrder_ReturnsConflictAndKeepsFirstSentAt()
    {
        var firstSentAt = Now.AddDays(-1);
        var orderId = await SeedSentOrderAsync(firstSentAt);

        var response = await PostWithTokenAsync(orderId, AdminToken());

        Assert.Multiple(async () =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(await SentAtAsync(orderId), Is.EqualTo(firstSentAt));
        });
    }

    private async Task<Guid> SeedSentOrderAsync(DateTimeOffset sentAt)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var packageType = (await PlannedOrderSeed.ActivePackageTypesAsync(dbContext))[0];
        var order = await PlannedOrderSeed.AddOrderAsync(dbContext, "sub-1", "Malthe", Home, Now, (packageType, 1));
        await PlannedOrderSeed.MarkSentAsync(dbContext, order, sentAt);
        return order.Id;
    }

    [Test]
    public async Task MarkShipped_CalledTwiceInARow_ReturnsNoContentThenConflict()
    {
        var orderId = await SeedPlannedOrderAsync();

        var first = await PostWithTokenAsync(orderId, AdminToken());
        var firstSentAt = await SentAtAsync(orderId);
        var second = await PostWithTokenAsync(orderId, AdminToken());

        Assert.Multiple(async () =>
        {
            Assert.That(first.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(await SentAtAsync(orderId), Is.EqualTo(firstSentAt));
        });
    }

    [Test]
    public async Task MarkShipped_AdminAndIdThatIsNotAGuid_ReturnsNotFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/admin/orders/not-a-guid/shipment");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken());

        var response = await _client.SendAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task MarkShipped_AdminAndTwoPlannedOrders_OnlyTheRequestedOneIsSent()
    {
        var shippedId = await SeedPlannedOrderAsync("sub-1");
        var otherId = await SeedPlannedOrderAsync("sub-2");

        await PostWithTokenAsync(shippedId, AdminToken());

        Assert.Multiple(async () =>
        {
            Assert.That(await SentAtAsync(shippedId), Is.Not.Null);
            Assert.That(await SentAtAsync(otherId), Is.Null);
        });
    }
}
