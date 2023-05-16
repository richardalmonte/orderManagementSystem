using System.Reflection;

namespace OrderService.Common.Mapping;

public static class DependencyInjection
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}