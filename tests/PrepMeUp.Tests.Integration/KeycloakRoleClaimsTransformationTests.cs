using System.Security.Claims;
using PrepMeUp.Api.Authentication;

namespace PrepMeUp.Tests.Integration;

public class KeycloakRoleClaimsTransformationTests
{
    private KeycloakRoleClaimsTransformation _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new KeycloakRoleClaimsTransformation();
    }

    // Test that when the realm_access claim contains two roles, the transformation adds those roles as claims.
    [Test]
    public async Task TransformAsync_RealmAccessWithTwoRoles_AddsBothRoleClaims()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["household","admin"]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.EquivalentTo(new[] { "household", "admin" }));
    }

    [Test]
    public async Task TransformAsync_OnlyAdminRole_AddsOnlyAdminRoleClaim()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["admin"]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.EquivalentTo(new[] { "admin" }));
    }

    [Test]
    public async Task TransformAsync_OnlyHouseholdRole_AddsOnlyHouseholdRoleClaim()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["household"]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.EquivalentTo(new[] { "household" }));
    }

    [Test]
    public async Task TransformAsync_EmptyRolesArray_AddsNoRoleClaims()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":[]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_RealmAccessClaimMissing_AddsNoRoleClaims()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(authenticationType: "test"));

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_RolesPropertyMissing_AddsNoRoleClaims()
    {
        var principal = PrincipalWithRealmAccess("{}");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_RolesIsNotAnArray_AddsNoRoleClaims()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":"admin"}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_MalformedJson_AddsNoRoleClaims()
    {
        var principal = PrincipalWithRealmAccess("not json");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_NonStringRoleElements_IgnoresThem()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["admin",42,null]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.EquivalentTo(new[] { "admin" }));
    }

    [Test]
    public async Task TransformAsync_DuplicateRoles_AddsOneRoleClaim()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["admin","admin"]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.EquivalentTo(new[] { "admin" }));
    }

    [Test]
    public async Task TransformAsync_AppliedTwice_DoesNotDuplicateRoleClaims()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["household","admin"]}""");

        await _sut.TransformAsync(principal);
        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.EquivalentTo(new[] { "household", "admin" }));
    }

    [Test]
    public async Task TransformAsync_UnauthenticatedPrincipal_AddsNoRoleClaims()
    {
        var identity = new ClaimsIdentity([new Claim("realm_access", """{"roles":["admin"]}""", "JSON")]);
        var principal = new ClaimsPrincipal(identity);

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_OnlyResourceAccessRoles_AddsNoRoleClaims()
    {
        var principal = PrincipalWithClaim("resource_access", """{"prepmeup-admin":{"roles":["admin"]}}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(RoleValues(result), Is.Empty);
    }

    [Test]
    public async Task TransformAsync_RoleWithDifferentCase_DoesNotMatchLowerCaseRole()
    {
        var principal = PrincipalWithRealmAccess("""{"roles":["Admin"]}""");

        var result = await _sut.TransformAsync(principal);

        Assert.That(result.IsInRole("admin"), Is.False);
    }

    // Helper Function: Emulates a Bearer token with a realm_access claim containing the specified JSON string.
    private static ClaimsPrincipal PrincipalWithRealmAccess(string realmAccessJson) =>
        PrincipalWithClaim("realm_access", realmAccessJson);

    private static ClaimsPrincipal PrincipalWithClaim(string type, string json)
    {
        var identity = new ClaimsIdentity([new Claim(type, json, "JSON")], authenticationType: "test");
        return new ClaimsPrincipal(identity);
    }

    // Helper Function: Fetches the users roles, as a list of strings, so they can be compared.
    private static IEnumerable<string> RoleValues(ClaimsPrincipal principal) =>
        principal.FindAll(ClaimTypes.Role).Select(claim => claim.Value);
}
