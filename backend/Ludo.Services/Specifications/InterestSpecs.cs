using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class InterestSpec : Specification<Interest>
{
    private InterestSpec() {}
    public static InterestSpec Get(Guid id)
    {
        var spec = new InterestSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static InterestSpec GetByAccountGame(Guid accountId, Guid gameId)
    {
        var spec = new InterestSpec();
        spec.Query.Where(e => e.AccountId == accountId);
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
}

public sealed class InterestProjectionSpec : Specification<Interest, InterestRecord>
{
    private InterestProjectionSpec() {}
    public static InterestProjectionSpec Get(Guid id)
    {
        var spec = new InterestProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                AccountId = e.AccountId,
                GameId = e.GameId,
                Ammount = e.Ammount
            });
        return spec;
    }
    private void Sort()
    {
        Query.OrderByDescending(x => x.CreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                AccountId = e.AccountId,
                GameId = e.GameId,
                Ammount = e.Ammount
            });
    }
    public static InterestProjectionSpec FilterByAccount(Guid? accountId)
    {
        var spec = new InterestProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.AccountId == accountId);
        return spec;
    }
    public static InterestProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new InterestProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
}
