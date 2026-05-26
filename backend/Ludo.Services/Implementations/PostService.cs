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

public class PostService(IRepository<WebAppDatabaseContext> repository) : IPostService
{
    public async Task<ServiceResponse> Add(PostCreateRecord post, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(GameSpec.Get(post.GameId), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.AddAsync(new Post
        {
            Title = post.Title,
            Message = post.Message,
            GameId = post.GameId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PostRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(PostProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<PostRecord>(CommonErrors.NotFound);
    }

    public async Task<ServiceResponse<PagedResponse<PostRecord>>> FilterByGame(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, PostProjectionSpec.FilterByGame(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<PostRecord>>> FilterByTitle(PaginationStringSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, PostProjectionSpec.FilterByTitle(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> Update(PostUpdateRecord post, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(PostSpec.Get(post.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var game = await repository.GetAsync(GameSpec.Get(entity.GameId), cancellationToken);
        if (game == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        entity.Title = post.Title ?? entity.Title;
        entity.Message = post.Message ?? entity.Message;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(PostSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        var game = await repository.GetAsync(GameSpec.Get(entity.GameId), cancellationToken);
        if (game == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.DeleteAsync<Post>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
