using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class ThemeSpec : Specification<Theme>
{
    private ThemeSpec() {}
    public static ThemeSpec Get(Guid id)
    {
        var spec = new ThemeSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static ThemeSpec GetByName(string name)
    {
        var spec = new ThemeSpec();
        spec.Query.Where(e => e.Name == name);
        return spec;
    }
}

public sealed class ThemeProjectionSpec : Specification<Theme, ThemeRecord>
{
    private ThemeProjectionSpec() {}
    public static ThemeProjectionSpec Get(Guid id)
    {
        var spec = new ThemeProjectionSpec();
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
    public static ThemeProjectionSpec FilterByName(string? name)
    {
        var spec = new ThemeProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
