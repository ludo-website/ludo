using Microsoft.EntityFrameworkCore;
using Ludo.Infrastructure.Extensions;

namespace Ludo.Database.Repository;

public sealed class WebAppDatabaseContext(DbContextOptions<WebAppDatabaseContext> options, IServiceProvider serviceProvider) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("unaccent")
            .ApplyConfigurationsFromAssemblies([], serviceProvider);
    }
}