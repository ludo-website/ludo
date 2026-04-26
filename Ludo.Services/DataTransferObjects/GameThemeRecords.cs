namespace Ludo.Services.DataTransferObjects;

public record GameThemeCreateRecord
{
    public Guid GameId { get; set; }
    public Guid ThemeId { get; set; }
}

public record GameThemeRecord
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid ThemeId { get; set; }
}
