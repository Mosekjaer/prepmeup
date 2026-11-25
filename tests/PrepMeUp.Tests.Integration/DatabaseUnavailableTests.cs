using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using PrepMeUp.Api.ErrorHandling;
using PrepMeUp.Application;
using PrepMeUp.Domain;

namespace PrepMeUp.Tests.Integration;

public class DatabaseUnavailableTests
{
    [Test]
    public async Task GetCustomer_DatabaseUnreachable_ReturnsServiceUnavailableWithEnglishMessage()
    {
        var missingFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "missing.db");
        using var connection = new SqliteConnection($"Data Source={missingFile};Mode=ReadWrite");
        using var factory = new SqliteApiFactory(connection);
        using var client = factory.CreateClient();

        var response = await client.SendAsync(GetUserRequest());
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.That(
            (response.StatusCode, problem?.Detail),
            Is.EqualTo((HttpStatusCode.ServiceUnavailable, DatabaseUnavailableExceptionHandler.Message)));
    }

    [Test]
    public async Task GetCustomer_OtherError_StillReturnsInternalServerError()
    {
        using var factory = new AuthenticatedApiFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddSingleton<ICustomerRepository, ThrowingCustomerRepository>()));
        using var client = factory.CreateClient();

        var response = await client.SendAsync(GetUserRequest());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    private static HttpRequestMessage GetUserRequest()
    {
        var token = TestJwt.Create(TestJwt.TrustedKey, roles: null,
            additionalClaims: new Dictionary<string, object> { ["sub"] = Guid.NewGuid().ToString() });
        var request = new HttpRequestMessage(HttpMethod.Get, "/customer");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private sealed class ThrowingCustomerRepository : ICustomerRepository
    {
        public Task<Customer?> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Not a database error");

        public Task AddAsync(Customer customer, CancellationToken cancellationToken) => Task.CompletedTask;

        public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
