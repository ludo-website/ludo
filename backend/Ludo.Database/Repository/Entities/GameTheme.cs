using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class GameTheme : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public Guid ThemeId { get; set; }
    public Theme Theme { get; set; } = null!;
}