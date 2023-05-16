namespace AddressBookService.Api.Contracts.V1.Requests;

public record AddressUpdateRequest
{
    public Guid UserId { get; set; }
    public Guid DeliveryAddressId { get; set; }
}