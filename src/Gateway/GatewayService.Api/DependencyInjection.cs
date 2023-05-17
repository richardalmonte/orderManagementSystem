using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;


namespace GatewayService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddControllers();
        services.AddOcelot(configuration)
            .AddCacheManager(settings => settings.WithDictionaryHandle());

        
        // This Swagger configuration is not working properly with Ocelot and should be
        // replaced with a custom implementation of Swagger UI. 
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "GatewayService.Api", Version = "v1" }); });

        return services;
    }
}