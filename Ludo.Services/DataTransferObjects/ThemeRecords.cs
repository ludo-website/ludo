namespace Ludo.Services.DataTransferObjects;

public record ThemeCreateRecord
{
    public string Name { get; set; } = null!;
}

public record ThemeRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public record ThemeUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
}
