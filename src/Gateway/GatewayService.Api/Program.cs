using GatewayService.Api;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApi(builder.Configuration);

    builder.Configuration
        .AddJsonFile("ocelot.json", false, true)
        .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "GatewayService.Api v1"); });
    }

    app.UseHttpsRedirection();


    app.MapControllers();

    app.UseOcelot();

    app.Run();
}