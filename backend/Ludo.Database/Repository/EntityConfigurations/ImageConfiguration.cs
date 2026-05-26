using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ludo.Database.Repository.Entities;

namespace Ludo.Database.Repository.EntityConfigurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.Property(e => e.UpdatedAt).IsRequired().HasColumnType("timestamp with time zone");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Path).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
        builder.HasIndex(e => new {e.Path, e.Name}).IsUnique();
        builder.HasOne(e => e.Post).WithMany(e => e.Images).HasForeignKey(e => e.PostId)
            .HasPrincipalKey(e => e.Id).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
