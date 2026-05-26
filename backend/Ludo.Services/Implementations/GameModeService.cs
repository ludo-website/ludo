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

public class GameModeService(IRepository<WebAppDatabaseContext> repository) : IGameModeService
{
    public async Task<ServiceResponse> Add(GameModeCreateRecord gameMode, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(ModeSpec.Get(gameMode.ModeId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gameMode.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (await repository.GetAsync(GameModeSpec.GetByGameMode(gameMode.GameId, gameMode.ModeId), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new GameMode
        {
            GameId = gameMode.GameId,
            ModeId = gameMode.ModeId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<GameModeRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GameModeProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GameModeRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<GameModeRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameModeProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<PagedResponse<GameModeRecord>>> FilterByMode(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameModeProjectionSpec.FilterByMode(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var gameMode = await repository.GetAsync(GameModeSpec.Get(id), cancellationToken);
        if (gameMode == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gameMode.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.DeleteAsync<GameMode>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
