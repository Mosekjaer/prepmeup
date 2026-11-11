namespace PrepMeUp.Domain;

public class PackageContent
{
    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    private PackageContent()
    {
        Item = null!;
    }

    public PackageContent(Item item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);

        Item = item;
        Quantity = quantity;
    }
}
