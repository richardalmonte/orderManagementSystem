using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.API.Contracts.V1.Requests;
using ProductService.API.Contracts.V1.Responses;
using ProductService.API.Controllers.V1;
using ProductService.API.Validators;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Tests.Systems.Controllers.V1;

public class CategoriesControllerTest
{
    private readonly Fixture _fixture;
    private readonly CategoriesController _sut;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ICategoryService> _categoryService;

    private const string ContextScheme = "http";
    private const string ContextHost = "localhost";
    private const int ContextPort = 5000;

    public CategoriesControllerTest()
    {
        _fixture = new Fixture();
        _mapper = new Mock<IMapper>();
        _categoryService = new Mock<ICategoryService>();
        _sut = new CategoriesController(_mapper.Object, _categoryService.Object);

        _sut.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                Request =
                {
                    Scheme = ContextScheme,
                    Host = new HostString(ContextHost, ContextPort)
                }
            }
        };

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async void CreateCategory_WhenCalledWithValidCategory_ShouldReturn201StatusCode()
    {
        // Arrange
        var categoryRequest = _fixture.Create<CategoryRegistrationRequest>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<Category>(categoryRequest)).Returns(actualCategory);
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.CreateCategoryAsync(actualCategory)).ReturnsAsync(actualCategory);

        // Act
        var result = await _sut.CreateCategory(categoryRequest);

        // Assert

        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        objectResult?.Location.Should()
            .Be($"{ContextScheme}://{ContextHost}:{ContextPort}/api/v1/Categories/{actualCategory.Id}");
    }

    [Fact]
    public async void CreateCategory_WhenCalledWithValidCategory_ShouldCallMapper()
    {
        // Arrange
        var categoryRequest = _fixture.Create<CategoryRegistrationRequest>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<Category>(categoryRequest)).Returns(actualCategory);
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.CreateCategoryAsync(actualCategory)).ReturnsAsync(actualCategory);

        // Act
        await _sut.CreateCategory(categoryRequest);

        // Assert
        _mapper.Verify(x => x.Map<Category>(categoryRequest), Times.Once);
        _mapper.Verify(x => x.Map<CategoryResponse>(actualCategory), Times.Once);
    }

    [Fact]
    public async void CreateCategory_WhenCalledWithValidCategory_ShouldCallService()
    {
        // Arrange
        var categoryRequest = _fixture.Create<CategoryRegistrationRequest>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<Category>(categoryRequest)).Returns(actualCategory);
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.CreateCategoryAsync(actualCategory)).ReturnsAsync(actualCategory);

        // Act
        await _sut.CreateCategory(categoryRequest);

        // Assert
        _categoryService.Verify(x => x.CreateCategoryAsync(actualCategory), Times.Once);
    }


    [Fact]
    public async void CreateCategory_WhenCalledWithInvalidCategory_ShouldReturn400StatusCode()
    {
        // Arrange

        var categoryRequest = _fixture.Build<CategoryRegistrationRequest>()
            .Without(u => u.Name)
            .Create();

        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<Category>(categoryRequest)).Returns(actualCategory);
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.CreateCategoryAsync(actualCategory)).ReturnsAsync(actualCategory);

        var validator = new CategoryRegistrationRequestValidator();
        var result = await validator.ValidateAsync(categoryRequest);
        result.AddToModelState(_sut.ModelState, null);

        // Act
        var response = await _sut.CreateCategory(categoryRequest);

        // Assert

        response.Should().BeOfType<BadRequestObjectResult>();
        var objectResult = response as BadRequestObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [Fact]
    public async void CreateCategory_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var categoryRequest = _fixture.Create<CategoryRegistrationRequest>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<Category>(categoryRequest)).Returns(actualCategory);
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());

        _categoryService.Setup(x => x.CreateCategoryAsync(actualCategory)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.CreateCategory(categoryRequest);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetCategory_WhenCalledWithValidId_ShouldReturn200StatusCode()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(actualCategory);

        // Act
        var result = await _sut.GetCategory(categoryId);

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetCategory_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null!);

        // Act
        var result = await _sut.GetCategory(categoryId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetCategory_WhenCalledWithValidId_ShouldCallService()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(actualCategory);

        // Act
        await _sut.GetCategory(categoryId);

        // Assert
        _categoryService.Verify(x => x.GetCategoryByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetCategory_WhenCalledWithValidId_ShouldCallMapper()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(actualCategory);

        // Act
        await _sut.GetCategory(categoryId);

        // Assert
        _mapper.Verify(x => x.Map<CategoryResponse>(actualCategory), Times.Once);
    }

    [Fact]
    public async Task GetCategory_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var categoryId = _fixture.Create<Guid>();
        var actualCategory = _fixture.Create<Category>();
        _mapper.Setup(x => x.Map<CategoryResponse>(actualCategory)).Returns(_fixture.Create<CategoryResponse>());
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetCategory(categoryId);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetAllCategories_WhenCalled_ShouldReturn200StatusCode()
    {
        // Arrange
        var actualCategories = _fixture.CreateMany<Category>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<CategoryResponse>>(actualCategories))
            .Returns(_fixture.CreateMany<CategoryResponse>().ToList());
        _categoryService.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(actualCategories);

        // Act
        var result = await _sut.GetAllCategories();

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async void GetAllCategories_WhenCalled_ShouldCallService()
    {
        // Arrange
        var actualCategories = _fixture.CreateMany<Category>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<CategoryResponse>>(actualCategories))
            .Returns(_fixture.CreateMany<CategoryResponse>().ToList());
        _categoryService.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(actualCategories);

        // Act
        await _sut.GetAllCategories();

        // Assert
        _categoryService.Verify(x => x.GetAllCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async void GetAllCategories_WhenCalled_ShouldCallMapper()
    {
        // Arrange
        var actualCategories = _fixture.CreateMany<Category>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<CategoryResponse>>(actualCategories))
            .Returns(_fixture.CreateMany<CategoryResponse>().ToList());
        _categoryService.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(actualCategories);

        // Act
        await _sut.GetAllCategories();

        // Assert
        _mapper.Verify(x => x.Map<IEnumerable<CategoryResponse>>(actualCategories), Times.Once);
    }

    [Fact]
    public async void GetAllCategories_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var actualCategories = _fixture.CreateMany<Category>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<CategoryResponse>>(actualCategories))
            .Returns(_fixture.CreateMany<CategoryResponse>().ToList());
        _categoryService.Setup(x => x.GetAllCategoriesAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetAllCategories();

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task UpdateCategory_WhenCategoryExists_ReturnsOk()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new Category { Id = categoryId };
        var categoryUpdateRequest = new CategoryUpdateRequest();
        var updatedCategory = new Category();
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);
        _categoryService.Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>())).ReturnsAsync(updatedCategory);
        _mapper.Setup(x => x.Map(categoryUpdateRequest, category)).Returns(updatedCategory);
        var categoryResponse = new CategoryResponse();
        _mapper.Setup(x => x.Map<CategoryResponse>(updatedCategory)).Returns(categoryResponse);

        // Act
        var result = await _sut.UpdateCategory(categoryId, categoryUpdateRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.Value.Should().BeEquivalentTo(categoryResponse);
    }

    [Fact]
    public async Task UpdateCategory_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null!);

        // Act
        var result = await _sut.UpdateCategory(categoryId, new CategoryUpdateRequest());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryExists_ReturnsNoContent()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new Category { Id = categoryId };
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _sut.DeleteCategory(categoryId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryService.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null!);

        // Act
        var result = await _sut.DeleteCategory(categoryId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}