using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class ImageProjectionSpec : Specification<Image, ImageRecord>
{
    private ImageProjectionSpec() {}
    public static ImageProjectionSpec Get(Guid id)
    {
        var spec = new ImageProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                Name = e.Name,
                PostId = e.PostId
            });
        return spec;
    }
    private void Sort()
    {
        Query.OrderByDescending(x => x.CreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                Name = e.Name,
                PostId = e.PostId
            });
    }
    public static ImageProjectionSpec FilterByPost(Guid? postId)
    {
        var spec = new ImageProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.PostId == postId);
        return spec;
    }
}
