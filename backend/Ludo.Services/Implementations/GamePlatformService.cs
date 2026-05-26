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

public class GamePlatformService(IRepository<WebAppDatabaseContext> repository) : IGamePlatformService
{
    public async Task<ServiceResponse> Add(GamePlatformCreateRecord gamePlatform, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(PlatformSpec.Get(gamePlatform.PlatformId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gamePlatform.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (await repository.GetAsync(GamePlatformSpec.GetByGamePlatform(gamePlatform.GameId, gamePlatform.PlatformId), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new GamePlatform
        {
            GameId = gamePlatform.GameId,
            PlatformId = gamePlatform.PlatformId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<GamePlatformRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GamePlatformProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GamePlatformRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<GamePlatformRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GamePlatformProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<PagedResponse<GamePlatformRecord>>> FilterByPlatform(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GamePlatformProjectionSpec.FilterByPlatform(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var gamePlatform = await repository.GetAsync(GamePlatformSpec.Get(id), cancellationToken);
        if (gamePlatform == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gamePlatform.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.DeleteAsync<GamePlatform>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
