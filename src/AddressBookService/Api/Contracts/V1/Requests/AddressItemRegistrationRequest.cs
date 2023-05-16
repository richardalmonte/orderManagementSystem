namespace AddressBookService.Api.Contracts.V1.Requests;

public record AddressItemRegistrationRequest
{
    public Guid AddressId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}