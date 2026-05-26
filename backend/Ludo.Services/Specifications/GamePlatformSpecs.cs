using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GamePlatformSpec : Specification<GamePlatform>
{
    private GamePlatformSpec() {}
    public static GamePlatformSpec Get(Guid id)
    {
        var spec = new GamePlatformSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GamePlatformSpec GetByGamePlatform(Guid gameId, Guid platformId)
    {
        var spec = new GamePlatformSpec();
        spec.Query.Where(e => e.GameId == gameId);
        spec.Query.Where(e => e.PlatformId == platformId);
        return spec;
    }
}

public sealed class GamePlatformProjectionSpec : Specification<GamePlatform, GamePlatformRecord>
{
    private GamePlatformProjectionSpec() {}
    public static GamePlatformProjectionSpec Get(Guid id)
    {
        var spec = new GamePlatformProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                PlatformId = e.PlatformId
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
                PlatformId = e.PlatformId
            });
    }
    public static GamePlatformProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new GamePlatformProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    public static GamePlatformProjectionSpec FilterByPlatform(Guid? platformId)
    {
        var spec = new GamePlatformProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.PlatformId == platformId);
        return spec;
    }
}
