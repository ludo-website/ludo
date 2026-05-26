using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.Authorization;
using Ludo.Services.Abstractions;

namespace Ludo.Services.Workers;

public class InitializerWorker(ILogger<InitializerWorker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var accountService = scope.ServiceProvider.GetService<IAccountService>();
            if (accountService == null)
            {
                logger.LogInformation("Couldn't create the account service!");
                return;
            }
            var count = await accountService.GetCount(cancellationToken);
            if (count.Result == 0)
            {
                logger.LogInformation("No account found, adding default account!");
                await accountService.Add(new()
                {
                    Email = "admin@default.com",
                    Name = "Admin",
                    Role = AccountRoleEnum.Admin,
                    Password = PasswordUtils.HashPassword("default")
                }, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing database!");
        }
    }
}