using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class GameMode : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public Guid ModeId { get; set; }
    public Mode Mode { get; set; } = null!;
}