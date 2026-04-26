using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ludo.Database.Repository;
using Ludo.Infrastructure.Configurations;
using Ludo.Infrastructure.Repositories.Implementation;
using Ludo.Infrastructure.Repositories.Interfaces;
using Ludo.Services.Abstractions;
using Ludo.Services.Implementations;
using Ludo.Services.Workers;

namespace Ludo.Services.Extensions;

public static class WebApplicationBuilderExtensions
{
    private const string WebAppDatabaseConnectionKey = "WebAppDatabase";

    extension(WebApplicationBuilder builder)
    {
        /// <summary>
        /// This extension method adds the database configuration and repository to the application builder.
        /// </summary>
        public WebApplicationBuilder AddRepository()
        {
            builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection(nameof(DatabaseConfiguration)));
            builder.Services.AddDbContext<WebAppDatabaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(WebAppDatabaseConnectionKey), // This gets the connection string from ConnectionStrings.WebAppDatabase in appsettings.json.
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                        .CommandTimeout((int)TimeSpan.FromMinutes(15).TotalSeconds)));
            builder.Services.AddScoped<IRepository<WebAppDatabaseContext>, Repository<WebAppDatabaseContext>>();

            return builder;
        }

        /// <summary>
        /// This extension method adds any necessary services to the application builder that need to be injected by the framework.
        /// </summary>
        public WebApplicationBuilder AddServices()
        {
            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));
            builder.Services.Configure<FileStorageConfiguration>(builder.Configuration.GetSection(nameof(FileStorageConfiguration)));
            builder.Services.Configure<MailConfiguration>(builder.Configuration.GetSection(nameof(MailConfiguration)));
            builder.Services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<ILoginService, LoginService>()
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IMailService, MailService>();

            return builder;
        }

        /// <summary>
        /// This extension method adds asynchronous workers to the application builder.
        /// </summary>
        public WebApplicationBuilder AddWorkers()
        {
            builder.Services.AddHostedService<InitializerWorker>();

            return builder;
        }
    }
}