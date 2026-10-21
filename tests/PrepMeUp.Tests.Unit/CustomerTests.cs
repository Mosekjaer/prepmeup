using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class CustomerTests
{
    [Test]
    public void Constructor_ValidDetails_SetsProperties()
    {
        var customer = new Customer("sub-1", "Malthe Winther", "Finlandsgade 22, 8200 Aarhus N");

        Assert.That(
            (customer.KeycloakId, customer.Name, customer.Address),
            Is.EqualTo(("sub-1", "Malthe Winther", "Finlandsgade 22, 8200 Aarhus N")));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankName_Throws(string? name)
    {
        Assert.That(() => new Customer("sub-1", name!, "Finlandsgade 22"), Throws.TypeOf<ArgumentException>());
    }

    [TestCase(1)]
    [TestCase(100)]
    public void Constructor_NameLengthWithinLimit_IsAccepted(int length)
    {
        var name = new string('a', length);

        var customer = new Customer("sub-1", name, "Finlandsgade 22");

        Assert.That(customer.Name, Is.EqualTo(name));
    }

    [Test]
    public void Constructor_NameLongerThan100_Throws()
    {
        Assert.That(
            () => new Customer("sub-1", new string('a', 101), "Finlandsgade 22"),
            Throws.TypeOf<ArgumentException>());
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankAddress_Throws(string? address)
    {
        Assert.That(() => new Customer("sub-1", "Malthe", address!), Throws.TypeOf<ArgumentException>());
    }

    [TestCase(1)]
    [TestCase(200)]
    public void Constructor_AddressLengthWithinLimit_IsAccepted(int length)
    {
        var address = new string('a', length);

        var customer = new Customer("sub-1", "Malthe", address);

        Assert.That(customer.Address, Is.EqualTo(address));
    }

    [Test]
    public void Constructor_AddressLongerThan200_Throws()
    {
        Assert.That(
            () => new Customer("sub-1", "Malthe", new string('a', 201)),
            Throws.TypeOf<ArgumentException>());
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void Constructor_BlankKeycloakId_Throws(string? keycloakId)
    {
        Assert.That(() => new Customer(keycloakId!, "Malthe", "Finlandsgade 22"), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void UpdateDetails_ValidDetails_ChangesNameAndAddressButNotKeycloakId()
    {
        var customer = new Customer("sub-1", "Malthe", "Finlandsgade 22");

        customer.UpdateDetails("Malthe Winther", "Nørrebrogade 1, 8000 Aarhus C");

        Assert.That(
            (customer.KeycloakId, customer.Name, customer.Address),
            Is.EqualTo(("sub-1", "Malthe Winther", "Nørrebrogade 1, 8000 Aarhus C")));
    }

    [TestCase("", "Nørrebrogade 1")]
    [TestCase("Malthe Winther", "")]
    public void UpdateDetails_InvalidDetails_ThrowsAndKeepsOldDetails(string name, string address)
    {
        var customer = new Customer("sub-1", "Malthe", "Finlandsgade 22");

        Assert.That(() => customer.UpdateDetails(name, address), Throws.TypeOf<ArgumentException>());
        Assert.That((customer.Name, customer.Address), Is.EqualTo(("Malthe", "Finlandsgade 22")));
    }
}
