namespace AddressBookService.Api.Contracts.V1.Requests;

public record AddressUpdateRequest
{
    public Guid UserId { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string ZipCode { get; set; }
}