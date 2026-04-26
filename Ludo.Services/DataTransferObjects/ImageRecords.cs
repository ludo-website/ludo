using Microsoft.AspNetCore.Http;

namespace Ludo.Services.DataTransferObjects;

public record ImageCreateRecord
{
    public IFormFile File { get; set; } = null!;
    public Guid PostId { get; set; }
}

public class ImageRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid PostId { get; set; }
}