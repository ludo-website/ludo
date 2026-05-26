using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Theme : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GameTheme> GameThemes { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;
}