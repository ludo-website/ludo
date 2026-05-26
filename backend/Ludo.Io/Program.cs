using Ludo.Database.Repository;
using Ludo.Infrastructure.Extensions;
using Ludo.Services.Extensions;

namespace Ludo.Io;

public static class Program
{
    private const string ApplicationName = "Ludo-Io";
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddCorsConfiguration()
            .AddRepository()
            .AddAuthorizationWithSwagger(ApplicationName)
            .AddIoServices()
            .UseLogger()
            .AddWorkers()
            .AddApi();
        var app = builder
            .Build()
            .ConfigureApplication(ApplicationName)
            .MigrateDatabase<WebAppDatabaseContext>();
        app.Run();
    }
}