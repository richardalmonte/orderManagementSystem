namespace ProductService.API.Contracts.V1.Requests;

public record CategoryUpdateRequest
{
    public string Name { get; set; }
    
}