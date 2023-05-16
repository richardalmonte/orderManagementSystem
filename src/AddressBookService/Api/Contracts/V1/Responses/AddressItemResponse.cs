namespace AddressBookService.Api.Contracts.V1.Responses;

public record AddressItemResponse
{
    public Guid Id { get; set; }
    public Guid AddressId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}