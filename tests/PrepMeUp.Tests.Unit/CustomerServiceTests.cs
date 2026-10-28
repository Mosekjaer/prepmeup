using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class CustomerServiceTests
{
    private ICustomerRepository _repository = null!;
    private CustomerService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<ICustomerRepository>();
        _sut = new CustomerService(_repository);
    }

    [Test]
    public async Task GetAsync_KnownCustomer_ReturnsCustomer()
    {
        var existing = new Customer("sub-1", "Malthe", "Finlandsgade 22");
        _repository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(existing);

        var result = await _sut.GetAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.SameAs(existing));
    }

    [Test]
    public async Task GetAsync_UnknownCustomer_ReturnsNull()
    {
        _repository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _sut.GetAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task SaveDetailsAsync_UnknownCustomer_AddsAndSavesNewCustomer()
    {
        _repository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns((Customer?)null);

        await _sut.SaveDetailsAsync("sub-1", "Malthe", "Finlandsgade 22", CancellationToken.None);

        await _repository.Received(1).AddAsync(
            Arg.Is<Customer>(c => c.KeycloakId == "sub-1" && c.Name == "Malthe" && c.Address == "Finlandsgade 22"),
            Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SaveDetailsAsync_ExistingCustomer_UpdatesWithoutAdding()
    {
        var existing = new Customer("sub-1", "Malthe", "Finlandsgade 22");
        _repository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(existing);

        await _sut.SaveDetailsAsync("sub-1", "Malthe Winther", "Nørrebrogade 1", CancellationToken.None);

        Assert.That((existing.Name, existing.Address), Is.EqualTo(("Malthe Winther", "Nørrebrogade 1")));
        await _repository.DidNotReceive().AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SaveDetailsAsync_UnknownCustomerWithInvalidDetails_ThrowsAndSavesNothing()
    {
        _repository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns((Customer?)null);

        Assert.That(
            () => _sut.SaveDetailsAsync("sub-1", "", "Finlandsgade 22", CancellationToken.None),
            Throws.TypeOf<ArgumentException>());
        await _repository.DidNotReceive().AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SaveDetailsAsync_ExistingCustomerWithInvalidDetails_ThrowsAndSavesNothing()
    {
        var existing = new Customer("sub-1", "Malthe", "Finlandsgade 22");
        _repository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(existing);

        Assert.That(
            () => _sut.SaveDetailsAsync("sub-1", "", "Nørrebrogade 1", CancellationToken.None),
            Throws.TypeOf<ArgumentException>());
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
