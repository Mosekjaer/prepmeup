using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Api.Controllers;
using PrepMeUp.Application;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class ItemsEndpointTests
{
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
    public async Task GetItems_ItemsInDatabase_ReturnsIdAndNameOfEachItem()
    {
        var expected = await ItemsInDatabaseAsync();

        var items = await CallControllerDirectlyAsync();

        Assert.That(
            items.Select(item => (item.Id, item.Name)),
            Is.EquivalentTo(expected.Select(item => (item.Id, item.Name))));
    }

    [Test]
    public async Task GetItems_EmptyTable_ReturnsEmptyList()
    {
        await DeleteAllItemsAsync();

        var items = await CallControllerDirectlyAsync();

        Assert.That(items, Is.Empty);
    }

    [Test]
    public async Task GetItems_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/items");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetItems_TokenSignedWithUntrustedKey_ReturnsUnauthorized()
    {
        var response = await GetWithTokenAsync(TestJwt.Create(TestJwt.UntrustedKey, ["household"]));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetItems_ValidToken_ReturnsItemsAsJson()
    {
        var expected = await ItemsInDatabaseAsync();

        var response = await GetWithTokenAsync(TestJwt.Create(TestJwt.TrustedKey, ["household"]));
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(
                body.RootElement.EnumerateArray().Select(item =>
                    (item.GetProperty("id").GetGuid(), item.GetProperty("name").GetString())),
                Is.EquivalentTo(expected.Select(item => (item.Id, (string?)item.Name))));
        });
    }

    private async Task<IReadOnlyList<ItemResponse>> CallControllerDirectlyAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var controller = new ItemsController(scope.ServiceProvider.GetRequiredService<ItemService>());

        var result = await controller.GetItems(CancellationToken.None);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);
        return ((IEnumerable<ItemResponse>)ok!.Value!).ToList();
    }

    private async Task<IReadOnlyList<Item>> ItemsInDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var items = await dbContext.Items.AsNoTracking().ToListAsync();
        Assert.That(items, Is.Not.Empty);
        return items;
    }

    private async Task DeleteAllItemsAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        await dbContext.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF; DELETE FROM Items;");
    }

    private Task<HttpResponseMessage> GetWithTokenAsync(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/items");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client.SendAsync(request);
    }
}
