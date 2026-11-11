namespace PrepMeUp.Api.Contracts;

public record CustomerResponse(string Name, string Email, string? Phone, AddressContract Address);
