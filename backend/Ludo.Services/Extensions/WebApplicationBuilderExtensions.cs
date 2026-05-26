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
using Ludo.Services.Clients.Abstractions;
using Ludo.Services.Clients.Implementation;

namespace Ludo.Services.Extensions;

public static class WebApplicationBuilderExtensions
{
    private const string WebAppDatabaseConnectionKey = "WebAppDatabase";
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddRepository()
        {
            builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection(nameof(DatabaseConfiguration)));
            builder.Services.AddDbContext<WebAppDatabaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(WebAppDatabaseConnectionKey),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                        .CommandTimeout((int)TimeSpan.FromMinutes(15).TotalSeconds)));
            builder.Services.AddScoped<IRepository<WebAppDatabaseContext>, Repository<WebAppDatabaseContext>>();
            return builder;
        }
        public WebApplicationBuilder AddApiServices()
        {
            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));
            var baseAddress = builder.Configuration.GetValue<string>("ClientConfiguration:BaseAddress");
            builder.Services.AddHttpClient<IAccountClient, AccountClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Account/");
            });
            builder.Services.AddHttpClient<IImageClient, ImageClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Image/");
            });
            builder.Services.AddHttpClient<IGameClient, GameClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Game/");
            });
            builder.Services.AddHttpClient<IInterestClient, InterestClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Interest/");
            });
            builder.Services.AddHttpClient<IPostClient, PostClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Post/");
            });
            builder.Services.AddHttpClient<ICommentClient, CommentClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Comment/");
            });
            builder.Services.AddHttpClient<IAestheticClient, AestheticClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Aesthetic/");
            });
            builder.Services.AddHttpClient<IGenreClient, GenreClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Genre/");
            });
            builder.Services.AddHttpClient<IModeClient, ModeClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Mode/");
            });
            builder.Services.AddHttpClient<IPerspectiveClient, PerspectiveClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Perspective/");
            });
            builder.Services.AddHttpClient<IPlatformClient, PlatformClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Platform/");
            });
            builder.Services.AddHttpClient<IThemeClient, ThemeClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Theme/");
            });
            builder.Services.AddHttpClient<IGameAestheticClient, GameAestheticClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/GameAesthetic/");
            });
            builder.Services.AddHttpClient<IGameGenreClient, GameGenreClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/GameGenre/");
            });
            builder.Services.AddHttpClient<IGameModeClient, GameModeClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/GameMode/");
            });
            builder.Services.AddHttpClient<IGamePerspectiveClient, GamePerspectiveClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/GamePerspective/");
            });
            builder.Services.AddHttpClient<IGamePlatformClient, GamePlatformClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/GamePlatform/");
            });
            builder.Services.AddHttpClient<IGameThemeClient, GameThemeClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/GameTheme/");
            });
            return builder;
        }
        public WebApplicationBuilder AddAuthServices()
        {
            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));
            var baseAddress = builder.Configuration.GetValue<string>("ClientConfiguration:BaseAddress");
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddHttpClient<IAccountClient, AccountClient>(client => {
                client.BaseAddress = new Uri(baseAddress + "/io/Account/");
            });
            return builder;
        }
        public WebApplicationBuilder AddIoServices()
        {
            builder.Services.Configure<FileStorageConfiguration>(builder.Configuration.GetSection(nameof(FileStorageConfiguration)));
            builder.Services.Configure<MailConfiguration>(builder.Configuration.GetSection(nameof(MailConfiguration)));
            builder.Services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<IMailService, MailService>()
                .AddScoped<IGameService, GameService>()
                .AddScoped<IInterestService, InterestService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<IAestheticService, AestheticService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IModeService, ModeService>()
                .AddScoped<IPerspectiveService, PerspectiveService>()
                .AddScoped<IPlatformService, PlatformService>()
                .AddScoped<IThemeService, ThemeService>()
                .AddScoped<IGameAestheticService, GameAestheticService>()
                .AddScoped<IGameGenreService, GameGenreService>()
                .AddScoped<IGameModeService, GameModeService>()
                .AddScoped<IGamePerspectiveService, GamePerspectiveService>()
                .AddScoped<IGamePlatformService, GamePlatformService>()
                .AddScoped<IGameThemeService, GameThemeService>();
            return builder;
        }
        public WebApplicationBuilder AddWorkers()
        {
            builder.Services.AddHostedService<InitializerWorker>();
            return builder;
        }
    }
}