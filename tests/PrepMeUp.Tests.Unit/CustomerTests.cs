using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class CustomerTests
{
    private const string KeycloakId = "sub-1";
    private const string Name = "Malthe Winther";
    private const string Email = "malthe@example.com";
    private const string Phone = "12345678";
    private static readonly Address Home = new("Finlandsgade 22", "8200", "Aarhus N");

    private static Customer CreateCustomer(
        string keycloakId = KeycloakId,
        string name = Name,
        string email = Email,
        string? phone = null,
        Address? address = null)
        => new(keycloakId, name, email, phone, address ?? Home);

    [Test]
    public void Constructor_ValidDetails_SetsProperties()
    {
        var customer = new Customer(KeycloakId, Name, Email, Phone, Home);

        Assert.That(
            (customer.KeycloakId, customer.Name, customer.Email, customer.Phone, customer.Address),
            Is.EqualTo((KeycloakId, Name, Email, Phone, Home)));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankKeycloakId_Throws(string? keycloakId)
    {
        Assert.That(() => CreateCustomer(keycloakId: keycloakId!), Throws.TypeOf<ArgumentException>());
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankName_Throws(string? name)
    {
        Assert.That(() => CreateCustomer(name: name!), Throws.TypeOf<ArgumentException>());
    }

    [TestCase(1)]
    [TestCase(Customer.NameMaxLength)]
    public void Constructor_NameLengthWithinLimit_IsAccepted(int length)
    {
        var name = new string('a', length);

        var customer = CreateCustomer(name: name);

        Assert.That(customer.Name, Is.EqualTo(name));
    }

    [Test]
    public void Constructor_NameLongerThanMax_Throws()
    {
        Assert.That(
            () => CreateCustomer(name: new string('a', Customer.NameMaxLength + 1)),
            Throws.TypeOf<ArgumentException>());
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    [TestCase("malthe.example.com")]
    public void Constructor_InvalidEmail_Throws(string? email)
    {
        Assert.That(() => CreateCustomer(email: email!), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Constructor_EmailAtMaxLength_IsAccepted()
    {
        var email = EmailOfLength(Customer.EmailMaxLength);

        var customer = CreateCustomer(email: email);

        Assert.That(customer.Email, Is.EqualTo(email));
    }

    [Test]
    public void Constructor_EmailLongerThanMax_Throws()
    {
        Assert.That(
            () => CreateCustomer(email: EmailOfLength(Customer.EmailMaxLength + 1)),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Constructor_NoPhone_IsAccepted()
    {
        var customer = CreateCustomer(phone: null);

        Assert.That(customer.Phone, Is.Null);
    }

    [TestCase("")]
    [TestCase("1234567")]
    [TestCase("123456789")]
    [TestCase("1234 5678")]
    [TestCase("+4512345678")]
    public void Constructor_InvalidPhone_Throws(string phone)
    {
        Assert.That(() => CreateCustomer(phone: phone), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Constructor_NullAddress_Throws()
    {
        Assert.That(
            () => new Customer(KeycloakId, Name, Email, null, null!),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void UpdateDetails_ValidDetails_ChangesDetailsButNotKeycloakIdOrEmail()
    {
        var customer = CreateCustomer();
        var newAddress = new Address("Nørrebrogade 1", "8000", "Aarhus C");

        customer.UpdateDetails("Malthe W.", Phone, newAddress);

        Assert.That(
            (customer.KeycloakId, customer.Email, customer.Name, customer.Phone, customer.Address),
            Is.EqualTo((KeycloakId, Email, "Malthe W.", Phone, newAddress)));
    }

    [TestCase("", Phone)]
    [TestCase(Name, "123")]
    public void UpdateDetails_InvalidDetails_ThrowsAndKeepsOldDetails(string name, string phone)
    {
        var customer = CreateCustomer();

        Assert.That(
            () => customer.UpdateDetails(name, phone, new Address("Nørrebrogade 1", "8000", "Aarhus C")),
            Throws.TypeOf<ArgumentException>());
        Assert.That(
            (customer.Name, customer.Phone, customer.Address),
            Is.EqualTo((Name, (string?)null, Home)));
    }

    private static string EmailOfLength(int length)
    {
        const string domain = "@example.com";
        return new string('a', length - domain.Length) + domain;
    }
}
