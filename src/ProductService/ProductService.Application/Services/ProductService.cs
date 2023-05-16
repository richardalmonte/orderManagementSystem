using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product> CreateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return _productRepository.CreateProductAsync(product);
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        ArgumentNullException.ThrowIfNull(productId);

        if (productId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(productId));
        }

        return await _productRepository.GetProductByIdAsync(productId);
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return _productRepository.GetAllProductsAsync();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(product.Id));
        }

        return await _productRepository.UpdateProductAsync(product);
    }

    public Task DeleteProductAsync(Guid productId)
    {
        ArgumentNullException.ThrowIfNull(productId);

        if (productId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(productId));
        }

        return _productRepository.DeleteProductAsync(productId);
    }
}