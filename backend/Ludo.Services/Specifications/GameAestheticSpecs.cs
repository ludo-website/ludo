using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GameAestheticSpec : Specification<GameAesthetic>
{
    private GameAestheticSpec() {}
    public static GameAestheticSpec Get(Guid id)
    {
        var spec = new GameAestheticSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GameAestheticSpec GetByGameAesthetic(Guid gameId, Guid aestheticId)
    {
        var spec = new GameAestheticSpec();
        spec.Query.Where(e => e.GameId == gameId);
        spec.Query.Where(e => e.AestheticId == aestheticId);
        return spec;
    }
}

public sealed class GameAestheticProjectionSpec : Specification<GameAesthetic, GameAestheticRecord>
{
    private GameAestheticProjectionSpec() {}
    public static GameAestheticProjectionSpec Get(Guid id)
    {
        var spec = new GameAestheticProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                AestheticId = e.AestheticId
            });
        return spec;
    }
    private void Sort()
    {
        Query.OrderByDescending(x => x.CreatedAt)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                AestheticId = e.AestheticId
            });
    }
    public static GameAestheticProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new GameAestheticProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    public static GameAestheticProjectionSpec FilterByAesthetic(Guid? aestheticId)
    {
        var spec = new GameAestheticProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.AestheticId == aestheticId);
        return spec;
    }
}
