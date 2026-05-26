using Ardalis.Specification;
using Ludo.Database.Repository.Entities;
using Ludo.Services.DataTransferObjects;

namespace Ludo.Services.Specifications;

public sealed class GameGenreSpec : Specification<GameGenre>
{
    private GameGenreSpec() {}
    public static GameGenreSpec Get(Guid id)
    {
        var spec = new GameGenreSpec();
        spec.Query.Where(e => e.Id == id);
        return spec;
    }
    public static GameGenreSpec GetByGameGenre(Guid gameId, Guid genreId)
    {
        var spec = new GameGenreSpec();
        spec.Query.Where(e => e.GameId == gameId);
        spec.Query.Where(e => e.GenreId == genreId);
        return spec;
    }
}

public sealed class GameGenreProjectionSpec : Specification<GameGenre, GameGenreRecord>
{
    private GameGenreProjectionSpec() {}
    public static GameGenreProjectionSpec Get(Guid id)
    {
        var spec = new GameGenreProjectionSpec();
        spec.Query.Where(e => e.Id == id)
            .Select(e => new()
            {
                Id = e.Id,
                GameId = e.GameId,
                GenreId = e.GenreId
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
                GenreId = e.GenreId
            });
    }
    public static GameGenreProjectionSpec FilterByGame(Guid? gameId)
    {
        var spec = new GameGenreProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GameId == gameId);
        return spec;
    }
    public static GameGenreProjectionSpec FilterByGenre(Guid? genreId)
    {
        var spec = new GameGenreProjectionSpec();
        spec.Sort();
        spec.Query.Where(e => e.GenreId == genreId);
        return spec;
    }
}
