using Ludo.Database.Repository;
using Ludo.Database.Repository.Entities;
using Ludo.Infrastructure.Errors;
using Ludo.Infrastructure.Repositories.Interfaces;
using Ludo.Infrastructure.Requests;
using Ludo.Infrastructure.Responses;
using Ludo.Services.Abstractions;
using Ludo.Services.DataTransferObjects;
using Ludo.Services.Specifications;

namespace Ludo.Services.Implementations;

public class AestheticService(IRepository<WebAppDatabaseContext> repository) : IAestheticService
{
    public async Task<ServiceResponse> Add(AestheticCreateRecord aesthetic, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(AestheticSpec.GetByName(aesthetic.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Aesthetic
        {
            Name = aesthetic.Name
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<AestheticRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(AestheticProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<AestheticRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<AestheticRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, AestheticProjectionSpec.FilterByName(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Update(AestheticUpdateRecord aesthetic, CancellationToken cancellationToken = default)
    {
        if (aesthetic.Name != null && await repository.GetAsync(AestheticSpec.GetByName(aesthetic.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        var entity = await repository.GetAsync(AestheticSpec.Get(aesthetic.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Name = aesthetic.Name ?? entity.Name;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(AestheticSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Aesthetic>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
