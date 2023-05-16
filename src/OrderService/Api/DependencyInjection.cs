using Microsoft.OpenApi.Models;
using OrderService.Common.Mapping;
using OrderService.Validators;

namespace OrderService;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddFluentValidation();
        services.AddMappings();

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoSì OrderService.Api", Version = "v1" });
        });
        return services;
    }
}