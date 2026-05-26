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

public class GameService(IRepository<WebAppDatabaseContext> repository) : IGameService
{
    public async Task<ServiceResponse> Add(GameCreateRecord game, CancellationToken cancellationToken = default)
    {
        var account = await repository.GetAsync(AccountSpec.Get(game.AccountId), cancellationToken);
        if (account == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (account.Role != AccountRoleEnum.Gamedev)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateRoleConflict);
        }
        if (await repository.GetAsync(GameSpec.GetByTitle(game.Title), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Game
        {
            Title = game.Title,
            ShortDescription = game.ShortDescription,
            FullDescription = game.FullDescription,
            AccountId = game.AccountId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<GameRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GameProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GameRecord>(CommonErrors.NotFound);
    }

    public async Task<ServiceResponse<PagedResponse<GameRecord>>> FilterByTitle(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameProjectionSpec.FilterByTitle(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<GameRecord>>> FilterByAccount(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GameProjectionSpec.FilterByAccount(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> Update(GameUpdateRecord game, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(GameSpec.Get(game.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        if (game.Title != null && game.Title != entity.Title && await repository.GetAsync(GameSpec.GetByTitle(game.Title), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        if (game.AccountId.HasValue)
        {
            var account = await repository.GetAsync(AccountSpec.Get(game.AccountId.Value), cancellationToken);
            if (account == null)
            {
                return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
            }
            if (account.Role != AccountRoleEnum.Gamedev)
            {
                return ServiceResponse.FromError(CommonErrors.AssociateRoleConflict);
            }
        }
        entity.Title = game.Title ?? entity.Title;
        entity.ShortDescription = game.ShortDescription;
        entity.FullDescription = game.FullDescription;
        entity.AccountId = game.AccountId ?? entity.AccountId;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(GameSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Game>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
