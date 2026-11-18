namespace PrepMeUp.Api.Configuration;

public static class RequiredConfiguration
{
    public static readonly string[] Keys =
    [
        "Keycloak:Authority",
        "ConnectionStrings:PrepMeUpDb",
    ];

    public static void EnsurePresent(IConfiguration configuration, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return;
        }

        foreach (var key in Keys)
        {
            if (string.IsNullOrWhiteSpace(configuration[key]))
            {
                throw new InvalidOperationException(
                    $"The configuration value '{key}' is missing. " +
                    $"Set it, for example with the environment variable '{key.Replace(":", "__")}', " +
                    $"before starting the API in the '{environment.EnvironmentName}' environment.");
            }
        }
    }
}
