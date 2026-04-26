namespace Ludo.Services.DataTransferObjects;

public record GamePerspectiveCreateRecord
{
    public Guid GameId { get; set; }
    public Guid PerspectiveId { get; set; }
}

public record GamePerspectiveRecord
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid PerspectiveId { get; set; }
}
