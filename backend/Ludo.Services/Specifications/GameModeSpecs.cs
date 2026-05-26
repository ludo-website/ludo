using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GameModeSpec : Specification<GameMode>
{
    private GameModeSpec() {}
    public static GameModeSpec Get(Guid id)
    {
        var spec = new GameModeSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GameModeSpec GetByGameMode(Guid gameId, Guid modeId)
    {
        var spec = new GameModeSpec();
        spec.Query.Where(e => e.GameId == gameId);
        spec.Query.Where(e => e.ModeId == modeId);
        return spec;
    }
}

public sealed class GameModeProjectionSpec : Specification<GameMode, GameModeRecord>
{
    private GameModeProjectionSpec() {}
    public static GameModeProjectionSpec Get(Guid id)
    {
        var spec = new GameModeProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                ModeId = e.ModeId
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
                ModeId = e.ModeId
            });
    }
    public static GameModeProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new GameModeProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    public static GameModeProjectionSpec FilterByMode(Guid? modeId)
    {
        var spec = new GameModeProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.ModeId == modeId);
        return spec;
    }
}
