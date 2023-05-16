using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderService.Application.Interfaces;
using OrderService.Contracts.V1.Requests;
using OrderService.Contracts.V1.Responses;
using OrderService.Controllers.V1;
using OrderService.Domain.Entities;
using OrderService.Validators;

namespace OrderService.Tests.Systems.Controllers.V1;

public class OrderControllerTest
{
    private readonly Fixture _fixture;
    private readonly OrdersController _sut;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IOrderService> _orderService;

    private const string contextScheme = "http";
    private const string contextHost = "localhost";
    private const int contextPort = 5000;

    public OrderControllerTest()
    {
        _fixture = new Fixture();
        _mapper = new Mock<IMapper>();
        _orderService = new Mock<IOrderService>();
        _sut = new OrdersController(_mapper.Object, _orderService.Object);

        _sut.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                Request =
                {
                    Scheme = contextScheme,
                    Host = new HostString(contextHost, contextPort)
                }
            }
        };


        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async void CreateOrder_WhenCalledWithValidOrder_ShouldReturn201StatusCode()
    {
        // Arrange
        var orderRequest = _fixture.Create<OrderRegistrationRequest>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<Order>(orderRequest)).Returns(actualOrder);
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.CreateOrderAsync(actualOrder)).ReturnsAsync(actualOrder);

        // Act
        var result = await _sut.CreateOrder(orderRequest);

        // Assert

        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        objectResult?.Location.Should()
            .Be($"{contextScheme}://{contextHost}:{contextPort}/api/v1/orders/{actualOrder.Id}");
    }

    [Fact]
    public async void CreateOrder_WhenCalledWithValidOrder_ShouldCallMapper()
    {
        // Arrange
        var orderRequest = _fixture.Create<OrderRegistrationRequest>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<Order>(orderRequest)).Returns(actualOrder);
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.CreateOrderAsync(actualOrder)).ReturnsAsync(actualOrder);

        // Act
        await _sut.CreateOrder(orderRequest);

        // Assert
        _mapper.Verify(x => x.Map<Order>(orderRequest), Times.Once);
        _mapper.Verify(x => x.Map<OrderResponse>(actualOrder), Times.Once);
    }

    [Fact]
    public async void CreateOrder_WhenCalledWithValidOrder_ShouldCallService()
    {
        // Arrange
        var orderRequest = _fixture.Create<OrderRegistrationRequest>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<Order>(orderRequest)).Returns(actualOrder);
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.CreateOrderAsync(actualOrder)).ReturnsAsync(actualOrder);

        // Act
        await _sut.CreateOrder(orderRequest);

        // Assert
        _orderService.Verify(x => x.CreateOrderAsync(actualOrder), Times.Once);
    }


    [Fact]
    public async void CreateOrder_WhenCalledWithInvalidOrder_ShouldReturn400StatusCode()
    {
        // Arrange
        var orderRequest = _fixture.Build<OrderRegistrationRequest>()
            .Without(u => u.UserId)
            .Create();

        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<Order>(orderRequest)).Returns(actualOrder);
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.CreateOrderAsync(actualOrder)).ReturnsAsync(actualOrder);

        var validator = new OrderRegistrationRequestValidator();
        var result = await validator.ValidateAsync(orderRequest);
        result.AddToModelState(_sut.ModelState, null);

        // Act
        var response = await _sut.CreateOrder(orderRequest);

        // Assert

        response.Should().BeOfType<BadRequestObjectResult>();
        var objectResult = response as BadRequestObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [Fact]
    public async void CreateOrder_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var orderRequest = _fixture.Create<OrderRegistrationRequest>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<Order>(orderRequest)).Returns(actualOrder);
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());

        _orderService.Setup(x => x.CreateOrderAsync(actualOrder)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.CreateOrder(orderRequest);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetOrder_WhenCalledWithValidId_ShouldReturn200StatusCode()
    {
        // Arrange
        var orderId = _fixture.Create<Guid>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(actualOrder);

        // Act
        var result = await _sut.GetOrder(orderId);

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetOrder_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null!);

        // Act
        var result = await _sut.GetOrder(orderId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetOrder_WhenCalledWithValidId_ShouldCallService()
    {
        // Arrange
        var orderId = _fixture.Create<Guid>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(actualOrder);

        // Act
        await _sut.GetOrder(orderId);

        // Assert
        _orderService.Verify(x => x.GetOrderByIdAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task GetOrder_WhenCalledWithValidId_ShouldCallMapper()
    {
        // Arrange
        var orderId = _fixture.Create<Guid>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(actualOrder);

        // Act
        await _sut.GetOrder(orderId);

        // Assert
        _mapper.Verify(x => x.Map<OrderResponse>(actualOrder), Times.Once);
    }

    [Fact]
    public async Task GetOrder_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var orderId = _fixture.Create<Guid>();
        var actualOrder = _fixture.Create<Order>();
        _mapper.Setup(x => x.Map<OrderResponse>(actualOrder)).Returns(_fixture.Create<OrderResponse>());
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetOrder(orderId);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetAllOrders_WhenCalled_ShouldReturn200StatusCode()
    {
        // Arrange
        var actualOrders = _fixture.CreateMany<Order>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<OrderResponse>>(actualOrders))
            .Returns(_fixture.CreateMany<OrderResponse>().ToList());
        _orderService.Setup(x => x.GetAllOrdersAsync()).ReturnsAsync(actualOrders);

        // Act
        var result = await _sut.GetAllOrders();

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async void GetAllOrders_WhenCalled_ShouldCallService()
    {
        // Arrange
        var actualOrders = _fixture.CreateMany<Order>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<OrderResponse>>(actualOrders))
            .Returns(_fixture.CreateMany<OrderResponse>().ToList());
        _orderService.Setup(x => x.GetAllOrdersAsync()).ReturnsAsync(actualOrders);

        // Act
        await _sut.GetAllOrders();

        // Assert
        _orderService.Verify(x => x.GetAllOrdersAsync(), Times.Once);
    }

    [Fact]
    public async void GetAllOrders_WhenCalled_ShouldCallMapper()
    {
        // Arrange
        var actualOrders = _fixture.CreateMany<Order>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<OrderResponse>>(actualOrders))
            .Returns(_fixture.CreateMany<OrderResponse>().ToList());
        _orderService.Setup(x => x.GetAllOrdersAsync()).ReturnsAsync(actualOrders);

        // Act
        await _sut.GetAllOrders();

        // Assert
        _mapper.Verify(x => x.Map<IEnumerable<OrderResponse>>(actualOrders), Times.Once);
    }

    [Fact]
    public async void GetAllOrders_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var actualOrders = _fixture.CreateMany<Order>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<OrderResponse>>(actualOrders))
            .Returns(_fixture.CreateMany<OrderResponse>().ToList());
        _orderService.Setup(x => x.GetAllOrdersAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetAllOrders();

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task UpdateOrder_WhenOrderExists_ReturnsOk()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId };
        var orderUpdateRequest = new OrderUpdateRequest();
        var updatedOrder = new Order();
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _orderService.Setup(x => x.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(updatedOrder);
        _mapper.Setup(x => x.Map(orderUpdateRequest, order)).Returns(updatedOrder);
        var orderResponse = new OrderResponse();
        _mapper.Setup(x => x.Map<OrderResponse>(updatedOrder)).Returns(orderResponse);

        // Act
        var result = await _sut.UpdateOrder(orderId, orderUpdateRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.Value.Should().BeEquivalentTo(orderResponse);
    }

    [Fact]
    public async Task UpdateOrder_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null!);

        // Act
        var result = await _sut.UpdateOrder(orderId, new OrderUpdateRequest());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteOrder_WhenOrderExists_ReturnsNoContent()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId };
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);

        // Act
        var result = await _sut.DeleteOrder(orderId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteOrder_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null!);

        // Act
        var result = await _sut.DeleteOrder(orderId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}