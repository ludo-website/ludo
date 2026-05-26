using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ludo.Infrastructure.Configurations;
using Ludo.Infrastructure.Middlewares;
using Serilog;

namespace Ludo.Services.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication application)
    {
        public WebApplication ConfigureApplication(string applicationName)
        {
            application.UseMiddleware<GlobalExceptionHandlerMiddleware>()
                .UseSwagger()
                .UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", applicationName))
                .UseCors()
                .UseRouting()
                .UseAuthentication()
                .UseSerilogRequestLogging()
                .UseAuthorization()
                .UseSerilogRequestLogging(options => 
                {
                    options.MessageTemplate = "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? "");
                        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme.ToUpper());
                        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress?.ToString() ?? "");
                    };
                });
            application.MapOpenApi();
            application.MapControllers();
            return application;
        }
        public WebApplication MigrateDatabase<T>() where T : DbContext
        {
            try
            {
                var options = application.Services.GetRequiredService<IOptions<DatabaseConfiguration>>();
                if (options.Value.EnableAutomaticMigrations)
                {
                    using var scope = application.Services.CreateScope();  
                    var dbContext = scope.ServiceProvider.GetRequiredService<T>();
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                var logger = application.Services.GetService<ILogger>();
                logger?.Error(ex, "Could not perform migration");
            }
            return application;
        }
    }
}
