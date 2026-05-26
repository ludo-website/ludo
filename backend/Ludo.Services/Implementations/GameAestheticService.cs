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

public class GameAestheticService(IRepository<WebAppDatabaseContext> repository) : IGameAestheticService
{
    public async Task<ServiceResponse> Add(GameAestheticCreateRecord gameAesthetic, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(AestheticSpec.Get(gameAesthetic.AestheticId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gameAesthetic.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (await repository.GetAsync(GameAestheticSpec.GetByGameAesthetic(gameAesthetic.GameId, gameAesthetic.AestheticId), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new GameAesthetic
        {
            GameId = gameAesthetic.GameId,
            AestheticId = gameAesthetic.AestheticId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<GameAestheticRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GameAestheticProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GameAestheticRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameAestheticProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse<PagedResponse<GameAestheticRecord>>> FilterByAesthetic(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameAestheticProjectionSpec.FilterByAesthetic(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var gameAesthetic = await repository.GetAsync(GameAestheticSpec.Get(id), cancellationToken);
        if (gameAesthetic == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var entity = await repository.GetAsync(GameSpec.Get(gameAesthetic.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.DeleteAsync<GameAesthetic>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
