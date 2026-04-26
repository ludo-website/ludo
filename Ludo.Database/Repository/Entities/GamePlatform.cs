using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class GamePlatform : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public Guid PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}