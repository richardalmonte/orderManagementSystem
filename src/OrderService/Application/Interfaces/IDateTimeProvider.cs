namespace OrderService.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}