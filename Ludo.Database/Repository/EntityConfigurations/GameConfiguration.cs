using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).HasMaxLength(255).IsRequired();
        builder.Property(e => e.ShortDescription).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.FullDescription).HasMaxLength(255).IsRequired(false);
        builder.HasAlternateKey(e => e.Title);
        builder.HasOne(e => e.Account).WithMany(e => e.Games).HasForeignKey(e => e.AccountId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
