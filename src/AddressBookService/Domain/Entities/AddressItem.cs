namespace AddressBookService.Domain.Entities;

public class AddressItem : BaseEntity
{
    public Guid AddressId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;

    public virtual Address Addresses { get; set; } = default!;
}