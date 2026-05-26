namespace Ludo.Services.DataTransferObjects;

public record LoginRecord
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
};

public record LoginResponseRecord
{
    public string Token { get; set; } = null!;
    public AccountRecord Account { get; set; } = null!;
}
