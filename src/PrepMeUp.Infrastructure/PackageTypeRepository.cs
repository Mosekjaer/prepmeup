using Microsoft.EntityFrameworkCore;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

public class PackageTypeRepository(PrepMeUpDbContext dbContext) : IPackageTypeRepository
{
    public async Task<IReadOnlyList<PackageType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.PackageTypes
            .Include(packageType => packageType.Contents)
            .ThenInclude(content => content.Item)
            .AsNoTracking()
            .OrderBy(packageType => packageType.Name)
            .ToListAsync(cancellationToken);
    }
}
