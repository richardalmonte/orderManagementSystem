using Microsoft.OpenApi.Models;
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
        
        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoSì UserService.Api", Version = "v1" });
        });
        return services;
    }
}