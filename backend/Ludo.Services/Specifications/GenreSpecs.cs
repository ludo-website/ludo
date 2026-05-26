using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GenreSpec : Specification<Genre>
{
    private GenreSpec() {}
    public static GenreSpec Get(Guid id)
    {
        var spec = new GenreSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GenreSpec GetByName(string name)
    {
        var spec = new GenreSpec();
        spec.Query.Where(e => e.Name == name);
        return spec;
    }
}

public sealed class GenreProjectionSpec : Specification<Genre, GenreRecord>
{
    private GenreProjectionSpec() {}
    public static GenreProjectionSpec Get(Guid id)
    {
        var spec = new GenreProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                Name = e.Name
            });
        return spec;
    }
    private void Sort()
    {
        Query.OrderByDescending(x => x.CreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                Name = e.Name
            });
    }
    private static string? GetSearchExpr(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;
        return search != null ? $"%{search.Replace(" ", "%")}%" : null;
    }
    public static GenreProjectionSpec FilterByName(string? name)
    {
        var spec = new GenreProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
