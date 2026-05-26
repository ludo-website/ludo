using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GameSpec : Specification<Game>
{
    private GameSpec() {}
    public static GameSpec Get(Guid id)
    {
        var spec = new GameSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GameSpec GetByTitle(string title)
    {
        var spec = new GameSpec();
        spec.Query.Where(e => e.Title == title);
        return spec;
    }
}

public sealed class GameProjectionSpec : Specification<Game, GameRecord>
{
    private GameProjectionSpec() {}
    public static GameProjectionSpec Get(Guid id)
    {
        var spec = new GameProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                Title = e.Title,
                ShortDescription = e.ShortDescription,
                FullDescription = e.FullDescription,
                AccountId = e.AccountId
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
                ShortDescription = e.ShortDescription,
                FullDescription = e.FullDescription,
                AccountId = e.AccountId
            });
    }
    public static GameProjectionSpec FilterByAccount(Guid? accountId)
    {
        var spec = new GameProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.AccountId == accountId);
        return spec;
    }
    private static string? GetSearchExpr(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;
        return search != null ? $"%{search.Replace(" ", "%")}%" : null;
    }
    public static GameProjectionSpec FilterByTitle(string? title)
    {
        var spec = new GameProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(title);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Title, searchExpr));
        }
        return spec;
    }
}
