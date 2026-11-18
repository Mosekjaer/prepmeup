using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Unit;

public class OrderExpiryTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly DateTimeOffset StartedAt = new(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);

    private Order _order = null!;

    [SetUp]
    public void SetUp()
    {
        _order = Subscription.Start(CustomerId, StartedAt, [NewLine("Normal - Adult", 1)]).Orders.Single();
    }

    [Test]
    [Description("Planned order, not sent")]
    public void ExpiresAt_PlannedOrder_IsNull()
    {
        Assert.That(_order.ExpiresAt, Is.Null);
    }

    [Test]
    [Description("Sent order, ordinary date")]
    public void MarkAsSent_OrdinaryDate_SetsExpiresAtOneYearLater()
    {
        _order.MarkAsSent(new DateTimeOffset(2026, 11, 18, 14, 30, 0, TimeSpan.Zero));

        Assert.That(_order.ExpiresAt, Is.EqualTo(new DateTimeOffset(2027, 11, 18, 14, 30, 0, TimeSpan.Zero)));
    }

    [Description("Boundary: sent on leap day and the day before a leap day")]
    [TestCase(2028, 2, 29, 2029, 2, 28)]
    [TestCase(2028, 2, 28, 2029, 2, 28)]
    public void MarkAsSent_AroundLeapDay_SetsExpiresAtToSameOrLastDayOfMonthNextYear(
        int sentYear, int sentMonth, int sentDay, int expiresYear, int expiresMonth, int expiresDay)
    {
        _order.MarkAsSent(new DateTimeOffset(sentYear, sentMonth, sentDay, 12, 0, 0, TimeSpan.Zero));

        Assert.That(
            _order.ExpiresAt,
            Is.EqualTo(new DateTimeOffset(expiresYear, expiresMonth, expiresDay, 12, 0, 0, TimeSpan.Zero)));
    }

    [Test]
    [Description("Invalid: already sent")]
    public void MarkAsSent_AlreadySent_KeepsFirstExpiresAt()
    {
        _order.MarkAsSent(new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));

        try
        {
            _order.MarkAsSent(new DateTimeOffset(2026, 12, 1, 12, 0, 0, TimeSpan.Zero));
        }
        catch (InvalidOperationException)
        {
        }

        Assert.That(_order.ExpiresAt, Is.EqualTo(new DateTimeOffset(2027, 11, 18, 12, 0, 0, TimeSpan.Zero)));
    }

    [Test]
    [Description("Planned order, not sent")]
    public void HasReachedReorderDate_PlannedOrder_ReturnsFalse()
    {
        var now = new DateTimeOffset(2027, 11, 10, 12, 0, 0, TimeSpan.Zero);

        Assert.That(_order.HasReachedReorderDate(now), Is.False);
    }

    [Description("Boundary: just before, right on and just after 14 days before expiry")]
    [TestCase(-1, false)] // (num-seconds-after-boundary-date, to-be-planned-now)
    [TestCase(0, true)]
    [TestCase(1, true)]
    public void HasReachedReorderDate_AroundFourteenDaysBeforeExpiry_ReturnsExpected(int secondsFromBoundary, bool expected)
    {
        _order.MarkAsSent(new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));
        var now = new DateTimeOffset(2027, 11, 4, 12, 0, 0, TimeSpan.Zero).AddSeconds(secondsFromBoundary);

        Assert.That(_order.HasReachedReorderDate(now), Is.EqualTo(expected));
    }

    [Test]
    [Description("Already expired")]
    public void HasReachedReorderDate_AlreadyExpired_ReturnsTrue()
    {
        _order.MarkAsSent(new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));
        var now = new DateTimeOffset(2027, 11, 19, 12, 0, 0, TimeSpan.Zero);

        Assert.That(_order.HasReachedReorderDate(now), Is.True);
    }

    private static OrderLine NewLine(string packageTypeName, int quantity) =>
        new(
            PackageType.Create(
                packageTypeName,
                [new PackageContent(new Item { Id = Guid.NewGuid(), Name = "Water", Type = ItemType.Food }, 5)]),
            quantity);
}
