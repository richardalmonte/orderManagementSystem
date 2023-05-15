using UserService.Common.Mapping;
using UserService.Validators;

namespace UserService;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddFluentValidation();
        services.AddMappings();
        return services;
    }
}