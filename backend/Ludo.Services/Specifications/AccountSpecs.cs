using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class AccountSpec : Specification<Account>
{
    private AccountSpec() {}
    public static AccountSpec Get(Guid id)
    {
        var spec = new AccountSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static AccountSpec GetByName(string? name)
    {
        var spec = new AccountSpec();
        if (name != null)
        {
            spec.Query.Where(e => e.Name == name);
        }
        return spec;
    }
}

public sealed class AccountProjectionSpec : Specification<Account, AccountRecord>
{
    private AccountProjectionSpec() {}
    public static AccountProjectionSpec Get(Guid id)
    {
        var spec = new AccountProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                Email = e.Email,
                Name = e.Name,
                Role = e.Role
            });
        return spec;
    }
    private void Sort()
    {
        Query.OrderByDescending(x => x.CreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                Email = e.Email,
                Name = e.Name,
                Role = e.Role
            });
    }
    private static string? GetSearchExpr(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;
        return search != null ? $"%{search.Replace(" ", "%")}%" : null;
    }
    public static AccountProjectionSpec FilterByName(string? name)
    {
        var spec = new AccountProjectionSpec();
        spec.Sort();
        var searchExpr = GetSearchExpr(name);
        if (searchExpr != null)
        {
            spec.Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
        }
        return spec;
    }
}
