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

public class PerspectiveService(IRepository<WebAppDatabaseContext> repository) : IPerspectiveService
{
    public async Task<ServiceResponse> Add(PerspectiveCreateRecord perspective, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(PerspectiveSpec.GetByName(perspective.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Perspective
        {
            Name = perspective.Name
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<PerspectiveRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(PerspectiveProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<PerspectiveRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<PerspectiveRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, PerspectiveProjectionSpec.FilterByName(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Update(PerspectiveUpdateRecord perspective, CancellationToken cancellationToken = default)
    {
        if (perspective.Name != null && await repository.GetAsync(PerspectiveSpec.GetByName(perspective.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        var entity = await repository.GetAsync(PerspectiveSpec.Get(perspective.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Name = perspective.Name ?? entity.Name;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(PerspectiveSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Perspective>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
