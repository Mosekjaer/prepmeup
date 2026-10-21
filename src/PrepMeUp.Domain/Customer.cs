namespace PrepMeUp.Domain;

public class Customer
{
    public const int NameMaxLength = 100;
    public const int AddressMaxLength = 200;

    public Guid Id { get; init; }
    public string KeycloakId { get; }
    public string Name { get; private set; }
    public string Address { get; private set; }

    public Customer(string keycloakId, string name, string address)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            throw new ArgumentException("keycloakId is required.", nameof(keycloakId));
        }

        KeycloakId = keycloakId;
        Name = Require(name, NameMaxLength, nameof(name));
        Address = Require(address, AddressMaxLength, nameof(address));
    }

    public void UpdateDetails(string name, string address)
    {
        var validName = Require(name, NameMaxLength, nameof(name));
        var validAddress = Require(address, AddressMaxLength, nameof(address));

        Name = validName;
        Address = validAddress;
    }

    private static string Require(string value, int maxLength, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} is required.", paramName);
        }

        if (value.Length > maxLength)
        {
            throw new ArgumentException($"{paramName} must be at most {maxLength} characters.", paramName);
        }

        return value;
    }
}
