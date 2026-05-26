using Ludo.Database.Repository;
using Ludo.Database.Repository.Entities;
using Ludo.Database.Repository.Enums;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Repositories.Interfaces;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Specifications;

namespace Ludo.Services.Implementations;

public class GameGenreService(IRepository<WebAppDatabaseContext> repository) : IGameGenreService
{
    public async Task<ServiceResponse> Add(GameGenreCreateRecord gameGenre, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(GenreSpec.Get(gameGenre.GenreId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gameGenre.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (await repository.GetAsync(GameGenreSpec.GetByGameGenre(gameGenre.GameId, gameGenre.GenreId), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new GameGenre
        {
            GameId = gameGenre.GameId,
            GenreId = gameGenre.GenreId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<GameGenreRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GameGenreProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GameGenreRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<GameGenreRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameGenreProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<PagedResponse<GameGenreRecord>>> FilterByGenre(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameGenreProjectionSpec.FilterByGenre(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var gameGenre = await repository.GetAsync(GameGenreSpec.Get(id), cancellationToken);
        if (gameGenre == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gameGenre.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.DeleteAsync<GameGenre>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
