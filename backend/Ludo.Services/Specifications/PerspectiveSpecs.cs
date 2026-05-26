using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class PerspectiveSpec : Specification<Perspective>
{
    private PerspectiveSpec() {}
    public static PerspectiveSpec Get(Guid id)
    {
        var spec = new PerspectiveSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static PerspectiveSpec GetByName(string name)
    {
        var spec = new PerspectiveSpec();
        spec.Query.Where(e => e.Name == name);
        return spec;
    }
}

public sealed class PerspectiveProjectionSpec : Specification<Perspective, PerspectiveRecord>
{
    private PerspectiveProjectionSpec() {}
    public static PerspectiveProjectionSpec Get(Guid id)
    {
        var spec = new PerspectiveProjectionSpec();
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
    public static PerspectiveProjectionSpec FilterByName(string? name)
    {
        var spec = new PerspectiveProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
