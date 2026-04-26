using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Mode : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GameMode> GameModes { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;
}