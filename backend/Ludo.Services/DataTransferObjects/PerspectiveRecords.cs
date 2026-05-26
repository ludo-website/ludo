namespace Ludo.Services.DataTransferObjects;

public record PerspectiveCreateRecord
{
    public string Name { get; set; } = null!;
}

public record PerspectiveRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public record PerspectiveUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
}
