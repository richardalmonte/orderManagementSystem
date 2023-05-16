namespace AddressBookService.Api.Contracts.V1.Responses;

public record AddressResponse
{
    public Guid AddressId { get; set; }
    public Guid UserId { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string ZipCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}