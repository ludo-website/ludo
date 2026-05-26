using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Platform : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GamePlatform> GameMode { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;
}