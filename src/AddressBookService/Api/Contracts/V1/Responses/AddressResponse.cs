namespace AddressBookService.Api.Contracts.V1.Responses;

public record AddressResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
}