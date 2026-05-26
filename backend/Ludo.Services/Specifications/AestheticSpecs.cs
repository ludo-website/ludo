using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class AestheticSpec : Specification<Aesthetic>
{
    private AestheticSpec() {}
    public static AestheticSpec Get(Guid id)
    {
        var spec = new AestheticSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static AestheticSpec GetByName(string name)
    {
        var spec = new AestheticSpec();
        spec.Query.Where(e => e.Name == name);
        return spec;
    }
}

public sealed class AestheticProjectionSpec : Specification<Aesthetic, AestheticRecord>
{
    private AestheticProjectionSpec() {}
    public static AestheticProjectionSpec Get(Guid id)
    {
        var spec = new AestheticProjectionSpec();
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
    public static AestheticProjectionSpec FilterByName(string? name)
    {
        var spec = new AestheticProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
