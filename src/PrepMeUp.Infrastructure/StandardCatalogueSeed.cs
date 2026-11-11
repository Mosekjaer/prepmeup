using Microsoft.EntityFrameworkCore;
using PrepMeUp.Domain;

namespace PrepMeUp.Infrastructure;

internal static class StandardCatalogueSeed
{
    private static readonly Guid WaterId = Guid.Parse("7034103d-0cce-40d5-a06a-345b47276ded");
    private static readonly Guid CannedFoodId = Guid.Parse("cd127a73-703c-4717-88bf-7726dd09944c");
    private static readonly Guid CrispbreadId = Guid.Parse("8e33e585-870c-4f60-bd6e-995bc86ef2c6");

    private static readonly Guid AdultId = Guid.Parse("dbd55106-cbca-4197-bf2b-e1b21b36e7bd");
    private static readonly Guid ChildId = Guid.Parse("52f89479-6e11-4698-8602-e7cbe63004ec");

    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().HasData(
            new Item { Id = WaterId, Name = "Water", Type = ItemType.Food },
            new Item { Id = CannedFoodId, Name = "Canned food", Type = ItemType.Food },
            new Item { Id = CrispbreadId, Name = "Crispbread", Type = ItemType.Food });

        modelBuilder.Entity<PackageType>().HasData(
            new { Id = AdultId, Name = "Normal - Adult", Status = PackageTypeStatus.Active },
            new { Id = ChildId, Name = "Normal - Child", Status = PackageTypeStatus.Active });

        modelBuilder.Entity<PackageContent>().HasData(
            new { Id = Guid.Parse("d2a0dd13-1f5e-448b-a748-44b769996004"), PackageTypeId = AdultId, ItemId = WaterId, Quantity = 6 },
            new { Id = Guid.Parse("257ad596-d194-4b4a-a713-465cc4da0155"), PackageTypeId = AdultId, ItemId = CannedFoodId, Quantity = 4 },
            new { Id = Guid.Parse("940be6bb-ef0f-4cbf-92c4-b0faa27177af"), PackageTypeId = AdultId, ItemId = CrispbreadId, Quantity = 2 },
            new { Id = Guid.Parse("701ffdee-edbc-4b6c-a8f9-88d11ef486e9"), PackageTypeId = ChildId, ItemId = WaterId, Quantity = 4 },
            new { Id = Guid.Parse("963b24e5-4ff6-4a05-a617-c057c01e86cf"), PackageTypeId = ChildId, ItemId = CannedFoodId, Quantity = 3 },
            new { Id = Guid.Parse("a3c1f0b2-5d7e-4c19-8b6a-2e9f4d1c7a30"), PackageTypeId = ChildId, ItemId = CrispbreadId, Quantity = 2 });
    }
}
