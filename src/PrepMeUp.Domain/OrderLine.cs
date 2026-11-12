namespace PrepMeUp.Domain;

public class OrderLine
{
    public PackageType PackageType { get; private set; }
    public int Quantity { get; private set; }

    private OrderLine()
    {
        PackageType = null!;
    }

    public OrderLine(PackageType packageType, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);

        if (packageType.Status != PackageTypeStatus.Active)
        {
            throw new ArgumentException("A package type that has been replaced cannot be ordered.", nameof(packageType));
        }

        PackageType = packageType;
        Quantity = quantity;
    }
}
