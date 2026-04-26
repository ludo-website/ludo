using Ludo.Database.Repository;
using Ludo.Infrastructure.Extensions;
using Ludo.Services.Extensions;

namespace Ludo.Api;

public static class Program
{
    private const string ApplicationName = "Ludo";
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.AddCorsConfiguration()
            .AddRepository()
            .AddAuthorizationWithSwagger(ApplicationName)
            .AddServices()
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