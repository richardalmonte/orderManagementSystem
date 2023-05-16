using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces;

public interface ICategoryService
{
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> GetCategoryByIdAsync(Guid categoryId);
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(Guid categoryId);
}