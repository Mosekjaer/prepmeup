using System.ComponentModel.DataAnnotations;
using PrepMeUp.Domain;

namespace PrepMeUp.Api.Contracts;

public record SaveCustomerRequest(
    [Required, MaxLength(Customer.NameMaxLength)] string Name,
    [Required, MaxLength(Customer.AddressMaxLength)] string Address);
