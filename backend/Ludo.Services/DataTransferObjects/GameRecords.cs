namespace Ludo.Services.DataTransferObjects;

public record GameCreateRecord
{
    public string Title { get; set; } = null!;
    public string? ShortDescription { get; set; } = null;
    public string? FullDescription { get; set; } = null;
    public Guid AccountId { get; set; }
}

public record GameRecord
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? ShortDescription { get; set; } = null;
    public string? FullDescription { get; set; } = null;
    public Guid AccountId { get; set; }
}

public record GameUpdateRecord
{
    public Guid Id { get; set; }
    public string? Title { get; set; } = null;
    public string? ShortDescription { get; set; } = null;
    public string? FullDescription { get; set; } = null;
    public Guid? AccountId { get; set; }
}
