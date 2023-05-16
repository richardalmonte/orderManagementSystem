using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid orderId);
}