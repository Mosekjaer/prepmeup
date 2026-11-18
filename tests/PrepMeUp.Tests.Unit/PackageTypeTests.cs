using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class PackageTypeTests
{
    private static Item NewItem(string name) => new() { Id = Guid.NewGuid(), Name = name, Type = ItemType.Food };

    [Test]
    public void Create_ValidInput_IsActiveWithName()
    {
        var packageType = PackageType.Create("Normal - Adult", [new PackageContent(NewItem("Water"), 5)]);

        Assert.That((packageType.Status, packageType.Name), Is.EqualTo((PackageTypeStatus.Active, "Normal - Adult")));
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    public void PackageContent_QuantityBelowOne_Throws(int quantity)
    {
        Assert.That(() => new PackageContent(NewItem("Water"), quantity), Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void PackageContent_NullItem_Throws()
    {
        Assert.That(
            () => new PackageContent(null!, 1),
            Throws.TypeOf<ArgumentNullException>().With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("item"));
    }

    [Test]
    public void Create_SameItemTwice_Throws()
    {
        var water = NewItem("Water");
        PackageContent[] contents = [new(water, 6), new(NewItem("Rice"), 2), new(water, 4)];

        Assert.That(() => PackageType.Create("Normal - Adult", contents), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Create_NameWithSurroundingSpaces_TrimsName()
    {
        var packageType = PackageType.Create(" Normal - Child ", [new PackageContent(NewItem("Water"), 1)]);

        Assert.That(packageType.Name, Is.EqualTo("Normal - Child"));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void Create_BlankName_Throws(string name)
    {
        Assert.That(() => PackageType.Create(name, [new PackageContent(NewItem("Water"), 1)]), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Create_NullName_Throws()
    {
        Assert.That(() => PackageType.Create(null!, [new PackageContent(NewItem("Water"), 1)]), Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void Create_SourceListChangedAfterwards_ContentsUnchanged()
    {
        var source = new List<PackageContent> { new(NewItem("Water"), 5) };
        var packageType = PackageType.Create("Normal - Adult", source);

        source.Add(new PackageContent(NewItem("Rice"), 2));

        Assert.That(packageType.Contents, Has.Count.EqualTo(1));
    }

    [Test]
    public void MarkReplaced_ActivePackageType_IsReplaced()
    {
        var packageType = PackageType.Create("Normal - Adult", [new PackageContent(NewItem("Water"), 5)]);

        packageType.MarkReplaced();

        Assert.That(packageType.Status, Is.EqualTo(PackageTypeStatus.Replaced));
    }

    [Test]
    public void MarkReplaced_ActivePackageType_KeepsNameAndContents()
    {
        PackageContent[] contents = [new(NewItem("Water"), 5), new(NewItem("Rice"), 2)];
        var packageType = PackageType.Create("Normal - Adult", contents);

        packageType.MarkReplaced();

        Assert.That((packageType.Name, packageType.Contents), Is.EqualTo(("Normal - Adult", contents)));
    }

    [Test]
    public void MarkReplaced_AlreadyReplaced_Throws()
    {
        var packageType = PackageType.Create("Normal - Adult", [new PackageContent(NewItem("Water"), 5)]);
        packageType.MarkReplaced();

        Assert.That(() => packageType.MarkReplaced(), Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Create_NoContents_Throws()
    {
        Assert.That(() => PackageType.Create("Normal - Adult", []), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Create_NullContents_Throws()
    {
        Assert.That(
            () => PackageType.Create("Normal - Adult", null!),
            Throws.TypeOf<ArgumentNullException>().With.Property(nameof(ArgumentNullException.ParamName)).EqualTo("contents"));
    }
}
