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

public class ThemeService(IRepository<WebAppDatabaseContext> repository) : IThemeService
{
    public async Task<ServiceResponse> Add(ThemeCreateRecord theme, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(ThemeSpec.GetByName(theme.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Theme
        {
            Name = theme.Name
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<ThemeRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(ThemeProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ThemeRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<ThemeRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, ThemeProjectionSpec.FilterByName(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Update(ThemeUpdateRecord theme, CancellationToken cancellationToken = default)
    {
        if (theme.Name != null && await repository.GetAsync(ThemeSpec.GetByName(theme.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        var entity = await repository.GetAsync(ThemeSpec.Get(theme.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Name = theme.Name ?? entity.Name;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(ThemeSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Theme>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
