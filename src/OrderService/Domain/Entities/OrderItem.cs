namespace OrderService.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;

    public virtual Order Order { get; set; } = default!;
}