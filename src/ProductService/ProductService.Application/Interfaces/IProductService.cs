using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces;

public interface IProductService
{
    Task<Product> CreateProductAsync(Product product);
    Task<Product> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(Guid productId);
    Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string category);
}