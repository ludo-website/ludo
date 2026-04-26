namespace Ludo.Services.DataTransferObjects;

public record GamePlatformCreateRecord
{
    public Guid GameId { get; set; }
    public Guid PlatformId { get; set; }
}

public record GamePlatformRecord
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid PlatformId { get; set; }
}
