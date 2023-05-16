using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderServiceDbContext _context;

    public OrderRepository(OrderServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var orderEntry = await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return orderEntry.Entity;
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(orderId));
        }

        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(order.Id));
        }

        var updatedOrder = _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        if (updatedOrder?.Entity is null)
        {
            throw new Exception("Order not found");
        }

        return updatedOrder.Entity;
    }

    public async Task<bool> DeleteOrderAsync(Guid orderId)
    {
        ArgumentNullException.ThrowIfNull(orderId);

        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

        if (order is null)
        {
            throw new Exception("Order not found");
        }

        _context.Orders.Remove(order);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}