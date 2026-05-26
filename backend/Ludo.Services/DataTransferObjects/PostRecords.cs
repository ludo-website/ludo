namespace Ludo.Services.DataTransferObjects;

public record PostCreateRecord
{
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public Guid GameId { get; set; }
}

public record PostRecord
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public Guid GameId { get; set; }
}

public record PostUpdateRecord
{
    public Guid Id { get; set; }
    public string? Title { get; set; } = null;
    public string? Message { get; set; } = null;
}
