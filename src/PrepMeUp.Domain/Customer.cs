namespace PrepMeUp.Domain;

public class Customer
{
    public const int NameMaxLength = 100;
    public const int EmailMaxLength = 254;
    public const int PhoneLength = 8;

    public Guid Id { get; init; }
    public string KeycloakId { get; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public Address Address { get; private set; }

    public Customer(string keycloakId, string name, string email, string? phone, Address address)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            throw new ArgumentException("keycloakId is required.", nameof(keycloakId));
        }

        KeycloakId = keycloakId;
        Name = Require(name, NameMaxLength, nameof(name));
        Email = RequireEmail(email);
        Phone = RequirePhone(phone);
        Address = address ?? throw new ArgumentNullException(nameof(address));
    }

    private Customer()
    {
        KeycloakId = null!;
        Name = null!;
        Email = null!;
        Address = null!;
    }
    public void UpdateDetails(string name, string? phone, Address address)
    {
        var validName = Require(name, NameMaxLength, nameof(name));
        var validPhone = RequirePhone(phone);
        ArgumentNullException.ThrowIfNull(address);

        Name = validName;
        Phone = validPhone;
        Address = address;
    }

    private static string RequireEmail(string email)
    {
        var validEmail = Require(email, EmailMaxLength, nameof(email));

        if (!validEmail.Contains('@'))
        {
            throw new ArgumentException("email must contain '@'.", nameof(email));
        }

        return validEmail;
    }

    private static string? RequirePhone(string? phone)
    {
        if (phone is null)
        {
            return null;
        }

        if (phone.Length != PhoneLength || !phone.All(char.IsAsciiDigit))
        {
            throw new ArgumentException($"phone must be {PhoneLength} digits.", nameof(phone));
        }

        return phone;
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
