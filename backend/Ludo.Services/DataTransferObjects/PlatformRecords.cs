namespace Ludo.Services.DataTransferObjects;

public record PlatformCreateRecord
{
    public string Name { get; set; } = null!;
}

public record PlatformRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public record PlatformUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
}
