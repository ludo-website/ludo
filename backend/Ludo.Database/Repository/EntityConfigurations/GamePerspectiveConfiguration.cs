using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class GamePerspectiveConfiguration : IEntityTypeConfiguration<GamePerspective>
{
    public void Configure(EntityTypeBuilder<GamePerspective> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(e => e.UpdatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new {e.GameId, e.PerspectiveId}).IsUnique();
        builder.HasOne(e => e.Perspective).WithMany(e => e.GamePerspectives).HasForeignKey(e => e.PerspectiveId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Game).WithMany(e => e.GamePerspectives).HasForeignKey(e => e.GameId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
