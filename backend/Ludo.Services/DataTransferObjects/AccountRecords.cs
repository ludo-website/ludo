using Ludo.Database.Repository.Enums;

namespace Ludo.Services.DataTransferObjects;

public record AccountCreateRecord
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AccountRoleEnum Role { get; set; }
}

public record AccountRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public AccountRoleEnum Role { get; set; }
}

public record AccountUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Password { get; set; } = null;
}
