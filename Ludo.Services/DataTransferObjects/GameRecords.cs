namespace Ludo.Services.DataTransferObjects;

public record GameCreateRecord
{
    public string Title { get; set; } = null!;
    public string? Short_Description { get; set; } = null;
    public string? Full_Description { get; set; } = null;
    public Guid Account_Id { get; set; }
}

public record GameRecord
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Short_Description { get; set; } = null;
    public string? Full_Description { get; set; } = null;
    public Guid Account_Id { get; set; }
}

public record GameUpdateRecord
{
    public Guid Id { get; set; }
    public string? Title { get; set; } = null;
    public string? Short_Description { get; set; } = null;
    public string? Full_Description { get; set; } = null;
    public Guid? Account_Id { get; set; }
}
