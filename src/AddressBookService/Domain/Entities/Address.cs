namespace AddressBookService.Domain.Entities;

public class Address : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }

    public virtual ICollection<AddressItem> AddressItems { get; set; }
}