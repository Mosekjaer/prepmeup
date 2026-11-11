using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class PackageTypeEndpointTests
{
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
    public async Task GetPackageTypes_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/package-types");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TestCase("household")]
    [TestCase("admin")]
    public async Task GetPackageTypes_AuthenticatedUser_ReturnsOk(string role)
    {
        var response = await GetWithRoleAsync("/package-types", role);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetPackageTypes_ReplacedPackageTypeExists_ReturnsOnlyActiveOnes()
    {
        await AddReplacedPackageTypeAsync("Replaced type");

        var response = await GetWithRoleAsync("/package-types", "household");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.That(
            body.RootElement.EnumerateArray().Select(packageType => packageType.GetProperty("name").GetString()),
            Is.EquivalentTo(new[] { "Normal - Adult", "Normal - Child" }));
    }

    [Test]
    public async Task GetPackageTypes_ContentOfPackageType_HasNameQuantityAndItemTypeButNoUnitOrPrice()
    {
        var response = await GetWithRoleAsync("/package-types", "household");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var content = body.RootElement[0].GetProperty("contents")[0];

        Assert.That(
            content.EnumerateObject().Select(property => property.Name),
            Is.EquivalentTo(new[] { "itemId", "itemName", "itemType", "quantity" }));
    }

    private async Task AddReplacedPackageTypeAsync(string name)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var packageType = PackageType.Create(name, [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Old item", Type = ItemType.Food }, 1)]);
        packageType.MarkReplaced();
        dbContext.PackageTypes.Add(packageType);
        await dbContext.SaveChangesAsync();
    }

    private Task<HttpResponseMessage> GetWithRoleAsync(string path, string role)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TestJwt.Create(TestJwt.TrustedKey, [role]));
        return _client.SendAsync(request);
    }
}
