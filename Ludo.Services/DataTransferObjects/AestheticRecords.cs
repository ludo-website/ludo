namespace Ludo.Services.DataTransferObjects;

public record AestheticCreateRecord
{
    public string Name { get; set; } = null!;
}

public record AestheticRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public record AestheticUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
}
