using NSubstitute;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class SubscriptionServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    private ICustomerRepository _customerRepository = null!;
    private ISubscriptionRepository _subscriptionRepository = null!;
    private SubscriptionService _sut = null!;
    private Customer _customer = null!;

    [SetUp]
    public void SetUp()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(Now);
        _sut = new SubscriptionService(_customerRepository, _subscriptionRepository, clock);
        _customer = new Customer("sub-1", "Malthe", "Finlandsgade 22") { Id = Guid.NewGuid() };
    }

    [Test]
    public async Task SubscribeAsync_KnownCustomerWithoutSubscription_AddsAndSavesSubscription()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>()).Returns((Subscription?)null);

        var result = await _sut.SubscribeAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.Subscribed));
        await _subscriptionRepository.Received(1).AddAsync(
            Arg.Is<Subscription>(s => s.CustomerId == _customer.Id && s.StartedAt == Now),
            Arg.Any<CancellationToken>());
        await _subscriptionRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_UnknownCustomer_ReturnsCustomerNotFoundAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _sut.SubscribeAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.CustomerNotFound));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SubscribeAsync_AlreadySubscribed_ReturnsAlreadySubscribedAndSavesNothing()
    {
        _customerRepository.GetByKeycloakIdAsync("sub-1", Arg.Any<CancellationToken>()).Returns(_customer);
        _subscriptionRepository.GetByCustomerIdAsync(_customer.Id, Arg.Any<CancellationToken>())
            .Returns(Subscription.Start(_customer.Id, Now));

        var result = await _sut.SubscribeAsync("sub-1", CancellationToken.None);

        Assert.That(result, Is.EqualTo(SubscribeResult.AlreadySubscribed));
        await _subscriptionRepository.DidNotReceive().AddAsync(Arg.Any<Subscription>(), Arg.Any<CancellationToken>());
        await _subscriptionRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task GetAsync_CustomerWithSubscription_ReturnsSubscription()
    {
        var existing = Subscription.Start(_customer.Id, Now);
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
