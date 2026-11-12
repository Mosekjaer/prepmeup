using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Api.Contracts;
using PrepMeUp.Application;

namespace PrepMeUp.Tests.Integration;

public class UserEndpointTests
{
    private const string Email = "malthe@example.com";
    private static readonly AddressContract Home = new("Finlandsgade 22", "8200", "Aarhus N");
    private static readonly AddressContract NewHome = new("Nørrebrogade 1", "8000", "Aarhus C");

    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var repository = new FakeCustomerRepository();
        _factory = new AuthenticatedApiFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services => services.AddSingleton<ICustomerRepository>(repository)));
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetUser_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/user");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetUser_NotOnboarded_ReturnsNotFound()
    {
        var response = await SendAsync(HttpMethod.Get, NewSub());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task PutUser_NoToken_ReturnsUnauthorized()
    {
        var response = await _client.PutAsJsonAsync("/user", Body());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task PutUser_TokenWithoutEmail_ReturnsUnauthorized()
    {
        var response = await SendAsync(HttpMethod.Put, NewSub(), Body(), email: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task PutUser_FirstOnboarding_ReturnsNoContentAndCustomerCanBeRead()
    {
        var sub = NewSub();

        var put = await SendAsync(HttpMethod.Put, sub, Body(phone: "12345678"));
        var customer = await GetCustomerAsync(sub);

        Assert.That(put.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(customer, Is.EqualTo(new CustomerResponse("Malthe", Email, "12345678", Home)));
    }

    [Test]
    public async Task PutUser_Twice_KeepsTheLatestDetailsAndTheFirstEmail()
    {
        var sub = NewSub();

        await SendAsync(HttpMethod.Put, sub, Body());
        var second = await SendAsync(
            HttpMethod.Put, sub, Body(name: "Malthe Winther", address: NewHome), email: "other@example.com");
        var customer = await GetCustomerAsync(sub);

        Assert.That(second.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(customer, Is.EqualTo(new CustomerResponse("Malthe Winther", Email, null, NewHome)));
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task PutUser_NoPhone_StoresPhoneAsNull(string? phone)
    {
        var sub = NewSub();

        var put = await SendAsync(HttpMethod.Put, sub, Body(phone: phone));
        var customer = await GetCustomerAsync(sub);

        Assert.That(put.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(customer?.Phone, Is.Null);
    }

    [Test]
    public async Task PutUser_BlankName_ReturnsBadRequest()
    {
        var response = await SendAsync(HttpMethod.Put, NewSub(), Body(name: ""));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase("", "8200", "Aarhus N")]
    [TestCase("Finlandsgade 22", "", "Aarhus N")]
    [TestCase("Finlandsgade 22", "8200", "")]
    public async Task PutUser_BlankAddressField_ReturnsBadRequest(string street, string postalCode, string city)
    {
        var body = Body(address: new AddressContract(street, postalCode, city));

        var response = await SendAsync(HttpMethod.Put, NewSub(), body);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PutUser_MissingAddress_ReturnsBadRequest()
    {
        var response = await SendAsync(HttpMethod.Put, NewSub(), new { Name = "Malthe" });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase(101, 1, 1)]
    [TestCase(1, 101, 1)]
    [TestCase(1, 1, 51)]
    public async Task PutUser_FieldOverMaxLength_ReturnsBadRequest(int nameLength, int streetLength, int cityLength)
    {
        var body = Body(
            name: new string('a', nameLength),
            address: new AddressContract(new string('a', streetLength), "8200", new string('a', cityLength)));

        var response = await SendAsync(HttpMethod.Put, NewSub(), body);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase("0999")]
    [TestCase("999")]
    [TestCase("10000")]
    [TestCase("82a0")]
    public async Task PutUser_InvalidPostalCode_ReturnsBadRequest(string postalCode)
    {
        var body = Body(address: new AddressContract("Finlandsgade 22", postalCode, "Aarhus N"));

        var response = await SendAsync(HttpMethod.Put, NewSub(), body);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase("1234567")]
    [TestCase("123456789")]
    [TestCase("+4512345678")]
    public async Task PutUser_InvalidPhone_ReturnsBadRequest(string phone)
    {
        var response = await SendAsync(HttpMethod.Put, NewSub(), Body(phone: phone));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetUser_AnotherUserIsOnboarded_ReturnsNotFound()
    {
        await SendAsync(HttpMethod.Put, NewSub(), Body());

        var response = await SendAsync(HttpMethod.Get, NewSub());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private static string NewSub() => Guid.NewGuid().ToString();

    private static SaveCustomerRequest Body(string name = "Malthe", string? phone = null, AddressContract? address = null)
        => new(name, phone, address ?? Home);

    private async Task<CustomerResponse?> GetCustomerAsync(string sub)
    {
        var response = await SendAsync(HttpMethod.Get, sub);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<CustomerResponse>() : null;
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string sub, object? body = null, string? email = Email)
    {
        var request = new HttpRequestMessage(method, "/user");
        var claims = new Dictionary<string, object> { ["sub"] = sub };
        if (email is not null)
        {
            claims["email"] = email;
        }

        var token = TestJwt.Create(TestJwt.TrustedKey, ["household"], additionalClaims: claims);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body);
        }

        return _client.SendAsync(request);
    }
}
