using Microsoft.OpenApi.Models;
using ProductService.API.Common.Mapping;
using ProductService.API.Validators;

namespace ProductService.API;

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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoSì ProductService.Api", Version = "v1" });
        });
        return services;
    }
}