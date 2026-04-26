using System.Net;
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

public class GameService(IRepository<WebAppDatabaseContext> repository)
    : IGameService
{
    public async Task<ServiceResponse> Add(GameCreateRecord game, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Gamedev && requestingAccount.Role != AccountRoleEnum.Admin)
        {
            return ServiceResponse.FromError(CommonErrors.NonGamedevAdd);
        }
        if (requestingAccount.Role == AccountRoleEnum.Gamedev && game.Account_Id != requestingAccount.Id) {
            return ServiceResponse.FromError(CommonErrors.NonOwnerAdd);
        }
        var result = await repository.GetAsync(new GameProjectionSpec(game.Title), cancellationToken);
        if (result != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Game
        {
            Title = game.Title,
            ShortDescription = game.Short_Description,
            FullDescription = game.Full_Description,
            AccountId = game.Account_Id
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<GameRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new GameProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) : 
            ServiceResponse.FromError<GameRecord>(CommonErrors.NotFound);
    }

    public async Task<ServiceResponse<PagedResponse<GameRecord>>> FilterByTitle(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new GameProjectionSpec(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<GameRecord>>> FilterByAccount(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new GameProjectionSpec(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> Update(GameUpdateRecord game, AccountRecord? requestingAccount, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamedev)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only a gamedev or the admin can update games!", ErrorCodes.CannotUpdate));
        }
        var entity = await repository.GetAsync(new GameSpec(game.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Game not found!", ErrorCodes.NotFound));
        }
        if (entity.AccountId != requestingAccount.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the gamedev owner can update the game!", ErrorCodes.CannotUpdate));
        }
        entity.Name = user.Name ?? entity.Name;
        entity.Password = user.Password ?? entity.Password;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, AccountRecord? requestingAccount = null, CancellationToken cancellationToken = default)
    {
        if (requestingAccount == null)
        {
            return ServiceResponse.FromError(CommonErrors.Unauthenticated);
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Role != AccountRoleEnum.Gamedev)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only a gamedev or the admin can delete games!", ErrorCodes.CannotDelete));
        }
        if (requestingAccount.Role != AccountRoleEnum.Admin && requestingAccount.Id != id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only a gamedev or the admin can delete games!", ErrorCodes.CannotDelete));
        }
        await repository.DeleteAsync<Account>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
