namespace PrepMeUp.Domain;

public class PackageContent
{
    public Item Item { get; }
    public int Quantity { get; }

    public PackageContent(Item item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);

        Item = item;
        Quantity = quantity;
    }
}
