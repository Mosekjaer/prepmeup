namespace PrepMeUp.Tests.Unit;

public class ItemKeyValidatorTests
{
    [Test]
    public void IsValid_ReturnsTrue_ForNonEmptyKey()
    {
        Assert.That(Application.ItemKeyValidator.IsValid("torch"), Is.True);
    }

    [Test]
    public void IsValid_ReturnsFalse_ForEmptyKey()
    {
        Assert.That(Application.ItemKeyValidator.IsValid(""), Is.False);
    }
}
