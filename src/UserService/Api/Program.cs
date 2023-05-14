using UserService;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddPresentation();
}


var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}