using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GameGenre> GameGenres { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;
}