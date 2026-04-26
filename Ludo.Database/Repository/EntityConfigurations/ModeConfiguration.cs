using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class ModeConfiguration : IEntityTypeConfiguration<Mode>
{
    public void Configure(EntityTypeBuilder<Mode> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
        builder.HasAlternateKey(e => e.Name);
        builder.HasMany(e => e.Games).WithMany(e => e.Modes).UsingEntity<GameMode>();
    }
}
