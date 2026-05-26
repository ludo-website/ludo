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

public class GamePerspectiveService(IRepository<WebAppDatabaseContext> repository) : IGamePerspectiveService
{
    public async Task<ServiceResponse> Add(GamePerspectiveCreateRecord gamePerspective, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(PerspectiveSpec.Get(gamePerspective.PerspectiveId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gamePerspective.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (await repository.GetAsync(GamePerspectiveSpec.GetByGamePerspective(gamePerspective.GameId, gamePerspective.PerspectiveId), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new GamePerspective
        {
            GameId = gamePerspective.GameId,
            PerspectiveId = gamePerspective.PerspectiveId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<GamePerspectiveRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GamePerspectiveProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GamePerspectiveRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<GamePerspectiveRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GamePerspectiveProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<PagedResponse<GamePerspectiveRecord>>> FilterByPerspective(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GamePerspectiveProjectionSpec.FilterByPerspective(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var gamePerspective = await repository.GetAsync(GamePerspectiveSpec.Get(id), cancellationToken);
        if (gamePerspective == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gamePerspective.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.DeleteAsync<GamePerspective>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
