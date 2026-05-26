using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GamePerspectiveSpec : Specification<GamePerspective>
{
    private GamePerspectiveSpec() {}
    public static GamePerspectiveSpec Get(Guid id)
    {
        var spec = new GamePerspectiveSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GamePerspectiveSpec GetByGamePerspective(Guid gameId, Guid perspectiveId)
    {
        var spec = new GamePerspectiveSpec();
        spec.Query.Where(e => e.GameId == gameId);
        spec.Query.Where(e => e.PerspectiveId == perspectiveId);
        return spec;
    }
}

public sealed class GamePerspectiveProjectionSpec : Specification<GamePerspective, GamePerspectiveRecord>
{
    private GamePerspectiveProjectionSpec() {}
    public static GamePerspectiveProjectionSpec Get(Guid id)
    {
        var spec = new GamePerspectiveProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                PerspectiveId = e.PerspectiveId
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
                PerspectiveId = e.PerspectiveId
            });
    }
    public static GamePerspectiveProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new GamePerspectiveProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    public static GamePerspectiveProjectionSpec FilterByPerspective(Guid? perspectiveId)
    {
        var spec = new GamePerspectiveProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.PerspectiveId == perspectiveId);
        return spec;
    }
}
