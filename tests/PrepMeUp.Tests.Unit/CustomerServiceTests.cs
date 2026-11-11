using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class CustomerServiceTests
{
    private const string KeycloakId = "sub-1";
    private const string Email = "malthe@example.com";
    private static readonly Address Home = new("Finlandsgade 22", "8200", "Aarhus N");
    private static readonly Address NewHome = new("Nørrebrogade 1", "8000", "Aarhus C");

    private ICustomerRepository _repository = null!;
    private CustomerService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<ICustomerRepository>();
        _sut = new CustomerService(_repository);
    }

    private static Customer ExistingCustomer() => new(KeycloakId, "Malthe", Email, null, Home);

    [Test]
    public async Task GetAsync_KnownCustomer_ReturnsCustomer()
    {
        var existing = ExistingCustomer();
        _repository.GetByKeycloakIdAsync(KeycloakId, Arg.Any<CancellationToken>()).Returns(existing);

        var result = await _sut.GetAsync(KeycloakId, CancellationToken.None);

        Assert.That(result, Is.SameAs(existing));
    }

    [Test]
    public async Task GetAsync_UnknownCustomer_ReturnsNull()
    {
        _repository.GetByKeycloakIdAsync(KeycloakId, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _sut.GetAsync(KeycloakId, CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task SaveDetailsAsync_UnknownCustomer_AddsAndSavesNewCustomer()
    {
        _repository.GetByKeycloakIdAsync(KeycloakId, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        await _sut.SaveDetailsAsync(KeycloakId, Email, "Malthe", "12345678", Home, CancellationToken.None);

        await _repository.Received(1).AddAsync(
            Arg.Is<Customer>(c =>
                c.KeycloakId == KeycloakId
                && c.Email == Email
                && c.Name == "Malthe"
                && c.Phone == "12345678"
                && c.Address == Home),
            Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SaveDetailsAsync_ExistingCustomer_UpdatesWithoutAddingAndKeepsEmail()
    {
        var existing = ExistingCustomer();
        _repository.GetByKeycloakIdAsync(KeycloakId, Arg.Any<CancellationToken>()).Returns(existing);

        await _sut.SaveDetailsAsync(
            KeycloakId, "other@example.com", "Malthe Winther", "12345678", NewHome, CancellationToken.None);

        Assert.That(
            (existing.Email, existing.Name, existing.Phone, existing.Address),
            Is.EqualTo((Email, "Malthe Winther", "12345678", NewHome)));
        await _repository.DidNotReceive().AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SaveDetailsAsync_UnknownCustomerWithInvalidDetails_ThrowsAndSavesNothing()
    {
        _repository.GetByKeycloakIdAsync(KeycloakId, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        Assert.That(
            () => _sut.SaveDetailsAsync(KeycloakId, Email, "", null, Home, CancellationToken.None),
            Throws.TypeOf<ArgumentException>());
        await _repository.DidNotReceive().AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SaveDetailsAsync_ExistingCustomerWithInvalidDetails_ThrowsAndSavesNothing()
    {
        _repository.GetByKeycloakIdAsync(KeycloakId, Arg.Any<CancellationToken>()).Returns(ExistingCustomer());

        Assert.That(
            () => _sut.SaveDetailsAsync(KeycloakId, Email, "", null, NewHome, CancellationToken.None),
            Throws.TypeOf<ArgumentException>());
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
