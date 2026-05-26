using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class GameAesthetic : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
    public Guid AestheticId { get; set; }
    public Aesthetic Aesthetic { get; set; } = null!;
}