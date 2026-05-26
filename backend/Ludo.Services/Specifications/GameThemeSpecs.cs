using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GameThemeSpec : Specification<GameTheme>
{
    private GameThemeSpec() {}
    public static GameThemeSpec Get(Guid id)
    {
        var spec = new GameThemeSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GameThemeSpec GetByGameTheme(Guid gameId, Guid themeId)
    {
        var spec = new GameThemeSpec();
        spec.Query.Where(e => e.GameId == gameId);
        spec.Query.Where(e => e.ThemeId == themeId);
        return spec;
    }
}

public sealed class GameThemeProjectionSpec : Specification<GameTheme, GameThemeRecord>
{
    private GameThemeProjectionSpec() {}
    public static GameThemeProjectionSpec Get(Guid id)
    {
        var spec = new GameThemeProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                ThemeId = e.ThemeId
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
                ThemeId = e.ThemeId
            });
    }
    public static GameThemeProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new GameThemeProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    public static GameThemeProjectionSpec FilterByTheme(Guid? themeId)
    {
        var spec = new GameThemeProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.ThemeId == themeId);
        return spec;
    }
}
