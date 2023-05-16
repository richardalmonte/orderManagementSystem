using AutoFixture;
using FluentAssertions;
using Moq;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Tests.Systems.Services;

public class ProductServiceTests
{
    private readonly Mock<ProductServiceDbContext> _context;

    private readonly Mock<IProductRepository> _productRepository;
    private readonly Fixture _fixture;
    private readonly IProductService _sut;

    public ProductServiceTests()
    {
        _context = new Mock<ProductServiceDbContext>();
        _fixture = new Fixture();
        _productRepository = new Mock<IProductRepository>();
        _sut = new Application.Services.ProductService(_productRepository.Object);


        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async void CreateProductAsync_ShouldCreateProduct_WhenCalledWithValidProduct()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _productRepository.Setup(x => x.CreateProductAsync(product)).ReturnsAsync(product);

        // Act

        var response = await _sut.CreateProductAsync(product);

        // Assert

        _productRepository.Verify(x => x.CreateProductAsync(product), Times.Once);
        response.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async void CreateProductAsync_ShouldThrowArgumentNullException_WhenCalledWithNullProduct()
    {
        // Arrange
        Product product = null!;

        // Act
        var action = async () => await _sut.CreateProductAsync(product);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetProductByIdAsync_ShouldReturnProduct_WhenCalledWithValidId()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _productRepository.Setup(x => x.GetProductByIdAsync(product.Id)).ReturnsAsync(product);

        // Act
        var response = await _sut.GetProductByIdAsync(product.Id);

        // Assert
        _productRepository.Verify(x => x.GetProductByIdAsync(product.Id), Times.Once);
        response.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnNull_WhenCalledWithInvalidId()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepository.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync((Product)null!);

        // Act
        var result = await _sut.GetProductByIdAsync(productId);

        // Assert
        _productRepository.Verify(x => x.GetProductByIdAsync(productId), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    public async void GetProductByIdAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var productId = Guid.Empty;

        // Act
        var action = async () => await _sut.GetProductByIdAsync(productId);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetAllProductsAsync_ShouldReturnAllProducts_WhenCalled()
    {
        // Arrange
        var products = _fixture.CreateMany<Product>().ToList();
        _productRepository.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

        // Act
        var response = await _sut.GetAllProductsAsync();

        // Assert
        _productRepository.Verify(x => x.GetAllProductsAsync(), Times.Once);
        response.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async void GetAllProductsAsync_ShouldReturnEmptyList_WhenCalled()
    {
        // Arrange
        var products = new List<Product>();
        _productRepository.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

        // Act
        var response = await _sut.GetAllProductsAsync();

        // Assert
        _productRepository.Verify(x => x.GetAllProductsAsync(), Times.Once);
        response.Should().BeEquivalentTo(products);
    }


    [Fact]
    public async void UpdateProductAsync_ShouldUpdateProduct_WhenCalledWithValidProduct()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _productRepository.Setup(x => x.UpdateProductAsync(product)).ReturnsAsync(product);

        // Act
        var response = await _sut.UpdateProductAsync(product);

        // Assert
        _productRepository.Verify(x => x.UpdateProductAsync(product), Times.Once);
        response.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrowArgumentNullException_WhenCalledWithNullProduct()
    {
        // Arrange
        Product product = null!;

        // Act
        var action = async () => await _sut.UpdateProductAsync(product);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpdateProductAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        product.Id = Guid.Empty;

        // Act
        var action = async () => await _sut.UpdateProductAsync(product);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProduct_WhenCalledWithValidId()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepository.Setup(x => x.DeleteProductAsync(productId)).ReturnsAsync(true);

        // Act
        await _sut.DeleteProductAsync(productId);

        // Assert
        _productRepository.Verify(x => x.DeleteProductAsync(productId), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var productId = Guid.Empty;

        // Act
        var action = new Func<Task>(async () => await _sut.DeleteProductAsync(productId));

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}