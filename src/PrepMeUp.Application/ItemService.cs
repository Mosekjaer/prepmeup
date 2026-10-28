using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public class ItemService(IItemRepository itemRepository)
{
    public Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken)
        => itemRepository.GetAllAsync(cancellationToken);
}
