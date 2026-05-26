using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class PostSpec : Specification<Post>
{
    private PostSpec() {}
    public static PostSpec Get(Guid id)
    {
        var spec = new PostSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
}

public sealed class PostProjectionSpec : Specification<Post, PostRecord>
{
    private PostProjectionSpec() {}
    public static PostProjectionSpec Get(Guid id)
    {
        var spec = new PostProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                Title = e.Title,
                Message = e.Message,
                GameId = e.GameId
            });
        return spec;
    }
    private void Sort()
    {
        Query.OrderByDescending(x => x.CreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                Title = e.Title,
                Message = e.Message,
                GameId = e.GameId
            });
    }
    public static PostProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new PostProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    private static string? GetSearchExpr(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;
        return search != null ? $"%{search.Replace(" ", "%")}%" : null;
    }
    public static PostProjectionSpec FilterByTitle(string? title)
    {
        var spec = new PostProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(title);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Title, searchExpr));
        }
        return spec;
    }
}
