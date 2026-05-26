using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Aesthetic : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GameAesthetic> GameAesthetics { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;

}