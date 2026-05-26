using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Perspective : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GamePerspective> GamePerspectives { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;
}