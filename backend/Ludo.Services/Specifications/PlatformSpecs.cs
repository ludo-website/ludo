using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class PlatformSpec : Specification<Platform>
{
    private PlatformSpec() {}
    public static PlatformSpec Get(Guid id)
    {
        var spec = new PlatformSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static PlatformSpec GetByName(string name)
    {
        var spec = new PlatformSpec();
        spec.Query.Where(e => e.Name == name);
        return spec;
    }
}

public sealed class PlatformProjectionSpec : Specification<Platform, PlatformRecord>
{
    private PlatformProjectionSpec() {}
    public static PlatformProjectionSpec Get(Guid id)
    {
        var spec = new PlatformProjectionSpec();
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
    public static PlatformProjectionSpec FilterByName(string? name)
    {
        var spec = new PlatformProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
