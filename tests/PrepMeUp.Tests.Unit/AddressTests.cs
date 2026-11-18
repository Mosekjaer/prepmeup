using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class AddressTests
{
    private const string Street = "Finlandsgade 22";
    private const string PostalCode = "8200";
    private const string City = "Aarhus N";

    [Test]
    public void Constructor_ValidDetails_SetsProperties()
    {
        var address = new Address(Street, PostalCode, City);

        Assert.That(
            (address.Street, address.PostalCode, address.City),
            Is.EqualTo((Street, PostalCode, City)));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankStreet_Throws(string? street)
    {
        Assert.That(() => new Address(street!, PostalCode, City), Throws.TypeOf<ArgumentException>());
    }

    [TestCase(1)]
    [TestCase(Address.StreetMaxLength)]
    public void Constructor_StreetLengthWithinLimit_IsAccepted(int length)
    {
        var street = new string('a', length);

        var address = new Address(street, PostalCode, City);

        Assert.That(address.Street, Is.EqualTo(street));
    }

    [Test]
    public void Constructor_StreetLongerThanMax_Throws()
    {
        Assert.That(
            () => new Address(new string('a', Address.StreetMaxLength + 1), PostalCode, City),
            Throws.TypeOf<ArgumentException>());
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankCity_Throws(string? city)
    {
        Assert.That(() => new Address(Street, PostalCode, city!), Throws.TypeOf<ArgumentException>());
    }

    [TestCase(1)]
    [TestCase(Address.CityMaxLength)]
    public void Constructor_CityLengthWithinLimit_IsAccepted(int length)
    {
        var city = new string('a', length);

        var address = new Address(Street, PostalCode, city);

        Assert.That(address.City, Is.EqualTo(city));
    }

    [Test]
    public void Constructor_CityLongerThanMax_Throws()
    {
        Assert.That(
            () => new Address(Street, PostalCode, new string('a', Address.CityMaxLength + 1)),
            Throws.TypeOf<ArgumentException>());
    }

    [TestCase("1000")]
    [TestCase("9999")]
    public void Constructor_PostalCodeOnBoundary_IsAccepted(string postalCode)
    {
        var address = new Address(Street, postalCode, City);

        Assert.That(address.PostalCode, Is.EqualTo(postalCode));
    }

    [TestCase("0999")]
    [TestCase("999")]
    [TestCase("10000")]
    [TestCase("82a0")]
    [TestCase(" 8200")]
    [TestCase("")]
    [TestCase(null)]
    public void Constructor_InvalidPostalCode_Throws(string? postalCode)
    {
        Assert.That(() => new Address(Street, postalCode!, City), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Equals_SameValues_AreEqual()
    {
        var first = new Address(Street, PostalCode, City);
        var second = new Address(Street, PostalCode, City);

        Assert.That(first, Is.EqualTo(second).And.Not.SameAs(second));
    }
}
