using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Application;
using PrepMeUp.Domain;
using PrepMeUp.Infrastructure;

namespace PrepMeUp.Tests.Integration;

public class PackageTypePersistenceTests
{
    private SqliteConnection _connection = null!;

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    [TearDown]
    public void TearDown()
    {
        _connection.Dispose();
    }

    [Test]
    public async Task SaveChanges_PackageTypeWithContents_ContentsAndQuantitiesAreReadBack()
    {
        var water = new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food };
        var torch = new Item { Id = Guid.NewGuid(), Name = "Torch", Type = ItemType.Equipment };
        var packageType = PackageType.Create("Test package type", [new PackageContent(water, 6), new PackageContent(torch, 1)]);
        using var factory = CreateFactory();
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            dbContext.PackageTypes.Add(packageType);
            await dbContext.SaveChangesAsync();
        }

        using var readScope = factory.Services.CreateScope();
        var packageTypes = await readScope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None);

        Assert.That(
            packageTypes.Single(saved => saved.Id == packageType.Id).Contents.Select(content => (content.Item.Id, content.Item.Type, content.Quantity)),
            Is.EquivalentTo(new[] { (water.Id, ItemType.Food, 6), (torch.Id, ItemType.Equipment, 1) }));
    }

    [Test]
    public async Task SaveChanges_ReplacedPackageType_NameAndStatusAreReadBack()
    {
        var packageType = PackageType.Create("Test replaced type", [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 3)]);
        packageType.MarkReplaced();
        using var factory = CreateFactory();
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
            dbContext.PackageTypes.Add(packageType);
            await dbContext.SaveChangesAsync();
        }

        using var readScope = factory.Services.CreateScope();
        var packageTypes = await readScope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None);

        Assert.That(
            packageTypes.Where(saved => saved.Id == packageType.Id).Select(saved => (saved.Name, saved.Status)),
            Is.EqualTo(new[] { ("Test replaced type", PackageTypeStatus.Replaced) }));
    }

    [Test]
    public async Task GetAllAsync_PackageTypes_AreOrderedByName()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();

        var packageTypes = await scope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None);

        Assert.That(packageTypes.Select(packageType => packageType.Name), Is.Ordered);
    }

    [Test]
    public async Task NewDatabase_HasOneActiveAdultAndOneActiveChildPackageType()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();

        var packageTypes = await scope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None);

        Assert.That(
            packageTypes.Select(packageType => (packageType.Name, packageType.Status)),
            Is.EquivalentTo(new[]
            {
                ("Normal - Adult", PackageTypeStatus.Active),
                ("Normal - Child", PackageTypeStatus.Active)
            }));
    }

    [Test]
    public async Task NewDatabase_EveryStandardPackageTypeHasContentWithPositiveQuantity()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();

        var packageTypes = await scope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None);

        Assert.That(
            packageTypes.Count > 0 && packageTypes.All(packageType => packageType.Contents.Count > 0 && packageType.Contents.All(content => content.Quantity > 0)),
            Is.True);
    }

    [Test]
    public async Task Database_SameItemTwiceInOnePackageType_IsRejected()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var existing = (await scope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None)).First();
        var content = existing.Contents.First();

        Assert.That(
            () => dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO PackageContents (Id, ItemId, PackageTypeId, Quantity) VALUES ({Guid.NewGuid()}, {content.Item.Id}, {existing.Id}, 1)"),
            Throws.InstanceOf<SqliteException>());
    }

    [Test]
    public async Task Database_QuantityBelowOne_IsRejected()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PrepMeUpDbContext>();
        var existing = (await scope.ServiceProvider.GetRequiredService<IPackageTypeRepository>()
            .GetAllAsync(CancellationToken.None)).First();
        var newItem = new Item { Id = Guid.NewGuid(), Name = "Extra item", Type = ItemType.Food };
        dbContext.Items.Add(newItem);
        await dbContext.SaveChangesAsync();

        Assert.That(
            () => dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO PackageContents (Id, ItemId, PackageTypeId, Quantity) VALUES ({Guid.NewGuid()}, {newItem.Id}, {existing.Id}, 0)"),
            Throws.InstanceOf<SqliteException>());
    }

    private SqliteApiFactory CreateFactory()
    {
        var factory = new SqliteApiFactory(_connection);
        factory.EnsureDatabaseCreated();
        return factory;
    }
}
