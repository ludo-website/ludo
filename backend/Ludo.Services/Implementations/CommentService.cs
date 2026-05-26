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

public class CommentService(IRepository<WebAppDatabaseContext> repository) : ICommentService
{
    public async Task<ServiceResponse> Add(CommentCreateRecord comment, CancellationToken cancellationToken = default)
    {
        if (await repository.GetAsync(PostSpec.Get(comment.PostId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        if (await repository.GetAsync(AccountSpec.Get(comment.AccountId), cancellationToken) == null)
        {
            return ServiceResponse.FromError(CommonErrors.AssociateNotFound);
        }
        await repository.AddAsync(new Comment
        {
            Message = comment.Message,
            AccountId = comment.AccountId,
            PostId = comment.PostId
        }, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<CommentRecord>> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(CommentProjectionSpec.Get(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<CommentRecord>(CommonErrors.NotFound);
    }

    public async Task<ServiceResponse<PagedResponse<CommentRecord>>> FilterByAccount(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, CommentProjectionSpec.FilterByAccount(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<CommentRecord>>> FilterByPost(PaginationIdSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, CommentProjectionSpec.FilterByPost(pagination.Search), cancellationToken);
        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> Update(CommentUpdateRecord comment, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(CommentSpec.Get(comment.Id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        entity.Message = comment.Message ?? entity.Message;
        await repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(CommentSpec.Get(id), cancellationToken);
        if (entity == null)
        {
            return ServiceResponse.FromError(CommonErrors.NotFound);
        }
        await repository.DeleteAsync<Comment>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}
