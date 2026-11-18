using PrepMeUp.Domain;

namespace PrepMeUp.Application;

public class PackageTypeService(IPackageTypeRepository packageTypeRepository)
{
    public async Task<IReadOnlyList<PackageType>> GetActiveAsync(CancellationToken cancellationToken)
    {
        var packageTypes = await packageTypeRepository.GetAllAsync(cancellationToken);

        return packageTypes.Where(packageType => packageType.Status == PackageTypeStatus.Active).ToList();
    }
}
