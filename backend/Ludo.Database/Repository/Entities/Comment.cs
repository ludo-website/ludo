using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Comment : BaseEntity
{
    public string Message { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}