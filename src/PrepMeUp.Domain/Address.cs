namespace PrepMeUp.Domain;

public sealed record Address
{
    public const int StreetMaxLength = 100;
    public const int PostalCodeLength = 4;
    public const int CityMaxLength = 50;

    public string Street { get; }
    public string PostalCode { get; }
    public string City { get; }

    public Address(string street, string postalCode, string city)
    {
        Street = Require(street, StreetMaxLength, nameof(street));
        PostalCode = RequirePostalCode(postalCode);
        City = Require(city, CityMaxLength, nameof(city));
    }

    private static string RequirePostalCode(string postalCode)
    {
        if (postalCode is null
            || postalCode.Length != PostalCodeLength
            || !postalCode.All(char.IsAsciiDigit)
            || postalCode[0] == '0')
        {
            throw new ArgumentException(
                $"postalCode must be {PostalCodeLength} digits between 1000 and 9999.", nameof(postalCode));
        }

        return postalCode;
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
