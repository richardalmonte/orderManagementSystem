

using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces;

public interface IProductRepository
{
    Task<Product> CreateProductAsync(Product product);

    Task<Product> GetProductByIdAsync(Guid productId);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productId);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string category);
}