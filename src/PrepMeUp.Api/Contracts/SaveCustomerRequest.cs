using System.ComponentModel.DataAnnotations;
using PrepMeUp.Domain;

namespace PrepMeUp.Api.Contracts;

public record SaveCustomerRequest(
    [Required, MaxLength(Customer.NameMaxLength)] string Name,
    [RegularExpression("^[0-9]{8}$", ErrorMessage = "Phone must be 8 digits.")] string? Phone,
    [Required] AddressContract Address);
