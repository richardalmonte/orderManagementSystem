namespace UserService.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}