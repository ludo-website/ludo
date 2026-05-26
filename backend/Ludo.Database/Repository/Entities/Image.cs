using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Image : BaseEntity
{
    public string Path { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}
