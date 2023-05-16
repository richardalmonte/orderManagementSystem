using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<Order> CreateOrderAsync(Order orderRequest)
    {
        ArgumentNullException.ThrowIfNull(orderRequest);

        return _orderRepository.CreateOrderAsync(orderRequest);
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        ArgumentNullException.ThrowIfNull(orderId);

        if (orderId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(orderId));
        }

        return await _orderRepository.GetOrderByIdAsync(orderId);
    }

    public Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return _orderRepository.GetAllOrdersAsync();
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(order.Id));
        }

        return await _orderRepository.UpdateOrderAsync(order);
    }

    public Task DeleteOrderAsync(Guid orderId)
    {
        ArgumentNullException.ThrowIfNull(orderId);

        if (orderId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(orderId));
        }

        return _orderRepository.DeleteOrderAsync(orderId);
    }
}