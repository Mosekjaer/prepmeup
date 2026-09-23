using PrepMeUp.Domain;

namespace PrepMeUp.Application;

// Define interface for self that infrastructure will implement and make work.
public interface IItemRepository
{
    Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken);
}

// CancellationToken = standard ASP.NET Core practice so a request can be aborted (e.g. client disconnects) without the DB query running to completion pointlessly