using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Path).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
        builder.HasAlternateKey(e => new {e.Path, e.Name});
        builder.HasOne(e => e.Post).WithMany(e => e.Images).HasForeignKey(e => e.PostId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
