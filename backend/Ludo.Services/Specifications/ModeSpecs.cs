using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class ModeSpec : Specification<Mode>
{
    private ModeSpec() {}
    public static ModeSpec Get(Guid id)
    {
        var spec = new ModeSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static ModeSpec GetByName(string name)
    {
        var spec = new ModeSpec();
        spec.Query.Where(e => e.Name == name);
        return spec;
    }
}

public sealed class ModeProjectionSpec : Specification<Mode, ModeRecord>
{
    private ModeProjectionSpec() {}
    public static ModeProjectionSpec Get(Guid id)
    {
        var spec = new ModeProjectionSpec();
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
    public static ModeProjectionSpec FilterByName(string? name)
    {
        var spec = new ModeProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
