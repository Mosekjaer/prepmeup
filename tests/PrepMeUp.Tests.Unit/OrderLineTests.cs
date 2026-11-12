using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class OrderLineTests
{
    private static PackageType NewPackageType() =>
        PackageType.Create(
            "Normal - Adult",
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]);

    [Test]
    public void Constructor_ActivePackageTypeAndQuantityOne_KeepsBoth()
    {
        var packageType = NewPackageType();

        var line = new OrderLine(packageType, 1);

        Assert.That((line.PackageType, line.Quantity), Is.EqualTo((packageType, 1)));
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    public void Constructor_QuantityBelowOne_Throws(int quantity)
    {
        Assert.That(() => new OrderLine(NewPackageType(), quantity), Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void Constructor_ReplacedPackageType_Throws()
    {
        var packageType = NewPackageType();
        packageType.MarkReplaced();

        Assert.That(() => new OrderLine(packageType, 1), Throws.TypeOf<ArgumentException>());
    }
}
