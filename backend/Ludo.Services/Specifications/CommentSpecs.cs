using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class CommentSpec : Specification<Comment>
{
    private CommentSpec() {}
    public static CommentSpec Get(Guid id)
    {
        var spec = new CommentSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
}

public sealed class CommentProjectionSpec : Specification<Comment, CommentRecord>
{
    private CommentProjectionSpec() {}
    public static CommentProjectionSpec Get(Guid id)
    {
        var spec = new CommentProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                Message = e.Message,
                AccountId = e.AccountId,
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
                Message = e.Message,
                AccountId = e.AccountId,
                PostId = e.PostId
            });
    }
    public static CommentProjectionSpec FilterByAccount(Guid? accountId)
    {
        var spec = new CommentProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.AccountId == accountId);
        return spec;
    }
    public static CommentProjectionSpec FilterByPost(Guid? postId)
    {
        var spec = new CommentProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.PostId == postId);
        return spec;
    }
}
