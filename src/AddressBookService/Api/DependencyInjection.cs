using AddressBookService.Api.Common.Mapping;
using AddressBookService.Api.Validators;
using Microsoft.OpenApi.Models;

namespace AddressBookService.Api;

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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoSì AddressBookService.Api", Version = "v1" });
        });
        return services;
    }
}