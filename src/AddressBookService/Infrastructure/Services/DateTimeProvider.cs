using AddressBookService.Application.Interfaces;

namespace AddressBookService.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}