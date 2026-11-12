using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public interface IPackageTypeRepository
{
    Task<IReadOnlyList<PackageType>> GetAllAsync(CancellationToken cancellationToken);
}
