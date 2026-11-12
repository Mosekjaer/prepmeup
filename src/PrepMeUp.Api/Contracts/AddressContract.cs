using System.ComponentModel.DataAnnotations;
using PrepMeUp.Domain;

namespace PrepMeUp.Api.Contracts;

public record AddressContract(
    [Required, MaxLength(Address.StreetMaxLength)] string Street,
    [Required, RegularExpression("^[1-9][0-9]{3}$", ErrorMessage = "Postal code must be 4 digits between 1000 and 9999.")] string PostalCode,
    [Required, MaxLength(Address.CityMaxLength)] string City);
