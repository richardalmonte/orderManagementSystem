using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<Category> CreateCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return _categoryRepository.CreateCategoryAsync(category);
    }

    public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
    {
        ArgumentNullException.ThrowIfNull(categoryId);

        if (categoryId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(categoryId));
        }

        return await _categoryRepository.GetCategoryByIdAsync(categoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllCategoriesAsync();
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (category.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(category.Id));
        }

        return await _categoryRepository.UpdateCategoryAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(Guid categoryId)
    {
        ArgumentNullException.ThrowIfNull(categoryId);

        if (categoryId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(categoryId));
        }

        return await _categoryRepository.DeleteCategoryAsync(categoryId);
    }
}