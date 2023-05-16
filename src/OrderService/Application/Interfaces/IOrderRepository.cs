using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);

    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<Order> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
}