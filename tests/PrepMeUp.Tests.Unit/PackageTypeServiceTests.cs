using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class PackageTypeServiceTests
{
    private static PackageType NewPackageType(string name) =>
        PackageType.Create(name, [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 1)]);

    [Test]
    public async Task GetActiveAsync_ActiveAndReplaced_ReturnsOnlyActive()
    {
        var active = NewPackageType("Normal - Adult");
        var replaced = NewPackageType("Normal - Child");
        replaced.MarkReplaced();
        var repository = Substitute.For<IPackageTypeRepository>();
        IReadOnlyList<PackageType> all = [active, replaced];
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(all);
        var sut = new PackageTypeService(repository);

        var result = await sut.GetActiveAsync(CancellationToken.None);

        Assert.That(result, Is.EqualTo(new[] { active }));
    }

    [Test]
    public async Task GetActiveAsync_NoPackageTypes_ReturnsEmpty()
    {
        var repository = Substitute.For<IPackageTypeRepository>();
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<PackageType>());
        var sut = new PackageTypeService(repository);

        var result = await sut.GetActiveAsync(CancellationToken.None);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetActiveAsync_OnlyReplaced_ReturnsEmpty()
    {
        var replaced = NewPackageType("Normal - Adult");
        replaced.MarkReplaced();
        var repository = Substitute.For<IPackageTypeRepository>();
        IReadOnlyList<PackageType> all = [replaced];
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(all);
        var sut = new PackageTypeService(repository);

        var result = await sut.GetActiveAsync(CancellationToken.None);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetActiveAsync_OneActive_ReturnsIt()
    {
        var active = NewPackageType("Normal - Adult");
        var repository = Substitute.For<IPackageTypeRepository>();
        IReadOnlyList<PackageType> all = [active];
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(all);
        var sut = new PackageTypeService(repository);

        var result = await sut.GetActiveAsync(CancellationToken.None);

        Assert.That(result, Is.EqualTo(new[] { active }));
    }
}
