using System.Reflection;
using FluentValidation;

namespace UserService.Validators;

public static class DependencyInjection
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}