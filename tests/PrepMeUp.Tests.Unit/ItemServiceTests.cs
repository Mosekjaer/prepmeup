using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class ItemServiceTests
{
    [Test]
    public async Task GetAllAsync_ReturnsItemsFromRepository()
    {
        var repository = Substitute.For<IItemRepository>();
        IReadOnlyList<Item> items = [new Item { Id = Guid.NewGuid(), Name = "Water" }];
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(items);
        var sut = new ItemService(repository);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.That(result, Is.SameAs(items));
    }
}
