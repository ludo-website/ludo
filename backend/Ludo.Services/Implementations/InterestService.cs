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

public class InterestService(IRepository<WebAppDatabaseContext> repository) : IInterestService
{
    public async Task<ServiceResponse> Add(InterestCreateRecord interest, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(GameSpec.Get(interest.GameId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        var entity = await repository.GetAsync(AccountSpec.Get(interest.AccountId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (entity.Role != AccountRoleEnum.Gamer)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateRoleConflict);
        }
        if (await repository.GetAsync(InterestSpec.GetByAccountGame(interest.AccountId, interest.GameId), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Interest
        {
            AccountId = interest.AccountId,
            GameId = interest.GameId,
            Ammount = Math.Max(interest.Ammount ?? 0, 0)
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<InterestRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(InterestProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<InterestRecord>(CommonErrors.NotFound);
    }

    public async Task<ServiceResponse<PagedResponse<InterestRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, InterestProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<InterestRecord>>> FilterByAccount(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, InterestProjectionSpec.FilterByAccount(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> Update(InterestUpdateRecord interest, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(InterestSpec.Get(interest.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Ammount = interest.Ammount ?? entity.Ammount;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(InterestSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Interest>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
