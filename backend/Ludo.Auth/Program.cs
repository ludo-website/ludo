using Ludo.Infrastructure.Extensions;
using Ludo.Services.Extensions;

namespace Ludo.Auth;

public static class Program
{
    private const string ApplicationName = "Ludo-Auth";
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddCorsConfiguration()
            .AddRepository()
            .AddAuthorizationWithSwagger(ApplicationName)
            .ConfigureAuthentication()
            .AddAuthServices()
            .UseLogger()
            .AddApi();
        var app = builder
            .Build()
            .ConfigureApplication(ApplicationName);
        app.Run();
    }
}