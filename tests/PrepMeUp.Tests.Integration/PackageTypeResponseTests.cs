using PrepMeUp.Api.Contracts;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Integration;

public class PackageTypeResponseTests
{
    [Test]
    public void From_ContentsInAnyOrder_AreOrderedByItemName()
    {
        var packageType = PackageType.Create(
            "Normal - Adult",
            [
                new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 6),
                new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Crispbread", Type = ItemType.Food }, 2),
                new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Canned food", Type = ItemType.Food }, 4)
            ]);

        var response = PackageTypeResponse.From(packageType);

        Assert.That(
            response.Contents.Select(content => content.ItemName),
            Is.EqualTo(new[] { "Canned food", "Crispbread", "Water" }));
    }
}
