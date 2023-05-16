namespace OrderService.Api.Contracts.V1.Requests;

public record OrderUpdateRequest
{
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
    public List<OrderItemUpdateRequest> OrderItems { get; set; }
}