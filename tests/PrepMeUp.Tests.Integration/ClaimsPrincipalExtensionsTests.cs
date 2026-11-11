using System.Security.Claims;
using PrepMeUp.Api.Authentication;

namespace PrepMeUp.Tests.Integration;

public class ClaimsPrincipalExtensionsTests
{
    [Test]
    public void GetKeycloakId_SubjectPresent_ReturnsSubject()
    {
        var principal = PrincipalWith(new Claim(ClaimTypes.NameIdentifier, "6f1c2a9e-3b4d-4e5f-8a7b-1c2d3e4f5a6b"));

        Assert.That(principal.GetKeycloakId(), Is.EqualTo("6f1c2a9e-3b4d-4e5f-8a7b-1c2d3e4f5a6b"));
    }

    [Test]
    public void GetKeycloakId_NoSubjectClaim_ReturnsNull()
    {
        var principal = PrincipalWith(new Claim(ClaimTypes.Role, "household"));

        Assert.That(principal.GetKeycloakId(), Is.Null);
    }

    [TestCase("")]
    [TestCase("   ")]
    public void GetKeycloakId_BlankSubject_ReturnsNull(string subject)
    {
        var principal = PrincipalWith(new Claim(ClaimTypes.NameIdentifier, subject));

        Assert.That(principal.GetKeycloakId(), Is.Null);
    }

    private static ClaimsPrincipal PrincipalWith(Claim claim) =>
        new(new ClaimsIdentity([claim], authenticationType: "Bearer"));
}
