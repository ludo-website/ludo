using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Interest : BaseEntity
{
    public decimal Ammount { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
}