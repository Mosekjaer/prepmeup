using Microsoft.EntityFrameworkCore;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class ItemRepository : IItemRepository
{
    private readonly PrepMeUpDbContext _dbContext;

    public ItemRepository(PrepMeUpDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // Fetch all rows of Items table and load all the rows into a C# List<Item>
    public async Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Items.ToListAsync(cancellationToken);
    }
}