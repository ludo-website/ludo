using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.BaseObjects;

namespace Ludo.Database.Repository.Entities;

public class Account : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AccountRoleEnum Role { get; set; }
    public ICollection<Comment> Comments { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = null!;
    public ICollection<Interest> Interests { get; set; } = null!;
}