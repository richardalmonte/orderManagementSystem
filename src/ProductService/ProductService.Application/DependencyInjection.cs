using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Interfaces;

namespace ProductService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, Services.ProductService>();

        return services;
    }
}