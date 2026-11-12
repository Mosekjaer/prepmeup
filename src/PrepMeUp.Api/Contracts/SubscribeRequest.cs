using System.ComponentModel.DataAnnotations;

namespace PrepMeUp.Api.Contracts;

public record SubscribeRequest([Required, MinLength(1)] IReadOnlyList<SubscribeLineRequest> Lines);

public record SubscribeLineRequest(Guid PackageTypeId, [Range(1, int.MaxValue)] int Quantity);
