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

public class ProductsControllerTest
{
    private readonly Fixture _fixture;
    private readonly ProductsController _sut;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IProductService> _productService;

    private const string ContextScheme = "http";
    private const string ContextHost = "localhost";
    private const int ContextPort = 5000;

    public ProductsControllerTest()
    {
        _fixture = new Fixture();
        _mapper = new Mock<IMapper>();
        _productService = new Mock<IProductService>();
        _sut = new ProductsController(_mapper.Object, _productService.Object);

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
    public async void CreateProduct_WhenCalledWithValidProduct_ShouldReturn201StatusCode()
    {
        // Arrange
        var productRequest = _fixture.Create<ProductRegistrationRequest>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<Product>(productRequest)).Returns(actualProduct);
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.CreateProductAsync(actualProduct)).ReturnsAsync(actualProduct);

        // Act
        var result = await _sut.CreateProduct(productRequest);

        // Assert

        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        objectResult?.Location.Should()
            .Be($"{ContextScheme}://{ContextHost}:{ContextPort}/api/v1/Products/{actualProduct.Id}");
    }

    [Fact]
    public async void CreateProduct_WhenCalledWithValidProduct_ShouldCallMapper()
    {
        // Arrange
        var productRequest = _fixture.Create<ProductRegistrationRequest>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<Product>(productRequest)).Returns(actualProduct);
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.CreateProductAsync(actualProduct)).ReturnsAsync(actualProduct);

        // Act
        await _sut.CreateProduct(productRequest);

        // Assert
        _mapper.Verify(x => x.Map<Product>(productRequest), Times.Once);
        _mapper.Verify(x => x.Map<ProductResponse>(actualProduct), Times.Once);
    }

    [Fact]
    public async void CreateProduct_WhenCalledWithValidProduct_ShouldCallService()
    {
        // Arrange
        var productRequest = _fixture.Create<ProductRegistrationRequest>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<Product>(productRequest)).Returns(actualProduct);
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.CreateProductAsync(actualProduct)).ReturnsAsync(actualProduct);

        // Act
        await _sut.CreateProduct(productRequest);

        // Assert
        _productService.Verify(x => x.CreateProductAsync(actualProduct), Times.Once);
    }


    [Fact]
    public async void CreateProduct_WhenCalledWithInvalidProduct_ShouldReturn400StatusCode()
    {
        // Arrange

        var productRequest = _fixture.Build<ProductRegistrationRequest>()
            .Without(u => u.CategoryId)
            .Create();

        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<Product>(productRequest)).Returns(actualProduct);
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.CreateProductAsync(actualProduct)).ReturnsAsync(actualProduct);

        var validator = new ProductRegistrationRequestValidator();
        var result = await validator.ValidateAsync(productRequest);
        result.AddToModelState(_sut.ModelState, null);

        // Act
        var response = await _sut.CreateProduct(productRequest);

        // Assert

        response.Should().BeOfType<BadRequestObjectResult>();
        var objectResult = response as BadRequestObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [Fact]
    public async void CreateProduct_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var productRequest = _fixture.Create<ProductRegistrationRequest>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<Product>(productRequest)).Returns(actualProduct);
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());

        _productService.Setup(x => x.CreateProductAsync(actualProduct)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.CreateProduct(productRequest);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetProduct_WhenCalledWithValidId_ShouldReturn200StatusCode()
    {
        // Arrange
        var productId = _fixture.Create<Guid>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(actualProduct);

        // Act
        var result = await _sut.GetProduct(productId);

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync((Product)null!);

        // Act
        var result = await _sut.GetProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProduct_WhenCalledWithValidId_ShouldCallService()
    {
        // Arrange
        var productId = _fixture.Create<Guid>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(actualProduct);

        // Act
        await _sut.GetProduct(productId);

        // Assert
        _productService.Verify(x => x.GetProductByIdAsync(productId), Times.Once);
    }

    [Fact]
    public async Task GetProduct_WhenCalledWithValidId_ShouldCallMapper()
    {
        // Arrange
        var productId = _fixture.Create<Guid>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(actualProduct);

        // Act
        await _sut.GetProduct(productId);

        // Assert
        _mapper.Verify(x => x.Map<ProductResponse>(actualProduct), Times.Once);
    }

    [Fact]
    public async Task GetProduct_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var productId = _fixture.Create<Guid>();
        var actualProduct = _fixture.Create<Product>();
        _mapper.Setup(x => x.Map<ProductResponse>(actualProduct)).Returns(_fixture.Create<ProductResponse>());
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetProduct(productId);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetAllProducts_WhenCalled_ShouldReturn200StatusCode()
    {
        // Arrange
        var actualProducts = _fixture.CreateMany<Product>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<ProductResponse>>(actualProducts))
            .Returns(_fixture.CreateMany<ProductResponse>().ToList());
        _productService.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(actualProducts);

        // Act
        var result = await _sut.GetAllProducts();

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async void GetAllProducts_WhenCalled_ShouldCallService()
    {
        // Arrange
        var actualProducts = _fixture.CreateMany<Product>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<ProductResponse>>(actualProducts))
            .Returns(_fixture.CreateMany<ProductResponse>().ToList());
        _productService.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(actualProducts);

        // Act
        await _sut.GetAllProducts();

        // Assert
        _productService.Verify(x => x.GetAllProductsAsync(), Times.Once);
    }

    [Fact]
    public async void GetAllProducts_WhenCalled_ShouldCallMapper()
    {
        // Arrange
        var actualProducts = _fixture.CreateMany<Product>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<ProductResponse>>(actualProducts))
            .Returns(_fixture.CreateMany<ProductResponse>().ToList());
        _productService.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(actualProducts);

        // Act
        await _sut.GetAllProducts();

        // Assert
        _mapper.Verify(x => x.Map<IEnumerable<ProductResponse>>(actualProducts), Times.Once);
    }

    [Fact]
    public async void GetAllProducts_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var actualProducts = _fixture.CreateMany<Product>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<ProductResponse>>(actualProducts))
            .Returns(_fixture.CreateMany<ProductResponse>().ToList());
        _productService.Setup(x => x.GetAllProductsAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetAllProducts();

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductExists_ReturnsOk()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId };
        var productUpdateRequest = new ProductUpdateRequest();
        var updatedProduct = new Product();
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(product);
        _productService.Setup(x => x.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(updatedProduct);
        _mapper.Setup(x => x.Map(productUpdateRequest, product)).Returns(updatedProduct);
        var productResponse = new ProductResponse();
        _mapper.Setup(x => x.Map<ProductResponse>(updatedProduct)).Returns(productResponse);

        // Act
        var result = await _sut.UpdateProduct(productId, productUpdateRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.Value.Should().BeEquivalentTo(productResponse);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync((Product)null!);

        // Act
        var result = await _sut.UpdateProduct(productId, new ProductUpdateRequest());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ReturnsNoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId };
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _sut.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync((Product)null!);

        // Act
        var result = await _sut.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}