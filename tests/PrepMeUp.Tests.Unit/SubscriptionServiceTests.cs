using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    private ICustomerRepository _customerRepository = null!;
    private ISubscriptionRepository _subscriptionRepository = null!;
    private IPackageTypeRepository _packageTypeRepository = null!;
    private SubscriptionService _sut = null!;
    private Customer _customer = null!;
    private PackageType _adult = null!;
    private PackageType _child = null!;

    private static PackageType NewPackageType(string name) =>
        PackageType.Create(
            name,
            [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]);

    [SetUp]
    public void SetUp()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        _packageTypeRepository = Substitute.For<IPackageTypeRepository>();
        _adult = NewPackageType("Normal - Adult");
        _child = NewPackageType("Normal - Child");
        _packageTypeRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns([_adult, _child]);
        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(Now);
        _sut = new SubscriptionService(_customerRepository, _subscriptionRepository, _packageTypeRepository, clock);
        _customer = new Customer(
            "sub-1", "Malthe", "malthe@example.com", null, new Address("Finlandsgade 22", "8200", "Aarhus N"))
        {
            Id = Guid.NewGuid()
        };
    }

    [Test]
    public async Task SubscribeAsync_ValidLines_AddsSubscriptionWhoseFirstOrderHasTheLines()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns((Subscription?)null);
        Subscription? saved = null;
        await _subscriptionRepository.AddAsync(Arg.Do<Subscription>(subscription => saved = subscription), Arg.Any<CancellationToken>());
        SubscribeLine[] lines = [new(_adult.Id, 2), new(_child.Id, 1)];

        var result = await _sut.SubscribeAsync("sub-1", lines, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(SubscribeResult.Subscribed));
            Assert.That(
                saved?.Orders.Single().Lines.Select(line => (line.PackageType.Id, line.Quantity)),
                Is.EqualTo(new[] { (_adult.Id, 2), (_child.Id, 1) }));
        });
        await _subscriptionRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_UnknownPackageType_ReturnsPackageTypeNotAvailableAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns((Subscription?)null);
        SubscribeLine[] lines = [new(_adult.Id, 1), new(Guid.NewGuid(), 1)];

        var result = await _sut.SubscribeAsync("sub-1", lines, CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.PackageTypeNotAvailable));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_ReplacedPackageType_ReturnsPackageTypeNotAvailableAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns((Subscription?)null);
        _child.MarkReplaced();
        SubscribeLine[] lines = [new(_adult.Id, 1), new(_child.Id, 1)];

        var result = await _sut.SubscribeAsync("sub-1", lines, CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.PackageTypeNotAvailable));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_SamePackageTypeTwice_ReturnsDuplicatePackageTypeAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns((Subscription?)null);
        SubscribeLine[] lines = [new(_adult.Id, 1), new(_adult.Id, 2)];

        var result = await _sut.SubscribeAsync("sub-1", lines, CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.DuplicatePackageType));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_AlreadySubscribedWithUnknownPackageType_ReturnsAlreadySubscribedWithoutLookingUpPackageTypes()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>())
            .Returns(Subscription.Start(_customer.Id, Now, [new OrderLine(_adult, 1)]));
        SubscribeLine[] lines = [new(Guid.NewGuid(), 1)];

        var result = await _sut.SubscribeAsync("sub-1", lines, CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.AlreadySubscribed));
        await _packageTypeRepository.DidNotReceive().GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_UnknownCustomer_ReturnsCustomerNotFoundAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _sut.SubscribeAsync("sub-1", [new SubscribeLine(_adult.Id, 1)], CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.CustomerNotFound));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_AlreadySubscribed_ReturnsAlreadySubscribedAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>())
            .Returns(Subscription.Start(_customer.Id, Now, [new OrderLine(_adult, 1)]));

        var result = await _sut.SubscribeAsync("sub-1", [new SubscribeLine(_adult.Id, 1)], CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.AlreadySubscribed));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetAsync_CustomerWithSubscription_ReturnsSubscription()
    {
        var existing = Subscription.Start(_customer.Id, Now, [new OrderLine(_adult, 1)]);
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns(existing);

        var result = await _sut.GetAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.SameAs(existing));
    }

    [Test]
    public async Task GetAsync_CustomerWithoutSubscription_ReturnsNull()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns((Subscription?)null);

        var result = await _sut.GetAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAsync_UnknownCustomer_ReturnsNullWithoutLookingUpSubscription()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _sut.GetAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.Null);
        await _subscriptionRepository.DidNotReceive().GetByCustomerIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
}
