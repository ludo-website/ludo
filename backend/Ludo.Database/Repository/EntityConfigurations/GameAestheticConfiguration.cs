using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class GameAestheticConfiguration : IEntityTypeConfiguration<GameAesthetic>
{
    public void Configure(EntityTypeBuilder<GameAesthetic> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(e => e.UpdatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new {e.GameId, e.AestheticId}).IsUnique();
        builder.HasOne(e => e.Aesthetic).WithMany(e => e.GameAesthetics).HasForeignKey(e => e.AestheticId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Game).WithMany(e => e.GameAesthetics).HasForeignKey(e => e.GameId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
