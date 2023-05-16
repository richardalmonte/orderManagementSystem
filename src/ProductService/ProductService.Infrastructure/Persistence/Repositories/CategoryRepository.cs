using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductServiceDbContext _context;

    public CategoryRepository(ProductServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        var categoryEntry = await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return categoryEntry.Entity;
    }

    public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(categoryId));
        }

        var response = await _context.Categories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        return response ?? throw new Exception("Category not found");
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Include(x => x.Products)
            .ToListAsync();
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (category.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(category.Id));
        }

        var updatedCategory = _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        if (updatedCategory?.Entity is null)
        {
            throw new Exception("Category not found");
        }

        return updatedCategory.Entity;
    }

    public async Task<bool> DeleteCategoryAsync(Guid categoryId)
    {
        ArgumentNullException.ThrowIfNull(categoryId);

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        if (category is null)
        {
            throw new Exception("Category not found");
        }

        _context.Categories.Remove(category);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}