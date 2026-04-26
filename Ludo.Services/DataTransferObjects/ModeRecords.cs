namespace Ludo.Services.DataTransferObjects;

public record ModeCreateRecord
{
    public string Name { get; set; } = null!;
}

public record ModeRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public record ModeUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
}
