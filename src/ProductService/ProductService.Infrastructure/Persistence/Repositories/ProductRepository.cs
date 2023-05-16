using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductServiceDbContext _context;

    public ProductRepository(ProductServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var productEntry = await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        await _context.Entry(productEntry.Entity).Reference(p => p.Category).LoadAsync();

        return productEntry.Entity;
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(productId));
        }

        var response = await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == productId);

        return response ?? throw new Exception("Product not found");
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var response = await _context.Products
            .Include(x => x.Category)
            .ToListAsync();

        return response;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(product.Id));
        }

        var updatedProduct = _context.Products.Update(product);
        await _context.SaveChangesAsync();

        if (updatedProduct?.Entity is null)
        {
            throw new Exception("Product not found");
        }

        return updatedProduct.Entity;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        ArgumentNullException.ThrowIfNull(productId);

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
        {
            throw new Exception("Product not found");
        }

        _context.Products.Remove(product);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}