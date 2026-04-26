namespace Ludo.Services.DataTransferObjects;

public record GameModeCreateRecord
{
    public Guid GameId { get; set; }
    public Guid ModeId { get; set; }
}

public record GameModeRecord
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid ModeId { get; set; }
}
