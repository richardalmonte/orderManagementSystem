namespace OrderService.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
    
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}