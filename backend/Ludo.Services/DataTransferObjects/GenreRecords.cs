namespace Ludo.Services.DataTransferObjects;

public record GenreCreateRecord
{
    public string Name { get; set; } = null!;
}

public record GenreRecord
{

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public record GenreUpdateRecord
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
}
