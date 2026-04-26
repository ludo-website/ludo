namespace Ludo.Services.DataTransferObjects;

public record CommentCreateRecord
{
    public string Message { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Guid PostId { get; set; }
}

public record CommentRecord
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Guid PostId { get; set; }
}

public record CommentUpdateRecord
{
    public Guid Id { get; set; }
    public string? Message { get; set; } = null;
}
