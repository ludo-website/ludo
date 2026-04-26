namespace Ludo.Services.DataTransferObjects;

public record GameAestheticCreateRecord
{
    public Guid GameId { get; set; }
    public Guid AestheticId { get; set; }
}

public record GameAestheticRecord
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid AestheticId { get; set; }
}
