using Ludo.Infrastructure.Extensions;
using Ludo.Services.Extensions;

namespace Ludo.Api;

public static class Program
{
    private const string ApplicationName = "Ludo-Api";
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddCorsConfiguration()
            .AddRepository()
            .AddAuthorizationWithSwagger(ApplicationName)
            .ConfigureAuthentication()
            .AddApiServices()
            .UseLogger()
            .AddApi();
        var app = builder
            .Build()
            .ConfigureApplication(ApplicationName);
        app.Run();
    }
}