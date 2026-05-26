using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Post : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
    public ICollection<Image> Images { get; set; } = null!;
}