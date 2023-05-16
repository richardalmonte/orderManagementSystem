using Microsoft.Extensions.DependencyInjection;
using AddressBookService.Application.Interfaces;
using AddressBookService.Application.Services;

namespace AddressBookService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}