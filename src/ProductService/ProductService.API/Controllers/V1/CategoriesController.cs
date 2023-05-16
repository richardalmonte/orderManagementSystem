using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Contracts;
using ProductService.API.Contracts.V1.Requests;
using ProductService.API.Contracts.V1.Responses;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.API.Controllers.V1;

[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _categoryService;

    public CategoriesController(IMapper mapper, ICategoryService categoryService)
    {
        _mapper = mapper;
        _categoryService = categoryService;
    }

    [HttpGet(ApiRoutes.Categories.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryResponses = _mapper.Map<IEnumerable<CategoryResponse>>(categories);
            return Ok(categoryResponses);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet(ApiRoutes.Categories.Get)]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategory(Guid categoryId)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (category is null)
            {
                return NotFound();
            }

            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return Ok(categoryResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost(ApiRoutes.Categories.Create)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRegistrationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryRequest = _mapper.Map<Category>(request);

            var createdCategory = await _categoryService.CreateCategoryAsync(categoryRequest);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" +
                              ApiRoutes.Categories.Get.Replace("{CategoryId}", createdCategory.Id.ToString());

            var response = _mapper.Map<CategoryResponse>(createdCategory);
            return Created(locationUri, response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPut(ApiRoutes.Categories.Update)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryUpdateRequest request)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (category is null)
            {
                return NotFound();
            }

            var updatedCategory = _mapper.Map(request, category);
            await _categoryService.UpdateCategoryAsync(updatedCategory);

            var categoryResponse = _mapper.Map<CategoryResponse>(updatedCategory);
            return Ok(categoryResponse);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete(ApiRoutes.Categories.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId);
        if (category is null)
        {
            return NotFound();
        }

        await _categoryService.DeleteCategoryAsync(categoryId);
        return NoContent();
    }
}