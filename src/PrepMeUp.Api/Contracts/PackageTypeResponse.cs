using PrepMeUp.Domain;

namespace PrepMeUp.Api.Contracts;

public record PackageTypeResponse(Guid Id, string Name, IReadOnlyList<PackageContentResponse> Contents)
{
    public static PackageTypeResponse From(PackageType packageType) => new(
        packageType.Id,
        packageType.Name,
        packageType.Contents
            .OrderBy(content => content.Item.Name)
            .Select(content => new PackageContentResponse(content.Item.Id, content.Item.Name, content.Item.Type, content.Quantity))
            .ToList());
}

public record PackageContentResponse(Guid ItemId, string ItemName, ItemType ItemType, int Quantity);
