using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class GamePerspective : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public Guid PerspectiveId { get; set; }
    public Perspective Perspective { get; set; } = null!;
}