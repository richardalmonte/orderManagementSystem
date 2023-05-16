using AutoFixture;
using FluentAssertions;
using Moq;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;


namespace OrderService.Tests.Systems.Services;

public class OrderServiceTests
{
    private readonly Mock<OrderServiceDbContext> _context;

    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Fixture _fixture;
    private readonly IOrderService _sut;

    public OrderServiceTests()
    {
        _context = new Mock<OrderServiceDbContext>();
        _fixture = new Fixture();
        _orderRepository = new Mock<IOrderRepository>();
        _sut = new Application.Services.OrderService(_orderRepository.Object);
        
        
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async void CreateOrderAsync_ShouldCreateOrder_WhenCalledWithValidOrder()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        _orderRepository.Setup(x => x.CreateOrderAsync(order)).ReturnsAsync(order);

        // Act

        var response = await _sut.CreateOrderAsync(order);

        // Assert

        _orderRepository.Verify(x => x.CreateOrderAsync(order), Times.Once);
        response.Should().BeEquivalentTo(order);
    }

    [Fact]
    public async void CreateOrderAsync_ShouldThrowArgumentNullException_WhenCalledWithNullOrder()
    {
        // Arrange
        Order order = null!;

        // Act
        var action = async () => await _sut.CreateOrderAsync(order);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetOrderByIdAsync_ShouldReturnOrder_WhenCalledWithValidId()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        _orderRepository.Setup(x => x.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

        // Act
        var response = await _sut.GetOrderByIdAsync(order.Id);

        // Assert
        _orderRepository.Verify(x => x.GetOrderByIdAsync(order.Id), Times.Once);
        response.Should().BeEquivalentTo(order);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnNull_WhenCalledWithInvalidId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderRepository.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null!);

        // Act
        var result = await _sut.GetOrderByIdAsync(orderId);

        // Assert
        _orderRepository.Verify(x => x.GetOrderByIdAsync(orderId), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    public async void GetOrderByIdAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var orderId = Guid.Empty;

        // Act
        var action = async () => await _sut.GetOrderByIdAsync(orderId);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetAllOrdersAsync_ShouldReturnAllOrders_WhenCalled()
    {
        // Arrange
        var orders = _fixture.CreateMany<Order>().ToList();
        _orderRepository.Setup(x => x.GetAllOrdersAsync()).ReturnsAsync(orders);

        // Act
        var response = await _sut.GetAllOrdersAsync();

        // Assert
        _orderRepository.Verify(x => x.GetAllOrdersAsync(), Times.Once);
        response.Should().BeEquivalentTo(orders);
    }

    [Fact]
    public async void GetAllOrdersAsync_ShouldReturnEmptyList_WhenCalled()
    {
        // Arrange
        var orders = new List<Order>();
        _orderRepository.Setup(x => x.GetAllOrdersAsync()).ReturnsAsync(orders);

        // Act
        var response = await _sut.GetAllOrdersAsync();

        // Assert
        _orderRepository.Verify(x => x.GetAllOrdersAsync(), Times.Once);
        response.Should().BeEquivalentTo(orders);
    }


    [Fact]
    public async void UpdateOrderAsync_ShouldUpdateOrder_WhenCalledWithValidOrder()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        _orderRepository.Setup(x => x.UpdateOrderAsync(order)).ReturnsAsync(order);

        // Act
        var response = await _sut.UpdateOrderAsync(order);

        // Assert
        _orderRepository.Verify(x => x.UpdateOrderAsync(order), Times.Once);
        response.Should().BeEquivalentTo(order);
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldThrowArgumentNullException_WhenCalledWithNullOrder()
    {
        // Arrange
        Order order = null!;

        // Act
        var action = async () => await _sut.UpdateOrderAsync(order);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpdateOrderAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        order.Id = Guid.Empty;

        // Act
        var action = async () => await _sut.UpdateOrderAsync(order);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenCalledWithValidId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderRepository.Setup(x => x.DeleteOrderAsync(orderId)).ReturnsAsync(true);

        // Act
        await _sut.DeleteOrderAsync(orderId);

        // Assert
        _orderRepository.Verify(x => x.DeleteOrderAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var orderId = Guid.Empty;

        // Act
        var action = new Func<Task>(async () => await _sut.DeleteOrderAsync(orderId));

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}