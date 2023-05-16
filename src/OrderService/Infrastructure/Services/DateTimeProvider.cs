using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}