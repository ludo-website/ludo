namespace Ludo.Services.DataTransferObjects;

public record GameGenreCreateRecord
{
    public Guid GameId { get; set; }
    public Guid GenreId { get; set; }
}

public record GameGenreRecord
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid GenreId { get; set; }
}
