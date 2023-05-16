using System.ComponentModel.DataAnnotations;

namespace OrderService.Contracts.V1.Requests;

public record OrderUpdateRequest
{
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
}