namespace ProductService.API.Contracts.V1.Responses;

public record CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}