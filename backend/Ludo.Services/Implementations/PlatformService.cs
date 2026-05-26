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

public class PlatformService(IRepository<WebAppDatabaseContext> repository) : IPlatformService
{
    public async Task<ServiceResponse> Add(PlatformCreateRecord platform, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(PlatformSpec.GetByName(platform.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Platform
        {
            Name = platform.Name
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<PlatformRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(PlatformProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<PlatformRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<PlatformRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, PlatformProjectionSpec.FilterByName(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Update(PlatformUpdateRecord platform, CancellationToken cancellationToken = default)
    {
        if (platform.Name != null && await repository.GetAsync(PlatformSpec.GetByName(platform.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        var entity = await repository.GetAsync(PlatformSpec.Get(platform.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Name = platform.Name ?? entity.Name;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(PlatformSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Platform>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
