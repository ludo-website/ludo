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

public class GenreService(IRepository<WebAppDatabaseContext> repository) : IGenreService
{
    public async Task<ServiceResponse> Add(GenreCreateRecord genre, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(GenreSpec.GetByName(genre.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        await repository.AddAsync(new Genre
        {
            Name = genre.Name
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse<GenreRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(GenreProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<GenreRecord>(CommonErrors.NotFound);
    }
    public async Task<ServiceResponse<PagedResponse<GenreRecord>>> FilterByName(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, GenreProjectionSpec.FilterByName(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }
    public async Task<ServiceResponse> Update(GenreUpdateRecord genre, CancellationToken cancellationToken = default)
    {
        if (genre.Name != null && await repository.GetAsync(GenreSpec.GetByName(genre.Name), cancellationToken) != null)
        {
            return ServiceResponse.FromError(CommonErrors.Duplicate);
        }
        var entity = await repository.GetAsync(GenreSpec.Get(genre.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Name = genre.Name ?? entity.Name;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(GenreSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Genre>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
