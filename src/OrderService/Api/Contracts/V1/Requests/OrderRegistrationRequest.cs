namespace OrderService.Api.Contracts.V1.Requests;

public record OrderRegistrationRequest
{
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
    
    public List<OrderItemRegistrationRequest> OrderItems { get; set; } = new();
}