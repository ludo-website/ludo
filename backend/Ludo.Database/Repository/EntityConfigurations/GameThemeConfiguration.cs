using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class GameThemeConfiguration : IEntityTypeConfiguration<GameTheme>
{
    public void Configure(EntityTypeBuilder<GameTheme> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(e => e.UpdatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new {e.GameId, e.ThemeId}).IsUnique();
        builder.HasOne(e => e.Theme).WithMany(e => e.GameThemes).HasForeignKey(e => e.ThemeId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Game).WithMany(e => e.GameThemes).HasForeignKey(e => e.GameId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
